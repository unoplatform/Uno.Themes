---
name: simple-button
description: Uses Simple Theme button styles in Uno Platform applications. Use when styling buttons with Primary, Neutral, Subtle, DangerPrimary, or DangerSubtle variants in Small (default) or Medium sizes, plus IconButton equivalents. Covers all Button style variants, size options, and their appropriate use cases following Simple Design System guidelines.
metadata:
  author: unoplatform
  version: "1.0"
  framework: uno-platform
  category: simple-theme-controls
---

# Simple Theme — Button Styles

## Overview

The Simple theme provides five button intent variants, two size tiers, and icon-only equivalents for each. When the Simple theme is active, the **implicit default style** applied to every `<Button>` is `SimplePrimaryButtonStyle`.

### Intent Variants

| Variant | Purpose |
|---------|---------|
| **Primary** | Main call-to-action. High emphasis, filled with brand color. |
| **Neutral** | Secondary actions. Outlined border, no fill. |
| **Subtle** | Low-emphasis or inline actions. No border or fill — text only. |
| **DangerPrimary** | Destructive primary actions (delete, remove). Filled with danger color. |
| **DangerSubtle** | Destructive low-emphasis actions. Danger-colored text, no fill. |

### Size Tiers

| Size | Prefix | MinHeight | FontSize | Padding | Use Case |
|------|--------|-----------|----------|---------|----------|
| **Small (Default)** | *(none)* | 32px | 14px | 8px | Standard buttons, forms, page content |
| Medium | `Medium` | 40px | 16px | 12px | Toolbars, prominent actions, larger touch targets |

The default (no prefix) button styles are Small. There is no "Large" size tier. Use `Small` prefix styles (`SimpleSmallPrimaryButtonStyle`, etc.) when you need an explicit reference to the same small size, or simply use the unprefixed styles (`SimplePrimaryButtonStyle`).

## Available Styles

### Regular Button Styles

| Style Key | Alias | IsDefaultStyle | Use Case |
|-----------|-------|----------------|----------|
| `SimplePrimaryButtonStyle` | `PrimaryButtonStyle`, `FilledButtonStyle` | **Yes** | Primary CTA, submit, confirm |
| `SimpleNeutralButtonStyle` | `NeutralButtonStyle`, `OutlinedButtonStyle` | | Secondary actions, cancel, back |
| `SimpleSubtleButtonStyle` | `SubtleButtonStyle`, `TextButtonStyle` | | Low-emphasis, inline, "Learn more" |
| `SimpleDangerPrimaryButtonStyle` | `DangerPrimaryButtonStyle` | | Delete, remove (high emphasis) |
| `SimpleDangerSubtleButtonStyle` | `DangerSubtleButtonStyle` | | Delete, remove (low emphasis) |

### Small Button Styles (Explicit)

These are identical to the default (no-prefix) styles. Use them when you want to be explicit about the size.

| Style Key | Alias |
|-----------|-------|
| `SimpleSmallPrimaryButtonStyle` | `SmallPrimaryButtonStyle` |
| `SimpleSmallNeutralButtonStyle` | `SmallNeutralButtonStyle` |
| `SimpleSmallSubtleButtonStyle` | `SmallSubtleButtonStyle` |
| `SimpleSmallDangerPrimaryButtonStyle` | `SmallDangerPrimaryButtonStyle` |
| `SimpleSmallDangerSubtleButtonStyle` | `SmallDangerSubtleButtonStyle` |

### Medium Button Styles

| Style Key | Alias |
|-----------|-------|
| `SimpleMediumPrimaryButtonStyle` | `MediumPrimaryButtonStyle` |
| `SimpleMediumNeutralButtonStyle` | `MediumNeutralButtonStyle` |
| `SimpleMediumSubtleButtonStyle` | `MediumSubtleButtonStyle` |
| `SimpleMediumDangerPrimaryButtonStyle` | `MediumDangerPrimaryButtonStyle` |
| `SimpleMediumDangerSubtleButtonStyle` | `MediumDangerSubtleButtonStyle` |

### Icon Button Styles

Icon buttons are for icon-only actions (no text). They use a pill/full-radius shape.

