#!/usr/bin/env bash
set -euo pipefail

sudo /usr/local/bin/init-firewall.sh

# The Docker named volume mounted at ~/.nuget/packages is created with root
# ownership when the volume is first populated (before the developer user
# exists in the container). After a WSL/Docker restart, ownership can also
# revert to root. Either case prevents `dotnet restore` from extracting
# packages and surfaces as `error MSB4236: The SDK 'Uno.Sdk' specified
# could not be found.` Re-asserting ownership on every container start fixes it.
sudo chown -R developer:developer "$HOME/.nuget/packages"

mkdir -p "$HOME/.local/share/Uno Platform"
if [ -d /tmp/uno-platform-host ]; then
  find /tmp/uno-platform-host -maxdepth 1 -type f -exec cp {} "$HOME/.local/share/Uno Platform/" \;
else
  echo "Note: /tmp/uno-platform-host not found — skipping Uno Platform license copy"
fi

dotnet dev-certs https --trust || true

printf 'claude --dangerously-skip-permissions\n' >> "$HOME/.bash_history"
printf 'copilot --autopilot --allow-all\n' >> "$HOME/.bash_history"

# Alias so bare `claude` always starts in bypass-permissions mode (safe in devcontainer)
# Note: CLAUDE_CODE_DISABLE_ADAPTIVE_THINKING is set as a container-wide ENV in the
# Dockerfile so the VS Code Claude Code extension inherits it too (the alias scope
# only covers the `claude` CLI).
for rc in "$HOME/.bashrc" "$HOME/.zshrc"; do
  if [ -f "$rc" ]; then
    # Drop any previous `alias claude=` line so env-var/flag changes propagate on container restart.
    sed -i '/^alias claude=/d' "$rc"
    printf '\nalias claude="claude --dangerously-skip-permissions"\n' >> "$rc"
  fi
done

# Alias so bare `copilot` runs in full autopilot mode (safe in devcontainer)
for rc in "$HOME/.bashrc" "$HOME/.zshrc"; do
  if [ -f "$rc" ] && ! grep -q 'alias copilot=' "$rc"; then
    printf '\nalias copilot="copilot --autopilot --allow-all"\n' >> "$rc"
  fi
done

