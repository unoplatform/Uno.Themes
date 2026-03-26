# Feature Specification: Simple Design System (SDS) Theme

**Library**: `Uno.Simple.WinUI`
**Design Reference**: Simple Design System (Figma)
**Source Path**: `src/library/Uno.Simple.WinUI/`

## Context

The Simple Design System is a clean, minimalist design language that prioritizes
clarity and usability. It uses a neutral color palette, generous spacing, and
subtle interaction states. The `Uno.Simple.WinUI` library provides WinUI
control styles that implement this system.

### Current Implementation Status

| Layer | File(s) | Status |
|-------|---------|--------|
| Color Palette | `ColorPalette.xaml` (164 lines) | ✅ Complete |
| Semantic Brushes | `SimpleColors.xaml` (180 lines, x:Class) | ✅ Complete |
| Typography | `Common/Fonts.xaml` (Inter) | ✅ Complete |
| Animation Constants | `Common/AnimationConstants.xaml` | ✅ Complete |
| Thickness Resources | `Common/Thickness.xaml` | ✅ Complete |
| Control Styles | 21 XAML files (75+ styles) | ✅ Complete |
| Implicit Styles & Aliases | `_Resources.xaml` | ✅ Complete |
| Entry Point | `SimpleTheme.cs` (extends BaseTheme) | ✅ Complete |
| XamlMerge | `simple-common.props` → `mergedpages.xaml` | ✅ Complete |

---

## Design Tokens

### Color Palette

Single palette (light-only design — no ThemeDictionaries Light/Dark split).

| Token | Hex | Usage |
|-------|-----|-------|
| Primary | `#2c2c2c` | Primary actions, filled buttons |
| OnPrimary | `#f5f5f5` | Text/icons on primary surfaces |
| Surface | `#ffffff` | Page backgrounds, card surfaces |
| SurfaceVariant | `#f5f5f5` | Secondary surfaces, input backgrounds |
| OnSurface | `#1e1e1e` | Primary text on surface |
| OnSurfaceMedium | `#757575` | Secondary text, placeholders |
| Outline | `#d9d9d9` | Borders, dividers |
| OutlineVariant | `#e8e8e8` | Subtle borders |
| Error | `#dc2626` | Error states, destructive actions |
| OnError | `#ffffff` | Text on error surfaces |
| Disabled | varies | Disabled backgrounds/foregrounds |

Additional SDS-specific tokens in ColorPalette.xaml for buttons, sliders,
inputs, cards, badges, tags, navigation components.

### Typography

| Style Key | Font | Size | Weight |
|-----------|------|------|--------|
| `SimpleDisplayLarge` | Inter | 36px | Bold |
| `SimpleHeadlineMedium` | Inter | 24px | SemiBold |
| `SimpleTitleMedium` | Inter | 18px | SemiBold |
| `SimpleBodyMedium` | Inter | 14–16px | Normal |
| `SimpleLabelMedium` | Inter | 12–14px | Medium |
| `SimpleCaptionMedium` | Inter | 10–12px | Normal |

**Font Family**: `SimpleFontFamily` = `Inter`

### Spacing Scale

`4, 8, 12, 16, 24, 32, 48, 64` — 4px-based.

### Corner Radius

| Context | Value |
|---------|-------|
| Buttons | `6px` |
| Inputs / TextBox | `6px` |
| Cards | `8px` |
| Avatars (circle) | `999px` |
| Avatars (square) | `8px` |

### Interaction States

SDS uses **opacity-based** state overlays (no ripple):

| State | Visual Treatment |
|-------|-----------------|
| Normal | Base appearance |
| PointerOver | Subtle background opacity change or border highlight |
| Pressed | Slightly darker opacity overlay |
| Focused | Outline/border emphasis (keyboard focus) |
| Disabled | `Opacity="0.4"` on entire control |

---

## Control Style Inventory (75+ Styles)

### Button (18 styles) — `Button.xaml`

