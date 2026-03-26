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

## Style Support Matrix

The following matrix shows which semantic style keys are supported by each theme. Only control **Styles** are listed here (not lightweight styling resources).

**Legend:**

| Icon | Meaning |
|------|---------|
| ✅ | Full support — the theme has a distinct style for this key |
| ⚠️ | Alias — the semantic key resolves to the closest available style (no distinct variant) |
| ❌ | No support — the theme does not implement this control or variant |

### Button

| Semantic Style Key       | Material | Simple | Notes |
|--------------------------|:--------:|:------:|-------|
| `FilledButtonStyle`      | ✅ | ✅ | |
| `FilledTonalButtonStyle` | ✅ | ⚠️ | Simple: aliases to `SimpleNeutralButtonStyle` (shared with Outlined) |
| `OutlinedButtonStyle`    | ✅ | ⚠️ | Simple: aliases to `SimpleNeutralButtonStyle` (shared with FilledTonal) |
| `TextButtonStyle`        | ✅ | ✅ | |
| `ElevatedButtonStyle`    | ✅ | ❌ | Simple has no elevated/shadow variant |
| `IconButtonStyle`        | ✅ | ✅ | |

### Floating Action Button (FAB)

| Semantic Style Key          | Material | Simple | Notes |
|-----------------------------|:--------:|:------:|-------|
| `FabStyle`                  | ✅ | ⚠️ | Simple: aliases to icon button styles (no dedicated FAB control) |
| `SmallFabStyle`             | ✅ | ⚠️ | |
| `LargeFabStyle`             | ✅ | ⚠️ | |
| `SecondaryFabStyle`         | ✅ | ⚠️ | |
| `SecondarySmallFabStyle`    | ✅ | ⚠️ | |
| `SecondaryLargeFabStyle`    | ✅ | ⚠️ | |
| `SurfaceFabStyle`           | ✅ | ⚠️ | |
| `SurfaceSmallFabStyle`      | ✅ | ⚠️ | |
| `SurfaceLargeFabStyle`      | ✅ | ⚠️ | |
| `TertiaryFabStyle`          | ✅ | ⚠️ | |
| `TertiarySmallFabStyle`     | ✅ | ⚠️ | |
| `TertiaryLargeFabStyle`     | ✅ | ⚠️ | |

### ToggleButton

| Semantic Style Key        | Material | Simple | Notes |
|---------------------------|:--------:|:------:|-------|
| `TextToggleButtonStyle`   | ✅ | ⚠️ | Simple: aliases to `SimpleDefaultToggleButtonStyle` |
| `IconToggleButtonStyle`   | ✅ | ✅ | |

### TextBox

| Semantic Style Key      | Material | Simple | Notes |
|-------------------------|:--------:|:------:|-------|
| `FilledTextBoxStyle`    | ✅ | ⚠️ | Simple: aliases to `SimpleTextBoxStyle` (filled is the default) |
| `OutlinedTextBoxStyle`  | ✅ | ✅ | |

### PasswordBox

| Semantic Style Key          | Material | Simple | Notes |
|-----------------------------|:--------:|:------:|-------|
| `FilledPasswordBoxStyle`    | ✅ | ⚠️ | Simple: aliases to `SimplePasswordBoxStyle` (filled is the default) |
| `OutlinedPasswordBoxStyle`  | ✅ | ✅ | |

### HyperlinkButton

| Semantic Style Key              | Material | Simple | Notes |
|---------------------------------|:--------:|:------:|-------|
| `HyperlinkButtonStyle`         | ✅ | ✅ | |
| `SecondaryHyperlinkButtonStyle`| ✅ | ✅ | |

### ComboBox

| Semantic Style Key | Material | Simple | Notes |
|--------------------|:--------:|:------:|-------|
| `ComboBoxStyle`    | ✅ | ⚠️ | Simple: aliases to `SimpleSelectFieldStyle` |

### CheckBox

