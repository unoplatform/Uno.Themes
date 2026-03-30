---
uid: Uno.Themes.Simple.Styles.ToggleSwitch
---

# ToggleSwitch Control

## Styles

| Style Key                  | IsDefaultStyle\* |
|----------------------------|------------------|
| `SimpleToggleSwitchStyle`  | True             |

IsDefaultStyle\*: Styles in this column will be set as the default implicit style for the matching control

## Lightweight Styling

### Structural Resources

| Key                                    | Type              | Value                    |
|----------------------------------------|-------------------|--------------------------|
| `SimpleToggleSwitchKnobWidth`          | `StaticResource`  | `SimpleSpace300`         |
| `SimpleToggleSwitchKnobHeight`         | `StaticResource`  | `SimpleSpace300`         |
| `SimpleToggleSwitchTrackRadius`        | `Double`          | 12                       |
| `SimpleToggleSwitchThumbOffMargin`     | `Thickness`       | 4,0,0,0                  |
| `SimpleToggleSwitchThumbOnMargin`      | `Thickness`       | 20,0,0,0                 |
| `SimpleToggleSwitchHeaderMargin`       | `Thickness`       | 0,0,0,4                  |

### Brush Resources

#### Off-Track

| Key                                          | Type              | Value                      |
|----------------------------------------------|-------------------|----------------------------|
| `ToggleSwitchOuterBorderFill`                | `SolidColorBrush` | `SurfaceVariantBrush`      |
| `ToggleSwitchOuterBorderStroke`              | `SolidColorBrush` | `OutlineBrush`             |
| `ToggleSwitchOuterBorderFillPressed`         | `SolidColorBrush` | `TertiaryContainerBrush`   |
| `ToggleSwitchOuterBorderFillDisabled`        | `SolidColorBrush` | `OnSurfaceDisabledBrush`   |
| `ToggleSwitchOuterBorderStrokeDisabled`      | `SolidColorBrush` | `OutlineDisabledBrush`     |

#### On-Track

| Key                                          | Type              | Value                      |
|----------------------------------------------|-------------------|----------------------------|
| `ToggleSwitchKnobBoundsFill`                 | `SolidColorBrush` | `PrimaryBrush`             |
| `ToggleSwitchKnobBoundsFillPressed`          | `SolidColorBrush` | `PrimaryVariantDarkBrush`  |
| `ToggleSwitchKnobBoundsFillDisabled`         | `SolidColorBrush` | `OnSurfaceDisabledBrush`   |

#### Thumb

| Key                                          | Type              | Value                      |
|----------------------------------------------|-------------------|----------------------------|
| `ToggleSwitchKnobOffFill`                    | `SolidColorBrush` | `OnSurfaceMediumBrush`     |
| `ToggleSwitchKnobOnFill`                     | `SolidColorBrush` | `OnPrimaryBrush`           |
| `ToggleSwitchKnobFillDisabled`               | `SolidColorBrush` | `OnSurfaceDisabledBrush`   |

#### Label

| Key                                          | Type              | Value                      |
|----------------------------------------------|-------------------|----------------------------|
| `ToggleSwitchContentForeground`              | `SolidColorBrush` | `OnSurfaceBrush`           |
| `ToggleSwitchContentForegroundDisabled`      | `SolidColorBrush` | `OnSurfaceDisabledBrush`   |
