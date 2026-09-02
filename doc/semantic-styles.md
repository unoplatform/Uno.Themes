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
- **Omarchy**: `FilledButtonStyle` &rarr; `OmarchyFilledButtonStyle`

## Control Style Mappings

The following tables show every semantic style key and how it resolves under each theme.

### Button

| Semantic Key | Material | Simple | Omarchy | Notes |
|---|---|---|---|---|
| `FilledButtonStyle` | `MaterialFilledButtonStyle` | `SimpleFilledButtonStyle` | `OmarchyFilledButtonStyle` | Default implicit style for Material and Simple; Omarchy's implicit `Button` is `OmarchyOutlinedButtonStyle` (the `outline(white)` widget default) |
| `ElevatedButtonStyle` | `MaterialElevatedButtonStyle` | **GAP** | **GAP** | Simple has no elevated/shadow variant |
| `FilledTonalButtonStyle` | `MaterialFilledTonalButtonStyle` | `SimpleFilledTonalButtonStyle` | `OmarchyFilledButtonWhiteStyle` | Simple "Neutral" is closest tonal match |
| `OutlinedButtonStyle` | `MaterialOutlinedButtonStyle` | `SimpleFilledTonalButtonStyle` | `OmarchyOutlinedButtonStyle` | Same Simple target as FilledTonal |
| `TextButtonStyle` | `MaterialTextButtonStyle` | `SimpleTextButtonStyle` | `OmarchyTextButtonStyle` | Text-only appearance |
| `IconButtonStyle` | `MaterialIconButtonStyle` | `SimpleIconButtonStyle` | `OmarchyIconButtonStyle` | Simple has multiple icon button colors; Primary is default |

### Floating Action Button (FAB)

FAB is a Material-specific concept. Under the Simple and Omarchy themes, FAB keys resolve to existing icon button styles.

| Semantic Key | Material | Simple | Omarchy | Notes |
|---|---|---|---|---|
| `FabStyle` | `MaterialFabStyle` | `SimpleIconButtonStyle` | `OmarchyIconButtonStyle` | Primary icon button as FAB equivalent |
| `SmallFabStyle` | `MaterialSmallFabStyle` | `SimpleIconButtonStyle` | `OmarchyIconButtonStyle` | Same as FabStyle |
| `LargeFabStyle` | `MaterialLargeFabStyle` | `SimpleIconButtonStyle` | `OmarchyIconButtonStyle` | No large variant in Simple |
| `SecondaryFabStyle` | `MaterialSecondaryFabStyle` | `SimpleIconButtonNeutralStyle` | `OmarchyIconButtonMagentaStyle` | |
| `SecondarySmallFabStyle` | `MaterialSecondarySmallFabStyle` | `SimpleIconButtonNeutralStyle` | `OmarchyIconButtonMagentaStyle` | |
| `SecondaryLargeFabStyle` | `MaterialSecondaryLargeFabStyle` | `SimpleIconButtonNeutralStyle` | `OmarchyIconButtonMagentaStyle` | |
| `TertiaryFabStyle` | `MaterialTertiaryFabStyle` | `SimpleIconButtonSubtleStyle` | `OmarchyIconButtonCyanStyle` | |
| `TertiarySmallFabStyle` | `MaterialTertiarySmallFabStyle` | `SimpleIconButtonSubtleStyle` | `OmarchyIconButtonCyanStyle` | |
| `TertiaryLargeFabStyle` | `MaterialTertiaryLargeFabStyle` | `SimpleIconButtonSubtleStyle` | `OmarchyIconButtonCyanStyle` | |
| `SurfaceFabStyle` | `MaterialSurfaceFabStyle` | `SimpleIconButtonNeutralStyle` | `OmarchyIconButtonStyle` | |
| `SurfaceSmallFabStyle` | `MaterialSurfaceSmallFabStyle` | `SimpleIconButtonNeutralStyle` | `OmarchyIconButtonStyle` | |
| `SurfaceLargeFabStyle` | `MaterialSurfaceLargeFabStyle` | `SimpleIconButtonNeutralStyle` | `OmarchyIconButtonStyle` | |

### ToggleButton

| Semantic Key | Material | Simple | Omarchy | Notes |
|---|---|---|---|---|
| `TextToggleButtonStyle` | `MaterialTextToggleButtonStyle` | `SimpleTextToggleButtonStyle` | `OmarchyTextToggleButtonStyle` | Text content toggle |
| `IconToggleButtonStyle` | `MaterialIconToggleButtonStyle` | `SimpleIconToggleButtonStyle` | `OmarchyIconToggleButtonStyle` | Compact icon-only toggle |

### TextBox

