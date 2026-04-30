# Design Tokens & Override Surface

Uno.Themes exposes a set of **shared design tokens** — semantic XAML resources for typography, spacing, shape (corner radius), and density (control height / icon size). These tokens are consumed by all control templates, so overriding a single token key globally affects every control that references it. Control density is driven by the spacing scale (`DefaultSpacing`) rather than a separate preset system.

## Token Categories

### Typography

Root typeface tokens cascade to all type-scale keys:

| Key | Default | Role |
|-----|---------|------|
| `TypefacePlain` | Segoe UI (Material: Roboto Regular, Simple: Inter) | Display, Headline |
| `TypefaceBrand` | Segoe UI (Material: Roboto Medium, Simple: Inter) | Title, Label, Body, Caption |

Per-scale keys follow the pattern `{Role}{Size}FontFamily`, `{Role}{Size}FontSize`, `{Role}{Size}FontWeight`, `{Role}{Size}CharacterSpacing` — for example `DisplayLargeFontFamily`, `BodyMediumFontSize`.

### Spacing

| Key | Value (px) | Thickness Key |
|-----|-----------|---------------|
| `Space0` | 0 | `Space0Thickness` |
| `Space050` | 2 | `Space050Thickness` |
| `Space100` | 4 | `Space100Thickness` |
| `Space150` | 6 | `Space150Thickness` |
| `Space200` | 8 | `Space200Thickness` |
| `Space300` | 12 | `Space300Thickness` |
| `Space400` | 16 | `Space400Thickness` |
| `Space500` | 20 | `Space500Thickness` |
| `Space600` | 24 | `Space600Thickness` |
| `Space800` | 32 | `Space800Thickness` |
| `Space1200` | 48 | `Space1200Thickness` |
| `Space1600` | 64 | `Space1600Thickness` |
| `Space2400` | 96 | `Space2400Thickness` |
| `Space4000` | 160 | `Space4000Thickness` |

### Shape (Corner Radius)

| Key | Value (px) | CornerRadius Key |
|-----|-----------|-----------------|
| `Radius0` | 0 | `Radius0CornerRadius` |
| `Radius050` | 2 | `Radius050CornerRadius` |
| `Radius100` | 4 | `Radius100CornerRadius` |
| `Radius200` | 8 | `Radius200CornerRadius` |
| `Radius300` | 12 | `Radius300CornerRadius` |
| `Radius400` | 16 | `Radius400CornerRadius` |
| `Radius700` | 28 | `Radius700CornerRadius` |
| `RadiusFull` | 9999 | `RadiusFullCornerRadius` |

### Density

| Key | Default Value (px) |
|-----|-------------------|
| `ControlHeightSmall` | 32 |
| `ControlHeightMedium` | 40 |
| `ControlHeightMediumLarge` | 44 |
| `ControlHeightLarge` | 48 |
| `TouchTargetMinSize` | 48 |
| `IconSizeSmall` | 16 |
| `IconSizeMedium` | 24 |
| `IconSizeLarge` | 32 |

## Overriding Tokens

### Via Scalar Properties

Set `DefaultCornerRadius` or `DefaultSpacing` on the theme to generate an entire scale from a single base value:

```xml
<!-- App.xaml -->
<MaterialTheme DefaultCornerRadius="4" DefaultSpacing="6" />
```

This generates all `Radius*` / `Space*` tokens as multiples of the base value. The same properties are available on `SimpleTheme`.

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
|----------|------|-------------|
| `DefaultCornerRadius` | `double` | Base corner radius unit; generates the full `Radius*` scale |
| `DefaultSpacing` | `double` | Base spacing unit; generates the full `Space*` scale |

These properties are defined on `BaseTheme` and inherited by `MaterialTheme`, `SimpleTheme`, and their toolkit wrappers (`MaterialToolkitTheme`, `SimpleToolkitTheme`).

### Density via DefaultSpacing

Control density (heights, padding) is driven by the **spacing scale**. Adjusting `DefaultSpacing` on the theme rescales every `Space*` token, which in turn changes control padding, margins, and layout. Combined with the fixed `ControlHeight*` tokens, this gives you compact-to-comfortable density without a separate preset system.

| DefaultSpacing | Feel |
|----------------|------|
| `3` | Compact — tighter padding for data-dense UIs |
| `4` | Regular (default) — balanced spacing |
| `5` | Comfortable — more generous padding |

```xml
<!-- App.xaml — opt into compact density -->
<SimpleTheme DefaultSpacing="3" />
```

### Typography Font Swap

To change the font for an entire app, override the root typeface tokens:

```xml
<!-- MyTypography.xaml -->
<ResourceDictionary xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
                    xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml">
    <FontFamily x:Key="TypefacePlain">ms-appx:///Fonts/MyFont-Regular.ttf#MyFont</FontFamily>
    <FontFamily x:Key="TypefaceBrand">ms-appx:///Fonts/MyFont-Medium.ttf#MyFont</FontFamily>
</ResourceDictionary>
```

This cascades to all type-scale `FontFamily` keys (`DisplayLargeFontFamily`, `BodyMediumFontFamily`, etc.) without needing to override each one individually.
