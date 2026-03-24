# Simple Theme: Migration to Shared Semantic Brushes

This spec proposes refactoring the Simple theme to use **only** the shared semantic color/brush system (`SharedColorPalette.xaml` + `SharedColors.xaml`) — the same foundation Material uses — eliminating the parallel SDS-specific primitive scales and brush definitions from `ColorPalette.xaml`.

## Status

**Draft** — awaiting review before implementation.

## Scope Decisions

- **Positive (Green) and Warning (Yellow) roles are excluded.** These SDS color families have zero downstream consumers in Simple control styles today and will be removed from `ColorPalette.xaml` entirely rather than mapped to shared brushes.
- **Neutral maps to the existing Surface family.** No new color roles are added to SharedColorPalette or SharedColors. SDS Neutral semantics are covered by Surface/SurfaceVariant/SurfaceInverse and their "On" counterparts.
- **No extensions to SharedColorPalette.xaml or SharedColors.xaml are required.** Simple uses only overrides of existing color keys.

## Motivation

Today Simple maintains **~80 primitive colors** (Brand, Gray, Slate, Red, Pink scales) and **~120 SDS semantic brushes** in its own `ColorPalette.xaml`, entirely separate from the shared system. Material, by contrast, defines only ~12 color overrides in `SharedColorPalette.xaml` and derives all brushes from `SharedColors.xaml`. This creates:

1. **Duplicated architecture** — two parallel color pipelines with different naming, different state strategies (distinct colors vs. opacity), and different brush generation.
2. **Inconsistent extensibility** — app developers can override `SharedColorPalette` colors for Material but must target a completely different set of keys for Simple.
3. **Maintenance burden** — every new control needs brushes wired in both vocabularies.

The goal is **one shared semantic brush vocabulary** across all themes, with each theme providing only its color palette overrides.

---

## Architecture Overview

### Current State

```
Material Theme                         Simple Theme
─────────────                         ─────────────
SharedColorPalette.xaml  ◄──────┐     ColorPalette.xaml (628 lines)
  ~32 Color definitions         │       ~80 Simple* primitive Colors
        │                       │       ~120 Simple* semantic Brushes
        ▼                       │       + 12 SharedColorPalette overrides
SharedColors.xaml               │
  ~350 semantic Brushes         │     (SharedColors.xaml NOT used by
  (opacity-based states)        │      Simple control styles)
```

### Proposed State

```
All Themes
──────────
SharedColorPalette.xaml (no changes needed)
  ~32 Color definitions  ◄── Material overrides (existing)
        │                ◄── Simple overrides (~25 color keys)
        │                ◄── Cupertino overrides
        ▼
SharedColors.xaml (no changes needed)
  ~280 semantic Brushes (opacity-based states)
        │
        ▼
Simple ColorPalette.xaml (slim: only color overrides + alias brushes)
  ~25 Color overrides for SharedColorPalette
  ~72 alias Brushes mapping SDS names → Shared names (backward compat)
```

---

## Part 1: SharedColorPalette — Simple Overrides

No new color roles are needed. Simple provides overrides for existing SharedColorPalette keys:

