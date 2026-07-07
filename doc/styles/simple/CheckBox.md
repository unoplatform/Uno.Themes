---
uid: Uno.Themes.Simple.Styles.CheckBox
---

# CheckBox Control

## Styles

| Style Key             | IsDefaultStyle\* |
|-----------------------|------------------|
| `SimpleCheckBoxStyle` | True             |

IsDefaultStyle\*: Styles in this column will be set as the default implicit style for the matching control

## Lightweight Styling

### Structural Resources

| Key                                    | Type              | Value                              |
|----------------------------------------|-------------------|------------------------------------|
| `SimpleCheckBoxSize`                   | `StaticResource`  | `SimpleSpace400`                   |
| `SimpleCheckBoxCornerRadius`           | `StaticResource`  | `SimpleRadius100CornerRadius`      |
| `SimpleCheckBoxStrokeThickness`        | `StaticResource`  | `SimpleStrokeBorder`               |
| `SimpleCheckBoxContentMargin`          | `Thickness`       | 16,0,0,0                          |
| `SimpleCheckBoxColumnWidth`            | `GridLength`      | 20                                 |
| `SimpleCheckBoxGlyphSize`             | `Double`          | 10                                 |
| `SimpleCheckBoxCheckGlyphPathData`     | `String`          | *(SVG path data)*                  |
| `SimpleCheckBoxHyphenGlyphPathData`    | `String`          | *(SVG path data)*                  |

### Brush Resources

#### Unchecked Background

| Key                                            | Type              | Value                    |
|------------------------------------------------|-------------------|--------------------------|
| `CheckBoxBackgroundUnchecked`                  | `SolidColorBrush` | `SurfaceBrush`           |
| `CheckBoxBackgroundUncheckedPointerOver`       | `SolidColorBrush` | `SurfaceBrush`           |
| `CheckBoxBackgroundUncheckedPressed`           | `SolidColorBrush` | `PrimaryContainerBrush`  |
| `CheckBoxBackgroundUncheckedDisabled`          | `SolidColorBrush` | `OnSurfaceDisabledBrush` |

#### Unchecked BorderBrush

| Key                                            | Type              | Value                    |
|------------------------------------------------|-------------------|--------------------------|
| `CheckBoxBorderBrushUnchecked`                 | `SolidColorBrush` | `OutlineBrush`           |
| `CheckBoxBorderBrushUncheckedPointerOver`      | `SolidColorBrush` | `OutlineBrush`           |
| `CheckBoxBorderBrushUncheckedPressed`          | `SolidColorBrush` | `OutlineBrush`           |
| `CheckBoxBorderBrushUncheckedDisabled`         | `SolidColorBrush` | `OutlineDisabledBrush`   |

#### Checked Background

| Key                                            | Type              | Value                      |
|------------------------------------------------|-------------------|----------------------------|
| `CheckBoxBackgroundChecked`                    | `SolidColorBrush` | `PrimaryBrush`             |
| `CheckBoxBackgroundCheckedPointerOver`         | `SolidColorBrush` | `PrimaryBrush`             |
| `CheckBoxBackgroundCheckedPressed`             | `SolidColorBrush` | `PrimaryVariantDarkBrush`  |
| `CheckBoxBackgroundCheckedDisabled`            | `SolidColorBrush` | `OnSurfaceDisabledBrush`   |

#### Checked BorderBrush

| Key                                            | Type              | Value                      |
|------------------------------------------------|-------------------|----------------------------|
| `CheckBoxBorderBrushChecked`                   | `SolidColorBrush` | `PrimaryBrush`             |
| `CheckBoxBorderBrushCheckedPointerOver`        | `SolidColorBrush` | `PrimaryBrush`             |
| `CheckBoxBorderBrushCheckedPressed`            | `SolidColorBrush` | `PrimaryVariantDarkBrush`  |
| `CheckBoxBorderBrushCheckedDisabled`           | `SolidColorBrush` | `OutlineDisabledBrush`     |

#### Checked Glyph Foreground

