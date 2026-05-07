# AI Agents Contribution & Coding Instructions

This document defines strict guardrails for any AI-assisted or automated agent contributions (including Copilot, custom prompt runners, or scripted refactors) working in the **Uno.Toolkit.UI** repository. Human contributors must also ensure generated changes comply before merge. It is the single source of truth for repo-wide orientation *and* the rules agents must follow; `CLAUDE.md` at the repo root is a thin pointer that includes this file.

<repository_orientation>

## Repository overview

Uno Toolkit ships higher-level UI controls for multi-platform Uno Platform / WinUI apps. Six NuGet packages are produced from this repo:

- `Uno.Toolkit.WinUI` — base controls (WinUI lineage). Built from `src/Uno.Toolkit.UI/Uno.Toolkit.WinUI.csproj`.
- `Uno.Toolkit.WinUI.Material` / `.Cupertino` — design-system style libraries on top of the base, under `src/library/`.
- `Uno.Toolkit.WinUI.Markup` / `.Material.Markup` — C# markup helpers, under `src/library/`.
- `Uno.Toolkit.WinUI.Simple` — minimal style set used by the `Simple` sample.

`src/Uno.Toolkit/Uno.Toolkit.csproj` is a tiny netstandard core (e.g. `ILoadable`) that all other projects reference. Despite the legacy `Uno.Toolkit.UI` folder name, the assembly is `Uno.Toolkit.WinUI` (see `AssemblyName` in the csproj).

## Solution layout

- `src/Uno.Toolkit.sln` — main solution. Includes libraries, samples, and runtime tests. Open this for full development.
- `samples/Uno.Toolkit.Samples.sln` — samples-only solution; references the libraries via project reference.
- `src/Uno.Toolkit.UI/` — all controls (`Controls/`), behaviors/extensions (`Behaviors/`), markup extensions (`Markup/`), helpers (`Helpers/`), themes (`Themes/`). XAML in `Controls/**/*.xaml` and `Behaviors/**/*.xaml` is merged into `Generated/mergedpages.xaml` at build time by `Uno.XamlMerge.Task` (config in `src/Uno.Toolkit.UI/xamlmerge-toolkit.props`). Don't hand-edit `Generated/`.
- `src/Uno.Toolkit.RuntimeTests/` — MSTest-based runtime tests; depends on `Uno.UI.RuntimeTests.Engine` and is hosted inside the sample apps.
- `samples/Uno.Toolkit.Samples/` — shared project (`.shproj`) with the sample UI; the platform heads in `samples/Uno.Toolkit.Samples.{Material,Cupertino,Simple}/` are the runnable apps.
- `ref/Uno.Themes` — sibling `Uno.Themes` repo content checked into this tree (not a git submodule despite appearances). Treat as vendored source unless told otherwise.

## Target frameworks and platform builds

Target frameworks are managed centrally:
- `src/tfms.props` defines `NetCurrent` (currently `net10.0`).
- `src/tfm-common-winui.props` expands library projects to `net9.0` + per-platform suffixes (`net9.0-ios`, `net9.0-android`, `net9.0-windows10.0.19041`, `net9.0-maccatalyst`); sample apps use `net10.0-*`.
- The Uno SDK version is pinned in `global.json` (`Uno.Sdk` and `Uno.Sdk.Private`).

The top-level `Directory.Build.props` also exposes `Build_Android`, `Build_iOS`, `Build_MacOS`, `Build_Windows` switches; non-Windows hosts default `Build_Windows=false`. The single-platform local-build flow (via `crosstargeting_override.props`) is documented in §4 below.

</repository_orientation>

<flow_orchestration>

### 1. Plan Node Default
- Enter plan mode for ANY non-trivial task (3+ steps or architectural decisions)
- If something goes sideways, STOP and re-plan immediately - don't keep pushing
- Use plan mode for verification steps, not just building
- Write detailed specs upfront to reduce ambiguity

### 2. Subagent Strategy
- Use subagents liberally to keep main context window clean
- Offload research, exploration, and parallel analysis to subagents
- For complex problems, throw more compute at it via subagents
- One tack per subagent for focused execution

### 3. Self-Improvement Loop
- After ANY correction from the user: update `specs/lessons.md` with the pattern (create the file/folder if it does not yet exist)
- Write rules for yourself that prevent the same mistake
- Ruthlessly iterate on these lessons until mistake rate drops
- Review lessons at session start

#### Where corrections are recorded

