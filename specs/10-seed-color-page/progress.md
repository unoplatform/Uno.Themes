# Seed Color sample navigation

## Root cause and fix

The shared Seed Color page used SemanticThemeHelper, which queries
Application.Current.Resources. In ThemesSampleApp, Application.Current remains the
wrapper in the default assembly load context; it has no BaseTheme. The first seed-mode
assignment throws InvalidOperationException during page construction, so navigation
never replaces the previous content.

Each sample now captures its own theme after App.InitializeComponent. The shared
page updates that theme's Colors. The reference lives in each guest assembly's own
NavigationHelper and does not introduce a host-side root or change public library APIs.
Standalone construction passed before the fix; the initial event-order hypothesis
was ruled out and no event subscription changes were made.

## Plan

- [x] Trace navigation and reproduce page construction failure with a runtime test.
- [x] Fix the shared page's theme lookup with a minimal change.
- [x] Verify construction, seed edits, mode changes, and reentry in regression tests.
- [x] Complete Desktop and WebAssembly builds and actual hosted navigation smoke runs.
- [x] Review final changes and record all validation results.

## Validation

- Before fix: four standalone scenarios passed. Four scenarios retaining XAML resources
  while removing the ambient application's direct theme failed with
  `No BaseTheme ... found in Application.Current.Resources.MergedDictionaries`.
- After fix: all eight scenarios passed (Light/Dark, Fidelity/TonalSpot, normal/separate
  ambient resource scope). Coverage checks palette settings, swatch/hex/snippet, and
  preserved seed and mode after reconstructing the page. Tests restore dictionary
  placement, the original ThemeColors object, and remembered picker selections.
- Full Simple desktop runtime suite: **264 passed, 0 failed, 1 existing ignored test**
  (`When_BaseThemeIsCollected_Then_HotReloadHandlerDoesNotResurrectIt`).
- Wrapper and Simple/Material/Cupertino Desktop Debug builds: passed, 0 errors.
- Simple WebAssembly Debug build: passed, 0 errors.
- Material WebAssembly Debug build: passed, 0 errors.
- Final focused regression: **8 passed, 0 failed**.
- Actual hosted Simple and Material NavigationView smoke: **PASS** in both guests
  (navigate, seed/mode edits, navigate away and return).

The new test lives in SimpleSampleApp/RuntimeTests. Material can run the same test for
local validation via a temporary Compile include; no project file needs to change.

## Environment notes

Default restore first hit sandbox DNS restrictions, then package download timeouts.
The same pinned Uno.WinRT 6.4.229 and Uno.Settings.DevServer 1.7.1 packages were fetched
in ranges into `/tmp/seed-nuget-feed`, with SHA-512 verification against NuGet metadata.
Builds use `-p:RestoreAdditionalProjectSources=/tmp/seed-nuget-feed` and the SDK's
`-p:UnoDisableHotDesign=true` switch to omit optional design tooling. No package versions
or repository restore configuration changed. Existing nullable, NuGet source-mapping,
and WASM trimming warnings remain; the changed files introduce no new warnings.

Runtime command pattern:

```sh
DOTNET_MODIFIABLE_ASSEMBLIES=debug \
UNO_RUNTIME_TESTS_OUTPUT_PATH=/tmp/results.xml \
UNO_RUNTIME_TESTS_RUN_TESTS='{"Filter":{"Value":"Given_SeedColorSamplePage"},"Attempts":1}' \
dotnet src/samples/SimpleSampleApp/bin/Debug/net10.0-desktop/SimpleSampleApp.dll \
  --runtime-tests=/tmp/results.xml
```

Native UI automation could not attach to the command-line-launched macOS host.
The embedded runtime runner also selects Window.Current (the wrapper's window) and
cannot initialize its test UI there. No hosted runtime-test result is claimed.
Actual guest navigation is instead checked with a temporary external Compile include:
`/tmp/SeedNavigationSmoke.cs` and `/tmp/seed-hosted-smoke.targets`. It dispatches through
the guest's MainWindow and calls NavigationHelper.NavigateTo on the real Shell, then
checks edits and reentry. Launch the wrapper with `--app=simple` or `--app=material`
plus `--seed-navigation-smoke`; the probe logs PASS/FAIL and exits. This instrumentation
is outside the repository diff.

## Review

Independent review found no blocking issues. Theme ownership is initialized before
shell navigation and remains isolated in each collectible guest assembly. Existing
library APIs, XAML event wiring, resource keys, project files, and dependencies are
unchanged. `git diff --check` passed. The normal final Desktop build passed with 0 errors and removed the temporary
smoke Compile include from local outputs. Existing warnings remain.
