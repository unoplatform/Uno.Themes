# AI Agents Contribution & Coding Instructions

This document defines strict guardrails for any AI-assisted or automated agent contributions (including Copilot, custom prompt runners, or scripted refactors) working in the **Uno.Themes** repository. Human contributors must also ensure generated changes comply before merge. It is the single source of truth for repo-wide orientation *and* the rules agents must follow; `CLAUDE.md` at the repo root is a thin pointer that includes this file.

<repository_orientation>

## Repository overview

Uno.Themes ships the design-system theme libraries used by Uno Platform / WinUI apps (Material Design, Cupertino / iOS-style, and the Simple density-driven theme), plus the shared Uno Themes core (semantic palette, seed-color generation, design tokens) and C# Markup helpers. The repo produces seven NuGet packages:

- `Uno.Themes.WinUI` — base theme infrastructure (semantic palette, seed-color generation, design tokens, shared converters/extensions). Built from `src/library/Uno.Themes/Uno.Themes.WinUI.csproj`. All other theme libraries reference it.
- `Uno.Material.WinUI` — Material Design 3 styles and resources. Built from `src/library/Uno.Material/Uno.Material.WinUI.csproj`. Has a v1/v2 style split (see `Styles/Application/v1`, `Styles/Application/v2`, etc.).
- `Uno.Cupertino.WinUI` — Cupertino / iOS-style theme. Built from `src/library/Uno.Cupertino/Uno.Cupertino.WinUI.csproj`.
- `Uno.Simple.WinUI` — minimal density-driven theme (see `Density.cs`, `SimpleTheme.cs`). Built from `src/library/Uno.Simple.WinUI/Uno.Simple.WinUI.csproj`.
- `Uno.Themes.WinUI.Markup`, `Uno.Material.WinUI.Markup`, `Uno.Simple.WinUI.Markup` — C# Markup helpers (extension methods, brush/color/style accessors) layered on top of the corresponding theme library.

Despite the legacy folder name `src/library/Uno.Themes`, the assembly is `Uno.Themes.WinUI` (see `AssemblyName` in the csproj). Internals of the base library are visible to `Uno.Material(.WinUI)`, `Uno.Cupertino(.WinUI)`, and `Uno.Simple.WinUI` via `InternalsVisibleTo` (see `themes-common.props`).

## Solution layout

- `Uno.Themes.sln` (repo root) — main solution. Includes all libraries, samples, and the runtime tests that live inside the sample apps. Open this for full development.
- `Uno.Themes-packages.slnf` (repo root) — solution filter limited to the packable libraries; useful for fast restore/build without the sample apps.
- `src/library/` — all packable libraries. Subfolders match the project list above. Theme XAML lives under each library's `Styles/` folder (e.g. `src/library/Uno.Material/Styles/{Application,Controls}/{v1,v2,Common}/`, `src/library/Uno.Cupertino/Styles/{Application,Controls}/`, `src/library/Uno.Simple.WinUI/Styles/`). The base `src/library/Uno.Themes/Styles/Applications/Common/` holds shared color/typography palettes (`SharedColorPalette.xaml`, `SharedColors.xaml`, `SharedTypography.xaml`, `Converters.xaml`).
- `src/library/xamlmerge.targets` — pulls in `Uno.XamlMerge.Task`. Each library's `*-common.props` declares `XamlMergeInput` items; the task merges them into `mergedpages.xaml` (Material splits into `mergedpages.v1.xaml` / `mergedpages.v2.xaml`). **Do not hand-edit anything generated under `obj/` or referenced via the merged dictionary.**
- `src/samples/` — runnable sample apps and the shared sample project:
  - `src/samples/SamplesApp.Shared/` — shared project (`.shproj` / `.projitems`) holding the sample UI, `Shell.xaml`, content pages, view models, helpers.
  - `src/samples/MaterialSampleApp/` — Material sample head.
  - `src/samples/CupertinoSampleApp/` — Cupertino sample head.
  - `src/samples/SimpleSampleApp/` — Simple sample head; **also hosts the runtime tests** under `src/samples/SimpleSampleApp/RuntimeTests/Given_*.cs` (e.g. `Given_SeedColorPalette.cs`, `Given_SemanticStyles.cs`, `Given_ColorOverridePrecedence.cs`).
