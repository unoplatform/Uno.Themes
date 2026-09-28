# Material semantic implementation review

Original source audit of Material v2, 2026-09-06. No product code changed and no runtime reproductions ran in the original audit lane. Historical findings and line references remain below; follow-up implementation and verification are recorded separately.

## Resolution update — 2026-09-07

The Material Desktop runtime host passed all 74 tests after these fixes, including
the v1 compatibility guard and two PipsPager path-string checks. The Material
WebAssembly build passed; no WebAssembly runtime or native WinUI runtime execution
is claimed. Logs are recorded in [implementation progress](../semantic-fixes/progress.md).

| Finding | Resolution | Regression |
|---|---|---|
| Selected-rating hover | Primary and secondary templates consume their selected-hover brushes | `Given_SemanticOverrides.When_SelectedRatingIsHovered_Then_SemanticHoverBrushIsUsed` |
| Calendar glyph | Normal glyph explicitly consumes its resource; disabled paint remains independent | `Given_SemanticOverrides.When_CalendarGlyphBrushIsOverridden_Then_TemplateConsumesNormalAndDisabledKeys` |
| Cross-theme button measurements | Simple now consumes `ButtonBorderThickness` and dynamic `LabelLargeFontSize`; per-theme corner and padding keys remain documented | Simple host: `Given_SimpleSemanticOverrideStates.When_ButtonMeasurementsOverridden_PortableBorderAndFontAreConsumed` |
| Typography precedence | A v2-specific base dictionary merges shared defaults before Material typography; v1 retains its prior resource graph | `Given_Fonts.When_LabelExtraSmallIsUsed_Then_MaterialTypographyWinsOverSharedDefaults`, `When_LegacyMaterialResourcesAreLoaded_Then_SharedTypographyAndFontsRemainAvailable` |
| Markup PipsPager value types | Typed helpers use strings; Material defaults are confirmed parseable geometry strings | `Given_SemanticOverrides.When_PipsNavigationDataIsResolved_Then_ItIsAPathString`; Simple host: `Given_MarkupSemanticResources.When_PipsPathHelpersAreUsed_Then_StringConsumersResolvePathData` |

The additional `ContentDialog` static `HeadlineSmallFontSize` observation in the
original coverage notes remains an **unverified follow-up**, outside the numbered
confirmed fixes. No runtime reproduction or behavior change is claimed for it.
The published rating, calendar, and typography pages describe the corrected behavior.

## Original confirmed implementation gaps

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

## Original coverage and positive results

- Material v2 declares every semantic control-style key listed by the semantic skill, plus the 19 typography style aliases. No missing Material alias was found.
- Material reuses BaseTheme for seeds, color overrides, spacing, density and shape generation, and declares the 12 per-control font-family aliases needed by the generated font layer in `MaterialTheme.FontFamilyAliasKeys`.
- Existing Material runtime suites are in the Material sample head: `Given_DefaultPalette`, `Given_Fonts`, and `Given_DesignTokens`. They cover palette defaults, a rendered default button in both appearances, explicit seed activation, font availability, token math, and selected rendered token behavior. The Simple host's `Given_SemanticStyles` tests Simple only.
- No Material suite currently exercises the full semantic style matrix or the documented per-control interaction-state override matrix. In particular, the rating and calendar glyph seams above lack regression tests.
- `ContentDialog.xaml:214` still reads `HeadlineSmallFontSize` statically while the adjacent family resource is dynamic; review this alongside the cross-theme typography override contract rather than claiming that the single font-family scalar itself is broken.

## Original documentation fixes made

- `doc/material-controls-styles.md`: added eight omitted current style keys (ComboBoxItem, DatePickerFlyoutPresenter, MediaTransportControls, MenuFlyoutSeparator, MenuFlyoutSubItem, RadioMenuFlyoutItem, Ripple, ToggleMenuFlyoutItem), documented the compatibility calendar key, identified Material-specific additions, and linked semantic mappings and override guidance. `SecondaryRatingControlStyle` is valid: it is declared directly in RatingControl.xaml, not aliased in `_Resources.xaml`.
- `doc/material-colors.md`: corrected the default ShadowColor from opaque black to `#33000000` in both appearances; clarified the common 33-color/280-brush surface, the missing ShadowBrush family, opacity overrides, and seed versus explicit color overrides.

Central documentation corrections passed to the primary lane: semantic mapping table omits DatePickerFlyoutPresenterStyle; design-token table omits Radius500; seed-color/helper sections still inconsistently describe Material/Simple only despite Fluent support.

## Original verification

- Parsed the Material alias dictionary and compared it to the Material style table.
- Parsed SharedColorPalette and compared every documented Light/Default color value: ShadowColor was the only mismatch.
- Compared all overlapping Material and shared typography values for Light/Default: Light LabelExtraSmallFontWeight was the only mismatch.
- `git diff --check` passed for the two edited documentation files. Build/runtime results belong to the primary review report; this lane did not build or run tests.
