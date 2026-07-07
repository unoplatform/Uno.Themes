# Color Tokens: Material vs Fluent

This document maps Material Design 3 color tokens to their WinUI/Fluent equivalents. Material uses a semantic color system with role-based naming, while Fluent uses system-level accent and control colors.

## Material Color System Overview

Material Design 3 implements a comprehensive semantic color system with:

- **Primary/Secondary/Tertiary**: Brand and accent colors
- **Container colors**: Background colors for components
- **"On" colors**: Foreground colors (text/icons) on corresponding backgrounds
- **Surface colors**: General background and surface colors
- **Variants**: Outline, inverse, and tint variations
- **Error colors**: Error and destructive action states

All colors are defined in both Light and Dark themes with full semantic parity.

**Source**: [SharedColorPalette.xaml](../../src/library/Uno.Themes/Styles/Applications/Common/SharedColorPalette.xaml)

## Primary Colors

| Material Token | Light (HEX) | Dark (HEX) | WinUI/Fluent Equivalent | Notes |
| --- | --- | --- | --- | --- |
| `PrimaryColor` | `#5946D2` | `#C7BFFF` | `SystemAccentColor` | Main brand/accent color |
| `OnPrimaryColor` | `#FFFFFF` | `#2A009F` | `SystemAltHighColor` or White | Text/icons on Primary |
| `PrimaryContainerColor` | `#E5DEFF` | `#4129BA` | `SystemAccentColorLight1` | Contained elements |
| `OnPrimaryContainerColor` | `#170065` | `#E4DFFF` | `SystemAccentColorDark1` | Text on container |
| `PrimaryInverseColor` | `#C8BFFF` | `#2A009F` | N/A | Inverse theme context |

### Legacy Primary Variants (Deprecated in M3)

| Material Token | Light (HEX) | Dark (HEX) | WinUI/Fluent Equivalent | Notes |
| --- | --- | --- | --- | --- |
| `PrimaryVariantDarkColor` | `#0021C1` | `#4128BA` | `SystemAccentColorDark2` | Legacy M2 variant |
| `PrimaryVariantLightColor` | `#9679FF` | `#C8BFFF` | `SystemAccentColorLight2` | Legacy M2 variant |

## Secondary Colors

| Material Token | Light (HEX) | Dark (HEX) | WinUI/Fluent Equivalent | Notes |
| --- | --- | --- | --- | --- |
| `SecondaryColor` | `#6B4EA2` | `#CDC2DC` | `SystemAccentColorLight1` or Custom | Secondary accent |
| `OnSecondaryColor` | `#FFFFFF` | `#332D41` | Foreground on secondary | Text/icons on Secondary |
| `SecondaryContainerColor` | `#EBDDFF` | `#433C52` | Custom | Container background |
| `OnSecondaryContainerColor` | `#220555` | `#EDDFFF` | Custom | Text on container |

### Legacy Secondary Variants (Deprecated in M3)

| Material Token | Light (HEX) | Dark (HEX) | WinUI/Fluent Equivalent | Notes |
| --- | --- | --- | --- | --- |
| `SecondaryVariantDarkColor` | `#3B2574` | `#441F8A` | Custom | Legacy M2 variant |
| `SecondaryVariantLightColor` | `#9B7BD5` | `#EBE6F1` | Custom | Legacy M2 variant |

## Tertiary Colors

| Material Token | Light (HEX) | Dark (HEX) | WinUI/Fluent Equivalent | Notes |
| --- | --- | --- | --- | --- |
| `TertiaryColor` | `#0061A4` | `#9FCAFF` | Custom | Tertiary accent |
| `OnTertiaryColor` | `#FFFFFF` | `#003258` | Foreground on tertiary | Text/icons on Tertiary |
| `TertiaryContainerColor` | `#CFE4FF` | `#00497D` | Custom | Container background |
| `OnTertiaryContainerColor` | `#001D36` | `#D1E4FF` | Custom | Text on container |

## Error Colors

| Material Token | Light (HEX) | Dark (HEX) | WinUI/Fluent Equivalent | Notes |
| --- | --- | --- | --- | --- |
| `ErrorColor` | `#B3261E` | `#FFB4AB` | `SystemErrorTextColor` | Destructive actions |
| `OnErrorColor` | `#FFFFFF` | `#690005` | White or Dark | Text/icons on Error |
| `ErrorContainerColor` | `#F9DEDC` | `#93000A` | Custom | Error container bg |
| `OnErrorContainerColor` | `#410E0B` | `#FFDAD6` | Custom | Text on error container |

## Background Colors