| Style Key | Alias | Use Case |
|-----------|-------|----------|
| `SimpleIconButtonPrimaryStyle` | `IconButtonPrimaryStyle` | Primary icon action |
| `SimpleIconButtonNeutralStyle` | `IconButtonNeutralStyle` | Secondary icon action |
| `SimpleIconButtonSubtleStyle` | `IconButtonSubtleStyle` | Low-emphasis icon action |
| `SimpleIconButtonDangerPrimaryStyle` | `IconButtonDangerPrimaryStyle` | Destructive icon action |
| `SimpleIconButtonDangerSubtleStyle` | `IconButtonDangerSubtleStyle` | Destructive low-emphasis icon |

Small and Medium icon button variants follow the same naming: `SimpleSmallIconButton{Variant}Style` and `SimpleMediumIconButton{Variant}Style`.

## Usage Examples

### Primary Button (Default)

```xml
<!-- Implicit — no Style needed because SimplePrimaryButtonStyle is default -->
<Button Content="Save" />

<!-- Explicit -->
<Button Content="Save"
        Style="{StaticResource SimplePrimaryButtonStyle}" />
```

### Button Hierarchy

```xml
<StackPanel Orientation="Horizontal" Spacing="8">
    <!-- Primary action -->
    <Button Content="Save" />

    <!-- Secondary action -->
    <Button Content="Cancel"
            Style="{StaticResource SimpleNeutralButtonStyle}" />

    <!-- Tertiary action -->
    <Button Content="Learn More"
            Style="{StaticResource SimpleSubtleButtonStyle}" />
</StackPanel>
```

### Danger Actions

```xml
<StackPanel Orientation="Horizontal" Spacing="8">
    <Button Content="Delete Account"
            Style="{StaticResource SimpleDangerPrimaryButtonStyle}" />

    <Button Content="Remove"
            Style="{StaticResource SimpleDangerSubtleButtonStyle}" />
</StackPanel>
```

### Size Variants

```xml
<!-- Default is Small — no prefix needed -->
<Button Content="Edit"
        Style="{StaticResource SimpleNeutralButtonStyle}" />

<!-- Medium for larger touch targets or prominent actions -->
<Button Content="Apply"
        Style="{StaticResource SimpleMediumPrimaryButtonStyle}" />
```

### Icon Button

```xml
<Button Style="{StaticResource SimpleIconButtonSubtleStyle}">
    <FontIcon Glyph="&#xE74D;" />
</Button>
```

## Style Hierarchy (BasedOn)

```
SimpleBaseButtonStyle
├── SimplePrimaryButtonStyle
│   ├── SimpleSmallPrimaryButtonStyle
│   └── SimpleMediumPrimaryButtonStyle
├── SimpleNeutralButtonStyle
│   ├── SimpleSmallNeutralButtonStyle
│   └── SimpleMediumNeutralButtonStyle
├── SimpleSubtleButtonStyle
│   ├── SimpleSmallSubtleButtonStyle
│   └── SimpleMediumSubtleButtonStyle
├── SimpleDangerPrimaryButtonStyle
│   ├── SimpleSmallDangerPrimaryButtonStyle
│   └── SimpleMediumDangerPrimaryButtonStyle
└── SimpleDangerSubtleButtonStyle
    ├── SimpleSmallDangerSubtleButtonStyle
    └── SimpleMediumDangerSubtleButtonStyle

SimpleBaseIconButtonStyle
├── SimpleIconButtonPrimaryStyle
│   ├── SimpleSmallIconButtonPrimaryStyle
│   └── SimpleMediumIconButtonPrimaryStyle
├── SimpleIconButtonNeutralStyle
│   ├── SimpleSmallIconButtonNeutralStyle
│   └── SimpleMediumIconButtonNeutralStyle
├── SimpleIconButtonSubtleStyle
│   ├── SimpleSmallIconButtonSubtleStyle
│   └── SimpleMediumIconButtonSubtleStyle
├── SimpleIconButtonDangerPrimaryStyle
│   ├── SimpleSmallIconButtonDangerPrimaryStyle
│   └── SimpleMediumIconButtonDangerPrimaryStyle
└── SimpleIconButtonDangerSubtleStyle
    ├── SimpleSmallIconButtonDangerSubtleStyle
    └── SimpleMediumIconButtonDangerSubtleStyle
```

