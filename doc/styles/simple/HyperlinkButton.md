---
uid: Uno.Themes.Simple.Styles.HyperlinkButton
---

# HyperlinkButton Control

> [!NOTE]
> Current implementation limitation: `SimpleSecondaryHyperlinkButtonStyle` uses `SecondaryHyperlinkButtonForeground` only at rest. It inherits the primary template, so hover, pressed, and disabled appearances read `HyperlinkButtonForegroundPointerOver`, `HyperlinkButtonForegroundPressed`, and `HyperlinkButtonForegroundDisabled`. The three corresponding `SecondaryHyperlinkButtonForeground*` state keys listed below resolve as resources but do not currently affect the rendered link. Override the primary state keys to customize those states; this also affects primary links in the same resource scope.

<!-- BEGIN GENERATED -->
<!-- This region is generated from the Styles XAML by `dotnet run build/scripts/GenerateStyleDocs.cs`; do not edit it by hand. -->

## Styles

| Style Key                             | Semantic Alias                  | IsDefaultStyle\* |
| ------------------------------------- | ------------------------------- | ---------------- |
| `SimpleHyperlinkButtonStyle`          | `HyperlinkButtonStyle`          | True             |
| `SimpleSecondaryHyperlinkButtonStyle` | `SecondaryHyperlinkButtonStyle` |                  |
| `SimpleDefaultHyperlinkButtonStyle`   |                                 | True             |

IsDefaultStyle\*: Styles in this column will be set as the default implicit style for the matching control

## Lightweight Styling

### Themed

| Key                                             | Type              | Light                    |
| ----------------------------------------------- | ----------------- | ------------------------ |
| `HyperlinkButtonBackgroundPointerOver`          | `SolidColorBrush` | Transparent              |
| `HyperlinkButtonBackgroundPressed`              | `SolidColorBrush` | Transparent              |
| `HyperlinkButtonForeground`                     | `SolidColorBrush` | `PrimaryBrush`           |
| `HyperlinkButtonForegroundDisabled`             | `SolidColorBrush` | `OnSurfaceDisabledBrush` |
| `HyperlinkButtonForegroundPointerOver`          | `SolidColorBrush` | `PrimaryBrush`           |
| `HyperlinkButtonForegroundPressed`              | `SolidColorBrush` | `PrimaryBrush`           |
| `SecondaryHyperlinkButtonForeground`            | `SolidColorBrush` | `OnSurfaceMediumBrush`   |
| `SecondaryHyperlinkButtonForegroundDisabled`    | `SolidColorBrush` | `OnSurfaceDisabledBrush` |
| `SecondaryHyperlinkButtonForegroundPointerOver` | `SolidColorBrush` | `OnSurfaceMediumBrush`   |
| `SecondaryHyperlinkButtonForegroundPressed`     | `SolidColorBrush` | `OnSurfaceMediumBrush`   |

The Dark theme uses the same values as Light.

<!-- END GENERATED -->
