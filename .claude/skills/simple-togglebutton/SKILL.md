---
name: simple-togglebutton
description: Uses Simple Theme ToggleButton styles in Uno Platform applications. Use when styling toggle buttons for binary on/off actions with Default, Small, or Medium size variants. Covers all ToggleButton style keys, visual states, and lightweight resource keys.
metadata:
  author: unoplatform
  version: "1.0"
  framework: uno-platform
  category: simple-theme-controls
---

# Simple Theme — ToggleButton Styles

## Overview

The Simple theme provides three ToggleButton size variants. When the Simple theme is active, the **implicit default style** applied to every `<ToggleButton>` is `SimpleDefaultToggleButtonStyle`.

## Available Styles

| Style Key | Alias | IsDefaultStyle | Use Case |
|-----------|-------|----------------|----------|
| `SimpleDefaultToggleButtonStyle` | `ToggleButtonStyle` | **Yes** | Standard toggle button |
| `SimpleSmallToggleButtonStyle` | `SmallToggleButtonStyle` | | Compact toggle in dense layouts |
| `SimpleMediumToggleButtonStyle` | `MediumToggleButtonStyle` | | Medium toggle for toolbars |

## Style Hierarchy (BasedOn)

```
SimpleDefaultToggleButtonStyle
├── SimpleSmallToggleButtonStyle
└── SimpleMediumToggleButtonStyle
```

## Usage Examples

```xml
<!-- Default toggle -->
<ToggleButton Content="Bold" />

<!-- Small toggle in a toolbar -->
<StackPanel Orientation="Horizontal" Spacing="4">
    <ToggleButton Content="B"
                  Style="{StaticResource SimpleSmallToggleButtonStyle}" />
    <ToggleButton Content="I"
                  Style="{StaticResource SimpleSmallToggleButtonStyle}" />
    <ToggleButton Content="U"
                  Style="{StaticResource SimpleSmallToggleButtonStyle}" />
</StackPanel>
```

## When to Use

Use ToggleButton instead of CheckBox when:
- The toggle represents a **mode** or **view option** (e.g., Bold, List view)
- The control should look like a button, not a checkbox

Use ToggleSwitch instead when:
- The toggle represents an **on/off setting** that takes immediate effect

## Lightweight Styling

### Unchecked State Resources

| Key | Type | Default Token |
|-----|------|---------------|
| `SimpleToggleButtonForeground` | `SolidColorBrush` | `SimpleTextDefaultDefaultBrush` |
| `SimpleToggleButtonForegroundPointerOver` | `SolidColorBrush` | `SimpleTextDefaultDefaultBrush` |
| `SimpleToggleButtonForegroundPressed` | `SolidColorBrush` | `SimpleTextDefaultDefaultBrush` |
| `SimpleToggleButtonForegroundDisabled` | `SolidColorBrush` | `SimpleTextDisabledDefaultBrush` |
| `SimpleToggleButtonBackground` | `SolidColorBrush` | `SimpleBackgroundDefaultSecondaryBrush` |
| `SimpleToggleButtonBackgroundPointerOver` | `SolidColorBrush` | `SimpleBackgroundDefaultSecondaryHoverBrush` |
| `SimpleToggleButtonBackgroundPressed` | `SolidColorBrush` | `SimpleBackgroundDefaultSecondaryPressedBrush` |
| `SimpleToggleButtonBackgroundDisabled` | `SolidColorBrush` | `SimpleBackgroundDisabledDefaultBrush` |
| `SimpleToggleButtonBorderBrush` | `SolidColorBrush` | `SimpleBorderDefaultDefaultBrush` |
| `SimpleToggleButtonBorderBrushPointerOver` | `SolidColorBrush` | `SimpleBorderDefaultDefaultBrush` |
| `SimpleToggleButtonBorderBrushPressed` | `SolidColorBrush` | `SimpleBorderDefaultDefaultBrush` |
| `SimpleToggleButtonBorderBrushDisabled` | `SolidColorBrush` | `SimpleBorderDisabledDefaultBrush` |

### Checked State Resources

