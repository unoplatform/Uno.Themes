---
uid: Uno.Themes.Styles.TimePicker
---

# TimePicker Control

## Styles

| Style Key                            | IsDefaultStyle\* |
|--------------------------------------|------------------|
| `TimePickerStyle`                    | True             |
| `TimePickerFlyoutPresenterStyle`     | True             |

IsDefaultStyle\*: Styles in this column will be set as the default implicit style for the matching control

## Lightweight Styling

| Key                                                   | Type              | Value                           |
|-------------------------------------------------------|-------------------|---------------------------------|
| `TimePickerFlyoutButtonBackground`                    | `SolidColorBrush` | `SystemControlTransparentBrush` |
| `TimePickerFlyoutPresenterBackground`                 | `SolidColorBrush` | `SurfaceBrush`                  |
| `TimePickerFlyoutPresenterForeground`                 | `SolidColorBrush` | `OnSurfaceBrush`                |
| `TimePickerFlyoutPresenterBorderBrush`                | `SolidColorBrush` | `OnSurfaceFocusedBrush`         |
| `TimePickerFlyoutPresenterSpacerFill`                 | `SolidColorBrush` | `OnSurfaceFocusedBrush`         |
| `TimePickerFlyoutPresenterHighlightFill`              | `SolidColorBrush` | `PrimarySelectedBrush`          |
| `TimePickerFlyoutPresenterCornerRadius`               | `CornerRadius`    | `OverlayCornerRadius`           |
| `TimePickerButtonBackground`                          | `SolidColorBrush` | `SurfaceVariantBrush`           |
| `TimePickerButtonBackgroundDisabled`                  | `SolidColorBrush` | `PrimaryFocusedBrush`           |
| `TimePickerHeaderForeground`                          | `SolidColorBrush` | `PrimaryBrush`                  |
| `TimePickerHeaderForegroundDisabled`                  | `SolidColorBrush` | `OnSurfaceLowBrush`             |
| `TimePickerButtonTimeTextForeground`                  | `SolidColorBrush` | `OnSurfaceVariantBrush`         |
| `TimePickerButtonTimeTextForegroundDisabled`          | `SolidColorBrush` | `OnSurfaceLowBrush`             |
| `TimePickerPlaceholderTextForeground`                 | `SolidColorBrush` | `OnSurfaceLowBrush`             |
| `TimePickerButtonBorderBrush`                         | `SolidColorBrush` | `PrimaryBrush`                  |
| `TimePickerButtonBorderBrushDisabled`                 | `SolidColorBrush` | `OnSurfaceLowBrush`             |
| `TimePickerSpacerFill`                                | `SolidColorBrush` | `OnSurfaceVariantBrush`         |
| `TimePickerSpacerFillDisabled`                        | `SolidColorBrush` | `OnSurfaceLowBrush`             |
| `TimePickerFlyoutPresenterFontFamily`                 | `FontFamily`      | `MaterialRegularFontFamily`     |
| `TimePickerFlyoutPresenterFontSize`                   | `Double`          | `ControlContentThemeFontSize`   |
| `TimePickerFlyoutBorderThickness`                     | `Double`          | `1`                             |
| `TimePickerSpacerThemeWidth`                          | `Double`          | `1`                             |
| `TimePickerColumnDividerWidth`                        | `Double`          | `1`                             |
| `TimePickerHeight`                                    | `Double`          | `53`                            |
| `TimePickerFlyoutElevation`                           | `Double`          | `8`                             |
| `TimePickerFlyoutButtonOpacityPressed`                | `Double`          | `0.65`                          |
| `TimePickerFlyoutButtonOpacityDisabled`               | `Double`          | `0.65`                          |
| `TimePickerFlyoutPresenterWidth`                      | `Double`          | `242`                           |
| `TimePickerFlyoutPresenterMinWidth`                   | `Double`          | `242`                           |
| `TimePickerFlyoutPresenterMaxHeight`                  | `Double`          | `398`                           |
| `TimePickerFlyoutPresenterAcceptDismissHostGridHeight`| `Double`          | `41`                            |
| `TimePickerFlyoutPresenterHighlightHeight`            | `Double`          | `ControlHeightMedium`           |
| `TimePickerButtonBottomBorderHeight`                  | `Double`          | `2`                             |
| `TimePickerButtonContentHeight`                       | `Double`          | `IconSizeMedium`                |
| `TimePickerButtonPlaceholderMargin`                   | `Thickness`       | `10,0,10,0`                     |
| `TimePickerHeaderFloatScale`                          | `Double`          | `0.7`                           |
| `TimePickerButtonContentMargin`                       | `Thickness`       | `10,0,10,0`                     |
| `TimePickerFlyoutPresenterMaxWidth`                   | `Double`          | `456`                           |
| `TimePickerColumnDividerMargin`                       | `Thickness`       | `2,0,2,0`                       |
| `TimePickerFlyoutPresenterTitleMargin`                | `Thickness`       | `16,12,16,4`                    |
| `TimePickerFlyoutButtonPadding`                       | `Thickness`       | `0`                             |
| `TimePickerCornerRadius`                              | `CornerRadius`    | `4,4,0,0`                       |

> [!NOTE]
> The field sizes to content and is left-aligned, like the Fluent `TimePicker`, with
> `TimePickerFlyoutPresenterMinWidth` as its floor. This also governs the flyout: `TimePickerFlyout` sizes the
> presenter to the target's `ActualWidth` on opening, so a stretched field produces a stretched flyout. Set
> `HorizontalAlignment="Stretch"` on the control to fill its container instead.
>
> The hour / minute / period text stays owned by the control, so culture ordering, `MinuteIncrement` and a
> 24-hour `ClockIdentifier` all still apply. The columns are separated by neutral rules styled with
> `TimePickerColumnDividerWidth`, `TimePickerColumnDividerMargin` and `TimePickerSpacerFill`. They are rules
> rather than a `:` glyph on purpose: the control reorders the hour / minute / period hosts per culture but
> never moves the dividers, so a literal separator is correct only while the hour happens to come first — in a
> period-first culture such as `ko-KR` it would land between the period and the hour.
>
> `TimePicker` only reports the `Normal` / `Disabled` and `HasTime` / `HasNoTime` visual states — it has no
> `PointerOver` or `Pressed` state of its own (unlike `DatePicker`). Pressed and disabled feedback comes from the
> `TimePickerFlyoutButtonOpacity*` keys applied to the flyout button that wraps the field.
>
> `Header` is a floating label, as in the Material `TextBox`. It and the value occupy two `Auto` rows centred in
> the field: with no `Header` the label collapses and the value centres, with a `Header` but no time the label
> centres and reads as the placeholder, and with both they stack. It is the only element rendering the header —
> there is no separate header-bound placeholder. `HeaderTemplate` is honoured. `TimePickerHeaderFloatScale` sets
> how far the label shrinks once it floats; the vertical movement is layout-driven, so there is no offset key to
> keep in sync with the field height.

## See also

- [DatePicker](DatePicker.md)
- [Lightweight Styling](xref:Uno.Themes.LightweightStyling)
