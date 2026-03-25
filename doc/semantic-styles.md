---
uid: Uno.Themes.SemanticStyles
---

# Uno Themes Semantic Design Language

## Overview

The **Uno Themes Semantic Design Language** provides a unified, theme-agnostic way to style controls across different design systems. Instead of referencing theme-specific style keys (e.g., `MaterialFilledButtonStyle` or `SimplePrimaryButtonStyle`), you use **Semantic Style Keys** — unprefixed names like `FilledButtonStyle` — that automatically resolve to the correct implementation for whichever theme is active in your application.

This allows you to write your XAML once and seamlessly switch between design systems (Material, Simple, etc.) without changing any style references.

### How It Works

Each theme library registers aliases that map semantic keys to its own native styles. For example:

- With **Material** active, `FilledButtonStyle` resolves to `MaterialFilledButtonStyle`
- With **Simple** active, `FilledButtonStyle` resolves to `SimplePrimaryButtonStyle`

```xml
<!-- Theme-agnostic — works with any active theme -->
<Button Style="{StaticResource FilledButtonStyle}" Content="Submit" />
```

When a design system doesn't have a direct equivalent for a semantic key, it either:

- **Aliases** the key to its closest match (e.g., Simple maps `FilledTonalButtonStyle` and `OutlinedButtonStyle` to its single `SimpleNeutralButtonStyle`)
- **Omits** the key entirely if the control isn't supported

## Style Mappings

### Button

| Semantic Key | Material Key | Simple Key |
|---|---|---|
| `FilledButtonStyle` | `MaterialFilledButtonStyle` | `SimplePrimaryButtonStyle` |
| `ElevatedButtonStyle` | `MaterialElevatedButtonStyle` | **GAP** |
| `FilledTonalButtonStyle` | `MaterialFilledTonalButtonStyle` | `SimpleNeutralButtonStyle` |
| `OutlinedButtonStyle` | `MaterialOutlinedButtonStyle` | `SimpleNeutralButtonStyle` |
| `TextButtonStyle` | `MaterialTextButtonStyle` | `SimpleSubtleButtonStyle` |
| `IconButtonStyle` | `MaterialIconButtonStyle` | `SimpleIconButtonPrimaryStyle` |

> **Many-to-one note**: `FilledTonalButtonStyle` and `OutlinedButtonStyle` both map to `SimpleNeutralButtonStyle`. Simple's "Neutral" concept covers both tonal and outlined treatments.

### Floating Action Button (FAB)

FAB is a Material-specific concept. Simple provides compatibility aliases that map FAB keys to existing Simple icon button styles.

| Semantic Key | Material Key | Simple Key |
|---|---|---|
| `FabStyle` | `MaterialFabStyle` | `SimpleIconButtonPrimaryStyle` |
| `SmallFabStyle` | `MaterialSmallFabStyle` | `SimpleSmallIconButtonPrimaryStyle` |
| `LargeFabStyle` | `MaterialLargeFabStyle` | `SimpleIconButtonPrimaryStyle` |
| `SecondaryFabStyle` | `MaterialSecondaryFabStyle` | `SimpleIconButtonNeutralStyle` |
| `SecondarySmallFabStyle` | `MaterialSecondarySmallFabStyle` | `SimpleSmallIconButtonNeutralStyle` |
| `SecondaryLargeFabStyle` | `MaterialSecondaryLargeFabStyle` | `SimpleIconButtonNeutralStyle` |
| `TertiaryFabStyle` | `MaterialTertiaryFabStyle` | `SimpleIconButtonSubtleStyle` |
| `TertiarySmallFabStyle` | `MaterialTertiarySmallFabStyle` | `SimpleSmallIconButtonSubtleStyle` |
| `TertiaryLargeFabStyle` | `MaterialTertiaryLargeFabStyle` | `SimpleIconButtonSubtleStyle` |
| `SurfaceFabStyle` | `MaterialSurfaceFabStyle` | `SimpleIconButtonNeutralStyle` |
| `SurfaceSmallFabStyle` | `MaterialSurfaceSmallFabStyle` | `SimpleSmallIconButtonNeutralStyle` |
| `SurfaceLargeFabStyle` | `MaterialSurfaceLargeFabStyle` | `SimpleIconButtonNeutralStyle` |

