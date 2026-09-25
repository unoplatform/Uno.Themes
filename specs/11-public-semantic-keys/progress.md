# Public semantic resource keys

> Numbered 11 because `specs/10-…` is already claimed by three branches
> (`10-gettheme-nested-walk`, `10-seed-color-page`, `10-swa-deploy-hygiene`).

No issue yet — opened as a draft PR on request.

## Goal

Let consumers enumerate the semantic resource keys the shared Uno.Themes layer declares or
generates — colors, opacities, brushes, typography, spacing, shape and density — from code, without
hard-coding their own copy. The motivating consumer is a designer (Hot Design) that lists and edits
design tokens; it currently has no way to ask the theme library which keys exist.

## Why the existing surface was not enough

- **`ThemesConstants` is internal.** Making it public was considered and rejected:
  - it only covers colors and font families; font sizes, weights and character spacing live only in
    `SharedTypography.xaml`, brush keys are composed at runtime, and the spacing / shape / density keys
    are built by string templates from private tables in `BaseTheme.ScaleGeneration.cs`;
  - its `PackageName` and resource-path fields are mutable `public static string` fields, and its
    arrays are writable — a consumer editing `SemanticColorKeys` would break `SemanticBrushUpdater`,
    which iterates it on every brush rebuild;
  - its implementation-facing members (`ThemeDictionaryKeys`, `BrushThemeSources`) would become
    public API to keep stable.
- **The C# Markup `Theme.*` classes** expose individual keys as properties per control, in a separate
  package; there is nothing to iterate.

## Design

A new public static class, `Uno.Themes.SemanticResourceKeys`, with one `IReadOnlyList<string>`
per family:

| Property            | Keys | Source of truth                                                          |
|---------------------|-----:|--------------------------------------------------------------------------|
| `Colors`            |   33 | `ThemesConstants.SemanticColorKeys` + `ShadowColor`                       |
| `Opacities`         |    8 | `ThemesConstants.BrushStateSuffixes` + `Opacity`                          |
| `Brushes`           |  280 | color roles × `BrushStateSuffixes` + `Brush`; `SurfaceTint` is base-only  |
| `FontFamilies`      |   20 | `DefaultFontFamily` + `ThemesConstants.TypeScaleSlots` + `FontFamily`     |
| `FontSizes`         |   19 | `TypeScaleSlots` + `FontSize`                                             |
| `FontWeights`       |   19 | `TypeScaleSlots` + `FontWeight`                                           |
| `CharacterSpacings` |   11 | `ThemesConstants.CharacterSpacingSlots` + `CharacterSpacing`              |
| `Spacing`           |   88 | `DesignTokenScales.SpacingTokens` — the tokens the spacing generator writes |
| `Shape`             |   18 | `DesignTokenScales.ShapeTokens` — the tokens the shape generator writes   |
| `ControlSizes`      |    8 | `DesignTokenScales.DensityTokens` — the fixed density defaults            |

Decisions:

- **Derived, never copied.** Every list is built once, at type initialization, from the same tables
  the library itself reads. The generated scales share one token enumerator with their generator
  (`DesignTokenScales.SpacingTokens` / `ShapeTokens` / `DensityTokens` yield `(Key, Value)` pairs,
  and the three `Generate*` methods write exactly those), so a new scale step reaches the public
  list and the generated dictionary together.
- **The tables live outside `BaseTheme`.** `DesignTokenScales` is a plain internal static class, so
  reading a key list never runs `BaseTheme`'s static initializer, which registers dependency
  properties.
- **`ControlSizes`, not `Density`.** The control heights, icon sizes and touch target are exactly the
  tokens that do *not* change with the density mode (the density mode scales `Spacing`), and a
  `Density` property would collide with the public `Uno.Themes.Density` enum.
- **Sizes are pinned.** Because the generated lists are compared with the generator they are built
  from, a removed scale step would not fail that comparison. A separate test pins each list's size,
  so any change to the public surface is a reviewed test edit.
- **Typography slots are listed once.** `TypeScaleSlots` (the 19 `{Role}{Size}` names) replaces the
  literal `TypefaceScaleKeys` array, which is now derived from it. Only 11 of the 19 slots declare a
  `CharacterSpacing`, so that subset is its own list.
