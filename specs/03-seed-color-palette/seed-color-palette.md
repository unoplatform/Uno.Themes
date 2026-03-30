# Seed Color-Based Algorithmic Color Palette Generation

**Issue:** #1656
**Status:** Implemented
**Branch:** `dev/mara/seed-color-palette`

## Overview

This feature allows users to supply a single **seed color** that algorithmically derives the full semantic color palette (Primary, Secondary, Tertiary, Error, Surface, Outline, etc.) for both Light and Dark themes, without manually defining every color resource.

The implementation follows the [Material Design 3 color system](https://m3.material.io/styles/color/the-color-system/key-colors-tones) and uses the **HCT (Hue-Chroma-Tone)** color space from [Material Color Utilities](https://github.com/nickvdyck/material-color-utilities).

## API

### XAML

```xml
<!-- Single seed color generates the entire palette -->
<MaterialTheme xmlns="using:Uno.Material"
               SeedColor="#6750A4" />

<!-- Optional secondary and tertiary seed colors for more control -->
<MaterialTheme xmlns="using:Uno.Material"
               SeedColor="#6750A4"
               SecondarySeedColor="#625B71"
               TertiarySeedColor="#7D5260" />

<!-- Seed color + manual overrides (overrides win) -->
<MaterialTheme xmlns="using:Uno.Material"
               SeedColor="#6750A4"
               ColorOverrideSource="ms-appx:///ColorPaletteOverride.xaml" />
```

### C#

```csharp
// Set at initialization
var theme = new MaterialTheme();
theme.SeedColor = Color.FromArgb(0xFF, 0x67, 0x50, 0xA4);

// Change at runtime (triggers full palette regeneration)
theme.SeedColor = Color.FromArgb(0xFF, 0x00, 0x6A, 0x6A);
```

### Properties

| Property | Type | Default | Description |
|---|---|---|---|
| `SeedColor` | `Color?` | `null` | Primary seed color. When set, generates all color roles. |
| `SecondarySeedColor` | `Color?` | `null` | Optional. Overrides the auto-derived secondary palette. |
| `TertiarySeedColor` | `Color?` | `null` | Optional. Overrides the auto-derived tertiary palette. |

All three properties are defined on `BaseTheme`, so they work with `MaterialTheme`, `SimpleTheme`, and any future theme subclass.

## How It Works

### Color Space: HCT

HCT (Hue-Chroma-Tone) combines:
- **Hue** (0-360): The color's position on the color wheel (from CAM16)
- **Chroma** (0+): How vivid/saturated the color is (from CAM16)
- **Tone** (0-100): Perceptual lightness (CIE L*)

This color space is perceptually uniform — equal steps in tone produce equal steps in perceived lightness — which is critical for generating accessible color palettes.

### Palette Derivation

From the seed color's HCT values (Hue **H**, Chroma **C**):

| Tonal Palette | Hue | Chroma | Purpose |
|---|---|---|---|
| **Primary** | H | max(C, 48) | Main brand color, boosted for vibrancy |
| **Secondary** | H | C / 3 | Desaturated complement for supporting roles |
| **Tertiary** | H + 60 | C / 2 | Hue-shifted accent for visual interest |
| **Neutral** | H | min(C/12, 4) | Near-gray for backgrounds and surfaces |
| **Neutral Variant** | H | min(C/6, 8) | Slightly tinted for outlines and surface variants |
| **Error** | 25 | 84 | Fixed red, independent of seed |

If `SecondarySeedColor` or `TertiarySeedColor` are provided, those palettes use the provided color's HCT values instead of the auto-derived ones.

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
| Error | 40 | 80 | Error |
| OnError | 100 | 20 | Error |
| ErrorContainer | 90 | 30 | Error |
| OnErrorContainer | 10 | 90 | Error |
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
Seed-generated palette (if SeedColor is set)
    ↓ overridden by
ColorOverrideDictionary / ColorOverrideSource (explicit user overrides)
```

This means:
- Setting `SeedColor` replaces all default palette colors
- Setting `ColorOverrideDictionary` on top of `SeedColor` lets you fine-tune specific roles
- Not setting `SeedColor` preserves the existing behavior (no breaking change)

## Architecture

### New Files

```
src/library/Uno.Themes/ColorGeneration/
    ColorMath.cs                    # sRGB/Linear/XYZ/L* conversions
    TonalPalette.cs                 # Hue+chroma → tone-indexed ARGB lookup
    SeedColorPaletteGenerator.cs    # Orchestrator: seed → ResourceDictionary
    Hct/
        Cam16.cs                    # CAM16 color appearance model
        HctColor.cs                 # Public HCT struct
        HctSolver.cs               # Bisection solver: HCT → ARGB
        ViewingConditions.cs        # Standard sRGB viewing conditions
```

### Modified Files

- `src/library/Uno.Themes/BaseTheme.cs` — Added `SeedColor`, `SecondarySeedColor`, `TertiarySeedColor` dependency properties; updated `UpdateSource()` to generate and merge the seed palette.

### Sample Page

- `src/samples/SamplesApp.Shared/Content/Styles/SeedColorSamplePage.xaml(.cs)` — Interactive demo with a `ColorPicker` (spectrum ring + value slider) that live-generates and displays the full tonal palette grid and semantic color role cards (Light + Dark) for any seed color. Accessible in both MaterialSampleApp and SimpleSampleApp under **Styles > Seed Color**.

### Tests

- `src/samples/SimpleSampleApp/RuntimeTests/Given_SeedColorPalette.cs` — MSTest runtime tests covering:
  - **HCT round-trip fidelity**: ARGB → HCT → ARGB preserves color within ±1 per channel
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

Generating the full palette (~78 HCT solves for all tones across 6 palettes) completes in under 1ms on modern hardware. The `TonalPalette` class caches computed tones, so repeated lookups are free.

## Compatibility

- **Non-breaking:** The `SeedColor` property defaults to `null`. Existing themes that don't set it behave identically to before.
- **All theme subclasses:** The properties live on `BaseTheme`, so `MaterialTheme`, `SimpleTheme`, and `CupertinoTheme` all inherit the capability.

## References

- [Material Theme Builder](https://material-foundation.github.io/material-theme-builder/)
- [Material Color Utilities — HCT](https://github.com/nickvdyck/material-color-utilities)
- [Material Design 3 — Color System](https://m3.material.io/styles/color/the-color-system/key-colors-tones)
- [CAM16 Color Appearance Model](https://doi.org/10.1002/col.22131)
