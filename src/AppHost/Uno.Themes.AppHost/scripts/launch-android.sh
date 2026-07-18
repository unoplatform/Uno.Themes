#!/usr/bin/env bash
# Uno.Themes sample-on-Android launcher invoked by the Aspire AppHost resource
# `simple-android`. Single foreground process so Aspire treats start/stop/log
# streaming the same way it does for the WASM and Desktop client resources.
#
# Pipeline:
#   1. Sanity check — adb on PATH, ANDROID_HOME readable, project path resolves.
#   2. Wait briefly for a usable Android device. Either:
#        - The `android-emulator` Aspire resource booted one (the default,
#          since `simple-android` declares `WaitFor(android-emulator)` in
#          AppHost/Program.cs — that emulator script owns the lifecycle), OR
#        - The dev has an externally-managed device attached (USB phone,
#          Android Studio AVD, `adb connect`), in which case the emulator
#          resource is skipped via ANDROID_AUTO_START_EMULATOR=0 in the
#          AppHost's environment.
#      Either way, this script does not provision the emulator itself —
#      lifecycle lives in `scripts/start-emulator.sh`.
#   3. dotnet build -t:InstallAndroidDependencies — Microsoft.Android.Sdk's own
#      target inspects the project, installs whatever build-time deps it needs
#      (platforms;android-<compile-level>, build-tools, NDK if applicable) into
#      ANDROID_HOME. Self-tuning to the workload version — when the SDK bumps
#      from 36.x to 37.x, no script change needed.
#   4. dotnet build -t:Install -f net10.0-android — builds + adb-installs the APK.
#   5. adb shell am force-stop + am start against the dynamically-resolved
#      LAUNCHER activity (avoids hardcoding the CRC64-suffixed MainActivity name).
#   6. adb logcat --pid <pid> — streams the app's log output to Aspire stdout.
#      logcat is the long-running foreground process; stopping this resource
#      only kills the launcher + logcat. The emulator outlives this script
#      (it's owned by `android-emulator`) so subsequent Start cycles skip the
#      cold-boot wait entirely.
#
# Env vars (set via AppHost `WithEnvironment` or in the resource's env override):
#   ANDROID_PROJECT        Path to the sample csproj (relative to AppHost project
#                          dir or absolute). Default:
#                          ../../samples/SimpleSampleApp/SimpleSampleApp.csproj
#   ANDROID_PACKAGE        Application id of the installed APK. Default:
#                          uno.platform.themes.simple. Override together with
#                          ANDROID_PROJECT to drive the Material/Cupertino heads
#                          (uno.platform.themes.material / .cupertino).
#   ANDROID_SERIAL         Optional adb device serial. Forwarded to adb as `-s`.
#                          Omit to let adb auto-pick when exactly one device is
#                          attached; with multiple devices adb errors out and the
#                          user must set this explicitly.
#   ANDROID_CONFIG         Debug / Release. Default: Debug.
#   DEVICE_WAIT_TIMEOUT_S  Seconds to wait for a usable device to appear when
#                          this script starts. Default: 300 (5 min) — long
#                          enough to absorb a fresh emulator cold-boot from
#                          android-emulator on an unwarmed AVD.

set -euo pipefail

# ── Args / env defaults ──────────────────────────────────────────────────────
PROJECT="${ANDROID_PROJECT:-../../samples/SimpleSampleApp/SimpleSampleApp.csproj}"
PACKAGE="${ANDROID_PACKAGE:-uno.platform.themes.simple}"
SERIAL="${ANDROID_SERIAL:-}"
CONFIG="${ANDROID_CONFIG:-Debug}"
DEVICE_WAIT_TIMEOUT_S="${DEVICE_WAIT_TIMEOUT_S:-300}"

# Build/install + emulator+sdk operations all rely on ANDROID_HOME pointing at a
# writable SDK tree. Defaults to /opt/android-sdk but is overridable for hosts
# that keep the SDK elsewhere.
ANDROID_HOME="${ANDROID_HOME:-/opt/android-sdk}"
export ANDROID_HOME ANDROID_SDK_ROOT="$ANDROID_HOME"

