# Fluent Theme — Progress

Plan of record: `specs/05-fluent-theme/spec.md`. Update checkboxes as work lands.

## Phase 0 — Spikes

- [x] Spec written (`spec.md`, 2026-07-14)
- [x] **S1** — cross-dictionary StaticResource alias resolution — **GO** (2026-07-14)
  - [x] Spike dictionary `FluentAliasSpikeDictionary.xaml` added to SimpleSampleApp RuntimeTests
  - [x] `Given_FluentAliasResolution.cs` added (cases 1–6 per spec §14.1)
  - [x] Headless desktop run green (CI-parity host): 14/14; full suite 107 passed / 1 skipped (pre-existing) / 0 failed
  - [x] Results recorded in `spike-results.md`; decisions recorded: D6 → mechanism C for palette; new D16 → no alias chaining
- [x] **S2** — Fluent token values captured on Skia desktop (Dark branch + accent set) — Light branch & Windows/WASM pending (Phase 1)
- [x] **S3** — `Default*Style` key availability table folded into spec §5.2 (Skia) — Windows/WASM probe & M-CODE verification pending (Phase 1)

## Phase 1 — Core library (`Uno.Fluent.WinUI`)

- [x] Project scaffold (csproj, props, constants, `FluentTheme`) (2026-07-15)
- [x] `_Resources.xaml` semantic aliases (post-S3 key set); Ⓜ keys + bundle-local
  aliases (TextButton/IconButton, 19 typography keys) late-bound in code
  (`FluentTheme._lateBoundStyleAliases` / `._bundleStyleAliases`)
- [x] Color palette — built in code per D6 mechanism C (`FluentColorPalette.cs`;
  no `ColorPalette.xaml`): live accent tokens + S2-captured per-branch neutrals,
  drift-guarded by `Given_FluentColorPalette`
- [x] `Typography.xaml` + `Fonts.xaml` + `TextBlock.xaml` + bridge Button styles.
  Typography/Fonts are Source-merged from `BaseDictionaries.xaml`, NOT part of the
  XamlMerge bundle (Release resolution — see lessons.md entry 2026-07-15)
- [x] Packaging: sln/slnf, InternalsVisibleTo, sample-app project reference.
  CI audit result: **no pipeline changes needed** — packages stage builds the
  slnf with `GeneratePackageOnBuild`, signing/publishing glob over `*.nupkg`,
  canary targets the sln
- [x] Runtime tests: `Given_FluentSemanticStyles`, `Given_FluentColorPalette`,
  `Given_FluentTypography`; `Given_ColorOverridePrecedence` extended with two
  FluentTheme cases
- [x] Docs: `fluent-getting-started.md` FluentTheme section, `semantic-styles.md`
  Fluent column + typography note, `themes-overview.md` roster entry