## Lightweight Styling

### Resource Key Pattern

All button lightweight resource keys follow this pattern:
`Simple{Variant}Button{Property}{State}`

States: *(none)* = Normal, `PointerOver`, `Pressed`, `Disabled`

### Primary Button Resources

| Key | Type | Default Token |
|-----|------|---------------|
| `SimplePrimaryButtonForeground` | `SolidColorBrush` | `SimpleTextBrandOnBrandBrush` |
| `SimplePrimaryButtonForegroundPointerOver` | `SolidColorBrush` | `SimpleTextBrandOnBrandBrush` |
| `SimplePrimaryButtonForegroundPressed` | `SolidColorBrush` | `SimpleTextBrandOnBrandBrush` |
| `SimplePrimaryButtonForegroundDisabled` | `SolidColorBrush` | `SimpleTextDisabledDefaultBrush` |
| `SimplePrimaryButtonBackground` | `SolidColorBrush` | `SimpleBackgroundBrandDefaultBrush` |
| `SimplePrimaryButtonBackgroundPointerOver` | `SolidColorBrush` | `SimpleBackgroundBrandHoverBrush` |
| `SimplePrimaryButtonBackgroundPressed` | `SolidColorBrush` | `SimpleBackgroundBrandPressedBrush` |
| `SimplePrimaryButtonBackgroundDisabled` | `SolidColorBrush` | `SimpleBackgroundDisabledDefaultBrush` |
| `SimplePrimaryButtonBorderBrush` | `SolidColorBrush` | `SimpleBorderBrandDefaultBrush` |
| `SimplePrimaryButtonBorderBrushPointerOver` | `SolidColorBrush` | `SimpleBorderBrandDefaultBrush` |
| `SimplePrimaryButtonBorderBrushPressed` | `SolidColorBrush` | `SimpleBorderBrandDefaultBrush` |
| `SimplePrimaryButtonBorderBrushDisabled` | `SolidColorBrush` | `SimpleBorderDisabledDefaultBrush` |

### Neutral Button Resources

| Key | Type | Default Token |
|-----|------|---------------|
| `SimpleNeutralButtonForeground` | `SolidColorBrush` | `SimpleTextDefaultDefaultBrush` |
| `SimpleNeutralButtonForegroundPointerOver` | `SolidColorBrush` | `SimpleTextDefaultDefaultBrush` |
| `SimpleNeutralButtonForegroundPressed` | `SolidColorBrush` | `SimpleTextDefaultDefaultBrush` |
| `SimpleNeutralButtonForegroundDisabled` | `SolidColorBrush` | `SimpleTextDisabledDefaultBrush` |
| `SimpleNeutralButtonBackground` | `SolidColorBrush` | `SimpleBackgroundDefaultDefaultBrush` |
| `SimpleNeutralButtonBackgroundPointerOver` | `SolidColorBrush` | `SimpleBackgroundDefaultDefaultHoverBrush` |
| `SimpleNeutralButtonBackgroundPressed` | `SolidColorBrush` | `SimpleBackgroundDefaultDefaultPressedBrush` |
| `SimpleNeutralButtonBackgroundDisabled` | `SolidColorBrush` | `SimpleBackgroundDisabledDefaultBrush` |
| `SimpleNeutralButtonBorderBrush` | `SolidColorBrush` | `SimpleBorderDefaultDefaultBrush` |
| `SimpleNeutralButtonBorderBrushPointerOver` | `SolidColorBrush` | `SimpleBorderDefaultDefaultBrush` |
| `SimpleNeutralButtonBorderBrushPressed` | `SolidColorBrush` | `SimpleBorderDefaultDefaultBrush` |
| `SimpleNeutralButtonBorderBrushDisabled` | `SolidColorBrush` | `SimpleBorderDisabledDefaultBrush` |

### Subtle Button Resources