### ToggleButton

| Semantic Key | Material Key | Simple Key |
|---|---|---|
| `TextToggleButtonStyle` | `MaterialTextToggleButtonStyle` | `SimpleDefaultToggleButtonStyle` |
| `IconToggleButtonStyle` | `MaterialIconToggleButtonStyle` | `SimpleIconToggleButtonStyle` |

### TextBox

| Semantic Key | Material Key | Simple Key |
|---|---|---|
| `FilledTextBoxStyle` | `MaterialFilledTextBoxStyle` | `SimpleTextBoxStyle` |
| `OutlinedTextBoxStyle` | `MaterialOutlinedTextBoxStyle` | `SimpleOutlinedTextBoxStyle` |

### PasswordBox

| Semantic Key | Material Key | Simple Key |
|---|---|---|
| `FilledPasswordBoxStyle` | `MaterialFilledPasswordBoxStyle` | `SimplePasswordBoxStyle` |
| `OutlinedPasswordBoxStyle` | `MaterialOutlinedPasswordBoxStyle` | `SimpleOutlinedPasswordBoxStyle` |

### HyperlinkButton

| Semantic Key | Material Key | Simple Key |
|---|---|---|
| `HyperlinkButtonStyle` | `MaterialHyperlinkButtonStyle` | `SimpleHyperlinkButtonStyle` |
| `SecondaryHyperlinkButtonStyle` | `MaterialSecondaryHyperlinkButtonStyle` | `SimpleSecondaryHyperlinkButtonStyle` |

### ComboBox

| Semantic Key | Material Key | Simple Key |
|---|---|---|
| `ComboBoxStyle` | `MaterialComboBoxStyle` | `SimpleSelectFieldStyle` |

### CheckBox

| Semantic Key | Material Key | Simple Key |
|---|---|---|
| `CheckBoxStyle` | `MaterialCheckBoxStyle` | `SimpleCheckBoxStyle` |

### RadioButton

| Semantic Key | Material Key | Simple Key |
|---|---|---|
| `RadioButtonStyle` | `MaterialRadioButtonStyle` | `SimpleRadioButtonStyle` |

### ToggleSwitch

| Semantic Key | Material Key | Simple Key |
|---|---|---|
| `ToggleSwitchStyle` | `MaterialToggleSwitchStyle` | `SimpleToggleSwitchStyle` |

### Slider

| Semantic Key | Material Key | Simple Key |
|---|---|---|
| `SliderStyle` | `MaterialSliderStyle` | `SimpleSliderStyle` |

### ProgressBar

| Semantic Key | Material Key | Simple Key |
|---|---|---|
| `ProgressBarStyle` | `MaterialProgressBarStyle` | `SimpleProgressBarStyle` |

### ProgressRing

| Semantic Key | Material Key | Simple Key |
|---|---|---|
| `ProgressRingStyle` | `MaterialProgressRingStyle` | `SimpleProgressRingStyle` |

### ListView

| Semantic Key | Material Key | Simple Key |
|---|---|---|
| `ListViewStyle` | `MaterialListViewStyle` | `SimpleListViewStyle` |
| `ListViewItemStyle` | `MaterialListViewItemStyle` | `SimpleListViewItemStyle` |

### ContentDialog

| Semantic Key | Material Key | Simple Key |
|---|---|---|
| `ContentDialogStyle` | `MaterialContentDialogStyle` | `SimpleContentDialogStyle` |

### CommandBar

| Semantic Key | Material Key | Simple Key |
|---|---|---|
| `CommandBarStyle` | `MaterialCommandBarStyle` | **GAP** |

### AppBarButton

| Semantic Key | Material Key | Simple Key |
|---|---|---|
| `AppBarButtonStyle` | `MaterialAppBarButtonStyle` | `SimpleAppBarButtonStyle` |

### NavigationView

| Semantic Key | Material Key | Simple Key |
|---|---|---|
| `NavigationViewStyle` | `MaterialNavigationViewStyle` | `SimpleNavigationViewStyle` |
| `NavigationViewItemStyle` | `MaterialNavigationViewItemStyle` | `SimpleNavigationViewItemStyle` |

### CalendarView

