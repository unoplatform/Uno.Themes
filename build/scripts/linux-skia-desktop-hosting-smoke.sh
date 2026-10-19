#!/bin/bash
set -x
set -uo pipefail
IFS=$'\n\t'

# TEMP DIAG version: runs the smoke twice (catalog order, then reversed) with a 60 s reclamation
# poll and an end-of-run census. If a guest ALC is still alive at the end of the reversed run
# (where the earlier diagnostic run saw Simple and Cupertino stay alive), the app holds while a
# heap dump is taken and analysed. Dumps land in $SMOKE_DIAG_DIR.

cd "src/samples/ThemesSampleApp/bin/Release/net10.0-desktop"

diag_dir="${SMOKE_DIAG_DIR:-/tmp/smoke-diag}"
mkdir -p "$diag_dir"
dotnet tool install -g dotnet-dump || true
export PATH="$PATH:$HOME/.dotnet/tools"

export SMOKE_DIAG=1

xvfb-run --auto-servernum --server-args='-screen 0 1280x1024x24' bash -c "{ fluxbox & } ; timeout 900 dotnet ThemesSampleApp.dll --smoke"
status1=$?

export SMOKE_REVERSE=1
export SMOKE_HOLD="$diag_dir/hold.pid"
rm -f "$SMOKE_HOLD"

xvfb-run --auto-servernum --server-args='-screen 0 1280x1024x24' bash -c "{ fluxbox & } ; timeout 900 dotnet ThemesSampleApp.dll --smoke" &
run2=$!

while kill -0 "$run2" 2>/dev/null && [ ! -f "$SMOKE_HOLD" ]; do sleep 2; done

if [ -f "$SMOKE_HOLD" ]; then
	sleep 2
	pid=$(cat "$SMOKE_HOLD")
	dotnet-dump collect -p "$pid" --type Heap -o "$diag_dir/smoke.dmp"
	dotnet-dump analyze "$diag_dir/smoke.dmp" -c "dumpheap -type GuestAssemblyLoadContext" -c "dumpheap -type System.Reflection.LoaderAllocator" -c "gchandles -stat" -c "exit" | tee "$diag_dir/alcs.txt"
	for addr in $(grep -E '^\s+[0-9a-f]{8,}\s' "$diag_dir/alcs.txt" | awk '{print $1}'); do
		dotnet-dump analyze "$diag_dir/smoke.dmp" -c "dumpobj $addr" -c "gcroot $addr" -c "exit" > "$diag_dir/gcroot-$addr.txt" 2>&1 || true
		echo "===== object $addr"
		grep -E '^Name:|_state|_name' "$diag_dir/gcroot-$addr.txt" | head -5
		echo "----- root kinds"
		grep -E '^\s{4}\S' "$diag_dir/gcroot-$addr.txt" | grep -v -- '->' | sed -E 's/^\s*\S+\s*//' | sort | uniq -c
		echo "----- root chains other than weak interior pointers (10)"
		awk '/^    [^ ]/ && !/->/ { keep = ($0 !~ /\(10\)/) } keep { print }' "$diag_dir/gcroot-$addr.txt" | head -200
	done
	kill "$pid" || true
fi

wait "$run2"
status2=$?

echo "run1 (catalog order) exit=$status1, run2 (reversed) exit=$status2"
[ "$status1" -eq 0 ] && [ "$status2" -eq 0 ]
