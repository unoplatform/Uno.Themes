---
uid: Uno.Themes.Simple.Styles.Button
---

# Button Control

## Styles

| Style Key                                 | IsDefaultStyle\* |
|-------------------------------------------|------------------|
| `SimplePrimaryButtonStyle`                | True             |
| `SimpleNeutralButtonStyle`                |                  |
| `SimpleSubtleButtonStyle`                 |                  |
| `SimpleDangerPrimaryButtonStyle`          |                  |
| `SimpleDangerSubtleButtonStyle`           |                  |
| `SimpleSmallPrimaryButtonStyle`           |                  |
| `SimpleSmallNeutralButtonStyle`           |                  |
| `SimpleSmallSubtleButtonStyle`            |                  |
| `SimpleSmallDangerPrimaryButtonStyle`     |                  |
| `SimpleSmallDangerSubtleButtonStyle`      |                  |
| `SimpleMediumPrimaryButtonStyle`          |                  |
| `SimpleMediumNeutralButtonStyle`          |                  |
| `SimpleMediumSubtleButtonStyle`           |                  |
| `SimpleMediumDangerPrimaryButtonStyle`    |                  |
| `SimpleMediumDangerSubtleButtonStyle`     |                  |
| `SimpleIconButtonPrimaryStyle`            |                  |
| `SimpleIconButtonNeutralStyle`            |                  |
| `SimpleIconButtonSubtleStyle`             |                  |
| `SimpleIconButtonDangerPrimaryStyle`      |                  |
| `SimpleIconButtonDangerSubtleStyle`       |                  |
| `SimpleSmallIconButtonPrimaryStyle`       |                  |
| `SimpleSmallIconButtonNeutralStyle`       |                  |
| `SimpleSmallIconButtonSubtleStyle`        |                  |
| `SimpleSmallIconButtonDangerPrimaryStyle` |                  |
| `SimpleSmallIconButtonDangerSubtleStyle`  |                  |
| `SimpleMediumIconButtonPrimaryStyle`      |                  |
| `SimpleMediumIconButtonNeutralStyle`      |                  |
| `SimpleMediumIconButtonSubtleStyle`       |                  |
| `SimpleMediumIconButtonDangerPrimaryStyle`|                  |
| `SimpleMediumIconButtonDangerSubtleStyle` |                  |

IsDefaultStyle\*: Styles in this column will be set as the default implicit style for the matching control

## Lightweight Styling

### Button

| Key                                  | Type              | Value                            |
|--------------------------------------|-------------------|----------------------------------|
| `SimpleButtonCornerRadius`           | `CornerRadius`    | `SimpleRadius200CornerRadius` (8)    |
| `SimpleButtonBorderThickness`        | `Thickness`       | `SimpleStrokeBorderThickness` (1)    |
| `SimpleButtonMediumPadding`          | `Thickness`       | `SimpleSpace300Thickness` (12)        |
| `SimpleButtonSmallPadding`           | `Thickness`       | `SimpleSpace200Thickness` (8)        |
| `SimpleButtonMediumMinHeight`        | `Double`          | `SimpleIconLarge` (40)                |
| `SimpleButtonSmallMinHeight`         | `Double`          | `SimpleIconMedium` (32)               |
| `SimpleButtonMediumFontSize`         | `Double`          | `SimpleTypographyScale03` (16)        |
| `SimpleButtonSmallFontSize`          | `Double`          | `SimpleTypographyScale02` (14)        |
| `SimpleButtonIconSpacing`            | `Double`          | `SimpleSpace200` (8)                 |
| `SimpleButtonFontFamily`             | `FontFamily`      | Inter                            |
| `SimpleButtonFontWeight`             | `String`          | Normal                           |

### IconButton

| Key                                  | Type              | Value                            |
|--------------------------------------|-------------------|----------------------------------|
| `SimpleIconButtonCornerRadius`       | `CornerRadius`    | `SimpleRadiusFullCornerRadius` (9999)   |
| `SimpleIconButtonBorderThickness`    | `Thickness`       | `SimpleStrokeBorderThickness` (1)    |
| `SimpleIconButtonMediumPadding`      | `Thickness`       | `SimpleSpace200Thickness` (8)        |
| `SimpleIconButtonSmallPadding`       | `Thickness`       | `SimpleSpace100Thickness` (4)        |
| `SimpleIconButtonMediumMinSize`      | `Double`          | `SimpleIconLarge` (40)                |
| `SimpleIconButtonSmallMinSize`       | `Double`          | `SimpleIconMedium` (32)               |

### Primary Variant

