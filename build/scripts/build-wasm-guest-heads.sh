#!/bin/bash
set -euo pipefail

# Builds the three theme sample heads for browserwasm so ThemesSampleApp can package their
# output as its guest payload. The wasm wrapper carries no ProjectReference to the heads
# (StaticWebAssets merges referenced projects' web assets and the heads' identical
# WasmCSS/Fonts.css collide), so every wasm wrapper build must run this first.
#
# Usage: build-wasm-guest-heads.sh [Configuration] [extra msbuild args...]

CONFIGURATION="${1:-Release}"
if [ "$#" -gt 0 ]; then
  shift
fi

for head in MaterialSampleApp CupertinoSampleApp SimpleSampleApp; do
  dotnet build "src/samples/$head/$head.csproj" \
    -c "$CONFIGURATION" \
    -f net10.0-browserwasm \
    -p:TargetFrameworkOverride=browserwasm \
    "$@"
done
