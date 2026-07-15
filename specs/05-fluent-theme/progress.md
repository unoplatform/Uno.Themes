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

- [ ] Project scaffold (csproj, props, constants, `FluentTheme`)
- [ ] `_Resources.xaml` semantic aliases (post-S3 key set)
- [ ] `ColorPalette.xaml` (mechanism per S1 outcome)
- [ ] `Typography.xaml` + `TextBlock.xaml` + bridge Button styles
- [ ] Packaging: sln/slnf, InternalsVisibleTo, CI package lists
- [ ] Runtime tests: `Given_FluentSemanticStyles`, `Given_FluentColorPalette`, `Given_FluentTypography`; extend `Given_ColorOverridePrecedence`
- [ ] Docs: `fluent-getting-started.md`, `semantic-styles.md` Fluent column, `themes-overview.md`

## Phase 2 — Seed → accent

- [ ] S4(a) accent closure enumeration
- [ ] Closure override implementation + `Given_FluentSeedAccent`
- [ ] `doc/seed-colors.md`

## Phase 3 — Lightweight bridge (per control: Button → TextBox → CheckBox → RadioButton → ToggleSwitch → Slider)

- [ ] S4(b) ThemeResource re-pointing validation
- [ ] Button
- [ ] TextBox
- [ ] CheckBox
- [ ] RadioButton
- [ ] ToggleSwitch
- [ ] Slider

## Phase 4 — Samples

- [ ] Fluent showcase / head decision

## Review log

- 2026-07-14 — Spec drafted; Spike S1 started (this session).
- 2026-07-14 — S1 complete, verdict GO: direct style aliases to XCR keys work (same-instance),
  `BasedOn` bridge styles work, FontFamily alias works; alias-of-alias does NOT resolve (→ D16)
  and per-theme-branch color aliases resolve the ambient theme (→ D6 mechanism C, reproducing
  the `specs/lessons.md` failure mode). S2/S3 Skia data captured in `spike-results.md`;
  spec §5.2 confidence column updated (9 keys need M-CODE type-keyed fallback on Skia).
  Spike tests kept as permanent mechanism guards; full runtime-test suite green.