| Key                                            | Type              | Value                      |
|------------------------------------------------|-------------------|----------------------------|
| `CheckBoxGlyphForegroundChecked`               | `SolidColorBrush` | `OnPrimaryBrush`           |
| `CheckBoxGlyphForegroundCheckedPointerOver`    | `SolidColorBrush` | `OnPrimaryBrush`           |
| `CheckBoxGlyphForegroundCheckedPressed`        | `SolidColorBrush` | `OnPrimaryBrush`           |
| `CheckBoxGlyphForegroundCheckedDisabled`       | `SolidColorBrush` | `OnSurfaceDisabledBrush`   |

#### Indeterminate Background

| Key                                            | Type              | Value                      |
|------------------------------------------------|-------------------|----------------------------|
| `CheckBoxBackgroundIndeterminate`              | `SolidColorBrush` | `PrimaryBrush`             |
| `CheckBoxBackgroundIndeterminatePointerOver`   | `SolidColorBrush` | `PrimaryBrush`             |
| `CheckBoxBackgroundIndeterminatePressed`       | `SolidColorBrush` | `PrimaryVariantDarkBrush`  |
| `CheckBoxBackgroundIndeterminateDisabled`      | `SolidColorBrush` | `OnSurfaceDisabledBrush`   |

#### Indeterminate BorderBrush

| Key                                            | Type              | Value                      |
|------------------------------------------------|-------------------|----------------------------|
| `CheckBoxBorderBrushIndeterminate`             | `SolidColorBrush` | `PrimaryBrush`             |
| `CheckBoxBorderBrushIndeterminatePointerOver`  | `SolidColorBrush` | `PrimaryBrush`             |
| `CheckBoxBorderBrushIndeterminatePressed`      | `SolidColorBrush` | `PrimaryVariantDarkBrush`  |
| `CheckBoxBorderBrushIndeterminateDisabled`     | `SolidColorBrush` | `OutlineDisabledBrush`     |

#### Indeterminate Glyph Foreground

| Key                                                  | Type              | Value                      |
|------------------------------------------------------|-------------------|----------------------------|
| `CheckBoxGlyphForegroundIndeterminate`               | `SolidColorBrush` | `OnPrimaryBrush`           |
| `CheckBoxGlyphForegroundIndeterminatePointerOver`    | `SolidColorBrush` | `OnPrimaryBrush`           |
| `CheckBoxGlyphForegroundIndeterminatePressed`        | `SolidColorBrush` | `OnPrimaryBrush`           |
| `CheckBoxGlyphForegroundIndeterminateDisabled`       | `SolidColorBrush` | `OnSurfaceDisabledBrush`   |

#### Foreground (Label)

| Key                                            | Type              | Value                      |
|------------------------------------------------|-------------------|----------------------------|
| `CheckBoxForegroundUnchecked`                  | `SolidColorBrush` | `OnSurfaceBrush`           |
| `CheckBoxForegroundUncheckedPointerOver`       | `SolidColorBrush` | `OnSurfaceBrush`           |
| `CheckBoxForegroundUncheckedPressed`           | `SolidColorBrush` | `OnSurfaceBrush`           |
| `CheckBoxForegroundUncheckedDisabled`          | `SolidColorBrush` | `OnSurfaceDisabledBrush`   |
| `CheckBoxForegroundChecked`                    | `SolidColorBrush` | `OnSurfaceBrush`           |
| `CheckBoxForegroundCheckedPointerOver`         | `SolidColorBrush` | `OnSurfaceBrush`           |
| `CheckBoxForegroundCheckedPressed`             | `SolidColorBrush` | `OnSurfaceBrush`           |
| `CheckBoxForegroundCheckedDisabled`            | `SolidColorBrush` | `OnSurfaceDisabledBrush`   |
| `CheckBoxForegroundIndeterminate`              | `SolidColorBrush` | `OnSurfaceBrush`           |
| `CheckBoxForegroundIndeterminatePointerOver`   | `SolidColorBrush` | `OnSurfaceBrush`           |
| `CheckBoxForegroundIndeterminatePressed`       | `SolidColorBrush` | `OnSurfaceBrush`           |
| `CheckBoxForegroundIndeterminateDisabled`      | `SolidColorBrush` | `OnSurfaceDisabledBrush`   |
