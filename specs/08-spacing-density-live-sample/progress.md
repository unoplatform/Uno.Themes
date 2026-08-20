# Live spacing & density sample page (stacked on #1701)

Stacked PR on `dev/sb/default-spacing` (PR #1701 — `DefaultSpacing` base unit + Density as a
pure mode). Adds the interactive counterpart to the Seed Color sample: slide the base spacing
unit and switch density modes, and watch the spacing tokens and the styled controls of the
**whole running app** update live — global, like the seed color picker, per explicit user
request (an earlier scoped-sandbox design was rejected: "i want the slider and selections to
affect the entire app at runtime").

## Design

Why this needs more than setting the DPs: `DefaultSpacing` / `DefaultDensity` changes on the
running theme regenerate the raw `Space*` token resources, but the per-control alias keys
(`ButtonPadding` → `Space400HorizontalThickness` StaticResource aliases in the control-style
layer) materialize lazily on first lookup and are then cached for the dictionary's lifetime —
and rendered controls only re-evaluate `{ThemeResource}` bindings on a theme-change pass. The
color picker gets live updates for free because semantic brushes are singleton instances
mutated in place; spacing tokens are `double`/`Thickness` values with no instance to mutate.

The page therefore applies knob changes to the running app theme in three steps
(`ApplyToTheme` in the page code-behind — sample-level only, no library changes):

1. **Reload the theme's static control-style layer** in place
   (`theme.Source = new Uri(theme.Source.OriginalString)`) so the per-control alias keys
   re-materialize lazily against the new scale. The theme instance keeps its `Colors` /
   font configuration — nothing to copy.
2. **Set `DefaultSpacing` / `DefaultDensity`** — the property-changed callbacks rebuild the
   dynamic token layers via `UpdateSource()`. (Guarded: the reload only happens when at least
   one DP will actually change, so `UpdateSource()` is guaranteed to run after the reload.)
3. **Force a theme-change pass** (flip the root's `RequestedTheme` to the opposite of
   `ActualTheme` and restore it, synchronously) so every live `{ThemeResource}` binding —
   style setters like `ButtonPadding`, the page's token bars — re-resolves app-wide. Same
   machinery as a dark/light switch; both writes happen in one dispatcher frame, so no flash.

Fallback if the in-place `Source` reload turns out not to refresh the alias cache (empirically
verified by the mechanism runtime tests below): swap a fresh theme instance of the same runtime
type into `Application.Resources.MergedDictionaries` (fresh-instance re-resolution is already
proven by the pre-existing §8 visual tests), copying `Colors` and override sources.

Known limitation, documented on the page: values baked with `{StaticResource Space*}` *inside*
control templates (a handful of margins in NavigationView/Flyout/etc.) refresh only when the
template re-applies — the same coverage a dark/light switch achieves.

The slider snaps to 0.5-px steps, so a full drag produces at most ~24 apply passes.
The knobs initialize from the running theme's current values, so page state survives
navigation with no static bookkeeping.

## Checklist

- [x] Mechanism runtime tests (Material head `Given_DesignTokens`):
  - [x] fresh-theme swap + `RequestedTheme` flip restyles an already-rendered button
  - [x] in-place `Source` reload + DP change + flip restyles an already-rendered button
- [x] `SpacingDensitySamplePage.xaml(.cs)` under `src/samples/SamplesApp.Shared/Content/Styles/`
      (Material + Simple designs, global apply via the running theme)
- [x] Registered in `SamplesApp.Shared.projitems`
- [x] Material-head runtime test: composition on a rendered control —
      `DefaultSpacing=6` × `Compact` → `FilledButtonStyle` Padding `(18,0,18,0)`
- [x] Material-head runtime test: end-to-end page test — drive the slider/density combo,
      assert the app theme carries the values and the rendered preview button restyled live
      (restores the global theme in `finally`)
- [x] `doc/design-tokens.md`: pointer to the interactive sample
- [x] Build clean (desktop TFM, Material + Simple heads); runtime tests green headlessly

## Review

Implemented as designed — sample-level only, driven entirely through existing public API
(`SemanticThemeHelper.GetTheme()`, `ResourceDictionary.Source`, `DefaultSpacing`,
`DefaultDensity`, `RequestedTheme`). Both candidate mechanisms were verified empirically
before choosing; the page uses the in-place `Source` reload (keeps `Colors`/font
configuration on the instance — nothing to copy), and the fresh-instance swap test remains
as documentation of the fallback recipe.

Verification (net10.0-desktop Debug, headless `--runtime-tests` runs on Windows, 2026-08-20):
- Material head, `Given_DesignTokens` filter: **36 cases, 0 failed**, including the four new
  tests — `When_FilledButton_WithCustomSpacingAndCompactDensity_Then_PaddingComposes`,
  `When_ThemeSwappedAndThemeFlipped_Then_RenderedButtonRestyles`,
  `When_SourceReloadedAfterSpacingChangeAndThemeFlipped_Then_RenderedButtonRestyles`, and the
  end-to-end `When_SpacingDensitySamplePage_KnobsChanged_Then_AppThemeRestylesLive` (loads the
  real page, drives `SpacingSlider`/`DensityCombo`, asserts the app theme carries 8/Compact and
  the already-rendered preview button restyled to `(24,0,24,0)`, restores the theme in
  `finally`).
- Simple head, full suite: **171 cases, 0 failed** — the shared page compiles and registers
  under the Simple head with no regressions.
- Both heads build with 0 errors; the only warnings are the pre-existing shared-project
  nullable warnings also present on the base branch (none from the added files — the one new
  CS8600 that appeared in an intermediate build was fixed by pattern-matching the
  `Activator.CreateInstance` result before it was ever committed, and that code path was later
  replaced by the in-place reload anyway).

Empirical findings worth keeping (they refine the base PR's construction-time story):
- A `RequestedTheme` flip (opposite of `ActualTheme`, then restore, synchronously) re-resolves
  `{ThemeResource}` style-setter bindings of already-rendered controls — including
  `Thickness`-valued ones like `ButtonPadding` — against the *current* resource graph, so a
  swapped or reloaded theme takes effect without recreating content.
- Re-setting `ResourceDictionary.Source` to the same URI on a live `BaseTheme` re-creates the
  static style layer with fresh lazy alias entries (it is not a same-value no-op), and the
  theme's own `UpdateSource()` afterwards re-attaches the dynamic layers cleanly. The reload
  must only be done when a subsequent DP change is guaranteed (the page guards this), since the
  dynamic layers are gone until `UpdateSource()` runs.