| SharedColorPalette Key | Material Light | Material Dark | Simple Light | Simple Dark | SDS Mapping Notes |
|---|---|---|---|---|---|
| `PrimaryColor` | `#5946D2` | `#C7BFFF` | `#2C2C2C` | `#F5F5F5` | SDS Brand primary (Gray-based, not colored) |
| `OnPrimaryColor` | `#FFFFFF` | `#2A009F` | `#F5F5F5` | `#1E1E1E` | Text on Brand primary bg |
| `PrimaryContainerColor` | `#E5DEFF` | `#4129BA` | `#E6E6E6` | `#444444` | SDS Brand secondary bg |
| `OnPrimaryContainerColor` | `#170065` | `#E4DFFF` | `#1E1E1E` | `#E6E6E6` | Text on Brand secondary bg |
| `PrimaryInverseColor` | `#C8BFFF` | `#2A009F` | `#F5F5F5` | `#1E1E1E` | SDS Brand inverse |
| `PrimaryVariantDarkColor` | `#0021C1` | `#4128BA` | `#1E1E1E` | `#D9D9D9` | SDS Brand pressed/dark |
| `PrimaryVariantLightColor` | `#9679FF` | `#C8BFFF` | `#F5F5F5` | `#1E1E1E` | SDS Brand light/tertiary bg |
| `SecondaryColor` | `#6B4EA2` | `#CDC2DC` | `#757575` | `#949494` | Available for future use |
| `OnSecondaryColor` | `#FFFFFF` | `#332D41` | `#F3F3F3` | `#242424` | Text on Secondary |
| `SecondaryContainerColor` | `#EBDDFF` | `#433C52` | `#CDCDCD` | `#434343` | Secondary container bg |
| `OnSecondaryContainerColor` | `#220555` | `#EDDFFF` | `#242424` | `#F3F3F3` | Text on secondary container bg |
| `TertiaryColor` | `#0061A4` | `#9FCAFF` | `#767676` | `#949494` | Available for future use |
| `OnTertiaryColor` | `#FFFFFF` | `#003258` | `#FFFFFF` | `#242424` | Text on Tertiary |
| `TertiaryContainerColor` | `#CFE4FF` | `#00497D` | `#D9D9D9` | `#383838` | Tertiary container bg |
| `OnTertiaryContainerColor` | `#001D36` | `#D1E4FF` | `#1E1E1E` | `#E6E6E6` | Text on tertiary container bg |
| `ErrorColor` | `#B3261E` | `#FFB4AB` | `#EC221F` | `#C00F0C` | SDS Danger = Red500/600 |
| `OnErrorColor` | `#FFFFFF` | `#690005` | `#FEE9E7` | `#FEE9E7` | Text on Danger bg |
| `ErrorContainerColor` | `#F9DEDC` | `#93000A` | `#FDD3D0` | `#690807` | SDS Danger secondary bg |
| `OnErrorContainerColor` | `#410E0B` | `#FFDAD6` | `#300603` | `#FEE9E7` | Text on Danger secondary bg |
| `BackgroundColor` | `#FCFBFF` | `#1C1B1F` | `#F5F5F5` | `#1E1E1E` | SDS page background |
| `OnBackgroundColor` | `#1C1B1F` | `#E5E1E6` | `#1E1E1E` | `#FFFFFF` | Text on background |
| `SurfaceColor` | `#FFFFFF` | `#302D37` | `#FFFFFF` | `#1E1E1E` | SDS Default default bg |
| `OnSurfaceColor` | `#1C1B1F` | `#E6E1E5` | `#1E1E1E` | `#FFFFFF` | Default text/icons |
| `SurfaceVariantColor` | `#F2EFF5` | `#47464F` | `#F5F5F5` | `#2C2C2C` | SDS Default secondary bg + Neutral tertiary bg |
| `OnSurfaceVariantColor` | `#8A8494` | `#C9C5D0` | `#757575` | `#B3B3B3` | Secondary text/icons + Neutral default text |
| `SurfaceInverseColor` | `#313033` | `#E6E1E5` | `#5A5A5A` | `#B2B2B2` | SDS Neutral default bg (Slate700/400) |
| `OnSurfaceInverseColor` | `#F4EFF4` | `#1C1B1F` | `#F3F3F3` | `#242424` | Text on Neutral bg (Slate100/1000) |
| `SurfaceTintColor` | `#5946D2` | `#C7BFFF` | `#2C2C2C` | `#F5F5F5` | Tint (matches Primary) |
| `OutlineColor` | `#79747E` | `#928F99` | `#D9D9D9` | `#444444` | SDS Default border |
| `OutlineVariantColor` | `#C9C5D0` | `#57545D` | `#767676` | `#757575` | SDS secondary border + Neutral secondary border |
| `ShadowColor` | `#33000000` | `#33000000` | `#33000000` | `#33000000` | Same |

> **Key change from current Simple overrides**: `SurfaceInverseColor` is updated from `#1E1E1E` to `#5A5A5A` (Light) and from `#F5F5F5` to `#B2B2B2` (Dark) to serve double duty as the SDS Neutral default background. `OnSurfaceInverseColor` updated similarly to match Neutral "on" text colors.

---

## Part 2: SDS Semantic Brush -> Shared Brush Mapping

This is the core mapping. Each Simple SDS brush maps to a shared semantic brush. Simple's `ColorPalette.xaml` would retain these SDS brush keys as **aliases** pointing to shared brushes for backward compatibility.

### Key Mapping Principles

| SDS Concept | Shared Concept | Rationale |
|---|---|---|
| **Brand** | **Primary** | Brand = primary action color in both systems |
| **Danger** | **Error** | Destructive/error actions |
| **Default** | **Surface / OnSurface** | Neutral default UI chrome |
| **Neutral** | **SurfaceInverse / OnSurfaceVariant** | Muted secondary chrome — SurfaceInverse provides the darker neutral bg, OnSurfaceVariant provides muted text |
| **Disabled** | **\*DisabledBrush** variants | Opacity-based in shared system |
| **Utilities** | Hard-coded or custom | Measurement/overlay tools -- not semantic |

