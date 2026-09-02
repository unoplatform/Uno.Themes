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
- `SimpleFontFamily` → removed as well (round 3): it was a pure alias of the root whose only
  consumer was Simple's `DatePicker.xaml`, now re-pointed at `DefaultFontFamily`.
- Legacy keys kept: `MaterialLight/Medium/RegularFontFamily` (still point at statics, consumed by
  the v1 styles), `CupertinoFontFamily` (alias of `DefaultFontFamily`; Cupertino's controls
  consume it and Cupertino has no semantic type scale).
- Consequence recorded in `doc/material-migration.md`: overriding `MaterialRegular/MediumFontFamily`
  no longer reaches the type scales.

## Changes in this layer

- `SharedTypography.xaml`: single `DefaultFontFamily` root token (Segoe UI baseline); all 19
  `*FontFamily` slot aliases re-pointed at it; per-slot weight/size/spacing tokens unchanged.
  It is the **only** declaration of the slot keys (round 3).
- Material `Fonts.xaml` → `Roboto.ttf#Roboto` entry point; v2 `Typography.xaml` slot aliases
  deleted (round 3; it overrides size/weight/spacing only). Simple `Fonts.xaml` collapsed to the
  root (per-weight keys, `SimpleFontFamily` and the 19 slot re-declarations deleted);
  `Typography.xaml` slot aliases deleted — its weight tokens already carried the
  Bold/SemiBold/Normal nuance. Simple `Button`/`ToggleButton`/`DatePicker` lightweight keys
  re-pointed at the root. Cupertino gains the root token, `CupertinoFontFamily` kept as alias.
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

### Round 2 (seven-lens panel on PR #1710 alone)

