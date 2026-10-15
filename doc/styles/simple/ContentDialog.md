---
uid: Uno.Themes.Simple.Styles.ContentDialog
---

# ContentDialog Control

<!-- BEGIN GENERATED -->
<!-- This region is generated from the Styles XAML by `dotnet run build/scripts/GenerateStyleDocs.cs`; do not edit it by hand. -->

## Styles

| Style Key                         | Semantic Alias       | IsDefaultStyle\* |
| --------------------------------- | -------------------- | ---------------- |
| `SimpleContentDialogStyle`        | `ContentDialogStyle` | True             |
| `SimpleDefaultContentDialogStyle` |                      | True             |

IsDefaultStyle\*: Styles in this column will be set as the default implicit style for the matching control

## Lightweight Styling

### Theme-agnostic

| Key                                              | Type           | Value                         |
| ------------------------------------------------ | -------------- | ----------------------------- |
| `SimpleContentDialogButtonSpacing`               | `GridLength`   | 8                             |
| `SimpleContentDialogCommandSpaceToContentMargin` | `Thickness`    | `Space300TopThickness`        |
| `SimpleContentDialogCornerRadius`                | `CornerRadius` | `SimpleRadius200CornerRadius` |
| `SimpleContentDialogEdgeMargin`                  | `Thickness`    | 0                             |
| `SimpleContentDialogMaxHeight`                   | `Double`       | 560                           |
| `SimpleContentDialogMaxWidth`                    | `Double`       | 560                           |
| `SimpleContentDialogMinHeight`                   | `Double`       | 132                           |
| `SimpleContentDialogMinWidth`                    | `Double`       | 288                           |
| `SimpleContentDialogPanelPadding`                | `Thickness`    | 32                            |
| `SimpleContentDialogTitleToContentMargin`        | `Thickness`    | `Space100BottomThickness`     |

<!-- END GENERATED -->

The Light and Dark ThemeDictionaries for ContentDialog are empty -- no control-specific themed brush keys are defined.

> [!NOTE]
> The ContentDialog style references shared theme brushes directly in setters and the template (e.g., `SurfaceBrush`, `OnSurfaceMediumBrush`, `OutlineBrush`, `OnSurfaceBrush`, `SimpleBackgroundUtilitiesScrimBrush`) rather than defining control-specific themed brush keys. The ThemeDictionaries are present but empty.