### State Mapping: SDS Explicit Colors vs. Shared Opacity

SDS uses **distinct color values** for hover/pressed (e.g., Brand800 -> Brand900 -> Brand1000). The shared system uses **opacity multipliers** on a single color. This is the most significant design change.

| SDS State Pattern | Shared State Pattern | Visual Impact |
|---|---|---|
| `BrandDefault` = Brand800 | `PrimaryBrush` (Opacity 1.0) | Exact match via color override |
| `BrandHover` = Brand900 (darker) | `PrimaryBrush` used with hover overlay | Subtle difference: opacity overlay vs. distinct darker shade |
| `BrandPressed` = Brand1000 (darkest) | `PrimaryBrush` used with pressed overlay | Subtle difference: opacity overlay vs. distinct shade |
| `BrandSecondary` = Brand200 | `PrimaryContainerBrush` (Opacity 1.0) | Good match -- lighter container variant |
| `BrandTertiary` = Brand100 | `PrimaryVariantLightBrush` (Opacity 1.0) | Lightest variant |

> **Decision needed**: Accept the opacity-based state model (consistent with Material) or extend SharedColors to support per-role explicit state colors. Recommendation: **Accept opacity model** for simplicity and cross-theme consistency.

---

### 2A. Background Brushes

#### Brand Backgrounds

| SDS Brush | Light Color | Proposed Shared Brush | Notes |
|---|---|---|---|
| `SimpleBackgroundBrandDefaultBrush` | Brand800 `#2C2C2C` | **`PrimaryBrush`** | Primary action bg |
| `SimpleBackgroundBrandHoverBrush` | Brand900 `#1E1E1E` | _Use `PrimaryBrush` + state overlay_ | Hover handled by control template |
| `SimpleBackgroundBrandPressedBrush` | Brand1000 `#111111` | _Use `PrimaryBrush` + state overlay_ | Pressed handled by control template |
| `SimpleBackgroundBrandSecondaryBrush` | Brand200 `#E6E6E6` | **`PrimaryContainerBrush`** | Secondary emphasis bg |
| `SimpleBackgroundBrandSecondaryHoverBrush` | Brand300 `#D9D9D9` | _Use `PrimaryContainerBrush` + overlay_ | |
| `SimpleBackgroundBrandSecondaryPressedBrush` | Brand400 `#B3B3B3` | _Use `PrimaryContainerBrush` + overlay_ | |
| `SimpleBackgroundBrandTertiaryBrush` | Brand100 `#F5F5F5` | **`PrimaryVariantLightBrush`** | Tertiary/subtle bg |
| `SimpleBackgroundBrandTertiaryHoverBrush` | Brand200 | _Use `PrimaryVariantLightBrush` + overlay_ | |
| `SimpleBackgroundBrandTertiaryPressedBrush` | Brand300 | _Use `PrimaryVariantLightBrush` + overlay_ | |

#### Danger Backgrounds

| SDS Brush | Light Color | Proposed Shared Brush | Notes |
|---|---|---|---|
| `SimpleBackgroundDangerDefaultBrush` | Red500 `#EC221F` | **`ErrorBrush`** | Error/danger action bg |
| `SimpleBackgroundDangerHoverBrush` | Red600 | _Use `ErrorBrush` + state overlay_ | |
| `SimpleBackgroundDangerPressedBrush` | Red700 | _Use `ErrorBrush` + state overlay_ | |
| `SimpleBackgroundDangerSecondaryBrush` | Red200 `#FDD3D0` | **`ErrorContainerBrush`** | Danger container bg |
| `SimpleBackgroundDangerSecondaryHoverBrush` | Red300 | _Use `ErrorContainerBrush` + overlay_ | |
| `SimpleBackgroundDangerSecondaryPressedBrush` | Red400 | _Use `ErrorContainerBrush` + overlay_ | |
| `SimpleBackgroundDangerTertiaryBrush` | Red100 `#FEE9E7` | **`OnErrorBrush`** | Lightest danger bg |
| `SimpleBackgroundDangerTertiaryHoverBrush` | Red200 | _Use `OnErrorBrush` + overlay_ | |
| `SimpleBackgroundDangerTertiaryPressedBrush` | Red300 | _Use `OnErrorBrush` + overlay_ | |

