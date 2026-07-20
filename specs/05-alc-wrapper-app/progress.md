# 05 — ThemesSampleApp: ALC wrapper head hosting the theme sample apps

Status: **complete** — all seven phases delivered 2026-07-18; see Review at the bottom. (Reference checkouts: `artifacts/uno` @ `21bf1ad6` = exact `6.7.0-dev.815` commit, `artifacts/studio.live` @ `49c33a0` — gitignored, re-clone if absent.)
Branch: `dev/sb/alc-wrapper-app`

## Context

Testing Uno.Themes today means launching a different sample head per design system (`MaterialSampleApp`, `CupertinoSampleApp`, `SimpleSampleApp`). The Uno 6.7-dev runtime (checkout at `artifacts/uno`) ships a secondary-AssemblyLoadContext (ALC) app-hosting feature — proven in production by `artifacts/studio.live` — that lets one Uno app host another in-process:

- The host places a public `Uno.UI.Xaml.Controls.AlcContentHost` (`ContentControl`) in its visual tree and sets the public `Uno.UI.Xaml.WindowHelper.ContentHostOverride` to it.
- The host loads the guest app's assemblies into a **collectible ALC** (Uno framework assemblies shared from the default ALC for type identity), builds a second `UnoPlatformHostBuilder` around the guest's `Application`, and runs it (desktop: dedicated background thread; WASM: `RunAsync`).
- The guest's `Window.Content` is transparently redirected into the `AlcContentHost`.
- Isolation is built into the runtime: per-ALC resource-dictionary registration (guest csproj opt-in `UnoEnableAlcAppSupport`), host-wins resource resolution (guest resources never recolor host chrome), per-guest `RequestedTheme` pinned at the `AlcContentHost` boundary, and a full static-cache purge on unload so the ALC can be collected.

We add **one wrapper head, `src/samples/ThemesSampleApp/`**, that hosts all three theme sample apps: launch one app, pick the theme sample to test. Decisions already made:

- **TFMs**: `net10.0-desktop` + `net10.0-browserwasm`. Basic, out-of-the-box hosting of **pre-built** guest binaries — none of studio.live's content pools / assembly dedup / build offloading / DevServer / HotDesign machinery (that is studio.live-specific).
- **No reference bleed**: the wrapper references **no theme library** and does **not** import `SamplesApp.Shared` (its XAML uses theme namespaces). Hosting/loader code lives in the wrapper head.
- **Uno version**: pin `Uno.Sdk.Private 6.7.0-dev.815` (ALC-capable, available on the `unoplatformdev` feed already configured in `nuget.config`). All three heads move with it (currently `6.5.153`) because guests bind to the host's shared `Uno.UI` at runtime.
- **Guest discovery (desktop)**: probe sibling `src/samples/<Head>/bin/<Config>/net10.0-desktop/` output dirs, plus `ProjectReference` with `ReferenceOutputAssembly=false` for build ordering.
- **Name**: `ThemesSampleApp`.

### Verified technical facts (from `artifacts/uno` @ 6.7-dev and `artifacts/studio.live`)

