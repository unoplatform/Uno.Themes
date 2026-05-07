# 037 — Hot Reload Local Repro

## Plan

- [x] Identify the local runtime-test command that matches CI for Uno.Themes sample apps.
- [x] Run the targeted hot reload test locally against the desktop sample app.
- [x] Inspect logs for dev-server workspace loading and metadata update flow.
- [x] Record whether the failure reproduces locally and capture the likely cause.

## Review

- Reproduced the CI-like desktop hot-reload runtime-test flow locally for `SimpleSampleApp`:

	```bash
	dotnet publish -c Debug -f net10.0-desktop -p:TargetFrameworkOverride=desktop -p:PublishTrimmed=false src/samples/SimpleSampleApp/SimpleSampleApp.csproj

	export ProjectPath='src/samples/SimpleSampleApp'
	export SampleAppName='SimpleSampleApp'
	export BUILD_CONFIG='Debug'
	export UNO_RUNTIME_TESTS_RUN_TESTS='{"Filter":{"Value":"HotReload"}}'
	export UNO_RUNTIME_TESTS_OUTPUT_PATH='/tmp/runtime-tests-hotreload-SimpleSampleApp.xml'
	export DOTNET_MODIFIABLE_ASSEMBLIES=debug
	build/scripts/linux-skia-desktop-runtime-tests.sh
	```

- The primary runtime-test app started successfully.
- The runtime-tests engine spawned:
	- a dev-server host (`Uno.UI.RemoteControl.Host.dll --httpPort 41253 --metadata-updates true`)
	- a secondary `SimpleSampleApp` instance for `[RunsInSecondaryApp]`
- The secondary app received the expected environment:
	- `UNO_DEV_SERVER_HOST=127.0.0.1`
	- `UNO_DEV_SERVER_PORT=41253`
	- `DOTNET_MODIFIABLE_ASSEMBLIES=debug`
	- `UNO_RUNTIME_TESTS_IS_SECONDARY_APP=true`
- The secondary app result file stayed empty (`/tmp/tmpWC714X.tmp`) while the run remained blocked past the 180s workspace timeout budget.
- The main app logs showed an early remote-control configuration error:

	```text
	Some endpoint for uno's dev-server has been configured in your application, but all are invalid (port is missing?).
	```

- The generated Debug MSBuild editorconfig confirms build-time remote-control host/port are empty in the sample app artifact:
	- `UnoRemoteControlPort =`
	- `UnoRemoteControlHost =`
	- `UnoRemoteControlProcessorsPath = /home/developer/.nuget/packages/uno.winui.devserver/6.5.153/buildTransitive/../tools/rc/processors`

- Conclusion: the hot-reload runtime-test does not pass locally in this isolated environment. The failure reproduces as a hang before results are written, with the strongest local signal being incomplete/invalid remote-control endpoint configuration combined with a workspace-load stall.

- After adding targeted diagnostics in the shared HR test and enabling runtime-test debug filters in the sample apps, the local repro became conclusive:
	- The runtime-tests engine starts the dev-server and the secondary app correctly.
	- The secondary app connects to the dev-server and receives the hot-reload processors.
	- The dev-server logs `Base project path: /studio-live/artifacts/Uno.Themes/src/samples/SimpleSampleApp/SimpleSampleApp.csproj`.
	- The secondary app logs show `ClientHotReloadProcessor` failures while the workspace is being compiled, including many `UXAML0001` errors from shared sample XAML files.
	- The first hard compiler failure in the captured output is:

		```text
		error CS7038: Failed to emit module 'SimpleSampleApp': Changing the version of an assembly reference is not allowed during debugging: 'Uno.Simple.WinUI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null' changed version to '0.0.0.0'.
		```

	- So the key issue is not the secondary app failing to connect to the dev-server. The connection succeeds; the hot-reload delta/workspace compilation fails afterward.

	- Additional isolation/fix attempts and outcomes:
		- Attempted to force deterministic assembly metadata on `Uno.Simple.WinUI` by explicitly setting:
			- `<Version>1.0.0</Version>`
			- `<AssemblyVersion>1.0.0.0</AssemblyVersion>`
			- `<FileVersion>1.0.0.0</FileVersion>`
		- Attempted to reduce head-project influence in Debug `SimpleSampleApp` by setting:
			- `<UnoForceIncludeProjectConfiguration>false</UnoForceIncludeProjectConfiguration>`
			- `<UnoForceIncludeServerProcessorsConfiguration>false</UnoForceIncludeServerProcessorsConfiguration>`
		- Re-ran the same CI-like hot-reload runtime-test flow after those changes.
		- Result: no behavioral change. The run still reaches secondary app + dev-server handshake, then fails during HR compilation with the same pattern:
			- repeated `UXAML0001` errors in `SamplesApp.Shared`
			- same terminal hard failure `CS7038` for `Uno.Simple.WinUI` version drift `1.0.0.0 -> 0.0.0.0`

	- Additional runtime observations from verbose logs:
		- `ServerHotReloadProcessor` still reports base project path as `SimpleSampleApp.csproj`.
		- The transient `/tmp/DevServer_*` workspace used for package bootstrap is short-lived and not reliably inspectable post-run.
		- The failure is consistently in the hot-reload compile/update stage, not in transport or processor discovery.

## Final root cause analysis (2026-05-05)

After re-running the CI-equivalent flow with verbose diagnostics enabled (clean rebuild, branch HEAD `dv/sb/themes-hr-harness`, `Uno.Sdk 6.5.31`, `Uno.Sdk.Private 6.5.153`), three independent bugs combine to make the test impossible to pass.

### Bug 1 — wrong file path argument in the test (cosmetic, but masks 2/3)

`HotReloadHelper.UpdateSourceFile(filPathRelativeToProject, …)` resolves `projectDir` from `[RuntimeTestsSourceProjectAttribute]` baked into the assembly that hosts the `[TestClass]` (i.e. `Uno.Themes.Samples.RuntimeTests.dll`). The generated attribute is:

```text
[assembly: RuntimeTestsSourceProject(".../src/samples/Uno.Themes.Samples.RuntimeTests/Uno.Themes.Samples.RuntimeTests.csproj")]
```

The test passes a **repo-rooted** path:

```csharp
HotReloadHelper.UpdateSourceFile(
    "src/samples/Uno.Themes.Samples.RuntimeTests/HotReload/HotReloadTarget.cs", …);
```

`Path.Combine` then produces `/.../Uno.Themes.Samples.RuntimeTests/src/samples/Uno.Themes.Samples.RuntimeTests/HotReload/HotReloadTarget.cs` — duplicated. Dev-server logs:

```text
Requested file '.../Uno.Themes.Samples.RuntimeTests/src/samples/Uno.Themes.Samples.RuntimeTests/HotReload/HotReloadTarget.cs' does not exists.
```

Fix: pass `"HotReload/HotReloadTarget.cs"` (relative to the runtime-tests project). Inherited from the original commit `d6207447` (test was in `SimpleSampleApp`, repo path was `src/samples/SimpleSampleApp/RuntimeTests/HotReload/HotReloadTarget.cs` — already double-pathed, just hidden by the test and head living in the same directory).

### Bug 2 — MSBuildWorkspace TargetFrameworks propagation breaks library evaluation (real blocker)

Decompiled `Uno.UI.RemoteControl.Server.Processors.dll`, method `ServerHotReloadProcessor.CreateCompilation`:

```csharp
properties["UnoIsHotReloadHost"] = "True";
…
if (properties.Remove("TargetFramework", out var targetFramework))
{
    properties["UnoHotReloadTargetFramework"] = targetFramework;
    properties["TargetFrameworks"] = targetFramework;          // <— propagates as global
}
await CompilationWorkspaceProvider.CreateWorkspaceAsync(
    configureServer.ProjectPath, _reporter, capabilities, properties, ct);
```

These end up as **MSBuild global properties** for the entire workspace (head + every transitive `ProjectReference`).

The compensating mechanism is `Uno.WinUI.DevServer.props`:

```xml
<PropertyGroup Condition="'$(UnoIsHotReloadHost)'=='true'">
  <RuntimeIdentifier Condition="'$(UnoHotReloadRuntimeIdentifier)' != ''">$(UnoHotReloadRuntimeIdentifier)</RuntimeIdentifier>
  <TargetFramework Condition="'$(UnoHotReloadTargetFramework)' != ''">$(UnoHotReloadTargetFramework)</TargetFramework>
</PropertyGroup>
```

