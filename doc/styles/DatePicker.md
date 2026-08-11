---
uid: Uno.Themes.Styles.DatePicker
---

# DatePicker Control

## Styles

| Style Key         | IsDefaultStyle\* |
|-------------------|------------------|
| `DatePickerStyle` | True             |

IsDefaultStyle\*: Styles in this column will be set as the default implicit style for the matching control

## Lightweight Styling

| Key                                                    | Type              | Value                           |
|--------------------------------------------------------|-------------------|---------------------------------|
| `DatePickerFlyoutButtonBackground`                     | `SolidColorBrush` | `SystemControlTransparentBrush` |
| `DatePickerFlyoutPresenterBackground`                  | `SolidColorBrush` | `SurfaceBrush`                  |
| `DatePickerFlyoutPresenterBorderBrush`                 | `SolidColorBrush` | `OnSurfaceFocusedBrush`         |
| `DatePickerFlyoutPresenterSpacerFill`                  | `SolidColorBrush` | `OnSurfaceFocusedBrush`         |
| `DatePickerFlyoutPresenterHighlightFill`               | `SolidColorBrush` | `PrimarySelectedBrush`          |
| `DatePickerFlyoutPresenterCornerRadius`                | `CornerRadius`    | `OverlayCornerRadius`           |
| `DatePickerButtonBackground`                           | `SolidColorBrush` | `SurfaceVariantBrush`           |
| `DatePickerButtonBackgroundPointerOver`                | `SolidColorBrush` | `SurfaceVariantBrush`           |
| `DatePickerButtonBackgroundPressed`                    | `SolidColorBrush` | `SurfaceVariantBrush`           |
| `DatePickerButtonBackgroundDisabled`                   | `SolidColorBrush` | `PrimaryFocusedBrush`           |
| `DatePickerButtonForeground`                           | `SolidColorBrush` | `PrimaryBrush`                  |
| `DatePickerButtonForegroundPointerOver`                | `SolidColorBrush` | `PrimaryBrush`                  |
| `DatePickerButtonForegroundPressed`                    | `SolidColorBrush` | `PrimaryBrush`                  |
| `DatePickerButtonForegroundDisabled`                   | `SolidColorBrush` | `OnSurfaceLowBrush`             |
| `DatePickerPlaceholderTextForeground`                  | `SolidColorBrush` | `OnSurfaceLowBrush`             |
| `DatePickerButtonDateTextForeground`                   | `SolidColorBrush` | `OnSurfaceVariantBrush`         |
| `DatePickerButtonDateTextForegroundPointerOver`        | `SolidColorBrush` | `OnSurfaceVariantBrush`         |
| `DatePickerButtonDateTextForegroundPressed`            | `SolidColorBrush` | `OnSurfaceVariantBrush`         |
| `DatePickerButtonDateTextForegroundDisabled`           | `SolidColorBrush` | `OnSurfaceLowBrush`             |
| `DatePickerButtonBorderBrush`                          | `SolidColorBrush` | `PrimaryBrush`                  |
| `DatePickerButtonBorderBrushPointerOver`               | `SolidColorBrush` | `PrimaryBrush`                  |
| `DatePickerButtonBorderBrushPressed`                   | `SolidColorBrush` | `PrimaryBrush`                  |
| `DatePickerButtonBorderBrushDisabled`                  | `SolidColorBrush` | `OnSurfaceLowBrush`             |
| `DatePickerFlyoutPresenterFontFamily`                  | `FontFamily`      | `MaterialRegularFontFamily`     |
| `DatePickerFlyoutPresenterFontSize`                    | `Double`          | `ControlContentThemeFontSize`   |
| `DatePickerFlyoutBorderThickness`                      | `Double`          | 1                               |
| `DatePickerSpacerThemeWidth`                           | `Double`          | 1                               |
| `DatePickerHeight`                                     | `Double`          | 53                              |
| `DatePickerFlyoutElevation`                            | `Double`          | 8                               |
| `DatePickerFlyoutButtonOpacityPressed`                 | `Double`          | 0.65                            |
| `DatePickerFlyoutButtonOpacityDisabled`                | `Double`          | 0.65                            |
| `DatePickerFlyoutPresenterWidth`                       | `Double`          | 296                             |
| `DatePickerFlyoutPresenterMinWidth`                    | `Double`          | 296                             |
| `DatePickerFlyoutPresenterMaxHeight`                   | `Double`          | 398                             |
| `DatePickerFlyoutPresenterAcceptDismissHostGridHeight` | `Double`          | 41                              |
| `DatePickerFlyoutPresenterHighlightHeight`             | `Double`          | 40                              |
| `DatePickerButtonBottomBorderHeight`                   | `Double`          | 2                               |
| `DatePickerButtonContentHeight`                        | `Double`          | 24                              |
| `DatePickerButtonHeaderMargin`                         | `Thickness`       | 10,8,10,0                       |
| `DatePickerHeaderFloatTranslateY`                      | `Double`          | `-11`                           |
| `DatePickerHeaderFloatScale`                           | `Double`          | `0.7`                           |
| `DatePickerButtonPlaceholderMargin`                    | `Thickness`       | 10,0,10,0                         |
| `DatePickerButtonContentMargin`                        | `Thickness`       | 6,24,10,0                       |
| `DatePickerHostPadding`                                | `Thickness`       | 24,24,8,8                       |
| `DatePickerFlyoutButtonPadding`                        | `Thickness`       | 0                               |
| `DatePickerCornerRadius`                               | `CornerRadius`    | 4,4,0,0                         |

> [!NOTE]
> The field sizes to content and is left-aligned, like the Fluent `DatePicker`, with
> `DatePickerFlyoutPresenterMinWidth` as its floor. This also governs the flyout: `DatePickerFlyout` sizes the
> presenter to the target's `ActualWidth` on opening, so a stretched field produces a stretched flyout. Set
> `HorizontalAlignment="Stretch"` on the control to fill its container instead.

> [!NOTE]
> `Header` is a floating label, as in the Material `TextBox`: it rests at the value's position and reads as the
> placeholder while no date is set, then animates up and shrinks once one is picked. It is the only element
> rendering the header — there is no separate header-bound placeholder. Tune the motion with
> `DatePickerHeaderFloatTranslateY` / `DatePickerHeaderFloatScale`.
