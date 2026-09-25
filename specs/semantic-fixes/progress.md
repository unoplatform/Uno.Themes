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

- [x] Fix shared nested appearance resolution and normalize invalid shape values.
- [x] Fix Simple outlined/icon-toggle/state resources, missing styles, grayscale legacy roles, and button measurements/typography.
- [x] Fix Material rating/calendar state resources and typography precedence.
- [x] Correct typed markup helpers and complete missing style/font/color helpers with consumer coverage.
- [x] Run focused red/green cases and full relevant suites; build Desktop and WebAssembly.
- [x] Complete the remaining fix layer, restack documentation, and prepare the validated stacked PR for publication.

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

### Remaining fixes and final review

- Shared/Simple runtime red: 104 failed, 565 passed, one existing skip in `remaining-all-red.xml`. Two palette cases initially timed out because empty Borders had no size; the corrected 20×20 consumers then reproduced the grayscale defect (`remaining-first-green.xml`).
- Material red: seven failures and 64 passes (`material-red.xml`): selected-rating hover, calendar glyph, and Light LabelExtraSmall weight. Final Material suite: 74 passed, zero failed (`material-final.xml`), including v1 typography preservation and actual Pips path-string resources.
- Markup compilation reproduced the unusable FontWeight/Pips generic signatures and missing helpers (`markup-red-build.log`). Runtime tests additionally reproduced eleven incorrect style TargetType attributes. All corrected typed consumers and metadata tests pass.
- Final Simple host suites: **671 passed, zero failed, one pre-existing skipped hot-reload guard in each real Light and Dark application appearance** (`remaining-verified-light.xml`, `remaining-verified-dark.xml`). Includes all Fluent tests, shared resources, Simple rendered-state/geometry/palette cases, and markup consumers.
- Final review added a HighContrast guard that reproduced a regression in the initial resolver extraction: shared brushes must retain HighContrast → Dark → Default palette selection. The shared resolver now accepts that explicit fallback list while Fluent retains its native appearance policy; the new guard passes.
- Desktop builds passed: Simple host 85 warnings, Material host 87 warnings, zero errors. Final WebAssembly builds passed: Simple host 93 warnings and Material host 91 warnings, zero errors. Warnings include existing obsolete API/nullability/trim warnings and NuGet advisories; no new warnings point to the changed tests or helper implementations.
- XamlStyler ran on all changed Simple and Material dictionaries. Unchanged lexical tokens kept their existing formatting; edited/new C#/XAML uses repository line-ending and indentation conventions.

The existing documentation branch was restacked above the Fluent commit. Because `gh stack add` appends to the stack, the remaining fixes use `dev/sb/fluent-theme-3-semantic-fixes` above `dev/sb/fluent-theme-2-semantic-docs`, preserving the published documentation PR. No runtime test was removed or disabled.

Platform limits: runtime execution was Uno Desktop; WebAssembly was compiled/linked, not run in a browser. Native WinUI, mobile runtimes, pixel screenshots, and an actual OS accent change were not validated. Markup consumer tests establish the resource scope before applying typed bindings, consistent with the current Markup package's eager resource lookup; they do not claim to fix upstream Markup resource-scope behavior.

- Independent final review reproduced finite radius overflow with `double.MaxValue` (`radius-overflow-red.xml`: one failed, 54 passed). Values above `double.MaxValue / 7` now use the default scale; the final 671-test appearance runs include this guard.
- Published-document validation passed: markdownlint and cSpell over all 17 changed published Markdown files (zero spelling issues). Table-only formatting corrections satisfy the current MD060 rule without relaxing the repository configuration.

- [x] Published all three branches with `gh stack submit --auto`: Fluent #1695, documentation #1719, and remaining fixes #1721 in stack #1720. The new PR is a draft and targets the documentation layer. The repository's commitsar 0.11.2 requires the breaking-change body to start with `BREAKING CHANGE:`; the commit message was reordered accordingly after its first CI check.