#### Default Backgrounds

| SDS Brush | Light Color | Proposed Shared Brush | Notes |
|---|---|---|---|
| `SimpleBackgroundDefaultDefaultBrush` | White `#FFFFFF` | **`SurfaceBrush`** | Default surface |
| `SimpleBackgroundDefaultDefaultHoverBrush` | Gray100 | _Use `SurfaceBrush` + overlay_ | |
| `SimpleBackgroundDefaultDefaultPressedBrush` | Gray200 | _Use `SurfaceBrush` + overlay_ | |
| `SimpleBackgroundDefaultSecondaryBrush` | Gray100 `#F5F5F5` | **`SurfaceVariantBrush`** | Secondary surface |
| `SimpleBackgroundDefaultSecondaryHoverBrush` | Gray200 | _Use `SurfaceVariantBrush` + overlay_ | |
| `SimpleBackgroundDefaultSecondaryPressedBrush` | Gray300 | _Use `SurfaceVariantBrush` + overlay_ | |
| `SimpleBackgroundDefaultTertiaryBrush` | Gray300 `#D9D9D9` | **`OutlineBrush`** | Tertiary/muted bg |
| `SimpleBackgroundDefaultTertiaryHoverBrush` | Gray400 | _Use `OutlineBrush` + overlay_ | |
| `SimpleBackgroundDefaultTertiaryPressedBrush` | Gray500 | _Use `OutlineBrush` + overlay_ | |

#### Disabled Backgrounds

| SDS Brush | Light Color | Proposed Shared Brush | Notes |
|---|---|---|---|
| `SimpleBackgroundDisabledDefaultBrush` | Brand300 `#D9D9D9` | **`OnSurfaceDisabledBrush`** | Shared uses opacity-based disabled |

> **Note**: The shared `OnSurfaceDisabledBrush` uses `OnSurfaceColor` at 12% opacity. The SDS disabled bg uses a flat `#D9D9D9`. These will look different -- the shared version is translucent. Consider adding a `DisabledBackgroundColor` to SharedColorPalette if exact match is needed, or accept the opacity approach.

#### Neutral Backgrounds

| SDS Brush | Light Color | Proposed Shared Brush | Notes |
|---|---|---|---|
| `SimpleBackgroundNeutralDefaultBrush` | Slate700 `#5A5A5A` | **`SurfaceInverseBrush`** | Neutral primary bg -- SurfaceInverse override carries this value |
| `SimpleBackgroundNeutralHoverBrush` | Slate800 | _Use `SurfaceInverseBrush` + overlay_ | |
| `SimpleBackgroundNeutralPressedBrush` | Slate900 | _Use `SurfaceInverseBrush` + overlay_ | |
| `SimpleBackgroundNeutralSecondaryBrush` | Slate300 `#CDCDCD` | **`OutlineVariantBrush`** | Neutral container bg -- uses muted outline |
| `SimpleBackgroundNeutralSecondaryHoverBrush` | Slate400 | _Use `OutlineVariantBrush` + overlay_ | |
| `SimpleBackgroundNeutralSecondaryPressedBrush` | Slate500 | _Use `OutlineVariantBrush` + overlay_ | |
| `SimpleBackgroundNeutralTertiaryBrush` | Slate200 `#E3E3E3` | **`SurfaceVariantBrush`** | Lightest neutral bg |
| `SimpleBackgroundNeutralTertiaryHoverBrush` | Slate300 | _Use `SurfaceVariantBrush` + overlay_ | |
| `SimpleBackgroundNeutralTertiaryPressedBrush` | Slate400 | _Use `SurfaceVariantBrush` + overlay_ | |

#### Utility Backgrounds

| SDS Brush | Light Color | Proposed Shared Brush | Notes |
|---|---|---|---|
| `SimpleBackgroundUtilitiesBlanketBrush` | `#B2000000` | **Keep as Simple-specific** | Dev/design tool overlays |
| `SimpleBackgroundUtilitiesMeasurementBrush` | Pink200 | **Keep as Simple-specific** | Dev/design tool |
| `SimpleBackgroundUtilitiesOverlayBrush` | `#80000000` | **`ShadowBrush`** or keep specific | Modal overlay |
| `SimpleBackgroundUtilitiesScrimBrush` | `#CCFFFFFF` (L) / `#CC000000` (D) | **Keep as Simple-specific** | Scrim overlay |

---

