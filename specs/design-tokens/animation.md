# Animation Tokens: Material vs Fluent

This document maps Material Design 3 animation tokens to their WinUI/Fluent equivalents. Material defines standard durations and easing functions for consistent, fluid motion across all components.

## Material Animation System Overview

Material's animation system provides:

- **Standard durations**: Predefined timing for different animation types
- **Easing functions**: Cubic easing curves for natural motion
- **Delayed timing**: Coordinated multi-element animations

**Philosophy**: Animations should be functional, purposeful, and quick. Material favors 200-300ms for most transitions.

**Source**: [AnimationConstants.xaml](../../src/library/Uno.Material/Styles/Application/Common/AnimationConstants.xaml)

## Duration Tokens

| Material Token | Value | Usage | WinUI/Fluent Equivalent |
| --------------- | ------- | ------- | ----------------------- |
| `MaterialAnimationDuration` | **0:0:0.25** (250ms) | Standard transitions | `ControlNormalAnimationDuration` (250ms) |
| `MaterialTextBoxAnimationDuration` | **0:0:0.25** (250ms) | TextBox label animation | `TextControlThemeAnimationDuration` (167ms) |
| `MaterialDelayedBeginTime` | **0:0:0.15** (150ms) | Staggered animations | Custom per control |
| `MaterialToggleSwitchNormalAnimationDuration` | **00:00:00.225** (225ms) | ToggleSwitch transitions | `ControlNormalAnimationDuration` (250ms) |
| `MaterialToggleSwitchFasterAnimationDuration` | **00:00:00.180** (180ms) | Fast ToggleSwitch states | `ControlFasterAnimationDuration` (167ms) |

## Easing Function Tokens

Material uses cubic easing for smooth, natural acceleration and deceleration.

| Material Token | Easing Type | Easing Mode | WinUI/Fluent Equivalent |
| --------------- | ------------- | ------------- | ----------------------- |
| `MaterialEaseInOutFunction` | `CubicEase` | `EaseInOut` | Default for most controls |
| `MaterialEaseOutFunction` | `CubicEase` | `EaseOut` | Exit/expand animations |

### Easing Characteristics

#### CubicEase EaseInOut

- **Purpose**: Smooth entry and exit
- **Usage**: Standard transitions, property changes, theme changes
- **Behavior**: Starts slow, accelerates in middle, slows at end
- **Fluent Equivalent**: Default cubic-bezier or `CubicEase`

```xml
<CubicEase x:Key="MaterialEaseInOutFunction" EasingMode="EaseInOut" />
```

#### CubicEase EaseOut

- **Purpose**: Quick entry, smooth exit
- **Usage**: Appearing elements, expanding panels
- **Behavior**: Starts fast, decelerates smoothly
- **Fluent Equivalent**: `CubicEase` with `EaseOut` mode

```xml
<CubicEase x:Key="MaterialEaseOutFunction" EasingMode="EaseOut" />
```

## Special Timing Tokens

### Delayed Begin Time

- **Value**: `0:0:0.15` (150ms)
- **Purpose**: Stagger multiple element animations
- **Usage**: TextBox label rising, coordinated transitions

**Example**: TextBox placeholder label waits 150ms before starting animation while background animates immediately.

### ToggleSwitch Splines

Material also defines KeySpline values for ToggleSwitch (legacy support):

| Material Token | Value | WinUI/Fluent Equivalent |
| --------------- | ------- | ----------------------- |
| `MaterialToggleSwitchFastOutSlowInKeySpline` | `0.5,0,0.5,1` | ToggleSwitch easing |

**KeySpline Format**: `x1,y1,x2,y2` (Bezier control points)

## Animation Duration Guidelines

Material follows these timing principles:

| Duration | Purpose | Examples |
| ---------- | --------- | ---------- |
| **100-150ms** | Micro-interactions | Icon changes, small ripples |
| **200-250ms** | Standard transitions | Button presses, navigation, fades |
| **300-400ms** | Complex animations | Panel slides, page transitions |
| **500ms+** | Slow, expressive | Splash screens, special effects |

**Material Default**: **250ms** for most transitions strikes balance between responsiveness and smoothness.

## Common Animation Patterns

### Fade Animations

```xml
<Storyboard>
    <DoubleAnimation Storyboard.TargetProperty="Opacity"
                     From="0" To="1"
                     Duration="{StaticResource MaterialAnimationDuration}"
                     EasingFunction="{StaticResource MaterialEaseOutFunction}" />
</Storyboard>
```

**Usage**: Appearing/disappearing elements
**Fluent Equivalent**: Similar, often with `FadeInThemeAnimation`

