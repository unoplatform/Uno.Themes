---
name: uno-themes-runtime-tests
description: Build, run, filter, and add runtime tests for the Uno.Themes design-system libraries. Use when running the SimpleSampleApp runtime tests headlessly under xvfb (CI-parity), launching the sample app interactively to use the in-app test runner UI, filtering tests by class or method name with the UNO_RUNTIME_TESTS_RUN_TESTS filter syntax, adding a new Given_*.cs runtime test under src/samples/SimpleSampleApp/RuntimeTests/, following the red/fix/green pattern for bug fixes, or reproducing the CI desktop runtime-tests pipeline locally.
metadata:
  author: uno-platform
  version: "1.0"
  category: testing
---

# Uno.Themes Runtime Tests

There is **no `dotnet test` entry point** in this repo. Runtime tests execute inside the running sample application, driven by `Uno.UI.RuntimeTests.Engine`. They exercise XAML resource resolution, semantic-style mapping, color-override precedence, seed-color palettes, design tokens, fonts, and hot-reload behavior against real `FrameworkElement` instances on the UI thread.

## Where tests live

- **Canonical host:** `src/samples/SimpleSampleApp/RuntimeTests/Given_*.cs`. Per `AGENTS.md` §5, new tests belong here unless the behavior is genuinely Material/Cupertino-specific.
- **Other hosts:** `src/samples/MaterialSampleApp/RuntimeTests/` (currently `Given_DesignTokens.cs`); `CupertinoSampleApp` references `Uno.UI.RuntimeTests.Engine` but hosts no tests today.
- **Naming:** `Given_<Subject>.cs`, e.g. `Given_SeedColorPalette`, `Given_SemanticStyles`, `Given_ColorOverridePrecedence`, `Given_DesignTokens`, `Given_Fonts`, `Given_HotReload`.
- **Attributes:** MSTest-style — `[TestClass]`, `[TestMethod]`, `[DataRow(...)]`, plus `[RunsOnUIThread]` for any test that touches `FrameworkElement` / `ResourceDictionary` / `Application.Resources`.

## CI pipeline

- Desktop is the only CI-validated runtime-test target. Pipeline: `build/stage-runtimetests-desktop.yml`. Driver script: `build/scripts/linux-skia-desktop-runtime-tests.sh`. It `dotnet publish`es `SimpleSampleApp` for `net10.0-desktop -c Release`, then runs the resulting `SimpleSampleApp.dll` under `xvfb-run` + `fluxbox`, writes NUnit XML, and post-validates that at least one `<test-case>` was emitted.
- WASM / Android / iOS runtime tests are not wired up in CI for this repo. They are technically reachable via `Uno.UI.RuntimeTests.Engine.Wasm.Runner` and per-platform heads, but treat that as out-of-scope here unless explicitly asked.

---

## Run the tests — headless (CI parity)

The fastest way to match what CI does, locally:

```bash
# From the repo root
dotnet publish -c Release -f net10.0-desktop -p:TargetFrameworkOverride=desktop \
  src/samples/SimpleSampleApp/SimpleSampleApp.csproj

ProjectPath=src/samples/SimpleSampleApp \
SampleAppName=SimpleSampleApp \
UNO_RUNTIME_TESTS_RUN_TESTS='{}' \
UNO_RUNTIME_TESTS_OUTPUT_PATH=/tmp/runtime-tests-results.xml \
  bash build/scripts/linux-skia-desktop-runtime-tests.sh
```

The script handles `xvfb-run`, fluxbox, and the post-run XML sanity check. Results land at `$UNO_RUNTIME_TESTS_OUTPUT_PATH` in NUnit format.

### Faster local iteration (Debug, no publish)

For day-to-day work, skip publish and run the Debug build directly:

```bash
dotnet build src/samples/SimpleSampleApp/SimpleSampleApp.csproj -c Debug -f net10.0-desktop

export DOTNET_MODIFIABLE_ASSEMBLIES=debug
export UNO_RUNTIME_TESTS_OUTPUT_PATH=/tmp/runtime-tests-results.xml
export UNO_RUNTIME_TESTS_RUN_TESTS='{}'

# Headless on a Linux host with no display: wrap in xvfb-run + fluxbox
xvfb-run --auto-servernum --server-args='-screen 0 1280x1024x24' bash -c "
  { fluxbox & } ; \
  dotnet src/samples/SimpleSampleApp/bin/Debug/net10.0-desktop/SimpleSampleApp.dll \
    --runtime-tests=\"$UNO_RUNTIME_TESTS_OUTPUT_PATH\"
"
```