- **`ShadowColor` is included in `Colors`.** It is a semantic palette key (seed-generated, overridable)
  but has no brush, which is why the internal `SemanticColorKeys` — the brush roles — omits it. The
  public list is "every semantic color", not "every brush role".
- **`SurfaceTintBrush` has no state variants** in `SharedColors.xaml`; `ThemesConstants.BaseBrushOnlyColorKeys`
  records that so `Brushes` lists only declared keys.
- **Read-only at runtime, not just by type.** Each list is a `ReadOnlyCollection<string>` over its own
  copy, so casting it to `IList<string>` and writing throws, and no internal array is shared with it.
  Key order is not part of the contract.
- **Scope.** The keys are the shared layer's, which `SimpleTheme` and Material's version 2 styles
  merge. The runtime tests assert resolution under `SimpleTheme` only (the Simple sample is the CI
  host). Material v1 uses its own palette (for example `MaterialShadowColor`), and Cupertino is not a
  `BaseTheme`.
- **Limit of the shape.** Plain key strings cannot carry per-key metadata (value type, "scales with
  density", which design systems declare it). If a designer needs that later, it is a separate
  descriptor API alongside these lists.
- **Out of scope.** Design-system prefixed keys (`SimpleButtonFontFamily`, …) and semantic *style*
  keys (`FilledButtonStyle`, …). A style-key list is a separate question, because not every design
  system provides every style.
- **`ThemesConstants` stays internal**; nothing in it changes shape except the typography slot split.

## Requirements

```gherkin
Scenario: Every shared XAML key is listed
  Given the shared color palette, brush and typography dictionaries
  When their declared keys are compared with the public lists
  Then each list holds exactly the keys its dictionary declares, under every theme dictionary

Scenario: Every generated scale key is listed
  Given a SimpleTheme
  When its generated spacing, shape and density dictionaries are inspected
  Then each list holds exactly the keys the generator wrote, under Light and Default

Scenario: Every listed key resolves
  Given a container with a SimpleTheme merged
  When each key of each list is looked up
  Then the lookup succeeds

Scenario: The public surface does not change silently
  Given the published size of each list
  When a list gains or loses a key
  Then a test fails until the published size is updated

Scenario: Consumers cannot corrupt the lists
  Given any public key list
  When a consumer casts it to IList<string> and writes to it
  Then the write throws NotSupportedException
```

## Plan

- [x] Spec (this file).
- [x] Runtime tests `Given_SemanticResourceKeys` (red: type does not exist).
- [x] `TypeScaleSlots` / `CharacterSpacingSlots` / `BaseBrushOnlyColorKeys` / `ShadowColorKey` in `ThemesConstants`.
- [x] Token enumerators in `DesignTokenScales`; generators use them.
- [x] `SemanticResourceKeys`.
- [x] `doc/design-tokens.md` section.
- [x] Release build — no new warnings; `dotnet format` gate (no XAML touched); full runtime-test run.

## Review

Verified on Linux, Skia desktop, Release:

- Build: 0 errors, same warning set as `master`.
- Full runtime suite: all existing tests still pass (the `master` baseline was 256 passed, 1 skipped;
  the same hot-reload leak test is skipped here). The `Given_DesignTokens` value tests pass unchanged,
  which shows the generator refactor writes the same values.
- Mutation check: dropping `ShadowColor` and one character-spacing slot failed 6 cases, each naming
  the missing key.
- Not built locally: Android, iOS, Windows. The new code uses only types the scale generators already
  used there.

Review (contract + skeptic lenses) findings folded in: `ControlSizes` rename, size-pinning test,
narrowed Material wording, copied `FontFamilies` backing array, clearer generated-dictionary
assertion, static-initializer ordering comment, regenerated-keys note in the doc.

Not changed here: `SemanticBrushUpdater` still looks up the 8 `SurfaceTint` state brushes that do not
exist, on every rebuild. That predates this change, and a miss is harmless (`TryGetValue` fails and
the loop continues). Skipping them would change the hot loop's per-state indexing, which is its own
change.
