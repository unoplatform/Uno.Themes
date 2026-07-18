# 05 — ThemesSampleApp: ALC wrapper head hosting the theme sample apps

Status: **in progress** — Phase 1 started 2026-07-18 (reference checkouts restored: `artifacts/uno` @ `21bf1ad6` = exact `6.7.0-dev.815` commit, `artifacts/studio.live` @ `49c33a0`)
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

### Phase 4 — Guest hosting (desktop path)

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

### Phase 5 — Build ordering

- [ ] Wrapper csproj `ProjectReference` to all three heads with `ReferenceOutputAssembly="false" Private="false" SkipGetTargetFrameworkProperties="true" SetTargetFramework="TargetFramework=$(TargetFramework)"` — guests build (matching TFM) before the wrapper, nothing flows into the wrapper's output.
- [ ] Verify a clean-clone `dotnet build ThemesSampleApp.csproj -f net10.0-desktop` builds the guests; assert **no theme dll** (`Uno.Material.WinUI.dll` etc.) lands in the wrapper's `bin` (no-bleed check).
- [ ] Fallback if Uno.Sdk single-project P2P negotiation misbehaves (dual-TFM builds enter each guest twice; XamlMerge/Resizetizer races possible): drop the `ProjectReference`s, rely on solution/CI build order, document "build guests first" for local use.

### Phase 6 — WASM

- [ ] csproj target `_IncludeGuestWasmAssemblies` (browserwasm only; `AfterTargets="ResolveProjectReferences"`, `BeforeTargets="AssignTargetPaths"`): per guest, glob **top-level** `*.dll` from `../<Head>/bin/$(Configuration)/net10.0-browserwasm/browser-wasm/` (fallback path without the RID segment). These are IL dlls; do **not** touch `wwwroot/_framework` (WebCIL bundle, not ALC-loadable). Exclude shared prefixes (`Uno.UI*`, `Uno.dll`, `Uno.Foundation*`, `SkiaSharp*`, `HarfBuzzSharp*`, `System.*`, `Microsoft.Extensions.*`, `Microsoft.UI.*`, `netstandard`, `mscorlib`) but **keep** `MSTest*`/`Microsoft.VisualStudio.TestPlatform.*`. Emit `<Content Link="GuestApps/<AppName>/<FileName>.dll.bin" />` (`.bin` so static hosts don't block `.dll`) + generated `GuestApps/<AppName>/manifest.txt` (one dll name per line via `WriteLinesToFile` into `$(IntermediateOutputPath)`).
- [ ] `GuestAppLoader` browser path: fetch `ms-appx:///GuestApps/<AppName>/manifest.txt`, fetch each `.bin`, write to MEMFS `/guest-apps/<AppName>/<name>`, return that dir — rest identical to desktop.
- [ ] Wrapper browserwasm publish: `<TrimmerRootAssembly Include="Uno.UI" />` (guests may need members the wrapper doesn't reference). `dotnet run` dev flow is untrimmed.
- [ ] WASM end-to-end (Verification 6).

### Phase 7 — Repo wiring

- [ ] `dotnet sln Uno.Themes.sln add src/samples/ThemesSampleApp/ThemesSampleApp.csproj --solution-folder samples` (no `SharedMSBuildProjectFiles` line — no shared-project import).
- [ ] CI: add matrix leg `{ SampleAppName: ThemesSampleApp, ProjectPath: src/samples/ThemesSampleApp }` to `build/stage-build-desktop.yml` and `build/stage-build-wasm.yml` only. No ios/android legs (no mobile TFMs); `build/stage-runtimetests-desktop.yml` unchanged; `.github/workflows/azure-static-web-apps.yml` (deploys SimpleSampleApp) unchanged — switching the public demo to the wrapper is a follow-up.
- [ ] `AGENTS.md`: short orientation note about the wrapper head in the samples section.
- [ ] Conventional commits per group: `chore: bump Uno.Sdk.Private to 6.7.0-dev.815` · `feat(samples): make theme sample heads ALC-hostable` · `feat(samples): add ThemesSampleApp ALC wrapper head` · `ci: build ThemesSampleApp for desktop and wasm`.

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

_(to be completed after implementation)_
