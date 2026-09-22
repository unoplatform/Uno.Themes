---
uid: Uno.Themes.SemanticStyles
---

# Semantic Styles

Uno Themes provides a **semantic style abstraction layer** that lets you write theme-agnostic XAML. Instead of referencing theme-prefixed style keys (e.g. `MaterialFilledButtonStyle` or `SimpleFilledButtonStyle`), you use a single **semantic key** like `FilledButtonStyle` and the active theme resolves it to the correct design-system-specific style at runtime.

```xml
<!-- Works under Material, Simple and Cupertino themes -->
<Button Style="{StaticResource FilledButtonStyle}" Content="Save" />
```

## How It Works

Each theme's `_Resources.xaml` defines `<StaticResource>` aliases that map semantic keys to theme-specific styles:

- **Material**: `FilledButtonStyle` &rarr; `MaterialFilledButtonStyle`
- **Simple**: `FilledButtonStyle` &rarr; `SimpleFilledButtonStyle`
- **Cupertino**: `FilledButtonStyle` &rarr; `CupertinoProminentButtonStyle`

## Control Style Mappings

The following tables show every semantic style key and how it resolves under each theme.

### Button

| Semantic Key             | Material                         | Simple                         | Cupertino                       | Notes                                                                                 |
| ------------------------ | -------------------------------- | ------------------------------ | ------------------------------- | ------------------------------------------------------------------------------------- |
| `FilledButtonStyle`      | `MaterialFilledButtonStyle`      | `SimpleFilledButtonStyle`      | `CupertinoProminentButtonStyle` | Default implicit style for Material and Simple; Cupertino defaults to TextButtonStyle |
| `ElevatedButtonStyle`    | `MaterialElevatedButtonStyle`    | **GAP**                        | `CupertinoGrayButtonStyle`      | Simple has no elevated/shadow variant                                                 |
| `FilledTonalButtonStyle` | `MaterialFilledTonalButtonStyle` | `SimpleFilledTonalButtonStyle` | `CupertinoTintedButtonStyle`    | Simple "Neutral" is closest tonal match                                               |
| `OutlinedButtonStyle`    | `MaterialOutlinedButtonStyle`    | `SimpleFilledTonalButtonStyle` | `CupertinoGrayButtonStyle`      | Same Simple target as FilledTonal                                                     |
| `TextButtonStyle`        | `MaterialTextButtonStyle`        | `SimpleTextButtonStyle`        | `CupertinoPlainButtonStyle`     | Text-only appearance                                                                  |
| `IconButtonStyle`        | `MaterialIconButtonStyle`        | `SimpleIconButtonStyle`        | `CupertinoIconButtonStyle`      | Simple has multiple icon button colors; Primary is default                            |

### Floating Action Button (FAB)

FAB is a Material-specific concept. Under Simple theme, FAB keys resolve to existing icon button styles.

| Semantic Key             | Material                         | Simple                         | Cupertino                                     | Notes                                 |
| ------------------------ | -------------------------------- | ------------------------------ | --------------------------------------------- | ------------------------------------- |
| `FabStyle`               | `MaterialFabStyle`               | `SimpleIconButtonStyle`        | `CupertinoGlassProminentIconButtonStyle`      | Primary icon button as FAB equivalent |
| `SmallFabStyle`          | `MaterialSmallFabStyle`          | `SimpleIconButtonStyle`        | `CupertinoGlassProminentIconButtonStyle`      | Same as FabStyle                      |
| `LargeFabStyle`          | `MaterialLargeFabStyle`          | `SimpleIconButtonStyle`        | `CupertinoLargeGlassProminentIconButtonStyle` | No large variant in Simple            |
| `SecondaryFabStyle`      | `MaterialSecondaryFabStyle`      | `SimpleIconButtonNeutralStyle` | `CupertinoGlassIconButtonStyle`               |                                       |
| `SecondarySmallFabStyle` | `MaterialSecondarySmallFabStyle` | `SimpleIconButtonNeutralStyle` | `CupertinoGlassIconButtonStyle`               |                                       |
| `SecondaryLargeFabStyle` | `MaterialSecondaryLargeFabStyle` | `SimpleIconButtonNeutralStyle` | `CupertinoLargeGlassIconButtonStyle`          |                                       |
| `TertiaryFabStyle`       | `MaterialTertiaryFabStyle`       | `SimpleIconButtonSubtleStyle`  | `CupertinoTintedIconButtonStyle`              |                                       |
| `TertiarySmallFabStyle`  | `MaterialTertiarySmallFabStyle`  | `SimpleIconButtonSubtleStyle`  | `CupertinoTintedIconButtonStyle`              |                                       |
| `TertiaryLargeFabStyle`  | `MaterialTertiaryLargeFabStyle`  | `SimpleIconButtonSubtleStyle`  | `CupertinoLargeTintedIconButtonStyle`         |                                       |
| `SurfaceFabStyle`        | `MaterialSurfaceFabStyle`        | `SimpleIconButtonNeutralStyle` | `CupertinoGrayIconButtonStyle`                |                                       |
| `SurfaceSmallFabStyle`   | `MaterialSurfaceSmallFabStyle`   | `SimpleIconButtonNeutralStyle` | `CupertinoGrayIconButtonStyle`                |                                       |
| `SurfaceLargeFabStyle`   | `MaterialSurfaceLargeFabStyle`   | `SimpleIconButtonNeutralStyle` | `CupertinoLargeGrayIconButtonStyle`           |                                       |

