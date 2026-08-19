---
uid: Uno.Themes.SeedColors
---

# Seed Color Palette Generation

Pick one color — typically your brand color — and Uno Themes builds the entire color theme from it: buttons, text, surfaces, outlines, hover and pressed states, for both Light and Dark mode. Instead of hand-defining 30+ color resources, you set a single **seed color** and the library derives the full semantic palette automatically, using the Material Design 3 [HCT](https://material.io/blog/science-of-color-design) color model.

Seed color generation is **opt-in**: by default, `MaterialTheme` and `SimpleTheme` use their built-in palettes. The generator only runs when you explicitly set `PrimarySeed` on a `ThemeColors` object.

> [!TIP]
> New to Uno Themes? Set up a theme first — see [Material getting started](xref:Uno.Themes.Material.GetStarted) or [Simple getting started](xref:Uno.Themes.Simple.GetStarted) — then come back here. Everything on this page assumes a `MaterialTheme` or `SimpleTheme` is already in your `App.xaml`.

## How it works, in plain terms

The generator reads three things off your seed color: its **hue** (which color family it belongs to — blue, red, green…), its **saturation** (how colorful vs. gray it is; "chroma" in HCT terms), and its **lightness** ("tone"). It then builds a ramp of lighter and darker steps of that color and picks the right step for each role — a strong one for `Primary`, soft ones for containers and surfaces, high-contrast ones for text. You never deal with any of this directly; it is what makes the generated theme feel consistent.

## Overview

Setting a `PrimarySeed` color on the `ThemeColors` object will generate:

- **Primary**, **Secondary** and **Tertiary** tonal palettes
- **Neutral** and **NeutralVariant** palettes (for Surface and Outline roles)
- All semantic color roles (`OnPrimary`, `PrimaryContainer`, `OnPrimaryContainer`, `Surface`, `OnSurface`, `Outline`, etc.) at the correct M3-spec tone levels
- Separate palettes for both **Light** and **Dark** themes

The **Error** palette is *not* generated. Material Design 3 pins Error to a fixed hue and chroma regardless of the seed, so the four Error keys keep the values defined by the theme's base palette. Override them explicitly if you need a different error color.

## Two generation modes

The `SeedColorMode` property on `ThemeColors` picks the recipe. There are two modes, and the default needs no configuration.

### Fidelity (default) — your exact color, guaranteed

Since version 8.0, the default mode keeps the generated theme true to the color you picked:

- In Light mode, `PrimaryColor` is your seed color **verbatim** — the hex you supply is the hex your buttons render. (The alpha channel is ignored: seeds are treated as fully opaque.)
- The color used on top of it (`OnPrimaryColor`, for button text and icons) is chosen automatically so it always stays readable: a pale seed gets dark text, a dark seed gets light text. The pairing always meets the WCAG AA contrast standard (4.5:1).
- Every supporting palette follows the character of your seed: a muted seed gives a muted theme, and a gray seed gives a fully neutral theme.
- In Dark mode, `PrimaryColor` is a lighter derivative of your seed — a dark brand color painted onto a dark background would be unreadable, so Dark mode always brightens it.

### Tonal spot — always vibrant

Set `SeedColorMode="TonalSpot"` to use Material Design 3's standard recipe instead. It enforces a minimum saturation, so even a muted seed produces a colorful theme — at the cost of not reproducing your exact color:

```xml
<MaterialTheme xmlns="using:Uno.Material">
    <MaterialTheme.Colors>
        <ut:ThemeColors xmlns:ut="using:Uno.Themes"
                        PrimarySeed="#6750A4"
                        SeedColorMode="TonalSpot" />
    </MaterialTheme.Colors>
</MaterialTheme>
```

**Which one should I use?** Keep the default (`Fidelity`) when brand accuracy matters — what you pick is what renders. Choose `TonalSpot` when you want the classic, always-colorful Material look and your seed is muted or near-gray.

> [!NOTE]
> `SeedColorMode="TonalSpot"` reproduces the pre-8.0 generation *recipe*, but not its exact output: a color-math bug that washed out saturated seeds was fixed in 8.0, so palettes are now more vivid in both modes. See [Upgrading from 7.x](#upgrading-from-7x) below.

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

Seed colors can be changed at runtime — for example from a settings page or a color picker. The whole app repaints immediately, including elements already on screen: no page re-navigation and no theme toggle required.

This works because the semantic brushes are updated in place rather than replaced, and it covers the state variants too (`PrimaryHoverBrush`, `PrimaryDisabledBrush`, …) along with their opacities — so overriding a token such as `HoverOpacity` in your override dictionary reaches those brushes as well.

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

// Switch the generation mode (default is Fidelity)
SemanticThemeHelper.SeedColorMode = SeedColorMode.TonalSpot;

// Clear seed to revert to the theme's default palette
SemanticThemeHelper.PrimarySeed = null;
```

> [!NOTE]
> The helper works on the theme merged into `Application.Current.Resources`. Its properties throw an `InvalidOperationException` if no `MaterialTheme`/`SimpleTheme` is merged yet — set up the theme in `App.xaml` first. `GetTheme()` is the non-throwing alternative: it returns `null` when no theme is found.

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

> [!TIP]
> The Material and Simple sample apps in this repository include a **Seed Color** page (under *Styles*) with a live color picker — drag it and watch the entire app re-theme in real time, and switch between the two generation modes to compare them.

## Upgrading from 7.x

Version 8.0 changes what the generator produces. **If you never set `PrimarySeed`, nothing changes for you** — the built-in palettes are untouched. If you did set a seed:

- Generated palettes are more vivid across the board. 7.x silently washed out saturated seeds (a bug in the color math, fixed in 8.0).
- The light `PrimaryColor` is now your seed color exactly, and the supporting palettes follow the seed's saturation — the new `Fidelity` default described above.
- To stay closest to the previous recipe, set `SeedColorMode="TonalSpot"`. The output will still differ from 7.x because of the color-math fix, but the vibrant character is the same.
- If you subclassed a theme and overrode the protected `UseHighFidelityColors` property, it still works but is obsolete — set `SeedColorMode` on `ThemeColors` instead.

## API Reference

### `ThemeColors`

Used as the value for `BaseTheme.Colors` (i.e., `MaterialTheme.Colors` or `SimpleTheme.Colors`).

| Property             | Type                 | Description                                                                                                                                                         |
|----------------------|----------------------|---------------------------------------------------------------------------------------------------------------------------------------------------------------------|
| `PrimarySeed`        | `Color?`             | The primary seed color. When set, derives the full semantic palette algorithmically.                                                                                |
| `SecondarySeed`      | `Color?`             | Optional secondary seed. If `null`, auto-derived from `PrimarySeed`.                                                                                                |
| `TertiarySeed`       | `Color?`             | Optional tertiary seed. If `null`, auto-derived from `PrimarySeed`.                                                                                                 |
| `SeedColorMode`      | `SeedColorMode`      | Default `Fidelity`: light `PrimaryColor` is the seed verbatim; palettes follow its saturation. `TonalSpot` selects Material's always-vibrant recipe.                |
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
| `SeedColorMode` | Property | Gets or sets the generation mode on the active theme: `Fidelity` (default) or `TonalSpot`.         |

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
