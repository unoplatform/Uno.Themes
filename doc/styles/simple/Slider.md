---
uid: Uno.Themes.Simple.Styles.Slider
---

# Slider Control

## Styles

| Style Key                | IsDefaultStyle\* |
|--------------------------|------------------|
| `SimpleSliderStyle`      | True             |
| `SimpleSliderThumbStyle` |                  |

IsDefaultStyle\*: Styles in this column will be set as the default implicit style for the matching control

## Lightweight Styling

### Structural Resources

| Key                                        | Type              | Value                            |
|--------------------------------------------|-------------------|----------------------------------|
| `SimpleSliderThumbCornerRadius`            | `StaticResource`  | `SliderThumbCornerRadius`        |
| `SimpleSliderTrackThickness`               | `StaticResource`  | `SliderTrackThickness`           |
| `SimpleSliderFillThickness`                | `StaticResource`  | `SliderFillThickness`            |
| `SliderThumbWidth`                         | `StaticResource`  | `SimpleSpace400`                 |
| `SliderThumbHeight`                        | `StaticResource`  | `SimpleSpace400`                 |
| `SliderThumbCornerRadius`                  | `StaticResource`  | `SimpleRadius200CornerRadius`    |
| `SliderTrackThickness`                     | `StaticResource`  | `SimpleSpace100`                 |
| `SliderFillThickness`                      | `StaticResource`  | `SimpleSpace200`                 |
| `SliderPreContentMargin`                   | `Double`          | 14                               |
| `SliderPostContentMargin`                  | `Double`          | 14                               |
| `SliderHorizontalHeight`                   | `StaticResource`  | `SimpleIconMedium`               |
| `SliderVerticalWidth`                      | `StaticResource`  | `SimpleIconMedium`               |
| `SliderHorizontalFillCornerRadius`         | `CornerRadius`    | 4,0,0,4                         |
| `SliderVerticalFillCornerRadius`           | `CornerRadius`    | 0,0,4,4                         |
| `SliderCornerRadius`                       | `CornerRadius`    | 2                                |
| `SliderFocusVisualMargin`                  | `Thickness`       | -7,0                             |
| `SliderHorizontalFocusVisualMargin`        | `Thickness`       | -14,-6,-14,-6                    |
| `SliderVerticalFocusVisualMargin`          | `Thickness`       | -6,-14,-6,-14                    |
| `SliderHeaderMargin`                       | `Thickness`       | 0,0,0,4                         |
| `SimpleSliderPreContentMargin`             | `Double`          | 14                               |
| `SimpleSliderPostContentMargin`            | `Double`          | 14                               |
| `SimpleSliderHorizontalFillCornerRadius`   | `CornerRadius`    | 4,0,0,4                         |
| `SimpleSliderVerticalFillCornerRadius`     | `CornerRadius`    | 0,0,4,4                         |
| `SimpleSliderCornerRadius`                 | `CornerRadius`    | 2                                |
| `SimpleSliderFocusVisualMargin`            | `Thickness`       | -7,0                             |
| `SimpleSliderHorizontalFocusVisualMargin`  | `Thickness`       | -14,-6,-14,-6                    |
| `SimpleSliderVerticalFocusVisualMargin`    | `Thickness`       | -6,-14,-6,-14                    |
| `SimpleSliderHeaderMargin`                 | `Thickness`       | 0,0,0,4                         |

### Brush Resources

#### Normal State

| Key                            | Type              | Value                      |
|--------------------------------|-------------------|----------------------------|
| `SliderThumbBackground`        | `SolidColorBrush` | `PrimaryBrush`             |
| `SliderTrackValueFill`         | `SolidColorBrush` | `PrimaryBrush`             |
| `SliderTrackFill`              | `SolidColorBrush` | `OutlineBrush`             |

#### Pressed State

| Key                            | Type              | Value                      |
|--------------------------------|-------------------|----------------------------|
| `SliderThumbBackgroundPressed` | `SolidColorBrush` | `PrimaryVariantDarkBrush`  |
| `SliderTrackValueFillPressed`  | `SolidColorBrush` | `PrimaryVariantDarkBrush`  |

#### Disabled State

| Key                             | Type              | Value                      |
|---------------------------------|-------------------|----------------------------|
| `SliderThumbBackgroundDisabled` | `SolidColorBrush` | `OnSurfaceDisabledBrush`   |
| `SliderTrackValueFillDisabled`  | `SolidColorBrush` | `OnSurfaceDisabledBrush`   |
| `SliderTrackFillDisabled`       | `SolidColorBrush` | `OnSurfaceDisabledBrush`   |