- `doc/` — published documentation (see §13).

There is **no separate runtime-tests project** — runtime tests live inside the sample apps and are driven by `Uno.UI.RuntimeTests.Engine` (`PackageReference Include="Uno.UI.RuntimeTests.Engine"` in each sample csproj).

## Target frameworks and platform builds

Target frameworks are managed centrally:
- `src/library/tfm-common-winui.props` expands library projects to `net9.0` plus per-platform suffixes (`net9.0-ios`, `net9.0-android`, `net9.0-windows10.0.19041`, `net9.0-maccatalyst`) based on `TargetFrameworkOverride` and the `Build_iOS` / `Build_Android` / `Build_Windows` switches.
- Each sample csproj declares its own `net10.0-*` set directly (see `MaterialSampleApp.csproj`, `CupertinoSampleApp.csproj`, `SimpleSampleApp.csproj`). Without an override, samples target `net10.0-android;net10.0-ios;net10.0-browserwasm;net10.0-desktop`.
- The Uno SDK version is pinned in `global.json` at the repo root and in `src/samples/global.json` (`Uno.Sdk` and `Uno.Sdk.Private` — keep these in sync).

The top-level `Directory.Build.props` exposes `Build_Android`, `Build_iOS`, `Build_MacOS`, `Build_Windows` switches; non-Windows hosts default `Build_Windows=false`. The single-platform local-build flow (via `crosstargeting_override.props`) is documented in §4 below. The repo's default branch is **`master`**.

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
- Diff behavior between `master` and your changes when relevant
- Ask yourself: "Would a staff engineer approve this?"
- Run tests, check logs, demonstrate correctness
- You MUST assume that for a given branch, the `master` branch is correct and failures are specific to the current branch. You MUST assume that changes in the current branch are the cause of any new failures.

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
✅ Minimize use of the null-forgiving operator (`!`); prefer explicit guards or refactors that satisfy the nullable flow. Sample apps enable `Nullable=enable` (`src/samples/Directory.Build.props`); when adding new files to libraries that don't yet enable nullable, prefer adding `#nullable enable` at the file scope rather than relying on `!`.  
✅ Separate concerns: theme infrastructure (`Uno.Themes`) | design-system styles (`Uno.Material`, `Uno.Cupertino`, `Uno.Simple.WinUI`) | C# markup helpers (`*.Markup`) | sample UI (`SamplesApp.Shared` + heads) | runtime tests (inside the Simple sample app).  
✅ Favor composition over inheritance; depend on abstractions where the API surface justifies it.  
✅ Optimize only with evidence (profiling/metrics).

---

## 2. Performance & Allocations

✅ Minimize allocations in hot paths (theme lookups, brush rebuilds on color override, palette regeneration on seed-color change). Prefer `StringBuilder` for string assembly and avoid LINQ in tight loops where measurement shows it costs.
✅ Use `readonly` on fields and structs where possible.
✅ Avoid boxing (watch generics, interpolated logging with value types). The HCT color math under `src/library/Uno.Themes/ColorGeneration/Hct/` runs in tight numeric loops — keep value-type discipline there.
✅ Reuse expensive objects (brushes, parsed resources, generated tonal palettes) where the lifecycle allows.
✅ Only introduce `Span<T>` / `Memory<T>` when profiling shows benefit.
✅ When using `System.Text.Json` (e.g. for Material Theme Builder DSP imports), prefer source-generated serializers (`[JsonSerializable]`) over reflection-based ones — required for AOT and beneficial for size on WASM.
✅ **Always use typed deserialization** — never parse JSON with `JsonDocument` / `JsonElement` / manual `GetProperty` chains. Define a typed model (only the fields you need) and deserialize into it.

