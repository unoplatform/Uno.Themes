# PR 1695 integration and review

- [x] Rebase onto current master and resolve conflicts, preserving both existing and Fluent behavior.
- [x] Audit all PR comments and failed CI jobs; apply code fixes and explain intentional leak-test collection.
- [x] Review the full branch diff for correctness, contracts, lifecycle, security, performance, samples, documentation, and CI coverage.
- [x] Build the affected libraries and sample heads; run runtime suites and record every failure and limitation.
- [x] Resolve four test-only GC review threads.
- [ ] Publish the rebased commits after explicit push approval, resolve the eight remaining addressed code-change threads, and obtain fresh CI results.
- [ ] Clear Azure preview staging capacity with the resource owner and verify deployment.

## Branch and changes

Initial state: clean `dev/sb/fluent-theme`, remote head `d7289c426d5fb1447de2a7223c9740fd8e7f4380`, PR #1695. Rebased onto `origin/master` at `deff47bd39c03e8c3f60148f80773dbc93747c46`. Master is an ancestor of the new branch. The original history is preserved in local backup ref `backup/pr-1695-before-rebase-20260914`.

Conflicts were reconciled in lessons, ignore rules, semantic documentation, and override tests. Master's package-version and MSTest 4/runtime-engine changes are preserved. Existing override-source regressions and Fluent tests remain present.

Implemented fixes:

- Explicit nullable Color casts for the two iOS CS0037 compilation errors.
- Typed MSTest 4 assertions and initialized XCR host resources for the review comments.
- Fluent sample shell capture and browser display name corrected.
- Native lightweight override clearing restores existing solid brushes, including scoped seed colors and explicit native overrides; generated semantic mappings respect explicit native keys.
- Fluent sample-page tests now run in the desktop CI matrix, discover pages without nullable tuple warnings, and restore both seed and generation mode.
- Stale documentation, a broken anchor, and one branch-introduced assertion-order warning corrected. High Contrast limitations documented publicly.

## Final verification

All commands ran on Windows with .NET SDK 10.0.400. Desktop means Skia/Win32, not the native Windows/WinUI target. Logs and XML are retained locally under `artifacts/` and excluded from git.

| Check | Result |
| --- | --- |
| `dotnet build Uno.Themes.sln -c Release -p:TargetFrameworkOverride=desktop` | Passed; 0 errors, 335 warnings |
| Full SimpleSampleApp Release runtime suite, Attempts=1 | Passed: 570, Failed: 0, Skipped: 1 |
| Full MaterialSampleApp Release runtime suite, Attempts=1 | Passed: 63, Failed: 0, Skipped: 0 |
| Full FluentSampleApp Release runtime suite, Attempts=1 | Passed: 2, Failed: 0, Skipped: 0 |
| Given_FluentThemeLifecycle with application appearance Light | Passed: 22, Failed: 0, Skipped: 0 |
| Given_FluentThemeLifecycle with application appearance Dark | Passed: 22, Failed: 0, Skipped: 0 |
| Material, Cupertino, Simple, Fluent per-head Release WebAssembly builds | All passed |
| ThemesSampleApp Release WebAssembly publish, CompressionEnabled=false | Passed; published guest payload includes FluentSampleApp and Uno.Fluent.WinUI |
| Changed published docs: cSpell | Passed, 13 files, 0 issues |
| Markdown with Node 18-compatible markdownlint-cli 0.44.0 | Passed, including CI-equivalent repository glob |
| `git diff --check` | Passed |
| Windows wrapper hosting smoke | Failed; equivalent master scenario also fails, detailed below |

The skipped test is the pre-existing `[Ignore]` on `Given_HotReload.When_BaseThemeIsCollected_Then_HotReloadHandlerDoesNotResurrectIt`. No tests were disabled or deleted. The original baseline had 558 Simple passes and the same skip; twelve regression cases were added.

Runtime execution used `DOTNET_MODIFIABLE_ASSEMBLIES=debug`, `UNO_RUNTIME_TESTS_RUN_TESTS={"Attempts":1}`, `UNO_RUNTIME_TESTS_OUTPUT_PATH`, and the built head DLL's `--runtime-tests=<path>` argument. Appearance checks additionally used `UNO_RUNTIME_TESTS_THEME=Light` or `Dark` and the `Given_FluentThemeLifecycle` filter.