# Xamarin.Installer.AndroidSDK (the engine behind `dotnet build
# -t:InstallAndroidDependencies`) reads $USER directly from the environment to
# tag installed archives with ownership metadata, and throws XAIAD7009 if either
# USER or LOGNAME is empty. Non-interactive shells leave USER unset on some
# startup paths, so derive both from `id -un` before any dotnet invocation.
USER="${USER:-$(id -un 2>/dev/null)}"
LOGNAME="${LOGNAME:-$USER}"
export USER LOGNAME

# ── adb target flag (empty when no serial set) ───────────────────────────────
if [ -n "$SERIAL" ]; then
  ADB_TARGET=(-s "$SERIAL")
else
  ADB_TARGET=()
fi

# ── 1. Sanity ────────────────────────────────────────────────────────────────
if ! command -v adb >/dev/null 2>&1; then
  echo "❌ 'adb' not found on PATH."
  echo "   Install platform-tools (e.g. 'apt install android-tools-adb' on"
  echo "   Debian/Ubuntu, 'brew install android-platform-tools' on macOS)."
  exit 1
fi

if ! command -v dotnet >/dev/null 2>&1; then
  echo "❌ 'dotnet' not found on PATH."
  exit 1
fi

if [ ! -d "$ANDROID_HOME" ]; then
  echo "❌ ANDROID_HOME ($ANDROID_HOME) does not exist."
  echo "   Set ANDROID_HOME to a writable Android SDK directory."
  exit 1
fi

if [ ! -f "$PROJECT" ]; then
  echo "❌ Project not found at: $PROJECT"
  echo "   Pass an absolute path via ANDROID_PROJECT or run from the AppHost project directory."
  exit 1
fi
PROJECT_ABS=$(readlink -f "$PROJECT")

# ── 2. cmdline-tools (shared with start-emulator.sh via sourced lib) ────────
# Needed even when this script runs against an externally-managed device:
# `dotnet build -t:InstallAndroidDependencies` (step 3) invokes sdkmanager
# under the hood, so the cmdline-tools binary must exist at $ANDROID_HOME.
# Idempotent — a no-op when start-emulator.sh has already done the bootstrap.
SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
# shellcheck source=lib/android-sdk-bootstrap.sh
. "$SCRIPT_DIR/lib/android-sdk-bootstrap.sh"
bootstrap_cmdline_tools

# ── 3. Build-time SDK deps (project-driven) ──────────────────────────────────
# `dotnet build -t:InstallAndroidDependencies` asks Microsoft.Android.Sdk itself
# what platforms / build-tools / NDK the project requires and installs them
# (uses sdkmanager under the hood). Always running it makes the script
# self-tuning: when the .NET Android workload bumps, no script change is needed.
# Idempotent + ~5 s when everything is already installed.
echo "📦 [simple-android] Resolving build-time Android SDK dependencies"
echo "   (dotnet build -t:InstallAndroidDependencies — installs whatever"
echo "    Microsoft.Android.Sdk decides the project needs)"
dotnet build "$PROJECT_ABS" \
  -f net10.0-android \
  -t:InstallAndroidDependencies \
  "-p:AndroidSdkDirectory=$ANDROID_HOME" \
  -p:AcceptAndroidSDKLicenses=true

# ── 4. Wait for a usable device ──────────────────────────────────────────────
# AppHost wires `simple-android` to `WaitFor(android-emulator)`, so by the time
# we run, the emulator script has already published "Running". But "Running"
# only means the emulator script process is up — it does not guarantee
# `sys.boot_completed`, and externally-attached devices bypass the wait
# entirely. Poll until we actually see a device, then wait for boot.
#
# When multiple devices are visible, the dev must disambiguate via
# ANDROID_SERIAL — adb's auto-pick is non-deterministic.
echo "⏳ [simple-android] Waiting for a usable device (timeout ${DEVICE_WAIT_TIMEOUT_S}s)…"
WAIT_DEADLINE=$((SECONDS + DEVICE_WAIT_TIMEOUT_S))
while :; do
  DEVICE_COUNT=$(adb devices | awk 'NR>1 && $2=="device" {n++} END{print n+0}')
  if [ "$DEVICE_COUNT" -ge 1 ]; then
    break
  fi
  if [ $SECONDS -ge $WAIT_DEADLINE ]; then
    echo "❌ No Android device became visible to adb within ${DEVICE_WAIT_TIMEOUT_S}s."
    echo "   Either start the android-emulator resource (if it's stopped),"
    echo "   attach a USB device, or set ANDROID_AUTO_START_EMULATOR=0 on this"
    echo "   resource (and arrange your own device) so the WaitFor gate is satisfied."
    exit 1
  fi
  sleep 2