User corrections, "do this / never do that" rules, workflow guardrails, and tool-usage policies that should bind **every** agent working on this repo MUST be written to a checked-in, shared file:
- Repo-wide rules → `AGENTS.md` (this file).
- Skill-specific rules (e.g. how to use a particular tool/MCP) → the relevant `.claude/skills/<skill>/SKILL.md`.
- Domain lessons / postmortems → `specs/lessons.md`.

🚫 **Never** record cross-agent corrections in personal/auto memory (e.g. `~/.claude/projects/<project>/memory/`, `feedback_*.md`, individual user preference files). Personal memory is per-user and not shared via git, so other agents and contributors will not see it and the mistake will repeat. If a correction is general enough that any future agent should follow it, it belongs in a checked-in file. Reserve personal memory for things that are genuinely individual to one user (their role, their preferences) — not project rules.

When in doubt: if removing the rule would let any other agent on this repo repeat the same mistake, the rule is shared and must be checked in.

### 4. Verification Before Done
- Never mark a task complete without proving it works
- Diff behavior between `main` and your changes when relevant
- Ask yourself: "Would a staff engineer approve this?"
- Run tests, check logs, demonstrate correctness
- You MUST assume that for a given branch, the `main` branch is correct and failures are specific to the current branch. You MUST assume that changes in the current branch are the cause of any new failures.

### 5. Demand Elegance (Balanced)
- For non-trivial changes: pause and ask "is there a more elegant way?"
- If a fix feels hacky: "Knowing everything I know now, implement the elegant solution"
- Skip this for simple, obvious fixes - don't over-engineer
- Challenge your own work before presenting it

### 6. Autonomous Bug Fixing
- When given a bug report: just fix it. Don't ask for hand-holding
- Point at logs, errors, failing tests - then resolve them
- Zero context switching required from the user
- Go fix failing CI tests without being told how

## Task Management

1. **Plan First**: Write plan to `specs/<topic>/progress.md` with checkable items
2. **Verify Plan**: Check in before starting implementation
3. **Track Progress**: Mark items complete as you go
4. **Explain Changes**: High-level summary at each step
5. **Document Results**: Add review section to `specs/<topic>/progress.md`
6. **Capture Lessons**: Update `specs/lessons.md` after corrections

## Core Principles

- **Simplicity First**: Make every change as simple as possible. Impact minimal code.
- **No Laziness**: Find root causes. No temporary fixes. Senior developer standards.
- **Minimal Impact**: Changes should only touch what's necessary. Avoid introducing bugs.

</flow_orchestration>


<coding_directives>

## 1. Core Engineering Principles

✅ Apply all SOLID principles (SRP, OCP, LSP, ISP, DIP).  
✅ Keep code simple, intention‑revealing; clarity > cleverness.  
✅ Minimize use of the null-forgiving operator (`!`); prefer explicit guards or refactors that satisfy the nullable flow (the repo enables `Nullable=enable` globally — see `src/Directory.Build.props`).  
✅ Separate concerns: control logic | visual state | resources/themes | extension/helpers | tests.  
✅ Favor composition over inheritance; depend on abstractions where the API surface justifies it.  
✅ Optimize only with evidence (profiling/metrics).

---

## 2. Performance & Allocations

✅ Minimize allocations in hot paths (measure conversions, layout, hit-testing, theme lookups). Prefer `StringBuilder` for string assembly and avoid LINQ in tight loops where measurement shows it costs.
✅ Use `readonly` on fields and structs where possible.
✅ Avoid boxing (watch generics, interpolated logging with value types).
✅ Reuse expensive objects (brushes, geometries, parsed resources) where the lifecycle allows.
✅ Only introduce `Span<T>` / `Memory<T>` when profiling shows benefit.
✅ When using `System.Text.Json`, prefer source-generated serializers (`[JsonSerializable]`) over reflection-based ones — required for AOT and beneficial for size on WASM.
✅ **Always use typed deserialization** — never parse JSON with `JsonDocument` / `JsonElement` / manual `GetProperty` chains. Define a typed model (only the fields you need) and deserialize into it.

### Memory & WASM Considerations

This repository targets WASM as a first-class platform (sample apps and runtime tests run there). On WASM, `WebAssembly.Memory.grow()` is **irreversible** — every peak allocation permanently inflates `HEAPU8.length`.

