# Uno Semantic Design Language implementation review

Reviewed `dev/sb/fluent-theme` at `4c9f46c8` on 2026-09-06. Scope: the current implementations, including shared infrastructure and Material v2, Simple, Fluent, C# Markup helpers, runtime coverage, and published documentation. This is a review plus documentation corrections; implementation behavior and tests were not changed.

## Assessment

The three themes share much of the semantic vocabulary, but **do not provide equivalent override behavior**. Several controls expose resources they never consume. Fluent's native-resource adapter has narrower input, scope, and lifecycle behavior than the shared theme layer. Passing existing tests does not establish the missing cross-theme contract.

The source inventory contains 55 semantic control-style keys, excluding Material-specific Ripple and compatibility keys. All three themes expose 19 semantic typography styles, 33 color roles, 280 shared brushes per appearance, and generated spacing, shape, and density resources.

| Theme | Semantic control styles | Missing keys |
|---|---:|---|
| Material v2 | 55/55 | None |
| Simple | 51/55 | `ElevatedButtonStyle`, `CommandBarStyle`, `MediaTransportControlsStyle`, `DatePickerFlyoutPresenterStyle` |
| Fluent | 54/55 | `DatePickerFlyoutPresenterStyle` |

Fluent's 54 includes the two button styles mapped in code, rather than only declarations in `_Resources.xaml`. Its nearest-match FAB/elevated mappings and nine empty styles preserving native templates are deliberate design choices. Different default appearances are valid; missing keys and ineffective explicit overrides are the portability problems.

## Priority findings

P2 means a functional defect or contract gap to address before claiming semantic override parity. P3 indicates a lower-impact implementation inconsistency. These findings are established by source inspection unless explicitly labeled as needing runtime confirmation. Existing runtime suites were executed separately; no newly written regression test reproduced these findings during this review.

### Shared infrastructure

1. **P2 — Nested color overrides collapse the appearance branches.** `SemanticBrushUpdater.TryResolve` only inspects a layer's immediate `ThemeDictionaries`, then falls back to `layer.TryGetValue`. A root override dictionary that wraps a child with Light red / Default blue through `MergedDictionaries` has no direct themed entries, so that final lookup uses the ambient theme for both generated brush branches. `BaseTheme.BuildColorLayer` supplies the complete root as one layer, without flattening it. The resulting branch-specific resource values and shared brush colors can disagree. Fix the resolver to traverse merged dictionaries with explicit appearance and precedence, including themed children, without ambient/system fallback. See `src/library/Uno.Themes/Helpers/SemanticBrushUpdater.cs`, methods `TryResolve` and `IsThemeScoped`, and `src/library/Uno.Themes/BaseTheme.cs`, `BuildColorLayer`.

2. **P2 — Shape input validation differs from spacing and the Fluent adapter.** `BaseTheme.UpdateSource` validates `DefaultSpacing` but passes `DefaultCornerRadius` directly to `GenerateShapeScale`. Negative, NaN, and infinite values therefore enter semantic radius resources; NaN/infinity also invalidate the nominal zero token. Fluent's `BuildPlatformTokenOverrides` rejects these inputs, leaving native control radii valid while semantic resources are invalid. Validate once in the shared generator and use the same normalized value in adapters. The local Uno `CornerRadius` constructor simply stores values: this is a confirmed invalid-token problem, not a proven crash. See `BaseTheme.cs`, `BaseTheme.ScaleGeneration.cs:177`, and `Uno.Fluent.WinUI/FluentTheme.cs:269`.

### Simple

3. **P2 — Outlined-button overrides are disconnected.** `OutlinedButtonStyle` selects the tonal style, whose consumers read `FilledTonalButton*`; the twelve `OutlinedButton*` aliases cannot customize that control. See `Uno.Simple.WinUI/Styles/Controls/_Resources.xaml:106` and `Button.xaml:302`.

4. **P2 — Icon-toggle override keys are not portable.** Simple's separate icon template consumes `TextToggleButton*`; Material's `IconToggleButton*` brush family is absent. Changing text-toggle keys affects both variants. See `Uno.Simple.WinUI/Styles/Controls/ToggleButton.xaml:340`.

