# Uno.Themes AppHost (Aspire)

A .NET Aspire orchestrator for local development of the Uno.Themes sample apps and runtime tests. One command opens a dashboard where every sample target and the runtime-test runner is a **stopped** tile — click Start to build/launch it, watch logs stream, Stop/Restart, or override env vars per run.

Unlike a typical Aspire app, this one has **no backend and needs no Docker** — every resource is a plain `dotnet run` / `bash` executable.

```sh
dotnet run --project src/AppHost/Uno.Themes.AppHost
```

Watch the console for the dashboard URL (e.g. `https://localhost:17134` — note the login token in the URL). `Ctrl+C` tears everything down.

## Resources

All resources are registered **stopped** (`WithExplicitStart()`), so AppHost boot is instant and nothing builds until you click Start.

| Resource | What Start does | Notes |
| --- | --- | --- |
| `material-desktop` / `cupertino-desktop` / `simple-desktop` | `dotnet run -f net10.0-desktop` the sample (Skia) | Hot reload enabled (`DOTNET_MODIFIABLE_ASSEMBLIES=debug`) |
| `material-wasm` / `cupertino-wasm` / `simple-wasm` | `dotnet run -f net10.0-browserwasm` the sample | Dashboard shows the real dev-server URL: **5000 / 5001 / 5002**. First build is slow (cold Uno bootstrap); later starts are seconds |
| `simple-runtime-tests` | Builds + runs the SimpleSampleApp runtime-test suite headlessly; tile turns red on failure | Bash-only (non-Windows). Filter/config via env — see below |
| `android-emulator` | Boots the `uno-themes` AVD; shuts it down (`adb emu kill`) on AppHost stop | Non-Windows only; skipped if `ANDROID_AUTO_START_EMULATOR=0` |
| `simple-android` | Builds/installs/launches the Simple sample on Android + streams logcat | Non-Windows only. `WaitFor`s the emulator and pulls it up transparently on Start |

Every resource passes `-p:TargetFrameworkOverride=<platform>` so the sample and its multi-targeted library dependencies collapse to a single platform — otherwise `dotnet run` would demand every platform's workload (android/ios/wasm). This also means the AppHost is **immune to any `crosstargeting_override.props`** you have set locally (a command-line global property wins), so you don't need to clear that file first.

## Prerequisites

- **.NET SDK 10.0.x** (already required by the sample heads). **No Docker.**
- Dashboard over HTTPS wants a trusted dev cert: `dotnet dev-certs https --trust` (one-time). The `http` launch profile works without it.
- **Android resources only:** `adb` on PATH, `dotnet workload install android`, and KVM for emulator acceleration. Everything else (cmdline-tools, system image, AVD) is bootstrapped lazily by the scripts on first Start.
- **`simple-runtime-tests` headless only:** `xvfb-run` + `fluxbox` (already CI-standard). Not needed when a `DISPLAY` is available — the script runs the window directly then.

## Runtime tests (`simple-runtime-tests`)

