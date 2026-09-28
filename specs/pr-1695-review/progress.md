# PR 1695 integration and review

- [x] Rebase onto current master and resolve conflicts, preserving both existing and Fluent behavior.
- [x] Audit all PR comments and failed CI jobs; apply code fixes and explain intentional leak-test collection.
- [x] Review the full branch diff for correctness, contracts, lifecycle, security, performance, samples, documentation, and CI coverage.
- [x] Build the affected libraries and sample heads; run runtime suites and record every failure and limitation.
- [x] Resolve four test-only GC review threads.
- [x] Publish the rebased commits after explicit push approval and resolve the eight remaining addressed code-change threads.
- [ ] Verify fresh CI and address any newly generated review findings.
- [x] Verify Azure preview deployment: fresh GitHub run 34919760692 succeeded; the earlier staging-capacity failure no longer reproduces.

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
- The original Azure staging-capacity failure cleared externally: GitHub run 34919760692 successfully built and deployed the first published revision. No Azure resources or workflow capacity checks were changed. See `ci-findings.md`.
- High Contrast remains an unresolved Fluent resource-support finding. Opposite-appearance native fallback and solid/gradient brush-type transitions are not fully verified. See `library-review.md`.
- Builds retain repository warnings. WebAssembly publish also reported transitive NuGet NU1903 audit warnings for System.Security.Cryptography.Xml 10.0.5. No dependency changes or warning suppressions were introduced; dependency remediation needs separate review.
- Latest markdownlint-cli 0.49.1 reports MD060 even on untouched master (204 diagnostics in the compared pages). Validation used the Node 18-compatible 0.44.0 CLI; no unrelated formatting churn was applied. CI globbing excludes hidden directories, so explicit `.claude` checks were not represented as CI failures.

## PR threads

Twelve threads were unresolved initially. All twelve are now explained and resolved on GitHub. The eight code-change threads cover XCR initialization, five nullable findings, sample shell capture, and display name. After explicit approval, the rebased branch was published with a lease pinned to the original remote head.

The new automated review generated eleven more threads across two revisions. Ten nullable findings in seed-accent and semantic-style tests are addressed with typed MSTest assertions. The same remaining nullable assertion patterns across the Fluent tests were audited and converted together, preserving existing messages and weak-reference lifetimes. The XCR container warning is a false positive: the host dictionary is populated through MergedDictionaries, which TryGetValue traverses; the fourteen native-key runtime cases verify this behavior.

Fresh Azure build 233676 passed all three runtime suites and both documentation checks on the initially published revision. CodeQL and Conventional Commits also passed. Platform builds and deployment are still being monitored; these intermediate results do not claim final CI success.
Follow-up validation: the seed-accent assertion changes build successfully for Release Desktop (0 errors, 115 repository warnings). The complete Simple suite passes again: 570 passed, 0 failed, 1 existing skip. No production behavior or test coverage was changed.

The complete assertion cleanup also passes the Release Desktop solution build (0 errors, 441 warnings from rebuilt projects), Simple suite (570 passed, 0 failed, 1 existing skip), and Fluent suite (2 passed, 0 failed). Azure build 233677 additionally passed the Linux hosting smoke, confirming the earlier local Windows-only result is not a CI failure.

Fresh CI revision 944d9c2e: all runtime suites, Desktop builds, individual WebAssembly guest builds, Linux hosting smoke, CodeQL, and preview deployment passed. Simple iOS exposed one additional CS0037 in the new lifecycle regression's seeded/null conditional (build 233682, log 414). Its null arm is now explicitly Color?, matching the two original iOS fixes; a follow-up scan found no remaining uncast null conditionals in the affected test heads. Final iOS confirmation requires the next CI run.

Final review follow-up: native resource loaders now catch the supported InvalidOperationException explicitly before their documented generic initialization fallback. Final catch-all handlers remain at dependency-property and platform/dispatcher callback boundaries, where exceptions must not escape into consumer layout. A redundant nullable diagnostic expression was removed; typed MSTest assertions already report the actual type. Release Desktop solution build passes (0 errors, 335 warnings); Simple 570 passed / 0 failed / 1 existing skip and Fluent 2 passed / 0 failed. An attempted XamlParseException catch was rejected by the pinned package compiler and removed before publication.

## 2026-09-28 pickup (master merge, semantic sweep, docs)

