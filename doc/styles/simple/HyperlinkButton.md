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
`SimpleSecondaryHyperlinkButtonStyle` consumes its own `SecondaryHyperlinkButtonForeground*` keys for normal, pointer-over, pressed, and disabled states. Each foreground applies to both content and underline without changing primary links in the same resource scope.

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
