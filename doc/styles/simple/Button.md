---
uid: Uno.Themes.Simple.Styles.Button
---

# Button Control

<!-- BEGIN GENERATED -->
<!-- This region is generated from the Styles XAML by `dotnet run build/scripts/GenerateStyleDocs.cs`; do not edit it by hand. -->

## Styles

| Style Key                            | Semantic Alias                                                                                                                             | IsDefaultStyle\* |
| ------------------------------------ | ------------------------------------------------------------------------------------------------------------------------------------------ | ---------------- |
| `SimpleFilledButtonStyle`            | `FilledButtonStyle`                                                                                                                        | True             |
| `SimpleFilledTonalButtonStyle`       | `FilledTonalButtonStyle`, `OutlinedButtonStyle`                                                                                            |                  |
| `SimpleTextButtonStyle`              | `TextButtonStyle`                                                                                                                          |                  |
| `SimpleDangerPrimaryButtonStyle`     |                                                                                                                                            |                  |
| `SimpleDangerSubtleButtonStyle`      |                                                                                                                                            |                  |
| `SimpleIconButtonStyle`              | `FabStyle`, `IconButtonStyle`, `LargeFabStyle`, `SmallFabStyle`                                                                            |                  |
| `SimpleIconButtonNeutralStyle`       | `SecondaryFabStyle`, `SecondaryLargeFabStyle`, `SecondarySmallFabStyle`, `SurfaceFabStyle`, `SurfaceLargeFabStyle`, `SurfaceSmallFabStyle` |                  |
| `SimpleIconButtonSubtleStyle`        | `TertiaryFabStyle`, `TertiaryLargeFabStyle`, `TertiarySmallFabStyle`                                                                       |                  |
| `SimpleIconButtonDangerPrimaryStyle` |                                                                                                                                            |                  |
| `SimpleIconButtonDangerSubtleStyle`  |                                                                                                                                            |                  |

IsDefaultStyle\*: Styles in this column will be set as the default implicit style for the matching control

## Lightweight Styling

### Theme-agnostic

| Key                               | Type           | Value                          |
| --------------------------------- | -------------- | ------------------------------ |
| `SimpleButtonBorderThickness`     | `Thickness`    | `SimpleStrokeBorderThickness`  |
| `SimpleButtonCornerRadius`        | `CornerRadius` | `SimpleRadius200CornerRadius`  |
| `SimpleButtonFontWeight`          | `String`       | Medium                         |
| `SimpleButtonIconSpacing`         | `Double`       | `SimpleSpace200`               |
| `SimpleIconButtonBorderThickness` | `Thickness`    | `SimpleStrokeBorderThickness`  |
| `SimpleIconButtonCornerRadius`    | `CornerRadius` | `SimpleRadiusFullCornerRadius` |

### Themed

