# Theme rebuild cost (unoplatform/Uno.Themes#1786)

## Profile of one `BaseTheme.UpdateSource` pass

Release, `net10.0-desktop` (Skia), Windows x64, `SimpleTheme`, average of 30 after 5 warm-up runs.

| Stage | Time | Allocated |
|---|---|---|
| `new SimpleTheme()` total | ~7 ms | ~1185 KB |
| First `SemanticBrushUpdater.Apply` on a fresh theme | ~5.7 ms | ~990 KB |
| Spacing / shape / density generation, per pass | ~0.15 ms | 128 KB |
| Shared palette reload, per pass | ~0.005 ms | 3 KB |
| Second pass when `Colors` is assigned | ~0.5–0.9 ms | ~160 KB |

The issue's premise did not hold: the duplicate constructor pass and the palette reload are cheap.
The cost is per theme instance — materialising the 840 XAML-declared brushes of `SharedColors.xaml`
(280 × Default / Light / HighContrast), each resolving two `StaticResource` references whose
values `Apply` then overwrites.

## Plan

- [x] Profile one pass.
- [x] Reuse the generated token layers (spacing, shape + capped radii, density, typefaces) while
  their input is unchanged.
- [x] Build the brush dictionary in code (`SemanticBrushUpdater.CreateBrushes`) instead of loading
  `SharedColors.xaml`. The file stays: `MaterialColorsV2` still loads it.
- [x] Tests: `Given_DesignTokens.When_ColorsChange_Then_TokenLayersAreReused`,
  `When_ATokenInputChanges_Then_OnlyItsLayerIsRegenerated`;
  `Given_SeedColorPalette.When_ThemeIsBuilt_Then_ItsBrushKeysMatchSharedColorsXaml` (3 rows).
- [x] Re-measure.

Dropped, with reasons:

- Deferring the constructor pass: at most ~0.5 ms / ~30 KB once layers are reused, and a
  `ResourceDictionary` has no portable hook announcing its first read.
- Caching the shared palette: 3 KB per pass.
- A pass-count test: the pass count is unchanged; the layer-identity tests guard what matters.

## Results

| Scenario | Before | After |
|---|---|---|
| `new SimpleTheme()` | ~7.0–7.9 ms, 1185 KB | ~3.8 ms, 844 KB |
| `new SimpleTheme { Colors = seed }` | ~7.0 ms, 1345 KB | ~2.7–3.7 ms, 876 KB |
| Runtime seed change | ~0.85 ms, 158 KB | ~0.73 ms, 30 KB |
| Runtime radius change | ~0.71 ms, 142 KB | ~0.58 ms, 74 KB |

Timings vary by about ±1 ms between runs; the allocation figures are stable.

### WebAssembly and Android

Same harness run at app startup, Release, `master` against this branch. WebAssembly: published
`net10.0-browserwasm` in headless Chrome. Android: `net10.0-android`, x86_64 emulator (Pixel 8 Pro,
API 36).

| Scenario | WebAssembly before | WebAssembly after | Android before | Android after |
|---|---|---|---|---|
| `new SimpleTheme()` | ~30 ms, 793 KB | ~10 ms, 539 KB | ~17–24 ms, 1362 KB | ~9–11 ms, 858 KB |
| `new SimpleTheme { Colors = seed }` | ~32 ms, 914 KB | ~13 ms, 561 KB | ~10–21 ms, 1525 KB | ~2.5–10 ms, 890 KB |
| Runtime seed change | ~3.4 ms, 121 KB | ~3.2 ms, 21 KB | ~1 ms, 161 KB | ~0.8 ms, 31 KB |
| Runtime radius change | ~2.6 ms, 109 KB | ~2.5 ms, 57 KB | ~0.6–1.8 ms, 144 KB | ~0.6 ms, 76 KB |

The Android timings are noisy (emulator, JIT tiering) and are not device figures; its allocation
figures repeat exactly between rounds.

## Review

- Runtime tests: SimpleSampleApp 356 passed, 1 skipped (pre-existing `[Ignore]`);
  MaterialSampleApp 143 passed. Desktop only.
- Not verified: iOS and the WinAppSDK head (cannot run headless). The runtime tests were not run on
  WebAssembly or Android, only the measurement harness. The brush
  dictionary is now created in code on real WinUI too.
- Behaviour difference: a role `Apply` cannot resolve from any color layer used to keep its
  parse-time XAML value and now stays a blank brush. `SharedColorPalette.xaml` defines every role,
  so this is not reachable today.
- Not addressed: a URI-backed `Colors.OverrideDictionary` is still re-read on every pass (the font
  override is cached). Not measured.