### 2B. Text Brushes

| SDS Brush | Proposed Shared Brush | Notes |
|---|---|---|
| **Brand Text** | | |
| `SimpleTextBrandDefaultBrush` | **`PrimaryBrush`** | Primary-colored text |
| `SimpleTextBrandOnBrandBrush` | **`OnPrimaryBrush`** | Text on primary bg |
| `SimpleTextBrandOnBrandSecondaryBrush` | **`OnPrimaryContainerBrush`** | Text on primary container bg |
| `SimpleTextBrandOnBrandTertiaryBrush` | **`PrimaryBrush`** | Text on lightest brand bg |
| `SimpleTextBrandSecondaryBrush` | **`PrimaryMediumBrush`** | Medium emphasis primary text |
| `SimpleTextBrandTertiaryBrush` | **`PrimaryLowBrush`** | Low emphasis primary text |
| **Danger Text** | | |
| `SimpleTextDangerDefaultBrush` | **`ErrorBrush`** | Error-colored text |
| `SimpleTextDangerOnDangerBrush` | **`OnErrorBrush`** | Text on error bg |
| `SimpleTextDangerOnDangerSecondaryBrush` | **`OnErrorContainerBrush`** | Text on error container |
| `SimpleTextDangerOnDangerTertiaryBrush` | **`OnErrorContainerBrush`** | Same |
| `SimpleTextDangerSecondaryBrush` | **`ErrorMediumBrush`** | Medium emphasis error text |
| `SimpleTextDangerTertiaryBrush` | **`ErrorLowBrush`** | Low emphasis error text |
| **Default Text** | | |
| `SimpleTextDefaultDefaultBrush` | **`OnSurfaceBrush`** | Default body text |
| `SimpleTextDefaultSecondaryBrush` | **`OnSurfaceMediumBrush`** | Secondary/muted text |
| `SimpleTextDefaultTertiaryBrush` | **`OnSurfaceLowBrush`** | Placeholder/hint text |
| **Disabled Text** | | |
| `SimpleTextDisabledDefaultBrush` | **`OnSurfaceDisabledBrush`** | Disabled text |
| `SimpleTextDisabledOnDisabledBrush` | **`OnSurfaceDisabledBrush`** | Disabled text on disabled bg |
| **Neutral Text** | | |
| `SimpleTextNeutralDefaultBrush` | **`OnSurfaceVariantBrush`** | Neutral-colored text (muted foreground) |
| `SimpleTextNeutralOnNeutralBrush` | **`OnSurfaceInverseBrush`** | Text on neutral (SurfaceInverse) bg |
| `SimpleTextNeutralOnNeutralSecondaryBrush` | **`OnSurfaceVariantBrush`** | Text on neutral container bg |
| `SimpleTextNeutralOnNeutralTertiaryBrush` | **`OnSurfaceVariantBrush`** | Same |
| `SimpleTextNeutralSecondaryBrush` | **`OnSurfaceVariantMediumBrush`** | Medium neutral text |
| `SimpleTextNeutralTertiaryBrush` | **`OnSurfaceVariantLowBrush`** | Low neutral text |
| **Utility Text** | | |
| `SimpleTextUtilitiesOnMeasurementBrush` | **Keep as Simple-specific** | Dev tool |
| `SimpleTextUtilitiesOnOverlayBrush` | **`OnSurfaceInverseBrush`** | Text on dark overlay |

---

### 2C. Icon Brushes

Icon brushes follow the **exact same mapping pattern** as Text brushes -- the SDS design system intentionally keeps Text and Icon color assignments identical per role/variant. Every row in section 2B applies identically for icons:

| SDS Pattern | Shared Brush |
|---|---|
| `SimpleIcon{Role}DefaultBrush` | Same as `SimpleText{Role}DefaultBrush` mapping |
| `SimpleIcon{Role}On{Role}Brush` | Same as `SimpleText{Role}On{Role}Brush` mapping |
| `SimpleIcon{Role}On{Role}SecondaryBrush` | Same as corresponding text brush mapping |
| `SimpleIcon{Role}SecondaryBrush` | Same as corresponding text brush mapping |
| `SimpleIcon{Role}TertiaryBrush` | Same as corresponding text brush mapping |
| `SimpleIconDisabledDefaultBrush` | `OnSurfaceDisabledBrush` |
| `SimpleIconDisabledOnDisabledBrush` | `OnSurfaceDisabledBrush` |
| `SimpleIconUtilitiesBrush` | Keep as Simple-specific |
| `SimpleIconUtilitiesOnMeasurementBrush` | Keep as Simple-specific |

