#!/bin/bash
# Opens an SSH session to the WSL host via the internal-only sshd (port 2222).
# sshd is bound to the WSL eth0 IP — not exposed to Windows host or LAN.
#
# Auth uses a dedicated ed25519 key bind-mounted at /etc/devcontainer-ssh/
# (NOT ~/.ssh/). The non-standard path keeps the key out of reach of any
# agent that naively shells out to `ssh user@host` — only this wrapper
# references it explicitly via -i. The keypair is provisioned on the WSL
# host by .devcontainer/initialize-host.sh.
KEY_PATH="/etc/devcontainer-ssh/id_ed25519"

if [[ ! -r "$KEY_PATH" ]]; then
  echo "ssh-wsl-host: key not found at $KEY_PATH" >&2
  echo "  Restart the devcontainer so initializeCommand (initialize-host.sh)" >&2
  echo "  can provision the keypair on the WSL host." >&2
  exit 1
fi

GATEWAY="$(ip route show default | awk '{print $3}')"

# WSL_HOST_USER is injected via containerEnv from the host's $USER.
# Fall back to a prompt only if the env var isn't set.
WSL_USER="${WSL_HOST_USER:-}"
if [[ -z "$WSL_USER" ]]; then
  read -rp "WSL username: " WSL_USER
fi

# Set a dark-red background to visually distinguish the WSL host terminal.
# OSC 11 sets the terminal background color; restored on exit via trap.
printf '\e]11;#2e1a1a\a'
trap 'printf "\e]11;reset\a"' EXIT

# Change to the workspace folder on the WSL host (same bind-mount source)
REMOTE_CD=""
if [[ -n "${WSL_HOST_WORKSPACE:-}" ]]; then
  REMOTE_CD="cd '${WSL_HOST_WORKSPACE}' 2>/dev/null; "
fi

# IdentitiesOnly + IdentityAgent=none + PreferredAuthentications=publickey
# guarantee this command can only authenticate with the bind-mounted key —
# no ssh-agent forwarding, no password fallback, no opportunistic key reuse.
exec ssh \
  -o StrictHostKeyChecking=accept-new \
  -o UserKnownHostsFile="$HOME/.ssh/wsl-host-known_hosts" \
  -o IdentitiesOnly=yes \
  -o IdentityAgent=none \
  -o PreferredAuthentications=publickey \
  -o ConnectTimeout=5 \
  -i "$KEY_PATH" \
  -t \
  -p 2222 \
  "${WSL_USER}@${GATEWAY}" \
  "${REMOTE_CD}exec \$SHELL -l"