| Material Token | Light (HEX) | Dark (HEX) | WinUI/Fluent Equivalent | Notes |
| --- | --- | --- | --- | --- |
| `BackgroundColor` | `#FCFBFF` | `#1C1B1F` | `SystemAltLowColor` or `ApplicationPageBackgroundThemeBrush` | App background |
| `OnBackgroundColor` | `#1C1B1F` | `#E5E1E6` | `SystemBaseHighColor` | Text on background |

## Surface Colors

| Material Token | Light (HEX) | Dark (HEX) | WinUI/Fluent Equivalent | Notes |
| --- | --- | --- | --- | --- |
| `SurfaceColor` | `#FFFFFF` | `#302D37` | `SystemControlBackgroundBaseLowBrush` | Component surfaces |
| `OnSurfaceColor` | `#1C1B1F` | `#E6E1E5` | `SystemControlForegroundBaseHighBrush` | Text on surfaces |
| `SurfaceVariantColor` | `#F2EFF5` | `#47464F` | `SystemControlBackgroundAltMediumBrush` | Variant surface |
| `OnSurfaceVariantColor` | `#8A8494` | `#C9C5D0` | `SystemControlForegroundBaseMediumBrush` | Text on variant |
| `SurfaceInverseColor` | `#313033` | `#E6E1E5` | Opposite theme surface | Inverse theme |
| `OnSurfaceInverseColor` | `#F4EFF4` | `#1C1B1F` | Opposite theme text | Text in inverse |
| `SurfaceTintColor` | `#5946D2` | `#C7BFFF` | `SystemAccentColor` | Tint overlay |

## Outline Colors

| Material Token | Light (HEX) | Dark (HEX) | WinUI/Fluent Equivalent | Notes |
| --- | --- | --- | --- | --- |
| `OutlineColor` | `#79747E` | `#928F99` | `SystemControlForegroundBaseMediumLowBrush` | Borders, dividers |
| `OutlineVariantColor` | `#C9C5D0` | `#57545D` | `SystemControlForegroundBaseLowBrush` | Subtle dividers |

## Special Colors

| Material Token | Light (HEX) | Dark (HEX) | WinUI/Fluent Equivalent | Notes |
| --- | --- | --- | --- | --- |
| `ShadowColor` | `#33000000` (20% opacity) | `#33000000` (20% opacity) | `SystemChromeBlackMediumLowColor` | Drop shadows |

## Color Roles and Use Cases

### Primary Color Family

- **Purpose**: Main brand identity, primary actions (FABs, prominent buttons)
- **Usage**: `PrimaryColor` for prominent elements, `PrimaryContainerColor` for less prominent
- **Fluent Equivalent**: `SystemAccentColor` is the closest match but lacks semantic container variations

### Secondary Color Family

- **Purpose**: Secondary actions, filtering, highlights
- **Usage**: Less prominent than Primary, used for supporting actions
- **Fluent Equivalent**: No direct equivalent; typically use `SystemAccentColorLight1` or custom

### Tertiary Color Family

- **Purpose**: Contrasting accent for balance
- **Usage**: Badges, pills, complementary highlights
- **Fluent Equivalent**: No direct equivalent; use custom colors

### Surface & Background

- **Purpose**: Layer hierarchy and component backgrounds
- **Usage**: `SurfaceColor` for cards/sheets, `BackgroundColor` for app backdrop
- **Fluent Equivalent**: Various `SystemControlBackground*` brushes, less semantic

### On Colors

- **Purpose**: Ensure readable contrast on colored backgrounds
- **Usage**: Always pair `OnPrimaryColor` with `PrimaryColor`, etc.
- **Fluent Equivalent**: Typically manual White/Black or `SystemAlt*Color`

## Theme Switching

Both Material and Fluent support Light/Dark themes through `ResourceDictionary.ThemeDictionaries`:

- **Material**: All colors automatically switch via `x:Key="Light"` and `x:Key="Default"` (Dark)
- **Fluent**: WinUI handles theme switching automatically for `System*` resources

## Customization

### Material Colors

Override colors in your `App.xaml`:

```xml
<Application.Resources>
    <ResourceDictionary>
        <ResourceDictionary.ThemeDictionaries>
            <ResourceDictionary x:Key="Light">
                <Color x:Key="PrimaryColor">#FF6200EE</Color>
            </ResourceDictionary>
        </ResourceDictionary.ThemeDictionaries>
    </ResourceDictionary>
</Application.Resources>
```

### Using Material DSP

Generate complete color schemes from a `.dsp.json` file:

See [Material DSP Documentation](../material-dsp.md)

## Related Documentation

- [Brushes Reference](brushes.md) - State-based brush variations of these colors
- [Material Colors Documentation](../material-colors.md) - Comprehensive Material color guide
- [Material Getting Started](../material-getting-started.md) - Setup and configuration
