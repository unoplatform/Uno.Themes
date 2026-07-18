#!/usr/bin/env bash
# Uno.Themes Android emulator lifecycle script — invoked by the Aspire AppHost
# resource `android-emulator`. Owns the emulator process for the duration of
# the AppHost session so the emulator dies with Aspire (Ctrl+C / dashboard Stop)
# rather than outliving it.
#
# Pipeline:
#   1. Bootstrap cmdline-tools into $ANDROID_HOME if missing (idempotent).
#   2. Install the `emulator` package + system-image if missing (one-time,
#      ~1.5 GB into $ANDROID_HOME).
#   3. Create the uno-themes AVD if missing.
#   4. Boot the emulator (windowed by default; headless when
#      ANDROID_EMULATOR_WINDOW=0). Wait for `sys.boot_completed`.
#   5. Foreground-wait on the emulator process. On SIGTERM / SIGINT, send
#      `adb emu kill` so the emulator shuts down cleanly with this script.
#
# When this script exits — whether via SIGTERM from Aspire, SIGINT from a
# stand-alone run, or the emulator process itself dying — the emulator is
# always reaped. `simple-android` depends on this resource via WaitFor in
# AppHost/Program.cs so it doesn't try to install/launch until the emulator
# is registered with adb.
#
# Env vars (set via AppHost `WithEnvironment` or in the resource's env override):
#   ANDROID_AVD_NAME       AVD created/booted. Default: uno-themes
#   ANDROID_SDK_API_LEVEL  Android API level used to build SYSTEM_IMAGE.
#                          Default: 34
#   ANDROID_SDK_IMAGE      Full `system-images;...` triple installed.
#                          Default: system-images;android-34;google_apis;x86_64
#   ANDROID_EMULATOR_WINDOW  Set to 0 for headless. Default: 1 (windowed via
#                          X11 — requires DISPLAY to point at a running X server,
#                          e.g. WSLg / native X / XQuartz).
#   ANDROID_EMULATOR_GPU   `-gpu <mode>` argument forwarded to the emulator.
#                          Default: swiftshader_indirect (software rendering).
#                          Set to `host` for hardware GPU acceleration.
#
# This script does NOT handle build/install/launch of the sample app —
# that lives in `launch-android.sh` (the `simple-android` resource).

set -euo pipefail

# ── Args / env defaults ──────────────────────────────────────────────────────
AVD_NAME="${ANDROID_AVD_NAME:-uno-themes}"
API_LEVEL="${ANDROID_SDK_API_LEVEL:-34}"
SYSTEM_IMAGE="${ANDROID_SDK_IMAGE:-system-images;android-${API_LEVEL};google_apis;x86_64}"
EMULATOR_WINDOW="${ANDROID_EMULATOR_WINDOW:-1}"
EMULATOR_GPU="${ANDROID_EMULATOR_GPU:-swiftshader_indirect}"

ANDROID_HOME="${ANDROID_HOME:-/opt/android-sdk}"
export ANDROID_HOME ANDROID_SDK_ROOT="$ANDROID_HOME"

# Xamarin.Installer.AndroidSDK reads $USER / $LOGNAME for archive ownership
# metadata and throws XAIAD7009 when either is empty. Non-interactive shells
# can leave USER unset on some startup paths.
USER="${USER:-$(id -un 2>/dev/null)}"
LOGNAME="${LOGNAME:-$USER}"
export USER LOGNAME

# ── Sanity ───────────────────────────────────────────────────────────────────
if ! command -v adb >/dev/null 2>&1; then
  echo "❌ 'adb' not found on PATH. Install Android platform-tools"
  echo "   (e.g. 'apt install android-tools-adb' on Debian/Ubuntu,"
  echo "   'brew install android-platform-tools' on macOS)."
  exit 1
fi

if [ ! -d "$ANDROID_HOME" ]; then
  echo "❌ ANDROID_HOME ($ANDROID_HOME) does not exist. Set ANDROID_HOME to a"
  echo "   writable Android SDK directory (the emulator + system image install there)."
  exit 1
fi

