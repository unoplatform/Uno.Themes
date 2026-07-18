# Progress 05 — Aspire AppHost for sample launching & runtime-test orchestration

- **Spec:** [`spec.md`](./spec.md)
- **Status:** In progress — Phase 0 ✅, Phase 1 ✅ (build-validated), Phase 2 ✅ (fully verified), Phase 3 ✅ (code + syntax-verified; emulator run env-blocked here), Phase 4 ✅ (sln add + CI audit; full Release sln build → Phase 6). Phase 5 (docs) in progress, Phase 6 pending.
- **Branch:** `dev/sb/aspire`

Companion to [`spec.md`](./spec.md). Check items off as they land. Do not start Phase 1 until the spec's §11 open questions are resolved and this plan is approved.

---

## Phase 0 — De-risk (do first, before writing the graph)

The whole design rests on Aspire 13.2.4 restoring against SDK 10.0.300 and the repo's feeds. Prove that before building anything on top of it.

- [x] Create the minimal AppHost csproj: `net10.0`, `<Sdk Name="Aspire.AppHost.Sdk" Version="13.2.4" />`, `Aspire.Hosting.AppHost` 13.2.4, `IsAspireHost=true`, `UserSecretsId`.
- [x] **Isolation via `src/AppHost/Directory.Build.props` + `Directory.Build.targets` (both shield from root, do NOT import it).** Discovered the root injects a *versionless* `DotNet.ReproducibleBuilds` reference (CPM-supplied under `src/library`), which would break restore (NU1604) since nothing under `src/AppHost/` provides a `Directory.Packages.props`; the root `Directory.Build.targets` also runs a per-build network icon download. Shielding both gives a clean standard-SDK build. `IsPackable=false` / `GeneratePackageOnBuild=false` set in the props.
- [x] `Program.cs` = empty graph.
- [x] `dotnet restore` succeeds — **Aspire 13.2.4 resolves on SDK 10.0.300 from nuget.org.** Gate passed.
- [x] **MessagePack pin (unplanned).** Restore surfaced NU1903 high (MessagePack 2.5.192 via StreamJsonRpc); added direct pin `2.5.301`, re-restore clean. Spec's "not taken" list corrected.
- [x] `dotnet run` opens the dashboard (http profile, `:15334`, login-token URL); tears down cleanly on timeout SIGTERM (port released, no orphan). **No Docker needed** for the executable-only graph.
- [x] `launchSettings.json` (https + http, ports 17134/15334/OTLP/resource) + `appsettings.json` (log levels). http profile works without a dev cert (`ASPIRE_ALLOW_UNSECURED_TRANSPORT`).
- [x] **Release** build produces **0** `.nupkg`, 0 warnings — packaging shield verified.

Exit criteria: ✅ empty AppHost boots + tears down on this SDK; packaging provably off.

---

## Phase 1 — Desktop + WASM resources (the core value)

- [x] Implemented the six client resources in `Program.cs` (DRY local helpers), all `AddExecutable` + `WithExplicitStart()`.
- [x] Desktop resources: `DOTNET_MODIFIABLE_ASSEMBLIES=debug`; explicit `--launch-profile "<Sample> (Desktop)"`.
- [x] WASM resources: `WithHttpEndpoint(port, isProxied: false)` + `WithExternalHttpEndpoints()`; explicit `--launch-profile "<Sample> (WebAssembly)"`.
- [x] **`-p:TargetFrameworkOverride=desktop`/`browserwasm` on every resource (Phase 1 finding).** `-f` alone still pulled in all platform TFMs of the multi-targeted lib deps → `NETSDK1147` workload failure. Override collapses to one platform and overrides any dev `crosstargeting_override.props`. Lesson captured in `specs/lessons.md`.
- [x] Re-pinned Cupertino WASM to `:5001` (only `:5000` refs were the two launchSettings files — no `doc/` dependency).
- [x] `SimpleSamplesApp` profile-name spelling used exactly (verified against launchSettings).

