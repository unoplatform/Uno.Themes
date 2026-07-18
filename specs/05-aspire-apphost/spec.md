# Spec 05 — Aspire AppHost for sample launching & runtime-test orchestration

- **Status:** Draft — awaiting review before implementation
- **Branch:** `dev/sb/aspire`
- **Date:** 2026-07-17
- **Reference implementation:** `artifacts/studio.live/src/AppHost/StudioLive.AppHost` (Aspire 13.2.4 AppHost orchestrating Uno client heads; see its `README.md` and `.claude/skills/studio-live-aspire/SKILL.md`)

## 1. Problem

Uno.Themes has three sample heads (Material, Cupertino, Simple) × several targets (desktop, WASM, Android, iOS), plus a runtime-test suite that runs *inside* SimpleSampleApp. Today every launch is a hand-typed `dotnet run`/`dotnet build` incantation with per-target flags, and the runtime tests need a separate script with four env vars. There is no single place to see what is running, read logs, restart a head, or kick off a filtered test run.

studio.live solved the same problem for its client apps with a .NET Aspire AppHost: one `dotnet run` opens a dashboard where every app target is a stopped tile — click Start, watch logs, Stop/Restart, override env vars per run. The client-orchestration half of that design ports to Uno.Themes almost directly, and **without studio.live's Docker prerequisite** because Uno.Themes has no backend: every resource here is a plain `AddExecutable`.

## 2. Goals

1. `dotnet run --project src/AppHost/Uno.Themes.AppHost` brings up the Aspire dashboard with every sample target and the runtime-test runner as **stopped** (`WithExplicitStart()`) resources. AppHost boot itself is instant — nothing builds or launches until a tile is started.
2. Desktop + WASM heads for all three samples startable from the dashboard, with logs streamed and the WASM dev-server URLs surfaced as clickable endpoints.
3. Android dev loop (emulator lifecycle + build/install/launch/logcat) for the Simple sample on Linux/macOS/WSL, ported from studio.live's two-resource pattern. Scripts stay parameterized so Material/Cupertino Android tiles are a later registration-only add.
4. A `simple-runtime-tests` resource that builds and runs the SimpleSampleApp runtime tests headlessly-or-windowed, honoring the `UNO_RUNTIME_TESTS_RUN_TESTS` filter, with a nonzero exit on failure so the tile goes red.
5. Zero impact on the seven shipped NuGet packages, existing CI pipelines, and existing local flows (`crosstargeting_override.props`, direct `dotnet run` of sample heads).

## 3. Non-goals

- **No CI migration.** `build/stage-runtimetests-desktop.yml` + `build/scripts/linux-skia-desktop-runtime-tests.sh` stay the CI entry point unchanged. The AppHost is a local dev-loop tool.
- **No iOS / macCatalyst / Windows-native resources** in v1. iOS requires a macOS host + simulator plumbing studio.live hasn't built either; add later if wanted.
- **No backend/container resources** — no Docker requirement, no `Aspire.Hosting.Azure.*` packages.
- **No `DistributedApplicationTestingBuilder` test project.** Rationale in §8.
- **No changes to sample app code.** Unlike studio.live (which needed `MainActivity` intent-extra plumbing for licensing), the sample heads need no login/backend wiring — the Android launcher just installs and starts them.

## 4. Design

### 4.1 Project layout

```
src/AppHost/
  README.md                          # human-facing: prerequisites, run, troubleshoot (adapted from studio.live)
  Uno.Themes.AppHost/
    Uno.Themes.AppHost.csproj        # net10.0, Aspire.AppHost.Sdk + Aspire.Hosting.AppHost 13.2.4
    Program.cs                       # the resource graph
    appsettings.json                 # log levels (Aspire.Hosting.Dcp → Warning)
    Properties/launchSettings.json   # dashboard ports, https + http profiles
    scripts/
      start-emulator.sh              # ported from studio.live (see §4.5)
      launch-android.sh              # ported from studio.live (see §4.5)
      run-runtime-tests.sh           # new (see §4.6)
      lib/android-sdk-bootstrap.sh   # ported from studio.live
```

The csproj must counteract root `Directory.Build.props` defaults that assume every non-Sample/non-Test project is a packable library:

