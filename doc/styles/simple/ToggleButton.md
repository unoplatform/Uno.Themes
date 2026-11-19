---
uid: Uno.Themes.Simple.Styles.ToggleButton
---

# ToggleButton Control

Both Simple variants consume `TextToggleButton*` brush resources. The separate icon template does not read Material's `IconToggleButton*` brush family, so shared overrides using those keys are ineffective under Simple. The default style derives from the text variant; see [semantic override compatibility](../../semantic-styles.md#override-compatibility).

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

| Key                                                   | Type              | Light                     |
| ----------------------------------------------------- | ----------------- | ------------------------- |
| `SimpleToggleButtonFontFamily`                        | `FontFamily`      | `DefaultFontFamily`       |
| `TextToggleButtonBackground`                          | `SolidColorBrush` | `SurfaceVariantBrush`     |
| `TextToggleButtonBackgroundChecked`                   | `SolidColorBrush` | `PrimaryBrush`            |
| `TextToggleButtonBackgroundCheckedDisabled`           | `SolidColorBrush` | `OnSurfaceDisabledBrush`  |
| `TextToggleButtonBackgroundCheckedPointerOver`        | `SolidColorBrush` | `PrimaryVariantDarkBrush` |
| `TextToggleButtonBackgroundCheckedPressed`            | `SolidColorBrush` | `PrimaryVariantDarkBrush` |
| `TextToggleButtonBackgroundDisabled`                  | `SolidColorBrush` | `OnSurfaceDisabledBrush`  |
| `TextToggleButtonBackgroundIndeterminate`             | `SolidColorBrush` | `SurfaceVariantBrush`     |
| `TextToggleButtonBackgroundIndeterminateDisabled`     | `SolidColorBrush` | `OnSurfaceDisabledBrush`  |
| `TextToggleButtonBackgroundIndeterminatePointerOver`  | `SolidColorBrush` | `PrimaryContainerBrush`   |
| `TextToggleButtonBackgroundIndeterminatePressed`      | `SolidColorBrush` | `TertiaryContainerBrush`  |
| `TextToggleButtonBackgroundPointerOver`               | `SolidColorBrush` | `PrimaryContainerBrush`   |
| `TextToggleButtonBackgroundPressed`                   | `SolidColorBrush` | `TertiaryContainerBrush`  |
| `TextToggleButtonBorderBrush`                         | `SolidColorBrush` | `OutlineBrush`            |
| `TextToggleButtonBorderBrushChecked`                  | `SolidColorBrush` | `PrimaryBrush`            |
| `TextToggleButtonBorderBrushCheckedDisabled`          | `SolidColorBrush` | `OutlineDisabledBrush`    |
| `TextToggleButtonBorderBrushCheckedPointerOver`       | `SolidColorBrush` | `PrimaryBrush`            |
| `TextToggleButtonBorderBrushCheckedPressed`           | `SolidColorBrush` | `PrimaryBrush`            |
| `TextToggleButtonBorderBrushDisabled`                 | `SolidColorBrush` | `OutlineDisabledBrush`    |
| `TextToggleButtonBorderBrushIndeterminate`            | `SolidColorBrush` | `OutlineBrush`            |
| `TextToggleButtonBorderBrushIndeterminateDisabled`    | `SolidColorBrush` | `OutlineDisabledBrush`    |
| `TextToggleButtonBorderBrushIndeterminatePointerOver` | `SolidColorBrush` | `OutlineBrush`            |
| `TextToggleButtonBorderBrushIndeterminatePressed`     | `SolidColorBrush` | `OutlineBrush`            |
| `TextToggleButtonBorderBrushPointerOver`              | `SolidColorBrush` | `OutlineBrush`            |
| `TextToggleButtonBorderBrushPressed`                  | `SolidColorBrush` | `OutlineBrush`            |
| `TextToggleButtonForeground`                          | `SolidColorBrush` | `OnSurfaceBrush`          |
| `TextToggleButtonForegroundChecked`                   | `SolidColorBrush` | `OnPrimaryBrush`          |
| `TextToggleButtonForegroundCheckedDisabled`           | `SolidColorBrush` | `OnSurfaceDisabledBrush`  |
| `TextToggleButtonForegroundCheckedPointerOver`        | `SolidColorBrush` | `OnPrimaryBrush`          |
| `TextToggleButtonForegroundCheckedPressed`            | `SolidColorBrush` | `OnPrimaryBrush`          |
| `TextToggleButtonForegroundDisabled`                  | `SolidColorBrush` | `OnSurfaceDisabledBrush`  |
| `TextToggleButtonForegroundIndeterminate`             | `SolidColorBrush` | `OnSurfaceBrush`          |
| `TextToggleButtonForegroundIndeterminateDisabled`     | `SolidColorBrush` | `OnSurfaceDisabledBrush`  |
| `TextToggleButtonForegroundIndeterminatePointerOver`  | `SolidColorBrush` | `OnSurfaceBrush`          |
| `TextToggleButtonForegroundIndeterminatePressed`      | `SolidColorBrush` | `OnSurfaceBrush`          |
| `TextToggleButtonForegroundPointerOver`               | `SolidColorBrush` | `OnSurfaceBrush`          |
| `TextToggleButtonForegroundPressed`                   | `SolidColorBrush` | `OnSurfaceBrush`          |

The Dark theme uses the same values as Light.

<!-- END GENERATED -->
