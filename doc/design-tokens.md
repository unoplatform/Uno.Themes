---
uid: Uno.Themes.DesignTokens
---

# Design Tokens & Override Surface

Uno.Themes exposes **shared design tokens** — semantic XAML resources for typography, spacing, shape (corner radius), and density (control height / icon size). An override affects the templates that consume that token. Material and Simple consume many tokens directly or through control-specific resources. Fluent exposes the tokens for app content, but its built-in templates use native Fluent keys; only selected theme properties are translated to those keys. See [Semantic Styles](semantic-styles.md#override-compatibility) for override scope and compatibility.

## Token Categories

### Typography

A single theme property, `DefaultFontFamily`, generates all type-scale family keys. The corresponding resource token is:

| Key | Default | Role |
| --------------------- | --------------------------------------------------------------- | ------------------ |
| `DefaultFontFamily` | Segoe UI (Material: Roboto, Simple: Inter, Cupertino: SF Pro, Fluent: the platform default via `ContentControlThemeFontFamily`) | Every type scale |

Per-scale variation is expressed through the `*FontWeight` tokens, not through separate font
families: the root points at a single family whose weights resolve from one reference (a variable
font, or a font with a [font manifest](https://platform.uno/docs/articles/features/custom-fonts.html#variable-fonts-and-font-manifest)
on platforms without variable-font support).

Cupertino has no semantic type scale: its control styles consume `CupertinoFontFamily`, an alias of the root, and that remains the key to override there — see [Change Default Font](cupertino-getting-started.md#change-default-font).

> [!IMPORTANT]
> **Breaking change**: the former `TypefacePlain` / `TypefaceBrand` token pair (and Simple's
> `SimpleFontFamily` and per-weight `SimpleRegular/Medium/SemiBold/BoldFontFamily` keys) have been removed. Override
> `DefaultFontFamily` instead; per-scale weight nuance is carried by the `*FontWeight` tokens.

The root token is also settable as a `DefaultFontFamily` property on the theme — see [Typography Font Swap](#typography-font-swap).

Per-scale keys follow the pattern `{Role}{Size}FontFamily`, `{Role}{Size}FontSize`, `{Role}{Size}FontWeight`, `{Role}{Size}CharacterSpacing` — for example `DisplayLargeFontFamily`, `BodyMediumFontSize`.

Character-spacing availability varies by slot; the portable keys are listed in [Semantic Typography](semantic-styles.md#typography). Family, size, and character-spacing resources have types `FontFamily`, `double`, and `int`. Weight resources are declared as strings such as `Normal` or `SemiBold`, converted when applied to `FontWeight` setters.

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

For variants `0`, `050`, `100`, `150`, `200`, `300`, `400`, `500`, `600`, and `800`, the generator also provides `HorizontalThickness`, `VerticalThickness`, `TopThickness`, `BottomThickness`, `LeftThickness`, and `RightThickness` companions. For example, `Space200HorizontalThickness` is `(8,0,8,0)` at default spacing. Larger variants provide only the uniform `Thickness` companion. A scalar override such as `Space200` does not recompute these separate resources; override each consumed companion or set the theme property to regenerate the scale.

### Shape (Corner Radius)

| Key          | Value (px) | CornerRadius Key         |
|--------------|------------|--------------------------|
| `Radius0`    | 0          | `Radius0CornerRadius`    |
| `Radius050`  | 2          | `Radius050CornerRadius`  |
| `Radius100`  | 4          | `Radius100CornerRadius`  |
| `Radius200`  | 8          | `Radius200CornerRadius`  |
| `Radius300`  | 12         | `Radius300CornerRadius`  |
| `Radius400`  | 16         | `Radius400CornerRadius`  |
| `Radius500`  | 20         | `Radius500CornerRadius`  |
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

This generates all `Radius*` / `Space*` tokens as multiples of the base value. The same properties are available on `SimpleTheme` and `FluentTheme`. Fluent's native templates do not consume `Space*`, so changing `DefaultSpacing` or `DefaultDensity` changes semantic resources and app content that uses them, but does not resize stock Fluent control padding.

Use finite, non-negative values. Negative, NaN, and infinite values for either `DefaultSpacing` or `DefaultCornerRadius` fall back to the default base of 4. Corner-radius inputs that would overflow the largest scale token also fall back to 4. The semantic shape scale and Fluent's native-radius mapping use the same normalized radius. Zero is valid: it produces square corners for scaled radius tokens; `RadiusFull` remains 9999.

For spacing, the [density mode](#density-modes) (`DefaultDensity`) composes with the base unit rather than replacing it: the effective spacing base is `DefaultSpacing × density factor` (`Compact` ×0.75, `Regular` ×1, `Comfy` ×1.25). With the default base of 4, the modes yield 3 / 4 / 5.

> [!NOTE]
> `DefaultCornerRadius`, `DefaultSpacing`, and `DefaultDensity` are **runtime-settable**. Assigning one regenerates the `Radius*` / `Space*` token resources, and controls created afterwards pick the new scale up through their styles.
>
> Controls **already on screen** keep the values they resolved when they loaded: these tokens are `CornerRadius` / `Thickness` / `double` **values**, so unlike a seed color — whose brushes are live instances the theme rewrites in place, see [Runtime Seed Color Changes](seed-colors.md#runtime-seed-color-changes) — there is nothing to mutate. Their styles read the tokens through `{ThemeResource}`, so a theme-change pass re-resolves them: toggle the root element's `RequestedTheme` away from its `ActualTheme` and back, or recreate the root content.

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

| Property | Type | Description |
| ----------------------- | -------------- | --------------------------------------------------------------------------------------------------------------------------------------------- |
| `DefaultCornerRadius` | `double` | Base corner radius unit; generates the full `Radius*` scale. Runtime-settable. Under `FluentTheme` it also re-points the platform `ControlCornerRadius` / `OverlayCornerRadius` tokens the built-in templates read. |
| `DefaultSpacing` | `double` | Base spacing unit (default 4); generates the full `Space*` scale, scaled by the `DefaultDensity` mode. Runtime-settable. |
| `DefaultDensity` | `Density` | Density mode that scales the spacing base unit (`Compact` ×0.75, `Regular` ×1, `Comfy` ×1.25). Runtime-settable. |
| `DefaultFontFamily` | `FontFamily` | The font the type scale is generated from: the `DefaultFontFamily` token and every `*FontFamily` key derived from it. Runtime-settable. |

These properties are defined on `BaseTheme` and inherited by `MaterialTheme`, `SimpleTheme`, `FluentTheme`, and the Material/Simple toolkit wrappers (`MaterialToolkitTheme`, `SimpleToolkitTheme`). All four properties regenerate their tokens when assigned at runtime; content reading those tokens through `{ThemeResource}` re-resolves on a theme-change pass — see the note above and [Typography Font Swap](#typography-font-swap). Fluent maps an explicitly set `DefaultCornerRadius` to `ControlCornerRadius` and twice that value to `OverlayCornerRadius`, and maps `DefaultFontFamily` to `ContentControlThemeFontFamily`. Other platform-specific measurements remain native Fluent resources.

### Density Modes

The `DefaultDensity` property controls the density of spacing-token consumers.
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

Switching it at runtime regenerates the `Space*` tokens; controls created afterwards use them, and controls already on screen re-resolve on a theme-change pass — see [Via Scalar Properties](#via-scalar-properties).

### Typography Font Swap

To change the font for an entire app, set one property on the theme:

```xml
<!-- App.xaml -->
<MaterialTheme DefaultFontFamily="ms-appx:///Fonts/MyFont.ttf#MyFont" />
```

That one value generates the `DefaultFontFamily` token and every type-scale `FontFamily` key
derived from it (`DisplayLargeFontFamily`, `BodyMediumFontFamily`, …), so nothing has to be
overridden individually. Left unset, the design system's own typeface stands. Point it at a family
that resolves multiple weights — a variable font, or a font shipping a font manifest — so the
per-scale `*FontWeight` tokens (`DisplayLargeFontWeight`, …) render as designed.

Like the scale measures above, this is **runtime-settable**: assigning it regenerates the derived
family keys, and text laid out afterwards picks the new font up. Text **already on screen** keeps the
family it resolved when it loaded — a `FontFamily` is an immutable value, so unlike a seed color
(whose brushes are live instances the theme rewrites in place) there is nothing to mutate. To move
text that is already rendered, run a theme-change pass (toggle the root's `RequestedTheme` away from
its `ActualTheme` and back), recreate the root content, or trigger the application-wide resource
refresh a hot reload performs.

> [!NOTE]
> This covers text the design system styles. A `TextBlock` with no style resolves the framework's own
> default instead — `FeatureConfiguration.Font.DefaultTextFontFamily`, which the Uno.Sdk sets to Open
> Sans on non-Windows targets — and `DefaultFontFamily` does not touch it. Set both if an app mixes
> styled and unstyled text.

For Material or Simple merged at application scope, with the `DefaultFontFamily` property unset,
the root token can instead be overridden in a font dictionary:

```xml
<!-- MyTypography.xaml, referenced as FontOverrideSource on the theme -->
<ResourceDictionary xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
                    xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml">
    <FontFamily x:Key="DefaultFontFamily">ms-appx:///Fonts/MyFont.ttf#MyFont</FontFamily>
</ResourceDictionary>
```

Reference the file as the theme's `FontOverrideSource` (`<MaterialTheme FontOverrideSource="ms-appx:///MyTypography.xaml" />`, likewise on `SimpleTheme`). Material/Simple slot aliases then resolve the root at application scope. A page-scoped root override does not provide the same alias cascade.

Fluent supports the same root-resource override when the `DefaultFontFamily` property is unset: it supplies concrete family values for every semantic slot and for the native `ContentControlThemeFontFamily` resource. Explicit slot keys, such as `BodyMediumFontFamily` in `FontOverrideDictionary` or `FontOverrideSource`, take precedence for that slot. Native Fluent controls use `ContentControlThemeFontFamily`, rather than all of the semantic type slots.

A font override wins **for each key it declares**: a key present in both the override and the generated layer takes the override value. Once the `DefaultFontFamily` property generates concrete slot values, overriding only the root key does not replace those slot values; override the individual slot keys as well or keep the property unset. A source-backed font dictionary is cached across unrelated rebuilds and re-read when reassigned or invalidated by hot reload.
