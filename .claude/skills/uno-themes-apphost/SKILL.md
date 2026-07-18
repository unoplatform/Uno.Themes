---
name: uno-themes-apphost
description: Run, extend, and troubleshoot the Uno.Themes Aspire AppHost (`src/AppHost/Uno.Themes.AppHost`) that launches the sample heads (Material/Cupertino/Simple × desktop/WASM), the Simple Android dev loop, and the runtime-test runner from one dashboard. Use when adding or debugging an AppHost resource, wiring a new sample target, changing the runtime-test launcher, bumping Aspire, or diagnosing why a resource won't start.
metadata:
  author: uno-platform
  version: "1.0"
  category: tooling
---

# Uno.Themes Aspire AppHost

A .NET Aspire AppHost at `src/AppHost/Uno.Themes.AppHost` orchestrates local dev of the Uno.Themes samples + runtime tests. It is **dev-only tooling** — not packaged, not built by CI, and has **no backend and no Docker dependency** (every resource is a plain `dotnet run` / `bash` executable, all `WithExplicitStart()`).

Run it:

```sh
dotnet run --project src/AppHost/Uno.Themes.AppHost
```

The dashboard URL (with a login token) prints to the console. Each resource is a Stopped tile — click Start to build/launch, read Logs, Stop/Restart, or override env in the Details panel. `Ctrl+C` tears everything down.

## Resource graph (`Program.cs`)

- `material-desktop` / `cupertino-desktop` / `simple-desktop` — `dotnet run -f net10.0-desktop --launch-profile "<Sample> (Desktop)"`, `DOTNET_MODIFIABLE_ASSEMBLIES=debug`.
- `material-wasm` / `cupertino-wasm` / `simple-wasm` — `dotnet run -f net10.0-browserwasm`, `WithHttpEndpoint(port, isProxied:false)` on **5000 / 5001 / 5002**.
- `simple-runtime-tests` — `bash scripts/run-runtime-tests.sh` (non-Windows).
- `android-emulator` + `simple-android` — the Android dev loop (non-Windows; emulator gated on `ANDROID_AUTO_START_EMULATOR != "0"`).

## Non-obvious rules (get these wrong and resources fail)

1. **`-p:TargetFrameworkOverride=<platform>` is mandatory on every sample resource.** `-f net10.0-desktop` alone does NOT restrict the referenced multi-targeted libraries' TFMs — `dotnet run` evaluates them all and fails with `NETSDK1147` (missing android/ios/wasm workloads). The override collapses the graph to one platform. As a command-line global it also overrides any dev `crosstargeting_override.props`. (See `specs/lessons.md`.)
2. **`AddExecutable`, not `AddProject`.** The `Projects.*` source generator mishandles multi-TFM Uno heads. Use `dotnet run` with explicit `-f` + `--launch-profile`.
3. **`isProxied: false`** on WASM endpoints so the dashboard surfaces the Uno bootstrap dev server's real URL, not an Aspire forwarder. Ports must match the sample's `(WebAssembly)` launch profile.
4. **Explicit `--launch-profile`** on desktop resources — the WASM profile is first in each sample's `launchSettings.json`, so bare `dotnet run` would pick the wrong one.
5. **Aspire executables don't inherit parent env vars.** Forward only via the `ForwardFromHostEnv(resource, "NAME", …)` allowlist helper. Empty parent values are skipped so scripts' `${VAR:-default}` fallbacks survive.
6. **`workingDirectory` is relative to the AppHost csproj dir**, not the built dll. Sample paths are `../../samples/<App>/<App>.csproj`.
7. **Explicit-start dependencies aren't auto-started by `WaitFor`.** `simple-android` `WaitFor`s `android-emulator` AND subscribes to `BeforeResourceStartedEvent` to issue `KnownResourceCommands.StartCommand` on the emulator (idempotent — no-op if already Running). This is what makes clicking Start on `simple-android` pull the emulator up transparently.

## Adding a resource

- **Another sample target** (e.g. `material-android`): reuse the parameterized launcher — `builder.AddExecutable("material-android", "bash", ".", "scripts/launch-android.sh").WithEnvironment("ANDROID_PROJECT", "../../samples/MaterialSampleApp/MaterialSampleApp.csproj").WithEnvironment("ANDROID_PACKAGE", "uno.platform.themes.material").WithExplicitStart()`, then `WaitFor` + start-kick like `simple-android`.
- **A desktop/WASM head**: use the `AddDesktopSample` / `AddWasmSample` local helpers — they already bake in the override, hot-reload env, and endpoint wiring.
- Keep everything `WithExplicitStart()` so AppHost boot stays instant.

## Verifying changes without launching everything

- **Enumerate the graph** (no build/launch): `dotnet run --project src/AppHost/Uno.Themes.AppHost --no-build -- --operation publish --publisher manifest --output-path /tmp/manifest.json`, then inspect resource names/commands/args/bindings. Catches typo'd paths, wrong profiles, missing endpoints.
- **Boot smoke** (empty of container deps): `dotnet run … --launch-profile http` under a timeout; confirm "Distributed application started" and clean teardown.
- **Scripts**: `bash -n scripts/*.sh` for syntax; run `scripts/run-runtime-tests.sh` directly with a filter to exercise the test path.

## Versioning

Aspire is pinned in `Uno.Themes.AppHost.csproj` (this repo uses CPM only under `src/library` and `src/samples`, not the AppHost). Bumping requires updating **both**:

```xml
<Sdk Name="Aspire.AppHost.Sdk" Version="13.2.4" />           <!-- MSBuild project SDK -->
<PackageReference Include="Aspire.Hosting.AppHost" Version="13.2.4" />
```

MSBuild SDKs are not governed by PackageReference versions, so they drift silently if you change only one. A direct `MessagePack` pin is also present to clear an NU1903 vuln that arrives transitively via the Aspire host's StreamJsonRpc dependency.

## Why the AppHost has its own `Directory.Build.props` / `.targets`

They deliberately do **not** import the repo root's. The root files are written for the packable libraries — they inject a versionless `DotNet.ReproducibleBuilds` reference (CPM-supplied under `src/library`, which the AppHost can't see → NU1604), turn on packaging in Release, remap obj/bin, and run a per-build network icon download. Shielding gives the AppHost a clean standard-SDK build.

## Troubleshooting

| Symptom | Cause / fix |
| --- | --- |
| Sample resource → `NETSDK1147 workloads must be installed` | Missing `-p:TargetFrameworkOverride`. The helpers add it; if hand-editing, include it (or `dotnet workload restore`). |
| `Endpoint with name 'http' already exists` | You added an endpoint that the launch profile already defines. Modify via `WithEndpoint(name, …)` instead of adding a duplicate. |
| WASM tile stuck `Starting` | Cold Uno bootstrap build (minutes) on first run; watch Logs. |
| `simple-runtime-tests` red instantly / no XML | Headless needs `xvfb-run` + `fluxbox`; the runner keys off `UNO_RUNTIME_TESTS_OUTPUT_PATH` (the script exports it). |
| `android-emulator` absent | Windows host (run from WSL) or `ANDROID_AUTO_START_EMULATOR=0`. |
| `NU1604 DotNet.ReproducibleBuilds` on the AppHost | The shield `Directory.Build.props`/`.targets` were removed or bypassed — restore them. |

## Related

- `src/AppHost/README.md` — human getting-started + full tunables tables.
- `uno-themes-runtime-tests` skill — the canonical runtime-test reference (filter syntax, adding tests, CI parity).
- `specs/05-aspire-apphost/` — spec + progress.
- `specs/lessons.md` — the `TargetFrameworkOverride` lesson.
