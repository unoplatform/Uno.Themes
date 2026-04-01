---
uid: Uno.Themes.Simple.Styles.Button
---

# Button Control

## Styles

| Style Key                                 | IsDefaultStyle\* |
|-------------------------------------------|------------------|
| `SimpleFilledButtonStyle`                | True             |
| `SimpleFilledTonalButtonStyle`                |                  |
| `SimpleTextButtonStyle`                 |                  |
| `SimpleDangerPrimaryButtonStyle`          |                  |
| `SimpleDangerSubtleButtonStyle`           |                  |
| `SimpleSmallFilledButtonStyle`           |                  |
| `SimpleSmallFilledTonalButtonStyle`           |                  |
| `SimpleSmallTextButtonStyle`            |                  |
| `SimpleSmallDangerPrimaryButtonStyle`     |                  |
| `SimpleSmallDangerSubtleButtonStyle`      |                  |
| `SimpleMediumFilledButtonStyle`          |                  |
| `SimpleMediumFilledTonalButtonStyle`          |                  |
| `SimpleMediumTextButtonStyle`           |                  |
| `SimpleMediumDangerPrimaryButtonStyle`    |                  |
| `SimpleMediumDangerSubtleButtonStyle`     |                  |
| `SimpleIconButtonStyle`            |                  |
| `SimpleIconButtonNeutralStyle`            |                  |
| `SimpleIconButtonSubtleStyle`             |                  |
| `SimpleIconButtonDangerPrimaryStyle`      |                  |
| `SimpleIconButtonDangerSubtleStyle`       |                  |
| `SimpleSmallIconButtonStyle`       |                  |
| `SimpleSmallIconButtonNeutralStyle`       |                  |
| `SimpleSmallIconButtonSubtleStyle`        |                  |
| `SimpleSmallIconButtonDangerPrimaryStyle` |                  |
| `SimpleSmallIconButtonDangerSubtleStyle`  |                  |
| `SimpleMediumIconButtonStyle`      |                  |
| `SimpleMediumIconButtonNeutralStyle`      |                  |
| `SimpleMediumIconButtonSubtleStyle`       |                  |
| `SimpleMediumIconButtonDangerPrimaryStyle`|                  |
| `SimpleMediumIconButtonDangerSubtleStyle` |                  |

IsDefaultStyle\*: Styles in this column will be set as the default implicit style for the matching control

## Lightweight Styling

### Button

| Key                                  | Type              | Value                            |
|--------------------------------------|-------------------|----------------------------------|
| `SimpleButtonCornerRadius`           | `CornerRadius`    | `SimpleRadius200CornerRadius`    |
| `SimpleButtonBorderThickness`        | `Thickness`       | `SimpleStrokeBorderThickness`    |
| `SimpleButtonMediumPadding`          | `Thickness`       | `SimpleSpace300Thickness`        |
| `SimpleButtonSmallPadding`           | `Thickness`       | `SimpleSpace200Thickness`        |
| `SimpleButtonMediumMinHeight`        | `Double`          | `SimpleIconLarge`                |
| `SimpleButtonSmallMinHeight`         | `Double`          | `SimpleIconMedium`               |
| `SimpleButtonMediumFontSize`         | `Double`          | `BodyMediumFontSize`             |
| `SimpleButtonSmallFontSize`          | `Double`          | `BodySmallFontSize`              |
| `SimpleButtonIconSpacing`            | `Double`          | `SimpleSpace200`                 |
| `SimpleButtonFontFamily`             | `FontFamily`      | Inter                            |
| `SimpleButtonFontWeight`             | `String`          | Normal                           |

### IconButton

| Key                                  | Type              | Value                            |
|--------------------------------------|-------------------|----------------------------------|
| `SimpleIconButtonCornerRadius`       | `CornerRadius`    | `SimpleRadiusFullCornerRadius`   |
| `SimpleIconButtonBorderThickness`    | `Thickness`       | `SimpleStrokeBorderThickness`    |
| `SimpleIconButtonMediumPadding`      | `Thickness`       | `SimpleSpace200Thickness`        |
| `SimpleIconButtonSmallPadding`       | `Thickness`       | `SimpleSpace100Thickness`        |
| `SimpleIconButtonMediumMinSize`      | `Double`          | `SimpleIconLarge`                |
| `SimpleIconButtonSmallMinSize`       | `Double`          | `SimpleIconMedium`               |