### Memory & WASM Considerations

This repository targets WASM as a first-class platform (sample apps and runtime tests run there, including the CI `stage-build-wasm.yml` pipeline). On WASM, `WebAssembly.Memory.grow()` is **irreversible** — every peak allocation permanently inflates `HEAPU8.length`.

✅ **Be allocation-aware in theme code paths.** Rebuilding a tonal palette, regenerating semantic brushes after a color override, or merging style dictionaries can run on every theme change; an extra `ToList()` or repeated string concatenation in a frequently-invoked code path becomes a measurable footprint on WASM.
✅ **Release before allocate** when replacing large object graphs (e.g. swapping out a generated palette, replacing a merged resource dictionary). Don't hold the previous instance alongside the replacement longer than necessary.
✅ **Watch for leaks via static event subscriptions** on framework elements (color-override listeners, `Application.Resources` change handlers, theme-change subscribers). Unsubscribe in `Unloaded` / disposal paths.

---

## 3. Framework & Platform Usage

✅ Honor `CancellationToken` on any async public API and I/O boundary.
✅ NEVER use `.Result` / `.GetAwaiter().GetResult()` outside a controlled, documented sync bridging point.
✅ ALWAYS prefer non-blocking async flow to remain WASM-safe; if a sync bridge is unavoidable, document why and keep it local.
✅ Prefer the WinUI/Uno-provided primitives (`DependencyProperty`, `VisualStateManager`, `ResourceDictionary` merging, `FrameworkElement.Loaded/Unloaded`, `Application.RequestedThemeChanged`) over hand-rolled equivalents.
✅ Multi-platform aware: code that compiles for net9.0-windows (libraries) / net10.0-windows (samples), net9.0/net10.0-android, net9.0/net10.0-ios, net9.0-maccatalyst, and net10.0-browserwasm (samples). When platform behavior diverges, isolate it behind partial classes / conditional compilation symbols (`__WASM__`, `__ANDROID__`, `__IOS__`, etc.) — don't sprinkle `#if` blocks across method bodies if a platform-specific partial would do the job.

---

## Code Style Guidelines

Formatting and style rules are defined in the repo configuration files below. Treat these files as the source of truth and avoid duplicating their contents here.

✅ EditorConfig formatting and naming rules: [.editorconfig](.editorconfig) at the repo root, plus `src/samples/.editorconfig` for sample-specific overrides.
✅ XAML formatting rules: [Settings.XamlStyler](Settings.XamlStyler) at the repo root.
✅ Conventional Commits enforcement: [.github/workflows/conventional-commits.yml](.github/workflows/conventional-commits.yml) — runs on PRs targeting `master`, `release/*/*`, `feature/*`.
✅ Tabs for `.cs` / `.xaml` / `.xml` / `.targets` / `.props`; spaces for `.yml` / `.md`; CRLF line endings.
✅ The repo root `Directory.Build.props` sets `LangVersion=latest` and `AllowUnsafeBlocks=true`. New code must compile clean. **Do not introduce a global `TreatWarningsAsErrors` switch without prior approval** — the repo intentionally leaves it off today; new warnings in your changes must still be triaged or suppressed with justification.

## 4. Build & Validation

Primary solutions:
- `Uno.Themes.sln` — full development solution (libraries, samples, runtime tests inside samples).
- `Uno.Themes-packages.slnf` — solution filter for the packable libraries only.

### Single-platform development

Copy `crosstargeting_override.props.sample` (at the repo root) → `crosstargeting_override.props` and uncomment the desired `TargetFrameworkOverride` (`windows`, `ios`, `android`, `browserwasm`, `desktop`, `maccatalyst`, `macos`). The platform suffix is expanded to the right TFM by each project (libraries → `net9.0-*`, samples → `net10.0-*`). **Always close the IDE before changing this file** — switching while the solution is open corrupts the NuGet restore cache.

