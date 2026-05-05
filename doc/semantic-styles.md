---
uid: Uno.Themes.SemanticStyles
---

# Semantic Styles

Uno Themes provides a **semantic style abstraction layer** that lets you write theme-agnostic XAML. Instead of referencing theme-prefixed style keys (e.g. `MaterialFilledButtonStyle` or `SimpleFilledButtonStyle`), you use a single **semantic key** like `FilledButtonStyle` and the active theme resolves it to the correct design-system-specific style at runtime.

```xml
<!-- Works under both Material and Simple themes -->
<Button Style="{StaticResource FilledButtonStyle}" Content="Save" />
```

## How It Works

Each theme's `_Resources.xaml` defines `<StaticResource>` aliases that map semantic keys to theme-specific styles:

- **Material**: `FilledButtonStyle` &rarr; `MaterialFilledButtonStyle`
- **Simple**: `FilledButtonStyle` &rarr; `SimpleFilledButtonStyle`

## Control Style Mappings

The following tables show every semantic style key and how it resolves under each theme.

### Button

| Semantic Key | Material | Simple | Notes |
|---|---|---|---|
| `FilledButtonStyle` | `MaterialFilledButtonStyle` | `SimpleFilledButtonStyle` | Default implicit style for both themes |
| `ElevatedButtonStyle` | `MaterialElevatedButtonStyle` | **GAP** | Simple has no elevated/shadow variant |
| `FilledTonalButtonStyle` | `MaterialFilledTonalButtonStyle` | `SimpleFilledTonalButtonStyle` | Simple "Neutral" is closest tonal match |
| `OutlinedButtonStyle` | `MaterialOutlinedButtonStyle` | `SimpleFilledTonalButtonStyle` | Same Simple target as FilledTonal |
| `TextButtonStyle` | `MaterialTextButtonStyle` | `SimpleTextButtonStyle` | Text-only appearance |
| `IconButtonStyle` | `MaterialIconButtonStyle` | `SimpleIconButtonStyle` | Simple has multiple icon button colors; Primary is default |

### Floating Action Button (FAB)

FAB is a Material-specific concept. Under Simple theme, FAB keys resolve to existing icon button styles.

| Semantic Key | Material | Simple | Notes |
|---|---|---|---|
| `FabStyle` | `MaterialFabStyle` | `SimpleIconButtonStyle` | Primary icon button as FAB equivalent |
| `SmallFabStyle` | `MaterialSmallFabStyle` | `SimpleIconButtonStyle` | Same as FabStyle |
| `LargeFabStyle` | `MaterialLargeFabStyle` | `SimpleIconButtonStyle` | No large variant in Simple |
| `SecondaryFabStyle` | `MaterialSecondaryFabStyle` | `SimpleIconButtonNeutralStyle` | |
| `SecondarySmallFabStyle` | `MaterialSecondarySmallFabStyle` | `SimpleIconButtonNeutralStyle` | |
| `SecondaryLargeFabStyle` | `MaterialSecondaryLargeFabStyle` | `SimpleIconButtonNeutralStyle` | |
| `TertiaryFabStyle` | `MaterialTertiaryFabStyle` | `SimpleIconButtonSubtleStyle` | |
| `TertiarySmallFabStyle` | `MaterialTertiarySmallFabStyle` | `SimpleIconButtonSubtleStyle` | |
| `TertiaryLargeFabStyle` | `MaterialTertiaryLargeFabStyle` | `SimpleIconButtonSubtleStyle` | |
| `SurfaceFabStyle` | `MaterialSurfaceFabStyle` | `SimpleIconButtonNeutralStyle` | |
| `SurfaceSmallFabStyle` | `MaterialSurfaceSmallFabStyle` | `SimpleIconButtonNeutralStyle` | |
| `SurfaceLargeFabStyle` | `MaterialSurfaceLargeFabStyle` | `SimpleIconButtonNeutralStyle` | |

### ToggleButton