| Semantic Key | Material Key | Simple Key |
|---|---|---|
| `CalendarViewStyle` | `MaterialCalendarViewStyle` | `SimpleCalendarViewStyle` |

### CalendarDatePicker

| Semantic Key | Material Key | Simple Key |
|---|---|---|
| `CalendarDatePickerStyle` | `MaterialCalendarDatePickerStyle` | `SimpleCalendarDatePickerStyle` |

### DatePicker

| Semantic Key | Material Key | Simple Key |
|---|---|---|
| `DatePickerStyle` | `MaterialDatePickerStyle` | `SimpleDatePickerStyle` |

### MediaPlayerElement

| Semantic Key | Material Key | Simple Key |
|---|---|---|
| `MediaTransportControlsStyle` | `MaterialMediaTransportControlsStyle` | **GAP** |

### PipsPager

| Semantic Key | Material Key | Simple Key |
|---|---|---|
| `PipsPagerStyle` | `MaterialPipsPagerStyle` | `SimplePipsPagerStyle` |

### RatingControl

| Semantic Key | Material Key | Simple Key |
|---|---|---|
| `RatingControlStyle` | `MaterialRatingControlStyle` | `SimpleRatingControlStyle` |

### Flyout / MenuFlyout

| Semantic Key | Material Key | Simple Key |
|---|---|---|
| `FlyoutPresenterStyle` | `MaterialFlyoutPresenterStyle` | `SimpleFlyoutPresenterStyle` |
| `MenuFlyoutPresenterStyle` | `MaterialMenuFlyoutPresenterStyle` | `SimpleMenuFlyoutPresenterStyle` |
| `MenuFlyoutItemStyle` | `MaterialMenuFlyoutItemStyle` | `SimpleMenuFlyoutItemStyle` |
| `ToggleMenuFlyoutItemStyle` | `MaterialToggleMenuFlyoutItemStyle` | `SimpleToggleMenuFlyoutItemStyle` |
| `RadioMenuFlyoutItemStyle` | `MaterialRadioMenuFlyoutItemStyle` | `SimpleRadioMenuFlyoutItemStyle` |
| `MenuFlyoutSubItemStyle` | `MaterialMenuFlyoutSubItemStyle` | `SimpleMenuFlyoutSubItemStyle` |
| `MenuFlyoutSeparatorStyle` | `MaterialMenuFlyoutSeparatorStyle` | `SimpleMenuFlyoutSeparatorStyle` |

## Typography Mappings

Both themes publish identical semantic alias keys for typography.

| Semantic Key | Material Key | Simple Key |
|---|---|---|
| `DisplayLarge` | `MaterialDisplayLarge` | `SimpleDisplayLarge` |
| `DisplayMedium` | `MaterialDisplayMedium` | `SimpleDisplayMedium` |
| `DisplaySmall` | `MaterialDisplaySmall` | `SimpleDisplaySmall` |
| `HeadlineLarge` | `MaterialHeadlineLarge` | `SimpleHeadlineLarge` |
| `HeadlineMedium` | `MaterialHeadlineMedium` | `SimpleHeadlineMedium` |
| `HeadlineSmall` | `MaterialHeadlineSmall` | `SimpleHeadlineSmall` |
| `TitleLarge` | `MaterialTitleLarge` | `SimpleTitleLarge` |
| `TitleMedium` | `MaterialTitleMedium` | `SimpleTitleMedium` |
| `TitleSmall` | `MaterialTitleSmall` | `SimpleTitleSmall` |
| `BodyLarge` | `MaterialBodyLarge` | `SimpleBodyLarge` |
| `BodyMedium` | `MaterialBodyMedium` | `SimpleBodyMedium` |
| `BodySmall` | `MaterialBodySmall` | `SimpleBodySmall` |
| `LabelLarge` | `MaterialLabelLarge` | `SimpleLabelLarge` |
| `LabelMedium` | `MaterialLabelMedium` | `SimpleLabelMedium` |
| `LabelSmall` | `MaterialLabelSmall` | `SimpleLabelSmall` |
| `LabelExtraSmall` | `MaterialLabelExtraSmall` | `SimpleLabelExtraSmall` |
| `CaptionLarge` | `MaterialCaptionLarge` | `SimpleCaptionLarge` |
| `CaptionMedium` | `MaterialCaptionMedium` | `SimpleCaptionMedium` |
| `CaptionSmall` | `MaterialCaptionSmall` | `SimpleCaptionSmall` |

