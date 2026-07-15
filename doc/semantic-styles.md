---
uid: Uno.Themes.SemanticStyles
---

# Semantic Styles

Uno Themes provides a **semantic style abstraction layer** that lets you write theme-agnostic XAML. Instead of referencing theme-prefixed style keys (e.g. `MaterialFilledButtonStyle` or `SimpleFilledButtonStyle`), you use a single **semantic key** like `FilledButtonStyle` and the active theme resolves it to the correct design-system-specific style at runtime.

```xml
<!-- Works under Material, Simple, and Fluent themes -->
<Button Style="{StaticResource FilledButtonStyle}" Content="Save" />
```

## How It Works

Each theme's `_Resources.xaml` defines `<StaticResource>` aliases that map semantic keys to theme-specific styles:

- **Material**: `FilledButtonStyle` &rarr; `MaterialFilledButtonStyle`
- **Simple**: `FilledButtonStyle` &rarr; `SimpleFilledButtonStyle`
- **Fluent**: `FilledButtonStyle` &rarr; `AccentButtonStyle` (the built-in WinUI style)

The Fluent theme is an *adapter*: it ships no control templates of its own and instead maps every semantic key onto the built-in Fluent styles provided by `XamlControlsResources` (WinUI on Windows, Uno.UI everywhere else). See [Fluent getting started](fluent-getting-started.md) for setup and behavior notes.

## Control Style Mappings

The following tables show every semantic style key and how it resolves under each theme.

A few Fluent targets are marked *built-in&nbsp;¹*: the corresponding Fluent style has no public resource key on every platform, so `FluentTheme` resolves the semantic key to the control's built-in default style at runtime. The control renders with its stock Fluent appearance either way.

### Button

| Semantic Key | Material | Simple | Fluent | Notes |
|---|---|---|---|---|
| `FilledButtonStyle` | `MaterialFilledButtonStyle` | `SimpleFilledButtonStyle` | `AccentButtonStyle` | Default implicit style for Material/Simple; Fluent accent button |
| `ElevatedButtonStyle` | `MaterialElevatedButtonStyle` | **GAP** | `DefaultButtonStyle` | Simple has no elevated/shadow variant; Fluent has no elevation, standard button is the nearest match |
| `FilledTonalButtonStyle` | `MaterialFilledTonalButtonStyle` | `SimpleFilledTonalButtonStyle` | `DefaultButtonStyle` | Simple "Neutral" is closest tonal match; Fluent standard button has a neutral fill |
| `OutlinedButtonStyle` | `MaterialOutlinedButtonStyle` | `SimpleFilledTonalButtonStyle` | `DefaultButtonStyle` | Fluent standard button carries a stroke |
| `TextButtonStyle` | `MaterialTextButtonStyle` | `SimpleTextButtonStyle` | `FluentTextButtonStyle` | Text-only appearance; Fluent "subtle button" (transparent at rest) |
| `IconButtonStyle` | `MaterialIconButtonStyle` | `SimpleIconButtonStyle` | `FluentIconButtonStyle` | Simple has multiple icon button colors; Primary is default |

### Floating Action Button (FAB)

FAB is a Material-specific concept. Under Simple theme, FAB keys resolve to existing icon button styles; under Fluent they resolve to the accent/standard buttons (Fluent buttons size to content, so there is no size differentiation).