| Style Key | Figma Name | Description |
|-----------|-----------|-------------|
| `SimplePrimaryButtonStyle` | Button/Primary | Filled dark button, main CTA |
| `SimpleLightButtonStyle` | Button/Light | Light-colored filled button |
| `SimpleNeutralButtonStyle` | Button/Neutral | Outlined border button |
| `SimpleSubtleButtonStyle` | Button/Subtle | Text-only button |
| `SimplePrimaryButtonSmallStyle` | Button/Primary/Small | Small primary |
| `SimpleNeutralButtonSmallStyle` | Button/Neutral/Small | Small neutral |
| `SimpleSubtleButtonSmallStyle` | Button/Subtle/Small | Small subtle |
| `SimplePrimaryButtonLargeStyle` | Button/Primary/Large | Large primary |
| `SimpleNeutralButtonLargeStyle` | Button/Neutral/Large | Large neutral |
| `SimpleSubtleButtonLargeStyle` | Button/Subtle/Large | Large subtle |
| `SimplePrimaryErrorButtonStyle` | Button/Error/Primary | Destructive filled |
| `SimplePrimaryErrorButtonSmallStyle` | Button/Error/Primary/Small | Small destructive |
| `SimpleSubtleErrorButtonStyle` | Button/Error/Subtle | Destructive text-only |
| `SimpleSubtleErrorButtonSmallStyle` | Button/Error/Subtle/Small | Small destructive text |
| `SimpleLightPricingCardButtonStyle` | Button/PricingCard/Light | Pricing card variant |
| `SimpleBrandedPricingCardButtonStyle` | Button/PricingCard/Branded | Branded pricing variant |
| `SimpleSelectedFilterButtonStyle` | Button/Filter/Selected | Filter active state |
| `SimpleUnselectedFilterButtonStyle` | Button/Filter/Unselected | Filter inactive state |

### TextBox (4 styles) — `TextBox.xaml`

| Style Key | Figma Name | Description |
|-----------|-----------|-------------|
| `SimpleTextBoxStyle` | Input Field | Default input (single SDS Input design) |
| `SimpleTextBoxErrorStyle` | Input Field/Error | Danger border variant |
| `SimpleTextBoxSmallStyle` | Input Field/Small | Compact input |
| `SimpleOutlinedTextBoxStyle` | Input Field/Outlined | Outlined border, surface background |

### ComboBox (3 styles) — `ComboBox.xaml`

| Style Key | Figma Name | Description |
|-----------|-----------|-------------|
| `SimpleSelectFieldStyle` | Select Field | Default dropdown |
| `SimpleSelectFieldItemStyle` | Select Field/Item | Dropdown item |
| `SimpleSelectFieldErrorStyle` | Select Field/Error | Error state dropdown |

### CheckBox (1 style) — `CheckBox.xaml`

| Style Key | Figma Name |
|-----------|-----------|
| `SimpleCheckBoxStyle` | Checkbox Field |

### RadioButton (1 style) — `RadioButton.xaml`

| Style Key | Figma Name |
|-----------|-----------|
| `SimpleRadioButtonStyle` | Radio Field |

### ToggleSwitch (1 style) — `ToggleSwitch.xaml`

| Style Key | Figma Name |
|-----------|-----------|
| `SimpleToggleSwitchStyle` | Switch Field |

### PersonPicture (6 styles) — `PersonPicture.xaml`

| Style Key | Figma Name | Description |
|-----------|-----------|-------------|
| `SimplePersonPictureStyle` | Avatar | Default circle avatar |
| `SimplePersonPictureSmallStyle` | Avatar/Small | Small circle |
| `SimplePersonPictureLargeStyle` | Avatar/Large | Large circle |
| `SimplePersonPictureSquareStyle` | Avatar/Square | Square avatar (8px radius) |
| `SimplePersonPictureSquareSmallStyle` | Avatar/Square/Small | Small square |
| `SimplePersonPictureSquareLargeStyle` | Avatar/Square/Large | Large square |

### PasswordBox (2 styles) — `PasswordBox.xaml`

| Style Key | Description |
|-----------|-------------|
| `SimplePasswordBoxStyle` | Default filled password input |
| `SimpleOutlinedPasswordBoxStyle` | Outlined border, surface background |

### ToggleButton (4 styles) — `ToggleButton.xaml`

| Style Key | Description |
|-----------|-------------|
| `SimpleDefaultToggleButtonStyle` | Default toggle button |
| `SimpleSmallToggleButtonStyle` | Small size variant |
| `SimpleMediumToggleButtonStyle` | Medium size variant |
| `SimpleIconToggleButtonStyle` | Icon-only toggle (glyph content) |

### HyperlinkButton (3 styles) — `HyperlinkButton.xaml`

| Style Key | Description |
|-----------|-------------|
| `SimpleHyperlinkButtonStyle` | Primary hyperlink |
| `SimpleSecondaryHyperlinkButtonStyle` | Secondary/muted hyperlink |
| `SimpleDefaultHyperlinkButtonStyle` | Default implicit style |

### FlyoutPresenter (2 styles) — `Flyout.xaml`