done

if [ "$DEVICE_COUNT" -gt 1 ] && [ -z "$SERIAL" ]; then
  echo "❌ Multiple Android devices/emulators detected:"
  adb devices | tail -n +2 | grep -E "device$" || true
  echo "   Set ANDROID_SERIAL=<serial> on the simple-android resource to disambiguate."
  exit 1
fi

echo "⏳ [simple-android] Waiting for sys.boot_completed on ${SERIAL:-<auto>}…"
WAIT_DEADLINE=$((SECONDS + DEVICE_WAIT_TIMEOUT_S))
until [ "$(adb "${ADB_TARGET[@]}" shell getprop sys.boot_completed 2>/dev/null | tr -d '\r\n')" = "1" ]; do
  if [ $SECONDS -ge $WAIT_DEADLINE ]; then
    echo "❌ sys.boot_completed didn't flip to 1 within ${DEVICE_WAIT_TIMEOUT_S}s."
    echo "   The android-emulator resource may have failed mid-boot — check"
    echo "   its log in the Aspire dashboard."
    exit 1
  fi
  sleep 3
done
echo "✅ [simple-android] Device ready."

echo "📦 [simple-android] Building + installing"
echo "   project = $PROJECT_ABS"
echo "   package = $PACKAGE"
echo "   config  = $CONFIG"
echo "   device  = ${SERIAL:-<auto>}"

# ── 5. Build + Install (the Install target chains the apk to adb install -r) ─
dotnet build "$PROJECT_ABS" \
  -f net10.0-android \
  -t:Install \
  -c "$CONFIG"

# ── 6. Launch via am start (no hardcoded activity name) ──────────────────────
# `am start -a MAIN -c LAUNCHER -p $PKG` alone doesn't work: `-p` is a package
# *filter*, not an activity *resolver*, so `am start` fails with `unable to
# resolve Intent`. Resolve the launcher activity dynamically so we never
# hardcode the CRC64-suffixed MainActivity name the dotnet/android binding emits
# (stable per build but moves with namespace edits — a poor thing to commit).
#
# Force-stop precedes start so the just-installed APK is what runs (a stale
# in-memory instance from a previous Start cycle would otherwise be brought
# forward).
LAUNCHER_COMPONENT=$(adb "${ADB_TARGET[@]}" shell cmd package resolve-activity --brief \
  -a android.intent.action.MAIN \
  -c android.intent.category.LAUNCHER \
  "$PACKAGE" 2>/dev/null | tail -n 1 | tr -d '\r')
if [ -z "$LAUNCHER_COMPONENT" ] || [ "$LAUNCHER_COMPONENT" = "No activity found" ]; then
  echo "❌ [simple-android] Could not resolve the LAUNCHER activity for $PACKAGE."
  echo "   Was the APK actually installed? Try \`adb shell pm list packages | grep ${PACKAGE}\`."
  exit 1
fi

echo "🚀 [simple-android] Launching $LAUNCHER_COMPONENT (cold start)"
adb "${ADB_TARGET[@]}" shell am force-stop "$PACKAGE"
adb "${ADB_TARGET[@]}" shell am start \
  -W \
  -n "$LAUNCHER_COMPONENT" \
  -a android.intent.action.MAIN \
  -c android.intent.category.LAUNCHER >/dev/null

# Give the app a moment to come up so pidof has something to find.
sleep 1

PID=$(adb "${ADB_TARGET[@]}" shell pidof "$PACKAGE" 2>/dev/null | tr -d '\r' || true)
if [ -z "$PID" ]; then
  echo "⚠️  [simple-android] Could not resolve PID for $PACKAGE — the launch may"
  echo "   have failed. Falling back to filtered whole-device logcat."
  exec adb "${ADB_TARGET[@]}" logcat -s "DOTNET:V" "*:E"
fi

echo "📋 [simple-android] Streaming logcat (pid=$PID, package=$PACKAGE)"
exec adb "${ADB_TARGET[@]}" logcat --pid="$PID"