Verification:
- [x] Graph shape proven via **Aspire manifest publish** — all six resources present with correct TFMs, launch-profile names, and WASM `http` bindings on 5000/5001/5002.
- [x] Boot shows WASM port-forwarding on 5000/5001/5002 with no collision; boot with nothing started is instant; no exceptions.
- [x] **Simple desktop head builds end-to-end** with the corrected command (`SimpleSampleApp.dll` produced; only pre-existing sample nullable warnings). Confirms project path + profile + TFM-override all correct. Assembly name is `SimpleSampleApp.dll` (not the stale `SimpleSamplesApp.dll` in the WSL2 profile) — noted for Phase 2.
- [ ] **Interactive (deferred to manual matrix):** actually clicking Start on each of the six tiles and seeing the window/WASM page render needs a display + multi-minute cold WASM builds; belongs in the human verification pass (Phase 6). Material/Cupertino heads not individually build-checked here (Simple desktop proves the shared mechanism).

Exit criteria: ✅ graph correct + one head build-validated. Interactive six-tile launch remains for Phase 6.

---

## Phase 2 — Runtime-test resource

- [x] Wrote `scripts/run-runtime-tests.sh` (build with `-p:TargetFrameworkOverride=desktop` → export env → `DISPLAY`-aware run → strict XML validation incl. failed/errored-case detection → summary line + nonzero exit).
- [x] Registered `simple-runtime-tests` (`AddExecutable` + `WithExplicitStart()`); forwards `CONFIG` / `UNO_RUNTIME_TESTS_RUN_TESTS` / `UNO_RUNTIME_TESTS_OUTPUT_PATH` via the `ForwardFromHostEnv` allowlist helper.
- [x] `src/AppHost/.gitattributes` → `*.sh text eol=lf`. Scripts `chmod +x`.

Verification:
- [x] **Filtered run** (`Given_SeedColorPalette`) headless under xvfb → **✓ 18/18 passed, exit 0**, XML with 18 `<test-case>`s.
- [x] **Failure detection** validated against a synthetic NUnit XML (1 pass / 1 fail) → validator prints `✗ 1/2`, lists the failed case, **exits 1**. Confirmed against the real engine's attribute format (`result="Passed"`/`"Failed"`; detector covers NUnit2 `success="False"` + NUnit3 `result` spellings).
- [x] Fixed a porting bug found by running it: the headless branch passed the app path into the xvfb subshell via `declare -f`, but the non-exported `$APP_DLL` expanded empty there → `dotnet` printed usage and no results. Rewrote to bake paths into the command string and `cd` into the bin dir (CI parity). Also confirmed the runner keys off `UNO_RUNTIME_TESTS_OUTPUT_PATH` (env), which the script exports.
- [ ] Parity spot-check vs `build/scripts/linux-skia-desktop-runtime-tests.sh` on a full (unfiltered) run → Phase 6.

Exit criteria: ✅ filtered run + failure detection verified; full-suite parity deferred to Phase 6.

---

## Phase 3 — Android dev loop (non-Windows, `simple-android` only per Q1)

- [x] Ported `scripts/start-emulator.sh`, `scripts/launch-android.sh`, `scripts/lib/android-sdk-bootstrap.sh` with the §4.5 deltas: stripped `UNO_STUDIO_API_KEY` / `STUDIOLIVE_BACKEND_BASE_URL` (launcher `am start` now carries no intent extras), AVD default `uno-themes`, Simple-sample defaults, generic (non-devcontainer-specific) messaging, guard var renamed `_UNO_THEMES_…`. Scripts stay fully parameterized for later heads.
- [x] `ForwardFromHostEnv` helper (shared with Phase 2) + `!OperatingSystem.IsWindows()` + `ANDROID_AUTO_START_EMULATOR` gating.
- [x] Registered `android-emulator` (ExplicitStart) + `simple-android` (ExplicitStart, `ANDROID_PROJECT`/`ANDROID_PACKAGE`/`ANDROID_CONFIG` set); wired `WaitFor` + the `BeforeResourceStartedEvent` start-kick. Compiles against 13.2.4 (`ResourceCommandService.ExecuteCommandAsync` + `KnownResourceCommands.StartCommand`).
- [x] App id `uno.platform.themes.simple` confirmed in `SimpleSampleApp.csproj`.