| Semantic Key | Material | Simple | Notes |
|---|---|---|---|
| `TextToggleButtonStyle` | `MaterialTextToggleButtonStyle` | `SimpleTextToggleButtonStyle` | Text content toggle |
| `IconToggleButtonStyle` | `MaterialIconToggleButtonStyle` | `SimpleIconToggleButtonStyle` | Compact icon-only toggle |

### TextBox

| Semantic Key | Material | Simple | Notes |
|---|---|---|---|
| `FilledTextBoxStyle` | `MaterialFilledTextBoxStyle` | `SimpleFilledTextBoxStyle` | Background fill, no border |
| `OutlinedTextBoxStyle` | `MaterialOutlinedTextBoxStyle` | `SimpleOutlinedTextBoxStyle` | Default implicit style for Simple |

### PasswordBox

| Semantic Key | Material | Simple | Notes |
|---|---|---|---|
| `FilledPasswordBoxStyle` | `MaterialFilledPasswordBoxStyle` | `SimpleFilledPasswordBoxStyle` | Background fill with border |
| `OutlinedPasswordBoxStyle` | `MaterialOutlinedPasswordBoxStyle` | `SimpleOutlinedPasswordBoxStyle` | Default implicit style for Simple |

### HyperlinkButton

| Semantic Key | Material | Simple | Notes |
|---|---|---|---|
| `HyperlinkButtonStyle` | `MaterialHyperlinkButtonStyle` | `SimpleHyperlinkButtonStyle` | Primary underlined link |
| `SecondaryHyperlinkButtonStyle` | `MaterialSecondaryHyperlinkButtonStyle` | `SimpleSecondaryHyperlinkButtonStyle` | Secondary underlined link |

### ComboBox

| Semantic Key | Material | Simple | Notes |
|---|---|---|---|
| `ComboBoxStyle` | `MaterialComboBoxStyle` | `SimpleComboBoxStyle` | Direct match |
| `ComboBoxItemStyle` | `MaterialComboBoxItemStyle` | `SimpleComboBoxItemStyle` | Direct match |

### CheckBox

| Semantic Key | Material | Simple | Notes |
|---|---|---|---|
| `CheckBoxStyle` | `MaterialCheckBoxStyle` | `SimpleCheckBoxStyle` | Direct match |

### RadioButton

| Semantic Key | Material | Simple | Notes |
|---|---|---|---|
| `RadioButtonStyle` | `MaterialRadioButtonStyle` | `SimpleRadioButtonStyle` | Direct match |

### ToggleSwitch

| Semantic Key | Material | Simple | Notes |
|---|---|---|---|
| `ToggleSwitchStyle` | `MaterialToggleSwitchStyle` | `SimpleToggleSwitchStyle` | Direct match |

### Slider

| Semantic Key | Material | Simple | Notes |
|---|---|---|---|
| `SliderStyle` | `MaterialSliderStyle` | `SimpleSliderStyle` | Direct match |

### ProgressBar

| Semantic Key | Material | Simple | Notes |
|---|---|---|---|
| `ProgressBarStyle` | `MaterialProgressBarStyle` | `SimpleProgressBarStyle` | Horizontal indicator |

### ProgressRing

| Semantic Key | Material | Simple | Notes |
|---|---|---|---|
| `ProgressRingStyle` | `MaterialProgressRingStyle` | `SimpleProgressRingStyle` | Circular indicator |

### ListView

| Semantic Key | Material | Simple | Notes |
|---|---|---|---|
| `ListViewStyle` | `MaterialListViewStyle` | `SimpleListViewStyle` | Direct match |
| `ListViewItemStyle` | `MaterialListViewItemStyle` | `SimpleListViewItemStyle` | Direct match |

### ContentDialog

| Semantic Key | Material | Simple | Notes |
|---|---|---|---|
| `ContentDialogStyle` | `MaterialContentDialogStyle` | `SimpleContentDialogStyle` | Direct match |

