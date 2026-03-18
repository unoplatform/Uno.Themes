---
name: simple-textbox
description: Uses Simple Theme TextBox styles in Uno Platform applications. Use when styling text input fields with Input (filled), Outlined, Error, or Small variants. Covers all TextBox style keys, lightweight resource keys, and appropriate use cases.
metadata:
  author: unoplatform
  version: "1.0"
  framework: uno-platform
  category: simple-theme-controls
---

# Simple Theme — TextBox Styles

## Overview

The Simple theme provides four TextBox styles. When the Simple theme is active, the **implicit default style** applied to every `<TextBox>` is `SimpleInputTextBoxStyle`.

## Available Styles

| Style Key | Alias(es) | IsDefaultStyle | Use Case |
|-----------|-----------|----------------|----------|
| `SimpleInputTextBoxStyle` | `InputTextBoxStyle`, `FilledTextBoxStyle` | **Yes** | Standard text input fields |
| `SimpleOutlinedInputTextBoxStyle` | `OutlinedInputTextBoxStyle`, `OutlinedTextBoxStyle` | | Alternative outlined border appearance |
| `SimpleInputTextBoxErrorStyle` | `InputTextBoxErrorStyle` | | Fields in an error/validation-failed state |
| `SimpleInputTextBoxSmallStyle` | `InputTextBoxSmallStyle` | | Compact text input for dense layouts |

## Style Hierarchy (BasedOn)

```
SimpleInputTextBoxStyle
├── SimpleOutlinedInputTextBoxStyle
├── SimpleInputTextBoxErrorStyle
└── SimpleInputTextBoxSmallStyle
```

## Usage Examples

### Standard Input (Default)

```xml
<!-- Implicit — no Style needed -->
<TextBox Header="Name"
         PlaceholderText="Enter your name" />

<!-- Explicit -->
<TextBox Header="Name"
         PlaceholderText="Enter your name"
         Style="{StaticResource SimpleInputTextBoxStyle}" />
```

### Outlined Input

```xml
<TextBox Header="Email"
         PlaceholderText="you@example.com"
         Style="{StaticResource SimpleOutlinedInputTextBoxStyle}" />
```

### Error State

```xml
<TextBox Header="Email"
         PlaceholderText="you@example.com"
         Style="{StaticResource SimpleInputTextBoxErrorStyle}" />
```

### Small / Compact

```xml
<TextBox PlaceholderText="Search..."
         Style="{StaticResource SimpleInputTextBoxSmallStyle}" />
```

## When to Use Each Style

| Style | When to Use |
|-------|-------------|
| **Input** (default) | Standard forms, settings pages, data entry |
| **Outlined** | When you want a more prominent border that persists across states; alternate visual weight alongside default inputs |
| **Error** | When validation fails — apply programmatically to signal an invalid field |
| **Small** | Search bars, filter fields, dense table editors, inline editing |

## Lightweight Styling

### Resource Key Pattern

`SimpleTextBox{Property}{State}`

States: *(none)* = Normal, `PointerOver`, `Focused`, `Disabled`

### Color Resources

| Key | Type | Default Token |
|-----|------|---------------|
| `SimpleTextBoxForeground` | `SolidColorBrush` | `SimpleTextDefaultDefaultBrush` |
| `SimpleTextBoxForegroundPointerOver` | `SolidColorBrush` | `SimpleTextDefaultDefaultBrush` |
| `SimpleTextBoxForegroundFocused` | `SolidColorBrush` | `SimpleTextDefaultDefaultBrush` |
| `SimpleTextBoxForegroundDisabled` | `SolidColorBrush` | `SimpleTextDisabledDefaultBrush` |
| `SimpleTextBoxBackground` | `SolidColorBrush` | `SimpleBackgroundDefaultDefaultBrush` |
| `SimpleTextBoxBackgroundPointerOver` | `SolidColorBrush` | `SimpleBackgroundDefaultDefaultBrush` |
| `SimpleTextBoxBackgroundFocused` | `SolidColorBrush` | `SimpleBackgroundDefaultDefaultBrush` |
| `SimpleTextBoxBackgroundDisabled` | `SolidColorBrush` | `SimpleBackgroundDisabledDefaultBrush` |
| `SimpleTextBoxBorderBrush` | `SolidColorBrush` | `SimpleBorderDefaultDefaultBrush` |
| `SimpleTextBoxBorderBrushPointerOver` | `SolidColorBrush` | `SimpleBorderDefaultDefaultBrush` |
| `SimpleTextBoxBorderBrushFocused` | `SolidColorBrush` | `SimpleBorderBrandSecondaryBrush` |
| `SimpleTextBoxBorderBrushDisabled` | `SolidColorBrush` | `SimpleBorderDisabledDefaultBrush` |
| `SimpleTextBoxPlaceholderForeground` | `SolidColorBrush` | `SimpleTextDefaultTertiaryBrush` |
| `SimpleTextBoxPlaceholderForegroundDisabled` | `SolidColorBrush` | `SimpleTextDisabledDefaultBrush` |
| `SimpleTextBoxHeaderForeground` | `SolidColorBrush` | `SimpleTextDefaultDefaultBrush` |
| `SimpleTextBoxHeaderForegroundDisabled` | `SolidColorBrush` | `SimpleTextDisabledDefaultBrush` |

### Error-Specific Resources

| Key | Type | Default Token |
|-----|------|---------------|
| `SimpleTextBoxErrorBorderBrush` | `SolidColorBrush` | `SimpleBorderDangerDefaultBrush` |
| `SimpleTextBoxErrorBorderBrushPointerOver` | `SolidColorBrush` | `SimpleBorderDangerDefaultBrush` |
| `SimpleTextBoxErrorBorderBrushFocused` | `SolidColorBrush` | `SimpleBorderDangerDefaultBrush` |

### Layout Resources

| Key | Type | Default Token |
|-----|------|---------------|
| `SimpleTextBoxCornerRadius` | `CornerRadius` | `SimpleRadius200CornerRadius` |
| `SimpleTextBoxBorderThickness` | `Thickness` | `SimpleStrokeBorderThickness` |
| `SimpleTextBoxFocusedBorderThickness` | `Thickness` | `SimpleStrokeFocusRingThickness` |
| `SimpleTextBoxPadding` | `Thickness` | `16,10,16,10` |
| `SimpleTextBoxMinHeight` | `Double` | `SimpleIconLarge` |
| `SimpleTextBoxSmallMinHeight` | `Double` | `SimpleIconMedium` |
| `SimpleTextBoxHeaderMargin` | `Thickness` | `0,0,0,4` |

### Lightweight Styling Example

```xml
<TextBox Header="Custom Field"
         PlaceholderText="Enter value">
    <TextBox.Resources>
        <SolidColorBrush x:Key="SimpleTextBoxBorderBrushFocused" Color="DarkBlue" />
        <SolidColorBrush x:Key="SimpleTextBoxBackground" Color="#F5F5F5" />
    </TextBox.Resources>
</TextBox>
```

## Related Skills

- [simple-passwordbox](../simple-passwordbox/SKILL.md)
- [simple-combobox](../simple-combobox/SKILL.md)
- [simple-autosuggestbox](../simple-autosuggestbox/SKILL.md)
