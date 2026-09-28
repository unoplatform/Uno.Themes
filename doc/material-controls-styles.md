---
uid: Uno.Themes.Material.Styles
---

# Material Controls Styles

> [!IMPORTANT]
> UnoFeatures: **Material** — add `<UnoFeatures>Material</UnoFeatures>` to your app's `.csproj` to include Uno Material resources.

These are the style keys exposed by `MaterialTheme` (Material v2). Use the unprefixed [semantic style keys](semantic-styles.md) when sharing XAML with Simple or Fluent; that page records the mappings and differences between themes. Resource overrides are described in [Lightweight Styling](lightweight-styling.md).

| Control                   | Style Key                       | IsDefaultStyle\* |
|---------------------------|---------------------------------|------------------|
| `AppBarButton`            | `AppBarButtonStyle`             | True             |
| `Button`                  | `ElevatedButtonStyle`           |                  |
| `Button`                  | `FilledButtonStyle`             | True             |
| `Button`                  | `FilledTonalButtonStyle`        |                  |
| `Button`                  | `OutlinedButtonStyle`           |                  |
| `Button`                  | `TextButtonStyle`               |                  |
| `Button`                  | `IconButtonStyle`               |                  |
| `Button`                  | `FabStyle`                      |                  |
| `Button`                  | `SurfaceFabStyle`               |                  |
| `Button`                  | `SecondaryFabStyle`             |                  |
| `Button`                  | `TertiaryFabStyle`              |                  |
| `Button`                  | `SmallFabStyle`                 |                  |
| `Button`                  | `SurfaceSmallFabStyle`          |                  |
| `Button`                  | `SecondarySmallFabStyle`        |                  |
| `Button`                  | `TertiarySmallFabStyle`         |                  |
| `Button`                  | `LargeFabStyle`                 |                  |
| `Button`                  | `SurfaceLargeFabStyle`          |                  |
| `Button`                  | `SecondaryLargeFabStyle`        |                  |
| `Button`                  | `TertiaryLargeFabStyle`         |                  |
| `CalendarDatePicker`      | `CalendarDatePickerStyle`       | True             |
| `CalendarView`            | `CalendarViewStyle`             | True             |
| `CheckBox`                | `CheckBoxStyle`                 | True             |
| `ComboBox`                | `ComboBoxStyle`                 | True             |
| `ComboBoxItem`            | `ComboBoxItemStyle`             |                  |
| `CommandBar`              | `CommandBarStyle`               | True             |
| `ContentDialog`           | `ContentDialogStyle`            | True             |
| `DatePicker`              | `DatePickerStyle`               | True             |
| `DatePickerFlyoutPresenter` | `DatePickerFlyoutPresenterStyle` | True            |
| `FlyoutPresenter`         | `FlyoutPresenterStyle`          | True             |
| `HyperlinkButton`         | `HyperlinkButtonStyle`          | True             |
| `HyperlinkButton`         | `SecondaryHyperlinkButtonStyle` |                  |
| `ListView`                | `ListViewStyle`                 | True             |
| `ListViewItem`            | `ListViewItemStyle`             | True             |
| `MediaTransportControls`  | `MediaTransportControlsStyle`   | True             |
| `MenuFlyoutItem`          | `MenuFlyoutItemStyle`           | True             |
| `MenuFlyoutPresenter`     | `MenuFlyoutPresenterStyle`      | True             |
| `MenuFlyoutSeparator`     | `MenuFlyoutSeparatorStyle`      | True             |
| `MenuFlyoutSubItem`       | `MenuFlyoutSubItemStyle`        | True             |
| `muxc:NavigationView`     | `NavigationViewStyle`           | True             |
| `muxc:NavigationViewItem` | `NavigationViewItemStyle`       | True             |
| `muxc:ProgressBar`        | `ProgressBarStyle`              | True             |
| `muxc:ProgressRing`       | `ProgressRingStyle`             | True             |
| `PasswordBox`             | `FilledPasswordBoxStyle`        |                  |
| `PasswordBox`             | `OutlinedPasswordBoxStyle`      | True             |
| `muxc:PipsPager`          | `PipsPagerStyle`                | True             |
| `RadioButton`             | `RadioButtonStyle`              | True             |
| `RadioMenuFlyoutItem`     | `RadioMenuFlyoutItemStyle`      | True             |
| `RatingControl`           | `RatingControlStyle`            | True             |
| `RatingControl`           | `SecondaryRatingControlStyle`   |                  |
| `Uno.Material.Ripple`     | `RippleStyle`                   | True             |
| `Slider`                  | `SliderStyle`                   | True             |
| `TextBlock`               | `DisplayLarge`                  |                  |
| `TextBlock`               | `DisplayMedium`                 |                  |
| `TextBlock`               | `DisplaySmall`                  |                  |
| `TextBlock`               | `HeadlineLarge`                 |                  |
| `TextBlock`               | `HeadlineMedium`                |                  |
| `TextBlock`               | `HeadlineSmall`                 |                  |
| `TextBlock`               | `TitleLarge`                    |                  |
| `TextBlock`               | `TitleMedium`                   |                  |
| `TextBlock`               | `TitleSmall`                    |                  |
| `TextBlock`               | `LabelLarge`                    |                  |
| `TextBlock`               | `LabelMedium`                   |                  |
| `TextBlock`               | `LabelSmall`                    |                  |
| `TextBlock`               | `LabelExtraSmall`               |                  |
| `TextBlock`               | `BodyLarge`                     |                  |
| `TextBlock`               | `BodyMedium`                    | True             |
| `TextBlock`               | `BodySmall`                     |                  |
| `TextBlock`               | `CaptionLarge`                  |                  |
| `TextBlock`               | `CaptionMedium`                 |                  |
| `TextBlock`               | `CaptionSmall`                  |                  |
| `TextBox`                 | `FilledTextBoxStyle`            |                  |
| `TextBox`                 | `OutlinedTextBoxStyle`          | True             |
| `ToggleButton`            | `TextToggleButtonStyle`         |                  |
| `ToggleButton`            | `IconToggleButtonStyle`         | True             |
| `ToggleMenuFlyoutItem`    | `ToggleMenuFlyoutItemStyle`     | True             |
| `ToggleSwitch`            | `ToggleSwitchStyle`             | True             |

IsDefaultStyle\*: Styles in this column will be set as the default implicit style for the matching control

`RippleStyle` and `SecondaryRatingControlStyle` are Material-specific additions. The compatibility key `DefaultMaterialCalendarViewStyle` also resolves to `MaterialDefaultCalendarViewStyle`; prefer `CalendarViewStyle` in new semantic XAML.