It collapses multi-TFM projects to a single `TargetFramework`. **But this only fires for projects that reference `Uno.WinUI.DevServer`** (which loads `DevServer.props`).

In the Uno.Themes graph, only the secondary harness (`Uno.Themes.Samples.RuntimeTests.csproj`, Debug-only) and the sample app head reference `Uno.WinUI.DevServer`. The libraries (`Uno.Themes.WinUI`, `Uno.Simple.WinUI`, `Uno.Material.WinUI`, `Uno.Cupertino.WinUI`) **do not** — and they target `net9.0` exclusively via `tfm-common-winui.props`.

Verified the propagation effect directly:

```bash
dotnet msbuild src/library/Uno.Simple.WinUI/Uno.Simple.WinUI.csproj \
  -p:TargetFrameworks=net10.0-desktop -p:UnoIsHotReloadHost=True \
  -getProperty:TargetFrameworks,TargetFramework
# → TargetFrameworks: "net10.0-desktop"   (lib's own `<TargetFrameworks>net9.0</TargetFrameworks>` is silently dropped)
```

So under the dev-server, Uno.Simple.WinUI evaluates as `net10.0-desktop`, but its WinUI props/targets and ProjectReferences (`Uno.Themes.WinUI`) are wired only for `net9.0`. MSBuildWorkspace then logs the cascade:

```text
[Failure] Project '../Uno.Themes/Uno.Themes.WinUI.csproj' targets 'net10.0-desktop'.
          It cannot be referenced by a project that targets '.NETCoreApp,Version=v10.0'.
[Warning] Found project reference without a matching metadata reference:
          .../src/library/Uno.Themes/Uno.Themes.WinUI.csproj
```

…then, during compilation, the missing references manifest as ~2000 lines of:

- `CS0234: namespace 'UI' does not exist in 'Microsoft'` (Uno.Material/Uno.Cupertino/Uno.Themes source files lose `Microsoft.UI.Xaml` types)
- `CS0246: AssemblyMetadataAttribute / Control / IconElement / IValueConverter / BindableAttribute could not be found`
- `UXAML0001: Cannot assign child of type 'Microsoft.UI.Xaml.Controls.FontIcon' to property of type 'IconElement'` in `SamplesApp.Shared/Content/Controls/ButtonSamplePage.xaml` (×30+) — XAML codegen can't resolve types when the referenced library is missing.

A `TreatAsLocalProperty="TargetFrameworks"` workaround on `<Project>` does *partially* unblock evaluation:

```bash
# With TreatAsLocalProperty="TargetFrameworks" → "net10.0-desktop;net9.0;"
```

…but the lib still picks `net10.0-desktop` first (no `net9.0-desktop` definition; the `_IsWinUI` check yields wrong defines), so it is not a complete fix. The real fix has to come from one of:

1. **Dev-server side** (`Uno.UI.RemoteControl.Server.Processors`): stop propagating `TargetFrameworks` as a global property; instead set it only on the head project's load options.
2. **Library side**: make the libs reference `Uno.WinUI.DevServer` privately so `DevServer.props` applies and reverses the global property — non-trivial, ships infra into shipped packages.
3. **Library side**: add `<TreatAsLocalProperty>TargetFramework;TargetFrameworks;RuntimeIdentifier</TreatAsLocalProperty>` on every lib `<Project>` AND add platform-suffix variants (`net9.0-desktop`, etc.) to `tfm-common-winui.props` — biggest invasive change.

### Bug 3 — CS7038 EnC version drift (downstream of Bug 2)

```text
error CS7038: Failed to emit module 'SimpleSampleApp':
  Changing the version of an assembly reference is not allowed during debugging:
  'Uno.Simple.WinUI, Version=1.0.0.0' changed version to '0.0.0.0'.
```

The published binary has `Uno.Simple.WinUI v1.0.0.0` (verified via `AssemblyName.GetAssemblyName`). Inside the dev-server's broken workspace, the lib's emitted-on-the-fly compilation defaults to `0.0.0.0` because the project graph never finishes evaluating (no `AssemblyVersionAttribute` resolution). Roslyn EnC refuses to apply the delta. Forcing `<Version>1.0.0</Version>/<AssemblyVersion>1.0.0.0</AssemblyVersion>` on the lib (already attempted) does **not** help because the workspace failure is upstream of version recording.

### Side observations (do not block but pollute the log)

- `MSBuildWorkspace [Failure]` for `NU1507`: NuGet reports central package management without `<packageSourceMapping>` for the two sources in `nuget.config` (`api.nuget.org`, `unoplatformdev`). At `dotnet publish` this is just a warning; in the dev-server's MSBuildWorkspace it surfaces as a `[Failure]` diagnostic.
- `Tmds.DBus.Protocol 0.21.2` (transitive via `Uno.WinUI.Runtime.Skia.X11/6.5.153`) carries a high-severity CVE; .NET SDK NuGetAudit treats this as a warning at restore, MSBuildWorkspace surfaces it as `[Failure]`.
- `Uno.Licensing.Sdk.Contracts` `FileNotFoundException` while loading `Uno.UI.App.Mcp.Server.dll` (and `WithHttpTransport()` not called) — MCP server plug-in registration noise; HR works without it.

### Reproduction summary

- **Test never produces results**: `/tmp/runtime-tests-hotreload-SimpleSampleApp.xml` stays absent; script ends with `Runtime tests did not produce results … exit 1`.
- **Process state**: secondary app + dev-server both alive, all worker threads in `futex_wait`/`epoll_wait` past the 180 s `DefaultWorkspaceTimeout`. The engine never receives a `WorkspaceLoaded` signal because the workspace creation loops on the failing project graph.
- **Identical pattern to Azure DevOps run 3706004**: timeout, no XML, "Runtime tests did not produce results".

### Recommended order of fixes

1. **Patch the test** — change `Given_HotReload.cs` to pass `"HotReload/HotReloadTarget.cs"`. Without this, even a fully working dev-server cannot find the file.
2. **Fix the dev-server TFM propagation** — file an issue / PR against `Uno.UI.RemoteControl` so `TargetFrameworks` is not promoted to a global property, or have it cleared for ProjectReferences that don't import `DevServer.props`.
3. **Resolve NU1507** — add `<packageSourceMapping>` to `nuget.config` (or drop one source). Mostly cosmetic but prevents [Failure] noise.
4. **Update `Tmds.DBus.Protocol`** — bump `Uno.WinUI.Runtime.Skia.X11` to a version that pulls a non-vulnerable Tmds.DBus, or add a transitive override in `Directory.Packages.props`.

Without #2, the test cannot pass on this codebase, regardless of #1/#3/#4. Bug #3 (CS7038) disappears automatically once #2 is fixed.

## Implementation (2026-05-06)

Applied the conservative workaround agreed with David: keep the dev-server fix on the backlog (other "bigger projects coming"), patch the test path locally, and unblock the workspace evaluation entirely on the library side, all under `Configuration=Debug`. Each change carries an in-source comment pointing back here.

### Changes

- `src/samples/Uno.Themes.Samples.RuntimeTests/HotReload/Given_HotReload.cs:34-43` — corrected the `UpdateSourceFile` argument from the repo-rooted path to `"HotReload/HotReloadTarget.cs"`. Also added an inline comment explaining how `[RuntimeTestsSourceProject]` resolves `projectDir`, so a future reader doesn't reintroduce the duplication.
- `src/library/tfm-common-winui.props` — appended a Debug-only `<PropertyGroup Condition="'$(Configuration)'=='Debug' and '$(UnoIsHotReloadHost)'=='true'">` that forces `TargetFrameworks=net9.0`, with a long comment explaining the dev-server's global-property propagation and why this PropertyGroup is the local antidote.
- `src/library/Uno.Simple.WinUI/Uno.Simple.WinUI.csproj`, `src/library/Uno.Themes/Uno.Themes.WinUI.csproj`, `src/library/Uno.Material/Uno.Material.WinUI.csproj`, `src/library/Uno.Cupertino/Uno.Cupertino.WinUI.csproj` — added `TreatAsLocalProperty="TargetFramework;TargetFrameworks;RuntimeIdentifier"` on the `<Project>` element (this attribute does not support a `Condition`, so it is unconditional, but inert outside HR because the global properties are only injected by the dev-server in Debug).

### Tentative "option 3" (rolled back)

