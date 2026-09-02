# Lessons

Domain lessons and postmortems for the Uno.Themes repo. Append new entries at the top.

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

## Typography slot aliases resolve their root against the *application* scope — declare the root in the dictionary merged after `SharedTypography.xaml`, and never test an alias cascade from a scoped container

**Context:** PR #1680 (`dev/sb/themes-revert`) reworked `BaseTheme` resource management and CI failed 5 `Given_Fonts` cases: Bold display slots (`DisplayLargeFontFamily`, `DisplayMediumFontFamily`) resolved to `Inter-Regular`, SemiBold slots to Regular/Medium. PR #1710 then collapsed the per-weight families into the single `DefaultFontFamily` root and deleted the Simple `Fonts.xaml` slot re-declarations that #1680's fix had introduced.

**Root cause (#1680, per-weight model):** Simple's `Typography.xaml` mapped the semantic slots to weight-specific keys (`SimpleBoldFontFamily`, …) through `<StaticResource>` aliases inside `ThemeDictionaries`, while `SharedTypography.xaml` declared the same slot keys aliased to the Segoe-derived defaults. An alias is stored as a redirect and its *target* is resolved at lookup time (`ResourceDictionary.TryResolveAlias` → `ResourceResolver.ResolveResourceStatic`), against the by-name scope stack and then the application's top-level resources — not against the dictionary the alias sits in. Which family a slot landed on therefore depended on what the application scope held for the target key. The fix duplicated the slot→weight aliases into Simple's `Fonts.xaml`, merged after `SharedTypography.xaml`, so the theme's mapping won deterministically.

**Why #1710 could delete those duplicates:** with one root, every slot alias in every layer (`SharedTypography.xaml`, Material v2 `Typography.xaml`, Simple `Typography.xaml`) targets the same key, `DefaultFontFamily`. Whichever alias is hit, its target resolves to what the application scope holds for that one key, and the theme's `Fonts.xaml` (merged after `SharedTypography.xaml`) wins for it. The per-slot duplication carried no information any more. What still matters is that the root token is declared in the dictionary merged *after* `SharedTypography.xaml`; `Given_Fonts.When_SimpleThemeLoaded_Then_TypographyScaleDerivesFromRoot` (19 slots) guards that ordering. The per-theme alias blocks are kept because each appearance block is then self-contained (Material v2 declares a HighContrast block `SharedTypography.xaml` does not), and the file headers say so.

**The sharper lesson (measured in #1710):** because the alias target resolves against the application scope, a `SimpleTheme` merged into a `Grid` with a `FontOverrideDictionary` that redefines `DefaultFontFamily` does **not** cascade — `BodyMediumFontFamily` looked up through that grid still returns the *application* theme's Inter root. The same override on the application-level theme (`Application.Current.GetTheme().FontOverrideDictionary = …`) cascades to every slot; that is the documented scenario and what `Given_Fonts.When_RootOverriddenOnApplicationTheme_Then_ScaleFollowsAndClears` pins. A scoped theme can only swap the scales by declaring the concrete `*FontFamily` keys (no alias), which is what the `DefaultFontFamily` property generator in #1707 does.

**How to apply:**
- Declare the root token in the theme's font dictionary that is merged after `SharedTypography.xaml`; that ordering, not the per-theme alias blocks, is what makes the theme's family win over the Segoe UI baseline.
- Never claim "override X cascades" for an aliased key without a test at the scope the doc describes. Container-scoped runtime tests are the wrong scope for alias cascades: they pass or fail on the ambient application theme, not on the container's. Mutate `Application.Current.GetTheme()` and restore it in `finally`.
- A "merge gate" test that measures rendered glyphs must first prove the font loaded: a missing `ms-appx` font falls back silently to the platform default, which has its own Bold, so Bold-vs-Normal alone is green on the very configuration it is meant to catch. Measure against a family known not to exist; equal widths mean both fell back.

**Verification trap (the more important lesson):** these font tests **passed in the minimal dedicated `Uno.Themes.RuntimeTests` host but failed in `SimpleSampleApp`** (and therefore in CI). The dedicated host merges `<SimpleTheme/>` app-wide, which "warms" the ambient resolution scope so the fragile `<StaticResource>` aliases happen to resolve to the right weight — a **false positive**. The real consumer-like host (`SimpleSampleApp`, also what CI runs) exposed the bug.
- **Always verify font/typography/resource-precedence changes in `SimpleSampleApp` (the CI host), not only in a minimal host.** A minimal single-theme host can mask cross-dictionary resolution and merge-order bugs. If two hosts disagree, trust the one that matches CI.
