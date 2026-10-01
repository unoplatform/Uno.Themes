---
uid: Uno.Themes.Styles.TextBox
---

# TextBox Control

<!-- BEGIN GENERATED -->
<!-- This region is generated from the Styles XAML by `dotnet run build/scripts/GenerateStyleDocs.cs`; do not edit it by hand. -->

## Styles

| Style Key                      | Semantic Alias         | IsDefaultStyle\* |
| ------------------------------ | ---------------------- | ---------------- |
| `MaterialFilledTextBoxStyle`   | `FilledTextBoxStyle`   |                  |
| `MaterialOutlinedTextBoxStyle` | `OutlinedTextBoxStyle` | True             |
| `MaterialDefaultTextBoxStyle`  |                        | True             |

IsDefaultStyle\*: Styles in this column will be set as the default implicit style for the matching control

## Lightweight Styling

### Theme-agnostic

| Key                     | Type     | Value                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                               |
| ----------------------- | -------- | --------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| `TextBoxClearGlyphData` | `String` | M14.9482 6.46442L13.534 5.05021L9.99849 8.58574L6.46296 5.05021L5.04874 6.46442L8.58428 9.99995L5.04874 13.5355L6.46296 14.9497L9.99849 11.4142L13.534 14.9497L14.9482 13.5355L11.4127 9.99995L14.9482 6.46442ZM17.0696 2.92889C13.1663 -0.974342 6.83065 -0.974342 2.92742 2.92889C-0.975807 6.83212 -0.975807 13.1678 2.92742 17.071C6.83065 20.9743 13.1663 20.9743 17.0696 17.071C20.9728 13.1678 20.9728 6.83212 17.0696 2.92889ZM4.34164 15.6568C1.22329 12.5385 1.22329 7.46144 4.34164 4.3431C7.45998 1.22476 12.537 1.22476 15.6553 4.3431C18.7737 7.46144 18.7737 12.5385 15.6553 15.6568C12.537 18.7752 7.45998 18.7752 4.34164 15.6568Z |

### Themed

