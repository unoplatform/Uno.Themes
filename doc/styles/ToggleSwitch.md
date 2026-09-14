---
uid: Uno.Themes.Styles.ToggleSwitch
---

# ToggleSwitch Control

> [!NOTE]
> Under the [Fluent theme](../fluent-getting-started.md), the key names below are bridged onto WinUI's per-control resources (`ToggleSwitchOuterBorderFill` → `ToggleSwitchFillOn`, `ToggleSwitchKnobOnFill*` → `ToggleSwitchKnobFillOn*`, …) via `Colors.OverrideDictionary`. Knob shadow/bounds, icon-presenter, and `Focused` knob keys have no Fluent equivalent. See [Lightweight Styling — Fluent theme](../lightweight-styling.md#fluent-theme).

## Styles

| Style Key           | IsDefaultStyle\* |
|---------------------|------------------|
| `ToggleSwitchStyle` | True             |

IsDefaultStyle\*: Styles in this column will be set as the default implicit style for the matching control

## Lightweight Styling

| Key                                              | Type              | Value                           |
|--------------------------------------------------|-------------------|---------------------------------|
| `ToggleSwitchOffOuterBorderFill`                 | `SolidColorBrush` | `SurfaceVariantBrush`           |
| `ToggleSwitchOffOuterBorderStroke`               | `SolidColorBrush` | `OutlineBrush`                  |
| `ToggleSwitchOuterBorderStroke`                  | `SolidColorBrush` | `OutlineBrush`                  |
| `ToggleSwitchOuterBorderFill`                    | `SolidColorBrush` | `SurfaceVariantBrush`           |
| `ToggleSwitchKnobOffFill`                        | `SolidColorBrush` | `OnSurfaceVariantBrush`         |
| `ToggleSwitchKnobOffFillPointerOver`             | `SolidColorBrush` | `OnSurfaceVariantBrush`         |
| `ToggleSwitchKnobOffFillFocused`                 | `SolidColorBrush` | `OnSurfaceVariantBrush`         |
| `ToggleSwitchKnobOffFillPressed`                 | `SolidColorBrush` | `OnSurfaceVariantBrush`         |
| `ToggleSwitchKnobOffFillDisabled`                | `SolidColorBrush` | `OnSurfaceLowBrush`             |
| `ToggleSwitchKnobOnFill`                         | `SolidColorBrush` | `OnPrimaryBrush`                |
| `ToggleSwitchKnobOnFillPointerOver`              | `SolidColorBrush` | `PrimaryContainerBrush`         |
| `ToggleSwitchKnobOnFillFocused`                  | `SolidColorBrush` | `PrimaryContainerBrush`         |
| `ToggleSwitchKnobOnFillPressed`                  | `SolidColorBrush` | `PrimaryContainerBrush`         |
| `ToggleSwitchKnobOnFillDisabled`                 | `SolidColorBrush` | `SurfaceBrush`                  |
| `ToggleSwitchKnobOnShadowFill`                   | `SolidColorBrush` | `SystemControlTransparentBrush` |
| `ToggleSwitchKnobOnShadowFillPointerOver`        | `SolidColorBrush` | `PrimaryHoverBrush`             |
| `ToggleSwitchKnobOnShadowFillFocused`            | `SolidColorBrush` | `PrimaryFocusedBrush`           |
| `ToggleSwitchKnobOnShadowFillPressed`            | `SolidColorBrush` | `PrimaryPressedBrush`           |
| `ToggleSwitchKnobOffShadowFill`                  | `SolidColorBrush` | `SystemControlTransparentBrush` |
| `ToggleSwitchKnobOffShadowFillPointerOver`       | `SolidColorBrush` | `OnSurfaceVariantHoverBrush`    |
| `ToggleSwitchKnobOffShadowFillFocused`           | `SolidColorBrush` | `OnSurfaceVariantFocusedBrush`  |
| `ToggleSwitchKnobOffShadowFillPressed`           | `SolidColorBrush` | `OnSurfaceVariantPressedBrush`  |
| `ToggleSwitchOnSwitchKnobBoundsFill`             | `SolidColorBrush` | `PrimaryBrush`                  |
| `ToggleSwitchKnobBoundsFill`                     | `SolidColorBrush` | `PrimaryBrush`                  |
| `ToggleSwitchKnobBoundsFillPointerOver`          | `SolidColorBrush` | `PrimaryBrush`                  |
| `ToggleSwitchKnobBoundsFillFocused`              | `SolidColorBrush` | `PrimaryBrush`                  |
| `ToggleSwitchKnobBoundsFillPressed`              | `SolidColorBrush` | `PrimaryBrush`                  |
| `ToggleSwitchKnobBoundsFillDisabled`             | `SolidColorBrush` | `OnSurfaceDisabledBrush`        |
| `ToggleSwitchOffIconPresenterForeground`         | `SolidColorBrush` | `SurfaceVariantBrush`           |
| `ToggleSwitchOffIconPresenterForegroundDisabled` | `SolidColorBrush` | `SurfaceVariantBrush`           |
| `ToggleSwitchOnIconPresenterForeground`          | `SolidColorBrush` | `OnPrimaryContainerBrush`       |
| `ToggleSwitchOnIconPresenterForegroundDisabled`  | `SolidColorBrush` | `OnSurfaceLowBrush`             |
| `ToggleSwitchThumb`                              | `SolidColorBrush` | `SystemControlTransparentBrush` |
| `SwitchKnobWidth`                                | `Double`          | 52                              |
| `SwitchKnobIncludingOffShadowWidth`              | `Double`          | 56                              |
| `SwitchKnobHeight`                               | `Double`          | 32                              |
| `SwitchKnobRadius`                               | `Double`          | 16                              |
| `SwitchKnobStrokeThickness`                      | `Double`          | 2                               |
| `SwitchKnobShadowSize`                           | `Double`          | 40                              |
| `SmallThumbSize`                                 | `Double`          | 16                              |
| `MediumThumbSize`                                | `Double`          | 24                              |
| `LargeThumbSize`                                 | `Double`          | 28                              |
| `KnobIconSize`                                   | `Double`          | 16                              |
| `LargeThumbCornerRadius`                         | `CornerRadius`    | 14                              |
| `KnobIconPadding`                                | `Thickness`       | 2                               |
| `KnobOnMargin`                                   | `Thickness`       | 2,0,0,0                         |
| `SwitchKnobOnMargin`                             | `Thickness`       | 26,0,0,0                        |
| `SwitchKnobOffMargin`                            | `Thickness`       | 0,0,22,0                        |
| `SwitchKnobOnShadowMargin`                       | `Thickness`       | 20,0,0,0                        |
| `SwitchKnobOffShadowMargin`                      | `Thickness`       | 0,0,20,0                        |
