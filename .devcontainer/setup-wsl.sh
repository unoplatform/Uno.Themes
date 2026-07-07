#!/usr/bin/env bash
# Setup script for running the Uno.Themes devcontainer on WSL (Windows
# Subsystem for Linux).
#
# This is a one-shot provisioning script for the WSL host. It will:
#   - install openssh-server and stand up a devcontainer-only sshd on port 2222,
#     bound to the WSL-internal eth0/docker0 IPs (NOT exposed to Windows or LAN),
#     used by the devcontainer's ssh-wsl-host helper to drop the user into a host
#     shell with the dedicated ed25519 key provisioned by initialize-host.sh
#   - prompt for an optional read-only GitHub PAT, validate it has no write
#     scopes, and persist it in the user's shell rc so the devcontainer's
#     GitHub MCP server can use it
#
# Re-run safely: every step is idempotent.

set -euo pipefail

echo "🚀 Uno.Themes WSL Setup"
echo "======================="
echo ""

# Check if running in WSL
if [[ -z "${WSL_DISTRO_NAME:-}" ]]; then
    echo "⚠️  Warning: This doesn't appear to be a WSL environment."
    echo "   WSL_DISTRO_NAME is not set."
    read -p "Continue anyway? (y/N) " -n 1 -r
    echo
    if [[ ! $REPLY =~ ^[Yy]$ ]]; then
        exit 1
    fi
fi

# Update package list
echo "📦 Updating apt index..."
sudo apt-get update

# ── SSH daemon for devcontainer access ──────────────────────────────────────
# Starts sshd on port 2222, bound to the WSL eth0 IP and (if present) the
# docker0 bridge IP so containers can reach it via their default gateway.
# These IPs change on every WSL reboot, so a helper script resolves them
# dynamically, rewrites the sshd drop-in config, and (re)starts sshd.
# ────────────────────────────────────────────────────────────────────────────

SSHD_PORT=2222
SSHD_CONFIG="/etc/ssh/sshd_config.d/devcontainer.conf"
SSHD_LAUNCHER="/usr/local/bin/start-devcontainer-sshd.sh"

echo ""
echo "🔒 Setting up SSH daemon for devcontainer access..."
echo ""

# Install openssh-server if missing
if ! command -v sshd &> /dev/null; then
    echo "Installing openssh-server..."
    sudo apt-get install -y openssh-server
fi

# Write a launcher script that resolves the current eth0 IP, writes the
# sshd config, and starts/restarts the daemon.
sudo tee "$SSHD_LAUNCHER" > /dev/null <<'LAUNCHER'
#!/bin/bash
# Resolve the current WSL-internal IP and (re)start sshd on port 2222.
SSHD_PORT=2222
SSHD_CONFIG="/etc/ssh/sshd_config.d/devcontainer.conf"

WSL_IP=$(ip -4 addr show eth0 2>/dev/null | grep -oP '(?<=inet\s)\d+(\.\d+){3}')
if [[ -z "$WSL_IP" ]]; then
    echo "start-devcontainer-sshd: could not determine eth0 IP — skipping." >&2
    exit 1
fi

# Also resolve the docker0 bridge IP so containers can reach sshd via their default gateway.
DOCKER_IP=$(ip -4 addr show docker0 2>/dev/null | grep -oP '(?<=inet\s)\d+(\.\d+){3}')

# Write/overwrite the drop-in config with the current IP(s)
cat > "$SSHD_CONFIG" <<EOF
# Devcontainer access — bound to WSL-internal interfaces only
ListenAddress ${WSL_IP}:${SSHD_PORT}
${DOCKER_IP:+ListenAddress ${DOCKER_IP}:${SSHD_PORT}}
PasswordAuthentication yes
EOF

# Kill any existing sshd listening on the devcontainer port, then start fresh.
# pkill -f won't match the port (it's in the config, not the CLI), so use ss to
# find the PID actually bound to the port.
BOUND_PID=$(ss -tlnp "sport = :${SSHD_PORT}" 2>/dev/null \
    | grep -oP '(?<=pid=)\d+' | sort -u)
if [[ -n "$BOUND_PID" ]]; then
    echo "$BOUND_PID" | xargs kill 2>/dev/null || true
    # Brief wait for the port to be released
    sleep 0.3
