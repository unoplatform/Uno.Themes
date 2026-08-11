# TimePicker styles for Material v2 and Simple

**Branch:** `dev/sb/time-picker`

## Problem

`TimePicker` had no style in Material v2, Simple, or Cupertino. The only coverage was
`src/library/Uno.Material/Styles/Controls/v1/TimePicker.xaml`, which:

- lives in the v1 merged dictionary only (`mergedpages.v1.xaml`);
- binds v1-only brushes (`MaterialPrimaryBrush`, `TextBoxFilledBackgroundColorBrush`,
  `MaterialBody2`) that no longer exist in v2;
- has no `ThemeDictionaries` block, so nothing is lightweight-stylable;
- exposes `MaterialTimePickerFlyoutPresenterStyle` as an `<ios:Style>` — the flyout presenter is
  unstyled on every non-iOS target;
- omits the named template parts Uno's `TimePicker` drives
  (`First|Second|ThirdPickerHost`, `First|Second|ThirdTextBlockColumn`,
  `First|SecondColumnDivider`, `HeaderContentPresenter`), so culture-driven hour/minute/period
  reordering and 24-hour column collapse do not work.

Scope of this change: Material v2 and Simple. Cupertino and Material v1 are untouched.

## Reference used for the template contract

Part names and visual states were taken from Uno's WinUI port, not guessed:

- `Uno.UI/UI/Xaml/Controls/TimePicker/TimePicker.partial.mux.cs` — `GetTemplateChild<Border>("FirstPickerHost")`,
  `GetTemplateChild<ColumnDefinition>("FirstTextBlockColumn")`, `GetTemplateChild<UIElement>("FirstColumnDivider")`, …
  States: `Normal` / `Disabled`, `HasTime` / `HasNoTime`. **No `PointerOver` / `Pressed`** — unlike `DatePicker`.
- `Uno.UI/UI/Xaml/Controls/TimePicker/TimePickerFlyoutPresenter.partial.mux.cs` — `Background`, `TitlePresenter`,
  `ContentPanel`, `First|Second|ThirdPickerHost`, `First|Second|ThirdPickerHostColumn`,
  `First|SecondPickerSpacing`, `AcceptDismissHostGrid`, `AcceptButton`, `DismissButton`.
- Windows SDK `generic.xaml` (10.0.26100.0, lines 11867 and 14894) for the reference layout.

## Work items

- [x] `src/library/Uno.Material/Styles/Controls/v2/TimePicker.xaml`
      — `MaterialTimePickerStyle`, `MaterialTimePickerFlyoutPresenterStyle`,
      `MaterialTimePickerFlyoutButtonStyle`, `MaterialDefaultTimePickerStyle`,
      `MaterialDefaultTimePickerFlyoutPresenterStyle`, plus Light/Default `ThemeDictionaries`.
      Layout mirrors `v2/DatePicker.xaml` (filled field, bottom border, `ElevatedView` flyout).
- [x] `src/library/Uno.Simple.WinUI/Styles/Controls/TimePicker.xaml`
      — `Simple*` equivalents on SDS tokens, layout mirroring `Simple/DatePicker.xaml`
      (header above an outlined field, plain `Border` flyout).
- [x] `v2/_Resources.xaml` — implicit styles for `TimePicker` / `TimePickerFlyoutPresenter`;
      semantic aliases `TimePickerStyle`, `TimePickerFlyoutPresenterStyle`.
- [x] Simple `Controls/_Resources.xaml` — same two implicit styles and two semantic aliases.
- [x] Sample: `TimePickerSamplePage.xaml` gains `M3MaterialTemplate` and `SimpleTemplate`
      (default / 24-hour / minute-increment / disabled); `SupportedDesigns` gains `Design.Simple`.
      The v1 `MaterialTemplate` is left as-is.
- [x] Runtime tests: `src/samples/SimpleSampleApp/RuntimeTests/Given_TimePickerStyles.cs`.
- [x] Docs: `doc/material-controls-styles.md`, `doc/simple-controls-styles.md`,
      `doc/semantic-styles.md`, `doc/lightweight-styling.md`, `doc/styles/TimePicker.md`,
      `doc/styles/simple/TimePicker.md`.

## Decisions

**Rectangle dividers, not a hardcoded `:`.** v1 rendered a literal colon `TextBlock` between hour and
minute. Uno reorders the hour / minute / period hosts per culture, and in period-first cultures
(`zh`, `ja`, `ko`) a fixed colon would land between the period and the hour. Both new styles use the
WinUI-shaped `Rectangle` dividers (`First|SecondColumnDivider`), themed via `TimePickerSpacerFill` /
`TimePickerColumnDividerWidth`, which stay correct in every culture and collapse automatically with
the column they separate.

**No `*PointerOver` / `*Pressed` lightweight keys.** `TimePicker` never enters those states, so such
keys would be dead API. Pressed/disabled feedback comes from the flyout-button opacity keys, matching
how `DatePicker`'s flyout button already works in both themes.

**No `FlyoutPresenterStyle` setter.** `DatePicker` sets `not_win:FlyoutPresenterStyle`, but
`TimePicker` has no such property (`TimePicker.Properties.cs` declares only `ClockIdentifier`,
`Header`, `HeaderTemplate`, `LightDismissOverlayMode`, `MinuteIncrement`, `SelectedTime`, `Time`).
The presenter is styled through the implicit `TimePickerFlyoutPresenter` style in `_Resources.xaml`
instead — the same mechanism that already covers `DatePickerFlyoutPresenter`.

