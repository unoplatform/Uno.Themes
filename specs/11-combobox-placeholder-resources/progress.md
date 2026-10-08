# Material ComboBox placeholder lightweight resources (issue #1739)

Implements https://github.com/unoplatform/Uno.Themes/issues/1739.

## Design

The Material v2 ComboBox floating label (`UpperPlaceholderElement`) hardcoded its `CompositeTransform`
(`ScaleX`/`ScaleY` 0.7, `TranslateY` -11) and its text style (`MaterialBodyLarge`), so tuning it meant copying the
template. Five additive lightweight keys now carry those values, with defaults equal to the former literals:

| Key | Type | Default | Used by |
|---|---|---|---|
| `ComboBoxUpperPlaceHolderScaleX` | `Double` | 0.7 | `UpperPlaceholderElement` transform |
| `ComboBoxUpperPlaceHolderScaleY` | `Double` | 0.7 | `UpperPlaceholderElement` transform |
| `ComboBoxUpperPlaceHolderTranslateY` | `Double` | -11 | `UpperPlaceholderElement` transform |
| `ComboBoxUpperPlaceHolderMargin` | `Thickness` | 0 | `UpperPlaceholderElement` |
| `ComboBoxPlaceholderTextStyle` | `Style` | `MaterialBodyLarge` | `PlaceholderElement` and `UpperPlaceholderElement` |

- Declared once at the root of Material v2 `SemanticStyles.xaml` (theme-invariant values, like `SmallThumbOffSize`), so
  they are `SemanticResources` and need no per-theme copies. The style key is a `StaticResource` alias, like the style
  aliases at the top of that file.
- The template reads them through `{ThemeResource}` so a local override (control/page resources) wins.
- The transform/margin keys keep the existing `PlaceHolder` casing of `ComboBoxUpperPlaceHolderForeground*`; the style
  key name `ComboBoxPlaceholderTextStyle` was chosen by the requester and covers both placeholders.
- Material v2 only: Simple's ComboBox has no placeholder, Cupertino has no floating label and no semantic layer,
  Fluent templates are WinUI's and not shipped here, Material v1 drives its label through converters.
- Not covered: the selected value's 5px offset (`NullToContentTranslateYConverter`) stays a converter literal (see Follow-ups).

## Checklist

- [x] Keys in `Styles/Controls/v2/SemanticStyles.xaml`
- [x] `ComboBox.xaml` template consumes them
- [x] C# Markup accessors (`Theme.ComboBox.Resources.Default.UpperPlaceHolder.*`, `PlaceholderTextStyle`)
- [x] `doc/styles/ComboBox.md`
- [x] Sample on `ComboBoxSamplePage` (M3)
- [x] Runtime tests `Given_ComboBox` (Material sample app; the Simple host does not reference Uno.Material) + semantic
      membership in Material `Given_SemanticResources`
- [x] Red run on the pre-change template, green after
- [x] Full Material suite green; formatting gates clean

## Known limitation (Uno)

On a runtime theme change, Uno re-resolves the `{ThemeResource}` on the `CompositeTransform` (a non-FrameworkElement)
from `Application.Resources` only, skipping the element's resource chain. Per-control/page overrides of the three
transform keys therefore revert to the defaults after a theme switch; App.xaml overrides survive. Margin and the text
style (FrameworkElement properties) keep local overrides in every scope. A `XamlReader.Load` template does not
reproduce it, only the compiled library template. Native WinUI not verified. Documented in `doc/styles/ComboBox.md`;
the tests assert what holds and name the gap. Upstream: https://github.com/unoplatform/uno/issues/24958.

## Follow-ups

- Uno: ThemeResource re-resolution on non-FrameworkElement objects in compiled templates (unoplatform/uno#24958).
  Once fixed, extend `When_ThemeChangesAfterLoad_Then_LocalMarginAndStyleAreKept` to the transform keys and drop the doc note.
- The selected value's 5px offset (`NullToContentTranslateYConverter`) is still fixed, so a moved/rescaled label cannot
  take the value with it. Likely the next request.

## Review

- Red: `When_LightweightResourcesOverridden` failed 4/4 rows on the pre-change template; default test passed.
- Green (desktop Skia, Debug): `Given_ComboBox` + `Given_SemanticResources` 22/22; full Material suite 95/95.
- XamlStyler, `dotnet format whitespace`, cspell and markdownlint clean on the changed files.
- Seven-agent review panel: no blockers; fixes applied (theme-switch coverage, all 5 keys in the semantic check, sample
  uses the style override, doc wording for Margin / style TargetType / ignored setters, this section).
