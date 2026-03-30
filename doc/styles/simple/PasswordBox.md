---
uid: Uno.Themes.Simple.Styles.PasswordBox
---

# PasswordBox Control

## Styles

| Style Key                          | IsDefaultStyle\* |
|------------------------------------|------------------|
| `SimplePasswordBoxStyle`           | True             |
| `SimpleOutlinedPasswordBoxStyle`   |                  |

IsDefaultStyle\*: Styles in this column will be set as the default implicit style for the matching control

## Lightweight Styling

### Simple Aliases

| Key                                                    | Type              | Value                                      |
|--------------------------------------------------------|-------------------|--------------------------------------------|
| `SimplePasswordBoxCornerRadius`                        | `CornerRadius`    | `FilledPasswordBoxCornerRadius`            |
| `SimplePasswordBoxBorderThickness`                     | `Thickness`       | `FilledPasswordBoxBorderThickness`         |
| `SimplePasswordBoxFocusedBorderThickness`              | `Thickness`       | `FilledPasswordBoxFocusedBorderThickness`  |
| `SimplePasswordBoxPadding`                             | `Thickness`       | `FilledPasswordBoxPadding`                 |
| `SimplePasswordBoxMinHeight`                           | `Double`          | `FilledPasswordBoxMinHeight`               |

### Filled Variant -- Sizing

| Key                                                    | Type              | Value                                      |
|--------------------------------------------------------|-------------------|--------------------------------------------|
| `FilledPasswordBoxCornerRadius`                        | `CornerRadius`    | `SimpleRadius200CornerRadius`              |
| `FilledPasswordBoxBorderThickness`                     | `Thickness`       | `SimpleStrokeBorderThickness`              |
| `FilledPasswordBoxFocusedBorderThickness`              | `Thickness`       | `SimpleStrokeFocusRingThickness`           |
| `FilledPasswordBoxPadding`                             | `Thickness`       | 16,10,16,10                                |
| `FilledPasswordBoxMinHeight`                           | `Double`          | `SimpleIconLarge`                          |
| `FilledPasswordBoxHeaderMargin`                        | `Thickness`       | 0,0,0,4                                   |

### Filled Variant -- Normal State

| Key                                                    | Type              | Value                                      |
|--------------------------------------------------------|-------------------|--------------------------------------------|
| `FilledPasswordBoxForeground`                          | `SolidColorBrush` | `OnSurfaceBrush`                           |
| `FilledPasswordBoxBackground`                          | `SolidColorBrush` | `SurfaceBrush`                             |
| `FilledPasswordBoxBorderBrush`                         | `SolidColorBrush` | `OutlineBrush`                             |
| `FilledPasswordBoxPlaceholderForeground`               | `SolidColorBrush` | `OnSurfaceLowBrush`                        |

### Filled Variant -- PointerOver State

| Key                                                    | Type              | Value                                      |
|--------------------------------------------------------|-------------------|--------------------------------------------|
| `FilledPasswordBoxForegroundPointerOver`               | `SolidColorBrush` | `OnSurfaceBrush`                           |
| `FilledPasswordBoxBackgroundPointerOver`               | `SolidColorBrush` | `SurfaceBrush`                             |
| `FilledPasswordBoxBorderBrushPointerOver`              | `SolidColorBrush` | `OutlineBrush`                             |

### Filled Variant -- Focused State

| Key                                                    | Type              | Value                                      |
|--------------------------------------------------------|-------------------|--------------------------------------------|
| `FilledPasswordBoxForegroundFocused`                   | `SolidColorBrush` | `OnSurfaceBrush`                           |
| `FilledPasswordBoxBackgroundFocused`                   | `SolidColorBrush` | `SurfaceBrush`                             |
| `FilledPasswordBoxBorderBrushFocused`                  | `SolidColorBrush` | `PrimaryMediumBrush`                       |

### Filled Variant -- Disabled State

| Key                                                    | Type              | Value                                      |
|--------------------------------------------------------|-------------------|--------------------------------------------|
| `FilledPasswordBoxForegroundDisabled`                  | `SolidColorBrush` | `OnSurfaceDisabledBrush`                   |
| `FilledPasswordBoxBackgroundDisabled`                  | `SolidColorBrush` | `OnSurfaceDisabledBrush`                   |
| `FilledPasswordBoxBorderBrushDisabled`                 | `SolidColorBrush` | `OutlineDisabledBrush`                     |
| `FilledPasswordBoxPlaceholderForegroundDisabled`       | `SolidColorBrush` | `OnSurfaceDisabledBrush`                   |

### Filled Variant -- Reveal Button

| Key                                                    | Type              | Value                                      |
|--------------------------------------------------------|-------------------|--------------------------------------------|
| `FilledPasswordBoxRevealForeground`                    | `SolidColorBrush` | `OnSurfaceBrush`                           |
| `FilledPasswordBoxRevealForegroundPointerOver`         | `SolidColorBrush` | `OnSurfaceBrush`                           |
| `FilledPasswordBoxRevealForegroundPressed`             | `SolidColorBrush` | `OnSurfaceBrush`                           |

