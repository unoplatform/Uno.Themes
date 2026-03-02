# Feature Specification: Cupertino (iOS HIG) Theme

**Library**: `Uno.Cupertino.WinUI`
**Design Reference**: [Apple Human Interface Guidelines](https://developer.apple.com/design/human-interface-guidelines)
**Source Path**: `src/library/Uno.Cupertino/`

## Context

The Cupertino theme implements Apple's Human Interface Guidelines (HIG) for a
native iOS/macOS look and feel. It is the **smallest** theme in Uno.Themes —
**~36 consumer-facing styles** across **16 control types**. It uses Apple's
SF Pro font, iOS semantic color system (with Light/Dark ThemeDictionaries), and
has a **different entry point pattern** than Material/Simple (no `BaseTheme`
inheritance).

### Current Implementation Status

| Layer | File(s) | Status |
|-------|---------|--------|
| Color Palette | `ColorPalette.xaml` (ThemeDictionaries L+D) | ✅ Complete |
| Semantic Brushes | `CupertinoColors.xaml` (brush references) | ✅ Complete |
| Typography | `Fonts.xaml` (SF Pro) | ✅ Complete |
| Animation Constants | `AnimationConstants.xaml` | ✅ Complete |
| State Constants | `StateConstants.xaml` (opacity values) | ✅ Complete |
| Control Styles | 16 XAML files (~36 styles) | ✅ Complete |
| Implicit Styles | `_Resources.xaml` (minimal) | ✅ Complete |
| Entry Point | `CupertinoResources.cs` (extends ResourceDictionary) | ✅ Complete |
| Lottie Assets | `CupertinoProgressRing.json` | ✅ Complete |
| XamlMerge | `cupertino-common.props` → `mergedpages.xaml` | ✅ Complete |

### ⚠️ Entry Point Difference

Unlike Material/Simple which extend `BaseTheme`, Cupertino's `CupertinoResources`
extends `ResourceDictionary` **directly**. This is a legacy pattern. It uses
three separate classes (`CupertinoColors`, `CupertinoFonts`, `CupertinoResources`)
loaded individually in App.xaml.

---

## Design Tokens

### Color Palette (Apple System Colors)

ThemeDictionaries with **Light** and **Dark** keys.

#### Tint Colors

| Token | Light | Dark | Usage |
|-------|-------|------|-------|
| Blue | `#007BFF` | `#0A84FF` | Primary actions, links |
| Green | `#34C759` | `#30D158` | Success states |
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
| Separator | `#493c3c43` | `#99545458` | Dividers |
| OpaqueSeparator | `#C6C6C8` | `#38383A` | Opaque separators |
| Link | `#007AFF` | `#0984FF` | Links |
| Shadow | `#33000000` | `#33000000` | Drop shadows |

### Brush Keys (~50)

**Tint**: `CupertinoBlueBrush`, `CupertinoGreenBrush`, `CupertinoIndigoBrush`,
`CupertinoOrangeBrush`, `CupertinoPinkBrush`, `CupertinoPurpleBrush`,
`CupertinoRedBrush`, `CupertinoTealBrush`, `CupertinoYellowBrush`,
`CupertinoWhiteBrush`, `CupertinoBlackBrush`

**Gray**: `CupertinoPrimaryGrayBrush` through `CupertinoSenaryGrayBrush`

**Semantic**: `CupertinoLabelBrush`, `CupertinoSecondaryLabelBrush`,
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

**Total: ~50 brush resources** (with 3 theme keys: Light, Default, HighContrast)

### Typography (Apple HIG Type Scale — 12 styles)

| Style Key | Size | Line Height | Weight | Apple HIG Name |
|-----------|------|-------------|--------|---------------|
| `CupertinoLargeTitle` | 34 | 41 | Normal | Large Title |
| `CupertinoPrimaryTitle` | 28 | 34 | Normal | Title 1 |
| `CupertinoSecondaryTitle` | 22 | 28 | Normal | Title 2 |
| `CupertinoTertiaryTitle` | 20 | 25 | Normal | Title 3 |
| `CupertinoHeadline` | 17 | 22 | SemiBold | Headline |
| `CupertinoBody` | 17 | 22 | Normal | Body |
| `CupertinoCallout` | 16 | 21 | Normal | Callout |
| `CupertinoSubhead` | 15 | 20 | Normal | Subhead |
| `CupertinoFootnote` | 13 | 18 | Normal | Footnote |
| `CupertinoPrimaryCaption` | 12 | 16 | Normal | Caption 1 |
| `CupertinoSecondaryCaption` | 11 | 13 | Normal | Caption 2 |
| `CupertinoBaseTextBlockStyle` | *(base)* | — | — | *(base)* |

**Font Family**: `CupertinoFontFamily` = SF Pro

### State / Opacity Constants

| Key | Value | Usage |
|-----|-------|-------|
| `CupertinoDisableStateOpacity` | 0.50 | Disabled controls |
| `CupertinoHoverOpacity` | 0.85 | PointerOver general |
| `CupertinoHoverOverlayOpacity` | 0.15 | Contained button hover |
| `CupertinoPressedOpacity` | 0.40 | Pressed (text-only) |
| `CupertinoPressedOverlayOpacity` | 0.60 | Pressed overlay (contained) |
| `CupertinoMediumOpacity` | 0.74 | Hyperlink hover |
| `CupertinoLowOpacity` | 0.38 | Low emphasis |
| `CupertinoFocusedOpacity` | 0.12 | Focus ring overlay |
| `CupertinoDraggedOpacity` | 0.08 | Drag state |
| `CupertinoSelectedOpacity` | 0.08 | Selected state |

### Interaction States

Cupertino uses **opacity-based** state transitions (no ripple):

| State | Visual Treatment |
|-------|-----------------|
| Normal | Base appearance |
| PointerOver | Entire control at 85% opacity, or 15% overlay |
| Pressed | Entire control at 40% opacity, or 60% overlay |
| Focused | Focus indicator at 12% opacity |
| Disabled | `Opacity=0.5` on entire control |

---

## Control Style Inventory (~36 Consumer Styles)

### Button (2) — `Button.xaml`

| Style Key | iOS Equivalent | Description |
|-----------|---------------|-------------|
| `CupertinoButtonStyle` | `.plain` | Text-only button |
| `CupertinoContainedButtonStyle` | `.borderedProminent` | Filled button |

### CalendarDatePicker (2) — `CalendarDatePicker.xaml`

| Style Key | Description |
|-----------|-------------|
| `CupertinoCalendarDatePickerStyle` | Consumer-facing |
| `DefaultCupertinoCalendarDatePickerStyle` | *(base)* |

### CalendarView (2+4 internal) — `CalendarView.xaml`

| Style Key | Description |
|-----------|-------------|
| `CupertinoCalendarViewStyle` | Consumer-facing |
| `DefaultCupertinoCalendarViewStyle` | *(base)* |
| + `WeekDayNameStyle`, `NavigationButtonStyle`, `HeaderButtonStyle`, `ScrollViewerStyle` | *(internal)* |

### CheckBox (2) — `CheckBox.xaml`

| Style Key | Description |
|-----------|-------------|
| `CupertinoCheckBoxStyle` | Primary checkbox |
| `CupertinoSecondaryCheckBoxStyle` | Secondary tint |

### ComboBox (2) — `ComboBox.xaml`

| Style Key | Description |
|-----------|-------------|
| `CupertinoComboBoxStyle` | Dropdown selector |
| `CupertinoComboBoxItemStyle` | Dropdown item |

### DatePicker (3) — `DatePicker.xaml`

| Style Key | Description |
|-----------|-------------|
| `CupertinoDatePickerStyle` | Date picker |
| `CupertinoDatePickerFlyoutButtonStyle` | Picker trigger |
| `CupertinoDatePickerFlyoutPresenterStyle` | Flyout container |

### HyperlinkButton (1) — `HyperlinkButton.xaml`

| Style Key | Description |
|-----------|-------------|
| `CupertinoHyperlinkButtonStyle` | Link button |

### NumberBox (1+3 internal) — `NumberBox.xaml`

| Style Key | Description |
|-----------|-------------|
| `CupertinoNumberBoxStyle` | Stepper input |
| + `NumberBoxPlusButtonStyle`, `NumberBoxMinusButtonStyle`, `NumberBoxTextBoxStyle` | *(internal)* |

### PasswordBox (1) — `PasswordBox.xaml`

| Style Key | Description |
|-----------|-------------|
| `CupertinoPasswordBoxStyle` | Secure input |

### ProgressBar (1) — `ProgressBar.xaml`

| Style Key | Description |
|-----------|-------------|
| `CupertinoProgressBarStyle` | Linear progress |

### ProgressRing (1) — `ProgressRing.xaml`

| Style Key | Description |
|-----------|-------------|
| `CupertinoProgressRingStyle` | Circular spinner (platform-split: win / non-win) |

### RadioButton (1) — `RadioButton.xaml`

| Style Key | Description |
|-----------|-------------|
| `CupertinoRadioButtonStyle` | iOS-style radio |

### Slider (2+1 internal) — `Slider.xaml`

| Style Key | Description |
|-----------|-------------|
| `CupertinoSliderStyle` | Primary slider |
| `CupertinoSecondarySliderStyle` | Secondary tint |
| `CupertinoSliderThumbStyle` | *(internal)* Thumb |

### TextBlock (12) — `TextBlock.xaml`

All 11 type scale styles + `CupertinoBaseTextBlockStyle` (see Typography section).

### TextBox (1+1 internal) — `TextBox.xaml`

| Style Key | Description |
|-----------|-------------|
| `CupertinoTextBoxStyle` | Text input |
| `DeleteButtonStyle` | *(internal)* Clear button |

### ToggleSwitch (2) — `ToggleSwitch.xaml`

| Style Key | Description |
|-----------|-------------|
| `CupertinoToggleSwitchStyle` | iOS toggle |
| `DefaultCupertinoToggleSwitchStyle` | *(base)* |

---

## _Resources.xaml — Minimal

Cupertino's `_Resources.xaml` is **minimal** — it only merges `CupertinoColors`
and `CupertinoFonts`. It does NOT define:

- Theme-agnostic aliases
- Implicit styles via XAML

Implicit styles are instead managed via the `WithImplicitStyles` property on
`CupertinoResources` (code-behind approach).

### Implicit Styles (via `WithImplicitStyles=true`)

When enabled, these styles become TargetType defaults:

`CupertinoButtonStyle`, `CupertinoCalendarDatePickerStyle`,
`CupertinoCalendarViewStyle`, `CupertinoCheckBoxStyle`,
`CupertinoComboBoxStyle`, `CupertinoDatePickerStyle`,
`CupertinoDatePickerFlyoutPresenterStyle`, `CupertinoHyperlinkButtonStyle`,
`CupertinoNumberBoxStyle`, `CupertinoPasswordBoxStyle`,
`CupertinoProgressBarStyle`, `CupertinoProgressRingStyle` (non-Desktop),
`CupertinoRadioButtonStyle`, `CupertinoSliderStyle`, `CupertinoBody`
(TextBlock), `CupertinoTextBoxStyle`, `CupertinoToggleSwitchStyle`

---

## Naming Rules (Cupertino-Specific)

1. All keys use `Cupertino` prefix: `Cupertino<Variant><ControlType>Style`
2. Some controls use `DefaultCupertino...` as base (reversed from Material):
   `DefaultCupertinoCalendarDatePickerStyle` → base, consumer BasedOn it
3. Button variants follow iOS paradigm: `CupertinoButtonStyle` (text),
   `CupertinoContainedButtonStyle` (filled)
4. CheckBox variants: primary + secondary (tint color difference)
5. **No `Default` implicit pattern** like Material — uses code-behind approach

---

## Lightweight Styling

Less formalized than Material:

- Per-control resource keys for dimensions and brushes:
  `CupertinoButtonPadding`, `CupertinoButtonCornerRadius`,
  `CupertinoTextBoxCornerRadius`, `CupertinoCheckBoxBorderBrush`, etc.
- Color override via `CupertinoColors.OverrideSource` property
- Font override via `CupertinoFonts.OverrideSource` property
- **No per-state resource key pattern** — relies on opacity constants + template bindings

---

## Theme Entry Point

```xml
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

| Class | Base | Role |
|-------|------|------|
| `CupertinoResources` | `ResourceDictionary` | Loads merged pages, manages implicit styles |
| `CupertinoColors` | `ResourceDictionary` | Loads `ColorPalette.xaml` + `CupertinoColors.xaml` |
| `CupertinoFonts` | `ResourceDictionary` | Loads `Fonts.xaml` |
| `CupertinoConstants` | static class | URI path constants |

---

## File Structure

```
src/library/Uno.Cupertino/
├── CupertinoResources.cs               ← Entry point (extends ResourceDictionary)
├── CupertinoColors.cs                  ← Color loader
├── CupertinoFonts.cs                   ← Font loader
├── CupertinoConstants.cs               ← URI path constants
├── AssemblyInfo.cs
├── LinkerConfig.xamarin.xml
├── cupertino-common.props              ← XamlMerge config
├── Uno.Cupertino.WinUI.csproj          ← WinUI lineage
├── Uno.Cupertino.csproj                ← UWP lineage
├── Assets/
│   └── CupertinoProgressRing.json      ← Lottie spinner animation
├── Styles/
│   ├── Application/
│   │   ├── AnimationConstants.xaml
│   │   ├── ColorPalette.xaml           ← Apple system colors (L+D)
│   │   ├── CupertinoColors.xaml        ← Brush definitions
│   │   ├── Fonts.xaml                  ← SF Pro font family
│   │   └── StateConstants.xaml         ← Opacity constants
│   └── Controls/
│       ├── _Resources.xaml             ← Minimal (Colors + Fonts only)
│       ├── Button.xaml                 ← 2 styles
│       ├── CalendarDatePicker.xaml     ← 2 styles
│       ├── CalendarView.xaml           ← 2+4 internal
│       ├── CheckBox.xaml               ← 2 styles
│       ├── ComboBox.xaml               ← 2 styles
│       ├── DatePicker.xaml             ← 3 styles
│       ├── HyperlinkButton.xaml        ← 1 style
│       ├── NumberBox.xaml              ← 1+3 internal
│       ├── PasswordBox.xaml            ← 1 style
│       ├── ProgressBar.xaml            ← 1 style
│       ├── ProgressRing.xaml           ← 1 style
│       ├── RadioButton.xaml            ← 1 style
│       ├── Slider.xaml                 ← 2+1 internal
│       ├── TextBlock.xaml              ← 12 styles
│       ├── TextBox.xaml                ← 1+1 internal
│       └── ToggleSwitch.xaml           ← 2 styles
└── Generated/
    └── mergedpages.xaml                ← Auto-generated
```

## Comparison with Other Themes

| Aspect | Cupertino | Material | Simple |
|--------|-----------|----------|--------|
| Named styles | ~36 | ~128 | 33 |
| Control files | 16 | 29 | 7 |
| Color roles | ~40 | 33 | 10 |
| Brush resources | ~50 | ~594 | ~90 |
| Typography styles | 12 | 21 | 6 |
| Theme support | Light + Dark | Light + Dark | Light only |
| Lightweight styling | Basic | Extensive | Basic |
| Custom controls | — | Ripple | — |
| Lottie animations | 1 | 2 | — |
| Version tracks | Single | V1 + V2 | Single |
| Entry point | `CupertinoResources` | `MaterialTheme` | `SimpleTheme` |
| Base class | `ResourceDictionary` | `BaseTheme` | `BaseTheme` |
| Font family | SF Pro | Roboto | Inter |
| Implicit styles | Code-behind | XAML + code | XAML + code |

---

## Success Criteria

- **SC-001**: All ~36 consumer styles render correctly in Light and Dark themes
- **SC-002**: `WithImplicitStyles=true` registers all expected implicit styles
- **SC-003**: `CupertinoColors.OverrideSource` replaces color palette correctly
- **SC-004**: `CupertinoFonts.OverrideSource` replaces SF Pro with custom font
- **SC-005**: iOS-native look and feel on all platforms (Skia rendering)
- **SC-006**: Lottie progress ring animates correctly
- **SC-007**: `dotnet build` succeeds with zero errors on all TFMs