---

### 2D. Border Brushes

| SDS Brush | Proposed Shared Brush | Notes |
|---|---|---|
| **Brand Borders** | | |
| `SimpleBorderBrandDefaultBrush` | **`PrimaryBrush`** | Primary border |
| `SimpleBorderBrandSecondaryBrush` | **`PrimaryMediumBrush`** | Medium primary border |
| `SimpleBorderBrandTertiaryBrush` | **`PrimaryLowBrush`** | Low primary border |
| **Danger Borders** | | |
| `SimpleBorderDangerDefaultBrush` | **`ErrorBrush`** | Error border |
| `SimpleBorderDangerSecondaryBrush` | **`ErrorMediumBrush`** | Medium error border |
| `SimpleBorderDangerTertiaryBrush` | **`ErrorLowBrush`** | Low error border |
| **Default Borders** | | |
| `SimpleBorderDefaultDefaultBrush` | **`OutlineBrush`** | Standard border |
| `SimpleBorderDefaultSecondaryBrush` | **`OutlineVariantBrush`** | Secondary border |
| `SimpleBorderDefaultTertiaryBrush` | **`OnSurfaceVariantBrush`** | Tertiary/strong border |
| **Disabled Borders** | | |
| `SimpleBorderDisabledDefaultBrush` | **`OutlineDisabledBrush`** | Disabled border |
| **Neutral Borders** | | |
| `SimpleBorderNeutralDefaultBrush` | **`OnSurfaceVariantBrush`** | Neutral border (muted foreground color) |
| `SimpleBorderNeutralSecondaryBrush` | **`OutlineVariantBrush`** | Medium neutral border |
| `SimpleBorderNeutralTertiaryBrush` | **`OutlineLowBrush`** | Low neutral border |
| **Utility Borders** | | |
| `SimpleBorderUtilitiesMeasurementBrush` | **Keep as Simple-specific** | Dev tool |
| `SimpleBorderUtilitiesSwatchBrush` | **Keep as Simple-specific** | Dev tool |

---

## Part 3: Primitive Color Scales -- Disposition

After migration, the SDS primitive color scales in `ColorPalette.xaml` are **no longer referenced** by semantic brushes and can be removed.

| Primitive Scale | Current Usage | Disposition |
|---|---|---|
| `SimpleBlack100-1000Color` | Only by SDS brushes | **Remove** -- alpha values baked into SharedColorPalette overrides |
| `SimpleWhite100-1000Color` | Only by SDS brushes | **Remove** |
| `SimpleBrand100-1000Color` | Only by SDS brushes | **Remove** -- mapped to Primary/PrimaryContainer/PrimaryVariant |
| `SimpleGray100-1000Color` | Only by SDS brushes | **Remove** -- mapped to Surface/SurfaceVariant/Outline |
| `SimpleSlate100-1000Color` | Only by SDS brushes | **Remove** -- mapped to SurfaceInverse/SurfaceVariant/OutlineVariant |
| `SimpleGreen100-1000Color` | Only by Positive brushes | **Remove** -- Positive role dropped (zero consumers) |
| `SimpleRed100-1000Color` | Only by SDS brushes | **Remove** -- mapped to Error/ErrorContainer |
| `SimplePink100-1000Color` | Only by Utility brushes | **Keep** (only ~4 brushes use them) |
| `SimpleYellow100-1000Color` | Only by Warning brushes | **Remove** -- Warning role dropped (zero consumers) |

---

## Part 4: SDS Brushes Removed (Not Mapped)

The following SDS brush families are **deleted entirely** rather than mapped. They have zero references in any Simple control style.

### Positive Brushes (removed)

| Category | Brushes Removed |
|---|---|
| Background | `SimpleBackgroundPositive{Default,Hover,Pressed}Brush`, `SimpleBackgroundPositiveSecondary{,Hover,Pressed}Brush`, `SimpleBackgroundPositiveTertiary{,Hover,Pressed}Brush` (9) |
| Text | `SimpleTextPositive{Default,OnPositive,OnPositiveSecondary,OnPositiveTertiary,Secondary,Tertiary}Brush` (6) |
| Icon | `SimpleIconPositive{Default,OnPositive,OnPositiveSecondary,OnPositiveTertiary,Secondary,Tertiary}Brush` (6) |
| Border | `SimpleBorderPositive{Default,Secondary,Tertiary}Brush` (3) |
| **Subtotal** | **24 brushes per theme (48 total)** |