| Key                                                  | Type              | Value                                       |
|------------------------------------------------------|-------------------|---------------------------------------------|
| `SimplePrimaryButtonForeground`                      | `SolidColorBrush` | `SimpleTextBrandOnBrandBrush`               |
| `SimplePrimaryButtonForegroundPointerOver`           | `SolidColorBrush` | `SimpleTextBrandOnBrandBrush`               |
| `SimplePrimaryButtonForegroundPressed`               | `SolidColorBrush` | `SimpleTextBrandOnBrandBrush`               |
| `SimplePrimaryButtonForegroundDisabled`              | `SolidColorBrush` | `SimpleTextDisabledDefaultBrush`            |
| `SimplePrimaryButtonBackground`                      | `SolidColorBrush` | `SimpleBackgroundBrandDefaultBrush`         |
| `SimplePrimaryButtonBackgroundPointerOver`           | `SolidColorBrush` | `SimpleBackgroundBrandHoverBrush`           |
| `SimplePrimaryButtonBackgroundPressed`               | `SolidColorBrush` | `SimpleBackgroundBrandPressedBrush`         |
| `SimplePrimaryButtonBackgroundDisabled`              | `SolidColorBrush` | `SimpleBackgroundDisabledDefaultBrush`      |
| `SimplePrimaryButtonBorderBrush`                     | `SolidColorBrush` | `SimpleBorderBrandDefaultBrush`             |
| `SimplePrimaryButtonBorderBrushPointerOver`          | `SolidColorBrush` | `SimpleBorderBrandDefaultBrush`             |
| `SimplePrimaryButtonBorderBrushPressed`              | `SolidColorBrush` | `SimpleBorderBrandDefaultBrush`             |
| `SimplePrimaryButtonBorderBrushDisabled`             | `SolidColorBrush` | `SimpleBorderDisabledDefaultBrush`          |

### Neutral Variant

| Key                                                  | Type              | Value                                              |
|------------------------------------------------------|-------------------|----------------------------------------------------|
| `SimpleNeutralButtonForeground`                      | `SolidColorBrush` | `SimpleTextDefaultDefaultBrush`                    |
| `SimpleNeutralButtonForegroundPointerOver`           | `SolidColorBrush` | `SimpleTextDefaultDefaultBrush`                    |
| `SimpleNeutralButtonForegroundPressed`               | `SolidColorBrush` | `SimpleTextDefaultDefaultBrush`                    |
| `SimpleNeutralButtonForegroundDisabled`              | `SolidColorBrush` | `SimpleTextDisabledDefaultBrush`                   |
| `SimpleNeutralButtonBackground`                      | `SolidColorBrush` | `SimpleBackgroundDefaultSecondaryBrush`            |
| `SimpleNeutralButtonBackgroundPointerOver`           | `SolidColorBrush` | `SimpleBackgroundDefaultSecondaryHoverBrush`       |
| `SimpleNeutralButtonBackgroundPressed`               | `SolidColorBrush` | `SimpleBackgroundDefaultSecondaryPressedBrush`     |
| `SimpleNeutralButtonBackgroundDisabled`              | `SolidColorBrush` | `SimpleBackgroundDisabledDefaultBrush`             |
| `SimpleNeutralButtonBorderBrush`                     | `SolidColorBrush` | `SimpleBorderDefaultDefaultBrush`                  |
| `SimpleNeutralButtonBorderBrushPointerOver`          | `SolidColorBrush` | `SimpleBorderDefaultDefaultBrush`                  |
| `SimpleNeutralButtonBorderBrushPressed`              | `SolidColorBrush` | `SimpleBorderDefaultDefaultBrush`                  |
| `SimpleNeutralButtonBorderBrushDisabled`             | `SolidColorBrush` | `SimpleBorderDisabledDefaultBrush`                 |

### Subtle Variant

| Key                                                  | Type              | Value                                              |
|------------------------------------------------------|-------------------|----------------------------------------------------|
| `SimpleSubtleButtonForeground`                       | `SolidColorBrush` | `SimpleTextBrandDefaultBrush`                      |
| `SimpleSubtleButtonForegroundPointerOver`            | `SolidColorBrush` | `SimpleTextBrandDefaultBrush`                      |
| `SimpleSubtleButtonForegroundPressed`                | `SolidColorBrush` | `SimpleTextBrandDefaultBrush`                      |
| `SimpleSubtleButtonForegroundDisabled`               | `SolidColorBrush` | `SimpleTextDisabledDefaultBrush`                   |
| `SimpleSubtleButtonBackground`                       | `SolidColorBrush` | `SystemControlTransparentBrush`                    |
| `SimpleSubtleButtonBackgroundPointerOver`            | `SolidColorBrush` | `SimpleBackgroundDefaultDefaultHoverBrush`         |
| `SimpleSubtleButtonBackgroundPressed`                | `SolidColorBrush` | `SimpleBackgroundDefaultDefaultPressedBrush`       |
| `SimpleSubtleButtonBackgroundDisabled`               | `SolidColorBrush` | `SimpleBackgroundDisabledDefaultBrush`             |
| `SimpleSubtleButtonBorderBrush`                      | `SolidColorBrush` | `SystemControlTransparentBrush`                    |
| `SimpleSubtleButtonBorderBrushPointerOver`           | `SolidColorBrush` | `SystemControlTransparentBrush`                    |
| `SimpleSubtleButtonBorderBrushPressed`               | `SolidColorBrush` | `SystemControlTransparentBrush`                    |
| `SimpleSubtleButtonBorderBrushDisabled`              | `SolidColorBrush` | `SimpleBorderDisabledDefaultBrush`                 |

