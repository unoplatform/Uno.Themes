---
uid: Uno.Themes.Simple.Styles.ToolTip
---

# ToolTip Control

<!-- BEGIN GENERATED -->
<!-- This region is generated from the Styles XAML by `dotnet run build/scripts/GenerateStyleDocs.cs`; do not edit it by hand. -->

## Styles

| Style Key            | Semantic Alias | IsDefaultStyle\* |
| -------------------- | -------------- | ---------------- |
| `SimpleToolTipStyle` |                | True             |

IsDefaultStyle\*: Styles in this column will be set as the default implicit style for the matching control

## Lightweight Styling

### Theme-agnostic

| Key                            | Type           | Value                         |
| ------------------------------ | -------------- | ----------------------------- |
| `SimpleToolTipBorderThickness` | `Thickness`    | `SimpleStrokeBorderThickness` |
| `SimpleToolTipCornerRadius`    | `CornerRadius` | `SimpleRadius200CornerRadius` |
| `SimpleToolTipPadding`         | `Thickness`    | 12,8,12,8                     |

<!-- END GENERATED -->

> [!NOTE]
> The ToolTip control does not expose overridable themed brush resource keys. Brushes are applied directly via shared semantic resources (`SurfaceBrush`, `OutlineBrush`, `OnSurfaceBrush`) in the style setters.
