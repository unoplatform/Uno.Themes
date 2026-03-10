---
uid: Uno.Themes.Simple.Styles.ToggleButton
---

# ToggleButton Control

## Styles

| Style Key                          | IsDefaultStyle\* |
|------------------------------------|------------------|
| `SimpleDefaultToggleButtonStyle`   | True             |
| `SimpleSmallToggleButtonStyle`     |                  |
| `SimpleMediumToggleButtonStyle`    |                  |

IsDefaultStyle\*: Styles in this column will be set as the default implicit style for the matching control

## Lightweight Styling

| Key                                          | Type              | Value                            |
|----------------------------------------------|-------------------|----------------------------------|
| `SimpleToggleButtonCornerRadius`             | `CornerRadius`    | `SimpleRadius200CornerRadius` (8)    |
| `SimpleToggleButtonBorderThickness`          | `Thickness`       | `SimpleStrokeBorderThickness` (1)    |
| `SimpleToggleButtonMediumPadding`            | `Thickness`       | `SimpleSpace300Thickness` (12)        |
| `SimpleToggleButtonSmallPadding`             | `Thickness`       | `SimpleSpace200Thickness` (8)        |
| `SimpleToggleButtonMediumMinHeight`          | `Double`          | `SimpleIconLarge` (40)                |
| `SimpleToggleButtonSmallMinHeight`           | `Double`          | `SimpleIconMedium` (32)               |
| `SimpleToggleButtonMediumFontSize`           | `Double`          | `SimpleTypographyScale03` (16)        |
| `SimpleToggleButtonSmallFontSize`            | `Double`          | `SimpleTypographyScale02` (14)        |
| `SimpleToggleButtonIconSpacing`              | `Double`          | `SimpleSpace200` (8)                 |
| `SimpleToggleButtonFontFamily`               | `FontFamily`      | Inter                            |

### Unchecked

| Key                                                        | Type              | Value                                              |
|------------------------------------------------------------|-------------------|----------------------------------------------------|
| `SimpleToggleButtonForeground`                             | `SolidColorBrush` | `SimpleTextDefaultDefaultBrush`                    |
| `SimpleToggleButtonForegroundPointerOver`                  | `SolidColorBrush` | `SimpleTextDefaultDefaultBrush`                    |
| `SimpleToggleButtonForegroundPressed`                      | `SolidColorBrush` | `SimpleTextDefaultDefaultBrush`                    |
| `SimpleToggleButtonForegroundDisabled`                     | `SolidColorBrush` | `SimpleTextDisabledDefaultBrush`                   |
| `SimpleToggleButtonBackground`                             | `SolidColorBrush` | `SimpleBackgroundDefaultSecondaryBrush`            |
| `SimpleToggleButtonBackgroundPointerOver`                  | `SolidColorBrush` | `SimpleBackgroundDefaultSecondaryHoverBrush`       |
| `SimpleToggleButtonBackgroundPressed`                      | `SolidColorBrush` | `SimpleBackgroundDefaultSecondaryPressedBrush`     |
| `SimpleToggleButtonBackgroundDisabled`                     | `SolidColorBrush` | `SimpleBackgroundDisabledDefaultBrush`             |
| `SimpleToggleButtonBorderBrush`                            | `SolidColorBrush` | `SimpleBorderDefaultDefaultBrush`                  |
| `SimpleToggleButtonBorderBrushPointerOver`                 | `SolidColorBrush` | `SimpleBorderDefaultDefaultBrush`                  |
| `SimpleToggleButtonBorderBrushPressed`                     | `SolidColorBrush` | `SimpleBorderDefaultDefaultBrush`                  |
| `SimpleToggleButtonBorderBrushDisabled`                    | `SolidColorBrush` | `SimpleBorderDisabledDefaultBrush`                 |

### Checked

