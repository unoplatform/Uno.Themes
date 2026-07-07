---
description: Run the seven reviewer subagents — architect, security, skeptic, quality, operability, contract, performance — in parallel against a scope (defaults to uncommitted changes; falls back to current branch vs master).
argument-hint: [scope — e.g. HEAD~1, master..HEAD, a PR number, or free-form; omit for auto]
allowed-tools: Agent, Bash(git status:*), Bash(git diff:*), Bash(git log:*), Bash(git rev-parse:*), Bash(git merge-base:*), Bash(git branch:*), Bash(git ls-files:*), Bash(gh pr view:*), Bash(gh pr diff:*)
---

# Review Panel Command

You are orchestrating a seven-agent review panel on this repo. Do not do the review yourself — your job is to pin down the scope, dispatch all seven reviewer subagents in parallel, then synthesize their findings into one consolidated report.

## 1. Resolve scope

Argument from the user: `$ARGUMENTS`

- If the argument is non-empty, use it as the scope verbatim. If it looks like a PR reference (`#123`, `PR 123`, a github.com PR URL), resolve the diff via `gh pr diff` / `gh pr view`. Otherwise treat it as a git revision range (e.g. `HEAD~1`, `master..HEAD`) or a free-form description.
- If the argument is empty, auto-resolve in this order:
  1. Run `git status --porcelain`. If there are any staged, unstaged, or untracked changes, the scope is **"uncommitted changes (working tree + index + untracked)"**.
  2. Otherwise run `git rev-parse --abbrev-ref HEAD`. If the current branch is not `master` or `main`, the scope is **"current branch vs master"** (this repo's default branch is `master`). Resolve the base commit with `git merge-base --fork-point master HEAD` and capture the output as `<base>`; the scope range is then the explicit `<base>..HEAD`. If `--fork-point` exits with empty stdout (typical after a rebase or across divergent histories), fall back to the literal range `master..HEAD`. `git merge-base` returns a single commit SHA, not a range — never pass it alone to `git diff` / `git log` (that would include unrelated ancestor history); always compose the two-dot range form before diffing.
  3. Otherwise stop with a one-line message: "Nothing to review — clean working tree on master. Pass a scope argument."

## 2. Gather scope detail

Collect — but do not analyze — the concrete change surface so each reviewer sees the same brief:

- File list with stat summary (`git diff --stat <range>` or `git status --porcelain` + untracked)
- Commit list if a range is in play (`git log --oneline <range>`)
- Full commit messages if a range is in play (`git log --format="--- commit %H%n%s%n%n%b" <range>`) — pass these verbatim to `quality` and `contract` so they can evaluate requirement alignment and intentional vs accidental contract changes
- Branch name and base

Keep this under ~30 lines for the file list; truncate with a note if the diff is huge. Do not paste the full diff into the panel prompt — each reviewer has `Read` / `Grep` / `Glob` and will open files itself.

## 3. Dispatch reviewers in parallel

Send **a single message with seven `Agent` tool calls** — one each to `architect`, `security`, `skeptic`, `quality`, `operability`, `contract`, `performance` — running concurrently. Each prompt must be self-contained (the subagents see none of this conversation) and must include:

- A one-paragraph scope statement (what's being reviewed, what commits/files, what branch).
- The file list from step 2, verbatim.
- The commit messages and problem statement verbatim — all seven agents benefit from understanding intent: `quality` for requirement alignment, `contract` to distinguish intentional vs accidental API / resource-key changes, `operability` and `performance` to know which targets are in scope, `skeptic` to verify the implementation matches the stated goal.
- What has already been verified (usually nothing — say so).
- The role-appropriate questions to focus on. Do not paraphrase the agent's own instructions back at it; the subagent's system prompt already defines its lens.
- An explicit word budget (suggest 400 words each) so the synthesis stays manageable.

Do not omit any of the seven reviewers. The panel is only meaningful when all seven lenses are applied.

## 4. Synthesize

Once all seven return, produce one consolidated report:

- Group by severity: **blocker → high → medium → low → info** (the shared scale across agents). Where a finding appears in multiple reviews, merge it and annotate which reviewers flagged it.
- Resolve `## Hand-off` pointers: if one agent hands off to another and that other agent independently raised the same concern, merge; if it didn't, surface the hand-off as its own finding attributed to the originating agent.
- For each finding keep: severity, category, `file:line`, one-line "what", one-line "why it matters", suggested direction (if any).
- End with a unified verdict — take the worst-case across the seven agent verdicts and map:
  - → **block-merge**: `reject` (skeptic), `needs-redesign` (architect), `block-merge` (security), `needs-rework` (quality / operability / contract / performance)
  - → **fix-first**: `fix-first` (skeptic), `approve-with-changes` (architect / quality / operability / contract / performance), `fix-before-merge` (security)
  - → **ship**: `ship` (skeptic), `no-findings` (security), `approve` (architect / quality / operability / contract / performance)
  - Worst-case wins across all seven.
- If the diff is trivial and all seven reviewers returned a one-line ack, the whole report is a one-line ack — do not pad.

## 5. Offer next steps

After the report, in one line, offer to apply the fixes or stage/commit when the user is ready. Do not apply fixes automatically.
