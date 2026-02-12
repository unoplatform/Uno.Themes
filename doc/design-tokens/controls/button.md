# Button Control Tokens: Material vs Fluent

This document details all design tokens for Material Button styles and maps them to WinUI/Fluent Button equivalents.

## Material Button Styles Overview

Material Design 3 defines 5 button variants, each with distinct visual characteristics:

1. **Filled Button** (Primary action) - `MaterialFilledButtonStyle`
2. **Filled Tonal Button** (Secondary action) - `MaterialFilledTonalButtonStyle`
3. **Elevated Button** (Important but not primary) - `MaterialElevatedButtonStyle`
4. **Outlined Button** (Medium emphasis) - `MaterialOutlinedButtonStyle`
5. **Text Button** (Low emphasis) - `MaterialTextButtonStyle`
6. **Icon Button** (Icon-only) - `MaterialIconButtonStyle`

**Source**: [Button.xaml](../../src/library/Uno.Material/Styles/Controls/v2/Button.xaml)

## Common Dimension Tokens

| Material Token | Value | WinUI/Fluent Equivalent | Notes |
| --- | --- | --- | --- |
| `ButtonMinHeight` | **40** | `ButtonMinHeight` (32) | Taller for touch |
| `ButtonMinWidth` | **40** | `ButtonMinWidth` (120) | Square minimum |
| `ButtonIconMinWidth` | **18** | `FontIconFontSize` (16) | Icon size |
| `ButtonCornerRadius` | **20** | `ControlCornerRadius` (4) | Highly rounded |
| `ButtonPadding` | **16,0** | `ButtonPadding` (8,5,8,6) | Horizontal padding |
| `TextButtonPadding` | **12,0** | Similar | Text button specific |
| `ButtonBorderThickness` | **0** | `ButtonBorderThickness` (2) | No border default |
| `OutlinedButtonBorderThickness` | **1** | `ButtonBorderThickness` (2) | For outlined variant |
| `ButtonContentMargin` | **8,0** | Internal spacing | Icon-text gap |
| `ButtonMargin` | **0** | `ButtonMargin` (0) | External margin |
| `TextButtonIconMargin` | **0,0,8,0** | Similar | Icon in text button |

## Elevation Tokens

| Material Token | Value | WinUI/Fluent Equivalent | Notes |
| --- | --- | --- | --- |
| `ElevatedButtonElevation` | **1** | ThemeShadow or custom | Drop shadow depth |
| `ElevatedButtonElevationDisabled` | **0** | None | No shadow when disabled |
| `ButtonElevation` | **0** | None | Other buttons flat |
| `ElevatedButtonMargin` | **0,0,0,1** | Compensation for shadow | Extra bottom space |
| `ElevatedButtonDisabledMargin` | **0** | Normal margin | No shadow space |

Material uses `Uno.Themes.ControlExtensions.Elevation` attached property for drop shadows. Fluent uses `ThemeShadow` or `DropShadowPanel`.

## Icon Button Opacity Tokens

| Material Token | Value | Usage | Fluent Equivalent |
| --- | --- | --- | --- |
| `IconButtonOpacityVisibleState` | **1.0** | Visible state layer | N/A |
| `IconButtonOpacityHiddenState` | **0.0** | Hidden state layer | N/A |

## Filled Button Tokens

Primary action button with solid background.

### Filled Button Colors

| State | Foreground | Background | State Layer | Border |
| --- | --- | --- | --- | --- |
| **Normal** | `PrimaryBrush` | `PrimaryBrush` | None | Transparent |
| **Hover** | `OnPrimaryBrush` | `PrimaryBrush` | `OnPrimaryHoverBrush` (8%) | Transparent |
| **Pressed** | `OnPrimaryBrush` | `PrimaryBrush` | `OnPrimaryPressedBrush` (12%) | Transparent |
| **Disabled** | `OnSurfaceDisabledBrush` (12%) | `OnSurfaceDisabledBrush` (12%) | None | Transparent |

### Filled Button Fluent Equivalent

