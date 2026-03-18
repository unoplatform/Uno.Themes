---
name: simple-calendardatepicker
description: Uses Simple Theme CalendarDatePicker styles in Uno Platform applications. Use when styling calendar date picker controls for date selection with flyout calendar. Covers both styles and lightweight resource keys.
metadata:
  author: unoplatform
  version: "1.0"
  framework: uno-platform
  category: simple-theme-controls
---

# Simple Theme — CalendarDatePicker Styles

## Overview

The Simple theme provides a single CalendarDatePicker style. When the Simple theme is active, the **implicit default style** applied to every `<CalendarDatePicker>` is `SimpleCalendarDatePickerStyle`.

## Available Styles

| Style Key | Alias | IsDefaultStyle |
|-----------|-------|----------------|
| `SimpleCalendarDatePickerStyle` | `CalendarDatePickerStyle` | **Yes** |

## Usage Examples

```xml
<!-- Implicit — no Style needed -->
<CalendarDatePicker Header="Select a date"
                    PlaceholderText="Pick a date" />
```

## Lightweight Styling

### Color Resources

| Key | Type | Default Token |
|-----|------|---------------|
| `SimpleCalendarDatePickerForeground` | `SolidColorBrush` | `SimpleTextDefaultDefaultBrush` |
| `SimpleCalendarDatePickerForegroundDisabled` | `SolidColorBrush` | `SimpleTextDisabledDefaultBrush` |
| `SimpleCalendarDatePickerTextForeground` | `SolidColorBrush` | `SimpleTextDefaultTertiaryBrush` |
| `SimpleCalendarDatePickerTextForegroundDisabled` | `SolidColorBrush` | `SimpleTextDisabledDefaultBrush` |
| `SimpleCalendarDatePickerTextForegroundSelected` | `SolidColorBrush` | `SimpleTextDefaultDefaultBrush` |
| `SimpleCalendarDatePickerCalendarGlyphForeground` | `SolidColorBrush` | `SimpleIconDefaultDefaultBrush` |
| `SimpleCalendarDatePickerCalendarGlyphForegroundDisabled` | `SolidColorBrush` | `SimpleIconDisabledOnDisabledBrush` |
| `SimpleCalendarDatePickerHeaderForeground` | `SolidColorBrush` | `SimpleTextDefaultDefaultBrush` |
| `SimpleCalendarDatePickerHeaderForegroundDisabled` | `SolidColorBrush` | `SimpleTextDisabledDefaultBrush` |
| `SimpleCalendarDatePickerBackground` | `SolidColorBrush` | `SimpleBackgroundDefaultDefaultBrush` |
| `SimpleCalendarDatePickerBackgroundPointerOver` | `SolidColorBrush` | `SimpleBackgroundDefaultDefaultBrush` |
| `SimpleCalendarDatePickerBackgroundPressed` | `SolidColorBrush` | `SimpleBackgroundDefaultDefaultBrush` |
| `SimpleCalendarDatePickerBackgroundDisabled` | `SolidColorBrush` | `SimpleBackgroundDisabledDefaultBrush` |
| `SimpleCalendarDatePickerBackgroundFocused` | `SolidColorBrush` | `SimpleBackgroundDefaultDefaultBrush` |
| `SimpleCalendarDatePickerBorderBrush` | `SolidColorBrush` | `SimpleBorderDefaultDefaultBrush` |
| `SimpleCalendarDatePickerBorderBrushPointerOver` | `SolidColorBrush` | `SimpleBorderDefaultDefaultBrush` |
| `SimpleCalendarDatePickerBorderBrushPressed` | `SolidColorBrush` | `SimpleBorderDefaultDefaultBrush` |
| `SimpleCalendarDatePickerBorderBrushDisabled` | `SolidColorBrush` | `SimpleBorderDisabledDefaultBrush` |
| `SimpleCalendarDatePickerBorderBrushFocused` | `SolidColorBrush` | `SimpleBorderBrandSecondaryBrush` |

### Layout Resources

| Key | Type | Default Token |
|-----|------|---------------|
| `SimpleCalendarDatePickerCornerRadius` | `CornerRadius` | `SimpleRadius200CornerRadius` |
| `SimpleCalendarDatePickerFlyoutPresenterCornerRadius` | `CornerRadius` | `SimpleRadius400CornerRadius` |
| `SimpleCalendarDatePickerBorderThemeThickness` | `Thickness` | `SimpleStrokeBorderThickness` |
| `SimpleCalendarDatePickerFlyoutPresenterPadding` | `Thickness` | `SimpleSpace0Thickness` |
| `SimpleCalendarDatePickerContentMargin` | `Thickness` | `12,0,12,0` |
| `SimpleCalendarDatePickerMinHeight` | `Double` | `SimpleIconLarge` |
| `SimpleCalendarDatePickerHeaderMargin` | `Thickness` | `0,0,0,8` |
| `SimpleCalendarDatePickerCalendarGlyphMargin` | `Thickness` | `12,0,0,0` |

## Related Skills

- [simple-datepicker](../simple-datepicker/SKILL.md)
- [simple-calendarview](../simple-calendarview/SKILL.md)
