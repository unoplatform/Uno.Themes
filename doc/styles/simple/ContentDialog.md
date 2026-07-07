---
uid: Uno.Themes.Simple.Styles.ContentDialog
---

# ContentDialog Control

## Styles

| Style Key                            | IsDefaultStyle\* |
|--------------------------------------|------------------|
| `SimpleContentDialogStyle`           |                  |
| `SimpleDefaultContentDialogStyle`    | True             |

IsDefaultStyle\*: Styles in this column will be set as the default implicit style for the matching control

## Lightweight Styling

### Themed Resources (ThemeDictionaries)

The Light and Dark ThemeDictionaries for ContentDialog are empty -- no control-specific themed brush keys are defined.

### Theme-Agnostic Resources

| Key                                              | Type              | Value                         |
|--------------------------------------------------|-------------------|-------------------------------|
| `SimpleContentDialogPanelPadding`                | `StaticResource`  | `SimpleSpace800Thickness`     |
| `SimpleContentDialogCornerRadius`                | `StaticResource`  | `SimpleRadius200CornerRadius` |
| `SimpleContentDialogMinWidth`                    | `Double`          | 288                           |
| `SimpleContentDialogMinHeight`                   | `Double`          | 132                           |
| `SimpleContentDialogMaxWidth`                    | `Double`          | 560                           |
| `SimpleContentDialogMaxHeight`                   | `Double`          | 560                           |
| `SimpleContentDialogButtonSpacing`               | `GridLength`      | 8                             |
| `SimpleContentDialogTitleToContentMargin`        | `Thickness`       | 0,0,0,8                       |
| `SimpleContentDialogCommandSpaceToContentMargin` | `Thickness`       | 0,24,0,0                      |

> [!NOTE]
> The ContentDialog style references shared theme brushes directly in setters and the template (e.g., `SurfaceBrush`, `OnSurfaceMediumBrush`, `OutlineBrush`, `OnSurfaceBrush`, `SimpleBackgroundUtilitiesScrimBrush`) rather than defining control-specific themed brush keys. The ThemeDictionaries are present but empty.
