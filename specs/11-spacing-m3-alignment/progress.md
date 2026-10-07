# Spacing scale on Material 3's 8dp base (issue #1746)

Implements https://github.com/unoplatform/Uno.Themes/issues/1746.

## Design

Material 3 defines spacing as a linear scale on an 8dp base (`md.sys.measurement.space100` = 8dp),
used only for padding, gaps and margins. Component sizes are separate, fixed component tokens.

- `DefaultSpacing` defaults to 8, so `Space100` = 8 as in M3. `Space{n}` stays
  `DefaultSpacing × n / 100`.
- Every Simple and Material v2 style reference moves to the half step (`Space400` → `Space200`),
  so the rendering at the default is unchanged.
- The multiplier table is M3's steps (0–9×, plus 0.25×, 0.5×, 0.75×, 1.25×) and four extensions
  (1.5×, 2.5×, 12×, 20×) so every value of the previous 4-based scale still has a token.
  `Space1600`, `Space2400` and `Space4000` are removed (breaking).
- Density factors are unchanged; with base 8 they give 6 / 8 / 10, the same pixels as before.
- Spacing tokens no longer set sizes. The 50 sites that did (selection boxes, slider parts, switch
  thumbs, pips, progress heights, the Material AppBarButton, menu and badge icon slots) use fixed
  tokens or values equal to their previous default.
- Controls with a fixed design size in M3 keep their default padding at any spacing: FABs, icon
  buttons, AppBarButton padding, drawer items and section header, menu check slots, PipsPager
  navigation buttons and ContentDialog panel padding.
- Every change is a token or value mapping. No control template changes its layout: an earlier
  attempt that moved ContentDialog's max size onto its content altered long-content dialogs at the
  default and was reverted (see `specs/lessons.md`).

## Checklist

- [x] `DefaultBaseSpacing` 4 → 8 and the new multiplier table (`BaseTheme.cs`, `BaseTheme.ScaleGeneration.cs`)
- [x] Half-step remap of every `Space*` / `SimpleSpace*` reference; `SimpleSpace*` aliases cover the whole scale
- [x] Spacing-driven sizes moved to fixed tokens or values
- [x] Fixed-size controls keep their default padding
- [x] Runtime tests: base-8 expectations, new M3 steps, and `When_DefaultSpacingChanges_Then_ControlSizeKeysStayFixed` in both heads
- [x] Docs: scale table, migration table, density table, getting-started and Simple per-control tables
- [x] Spacing Styles sample page (`SamplesApp.Shared/Content/Styles/SpacingStylesSamplePage.xaml`)

## Review

Verification (net10.0-desktop Release, Linux Skia, 2026-10-07):

- Captured the Spacing Styles page and a ContentDialog from master and from this branch with the
  same sample page. At the default (4 on master, 8 here) both themes are byte-identical. Two
  captures of one build were identical, so the comparison is exact.
- The branch at 2× a value matches master at that value; above the default it is shorter because
  fixed-size controls no longer grow (Simple at 32: 2,821 px vs 4,928 px on master).
- Runtime tests: Simple 292 passed, 1 skipped (pre-existing `[Ignore]`); Material 101 passed.
- Docs: no new markdownlint findings against master; cspell clean.

Known glitches left for follow-up because the fixes are token-only (listed in #1746):

- ContentDialog keeps its 560 px frame cap; at very large spacing the growing button row squeezes
  the body out (Simple at 64).
- Material outlined TextBox: vertical padding positions the text, so it stays a spacing token and
  the box grows taller at large values.
- Simple text buttons pad on all four sides with spacing, so they grow tall at large values.
- Simple AutoSuggestBox delete/query buttons are a fixed 32×28 with spacing padding; their glyphs
  may clip at large values (not observed up to 32).
- Unrelated to spacing, present on master at the default: the top-pane NavigationView's selected
  item renders blank, Material PipsPager shows one pip, Simple DatePicker renders an empty field.
