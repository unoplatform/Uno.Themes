# Simple theme — input validation visuals

## Context

Uno's input validation (`Uno.Extras.Input.Validation`: `Mode`, `Kind`, `ErrorTemplate`, `Errors`,
`HasErrors`, plus `FeatureConfiguration.InputValidation.IsEnabled`) is being added to the framework
in unoplatform/uno#24838. Controls that declare a validation property (TextBox, PasswordBox,
AutoSuggestBox, ComboBox) drive two visual-state groups and fill an `ErrorPresenter` template part.
For that to show anything, the theme's templates have to declare them.

This work adds those groups and parts to the Simple theme's input styles, so that a validated
control shows an error ring, a tint, and either a compact icon (errors in its tooltip) or an
inline error list under the field. With no errors the additions cost nothing: a zero-width icon
column, a zero-height inline row, and parts that stay Collapsed. A clean control renders exactly
as before.

The visuals were first prototyped as app-side style copies (up2384_InputValidation,
`Themes/SimpleInputTemplates.xaml`, taken at `faeacea7`). The template bodies have not changed
since then, so the additions port onto HEAD directly.

## Design

Each participating template gets the same five additions:

1. `Validation.ErrorTemplate` setter → `SimpleInputValidationErrorTemplate`.
2. `ErrorIconColumn` (MaxWidth 0 → 28 in `CompactErrors`) and `ErrorPresenterRow`
   (MinHeight 0 → 20 in `InlineValidationEnabled`).
3. `ErrorPresenter` part (`x:Load="False"`, Collapsed by default). The framework fills it: the
   error template for Inline, `DefaultCompactErrorIconTemplate` for Compact.
4. `ErrorBorderElement` (ring) and `ErrorBackgroundElement` (ErrorBrush tint at 0.12 opacity).
5. `InputValidationEnabledStates` / `InputValidationErrorStates` groups.

Why the ring and tint are separate parts: every VisualState setter writes one shared slot per
property, and a group leaving a state clears that slot. If a validation state wrote
`BorderElement.BorderBrush`, which CommonStates also writes, the error brush would be overwritten on
hover and then cleared. So the validation states only write parts that no other group touches.

Per-control shape:

- TextBox / PasswordBox (Outlined + Filled): `BorderElement` is a bare overlay, so it is
  collapsed while in error and the ring takes its place.
- ComboBox: `BorderElement` carries the content, so the ring covers it at the theme's
  heaviest stroke instead of replacing it.
- AutoSuggestBox: the field is drawn by the inner TextBox's template, a separate namescope. The
  outer error states set the inner `TextBox.Tag`. The inner style
  (`SimpleAutoSuggestBoxTextBoxStyle`, changed in place) hides its stroke and shows its tint while
  `Tag` is set. The outer template draws the ring as an overlay, sized to the field by an
  invisible `HeaderSpacer`.

`net10.0-windows` has no `Uno.Extras.Input`, so the ErrorTemplate setters and the two DataTemplates
are `not_win:`-gated. The added parts and states are inert there.

### Resource keys

| Key | Kind | Notes |
| --- | ---- | ----- |
| `SimpleInputValidationErrorTemplate` | DataTemplate | New. Error list; set via each style's `Validation.ErrorTemplate` setter |
| `DefaultCompactErrorIconTemplate` | DataTemplate | Framework-defined name, overridden at app level by the theme. InfoSolid glyph, empty tooltip the framework fills |

## Plan

Branch: `dev/xygu/20261005/simple-input-validation`

### Phase A — against the local validation build (Uno 7.0.0-dev.701 override)

Local only, never committed: Composition `HintPath` references in `src/samples/Directory.Build.props`.

- [x] Error templates — `Styles/Controls/InputValidation.xaml`
- [x] TextBox — `SimpleOutlinedTextBoxStyle`, `SimpleFilledTextBoxStyle`
- [x] PasswordBox — `SimpleOutlinedPasswordBoxStyle`, `SimpleFilledPasswordBoxStyle`
- [x] AutoSuggestBox — `SimpleAutoSuggestBoxTextBoxStyle`, `SimpleAutoSuggestBoxStyle`
- [x] ComboBox — `SimpleComboBoxStyle`
- [x] Sample — `IndeiStateViewer` control
- [x] Sample — `InputValidationSamplePage` + `InputValidationSampleViewModel` (INDEI), Compact and Inline sections
- [x] SimpleSampleApp — `FeatureConfiguration.InputValidation.IsEnabled = true`
- [x] Runtime tests — `Given_InputValidation`
- [x] Docs — `doc/simple-controls-styles.md` "Input validation" section
- [x] Verify
  - [x] Library builds for `windows` (not_win gating)
  - [x] SimpleSampleApp builds and runs on `net10.0-desktop`
  - [x] Sample page checked in Light/Dark × Compact/Inline; existing input pages unchanged
  - [x] `Given_InputValidation` + full runtime suite pass
  - [x] XAML Styler + `dotnet format whitespace` verify clean
- [ ] ⛔ **Checkpoint — human review before Phase B**

### Phase B — against Uno 7.0.0-dev.703 (no validation), temporary shim

Local only, never committed: the `Uno.Sdk.Private` 7.0.0-dev.703 pin in both `global.json`.

- [ ] Temporary `Uno.Extras.Input.Validation` shim drives the same states and fills `ErrorPresenter`
- [ ] SimpleSampleApp — drop the `FeatureConfiguration.InputValidation` line (absent on 703)
- [ ] Verify — desktop + windows builds, sample page matches Phase A, runtime suite passes

## TODO — when unoplatform/uno#24838 is merged and published

- [ ] Bump Uno to the first version that includes it
- [ ] Delete the temporary `Validation` shim
- [ ] Restore `FeatureConfiguration.InputValidation.IsEnabled = true` in SimpleSampleApp
- [ ] Re-run `Given_InputValidation` + the full suite against the real implementation

## Known limitations

- Commits between Phase A and the Phase B shim don't build on CI (the published 701 has no `Uno.Extras.Input`).
- Hosted in ThemesSampleApp, the feature flag is set after the host has registered its own bindings. Standalone SimpleSampleApp is the reference.

## Review

### Phase A (2026-10-05)

Verified against the local validation build (Uno 7.0.0-dev.701 override):

- Library builds for `net10.0` and `net10.0-windows10.0.19041` (MSBuild), Debug and Release; the Release
  XamlMerge output keeps the `input` / `not_win` namespaces.
- `Given_InputValidation`: 20/20 pass. Full SimpleSampleApp suite: all pass (one pre-existing `[Ignore]`).
- Existing TextBox / PasswordBox / AutoSuggestBox / ComboBox sample pages render pixel-identical to the
  master styles (RenderTargetBitmap comparison). This caught one regression, fixed in
  `fix(simple): keep the AutoSuggestBox fill inset under its stroke`: splitting the stroke off the fill
  let the fill extend under the translucent disabled stroke.
- XAML Styler and `dotnet format whitespace` verify clean; markdownlint and cSpell clean on the doc.

Local-only environment workarounds (never committed): the local framework build left empty
`ref/net10.0` folders in the cached `uno.foundation` / `uno.winrt` 7.0.0-dev.701 packages, which hide
their compile assets on any fresh restore, and split `Uno.UI.Composition.*` out of `Uno.UI`. Both are
patched with `Reference`/`HintPath` items in `Directory.Build.props` and `src/samples/Directory.Build.props`.

Open points for review:

- The inline error text inherits the field's font size (BodyLarge), larger than typical helper text.
- An inline-validated field reserves its 20px error row even when clean, by design (no jump on first error).
