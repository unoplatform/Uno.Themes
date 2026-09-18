# 10 — Cupertino v2: Liquid Glass era theme on the semantic system

Status: **Phase 0.5 spike run on the software path; Phase 1 in progress** (2026-09-18). D-1 / D-2 / D-3 / D-7 confirmed as
recommended; D-5 open until the GPU spike items are run on real hardware.
Branch: `dev/sb/cupertino-v2` (currently identical to `master`).

Companion documents in this folder:

- `apple-hig-reference.md` — retained digest of Apple's HIG / Liquid Glass guidance with every published number.
- `token-and-style-mapping.md` — Apple → shared semantic tokens; typography; per-control style matrix;
  legacy-key compatibility and breaking changes.
- `liquid-glass-rendering.md` — how the glass is drawn on Uno Skia (two-tier `GlassPanel`), evidence, spike.
- `reference-cupertino-avalonia.md` — cross-check values and prior art from `jsuarezruiz/Cupertino.Avalonia`.

## Context

`Uno.Cupertino.WinUI` is the one theme in this repo that never moved onto the semantic system. Today it is
16 control dictionaries (3.5 k lines) built on iOS 13-era metrics, its own `Cupertino*` colour vocabulary,
no type scale, no design tokens, no seed-colour support, no semantic aliases, no runtime tests, and a
three-dictionary App.xaml setup (`CupertinoColors` / `CupertinoFonts` / `CupertinoResources`) that
Material and Simple replaced with `MaterialTheme` / `SimpleTheme : BaseTheme`. Apple's design language has
meanwhile moved to Liquid Glass (iOS 26 / macOS 26, refined in 27): translucent, refracting control
surfaces that float above content, capsule shapes, concentric corners, a re-tuned system palette and a
bolder, left-aligned typography.

Goal: revive Cupertino as a first-class semantic theme — `CupertinoTheme : BaseTheme`, Apple's palette
mapped onto the shared colour roles, the SF text styles mapped onto the shared type scale, every semantic
style key aliased, the shared tokens consumed — and reproduce the Liquid Glass look on Skia-rendered Uno
apps (Desktop, WebAssembly, iOS, Android via `SkiaRenderer`). Native renderers are out of scope; the
WinAppSDK head must compile and run with the solid fallback.

### Verified facts driving the design

- `BaseTheme` already does everything Cupertino lacks: shared palette + theme base palette + seed palette +
  consumer override layering (`BuildColorLayer`), live brush rewriting including the `*Opacity` ladder
  (`SemanticBrushUpdater`), generated `Space*` / `Radius*` / `ControlHeight*` scales, `DefaultFontFamily`
  cascade with per-theme `FontFamilyAliasKeys`, hot reload, and the `AddThemeSpecificResources` seam.
  `SimpleTheme` is 60 lines; Cupertino's theme class will be about the same plus three settings DPs.
- The XamlMerge precedence rules in `specs/lessons.md` apply verbatim: a theme file that must override a
  shared default is removed from the `XamlMergeInput` glob and merged by `BaseDictionaries.xaml` after the
  file it overrides; anything a generated layer must override is declared inside `ThemeDictionaries`;
  consuming setters use `{ThemeResource}`; a design-system-specific alias of the root font key is dead for
  overriding purposes.
- Material's deprecated `MaterialResourcesV2` trio is standalone (Source = mergedpages + `MaterialColorsV2`
  with Source = `SharedColors.xaml`), but Material v2 styles consume the generated `Space*` / `Radius*`
  tokens in 21 files and no static token defaults exist anywhere — so a standalone legacy trio is **not** a
  safe blueprint for Cupertino (whose new styles also consume the generated tokens). The legacy
  `CupertinoResources` therefore has to *be* the theme. (Whether the Material trio still resolves tokens
  standalone is a separate question; noted in "Hand-offs".)
- Uno Skia (7.0-dev) implements composition backdrop brushes (that is how `AcrylicBrush` renders);
  `SKCanvasElement` draws on the shared frame canvas, so a `SaveLayer` with a backdrop image filter can read
  what is beneath it; `SKImageFilter.CreateDisplacementMapEffect` gives refraction without waiting for SkSL
  image filters. `canvas.Surface` is unavailable inside `RenderOverride` (recording canvas). Details and
  confidence tags in `liquid-glass-rendering.md` §2.
- `UISettings.AdvancedEffectsEnabled` is a stub on Uno; the theme must own its Reduce Transparency /
  Reduce Motion switches.
- SF Pro and SF Symbols cannot be redistributed (Apple font license). **The sample tree currently checks in
  `SamplesApp.Shared/Assets/Fonts/Cupertino/SF-Pro.ttf` (6.1 MB)** referenced by
  `CupertinoSampleApp/CupertinoFontsOverride.xaml`; Phase 1 removes it. Inter is already packaged.
- `uno.toolkit.ui` ships `CupertinoToolkitTheme` (TabBar, NavigationBar, …) on top of today's Cupertino keys —
  every existing key stays as an alias; legacy content-opacity keys keep literal values.
- Libraries target `net10.0` (+ `net10.0-ios/android/windows10.0.19041`) per `src/library/tfm-common-winui.props`;
  `AGENTS.md` still says `net9.0` — fixed in Phase 1's doc delta.

## Decisions required before Phase 1 (recommendation first)