| Key | Type | Default Token |
|-----|------|---------------|
| `SimpleSubtleButtonForeground` | `SolidColorBrush` | `SimpleTextBrandDefaultBrush` |
| `SimpleSubtleButtonForegroundPointerOver` | `SolidColorBrush` | `SimpleTextBrandDefaultBrush` |
| `SimpleSubtleButtonForegroundPressed` | `SolidColorBrush` | `SimpleTextBrandDefaultBrush` |
| `SimpleSubtleButtonForegroundDisabled` | `SolidColorBrush` | `SimpleTextDisabledDefaultBrush` |
| `SimpleSubtleButtonBackground` | `SolidColorBrush` | `SystemControlTransparentBrush` |
| `SimpleSubtleButtonBackgroundPointerOver` | `SolidColorBrush` | `SimpleBackgroundDefaultDefaultHoverBrush` |
| `SimpleSubtleButtonBackgroundPressed` | `SolidColorBrush` | `SimpleBackgroundDefaultDefaultPressedBrush` |
| `SimpleSubtleButtonBackgroundDisabled` | `SolidColorBrush` | `SystemControlTransparentBrush` |
| `SimpleSubtleButtonBorderBrush` | `SolidColorBrush` | `SystemControlTransparentBrush` |
| `SimpleSubtleButtonBorderBrushPointerOver` | `SolidColorBrush` | `SystemControlTransparentBrush` |
| `SimpleSubtleButtonBorderBrushPressed` | `SolidColorBrush` | `SystemControlTransparentBrush` |
| `SimpleSubtleButtonBorderBrushDisabled` | `SolidColorBrush` | `SystemControlTransparentBrush` |

### DangerPrimary Button Resources

| Key | Type | Default Token |
|-----|------|---------------|
| `SimpleDangerPrimaryButtonForeground` | `SolidColorBrush` | `SimpleTextDangerOnDangerBrush` |
| `SimpleDangerPrimaryButtonForegroundPointerOver` | `SolidColorBrush` | `SimpleTextDangerOnDangerBrush` |
| `SimpleDangerPrimaryButtonForegroundPressed` | `SolidColorBrush` | `SimpleTextDangerOnDangerBrush` |
| `SimpleDangerPrimaryButtonForegroundDisabled` | `SolidColorBrush` | `SimpleTextDisabledDefaultBrush` |
| `SimpleDangerPrimaryButtonBackground` | `SolidColorBrush` | `SimpleBackgroundDangerDefaultBrush` |
| `SimpleDangerPrimaryButtonBackgroundPointerOver` | `SolidColorBrush` | `SimpleBackgroundDangerHoverBrush` |
| `SimpleDangerPrimaryButtonBackgroundPressed` | `SolidColorBrush` | `SimpleBackgroundDangerPressedBrush` |
| `SimpleDangerPrimaryButtonBackgroundDisabled` | `SolidColorBrush` | `SimpleBackgroundDisabledDefaultBrush` |
| `SimpleDangerPrimaryButtonBorderBrush` | `SolidColorBrush` | `SimpleBorderDangerDefaultBrush` |
| `SimpleDangerPrimaryButtonBorderBrushPointerOver` | `SolidColorBrush` | `SimpleBorderDangerDefaultBrush` |
| `SimpleDangerPrimaryButtonBorderBrushPressed` | `SolidColorBrush` | `SimpleBorderDangerDefaultBrush` |
| `SimpleDangerPrimaryButtonBorderBrushDisabled` | `SolidColorBrush` | `SimpleBorderDisabledDefaultBrush` |

### DangerSubtle Button Resources

