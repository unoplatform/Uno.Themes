# Brush Tokens: Material vs Fluent

This document maps Material Design 3 brush tokens to their WinUI/Fluent equivalents. Material generates 9 state-based brush variations for each color, creating ~360 total brushes.

## Material Brush System Overview

Material's brush system applies opacity-based state overlays to semantic colors:

- Each color token (e.g., `PrimaryColor`) generates 9 brush variations
- States: Default, Hover, Focused, Pressed, Dragged, Selected, Medium, Low, Disabled
- Opacity values are consistent across all colors for unified interaction behavior
- Brushes reference colors from [SharedColorPalette.xaml](../../src/library/Uno.Themes/Styles/Applications/Common/SharedColorPalette.xaml)

**Source**: [SharedColors.xaml](../../src/library/Uno.Themes/Styles/Applications/Common/SharedColors.xaml)

## Brush Naming Convention

**Pattern**: `{ColorRole}{State}Brush`

Examples:

- `PrimaryBrush` - Default Primary color brush
- `PrimaryHoverBrush` - Primary color with Hover opacity (0.08)
- `OnSurfaceDisabledBrush` - OnSurface color with Disabled opacity (0.12)

## State Suffixes and Opacity

| State Suffix | Opacity | Usage | Fluent Equivalent Pattern |
| ------------ | ------- | ----- | ------------------------- |
| *(none)* | 1.0 | Default state | `{Control}Foreground`, `{Control}Background` |
| `Hover` | 0.08 | Pointer over | `{Control}ForegroundPointerOver`, `{Control}BackgroundPointerOver` |
| `Focused` | 0.12 | Keyboard focus | `{Control}ForegroundFocused` |
| `Pressed` | 0.12 | Pointer pressed | `{Control}ForegroundPressed`, `{Control}BackgroundPressed` |
| `Dragged` | 0.16 | During drag | Custom per control |
| `Selected` | 0.08 | Selected state | `{Control}ForegroundSelected` |
| `Medium` | 0.64 | Medium emphasis | `SystemControlForegroundBaseMediumBrush` |
| `Low` | 0.32 | Low emphasis | `SystemControlForegroundBaseLowBrush` |
| `Disabled` | 0.12 | Disabled state | `{Control}ForegroundDisabled`, `{Control}BackgroundDisabled` |

See [Opacity & State Values](opacity-states.md) for detailed opacity documentation.

## Primary Color Brushes

| Material Brush | Opacity | WinUI/Fluent Equivalent | Notes |
| -------------- | ------- | ----------------------- | ----- |
| `PrimaryBrush` | 1.0 | `SystemAccentBrush` | Brand accent color |
| `PrimaryHoverBrush` | 0.08 | Custom overlay | State layer |
| `PrimaryFocusedBrush` | 0.12 | Custom overlay | Focus indicator |
| `PrimaryPressedBrush` | 0.12 | Custom overlay | Press state |
| `PrimaryDraggedBrush` | 0.16 | Custom overlay | Drag state |
| `PrimarySelectedBrush` | 0.08 | Custom overlay | Selection state |
| `PrimaryMediumBrush` | 0.64 | `SystemAccentBrushAlt2` (similar) | Medium emphasis |
| `PrimaryLowBrush` | 0.32 | `SystemAccentBrushAlt3` (similar) | Low emphasis |
| `PrimaryDisabledBrush` | 0.12 | `SystemControlDisabledBaseMediumLowBrush` | Disabled state |

## Primary Container Brushes

| Material Brush | Opacity | WinUI/Fluent Equivalent | Notes |
| -------------- | ------- | ----------------------- | ----- |
| `PrimaryContainerBrush` | 1.0 | `SystemAccentBrushAlt1` (similar) | Container background |
| `PrimaryContainerHoverBrush` | 0.08 | Custom overlay | Hover state layer |
| `PrimaryContainerFocusedBrush` | 0.12 | Custom overlay | Focus state layer |
| `PrimaryContainerPressedBrush` | 0.12 | Custom overlay | Press state layer |
| `PrimaryContainerDraggedBrush` | 0.16 | Custom overlay | Drag state layer |
| `PrimaryContainerSelectedBrush` | 0.08 | Custom overlay | Selection state layer |
| `PrimaryContainerMediumBrush` | 0.64 | Custom | Medium emphasis |
| `PrimaryContainerLowBrush` | 0.32 | Custom | Low emphasis |
| `PrimaryContainerDisabledBrush` | 0.12 | Custom | Disabled state |

## OnPrimary Brushes (Foreground on Primary)

