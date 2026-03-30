---
uid: Uno.Themes.Simple.Styles.ComboBox
---

# ComboBox Control

## Styles

| Style Key                        | IsDefaultStyle\* |
|----------------------------------|------------------|
| `SimpleSelectFieldStyle`         | True             |
| `SimpleSelectFieldErrorStyle`    |                  |
| `SimpleSelectFieldItemStyle`     |                  |

IsDefaultStyle\*: Styles in this column will be set as the default implicit style for the matching control

## Lightweight Styling

### Structural Resources

| Key                                     | Type              | Value                            |
|-----------------------------------------|-------------------|----------------------------------|
| `ComboBoxCornerRadius`                  | `CornerRadius`    | `SimpleRadius200CornerRadius`    |
| `ComboBoxMinHeight`                     | `Double`          | `SimpleIconLarge`                |
| `ComboBoxBorderThickness`              | `Thickness`       | `SimpleStrokeBorderThickness`    |
| `ComboBoxPadding`                       | `Thickness`       | 16,10,12,10                      |

### Simple Aliases

| Key                                     | Type              | Value                            |
|-----------------------------------------|-------------------|----------------------------------|
| `SimpleComboBoxCornerRadius`            | `CornerRadius`    | `ComboBoxCornerRadius`           |
| `SimpleComboBoxMinHeight`              | `Double`          | `ComboBoxMinHeight`              |
| `SimpleComboBoxBorderThickness`        | `Thickness`       | `ComboBoxBorderThickness`        |
| `SimpleComboBoxFocusedBorderThickness` | `Thickness`       | `SimpleStrokeFocusRingThickness` |
| `SimpleComboBoxPadding`               | `Thickness`       | 16,10,12,10                      |

### Simple-Only Layout Resources

| Key                                     | Type              | Value                            |
|-----------------------------------------|-------------------|----------------------------------|
| `SimpleComboBoxDropDownGlyphMargin`    | `Thickness`       | 8,0,12,0                         |
| `SimpleComboBoxPopupPadding`           | `Thickness`       | 0,4                              |
| `SimpleComboBoxPopupMaxHeight`         | `Double`          | 400                              |
| `SimpleComboBoxDropDownGlyphIcon`      | `String`          | &#xE96E;                         |

### ComboBox -- Normal State

| Key                                             | Type              | Value                          |
|-------------------------------------------------|-------------------|--------------------------------|
| `ComboBoxBackground`                            | `SolidColorBrush` | `SurfaceBrush`                 |
| `ComboBoxBorderBrush`                           | `SolidColorBrush` | `OutlineBrush`                 |
| `ComboBoxForeground`                            | `SolidColorBrush` | `OnSurfaceBrush`               |
| `ComboBoxPlaceHolderForeground`                 | `SolidColorBrush` | `OnSurfaceLowBrush`            |
| `ComboBoxDropDownGlyphForeground`               | `SolidColorBrush` | `OnSurfaceBrush`               |

### ComboBox -- PointerOver State

| Key                                             | Type              | Value                          |
|-------------------------------------------------|-------------------|--------------------------------|
| `ComboBoxBackgroundPointerOver`                 | `SolidColorBrush` | `SurfaceBrush`                 |
| `ComboBoxBorderBrushPointerOver`                | `SolidColorBrush` | `OutlineBrush`                 |
| `ComboBoxForegroundPointerOver`                 | `SolidColorBrush` | `OnSurfaceBrush`               |

### ComboBox -- Pressed State

| Key                                             | Type              | Value                          |
|-------------------------------------------------|-------------------|--------------------------------|
| `ComboBoxBackgroundPressed`                     | `SolidColorBrush` | `SurfaceBrush`                 |
| `ComboBoxBorderBrushPressed`                    | `SolidColorBrush` | `PrimaryMediumBrush`           |
| `ComboBoxForegroundPressed`                     | `SolidColorBrush` | `OnSurfaceBrush`               |

### ComboBox -- Disabled State

| Key                                             | Type              | Value                          |
|-------------------------------------------------|-------------------|--------------------------------|
| `ComboBoxBackgroundDisabled`                    | `SolidColorBrush` | `OnSurfaceDisabledBrush`       |
| `ComboBoxBorderBrushDisabled`                   | `SolidColorBrush` | `OutlineDisabledBrush`         |
| `ComboBoxForegroundDisabled`                    | `SolidColorBrush` | `OnSurfaceDisabledBrush`       |
| `ComboBoxDropDownGlyphForegroundDisabled`       | `SolidColorBrush` | `OnSurfaceDisabledBrush`       |

### ComboBox -- Dropdown

| Key                                             | Type              | Value                          |
|-------------------------------------------------|-------------------|--------------------------------|
| `ComboBoxDropDownBackground`                    | `SolidColorBrush` | `SurfaceBrush`                 |
| `ComboBoxDropDownBorderBrush`                   | `SolidColorBrush` | `OutlineBrush`                 |

### ComboBoxItem -- Normal State

| Key                                             | Type              | Value                          |
|-------------------------------------------------|-------------------|--------------------------------|
| `ComboBoxItemBackground`                        | `SolidColorBrush` | `SystemControlTransparentBrush` |
| `ComboBoxItemForeground`                        | `SolidColorBrush` | `OnSurfaceBrush`               |

### ComboBoxItem -- PointerOver State

| Key                                             | Type              | Value                          |
|-------------------------------------------------|-------------------|--------------------------------|
| `ComboBoxItemBackgroundPointerOver`             | `SolidColorBrush` | `SurfaceVariantBrush`          |
| `ComboBoxItemForegroundPointerOver`             | `SolidColorBrush` | `OnSurfaceBrush`               |

### ComboBoxItem -- Pressed State

| Key                                             | Type              | Value                          |
|-------------------------------------------------|-------------------|--------------------------------|
| `ComboBoxItemBackgroundPressed`                 | `SolidColorBrush` | `TertiaryContainerBrush`       |
| `ComboBoxItemForegroundPressed`                 | `SolidColorBrush` | `OnSurfaceBrush`               |

### ComboBoxItem -- Selected State

| Key                                             | Type              | Value                          |
|-------------------------------------------------|-------------------|--------------------------------|
| `ComboBoxItemBackgroundSelected`                | `SolidColorBrush` | `PrimaryBrush`                 |
| `ComboBoxItemForegroundSelected`                | `SolidColorBrush` | `OnPrimaryBrush`               |

### ComboBoxItem -- SelectedPointerOver State

| Key                                             | Type              | Value                          |
|-------------------------------------------------|-------------------|--------------------------------|
| `ComboBoxItemBackgroundSelectedPointerOver`     | `SolidColorBrush` | `PrimaryVariantDarkBrush`      |
| `ComboBoxItemForegroundSelectedPointerOver`     | `SolidColorBrush` | `OnPrimaryBrush`               |

### ComboBoxItem -- SelectedPressed State

| Key                                             | Type              | Value                          |
|-------------------------------------------------|-------------------|--------------------------------|
| `ComboBoxItemBackgroundSelectedPressed`         | `SolidColorBrush` | `PrimaryVariantDarkBrush`      |
| `ComboBoxItemForegroundSelectedPressed`         | `SolidColorBrush` | `OnPrimaryBrush`               |

### ComboBoxItem -- Disabled State

| Key                                             | Type              | Value                          |
|-------------------------------------------------|-------------------|--------------------------------|
| `ComboBoxItemBackgroundDisabled`                | `SolidColorBrush` | `SystemControlTransparentBrush` |
| `ComboBoxItemForegroundDisabled`                | `SolidColorBrush` | `OnSurfaceDisabledBrush`       |