| Semantic Key | Material | Simple | Omarchy | Notes |
|---|---|---|---|---|
| `FilledTextBoxStyle` | `MaterialFilledTextBoxStyle` | `SimpleFilledTextBoxStyle` | `OmarchyFilledTextBoxStyle` | Background fill, no border |
| `OutlinedTextBoxStyle` | `MaterialOutlinedTextBoxStyle` | `SimpleOutlinedTextBoxStyle` | `OmarchyOutlinedTextBoxStyle` | Default implicit style for Simple |

### PasswordBox

| Semantic Key | Material | Simple | Omarchy | Notes |
|---|---|---|---|---|
| `FilledPasswordBoxStyle` | `MaterialFilledPasswordBoxStyle` | `SimpleFilledPasswordBoxStyle` | `OmarchyFilledPasswordBoxStyle` | Background fill with border |
| `OutlinedPasswordBoxStyle` | `MaterialOutlinedPasswordBoxStyle` | `SimpleOutlinedPasswordBoxStyle` | `OmarchyOutlinedPasswordBoxStyle` | Default implicit style for Simple |

### HyperlinkButton

| Semantic Key | Material | Simple | Omarchy | Notes |
|---|---|---|---|---|
| `HyperlinkButtonStyle` | `MaterialHyperlinkButtonStyle` | `SimpleHyperlinkButtonStyle` | `OmarchyHyperlinkButtonStyle` | Primary underlined link |
| `SecondaryHyperlinkButtonStyle` | `MaterialSecondaryHyperlinkButtonStyle` | `SimpleSecondaryHyperlinkButtonStyle` | `OmarchySecondaryHyperlinkButtonStyle` | Secondary underlined link |

### ComboBox

| Semantic Key | Material | Simple | Omarchy | Notes |
|---|---|---|---|---|
| `ComboBoxStyle` | `MaterialComboBoxStyle` | `SimpleComboBoxStyle` | `OmarchyComboBoxStyle` | Direct match |
| `ComboBoxItemStyle` | `MaterialComboBoxItemStyle` | `SimpleComboBoxItemStyle` | `OmarchyComboBoxItemStyle` | Direct match |

### CheckBox

| Semantic Key | Material | Simple | Omarchy | Notes |
|---|---|---|---|---|
| `CheckBoxStyle` | `MaterialCheckBoxStyle` | `SimpleCheckBoxStyle` | `OmarchyCheckBoxStyle` | Direct match |

### RadioButton

| Semantic Key | Material | Simple | Omarchy | Notes |
|---|---|---|---|---|
| `RadioButtonStyle` | `MaterialRadioButtonStyle` | `SimpleRadioButtonStyle` | `OmarchyRadioButtonStyle` | Direct match |

### ToggleSwitch

| Semantic Key | Material | Simple | Omarchy | Notes |
|---|---|---|---|---|
| `ToggleSwitchStyle` | `MaterialToggleSwitchStyle` | `SimpleToggleSwitchStyle` | `OmarchyToggleSwitchStyle` | Direct match |

### Slider

| Semantic Key | Material | Simple | Omarchy | Notes |
|---|---|---|---|---|
| `SliderStyle` | `MaterialSliderStyle` | `SimpleSliderStyle` | `OmarchySliderStyle` | Direct match |

### ProgressBar

| Semantic Key | Material | Simple | Omarchy | Notes |
|---|---|---|---|---|
| `ProgressBarStyle` | `MaterialProgressBarStyle` | `SimpleProgressBarStyle` | `OmarchyProgressBarStyle` | Horizontal indicator |

### ProgressRing

| Semantic Key | Material | Simple | Omarchy | Notes |
|---|---|---|---|---|
| `ProgressRingStyle` | `MaterialProgressRingStyle` | `SimpleProgressRingStyle` | `OmarchyProgressRingStyle` | Circular indicator |

### ListView

| Semantic Key | Material | Simple | Omarchy | Notes |
|---|---|---|---|---|
| `ListViewStyle` | `MaterialListViewStyle` | `SimpleListViewStyle` | `OmarchyListViewStyle` | Direct match |
| `ListViewItemStyle` | `MaterialListViewItemStyle` | `SimpleListViewItemStyle` | `OmarchyListViewItemStyle` | Direct match |

### ContentDialog

| Semantic Key | Material | Simple | Omarchy | Notes |
|---|---|---|---|---|
| `ContentDialogStyle` | `MaterialContentDialogStyle` | `SimpleContentDialogStyle` | `OmarchyContentDialogStyle` | Direct match |

### CommandBar

| Semantic Key | Material | Simple | Omarchy | Notes |
|---|---|---|---|---|
| `CommandBarStyle` | `MaterialCommandBarStyle` | **GAP** | **GAP** | Simple has no CommandBar style |

