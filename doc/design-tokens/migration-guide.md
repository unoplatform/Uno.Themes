# Migration Guide: Fluent to Material Design Tokens

This guide helps developers migrate from WinUI/Fluent Design System to Uno Material Design 3, focusing on design token translation and common patterns.

## Overview

| Aspect | Fluent/WinUI | Material M3 | Migration Challenge |
| --- | --- | --- | --- |
| **Philosophy** | System-centric, control-specific | Semantic, role-based | Mapping system names to roles |
| **Color System** | Accent + control-specific | 40+ semantic colors | More granular color roles |
| **Typography** | Named styles | 15-scale type system | Scale-based vs. style-based |
| **States** | Property-specific | Opacity-based overlays | Architectural difference |
| **Customization** | Override resources | Override colors/tokens | More token-driven |

## Quick Token Lookup

### Essential Color Mappings

| Your Fluent Code | Material Equivalent | Notes |
| --- | --- | --- |
| `SystemAccentColor` | `PrimaryColor` | Main brand color |
| `SystemAccentColorLight1` | `PrimaryContainerColor` | Lighter accent |
| `SystemAccentColorDark1` | `OnPrimaryContainerColor` | Darker accent/text |
| White on accent | `OnPrimaryColor` | Text/icons on Primary |
| `SystemAltLowColor` | `BackgroundColor` | App background |
| `SystemBaseLowColor` | `SurfaceColor` | Card/component surface |
| `SystemBaseHighColor` | `OnSurfaceColor` | Primary text |
| `SystemBaseMediumColor` | `OnSurfaceMediumBrush` | Secondary text |
| `SystemErrorTextColor` | `ErrorColor` | Error/destructive |
| `SystemControlForegroundBaseMediumLowBrush` | `OutlineBrush` | Borders |

### Typography Mappings

| Your Fluent Style | Material Token Group | Font Size |
| --- | --- | --- |
| `HeaderTextBlockStyle` | `DisplayMedium` | 45px |
| `TitleTextBlockStyle` | `HeadlineMedium` | 28px |
| `SubtitleTextBlockStyle` | `TitleLarge` | 22px |
| `BaseTextBlockStyle` | `TitleMedium` | 16px |
| `BodyTextBlockStyle` | `BodyLarge` or `BodyMedium` | 16px or 14px |
| `CaptionTextBlockStyle` | `LabelMedium` | 12px |
| Button text | `LabelLarge` | 14px |

### Opacity/State Mappings

| Fluent Concept | Material Token | Value |
| --- | --- | --- |
| Hover overlay (~10%) | `HoverOpacity` | 0.08 (8%) |
| Focus overlay (~15%) | `FocusedOpacity` | 0.12 (12%) |
| Pressed overlay | `PressedOpacity` | 0.12 (12%) |
| Disabled opacity (~40%) | `DisabledOpacity` | 0.12 (12%) ⚠️ Lower |
| Secondary text | `MediumOpacity` | 0.64 (64%) |
| Tertiary text | `LowOpacity` | 0.32 (32%) |

## Step-by-Step Migration

### 1. Add Material Theme

```xml
<!-- App.xaml -->
<Application.Resources>
    <ResourceDictionary>
        <ResourceDictionary.MergedDictionaries>
            <!-- Remove or comment out Fluent -->
            <!-- <XamlControlsResources xmlns="using:Microsoft.UI.Xaml.Controls" /> -->

            <!-- Add Material -->
            <MaterialTheme xmlns="using:Uno.Material" />
        </ResourceDictionary.MergedDictionaries>
    </ResourceDictionary>
</Application.Resources>
```

### 2. Customize Brand Colors

```xml
<Application.Resources>
    <ResourceDictionary>
        <ResourceDictionary.MergedDictionaries>
            <MaterialTheme xmlns="using:Uno.Material" />
        </ResourceDictionary.MergedDictionaries>

        <!-- Override Material colors -->
        <ResourceDictionary.ThemeDictionaries>
            <ResourceDictionary x:Key="Light">
                <!-- Your brand color -->
                <Color x:Key="PrimaryColor">#0078D4</Color> <!-- Fluent blue -->
            </ResourceDictionary>
            <ResourceDictionary x:Key="Default"> <!-- Dark theme -->
                <Color x:Key="PrimaryColor">#60CDFF</Color>
            </ResourceDictionary>
        </ResourceDictionary.ThemeDictionaries>
    </ResourceDictionary>
</Application.Resources>
```

### 3. Update Control References

#### Before (Fluent)

```xml
<Button Background="{ThemeResource SystemAccentBrush}"
        Foreground="White" />

<TextBlock Foreground="{ThemeResource SystemBaseHighColor}"
           Style="{ThemeResource BodyTextBlockStyle}" />
```

#### After (Material)

