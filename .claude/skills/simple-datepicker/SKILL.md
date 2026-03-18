---
name: simple-datepicker
description: Uses Simple Theme DatePicker styles in Uno Platform applications. Use when styling date picker controls with spinner flyout. Covers DatePicker and DatePickerFlyoutPresenter styles and lightweight resource keys.
metadata:
  author: unoplatform
  version: "1.0"
  framework: uno-platform
  category: simple-theme-controls
---

# Simple Theme — DatePicker Styles

## Overview

The Simple theme provides styles for both DatePicker and its flyout presenter. When the Simple theme is active, the **implicit default styles** are `SimpleDatePickerStyle` and `SimpleDatePickerFlyoutPresenterStyle`.

## Available Styles

| Style Key | Alias | IsDefaultStyle | TargetType |
|-----------|-------|----------------|------------|
| `SimpleDatePickerStyle` | `DatePickerStyle` | **Yes** | `DatePicker` |
| `SimpleDatePickerFlyoutPresenterStyle` | — | **Yes** | `DatePickerFlyoutPresenter` |
| `SimpleDatePickerFlyoutButtonStyle` | — | — | `Button` (internal) |

## Usage Examples

```xml
<!-- Implicit — no Style needed -->
<DatePicker Header="Date of birth" />
```

## Lightweight Styling

### DatePicker Button Colors

| Key | Type | Default Token |
|-----|------|---------------|
| `SimpleDatePickerButtonBackground` | `SolidColorBrush` | `SimpleBackgroundDefaultDefaultBrush` |
| `SimpleDatePickerButtonBackgroundPointerOver` | `SolidColorBrush` | `SimpleBackgroundDefaultDefaultBrush` |
| `SimpleDatePickerButtonBackgroundPressed` | `SolidColorBrush` | `SimpleBackgroundDefaultDefaultBrush` |
| `SimpleDatePickerButtonBackgroundDisabled` | `SolidColorBrush` | `SimpleBackgroundDisabledDefaultBrush` |
| `SimpleDatePickerButtonForeground` | `SolidColorBrush` | `SimpleTextDefaultDefaultBrush` |
| `SimpleDatePickerButtonForegroundPointerOver` | `SolidColorBrush` | `SimpleTextDefaultDefaultBrush` |
| `SimpleDatePickerButtonForegroundPressed` | `SolidColorBrush` | `SimpleTextDefaultDefaultBrush` |
| `SimpleDatePickerButtonForegroundDisabled` | `SolidColorBrush` | `SimpleTextDisabledDefaultBrush` |
| `SimpleDatePickerButtonBorderBrush` | `SolidColorBrush` | `SimpleBorderDefaultDefaultBrush` |
| `SimpleDatePickerButtonBorderBrushPointerOver` | `SolidColorBrush` | `SimpleBorderDefaultDefaultBrush` |
| `SimpleDatePickerButtonBorderBrushPressed` | `SolidColorBrush` | `SimpleBorderDefaultDefaultBrush` |
| `SimpleDatePickerButtonBorderBrushDisabled` | `SolidColorBrush` | `SimpleBorderDisabledDefaultBrush` |
| `SimpleDatePickerButtonDateTextForeground` | `SolidColorBrush` | `SimpleTextDefaultDefaultBrush` |
| `SimpleDatePickerButtonDateTextForegroundPointerOver` | `SolidColorBrush` | `SimpleTextDefaultDefaultBrush` |
| `SimpleDatePickerButtonDateTextForegroundPressed` | `SolidColorBrush` | `SimpleTextDefaultDefaultBrush` |
| `SimpleDatePickerButtonDateTextForegroundDisabled` | `SolidColorBrush` | `SimpleTextDisabledDefaultBrush` |
| `SimpleDatePickerPlaceholderTextForeground` | `SolidColorBrush` | `SimpleTextDefaultTertiaryBrush` |
| `SimpleDatePickerHeaderForeground` | `SolidColorBrush` | `SimpleTextDefaultDefaultBrush` |
| `SimpleDatePickerHeaderForegroundDisabled` | `SolidColorBrush` | `SimpleTextDisabledDefaultBrush` |

### Flyout Presenter Resources

| Key | Type | Default Token |
|-----|------|---------------|
| `SimpleDatePickerFlyoutPresenterBackground` | `SolidColorBrush` | `SimpleBackgroundDefaultDefaultBrush` |
| `SimpleDatePickerFlyoutPresenterBorderBrush` | `SolidColorBrush` | `SimpleBorderDefaultDefaultBrush` |
| `SimpleDatePickerFlyoutPresenterSpacerFill` | `SolidColorBrush` | `SimpleBorderDefaultDefaultBrush` |
| `SimpleDatePickerFlyoutPresenterHighlightFill` | `SolidColorBrush` | `SimpleBackgroundBrandTertiaryBrush` |
| `SimpleDatePickerFlyoutButtonBackground` | `SolidColorBrush` | `SystemControlTransparentBrush` |

### Layout Resources

| Key | Type | Default Token |
|-----|------|---------------|
| `SimpleDatePickerCornerRadius` | `CornerRadius` | `SimpleRadius200CornerRadius` |
| `SimpleDatePickerBorderThemeThickness` | `Thickness` | `SimpleStrokeBorderThickness` |
| `SimpleDatePickerContentMargin` | `Thickness` | `16,10,16,10` |
| `SimpleDatePickerMinHeight` | `Double` | `44` |
| `SimpleDatePickerHeaderMargin` | `Thickness` | `0,0,0,4` |
| `SimpleDatePickerFlyoutPresenterCornerRadius` | `CornerRadius` | `SimpleRadius400CornerRadius` |
| `SimpleDatePickerFlyoutPresenterWidth` | `Double` | `296` |
| `SimpleDatePickerFlyoutPresenterMinWidth` | `Double` | `296` |
| `SimpleDatePickerFlyoutPresenterMaxHeight` | `Double` | `398` |
| `SimpleDatePickerFlyoutPresenterHighlightHeight` | `Double` | `SimpleIconLarge` |
| `SimpleDatePickerFlyoutPresenterAcceptDismissHostGridHeight` | `Double` | `52` |
| `SimpleDatePickerFlyoutPresenterFontFamily` | `FontFamily` | `SimpleFontFamily` |

## Related Skills

- [simple-calendardatepicker](../simple-calendardatepicker/SKILL.md)
- [simple-calendarview](../simple-calendarview/SKILL.md)
