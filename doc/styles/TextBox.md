---
uid: Uno.Themes.Styles.TextBox
---

# TextBox Control

## Styles

| Style Key              | IsDefaultStyle\* |
| ---------------------- | ---------------- |
| `FilledTextBoxStyle`   |                  |
| `OutlinedTextBoxStyle` | True             |

IsDefaultStyle\*: Styles in this column will be set as the default implicit style for the matching control

## Lightweight Styling

| Key                                               | Type              | Value                         |
| ------------------------------------------------- | ----------------- | ----------------------------- |
| `TextBoxDeleteButtonForeground`                   | `SolidColorBrush` | `OnSurfaceVariantBrush`       |
| `TextBoxDeleteButtonForegroundPointerOver`        | `SolidColorBrush` | `OnSurfaceVariantBrush`       |
| `TextBoxDeleteButtonForegroundPressed`            | `SolidColorBrush` | `OnSurfaceVariantBrush`       |
| `TextBoxDeleteButtonForegroundDisabled`           | `SolidColorBrush` | `OnSurfaceLowBrush`           |
| `TextBoxLeadingIconForeground`                    | `SolidColorBrush` | `OnSurfaceVariantBrush`       |
| `TextBoxLeadingIconForegroundDisabled`            | `SolidColorBrush` | `OnSurfaceLowBrush`           |
| `FilledTextBoxBackground`                         | `SolidColorBrush` | `SurfaceVariantBrush`         |
| `FilledTextBoxBackgroundPointerOver`              | `SolidColorBrush` | `OnSurfaceVariantHoverBrush`  |
| `FilledTextBoxBackgroundFocused`                  | `SolidColorBrush` | `SurfaceVariantBrush`         |
| `FilledTextBoxBackgroundDisabled`                 | `SolidColorBrush` | `OnSurfaceDisabledBrush`      |
| `FilledTextBoxBorderBrush`                        | `SolidColorBrush` | `OnSurfaceVariantBrush`       |
| `FilledTextBoxBorderBrushPointerOver`             | `SolidColorBrush` | `OnSurfaceBrush`              |
| `FilledTextBoxBorderBrushFocused`                 | `SolidColorBrush` | `PrimaryBrush`                |
| `FilledTextBoxBorderBrushDisabled`                | `SolidColorBrush` | `OnSurfaceDisabledBrush`      |
| `FilledTextBoxDeleteButtonForeground`             | `SolidColorBrush` | `OnSurfaceVariantBrush`       |
| `FilledTextBoxDeleteButtonForegroundPointerOver`  | `SolidColorBrush` | `OnSurfaceVariantBrush`       |
| `FilledTextBoxDeleteButtonForegroundFocused`      | `SolidColorBrush` | `OnSurfaceVariantBrush`       |
| `FilledTextBoxDeleteButtonForegroundDisabled`     | `SolidColorBrush` | `OnSurfaceLowBrush`           |
| `FilledTextBoxForeground`                         | `SolidColorBrush` | `OnSurfaceBrush`              |
| `FilledTextBoxForegroundPointerOver`              | `SolidColorBrush` | `OnSurfaceBrush`              |
| `FilledTextBoxForegroundFocused`                  | `SolidColorBrush` | `OnSurfaceBrush`              |
| `FilledTextBoxForegroundDisabled`                 | `SolidColorBrush` | `OnSurfaceBrush`              |
| `FilledTextBoxForegroundOpacityDisabled`          | `Double`          | `LowOpacity`                  |
| `FilledTextBoxPlaceholderForeground`              | `SolidColorBrush` | `OnSurfaceVariantBrush`       |
| `FilledTextBoxPlaceholderForegroundPointerOver`   | `SolidColorBrush` | `OnSurfaceVariantBrush`       |
| `FilledTextBoxPlaceholderForegroundFocused`       | `SolidColorBrush` | `OnSurfaceBrush`              |
| `FilledTextBoxPlaceholderForegroundDisabled`      | `SolidColorBrush` | `OnSurfaceLowBrush`           |
| `FilledTextBoxHeaderForeground`                   | `SolidColorBrush` | `OnSurfaceVariantBrush`       |
| `FilledTextBoxHeaderForegroundPointerOver`        | `SolidColorBrush` | `OnSurfaceVariantBrush`       |
| `FilledTextBoxHeaderForegroundFocused`            | `SolidColorBrush` | `PrimaryBrush`                |
| `FilledTextBoxHeaderForegroundDisabled`           | `SolidColorBrush` | `OnSurfaceLowBrush`           |
| `OutlinedTextBoxBorderBrush`                      | `SolidColorBrush` | `OutlineBrush`                |
| `OutlinedTextBoxBorderBrushPointerOver`           | `SolidColorBrush` | `OnSurfaceBrush`              |
| `OutlinedTextBoxBorderBrushFocused`               | `SolidColorBrush` | `PrimaryBrush`                |
| `OutlinedTextBoxBorderBrushDisabled`              | `SolidColorBrush` | `OnSurfaceDisabledBrush`      |
| `OutlinedTextBoxForeground`                       | `SolidColorBrush` | `OnSurfaceBrush`              |
| `OutlinedTextBoxForegroundPointerOver`            | `SolidColorBrush` | `OnSurfaceBrush`              |
| `OutlinedTextBoxForegroundFocused`                | `SolidColorBrush` | `OnSurfaceBrush`              |
| `OutlinedTextBoxForegroundDisabled`               | `SolidColorBrush` | `OnSurfaceBrush`              |
| `OutlinedTextBoxForegroundOpacityDisabled`        | `Double`          | `LowOpacity`                  |
| `OutlinedTextBoxPlaceholderForeground`            | `SolidColorBrush` | `OnSurfaceVariantBrush`       |
| `OutlinedTextBoxPlaceholderForegroundPointerOver` | `SolidColorBrush` | `OnSurfaceVariantBrush`       |
| `OutlinedTextBoxPlaceholderForegroundFocused`     | `SolidColorBrush` | `OnSurfaceBrush`              |
| `OutlinedTextBoxPlaceholderForegroundDisabled`    | `SolidColorBrush` | `OnSurfaceLowBrush`           |
| `OutlinedTextBoxHeaderForeground`                 | `SolidColorBrush` | `OnSurfaceVariantBrush`       |
| `OutlinedTextBoxHeaderForegroundPointerOver`      | `SolidColorBrush` | `OnSurfaceVariantBrush`       |
| `OutlinedTextBoxHeaderForegroundFocused`          | `SolidColorBrush` | `PrimaryBrush`                |
| `OutlinedTextBoxHeaderForegroundDisabled`         | `SolidColorBrush` | `OnSurfaceLowBrush`           |
| `OutlinedTextBoxFontFamily`                       | `FontFamily`      | `MaterialMediumFontFamily`    |
| `OutlinedTextBoxFontWeight`                       | `String`          | `BodyLargeFontWeight`         |
| `OutlinedTextBoxFontSize`                         | `Double`          | `BodyLargeFontSize`           |
| `OutlinedTextBoxCharacterSpacing`                 | `Int32`           | `BodyLargeCharacterSpacing`   |
| `FilledTextBoxFontFamily`                         | `FontFamily`      | `MaterialMediumFontFamily`    |
| `FilledTextBoxFontWeight`                         | `String`          | `BodyLargeFontWeight`         |
| `FilledTextBoxFontSize`                           | `Double`          | `BodyLargeFontSize`           |
| `FilledTextBoxCharacterSpacing`                   | `Int32`           | `BodyLargeCharacterSpacing`   |
| `FilledTextBoxBorderThicknessNormal`              | `Double`          | `TextBoxOutlinedStrokeHeight` |
| `FilledTextBoxBorderThicknessFocused`             | `Double`          | `TextBoxFocusStrokeWidth`     |
| `TextBoxClearGlyphWidth`                          | `Double`          | 20                            |
| `TextBoxClearGlyphHeight`                         | `Double`          | 20                            |
| `FilledTextBoxCornerRadius`                       | `CornerRadius`    | 4,4,0,0                       |
| `FilledTextBoxPadding`                            | `Thickness`       | 16,7                          |
| `FilledTextBoxMinHeight`                          | `Double`          | 58                            |
| `FilledTextBoxBorderHeightFocused`                | `Double`          | 2                             |
| `OutlinedTextBoxBorderThickness`                  | `Double`          | 1                             |
| `OutlinedTextBoxCornerRadius`                     | `CornerRadius`    | 4                             |
| `OutlinedTextBoxPadding`                          | `Thickness`       | 8                             |
| `OutlinedTextBoxMinHeight`                        | `Double`          | 56                            |
| `OutlinedTextBoxBorderThicknessFocused`           | `Double`          | 2                             |
| `OutlinedTextBoxBorderThicknessPointerOver`       | `Double`          | 2                             |