✅ **Be allocation-aware in controls and behaviors.** Layout, hit-testing, and template instantiation can run on every interaction; an extra `ToList()` or repeated string concatenation in a frequently-invoked code path becomes a measurable footprint on WASM.
✅ **Release before allocate** when replacing large object graphs (e.g. swapping out a heavy template, disposing a child visual tree). Don't hold the previous instance alongside the replacement longer than necessary.
✅ **Watch for leaks via static event subscriptions** on framework elements. The `LeakTest` runtime test exists for a reason — keep weak references / unsubscription discipline tight, especially in attached behaviors.

---

## 3. Framework & Platform Usage

✅ Honor `CancellationToken` on any async public API and I/O boundary.
✅ NEVER use `.Result` / `.GetAwaiter().GetResult()` outside a controlled, documented sync bridging point.
✅ ALWAYS prefer non-blocking async flow to remain WASM-safe; if a sync bridge is unavoidable, document why and keep it local.
✅ Prefer the WinUI/Uno-provided primitives (`DependencyProperty`, `VisualStateManager`, `ResourceDictionary` merging, `ItemsRepeater`, `FrameworkElement.Loaded/Unloaded`) over hand-rolled equivalents.
✅ Multi-platform aware: code that compiles for net10.0-windows, net9.0-android, net9.0-ios, net9.0-maccatalyst, and browser-wasm. When platform behavior diverges, isolate it behind partial classes / conditional compilation symbols (`__WASM__`, `__ANDROID__`, `__IOS__`, etc.) — don't sprinkle `#if` blocks across method bodies if a platform-specific partial would do the job.

---

## Code Style Guidelines

Formatting and style rules are defined in the repo configuration files below. Treat these files as the source of truth and avoid duplicating their contents here.

✅ EditorConfig formatting and naming rules: [src/.editorconfig](src/.editorconfig)
✅ XAML formatting rules: [Settings.XamlStyler](Settings.XamlStyler) (and `src/Settings.XamlStyler` where present)
✅ Conventional Commits enforcement: [.github/workflows/conventional-commits.yml](.github/workflows/conventional-commits.yml)
✅ Tabs for `.cs` / `.xaml` / `.xml` / `.targets` / `.props`; 2-space tabs for `.csproj`; spaces for `.yml` / `.md`; CRLF line endings.
✅ `src/Directory.Build.props` sets `TreatWarningsAsErrors=true`, `Nullable=enable`, `LangVersion=latest`, and `AnalysisModePerformance=AllEnabledByDefault` for non-test projects. New code must compile clean under these.

## 4. Build & Validation

Primary solutions:
- `src/Uno.Toolkit.sln` — full development solution (libraries, samples, runtime tests).
- `samples/Uno.Toolkit.Samples.sln` — samples-only.

Release build enforces warnings as errors via `src/Directory.Build.props` (`TreatWarningsAsErrors=true`, `Nullable=enable`, `LangVersion=latest`).

### Single-platform development

Copy `src/crosstargeting_override.props.sample` → `src/crosstargeting_override.props` and uncomment the desired `TargetFrameworkOverride` (`windows`, `ios`, `android`, `browserwasm`, `desktop`, `maccatalyst`, `macos`). **Always close the IDE before changing this file** — switching while the solution is open corrupts the NuGet restore cache.

CLI equivalent:

```bash
# Restore (fast, single-platform)
dotnet restore src/Uno.Toolkit.sln -p:TargetFrameworkOverride=desktop -p:DisableMobileTargets=true

# Debug build (warnings allowed temporarily)
dotnet build src/Uno.Toolkit.sln -c Debug -p:TargetFrameworkOverride=desktop

# Release build MUST be 100% clean
dotnet build src/Uno.Toolkit.sln -c Release -p:TargetFrameworkOverride=desktop "/clp:WarningsOnly;Summary"
```

### Sample-app builds (target framework chosen explicitly)

```bash
# Material Desktop / Skia
dotnet build samples/Uno.Toolkit.Samples.Material/MaterialSampleApp.csproj -c Debug -f net10.0-desktop

# Cupertino WASM
dotnet build samples/Uno.Toolkit.Samples.Cupertino/CupertinoSampleApp.csproj -c Debug -f net10.0-browserwasm

# Run the desktop sample app interactively
dotnet run --project samples/Uno.Toolkit.Samples.Material/MaterialSampleApp.csproj -f net10.0-desktop
```

VS Code task labels in `.vscode/tasks.json` (`build-desktop-Material`, `build-wasm-Cupertino`, etc.) wrap these commands.

