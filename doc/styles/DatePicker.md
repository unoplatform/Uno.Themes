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
| `DatePickerButtonHeaderMargin` *(deprecated)*          | `Thickness`       | 10,8,10,0                       |
| `DatePickerHeaderFloatScale`                           | `Double`          | `0.7`                           |
| `DatePickerButtonPlaceholderMargin`                    | `Thickness`       | 10,0,10,0                       |
| `DatePickerButtonContentMargin`                        | `Thickness`       | 10,0,10,0                       |
| `DatePickerHostPadding`                                | `Thickness`       | 24,24,8,8                       |
| `DatePickerFlyoutButtonPadding`                        | `Thickness`       | 0                               |
| `DatePickerCornerRadius`                               | `CornerRadius`    | 4,4,0,0                         |

> [!NOTE]
> The field sizes to content and is left-aligned, like the Fluent `DatePicker`, with
> `DatePickerFlyoutPresenterMinWidth` as its floor. This also governs the flyout: `DatePickerFlyout` sizes the
> presenter to the target's `ActualWidth` on opening, so a stretched field produces a stretched flyout. Set
> `HorizontalAlignment="Stretch"` on the control to fill its container instead.
>
> `Header` is a floating label, as in the Material `TextBox`. It and the value occupy two `Auto` rows centred in
> the field: with no `Header` the label collapses and the value centres, with a `Header` but no date the label
> centres and reads as the placeholder, and with both they stack. It is the only element rendering the header —
> there is no separate header-bound placeholder. `HeaderTemplate` is honoured. `DatePickerHeaderFloatScale` sets
> how far the label shrinks once it floats; the vertical movement is layout-driven, so there is no offset key to
> keep in sync with the field height.
>
> `DatePickerButtonHeaderMargin` is **deprecated**: it positioned the separate header line, which no longer
> exists. It still resolves — and `Uno.Themes.WinUI.Markup` still exposes it as `DatePicker.Button.HeaderMargin`
> — but overriding it has no effect. Use `DatePickerButtonPlaceholderMargin` to inset the header instead.
