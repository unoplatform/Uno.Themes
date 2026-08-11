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
| `TimePickerButtonHeaderMargin`                        | `Thickness`       | `10,8,10,0`                     |
| `TimePickerButtonPlaceholderMargin`                   | `Thickness`       | `10,0,10,0`                     |
| `TimePickerHeaderFloatTranslateY`                     | `Double`          | `-11`                           |
| `TimePickerHeaderFloatScale`                          | `Double`          | `0.7`                           |
| `TimePickerButtonContentMargin`                       | `Thickness`       | `10,24,10,0`                    |
| `TimePickerColumnDividerMargin`                       | `Thickness`       | `2,0,2,0`                       |
| `TimePickerFlyoutPresenterTitleMargin`                | `Thickness`       | `16,12,16,4`                    |
| `TimePickerFlyoutButtonPadding`                       | `Thickness`       | `0`                             |
| `TimePickerCornerRadius`                              | `CornerRadius`    | `4,4,0,0`                       |

> [!NOTE]
> The field sizes to content and is left-aligned, like the Fluent `TimePicker`, with
> `TimePickerFlyoutPresenterMinWidth` as its floor. This also governs the flyout: `TimePickerFlyout` sizes the
> presenter to the target's `ActualWidth` on opening, so a stretched field produces a stretched flyout. Set
> `HorizontalAlignment="Stretch"` on the control to fill its container instead.

> [!NOTE]
> The field renders the selected time as a single left-aligned value (`9:41 AM`), like the `DatePicker` field.
> The hour / minute / period text remains owned by the control, so culture ordering, `MinuteIncrement` and a
> 24-hour `ClockIdentifier` all still apply; `TimePickerColumnDividerMargin` spaces the `:` separator and the
> period, and `TimePickerSpacerFill` colors the separator. `TimePickerColumnDividerWidth` is kept for
> back-compat but is no longer used by the default template (the separator is text, not a rule).

> [!NOTE]
> `TimePicker` only reports the `Normal` / `Disabled` and `HasTime` / `HasNoTime` visual states — it has no
> `PointerOver` or `Pressed` state of its own (unlike `DatePicker`). Pressed and disabled feedback comes from the
> `TimePickerFlyoutButtonOpacity*` keys applied to the flyout button that wraps the field.

> [!NOTE]
> `Header` is a floating label, as in the Material `TextBox`: it rests at the value's position and reads as the
> placeholder while no time is set, then animates up and shrinks once one is picked. It is the only element
> rendering the header — there is no separate header-bound placeholder. Tune the motion with
> `TimePickerHeaderFloatTranslateY` / `TimePickerHeaderFloatScale`.

## See also

- [DatePicker](DatePicker.md)
- [Lightweight Styling](xref:Uno.Themes.LightweightStyling)
