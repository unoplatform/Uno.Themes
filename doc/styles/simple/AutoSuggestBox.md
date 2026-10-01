---
uid: Uno.Themes.Simple.Styles.AutoSuggestBox
---

# AutoSuggestBox Control

<!-- BEGIN GENERATED -->
<!-- This region is generated from the Styles XAML by `dotnet run build/scripts/GenerateStyleDocs.cs`; do not edit it by hand. -->

## Styles

| Style Key                          | Semantic Alias | IsDefaultStyle\* |
| ---------------------------------- | -------------- | ---------------- |
| `SimpleAutoSuggestBoxStyle`        |                | True             |
| `SimpleDefaultAutoSuggestBoxStyle` |                | True             |

IsDefaultStyle\*: Styles in this column will be set as the default implicit style for the matching control

## Lightweight Styling

### Themed

| Key                                              | Type           | Light                            |
| ------------------------------------------------ | -------------- | -------------------------------- |
| `SimpleAutoSuggestBoxBorderThickness`            | `Thickness`    | `SimpleStrokeBorderThickness`    |
| `SimpleAutoSuggestBoxCornerRadius`               | `CornerRadius` | 20                               |
| `SimpleAutoSuggestBoxFocusedBorderThickness`     | `Thickness`    | `SimpleStrokeFocusRingThickness` |
| `SimpleAutoSuggestBoxIconFontSize`               | `Double`       | `BodyLargeFontSize`              |
| `SimpleAutoSuggestBoxMinHeight`                  | `Double`       | `SimpleIconLarge`                |
| `SimpleAutoSuggestBoxPadding`                    | `Thickness`    | 16,10,40,10                      |
| `SimpleAutoSuggestBoxSuggestionsBorderThickness` | `Thickness`    | `SimpleStrokeBorderThickness`    |
| `SimpleAutoSuggestBoxSuggestionsCornerRadius`    | `CornerRadius` | `SimpleRadius200CornerRadius`    |
| `SimpleAutoSuggestBoxSuggestionsPadding`         | `Thickness`    | `SimpleSpace100Thickness`        |

The Dark theme uses the same values as Light.

<!-- END GENERATED -->

> [!NOTE]
> The AutoSuggestBox control does not expose overridable themed brush resource keys. Brushes are applied directly via shared semantic resources (e.g. `OnSurfaceBrush`, `SurfaceBrush`, `OutlineBrush`, `PrimaryMediumBrush`) in the style templates and visual states.
