# PR 1730 semantic control resource fix

## Plan

- [x] Review P2: portable lightweight styling keys are declared in plain control dictionaries.
- [x] Add runtime regressions for semantic control resources and reproduce failure.
- [x] Preserve each resource value and lookup behavior while marking its declaring dictionary.
- [x] Build desktop and WebAssembly, run affected runtime suites, review the diff.

## Review

Base: PR head 91ab05d09e4fd95c46d59abb2ebf43927635ff0d. Work is isolated from the main checkout; no push is authorized.

## Implementation

The missing marker affects lightweight styling across control families, not just buttons. Move portable brush, scalar and layout declarations into each theme's existing `SemanticStyles.xaml`; leave prefixed aliases, control styles/templates, converters and internal transform constants in the control dictionaries. The shared Markup resource API provides the public inventory. No resource values or appearance scopes change, and no runtime key-name heuristic or second value registry is introduced.

## Verification so far

- Red: both new Light/Default membership cases fail on `FilledButtonBackground` before the library changes (`/tmp/pr1730-red.xml`).
- Structural audit: compare all keyed root/theme declarations against the PR head, including complete element trees, values and appearance scope; zero additions, removals or value changes.
- XAML Styler: all 40 changed XAML files pass the pinned formatter in verify mode.
- Simple desktop: 270 passed, 1 pre-existing skip, no failures with one attempt (`/tmp/pr1730-simple-verified.xml`).
- The new rendered checks cover filled-button background/foreground state values, scoped overrides and SliderThumbWidth under Light and Dark.
- Native WinUI runtime verification remains outside this P2 fix and is not claimed.

- Material desktop: 77 passed, no skips or failures, one attempt (`/tmp/pr1730-material-verified.xml`).
- C# whitespace verification passed for both changed runtime-test files; workspace loading emitted a warning.
- Desktop builds passed with existing warnings (Simple 83, Material 79); no diagnostics in the new tests.

## Final review

- Release desktop and browserwasm builds succeeded for Simple and Material. WebAssembly builds retained existing warnings (Simple 84, Material 81), with zero errors.
- Runtime regression was demonstrated red before the fix; final desktop suites pass with one attempt: Simple 270 passed / 1 existing skip; Material 77 passed. WebAssembly was build-validated, not runtime-tested.
- All changed XAML passed XAML Styler; changed C# passed whitespace verification; `git diff --check` passed.
- Independent review and structural comparison found no introduced defect or changed resource value.
- No packages, frameworks, public keys or resource values changed. Native WinUI runtime remains unverified.
- The fix is local on `fix/1730-semantic-control-resources`; no push or PR update was authorized.
