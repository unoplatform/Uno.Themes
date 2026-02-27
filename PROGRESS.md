# Simple Theme Refactoring — Progress Document

> **Last updated:** 2026-02-26
> **Purpose:** Hand-off context for continuing the Simple Design System (SDS) theme migration in `uno.themes` and its sample app.

---

## Goal

Refactor the entire **Uno Simple Theme** (`Uno.Simple.WinUI`) by mapping design tokens and component styles from the **SDS React/Figma design system** (located in the sibling workspace `x:\src\sds`) into XAML resources, following the **lightweight styling patterns** established by the Material theme in this same repo.

Key constraints from the original request:
- All resources must be **Simple theme specific** (prefixed `Simple*`). Abstraction aliases for a shared design language will come in a later phase.
- Follow Material theme lightweight styling conventions (per-visual-state `{ThemeResource}` keys exposed as `<StaticResource>` aliases in `ThemeDictionaries`).

---

## Architecture Overview

### SDS Design Token Tiers (CSS → XAML)

| Tier | CSS example | XAML example |
|---|---|---|
| **Primitive** | `--sds-color-brand-800: #2C2C2C` | `<Color x:Key="SimpleBrand800Color">#2C2C2C</Color>` |
| **Semantic** | `--sds-color-background-brand-default` | `<SolidColorBrush x:Key="SimpleBackgroundBrandDefaultBrush" Color="{StaticResource SimpleBrand800Color}" />` |
| **Component / Lightweight** | (button CSS vars) | `<StaticResource x:Key="SimplePrimaryButtonBackground" ResourceKey="SimpleBackgroundBrandDefaultBrush" />` |

### Color Hex Conversion

- CSS 6-digit `#RRGGBB` → XAML `#RRGGBB` (same)
- CSS 8-digit `#RRGGBBAA` → XAML `#AARRGGBB` (alpha moves to front)

### Theme Mapping

- CSS `@media (prefers-color-scheme: light)` / default → XAML `<ResourceDictionary x:Key="Light">`
- CSS `@media (prefers-color-scheme: dark)` → XAML `<ResourceDictionary x:Key="Default">`

### Resource Loading Order (SimpleTheme.cs / BaseTheme.cs)

1. `SharedColorPalette.xaml` (from Uno.Themes base)
2. **`ColorPalette.xaml`** — SafeMerged over SharedColorPalette (overrides + SDS tokens)
3. `SharedColors.xaml` — generates brushes from palette colors
4. `mergedpages.xaml` (XamlMerge output — all control styles including Button)
5. `Fonts.xaml` — loaded separately
6. `Thickness.xaml` — loaded separately

**XamlMerge config** (`simple-common.props`):
- Includes: `Styles\Controls\**\*.xaml`, `Styles\Application\**\*.xaml`
- Excludes: `ColorPalette.xaml`, `Common\Fonts.xaml`, `Common\Thickness.xaml`

---

## SDS Source Files (Reference)

All source design tokens live in `x:\src\sds`:

| File | Contains |
|---|---|
| `src/theme.css` (526 lines) | All CSS custom properties — primitives, semantic light/dark tokens |
| `scripts/tokens/tokens.json` (4843 lines) | Figma variable definitions |
| `scripts/tokens/styles.json` (1092 lines) | Figma text/typography styles |
| `scripts/tokens/tokenVariableSyntaxAndDescriptionSnippet.js` (342 lines) | CSS variable syntax/description mappings |
| `src/ui/primitives/Button/Button.tsx` | React Button component (variants: primary, neutral, subtle, danger-primary, danger-subtle; sizes: small, medium) |
| `src/ui/primitives/Button/button.css` | Full CSS for all button variant colors, hover/pressed/disabled states, sizing |

### Material Theme Pattern Reference

The lightweight styling pattern was studied from:
- `src/library/Uno.Material/Styles/Controls/v2/Button.xaml` — canonical example of per-state `{ThemeResource}` keys
- `src/library/Uno.Themes/Styles/Applications/Common/SharedColorPalette.xaml` — base palette structure
- `src/library/Uno.Themes/Styles/Applications/Common/SharedColors.xaml` — brush generation with opacity variants

---

## Completed Work

### 1. ColorPalette.xaml — FULLY REWRITTEN ✅

**Path:** `src/library/Uno.Simple.WinUI/Styles/Application/ColorPalette.xaml`

Contains Light and Default (Dark) ThemeDictionaries with:

- **SharedColorPalette overrides** — `PrimaryColor`, `OnPrimaryColor`, `SurfaceColor`, `BackgroundColor`, `ErrorColor`, etc. mapped to SDS brand palette for each theme
- **100 SDS primitive Colors** — 10 palettes (Black, White, Brand, Gray, Slate, Green, Red, Pink, Yellow) × 10 scales (100–1000), duplicated in both themes
- **~135 semantic SolidColorBrush resources per theme** in 4 categories:
  - `SimpleBackground*Brush` — Brand/Default/Danger/Positive/Warning/Neutral/Disabled/Utilities roles with Default/Hover/Secondary/Tertiary variants
  - `SimpleText*Brush` — same role structure + OnBrand/OnDanger/etc. variants
  - `SimpleIcon*Brush` — mirrors Text structure
  - `SimpleBorder*Brush` — Default/Secondary/Tertiary per role

