# Cupertino (iOS) Design System — Theme Specification

**Theme**: Cupertino  
**Created**: 2026-02-16  
**Status**: Active  
**Library**: `Uno.Cupertino.WinUI`  
**Design Reference**: [Apple Human Interface Guidelines](https://developer.apple.com/design/human-interface-guidelines)

## Overview

The Cupertino theme implements Apple's Human Interface Guidelines (HIG) for a
native iOS/macOS look and feel. It is the **smallest** theme in the Uno.Themes
repository with **~33 named styles** across **16 control types**. Unlike
Material, it has a single version track (no V1/V2 split) and uses Apple's SF Pro
font family and iOS semantic color system.

## Design Tokens

### Color Palette (Apple System Colors)

#### Tint Colors (Light / Dark)

| Token | Light | Dark | Usage |
|-------|-------|------|-------|
| Blue | `#007BFF` | `#0A84FF` | Primary actions, links, tint |
| Green | `#34C759` | `#30D158` | Success, positive states |
| Indigo | `#5856D6` | `#5E5CE6` | Accent |
| Orange | `#FF9500` | `#FF9F0A` | Warnings |
| Pink | `#FF2D54` | `#FF375F` | Emphasis |
| Purple | `#AF52DE` | `#BF5AF2` | Accent |
| Red | `#FF3B30` | `#FF443A` | Destructive, errors |
| Teal | `#5AC7FA` | `#64D3FF` | Accent |
| Yellow | `#FFCC00` | `#FFD60A` | Attention |

#### Gray Scale

| Token | Light | Dark |
|-------|-------|------|
| PrimaryGray | `#8E8E93` | `#8E8E93` |
| SecondaryGray | `#AEAEB2` | `#636366` |
| TertiaryGray | `#C7C7CC` | `#48484A` |
| QuaternaryGray | `#D1D1D6` | `#3A3A3C` |
| QuinaryGray | `#E5E5EA` | `#2C2C2E` |
| SenaryGray | `#F2F2F7` | `#1C1C1E` |

#### Semantic Colors

| Token | Light | Dark | Usage |
|-------|-------|------|-------|
| Label | `#000000` | `#FFFFFF` | Primary text |
| SecondaryLabel | `#993c3c43` | `#99ebebf5` | Secondary text |
| TertiaryLabel | `#4c3c3c43` | `#4cebebf5` | Tertiary text |
| QuaternaryLabel | `#2d3c3c43` | `#2debebf5` | Quaternary text |
| SystemFill | `#33787880` | `#5b787880` | Input backgrounds |
| SecondarySystemFill | `#28787880` | `#51787880` | Secondary fills |
| TertiarySystemFill | `#1e767680` | `#3d767680` | Tertiary fills |
| QuaternarySystemFill | `#14747480` | `#2d767680` | Quaternary fills |
| PlaceholderText | `#4c3c3c43` | `#4cebebf5` | Placeholder text |
| SystemBackground | `#FFFFFF` | `#000000` | Primary background |
| SecondarySystemBackground | `#F2F2F7` | `#1C1C1E` | Grouped backgrounds |
| TertiarySystemBackground | `#FFFFFF` | `#2C2C2E` | Elevated surfaces |
| Separator | `#493c3c43` | `#99545458` | Dividers, separators |
| OpaqueSeparator | `#C6C6C8` | `#38383A` | Opaque separators |
| Link | `#007AFF` | `#0984FF` | Links, hyperlinks |
| Shadow | `#33000000` | `#33000000` | Drop shadows |

### Brush Keys

Each color above has a corresponding brush:

**Tint brushes**: `CupertinoBlueBrush`, `CupertinoGreenBrush`,
`CupertinoIndigoBrush`, `CupertinoOrangeBrush`, `CupertinoPinkBrush`,
`CupertinoPurpleBrush`, `CupertinoRedBrush`, `CupertinoTealBrush`,
`CupertinoYellowBrush`, `CupertinoWhiteBrush`, `CupertinoBlackBrush`

**Gray brushes**: `CupertinoPrimaryGrayBrush` through `CupertinoSenaryGrayBrush`

**Semantic brushes**: `CupertinoLabelBrush`, `CupertinoSecondaryLabelBrush`,
`CupertinoTertiaryLabelBrush`, `CupertinoQuaternaryLabelBrush`,
`CupertinoSystemFillBrush`, `CupertinoSecondarySystemFillBrush`,
`CupertinoTertiarySystemFillBrush`, `CupertinoQuaternarySystemFillBrush`,
`CupertinoPlaceholderTextBrush`, `CupertinoSystemBackgroundBrush`,
`CupertinoSecondarySystemBackgroundBrush`,
`CupertinoTertiarySystemBackgroundBrush`,
`CupertinoSystemGroupedBackgroundBrush`,
`CupertinoSecondarySystemGroupedBackgroundBrush`,
`CupertinoTertiarySystemGroupedBackgroundBrush`,
`CupertinoSeparatorBrush`, `CupertinoOpaqueSeparatorBrush`,
`CupertinoLinkBrush`

**Total: ~50 brush resources** (with 3 theme variants: Light, Default, HighContrast)

### Typography (Apple HIG Type Scale)

| Style Key | Size | Line Height | Weight | Apple HIG Name |
|-----------|------|-------------|--------|---------------|
| `CupertinoLargeTitle` | 34 | 41 | Normal | Large Title |
| `CupertinoPrimaryTitle` | 28 | 34 | Normal | Title 1 |
| `CupertinoSecondaryTitle` | 22 | 28 | Normal | Title 2 |
| `CupertinoTertiaryTitle` | 20 | 25 | Normal | Title 3 |
| `CupertinoHeadline` | 17 | 22 | **SemiBold** | Headline |
| `CupertinoBody` | 17 | 22 | Normal | Body |
| `CupertinoCallout` | 16 | 21 | Normal | Callout |
| `CupertinoSubhead` | 15 | 20 | Normal | Subhead |
| `CupertinoFootnote` | 13 | 18 | Normal | Footnote |
| `CupertinoPrimaryCaption` | 12 | 16 | Normal | Caption 1 |
| `CupertinoSecondaryCaption` | 11 | 13 | Normal | Caption 2 |

All styles inherit from `CupertinoBaseTextBlockStyle` which sets
`TextTrimming=CharacterEllipsis`, `TextWrapping=Wrap`,
`FontFamily={ThemeResource CupertinoFontFamily}`.

**Font Family**: SF Pro (`CupertinoFontFamily`)

### Animation Constants

| Key | Value |
|-----|-------|
| `CupertinoEaseInOutFunction` | CubicEase (EaseInOut) |
| `CupertinoEaseOutFunction` | CubicEase (EaseOut) |
| `CupertinoAnimationDuration` | 0:0:0.10 |
| `CupertinoTextBoxAnimationDuration` | 0:0:0.25 |
| `CupertinoDelayedBeginTime` | 0:0:0.15 |

### State / Opacity Constants

| Key | Value | Usage |
|-----|-------|-------|
| `CupertinoDisableStateOpacity` | 0.50 | Disabled controls |
| `CupertinoHoverOpacity` | 0.85 | PointerOver general |
| `CupertinoHoverOverlayOpacity` | 0.15 | Contained button hover overlay |
| `CupertinoPressedOpacity` | 0.40 | Pressed (text-only) |
| `CupertinoPressedOverlayOpacity` | 0.60 | Pressed overlay (contained) |
| `CupertinoMediumOpacity` | 0.74 | Hyperlink hover |
| `CupertinoLowOpacity` | 0.38 | Low emphasis |
| `CupertinoFocusedOpacity` | 0.12 | Focus ring overlay |
| `CupertinoDraggedOpacity` | 0.08 | Drag state |
| `CupertinoSelectedOpacity` | 0.08 | Selected state |

### Interaction States

Cupertino uses **opacity-based** state transitions (no Material ripple):

| State | Visual Treatment |
|-------|-----------------|
| Normal | Base appearance |
| PointerOver | Entire control at 85% opacity, or 15% overlay |
| Pressed | Entire control at 40% opacity, or 60% overlay |
| Focused | Focus indicator at 12% opacity |
| Disabled | `Opacity=0.5` on the entire control |

## Control Style Inventory

### WinUI Built-in Controls

| Control | Style Count | Named Styles |
|---------|-------------|-------------|
| **Button** | 2 | `CupertinoButtonStyle` (text), `CupertinoContainedButtonStyle` (filled) |
| **CalendarDatePicker** | 2 | `CupertinoCalendarDatePickerStyle`, `DefaultCupertinoCalendarDatePickerStyle` |
| **CalendarView** | 2+4 | `CupertinoCalendarViewStyle`, `DefaultCupertinoCalendarViewStyle`, + 4 internal |
| **CheckBox** | 2 | `CupertinoCheckBoxStyle`, `CupertinoSecondaryCheckBoxStyle` |
| **ComboBox** | 2 | `CupertinoComboBoxStyle`, `CupertinoComboBoxItemStyle` |
| **DatePicker** | 3 | `CupertinoDatePickerStyle`, `CupertinoDatePickerFlyoutButtonStyle`, `CupertinoDatePickerFlyoutPresenterStyle` |
| **HyperlinkButton** | 1 | `CupertinoHyperlinkButtonStyle` |
| **NumberBox** | 1+3 | `CupertinoNumberBoxStyle` + 3 internal |
| **PasswordBox** | 1 | `CupertinoPasswordBoxStyle` |
| **ProgressBar** | 1 | `CupertinoProgressBarStyle` |
| **ProgressRing** | 1 | `CupertinoProgressRingStyle` |
| **RadioButton** | 1 | `CupertinoRadioButtonStyle` |
| **Slider** | 2 | `CupertinoSliderStyle`, `CupertinoSliderThumbStyle` |
| **TextBlock** | 12 | Type scale (11 sizes) + `CupertinoBaseTextBlockStyle` |
| **TextBox** | 1+1 | `CupertinoTextBoxStyle` + 1 internal `DeleteButtonStyle` |
| **ToggleSwitch** | 2 | `CupertinoToggleSwitchStyle`, `DefaultCupertinoToggleSwitchStyle` |

**Total: ~33 named styles across 16 control types**

### Controls NOT Styled in Cupertino (But Available in Material)

The following controls have **no** Cupertino styles — Material-only:

- AppBarButton
- CommandBar
- ContentDialog
- FloatingActionButton (all 12 variants)
- Flyout / MenuFlyout (all 14 styles)
- ListView / ListViewItem
- MediaPlayerElement
- NavigationView
- PipsPager
- RatingControl
- Ripple (Material custom control)
- ToggleButton

## Naming Rules (Cupertino-Specific)

1. All style keys use `Cupertino` prefix: `Cupertino<Variant><ControlType>Style`
2. Some controls use `DefaultCupertino...` as base style with `Cupertino...`
   BasedOn it (reversed from Material's pattern):
   - `DefaultCupertinoCalendarDatePickerStyle` → base
   - `CupertinoCalendarDatePickerStyle` → consumer-facing (BasedOn default)
3. Button variants follow iOS paradigm:
   - `CupertinoButtonStyle` → text-only (like iOS `.plain`)
   - `CupertinoContainedButtonStyle` → filled (like iOS `.borderedProminent`)
4. CheckBox variants: `CupertinoCheckBoxStyle` (primary),
   `CupertinoSecondaryCheckBoxStyle` (secondary tint)
5. Implicit style promotion: When `CupertinoResources` has
   `WithImplicitStyles=true`, named styles are also registered as TargetType
   defaults

## Lightweight Styling

Cupertino's lightweight styling is **less formalized** than Material:

- **Per-control resource keys** for dimensions and brushes:
  - `CupertinoButtonPadding`, `CupertinoButtonCornerRadius`,
    `CupertinoButtonFontSize`
  - `CupertinoTextBoxCornerRadius`, `CupertinoTextBoxBorderBrush`
  - `CupertinoComboBoxCornerRadius`, `CupertinoPasswordBoxCornerRadius`
  - `CupertinoCheckBoxBorderBrush`, `CupertinoDatePickerCornerRadius`
- **Color palette override** via `CupertinoColors.OverrideSource` property
- **Font override** via `CupertinoFonts.OverrideSource` property
- **No per-state resource key pattern** — unlike Material's
  `{Control}{Property}{State}` convention, Cupertino relies on template bindings
  and opacity state constants

## Theme Entry Point

```xml
<!-- App.xaml -->
<ResourceDictionary.MergedDictionaries>
    <CupertinoColors xmlns="using:Uno.Cupertino"
                     OverrideSource="ms-appx:///Styles/MyColorOverride.xaml" />
    <CupertinoFonts xmlns="using:Uno.Cupertino"
                    OverrideSource="ms-appx:///Styles/MyFontOverride.xaml" />
    <CupertinoResources xmlns="using:Uno.Cupertino"
                        WithImplicitStyles="True" />
</ResourceDictionary.MergedDictionaries>
```

### Entry Point Classes

| Class | Role |
|-------|------|
| `CupertinoResources` | Main ResourceDictionary — loads merged pages, registers implicit styles |
| `CupertinoColors` | Color ResourceDictionary — loads `ColorPalette.xaml` + `CupertinoColors.xaml`, applies overrides |
| `CupertinoFonts` | Font ResourceDictionary — loads `Fonts.xaml`, applies overrides |
| `CupertinoConstants` | Static class — URI path constants for resource files |

### Implicit Style Registration

When `WithImplicitStyles=true`, the following styles become the default for
their TargetType:

`CupertinoButtonStyle`, `CupertinoCalendarDatePickerStyle`,
`CupertinoCalendarViewStyle`, `CupertinoCheckBoxStyle`,
`CupertinoComboBoxStyle`, `CupertinoDatePickerStyle`,
`CupertinoDatePickerFlyoutPresenterStyle`, `CupertinoHyperlinkButtonStyle`,
`CupertinoNumberBoxStyle`, `CupertinoPasswordBoxStyle`,
`CupertinoProgressBarStyle`, `CupertinoProgressRingStyle` (non-Desktop),
`CupertinoRadioButtonStyle`, `CupertinoSliderStyle`, `CupertinoBody`
(TextBlock), `CupertinoTextBoxStyle`, `CupertinoToggleSwitchStyle`

## File Structure

```
src/library/Uno.Cupertino/
├── CupertinoResources.cs                ← Main entry point (ResourceDictionary)
├── CupertinoColors.cs                   ← Color resource loader
├── CupertinoFonts.cs                    ← Font resource loader
├── CupertinoConstants.cs                ← URI path constants
├── AssemblyInfo.cs
├── LinkerConfig.xamarin.xml
├── cupertino-common.props
├── Uno.Cupertino.WinUI.csproj           ← WinUI lineage
├── Uno.Cupertino.csproj                 ← UWP lineage
├── Assets/
│   └── CupertinoProgressRing.json       ← Lottie animation
├── Styles/
│   ├── Application/
│   │   ├── AnimationConstants.xaml       ← Easing, durations
│   │   ├── ColorPalette.xaml             ← Raw color tokens
│   │   ├── CupertinoColors.xaml          ← Brush definitions
│   │   ├── Fonts.xaml                    ← SF Pro font family
│   │   └── StateConstants.xaml           ← Opacity constants
│   └── Controls/
│       ├── _Resources.xaml               ← Implicit styles
│       ├── Button.xaml
│       ├── CalendarDatePicker.xaml
│       ├── CalendarView.xaml
│       ├── CheckBox.xaml
│       ├── ComboBox.xaml
│       ├── DatePicker.xaml
│       ├── HyperlinkButton.xaml
│       ├── NumberBox.xaml
│       ├── PasswordBox.xaml
│       ├── ProgressBar.xaml
│       ├── ProgressRing.xaml
│       ├── RadioButton.xaml
│       ├── Slider.xaml
│       ├── TextBlock.xaml
│       ├── TextBox.xaml
│       └── ToggleSwitch.xaml
└── Generated/
    └── mergedpages.xaml                  ← Auto-generated merged dict
```

## Comparison with Other Themes

| Aspect | Cupertino | Material | Simple |
|--------|-----------|----------|--------|
| Named styles | ~33 | ~140 | 33 |
| Control files | 16 | 29 | 7 |
| Color roles | ~40 | 33 | 10 |
| Brush resources | ~50 | ~594 | ~90 |
| Typography styles | 12 | 21 | 6 |
| Lightweight styling | Basic | Extensive | Basic |
| Custom controls | — | Ripple | — |
| Lottie animations | 1 | 2 | — |
| Version tracks | Single | V1 + V2 | Single |
| Entry point | `CupertinoResources` | `MaterialTheme` | `SimpleTheme` |
| Color override | DP-based | DP-based | DP-based |
| Font override | DP-based | DP-based | — |
| Font family | SF Pro | Roboto | Inter |
| Implicit style pattern | `WithImplicitStyles` prop | Always implicit + named | Always implicit + named |
| Button variants | 2 (text, contained) | 7 (filled, elevated, tonal, outlined, text, icon, base) | 4 (primary, neutral, subtle, light) + sizes |

### Coverage Gap Analysis

Controls that **should** be added to Cupertino for parity:

| Control | In Material | In Simple | In Cupertino | Notes |
|---------|------------|-----------|-------------|-------|
| ListView | ✅ | ❌ | ❌ | iOS UITableView equivalent |
| NavigationView | ✅ | ❌ | ❌ | iOS UINavigationController |
| CommandBar | ✅ | ❌ | ❌ | iOS UIToolbar |
| ContentDialog | ✅ | ❌ | ❌ | iOS UIAlertController |
| Flyout/MenuFlyout | ✅ (14) | ❌ | ❌ | iOS UIMenu |
| ToggleButton | ✅ | ❌ | ❌ | — |
| AppBarButton | ✅ | ❌ | ❌ | iOS UIBarButtonItem |
| PipsPager | ✅ | ❌ | ❌ | iOS UIPageControl |
| RatingControl | ✅ | ❌ | ❌ | — |
| MediaPlayerElement | ✅ | ❌ | ❌ | — |

**Version**: 1.0.0 | **Last Updated**: 2026-02-16