### ToggleButton

| Semantic Key            | Material                        | Simple                        | Cupertino                        | Notes                    |
| ----------------------- | ------------------------------- | ----------------------------- | -------------------------------- | ------------------------ |
| `TextToggleButtonStyle` | `MaterialTextToggleButtonStyle` | `SimpleTextToggleButtonStyle` | `CupertinoTextToggleButtonStyle` | Text content toggle      |
| `IconToggleButtonStyle` | `MaterialIconToggleButtonStyle` | `SimpleIconToggleButtonStyle` | `CupertinoIconToggleButtonStyle` | Compact icon-only toggle |

### TextBox

| Semantic Key           | Material                       | Simple                       | Cupertino                     | Notes                             |
| ---------------------- | ------------------------------ | ---------------------------- | ----------------------------- | --------------------------------- |
| `FilledTextBoxStyle`   | `MaterialFilledTextBoxStyle`   | `SimpleFilledTextBoxStyle`   | `CupertinoFilledTextBoxStyle` | Background fill, no border        |
| `OutlinedTextBoxStyle` | `MaterialOutlinedTextBoxStyle` | `SimpleOutlinedTextBoxStyle` | `CupertinoTextBoxStyle`       | Default implicit style for Simple |

### PasswordBox

| Semantic Key               | Material                           | Simple                           | Cupertino                         | Notes                             |
| -------------------------- | ---------------------------------- | -------------------------------- | --------------------------------- | --------------------------------- |
| `FilledPasswordBoxStyle`   | `MaterialFilledPasswordBoxStyle`   | `SimpleFilledPasswordBoxStyle`   | `CupertinoFilledPasswordBoxStyle` | Background fill with border       |
| `OutlinedPasswordBoxStyle` | `MaterialOutlinedPasswordBoxStyle` | `SimpleOutlinedPasswordBoxStyle` | `CupertinoPasswordBoxStyle`       | Default implicit style for Simple |

### HyperlinkButton

| Semantic Key                    | Material                                | Simple                                | Cupertino                                | Notes                     |
| ------------------------------- | --------------------------------------- | ------------------------------------- | ---------------------------------------- | ------------------------- |
| `HyperlinkButtonStyle`          | `MaterialHyperlinkButtonStyle`          | `SimpleHyperlinkButtonStyle`          | `CupertinoHyperlinkButtonStyle`          | Primary underlined link   |
| `SecondaryHyperlinkButtonStyle` | `MaterialSecondaryHyperlinkButtonStyle` | `SimpleSecondaryHyperlinkButtonStyle` | `CupertinoSecondaryHyperlinkButtonStyle` | Secondary underlined link |

### ComboBox

| Semantic Key        | Material                    | Simple                    | Cupertino                    | Notes        |
| ------------------- | --------------------------- | ------------------------- | ---------------------------- | ------------ |
| `ComboBoxStyle`     | `MaterialComboBoxStyle`     | `SimpleComboBoxStyle`     | `CupertinoComboBoxStyle`     | Direct match |
| `ComboBoxItemStyle` | `MaterialComboBoxItemStyle` | `SimpleComboBoxItemStyle` | `CupertinoComboBoxItemStyle` | Direct match |

### CheckBox

| Semantic Key    | Material                | Simple                | Cupertino                | Notes        |
| --------------- | ----------------------- | --------------------- | ------------------------ | ------------ |
| `CheckBoxStyle` | `MaterialCheckBoxStyle` | `SimpleCheckBoxStyle` | `CupertinoCheckBoxStyle` | Direct match |

