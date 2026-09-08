# Semantic implementation fixes

Implement the findings from `specs/semantic-implementation-review/review.md` (documentation branch), preserving design-system defaults while making explicit semantic overrides effective.

## Stack plan

1. Update `dev/sb/fluent-theme` / PR #1695 with Fluent-specific fixes and regression tests.
2. Add a stack layer for shared, Simple, Material, and C# Markup fixes with their regression tests.
3. Restack documentation PR #1719 and update documentation to describe the fixed behavior; publish the resulting stack.

## Fluent layer

- [x] Reproduce nested/constructor/source override failures, appearance fallback, and semantic accent color/brush overrides.
- [x] Fix accent and lightweight bridge resolution and text-button rendered states; expose the missing presenter style.
- [x] Reproduce and fix root-font override cascading, source reload fallback, system-accent refresh, and same-control live seed/override changes.
- [x] Run focused red/green cases and the complete Desktop runtime suite; build Desktop and WebAssembly.
- [x] Commit Fluent changes on the implementation branch.

## Remaining layer

- [ ] Fix shared nested appearance resolution and normalize invalid shape values.
- [ ] Fix Simple outlined/icon-toggle/state resources, missing styles, grayscale legacy roles, and button measurements/typography.
- [ ] Fix Material rating/calendar state resources and typography precedence.
- [ ] Correct typed markup helpers and complete missing style/font/color helpers with consumer coverage.
- [ ] Run focused red/green cases and full relevant suites; build Desktop and WebAssembly.
- [ ] Commit, restack documentation, update docs, and publish the new stacked PR.

## Validation

Record actual command outcomes, failing repros, and remaining platform limits here as work proceeds. No finding is considered fixed solely because its resource key resolves; rendered-state issues require realized controls.

### Fluent red/green work

- Initial Fluent suite: 37 reproduced failures, 253 passed (`fluent-all-red.xml`). Includes source-reload exception, constructor overrides, nested bridge input, text-button states, font-root cascading, stale system accent, and same-control seed/override changes.
- First complete Light run after fixes: 556 passed, 0 failed, 1 existing ignored hot-reload leak guard (`fluent-layer-full-light.xml`).
- WebAssembly build passed with 92 warnings, 0 errors (`fluent-wasm-build.log`); native linking succeeded with the updated execution permissions. Runtime execution remains Desktop-only at this stage.
- A subsequently added text-button collection guard exposed semantic-path retention after loaded/style-replacement/unload; a native-style baseline passes. This must be resolved before committing the layer.
- The observer lifetime test replaces the theme before collecting the old one: Uno's Source-dictionary cache retains the latest theme through shared merged-child parent pointers. The test deliberately keeps the old UISettings publisher alive and confirms its subscription does not retain the replaced theme.

Commands use `dotnet build src/samples/SimpleSampleApp/SimpleSampleApp.csproj -c Debug -f net10.0-desktop -p:TargetFrameworkOverride=desktop -p:PackageIcon=uno.png --no-restore` (or browserwasm). The runtime host receives `DOTNET_MODIFIABLE_ASSEMBLIES=debug`, a JSON `UNO_RUNTIME_TESTS_RUN_TESTS` filter, and both `UNO_RUNTIME_TESTS_OUTPUT_PATH` and `--runtime-tests`. `UNO_RUNTIME_TESTS_THEME=Light|Dark` now selects the real application appearance before XAML initialization; element-only RequestedTheme does not test application fallback.

Final Fluent layer: complete Desktop suites passed in both real application appearances: 558 passed, 0 failed, 1 existing skipped test each (fluent-final-light.xml / fluent-final-dark.xml). Desktop build: 90 warnings, 0 errors; final WebAssembly build: 92 warnings, 0 errors. XAML Styler ran on the three changed dictionaries; unchanged existing token formatting was retained to avoid unrelated churn.

The text-button guard exposed a real Uno-specific lifetime defect in the new adapter: plain PropertyMetadata on shared Brush-valued attached properties creates an inherited context whose associated parent can survive clearing to null. Registering these resource inputs with ValueDoesNotInheritDataContext under HAS_UNO fixes the retention. Native-style baseline and semantic-style teardown now both pass. The observer test and all existing tests remain enabled; only the branch's pre-existing ignored hot-reload guard is skipped.
