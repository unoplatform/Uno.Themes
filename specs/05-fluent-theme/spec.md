# Spec 05 — Uno Fluent Theme (`Uno.Fluent.WinUI`)

| | |
|---|---|
| **Status** | Phases 1–2 **done** (2026-07-15 — library, seed→accent cascade, tests, docs; CI-parity suite green; implementation deltas in `progress.md` review log); Windows/WASM validation pending; Phase 3 next (S4(b) first) |
| **Owner** | Steve Bilogan |
| **Created** | 2026-07-14 |
| **Related** | `specs/01-design-tokens/`, `specs/02-semantic-brushes/`, `specs/03-seed-color-palette/`, `.specify/specs/semantic-abstraction-layer/spec.md`, `doc/semantic-styles.md`, `doc/fluent-getting-started.md`, `specs/lessons.md` (eager `<StaticResource>` resolution) |

---

## 1. Motivation & Goals

Uno Themes ships a **semantic abstraction layer**: theme-agnostic style keys
(`FilledButtonStyle`, `OutlinedTextBoxStyle`, …), a semantic color palette
(33 color roles → ~843 generated brushes), a semantic typography ramp
(19 type slots), and design tokens (`Space*`, `Radius*`). Today this layer has
two concrete design-system implementations that consume it — Material and
Simple (Cupertino predates the semantic layer and is only partially aligned).

**Fluent** — the design system of WinUI and the *default* look of every Uno
Platform / WinAppSDK app — is not represented. An app written against the
semantic layer cannot today say "give me the platform-default Fluent look"
without abandoning semantic keys and rewriting its XAML against WinUI's own
keys (`AccentButtonStyle`, `AccentFillColorDefaultBrush`, `BodyTextBlockStyle`, …).

**Goal:** a `FluentTheme` such that

```xml
<Application.Resources>
    <ResourceDictionary>
        <ResourceDictionary.MergedDictionaries>
            <XamlControlsResources xmlns="using:Microsoft.UI.Xaml.Controls" />
            <FluentTheme xmlns="using:Uno.Fluent" />
        </ResourceDictionary.MergedDictionaries>
    </ResourceDictionary>
</Application.Resources>
```

makes all of the following true:

- **G1** — Every semantic style key documented in `doc/semantic-styles.md`
  resolves to the visually-correct built-in Fluent style (or a documented
  nearest match). `<Button Style="{StaticResource FilledButtonStyle}"/>`
  renders as a Fluent accent button.
- **G2** — Every semantic color role and every generated semantic brush
  (`PrimaryBrush`, `OnSurfaceBrush`, `OutlineBrush`, …) carries Fluent color
  values, so `{ThemeResource PrimaryBrush}` in app XAML looks native under
  Fluent, in both Light and Dark themes.
- **G3** — The semantic typography slots (`DisplayLarge` … `CaptionSmall`,
  both the TextBlock styles and the `*FontFamily/*FontSize/*FontWeight/*CharacterSpacing`
  value keys) carry Fluent type-ramp values.
- **G4** — `FluentTheme` participates fully in the `BaseTheme` machinery:
  `Colors` (`ThemeColors`), seed-color palette generation, `DefaultDensity`,
  `DefaultCornerRadius`, hot reload, override precedence.
- **G5 (Phase 2)** — Setting `Colors.PrimarySeed` recolors the *built-in
  Fluent controls* too (accent cascade), not only the semantic brushes.
- **G6 (Phase 3)** — The documented semantic lightweight-styling keys
  (`FilledButtonBackground`, …) affect Fluent-styled controls, per-control,
  incrementally.

## 2. Non-Goals

- **N1** — Re-implementing / re-templating Fluent control templates. WinUI and
  Uno.UI own those templates; this library must never fork them. (See D1.)
- **N2** — Pixel-perfect parity of the semantic *state* model (opacity-based
  hover/pressed overlays) with Fluent's discrete per-state fill colors. The two
  models are structurally different; we map rest values and accept Fluent's own
  state behavior inside built-in templates. (See §6.4.)
- **N3** — Mica / Acrylic material backgrounds. v1 maps to the solid-color
  fallback tokens (`SolidBackgroundFillColor*`). Materials can be layered by
  the app itself.
- **N4** — A `Uno.Fluent.WinUI.Markup` C# Markup companion package (can follow
  later, mirroring `Uno.Simple.WinUI.Markup`).
- **N5** — UWP-lineage (`Uno.Fluent` non-WinUI) package. WinUI-lineage only,
  like `Uno.Simple.WinUI`.

## 3. Architecture Overview

### 3.1 An adapter, not a style library

Material, Cupertino, and Simple each ship their **own control templates** and
then alias semantic keys onto them
(`src/library/Uno.Simple.WinUI/Styles/Controls/_Resources.xaml`).
For Fluent the target styles **already exist in every app** as keyed resources
provided by `XamlControlsResources` (WinUI on Windows, Uno.UI everywhere else):
`AccentButtonStyle`, `DefaultButtonStyle`, `DefaultTextBoxStyle`, … and they
are already the *implicit* defaults for every control.

Therefore `Uno.Fluent.WinUI` is an **adapter**:

| Layer | Material/Simple | Fluent |
|---|---|---|
| Control templates | shipped by the library | **none** — provided by WinUI / Uno.UI |
| Implicit styles | declared in `_Resources.xaml` | **none** — Fluent is already implicit |
| Semantic style keys | alias → own styles | alias → **XamlControlsResources keys** |
| Semantic color roles | own palette values | **Fluent token values** |
| Semantic typography | own ramp values | **Fluent type-ramp values** |
| Bridge styles | n/a | ~2–4 tiny `BasedOn` styles for semantic variants Fluent lacks a key for |

The resulting merged dictionary is tiny (aliases + palette + typography), and
the library carries near-zero maintenance drift risk against WinUI.

### 3.2 Library layout

New project `src/library/Uno.Fluent.WinUI/`, cloned structurally from
`Uno.Simple.WinUI` (the newest, smallest theme):

```
src/library/Uno.Fluent.WinUI/
├── Uno.Fluent.WinUI.csproj          # clone of Uno.Simple.WinUI.csproj (PackageId/AssemblyName=Uno.Fluent.WinUI)
├── fluent-common.props              # clone of simple-common.props (XamlMergeInput globs, mobile/not_mobile ns, imports ..\xamlmerge.targets)
├── FluentTheme.cs                   # : BaseTheme (see §3.3)
├── FluentConstants.cs               # internal resource-path constants (mirrors SimpleConstants)
├── AssemblyInfo.cs
├── LinkerConfig.xamarin.xml
├── build/
│   ├── Uno.Fluent.WinUI.targets
│   └── Package.targets
└── Styles/
    ├── Application/
    │   ├── BaseDictionaries.xaml    # merges shared Converters + SharedTypography + Fluent Typography.xaml
    │   ├── ColorPalette.xaml        # semantic role → Fluent values (EXCLUDED from XamlMergeInput; loaded via ctor colorOverride path)
    │   └── Typography.xaml          # Fluent type-ramp values for the 19 semantic slots
    └── Controls/
        ├── _Resources.xaml          # semantic alias table (§5) — the heart of the library
        ├── Button.xaml              # FluentTextButtonStyle / FluentIconButtonStyle bridge styles only (§5.4)
        └── TextBlock.xaml           # semantic TextBlock styles DisplayLarge…CaptionSmall (§7.3)
```