- [ ] Windows/WASM validation of the S1/S3 assumptions (deferred residual risk —
  spec §14.1/§5.3; tracked for the PR's platform-tested checklist)

## Phase 2 — Seed → accent

- [x] S4(a) accent closure enumeration (2026-07-15, Skia desktop — see
  `spike-results.md` §S4: on Uno, `SystemAccentColor*` alone cascades fully;
  the closure is written anyway as the Windows insurance per D12, with
  seed-invariant tokens — `TextOnAccentFillColor*`, `*Disabled` — excluded)
- [x] Closure override implementation (`FluentAccentPalette` + FluentTheme
  rebuild hook, single-entry cache keyed by seed) + `Given_FluentSeedAccent`
- [x] `doc/seed-colors.md` (Fluent tab + "Seed → Accent Cascade" section,
  including the in-place-clear cache caveat)

## Phase 3 — Lightweight bridge (per control: Button → TextBox → CheckBox → RadioButton → ToggleSwitch → Slider)

- [x] S4(b) ThemeResource re-pointing validation (2026-07-15 — verdict YES on
  Uno for app scope, nested dynamic layer, attached layer swaps, and container
  scope; see `spike-results.md` §S4(b))
- [x] Button (2026-07-15 — `FluentLightweightBridge`: semantic defaults per
  branch + override-driven re-pointing via `Colors.OverrideDictionary`;
  `{ThemeResource}` foreground setters on the shipped bridge styles;
  `Given_FluentLightweightStyling`; docs. Not bridged (documented):
  FilledTonal*/Elevated* (share the standard button), IconForeground variants,
  StateLayer/Elevation keys; Outlined hover/pressed/Disabled are pass-through
  only)
- [x] TextBox (2026-07-15 — both semantic families re-pointed onto the single
  Fluent TextBox's `TextControl*` resources, Outlined wins collisions; rest
  defaults from verified tokens; rendered E2E test)
- [x] CheckBox (2026-07-15 — key names are WinUI-native except the glyph
  family, bridged onto `CheckBoxCheckGlyphForeground*`; native names guarded
  by an XCR presence probe)
- [x] RadioButton (2026-07-15 — fully WinUI-native key names, nothing to
  bridge; presence-guarded. `RadioButtonStateCircleBackground*` not bridged)
- [x] ToggleSwitch (2026-07-15 — Material names mapped onto WinUI's
  (`FillOn/Off`, `StrokeOn/Off`, `KnobFillOn/Off*`); ON-family defaults from
  the accent fill; shadow/bounds/icon/Focused keys not bridged)
- [x] Slider (2026-07-15 — fully WinUI-native key names (`SliderTrackFill*`,
  `SliderTrackValueFill*`, `SliderThumbBackground*`, `SliderTickBarFill`),
  nothing to bridge; presence-guarded)

## Phase 4 — Samples

- [x] Head decision (2026-07-15): **dedicated `FluentSampleApp`** (owner call)
- [x] `src/samples/FluentSampleApp/` head: XCR→FluentTheme App.xaml topology,
  shared SamplesApp.Shared import, all four sample TFMs; registered in
  `Uno.Themes.sln` and the four CI sample-head matrices (android/desktop/
  ios/wasm)
- [x] Shared UI: `Design.Fluent` (enum, `SamplePageLayout.FluentTemplate` DP,
  mapping, visual state + content container). Samples are opt-in per page via
  `SupportedDesigns` — pages without a FluentTemplate simply don't appear in
  the Fluent head (no fallback-crash risk)
- [x] Fluent sample templates (semantic keys only) for the six bridged
  controls: Button, TextBox, CheckBox, RadioButton, ToggleSwitch, Slider
- [x] Fluent templates for the remaining §5 alias surface (2026-07-15) — 22
  more pages opt into `Design.Fluent` with semantic-key-only FluentTemplates:
  AppBarButton, Fab, IconButton, ToggleButton, HyperlinkButton, PasswordBox,
  ComboBox (+`ComboBoxItemStyle`), ListView (+`ListViewItemStyle`), TextBlock
  (19 typography slots), MenuFlyout (item/toggle/radio/sub/separator), Flyout
  (+`FlyoutPresenterStyle`), ContentDialog, CommandBar, ProgressBar,
  ProgressRing, CalendarView, CalendarDatePicker, DatePicker, PipsPager,
  RatingControl, NavigationView (MUX), Top NavigationView (MUX). §15
  acceptance "renders all §5 aliases" now met **except** MediaTransportControls
  (see deferral below). Verified: `FluentSampleApp` navigates all **28** Fluent
  pages under xvfb with `ok=28/28`, zero exceptions, and no unresolved semantic
  key (only the pre-existing design-independent `Material*`/`Cupertino*`
  shared-chrome `warn:` lines from the other designs' templates in each shared
  page).
- [ ] **MediaTransportControls** (`MediaTransportControlsStyle`) sample —
  deferred. The key resolves and is guarded by `Given_FluentSemanticStyles`,
  but `MediaPlayerElementSamplePage` is Material-only, launches nested pages,
  and hosts live media playback (fragile headless); spec §5.2 itself flags
  MediaTransportControls availability as platform-varying (D8 GAP exception).
- [ ] Screenshot pass vs WinUI Gallery (§15) — needs a windowed environment

## Phase 5 — ThemesSampleApp wrapper hosting

`ThemesSampleApp` (the ALC wrapper head, `specs/05-alc-wrapper-app/`) landed on
`master` while this branch was in flight. Rebased onto it and registered
`FluentSampleApp` as its fourth hostable guest.

- [x] Head-side accommodations (the only two the wrapper asks of a guest, same as
  the other three heads): `UnoEnableAlcAppSupport=true` in `FluentSampleApp.csproj`,
  and `MainWindow = new Microsoft.UI.Xaml.Window()` in place of `Window.Current`
  (a process-wide static in the *shared* `Uno.UI` — hosted, the guest would grab
  the wrapper's window). Both stay correct standalone.
- [x] All six wrapper declaration sites, per the list in `GuestAppCatalog`'s own
  docstring:
  1. `GuestAppCatalog.Apps` — `("Fluent", "FluentSampleApp", "FluentSampleApp")`,
     last in picker order
  2. `ThemesSampleApp.csproj` — `_GuestWasmApp` payload list + the desktop
     build-ordering `ProjectReference`
  3. `Uno.Themes.sln` — wrapper's `ProjectDependencies` (IDE/solution build order;
     the csproj P2P only covers override-driven CLI builds)
  4. `build/scripts/build-wasm-guest-heads.sh` — the wasm guest-head loop
  5. `GuestHosting/GuestSharedAssemblies.txt` — `!Uno.Fluent.WinUI`
  6. `.vscode/tasks.json` — `build-Fluent-wasm` in `build-Themes-wasm`'s `dependsOn`
- [x] **`!Uno.Fluent.WinUI` (isolate), not shared.** Same reasoning as the other
  repo-built theme libraries (lessons.md, 2026-08): "share if already loaded" is
  version-unsafe for any assembly the guest ships, and the Debug-only Hot Design
  tooling drags the *published* `Uno.Themes.WinUI` into a host bin. Note the rule
  does not collide with the pre-existing shared `^Uno.UI.FluentTheme` prefix —
  different assembly (`XamlControlsResources`' home, which guests legitimately
  share with the host for type identity).
- [x] **No font package needed for the Fluent guest.** A hosted guest resolves
  `ms-appx:///` against the *host's* package root, which is why the wrapper carries
  Roboto/Inter for Material/Simple. Fluent uses the platform default via
  `ContentControlThemeFontFamily` (D11) so it adds nothing — recorded in the
  wrapper csproj's font comment so the omission reads as deliberate.
- [x] **Verified** (2026-08-11, Release desktop, WSLg):
  - `dotnet build ThemesSampleApp.csproj -c Release -f net10.0-desktop
    -p:TargetFrameworkOverride=desktop` — 0 errors, warning set unchanged
    (the pre-existing `SamplesApp.Shared` nullable set, shared by every head);
    no-bleed still green (no theme / ShowMeTheXAML / MSTest dll in the wrapper bin).
  - Hosting smoke `--smoke`: **RESULT: PASS** (exit 0) — all four guests load,
    present content and are hosted in turn, and every unloaded guest ALC is
    reclaimed, Fluent included.
  - Share/isolate invariant re-checked mechanically against the built bins: the
    Fluent guest carries 2 isolated theme libraries and 36 host-shared
    assemblies, with **no** assembly marked `=`/`^` that is absent from the
    wrapper's closure (that invariant is what strands a wasm guest).
  - Full runtime-test suite (CI-parity host): **323 cases — 322 passed, 1
    skipped** (`When_BaseThemeIsCollected_Then_HotReloadHandlerDoesNotResurrectIt`,
    the pre-existing skip). `Given_Fluent*` alone: 227/227.
  - Not run locally: the wasm leg (guest payload packaging + browser `?smoke`).
    CI covers it via `build/stage-build-wasm.yml`; the payload list and the
    guest-head build script are the two Fluent-specific inputs there.

## Phase 6 — Master integration (2026-09-04)

`master` moved 15 commits while this branch was in flight (Color Fidelity #1697,
DefaultSpacing/Density #1701, `Application.GetTheme()` #1699, single typeface
`DefaultFontFamily` #1707/#1710, ThemeResource scale setters #1715). Merged
(`git merge origin/master`, 4 conflicts: `.gitignore`, `doc/semantic-styles.md`,
`specs/lessons.md`, `Given_ColorOverridePrecedence.cs` — all additive, both sides
kept) and adapted:

- [x] **Accent cascade follows `SeedColorMode`** (red/fix/green). #1697 made
  `Fidelity` the default and pinned the generated light `PrimaryColor` to the
  seed verbatim; `FluentAccentPalette` still re-solved tone 40 of a raw-chroma
  palette, so under a seed the built-in accent (`SystemAccentColor`) no longer
  equalled the semantic `PrimaryColor` — the §9.3 contract — and
  `SeedColorMode.TonalSpot` was ignored entirely. Fix: `FluentTheme` reads
  `Colors.SeedColorMode`, `FluentAccentPalette.SeedTones` derives the accent
  the generator's way (Fidelity: seed verbatim; TonalSpot: tone 40 of the
  chroma-≥48 palette) and `PaletteOf(color, mode)` is shared with the
  lightweight bridge's fill tones. The pure-seed cache key now includes the mode.
- [x] **Dropped the obsolete `UseHighFidelityColors` override** (CS0672 build
  warning after the merge; the base default is now Fidelity — behavior unchanged).
- [x] **Stopped double-merging `FontOverrideDictionary`**: `BaseTheme.UpdateSource`
  now resolves and merges it itself (last, hot-reload re-read); the copy in
  `AddThemeSpecificResources` would nest the same instance twice (illegal on WinUI).
- [x] **Adopted the `DefaultFontFamily` root** (red/fix/green): `Fonts.xaml` now
  declares `DefaultFontFamily` → `ContentControlThemeFontFamily` in place of the
  removed `TypefacePlain`/`TypefaceBrand` pair, and keeps every slot alias direct
  (Fluent's root is itself an alias, and alias chaining does not resolve — D16 —
  so `SharedTypography.xaml`'s slot → root cascade cannot serve Fluent). The
  duplicated slot aliases in `Typography.xaml` were dead (Fonts.xaml is merged
  after it) and are deleted; that file now overrides size/weight/spacing only,
  like the other design systems. `BaseTheme.DefaultFontFamily` (runtime) reaches
  the Fluent slots through the generated layer — guarded by
  `Given_FluentTypography.When_DefaultFontFamilySet_SlotFollows_AndClearRestoresPlatformDefault`.
- [x] Tests: `Given_FluentSeedAccent` +3 (`When_SeedSet_SystemAccentColorIsTheSeed`,
  `When_SeedSetInTonalSpotMode_AccentFollowsBoostedPalette`,
  `When_SeedSet_ForwardAndReverseFlowsAgree_InBothBranches` — a resource-graph
  assertion that holds in either ambient theme; the pre-existing ambient-branch
  test passed on a dark-mode dev machine and only the Light CI host would have
  caught the drift), `Given_FluentTypography` root row → `DefaultFontFamily` +4
  `DefaultFontFamily` rows. Red proven by stashing the library fix: 5 of the new
  cases fail on the pre-fix library, all pass after.
- [x] Docs: `seed-colors.md` (cascade bullets mode-aware), `fluent-getting-started.md`
  (seed bullet), `design-tokens.md` (Fluent typeface default), spec §3.3/D4/§7.2/§9.1.
- [x] Verification: Debug desktop, Fluent + precedence filter: **239 / 0 failed**
  (baseline post-merge 234 / 0); `FluentSampleApp` desktop build clean; Release
  CI-parity full suite — see the review log entry.

## Gap audit — 2026-09-04

Requested with the master integration: a full inspection of the adapter, its
distance from stock Fluent (`XamlControlsResources`), and the parts of the
semantic layer it does not cover. Facts below were checked against the Uno.UI
source (`src/Uno.UI.FluentTheme.v2/Resources/Version2`) and this repo; items are
ranked by consumer impact. None are fixed here (owner call on scope); each
names the cheapest fix.

### A. Fluent theme implementation — things to fix

1. **`DefaultFontFamily` does not reach built-in Fluent controls.** XCR templates
   read `{ThemeResource ContentControlThemeFontFamily}`; under Material/Simple the
   templates read the theme's tokens, so one property swaps the font app-wide,
   under Fluent only semantic-styled text follows. Fix: when `DefaultFontFamily`
   is set, `FluentTheme` writes `ContentControlThemeFontFamily` (the literal
   family) into its dynamic layer — the same override-driven re-pointing the
   bridge uses; Fluent's slot aliases then resolve to it too (one level, D16-safe).
   ~5 lines + a rendered test.
2. **`DefaultCornerRadius` does not reach built-in Fluent controls** (G4 claims
   participation; spec §8 says "nothing to build"). XCR templates consume
   `ControlCornerRadius` / `OverlayCornerRadius`: 44 files via `{ThemeResource}`
   (re-pointable — S4(b) mechanism), 6 via `{StaticResource}` (CalendarView,
   ColorPicker, PagerControl, RadioButton, Slider, ToggleSwitch — not reachable
   without re-templating). Fix: when `DefaultCornerRadius` ≠ 4, write
   `ControlCornerRadius` = `Radius100` and `OverlayCornerRadius` = `Radius200`
   into the dynamic layer. ~15 lines + test + `design-tokens.md` note.
3. **Accent shades are absolute tones.** `Light1–3`/`Dark1–3` are tones
   60/70/80/30/20/10 of the seed's palette regardless of the seed's own tone, so a
   seed darker than tone 30 (navy brand colors) gets a light-theme fill (`Dark1`)
   *lighter* than the accent, and `Dark3` (tone 10) may be lighter than a very
   dark override basis. Windows derives shades relative to the accent's lightness.
   Fix: tone offsets from the seed's own HCT tone, clamped to [0, 100], in
   `SeedTones` / `BuildBranchFor`. Recorded in spec §9.1.
4. **Override-driven verbatim fill ignores contrast.** A light `PrimaryColor`
   override becomes the accent fill verbatim while `TextOnAccentFillColorPrimary`
   stays white → unreadable accent buttons. Material picks `OnPrimary` by
   contrast. Fix: in override mode also write `TextOnAccentFillColorPrimary` (+
   `Brush`, + `Secondary`) from the effective `OnPrimaryColor` (override >
   palette > default). Seeds are unaffected (fill = tone 30 / 70 keeps contrast).
5. **Palette self-heal is one rebuild late for brushes.** When XCR is merged
   after FluentTheme (against the documented order), the palette heals in
   `AddThemeSpecificResources`, but `SemanticBrushUpdater` swept the brushes
   earlier in the same pass — `*Color` keys are correct immediately, `*Brush`
   instances catch up on the next rebuild. Documented at the code site; a clean
   fix needs a pre-color hook in `BaseTheme` (not worth it for a misordering
   the docs already forbid). Verified the documented order is what the Uno XAML
   generator emits (`Resources.MergedDictionaries.Add` in document order, before
   the next item is constructed), so consumers following the docs never hit it.
6. **Semantic Primary vs Fluent fill is a documented characteristic, not a bug:**
   Fluent's accent-button fill is `Dark1` (light) / `Light2` (dark), while
   `PrimaryColor` = `SystemAccentColor` (light) / `Light2` (dark), so under a
   seed `PrimaryBrush` ≠ `FilledButtonStyle` background in light theme (tone 30
   vs seed) and in dark theme (tone 70 vs 80). Under Material they are equal.
   Worth a sentence in `fluent-getting-started.md` "Behavior notes".
7. Tooling note: `dotnet dnx XamlStyler.Console` on this checkout rewrites
   CRLF files as LF (whole-file diffs); the two Fluent XAML edits were re-applied
   without it. Check `git diff --stat` after any styler run.

### B. FluentTheme vs stock Fluent (`XamlControlsResources`)

1. **Lightweight styling only through `Colors.OverrideDictionary`.** The
   idiomatic WinUI form — `<SolidColorBrush x:Key="FilledButtonBackground">` in
   `App.xaml` after the theme, or in `Page.Resources` — re-points nothing under
   Fluent, while the same XAML works under Material/Simple (their templates read
   the semantic keys). Documented, but it is the largest portability gap. The
   alternative (always re-point `AccentButtonBackground` ←
   `{ThemeResource FilledButtonBackground}`, declaratively) was rejected in Phase 3
   to leave stock rendering and Windows live-accent tracking untouched; worth
   re-deciding once the windows TFM is dropped (the live-accent argument goes
   with it).
2. **`DefaultSpacing` / `DefaultDensity` have no Fluent effect** (spec §8,
   accepted). WinUI's own compact set (`XamlControlsResources.UseCompactResources`)
   lives on the consumer's XCR instance — out of the adapter's reach; document
   the combination in `fluent-getting-started.md`.
3. **No HighContrast branch.** XCR switches to system colors under Windows high
   contrast; the Fluent palette / lightweight defaults declare Light/Default
   only, so semantic brushes resolve HighContrast → Dark → Default
   (`ThemesConstants.BrushThemeSources`) and keep Fluent dark values next to
   system-colored built-in controls. Applies to every theme; most visible here.
   Fix: a HighContrast branch in `ColorPalette.xaml` mapped to `SystemColor*`.
4. **Surface roles map to solid fallbacks** (`SolidBackgroundFillColor*`, N3):
   Fluent's translucent `CardBackgroundFillColorDefault` / `LayerFillColorDefault`
   have no semantic counterpart, so a semantic "card" renders flat-solid.
   Accepted non-goal; note alongside Mica/Acrylic.
5. Open from earlier phases: MediaTransportControls sample, screenshot pass vs
   WinUI Gallery, Windows/WASM validation of S1/S3, Windows-Skia accent probe,
   D12 closure strip after the windows-TFM drop.

### C. Semantic layer coverage under Fluent

1. **Semantic style keys: complete.** Every §5 key resolves (no GAPs — Fluent is
   the only theme with none; Simple has three). Material-only keys
   (`DatePickerFlyoutPresenterStyle`, `RippleStyle`) and Simple-only controls
   are not semantic — n/a.
2. **Color roles: complete** (33/33; `ShadowColor` keeps the shared default).
   Semantic-layer gap (all themes): no roles for Fluent's Success / Caution /
   Attention system fills (InfoBar severities), card/layer fills, focus strokes
   — apps cannot express those portably. A semantic-layer extension, not a
   Fluent fix.
3. **Typography: complete** (19 slots, `DefaultFontFamily` root, runtime swap).
4. **Lightweight-styling keys: 6 of 20 documented controls bridged.** Pages
   without a Fluent column (documented key count): AppBarButton 7,
   CalendarDatePicker 47, ComboBox 105, DatePicker 62, FloatingActionButton 82,
   HyperlinkButton 32, NavigationView 172, PasswordBox 93, PipsPager 66,
   ProgressBar 9, ProgressRing 6, RatingControl 39, TextBlock 92, ToggleButton
   110. Under Fluent these keys have **no default at all** — app XAML that reads
   a documented key directly (`{ThemeResource HyperlinkButtonForeground}`)
   fails to resolve, where Material/Simple provide a value. Cheapest next
   increments, in order: **HyperlinkButton** (the documented names look like
   WinUI's own `HyperlinkButtonForeground*`/`Background*`/`BorderBrush*` —
   probe like RadioButton/Slider, likely native), **PasswordBox** (same
   `TextControl*` family as the TextBox map, plus the reveal button),
   **ProgressBar/ProgressRing** (few keys), then ToggleButton and ComboBox
   (divergent names, full maps).
5. Tokens: `Space*`/`Radius*` are generated but unconsumed by Fluent controls
   (A2, B2); opacity/state tokens map to nothing in Fluent's discrete state model
   (N2, accepted).

## Follow-ups

- [ ] **Windows-TFM drop (announced 2026-08-11, timing TBD):** when the
  WinAppSDK (`net*-windows`) TFM is dropped (Windows served by Skia
  `net10.0-desktop`), strip the D12 accent closure from `FluentAccentPalette`
  (`_closureKeys`, `WriteClosure`, `BuildSeedClosure`, the closure halves of
  `BuildBranchFor`) — it exists solely as Windows eager-resolution insurance
  (S4(a): on Uno the shades alone cascade). Retire D12 and the Windows-side
  residual items in spec §14/§5.3 in the same change. Do NOT strip while
  packages still ship the windows TFM (silently breaks G5 for WinAppSDK
  consumers).
- [ ] **Windows-Skia accent probe:** on a Windows machine, change the OS
  accent color and dump `SystemAccentColor` in FluentSampleApp on
  `net10.0-desktop` — does Uno wire the OS accent through? Determines whether
  the "live accent tracking" rationale survives the TFM drop (and how the
  FluentColorPalette / seed-colors doc comments should be worded after the
  closure strip). S2/S4 captures on macOS/Linux showed Uno's baked default
  (`#FF0078D7`), not an OS value.

## Review log

- 2026-09-04 — **Master integration + gap audit** (Phase 6 above). Merge of
  15 master commits; four adaptations, two of them behavior fixes proven
  red/fix/green (accent cascade follows `SeedColorMode` — the pre-existing
  ambient-branch agreement test was green on this dark-mode machine and would
  only have failed in the Light CI host, hence the new both-branches
  resource-graph guard; `DefaultFontFamily` root). Debug desktop Fluent +
  precedence filter: 239 cases / 0 failed. Release CI-parity full suite:
  **491 cases — 490 passed / 1 pre-existing skip (`When_BaseThemeIsCollected_Then_HotReloadHandlerDoesNotResurrectIt`) / 0 failed** (Windows desktop host, dark ambient theme; master baseline before this branch is not directly comparable since the merge added its own suites). Gap audit recorded above (A1/A2 are the two
  cheap, high-value fixes: `DefaultFontFamily` and `DefaultCornerRadius` not
  reaching built-in Fluent controls).

- 2026-08-11 — **Direct semantic-key lookups now honor lightweight overrides
  (red/fix/green).** Found during the declarative-pass audit: the bridge's
  default layers (LightweightDefaults.xaml + code accent defaults) are merged
  *above* the colors layer carrying `Colors.OverrideDictionary`, so an
  override of a defaulted lightweight key (e.g. `FilledButtonBackground`)
  rendered correctly via re-pointing but a **direct**
  `{ThemeResource <semantic key>}` lookup still saw the default — diverging
  from Material/Simple, whose defaults sit below the colors layer. Fix:
  `ApplyRepointing` mirrors each override value onto the semantic key itself
  (alongside the Fluent per-control key), with the bridge-style-only keys
  (`TextButton*`/`IconButtonForeground`) covered via a mirror-only list.
  Red/fix/green:
  `When_DefaultedSemanticKeyOverridden_DirectLookupSeesOverride` (red on both
  the accent-derived and neutral variants, green after). Full CI-parity
  Release suite: 323 — 322 passed / 1 pre-existing skip.

- 2026-08-11 — **Declarative-first pass (owner correction: no runtime C#
  resource construction unless XAML provably cannot express it —
  `specs/lessons.md` entry of the same date).** Three code-built resource sets
  moved to declarative XAML, values unchanged:
  1. **Neutral palette** → new `Styles/Application/ColorPalette.xaml` (17
     roles × Light/Default, token-annotated). `FluentColorPalette` now builds
     only the 15 accent-derived roles per branch (mechanism C, justified: live
     tokens + branch fidelity) and transport-copies the XAML neutrals into the
     same branch dictionary, preserving the exact palette shape the
     SafeMerge/override-precedence contract is proven against; the
     all-or-nothing gate and the ctor-failure self-heal are unchanged.
  2. **Lightweight neutral defaults** → new
     `Styles/Application/LightweightDefaults.xaml` (30 brushes × branch),
     loaded once per theme instance and merged below the code bridge layer.
     `FluentLightweightBridge` keeps only the accent-derived defaults
     (Filled background family @1/0.9/0.8, ToggleSwitch ON track) and the
     override-driven re-pointing — the per-rebuild allocation drops from ~70
     brushes to ≤10 (WASM §2 concern resolved); the two duplicated
     "keep in sync" C# capture tables are deleted.
  3. **Ⓜ gap keys** → nine declarative empty styles in `_Resources.xaml`
     (`FluentTheme._lateBoundStyleAliases` + `ResolveBuiltInStyle` deleted).
     On Uno the code path always produced the empty style anyway; an empty
     style keeps the built-in default template, so Windows renders identically
     through the pure default-style path. Behavior note: an app-defined
     *implicit* style for one of these controls is no longer picked up by the
     semantic key — the key now always means "the Fluent default", which is
     the D8 promise.
  Both new XAML files are `XamlMergeInput`-excluded (theme-branch resources in
  the merge bundle break in Release — existing lesson) and load through
  guarded paths that log-and-degrade instead of throwing. Stays code-built
  (justified per the lesson): `FluentAccentPalette` + re-pointing
  (runtime-input-driven, owner-approved "for now"), the accent-derived
  palette/bridge values (live tokens, branch fidelity), and the bundle-local
  style aliases (`_bundleStyleAliases` — a XAML alias below app scope binds a
  foreign app-level key or nothing; container-scope correctness is
  test-guarded). Verification: Fluent-filtered Debug run 228/228; full
  CI-parity Release suite **322 — 321 passed / 1 pre-existing skip / 0
  failed** (identical to baseline); library build clean (no new warnings).
  Tests changed only in comments/messages + two method renames
  (`When_LateBoundAlias_*` → `When_GapKey_*`); zero assertion changes.

- 2026-07-16 — **Override-driven accent cascade (owner report).** FluentTheme
  handled only `Colors.PrimarySeed` in its Fluent-specific layers: a
  `PrimaryColor` override — via `Colors.OverrideDictionary`/`OverrideSource`
  or the obsolete `ColorOverrideDictionary`/`ColorOverrideSource` BaseTheme
  properties — changed the semantic palette but left every visible built-in
  Fluent control untouched (no parity with Material/Simple, whose templates
  consume the semantic brushes directly). Fix: the accent cascade
  (`FluentAccentPalette`) and the lightweight bridge's accent fill now derive
  from the **effective primary** — per-branch `PrimaryColor` override
  (verbatim, shades derived tonally from it) > seed (tonal mapping,
  unchanged) > platform accent. Consumer-explicit accent-family keys
  (`SystemAccentColor*`, `AccentFillColor*`, …) always win over derived
  values. Also fixed: manual `ThemeDictionaries` reads (bridge re-pointing +
  basis resolution) now honor the consumer's `Dark` branch key with `Default`
  as the universal fallback (consumers never write `Default` — see
  `ColorPaletteOverride.xaml` in the sample heads); URI-backed overrides are
  re-resolved per rebuild for the accent/bridge layers (hot-reload parity
  with the base colors layer); the pure-seed accent dictionary stays cached,
  override-driven passes rebuild (override contents can mutate without a
  reference change). Red/fix/green: 8 new runtime tests
  (`Given_FluentSeedAccent` override region ×6 incl. both obsolete channels
  and the URI channel via `RuntimeTests/FluentColorOverride.xaml`;
  `Given_FluentLightweightStyling` branch-key tests ×2) — all red before,
  green after. Full CI-parity Release suite: 322 total — 321 passed / 1
  pre-existing skip (baseline 313/1, +8 new). Docs: `seed-colors.md`
  ("PrimaryColor overrides drive the accent too"), `lightweight-styling.md`
  (theme-branched override note); new `specs/lessons.md` entry (Dark branch
  key + dark-branch testability).

- 2026-07-15 — **Phase 4 §5 samples — review pass.** `quality` → *approve*
  (no blocker/high/medium; keys design-agnostic, formatting consistent, doc
  edits accurate); `skeptic` → *ship* (cross-head impact structurally
  impossible — collapsed content presenters never inflate the non-active
  template, proven by the Fluent head, which merges no Material resources,
  inflating all 28 pages without throwing on their Material templates' keys;
  zero key-resolution gaps). Review-driven follow-ups landed: (1) MenuFlyout
  now applies the semantic keys (`MenuFlyoutPresenterStyle` + item/toggle/
  radio/sub/separator) instead of implicit styling, so it actually demonstrates
  them; (2) the determinate `ProgressRing` gained `IsActive="True"` (it rendered
  an empty ring, a bug inherited from the Simple sibling); (3)
  `Given_FluentTypography` alias coverage extended 6 → all 19 slots. Also
  confirmed `MaterialSampleApp` + `CupertinoSampleApp` desktop builds are clean
  (shared XAML is head-agnostic). Residual (low, tracked): flyout/dialog
  *interaction* paths and non-desktop (Android/iOS/WASM) inflation are
  unexercised here; the pre-existing `async void ShowContentDialog` lacks a
  full-body try/catch (AGENTS §10) — shared infra, flagged for a separate fix.
- 2026-07-15 — **Phase 4 §5 sample surface complete.** Extended `Design.Fluent`
  opt-in + semantic-key-only FluentTemplates to 22 more shared sample pages,
  bringing the Fluent head to **28** pages covering the whole §5 alias surface
  (all button/toggle/input/collection/overlay/progress/picker/pager/nav
  controls) — only MediaTransportControls deferred (Material-only, nested-page
  media host; key resolves + is test-guarded). Templates reference only the
  semantic keys proven by `Given_FluentSemanticStyles`, including the
  late-bound Ⓜ keys (`NavigationViewStyle`/`NavigationViewItemStyle`,
  `CommandBarStyle`, `ProgressRingStyle`, `PipsPagerStyle`,
  `RatingControlStyle`, `CalendarDatePickerStyle`, `ListViewStyle`) whose
  empty-style fallback keeps the built-in Fluent default template.
  ContentDialog reuses the shared theme-agnostic dialog builders (they inherit
  Fluent's implicit ContentDialog style). **Verification:** temporary
  navigation sweep in `FluentSampleApp` under xvfb visited all 28 Fluent pages
  → `ok=28/28`, zero exceptions/unhandled, and no unresolved *semantic* key
  (only the pre-existing design-independent `Material*`/`Cupertino*`
  shared-chrome `warn:` lines from the other designs' templates in each shared
  page — same class the Simple head logs). CI-parity runtime suite unchanged:
  301 total / 300 passed / 1 pre-existing skip. Debug build clean (0 errors, no
  new warnings); SimpleSampleApp Release publish clean. No library/API/resource-
  key change — samples only, purely additive (`SupportedDesigns` only ever
  gained `Design.Fluent`, so the Material/Cupertino/Simple head rosters are
  unchanged).
- 2026-07-15 — **Phase 4 landed (initial)** — dedicated `FluentSampleApp`
  head per owner decision. Shared UI gained first-class `Design.Fluent`
  support; sample pages opt in per page (strict `SupportedDesigns` filter),
  so only pages with a real FluentTemplate appear — no template-fallback
  crash risk. Six bridged controls have Fluent templates using semantic keys
  only. Verification: head builds (desktop) and runs 25s+ under xvfb with no
  exceptions (the `Material*` resource warnings in the log are pre-existing
  shared-chrome behavior — the Simple head logs the same 31 occurrences);
  full CI-parity suite unchanged at 301 (300 passed / 1 pre-existing skip).
  CI: head added to the four sample-head build matrices. Remaining (tracked
  above): Fluent templates for the rest of the §5 alias surface; screenshot
  pass vs WinUI Gallery.
- 2026-07-15 — **Phase 3 complete** (TextBox/CheckBox/RadioButton/
  ToggleSwitch/Slider increments). Key-name probe against a fresh
  `XamlControlsResources` (recorded in `spike-results.md` §S4 addendum):
  Material's documented semantic keys for CheckBox (minus glyph family),
  RadioButton, and Slider ARE WinUI's per-control resource names — those
  controls need no bridging at all, overrides work natively at any scope
  (guarded by an XCR presence probe so an Uno.UI rename fails fast).
  TextBox maps both semantic families onto the single Fluent TextBox
  (`TextControl*`; Outlined wins collisions — documented); ToggleSwitch maps
  the divergent Material names onto WinUI's; CheckBox bridges only
  `CheckBoxGlyphForeground*`. Defaults follow the verified-tokens-only rule;
  everything else is re-point pass-through. Rendered E2E: TextBox required an
  explicit `DefaultTextBoxStyle` in the CI host (the app's implicit TextBox
  style is Simple's — host artifact, noted in the test).
  Verification: full CI-parity suite green (301 total — 300 passed / 1
  pre-existing skip); 32 new cases.
- 2026-07-15 — **Phase 3, Button increment complete** (S4(b) + bridge).
  S4(b) isolation verdict: per-control redefinitions win XCR-template
  `{ThemeResource}` lookups on Uno in every relevant topology, including
  nested-layer swaps while attached (`spike-results.md` §S4(b)).
  Design decisions beyond the spec text:
  1. **Re-pointing is override-driven only** — without a consumer override the
     bridge writes no per-control keys, so stock Fluent rendering (and live
     system-accent tracking on Windows) is untouched.
  2. **Override channel is `Colors.OverrideDictionary`** for app-wide semantic
     overrides (it triggers the rebuild the bridge hooks); plain app-scope
     resource definitions after the theme are not visible at rebuild time
     (documented). Page-scoped overrides use the Fluent per-control keys —
     except `TextButtonForeground*`/`IconButtonForeground`, which the shipped
     bridge styles consume via `{ThemeResource}` setters (element-scope, so
     scoped semantic overrides work directly).
  3. **Coverage:** Filled* (accent button) and Outlined* rest keys carry
     verified defaults; Outlined hover/pressed and all Disabled keys are
     re-point pass-through without defaults (platform values unverified per
     branch); FilledTonal*/Elevated* not bridged (indistinguishable from the
     standard button without re-templating — D1).
  Verification: full CI-parity suite green, 31 new cases (269 total — 268
  passed / 1 pre-existing skip).
- 2026-07-15 — **Phase 2 complete.** S4(a) ran on Skia desktop (results in
  `spike-results.md` §S4): on Uno, XCR's accent brushes re-resolve late-bound,
  so overriding the `SystemAccentColor*` shades alone recolors built-in
  controls — the D12 eager-resolution concern is Windows-only. Implementation
  (`FluentAccentPalette`, hooked into `AddThemeSpecificResources`) writes the
  shade set (flat, theme-invariant) plus the accent-derived closure per branch
  (fill = Dark1/Light2 at 1/0.9/0.8 brush opacity, accent text = Dark2/Dark3/
  Dark1 vs Light3/Light3/Light2, selected-text bg = base accent, legacy
  `SystemControl*Accent*` brushes) as the Windows insurance. **D12 refinement:**
  `TextOnAccentFillColor*` and `*Disabled` are excluded — their values are
  seed-invariant (S4 evidence). Single-entry cache keyed by seed avoids
  re-solving HCT on unrelated rebuilds. **Known platform caveat** (documented
  in `doc/seed-colors.md`): after an in-place seed CLEAR, XCR's already-
  materialized accent brushes can keep the last value until the next app-scope
  resource change; unmerging the theme restores fully (guarded by
  `Given_FluentSeedAccent.When_SeedCleared_PlatformAccentRestored`).
  S4(b) — whether a per-control resource redefinition alone wins
  `{ThemeResource}` lookups inside XCR templates — remains open for Phase 3.
- 2026-07-15 — **Phase 1 complete.** Library implemented per spec with three
  deviations, all recorded in `specs/lessons.md`:
  1. Semantic keys targeting bundle-local styles (TextButton/IconButton + 19
     typography aliases) cannot be XAML aliases — a `<StaticResource>` alias only
     resolves against app-level scope, so a container-scoped theme's aliases
     miss its own bundle. Resolved late-bound in code (`_bundleStyleAliases`).
  2. `Typography.xaml`/`Fonts.xaml` are Source-merged from `BaseDictionaries.xaml`
     instead of flowing into the XamlMerge bundle: bundle-level ThemeDictionaries
     resolve in Debug but NOT in Release (found by the CI-parity run; trimming
     ruled out). Follow-up: Simple has the same latent structure, untested.
  3. The generated semantic brushes (`PrimaryBrush`, …) materialize once against
     app-level scope; the brush value spot-checks run with FluentTheme merged at
     app scope (the documented consumer topology) inside try/finally.
  Ⓜ keys resolve via named key → type-keyed lookup → explicit empty style
  (keeps the built-in template; D8 nearest-match honored on Uno where neither
  public key nor type-keyed entry exists).
  **Verification:** full CI-parity suite (Release, headless script):
  227 cases — 226 passed / 1 skipped (pre-existing) / 0 failed (baseline was
  107/1/0). CI package audit: no pipeline changes needed (slnf-driven packaging,
  glob signing/publish, sln-driven canary).
- 2026-07-14 — Spec drafted; Spike S1 started (this session).
- 2026-07-14 — S1 complete, verdict GO: direct style aliases to XCR keys work (same-instance),
  `BasedOn` bridge styles work, FontFamily alias works; alias-of-alias does NOT resolve (→ D16)
  and per-theme-branch color aliases resolve the ambient theme (→ D6 mechanism C, reproducing
  the `specs/lessons.md` failure mode). S2/S3 Skia data captured in `spike-results.md`;
  spec §5.2 confidence column updated (9 keys need M-CODE type-keyed fallback on Skia).
  Spike tests kept as permanent mechanism guards; full runtime-test suite green.