### Filled Variant -- Header

| Key                                                    | Type              | Value                                      |
|--------------------------------------------------------|-------------------|--------------------------------------------|
| `FilledPasswordBoxHeaderForeground`                    | `SolidColorBrush` | `OnSurfaceBrush`                           |
| `FilledPasswordBoxHeaderForegroundDisabled`            | `SolidColorBrush` | `OnSurfaceDisabledBrush`                   |

### Outlined Variant -- Sizing

| Key                                                    | Type              | Value                                      |
|--------------------------------------------------------|-------------------|--------------------------------------------|
| `OutlinedPasswordBoxCornerRadius`                      | `CornerRadius`    | `FilledPasswordBoxCornerRadius`            |
| `OutlinedPasswordBoxBorderThickness`                   | `Thickness`       | `FilledPasswordBoxBorderThickness`         |
| `OutlinedPasswordBoxFocusedBorderThickness`            | `Thickness`       | `FilledPasswordBoxFocusedBorderThickness`  |
| `OutlinedPasswordBoxPadding`                           | `Thickness`       | `FilledPasswordBoxPadding`                 |
| `OutlinedPasswordBoxMinHeight`                         | `Double`          | `FilledPasswordBoxMinHeight`               |
| `OutlinedPasswordBoxHeaderMargin`                      | `Thickness`       | `FilledPasswordBoxHeaderMargin`            |

### Outlined Variant -- Normal State

| Key                                                    | Type              | Value                                      |
|--------------------------------------------------------|-------------------|--------------------------------------------|
| `OutlinedPasswordBoxForeground`                        | `SolidColorBrush` | `FilledPasswordBoxForeground`              |
| `OutlinedPasswordBoxBackground`                        | `SolidColorBrush` | `SurfaceBrush`                             |
| `OutlinedPasswordBoxBorderBrush`                       | `SolidColorBrush` | `FilledPasswordBoxBorderBrush`             |
| `OutlinedPasswordBoxPlaceholderForeground`             | `SolidColorBrush` | `FilledPasswordBoxPlaceholderForeground`   |

### Outlined Variant -- PointerOver State

| Key                                                    | Type              | Value                                      |
|--------------------------------------------------------|-------------------|--------------------------------------------|
| `OutlinedPasswordBoxForegroundPointerOver`             | `SolidColorBrush` | `FilledPasswordBoxForegroundPointerOver`   |
| `OutlinedPasswordBoxBackgroundPointerOver`             | `SolidColorBrush` | `SurfaceBrush`                             |
| `OutlinedPasswordBoxBorderBrushPointerOver`            | `SolidColorBrush` | `FilledPasswordBoxBorderBrushPointerOver`  |

### Outlined Variant -- Focused State

| Key                                                    | Type              | Value                                      |
|--------------------------------------------------------|-------------------|--------------------------------------------|
| `OutlinedPasswordBoxForegroundFocused`                 | `SolidColorBrush` | `FilledPasswordBoxForegroundFocused`       |
| `OutlinedPasswordBoxBackgroundFocused`                 | `SolidColorBrush` | `SurfaceBrush`                             |
| `OutlinedPasswordBoxBorderBrushFocused`                | `SolidColorBrush` | `FilledPasswordBoxBorderBrushFocused`      |

### Outlined Variant -- Disabled State

| Key                                                    | Type              | Value                                      |
|--------------------------------------------------------|-------------------|--------------------------------------------|
| `OutlinedPasswordBoxForegroundDisabled`                | `SolidColorBrush` | `FilledPasswordBoxForegroundDisabled`      |
| `OutlinedPasswordBoxBackgroundDisabled`                | `SolidColorBrush` | `OnSurfaceDisabledBrush`                   |
| `OutlinedPasswordBoxBorderBrushDisabled`               | `SolidColorBrush` | `FilledPasswordBoxBorderBrushDisabled`     |
| `OutlinedPasswordBoxPlaceholderForegroundDisabled`     | `SolidColorBrush` | `FilledPasswordBoxPlaceholderForegroundDisabled` |

### Outlined Variant -- Reveal Button

| Key                                                    | Type              | Value                                      |
|--------------------------------------------------------|-------------------|--------------------------------------------|
| `OutlinedPasswordBoxRevealForeground`                  | `SolidColorBrush` | `FilledPasswordBoxRevealForeground`        |
| `OutlinedPasswordBoxRevealForegroundPointerOver`       | `SolidColorBrush` | `FilledPasswordBoxRevealForegroundPointerOver` |
| `OutlinedPasswordBoxRevealForegroundPressed`           | `SolidColorBrush` | `FilledPasswordBoxRevealForegroundPressed`  |

### Outlined Variant -- Header

| Key                                                    | Type              | Value                                      |
|--------------------------------------------------------|-------------------|--------------------------------------------|
| `OutlinedPasswordBoxHeaderForeground`                  | `SolidColorBrush` | `FilledPasswordBoxHeaderForeground`        |
| `OutlinedPasswordBoxHeaderForegroundDisabled`          | `SolidColorBrush` | `FilledPasswordBoxHeaderForegroundDisabled` |