### Primary Variant

| Key                                                  | Type              | Value                          |
|------------------------------------------------------|-------------------|--------------------------------|
| `FilledButtonForeground`                             | `SolidColorBrush` | `OnPrimaryBrush`               |
| `FilledButtonForegroundPointerOver`                  | `SolidColorBrush` | `OnPrimaryBrush`               |
| `FilledButtonForegroundPressed`                      | `SolidColorBrush` | `OnPrimaryBrush`               |
| `FilledButtonForegroundDisabled`                     | `SolidColorBrush` | `OnSurfaceDisabledBrush`       |
| `FilledButtonBackground`                             | `SolidColorBrush` | `PrimaryBrush`                 |
| `FilledButtonBackgroundPointerOver`                  | `SolidColorBrush` | `PrimaryVariantDarkBrush`      |
| `FilledButtonBackgroundPressed`                      | `SolidColorBrush` | `PrimaryVariantDarkBrush`      |
| `FilledButtonBackgroundDisabled`                     | `SolidColorBrush` | `OnSurfaceDisabledBrush`       |
| `FilledButtonBorderBrush`                            | `SolidColorBrush` | `PrimaryBrush`                 |
| `FilledButtonBorderBrushPointerOver`                 | `SolidColorBrush` | `PrimaryBrush`                 |
| `FilledButtonBorderBrushPressed`                     | `SolidColorBrush` | `PrimaryBrush`                 |
| `FilledButtonBorderBrushDisabled`                    | `SolidColorBrush` | `OutlineDisabledBrush`         |

### Neutral Variant

| Key                                                  | Type              | Value                          |
|------------------------------------------------------|-------------------|--------------------------------|
| `FilledTonalButtonForeground`                        | `SolidColorBrush` | `OnSurfaceBrush`               |
| `FilledTonalButtonForegroundPointerOver`             | `SolidColorBrush` | `OnSurfaceBrush`               |
| `FilledTonalButtonForegroundPressed`                 | `SolidColorBrush` | `OnSurfaceBrush`               |
| `FilledTonalButtonForegroundDisabled`                | `SolidColorBrush` | `OnSurfaceDisabledBrush`       |
| `FilledTonalButtonBackground`                        | `SolidColorBrush` | `SurfaceVariantBrush`          |
| `FilledTonalButtonBackgroundPointerOver`             | `SolidColorBrush` | `PrimaryContainerBrush`        |
| `FilledTonalButtonBackgroundPressed`                 | `SolidColorBrush` | `TertiaryContainerBrush`       |
| `FilledTonalButtonBackgroundDisabled`                | `SolidColorBrush` | `OnSurfaceDisabledBrush`       |
| `FilledTonalButtonBorderBrush`                       | `SolidColorBrush` | `OutlineBrush`                 |
| `FilledTonalButtonBorderBrushPointerOver`            | `SolidColorBrush` | `OutlineBrush`                 |
| `FilledTonalButtonBorderBrushPressed`                | `SolidColorBrush` | `OutlineBrush`                 |
| `FilledTonalButtonBorderBrushDisabled`               | `SolidColorBrush` | `OutlineDisabledBrush`         |

### Subtle Variant

| Key                                                  | Type              | Value                              |
|------------------------------------------------------|-------------------|------------------------------------|
| `TextButtonForeground`                               | `SolidColorBrush` | `PrimaryBrush`                     |
| `TextButtonForegroundPointerOver`                    | `SolidColorBrush` | `PrimaryBrush`                     |
| `TextButtonForegroundPressed`                        | `SolidColorBrush` | `PrimaryBrush`                     |
| `TextButtonForegroundDisabled`                       | `SolidColorBrush` | `OnSurfaceDisabledBrush`           |
| `TextButtonBackground`                               | `SolidColorBrush` | `SystemControlTransparentBrush`    |
| `TextButtonBackgroundPointerOver`                    | `SolidColorBrush` | `SurfaceVariantBrush`              |
| `TextButtonBackgroundPressed`                        | `SolidColorBrush` | `PrimaryContainerBrush`            |
| `TextButtonBackgroundDisabled`                       | `SolidColorBrush` | `OnSurfaceDisabledBrush`           |
| `TextButtonBorderBrush`                              | `SolidColorBrush` | `SystemControlTransparentBrush`    |
| `TextButtonBorderBrushPointerOver`                   | `SolidColorBrush` | `SystemControlTransparentBrush`    |
| `TextButtonBorderBrushPressed`                       | `SolidColorBrush` | `SystemControlTransparentBrush`    |
| `TextButtonBorderBrushDisabled`                      | `SolidColorBrush` | `OutlineDisabledBrush`             |

