---
uid: Uno.Themes.SeedColors
---

# Seed Color Palette Generation

Uno Themes supports algorithmic color palette generation using the Material Design 3 [HCT (Hue-Chroma-Tone)](https://material.io/blog/science-of-color-design) color space. Instead of manually defining 30+ color resources for Light and Dark themes, you can provide a single **seed color** and the library will derive the full semantic color palette automatically.

Both `MaterialTheme` and `SimpleTheme` use seed color generation by default — even without explicit configuration. `MaterialTheme` uses `#5946D2` (deep purple) and `SimpleTheme` uses `#808080` (neutral gray) as their built-in default seeds. You can override them by setting `PrimarySeedColor` directly on the theme.

## Overview

Setting a `PrimarySeedColor` on the theme will generate:

- **Primary**, **Secondary**, **Tertiary**, **Error** tonal palettes
- **Neutral** and **NeutralVariant** palettes (for Surface and Outline roles)
- All semantic color roles (`OnPrimary`, `PrimaryContainer`, `OnPrimaryContainer`, `Surface`, `OnSurface`, `Outline`, etc.) at the correct M3-spec tone levels
- Separate palettes for both **Light** and **Dark** themes

## Getting Started

### Basic Usage - Single Seed Color

In your `App.xaml`, set the `PrimarySeedColor` directly on the theme:

#### [**Material**](#tab/material)

```xml
<MaterialTheme xmlns="using:Uno.Material"
               PrimarySeedColor="#6750A4" />
```

#### [**Simple**](#tab/simple)

```xml
<us:SimpleTheme xmlns:us="using:Uno.Simple"
                PrimarySeedColor="#6750A4" />
```

---

### Optional Secondary and Tertiary Seeds

By default, the Secondary and Tertiary palettes are automatically derived from the `PrimarySeedColor`. You can override them independently:

```xml
<MaterialTheme xmlns="using:Uno.Material"
               PrimarySeedColor="#6750A4"
               SecondarySeedColor="#625B71"
               TertiarySeedColor="#7D5260" />
```

### Seed Colors with Manual Overrides

Seed-generated colors can be combined with manual overrides. The `ColorOverrideSource` or `ColorOverrideDictionary` on the theme takes highest precedence, overriding both default and seed-generated colors:

```xml
<MaterialTheme xmlns="using:Uno.Material"
               PrimarySeedColor="#6750A4"
               ColorOverrideSource="ms-appx:///Styles/ColorPaletteOverride.xaml" />
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
SemanticThemeHelper.PrimarySeedColor = Colors.Green;

// Optionally set secondary/tertiary seeds
SemanticThemeHelper.SecondarySeedColor = Colors.Teal;
SemanticThemeHelper.TertiarySeedColor = Colors.Orange;

// Clear seed to revert to theme's default seed color
// (MaterialTheme → #5946D2, SimpleTheme → #808080)
SemanticThemeHelper.PrimarySeedColor = null;
```

### Direct Access via the Theme

You can also access seed properties directly on the theme:

```csharp
using Uno.Themes;

var theme = SemanticThemeHelper.GetTheme();
if (theme is { })
{
    theme.PrimarySeedColor = myColor;
}
```

## API Reference

### `BaseTheme` (seed and override properties)

| Property                | Type                 | Description                                                                                                    |
|-------------------------|----------------------|----------------------------------------------------------------------------------------------------------------|
| `PrimarySeedColor`      | `Color?`             | The primary seed color. When set, derives the full semantic palette algorithmically.                           |
| `SecondarySeedColor`    | `Color?`             | Optional secondary seed. If `null`, auto-derived from `PrimarySeedColor`.                                      |
| `TertiarySeedColor`     | `Color?`             | Optional tertiary seed. If `null`, auto-derived from `PrimarySeedColor`.                                       |
| `ColorOverrideSource`   | `string`             | URI to a `ResourceDictionary` with color overrides. These override both defaults and seed-generated colors.    |
| `ColorOverrideDictionary` | `ResourceDictionary` | Direct `ResourceDictionary` with color overrides. Highest precedence.                                        |

### `SemanticThemeHelper`

Static convenience class for runtime theme configuration.

| Member               | Type     | Description                                                                                             |
|----------------------|----------|---------------------------------------------------------------------------------------------------------|
| `GetTheme()`         | Method   | Returns the `BaseTheme` instance from `Application.Current.Resources`, or `null` if none is found.      |
| `PrimarySeedColor`   | Property | Gets or sets the primary seed color on the active theme. Setting regenerates the full palette.          |
| `SecondarySeedColor` | Property | Gets or sets the secondary seed color. `null` to auto-derive from primary.                              |
| `TertiarySeedColor`  | Property | Gets or sets the tertiary seed color. `null` to auto-derive from primary.                               |

## Color Precedence

When building the final theme palette, the following precedence order applies (highest wins):

1. **`ColorOverrideDictionary`** (or `ColorOverrideSource`) - explicit user overrides
2. **Seed-generated palette** - algorithmically derived from `PrimarySeedColor` (or the theme's `DefaultPrimarySeed`)
3. **Theme base colors** - e.g., Simple's grayscale palette or Material's built-in defaults
4. **`SharedColorPalette`** - library defaults

Both themes provide default primary seeds (`MaterialTheme` → `#5946D2`, `SimpleTheme` → `#808080`), so seed generation is always active. Setting `PrimarySeedColor` overrides the default; clearing it (`null`) reverts to the built-in seed.

This means seed colors override the built-in defaults, but any colors you explicitly set in the `ColorOverrideDictionary` will take precedence over the seed-generated values.

## Further Reading

- [Material Getting Started](xref:Uno.Themes.Material.GetStarted)
- [Simple Getting Started](xref:Uno.Themes.Simple.GetStarted)
