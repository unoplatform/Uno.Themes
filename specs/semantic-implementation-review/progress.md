# Semantic implementation review

Review the current Material v2, Simple, and Fluent implementations against the shared Uno Semantic Design Language. Report implementation gaps with evidence; fix documentation gaps without changing product behavior.

## Plan

- [x] Inventory semantic control styles, typography, palette/brush keys, design tokens, and public configuration APIs.
- [x] Review each theme's resource resolution, override scopes, precedence, runtime updates, and platform assumptions.
- [x] Compare runtime coverage with the cross-theme contract and investigate concrete discrepancies.
- [x] Correct published documentation and cross-links to match verified implementation behavior and explicitly identify portability gaps.
- [x] Attempt Desktop and WebAssembly builds and run available runtime tests; record the WebAssembly and documentation-tool limitations below.
- [x] Record findings, validation results, and remaining limitations in a review report.

## Review

Review complete at branch commit `4c9f46c8`. See [review.md](review.md) for the prioritized findings and links to the three detailed implementation audits. Product defects remain findings for follow-up implementation; this task changed documentation and public XML comments only. A second independent documentation review corrected three factual issues before final validation.

## Validation results

| Check | Result |
|---|---|
| SimpleSampleApp Desktop build, including shared, Simple, and Fluent libraries | Passed; final build 92 warnings, 0 errors |
| MaterialSampleApp Desktop build | Passed; final build 89 warnings, 0 errors |
| Uno.Themes.WinUI.Markup library build | Passed; 8 warnings, 0 errors |
| SimpleSampleApp complete runtime suite | 509 passed, 0 failed, 1 existing skipped test |
| MaterialSampleApp complete runtime suite | 63 passed, 0 failed, 0 skipped |
| SimpleSampleApp WebAssembly build | Failed at native linking: Emscripten Node reported `EPERM: operation not permitted, lstat 'C:\Users\SteveBilogan'` |
| Semantic source inventory against documentation | All 55 control-style keys documented; availability 55 Material / 51 Simple / 54 Fluent; 33 colors and 280 brush keys per appearance verified from XAML |
| Markdown relative files and heading links in all 17 changed docs | Passed |
| Non-documentation content of `BaseTheme.cs` compared with HEAD | Identical; no executable source changes |
| `git diff --check` | Passed |
| markdownlint / cSpell | Not run: executables unavailable; offline `npx` attempts returned `ENOTCACHED` |

Build commands used Debug, `--no-restore`, `-p:TargetFrameworkOverride=desktop` (or `browserwasm`), and `-p:PackageIcon=uno.png`. The icon property bypassed a packaging-only remote logo download that initially failed with an SSL/credential error; no project settings were modified. Builds reported existing code/package-source/obsolete-API warnings, including cached package vulnerability warnings. No package upgrades were performed. Runtime tests used `DOTNET_MODIFIABLE_ASSEMBLIES=debug`, `UNO_RUNTIME_TESTS_RUN_TESTS={}`, an explicit output environment variable, and the matching `--runtime-tests` argument.

The skipped test is `Given_HotReload.When_BaseThemeIsCollected_Then_HotReloadHandlerDoesNotResurrectIt`, already marked `[Ignore]` in the branch. This leaves the leak guard unvalidated; no tests were skipped or changed by this review. The hidden Windows renderer logged `BitBlt` invalid-handle and `DestroyWindow` access errors during the Simple run despite its successful test results, so these runs are not pixel-level visual verification.

No WebAssembly runtime suite or native WinUI/Windows-target build was completed. Newly found override behaviors were traced from source, not red-proven with new tests. The possible Fluent Dark-only fallback and same-control native seed-refresh issues are explicitly recorded as requiring runtime confirmation.

Full local build/test logs and NUnit XML are retained in this directory and ignored by its `.gitignore`. The checked-in report includes the durable outcome rather than generated artifacts.

## Documentation stack

- [x] Confirm the pending review changes consist of documentation, review notes, and XML comments only.
- [x] Use `gh stack` to create `dev/sb/fluent-theme-2-semantic-docs` above `dev/sb/fluent-theme`, keeping the implementation branch at `4c9f46c8`.
- [x] Commit the review changes on the documentation branch and verify the split is lossless.

The build and runtime results above apply to this documentation content. The split changes Git organization only; WebAssembly and documentation-tool limitations remain as recorded above. Publishing the stack is outside this local branch operation.