CLI equivalent:

```bash
# Restore (fast, single-platform)
dotnet restore Uno.Themes.sln -p:TargetFrameworkOverride=desktop

# Debug build
dotnet build Uno.Themes.sln -c Debug -p:TargetFrameworkOverride=desktop

# Release build
dotnet build Uno.Themes.sln -c Release -p:TargetFrameworkOverride=desktop "/clp:WarningsOnly;Summary"
```

### Sample-app builds (target framework chosen explicitly)

```bash
# Material Desktop / Skia
dotnet build src/samples/MaterialSampleApp/MaterialSampleApp.csproj -c Debug -f net10.0-desktop

# Cupertino WASM
dotnet build src/samples/CupertinoSampleApp/CupertinoSampleApp.csproj -c Debug -f net10.0-browserwasm

# Simple desktop (also hosts the runtime tests)
dotnet build src/samples/SimpleSampleApp/SimpleSampleApp.csproj -c Debug -f net10.0-desktop

# Run the desktop sample app interactively
dotnet run --project src/samples/MaterialSampleApp/MaterialSampleApp.csproj -f net10.0-desktop
```

✅ Treat new build warnings introduced by your change as defects — fix or, with justification, suppress at the narrowest possible scope.
✅ Suppress a warning only with justification in PR + targeted scope (`#pragma` with comment).
✅ Do not disable deterministic builds.
✅ Do not expand global `<NoWarn>` in `Directory.Build.props` (root) or `src/samples/Directory.Build.props` without prior approval.

## 5. Testing Requirements

Runtime tests live **inside the Simple sample app** under `src/samples/SimpleSampleApp/RuntimeTests/` (e.g. `Given_SeedColorPalette.cs`, `Given_SemanticStyles.cs`, `Given_ColorOverridePrecedence.cs`). They are MSTest-style and hosted by `Uno.UI.RuntimeTests.Engine`. There is **no `dotnet test`** entry point.

- Interactive: launch a sample app (the runtime-test runner UI is available when the test engine is wired up — for the Simple sample, runtime tests are the canonical host).
- Headless (used in CI): launch the built sample DLL with `--runtime-tests=<results.xml>`. The script `build/scripts/linux-skia-desktop-runtime-tests.sh` does this under `xvfb-run` against `bin/Release/net10.0-desktop`. The CI pipeline lives in `build/stage-runtimetests-desktop.yml`.

### General testing rules

✅ Every new public behavior in the theme libraries must include tests under `src/samples/SimpleSampleApp/RuntimeTests/` (or, for Material/Cupertino-only behavior that genuinely cannot be exercised through the Simple host, a clearly-justified placement). Prefer `Given_*` naming.
✅ AAA pattern (Arrange / Act / Assert).
✅ Lack of coverage for new logic blocks merge.
✅ **Every bug fix MUST follow red/fix/green**: first add a runtime test that reproduces the bug and fails, then apply the fix, then confirm it passes. The failing test must be committed alongside the fix so the regression is permanently guarded.
🚫 **Never use `Assert.Inconclusive`** (or equivalent). A test either asserts behavior and passes, or fails. Skipping via inconclusive hides regressions. If a scenario cannot run on a platform, gate it with an explicit platform attribute / `#if` instead.
🚫 **Do not deactivate, skip, `[Ignore]`, or delete an existing test** unless the underlying feature is being entirely removed from the product — not merely because a file was refactored, renamed, or a class was restructured. Refactors must keep the behavioral coverage intact; if a test no longer compiles after a rename, update it, don't remove it.

### Minimum Test Additions Per PR