```xml
<!-- Material Filled Button (similar to accent button) -->
<Button Style="{StaticResource MaterialFilledButtonStyle}"
        Content="Action" />

<TextBlock Foreground="{ThemeResource OnSurfaceBrush}"
           FontFamily="{StaticResource BodyLargeFontFamily}"
           FontSize="{StaticResource BodyLargeFontSize}"
           FontWeight="{StaticResource BodyLargeFontWeight}" />

<!-- OR use lightweight styling -->
<TextBlock Style="{StaticResource MaterialBodyLargeTextBlockStyle}"
           Text="Body text" />
```

### 4. Migrate Custom Controls

#### Fluent Approach

```xml
<Border Background="{ThemeResource SystemControlBackgroundBaseLowBrush}"
        BorderBrush="{ThemeResource SystemControlForegroundBaseMediumLowBrush}"
        BorderThickness="1"
        CornerRadius="4">
    <TextBlock Text="Content"
               Foreground="{ThemeResource SystemControlForegroundBaseHighBrush}" />
</Border>
```

#### Material Approach

```xml
<Border Background="{ThemeResource SurfaceBrush}"
        BorderBrush="{ThemeResource OutlineBrush}"
        BorderThickness="1"
        CornerRadius="4">
    <TextBlock Text="Content"
               Foreground="{ThemeResource OnSurfaceBrush}" />
</Border>
```

## Common Migration Patterns

### Pattern 1: Accent Buttons

#### Pattern 1: Fluent Example

```xml
<Button Background="{ThemeResource SystemAccentBrush}"
        Foreground="{ThemeResource SystemControlForegroundChromeWhiteBrush}"
        Content="Action" />
```

#### Pattern 1: Material Example

```xml
<!-- Option 1: Use Material style -->
<Button Style="{StaticResource MaterialFilledButtonStyle}"
        Content="Action" />

<!-- Option 2: Manual theming -->
<Button Background="{ThemeResource PrimaryBrush}"
        Foreground="{ThemeResource OnPrimaryBrush}"
        Content="Action" />
```

**Material Styles**:

- `MaterialFilledButtonStyle` - Solid accent button (most common)
- `MaterialElevatedButtonStyle` - Elevated surface button
- `MaterialOutlinedButtonStyle` - Outlined button
- `MaterialTextButtonStyle` - Text-only button

### Pattern 2: Lists with Selection

#### Pattern 2: Fluent Example

```xml
<ListView>
    <ListView.ItemContainerStyle>
        <Style TargetType="ListViewItem">
            <Setter Property="Background" Value="Transparent" />
        </Style>
    </ListView.ItemContainerStyle>
</ListView>
```

#### Pattern 2: Material Example

```xml
<ListView>
    <!-- Material styles automatically applied -->
</ListView>
```

Material ListView automatically applies selection state with `PrimarySelectedBrush` (8% opacity overlay).

### Pattern 3: Cards/Surfaces

#### Pattern 3: Fluent Example

```xml
<Grid Background="{ThemeResource SystemControlBackgroundAltHighBrush}"
      CornerRadius="8">
    <!-- Card content -->
</Grid>
```

#### Pattern 3: Material Example

```xml
<Grid Background="{ThemeResource SurfaceBrush}"
      CornerRadius="12"
      xmlns:ut="using:Uno.Themes"
      ut:ControlExtensions.Elevation="1">
    <!-- Card content -->
</Grid>
```

**Material Elevation**: Use `ut:ControlExtensions.Elevation` for drop shadows (0-5 supported).

### Pattern 4: Form Fields

#### Pattern 4: Fluent Example

```xml
<TextBox PlaceholderText="Enter text"
         Header="Label" />
```

#### Pattern 4: Material Example

```xml
<!-- Outlined style (default) -->
<TextBox Style="{StaticResource MaterialOutlinedTextBoxStyle}"
         PlaceholderText="Enter text" />

<!-- Filled style -->
<TextBox Style="{StaticResource MaterialFilledTextBoxStyle}"
         PlaceholderText="Enter text" />
```

**Material TextBox**: Animated floating label, uses `OutlineBrush` for borders.

### Pattern 5: Icons and Icon Buttons

#### Pattern 5: Fluent Example

```xml
<Button>
    <FontIcon Glyph="&#xE710;" /> <!-- Calendar icon -->
</Button>
```

#### Pattern 5: Material Example

```xml
<Button Style="{StaticResource MaterialIconButtonStyle}">
    <FontIcon Glyph="&#xE787;" FontFamily="{StaticResource SymbolThemeFontFamily}" />
</Button>
```

**Material Icon Button**: Circular ripple, uses `OnSurfaceVariantBrush` for icons.

## Architectural Differences

### State Overlay Architecture

#### Fluent: Direct Property Changes

```xml
<Button Background="Primary"
        PointerOverBackground="PrimaryLight"
        PressedBackground="PrimaryDark" />
```

#### Material: State Layers

