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
| `SimpleTextBoxCornerRadius`                        | `CornerRadius`    | `FilledTextBoxCornerRadius`                |
| `SimpleTextBoxBorderThickness`                     | `Thickness`       | `FilledTextBoxBorderThicknessNormal`       |
| `SimpleTextBoxFocusedBorderThickness`              | `Thickness`       | `FilledTextBoxBorderThicknessFocused`      |
| `SimpleTextBoxPadding`                             | `Thickness`       | `FilledTextBoxPadding`                     |
| `SimpleTextBoxMinHeight`                           | `Double`          | `FilledTextBoxMinHeight`                   |
| `SimpleTextBoxSmallMinHeight`                      | `Double`          | `SimpleIconMedium`                         |

### Filled Variant -- Sizing

| Key                                                | Type              | Value                                      |
|----------------------------------------------------|-------------------|--------------------------------------------|
| `FilledTextBoxCornerRadius`                        | `CornerRadius`    | `SimpleRadius200CornerRadius`              |
| `FilledTextBoxBorderThicknessNormal`               | `Thickness`       | `SimpleStrokeBorderThickness`              |
| `FilledTextBoxBorderThicknessFocused`              | `Thickness`       | `SimpleStrokeFocusRingThickness`           |
| `FilledTextBoxPadding`                             | `Thickness`       | 16,10,16,10                                |
| `FilledTextBoxMinHeight`                           | `Double`          | `SimpleIconLarge`                          |

### Filled Variant -- Normal State

| Key                                                | Type              | Value                                      |
|----------------------------------------------------|-------------------|--------------------------------------------|
| `FilledTextBoxForeground`                          | `SolidColorBrush` | `OnSurfaceBrush`                           |
| `FilledTextBoxBackground`                          | `SolidColorBrush` | `SurfaceBrush`                             |
| `FilledTextBoxBorderBrush`                         | `SolidColorBrush` | `OutlineBrush`                             |
| `FilledTextBoxPlaceholderForeground`               | `SolidColorBrush` | `OnSurfaceLowBrush`                        |

### Filled Variant -- PointerOver State

| Key                                                | Type              | Value                                      |
|----------------------------------------------------|-------------------|--------------------------------------------|
| `FilledTextBoxForegroundPointerOver`               | `SolidColorBrush` | `OnSurfaceBrush`                           |
| `FilledTextBoxBackgroundPointerOver`               | `SolidColorBrush` | `SurfaceBrush`                             |
| `FilledTextBoxBorderBrushPointerOver`              | `SolidColorBrush` | `OutlineBrush`                             |
| `FilledTextBoxPlaceholderForegroundPointerOver`    | `SolidColorBrush` | `OnSurfaceLowBrush`                        |

### Filled Variant -- Focused State

| Key                                                | Type              | Value                                      |
|----------------------------------------------------|-------------------|--------------------------------------------|
| `FilledTextBoxForegroundFocused`                   | `SolidColorBrush` | `OnSurfaceBrush`                           |
| `FilledTextBoxBackgroundFocused`                   | `SolidColorBrush` | `SurfaceBrush`                             |
| `FilledTextBoxBorderBrushFocused`                  | `SolidColorBrush` | `PrimaryMediumBrush`                       |
| `FilledTextBoxPlaceholderForegroundFocused`        | `SolidColorBrush` | `OnSurfaceLowBrush`                        |

### Filled Variant -- Disabled State

| Key                                                | Type              | Value                                      |
|----------------------------------------------------|-------------------|--------------------------------------------|
| `FilledTextBoxForegroundDisabled`                  | `SolidColorBrush` | `OnSurfaceDisabledBrush`                   |
| `FilledTextBoxBackgroundDisabled`                  | `SolidColorBrush` | `OnSurfaceDisabledBrush`                   |
| `FilledTextBoxBorderBrushDisabled`                 | `SolidColorBrush` | `OutlineDisabledBrush`                     |
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
| `OutlinedTextBoxCornerRadius`                      | `CornerRadius`    | `FilledTextBoxCornerRadius`                |
| `OutlinedTextBoxBorderThickness`                   | `Thickness`       | `FilledTextBoxBorderThicknessNormal`       |
| `OutlinedTextBoxBorderThicknessFocused`            | `Thickness`       | `FilledTextBoxBorderThicknessFocused`      |
| `OutlinedTextBoxPadding`                           | `Thickness`       | `FilledTextBoxPadding`                     |
| `OutlinedTextBoxMinHeight`                         | `Double`          | `FilledTextBoxMinHeight`                   |

