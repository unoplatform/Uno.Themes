# Seed Color-Based Algorithmic Color Palette Generation

**Issue:** #1656
**Status:** Implemented
**Branch:** `dev/mara/seed-color-palette`

## Overview

This feature allows users to supply a single **seed color** that algorithmically derives the full semantic color palette (Primary, Secondary, Tertiary, Error, Surface, Outline, etc.) for both Light and Dark themes, without manually defining every color resource.

The implementation follows the [Material Design 3 color system](https://m3.material.io/styles/color/the-color-system/key-colors-tones) and uses the **HCT (Hue-Chroma-Tone)** color space, ported from [material-color-utilities](https://github.com/material-foundation/material-color-utilities) (Apache 2.0).

## API

### XAML

```xml
<!-- Single seed color generates the entire palette -->
<MaterialTheme xmlns="using:Uno.Material"
               PrimarySeedColor="#6750A4" />

<!-- Optional secondary and tertiary seeds for more control -->
<MaterialTheme xmlns="using:Uno.Material"
               PrimarySeedColor="#6750A4"
               SecondarySeedColor="#625B71"
               TertiarySeedColor="#7D5260" />

<!-- Seed + manual overrides (overrides win) -->
<MaterialTheme xmlns="using:Uno.Material"
               PrimarySeedColor="#6750A4"
               ColorOverrideSource="ms-appx:///ColorPaletteOverride.xaml" />
```

### C#

```csharp
// Simplest runtime usage via static helper
SemanticThemeHelper.PrimarySeedColor = Colors.Green;

// Or configure at initialization
var theme = new MaterialTheme
{
    PrimarySeedColor = Color.FromArgb(0xFF, 0x67, 0x50, 0xA4),
};

// Runtime change directly on the theme
theme.PrimarySeedColor = Color.FromArgb(0xFF, 0x00, 0x6A, 0x6A);
```

### Properties on `BaseTheme`

| Property | Type | Default | Description |
|---|---|---|---|
| `PrimarySeedColor` | `Color?` | `null` | Primary seed color. When set, generates all color roles algorithmically. |
| `SecondarySeedColor` | `Color?` | `null` | Optional. Overrides the auto-derived secondary palette. |
| `TertiarySeedColor` | `Color?` | `null` | Optional. Overrides the auto-derived tertiary palette. |
| `ColorOverrideSource` | `string` | `null` | URI to a color override ResourceDictionary. |
| `ColorOverrideDictionary` | `ResourceDictionary` | `null` | Direct color override dictionary. Highest precedence. |
| `DefaultPrimarySeed` | `Color?` (virtual) | `null` | Theme-specific default primary seed. `MaterialTheme` returns `#5946D2`; `SimpleTheme` returns `null`. When non-null, seed generation is always active even without an explicit `PrimarySeedColor`. |

### Default Seed Behavior

`MaterialTheme` provides a default primary seed so that seed color generation is always active — even when no `PrimarySeedColor` is explicitly set:

- **`MaterialTheme`**: Default seed `#5946D2` (deep purple), producing a vibrant Material palette.
- **`SimpleTheme`**: No default seed; ships its hand-crafted grayscale palette. Setting `PrimarySeedColor` opts into seed-driven generation in high-fidelity mode (preserves source chroma, so low-chroma seeds stay neutral).

Setting `PrimarySeedColor` overrides the default in either theme. Clearing it (`null`) reverts to the theme's `DefaultPrimarySeed`.

### `SemanticThemeHelper` (static convenience API)

| Member | Type | Description |
|---|---|---|
| `GetTheme()` | `BaseTheme` | Returns the `BaseTheme` from `Application.Current.Resources`, or `null`. |
| `PrimarySeedColor` | `Color?` | Gets/sets the primary seed color on the active theme. |
| `SecondarySeedColor` | `Color?` | Gets/sets the secondary seed color on the active theme. |
| `TertiarySeedColor` | `Color?` | Gets/sets the tertiary seed color on the active theme. |

Throws `InvalidOperationException` if no `BaseTheme` is found in application resources.

## How It Works

### Color Space: HCT

HCT (Hue-Chroma-Tone) combines:
- **Hue** (0-360): The color's position on the color wheel (from CAM16)
- **Chroma** (0+): How vivid/saturated the color is (from CAM16)
- **Tone** (0-100): Perceptual lightness (CIE L*)

The CAM16 model operates in CIE XYZ. The conversion pipeline is:
- **Forward (sRGB → HCT):** sRGB → linearize → XYZ → M16 → CAM16 adapted responses → hue/chroma; XYZ → L* (tone)
- **Inverse (HCT → sRGB):** hue/chroma/J → CAM16 adapted responses → M16⁻¹ → XYZ → linearize⁻¹ → sRGB

This color space is perceptually uniform — equal steps in tone produce equal steps in perceived lightness — which is critical for generating accessible color palettes.

### Palette Derivation

From the seed color's HCT Hue (**H**) and Chroma (**C**), using the **TonalSpot** scheme (the standard M3 variant used by Android):

| Tonal Palette | Hue | Chroma | Purpose |
|---|---|---|---|
| **Primary** | H | max(C, 48) | Main brand color, boosted for vibrancy |
| **Secondary** | H | 16 | Desaturated complement for supporting roles |
| **Tertiary** | H + 60 | 24 | Hue-shifted accent for visual interest |
| **Neutral** | H | 4 | Near-gray for backgrounds and surfaces |
| **Neutral Variant** | H | 8 | Slightly tinted for outlines and surface variants |

Fixed chroma values for Secondary through NeutralVariant (matching M3 TonalSpot) ensure consistent visual separation between color roles across all seed colors.

Error colors are **not generated** from the seed — they use the fixed values from `SharedColorPalette.xaml`, matching the Material Theme Builder behavior where Error always stays red regardless of seed color.

If `SecondarySeed` or `TertiarySeed` are provided, those palettes use the provided color's HCT values instead of the fixed defaults.

### Tone Mapping

Each tonal palette maps to semantic color roles at specific tone levels:

| Semantic Role | Light Tone | Dark Tone | Palette |
|---|---|---|---|
| Primary | 40 | 80 | Primary |
| OnPrimary | 100 | 20 | Primary |
| PrimaryContainer | 90 | 30 | Primary |
| OnPrimaryContainer | 10 | 90 | Primary |
| PrimaryInverse | 80 | 40 | Primary |
| PrimaryVariantDark | 30 | 30 | Primary |
| PrimaryVariantLight | 70 | 80 | Primary |
| Secondary | 40 | 80 | Secondary |
| OnSecondary | 100 | 20 | Secondary |
| SecondaryContainer | 90 | 30 | Secondary |
| OnSecondaryContainer | 10 | 90 | Secondary |
| SecondaryVariantDark | 30 | 30 | Secondary |
| SecondaryVariantLight | 70 | 80 | Secondary |
| Tertiary | 40 | 80 | Tertiary |
| OnTertiary | 100 | 20 | Tertiary |
| TertiaryContainer | 90 | 30 | Tertiary |
| OnTertiaryContainer | 10 | 90 | Tertiary |
| Background | 99 | 10 | Neutral |
| OnBackground | 10 | 90 | Neutral |
| Surface | 99 | 20 | Neutral |
| OnSurface | 10 | 90 | Neutral |
| SurfaceVariant | 90 | 30 | Neutral Variant |
| OnSurfaceVariant | 30 | 80 | Neutral Variant |
| SurfaceInverse | 20 | 90 | Neutral |
| OnSurfaceInverse | 95 | 20 | Neutral |
| SurfaceTint | 40 | 80 | Primary |
| Outline | 50 | 60 | Neutral Variant |
| OutlineVariant | 80 | 30 | Neutral Variant |
| Shadow | #33000000 | #33000000 | Fixed |

### Accessibility

The tone pairings are chosen to guarantee **WCAG AA contrast** (minimum 4.5:1):
- Role/OnRole pairs (40/100 light, 80/20 dark) achieve ~7:1 contrast
- Container/OnContainer pairs (90/10 light, 30/90 dark) achieve ~7:1 contrast
- Background/OnBackground and Surface/OnSurface pairs similarly exceed 4.5:1

### Override Precedence

Colors resolve in this order (last wins):

```
SharedColorPalette.xaml (built-in defaults)
    ↓ overridden by
GetDefaultColorPalette() (theme-specific base colors, e.g. SimpleTheme's grayscale palette)
    ↓ overridden by
Seed-generated palette (if PrimarySeedColor or DefaultPrimarySeed is set)
    ↓ overridden by
ColorOverrideDictionary (explicit user overrides)
```

This means:
- Theme-specific base colors (e.g. SimpleTheme's palette) serve as defaults but are overridden by the seed
- Setting `PrimarySeedColor` replaces all default and theme-specific palette colors
- Setting `ColorOverrideDictionary` on top of `PrimarySeedColor` lets you fine-tune specific roles

## Architecture

### New Files

```
src/library/Uno.Themes/
    Helpers/SemanticThemeHelper.cs  # Static convenience API for runtime seed color changes
    ColorGeneration/
        ColorMath.cs                # sRGB/Linear/XYZ/L* conversions
        TonalPalette.cs             # Hue+chroma → tone-indexed ARGB lookup
        SeedColorPaletteGenerator.cs # Orchestrator: seed → ResourceDictionary (with caching)
        Hct/
            Cam16.cs                # CAM16 color appearance model
            HctColor.cs             # Public HCT struct
            HctSolver.cs            # Bisection solver: HCT → ARGB
            ViewingConditions.cs    # Standard sRGB viewing conditions
```

### Modified Files

- `src/library/Uno.Themes/BaseTheme.cs` — Added `PrimarySeedColor`, `SecondarySeedColor`, `TertiarySeedColor` dependency properties directly on the theme; updated `UpdateSource()` to generate and merge the seed palette with runtime brush tracking. All seed and override changes trigger a full `UpdateSource()` rebuild — there is no separate fast path for seed changes, because the in-place patch left underlying Color resources stale and only updated cached brush instances.

### Sample Page

- `src/samples/SamplesApp.Shared/Content/Styles/SeedColorSamplePage.xaml(.cs)` — Interactive demo with a `ColorPicker` that live-generates and displays semantic color roles and a controls preview for any seed color. Accessible in both MaterialSampleApp and SimpleSampleApp under **Styles > Seed Color**.

### Tests

- `src/samples/SimpleSampleApp/RuntimeTests/Given_SeedColorPalette.cs` — MSTest runtime tests covering:
  - **HCT round-trip fidelity**: ARGB → HCT → ARGB preserves color within ±20 per channel for realistic seed colors (the simplified bisection solver has larger errors at extreme gamut boundaries)
  - **HCT value correctness**: Known colors produce expected hue/chroma/tone ranges
  - **Tonal palette monotonicity**: L* increases with tone; tone 0 = black, tone 100 = white
  - **Tone accuracy**: Generated colors match their target L* within ±2
  - **WCAG AA contrast**: All role/on-role pairings exceed 4.5:1 contrast ratio

### Public API Surface

Two types are made public for advanced C# usage:

- `Uno.Themes.ColorGeneration.Hct.HctColor` — Convert between ARGB and HCT color space
- `Uno.Themes.ColorGeneration.TonalPalette` — Generate tonal palette colors at any tone level

All other color math internals (`Cam16`, `HctSolver`, `ViewingConditions`, `ColorMath`) remain `internal`.

## Dependencies

- **Zero external packages.** All HCT color math is implemented in pure C# within the `Uno.Themes` library.
- Works across all Uno Platform targets: WinUI Desktop, iOS, Android, macOS, WebAssembly.

## Performance

Generating the full palette (~78 HCT solves for all tones across 6 palettes) completes in under 1ms on modern hardware. The `TonalPalette` class caches computed tones, so repeated lookups are free. `SeedColorPaletteGenerator` caches the most recent seed → palette result, so regenerating with the same seed returns instantly without unbounded memory growth during color picker dragging.

## Runtime Color Updates

All runtime seed and override changes trigger a full `UpdateSource()` rebuild:

- Changing `PrimarySeedColor`, `SecondarySeedColor`, or `TertiarySeedColor` on the theme.
- Changing `ColorOverrideDictionary` on the theme (or `ColorOverrideSource`, which forwards to `ColorOverrideDictionary`).

`UpdateSource()` clears and rebuilds the merged dictionaries — `GetDefaultColorPalette()` first, then the seed-generated palette, then `ColorOverrideDictionary` last (highest precedence). It also captures `SolidColorBrush` instances created before the rebuild and patches their `Color` values to the new palette so UI elements that already hold brush references update in place. This preserves visual continuity even though the underlying resource tree has been rebuilt.

An earlier "fast path" (`UpdateSeedColors`) patched brushes in-place without rebuilding the tree — it was removed because the underlying `Color` resources in `MergedDictionaries` stayed at the old values, so any consumer resolving via `{StaticResource ColorKey}` (or any brush built after the seed change) read stale colors. The full rebuild is the only correct path.

## Compatibility

- The seed properties (`PrimarySeedColor`, `SecondarySeedColor`, `TertiarySeedColor`) default to `null`. Themes that don't set them behave identically to before — `MaterialTheme` still uses its `DefaultPrimarySeed` (`#5946D2`); `SimpleTheme` ships its grayscale palette.
- **All theme subclasses:** The properties live on `BaseTheme`, so `MaterialTheme`, `SimpleTheme`, and `CupertinoTheme` all inherit the capability.

## References

- [Material Theme Builder](https://material-foundation.github.io/material-theme-builder/)
- [Material Color Utilities — HCT](https://github.com/nickvdyck/material-color-utilities)
- [Material Design 3 — Color System](https://m3.material.io/styles/color/the-color-system/key-colors-tones)
- [CAM16 Color Appearance Model](https://doi.org/10.1002/col.22131)
