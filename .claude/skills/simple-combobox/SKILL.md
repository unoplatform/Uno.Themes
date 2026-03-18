---
name: simple-combobox
description: Uses Simple Theme ComboBox (SelectField) styles in Uno Platform applications. Use when styling dropdown select fields with standard, error, or item styles. Covers all ComboBox/SelectField style keys and lightweight resource keys.
metadata:
  author: unoplatform
  version: "1.0"
  framework: uno-platform
  category: simple-theme-controls
---

# Simple Theme — ComboBox (SelectField) Styles

## Overview

The Simple theme styles ComboBox as a "SelectField". When the Simple theme is active, the **implicit default style** applied to every `<ComboBox>` is `SimpleSelectFieldStyle`.

## Available Styles

| Style Key | Alias(es) | IsDefaultStyle | TargetType | Use Case |
|-----------|-----------|----------------|------------|----------|
| `SimpleSelectFieldStyle` | `SelectFieldStyle`, `ComboBoxStyle` | **Yes** | `ComboBox` | Standard dropdown select field |
| `SimpleSelectFieldErrorStyle` | `SelectFieldErrorStyle` | | `ComboBox` | Validation error state |
| `SimpleSelectFieldItemStyle` | `SelectFieldItemStyle` | | `ComboBoxItem` | Individual items in the dropdown |

## Style Hierarchy (BasedOn)

```
SimpleSelectFieldStyle
└── SimpleSelectFieldErrorStyle
```

## Usage Examples

### Standard Select Field

```xml
<!-- Implicit — no Style needed -->
<ComboBox Header="Country"
          PlaceholderText="Select a country">
    <ComboBoxItem Content="United States" />
    <ComboBoxItem Content="Canada" />
    <ComboBoxItem Content="Mexico" />
</ComboBox>
```

### Error State

```xml
<ComboBox Header="Country"
          PlaceholderText="Select a country"
          Style="{StaticResource SimpleSelectFieldErrorStyle}">
    <ComboBoxItem Content="United States" />
</ComboBox>
```

## When to Use Each Style

| Style | When to Use |
|-------|-------------|
| **SelectField** (default) | Standard dropdown menus, form fields, filters |
| **SelectFieldError** | Apply programmatically when validation fails on a required dropdown |

## Lightweight Styling

### ComboBox Container Resources

| Key | Type | Default Token |
|-----|------|---------------|
| `SimpleComboBoxBackground` | `SolidColorBrush` | `SimpleBackgroundDefaultDefaultBrush` |
| `SimpleComboBoxBackgroundPointerOver` | `SolidColorBrush` | `SimpleBackgroundDefaultDefaultBrush` |
| `SimpleComboBoxBackgroundPressed` | `SolidColorBrush` | `SimpleBackgroundDefaultDefaultBrush` |
| `SimpleComboBoxBackgroundDisabled` | `SolidColorBrush` | `SimpleBackgroundDisabledDefaultBrush` |
| `SimpleComboBoxBorderBrush` | `SolidColorBrush` | `SimpleBorderDefaultDefaultBrush` |
| `SimpleComboBoxBorderBrushPointerOver` | `SolidColorBrush` | `SimpleBorderDefaultDefaultBrush` |
| `SimpleComboBoxBorderBrushPressed` | `SolidColorBrush` | `SimpleBorderBrandSecondaryBrush` |
| `SimpleComboBoxBorderBrushDisabled` | `SolidColorBrush` | `SimpleBorderDisabledDefaultBrush` |
| `SimpleComboBoxBorderBrushFocused` | `SolidColorBrush` | `SimpleBorderBrandSecondaryBrush` |
| `SimpleComboBoxBorderBrushError` | `SolidColorBrush` | `SimpleBorderDangerDefaultBrush` |
| `SimpleComboBoxForeground` | `SolidColorBrush` | `SimpleTextDefaultDefaultBrush` |
| `SimpleComboBoxForegroundPointerOver` | `SolidColorBrush` | `SimpleTextDefaultDefaultBrush` |
| `SimpleComboBoxForegroundPressed` | `SolidColorBrush` | `SimpleTextDefaultDefaultBrush` |
| `SimpleComboBoxForegroundDisabled` | `SolidColorBrush` | `SimpleTextDisabledDefaultBrush` |
| `SimpleComboBoxPlaceholderForeground` | `SolidColorBrush` | `SimpleTextDefaultTertiaryBrush` |
| `SimpleComboBoxDropDownGlyphForeground` | `SolidColorBrush` | `SimpleIconDefaultDefaultBrush` |
| `SimpleComboBoxDropDownGlyphForegroundDisabled` | `SolidColorBrush` | `SimpleIconDisabledOnDisabledBrush` |