### RadioButton

| Semantic Key       | Material                   | Simple                   | Cupertino                   | Notes        |
| ------------------ | -------------------------- | ------------------------ | --------------------------- | ------------ |
| `RadioButtonStyle` | `MaterialRadioButtonStyle` | `SimpleRadioButtonStyle` | `CupertinoRadioButtonStyle` | Direct match |

### ToggleSwitch

| Semantic Key        | Material                    | Simple                    | Cupertino                    | Notes        |
| ------------------- | --------------------------- | ------------------------- | ---------------------------- | ------------ |
| `ToggleSwitchStyle` | `MaterialToggleSwitchStyle` | `SimpleToggleSwitchStyle` | `CupertinoToggleSwitchStyle` | Direct match |

### Slider

| Semantic Key  | Material              | Simple              | Cupertino              | Notes        |
| ------------- | --------------------- | ------------------- | ---------------------- | ------------ |
| `SliderStyle` | `MaterialSliderStyle` | `SimpleSliderStyle` | `CupertinoSliderStyle` | Direct match |

### ProgressBar

| Semantic Key       | Material                   | Simple                   | Cupertino                   | Notes                |
| ------------------ | -------------------------- | ------------------------ | --------------------------- | -------------------- |
| `ProgressBarStyle` | `MaterialProgressBarStyle` | `SimpleProgressBarStyle` | `CupertinoProgressBarStyle` | Horizontal indicator |

### ProgressRing

| Semantic Key        | Material                    | Simple                    | Cupertino                    | Notes              |
| ------------------- | --------------------------- | ------------------------- | ---------------------------- | ------------------ |
| `ProgressRingStyle` | `MaterialProgressRingStyle` | `SimpleProgressRingStyle` | `CupertinoProgressRingStyle` | Circular indicator |

### ListView

| Semantic Key        | Material                    | Simple                    | Cupertino                    | Notes        |
| ------------------- | --------------------------- | ------------------------- | ---------------------------- | ------------ |
| `ListViewStyle`     | `MaterialListViewStyle`     | `SimpleListViewStyle`     | `CupertinoListViewStyle`     | Direct match |
| `ListViewItemStyle` | `MaterialListViewItemStyle` | `SimpleListViewItemStyle` | `CupertinoListViewItemStyle` | Direct match |

### ContentDialog

| Semantic Key         | Material                     | Simple                     | Cupertino                     | Notes        |
| -------------------- | ---------------------------- | -------------------------- | ----------------------------- | ------------ |
| `ContentDialogStyle` | `MaterialContentDialogStyle` | `SimpleContentDialogStyle` | `CupertinoContentDialogStyle` | Direct match |

### CommandBar

| Semantic Key      | Material                  | Simple  | Cupertino                  | Notes                          |
| ----------------- | ------------------------- | ------- | -------------------------- | ------------------------------ |
| `CommandBarStyle` | `MaterialCommandBarStyle` | **GAP** | `CupertinoCommandBarStyle` | Simple has no CommandBar style |

### AppBarButton

| Semantic Key        | Material                    | Simple                    | Cupertino                    | Notes        |
| ------------------- | --------------------------- | ------------------------- | ---------------------------- | ------------ |
| `AppBarButtonStyle` | `MaterialAppBarButtonStyle` | `SimpleAppBarButtonStyle` | `CupertinoAppBarButtonStyle` | Direct match |

### NavigationView

| Semantic Key              | Material                          | Simple                          | Cupertino                          | Notes |
| ------------------------- | --------------------------------- | ------------------------------- | ---------------------------------- | ----- |
| `NavigationViewStyle`     | `MaterialNavigationViewStyle`     | `SimpleNavigationViewStyle`     | `CupertinoNavigationViewStyle`     |       |
| `NavigationViewItemStyle` | `MaterialNavigationViewItemStyle` | `SimpleNavigationViewItemStyle` | `CupertinoNavigationViewItemStyle` |       |

### CalendarView

| Semantic Key        | Material                    | Simple                    | Cupertino                    | Notes        |
| ------------------- | --------------------------- | ------------------------- | ---------------------------- | ------------ |
| `CalendarViewStyle` | `MaterialCalendarViewStyle` | `SimpleCalendarViewStyle` | `CupertinoCalendarViewStyle` | Direct match |

### CalendarDatePicker