```xml
<!-- Fluent Accent Button (closest match) -->
<Button Style="{StaticResource AccentButtonStyle}" />

<!-- Manual equivalent -->
<Button Background="{ThemeResource SystemAccentBrush}"
        Foreground="White" />
```

**Key Differences**:

- Material: Background stays `Primary`, state layer overlays `OnPrimary` color
- Fluent: Background changes to lighter/darker variations directly

## Filled Tonal Button Tokens

Secondary action button with muted background.

### Tonal Button Colors

| State | Foreground | Background | State Layer | Border |
| --- | --- | --- | --- | --- |
| **Normal** | `OnSecondaryContainerBrush` | `SecondaryContainerBrush` | None | Transparent |
| **Hover** | `OnSecondaryContainerBrush` | `SecondaryContainerBrush` | `OnSecondaryContainerHoverBrush` (8%) | Transparent |
| **Pressed** | `OnSecondaryContainerBrush` | `SecondaryContainerBrush` | `OnSecondaryContainerPressedBrush` (12%) | Transparent |
| **Disabled** | `OnSurfaceDisabledBrush` (12%) | `OnSurfaceDisabledBrush` (12%) | None | Transparent |

### Tonal Button Fluent Equivalent

No direct equivalent. Closest is standard `Button` with custom colors:

```xml
<Button Background="{ThemeResource SystemControlBackgroundBaseLowBrush}"
        Foreground="{ThemeResource SystemControlForegroundBaseHighBrush}" />
```

## Elevated Button Tokens

Important action with elevation/shadow, no background fill.

### Elevated Button Colors

| State | Foreground | Background | State Layer | Border |
| --- | --- | --- | --- | --- |
| **Normal** | `PrimaryBrush` | `SurfaceBrush` | None | Transparent |
| **Hover** | `PrimaryBrush` | `SurfaceBrush` | `PrimaryHoverBrush` (8%) | Transparent |
| **Pressed** | `PrimaryBrush` | `SurfaceBrush` | `PrimaryPressedBrush` (12%) | Transparent |
| **Disabled** | `OnSurfaceDisabledBrush` (12%) | `OnSurfaceDisabledBrush` (12%) | None | Transparent |

### Elevation

- **Normal**: `Elevation="1"` (drop shadow)
- **Disabled**: `Elevation="0"` (no shadow)

### Elevated Button Fluent Equivalent

Standard button with subtle shadow:

```xml
<Button Background="{ThemeResource SystemControlBackgroundBaseLowBrush}"
        Foreground="{ThemeResource SystemAccentBrush}">
    <Button.Shadow>
        <ThemeShadow />
    </Button.Shadow>
</Button>
```

## Outlined Button Tokens

Medium emphasis button with border, no fill.

### Outlined Button Colors

| State | Foreground | Background | State Layer | Border |
| --- | --- | --- | --- | --- |
| **Normal** | `PrimaryBrush` | Transparent | None | `OutlineBrush` |
| **Hover** | `PrimaryBrush` | Transparent | `PrimaryHoverBrush` (8%) | `OutlineBrush` |
| **Pressed** | `PrimaryBrush` | Transparent | `PrimaryPressedBrush` (12%) | `OutlineBrush` |
| **Disabled** | `OnSurfaceDisabledBrush` (12%) | `OnSurfaceDisabledBrush` (12%) | None | `OutlineDisabledBrush` |

### Outlined Button Fluent Equivalent

Standard button (has border by default):

```xml
<Button Background="Transparent"
        Foreground="{ThemeResource SystemAccentBrush}"
        BorderBrush="{ThemeResource SystemControlForegroundBaseMediumLowBrush}"
        BorderThickness="1" />
```

## Text Button Tokens

Low emphasis button, text only, no background or border.

### Text Button Colors

| State | Foreground | Background | State Layer | Border |
| --- | --- | --- | --- | --- |
| **Normal** | `PrimaryBrush` | Transparent | None | Transparent |
| **Hover** | `PrimaryBrush` | Transparent | `PrimaryHoverBrush` (8%) | Transparent |
| **Pressed** | `PrimaryBrush` | Transparent | `PrimaryPressedBrush` (12%) | Transparent |
| **Disabled** | `OnSurfaceDisabledBrush` (12%) | Transparent | None | Transparent |

