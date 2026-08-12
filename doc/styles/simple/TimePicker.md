---
uid: Uno.Themes.Simple.Styles.TimePicker
---

# TimePicker Control

## Styles

| Style Key                                      | IsDefaultStyle\* |
|------------------------------------------------|------------------|
| `SimpleTimePickerStyle`                        |                  |
| `SimpleDefaultTimePickerStyle`                 | True             |
| `SimpleTimePickerFlyoutPresenterStyle`         |                  |
| `SimpleDefaultTimePickerFlyoutPresenterStyle`  | True             |
| `SimpleTimePickerFlyoutButtonStyle`            |                  |

IsDefaultStyle\*: Styles in this column will be set as the default implicit style for the matching control

## Lightweight Styling

Every visual-state brush and dimension is exposed as a semantic `TimePicker*` key in the style's
`ThemeDictionaries`, so an app can override any single key without replacing the template.

### FlyoutButton

| Key                                     | Type              | Value                           |
|-----------------------------------------|-------------------|---------------------------------|
| `TimePickerFlyoutButtonBackground`      | `SolidColorBrush` | `SystemControlTransparentBrush` |
| `TimePickerFlyoutButtonPadding`         | `Thickness`       | `SimpleSpace0Thickness`         |
| `TimePickerFlyoutButtonOpacityPressed`  | `Double`          | `0.8`                           |
| `TimePickerFlyoutButtonOpacityDisabled` | `Double`          | `0.5`                           |

### FlyoutPresenter

| Key                                                   | Type              | Value                          |
|-------------------------------------------------------|-------------------|--------------------------------|
| `TimePickerFlyoutPresenterBackground`                 | `SolidColorBrush` | `SurfaceBrush`                 |
| `TimePickerFlyoutPresenterForeground`                 | `SolidColorBrush` | `OnSurfaceBrush`               |
| `TimePickerFlyoutPresenterBorderBrush`                | `SolidColorBrush` | `OutlineBrush`                 |
| `TimePickerFlyoutPresenterSpacerFill`                 | `SolidColorBrush` | `OutlineBrush`                 |
| `TimePickerFlyoutPresenterHighlightFill`              | `SolidColorBrush` | `SurfaceVariantBrush`          |
| `TimePickerFlyoutPresenterCornerRadius`               | `CornerRadius`    | `SimpleRadius400CornerRadius`  |
| `TimePickerFlyoutPresenterFontFamily`                 | `FontFamily`      | `SimpleFontFamily`             |
| `TimePickerFlyoutPresenterWidth`                      | `Double`          | `242`                          |
| `TimePickerFlyoutPresenterMinWidth`                   | `Double`          | `242`                          |
| `TimePickerFlyoutPresenterMaxHeight`                  | `Double`          | `398`                          |
| `TimePickerFlyoutPresenterHighlightHeight`            | `Double`          | `SimpleIconLarge`              |
| `TimePickerFlyoutPresenterAcceptDismissHostGridHeight`| `Double`          | `52`                           |
| `TimePickerFlyoutPresenterTitleMargin`                | `Thickness`       | `16,12,16,4`                   |
| `TimePickerFlyoutBorderThickness`                     | `Double`          | `SimpleStrokeBorder`           |
| `TimePickerSpacerThemeWidth`                          | `Double`          | `SimpleStrokeBorder`           |

### Field

| Key                                          | Type              | Value                          |
|-----------------------------------------------|-------------------|--------------------------------|
| `TimePickerButtonBackground`                 | `SolidColorBrush` | `SurfaceBrush`                 |
| `TimePickerButtonBackgroundDisabled`         | `SolidColorBrush` | `OnSurfaceDisabledBrush`       |
| `TimePickerButtonBorderBrush`                | `SolidColorBrush` | `OutlineBrush`                 |
| `TimePickerButtonBorderBrushDisabled`        | `SolidColorBrush` | `OutlineDisabledBrush`         |
| `TimePickerCornerRadius`                     | `CornerRadius`    | `SimpleRadius200CornerRadius`  |
| `TimePickerBorderThemeThickness`             | `Thickness`       | `SimpleStrokeBorderThickness`  |
| `TimePickerContentMargin`                    | `Thickness`       | `16,10,16,10`                  |
| `TimePickerMinHeight`                        | `Double`          | `ControlHeightMediumLarge`     |