# ---------------------------------------------------------------------------
# GitHub MCP: read-only token validation + registration
# ---------------------------------------------------------------------------
if [ -n "${GH_TOKEN:-}" ]; then
  echo "Validating GitHub token is read-only..."

  # First, verify the token is actually valid
  AUTH_STATUS=$(curl -sS -o /dev/null -w "%{http_code}" \
    -H "Authorization: token ${GH_TOKEN}" \
    https://api.github.com/user 2>/dev/null || echo "000")

  # Define write scopes that MUST NOT be present
  WRITE_SCOPES="repo,public_repo,delete_repo,gist,workflow,write:org,admin:org,write:public_key,admin:public_key,write:repo_hook,admin:repo_hook,admin:org_hook,write:packages,write:gpg_key,admin:gpg_key,write:discussion,admin:enterprise"

  TOKEN_REJECTED=false
  if [ "$AUTH_STATUS" != "200" ]; then
    echo "ERROR: GitHub token is invalid or unauthorized (HTTP ${AUTH_STATUS})." >&2
    TOKEN_REJECTED=true
  else
    # Fetch token scopes from the API response headers (classic PATs)
    SCOPES_HEADER=$(curl -sS -f -H "Authorization: token ${GH_TOKEN}" \
      -I https://api.github.com/user 2>/dev/null \
      | grep -i '^x-oauth-scopes:' | cut -d: -f2- | tr -d '[:space:]') || true

    if [ -n "$SCOPES_HEADER" ]; then
      # Classic PAT — check each write scope
      IFS=',' read -ra BLOCKED <<< "$WRITE_SCOPES"
      for scope in "${BLOCKED[@]}"; do
        scope=$(echo "$scope" | xargs)  # trim whitespace
        if echo ",$SCOPES_HEADER," | grep -qi ",$scope,"; then
          echo "ERROR: GitHub token has write scope '$scope'. Only read-only tokens are allowed in the devcontainer." >&2
          TOKEN_REJECTED=true
          break
        fi
      done
    else
      # Fine-grained PAT — no X-OAuth-Scopes header is returned.
      # Probe a write endpoint with invalid payload: 403 = no write access (good),
      # 422 = has write access but bad payload (rejected).
      WRITE_TEST=$(curl -sS -o /dev/null -w "%{http_code}" \
        -H "Authorization: token ${GH_TOKEN}" \
        -H "Content-Type: application/json" \
        -X POST https://api.github.com/user/repos \
        -d '{"name":""}' 2>/dev/null) || true

      if [ "$WRITE_TEST" = "422" ] || [ "$WRITE_TEST" = "201" ]; then
        echo "ERROR: GitHub token has write permissions (repo create returned HTTP $WRITE_TEST). Only read-only tokens are allowed in the devcontainer." >&2
        TOKEN_REJECTED=true
      fi
      # 403 or 404 = no write access, which is what we want
    fi
  fi

  if [ "$TOKEN_REJECTED" = true ]; then
    echo "GitHub MCP will NOT be registered. Please use a classic PAT with only read scopes." >&2
    unset GH_TOKEN
  else
    echo "GitHub token validated — no write scopes detected."

    # Register GitHub MCP for Claude Code
    # The server reads GITHUB_PERSONAL_ACCESS_TOKEN; map from GH_TOKEN.
    # NOTE: -e must come AFTER the server name — the flag is variadic and
    # consumes all subsequent args until `--` if placed before the name.
    claude mcp remove github || true
    claude mcp add --scope user --transport stdio \
      github -e "GITHUB_PERSONAL_ACCESS_TOKEN=${GH_TOKEN}" \
      -- npx -y @modelcontextprotocol/server-github 2>/dev/null || true
    echo "Claude GitHub MCP registered."
  fi
else
  echo "Note: GH_TOKEN not set — GitHub MCP will not be registered. Set GH_TOKEN_READONLY in your WSL environment to enable it."
fi

echo "Registering Claude MCPs for Uno Platform: uno (HTTP docs server) and uno-app (stdio app tooling)."
echo "To verify, run: claude mcp list"

claude mcp remove uno || true
claude mcp add --scope user --transport http uno https://mcp.platform.uno/v1 || true
claude mcp remove uno-app || true
claude mcp add --scope user --transport stdio uno-app -- dotnet dnx -y uno.devserver --mcp-app --solution-dir /uno-themes || true

echo "Claude MCP registration complete. If you encounter issues, run 'claude mcp list' or 'claude mcp inspect uno' / 'claude mcp inspect uno-app'."

# ---------------------------------------------------------------------------
# Copilot CLI: MCP servers & settings
# ---------------------------------------------------------------------------
COPILOT_DIR="$HOME/.copilot"
mkdir -p "$COPILOT_DIR"

echo "Registering Copilot MCPs for Uno Platform: uno (HTTP docs server) and uno-app (stdio app tooling)."

COPILOT_MCP_CONFIG="$COPILOT_DIR/mcp-config.json"

# Build MCP servers JSON — include GitHub MCP only if GH_TOKEN is available
if [ -n "${GH_TOKEN:-}" ]; then
  MCP_SERVERS=$(jq -n \
    --arg gh_token "$GH_TOKEN" \
    '{
      "mcpServers": {
        "uno": { "type": "http", "url": "https://mcp.platform.uno/v1" },
        "uno-app": {
          "type": "stdio",
          "command": "dotnet",
          "args": ["dnx", "-y", "--prerelease", "uno.devserver", "--mcp-app", "--solution-dir", "/uno-themes"]
        },
        "github": {
          "type": "stdio",
          "command": "npx",
          "args": ["-y", "@modelcontextprotocol/server-github"],
          "env": { "GITHUB_PERSONAL_ACCESS_TOKEN": $gh_token }
        }
      }
    }')
  echo "Copilot MCPs: uno, uno-app, github"
else
  MCP_SERVERS='{
    "mcpServers": {
      "uno": { "type": "http", "url": "https://mcp.platform.uno/v1" },
      "uno-app": {
        "type": "stdio",
        "command": "dotnet",
        "args": ["dnx", "-y", "--prerelease", "uno.devserver", "--mcp-app", "--solution-dir", "/uno-themes"]
      }
    }
  }'
  echo "Copilot MCPs: uno, uno-app (GitHub MCP skipped — no GH_TOKEN)"
fi

