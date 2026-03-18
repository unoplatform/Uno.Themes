---
name: simple-menuflyout
description: Uses Simple Theme MenuFlyout styles in Uno Platform applications. Use when styling context menus, flyout menus, and menu items. Covers MenuFlyoutPresenter, MenuFlyoutItem, MenuFlyoutSubItem, MenuFlyoutSeparator, and ToggleMenuFlyoutItem styles with all lightweight resource keys.
metadata:
  author: unoplatform
  version: "1.0"
  framework: uno-platform
  category: simple-theme-controls
---

# Simple Theme — MenuFlyout Styles

## Overview

The Simple theme provides styles for all MenuFlyout-related controls. When the Simple theme is active, implicit defaults are applied to all menu flyout types.

## Available Styles

| Style Key | Alias | IsDefaultStyle | TargetType |
|-----------|-------|----------------|------------|
| `SimpleMenuFlyoutPresenterStyle` | `MenuFlyoutPresenterStyle` | **Yes** | `MenuFlyoutPresenter` |
| `SimpleMenuFlyoutItemStyle` | `MenuFlyoutItemStyle` | **Yes** | `MenuFlyoutItem` |
| `SimpleMenuFlyoutSubItemStyle` | `MenuFlyoutSubItemStyle` | **Yes** | `MenuFlyoutSubItem` |
| `SimpleMenuFlyoutSeparatorStyle` | `MenuFlyoutSeparatorStyle` | **Yes** | `MenuFlyoutSeparator` |
| `SimpleToggleMenuFlyoutItemStyle` | `ToggleMenuFlyoutItemStyle` | **Yes** | `ToggleMenuFlyoutItem` |

## Usage Examples

```xml
<Button Content="Options">
    <Button.Flyout>
        <MenuFlyout>
            <MenuFlyoutItem Text="Cut" />
            <MenuFlyoutItem Text="Copy" />
            <MenuFlyoutItem Text="Paste" />
            <MenuFlyoutSeparator />
            <MenuFlyoutSubItem Text="More">
                <MenuFlyoutItem Text="Select All" />
            </MenuFlyoutSubItem>
            <ToggleMenuFlyoutItem Text="Word Wrap" />
        </MenuFlyout>
    </Button.Flyout>
</Button>
```

## Lightweight Styling

### Presenter Resources

| Key | Type | Default Token |
|-----|------|---------------|
| `SimpleMenuFlyoutPresenterBackground` | `SolidColorBrush` | `SimpleBackgroundDefaultDefaultBrush` |
| `SimpleMenuFlyoutPresenterBorderBrush` | `SolidColorBrush` | `SimpleBorderDefaultDefaultBrush` |

### Item Color Resources

| Key | Type | Default Token |
|-----|------|---------------|
| `SimpleMenuFlyoutItemBackground` | `SolidColorBrush` | `Transparent` |
| `SimpleMenuFlyoutItemForeground` | `SolidColorBrush` | `SimpleTextDefaultDefaultBrush` |
| `SimpleMenuFlyoutItemDescriptionForeground` | `SolidColorBrush` | `SimpleTextDefaultSecondaryBrush` |
| `SimpleMenuFlyoutItemIconForeground` | `SolidColorBrush` | `SimpleIconDefaultDefaultBrush` |
| `SimpleMenuFlyoutItemBackgroundPointerOver` | `SolidColorBrush` | `SimpleBackgroundBrandDefaultBrush` |
| `SimpleMenuFlyoutItemForegroundPointerOver` | `SolidColorBrush` | `SimpleTextBrandOnBrandBrush` |
| `SimpleMenuFlyoutItemIconForegroundPointerOver` | `SolidColorBrush` | `SimpleIconBrandOnBrandBrush` |
| `SimpleMenuFlyoutItemBackgroundPressed` | `SolidColorBrush` | `SimpleBackgroundBrandPressedBrush` |
| `SimpleMenuFlyoutItemForegroundPressed` | `SolidColorBrush` | `SimpleTextBrandOnBrandBrush` |
| `SimpleMenuFlyoutItemIconForegroundPressed` | `SolidColorBrush` | `SimpleIconBrandOnBrandBrush` |
| `SimpleMenuFlyoutItemForegroundDisabled` | `SolidColorBrush` | `SimpleTextDisabledDefaultBrush` |
| `SimpleMenuFlyoutItemIconForegroundDisabled` | `SolidColorBrush` | `SimpleIconDisabledDefaultBrush` |

### Separator Resources

| Key | Type | Default Token |
|-----|------|---------------|
| `SimpleMenuFlyoutSeparatorBackground` | `SolidColorBrush` | `SimpleBorderDefaultDefaultBrush` |

### Layout Resources

| Key | Type | Default Token |
|-----|------|---------------|
| `SimpleMenuFlyoutPresenterMinWidth` | `Double` | `112` |
| `SimpleMenuFlyoutPresenterMaxWidth` | `Double` | `320` |
| `SimpleMenuFlyoutItemHeight` | `Double` | `SimpleIconLarge` |
| `SimpleMenuFlyoutSeparatorHeight` | `Double` | `SimpleStrokeBorder` |
| `SimpleMenuFlyoutItemPadding` | `Thickness` | `12,8` |
| `SimpleMenuFlyoutItemIconMargin` | `Thickness` | `0,0,16,0` |
| `SimpleMenuFlyoutItemAcceleratorMargin` | `Thickness` | `16,0,0,0` |
| `SimpleMenuFlyoutCheckGlyphMargin` | `Thickness` | `0,0,8,0` |
| `SimpleMenuFlyoutSeparatorPadding` | `Thickness` | `16,4` |

## Related Skills

- [simple-button](../simple-button/SKILL.md)
- [simple-contentdialog](../simple-contentdialog/SKILL.md)