### Popup Resources

| Key | Type | Default Token |
|-----|------|---------------|
| `SimpleComboBoxPopupBackground` | `SolidColorBrush` | `SimpleBackgroundDefaultDefaultBrush` |
| `SimpleComboBoxPopupBorderBrush` | `SolidColorBrush` | `SimpleBorderDefaultDefaultBrush` |

### ComboBoxItem Resources

| Key | Type | Default Token |
|-----|------|---------------|
| `SimpleComboBoxItemBackground` | `SolidColorBrush` | `SystemControlTransparentBrush` |
| `SimpleComboBoxItemBackgroundPointerOver` | `SolidColorBrush` | `SimpleBackgroundDefaultSecondaryBrush` |
| `SimpleComboBoxItemBackgroundPressed` | `SolidColorBrush` | `SimpleBackgroundDefaultSecondaryPressedBrush` |
| `SimpleComboBoxItemBackgroundSelected` | `SolidColorBrush` | `SimpleBackgroundBrandDefaultBrush` |
| `SimpleComboBoxItemBackgroundSelectedPointerOver` | `SolidColorBrush` | `SimpleBackgroundBrandHoverBrush` |
| `SimpleComboBoxItemBackgroundSelectedPressed` | `SolidColorBrush` | `SimpleBackgroundBrandPressedBrush` |
| `SimpleComboBoxItemBackgroundDisabled` | `SolidColorBrush` | `SystemControlTransparentBrush` |
| `SimpleComboBoxItemForeground` | `SolidColorBrush` | `SimpleTextDefaultDefaultBrush` |
| `SimpleComboBoxItemForegroundPointerOver` | `SolidColorBrush` | `SimpleTextDefaultDefaultBrush` |
| `SimpleComboBoxItemForegroundPressed` | `SolidColorBrush` | `SimpleTextDefaultDefaultBrush` |
| `SimpleComboBoxItemForegroundSelected` | `SolidColorBrush` | `SimpleTextBrandOnBrandBrush` |
| `SimpleComboBoxItemForegroundSelectedPointerOver` | `SolidColorBrush` | `SimpleTextBrandOnBrandBrush` |
| `SimpleComboBoxItemForegroundSelectedPressed` | `SolidColorBrush` | `SimpleTextBrandOnBrandBrush` |
| `SimpleComboBoxItemForegroundDisabled` | `SolidColorBrush` | `SimpleTextDisabledDefaultBrush` |

### Layout Resources

| Key | Type | Default Token |
|-----|------|---------------|
| `SimpleComboBoxCornerRadius` | `CornerRadius` | `SimpleRadius200CornerRadius` |
| `SimpleComboBoxMinHeight` | `Double` | `SimpleIconLarge` |
| `SimpleComboBoxBorderThickness` | `Thickness` | `SimpleStrokeBorderThickness` |
| `SimpleComboBoxFocusedBorderThickness` | `Thickness` | `SimpleStrokeFocusRingThickness` |
| `SimpleComboBoxPadding` | `Thickness` | `16,10,12,10` |
| `SimpleComboBoxDropDownGlyphMargin` | `Thickness` | `8,0,12,0` |
| `SimpleComboBoxPopupPadding` | `Thickness` | `0,4` |
| `SimpleComboBoxPopupMaxHeight` | `Double` | `400` |
| `SimpleComboBoxDropDownGlyphIcon` | `String` | `&#xE96E;` |

## Related Skills

- [simple-textbox](../simple-textbox/SKILL.md)
- [simple-autosuggestbox](../simple-autosuggestbox/SKILL.md)