if [ -f "$COPILOT_MCP_CONFIG" ]; then
  if jq empty "$COPILOT_MCP_CONFIG" 2>/dev/null; then
    # Deep-merge new servers into existing config (preserves user-added servers)
    echo "$MCP_SERVERS" | jq -s '.[0] * .[1]' "$COPILOT_MCP_CONFIG" - > "${COPILOT_MCP_CONFIG}.tmp" \
      && mv "${COPILOT_MCP_CONFIG}.tmp" "$COPILOT_MCP_CONFIG"
  else
    echo "Warning: $COPILOT_MCP_CONFIG contains invalid JSON. Backing up to ${COPILOT_MCP_CONFIG}.bak" >&2
    cp "$COPILOT_MCP_CONFIG" "${COPILOT_MCP_CONFIG}.bak"
    echo "$MCP_SERVERS" | jq . > "$COPILOT_MCP_CONFIG"
  fi
else
  echo "$MCP_SERVERS" | jq . > "$COPILOT_MCP_CONFIG"
fi

echo "Copilot MCP registration complete. Config written to $COPILOT_MCP_CONFIG."

# ---------------------------------------------------------------------------
# Claude Code status line
# ---------------------------------------------------------------------------
cat > "$HOME/.claude/statusline.sh" << 'STATUSLINE'
#!/bin/bash
export LANG=C.UTF-8
input=$(cat)

MODEL=$(echo "$input" | jq -r '.model.display_name')
DIR=$(echo "$input" | jq -r '.workspace.current_dir')
COST=$(echo "$input" | jq -r '.cost.total_cost_usd // 0')
PCT=$(echo "$input" | jq -r '.context_window.used_percentage // 0' | cut -d. -f1)
DURATION_MS=$(echo "$input" | jq -r '.cost.total_duration_ms // 0')

CYAN='\033[36m'; GREEN='\033[32m'; YELLOW='\033[33m'; RED='\033[31m'; RESET='\033[0m'

# Pick bar color based on context usage
if [ "$PCT" -ge 90 ]; then BAR_COLOR="$RED"
elif [ "$PCT" -ge 70 ]; then BAR_COLOR="$YELLOW"
else BAR_COLOR="$GREEN"; fi

FILLED=$((PCT / 10)); EMPTY=$((10 - FILLED))
BAR=$(printf "%${FILLED}s" | tr ' ' '=')$(printf "%${EMPTY}s" | tr ' ' '-')

MINS=$((DURATION_MS / 60000)); SECS=$(((DURATION_MS % 60000) / 1000))

BRANCH=""
git rev-parse --git-dir > /dev/null 2>&1 && BRANCH=" | 🌿 $(git branch --show-current 2>/dev/null)"

echo -e "${CYAN}[$MODEL]${RESET} 📁 ${DIR##*/}$BRANCH"
COST_FMT=$(printf '$%.2f' "$COST")
echo -e "${BAR_COLOR}${BAR}${RESET} ${PCT}% | ${YELLOW}${COST_FMT}${RESET} | ⏱️ ${MINS}m ${SECS}s"
STATUSLINE
chmod +x "$HOME/.claude/statusline.sh"

# Deep-merge statusLine config into Claude settings (preserves existing nested keys)
CLAUDE_SETTINGS="$HOME/.claude/settings.json"
STATUSLINE_CFG='{"skipDangerousModePermissionPrompt":true,"permissions":{"defaultMode":"bypassPermissions"},"statusLine":{"type":"command","command":"~/.claude/statusline.sh","padding":2}}'
if [ -f "$CLAUDE_SETTINGS" ]; then
  if jq empty "$CLAUDE_SETTINGS" 2>/dev/null; then
    jq ". * $STATUSLINE_CFG" "$CLAUDE_SETTINGS" > "${CLAUDE_SETTINGS}.tmp" && mv "${CLAUDE_SETTINGS}.tmp" "$CLAUDE_SETTINGS"
  else
    echo "Warning: $CLAUDE_SETTINGS contains invalid JSON. Backing up to ${CLAUDE_SETTINGS}.bak" >&2
    cp "$CLAUDE_SETTINGS" "${CLAUDE_SETTINGS}.bak"
    echo "$STATUSLINE_CFG" | jq . > "$CLAUDE_SETTINGS"
  fi
else
  echo "$STATUSLINE_CFG" | jq . > "$CLAUDE_SETTINGS"
fi

