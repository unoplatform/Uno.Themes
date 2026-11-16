---
uid: Uno.Themes.Simple.Styles.MenuFlyout
---

# MenuFlyout Control

<!-- BEGIN GENERATED -->
<!-- This region is generated from the Styles XAML by `dotnet run build/scripts/GenerateStyleDocs.cs`; do not edit it by hand. -->

## Styles

| Style Key                                | Semantic Alias              | IsDefaultStyle\* |
| ---------------------------------------- | --------------------------- | ---------------- |
| `SimpleMenuFlyoutPresenterStyle`         | `MenuFlyoutPresenterStyle`  | True             |
| `SimpleMenuFlyoutItemStyle`              | `MenuFlyoutItemStyle`       | True             |
| `SimpleToggleMenuFlyoutItemStyle`        | `ToggleMenuFlyoutItemStyle` | True             |
| `SimpleMenuFlyoutSubItemStyle`           | `MenuFlyoutSubItemStyle`    | True             |
| `SimpleRadioMenuFlyoutItemStyle`         | `RadioMenuFlyoutItemStyle`  | True             |
| `SimpleMenuFlyoutSeparatorStyle`         | `MenuFlyoutSeparatorStyle`  | True             |
| `SimpleDefaultMenuFlyoutPresenterStyle`  |                             | True             |
| `SimpleDefaultMenuFlyoutItemStyle`       |                             | True             |
| `SimpleDefaultToggleMenuFlyoutItemStyle` |                             | True             |
| `SimpleDefaultMenuFlyoutSubItemStyle`    |                             | True             |
| `SimpleDefaultMenuFlyoutSeparatorStyle`  |                             | True             |
| `SimpleDefaultRadioMenuFlyoutItemStyle`  |                             | True             |

IsDefaultStyle\*: Styles in this column will be set as the default implicit style for the matching control

## Lightweight Styling

### Theme-agnostic

| Key                                     | Type        | Value                    |
| --------------------------------------- | ----------- | ------------------------ |
| `SimpleMenuFlyoutCheckGlyphMargin`      | `Thickness` | 0,0,8,0                  |
| `SimpleMenuFlyoutItemAcceleratorMargin` | `Thickness` | 16,0,0,0                 |
| `SimpleMenuFlyoutItemHeight`            | `Double`    | 36                       |
| `SimpleMenuFlyoutItemIconMargin`        | `Thickness` | `Space200RightThickness` |
| `SimpleMenuFlyoutItemPadding`           | `Thickness` | 16,12                    |
| `SimpleMenuFlyoutPresenterMaxWidth`     | `Double`    | 320                      |
| `SimpleMenuFlyoutPresenterMinWidth`     | `Double`    | 112                      |
| `SimpleMenuFlyoutSeparatorHeight`       | `Double`    | 1                        |
| `SimpleMenuFlyoutSeparatorPadding`      | `Thickness` | 16,4                     |

### Themed

| Key                              | Type              | Light       |
| -------------------------------- | ----------------- | ----------- |
| `SimpleMenuFlyoutItemBackground` | `SolidColorBrush` | Transparent |

The Dark theme uses the same values as Light.

<!-- END GENERATED -->

> [!NOTE]
> The MenuFlyout styles reference shared theme brushes directly in setters and visual states (e.g., `SurfaceBrush`, `OutlineBrush`, `OnSurfaceBrush`, `OnSurfaceMediumBrush`, `PrimaryBrush`, `OnPrimaryBrush`, `PrimaryVariantDarkBrush`, `OnSurfaceDisabledBrush`) rather than defining control-specific lightweight styling keys for each state. Only `SimpleMenuFlyoutItemBackground` is exposed as an overridable themed resource.
