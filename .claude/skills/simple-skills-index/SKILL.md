---
name: simple-skills-index
description: Index of all Simple Theme agent skills for Uno Platform development. Use when looking for Simple theme styling guidance, finding the right skill for a control, or understanding available Simple Design System controls.
metadata:
  author: unoplatform
  version: "1.0"
  framework: uno-platform
  category: simple-theme-controls
---

# Simple Theme — Skills Index

This is the index of all agent skills for the Uno Platform Simple Theme library. Each skill covers a specific control or control family, providing:
- All available style keys and which is the implicit default
- Compatibility aliases (SDS, Material, and legacy naming)
- Lightweight styling resource keys with types and default design tokens
- Use-case guidance for choosing between style variants

## Setup

| Skill | Description |
|-------|-------------|
| [simple-installation](../simple-installation/SKILL.md) | Install and configure Simple Theme in new or existing Uno Platform apps. Covers UnoFeatures, App.xaml setup, color/font customization. |

## Input Controls

| Skill | Controls | Default Implicit Style |
|-------|----------|----------------------|
| [simple-textbox](../simple-textbox/SKILL.md) | TextBox | `SimpleInputTextBoxStyle` |
| [simple-passwordbox](../simple-passwordbox/SKILL.md) | PasswordBox | `SimplePasswordBoxStyle` |
| [simple-combobox](../simple-combobox/SKILL.md) | ComboBox, ComboBoxItem | `SimpleSelectFieldStyle` |
| [simple-autosuggestbox](../simple-autosuggestbox/SKILL.md) | AutoSuggestBox | `SimpleAutoSuggestBoxStyle` |
| [simple-slider](../simple-slider/SKILL.md) | Slider | `SimpleSliderStyle` |

## Action Controls

| Skill | Controls | Default Implicit Style |
|-------|----------|----------------------|
| [simple-button](../simple-button/SKILL.md) | Button (30 styles) | `SimplePrimaryButtonStyle` |
| [simple-togglebutton](../simple-togglebutton/SKILL.md) | ToggleButton | `SimpleDefaultToggleButtonStyle` |
| [simple-appbarbutton](../simple-appbarbutton/SKILL.md) | AppBarButton | `SimpleAppBarButtonStyle` |

## Selection Controls

| Skill | Controls | Default Implicit Style |
|-------|----------|----------------------|
| [simple-checkbox](../simple-checkbox/SKILL.md) | CheckBox | `SimpleCheckBoxStyle` |
| [simple-radiobutton](../simple-radiobutton/SKILL.md) | RadioButton | `SimpleRadioButtonStyle` |
| [simple-toggleswitch](../simple-toggleswitch/SKILL.md) | ToggleSwitch | `SimpleToggleSwitchStyle` |

## Typography

| Skill | Controls | Default Implicit Style |
|-------|----------|----------------------|
| [simple-textblock](../simple-textblock/SKILL.md) | TextBlock (32 styles) | `SimpleBaseTextBlockStyle` |

## Date & Calendar Controls

| Skill | Controls | Default Implicit Style |
|-------|----------|----------------------|
| [simple-datepicker](../simple-datepicker/SKILL.md) | DatePicker, DatePickerFlyoutPresenter | `SimpleDatePickerStyle` |
| [simple-calendardatepicker](../simple-calendardatepicker/SKILL.md) | CalendarDatePicker | `SimpleCalendarDatePickerStyle` |
| [simple-calendarview](../simple-calendarview/SKILL.md) | CalendarView | `SimpleCalendarViewStyle` |

## Collection & Layout Controls

| Skill | Controls | Default Implicit Style |
|-------|----------|----------------------|
| [simple-listview](../simple-listview/SKILL.md) | ListView, ListViewItem | `SimpleListViewStyle` |
| [simple-expander](../simple-expander/SKILL.md) | Expander | `SimpleExpanderStyle` |

## Overlay & Popup Controls

| Skill | Controls | Default Implicit Style |
|-------|----------|----------------------|
| [simple-contentdialog](../simple-contentdialog/SKILL.md) | ContentDialog | `SimpleContentDialogStyle` |
| [simple-menuflyout](../simple-menuflyout/SKILL.md) | MenuFlyoutPresenter, MenuFlyoutItem, MenuFlyoutSubItem, MenuFlyoutSeparator, ToggleMenuFlyoutItem | `SimpleMenuFlyoutItemStyle`, etc. |
| [simple-tooltip](../simple-tooltip/SKILL.md) | ToolTip | `SimpleToolTipStyle` |

## Display Controls

| Skill | Controls | Default Implicit Style |
|-------|----------|----------------------|
| [simple-personpicture](../simple-personpicture/SKILL.md) | PersonPicture (6 styles) | *(none — must be explicitly styled)* |

## Common Patterns

### Applying a Simple Style

```xml
<!-- Most controls get their Simple style automatically (implicit default) -->
<Button Content="Save" />

<!-- To use a non-default variant, reference it explicitly -->
<Button Content="Cancel"
        Style="{StaticResource SimpleNeutralButtonStyle}" />
```

### Lightweight Styling Override

```xml
<Button Content="Custom">
    <Button.Resources>
        <SolidColorBrush x:Key="SimplePrimaryButtonBackground" Color="DarkBlue" />
    </Button.Resources>
</Button>
```

### Using Compatibility Aliases

All Simple styles have short aliases (e.g., `PrimaryButtonStyle` for `SimplePrimaryButtonStyle`) and legacy Material aliases (e.g., `FilledButtonStyle`) for cross-theme portability.

## Controls NOT Yet Styled

The following controls are planned but not yet implemented in the Simple theme:
- CommandBar
- FlyoutPresenter
- HyperlinkButton
- MediaTransportControls
- NavigationView / NavigationViewItem
- PipsPager
- ProgressBar
- ProgressRing
- RadioMenuFlyoutItem
- RatingControl