| Semantic Key | Material | Simple | Fluent | Notes |
|---|---|---|---|---|
| `FabStyle` | `MaterialFabStyle` | `SimpleIconButtonStyle` | `AccentButtonStyle` | Primary icon button as FAB equivalent |
| `SmallFabStyle` | `MaterialSmallFabStyle` | `SimpleIconButtonStyle` | `AccentButtonStyle` | Same as FabStyle |
| `LargeFabStyle` | `MaterialLargeFabStyle` | `SimpleIconButtonStyle` | `AccentButtonStyle` | No large variant in Simple/Fluent |
| `SecondaryFabStyle` | `MaterialSecondaryFabStyle` | `SimpleIconButtonNeutralStyle` | `DefaultButtonStyle` | |
| `SecondarySmallFabStyle` | `MaterialSecondarySmallFabStyle` | `SimpleIconButtonNeutralStyle` | `DefaultButtonStyle` | |
| `SecondaryLargeFabStyle` | `MaterialSecondaryLargeFabStyle` | `SimpleIconButtonNeutralStyle` | `DefaultButtonStyle` | |
| `TertiaryFabStyle` | `MaterialTertiaryFabStyle` | `SimpleIconButtonSubtleStyle` | `DefaultButtonStyle` | |
| `TertiarySmallFabStyle` | `MaterialTertiarySmallFabStyle` | `SimpleIconButtonSubtleStyle` | `DefaultButtonStyle` | |
| `TertiaryLargeFabStyle` | `MaterialTertiaryLargeFabStyle` | `SimpleIconButtonSubtleStyle` | `DefaultButtonStyle` | |
| `SurfaceFabStyle` | `MaterialSurfaceFabStyle` | `SimpleIconButtonNeutralStyle` | `DefaultButtonStyle` | |
| `SurfaceSmallFabStyle` | `MaterialSurfaceSmallFabStyle` | `SimpleIconButtonNeutralStyle` | `DefaultButtonStyle` | |
| `SurfaceLargeFabStyle` | `MaterialSurfaceLargeFabStyle` | `SimpleIconButtonNeutralStyle` | `DefaultButtonStyle` | |

### ToggleButton

| Semantic Key | Material | Simple | Fluent | Notes |
|---|---|---|---|---|
| `TextToggleButtonStyle` | `MaterialTextToggleButtonStyle` | `SimpleTextToggleButtonStyle` | `DefaultToggleButtonStyle` | Text content toggle |
| `IconToggleButtonStyle` | `MaterialIconToggleButtonStyle` | `SimpleIconToggleButtonStyle` | `DefaultToggleButtonStyle` | Compact icon-only toggle |

### TextBox

| Semantic Key | Material | Simple | Fluent | Notes |
|---|---|---|---|---|
| `FilledTextBoxStyle` | `MaterialFilledTextBoxStyle` | `SimpleFilledTextBoxStyle` | `DefaultTextBoxStyle` | Background fill, no border; the Fluent TextBox (fill + underline) serves both variants |
| `OutlinedTextBoxStyle` | `MaterialOutlinedTextBoxStyle` | `SimpleOutlinedTextBoxStyle` | `DefaultTextBoxStyle` | Default implicit style for Simple |

### PasswordBox

| Semantic Key | Material | Simple | Fluent | Notes |
|---|---|---|---|---|
| `FilledPasswordBoxStyle` | `MaterialFilledPasswordBoxStyle` | `SimpleFilledPasswordBoxStyle` | `DefaultPasswordBoxStyle` | Background fill with border |
| `OutlinedPasswordBoxStyle` | `MaterialOutlinedPasswordBoxStyle` | `SimpleOutlinedPasswordBoxStyle` | `DefaultPasswordBoxStyle` | Default implicit style for Simple |

### HyperlinkButton

| Semantic Key | Material | Simple | Fluent | Notes |
|---|---|---|---|---|
| `HyperlinkButtonStyle` | `MaterialHyperlinkButtonStyle` | `SimpleHyperlinkButtonStyle` | `DefaultHyperlinkButtonStyle` | Primary underlined link |
| `SecondaryHyperlinkButtonStyle` | `MaterialSecondaryHyperlinkButtonStyle` | `SimpleSecondaryHyperlinkButtonStyle` | `DefaultHyperlinkButtonStyle` | Secondary underlined link |

### ComboBox

| Semantic Key | Material | Simple | Fluent | Notes |
|---|---|---|---|---|
| `ComboBoxStyle` | `MaterialComboBoxStyle` | `SimpleComboBoxStyle` | `DefaultComboBoxStyle` | Direct match |
| `ComboBoxItemStyle` | `MaterialComboBoxItemStyle` | `SimpleComboBoxItemStyle` | `DefaultComboBoxItemStyle` | Direct match |

### CheckBox

| Semantic Key | Material | Simple | Fluent | Notes |
|---|---|---|---|---|
| `CheckBoxStyle` | `MaterialCheckBoxStyle` | `SimpleCheckBoxStyle` | `DefaultCheckBoxStyle` | Direct match |

### RadioButton

