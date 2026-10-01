---
uid: Uno.Themes.Styles.Button
---

# Button Control

<!-- BEGIN GENERATED -->
<!-- This region is generated from the Styles XAML by `dotnet run build/scripts/GenerateStyleDocs.cs`; do not edit it by hand. -->

## Styles

| Style Key                        | Semantic Alias           | IsDefaultStyle\* |
| -------------------------------- | ------------------------ | ---------------- |
| `MaterialElevatedButtonStyle`    | `ElevatedButtonStyle`    |                  |
| `MaterialFilledButtonStyle`      | `FilledButtonStyle`      | True             |
| `MaterialFilledTonalButtonStyle` | `FilledTonalButtonStyle` |                  |
| `MaterialOutlinedButtonStyle`    | `OutlinedButtonStyle`    |                  |
| `MaterialTextButtonStyle`        | `TextButtonStyle`        |                  |
| `MaterialIconButtonStyle`        | `IconButtonStyle`        |                  |
| `MaterialDefaultButtonStyle`     |                          | True             |

IsDefaultStyle\*: Styles in this column will be set as the default implicit style for the matching control

## Lightweight Styling

### Theme-agnostic

| Key                                    | Type     | Value   |
| -------------------------------------- | -------- | ------- |
| `ButtonHorizontalContentAlignment`     | `String` | Center  |
| `ButtonIconHorizontalAlignment`        | `String` | Stretch |
| `ButtonIconVerticalAlignment`          | `String` | Stretch |
| `ButtonVerticalContentAlignment`       | `String` | Center  |
| `IconButtonEllipseHorizontalAlignment` | `String` | Stretch |
| `IconButtonEllipseVerticalAlignment`   | `String` | Stretch |

### Themed