# ---------------------------------------------------------------------------
# VS Code extensions — synchronous install before the workspace activates.
#
# Why here (postStartCommand) instead of postAttachCommand: waitFor in
# devcontainer.json is set to "postStartCommand", so VS Code blocks workspace
# activation until this script returns. Installing extensions at postAttach
# races with VS Code's workspace-recommendation check, which fires the
# "Do you want to install the recommended 'Uno Platform' extension..." prompt
# on every rebuild because unoplatform.vscode isn't yet present when the
# check runs. By installing synchronously here, every extension declared in
# customizations.vscode.extensions is on disk before the workspace activates,
# and the recommendation check passes silently.
#
# devcontainer.json is parsed (rather than hardcoding the list) so this stays
# in sync if the extensions array changes.
# ---------------------------------------------------------------------------
# `code` is provided by VS Code Server, which the Dev Containers extension
# installs shortly after the container starts — it may not be on PATH the
# moment this script begins. Poll briefly before giving up.
for _ in $(seq 1 30); do
  if command -v code >/dev/null 2>&1; then
    break
  fi
  sleep 1
done

if ! command -v code >/dev/null 2>&1; then
  echo "Note: VS Code CLI 'code' not available after 30s; skipping extension pre-install."
  echo "      VS Code's own auto-install from devcontainer.json will still run, but the"
  echo "      workspace-recommendation prompt may fire on first attach."
else
  devcontainer_json="/uno-themes/.devcontainer/devcontainer.json"

  mapfile -t required_extensions < <(
    awk '
      /"extensions"[[:space:]]*:[[:space:]]*\[/ { in_extensions = 1; next }
      in_extensions && /\]/ { in_extensions = 0; next }
      in_extensions {
        if ($0 ~ /"/) {
          line = $0
          sub(/^[^"]*"/, "", line)
          sub(/".*$/, "", line)
          if (line != "") {
            print line
          }
        }
      }
    ' "$devcontainer_json"
  )

  if [ "${#required_extensions[@]}" -eq 0 ]; then
    echo "Note: no extensions declared in $devcontainer_json"
  else
    installed_extensions="$(code --list-extensions 2>/dev/null || true)"
    for extension_id in "${required_extensions[@]}"; do
      if ! grep -Fxiq "$extension_id" <<<"$installed_extensions"; then
        echo "Installing VS Code extension: $extension_id"
        code --install-extension "$extension_id" --force \
          || echo "Warning: failed to install extension $extension_id"
      fi
    done
  fi

  # ---------------------------------------------------------------------
  # Workaround for unoplatform.vscode not activating after auto-install.
  #
  # When VS Code's Dev Containers extension installs unoplatform.vscode
  # via customizations.vscode.extensions, the install completes and the
  # extension appears in `code --list-extensions`, but it never activates
  # (no _doActivateExtension log entry, no globalStorage created), even
  # after its dependencies (ms-dotnettools.csharp, csdevkit) are active
  # and the workspaceContains pattern matches. Symptom: extension shows
  # grayed out in the Extensions view, no commands in the palette.
  #
  # The auto-install path writes `isApplicationScoped: true` into the
  # extension metadata. Reinstalling via `code --install-extension`
  # produces working state (isMachineScoped: true instead) and triggers
  # proper activation. Other auto-installed extensions also have
  # isApplicationScoped: true but activate normally — the interaction
  # is specific to this extension.
  #
  # Detect the bad state and force a clean reinstall.
  extensions_json="/home/developer/.vscode-server/extensions/extensions.json"
  if [ -f "$extensions_json" ] && command -v jq >/dev/null 2>&1; then
    # Detection is best-effort: if VS Code is mid-rewrite of extensions.json
    # (truncated/invalid JSON) jq exits non-zero and `set -e` would abort the
    # whole post-start. Swallow the failure and treat the state as unknown.
    uno_bad_state=$(jq -r '
      .[] | select(.identifier.id == "unoplatform.vscode")
          | .metadata.isApplicationScoped // false
    ' "$extensions_json" 2>/dev/null || echo "")
    if [ "$uno_bad_state" = "true" ]; then
      echo "Detected unoplatform.vscode installed via Dev Containers auto-install"
      echo "(isApplicationScoped:true blocks activation). Force-reinstalling..."
      code --uninstall-extension unoplatform.vscode 2>/dev/null || true
      code --install-extension unoplatform.vscode --force 2>/dev/null \
        || echo "Warning: force-reinstall of unoplatform.vscode failed"
    fi
  fi
fi
