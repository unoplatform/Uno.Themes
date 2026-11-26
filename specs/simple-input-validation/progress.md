# Simple theme — input validation visuals

## Context

Uno's input validation (`Uno.UI.Xaml.Controls.Validation`: `Mode`, `Kind`, `ErrorTemplate`, `Errors`,
`HasErrors`, plus `FeatureConfiguration.InputValidation.IsEnabled`) was added to the framework in
unoplatform/uno#24838 (merged 2026-10-21; shipped in Uno 7.1.0-dev.477, which master uses). Controls that declare a validation property (TextBox, PasswordBox,
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

On `net10.0-windows` the `Validation` properties exist (`Uno.UI.Extras.dll` registers the same attached properties)
but have no effect, so the ErrorTemplate setters and the two DataTemplates load there unused, and every key exists
on every platform. The added parts and states are inert there. (Until Phase C they were `not_win:`-gated, since
the stand-in namespace only existed on Uno.)

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
- [x] ⛔ **Checkpoint — human review before Phase B**

### Phase B — against Uno 7.0.0-dev.703 (no validation), temporary shim

Local only, never committed: the `Uno.Sdk.Private` 7.0.0-dev.703 pin in both `global.json`.

- [x] Temporary `Uno.Extras.Input.Validation` shim drives the same states and fills `ErrorPresenter`
      (`src/library/Uno.Themes/InputValidation/`, Uno-only via `HAS_UNO`; in Uno.Themes.WinUI because the
      shared sample page also compiles into the Material and Cupertino heads)
- [x] SimpleSampleApp — drop the `FeatureConfiguration.InputValidation` line (absent on 703), left as a TODO comment
- [x] Verify — desktop + windows builds, sample page matches Phase A, runtime suite passes

### Phase C — the framework implementation (rebased onto master, Uno 7.1.0-dev.477)

No bump needed: master's Uno already ships unoplatform/uno#24838. The framework's namespace is
`Uno.UI.Xaml.Controls`, not the `Uno.Extras.Input` the shim had copied; the template contract (state groups,
`ErrorPresenter`, `DefaultCompactErrorIconTemplate`) is unchanged.

- [x] Styles, sample page and `Given_InputValidation` → `Uno.UI.Xaml.Controls` (the test aliases the types:
      that namespace also declares a `ComboBox`)
- [x] Delete the temporary shim (`src/library/Uno.Themes/InputValidation/`)
- [x] Enable `FeatureConfiguration.InputValidation.IsEnabled` in SimpleSampleApp (`HAS_UNO`)
- [x] Docs — namespace, app-wide switch, link to the framework's input validation page
- [x] Accept the framework's states when validation is disabled: on `Mode` → `Disabled` it goes to
      `ValidationDisabled` but leaves `InputValidationErrorStates` where it was (`UpdateValidationStatesInternal`),
      so the ring and `ErrorPresenter` stay visible while `HasErrors` is false; the shim cleared both groups.
      The state groups match WinUI, whose `CControl::EnsureValidationVisuals` only goes to `ValidationDisabled`.
      The presenter does not: WinUI's `DeferErrors` unloads the `x:Load` `ErrorPresenter` (`TryDefer`), which
      Uno cannot do, so it stays visible on Uno (a framework gap, not worked around here). The test, renamed
      `When_ModeDisabledThenAuto_Then_ValidationDisablesAndReturns`, now asserts the state groups, not the
      presenter (theme-side and framework-side alternatives declined)
- [x] WinAppSDK heads: the sample page resolves `Uno.UI.Xaml.Controls` from `Uno.UI.Extras.dll`;
      `Given_InputValidation` is gated to `HAS_UNO` (the properties are a no-op there); the sample view model's
      `IReadOnlyList<string>` collection expression is an array (CsWinRT1032). Simple and Material heads build
- [x] Drop the `not_win:` gating: `Uno.UI.Extras.dll` defines the same `Validation` attached properties on Windows,
      so the ErrorTemplate setters and the two DataTemplates load there unused, and `SimpleInputValidationErrorTemplate`
      exists on every platform

## Known limitations

