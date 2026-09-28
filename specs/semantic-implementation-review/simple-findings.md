# Simple semantic implementation review

Original source audit, 2026-09-06. No product changes or runtime execution occurred in the original review lane. Historical findings and line references below are retained; follow-up fixes and regression results are recorded separately. Paths are repository-relative.

## Resolution update — 2026-09-07

All seven findings are implemented and regression-tested in the Simple Desktop
host. The final full runs passed 671 tests with zero failures and one
pre-existing skipped hot-reload test in each real Light and Dark application appearance. Final appearance runs and build logs are in
[implementation progress](../semantic-fixes/progress.md). WebAssembly builds passed;
WebAssembly runtime and native WinUI runtime were not exercised.

| Finding | Resolution | Regression in `Given_SimpleSemanticOverrideStates` |
|---|---|---|
| 1: outlined resources | New `SimpleOutlinedButtonStyle` consumes its own brushes and retains the previous tonal defaults | `When_OutlinedButtonOverridden_UsesOwnStateKeys` |
| 2: icon toggle resources | 36 separate icon foreground/background/border keys, defaulting to matching text-family brushes | `When_IconToggleOverridden_UsesIndependentStateKeys` |
| 3: checkbox labels | Checked and indeterminate label keys are consumed in every combined interaction state | `When_CheckBoxLabelOverridden_UsesMatchingCombinedState` |
| 4: textbox header/placeholder | Both variants consume hover/focus keys alongside existing normal/disabled keys | `When_TextBoxLabelOverridden_HeaderAndPlaceholderFollowState` |
| 5: secondary hyperlink | Content and underline consume the secondary state family independently of primary links | `When_SecondaryHyperlinkOverridden_ContentAndUnderlineUseOwnState` |
| 6: missing styles | All 55 keys exist. Elevated maps to tonal without shadows, DatePickerFlyoutPresenter maps to its Simple presenter, CommandBar/MediaTransportControls use typed empty styles preserving native templates | `When_MissingSemanticStyleApplied_ResolvesOwnStyleAndTemplate` |
| 7: legacy secondary palette | Both appearances supply achromatic legacy secondary variants | `When_SeedlessSimplePaletteUsed_LegacySecondaryBrushesAreGrayscale` |
| Cross-theme measurement finding | The text-button base consumes portable `ButtonBorderThickness` and dynamic `LabelLargeFontSize`; Simple corner/padding keys retain existing scope | `When_ButtonMeasurementsOverridden_PortableBorderAndFontAreConsumed` |

The per-control documentation now describes these working consumers and the
intentional fallbacks. The original workaround notes below describe the audit
state and have been replaced in the published pages.

## Original confirmed implementation gaps

1. **P2 — Outlined button overrides are ineffective.** `src/library/Uno.Simple.WinUI/Styles/Controls/_Resources.xaml:106` maps `OutlinedButtonStyle` directly to `SimpleFilledTonalButtonStyle`. `Button.xaml:63` declares twelve `OutlinedButton*` brush aliases, but the actual style and template (`Button.xaml:302`) read only `FilledTonalButton*`. A scoped red `OutlinedButtonBackground` override resolves as a resource yet never reaches the outlined button. These aliases provide lookup parity, not override parity. Give the outlined semantic style consumers of its own keys, retaining the current default appearance if desired. Repro coverage: compare outlined and tonal controls with distinct scoped normal/state brushes, in Light and Dark.

2. **P2 — Icon toggle buttons consume the text toggle resource family.** `src/library/Uno.Simple.WinUI/Styles/Controls/ToggleButton.xaml:340` sets the icon variant's foreground/background/border from `TextToggleButton*`; every state in that template does the same. Simple declares no `IconToggleButton*` brush family, whereas Material exposes it and `doc/styles/ToggleButton.md:66` documents it. An `IconToggleButtonForegroundChecked` override is ignored; using the text key changes both variants. Define and consume the icon family independently. Repro coverage: one text toggle and one icon toggle, different checked colors, plus sibling-scope isolation.

3. **P2 — Checked and indeterminate CheckBox labels use unchecked keys.** `src/library/Uno.Simple.WinUI/Styles/Controls/CheckBox.xaml:88` declares the checked/indeterminate foreground families, but `CheckedNormal` (`:261`) and `IndeterminateNormal` (`:298`) never set a label foreground. Checked/indeterminate hover, pressed, and disabled states explicitly read the unchecked family (`:275`, `:284`, `:293`, `:312`, `:321`, `:330`). All eight checked/indeterminate label keys are unused. Wire each combined state to its matching key. Repro coverage: distinct label colors per check state and interaction state, inspecting the realized content presenter.

