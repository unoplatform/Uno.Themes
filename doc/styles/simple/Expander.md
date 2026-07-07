---
uid: Uno.Themes.Simple.Styles.Expander
---

# Expander Control

## Styles

| Style Key                        | IsDefaultStyle\* |
|----------------------------------|------------------|
| `SimpleExpanderStyle`            |                  |
| `SimpleDefaultExpanderStyle`     | True             |

IsDefaultStyle\*: Styles in this column will be set as the default implicit style for the matching control

## Lightweight Styling

| Key                                          | Type              | Value                            |
|----------------------------------------------|-------------------|----------------------------------|
| `SimpleExpanderMinHeight`                    | `Double`          | `SimpleSpace1200` (48)                |
| `SimpleExpanderCornerRadius`                 | `CornerRadius`    | `SimpleRadius200CornerRadius` (8)    |
| `SimpleExpanderHeaderPadding`                | `Thickness`       | 16,0,0,0                         |
| `SimpleExpanderContentPadding`               | `Thickness`       | `SimpleSpace400Thickness` (16)        |
| `SimpleExpanderHeaderBorderThickness`        | `Thickness`       | `SimpleStrokeBorderThickness` (1)    |
| `SimpleExpanderContentBorderThickness`       | `Thickness`       | 1,0,1,1                          |
| `SimpleExpanderContentUpBorderThickness`     | `Thickness`       | 1,1,1,0                          |
| `SimpleExpanderChevronMargin`                | `Thickness`       | 20,0,8,0                         |
| `SimpleExpanderChevronButtonSize`            | `Double`          | `SimpleIconMedium` (32)               |
| `SimpleExpanderChevronGlyphSize`             | `Double`          | 12                                |
| `SimpleExpanderChevronDownGlyph`             | `String`          | \uE70D                            |

> [!NOTE]
> The Expander control does not expose overridable themed brush resource keys. Brushes are applied directly via shared semantic resources (e.g. `SurfaceVariantBrush`, `OnSurfaceBrush`, `OutlineBrush`) in the style setters and visual states.