# ── Load shared SDK helpers (bootstrap_cmdline_tools, accept_licenses_once) ──
SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
# shellcheck source=lib/android-sdk-bootstrap.sh
. "$SCRIPT_DIR/lib/android-sdk-bootstrap.sh"

# ── 1. cmdline-tools ─────────────────────────────────────────────────────────
bootstrap_cmdline_tools

# ── 2. Emulator + system-image ───────────────────────────────────────────────
if [ ! -x "$EMULATOR_BIN" ] || [ ! -d "$ANDROID_HOME/system-images" ]; then
  echo "📦 [android-emulator] Installing emulator + $SYSTEM_IMAGE"
  echo "   This is a one-time ~1.5 GB download; subsequent runs read from"
  echo "   $ANDROID_HOME."
  accept_licenses_once
  "$SDKMANAGER" "emulator" "$SYSTEM_IMAGE"
fi

# ── 3. AVD create if missing ─────────────────────────────────────────────────
if ! "$AVDMANAGER" list avd 2>/dev/null | grep -q "Name: $AVD_NAME"; then
  echo "📱 [android-emulator] Creating AVD '$AVD_NAME' from $SYSTEM_IMAGE (pixel_5)"
  # `echo no` answers "Do you wish to create a custom hardware profile?".
  echo "no" | "$AVDMANAGER" create avd \
    -n "$AVD_NAME" \
    -k "$SYSTEM_IMAGE" \
    -d "pixel_5"
fi

# ── 4. Boot the emulator ─────────────────────────────────────────────────────
# If an emulator instance for this AVD is already attached to adb, skip the
# boot — most likely a previous incarnation of this script left it running
# (we DO kill on SIGTERM, but if Aspire was force-killed via SIGKILL the trap
# wouldn't fire and the emulator would persist). Take ownership in that case
# so the next SIGTERM cleans up.
EXISTING_EMU=$(adb devices | awk 'NR>1 && $1 ~ /^emulator-/ && $2=="device" {print $1; exit}')
if [ -n "$EXISTING_EMU" ]; then
  echo "♻️  [android-emulator] Reusing already-running emulator: $EXISTING_EMU"
  TARGET_SERIAL="$EXISTING_EMU"
  STARTED_EMULATOR=0
else
  # Default args:
  #   -no-audio: ALSA isn't always wired in headless/container hosts.
  #   -no-snapshot-save: don't write a snapshot on shutdown (disk churn).
  #   -gpu <mode>: software default; override via ANDROID_EMULATOR_GPU=host.
  EMULATOR_ARGS=(-avd "$AVD_NAME" -no-audio -no-snapshot-save -gpu "$EMULATOR_GPU")
  if [ "$EMULATOR_WINDOW" = "1" ]; then
    if [ -z "${DISPLAY:-}" ]; then
      echo "⚠️  ANDROID_EMULATOR_WINDOW=1 but DISPLAY is not set — the emulator"
      echo "   window will fail to open. Set ANDROID_EMULATOR_WINDOW=0 for headless,"
      echo "   or ensure DISPLAY points at a running X server."
    fi
    echo "🔌 [android-emulator] Booting '$AVD_NAME' (windowed, DISPLAY=${DISPLAY:-unset})"
  else
    EMULATOR_ARGS+=(-no-window)
    echo "🔌 [android-emulator] Booting '$AVD_NAME' (headless)"
  fi

  # Run the emulator as a direct child so SIGTERM to this script can propagate
  # cleanly via the trap below. Redirect stdout/stderr to a logfile so the
  # noisy emulator output doesn't pollute the Aspire dashboard log viewer.
  mkdir -p "$ANDROID_HOME/logs"
  "$EMULATOR_BIN" "${EMULATOR_ARGS[@]}" \
    > "$ANDROID_HOME/logs/emulator-$AVD_NAME.log" 2>&1 &
  EMULATOR_PID=$!
  echo "   emulator PID=$EMULATOR_PID, log=$ANDROID_HOME/logs/emulator-$AVD_NAME.log"
  STARTED_EMULATOR=1
  TARGET_SERIAL=""
fi