```xml
<IsPackable>false</IsPackable>
<GeneratePackageOnBuild>false</GeneratePackageOnBuild>
<SourceLinkEnabled>false</SourceLinkEnabled>
```

(Without these, a Release build of the solution would try to pack the AppHost and inject SourceLink/ReproducibleBuilds.)

Aspire versions are pinned directly in the csproj — the repo does not use Central Package Management. Two places must stay in lockstep on future bumps: the `<Sdk Name="Aspire.AppHost.Sdk" Version="…"/>` element and the `Aspire.Hosting.AppHost` PackageReference. Start at **13.2.4** (the version studio.live has validated against SDK 10.0.x; local SDK is 10.0.300).

### 4.2 Resource graph

All resources are `AddExecutable` + `WithExplicitStart()`. `AddProject` is deliberately avoided: studio.live's experience is that the `Projects.*` source generator mis-handles multi-TFM Uno heads; `dotnet run` with an explicit `-f` + `--launch-profile` is the reliable path.

| Resource | Command | Notes |
| --- | --- | --- |
| `material-desktop` | `dotnet run --project ../../samples/MaterialSampleApp/MaterialSampleApp.csproj -f net10.0-desktop --launch-profile "MaterialSampleApp (Desktop)"` | `DOTNET_MODIFIABLE_ASSEMBLIES=debug` pinned via `WithEnvironment` (hot reload) |
| `cupertino-desktop` | same shape, profile `"CupertinoSampleApp (Desktop)"` | |
| `simple-desktop` | same shape, profile `"SimpleSamplesApp (Desktop)"` | note the profile name really is `SimpleSamplesApp` (extra `s`) |
| `material-wasm` | `dotnet run … -f net10.0-browserwasm --launch-profile "MaterialSampleApp (WebAssembly)"` | `WithHttpEndpoint(port: 5000, isProxied: false)` + `WithExternalHttpEndpoints()` |
| `cupertino-wasm` | same shape, profile `"CupertinoSampleApp (WebAssembly)"` | port **5001** — requires the launchSettings re-pin in §4.4 |
| `simple-wasm` | same shape, profile `"SimpleSamplesApp (WebAssembly)"` | port 5002 |
| `android-emulator` | `bash scripts/start-emulator.sh` | registered only when `!OperatingSystem.IsWindows()` **and** `ANDROID_AUTO_START_EMULATOR != "0"` |
| `simple-android` | `bash scripts/launch-android.sh` with `ANDROID_PROJECT=../../samples/SimpleSampleApp/SimpleSampleApp.csproj`, `ANDROID_PACKAGE=uno.platform.themes.simple` | non-Windows only; `WaitFor(android-emulator)` + start-kick (§4.3). Material/Cupertino Android tiles deferred (Q1); scripts stay parameterized so adding them later is a pure registration change. |
| `simple-runtime-tests` | `bash scripts/run-runtime-tests.sh` | §4.6; runs on any OS with bash (non-Windows only in v1, same gate as the other scripts) |

The explicit `--launch-profile` on desktop resources matters: the WASM profile is listed first in each sample's `launchSettings.json` (so bare `dotnet run` picks it), and Aspire-managed launches must not depend on profile ordering.

**Each resource also passes `-p:TargetFrameworkOverride=<platform>` (Phase 1 finding).** Without it, `dotnet run -f net10.0-desktop` still evaluates every platform TFM of the referenced multi-targeted libraries (net9.0-ios / -android, net10.0-android) and fails with `NETSDK1147` demanding workloads that aren't installed — `-f` does not cascade to dependencies. `TargetFrameworkOverride` is the repo's single-platform collapse switch (AGENTS.md §4). Passing it as a command-line `-p:` property additionally makes each resource immune to whatever `crosstargeting_override.props` a developer has set (command-line globals win), which is *stronger* than the §6 caveat originally anticipated — the override note there is superseded by this.

### 4.3 Patterns carried over from studio.live (verbatim, with rationale)

