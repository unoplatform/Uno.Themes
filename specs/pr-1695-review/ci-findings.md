# PR #1695 CI findings

Investigated 2026-09-14 against Azure build 232375 and GitHub Actions run 34120261845. Builds and runtime tests for the revised branch are tracked separately in `progress.md`.

## iOS compilation failure

[Azure build 232375, Simple iOS build log 473](https://dev.azure.com/uno-platform/1dd81cbd-cb35-41de-a570-b0df3571a196/_apis/build/builds/232375/logs/473?api-version=7.1) reports two errors in `src/samples/SimpleSampleApp/RuntimeTests/Given_FluentSeedAccent.cs`:

```text
Given_FluentSeedAccent.cs(155,75): error CS0037: Cannot convert null to 'Color' because it is a non-nullable value type
Given_FluentSeedAccent.cs(852,66): error CS0037: Cannot convert null to 'Color' because it is a non-nullable value type
```

Both expressions used an untyped null arm with a `Color` value arm. Explicit `(Color?)null` arms preserve the nullable result and remove the platform-dependent conditional-expression conversion. The existing test methods retain their behavioral assertions. A search of the other `Given_Fluent*.cs` files found no additional matching nullable-color conditionals. The recorded CI failure provides the red compilation evidence; an iOS build on macOS remains necessary to confirm green on the original platform.

## Deployment capacity blocker

[GitHub deploy job 101738767138](https://github.com/unoplatform/Uno.Themes/actions/runs/34120261845/job/101738767138) downloaded the successful WASM build artifact and reached Azure upload. Azure rejected it with `BadRequest` because the Static Web App already had the maximum number of staging environments. This is an environment-capacity failure after a successful build.

The workflow already closes environments on same-repository PR closure. Its upload and closure logic matches master. The same capacity failure occurred in another PR's September 12 deployment; a September 14 PR-close cleanup completed successfully, but its associated upload had failed, so that does not demonstrate a released slot.

Read-only Azure CLI inspection found no Uno.Themes Static Web App in either accessible subscription (`Uno Platform - Operations`, `Uno Platform Dev Operations`). The account cannot currently identify or clean up the affected resource. An owner with access must inspect the actual staging environments and remove an obsolete one or increase capacity before retrying deployment. No Azure resources were changed, and the workflow failure has not been suppressed.

## Runtime-test CI coverage

`FluentSampleApp/RuntimeTests/Given_FluentSamplePages.cs` verifies page templates against an application-level Fluent theme. It deliberately resides in the Fluent head, while the library tests reside in the Simple head. The desktop runtime-test matrix previously ran only Material and Simple; building Fluent or loading it in the wrapper hosting smoke does not execute these page tests.

Added Fluent to `build/stage-runtimetests-desktop.yml` so the existing generic build/run/publish steps execute and report its runtime tests.

## Project integration review

- Master's MSTest migration (`deff47bd`, PR #1724) is preserved: runtime engine `2.0.0-dev.85` and MSTest `4.3.3` move together. Fluent uses the centralized package versions. Its tests do not use removed `ExpectedException` or `ThrowsException` APIs. The shared `DescriptionAttribute` disambiguation is retained.
- Fluent is included in the Android, iOS, Desktop, and WASM build matrices, the main solution, and the packable-library solution filter.
- The wrapper's desktop ordering reference, WASM payload list, guest build script, and guest catalog all include Fluent. No extra font package is needed for its platform-default font.
- The Fluent library declares its package and assembly identity consistently. Standalone font, typography, palette, and lightweight-default dictionaries are excluded from XamlMerge as intended and remain XAML pages.
- Sample application identity, shared-project import, and application resource ordering follow the other sample heads; XamlControlsResources precedes FluentTheme.

No further blocking project-integration defects were identified by this static review. No builds or tests were run by this review lane to avoid contention with the main verification process.

## Fresh publication deployment

GitHub run 34919760692 successfully built and deployed revision 7e9c6fc4 after publication. The earlier staging-capacity failure no longer reproduces; no Azure resources or workflow checks were modified to obtain this result. Later revisions still require their own final CI checks.
