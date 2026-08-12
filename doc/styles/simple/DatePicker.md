---
uid: Uno.Themes.Simple.Styles.DatePicker
---

# DatePicker Control

## Styles

| Style Key                                      | IsDefaultStyle\* |
|------------------------------------------------|------------------|
| `SimpleDatePickerStyle`                        |                  |
| `SimpleDefaultDatePickerStyle`                 | True             |
| `SimpleDatePickerFlyoutPresenterStyle`         |                  |
| `SimpleDefaultDatePickerFlyoutPresenterStyle`  | True             |
| `SimpleDatePickerFlyoutButtonStyle`            |                  |

IsDefaultStyle\*: Styles in this column will be set as the default implicit style for the matching control

## Lightweight Styling

### Themed Resources (ThemeDictionaries)

#### Semantic Keys (source of truth)

##### FlyoutButton

| Key                                            | Type              | Value                           |
|------------------------------------------------|-------------------|---------------------------------|
| `DatePickerFlyoutButtonBackground`             | `SolidColorBrush` | `SystemControlTransparentBrush` |

##### FlyoutPresenter

| Key                                                 | Type              | Value                           |
|-----------------------------------------------------|-------------------|---------------------------------|
| `DatePickerFlyoutPresenterBackground`               | `SolidColorBrush` | `SurfaceBrush`                  |
| `DatePickerFlyoutPresenterBorderBrush`              | `SolidColorBrush` | `OutlineBrush`                  |
| `DatePickerFlyoutPresenterSpacerFill`               | `SolidColorBrush` | `OutlineBrush`                  |
| `DatePickerFlyoutPresenterHighlightFill`            | `SolidColorBrush` | `SurfaceVariantBrush`           |
| `DatePickerFlyoutPresenterCornerRadius`             | `StaticResource`  | `SimpleRadius400CornerRadius`   |
| `DatePickerFlyoutPresenterWidth`                    | `Double`          | 296                             |
| `DatePickerFlyoutPresenterMinWidth`                 | `Double`          | 296                             |
| `DatePickerFlyoutPresenterMaxHeight`                | `Double`          | 398                             |
| `DatePickerFlyoutPresenterHighlightHeight`          | `StaticResource`  | `SimpleIconLarge`               |
| `DatePickerFlyoutPresenterAcceptDismissHostGridHeight` | `Double`       | 52                              |
| `DatePickerSpacerThemeWidth`                        | `StaticResource`  | `SimpleStrokeBorder`            |
| `DatePickerFlyoutBorderThickness`                   | `StaticResource`  | `SimpleStrokeBorder`            |
| `DatePickerFlyoutPresenterFontFamily`               | `StaticResource`  | `SimpleFontFamily`              |

##### DatePicker Button (field appearance)

| Key                                            | Type              | Value                           |
|------------------------------------------------|-------------------|---------------------------------|
| `DatePickerButtonBackground`                   | `SolidColorBrush` | `SurfaceBrush`                  |
| `DatePickerButtonBackgroundPointerOver`        | `SolidColorBrush` | `SurfaceBrush`                  |
| `DatePickerButtonBackgroundPressed`            | `SolidColorBrush` | `SurfaceBrush`                  |
| `DatePickerButtonBackgroundDisabled`           | `SolidColorBrush` | `OnSurfaceDisabledBrush`        |
| `DatePickerButtonForeground`                   | `SolidColorBrush` | `OnSurfaceBrush`                |
| `DatePickerButtonForegroundPointerOver`        | `SolidColorBrush` | `OnSurfaceBrush`                |
| `DatePickerButtonForegroundPressed`            | `SolidColorBrush` | `OnSurfaceBrush`                |
| `DatePickerButtonForegroundDisabled`           | `SolidColorBrush` | `OnSurfaceDisabledBrush`        |
| `DatePickerButtonBorderBrush`                  | `SolidColorBrush` | `OutlineBrush`                  |
| `DatePickerButtonBorderBrushPointerOver`       | `SolidColorBrush` | `OutlineBrush`                  |
| `DatePickerButtonBorderBrushPressed`           | `SolidColorBrush` | `OutlineBrush`                  |
| `DatePickerButtonBorderBrushDisabled`          | `SolidColorBrush` | `OutlineDisabledBrush`          |

