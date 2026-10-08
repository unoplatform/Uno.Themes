---
uid: Uno.Themes.Simple.Styles.TextBox
---

# TextBox Control

<!-- BEGIN GENERATED -->
<!-- This region is generated from the Styles XAML by `dotnet run build/scripts/GenerateStyleDocs.cs`; do not edit it by hand. -->

## Styles

| Style Key                    | Semantic Alias         | IsDefaultStyle\* |
| ---------------------------- | ---------------------- | ---------------- |
| `SimpleFilledTextBoxStyle`   | `FilledTextBoxStyle`   |                  |
| `SimpleTextBoxErrorStyle`    |                        |                  |
| `SimpleTextBoxSmallStyle`    |                        |                  |
| `SimpleOutlinedTextBoxStyle` | `OutlinedTextBoxStyle` | True             |

IsDefaultStyle\*: Styles in this column will be set as the default implicit style for the matching control

## Lightweight Styling

### Theme-agnostic

| Key                         | Type        | Value                     |
| --------------------------- | ----------- | ------------------------- |
| `SimpleTextBoxHeaderMargin` | `Thickness` | `Space100BottomThickness` |

### Themed

| Key                                               | Type              | Light                            |
| ------------------------------------------------- | ----------------- | -------------------------------- |
| `FilledTextBoxBackground`                         | `SolidColorBrush` | `SurfaceBrush`                   |
| `FilledTextBoxBackgroundDisabled`                 | `SolidColorBrush` | `OnSurfaceDisabledBrush`         |
| `FilledTextBoxBackgroundFocused`                  | `SolidColorBrush` | `SurfaceBrush`                   |
| `FilledTextBoxBackgroundPointerOver`              | `SolidColorBrush` | `SurfaceVariantBrush`            |
| `FilledTextBoxBorderBrush`                        | `SolidColorBrush` | `SystemControlTransparentBrush`  |
| `FilledTextBoxBorderBrushDisabled`                | `SolidColorBrush` | `SystemControlTransparentBrush`  |
| `FilledTextBoxBorderBrushFocused`                 | `SolidColorBrush` | `SystemControlTransparentBrush`  |
| `FilledTextBoxBorderBrushPointerOver`             | `SolidColorBrush` | `SystemControlTransparentBrush`  |
| `FilledTextBoxBorderThicknessFocused`             | `Thickness`       | 0                                |
| `FilledTextBoxBorderThicknessNormal`              | `Thickness`       | 0                                |
| `FilledTextBoxCornerRadius`                       | `CornerRadius`    | `SimpleRadius200CornerRadius`    |
| `FilledTextBoxForeground`                         | `SolidColorBrush` | `OnSurfaceBrush`                 |
| `FilledTextBoxForegroundDisabled`                 | `SolidColorBrush` | `OnSurfaceDisabledBrush`         |
| `FilledTextBoxForegroundFocused`                  | `SolidColorBrush` | `OnSurfaceBrush`                 |
| `FilledTextBoxForegroundPointerOver`              | `SolidColorBrush` | `OnSurfaceBrush`                 |
| `FilledTextBoxHeaderForeground`                   | `SolidColorBrush` | `OnSurfaceBrush`                 |
| `FilledTextBoxHeaderForegroundDisabled`           | `SolidColorBrush` | `OnSurfaceDisabledBrush`         |
| `FilledTextBoxHeaderForegroundFocused`            | `SolidColorBrush` | `OnSurfaceBrush`                 |
| `FilledTextBoxHeaderForegroundPointerOver`        | `SolidColorBrush` | `OnSurfaceBrush`                 |
| `FilledTextBoxMinHeight`                          | `Double`          | `SimpleIconLarge`                |
| `FilledTextBoxPadding`                            | `Thickness`       | 16,10,16,10                      |
| `FilledTextBoxPlaceholderForeground`              | `SolidColorBrush` | `OnSurfaceLowBrush`              |
| `FilledTextBoxPlaceholderForegroundDisabled`      | `SolidColorBrush` | `OnSurfaceDisabledBrush`         |
| `FilledTextBoxPlaceholderForegroundFocused`       | `SolidColorBrush` | `OnSurfaceLowBrush`              |
| `FilledTextBoxPlaceholderForegroundPointerOver`   | `SolidColorBrush` | `OnSurfaceLowBrush`              |
| `OutlinedTextBoxBackground`                       | `SolidColorBrush` | `SurfaceBrush`                   |
| `OutlinedTextBoxBackgroundDisabled`               | `SolidColorBrush` | `OnSurfaceDisabledBrush`         |
| `OutlinedTextBoxBackgroundFocused`                | `SolidColorBrush` | `SurfaceBrush`                   |
| `OutlinedTextBoxBackgroundPointerOver`            | `SolidColorBrush` | `SurfaceBrush`                   |
| `OutlinedTextBoxBorderBrush`                      | `SolidColorBrush` | `OutlineBrush`                   |
| `OutlinedTextBoxBorderBrushDisabled`              | `SolidColorBrush` | `OutlineDisabledBrush`           |
| `OutlinedTextBoxBorderBrushFocused`               | `SolidColorBrush` | `PrimaryMediumBrush`             |
| `OutlinedTextBoxBorderBrushPointerOver`           | `SolidColorBrush` | `OutlineBrush`                   |
| `OutlinedTextBoxBorderThickness`                  | `Thickness`       | `SimpleStrokeBorderThickness`    |
| `OutlinedTextBoxBorderThicknessFocused`           | `Thickness`       | `SimpleStrokeFocusRingThickness` |
| `OutlinedTextBoxCornerRadius`                     | `CornerRadius`    | `SimpleRadius200CornerRadius`    |
| `OutlinedTextBoxForeground`                       | `SolidColorBrush` | `OnSurfaceBrush`                 |
| `OutlinedTextBoxForegroundDisabled`               | `SolidColorBrush` | `OnSurfaceDisabledBrush`         |
| `OutlinedTextBoxForegroundFocused`                | `SolidColorBrush` | `OnSurfaceBrush`                 |
| `OutlinedTextBoxForegroundPointerOver`            | `SolidColorBrush` | `OnSurfaceBrush`                 |
| `OutlinedTextBoxHeaderForeground`                 | `SolidColorBrush` | `OnSurfaceBrush`                 |
| `OutlinedTextBoxHeaderForegroundDisabled`         | `SolidColorBrush` | `OnSurfaceDisabledBrush`         |
| `OutlinedTextBoxHeaderForegroundFocused`          | `SolidColorBrush` | `OnSurfaceBrush`                 |
| `OutlinedTextBoxHeaderForegroundPointerOver`      | `SolidColorBrush` | `OnSurfaceBrush`                 |
| `OutlinedTextBoxMinHeight`                        | `Double`          | `SimpleIconLarge`                |
| `OutlinedTextBoxPadding`                          | `Thickness`       | 16,10,16,10                      |
| `OutlinedTextBoxPlaceholderForeground`            | `SolidColorBrush` | `OnSurfaceLowBrush`              |
| `OutlinedTextBoxPlaceholderForegroundDisabled`    | `SolidColorBrush` | `OnSurfaceDisabledBrush`         |
| `OutlinedTextBoxPlaceholderForegroundFocused`     | `SolidColorBrush` | `OnSurfaceLowBrush`              |
| `OutlinedTextBoxPlaceholderForegroundPointerOver` | `SolidColorBrush` | `OnSurfaceLowBrush`              |
| `SimpleTextBoxBorderThickness`                    | `Thickness`       | `SimpleStrokeBorderThickness`    |
| `SimpleTextBoxCornerRadius`                       | `CornerRadius`    | `SimpleRadius200CornerRadius`    |
| `SimpleTextBoxFocusedBorderThickness`             | `Thickness`       | `SimpleStrokeFocusRingThickness` |
| `SimpleTextBoxMinHeight`                          | `Double`          | `SimpleIconLarge`                |
| `SimpleTextBoxPadding`                            | `Thickness`       | 16,10,16,10                      |
| `SimpleTextBoxSmallMinHeight`                     | `Double`          | `SimpleIconMedium`               |

The Dark theme uses the same values as Light.

<!-- END GENERATED -->
