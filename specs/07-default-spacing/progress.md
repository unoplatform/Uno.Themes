# DefaultSpacing token + Density-as-mode composition (issue #1688)

Implements https://github.com/unoplatform/Uno.Themes/issues/1688.

## Design

Revised after review: the first cut treated `DefaultSpacing` and `DefaultDensity` as two
competing sources of the base unit (explicit spacing wins, density preset as fallback,
`Compact = 3` / `Regular = 4` / `Comfy = 5` baked into the enum values). That collapsed two
orthogonal axes into one knob — a branded base unit silently discarded the density axis.
See `specs/lessons.md` ("A design-token 'mode' (density) must be a factor…").

Current model — spacing and density **compose**:

- `DefaultSpacing` (double DP on `BaseTheme`, default **4**) is the base spacing unit.
  Non-finite or negative consumer values degrade to the default base of 4 (the value is
  consumed from property-changed callbacks and must not poison the `Space*` scale).
- `Density` is a pure **mode** (`Compact`, `Regular`, `Comfy` — no meaningful underlying
  values). It scales the base unit: Compact ×0.75, Regular ×1, Comfy ×1.25. Undefined enum
  values degrade to ×1.
- Effective base = `DefaultSpacing × density factor`. Factors are chosen so the default
  base of 4 reproduces the historical presets exactly (3 / 4 / 5) — density-only consumers
  see no change.
- `FixedDensityDefaults` (`ControlHeight*`, `IconSize*`, `TouchTargetMinSize`) stay
  invariant across both axes (unchanged).
- Breaking change (8.0): `Density` enum underlying values changed from 3/4/5 to 0/1/2.
  Anyone persisting `(int)Density` or casting the enum to a number is affected; the names
  and the rendered output at defaults are unchanged.

## Checklist

- [x] `DefaultSpacing` DP on `BaseTheme` (default 4) with XML docs (construction-time remark,
      composition with density)
- [x] `Density` enum reduced to a mode (no base-unit values); XML docs state the factors
- [x] `UpdateSource()`: effective base = sanitized `DefaultSpacing` × density factor
- [x] Runtime tests in `SimpleSampleApp/RuntimeTests/Given_DesignTokens.cs`:
  - [x] `DefaultSpacing` set (Regular) → `Space*` tokens are multiples of it
  - [x] Composition: spacing × Compact/Regular/Comfy → scaled base (4.5 / 6 / 7.5 for base 6)
  - [x] Runtime switch: assign `DefaultSpacing`, then set NaN → default base returns, mode kept
  - [x] Thickness companions derive from `DefaultSpacing`
  - [x] Edge: NaN / negative / infinity → default base (4) × mode
  - [x] Shape tokens unaffected by `DefaultSpacing`; fixed tokens invariant
  - [x] Pre-existing density-only tests (Simple + Material heads) unchanged — outputs
        identical by construction
- [x] `doc/design-tokens.md`: composition documented ("Density Modes" section, factor table)
- [x] `doc/material-getting-started.md` + `doc/material-migration.md` updated
- [x] Build clean (desktop TFM) + `Given_DesignTokens` runtime tests green headlessly

## Review

Implemented as designed. `Enum.IsDefined` fallback replaced by the factor `switch` default
(same graceful degradation, one less reflection call). No new resource keys, no Markup
helpers needed.

Verification (Simple host, net10.0-desktop Debug, headless `--runtime-tests` run, 2026-08-19):
- All 18 DefaultSpacing/composition cases passed (value rows, composition matrix
  Compact/Regular/Comfy, invalid-value fallbacks incl. NaN, runtime set/clear, thickness
  companions, shape independence, fixed-token invariance).
- Pre-existing density-only tests unchanged and passing — outputs identical by
  construction (default base 4 × factors = 3/4/5). Material head run separately
  (`Given_DesignTokens` filter): 32 cases, 0 failed.
- Full suite: 171 cases — 0 failed, 1 skipped (pre-existing `[Ignore]`d hot-reload leak
  guard, unrelated).
- Build clean: no new warnings in `BaseTheme.cs` / `Given_DesignTokens.cs` (only the two
  pre-existing CS0618 obsolete-member warnings in `BaseTheme.cs`).

## Review addendum (2026-09-03) — the seam now reaches realized controls

`doc/design-tokens.md` had recorded the three scale measures as construction-time: "assigning them
later does regenerate the tokens but does not restyle controls". That was a defect, not a property
of the design. The generated `Space*` / `Radius*` keys were fine; the control styles read them —
directly or through per-control aliases such as `SimpleButtonCornerRadius` and
`MaterialContentDialogPanelPadding` — with `{StaticResource}`, which snapshots at parse time.

- **Sweep.** Alias closure over every `<StaticResource x:Key ResourceKey>` declaration rooted at a
  generated key, then every `{StaticResource K}` consumer of that closure, across Simple and
  Material v2: 98 sites in 22 files (78 Simple, 20 Material v2), all rewritten to
  `{ThemeResource}`. Density constants (`ControlHeight*`, `IconSize*`, `TouchTargetMinSize`) are
  regenerated with fixed values and were left as they are.
- **Red / green.** `Given_DesignTokens.When_ScaleMeasuresChangeAtRuntime_Then_RealizedButtonFollows`
  (Simple head): a realized `SimpleFilledButtonStyle` button kept `Padding` 12 / `CornerRadius` 8
  after `DefaultSpacing` = 10, `DefaultDensity` = Compact, `DefaultCornerRadius` = 1 — expected
  22.5 / 2 — and a button created after the change is asserted too.
  `Given_DesignTokens.When_ScaleMeasuresChangeAtRuntime_Then_RealizedContentDialogFollows`
  (Material head): a realized `MaterialContentDialogStyle` dialog kept `CornerRadius` 28 — expected
  7. Both red before the rewrite, green after. Both mutate the application theme (the scope a
  designer edits) and restore it in `finally`; realized content re-resolves on a `RequestedTheme`
  flip, the same route `FontFamilyTunerControl` uses.
- **Suites.** Simple head 256 / 0 failed; Material head 63 / 0 failed (net10.0-desktop Debug,
  headless `--runtime-tests`). Build clean, no new warnings.
- **Docs.** `doc/design-tokens.md`: the scale measures are documented as runtime-settable with the
  already-realized caveat and the theme-change-pass route, matching the `DefaultFontFamily` wording.