| Key                                                | Type              | Light                              |
| -------------------------------------------------- | ----------------- | ---------------------------------- |
| `ButtonBorderThickness`                            | `Thickness`       | 0                                  |
| `ButtonContentMargin`                              | `Thickness`       | `Space200HorizontalThickness`      |
| `ButtonCornerRadius`                               | `CornerRadius`    | `Radius500CornerRadius`            |
| `ButtonElevation`                                  | `Double`          | 0                                  |
| `ButtonIconMinWidth`                               | `Double`          | 18                                 |
| `ButtonMargin`                                     | `Thickness`       | 0                                  |
| `ButtonMinHeight`                                  | `Double`          | `ControlHeightMedium`              |
| `ButtonMinWidth`                                   | `Double`          | `ControlHeightMedium`              |
| `ButtonPadding`                                    | `Thickness`       | `Space400HorizontalThickness`      |
| `ElevatedButtonBackground`                         | `SolidColorBrush` | `SurfaceBrush`                     |
| `ElevatedButtonBackgroundDisabled`                 | `SolidColorBrush` | `OnSurfaceDisabledBrush`           |
| `ElevatedButtonBackgroundPointerOver`              | `SolidColorBrush` | `SurfaceBrush`                     |
| `ElevatedButtonBackgroundPressed`                  | `SolidColorBrush` | `SurfaceBrush`                     |
| `ElevatedButtonBorderBrush`                        | `SolidColorBrush` | `SystemControlTransparentBrush`    |
| `ElevatedButtonBorderBrushDisabled`                | `SolidColorBrush` | `SystemControlTransparentBrush`    |
| `ElevatedButtonBorderBrushPointerOver`             | `SolidColorBrush` | `SystemControlTransparentBrush`    |
| `ElevatedButtonBorderBrushPressed`                 | `SolidColorBrush` | `SystemControlTransparentBrush`    |
| `ElevatedButtonDisabledMargin`                     | `Thickness`       | 0                                  |
| `ElevatedButtonElevation`                          | `Double`          | 1                                  |
| `ElevatedButtonElevationDisabled`                  | `Double`          | 0                                  |
| `ElevatedButtonForeground`                         | `SolidColorBrush` | `PrimaryBrush`                     |
| `ElevatedButtonForegroundDisabled`                 | `SolidColorBrush` | `OnSurfaceDisabledBrush`           |
| `ElevatedButtonForegroundPointerOver`              | `SolidColorBrush` | `PrimaryBrush`                     |
| `ElevatedButtonForegroundPressed`                  | `SolidColorBrush` | `PrimaryBrush`                     |
| `ElevatedButtonIconForeground`                     | `SolidColorBrush` | `PrimaryBrush`                     |
| `ElevatedButtonIconForegroundDisabled`             | `SolidColorBrush` | `OnSurfaceDisabledBrush`           |
| `ElevatedButtonIconForegroundPointerOver`          | `SolidColorBrush` | `PrimaryBrush`                     |
| `ElevatedButtonIconForegroundPressed`              | `SolidColorBrush` | `PrimaryBrush`                     |
| `ElevatedButtonMargin`                             | `Thickness`       | 0,0,0,1                            |
| `ElevatedButtonStateLayerBackgroundPointerOver`    | `SolidColorBrush` | `PrimaryHoverBrush`                |
| `ElevatedButtonStateLayerBackgroundPressed`        | `SolidColorBrush` | `PrimaryPressedBrush`              |
| `FilledButtonBackground`                           | `SolidColorBrush` | `PrimaryBrush`                     |
| `FilledButtonBackgroundDisabled`                   | `SolidColorBrush` | `OnSurfaceDisabledBrush`           |
| `FilledButtonBackgroundPointerOver`                | `SolidColorBrush` | `PrimaryBrush`                     |
| `FilledButtonBackgroundPressed`                    | `SolidColorBrush` | `PrimaryBrush`                     |
| `FilledButtonBorderBrush`                          | `SolidColorBrush` | `SystemControlTransparentBrush`    |
| `FilledButtonBorderBrushDisabled`                  | `SolidColorBrush` | `SystemControlTransparentBrush`    |
| `FilledButtonBorderBrushPointerOver`               | `SolidColorBrush` | `SystemControlTransparentBrush`    |
| `FilledButtonBorderBrushPressed`                   | `SolidColorBrush` | `SystemControlTransparentBrush`    |
| `FilledButtonForeground`                           | `SolidColorBrush` | `OnPrimaryBrush`                   |
| `FilledButtonForegroundDisabled`                   | `SolidColorBrush` | `OnSurfaceDisabledBrush`           |
| `FilledButtonForegroundPointerOver`                | `SolidColorBrush` | `OnPrimaryBrush`                   |
| `FilledButtonForegroundPressed`                    | `SolidColorBrush` | `OnPrimaryBrush`                   |
| `FilledButtonIconForeground`                       | `SolidColorBrush` | `OnPrimaryBrush`                   |
| `FilledButtonIconForegroundDisabled`               | `SolidColorBrush` | `OnSurfaceDisabledBrush`           |
| `FilledButtonIconForegroundPointerOver`            | `SolidColorBrush` | `OnPrimaryBrush`                   |
| `FilledButtonIconForegroundPressed`                | `SolidColorBrush` | `OnPrimaryBrush`                   |
| `FilledButtonStateLayerBackgroundPointerOver`      | `SolidColorBrush` | `OnPrimaryHoverBrush`              |
| `FilledButtonStateLayerBackgroundPressed`          | `SolidColorBrush` | `OnPrimaryPressedBrush`            |
| `FilledTonalButtonBackground`                      | `SolidColorBrush` | `SecondaryContainerBrush`          |
| `FilledTonalButtonBackgroundDisabled`              | `SolidColorBrush` | `OnSurfaceDisabledBrush`           |
| `FilledTonalButtonBackgroundPointerOver`           | `SolidColorBrush` | `SecondaryContainerBrush`          |
| `FilledTonalButtonBackgroundPressed`               | `SolidColorBrush` | `SecondaryContainerBrush`          |
| `FilledTonalButtonBorderBrush`                     | `SolidColorBrush` | `SystemControlTransparentBrush`    |
| `FilledTonalButtonBorderBrushDisabled`             | `SolidColorBrush` | `SystemControlTransparentBrush`    |
| `FilledTonalButtonBorderBrushPointerOver`          | `SolidColorBrush` | `SystemControlTransparentBrush`    |
| `FilledTonalButtonBorderBrushPressed`              | `SolidColorBrush` | `SystemControlTransparentBrush`    |
| `FilledTonalButtonForeground`                      | `SolidColorBrush` | `OnSecondaryContainerBrush`        |
| `FilledTonalButtonForegroundDisabled`              | `SolidColorBrush` | `OnSurfaceDisabledBrush`           |
| `FilledTonalButtonForegroundPointerOver`           | `SolidColorBrush` | `OnSecondaryContainerBrush`        |
| `FilledTonalButtonForegroundPressed`               | `SolidColorBrush` | `OnSecondaryContainerBrush`        |
| `FilledTonalButtonIconForeground`                  | `SolidColorBrush` | `OnSecondaryContainerBrush`        |
| `FilledTonalButtonIconForegroundDisabled`          | `SolidColorBrush` | `OnSurfaceDisabledBrush`           |
| `FilledTonalButtonIconForegroundPointerOver`       | `SolidColorBrush` | `OnSecondaryContainerBrush`        |
| `FilledTonalButtonIconForegroundPressed`           | `SolidColorBrush` | `OnSecondaryContainerBrush`        |
| `FilledTonalButtonStateLayerBackgroundPointerOver` | `SolidColorBrush` | `OnSecondaryContainerHoverBrush`   |
| `FilledTonalButtonStateLayerBackgroundPressed`     | `SolidColorBrush` | `OnSecondaryContainerPressedBrush` |
| `IconButtonEllipseFillFocused`                     | `SolidColorBrush` | `PrimaryFocusedBrush`              |
| `IconButtonEllipseFillPointerOver`                 | `SolidColorBrush` | `PrimaryHoverBrush`                |
| `IconButtonEllipseFillPressed`                     | `SolidColorBrush` | `PrimaryPressedBrush`              |
| `IconButtonForeground`                             | `SolidColorBrush` | `OnSurfaceVariantBrush`            |
| `IconButtonForegroundDisabled`                     | `SolidColorBrush` | `OnSurfaceLowBrush`                |
| `IconButtonOpacityHiddenState`                     | `Double`          | 0                                  |
| `IconButtonOpacityVisibleState`                    | `Double`          | 1                                  |
| `OutlinedButtonBackground`                         | `SolidColorBrush` | `SystemControlTransparentBrush`    |
| `OutlinedButtonBackgroundDisabled`                 | `SolidColorBrush` | `OnSurfaceDisabledBrush`           |
| `OutlinedButtonBackgroundPointerOver`              | `SolidColorBrush` | `SystemControlTransparentBrush`    |
| `OutlinedButtonBackgroundPressed`                  | `SolidColorBrush` | `SystemControlTransparentBrush`    |
| `OutlinedButtonBorderBrush`                        | `SolidColorBrush` | `OutlineBrush`                     |
| `OutlinedButtonBorderBrushDisabled`                | `SolidColorBrush` | `OutlineDisabledBrush`             |
| `OutlinedButtonBorderBrushPointerOver`             | `SolidColorBrush` | `OutlineBrush`                     |
| `OutlinedButtonBorderBrushPressed`                 | `SolidColorBrush` | `OutlineBrush`                     |
| `OutlinedButtonBorderThickness`                    | `Thickness`       | 1                                  |
| `OutlinedButtonForeground`                         | `SolidColorBrush` | `PrimaryBrush`                     |
| `OutlinedButtonForegroundDisabled`                 | `SolidColorBrush` | `OnSurfaceDisabledBrush`           |
| `OutlinedButtonForegroundPointerOver`              | `SolidColorBrush` | `PrimaryBrush`                     |
| `OutlinedButtonForegroundPressed`                  | `SolidColorBrush` | `PrimaryBrush`                     |
| `OutlinedButtonIconForeground`                     | `SolidColorBrush` | `PrimaryBrush`                     |
| `OutlinedButtonIconForegroundDisabled`             | `SolidColorBrush` | `OnSurfaceDisabledBrush`           |
| `OutlinedButtonIconForegroundPointerOver`          | `SolidColorBrush` | `PrimaryBrush`                     |
| `OutlinedButtonIconForegroundPressed`              | `SolidColorBrush` | `PrimaryBrush`                     |
| `OutlinedButtonStateLayerBackgroundPointerOver`    | `SolidColorBrush` | `PrimaryHoverBrush`                |
| `OutlinedButtonStateLayerBackgroundPressed`        | `SolidColorBrush` | `PrimaryPressedBrush`              |
| `TextButtonBackground`                             | `SolidColorBrush` | `SystemControlTransparentBrush`    |
| `TextButtonBackgroundPointerOver`                  | `SolidColorBrush` | `SystemControlTransparentBrush`    |
| `TextButtonBackgroundPressed`                      | `SolidColorBrush` | `SystemControlTransparentBrush`    |
| `TextButtonBorderBrush`                            | `SolidColorBrush` | `SystemControlTransparentBrush`    |
| `TextButtonBorderBrushDisabled`                    | `SolidColorBrush` | `SystemControlTransparentBrush`    |
| `TextButtonBorderBrushPointerOver`                 | `SolidColorBrush` | `SystemControlTransparentBrush`    |
| `TextButtonBorderBrushPressed`                     | `SolidColorBrush` | `SystemControlTransparentBrush`    |
| `TextButtonForeground`                             | `SolidColorBrush` | `PrimaryBrush`                     |
| `TextButtonForegroundDisabled`                     | `SolidColorBrush` | `OnSurfaceDisabledBrush`           |
| `TextButtonForegroundPointerOver`                  | `SolidColorBrush` | `PrimaryBrush`                     |
| `TextButtonForegroundPressed`                      | `SolidColorBrush` | `PrimaryBrush`                     |
| `TextButtonIconForeground`                         | `SolidColorBrush` | `PrimaryBrush`                     |
| `TextButtonIconForegroundDisabled`                 | `SolidColorBrush` | `OnSurfaceDisabledBrush`           |
| `TextButtonIconForegroundPointerOver`              | `SolidColorBrush` | `PrimaryBrush`                     |
| `TextButtonIconForegroundPressed`                  | `SolidColorBrush` | `PrimaryBrush`                     |
| `TextButtonIconMargin`                             | `Thickness`       | `Space200RightThickness`           |
| `TextButtonPadding`                                | `Thickness`       | `Space300HorizontalThickness`      |
| `TextButtonStateLayerBackgroundPointerOver`        | `SolidColorBrush` | `PrimaryHoverBrush`                |
| `TextButtonStateLayerBackgroundPressed`            | `SolidColorBrush` | `PrimaryPressedBrush`              |

The Dark theme uses the same values as Light.

<!-- END GENERATED -->