### 2. Button.xaml — FULLY REWRITTEN ✅

**Path:** `src/library/Uno.Simple.WinUI/Styles/Controls/Button.xaml`

Contains:

- **Theme-agnostic layout resources** — `SimpleButtonCornerRadius(8)`, `SimpleButtonBorderThickness(1)`, `SimpleButtonPadding(12)`, `SimpleButtonMinHeight(40)`, `SimpleButtonFontSize(16)`, small variants (`SimpleButtonSmallPadding(8)`, `SimpleButtonSmallMinHeight(32)`, `SimpleButtonSmallFontSize(14)`)
- **60 lightweight styling `<StaticResource>` aliases per theme** — 5 variants × 4 states (Normal/PointerOver/Pressed/Disabled) × 3 properties (Foreground/Background/BorderBrush)
- **5 variant styles**, each with own `ControlTemplate` + `{ThemeResource}` visual state setters:
  - `SimplePrimaryButtonStyle` — dark bg, light text (brand-800/brand-100 light; brand-100/brand-900 dark)
  - `SimpleNeutralButtonStyle` — light bg with border (gray-100/gray-900 light; gray-800/white dark)
  - `SimpleSubtleButtonStyle` — transparent bg, brand text
  - `SimpleDangerPrimaryButtonStyle` — red bg, light text
  - `SimpleDangerSubtleButtonStyle` — transparent bg, red text
- **`SimpleBaseButtonStyle`** — shared property setters (font, padding, corner radius, etc.)
- **5 small size variants** — `SimpleSmall*ButtonStyle` using `BasedOn` (no template duplication)

### 3. Fonts.xaml — UPDATED ✅

**Path:** `src/library/Uno.Simple.WinUI/Styles/Application/Common/Fonts.xaml`

Added:
- 4 font families: `SimpleFontFamily`/`SimpleFontFamilySans` (Inter), `SimpleFontFamilySerif` (Noto Serif), `SimpleFontFamilyMono` (Roboto Mono)
- 10-step typography scale: `SimpleTypographyScale01`(12) through `SimpleTypographyScale10`(72)
- 10-step line heights: `SimpleLineHeight01`(16) through `SimpleLineHeight10`(80)
- 4 font weights: `SimpleRegularFontWeight`, `SimpleMediumFontWeight`, `SimpleSemiBoldFontWeight`, `SimpleBoldFontWeight`

### 4. _Resources.xaml — UPDATED ✅

**Path:** `src/library/Uno.Simple.WinUI/Styles/Controls/_Resources.xaml`

Added aliases:
- `DangerPrimaryButtonStyle` → `SimpleDangerPrimaryButtonStyle`
- `DangerSubtleButtonStyle` → `SimpleDangerSubtleButtonStyle`
- 5 small button aliases: `SmallPrimaryButtonStyle`, `SmallNeutralButtonStyle`, `SmallSubtleButtonStyle`, `SmallDangerPrimaryButtonStyle`, `SmallDangerSubtleButtonStyle`

### 5. Build Validation ✅

`dotnet build Uno.Simple.WinUI.csproj -p:TargetFramework=net9.0` → **Build succeeded, 0 errors**

(The only error in the full multi-target build is a pre-existing `MSB4062 ExpandPriContent` task error in `Uno.Themes.WinUI.csproj` for `net9.0-windows10.0.19041` — unrelated to Simple theme changes.)

### 6. ButtonSamplePage.xaml — FULLY REWRITTEN ✅

**Path:** `src/samples/SimpleSamplesApp/Content/Controls/ButtonSamplePage.xaml`

The sample page previously referenced many non-existent styles (`SimpleLightButtonStyle`, `SimplePrimaryButtonSmallStyle`, `SimplePrimaryErrorButtonStyle`, `SimpleBrandedPricingCardButtonStyle`, etc.). It has been rewritten to demonstrate exactly the 11 styles defined in `Button.xaml`:

- **5 medium (default) variant sections** — each showing enabled + disabled states:
  - `SimplePrimaryButtonStyle`
  - `SimpleNeutralButtonStyle`
  - `SimpleSubtleButtonStyle`
  - `SimpleDangerPrimaryButtonStyle`
  - `SimpleDangerSubtleButtonStyle`
- **Small size variants section** — all 5 `SimpleSmall*ButtonStyle` variants in enabled state
- **Small size variants — disabled section** — all 5 small variants in disabled state

Each section uses `smtx:XamlDisplay` with unique keys (`Simple_Button_Primary`, `Simple_Button_DangerPrimary`, `Simple_Button_SmallVariants`, etc.) for ShowMeTheXAML integration.