| Semantic Style Key | Material | Simple | Notes |
|--------------------|:--------:|:------:|-------|
| `CheckBoxStyle`    | ✅ | ✅ | |

### RadioButton

| Semantic Style Key  | Material | Simple | Notes |
|---------------------|:--------:|:------:|-------|
| `RadioButtonStyle`  | ✅ | ✅ | |

### ToggleSwitch

| Semantic Style Key   | Material | Simple | Notes |
|----------------------|:--------:|:------:|-------|
| `ToggleSwitchStyle`  | ✅ | ✅ | |

### Slider

| Semantic Style Key | Material | Simple | Notes |
|--------------------|:--------:|:------:|-------|
| `SliderStyle`      | ✅ | ✅ | |

### AppBarButton

| Semantic Style Key    | Material | Simple | Notes |
|-----------------------|:--------:|:------:|-------|
| `AppBarButtonStyle`   | ✅ | ✅ | |

### CalendarView

| Semantic Style Key    | Material | Simple | Notes |
|-----------------------|:--------:|:------:|-------|
| `CalendarViewStyle`   | ✅ | ✅ | |

### CalendarDatePicker

| Semantic Style Key          | Material | Simple | Notes |
|-----------------------------|:--------:|:------:|-------|
| `CalendarDatePickerStyle`   | ✅ | ✅ | |

### DatePicker

| Semantic Style Key | Material | Simple | Notes |
|--------------------|:--------:|:------:|-------|
| `DatePickerStyle`  | ✅ | ✅ | |

### ContentDialog

| Semantic Style Key     | Material | Simple | Notes |
|------------------------|:--------:|:------:|-------|
| `ContentDialogStyle`   | ✅ | ✅ | |

### ListView

| Semantic Style Key    | Material | Simple | Notes |
|-----------------------|:--------:|:------:|-------|
| `ListViewStyle`       | ✅ | ✅ | |
| `ListViewItemStyle`   | ✅ | ✅ | |

### MenuFlyout

| Semantic Style Key            | Material | Simple | Notes |
|-------------------------------|:--------:|:------:|-------|
| `MenuFlyoutPresenterStyle`    | ✅ | ✅ | |
| `MenuFlyoutItemStyle`         | ✅ | ✅ | |
| `MenuFlyoutSeparatorStyle`    | ✅ | ✅ | |
| `MenuFlyoutSubItemStyle`      | ✅ | ✅ | |
| `ToggleMenuFlyoutItemStyle`   | ✅ | ✅ | |
| `RadioMenuFlyoutItemStyle`    | ✅ | ✅ | |

### FlyoutPresenter

| Semantic Style Key       | Material | Simple | Notes |
|--------------------------|:--------:|:------:|-------|
| `FlyoutPresenterStyle`   | ✅ | ✅ | |

### CommandBar

| Semantic Style Key  | Material | Simple | Notes |
|---------------------|:--------:|:------:|-------|
| `CommandBarStyle`   | ✅ | ❌ | Simple has no CommandBar style |

### NavigationView

| Semantic Style Key        | Material | Simple | Notes |
|---------------------------|:--------:|:------:|-------|
| `NavigationViewStyle`     | ✅ | ✅ | |
| `NavigationViewItemStyle` | ✅ | ✅ | |

### ProgressBar

| Semantic Style Key   | Material | Simple | Notes |
|----------------------|:--------:|:------:|-------|
| `ProgressBarStyle`   | ✅ | ✅ | |

### ProgressRing

| Semantic Style Key   | Material | Simple | Notes |
|----------------------|:--------:|:------:|-------|
| `ProgressRingStyle`  | ✅ | ✅ | |

### PipsPager

| Semantic Style Key | Material | Simple | Notes |
|--------------------|:--------:|:------:|-------|
| `PipsPagerStyle`   | ✅ | ✅ | |

### RatingControl

| Semantic Style Key   | Material | Simple | Notes |
|----------------------|:--------:|:------:|-------|
| `RatingControlStyle` | ✅ | ✅ | |

