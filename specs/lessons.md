# Lessons

Domain lessons and postmortems for the Uno.Themes repo. Append new entries at the top.

---

## Shared brush inputs on attached properties must not inherit the consuming element's data context

**Context:** Fluent semantic fixes (2026-09-07, `d7289c42`). The text-button
adapter supplied semantic brushes through Brush-valued attached properties.
Rendered state, style replacement, and resource cleanup tests passed, but a
collection guard retained the old button after its style was removed and the
button was unloaded. An equivalent native-style button was collected.

**Root cause:** the attached properties used ordinary `PropertyMetadata`.
On the tested Uno runtime that lets shared brushes enter the data-context
inheritance association path, which can retain the consuming button through an
associated parent even after the adapter's resource dictionary has been removed
and property values cleared. The relevant upstream path is
`X:/src/uno/src/Uno.UI/UI/Xaml/DependencyObjectStore.Binder.cs` in the local Uno
source checkout: `SetChildrenBindableValue` /
`AssociateParent` manage this relationship. Event detachment alone cannot break
a framework-owned inheritance association.

**Fix and application:** resource-only Brush inputs do not need to inherit a
button's DataContext. Under `HAS_UNO`, register them with
`FrameworkPropertyMetadataOptions.ValueDoesNotInheritDataContext`; retain the
native `PropertyMetadata` path on WinUI. Uno's handling of this flag avoids the
shared-brush association path. Do not apply the flag indiscriminately to values
whose bindings intentionally rely on inherited DataContext.

**Proof:**
`Given_FluentLightweightStyling.When_TextButtonStyleIsCleared_ThemeDoesNotRetainButton`
now passes for both the native baseline and semantic style. Keep a live theme
during the collection test, hold WeakReference trackers in fields, and collect
from a separate non-inlined method after the method owning strong references
has returned. State/resource assertions prove rendering and cleanup; they do
not substitute for proving that the old element is collectible.

---

## `ResourceDictionary.TryGetValue` falls back to the SYSTEM resources — a per-branch assertion on a system key through it reads the ambient XCR value, not the branch

**Context:** Fluent theme, gap-audit fixes (2026-09-04, `specs/05-fluent-theme/` Phase 7). A new
resource-graph test walked the theme's `ThemeDictionaries` to assert the Light and Default branches
of `TextOnAccentFillColorPrimary` independently, using `branch.TryGetValue(key, …)` on each
candidate branch dictionary. Before the fix the key was not written anywhere in the theme, yet the
"pale accent → black text" case passed and only the "navy accent → white text" case failed.

**Root cause:** Uno's public `ResourceDictionary.TryGetValue(object, out object)` is
`TryGetValue(key, out value, shouldCheckSystem: true)` (`ResourceDictionary.cs`). For a key the
dictionary does not hold, it consults the system resources — where every `XamlControlsResources`
token lives — resolved against the **ambient** application theme. The first branch dictionary probed
(a code-built one that lacked the key) therefore returned XCR's stock value for the dark-mode dev
machine: black. A test on a non-system key (`PrimaryColor`) written the same way was correct by luck.

**How to apply:**
- Own-entries reads of a dictionary must **enumerate** it (`foreach (var pair in dictionary)`),
  never `TryGetValue`/`ContainsKey` — both check the system dictionary by default. Catch
  `NotSupportedException` for XAML-backed dictionaries (not enumerable on Uno) and treat them as
  "not here"; make sure the asserted keys live in code-built dictionaries. The library code already
  follows this (`ToOwnEntries` in `FluentAccentPalette` / `FluentLightweightBridge`); the test
  helper had not.
- A branch-level test on a key that XCR also defines must be red-proven in **both** directions
  (a value that differs from the ambient stock, and one that matches it): only the differing case
  can expose a system-fallback leak, and which direction differs depends on the host's theme.