4. **P2 — TextBox header and placeholder hover/focus overrides are unused.** `src/library/Uno.Simple.WinUI/Styles/Controls/TextBox.xaml:55` onward declares both families' header/placeholder hover/focus keys. Filled state setters (`:250`, `:258`) and outlined state setters (`:397`, `:405`) update only the background, border, and input content. No other consumers read the eight header/placeholder state keys. Disabled keys are wired correctly. Repro coverage: populated header plus empty placeholder, distinct normal/hover/focus colors, both styles and appearances.

5. **P2 — Secondary hyperlink state overrides are unused.** `src/library/Uno.Simple.WinUI/Styles/Controls/HyperlinkButton.xaml:137` derives the secondary style from the primary template and overrides only normal `Foreground`. The template at `:90`, `:97`, and `:105` reads primary foreground keys, leaving `SecondaryHyperlinkButtonForegroundPointerOver`, `Pressed`, and `Disabled` declarations unused. The default secondary link also changes to the primary accent when hovered/pressed. Repro coverage: secondary-only state overrides and a primary sibling; inspect both content and underline.

6. **P2 — Semantic style coverage is incomplete.** `src/library/Uno.Simple.WinUI/Styles/Controls/_Resources.xaml:104`, `:189`, and `:192` explicitly omit `ElevatedButtonStyle`, `CommandBarStyle`, and `MediaTransportControlsStyle`. In addition, the semantic DatePicker block at `:148` omits `DatePickerFlyoutPresenterStyle` despite the concrete style existing in `DatePicker.xaml:227` and its implicit style being registered at `_Resources.xaml:57`. Material declares the semantic flyout key. Portable XAML cannot rely on the complete semantic style surface. Add the missing existing-style alias and decide supported nearest-match mappings for the other three. Repro coverage: one shared canonical style-key inventory, verified against each theme's own resources and expected target types, with explicit platform exceptions only.

7. **P2 — Simple's no-seed palette leaks purple legacy secondary colors.** `src/library/Uno.Simple.WinUI/Styles/Application/ColorPalette.xaml:38` and `:121` override the four modern secondary roles but omit `SecondaryVariantDarkColor` and `SecondaryVariantLightColor`. `SimpleTheme.cs:28` opts out of the default seed, so these roles fall through to shared defaults in `src/library/Uno.Themes/Styles/Applications/Common/SharedColorPalette.xaml:33` and `:89`: purple colors in both appearances. They are recognized shared roles (`SemanticColorKeys.cs:28`), so using their brushes in an otherwise grayscale Simple app introduces unrelated Material colors. Supply Simple defaults for both roles. Repro coverage: enumerate the full shared role inventory in both appearances under a seedless Simple theme and assert intended design-system values, including legacy roles.

## Original documentation changes made

- `doc/styles/simple/CheckBox.md`: explain which label-state keys are currently consumed and which declarations are ineffective.
- `doc/styles/simple/HyperlinkButton.md`: explain the inherited primary state keys and the scope effect of the current workaround.
- `doc/styles/simple/TextBox.md`: distinguish unwired hover/focus header/placeholder keys from working disabled and input-state overrides.

These notes describe current limitations, not an intended semantic contract. Remove/update them when the corresponding implementation and regression tests are fixed.

## Original additional central documentation corrections for the primary review lane

- `doc/semantic-styles.md` claims its mappings show every semantic style key but omits `DatePickerFlyoutPresenterStyle` entirely.
- Its portability prose should distinguish keys that resolve from keys actually consumed by the templates, specifically Simple outlined buttons and icon toggle buttons.
- The statement that Material and Simple base their typography values on M3 is inaccurate for Simple. `Styles/Application/Typography.xaml:10` maps SDS title-hero/page/body roles to shared slot names (for example DisplayLarge is 72/Bold); shared names do not imply shared numeric values.
- `doc/styles/ToggleButton.md:12` labels the icon variant as the implicit default without theme qualification; Simple's default is `SimpleTextToggleButtonStyle` (`ToggleButton.xaml:458`).
- Many shared per-control resource pages list Material values without labeling their design-system scope; the existence of Simple-specific pages does not make the shared table an override compatibility matrix.

## Original coverage assessment and validation

`Given_SemanticStyles.cs` checks normal appearance equivalence for five button variants and override behavior for only filled, tonal, and text buttons. Its outline equivalence row does not override outlined keys, so it cannot detect finding 1. The other gaps require realized-state checks, not only resource-resolution assertions. Review inspected every direct consumer of the keys cited above; no source build or runtime test was executed by this lane. Three documentation files were edited; product code and tests are unchanged.