### Slide Animations

```xml
<Storyboard>
    <DoubleAnimation Storyboard.TargetProperty="(UIElement.RenderTransform).(TranslateTransform.Y)"
                     From="20" To="0"
                     Duration="{StaticResource MaterialAnimationDuration}"
                     EasingFunction="{StaticResource MaterialEaseOutFunction}" />
</Storyboard>
```

**Usage**: Panels, drawers, bottom sheets
**Fluent Equivalent**: `DrillInNavigationTransitionInfo`, `SlideNavigationTransitionInfo`

### Scale Animations

```xml
<Storyboard>
    <DoubleAnimation Storyboard.TargetProperty="(UIElement.RenderTransform).(ScaleTransform.ScaleX)"
                     From="0.8" To="1.0"
                     Duration="{StaticResource MaterialAnimationDuration}"
                     EasingFunction="{StaticResource MaterialEaseOutFunction}" />
</Storyboard>
```

**Usage**: Dialogs, FAB transformations
**Fluent Equivalent**: `PopupThemeTransition`

## Control-Specific Animation Examples

### Button Ripple Effect

- **Duration**: ~400ms (ripple expansion)
- **Easing**: EaseOut
- **Layers**: Press state layer fades in 250ms

### TextBox Label

- **Duration**: 250ms (`MaterialTextBoxAnimationDuration`)
- **Delay**: 150ms (`MaterialDelayedBeginTime`)
- **Easing**: EaseInOut
- **Animation**: Label translates up and scales down

### NavigationView Expansion

- **Duration**: 250ms (pane slide)
- **Easing**: EaseOut
- **Layers**: Backdrop fade, pane slide

### ToggleSwitch

- **Duration**: 225ms (normal), 180ms (faster for states)
- **Easing**: Fast-out-slow-in (0.5,0,0.5,1)
- **Animation**: Thumb slides, track color transitions

## Fluent Animation Durations

For comparison, Fluent Design defines:

| Fluent Resource | Value | Material Equivalent |
| ---------------- | ------- | --------------------- |
| `ControlFasterAnimationDuration` | **167ms** | `MaterialToggleSwitchFasterAnimationDuration` (180ms) |
| `ControlFastAnimationDuration` | **167ms** | Close to Material default |
| `ControlNormalAnimationDuration` | **250ms** | `MaterialAnimationDuration` ✓ |
| `ControlSlowAnimationDuration` | **500ms** | Material avoids this (too slow) |

**Alignment**: Material and Fluent both favor **250ms** as the standard duration.

## Motion Design Principles

### Material Motion Characteristics

1. **Responsive**: Quick enough to feel instant (200-300ms)
2. **Natural**: Uses easing to mimic physical motion
3. **Aware**: Elements respond to user input with purpose
4. **Intentional**: Every animation has a functional purpose

### When to Animate (Material)

✅ **Do animate**:

- State changes (hover, focus, press)
- Transitions between views
- Revealing/hiding content
- Indicating progress

❌ **Avoid animating**:

- Initial load (except hero/splash)
- Purely decorative motion
- Frequent repetitive actions

### Accessibility

- **Respect user preferences**: Check `UISettings.AnimationsEnabled` or `prefers-reduced-motion`
- **Provide alternatives**: Don't rely solely on animation to convey information
- **Keep it brief**: Long animations can frustrate users

## Customization

### Global Duration Override

```xml
<Application.Resources>
    <ResourceDictionary>
        <Duration x:Key="MaterialAnimationDuration">0:0:0.2</Duration>
    </ResourceDictionary>
</Application.Resources>
```

**Effect**: All animations using this token update to 200ms.

### Custom Easing

```xml
<Application.Resources>
    <!-- Faster ease-out -->
    <ExponentialEase x:Key="MaterialEaseOutFunction"
                     EasingMode="EaseOut"
                     Exponent="7" />
</Application.Resources>
```

### Disable Animations (Accessibility)

```csharp
// Check user preference
var uiSettings = new UISettings();
bool animationsEnabled = uiSettings.AnimationsEnabled;

if (!animationsEnabled)
{
    // Use instant transitions
    duration = TimeSpan.Zero;
}
```

## Related Documentation

- [Material Design: Motion](https://m3.material.io/styles/motion) - Official M3 motion guidelines
- [Material Getting Started](../material-getting-started.md) - Animation setup
- [Control-Specific Tokens](controls/) - Per-control animation details
- [Fluent Motion](https://learn.microsoft.com/en-us/windows/apps/design/motion/) - Microsoft motion guidelines
