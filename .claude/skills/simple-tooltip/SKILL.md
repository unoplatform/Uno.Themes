---
name: simple-tooltip
description: Uses Simple Theme ToolTip style in Uno Platform applications. Use when styling tooltips for hover/focus information popups. Covers the single ToolTip style and lightweight resource keys.
metadata:
  author: unoplatform
  version: "1.0"
  framework: uno-platform
  category: simple-theme-controls
---

# Simple Theme — ToolTip Style

## Overview

The Simple theme provides a single ToolTip style. When the Simple theme is active, the **implicit default style** applied to every `<ToolTip>` is `SimpleToolTipStyle`.

## Available Styles

| Style Key | Alias | IsDefaultStyle |
|-----------|-------|----------------|
| `SimpleToolTipStyle` | `ToolTipStyle` | **Yes** |

## Usage Examples

```xml
<Button Content="Save">
    <ToolTipService.ToolTip>
        <ToolTip Content="Save your changes (Ctrl+S)" />
    </ToolTipService.ToolTip>
</Button>
```

## Lightweight Styling

### Color Resources

| Key | Type | Default Token |
|-----|------|---------------|
| `SimpleToolTipBackground` | `SolidColorBrush` | `SimpleBackgroundDefaultDefaultBrush` |
| `SimpleToolTipBorderBrush` | `SolidColorBrush` | `SimpleBorderDefaultDefaultBrush` |
| `SimpleToolTipForeground` | `SolidColorBrush` | `SimpleTextDefaultDefaultBrush` |

### Layout Resources

| Key | Type | Default Token |
|-----|------|---------------|
| `SimpleToolTipPadding` | `Thickness` | `12,8,12,8` |
| `SimpleToolTipCornerRadius` | `CornerRadius` | `SimpleRadius200CornerRadius` |
| `SimpleToolTipBorderThickness` | `Thickness` | `SimpleStrokeBorderThickness` |

## Related Skills

- [simple-skills-index](../simple-skills-index/SKILL.md)
