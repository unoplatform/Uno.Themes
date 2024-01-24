---
uid: Uno.Themes.Styles.FloatingActionButton
---

# FloatingActionButton Control

## Styles

| Style Key                | IsDefaultStyle\* |
|--------------------------|------------------|
| `FabStyle`               | True             |
| `SurfaceFabStyle`        |                  |
| `SecondaryFabStyle`      |                  |
| `TertiaryFabStyle`       |                  |
| `SmallFabStyle`          |                  |
| `SurfaceSmallFabStyle`   |                  |
| `SecondarySmallFabStyle` |                  |
| `TertiarySmallFabStyle`  |                  |
| `LargeFabStyle`          |                  |
| `SurfaceLargeFabStyle`   |                  |
| `SecondaryLargeFabStyle` |                  |
| `TertiaryLargeFabStyle`  |                  |

IsDefaultStyle\*: Styles in this column will be set as the default implicit style for the matching control

## Lightweight Styling

| Key                                             | Type              | Value                              |
|-------------------------------------------------|-------------------|------------------------------------|
| `FabForeground`                                 | `SolidColorBrush` | `OnPrimaryContainerBrush`          |
| `FabForegroundPressed`                          | `SolidColorBrush` | `OnPrimaryContainerBrush`          |
| `FabForegroundPointerOver`                      | `SolidColorBrush` | `OnPrimaryContainerBrush`          |
| `FabForegroundDisabled`                         | `SolidColorBrush` | `OnSurfaceDisabledBrush`           |
| `FabBackground`                                 | `SolidColorBrush` | `PrimaryContainerBrush`            |
| `FabBackgroundPressed`                          | `SolidColorBrush` | `PrimaryContainerBrush`            |
| `FabBackgroundPointerOver`                      | `SolidColorBrush` | `PrimaryContainerBrush`            |
| `FabBackgroundDisabled`                         | `SolidColorBrush` | `SystemControlTransparentBrush`    |
| `FabStateOverlayBackground`                     | `SolidColorBrush` | `SystemControlTransparentBrush`    |
| `FabStateOverlayBackgroundPointerOver`          | `SolidColorBrush` | `OnPrimaryContainerHoverBrush`     |
| `FabStateOverlayBackgroundFocused`              | `SolidColorBrush` | `OnPrimaryContainerFocusedBrush`   |
| `FabStateOverlayBackgroundPressed`              | `SolidColorBrush` | `OnPrimaryContainerPressedBrush`   |
| `FabStateOverlayBackgroundDisabled`             | `SolidColorBrush` | `OnSurfaceDisabledLowBrush`        |
| `FabSurfaceForeground`                          | `SolidColorBrush` | `OnPrimaryContainerBrush`          |
| `FabSurfaceForegroundPressed`                   | `SolidColorBrush` | `OnPrimaryContainerBrush`          |
| `FabSurfaceForegroundPointerOver`               | `SolidColorBrush` | `OnPrimaryContainerBrush`          |
| `FabSurfaceForegroundDisabled`                  | `SolidColorBrush` | `OnSurfaceDisabledBrush`           |
| `FabSurfaceBackground`                          | `SolidColorBrush` | `SurfaceBrush`                     |
| `FabSurfaceBackgroundPressed`                   | `SolidColorBrush` | `SurfaceBrush`                     |
| `FabSurfaceBackgroundPointerOver`               | `SolidColorBrush` | `SurfaceBrush`                     |
| `FabSurfaceBackgroundDisabled`                  | `SolidColorBrush` | `SystemControlTransparentBrush`    |
| `FabSurfaceStateOverlayBackground`              | `SolidColorBrush` | `SystemControlTransparentBrush`    |
| `FabSurfaceStateOverlayBackgroundPointerOver`   | `SolidColorBrush` | `PrimaryHoverBrush`                |
| `FabSurfaceStateOverlayBackgroundFocused`       | `SolidColorBrush` | `PrimaryFocusedBrush`              |
| `FabSurfaceStateOverlayBackgroundPressed`       | `SolidColorBrush` | `PrimaryPressedBrush`              |
| `FabSurfaceStateOverlayBackgroundDisabled`      | `SolidColorBrush` | `OnSurfaceDisabledLowBrush`        |
| `FabSecondaryForeground`                        | `SolidColorBrush` | `OnSecondaryContainerBrush`        |
| `FabSecondaryForegroundDisabled`                | `SolidColorBrush` | `OnSurfaceDisabledBrush`           |
| `FabSecondaryForegroundPointerOver`             | `SolidColorBrush` | `OnSecondaryContainerBrush`        |
| `FabSecondaryForegroundPressed`                 | `SolidColorBrush` | `OnSecondaryContainerBrush`        |
| `FabSecondaryBackground`                        | `SolidColorBrush` | `SecondaryContainerBrush`          |
| `FabSecondaryBackgroundPointerOver`             | `SolidColorBrush` | `SecondaryContainerBrush`          |
| `FabSecondaryBackgroundPressed`                 | `SolidColorBrush` | `SecondaryContainerBrush`          |
| `FabSecondaryBackgroundDisabled`                | `SolidColorBrush` | `SystemControlTransparentBrush`    |
| `FabSecondaryStateOverlayBackground`            | `SolidColorBrush` | `SystemControlTransparentBrush`    |
| `FabSecondaryStateOverlayBackgroundPointerOver` | `SolidColorBrush` | `OnSecondaryContainerHoverBrush`   |
| `FabSecondaryStateOverlayBackgroundFocused`     | `SolidColorBrush` | `OnSecondaryContainerFocusedBrush` |
| `FabSecondaryStateOverlayBackgroundPressed`     | `SolidColorBrush` | `OnSecondaryContainerPressedBrush` |
| `FabSecondaryStateOverlayBackgroundDisabled`    | `SolidColorBrush` | `OnSurfaceDisabledLowBrush`        |
| `FabTertiaryForeground`                         | `SolidColorBrush` | `OnTertiaryContainerBrush`         |
| `FabTertiaryForegroundPointerOver`              | `SolidColorBrush` | `OnTertiaryContainerBrush`         |
| `FabTertiaryForegroundPressed`                  | `SolidColorBrush` | `OnTertiaryContainerBrush`         |
| `FabTertiaryForegroundDisabled`                 | `SolidColorBrush` | `OnSurfaceDisabledBrush`           |
| `FabTertiaryBackground`                         | `SolidColorBrush` | `TertiaryContainerBrush`           |
| `FabTertiaryBackgroundPointerOver`              | `SolidColorBrush` | `TertiaryContainerBrush`           |
| `FabTertiaryBackgroundPressed`                  | `SolidColorBrush` | `TertiaryContainerBrush`           |
| `FabTertiaryBackgroundDisabled`                 | `SolidColorBrush` | `SystemControlTransparentBrush`    |
| `FabTertiaryStateOverlayBackgroundPointerOver`  | `SolidColorBrush` | `SystemControlTransparentBrush`    |
| `FabTertiaryStateOverlayBackgroundFocused`      | `SolidColorBrush` | `SystemControlTransparentBrush`    |
| `FabTertiaryStateOverlayBackgroundPressed`      | `SolidColorBrush` | `SystemControlTransparentBrush`    |
| `FabTertiaryStateOverlayBackgroundDisabled`     | `SolidColorBrush` | `OnSurfaceDisabledLowBrush`        |
