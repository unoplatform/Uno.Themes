---
name: simple-toggleswitch
description: Uses Simple Theme ToggleSwitch style in Uno Platform applications. Use when styling toggle switches for on/off binary settings. Covers the single ToggleSwitch style, track and thumb states, and lightweight resource keys.
metadata:
  author: unoplatform
  version: "1.0"
  framework: uno-platform
  category: simple-theme-controls
---

# Simple Theme — ToggleSwitch Style

## Overview

The Simple theme provides a single ToggleSwitch style. When the Simple theme is active, the **implicit default style** applied to every `<ToggleSwitch>` is `SimpleToggleSwitchStyle`.

## Available Styles

| Style Key | Alias | IsDefaultStyle |
|-----------|-------|----------------|
| `SimpleToggleSwitchStyle` | `ToggleSwitchStyle` | **Yes** |

## Usage Examples

```xml
<!-- Implicit — no Style needed -->
<ToggleSwitch Header="Dark mode"
              OnContent="On"
              OffContent="Off" />

<!-- Explicit -->
<ToggleSwitch Header="Notifications"
              Style="{StaticResource SimpleToggleSwitchStyle}" />
```

## When to Use

Use ToggleSwitch instead of CheckBox when:
- The setting takes effect **immediately** (no "Apply" button needed)
- The choice is binary on/off (not a selection from a list)
- The user expects a physical switch metaphor (e.g., settings pages)

Use CheckBox instead when:
- The change requires confirmation or a submit action
- Multiple independent options can be selected simultaneously

## Lightweight Styling

### Resource Key Pattern

`SimpleToggleSwitch{Part}{State}`

Parts: `TrackOff`, `TrackOn`, `ThumbOff`, `ThumbOn`, `TrackDisabled`, `ThumbDisabled`

### Track & Thumb Resources

| Key | Type | Default Token |
|-----|------|---------------|
| `SimpleToggleSwitchTrackOffBackground` | `SolidColorBrush` | `SimpleBackgroundDefaultSecondaryBrush` |
| `SimpleToggleSwitchTrackOffBorderBrush` | `SolidColorBrush` | `SimpleBorderDefaultDefaultBrush` |
| `SimpleToggleSwitchTrackOnBackground` | `SolidColorBrush` | `SimpleBackgroundBrandDefaultBrush` |
| `SimpleToggleSwitchThumbOffFill` | `SolidColorBrush` | `SimpleIconDefaultSecondaryBrush` |
| `SimpleToggleSwitchThumbOnFill` | `SolidColorBrush` | `SimpleIconBrandOnBrandBrush` |
| `SimpleToggleSwitchTrackOffBackgroundPressed` | `SolidColorBrush` | `SimpleBackgroundDefaultSecondaryPressedBrush` |
| `SimpleToggleSwitchTrackOnBackgroundPressed` | `SolidColorBrush` | `SimpleBackgroundBrandPressedBrush` |
| `SimpleToggleSwitchTrackDisabledBackground` | `SolidColorBrush` | `SimpleBackgroundDisabledDefaultBrush` |
| `SimpleToggleSwitchTrackDisabledBorderBrush` | `SolidColorBrush` | `SimpleBorderDisabledDefaultBrush` |
| `SimpleToggleSwitchThumbDisabledFill` | `SolidColorBrush` | `SimpleIconDisabledDefaultBrush` |
| `SimpleToggleSwitchForeground` | `SolidColorBrush` | `SimpleTextDefaultDefaultBrush` |
| `SimpleToggleSwitchForegroundDisabled` | `SolidColorBrush` | `SimpleTextDisabledDefaultBrush` |

### Layout Resources

| Key | Type | Default Token |
|-----|------|---------------|
| `SimpleToggleSwitchTrackWidth` | `Double` | `SimpleIconLarge` |
| `SimpleToggleSwitchTrackHeight` | `Double` | `SimpleIconSmall` |
| `SimpleToggleSwitchTrackRadius` | `Double` | `12` |
| `SimpleToggleSwitchStrokeThickness` | `Double` | `SimpleStrokeBorder` |
| `SimpleToggleSwitchThumbSize` | `Double` | `SimpleSpace400` |
| `SimpleToggleSwitchThumbOffMargin` | `Thickness` | `4,0,0,0` |
| `SimpleToggleSwitchThumbOnMargin` | `Thickness` | `20,0,0,0` |
| `SimpleToggleSwitchHeaderMargin` | `Thickness` | `0,0,0,4` |

## Related Skills

- [simple-checkbox](../simple-checkbox/SKILL.md)
- [simple-radiobutton](../simple-radiobutton/SKILL.md)