Namespace `Uno.Fluent`, public type `FluentTheme`, internal `FluentConstants`.
No `Assets/`, no fonts package (D11: Fluent uses the platform default font).

### 3.3 `FluentTheme` class

```csharp
namespace Uno.Fluent;

/// <summary>
/// Fluent (WinUI-default) theme resources: maps the Uno Themes semantic
/// styles, colors, and typography onto the built-in Fluent styles provided
/// by XamlControlsResources.
/// </summary>
public class FluentTheme(ResourceDictionary colorOverride = null, ResourceDictionary fontOverride = null)
    : BaseTheme(GetFluentColorOverride(colorOverride), fontOverride)
{
    // Fluent's default colors come from the platform (accent + neutrals),
    // not from a generated seed palette. Seed generation stays opt-in,
    // consistent with the repo-wide opt-in decision (commit 09187371).
    protected override Color? DefaultPrimarySeed => null;

    // When a consumer opts into a seed, preserve source chroma: Windows
    // accent colors are often corporate colors that must not be re-saturated
    // by the M3 minimum-chroma floor.
    protected override bool UseHighFidelityColors => true;

    protected override string DefaultStylesSource => FluentConstants.ResourcePaths.MergedPages;

    private static ResourceDictionary GetFluentColorOverride(ResourceDictionary colorOverride)
    {
        var fluentColors = new ResourceDictionary { Source = new Uri(FluentConstants.ResourcePaths.ColorPalette) };
        if (colorOverride is { })
        {
            fluentColors.SafeMerge(colorOverride);   // requires InternalsVisibleTo (§11.2)
        }
        return fluentColors;
    }
}
```

`BaseTheme` then provides, unmodified: color-layer rebuild on
seed/override change, `Space*`/`Radius*`/density token generation
(`BaseTheme.ScaleGeneration.cs`), hot reload (`BaseTheme.HotReload.cs`), and
the override-precedence contract verified by `Given_ColorOverridePrecedence`.

### 3.4 Resource topology and merge order

```
Application.Resources
└── MergedDictionaries
    ├── [0] XamlControlsResources        ← REQUIRED, must precede FluentTheme
    └── [1] FluentTheme : ResourceDictionary
        ├── Source = ms-appx:///Uno.Fluent.WinUI/Generated/mergedpages.xaml
        │     └── BaseDictionaries.xaml → Converters, SharedTypography, Typography.xaml
        │     └── _Resources.xaml (semantic aliases), Button.xaml, TextBlock.xaml
        └── MergedDictionaries (dynamic, rebuilt by UpdateSource())
            ├── spacing scale   (generated)
            ├── shape scale     (generated)
            ├── density scale   (generated)
            └── colors layer:
                SharedColors.xaml (brushes)
                └── SharedColorPalette.xaml   (M3 defaults)
                    ⊕ FluentTheme ColorPalette.xaml   (Fluent values — §6)
                    ⊕ seed palette (only if Colors.PrimarySeed set)
                    ⊕ Colors.OverrideDictionary (consumer)
```

**Hard requirement:** every `<StaticResource ResourceKey="…"/>` alias in this
library targets a key defined in a *sibling* merged dictionary
(`XamlControlsResources`), which must already be loaded when `FluentTheme`'s
XAML parses. This is the single load-bearing platform assumption of the whole
design and is validated by **Spike S1** before any scaffolding is built
(§14.1). `specs/lessons.md` documents a directly-relevant failure mode:
`<StaticResource>` aliases inside `ThemeDictionaries` resolve **eagerly at
parse time** and cross-dictionary resolution has been unreliable on Uno in at
least one shipped scenario (Simple typography weights). The documentation-level
usage requirement ("`XamlControlsResources` first, `FluentTheme` second")
matches what `MaterialTheme`/`SimpleTheme` already require of consumers.

**Fallback mechanism (M-CODE), if S1 shows XAML aliasing is unreliable on any
target:** perform the aliasing in code instead. `FluentTheme` overrides
`AddThemeSpecificResources()` and copies the resolved objects under semantic
keys (`this["FilledButtonStyle"] = Application.Current.Resources["AccentButtonStyle"]`,
palette colors likewise). Code-level lookup through
`Application.Current.Resources` is late-bound and immune to the eager-parse
problem; it also enables type-keyed implicit-style lookups
(`Application.Current.Resources[typeof(NavigationView)]`) for controls whose
default style has no public key. The XAML-alias form is preferred for hot
reload and readability; M-CODE is the insurance policy.

## 4. Decision log