| Material Brush | Opacity | WinUI/Fluent Equivalent | Notes |
| -------------- | ------- | ----------------------- | ----- |
| `OnPrimaryBrush` | 1.0 | `SystemControlForegroundChromeWhiteBrush` | Text/icons on Primary |
| `OnPrimaryHoverBrush` | 0.08 | Custom overlay | Hover state layer |
| `OnPrimaryFocusedBrush` | 0.12 | Custom overlay | Focus state layer |
| `OnPrimaryPressedBrush` | 0.12 | Custom overlay | Press state layer |
| `OnPrimaryDraggedBrush` | 0.16 | Custom overlay | Drag state layer |
| `OnPrimarySelectedBrush` | 0.08 | Custom overlay | Selection state layer |
| `OnPrimaryMediumBrush` | 0.64 | Custom | Medium emphasis |
| `OnPrimaryLowBrush` | 0.32 | Custom | Low emphasis |
| `OnPrimaryDisabledBrush` | 0.12 | `SystemControlDisabledChromeWhiteBrush` | Disabled text on Primary |

## Surface Color Brushes

| Material Brush | Opacity | WinUI/Fluent Equivalent | Notes |
| -------------- | ------- | ----------------------- | ----- |
| `SurfaceBrush` | 1.0 | `SystemControlBackgroundBaseLowBrush` | Component surfaces |
| `SurfaceHoverBrush` | 0.08 | Custom overlay | Hover effect |
| `SurfaceFocusedBrush` | 0.12 | Custom overlay | Focus effect |
| `SurfacePressedBrush` | 0.12 | Custom overlay | Press effect |
| `SurfaceDraggedBrush` | 0.16 | Custom overlay | Drag effect |
| `SurfaceSelectedBrush` | 0.08 | Custom overlay | Selection effect |
| `SurfaceMediumBrush` | 0.64 | `SystemControlBackgroundBaseMediumBrush` | Medium emphasis |
| `SurfaceLowBrush` | 0.32 | `SystemControlBackgroundBaseLowBrush` | Low emphasis |
| `SurfaceDisabledBrush` | 0.12 | `SystemControlDisabledBaseLowBrush` | Disabled surface |

## OnSurface Brushes (Primary Text/Icons)

| Material Brush | Opacity | WinUI/Fluent Equivalent | Notes |
| -------------- | ------- | ----------------------- | ----- |
| `OnSurfaceBrush` | 1.0 | `SystemControlForegroundBaseHighBrush` | Primary text/icons |
| `OnSurfaceHoverBrush` | 0.08 | Custom overlay | Hover state layer |
| `OnSurfaceFocusedBrush` | 0.12 | Custom overlay | Focus state layer |
| `OnSurfacePressedBrush` | 0.12 | Custom overlay | Press state layer |
| `OnSurfaceDraggedBrush` | 0.16 | Custom overlay | Drag state layer |
| `OnSurfaceSelectedBrush` | 0.08 | Custom overlay | Selection state layer |
| `OnSurfaceMediumBrush` | 0.64 | `SystemControlForegroundBaseMediumBrush` | Secondary text |
| `OnSurfaceLowBrush` | 0.32 | `SystemControlForegroundBaseLowBrush` | Tertiary text |
| `OnSurfaceDisabledBrush` | 0.12 | `SystemControlDisabledBaseMediumLowBrush` | Disabled text |

## Secondary Color Brushes

The same 9-state pattern applies to Secondary colors:

- `SecondaryBrush`, `SecondaryHoverBrush`, `SecondaryFocusedBrush`, etc.
- `SecondaryContainerBrush`, `SecondaryContainerHoverBrush`, etc.
- `OnSecondaryBrush`, `OnSecondaryHoverBrush`, etc.
- `OnSecondaryContainerBrush`, `OnSecondaryContainerHoverBrush`, etc.

**Fluent Equivalent**: Typically custom or secondary accent colors, no standard system secondary color.

## Tertiary Color Brushes

The same 9-state pattern applies to Tertiary colors:

- `TertiaryBrush`, `TertiaryHoverBrush`, `TertiaryFocusedBrush`, etc.
- `TertiaryContainerBrush`, `TertiaryContainerHoverBrush`, etc.
- `OnTertiaryBrush`, `OnTertiaryHoverBrush`, etc.
- `OnTertiaryContainerBrush`, `OnTertiaryContainerHoverBrush`, etc.

**Fluent Equivalent**: Custom colors, no standard tertiary in Fluent.

## Error Color Brushes

