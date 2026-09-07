---
uid: Uno.Themes.Simple.Styles.ToggleButton
---

# ToggleButton Control

Both Simple variants consume `TextToggleButton*` brush resources. The separate icon template does not read Material's `IconToggleButton*` brush family, so shared overrides using those keys are ineffective under Simple. The default style derives from the text variant; see [semantic override compatibility](../../semantic-styles.md#override-compatibility).

## Styles

| Style Key                          | IsDefaultStyle\* |
|------------------------------------|------------------|
| `SimpleDefaultToggleButtonStyle`   | True             |
| `SimpleTextToggleButtonStyle`      |                  |
| `SimpleIconToggleButtonStyle`      |                  |

IsDefaultStyle\*: Styles in this column will be set as the default implicit style for the matching control

## Lightweight Styling

| Key                                          | Type              | Value                            |
|----------------------------------------------|-------------------|----------------------------------|
| `SimpleToggleButtonCornerRadius`             | `CornerRadius`    | `SimpleRadius200CornerRadius`    |
| `SimpleToggleButtonBorderThickness`          | `Thickness`       | `SimpleStrokeBorderThickness`    |
| `SimpleToggleButtonMediumPadding`            | `Thickness`       | `SimpleSpace300Thickness`        |
| `SimpleToggleButtonSmallPadding`             | `Thickness`       | `SimpleSpace200Thickness`        |
| `SimpleToggleButtonMediumMinHeight`          | `Double`          | `SimpleIconLarge`                |
| `SimpleToggleButtonSmallMinHeight`           | `Double`          | `SimpleIconMedium`               |
| `SimpleToggleButtonMediumFontSize`           | `Double`          | `BodyLargeFontSize`              |
| `SimpleToggleButtonSmallFontSize`            | `Double`          | `BodyMediumFontSize`             |
| `SimpleToggleButtonIconSpacing`              | `Double`          | `SimpleSpace200`                 |
| `SimpleToggleButtonFontFamily`               | `FontFamily`      | Inter                            |
| `SimpleToggleButtonFontWeight`               | `String`          | Medium                           |

### Unchecked

| Key                                                        | Type              | Value                        |
|------------------------------------------------------------|-------------------|------------------------------|
| `TextToggleButtonForeground`                               | `SolidColorBrush` | `OnSurfaceBrush`             |
| `TextToggleButtonForegroundPointerOver`                    | `SolidColorBrush` | `OnSurfaceBrush`             |
| `TextToggleButtonForegroundPressed`                        | `SolidColorBrush` | `OnSurfaceBrush`             |
| `TextToggleButtonForegroundDisabled`                       | `SolidColorBrush` | `OnSurfaceDisabledBrush`     |
| `TextToggleButtonBackground`                               | `SolidColorBrush` | `SurfaceVariantBrush`        |
| `TextToggleButtonBackgroundPointerOver`                    | `SolidColorBrush` | `PrimaryContainerBrush`      |
| `TextToggleButtonBackgroundPressed`                        | `SolidColorBrush` | `TertiaryContainerBrush`     |
| `TextToggleButtonBackgroundDisabled`                       | `SolidColorBrush` | `OnSurfaceDisabledBrush`     |
| `TextToggleButtonBorderBrush`                              | `SolidColorBrush` | `OutlineBrush`               |
| `TextToggleButtonBorderBrushPointerOver`                   | `SolidColorBrush` | `OutlineBrush`               |
| `TextToggleButtonBorderBrushPressed`                       | `SolidColorBrush` | `OutlineBrush`               |
| `TextToggleButtonBorderBrushDisabled`                      | `SolidColorBrush` | `OutlineDisabledBrush`       |

### Checked

| Key                                                        | Type              | Value                        |
|------------------------------------------------------------|-------------------|------------------------------|
| `TextToggleButtonForegroundChecked`                        | `SolidColorBrush` | `OnPrimaryBrush`             |
| `TextToggleButtonForegroundCheckedPointerOver`             | `SolidColorBrush` | `OnPrimaryBrush`             |
| `TextToggleButtonForegroundCheckedPressed`                 | `SolidColorBrush` | `OnPrimaryBrush`             |
| `TextToggleButtonForegroundCheckedDisabled`                | `SolidColorBrush` | `OnSurfaceDisabledBrush`     |
| `TextToggleButtonBackgroundChecked`                        | `SolidColorBrush` | `PrimaryBrush`               |
| `TextToggleButtonBackgroundCheckedPointerOver`             | `SolidColorBrush` | `PrimaryVariantDarkBrush`    |
| `TextToggleButtonBackgroundCheckedPressed`                 | `SolidColorBrush` | `PrimaryVariantDarkBrush`    |
| `TextToggleButtonBackgroundCheckedDisabled`                | `SolidColorBrush` | `OnSurfaceDisabledBrush`     |
| `TextToggleButtonBorderBrushChecked`                       | `SolidColorBrush` | `PrimaryBrush`               |
| `TextToggleButtonBorderBrushCheckedPointerOver`            | `SolidColorBrush` | `PrimaryBrush`               |
| `TextToggleButtonBorderBrushCheckedPressed`                | `SolidColorBrush` | `PrimaryBrush`               |
| `TextToggleButtonBorderBrushCheckedDisabled`               | `SolidColorBrush` | `OutlineDisabledBrush`       |

### Indeterminate

| Key                                                            | Type              | Value                        |
|----------------------------------------------------------------|-------------------|------------------------------|
| `TextToggleButtonForegroundIndeterminate`                      | `SolidColorBrush` | `OnSurfaceBrush`             |
| `TextToggleButtonForegroundIndeterminatePointerOver`           | `SolidColorBrush` | `OnSurfaceBrush`             |
| `TextToggleButtonForegroundIndeterminatePressed`               | `SolidColorBrush` | `OnSurfaceBrush`             |
| `TextToggleButtonForegroundIndeterminateDisabled`              | `SolidColorBrush` | `OnSurfaceDisabledBrush`     |
| `TextToggleButtonBackgroundIndeterminate`                      | `SolidColorBrush` | `SurfaceVariantBrush`        |
| `TextToggleButtonBackgroundIndeterminatePointerOver`           | `SolidColorBrush` | `PrimaryContainerBrush`      |
| `TextToggleButtonBackgroundIndeterminatePressed`               | `SolidColorBrush` | `TertiaryContainerBrush`     |
| `TextToggleButtonBackgroundIndeterminateDisabled`              | `SolidColorBrush` | `OnSurfaceDisabledBrush`     |
| `TextToggleButtonBorderBrushIndeterminate`                     | `SolidColorBrush` | `OutlineBrush`               |
| `TextToggleButtonBorderBrushIndeterminatePointerOver`          | `SolidColorBrush` | `OutlineBrush`               |
| `TextToggleButtonBorderBrushIndeterminatePressed`              | `SolidColorBrush` | `OutlineBrush`               |
| `TextToggleButtonBorderBrushIndeterminateDisabled`             | `SolidColorBrush` | `OutlineDisabledBrush`       |
