---
name: simple-passwordbox
description: Uses Simple Theme PasswordBox style in Uno Platform applications. Use when styling password input fields. Covers the single PasswordBox style, reveal button customization, and lightweight resource keys.
metadata:
  author: unoplatform
  version: "1.0"
  framework: uno-platform
  category: simple-theme-controls
---

# Simple Theme — PasswordBox Style

## Overview

The Simple theme provides a single PasswordBox style. When the Simple theme is active, the **implicit default style** applied to every `<PasswordBox>` is `SimplePasswordBoxStyle`.

## Available Styles

| Style Key | Alias(es) | IsDefaultStyle |
|-----------|-----------|----------------|
| `SimplePasswordBoxStyle` | `PasswordBoxStyle`, `FilledPasswordBoxStyle` | **Yes** |

## Usage Examples

```xml
<!-- Implicit — no Style needed -->
<PasswordBox Header="Password"
             PlaceholderText="Enter your password" />

<!-- With reveal button -->
<PasswordBox Header="Password"
             PlaceholderText="Enter your password"
             PasswordRevealMode="Peek" />
```

## Lightweight Styling

### Color Resources

| Key | Type | Default Token |
|-----|------|---------------|
| `SimplePasswordBoxForeground` | `SolidColorBrush` | `SimpleTextDefaultDefaultBrush` |
| `SimplePasswordBoxForegroundPointerOver` | `SolidColorBrush` | `SimpleTextDefaultDefaultBrush` |
| `SimplePasswordBoxForegroundFocused` | `SolidColorBrush` | `SimpleTextDefaultDefaultBrush` |
| `SimplePasswordBoxForegroundDisabled` | `SolidColorBrush` | `SimpleTextDisabledDefaultBrush` |
| `SimplePasswordBoxBackground` | `SolidColorBrush` | `SimpleBackgroundDefaultDefaultBrush` |
| `SimplePasswordBoxBackgroundPointerOver` | `SolidColorBrush` | `SimpleBackgroundDefaultDefaultBrush` |
| `SimplePasswordBoxBackgroundFocused` | `SolidColorBrush` | `SimpleBackgroundDefaultDefaultBrush` |
| `SimplePasswordBoxBackgroundDisabled` | `SolidColorBrush` | `SimpleBackgroundDisabledDefaultBrush` |
| `SimplePasswordBoxBorderBrush` | `SolidColorBrush` | `SimpleBorderDefaultDefaultBrush` |
| `SimplePasswordBoxBorderBrushPointerOver` | `SolidColorBrush` | `SimpleBorderDefaultDefaultBrush` |
| `SimplePasswordBoxBorderBrushFocused` | `SolidColorBrush` | `SimpleBorderBrandSecondaryBrush` |
| `SimplePasswordBoxBorderBrushDisabled` | `SolidColorBrush` | `SimpleBorderDisabledDefaultBrush` |
| `SimplePasswordBoxPlaceholderForeground` | `SolidColorBrush` | `SimpleTextDefaultTertiaryBrush` |
| `SimplePasswordBoxPlaceholderForegroundDisabled` | `SolidColorBrush` | `SimpleTextDisabledDefaultBrush` |
| `SimplePasswordBoxHeaderForeground` | `SolidColorBrush` | `SimpleTextDefaultDefaultBrush` |
| `SimplePasswordBoxHeaderForegroundDisabled` | `SolidColorBrush` | `SimpleTextDisabledDefaultBrush` |

### Reveal Button Resources

| Key | Type | Default Token |
|-----|------|---------------|
| `SimplePasswordBoxRevealForeground` | `SolidColorBrush` | `SimpleIconDefaultDefaultBrush` |
| `SimplePasswordBoxRevealForegroundPointerOver` | `SolidColorBrush` | `SimpleIconDefaultDefaultBrush` |
| `SimplePasswordBoxRevealForegroundPressed` | `SolidColorBrush` | `SimpleIconDefaultDefaultBrush` |

### Layout Resources

| Key | Type | Default Token |
|-----|------|---------------|
| `SimplePasswordBoxCornerRadius` | `CornerRadius` | `SimpleRadius200CornerRadius` |
| `SimplePasswordBoxBorderThickness` | `Thickness` | `SimpleStrokeBorderThickness` |
| `SimplePasswordBoxFocusedBorderThickness` | `Thickness` | `SimpleStrokeFocusRingThickness` |
| `SimplePasswordBoxPadding` | `Thickness` | `16,10,16,10` |
| `SimplePasswordBoxMinHeight` | `Double` | `SimpleIconLarge` |
| `SimplePasswordBoxHeaderMargin` | `Thickness` | `0,0,0,4` |

## Related Skills

- [simple-textbox](../simple-textbox/SKILL.md)
