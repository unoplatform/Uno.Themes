---
uid: Uno.Themes.SeedColors
---

# Seed Color Palette Generation

Uno Themes supports algorithmic color palette generation using the Material Design 3 [HCT (Hue-Chroma-Tone)](https://material.io/blog/science-of-color-design) color space. Instead of manually defining 30+ color resources for Light and Dark themes, you can provide a single **seed color** and the library will derive the full semantic color palette automatically.

Seed color generation is **opt-in**: by default, `MaterialTheme` and `SimpleTheme` use their built-in palettes. The generator only runs when you explicitly set `PrimarySeed` on a `ThemeColors` object.

## Overview

Setting a `PrimarySeed` color on the `ThemeColors` object will generate:

- **Primary**, **Secondary** and **Tertiary** tonal palettes
- **Neutral** and **NeutralVariant** palettes (for Surface and Outline roles)
- All semantic color roles (`OnPrimary`, `PrimaryContainer`, `OnPrimaryContainer`, `Surface`, `OnSurface`, `Outline`, etc.) at the correct M3-spec tone levels
- Separate palettes for both **Light** and **Dark** themes

The **Error** palette is *not* generated. Material Design 3 pins Error to a fixed hue and chroma regardless of the seed, so the four Error keys keep the values defined by the theme's base palette. Override them explicitly if you need a different error color.

## Exact-seed Primary

Since version 8.0, the light `PrimaryColor` resource is the seed color **verbatim** — the hex you supply is the hex that renders. Everything else is derived from it:

- Dark `PrimaryColor` stays derived (tone 80). A dark brand color pinned onto a dark surface would be unreadable.
- Light `OnPrimaryColor` is chosen for contrast against the pinned seed rather than being fixed at tone 100, so a pale seed gets a dark foreground and a dark seed gets a light one. The pairing always clears WCAG AA (4.5:1).
- The Secondary, Tertiary, Neutral and NeutralVariant palettes are scaled from the seed's own chroma, so a low-chroma seed such as gray produces a neutral palette instead of a colored one.

Set `PreserveSeedColor="False"` to get the Material Design 3 "tonal spot" behavior instead — Primary derived at tone 40 with a minimum chroma of 48, and fixed chromas on the supporting palettes. That is more vibrant for muted seeds, but does not reproduce the seed color:

```xml
<MaterialTheme xmlns="using:Uno.Material">
    <MaterialTheme.Colors>
        <ut:ThemeColors xmlns:ut="using:Uno.Themes"
                        PrimarySeed="#6750A4"
                        PreserveSeedColor="False" />
    </MaterialTheme.Colors>
</MaterialTheme>
```

> [!NOTE]
> `PreserveSeedColor="False"` reproduces the pre-8.0 generation behavior, but not its output: the HCT gamut solver was corrected in 8.0, so saturated seeds now produce noticeably more vivid palettes in both modes.

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

Seed colors can be changed at runtime. The library rewrites the `Color` of the existing `SolidColorBrush` instances behind the semantic `*Brush` resources, so already-rendered elements repaint immediately — no page re-navigation and no theme toggle required.

This matters because `{ThemeResource PrimaryBrush}` resolves to a brush *instance* and re-evaluates only on a theme change. Replacing the brush would leave everything already on screen painting with the previous one, so the instance is kept and recoloured instead. Each brush's per-state `Opacity` (`PrimaryHoverBrush`, `PrimaryDisabledBrush`, …) is defined in XAML and is preserved.

> [!NOTE]
> Roles that are not generated from the seed — the four `Error*` keys — keep their base-palette values, and a `*Brush` key you define yourself in an override dictionary still wins over the generated one.

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

| Property             | Type                 | Description                                                                                                                                                         |
|----------------------|----------------------|---------------------------------------------------------------------------------------------------------------------------------------------------------------------|
| `PrimarySeed`        | `Color?`             | The primary seed color. When set, derives the full semantic palette algorithmically.                                                                                |
| `SecondarySeed`      | `Color?`             | Optional secondary seed. If `null`, auto-derived from `PrimarySeed`.                                                                                                |
| `TertiarySeed`       | `Color?`             | Optional tertiary seed. If `null`, auto-derived from `PrimarySeed`.                                                                                                 |
| `PreserveSeedColor`  | `bool`               | Default `true`. Pins the light `PrimaryColor` to `PrimarySeed` and scales the derived palettes from its chroma. `False` selects the M3 tonal-spot behavior instead. |
| `OverrideSource`     | `string`             | URI to a `ResourceDictionary` with color overrides. These override both defaults and seed-generated colors.                                                         |
| `OverrideDictionary` | `ResourceDictionary` | Direct `ResourceDictionary` with color overrides. Highest precedence.                                                                                               |

### `SemanticThemeHelper`

Static convenience class for runtime theme configuration.

| Member          | Type     | Description                                                                                        |
|-----------------|----------|----------------------------------------------------------------------------------------------------|
| `GetTheme()`    | Method   | Returns the `BaseTheme` instance from `Application.Current.Resources`, or `null` if none is found. |
| `PrimarySeed`   | Property | Gets or sets the primary seed color on the active theme. Setting regenerates the full palette.     |
| `SecondarySeed` | Property | Gets or sets the secondary seed color. `null` to auto-derive from primary.                         |
| `TertiarySeed`  | Property | Gets or sets the tertiary seed color. `null` to auto-derive from primary.                          |

## Color Precedence

When building the final theme palette, the following precedence order applies (highest wins):

1. **`ThemeColors.OverrideDictionary`** (or `OverrideSource`) - explicit user overrides
2. **Seed-generated palette** - algorithmically derived from `PrimarySeed`, only when one is explicitly set
3. **Theme base colors** - e.g., Simple's grayscale palette or Material's built-in defaults
4. **`SharedColorPalette`** - library defaults

Neither theme sets a seed by default, so without explicit configuration the built-in default palettes apply. Setting `PrimarySeed` activates generation; clearing it (`null`) reverts to the default palette.

This means seed colors override the built-in defaults, but any colors you explicitly set in the `OverrideDictionary` will take precedence over the seed-generated values.

## Further Reading

- [Material Colors](xref:Uno.Themes.Material.Colors) — the color roles a seed palette fills in
- [Semantic Styles](xref:Uno.Themes.SemanticStyles)
- [Material Getting Started](xref:Uno.Themes.Material.GetStarted)
- [Simple Getting Started](xref:Uno.Themes.Simple.GetStarted)