| Change Type | Required Tests |
|-------------|----------------|
| New theme/palette/converter API | Happy path + 1 edge case (runtime test under `SimpleSampleApp/RuntimeTests/`) |
| New attached property / extension | Set/clear scenario + a teardown-leak guard if it subscribes to events |
| Bug fix | Repro test + non‑regression guard |
| New / changed style key | Resource-resolution test (e.g. asserting the key resolves under both Light and Dark themes) |
| Color-override / seed-color logic | Precedence test (extend `Given_ColorOverridePrecedence` / `Given_SeedColorPalette` rather than duplicating) |

### String equivalence assertions (multiline)

- Use raw strings (`"""..."""`) for expected and actual samples.
- Avoid manual newline normalization (`Replace("\r\n", "\n")`); rely on the test framework's options where available.

### Format XAML

```bash
# Run xaml styler (uses Settings.XamlStyler at the repo root)
dotnet dnx XamlStyler.Console -r -l Debug -c Settings.XamlStyler -d "."
```

✅ Always run runtime tests as part of verification of theme/style changes — they are not optional manual steps.
✅ Maintain or improve passing test count.
✅ Never delete tests without equivalent protection.

---

## 6. API Conventions

The deliverable here is a public NuGet API consumed by external Uno apps. Stability matters more than in an app-only codebase.

✅ Public types and members must have XML doc comments suitable for IntelliSense.
✅ `DependencyProperty` registrations: name the property field `<Name>Property`, expose a CLR wrapper, and follow the `Get/Set<Name>(DependencyObject)` convention for attached properties. Centralize default values, do not duplicate them.
✅ **Resource keys are part of the public API.** Renaming, removing, or changing the type of a published style/brush/color/converter key is a breaking change. Additive changes are preferred; removals require a deprecation path and explicit justification in the PR.
✅ Prefer additive change. A breaking change to a public API requires explicit justification in the PR description and is likely to be rejected (see PR template).
✅ Persisted enums: explicit underlying type; mark `[Flags]` only when actually flag-shaped.

---

## 7. Logging & Diagnostics

✅ Structured logging where applicable (`logger.LogInformation("Generated palette with seed {Seed}", seed)`).
✅ No PII / secrets / device identifiers in logs.
✅ Correct level semantics (Trace/Debug/Info/Warning/Error/Critical).
✅ If values are computed only for logging (for example `ToList()`, `string.Join(...)`, projections, or expensive formatting), wrap that computation in `if (logger.IsEnabled(LogLevel.X))` for the corresponding level so disabled logs do not pay the allocation/computation cost.

---

## 8. Error Handling

✅ Never swallow exceptions silently — wrap with context or let propagate.
✅ Order `catch` blocks from most specific to most general: catch predicted exceptions first (`OperationCanceledException`, `InvalidOperationException`, etc.), then use a generic `catch (Exception ex)` as the final fallback. Never use a bare `catch` or generic-only handler when specific exceptions are foreseeable.
✅ For theme code, prefer graceful degradation (e.g. fall back to a default brush / a known-good palette) over throwing from resource-resolution / template-application paths — exceptions in those paths can take down the visual tree on consumers.

---

## 9. Constants & Magic Strings

✅ Centralize non-trivial strings (resource keys, theme keys, color names, style names) — see `MaterialConstants.cs`, `CupertinoConstants.cs`, `SimpleConstants.cs`, `ThemesConstants.cs`. New keys belong in the matching constants file.
✅ Comment rationale for tonal-palette tones, opacity values, animation durations, threshold values.
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
✅ XAML in each library's `Styles/` tree is merged into one or more `mergedpages*.xaml` files at build time by `Uno.XamlMerge.Task` (configured per library in `cupertino-common.props`, `material-common.props`, `simple-common.props`, etc.; the task itself is wired in `src/library/xamlmerge.targets`). Material splits into `mergedpages.v1.xaml` and `mergedpages.v2.xaml`. **Do not edit merged outputs by hand.**
✅ Adding or moving a XAML file under a library's `Styles/` tree is picked up automatically by the `XamlMergeInput` glob — do not add an explicit `<Page>` reference. If a file must be excluded from the merge, declare an `XamlMergeInput Remove="..."` entry in the library's `*-common.props` (see how `Cupertino`/`Material` already exclude color-palette and font dictionaries that are exposed as standalone resources).
✅ XAML namespaces `mobile` / `not_mobile` are toggled via `IncludeXamlNamespaces` / `ExcludeXamlNamespaces` in `themes-common.props` for mobile vs non-mobile targets — use them rather than `#if` for XAML-side platform divergence.

