# Opacity & State Values: Material vs Fluent

This document details Material Design 3's systematic opacity values used for state overlays and emphasis levels. These values are applied consistently across all color brushes to create unified interaction feedback.

## Material Opacity System Overview

Material uses predefined opacity values for:

- **Interactive states**: Hover, Focused, Pressed, Dragged, Selected
- **Emphasis levels**: Medium, Low, Disabled
- **State layers**: Semi-transparent overlays that communicate interaction

All opacity values are defined as `x:Double` resources and applied uniformly across all color families.

**Source**: [SharedColorPalette.xaml](../../src/library/Uno.Themes/Styles/Applications/Common/SharedColorPalette.xaml#L126-L134)

## Opacity Token Reference

| Material Token | Value | Usage | WinUI/Fluent Equivalent |
| --- | --- | --- | --- |
| `HoverOpacity` | **0.08** (8%) | Pointer hover state layer | Control-specific or ~10% |
| `FocusedOpacity` | **0.12** (12%) | Keyboard focus state layer | Control-specific or ~15% |
| `PressedOpacity` | **0.12** (12%) | Pointer press state layer | Control-specific or ~15% |
| `DraggedOpacity` | **0.16** (16%) | Drag operation state layer | Control-specific |
| `SelectedOpacity` | **0.08** (8%) | Selected item state layer | Control-specific or ~10% |
| `MediumOpacity` | **0.64** (64%) | Medium emphasis content | `SystemControlForegroundBaseMediumBrush` |
| `LowOpacity` | **0.32** (32%) | Low emphasis content | `SystemControlForegroundBaseLowBrush` |
| `DisabledOpacity` | **0.12** (12%) | Disabled state | `ListViewItemDisabledThemeOpacity` (~0.4) |

## Interactive State Opacities

These opacities create state layers - semi-transparent overlays that provide visual feedback during interaction.

### Hover State

- **Value**: `0.08` (8%)
- **Purpose**: Indicates an element is interactive and under the pointer
- **Applied to**: All interactive surfaces (buttons, list items, etc.)
- **Fluent Equivalent**: ~10% overlay or lighter background variation

**Example Usage**:

```xml
<SolidColorBrush x:Key="PrimaryHoverBrush"
                 Color="{StaticResource PrimaryColor}"
                 Opacity="{StaticResource HoverOpacity}" />
```

### Focused State

- **Value**: `0.12` (12%)
- **Purpose**: Indicates keyboard focus for accessibility
- **Applied to**: Focused interactive elements
- **Fluent Equivalent**: ~15% overlay or focus indicator border

### Pressed State

- **Value**: `0.12` (12%)
- **Purpose**: Provides feedback during pointer press
- **Applied to**: Active press on buttons, toggles, etc.
- **Fluent Equivalent**: ~15% overlay or darker background

**Material Design Rationale**: Press and Focus use the same opacity to maintain visual consistency while the state layer architecture allows both to potentially stack if needed.

### Dragged State

- **Value**: `0.16` (16%)
- **Purpose**: Highest emphasis for drag operations
- **Applied to**: Elements being dragged (reordering, etc.)
- **Fluent Equivalent**: Custom per control, often ~20%

### Selected State

- **Value**: `0.08` (8%)
- **Purpose**: Subtle persistent indication of selection
- **Applied to**: Selected list items, tabs, etc.
- **Fluent Equivalent**: ~10% tint or different background

## Emphasis Level Opacities

These opacities establish content hierarchy through reduced emphasis.

### Medium Emphasis

- **Value**: `0.64` (64%)
- **Purpose**: Secondary content, supporting text
- **Applied to**: Subtitles, helper text, less important UI elements
- **Fluent Equivalent**: `SystemControlForegroundBaseMediumBrush` (similar concept)

**Example**: Secondary text in a list item, timestamps, metadata

### Low Emphasis

- **Value**: `0.32` (32%)
- **Purpose**: Tertiary content, placeholders, icons
- **Applied to**: Placeholder text, dividers, less critical icons
- **Fluent Equivalent**: `SystemControlForegroundBaseLowBrush` (similar concept)

**Example**: TextBox placeholder, subtle dividers

### Disabled Emphasis

- **Value**: `0.12` (12%)
- **Purpose**: Indicates non-interactive/unavailable state
- **Applied to**: Disabled buttons, text, controls
- **Fluent Equivalent**: `ListViewItemDisabledThemeOpacity` (~0.4, higher than Material)

**Material Philosophy**: Low opacity emphasizes unavailability. Fluent uses higher opacity (~40%) for better visibility of disabled states.

## State Layer Stacking

Material's state layer architecture allows overlays to stack on a base color:

### Single State

```xml
<!-- Base: PrimaryColor (100%) -->
<!-- Hover Layer: PrimaryColor @ 8% -->
<!-- Combined: ~108% intensity (additive) -->
```

### Multiple States (Uncommon)

```xml
<!-- Base: PrimaryColor (100%) -->
<!-- Hover Layer: PrimaryColor @ 8% -->
<!-- Focus Layer: PrimaryColor @ 12% -->
<!-- Combined: ~120% intensity (rare, usually one state active) -->
```

**Fluent Approach**: Single state background replacement rather than stacking:

```xml
<Button Background="PrimaryColor"
        PointerOverBackground="PrimaryColorLighter" />
```

## Brush Application Pattern

Material brushes combine colors with these opacity values:

```xml
<!-- Default: Full opacity -->
<SolidColorBrush x:Key="PrimaryBrush" Color="{StaticResource PrimaryColor}" />

<!-- Hover: With hover opacity -->
<SolidColorBrush x:Key="PrimaryHoverBrush"
                 Color="{StaticResource PrimaryColor}"
                 Opacity="{StaticResource HoverOpacity}" />

<!-- Disabled: With disabled opacity -->
<SolidColorBrush x:Key="PrimaryDisabledBrush"
                 Color="{StaticResource PrimaryColor}"
                 Opacity="{StaticResource DisabledOpacity}" />

<!-- Medium emphasis: With medium opacity -->
<SolidColorBrush x:Key="PrimaryMediumBrush"
                 Color="{StaticResource PrimaryColor}"
                 Opacity="{StaticResource MediumOpacity}" />
```

See [Brushes Reference](brushes.md) for complete brush listings.

## Opacity vs Alpha in Color

Material separates color definition from opacity application:

### Material Approach (Separation)

```xml
<!-- Define color once -->
<Color x:Key="PrimaryColor">#5946D2</Color>

<!-- Apply different opacities -->
<SolidColorBrush x:Key="PrimaryBrush" Color="{StaticResource PrimaryColor}" />
<SolidColorBrush x:Key="PrimaryHoverBrush"
                 Color="{StaticResource PrimaryColor}"
                 Opacity="0.08" />
```

**Benefits**:

- Single source of truth for color values
- Easy global opacity adjustments
- Opacity tokens reusable across all colors

### Alternative (Alpha in Color)

```xml
<!-- Color with alpha -->
<Color x:Key="PrimaryHoverColor">#145946D2</Color> <!-- 8% alpha -->
<SolidColorBrush x:Key="PrimaryHoverBrush" Color="{StaticResource PrimaryHoverColor}" />
```

**Drawback**: Changes to hover opacity require updating every color definition.

## Control-Specific Opacity Examples

### Button States

```xml
<!-- Normal: 100% -->
Background="{ThemeResource FilledButtonBackground}"

<!-- Hover: Base + 8% state layer -->
<Border Background="{ThemeResource OnPrimaryHoverBrush}" />

<!-- Pressed: Base + 12% state layer -->
<Border Background="{ThemeResource OnPrimaryPressedBrush}" />

<!-- Disabled: 12% of base -->
Background="{ThemeResource FilledButtonBackgroundDisabled}"
```

### Text Emphasis

```xml
<!-- Primary text: 100% -->
<TextBlock Foreground="{ThemeResource OnSurfaceBrush}" />

<!-- Secondary text: 64% -->
<TextBlock Foreground="{ThemeResource OnSurfaceMediumBrush}" />

<!-- Tertiary text: 32% -->
<TextBlock Foreground="{ThemeResource OnSurfaceLowBrush}" />

<!-- Disabled text: 12% -->
<TextBlock Foreground="{ThemeResource OnSurfaceDisabledBrush}" />
```

## Fluent Comparison

| Concept | Material | Fluent |
| --- | --- | --- |
| **Hover** | 8% overlay | Variable, often explicit lighter color |
| **Focus** | 12% overlay | Focus rectangle or 15% overlay |
| **Pressed** | 12% overlay | Variable, often 15-20% darker |
| **Disabled** | 12% opacity | ~40% opacity (more visible) |
| **Secondary Text** | 64% opacity | Named brush (BaseMedium) ~60% |
| **Tertiary Text** | 32% opacity | Named brush (BaseLow) ~40% |

**Key Difference**: Material's systematic opacity values vs. Fluent's control-specific or named brush approach.

## Customization

### Adjust All State Opacities

```xml
<Application.Resources>
    <ResourceDictionary>
        <ResourceDictionary.ThemeDictionaries>
            <ResourceDictionary x:Key="Default">
                <!-- More subtle hover -->
                <x:Double x:Key="HoverOpacity">0.05</x:Double>

                <!-- More visible disabled -->
                <x:Double x:Key="DisabledOpacity">0.38</x:Double>
            </ResourceDictionary>
        </ResourceDictionary.ThemeDictionaries>
    </ResourceDictionary>
</Application.Resources>
```

**Effect**: All brushes using these opacity tokens update automatically (360+ brushes).

### Accessibility Considerations

Material's low disabled opacity (12%) may not meet WCAG contrast requirements. Consider:

- Increasing `DisabledOpacity` to **0.38** (38%) for better visibility
- Using alternative disabled state indicators (borders, icons)
- Testing with screen readers and keyboard navigation

## Related Documentation

- [Brushes Reference](brushes.md) - Brushes that use these opacity values
- [Colors Reference](colors.md) - Base colors for brushes
- [Material Colors](../material-colors.md) - Complete color and brush documentation
- [Material Design: State Layers](https://m3.material.io/foundations/interaction/states/state-layers) - Official M3 specification
