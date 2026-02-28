# Simple Theme Refactoring — Progress Document

> **Last updated:** 2025-07-28
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
| `src/ui/primitives/Checkbox/Checkbox.tsx` | React Checkbox component (unchecked/checked/indeterminate, disabled) |
| `src/ui/primitives/Checkbox/checkbox.css` | CSS for checkbox states, 16×16 box, 4px radius |
| `src/ui/primitives/Select/Select.tsx` | React Select (ComboBox) component with items, error variant |
| `src/ui/primitives/Select/select.css` | CSS for select states, popup, item selection, error |
| `src/ui/primitives/Switch/Switch.tsx` | React Switch (ToggleSwitch) component, off/on/disabled |
| `src/ui/primitives/Switch/switch.css` | CSS for switch track+thumb states, 40×24 pill shape |
| `src/ui/primitives/Radio/Radio.tsx` | React Radio component (unchecked/checked, disabled) |
| `src/ui/primitives/Radio/radio.css` | CSS for radio states, 16×16 circle, 8×8 dot |

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

### 7. CheckBox.xaml — FULLY REWRITTEN ✅

**Path:** `src/library/Uno.Simple.WinUI/Styles/Controls/CheckBox.xaml`

Contains:

- **36 lightweight styling `<StaticResource>` aliases per theme (72 total)** — 3 check states (Unchecked/Checked/Indeterminate) × 4 interaction states (Normal/PointerOver/Pressed/Disabled) × properties (Background/BorderBrush/GlyphFill) + 4 Foreground keys
- **`SimpleCheckBoxStyle`** with custom `ControlTemplate` using **CombinedStates** visual state group pattern (same as Material CheckBox)
- **SDS dimensions:** 16×16 box, 4px corner radius, 1px stroke, 10px glyph
- **Token mapping:**
  - Unchecked: `SimpleBackgroundDefaultDefaultBrush` + `SimpleBorderDefaultDefaultBrush`
  - Checked/Indeterminate: `SimpleBackgroundBrandDefaultBrush` (bg=border in SDS) + `SimpleIconBrandOnBrandBrush` glyph
  - Disabled unchecked: `SimpleBackgroundDisabledDefaultBrush` + `SimpleBorderDisabledDefaultBrush`
  - Disabled checked/indeterminate: `SimpleBackgroundDisabledDefaultBrush` + `SimpleIconDisabledOnDisabledBrush` glyph
  - Label: `SimpleTextDefaultDefaultBrush` / `SimpleTextDisabledDefaultBrush`

### 8. ComboBox.xaml — FULLY REWRITTEN ✅

**Path:** `src/library/Uno.Simple.WinUI/Styles/Controls/ComboBox.xaml`

Contains:

- **32 lightweight styling `<StaticResource>` aliases per theme (64 total)** — 18 ComboBox keys + 14 ComboBoxItem keys
- **3 named styles:**
  - `SimpleSelectFieldStyle` — main ComboBox style with full visual state coverage
  - `SimpleSelectFieldItemStyle` — ComboBoxItem style with selection highlighting
  - `SimpleSelectFieldErrorStyle` — error variant with danger border
- **Visual state groups:** CommonStates, FocusStates, DropDownStates
- **SDS dimensions:** 8px corner radius, 1px border, padding 16,10,12,10
- **Token mapping:**
  - Normal: `SimpleBackgroundDefaultDefaultBrush` bg, `SimpleBorderDefaultDefaultBrush` border, `SimpleTextDefaultDefaultBrush` text
  - Focus/Pressed: `SimpleBorderBrandSecondaryBrush` border
  - Error: `SimpleBorderDangerDefaultBrush` border
  - Disabled: `SimpleBackgroundDisabledDefaultBrush` + `SimpleTextDisabledDefaultBrush`
  - Items selected: `SimpleBackgroundBrandDefaultBrush` bg + `SimpleTextBrandOnBrandBrush` text
  - Popup: `SimpleBackgroundDefaultDefaultBrush` bg + `SimpleBorderDefaultDefaultBrush` border