### Danger Primary Variant

| Key                                                       | Type              | Value                                         |
|-----------------------------------------------------------|-------------------|-----------------------------------------------|
| `SimpleDangerPrimaryButtonForeground`                     | `SolidColorBrush` | `SimpleTextDangerOnDangerBrush`               |
| `SimpleDangerPrimaryButtonForegroundPointerOver`          | `SolidColorBrush` | `SimpleTextDangerOnDangerBrush`               |
| `SimpleDangerPrimaryButtonForegroundPressed`              | `SolidColorBrush` | `SimpleTextDangerOnDangerBrush`               |
| `SimpleDangerPrimaryButtonForegroundDisabled`             | `SolidColorBrush` | `SimpleTextDisabledDefaultBrush`              |
| `SimpleDangerPrimaryButtonBackground`                     | `SolidColorBrush` | `SimpleBackgroundDangerDefaultBrush`          |
| `SimpleDangerPrimaryButtonBackgroundPointerOver`          | `SolidColorBrush` | `SimpleBackgroundDangerHoverBrush`            |
| `SimpleDangerPrimaryButtonBackgroundPressed`              | `SolidColorBrush` | `SimpleBackgroundDangerPressedBrush`          |
| `SimpleDangerPrimaryButtonBackgroundDisabled`             | `SolidColorBrush` | `SimpleBackgroundDisabledDefaultBrush`        |
| `SimpleDangerPrimaryButtonBorderBrush`                    | `SolidColorBrush` | `SimpleBorderDangerSecondaryBrush`            |
| `SimpleDangerPrimaryButtonBorderBrushPointerOver`         | `SolidColorBrush` | `SimpleBorderDangerDefaultBrush`              |
| `SimpleDangerPrimaryButtonBorderBrushPressed`             | `SolidColorBrush` | `SimpleBorderDangerDefaultBrush`              |
| `SimpleDangerPrimaryButtonBorderBrushDisabled`            | `SolidColorBrush` | `SimpleBorderDisabledDefaultBrush`            |

### Danger Subtle Variant

| Key                                                       | Type              | Value                                              |
|-----------------------------------------------------------|-------------------|----------------------------------------------------|
| `SimpleDangerSubtleButtonForeground`                      | `SolidColorBrush` | `SimpleTextDangerDefaultBrush`                     |
| `SimpleDangerSubtleButtonForegroundPointerOver`           | `SolidColorBrush` | `SimpleTextDangerDefaultBrush`                     |
| `SimpleDangerSubtleButtonForegroundPressed`               | `SolidColorBrush` | `SimpleTextDangerDefaultBrush`                     |
| `SimpleDangerSubtleButtonForegroundDisabled`              | `SolidColorBrush` | `SimpleTextDisabledDefaultBrush`                   |
| `SimpleDangerSubtleButtonBackground`                      | `SolidColorBrush` | `SystemControlTransparentBrush`                    |
| `SimpleDangerSubtleButtonBackgroundPointerOver`           | `SolidColorBrush` | `SimpleBackgroundDangerTertiaryHoverBrush`         |
| `SimpleDangerSubtleButtonBackgroundPressed`               | `SolidColorBrush` | `SimpleBackgroundDangerTertiaryPressedBrush`       |
| `SimpleDangerSubtleButtonBackgroundDisabled`              | `SolidColorBrush` | `SimpleBackgroundDisabledDefaultBrush`             |
| `SimpleDangerSubtleButtonBorderBrush`                     | `SolidColorBrush` | `SystemControlTransparentBrush`                    |
| `SimpleDangerSubtleButtonBorderBrushPointerOver`          | `SolidColorBrush` | `SystemControlTransparentBrush`                    |
| `SimpleDangerSubtleButtonBorderBrushPressed`              | `SolidColorBrush` | `SystemControlTransparentBrush`                    |
| `SimpleDangerSubtleButtonBorderBrushDisabled`             | `SolidColorBrush` | `SimpleBorderDisabledDefaultBrush`                 |