- [x] Merge `origin/master` (f6eec9f5, Uno 7 ALC theme-handle changes). Four conflicts, all in samples/specs: `SeedColorSamplePage` keeps the Fluent preview flow and routes seed writes through `SampleThemeHelper`; `ListViewSamplePage` takes master's `DataType` attribute plus `Design.Fluent`; Simple `App` keeps both the ALC registration and the `UNO_RUNTIME_TESTS_THEME` selection; `lessons.md` keeps both sides. The Fluent head now registers `SampleThemeHelper.CurrentApplication` and names its `XamlDisplay` assembly like the other heads.
- [x] iOS CI (build 235690, all four heads): `CS0037` in `SeedColorSamplePage.CurrentPrimary` - the `? color : null` arm needs `(Color?)null` on iOS, where Uno's `UIColor` conversion gives the conditional a `Color` natural type. Fixed; the lookup also goes through the head's own application handle now.
- [x] Review threads: all 42 inline threads are bot-originated (code-quality / Copilot) and `kazo0` is the author. 41 resolved; the one open thread (`_initializing` readonly suggestion) is a false positive - the flag suppresses `ApplySeedColor` during constructor seeding. Reply and resolve on GitHub.
- [x] Semantic coverage sweep against the shared contract: all 54 documented style keys resolve (43 aliases, 2 late-bound bridge styles, 9 gap styles), 32/33 color roles are Fluent-mapped (`ShadowColor` keeps the shared default by design), all 19 typography slots + 11 character-spacing keys + root font, tokens inherited. Extra: `DatePickerFlyoutPresenterStyle` (Material-only until now) is shipped as a gap style - now documented. `RippleStyle` stays Material-only.
- [x] Found: value assertions in the Fluent suites resolve against the ambient branch only, and CI ran a single (Light) appearance. Added a `SimpleDark` matrix leg driven by `UNO_RUNTIME_TESTS_THEME`; documented the variable in the runtime-tests skill.
- [x] Late-bound bundle alias misses now log a warning instead of silently leaving `TextButtonStyle`/typography style keys unresolved.
- [x] Docs: stale Material/Simple enumerations on `design-tokens.md`, `seed-colors.md`, `themes-overview.md`, `cupertino-getting-started.md`; `DatePickerFlyoutPresenterStyle` row in `semantic-styles.md`; `AGENTS.md` package list / IVT list now include `Uno.Fluent.WinUI`.
- Note for #1719: several of its Fluent "limitation" claims (accent read once, `PrimaryBrush`/`OnPrimary*` not cascading, merged dictionaries not inspected, only rest-state `TextButton*` consumed, `DatePickerFlyoutPresenterStyle` GAP) describe the adapter before fbdc81f4 and are contradicted by the code in its own base. Re-verify before merging that layer.

Verification (Windows, Debug net10.0-desktop): Fluent and Simple heads build clean; full Simple suite Light 608 passed / 0 failed / 1 pre-existing skip; Fluent + override-precedence suites under Dark 354 passed / 0 failed. Release desktop restore failed locally only on a missing `Microsoft.NETCore.App.Runtime.Mono.win-x64 10.0.1` pack (environment). iOS not built locally.

## 2026-09-28 skeptic pass (full stack) and sample run

Integration check: `origin/dev/sb/fluent-theme-3-semantic-fixes` merged onto the updated #1695 head in a worktree (doc conflicts only, stack side kept). Simple suite 721 passed / 0 failed / 1 skip under both Light and Dark; Material 79/79; Fluent head 4/4; all three heads build clean on Debug desktop.

Sample run: Fluent AppBarButton page used `Content=` (Material/Simple templates render it, the native Fluent template only renders `Label`), so labels were missing under Fluent. Switched the Fluent template to `Label`. The 68px standalone width is WinUI's own `DefaultAppBarButtonStyle`.

Fluent library (#1695) - fix-first before packaging:
1. `FluentTheme.Resources.cs:82-87` merges `_lightweightDefaults` under a second parent (`baseline`) while it is already merged in the theme. `ResourceDictionaryExtensions.cs:47` records that WinAppSDK rejects the same dictionary instance under two parents; this runs from the constructor, so a WinUI-native app could fail at `<FluentTheme/>`. No head targets `net10.0-windows`, so every `!HAS_UNO` path is unexercised. Fix: build the baseline from a clone, and add a Windows head build (at least) to CI.
2. `FluentTextButtonResources.cs:136-140` roots up to 9 brushes + 18 bindings per text/icon button on theme-owned brushes; the only leak guard clears the style first. Add a guard that drops a styled button without re-styling.
3. `FluentTextButtonResources.FindExplicitOverride` (`:195-245`) enumerates every ancestor dictionary on each Loaded/ActualThemeChanged, which materializes lazy consumer resources (lessons.md hazard) and misses template-generated elements whose `Parent` is null.
4. `FluentLightweightBridge.CaptureNativeFallbacks` (`:237`) captures the ambient branch only; clearing an override re-binds the Dark-branch retained brush to the Light value in a `RequestedTheme="Dark"` subtree.
5. Nits: `SeedColorMode` re-derived instead of consumed (`FluentTheme.cs:179` vs `BaseTheme.cs:744`), library XAML edits ignored by hot reload, theme-key/token strings not in `FluentConstants`, per-rebuild allocation of two platform closures + bridge + fallbacks.

Stack (#1719 docs, #1721 fixes):
1. `ThemeResourceResolver.cs:38` enumerates every entry of every color layer per key per appearance per rebuild (Uno materializes lazy entries on enumeration; runs per color-picker tick). Replaces hash lookups; no `OverrideSource` XAML test with StaticResource-bearing values.
2. New resolver stops at the first existing appearance branch; the old `SemanticBrushUpdater` probed Dark then Default per key. Behavior change for Material/Simple consumers with partial Dark overrides, not in the BREAKING body and untested.
3. Markup breaking change is defensible (old generics were unusable as typed) but needs the justification in the PR body.
4. Simple default rendering changes for non-overriding consumers: `OutlinedButtonStyle` re-targeted, `SecondaryVariant*` grayscale, new public `ButtonBorderThickness` key - need changelog/doc entries.
5. #1719 documents behavior (accent refresh, nested overrides, TextButton states, radius normalization, `SimpleDatePickerFlyoutPresenterStyle`) that only exists in #1721, and edits `BaseTheme.cs`; not mergeable alone. Recommend squashing #1719 into #1721 and landing two PRs.