| Semantic Key | Material | Simple | Fluent | Notes |
|---|---|---|---|---|
| `RadioButtonStyle` | `MaterialRadioButtonStyle` | `SimpleRadioButtonStyle` | `DefaultRadioButtonStyle` | Direct match |

### ToggleSwitch

| Semantic Key | Material | Simple | Fluent | Notes |
|---|---|---|---|---|
| `ToggleSwitchStyle` | `MaterialToggleSwitchStyle` | `SimpleToggleSwitchStyle` | `DefaultToggleSwitchStyle` | Direct match |

### Slider

| Semantic Key | Material | Simple | Fluent | Notes |
|---|---|---|---|---|
| `SliderStyle` | `MaterialSliderStyle` | `SimpleSliderStyle` | `DefaultSliderStyle` | Direct match |

### ProgressBar

| Semantic Key | Material | Simple | Fluent | Notes |
|---|---|---|---|---|
| `ProgressBarStyle` | `MaterialProgressBarStyle` | `SimpleProgressBarStyle` | `DefaultProgressBarStyle` | Horizontal indicator |

### ProgressRing

| Semantic Key | Material | Simple | Fluent | Notes |
|---|---|---|---|---|
| `ProgressRingStyle` | `MaterialProgressRingStyle` | `SimpleProgressRingStyle` | built-in&nbsp;¹ | Circular indicator |

### ListView

| Semantic Key | Material | Simple | Fluent | Notes |
|---|---|---|---|---|
| `ListViewStyle` | `MaterialListViewStyle` | `SimpleListViewStyle` | built-in&nbsp;¹ | Direct match |
| `ListViewItemStyle` | `MaterialListViewItemStyle` | `SimpleListViewItemStyle` | `DefaultListViewItemStyle` | Direct match |

### ContentDialog

| Semantic Key | Material | Simple | Fluent | Notes |
|---|---|---|---|---|
| `ContentDialogStyle` | `MaterialContentDialogStyle` | `SimpleContentDialogStyle` | `DefaultContentDialogStyle` | Direct match |

### CommandBar

| Semantic Key | Material | Simple | Fluent | Notes |
|---|---|---|---|---|
| `CommandBarStyle` | `MaterialCommandBarStyle` | **GAP** | built-in&nbsp;¹ | Simple has no CommandBar style |

### AppBarButton

| Semantic Key | Material | Simple | Fluent | Notes |
|---|---|---|---|---|
| `AppBarButtonStyle` | `MaterialAppBarButtonStyle` | `SimpleAppBarButtonStyle` | `DefaultAppBarButtonStyle` | Direct match |

### NavigationView

| Semantic Key | Material | Simple | Fluent | Notes |
|---|---|---|---|---|
| `NavigationViewStyle` | `MaterialNavigationViewStyle` | `SimpleNavigationViewStyle` | built-in&nbsp;¹ | |
| `NavigationViewItemStyle` | `MaterialNavigationViewItemStyle` | `SimpleNavigationViewItemStyle` | built-in&nbsp;¹ | |

### CalendarView

| Semantic Key | Material | Simple | Fluent | Notes |
|---|---|---|---|---|
| `CalendarViewStyle` | `MaterialCalendarViewStyle` | `SimpleCalendarViewStyle` | `DefaultCalendarViewStyle` | Direct match |

### CalendarDatePicker

| Semantic Key | Material | Simple | Fluent | Notes |
|---|---|---|---|---|
| `CalendarDatePickerStyle` | `MaterialCalendarDatePickerStyle` | `SimpleCalendarDatePickerStyle` | built-in&nbsp;¹ | Direct match |

### DatePicker

| Semantic Key | Material | Simple | Fluent | Notes |
|---|---|---|---|---|
| `DatePickerStyle` | `MaterialDatePickerStyle` | `SimpleDatePickerStyle` | `DefaultDatePickerStyle` | Direct match |

### MediaPlayerElement

| Semantic Key | Material | Simple | Fluent | Notes |
|---|---|---|---|---|
| `MediaTransportControlsStyle` | `MaterialMediaTransportControlsStyle` | **GAP** | `DefaultMediaTransportControlsStyle` | Simple has no media style |

### PipsPager

