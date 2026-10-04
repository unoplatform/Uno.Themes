# SemanticResources marker dictionary (issue #1728)

Implements https://github.com/unoplatform/Uno.Themes/issues/1728.

## Design

Semantic (theme-agnostic) keys and theme-prefixed keys used to share the same dictionaries, so a
tool could only guess from the name. The signal is now the dictionary type: `Uno.Themes.SemanticResources`
(an empty `ResourceDictionary` subclass).

Contract: a key is semantic when it is declared in a `SemanticResources` dictionary **or in one of its
`ThemeDictionaries`**. The theme-dictionary clause is required, not a convenience: loading a file through
`Source` copies its theme dictionaries in as plain `ResourceDictionary` instances, and that is where every
theme-aware colour, brush and type token lives. A walker already knows when it descends into a theme
dictionary, so it carries the flag down.

Where the type is applied:

- Style aliases: moved out of `_Resources.xaml` (Material v2, Simple) into `SemanticStyles.xaml`. The file is
  removed from the XamlMerge glob - flattened into `mergedpages.xaml` its entries would land on the theme's own
  dictionary and lose the type - and merged by `BaseTheme` right after `Source`, through the new
  `protected virtual string SemanticStylesSource`. `MaterialResourcesV2` (deprecated, still ships) merges it too.
- Generated spacing / shape / density / typeface layers: outer and per-theme inner dictionaries are `SemanticResources`.
- `SharedColorPalette.xaml` and `SharedColors.xaml`: loaded as `SemanticResources` in `BuildColorLayer`.
- `SharedTypography.xaml`: removed from both `BaseDictionaries.xaml`; `BaseTheme`, `MaterialResourcesV1` and
  `MaterialResourcesV2` insert it typed at index 0 of `MergedDictionaries` (searched last, the position it had). Declaring
  `<themes:SemanticResources Source=.../>` in XAML does not work: the Uno XAML generator emits a plain `ResourceDictionary`
  for a Source-merged entry regardless of the declared type (verified with a runtime probe).
- Generated typeface layer: the type-scale keys are typed; the per-control `FontFamilyAliasKeys` (`SimpleButtonFontFamily`,
  ...) go to a separate plain layer so a theme-prefixed key never lands in a `SemanticResources`.
- Material v2 control files referenced three aliases from inside the bundle (`BodyLarge`, `TitleSmall`, `AppBarButtonStyle`);
  they now reference the `Material*` keys, so the bundle does not depend on the alias dictionary.
- Cupertino does not derive from `BaseTheme` and has no alias block; untouched.
- `DefaultMaterialCalendarViewStyle` is a Material-named legacy alias, so it stays in `_Resources.xaml`.

## Checklist

- [x] `SemanticResources` public type in `Uno.Themes`
- [x] `BaseTheme.SemanticStylesSource` + merge at construction
- [x] Material: `SemanticStyles.xaml`, glob removal, constant, override, `MaterialResourcesV2`
- [x] Simple: `SemanticStyles.xaml`, glob removal, constant, override
- [x] Generated scales and shared palette / brushes / typography typed
- [x] Runtime tests `Given_SemanticResources` (Simple + Material sample apps): membership, absence of prefixed keys, alias
      keys absent from plain dictionaries, `DefaultFontFamily` variant, aliases survive a hot-reload rebuild
- [x] Desktop build clean, runtime tests green: Simple 259 passed / 1 pre-existing `[Ignore]`, Material 66 passed

## Notes

- A `Source`-copied dictionary keeps its `ThemeDictionaries` children as `ResourceDictionary.LazyInitializer` until the
  indexer materializes them; enumerating `ThemeDictionaries` yields the wrapper, not a `ResourceDictionary`. A walker must
  index by key. Hot Design's `IterateMergedAndThemeDictionaries` filters on `td.Value is ResourceDictionary` while
  enumerating and would skip them (pre-existing; to raise on the Hot Design side).

- Native WinUI head: not runtime-verified (no sample app targets it, CI runs Skia desktop only). The pattern is not new:
  `BaseTheme` has loaded `SharedColors.xaml` standalone with `{StaticResource PrimaryColor}` references into a sibling
  dictionary since the colour-fidelity work, and Simple's `Thickness.xaml` aliases runtime-generated `Space*` keys. The
  Windows TFM compiles; runtime behaviour of alias entries in a Source-loaded dictionary is for the issue author to confirm.

- Local Debug builds of the sample heads need `-p:UnoDisableHotDesign=true -p:UnoDisableMCPSupport=true`: the SDK
  injects `Uno.UI.HotDesign` and `Uno.UI.App.Mcp` into non-optimized builds, and their current dev packages bring an
  incompatible `Uno.Toolkit` / a missing `SkiaSharp.Views.Windows`. CI publishes Release and never sees it.
