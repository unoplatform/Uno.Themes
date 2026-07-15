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

## S4 — Accent/lightweight override cascade

Not started (scheduled before Phase 2/3 — see spec §14.4).