| Key | Type | Default Token |
|-----|------|---------------|
| `SimpleToggleButtonForegroundChecked` | `SolidColorBrush` | `SimpleTextBrandOnBrandBrush` |
| `SimpleToggleButtonForegroundCheckedPointerOver` | `SolidColorBrush` | `SimpleTextBrandOnBrandBrush` |
| `SimpleToggleButtonForegroundCheckedPressed` | `SolidColorBrush` | `SimpleTextBrandOnBrandBrush` |
| `SimpleToggleButtonForegroundCheckedDisabled` | `SolidColorBrush` | `SimpleTextDisabledDefaultBrush` |
| `SimpleToggleButtonBackgroundChecked` | `SolidColorBrush` | `SimpleBackgroundBrandDefaultBrush` |
| `SimpleToggleButtonBackgroundCheckedPointerOver` | `SolidColorBrush` | `SimpleBackgroundBrandHoverBrush` |
| `SimpleToggleButtonBackgroundCheckedPressed` | `SolidColorBrush` | `SimpleBackgroundBrandPressedBrush` |
| `SimpleToggleButtonBackgroundCheckedDisabled` | `SolidColorBrush` | `SimpleBackgroundDisabledDefaultBrush` |
| `SimpleToggleButtonBorderBrushChecked` | `SolidColorBrush` | `SimpleBorderBrandDefaultBrush` |
| `SimpleToggleButtonBorderBrushCheckedPointerOver` | `SolidColorBrush` | `SimpleBorderBrandDefaultBrush` |
| `SimpleToggleButtonBorderBrushCheckedPressed` | `SolidColorBrush` | `SimpleBorderBrandDefaultBrush` |
| `SimpleToggleButtonBorderBrushCheckedDisabled` | `SolidColorBrush` | `SimpleBorderDisabledDefaultBrush` |

### Indeterminate State Resources

| Key | Type | Default Token |
|-----|------|---------------|
| `SimpleToggleButtonForegroundIndeterminate` | `SolidColorBrush` | `SimpleTextDefaultDefaultBrush` |
| `SimpleToggleButtonForegroundIndeterminatePointerOver` | `SolidColorBrush` | `SimpleTextDefaultDefaultBrush` |
| `SimpleToggleButtonForegroundIndeterminatePressed` | `SolidColorBrush` | `SimpleTextDefaultDefaultBrush` |
| `SimpleToggleButtonForegroundIndeterminateDisabled` | `SolidColorBrush` | `SimpleTextDisabledDefaultBrush` |
| `SimpleToggleButtonBackgroundIndeterminate` | `SolidColorBrush` | `SimpleBackgroundDefaultSecondaryBrush` |
| `SimpleToggleButtonBackgroundIndeterminatePointerOver` | `SolidColorBrush` | `SimpleBackgroundDefaultSecondaryHoverBrush` |
| `SimpleToggleButtonBackgroundIndeterminatePressed` | `SolidColorBrush` | `SimpleBackgroundDefaultSecondaryPressedBrush` |
| `SimpleToggleButtonBackgroundIndeterminateDisabled` | `SolidColorBrush` | `SimpleBackgroundDisabledDefaultBrush` |
| `SimpleToggleButtonBorderBrushIndeterminate` | `SolidColorBrush` | `SimpleBorderDefaultDefaultBrush` |
| `SimpleToggleButtonBorderBrushIndeterminatePointerOver` | `SolidColorBrush` | `SimpleBorderDefaultDefaultBrush` |
| `SimpleToggleButtonBorderBrushIndeterminatePressed` | `SolidColorBrush` | `SimpleBorderDefaultDefaultBrush` |
| `SimpleToggleButtonBorderBrushIndeterminateDisabled` | `SolidColorBrush` | `SimpleBorderDisabledDefaultBrush` |

### Layout Resources

| Key | Type | Default Token |
|-----|------|---------------|
| `SimpleToggleButtonCornerRadius` | `CornerRadius` | `SimpleRadius200CornerRadius` (8px) |
| `SimpleToggleButtonBorderThickness` | `Thickness` | `SimpleStrokeBorderThickness` (1px) |
| `SimpleToggleButtonMediumPadding` | `Thickness` | `SimpleSpace300Thickness` (12px) |
| `SimpleToggleButtonSmallPadding` | `Thickness` | `SimpleSpace200Thickness` (8px) |
| `SimpleToggleButtonMediumMinHeight` | `Double` | `SimpleIconLarge` (40px) |
| `SimpleToggleButtonSmallMinHeight` | `Double` | `SimpleIconMedium` (32px) |
| `SimpleToggleButtonMediumFontSize` | `Double` | `SimpleTypographyScale03` (16px) |
| `SimpleToggleButtonSmallFontSize` | `Double` | `SimpleTypographyScale02` (14px) |
| `SimpleToggleButtonIconSpacing` | `Double` | `SimpleSpace200` (8px) |
| `SimpleToggleButtonFontFamily` | `FontFamily` | `"Inter"` |

## Related Skills

- [simple-button](../simple-button/SKILL.md)
- [simple-checkbox](../simple-checkbox/SKILL.md)
