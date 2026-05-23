#!/usr/bin/env bash
# Runs on the WSL HOST (not inside the container) via initializeCommand,
# before the devcontainer starts.
#
# Idempotently provisions a dedicated SSH keypair for the devcontainer's
# ssh-wsl-host helper. The key is bind-mounted into the container at a
# non-standard path (/etc/devcontainer-ssh/, not ~/.ssh/), so AI agents
# running inside the container won't pick it up by default — only
# ssh-wsl-host.sh references it explicitly with -i.
#
# Why a dedicated key (and not the user's regular id_*): we want to limit
# blast radius. This key only authorizes connections from the devcontainer
# back to the WSL host's internal sshd on port 2222.
set -euo pipefail

KEY_NAME="unothemes-devcontainer-ed25519"
KEY_PATH="${HOME}/.ssh/${KEY_NAME}"
AUTHORIZED_KEYS="${HOME}/.ssh/authorized_keys"

mkdir -p "${HOME}/.ssh"
chmod 700 "${HOME}/.ssh"

if [[ ! -f "${KEY_PATH}" ]]; then
  echo "initialize-host: generating dedicated SSH key for devcontainer ssh-wsl-host (${KEY_NAME})..."
  ssh-keygen -t ed25519 -N "" -C "uno-themes-devcontainer-host-shell" -f "${KEY_PATH}" >/dev/null
fi
chmod 600 "${KEY_PATH}"
chmod 644 "${KEY_PATH}.pub"

touch "${AUTHORIZED_KEYS}"
chmod 600 "${AUTHORIZED_KEYS}"

# Append the public key if its key material isn't already in authorized_keys.
# Compare by key material (field 2 of the public key file, which is field 3 of
# an authorized_keys line that carries leading options) so a re-added entry
# with a different comment or different restrictions doesn't create a duplicate.
#
# The entry is prefixed with sshd authorized_keys options that limit what a
# compromised devcontainer can do with this key:
#   - restrict           : opt out of every permission (forwarding, command, etc.);
#                          the more-specific options below re-enable only what we
#                          need. Future-proofs us against new sshd capabilities.
#   - pty                : re-enable PTY allocation (ssh-wsl-host runs `exec $SHELL -l`).
# Forwarding (agent/X11/port/StreamLocal) and `command=` overrides stay denied
# by `restrict`. We cannot pin `from=` to a specific IP because the WSL eth0 /
# docker0 IPs change on every WSL reboot.
KEY_OPTIONS='restrict,pty'
KEY_FIELD=$(awk '{print $2}' "${KEY_PATH}.pub")
KEY_LINE="${KEY_OPTIONS} $(cat "${KEY_PATH}.pub")"

# An existing entry matches if its key material (the ssh-ed25519 base64 blob)
# equals ours, regardless of leading options or trailing comment.
already_present=0
if [[ -s "${AUTHORIZED_KEYS}" ]]; then
  while IFS= read -r line; do
    # Strip leading options (everything up to and including the first space
    # before "ssh-" / "ecdsa-" / etc.) then take field 2 = the key material.
    material=$(awk '{ for (i=1;i<=NF;i++) if ($i ~ /^(ssh|ecdsa|sk)-/) { print $(i+1); exit } }' <<< "$line")
    if [[ "$material" == "$KEY_FIELD" ]]; then
      already_present=1
      break
    fi
  done < "${AUTHORIZED_KEYS}"
fi

if (( already_present == 0 )); then
  echo "initialize-host: adding restricted devcontainer public key to ${AUTHORIZED_KEYS}"
  # Ensure the existing file ends with a newline before we append; otherwise our
  # new entry would be concatenated onto the previous line, corrupting both
  # entries and locking the devcontainer key out.
  if [[ -s "${AUTHORIZED_KEYS}" ]] && [[ "$(tail -c1 "${AUTHORIZED_KEYS}" | wc -l)" -eq 0 ]]; then
    printf '\n' >> "${AUTHORIZED_KEYS}"
  fi
  printf '%s\n' "${KEY_LINE}" >> "${AUTHORIZED_KEYS}"
fi
