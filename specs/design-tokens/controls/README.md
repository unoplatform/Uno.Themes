# Control-Specific Tokens

This directory contains detailed design token documentation for individual Material controls, mapping them to WinUI/Fluent equivalents.

## Available Control Documentation

### Common Controls

- [Button](button.md) - Button styles, dimensions, colors, and elevation tokens

### Coming Soon

The following controls will be documented with their specific design tokens:

#### Input Controls

- **TextBox** - Outlined and Filled variants, padding, corner radius, label animation
- **PasswordBox** - Similar to TextBox with reveal button tokens
- **ComboBox** - Dropdown height, item padding, selection states
- **CheckBox** - Size, check mark, indeterminate states
- **RadioButton** - Size, selection indicator, group spacing
- **ToggleSwitch** - Track dimensions, thumb size, animation timing
- **Slider** - Track height, thumb size, step indicators

#### Selection Controls

- **CalendarDatePicker** - Calendar dimensions, date cell sizes
- **DatePicker** - Flyout styling, picker dimensions
- **TimePicker** - Clock face dimensions

#### Navigation Controls

- **NavigationView** - Pane width, item height, compact mode dimensions
- **TabView** - Tab dimensions, close button size
- **PipsPager** - Pip size, spacing

#### Content Controls

- **AppBarButton** - Icon size, label spacing, compact heights
- **FloatingActionButton (FAB)** - FAB sizes (small, medium, large), elevation levels
- **ProgressBar** - Track height, indicator thickness
- **ProgressRing** - Ring diameter, stroke width
- **RatingControl** - Star size, spacing

#### Collections

- **ListView** - Item padding, divider thickness, selection overlay
- **GridView** - Item spacing, selection states

#### Other

- **ToggleButton** - Similar to Button with toggled state tokens
- **HyperlinkButton** - Text decoration, hover states

## Token Categories by Control

Each control document includes:

### 1. Dimension Tokens

- MinHeight, MinWidth
- Padding, Margin
- CornerRadius
- Icon/glyph sizes
- Spacing between elements

### 2. Color Tokens

- Foreground (all states)
- Background (all states)
- Border/Outline (all states)
- State layers (Hover, Pressed, Focused)
- Disabled state colors

### 3. Elevation Tokens (if applicable)

- Shadow depth levels
- Elevation changes per state

### 4. Animation Tokens (if applicable)

- Transition durations
- Easing functions
- Storyboard timing

### 5. Typography Tokens (if applicable)

- Font family
- Font size
- Font weight
- Character spacing

## How to Read Control Token Documentation

### Token Naming Convention

Control tokens follow the pattern:

```text
{ControlName}{Variant}{Property}{State}
```

Examples:

- `FilledButtonBackground` - Filled button background, normal state
- `FilledButtonForegroundPointerOver` - Filled button foreground on hover
- `OutlinedTextBoxCornerRadius` - Outlined TextBox corner radius
- `ElevatedButtonElevation` - Elevated button shadow depth

### State Suffixes

- *(none)* - Normal/default state
- `PointerOver` - Hover state
- `Pressed` - Pressed/active state
- `Focused` - Keyboard focus state
- `Disabled` - Disabled state
- `Selected` - Selected state (lists, toggles)
- `Checked` - Checked state (checkboxes, toggles)
- `Indeterminate` - Indeterminate state (checkboxes, progress)

## Fluent Mapping Strategy

### Direct Equivalents

Some Material tokens have direct Fluent counterparts:

```text
Material: ButtonMinHeight (40)
Fluent:   ButtonMinHeight (32)
```

### Semantic Equivalents

Some require mapping semantic roles:

```text
Material: FilledButtonBackground -> PrimaryBrush
Fluent:   FilledButtonBackground -> SystemAccentBrush
```

### No Direct Equivalent

Some Material concepts don't exist in Fluent:

```text
Material: FilledTonalButtonStyle
Fluent:   Custom implementation needed
```

### Custom Equivalents

Some Fluent features require Material customization:

```text
Fluent:   ThemeShadow
Material: ControlExtensions.Elevation
```

## Contributing Control Documentation

When documenting a new control, include:

1. **Overview** - Control variants and usage
2. **Dimension Token Table** - All sizing tokens
3. **Color Token Tables** - Per variant, organized by state
4. **Special Token Tables** - Elevation, animation, typography
5. **Usage Examples** - XAML snippets showing token application
6. **Fluent Comparison** - Equivalent patterns and resources
7. **Accessibility Notes** - Touch targets, contrast, focus
8. **Related Documentation** - Links to lightweight styling guides

## Related Documentation

- [Design Token Matrix](../../design-tokens-matrix.md) - Main index
- [Colors Reference](../colors.md) - Color token details
- [Typography Reference](../typography.md) - Typography scales
- [Opacity & States](../opacity-states.md) - State opacity values
- [Animation Reference](../animation.md) - Animation timings
- [Material Control Styles](../../material-controls-styles.md) - Complete style listing