| # | Decision | Recommendation | Alternative / hidden cost |
|---|---|---|---|
| D-1 | Replace in place vs Material-style v1/v2 split | **Replace in place, shipped in the next major**, every public key kept as an alias, plus a frozen opt-in `CupertinoResourcesV1` (today's dictionaries as `mergedpages.v1.xaml`, no tests, removed the following major) as the escape hatch | Full v1/v2 maintenance like Material. Hidden cost of the recommendation: appearance changes on restore for every Cupertino app (`CupertinoContainedButtonStyle` becomes a glass capsule, padding / radius / font-size values change) — the frozen v1 is what makes that acceptable |
| D-2 | Default (implicit) button look | **`.plain`** — accent text, no background (Apple's default `UIButton`; also today's `CupertinoButtonStyle`). Glass is opt-in (`CupertinoGlassButtonStyle`) and used by bars; semantic `FilledButtonStyle` → `.glassProminent` | Implicit glass: contradicts the HIG's "never in the content layer" and would put a backdrop `SaveLayer` in every list item that contains a button |
| D-3 | Default typeface | **Inter** via `Uno.Fonts.Inter` (deterministic on every Skia host; license-clean); one-line opt-in to the host SF on Apple targets | `SF Pro` by name (today): renders as whatever Skia falls back to off-Apple. Hidden cost of Inter: not metric-compatible with SF; ~1 MB shipped even on iOS |
| D-4 | `Tertiary` role | **systemIndigo** as the complementary accent (M3's tertiary intent) | Teal, or gray. No hidden cost — any seed overrides it |
| D-5 | Glass package placement | **Decide after spike item 8.** Preferred: `Uno.WinUI.Graphics2DSK` referenced by `Uno.Cupertino.WinUI`, backplate compiled out on the Windows TFM | Separate `Uno.Cupertino.WinUI.Skia` package (Toolkit's model). Hidden cost of the preferred option: SkiaSharp natives dragged into iOS / Android native-renderer consumers and the Windows TFM |
| D-6 | Idiom | **iOS / iPadOS touch metrics** everywhere (44 pt targets, 17 pt body) | `CupertinoTheme.Idiom = Pointer` for macOS (13 pt body, 22–28 pt controls, hover-first) — follow-up |
| D-7 | Lottie | **Drop** `UnoFeatures=Lottie`; activity indicator becomes an 8-spoke XAML storyboard. **Breaking**: `CupertinoDeterminateAnimation_Uno` / `CupertinoIndeterminateAnimation_Uno` removed, transitive `Uno.WinUI.Lottie` gone (documented consumer action) | Keep the Lottie ring and the dependency |
| ~~D-8~~ | Increased Contrast palette | **Cut.** WinUI `HighContrast` is the OS mode and is never selected on Skia; the 12 colours would be unreachable | `CupertinoTheme.IncreaseContrast` bool if ever requested |

## Plan

Phase gates are hard: a phase is complete only when its runtime tests pass under the CI script
(`build/scripts/linux-skia-desktop-runtime-tests.sh` against `CupertinoSampleApp`), the Material and Simple
runtime suites are **unchanged** (same pass / skip set, not a hard-coded count), XAML Styler (two passes) and
`dotnet format` verify clean, and each phase's PR carries its own `doc/` delta (the PR template requires it;
docs are not deferred to the end).

### Phase 0 — Spec and decisions

- [x] Repo orientation: `BaseTheme` layering, Simple as the template theme, current Cupertino surface,
  XamlMerge lessons, sample and test hosting, CI matrix.
- [x] Research: Apple HIG / Liquid Glass digest; Cupertino.Avalonia reference; Uno Skia rendering survey.
- [x] Write the four companion documents and this plan.
- [x] Skeptic review; findings folded in (see "Review").
- [x] D-1, D-2, D-3, D-7 confirmed by the maintainer on 2026-09-18, each as recommended (replace in place +
  frozen V1; plain implicit button; Inter via `Uno.Fonts.Inter`, SF-Pro.ttf removed; Lottie dropped). D-4
  applied as recommended (indigo). D-5 waits on the GPU spike items; D-6 is a follow-up.

### Phase 0.5 — Liquid Glass spike (size S, throwaway)

Prototype in `CupertinoSampleApp` (not the library) answering `liquid-glass-rendering.md` §7, items 1–9:
backdrop reads on each Skia target, stale halo under damage regions, `Opacity`, backplate inside
`Popup` / `Flyout` / `ContentDialog`, a glass bar over a scrolling `ListView`, WASM WebGL2 frame time for
five surfaces, a capture method for the pixel test, package / TFM restore cost (D-5), and the system-font
family name on Apple hosts (D-3 docs).

- [x] Software-path findings recorded in `liquid-glass-rendering.md` §8 (items 1, 3, 4, 7, 8 answered; 2 and 5
  answered for correctness). Two design corrections came out of it: clip **before** the backdrop
  `SaveLayer`, and the backplate has to apply `Opacity` itself. The pixel test is no longer GPU-gated.
- [ ] GPU items — 1 / 2 / 5 / 6 on a GPU desktop and a WebGL2 browser, 9 on an Apple host. **Blocked on
  hardware:** the dev box has no GPU (ASPEED BMC) and agents run in a disconnected RDP session, so Uno
  always picks the software renderer there and the frame clock is throttled to ~4 fps regardless of
  content. Needs a manual `CupertinoSampleApp --glass-spike` run on real hardware.
- [ ] D-5 taken (evidence in §8 item 8).
- [ ] Gate: go / no-go on the Liquid tier — **provisional go** on correctness; cost unmeasured. A no-go
  means Phases 1, 3 and 4 still ship (Solid tier everywhere) and Phase 2 is replaced by the
  composition-brush follow-up.
- [ ] Delete `CupertinoSampleApp/Spike/` and the `--glass-spike` hook in `App.xaml.cs` once the GPU items
  are recorded.

### Phase 1 — Foundation: `CupertinoTheme : BaseTheme` (size L, purely additive)

No public key is renamed in this phase. Semantic aliases point at the **existing** legacy style names;
renames arrive with the new styles in Phase 3.

Library (`src/library/Uno.Cupertino/`):

- [ ] `CupertinoTheme.cs` (mirrors `SimpleTheme`): ctor `(colorOverride, fontOverride)`, `DefaultStylesSource
  = CupertinoConstants.ResourcePaths.MergedPages`, `DefaultPrimarySeed => null`, `FontFamilyAliasKeys`
  (`CupertinoFontFamily` + every per-control alias), DPs `ReduceTransparency`, `ReduceMotion`,
  `GlassRenderingMode` written as resources in `AddThemeSpecificResources` (construction-time, like
  `DefaultCornerRadius`; PCCs never throw). `AddThemeSpecificResources` also rewrites the `Cupertino*Brush`
  colours from the resolved colour layers (`SemanticBrushUpdater.Apply` generalised to a key list).
- [ ] `Styles/Application/BaseDictionaries.xaml` (Converters, SharedTypography, then Cupertino
  `Typography.xaml`, `Fonts.xaml`, `Thickness.xaml`, `CupertinoBrushes.xaml`) with the matching
  `XamlMergeInput Remove` entries in `cupertino-common.props` — remove + Source as one indivisible edit.
- [ ] `Styles/Application/ColorPalette.xaml`: opaque shared-role overrides per `token-and-style-mapping.md`
  §1 (shared opacity ladder untouched), every legacy `Cupertino*` colour key, new Mint / Cyan / Brown /
  glass-tint / switch keys. `CupertinoBrushes.xaml`: the legacy `Cupertino*Brush` set.
- [ ] `Typography.xaml` (19 slots, §3); `Fonts.xaml` (`DefaultFontFamily` = Inter in Light / Default /
  HighContrast); `Thickness.xaml` (§4 tokens, all static); `AnimationConstants.xaml` (§5 tokens **inside
  `ThemeDictionaries`**, legacy keys literal); `StateConstants.xaml` (legacy content-opacity keys, literal
  values).
- [ ] `Styles/Controls/_Resources.xaml`: implicit styles + every semantic alias in Simple's list (pointing at
  the existing styles) + typography aliases.
- [ ] Legacy shim per §8: `CupertinoResources : CupertinoTheme` (`[Obsolete]`, reads the static override
  URIs, `WithImplicitStyles` no-op); `CupertinoColors` / `CupertinoFonts` record the URI only; a
  `CupertinoFontFamily` override is translated to `DefaultFontFamily`. `CupertinoConstants` gains
  `ResourcePaths`, `SemanticStyleKeys`, `PaletteKeys`, `BrushColorKeys`.
- [ ] Frozen `CupertinoResourcesV1` over `Generated/mergedpages.v1.xaml` (D-1): move today's dictionaries
  under `Styles/Controls/v1/` + `Styles/Application/v1/`, second `XamlMergeInput` mapping, no tests.
- [ ] Add `Uno.Fonts.Inter`; keep `Lottie` until Phase 3 replaces the ring (D-7).
- [ ] Inventory every Cupertino key `uno.toolkit.ui`'s `CupertinoToolkitTheme` reads (grep its Styles tree);
  record the list here and assert each key resolves in `Given_CupertinoPalette`.
- [ ] Doc comments on every new public member.

Samples, tests, CI, docs:

- [ ] `CupertinoSampleApp/App.xaml` → `<CupertinoTheme xmlns="using:Uno.Cupertino" />`; delete
  `CupertinoFontsOverride.xaml` **and `SamplesApp.Shared/Assets/Fonts/Cupertino/SF-Pro.ttf`** (license).
- [ ] `RuntimeTests/Given_CupertinoTheme.cs`: every key in `CupertinoConstants.SemanticStyleKeys` resolves to
  a `Style`; shared roles resolve to the Apple values under Light and Dark (`ThemeDictionaries.TryGetValue`);
  `Colors.PrimarySeed` rewrites `PrimaryBrush` **and** `CupertinoBlueBrush` live; `DefaultFontFamily`
  cascades to `CupertinoFontFamily`; `Space*` / `Radius*` generate; `ReduceMotion=true` at construction →
  a realized control's storyboard `Duration` is zero (rendered path, not `TryGetValue`).
- [ ] `Given_CupertinoPalette.cs`: Light / Dark key-set and type parity over `PaletteKeys`; legacy keys
  present; Toolkit inventory keys present.
- [ ] `Given_CupertinoLegacyResources.cs`: the obsolete trio in the documented order, with a colour
  `OverrideSource` and a font `OverrideSource` redefining `CupertinoFontFamily`, resolves the same brushes
  and font as the equivalent `CupertinoTheme`; `Application.GetTheme()` finds it.
- [ ] `build/stage-runtimetests-desktop.yml`: add the `Cupertino` matrix row.
- [ ] Docs delta: `doc/cupertino-getting-started.md` rewritten around `CupertinoTheme` (colours / seed,
  `DefaultFontFamily`, migration from the trio incl. the `CupertinoFontFamily` note; fix the existing
  `<Color x:Key="CupertinoBlueBrush">` mistake), `doc/seed-colors.md` and `doc/design-tokens.md` drop the
  Cupertino caveats, `AGENTS.md` TFM line and runtime-test hosting note.
- [ ] Gate: Cupertino suite green under the CI script; Material and Simple suites unchanged; three heads
  build for `net10.0-desktop`; `ThemesSampleApp` hosting smoke still cycles Cupertino.

### Phase 2 — `GlassPanel` primitive (size M, only if Phase 0.5 is a go)

- [ ] `GlassPanel : Control` per `liquid-glass-rendering.md` §3: `GlassMaterial` / `GlassRenderingMode`
  enums with explicit underlying type, `TintColor : Color`, clamped advanced parameters, tier chosen on
  `Loaded` from the theme resources; preset table in `GlassPanel.Presets` (constants, AGENTS.md §9).
- [ ] `SkiaGlassBackplate : SKCanvasElement` (partial, excluded on the Windows TFM): displacement-map
  refraction, downsampled clamp blur, saturation + adaptive tint colour filter, rim + inner shadow inside the
  layer; filter chain cached per quantised `(size, scale, preset)` with an LRU cap of 32.
- [ ] Solid tier; Reduce Transparency routing; `CompositionTarget.Rendering` subscription only while
  animating, dropped in `Unloaded`, no double-subscribe.
- [ ] Press interaction (scale 1.04 + light boost over `CupertinoPressDuration`, spring-back over
  `CupertinoMorphDuration`), skipped when `CupertinoReduceMotion`.
- [ ] Tests (`Given_GlassPanel.cs`) per `liquid-glass-rendering.md` §6: tier vs capabilities; Reduce
  Transparency → Solid; clamping; unsubscribe-on-unload leak guard; bounded cache; GPU-gated pixel test
  using the capture method the spike selected.
- [ ] `SamplesApp.Shared/Content/Styles/LiquidGlassSamplePage.xaml`: every material over a photo wallpaper,
  light / dark, mode switcher.
- [ ] Docs delta: new `doc/cupertino-liquid-glass.md` (API, materials, tiers, performance rules,
  accessibility), `doc/toc.yml`.
- [ ] Gate: tests green on desktop CI; **hosting smoke green with the backplate loaded in a guest ALC**;
  screenshots from Desktop GPU, WASM WebGL2 and Android attached to the PR.

### Phase 3 — Core controls (size L)

Each control: new dictionary under `Styles/Controls/` following Simple's lightweight-styling pattern
(semantic keys in `ThemeDictionaries`, `{ThemeResource}` setters, `Cupertino*` variants), legacy style keys
re-pointed as aliases (the renames happen here), sample page's `CupertinoTemplate` updated, runtime-test rows
added to `Given_CupertinoControls.cs` (rendered Background / Foreground / size assertions; a lightweight
override flows through), `doc/styles/cupertino/<Control>.md` snapshot and `doc/cupertino-controls-styles.md`
row updated in the same PR.

- [ ] Button family: Plain (implicit), Prominent, Tinted, Gray, Glass (opt-in), Destructive, Small / Large,
  icon variants, FAB aliases.
- [ ] ToggleButton (text / icon), HyperlinkButton (primary / secondary).
- [ ] TextBox (default / filled / plain / search), PasswordBox, NumberBox (stepper capsule), AutoSuggestBox.
- [ ] CheckBox, RadioButton, ToggleSwitch (51 × 31 / 27), Slider (capsule track, 28 thumb).
- [ ] ProgressBar, ProgressRing (8-spoke storyboard; remove the `Lottie` feature, the two `*Animation_Uno`
  keys and the `#if !WinUI_Desktop` special case) (D-7).
- [ ] TextBlock styles for every slot + Apple-named aliases.
- [ ] ComboBox (pop-up button + glass popup) and ComboBoxItem.
- [ ] Gate: `Given_CupertinoControls` green; formatters clean; docs rows present for every style in the PR.

### Phase 4 — Containers, navigation, glass interactivity (size L)

- [ ] ListView / ListViewItem (inset grouped, hairline separators, group radius token; no glass).
- [ ] ContentDialog (alert on Thick glass over a dimming layer; hairline button rows).
- [ ] Flyout / MenuFlyout family / ToolTip (Thick glass popovers, 44 pt items, pop-open animation).
- [ ] NavigationView / NavigationViewItem (glass sidebar, top bar), CommandBar / AppBarButton (one shared
  glass slab per group, prominent primary), PipsPager, RatingControl, Expander.
- [ ] CalendarView, CalendarDatePicker, DatePicker (+ flyout presenter on a glass backplate).
- [ ] Glass interactivity polish: switch and slider knobs become `Thin` glass while dragging.
- [ ] Gate: `Given_CupertinoContainers` green (dialog / flyout resolve and render on desktop; NavigationView
  pane resources resolve Light / Dark); sample pages updated; `doc/semantic-styles.md` gains its Cupertino
  column; WASM smoke of the sample app under `ThemesSampleApp`.

### Phase 5 — Wrap-up (size S)

- [ ] `doc/lightweight-styling.md` cross-links; `doc/cupertino-controls-styles.md` regenerated in full.
- [ ] `specs/lessons.md`: corrections received during the work.
- [ ] `/review-panel` on the final diff; findings triaged here under "Review".
- [ ] Follow-up issues: uno.toolkit.ui `CupertinoToolkitTheme` → derive from `CupertinoTheme`, adopt
  `GlassPanel` for TabBar (floating bottom / segmented) and NavigationBar; macOS `Pointer` idiom (D-6);
  `Uno.Cupertino.WinUI.Markup` package; SkSL single-pass glass when SkiaSharp `ToImageFilter` ships;
  composition-brush glass tier if a non-`SKCanvas` backend lands; chromatic dispersion; sibling morphing.

## Out of scope (this spec)

- Native renderers (UIKit / Android views / WinAppSDK composition) — the Windows head gets the solid tier.
- macOS pointer idiom metrics — follow-up (D-6).
- Toolkit controls (TabBar, NavigationBar, Chip, Card, Drawer) — uno.toolkit.ui follow-up.
- SF Symbols icon set — not redistributable; samples use Uno's existing icon assets.
- Dynamic Type / text scale factor — a `BaseTheme`-level feature request, not Cupertino-specific.
- Device-motion (gyro) specular response; chromatic dispersion; live switching of glass settings on
  already-loaded panels.

## Verification summary

| What | How | Where |
|---|---|---|
| Semantic mapping | Resolution over `CupertinoConstants.SemanticStyleKeys`; rendered Background / Foreground parity like `Given_SemanticStyles` | `CupertinoSampleApp/RuntimeTests/Given_CupertinoTheme.cs`, `Given_CupertinoControls.cs` |
| Palette integrity | Light / Dark key-set + type parity over `PaletteKeys`; Apple values; legacy and Toolkit keys present | `Given_CupertinoPalette.cs` |
| Legacy App.xaml | Obsolete trio, documented order, colour + font overrides → identical brushes / font; `GetTheme()` finds it | `Given_CupertinoLegacyResources.cs` |
| Seed / font / tokens / motion | Inherited `BaseTheme` behaviour on `CupertinoTheme`; `Cupertino*Brush` follow a seed; `ReduceMotion` asserted on a realized storyboard | `Given_CupertinoTheme.cs` |
| Glass | Tier selection, clamping, unsubscribe leak guard, bounded cache, GPU-gated pixel test | `Given_GlassPanel.cs` |
| CI | `build/stage-runtimetests-desktop.yml` matrix row `Cupertino`; hosting smoke (Phase 1 and Phase 2 gates) | Azure pipeline |
| Visual | Screenshots per phase from Desktop GPU, WASM WebGL2, Android against iOS 26 screenshots and the Figma 27 kit | PR description |
| Formatting | `dotnet xstyler --passive` (two passes) and `dotnet format whitespace --verify-no-changes` | Code Style stage |

## Hand-offs noticed while specifying (not in this spec's scope)

- `doc/cupertino-getting-started.md` lines 147 / 156 declare `<Color x:Key="CupertinoBlueBrush">` — a
  `Color` under a `Brush` key; fixed as part of the Phase 1 doc rewrite.
- Material's deprecated `MaterialResourcesV2` / `MaterialColorsV2` trio merges no generated token layer while
  Material v2 styles read `Space*` / `Radius*` / `ControlHeight*` in 21 files; whether that standalone path
  still renders is unverified. Worth a Material runtime test or a doc note; separate issue.
- `src/samples/SamplesApp.Shared/Assets/Fonts/Cupertino/SF-Pro.ttf` is an Apple-licensed font in a public
  repository, independent of this work; Phase 1 deletes it, but it should go even if this spec stalls.

- `ThemesSampleApp --smoke` fails on `master` on Win32 with Uno 7.0.0-dev.701: the Material guest ALC is not
  reclaimed and `Application.CleanupNonDefaultAlcCaches` throws `TargetParameterCountException` (reflection
  signature drift in `GuestHosting`). CI runs the smoke on X11, so this may be Win32-only; separate issue.

- A colour override passed to a theme **constructor** (`new SimpleTheme(colorOverride)`, and the Material
  equivalent) is `SafeMerge`d into the theme's base palette. `SemanticBrushUpdater` resolves each layer from
  its own `ThemeDictionaries` and never descends into a layer's merged dictionaries, so the `*Color` keys
  follow such an override while the `*Brush` instances do not. Found while writing the Cupertino legacy
  shim; unverified for Simple / Material and therefore not fixed here — it needs its own red/fix/green in
  the Simple host. The `ColorOverrideSource` / `Colors.OverrideDictionary` path is unaffected.

## Review

### Skeptic review — 2026-09-18 (pre-approval)

Twenty findings; disposition:

| # | Finding | Disposition |
|---|---|---|
| 1 | Implicit glass button contradicts the HIG and the per-item glass rule | **Accepted.** D-2 flipped to plain; glass opt-in and in bars |
| 2 | `ReduceMotion` seam dead (top-level tokens; `Duration` snapshots) | **Accepted.** Motion tokens moved into `ThemeDictionaries`; `ReduceMotion` documented as construction-time; test asserts a realized storyboard |
| 3 | Retuned shared opacity ladder + alpha roles break every semantic-brush consumer | **Accepted.** Shared ladder untouched; container / outline / `OnSurfaceVariant` roles pre-composited to opaque; Apple dimming as control-level keys; seed caveat added |
| 4 | Forwarding shim breaks five ways | **Accepted with a different fix than proposed.** The Material standalone precedent is not viable (no static token layer, see facts), so `CupertinoResources : CupertinoTheme` *is* the theme; `CupertinoFontFamily` override translated; brushes rewritten on rebuild; implicit-styles behaviour change documented; `GetTheme()` and double-parse resolved by construction |
| 5 | Composition "Frosted" tier unreachable; CPU normal-map path dead | **Accepted.** Two tiers; both paths deleted; composition tier listed as follow-up for a non-Skia backend |
| 6 | Spike sequenced after the decisions it informs; four questions missing | **Accepted.** Spike is Phase 0.5, gates D-5; popup, scrolling list, freeze mechanism (dropped), capture method added |
| 7 | D-7 removes public keys and a transitive package silently | **Accepted.** Listed as breaking with consumer action |
| 8 | D-1 is a breaking visual change with no version or escape hatch | **Accepted.** Next major; frozen opt-in `CupertinoResourcesV1`; Toolkit key inventory in Phase 1 |
| 9 | Aliasing legacy content opacities to the overlay ladder | **Accepted.** Literal values kept |
| 10 | Tests cannot enumerate XAML dictionaries | **Accepted.** Key lists in `CupertinoConstants` |
| 11 | `OnSecondaryContainer` = accent breaks the contrast contract | **Accepted.** Role = label; styles set accent foreground |
| 12 | `GlassPanel` robustness gaps | **Accepted.** `TintColor : Color`, clamps, quantised LRU cache, unsubscribe rules, `IsBackdropFrozen` dropped, settings read from resources at load |
| 13 | D-8 targets the wrong mechanism | **Accepted.** Cut |
| 14 | Density-dependent row height contradicts two lessons | **Accepted.** Static 44 |
| 15 | D-5 hidden costs; hosting smoke missing from the glass gate | **Accepted.** D-5 after spike; hosting smoke in the Phase 2 gate |
| 16 | Samples redistribute SF Pro | **Accepted.** Deleted in Phase 1; surfaced as a hand-off |
| 17 | Phase 1 renames early; size understated; docs deferred; hard-coded counts | **Accepted.** Phase 1 additive-only and size L; docs per phase; gates on "unchanged" |
| 18 | YAGNI cuts | **Accepted.** Gray aliases, line-height tokens, spring ease, dispersion, auto-dimming cut |
| 19 | D-3 hidden cost | **Accepted as stated cost;** decision unchanged |
| 20 | Settings PCCs trigger a full rebuild | **Noted.** Construction-time settings make this a one-off cost |

_Per-phase results, deviations and review-panel findings to be appended below as work lands._

### Phase 1, slice 1 — 2026-09-18: `CupertinoTheme` on `BaseTheme` (additive)

Landed: `CupertinoTheme.cs` (mirrors `SimpleTheme`), `Styles/Application/BaseDictionaries.xaml`
(Converters → SharedTypography → Cupertino `Fonts.xaml`), the 33 shared colour roles of
`token-and-style-mapping.md` §1 added to both blocks of `ColorPalette.xaml`, and
`CupertinoSampleApp/RuntimeTests/Given_CupertinoTheme.cs` — the first runtime tests this head has hosted.
Result: **16 / 16 passed** on `net10.0-desktop` Release (palette values per appearance, `PrimaryBrush`
rewritten from the Apple palette, a seed repaints the same brush instance, `Space*` / `Radius*` generate,
legacy style keys resolve through the theme). XAML Styler and `dotnet format` verify clean. No existing key
changed value; the legacy trio in the sample's `App.xaml` is untouched and still boots. Simple and Material
were not re-run: neither references `Uno.Cupertino`.

Deviations from the Phase 1 list, each deliberate:

- **`ReduceTransparency` / `ReduceMotion` / `GlassRenderingMode` DPs deferred** to the phase that gives them
  a consumer (`GlassPanel` in Phase 2, the new storyboards in Phase 3). Nothing in the current templates
  reads them, so in Phase 1 they would be public API with no effect and no way to test the rendered path
  the plan demands.
- **`FontFamilyAliasKeys` lists `CupertinoFontFamily` only.** `CupertinoHyperlinkButtonFontFamily` and
  `CupertinoRadioButtonFontFamily` are top-level literals (`SF Pro`), which no generated layer can shadow
  (lesson: "a generated layer is always a merged dictionary"). They move into `ThemeDictionaries` when
  those two controls are restyled in Phase 3.
- **No font-cascade test from a scoped container** — `specs/lessons.md` records that alias cascades
  resolve against the application scope; that test arrives with the sample's `App.xaml` switch.

Still open in Phase 1: `Thickness.xaml` and motion tokens (unblocked, but nothing reads them before Phase 3); Toolkit
key inventory; CI matrix row; docs delta.

### Phase 1, slice 2 — 2026-09-18: type scale and semantic style aliases (additive)

Landed: `Styles/Application/Typography.xaml` (19 slots per `token-and-style-mapping.md` §3, tracking 0),
removed from the `XamlMergeInput` glob **and** merged by `BaseDictionaries.xaml` after
`SharedTypography.xaml` in the same edit; 15 semantic style aliases in `_Resources.xaml` pointing at the
existing Cupertino styles. Result: **38 / 38 passed**; the type-scale rows discriminate (the shared default
for `DisplayLargeFontSize` is 57, the theme resolves 40). Formatters clean, no warnings from the changed
files.

Deviations:

- **No implicit styles yet.** `_Resources.xaml` is part of `mergedpages.xaml`, which the legacy
  `CupertinoResources` also loads, so implicit styles declared there would switch on for every legacy
  consumer that never set `WithImplicitStyles` — the behaviour change §8 attributes to D-1. Keyed aliases
  are additive and safe; implicit styles land with the legacy shim.
- **`ProgressRingStyle` not aliased:** `CupertinoProgressRingStyle` is compiled out on the Windows TFM, so
  the alias would dangle there. It arrives with the storyboard ring (D-7, Phase 3).
- **No `CupertinoConstants.SemanticStyleKeys` list:** nothing in the library consumes it and the sample
  head cannot see `internal` members, so the tests carry the keys as `DataRow`s. Add the list when a
  library consumer appears.
- Semantic keys with no Cupertino style today (tonal / outlined / icon buttons, toggle buttons, filled
  text fields, list, dialog, navigation, menus, FABs, pips, rating) and the TextBlock slot aliases arrive
  with their controls in Phases 3–4.

### Phase 1, slice 3 — 2026-09-18: Inter, sample head on `CupertinoTheme`, SF Pro removed (D-3)

Landed: `Uno.Fonts.Inter` referenced from `cupertino-common.props` (version already pinned in
`src/library/Directory.Packages.props`); `Fonts.xaml` root = Inter in all three blocks; the two dead
`SF Pro` literals (`CupertinoHyperlinkButtonFontFamily`, `CupertinoRadioButtonFontFamily` — declared, read by
no style) now alias `CupertinoFontFamily`; `CupertinoSampleApp/App.xaml` merges `<CupertinoTheme />`;
`CupertinoFontsOverride.xaml` and the Apple-licensed `SamplesApp.Shared/Assets/Fonts/Cupertino/SF-Pro.ttf`
deleted. Result: **40 / 40 passed**, including the application-scope cascade (`GetTheme()` is a
`CupertinoTheme`; root, `CupertinoFontFamily` and three slots derive from Inter) and a realized
`CupertinoButtonStyle` button rendering Inter. `Uno.Fonts.Inter/Fonts/*.ttf` is present in the head's output,
so the assertion is not passing on a silent fallback. The real shell boots under the theme and stays up
with clean logs. Formatters clean.

Hosting smoke (`ThemesSampleApp --smoke`, Win32, Release): **FAIL, and identically on an untouched `master`
worktree** — in both, every guest hosts, the *Material* guest's ALC is reported not reclaimed, and
`Application.CleanupNonDefaultAlcCaches` throws `TargetParameterCountException` (the wrapper's reflection
call no longer matches Uno 7.0.0-dev.701). On this branch Cupertino hosts and its ALC is fully collected.
Not caused by this work; recorded under "Hand-offs".

### Phase 1, slice 4 — 2026-09-18: semantic contract ratchet, live legacy brushes, legacy shim (D-1)

Landed:

- `Given_CupertinoTheme.When_ThemeLoaded_Then_EverySemanticKeyResolvesOrIsTrackedAsPending` — the 72-key
  semantic contract (Simple's 70 aliases + `ElevatedButtonStyle` + `CommandBarStyle`) with an explicit
  `PendingSemanticKeys` list. It fails when an unlisted key does not resolve **and** when a pending key
  starts resolving, so the list can only shrink. Mutation-checked both ways; the failure names the key.
- `Uno.Themes`: `BaseTheme.ColorLayers` (internal) exposes the resolved colour layers of the last rebuild;
  `SemanticBrushUpdater.Apply(brushes, layers, map)` rewrites a design system's own brushes from them. The
  per-layer lookup was factored out of `TryResolve` unchanged. A map entry lists candidate colour keys:
  the highest-precedence layer defining any of them wins, and within a layer the first listed wins.
- `CupertinoColors.xaml` → `CupertinoBrushes.xaml` (no code-behind class), owned by the theme: loaded
  once, rewritten in place on every rebuild through `CupertinoConstants.BrushColorKeys`, re-added as a
  dynamic layer. `CupertinoBlueBrush` and `CupertinoLinkBrush` list `PrimaryColor` second, so they follow a
  seed or a `PrimaryColor` override while a consumer's `CupertinoBlueColor` override still wins.
- Palette: the nine existing system colours and `LinkColor` moved to Apple's 2025-06-09 values;
  `CupertinoMint` / `Cyan` / `Brown` `Color` + `Brush` added.
- Legacy shim: `CupertinoColors` and `CupertinoFonts` are `[Obsolete]` recorders of `OverrideSource`;
  `CupertinoResources : CupertinoTheme` (`[Obsolete]`, still `sealed`) applies the recorded overrides and
  translates a legacy `CupertinoFontFamily` override into `DefaultFontFamily`. `_Resources.xaml` no longer
  nests the two legacy dictionaries.

Result: Cupertino **54 / 54**; Simple **256 passed + 1 skipped** (the `[Ignore]` already on `master` from
#1679); Material **63 / 63** — the shared-library change left both suites unchanged. Formatters clean; the
real shell boots with clean logs.

Deviations:

- **The shim routes the colour override through `ColorOverrideSource`, not the base constructor.** A
  constructor `colorOverride` is merged *into* the theme's base palette, where the brush rewrite (which
  reads each layer's own theme blocks) cannot see it; the first version of the shim failed its own test for
  exactly that reason. Cost: one extra theme rebuild at startup for legacy consumers with a colour
  override. See "Hand-offs" — the constructor path has the same blind spot in Simple and Material.
- **Glass-tint and switch colour keys not added yet** — nothing reads them before Phases 2–3.
- `AddThemeSpecificResources` loads `CupertinoBrushes.xaml` unguarded, exactly as `BaseTheme` loads
  `SharedColors.xaml`: an in-package URI that fails to load means every style is missing too, and catching
  it without a logger available in this assembly would be a silent swallow (AGENTS.md §8).

### Phase 1, slice 5 — 2026-09-18: implicit styles, frozen V1, Toolkit inventory, docs

Landed:

- **Implicit styles** in `_Resources.xaml`: the set `CupertinoResources.WithImplicitStyles` used to export,
  minus `ProgressRing` (compiled out on Windows until D-7's storyboard ring). Implicit `Button` = the plain
  style (D-2). Asserted on realized controls, not by lookup.
- **`CupertinoResourcesV1`** (D-1 escape hatch): `master`'s 17 control dictionaries and 5 application
  dictionaries copied verbatim under `Styles/{Controls,Application}/v1/` and merged into a second output,
  `Generated/mergedpages.v1.xaml`; `CupertinoColorsV1` (x:Class, palette merged before
  `InitializeComponent` so the brushes' `{StaticResource}` resolve) and `CupertinoFontsV1` read the
  overrides recorded by the obsolete `CupertinoColors` / `CupertinoFonts`. All three types are `[Obsolete]`
  from birth. A diff of every v1 file against `master` shows only the two intended edits (the `x:Class`
  rename, the two nested dictionary names). `mergedpages.xaml` is unaffected (no v1 origins).
- **Toolkit key inventory** (`uno.toolkit.ui@d88a0c12`, `src/library/Uno.Toolkit.Cupertino`): referenced keys
  minus keys it declares itself minus WinUI system brushes leaves **8** keys it needs from this library —
  `CupertinoBlueBrush`, `CupertinoLabelBrush`, `CupertinoSystemBackgroundBrush`,
  `CupertinoTertiarySystemFillBrush`, `CupertinoBlueColor`, `CupertinoQuaternaryGrayColor`, `LabelColor`,
  `SystemBackgroundColor`. All resolve under `CupertinoTheme`; pinned by
  `When_ThemeLoaded_Then_KeysTheToolkitDependsOnResolve`. It has no code dependency on the legacy classes.
- **Docs:** `cupertino-getting-started.md` installs and customises through `CupertinoTheme`, documents the
  Cupertino colour vocabulary and its live brushes, and gains a migration section (what changes on upgrade,
  the V1 escape hatch); `seed-colors.md` / `themes-overview.md` / `design-tokens.md` no longer exclude
  Cupertino.

Result: Cupertino **67 / 67** on `net10.0-desktop` Release. Formatters clean; cSpell and markdownlint clean
on the edited pages.

Deviations and things to know:

- **The V1 override test was split.** Uno builds the dictionary graph behind a `Source`-loaded
  `mergedpages.v1.xaml` once per process, so every `CupertinoResourcesV1` shares one nested
  `CupertinoColorsV1`: the recorded override is read on the first load only. A brush-level "with override"
  and "without override" assertion therefore cannot both pass in one run, in either order. The suite asserts
  the override at the `CupertinoColorsV1` colour level instead; the end-to-end brush assertion was confirmed
  passing alone in a fresh process, which is the real-app scenario (one `App.xaml`, loaded once).
- **+23 duplicated `Uno0001` warnings** ("not implemented in Uno") from the v1 merged page's generated code —
  the same 23 the main merged page already emits, because the XAML is identical. Not suppressed: widening
  `NoWarn` needs approval (AGENTS.md §4). Material's props already carry `NoWarn` for it; **maintainer call.**
- **V1 keeps the Lottie progress ring**, so D-7 (drop Lottie) and the frozen V1 collide in Phase 3: either V1
  loses its `ProgressRing.xaml`, or the Lottie dependency outlives D-7 by one major. Decide there.
- Only `net10.0-desktop` was built and tested. The x:Class dictionary and the `WinUI_Desktop` branch of
  `CupertinoResourcesV1` are unverified on `net10.0-windows10.0.19041`; CI covers it.

**Phase 1 status:** every item is done or explicitly deferred with a reason, except the phase gate's hosting
smoke, which fails identically on `master` (see "Hand-offs"), and the three settings DPs, which move to
Phases 2–3 with their consumers.

Environment note, **corrected 2026-09-18**: this note used to blame a locally overridden Uno build for the
Debug failures of the sample heads. That was an unverified guess and it was wrong. Debug was broken on
`master` for every head by the SDK's Debug-only tooling packages (see `specs/lessons.md`, top entry); fixed in
`fix(samples): make the sample heads build and start in Debug on Uno 7`. Slices 1–5 above were verified in
Release only at the time; the Cupertino suite has since been run in **Debug** as well (67 / 67), which also
exercises XamlMerge's unmerged Debug output.

