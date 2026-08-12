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

**`FlyoutPresenterStyle` setter — required after all.** An earlier revision of this spec claimed
`TimePicker` had no such property, having looked only at `TimePicker.Properties.cs`. It is declared in
`TimePicker.Flyout.cs:56` as an Uno-only DP, exactly mirroring `DatePicker.FlyoutPresenterStyle`, and
`TimePicker.Flyout.cs:97` forwards it to `TimePickerFlyout.TimePickerFlyoutPresenterStyle`. Without the
setter an explicitly-styled `TimePicker` on non-Windows targets falls back to the Fluent presenter and
the entire `TimePickerFlyoutPresenter*` key family is dead. Both new styles now carry
`not_win:Setter Property="FlyoutPresenterStyle"`, matching their `DatePicker` siblings.

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
2. **Field columns follow the control, not the template.** An intermediate revision drifted from the
   "Rectangle dividers" decision above and shipped a literal `:` in `FirstColumnDivider` plus `Auto`
   column widths. Review caught both:
   - `UpdateOrderAndLayout` only toggles divider *visibility*; it never repositions them. With
     `periodOrder == 0` (ko-KR / zh-CN / ja-JP) the period occupies `FirstPickerHost`, so the colon
     rendered between period and hour and the hour/minute pair had none.
   - The same method assigns `1*` to every populated `*TextBlockColumn` on each pass, so the `Auto`
     widths — and the "single compact value" behaviour attributed to them — never existed at runtime.

   Restored to the original decision: neutral `Rectangle` dividers and `*` columns, matching Fluent.
   The parts stay owned by the control, so culture ordering, `MinuteIncrement` and 24-hour
   `ClockIdentifier` still apply.
3. **Pickers size to content.** `HorizontalAlignment` `Stretch` → `Left` on all four picker styles, with
   `*FlyoutPresenterMinWidth` as the floor. This also fixes the full-width flyout:
   `TimePickerFlyout` / `DatePickerFlyout` assign `presenter.Width = target.ActualWidth` on opening.
4. **Header no longer rendered twice.** Material: the header is a floating label (Material TextBox
   behaviour); the separate header-bound placeholder is gone. Simple: the header renders only as the
   in-field placeholder, as `SimpleTextBox` does with `PlaceholderText`; with no header, the control's
   own hour / minute / period run remains as the fallback placeholder. Both themes render the header
   through a `ContentPresenter`, so a non-string `Header` and `HeaderTemplate` are honoured rather than
   silently `ToString()`-ed and dropped.

5. **Header float is layout-driven, not a magic offset.** The first attempt translated the label by a
   hand-computed `-11` derived from the 53px field height, which was never pixel-verified and broke the
   header-less case: with `ContentMargin` `10,24,10,0` and `VerticalAlignment="Top"`, a bare
   `<TimePicker />` rendered its value against the bottom edge. Header and value now occupy two `Auto`
   rows centred in the field, so all three combinations fall out of layout — no header (label collapses,
   value centres), header without value (label centres as the placeholder), and both (they stack).
   `*HeaderFloatTranslateY` is gone; only `*HeaderFloatScale` remains, and it lives at dictionary scope
   because the Storyboard reads it through `StaticResource`, which does not re-resolve per
   `ThemeDictionary`.

## Declined findings

**Deduplicating the four flyout-button / flyout-presenter styles.** Review flagged
`Material|Simple {Date,Time}PickerFlyoutButtonStyle` as near-identical copies differing only by key
prefix. They must stay separate: each reads its own `{Date,Time}PickerFlyoutButton*` key family, and a
shared `BasedOn` base would have to pick one family, silently breaking per-control lightweight styling —
the documented contract in `doc/lightweight-styling.md`. The duplication is the contract, not an
accident. The same applies to the two float Storyboards, which read `{Date,Time}PickerHeaderFloatScale`.

## Verification

Runtime tests: `Given_TimePickerStyles` and `Given_PickerFieldLayout` (SimpleSampleApp), and
`Given_MaterialPickerStyles` (MaterialSampleApp) — the Material templates previously had no coverage at
all. New guards worth calling out:

- Selection-band contrast is now asserted under Light **and** Dark by resolving the brushes through a
  themed, loaded element. The previous version used the `ResourceDictionary` indexer, which resolves
  ThemeDictionaries against the *application* theme — both `DataRow`s asserted the same value, and
  because Light already contrasted (`#F5F5F5` vs `#FFFFFF`) the test passed on `master` and never
  reproduced the Dark bug it was written for.
- Divider type (`not a TextBlock`) and control-assigned `1*` column widths, pinning the two behaviours
  that the culture and layout defects above turned on.
- Header-less vertical centring, the case the float offset broke.
