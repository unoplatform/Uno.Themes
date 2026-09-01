---
uid: Uno.Themes.DesignTokens
---

# Design Tokens & Override Surface

Uno.Themes exposes a set of **shared design tokens** — semantic XAML resources for typography, spacing, shape (corner radius), and density (control height / icon size). These tokens are consumed by all control templates, so overriding a single token key globally affects every control that references it.

## Token Categories

### Typography

A single root typeface token cascades to all type-scale keys:

| Key                 | Default                                                       | Role             |
|---------------------|---------------------------------------------------------------|------------------|
| `DefaultFontFamily` | Segoe UI (Material: Roboto, Simple: Inter, Cupertino: SF Pro) | Every type scale |

Per-scale variation is expressed through the `*FontWeight` tokens, not through separate font
families: the root points at a single family whose weights resolve from one reference (a variable
font, or a font with a [font manifest](https://platform.uno/docs/articles/features/custom-fonts.html#variable-fonts-and-font-manifest)
on platforms without variable-font support).

> [!IMPORTANT]
> **Breaking change**: the former `TypefacePlain` / `TypefaceBrand` token pair (and Simple's
> per-weight `SimpleRegular/Medium/SemiBold/BoldFontFamily` keys) have been removed. Override
> `DefaultFontFamily` instead; per-scale weight nuance is carried by the `*FontWeight` tokens.

Per-scale keys follow the pattern `{Role}{Size}FontFamily`, `{Role}{Size}FontSize`, `{Role}{Size}FontWeight`, `{Role}{Size}CharacterSpacing` — for example `DisplayLargeFontFamily`, `BodyMediumFontSize`.

### Spacing

| Key         | Value (px) | Thickness Key        |
|-------------|------------|----------------------|
| `Space0`    | 0          | `Space0Thickness`    |
| `Space050`  | 2          | `Space050Thickness`  |
| `Space100`  | 4          | `Space100Thickness`  |
| `Space150`  | 6          | `Space150Thickness`  |
| `Space200`  | 8          | `Space200Thickness`  |
| `Space300`  | 12         | `Space300Thickness`  |
| `Space400`  | 16         | `Space400Thickness`  |
| `Space500`  | 20         | `Space500Thickness`  |
| `Space600`  | 24         | `Space600Thickness`  |
| `Space800`  | 32         | `Space800Thickness`  |
| `Space1200` | 48         | `Space1200Thickness` |
| `Space1600` | 64         | `Space1600Thickness` |
| `Space2400` | 96         | `Space2400Thickness` |
| `Space4000` | 160        | `Space4000Thickness` |

### Shape (Corner Radius)

| Key          | Value (px) | CornerRadius Key         |
|--------------|------------|--------------------------|
| `Radius0`    | 0          | `Radius0CornerRadius`    |
| `Radius050`  | 2          | `Radius050CornerRadius`  |
| `Radius100`  | 4          | `Radius100CornerRadius`  |
| `Radius200`  | 8          | `Radius200CornerRadius`  |
| `Radius300`  | 12         | `Radius300CornerRadius`  |
| `Radius400`  | 16         | `Radius400CornerRadius`  |
| `Radius700`  | 28         | `Radius700CornerRadius`  |
| `RadiusFull` | 9999       | `RadiusFullCornerRadius` |

### Density

| Key                        | Default Value (px) |
|----------------------------|--------------------|
| `ControlHeightSmall`       | 32                 |
| `ControlHeightMedium`      | 40                 |
| `ControlHeightMediumLarge` | 44                 |
| `ControlHeightLarge`       | 48                 |
| `TouchTargetMinSize`       | 48                 |
| `IconSizeSmall`            | 16                 |
| `IconSizeMedium`           | 24                 |
| `IconSizeLarge`            | 32                 |

## Overriding Tokens

### Via Scalar Properties

Set `DefaultCornerRadius` (shape) or `DefaultSpacing` (spacing) on the theme to generate an entire scale from a single base value:

```xml
<!-- App.xaml -->
<MaterialTheme DefaultCornerRadius="4" DefaultSpacing="6" />
```

This generates all `Radius*` / `Space*` tokens as multiples of the base value. The same properties are available on `SimpleTheme`.

For spacing, the [density mode](#density-modes) (`DefaultDensity`) composes with the base unit rather than replacing it: the effective spacing base is `DefaultSpacing × density factor` (`Compact` ×0.75, `Regular` ×1, `Comfy` ×1.25). With the default base of 4, the modes yield 3 / 4 / 5.

> [!IMPORTANT]
> `DefaultCornerRadius`, `DefaultSpacing`, and `DefaultDensity` are **construction-time settings**. Set them where the theme is declared — normally `App.xaml` — and treat them as fixed for the lifetime of the theme.
>
> Assigning them later does regenerate the `Radius*` and `Space*` token resources, but it does **not** restyle controls: neither the ones already on screen nor ones created afterwards. Unlike colors — which *can* change live, see [Runtime Seed Color Changes](seed-colors.md#runtime-seed-color-changes) — these tokens are `CornerRadius` / `Thickness` / `double` **values**, and the per-control keys that consume them (`ButtonCornerRadius`, `ButtonPadding`, …) are resolved once when the theme's control-style dictionaries are first parsed. There is no live instance to update, so the new value never reaches the control templates.
>
> If you need to offer density or shape as a user setting, change the property and then recreate the root content (or re-navigate) so the styles are applied fresh.

### Via Lightweight Styling

To override individual tokens without changing the whole scale, use standard XAML resource overrides at any level:

```xml
<Page.Resources>
    <x:Double x:Key="Space200">12</x:Double>
    <Thickness x:Key="Space200Thickness">12</Thickness>
    <CornerRadius x:Key="Radius200CornerRadius">10</CornerRadius>
</Page.Resources>
```

### Properties Reference

| Property              | Type      | Description                                                                                                                             |
|-----------------------|-----------|-----------------------------------------------------------------------------------------------------------------------------------------|
| `DefaultCornerRadius` | `double`  | Base corner radius unit; generates the full `Radius*` scale. Construction-time.                                                         |
| `DefaultSpacing`      | `double`  | Base spacing unit (default 4); generates the full `Space*` scale, scaled by the `DefaultDensity` mode. Construction-time.               |
| `DefaultDensity`      | `Density` | Density mode that scales the spacing base unit (`Compact` ×0.75, `Regular` ×1, `Comfy` ×1.25). Construction-time.                       |

These properties are defined on `BaseTheme` and inherited by `MaterialTheme`, `SimpleTheme`, and their toolkit wrappers (`MaterialToolkitTheme`, `SimpleToolkitTheme`). All are construction-time settings — see the note above. Color configuration lives on the separate `Colors` property (`ThemeColors`), which *does* support runtime changes — see [Seed Color Palette](seed-colors.md).

### Density Modes

The `DefaultDensity` property controls the spacing density of all controls.
It is a *mode*, not a value: it scales the `DefaultSpacing` base unit (effective base = `DefaultSpacing × factor`), adjusting padding and margins (Space* tokens) while keeping control heights and icon sizes constant. The two axes are orthogonal — a branded base unit and a density mode compose freely. The fixed tokens (`ControlHeight*`, `IconSize*`, `TouchTargetMinSize`) never change across density modes.

| DefaultDensity      | Factor | Base at default spacing (4) | Feel                               |
|---------------------|:------:|:---------------------------:|------------------------------------|
| `Compact`           | ×0.75  |              3              | Tighter padding for data-dense UIs |
| `Regular` (default) |   ×1   |              4              | Balanced spacing                   |
| `Comfy`             | ×1.25  |              5              | More generous padding              |

```xml
<!-- App.xaml — Material with compact density -->
<MaterialTheme xmlns="using:Uno.Material" DefaultDensity="Compact" />

<!-- App.xaml — Simple: branded 6px base unit in comfortable mode (effective base 7.5) -->
<SimpleTheme xmlns="using:Uno.Simple" DefaultSpacing="6" DefaultDensity="Comfy" />
```

Pick the density where the theme is declared. Switching it at runtime does not restyle existing or newly-created controls — see [Via Scalar Properties](#via-scalar-properties).

### Typography Font Swap

To change the font for an entire app, override the root typeface token:

```xml
<!-- MyTypography.xaml -->
<ResourceDictionary xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
                    xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml">
    <FontFamily x:Key="DefaultFontFamily">ms-appx:///Fonts/MyFont.ttf#MyFont</FontFamily>
</ResourceDictionary>
```

This cascades to all type-scale `FontFamily` keys (`DisplayLargeFontFamily`, `BodyMediumFontFamily`, etc.) without needing to override each one individually. Point it at a family that resolves multiple weights — a variable font, or a font shipping a font manifest — so the per-scale `*FontWeight` tokens (`DisplayLargeFontWeight`, …) render as designed.
