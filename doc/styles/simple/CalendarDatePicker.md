---
uid: Uno.Themes.Simple.Styles.CalendarDatePicker
---

# CalendarDatePicker Control

## Styles

| Style Key                                  | IsDefaultStyle\* |
|--------------------------------------------|------------------|
| `SimpleCalendarDatePickerStyle`            |                  |
| `SimpleDefaultCalendarDatePickerStyle`     | True             |

IsDefaultStyle\*: Styles in this column will be set as the default implicit style for the matching control

## Lightweight Styling

### Themed Resources (ThemeDictionaries)

#### Semantic Keys (source of truth)

| Key                                                | Type              | Value                           |
|----------------------------------------------------|-------------------|---------------------------------|
| `CalendarDatePickerCornerRadius`                   | `StaticResource`  | `SimpleRadius200CornerRadius`   |
| `CalendarDatePickerFlyoutPresenterCornerRadius`    | `StaticResource`  | `SimpleRadius400CornerRadius`   |
| `CalendarDatePickerBorderThemeThickness`           | `StaticResource`  | `SimpleStrokeBorderThickness`   |
| `CalendarDatePickerFlyoutPresenterPadding`         | `StaticResource`  | `SimpleSpace0Thickness`         |
| `CalendarDatePickerContentMargin`                  | `Thickness`       | 12,0,12,0 (Light) / 16,10,16,10 (Dark) |
| `CalendarDatePickerHeight`                         | `StaticResource`  | `SimpleIconLarge` (Light only)  |
| `CalendarDatePickerMinHeight`                      | `StaticResource`  | `SimpleIconLarge` (Light) / 44 (Dark) |
| `CalendarDatePickerHeaderMargin`                   | `Thickness`       | 0,0,0,8                         |
| `CalendarDatePickerCalendarGlyphMargin`            | `Thickness`       | 12,0,0,0                        |
| `CalendarDatePickerForeground`                     | `SolidColorBrush` | `OnSurfaceBrush`                |
| `CalendarDatePickerForegroundDisabled`             | `SolidColorBrush` | `OnSurfaceDisabledBrush`        |
| `CalendarDatePickerTextForeground`                 | `SolidColorBrush` | `OnSurfaceLowBrush`             |
| `CalendarDatePickerTextForegroundDisabled`         | `SolidColorBrush` | `OnSurfaceDisabledBrush`        |
| `CalendarDatePickerTextForegroundSelected`         | `SolidColorBrush` | `OnSurfaceBrush`                |
| `CalendarDatePickerCalendarGlyphForeground`        | `SolidColorBrush` | `OnSurfaceBrush`                |
| `CalendarDatePickerCalendarGlyphForegroundDisabled`| `SolidColorBrush` | `OnSurfaceDisabledBrush`        |
| `CalendarDatePickerHeaderForeground`               | `SolidColorBrush` | `OnSurfaceBrush`                |
| `CalendarDatePickerHeaderForegroundDisabled`       | `SolidColorBrush` | `OnSurfaceDisabledBrush`        |
| `CalendarDatePickerBackground`                     | `SolidColorBrush` | `SurfaceBrush`                  |
| `CalendarDatePickerBackgroundPointerOver`          | `SolidColorBrush` | `SurfaceBrush`                  |
| `CalendarDatePickerBackgroundPressed`              | `SolidColorBrush` | `SurfaceBrush`                  |
| `CalendarDatePickerBackgroundDisabled`             | `SolidColorBrush` | `OnSurfaceDisabledBrush`        |
| `CalendarDatePickerBackgroundFocused`              | `SolidColorBrush` | `SurfaceBrush`                  |
| `CalendarDatePickerBorderBrush`                    | `SolidColorBrush` | `OutlineBrush`                  |
| `CalendarDatePickerBorderBrushPointerOver`         | `SolidColorBrush` | `OutlineBrush`                  |
| `CalendarDatePickerBorderBrushPressed`             | `SolidColorBrush` | `OutlineBrush`                  |
| `CalendarDatePickerBorderBrushDisabled`            | `SolidColorBrush` | `OutlineDisabledBrush`          |
| `CalendarDatePickerBorderBrushFocused`             | `SolidColorBrush` | `PrimaryMediumBrush`            |

#### Backwards-Compat Aliases

| Key                                                     | Type             | Value                                           |
|---------------------------------------------------------|------------------|-------------------------------------------------|
| `SimpleCalendarDatePickerCornerRadius`                  | `StaticResource` | `CalendarDatePickerCornerRadius`                |
| `SimpleCalendarDatePickerFlyoutPresenterCornerRadius`   | `StaticResource` | `CalendarDatePickerFlyoutPresenterCornerRadius` |
| `SimpleCalendarDatePickerBorderThemeThickness`          | `StaticResource` | `CalendarDatePickerBorderThemeThickness`        |
| `SimpleCalendarDatePickerFlyoutPresenterPadding`        | `StaticResource` | `CalendarDatePickerFlyoutPresenterPadding`      |
| `SimpleCalendarDatePickerMinHeight`                     | `StaticResource` | `CalendarDatePickerMinHeight`                   |