✅ Zero warnings in Release is mandatory.
✅ Suppress a warning only with justification in PR + targeted scope (`#pragma` with comment).
✅ Do not disable deterministic builds.
✅ Do not expand global `<NoWarn>` in `src/Directory.Build.props` without prior approval.

## 5. Testing Requirements

The runtime test project `src/Uno.Toolkit.RuntimeTests/` is **MSTest hosted inside the sample apps** via `Uno.UI.RuntimeTests.Engine`. There is **no `dotnet test`** entry point.

- Interactive: launch a sample app with `--mode=rt` (or set `UNO_RUNTIME_TESTS_RUN_TESTS`) — see `samples/Uno.Toolkit.Samples/RuntimeTesting/RuntimeTestModeDetector.cs`. The app shows a runtime-test runner UI.
- Headless (used in CI): launch the built sample DLL with `--runtime-tests=<results.xml>`. The script `build/workflow/scripts/linux-skia-runtime-tests.sh` does this under `xvfb-run` for desktop.
- Filter tests by name with `UNO_RUNTIME_TESTS_RUN_TESTS='{"Filter":{"Value":"<substring>"}}'` (the hot-reload CI job uses `_HotReload`).

### Hot-reload tests

Files in `src/Uno.Toolkit.RuntimeTests/Tests/HotReload/` use `[RunsInSecondaryApp]` and `HotReloadHelper.UpdateSourceFile(...)`. Constraints:

- **Debug only.** The whole HotReload folder is wrapped in `#if DEBUG`; the `Uno.WinUI.DevServer` package reference is conditioned to Debug. Building the runtime-test project in Release intentionally drops these tests.
- Hot-reload paths in `UpdateSourceFile(...)` are repo-relative (e.g. `"../../src/Uno.Toolkit.RuntimeTests/Tests/HotReload/HotReloadTarget.cs"`). Don't break that relative layout.
- `Given_HotReload.cs` raises the default workspace timeout to 180s — Roslyn workspace load is slow on CI; treat HR test flakes as timing-sensitive.

### General testing rules

✅ Every new public behavior must include tests (runtime test in `src/Uno.Toolkit.RuntimeTests/Tests/`, with reusable test pages in `Tests/TestPages/`).
✅ Namespace parity: implementation namespaces mirrored in test files where reasonable.
✅ AAA pattern (Arrange / Act / Assert).
✅ Lack of coverage for new logic blocks merge.
✅ **Every bug fix MUST follow red/fix/green**: first add a runtime test that reproduces the bug and fails, then apply the fix, then confirm it passes. The failing test must be committed alongside the fix so the regression is permanently guarded.
🚫 **Never use `Assert.Inconclusive`** (or equivalent). A test either asserts behavior and passes, or fails. Skipping via inconclusive hides regressions. If a scenario cannot run on a platform, gate it with an explicit platform attribute / `#if` instead.
🚫 **Do not deactivate, skip, `[Ignore]`, or delete an existing test** unless the underlying feature is being entirely removed from the product — not merely because a file was refactored, renamed, or a class was restructured. Refactors must keep the behavioral coverage intact; if a test no longer compiles after a rename, update it, don't remove it.

### Minimum Test Additions Per PR

| Change Type | Required Tests |
|-------------|----------------|
| New control / behavior | Happy path + 1 failure/edge case (runtime test, ideally with a `TestPages/` companion) |
| New attached property / extension | Set/clear scenario + a teardown-leak guard if it subscribes to events |
| Bug fix | Repro test + non‑regression guard |
| Hot-reload-sensitive change | Add or update a `Tests/HotReload/*HrTest.cs` case |

### String equivalence assertions (multiline)

- Use raw strings (`"""..."""`) for expected and actual samples.
- Avoid manual newline normalization (`Replace("\r\n", "\n")`); rely on the test framework's options where available.

### Format XAML

```bash
# Run xaml styler (uses Settings.XamlStyler at the repo root)
dotnet dnx XamlStyler.Console -r -l Debug -c Settings.XamlStyler -d "."
```

✅ Always run runtime tests as part of verification of UI/control changes — they are not optional manual steps.
✅ Maintain or improve passing test count.
✅ Never delete tests without equivalent protection.

---

## 6. API Conventions

The deliverable here is a public NuGet API consumed by external Uno apps. Stability matters more than in an app-only codebase.