| Semantic Key | Material | Simple | Fluent | Notes |
|---|---|---|---|---|
| `PipsPagerStyle` | `MaterialPipsPagerStyle` | `SimplePipsPagerStyle` | built-in&nbsp;¹ | Pagination dots |

### RatingControl

| Semantic Key | Material | Simple | Fluent | Notes |
|---|---|---|---|---|
| `RatingControlStyle` | `MaterialRatingControlStyle` | `SimpleRatingControlStyle` | built-in&nbsp;¹ | Star rating with brand colors |

### Flyout / MenuFlyout

| Semantic Key | Material | Simple | Fluent | Notes |
|---|---|---|---|---|
| `FlyoutPresenterStyle` | `MaterialFlyoutPresenterStyle` | `SimpleFlyoutPresenterStyle` | `DefaultFlyoutPresenterStyle` | |
| `MenuFlyoutPresenterStyle` | `MaterialMenuFlyoutPresenterStyle` | `SimpleMenuFlyoutPresenterStyle` | `DefaultMenuFlyoutPresenterStyle` | Direct match |
| `MenuFlyoutItemStyle` | `MaterialMenuFlyoutItemStyle` | `SimpleMenuFlyoutItemStyle` | `DefaultMenuFlyoutItemStyle` | Direct match |
| `MenuFlyoutSeparatorStyle` | `MaterialMenuFlyoutSeparatorStyle` | `SimpleMenuFlyoutSeparatorStyle` | built-in&nbsp;¹ | Direct match |
| `MenuFlyoutSubItemStyle` | `MaterialMenuFlyoutSubItemStyle` | `SimpleMenuFlyoutSubItemStyle` | `DefaultMenuFlyoutSubItemStyle` | Direct match |
| `ToggleMenuFlyoutItemStyle` | `MaterialToggleMenuFlyoutItemStyle` | `SimpleToggleMenuFlyoutItemStyle` | `DefaultToggleMenuFlyoutItemStyle` | Direct match |
| `RadioMenuFlyoutItemStyle` | `MaterialRadioMenuFlyoutItemStyle` | `SimpleRadioMenuFlyoutItemStyle` | `DefaultRadioMenuFlyoutItemStyle` | Radio bullet indicator |

¹ *built-in*: no public Fluent style key exists on every platform for this control; `FluentTheme` resolves the semantic key to the control's built-in default style at runtime, so the key always resolves and the control keeps its stock Fluent appearance.

## Typography

All themes provide identical semantic typography keys.

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

Material and Simple base their values on the Material Design 3 type scale. Fluent maps the slots onto the [Fluent (WinUI) type ramp](https://learn.microsoft.com/en-us/windows/apps/design/style/typography): where Fluent has a counterpart slot it adopts its size and weight (e.g. `DisplayLarge` = Fluent Display, 68/SemiBold; `BodyMedium` = Fluent Body, 14/Regular), and where it doesn't, the shared size is kept with Fluent weight conventions applied. Under Fluent, every slot uses the platform-default font (`ContentControlThemeFontFamily`) and zero letter-spacing; no font package is shipped.

## Lightweight Styling Portability

Semantic style keys also enable portable [lightweight styling](lightweight-styling.md). Material and Simple expose semantic resource keys for customizing control appearance:

```xml
<!-- This override works under both Material and Simple themes -->
<SolidColorBrush x:Key="FilledButtonForeground" Color="Red" />
```

Both **Material** and **Simple** templates reference the same unprefixed keys (e.g. `FilledButtonForeground`) directly.

> [!NOTE]
> The **Fluent** theme does not yet bridge the semantic lightweight-styling keys to the built-in Fluent control resources — Fluent-styled controls keep their own resource keys (`AccentButtonBackground`, …) for now. Per-control bridging is planned.

For more details on per-control lightweight styling resources, see the individual control style pages:

- [Button](styles/Button.md)
- [TextBox](styles/TextBox.md)
- [PasswordBox](styles/PasswordBox.md)
- [CheckBox](styles/CheckBox.md)
- [RadioButton](styles/RadioButton.md)
- [ToggleSwitch](styles/ToggleSwitch.md)
- [Slider](styles/Slider.md)
- [ToggleButton](styles/ToggleButton.md)
