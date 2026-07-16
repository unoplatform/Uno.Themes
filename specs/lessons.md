# Lessons

Domain lessons and postmortems for the Uno.Themes repo. Append new entries at the top.

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

2. **Generated semantic brushes materialize once, against the app-level scope.** `SharedColors.xaml` defines `<SolidColorBrush Color="{StaticResource PrimaryColor}"/>`; that color reference is a one-time resolution (already documented atop `Given_ColorOverridePrecedence`). Under a container-scoped theme in a host with a different app-level theme, `PrimaryBrush` & co. carry the *app-level* theme's palette, even though the `*Color` keys resolve correctly from the container. This is a pre-existing property of the shared brush layer (Simple/Material behave identically), not a FluentTheme bug.

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

## Typography slot→weight font mappings must be duplicated in Fonts.xaml (not only Typography.xaml)

**Context:** PR #1680 (`dev/sb/themes-revert`) — reworking `BaseTheme` resource management. CI runtime tests failed with 5 `Given_Fonts` cases: Bold display slots (`DisplayLargeFontFamily`, `DisplayMediumFontFamily`) resolved to `Inter-Regular` instead of `Inter-Bold`, and SemiBold slots (`HeadlineMediumFontFamily`, `TitleMediumFontFamily`, `LabelLargeFontFamily`) resolved to `Inter-Regular`/`Inter-Medium` instead of `Inter-SemiBold`.

**Root cause:** Simple's `Typography.xaml` maps the semantic font-family slots via `<StaticResource ResourceKey="SimpleBoldFontFamily" />` etc. `SharedTypography.xaml` (Uno.Themes core) *also* defines the same `*FontFamily` keys, aliased to `TypefacePlain`/`TypefaceBrand` (the Segoe-derived defaults). `<StaticResource>` aliases inside `ResourceDictionary.ThemeDictionaries` are resolved **eagerly at parse time** against whatever is visible in scope then — across separate merged dictionaries this resolution is unreliable, so the shared (wrong-weight) defaults can win. Master's fix was to **also** declare the slot→weight `<StaticResource>` mappings in `Fonts.xaml` (which is merged *after* `SharedTypography.xaml` inside `BaseDictionaries.xaml`), making the correct weights win deterministically. A reshape of `Fonts.xaml` deleted those duplicated mappings, reintroducing the bug.

**How to apply:**
- When a per-design-system typography file maps font-family slots to weight-specific keys, keep the matching mappings in the **font dictionary that is merged after `SharedTypography.xaml`** (e.g. Simple's `Fonts.xaml`). Do not assume the aliases in `Typography.xaml` alone are sufficient — they are not, because of eager cross-dictionary `<StaticResource>` resolution in theme dictionaries.
- Treat the slot→weight mappings in `Fonts.xaml` as load-bearing, not redundant. The comment in that file explains why; preserve it on any refactor.

**Verification trap (the more important lesson):** these font tests **passed in the minimal dedicated `Uno.Themes.RuntimeTests` host but failed in `SimpleSampleApp`** (and therefore in CI). The dedicated host merges `<SimpleTheme/>` app-wide, which "warms" the ambient resolution scope so the fragile `<StaticResource>` aliases happen to resolve to the right weight — a **false positive**. The real consumer-like host (`SimpleSampleApp`, also what CI runs) exposed the bug.
- **Always verify font/typography/resource-precedence changes in `SimpleSampleApp` (the CI host), not only in a minimal host.** A minimal single-theme host can mask cross-dictionary resolution and merge-order bugs. If two hosts disagree, trust the one that matches CI.