Builds `SimpleSampleApp` (net10.0-desktop) and runs the `Uno.UI.RuntimeTests.Engine` suite, writing NUnit XML and exiting nonzero if anything fails (so the tile goes red). Controlled by env vars — set them in the shell **before** launching the AppHost (they're forwarded via an allowlist), or override them on the resource in the dashboard's Details panel:

| Variable | Default | Purpose |
| --- | --- | --- |
| `CONFIG` | `Debug` | Build configuration. Set `Release` for exact CI parity. |
| `UNO_RUNTIME_TESTS_RUN_TESTS` | `{}` (all) | Engine filter JSON, e.g. `{"Filter":{"Value":"Given_SeedColorPalette"},"Attempts":1}`. |
| `UNO_RUNTIME_TESTS_OUTPUT_PATH` | temp file | NUnit results path. |

This is a convenience wrapper around the same mechanism as `build/scripts/linux-skia-desktop-runtime-tests.sh` (which remains the CI entry point). For the full runtime-test reference, see the `uno-themes-runtime-tests` skill.

## Android dev loop

Two resources so the emulator lifecycle is tied to the AppHost process (it dies on `Ctrl+C`) but stays out of the way until needed:

- **`android-emulator`** — provisions + boots the `uno-themes` AVD, and shuts it down cleanly on AppHost stop. Reuses an already-running emulator if one is attached.
- **`simple-android`** — `WaitFor`s the emulator (pulling it up transparently on Start via a start-kick), then `dotnet build -t:InstallAndroidDependencies` → `-t:Install` → `am start` → `adb logcat --pid`.

Only the Simple head ships today; the launcher is parameterized (`ANDROID_PROJECT` / `ANDROID_PACKAGE`), so you can point `simple-android` at the Material or Cupertino sample (`uno.platform.themes.material` / `.cupertino`) via a dashboard env override, and adding dedicated tiles later is a registration-only change.

### Android tunables (env vars, forwarded from the launching shell)

| Variable | Default | What it does |
| --- | --- | --- |
| `ANDROID_AUTO_START_EMULATOR` | `1` | Set `0` to skip the emulator resource entirely and use your own device (USB / `adb connect` / Android Studio AVD). |
| `ANDROID_AVD_NAME` | `uno-themes` | AVD created/booted. |
| `ANDROID_SDK_API_LEVEL` | `34` | Emulator API level (drives the system image + AVD). |
| `ANDROID_SDK_IMAGE` | `system-images;android-34;google_apis;x86_64` | Full sdkmanager image triple. |
| `ANDROID_EMULATOR_WINDOW` | `1` | `0` for headless (CI / pure adb). Windowed needs `DISPLAY`. |
| `ANDROID_EMULATOR_GPU` | `swiftshader_indirect` | `-gpu` mode; `host` for hardware acceleration. |
| `ANDROID_SERIAL` | *(unset)* | adb serial; required when multiple devices are attached. |
| `DEVICE_WAIT_TIMEOUT_S` | `300` | Seconds to wait for a usable device / boot. |
| `ANDROID_HOME` | `/opt/android-sdk` | SDK directory (must be writable). |

## Project layout

| Path | Purpose |
| --- | --- |
| `Uno.Themes.AppHost/Program.cs` | The resource graph. |
| `Uno.Themes.AppHost/scripts/` | `start-emulator.sh`, `launch-android.sh`, `run-runtime-tests.sh`, `lib/android-sdk-bootstrap.sh`. |
| `Directory.Build.props` / `.targets` | Shield the AppHost from the repo-root build files (it's a standalone dev exe, not a packable library — see the comments in those files). |

Aspire versions are pinned directly in `Uno.Themes.AppHost.csproj` (this repo uses Central Package Management only under `src/library` and `src/samples`). When bumping Aspire, update **both** the `<Sdk Name="Aspire.AppHost.Sdk" …/>` element and the `Aspire.Hosting.AppHost` PackageReference.

## Troubleshooting

- **A sample resource fails with `NETSDK1147: workloads must be installed`** — you're launching a sample without the platform override. The AppHost passes `-p:TargetFrameworkOverride=<platform>` for you; if you see this, you're likely running the raw `dotnet run` by hand. Add the override, or install the workload (`dotnet workload restore`).
- **WASM tile stays in `Starting` for minutes** — first-run cold WASM build (Uno bootstrap). Watch the resource's Logs pane; later starts are seconds.
- **`simple-runtime-tests` produces no results / tile red immediately** — ensure `xvfb-run` + `fluxbox` are installed when headless. With a `DISPLAY` set, the app runs windowed instead.
- **`android-emulator` missing from the dashboard** — you're on Windows (bash-only launcher; run the AppHost from WSL) or you set `ANDROID_AUTO_START_EMULATOR=0`.
- **`'adb' / 'dotnet' not found`** (Android resources) — install Android platform-tools and the .NET Android workload; see Prerequisites.
- **Emulator boot hangs at `sys.boot_completed`** — first cold boot of a fresh API 34 image under software rendering is 30–90 s. If it exceeds the deadline, check `/dev/kvm` is exposed (`ls -l /dev/kvm`).
- **Port already in use (5000–5002)** — a sample dev server is already running outside the AppHost. The AppHost reuses the sample's own ports (`isProxied:false`), so you can't run the same WASM head both ways at once.

## Related

- Spec: [`specs/05-aspire-apphost/spec.md`](../../specs/05-aspire-apphost/spec.md) · progress: [`specs/05-aspire-apphost/progress.md`](../../specs/05-aspire-apphost/progress.md).
- Skills: `uno-themes-apphost` (agent-facing AppHost reference), `uno-themes-runtime-tests` (full runtime-test reference).
