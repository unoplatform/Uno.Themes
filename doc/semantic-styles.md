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
| `FilledButtonStyle`      | ✅ | ✅ | Simple: resolves to `SimplePrimaryButtonStyle` |
| `FilledTonalButtonStyle` | ✅ | ⚠️ | Simple: resolves to `SimpleNeutralButtonStyle` (shared with Outlined) |
| `OutlinedButtonStyle`    | ✅ | ⚠️ | Simple: resolves to `SimpleNeutralButtonStyle` (shared with FilledTonal) |
| `TextButtonStyle`        | ✅ | ✅ | Simple: resolves to `SimpleSubtleButtonStyle` |
| `ElevatedButtonStyle`    | ✅ | ❌ | Simple has no elevated/shadow variant |
| `IconButtonStyle`        | ✅ | ✅ | Simple: resolves to `SimpleIconButtonPrimaryStyle` |

### Floating Action Button (FAB)

| Semantic Style Key          | Material | Simple | Notes |
|-----------------------------|:--------:|:------:|-------|
| `FabStyle`                  | ✅ | ⚠️ | Simple: resolves to `SimpleIconButtonPrimaryStyle` |
| `SmallFabStyle`             | ✅ | ⚠️ | Simple: resolves to `SimpleSmallIconButtonPrimaryStyle` |
| `LargeFabStyle`             | ✅ | ⚠️ | Simple: resolves to `SimpleIconButtonPrimaryStyle` |
| `SecondaryFabStyle`         | ✅ | ⚠️ | Simple: resolves to `SimpleIconButtonNeutralStyle` |
| `SecondarySmallFabStyle`    | ✅ | ⚠️ | Simple: resolves to `SimpleSmallIconButtonNeutralStyle` |
| `SecondaryLargeFabStyle`    | ✅ | ⚠️ | Simple: resolves to `SimpleIconButtonNeutralStyle` |
| `SurfaceFabStyle`           | ✅ | ⚠️ | Simple: resolves to `SimpleIconButtonNeutralStyle` |
| `SurfaceSmallFabStyle`      | ✅ | ⚠️ | Simple: resolves to `SimpleSmallIconButtonNeutralStyle` |
| `SurfaceLargeFabStyle`      | ✅ | ⚠️ | Simple: resolves to `SimpleIconButtonNeutralStyle` |
| `TertiaryFabStyle`          | ✅ | ⚠️ | Simple: resolves to `SimpleIconButtonSubtleStyle` |
| `TertiarySmallFabStyle`     | ✅ | ⚠️ | Simple: resolves to `SimpleSmallIconButtonSubtleStyle` |
| `TertiaryLargeFabStyle`     | ✅ | ⚠️ | Simple: resolves to `SimpleIconButtonSubtleStyle` |

### ToggleButton

| Semantic Style Key        | Material | Simple | Notes |
|---------------------------|:--------:|:------:|-------|
| `TextToggleButtonStyle`   | ✅ | ⚠️ | Simple: resolves to `SimpleDefaultToggleButtonStyle` |
| `IconToggleButtonStyle`   | ✅ | ✅ | Simple: resolves to `SimpleIconToggleButtonStyle` |

### TextBox

| Semantic Style Key      | Material | Simple | Notes |
|-------------------------|:--------:|:------:|-------|
| `FilledTextBoxStyle`    | ✅ | ⚠️ | Simple: resolves to `SimpleTextBoxStyle` (single variant) |
| `OutlinedTextBoxStyle`  | ✅ | ✅ | Simple: resolves to `SimpleOutlinedTextBoxStyle` (surface background, visible border) |

### PasswordBox

| Semantic Style Key          | Material | Simple | Notes |
|-----------------------------|:--------:|:------:|-------|
| `FilledPasswordBoxStyle`    | ✅ | ⚠️ | Simple: resolves to `SimplePasswordBoxStyle` (single variant) |
| `OutlinedPasswordBoxStyle`  | ✅ | ✅ | Simple: resolves to `SimpleOutlinedPasswordBoxStyle` (surface background, visible border) |

### HyperlinkButton

| Semantic Style Key              | Material | Simple | Notes |
|---------------------------------|:--------:|:------:|-------|
| `HyperlinkButtonStyle`         | ✅ | ✅ | Simple: resolves to `SimpleHyperlinkButtonStyle` |
| `SecondaryHyperlinkButtonStyle`| ✅ | ✅ | Simple: resolves to `SimpleSecondaryHyperlinkButtonStyle` |

### ComboBox

| Semantic Style Key | Material | Simple | Notes |
|--------------------|:--------:|:------:|-------|
| `ComboBoxStyle`    | ✅ | ✅ | Simple: resolves to `SimpleSelectFieldStyle` |

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
| `RadioMenuFlyoutItemStyle`    | ✅ | ✅ | Simple: resolves to `SimpleRadioMenuFlyoutItemStyle` |

### FlyoutPresenter

| Semantic Style Key       | Material | Simple | Notes |
|--------------------------|:--------:|:------:|-------|
| `FlyoutPresenterStyle`   | ✅ | ✅ | Simple: resolves to `SimpleFlyoutPresenterStyle` |

### CommandBar

| Semantic Style Key  | Material | Simple | Notes |
|---------------------|:--------:|:------:|-------|
| `CommandBarStyle`   | ✅ | ❌ | Simple has no CommandBar style |

### NavigationView

| Semantic Style Key        | Material | Simple | Notes |
|---------------------------|:--------:|:------:|-------|
| `NavigationViewStyle`     | ✅ | ✅ | Simple: resolves to `SimpleNavigationViewStyle` |
| `NavigationViewItemStyle` | ✅ | ✅ | Simple: resolves to `SimpleNavigationViewItemStyle` |

### ProgressBar

| Semantic Style Key   | Material | Simple | Notes |
|----------------------|:--------:|:------:|-------|
| `ProgressBarStyle`   | ✅ | ✅ | Simple: resolves to `SimpleProgressBarStyle` |

### ProgressRing

| Semantic Style Key   | Material | Simple | Notes |
|----------------------|:--------:|:------:|-------|
| `ProgressRingStyle`  | ✅ | ✅ | Simple: resolves to `SimpleProgressRingStyle` |

### PipsPager

| Semantic Style Key | Material | Simple | Notes |
|--------------------|:--------:|:------:|-------|
| `PipsPagerStyle`   | ✅ | ✅ | Simple: resolves to `SimplePipsPagerStyle` |

### RatingControl

| Semantic Style Key   | Material | Simple | Notes |
|----------------------|:--------:|:------:|-------|
| `RatingControlStyle` | ✅ | ✅ | Simple: resolves to `SimpleRatingControlStyle` |

### MediaTransportControls

| Semantic Style Key             | Material | Simple | Notes |
|--------------------------------|:--------:|:------:|-------|
| `MediaTransportControlsStyle`  | ✅ | ❌ | Simple has no media transport style |

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