| Style Key | Description |
|-----------|-------------|
| `SimpleFlyoutPresenterStyle` | Flyout container with rounded corners + shadow |
| `SimpleDefaultFlyoutPresenterStyle` | Default implicit style |

### MenuFlyout (12 styles) — `MenuFlyout.xaml`

| Style Key | Description |
|-----------|-------------|
| `SimpleMenuFlyoutPresenterStyle` | Menu container |
| `SimpleMenuFlyoutItemStyle` | Standard menu item |
| `SimpleToggleMenuFlyoutItemStyle` | Checkable menu item |
| `SimpleMenuFlyoutSubItemStyle` | Submenu/cascading item |
| `SimpleMenuFlyoutSeparatorStyle` | Divider line |
| `SimpleRadioMenuFlyoutItemStyle` | Radio-select menu item (Ellipse indicator) |
| + 6 `SimpleDefault*` implicit style aliases | |

### NavigationView (4 styles) — `NavigationView.xaml`

| Style Key | Description |
|-----------|-------------|
| `SimpleNavigationViewStyle` | NavigationView via lightweight ThemeResource overrides |
| `SimpleNavigationViewItemStyle` | NavigationViewItem overrides |
| `SimpleDefaultNavigationViewStyle` | Default implicit style |
| `SimpleDefaultNavigationViewItemStyle` | Default implicit style |

### ProgressBar (2 styles) — `ProgressBar.xaml`

| Style Key | Description |
|-----------|-------------|
| `SimpleProgressBarStyle` | Determinate/indeterminate bar with brand color |
| `SimpleDefaultProgressBarStyle` | Default implicit style |

### ProgressRing (2 styles) — `ProgressRing.xaml`

| Style Key | Description |
|-----------|-------------|
| `SimpleProgressRingStyle` | Spinning/determinate ring with brand color |
| `SimpleDefaultProgressRingStyle` | Default implicit style |

### PipsPager (2 styles) — `PipsPager.xaml`

| Style Key | Description |
|-----------|-------------|
| `SimplePipsPagerStyle` | Dot-based page indicator |
| `SimpleDefaultPipsPagerStyle` | Default implicit style |

### RatingControl (2 styles) — `RatingControl.xaml`

| Style Key | Description |
|-----------|-------------|
| `SimpleRatingControlStyle` | Star rating with brand accent |
| `SimpleDefaultRatingControlStyle` | Default implicit style |

---

## _Resources.xaml — Aliases & Implicit Styles

### Implicit Styles (35)

These register default styles for controls when no explicit `Style` is set:

- `Button` → `SimplePrimaryButtonStyle`
- `CheckBox` → `SimpleCheckBoxStyle`
- `ComboBox` → `SimpleSelectFieldStyle`
- `RadioButton` → `SimpleRadioButtonStyle`
- `TextBox` → `SimpleTextBoxStyle`
- `ToggleSwitch` → `SimpleToggleSwitchStyle`
- `PasswordBox` → `SimplePasswordBoxStyle`
- `Slider` → `SimpleSliderStyle`
- `ToolTip` → `SimpleToolTipStyle`
- `TextBlock` → `SimpleBaseTextBlockStyle`
- `ListViewItem` → `SimpleDefaultListViewItemStyle`
- `ListView` → `SimpleDefaultListViewStyle`
- `ContentDialog` → `SimpleDefaultContentDialogStyle`
- `MenuFlyoutItem` → `SimpleDefaultMenuFlyoutItemStyle`
- `MenuFlyoutPresenter` → `SimpleDefaultMenuFlyoutPresenterStyle`
- `MenuFlyoutSeparator` → `SimpleDefaultMenuFlyoutSeparatorStyle`
- `MenuFlyoutSubItem` → `SimpleDefaultMenuFlyoutSubItemStyle`
- `ToggleMenuFlyoutItem` → `SimpleDefaultToggleMenuFlyoutItemStyle`
- `CalendarView` → `SimpleDefaultCalendarViewStyle`
- `AppBarButton` → `SimpleDefaultAppBarButtonStyle`
- `CalendarDatePicker` → `SimpleDefaultCalendarDatePickerStyle`
- `DatePicker` → `SimpleDefaultDatePickerStyle`
- `DatePickerFlyoutPresenter` → `SimpleDefaultDatePickerFlyoutPresenterStyle`
- `AutoSuggestBox` → `SimpleDefaultAutoSuggestBoxStyle`
- `Expander` → `SimpleDefaultExpanderStyle`
- `ToggleButton` → `SimpleDefaultToggleButtonStyle`
- `FlyoutPresenter` → `SimpleDefaultFlyoutPresenterStyle`
- `HyperlinkButton` → `SimpleDefaultHyperlinkButtonStyle`
- `muxc:NavigationView` → `SimpleDefaultNavigationViewStyle`
- `muxc:NavigationViewItem` → `SimpleDefaultNavigationViewItemStyle`
- `muxc:PipsPager` → `SimpleDefaultPipsPagerStyle`
- `muxc:ProgressBar` → `SimpleDefaultProgressBarStyle`
- `muxc:ProgressRing` → `SimpleDefaultProgressRingStyle`
- `muxc:RadioMenuFlyoutItem` → `SimpleDefaultRadioMenuFlyoutItemStyle`
- `RatingControl` → `SimpleDefaultRatingControlStyle`