- Docs no longer describe the `DefaultFontFamily` *property* (that is #1707): the Material
  getting-started font section and `semantic-styles.md` show the `FontOverrideSource` path only;
  `design-tokens.md` says how to wire the override file and scopes the Cupertino claim
  (Cupertino has no semantic type scale; `CupertinoFontFamily` aliases the root and stays the key
  to override, also noted in `cupertino-getting-started.md`).
- `material-migration.md`: the typography break moved from the v7 section to a v8 section that
  names the removed 7.1.1 tokens and Simple's per-weight keys, lists the re-pointed v2 per-control
  keys, scopes the Material legacy-key note to v2 (v1 unchanged), and records
  `SimpleButtonFontWeight` Normal→Medium plus the new `SimpleToggleButtonFontWeight`.
- `doc/styles/*`: the five re-pointed v2 control rows now read `DefaultFontFamily`; Simple
  `Button.md` weight corrected, `ToggleButton.md` gained the new weight row; `TextBlock.md`
  table re-aligned (the collapse commit had broken `MD060`; `semantic-styles.md` and
  `simple/Button.md` fail `MD060` on master already, untouched).
- `Given_Fonts`: cascade test added at **application** scope
  (`When_RootOverriddenOnApplicationTheme_Then_ScaleFollowsAndClears`: override on
  `Application.Current.GetTheme()` reaches `BodyMediumFontFamily` and a rendered BodyMedium
  TextBlock under Dark and Light, and clears back to Inter). A container-scoped variant was
  written first and **failed**: the slot alias resolves its target against the application
  scope, so a scoped theme's override never reaches it. Recorded in `specs/lessons.md`; #1707's
  generator (concrete `*FontFamily` values, no alias) is the answer for scoped themes.
- `Given_Fonts` width gate made fallback-aware: it now measures the Inter entry point against a
  family known not to exist and fails with "entry point did not load" when both fall back, then
  checks Bold vs Normal. Content assignment moved inside `try`; the appearance-guard comment
  corrected (a dropped entry falls through to the Segoe UI baseline, not to app scope).
- XAML comments: Simple `TextBlock.xaml` header no longer points at the deleted `Fonts.xaml`
  slot block; Simple/Material v2 `Typography.xaml` and `SharedTypography.xaml` say why the slot
  aliases are repeated per theme (self-contained appearance blocks, incl. Material's
  HighContrast); the "previously never defined" history comment dropped.
- `specs/lessons.md` typography entry rewritten for the single-root model (the old text mandated
  the Simple `Fonts.xaml` duplicates this layer deletes).
- `MaterialSampleApp/MaterialFontsOverride.xaml` deleted: unreferenced, pointed at assets the
  sample does not ship, and overrode only legacy keys that no longer reach the scales.

### Round 3 (2026-09-02, "shouldn't SharedTypography drive all of this?")

- The 19 slot aliases re-declared in Material v2 `Typography.xaml` (×3 appearance blocks) and
  Simple `Typography.xaml` (×2) were pure duplication once every alias targeted the same key:
  the alias target resolves at lookup time against application scope, and a HighContrast miss
  falls through to the next dictionary's `Default` block, so `SharedTypography.xaml`'s aliases
  serve every appearance. Deleted; the per-theme files now override size/weight/spacing only,
  and the round-2 header comments that defended the duplication are retracted.
- `SimpleFontFamily` removed: a design-system-specific key for the root is a dead alias (it
  resolves, overriding it reaches nothing), and its only consumer was `DatePicker.xaml`'s
  `DatePickerFlyoutPresenterFontFamily`, now re-pointed at `DefaultFontFamily`. Test row, docs
  (`design-tokens.md`, `material-migration.md`, `doc/styles/simple/DatePicker.md`) updated.
- Not done here: Cupertino's 21 control sites still consume `CupertinoFontFamily` (the alias),
  and the two hard-coded `SF Pro` keys in `HyperlinkButton.xaml` / `RadioButton.xaml` bypass
  even that. Flipping the controls to `DefaultFontFamily` changes what a `CupertinoFonts`
  `OverrideSource` file must redefine, so it needs its own migration note; separate change.

## Verification

Simple host, `net10.0-desktop` Debug, headless `--runtime-tests`, filter `Given_Fonts`, run at
this layer alone (`dev/sb/1705-1-single-typeface`, no #1707 on top):

| Font packages                              | Result (round 2 → round 3)                                |
| ------------------------------------------ | --------------------------------------------------------- |
| local `99.0.0-local.1` (uno.fonts#75 bits) | 37 / 37 → 36 / 36 passed (the `SimpleFontFamily` row is gone) |
| published pins (`Directory.Packages.props`) | 36 / 37 → 35 / 36; only `When_WeightsDiffer_Then_RenderedWidthsDiffer` fails, on "the Inter entry point did not load" |

`Uno.Material.WinUI` and `SimpleSampleApp` build with zero errors and no warnings in changed
files (desktop TFM). `markdownlint` + `cspell` (CI config) clean on every changed doc page except
the pre-existing `MD060` hits in `semantic-styles.md` / `simple/Button.md`.

### Round 4 (2026-09-02): validated against the uno.fonts#76 CI artifacts

unoplatform/uno.fonts#75 merged on 2026-09-01; its review follow-up **#76** ("trim variable font on
Skia, ship every static weight, pack OFL") is what actually ships the `Inter.ttf` / `Roboto.ttf`
entry points, their `.ttf.manifest` files and all nine static weights (Thin → Black). Its CI
artifacts (`2.10.0-dev.9.g9872bd79f2`, downloaded, served from a scratch folder through
`RestoreAdditionalProjectSources`, pins edited locally and reverted) were used to validate both
heads at the seam tip (`dev/dr/1705-root-typeface-seam` = base + #1707):

| Head (net10.0-desktop Debug, headless)                  | Result                                                       |
| ------------------------------------------------------- | ------------------------------------------------------------ |
| Simple, `Given_Fonts` + `Given_DefaultFontFamily` + `Given_FontFamilyTuner` | 78 / 78 passed, width gate included               |
| Material, new `Given_Fonts` (root + 25 keys + Roboto width gate) | 27 / 27 passed; on the public pins 26 / 27, only the width gate red |

`FontDetailsCache` reported no load failure for any bundled file; the only failing URIs were the
three deliberately nonexistent test fonts. Both manifests map weights 100–900 to the statics, which
closes the "Roboto Bold on Skia" question: `FontWeight="Bold"` now selects `Roboto-Bold.ttf`.

Added `src/samples/MaterialSampleApp/RuntimeTests/Given_Fonts.cs`: the Simple host cannot reference
`Uno.Fonts.Roboto`, so the Material head carries the Roboto entry-point pin, the 19-slot + six
re-pointed per-control key guard, and the rendered-weight gate (AGENTS.md §5 placement exception).

## Open items

- [ ] unoplatform/uno.fonts#76 merged + packages published; `Directory.Packages.props` (library
      and samples) re-pinned to that version (blocks merge; both width gates are red on CI until
      then)
- [ ] Squash note: the BREAKING CHANGE footer should also list `SimpleButtonFontWeight`
      Normal→Medium, the new `SimpleToggleButtonFontWeight` / `SimpleRegularFontWeight` /
      `SimpleSemiBoldFontWeight` keys, `SimpleFontFamily`, and the six re-pointed Material v2
      per-control keys
- [x] Skia + Roboto Bold: resolved by #76's manifest (700 → `Roboto-Bold.ttf`)
- [x] Stacked PR #1707 (`DefaultFontFamily` property seam) rebased on top of this layer