### Warning Brushes (removed)

| Category | Brushes Removed |
|---|---|
| Background | `SimpleBackgroundWarning{Default,Hover,Pressed}Brush`, `SimpleBackgroundWarningSecondary{,Hover,Pressed}Brush`, `SimpleBackgroundWarningTertiary{,Hover,Pressed}Brush` (9) |
| Text | `SimpleTextWarning{Default,OnWarning,OnWarningSecondary,OnWarningTertiary,Secondary,Tertiary}Brush` (6) |
| Icon | `SimpleIconWarning{Default,OnWarning,OnWarningSecondary,OnWarningTertiary,Secondary,Tertiary}Brush` (6) |
| Border | `SimpleBorderWarning{Default,Secondary,Tertiary}Brush` (3) |
| **Subtotal** | **24 brushes per theme (48 total)** |

**Total removed: 48 brushes per theme (96 entries) + 20 primitive colors per theme (40 entries) = 136 entries deleted.**

---

## Part 5: Simple `ColorPalette.xaml` After Refactoring

The refactored file would contain only:

1. **SharedColorPalette overrides** (~25 Color entries per theme) -- setting Simple's grayscale/flat values for the shared roles
2. **SDS alias brushes** (~72 SolidColorBrush entries per theme) -- `SimpleXxxBrush` keys pointing to shared brush resources for backward compatibility
3. **Utility-only primitives** -- Pink scale + hardcoded utility colors

### Example (Light theme excerpt)

```xml
<ResourceDictionary x:Key="Light">
    <!-- SharedColorPalette Overrides -->
    <Color x:Key="PrimaryColor">#2C2C2C</Color>
    <Color x:Key="OnPrimaryColor">#F5F5F5</Color>
    <Color x:Key="PrimaryContainerColor">#E6E6E6</Color>
    <Color x:Key="OnPrimaryContainerColor">#1E1E1E</Color>
    <Color x:Key="ErrorColor">#EC221F</Color>
    <Color x:Key="SurfaceInverseColor">#5A5A5A</Color>
    <Color x:Key="OnSurfaceInverseColor">#F3F3F3</Color>
    <!-- ... remaining overrides ... -->

    <!-- SDS Alias Brushes (backward compat) -->
    <StaticResource x:Key="SimpleBackgroundBrandDefaultBrush" ResourceKey="PrimaryBrush" />
    <StaticResource x:Key="SimpleBackgroundDangerDefaultBrush" ResourceKey="ErrorBrush" />
    <StaticResource x:Key="SimpleBackgroundDefaultDefaultBrush" ResourceKey="SurfaceBrush" />
    <StaticResource x:Key="SimpleBackgroundNeutralDefaultBrush" ResourceKey="SurfaceInverseBrush" />
    <StaticResource x:Key="SimpleTextDefaultDefaultBrush" ResourceKey="OnSurfaceBrush" />
    <StaticResource x:Key="SimpleTextNeutralDefaultBrush" ResourceKey="OnSurfaceVariantBrush" />
    <!-- ... remaining aliases ... -->
</ResourceDictionary>
```

---

## Part 6: Known Gaps and Trade-offs

### 6.1 State Color Fidelity

**Issue**: SDS uses distinct hex colors for hover/pressed states (e.g., Brand800->900->1000). The shared system uses opacity overlays (0.08 hover, 0.12 pressed) on a single base color.

**Impact**: Hover/pressed states will look slightly different -- opacity overlay produces a translucent tint rather than a distinct darker shade.

**Recommendation**: Accept the opacity model. The visual difference is subtle and the consistency gain is significant. If specific controls need exact shades, they can use lightweight styling overrides.

### 6.2 SDS "OnXxx" Role Asymmetry

SDS defines separate text colors for "on brand" at three emphasis levels (OnBrand, OnBrandSecondary, OnBrandTertiary). The shared system has only `OnPrimary` and `OnPrimaryContainer`. Some SDS "on" variants may need to map to the same shared brush.

### 6.3 SDS Category Dimension (Background/Text/Icon/Border)

The shared system doesn't separate brushes by usage category -- `PrimaryBrush` is used for backgrounds AND text AND icons. SDS explicitly separates these. After migration, the same `PrimaryBrush` serves all categories, which is actually the correct semantic approach (the color role determines the color, not the element type).

### 6.4 Disabled State Model