### Theme-Agnostic Aliases (~65)

Short names without the `Simple` prefix for consumer convenience:

`PrimaryButtonStyle`, `NeutralButtonStyle`, `SubtleButtonStyle`,
`TextBoxStyle`, `SelectFieldStyle`,
`CheckBoxStyle`, `RadioButtonStyle`, `ToggleSwitchStyle`,
`PersonPictureStyle`, `HyperlinkButtonStyle`, `NavigationViewStyle`,
`ProgressBarStyle`, `ProgressRingStyle`, `PipsPagerStyle`,
`RatingControlStyle`, `RadioMenuFlyoutItemStyle`, `FlyoutPresenterStyle`,
`IconToggleButtonStyle`, etc.

### Semantic/Material-Compatibility Aliases (~13)

Map Material naming convention to SDS naming:

`FilledButtonStyle` → `SimplePrimaryButtonStyle`,
`OutlinedButtonStyle` → `SimpleNeutralButtonStyle`,
`TextButtonStyle` → `SimpleSubtleButtonStyle`,
`FilledTextBoxStyle` → `SimpleTextBoxStyle`,
`OutlinedTextBoxStyle` → `SimpleOutlinedTextBoxStyle`,
`FilledPasswordBoxStyle` → `SimplePasswordBoxStyle`,
`OutlinedPasswordBoxStyle` → `SimpleOutlinedPasswordBoxStyle`,
`IconToggleButtonStyle` → `SimpleIconToggleButtonStyle`, etc.

### FAB Aliases (12)

Map Material FAB naming to existing Simple icon button styles:

`FabStyle` → `SimpleIconButtonPrimaryStyle`,
`SecondaryFabStyle` → `SimpleIconButtonNeutralStyle`,
`TertiaryFabStyle` → `SimpleIconButtonSubtleStyle`,
`SurfaceFabStyle` → `SimpleIconButtonNeutralStyle`,
+ Small/Large variants for each.

### Typography Aliases (~19)

Map design-system-agnostic names to SDS typography:

`DisplayLarge` → `SimpleDisplayLarge`, `BodyMedium` → `SimpleBodyMedium`, etc.

### TODO Controls (from _Resources.xaml comments)

The following controls are listed as NOT YET implemented:

CommandBar, MediaPlayerElement

---

## Naming Rules (SDS-Specific)

1. All style keys use `Simple` prefix: `Simple<Variant><ControlType>Style`
2. Names follow **Figma component names**, not WinUI control names:
   - Figma "Input Field" → `SimpleTextBoxStyle`
   - Figma "Select Field" → `SimpleSelectFieldStyle`
   - Figma "Button/Primary" → `SimplePrimaryButtonStyle`
3. Size variants append to name: `SimplePrimaryButtonSmallStyle`
4. Error variants: `SimplePrimaryErrorButtonStyle`

---

## Reference Implementation Pattern

### Entry Point (`SimpleTheme.cs`)

Extends `BaseTheme` (from `Uno.Themes.WinUI`):

```csharp
public partial class SimpleTheme : BaseTheme
{
    public SimpleTheme() : base() { }
    // Loads SimpleColors as ColorOverrideSource
    // Loads mergedpages.xaml via BaseTheme mechanism
    // Loads _Resources.xaml for implicit styles
}
```

### Constants (`SimpleConstants.cs`)

- `PackageName` = `Uno.Simple.WinUI`
- Resource paths for: `ColorPalette`, `SimpleColors`, `Fonts`, `Thickness`,
  `AnimationConstants`, `MergedPages`, `_Resources`

### XamlMerge Configuration

- Input: `Styles/Controls/**/*.xaml` + `Styles/Application/**/*.xaml`
- Excludes: `SimpleColors.xaml`, `ColorPalette.xaml`, `Common/Fonts.xaml`,
  `Common/Thickness.xaml`