### Adding a new style or theme resource — checklist

1. Decide the layer:
   - Cross-design-system shared resources → `src/library/Uno.Themes/Styles/Applications/Common/`.
   - Material → `src/library/Uno.Material/Styles/{Application,Controls}/{v1,v2,Common}/` (pick the correct version slot; if both, also extend the `XamlMergeInput` mapping in `material-common.props`).
   - Cupertino → `src/library/Uno.Cupertino/Styles/{Application,Controls}/`.
   - Simple → `src/library/Uno.Simple.WinUI/Styles/`.
2. Companion XAML files are picked up automatically by the `XamlMergeInput` glob — do not add an explicit `<Page>` reference.
3. Add a sample page (or extend an existing one) under `src/samples/SamplesApp.Shared/Content/` so all three sample heads pick it up.
4. Add a runtime test under `src/samples/SimpleSampleApp/RuntimeTests/` covering resolution under Light and Dark themes (and, where relevant, override precedence).
5. Update the doc:
   - Material → `doc/material-controls-styles.md`, plus `doc/material-colors.md` / `doc/material-dsp.md` / `doc/material-getting-started.md` as relevant.
   - Cupertino → `doc/cupertino-controls-styles.md`, `doc/cupertino-getting-started.md`.
   - Simple → `doc/simple-controls-styles.md`, `doc/simple-getting-started.md`.
   - Lightweight-styling key → `doc/lightweight-styling.md`.
   - Cross-cutting (semantic styles, design tokens, seed colors, shared brushes) → `doc/semantic-styles.md`, `doc/design-tokens.md`, `doc/seed-colors.md`, `doc/themes-overview.md`, `doc/themes-control-extensions.md`.

---

## 12. Security & Reliability

✅ No secrets in code.
✅ Validate input at public API entry points where it influences resource lookups, file paths, or reflection. Theme overrides accept consumer-supplied keys and color values — treat them as untrusted.
✅ When deserializing user-supplied content (Material Theme Builder DSP JSON, theme overrides, color palettes) treat it as untrusted and prefer typed, source-generated deserializers.

---

## 13. Documentation

The PR template (`.github/pull_request_template.md`) explicitly calls out `doc/material-controls-styles.md`, `doc/cupertino-controls-styles.md`, and `doc/lightweight-styling.md`. Documentation lives at the repo's `doc/` root (no nested `controls/` or `helpers/` subfolders for this repo):

- **General overviews / getting started**: `doc/themes-overview.md`, `doc/material-getting-started.md`, `doc/cupertino-getting-started.md`, `doc/simple-getting-started.md`, `doc/fluent-getting-started.md`.
- **Style keys / lightweight styling**: `doc/material-controls-styles.md`, `doc/cupertino-controls-styles.md`, `doc/simple-controls-styles.md`, `doc/lightweight-styling.md`. Per-control style snapshots live under `doc/styles/`.
- **Theme system internals**: `doc/semantic-styles.md`, `doc/design-tokens.md`, `doc/seed-colors.md`, `doc/material-colors.md`, `doc/material-dsp.md`, `doc/themes-control-extensions.md`, `doc/material-migration.md`.

✅ Prefer updating an existing page over adding a new one.
✅ Cross-link relevant pages (e.g. between `lightweight-styling.md` and a per-design-system controls-styles page).
✅ Sample pages: add a page under `src/samples/SamplesApp.Shared/Content/` so all sample heads pick it up.