✅ Public types and members must have XML doc comments suitable for IntelliSense.
✅ `DependencyProperty` registrations: name the property field `<Name>Property`, expose a CLR wrapper, and follow the `Get/Set<Name>(DependencyObject)` convention for attached properties. Centralize default values, do not duplicate them.
✅ Prefer additive change. A breaking change to a public API requires explicit justification in the PR description and is likely to be rejected (see PR template).
✅ Persisted enums: explicit underlying type; mark `[Flags]` only when actually flag-shaped.

---

## 7. Logging & Diagnostics

✅ Structured logging where applicable (`logger.LogInformation("Processed {Id}", id)`).
✅ No PII / secrets / device identifiers in logs.
✅ Correct level semantics (Trace/Debug/Info/Warning/Error/Critical).
✅ If values are computed only for logging (for example `ToList()`, `string.Join(...)`, projections, or expensive formatting), wrap that computation in `if (logger.IsEnabled(LogLevel.X))` for the corresponding level so disabled logs do not pay the allocation/computation cost.

---

## 8. Error Handling

✅ Never swallow exceptions silently — wrap with context or let propagate.
✅ Order `catch` blocks from most specific to most general: catch predicted exceptions first (`OperationCanceledException`, `InvalidOperationException`, etc.), then use a generic `catch (Exception ex)` as the final fallback. Never use a bare `catch` or generic-only handler when specific exceptions are foreseeable.
✅ For controls, prefer graceful degradation (e.g. fall back to a default visual state) over throwing from layout / template-application paths — exceptions in those paths can take down the visual tree on consumers.

---

## 9. Constants & Magic Strings

✅ Centralize non-trivial strings (resource keys, theme keys, visual state names) and numeric literals.
✅ Comment rationale for timeouts, animation durations, threshold values.
✅ Avoid scattering duplicate values across XAML and code-behind.

---

## 10. Async & Concurrency

✅ All I/O-bound operations async.
✅ Honor `CancellationToken` quickly.
✅ Avoid shared mutable state; where needed protect with locks/concurrent collections.
✅ Use `ConfigureAwait(false)` only in library layers that genuinely don't need to resume on the UI thread.
🚫 **NEVER use `async void`** except for event handlers required by the framework (e.g. XAML event handlers, `OnLaunched`).
✅ Every `async void` method **MUST** wrap its entire body in `try/catch` — unhandled exceptions in `async void` crash the runtime (especially critical on WASM where the runtime runs in a web worker).
✅ Prefer returning `Task`/`ValueTask` from async methods so callers can observe exceptions.
✅ Fire-and-forget patterns (`_ = SomeAsync()`) **MUST** have a `try/catch` inside the called method.

---

## 11. UI / XAML

✅ Minimize code-behind; prefer attached properties, behaviors, or template parts for reusable logic.
✅ Use bindings for state propagation; avoid hand-wired property change handlers when a binding suffices.
✅ Avoid manual dispatcher usage unless necessary.
✅ WinUI supports implicit `bool` → `Visibility` bindings; do not add redundant BoolToVisibility converters.
✅ XAML in `Controls/**/*.xaml` and `Behaviors/**/*.xaml` is merged into `Generated/mergedpages.xaml` at build by `Uno.XamlMerge.Task` (config in `src/Uno.Toolkit.UI/xamlmerge-toolkit.props`). Material/Cupertino styles merge via `xamlmerge-{material,cupertino}.props`. **Do not hand-edit anything under `Generated/`.**
✅ When adding a control under `src/Uno.Toolkit.UI/Controls/<Name>/`: companion `.xaml` files are picked up automatically by the `XamlMergeInput` glob — do not add an explicit `<Page>` reference.

### Adding a new control — checklist

1. Code-behind goes under `src/Uno.Toolkit.UI/Controls/<Name>/`; companion `.xaml` files are picked up automatically by the `XamlMergeInput` glob and merged into `Generated/mergedpages.xaml` — do not add an explicit `<Page>` reference.
2. Material/Cupertino styles go under `src/library/Uno.Toolkit.{Material,Cupertino}/Styles/` and merge into their own `mergedpages.xaml` via `xamlmerge-{material,cupertino}.props`.
3. Add a sample page under `samples/Uno.Toolkit.Samples/Content/` (the shared project) so all three sample heads pick it up.
4. Add tests under `src/Uno.Toolkit.RuntimeTests/Tests/`. Reusable test pages live in `Tests/TestPages/`.
5. Add a doc under `doc/controls/`; if the control introduces new style keys, update `doc/controls-styles.md` and `doc/lightweight-styling.md` accordingly.

