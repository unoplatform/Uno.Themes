# Simple Design System (SDS) — Theme Specification

**Theme**: Simple / SDS  
**Created**: 2026-02-16  
**Status**: Active  
**Figma Source**: Simple Design System (Figma)  

## Overview

The Simple Design System is a clean, minimalist design language that prioritizes
clarity and usability. It uses a neutral color palette, generous spacing, and
subtle interaction states. This spec governs the `Uno.Simple.WinUI` theme library
within the Uno.Themes repository.

## Design Tokens

### Color Palette

| Token | Hex | Usage |
|-------|-----|-------|
| Primary | `#2c2c2c` | Primary actions, filled buttons |
| OnPrimary | `#f5f5f5` | Text/icons on primary surfaces |
| Surface | `#ffffff` | Page backgrounds, card surfaces |
| SurfaceVariant | `#f5f5f5` | Secondary surfaces, input backgrounds |
| OnSurface | `#1e1e1e` | Primary text on surface |
| OnSurfaceMedium | `#757575` | Secondary text, placeholders |
| Outline | `#d9d9d9` | Borders, dividers |
| OutlineVariant | `#e8e8e8` | Subtle borders |
| Error | `#dc2626` | Error states, destructive actions |
| OnError | `#ffffff` | Text on error surfaces |

### Typography

| Style | Font | Size | Weight |
|-------|------|------|--------|
| Display | Inter | 36px | Bold |
| Headline | Inter | 24px | SemiBold |
| Title | Inter | 18px | SemiBold |
| Body | Inter | 14–16px | Normal |
| Label | Inter | 12–14px | Medium |
| Caption | Inter | 10–12px | Normal |

### Spacing Scale

`4, 8, 12, 16, 24, 32, 48, 64` — all spacing, padding, and margins derive from
this 4px-based scale.

### Corner Radius

| Context | Value |
|---------|-------|
| Buttons | `6px` |
| Inputs / TextBox | `6px` |
| Cards | `8px` |
| Avatars (circle) | `999px` (full circle) |
| Avatars (square) | `8px` |

## Control Style Inventory

### WinUI Built-in Controls (in `Uno.Simple.WinUI`)

| Control | Figma Name | Style Keys | Status |
|---------|-----------|------------|--------|
| Button | Button | `SimplePrimaryButtonStyle`, `SimpleNeutralButtonStyle`, `SimpleSubtleButtonStyle`, `SimpleLightButtonStyle` + Small/Large/Error/PricingCard/Filter variants (18 total) | ✅ Complete |
| TextBox | Input Field | `SimpleInputTextBoxStyle`, `SimpleOutlinedInputTextBoxStyle`, `SimpleInputTextBoxSmallStyle` | ✅ Complete |
| ComboBox | Select Field | `SimpleSelectFieldStyle`, `SimpleSelectFieldItemStyle`, `SimpleSelectFieldErrorStyle` | ✅ Complete |
| CheckBox | Checkbox Field | `SimpleCheckBoxStyle` | ✅ Complete |
| RadioButton | Radio Field | `SimpleRadioButtonStyle` | ✅ Complete |
| ToggleSwitch | Switch Field | `SimpleToggleSwitchStyle` | ✅ Complete |
| PersonPicture | Avatar | `SimplePersonPictureStyle` + Small/Large/Square/SquareSmall/SquareLarge (6 total) | ✅ Complete |

## Naming Rules (SDS-Specific)

1. Style keys use `Simple` prefix: `Simple<Variant><ControlType>Style`
2. Names follow Figma component names, not WinUI control names:
   - Figma "Input Field" → `SimpleInputTextBoxStyle` (not `SimpleFilledTextBoxStyle`)
   - Figma "Select Field" → `SimpleSelectFieldStyle` (not `SimpleComboBoxStyle`)
   - Figma "Button/Primary" → `SimplePrimaryButtonStyle` (not `SimpleFilledButtonStyle`)
3. Size variants append to the name: `SimplePrimaryButtonSmallStyle`,
   `SimplePrimaryButtonLargeStyle`
4. Error variants use `Error` before the control type:
   `SimplePrimaryErrorButtonStyle`
5. Theme-agnostic aliases (without `Simple` prefix) are provided in
   `_Resources.xaml` for consumer convenience

## Interaction States

SDS uses **opacity-based** state overlays (simpler than Material's ripple):

| State | Visual Treatment |
|-------|-----------------|
| Normal | Base appearance |
| PointerOver | Subtle background opacity change or border highlight |
| Pressed | Slightly darker opacity overlay |
| Focused | Outline/border emphasis (keyboard focus) |
| Disabled | `Opacity="0.4"` on the entire control |

## File Structure

```
src/library/Uno.Simple.WinUI/
├── SimpleTheme.cs
├── SimpleConstants.cs
├── Styles/
│   ├── Application/
│   │   ├── ColorPalette.xaml
│   │   ├── SimpleColors.xaml
│   │   └── Common/Fonts.xaml
│   └── Controls/
│       ├── Button.xaml          (18 named styles)
│       ├── CheckBox.xaml        (1 named style)
│       ├── ComboBox.xaml        (3 named styles)
│       ├── PersonPicture.xaml   (6 named styles)
│       ├── RadioButton.xaml     (1 named style)
│       ├── TextBox.xaml         (3 named styles)
│       ├── ToggleSwitch.xaml    (1 named style)
│       └── _Resources.xaml      (implicit styles + aliases)
└── Generated/
    └── mergedpages.xaml         (auto-generated)
```

**Version**: 1.0.0 | **Last Updated**: 2026-02-16