The original plan was to also reference `Uno.WinUI.DevServer` Debug-only in each library so that `DevServer.props` would apply to libs as well as the head. Tested locally and reverted: with libs referencing `Uno.WinUI.DevServer`, the secondary app fails to connect to the dev-server entirely:

```text
fail: Uno.UI.RemoteControl.RemoteControlClient[0]
      Failed to connect to the server (all endpoints failed).
```

The dev-server starts and listens on its port, but the secondary app's `RemoteControlClient.Initialize` never establishes the WebSocket. No `App Connected: AppLaunchMessage` log on the dev-server side. The new `Uno.WinUI.DevServer` reference inside each lib pulls in `[ServerEndpoint]` and HR-info plumbing into the lib assemblies; one of those side-effects races against / shadows the env-var-based endpoint setup that runtime-tests relies on (`UNO_DEV_SERVER_HOST`/`UNO_DEV_SERVER_PORT` set on the secondary app). The end result is the secondary app's connection list ends up empty or invalid, and `WaitForConnection` times out at the workspace stage (fed back as `TimeoutException: Timeout while waiting for hot reload workspace to be loaded`).

The `<PackageVersion Include="Uno.WinUI.DevServer">` entry in `src/library/Directory.Packages.props` was reverted alongside the package references.

Outcome: option 2 alone (TreatAsLocalProperty + tfm-common-winui.props HR override) is the right move. Option 3 is a non-starter without first fixing the dev-server's RemoteControlClient endpoint setup — that's part of the "bigger projects coming" pile.

### Test outcome with option 2

Re-running the CI flow (`build/scripts/linux-skia-desktop-runtime-tests.sh` with `BUILD_CONFIG=Debug` + `HotReload` filter) now reaches the actual hot-reload path:

- Secondary app connects to the dev-server (`App Connected: AppLaunchMessage { ... }` logged server-side).
- `ConfigureServer` is processed; `Server loaded processors: FileUpdateProcessor + ServerHotReloadProcessor` registered.
- The dev-server's MSBuildWorkspace **does** load the project graph successfully (no more `[Failure]` cascade on TFM mismatch — only the cosmetic NU1507 / Tmds.DBus.Protocol vuln entries remain).
- The Roslyn delta is produced: `[Output] Found 2 metadata updates after 00:00:01.6340297`.
- The XML test result file is written: `/tmp/runtime-tests-hotreload-SimpleSampleApp.xml`.

The test still ends in `Failed`, with a different — and much more informative — error:

```text
Microsoft.VisualStudio.TestTools.UnitTesting.AssertFailedException: Assert.AreEqual failed. Expected:<updated>. Actual:<original>.
```

…driven by this warning from the secondary app's `ClientHotReloadProcessor`:

```text
warn: Uno.UI.RemoteControl.HotReload.ClientHotReloadProcessor[0]
      Unable to access Dispatcher/DispatcherQueue in order to invoke ReloadWithUpdatedTypes.
      Make sure you have enabled hot-reload (Window.UseStudio()) in app startup.
      See https://aka.platform.uno/hot-reload
```

The HR delta lands at the client successfully but `ClientHotReloadProcessor.CurrentWindow` is never set, so the type swap (`HotReloadTarget.GetValue()` → `"updated"`) is not dispatched onto the running app. `SetWindow` is `internal` and is wired up by `Uno.UI.WindowExtensions.UseStudio()` (shipped via `Uno.UI.HotDesign.Client.dll`).

### What's left for HR test to actually pass

`MainWindow.UseStudio(launchHotDesignOnStart: false)` (or equivalent) needs to be called in `OnLaunched` of the sample app, behind the same `#if DEBUG && HAS_UNO` guard StudioLive uses. That brings in `Uno.UI.HotDesign` as a transitive ; out of scope for this round per David's call ("bigger projects coming"). The infrastructure-level fix is in, the test now produces a meaningful failure mode, and CI run 3706004's hang/no-XML pattern is gone — replaced by a deterministic Pass-or-Failed verdict.

## Round 2 (2026-05-06, after option 2 landed)

Pushed two more fixes to see how far we could get the test:

### Fix A — `MainWindow.UseStudio()` in `App.xaml.cs`