fi
mkdir -p /run/sshd
/usr/sbin/sshd -f /etc/ssh/sshd_config
echo "start-devcontainer-sshd: sshd listening on ${WSL_IP}:${SSHD_PORT}${DOCKER_IP:+ and ${DOCKER_IP}:${SSHD_PORT}}"
LAUNCHER
sudo chmod +x "$SSHD_LAUNCHER"
echo "✅ Launcher script written to $SSHD_LAUNCHER"

# Ensure passwordless sudo for the launcher (needed by the profile snippet)
SSHD_SUDOERS="/etc/sudoers.d/sshd-devcontainer"
sudo tee "$SSHD_SUDOERS" > /dev/null <<SUDOEOF
$(whoami) ALL=(root) NOPASSWD: ${SSHD_LAUNCHER}
SUDOEOF
sudo chmod 0440 "$SSHD_SUDOERS"

# Start sshd now
if command -v systemctl &> /dev/null && systemctl status ssh &>/dev/null; then
    sudo "$SSHD_LAUNCHER"
    sudo systemctl enable ssh
    echo "✅ sshd started and enabled via systemd."
else
    sudo "$SSHD_LAUNCHER"
fi

# ── Persist sshd auto-start for non-systemd WSL ─────────────────────────────
# Without systemd, sshd won't survive a WSL restart. Add a snippet to
# ~/.profile that re-runs the launcher on first login.
SSHD_SNIPPET='
# Auto-start sshd for devcontainer access (WSL, non-systemd)
if ! ss -tlnp 2>/dev/null | grep -q ":2222 "; then
    sudo /usr/local/bin/start-devcontainer-sshd.sh 2>/dev/null
fi
'

if ! grep -q 'Auto-start sshd for devcontainer' ~/.profile 2>/dev/null; then
    echo ""
    read -p "Add sshd auto-start snippet to ~/.profile? (Y/n) " -n 1 -r
    echo
    if [[ ! $REPLY =~ ^[Nn]$ ]]; then
        echo "$SSHD_SNIPPET" >> ~/.profile
        echo "✅ sshd auto-start snippet added to ~/.profile."
    fi
fi

# ── GitHub read-only token for devcontainer MCP ──────────────────────────────
# The devcontainer reads GH_TOKEN_READONLY from the WSL environment to register
# a GitHub MCP server (PR comments, build status, issues). Only read-only
# tokens are accepted — the container validates and rejects tokens with write
# scopes on startup.
# ─────────────────────────────────────────────────────────────────────────────

echo ""
echo "🔑 GitHub read-only token (optional)"
echo ""
echo "The devcontainer can register a GitHub MCP server so AI agents can read"
echo "PR comments, build status, and issues. This requires a read-only GitHub"
echo "personal access token (fine-grained PAT recommended)."
echo ""
echo "Create one at: https://github.com/settings/personal-access-tokens/new"
echo "  → Repository access: select your repo(s)"
echo "  → Permissions: Contents (read), Pull requests (read), Issues (read)"
echo "  → Do NOT grant any write permissions"
echo ""

