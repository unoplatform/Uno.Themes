---
uid: Uno.Themes.Simple.Styles.TextBox
---

# TextBox Control

## Styles

| Style Key                          | IsDefaultStyle\* |
|------------------------------------|------------------|
| `SimpleFilledTextBoxStyle`    |                  |
| `SimpleTextBoxErrorStyle`     |                  |
| `SimpleTextBoxSmallStyle`     |                  |
| `SimpleOutlinedTextBoxStyle`  | True             |

IsDefaultStyle\*: Styles in this column will be set as the default implicit style for the matching control

## Lightweight Styling

### Structural Resources

| Key                                                | Type              | Value                                      |
|----------------------------------------------------|-------------------|--------------------------------------------|
| `SimpleTextBoxHeaderMargin`                        | `Thickness`       | 0,0,0,4                                   |

### Simple Aliases

| Key                                                | Type              | Value                                      |
|----------------------------------------------------|-------------------|--------------------------------------------|
| `SimpleTextBoxCornerRadius`                        | `CornerRadius`    | `SimpleRadius200CornerRadius`              |
| `SimpleTextBoxBorderThickness`                     | `Thickness`       | `SimpleStrokeBorderThickness`              |
| `SimpleTextBoxFocusedBorderThickness`              | `Thickness`       | `SimpleStrokeFocusRingThickness`           |
| `SimpleTextBoxPadding`                             | `Thickness`       | 16,10,16,10                                |
| `SimpleTextBoxMinHeight`                           | `Double`          | `SimpleIconLarge`                          |
| `SimpleTextBoxSmallMinHeight`                      | `Double`          | `SimpleIconMedium`                         |

### Filled Variant -- Sizing

| Key                                                | Type              | Value                                      |
|----------------------------------------------------|-------------------|--------------------------------------------|
| `FilledTextBoxCornerRadius`                        | `CornerRadius`    | `SimpleRadius200CornerRadius`              |
| `FilledTextBoxBorderThicknessNormal`               | `Thickness`       | 0                                          |
| `FilledTextBoxBorderThicknessFocused`              | `Thickness`       | 0                                          |
| `FilledTextBoxPadding`                             | `Thickness`       | 16,10,16,10                                |
| `FilledTextBoxMinHeight`                           | `Double`          | `SimpleIconLarge`                          |

### Filled Variant -- Normal State

| Key                                                | Type              | Value                                      |
|----------------------------------------------------|-------------------|--------------------------------------------|
| `FilledTextBoxForeground`                          | `SolidColorBrush` | `OnSurfaceBrush`                           |
| `FilledTextBoxBackground`                          | `SolidColorBrush` | `SurfaceBrush`                             |
| `FilledTextBoxBorderBrush`                         | `SolidColorBrush` | `SystemControlTransparentBrush`            |
| `FilledTextBoxPlaceholderForeground`               | `SolidColorBrush` | `OnSurfaceLowBrush`                        |

### Filled Variant -- PointerOver State

| Key                                                | Type              | Value                                      |
|----------------------------------------------------|-------------------|--------------------------------------------|
| `FilledTextBoxForegroundPointerOver`               | `SolidColorBrush` | `OnSurfaceBrush`                           |
| `FilledTextBoxBackgroundPointerOver`               | `SolidColorBrush` | `SurfaceVariantBrush`                      |
| `FilledTextBoxBorderBrushPointerOver`              | `SolidColorBrush` | `SystemControlTransparentBrush`            |
| `FilledTextBoxPlaceholderForegroundPointerOver`    | `SolidColorBrush` | `OnSurfaceLowBrush`                        |

### Filled Variant -- Focused State

| Key                                                | Type              | Value                                      |
|----------------------------------------------------|-------------------|--------------------------------------------|
| `FilledTextBoxForegroundFocused`                   | `SolidColorBrush` | `OnSurfaceBrush`                           |
| `FilledTextBoxBackgroundFocused`                   | `SolidColorBrush` | `SurfaceBrush`                             |
| `FilledTextBoxBorderBrushFocused`                  | `SolidColorBrush` | `SystemControlTransparentBrush`            |
| `FilledTextBoxPlaceholderForegroundFocused`        | `SolidColorBrush` | `OnSurfaceLowBrush`                        |

### Filled Variant -- Disabled State

| Key                                                | Type              | Value                                      |
|----------------------------------------------------|-------------------|--------------------------------------------|
| `FilledTextBoxForegroundDisabled`                  | `SolidColorBrush` | `OnSurfaceDisabledBrush`                   |
| `FilledTextBoxBackgroundDisabled`                  | `SolidColorBrush` | `OnSurfaceDisabledBrush`                   |
| `FilledTextBoxBorderBrushDisabled`                 | `SolidColorBrush` | `SystemControlTransparentBrush`            |
| `FilledTextBoxPlaceholderForegroundDisabled`       | `SolidColorBrush` | `OnSurfaceDisabledBrush`                   |