Added in [SimpleSampleApp/App.xaml.cs:50-65](../../src/samples/SimpleSampleApp/App.xaml.cs#L50-L65), behind `#if DEBUG && HAS_UNO`:

```csharp
MainWindow.UseStudio(showHotReloadIndicator: false);
```

Provided by `Uno.UI.HotDesign.Client` (transitively pulled in by `Uno.Sdk.Private` in non-Optimize builds — package `Uno.UI.HotDesign.Client.dll` is already present in the publish output). Initial draft used `launchHotDesignOnStart: false` (StudioLive's variant); the ICSharpCode decompile of `Uno.UI.WindowExtensions.UseStudio` showed the HotDesign 1.19.0 signature is `(this Window, bool showHotReloadIndicator = true)` — corrected accordingly.

**Effect**: The `Unable to access Dispatcher/DispatcherQueue in order to invoke ReloadWithUpdatedTypes` warning disappears. `ClientHotReloadProcessor.SetWindow` is now called via `WindowExtensions.UseStudio` → `EnableHotReload` → `SetWindow`, and `CurrentWindow` is non-null when the HR delta arrives.

### Fix B — `[MethodImpl(NoInlining)]` on `HotReloadTarget.GetValue`

[HotReloadTarget.cs:7-22](../../src/samples/Uno.Themes.Samples.RuntimeTests/HotReload/HotReloadTarget.cs#L7-L22):

```csharp
[MethodImpl(MethodImplOptions.NoInlining)]
internal static string GetValue() => "original";
```

The original method was a single-line `return "literal";`, small enough that the JIT inlines it into the test caller. EnC deltas rewrite the callee's IL but do not re-JIT existing callers, so an inlined call site keeps the original literal even after the delta is applied. NoInlining forces a real call frame so the new IL takes effect on the next invocation.

### Remaining gap — MVID mismatch (delta produced but never applied)

With Fix A + Fix B, the test still fails with `Assert.AreEqual failed. Expected:<updated>. Actual:<original>.`, but the failure mode is now **diagnostic-rich**, not infrastructural. Concrete evidence in the log:

- `App Connected: MVID=9a89a472-04fb-43d8-b9fe-b4b2e0a549e6 Platform=Desktop1.0 Debug=False — No immediate match, pending handled by ApplicationLaunchMonitor.`
- `Received Frame [HotReload / UpdateFileRequest] to be processed by Uno.UI.RemoteControl.Host.HotReload.ServerHotReloadProcessor`
- `[Output] Found 2 metadata updates after 00:00:01.6980776` (and 5 more on the 3 retry attempts)
- `MetadataUpdateHandler.MetadataUpdated` event fires (otherwise `HotReloadHelper.SendMessageCore` would throw `TimeoutException` after 60 s — it does not, the test proceeds straight to the Assert)
- `ClientHotReloadProcessor.CurrentWindow` is now non-null (no Dispatcher warning)
- And yet `HotReloadTarget.GetValue()` still returns `"original"` after the delta is "applied"

Decompiled `Uno.UI.RemoteControl.HotReload.MetadataUpdater.HotReloadAgent.ApplyDeltas`:

```csharp
foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
{
    Guid? guid = TryGetModuleId(assembly);     // ModuleVersionId of the loaded DLL
    if (guid is { } valueOrDefault && valueOrDefault == updateDelta.ModuleId)
    {
        ApplyUpdate(assembly, updateDelta);     // System.Reflection.Metadata.MetadataUpdater.ApplyUpdate
    }
}
// No match → silently skip ApplyUpdate, but still raise MetadataUpdated event so the helper unblocks.
```

`updateDelta.ModuleId` is the MVID of the **dev-server's in-memory compilation baseline** (Roslyn's `WatchHotReloadService` generates this at workspace creation time inside `ServerHotReloadProcessor.CreateCompilation`). The secondary app, however, loads `Uno.Themes.Samples.RuntimeTests.dll` from the publish output, which has a **different** MVID (`mvid=11cf8352-d620-41ee-a8a3-9172de8468b2` for the current publish). They never match — the delta is queued in `_deltas` but never `ApplyUpdate`d, the MetadataUpdated event fires anyway, the test caller observes the unchanged in-memory IL.

Verified directly :

```bash
$ dotnet run -- src/samples/SimpleSampleApp/bin/Debug/net10.0-desktop/Uno.Themes.Samples.RuntimeTests.dll
Uno.Themes.Samples.RuntimeTests.dll mvid=11cf8352-d620-41ee-a8a3-9172de8468b2
```

This is the deeper hot-reload pipeline issue: the dev-server compiles the workspace from source (fresh MVIDs), but the runtime-tests engine launches the secondary app against the **previously-published** binaries (different MVIDs). For HR to land, one of the following must happen — none of which are fixable from the Uno.Themes repo :

1. The dev-server emits the workspace's compilation to disk **before** the secondary app launches, and the secondary loads those instead of the publish output.
2. The dev-server creates its `WatchHotReloadService` baseline by **loading the published assemblies' metadata** (Roslyn supports an `Assembly` baseline overload), so the baseline MVID equals the running MVID and deltas apply.
3. The runtime-tests engine post-processes the publish output to align MVIDs with what the dev-server will produce.

(1) and (2) live in `Uno.UI.RemoteControl.Server.Processors`; (3) lives in `Uno.UI.RuntimeTests.Engine`.

### Final state after this round

| Stage | Before | After (this round) |
| --- | --- | --- |
| Hot-reload runtime-tests script | hangs past 180 s, no XML | exits in ~16 s with valid XML |
| Workspace load | `MSBuildWorkspace [Failure]` cascade, CS0234/CS0246/UXAML0001 | clean (only NU1507 / Tmds.DBus.Protocol cosmetic noise) |
| App ↔ dev-server connection | `Failed to connect (all endpoints failed)` (after option 3) ; or works | works (option 3 dropped) |
| File-edit path | `Requested file '...' does not exists.` | edit lands on disk |
| Delta production | `CS7038: ...0.0.0.0` (workspace too broken to emit) | `Found 2 metadata updates after ~1.6 s` |
| Delta dispatch | `Unable to access Dispatcher/DispatcherQueue ...` | dispatcher hookup ok via `UseStudio` |
| Delta application | n/a | silently skipped — MVID mismatch (above) |
| Test verdict | hang, exit 1, no test cases | `Assert.AreEqual<updated, original>` Failed, exit 0, 1 test case reported |

Test still ends `Failed`, but every step of the pipeline is now observable and the only remaining blocker is upstream of this repo.

## Round 3 (2026-05-06, with Trace logs)

The "MVID mismatch" hypothesis from Round 2 was wrong. After raising `Uno.UI.RemoteControl` log filter to `LogLevel.Trace` we get the actual delta-apply trace:

```text
Applying delta to SimpleSampleApp,                      Version=1.0.0.0 / 22936315-f53f-44ee-9c7e-256386245603
Applying delta to Uno.Themes.Samples.RuntimeTests,      Version=1.0.0.0 / 11cf8352-d620-41ee-a8a3-9172de8468b2
UpdateApplication (changed types: __Uno.HotReload.HotReloadInfo)
UpdateApplication (changed types: Uno.Themes.Samples.RuntimeTests.HotReload.HotReloadTarget)
Deltas applied.
Done applying IL Delta for ... HotReloadTarget.cs ...
```

Both MVIDs (`22936315…` for SimpleSampleApp and `11cf8352…` for the runtime-tests assembly) match the published binaries on disk — confirmed via `AssemblyName.GetAssemblyName(...)`. Deterministic builds align the dev-server's in-memory compilation with the published output, so `MetadataUpdater.ApplyUpdate` is invoked on the right loaded `Assembly` and the runtime reports `UpdateApplication (changed types: ... HotReloadTarget)`.

Despite that, `HotReloadTarget.GetValue()` keeps returning `"original"` from the test's perspective and the assert fails on every retry. Things that did NOT fix it:

1. **Race-condition workaround in the test harness** — `HotReloadHelper.SendMessageCore` waits for the *first* `MetadataUpdated` event, which fires after the first of the two deltas (SimpleSampleApp's `__Uno.HotReload.HotReloadInfo` source-gen update). I added a `while (HotReloadTarget.GetValue() != "updated" && DateTime.UtcNow < deadline) await Task.Delay(50, ct);` poll loop bound by `HotReloadHelper.DefaultMetadataUpdateTimeout` (60 s). Each of the 3 attempts now polls for a full minute and still observes `"original"` — and meanwhile the `Done applying IL Delta` for the second delta (the one that actually targets HotReloadTarget) is logged on the dev-server side. So the apply happens before the poll exits, the value still doesn't change.
2. **`[MethodImpl(NoInlining | NoOptimization)]`** on `HotReloadTarget.GetValue` — to stop the JIT from inlining the literal-returning method into the test caller, and to keep the method off the tier-1 path that might fold the constant. No effect.
3. **`DOTNET_TieredCompilation=0`** as an env var on the secondary process — same negative result.

Code stays as documented in `Round 2` plus the poll-loop in [Given_HotReload.cs:50-67](../../src/samples/Uno.Themes.Samples.RuntimeTests/HotReload/Given_HotReload.cs#L50-L67) and `NoOptimization` in [HotReloadTarget.cs:18](../../src/samples/Uno.Themes.Samples.RuntimeTests/HotReload/HotReloadTarget.cs#L18) — both annotated with the rationale.

### Why the apply is a no-op (working theory)

The dev-server's `WatchHotReloadService.EmitSolutionUpdateAsync` sees TWO assemblies change per cycle :

- `SimpleSampleApp.dll` (MVID `22936315…`): only the source-generated `__Uno.HotReload.HotReloadInfo` version field flips, no user-visible code change.
- `Uno.Themes.Samples.RuntimeTests.dll` (MVID `11cf8352…`): both `__Uno.HotReload.HotReloadInfo` AND `HotReloadTarget` source files have changed.

Roslyn computes one EnC delta per assembly. The test eventually observes `Done applying IL Delta` for the runtime-tests delta. But — and this is the part I cannot pin from inside this repo — the `MetadataUpdater.ApplyUpdate(assembly, metadataDelta, ilDelta, pdbBytes)` call with the runtime-tests delta does not actually swap `HotReloadTarget.GetValue`'s body in the running app. No exception, no warning. Subsequent calls to `GetValue()` return the original literal. Possibilities I cannot prove or disprove without instrumenting the runtime / `WatchHotReloadService` directly:

- The IL delta produced by `WatchHotReloadService` for that compilation is empty for `HotReloadTarget` (only the metadata table changes, not the IL of `GetValue`). The `__Uno.HotReload.HotReloadInfo` regeneration may be confusing Roslyn's diff machinery into emitting the type-changed list without an actual method-body update — `UpdateApplication (changed types: ...)` is a heuristic over `UpdatedTypes` from the delta, which can be non-empty even when the body bytes are unchanged.
- The runtime DOES apply the delta but the JIT keeps a cached entry-point that bypasses the EnC table for static methods on a static class — possible but unusual on .NET 10 desktop with `Baseline` capability advertised by `MetadataUpdater.GetCapabilities()`.

Either way the next concrete debugging step is on Roslyn / `Uno.UI.RemoteControl.Server.Processors.WatchHotReloadService` to log the actual `ilDelta` byte length / disassembly — which means hooking the dev-server, not patching Uno.Themes.

### Final landed state

Files modified in `artifacts/Uno.Themes/`:

- [src/library/tfm-common-winui.props](../../src/library/tfm-common-winui.props) — Debug-only `UnoIsHotReloadHost` PropertyGroup forcing `TargetFrameworks=net9.0` (Round 1).
- [src/library/Uno.Simple.WinUI/Uno.Simple.WinUI.csproj](../../src/library/Uno.Simple.WinUI/Uno.Simple.WinUI.csproj), [Uno.Themes.WinUI.csproj](../../src/library/Uno.Themes/Uno.Themes.WinUI.csproj), [Uno.Material.WinUI.csproj](../../src/library/Uno.Material/Uno.Material.WinUI.csproj), [Uno.Cupertino.WinUI.csproj](../../src/library/Uno.Cupertino/Uno.Cupertino.WinUI.csproj) — `TreatAsLocalProperty="TargetFramework;TargetFrameworks;RuntimeIdentifier"` on each `<Project>` (Round 1).
- [src/samples/SimpleSampleApp/App.xaml.cs](../../src/samples/SimpleSampleApp/App.xaml.cs) — `MainWindow.UseStudio(showHotReloadIndicator: false)` Debug+HAS_UNO (Round 2) ; HR-runtime-tests log filters at Trace (Round 3 diagnostic).
- [src/samples/Uno.Themes.Samples.RuntimeTests/HotReload/HotReloadTarget.cs](../../src/samples/Uno.Themes.Samples.RuntimeTests/HotReload/HotReloadTarget.cs) — `[MethodImpl(NoInlining | NoOptimization)]` (Rounds 2-3).
- [src/samples/Uno.Themes.Samples.RuntimeTests/HotReload/Given_HotReload.cs](../../src/samples/Uno.Themes.Samples.RuntimeTests/HotReload/Given_HotReload.cs) — corrected file path, added HR-DIAG logging, added the `MetadataUpdated`-race poll loop (Rounds 1-3).

### Test verdict per round

| Round | Workspace | Connection | Edit applied | Delta produced | Delta applied to right MVID | Method body swap | Test verdict |
| --- | --- | --- | --- | --- | --- | --- | --- |
| Pre-fix | broken | n/a | n/a | n/a | n/a | n/a | hang ; no XML ; exit 1 |
| Round 1 (option 2) | clean | ok | ok | ok | n/a (dispatch fails first) | n/a | exit 0, 1 case `Failed` (Dispatcher missing) |
| Round 2 (UseStudio + NoInlining) | clean | ok | ok | ok | ok | apparently no | exit 0, 1 case `Failed` (Assert mismatch) |
| Round 3 (poll loop + NoOpt + Trace) | clean | ok | ok | ok | ok | apparently no | exit 0, 1 case `Failed` (Assert mismatch) |

The CI pattern from run 3706004 (180 s hang, no XML produced) is fully gone. The test now exits deterministically with one test case reported and a precise failure message that points at a runtime-level limitation, not at a missing piece of infrastructure on the Uno.Themes side. Further investigation requires instrumenting `Uno.UI.RemoteControl.Server.Processors` to dump the actual EnC IL delta bytes ; doing it from this repo would only paper over the symptom.

## Round 4 (2026-05-06) — definitive proof: empty IL delta upstream

To distinguish "JIT inlined/cached the result" from "EnC didn't actually rewrite the method body", I added a side-by-side diagnostic just before the assert :

```csharp
var directValue = HotReloadTarget.GetValue();                                                    // direct call
var reflectionValue = typeof(HotReloadTarget)                                                    // bypasses any
    .GetMethod("GetValue", BindingFlags.Static | BindingFlags.NonPublic)                         // JIT-inlined
    ?.Invoke(null, null) as string;                                                              // call site
Console.Error.WriteLine($"[HR-DIAG:DirectVsReflection] direct={directValue} reflection={reflectionValue}");
```

Reflection's `MethodInfo.Invoke` goes through the runtime's method-table dispatch — it cannot use any JIT-emitted-then-cached call site at the test caller. If EnC had successfully rewritten `GetValue`'s IL, reflection would observe the new value even when the direct call site did not.

The runtime-tests output captured (Console.Error is forwarded by the engine as `[ERROR]` line in the secondary's stdout pipe — `LogInformation` was getting swallowed by the `Uno -> Warning` filter David already had in `App.xaml.cs`):

```text
[HR-DIAG:DirectVsReflection] direct=original reflection=original
```

Both reads are `"original"`. The delta is a **no-op** for `GetValue`'s body — the runtime applied something, the `[MetadataUpdateHandlerAttribute]`-registered handler fired (otherwise our poll loop would never have unblocked the assert), but the IL associated with `HotReloadTarget.GetValue` after `MetadataUpdater.ApplyUpdate` returned is identical to before.

This kills the JIT/inlining theories entirely. NoInlining, NoOptimization, TieredCompilation=0 — none of them matter because there is nothing new for the runtime to use. The `UpdateApplication (changed types: Uno.Themes.Samples.RuntimeTests.HotReload.HotReloadTarget)` log entry is misleading: the type token shows up in `UpdatedTypes` because the source-generator's `__Uno.HotReload.HotReloadInfo` regeneration touches the assembly's metadata table, which Roslyn lumps into the same delta. The actual IL byte stream produced by `WatchHotReloadService.EmitSolutionUpdateAsync` for `HotReloadTarget.GetValue` is empty.

Why Roslyn produces an empty IL delta for a clearly-changed method body is the next investigation step — and it lives in `Microsoft.CodeAnalysis.Workspaces` / `WatchHotReloadService` (the Roslyn internal API the dev-server reflects into). The most plausible cause given the dual-delta pattern (`SimpleSampleApp` updates only its `__Uno.HotReload.HotReloadInfo`; `Uno.Themes.Samples.RuntimeTests` updates `__Uno.HotReload.HotReloadInfo` AND `HotReloadTarget`) is that the source-generator output regeneration in the same edit window confuses Roslyn's EnC analyzer about which methods actually need an IL emit. Reproducing this in isolation against `WatchHotReloadService` directly is the next step ; not feasible from inside `artifacts/Uno.Themes`.

### Summary of what we proved (and didn't)

| Claim | Status | Evidence |
| --- | --- | --- |
| Workspace loads cleanly under HR mode | ✅ Confirmed | No more `MSBuildWorkspace [Failure]` cascade after Round 1 (TreatAsLocalProperty + tfm-common-winui.props HR override) |
| Secondary connects to dev-server | ✅ Confirmed | `App Connected: AppLaunchMessage { ... }` server-side, after rolling back option 3 |
| Dev-server compiles a delta | ✅ Confirmed | `[Output] Found 2 metadata updates after ~1.6 s` |
| Delta is sent to the right MVID | ✅ Confirmed | `Applying delta to Uno.Themes.Samples.RuntimeTests, Version=1.0.0.0 / 11cf8352-d620-41ee-a8a3-9172de8468b2` matches `AssemblyName.GetAssemblyName(...)` of the publish output |
| Delta is "applied" (handler fires) | ✅ Confirmed | `Deltas applied.`, `Done applying IL Delta`, `MetadataUpdated` event unblocks the helper |
| Delta actually rewrites `GetValue`'s IL | ❌ Refuted | Both direct call and reflection return `"original"` after the apply |
| `UpdateApplication (changed types: HotReloadTarget)` implies IL changed for that method | ❌ Refuted | Misleading; type appears because the assembly's metadata changed (source-gen output), not because the listed method's IL was emitted |
| Dispatch / Window / `UseStudio` is required | ✅ Confirmed | Without it the warning fires and we can't even invoke `ReloadWithUpdatedTypes` |
| `[MethodImpl(NoInlining)]` matters | ⚪ Inconclusive | It's the right thing to keep (would matter the moment a real IL update lands), but it does not affect the current failure since the delta is empty regardless |

### Closing state

This investigation has bottomed out: every step from "press save" to "method-table dispatch" works on the Uno.Themes side, but the IL delta produced by `WatchHotReloadService` upstream for the changed method is empty. `Given_HotReload.When_UpdateMethodBody_Then_MetadataUpdateReceived` cannot pass on Linux Skia desktop with `Uno.Sdk.Private 6.5.153 / Uno.UI.RuntimeTests.Engine 2.0.0-dev.74` until that upstream behavior changes. The local repro is exact and the failure is now deterministic, observable, and debuggable line-by-line — exactly the signal needed to file an upstream bug or hand off to the Uno hot-reload team for the "bigger projects coming" mentioned by David.

The diagnostic instrumentation (`[HR-DIAG:DirectVsReflection]`) in [Given_HotReload.cs](../../src/samples/Uno.Themes.Samples.RuntimeTests/HotReload/Given_HotReload.cs) is intentionally left in place — it produces a clean, single-line proof in the runtime-tests XML that anyone running the test will see and can act on.

## Round 5 (2026-05-06) — disproved theory: source-generator noise

The Round 4 hypothesis was that `__Uno.HotReload.HotReloadInfo` (auto-generated on every compile by `Uno.WinUI.DevServer.props`'s `UnoGenerateHotReloadInfo=True`) confused Roslyn into reporting `HotReloadTarget` in the `UpdatedTypes` of an otherwise empty IL delta.

Tested by setting `<UnoGenerateHotReloadInfo>false</UnoGenerateHotReloadInfo>` on `Uno.Themes.Samples.RuntimeTests.csproj`, rebuilding, and re-running. With the source generator off:

- The runtime-tests assembly delta no longer contains `__Uno.HotReload.HotReloadInfo` in its `UpdatedTypes` — confirmed in the trace: only `UpdateApplication (changed types: Uno.Themes.Samples.RuntimeTests.HotReload.HotReloadTarget)` for the second delta.
- The diagnostic still prints `[HR-DIAG:DirectVsReflection] direct=original reflection=original`.
- The test still fails with the same `Assert.AreEqual<updated, original>`.

So the `HotReloadInfo` source-gen is not the cause. Reverted the project change. The empty IL delta for `HotReloadTarget.GetValue` is produced by Roslyn's `WatchHotReloadService.EmitSolutionUpdateAsync` regardless of source-gen activity — likely a more fundamental behavior of how the workspace diff is computed against a baseline that was loaded with `forceEmitCompilationOutput=false` (the dev-server's default in `CompilationWorkspaceProvider.CreateWorkspaceAsync`).

### Final landing state

Investigation has bottomed out conclusively. From the Uno.Themes side everything that can work, works. The remaining gap is upstream and requires either patching the dev-server's `WatchHotReloadService` baseline strategy or filing the bug against `dotnet/roslyn` directly. The local repro produces a single-line XML failure with the exact diagnostic needed for that handoff:

```text
[HR-DIAG:DirectVsReflection] direct=original reflection=original
…
Assert.AreEqual failed. Expected:<updated>. Actual:<original>.
```

No further patches make sense in `artifacts/Uno.Themes` — moving the problem any further requires hooking into `Microsoft.CodeAnalysis.Workspaces` or `Uno.UI.RemoteControl.Server.Processors`.

## Round 6 (2026-05-06) — Roslyn EnC trace, root cause identified

Roslyn's EnC service writes a structured `Session.log` when `Microsoft_CodeAnalysis_EditAndContinue_LogDir` is set. The dev-server forwards this env var via `properties["UnoHotReloadDiagnosticsLogPath"]` (decompiled `CompilationWorkspaceProvider.CreateWorkspaceAsync`), but it can be set directly on the parent shell since the env propagates to the child dev-server process. Setting it and re-running gave us the actual diff machinery output:

```text
Found 1 potentially changed document(s) in project Uno.Themes.Samples.RuntimeTests
Document changed, added, or deleted: '.../HotReload/HotReloadTarget.cs'
Project summary for Uno.Themes.Samples.RuntimeTests '....csproj': ValidChanges
Emitting update of Uno.Themes.Samples.RuntimeTests '....csproj': project not built
Solution update 1.1 status: Ready
Module update: capabilities=[Baseline], types=[0200000D], methods=[06000051]
Edit session restarted (break state: False)
```

Decoding the metadata tokens against the published binary (`AssemblyName.GetAssemblyName` + `Module.ResolveMember`):

- `0x0200000D` = `Uno.Themes.Samples.RuntimeTests.HotReload.HotReloadTarget`
- `0x06000051` = `GetValue` (with `MethodImplementationFlags=72` = `NoInlining|NoOptimization`, matching what we shipped)

Roslyn IS targeting the right method. But the `project not built` line is the smoking gun. From the Roslyn source, this message comes from `EditAndContinueService` when the project's emit baseline is missing — i.e. the workspace was opened (`MSBuildWorkspace.OpenProjectAsync`) and the compilation was obtained (`Project.GetCompilationAsync`) but the project was never actually `Compilation.Emit(...)`-ed to disk. EnC then computes the IL delta against an in-memory baseline that doesn't carry the PE/PDB bytes Roslyn needs to produce a valid replacement IL stream — the resulting `methods=[06000051]` entry is in the metadata table but the `ilDelta` payload is empty/incomplete. The runtime's `MetadataUpdater.ApplyUpdate` happily applies the empty delta and returns success, but no IL is actually replaced — exactly what `[HR-DIAG:DirectVsReflection] direct=original reflection=original` proved in Round 4.

`CompilationWorkspaceProvider.CreateWorkspaceAsync` (decompiled, [Round 1 dump](#bug-2--msbuildworkspace-targetframeworks-propagation-breaks-library-evaluation-real-blocker)) loads sources and warms up `Project.GetCompilationAsync` but does not call `Compilation.Emit`. Compare to the unit-test code in this repo:

```csharp
// src/uno.roslyn.adhoc.Tests/TestUtils/HotReloadHelper.cs
manager = await HotReloadManager.CreateAsync(
    workspace, capabilities, ..., tracker, changesDetector, ct,
    forceEmitCompilationOutput: true);   // <-- key flag for unit-test scenarios
```

The `Uno.Roslyn.AdHoc.HotReloadManager` accepts `forceEmitCompilationOutput`, which causes the workspace bootstrap to emit a baseline DLL/PDB to a temp dir before starting the EnC session. The dev-server's `ServerHotReloadProcessor.CreateCompilation` does NOT pass this flag (it doesn't even exist on the public surface of `Uno.UI.RemoteControl.Server.Processors`'s Roslyn integration). That's the upstream mismatch.

### Fix is upstream

The patch must land in `Uno.UI.RemoteControl.Server.Processors.CompilationWorkspaceProvider.CreateWorkspaceAsync` or in `ServerHotReloadProcessor.CreateCompilation`: emit the workspace's projects to a temp directory before calling `WatchHotReloadService.StartSessionAsync`, OR pass an existing PE/PDB baseline (from the publish output) through `WatchHotReloadService`'s baseline overload. Neither is reachable from inside `artifacts/Uno.Themes`.

### Notable confirmation: source-gen disabled didn't help

Round 5 had ruled out the `__Uno.HotReload.HotReloadInfo` source-generator as the cause by setting `<UnoGenerateHotReloadInfo>false</UnoGenerateHotReloadInfo>` on the runtime-tests project. The Round 6 trace confirms why: even without the auto-generated companion type, Roslyn still emits `methods=[06000051]` with the same `project not built` warning — the issue is purely the missing emit baseline. The source-gen was always orthogonal noise.

### Final wrap-up

The Uno.Themes side of the investigation is done. Every fix in this repo lands cleanly under `Configuration=Debug` with explanatory comments. The local repro now produces, in ~3 minutes, a deterministic XML failure with a single line that names the upstream method (`Microsoft.CodeAnalysis.WatchHotReloadService` not having a built baseline) — perfect leverage for an upstream issue or a one-line patch in `Uno.UI.RemoteControl.Server.Processors`.

Action items for the upstream team (out of scope for `artifacts/Uno.Themes`):

1. **`Uno.UI.RemoteControl.Server.Processors`** : in `CompilationWorkspaceProvider.CreateWorkspaceAsync`, emit each project's `Compilation` to a temp directory after `OpenProjectAsync` so `WatchHotReloadService.StartSessionAsync` has a real PE/PDB baseline. Equivalent to the `forceEmitCompilationOutput=true` flag that already exists in `Uno.Roslyn.AdHoc.HotReloadManager` in this same repo (`src/uno.roslyn.adhoc/uno.roslyn/`).

## Round 7 (2026-05-06) — TFM alignment lib ↔ head (test setup mirroring `artifacts/uno.extensions`)

The user's hypothesis: "`project not built` may be caused by misaligned compilation options — my main suspect would be an invalid TFM flow." We compared `artifacts/Uno.Themes` to `artifacts/uno.extensions` (which has a working HR runtime test) and found one structural difference:

| | Uno.Extensions (HR ✅) | Uno.Themes (HR ❌) |
|---|---|---|
| Sample app head | `net9.0-desktop` (+ flavors net9.0-*) | `net10.0-desktop` |
| Runtime test lib | `net9.0-desktop` | `net10.0-desktop` |
| Design libs | `net9.0-*` | `net9.0` only (Release) |
| All projects same TFM family? | YES | NO |

The dev-server (`ServerHotReloadProcessor.CreateCompilation`) injects `TargetFrameworks=<head TFM>` as a global MSBuild property when it loads the Roslyn workspace. In Uno.Themes, that propagated `net10.0-desktop` to libs that only ship `net9.0`, producing the cascade. Uno.Extensions has no such mismatch.

### Approach (Option B — keep apps on net10.0, widen libs in Debug only)

Per the user's preference (apps stay on net10.0-desktop to keep alignment with the rest of the production CI), expanded the libs in Debug:

- `src/library/tfm-common-winui.props` : added `<TargetFrameworks>$(TargetFrameworks);net10.0-desktop</TargetFrameworks>` under `Configuration=Debug`. Replaces the previous `UnoIsHotReloadHost → net9.0` override (which fought the dev-server propagation instead of accepting it).
- Opt-out hatch `_UnoOptOutHotReloadTfmExpansion` (no project currently uses it).
- `Uno.Simple.WinUI.csproj` : switched `Sdk="Microsoft.NET.Sdk"` → `Sdk="Uno.Sdk"`. Required because Microsoft.NET.Sdk doesn't recognize the `desktop` TPI (NETSDK1139); Uno.Sdk extends `_AdditionalKnownTargetPlatformIdentifiers` to handle it. Aligns with the 3 sibling design libs.
- `src/samples/Directory.Packages.props` : enabled `<CentralPackageTransitivePinningEnabled>true</CentralPackageTransitivePinningEnabled>` and pinned `<PackageVersion Include="Microsoft.Extensions.Logging.Console" Version="10.0.2" />`. Required because once the libs build for `net10.0-desktop`, Uno.Sdk's net10 implicit packages bring `Logging.Console >= 10.0.2` while the head's Uno.Sdk.Private resolves `10.0.0` (NU1605 downgrade across the `Uno.UI.HotDesign 1.17.415 → Uno.Themes.WinUI` transitive chain).

### Validation result

- ✅ `dotnet publish -c Debug -f net10.0-desktop` clean (no NETSDK1139, no NU1605).
- ✅ Test runs end-to-end and produces deterministic XML (~3 minutes) — the secondary app starts, loads runtime-test engine, executes the HR test method.
- ✅ Roslyn EnC `Session.log` no longer shows the CS0234/CS0246 cascade — the workspace evaluates cleanly.
- ✅ The metadata IL delta now contains the right user string. Hex-decoding `1.meta` shows `"updated"` (UTF-16) added to the `#US` heap and the IL `ldstr` token wired up correctly.
- ❌ `[HR-DIAG:DirectVsReflection] direct=original reflection=original` — same Round 6 outcome. The IL is computed correctly but not actually applied at runtime.
- ❌ Roslyn EnC log still emits `Emitting update of <project>: project not built` for both `SimpleSampleApp` and `Uno.Themes.Samples.RuntimeTests`. Same upstream baseline issue documented in Round 6.

### Conclusion

The TFM mismatch was a real bug — it produced the cascade observed in earlier rounds — but it was **orthogonal** to the EnC baseline issue. Aligning the TFM eliminates the cascade and the Logging.Console downgrade, but `Microsoft.CodeAnalysis.WatchHotReloadService` still fails to apply the IL because no PE/PDB baseline was emitted by `Uno.UI.RemoteControl.Server.Processors.CompilationWorkspaceProvider.CreateWorkspaceAsync`. The fix from Round 6 (upstream patch in `Uno.UI.RemoteControl`) is still required.

The Round 7 changes are kept regardless because:
- They make the Debug workspace coherent (no more spurious cascade noise in any HR diagnostic the dev-server outputs).
- They mirror the known-working `artifacts/uno.extensions` setup as closely as possible while keeping the apps on net10.0.
- They produce a clean substrate when the upstream baseline patch lands — there's no second TFM problem to chase afterwards.
2. *(optional)* propagate `UnoHotReloadDiagnosticsLogPath` through the DevServer.targets so end users can opt in to the Roslyn EnC trace from a `<PropertyGroup>` instead of an env var.

## Round 8 (2026-05-06) — Round 6's conclusion was wrong: `project not built` is not the bug. **MVID mismatch** is the real cause.

Empirical check: ran the HR test from `artifacts/uno.extensions` on this machine. All 5 tests pass in ~22 seconds, including `When_UpdateMethodBody_Then_MetadataUpdateReceived` (the equivalent of the test that fails in Themes). The machine and the environment work — the bug is specific to Themes.

The `Roslyn EnC Session.log` from Uno.Extensions shows EXACTLY the same "alarming" messages as Themes:
- `Emitting update of Uno.Extensions.RuntimeTests(net9.0-desktop) ... : project not built`
- `Project Uno.Extensions.Reactive.WinUI.csproj doesn't support EnC: no generated files output directory`

Yet the tests pass. **Round 6 had incorrectly identified `project not built` as a blocker** — these two messages are actually informational and do not prevent EnC from working.

### The real cause: MVID mismatch between dev-server and published binary

MVID dump verification:

| | Published binary on disk | MVID that the dev-server pushes via `Apply IL Delta` |
|---|---|---|
| Themes `Uno.Themes.Samples.RuntimeTests.dll` | `12042264-b46b-45fd-be4a-81413828c7ec` | `fc7a4b6e-210c-4554-b705-a9ea5e9de52d` ❌ |
| Extensions `Uno.Extensions.Reactive.WinUI.Tests.dll` | `69e21d74-028e-4fd5-a7a4-5a9bf11a401e` | `69e21d74-028e-4fd5-a7a4-5a9bf11a401e` ✅ |

In Extensions the MVIDs are identical. In Themes they differ. So the runtime calls `MetadataUpdater.ApplyUpdate(asm, ...)` with an MVID that does not exist in the loaded modules, the delta is silently ignored (no error from the .NET runtime), and `[HR-DIAG:DirectVsReflection] direct=original reflection=original` is exactly what we'd expect to see.

### Why the MVID differs in Themes

The Themes test lib has two path-dependent assembly-level attributes, emitted by internal generators in the `Uno.UI.RuntimeTests.Engine` package (which enables `UnoForceHotReloadCodeGen=true`):

```csharp
[assembly: __Uno.HotReload.HotReloadInfoAttribute("/studio-live/artifacts/Uno.Themes/src/samples/Uno.Themes.Samples.RuntimeTests/obj/Debug/net10.0-desktop/uno.hot-reload.info/HotReloadInfo.g.cs")]
[assembly: Uno.UI.RuntimeTests.Engine.RuntimeTestsSourceProjectAttribute("/studio-live/artifacts/Uno.Themes/src/samples/Uno.Themes.Samples.RuntimeTests/Uno.Themes.Samples.RuntimeTests.csproj")]
```

These strings encode absolute paths. When the dev-server loads the project via `MSBuildWorkspace`, its MSBuild evaluation resolves these paths slightly differently from what `dotnet build` / `dotnet publish` writes (typically because a `WriteCodeFragment` pass does not run inside the workspace, or because MSBuildWorkspace canonicalisation differs). The contents of the in-memory compiled assembly differ from those of the binary on disk → Roslyn MVID ≠ loaded-binary MVID.

On the Uno.Extensions side: the `Uno.Extensions.Reactive.WinUI.Tests` test lib has NEITHER `HotReloadInfoAttribute` NOR `RuntimeTestsSourceProjectAttribute` at assembly level. The Engine package lives in `Uno.Extensions.RuntimeTests.Core` (a separate project), so only Core carries those attributes. The test lib stays neutral on the metadata side → its MVID is stable between dev-server and publish → HR works.

### What was tried on the Themes side

Two approaches tried in this session:

1. **Disable the generators** on the test lib (`<UnoForceHotReloadCodeGen>false</UnoForceHotReloadCodeGen>` + `<UnoGenerateHotReloadInfo>false</UnoGenerateHotReloadInfo>`). Effect: `HotReloadInfoAttribute` disappears from the binary, MVID effectively aligned between publish and dev-server (`Guid:175725c5-...` on the delta side = `MVID=175725c5-...` on the binary side). BUT the test still fails with `Expected:<updated>. Actual:<original>` after 197s — IL is dispatched but `direct=original reflection=original`. Something else prevents the IL mutation despite the MVID alignment.

2. **"Core split" restructuring**: create `Uno.Themes.Samples.RuntimeTests.Core` with the Engine PackageReference, move the dependency into the test lib via ProjectReference, add `[InternalsVisibleTo]` for the `UnitTestsUIContentHelper.Content` usages (setter is `internal`), pin `Microsoft.Extensions.Logging` (10.0.2) and `Uno.UI.Adapter.Microsoft.Extensions.Logging` in samples Directory.Packages.props, add a direct ProjectReference to Core from `SimpleSampleApp` so that `Uno.WinUI.DevServer` (PrivateAssets="all") flows up to the head. Effect: the test lib ends up clean on the assembly-attributes side (zero path-dependent) AND its MVID is stable. BUT the test runner stops starting altogether — 300s timeout with no test-runner execution log (only the `Couldn't statically resolve resource Material*Style` warnings then silence). A secondary-app init regression introduced by my reference changes; I did not take the time to dig into it. Reverted.

Approach 2 had the correct analysis (MVID is the real problem) but the implementation broke something else. The direction is right, the execution needs refining.

### State kept after Round 8

Kept (since Round 7):
- `tfm-common-winui.props`: `net10.0-desktop` widening on the libs in Debug.
- `Uno.Simple.WinUI.csproj`: `Sdk="Uno.Sdk"` (instead of `Microsoft.NET.Sdk`).
- 4 lib csprojs: `TreatAsLocalProperty="TargetFramework;TargetFrameworks;RuntimeIdentifier"`.
- `src/samples/Directory.Packages.props`: `CentralPackageTransitivePinningEnabled=true` + pin `Microsoft.Extensions.Logging.Console=10.0.2`.

Reverted to original state:
- The Core split (deletion of `Uno.Themes.Samples.RuntimeTests.Core/`).
- The `UnoForceIncludeProjectConfiguration=false` override on `SimpleSampleApp.csproj`.

⚠️ The `git checkout` of the revert also reset the test path to `"src/samples/Uno.Themes.Samples.RuntimeTests/HotReload/HotReloadTarget.cs"` (the Round 1 bug) — needs to be reset to `"HotReload/HotReloadTarget.cs"`.

### Action items to finish the fix (out of session)

1. **Reproduce the full solution** on the Themes side mirroring Uno.Extensions:
   - Create a `Uno.Themes.Samples.RuntimeTests.Core` project that takes the `Uno.UI.RuntimeTests.Engine` PackageReference + `Uno.WinUI.DevServer` + the `Microsoft.Extensions.Logging` / `Uno.UI.Adapter.Microsoft.Extensions.Logging` dependencies.
   - Remove these packages from `Uno.Themes.Samples.RuntimeTests.csproj`; add a ProjectReference to Core.
   - Add `[InternalsVisibleTo]` Core → test-lib for `UnitTestsUIContentHelper`'s `internal` setters.
   - Direct ProjectReference to Core from each sample app head (Simple / Material / Cupertino) so DevServer flows.
   - Update the path in `Given_HotReload.cs`: `"../Uno.Themes.Samples.RuntimeTests/HotReload/HotReloadTarget.cs"`.
   - Add PackageVersion entries in `src/samples/Directory.Packages.props` for `Microsoft.Extensions.Logging` and `Uno.UI.Adapter.Microsoft.Extensions.Logging`.
   - **Diagnose the secondary-app init regression** observed during this session (300s timeout with no test runner log). Suspects: `[Application]` source-gen interaction when the engine lives in Core; ProjectReference ordering; something in `App.xaml.cs` that assumes `HotReloadHelper.Assembly == typeof(MyApp).Assembly`.

2. **Restore the relative path** in `Given_HotReload.cs`: `"HotReload/HotReloadTarget.cs"` (the current repo-rooted path will reproduce the Round 1 bug).

## Round 9 (2026-05-07) — ✅ HR test passes. Round 8 was a false lead, Round 7 was the complete solution.

After resetting the invasive changes and restoring the relative path `"HotReload/HotReloadTarget.cs"` in `Given_HotReload.cs`, full clean rebuild (`rm -rf obj/ bin/` then `dotnet publish -c Debug -f net10.0-desktop`), the HR test passes:

```
<test-case name="When_UpdateMethodBody_Then_MetadataUpdateReceived(System.Threading.CancellationToken)" duration="15.8997662" result="Passed" />
```

3 successive runs: 14.7s, 14.4s, 14.6s. Stable.

### MVID verification — natural alignment after a clean build

```
$ od -A x -t x1z -v Uno.Themes.Samples.RuntimeTests.dll | grep MVID  ← deployed binary
MVID=b7012c38-0601-4e68-8022-e3bc59dcd2bf

$ grep "Apply IL Delta after.*HotReloadTarget" /tmp/hr-r9b.log
            Applying IL Delta after .../HotReloadTarget.cs, Guid:b7012c38-0601-4e68-8022-e3bc59dcd2bf  ← matches!
```

Same for SimpleSampleApp (`3e2d8ee6-...` on both sides). With a clean build, the dev-server's `MSBuildWorkspace` and `dotnet publish` produce the **same MVID** — even with `HotReloadInfoAttribute` and `RuntimeTestsSourceProjectAttribute` carrying absolute paths.

### Correction to Round 8

Round 8 was based on a correct observation (diverging MVIDs) but a wrong INTERPRETATION:

- **What was observed**: MVID `12042264-...` on the binary side vs `fc7a4b6e-...` on the delta-apply side.
- **What I had inferred**: "the absolute path encoded in `HotReloadInfoAttribute` is canonicalised differently between `MSBuildWorkspace` and `dotnet publish` → divergent metadata content → divergent MVID". This hypothesis was never directly verified by a byte-by-byte dump.
- **What was actually causing it**: `obj/` staleness accumulated through Round 6-8 iterations (I had successively added/removed `[MethodImpl]` on `HotReloadTarget.GetValue`, disabled/re-enabled `UnoForceHotReloadCodeGen`, created/deleted a Core project). The `Uno.Sdk` incremental build did not consistently re-invalidate source-gen outputs across these variations, and the dev-server's `MSBuildWorkspace`, which restarts from a clean graph each launch, ended up seeing a different state from the published binary.
- **How a Round 9 clean rebuild resolves it**: `rm -rf src/samples/SimpleSampleApp/obj src/samples/Uno.Themes.Samples.RuntimeTests/obj` before `dotnet publish` → publish and workspace agree on each source-gen output's content → identical MVIDs.

So the "Core split" approach attempted in Round 8 was not necessary: it addressed a problem that only existed under the effect of my own build staleness.

### Final working configuration (to keep)

All substantive changes come from Round 7:

- [src/library/tfm-common-winui.props](../../src/library/tfm-common-winui.props): `<TargetFrameworks>$(TargetFrameworks);net10.0-desktop</TargetFrameworks>` under `Configuration=Debug`. Eliminates the CS0234/CS0246 cascade when the dev-server propagates `TargetFrameworks=net10.0-desktop` to the libs.
- [src/library/Uno.Simple.WinUI/Uno.Simple.WinUI.csproj](../../src/library/Uno.Simple.WinUI/Uno.Simple.WinUI.csproj): `Sdk="Uno.Sdk"` (instead of `Microsoft.NET.Sdk`) — required for the `desktop` TPI to be recognised.
- 4 lib csprojs: `TreatAsLocalProperty="TargetFramework;TargetFrameworks;RuntimeIdentifier"` — defense-in-depth against the dev-server's MSBuild global property propagation.
- [src/samples/Directory.Packages.props](../../src/samples/Directory.Packages.props): `<CentralPackageTransitivePinningEnabled>true</CentralPackageTransitivePinningEnabled>` + pin `<PackageVersion Include="Microsoft.Extensions.Logging.Console" Version="10.0.2" />` — resolves the NU1605 caused by the implicit-package mismatch between Uno.Sdk (libs) and Uno.Sdk.Private (head).

Clean local validation procedure:

```bash
cd /studio-live/artifacts/Uno.Themes
rm -rf src/samples/SimpleSampleApp/obj src/samples/Uno.Themes.Samples.RuntimeTests/obj
dotnet publish -c Debug -f net10.0-desktop -p:TargetFrameworkOverride=desktop -p:PublishTrimmed=false src/samples/SimpleSampleApp/SimpleSampleApp.csproj
cd src/samples/SimpleSampleApp/bin/Debug/net10.0-desktop
DOTNET_MODIFIABLE_ASSEMBLIES=debug \
UNO_RUNTIME_TESTS_RUN_TESTS='{"Filter":{"Value":"HotReload"}}' \
UNO_RUNTIME_TESTS_OUTPUT_PATH=/tmp/hr.xml \
xvfb-run --auto-servernum --server-args='-screen 0 1280x1024x24' \
  bash -c '{ fluxbox & } ; dotnet SimpleSampleApp.dll --runtime-tests=/tmp/hr.xml'
grep result= /tmp/hr.xml   # → result="Passed", duration="15.x"
```

### Comment update

The long Round 7 comments in [tfm-common-winui.props](../../src/library/tfm-common-winui.props), [Uno.Simple.WinUI.csproj](../../src/library/Uno.Simple.WinUI/Uno.Simple.WinUI.csproj) and [src/samples/Directory.Packages.props](../../src/samples/Directory.Packages.props) are still valid — they correctly describe the rationale of each property. The comment in `HotReloadTarget.cs` that mentioned "[MethodImpl] attributes broke EnC IL apply on this codebase; cf. progress.md Round 8" is on the other hand incorrect: the absence of `[MethodImpl]` is in place (as in Uno.Extensions), but that attribute was not what was breaking HR.