### CommandBar

| Semantic Key | Material | Simple | Notes |
|---|---|---|---|
| `CommandBarStyle` | `MaterialCommandBarStyle` | **GAP** | Simple has no CommandBar style |

### AppBarButton

| Semantic Key | Material | Simple | Notes |
|---|---|---|---|
| `AppBarButtonStyle` | `MaterialAppBarButtonStyle` | `SimpleAppBarButtonStyle` | Direct match |

### NavigationView

| Semantic Key | Material | Simple | Notes |
|---|---|---|---|
| `NavigationViewStyle` | `MaterialNavigationViewStyle` | `SimpleNavigationViewStyle` | |
| `NavigationViewItemStyle` | `MaterialNavigationViewItemStyle` | `SimpleNavigationViewItemStyle` | |

### CalendarView

| Semantic Key | Material | Simple | Notes |
|---|---|---|---|
| `CalendarViewStyle` | `MaterialCalendarViewStyle` | `SimpleCalendarViewStyle` | Direct match |

### CalendarDatePicker

| Semantic Key | Material | Simple | Notes |
|---|---|---|---|
| `CalendarDatePickerStyle` | `MaterialCalendarDatePickerStyle` | `SimpleCalendarDatePickerStyle` | Direct match |

### DatePicker

| Semantic Key | Material | Simple | Notes |
|---|---|---|---|
| `DatePickerStyle` | `MaterialDatePickerStyle` | `SimpleDatePickerStyle` | Direct match |

### MediaPlayerElement

| Semantic Key | Material | Simple | Notes |
|---|---|---|---|
| `MediaTransportControlsStyle` | `MaterialMediaTransportControlsStyle` | **GAP** | Simple has no media style |

### PipsPager

| Semantic Key | Material | Simple | Notes |
|---|---|---|---|
| `PipsPagerStyle` | `MaterialPipsPagerStyle` | `SimplePipsPagerStyle` | Pagination dots |

### RatingControl

| Semantic Key | Material | Simple | Notes |
|---|---|---|---|
| `RatingControlStyle` | `MaterialRatingControlStyle` | `SimpleRatingControlStyle` | Star rating with brand colors |

### Flyout / MenuFlyout

| Semantic Key | Material | Simple | Notes |
|---|---|---|---|
| `FlyoutPresenterStyle` | `MaterialFlyoutPresenterStyle` | `SimpleFlyoutPresenterStyle` | |
| `MenuFlyoutPresenterStyle` | `MaterialMenuFlyoutPresenterStyle` | `SimpleMenuFlyoutPresenterStyle` | Direct match |
| `MenuFlyoutItemStyle` | `MaterialMenuFlyoutItemStyle` | `SimpleMenuFlyoutItemStyle` | Direct match |
| `MenuFlyoutSeparatorStyle` | `MaterialMenuFlyoutSeparatorStyle` | `SimpleMenuFlyoutSeparatorStyle` | Direct match |
| `MenuFlyoutSubItemStyle` | `MaterialMenuFlyoutSubItemStyle` | `SimpleMenuFlyoutSubItemStyle` | Direct match |
| `ToggleMenuFlyoutItemStyle` | `MaterialToggleMenuFlyoutItemStyle` | `SimpleToggleMenuFlyoutItemStyle` | Direct match |
| `RadioMenuFlyoutItemStyle` | `MaterialRadioMenuFlyoutItemStyle` | `SimpleRadioMenuFlyoutItemStyle` | Radio bullet indicator |

## Typography

Both themes provide identical semantic typography keys based on the Material Design 3 type scale.

