---
uid: Uno.Themes.Simple.Styles.CalendarView
---

# CalendarView Control

## Styles

| Style Key                          | IsDefaultStyle\* |
|------------------------------------|------------------|
| `SimpleCalendarViewStyle`          |                  |
| `SimpleDefaultCalendarViewStyle`   | True             |

IsDefaultStyle\*: Styles in this column will be set as the default implicit style for the matching control

## Lightweight Styling

### Themed Resources (ThemeDictionaries)

| Key                               | Type        | Value      |
|-----------------------------------|-------------|------------|
| `SimpleCalendarViewHeaderPadding` | `Thickness` | 12,0,0,0   |

### Theme-Agnostic Resources

| Key                        | Type     | Value                          |
|----------------------------|----------|--------------------------------|
| `SimpleDownArrowPathData`  | `String` | M0,0L32,0 16,19.745z          |

> [!NOTE]
> The CalendarView style sets most visual properties (borders, foregrounds, backgrounds, typography) directly via CalendarView-specific dependency properties (e.g., `FocusBorderBrush`, `SelectedForeground`, `CalendarItemBackground`, `TodayForeground`, etc.) using shared theme brushes like `PrimaryBrush`, `OnSurfaceBrush`, `OutlineBrush`, `OnPrimaryBrush`, `OnSurfaceDisabledBrush`, `SurfaceBrush`, and `SystemControlTransparentBrush`. These are set as style Setters, not as overridable lightweight styling ThemeResource keys. Only `SimpleCalendarViewHeaderPadding` is exposed as an overridable themed resource.