### Text Button Fluent Equivalent

Hyperlink button or custom borderless button:

```xml
<HyperlinkButton Content="Action" />

<!-- Or custom button -->
<Button Background="Transparent"
        BorderThickness="0"
        Foreground="{ThemeResource SystemAccentBrush}" />
```

## Icon Button Tokens

Icon-only button, circular ripple effect.

### Icon Button Colors

| State | Foreground | Ripple Background | Notes |
| --- | --- | --- | --- |
| **Normal** | `OnSurfaceVariantBrush` | None | Neutral icon color |
| **Hover** | `OnSurfaceVariantBrush` | `PrimaryHoverBrush` (circular) | Circular ripple |
| **Pressed** | `OnSurfaceVariantBrush` | `PrimaryPressedBrush` (circular) | Stronger ripple |
| **Focused** | `OnSurfaceVariantBrush` | `PrimaryFocusedBrush` (circular) | Focus ring |
| **Disabled** | `OnSurfaceLowBrush` (32%) | None | Faded icon |

### Icon Button Fluent Equivalent

Icon button or AppBarButton without label:

```xml
<AppBarButton Icon="Accept" Label="" />

<!-- Or custom -->
<Button Background="Transparent"
        BorderThickness="0"
        Width="40" Height="40"
        CornerRadius="20">
    <FontIcon Glyph="&#xE8FB;" />
</Button>
```

## Button Hierarchy Usage Guide

| Scenario | Recommended Style | Fluent Equivalent |
| --- | --- | --- |
| **Primary/urgent action** | Filled Button | AccentButtonStyle |
| **Secondary important action** | Filled Tonal Button | Default Button |
| **Important without prominence** | Elevated Button | Default with shadow |
| **Medium emphasis** | Outlined Button | Default Button |
| **Lowest emphasis** | Text Button | Hyperlink or borderless |
| **Icon-only actions** | Icon Button | AppBarButton |
| **Floating Action Button (FAB)** | N/A (separate control) | N/A |

## Combining Tokens in Custom Styles

### Custom Filled Button with Brand Color

```xml
<Style x:Key="BrandButtonStyle" TargetType="Button" BasedOn="{StaticResource MaterialFilledButtonStyle}">
    <!-- Override colors -->
    <Setter Property="Background" Value="#FF6200EE" />
    <Setter Property="Foreground" Value="White" />

    <!-- Keep Material dimensions -->
    <!-- MinHeight, MinWidth, CornerRadius inherited -->
</Style>
```

### Custom Text Button with Icon

```xml
<Button Style="{StaticResource MaterialTextButtonStyle}"
        Padding="{StaticResource TextButtonPadding}">
    <StackPanel Orientation="Horizontal" Spacing="{StaticResource ButtonContentMargin}">
        <FontIcon Glyph="&#xE710;" FontSize="{StaticResource ButtonIconMinWidth}" />
        <TextBlock Text="Action" />
    </StackPanel>
</Button>
```

## Related Controls

- **FAB** (Floating Action Button): See [FloatingActionButton](../../styles/FloatingActionButton.md)
- **ToggleButton**: Similar token structure with toggle states
- **AppBarButton**: See [AppBarButton](../../styles/AppBarButton.md)

## Accessibility Considerations

1. **Minimum Size**: Material's `ButtonMinHeight` (40px) meets touch target requirements (44px recommended)
2. **Disabled Contrast**: Material's 12% opacity may be too low. Override `DisabledOpacity` to 38% if needed
3. **Focus Indicators**: Material uses 12% focus state layer. Ensure visible focus indicator for keyboard users
4. **Icon Buttons**: Always provide accessible label: `AutomationProperties.Name="Action"`

## Related Documentation

- [Button Lightweight Styling](../../styles/Button.md)
- [Material Colors](../material-colors.md)
- [Typography Tokens](typography.md)
- [Opacity & States](opacity-states.md)
- [Material Design: Buttons](https://m3.material.io/components/buttons)