- **`WithExplicitStart()` on everything.** Keeps AppHost boot instant; no unsolicited windows or cold WASM builds.
- **`isProxied: false` endpoints** for the WASM dev servers — the Uno bootstrap binds its own port from `launchSettings.json`; Aspire should display the real URL, not a forwarder.
- **Env-forwarding allowlist (`ForwardFromHostEnv` helper).** Aspire executables do **not** inherit arbitrary parent env vars. Forward explicitly: `ANDROID_AVD_NAME`, `ANDROID_SDK_API_LEVEL`, `ANDROID_SDK_IMAGE`, `ANDROID_EMULATOR_WINDOW`, `ANDROID_EMULATOR_GPU`, `DISPLAY` (emulator resource); `ANDROID_SERIAL` (android launchers); `UNO_RUNTIME_TESTS_RUN_TESTS`, `UNO_RUNTIME_TESTS_OUTPUT_PATH` (runtime-tests resource). Empty/unset parent values are skipped so `${VAR:-default}` fallbacks in the scripts keep working.
- **Two-resource Android split + start-kick.** `WaitFor(emulator)` alone does not start an `ExplicitStart` dependency; subscribe to `BeforeResourceStartedEvent` on the `simple-android` resource and issue `KnownResourceCommands.StartCommand` for the emulator when it isn't already Running (idempotent). Emulator lifecycle stays bound to the AppHost process (SIGTERM → `adb emu kill`), so it survives app restart cycles but never outlives the AppHost.
- **`workingDirectory` is relative to the AppHost csproj directory**, not the output dll — all `../../samples/...` paths above assume this.
- **Dashboard env overrides** remain the one-off tuning mechanism (e.g. set a test filter for a single run without touching the shell).

### 4.4 Cupertino WASM port re-pin (the only edit outside `src/AppHost/`)

`MaterialSampleApp` and `CupertinoSampleApp` both pin `http://localhost:5000` in their `(WebAssembly)` profiles today — fine when running one at a time, a collision under an orchestrator built to run them side by side. Change Cupertino's `"CupertinoSampleApp (WebAssembly)"` `applicationUrl` to `http://localhost:5001` (Simple already owns 5002). Implementation must grep `doc/` and sample sources for hardcoded `localhost:5000` references to Cupertino before assuming the edit is free.

### 4.5 Android scripts — porting notes

`start-emulator.sh`, `launch-android.sh`, and `lib/android-sdk-bootstrap.sh` port from studio.live nearly verbatim (they are already parameterized via `ANDROID_PROJECT` / `ANDROID_PACKAGE` / `ANDROID_CONFIG` / `ANDROID_AVD_NAME` / etc.). Deltas:

1. **Strip studio-specific plumbing:** the `UNO_STUDIO_API_KEY` intent-extra auto-login and the `STUDIOLIVE_BACKEND_BASE_URL` forwarding (steps 5–6 of their launcher pipeline lose the `--es …` arguments; `am start` keeps the dynamic LAUNCHER-activity resolution).
2. **Rename defaults:** AVD name default `uno-themes` (was `studio-live`); comments/headers reference Uno.Themes resources.
3. **Keep all tunables** (`ANDROID_AUTO_START_EMULATOR`, `DEVICE_WAIT_TIMEOUT_S`, `ANDROID_EMULATOR_WINDOW`, `ANDROID_EMULATOR_GPU`, `ANDROID_SDK_API_LEVEL`, `ANDROID_SDK_IMAGE`, `ANDROID_SERIAL`, `ANDROID_CONFIG`) and behaviors (SDK lazy-bootstrap, reuse of an already-running emulator, `sys.boot_completed` polling, `dotnet build -t:InstallAndroidDependencies` self-tuning SDK install, `adb logcat --pid` as the foreground process).
4. **Line endings:** the repo defaults to CRLF; bash scripts must be LF. Add a `.gitattributes` rule (`*.sh text eol=lf`) if the repo doesn't already carry one.

### 4.6 `run-runtime-tests.sh` (new)

A single foreground process so Aspire's Start/Stop/logs semantics apply, mirroring the CI script's mechanics but improving on its failure detection:

1. `dotnet build src/samples/SimpleSampleApp/SimpleSampleApp.csproj -c $CONFIG -f net10.0-desktop` (`CONFIG` default `Debug` — day-to-day iteration speed; CI keeps its Release publish).
2. Export `DOTNET_MODIFIABLE_ASSEMBLIES=debug` (mandatory — `Given_HotReload` fails silently without it), `UNO_RUNTIME_TESTS_RUN_TESTS` (default `'{}'` = run everything), `UNO_RUNTIME_TESTS_OUTPUT_PATH` (default under the AppHost's obj/ or `$TMPDIR`).
3. Run `dotnet <bin>/SimpleSampleApp.dll --runtime-tests="$UNO_RUNTIME_TESTS_OUTPUT_PATH"`. If `DISPLAY` is set (WSLg / desktop Linux / macOS), run directly — the window is visible and watchable; otherwise fall back to `xvfb-run` + `fluxbox` exactly like CI.
4. Post-run: exit nonzero if the results XML is missing, contains zero `<test-case>` elements, **or contains any failed case** — stricter than the CI script's existence check, so the dashboard tile reliably turns red on test failures. Print a one-line pass/fail summary with counts as the last log line.

Filtered runs: set `UNO_RUNTIME_TESTS_RUN_TESTS` in the shell before launching the AppHost (forwarded via the allowlist), or override it on the resource in the dashboard's Details panel, then Start.

### 4.7 Solution integration

Add `Uno.Themes.AppHost` to `Uno.Themes.sln` under a new `apphost` solution folder (sibling of `library` / `samples`), so IDE users see it. It must **not** be added to `Uno.Themes-packages.slnf` (that filter is an explicit include-list, so no action needed beyond not adding it).

Risk to verify during implementation: any CI stage that builds `Uno.Themes.sln` wholesale will now also build the AppHost. Expected to be benign (net10.0 SDK is already required by the samples; `IsPackable=false` guards packaging), but the implementer must check each `build/stage-*.yml` for solution-level builds and confirm — falling back to keeping the project out of the .sln (standalone, `dotnet run --project` only) if anything objects.

## 5. Prerequisites (documented in `src/AppHost/README.md`)

- .NET SDK 10.0.x (already required by the sample heads). **No Docker.**
- Dashboard over HTTPS wants a trusted dev cert (`dotnet dev-certs https --trust`); ship an `http` launch profile as the fallback, mirroring studio.live.
- Android resources only: `adb` on PATH, `dotnet workload install android`, KVM for emulator acceleration; everything else (cmdline-tools, system image, AVD) is lazily bootstrapped by the scripts.
- Headless runtime tests only: `xvfb-run` + `fluxbox` (already CI-standard); not needed when `DISPLAY` is available.

## 6. Interactions with existing local flows

- **`crosstargeting_override.props`:** *mitigated* — each resource passes `-p:TargetFrameworkOverride=<platform>` on the command line, and a command-line global property wins over the in-project reassignment in `crosstargeting_override.props`. So the AppHost builds the platform each resource intends regardless of the dev's local override file; no need to clear it first. (README still notes the relationship for anyone debugging a build.)
- **First WASM start is slow** (cold Uno bootstrap build, minutes); subsequent starts are seconds. README + a log-line expectation note.
- **Ports are shared by design** with direct `dotnet run` launches (`isProxied: false` reuses the launchSettings pins), so an AppHost-launched WASM head and a manually-launched one for the same sample cannot run simultaneously — same conflict as two manual launches today.
- Concurrent Starts of multiple heads trigger concurrent `dotnet build`s; each head has its own project/obj so this is safe, just CPU-noisy.

## 7. What is intentionally *not* taken from studio.live

| studio.live piece | Why not |
| --- | --- |
| `AddAzureStorage().RunAsEmulator()` / Azurite / connection-string wiring | No backend in Uno.Themes; removing this eliminates the Docker prerequisite entirely. |
| `AddProject` + `/health` probes + launchSettings endpoint discovery for services | No ASP.NET Core services here. |
| `UNO_STUDIO_API_KEY` auto-login + intent-extra env relay in `MainActivity` | Sample apps have no licensing/login gate. |
| dind devcontainer changes | Docker not needed at all. |
| `StudioLive.AppHost.Tests` smoke tests | See §8. |

> **Correction (Phase 0 finding):** the `MessagePack` direct pin **is** needed after all — it arrives transitively via the Aspire host's StreamJsonRpc dependency at the vulnerable 2.5.192 (NU1903 high). Pinned to 2.5.301 in the AppHost csproj, same as studio.live. This row was originally in the "not taken" list; restore proved otherwise.

## 8. Testing & verification strategy (Exceptions Process per AGENTS.md §4)

**No runtime tests are added** under `src/samples/SimpleSampleApp/RuntimeTests/`, and no AppHost test project is created.

- **Constraint:** AGENTS.md requires tests for "new public behavior in the theme libraries". The AppHost is unpackaged dev tooling — it adds no library behavior, no public API, no resource keys. An Aspire smoke test in the studio.live style would assert nothing useful here: every resource is `ExplicitStart`, so a booted AppHost has no health state to observe; actually starting resources from a test means multi-minute sample builds and an Android SDK — unusable as a PR gate.
- **Impact:** regressions in the AppHost graph (typo'd project path, wrong profile name) surface at first interactive use rather than in CI.
- **Mitigation:** the verification checklist in `progress.md` exercises every resource end-to-end (each desktop head, each WASM head, a full + a filtered runtime-test run, and the Android loop where the environment allows) before the PR is opened, and the PR description records the results. A follow-up issue may add an opt-in smoke test (`[TestCategory("Integration:Aspire")]`-style, excluded from CI) if graph rot proves to be a real problem.

The **behavioral guarantee that matters** — the runtime tests themselves — is unchanged and still enforced by the existing CI pipeline.

## 9. Documentation plan

- `src/AppHost/README.md` — adapted from studio.live's: prerequisites, run, per-resource docs, tunables table, troubleshooting (minus all Docker/backend content).
- New skill `.claude/skills/uno-themes-apphost/SKILL.md` — agent-facing: when to use the AppHost vs direct `dotnet run`, resource/command reference, versioning (the two-place Aspire pin), troubleshooting.
- `.claude/skills/uno-themes-runtime-tests/SKILL.md` — add a short section pointing at the `simple-runtime-tests` resource as an alternative launcher (existing content stays authoritative for CI parity).
- `AGENTS.md` `<repository_orientation>` — one bullet for `src/AppHost/` (dev-only orchestrator, not packable) in the solution-layout list.
- `doc/` (published docs) — **no changes**: this is contributor tooling, not product surface.

## 10. Risks

| Risk | Mitigation |
| --- | --- |
| Aspire 13.2.4 mis-pairs with SDK 10.0.300 or the repo's NuGet feeds lack the packages | Validate restore as the first implementation step; studio.live runs the same pairing (10.0.203). |
| CRLF corruption of the bash scripts on checkout | `.gitattributes` `*.sh text eol=lf` entry. |
| Root `Directory.Build.props` side effects on the AppHost (packing, SourceLink, output-path remap) | Explicit `IsPackable=false` / `GeneratePackageOnBuild=false` / `SourceLinkEnabled=false`; verify with a Release solution build that no `Uno.Themes.AppHost.nupkg` is produced. |
| CI stages that build `Uno.Themes.sln` pick up the AppHost and fail | Audit `build/stage-*.yml` during implementation; fall back to standalone project (not in .sln). |
| Cupertino port re-pin breaks a documented URL | Grep `doc/` + samples for `localhost:5000` before the edit. |
| Windows-host contributors get a reduced resource set (no Android, no runtime-tests scripts — bash-only) | Same posture as studio.live: documented "run from WSL" guidance; PowerShell siblings are a possible follow-up. |

## 11. Decisions (resolved 2026-07-18)

1. **Scope of v1 Android:** `simple-android` only (+ `android-emulator`). Material/Cupertino Android tiles deferred; scripts stay parameterized so they're a pure registration add later.
2. **Solution membership:** add to `Uno.Themes.sln` under a new `apphost` folder; standalone is the fallback only if the Phase 4 CI audit surfaces a regression.
3. **Resource naming:** by-sample (`material-desktop`, `material-wasm`, …).
4. **`simple-runtime-tests` default config:** `Debug` (overridable via `CONFIG`).
