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

| Key                               | Type           | Value                            |
| --------------------------------- | -------------- | -------------------------------- |
| `SimpleButtonBorderThickness`     | `Thickness`    | `SimpleStrokeBorderThickness`    |
| `SimpleButtonCornerRadius`        | `CornerRadius` | `SimpleShapeControlCornerRadius` |
| `SimpleButtonFontWeight`          | `String`       | Medium                           |
| `SimpleButtonIconSpacing`         | `Double`       | `SimpleSpace100`                 |
| `SimpleIconButtonBorderThickness` | `Thickness`    | `SimpleStrokeBorderThickness`    |
| `SimpleIconButtonCornerRadius`    | `CornerRadius` | 20                               |

### Themed

| Key                                                | Type              | Light                                              |
| -------------------------------------------------- | ----------------- | -------------------------------------------------- |
| `FilledButtonBackground`                           | `SolidColorBrush` | `PrimaryBrush`                                     |
| `FilledButtonBackgroundDisabled`                   | `SolidColorBrush` | `OnSurfaceDisabledBrush`                           |
| `FilledButtonBackgroundPointerOver`                | `SolidColorBrush` | `PrimaryBrush`                                     |
| `FilledButtonBackgroundPressed`                    | `SolidColorBrush` | `PrimaryBrush`                                     |
| `FilledButtonBorderBrush`                          | `SolidColorBrush` | `PrimaryBrush`                                     |
| `FilledButtonBorderBrushDisabled`                  | `SolidColorBrush` | `OutlineDisabledBrush`                             |
| `FilledButtonBorderBrushPointerOver`               | `SolidColorBrush` | `PrimaryBrush`                                     |
| `FilledButtonBorderBrushPressed`                   | `SolidColorBrush` | `PrimaryBrush`                                     |
| `FilledButtonForeground`                           | `SolidColorBrush` | `OnPrimaryBrush`                                   |
| `FilledButtonForegroundDisabled`                   | `SolidColorBrush` | `OnSurfaceDisabledBrush`                           |
| `FilledButtonForegroundPointerOver`                | `SolidColorBrush` | `OnPrimaryBrush`                                   |
| `FilledButtonForegroundPressed`                    | `SolidColorBrush` | `OnPrimaryBrush`                                   |
| `FilledButtonStateLayerBackgroundPointerOver`      | `SolidColorBrush` | `OnPrimaryHoverBrush`                              |
| `FilledButtonStateLayerBackgroundPressed`          | `SolidColorBrush` | `OnPrimaryPressedBrush`                            |
| `FilledTonalButtonBackground`                      | `SolidColorBrush` | `SurfaceVariantBrush`                              |
| `FilledTonalButtonBackgroundDisabled`              | `SolidColorBrush` | `OnSurfaceDisabledBrush`                           |
| `FilledTonalButtonBackgroundPointerOver`           | `SolidColorBrush` | `SurfaceVariantBrush`                              |
| `FilledTonalButtonBackgroundPressed`               | `SolidColorBrush` | `SurfaceVariantBrush`                              |
| `FilledTonalButtonBorderBrush`                     | `SolidColorBrush` | `OutlineBrush`                                     |
| `FilledTonalButtonBorderBrushDisabled`             | `SolidColorBrush` | `OutlineDisabledBrush`                             |
| `FilledTonalButtonBorderBrushPointerOver`          | `SolidColorBrush` | `OutlineBrush`                                     |
| `FilledTonalButtonBorderBrushPressed`              | `SolidColorBrush` | `OutlineBrush`                                     |
| `FilledTonalButtonForeground`                      | `SolidColorBrush` | `OnSurfaceBrush`                                   |
| `FilledTonalButtonForegroundDisabled`              | `SolidColorBrush` | `OnSurfaceDisabledBrush`                           |
| `FilledTonalButtonForegroundPointerOver`           | `SolidColorBrush` | `OnSurfaceBrush`                                   |
| `FilledTonalButtonForegroundPressed`               | `SolidColorBrush` | `OnSurfaceBrush`                                   |
| `FilledTonalButtonStateLayerBackgroundPointerOver` | `SolidColorBrush` | `OnSurfaceHoverBrush`                              |
| `FilledTonalButtonStateLayerBackgroundPressed`     | `SolidColorBrush` | `OnSurfacePressedBrush`                            |
| `IconButtonBackground`                             | `SolidColorBrush` | `FilledButtonBackground`                           |
| `IconButtonBackgroundDisabled`                     | `SolidColorBrush` | `FilledButtonBackgroundDisabled`                   |
| `IconButtonBackgroundPointerOver`                  | `SolidColorBrush` | `FilledButtonBackgroundPointerOver`                |
| `IconButtonBackgroundPressed`                      | `SolidColorBrush` | `FilledButtonBackgroundPressed`                    |
| `IconButtonBorderBrush`                            | `SolidColorBrush` | `FilledButtonBorderBrush`                          |
| `IconButtonBorderBrushDisabled`                    | `SolidColorBrush` | `FilledButtonBorderBrushDisabled`                  |
| `IconButtonBorderBrushPointerOver`                 | `SolidColorBrush` | `FilledButtonBorderBrushPointerOver`               |
| `IconButtonBorderBrushPressed`                     | `SolidColorBrush` | `FilledButtonBorderBrushPressed`                   |
| `IconButtonForeground`                             | `SolidColorBrush` | `FilledButtonForeground`                           |
| `IconButtonForegroundDisabled`                     | `SolidColorBrush` | `FilledButtonForegroundDisabled`                   |
| `IconButtonForegroundPointerOver`                  | `SolidColorBrush` | `FilledButtonForegroundPointerOver`                |
| `IconButtonForegroundPressed`                      | `SolidColorBrush` | `FilledButtonForegroundPressed`                    |
| `IconButtonStateLayerBackgroundPointerOver`        | `SolidColorBrush` | `FilledButtonStateLayerBackgroundPointerOver`      |
| `IconButtonStateLayerBackgroundPressed`            | `SolidColorBrush` | `FilledButtonStateLayerBackgroundPressed`          |
| `OutlinedButtonBackground`                         | `SolidColorBrush` | `FilledTonalButtonBackground`                      |
| `OutlinedButtonBackgroundDisabled`                 | `SolidColorBrush` | `FilledTonalButtonBackgroundDisabled`              |
| `OutlinedButtonBackgroundPointerOver`              | `SolidColorBrush` | `FilledTonalButtonBackgroundPointerOver`           |
| `OutlinedButtonBackgroundPressed`                  | `SolidColorBrush` | `FilledTonalButtonBackgroundPressed`               |
| `OutlinedButtonBorderBrush`                        | `SolidColorBrush` | `FilledTonalButtonBorderBrush`                     |
| `OutlinedButtonBorderBrushDisabled`                | `SolidColorBrush` | `FilledTonalButtonBorderBrushDisabled`             |
| `OutlinedButtonBorderBrushPointerOver`             | `SolidColorBrush` | `FilledTonalButtonBorderBrushPointerOver`          |
| `OutlinedButtonBorderBrushPressed`                 | `SolidColorBrush` | `FilledTonalButtonBorderBrushPressed`              |
| `OutlinedButtonForeground`                         | `SolidColorBrush` | `FilledTonalButtonForeground`                      |
| `OutlinedButtonForegroundDisabled`                 | `SolidColorBrush` | `FilledTonalButtonForegroundDisabled`              |
| `OutlinedButtonForegroundPointerOver`              | `SolidColorBrush` | `FilledTonalButtonForegroundPointerOver`           |
| `OutlinedButtonForegroundPressed`                  | `SolidColorBrush` | `FilledTonalButtonForegroundPressed`               |
| `OutlinedButtonStateLayerBackgroundPointerOver`    | `SolidColorBrush` | `FilledTonalButtonStateLayerBackgroundPointerOver` |
| `OutlinedButtonStateLayerBackgroundPressed`        | `SolidColorBrush` | `FilledTonalButtonStateLayerBackgroundPressed`     |
| `SimpleButtonFontFamily`                           | `FontFamily`      | `DefaultFontFamily`                                |
| `TextButtonBackground`                             | `SolidColorBrush` | `SystemControlTransparentBrush`                    |
| `TextButtonBackgroundDisabled`                     | `SolidColorBrush` | `OnSurfaceDisabledBrush`                           |
| `TextButtonBackgroundPointerOver`                  | `SolidColorBrush` | `SurfaceVariantBrush`                              |
| `TextButtonBackgroundPressed`                      | `SolidColorBrush` | `PrimaryContainerBrush`                            |
| `TextButtonBorderBrush`                            | `SolidColorBrush` | `SystemControlTransparentBrush`                    |
| `TextButtonBorderBrushDisabled`                    | `SolidColorBrush` | `OutlineDisabledBrush`                             |
| `TextButtonBorderBrushPointerOver`                 | `SolidColorBrush` | `SystemControlTransparentBrush`                    |
| `TextButtonBorderBrushPressed`                     | `SolidColorBrush` | `SystemControlTransparentBrush`                    |
| `TextButtonForeground`                             | `SolidColorBrush` | `PrimaryBrush`                                     |
| `TextButtonForegroundDisabled`                     | `SolidColorBrush` | `OnSurfaceDisabledBrush`                           |
| `TextButtonForegroundPointerOver`                  | `SolidColorBrush` | `PrimaryBrush`                                     |
| `TextButtonForegroundPressed`                      | `SolidColorBrush` | `PrimaryBrush`                                     |

The Dark theme uses the same values as Light.

<!-- END GENERATED -->

The `OutlinedButton*` keys alias the Neutral (`FilledTonalButton*`) variant for cross-design-system compatibility.

> [!NOTE]
> The filled, neutral (tonal) and icon button styles keep their resting fill in the **PointerOver** and **Pressed** states and tint it with a translucent state layer, so a hovered or pressed button stays a shade of its own color and its label keeps a readable foreground with any palette. To change that tint, override the `*StateLayerBackgroundPointerOver` / `*StateLayerBackgroundPressed` keys. A translucent brush is expected: an opaque one hides the fill.