✅ Always set `DOTNET_MODIFIABLE_ASSEMBLIES=debug` when running runtime tests — hot-reload tests under `Given_HotReload.cs` rely on modifiable assemblies and fail silently without it. The CI script sets this automatically; manual invocations must do it themselves.

---

## Run the tests — interactive (with UI)

Launch the Simple sample app normally and use the in-app runtime-test runner UI (the `UnitTestsControl` page wired up by `Uno.UI.RuntimeTests.Engine`):

```bash
dotnet run --project src/samples/SimpleSampleApp/SimpleSampleApp.csproj -f net10.0-desktop
```

Navigate to the runtime-tests page in the sample app shell. Use the built-in search box to filter, and click a test or class to run only that subset. Useful when debugging a single failing test or watching a UI assertion fail visually.

The Material and Cupertino sample heads also build and run interactively, but they are not the canonical runtime-test host; only run them this way if you've added a Material/Cupertino-specific test under their `RuntimeTests/` folder.

---

## Filter syntax

`UNO_RUNTIME_TESTS_RUN_TESTS` is JSON. Filter values are **text-contains** matches against the full test name (`Namespace.Class.Method`), composable with these operators:

- `&` — and
- `|` or `;` — or
- `!` — not
- `( ... )` — grouping

### Run everything

```bash
export UNO_RUNTIME_TESTS_RUN_TESTS='{}'
```

### Filter by class name

```bash
# Only the seed-color palette tests
export UNO_RUNTIME_TESTS_RUN_TESTS='{"Filter": {"Value": "Given_SeedColorPalette"}, "Attempts": 1}'
```

### Filter by method name

```bash
# Any test whose name contains "When_OverrideApplied"
export UNO_RUNTIME_TESTS_RUN_TESTS='{"Filter": {"Value": "When_OverrideApplied"}, "Attempts": 1}'
```

### Combine

```bash
# Color-override OR seed-color tests, excluding hot-reload
export UNO_RUNTIME_TESTS_RUN_TESTS='{"Filter": {"Value": "(Given_ColorOverridePrecedence | Given_SeedColorPalette) & !HotReload"}, "Attempts": 1}'
```

### Retry flakes