| Semantic Key              | Material                          | Simple                          | Cupertino                          | Notes        |
| ------------------------- | --------------------------------- | ------------------------------- | ---------------------------------- | ------------ |
| `CalendarDatePickerStyle` | `MaterialCalendarDatePickerStyle` | `SimpleCalendarDatePickerStyle` | `CupertinoCalendarDatePickerStyle` | Direct match |

### DatePicker

| Semantic Key      | Material                  | Simple                  | Cupertino                  | Notes        |
| ----------------- | ------------------------- | ----------------------- | -------------------------- | ------------ |
| `DatePickerStyle` | `MaterialDatePickerStyle` | `SimpleDatePickerStyle` | `CupertinoDatePickerStyle` | Direct match |

### MediaPlayerElement

| Semantic Key                  | Material                              | Simple  | Cupertino | Notes                                    |
| ----------------------------- | ------------------------------------- | ------- | --------- | ---------------------------------------- |
| `MediaTransportControlsStyle` | `MaterialMediaTransportControlsStyle` | **GAP** | **GAP**   | Simple and Cupertino have no media style |

### PipsPager

| Semantic Key     | Material                 | Simple                 | Cupertino                 | Notes           |
| ---------------- | ------------------------ | ---------------------- | ------------------------- | --------------- |
| `PipsPagerStyle` | `MaterialPipsPagerStyle` | `SimplePipsPagerStyle` | `CupertinoPipsPagerStyle` | Pagination dots |

### RatingControl

| Semantic Key         | Material                     | Simple                     | Cupertino                     | Notes                         |
| -------------------- | ---------------------------- | -------------------------- | ----------------------------- | ----------------------------- |
| `RatingControlStyle` | `MaterialRatingControlStyle` | `SimpleRatingControlStyle` | `CupertinoRatingControlStyle` | Star rating with brand colors |

### Flyout / MenuFlyout

| Semantic Key                | Material                            | Simple                            | Cupertino                            | Notes                      |
| --------------------------- | ----------------------------------- | --------------------------------- | ------------------------------------ | -------------------------- |
| `FlyoutPresenterStyle`      | `MaterialFlyoutPresenterStyle`      | `SimpleFlyoutPresenterStyle`      | `CupertinoFlyoutPresenterStyle`      |                            |
| `MenuFlyoutPresenterStyle`  | `MaterialMenuFlyoutPresenterStyle`  | `SimpleMenuFlyoutPresenterStyle`  | `CupertinoMenuFlyoutPresenterStyle`  | Direct match               |
| `MenuFlyoutItemStyle`       | `MaterialMenuFlyoutItemStyle`       | `SimpleMenuFlyoutItemStyle`       | `CupertinoMenuFlyoutItemStyle`       | Direct match               |
| `MenuFlyoutSeparatorStyle`  | `MaterialMenuFlyoutSeparatorStyle`  | `SimpleMenuFlyoutSeparatorStyle`  | `CupertinoMenuFlyoutSeparatorStyle`  | Direct match               |
| `MenuFlyoutSubItemStyle`    | `MaterialMenuFlyoutSubItemStyle`    | `SimpleMenuFlyoutSubItemStyle`    | `CupertinoMenuFlyoutSubItemStyle`    | Direct match               |
| `ToggleMenuFlyoutItemStyle` | `MaterialToggleMenuFlyoutItemStyle` | `SimpleToggleMenuFlyoutItemStyle` | `CupertinoToggleMenuFlyoutItemStyle` | Direct match               |
| `RadioMenuFlyoutItemStyle`  | `MaterialRadioMenuFlyoutItemStyle`  | `SimpleRadioMenuFlyoutItemStyle`  | `CupertinoRadioMenuFlyoutItemStyle`  | Cupertino uses a checkmark |

## Typography

All three themes provide the same semantic typography keys. Material and Simple use the Material Design 3 scale; Cupertino maps them to its Apple-inspired text styles.