| Key | Type | Default Token |
|-----|------|---------------|
| `SimpleDangerSubtleButtonForeground` | `SolidColorBrush` | `SimpleTextDangerDefaultBrush` |
| `SimpleDangerSubtleButtonForegroundPointerOver` | `SolidColorBrush` | `SimpleTextDangerDefaultBrush` |
| `SimpleDangerSubtleButtonForegroundPressed` | `SolidColorBrush` | `SimpleTextDangerDefaultBrush` |
| `SimpleDangerSubtleButtonForegroundDisabled` | `SolidColorBrush` | `SimpleTextDisabledDefaultBrush` |
| `SimpleDangerSubtleButtonBackground` | `SolidColorBrush` | `SystemControlTransparentBrush` |
| `SimpleDangerSubtleButtonBackgroundPointerOver` | `SolidColorBrush` | `SimpleBackgroundDefaultDefaultHoverBrush` |
| `SimpleDangerSubtleButtonBackgroundPressed` | `SolidColorBrush` | `SimpleBackgroundDefaultDefaultPressedBrush` |
| `SimpleDangerSubtleButtonBackgroundDisabled` | `SolidColorBrush` | `SystemControlTransparentBrush` |
| `SimpleDangerSubtleButtonBorderBrush` | `SolidColorBrush` | `SystemControlTransparentBrush` |
| `SimpleDangerSubtleButtonBorderBrushPointerOver` | `SolidColorBrush` | `SystemControlTransparentBrush` |
| `SimpleDangerSubtleButtonBorderBrushPressed` | `SolidColorBrush` | `SystemControlTransparentBrush` |
| `SimpleDangerSubtleButtonBorderBrushDisabled` | `SolidColorBrush` | `SystemControlTransparentBrush` |

### Shared Layout Resources

| Key | Type | Default Token |
|-----|------|---------------|
| `SimpleButtonCornerRadius` | `CornerRadius` | `SimpleRadius200CornerRadius` (8px) |
| `SimpleButtonBorderThickness` | `Thickness` | `SimpleStrokeBorderThickness` (1px) |
| `SimpleButtonMediumPadding` | `Thickness` | `SimpleSpace300Thickness` (12px) |
| `SimpleButtonSmallPadding` | `Thickness` | `SimpleSpace200Thickness` (8px) |
| `SimpleButtonMediumMinHeight` | `Double` | `SimpleIconLarge` (40px) |
| `SimpleButtonSmallMinHeight` | `Double` | `SimpleIconMedium` (32px) |
| `SimpleButtonMediumFontSize` | `Double` | `SimpleTypographyScale03` (16px) |
| `SimpleButtonSmallFontSize` | `Double` | `SimpleTypographyScale02` (14px) |
| `SimpleButtonIconSpacing` | `Double` | `SimpleSpace200` (8px) |
| `SimpleButtonFontFamily` | `FontFamily` | `"Inter"` |
| `SimpleButtonFontWeight` | `String` | `"Normal"` |
| `SimpleIconButtonCornerRadius` | `CornerRadius` | `SimpleRadiusFullCornerRadius` (pill) |
| `SimpleIconButtonBorderThickness` | `Thickness` | `SimpleStrokeBorderThickness` (1px) |
| `SimpleIconButtonMediumPadding` | `Thickness` | `SimpleSpace200Thickness` (8px) |
| `SimpleIconButtonSmallPadding` | `Thickness` | `SimpleSpace100Thickness` (4px) |
| `SimpleIconButtonMediumMinSize` | `Double` | `SimpleIconLarge` (40px) |
| `SimpleIconButtonSmallMinSize` | `Double` | `SimpleIconMedium` (32px) |

### Lightweight Styling Example

```xml
<Button Content="Custom Brand Button">
    <Button.Resources>
        <SolidColorBrush x:Key="SimplePrimaryButtonBackground" Color="DarkBlue" />
        <SolidColorBrush x:Key="SimplePrimaryButtonForeground" Color="White" />
        <SolidColorBrush x:Key="SimplePrimaryButtonBackgroundPointerOver" Color="Blue" />
    </Button.Resources>
</Button>
```

## Design Guidelines

- Use **one Primary** button per logical section — it draws the eye.
- Pair Primary with Neutral or Subtle for secondary/tertiary actions.
- Use **DangerPrimary** only for irreversible destructive actions (e.g., "Delete Account").
- Use **DangerSubtle** for less critical destructive actions (e.g., "Remove item").
- The default size is **Small** — use it for standard forms and page content. Use **Medium** for larger touch targets, toolbars, or prominent standalone actions.
- Prefer **IconButton** styles for icon-only buttons — they use pill shapes for better hit targets.

## Related Skills

- [simple-skills-index](../simple-skills-index/SKILL.md)
- [simple-togglebutton](../simple-togglebutton/SKILL.md)
