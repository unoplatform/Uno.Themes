---
uid: Uno.Themes.SeedColors
---

# Seed Color Palette Generation

Pick one color — typically your brand color — and Uno Themes builds the entire color theme from it: buttons, text, surfaces, outlines, hover and pressed states, for both Light and Dark mode. Instead of hand-defining 30+ color resources, you set a single **seed color** and the library derives the full semantic palette automatically, using the Material Design 3 [HCT](https://material.io/blog/science-of-color-design) color model.

Seed color generation is **opt-in**: by default, `MaterialTheme`, `SimpleTheme`, and `FluentTheme` use their built-in palettes (for Fluent, the platform accent and neutrals). The generator only runs when you explicitly set `PrimarySeed` on a `ThemeColors` object.

> [!TIP]
> Set up [Material](xref:Uno.Themes.Material.GetStarted), [Simple](xref:Uno.Themes.Simple.GetStarted), or [Fluent](fluent-getting-started.md) first. This page assumes the corresponding theme is already in your `App.xaml`. Fluent translates the primary palette into native accent resources, with the differences described in [Fluent's seed cascade](#fluenttheme-seed--accent-cascade).

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

- In Light mode, `PrimaryColor` is your seed color **verbatim**. Material/Simple filled buttons use this color; Fluent's native accent fill uses a derived shade, as described below. The alpha channel is ignored: seeds are treated as fully opaque.
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

Seed colors can be changed at runtime — for example from a settings page or a color picker. Elements holding the theme's shared semantic brushes repaint immediately, including those already on screen: no page re-navigation and no theme toggle is required for those brushes.

This works because the semantic brushes are updated in place rather than replaced, and it covers the state variants too (`PrimaryHoverBrush`, `PrimaryDisabledBrush`, …) along with their opacities — so overriding a token such as `HoverOpacity` in your override dictionary reaches those brushes as well.

Fluent's generated native accent and lightweight resources are replaced during a rebuild, rather than mutated with the shared brushes. Existing native controls are therefore outside this live-instance guarantee and can require resource re-resolution or recreation. This also applies when clearing the seed; see [Fluent behavior notes](fluent-getting-started.md#behavior-notes).

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

### Instance-Based Access via `ApplicationExtensions`

`SemanticThemeHelper.GetTheme()` always reads from `Application.Current`. When you hold a specific `Application` instance — for example in multi-app or hosted scenarios where `Application.Current` is not the application whose theme you want — use the `application.GetTheme()` extension method instead:

```csharp
using Uno.Themes;

var theme = someApplication.GetTheme();
if (theme?.Colors is { } colors)
{
    colors.PrimarySeed = myColor;
}
```

`SemanticThemeHelper.GetTheme()` is equivalent to `Application.Current.GetTheme()`.

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

Used as the value for `BaseTheme.Colors` (`MaterialTheme.Colors`, `SimpleTheme.Colors`, or `FluentTheme.Colors`).

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
| `GetTheme()`    | Method   | Returns the first `BaseTheme` directly in `Application.Current.Resources.MergedDictionaries`, or `null` if none is found. |
| `PrimarySeed`   | Property | Gets or sets the primary seed color on the active theme. Setting regenerates the full palette.     |
| `SecondarySeed` | Property | Gets or sets the secondary seed color. `null` to auto-derive from primary.                         |
| `TertiarySeed`  | Property | Gets or sets the tertiary seed color. `null` to auto-derive from primary.                          |
| `SeedColorMode` | Property | Gets or sets the generation mode on the active theme: `Fidelity` (default) or `TonalSpot`.         |
| `DefaultFontFamily` | Property | Gets or sets the active theme's typeface; see [Typography Font Swap](design-tokens.md#typography-font-swap). |

The properties require an active application theme and throw `InvalidOperationException` if none is found. Setting a secondary or tertiary seed alone does not start generation: an effective primary seed is required. Clearing `PrimarySeed` restores the built-in palette for Material, Simple, and Fluent; explicit override resources remain in effect. Perform theme resource changes on the UI thread.

The helper does not search nested dictionaries or page resources and does not select the last theme by resource precedence. Keep one application theme directly merged, or retain and configure an explicit theme instance for scoped use.

### `ApplicationExtensions`

Extension methods on `Application` for theme access.

| Member                       | Type             | Description                                                                                          |
|------------------------------|------------------|------------------------------------------------------------------------------------------------------|
| `GetTheme(this Application)` | Extension method | Returns the `BaseTheme` instance from the given application's resources, or `null` if none is found. |

## FluentTheme: Seed → Accent Cascade

Under [`FluentTheme`](fluent-getting-started.md), a seed color does more than generate the semantic palette — it also recolors the **built-in Fluent controls** (accent buttons, checked checkboxes, toggle switches, slider fills, …), so stock and semantic-styled UI stay visually coherent:

- The `SystemAccentColor` shade set (`SystemAccentColor`, `Light1`–`Light3`, `Dark1`–`Dark3`) is overridden from the seed's tonal palette: the base accent is the generated light `PrimaryColor` — the seed itself under the default `Fidelity` mode, tone 40 of the chroma-boosted palette under `TonalSpot`. The shades are derived **relative to the accent's own tone**, so a very dark brand color gets darker shades still (its light-theme fill stays navy, not a lighter blue): the dark shades sit at 3/4, 1/2 and 1/4 of the accent's tone, and `Light2` — Fluent's dark-theme accent fill — is anchored at the dark-theme `PrimaryColor` tone (80), with `Light1` midway to it and `Light3` midway from it to white.
- The accent-derived design tokens (`AccentFillColor*`, `AccentTextFillColor*` colors and brushes) are overridden per Light/Dark theme following Fluent's own structure — for example, the light-theme accent fill is the `Dark1` shade and the dark-theme fill is `Light2`, exactly as with the platform accent.
- The cascade honors [`SeedColorMode`](#two-generation-modes) like the semantic palette does, so the built-in accent and the semantic `PrimaryColor` agree at `SystemAccentColor` in Light and `SystemAccentColorLight2` in Dark. The Light native fill still uses the `Dark1` shade. Under the default `Fidelity` mode a muted corporate accent keeps its character in both; `TonalSpot` re-saturates both.
- Text on the accent (`TextOnAccentFillColorPrimary` / `Secondary` and their brushes) is picked for **contrast against the derived fill** — Fluent's white (light theme) and black (dark theme) families assume a mid-tone platform accent, so a pale brand color gets black text on its accent buttons, checked check boxes and toggle switches instead of unreadable white. The semantic defaults that sit on the fill (`FilledButtonForeground*`, `CheckBoxGlyphForegroundChecked`, `ToggleSwitchKnobOnFill`) follow the same pick.

### PrimaryColor overrides drive the accent too

The cascade is not limited to seeds. An explicit **`PrimaryColor` override** — through any channel (`Colors.OverrideDictionary`, `Colors.OverrideSource`, or the legacy `ColorOverrideDictionary` / `ColorOverrideSource` theme properties) — also recolors the built-in Fluent controls, matching how a `PrimaryColor` override visibly recolors controls under Material and Simple:

- Unlike a seed (a *generator input*, mapped through tonal-palette tones), an override is the highest-precedence statement of what Primary **is** — it becomes the accent fill **verbatim** for its theme branch, with the surrounding `SystemAccentColor*` shades and accent-text tones derived relative to it, and the on-accent text family picked by contrast against it.
- A flat value drives both appearances. `Light` and `Dark` values select their respective appearances; `Default` supplies a fallback for either appearance without its own value. Currently an isolated Dark-only override without a seed can also populate the generated native `Default` branch, making it a fallback in Light. Provide both appearance values for predictable accent behavior.
- When both a seed and a `PrimaryColor` override are set, the **override wins** — the same precedence as in the semantic palette.
- Any accent-family key you override **explicitly** (`SystemAccentColor*`, `AccentFillColor*`, `AccentTextFillColor*`) always wins over the derived values.

```xml
<uf:FluentTheme xmlns:uf="using:Uno.Fluent"
                ColorOverrideSource="ms-appx:///ColorPaletteOverride.xaml" />
```

With `ColorPaletteOverride.xaml` defining `PrimaryColor` per theme branch, both semantic-keyed XAML **and** the stock Fluent controls follow it.

The supported bridge input is the theme's configured override dictionary (including the source and obsolete property channels listed above). The constructor's `colorOverride` argument and entries nested in `MergedDictionaries` do not feed Fluent's accent bridge. Other semantic color/brush overrides, such as `OnPrimaryColor`, are not general native accent drivers; see [override compatibility](semantic-styles.md#override-compatibility).

Without a seed or a `PrimaryColor` override, none of these overrides exist and the controls follow the platform accent (on Windows, the user's chosen accent color).

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

- [Material Colors](xref:Uno.Themes.Material.Colors) — the color roles a seed palette fills in
- [Semantic Styles](xref:Uno.Themes.SemanticStyles)
- [Material Getting Started](xref:Uno.Themes.Material.GetStarted)
- [Simple Getting Started](xref:Uno.Themes.Simple.GetStarted)
