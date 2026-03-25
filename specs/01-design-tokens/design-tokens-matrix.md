# Design Token Matrix: Material vs Fluent

This comprehensive reference documents all design tokens in the Uno Material theme and maps them to their WinUI/Fluent Design System equivalents. This matrix serves as a developer reference, migration guide, and design system comparison tool.

## Overview

The Uno Material theme implements Google's Material Design 3 (M3) design system with extensive design tokens for colors, typography, spacing, and component-specific styling. The Fluent theme leverages WinUI's built-in resources following Microsoft's Fluent Design System.

### Token Categories

| Category | Material Tokens | Fluent Equivalents | Documentation |
| -------- | --------------- | ------------------ | ------------- |
| **Colors** | ~40 semantic colors | SystemAccent, SystemBackground, etc. | [Colors Reference](design-tokens/colors.md) |
| **Brushes** | ~360 state-based brushes | SystemControl brushes | [Brushes Reference](design-tokens/brushes.md) |
| **Typography** | 15 type scales (M3) | TextBlock styles | [Typography Reference](design-tokens/typography.md) |
| **Opacity & States** | 8 opacity values | Control-specific opacities | [Opacity Reference](design-tokens/opacity-states.md) |
| **Animation** | 4 durations + easing | ControlAnimation resources | [Animation Reference](design-tokens/animation.md) |
| **Control-Specific** | Per-control tokens | Per-control resources | [Controls Reference](design-tokens/controls/) |

### Material Design System Characteristics

- **Semantic naming**: Tokens named by role (Primary, OnSurface, etc.)
- **State overlays**: Systematic opacity-based states (Hover, Pressed, Disabled, etc.)
- **Type scale**: 15 predefined typography scales from Display Large to Label Extra Small
- **Container concept**: Surface, container, and "On" (foreground) color relationships
- **Theme support**: Comprehensive Light and Dark theme definitions

### Fluent Design System Characteristics

- **System resources**: Built into WinUI as `SystemAccentColor`, `SystemControlBackgroundBrush`, etc.
- **Control-centric**: Resources often tied to specific control states
- **Acrylic/Reveal**: Material and transparency effects
- **Limited customization**: Fewer semantic tokens, more direct property setting

## Quick Reference: Common Mappings

### Colors

| Material | Fluent Equivalent | Notes |
| -------- | ----------------- | ----- |
| `PrimaryColor` | `SystemAccentColor` | Accent/brand color |
| `OnPrimaryColor` | Foreground on accent | Often White/Black |
| `SurfaceColor` | `SystemControlBackgroundBaseLowBrush` | Background surface |
| `OnSurfaceColor` | `SystemControlForegroundBaseHighBrush` | Primary text |
| `ErrorColor` | `SystemErrorTextColor` | Error/destructive actions |

### Typography

| Material Scale | Fluent Style | Font Size |
| -------------- | ------------ | --------- |
| `DisplayLarge` | Custom | 57px |
| `HeadlineMedium` | `TitleTextBlockStyle` | 28px |
| `TitleLarge` | `SubtitleTextBlockStyle` | 22px |
| `BodyLarge` | `BodyTextBlockStyle` | 16px |
| `LabelMedium` | `CaptionTextBlockStyle` | 12px |

## Migration Guide

For detailed guidance on migrating from Fluent to Material or vice versa, see:

- [Migration Guide](design-tokens/migration-guide.md)
- [Material Getting Started](material-getting-started.md)
- [Fluent Getting Started](fluent-getting-started.md)

## Token Inventory

### Core Design Tokens

- **40** base colors (Light + Dark themes)
- **360** state-based brushes (9 states per color)
- **15** typography scales
- **8** opacity/state values
- **4** animation durations

### Control-Specific Tokens

- **60+** controls with dedicated styling
- Spacing tokens (Padding, Margin)
- Dimension tokens (MinHeight, MinWidth, CornerRadius)
- Elevation values (Material M3 only)
- Border thickness values

## Related Documentation

- [Material Colors, Opacities and Brushes](material-colors.md) - Existing comprehensive color reference
- [Material DSP](material-dsp.md) - Color generation from Design System Package files
- [Material Control Styles](material-controls-styles.md) - Complete control style listing
- [Lightweight Styling](lightweight-styling.md) - Cross-theme customization approach

## Additional Resources

### Material Design 3 Specifications

- [Material Design 3](https://m3.material.io/) - Official M3 documentation
- [Material Theme Builder](https://m3.material.io/theme-builder) - Generate custom color schemes

### WinUI/Fluent Resources

- [WinUI 3 Gallery](https://github.com/microsoft/WinUI-Gallery) - Microsoft's official sample app
- [Fluent Design System](https://fluent2.microsoft.design/) - Microsoft's design system documentation

## C# Markup Support

The `Uno.Themes.WinUI.Markup` package provides strongly-typed C# accessors for all design tokens:

```csharp
using Uno.Themes.Markup;

// Access Material colors
var primary = Theme.Colors.Primary;
var onSurface = Theme.Brushes.OnSurface;

// Typography
var titleStyle = Theme.Typography.TitleLarge;
```

See [Uno.Themes.WinUI.Markup](../src/library/Uno.Themes.WinUI.Markup/) for more details.