| # | Decision | Rationale / alternatives rejected |
|---|---|---|
| **D1** | Adapter library; never fork Fluent templates. | Forking duplicates Uno.UI/WinUI and drifts every WinUI release. Rejected: "Material-style" full restyle. |
| **D2** | Package/assembly `Uno.Fluent.WinUI`, namespace `Uno.Fluent`, type `FluentTheme`. | Mirrors `Uno.Simple.WinUI`/`Uno.Simple`/`SimpleTheme` naming exactly. |
| **D3** | `FluentTheme : BaseTheme`, layered exactly like `SimpleTheme` (palette via ctor `colorOverride`, styles via `DefaultStylesSource`). | Gets seed colors, tokens, hot reload, precedence for free; keeps one theme lifecycle to maintain. |
| **D4** | `DefaultPrimarySeed => null` (no seed by default); `UseHighFidelityColors => true`. | Fluent's default colors are the platform's. Seed stays opt-in (consistent with 09187371). High fidelity because Windows accent/corporate colors must keep their chroma. |
| **D5** | Color mapping is **role → Fluent token name** (normative), with concrete hexes captured from the running platform (Spike S2), not hand-transcribed. | Token names are the stable contract; hexes vary by WinUI version and are easy to get subtly wrong. |
| **D6** | **DECIDED by S1 (2026-07-14): mechanism C** — the color palette is built in code (per-theme-branch token values resolved from `Application.Current.Resources` during `UpdateSource()`), never via per-branch XAML aliases. Mechanism A remains in use for *style* aliases only. | S1 case 4 reproduced the `specs/lessons.md` failure mode: per-branch `<StaticResource>` color aliases resolve eagerly against the **ambient** theme, so both branches carry the same value. C is late-bound, branch-correct, and on Windows can read the live system accent. See `spike-results.md` §S1. |
| **D7** | Typography rule: **where Fluent's type ramp has a counterpart slot, adopt its size/weight; where it doesn't, keep the shared (M3) size and apply Fluent weight conventions** (SemiBold for Display/Headline/Title/Label emphasis slots, Regular for Body/Caption). `CharacterSpacing = 0` everywhere. FontFamily = platform default via `ContentControlThemeFontFamily`. | Fluent's ramp has 8 slots, semantic has 19 — a pure projection would collapse slots into duplicates and destroy the app's visual hierarchy. Hybrid preserves progression while looking Fluent. |
| **D8** | **Nearest-match over GAP**: every semantic style key resolves under FluentTheme, even where Fluent has no concept (FAB, Elevated). Divergence from Simple (which left `ElevatedButtonStyle`/`CommandBarStyle` as GAPs) is deliberate. | The point of a Fluent theme is portability: an app authored under Material must not crash when switched to Fluent. Fluent *always* has a functional neighbor (worst case `DefaultButtonStyle`). Exceptions: a key is left GAP only if the control itself doesn't exist on the platform. |
| **D9** | No implicit styles in `_Resources.xaml`. | Fluent already *is* the implicit default; re-declaring implicit styles would be a no-op at best and an override-ordering hazard at worst. |
| **D10** | Ship 2 bridge styles only: `FluentTextButtonStyle`, `FluentIconButtonStyle` (both `BasedOn="{StaticResource DefaultButtonStyle}"`, setters only, **no template**). | Fluent has no public keyed "subtle/text button". A `BasedOn` style with transparent rest Background/Border reproduces Fluent's subtle-button behavior because the template's VSM still applies `ButtonBackgroundPointerOver`/`Pressed` on interaction. No template = no fork (D1 upheld). |
| **D11** | No font package dependency. All 19 `*FontFamily` slots alias `ContentControlThemeFontFamily` (Segoe UI (Variable) on Windows, the platform/Uno default elsewhere). | Shipping Segoe is not licensable; platform default *is* the Fluent answer. |
| **D12** | Phase 2 (seed → accent) overrides the full accent token closure — `SystemAccentColor` + `Light1–3/Dark1–3` **and** the `AccentFillColor*` / `AccentTextFillColor*` / `TextOnAccentFillColor*` colors+brushes directly. | XCR resolves its internal `{StaticResource SystemAccentColorDark1}` references eagerly at *its own* load time, before `FluentTheme` merges; overriding only `SystemAccentColor*` provably does not cascade. Exact closure enumerated by Spike S4. |
| **D13** | Phase 3 (lightweight bridging) is per-control and incremental: FluentTheme defines the semantic lightweight keys (defaulting to Fluent token values) and *re-points Fluent's per-control resources at them*. Rollout order: Button → TextBox → CheckBox → RadioButton → ToggleSwitch → Slider. | Doing all controls at once is a huge surface with per-key testing needs; the chosen order matches the repo's "Minimum Test Additions" table and the most-customized controls first. |
| **D14** | v1 runtime tests are hosted in **SimpleSampleApp** (the CI host), instantiating `new FluentTheme()` scoped to a test container (pattern of `Given_SemanticStyles.CreateThemedContainer`). A dedicated `FluentSampleApp` head is Phase 4. | `specs/lessons.md` verification trap: minimal hosts mask cross-dictionary resolution bugs; SimpleSampleApp merges `XamlControlsResources` at app scope (see its `App.xaml`), which is exactly FluentTheme's required topology. A new head is heavy and adds no verification power for phases 1–3. |
| **D15** | Spec + spike results live in `specs/05-fluent-theme/` (this folder), continuing the numbered series 01–04. | Repo convention. |
| **D16** | **Never chain aliases** (added from S1, 2026-07-14): every semantic key in `_Resources.xaml` must target a *concrete* key (an XCR style or a locally-defined bridge style) directly — an alias whose `ResourceKey` is itself an alias does not resolve on Uno. | S1 case 2: alias-of-alias yields `"Couldn't statically resolve resource"` and a null lookup. Alias → concrete style within the same merged bundle is proven fine (Simple's `_Resources.xaml` → `Button.xaml`). Guarded permanently by `Given_FluentAliasResolution.When_AliasOfAlias_DoesNotResolve`. |

## 5. Semantic style alias mapping (Layer 1)

### 5.1 Mechanism

`Styles/Controls/_Resources.xaml`, same shape as Simple's, minus implicit
styles (D9):

```xml
<!-- Semantic aliases (design-system-agnostic) → built-in Fluent styles -->
<StaticResource x:Key="FilledButtonStyle"   ResourceKey="AccentButtonStyle" />
<StaticResource x:Key="OutlinedButtonStyle" ResourceKey="DefaultButtonStyle" />
...
```

**Confidence legend** for the "Fluent target" column (updated from Spike S3,
Skia desktop run 2026-07-14 — see `spike-results.md`; Windows/WASM probe
still pending):
- ✅ = verified present on Uno Skia (S3 probe).
- Ⓜ = **absent on Uno Skia** → implement via mechanism M-CODE (§3.4)
  type-keyed implicit-style lookup; re-check availability on Windows/WASM
  during Phase 1.

### 5.2 Full mapping table

#### Button

| Semantic key | Fluent target | Conf. | Notes |
|---|---|---|---|
| `FilledButtonStyle` | `AccentButtonStyle` | ✅ | The Fluent emphasized button. |
| `ElevatedButtonStyle` | `DefaultButtonStyle` | ✅ | Fluent has no elevation; standard button is the functional neighbor (D8; diverges from Simple's GAP). |
| `FilledTonalButtonStyle` | `DefaultButtonStyle` | ✅ | Fluent standard button ≈ tonal (neutral fill). |
| `OutlinedButtonStyle` | `DefaultButtonStyle` | ✅ | Fluent standard button carries a stroke; closest to outlined. |
| `TextButtonStyle` | `FluentTextButtonStyle` (shipped, §5.4) | ✅ | Fluent "subtle button" approximation. |
| `IconButtonStyle` | `FluentIconButtonStyle` (shipped, §5.4) | ✅ | Square padding, transparent rest. |

#### Floating Action Button (Material-only concept; D8 nearest-match)

| Semantic key | Fluent target | Conf. |
|---|---|---|
| `FabStyle`, `SmallFabStyle`, `LargeFabStyle` | `AccentButtonStyle` | ✅ |
| `SecondaryFabStyle`, `SecondarySmallFabStyle`, `SecondaryLargeFabStyle` | `DefaultButtonStyle` | ✅ |
| `TertiaryFabStyle`, `TertiarySmallFabStyle`, `TertiaryLargeFabStyle` | `DefaultButtonStyle` | ✅ |
| `SurfaceFabStyle`, `SurfaceSmallFabStyle`, `SurfaceLargeFabStyle` | `DefaultButtonStyle` | ✅ |

*(No size differentiation: Fluent buttons size to content; FAB sizing is a
Material shape concern that does not survive translation.)*

#### ToggleButton

| Semantic key | Fluent target | Conf. |
|---|---|---|
| `TextToggleButtonStyle` | `DefaultToggleButtonStyle` | ✅ |
| `IconToggleButtonStyle` | `DefaultToggleButtonStyle` | ✅ |

#### TextBox / PasswordBox

| Semantic key | Fluent target | Conf. | Notes |
|---|---|---|---|
| `FilledTextBoxStyle` | `DefaultTextBoxStyle` | ✅ | Fluent TextBox (fill + underline) serves both variants. |
| `OutlinedTextBoxStyle` | `DefaultTextBoxStyle` | ✅ | |
| `FilledPasswordBoxStyle` | `DefaultPasswordBoxStyle` | ✅ | |
| `OutlinedPasswordBoxStyle` | `DefaultPasswordBoxStyle` | ✅ | |

#### HyperlinkButton

| Semantic key | Fluent target | Conf. |
|---|---|---|
| `HyperlinkButtonStyle` | `DefaultHyperlinkButtonStyle` | ✅ |
| `SecondaryHyperlinkButtonStyle` | `DefaultHyperlinkButtonStyle` | ✅ |

#### Selection & range controls

| Semantic key | Fluent target | Conf. |
|---|---|---|
| `ComboBoxStyle` | `DefaultComboBoxStyle` | ✅ |
| `ComboBoxItemStyle` | `DefaultComboBoxItemStyle` | ✅ |
| `CheckBoxStyle` | `DefaultCheckBoxStyle` | ✅ |
| `RadioButtonStyle` | `DefaultRadioButtonStyle` | ✅ |
| `ToggleSwitchStyle` | `DefaultToggleSwitchStyle` | ✅ |
| `SliderStyle` | `DefaultSliderStyle` | ✅ |

#### Progress

| Semantic key | Fluent target | Conf. |
|---|---|---|
| `ProgressBarStyle` | `DefaultProgressBarStyle` | ✅ |
| `ProgressRingStyle` | `DefaultProgressRingStyle` | Ⓜ (`typeof(ProgressRing)`) |

#### Lists

| Semantic key | Fluent target | Conf. |
|---|---|---|
| `ListViewStyle` | `DefaultListViewStyle` | Ⓜ (`typeof(ListView)`) |
| `ListViewItemStyle` | `DefaultListViewItemStyle` | ✅ |

#### Dialogs, bars, navigation

| Semantic key | Fluent target | Conf. |
|---|---|---|
| `ContentDialogStyle` | `DefaultContentDialogStyle` | ✅ |
| `CommandBarStyle` | `DefaultCommandBarStyle` | Ⓜ (`typeof(CommandBar)`) |
| `AppBarButtonStyle` | `DefaultAppBarButtonStyle` | ✅ |
| `NavigationViewStyle` | Ⓜ (`typeof(NavigationView)`) — confirmed absent on Skia |
| `NavigationViewItemStyle` | Ⓜ (`typeof(NavigationViewItem)`) — confirmed absent on Skia |

#### Pickers & calendar

| Semantic key | Fluent target | Conf. |
|---|---|---|
| `CalendarViewStyle` | `DefaultCalendarViewStyle` | ✅ |
| `CalendarDatePickerStyle` | `DefaultCalendarDatePickerStyle` | Ⓜ (`typeof(CalendarDatePicker)`) |
| `DatePickerStyle` | `DefaultDatePickerStyle` | ✅ |

#### Media

| Semantic key | Fluent target | Conf. |
|---|---|---|
| `MediaTransportControlsStyle` | `DefaultMediaTransportControlsStyle` | ✅ (present on Skia; re-verify per platform — control availability varies; D8 exception allows GAP where the control itself is absent). |

#### Pagers & rating

| Semantic key | Fluent target | Conf. |
|---|---|---|
| `PipsPagerStyle` | `DefaultPipsPagerStyle` | Ⓜ (`typeof(PipsPager)`) |
| `RatingControlStyle` | `DefaultRatingControlStyle` | Ⓜ (`typeof(RatingControl)`) |

#### Flyouts & menus

| Semantic key | Fluent target | Conf. |
|---|---|---|
| `FlyoutPresenterStyle` | `DefaultFlyoutPresenterStyle` | ✅ |
| `MenuFlyoutPresenterStyle` | `DefaultMenuFlyoutPresenterStyle` | ✅ |
| `MenuFlyoutItemStyle` | `DefaultMenuFlyoutItemStyle` | ✅ |
| `MenuFlyoutSeparatorStyle` | `DefaultMenuFlyoutSeparatorStyle` | Ⓜ (`typeof(MenuFlyoutSeparator)`) |
| `MenuFlyoutSubItemStyle` | `DefaultMenuFlyoutSubItemStyle` | ✅ |
| `ToggleMenuFlyoutItemStyle` | `DefaultToggleMenuFlyoutItemStyle` | ✅ |
| `RadioMenuFlyoutItemStyle` | `DefaultRadioMenuFlyoutItemStyle` | ✅ |

### 5.3 Spike S3 status

S3 probed every candidate key against `Application.Current.Resources` in the
CI host on **Skia desktop (2026-07-14)**; the tables above reflect that run
(raw output in `spike-results.md`). Remaining before Phase 1 closes: repeat
the probe on Windows and WASM, and verify the M-CODE type-keyed lookup
actually returns the implicit style for each Ⓜ entry. The final
`_Resources.xaml` contains only keys proven present; Ⓜ entries are populated
in code.

### 5.4 Shipped bridge styles (the only styles this library authors)

```xml
<!-- Styles/Controls/Button.xaml -->

<!-- Fluent "subtle button": transparent at rest; the built-in template's VSM
     still applies ButtonBackgroundPointerOver/Pressed on interaction, which
     is exactly Fluent's subtle-button hover behavior. NO custom template. -->
<Style x:Key="FluentTextButtonStyle"
       TargetType="Button"
       BasedOn="{StaticResource DefaultButtonStyle}">
    <Setter Property="Background" Value="Transparent" />
    <Setter Property="BorderBrush" Value="Transparent" />
</Style>

<Style x:Key="FluentIconButtonStyle"
       TargetType="Button"
       BasedOn="{StaticResource DefaultButtonStyle}">
    <Setter Property="Background" Value="Transparent" />
    <Setter Property="BorderBrush" Value="Transparent" />
    <Setter Property="Padding" Value="8" />
    <Setter Property="MinWidth" Value="40" />
    <Setter Property="MinHeight" Value="40" />
</Style>
```

Constraint (D1): bridge styles may contain **setters only, never a
`Template` setter**. If a semantic variant cannot be expressed without
re-templating, it maps to the plain Fluent style instead.

## 6. Color mapping (Layer 2)

### 6.1 Direction

Two independent flows; do not conflate them:

- **Forward (v1, this section):** Fluent token values → semantic color roles →
  the ~843 generated semantic brushes. Makes *semantic-keyed app XAML* look
  Fluent. Implemented by `ColorPalette.xaml` via the `colorOverride`
  constructor path (identical topology to `SimpleTheme`'s grayscale palette:
  merged **below** the seed palette and consumer overrides, so
  `Given_ColorOverridePrecedence` semantics hold unchanged).
- **Reverse (Phase 2, §9):** seed-generated palette → Fluent accent tokens →
  *built-in Fluent controls*. Makes stock Fluent controls follow the brand.

### 6.2 Mechanism (D6)

Preferred form — per-theme-branch `<StaticResource>` alias, e.g.:

```xml
<ResourceDictionary.ThemeDictionaries>
    <ResourceDictionary x:Key="Light">
        <StaticResource x:Key="PrimaryColor" ResourceKey="SystemAccentColor" />
        <StaticResource x:Key="OnSurfaceColor" ResourceKey="TextFillColorPrimary" />
        ...
    </ResourceDictionary>
    <ResourceDictionary x:Key="Default"> ... </ResourceDictionary>
</ResourceDictionary.ThemeDictionaries>
```

This is the **exact pattern `specs/lessons.md` warns about** (eager parse-time
resolution across sibling dictionaries, per theme branch), and **Spike S1
case 4 confirmed it is broken on Uno**: both theme branches resolved the
ambient theme's value. **Decision (D6): mechanism C** — `FluentTheme` resolves
each token per theme branch in code during `UpdateSource()` and writes literal
`Color` values into the palette dictionary programmatically (also the path
that lets Windows track the user's real accent color). The XAML snippet above
is retained only to document why it is *not* used.

### 6.3 Full role table

Normative column: **Fluent token**. The "informative value" columns are the
WinUI 3 defaults for the standard blue accent, **to be captured/verified per
platform by Spike S2** — do not hand-copy them into code. Entries marked
*(S2)* are known only approximately at spec time.

#### Light theme

| Semantic role | Fluent token | Informative value |
|---|---|---|
| `PrimaryColor` | `SystemAccentColor` | `#0078D4` |
| `OnPrimaryColor` | `TextOnAccentFillColorPrimary` | `#FFFFFF` |
| `PrimaryContainerColor` | `SystemAccentColorLight2` | *(S2)* light accent |
| `OnPrimaryContainerColor` | `SystemAccentColorDark2` | *(S2)* dark accent |
| `PrimaryInverseColor` | `SystemAccentColorLight2` | *(S2)* — accent usable on dark surfaces |
| `PrimaryVariantDarkColor` | `SystemAccentColorDark1` | *(S2)* |
| `PrimaryVariantLightColor` | `SystemAccentColorLight1` | *(S2)* |
| `SecondaryColor` | `SystemAccentColorDark1` | *(S2)* |
| `OnSecondaryColor` | `TextOnAccentFillColorPrimary` | `#FFFFFF` |
| `SecondaryContainerColor` | `SystemAccentColorLight3` | *(S2)* |
| `OnSecondaryContainerColor` | `SystemAccentColorDark3` | *(S2)* |
| `SecondaryVariantDarkColor` | `SystemAccentColorDark2` | *(S2)* |
| `SecondaryVariantLightColor` | `SystemAccentColor` | `#0078D4` |
| `TertiaryColor` | `SystemAccentColorDark2` | *(S2)* |
| `OnTertiaryColor` | `TextOnAccentFillColorPrimary` | `#FFFFFF` |
| `TertiaryContainerColor` | `SystemAccentColorLight3` | *(S2)* |
| `OnTertiaryContainerColor` | `SystemAccentColorDark3` | *(S2)* |
| `ErrorColor` | `SystemFillColorCritical` | `#C42B1C` |
| `OnErrorColor` | `TextOnAccentFillColorPrimary` | `#FFFFFF` |
| `ErrorContainerColor` | `SystemFillColorCriticalBackground` | `#FDE7E9` |
| `OnErrorContainerColor` | `SystemFillColorCritical` | `#C42B1C` |
| `BackgroundColor` | `SolidBackgroundFillColorBase` | `#F3F3F3` |
| `OnBackgroundColor` | `TextFillColorPrimary` | `#E4000000` |
| `SurfaceColor` | `SolidBackgroundFillColorTertiary` | `#F9F9F9` *(S2)* — the elevated/card layer; solid fallback per N3 |
| `OnSurfaceColor` | `TextFillColorPrimary` | `#E4000000` |
| `SurfaceVariantColor` | `SolidBackgroundFillColorSecondary` | `#EEEEEE` *(S2)* |
| `OnSurfaceVariantColor` | `TextFillColorSecondary` | *(S2)* ~60% black |
| `SurfaceInverseColor` | dark-branch `SolidBackgroundFillColorBase` | `#202020` — cross-theme value; needs mechanism C or a literal |
| `OnSurfaceInverseColor` | `TextFillColorInverse` | `#FFFFFF` *(S2)* |
| `SurfaceTintColor` | `SystemAccentColor` | `#0078D4` |
| `OutlineColor` | `ControlStrongStrokeColorDefault` | *(S2)* ~45% black — checkbox/radio rest borders |
| `OutlineVariantColor` | `DividerStrokeColorDefault` | *(S2)* ~6% black |
| `ShadowColor` | *(keep shared default)* | `#33000000` — Fluent has no shadow color token |

#### Dark theme

| Semantic role | Fluent token | Informative value |
|---|---|---|
| `PrimaryColor` | `SystemAccentColorLight2` | *(S2)* — Fluent dark-theme accent fill is Light2, not the base accent |
| `OnPrimaryColor` | `TextOnAccentFillColorPrimary` (dark branch) | *(S2)* — near-black in dark theme |
| `PrimaryContainerColor` | `SystemAccentColorDark1` | *(S2)* |
| `OnPrimaryContainerColor` | `SystemAccentColorLight3` | *(S2)* |
| `PrimaryInverseColor` | `SystemAccentColor` | `#0078D4` |
| `PrimaryVariantDarkColor` | `SystemAccentColorDark1` | *(S2)* |
| `PrimaryVariantLightColor` | `SystemAccentColorLight1` | *(S2)* |
| `SecondaryColor` | `SystemAccentColorLight1` | *(S2)* |
| `OnSecondaryColor` | `TextOnAccentFillColorPrimary` (dark branch) | *(S2)* |
| `SecondaryContainerColor` | `SystemAccentColorDark2` | *(S2)* |
| `OnSecondaryContainerColor` | `SystemAccentColorLight3` | *(S2)* |
| `SecondaryVariantDarkColor` | `SystemAccentColorDark2` | *(S2)* |
| `SecondaryVariantLightColor` | `SystemAccentColorLight2` | *(S2)* |
| `TertiaryColor` | `SystemAccentColorLight3` | *(S2)* |
| `OnTertiaryColor` | `TextOnAccentFillColorPrimary` (dark branch) | *(S2)* |
| `TertiaryContainerColor` | `SystemAccentColorDark3` | *(S2)* |
| `OnTertiaryContainerColor` | `SystemAccentColorLight2` | *(S2)* |
| `ErrorColor` | `SystemFillColorCritical` (dark branch) | `#FF99A4` |
| `OnErrorColor` | dark-branch on-accent | *(S2)* near-black |
| `ErrorContainerColor` | `SystemFillColorCriticalBackground` (dark) | `#442726` |
| `OnErrorContainerColor` | `SystemFillColorCritical` (dark) | `#FF99A4` |
| `BackgroundColor` | `SolidBackgroundFillColorBase` | `#202020` |
| `OnBackgroundColor` | `TextFillColorPrimary` | `#FFFFFF` |
| `SurfaceColor` | `SolidBackgroundFillColorTertiary` | `#282828` *(S2)* |
| `OnSurfaceColor` | `TextFillColorPrimary` | `#FFFFFF` |
| `SurfaceVariantColor` | `SolidBackgroundFillColorSecondary` | `#1C1C1C` *(S2)* |
| `OnSurfaceVariantColor` | `TextFillColorSecondary` | *(S2)* ~77% white |
| `SurfaceInverseColor` | light-branch `SolidBackgroundFillColorBase` | `#F3F3F3` |
| `OnSurfaceInverseColor` | `TextFillColorInverse` | *(S2)* near-black |
| `SurfaceTintColor` | `SystemAccentColorLight2` | *(S2)* |
| `OutlineColor` | `ControlStrongStrokeColorDefault` | *(S2)* ~60% white |
| `OutlineVariantColor` | `DividerStrokeColorDefault` | *(S2)* ~8% white |
| `ShadowColor` | *(keep shared default)* | shared dark default |

**Judgment calls embedded above (all overridable by consumers via
`Colors.OverrideDictionary`):**
- Fluent has a *single* accent; Material's Secondary/Tertiary are mapped to
  darker/lighter shades of that accent rather than to neutrals, so
  Secondary/Tertiary-styled UI stays visibly "branded" and hierarchical.
- `Surface*` maps to the **solid** background stack (N3), choosing the
  layer-appropriate `SolidBackgroundFillColor*` token per role.
- `OnErrorContainerColor` reuses `SystemFillColorCritical` (Fluent defines no
  "on critical container" token; critical-on-critical-background is the WinUI
  InfoBar pattern).

### 6.4 State-model mismatch (documented behavior, not a bug)

Semantic brushes encode interaction states as opacity variants
(`PrimaryHoverBrush` = `PrimaryColor` @ hover opacity, per
`specs/01-design-tokens/opacity-states.md`). Fluent encodes states as discrete
colors (`ControlFillColorSecondary` on hover, …). Under FluentTheme:
- Inside **built-in templates**, Fluent's own state resources apply — correct
  by definition.
- **App XAML** using semantic state brushes (`PrimaryHoverBrush`) gets
  opacity-derived variants of the Fluent rest colors — visually consistent,
  not token-identical to Fluent's hover colors. This is accepted (N2) and must
  be stated in `doc/fluent-getting-started.md`.

## 7. Typography mapping (Layer 3)

### 7.1 Reference — Fluent (WinUI) type ramp

| Fluent slot | Size | Weight | XCR TextBlock style key |
|---|---|---|---|
| Caption | 12 | Regular | `CaptionTextBlockStyle` |
| Body | 14 | Regular | `BodyTextBlockStyle` |
| Body Strong | 14 | SemiBold | `BodyStrongTextBlockStyle` |
| Body Large | 18 | Regular | *(no XCR style)* |
| Subtitle | 20 | SemiBold | `SubtitleTextBlockStyle` |
| Title | 28 | SemiBold | `TitleTextBlockStyle` |
| Title Large | 40 | SemiBold | `TitleLargeTextBlockStyle` |
| Display | 68 | SemiBold | `DisplayTextBlockStyle` |

### 7.2 Slot-by-slot decision (rule D7)

Shared column = current defaults in `SharedTypography.xaml` (verified from
source, 2026-07-14). All FluentTheme `*CharacterSpacing` = `0`. All
`*FontFamily` = alias → `ContentControlThemeFontFamily` (D11).

| Semantic slot | Shared (M3) | **FluentTheme value** | Derivation |
|---|---|---|---|
| `DisplayLarge` | 57 / Normal | **68 / SemiBold** | = Fluent Display |
| `DisplayMedium` | 45 / Normal | **54 / SemiBold** | interpolated: 68 × (45⁄57) ≈ 54; no Fluent slot |
| `DisplaySmall` | 36 / Normal | **40 / SemiBold** | = Fluent Title Large |
| `HeadlineLarge` | 32 / Normal | **32 / SemiBold** | keep size (no Fluent slot between 40 and 28); Fluent title weight |
| `HeadlineMedium` | 28 / Normal | **28 / SemiBold** | = Fluent Title |
| `HeadlineSmall` | 24 / Normal | **24 / SemiBold** | keep size; Fluent title weight |
| `TitleLarge` | 22 / Normal | **20 / SemiBold** | = Fluent Subtitle |
| `TitleMedium` | 16 / Medium | **16 / SemiBold** | keep size; Fluent emphasis weight |
| `TitleSmall` | 14 / Medium | **14 / SemiBold** | = Fluent Body Strong |
| `BodyLarge` | 16 / Medium | **18 / Regular** | = Fluent Body Large |
| `BodyMedium` | 14 / Medium | **14 / Regular** | = Fluent Body |
| `BodySmall` | 12 / Medium | **12 / Regular** | Fluent Caption size in a body role |
| `LabelLarge` | 14 / Medium | **14 / SemiBold** | = Fluent Body Strong (button labels) |
| `LabelMedium` | 12 / Medium | **12 / SemiBold** | Caption size + emphasis weight |
| `LabelSmall` | 11 / Medium | **11 / SemiBold** | keep size (below Fluent ramp) |
| `LabelExtraSmall` | 11 / Normal | **11 / Regular** | keep size |
| `CaptionLarge` | 13 / Medium | **13 / Regular** | keep size; Fluent caption weight |
| `CaptionMedium` | 12 / Medium | **12 / Regular** | = Fluent Caption |
| `CaptionSmall` | 11 / Medium | **11 / Regular** | keep size |

`TypefacePlain` and `TypefaceBrand` both alias `ContentControlThemeFontFamily`.

### 7.3 TextBlock styles

`Styles/Controls/TextBlock.xaml` defines the 19 semantic TextBlock styles.
Simple prefixes its styles (`SimpleDisplayLarge`) and aliases the unprefixed
semantic keys in `_Resources.xaml`; Fluent follows the same two-step
(`FluentDisplayLarge` + alias `DisplayLarge`) for consistency. Each style sets
only `FontFamily`/`FontSize`/`FontWeight`/`CharacterSpacing` from the slot
value keys. XCR's own ramp styles (`BodyTextBlockStyle`, …) remain untouched
and usable side-by-side.

**Note (lessons.md):** the shared layer *also* defines the `*FontFamily/…`
value keys. Simple needed load-bearing duplicate mappings in its `Fonts.xaml`
(merged after `SharedTypography.xaml`) because per-slot `<StaticResource>`
weight-family aliases resolved unreliably across dictionaries. FluentTheme's
exposure is smaller (single family, literal sizes/weights — only the FontFamily
keys are aliases), but the Typography dictionary **must be merged after
`SharedTypography.xaml` inside `BaseDictionaries.xaml`**, mirroring Simple, and
S1 case 5 validates the FontFamily alias mechanism.

## 8. Design tokens (spacing / shape / density)

Nothing Fluent-specific to build — `BaseTheme` generates `Space*`, `Radius*`,
and density defaults in code for every theme. Notable alignment, to document:

- `DefaultCornerRadius` default **4** == Fluent `ControlCornerRadius` (4px). ✅
- `Radius200` (8 at default unit) == Fluent `OverlayCornerRadius` (8px). ✅
- Fluent controls do not consume `Space*` tokens (their metrics are baked into
  XCR templates); the tokens remain available to app layouts, same as under
  any theme.

## 9. Phase 2 — Seed color → Fluent accent (reverse mapping)

Goal G5: `Colors.PrimarySeed` must recolor built-in Fluent controls.

### 9.1 Token assignment from the generated tonal palette

`SeedColorPaletteGenerator` produces an HCT tonal palette. Map tones to the
Windows accent-shade semantics (chosen so Fluent's own usage — light theme
accent fill = `Dark1`, dark theme accent fill = `Light2` — lands on tones with
correct contrast):

| Fluent token | Tonal-palette tone |
|---|---|
| `SystemAccentColor` | Primary tone 40 |
| `SystemAccentColorLight1` | tone 60 |
| `SystemAccentColorLight2` | tone 70 |
| `SystemAccentColorLight3` | tone 80 |
| `SystemAccentColorDark1` | tone 30 |
| `SystemAccentColorDark2` | tone 20 |
| `SystemAccentColorDark3` | tone 10 |

### 9.2 Why overriding `SystemAccentColor*` alone is not enough (D12)

XCR's internal accent resources (`AccentFillColorDefault`,
`AccentTextFillColorPrimary`, `TextOnAccentFillColorPrimary`,
`AccentFillColorDefaultBrush`, …) reference `SystemAccentColor*` via
`{StaticResource}` resolved eagerly **when XCR itself loads** — before
`FluentTheme` merges. A later sibling override of `SystemAccentColor` does not
retro-propagate. Phase 2 therefore overrides the **full closure**: the
`SystemAccentColor*` set *plus* every `AccentFillColor*` / `AccentTextFillColor*`
/ `TextOnAccentFillColor*` color and brush, per theme branch. Spike S4
enumerates the exact closure per platform — Uno.UI's set **does** differ from
WinUI's: S2 already found `AccentTextFillColorPrimary/Secondary/Tertiary`
absent on Skia (only `TextOnAccentFillColor*` exists there), so the closure
must be built per-platform from what actually resolves. Implementation is code-level (mechanism C) inside the dynamic colors
layer so it rebuilds on every seed change and participates in hot reload.

### 9.3 Interaction with the forward mapping

When a seed is active, `BaseTheme.UpdateSource()` already merges the seed
palette **above** `ColorPalette.xaml`, so semantic brushes follow the seed
automatically (existing behavior, guarded by `Given_SeedColorPalette`). The
accent closure must be derived from the *effective* palette (seed if present,
else Fluent defaults) so both flows agree on what "Primary" is.

## 10. Phase 3 — Lightweight-styling bridge

Semantic lightweight keys (documented in `doc/styles/*.md`) are referenced
*directly by Material/Simple templates*. Fluent templates reference **their
own** per-control resources. Bridge mechanism, per control:

1. FluentTheme defines each documented semantic key, defaulting to the Fluent
   value: `FilledButtonBackground` ← `AccentFillColorDefaultBrush`, etc.
2. FluentTheme redefines the Fluent per-control resource to consume it:
   `AccentButtonBackground` ← `{ThemeResource FilledButtonBackground}`.
3. A consumer override of `FilledButtonBackground` then reaches Fluent-styled
   controls, matching Material/Simple behavior.

Step 2 has the same eager-resolution question as §9.2 (XCR templates resolved
their `{ThemeResource AccentButtonBackground}` lookups against XCR's own
definition — but `ThemeResource` lookups re-resolve per element scope, unlike
`StaticResource`, so a later app-scope redefinition *should* win). **Spike S4
validates this** before Phase 3 starts.

Illustrative Button mapping (full key set in `doc/styles/Button.md`):

| Semantic key | Fluent resource consumed / re-pointed |
|---|---|
| `FilledButtonBackground` (+`PointerOver/Pressed/Disabled`) | `AccentButtonBackground` (+ state variants) |
| `FilledButtonForeground` (+states) | `AccentButtonForeground` (+states) |
| `FilledButtonBorderBrush` (+states) | `AccentButtonBorderBrush` (+states) |
| `OutlinedButtonBackground/Foreground/BorderBrush` (+states) | `ButtonBackground/ButtonForeground/ButtonBorderBrush` (+states) |
| `TextButtonForeground` (+states) | foreground setters of `FluentTextButtonStyle` |
| `*ButtonIconForeground*`, `*StateLayer*`, `*Elevation*` | **no Fluent equivalent — not bridged** (documented) |

Rollout order (D13): Button → TextBox → CheckBox → RadioButton →
ToggleSwitch → Slider. Each control lands with its own resolution runtime
tests before the next starts.

## 11. Packaging & repo mechanics

### 11.1 New files (Phase 1)

Everything under `src/library/Uno.Fluent.WinUI/` per §3.2. `FluentConstants`:

```csharp
namespace Uno.Fluent;

internal static class FluentConstants
{
    public static readonly string PackageName = "Uno.Fluent.WinUI";

    public static class ResourcePaths
    {
        public static readonly string ColorPalette = $"ms-appx:///{PackageName}/Styles/Application/ColorPalette.xaml";
        public static readonly string MergedPages = $"ms-appx:///{PackageName}/Generated/mergedpages.xaml";
    }
}
```

`fluent-common.props` mirrors `simple-common.props` with:
- `XamlMergeInput Include="Styles\Controls\**\*.xaml"` + `Styles\Application\**\*.xaml`
- `XamlMergeInput Remove="Styles\Application\ColorPalette.xaml"` (loaded standalone)
- **no** font `PackageReference` (D11)
- `<Using Include="Uno.Fluent" />` for implicit-usings consumers

### 11.2 Existing files to touch

| File | Change |
|---|---|
| `src/library/Uno.Themes/themes-common.props` | add `<InternalsVisibleTo Include="Uno.Fluent.WinUI" />` (needed for `ResourceDictionaryExtensions.SafeMerge`) |
| `Uno.Themes.sln` | add project |
| `Uno.Themes-packages.slnf` | add project |
| CI: `build/*.yml` stages that enumerate packable projects / NuGet artifacts | add `Uno.Fluent.WinUI` wherever `Uno.Simple.WinUI` appears (audit at implementation time) |
| `src/samples/SamplesApp.Shared` (Phase 4) | Fluent showcase page(s) |

### 11.3 Public API surface (Phase 1)

- Type `Uno.Fluent.FluentTheme` (ctors matching `SimpleTheme`'s: default +
  `(ResourceDictionary colorOverride, ResourceDictionary fontOverride)`), XML-doc'd.
- Resource keys: the semantic aliases (§5), `FluentTextButtonStyle`,
  `FluentIconButtonStyle`, `FluentDisplayLarge`…`FluentCaptionSmall`,
  and the palette/typography values. **All resource keys are public API**
  (repo rule); additive-only from day one.

## 12. Testing strategy

Host: **SimpleSampleApp** runtime tests (D14) — CI-parity host with
`XamlControlsResources` at app scope. Pattern: per-test
`new FluentTheme()` merged into a container `Grid` (as
`Given_SemanticStyles.CreateThemedContainer` does with `SimpleTheme`).

| Test class | Guards | Phase |
|---|---|---|
| `Given_FluentAliasResolution` | the platform *mechanism*: StaticResource alias to XCR keys across sibling merged dictionaries, alias-of-alias, `BasedOn` XCR style, theme-branch color alias, FontFamily alias, presence of the ✅ core XCR keys | Spike S1 (committed as a permanent mechanism guard) |
| `Given_FluentSemanticStyles` | every §5 alias resolves; accent/standard visual split (FilledButton bg == AccentButton bg ≠ OutlinedButton bg); Light+Dark | 1 |
| `Given_FluentColorPalette` | every §6 role resolves to the mapped token's value; Light+Dark; semantic brush spot-checks (`PrimaryBrush`, `OnSurfaceBrush`, `OutlineBrush`) | 1 |
| `Given_FluentTypography` | 19 slots: size/weight/family values per §7.2; TextBlock styles apply them | 1 |
| `Given_FluentSeedAccent` | seed set → `SystemAccentColor*`+closure follow tonal palette; seed cleared → defaults restored | 2 |
| `Given_FluentLightweightStyling` | per bridged control: override semantic key → Fluent-styled control reflects it (extends per D13 rollout) | 3 |
| `Given_ColorOverridePrecedence` | **extend, don't duplicate** (repo rule) with FluentTheme cases: base palette < seed < consumer override | 1–2 |

Every phase runs green through the headless desktop script
(`build/scripts/linux-skia-desktop-runtime-tests.sh` equivalent locally, per
the `/uno-themes-runtime-tests` skill) before merge.

## 13. Documentation impact

| Doc | Change |
|---|---|
| `doc/fluent-getting-started.md` | extend: today it only covers enabling XCR; add "FluentTheme" section (install, App.xaml, ordering requirement XCR→FluentTheme, state-model note §6.4, N3 materials note) |
| `doc/semantic-styles.md` | add **Fluent** column to every mapping table (§5 values); update typography section |
| `doc/themes-overview.md` | add Fluent to the theme roster |
| `doc/seed-colors.md` | Phase 2: FluentTheme accent-cascade section |
| `doc/lightweight-styling.md` + `doc/styles/*.md` | Phase 3: per-control Fluent columns as controls are bridged |
| `.github/pull_request_template.md` checklist | follows automatically (controls-styles pages listed there don't gain a Fluent page until Phase 3+) |

## 14. Spikes

All spike outcomes are recorded in `specs/05-fluent-theme/spike-results.md`
as they land; a spike is **done** only when its exit criterion is met and the
result is written down.

### 14.1 — S1: cross-dictionary StaticResource alias resolution *(the go/no-go spike — ✅ DONE 2026-07-14, Skia desktop; verdict: GO — styles via mechanism A (direct aliases only, D16), palette via mechanism C (D6). Details in `spike-results.md`.)*

- **Question:** In the real topology (XCR merged at app scope, adapter
  dictionary merged later/scoped lower), do the following resolve correctly on
  Uno (Skia desktop, CI host) — knowing `specs/lessons.md` documents eager
  parse-time resolution as a real failure mode?
  1. `<StaticResource x:Key="A" ResourceKey="AccentButtonStyle"/>` (style alias to XCR key)
  2. alias-of-alias (`B` → `A` → XCR)
  3. `BasedOn="{StaticResource DefaultButtonStyle}"` bridge style
  4. per-theme-branch `<StaticResource>` **color** alias (`ResourceKey="TextFillColorPrimary"`) resolving to the *correct branch's* value under Light and Dark
  5. FontFamily alias (`ResourceKey="ContentControlThemeFontFamily"`)
  6. presence probe of the ✅ core XCR style keys (S3-core)
- **Vehicle:** `src/samples/SimpleSampleApp/RuntimeTests/Given_FluentAliasResolution.cs`
  plus a compiled spike dictionary
  `src/samples/SimpleSampleApp/RuntimeTests/FluentSpike/FluentAliasSpikeDictionary.xaml`
  loaded via `ms-appx` `Source` URI — same load path `FluentTheme` will use.
  Run headless via the desktop runtime-tests flow.
- **Exit criteria:** cases 1–3, 5, 6 green → **mechanism A viable for styles**.
  Case 4 green → mechanism A viable for the palette; case 4 red/flaky →
  **mechanism C for the palette** (D6), styles decision unchanged unless 1–3
  also fail. Any failure of 1–3 → M-CODE for style aliases.
- **Residual risk after S1:** validated on Skia desktop only. Windows (real
  WinUI — where StaticResource aliasing across app-scope dictionaries is
  well-specified) and WASM validation deferred to Phase 1 CI/manual pass;
  tracked, not blocking.

### 14.2 — S2: capture concrete Fluent token values *(⚙️ PARTIAL — Dark branch + accent set captured on Skia 2026-07-14; Light branch and Windows/WASM pending — see `spike-results.md`)*

- **Question:** exact per-platform hex values for every token in §6.3, both
  theme branches (fills the *(S2)* cells; sanity-checks the mapping choices).
- **Vehicle:** discovery test in `Given_FluentAliasResolution` that dumps the
  token list with resolved values (report written to a temp file / test log)
  — run on Skia desktop now; Windows + WASM during Phase 1.
- **Exit criterion:** §6.3 informative columns replaced with captured values in
  `spike-results.md`; discrepancies between platforms called out.

### 14.3 — S3: enumerate available `Default*Style` keys *(⚙️ PARTIAL — Skia table captured 2026-07-14 and folded into §5.2; Windows/WASM probe + M-CODE verification pending)*

- **Question:** which 🔍 keys of §5.2 exist in Uno.UI's XCR (and which only on
  WinUI/Windows)?
- **Vehicle:** same discovery test — probe list covering every 🔍 key + the
  XCR text ramp styles.
- **Exit criterion:** §5.2 table updated: every 🔍 becomes ✅ (present),
  **M-CODE** (absent, type-keyed lookup works), or **GAP** (absent, no
  workable fallback — requires explicit sign-off).

### 14.4 — S4: accent/lightweight override cascade (before Phase 2/3)

- **Questions:** (a) which XCR resources hold copies of `SystemAccentColor*`
  resolved at XCR load (the D12 closure)? (b) does redefining a per-control
  resource (`AccentButtonBackground`) in a later app-scope dictionary win for
  `{ThemeResource}` lookups made inside XCR templates, per platform?
- **Vehicle:** runtime tests: merge overrides, instantiate controls, assert
  rendered brush colors.
- **Exit criterion:** documented closure list + a yes/no per platform for (b);
  Phase 3 design confirmed or revised.

## 15. Phasing & acceptance criteria

| Phase | Scope | Done when |
|---|---|---|
| **0 — Spike S1/S2/S3** *(now)* | mechanism validation + token/key capture | S1 exit criteria met; results in `spike-results.md`; mechanism A/C decision recorded |
| **1 — Core library** | project scaffold, `_Resources.xaml`, `ColorPalette.xaml`, `Typography.xaml`, TextBlock + bridge styles, packaging (§11), tests (§12 phase-1 rows), docs (§13 phase-1 rows) | G1–G4 hold; all phase-1 runtime tests green in CI host; packages build via `Uno.Themes-packages.slnf` |
| **2 — Seed → accent** | S4(a), §9 closure override | G5 holds; `Given_FluentSeedAccent` green; `doc/seed-colors.md` updated |
| **3 — Lightweight bridge** | S4(b), §10 per-control rollout | per-control: `Given_FluentLightweightStyling` green + doc column added |
| **4 — Samples** | Fluent showcase in SamplesApp.Shared and/or `FluentSampleApp` head decision | sample renders all §5 aliases; screenshot pass vs WinUI Gallery |

## 16. Risks & mitigations

| Risk | Severity | Mitigation |
|---|---|---|
| Eager cross-dictionary `<StaticResource>` resolution unreliable on Uno (**observed before** — `specs/lessons.md`) | High — invalidates mechanism A | **Resolved by S1 (2026-07-14):** style aliases work (direct-target only — D16); per-branch color aliases confirmed broken → palette uses mechanism C (D6). Permanent guard: `Given_FluentAliasResolution` |
| A 🔍 key missing on some platform only (Uno.UI vs WinUI drift) | Medium | S3 per-platform table; M-CODE type-keyed fallback; alias set can differ per platform via `not_win`/`win` namespaces if needed |
| Uno.UI updates change Fluent values/keys between releases | Medium | tests assert against live XCR resources (not baked hexes) wherever possible; `Given_FluentAliasResolution` presence probes fail fast on Uno.UI bumps |
| Consumer merges FluentTheme *before* XCR | Medium — everything breaks | doc ordering requirement (§13); consider a code guard in `FluentTheme` ctor: probe `Application.Current.Resources["AccentButtonStyle"]` and log a structured error (graceful degradation per §8 of AGENTS — never throw from theme init) |
| Semantic state brushes ≠ Fluent state tokens (§6.4) | Low — cosmetic | documented behavior; N2 |
| WASM/iOS/Android resolution differences not covered by desktop-only spike | Medium | Phase 1 CI (`stage-build-wasm.yml`) + manual pass on mobile heads before release; risk logged in `spike-results.md` |
| Package-pipeline misses (canary updater, NuGet lists) | Low | §11.2 audit item; mirror every occurrence of `Uno.Simple.WinUI` in `build/` |