WebAssembly verification follows CI's dependency sequence: for each guest head, `dotnet build <head.csproj> -c Release -f net10.0-browserwasm -p:TargetFrameworkOverride=browserwasm -p:CompressionEnabled=false`; then `dotnet publish src/samples/ThemesSampleApp/ThemesSampleApp.csproj` with the same switches.

### Red/fix/green evidence

- Native fill clearing initially failed for Filled and Outlined buttons: retained brushes stayed red instead of returning to the platform baseline (`uno-themes-simple-red.xml`).
- Native versus semantic precedence failed with expected blue and actual red (`uno-themes-lifecycle-red2.xml`).
- A distinct scoped seed exposed the scoped-clear bug: expected orange `#FFFFB95C`, actual application blue `#FFBEC2FF` (`uno-themes-simple-final2.xml`).
- Final runtime suites and separate Light/Dark launches pass all added cases, including six native button state aliases and retained explicit native overrides.

An intermediate implementation enumerated lazy XCR resources and threw during materialization. It was replaced with targeted key lookups. Those failed attempts are not counted as successful validation.

## Remaining failures and limits

### Windows hosting smoke reproduces on master

The branch smoke exits 1: Material and Simple are not reclaimed during subsequent guest transitions; Cupertino and final Fluent unload are reclaimed.

An isolated checkout of exact master `deff47bd` builds successfully (0 errors, 300 warnings) and also fails Material reclamation. To compare the Simple transition fairly, the baseline catalog was experimentally extended with only a fourth `Material repeat` entry. That baseline builds (0 errors, 238 warnings) and reproduces the branch pattern exactly: Material and Simple fail transition reclamation, Cupertino and the final guest reclaim. Thus the failure is not Fluent-specific. No checks were suppressed. The experimental patch and logs are preserved in `artifacts/`; no experimental change was copied into this branch.

Linux/X11 hosting smoke was not run locally; fresh CI remains necessary.

### Full-solution WebAssembly invocation

The direct solution-wide WebAssembly build failed with MSB4057 (`GetCopyToPublishDirectoryItems`) on all four guest projects. CLI solution dependencies become synthetic references even though the wrapper intentionally excludes guest WASM project references. Master has the same three pre-existing dependency declarations; this comparison is structural, not a reproduced master WebAssembly failure. The documented CI per-head build/publish sequence passed. No empty targets or suppressions were added to conceal the failing invocation.

### External CI and review limits

- iOS CS0037 errors are corrected, but original-platform green requires macOS/CI; no local iOS build was claimed.
- Azure deployment remains blocked by maximum staging environments. The affected Static Web App is absent from both accessible Azure subscriptions. An owner must free obsolete staging capacity or increase capacity; no Azure resources were changed. See `ci-findings.md`.
- High Contrast remains an unresolved Fluent resource-support finding. Opposite-appearance native fallback and solid/gradient brush-type transitions are not fully verified. See `library-review.md`.
- Builds retain repository warnings. WebAssembly publish also reported transitive NuGet NU1903 audit warnings for System.Security.Cryptography.Xml 10.0.5. No dependency changes or warning suppressions were introduced; dependency remediation needs separate review.
- Latest markdownlint-cli 0.49.1 reports MD060 even on untouched master (204 diagnostics in the compared pages). Validation used the Node 18-compatible 0.44.0 CLI; no unrelated formatting churn was applied. CI globbing excludes hidden directories, so explicit `.claude` checks were not represented as CI failures.

## PR threads

Twelve threads were unresolved initially. Four test-only GC findings were explained and resolved on GitHub after leak-test verification. The other eight have local code fixes (XCR initialization, five nullable findings, sample shell capture, and display name); publication and closure remain pending push approval. Replies must not imply unpushed commits are already in the remote PR.

Push approval is required by the user's shared Git instructions. Publication should use force-with-lease pinned to the original remote head above, followed by review-thread resolution and fresh CI inspection.