| Material Brush | Opacity | WinUI/Fluent Equivalent | Notes |
| -------------- | ------- | ----------------------- | ----- |
| `ErrorBrush` | 1.0 | `SystemErrorTextBrush` | Error/destructive |
| `ErrorHoverBrush` | 0.08 | Custom overlay | Hover state layer |
| `ErrorFocusedBrush` | 0.12 | Custom overlay | Focus state layer |
| `ErrorPressedBrush` | 0.12 | Custom overlay | Press state layer |
| `ErrorDisabledBrush` | 0.12 | Custom disabled error | Disabled error state |

Additional Error family brushes follow the same pattern:

- `ErrorContainerBrush` family
- `OnErrorBrush` family
- `OnErrorContainerBrush` family

## Background & Outline Brushes

| Material Brush | Opacity | WinUI/Fluent Equivalent | Notes |
| -------------- | ------- | ----------------------- | ----- |
| `BackgroundBrush` | 1.0 | `ApplicationPageBackgroundThemeBrush` | App background |
| `OnBackgroundBrush` | 1.0 | `SystemControlForegroundBaseHighBrush` | Text on background |
| `OutlineBrush` | 1.0 | `SystemControlForegroundBaseMediumLowBrush` | Borders |
| `OutlineDisabledBrush` | 0.12 | `SystemControlDisabledBaseMediumLowBrush` | Disabled borders |
| `OutlineVariantBrush` | 1.0 | `SystemControlForegroundBaseLowBrush` | Subtle borders |

## Complete Brush Matrix

Material generates these brush families (each with 9 state variations):

1. **Primary**: `Primary`, `OnPrimary`, `PrimaryContainer`, `OnPrimaryContainer`
2. **Secondary**: `Secondary`, `OnSecondary`, `SecondaryContainer`, `OnSecondaryContainer`
3. **Tertiary**: `Tertiary`, `OnTertiary`, `TertiaryContainer`, `OnTertiaryContainer`
4. **Error**: `Error`, `OnError`, `ErrorContainer`, `OnErrorContainer`
5. **Surface**: `Surface`, `OnSurface`, `SurfaceVariant`, `OnSurfaceVariant`
6. **Background**: `Background`, `OnBackground`
7. **Outline**: `Outline`, `OutlineVariant`
8. **Inverse**: `PrimaryInverse`, `SurfaceInverse`, `OnSurfaceInverse`
9. **Legacy**: `PrimaryVariantLight`, `PrimaryVariantDark`, `SecondaryVariantLight`, `SecondaryVariantDark`

**Total**: ~40 color families × 9 states = **~360 brushes**

## State Layer Architecture

Material's state layer approach differs from Fluent's direct state properties:

### Material Approach

```xml
<!-- Background remains constant -->
<Border Background="{ThemeResource PrimaryBrush}">
    <!-- State layer overlays on hover/press -->
    <Border Background="{ThemeResource OnPrimaryHoverBrush}"
            x:Name="StateLayer" />
</Border>
```

### Fluent Approach

```xml
<!-- Background changes per state -->
<Button Background="{ThemeResource ButtonBackground}"
        PointerOverBackground="{ThemeResource ButtonBackgroundPointerOver}"
        PressedBackground="{ThemeResource ButtonBackgroundPressed}" />
```

## Usage in Control Styles

Control-specific tokens reference these brushes:

```xml
<!-- Material Button -->
<StaticResource x:Key="FilledButtonBackground" ResourceKey="PrimaryBrush" />
<StaticResource x:Key="FilledButtonForeground" ResourceKey="OnPrimaryBrush" />
<StaticResource x:Key="FilledButtonStateLayerBackgroundPointerOver"
                ResourceKey="OnPrimaryHoverBrush" />
```

See [Controls Reference](controls/) for control-specific brush mappings.

## Customization

### Override Specific Brushes

```xml
<Application.Resources>
    <ResourceDictionary>
        <ResourceDictionary.ThemeDictionaries>
            <ResourceDictionary x:Key="Light">
                <!-- Override just the hover opacity -->
                <x:Double x:Key="HoverOpacity">0.12</x:Double>
            </ResourceDictionary>
        </ResourceDictionary.ThemeDictionaries>
    </ResourceDictionary>
</Application.Resources>
```

Changing opacity values affects all brushes using that state.

## Related Documentation

- [Colors Reference](colors.md) - Base colors that brushes derive from
- [Opacity & States](opacity-states.md) - Opacity values for each state
- [Material Colors](../material-colors.md) - Comprehensive brush listing with HEX values
