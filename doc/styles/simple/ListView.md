---
uid: Uno.Themes.Simple.Styles.ListView
---

# ListView Control

## Styles

| Style Key                          | IsDefaultStyle\* |
|------------------------------------|------------------|
| `SimpleListViewStyle`              |                  |
| `SimpleDefaultListViewStyle`       | True             |
| `SimpleListViewItemStyle`          |                  |
| `SimpleDefaultListViewItemStyle`   | True             |

IsDefaultStyle\*: Styles in this column will be set as the default implicit style for the matching control

## Lightweight Styling

### ListView Container

| Key                                                  | Type              | Value                              |
|------------------------------------------------------|-------------------|------------------------------------|
| `ListViewBackground`                                 | `SolidColorBrush` | `SurfaceBrush`                     |
| `ListViewBorderBrush`                                | `SolidColorBrush` | `OutlineBrush`                     |

### ListViewItem — Normal

| Key                                                  | Type              | Value                              |
|------------------------------------------------------|-------------------|------------------------------------|
| `ListViewItemBackground`                             | `SolidColorBrush` | `SystemControlTransparentBrush`    |
| `ListViewItemForeground`                             | `SolidColorBrush` | `OnSurfaceBrush`                   |

### ListViewItem — PointerOver

| Key                                                  | Type              | Value                              |
|------------------------------------------------------|-------------------|------------------------------------|
| `ListViewItemBackgroundPointerOver`                  | `SolidColorBrush` | `SurfaceVariantBrush`              |
| `ListViewItemForegroundPointerOver`                  | `SolidColorBrush` | `OnSurfaceBrush`                   |

### ListViewItem — Pressed

| Key                                                  | Type              | Value                              |
|------------------------------------------------------|-------------------|------------------------------------|
| `ListViewItemBackgroundPressed`                      | `SolidColorBrush` | `OutlineBrush`                     |
| `ListViewItemForegroundPressed`                      | `SolidColorBrush` | `OnSurfaceBrush`                   |

### ListViewItem — Selected

| Key                                                  | Type              | Value                              |
|------------------------------------------------------|-------------------|------------------------------------|
| `ListViewItemBackgroundSelected`                     | `SolidColorBrush` | `PrimaryBrush`                     |
| `ListViewItemForegroundSelected`                     | `SolidColorBrush` | `OnPrimaryBrush`                   |

### ListViewItem — PointerOverSelected

| Key                                                  | Type              | Value                              |
|------------------------------------------------------|-------------------|------------------------------------|
| `ListViewItemBackgroundPointerOverSelected`          | `SolidColorBrush` | `PrimaryVariantDarkBrush`          |
| `ListViewItemForegroundPointerOverSelected`          | `SolidColorBrush` | `OnPrimaryBrush`                   |

### ListViewItem — PressedSelected

| Key                                                  | Type              | Value                              |
|------------------------------------------------------|-------------------|------------------------------------|
| `ListViewItemBackgroundPressedSelected`              | `SolidColorBrush` | `PrimaryVariantDarkBrush`          |
| `ListViewItemForegroundPressedSelected`              | `SolidColorBrush` | `OnPrimaryBrush`                   |

### ListViewItem — Disabled

| Key                                                  | Type              | Value                              |
|------------------------------------------------------|-------------------|------------------------------------|
| `ListViewItemForegroundDisabled`                     | `SolidColorBrush` | `OnSurfaceDisabledBrush`           |