- Corollary for comparing "stock" values: the app-level `TryGetValue` result may not have the type
  the XCR XAML declares (the host's app-level theme or the system lookup decides what is returned);
  compare container-vs-app resolution as objects instead of asserting the declared type.

---

## A derived layer that re-implements a base recipe drifts silently when the base gains a mode — consume the mode, and prove agreement for both theme branches from the resource graph

**Context:** Fluent theme, master integration (2026-09-04, `specs/05-fluent-theme/`). #1697 added
`SeedColorMode` and made `Fidelity` the default, pinning the generated light `PrimaryColor` to the
seed verbatim. `FluentAccentPalette` had re-implemented the old recipe (tone 40 of a raw-chroma
palette) instead of consuming the generator's mode, so after the merge the built-in Fluent accent and
the semantic `PrimaryColor` disagreed under every seed, and `TonalSpot` was ignored — while the
whole Fluent suite stayed green locally.

**Why it stayed green:** `When_SeedSet_ForwardAndReverseFlowsAgree` asserted the *ambient* branch.
The dev machine runs Windows dark mode, so it compared the dark `PrimaryColor` (tone 80, unchanged
by Fidelity) with `SystemAccentColorLight3` (tone 80) — a tautology in that host. Only the Light CI
host would have failed. Proven by stashing the library fix and re-running: 5 new cases red, the old
ambient test still green.

**How to apply:**
- When `BaseTheme` gains a knob (`SeedColorMode`, `DefaultFontFamily`, `DefaultSpacing`, …), grep
  every concrete theme for code that **re-derives** what the base now parameterizes
  (`new TonalPalette(hct.Hue, hct.Chroma)`, hard-coded tones, literal font keys) and route it through
  the same mode/value. Prefer consuming the base's resolved output; when a recipe must be mirrored,
  put it in one shared helper (`FluentAccentPalette.PaletteOf(color, mode)`) and name the base
  method it mirrors in the doc comment.
- An "A agrees with B" contract must be asserted **for both branches** from the theme's own
  `ThemeDictionaries` (walk later merged dictionaries first — the `FindBranchColor` pattern in
  `Given_FluentSeedAccent`), never only through the ambient lookup: the CI host and developer
  machines run different appearances, so an ambient-only test proves a different half on each.
- A merge from master is a **behavior change** for derived libraries even when nothing in them
  conflicted. Re-run the derived suites in the CI-parity host and read the *new* warnings (here
  CS0672 pointed straight at the obsolete override) before assuming a clean merge is a no-op.
- Red-proof after the fact is cheap: `git stash push -- <lib files>`, build, run the filter, pop
  (~2 min here). Do it when the test was written alongside the fix.

---

## A generated layer is always a *merged* dictionary, so it can only shadow keys declared inside `ThemeDictionaries`

**Context:** Spec 09 (`DefaultFontFamily`, PR #1707). `When_DefaultFontFamilySet_Then_ThemeAliasKeysFollow`
failed for `SimpleButtonFontFamily` and `SimpleToggleButtonFontFamily` while passing for
`DatePickerFlyoutPresenterFontFamily` — all three listed in the same `SimpleTheme.FontFamilyAliasKeys`
and all three regenerated by the same loop.

**Root cause:** the two failing keys were declared at the **top level** of `Button.xaml` /
`ToggleButton.xaml`, under a "Layout / sizing resources (theme-agnostic)" heading; the passing one is
declared inside `ResourceDictionary.ThemeDictionaries`. `BaseTheme` sets its **own** `Source` to
`mergedpages.xaml` (`BaseTheme.cs:505`), so every top-level key in that merge is one of the theme
dictionary's *own* resources — and own resources beat `MergedDictionaries`. `GenerateFontFamilyScale`
returns a dictionary added through `AddThemeDictionary`, i.e. a merged one, so it can never shadow a
non-themed key no matter what it contains.

The two halves of the precedence rule are opposites, and both are load-bearing here:

- **themed lookup:** a merged dictionary's `ThemeDictionaries` beat the parent's own — this is what
  makes the generated layer work at all, and what the `mergedpages` typography lesson above records.
- **non-themed lookup:** the parent's own keys beat every merged dictionary — which is why the
  generated layer is powerless against a top-level declaration.

**How to apply:**
- **Any key a generated/runtime layer has to override must be declared inside `ThemeDictionaries`**,
  even when its value is genuinely appearance-independent. "Theme-agnostic" describes the *value*; the
  `ThemeDictionaries` block is what determines *who can override it*. Declaring it once per appearance
  is the cost of being overridable.
- **Adding the key to the generated dictionary's top level does not work** — that dictionary is merged,
  so its non-themed entries lose to the theme's own. This was tried and measured before being reverted;
  do not reach for it again.
- **Two consumers of one key can disagree about whether it is broken.** After the setters moved to
  `{ThemeResource}` the rendered button followed a runtime change correctly while a direct
  `Resources.TryGetValue` on the same key still returned the parse-time snapshot — the rendered test
  passed and the token test failed, in the same run. Fixing the read path does not fix the key; assert
  both, and when they disagree, believe the one that says something is still wrong.
- Resource keys are public API here, so the direct lookup is a real consumer, not just a test artefact.

---

## Regenerating a resource key is half a runtime seam — the setter that reads it must be `{ThemeResource}`

**Context:** Spec 09 (`DefaultFontFamily`, issue #1705). The generated typeface layer rewrites each
design system's per-control alias keys (`SimpleButtonFontFamily`, …) so a runtime family change
reaches control templates, and a token test proved the keys followed. Simple's `Button` and
`ToggleButton` read them with `{StaticResource}`, so the rendered controls stayed on Inter.

**Root cause:** `{StaticResource}` in a `Setter` resolves once, when the dictionary is parsed, and
holds the value. Writing a new value under that key afterwards changes nothing for controls already
styled from it, and nothing for controls styled later either — the setter no longer consults the
dictionary. `{ThemeResource}` keeps the reference and re-resolves on a theme-change pass.

**How to apply:**
- Any key a `BaseTheme` layer generates or regenerates at runtime (`*FontFamily`, `Space*`,
  `*CornerRadius`, `ControlHeight*`, the semantic brushes) must be read with `{ThemeResource}`
  wherever a style consumes it. `{StaticResource}` is correct only for values no theme property can
  move. The `<StaticResource x:Key=… ResourceKey=…>` *alias declaration* is unaffected — it is the
  consuming `Setter`/attribute that has to be `{ThemeResource}`.
- **A token test cannot see this.** `TryGetValue` walks the dictionaries; a setter does not.
  Whenever a fix's claim is "the control follows", assert the rendered `FontFamily` / `Padding` /
  `CornerRadius` on a realized control after the property changes, not the resource lookup. This is
  the same trap the `SharedTypography` lesson below records, hit from the other side.
- **The same defect held for the spacing/shape seam, and was measured before it was fixed.** A
  sweep over Simple and Material v2 (alias closure over `<StaticResource x:Key ResourceKey>`
  declarations rooted at the generated `Space*` / `Radius*` keys, then every `{StaticResource K}`
  consumer of that closure) found 98 sites in 22 files — 78 Simple, 20 Material v2. Red: a
  realized Simple button kept `Padding` 12 after `DefaultSpacing` moved it to 22.5; a realized
  Material `ContentDialog` kept `CornerRadius` 28 after `DefaultCornerRadius` moved it to 7. The
  sweep is the way to find these: grep for the *generated* key alone misses every per-control
  alias (`SimpleButtonCornerRadius`, `MaterialContentDialogPanelPadding`, …) that stands between
  the token and the setter. Density constants (`ControlHeight*`, `IconSize*`,
  `TouchTargetMinSize`) are regenerated with fixed values, so a `{StaticResource}` read of those
  is harmless and was left alone.
- **`doc/design-tokens.md` had documented the defect as a design property** ("construction-time
  settings … the new value never reaches the control templates"). A limitation written into the
  docs is still a claim; check it against the mechanism before carrying it forward.

---

## A resource merged into `mergedpages` cannot override one merged *by* it — and a comment claiming it can is not evidence

**Context:** Spec 09 (single typeface, PR #1710). CI failed eight rows asserting Simple's
`*FontWeight` tokens; each returned the value `SharedTypography.xaml` declares. The tokens were
declared, correctly, in `Uno.Simple.WinUI/Styles/Application/Typography.xaml`, and
`BaseDictionaries.xaml` carried a comment stating that file's resources "shadow these shared
defaults". They never had.

**Root cause:** `Uno.XamlMerge.Task` hoists every input's `MergedDictionaries` to the top of
`mergedpages.xaml` and folds every input's themed resources into mergedpages' *own*
`ThemeDictionaries`. A merged dictionary out-ranks the parent's own theme dictionaries, so
anything `BaseDictionaries.xaml` merges — `SharedTypography.xaml` here — beats every file left in
the `Styles\Application\` glob. `Fonts.xaml` and `Thickness.xaml` won only because they had been
removed from the glob and merged explicitly; that was never written down as the *reason*, so the
next file to need an override didn't get it.

**How to apply:**
- In a XamlMerge library, "later in the glob wins" is false. The only way a theme file overrides a
  shared default is `XamlMergeInput Remove` plus an explicit `<ResourceDictionary Source=...>` in
  the base dictionary, listed after the one it overrides. Treat the `Remove` and the `Source` as a
  single indivisible edit; either alone is silent breakage.
- A file that is merged by `Source` must load standalone. `Typography.xaml` could only be moved
  once the single-typeface collapse left it literal-only — while it still carried
  `<StaticResource ResourceKey="SimpleBoldFontFamily" />` it would have resolved against the
  ambient application scope instead (see the `StaticResource` lesson below).
- **A comment describing merge order is a claim, not a fact.** Two comments in this tree described
  the ordering, and they contradicted each other: `BaseDictionaries.xaml` said the glob wins,
  Simple's old `Fonts.xaml` said it doesn't and duplicated the slot mappings to work around it.
  The one that had a workaround attached to it was the true one. When two comments disagree,
  believe the one someone paid for.
- **Assert the rendered value, not only the token.** Resource lookup (`TryGetValue`) and a
  control's `{ThemeResource}` setters are different resolution paths. Here both were wrong, but
  that is luck: a token test alone cannot tell you which layer moved.
- **The blast radius of a shadowing bug is every key in the file, not the ones with tests.** Only
  the weights failed CI because only the weights were asserted; twelve `*FontSize` tokens and
  every `*CharacterSpacing` were equally shadowed, and Simple's display text had been rendering at
  the Material 57px baseline instead of 72px. When a lookup-order defect is confirmed, enumerate
  the whole dictionary against what it is supposed to override before sizing the fix.

---

## A design-token "mode" (density) must be a factor over the scale's base unit, never a competing source of the base value

**Context:** Spec 07 (`DefaultSpacing`, issue #1688). The first implementation gave `Density` enum members base-unit pixel values (`Compact = 3`, `Regular = 4`, `Comfy = 5`) and made `DefaultSpacing` an *override* that beat the preset ("override-beats-preset, like the color stack"). The user rejected this: density is a **mode** the consumer picks (Compact / Regular / Comfy), not an alternate spelling of a pixel value.

**Root cause:** two orthogonal design axes were collapsed into one knob. Spacing (the scale's base unit — a brand/identity decision) and density (a contextual mode over that scale — a usability decision) both fed the same variable, making them mutually exclusive: setting a branded base unit silently discarded the density axis, and `DefaultSpacing="6"` + `Compact` was inexpressible. Encoding the magnitudes as the enum's underlying values (`Compact = 3`) hard-coupled the presets to the default base and leaked implementation into the public API surface.

**How to apply:**
- When a token system exposes both a scale and a mode, make them **compose**: `effective = base × modeFactor` (here Compact ×0.75, Regular ×1, Comfy ×1.25 — chosen so the default base of 4 reproduces the historical 3/4/5). "Override beats preset" is the right pattern for two sources of the *same* value (color overrides); it is the wrong pattern for two *different* axes.
- Enum underlying values are API surface. If a preset's magnitude is meaningful, map it in code (a `switch` near its single consumer); never make the enum value *be* the magnitude — it can't survive a base-unit change and it invites `(double)enum` casts.
- Fixed tokens (`ControlHeight*`, `IconSize*`, `TouchTargetMinSize`) stay invariant across **both** axes — density and spacing scale breathing room, never structure or touch targets.

---

## Pick the oracle the reference *implementation* emits, not the value the spec *publishes* — and never assume a numeric fallback is doing the job its name claims

**Context:** Seed color fidelity (spec 06). Two distinct traps hit while correcting `HctSolver`.

**Trap 1 — the wrong oracle.** The plan asserted `new TonalPalette(25, 84).GetArgb(40)` "must be `#B3261E`", M3's published error swatch. It must not: `#B3261E` is HCT(26.0, **76.3**, 39.7), not the tone-40 entry of a chroma-84 palette. The corrected solver emits `#BA1A1A` — which is exactly what material-color-utilities' own `SchemeTest` asserts (`0xffba1a1a`), along with `#410002`, `#FFB4AB` and `#FFDAD6` for the other tones. Had the test been written to the plan's number, a correct solver would have been "fixed" back into a wrong one.

**Trap 2 — the fallback that was doing all the work.** `SolveToArgb` derived CAM16 `J` from a *gray* at the target tone, then rejected any candidate whose L\* missed by more than 1.0. Since the `J` producing a given L\* depends on chroma, every saturated request failed that check immediately and fell through to `BisectChroma`, which cut chroma until the tone error closed — i.e. until the color was nearly gray. The code, its comments, the test's tolerance and the spec all described this as a *gamut* limitation. It was not; the gamut was never reached. The fix was to solve for `J` (Newton, per MCU's `findResultByJ`) rather than assume it, which reduced the bisection to the genuine out-of-gamut fallback it had always been named for.

**How to apply:**
- When porting from a reference implementation, the oracle is **what that implementation outputs**, taken from its own test suite — not the design spec's published swatches. Specs publish hand-picked values; generators emit computed ones, and they differ. Pin a **whole ramp** (six tones here), never a single value: one match can be coincidence, six cannot.
- If a numeric routine has a "fallback" path, measure how often the primary path actually succeeds before trusting any description of the fallback. A fallback that runs on *every* saturated input is not a fallback, it is the algorithm — and its comment will be describing a scenario that never occurs.
- When a comment, a test tolerance, and a spec all agree on a cause, that is not corroboration if all three were written by the same person from the same assumption. Reproduce the failure standalone (`ColorGeneration` compiles with no WinUI dependency — a 6-file console project prints the whole palette in one command) and check the claimed cause is present at all.
- Contrast targets must be **swept, not spot-checked**. D2's "flip between tone 100 and tone 10" reads as obviously sufficient and provably is not — it tops out at 4.48:1 for backgrounds near L\* 50. A brute-force sweep over hue x tone x chroma found the true worst case in seconds and justified adding tone 0 as a third candidate (worst case 4.617:1).

---

## `StaticResource` in a `Source`-loaded ResourceDictionary resolves against the *application* scope — the only way to update those resources later is to mutate the instances

**Context:** Seed color fidelity (spec 06). A seeded theme updated every `*Color` resource correctly while every `*Brush` resource — and therefore every rendered control and the sample page's live color picker — kept painting the previous palette.

**Root cause:** `SharedColors.xaml` declares ~840 brushes as `<SolidColorBrush Color="{StaticResource PrimaryColor}" />`. `BaseTheme.UpdateSource` built that dictionary with `new ResourceDictionary { Source = ... }` and merged the palette/seed/override dictionaries into it *afterwards*. `StaticResource` resolves eagerly at parse time, and — measured, not assumed — it resolves against `Application.Current.Resources`, not against the dictionary's own merge tree. Four arrangements were probed (`Source` first, `Source` last on the same dictionary, brushes merged last under a parent, and app scope primed before the parse); **only** the primed-app-scope one worked. No merge ordering fixes it.

**How to apply:**
- A resource declared as `<SolidColorBrush Color="{StaticResource X}" />` is a **snapshot taken against the ambient application scope**, not a binding and not a lookup against its siblings. Redefining `X` in a dictionary merged later changes nothing. Verify by probing arrangements before designing a fix around merge order — the intuitive "merge the values first" does not work.
- `{ThemeResource SomeBrush}` resolves to a brush **instance** and re-evaluates only on a theme change. So replacing a dictionary can never update anything already rendered. If a resource must change without re-navigation, keep one instance and mutate it (`brush.Color = …` does repaint loaded elements). Keeping the instance stable across rebuilds is part of the contract, not an optimization.
- Under Uno, `ResourceDictionary.ThemeDictionaries.Keys` and enumerating a XAML-backed `ResourceDictionary` both throw `NotSupportedException`; only keyed `TryGetValue` works — and `ThemeDictionaries.TryGetValue` also materializes the lazy initializer where the indexer returns a raw `LazyInitializer`. Any code that must sweep XAML-declared resources needs an explicit key list, so validate the naming convention against the XAML first (all 840 brushes matched `<role><state>Brush` → `<role>Color` with zero violations, which is what made a generated key list safe).
- `ResourceDictionary.TryGetValue` resolves `ThemeDictionaries` against the **application** theme, ignoring any `FrameworkElement.RequestedTheme`. A test that needs Light and Dark independently cannot use it, and the app theme in the runtime-test host is not guaranteed to be Light.

**The follow-on bug this caused, which is the sharper lesson:** sweeping the brushes required *materializing* the theme dictionaries, and doing that inside `UpdateSource` moved materialization **earlier** than Uno's lazy initializer would have — to construction time, before the theme is reachable from `Application.Current.Resources`. The brushes also carry `Opacity="{StaticResource HoverOpacity}"`, which then resolved to nothing, so every overlay brush became fully opaque and the NavigationView hover pill rendered as a solid block hiding its own label. Only `Color` was being rewritten, so nothing corrected it.

- **Forcing a lazy resource to initialize is a behavioural change, not an implementation detail.** Laziness in a resource system is often load-bearing ordering: it defers resolution until the ambient scope is complete. If you materialize early, you inherit whatever scope exists at that moment, and every eagerly-resolved value in that dictionary — not just the one you came for — silently degrades to its default.
- If you take over resolution for one property of a resource, take over **all** of them. Half-patching leaves the other properties resolving through the path you just broke. Enumerate what the XAML actually depends on first: here, a scan showed the only non-`*Color` `StaticResource` references across all 840 brushes were the 8 `<state>Opacity` tokens, each matching its own brush's state suffix with zero violations — which is what made patching both safe.
- **A test that captures the current value and asserts it is unchanged cannot detect a wrong current value.** `originalOpacity = brush.Opacity` then asserting equality after a rebuild passed happily against `1.0`. Assert the *expected* value (`0.08`), not stability.
- Runtime tests run inside an app whose theme is already merged, so the ambient scope is always complete and cold-start resolution failures are invisible to them. When the suspected trigger is startup ordering, instrument the real app (`Console.WriteLine` in the code path, run the sample head, grep stdout) — `ambientHoverOpacity=<unresolvable>` settled in one run what the test suite could not express.

**Verification trap:** the first version of these tests asserted on a rendered `Button.Background` and every case returned the same wrong color, which reads as "the generator is broken". The generator was correct; the brush layer was. When an end-to-end assertion fails, confirm which layer actually moved (here: `*Color` updated, `*Brush` did not) before touching the algorithm.

---

## A tolerance-based test whose inputs all sit outside the failure region is not coverage — it is a green light on a broken algorithm

**Context:** Seed color generation (spec 06). `Given_SeedColorPalette.When_RoundTripping_Argb_Through_Hct_Then_ColorIsPreserved` had been passing since the feature shipped in 7.0.3, with a ±20 per-channel tolerance and a comment explaining that the "simplified bisection solver" loses precision "at sRGB gamut boundaries". The solver was in fact clamping chroma to ~27-43 whenever more than ~36 was requested — pure red round-tripped to `#AA6D63` (off by 109), pure green to `#BBECAA` (off by 187), and M3's own error color `#B3261E` came back as `#7E4F48`. None of it was caught.

**Root cause:** the five `DataRow`s were black, white, mid-gray, `#6750A4` and `#386A20`. The first three have chroma ~0 and are *mathematically incapable* of failing a chroma-precision test. `#6750A4` measured 17 against a tolerance of 20. The test therefore asserted nothing about the property it claimed to cover, and the tolerance had been sized to whatever the implementation happened to produce rather than to what correctness required. The prose comment about "extreme gamut boundaries" made the gap look like a known, bounded limitation instead of an untested region.

**How to apply:**
- When a test takes a **tolerance**, ask what value the current implementation actually produces before accepting the threshold. A tolerance chosen to make today's code pass encodes the bug as the specification. Record the measured margin in the assertion message so later drift is visible.
- Choose `DataRow` inputs by where the algorithm is **most likely to break**, not by what is convenient or realistic-looking. For color math that means maximum chroma and gamut corners; the neutral cases are free but prove nothing.
- Treat "simplified", "approximate", or "good enough for realistic inputs" in a comment as a **claim requiring a test that pins the actual error bound** — otherwise it is an unfalsifiable excuse that survives every future review.
- Porting a reference algorithm (here material-color-utilities) and simplifying one step is fine; validating it only against inputs the simplification handles well is not. Diff against the reference implementation's own published values — M3's baseline palettes are the oracle and cost nothing to check.

---

## An app that loads assemblies by reflection cannot be trimmed — ILLink strips facade type-forwarders, and rooting is a treadmill

**Context:** ThemesSampleApp ALC wrapper (spec 05). In the browser, every guest died at `Assembly.GetTypes()` with `TypeLoadException: Could not resolve type with token 010003c9 from typeref (expected class 'System.IO.StringWriter' in assembly 'System.Runtime')`. An earlier investigation had noted that pre-loading `System.Runtime` merely advanced the failure to the next typeref (`System.IAsyncDisposable`) and concluded "rooting types one at a time is a treadmill" without identifying why.

**Root cause:** ILLink does not only drop unused *code* — it drops **type-forwarders from the framework facades**. The host's trimmed `System.Runtime` was 15 KB against the runtime pack's 45 KB, with the `StringWriter` and `IAsyncDisposable` forwarders removed (`grep -ac` on `obj/<cfg>/<tfm>/linked/System.Runtime.dll` vs the runtime pack copy proves it in one command). Guest assemblies are loaded by reflection at runtime, so the trimmer sees nothing they reference; every forwarder the *host* happens not to use is gone, and the guest hits them one at a time. The same mechanism explains a facade being absent from `_framework` entirely (`netstandard`).

**How to apply:**
- Any app whose job is loading assemblies the build cannot see — plugin hosts, ALC guest hosts, script runners — must publish **untrimmed** (`PublishTrimmed=false`). `TrimmerRootAssembly` treats the symptom: it restores one forwarder and the next missing one surfaces immediately. Scope it to the hosting head so ordinary heads keep trimming.
- Diagnose facade trimming by **diffing the ILLink output against the runtime pack**, not by reading `_framework` (webcil-encoded, so byte scans there prove nothing): `obj/Release/<tfm>/linked/<Assembly>.dll` is plain IL.
- `TypeLoadException ... from typeref (expected class X in assembly Y)` where Y is a facade means a **missing type-forward**, not a missing assembly. Check the forwarder before suspecting probing or ALC policy.
- **Never publish over a previous publish with different trim/AOT settings.** Mixed fingerprinted output produces mono's "Your mono runtime and class libraries are out of sync" + `RuntimeError: function signature mismatch`, which looks like a runtime bug and is not one. Wipe `bin`/`obj` for the TFM between configurations.
- A hosted guest resolves `ms-appx:///` against the **host's** package root. Assets a guest expects (fonts especially) must be carried by the host, or the guest silently degrades — Cupertino fell back to a default typeface with only a `FontDetailsCache` error in the console.

---

## A nested Uno host's `Run()` returning is not a lifetime signal — and desktop backends differ

**Context:** ThemesSampleApp ALC wrapper (spec 05). Every guest load failed ~1 s in on **Windows/Win32**, while the same code had been verified end-to-end on X11 (Xvfb/WSLg). The guest visibly booted — runtime-tests module init, ALC window mode, theme XAML parsed — and was then torn down by the host.

**Root cause:** `Win32Host.RunLoop()` gates the message pump on a **static** `_isRunning` field. The host app owns the process's only Win32 loop, so a hosted guest's `RunLoop()` only *schedules* its `Application.Start` onto that shared loop and returns `Task.CompletedTask` immediately — the guest then runs on the **host's** UI thread. `X11ApplicationHost.RunLoop()` does the opposite: it schedules `StartApp` and then blocks in a `while (!ShouldExit()) Thread.Sleep(100)` keep-alive for the guest's lifetime. The loader raced the run-loop task against "first content" and treated any completion as "guest exited before presenting content", so on Win32 it aborted mid-boot.

**How to apply:**
- When hosting a guest through `UnoPlatformHostBuilder`, **only a faulted run-loop task means failure**. A completed one carries no information — never use it as a proxy for "the guest died". Wait for a real readiness signal (`AlcContentHost.ContentChanged`) against a timeout instead.
- `SkiaHost.RunLoop` semantics are **per backend**, and a `.UseX11().UseLinuxFrameBuffer().UseMacOS().UseWin32()` builder picks one at runtime. Verifying ALC hosting on one desktop backend proves nothing about the others; Win32 vs X11 is the pair that bites, because the wrapper's dedicated `GuestApp-*` thread is load-bearing on X11 and vestigial on Win32.
- When a hosting failure has to be diagnosed from logs, the failure's *message* must be in the logs. Surfacing a user-presentable exception only in an `InfoBar` makes headless, CI, and screenshot-based runs undiagnosable — log it as well as showing it.
- Decompiling the pinned Uno runtime (`ilspycmd -t <Type> <Assembly>.dll` against the app's own `bin`) is the fastest way to settle "what does the host actually do here" when no reference checkout is present; the answer was a single static field.

---

## Never share "whatever the host already loaded" with an ALC guest — the Uno SDK's Debug tooling loads the *published* Uno.Themes.WinUI into the host

**Context:** ThemesSampleApp ALC wrapper (spec 05). First Debug desktop run of the wrapper after the July delivery crashed the whole process at guest boot: `TypeLoadException: Method 'GenerateSpecificResources' in type 'Uno.Material.MaterialTheme' … does not have an implementation` — repo-built `Uno.Material.WinUI` paired with a wrong-version `Uno.Themes.WinUI`.

**Root cause chain:** the Uno SDK implicitly references `Uno.UI.HotDesign` for every `Exe` in Debug (`Optimize != true`; opt-out: `UnoDisableHotDesign`), and Hot Design depends on the **published `Uno.Themes.WinUI` NuGet package**, which therefore lands in every Debug app bin — including hosts that deliberately reference no theme library. The dev-server client eagerly loads the Hot Design suite *and its theme dependency* into the default ALC at startup, headless, with no IDE attached (provable via `/proc/<pid>/maps` — package-loaded assemblies are file-mapped). The guest loader's tier 1 ("already loaded in the default ALC → share by simple name") then bound the guest's repo-built theme libraries against the published package version: the stale base class still declared an abstract member the repo has since removed → unhandled `TypeLoadException` on the guest's UI thread → SIGABRT of the host. Release never hits this (`IncludeAssets=None` when optimized), so Release-based soaks and CI stayed green while every Debug/IDE run was broken.

**How to apply:**
- "Share if already loaded" is inherently **version-unsafe** for any assembly the guest ships: whether the host has a same-named assembly loaded depends on tooling and timing, not on design. Assemblies under test (the repo's own theme libraries) must resolve **deterministically from the guest directory** — `GuestAssemblyLoadContext` now has an `_isolatedStartsWith` list checked before the share tiers. Keep it in sync if the repo grows a new packable library family.
- A host that must stay theme-free should also set `UnoDisableHotDesign=true` — Hot Design is the one SDK-implicit package that transitively carries a theme library into the bin (no-bleed checks that only run in Release will miss it).
- When an unexpected assembly appears in a bin, don't assume staleness from file dates (NuGet preserves package timestamps): clean-rebuild, then trace provenance through `obj/project.assets.json` dependency edges.
- Cross-version `TypeLoadException` at ALC guest boot ("method … does not have an implementation") is the signature of a mixed-version pairing between a guest assembly and a host-shared dependency; check which context supplied the base assembly before suspecting the build.

---

## Collectible-ALC guests: sweep-proof roots pin the ALC; verify reclamation with weak-ref telemetry, not RSS eyeballing

**Context:** ThemesSampleApp ALC wrapper (spec 05, Uno 6.7-dev). Load/unload soak leaked every guest ALC (~50 MB/cycle RSS growth); Debug builds of the wrapper collected fine, Release never did, with identical teardown logs.

**Root causes (three distinct, found via `dotnet-dump` `gcroot`):**
1. `DependencyProperty._getPropertyCache` caches `(targetType, "ns:Owner.Property") → DP` from style/VSM target paths. A guest style targeting an attached property on a **framework** element stores a default-ALC key with a guest-ALC value; Uno's `RemoveNonDefaultAlcEntries` checks only the **key's** ALC, so the entry — and through the DP's owner type, the whole guest ALC — survives every sweep.
2. The samples' `Shell` subscribes to the process-wide `SystemNavigationManager.BackRequested` and never unsubscribes; Uno's ALC event-subscription pruning does not cover that singleton, so the entire guest visual tree stays rooted.
3. Guest `DependencyObject` finalizers run during ALC unload and can re-populate caches **after** `ExitAlcApplication`'s sweep ran.

**How to apply:**
- When hosting (or testing) collectible-ALC guests, treat "Exit ran + Unload called" as insufficient: add a `WeakReference<AssemblyLoadContext>` check after a post-unload `GC.Collect/WaitForPendingFinalizers/GC.Collect`, and log collected/alive every cycle. RSS alone conflates managed leaks, GC retention, and native leaks.
- Diagnose with `dotnet-dump`: `dumpheap -type <ALC>` → `gcroot <addr>`, then **census all root anchors** (`grep "static variable:" | sort | uniq -c`) instead of reading only the first chain — multi-root pinning is the norm, and each fix reveals the next root. Dependent handles `(10)` are usually circular (CWT/collectible-statics), not true roots; a strong handle directly on an ALC in `_state == 1` (Unloading) is the runtime's own until unload completes.
- Debug-vs-Release differences in ALC collection are usually **timing masks**, not fixes — never conclude "works in Debug" means reclaimed.
- Mixed-ALC key/value caches are a general Uno hazard: any process-wide cache keyed by framework type but holding guest values defeats per-ALC sweeps. Prefer upstream fixes that also check the **value's** ALC.
- Known upstream gap (Uno 6.7-dev): each ALC guest window create/close leaks its native X11 GL context (+ llvmpipe threads) even through `Window.CloseAlcWindows`; managed heap stays clean. Track via llvmpipe thread-group count.

---

## Declarative-first for theme resources: build ResourceDictionaries in C# only when XAML provably cannot express them

**Context:** Fluent theme (2026-08-11, `specs/05-fluent-theme/`) — owner correction: "we shouldn't be doing any runtime C# changes to resource dictionaries and resources". The first implementation built three resource sets in code that are all expressible as plain XAML: the neutral semantic palette values (literal per-branch colors — Simple's `ColorPalette.xaml` is the shipped precedent), the lightweight-styling neutral brush defaults, and the Ⓜ gap-key styles (whose code path always landed on an empty style on Uno anyway). Code construction also duplicated the captured platform values across two C# tables with a "keep in sync" comment, and rebuilt ~70 brushes on every rebuild pass (a WASM memory-growth hazard, AGENTS §2).

**The rule:** in a theme library, a resource belongs in XAML unless one of two justifications holds:

1. **The value is computed from runtime input** — a seed color's tonal palette, an override-driven re-pointing, a live platform token that must be read at runtime (e.g. the Windows system accent, which also varies per user).
2. **The XAML mechanism is proven broken on Uno** for that shape — per-theme-branch `<StaticResource>` aliases (S1 case 4), intra-bundle aliases under container scope, theme-branch resources inside the XamlMerge bundle in Release (each documented in this file).

Everything else — literal per-branch colors/brushes, empty styles, setters-only styles, aliases to app-scope keys — is declarative. When code *transports* declarative values (e.g. `FluentColorPalette` copying `ColorPalette.xaml` branches so accent + neutral roles share one branch dictionary), the XAML file is the single source of truth and the code contains no values.

**How to apply:** before writing `dictionary[key] = ...` in a theme library, write the resource in XAML first and justify any remaining code path against the two criteria above (in the PR and in a comment at the code site). Load declarative theme-branch dictionaries once (static or per-instance cache), not per rebuild pass.

---

## Manual ThemeDictionaries reads must honor "Dark" (consumers never write "Default"); dark-branch rendering is not testable in the CI host

**Context:** Fluent override-driven accent cascade (2026-07-16, `specs/05-fluent-theme/`). Two related traps:

1. **Branch keys.** Code that reads a consumer override's `ThemeDictionaries` by hand (FluentLightweightBridge re-pointing, FluentAccentPalette basis resolution) originally read only the `Light`/`Default` keys — the keys *we* write. Consumers write **`Dark`** (see every sample head's `ColorPaletteOverride.xaml`); `Default` is the universal fallback for either theme. Manual reads must mirror the native semantics: exact branch key first (`Light`/`Dark`), then `Default`, then flat — and read OWN entries only (`TryGetValue` searches the ambient theme branch and breaks branch fidelity).

2. **Verification.** There is no way to *render* the dark branch in the CI host: the ambient app theme is fixed at launch, and setting `RequestedTheme` on an element (before load or flipped after load) does **not** re-branch `{ThemeResource}` lookups made inside XCR templates against app-scope dictionaries on this target — the control keeps the ambient branch's values. Guard dark-branch behavior at the produced resource-graph level (the theme IS a `ResourceDictionary`; assert its branch contents) and keep rendered assertions for the ambient branch. See `Given_FluentLightweightStyling.When_OverrideUsesDarkBranchKey_DarkBranchIsRepointed`.

**How to apply:** any new code path that inspects consumer-supplied `ThemeDictionaries` must handle `Light`/`Dark`/`Default` with the fallback above, and its dark-branch tests must assert dictionary structure, not rendered brushes.

---

## Theme-branch resources inside a XamlMerge bundle don't resolve in Release builds — put them in Source-merged dictionaries

**Context:** Fluent theme Phase 1 (2026-07-15, `specs/05-fluent-theme/`). `Given_FluentTypography` passed in Debug but failed 30/39 in the Release (CI-parity) run: every slot value (`DisplayLargeFontSize` = 68, SemiBold weights, zero character spacing) resolved to the shared (M3) `SharedTypography.xaml` values instead. `UnoXamlResourcesTrimming` was ruled out (`-p:UnoXamlResourcesTrimming=false` still failed).

**Root cause (behavioral):** Fluent's `Typography.xaml` was part of the `XamlMergeInput` glob, so its `ThemeDictionaries` (Light/Default branches carrying the slot values) landed inside `Generated/mergedpages.xaml` — the dictionary set as `BaseTheme.Source`. In Debug, lookups consult those bundle theme branches before the bundle's merged `SharedTypography.xaml`; in Release (compiled XAML codegen), the bundle's own theme branches lose to the merged dictionaries and the shared defaults win. The merged output itself was correct (both branches present) — it is the *resolution* that differs per configuration. Simple has the same structure and is likely affected too, but nothing asserts its absolute slot values, and its load-bearing font mappings were already moved to the Source-merged `Fonts.xaml` for a related reason (see the Fonts.xaml lesson below).

**Fix / how to apply:** theme-branch (Light/Dark-varying) resources that must shadow a shared default belong in a **standalone dictionary loaded via ms-appx Source from `BaseDictionaries.xaml`**, merged after the shared dictionary — never in a page that flows into the XamlMerge bundle. That path is CI-proven (Simple's `Fonts.xaml`, `Given_Fonts`). Fluent's `Typography.xaml` is now excluded from `XamlMergeInput` and Source-merged between `SharedTypography.xaml` and `Fonts.xaml`. Plain (non-theme-branch) resources — style aliases, control styles — resolve fine from the merged bundle in both configurations.

**Verification trap:** Debug runs mask this entirely. Always run typography/theme-branch assertions through the Release CI-parity script (`build/scripts/linux-skia-desktop-runtime-tests.sh`) before declaring green.

---

## Container-scoped themes: intra-bundle aliases and generated brushes resolve against the APP-LEVEL scope

**Context:** Fluent theme Phase 1 (2026-07-15, `specs/05-fluent-theme/`). The runtime tests instantiate `new FluentTheme()` scoped to a test container (spec 05 D14) inside SimpleSampleApp, whose app-level theme is `SimpleTheme`. Two failure modes surfaced that are invisible when the theme under test is the same as the app-level theme:

1. **Intra-bundle `<StaticResource>` aliases don't see their own bundle below app scope.** An alias like `TextButtonStyle` → `FluentTextButtonStyle` (both shipped in the same merged bundle) resolves at parse time against the app-level scope only. At app scope it happens to work (which is why Simple's `_Resources.xaml` → `Button.xaml` aliases pass); scoped lower, the alias yields nothing — or worse, silently binds to a *foreign* app-level theme's key of the same name. Fix: semantic keys targeting styles the library itself ships are resolved late-bound in code (`FluentTheme._bundleStyleAliases`), from the theme's own `Source` bundle.

2. **Generated semantic brushes materialize once, against the app-level scope.** `SharedColors.xaml` defines `<SolidColorBrush Color="{StaticResource PrimaryColor}"/>`; that color reference is a one-time resolution (already documented atop `Given_ColorOverridePrecedence`). Under a container-scoped theme in a host with a different app-level theme, `PrimaryBrush` & co. carry the *app-level* theme's palette, even though the `*Color` keys resolve correctly from the container. This is a pre-existing property of the shared brush layer (Simple/Material behave identically), not a FluentTheme bug. *Superseded 2026-09 by `SemanticBrushUpdater` (#1697): brushes are now per-theme-instance and rewritten from the theme's own color layers, so a container-scoped theme's brushes carry its own palette. The app-scope test topology remains the documented consumer shape but is no longer required for correctness.*

**How to apply:** when adding semantic aliases whose target ships in the same library, alias them in code, not XAML. When writing runtime tests that assert *generated brush values*, merge the theme into `Application.Current.Resources.MergedDictionaries` (the documented consumer topology) inside try/finally — container-scoped assertions on generated brushes test the wrong scope. Container-scoped assertions on `*Color` keys, styles, and typography values remain fine.

---

## `<StaticResource>` alias limits on Uno: no alias chaining; per-theme-branch aliases resolve the ambient theme

**Context:** Fluent theme Spike S1 (2026-07-14, `specs/05-fluent-theme/`) validated the alias mechanisms the planned `FluentTheme` adapter relies on, in the CI host (SimpleSampleApp, `XamlControlsResources` merged at app scope, Skia desktop). Findings are permanently guarded by `src/samples/SimpleSampleApp/RuntimeTests/Given_FluentAliasResolution.cs`.

**What works:** a `<StaticResource x:Key="A" ResourceKey="XcrKey"/>` alias in a dictionary loaded via `ms-appx` Source, merged as a later sibling of `XamlControlsResources`, resolves to the *same instance* as the XCR resource. `BasedOn="{StaticResource XcrKey}"` setters-only styles and FontFamily aliases work the same way. Alias → concrete style in the same merged bundle also works (Simple's `_Resources.xaml` → `Button.xaml` relies on it).

**What does not work:**
1. **Alias chaining** — an alias whose `ResourceKey` targets *another alias* in the same dictionary does not resolve (`"Couldn't statically resolve resource"`, lookup yields null). Every alias must target a concrete resource directly.
2. **Per-theme-branch color aliases** — a `<StaticResource>` alias placed inside `ThemeDictionaries` `Light`/`Default` branches resolves eagerly against the **ambient** theme, so both branches end up with the same value instead of each branch's own. This is the same eager-resolution family as the Fonts.xaml lesson below. Theme-dependent values sourced from another dictionary must be resolved **in code** per branch (see spec 05, decision D6 "mechanism C"), not via per-branch XAML aliases.

**How to apply:** when adding semantic aliases to any `_Resources.xaml`, alias concrete style keys only — never another alias. When mapping theme-varying colors across dictionaries, build the values programmatically per branch. If `Given_FluentAliasResolution.When_AliasOfAlias_DoesNotResolve` or `When_ThemeBranchColorAlias_BranchesResolveAmbientTheme` ever *fail*, the platform constraint has been lifted — revisit spec 05 D6/D16 before assuming either way.

---

## Typography slot aliases resolve their root against the *application* scope — declare the root in the dictionary merged after `SharedTypography.xaml`, and never test an alias cascade from a scoped container

**Context:** PR #1680 (`dev/sb/themes-revert`) reworked `BaseTheme` resource management and CI failed 5 `Given_Fonts` cases: Bold display slots (`DisplayLargeFontFamily`, `DisplayMediumFontFamily`) resolved to `Inter-Regular`, SemiBold slots to Regular/Medium. PR #1710 then collapsed the per-weight families into the single `DefaultFontFamily` root and deleted the Simple `Fonts.xaml` slot re-declarations that #1680's fix had introduced.

**Root cause (#1680, per-weight model):** Simple's `Typography.xaml` mapped the semantic slots to weight-specific keys (`SimpleBoldFontFamily`, …) through `<StaticResource>` aliases inside `ThemeDictionaries`, while `SharedTypography.xaml` declared the same slot keys aliased to the Segoe-derived defaults. An alias is stored as a redirect and its *target* is resolved at lookup time (`ResourceDictionary.TryResolveAlias` → `ResourceResolver.ResolveResourceStatic`), against the by-name scope stack and then the application's top-level resources — not against the dictionary the alias sits in. Which family a slot landed on therefore depended on what the application scope held for the target key. The fix duplicated the slot→weight aliases into Simple's `Fonts.xaml`, merged after `SharedTypography.xaml`, so the theme's mapping won deterministically.

**Why #1710 could delete those duplicates:** with one root, every slot alias in every layer (`SharedTypography.xaml`, Material v2 `Typography.xaml`, Simple `Typography.xaml`) targets the same key, `DefaultFontFamily`. Whichever alias is hit, its target resolves to what the application scope holds for that one key, and the theme's `Fonts.xaml` (merged after `SharedTypography.xaml`) wins for it. The per-slot duplication carried no information any more. What still matters is that the root token is declared in the dictionary merged *after* `SharedTypography.xaml`; `Given_Fonts.When_SimpleThemeLoaded_Then_TypographyScaleDerivesFromRoot` (19 slots) guards that ordering. The per-theme alias blocks in Material v2 and Simple `Typography.xaml` were then deleted too: a lookup that misses a key in one dictionary's HighContrast block continues to the next merged dictionary and then to its `Default` block (`ResourceDictionary.GetThemeDictionary` fallback), so `SharedTypography.xaml`'s aliases serve every appearance. `SharedTypography.xaml` is the only declaration of the `*FontFamily` slot keys; the per-theme Typography files override size, weight and spacing only.

**The sharper lesson (measured in #1710):** because the alias target resolves against the application scope, a `SimpleTheme` merged into a `Grid` with a `FontOverrideDictionary` that redefines `DefaultFontFamily` does **not** cascade — `BodyMediumFontFamily` looked up through that grid still returns the *application* theme's Inter root. The same override on the application-level theme (`Application.Current.GetTheme().FontOverrideDictionary = …`) cascades to every slot; that is the documented scenario and what `Given_Fonts.When_RootOverriddenOnApplicationTheme_Then_ScaleFollowsAndClears` pins. A scoped theme can only swap the scales by declaring the concrete `*FontFamily` keys (no alias), which is what the `DefaultFontFamily` property generator in #1707 does.

**How to apply:**
- Declare the root token in the theme's font dictionary that is merged after `SharedTypography.xaml`; that ordering is what makes the theme's family win over the Segoe UI baseline. Do not re-declare the slot aliases per theme; a design-system-specific key for the root (`SimpleFontFamily`, `CupertinoFontFamily`) is a dead alias: it resolves, but overriding it reaches nothing.
- Never claim "override X cascades" for an aliased key without a test at the scope the doc describes. Container-scoped runtime tests are the wrong scope for alias cascades: they pass or fail on the ambient application theme, not on the container's. Mutate `Application.Current.GetTheme()` and restore it in `finally`.
- A "merge gate" test that measures rendered glyphs must first prove the font loaded: a missing `ms-appx` font falls back silently to the platform default, which has its own Bold, so Bold-vs-Normal alone is green on the very configuration it is meant to catch. Measure against a family known not to exist; equal widths mean both fell back.

**Verification trap (the more important lesson):** these font tests **passed in the minimal dedicated `Uno.Themes.RuntimeTests` host but failed in `SimpleSampleApp`** (and therefore in CI). The dedicated host merges `<SimpleTheme/>` app-wide, which "warms" the ambient resolution scope so the fragile `<StaticResource>` aliases happen to resolve to the right weight — a **false positive**. The real consumer-like host (`SimpleSampleApp`, also what CI runs) exposed the bug.
- **Always verify font/typography/resource-precedence changes in `SimpleSampleApp` (the CI host), not only in a minimal host.** A minimal single-theme host can mask cross-dictionary resolution and merge-order bugs. If two hosts disagree, trust the one that matches CI.
