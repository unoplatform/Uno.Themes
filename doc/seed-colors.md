---
uid: Uno.Themes.SeedColors
---

# Seed Color Palette Generation

Uno Themes supports algorithmic color palette generation using the Material Design 3 [HCT (Hue-Chroma-Tone)](https://material.io/blog/science-of-color-design) color space. Instead of manually defining 30+ color resources for Light and Dark themes, you can provide a single **seed color** and the library will derive the full semantic color palette automatically.

Seed color generation is **opt-in**: by default, `MaterialTheme`, `SimpleTheme`, and `FluentTheme` use their built-in palettes (for Fluent, the platform accent and neutrals). The generator only runs when you explicitly set `PrimarySeed` on a `ThemeColors` object.

## Overview

Setting a `PrimarySeed` color on the `ThemeColors` object will generate:

- **Primary**, **Secondary**, **Tertiary**, **Error** tonal palettes
- **Neutral** and **NeutralVariant** palettes (for Surface and Outline roles)
- All semantic color roles (`OnPrimary`, `PrimaryContainer`, `OnPrimaryContainer`, `Surface`, `OnSurface`, `Outline`, etc.) at the correct M3-spec tone levels
- Separate palettes for both **Light** and **Dark** themes

## Getting Started

### Basic Usage - Single Seed Color

In your `App.xaml`, set the `PrimarySeed` property on a `ThemeColors` object via the theme's `Colors` property:

#### [**Material**](#tab/material)

```xml
<MaterialTheme xmlns="using:Uno.Material">
    <MaterialTheme.Colors>
        <ut:ThemeColors xmlns:ut="using:Uno.Themes"
                        PrimarySeed="#6750A4" />
    </MaterialTheme.Colors>
</MaterialTheme>
```

#### [**Simple**](#tab/simple)

```xml
<us:SimpleTheme xmlns:us="using:Uno.Simple">
    <us:SimpleTheme.Colors>
        <ut:ThemeColors xmlns:ut="using:Uno.Themes"
                        PrimarySeed="#6750A4" />
    </us:SimpleTheme.Colors>
</us:SimpleTheme>
```

#### [**Fluent**](#tab/fluent)

```xml
<uf:FluentTheme xmlns:uf="using:Uno.Fluent">
    <uf:FluentTheme.Colors>
        <ut:ThemeColors xmlns:ut="using:Uno.Themes"
                        PrimarySeed="#6750A4" />
    </uf:FluentTheme.Colors>
</uf:FluentTheme>
```

---

### Optional Secondary and Tertiary Seeds

By default, the Secondary and Tertiary palettes are automatically derived from the `PrimarySeed`. You can override them independently:

```xml
<MaterialTheme xmlns="using:Uno.Material">
    <MaterialTheme.Colors>
        <ut:ThemeColors xmlns:ut="using:Uno.Themes"
                        PrimarySeed="#6750A4"
                        SecondarySeed="#625B71"
                        TertiarySeed="#7D5260" />
    </MaterialTheme.Colors>
</MaterialTheme>
```

### Seed Colors with Manual Overrides

Seed-generated colors can be combined with manual overrides. The `OverrideSource` or `OverrideDictionary` on `ThemeColors` takes highest precedence, overriding both default and seed-generated colors:

```xml
<MaterialTheme xmlns="using:Uno.Material">
    <MaterialTheme.Colors>
        <ut:ThemeColors xmlns:ut="using:Uno.Themes"
                        PrimarySeed="#6750A4"
                        OverrideSource="ms-appx:///Styles/ColorPaletteOverride.xaml" />
    </MaterialTheme.Colors>
</MaterialTheme>
```

The override `ResourceDictionary` follows the same format as the existing [manual color overrides](xref:Uno.Themes.Material.GetStarted#manual-color-overrides), using `ThemeDictionaries` with `Light` and `Dark` keys.

## Runtime Seed Color Changes

Seed colors can be changed at runtime. The library patches existing `SolidColorBrush` instances in-place, so UI elements update automatically without page re-navigation.

### Using `SemanticThemeHelper`

The `SemanticThemeHelper` static class provides a convenient one-liner API:

```csharp
using Uno.Themes;
using Windows.UI;

// Change the primary seed color at runtime
SemanticThemeHelper.PrimarySeed = Colors.Green;

// Optionally set secondary/tertiary seeds
SemanticThemeHelper.SecondarySeed = Colors.Teal;
SemanticThemeHelper.TertiarySeed = Colors.Orange;

// Clear seed to revert to the theme's default palette
SemanticThemeHelper.PrimarySeed = null;
```

### Direct Access via `ThemeColors`

You can also access the `ThemeColors` object directly from the theme:

```csharp
using Uno.Themes;

var theme = SemanticThemeHelper.GetTheme();
if (theme?.Colors is { } colors)
{
    colors.PrimarySeed = myColor;
}
```

## API Reference

### `ThemeColors`

Used as the value for `BaseTheme.Colors` (i.e., `MaterialTheme.Colors` or `SimpleTheme.Colors`).

| Property              | Type                 | Description                                                                                                    |
|-----------------------|----------------------|----------------------------------------------------------------------------------------------------------------|
| `PrimarySeed`         | `Color?`             | The primary seed color. When set, derives the full semantic palette algorithmically.                           |
| `SecondarySeed`       | `Color?`             | Optional secondary seed. If `null`, auto-derived from `PrimarySeed`.                                           |
| `TertiarySeed`        | `Color?`             | Optional tertiary seed. If `null`, auto-derived from `PrimarySeed`.                                            |
| `OverrideSource`      | `string`             | URI to a `ResourceDictionary` with color overrides. These override both defaults and seed-generated colors.    |
| `OverrideDictionary`  | `ResourceDictionary` | Direct `ResourceDictionary` with color overrides. Highest precedence.                                          |

### `SemanticThemeHelper`

Static convenience class for runtime theme configuration.

| Member          | Type     | Description                                                                                             |
|-----------------|----------|---------------------------------------------------------------------------------------------------------|
| `GetTheme()`    | Method   | Returns the `BaseTheme` instance from `Application.Current.Resources`, or `null` if none is found.      |
| `PrimarySeed`   | Property | Gets or sets the primary seed color on the active theme. Setting regenerates the full palette.           |
| `SecondarySeed` | Property | Gets or sets the secondary seed color. `null` to auto-derive from primary.                              |
| `TertiarySeed`  | Property | Gets or sets the tertiary seed color. `null` to auto-derive from primary.                               |

## FluentTheme: Seed → Accent Cascade

Under [`FluentTheme`](fluent-getting-started.md), a seed color does more than generate the semantic palette — it also recolors the **built-in Fluent controls** (accent buttons, checked checkboxes, toggle switches, slider fills, …), so stock and semantic-styled UI stay visually coherent:

- The `SystemAccentColor` shade set (`SystemAccentColor`, `Light1`–`Light3`, `Dark1`–`Dark3`) is overridden with tones from the seed's tonal palette (tone 40 for the base accent; 60/70/80 for the light shades; 30/20/10 for the dark shades).
- The accent-derived design tokens (`AccentFillColor*`, `AccentTextFillColor*` colors and brushes) are overridden per Light/Dark theme following Fluent's own structure — for example, the light-theme accent fill is the `Dark1` shade and the dark-theme fill is `Light2`, exactly as with the platform accent.
- `FluentTheme` uses **high-fidelity** generation: the seed's chroma is preserved rather than boosted to the Material minimum, so muted corporate colors keep their character.

Without a seed, none of these overrides exist and the controls follow the platform accent (on Windows, the user's chosen accent color).

> [!NOTE]
> Clearing the seed at runtime (`PrimarySeed = null`) immediately restores the semantic palette and the `SystemAccentColor*` values. Built-in controls that already materialized their accent brushes may keep the last seeded color until the next app-scope resource change or theme switch — a platform resource-cache behavior. Unmerging the theme always restores the platform accent completely.

## Color Precedence

When building the final theme palette, the following precedence order applies (highest wins):

1. **`ThemeColors.OverrideDictionary`** (or `OverrideSource`) - explicit user overrides
2. **Seed-generated palette** - algorithmically derived from `PrimarySeed`, only when one is explicitly set
3. **Theme base colors** - e.g., Simple's grayscale palette or Material's built-in defaults
4. **`SharedColorPalette`** - library defaults

Neither theme sets a seed by default, so without explicit configuration the built-in default palettes apply. Setting `PrimarySeed` activates generation; clearing it (`null`) reverts to the default palette.

This means seed colors override the built-in defaults, but any colors you explicitly set in the `OverrideDictionary` will take precedence over the seed-generated values.

## Further Reading

- [Material Getting Started](xref:Uno.Themes.Material.GetStarted)
- [Simple Getting Started](xref:Uno.Themes.Simple.GetStarted)
