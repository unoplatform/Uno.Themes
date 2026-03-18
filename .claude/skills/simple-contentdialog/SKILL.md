---
name: simple-contentdialog
description: Uses Simple Theme ContentDialog styles in Uno Platform applications. Use when styling modal dialogs for confirmations, alerts, or custom content. Covers ContentDialog styles, internal button styles, and lightweight resource keys.
metadata:
  author: unoplatform
  version: "1.0"
  framework: uno-platform
  category: simple-theme-controls
---

# Simple Theme — ContentDialog Styles

## Overview

The Simple theme provides a ContentDialog style plus internal button styles. When the Simple theme is active, the **implicit default style** applied to every `<ContentDialog>` is `SimpleContentDialogStyle`.

## Available Styles

| Style Key | Alias | IsDefaultStyle | TargetType | Use Case |
|-----------|-------|----------------|------------|----------|
| `SimpleContentDialogStyle` | `ContentDialogStyle` | **Yes** | `ContentDialog` | Modal dialogs |
| `SimpleContentDialogButtonStyle` | — | — | `Button` | Secondary dialog buttons (internal, BasedOn `SimpleSubtleButtonStyle`) |
| `SimpleContentDialogDefaultButtonStyle` | — | — | `Button` | Primary dialog button (internal, BasedOn `SimplePrimaryButtonStyle`) |

## Usage Examples

```xml
<ContentDialog Title="Delete item?"
               Content="This action cannot be undone."
               PrimaryButtonText="Delete"
               CloseButtonText="Cancel" />
```

## Lightweight Styling

### Color Resources

| Key | Type | Default Token |
|-----|------|---------------|
| `SimpleContentDialogBackground` | `SolidColorBrush` | `SimpleBackgroundDefaultDefaultBrush` |
| `SimpleContentDialogBorderBrush` | `SolidColorBrush` | `SimpleBorderDefaultDefaultBrush` |
| `SimpleContentDialogSmokeLayerBackground` | `SolidColorBrush` | `SimpleBackgroundUtilitiesScrimBrush` |
| `SimpleContentDialogTitleForeground` | `SolidColorBrush` | `SimpleTextDefaultDefaultBrush` |
| `SimpleContentDialogContentForeground` | `SolidColorBrush` | `SimpleTextDefaultSecondaryBrush` |

### Layout Resources

| Key | Type | Default Token |
|-----|------|---------------|
| `SimpleContentDialogMinWidth` | `Double` | `288` |
| `SimpleContentDialogMinHeight` | `Double` | `132` |
| `SimpleContentDialogMaxWidth` | `Double` | `560` |
| `SimpleContentDialogMaxHeight` | `Double` | `560` |
| `SimpleContentDialogEdgeMargin` | `Thickness` | `SimpleSpace800Thickness` |
| `SimpleContentDialogPanelPadding` | `Thickness` | `SimpleSpace800Thickness` |
| `SimpleContentDialogCornerRadius` | `CornerRadius` | `SimpleRadius200CornerRadius` |
| `SimpleContentDialogButtonSpacing` | `GridLength` | `8` |
| `SimpleContentDialogTitleToContentMargin` | `Thickness` | `0,0,0,8` |
| `SimpleContentDialogCommandSpaceToContentMargin` | `Thickness` | `0,24,0,0` |

## Related Skills

- [simple-button](../simple-button/SKILL.md)