### Time text / header / dividers

| Key                                          | Type              | Value                          |
|-----------------------------------------------|-------------------|--------------------------------|
| `TimePickerButtonTimeTextForeground`         | `SolidColorBrush` | `OnSurfaceBrush`               |
| `TimePickerButtonTimeTextForegroundDisabled` | `SolidColorBrush` | `OnSurfaceDisabledBrush`       |
| `TimePickerPlaceholderTextForeground`        | `SolidColorBrush` | `OnSurfaceLowBrush`            |
| `TimePickerHeaderForeground`                 | `SolidColorBrush` | `OnSurfaceLowBrush`            |
| `TimePickerHeaderForegroundDisabled`         | `SolidColorBrush` | `OnSurfaceDisabledBrush`       |
| `TimePickerSpacerFill`                       | `SolidColorBrush` | `OnSurfaceVariantBrush`        |
| `TimePickerSpacerFillDisabled`               | `SolidColorBrush` | `OutlineDisabledBrush`         |
| `TimePickerColumnDividerWidth`               | `Double`          | `SimpleStrokeBorder`           |
| `TimePickerColumnDividerMargin`              | `Thickness`       | `2,0,2,0`                      |

> [!NOTE]
> The field sizes to content and is left-aligned, like the Fluent `TimePicker`, with
> `TimePickerFlyoutPresenterMinWidth` as its floor. This also governs the flyout: `TimePickerFlyout` sizes the
> presenter to the target's `ActualWidth` on opening, so a stretched field produces a stretched flyout. Set
> `HorizontalAlignment="Stretch"` on the control to fill its container instead.
>
> The hour / minute / period text stays owned by the control, so culture ordering, `MinuteIncrement` and a
> 24-hour `ClockIdentifier` all still apply. The columns are separated by neutral rules styled with
> `TimePickerColumnDividerWidth`, `TimePickerColumnDividerMargin` and `TimePickerSpacerFill`. They are rules
> rather than a `:` glyph on purpose: the control reorders the hour / minute / period hosts per culture but
> never moves the dividers, so a literal separator is correct only while the hour happens to come first — in a
> period-first culture such as `ko-KR` it would land between the period and the hour.
>
> `TimePicker` only reports the `Normal` / `Disabled` and `HasTime` / `HasNoTime` visual states — it has no
> `PointerOver` or `Pressed` state of its own (unlike `DatePicker`), so there are no `*PointerOver` / `*Pressed`
> brush keys. Pressed and disabled feedback comes from the `TimePickerFlyoutButtonOpacity*` keys.
>
> `Header` renders **only** as the in-field placeholder, the way the Simple `TextBox` shows `PlaceholderText` —
> there is no separate header line above the field, and `HeaderTemplate` is honoured. The placeholder is
> coloured by `TimePickerHeaderForeground`, and by `TimePickerHeaderForegroundDisabled` when the control is
> disabled; `TimePickerPlaceholderTextForeground` dims the control's own hour / minute / period run. With no
> `Header`, that run stays visible as the placeholder instead.

## Example

```xml
<Page.Resources>
    <!-- Tint just the field border, everything else stays on the Simple palette -->
    <SolidColorBrush x:Key="TimePickerButtonBorderBrush" Color="#7C3AED" />
</Page.Resources>

<TimePicker Header="Start Time" ClockIdentifier="24HourClock" />
```

## See also

- [DatePicker](DatePicker.md)
- [Simple Controls Styles](xref:Uno.Themes.Simple.Styles)
