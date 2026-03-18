---
name: simple-calendarview
description: Uses Simple Theme CalendarView styles in Uno Platform applications. Use when styling inline calendar views for date browsing and selection. Covers both CalendarView styles and lightweight resource keys for day items, navigation, selection, and today indicators.
metadata:
  author: unoplatform
  version: "1.0"
  framework: uno-platform
  category: simple-theme-controls
---

# Simple Theme — CalendarView Styles

## Overview

The Simple theme provides a single CalendarView style. When the Simple theme is active, the **implicit default style** applied to every `<CalendarView>` is `SimpleCalendarViewStyle`.

## Available Styles

| Style Key | Alias | IsDefaultStyle |
|-----------|-------|----------------|
| `SimpleCalendarViewStyle` | `CalendarViewStyle` | **Yes** |

Internal template styles (not for direct use):
- `SimpleWeekDayNameStyle` (TextBlock)
- `SimpleNavigationButtonStyle` (Button)
- `SimpleHeaderButtonStyle` (Button, BasedOn NavigationButtonStyle)
- `SimpleScrollViewerStyle` (ScrollViewer)

## Usage Examples

```xml
<!-- Implicit — no Style needed -->
<CalendarView />

<!-- With selection -->
<CalendarView SelectionMode="Single" />
```

## Lightweight Styling

### Container Resources

| Key | Type | Default Token |
|-----|------|---------------|
| `SimpleCalendarViewBackground` | `SolidColorBrush` | `SimpleBackgroundDefaultDefaultBrush` |
| `SimpleCalendarViewBorderBrush` | `SolidColorBrush` | `SimpleBorderDefaultDefaultBrush` |
| `SimpleCalendarViewForeground` | `SolidColorBrush` | `SimpleTextDefaultDefaultBrush` |

### Header & Navigation Resources

| Key | Type | Default Token |
|-----|------|---------------|
| `SimpleCalendarViewHeaderForeground` | `SolidColorBrush` | `SimpleTextDefaultDefaultBrush` |
| `SimpleCalendarViewNavigationButtonForeground` | `SolidColorBrush` | `SimpleTextDefaultDefaultBrush` |
| `SimpleCalendarViewNavigationButtonForegroundPointerOver` | `SolidColorBrush` | `SimpleTextDefaultSecondaryBrush` |
| `SimpleCalendarViewNavigationButtonForegroundPressed` | `SolidColorBrush` | `SimpleTextDefaultSecondaryBrush` |
| `SimpleCalendarViewNavigationButtonBackground` | `SolidColorBrush` | `SystemControlTransparentBrush` |
| `SimpleCalendarViewWeekDayForeground` | `SolidColorBrush` | `SimpleTextDefaultSecondaryBrush` |
| `SimpleCalendarViewWeekDayForegroundDisabled` | `SolidColorBrush` | `SimpleTextDisabledDefaultBrush` |

### Day Item Resources

| Key | Type | Default Token |
|-----|------|---------------|
| `SimpleCalendarViewCalendarItemForeground` | `SolidColorBrush` | `SimpleTextDefaultDefaultBrush` |
| `SimpleCalendarViewCalendarItemBackground` | `SolidColorBrush` | `SystemControlTransparentBrush` |
| `SimpleCalendarViewCalendarItemBorderBrush` | `SolidColorBrush` | `SystemControlTransparentBrush` |
| `SimpleCalendarViewHoverBorderBrush` | `SolidColorBrush` | `SimpleBorderDefaultDefaultBrush` |
| `SimpleCalendarViewPressedBorderBrush` | `SolidColorBrush` | `SimpleBorderDefaultDefaultBrush` |
| `SimpleCalendarViewPressedForeground` | `SolidColorBrush` | `SimpleTextDefaultDefaultBrush` |
| `SimpleCalendarViewFocusBorderBrush` | `SolidColorBrush` | `SimpleBackgroundBrandDefaultBrush` |

### Selected Day Resources

| Key | Type | Default Token |
|-----|------|---------------|
| `SimpleCalendarViewSelectedBackground` | `SolidColorBrush` | `SimpleBackgroundBrandDefaultBrush` |
| `SimpleCalendarViewSelectedForeground` | `SolidColorBrush` | `SimpleTextBrandOnBrandBrush` |
| `SimpleCalendarViewSelectedBorderBrush` | `SolidColorBrush` | `SimpleBackgroundBrandDefaultBrush` |
| `SimpleCalendarViewSelectedHoverBorderBrush` | `SolidColorBrush` | `SimpleBackgroundBrandHoverBrush` |
| `SimpleCalendarViewSelectedPressedBorderBrush` | `SolidColorBrush` | `SimpleBackgroundBrandHoverBrush` |

### Today & Special Day Resources

| Key | Type | Default Token |
|-----|------|---------------|
| `SimpleCalendarViewTodayForeground` | `SolidColorBrush` | `SimpleTextBrandDefaultBrush` |
| `SimpleCalendarViewTodaySelectedBackground` | `SolidColorBrush` | `SimpleBackgroundBrandDefaultBrush` |
| `SimpleCalendarViewBlackoutForeground` | `SolidColorBrush` | `SimpleTextDisabledDefaultBrush` |
| `SimpleCalendarViewOutOfScopeForeground` | `SolidColorBrush` | `SimpleTextDisabledDefaultBrush` |
| `SimpleCalendarViewOutOfScopeBackground` | `SolidColorBrush` | `SystemControlTransparentBrush` |

### Layout Resources

| Key | Type | Default Value |
|-----|------|---------------|
| `SimpleCalendarViewHeaderPadding` | `Thickness` | `12,0,0,0` |
| `SimpleDownArrowPathData` | `String` | `M0,0L32,0 16,19.745z` |

## Related Skills

- [simple-calendardatepicker](../simple-calendardatepicker/SKILL.md)
- [simple-datepicker](../simple-datepicker/SKILL.md)