</coding_directives>

<review_directives>

## 1. Pull Request (Agent) Checklist

Beyond the items in `.github/pull_request_template.md`:

- [ ] Related specs and progress.md updated with context and plan (if a plan was used).
- [ ] Build clean for the platforms the change touches; no new warnings introduced.
- [ ] Tests added for new/changed logic (list names) under `src/samples/SimpleSampleApp/RuntimeTests/`.
- [ ] Runtime tests have been run via `SimpleSampleApp` (interactively or via the desktop runtime-tests script) and MUST pass.
- [ ] No unjustified additions to `<NoWarn>`.
- [ ] SOLID + separation of concerns respected.
- [ ] Public API changes (including resource keys) documented with XML docs and reflected in `doc/`.
- [ ] Structured logging; no sensitive data.
- [ ] Error handling consistent.
- [ ] No magic strings (constants added to the matching `*Constants.cs` where needed).
- [ ] Performance considerations documented if a hot path changed (palette regen, brush rebuild, theme switch).
- [ ] PR description matches actual change scope and includes the required issue link.
- [ ] PR template platform-tested checkboxes (UWP/iOS/Android/WASM/MacOS) reflect actual coverage.

---

## 2. Agent Prompting Guidance

Provide explicit constraints to reduce refactor churn:

1. Specify the target layer (e.g. `Uno.Themes` core, `Uno.Material` v1/v2 styles, `Uno.Cupertino`, `Uno.Simple.WinUI`, a `*.Markup` helper, sample UI, runtime tests).
2. Define the contract (DPs/attached properties, events, methods, resource keys — inputs, outputs, errors).
3. Request tests inline with implementation.
4. State performance expectations (no extra allocations on theme switch / color override / palette regen).
5. Indicate error strategy (graceful fallback to a default brush vs. exceptions).

Example:
> Add a `BorderBrushPressed` lightweight-styling key to the Material `Button` style under `src/library/Uno.Material/Styles/Controls/v2/`. Expose the key in `MaterialConstants.cs`, ensure the `XamlMergeInput` glob picks the file up (no explicit `<Page>` reference), add a runtime test under `src/samples/SimpleSampleApp/RuntimeTests/` asserting the key resolves under both Light and Dark themes, update `doc/material-controls-styles.md` and `doc/lightweight-styling.md`, and add a sample page (or extend the existing Button sample) under `src/samples/SamplesApp.Shared/Content/`.

---

## 3. Definition of Done

1. Build clean on the target platforms; no new warnings.
2. Tests added & passing (relevant runtime tests run via `SimpleSampleApp`).
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
| Build | No new warnings; suppress with justification only |
| Tests | New behavior + edge case; runtime tests live in `SimpleSampleApp/RuntimeTests/` |
| SOLID | All five applied |
| Allocations | Minimize on theme/palette/brush rebuilds; WASM-aware |
| Logging | Structured; no PII |
| Errors | Graceful fallback for resource resolution; specific catches |
| API | XML docs on public surface (incl. resource keys); additive change preferred |
| Constants | Centralize in `*Constants.cs` |
| Validation | At public entry points; treat consumer overrides as untrusted |
| Async | Honor cancellation; NEVER produce blocking code |
| XAML | Don't edit merged outputs; let `XamlMergeInput` glob pick files up; use `mobile` / `not_mobile` namespaces for platform divergence |

---

## 6. Source Control

- Commit messages: clear, imperative, reference issues.
- MUST follow [Conventional Commits](https://www.conventionalcommits.org/) — enforced by `.github/workflows/conventional-commits.yml`. Bullet points, no big walls of text.
- Default branch is **`master`**.

</review_directives>

---

## Final Note

Agents must act deterministically and transparently. This document is the authoritative guardrail—adhere strictly to sustain maintainability, reliability, and trust.

---

(End of AGENTS Instructions)
