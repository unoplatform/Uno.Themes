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
- [ ] TextBox
- [ ] CheckBox
- [ ] RadioButton
- [ ] ToggleSwitch
- [ ] Slider

## Phase 4 — Samples

- [ ] Fluent showcase / head decision

## Review log

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
