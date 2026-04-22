#!/bin/bash
set -x
set -euo pipefail
IFS=$'\n\t'

export DOTNET_MODIFIABLE_ASSEMBLIES=debug

# BUILD_CONFIG defaults to Release so the existing Release pipeline keeps working
# unchanged. The hot-reload stage sets BUILD_CONFIG=Debug (DevServer is Debug-only).
BUILD_CONFIG="${BUILD_CONFIG:-Release}"

results_path="$UNO_RUNTIME_TESTS_OUTPUT_PATH"

cd "$ProjectPath/bin/$BUILD_CONFIG/net10.0-desktop"

xvfb-run --auto-servernum --server-args='-screen 0 1280x1024x24' bash -c "{ fluxbox & } ; dotnet $SampleAppName.dll --runtime-tests=\"$results_path\"" || true

if [ ! -f "$results_path" ]; then
    echo "Runtime tests did not produce results at $results_path"
    exit 1
fi

python3 - <<'PY'
import os
import xml.etree.ElementTree as ET

results_path = os.environ["UNO_RUNTIME_TESTS_OUTPUT_PATH"]

if not os.path.exists(results_path):
    raise SystemExit(f"Runtime tests results not found at {results_path}")

try:
    tree = ET.parse(results_path)
except ET.ParseError as exc:
    raise SystemExit(f"Unable to parse runtime tests results: {exc}")

cases = tree.findall('.//test-case')
if not cases:
    raise SystemExit("Runtime tests produced no test cases.")

print(f"Validated {len(cases)} runtime test cases.")
PY
