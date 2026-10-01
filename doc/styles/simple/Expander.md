---
uid: Uno.Themes.Simple.Styles.Expander
---

# Expander Control

<!-- BEGIN GENERATED -->
<!-- This region is generated from the Styles XAML by `dotnet run build/scripts/GenerateStyleDocs.cs`; do not edit it by hand. -->

## Styles

| Style Key                    | Semantic Alias | IsDefaultStyle\* |
| ---------------------------- | -------------- | ---------------- |
| `SimpleExpanderStyle`        |                | True             |
| `SimpleDefaultExpanderStyle` |                | True             |

IsDefaultStyle\*: Styles in this column will be set as the default implicit style for the matching control

## Lightweight Styling

### Theme-agnostic

| Key                                      | Type           | Value                         |
| ---------------------------------------- | -------------- | ----------------------------- |
| `SimpleExpanderChevronButtonSize`        | `Double`       | `SimpleIconMedium`            |
| `SimpleExpanderChevronDownGlyph`         | `String`       | \uE70D                        |
| `SimpleExpanderChevronGlyphSize`         | `Double`       | `SimpleSpace300`              |
| `SimpleExpanderChevronMargin`            | `Thickness`    | 20,0,8,0                      |
| `SimpleExpanderContentBorderThickness`   | `Thickness`    | 1,0,1,1                       |
| `SimpleExpanderContentPadding`           | `Thickness`    | `SimpleSpace400Thickness`     |
| `SimpleExpanderContentUpBorderThickness` | `Thickness`    | 1,1,1,0                       |
| `SimpleExpanderCornerRadius`             | `CornerRadius` | `SimpleRadius200CornerRadius` |
| `SimpleExpanderHeaderBorderThickness`    | `Thickness`    | `SimpleStrokeBorderThickness` |
| `SimpleExpanderHeaderPadding`            | `Thickness`    | `Space400LeftThickness`       |
| `SimpleExpanderMinHeight`                | `Double`       | `SimpleSpace1200`             |

<!-- END GENERATED -->

> [!NOTE]
> The Expander control does not expose overridable themed brush resource keys. Brushes are applied directly via shared semantic resources (e.g. `SurfaceVariantBrush`, `OnSurfaceBrush`, `OutlineBrush`) in the style setters and visual states.