### Filled Variant -- Header

| Key                                                | Type              | Value                                      |
|----------------------------------------------------|-------------------|--------------------------------------------|
| `FilledTextBoxHeaderForeground`                    | `SolidColorBrush` | `OnSurfaceBrush`                           |
| `FilledTextBoxHeaderForegroundPointerOver`         | `SolidColorBrush` | `OnSurfaceBrush`                           |
| `FilledTextBoxHeaderForegroundFocused`             | `SolidColorBrush` | `OnSurfaceBrush`                           |
| `FilledTextBoxHeaderForegroundDisabled`            | `SolidColorBrush` | `OnSurfaceDisabledBrush`                   |

### Outlined Variant -- Sizing

| Key                                                | Type              | Value                                      |
|----------------------------------------------------|-------------------|--------------------------------------------|
| `OutlinedTextBoxCornerRadius`                      | `CornerRadius`    | `SimpleRadius200CornerRadius`              |
| `OutlinedTextBoxBorderThickness`                   | `Thickness`       | `SimpleStrokeBorderThickness`              |
| `OutlinedTextBoxBorderThicknessFocused`            | `Thickness`       | `SimpleStrokeFocusRingThickness`           |
| `OutlinedTextBoxPadding`                           | `Thickness`       | 16,10,16,10                                |
| `OutlinedTextBoxMinHeight`                         | `Double`          | `SimpleIconLarge`                          |

### Outlined Variant -- Normal State

| Key                                                | Type              | Value                                      |
|----------------------------------------------------|-------------------|--------------------------------------------|
| `OutlinedTextBoxForeground`                        | `SolidColorBrush` | `OnSurfaceBrush`                           |
| `OutlinedTextBoxBackground`                        | `SolidColorBrush` | `SurfaceBrush`                             |
| `OutlinedTextBoxBorderBrush`                       | `SolidColorBrush` | `OutlineBrush`                             |
| `OutlinedTextBoxPlaceholderForeground`             | `SolidColorBrush` | `OnSurfaceLowBrush`                        |

### Outlined Variant -- PointerOver State

| Key                                                | Type              | Value                                      |
|----------------------------------------------------|-------------------|--------------------------------------------|
| `OutlinedTextBoxForegroundPointerOver`             | `SolidColorBrush` | `OnSurfaceBrush`                           |
| `OutlinedTextBoxBackgroundPointerOver`             | `SolidColorBrush` | `SurfaceBrush`                             |
| `OutlinedTextBoxBorderBrushPointerOver`            | `SolidColorBrush` | `OutlineBrush`                             |
| `OutlinedTextBoxPlaceholderForegroundPointerOver`  | `SolidColorBrush` | `OnSurfaceLowBrush`                        |

### Outlined Variant -- Focused State

| Key                                                | Type              | Value                                      |
|----------------------------------------------------|-------------------|--------------------------------------------|
| `OutlinedTextBoxForegroundFocused`                 | `SolidColorBrush` | `OnSurfaceBrush`                           |
| `OutlinedTextBoxBackgroundFocused`                 | `SolidColorBrush` | `SurfaceBrush`                             |
| `OutlinedTextBoxBorderBrushFocused`                | `SolidColorBrush` | `PrimaryMediumBrush`                       |
| `OutlinedTextBoxPlaceholderForegroundFocused`      | `SolidColorBrush` | `OnSurfaceLowBrush`                        |

### Outlined Variant -- Disabled State

| Key                                                | Type              | Value                                      |
|----------------------------------------------------|-------------------|--------------------------------------------|
| `OutlinedTextBoxForegroundDisabled`                | `SolidColorBrush` | `OnSurfaceDisabledBrush`                   |
| `OutlinedTextBoxBackgroundDisabled`                | `SolidColorBrush` | `OnSurfaceDisabledBrush`                   |
| `OutlinedTextBoxBorderBrushDisabled`               | `SolidColorBrush` | `OutlineDisabledBrush`                     |
| `OutlinedTextBoxPlaceholderForegroundDisabled`     | `SolidColorBrush` | `OnSurfaceDisabledBrush`                   |

### Outlined Variant -- Header

| Key                                                | Type              | Value                                      |
|----------------------------------------------------|-------------------|--------------------------------------------|
| `OutlinedTextBoxHeaderForeground`                  | `SolidColorBrush` | `OnSurfaceBrush`                           |
| `OutlinedTextBoxHeaderForegroundPointerOver`       | `SolidColorBrush` | `OnSurfaceBrush`                           |
| `OutlinedTextBoxHeaderForegroundFocused`           | `SolidColorBrush` | `OnSurfaceBrush`                           |
| `OutlinedTextBoxHeaderForegroundDisabled`          | `SolidColorBrush` | `OnSurfaceDisabledBrush`                   |