```xml
<Button Background="Primary">
    <!-- State layer overlays on hover/press -->
    <VisualStateManager.VisualStateGroups>
        <VisualStateGroup x:Name="CommonStates">
            <VisualState x:Name="PointerOver">
                <!-- Add 8% overlay -->
            </VisualState>
        </VisualStateGroup>
    </VisualStateManager.VisualStateGroups>
</Button>
```

**Implication**: Material buttons maintain base color, states overlay semi-transparent layers.

### "On" Color Concept

Material pairs every background color with a foreground "On" color for guaranteed contrast:

| Background | Foreground |
| --- | --- |
| `PrimaryColor` | `OnPrimaryColor` |
| `SurfaceColor` | `OnSurfaceColor` |
| `ErrorColor` | `OnErrorColor` |

**Migration Rule**: Whenever you set a Material background color, use its corresponding "On" color for text/icons.

```xml
<!-- ✅ Correct -->
<Border Background="{ThemeResource PrimaryBrush}">
    <TextBlock Foreground="{ThemeResource OnPrimaryBrush}" Text="Text" />
</Border>

<!-- ❌ Incorrect (may fail contrast) -->
<Border Background="{ThemeResource PrimaryBrush}">
    <TextBlock Foreground="{ThemeResource OnSurfaceBrush}" Text="Text" />
</Border>
```

## Breaking Changes & Gotchas

### 1. Lower Disabled Opacity

**Issue**: Material's `DisabledOpacity` (12%) is much lower than Fluent (~40%).

**Solution**: Override if accessibility is a concern:

```xml
<x:Double x:Key="DisabledOpacity">0.38</x:Double>
```

### 2. Missing System Styles

**Issue**: `HeaderTextBlockStyle`, `TitleTextBlockStyle` not defined in Material.

**Solution**: Use Material type scales or create compatibility shims:

```xml
<Style x:Key="HeaderTextBlockStyle" TargetType="TextBlock">
    <Setter Property="FontFamily" Value="{StaticResource DisplayMediumFontFamily}" />
    <Setter Property="FontSize" Value="{StaticResource DisplayMediumFontSize}" />
    <Setter Property="FontWeight" Value="{StaticResource DisplayMediumFontWeight}" />
</Style>
```

### 3. No Secondary Accent Color

**Issue**: Fluent has `SystemAccentColorLight1/2/3`, Material has distinct Secondary/Tertiary.

**Solution**: Map Fluent secondary colors to Material:

```xml
<Color x:Key="SecondaryColor">#SystemAccentColorLight1#</Color>
```

### 4. CornerRadius Styles

**Issue**: Material uses larger corner radii (buttons: 20px vs. Fluent's ~4px).

**Solution**: Override if you prefer sharper corners:

```xml
<CornerRadius x:Key="ButtonCornerRadius">4</CornerRadius>
```

### 5. Elevation System

**Issue**: Material has built-in elevation, Fluent uses `ThemeShadow`.

**Solution**: Use Material's `ControlExtensions.Elevation`:

```xml
xmlns:ut="using:Uno.Themes"
ut:ControlExtensions.Elevation="2"
```

## Tips for Smooth Migration

### Use Lightweight Styling

Material provides lightweight styling for cross-theme compatibility:

```xml
<!-- Works with Material, Cupertino, and Fluent -->
<Button Style="{StaticResource MaterialFilledButtonStyle}"
        Content="Button"
        Background="Red"          <!-- Lightweight override -->
        Foreground="White" />
```

See [Lightweight Styling](../lightweight-styling.md).

### Color Generation Tool

Use Material DSP (Design System Package) to generate complete color schemes:

```json
// colors.dsp.json
{
  "colors": {
    "primary": "#0078D4"
  }
}
```

See [Material DSP](../material-dsp.md) for tooling.

### Gradual Migration Strategy

1. **Phase 1**: Keep Fluent, add Material in isolated areas
2. **Phase 2**: Switch theme, fix breaking styles
3. **Phase 3**: Refactor to embrace Material semantics
4. **Phase 4**: Optimize using Material patterns (containers, state layers)

## Testing Checklist

- [ ] Light theme rendering
- [ ] Dark theme rendering
- [ ] High contrast mode (if applicable)
- [ ] Focus indicators (keyboard navigation)
- [ ] Hover states
- [ ] Disabled states
- [ ] Typography hierarchy
- [ ] Color contrast (WCAG AA minimum)
- [ ] Animation performance
- [ ] Theme switching

## Additional Resources

- [Material Getting Started](../material-getting-started.md)
- [Color Tokens Reference](colors.md)
- [Typography Tokens Reference](typography.md)
- [Material Design 3 Guidelines](https://m3.material.io/)
- [Control Styles Documentation](../material-controls-styles.md)

## Support

For migration issues, consult:

- [Uno Platform Documentation](https://platform.uno)
- [Uno Platform Discord](https://discord.gg/eBHZSKG)
- [GitHub Issues](https://github.com/unoplatform/Uno.Themes/issues)