### 9. ToggleSwitch.xaml — FULLY REWRITTEN ✅

**Path:** `src/library/Uno.Simple.WinUI/Styles/Controls/ToggleSwitch.xaml`

Contains:

- **10 lightweight styling `<StaticResource>` aliases per theme (20 total)** — Off track (bg+border), On track (bg), thumbs (off+on), disabled (track bg+border, thumb), label (normal+disabled)
- **`SimpleToggleSwitchStyle`** with simplified template using dual visual state groups:
  - **ToggleStates** (declared first) — controls Off/On visibility via Opacity
  - **CommonStates** (declared second) — Disabled state overrides all fill colors
- **SDS dimensions:** 40×24 track, 12px radius (pill shape), 16px thumb, 1px stroke
- **Token mapping:**
  - Off: `SimpleBackgroundDefaultSecondaryBrush` track + `SimpleBorderDefaultDefaultBrush` border + `SimpleIconDefaultSecondaryBrush` thumb
  - On: `SimpleBackgroundBrandDefaultBrush` track + `SimpleIconBrandOnBrandBrush` thumb
  - Disabled: `SimpleBackgroundDisabledDefaultBrush` track + `SimpleBorderDisabledDefaultBrush` border + `SimpleIconDisabledDefaultBrush` thumb
  - Label: `SimpleTextDefaultDefaultBrush` / `SimpleTextDisabledDefaultBrush`

### 10. RadioButton.xaml — FULLY REWRITTEN ✅

**Path:** `src/library/Uno.Simple.WinUI/Styles/Controls/RadioButton.xaml`

Contains:

- **24 lightweight styling `<StaticResource>` aliases per theme (48 total)** — 2 check states (Unchecked/Checked) × 4 interaction states × properties (Background/BorderBrush/DotFill) + 4 Foreground keys + Indeterminate fallback states
- **`SimpleRadioButtonStyle`** with custom `ControlTemplate` using **CombinedStates** visual state group pattern (same as CheckBox)
- **SDS dimensions:** 16×16 outer circle, 8×8 inner dot, 1px stroke
- **Token mapping:**
  - Unchecked: `SimpleBackgroundDefaultDefaultBrush` + `SimpleBorderDefaultDefaultBrush`
  - Checked: `SimpleBackgroundBrandDefaultBrush` (bg=border in SDS) + `SimpleIconBrandOnBrandBrush` dot
  - Disabled unchecked: `SimpleBackgroundDisabledDefaultBrush` + `SimpleBorderDisabledDefaultBrush`
  - Disabled checked: `SimpleBackgroundDisabledDefaultBrush` + `SimpleIconDisabledOnDisabledBrush` dot
  - Label: `SimpleTextDefaultDefaultBrush` / `SimpleTextDisabledDefaultBrush`

### 11. _Resources.xaml — VERIFIED ✅

All needed aliases already existed from prior work:
- `CheckBoxStyle` → `SimpleCheckBoxStyle`
- `RadioButtonStyle` → `SimpleRadioButtonStyle`
- `ToggleSwitchStyle` → `SimpleToggleSwitchStyle`
- `SelectFieldStyle` → `SimpleSelectFieldStyle`
- `SelectFieldItemStyle` → `SimpleSelectFieldItemStyle`
- `SelectFieldErrorStyle` → `SimpleSelectFieldErrorStyle`
- `ComboBoxStyle` → `SimpleSelectFieldStyle`

No changes needed.

### 12. PersonPicture.xaml — REWRITTEN ✅

**Path:** `src/library/Uno.Simple.WinUI/Styles/Controls/PersonPicture.xaml`

Contains:

- **ThemeDictionaries** with Light + Default themes containing sizing keys and color token aliases
- **Color mapping:** `SimpleBackgroundBrandDefaultBrush` background, `SimpleTextBrandOnBrandBrush` foreground, existing `PositiveColor`/`OnPositiveColor` for online badge
- **6 named styles** (3 circle + 3 square for small/medium/large):
  - `SimplePersonPictureStyle` (48px circle), `SimpleSmallPersonPictureStyle` (32px), `SimpleLargePersonPictureStyle` (80px)
  - Square variants: `SimpleSquarePersonPictureStyle`, `SimpleSmallSquarePersonPictureStyle`, `SimpleLargeSquarePersonPictureStyle`
- **Lightweight keys:** `SimplePersonPictureBackground`, `SimplePersonPictureForeground`, `SimplePersonPictureBadgeBackground`, `SimplePersonPictureBadgeForeground`, sizing keys per variant
- Initials FontWeight changed to SemiBold per SDS Avatar spec

### 13. TextBox.xaml — REWRITTEN ✅

**Path:** `src/library/Uno.Simple.WinUI/Styles/Controls/TextBox.xaml`

Contains:

- **ThemeDictionaries** with full state tokens (Normal/PointerOver/Focused/Disabled/Error/Header) — ~16 lightweight styling keys per theme
- **4 named styles:**
  - `SimpleInputTextBoxStyle` — SDS outlined input with border, placeholder support, header support
  - `SimpleOutlinedInputTextBoxStyle` — alias (BasedOn SimpleInputTextBoxStyle)
  - `SimpleInputTextBoxErrorStyle` — danger border override for all states
  - `SimpleInputTextBoxSmallStyle` — 32px MinHeight, 14px font
- **SDS dimensions:** 8px corner radius, 1px border, padding 16,10,16,10, 40px min height
- **Token mapping:**
  - Normal: `SimpleBackgroundDefaultDefaultBrush` bg + `SimpleBorderDefaultDefaultBrush` border
  - Focused: `SimpleBorderBrandSecondaryBrush` border
  - Error: `SimpleBorderDangerDefaultBrush` border
  - Disabled: `SimpleBackgroundDisabledDefaultBrush` + `SimpleTextDisabledDefaultBrush`
  - Placeholder: `SimpleTextDefaultTertiaryBrush`
  - Header: `SimpleTextDefaultSecondaryBrush`

### 14. PasswordBox.xaml — NEW ✅

**Path:** `src/library/Uno.Simple.WinUI/Styles/Controls/PasswordBox.xaml`

Contains:

