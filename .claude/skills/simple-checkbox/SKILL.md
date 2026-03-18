---
name: simple-checkbox
description: Uses Simple Theme CheckBox style in Uno Platform applications. Use when styling checkboxes for boolean or tri-state selection. Covers the single CheckBox style, all visual states, and lightweight resource keys.
metadata:
  author: unoplatform
  version: "1.0"
  framework: uno-platform
  category: simple-theme-controls
---

# Simple Theme — CheckBox Style

## Overview

The Simple theme provides a single CheckBox style. When the Simple theme is active, the **implicit default style** applied to every `<CheckBox>` is `SimpleCheckBoxStyle`.

## Available Styles

| Style Key | Alias | IsDefaultStyle |
|-----------|-------|----------------|
| `SimpleCheckBoxStyle` | `CheckBoxStyle` | **Yes** |

## Usage Examples

```xml
<!-- Implicit — no Style needed -->
<CheckBox Content="Accept terms and conditions" />

<!-- Explicit -->
<CheckBox Content="Accept terms and conditions"
          Style="{StaticResource SimpleCheckBoxStyle}" />

<!-- Tri-state -->
<CheckBox Content="Select all"
          IsThreeState="True" />
```

## Lightweight Styling

### Resource Key Pattern

`SimpleCheckBox{CheckState}{Property}{InteractionState}`

Check states: `Unchecked`, `Checked`, `Indeterminate`
Properties: `Background`, `BorderBrush`, `GlyphFill`
Interaction states: *(none)* = Normal, `PointerOver`, `Pressed`, `Disabled`

### Unchecked State Resources

| Key | Type | Default Token |
|-----|------|---------------|
| `SimpleCheckBoxUncheckedBackground` | `SolidColorBrush` | `SimpleBackgroundDefaultDefaultBrush` |
| `SimpleCheckBoxUncheckedBackgroundPointerOver` | `SolidColorBrush` | `SimpleBackgroundDefaultDefaultBrush` |
| `SimpleCheckBoxUncheckedBackgroundPressed` | `SolidColorBrush` | `SimpleBackgroundDefaultDefaultPressedBrush` |
| `SimpleCheckBoxUncheckedBackgroundDisabled` | `SolidColorBrush` | `SimpleBackgroundDisabledDefaultBrush` |
| `SimpleCheckBoxUncheckedBorderBrush` | `SolidColorBrush` | `SimpleBorderDefaultDefaultBrush` |
| `SimpleCheckBoxUncheckedBorderBrushPointerOver` | `SolidColorBrush` | `SimpleBorderDefaultDefaultBrush` |
| `SimpleCheckBoxUncheckedBorderBrushPressed` | `SolidColorBrush` | `SimpleBorderDefaultDefaultBrush` |
| `SimpleCheckBoxUncheckedBorderBrushDisabled` | `SolidColorBrush` | `SimpleBorderDisabledDefaultBrush` |

### Checked State Resources

| Key | Type | Default Token |
|-----|------|---------------|
| `SimpleCheckBoxCheckedBackground` | `SolidColorBrush` | `SimpleBackgroundBrandDefaultBrush` |
| `SimpleCheckBoxCheckedBackgroundPointerOver` | `SolidColorBrush` | `SimpleBackgroundBrandDefaultBrush` |
| `SimpleCheckBoxCheckedBackgroundPressed` | `SolidColorBrush` | `SimpleBackgroundBrandPressedBrush` |
| `SimpleCheckBoxCheckedBackgroundDisabled` | `SolidColorBrush` | `SimpleBackgroundDisabledDefaultBrush` |
| `SimpleCheckBoxCheckedBorderBrush` | `SolidColorBrush` | `SimpleBackgroundBrandDefaultBrush` |
| `SimpleCheckBoxCheckedBorderBrushPointerOver` | `SolidColorBrush` | `SimpleBackgroundBrandDefaultBrush` |
| `SimpleCheckBoxCheckedBorderBrushPressed` | `SolidColorBrush` | `SimpleBackgroundBrandPressedBrush` |
| `SimpleCheckBoxCheckedBorderBrushDisabled` | `SolidColorBrush` | `SimpleBorderDisabledDefaultBrush` |
| `SimpleCheckBoxCheckedGlyphFill` | `SolidColorBrush` | `SimpleIconBrandOnBrandBrush` |
| `SimpleCheckBoxCheckedGlyphFillPointerOver` | `SolidColorBrush` | `SimpleIconBrandOnBrandBrush` |
| `SimpleCheckBoxCheckedGlyphFillPressed` | `SolidColorBrush` | `SimpleIconBrandOnBrandBrush` |
| `SimpleCheckBoxCheckedGlyphFillDisabled` | `SolidColorBrush` | `SimpleIconDisabledOnDisabledBrush` |