---

## 12. Security & Reliability

✅ No secrets in code.
✅ Validate input at public API entry points where it influences resource lookups, file paths, or reflection.
✅ When deserializing user-supplied content (JSON, XAML fragments, theme data) treat it as untrusted and prefer typed, source-generated deserializers.

---

## 13. Documentation

The PR template (`.github/pull_request_template.md`) requires updating documentation as needed:

- **General docs**: `doc/` (e.g. `getting-started.md`, framework-specific getting-started variants).
- **Controls**: `doc/controls/` — every new control or behavioral change to an existing control needs a doc update here.
- **Extensions / helpers**: `doc/helpers/`.
- **Style keys**: `doc/controls-styles.md` and `doc/lightweight-styling.md` whenever resource keys are added, renamed, or removed.

✅ Prefer updating an existing page over adding a new one.
✅ Cross-link relevant pages (e.g. between a control's doc and `controls-styles.md`).
✅ Sample pages: add a page under `samples/Uno.Toolkit.Samples/Content/` (the shared project) so all sample heads pick it up.

</coding_directives>

<review_directives>

## 1. Pull Request (Agent) Checklist

Beyond the items in `.github/pull_request_template.md`:

- [ ] Related specs and progress.md updated with context and plan (if a plan was used).
- [ ] Release build: zero warnings/errors.
- [ ] Tests added for new/changed logic (list names).
- [ ] Runtime tests have been run via the appropriate sample-app head and MUST pass.
- [ ] Hot-reload tests added/updated where the change touches an HR-sensitive surface.
- [ ] No unjustified additions to `<NoWarn>`.
- [ ] SOLID + separation of concerns respected.
- [ ] Public API changes documented with XML docs and reflected in `doc/`.
- [ ] Structured logging; no sensitive data.
- [ ] Error handling consistent.
- [ ] No magic strings (constants added where needed).
- [ ] Performance considerations documented if hot path changed.
- [ ] PR description matches actual change scope and includes the required issue link.

---

## 2. Agent Prompting Guidance

Provide explicit constraints to reduce refactor churn:

1. Specify the target layer (e.g. control, behavior, extension, theme resource, runtime test).
2. Define the contract (DPs/attached properties, events, methods — inputs, outputs, errors).
3. Request tests inline with implementation.
4. State performance expectations (no extra allocations in measure/arrange, single pass).
5. Indicate error strategy (graceful fallback vs. exceptions).

Example:
> Add a `Loading` attached property in `Uno.Toolkit.UI` that, when set on a `FrameworkElement`, swaps in a `LoadingView`. Implement as an attached `DependencyProperty` with a `Get/Set<Name>` pair, hook `Loaded`/`Unloaded` to avoid leaks, add a runtime test under `src/Uno.Toolkit.RuntimeTests/Tests/` covering attach, change, and detach, plus a doc update under `doc/helpers/` and a sample under `samples/Uno.Toolkit.Samples/Content/`.

---

## 3. Definition of Done

1. Release build warning-free.
2. Tests added & passing (relevant runtime tests run).
3. Principles & conventions adhered to.
4. No unjustified performance regressions.
5. PR template checklist completed.

---

## 4. Exceptions Process

If a guideline cannot be met:

- Constraint
- Impact
- Mitigation / follow-up issue reference

Unexplained deviations block merge.

---

## 5. Quick Reference Table

| Area | Rule |
|------|------|
| Build | Release: zero warnings (TreatWarningsAsErrors) |
| Tests | New behavior + edge case (runtime test) |
| SOLID | All five applied |
| Allocations | Minimize hot paths; WASM-aware |
| Logging | Structured; no PII |
| Errors | Graceful for UI paths; specific catches |
| API | XML docs on public surface; additive change preferred |
| Constants | Centralize and document |
| Validation | At public entry points |
| Async | Honor cancellation; NEVER produce blocking code |
| XAML | Don't edit `Generated/`; let `XamlMergeInput` glob pick files up |

---

## 6. Source Control

- Commit messages: clear, imperative, reference issues.
- MUST follow [Conventional Commits](https://www.conventionalcommits.org/) — enforced by `.github/workflows/conventional-commits.yml`. Bullet points, no big walls of text.

</review_directives>

---

## Final Note

Agents must act deterministically and transparently. This document is the authoritative guardrail—adhere strictly to sustain maintainability, reliability, and trust.

---

(End of AGENTS Instructions)