**Build validated:** `dotnet build SimpleSamplesApp.csproj -p:TargetFramework=net10.0-desktop` → **Build succeeded, 0 errors**

---

## Files NOT Changed

| File | Status |
|---|---|
| `SimpleTheme.cs` | No changes needed — resource loading order is correct |
| `SimpleConstants.cs` | No changes needed — resource paths unchanged |
| `simple-common.props` | No changes needed — XamlMerge config is correct |
| `Thickness.xaml` | Existing spacing/sizing tokens are adequate for now |
| `AnimationConstants.xaml` | Not touched |

---

## Remaining Work (Not Started)

### Controls to Migrate (same lightweight styling pattern as Button)

The following control styles in `src/library/Uno.Simple.WinUI/Styles/Controls/` need to be refactored to use SDS design tokens + lightweight styling:

| File | Current State |
|---|---|
| `CheckBox.xaml` | Exists — needs SDS token mapping + lightweight styling keys |
| `ComboBox.xaml` | Exists — needs SDS token mapping + lightweight styling keys |
| `RadioButton.xaml` | Exists — needs SDS token mapping + lightweight styling keys |
| `TextBox.xaml` | Exists — needs SDS token mapping + lightweight styling keys |
| `ToggleSwitch.xaml` | Exists — needs SDS token mapping + lightweight styling keys |
| `PersonPicture.xaml` | Exists — likely needs token mapping |

For each control:
1. Study the SDS React component in `x:\src\sds\src\ui\primitives\{Control}\` for variants, states, and CSS token usage
2. Map CSS color tokens to the semantic brushes already defined in `ColorPalette.xaml`
3. Create `<StaticResource>` lightweight styling aliases in `ThemeDictionaries` (per variant × per state × per property)
4. Build the `ControlTemplate` with `{ThemeResource}` visual state setters
5. Update `_Resources.xaml` if new aliases are needed

### Typography Styles (Not Yet Created)

The `_Resources.xaml` references typography aliases (`SimpleDisplayLarge`, `SimpleBodyMedium`, etc.) that don't have backing `Style` resources yet. These need to be created as `TextBlock` styles using the font family, scale, line height, and weight primitives already defined in `Fonts.xaml`.

### Abstraction Layer (Future Phase)

Per the original requirement: "at a later phase we will create abstraction aliases for all resource keys so they can be mapped from a more general design language." All current keys are `Simple*` prefixed and theme-specific.

---

## Key Button Color Mappings (Quick Reference)

### Light Theme

| Variant | Background | Background Hover | Foreground | Border |
|---|---|---|---|---|
| Primary | Brand800 `#2C2C2C` | Brand900 `#1E1E1E` | Brand100 `#F5F5F5` | Brand800 |
| Neutral | Gray100 `#F5F5F5` | Gray200 `#E6E6E6` | Gray900 `#1E1E1E` | Gray300 `#D9D9D9` |
| Subtle | Transparent | Gray100 | Brand800 `#2C2C2C` | Transparent |
| DangerPrimary | Red500 `#EC221F` | Red600 `#C00F0C` | Red100 `#FEE9E7` | Red600 |
| DangerSubtle | Transparent | Red200 `#FDD3D0` | Red700 `#900B09` | Transparent |
| Disabled (all) | Brand300 `#D9D9D9` | — | Brand400 `#B3B3B3` | Brand400 |

### Dark Theme

| Variant | Background | Background Hover | Foreground | Border |
|---|---|---|---|---|
| Primary | Brand100 `#F5F5F5` | Brand300 `#D9D9D9` | Brand900 `#1E1E1E` | Brand100 |
| Neutral | Gray800 `#2C2C2C` | Gray900 `#1E1E1E` | White `#FFFFFF` | Gray600 `#444444` |
| Subtle | Transparent | Gray700 `#383838` | Brand100 `#F5F5F5` | Transparent |
| DangerPrimary | Red600 `#C00F0C` | Red700 `#900B09` | Red100 `#FEE9E7` | Red400 `#F4776A` |
| DangerSubtle | Transparent | Red1000 `#300603` | Red200 `#FDD3D0` | Transparent |
| Disabled (all) | Brand700 `#383838` | — | Brand500 `#757575` | Brand600 `#444444` |

---

## Build Commands

```bash
# Build Simple theme library (single target, fast)
dotnet build src/library/Uno.Simple.WinUI/Uno.Simple.WinUI.csproj --no-restore -p:TargetFramework=net9.0

# Build all targets (will hit pre-existing Windows SDK error)
dotnet build src/library/Uno.Simple.WinUI/Uno.Simple.WinUI.csproj --no-restore

# Build Simple sample app (targets net10.0-*; use desktop for quick validation)
dotnet build src/samples/SimpleSamplesApp/SimpleSamplesApp.csproj -p:TargetFramework=net10.0-desktop
```
