---
uid: Uno.Themes.Simple.Styles.HyperlinkButton
---

# HyperlinkButton Control

## Styles

| Style Key                              | IsDefaultStyle\* |
|----------------------------------------|------------------|
| `SimpleHyperlinkButtonStyle`           |                  |
| `SimpleSecondaryHyperlinkButtonStyle`  |                  |
| `SimpleDefaultHyperlinkButtonStyle`    | True             |

IsDefaultStyle\*: Styles in this column will be set as the default implicit style for the matching control

## Lightweight Styling

> [!NOTE]
> Current implementation limitation: `SimpleSecondaryHyperlinkButtonStyle` uses `SecondaryHyperlinkButtonForeground` only at rest. It inherits the primary template, so hover, pressed, and disabled appearances read `HyperlinkButtonForegroundPointerOver`, `HyperlinkButtonForegroundPressed`, and `HyperlinkButtonForegroundDisabled`. The three corresponding `SecondaryHyperlinkButtonForeground*` state keys listed below resolve as resources but do not currently affect the rendered link. Override the primary state keys to customize those states; this also affects primary links in the same resource scope.

### Primary Variant

| Key                                              | Type              | Value                              |
|--------------------------------------------------|-------------------|------------------------------------|
| `HyperlinkButtonForeground`                      | `SolidColorBrush` | `PrimaryBrush`                     |
| `HyperlinkButtonForegroundPointerOver`           | `SolidColorBrush` | `PrimaryBrush`                     |
| `HyperlinkButtonForegroundPressed`               | `SolidColorBrush` | `PrimaryBrush`                     |
| `HyperlinkButtonForegroundDisabled`              | `SolidColorBrush` | `OnSurfaceDisabledBrush`           |
| `HyperlinkButtonBackgroundPointerOver`           | `SolidColorBrush` | Transparent                        |
| `HyperlinkButtonBackgroundPressed`               | `SolidColorBrush` | Transparent                        |

### Secondary Variant

| Key                                                      | Type              | Value                              |
|----------------------------------------------------------|-------------------|------------------------------------|
| `SecondaryHyperlinkButtonForeground`                     | `SolidColorBrush` | `OnSurfaceMediumBrush`             |
| `SecondaryHyperlinkButtonForegroundPointerOver`          | `SolidColorBrush` | `OnSurfaceMediumBrush`             |
| `SecondaryHyperlinkButtonForegroundPressed`              | `SolidColorBrush` | `OnSurfaceMediumBrush`             |
| `SecondaryHyperlinkButtonForegroundDisabled`             | `SolidColorBrush` | `OnSurfaceDisabledBrush`           |