##### Date Text

| Key                                            | Type              | Value                           |
|------------------------------------------------|-------------------|---------------------------------|
| `DatePickerButtonDateTextForeground`           | `SolidColorBrush` | `OnSurfaceBrush`                |
| `DatePickerButtonDateTextForegroundPointerOver`| `SolidColorBrush` | `OnSurfaceBrush`                |
| `DatePickerButtonDateTextForegroundPressed`    | `SolidColorBrush` | `OnSurfaceBrush`                |
| `DatePickerButtonDateTextForegroundDisabled`   | `SolidColorBrush` | `OnSurfaceDisabledBrush`        |

##### Placeholder Text

| Key                                            | Type              | Value                           |
|------------------------------------------------|-------------------|---------------------------------|
| `DatePickerPlaceholderTextForeground`          | `SolidColorBrush` | `OnSurfaceLowBrush`             |

##### Header

| Key                                            | Type              | Value                           |
|------------------------------------------------|-------------------|---------------------------------|
| `DatePickerHeaderForeground`                   | `SolidColorBrush` | `OnSurfaceBrush`                |
| `DatePickerHeaderForegroundDisabled`           | `SolidColorBrush` | `OnSurfaceDisabledBrush`        |

##### Dimensions

| Key                                            | Type              | Value                           |
|------------------------------------------------|-------------------|---------------------------------|
| `DatePickerCornerRadius`                       | `StaticResource`  | `SimpleRadius200CornerRadius`   |
| `DatePickerBorderThemeThickness`               | `StaticResource`  | `SimpleStrokeBorderThickness`   |
| `DatePickerContentMargin`                      | `Thickness`       | 16,10,16,10                     |
| `DatePickerFlyoutButtonPadding`                | `StaticResource`  | `SimpleSpace0Thickness`         |
| `DatePickerMinHeight`                          | `Double`          | 44                              |
| `DatePickerFlyoutButtonOpacityPressed`         | `Double`          | 0.8                             |
| `DatePickerFlyoutButtonOpacityDisabled`        | `Double`          | 0.5                             |
| `DatePickerHeaderMargin`                       | `Thickness`       | 0,0,0,4                         |

#### Backwards-Compat Aliases

| Key                                                 | Type             | Value                                    |
|-----------------------------------------------------|------------------|------------------------------------------|
| `SimpleDatePickerFlyoutPresenterCornerRadius`       | `StaticResource` | `DatePickerFlyoutPresenterCornerRadius`  |
| `SimpleDatePickerFlyoutBorderThickness`             | `StaticResource` | `DatePickerFlyoutBorderThickness`        |
| `SimpleDatePickerCornerRadius`                      | `StaticResource` | `DatePickerCornerRadius`                 |
| `SimpleDatePickerBorderThemeThickness`              | `StaticResource` | `DatePickerBorderThemeThickness`         |
| `SimpleDatePickerFlyoutButtonPadding`               | `StaticResource` | `DatePickerFlyoutButtonPadding`          |

> [!NOTE]
> The field sizes to content and is left-aligned, like the Fluent `DatePicker`, with
> `DatePickerFlyoutPresenterMinWidth` as its floor. This also governs the flyout: `DatePickerFlyout` sizes the
> presenter to the target's `ActualWidth` on opening, so a stretched field produces a stretched flyout. Set
> `HorizontalAlignment="Stretch"` on the control to fill its container instead.
>
> `Header` renders **only** as the in-field placeholder, the way the Simple `TextBox` shows `PlaceholderText` —
> there is no separate header line above the field, and `HeaderTemplate` is honoured. The placeholder is
> coloured by `DatePickerPlaceholderTextForeground`; the value uses `DatePickerButtonDateTextForeground`.
>
> `DatePickerHeaderForeground`, `DatePickerHeaderForegroundDisabled` and `DatePickerHeaderMargin` are
> **deprecated**: they styled the separate header line, which no longer exists. They still resolve, but
> overriding them has no effect.
