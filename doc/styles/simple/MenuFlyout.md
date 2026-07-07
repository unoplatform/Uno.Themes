---
uid: Uno.Themes.Simple.Styles.MenuFlyout
---

# MenuFlyout Control

## Styles

| Style Key                                    | IsDefaultStyle\* |
|----------------------------------------------|------------------|
| `SimpleMenuFlyoutPresenterStyle`             |                  |
| `SimpleDefaultMenuFlyoutPresenterStyle`      | True             |
| `SimpleMenuFlyoutItemStyle`                  |                  |
| `SimpleDefaultMenuFlyoutItemStyle`           | True             |
| `SimpleToggleMenuFlyoutItemStyle`            |                  |
| `SimpleDefaultToggleMenuFlyoutItemStyle`     | True             |
| `SimpleMenuFlyoutSubItemStyle`               |                  |
| `SimpleDefaultMenuFlyoutSubItemStyle`        | True             |
| `SimpleMenuFlyoutSeparatorStyle`             |                  |
| `SimpleDefaultMenuFlyoutSeparatorStyle`      | True             |
| `SimpleRadioMenuFlyoutItemStyle`             |                  |
| `SimpleDefaultRadioMenuFlyoutItemStyle`      | True             |

IsDefaultStyle\*: Styles in this column will be set as the default implicit style for the matching control

## Lightweight Styling

### Themed Resources (ThemeDictionaries)

| Key                              | Type              | Value       |
|----------------------------------|-------------------|-------------|
| `SimpleMenuFlyoutItemBackground` | `SolidColorBrush` | Transparent |

### Theme-Agnostic Resources

| Key                                     | Type        | Value    |
|-----------------------------------------|-------------|----------|
| `SimpleMenuFlyoutPresenterMinWidth`     | `Double`    | 112      |
| `SimpleMenuFlyoutPresenterMaxWidth`     | `Double`    | 320      |
| `SimpleMenuFlyoutItemPadding`           | `Thickness` | 12,8     |
| `SimpleMenuFlyoutItemIconMargin`        | `Thickness` | 0,0,16,0 |
| `SimpleMenuFlyoutItemAcceleratorMargin` | `Thickness` | 16,0,0,0 |
| `SimpleMenuFlyoutCheckGlyphMargin`      | `Thickness` | 0,0,8,0  |
| `SimpleMenuFlyoutSeparatorPadding`      | `Thickness` | 16,4     |

> [!NOTE]
> The MenuFlyout styles reference shared theme brushes directly in setters and visual states (e.g., `SurfaceBrush`, `OutlineBrush`, `OnSurfaceBrush`, `OnSurfaceMediumBrush`, `PrimaryBrush`, `OnPrimaryBrush`, `PrimaryVariantDarkBrush`, `OnSurfaceDisabledBrush`) rather than defining control-specific lightweight styling keys for each state. Only `SimpleMenuFlyoutItemBackground` is exposed as an overridable themed resource. The styles also reference `SimpleMenuFlyoutItemHeight` and `SimpleMenuFlyoutSeparatorHeight` via `{StaticResource}`, but these keys are not defined in the MenuFlyout XAML file itself -- they are expected to be provided by a shared resource dictionary.
