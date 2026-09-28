#!/bin/bash
set -x
set -euo pipefail
IFS=$'\n\t'

# Drives the ThemesSampleApp hosting smoke (--smoke): loads Material, Cupertino, Simple and
# Fluent in sequence through secondary ALCs, unloads, and verifies each unloaded guest ALC is
# reclaimed.
# The wrapper exits 0 on pass / 1 on fail; `timeout` guards a hung run (exit 124).

cd "src/samples/ThemesSampleApp/bin/Release/net10.0-desktop"

xvfb-run --auto-servernum --server-args='-screen 0 1280x1024x24' bash -c "{ fluxbox & } ; timeout 900 dotnet ThemesSampleApp.dll --smoke"
