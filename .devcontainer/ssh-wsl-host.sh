#!/bin/bash
# Opens an SSH session to the WSL host via the internal-only sshd (port 2222).
# sshd is bound to the WSL eth0 IP — not exposed to Windows host or LAN.
# Credentials stay on the host — the agent never sees them.
GATEWAY="$(ip route show default | awk '{print $3}')"
read -rp "WSL username: " WSL_USER

# Set a dark-red background to visually distinguish the WSL host terminal.
# OSC 11 sets the terminal background color; restored on exit via trap.
printf '\e]11;#2e1a1a\a'
trap 'printf "\e]11;reset\a"' EXIT

# Change to the workspace folder on the WSL host (same bind-mount source)
REMOTE_CD=""
if [[ -n "${WSL_HOST_WORKSPACE:-}" ]]; then
  REMOTE_CD="cd '${WSL_HOST_WORKSPACE}' 2>/dev/null; "
fi

exec ssh \
  -o StrictHostKeyChecking=accept-new \
  -o UserKnownHostsFile="$HOME/.ssh/wsl-host-known_hosts" \
  -o ConnectTimeout=5 \
  -t \
  -p 2222 \
  "${WSL_USER}@${GATEWAY}" \
  "${REMOTE_CD}exec \$SHELL -l"
