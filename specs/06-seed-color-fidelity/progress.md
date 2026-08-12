# 06 — Seed color fidelity: exact-seed Primary, working HCT solver, complete content mode

Status: **investigated, not started.** No product code has been written — this branch carries the spec, a
lessons entry, and the `version.json` major bump only. This document is a complete handoff; a new agent
should be able to pick it up cold and finish it without re-deriving anything below.
All five decisions (D1–D5) are settled. **Ships as 8.0** — see [Release mechanics](#release-mechanics-for-d5).
Branch: `dev/sb/seed-color-fidelity`.
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

- [ ] **Red first.** Extend `Given_SeedColorPalette.When_RoundTripping_Argb_Through_Hct_Then_ColorIsPreserved`
      with saturated `DataRow`s: `#FF0000`, `#00FF00`, `#0000FF`, `#B3261E`. Confirm they fail at the current
      ±20 tolerance (they fail by 109 / 187 / 68 / 53 — see Finding 4).
- [ ] Add a test asserting `new TonalPalette(25, 84).GetArgb(40)` is within a few units of `#B3261E` — this is
      the single clearest expression of the bug, and it is M3's own published value.
- [ ] Replace `HctSolver.BisectChroma` with the analytical approach from material-color-utilities
      `HctSolver.solveToInt` (linear-RGB plane intersection, then `bisectToLimit` for the boundary). Keep the
      existing early-outs at `HctSolver.cs:24-34` (tone <0.0001 → black, >99.9999 → white, chroma <0.5 → gray)
      — those are correct and cheap.
- [ ] **Green.** Then tighten the ±20 tolerance to what the corrected solver actually holds (expect ≤3) so the
      test stops being able to pass for the wrong reason.
- [ ] Watch allocations — this runs in `TonalPalette.GetArgb`, called ~60× per palette generation, and
      per AGENTS.md §2 the HCT math is a value-type-discipline hot path. `TonalPalette` already memoizes per
      tone (`TonalPalette.cs:19,45-50`); keep that.

### 2. Complete content / fidelity mode

- [ ] In `GenerateCore`, apply the content-variant chromas when `useHighFidelity` is true:
      secondary `chroma/3`, tertiary `chroma/2`, neutral `min(chroma/12, 4)`, neutralVariant `min(chroma/6, 8)`.
      Non-fidelity mode keeps 16 / 24 / 4 / 8 unchanged.
- [ ] Explicit seeds still win: `SecondarySeed` / `TertiarySeed`, when set, continue to use that color's own
      hue **and** chroma (`SeedColorPaletteGenerator.cs:78-97`). Do not route those through the content math.
- [ ] Runtime test: gray seed `#808080` in fidelity mode must produce a near-gray Secondary and Tertiary
      (chroma < 5), not today's `#4B6367` / `#525D7D`.
- [ ] Fix the now-stale comment at `SeedColorPaletteGenerator.cs:71-73`.

### 3. Pin `PrimaryColor` to the seed (the actual request)

- [ ] Light `PrimaryColor` = the seed ARGB verbatim (D3). Dark stays `primary.GetArgb(80)`.
- [ ] `OnPrimaryColor` (light) picks tone 100 or tone 10 by contrast against the pinned seed (D2). Reuse the
      L*-based contrast math already present in the test file's `ColorMathAccessor` — but move it into
      `ColorMath` as a proper internal API rather than duplicating it (AGENTS.md §1, no duplication).
- [ ] Pinning only makes sense with the seed's own chroma preserved, so this implies fidelity mode. Decide and
      document: does pinning force `UseHighFidelityColors`, or is `max(chroma, 48)` still applied to the rest
      of the ramp while Primary alone is exact? **Recommend forcing fidelity** — a chroma-48 ramp around a
      chroma-42 pinned swatch has a visible discontinuity at the Primary/PrimaryContainer boundary.
- [ ] Runtime tests: (a) generated light `PrimaryColor` equals the seed exactly, for several seeds across the
      tone range; (b) `OnPrimary`/`Primary` contrast ≥ 4.5:1 for a pale seed (~T75), a mid seed (~T40) and a
      dark seed (~T20); (c) `#006495` still round-trips to itself.

### 4. Promote the flag to public API (D4)

- [ ] Add a DP on `ThemeColors` — suggested name `PreserveSeedColor` (`bool`, default follows D1/D3 outcome).
      XML docs required (AGENTS.md §6).
- [ ] `BaseTheme.UseHighFidelityColors` becomes `[Obsolete("Use ThemeColors.PreserveSeedColor instead.")]` and
      forwards; do not delete it (shipped in 7.0.3).
- [ ] `BaseTheme.UpdateSource` (`BaseTheme.cs:405-413`) resolves the effective value: `Colors?.PreserveSeedColor`
      falling back to the obsolete virtual, mirroring how `PrimarySeed` already falls back to `DefaultPrimarySeed`.
- [ ] `SimpleTheme.cs:30` moves to the new mechanism.
- [ ] Changing a `ThemeColors` DP must trigger a rebuild — verify it flows through
      `ThemeColors.OnPropertyChanged` → `SetChangedCallback` → `BaseTheme.UpdateSource`, and that it is treated
      as a **non**-structural change (`isStructural` is currently true only for `OverrideDictionaryProperty`,
      `ThemeColors.cs:137`).

### 5. Documentation

- [ ] `doc/seed-colors.md`: remove Error from the generated list; document exact-seed Primary, the
      `OnPrimary` contrast flip, and the new `PreserveSeedColor` property.
- [ ] `specs/03-seed-color-palette/seed-color-palette.md`: fix line 9 (Error), and line 230's
      "extreme gamut boundaries" characterization once the solver is fixed.
- [ ] Cross-link `doc/seed-colors.md` ↔ `doc/material-colors.md` (AGENTS.md §13).
- [ ] Consider a sample page under `src/samples/SamplesApp.Shared/Content/` with a live seed picker so the
      pinning behavior is visible in all three heads.

### 6. Verification (AGENTS.md §4, §5 — none of this is optional)

- [ ] Runtime tests pass headless:
      `dotnet build src/samples/SimpleSampleApp/SimpleSampleApp.csproj -c Release -f net10.0-desktop`
      then `build/scripts/linux-skia-desktop-runtime-tests.sh` (or the sample DLL with
      `--runtime-tests=<results.xml>`). See the `/uno-themes-runtime-tests` skill for filter syntax.
- [ ] Build clean, no new warnings, for at least desktop + wasm.
- [ ] Diff generated output against `master` for a spread of seeds and eyeball it — the harness below prints a
      full palette in one command. Attach a before/after table to the PR.
- [ ] Run the seven-lens review panel (`/review-panel`) before opening the PR; `contract` and `quality` are the
      load-bearing lenses here (public API change + shipped-behavior change).

### 7. Release (D5)

- [x] `version.json` moved from `7.2-dev.{height}` to `8.0-dev.{height}` on this branch.
- [ ] Confirm the manual bump with whoever owns the release process before merging — it is normally automated
      after a release-branch cut (`aa91e6f3`, `157ff71f`).
- [ ] Mark the solver and Primary-pinning commits `feat!:` / `fix!:` or add a `BREAKING CHANGE:` footer.
- [ ] Release note with a before/after color table for at least one saturated seed, so consumers can see what
      changed in their app before they upgrade. The harness below generates it in one command.

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