- Commits between Phase A and the Phase B shim don't build on CI (the published 701 has no `Uno.Extras.Input`).
- The WinAppSDK heads (added on master by #1740 after this branch was cut) don't build at the Phase B commits:
  the test's `Uno.Extras.Input` is Uno-only.

## Review

### Phase A (2026-10-05)

Verified against the local validation build (Uno 7.0.0-dev.701 override):

- Library builds for `net10.0` and `net10.0-windows10.0.19041` (MSBuild), Debug and Release; the Release
  XamlMerge output keeps the `input` / `not_win` namespaces.
- `Given_InputValidation`: 20/20 pass, Debug and Release (merged dictionaries). Full SimpleSampleApp
  suite: 298 pass, 1 skipped (pre-existing `[Ignore]`), Debug and Release. MaterialSampleApp and
  CupertinoSampleApp (which also compile the shared page) build with no new warnings.
- Existing TextBox / PasswordBox / AutoSuggestBox / ComboBox sample pages render pixel-identical to the
  master styles (RenderTargetBitmap comparison), in Light and Dark, resting and with the first input in
  its Focused state. This caught two AutoSuggestBox regressions from splitting the stroke off the fill: the
  fill extended under the translucent disabled and focus strokes. Both fixed (`fix(simple): keep the
  AutoSuggestBox fill inset ...`).
- XAML Styler and `dotnet format whitespace` verify clean; markdownlint and cSpell clean on the doc.

Local-only environment workarounds (never committed): the local framework build left empty
`ref/net10.0` folders in the cached `uno.foundation` / `uno.winrt` 7.0.0-dev.701 packages, which hide
their compile assets on any fresh restore, and split `Uno.UI.Composition.*` out of `Uno.UI`. Both are
patched with `Reference`/`HintPath` items in `Directory.Build.props` and `src/samples/Directory.Build.props`.

Open points for review:

- The inline error text inherits the field's font size (BodyLarge), larger than typical helper text.
- An inline-validated field reserves its 20px error row even when clean, by design (no jump on first error).

### Phase B (2026-10-05)

Verified against Uno 7.0.0-dev.703 (no framework validation) with the temporary shim:

- Library builds for `net10.0` and `net10.0-windows10.0.19041`; Simple, Material and Cupertino heads build
  for desktop with no new warnings.
- `Given_InputValidation`: 24/24 pass, including the attached-property set/clear scenario
  (`When_ModeDisabledThenAuto_...`) and the teardown-leak guard (`When_ControlRemoved_Then_LongLivedSourceDoesNotKeepItAlive`,
  confirmed red with the Unloaded unsubscribe removed). Full SimpleSampleApp suite, Debug and Release: 302 pass,
  1 skipped (pre-existing `[Ignore]`). ThemesSampleApp `--smoke` was not run.
- The sample page renders the same visuals as in Phase A (Light/Dark × Inline/Compact; pixel comparison across
  the two framework builds is not meaningful, as text rendering differs). The existing input sample pages render
  pixel-identical between master and branch styles on 703.
- Found and fixed while verifying: controls loaded inside a collapsed section never got their error states
  (no template yet when the shim first ran). Guarded by `When_ShownAfterBeingCollapsed_Then_ErrorStateShows`,
  confirmed red without the fix and green with it.

### Phase C (2026-11-25)

Verified on master's Uno 7.1.0-dev.477 with the framework implementation, shim removed:

- SimpleSampleApp builds for `net10.0-desktop` (Debug). Simple and Material WinAppSDK heads build with
  MSBuild (`net10.0-windows10.0.19041`, x64, Debug): 0 errors, no warnings from the input validation files.
  Before this phase the Simple head failed on the test's Uno-only `Uno.Extras.Input` (17 errors).
- `check-xaml-resource-keys.ps1`: all references resolve.
- Full SimpleSampleApp suite, Debug: 380 pass, 1 fail, 1 skipped (pre-existing `[Ignore]`). The failure was
  `When_ModeDisabledThenAuto_Then_ErrorStateClearsAndReturns` ("ErrorPresenter should hide": still Visible after
  `Mode` → `Disabled`), the disabled-state difference above. With the test rewritten, `Given_InputValidation`: 24/24.
- `When_ShownAfterBeingCollapsed_Then_ErrorStateShows` passes: the framework re-applies the states on template
  application, which the shim had to emulate.
- Without the `not_win:` gating (2026-11-26): the Simple WinAppSDK head builds (MSBuild, x64, Debug) with no XAML
  compiler diagnostics, and the library's `XamlTypeInfo.g.cs` registers `Validation.ErrorTemplate`.
  `check-xaml-resource-keys.ps1` resolves every key. In the packaged app, `Given_AllSamplePages` passes (1/1), and
  the Input Validation page survives switching Compact/Inline and entering invalid input, with no visuals (inert,
  as expected). The desktop head builds.