**`TitlePresenter` included.** The repo's `DatePickerFlyoutPresenter` styles omit it; including it
here costs six lines and makes `TimePickerFlyout.Title` actually render. Uno null-guards the lookup,
so its absence was silent rather than fatal — which is why it was easy to miss.

## Known gaps / follow-ups

- **Cupertino** still has no `TimePicker` style. Out of scope for this change.
- **Material v1** is unchanged; its `TimePicker.xaml` keeps the colon divider, the iOS-only presenter
  style, and the missing named parts.
- `TimePickerSamplePage.xaml.cs` still carries a `#if !__WASM__ && !__MACOS__` guard predating this
  change. It was left in place because re-enabling it could not be verified on those heads here.

## Verification

- `dotnet build Uno.Themes-packages.slnf -c Debug` → **0 errors**; warnings all pre-existing
  (`Uno0001` not-implemented notices and `CS0618` obsolete-`MaterialResources` notices), none
  referencing TimePicker.
- `dotnet build src/samples/SimpleSampleApp/SimpleSampleApp.csproj -c Debug -f net10.0-desktop`
  → **0 errors**.
- `dotnet build src/samples/MaterialSampleApp/MaterialSampleApp.csproj -c Debug -f net10.0-desktop`
  → **0 errors**.
- Merged dictionaries confirmed to include both new files
  (`Uno.Material/Generated/mergedpages.v2.xaml`, `Uno.Simple.WinUI/Generated/mergedpages.xaml`).
- Runtime tests, desktop head, filtered to `Given_TimePickerStyles`: **18/18 passed**.
- Full runtime-test suite: **111 passed, 0 failed, 1 skipped**. The skip is the pre-existing
  `Given_HotReload.When_BaseThemeIsCollected_Then_HotReloadHandlerDoesNotResurrectIt`, which carries
  an `[Ignore]` attribute predating this change.

### Running the tests locally on Windows

`build/scripts/linux-skia-desktop-runtime-tests.sh` is Linux-only (`xvfb-run`). On Windows, run the
built DLL directly — and note that **`--runtime-tests=<path>` alone is not enough** for engine
2.0.0-dev.60: `RuntimeTestEmbeddedRunner.AutoStartTests` reads `UNO_RUNTIME_TESTS_OUTPUT_PATH` /
`UNO_RUNTIME_TESTS_OUTPUT_URL`, and without one it logs
"Application has not been configured with output destination, aborting runtime-test embedded runner"
and just opens the app.

```bash
cd src/samples/SimpleSampleApp/bin/Debug/net10.0-desktop
export DOTNET_MODIFIABLE_ASSEMBLIES=debug
export UNO_RUNTIME_TESTS_RUN_TESTS='{"Filter":{"Value":"Given_TimePickerStyles"},"Attempts":1}'
export UNO_RUNTIME_TESTS_OUTPUT_PATH='results.xml'
dotnet SimpleSampleApp.dll
```

The app does not exit after writing results — kill it before the next build or the output DLLs stay
locked (`MSB3027`).

### XamlStyler caveat

`Settings.XamlStyler` does not set `IndentWithTabs`, so the documented
`dotnet dnx XamlStyler.Console …` command reformats `.xaml` to **4 spaces**, contradicting
`.editorconfig` (`[*.xaml] indent_style = tab`). The three XAML files here were re-tabbed after
styling (N leading spaces → N/4 tabs + N%4 spaces, which preserves attribute alignment) to match
every other file in the repo. Worth fixing in `Settings.XamlStyler` separately.

## Follow-up session — review feedback

Changes made after the first visual review of the sample apps:

1. **Dark selection band invisible.** Simple's `*FlyoutPresenterHighlightFill` used
   `PrimaryVariantLightBrush`, which is `#1E1E1E` in Dark — identical to `SurfaceColor`, i.e. the band
   painted itself onto the flyout background. Both `TimePicker` and `DatePicker` now use
   `SurfaceVariantBrush` (`#F5F5F5` Light, unchanged appearance / `#2C2C2C` Dark).
2. **Field reads as a single value.** The hour / minute / period run is laid out with `Auto` columns in
   a left-aligned grid, and `First|SecondColumnDivider` (typed `UIElement` in the control contract)
   carry the `:` separator instead of a vertical rule. The parts stay owned by the control, so culture
   ordering, `MinuteIncrement` and 24-hour `ClockIdentifier` still apply.
3. **Pickers size to content.** `HorizontalAlignment` `Stretch` → `Left` on all four picker styles, with
   `*FlyoutPresenterMinWidth` as the floor. This also fixes the full-width flyout:
   `TimePickerFlyout` / `DatePickerFlyout` assign `presenter.Width = target.ActualWidth` on opening.
4. **Header no longer rendered twice.** Material: the header is a floating label (Material TextBox
   behaviour) — resting at body size, centered in the field, animating up and scaling to 0.7 once a
   value is set; the separate header-bound placeholder is gone. Simple: the header renders only as the
   in-field placeholder, as `SimpleTextBox` does with `PlaceholderText`; with no header, the control's
   own `hour : minute AM` run remains as the fallback placeholder.

Runtime tests: `Given_TimePickerStyles` and `Given_PickerFieldLayout` (sizing + header-renders-once,
both pickers). Full `SimpleSampleApp` suite: 121 passed / 0 failed.

Not verified by tests: the Material floating-label geometry (`*HeaderFloatTranslateY` = -11,
`*HeaderFloatScale` = 0.7) is computed from the 53px field metrics and needs a visual check.
