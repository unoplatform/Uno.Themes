---
name: simple-expander
description: Uses Simple Theme Expander styles in Uno Platform applications. Use when styling expandable/collapsible content sections. Covers Expander styles, header and content customization, and lightweight resource keys.
metadata:
  author: unoplatform
  version: "1.0"
  framework: uno-platform
  category: simple-theme-controls
---

# Simple Theme — Expander Styles

## Overview

The Simple theme provides an Expander style plus internal header toggle button styles. When the Simple theme is active, the **implicit default style** applied to every `<Expander>` is `SimpleExpanderStyle`.

## Available Styles

| Style Key | Alias | IsDefaultStyle | TargetType |
|-----------|-------|----------------|------------|
| `SimpleExpanderStyle` | `ExpanderStyle` | **Yes** | `Expander` |
| `SimpleExpanderHeaderDownStyle` | — | — | `ToggleButton` (internal) |
| `SimpleExpanderHeaderUpStyle` | — | — | `ToggleButton` (internal, BasedOn DownStyle) |

## Usage Examples

```xml
<!-- Implicit — no Style needed -->
<Expander Header="More details">
    <TextBlock Text="Expanded content goes here." />
</Expander>
```

## Lightweight Styling

### Collapsed Header Resources

| Key | Type | Default Token |
|-----|------|---------------|
| `SimpleExpanderHeaderBackground` | `SolidColorBrush` | `SimpleBackgroundDefaultSecondaryBrush` |
| `SimpleExpanderHeaderBackgroundPointerOver` | `SolidColorBrush` | `SimpleBackgroundDefaultSecondaryHoverBrush` |
| `SimpleExpanderHeaderBackgroundPressed` | `SolidColorBrush` | `SimpleBackgroundDefaultSecondaryPressedBrush` |
| `SimpleExpanderHeaderBackgroundDisabled` | `SolidColorBrush` | `SimpleBackgroundDisabledDefaultBrush` |
| `SimpleExpanderHeaderForeground` | `SolidColorBrush` | `SimpleTextDefaultDefaultBrush` |
| `SimpleExpanderHeaderForegroundPointerOver` | `SolidColorBrush` | `SimpleTextDefaultDefaultBrush` |
| `SimpleExpanderHeaderForegroundPressed` | `SolidColorBrush` | `SimpleTextDefaultDefaultBrush` |
| `SimpleExpanderHeaderForegroundDisabled` | `SolidColorBrush` | `SimpleTextDisabledDefaultBrush` |
| `SimpleExpanderHeaderBorderBrush` | `SolidColorBrush` | `SimpleBorderDefaultDefaultBrush` |
| `SimpleExpanderHeaderBorderBrushPointerOver` | `SolidColorBrush` | `SimpleBorderDefaultDefaultBrush` |
| `SimpleExpanderHeaderBorderBrushPressed` | `SolidColorBrush` | `SimpleBorderDefaultDefaultBrush` |
| `SimpleExpanderHeaderBorderBrushDisabled` | `SolidColorBrush` | `SimpleBorderDisabledDefaultBrush` |

### Expanded Header Resources

| Key | Type | Default Token |
|-----|------|---------------|
| `SimpleExpanderHeaderExpandedBackground` | `SolidColorBrush` | `SimpleBackgroundDefaultDefaultBrush` |
| `SimpleExpanderHeaderExpandedBackgroundPointerOver` | `SolidColorBrush` | `SimpleBackgroundDefaultDefaultHoverBrush` |
| `SimpleExpanderHeaderExpandedBackgroundPressed` | `SolidColorBrush` | `SimpleBackgroundDefaultDefaultPressedBrush` |
| `SimpleExpanderHeaderExpandedBackgroundDisabled` | `SolidColorBrush` | `SimpleBackgroundDisabledDefaultBrush` |
| `SimpleExpanderHeaderExpandedForeground` | `SolidColorBrush` | `SimpleTextDefaultDefaultBrush` |
| `SimpleExpanderHeaderExpandedForegroundPointerOver` | `SolidColorBrush` | `SimpleTextDefaultDefaultBrush` |
| `SimpleExpanderHeaderExpandedForegroundPressed` | `SolidColorBrush` | `SimpleTextDefaultDefaultBrush` |
| `SimpleExpanderHeaderExpandedForegroundDisabled` | `SolidColorBrush` | `SimpleTextDisabledDefaultBrush` |
| `SimpleExpanderHeaderExpandedBorderBrush` | `SolidColorBrush` | `SimpleBorderDefaultDefaultBrush` |
| `SimpleExpanderHeaderExpandedBorderBrushPointerOver` | `SolidColorBrush` | `SimpleBorderDefaultDefaultBrush` |
| `SimpleExpanderHeaderExpandedBorderBrushPressed` | `SolidColorBrush` | `SimpleBorderDefaultDefaultBrush` |
| `SimpleExpanderHeaderExpandedBorderBrushDisabled` | `SolidColorBrush` | `SimpleBorderDisabledDefaultBrush` |

### Chevron Resources

| Key | Type | Default Token |
|-----|------|---------------|
| `SimpleExpanderChevronForeground` | `SolidColorBrush` | `SimpleIconDefaultDefaultBrush` |
| `SimpleExpanderChevronForegroundPointerOver` | `SolidColorBrush` | `SimpleIconDefaultDefaultBrush` |
| `SimpleExpanderChevronForegroundPressed` | `SolidColorBrush` | `SimpleIconDefaultDefaultBrush` |
| `SimpleExpanderChevronForegroundDisabled` | `SolidColorBrush` | `SimpleIconDisabledDefaultBrush` |

### Content Area Resources

| Key | Type | Default Token |
|-----|------|---------------|
| `SimpleExpanderContentBackground` | `SolidColorBrush` | `SimpleBackgroundDefaultDefaultBrush` |
| `SimpleExpanderContentBorderBrush` | `SolidColorBrush` | `SimpleBorderDefaultDefaultBrush` |
| `SimpleExpanderContentForeground` | `SolidColorBrush` | `SimpleTextDefaultDefaultBrush` |

### Layout Resources

| Key | Type | Default Token |
|-----|------|---------------|
| `SimpleExpanderMinHeight` | `Double` | `SimpleSpace1200` |
| `SimpleExpanderCornerRadius` | `CornerRadius` | `SimpleRadius200CornerRadius` |
| `SimpleExpanderHeaderPadding` | `Thickness` | `16,0,0,0` |
| `SimpleExpanderContentPadding` | `Thickness` | `SimpleSpace400Thickness` |
| `SimpleExpanderHeaderBorderThickness` | `Thickness` | `SimpleStrokeBorderThickness` |
| `SimpleExpanderContentBorderThickness` | `Thickness` | `1,0,1,1` |
| `SimpleExpanderContentUpBorderThickness` | `Thickness` | `1,1,1,0` |
| `SimpleExpanderChevronMargin` | `Thickness` | `20,0,8,0` |
| `SimpleExpanderChevronButtonSize` | `Double` | `SimpleIconMedium` |
| `SimpleExpanderChevronGlyphSize` | `Double` | `12` |
| `SimpleExpanderChevronDownGlyph` | `String` | `&#xE70D;` |

## Related Skills

- [simple-listview](../simple-listview/SKILL.md)
