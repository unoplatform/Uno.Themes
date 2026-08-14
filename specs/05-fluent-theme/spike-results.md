# Fluent Theme — Spike Results

Companion to `spec.md`. Each spike's raw findings land here; the spec's
decision log references them.

## S1 — Cross-dictionary StaticResource alias resolution ✅ DONE (Skia desktop)

**Run:** 2026-07-14, macOS, SimpleSampleApp (CI host, `XamlControlsResources`
merged at app scope), Debug `net10.0-desktop`, headless
`--runtime-tests`, filter `Given_FluentAliasResolution`. Ambient theme: Dark.
Guard tests committed at
`src/samples/SimpleSampleApp/RuntimeTests/Given_FluentAliasResolution.cs` +
`RuntimeTests/FluentSpike/FluentAliasSpikeDictionary.xaml`; final state
**14/14 green**, full suite 107 passed / 1 skipped (pre-existing) / 0 failed.

| Case | Mechanism | Result |
|---|---|---|
| 1 | `<StaticResource x:Key ResourceKey="AccentButtonStyle"/>` style alias to an XCR key, from a dictionary loaded via `ms-appx` Source merged as a later sibling / lower scope | ✅ **Works** — resolves to the *same instance* as `Application.Current.Resources["AccentButtonStyle"]`; applying the alias renders identical accent visuals, distinct from `DefaultButtonStyle` |
| 2 | Alias of an alias (same dictionary) | ❌ **Fails** — `"Couldn't statically resolve resource SpikeFilledButtonStyle"` at parse; key yields no `Style`. → **spec D16: never chain aliases**; every semantic key targets a concrete key directly. (Alias → concrete style inside the same merged bundle is fine — Simple's `_Resources.xaml` → `Button.xaml` proves it in production.) |
| 3 | `BasedOn="{StaticResource DefaultButtonStyle}"` setters-only bridge style | ✅ **Works** — `BasedOn` references the XCR instance; transparent-background setter wins at rest |
| 4 | Per-theme-branch color alias (`ResourceKey="TextFillColorPrimary"` inside `ThemeDictionaries` Light/Default) | ❌ **Fails branch-correctness** — both branches resolved the **ambient** theme's value (`#FFFFFFFF`, dark, in a dark session). Exact `specs/lessons.md` failure mode. → **spec D6 decided: mechanism C** — palette built in code |
| 5 | FontFamily alias (`ResourceKey="ContentControlThemeFontFamily"`) | ✅ **Works** (`XamlAutoFontFamily`) |
| 6 | Core XCR keys presence (`AccentButtonStyle`, `DefaultButtonStyle`, `DefaultTextBoxStyle`, `DefaultCheckBoxStyle`, `DefaultContentDialogStyle`) | ✅ All present |

**Verdict:** the adapter architecture is viable on Uno Skia.
- **Style aliases:** mechanism A (XAML `<StaticResource>` aliases), direct-target only (D16).
- **Color palette:** mechanism C (code-resolved per-branch values) — mandatory, not optional.

**Residual:** Windows (WinAppSDK) and WASM not yet exercised — schedule in
Phase 1 CI/manual pass. Windows is the reference WinUI implementation where
case 1/3 semantics are specified behavior; case 4 will use mechanism C
everywhere regardless, so the residual risk concentrates on exotic key
availability (see S3).

## S2 — Concrete Fluent token values ⚙️ PARTIAL (Dark branch captured; Light pending)

Captured 2026-07-14 on Skia desktop (macOS), **Dark** theme, via the
`When_ProbingCandidateXcrKeys_ReportIsWritten` probe. Light-branch capture
requires a light-themed session (or walking XCR's ThemeDictionaries branches
directly) — scheduled with Phase 1. Note: `SystemAccentColor` on this host
resolved to `#FF0078D7` (legacy-default blue), not the WinUI-3-documented
`#0078D4` — one more reason values must be read at runtime (mechanism C), not
baked.

### Accent set (theme-invariant)

| Token | Value |
|---|---|
| `SystemAccentColor` | `#FF0078D7` |
| `SystemAccentColorLight1` | `#FF429CE3` |
| `SystemAccentColorLight2` | `#FF76B9ED` |
| `SystemAccentColorLight3` | `#FFA6D8FF` |
| `SystemAccentColorDark1` | `#FF005A9E` |
| `SystemAccentColorDark2` | `#FF004275` |
| `SystemAccentColorDark3` | `#FF002642` |

`AccentFillColorDefaultBrush` (Dark) = `SolidColorBrush(#FF76B9ED)` =
`SystemAccentColorLight2` → **confirms** the spec §6.3 dark-theme
`PrimaryColor ← SystemAccentColorLight2` assumption.

### Dark-branch neutrals & semantics

| Token | Value |
|---|---|
| `TextFillColorPrimary` | `#FFFFFFFF` |
| `TextFillColorSecondary` | `#C5FFFFFF` |
| `TextFillColorTertiary` | `#87FFFFFF` |
| `TextFillColorDisabled` | `#5DFFFFFF` |
| `TextFillColorInverse` | `#E4000000` |
| `TextOnAccentFillColorPrimary` | `#FF000000` |
| `TextOnAccentFillColorSecondary` | `#80000000` |
| `SolidBackgroundFillColorBase` | `#FF202020` |
| `SolidBackgroundFillColorSecondary` | `#FF1C1C1C` |
| `SolidBackgroundFillColorTertiary` | `#FF282828` |
| `SolidBackgroundFillColorQuarternary` | `#FF2C2C2C` |
| `CardBackgroundFillColorDefault` | `#0DFFFFFF` |
| `ControlFillColorDefault` | `#0FFFFFFF` |
| `ControlStrokeColorDefault` | `#12FFFFFF` |
| `ControlStrongStrokeColorDefault` | `#8BFFFFFF` |
| `DividerStrokeColorDefault` | `#15FFFFFF` |
| `SystemFillColorCritical` | `#FFFF99A4` |
| `SystemFillColorCriticalBackground` | `#FF442726` |
| `SystemFillColorSuccess` | `#FF6CCB5F` |
| `SystemFillColorCaution` | `#FFFCE100` |
| `ContentControlThemeFontFamily` | `XamlAutoFontFamily` |

### Missing tokens on Uno Skia (mapping impact)

| Token | Impact |
|---|---|
| `AccentTextFillColorPrimary/Secondary/Tertiary` | **absent** — Phase 2 closure (spec §9.2) must use `TextOnAccentFillColor*`; spec §6.3 already does |
| `SystemFillColorAttention` | absent — unused by the mapping |

## S3 — `Default*Style` key availability ⚙️ PARTIAL (Skia desktop done; Windows/WASM pending)

Same probe run. Spec §5.2 confidence column updated from this table.

### Present on Uno Skia ✅

`AccentButtonStyle`, `DefaultButtonStyle`, `DefaultToggleButtonStyle`,
`DefaultTextBoxStyle`, `DefaultPasswordBoxStyle`, `DefaultComboBoxStyle`,
`DefaultComboBoxItemStyle`, `DefaultCheckBoxStyle`, `DefaultRadioButtonStyle`,
`DefaultToggleSwitchStyle`, `DefaultSliderStyle`, `DefaultProgressBarStyle`,
`DefaultListViewItemStyle`, `DefaultContentDialogStyle`,
`DefaultAppBarButtonStyle`, `DefaultHyperlinkButtonStyle`,
`DefaultCalendarViewStyle`, `DefaultDatePickerStyle`,
`DefaultFlyoutPresenterStyle`, `DefaultMenuFlyoutPresenterStyle`,
`DefaultMenuFlyoutItemStyle`, `DefaultMenuFlyoutSubItemStyle`,
`DefaultToggleMenuFlyoutItemStyle`, `DefaultRadioMenuFlyoutItemStyle`,
`DefaultMediaTransportControlsStyle`, `DefaultAutoSuggestBoxStyle`,
`TextBlockButtonStyle`, and the full XCR text ramp
(`Caption/Body/BodyStrong/Subtitle/Title/TitleLarge/Display` TextBlock styles).

### MISSING on Uno Skia → M-CODE type-keyed lookup (or Windows-only alias)

| Key | Affected semantic keys |
|---|---|
| `DefaultProgressRingStyle` | `ProgressRingStyle` |
| `DefaultListViewStyle` | `ListViewStyle` |
| `DefaultCommandBarStyle` | `CommandBarStyle` |
| `DefaultCalendarDatePickerStyle` | `CalendarDatePickerStyle` |
| `DefaultTimePickerStyle` | *(not in the semantic set — informational)* |
| `DefaultPipsPagerStyle` | `PipsPagerStyle` |
| `DefaultRatingControlStyle` | `RatingControlStyle` |
| `DefaultMenuFlyoutSeparatorStyle` | `MenuFlyoutSeparatorStyle` |
| `DefaultNavigationViewStyle` / `DefaultNavigationViewItemStyle` | `NavigationViewStyle` / `NavigationViewItemStyle` |

Follow-up for Phase 1: verify whether the M-CODE fallback
(`Application.Current.Resources[typeof(ProgressRing)]` etc.) returns the
implicit style for each of these on Uno, and probe the same key list on
Windows + WASM.

## S4 — Accent/lightweight override cascade — (a) ✅ DONE (Skia desktop); (b) partially answered

**Run:** 2026-07-15, Linux, SimpleSampleApp (CI host), Debug `net10.0-desktop`,
headless, ambient theme **Light**, via a temporary `Given_TempS4Diag` probe
(not committed). Probed the accent key space at app scope and layered override
dictionaries as later siblings of XCR (FluentTheme's position), rendering an
`AccentButtonStyle` button after each layer.

### (a) Closure enumeration — Light branch (values relative to the shade set)

| Key | Light value | Relationship |
|---|---|---|
| `AccentFillColorDefaultBrush` | `#FF005A9E` @1 | = `SystemAccentColorDark1` |
| `AccentFillColorSecondaryBrush` | `#FF005A9E` @0.9 | = Dark1, **brush** opacity 0.9 |
| `AccentFillColorTertiaryBrush` | `#FF005A9E` @0.8 | = Dark1, brush opacity 0.8 |
| `AccentFillColorSelectedTextBackgroundBrush` | `#FF0078D7` | = `SystemAccentColor` |
| `AccentTextFillColorPrimaryBrush` | `#FF004275` | = `SystemAccentColorDark2` |
| `AccentTextFillColorSecondaryBrush` | `#FF002642` | = `SystemAccentColorDark3` |
| `AccentTextFillColorTertiaryBrush` | `#FF005A9E` | = `SystemAccentColorDark1` |
| `TextOnAccentFillColorPrimary/…` | white family | **seed-invariant** (not accent-derived) |
| `AccentFillColorDisabled(Brush)` | `#37000000` | seed-invariant neutral |
| `SystemControl*Accent*` + `SystemColorControlAccentBrush` + `SystemControlHyperlinkTextBrush` | `#FF0078D7` | = `SystemAccentColor` (legacy UWP-era set, present on Uno) |
| `AccentButtonBackground(+PointerOver/Pressed)` | Dark1 @1/0.9/0.8 | per-control copies of the fill set |
| `AccentFillColorDefault/Secondary/Tertiary` **colors** | MISSING on Uno Skia | only the brushes exist (colors exist on Windows) |
| `AccentTextFillColorPrimary/Secondary/Tertiary` **colors** | MISSING on Uno Skia | matches the S2 finding |

Dark-branch relationships (from S2 + WinUI structure): fill = `Light2`,
accent text = `Light3`/`Light3`/`Light2`. Windows capture still pending
(tracked residual risk).

### Cascade experiment (the load-bearing result)

| Layer merged (later sibling of XCR, per theme branch) | Rendered accent button |
|---|---|
| baseline | `#FF005A9E` (= Dark1) |
| **A: `SystemAccentColor*` shades only** | **`#FFB00020` (the override) — FULL CASCADE** |
| B: + `AccentFillColor*`/TextOnAccent colors+brushes | `#FFB00020` |
| C: + `AccentButtonBackground/Foreground` | `#FFB00020` |
| removed | `#FF005A9E` — clean restore |

**Conclusion (a):** on Uno, XCR's accent brushes re-resolve **late-bound**
against the ambient scope — overriding the `SystemAccentColor*` shades alone
recolors built-in controls; `AccentFillColorDefaultBrush` itself re-resolved to
the override. The D12 eager-resolution concern is a **Windows-only** problem.
Phase 2 therefore writes the shades (sufficient on Uno) **plus** the
accent-derived closure with values mirroring the platform structure above
(insurance for Windows), and skips `TextOnAccentFillColor*` / `*Disabled`
(seed-invariant — refines D12's closure list, recorded in the review log).

**Conclusion (b), partial (2026-07-15 morning):** redefining
`AccentButtonBackground` in a later app-scope sibling did not *break* rendering
(C) and a flat (non-theme-branch) redefinition also rendered correctly (B2) —
but because layer A already cascaded, C could not isolate whether the
per-control redefinition *itself* wins `{ThemeResource}` lookups from inside
XCR templates.

### (b) Isolation run ✅ DONE (2026-07-15, Skia desktop, ambient Light)

Temporary `Given_TempS4bDiag` probe (not committed), **no accent-shade
overrides anywhere** — pure per-control redefinitions, rendered
`AccentButtonStyle` + `DefaultButtonStyle` buttons after each step:

| Scenario | Result |
|---|---|
| Q1 — app-scope later sibling, per-branch `AccentButtonBackground`/`ButtonBackground` | ✅ both buttons follow the override; clean restore on removal |
| Q1b — same, flat (no ThemeDictionaries) | ✅ follows |
| Q2 — redefinition NESTED in an outer app-scope dictionary's MergedDictionaries (the FluentTheme/AddThemeDictionary topology), configured before merge | ✅ follows |
| Q3 — nested layer swapped while attached, then a NEW control rendered | ✅ new control shows the new value (no app-scope poke needed) |
| Q4/Q4b — container-scoped redefinition (per-branch and flat) | ✅ contained buttons follow |

**Verdict (b): YES on Uno** — per-control resource redefinitions in a later
sibling (or nested dynamic layer) win `{ThemeResource}` lookups made inside
XCR templates, at app and container scope, and `UpdateSource`-style layer
swaps propagate to controls rendered afterwards. Phase 3's re-pointing design
is confirmed; implemented override-driven (no re-pointing without an override,
so stock rendering and Windows live-accent tracking stay untouched).
Bonus capture: light-branch `ButtonBackground` = `#B3FFFFFF`
(= `ControlFillColorDefault` light). Windows validation remains pending
(tracked residual risk).

### Addendum — per-control key-name probe (2026-07-15, fresh XCR instance)

Probed the documented semantic key names and WinUI candidate targets against
`new XamlControlsResources()` (bypassing the host's app-level SimpleTheme):

| Control | Finding |
|---|---|
| TextBox | Full `TextControl*` family present (Background/Foreground/BorderBrush/Placeholder × PointerOver/Focused/Disabled, Header + Disabled, ButtonForeground + PointerOver/Pressed). Border brushes are gradients at rest. Material's `FilledTextBox*`/`OutlinedTextBox*` names absent → **map both families** (single Fluent TextBox; Outlined wins collisions) |
| CheckBox | All documented names natively present EXCEPT `CheckBoxGlyphForeground*` (WinUI: `CheckBoxCheckGlyphForeground*`) → **glyph family mapped, rest native** |
| RadioButton | `RadioButtonForeground`, `RadioButtonOuterEllipse{Stroke,Fill,CheckedStroke,CheckedFill}`, `RadioButtonCheckGlyphFill` all native → **nothing to bridge** |
| ToggleSwitch | WinUI names (`ToggleSwitchFillOff/On`, `StrokeOff/On`, `KnobFillOff/On` + PointerOver/Pressed/Disabled) present; Material's (`ToggleSwitchOffOuterBorderFill`, `ToggleSwitchKnobOffFill`, …) absent → **mapped** |
| Slider | `SliderTrackFill*`, `SliderTrackValueFill*`, `SliderThumbBackground*` (each + PointerOver/Pressed/Disabled), `SliderTickBarFill`, `SliderInlineTickBarFill` all native → **nothing to bridge** |

Native names are permanently guarded by
`Given_FluentLightweightStyling.When_NativeSemanticKey_IsProvidedByXcr`.