- `AlcContentHost` (incl. its `ContentChanged` event), `WindowHelper.ContentHostOverride`, and `UnoPlatformHostBuilder` are **public** — the wrapper's main path needs **zero reflection** into Uno internals.
- `Application.Exit()` routes to internal `ExitAlcApplication()` for a non-default-ALC app under `__SKIA__` (`artifacts/uno/src/Uno.UI/UI/Xaml/Application.cs:436-448`). Both wrapper TFMs use `SkiaRenderer`. `ExitAlcApplication` internally runs `Window.CloseAlcWindows()` + `Application.CleanupNonDefaultAlcCaches()` (DP/Style/metadata/resource-resolver/event-subscription sweeps) — the host never calls internals directly.
- `Application.HasSecondaryApps` auto-latches when a guest `Application` constructor registers via `SetCurrentApplication` (`Application.Alc.cs`) — no manual set needed.
- Guest opt-in `UnoEnableAlcAppSupport=true` makes the XAML source generator stamp the owning ALC into `GlobalStaticResources.__ParseContext_` and register all `ms-appx:///` dictionaries **scoped to that ALC**. Standalone (default ALC) behavior is unchanged.
- **Required guest fix**: all three heads do `MainWindow = Microsoft.UI.Xaml.Window.Current;` (`App.xaml.cs:34`). `Window.Current` is a process-wide static in the *shared* `Uno.UI` — hosted, a guest would grab the **wrapper's** window and `InitializeAlcWindowMode` would close the host's real native window. Must become `MainWindow = new Microsoft.UI.Xaml.Window();` (the pattern the reference guest uses; still correct standalone — the first `new Window()` maps to the main window on single-window platforms, and the multi-window guard explicitly allows the hosted case when `ContentHostOverride` is set).
- Guests run **interpreted IL** inside the wrapper (ALC loads IL, no AOT) — WASM uses the guests' *build* output, not publish output.

### Reference implementations (read, don't copy wholesale)