read -p "Set up a GitHub read-only token now? (y/N) " -n 1 -r
echo
if [[ $REPLY =~ ^[Yy]$ ]]; then
    echo ""
    read -s -p "Paste your read-only GitHub token: " -r GH_TOKEN_INPUT
    echo ""  # newline after silent input

    if [[ -z "$GH_TOKEN_INPUT" ]]; then
        echo "⚠️  No token entered — skipping."
    else
        # Validate the token authenticates at all
        HTTP_STATUS=$(curl -sS -o /dev/null -w "%{http_code}" \
            -H "Authorization: token ${GH_TOKEN_INPUT}" \
            https://api.github.com/user 2>/dev/null) || true

        if [[ "$HTTP_STATUS" != "200" ]]; then
            echo "❌ Token validation failed (HTTP $HTTP_STATUS). Check that the token is valid."
            echo "   Skipping — you can add it manually later:"
            echo "   echo 'export GH_TOKEN_READONLY=your_token' >> ~/.bashrc"
        else
            # Check for write scopes (classic PATs)
            SCOPES_HEADER=$(curl -sS -f -H "Authorization: token ${GH_TOKEN_INPUT}" \
                -I https://api.github.com/user 2>/dev/null \
                | grep -i '^x-oauth-scopes:' | cut -d: -f2- | tr -d '[:space:]') || true

            TOKEN_OK=true
            if [[ -n "$SCOPES_HEADER" ]]; then
                WRITE_SCOPES="repo,public_repo,delete_repo,gist,workflow,write:org,admin:org,write:public_key,admin:public_key,write:repo_hook,admin:repo_hook,admin:org_hook,write:packages,write:gpg_key,admin:gpg_key,write:discussion,admin:enterprise"
                IFS=',' read -ra BLOCKED <<< "$WRITE_SCOPES"
                for scope in "${BLOCKED[@]}"; do
                    scope=$(echo "$scope" | xargs)
                    if echo ",$SCOPES_HEADER," | grep -qi ",$scope,"; then
                        echo "❌ Token has write scope '$scope'. Only read-only tokens are allowed."
                        TOKEN_OK=false
                        break
                    fi
                done
            else
                # Fine-grained PAT — probe a write endpoint
                WRITE_TEST=$(curl -sS -o /dev/null -w "%{http_code}" \
                    -H "Authorization: token ${GH_TOKEN_INPUT}" \
                    -H "Content-Type: application/json" \
                    -X POST https://api.github.com/user/repos \
                    -d '{"name":""}' 2>/dev/null) || true

                if [[ "$WRITE_TEST" == "422" || "$WRITE_TEST" == "201" ]]; then
                    echo "❌ Token has write permissions (repo create returned HTTP $WRITE_TEST)."
                    TOKEN_OK=false
                fi
            fi

            if [[ "$TOKEN_OK" == true ]]; then
                echo "✅ Token validated — read-only confirmed."

                # Determine which shell profile to use. $SHELL is unreliable when
                # launched from wsl.exe (often inherits bash from the parent) and
                # may be unset under `set -u`, so consult the user's login shell
                # from getent first and use ${SHELL:-} only as a fallback.
                SHELL_RC="$HOME/.bashrc"
                LOGIN_SHELL="$(getent passwd "$USER" 2>/dev/null | cut -d: -f7)"
                if [[ -f "$HOME/.zshrc" && ( "$LOGIN_SHELL" == */zsh || "${SHELL:-}" == */zsh ) ]]; then
                    SHELL_RC="$HOME/.zshrc"
                fi

                EXPORT_LINE="export GH_TOKEN_READONLY=\"${GH_TOKEN_INPUT}\""

                if grep -q 'GH_TOKEN_READONLY' "$SHELL_RC" 2>/dev/null; then
                    # Update existing export
                    sed -i "s|^export GH_TOKEN_READONLY=.*|${EXPORT_LINE}|" "$SHELL_RC"
                    echo "✅ Updated GH_TOKEN_READONLY in $SHELL_RC"
                else
                    echo "" >> "$SHELL_RC"
                    echo "# GitHub read-only token for devcontainer MCP" >> "$SHELL_RC"
                    echo "$EXPORT_LINE" >> "$SHELL_RC"
                    echo "✅ Added GH_TOKEN_READONLY to $SHELL_RC"
                fi

                export GH_TOKEN_READONLY="$GH_TOKEN_INPUT"
                echo "   Token is active in this shell and will persist for new shells."
            else
                echo "   Skipping — create a read-only token and add it manually:"
                echo "   echo 'export GH_TOKEN_READONLY=your_token' >> ~/.bashrc"
            fi
        fi
    fi
else
    echo "   Skipped. To add later, export GH_TOKEN_READONLY in ~/.bashrc or ~/.zshrc."
fi

echo ""
echo "✨ Setup complete!"
echo ""
echo "Next steps:"
echo "  1. Open the workspace in VS Code and reopen in the devcontainer."
echo "  2. Once attached, run a single-platform restore + build:"
echo "       dotnet restore Uno.Themes.sln -p:TargetFrameworkOverride=desktop"
echo "       dotnet build Uno.Themes.sln -c Debug -p:TargetFrameworkOverride=desktop"
echo "  3. Run the Material sample (desktop):"
echo "       dotnet run --project src/samples/MaterialSampleApp/MaterialSampleApp.csproj -f net10.0-desktop"
echo ""
