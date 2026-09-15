# Library review — PR #1695

Reviewed the rebased `origin/master...HEAD` library diff: the new Fluent adapter, its XAML/resource API and package configuration, and changes to `BaseTheme`, semantic color constants, seed generation, and brush updates. Checked surrounding runtime tests, the repository review lenses and lessons, and the local Uno implementations of `ResourceDictionary`, `XamlControlsResources`, and native button/check-box/toggle-switch resource aliases.

## Fixed findings

### Lightweight overrides did not restore existing native brushes when cleared

`FluentTheme.UpdateLightweightResources` supplied only semantic keys as the updater's fallback. The generated native keys (`AccentButtonBackground`, `ButtonBackground`, etc.) therefore had no fallback when an override was removed. Their dictionary entries disappeared, but already realized controls retained the old brush and color.

The fallback now captures native resources while the old generated lightweight layer is detached. A higher fallback layer resolves explicit native consumer overrides and the verified XCR accent aliases from this theme's current accent closure, per appearance. This preserves scoped seeds and overrides instead of replacing them with application defaults. The updater restores its own retained brush and leaves the consumer's original brush untouched.

- Red: both Filled and Outlined rows of `When_LightweightOverrideClears_ExistingNativeButtonAndRetainedBrushReturnToBaseline` failed with the old override color instead of the native baseline (`uno-themes-simple-red.xml`, run by the main agent).
- Coverage now includes Filled and Outlined without a seed, Filled with an application seed, and `When_ScopedLightweightOverrideClears_ExistingButtonAndRetainedBrushReturnToScopedSeed` for a scoped seed.
- `When_ScopedButtonOverrideClears_RetainedStateBrushUsesScopedAccent` covers all six accent-button background/foreground rest, hover, and pressed aliases; `When_ScopedSemanticMappingClears_ExplicitNativeOverrideRemains` covers a realized scoped control retaining its explicit native override after the semantic mapping disappears.
- The scoped case also asserts that the seed's baseline differs from the application accent, preventing a stock-color restoration from passing accidentally. Strengthening this test exposed a second real failure: the scoped orange baseline was `#FFFFB95C`, while the restored brush used the application's blue `#FFBEC2FF` (full run: 562 passed, 1 failed, 1 skipped). This requires the scope's current accent closure/native overrides above the ambient fallback; a matching app and scoped seed had masked it in the first run.
- An initial capture implementation enumerated the entire XCR graph and exposed lazy materialization changing the enumerated dictionary. It was replaced with native-key lookups; this avoids materializing unrelated resources and the enumeration failure.

### Generated semantic mapping shadowed an explicitly overridden native resource

When the same override dictionary defined `FilledButtonBackground` and `AccentButtonBackground`, the generated native mapping replaced the latter with the semantic value. This differed from the accent closure's explicit-native precedence and ignored the consumer's direct choice for the native control key.

`ApplyRepointing` now checks the consumer's native key before writing a mapped fallback. The semantic resource itself retains the semantic override.

- Red: `When_NativeAndSemanticLightweightKeysAreOverridden_ExplicitNativeResourceWins` expected the native blue brush but resolved the semantic red brush (`uno-themes-lifecycle-red2.xml`, run by the main agent).
- That run passed the other 14 lifecycle cases, including the initial clear regressions. The final desktop run after the precedence/scoped fixes and stronger baseline assertions passed **570 Simple cases**, with **0 failures** and **1 pre-existing skip**. The Fluent sample suite passed **2/2**, and Material passed **63/63** (run by the main agent).

## Other review conclusions

- The explicit consumer resolver's own-entry, reverse-merged, then selected-theme precedence agrees with the inspected Uno implementation. Exact appearance dictionaries suppress the `Default` fallback when present.
- The semantic color constants extraction preserves the existing strings and adds no consumer-facing API break.
- The weak system-accent observer does not strongly capture its theme; the existing lifecycle test keeps the publisher alive while checking theme collection. The forced-GC helper is now explicitly documented as a leak guard.
- No additional confirmed security, API/resource-key compatibility, or package-configuration defect was found in this library scope.
- Combining the `DefaultFontFamily` property with a root-only font dictionary override follows the base library's concrete-slot generation behavior; it was not classified as a Fluent-specific regression.

## Remaining validation limits

- Native platform fallback lookups use the active resource context and are stored as a flat dictionary. Public native lookup does not let this adapter force XCR aliases into a different appearance. Scoped consumer native resources and supported accent aliases have a separate explicit-appearance fallback layer; stock neutral values remain ambient. Rendered ambient tests and produced-graph tests remain distinct, as documented in `specs/lessons.md`.
- **Unresolved finding — High Contrast:** this existing recorded gap (`specs/05-fluent-theme/progress.md`, section B.3) remains: Fluent semantic palettes/lightweight defaults have only Light/Default branches. Semantic resources can retain dark-theme colors alongside system-colored native controls. This review did not add a new accessibility resource architecture.
- Native Windows/WinUI and OS High Contrast behavior were not exercised by this review. Desktop Skia and WebAssembly validation is coordinated and reported by the main agent.
- The retained-brush updater mutates `SolidColorBrush` instances. Runtime checks here cover solid native baselines; clearing a solid border override back to a native gradient (or changing brush types on an already realized control) has not been rendered and verified. The updater cannot change an existing brush object's runtime type, so these transitions must not be represented as covered by the solid-fill regressions.

## Verification ownership

The main agent runs builds and runtime tests to avoid concurrent builds in the shared checkout. Final desktop validation is green as reported above. Exact commands, WebAssembly/hosting results, and remaining failures belong in this review's `progress.md`; this document does not claim unrun checks passed.
