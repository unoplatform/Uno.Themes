#!/usr/bin/env bash
# Shared Android SDK bootstrap helpers. Sourced (not executed) by both
# `start-emulator.sh` and `launch-android.sh` so each script remains usable
# standalone — running either one against a fresh ANDROID_HOME will install
# cmdline-tools on first invocation and no-op on every subsequent call.
#
# Requires the caller to have set ANDROID_HOME + ANDROID_SDK_ROOT and to have
# `curl` + `unzip` on PATH. Does not export any variables of its own.

# Guard against double-sourcing in case both scripts source this file inside
# the same shell (currently they don't, but the guard is cheap insurance
# against later refactors).
if [ -n "${_UNO_THEMES_ANDROID_SDK_BOOTSTRAP_SOURCED:-}" ]; then
  return 0
fi
_UNO_THEMES_ANDROID_SDK_BOOTSTRAP_SOURCED=1

# Canonical paths under ANDROID_HOME. Exposed so callers can run sdkmanager /
# avdmanager / emulator without redoing the path arithmetic.
CMDLINE_TOOLS_BIN="$ANDROID_HOME/cmdline-tools/latest/bin"
SDKMANAGER="$CMDLINE_TOOLS_BIN/sdkmanager"
AVDMANAGER="$CMDLINE_TOOLS_BIN/avdmanager"
EMULATOR_BIN="$ANDROID_HOME/emulator/emulator"

# Tracks whether `accept_licenses_once` has already run in this shell so a
# pipeline that installs several SDK components doesn't re-prompt for licenses
# on every install.
_LICENSES_ACCEPTED=0

# Accepts the Android SDK license suite. Safe to call multiple times — only
# the first call actually shells out to sdkmanager. `|| true` shields against
# sdkmanager exiting non-zero when there are no pending licenses to accept
# (post-acceptance reruns are a no-op).
accept_licenses_once() {
  if [ "$_LICENSES_ACCEPTED" = "0" ]; then
    yes | "$SDKMANAGER" --licenses >/dev/null 2>&1 || true
    _LICENSES_ACCEPTED=1
  fi
}

# Installs Android cmdline-tools into $ANDROID_HOME/cmdline-tools/latest if
# they aren't already present. Idempotent — bypasses the download when the
# canonical sdkmanager binary already exists.
bootstrap_cmdline_tools() {
  if [ -x "$SDKMANAGER" ]; then
    return 0
  fi

  echo "📦 [android-sdk-bootstrap] cmdline-tools not present in $ANDROID_HOME — bootstrapping."
  echo "   Pin: commandlinetools-linux-11076708 (released Sep 2023; covers Android API ≤ 34)."

  local clt_zip
  clt_zip=$(mktemp -t cmdline-tools.XXXXXX.zip)
  curl -fsSL -o "$clt_zip" \
    https://dl.google.com/android/repository/commandlinetools-linux-11076708_latest.zip
  mkdir -p "$ANDROID_HOME/cmdline-tools"
  # The zip extracts as `cmdline-tools/`; rename to `latest/` so sdkmanager
  # finds itself at the canonical path the Android SDK layout expects.
  unzip -q -o "$clt_zip" -d "$ANDROID_HOME/cmdline-tools.staging"
  rm -rf "$ANDROID_HOME/cmdline-tools/latest"
  mv "$ANDROID_HOME/cmdline-tools.staging/cmdline-tools" "$ANDROID_HOME/cmdline-tools/latest"
  rmdir "$ANDROID_HOME/cmdline-tools.staging"
  rm "$clt_zip"

  accept_licenses_once
}