- **Same visual treatment as TextBox** — ThemeDictionaries with same lightweight styling key pattern
- **`SimplePasswordBoxStyle`** with custom ControlTemplate including:
  - Header support via ContentPresenter
  - PlaceholderText support via ContentControl
  - Reveal button (glyph &#xE052;) with ButtonStates visual state group
  - Reveal button uses `SimplePasswordBoxRevealForeground` tokens
- **SDS dimensions:** 8px corner radius, 1px border, padding 16,10,16,10
- **State coverage:** Normal, PointerOver, Focused, Disabled — all with matching TextBox tokens

### 15. IconButton.xaml — NEW ✅

**Path:** `src/library/Uno.Simple.WinUI/Styles/Controls/IconButton.xaml`

Contains:

- **`SimpleBaseIconButtonStyle`** — shared setters (circular shape CornerRadius=9999, square aspect ratio)
- **5 variant styles** with own ControlTemplate + VSM:
  - `SimpleIconButtonPrimaryStyle` — reuses `SimplePrimaryButton*` ThemeResource keys
  - `SimpleIconButtonNeutralStyle` — reuses `SimpleNeutralButton*` keys
  - `SimpleIconButtonSubtleStyle` — reuses `SimpleSubtleButton*` keys
  - `SimpleIconButtonDangerPrimaryStyle` — reuses `SimpleDangerPrimaryButton*` keys
  - `SimpleIconButtonDangerSubtleStyle` — reuses `SimpleDangerSubtleButton*` keys
- **5 small size variants** — `SimpleSmallIconButton{Variant}Style` (32×32 vs 40×40)
- **SDS dimensions:** Medium=40×40, Small=32×32, CornerRadius=9999 (fully round)

### 16. Slider.xaml — NEW ✅

**Path:** `src/library/Uno.Simple.WinUI/Styles/Controls/Slider.xaml`

Contains:

- **ThemeDictionaries** with thumb + track color tokens for all states (Normal/PointerOver/Pressed/Disabled) — ~12 lightweight keys per theme
- **`SimpleSliderStyle`** with custom horizontal template:
  - DecreaseRect (filled region, brand-default)
  - Ellipse thumb (16px, brand-default)
  - IncreaseRect (empty region, brand-secondary)
  - Header support via ContentPresenter
- **SDS dimensions:** Track height=8px, thumb=16px, track corner radius=9999 (pill)
- **Token mapping:**
  - Normal: `SimpleBackgroundBrandDefaultBrush` thumb + filled track, `SimpleBackgroundBrandSecondaryBrush` empty track
  - Disabled: `SimpleBackgroundDisabledDefaultBrush` thumb + track

### 17. TextBlock.xaml — NEW ✅

**Path:** `src/library/Uno.Simple.WinUI/Styles/Controls/TextBlock.xaml`

Contains:

- **ThemeDictionaries** with semantic foreground aliases: `SimpleTextBlockForeground`, `SimpleTextBlockSecondaryForeground`, `SimpleTextBlockTertiaryForeground`, `SimpleTextBlockBrandForeground`, `SimpleTextBlockDangerForeground`, `SimpleTextBlockDisabledForeground`
- **`SimpleBaseTextBlockStyle`** — default body-base (16px Normal) with `SimpleFontFamily` (Inter)
- **12 SDS typography styles:**
  - `SimpleTitleHeroTextBlockStyle` (72px Bold), `SimpleTitlePageTextBlockStyle` (48px Bold)
  - `SimpleSubtitleTextBlockStyle` (32px Normal), `SimpleHeadingTextBlockStyle` (24px SemiBold)
  - `SimpleSubheadingTextBlockStyle` (20px Normal)
  - `SimpleBodyBaseTextBlockStyle` (16px Normal), `SimpleBodyStrongTextBlockStyle` (16px SemiBold)
  - `SimpleBodyEmphasisTextBlockStyle` (16px Italic)
  - `SimpleBodySmallTextBlockStyle` (14px Normal), `SimpleBodySmallStrongTextBlockStyle` (14px SemiBold)
  - `SimpleBodyCodeTextBlockStyle` (16px Roboto Mono)
  - `SimpleCaptionTextBlockStyle` (12px Normal)
- **19 Material-compatible aliases** mapping Material type scale names to SDS:
  - Display: Large→TitleHero, Medium→TitlePage, Small→Subtitle
  - Headline: Large→Heading, Medium→Subheading, Small→BodyStrong
  - Title: Large→TitlePage, Medium→Heading, Small→Subheading
  - Body: Large→Subheading, Medium→BodyBase, Small→BodySmall
  - Label: Large→BodyStrong, Medium→BodySmallStrong, Small/ExtraSmall→Caption
  - Caption: Large→BodySmall, Medium/Small→Caption

### 18. ToolTip.xaml — NEW ✅

**Path:** `src/library/Uno.Simple.WinUI/Styles/Controls/ToolTip.xaml`

Contains:

- **ThemeDictionaries** with 3 lightweight keys per theme: `SimpleToolTipBackground`, `SimpleToolTipBorderBrush`, `SimpleToolTipForeground`
- **`SimpleToolTipStyle`** with custom ControlTemplate (Border + ContentPresenter)
- **SDS dimensions:** Padding 12,8, CornerRadius=8, BorderThickness=1, MaxWidth=320
- **Token mapping:** `SimpleBackgroundDefaultDefaultBrush` bg, `SimpleBorderDefaultDefaultBrush` border, `SimpleTextDefaultDefaultBrush` text
- **Font:** 14px (`SimpleFontScale02`)

### 19. _Resources.xaml — UPDATED ✅

**Path:** `src/library/Uno.Simple.WinUI/Styles/Controls/_Resources.xaml`

Updates in this batch:
- **New implicit styles:** PasswordBox, Slider, ToolTip, TextBlock
- **New aliases:**
  - `InputTextBoxErrorStyle` → `SimpleInputTextBoxErrorStyle`
  - `InputTextBoxSmallStyle` → `SimpleInputTextBoxSmallStyle`
  - `PasswordBoxStyle` → `SimplePasswordBoxStyle`
  - 5 IconButton aliases + 5 SmallIconButton aliases
  - `SliderStyle` → `SimpleSliderStyle`
  - `ToolTipStyle` → `SimpleToolTipStyle`
- **Legacy alias:** `FilledPasswordBoxStyle` → `SimplePasswordBoxStyle`
- **Removed from TODO block:** PasswordBox, Slider, TextBlock entries

### 20. ListView.xaml — NEW ✅

**Path:** `src/library/Uno.Simple.WinUI/Styles/Controls/ListView.xaml`

Contains:

- **ThemeDictionaries** with 14 lightweight keys per theme covering container bg/border, item Normal/PointerOver/Pressed/Selected/PointerOverSelected/PressedSelected/Disabled states
- **`SimpleListViewItemStyle`** with CombinedStates visual state group (CommonStates × DisabledStates)
- **`SimpleListViewStyle`** with ScrollViewer-based template, ItemsStackPanel ItemsPanel
- **`SimpleDefaultListViewItemStyle`** and **`SimpleDefaultListViewStyle`** aliases
- **SDS dimensions:** MinHeight=40, Padding=8, CornerRadius=8
- **Token mapping:** Selected → `SimpleBackgroundBrandDefaultBrush` / `SimpleTextBrandOnBrandBrush`; PointerOver → `SimpleBackgroundDefaultSecondaryBrush`; Disabled → `SimpleTextDisabledDefaultBrush`

### 21. MenuFlyout.xaml — NEW ✅

**Path:** `src/library/Uno.Simple.WinUI/Styles/Controls/MenuFlyout.xaml`

Contains:

- **ThemeDictionaries** with 16 lightweight keys per theme
- **5 styles:** `SimpleMenuFlyoutPresenterStyle`, `SimpleMenuFlyoutItemStyle`, `SimpleToggleMenuFlyoutItemStyle`, `SimpleMenuFlyoutSubItemStyle`, `SimpleMenuFlyoutSeparatorStyle`
- Each style has full ControlTemplate + VisualStateManager (CommonStates)
- **SDS dimensions:** MinWidth=112, MaxWidth=320, ItemHeight=40, SeparatorHeight=1, CornerRadius=8, Padding=4
- **Token mapping:** Hover/Focused → `SimpleBackgroundBrandDefaultBrush` / `SimpleTextBrandOnBrandBrush`; Disabled → `SimpleTextDisabledDefaultBrush` / `SimpleIconDisabledDefaultBrush`; Separator → `SimpleBorderDefaultDefaultBrush`

### 22. ContentDialog.xaml — NEW ✅

**Path:** `src/library/Uno.Simple.WinUI/Styles/Controls/ContentDialog.xaml`

Contains:

- **ThemeDictionaries** with 5 lightweight keys per theme (Background, BorderBrush, SmokeLayer, TitleForeground, ContentForeground)
- **`SimpleContentDialogButtonStyle`** (BasedOn SubtleButtonStyle), **`SimpleContentDialogDefaultButtonStyle`** (BasedOn PrimaryButtonStyle)
- **`SimpleContentDialogStyle`** with full template: DialogShowingStates, ButtonsVisibilityStates, DefaultButtonStates VSMs
- **SDS dimensions:** CornerRadius=8, Padding=32, MinWidth=288, MaxWidth=560, ButtonSpacing=8
- **Title:** FontSize=24, FontWeight=SemiBold, left-aligned
- **Token mapping:** bg → `SimpleBackgroundDefaultDefaultBrush`, border → `SimpleBorderDefaultDefaultBrush`, scrim → `SimpleBackgroundUtilitiesScrimBrush`, title → `SimpleTextDefaultDefaultBrush`, content → `SimpleTextDefaultSecondaryBrush`

### 23. _Resources.xaml — UPDATED (Batch 2) ✅

**Path:** `src/library/Uno.Simple.WinUI/Styles/Controls/_Resources.xaml`

Updates in this batch:
- **New implicit styles:** ListView, ListViewItem, ContentDialog, MenuFlyoutPresenter, MenuFlyoutItem, MenuFlyoutSeparator, MenuFlyoutSubItem, ToggleMenuFlyoutItem
- **New aliases:**
  - `ListViewStyle` → `SimpleListViewStyle`
  - `ListViewItemStyle` → `SimpleListViewItemStyle`
  - `ContentDialogStyle` → `SimpleContentDialogStyle`
  - `MenuFlyoutPresenterStyle` → `SimpleMenuFlyoutPresenterStyle`
  - `MenuFlyoutItemStyle` → `SimpleMenuFlyoutItemStyle`
  - `MenuFlyoutSeparatorStyle` → `SimpleMenuFlyoutSeparatorStyle`
  - `MenuFlyoutSubItemStyle` → `SimpleMenuFlyoutSubItemStyle`
  - `ToggleMenuFlyoutItemStyle` → `SimpleToggleMenuFlyoutItemStyle`
- **Removed from TODO block:** ContentDialog, ListView, ListViewItem, MenuFlyout items, ToggleMenuFlyoutItem

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

The following control styles in `src/library/Uno.Simple.WinUI/Styles/Controls/` still need to be refactored to use SDS design tokens + lightweight styling:

| Control | File | Notes |
|---|---|---|
| AppBarButton | Not created | Needs SDS mapping |
| CalendarDatePicker | Not created | Needs SDS mapping |
| CalendarView | Not created | Needs SDS mapping |
| CommandBar | Not created | Needs SDS mapping |
| DatePicker | Not created | Needs SDS mapping |
| FlyoutPresenter | Not created | Needs SDS mapping |
| HyperlinkButton | Not created | Needs SDS mapping |
| NavigationView | Not created | Needs SDS mapping |
| PipsPager | Not created | Needs SDS mapping |
| ProgressBar / ProgressRing | Not created | Needs SDS mapping |
| RatingControl | Not created | Needs SDS mapping |
| ToggleButton | Not created | Needs SDS mapping |

For each control:
1. Study the SDS React component in `x:\src\sds\src\ui\primitives\{Control}\` for variants, states, and CSS token usage
2. Map CSS color tokens to the semantic brushes already defined in `ColorPalette.xaml`
3. Create `<StaticResource>` lightweight styling aliases in `ThemeDictionaries` (per variant × per state × per property)
4. Build the `ControlTemplate` with `{ThemeResource}` visual state setters
5. Update `_Resources.xaml` if new aliases are needed

### Sample Pages

Sample pages created/updated in `src/samples/SimpleSamplesApp/Content/Controls/`:
- **ListViewSamplePage** — Basic list, single selection, multiple selection, disabled items
- **MenuFlyoutSamplePage** — Basic menu, icons, separators, toggle items, sub-items, disabled items
- **ContentDialogSamplePage** — Basic (close-only), confirmation (primary + close), three-button (save dialog), custom content (with TextBox input), result display

Still need sample pages for:
- PersonPicture, TextBox, PasswordBox, IconButton, Slider, TextBlock, ToolTip

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