5. **P2 — Several documented state overrides have no consumers.** Checked/indeterminate CheckBox labels use unchecked resources; TextBox header/placeholder hover/focus keys are unused in both variants; secondary HyperlinkButton states read the primary resource family. See `CheckBox.xaml:261`, `TextBox.xaml:250`, and `HyperlinkButton.xaml:137` under Simple's controls tree. Each needs realized-state coverage with distinct override values.

6. **P2 — Style availability and the seedless palette are incomplete.** Besides the four missing aliases above, Simple omits the two legacy secondary variant colors from its grayscale palette, leaving the shared purple Material values. `DatePickerFlyoutPresenterStyle` could map to an existing Simple concrete presenter; the other style omissions require an explicit implementation/fallback decision. See `Uno.Simple.WinUI/Styles/Controls/_Resources.xaml:148` and `Styles/Application/ColorPalette.xaml:38`.

### Material and shared control measurements

7. **P2 — Rating and calendar-glyph override keys are ignored.** Material's selected-rating hover states use the ordinary selected brush, including the secondary variant. CalendarDatePicker's enabled glyph never consumes its advertised normal foreground key, though the disabled key is wired. See `Uno.Material/Styles/Controls/v2/RatingControl.xaml:127` and `CalendarDatePicker.xaml:303`.

8. **P2 — Button measurement/typography resources are not consistently overridable.** Material reads `ButtonBorderThickness` and `LabelLargeFontSize` dynamically. Simple uses `SimpleButtonBorderThickness` and static size resolution. Matching semantic button styles therefore do not support all the same lightweight geometry and typography overrides or refresh behavior. See Material `Button.xaml:288` and Simple `Button.xaml:201`.

9. **P3 — Material has shadowed typography declarations.** Material's merged own typography entries sit below the merged shared typography dictionary. The differing Light `LabelExtraSmallFontWeight` is declared Medium in Material but Normal in the shared dictionary. Most other overlapping values currently match, hiding this precedence problem. Verify intended weight with a realized Light text block before correcting the merge. See Material `Styles/Application/v2/Typography.xaml:241`, `Styles/Application/Common/BaseDictionaries.xaml:22`, and `material-common.props:121`.

### Fluent

10. **P2 — Override input channels and dictionary structures disagree.** Both Fluent bridges ignore nested `MergedDictionaries`, and constructor `colorOverride` values never enter the bridge input read from `Colors.OverrideDictionary`. An override can change semantic lookup while leaving native controls unchanged. See `FluentAccentPalette.cs:225`, `FluentLightweightBridge.cs:313`, and `FluentTheme.cs:189`.

11. **P2 — Text-button resource presence overstates rendered support.** Only normal `TextButtonForeground` is consumed; advertised hover/pressed foreground, background, and border semantic values are mirrored but unused. See `Uno.Fluent.WinUI/Styles/Controls/Button.xaml:21` and `FluentLightweightBridge.cs:180`.

12. **P2 — Root-font and general semantic palette/brush overrides do not have parity.** Fluent font slots target `ContentControlThemeFontFamily` directly, so a dictionary overriding only `DefaultFontFamily` does not cascade as it does through app-level Material/Simple aliases. The `DefaultFontFamily` property works by generating concrete keys. Similarly, native accent translation selects `PrimaryColor` only: overriding `OnPrimaryColor`, `OnPrimaryBrush`, or `PrimaryBrush` changes semantic resources but does not generally customize native filled-button foreground/fill. See `Uno.Fluent.WinUI/Styles/Application/Fonts.xaml:35` and `FluentAccentPalette.cs:232`.

13. **P2 — System accent is captured, not observed.** Fluent populates its semantic palette at construction and only retries if population failed. A successful palette is not repopulated after the Windows accent changes, even on an unrelated theme rebuild. See `FluentTheme.cs:161` and `FluentColorPalette.cs:80`.

**P2 — Fluent can throw while reloading a previously valid override source.** `BaseTheme.BuildColorLayer` catches a failed source reload and retains the assigned override, but `FluentTheme.AddThemeSpecificResources` immediately loads that source again without a catch (`FluentTheme.cs:189`). A source failure after initial assignment can therefore escape a property-change/hot-reload callback after the base has committed its replacement layers. Reuse the base's resolved override or apply the same guarded fallback; test a previously valid source that becomes unresolvable. This is a source-confirmed missing guard, not a runtime failure injected during the audit.

### Runtime confirmation still required