| Key                                                        | Type              | Value                                      |
|------------------------------------------------------------|-------------------|--------------------------------------------|
| `SimpleToggleButtonForegroundChecked`                      | `SolidColorBrush` | `SimpleTextBrandOnBrandBrush`              |
| `SimpleToggleButtonForegroundCheckedPointerOver`           | `SolidColorBrush` | `SimpleTextBrandOnBrandBrush`              |
| `SimpleToggleButtonForegroundCheckedPressed`               | `SolidColorBrush` | `SimpleTextBrandOnBrandBrush`              |
| `SimpleToggleButtonForegroundCheckedDisabled`              | `SolidColorBrush` | `SimpleTextDisabledDefaultBrush`           |
| `SimpleToggleButtonBackgroundChecked`                      | `SolidColorBrush` | `SimpleBackgroundBrandDefaultBrush`        |
| `SimpleToggleButtonBackgroundCheckedPointerOver`           | `SolidColorBrush` | `SimpleBackgroundBrandHoverBrush`          |
| `SimpleToggleButtonBackgroundCheckedPressed`               | `SolidColorBrush` | `SimpleBackgroundBrandPressedBrush`        |
| `SimpleToggleButtonBackgroundCheckedDisabled`              | `SolidColorBrush` | `SimpleBackgroundDisabledDefaultBrush`     |
| `SimpleToggleButtonBorderBrushChecked`                     | `SolidColorBrush` | `SimpleBorderBrandDefaultBrush`            |
| `SimpleToggleButtonBorderBrushCheckedPointerOver`          | `SolidColorBrush` | `SimpleBorderBrandDefaultBrush`            |
| `SimpleToggleButtonBorderBrushCheckedPressed`              | `SolidColorBrush` | `SimpleBorderBrandDefaultBrush`            |
| `SimpleToggleButtonBorderBrushCheckedDisabled`             | `SolidColorBrush` | `SimpleBorderDisabledDefaultBrush`         |

### Indeterminate

| Key                                                            | Type              | Value                                              |
|----------------------------------------------------------------|-------------------|----------------------------------------------------|
| `SimpleToggleButtonForegroundIndeterminate`                    | `SolidColorBrush` | `SimpleTextDefaultDefaultBrush`                    |
| `SimpleToggleButtonForegroundIndeterminatePointerOver`         | `SolidColorBrush` | `SimpleTextDefaultDefaultBrush`                    |
| `SimpleToggleButtonForegroundIndeterminatePressed`             | `SolidColorBrush` | `SimpleTextDefaultDefaultBrush`                    |
| `SimpleToggleButtonForegroundIndeterminateDisabled`            | `SolidColorBrush` | `SimpleTextDisabledDefaultBrush`                   |
| `SimpleToggleButtonBackgroundIndeterminate`                    | `SolidColorBrush` | `SimpleBackgroundDefaultSecondaryBrush`            |
| `SimpleToggleButtonBackgroundIndeterminatePointerOver`         | `SolidColorBrush` | `SimpleBackgroundDefaultSecondaryHoverBrush`       |
| `SimpleToggleButtonBackgroundIndeterminatePressed`             | `SolidColorBrush` | `SimpleBackgroundDefaultSecondaryPressedBrush`     |
| `SimpleToggleButtonBackgroundIndeterminateDisabled`            | `SolidColorBrush` | `SimpleBackgroundDisabledDefaultBrush`             |
| `SimpleToggleButtonBorderBrushIndeterminate`                   | `SolidColorBrush` | `SimpleBorderDefaultDefaultBrush`                  |
| `SimpleToggleButtonBorderBrushIndeterminatePointerOver`        | `SolidColorBrush` | `SimpleBorderDefaultDefaultBrush`                  |
| `SimpleToggleButtonBorderBrushIndeterminatePressed`            | `SolidColorBrush` | `SimpleBorderDefaultDefaultBrush`                  |
| `SimpleToggleButtonBorderBrushIndeterminateDisabled`           | `SolidColorBrush` | `SimpleBorderDisabledDefaultBrush`                 |
