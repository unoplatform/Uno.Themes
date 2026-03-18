---
name: simple-radiobutton
description: Uses Simple Theme RadioButton style in Uno Platform applications. Use when styling radio buttons for mutually exclusive single-choice selection. Covers the single RadioButton style, all visual states, and lightweight resource keys.
metadata:
  author: unoplatform
  version: "1.0"
  framework: uno-platform
  category: simple-theme-controls
---

# Simple Theme — RadioButton Style

## Overview

The Simple theme provides a single RadioButton style. When the Simple theme is active, the **implicit default style** applied to every `<RadioButton>` is `SimpleRadioButtonStyle`.

## Available Styles

| Style Key | Alias | IsDefaultStyle |
|-----------|-------|----------------|
| `SimpleRadioButtonStyle` | `RadioButtonStyle` | **Yes** |

## Usage Examples

```xml
<!-- Implicit — no Style needed -->
<StackPanel>
    <RadioButton Content="Option A" GroupName="Options" />
    <RadioButton Content="Option B" GroupName="Options" />
    <RadioButton Content="Option C" GroupName="Options" />
</StackPanel>
```

## Lightweight Styling

### Resource Key Pattern

`SimpleRadioButton{CheckState}{Property}{InteractionState}`

Check states: `Unchecked`, `Checked`
Properties: `Background`, `BorderBrush`, `DotFill`
Interaction states: *(none)* = Normal, `PointerOver`, `Pressed`, `Disabled`

### Unchecked State Resources

| Key | Type | Default Token |
|-----|------|---------------|
| `SimpleRadioButtonUncheckedBackground` | `SolidColorBrush` | `SimpleBackgroundDefaultDefaultBrush` |
| `SimpleRadioButtonUncheckedBackgroundPointerOver` | `SolidColorBrush` | `SimpleBackgroundDefaultDefaultBrush` |
| `SimpleRadioButtonUncheckedBackgroundPressed` | `SolidColorBrush` | `SimpleBackgroundDefaultDefaultPressedBrush` |
| `SimpleRadioButtonUncheckedBackgroundDisabled` | `SolidColorBrush` | `SimpleBackgroundDisabledDefaultBrush` |
| `SimpleRadioButtonUncheckedBorderBrush` | `SolidColorBrush` | `SimpleBorderDefaultDefaultBrush` |
| `SimpleRadioButtonUncheckedBorderBrushPointerOver` | `SolidColorBrush` | `SimpleBorderDefaultDefaultBrush` |
| `SimpleRadioButtonUncheckedBorderBrushPressed` | `SolidColorBrush` | `SimpleBorderDefaultDefaultBrush` |
| `SimpleRadioButtonUncheckedBorderBrushDisabled` | `SolidColorBrush` | `SimpleBorderDisabledDefaultBrush` |

### Checked State Resources

| Key | Type | Default Token |
|-----|------|---------------|
| `SimpleRadioButtonCheckedBackground` | `SolidColorBrush` | `SimpleBackgroundBrandDefaultBrush` |
| `SimpleRadioButtonCheckedBackgroundPointerOver` | `SolidColorBrush` | `SimpleBackgroundBrandDefaultBrush` |
| `SimpleRadioButtonCheckedBackgroundPressed` | `SolidColorBrush` | `SimpleBackgroundBrandPressedBrush` |
| `SimpleRadioButtonCheckedBackgroundDisabled` | `SolidColorBrush` | `SimpleBackgroundDisabledDefaultBrush` |
| `SimpleRadioButtonCheckedBorderBrush` | `SolidColorBrush` | `SimpleBackgroundBrandDefaultBrush` |
| `SimpleRadioButtonCheckedBorderBrushPointerOver` | `SolidColorBrush` | `SimpleBackgroundBrandDefaultBrush` |
| `SimpleRadioButtonCheckedBorderBrushPressed` | `SolidColorBrush` | `SimpleBackgroundBrandPressedBrush` |
| `SimpleRadioButtonCheckedBorderBrushDisabled` | `SolidColorBrush` | `SimpleBorderDisabledDefaultBrush` |
| `SimpleRadioButtonCheckedDotFill` | `SolidColorBrush` | `SimpleIconBrandOnBrandBrush` |
| `SimpleRadioButtonCheckedDotFillPointerOver` | `SolidColorBrush` | `SimpleIconBrandOnBrandBrush` |
| `SimpleRadioButtonCheckedDotFillPressed` | `SolidColorBrush` | `SimpleIconBrandOnBrandBrush` |
| `SimpleRadioButtonCheckedDotFillDisabled` | `SolidColorBrush` | `SimpleIconDisabledOnDisabledBrush` |

### Foreground (Label) Resources

| Key | Type | Default Token |
|-----|------|---------------|
| `SimpleRadioButtonForeground` | `SolidColorBrush` | `SimpleTextDefaultDefaultBrush` |
| `SimpleRadioButtonForegroundPointerOver` | `SolidColorBrush` | `SimpleTextDefaultDefaultBrush` |
| `SimpleRadioButtonForegroundPressed` | `SolidColorBrush` | `SimpleTextDefaultDefaultBrush` |
| `SimpleRadioButtonForegroundDisabled` | `SolidColorBrush` | `SimpleTextDisabledDefaultBrush` |

### Layout Resources

| Key | Type | Default Token |
|-----|------|---------------|
| `SimpleRadioButtonSize` | `Double` | `SimpleSpace400` |
| `SimpleRadioButtonDotSize` | `Double` | `SimpleSpace200` |
| `SimpleRadioButtonStrokeThickness` | `Double` | `SimpleStrokeBorder` |
| `SimpleRadioButtonContentMargin` | `Thickness` | `16,0,0,0` |
| `SimpleRadioButtonColumnWidth` | `GridLength` | `16` |

## Related Skills

- [simple-checkbox](../simple-checkbox/SKILL.md)
- [simple-toggleswitch](../simple-toggleswitch/SKILL.md)