### AppBarButton

| Semantic Key | Material | Simple | Omarchy | Notes |
|---|---|---|---|---|
| `AppBarButtonStyle` | `MaterialAppBarButtonStyle` | `SimpleAppBarButtonStyle` | **GAP** | Direct match |

### NavigationView

| Semantic Key | Material | Simple | Omarchy | Notes |
|---|---|---|---|---|
| `NavigationViewStyle` | `MaterialNavigationViewStyle` | `SimpleNavigationViewStyle` | `OmarchyNavigationViewStyle` | |
| `NavigationViewItemStyle` | `MaterialNavigationViewItemStyle` | `SimpleNavigationViewItemStyle` | `OmarchyNavigationViewItemStyle` | |

### CalendarView

| Semantic Key | Material | Simple | Omarchy | Notes |
|---|---|---|---|---|
| `CalendarViewStyle` | `MaterialCalendarViewStyle` | `SimpleCalendarViewStyle` | **GAP** | Direct match |

### CalendarDatePicker

| Semantic Key | Material | Simple | Omarchy | Notes |
|---|---|---|---|---|
| `CalendarDatePickerStyle` | `MaterialCalendarDatePickerStyle` | `SimpleCalendarDatePickerStyle` | **GAP** | Direct match |

### DatePicker

| Semantic Key | Material | Simple | Omarchy | Notes |
|---|---|---|---|---|
| `DatePickerStyle` | `MaterialDatePickerStyle` | `SimpleDatePickerStyle` | **GAP** | Direct match |

### MediaPlayerElement

| Semantic Key | Material | Simple | Omarchy | Notes |
|---|---|---|---|---|
| `MediaTransportControlsStyle` | `MaterialMediaTransportControlsStyle` | **GAP** | **GAP** | Simple has no media style |

### PipsPager

| Semantic Key | Material | Simple | Omarchy | Notes |
|---|---|---|---|---|
| `PipsPagerStyle` | `MaterialPipsPagerStyle` | `SimplePipsPagerStyle` | **GAP** | Pagination dots |

### RatingControl

| Semantic Key | Material | Simple | Omarchy | Notes |
|---|---|---|---|---|
| `RatingControlStyle` | `MaterialRatingControlStyle` | `SimpleRatingControlStyle` | **GAP** | Star rating with brand colors |

### Flyout / MenuFlyout

| Semantic Key | Material | Simple | Omarchy | Notes |
|---|---|---|---|---|
| `FlyoutPresenterStyle` | `MaterialFlyoutPresenterStyle` | `SimpleFlyoutPresenterStyle` | `OmarchyFlyoutPresenterStyle` | |
| `MenuFlyoutPresenterStyle` | `MaterialMenuFlyoutPresenterStyle` | `SimpleMenuFlyoutPresenterStyle` | `OmarchyMenuFlyoutPresenterStyle` | Direct match |
| `MenuFlyoutItemStyle` | `MaterialMenuFlyoutItemStyle` | `SimpleMenuFlyoutItemStyle` | `OmarchyMenuFlyoutItemStyle` | Direct match |
| `MenuFlyoutSeparatorStyle` | `MaterialMenuFlyoutSeparatorStyle` | `SimpleMenuFlyoutSeparatorStyle` | `OmarchyMenuFlyoutSeparatorStyle` | Direct match |
| `MenuFlyoutSubItemStyle` | `MaterialMenuFlyoutSubItemStyle` | `SimpleMenuFlyoutSubItemStyle` | `OmarchyMenuFlyoutSubItemStyle` | Direct match |
| `ToggleMenuFlyoutItemStyle` | `MaterialToggleMenuFlyoutItemStyle` | `SimpleToggleMenuFlyoutItemStyle` | `OmarchyToggleMenuFlyoutItemStyle` | Direct match |
| `RadioMenuFlyoutItemStyle` | `MaterialRadioMenuFlyoutItemStyle` | `SimpleRadioMenuFlyoutItemStyle` | `OmarchyRadioMenuFlyoutItemStyle` | Radio bullet indicator |

## Typography

All themes provide identical semantic typography keys based on the Material Design 3 type scale (Omarchy maps them onto its single monospace face: bold cuts for Display/Headline/Title, the italic cut for Caption).

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

The **Material**, **Simple** and **Omarchy** templates reference the same unprefixed keys (e.g. `FilledButtonForeground`) directly. Omarchy derives hover, pressed and disabled visuals from opacity tokens rather than per-state brushes, so only the base keys (`*Foreground`, `*Background`, `*BorderBrush`) exist there — see [Omarchy Controls Styles](omarchy-controls-styles.md).

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
