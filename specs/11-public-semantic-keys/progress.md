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
| `Spacing`           |   88 | `BaseTheme.SpacingTokens` — the tokens the spacing generator writes       |
| `Shape`             |   18 | `BaseTheme.ShapeTokens` — the tokens the shape generator writes           |
| `Density`           |    8 | `BaseTheme.FixedDensityDefaults`                                          |

Decisions:

- **Derived, never copied.** Every list is built once, at type initialization, from the same tables
  the library itself reads. The generated scales share one token enumerator with their generator
  (`SpacingTokens` / `ShapeTokens` yield `(Key, Value)` pairs, and `GenerateSpacingScale` /
  `GenerateShapeScale` write exactly those), so a new scale step reaches the public list and the
  generated dictionary together.
- **Typography slots are listed once.** `TypeScaleSlots` (the 19 `{Role}{Size}` names) replaces the
  literal `TypefaceScaleKeys` array, which is now derived from it. Only 11 of the 19 slots declare a
  `CharacterSpacing`, so that subset is its own list.
- **`ShadowColor` is included in `Colors`.** It is a semantic palette key (seed-generated, overridable)
  but has no brush, which is why the internal `SemanticColorKeys` — the brush roles — omits it. The
  public list is "every semantic color", not "every brush role".
- **`SurfaceTintBrush` has no state variants** in `SharedColors.xaml`; `ThemesConstants.BaseBrushOnlyColorKeys`
  records that so `Brushes` lists only declared keys.
- **Read-only at runtime, not just by type.** Each list is a `ReadOnlyCollection<string>`, so casting
  it to `IList<string>` and writing throws; the internal arrays are never handed out.
- **Scope.** The keys are the shared layer's: `MaterialTheme` and `SimpleTheme` resolve all of them.
  Cupertino is not a `BaseTheme` and has no semantic type scale or generated scales. Design-system
  prefixed keys (`SimpleButtonFontFamily`, …) and semantic *style* keys (`FilledButtonStyle`, …) are
  out of scope — a style-key list is a separate question, because not every design system provides
  every style.
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

Scenario: Consumers cannot corrupt the lists
  Given any public key list
  When a consumer casts it to IList<string> and writes to it
  Then the write throws NotSupportedException
```

## Plan

- [x] Spec (this file).
- [ ] Runtime tests `Given_SemanticResourceKeys` (red: type does not exist).
- [ ] `TypeScaleSlots` / `CharacterSpacingSlots` / `BaseBrushOnlyColorKeys` / `ShadowColorKey` in `ThemesConstants`.
- [ ] Token enumerators in `BaseTheme.ScaleGeneration.cs`; generators use them.
- [ ] `SemanticResourceKeys`.
- [ ] `doc/design-tokens.md` section.
- [ ] Release build — no new warnings; `dotnet format` + XAML Styler gates; full runtime-test run.

## Review

_Filled in after verification._
