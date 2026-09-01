# Single root typeface: `DefaultFontFamily` token replaces the plain/brand pair

> Bottom layer of a two-PR stack. The `BaseTheme.DefaultFontFamily` property seam (Hot Design's
> editing surface, plus the #1705 font-override re-resolution fix) stacks on top in PR #1707
> (`specs/09-default-font-family/`). This layer is tokens, XAML, and docs only — no library C#.

Decision (Steve, 2026-08-31): drop the M3-style two-typeface token model (`TypefacePlain` /
`TypefaceBrand`, shipped 7.1.1) in favor of **one root typeface**, with per-scale variation
expressed through the existing per-slot `*FontWeight` tokens — which the semantic TextBlock styles
already apply (`Uno.Simple.WinUI/Styles/Controls/TextBlock.xaml`,
`Uno.Material/Styles/Controls/v2/TextBlock.xaml`).

## Why one typeface instead of the M3 plain/brand pair

- The shipped themes never used two families: Material was Roboto Regular vs Roboto **Medium**,
  Simple was Inter Regular vs Inter **Medium** — the "two typefaces" differed only by weight, and
  the weight nuance was *also* already carried by the per-scale `*FontWeight` tokens. The pair was
  an artifact of baking weights into separate font files.
- Weights belong to the `FontWeight` axis. A single `FontFamily` reference resolves real weights
  through the **font manifest** (`.ttf.manifest`, Uno 5.3+) on Skia/iOS and through the variable
  font itself on native Windows/Android/WASM. Variable fonts alone are *not* enough today: Uno's
  Skia text stack loads file fonts with a bare `SKTypeface.FromStream()` (no variation axes;
  SkiaSharp's fix landed only in 4.147.0-preview while Uno pins 3.119, `uno#13862` open), which is
  exactly what the manifest works around.
- `Uno.Fonts.Inter` / `Uno.Fonts.Roboto` shipped per-weight statics only.
  **unoplatform/uno.fonts#75** adds the variable `Inter.ttf` / `Roboto.ttf` entry points (family
  names verified as "Inter"/"Roboto", full 100–900 `wght` axes) plus manifests mapping
  400/500/600/700 (Inter) and 300/400/500 (Roboto) to the existing statics — the
  `Uno.Fonts.OpenSans` model. **This layer depends on that publish**; until then a local feed
  override in `NuGet.Config` + `Directory.Packages.props` (version `99.0.0-local.1`, uncommitted)
  stands in, and merge is gated on re-pinning to the published versions.

## Breaking change (deliberate, `feat!`)

Removed outright (no deprecated aliases — an alias that no longer feeds the scales would resolve
but silently do nothing, worse than a clean break):

- Tokens `TypefacePlain`, `TypefaceBrand` → replaced by `DefaultFontFamily`.
- Simple's per-weight `SimpleRegular/Medium/SemiBold/BoldFontFamily` keys → per-scale weight lives
  in the `*FontWeight` tokens; families all derive from the root.
- Legacy keys kept: `MaterialLight/Medium/RegularFontFamily` (still point at statics),
  `SimpleFontFamily`, `CupertinoFontFamily` (now aliases of `DefaultFontFamily`).
- Consequence recorded in `doc/material-migration.md`: overriding `MaterialRegular/MediumFontFamily`
  no longer reaches the type scales.

## Changes in this layer

- `SharedTypography.xaml`: single `DefaultFontFamily` root token (Segoe UI baseline); all 19
  `*FontFamily` slot aliases re-pointed at it; per-slot weight/size/spacing tokens unchanged.
- Material `Fonts.xaml` → `Roboto.ttf#Roboto` entry point; v2 `Typography.xaml` slot aliases
  re-pointed. Simple `Fonts.xaml` collapsed to the root + legacy alias (per-weight keys and the
  19 slot re-declarations deleted); `Typography.xaml` aliases re-pointed — its weight tokens
  already carried the Bold/SemiBold/Normal nuance. Simple `Button`/`ToggleButton` lightweight keys
  re-pointed. Cupertino gains the root token, `CupertinoFontFamily` kept as alias.
- `DesignTokensSamplePage`: the two token showcase TextBlocks become one.
- `Given_Fonts` rewritten to pin the new contract: root + legacy key + all 19 slots resolve to the
  Inter entry point, and the weight tokens carry the per-scale nuance. Its 19-slot assertion is
  the guard for the eager-alias-resolution trap recorded in `specs/lessons.md`.
- `doc/design-tokens.md` (token table, breaking-change callout, token-only font swap) and
  `doc/material-migration.md`.

## Review-panel fixes (2026-09-01)

- Material v2 control styles re-pointed at the root (12 per-control aliases + 2 direct refs were
  still on the legacy weight-baked keys; every site already carries an explicit `FontWeight`).
- Simple `Button`/`ToggleButton` weight parity restored: the old family was Inter-Medium-baked, so
  the weight keys are now `Medium` and the setters route through them (`SimpleToggleButtonFontWeight`
  is new).
- Pre-existing dangling tokens fixed: `SimpleRegularFontWeight`/`SimpleSemiBoldFontWeight` were
  referenced by Expander/CalendarView but never defined; now declared in Simple `Typography.xaml`.
- Docs de-drifted: `material-getting-started.md` font section rewritten around `DefaultFontFamily`,
  `semantic-styles.md` gained the root-token paragraph, `doc/styles/TextBlock.md` refreshed.
- Tests: Light+Dark appearance guard, and a rendered-weight guard (Bold vs Normal widths must
  differ from the single Inter reference) that turns the uno.fonts#75 merge gate into a red test.

## Verification

Simple host, `net10.0-desktop` Debug, headless `--runtime-tests`, against local `99.0.0-local.1`
font packages: full suite green as part of the stacked verification (209 passed / 1 pre-existing
skip across both layers; this layer's `Given_Fonts` is 30 cases). Libraries build with zero
errors and no warnings in changed files.

## Open items

- [ ] unoplatform/uno.fonts#75 merged + packages published; `Directory.Packages.props` re-pinned
      (blocks merge)
- [ ] Stacked PR #1707 (`DefaultFontFamily` property seam) adapted on top of this layer
