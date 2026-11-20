# Material semantic implementation review

Source audit of the current Material v2 implementation, 2026-09-06. No product code changed in this lane. Findings below are established from resource declarations and their consuming templates; runtime reproductions were not executed by this lane.

## Confirmed implementation gaps

### P2: selected-rating hover overrides are ignored

- `src/library/Uno.Material/Styles/Controls/v2/RatingControl.xaml:127` uses `RatingControlSelectedForeground` in `PointerOverSet`, the same resource as the normal `Set` state.
- The secondary template repeats this at line 217 with `SecondaryRatingControlSelectedForeground`.
- Both `RatingControlSelectedForegroundPointerOver` and `SecondaryRatingControlSelectedForegroundPointerOver` are declared in the same file and advertised in `doc/styles/RatingControl.md:29` and `:37`, but neither is consumed by these state setters.
- Reproduction: override the selected and selected-pointer-over brushes with different colors, apply `RatingControlStyle` (then `SecondaryRatingControlStyle`), set a rating, and enter `PointerOverSet`. The foreground presenter receives the normal selected brush, so the documented hover override cannot affect this state.
- Regression needed: realize both styles and assert the foreground presenter under `Set` and `PointerOverSet`, with separate override brushes, under Light and Dark.

### P2: CalendarDatePicker ignores its normal glyph foreground key

- `src/library/Uno.Material/Styles/Controls/v2/CalendarDatePicker.xaml:24` and `:67` declare `CalendarDatePickerCalendarGlyphForeground` for the appearance branches.
- The `CalendarGlyph` FontIcon at line 303 has no foreground resource binding. The disabled animation does consume the separate `CalendarDatePickerCalendarGlyphForegroundDisabled` key at line 176.
- The normal key is documented in `doc/styles/CalendarDatePicker.md:31`. Overriding it cannot affect this FontIcon because there is no reference to it in the template; the normal glyph inherits its foreground instead.
- Regression needed: realize `CalendarDatePickerStyle`, override normal and disabled glyph brushes independently, and inspect the glyph in both enabled and disabled states under Light and Dark.

### P2: button typography and geometry overrides are not a uniform cross-theme contract

- Material's base button reads `LabelLargeFontSize` with `ThemeResource` in `src/library/Uno.Material/Styles/Controls/v2/Button.xaml:288`; Simple reads the same semantic type-size key through `StaticResource` in `src/library/Uno.Simple.WinUI/Styles/Controls/Button.xaml:201`. A later runtime font-resource replacement and theme refresh therefore do not have equivalent behavior.
- Material reads the unprefixed `ButtonBorderThickness` through `ThemeResource` (`Button.xaml:296`), while Simple reads `SimpleButtonBorderThickness` through `StaticResource` (`Simple Button.xaml:206`). Material's `ButtonCornerRadius` differs from Simple's `SimpleButtonCornerRadius`; their padding keys differ too.
- Shared semantic brush names do not establish parity for the complete lightweight resource surface. A `ButtonBorderThickness` override copied from `doc/lightweight-styling.md` is Material-specific even when both controls use `FilledButtonStyle`.
- This is a contract gap, not a request to make the design systems look identical. Preserve different defaults while providing consistent override names and resolution behavior, or clearly delimit the portable subset.

### P3: Material typography declarations are shadowed by the shared dictionary

- `src/library/Uno.Material/Styles/Application/Common/BaseDictionaries.xaml:22` merges `SharedTypography.xaml` by Source. `material-common.props:121` leaves Material v2 Typography in the XamlMerge input, so its themed entries become the parent's own themed entries, below the merged shared dictionary (the repository's recorded XamlMerge precedence lesson).
- The measurable differing value is Light `LabelExtraSmallFontWeight`: Material declares `Medium` at `Styles/Application/v2/Typography.xaml:241`; shared Light declares `Normal` at `src/library/Uno.Themes/Styles/Applications/Common/SharedTypography.xaml:194`. The intended Material value cannot take precedence through this resource graph.
- Other overlapping Light/Default size, weight and spacing values currently match, so this is mostly a latent maintenance trap. Do not claim all Material typography is visually wrong.
- Regression needed: assert the declared Material Light weight and a realized `LabelExtraSmall` TextBlock. A correction should follow the same explicit-source merge approach already used by Simple's typography, if `Medium` is the intended Material value.

## Coverage and positive results

- Material v2 declares every semantic control-style key listed by the semantic skill, plus the 19 typography style aliases. No missing Material alias was found.
- Material reuses BaseTheme for seeds, color overrides, spacing, density and shape generation, and declares the 12 per-control font-family aliases needed by the generated font layer in `MaterialTheme.FontFamilyAliasKeys`.
- Existing Material runtime suites are in the Material sample head: `Given_DefaultPalette`, `Given_Fonts`, and `Given_DesignTokens`. They cover palette defaults, a rendered default button in both appearances, explicit seed activation, font availability, token math, and selected rendered token behavior. The Simple host's `Given_SemanticStyles` tests Simple only.
- No Material suite currently exercises the full semantic style matrix or the documented per-control interaction-state override matrix. In particular, the rating and calendar glyph seams above lack regression tests.
- `ContentDialog.xaml:214` still reads `HeadlineSmallFontSize` statically while the adjacent family resource is dynamic; review this alongside the cross-theme typography override contract rather than claiming that the single font-family scalar itself is broken.

## Documentation fixes made

- `doc/material-controls-styles.md`: added eight omitted current style keys (ComboBoxItem, DatePickerFlyoutPresenter, MediaTransportControls, MenuFlyoutSeparator, MenuFlyoutSubItem, RadioMenuFlyoutItem, Ripple, ToggleMenuFlyoutItem), documented the compatibility calendar key, identified Material-specific additions, and linked semantic mappings and override guidance. `SecondaryRatingControlStyle` is valid: it is declared directly in RatingControl.xaml, not aliased in `_Resources.xaml`.
- `doc/material-colors.md`: corrected the default ShadowColor from opaque black to `#33000000` in both appearances; clarified the common 33-color/280-brush surface, the missing ShadowBrush family, opacity overrides, and seed versus explicit color overrides.

Central documentation corrections passed to the primary lane: semantic mapping table omits DatePickerFlyoutPresenterStyle; design-token table omits Radius500; seed-color/helper sections still inconsistently describe Material/Simple only despite Fluent support.

## Verification

- Parsed the Material alias dictionary and compared it to the Material style table.
- Parsed SharedColorPalette and compared every documented Light/Default color value: ShadowColor was the only mismatch.
- Compared all overlapping Material and shared typography values for Light/Default: Light LabelExtraSmallFontWeight was the only mismatch.
- `git diff --check` passed for the two edited documentation files. Build/runtime results belong to the primary review report; this lane did not build or run tests.