- Output: `Generated/mergedpages.xaml` (single file, no versioning)

## File Structure

```
src/library/Uno.Simple.WinUI/
├── SimpleTheme.cs                      ← Entry point (extends BaseTheme)
├── SimpleConstants.cs                  ← Package name + URI paths
├── AssemblyInfo.cs
├── simple-common.props                 ← XamlMerge + platform namespaces
├── Uno.Simple.WinUI.csproj             ← WinUI lineage
├── Styles/
│   ├── Application/
│   │   ├── ColorPalette.xaml           ← 164 lines, raw Color resources
│   │   ├── SimpleColors.xaml           ← 180 lines, SolidColorBrush (x:Class)
│   │   └── Common/
│   │       ├── Fonts.xaml              ← SimpleFontFamily = Inter
│   │       ├── AnimationConstants.xaml
│   │       └── Thickness.xaml
│   └── Controls/
│       ├── _Resources.xaml             ← Implicit styles + aliases
│       ├── Button.xaml                 ← 18 styles
│       ├── CheckBox.xaml               ← 1 style
│       ├── ComboBox.xaml               ← 3 styles
│       ├── Flyout.xaml                 ← 1 style (FlyoutPresenter)
│       ├── HyperlinkButton.xaml        ← 3 styles (Primary, Secondary, Default)
│       ├── MenuFlyout.xaml             ← 7 styles (incl. RadioMenuFlyoutItem)
│       ├── NavigationView.xaml         ← 4 styles (NavigationView + Item)
│       ├── PasswordBox.xaml            ← 2 styles (Filled + Outlined)
│       ├── PersonPicture.xaml          ← 6 styles
│       ├── PipsPager.xaml              ← 2 styles
│       ├── ProgressBar.xaml            ← 2 styles
│       ├── ProgressRing.xaml           ← 2 styles
│       ├── RadioButton.xaml            ← 1 style
│       ├── RatingControl.xaml          ← 2 styles
│       ├── TextBox.xaml                ← 4 styles (incl. Outlined)
│       ├── ToggleButton.xaml           ← 4 styles (incl. IconToggle)
│       └── ToggleSwitch.xaml           ← 1 style
└── Generated/
    └── mergedpages.xaml                ← Auto-generated
```

---

## Success Criteria

- **SC-001**: All 75+ named styles render correctly on desktop, WASM, Android
- **SC-002**: Implicit styles apply correctly when no explicit Style is set
- **SC-003**: Theme-agnostic aliases resolve to the correct Simple styles
- **SC-004**: Legacy Material-compatibility aliases work
- **SC-005**: Typography aliases resolve correctly
- **SC-006**: All interaction states (hover, press, focus, disabled) work
- **SC-007**: `dotnet build` succeeds with zero errors on all TFMs
- **SC-008**: Sample app demonstrates all styles with `XamlDisplay`

---

## Coverage Gap (Controls NOT Yet Styled)

These controls have Material styles but NO Simple equivalent:

| Control | Material Styles | Priority | SDS Figma Equivalent |
|---------|----------------|----------|---------------------|
| CommandBar | 3 | Low | — |
| MediaPlayerElement | 6+ | Low | — |

### Controls Recently Styled (issues #1639–#1650)

| Control | Simple Style(s) | Issue |
|---------|----------------|-------|
| HyperlinkButton | `SimpleHyperlinkButtonStyle`, `SimpleSecondaryHyperlinkButtonStyle` | #1643 |
| PasswordBox (Outlined) | `SimpleOutlinedPasswordBoxStyle` | #1642 |
| TextBox (Outlined) | `SimpleOutlinedTextBoxStyle` | #1641 |
| ProgressBar | `SimpleProgressBarStyle` | #1647 |
| ProgressRing | `SimpleProgressRingStyle` | #1648 |
| NavigationView | `SimpleNavigationViewStyle`, `SimpleNavigationViewItemStyle` | #1646 |
| PipsPager | `SimplePipsPagerStyle` | #1649 |
| RatingControl | `SimpleRatingControlStyle` | #1650 |
| ToggleButton (Icon) | `SimpleIconToggleButtonStyle` | #1640 |
| FlyoutPresenter | `SimpleFlyoutPresenterStyle` | #1645 |
| RadioMenuFlyoutItem | `SimpleRadioMenuFlyoutItemStyle` | #1644 |
| FAB (aliases) | 12 aliases mapped to existing icon button styles | #1639 |