SDS uses flat colors for disabled states (e.g., Brand300 `#D9D9D9`). The shared system uses the base color at 12% opacity. This changes how disabled controls look -- they become translucent rather than flat gray.

**Recommendation**: Accept opacity-based disabled for most cases. If specific controls need opaque disabled backgrounds, handle via lightweight styling overrides.

### 6.5 Dark Theme Inversion

SDS Brand/Gray/Slate scales use the **same hex values** in both Light and Dark themes -- the semantic brushes just pick different indices (Brand800 in Light -> Brand100 in Dark). The shared system uses **different color values** per theme dictionary. This is already handled by the Simple overrides having different hex values in each theme dictionary.

### 6.6 Utility Brushes

Blanket, Measurement, Overlay, Scrim brushes are design-tool utilities specific to SDS. They have no shared equivalent and should remain Simple-specific. This is a small set (~8 brushes) and doesn't compromise the "use shared brushes" goal.

### 6.7 SurfaceInverse Repurposing

`SurfaceInverseColor` is repurposed to carry the SDS Neutral default background value (`#5A5A5A` Light / `#B2B2B2` Dark) instead of the original near-black/near-white values. This is a deliberate trade-off: the Neutral role is more useful to Simple controls than a pure inverse surface. If both concepts are needed simultaneously, lightweight styling overrides can provide the original values per control.

---

## Part 7: Implementation Order

1. **Remove Positive/Warning** -- Delete Green/Yellow primitive scales and all Positive/Warning brushes from ColorPalette.xaml
2. **Update Simple ColorPalette.xaml** -- Add full SharedColorPalette overrides (including updated SurfaceInverse values), replace remaining SDS brush definitions with aliases to shared brushes
3. **Update Simple control styles** -- Replace `SimpleXxxBrush` references with shared brush names (or rely on aliases during transition)
4. **Update BaseTheme / SimpleTheme** -- Ensure Simple loads SharedColors.xaml in its resource chain (it may already via BaseTheme)
5. **Visual regression testing** -- Compare before/after screenshots for all Simple-styled controls
6. **Remove primitive scales** -- Delete unused Simple*Color primitives from ColorPalette.xaml
7. **Document** -- Update migration guide with old->new brush key mapping

---

## Appendix A: Complete SDS Brush Count (After Positive/Warning Removal)

| Category | Roles | Variants per Role | Brushes |
|---|---|---|---|
| Background | 4 (Brand, Danger, Default, Neutral) | 9 (Default + Hover + Pressed x 3 tiers) | 36 |
| Background | 1 (Disabled) | 1 | 1 |
| Background | 1 (Utilities) | 4 (Blanket, Measurement, Overlay, Scrim) | 4 |
| Text | 4 roles | 6 (Default, OnXxx, OnXxxSecondary, OnXxxTertiary, Secondary, Tertiary) | 24 |
| Text | 1 (Disabled) | 2 (Default, OnDisabled) | 2 |
| Text | 1 (Utilities) | 2 (OnMeasurement, OnOverlay) | 2 |
| Icon | 4 roles | 6 each | 24 |
| Icon | 1 (Disabled) | 2 | 2 |
| Icon | 1 (Utilities) | 2 (Utilities, OnMeasurement) | 2 |
| Border | 4 roles | 3 (Default, Secondary, Tertiary) | 12 |
| Border | 1 (Disabled) | 1 | 1 |
| Border | 1 (Utilities) | 2 (Measurement, Swatch) | 2 |
| **Total** | | | **~112 per theme** |

## Appendix B: SharedColors.xaml — No Changes Required

The existing SharedColors.xaml already provides all brush families needed:

| Roles Used by Simple | Brush Families | Brushes (9 states each) |
|---|---|---|
| Primary, OnPrimary, PrimaryContainer, OnPrimaryContainer | 4 | 36 |
| PrimaryVariantLight, PrimaryVariantDark | 2 | 18 |
| Error, OnError, ErrorContainer, OnErrorContainer | 4 | 36 |
| Surface, OnSurface, SurfaceVariant, OnSurfaceVariant | 4 | 36 |
| SurfaceInverse, OnSurfaceInverse | 2 | 18 |
| Outline, OutlineVariant | 2 | 18 |
| Background, OnBackground | 2 | 18 |
| **Total used by Simple** | **20** | **~180 brushes** |

The remaining shared brush families (Secondary*, Tertiary*, PrimaryInverse, SurfaceTint) are available but not actively mapped to SDS aliases. They remain available for direct use by controls or future SDS role expansion.