`Attempts` re-runs failures up to N times before marking the test failed. Default 1; raise only when diagnosing a known flake (don't paper over a real intermittent bug — see `AGENTS.md` §5).

```bash
export UNO_RUNTIME_TESTS_RUN_TESTS='{"Filter": {"Value": "Given_HotReload"}, "Attempts": 3}'
```

---

## Adding a new runtime test

Follow `AGENTS.md` §5 and the "Adding a new style or theme resource" checklist in §11. Quick form:

1. **Extend, don't duplicate.** If a `Given_*` file already covers the subject (e.g. `Given_ColorOverridePrecedence` for override-precedence work, `Given_SeedColorPalette` for seed-color logic, `Given_SemanticStyles` for semantic-key resolution), add a `[TestMethod]` there. Only create a new `Given_<Subject>.cs` when no existing file fits.
2. **Place under `src/samples/SimpleSampleApp/RuntimeTests/`.** A test added anywhere else (a fresh top-level test project, a different sample's `RuntimeTests/`) will silently not run under `linux-skia-desktop-runtime-tests.sh`.
3. **Mark UI-touching tests with `[RunsOnUIThread]`.** Anything that constructs a `FrameworkElement`, merges a `ResourceDictionary`, or reads `Application.Resources` needs it. Tests without the attribute run on a pool thread and will throw at the first DP touch.
4. **Use `[DataRow]` for matrix tests.** See `Given_SemanticStyles.When_SemanticAndSimpleStyles_AreApplied_LookIdentical` — one `[TestMethod]` with five `[DataRow]`s beats five copy-pasted methods.
5. **Cover the cleanup path too.** For attached properties and theme-change subscriptions, assert both the apply and the clear/revert path. Leak-guard tests using `WeakReference<T>` must store the tracker as a field, not a local — see `AGENTS.md` §2 "Separate stack frames for leak-guard runtime tests".
6. **Light + Dark.** For any new style/brush/color key, assert it resolves under both themes — the standard pattern is to switch `Application.Current.RequestedTheme` (or attach an explicit `RequestedTheme` to the container) and re-resolve.

### Skeleton

```csharp
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Uno.UI.RuntimeTests;

namespace Uno.Themes.Samples.RuntimeTests;

[TestClass]
public class Given_MyNewSubject
{
    [TestMethod]
    [RunsOnUIThread]
    public async Task When_<Scenario>_Then_<ExpectedOutcome>()
    {
        // Arrange — build a themed container so colors/styles are scoped
        var container = CreateThemedContainer();

        // Act
        // …

        // Assert
        // …
    }
}
```

---

## Red / fix / green (bug fixes)

`AGENTS.md` §5 requires this for every bug fix:

1. **Red.** Add a `[TestMethod]` that reproduces the bug. Run it locally with `UNO_RUNTIME_TESTS_RUN_TESTS='{"Filter":{"Value":"<your method>"}}'` and confirm it **fails**.
2. **Fix.** Make the smallest change that turns the test green.
3. **Green.** Re-run the same filter and confirm pass. Run the full suite at least once before committing to make sure nothing else regressed.
4. **Commit the failing test alongside the fix** so the regression is permanently guarded.

Skipping the red step (test added after the fix is already in place) defeats the purpose — there's no proof the test would have caught the bug.

---

## Inspecting results

The output XML is NUnit-format. Quick scans:

```bash
# Pass/fail summary
python3 - <<'PY'
import os, xml.etree.ElementTree as ET
tree = ET.parse(os.environ["UNO_RUNTIME_TESTS_OUTPUT_PATH"])
root = tree.getroot()
cases = tree.findall('.//test-case')
failed = [c for c in cases if c.get('result') == 'Failed']
print(f"{len(cases)} cases, {len(failed)} failed")
for c in failed:
    print(f"  FAIL: {c.get('fullname')}")
    fail = c.find('failure/message')
    if fail is not None and fail.text:
        print(f"    {fail.text.strip().splitlines()[0]}")
PY
```

Or open the XML and grep for `result="Failed"`.

---

## Common pitfalls

- **`DOTNET_MODIFIABLE_ASSEMBLIES=debug` missing** → `Given_HotReload` tests fail with a metadata-update error. Always export it for headless runs.
- **Test added outside `src/samples/SimpleSampleApp/RuntimeTests/`** → silently not picked up by CI. The CI script targets `SimpleSampleApp.dll`; anything else is invisible.
- **Missing `[RunsOnUIThread]` on a UI-touching test** → first DP access throws `RPC_E_WRONG_THREAD` / `InvalidOperationException` and the test is marked failed for the wrong reason.
- **Assertions that mutate `Application.Current.Resources`** → leak state into subsequent tests. Scope changes to a local container (`new Grid { Resources = { MergedDictionaries = { new SimpleTheme() } } }`) per `Given_SemanticStyles.CreateThemedContainer`.
- **`Assert.Inconclusive`** → banned by `AGENTS.md` §5. Gate platform-specific tests with `#if __WASM__` / `#if !__WASM__` or an explicit attribute, not by skipping.
- **Deleting or `[Ignore]`-ing a test on a refactor** → also banned by `AGENTS.md` §5. If a rename broke the test, update it; don't remove it.

---

## Quick reference

| Action | Command |
| --- | --- |
| Headless full run (CI parity) | `dotnet publish -c Release -f net10.0-desktop … ; bash build/scripts/linux-skia-desktop-runtime-tests.sh` |
| Headless quick run (Debug) | `dotnet build … -c Debug -f net10.0-desktop ; xvfb-run … dotnet SimpleSampleApp.dll --runtime-tests=<path>` |
| Interactive runner | `dotnet run --project src/samples/SimpleSampleApp/SimpleSampleApp.csproj -f net10.0-desktop` |
| Filter by class | `UNO_RUNTIME_TESTS_RUN_TESTS='{"Filter":{"Value":"Given_SeedColorPalette"}}'` |
| Filter by method | `UNO_RUNTIME_TESTS_RUN_TESTS='{"Filter":{"Value":"When_OverrideApplied"}}'` |
| Exclude a class | `UNO_RUNTIME_TESTS_RUN_TESTS='{"Filter":{"Value":"!Given_HotReload"}}'` |
| Retry flakes | `UNO_RUNTIME_TESTS_RUN_TESTS='{"Filter":{"Value":"…"}, "Attempts": 3}'` |
| Hot-reload tests need | `export DOTNET_MODIFIABLE_ASSEMBLIES=debug` |
