#!/usr/bin/env bash
# Uno.Themes runtime-test runner, invoked by the Aspire AppHost resource `simple-runtime-tests`.
# Single foreground process so Aspire's Start/Stop/log-streaming semantics apply, and the tile
# turns red (nonzero exit) when tests fail.
#
# Mirrors the CI driver (build/scripts/linux-skia-desktop-runtime-tests.sh) but:
#   • builds Debug by default (fast local loop; override with CONFIG=Release for exact CI parity),
#   • runs directly when a DISPLAY is available (watchable window) and only falls back to
#     xvfb + fluxbox when headless,
#   • fails on missing results, zero test-cases, OR any failed case (CI only checks existence),
#     and prints a one-line pass/fail summary as the last log line.
#
# Env vars (forwarded from the AppHost via the WithEnvironment allowlist, or overridden on the
# resource in the dashboard):
#   CONFIG                        Debug | Release. Default: Debug.
#   UNO_RUNTIME_TESTS_RUN_TESTS   Engine filter JSON. Default: '{}' (run everything).
#                                 e.g. '{"Filter":{"Value":"Given_SeedColorPalette"},"Attempts":1}'
#   UNO_RUNTIME_TESTS_OUTPUT_PATH NUnit results path. Default: a temp file.
set -euo pipefail
IFS=$'\n\t'

SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
REPO_ROOT="$(cd "$SCRIPT_DIR/../../../.." && pwd)"

CONFIG="${CONFIG:-Debug}"
PROJECT="$REPO_ROOT/src/samples/SimpleSampleApp/SimpleSampleApp.csproj"
APP_DLL="$REPO_ROOT/src/samples/SimpleSampleApp/bin/$CONFIG/net10.0-desktop/SimpleSampleApp.dll"
export UNO_RUNTIME_TESTS_RUN_TESTS="${UNO_RUNTIME_TESTS_RUN_TESTS:-{\}}"
export UNO_RUNTIME_TESTS_OUTPUT_PATH="${UNO_RUNTIME_TESTS_OUTPUT_PATH:-$(mktemp -t uno-themes-runtime-tests-XXXXXX.xml)}"

# Hot-reload runtime tests (Given_HotReload) fail silently without modifiable assemblies.
export DOTNET_MODIFIABLE_ASSEMBLIES=debug

echo "▶ Building SimpleSampleApp ($CONFIG, net10.0-desktop) …"
# -p:TargetFrameworkOverride=desktop collapses the sample + its multi-targeted library deps to a
# single TFM. Without it the build demands android/ios/wasm workloads (NETSDK1147); see
# specs/lessons.md. As a command-line global it also wins over any crosstargeting_override.props.
dotnet build "$PROJECT" -c "$CONFIG" -f net10.0-desktop -p:TargetFrameworkOverride=desktop

if [[ ! -f "$APP_DLL" ]]; then
	echo "✗ Expected app assembly not found: $APP_DLL" >&2
	exit 1
fi

echo "▶ Running runtime tests → $UNO_RUNTIME_TESTS_OUTPUT_PATH"
echo "  filter: $UNO_RUNTIME_TESTS_RUN_TESTS"

# The embedded runner reads its output destination from UNO_RUNTIME_TESTS_OUTPUT_PATH (exported
# above) — without it the app aborts the runner and launches normally. We cd into the app's bin
# directory before launching (as CI does) so asset/config resolution matches a normal run.
APP_DIR="$(dirname "$APP_DLL")"
APP_NAME="$(basename "$APP_DLL")"

if [[ -n "${DISPLAY:-}" ]]; then
	# A display is available (WSLg / desktop Linux / macOS) — run directly so the window is watchable.
	( cd "$APP_DIR" && dotnet "$APP_NAME" --runtime-tests="$UNO_RUNTIME_TESTS_OUTPUT_PATH" ) || true
else
	# Headless — match CI: virtual framebuffer + a minimal WM so Skia has somewhere to draw. Paths are
	# expanded by THIS shell into the command string (the inner shell wouldn't see non-exported vars).
	xvfb-run --auto-servernum --server-args='-screen 0 1280x1024x24' \
		bash -c "{ fluxbox >/dev/null 2>&1 & } ; cd \"$APP_DIR\" && dotnet \"$APP_NAME\" --runtime-tests=\"$UNO_RUNTIME_TESTS_OUTPUT_PATH\"" || true
fi

# ── Validate results ──────────────────────────────────────────────────────────
# Fail on missing file, zero cases, or any failed/errored case. Prints a summary line last.
python3 - <<'PY'
import os, sys, xml.etree.ElementTree as ET

path = os.environ["UNO_RUNTIME_TESTS_OUTPUT_PATH"]
if not os.path.exists(path):
    print(f"✗ Runtime tests produced no results at {path}", file=sys.stderr)
    sys.exit(1)

try:
    root = ET.parse(path).getroot()
except ET.ParseError as exc:
    print(f"✗ Unable to parse runtime-test results: {exc}", file=sys.stderr)
    sys.exit(1)

cases = root.findall('.//test-case')
if not cases:
    print("✗ Runtime tests produced no test cases.", file=sys.stderr)
    sys.exit(1)

def is_failure(c):
    # NUnit-style: success="False" or result in {Failure,Error}. Ignored/skipped don't count as failures.
    success = (c.get("success") or "").strip().lower()
    result = (c.get("result") or "").strip().lower()
    if success == "false":
        return True
    return result in ("failure", "error", "failed")

failed = [c for c in cases if is_failure(c)]
total = len(cases)
print(f"{'✗' if failed else '✓'} Runtime tests: {total - len(failed)}/{total} passed, {len(failed)} failed.")
for c in failed[:20]:
    print(f"    FAILED: {c.get('name')}")
sys.exit(1 if failed else 0)
PY
