# `RadiusFull` draws an ellipse, not a pill (issue #1789)

Implements https://github.com/unoplatform/Uno.Themes/issues/1789.

## Finding: WinUI draws the same ellipse (the issue's open question)

Settled from the WinUI source (`microsoft-ui-xaml`), not from a rendered Windows run:

- `CGeometryBuilder::CalculateRoundedCornersRectangle` (`src/dxaml/xcp/core/core/elements/geometry.cpp`)
  resolves an oversized radius **per edge**: when the two radii of an edge overlap, it splits that edge
  between them in proportion. Four equal 9999 radii split the top edge at W/2 and the left edge at H/2,
  so every corner becomes W/2 × H/2.
- The composition clip path (`ClampRadii` in `HWCompNodeWinRT.cpp`) follows "the same policy".

Uno's `CornerRadius.GetRadii` matches WinUI. This is not a Uno parity bug, and no fixed number can mean
"half the shorter side", so the token can't be repaired. Issue items 1 and 3 apply.

## Scope

- [x] Keep `RadiusFull` / `RadiusFullCornerRadius` and Simple's `SimpleRadiusFull*` aliases at 9999. Resource
      keys are public API; the existing `RadiusFull == 9999` tests now guard compatibility.
- [x] Deprecate them in the XML docs (`BaseTheme.cs`, `BaseTheme.ScaleGeneration.cs`), the alias comments
      (`Thickness.xaml`) and the docs. `[Obsolete]` isn't possible: no C# accessor exposes these keys.
- [x] `SimpleIconButtonCornerRadius` (the library's only consumer; also used by `SimpleIconToggleButtonStyle`)
      becomes a literal 20, half the fixed 40px side. It's deliberately not a `CappedRadius`: it was a circle at
      every `DefaultCornerRadius` and stays one (a capped role would go square at 0).
- [x] `doc/design-tokens.md`: `RadiusFull` removed from the scale table; new "Pills and circles" section
      with the rule (pill = half the height, circle = half the side), why, and the deprecation warning.
- [x] `doc/simple-getting-started.md`: alias marked deprecated.
- [x] Design Tokens sample: both "Full" tiles (48 × 48) use a literal 24 and the label "Circle".
- [x] Regenerated `doc/styles/simple/Button.md`.

### Behaviour changes

- Overriding `RadiusFull*` / `SimpleRadiusFull*` no longer reaches the Simple icon buttons.
- An app that resizes a Simple icon button (through its `Min`/`Max` size) must also override
  `SimpleIconButtonCornerRadius`. Enlarged evenly (say 48 × 48), it was a circle and is now a square with 20px
  corners. Widened only, it was an ellipse and is now a pill. The alternative was to keep the deprecated alias
  for this always-square control, which would leave the library using a token its docs tell apps to avoid.

## Verification

- [x] Red: `Given_DesignTokens.When_IconButtonLoaded_Then_RadiusIsHalfItsSide` (6 rows) failed on the unfixed
      tree (resolved 9999, expected 20).
- [x] Green: Simple desktop Debug, full suite 424/424.
- [x] Material desktop Debug, 142/143. `Given_Fonts.When_WeightsDiffer_Then_RenderedWidthsDiffer` fails the
      same way on `master` (the local Roboto package is missing its entry point); not related.
- [x] `xstyler --passive` (199/199), `dotnet format whitespace --verify-no-changes`, style docs `--check`,
      cspell and markdownlint on the changed pages.
- [ ] `check-xaml-resource-keys.ps1`: not run (needs a WinAppSDK `generic.xaml`, unavailable on macOS). The
      change removes a key reference and adds none.
- [ ] WinAppSDK head: not run.

## Follow-ups (outside this repo)

- Issue item 2, a size-aware pill/circle attached property, belongs in Uno.Toolkit.
- Uno.Samples (for example, the CRM sample's `DataTemplates.xaml` 88 × 8 bar) and the external reference docs
  and skills that generate `RadiusFullCornerRadius` for badges and chips.