Verification:
- [x] AppHost builds clean with the Android wiring; all three Android-related resources appear in the manifest (`android-emulator`, `simple-android`).
- [x] All three ported scripts pass `bash -n` (syntax).
- [ ] **Env-blocked here (no KVM / Android SDK):** actually booting the emulator, APK install/launch/logcat, `adb emu kill` teardown, `ANDROID_AUTO_START_EMULATOR=0` fallback, and the deferred-head override. Must be run on a Linux/WSL host with KVM in Phase 6 and recorded in the PR (do not claim success otherwise).

Exit criteria: ✅ code complete + compiles + scripts syntax-valid. Emulator end-to-end is an explicit Phase 6 / reviewer environment task.

---

## Phase 4 — Solution & CI integration

- [x] Added `Uno.Themes.AppHost` to `Uno.Themes.sln` under a new `apphost` solution folder (`dotnet sln add --solution-folder apphost`).
- [x] Confirmed **absent** from `Uno.Themes-packages.slnf` (that filter is an explicit include-list).
- [x] **CI audit done:** no `build/stage-*.yml` builds `Uno.Themes.sln` wholesale — each builds a specific sample `.csproj` with `TargetFrameworkOverride`; the only solution build (`stage-build-packages.yml`) uses `Uno.Themes-packages.slnf` (excludes the AppHost). So the AppHost is not picked up by any CI stage. No standalone fallback needed.
- [ ] Full `dotnet build Uno.Themes.sln -c Release -p:TargetFrameworkOverride=desktop` stays clean → Phase 6 (heavy build; standalone AppHost Release already verified 0-warning / 0-package in Phase 0).

Exit criteria: ✅ sln membership + slnf exclusion + CI audit done. Full-sln Release build → Phase 6.

---

## Phase 5 — Documentation

- [x] `src/AppHost/README.md` — prerequisites (no Docker), run, per-resource table, runtime-test env table, Android tunables, `TargetFrameworkOverride`/`crosstargeting_override` note, troubleshooting.
- [x] `.claude/skills/uno-themes-apphost/SKILL.md` — agent-facing (resource graph, the 7 non-obvious rules, adding resources, non-launch verification via manifest, the two-place Aspire pin, why the shield build files, troubleshooting).
- [x] `.claude/skills/uno-themes-runtime-tests/SKILL.md` — added a `simple-runtime-tests` cross-link section (CI-parity content stays authoritative).
- [x] `AGENTS.md` `<repository_orientation>` — solution-layout bullet for `src/AppHost/` (dev-only, not packable, shields from root build files).
- [x] No `doc/` (published) changes.

Exit criteria: ✅ README + skills + orientation in place.

---

## Phase 6 — Final verification & PR

- [ ] Re-run the full manual matrix (Phases 1–3 verification) on a clean checkout.
- [ ] Confirm existing local flows still work untouched: direct `dotnet run` of each sample; `build/scripts/linux-skia-desktop-runtime-tests.sh`; a `crosstargeting_override.props` single-TFM build.
- [ ] Conventional-commits-compliant history (`feat(apphost): …`, `docs(apphost): …`, `chore(apphost): …`).
- [ ] PR description: scope, the §8 testing exception (why no runtime tests), the manual verification matrix results (incl. which Android checks ran vs were env-blocked), and the required issue link.
- [ ] Fill in the Review section below.

---

## Decisions locked before implementation (spec §11)

Resolved 2026-07-18:

- [x] Q1 — Android v1 scope. → **decision: `simple-android` only** (+ `android-emulator`). Material/Cupertino Android tiles deferred; drive them by overriding `ANDROID_PROJECT`/`ANDROID_PACKAGE` on the `simple-android` resource if needed. Scripts stay fully parameterized so adding the other two later is a pure registration change.
- [x] Q2 — Solution membership. → **decision: add to `Uno.Themes.sln`** under a new `apphost` folder; standalone is the fallback only if the Phase 4 CI audit surfaces a regression.
- [x] Q3 — Resource naming. → **decision: by-sample** (`material-desktop`, `material-wasm`, …).
- [x] Q4 — `simple-runtime-tests` default config. → **decision: Debug** (overridable via `CONFIG`).

---

## Lessons captured

_(Append anything that bites during implementation; promote repo-wide rules to `AGENTS.md` / the relevant skill per AGENTS.md §3, not just here.)_

---

## Review

**Shipped (Phases 0–5 code-complete):**
- `src/AppHost/Uno.Themes.AppHost/` — Aspire 13.2.4 AppHost: 6 sample heads (desktop+WASM), `simple-runtime-tests`, `android-emulator` + `simple-android`. All `AddExecutable` + `WithExplicitStart()`. Plus `Directory.Build.props`/`.targets` shields, `appsettings.json`, `launchSettings.json`, `.gitattributes`, and 4 scripts under `scripts/`.
- One sample edit: Cupertino WASM port 5000 → 5001 (`CupertinoSampleApp/Properties/launchSettings.json`).
- Solution: added under an `apphost` folder in `Uno.Themes.sln`; excluded from the packages slnf.
- Docs: `src/AppHost/README.md`, `.claude/skills/uno-themes-apphost/SKILL.md`, runtime-tests skill cross-link, AGENTS.md orientation bullet. `specs/lessons.md` gained the `TargetFrameworkOverride` lesson.

**Deviations / discoveries vs the original plan:**
1. **MessagePack pin needed after all** (spec §7 originally said "not taken") — NU1903 high arrives via the Aspire host's StreamJsonRpc. Pinned 2.5.301.
2. **`-p:TargetFrameworkOverride=<platform>` required** on every sample resource — `-f` alone demands all platform workloads (NETSDK1147). Bonus: it overrides any dev `crosstargeting_override.props`, superseding the §6 caveat.
3. **AppHost shielded from root build files** — root injects a versionless `DotNet.ReproducibleBuilds` ref (breaks restore under `src/AppHost/`) + a per-build icon download; added minimal non-importing `Directory.Build.props`/`.targets`.
4. **run-runtime-tests.sh headless bug** found and fixed by running it (non-exported `$APP_DLL` lost across the xvfb subshell).

**Verification evidence:**
- Phase 0: restore/build/boot/teardown clean; Release → 0 warnings, 0 nupkg.
- Phase 1: manifest confirms all 6 heads (correct TFMs/profiles/ports); Simple desktop **builds end-to-end**.
- Phase 2: `simple-runtime-tests` → **✓ 18/18 passed, exit 0** headless; failure detection **exits 1** on a synthetic failure.
- Phase 3: AppHost builds with Android wiring; manifest lists `android-emulator` + `simple-android`; all scripts pass `bash -n`.
- Phase 4: CI audit — no wholesale sln build; sln add + slnf exclusion confirmed.

**Remaining (Phase 6 — needs the reviewer's environment / explicit go):**
- Interactive: click Start on each of the 6 tiles and see the window / WASM page render.
- Android end-to-end on a KVM host (boot, install, launch, logcat, `adb emu kill` teardown, `ANDROID_AUTO_START_EMULATOR=0` fallback).
- Full `dotnet build Uno.Themes.sln -c Release -p:TargetFrameworkOverride=desktop` clean.
- Full (unfiltered) runtime-test parity vs the CI script.
- Commit + PR (not done — awaiting user request per repo source-control policy).

**No runtime tests added** — per spec §8 (AGENTS.md Exceptions): the AppHost is unpackaged dev tooling with no library behavior/public API/resource keys; an all-ExplicitStart graph has nothing bootable to assert cheaply. Runtime-test coverage of the libraries is unchanged and still enforced by CI.
