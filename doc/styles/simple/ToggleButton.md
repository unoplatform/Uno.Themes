---
uid: Uno.Themes.Simple.Styles.ToggleButton
---

# ToggleButton Control

> [!NOTE]
> For every `TextToggleButton` foreground, background, and border brush below, an `IconToggleButton` key with the same suffix supplies the icon variant (36 keys covering normal, pointer-over, pressed, and disabled for unchecked, checked, and indeterminate). Each defaults to its matching text-toggle brush. See [semantic override compatibility](../../semantic-styles.md#override-compatibility).

<!-- BEGIN GENERATED -->
<!-- This region is generated from the Styles XAML by `dotnet run build/scripts/GenerateStyleDocs.cs`; do not edit it by hand. -->

## Styles

| Style Key                        | Semantic Alias          | IsDefaultStyle\* |
| -------------------------------- | ----------------------- | ---------------- |
| `SimpleTextToggleButtonStyle`    | `TextToggleButtonStyle` | True             |
| `SimpleIconToggleButtonStyle`    | `IconToggleButtonStyle` |                  |
| `SimpleDefaultToggleButtonStyle` |                         | True             |

IsDefaultStyle\*: Styles in this column will be set as the default implicit style for the matching control

## Lightweight Styling

### Theme-agnostic

| Key                                 | Type           | Value                            |
| ----------------------------------- | -------------- | -------------------------------- |
| `SimpleToggleButtonBorderThickness` | `Thickness`    | `SimpleStrokeBorderThickness`    |
| `SimpleToggleButtonCornerRadius`    | `CornerRadius` | `SimpleShapeControlCornerRadius` |
| `SimpleToggleButtonFontWeight`      | `String`       | Medium                           |
| `SimpleToggleButtonIconSpacing`     | `Double`       | `SimpleSpace100`                 |

### Themed

| Key                                                   | Type              | Light                                                 |
| ----------------------------------------------------- | ----------------- | ----------------------------------------------------- |
| `IconToggleButtonBackground`                          | `SolidColorBrush` | `TextToggleButtonBackground`                          |
| `IconToggleButtonBackgroundChecked`                   | `SolidColorBrush` | `TextToggleButtonBackgroundChecked`                   |
| `IconToggleButtonBackgroundCheckedDisabled`           | `SolidColorBrush` | `TextToggleButtonBackgroundCheckedDisabled`           |
| `IconToggleButtonBackgroundCheckedPointerOver`        | `SolidColorBrush` | `TextToggleButtonBackgroundCheckedPointerOver`        |
| `IconToggleButtonBackgroundCheckedPressed`            | `SolidColorBrush` | `TextToggleButtonBackgroundCheckedPressed`            |
| `IconToggleButtonBackgroundDisabled`                  | `SolidColorBrush` | `TextToggleButtonBackgroundDisabled`                  |
| `IconToggleButtonBackgroundIndeterminate`             | `SolidColorBrush` | `TextToggleButtonBackgroundIndeterminate`             |
| `IconToggleButtonBackgroundIndeterminateDisabled`     | `SolidColorBrush` | `TextToggleButtonBackgroundIndeterminateDisabled`     |
| `IconToggleButtonBackgroundIndeterminatePointerOver`  | `SolidColorBrush` | `TextToggleButtonBackgroundIndeterminatePointerOver`  |
| `IconToggleButtonBackgroundIndeterminatePressed`      | `SolidColorBrush` | `TextToggleButtonBackgroundIndeterminatePressed`      |
| `IconToggleButtonBackgroundPointerOver`               | `SolidColorBrush` | `TextToggleButtonBackgroundPointerOver`               |
| `IconToggleButtonBackgroundPressed`                   | `SolidColorBrush` | `TextToggleButtonBackgroundPressed`                   |
| `IconToggleButtonBorderBrush`                         | `SolidColorBrush` | `TextToggleButtonBorderBrush`                         |
| `IconToggleButtonBorderBrushChecked`                  | `SolidColorBrush` | `TextToggleButtonBorderBrushChecked`                  |
| `IconToggleButtonBorderBrushCheckedDisabled`          | `SolidColorBrush` | `TextToggleButtonBorderBrushCheckedDisabled`          |
| `IconToggleButtonBorderBrushCheckedPointerOver`       | `SolidColorBrush` | `TextToggleButtonBorderBrushCheckedPointerOver`       |
| `IconToggleButtonBorderBrushCheckedPressed`           | `SolidColorBrush` | `TextToggleButtonBorderBrushCheckedPressed`           |
| `IconToggleButtonBorderBrushDisabled`                 | `SolidColorBrush` | `TextToggleButtonBorderBrushDisabled`                 |
| `IconToggleButtonBorderBrushIndeterminate`            | `SolidColorBrush` | `TextToggleButtonBorderBrushIndeterminate`            |
| `IconToggleButtonBorderBrushIndeterminateDisabled`    | `SolidColorBrush` | `TextToggleButtonBorderBrushIndeterminateDisabled`    |
| `IconToggleButtonBorderBrushIndeterminatePointerOver` | `SolidColorBrush` | `TextToggleButtonBorderBrushIndeterminatePointerOver` |
| `IconToggleButtonBorderBrushIndeterminatePressed`     | `SolidColorBrush` | `TextToggleButtonBorderBrushIndeterminatePressed`     |
| `IconToggleButtonBorderBrushPointerOver`              | `SolidColorBrush` | `TextToggleButtonBorderBrushPointerOver`              |
| `IconToggleButtonBorderBrushPressed`                  | `SolidColorBrush` | `TextToggleButtonBorderBrushPressed`                  |
| `IconToggleButtonForeground`                          | `SolidColorBrush` | `TextToggleButtonForeground`                          |
| `IconToggleButtonForegroundChecked`                   | `SolidColorBrush` | `TextToggleButtonForegroundChecked`                   |
| `IconToggleButtonForegroundCheckedDisabled`           | `SolidColorBrush` | `TextToggleButtonForegroundCheckedDisabled`           |
| `IconToggleButtonForegroundCheckedPointerOver`        | `SolidColorBrush` | `TextToggleButtonForegroundCheckedPointerOver`        |
| `IconToggleButtonForegroundCheckedPressed`            | `SolidColorBrush` | `TextToggleButtonForegroundCheckedPressed`            |
| `IconToggleButtonForegroundDisabled`                  | `SolidColorBrush` | `TextToggleButtonForegroundDisabled`                  |
| `IconToggleButtonForegroundIndeterminate`             | `SolidColorBrush` | `TextToggleButtonForegroundIndeterminate`             |
| `IconToggleButtonForegroundIndeterminateDisabled`     | `SolidColorBrush` | `TextToggleButtonForegroundIndeterminateDisabled`     |
| `IconToggleButtonForegroundIndeterminatePointerOver`  | `SolidColorBrush` | `TextToggleButtonForegroundIndeterminatePointerOver`  |
| `IconToggleButtonForegroundIndeterminatePressed`      | `SolidColorBrush` | `TextToggleButtonForegroundIndeterminatePressed`      |
| `IconToggleButtonForegroundPointerOver`               | `SolidColorBrush` | `TextToggleButtonForegroundPointerOver`               |
| `IconToggleButtonForegroundPressed`                   | `SolidColorBrush` | `TextToggleButtonForegroundPressed`                   |
| `SimpleToggleButtonFontFamily`                        | `FontFamily`      | `DefaultFontFamily`                                   |
| `TextToggleButtonBackground`                          | `SolidColorBrush` | `SurfaceVariantBrush`                                 |
| `TextToggleButtonBackgroundChecked`                   | `SolidColorBrush` | `PrimaryBrush`                                        |
| `TextToggleButtonBackgroundCheckedDisabled`           | `SolidColorBrush` | `OnSurfaceDisabledBrush`                              |
| `TextToggleButtonBackgroundCheckedPointerOver`        | `SolidColorBrush` | `PrimaryVariantDarkBrush`                             |
| `TextToggleButtonBackgroundCheckedPressed`            | `SolidColorBrush` | `PrimaryVariantDarkBrush`                             |
| `TextToggleButtonBackgroundDisabled`                  | `SolidColorBrush` | `OnSurfaceDisabledBrush`                              |
| `TextToggleButtonBackgroundIndeterminate`             | `SolidColorBrush` | `SurfaceVariantBrush`                                 |
| `TextToggleButtonBackgroundIndeterminateDisabled`     | `SolidColorBrush` | `OnSurfaceDisabledBrush`                              |
| `TextToggleButtonBackgroundIndeterminatePointerOver`  | `SolidColorBrush` | `PrimaryContainerBrush`                               |
| `TextToggleButtonBackgroundIndeterminatePressed`      | `SolidColorBrush` | `TertiaryContainerBrush`                              |
| `TextToggleButtonBackgroundPointerOver`               | `SolidColorBrush` | `PrimaryContainerBrush`                               |
| `TextToggleButtonBackgroundPressed`                   | `SolidColorBrush` | `TertiaryContainerBrush`                              |
| `TextToggleButtonBorderBrush`                         | `SolidColorBrush` | `OutlineBrush`                                        |
| `TextToggleButtonBorderBrushChecked`                  | `SolidColorBrush` | `PrimaryBrush`                                        |
| `TextToggleButtonBorderBrushCheckedDisabled`          | `SolidColorBrush` | `OutlineDisabledBrush`                                |
| `TextToggleButtonBorderBrushCheckedPointerOver`       | `SolidColorBrush` | `PrimaryBrush`                                        |
| `TextToggleButtonBorderBrushCheckedPressed`           | `SolidColorBrush` | `PrimaryBrush`                                        |
| `TextToggleButtonBorderBrushDisabled`                 | `SolidColorBrush` | `OutlineDisabledBrush`                                |
| `TextToggleButtonBorderBrushIndeterminate`            | `SolidColorBrush` | `OutlineBrush`                                        |
| `TextToggleButtonBorderBrushIndeterminateDisabled`    | `SolidColorBrush` | `OutlineDisabledBrush`                                |
| `TextToggleButtonBorderBrushIndeterminatePointerOver` | `SolidColorBrush` | `OutlineBrush`                                        |
| `TextToggleButtonBorderBrushIndeterminatePressed`     | `SolidColorBrush` | `OutlineBrush`                                        |
| `TextToggleButtonBorderBrushPointerOver`              | `SolidColorBrush` | `OutlineBrush`                                        |
| `TextToggleButtonBorderBrushPressed`                  | `SolidColorBrush` | `OutlineBrush`                                        |
| `TextToggleButtonForeground`                          | `SolidColorBrush` | `OnSurfaceBrush`                                      |
| `TextToggleButtonForegroundChecked`                   | `SolidColorBrush` | `OnPrimaryBrush`                                      |
| `TextToggleButtonForegroundCheckedDisabled`           | `SolidColorBrush` | `OnSurfaceDisabledBrush`                              |
| `TextToggleButtonForegroundCheckedPointerOver`        | `SolidColorBrush` | `OnPrimaryBrush`                                      |
| `TextToggleButtonForegroundCheckedPressed`            | `SolidColorBrush` | `OnPrimaryBrush`                                      |
| `TextToggleButtonForegroundDisabled`                  | `SolidColorBrush` | `OnSurfaceDisabledBrush`                              |
| `TextToggleButtonForegroundIndeterminate`             | `SolidColorBrush` | `OnSurfaceBrush`                                      |
| `TextToggleButtonForegroundIndeterminateDisabled`     | `SolidColorBrush` | `OnSurfaceDisabledBrush`                              |
| `TextToggleButtonForegroundIndeterminatePointerOver`  | `SolidColorBrush` | `OnSurfaceBrush`                                      |
| `TextToggleButtonForegroundIndeterminatePressed`      | `SolidColorBrush` | `OnSurfaceBrush`                                      |
| `TextToggleButtonForegroundPointerOver`               | `SolidColorBrush` | `OnSurfaceBrush`                                      |
| `TextToggleButtonForegroundPressed`                   | `SolidColorBrush` | `OnSurfaceBrush`                                      |

The Dark theme uses the same values as Light.

<!-- END GENERATED -->