## Gap Summary

Semantic keys that exist in Material but have no Simple equivalent. Controls fall back to WinUI default styling when the active theme has no mapping.

| Semantic Key | Priority |
|---|---|
| `ElevatedButtonStyle` | High |
| `CommandBarStyle` | Medium |
| `MediaTransportControlsStyle` | Low |

## Simple-Only Styles (Not in Semantic Layer)

These styles exist in Simple but have no Material equivalent. They are accessible only via their `Simple`-prefixed keys or SDS-native aliases.

| Style Key(s) | Control |
|---|---|
| `SimpleDangerPrimaryButtonStyle` | Button |
| `SimpleDangerSubtleButtonStyle` | Button |
| `SimpleIconButtonNeutralStyle` | Button |
| `SimpleIconButtonSubtleStyle` | Button |
| `SimpleIconButtonDangerPrimaryStyle` | Button |
| `SimpleIconButtonDangerSubtleStyle` | Button |
| All `SimpleSmall*` / `SimpleMedium*` variants | Button |
| `SimpleTextBoxErrorStyle` | TextBox |
| `SimpleTextBoxSmallStyle` | TextBox |
| `SimpleSelectFieldErrorStyle` | ComboBox |
| `SimplePersonPictureStyle` (6 variants) | PersonPicture |
| `SimpleExpanderStyle` | Expander |
| `SimpleAutoSuggestBoxStyle` | AutoSuggestBox |
| `SimpleToolTipStyle` | ToolTip |

## Material-Only Styles (Excluded from Semantic Layer)

Internal or theme-specific styles excluded from the semantic contract.

| Style Key | Reason |
|---|---|
| `MaterialRippleStyle` | Ripple is a Material-specific control |
| `MaterialBaseButtonStyle` | Internal base style |
| `MaterialBaseTextBlockStyle` | Internal base style |
| `MaterialBaseCommandBarStyle` | Internal base style |
| `MaterialDeleteButtonStyle` | TextBox internal button |
| `MaterialRevealButtonStyle` | PasswordBox internal button |
| `MaterialSliderThumbStyle` | Slider internal part |
| `MaterialContentDialogButtonStyle` | Dialog internal button |
| `MaterialContentDialogDefaultButtonStyle` | Dialog internal button |
| `MaterialContentFlyoutPresenterStyle` | Internal variant |
| `MaterialDatePickerFlyoutButtonStyle` | DatePicker internal |
| `MaterialDatePickerFlyoutPresenterStyle` | DatePicker internal |
| `MaterialComboBoxItemStyle` | ComboBox internal |
| `MaterialNavigationViewIconButtonStyle` | NavigationView internal |
| `MaterialNavigationViewIconTextButtonStyle` | NavigationView internal |
| `MaterialPipsPager*ButtonStyle` (6 styles) | PipsPager internals |
| CalendarView internal styles | CalendarView internals |
| All `MaterialDefault*Style` keys | Implicit style targets |

## Lightweight Styling Resources

For the complete list of every semantic lightweight styling resource key per control, see [Semantic Lightweight Styling](semantic-lightweight-styling.md).

## Using Theme-Specific Style Keys Directly

While Semantic Style Keys are the recommended approach, you can reference theme-prefixed style keys directly if needed. For example, if you want to use a Simple-specific style that has no semantic equivalent:

```xml
<!-- Simple-specific styles can still be referenced directly -->
<Button Style="{StaticResource SimpleDangerPrimaryButtonStyle}" Content="Delete" />
<TextBox Style="{StaticResource SimpleTextBoxErrorStyle}" />
```

> [!NOTE]
> Referencing theme-prefixed keys (e.g., `SimplePrimaryButtonStyle`, `MaterialFilledButtonStyle`) ties your XAML to a specific design system. Prefer Semantic Style Keys for cross-theme compatibility.

For the full list of theme-specific styles, see:

- [Material Control Styles](material-controls-styles.md)
- [Simple Control Styles](simple-controls-styles.md)