Every `*FontFamily` key derives from the single `DefaultFontFamily` root token, so overriding that one key swaps the typeface across the whole type scale; per-scale weight nuance is carried by the `*FontWeight` tokens. See [Design Tokens - Typography](design-tokens.md#typography).

| Semantic Style Key | Font Resource Keys                                                                                                     |
| ------------------ | ---------------------------------------------------------------------------------------------------------------------- |
| `DisplayLarge`     | `DisplayLargeFontFamily`, `DisplayLargeFontSize`, `DisplayLargeFontWeight`, `DisplayLargeCharacterSpacing`             |
| `DisplayMedium`    | `DisplayMediumFontFamily`, `DisplayMediumFontSize`, `DisplayMediumFontWeight`                                          |
| `DisplaySmall`     | `DisplaySmallFontFamily`, `DisplaySmallFontSize`, `DisplaySmallFontWeight`                                             |
| `HeadlineLarge`    | `HeadlineLargeFontFamily`, `HeadlineLargeFontSize`, `HeadlineLargeFontWeight`                                          |
| `HeadlineMedium`   | `HeadlineMediumFontFamily`, `HeadlineMediumFontSize`, `HeadlineMediumFontWeight`                                       |
| `HeadlineSmall`    | `HeadlineSmallFontFamily`, `HeadlineSmallFontSize`, `HeadlineSmallFontWeight`                                          |
| `TitleLarge`       | `TitleLargeFontFamily`, `TitleLargeFontSize`, `TitleLargeFontWeight`                                                   |
| `TitleMedium`      | `TitleMediumFontFamily`, `TitleMediumFontSize`, `TitleMediumFontWeight`                                                |
| `TitleSmall`       | `TitleSmallFontFamily`, `TitleSmallFontSize`, `TitleSmallFontWeight`                                                   |
| `BodyLarge`        | `BodyLargeFontFamily`, `BodyLargeFontSize`, `BodyLargeFontWeight`, `BodyLargeCharacterSpacing`                         |
| `BodyMedium`       | `BodyMediumFontFamily`, `BodyMediumFontSize`, `BodyMediumFontWeight`, `BodyMediumCharacterSpacing`                     |
| `BodySmall`        | `BodySmallFontFamily`, `BodySmallFontSize`, `BodySmallFontWeight`, `BodySmallCharacterSpacing`                         |
| `LabelLarge`       | `LabelLargeFontFamily`, `LabelLargeFontSize`, `LabelLargeFontWeight`, `LabelLargeCharacterSpacing`                     |
| `LabelMedium`      | `LabelMediumFontFamily`, `LabelMediumFontSize`, `LabelMediumFontWeight`, `LabelMediumCharacterSpacing`                 |
| `LabelSmall`       | `LabelSmallFontFamily`, `LabelSmallFontSize`, `LabelSmallFontWeight`, `LabelSmallCharacterSpacing`                     |
| `LabelExtraSmall`  | `LabelExtraSmallFontFamily`, `LabelExtraSmallFontSize`, `LabelExtraSmallFontWeight`, `LabelExtraSmallCharacterSpacing` |
| `CaptionLarge`     | `CaptionLargeFontFamily`, `CaptionLargeFontSize`, `CaptionLargeFontWeight`, `CaptionLargeCharacterSpacing`             |
| `CaptionMedium`    | `CaptionMediumFontFamily`, `CaptionMediumFontSize`, `CaptionMediumFontWeight`, `CaptionMediumCharacterSpacing`         |
| `CaptionSmall`     | `CaptionSmallFontFamily`, `CaptionSmallFontSize`, `CaptionSmallFontWeight`, `CaptionSmallCharacterSpacing`             |

Each theme's `Typography.xaml` provides the concrete values for these keys. The font resource keys (e.g. `BodyLargeFontSize`) are the same across themes and can be used directly for lightweight styling overrides.

## Lightweight Styling Portability

Semantic style keys also enable portable [lightweight styling](lightweight-styling.md). Material and Simple expose shared resource keys for customizing control appearance:

```xml
<!-- This override works under both Material and Simple themes -->
<SolidColorBrush x:Key="FilledButtonForeground" Color="Red" />
```

Both **Material** and **Simple** templates reference the same unprefixed keys (e.g. `FilledButtonForeground`) directly. Cupertino shares the semantic style keys and palette roles, but its control-specific override keys differ; see [Cupertino control styles](cupertino-controls-styles.md).

The color palette underneath these keys can itself be swapped wholesale — generated from a single seed color, and even changed at runtime — see [Seed Color Palette](seed-colors.md).

For more details on per-control lightweight styling resources, see the individual control style pages:

- [Button](styles/Button.md)
- [TextBox](styles/TextBox.md)
- [PasswordBox](styles/PasswordBox.md)
- [CheckBox](styles/CheckBox.md)
- [RadioButton](styles/RadioButton.md)
- [ToggleSwitch](styles/ToggleSwitch.md)
- [Slider](styles/Slider.md)
- [ToggleButton](styles/ToggleButton.md)
