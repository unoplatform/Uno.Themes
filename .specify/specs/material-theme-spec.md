# Material Design System — Theme Specification

**Theme**: Material (M3 / Material Design 3)  
**Created**: 2026-02-16  
**Status**: Active  
**Library**: `Uno.Material.WinUI`  
**Design Reference**: [Material Design 3](https://m3.material.io)

## Overview

The Material theme implements Google's Material Design 3 (M3) guidelines. It is
the most comprehensive theme in the Uno.Themes repository, covering **~140
named styles** across **29 control files** with full support for dynamic color
theming, lightweight styling, and two version tracks (V1 legacy, V2 current).

This spec documents the **V2 (current)** styles only. V1 is deprecated.

## Design Tokens

### Color Palette (M3 Tonal Palette)

| Token | Light | Dark | Usage |
|-------|-------|------|-------|
| Primary | `#5946D2` | `#C7BFFF` | Primary actions, FABs, active indicators |
| OnPrimary | `#FFFFFF` | `#2A009F` | Text/icons on primary surfaces |
| PrimaryContainer | `#E5DEFF` | `#4129BA` | Tonal container backgrounds |
| OnPrimaryContainer | `#170065` | `#E4DFFF` | Text on primary containers |
| PrimaryInverse | `#C8BFFF` | `#2A009F` | Inverse primary for snackbars |
| Secondary | `#6B4EA2` | `#CDC2DC` | Secondary actions, filters |
| OnSecondary | `#FFFFFF` | `#332D41` | Text on secondary surfaces |
| SecondaryContainer | `#EBDDFF` | `#433C52` | Chips, filter backgrounds |
| OnSecondaryContainer | `#220555` | `#EDDFFF` | Text on secondary containers |
| Tertiary | `#0061A4` | `#9FCAFF` | Accent color, contrast elements |
| OnTertiary | `#FFFFFF` | `#003258` | Text on tertiary surfaces |
| TertiaryContainer | `#CFE4FF` | `#00497D` | Tertiary container backgrounds |
| OnTertiaryContainer | `#001D36` | `#D1E4FF` | Text on tertiary containers |
| Error | `#B3261E` | `#FFB4AB` | Error states, destructive actions |
| OnError | `#FFFFFF` | `#690005` | Text on error surfaces |
| ErrorContainer | `#F9DEDC` | `#93000A` | Error container backgrounds |
| OnErrorContainer | `#410E0B` | `#FFDAD6` | Text on error containers |
| Background | `#FCFBFF` | `#1C1B1F` | Page backgrounds |
| OnBackground | `#1C1B1F` | `#E5E1E6` | Text on background |
| Surface | `#FFFFFF` | `#302D37` | Card, dialog surfaces |
| OnSurface | `#1C1B1F` | `#E6E1E5` | Primary text on surfaces |
| SurfaceVariant | `#F2EFF5` | `#47464F` | Input backgrounds, chips |
| OnSurfaceVariant | `#8A8494` | `#C9C5D0` | Secondary text, icons |
| SurfaceInverse | `#313033` | `#E6E1E5` | Snackbar backgrounds |
| OnSurfaceInverse | `#F4EFF4` | `#1C1B1F` | Text on inverse surfaces |
| SurfaceTint | `#5946D2` | `#C7BFFF` | Tint overlay on elevated surfaces |
| Outline | `#79747E` | `#928F99` | Borders, dividers |
| OutlineVariant | `#C9C5D0` | `#57545D` | Subtle borders |
| Shadow | `#33000000` | `#33000000` | Drop shadows |

### Brush Generation Pattern

For **each** of the 33 color roles above, **9 opacity variants** are generated:

| Suffix | Opacity Constant |
|--------|-----------------|
| `{Role}Brush` | 1.0 (full) |
| `{Role}HoverBrush` | 0.08 |
| `{Role}FocusedBrush` | 0.12 |
| `{Role}PressedBrush` | 0.12 |
| `{Role}DraggedBrush` | 0.16 |
| `{Role}SelectedBrush` | 0.08 |
| `{Role}MediumBrush` | 0.64 |
| `{Role}LowBrush` | 0.32 |
| `{Role}DisabledBrush` | 0.12 |

**Total: ~33 × 9 × 2 themes ≈ 594 brush resources**

### Typography (M3 Type Scale)

| Style Key | Font Family | Weight | Size | Char Spacing |
|-----------|-------------|--------|------|-------------|
| `MaterialDisplayLarge` | Roboto Regular | Normal | 57 | -17 |
| `MaterialDisplayMedium` | Roboto Regular | Normal | 45 | — |
| `MaterialDisplaySmall` | Roboto Regular | Normal | 36 | — |
| `MaterialHeadlineLarge` | Roboto Regular | Normal | 32 | — |
| `MaterialHeadlineMedium` | Roboto Regular | Normal | 28 | — |
| `MaterialHeadlineSmall` | Roboto Regular | Normal | 24 | — |
| `MaterialTitleLarge` | Roboto Regular | Normal | 22 | — |
| `MaterialTitleMedium` | Roboto Medium | Medium | 16 | — |
| `MaterialTitleSmall` | Roboto Medium | Medium | 14 | — |
| `MaterialLabelLarge` | Roboto Medium | Medium | 14 | 7 |
| `MaterialLabelMedium` | Roboto Medium | Medium | 12 | 41 |
| `MaterialLabelSmall` | Roboto Medium | Medium | 11 | 45 |
| `MaterialLabelExtraSmall` | Roboto Medium | Normal | 11 | 7 |
| `MaterialBodyLarge` | Roboto Medium | Medium | 16 | 9 |
| `MaterialBodyMedium` | Roboto Medium | Medium | 14 | 17 |
| `MaterialBodySmall` | Roboto Medium | Medium | 12 | 33 |
| `MaterialCaptionLarge` | Roboto Medium | Medium | 13 | -3 |
| `MaterialCaptionMedium` | Roboto Medium | Medium | 12 | 33 |
| `MaterialCaptionSmall` | Roboto Medium | Medium | 11 | 7 |
| `MaterialBaseTextBlockStyle` | *(base)* | — | — | — |
| `MaterialDefaultTextBlockStyle` | *(implicit)* | — | = BodyMedium | — |

### Spacing & Sizing Constants

| Key | Value | Control |
|-----|-------|---------|
| `ButtonMinHeight` | 40 | Button |
| `ButtonMinWidth` | 40 | Button |
| `ElevatedButtonElevation` | 1 | Button |
| `MaterialFabPadding` | 20 | FAB |
| `MaterialFabCornerRadius` | 16 | FAB |
| `MaterialSmallFabPadding` | 12 | FAB (small) |
| `MaterialSmallFabCornerRadius` | 12 | FAB (small) |
| `MaterialLargeFabPadding` | 36 | FAB (large) |
| `MaterialLargeFabCornerRadius` | 28 | FAB (large) |
| `ComboBoxMinHeight` | 56 | ComboBox |
| `ComboBoxCornerRadius` | 4 | ComboBox |
| `CalendarDatePickerHeight` | 53 | CalendarDatePicker |
| `DatePickerHeight` | 53 | DatePicker |
| `DatePickerFlyoutElevation` | 8 | DatePicker |
| `MaterialContentDialogMinWidth` | 288 | ContentDialog |
| `MaterialContentDialogMaxWidth` | 560 | ContentDialog |
| `MaterialContentDialogCornerRadius` | 28 | ContentDialog |
| `TextBoxOutlinedCorderRadius` | 4 | TextBox |
| `TextBoxFocusStrokeWidth` | 2 | TextBox |

### Animation Constants

| Key | Value |
|-----|-------|
| `MaterialEaseInOutFunction` | CubicEase (EaseInOut) |
| `MaterialEaseOutFunction` | CubicEase (EaseOut) |
| `MaterialAnimationDuration` | 0:0:0.25 |
| `MaterialDelayedBeginTime` | 0:0:0.15 |

### Interaction States

M3 uses **opacity overlays** with state-layer tokens:

| State | Opacity | Visual Treatment |
|-------|---------|-----------------|
| Normal | 0 | Base appearance |
| Hover | 0.08 | State layer overlay (primary/surface) |
| Focused | 0.12 | State layer overlay + focus indicator |
| Pressed | 0.12 | State layer overlay |
| Dragged | 0.16 | State layer overlay |
| Disabled | 0.12 + 0.38 | Background at 12%, content at 38% |

## Control Style Inventory

### WinUI Built-in Controls (V2 — Current)

| Control | Style Count | Named Styles |
|---------|-------------|-------------|
| **AppBarButton** | 2 | `MaterialAppBarButtonStyle`, `MaterialDefaultAppBarButtonStyle` |
| **Button** | 10 | `MaterialBaseButtonStyle`, `MaterialFilledButtonStyle`, `MaterialElevatedButtonStyle`, `MaterialFilledTonalButtonStyle`, `MaterialOutlinedButtonStyle`, `MaterialTextButtonStyle`, `MaterialIconButtonStyle`, `MaterialDefaultButtonStyle`, `MaterialContentDialogButtonStyle`, `MaterialContentDialogDefaultButtonStyle` |
| **CalendarDatePicker** | 2 | `MaterialCalendarDatePickerStyle`, `MaterialDefaultCalendarDatePickerStyle` |
| **CalendarView** | 2+3 | `MaterialCalendarViewStyle`, `MaterialDefaultCalendarViewStyle`, + 3 internal |
| **CheckBox** | 2 | `MaterialCheckBoxStyle`, `MaterialDefaultCheckBoxStyle` |
| **ComboBox** | 3 | `MaterialComboBoxStyle`, `MaterialComboBoxItemStyle`, `MaterialDefaultComboBoxStyle` |
| **CommandBar** | 3 | `MaterialBaseCommandBarStyle`, `MaterialCommandBarStyle`, `MaterialDefaultCommandBarStyle` |
| **ContentDialog** | 2 | `MaterialContentDialogStyle`, `MaterialDefaultContentDialogStyle` |
| **DatePicker** | 5 | `MaterialDatePickerStyle`, `MaterialDatePickerFlyoutButtonStyle`, `MaterialDatePickerFlyoutPresenterStyle`, + 2 defaults |
| **FAB (Button)** | 12 | `MaterialFabStyle` × {Small,Large} × {Surface,Secondary,Tertiary} |
| **Flyout** | 7+7 | `MaterialFlyoutPresenterStyle`, `MaterialMenuFlyoutPresenterStyle`, `MaterialMenuFlyoutItemStyle`, `MaterialMenuFlyoutSeparatorStyle`, `MaterialMenuFlyoutSubItemStyle`, `MaterialToggleMenuFlyoutItemStyle`, `MaterialRadioMenuFlyoutItemStyle` + 7 defaults |
| **HyperlinkButton** | 3 | `MaterialHyperlinkButtonStyle`, `MaterialSecondaryHyperlinkButtonStyle`, `MaterialDefaultHyperlinkButtonStyle` |
| **ListView** | 4 | `MaterialListViewStyle`, `MaterialListViewItemStyle` + 2 defaults |
| **MediaPlayerElement** | 6+ | `MaterialMediaTransportControlsStyle` + 5 internal |
| **NavigationView** | 10+ | `MaterialNavigationViewStyle`, `MaterialNavigationViewItemStyle` + 8 internal |
| **PasswordBox** | 4 | `MaterialFilledPasswordBoxStyle`, `MaterialOutlinedPasswordBoxStyle`, `MaterialRevealButtonStyle`, `MaterialDefaultPasswordBoxStyle` |
| **PipsPager** | 8 | `MaterialPipsPagerStyle` + 6 internal button styles + default |
| **ProgressBar** | 2 | `MaterialProgressBarStyle`, `MaterialDefaultProgressBarStyle` |
| **ProgressRing** | 2 | `MaterialProgressRingStyle`, `MaterialDefaultProgressRingStyle` |
| **RadioButton** | 2 | `MaterialRadioButtonStyle`, `MaterialDefaultRadioButtonStyle` |
| **RatingControl** | 3 | `MaterialRatingControlStyle`, `SecondaryRatingControlStyle`, `MaterialDefaultRatingControlStyle` |
| **Ripple** | 2 | `MaterialRippleStyle`, `MaterialDefaultRippleStyle` |
| **Slider** | 3 | `MaterialSliderStyle`, `MaterialSliderThumbStyle`, `MaterialDefaultSliderStyle` |
| **TextBlock** | 21 | Type scale (19 sizes) + base + default |
| **TextBox** | 4 | `MaterialFilledTextBoxStyle`, `MaterialOutlinedTextBoxStyle`, `MaterialDeleteButtonStyle`, `MaterialDefaultTextBoxStyle` |
| **ToggleButton** | 3 | `MaterialTextToggleButtonStyle`, `MaterialIconToggleButtonStyle`, `MaterialDefaultToggleButtonStyle` |
| **ToggleSwitch** | 2 | `MaterialToggleSwitchStyle`, `MaterialDefaultToggleSwitchStyle` |

**Total: ~140 named styles across 29 control files**

### Controls NOT in Simple or Cupertino

The following controls are styled **only** in Material:

- AppBarButton
- CalendarDatePicker, CalendarView
- CommandBar
- ContentDialog
- DatePicker
- FloatingActionButton (12 variants)
- Flyout / MenuFlyout (14 styles)
- HyperlinkButton
- ListView / ListViewItem
- MediaPlayerElement
- NavigationView
- PasswordBox
- PipsPager
- ProgressBar, ProgressRing
- RatingControl
- Ripple (custom control)
- Slider
- ToggleButton

## Naming Rules (Material-Specific)

1. All style keys use `Material` prefix: `Material<Variant><ControlType>Style`
2. Each control has a `MaterialDefault<ControlType>Style` that serves as the
   implicit style (applied when no explicit `Style` is set)
3. Variant names follow M3 naming:
   - Buttons: `Filled`, `Elevated`, `FilledTonal`, `Outlined`, `Text`, `Icon`
   - TextBox/PasswordBox: `Filled`, `Outlined`
   - FAB: `Fab`, `SmallFab`, `LargeFab` × `Surface`, `Secondary`, `Tertiary`
4. Base styles use `MaterialBase<ControlType>Style`
5. Theme-agnostic aliases (without `Material` prefix) registered in `_Resources.xaml`:
   - `FilledButtonStyle` → `MaterialFilledButtonStyle`
   - `OutlinedTextBoxStyle` → `MaterialOutlinedTextBoxStyle`
   - etc. (~36 style aliases + ~22 typography/resource aliases)

## Lightweight Styling

Material uses the **most extensive** lightweight styling system in Uno.Themes:

- **Per-control `ThemeDictionary` blocks** at the top of each style file
- **Naming convention**: `{ControlName}{Property}{State}` — e.g.,
  `FilledButtonForegroundPointerOver`, `CheckBoxBackgroundCheckedDisabled`
- **All brushes reference semantic tokens** via `{ThemeResource PrimaryBrush}` —
  never hardcoded hex values
- **State variants**: Normal, PointerOver, Pressed, Disabled, Focused, Selected,
  Checked, CheckedPointerOver, CheckedPressed, CheckedDisabled
- **Override mechanism**: Apps set `ColorOverrideSource` or
  `ColorOverrideDictionary` on `MaterialTheme`, which replaces Color tokens in
  `SharedColorPalette`, cascading to all ~594 brushes and every style

## Theme Entry Point

```xml
<!-- App.xaml -->
<MaterialTheme xmlns="using:Uno.Material"
               ColorOverrideSource="ms-appx:///Styles/MyColors.xaml"
               FontOverrideSource="ms-appx:///Styles/MyFonts.xaml" />
```

### Class Hierarchy

- `MaterialTheme` (extends `BaseTheme` from `Uno.Themes.WinUI`)
  - Loads `mergedpages.v2.xaml` (all V2 control styles, auto-merged)
  - Loads `Fonts.xaml` (font resources)
  - Merges font overrides if provided
  - Color loading handled by `BaseTheme` → `SharedColorPalette` + `SharedColors`

### Version History

| Version | Status | Entry Point | Notes |
|---------|--------|-------------|-------|
| V1 | Deprecated | `MaterialResourcesV1` | Fewer controls, different color tokens |
| V2 | **Current** | `MaterialTheme` | Full M3 spec, 29 controls |

## File Structure

```
src/library/Uno.Material/
├── MaterialTheme.cs                     ← Current entry point
├── MaterialConstants.cs                 ← URI path constants
├── MaterialColors.cs                    ← Alias for V2
├── MaterialColorsV2.cs                  ← Color resource loader
├── MaterialFonts.cs                     ← Font resource loader
├── MaterialResources.cs                 ← Alias for V2
├── MaterialResourcesV2.cs              ← V2 resource aggregator
├── Controls/
│   └── Ripple.cs                        ← Custom ripple control
├── Converters/
│   └── FromTextBoxEmptyStringToValueConverter.cs
├── Assets/
│   ├── MaterialDeterminate.json         ← Lottie (ProgressBar)
│   └── MaterialIndeterminate.json       ← Lottie (ProgressBar)
├── Styles/
│   ├── Application/
│   │   ├── Common/
│   │   │   ├── AnimationConstants.xaml
│   │   │   ├── Fonts.xaml               ← Roboto font definitions
│   │   │   └── TextBoxVariables.xaml
│   │   ├── v1/
│   │   │   ├── ColorPalette.xaml        ← V1 colors (deprecated)
│   │   │   └── MaterialColors.xaml
│   │   └── v2/
│   │       └── Typography.xaml          ← V2 type scale variables
│   └── Controls/
│       ├── v1/  (27 files — legacy, deprecated)
│       └── v2/  (29 files — CURRENT)
│           ├── _Resources.xaml          ← Implicit styles + aliases
│           ├── AppBarButton.xaml
│           ├── Button.xaml
│           ├── CalendarDatePicker.xaml
│           ├── CalendarView.xaml
│           ├── CheckBox.xaml
│           ├── ComboBox.xaml
│           ├── CommandBar.xaml
│           ├── ContentDialog.xaml
│           ├── DatePicker.xaml
│           ├── FloatingActionButton.xaml
│           ├── Flyout.xaml
│           ├── HyperlinkButton.xaml
│           ├── ListView.xaml
│           ├── MediaPlayerElement.xaml
│           ├── NavigationView.xaml
│           ├── PasswordBox.xaml
│           ├── PipsPager.Base.xaml
│           ├── PipsPager.xaml
│           ├── ProgressBar.xaml
│           ├── ProgressRing.xaml
│           ├── RadioButton.xaml
│           ├── RatingControl.xaml
│           ├── Ripple.xaml
│           ├── Slider.xaml
│           ├── TextBlock.xaml
│           ├── TextBox.xaml
│           ├── ToggleButton.xaml
│           └── ToggleSwitch.xaml
└── Generated/
    ├── mergedpages.v1.xaml              ← V1 auto-merged (deprecated)
    └── mergedpages.v2.xaml              ← V2 auto-merged (current)
```

## Comparison with Other Themes

| Aspect | Material | Simple | Cupertino |
|--------|----------|--------|-----------|
| Named styles | ~140 | 33 | ~33 |
| Control files | 29 | 7 | 16 |
| Color roles | 33 | 10 | ~40 |
| Brush resources | ~594 | ~90 | ~50 |
| Typography styles | 21 | 6 | 12 |
| Lightweight styling | Extensive | Basic | Basic |
| Custom controls | Ripple | — | — |
| Lottie animations | 2 | — | 1 |
| Version tracks | V1 + V2 | Single | Single |
| Entry point | `MaterialTheme` | `SimpleTheme` | `CupertinoResources` |
| Color override | DP-based | DP-based | DP-based |
| Font override | DP-based | — | DP-based |

**Version**: 1.0.0 | **Last Updated**: 2026-02-16
