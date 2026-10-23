---
uid: Uno.Themes.Simple.Styles.CalendarView
---

# CalendarView Control

<!-- BEGIN GENERATED -->
<!-- This region is generated from the Styles XAML by `dotnet run build/scripts/GenerateStyleDocs.cs`; do not edit it by hand. -->

## Styles

| Style Key                        | Semantic Alias      | IsDefaultStyle\* |
| -------------------------------- | ------------------- | ---------------- |
| `SimpleCalendarViewStyle`        | `CalendarViewStyle` | True             |
| `SimpleDefaultCalendarViewStyle` |                     | True             |

IsDefaultStyle\*: Styles in this column will be set as the default implicit style for the matching control

## Lightweight Styling

### Theme-agnostic

| Key                       | Type     | Value                |
| ------------------------- | -------- | -------------------- |
| `SimpleDownArrowPathData` | `String` | M0,0L32,0 16,19.745z |

### Themed

| Key                               | Type        | Light    |
| --------------------------------- | ----------- | -------- |
| `SimpleCalendarViewHeaderPadding` | `Thickness` | 12,0,0,0 |

The Dark theme uses the same values as Light.

<!-- END GENERATED -->

> [!NOTE]
> The CalendarView style sets most visual properties (borders, foregrounds, backgrounds, typography) directly via CalendarView-specific dependency properties (e.g., `FocusBorderBrush`, `SelectedForeground`, `CalendarItemBackground`, `TodayForeground`, etc.) using shared theme brushes like `PrimaryBrush`, `OnSurfaceBrush`, `OutlineBrush`, `OnPrimaryBrush`, `OnSurfaceDisabledBrush`, `SurfaceBrush`, and `SystemControlTransparentBrush`. These are set as style Setters, not as overridable lightweight styling ThemeResource keys. Only `SimpleCalendarViewHeaderPadding` is exposed as an overridable themed resource.
