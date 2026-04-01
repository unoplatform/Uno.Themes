---
uid: Uno.Themes.Simple.Styles.RadioButton
---

# RadioButton Control

## Styles

| Style Key                  | IsDefaultStyle\* |
|----------------------------|------------------|
| `SimpleRadioButtonStyle`   | True             |

IsDefaultStyle\*: Styles in this column will be set as the default implicit style for the matching control

## Lightweight Styling

### Structural Resources

| Key                                  | Type              | Value                         |
|--------------------------------------|-------------------|-------------------------------|
| `SimpleRadioButtonSize`              | `StaticResource`  | `SimpleSpace400`              |
| `SimpleRadioButtonDotSize`           | `StaticResource`  | `SimpleSpace200`              |
| `SimpleRadioButtonStrokeThickness`   | `StaticResource`  | `SimpleStrokeBorder`          |
| `SimpleRadioButtonContentMargin`     | `Thickness`       | 16,0,0,0                     |
| `SimpleRadioButtonColumnWidth`       | `GridLength`      | 16                            |

### Brush Resources

#### Unchecked Background (Outer Ellipse Fill)

| Key                                          | Type              | Value                      |
|----------------------------------------------|-------------------|----------------------------|
| `RadioButtonOuterEllipseFill`                | `SolidColorBrush` | `SurfaceBrush`             |
| `RadioButtonOuterEllipseFillPointerOver`     | `SolidColorBrush` | `SurfaceBrush`             |
| `RadioButtonOuterEllipseFillPressed`         | `SolidColorBrush` | `PrimaryContainerBrush`    |
| `RadioButtonOuterEllipseFillDisabled`        | `SolidColorBrush` | `OnSurfaceDisabledBrush`   |

#### Unchecked Border (Outer Ellipse Stroke)

| Key                                          | Type              | Value                      |
|----------------------------------------------|-------------------|----------------------------|
| `RadioButtonOuterEllipseStroke`              | `SolidColorBrush` | `OutlineBrush`             |
| `RadioButtonOuterEllipseStrokePointerOver`   | `SolidColorBrush` | `OutlineBrush`             |
| `RadioButtonOuterEllipseStrokePressed`       | `SolidColorBrush` | `OutlineBrush`             |
| `RadioButtonOuterEllipseStrokeDisabled`      | `SolidColorBrush` | `OutlineDisabledBrush`     |

#### Checked Background (Outer Ellipse Checked Fill)

| Key                                                | Type              | Value                      |
|----------------------------------------------------|-------------------|----------------------------|
| `RadioButtonOuterEllipseCheckedFill`               | `SolidColorBrush` | `PrimaryBrush`             |
| `RadioButtonOuterEllipseCheckedFillPointerOver`    | `SolidColorBrush` | `PrimaryBrush`             |
| `RadioButtonOuterEllipseCheckedFillPressed`        | `SolidColorBrush` | `PrimaryVariantDarkBrush`  |
| `RadioButtonOuterEllipseCheckedFillDisabled`       | `SolidColorBrush` | `OnSurfaceDisabledBrush`   |

#### Checked Border (Outer Ellipse Checked Stroke)

| Key                                                | Type              | Value                      |
|----------------------------------------------------|-------------------|----------------------------|
| `RadioButtonOuterEllipseCheckedStroke`             | `SolidColorBrush` | `PrimaryBrush`             |
| `RadioButtonOuterEllipseCheckedStrokePointerOver`  | `SolidColorBrush` | `PrimaryBrush`             |
| `RadioButtonOuterEllipseCheckedStrokePressed`      | `SolidColorBrush` | `PrimaryVariantDarkBrush`  |
| `RadioButtonOuterEllipseCheckedStrokeDisabled`     | `SolidColorBrush` | `OutlineDisabledBrush`     |

#### Foreground (Label)

| Key                                    | Type              | Value                      |
|----------------------------------------|-------------------|----------------------------|
| `RadioButtonForeground`                | `SolidColorBrush` | `OnSurfaceBrush`           |
| `RadioButtonForegroundPointerOver`     | `SolidColorBrush` | `OnSurfaceBrush`           |
| `RadioButtonForegroundPressed`         | `SolidColorBrush` | `OnSurfaceBrush`           |
| `RadioButtonForegroundDisabled`        | `SolidColorBrush` | `OnSurfaceDisabledBrush`   |
