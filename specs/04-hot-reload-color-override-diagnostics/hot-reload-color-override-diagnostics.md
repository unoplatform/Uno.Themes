# 04 — Hot Reload color-override diagnostics

## Context

Studio Live inner-apps that consume `Uno.Toolkit.WinUI.Simple`
(`SimpleToolkitTheme`) and edit color overrides at design time via Hot Reload
are reporting that the rendered colors do not update — the XAML change applies
but the UI keeps the previous palette. Symptoms point at the override pipeline
in `Uno.Themes.BaseTheme` (which owns `ColorOverrideDictionary`,
`Colors.OverrideDictionary`, and the brush-instance-preservation logic that
lets already-rendered controls pick up new colors without re-layout).

Before changing any behavior we need ground-truth telemetry from a running app
to tell which branch is firing on a Hot Reload and which is silently skipped.
This spec covers the **diagnostic logging pass only**. Once we have feedback
bundles with the new `[ThemeHR]` lines we can target the real fix in a
follow-up spec.

This is the `uno.themes` side of the work. The companion spec in
`artifacts/uno.toolkit.ui/specs/hot-reload-color-override-diagnostics/` covers
the `SimpleToolkitTheme` entry-point logging.

## Scope (this PR)

Add structured `[ThemeHR]`-prefixed `LogInformation` calls at every callable
entry point in the Hot-Reload-relevant theme pipeline:

| File | Logged sites |
| --- | --- |
| `src/library/Uno.Themes/BaseTheme.cs` | ctor, `OnFontOverrideSourceChanged`, `OnColorOverrideSourceChanged`, `OnFontOverrideChanged` (incl. muted path), `OnColorOverrideChanged` (incl. re-entrant suppression), `OnColorsChanged` (incl. callback firings via `SetChangedCallback`), `OnDefaultCornerRadiusChanged`, `OnDefaultDensityChanged`, `UpdateSource` (ENTER/EXIT, initial brush capture, `addedFromSelf` / `addedFromApp`, `UpdateOldBrushes` invocation vs. skip) |
| `src/library/Uno.Themes/BaseTheme.SeedColors.cs` | `UpdateSeedColors` (ENTER/EXIT, both fall-back-to-`UpdateSource` branches), `UpdateBrushEntriesInPlace` (count of patched brushes), `UpdateOldBrushes` (themed/fallback/unresolved counts), `IsInResourceTree` |
| `src/library/Uno.Themes/ThemeColors.cs` | `OnOverrideSourceChanged`, `OnPropertyChanged` (with DP-name disambiguation) |
| `src/library/Uno.Simple.WinUI/SimpleTheme.cs` | `GetSimpleColorOverride` merge, `GenerateSpecificResources` |

All call sites are guarded by `if (_log.IsEnabled(LogLevel.Information))` so
they are free when logging is disabled. Logger is a `static readonly ILogger`
field per type using the existing `Uno.Logging.Log()` extension (already a
transitive dependency via `Uno.Core.Extensions.Logging.Singleton` declared in
`themes-common.props`).

No behavior changes. No new public surface. No DP wiring changed.

## Why these specific sites

Three hypotheses for the observed bug, and what each log line is designed to
disprove or confirm:

1. **Hot Reload mutates inner dictionary content without re-setting the
   `ColorOverrideDictionary` DP itself.** If true, neither
   `OnColorOverrideChanged` nor `ThemeColors.OnPropertyChanged` will fire on
   reload — only the inner `Color` resources change, and nothing rebuilds the
   brushes. Confirmed by **absence** of the DP-change log lines on reload.
2. **`_isInResourceTree` is still `false` when the first DP change arrives**,
   so `_originalBrushes` never gets captured, so `UpdateOldBrushes` is skipped
   and UI elements keep their stale brush instances. Confirmed by the
   `UpdateSource ... SKIPPING initial brush capture` log on the reload tick.
3. **Re-entrant suppression (`_isUpdatingColorOverrides`) eats the change.**
   The deprecated `ColorOverrideDictionary` DP and the newer
   `Colors.OverrideDictionary` path both flip this flag, and the `ThemeColors`
   `SetChangedCallback` returns early when it is set. Confirmed by the
   `SUPPRESSED (re-entrant ...)` log.

## How to read the logs

Filter Studio Live feedback bundles (`logs.json`) for the literal `[ThemeHR]`.
Expected golden-path sequence on a Hot Reload that changes a single
`<Color x:Key="PrimaryColor">` inside the `Default` themed dict of
`SimpleToolkitTheme.ColorOverrideDictionary`:

```
[ThemeHR] OnColorOverrideChanged (deprecated DP) on SimpleToolkitTheme: oldEntries=..., newEntries=...
[ThemeHR] ThemeColors property changed: OverrideDictionary, isStructural=True, hasCallback=True, ...
[ThemeHR] ThemeColors callback fired on SimpleToolkitTheme: isStructural=True, isUpdatingOverrides=True   <-- expected: callback bails because the outer DP routes already call UpdateSource
[ThemeHR] UpdateSource ENTER on SimpleToolkitTheme: isInResourceTree=True, originalBrushes=N, ...
[ThemeHR] UpdateSource on SimpleToolkitTheme: post-rebuild brush tracking — added X from theme tree, Y from app resources, total tracked=...
[ThemeHR] UpdateOldBrushes: total=..., themedPatches=..., fallbackPatches=..., unresolved=...
[ThemeHR] UpdateSource EXIT on SimpleToolkitTheme
```

If any line above is missing on a reload tick, that is the bug — the spec for
the actual fix gets written against whichever line went missing.

## Out of scope

- Fixing the bug. This pass is observation only.
- Adding tests. Diagnostic logging is verified by running an app and reading
  the log; there is no observable behavior to assert against. Per AGENTS.md §5,
  the eventual *fix* will require a red/fix/green runtime test under
  `src/samples/SimpleSampleApp/RuntimeTests/`.
- Wiring the logs to a new sink. They go through the standard
  `Microsoft.Extensions.Logging` pipeline; Studio Live already captures
  `Information`+ at the host level.

## Done when

- [x] Logs added to all sites in the scope table.
- [x] `dotnet build src/library/Uno.Themes/Uno.Themes.WinUI.csproj -c Debug` clean.
- [x] `dotnet build src/library/Uno.Simple.WinUI/Uno.Simple.WinUI.csproj -c Debug` clean.
- [x] Local NuGet cache override engaged (build log shows `OVERRIDING NUGET PACKAGE CACHE`).
- [ ] Studio Live reproduces the issue, the `[ThemeHR]` sequence is captured in a feedback bundle, and a follow-up spec is opened against the first missing/unexpected line.

## Follow-up

Once a real `logs.json` is in hand, open `specs/05-color-override-hot-reload-fix/`
with the specific failing branch identified and a targeted behavior change
plus a `Given_*` runtime test that reproduces the regression.