| Semantic Style Key | Font Resource Keys |
|---|---|
| `DisplayLarge` | `DisplayLargeFontFamily`, `DisplayLargeFontSize`, `DisplayLargeFontWeight`, `DisplayLargeCharacterSpacing` |
| `DisplayMedium` | `DisplayMediumFontFamily`, `DisplayMediumFontSize`, `DisplayMediumFontWeight` |
| `DisplaySmall` | `DisplaySmallFontFamily`, `DisplaySmallFontSize`, `DisplaySmallFontWeight` |
| `HeadlineLarge` | `HeadlineLargeFontFamily`, `HeadlineLargeFontSize`, `HeadlineLargeFontWeight` |
| `HeadlineMedium` | `HeadlineMediumFontFamily`, `HeadlineMediumFontSize`, `HeadlineMediumFontWeight` |
| `HeadlineSmall` | `HeadlineSmallFontFamily`, `HeadlineSmallFontSize`, `HeadlineSmallFontWeight` |
| `TitleLarge` | `TitleLargeFontFamily`, `TitleLargeFontSize`, `TitleLargeFontWeight` |
| `TitleMedium` | `TitleMediumFontFamily`, `TitleMediumFontSize`, `TitleMediumFontWeight` |
| `TitleSmall` | `TitleSmallFontFamily`, `TitleSmallFontSize`, `TitleSmallFontWeight` |
| `BodyLarge` | `BodyLargeFontFamily`, `BodyLargeFontSize`, `BodyLargeFontWeight`, `BodyLargeCharacterSpacing` |
| `BodyMedium` | `BodyMediumFontFamily`, `BodyMediumFontSize`, `BodyMediumFontWeight`, `BodyMediumCharacterSpacing` |
| `BodySmall` | `BodySmallFontFamily`, `BodySmallFontSize`, `BodySmallFontWeight`, `BodySmallCharacterSpacing` |
| `LabelLarge` | `LabelLargeFontFamily`, `LabelLargeFontSize`, `LabelLargeFontWeight`, `LabelLargeCharacterSpacing` |
| `LabelMedium` | `LabelMediumFontFamily`, `LabelMediumFontSize`, `LabelMediumFontWeight`, `LabelMediumCharacterSpacing` |
| `LabelSmall` | `LabelSmallFontFamily`, `LabelSmallFontSize`, `LabelSmallFontWeight`, `LabelSmallCharacterSpacing` |
| `LabelExtraSmall` | `LabelExtraSmallFontFamily`, `LabelExtraSmallFontSize`, `LabelExtraSmallFontWeight`, `LabelExtraSmallCharacterSpacing` |
| `CaptionLarge` | `CaptionLargeFontFamily`, `CaptionLargeFontSize`, `CaptionLargeFontWeight`, `CaptionLargeCharacterSpacing` |
| `CaptionMedium` | `CaptionMediumFontFamily`, `CaptionMediumFontSize`, `CaptionMediumFontWeight`, `CaptionMediumCharacterSpacing` |
| `CaptionSmall` | `CaptionSmallFontFamily`, `CaptionSmallFontSize`, `CaptionSmallFontWeight`, `CaptionSmallCharacterSpacing` |

Each theme's `Typography.xaml` provides the concrete values for these keys. The font resource keys (e.g. `BodyLargeFontSize`) are the same across themes and can be used directly for lightweight styling overrides.

## Lightweight Styling Portability

Semantic style keys also enable portable [lightweight styling](lightweight-styling.md). Both themes expose semantic resource keys for customizing control appearance:

```xml
<!-- This override works under both Material and Simple themes -->
<SolidColorBrush x:Key="FilledButtonForeground" Color="Red" />
```

Both **Material** and **Simple** templates reference the same unprefixed keys (e.g. `FilledButtonForeground`) directly.

For more details on per-control lightweight styling resources, see the individual control style pages:

- [Button](styles/Button.md)
- [TextBox](styles/TextBox.md)
- [PasswordBox](styles/PasswordBox.md)
- [CheckBox](styles/CheckBox.md)
- [RadioButton](styles/RadioButton.md)
- [ToggleSwitch](styles/ToggleSwitch.md)
- [Slider](styles/Slider.md)
- [ToggleButton](styles/ToggleButton.md)