14. **P2 candidate — Dark-only native accent overrides can leak into Light.** With no seed and only a `Dark` primary override, the generated accent dictionary has only `Default`, which is also a Light fallback. Literal branch-walking tests miss this resolution path. Validate using an actual Light consumer before treating the runtime failure as reproduced. See `FluentAccentPalette.cs:265` and detailed finding F2.

15. **P2 validation gap — Live native Fluent seed changes replace brush instances.** Generated native accent/bridge brushes are replaced; only shared semantic brushes are mutated in place. Existing tests set the seed before realization or clear it and recreate the control. They do not prove that the same realized native controls follow seed A → seed B → clear. Add that test in both appearances and on both Uno/Desktop and native WinUI. See `FluentTheme.cs:212`, `FluentLightweightBridge.cs:270`, and detailed finding F8. Stale rendering is a risk supported by the replacement mechanism, not a new runtime failure reproduced here.

Full source evidence, affected states, and suggested reproductions are in [Material findings](material-findings.md), [Simple findings](simple-findings.md), and [Fluent findings](fluent-findings.md).

### C# Markup API

16. **P2 — Typed helpers describe incompatible value types.** All 19 `Theme.Typography.*.FontWeight` helpers use `ThemeResourceKey<Microsoft.UI.Text.FontWeights>`, the provider class rather than the `FontWeight` property value type; the XAML resources themselves are strings converted by setters. PipsPager's `PreviousPageButtonData` and `NextPageButtonData` helpers use `ThemeResourceKey<double>` for path-data string resources. These are incorrect public generic/metadata declarations; this review has not compiled or executed a consumer repro for each conversion path. Add typed consumer compilation/override coverage before correcting the API. See `src/library/Uno.Themes.WinUI.Markup/Theme/Typography.cs:13` and `Theme/PipsPager.cs:148`, compared with Material `Styles/Controls/v2/PipsPager.Base.xaml`.

17. **P3 — The markup surface is incomplete.** Seven of the 55 semantic control-style keys have no wrapper: ComboBoxItem, DatePickerFlyoutPresenter, MediaTransportControls, MenuFlyoutSeparator, MenuFlyoutSubItem, RadioMenuFlyoutItem, and ToggleMenuFlyoutItem. Typography helpers also omit family keys, and the color helpers omit ShadowColor. This does not remove the underlying XAML resources, but contradicts the previous documentation's claim that all keys have helpers. An attribute/constructor-string comparison across 1,232 helper declarations found no string mismatches.

## Documentation corrections

- Expanded `doc/semantic-styles.md` into the cross-theme semantic contract: all 55 style mappings, typography differences, all 33 roles, exact 280-brush inventory and exceptions, state opacities, pairing guidance, override channels/scopes, appearance fallback, and compatibility gaps.
- Corrected missing `Radius500` and directional spacing companions, scalar-versus-companion independence, font-root/property precedence, source-cache lifecycle, Fluent token limitations, and invalid-radius behavior in `doc/design-tokens.md`.
- Corrected Fluent setup/lifecycle claims, seed guidance, helper API coverage, lightweight bridge scope and missing consumers, and overview cross-links.
- Added missing Material style entries, corrected ShadowColor alpha, and clarified the shared palette versus Material defaults.
- Added focused control-page notes for ineffective overrides. Corrected public `BaseTheme` XML comments that still described spacing/shape/density as construction-only and overstated font-root override precedence. XML comments are the only library-source edits; executable behavior is unchanged.

## Verification

Results and limitations are recorded in [progress.md](progress.md). Local build/runtime logs and NUnit XML are retained beside this report and excluded from git. The source inventory and documentation links were also checked independently. No dependencies, target frameworks, or project structure were changed.

## Recommended regression contract

Use one canonical inventory across all three themes, asserting expected control target types and direct resource values in Light and Dark. For every override family, realize controls with deliberately different rest/hover/pressed/disabled values; cover checked/indeterminate states where applicable. A resource lookup alone cannot detect unused template keys.

Exercise constructor, dictionary, source, deprecated forwarding, and clear/reset paths; flat, appearance-specific, and nested dictionaries; application, page, and control scopes; competing merged siblings; root-font versus individual-slot overrides; and same-control runtime changes. Include branch fallbacks in actual consumer resolution rather than only testing named dictionary entries. Preserve stock Fluent differences while asserting that shared explicit overrides behave consistently.
