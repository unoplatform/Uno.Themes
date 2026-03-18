---
name: simple-listview
description: Uses Simple Theme ListView and ListViewItem styles in Uno Platform applications. Use when styling list views for scrollable item collections with selection support. Covers all ListView styles and lightweight resource keys.
metadata:
  author: unoplatform
  version: "1.0"
  framework: uno-platform
  category: simple-theme-controls
---

# Simple Theme — ListView & ListViewItem Styles

## Overview

The Simple theme provides styles for both ListView and ListViewItem. When the Simple theme is active, the **implicit default styles** are `SimpleListViewStyle` and `SimpleListViewItemStyle`.

## Available Styles

| Style Key | Alias | IsDefaultStyle | TargetType |
|-----------|-------|----------------|------------|
| `SimpleListViewStyle` | `ListViewStyle` | **Yes** | `ListView` |
| `SimpleListViewItemStyle` | `ListViewItemStyle` | **Yes** | `ListViewItem` |

## Usage Examples

```xml
<!-- Implicit — no Style needed -->
<ListView ItemsSource="{x:Bind Items}">
    <ListView.ItemTemplate>
        <DataTemplate>
            <TextBlock Text="{Binding Name}" />
        </DataTemplate>
    </ListView.ItemTemplate>
</ListView>
```

## Lightweight Styling

### ListView Container Resources

| Key | Type | Default Token |
|-----|------|---------------|
| `SimpleListViewBackground` | `SolidColorBrush` | `SimpleBackgroundDefaultDefaultBrush` |
| `SimpleListViewBorderBrush` | `SolidColorBrush` | `SimpleBorderDefaultDefaultBrush` |

### ListViewItem Resources

| Key | Type | Default Token |
|-----|------|---------------|
| `SimpleListViewItemBackground` | `SolidColorBrush` | `Transparent` |
| `SimpleListViewItemForeground` | `SolidColorBrush` | `SimpleTextDefaultDefaultBrush` |
| `SimpleListViewItemSecondaryForeground` | `SolidColorBrush` | `SimpleTextDefaultSecondaryBrush` |
| `SimpleListViewItemBackgroundPointerOver` | `SolidColorBrush` | `SimpleBackgroundDefaultSecondaryBrush` |
| `SimpleListViewItemForegroundPointerOver` | `SolidColorBrush` | `SimpleTextDefaultDefaultBrush` |
| `SimpleListViewItemBackgroundPressed` | `SolidColorBrush` | `SimpleBackgroundDefaultTertiaryBrush` |
| `SimpleListViewItemForegroundPressed` | `SolidColorBrush` | `SimpleTextDefaultDefaultBrush` |
| `SimpleListViewItemBackgroundSelected` | `SolidColorBrush` | `SimpleBackgroundBrandDefaultBrush` |
| `SimpleListViewItemForegroundSelected` | `SolidColorBrush` | `SimpleTextBrandOnBrandBrush` |
| `SimpleListViewItemBackgroundSelectedPointerOver` | `SolidColorBrush` | `SimpleBackgroundBrandHoverBrush` |
| `SimpleListViewItemForegroundSelectedPointerOver` | `SolidColorBrush` | `SimpleTextBrandOnBrandBrush` |
| `SimpleListViewItemBackgroundSelectedPressed` | `SolidColorBrush` | `SimpleBackgroundBrandPressedBrush` |
| `SimpleListViewItemForegroundSelectedPressed` | `SolidColorBrush` | `SimpleTextBrandOnBrandBrush` |
| `SimpleListViewItemForegroundDisabled` | `SolidColorBrush` | `SimpleTextDisabledDefaultBrush` |

## Related Skills

- [simple-expander](../simple-expander/SKILL.md)
- [simple-menuflyout](../simple-menuflyout/SKILL.md)