### Outlined Variant -- Normal State

| Key                                                | Type              | Value                                      |
|----------------------------------------------------|-------------------|--------------------------------------------|
| `OutlinedTextBoxForeground`                        | `SolidColorBrush` | `FilledTextBoxForeground`                  |
| `OutlinedTextBoxBackground`                        | `SolidColorBrush` | `SurfaceBrush`                             |
| `OutlinedTextBoxBorderBrush`                       | `SolidColorBrush` | `FilledTextBoxBorderBrush`                 |
| `OutlinedTextBoxPlaceholderForeground`             | `SolidColorBrush` | `FilledTextBoxPlaceholderForeground`       |

### Outlined Variant -- PointerOver State

| Key                                                | Type              | Value                                      |
|----------------------------------------------------|-------------------|--------------------------------------------|
| `OutlinedTextBoxForegroundPointerOver`             | `SolidColorBrush` | `FilledTextBoxForegroundPointerOver`       |
| `OutlinedTextBoxBackgroundPointerOver`             | `SolidColorBrush` | `SurfaceBrush`                             |
| `OutlinedTextBoxBorderBrushPointerOver`            | `SolidColorBrush` | `FilledTextBoxBorderBrushPointerOver`      |
| `OutlinedTextBoxPlaceholderForegroundPointerOver`  | `SolidColorBrush` | `FilledTextBoxPlaceholderForegroundPointerOver` |

### Outlined Variant -- Focused State

| Key                                                | Type              | Value                                      |
|----------------------------------------------------|-------------------|--------------------------------------------|
| `OutlinedTextBoxForegroundFocused`                 | `SolidColorBrush` | `FilledTextBoxForegroundFocused`           |
| `OutlinedTextBoxBackgroundFocused`                 | `SolidColorBrush` | `SurfaceBrush`                             |
| `OutlinedTextBoxBorderBrushFocused`                | `SolidColorBrush` | `FilledTextBoxBorderBrushFocused`          |
| `OutlinedTextBoxPlaceholderForegroundFocused`      | `SolidColorBrush` | `FilledTextBoxPlaceholderForegroundFocused` |

### Outlined Variant -- Disabled State

| Key                                                | Type              | Value                                      |
|----------------------------------------------------|-------------------|--------------------------------------------|
| `OutlinedTextBoxForegroundDisabled`                | `SolidColorBrush` | `FilledTextBoxForegroundDisabled`          |
| `OutlinedTextBoxBackgroundDisabled`                | `SolidColorBrush` | `OnSurfaceDisabledBrush`                   |
| `OutlinedTextBoxBorderBrushDisabled`               | `SolidColorBrush` | `FilledTextBoxBorderBrushDisabled`         |
| `OutlinedTextBoxPlaceholderForegroundDisabled`     | `SolidColorBrush` | `FilledTextBoxPlaceholderForegroundDisabled` |

### Outlined Variant -- Header

| Key                                                | Type              | Value                                      |
|----------------------------------------------------|-------------------|--------------------------------------------|
| `OutlinedTextBoxHeaderForeground`                  | `SolidColorBrush` | `FilledTextBoxHeaderForeground`            |
| `OutlinedTextBoxHeaderForegroundPointerOver`       | `SolidColorBrush` | `FilledTextBoxHeaderForegroundPointerOver` |
| `OutlinedTextBoxHeaderForegroundFocused`           | `SolidColorBrush` | `FilledTextBoxHeaderForegroundFocused`     |
| `OutlinedTextBoxHeaderForegroundDisabled`          | `SolidColorBrush` | `FilledTextBoxHeaderForegroundDisabled`    |