| Key                                                           | Type              | Light                         |
| ------------------------------------------------------------- | ----------------- | ----------------------------- |
| `DownscaledHeaderCompositeTransformScaleX`                    | `Double`          | 0.7                           |
| `DownscaledHeaderCompositeTransformScaleY`                    | `Double`          | 0.7                           |
| `DownscaledHeaderHeaderCompositeTransformTranslateY`          | `Double`          | -11                           |
| `DownscaledHeaderPlaceholderTextCompositeTransformTranslateY` | `Double`          | 8                             |
| `DownscaledHeaderTextCompositeTransformTranslateY`            | `Double`          | 8                             |
| `FilledTextBoxBackground`                                     | `SolidColorBrush` | `SurfaceVariantBrush`         |
| `FilledTextBoxBackgroundDisabled`                             | `SolidColorBrush` | `OnSurfaceDisabledBrush`      |
| `FilledTextBoxBackgroundFocused`                              | `SolidColorBrush` | `SurfaceVariantBrush`         |
| `FilledTextBoxBackgroundPointerOver`                          | `SolidColorBrush` | `OnSurfaceVariantHoverBrush`  |
| `FilledTextBoxBorderBrush`                                    | `SolidColorBrush` | `OnSurfaceVariantBrush`       |
| `FilledTextBoxBorderBrushDisabled`                            | `SolidColorBrush` | `OnSurfaceDisabledBrush`      |
| `FilledTextBoxBorderBrushFocused`                             | `SolidColorBrush` | `PrimaryBrush`                |
| `FilledTextBoxBorderBrushPointerOver`                         | `SolidColorBrush` | `OnSurfaceBrush`              |
| `FilledTextBoxBorderHeightFocused`                            | `Double`          | 2                             |
| `FilledTextBoxBorderThicknessFocused`                         | `Double`          | `TextBoxFocusStrokeWidth`     |
| `FilledTextBoxBorderThicknessNormal`                          | `Double`          | `TextBoxOutlinedStrokeHeight` |
| `FilledTextBoxCharacterSpacing`                               | `Int32`           | `BodyLargeCharacterSpacing`   |
| `FilledTextBoxCornerRadius`                                   | `CornerRadius`    | 4,4,0,0                       |
| `FilledTextBoxDeleteButtonForeground`                         | `SolidColorBrush` | `OnSurfaceVariantBrush`       |
| `FilledTextBoxDeleteButtonForegroundDisabled`                 | `SolidColorBrush` | `OnSurfaceLowBrush`           |
| `FilledTextBoxDeleteButtonForegroundFocused`                  | `SolidColorBrush` | `OnSurfaceVariantBrush`       |
| `FilledTextBoxDeleteButtonForegroundPointerOver`              | `SolidColorBrush` | `OnSurfaceVariantBrush`       |
| `FilledTextBoxFontFamily`                                     | `FontFamily`      | `BodyLargeFontFamily`         |
| `FilledTextBoxFontSize`                                       | `Double`          | `BodyLargeFontSize`           |
| `FilledTextBoxFontWeight`                                     | `String`          | `BodyLargeFontWeight`         |
| `FilledTextBoxForeground`                                     | `SolidColorBrush` | `OnSurfaceBrush`              |
| `FilledTextBoxForegroundDisabled`                             | `SolidColorBrush` | `OnSurfaceBrush`              |
| `FilledTextBoxForegroundFocused`                              | `SolidColorBrush` | `OnSurfaceBrush`              |
| `FilledTextBoxForegroundOpacityDisabled`                      | `Double`          | `LowOpacity`                  |
| `FilledTextBoxForegroundPointerOver`                          | `SolidColorBrush` | `OnSurfaceBrush`              |
| `FilledTextBoxHeaderForeground`                               | `SolidColorBrush` | `OnSurfaceVariantBrush`       |
| `FilledTextBoxHeaderForegroundDisabled`                       | `SolidColorBrush` | `OnSurfaceLowBrush`           |
| `FilledTextBoxHeaderForegroundFocused`                        | `SolidColorBrush` | `PrimaryBrush`                |
| `FilledTextBoxHeaderForegroundPointerOver`                    | `SolidColorBrush` | `OnSurfaceVariantBrush`       |
| `FilledTextBoxMinHeight`                                      | `Double`          | 58                            |
| `FilledTextBoxPadding`                                        | `Thickness`       | 16,8                          |
| `FilledTextBoxPlaceholderForeground`                          | `SolidColorBrush` | `OnSurfaceVariantBrush`       |
| `FilledTextBoxPlaceholderForegroundDisabled`                  | `SolidColorBrush` | `OnSurfaceLowBrush`           |
| `FilledTextBoxPlaceholderForegroundFocused`                   | `SolidColorBrush` | `OnSurfaceBrush`              |
| `FilledTextBoxPlaceholderForegroundPointerOver`               | `SolidColorBrush` | `OnSurfaceVariantBrush`       |
| `OutlinedTextBoxBorderBrush`                                  | `SolidColorBrush` | `OutlineBrush`                |
| `OutlinedTextBoxBorderBrushDisabled`                          | `SolidColorBrush` | `OnSurfaceDisabledBrush`      |
| `OutlinedTextBoxBorderBrushFocused`                           | `SolidColorBrush` | `PrimaryBrush`                |
| `OutlinedTextBoxBorderBrushPointerOver`                       | `SolidColorBrush` | `OnSurfaceBrush`              |
| `OutlinedTextBoxBorderThickness`                              | `Double`          | 1                             |
| `OutlinedTextBoxBorderThicknessFocused`                       | `Double`          | 2                             |
| `OutlinedTextBoxBorderThicknessPointerOver`                   | `Double`          | 2                             |
| `OutlinedTextBoxCharacterSpacing`                             | `Int32`           | `BodyLargeCharacterSpacing`   |
| `OutlinedTextBoxCornerRadius`                                 | `CornerRadius`    | `Radius100CornerRadius`       |
| `OutlinedTextBoxFontFamily`                                   | `FontFamily`      | `BodyLargeFontFamily`         |
| `OutlinedTextBoxFontSize`                                     | `Double`          | `BodyLargeFontSize`           |
| `OutlinedTextBoxFontWeight`                                   | `String`          | `BodyLargeFontWeight`         |
| `OutlinedTextBoxForeground`                                   | `SolidColorBrush` | `OnSurfaceBrush`              |
| `OutlinedTextBoxForegroundDisabled`                           | `SolidColorBrush` | `OnSurfaceBrush`              |
| `OutlinedTextBoxForegroundFocused`                            | `SolidColorBrush` | `OnSurfaceBrush`              |
| `OutlinedTextBoxForegroundOpacityDisabled`                    | `Double`          | `LowOpacity`                  |
| `OutlinedTextBoxForegroundPointerOver`                        | `SolidColorBrush` | `OnSurfaceBrush`              |
| `OutlinedTextBoxHeaderForeground`                             | `SolidColorBrush` | `OnSurfaceVariantBrush`       |
| `OutlinedTextBoxHeaderForegroundDisabled`                     | `SolidColorBrush` | `OnSurfaceLowBrush`           |
| `OutlinedTextBoxHeaderForegroundFocused`                      | `SolidColorBrush` | `PrimaryBrush`                |
| `OutlinedTextBoxHeaderForegroundPointerOver`                  | `SolidColorBrush` | `OnSurfaceVariantBrush`       |
| `OutlinedTextBoxMinHeight`                                    | `Double`          | 56                            |
| `OutlinedTextBoxPadding`                                      | `Thickness`       | `Space200Thickness`           |
| `OutlinedTextBoxPlaceholderForeground`                        | `SolidColorBrush` | `OnSurfaceVariantBrush`       |
| `OutlinedTextBoxPlaceholderForegroundDisabled`                | `SolidColorBrush` | `OnSurfaceLowBrush`           |
| `OutlinedTextBoxPlaceholderForegroundFocused`                 | `SolidColorBrush` | `OnSurfaceBrush`              |
| `OutlinedTextBoxPlaceholderForegroundPointerOver`             | `SolidColorBrush` | `OnSurfaceVariantBrush`       |
| `TextBoxClearGlyphHeight`                                     | `Double`          | 20                            |
| `TextBoxClearGlyphWidth`                                      | `Double`          | 20                            |
| `TextBoxDeleteButtonForeground`                               | `SolidColorBrush` | `OnSurfaceVariantBrush`       |
| `TextBoxDeleteButtonForegroundDisabled`                       | `SolidColorBrush` | `OnSurfaceLowBrush`           |
| `TextBoxDeleteButtonForegroundPointerOver`                    | `SolidColorBrush` | `OnSurfaceVariantBrush`       |
| `TextBoxDeleteButtonForegroundPressed`                        | `SolidColorBrush` | `OnSurfaceVariantBrush`       |
| `TextBoxLeadingIconForeground`                                | `SolidColorBrush` | `OnSurfaceVariantBrush`       |
| `TextBoxLeadingIconForegroundDisabled`                        | `SolidColorBrush` | `OnSurfaceLowBrush`           |

The Dark theme uses the same values as Light.

<!-- END GENERATED -->