- `artifacts/studio.live/src/uno.studiolive/Presentation/Controls/AppAssemblyLoadContext.cs` — collectible ALC + share-vs-isolate policy (it deliberately loads `Uno.Themes.WinUI`/`Uno.Simple.WinUI` per-ALC).
- `artifacts/studio.live/src/uno.studiolive/Presentation/Controls/AppBinaryLoader.cs` — locate assembly → `Expression`-compiled `Func<Application>` factory → inner `UnoPlatformHostBuilder` boot (never runs the guest's `Program.Main`) → ordered teardown.
- `artifacts/studio.live/src/uno.studiolive/Presentation/Controls/AppBuilderBinaryLoaderModel.cs` — persistent `AlcContentHost` (`EnsureContentHost`).
- `artifacts/uno/src/Uno.UI.RuntimeTests/Tests/AssemblyLoadContext/` — `Given_AlcContentHost.cs` (`StartSecondaryAlcAppAsync`), reference guest `AlcApp/`, `ALC_IMPLEMENTATION.md`; spec: `artifacts/uno/specs/000-alc-secondary-app-support.md`.

---

## Plan

### Phase 1 — SDK bump (gate: validate before any new code) ✅ 2026-07-18

- [x] `global.json` + `src/samples/global.json`: `Uno.Sdk.Private` `6.5.153` → `6.7.0-dev.815` (keep the two files in sync; leave `Uno.Sdk 6.4.53` — the seven shipping library packages are untouched).
- [x] Build each head: `dotnet publish -c Release -f net10.0-desktop -p:TargetFrameworkOverride=desktop` (×3, CI parity) — green. **Warning parity proven**: warning-set diff vs a `6.5.153` baseline build shows zero new warnings (only delta was pre-existing `BaseTheme.cs` CS0618s that the baseline's incremental build skipped re-emitting).
- [x] Launch one head standalone on desktop: Cupertino under Xvfb — shell (nav menu, theme toggle) + Overview page with styled sample cards render correctly (screenshot-verified; click-through navigation not exercised headlessly — runtime tests cover resource/style behavior).
- [x] Runtime tests via `build/scripts/linux-skia-desktop-runtime-tests.sh`: **Material 35/35 passed**, **Simple 93 passed / 1 skipped (pre-existing `[Ignore]` leak-guard in `Given_HotReload.cs`) / 0 failed**. The `Given_Fonts` weight-mapping tests (the `specs/lessons.md` trap) are green under 6.7.

### Phase 2 — Make the theme heads hostable (still fully standalone) ✅ 2026-07-18

- [x] `MaterialSampleApp.csproj`, `CupertinoSampleApp.csproj`, `SimpleSampleApp.csproj`: add `<UnoEnableAlcAppSupport>true</UnoEnableAlcAppSupport>` to the main `PropertyGroup` (per-head, **not** `Directory.Build.props`, so the wrapper doesn't get guest codegen). Verified the property is a `CompilerVisibleProperty` in `Uno.WinUI 6.7.0-dev.815`'s `Uno.UI.SourceGenerators.props`.
- [x] All three heads' `App.xaml.cs` `OnLaunched`: `MainWindow = Microsoft.UI.Xaml.Window.Current;` → `MainWindow = new Microsoft.UI.Xaml.Window();` (same pattern as the reference guest `AlcApp/App.cs`; rationale comment left in code).
- [x] Re-run Phase 1 validation: 3 heads publish clean; Material 35/35, Simple 93/94 (same pre-existing skip, incl. `Given_Fonts`); Cupertino standalone smoke renders identically under the `new Window()` path (screenshot-compared).

### Phase 3 — Wrapper head skeleton ✅ 2026-07-18

- [x] `src/samples/Directory.Packages.props`: add `<PackageVersion Include="Uno.Fonts.Roboto" Version="2.2.2" />` and `<PackageVersion Include="Uno.Fonts.Inter" Version="2.9.0-dev.12" />` (mirror `src/library/Directory.Packages.props:17-18`).
- [x] `src/samples/ThemesSampleApp/ThemesSampleApp.csproj`:
  - `Sdk="Uno.Sdk.Private"`, `OutputType=Exe`, `UnoSingleProject=true`, `UnoFeatures>SkiaRenderer;`.
  - TFMs `net10.0-browserwasm;net10.0-desktop` + the heads' `TargetFrameworkOverride` suffix-expansion block (desktop/browserwasm rows only, plus a `net10.0-desktop` fallback row so an unsupported override — e.g. repo-wide `ios` — never leaves the project TFM-less).
  - `<RootNamespace>Uno.Themes.WrapperApp</RootNamespace>` — samples `Directory.Build.props` otherwise forces the guests' `Uno.Themes.Samples` namespace; wrapper types (incl. its `App`) must not collide with guest type names.
  - PackageReferences: `Uno.Fonts.Roboto`, `Uno.Fonts.Inter` (asset-only, so guest theme fonts resolve; Cupertino uses system fonts). **No** `SamplesApp.Shared` import, theme refs, ShowMeTheXAML, RuntimeTests.Engine, or MSTest.
  - **Deviation from original plan (deliberate):** the wrapper also sets `<UnoEnableAlcAppSupport>true</UnoEnableAlcAppSupport>`. The 6.7 Uno linker substitutions (`LinkerSubstitution.{Skia,Wasm}.xml`, embedded for Release trimming via `Uno.UI.Tasks.targets:280-291`) stub `Application.get_HasSecondaryApps()` to constant `false` when that linker feature is off — which would disable per-ALC resource resolution in a **trimmed WASM Release publish** of the wrapper regardless of the runtime auto-latch; `TrimmerRootAssembly` cannot prevent body substitution. Harmless otherwise (wrapper has no theme XAML).
- [x] `App.xaml` (theme-free: `XamlControlsResources` only) + `App.xaml.cs` (`MainWindow = new Window()`, content = `MainPage`, DEBUG logging copied from heads, explicit `Window.Title`).
- [x] `Platforms/Desktop/Program.cs`, `Platforms/WebAssembly/Program.cs` (+ `LinkerConfig.xml`, `manifest.webmanifest`, **`WasmScripts/AppManifest.js`** — required: `Uno.Resizetizer`'s `GenerateWasmSplashAssets` fails with MSB4181 without it), `Properties/launchSettings.json`, `app.manifest` — copied from `SimpleSampleApp`, wrapper namespace.
- [x] Builds: desktop 0 warnings; browserwasm green with only the same transitive `NU1903` advisories (`System.Security.Cryptography.Xml 10.0.5`) every head's wasm build emits under this .NET SDK. Desktop empty shell renders (screenshot-verified under Xvfb).

### Phase 4 — Guest hosting (desktop path) ✅ 2026-07-18 (see Findings below)

- [ ] `GuestHosting/GuestAppCatalog.cs`: `record GuestAppInfo(string DisplayName, string ProjectFolderName, string AssemblyName)` + static list (Material / Cupertino / Simple). Catalog-driven assembly names — no runtimeconfig heuristics needed.
- [ ] `GuestHosting/GuestAssemblyLoadContext.cs` — collectible (`isCollectible: true`, name `GuestALC-<App>-<Guid>`); `Load(AssemblyName)`:
  1. Already loaded in Default ALC by simple name → share (covers BCL, `Microsoft.Extensions.*`, Uno framework).
  2. Explicit share list → `AssemblyLoadContext.Default.LoadFromAssemblyName` (fall through on failure):
     - equals: `Uno.UI`, `Uno`, `Uno.UI.Composition`, `Uno.Foundation`, `Uno.Foundation.Logging`, `Uno.UI.Dispatching`, `Uno.WinUI.Graphics2DSK`, `Microsoft.CSharp`
     - starts-with: `Uno.UI.Runtime.`, `Uno.UI.FluentTheme`, `SkiaSharp`, `HarfBuzzSharp`, `System`, `Microsoft.Extensions.`, `netstandard`, `mscorlib`
  3. Else guest directory — **`LoadFromStream` on desktop** (no file locks; heads can be rebuilt while the wrapper runs), **`LoadFromAssemblyPath` on WASM** (MEMFS). This is where `Uno.Themes.WinUI`, `Uno.{Material,Cupertino,Simple}.WinUI`, `Uno.ShowMeTheXAML*`, `MSTest*`, and the head assembly land — per-ALC by construction.
  4. Not found → `null`. Guard with a disposed flag.
  - ⚠️ Do **not** blanket-share `Uno.*` (the runtime-test harness does; it would try to share theme libs the wrapper doesn't have).
- [ ] `GuestHosting/GuestAppLoader.cs` — owns at most one `Session` (Alc, GuestApp, ExecutionTask, ExecutionThread?, Info); `LoadAsync` / `UnloadAsync` / `SwitchToAsync` / `ReloadAsync`:
  - **Locate (desktop)**: try `AppContext.BaseDirectory/GuestApps/<ProjectFolderName>` first (future self-contained layout), then walk up from `AppContext.BaseDirectory` to the `samples` dir and probe `<Head>/bin/{ownConfig, Debug, Release}/net10.0-desktop/`; on multiple hits pick the newest `LastWriteTimeUtc` of `<AssemblyName>.dll`; friendly status-surface error if missing ("Build MaterialSampleApp for net10.0-desktop first").
  - **Boot**: load main assembly → find the `Application`-derived type → `Expression.Lambda<Func<Application>>(Expression.New(ctor)).Compile(preferInterpretation: true)` (AOT-safe), wrapped to capture the created instance into the session → `UnoPlatformHostBuilder.Create().App(factory)`; `#if __WASM__` `.UseWebAssembly()` else `.UseX11().UseLinuxFrameBuffer().UseMacOS().UseWin32()` → desktop: `new Thread(() => host.Run()) { IsBackground = true, Name = "GuestApp-Main" }`; WASM: `Task.Run(() => host.RunAsync())`. Await first `AlcContentHost.ContentChanged` with ~30 s timeout; surface failure.
  - **Teardown (ordered, minimal)**:
    1. UI thread: `contentHost.Content = null` (releases guest visual tree + projected resources).
    2. Await execution task ~5 s; desktop: if `GuestApp-Main` still alive, `Thread.Interrupt()` + `Join(2 s)`, retry once; if still stuck → surface error and **skip** ALC unload this cycle (never unload a running ALC).
    3. UI thread: `session.GuestApp.Exit()` (→ `ExitAlcApplication()`); tiny reflection fallback to `ExitAlcApplication` only if `Exit()` no-ops.
    4. Null the whole session; `alc.Unload()` off the UI thread; `GC.Collect(); GC.WaitForPendingFinalizers(); GC.Collect();` (release-before-allocate — WASM `memory.grow` is irreversible).
    5. `WindowHelper.ContentHostOverride` keeps pointing at the persistent `AlcContentHost` for the next load.
  - Repo rules: `async void` only on XAML event handlers with full-body `try/catch`; `EventHandler<TEventArgs>` for any events; nullable-clean; structured logging.
- [ ] `MainPage.xaml(.cs)`: app-picker buttons from the catalog + `Unload`/`Reload`, `InfoBar` status/error surface, `Border` guest region; create the `AlcContentHost` **once**, set `WindowHelper.ContentHostOverride`, reassert before each load.
- [ ] Desktop end-to-end passes (see Verification 3–4).

#### Phase 4 findings (deviations + leak investigation)

Implemented as planned (`GuestAppCatalog` / `GuestAssemblyLoadContext` / `GuestAppLoader` / `MainPage` picker), with these deviations and additions, all verified end-to-end under Xvfb (screenshots + heap dumps in session scratchpad):

- **Boot**: `Expression.Lambda<Func<Application>>` factory (non-generic — a `Func<TApp>` shared-generic dictionary entry would pin the collectible ALC), wait for first **non-null** `AlcContentHost.ContentChanged` with a fail-fast race against the run-loop task. `WaitForSecondaryWindowAsync`-style 30 s budget.
- **Teardown order** (differs from plan): clear content → stop run loop (5 s wait, then `Thread.Interrupt` + 2 s/3 s joins; skip unload if stuck) → `guestApp.Exit()` on host UI thread (no reflection fallback needed — public `Exit()` routes to `ExitAlcApplication` on Skia) → restore `BindableMetadata.Provider` (guests null the process-wide provider; same hygiene as Uno's own runtime tests) → `alc.Unload()` → finalizer drain → **post-unload sweeps** → `GC.Collect`.
- **ALC reclamation required three wrapper-side mitigations for Uno 6.7-dev sweep gaps** (each found via `dotnet-dump` `gcroot` census, each verified by before/after soak):
  1. `DependencyProperty._getPropertyCache` memoizes `(targetType, "ns:Owner.Property")` → DP; a guest style targeting an attached property on a framework element caches a **default-ALC key with a guest-ALC value**, which `RemoveNonDefaultAlcEntries` (key-ALC check only) can never remove. Wrapper clears this pure cache via reflection post-unload. **Upstream fix needed** (check value ALC too).
  2. `SystemNavigationManager._backRequested`/`InternalBackRequested`: the samples' `Shell` subscribes and nothing unsubscribes on ALC teardown (`PruneCollectibleAlcEventSubscriptions` does not cover this singleton), rooting the whole guest visual tree. Wrapper prunes guest-ALC handlers via reflection. **Upstream fix needed.**
  3. Re-running `Application.CleanupNonDefaultAlcCaches` after finalizers drain (guest `DependencyObject` finalizers can re-populate swept caches during unload).
- **Loader diagnostics are Release-visible by design** (wrapper logging is not `#if DEBUG` like the heads): teardown branch logging + a `WeakReference` check that logs "Previous guest ALC was fully collected" / "still alive" on each load. The Release/Debug JIT-timing difference masked the leak in Debug builds — only the weak-ref telemetry makes regressions visible.
- **Verified**: 16-cycle Release soak — previous ALC collected **15/15**, managed heap flat at ~32 MB (`eeheap`).
- **Known limitation (upstream)**: each guest window create/close leaks its **native** X11 GL context (~12-15 MB native + 7 llvmpipe threads per cycle; llvmpipe group count grows 1:1 with cycles). Reproduces with and without an explicit `Window.Close()` before `Exit()` — the leak is in Uno's ALC guest-window native teardown, not reachable from the wrapper. Managed side is fully reclaimed. **Upstream issue to file.**
- The stale-guest-build failure mode (`ReflectionTypeLoadException` from mixed-version theme dlls in a head's bin) is wrapped with an actionable "rebuild the head" message.

### Phase 5 — Build ordering ✅ 2026-07-18 (desktop only — wasm fell back, see below)

- [x] Wrapper csproj `ProjectReference` to all three heads with `ReferenceOutputAssembly="false" Private="false" SkipGetTargetFrameworkProperties="true" SetTargetFramework="TargetFramework=$(TargetFramework)"` — guests build (matching TFM) before the wrapper, nothing flows into the wrapper's output. **Gated on a desktop `TargetFrameworkOverride`**: with no override, referencing the guests drags their full android/ios restore into the wrapper build (NETSDK1147 without mobile workloads) — same constraint as the rest of the repo's local workflow.
- [x] Clean-wipe `dotnet build ThemesSampleApp.csproj -f net10.0-desktop -p:TargetFrameworkOverride=desktop` builds all three guests via P2P; **no theme/ShowMeTheXAML/MSTest dll** lands in the wrapper's `bin` (no-bleed check green); warning set identical to the heads' own pre-existing warnings.
- [x] **Fallback exercised for browserwasm**: the StaticWebAssets SDK merges referenced projects' web assets regardless of `ReferenceOutputAssembly=false`, and the heads' identical `WasmCSS/Fonts.css` collide ("Conflicting assets with the same target path"). The wasm leg therefore carries **no** P2P refs — the guest-payload target warns when guest wasm bins are missing, and the CI wasm leg builds the three heads before the wrapper (Phase 7).

### Phase 6 — WASM ✅ 2026-07-18 (browser boot verification deferred to CI/user — no browser in the dev environment)

- [x] csproj target `_IncludeGuestWasmAssemblies` (browserwasm only; `AfterTargets="ResolveProjectReferences"`, `BeforeTargets="AssignTargetPaths"`): per guest, glob **top-level** `*.dll` from the head's wasm bin (RID dir probed first; actual 6.7 layout has **no** `browser-wasm` RID segment). Exclusions extended beyond plan (`Microsoft.Win32*`, `Microsoft.VisualBasic*` — runtime-pack facades that tier-1 sharing covers), trimming payload from ~44 to 16-17 dlls (~10 MB/guest). `MSTest*`/`Microsoft.VisualStudio.TestPlatform.*` kept. Manifest emitted per app via `WriteLinesToFile` + explicit per-app `Content Link` (`%(RecursiveDir)` capture produced colliding links — Uno.Wasm.Bootstrap "incompatible asset kinds" error). Emits a **build warning** naming the exact command when a guest's wasm bins are missing (replaces the dropped wasm P2P ordering).
- [x] `GuestAppLoader` browser path: fetch `ms-appx:///GuestApps/<AppName>/manifest.txt` + each `.bin` via `StorageFile.GetFileFromApplicationUriAsync`, write to MEMFS `/guest-apps/<AppName>/`, reuse cached payload on reload; friendly `GuestAppLoadException` when the payload is absent from the build.
- [x] `TrimmerRootAssembly`: `Uno.UI` **+ `Uno` + `Uno.Foundation`** (the loader's reflection targets and guests' shared framework surface live across all three).
- [x] Verified: browserwasm build green; payload present under `wwwroot/package_<hash>/GuestApps/<App>/` (the package `index.html` references) with gzip precompression; manifest + head dll + theme dll all fetch **HTTP 200** through a static server. CI-parity trimmed publish validated. **In-browser guest boot not verified locally** (no browser available headlessly) — covered by the CI wasm leg artifact + manual verification; the desktop path proves the loader logic itself.

### Phase 7 — Repo wiring ✅ 2026-07-18

- [x] Added to `Uno.Themes.sln` under the `samples` solution folder, **plus solution-level `ProjectDependencies` on the three heads** so IDE/solution builds order the guests before the wrapper (the csproj P2P only covers override-driven CLI builds).
- [x] CI: `Themes` matrix leg added to `build/stage-build-desktop.yml` (P2P builds the guests) and `build/stage-build-wasm.yml` (explicit guest-head build step gated by a `BuildGuestHeads` matrix variable, since wasm carries no P2P). No ios/android legs; `build/stage-runtimetests-desktop.yml` unchanged; `azure-static-web-apps.yml` unchanged — switching the public demo to the wrapper is a follow-up.
- [x] `AGENTS.md`: orientation note in the samples section (wrapper design, no-bleed rule, build-the-heads-first workflow, spec pointer).
- [x] Conventional commits per group (finer-grained than planned): `chore:` SDK bump · `feat(samples):` heads ALC-hostable · `feat(samples):` wrapper skeleton · `feat(samples):` guest hosting · `feat(samples):` ordering P2P · `feat(samples):` wasm payload · `ci:` + `chore(samples):` wiring.

---

## Risks

- **6.5.153 → 6.7.0-dev.815**: heads' implicit `Uno.WinUI.*` packages jump two dev-minor versions. Expect at most NU1608-class unification warnings (libraries reference lower Uno.WinUI); watch for theme-style/rendering diffs and `Uno.ShowMeTheXAML 2.0.0-dev0015` / `Uno.UI.RuntimeTests.Engine 2.0.0-dev.60` binary compat. Runtime tests are the gate. Dev packages can be evicted from `unoplatformdev` — re-pin to a stable 6.7 once released.
- **Dual-TFM wrapper build** enters each guest project twice with different global properties; if per-project targets race, use the Phase 5 fallback.
- **`new Window()` migration** changes head startup on all four TFMs — covered by the existing CI matrix + standalone smoke.
- **Known v1 limitations** (accepted): guest satellite assemblies and `Assets/**` are not carried on WASM (neutral-language strings; some guest images may 404 — fonts mitigated via wrapper font packages); WASM ALC unload can leave residual roots (functionally harmless; matches studio.live behavior); the desktop wrapper output is not self-contained (probes sibling bins — the `GuestApps/` probe path is the seam for a future packaged layout).

## Verification

1. After Phase 1: three heads build (desktop), one launches standalone, Material + Simple runtime tests green via `build/scripts/linux-skia-desktop-runtime-tests.sh`.
2. After Phase 2: repeat 1 (regression gate: heads remain fully standalone).
3. Wrapper desktop build builds guests via P2P; **no theme dll in wrapper output** (no-bleed check).
4. Launch wrapper on desktop: load Material → shell renders with Material styling inside the host region; switch Cupertino → Simple → Material (ALC re-registration smoke); `Unload` empties the region; `Reload` works; errors surface in the `InfoBar` (e.g. remove a guest bin dir and try loading it).
5. Informal memory smoke: repeated load/unload cycles, no unbounded process growth.
6. WASM: `-f net10.0-browserwasm` build contains `GuestApps/*/manifest.txt` + `.bin` payloads; `dotnet run` the wasm head, load each guest in the browser, switch/unload.
7. CI: two new legs green; existing ios/android/wasm/desktop legs of the three heads green under 6.7.

## Review

**Delivered 2026-07-18** on `dev/sb/alc-wrapper-app`, seven phases, all verification gates run (desktop e2e exercised under Xvfb with synthetic X11 input; wasm verified to CI-parity publish + HTTP payload fetch — in-browser boot is the one item deferred to CI/manual verification since the dev environment has no browser).

**What shipped**
- `Uno.Sdk.Private 6.7.0-dev.815` across both `global.json`s; heads' standalone behavior proven unchanged (zero new build warnings vs 6.5.153 baseline; Material 35/35 + Simple 93/94 runtime tests at every gate incl. branch tip; Cupertino pixel-compared smoke).
- Heads made hostable with exactly two accommodations: `UnoEnableAlcAppSupport` (verified in generated code: per-ALC parse-context stamp + scoped `ms-appx` registration) and `new Window()` instead of `Window.Current`.
- `ThemesSampleApp` wrapper head (desktop+wasm, theme-free, no shared-project import) hosting all three heads in-process: picker UI, InfoBar status/error surfacing, load/switch/unload/reload all verified, friendly errors for missing/stale guest builds.
- Full ALC reclamation on desktop: 15/15 collected across a 16-cycle soak, managed heap flat (~32 MB), with per-cycle collection telemetry built into the loader.

**Deviations from plan** (each documented in-phase): wrapper also sets `UnoEnableAlcAppSupport` (trimmed-publish linker substitution would hard-code `HasSecondaryApps=false`); `WasmScripts/AppManifest.js` required by Resizetizer; wasm P2P replaced by build-time warning + explicit CI guest builds (StaticWebAssets collision); three reflection-based teardown sweeps compensating Uno 6.7-dev per-ALC cleanup gaps; sln-level ProjectDependencies added.

**Upstream issues to file against unoplatform/uno (6.7-dev ALC support)**
1. `NameToPropertyDictionary.RemoveNonDefaultAlcEntries` misses cross-ALC entries (default-ALC key type, guest-ALC DP value) — pins guest ALCs via `_getPropertyCache`.
2. ALC teardown does not prune `SystemNavigationManager` event subscriptions — a guest Shell's `BackRequested` handler roots its whole visual tree.
3. Guest finalizers during unload re-populate property-system caches after `ExitAlcApplication`'s sweep.
4. Native X11 window/GL context (+ render threads) leak per ALC-guest window create/close cycle (~12-15 MB native/cycle; managed side fully reclaimed). Reproduces with and without an explicit pre-Exit `Window.Close()`.

**Accepted v1 limitations**: the native leak above (bounded by switch count in a dev tool); guest `Assets/**`/satellites not carried (some guest images may 404 — fonts covered by wrapper font packages); desktop wrapper output not self-contained (probes sibling bins; `GuestApps/` probe is the packaged-layout seam); single guest at a time by design.

**Post-review fixes (2026-07-20)** — a seven-lens review panel (verdict: fix-first, nothing block-merge) was applied in full except three tracked follow-ups. Fixed: late-guest-content race on the load-timeout teardown path (re-clear + verify before unload); stuck-run-loop is now a surfaced, latched terminal state (`_faulted` — hosting disabled until restart, no more false "unloaded" success) with the binding-provider restore moved ahead of the early-out; WASM unload no longer burns a fixed 5 s (run loop observed after `Exit()`, where it can actually complete); a partial WASM payload download can no longer poison the MEMFS cache (`.partial` staging + rename, cleanup on failure); payload fetch streams instead of double-buffering each dll; the post-unload sweep dispatch result is checked and logged; per-sweep isolation + a not-found warning on the nav-handler prune; UI dispatches that time out are flagged so they can't run late against an unloading ALC; the wasm payload-exclusion filter is now exactly the ALC-shareable set (**fixes `Uno.UI.Lottie` being stranded on wasm** — neither shipped nor shareable; `Microsoft.Win32*`/`Microsoft.VisualBasic*`/`Uno.UI.Adapter.*` added to the ALC share prefixes to keep every exclusion resolvable) with reciprocal keep-in-sync comments; desktop sibling-bin probe anchored on `SamplesApp.Shared` (no DLL execution from arbitrary same-named trees); locate-before-teardown (a click on a missing guest no longer destroys the running session); `Reload` targets only accepted requests; tier-1 ALC resolution uses an invalidation-cached name map; manifest entries validated against path separators; reflection lookups can no longer crash type initialization; guest-list sync sites documented at the catalog; catalog types made internal. Re-verified after the fixes: desktop e2e (Material/Cupertino/Simple, unload, reload) + 3-cycle soak with per-cycle ALC collection, desktop and wasm builds clean, Lottie present in all three wasm payload manifests.

**Follow-ups (not applied)**: file the four upstream unoplatform/uno issues and replace the spec-pointer comments at the loader's reflection sites with the issue URLs; add a scripted desktop hosting smoke to CI (the legs only build/publish today); split `GuestAppLoader` into platform partials when next touched.
