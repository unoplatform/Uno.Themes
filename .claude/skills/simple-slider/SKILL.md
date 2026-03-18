---
name: simple-slider
description: Uses Simple Theme Slider style in Uno Platform applications. Use when styling sliders for numeric range selection. Covers the Slider style, thumb and track customization, and lightweight resource keys.
metadata:
  author: unoplatform
  version: "1.0"
  framework: uno-platform
  category: simple-theme-controls
---

# Simple Theme — Slider Style

## Overview

The Simple theme provides a single Slider style (plus a dedicated Thumb style used internally). When the Simple theme is active, the **implicit default style** applied to every `<Slider>` is `SimpleSliderStyle`.

## Available Styles

| Style Key | Alias | IsDefaultStyle | TargetType |
|-----------|-------|----------------|------------|
| `SimpleSliderStyle` | `SliderStyle` | **Yes** | `Slider` |
| `SimpleSliderThumbStyle` | — | — | `Thumb` (internal) |

## Usage Examples

```xml
<!-- Implicit — no Style needed -->
<Slider Minimum="0" Maximum="100" Value="50" />

<!-- Explicit with header -->
<Slider Header="Volume"
        Minimum="0"
        Maximum="100"
        Value="75"
        Style="{StaticResource SimpleSliderStyle}" />
```

## Lightweight Styling

### Color Resources

| Key | Type | Default Token |
|-----|------|---------------|
| `SimpleSliderThumbBackground` | `SolidColorBrush` | `SimpleBackgroundBrandDefaultBrush` |
| `SimpleSliderTrackValueFill` | `SolidColorBrush` | `SimpleBackgroundBrandDefaultBrush` |
| `SimpleSliderTrackFill` | `SolidColorBrush` | `SimpleBackgroundBrandSecondaryBrush` |
| `SimpleSliderThumbBackgroundPressed` | `SolidColorBrush` | `SimpleBackgroundBrandPressedBrush` |
| `SimpleSliderTrackValueFillPressed` | `SolidColorBrush` | `SimpleBackgroundBrandPressedBrush` |
| `SimpleSliderThumbBackgroundDisabled` | `SolidColorBrush` | `SimpleBackgroundDisabledDefaultBrush` |
| `SimpleSliderTrackValueFillDisabled` | `SolidColorBrush` | `SimpleBackgroundDisabledDefaultBrush` |
| `SimpleSliderTrackFillDisabled` | `SolidColorBrush` | `SimpleBackgroundDisabledDefaultBrush` |

### Layout Resources

| Key | Type | Default Token |
|-----|------|---------------|
| `SimpleSliderThumbWidth` | `Double` | `SimpleSpace400` (16px) |
| `SimpleSliderThumbHeight` | `Double` | `SimpleSpace400` (16px) |
| `SimpleSliderThumbCornerRadius` | `CornerRadius` | `SimpleRadius200CornerRadius` |
| `SimpleSliderTrackThickness` | `Double` | `SimpleSpace100` (4px) |
| `SimpleSliderFillThickness` | `Double` | `SimpleSpace200` (8px) |
| `SimpleSliderPreContentMargin` | `Double` | `14` |
| `SimpleSliderPostContentMargin` | `Double` | `14` |
| `SimpleSliderHorizontalHeight` | `Double` | `SimpleIconMedium` (32px) |
| `SimpleSliderVerticalWidth` | `Double` | `SimpleIconMedium` (32px) |
| `SimpleSliderHorizontalFillCornerRadius` | `CornerRadius` | `4,0,0,4` |
| `SimpleSliderVerticalFillCornerRadius` | `CornerRadius` | `0,0,4,4` |
| `SimpleSliderCornerRadius` | `CornerRadius` | `2` |
| `SimpleSliderHeaderMargin` | `Thickness` | `0,0,0,4` |

## Related Skills

- [simple-toggleswitch](../simple-toggleswitch/SKILL.md)
