---
name: simple-appbarbutton
description: Uses Simple Theme AppBarButton styles in Uno Platform applications. Use when styling app bar buttons in CommandBar or NavigationView. Covers both AppBarButton styles and lightweight resource keys.
metadata:
  author: unoplatform
  version: "1.0"
  framework: uno-platform
  category: simple-theme-controls
---

# Simple Theme — AppBarButton Styles

## Overview

The Simple theme provides a single AppBarButton style. When the Simple theme is active, the **implicit default style** applied to every `<AppBarButton>` is `SimpleAppBarButtonStyle`.

## Available Styles

| Style Key | Alias | IsDefaultStyle |
|-----------|-------|----------------|
| `SimpleAppBarButtonStyle` | `AppBarButtonStyle` | **Yes** |

## Usage Examples

```xml
<CommandBar>
    <AppBarButton Icon="Save" Label="Save" />
    <AppBarButton Icon="Delete" Label="Delete" />
</CommandBar>
```

## Lightweight Styling

### Color Resources

| Key | Type | Default Token |
|-----|------|---------------|
| `SimpleAppBarButtonForeground` | `SolidColorBrush` | `SimpleTextDefaultSecondaryBrush` |
| `SimpleAppBarButtonForegroundPointerOver` | `SolidColorBrush` | `SimpleTextDefaultSecondaryBrush` |
| `SimpleAppBarButtonForegroundPressed` | `SolidColorBrush` | `SimpleTextDefaultSecondaryBrush` |
| `SimpleAppBarButtonForegroundDisabled` | `SolidColorBrush` | `SimpleTextDisabledDefaultBrush` |
| `SimpleAppBarButtonIconForeground` | `SolidColorBrush` | `SimpleIconDefaultSecondaryBrush` |
| `SimpleAppBarButtonIconForegroundPointerOver` | `SolidColorBrush` | `SimpleIconDefaultSecondaryBrush` |
| `SimpleAppBarButtonIconForegroundPressed` | `SolidColorBrush` | `SimpleIconDefaultSecondaryBrush` |
| `SimpleAppBarButtonIconForegroundDisabled` | `SolidColorBrush` | `SimpleIconDisabledDefaultBrush` |
| `SimpleAppBarButtonBackground` | `SolidColorBrush` | `SystemControlTransparentBrush` |
| `SimpleAppBarButtonBackgroundPointerOver` | `SolidColorBrush` | `SimpleBackgroundDefaultDefaultHoverBrush` |
| `SimpleAppBarButtonBackgroundPressed` | `SolidColorBrush` | `SimpleBackgroundDefaultDefaultPressedBrush` |
| `SimpleAppBarButtonBackgroundDisabled` | `SolidColorBrush` | `SystemControlTransparentBrush` |
| `SimpleAppBarButtonBorderBrush` | `SolidColorBrush` | `SystemControlTransparentBrush` |
| `SimpleAppBarButtonBorderBrushPointerOver` | `SolidColorBrush` | `SystemControlTransparentBrush` |
| `SimpleAppBarButtonBorderBrushPressed` | `SolidColorBrush` | `SystemControlTransparentBrush` |
| `SimpleAppBarButtonBorderBrushDisabled` | `SolidColorBrush` | `SystemControlTransparentBrush` |

### Sizing Resources

| Key | Type | Default Token |
|-----|------|---------------|
| `SimpleAppBarButtonHeight` | `Double` | `SimpleSpace1600` |
| `SimpleAppBarButtonWidth` | `Double` | `SimpleSpace1600` |
| `SimpleAppBarButtonIconSize` | `Double` | `SimpleIconSmall` |
| `SimpleAppBarButtonContentSpacing` | `Double` | `SimpleSpace200` |
| `SimpleAppBarButtonPadding` | `Thickness` | `SimpleSpace200Thickness` |

## Related Skills

- [simple-button](../simple-button/SKILL.md)