### MediaTransportControls

| Semantic Style Key             | Material | Simple | Notes |
|--------------------------------|:--------:|:------:|-------|
| `MediaTransportControlsStyle`  | ✅ | ❌ | Simple has no media transport style |

## SDS (Simple Design System) Style Keys

In addition to the Material-derived semantic keys above, both themes also support **SDS-naming aliases** — a second set of theme-agnostic keys that use the Simple Design System vocabulary. These resolve to the best-fit style in whichever theme is active.

### Button

| SDS Style Key | Material | Simple | Notes |
|---|:---:|:---:|---|
| `PrimaryButtonStyle` | ⚠️ | ✅ | Material: aliases to `MaterialFilledButtonStyle` |
| `NeutralButtonStyle` | ⚠️ | ✅ | Material: aliases to `MaterialFilledTonalButtonStyle` |
| `SubtleButtonStyle` | ⚠️ | ✅ | Material: aliases to `MaterialTextButtonStyle` |
| `DangerPrimaryButtonStyle` | ❌ | ✅ | Material has no danger variant |
| `DangerSubtleButtonStyle` | ❌ | ✅ | Material has no danger variant |

> [!NOTE]
> Simple also provides `Small` and `Medium` size variants for each button style (e.g., `SmallPrimaryButtonStyle`, `MediumNeutralButtonStyle`). Material has no size variant system, so these are Simple-only.

### IconButton

| SDS Style Key | Material | Simple | Notes |
|---|:---:|:---:|---|
| `IconButtonPrimaryStyle` | ⚠️ | ✅ | Material: aliases to `MaterialIconButtonStyle` |
| `IconButtonNeutralStyle` | ❌ | ✅ | Material has a single icon button style |
| `IconButtonSubtleStyle` | ❌ | ✅ | Material has a single icon button style |
| `IconButtonDangerPrimaryStyle` | ❌ | ✅ | Material has no danger variant |
| `IconButtonDangerSubtleStyle` | ❌ | ✅ | Material has no danger variant |

### TextBox

| SDS Style Key | Material | Simple | Notes |
|---|:---:|:---:|---|
| `TextBoxStyle` | ⚠️ | ✅ | Material: aliases to `MaterialFilledTextBoxStyle` |
| `TextBoxErrorStyle` | ❌ | ✅ | Material has no dedicated error TextBox style |
| `TextBoxSmallStyle` | ❌ | ✅ | Material has no small TextBox variant |

### PasswordBox

| SDS Style Key | Material | Simple | Notes |
|---|:---:|:---:|---|
| `PasswordBoxStyle` | ⚠️ | ✅ | Material: aliases to `MaterialFilledPasswordBoxStyle` |

### ComboBox (Select Field)

| SDS Style Key | Material | Simple | Notes |
|---|:---:|:---:|---|
| `SelectFieldStyle` | ⚠️ | ✅ | Material: aliases to `MaterialComboBoxStyle` |
| `SelectFieldItemStyle` | ❌ | ✅ | Material has no dedicated ComboBox item style |
| `SelectFieldErrorStyle` | ❌ | ✅ | Material has no dedicated error ComboBox style |

### ToggleButton

| SDS Style Key | Material | Simple | Notes |
|---|:---:|:---:|---|
| `ToggleButtonStyle` | ⚠️ | ✅ | Material: aliases to `MaterialTextToggleButtonStyle` |
| `SmallToggleButtonStyle` | ❌ | ✅ | Material has no size variants |
| `MediumToggleButtonStyle` | ❌ | ✅ | Material has no size variants |

### Controls with no Material equivalent

| SDS Style Key | Material | Simple | Notes |
|---|:---:|:---:|---|
| `ToolTipStyle` | ❌ | ✅ | Material v2 has no ToolTip style |
| `ExpanderStyle` | ❌ | ✅ | Material v2 has no Expander style |
| `AutoSuggestBoxStyle` | ❌ | ✅ | Material v2 has no AutoSuggestBox style |

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
