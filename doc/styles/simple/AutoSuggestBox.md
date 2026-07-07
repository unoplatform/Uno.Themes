---
uid: Uno.Themes.Simple.Styles.AutoSuggestBox
---

# AutoSuggestBox Control

## Styles

| Style Key                            | IsDefaultStyle\* |
|--------------------------------------|------------------|
| `SimpleAutoSuggestBoxStyle`          |                  |
| `SimpleDefaultAutoSuggestBoxStyle`   | True             |
| `SimpleAutoSuggestBoxTextBoxStyle`   |                  |

IsDefaultStyle\*: Styles in this column will be set as the default implicit style for the matching control

## Lightweight Styling

| Key                                                           | Type              | Value                                     |
|---------------------------------------------------------------|-------------------|-------------------------------------------|
| `SimpleAutoSuggestBoxCornerRadius`                            | `CornerRadius`    | 20                                        |
| `SimpleAutoSuggestBoxBorderThickness`                         | `StaticResource`  | `SimpleStrokeBorderThickness` (1)         |
| `SimpleAutoSuggestBoxFocusedBorderThickness`                  | `StaticResource`  | `SimpleStrokeFocusRingThickness` (2)      |
| `SimpleAutoSuggestBoxPadding`                                 | `Thickness`       | 16,10,40,10                               |
| `SimpleAutoSuggestBoxMinHeight`                               | `StaticResource`  | `SimpleIconLarge` (40)                    |
| `SimpleAutoSuggestBoxIconFontSize`                            | `StaticResource`  | `BodyLargeFontSize` (16)                  |
| `SimpleAutoSuggestBoxSuggestionsCornerRadius`                 | `StaticResource`  | `SimpleRadius200CornerRadius` (8)         |
| `SimpleAutoSuggestBoxSuggestionsBorderThickness`              | `StaticResource`  | `SimpleStrokeBorderThickness` (1)         |
| `SimpleAutoSuggestBoxSuggestionsPadding`                      | `StaticResource`  | `SimpleSpace100Thickness` (4)             |

> [!NOTE]
> The AutoSuggestBox control does not expose overridable themed brush resource keys. Brushes are applied directly via shared semantic resources (e.g. `OnSurfaceBrush`, `SurfaceBrush`, `OutlineBrush`, `PrimaryMediumBrush`) in the style templates and visual states.
