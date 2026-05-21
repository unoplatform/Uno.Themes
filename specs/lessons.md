# Agent lessons — Uno.Themes

Capture every user correction here so future agents working on this repo do
not repeat the same mistake. Per `AGENTS.md` §3, this file is the
domain-lessons / postmortems record. Add an entry whenever a user has to tell
you something you should have known.

---

## L1 — Edit the `artifacts/uno.themes` checkout when working from Studio Live, not `X:\src\uno.themes`

**Date:** 2026-05-20

**What happened.** The user asked for diagnostic logging in
`Uno.Themes.BaseTheme`, `BaseTheme.SeedColors.cs`, `ThemeColors.cs`, and
`Uno.Simple.WinUI/SimpleTheme.cs` to debug Studio Live Hot Reload. There are
two distinct working copies of this repo on disk:

- `X:\src\uno.themes\` — a generic developer clone of `unoplatform/uno.themes`.
- `X:\src\studio.live\artifacts\uno.themes\` — the Studio Live work checkout, on
  branch `dev/sb/hr-fixes`, wired into Studio Live's local NuGet override flow
  so changes flow into Studio Live builds.

The agent edited `X:\src\uno.themes\` (the wrong one). Studio Live did not see
the changes, and the work had to be reverted and re-applied in the artifacts
checkout. ~10 minutes lost.

**Why it matters.** Files exist at the same relative path in both checkouts,
the C# is byte-identical at the baseline, and both build successfully — so
there is no compile-time signal that you are in the wrong tree. The only
signal is that the Studio Live local NuGet cache override is wired to the
artifacts checkout. Building from `X:\src\uno.themes` *also* produces an
`OVERRIDING NUGET PACKAGE CACHE` log line because it shares the same package
override mechanism, so even the build log can mislead. The mistake is silent
until the user runs the inner app and sees no `[ThemeHR]` logs.

**How to apply.**

1. **Before editing any file under `library/Uno.Themes`, `library/Uno.Simple.WinUI`,
   `library/Uno.Material*`, `library/Uno.Cupertino*`, etc., always verify the path
   prefix is `X:\src\studio.live\artifacts\uno.themes\...` if the task is part
   of a Studio Live session.** Whenever the user references Studio Live, inner-apps,
   `<utus:SimpleToolkitTheme>`, hot reload of inner-app themes, or feedback bundles,
   the implied checkout is the `artifacts/` one.
2. If the user provides repo file paths *without* the `artifacts/` prefix, ask
   "to confirm — is this against the `artifacts/uno.themes` checkout?" before
   editing. Cost of one clarifying question << cost of redoing a multi-file
   edit pass.
3. The two checkouts can drift. **Do not assume baseline parity.** Always
   `Read` the artifacts file before editing it, even if you have already read
   the same file at the global path. The MCP tool harness enforces a
   read-before-edit gate per absolute path anyway, so this is a free check.
4. When the task crosses into `uno.toolkit.ui` as well, the analogous
   correct checkout is `X:\src\studio.live\artifacts\uno.toolkit.ui`. Same
   reasoning applies.

**Detection heuristic for future you.** If you have just built in
`X:\src\uno.themes` *and* Studio Live is involved, ask the user before
considering the task done — the build succeeding there does not mean
Studio Live picked up the change.
