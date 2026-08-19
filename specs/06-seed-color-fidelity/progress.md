# 06 — Seed color fidelity: exact-seed Primary, working HCT solver, complete content mode

Status: **implemented.** Items 1-5 are done and verified; item 6 is done except for the review panel, and
item 7 (release) is the only open work — see [Review](#review). This document keeps the original
investigation intact so the reasoning behind each decision stays available.
All five decisions (D1–D5) are settled. **Ships as 8.0** — see [Release mechanics](#release-mechanics-for-d5).
Branch: `dev/sb/seed-color-fidelity`. Tracking issue: [unoplatform/Uno.Themes#1700](https://github.com/unoplatform/Uno.Themes/issues/1700).
Supersedes nothing; extends [`specs/03-seed-color-palette/seed-color-palette.md`](../03-seed-color-palette/seed-color-palette.md) (the original feature, shipped in 7.0.3).

## Why this exists

A user comparing Uno Themes' seed generation against a design tool's "single seed color → 22 colors" view
asked whether we genuinely derive the whole palette from one seed. We do (see Finding 1), but investigating
it turned up three defects, one of them severe (Finding 4). The user then asked for a behavior change:

> "I'd like it better if PrimaryColor was the actual hex value the user gives as the seed and then we
> generate based on that"

That change is item 3 below, but it is **gated on the solver fix** — pinning Primary to the seed while the
surrounding ramp is desaturated by a broken solver produces an accurate swatch inside a muddy palette, which
is worse than the status quo.

---

## Verified findings

Every number below was produced by running the repo's own `ColorGeneration` code standalone. See
[Reproduction harness](#reproduction-harness) at the bottom — rebuild it first, before changing anything, so
you have a baseline to diff against.

### 1. Single-seed derivation genuinely works (no bug here)

`SeedColorPaletteGenerator` emits **28 of the 32** color keys that `SharedColors.xaml` consumes, plus
`ShadowColor`. The only omissions are the four Error keys, which is deliberate and spec-correct (Finding 3).
Mechanism (`SeedColorPaletteGenerator.cs:66-100`): the seed is converted to HCT and **only the hue survives**
— tone is discarded entirely, chroma is used for Primary only, and only in fidelity mode.

| Palette | Hue | Chroma | For seed `#006495` (H 244.0 / C 42.7 / T 40.1) |
| --- | --- | --- | --- |
| Primary | `hue` | `max(chroma, 48)` | H 244, C 48 |
| Secondary | `hue` | **16** | H 244, C 16 |
| Tertiary | `hue + 60` | **24** | H 304, C 24 |
| Neutral | `hue` | **4** | H 244, C 4 |
| NeutralVariant | `hue` | **8** | H 244, C 8 |

These constants are verbatim from `CorePalette.of(argb, isContent: false)` in material-color-utilities. The
role→tone table at `SeedColorPaletteGenerator.cs:111-148` was checked against the M3 spec and is correct,
with one intentional repo deviation: **dark `SurfaceColor` uses neutral T20, not M3's T10**, matching the
existing default palette where dark Surface (`#302D37`) sits above Background (`#1C1B1F`). Leave it.

Incidental note for whoever revisits the original question: the design-tool screenshot that prompted this was
**not** showing single-seed output. Its Secondary was hue 319 and its Tertiary hue 359 (the M3 baseline rose
`#7D5260`/`#EFB8C8`), against a seed at hue 244. A true single-seed derivation must put Secondary at hue 244
and Tertiary at hue 304. That deviation is in the other tool, not in this repo.

### 2. Content / "high fidelity" mode is half-implemented

`BaseTheme.cs:321` declares `protected virtual bool UseHighFidelityColors => false`.
`SimpleTheme.cs:30` overrides it to `true`. Its entire effect is one line
(`SeedColorPaletteGenerator.cs:74`):

```csharp
double primaryChroma = useHighFidelity ? chroma : Math.Max(chroma, 48);
```

Secondary/Tertiary/Neutral/NeutralVariant keep the **non-content** constants (16 / 24 / 4 / 8).
`CorePalette.of(argb, isContent: true)` requires `chroma/3`, `chroma/2`, `min(chroma/12, 4)`,
`min(chroma/6, 8)` respectively.

The code comment claims fidelity mode is "needed for neutral/gray seeds". Measured with `#808080` (C 1.9):

| Role (light T40) | We emit | Content mode should emit |
| --- | --- | --- |
| Secondary | `#4B6367` (teal) | `#5F5E5E` (gray) |
| Tertiary | `#525D7D` (blue) | `#5F5E5E` (gray) |

So a gray-seeded `SimpleTheme` gets a colored secondary and tertiary today.

### 3. Error is ignored and always red — correctly, but document it

`SeedColorPaletteGenerator.cs:102-104` skips Error deliberately; the four keys fall through to the base
layer. M3 agrees — `CorePalette` pins Error to hue 25 / chroma 84 regardless of seed. **Do not "fix" this by
generating Error.** Two things do need attention:

- **Which** red you get depends on the theme. Shared/Material gives `#B3261E` / `#FFB4AB` (the correct M3
  baseline ramp). `Uno.Simple.WinUI/Styles/Application/ColorPalette.xaml` overrides `ErrorColor` to
  `#EC221F` (H 25.8 / C 99.0 / T 50.8 — a different tone, not just a different red).
- **Doc drift**, two places, both claiming Error is generated:
  - `doc/seed-colors.md`: "will generate: Primary, Secondary, Tertiary, **Error** tonal palettes"
  - `specs/03-seed-color-palette/seed-color-palette.md:9` says the same, while line 133 of the same file
    correctly says Error is *not* generated.

### 4. `HctSolver` clamps chroma far short of the sRGB gamut — this is the severe one

> **Correction (see [Review](#review)):** the expected value here is `#BA1A1A`, not `#B3261E`. `#B3261E` is M3's
> published baseline *swatch*, which is not the tone-40 entry of a chroma-84 palette. The diagnosis below is
> otherwise accurate; the root cause turned out to be the gray-derived `J`, not the bisection itself.

Asked for M3's own error palette (hue 25 / chroma 84 / tone 40), which must be `#B3261E`, the solver returns
**`#834E47`** — a muddy brown. The bisection in `HctSolver.BisectChroma` gives up well before the gamut
boundary:

| Hue | Requested chroma | Chroma actually delivered |
| --- | --- | --- |
| 25 (red) | 84 | **27** |
| 244 (blue) | 84 | **43** |
| 140 (green) | 84 | **41** |

Loss begins around chroma 36-48 — i.e. across the range where most brand colors live, not at "extreme gamut
boundaries" as `specs/03-seed-color-palette/seed-color-palette.md:230` currently claims. Round-trips:

```
pure red     #FF0000 -> #AA6D63    worst channel off by 109
pure green   #00FF00 -> #BBECAA    worst channel off by 187
pure blue    #0000FF -> #2E36BB    worst channel off by  68
M3 error     #B3261E -> #7E4F48    worst channel off by  53
M3 purple    #6750A4 -> #635293    worst channel off by  17
seed #006495 #006495 -> #036596    worst channel off by   3
```

**Why the test suite is green:** `Given_SeedColorPalette.When_RoundTripping_Argb_Through_Hct_Then_ColorIsPreserved`
has a ±20 per-channel tolerance and five `DataRow`s — black, white, mid-gray, `#6750A4`, `#386A20`. The first
three cannot fail (chroma ~0), and `#6750A4` lands at 17 against a tolerance of 20. Not one saturated color is
tested. Fixing the solver **requires** adding those rows first (red/fix/green per AGENTS.md §5).

The fix is to replace the bisection with the analytical gamut-boundary intersection from
material-color-utilities' `HctSolver.solveToInt` (linear-RGB plane intersection + `bisectToLimit`), rather
than tuning the existing bisection's iteration count.

### 5. Seed generation reaches Material v2 and Simple only

- **Cupertino has no seed support at all.** There is no `CupertinoTheme : BaseTheme` — `CupertinoResources`
  is a plain `ResourceDictionary`, and its 37 keys (`CupertinoBlueColor`, `LabelColor`,
  `SystemBackgroundColor`, …) are a separate namespace the generator never touches.
- **Material v1's 22 `Material*Color` keys** are likewise a separate legacy namespace. Not reachable by seed.
- A handful of colors are hardcoded inside control styles and immune to any seed: `InfoBar.xaml:10-11`
  (severity colors), `MediaPlayerElement.xaml:13,18,42,47`, `Slider.xaml:11,64`
  (`MaterialSliderThumbDisabledColor`), `FloatingActionButton.xaml:15,20`.

**Out of scope for this spec** — recorded so nobody re-discovers it. If Cupertino seed support is wanted,
that is its own spec.

### 6. Seed generation is opt-in

`MaterialTheme.cs:29` and `SimpleTheme.cs:27` both declare `DefaultPrimarySeed => null` (commit `09187371`,
"fix: make seed generation opt-in"). Nothing is generated unless a consumer sets `Colors.PrimarySeed`. This
materially limits the blast radius of every change below and is the basis for decision D1.

---

## Decisions already made

| # | Decision | Rationale |
| --- | --- | --- |
| **D1** | Exact-seed Primary **replaces** the current behavior rather than becoming a separate opt-in mode. | Seed generation is already opt-in (Finding 6), so only consumers who explicitly set `PrimarySeed` see any change — and this is what they expected. Avoids a second code path to keep tested. |
| **D2** | `OnPrimary` **auto-flips** between tone 100 and tone 10, whichever clears contrast against the pinned seed. Containers stay at their M3 tones. | User's explicit choice. Keeps the promise literal (Primary is *always* the seed hex) while staying legible when the seed's tone is far from 40. |
| **D3** | Only **light** `PrimaryColor` is pinned to the seed. Dark stays derived at T80. | A dark brand color pinned onto a dark surface is unreadable; pinning both is not a coherent reading of the request. |
| **D4** | Reuse the existing `UseHighFidelityColors` flag rather than adding a new one — but **promote it to a public DP on `ThemeColors`** and keep the `BaseTheme` member as an `[Obsolete]` forwarder. | Preserving the seed's chroma *is* the prerequisite for pinning Primary, so the concepts are the same knob. It shipped in 7.0.3 as `protected virtual` on the public `BaseTheme`, so external subclassers may have overridden it — deleting it outright is a breaking change (AGENTS.md §6). |
| **D5** | **Ship the whole of this spec as a major version bump — 8.0.** No opt-out flag, no bug-compatible code path. | Correcting `HctSolver` changes *every* generated color for anyone already using `PrimarySeed` on 7.0.3 (the hue-25 case moves from `#834E47` to `#B3261E`). It is a fix rather than a regression, but consumers get it without touching their code, and combined with exact-seed Primary (D1) the palette output changes materially. A major bump is the honest signal, and it avoids keeping the broken bisection alive as a second path to test forever. |

### Release mechanics for D5

`version.json` is Nerdbank.GitVersioning, currently `7.2-dev.{height}`; this branch moves it to
`8.0-dev.{height}`. Three things to get right:

- ⚠️ **Do not merge `version.json` to `master` ahead of the implementation.** This branch starts as spec-only.
  If the version bump lands on `master` before the breaking change exists, every `master` dev build reports
  8.0 with nothing in it to justify the major. Either hold the whole branch until the work is done, or drop
  the `version.json` hunk from any early merge and re-apply it with item 1.
- ⚠️ **The bump is normally automated.** `version.json` is bumped by `unodevops` after a release-branch cut
  (see `aa91e6f3`, `157ff71f`). A hand-edited version on a feature branch can conflict with that process —
  confirm with whoever owns releases before merging, rather than assuming the manual edit is the whole story.
- **Commit messages must carry the breaking marker.** Conventional Commits is enforced
  (`.github/workflows/conventional-commits.yml`). The solver and Primary-pinning commits need `feat!:` /
  `fix!:` or a `BREAKING CHANGE:` footer, or the release tooling will not read them as major.

### Sequencing (previously open, now settled)

Solver first, then content mode, then Primary pinning — the order in Next steps below. Pinning Primary while
the surrounding ramp is still desaturated yields one accurate swatch inside a muddy palette, which is worse
than today's consistent-but-wrong output.

---

## Next steps

Work in this order. Items 1 and 2 are prerequisites for 3 — do not reorder.

### 1. Fix `HctSolver` chroma clamping

- [x] **Red first.** Extend `Given_SeedColorPalette.When_RoundTripping_Argb_Through_Hct_Then_ColorIsPreserved`
      with saturated `DataRow`s: `#FF0000`, `#00FF00`, `#0000FF`, `#B3261E`. Confirm they fail at the current
      ±20 tolerance (they fail by 109 / 187 / 68 / 53 — see Finding 4).
- [x] Add a test asserting `new TonalPalette(25, 84).GetArgb(40)` is within a few units of `#B3261E` — this is
      the single clearest expression of the bug, and it is M3's own published value.
- [x] Replace `HctSolver.BisectChroma` with the analytical approach from material-color-utilities
      `HctSolver.solveToInt` (linear-RGB plane intersection, then `bisectToLimit` for the boundary). Keep the
      existing early-outs at `HctSolver.cs:24-34` (tone <0.0001 → black, >99.9999 → white, chroma <0.5 → gray)
      — those are correct and cheap.
- [x] **Green.** Then tighten the ±20 tolerance to what the corrected solver actually holds (expect ≤3) so the
      test stops being able to pass for the wrong reason.
- [x] Watch allocations — this runs in `TonalPalette.GetArgb`, called ~60× per palette generation, and
      per AGENTS.md §2 the HCT math is a value-type-discipline hot path. `TonalPalette` already memoizes per
      tone (`TonalPalette.cs:19,45-50`); keep that.

### 2. Complete content / fidelity mode

- [x] In `GenerateCore`, apply the content-variant chromas when `useHighFidelity` is true:
      secondary `chroma/3`, tertiary `chroma/2`, neutral `min(chroma/12, 4)`, neutralVariant `min(chroma/6, 8)`.
      Non-fidelity mode keeps 16 / 24 / 4 / 8 unchanged.
- [x] Explicit seeds still win: `SecondarySeed` / `TertiarySeed`, when set, continue to use that color's own
      hue **and** chroma (`SeedColorPaletteGenerator.cs:78-97`). Do not route those through the content math.
- [x] Runtime test: gray seed `#808080` in fidelity mode must produce a near-gray Secondary and Tertiary
      (chroma < 5), not today's `#4B6367` / `#525D7D`.
- [x] Fix the now-stale comment at `SeedColorPaletteGenerator.cs:71-73`.

### 3. Pin `PrimaryColor` to the seed (the actual request)

- [x] Light `PrimaryColor` = the seed ARGB verbatim (D3). Dark stays `primary.GetArgb(80)`.
- [x] `OnPrimaryColor` (light) picks tone 100 or tone 10 by contrast against the pinned seed (D2). Reuse the
      L*-based contrast math already present in the test file's `ColorMathAccessor` — but move it into
      `ColorMath` as a proper internal API rather than duplicating it (AGENTS.md §1, no duplication).
      **Partially done, deliberately.** `ColorMath.ContrastRatio` exists and the product uses it, and the
      test's duplicated L*/Y math is gone (it now reads tone through the public `HctColor.FromArgb().Tone`).
      The contrast formula itself is still restated in the test: the sample app has no `InternalsVisibleTo`,
      and — more importantly — asserting the product's colour choices with the product's own arithmetic
      would make those assertions self-referential and unable to fail. It is an oracle, not a duplicate,
      and is commented as such.
- [x] Pinning only makes sense with the seed's own chroma preserved, so this implies fidelity mode. Decide and
      document: does pinning force `UseHighFidelityColors`, or is `max(chroma, 48)` still applied to the rest
      of the ramp while Primary alone is exact? **Recommend forcing fidelity** — a chroma-48 ramp around a
      chroma-42 pinned swatch has a visible discontinuity at the Primary/PrimaryContainer boundary.
- [x] Runtime tests: (a) generated light `PrimaryColor` equals the seed exactly, for several seeds across the
      tone range; (b) `OnPrimary`/`Primary` contrast ≥ 4.5:1 for a pale seed (~T75), a mid seed (~T40) and a
      dark seed (~T20); (c) `#006495` still round-trips to itself.

### 4. Promote the flag to public API (D4)

- [x] Add a DP on `ThemeColors` — suggested name `PreserveSeedColor` (`bool`, default follows D1/D3 outcome).
      XML docs required (AGENTS.md §6).
- [x] `BaseTheme.UseHighFidelityColors` becomes `[Obsolete("Use ThemeColors.PreserveSeedColor instead.")]` and
      forwards; do not delete it (shipped in 7.0.3).
- [x] `BaseTheme.UpdateSource` (`BaseTheme.cs:405-413`) resolves the effective value: `Colors?.PreserveSeedColor`
      falling back to the obsolete virtual, mirroring how `PrimarySeed` already falls back to `DefaultPrimarySeed`.
- [x] `SimpleTheme.cs:30` moves to the new mechanism.
- [x] Changing a `ThemeColors` DP must trigger a rebuild — verify it flows through
      `ThemeColors.OnPropertyChanged` → `SetChangedCallback` → `BaseTheme.UpdateSource`, and that it is treated
      as a **non**-structural change (`isStructural` is currently true only for `OverrideDictionaryProperty`,
      `ThemeColors.cs:137`).

### 5. Documentation

- [x] `doc/seed-colors.md`: remove Error from the generated list; document exact-seed Primary, the
      `OnPrimary` contrast flip, and the new `PreserveSeedColor` property.
- [x] `specs/03-seed-color-palette/seed-color-palette.md`: fix line 9 (Error), and line 230's
      "extreme gamut boundaries" characterization once the solver is fixed.
- [x] Cross-link `doc/seed-colors.md` ↔ `doc/material-colors.md` (AGENTS.md §13).
- [x] Consider a sample page under `src/samples/SamplesApp.Shared/Content/` with a live seed picker so the
      pinning behavior is visible in all three heads.

### 6. Verification (AGENTS.md §4, §5 — none of this is optional)

- [x] Runtime tests pass headless:
      `dotnet build src/samples/SimpleSampleApp/SimpleSampleApp.csproj -c Release -f net10.0-desktop`
      then `build/scripts/linux-skia-desktop-runtime-tests.sh` (or the sample DLL with
      `--runtime-tests=<results.xml>`). See the `/uno-themes-runtime-tests` skill for filter syntax.
- [x] Build clean, no new warnings, for at least desktop + wasm.
- [x] Diff generated output against `master` for a spread of seeds and eyeball it — the harness below prints a
      full palette in one command. Attach a before/after table to the PR.
- [ ] Run the seven-lens review panel (`/review-panel`) before opening the PR; `contract` and `quality` are the
      load-bearing lenses here (public API change + shipped-behavior change).

### 7. Release (D5)

- [x] `version.json` moved from `7.2-dev.{height}` to `8.0-dev.{height}` on this branch.
- [ ] Confirm the manual bump with whoever owns the release process before merging — it is normally automated
      after a release-branch cut (`aa91e6f3`, `157ff71f`).
- [x] Mark the solver and Primary-pinning commits `feat!:` / `fix!:` or add a `BREAKING CHANGE:` footer.
      Done: the solver/pinning commit carries `feat!:` plus a `BREAKING CHANGE:` footer, and the
      HighContrast-sweep and SeedColorMode commits carry `fix!:` / `feat!:` respectively.
- [ ] Release note with a before/after color table for at least one saturated seed, so consumers can see what
      changed in their app before they upgrade. The harness below generates it in one command.

---

## Review

Implemented on `dev/sb/seed-color-fidelity` in the order the plan prescribed. Everything below was measured,
not assumed.

### What changed

| File | Change |
| --- | --- |
| `ColorGeneration/Hct/Cam16.cs` | `ToArgb` overloads and `GrayArgbFromJ` replaced by a single `ToLinrgb` returning **unclamped** linear RGB. The gamut test moves to the caller, where it can be exact. |
| `ColorGeneration/Hct/HctSolver.cs` | The gray-derived `J` is replaced by `TrySolveExact` — Newton iteration on `J` against the target luminance, ported from material-color-utilities' `findResultByJ`. Chroma bisection is retained only as the out-of-gamut fallback, and now uses `TrySolveExact` as its in-gamut predicate. |
| `ColorGeneration/ColorMath.cs` | Added `YFromArgb`, `ArgbFromLinrgb`, `ContrastRatio`. `LstarFromArgb` now routes through `YFromArgb`, dropping a `double[]` allocation per call. |
| `ColorGeneration/SeedColorPaletteGenerator.cs` | Content-variant chromas for Secondary/Tertiary/Neutral/NeutralVariant; light `PrimaryColor` and `SurfaceTintColor` pinned to the seed; `OnPrimaryColor` picked by contrast. Flag renamed `useHighFidelity` to `preserveSeedColor`, default `true`. |
| `ThemeColors.cs` | New public `PreserveSeedColor` DP (default `true`) plus internal `HasExplicitPreserveSeedColor`. |
| `BaseTheme.cs` | `UseHighFidelityColors` marked `[Obsolete]`, default flipped to `true`; `UpdateSource` prefers an explicitly-assigned `Colors.PreserveSeedColor` and falls back to the obsolete virtual. |
| `SimpleTheme.cs` | Its `UseHighFidelityColors` override removed — the new default already does this. |
| `SemanticThemeHelper.cs` | `PreserveSeedColor` property, for parity with the seed properties. |
| `SeedColorSamplePage.xaml(.cs)` | Live "Preserve seed color" toggle; the generated XAML snippet reflects it. |
| `ThemesConstants.cs` | `ThemeDictionaryKeys`, `SemanticColorKeys` (32 roles) and `BrushStateSuffixes` (9 states). |
| `Helpers/SemanticBrushUpdater.cs` (new) | Rewrites the `Color` of the existing semantic `SolidColorBrush` instances from the resolved colour layers. |

### The root cause of Finding 4 was not the bisection

The bisection was a symptom. `SolveToArgb` took `J` from **a gray at the target tone**, then required the
result's L* to land within 1.0 of that tone. In CAM16 the `J` that produces a given L* depends on the chroma, so
for any saturated color the first candidate missed the tone badly, the accept-test failed, and the bisection
dutifully cut chroma until the tone error closed — i.e. until the color was nearly gray. Nothing about the sRGB
gamut was involved. Solving for `J` instead of assuming it makes the requested chroma land exactly whenever it is
achievable, and reduces the bisection to what its name always claimed: an out-of-gamut fallback.

### Deviation 1: the oracle is `#BA1A1A`, not `#B3261E`

The plan asserted `new TonalPalette(25, 84).GetArgb(40)` "must be `#B3261E`". It must not. `#B3261E` is the M3
*published baseline swatch* and sits at HCT(26.0, **76.3**, 39.7) — it is not the tone-40 entry of a chroma-84
palette. material-color-utilities' own `SchemeTest` asserts `0xffba1a1a`, and the corrected solver reproduces
the entire published ramp to the byte:

| Tone | 10 | 20 | 30 | 40 | 80 | 90 |
| --- | --- | --- | --- | --- | --- | --- |
| Expected (MCU) | `#410002` | `#690005` | `#93000A` | `#BA1A1A` | `#FFB4AB` | `#FFDAD6` |
| Produced | `#410002` | `#690005` | `#93000A` | `#BA1A1A` | `#FFB4AB` | `#FFDAD6` |

Six independent published values matching exactly is far stronger evidence than the single value the plan named,
so `When_GeneratingM3ErrorPalette_Then_ToneMatchesReferenceImplementation` pins all six.

### Deviation 2: `OnPrimary` needs tone 0, not just 100 and 10

D2 specified a flip between tone 100 and tone 10. Measured, that pairing **cannot** reach WCAG AA for seeds
around L* 49-55: at L* 50 the better of the two is 4.48:1 (white) against 3.84:1 (tone 10). Adding tone 0 as a
third candidate closes the gap — a sweep over every hue x tone x chroma combination puts the worst case at
**4.617:1** (hue 111, tone 49, `#797900`) — the true analytic bound is **4.5826:1**, at the black/white
crossover where Y = 0.17913; the sweep's figure was an artefact of its grid step. Either way it clears AA.
A knowingly sub-AA default pairing is not acceptable, so `PickOnColor`
takes the best of {tone 100, tone 10, tone 0}. For ordinary seeds this is indistinguishable from D2 — a T40 seed
still gets tone 100, a T80 seed still gets tone 10.

### Deviation 3: `PreserveSeedColor` defaults to `true`, and pinning is tied to it

D1 said exact-seed Primary replaces the old behavior; item 3 recommended that pinning force fidelity mode. Taken
together those make the two concepts one knob:

- `PreserveSeedColor = true` (default): light Primary pinned to the seed, contrast-picked OnPrimary, every
  supporting palette scaled from the seed's own chroma.
- `PreserveSeedColor = false`: the pre-8.0 tonal-spot path — Primary derived at tone 40 with the chroma-48 floor,
  fixed 16/24/4/8 supporting chromas, `OnPrimary` at tone 100. No pinning, because a pinned swatch inside a
  chroma-48 ramp is exactly the discontinuity the plan warned about.

`SimpleTheme`'s override is therefore redundant and was removed rather than migrated.
`BaseTheme.UseHighFidelityColors` survives as an `[Obsolete]` virtual whose default flipped to `true`, and it is
consulted only when the consumer has **not** assigned `Colors.PreserveSeedColor` (detected via `ReadLocalValue`).
An external 7.0.3 subclass that overrode it to `false` keeps its behavior.

### Discovered mid-implementation: seed colors never reached the `*Brush` resources

The first cut of the runtime tests asserted on a rendered `Button`'s `Background`. Every case failed with
`#2C2C2C` — Simple's hand-crafted grayscale primary — for pinned and unpinned seeds alike. Cause:
`SharedColors.xaml` declares brushes as `<SolidColorBrush Color="{StaticResource PrimaryColor}" />`, which
resolves **eagerly at parse time** against the ambient scope (the app-level theme), before `UpdateSource` merges
the seed palette into that same dictionary. A seed therefore changes the `*Color` resources while the `*Brush`
resources keep the base palette's values. This is the same eager-`StaticResource` mechanism already recorded in
`specs/lessons.md` for `Fonts.xaml`, and `Given_ColorOverridePrecedence`'s class comment documents the
consequence for overrides ("To override the rendered brush, include `PrimaryBrush` in the override dict")
without connecting it to seeds.

This is **pre-existing on `master` and not caused by anything here**, so it is left alone — but it means the
user-visible payoff of this spec currently reaches only consumers who bind to `*Color` resources. It deserves its
own spec before 8.0 ships. The tests consequently read the generated palette rather than a rendered brush, and
the helper documents why.

### Follow-up (in this branch): the `*Brush` gap is fixed, not deferred

The section above was written when the brush gap was still out of scope. It has since been fixed here,
because without it the seed algorithm has no visible effect — the sample page's live color picker updated
`PrimaryColor` while every swatch and control kept painting the previous palette.

**Measured, in this order:**

1. `PrimaryColor` updates correctly on every seed change; `PrimaryBrush` never moves off the base palette.
2. Re-ordering the merge does not help. Four arrangements were probed (`Source` first, `Source` last, brushes
   merged last, and app-scope primed); only priming `Application.Current.Resources` before the parse made the
   brush see the seed. So `StaticResource` inside a `Source`-loaded dictionary resolves against the **ambient
   application scope**, never against the dictionaries merged alongside it afterwards. No ordering fixes this.
3. Mutating `SolidColorBrush.Color` on an instance a loaded element is painting with **does** repaint it.
4. `ResourceDictionary.ThemeDictionaries.Keys` and enumeration of a XAML-backed dictionary both throw
   `NotSupportedException` under Uno, so the brush keys cannot be discovered at runtime — but keyed
   `TryGetValue` works, and it also materializes Uno's lazy initializer where the indexer does not.

**Fix.** `BaseTheme` creates the `SharedColors.xaml` dictionary **once** and keeps it for the theme's lifetime;
every rebuild resolves the colour layers and writes the results into the existing brushes
(`Helpers/SemanticBrushUpdater.cs`). Because point 4 rules out enumeration, the brush keys are generated from
`ThemesConstants.SemanticColorKeys` x `ThemesConstants.BrushStateSuffixes` and looked up by key. That naming
convention was validated against the XAML before relying on it: all **840** brushes in `SharedColors.xaml`
parse as `<role><state>Brush` -> `<role>Color`, with 9 distinct state suffixes and **zero** violations,
including the longest-prefix cases (`PrimaryInverseBrush` -> `PrimaryInverseColor`, not `PrimaryColor`).
The ~288 key strings are precomputed once rather than rebuilt per pass — `Apply` runs on every rebuild, which
during a color-picker drag is once per frame.

Consequences worth noting:

- A role missing from `SemanticColorKeys` silently keeps its parse-time colour. The list is the contract; it is
  the same set `doc/material-colors.md` publishes.
- **`Opacity` has to be rewritten too, not just `Color`.** Forcing the brush dictionary to materialize
  inside `UpdateSource` moved it *earlier* than Uno's lazy initializer would have — to construction time,
  before the theme is reachable from `Application.Current.Resources`. `Opacity="{StaticResource HoverOpacity}"`
  then resolved to nothing and every overlay brush silently became fully opaque, which rendered the
  NavigationView hover pill as a solid block that hid its own label. Instrumenting the real Material sample
  startup showed `ambientHoverOpacity=<unresolvable>` with `Opacity=1` and even `Color=#00FFFFFF` on the first
  materialization. Resolving both `Color` and `Opacity` from the theme's own layers makes the brush dictionary
  independent of ambient scope and of when it is materialized — verified against the XAML first: the only
  non-`*Color` `StaticResource` references in all 840 brushes are the 8 `<state>Opacity` tokens, and every
  brush's opacity token matches its own state suffix with zero violations.
- The consumer override dictionary is merged **after** the brush dictionary, so an explicitly-defined `*Brush`
  key still wins — `Given_ColorOverridePrecedence` continues to pass unchanged.
- Clearing the seed reverts the brushes to the base palette (covered by a test); the previous behaviour would
  have left the last seed applied.

**Tests added** (`Given_SeedColorPalette`): brush recoloured in place with instance identity preserved; state
brushes follow the role and keep their opacity; clearing the seed reverts; `Error*` brushes never follow the
seed; a rendered control paints with the generated colour; and an **already-rendered** control repaints when
the seed changes — the color-picker scenario, which is the one the previous behaviour failed.

### Review-panel follow-ups applied

A seven-lens review panel ran against the branch. Fixed here:

- **HighContrast was never swept.** `SharedColors.xaml` declares a third theme dictionary carrying its own
  copy of all 280 brushes, and the sweep only visited Light and Default — so a high-contrast user got brushes
  frozen at parse time, following neither the seed nor an override. No color layer anywhere defines
  HighContrast values (the HC brushes reference the same `*Color` roles), so `ThemesConstants.BrushThemeSources`
  now maps each brush theme to the color theme it resolves from, with HighContrast reading Default. Verified
  red: without the mapping the new test reads `#F5F5F5` (base) instead of the seed-derived `#8FCDFF`.
- **`UpdateSource` could strip the theme permanently.** It cleared every dynamic layer *before* rebuilding,
  with no boundary, and runs from seven property-changed callbacks plus the hot-reload handler — so any throw
  left the consuming app with no colour, spacing or shape dictionaries at all. Split into `BuildColorLayer`
  plus a commit phase that touches `MergedDictionaries` only once the new layers exist. Deliberately **not**
  a `try/catch`: the exception still propagates exactly as before (AGENTS.md §8 forbids swallowing), but the
  theme now keeps its last good palette.
- **Unguarded `new Uri` in two property-changed callbacks** (`ThemeColors.OverrideSource`,
  `BaseTheme.FontOverrideSource`) — a typo'd URI threw `UriFormatException` out of a PCC. Now `Uri.TryCreate`
  with a fall back to no override.
- **The opacity DataRows were a false guard.** They construct a theme inside a host that already has one
  merged app-wide, so ambient resolution supplies the tokens whether or not the sweep runs — verified: they
  stay green with the opacity write deleted. Kept (they pin the default values) but labelled, and joined by
  `When_OverriddenOpacityTokens_Then_EveryStateBrushUsesThem`, which overrides all eight tokens to values that
  exist nowhere in the ambient scope and does fail without the sweep.
- **`SemanticColorKeys` drift was unguarded** — only Primary and Error were asserted out of 32 roles.
  `When_SeedIsSet_Then_EverySemanticRoleBrushFollowsItsColor` now sweeps every role against an independent
  list held in the test.

Left open from the panel (not in this pass): seed alpha not masked, `SemanticThemeHelper` throwing from a getter,
thread-affinity of the brush mutation, no logging, the sample page's unsubscribed `ActualThemeChanged`, the
per-key themed-dictionary re-resolution and the re-parsed `SharedColorPalette.xaml` (both per-frame during a
picker drag), and `SetChangedCallback` being a single-slot `Action<bool>`.

### Docs sweep (post-review)

A full pass over `doc/` for the 8.0 rework, aimed at newcomers: `seed-colors.md` rewritten in plain
language (prerequisites tip, "How it works" without HCT jargon, Fidelity/TonalSpot presented as a
choice with guidance, simplified runtime section, sample-page pointer, new "Upgrading from 7.x"
section); `material-migration.md` gained the missing "Upgrading to Uno Themes v8" section
(palette-output change, `UseHighFidelityColors` obsoletion, live repaint); the Material/Simple
getting-started pages now lead Customization with the seed option and their `MaterialTheme`
properties table documents `Colors`/`DefaultCornerRadius`/`DefaultDensity` instead of only the
deprecated `ColorOverrideSource`; Simple's color-override sample migrated to `Colors.OverrideDictionary`;
reciprocal seed links added to `themes-overview.md`, `material-colors.md`, `design-tokens.md`,
`semantic-styles.md`, `lightweight-styling.md`, `material-dsp.md`; Cupertino's page now states seeds
are Material/Simple-only. cSpell and markdownlint pass on every touched file (the MD060 errors in
`semantic-styles.md` / `simple-controls-styles.md` pre-exist on `master` — new lint rule, untouched files).

### D6 (post-review): `PreserveSeedColor` bool → `SeedColorMode` enum

The unshipped `PreserveSeedColor` DP was replaced by a public `SeedColorMode` enum
(`Fidelity` = 0, default; `TonalSpot` = 1), property `ThemeColors.SeedColorMode`, mirrored on
`SemanticThemeHelper`. Rationale: the values are exact material-color-utilities /
`DynamicSchemeVariant` vocabulary (googleable, self-documenting), and the enum is extensible to
further M3 variants (Vibrant, Expressive, …) without another breaking change — a bool→enum
conversion after 8.0 ships would itself be breaking, while pre-release it is a rename sweep.
Notes: our `Fidelity` does more than M3's content variant (it also pins light Primary to the exact
seed hex — the XML docs own that deviation); an out-of-range cast degrades to `Fidelity` in the
generator rather than throwing (PCC path); the `[Obsolete] UseHighFidelityColors` bridge is
unchanged — `true` maps to `Fidelity`, `false` to `TonalSpot`, and an explicit `SeedColorMode`
assignment still wins via `HasExplicitSeedColorMode`. Everything in this document above this
section predates the rename and reads `PreserveSeedColor` — left intact as history.

### Before / after

Light `PrimaryColor` — 7.x default (tonal spot on master's solver) vs the 8.0 default:

| Seed | 7.x | 8.0 |
| --- | --- | --- |
| `#006495` | `#006597` | `#006495` |
| `#FF0000` | `#7E5149` | `#FF0000` |
| `#386A20` | `#3C6C24` | `#386A20` |
| `#808080` | `#006B76` | `#808080` |

Holding the generator at tonal spot isolates the solver's own magnitude: seed `#FF0000` moves from `#7E5149`
(master) to `#C00100`. Gray seed `#808080` in preserve mode now yields Secondary and Tertiary `#5F5E5E` instead
of `#4B6367` / `#525D7D`.

### Verification performed

- Full runtime suite on desktop: **144 passed, 0 failed, 1 skipped**
  (`When_BaseThemeIsCollected_Then_HotReloadHandlerDoesNotResurrectIt`, already `[Ignore]`d on `master`,
  unrelated). Run via `SimpleSampleApp.dll --runtime-tests=...` on Windows/Win32;
  `build/scripts/linux-skia-desktop-runtime-tests.sh` needs a Linux host and was not run locally — CI covers it.
- Red/fix/green confirmed for the solver: the four saturated `DataRow`s fail on `master` by 109 / 187 / 68 / 53
  and pass within 2 after the fix. Round-trip tolerance tightened from +/-20 to +/-2; measured error is 0 on
  every row.
- `Uno.Themes-packages.slnf` and `SimpleSampleApp` build clean for **desktop** and **browserwasm**, with no new
  warnings in any changed file.
- Palette output diffed against `master` using the harness below, with master's `ColorGeneration` extracted via
  `git show master:...` into a sibling project, for the seeds in the table above.

### Remaining

- [ ] `/review-panel` before opening the PR (`contract` and `quality` are the load-bearing lenses).
- [ ] Item 7 in full: confirm the `version.json` bump with the release owner and write the release note
      (the breaking commit markers are done).

---

## Reproduction harness

The `ColorGeneration` sources are pure math with no WinUI dependency, so they compile standalone — this is by
far the fastest way to see generator output without launching a sample app. Recreate it anywhere outside the
repo (do **not** check it in):

```xml
<!-- seedcheck.csproj -->
<Project Sdk="Microsoft.NET.Sdk">
  <PropertyGroup>
    <OutputType>Exe</OutputType>
    <TargetFramework>net10.0</TargetFramework>
    <EnableDefaultCompileItems>false</EnableDefaultCompileItems>
  </PropertyGroup>
  <ItemGroup>
    <Compile Include="Program.cs" />
    <Compile Include="$(RepoRoot)/src/library/Uno.Themes/ColorGeneration/ColorMath.cs" />
    <Compile Include="$(RepoRoot)/src/library/Uno.Themes/ColorGeneration/TonalPalette.cs" />
    <Compile Include="$(RepoRoot)/src/library/Uno.Themes/ColorGeneration/Hct/Cam16.cs" />
    <Compile Include="$(RepoRoot)/src/library/Uno.Themes/ColorGeneration/Hct/HctColor.cs" />
    <Compile Include="$(RepoRoot)/src/library/Uno.Themes/ColorGeneration/Hct/HctSolver.cs" />
    <Compile Include="$(RepoRoot)/src/library/Uno.Themes/ColorGeneration/Hct/ViewingConditions.cs" />
  </ItemGroup>
</Project>
```

`SeedColorPaletteGenerator` itself cannot be included (it takes a WinUI `ResourceDictionary`), so mirror its
`GenerateCore` body in `Program.cs` — the chroma constants are in Finding 1 and the tone table is at
`SeedColorPaletteGenerator.cs:111-148`. Keep the mirror in sync with any change you make to the real
generator, or the harness will quietly lie to you.

Baseline values to check the harness against before trusting it (current `master`, seed `#006495`):
`PrimaryColor` light `#006597` / dark `#91CEFF`; `SecondaryColor` light `#50606F`; `TertiaryColor` light
`#645779`; `new TonalPalette(25, 84).GetArgb(40)` = `#834E47`.