### Indeterminate State Resources

| Key | Type | Default Token |
|-----|------|---------------|
| `SimpleCheckBoxIndeterminateBackground` | `SolidColorBrush` | `SimpleBackgroundBrandDefaultBrush` |
| `SimpleCheckBoxIndeterminateBackgroundPointerOver` | `SolidColorBrush` | `SimpleBackgroundBrandDefaultBrush` |
| `SimpleCheckBoxIndeterminateBackgroundPressed` | `SolidColorBrush` | `SimpleBackgroundBrandPressedBrush` |
| `SimpleCheckBoxIndeterminateBackgroundDisabled` | `SolidColorBrush` | `SimpleBackgroundDisabledDefaultBrush` |
| `SimpleCheckBoxIndeterminateBorderBrush` | `SolidColorBrush` | `SimpleBackgroundBrandDefaultBrush` |
| `SimpleCheckBoxIndeterminateBorderBrushPointerOver` | `SolidColorBrush` | `SimpleBackgroundBrandDefaultBrush` |
| `SimpleCheckBoxIndeterminateBorderBrushPressed` | `SolidColorBrush` | `SimpleBackgroundBrandPressedBrush` |
| `SimpleCheckBoxIndeterminateBorderBrushDisabled` | `SolidColorBrush` | `SimpleBorderDisabledDefaultBrush` |
| `SimpleCheckBoxIndeterminateGlyphFill` | `SolidColorBrush` | `SimpleIconBrandOnBrandBrush` |
| `SimpleCheckBoxIndeterminateGlyphFillPointerOver` | `SolidColorBrush` | `SimpleIconBrandOnBrandBrush` |
| `SimpleCheckBoxIndeterminateGlyphFillPressed` | `SolidColorBrush` | `SimpleIconBrandOnBrandBrush` |
| `SimpleCheckBoxIndeterminateGlyphFillDisabled` | `SolidColorBrush` | `SimpleIconDisabledOnDisabledBrush` |

### Foreground (Label) Resources

| Key | Type | Default Token |
|-----|------|---------------|
| `SimpleCheckBoxForeground` | `SolidColorBrush` | `SimpleTextDefaultDefaultBrush` |
| `SimpleCheckBoxForegroundPointerOver` | `SolidColorBrush` | `SimpleTextDefaultDefaultBrush` |
| `SimpleCheckBoxForegroundPressed` | `SolidColorBrush` | `SimpleTextDefaultDefaultBrush` |
| `SimpleCheckBoxForegroundDisabled` | `SolidColorBrush` | `SimpleTextDisabledDefaultBrush` |

### Layout Resources

| Key | Type | Default Token |
|-----|------|---------------|
| `SimpleCheckBoxSize` | `Double` | `SimpleSpace400` |
| `SimpleCheckBoxCornerRadius` | `CornerRadius` | `SimpleRadius100CornerRadius` |
| `SimpleCheckBoxStrokeThickness` | `Double` | `SimpleStrokeBorder` |
| `SimpleCheckBoxContentMargin` | `Thickness` | `16,0,0,0` |
| `SimpleCheckBoxColumnWidth` | `GridLength` | `20` |
| `SimpleCheckBoxGlyphSize` | `Double` | `10` |

## Related Skills

- [simple-radiobutton](../simple-radiobutton/SKILL.md)
- [simple-toggleswitch](../simple-toggleswitch/SKILL.md)
