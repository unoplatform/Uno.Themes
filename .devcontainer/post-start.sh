#!/usr/bin/env bash
set -euo pipefail

sudo /usr/local/bin/init-firewall.sh

dotnet dev-certs https --trust || true

history_file="${HISTFILE:-$HOME/.bash_history}"
printf 'claude --dangerously-skip-permissions\n' >> "$history_file"

echo "Registering Claude MCPs for Uno Platform: uno (HTTP docs server)."
echo "To verify, run: claude mcp list"

claude mcp add --scope user --transport http uno https://mcp.platform.uno/v1 || true

echo "Claude MCP registration complete. If you encounter issues, run 'claude mcp list' or 'claude mcp inspect uno'."