# ── 4a. Trap so the emulator dies with this script ───────────────────────────
# Aspire sends SIGTERM on dashboard Stop / Ctrl+C of the AppHost. The trap
# uses `adb emu kill` first (graceful Android shutdown), then falls back to
# SIGKILL on the host emulator process if it's still around after a grace
# window. Only fires when we ourselves started the emulator — a reused one
# is left alone in case another resource also depends on it.
cleanup_emulator() {
  local exit_code=$?
  if [ "${STARTED_EMULATOR:-0}" = "1" ] && [ -n "${EMULATOR_PID:-}" ]; then
    echo "🛑 [android-emulator] Received exit/SIGTERM — shutting down emulator (PID=$EMULATOR_PID)."
    # Graceful shutdown — instructs the emulator to flush state and exit.
    adb emu kill >/dev/null 2>&1 || true
    # Give it 5s to land before forcing.
    for _ in 1 2 3 4 5; do
      if ! kill -0 "$EMULATOR_PID" 2>/dev/null; then
        break
      fi
      sleep 1
    done
    if kill -0 "$EMULATOR_PID" 2>/dev/null; then
      echo "   emulator did not exit gracefully — sending SIGTERM."
      kill -TERM "$EMULATOR_PID" 2>/dev/null || true
      sleep 2
      kill -KILL "$EMULATOR_PID" 2>/dev/null || true
    fi
  fi
  exit "$exit_code"
}
trap cleanup_emulator EXIT INT TERM

# ── 4b. Wait for adb to see the device + Android to finish booting ──────────
if [ "${STARTED_EMULATOR:-0}" = "1" ]; then
  echo "⏳ [android-emulator] Waiting for device to come online…"
  # 60s for the QEMU bridge to register the emulator with adb.
  WAIT_DEADLINE=$((SECONDS + 60))
  until adb devices | awk 'NR>1 && $1 ~ /^emulator-/ && $2=="device"' | grep -q .; do
    if [ $SECONDS -ge $WAIT_DEADLINE ]; then
      echo "❌ Emulator didn't register with adb within 60s. Last lines of emulator log:"
      tail -40 "$ANDROID_HOME/logs/emulator-$AVD_NAME.log" || true
      exit 1
    fi
    sleep 2
  done
  TARGET_SERIAL=$(adb devices | awk 'NR>1 && $1 ~ /^emulator-/ && $2=="device" {print $1; exit}')
fi

echo "⏳ [android-emulator] Waiting for sys.boot_completed on $TARGET_SERIAL…"
# Cold-boot of a fresh API 34 emulator takes 30–90 s under software rendering.
WAIT_DEADLINE=$((SECONDS + 300))
until [ "$(adb -s "$TARGET_SERIAL" shell getprop sys.boot_completed 2>/dev/null | tr -d '\r\n')" = "1" ]; do
  if [ $SECONDS -ge $WAIT_DEADLINE ]; then
    echo "❌ sys.boot_completed didn't flip to 1 within 5 minutes. Last lines of emulator log:"
    tail -40 "$ANDROID_HOME/logs/emulator-$AVD_NAME.log" || true
    exit 1
  fi
  sleep 3
done

echo "✅ [android-emulator] Emulator '$AVD_NAME' ready ($TARGET_SERIAL). Holding open until Aspire shutdown."

# ── 5. Hold the script in the foreground for the rest of the AppHost session ─
if [ "${STARTED_EMULATOR:-0}" = "1" ]; then
  # Block on the emulator PID. If the emulator dies for any reason, this
  # returns and the EXIT trap fires — Aspire then marks the resource
  # `FailedToStart` / exited so the dev knows something went wrong.
  wait "$EMULATOR_PID"
else
  # Reused emulator — we don't own its PID, so loop on a cheap adb probe to
  # detect disconnection. SIGTERM still fires the trap (which no-ops for the
  # reused case) so Aspire shutdown is clean and immediate.
  while adb -s "$TARGET_SERIAL" get-state >/dev/null 2>&1; do
    sleep 5
  done
  echo "ℹ️  [android-emulator] Reused emulator '$TARGET_SERIAL' disappeared — exiting."
fi