| Key                                       | Type              | Light                                     |
| ----------------------------------------- | ----------------- | ----------------------------------------- |
| `FilledButtonBackground`                  | `SolidColorBrush` | `PrimaryBrush`                            |
| `FilledButtonBackgroundDisabled`          | `SolidColorBrush` | `OnSurfaceDisabledBrush`                  |
| `FilledButtonBackgroundPointerOver`       | `SolidColorBrush` | `PrimaryVariantDarkBrush`                 |
| `FilledButtonBackgroundPressed`           | `SolidColorBrush` | `PrimaryVariantDarkBrush`                 |
| `FilledButtonBorderBrush`                 | `SolidColorBrush` | `PrimaryBrush`                            |
| `FilledButtonBorderBrushDisabled`         | `SolidColorBrush` | `OutlineDisabledBrush`                    |
| `FilledButtonBorderBrushPointerOver`      | `SolidColorBrush` | `PrimaryBrush`                            |
| `FilledButtonBorderBrushPressed`          | `SolidColorBrush` | `PrimaryBrush`                            |
| `FilledButtonForeground`                  | `SolidColorBrush` | `OnPrimaryBrush`                          |
| `FilledButtonForegroundDisabled`          | `SolidColorBrush` | `OnSurfaceDisabledBrush`                  |
| `FilledButtonForegroundPointerOver`       | `SolidColorBrush` | `OnPrimaryBrush`                          |
| `FilledButtonForegroundPressed`           | `SolidColorBrush` | `OnPrimaryBrush`                          |
| `FilledTonalButtonBackground`             | `SolidColorBrush` | `SurfaceVariantBrush`                     |
| `FilledTonalButtonBackgroundDisabled`     | `SolidColorBrush` | `OnSurfaceDisabledBrush`                  |
| `FilledTonalButtonBackgroundPointerOver`  | `SolidColorBrush` | `PrimaryContainerBrush`                   |
| `FilledTonalButtonBackgroundPressed`      | `SolidColorBrush` | `TertiaryContainerBrush`                  |
| `FilledTonalButtonBorderBrush`            | `SolidColorBrush` | `OutlineBrush`                            |
| `FilledTonalButtonBorderBrushDisabled`    | `SolidColorBrush` | `OutlineDisabledBrush`                    |
| `FilledTonalButtonBorderBrushPointerOver` | `SolidColorBrush` | `OutlineBrush`                            |
| `FilledTonalButtonBorderBrushPressed`     | `SolidColorBrush` | `OutlineBrush`                            |
| `FilledTonalButtonForeground`             | `SolidColorBrush` | `OnSurfaceBrush`                          |
| `FilledTonalButtonForegroundDisabled`     | `SolidColorBrush` | `OnSurfaceDisabledBrush`                  |
| `FilledTonalButtonForegroundPointerOver`  | `SolidColorBrush` | `OnSurfaceBrush`                          |
| `FilledTonalButtonForegroundPressed`      | `SolidColorBrush` | `OnSurfaceBrush`                          |
| `IconButtonBackground`                    | `SolidColorBrush` | `FilledButtonBackground`                  |
| `IconButtonBackgroundDisabled`            | `SolidColorBrush` | `FilledButtonBackgroundDisabled`          |
| `IconButtonBackgroundPointerOver`         | `SolidColorBrush` | `FilledButtonBackgroundPointerOver`       |
| `IconButtonBackgroundPressed`             | `SolidColorBrush` | `FilledButtonBackgroundPressed`           |
| `IconButtonBorderBrush`                   | `SolidColorBrush` | `FilledButtonBorderBrush`                 |
| `IconButtonBorderBrushDisabled`           | `SolidColorBrush` | `FilledButtonBorderBrushDisabled`         |
| `IconButtonBorderBrushPointerOver`        | `SolidColorBrush` | `FilledButtonBorderBrushPointerOver`      |
| `IconButtonBorderBrushPressed`            | `SolidColorBrush` | `FilledButtonBorderBrushPressed`          |
| `IconButtonForeground`                    | `SolidColorBrush` | `FilledButtonForeground`                  |
| `IconButtonForegroundDisabled`            | `SolidColorBrush` | `FilledButtonForegroundDisabled`          |
| `IconButtonForegroundPointerOver`         | `SolidColorBrush` | `FilledButtonForegroundPointerOver`       |
| `IconButtonForegroundPressed`             | `SolidColorBrush` | `FilledButtonForegroundPressed`           |
| `OutlinedButtonBackground`                | `SolidColorBrush` | `FilledTonalButtonBackground`             |
| `OutlinedButtonBackgroundDisabled`        | `SolidColorBrush` | `FilledTonalButtonBackgroundDisabled`     |
| `OutlinedButtonBackgroundPointerOver`     | `SolidColorBrush` | `FilledTonalButtonBackgroundPointerOver`  |
| `OutlinedButtonBackgroundPressed`         | `SolidColorBrush` | `FilledTonalButtonBackgroundPressed`      |
| `OutlinedButtonBorderBrush`               | `SolidColorBrush` | `FilledTonalButtonBorderBrush`            |
| `OutlinedButtonBorderBrushDisabled`       | `SolidColorBrush` | `FilledTonalButtonBorderBrushDisabled`    |
| `OutlinedButtonBorderBrushPointerOver`    | `SolidColorBrush` | `FilledTonalButtonBorderBrushPointerOver` |
| `OutlinedButtonBorderBrushPressed`        | `SolidColorBrush` | `FilledTonalButtonBorderBrushPressed`     |
| `OutlinedButtonForeground`                | `SolidColorBrush` | `FilledTonalButtonForeground`             |
| `OutlinedButtonForegroundDisabled`        | `SolidColorBrush` | `FilledTonalButtonForegroundDisabled`     |
| `OutlinedButtonForegroundPointerOver`     | `SolidColorBrush` | `FilledTonalButtonForegroundPointerOver`  |
| `OutlinedButtonForegroundPressed`         | `SolidColorBrush` | `FilledTonalButtonForegroundPressed`      |
| `SimpleButtonFontFamily`                  | `FontFamily`      | `DefaultFontFamily`                       |
| `TextButtonBackground`                    | `SolidColorBrush` | `SystemControlTransparentBrush`           |
| `TextButtonBackgroundDisabled`            | `SolidColorBrush` | `OnSurfaceDisabledBrush`                  |
| `TextButtonBackgroundPointerOver`         | `SolidColorBrush` | `SurfaceVariantBrush`                     |
| `TextButtonBackgroundPressed`             | `SolidColorBrush` | `PrimaryContainerBrush`                   |
| `TextButtonBorderBrush`                   | `SolidColorBrush` | `SystemControlTransparentBrush`           |
| `TextButtonBorderBrushDisabled`           | `SolidColorBrush` | `OutlineDisabledBrush`                    |
| `TextButtonBorderBrushPointerOver`        | `SolidColorBrush` | `SystemControlTransparentBrush`           |
| `TextButtonBorderBrushPressed`            | `SolidColorBrush` | `SystemControlTransparentBrush`           |
| `TextButtonForeground`                    | `SolidColorBrush` | `PrimaryBrush`                            |
| `TextButtonForegroundDisabled`            | `SolidColorBrush` | `OnSurfaceDisabledBrush`                  |
| `TextButtonForegroundPointerOver`         | `SolidColorBrush` | `PrimaryBrush`                            |
| `TextButtonForegroundPressed`             | `SolidColorBrush` | `PrimaryBrush`                            |

The Dark theme uses the same values as Light.

<!-- END GENERATED -->

The `OutlinedButton*` keys alias the Neutral (`FilledTonalButton*`) variant for cross-design-system compatibility.