### IconButton Variant

| Key                                                  | Type              | Value                              |
|------------------------------------------------------|-------------------|------------------------------------|
| `IconButtonForeground`                               | `SolidColorBrush` | `FilledButtonForeground`           |
| `IconButtonForegroundPointerOver`                    | `SolidColorBrush` | `FilledButtonForegroundPointerOver`|
| `IconButtonForegroundPressed`                        | `SolidColorBrush` | `FilledButtonForegroundPressed`    |
| `IconButtonForegroundDisabled`                       | `SolidColorBrush` | `FilledButtonForegroundDisabled`   |
| `IconButtonBackground`                               | `SolidColorBrush` | `FilledButtonBackground`           |
| `IconButtonBackgroundPointerOver`                    | `SolidColorBrush` | `FilledButtonBackgroundPointerOver`|
| `IconButtonBackgroundPressed`                        | `SolidColorBrush` | `FilledButtonBackgroundPressed`    |
| `IconButtonBackgroundDisabled`                       | `SolidColorBrush` | `FilledButtonBackgroundDisabled`   |
| `IconButtonBorderBrush`                              | `SolidColorBrush` | `FilledButtonBorderBrush`          |
| `IconButtonBorderBrushPointerOver`                   | `SolidColorBrush` | `FilledButtonBorderBrushPointerOver`|
| `IconButtonBorderBrushPressed`                       | `SolidColorBrush` | `FilledButtonBorderBrushPressed`   |
| `IconButtonBorderBrushDisabled`                      | `SolidColorBrush` | `FilledButtonBorderBrushDisabled`  |

### Outlined Variant (Compatibility Aliases)

These keys alias the Neutral (FilledTonal) variant for cross-design-system compatibility.

| Key                                                  | Type              | Value                                    |
|------------------------------------------------------|-------------------|------------------------------------------|
| `OutlinedButtonForeground`                           | `SolidColorBrush` | `FilledTonalButtonForeground`            |
| `OutlinedButtonForegroundPointerOver`                | `SolidColorBrush` | `FilledTonalButtonForegroundPointerOver` |
| `OutlinedButtonForegroundPressed`                    | `SolidColorBrush` | `FilledTonalButtonForegroundPressed`     |
| `OutlinedButtonForegroundDisabled`                   | `SolidColorBrush` | `FilledTonalButtonForegroundDisabled`    |
| `OutlinedButtonBackground`                           | `SolidColorBrush` | `FilledTonalButtonBackground`            |
| `OutlinedButtonBackgroundPointerOver`                | `SolidColorBrush` | `FilledTonalButtonBackgroundPointerOver` |
| `OutlinedButtonBackgroundPressed`                    | `SolidColorBrush` | `FilledTonalButtonBackgroundPressed`     |
| `OutlinedButtonBackgroundDisabled`                   | `SolidColorBrush` | `FilledTonalButtonBackgroundDisabled`    |
| `OutlinedButtonBorderBrush`                          | `SolidColorBrush` | `FilledTonalButtonBorderBrush`           |
| `OutlinedButtonBorderBrushPointerOver`               | `SolidColorBrush` | `FilledTonalButtonBorderBrushPointerOver`|
| `OutlinedButtonBorderBrushPressed`                   | `SolidColorBrush` | `FilledTonalButtonBorderBrushPressed`    |
| `OutlinedButtonBorderBrushDisabled`                  | `SolidColorBrush` | `FilledTonalButtonBorderBrushDisabled`   |
