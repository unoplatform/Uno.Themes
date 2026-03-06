# Feature Specification: Material Design 3 (M3) Theme

**Library**: `Uno.Material.WinUI`
**Design Reference**: [Material Design 3](https://m3.material.io)
**Source Path**: `src/library/Uno.Material/`

## Context

The Material theme implements Google's Material Design 3 (M3) guidelines. It is
the **most comprehensive** theme in Uno.Themes — **~128 consumer-facing styles**
across **29 control files** with full Light/Dark theming, extensive lightweight
styling, a custom Ripple control, and two version tracks (V1 legacy, V2 current).

This spec documents the **V2 (current)** implementation. V1 is deprecated.

### Current Implementation Status

| Layer | File(s) | Status |
|-------|---------|--------|
| Color Palette | `v2/ColorPalette.xaml` (via SharedColorPalette) | ✅ Complete |
| Semantic Brushes | `v2/MaterialColors.xaml` (ThemeDictionaries L+D) | ✅ Complete |
| Typography | `v2/Typography.xaml` (484 lines, ThemeDictionaries) | ✅ Complete |
| Animation Constants | `Common/AnimationConstants.xaml` | ✅ Complete |
| TextBox Variables | `Common/TextBoxVariables.xaml` | ✅ Complete |
| Control Styles (V2) | 29 XAML files (~128 consumer + internals) | ✅ Complete |
| Implicit Styles & Aliases | `v2/_Resources.xaml` | ✅ Complete |
| Entry Point | `MaterialTheme.cs` (extends BaseTheme) | ✅ Complete |
| Custom Controls | `Ripple.cs` | ✅ Complete |
| Lottie Assets | `MaterialDeterminate.json`, `MaterialIndeterminate.json` | ✅ Complete |
| XamlMerge | `material-common.props` → v1 + v2 merged pages | ✅ Complete |

---

## Design Tokens

### Color Palette (M3 Tonal Palette)

ThemeDictionaries with **Light** and **Dark** keys:

| Token | Light | Dark | Usage |
|-------|-------|------|-------|
| Primary | `#5946D2` | `#C7BFFF` | Primary actions, FABs |
| OnPrimary | `#FFFFFF` | `#2A009F` | Text on primary |
| PrimaryContainer | `#E5DEFF` | `#4129BA` | Tonal containers |
| OnPrimaryContainer | `#170065` | `#E4DFFF` | Text on containers |
| PrimaryInverse | `#C8BFFF` | `#2A009F` | Inverse primary |
| Secondary | `#6B4EA2` | `#CDC2DC` | Secondary actions |
| OnSecondary | `#FFFFFF` | `#332D41` | Text on secondary |
| SecondaryContainer | `#EBDDFF` | `#433C52` | Chips, filters |
| OnSecondaryContainer | `#220555` | `#EDDFFF` | Text on secondary containers |
| Tertiary | `#0061A4` | `#9FCAFF` | Accent, contrast |
| OnTertiary | `#FFFFFF` | `#003258` | Text on tertiary |
| TertiaryContainer | `#CFE4FF` | `#00497D` | Tertiary containers |
| OnTertiaryContainer | `#001D36` | `#D1E4FF` | Text on tertiary containers |
| Error | `#B3261E` | `#FFB4AB` | Errors, destructive |
| OnError | `#FFFFFF` | `#690005` | Text on error |
| ErrorContainer | `#F9DEDC` | `#93000A` | Error containers |
| OnErrorContainer | `#410E0B` | `#FFDAD6` | Text on error containers |
| Background | `#FCFBFF` | `#1C1B1F` | Page backgrounds |
| OnBackground | `#1C1B1F` | `#E5E1E6` | Text on background |
| Surface | `#FFFFFF` | `#302D37` | Cards, dialogs |
| OnSurface | `#1C1B1F` | `#E6E1E5` | Primary text |
| SurfaceVariant | `#F2EFF5` | `#47464F` | Inputs, chips |
| OnSurfaceVariant | `#8A8494` | `#C9C5D0` | Secondary text |
| SurfaceInverse | `#313033` | `#E6E1E5` | Snackbar backgrounds |
| OnSurfaceInverse | `#F4EFF4` | `#1C1B1F` | Text on inverse |
| SurfaceTint | `#5946D2` | `#C7BFFF` | Tint overlays |
| Outline | `#79747E` | `#928F99` | Borders, dividers |
| OutlineVariant | `#C9C5D0` | `#57545D` | Subtle borders |
| Shadow | `#33000000` | `#33000000` | Drop shadows |

### Brush Generation Pattern

For **each** of the 33 color roles, **9 opacity variants** are generated:

| Suffix | Opacity |
|--------|---------|
| `{Role}Brush` | 1.0 |
| `{Role}HoverBrush` | 0.08 |
| `{Role}FocusedBrush` | 0.12 |
| `{Role}PressedBrush` | 0.12 |
| `{Role}DraggedBrush` | 0.16 |
| `{Role}SelectedBrush` | 0.08 |
| `{Role}MediumBrush` | 0.64 |
| `{Role}LowBrush` | 0.32 |
| `{Role}DisabledBrush` | 0.12 |

**Total: ~594 brush resources** (33 roles × 9 variants × 2 themes)

### Typography (M3 Type Scale — 21 styles)

| Style Key | Font | Weight | Size | Tracking |
|-----------|------|--------|------|----------|
| `MaterialDisplayLarge` | Roboto | Normal | 57 | -17 |
| `MaterialDisplayMedium` | Roboto | Normal | 45 | — |
| `MaterialDisplaySmall` | Roboto | Normal | 36 | — |
| `MaterialHeadlineLarge` | Roboto | Normal | 32 | — |
| `MaterialHeadlineMedium` | Roboto | Normal | 28 | — |
| `MaterialHeadlineSmall` | Roboto | Normal | 24 | — |
| `MaterialTitleLarge` | Roboto | Normal | 22 | — |
| `MaterialTitleMedium` | Roboto | Medium | 16 | — |
| `MaterialTitleSmall` | Roboto | Medium | 14 | — |
| `MaterialLabelLarge` | Roboto | Medium | 14 | 7 |
| `MaterialLabelMedium` | Roboto | Medium | 12 | 41 |
| `MaterialLabelSmall` | Roboto | Medium | 11 | 45 |
| `MaterialLabelExtraSmall` | Roboto | Normal | 11 | 7 |
| `MaterialBodyLarge` | Roboto | Medium | 16 | 9 |
| `MaterialBodyMedium` | Roboto | Medium | 14 | 17 |
| `MaterialBodySmall` | Roboto | Medium | 12 | 33 |
| `MaterialCaptionLarge` | Roboto | Medium | 13 | -3 |
| `MaterialCaptionMedium` | Roboto | Medium | 12 | 33 |
| `MaterialCaptionSmall` | Roboto | Medium | 11 | 7 |
| `MaterialBaseTextBlockStyle` | *(base)* | — | — | — |
| `MaterialDefaultTextBlockStyle` | *(implicit)* | = BodyMedium | — |

**Font Family**: `MaterialRegularFontFamily` = Roboto

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
| `TextBoxOutlinedCorderRadius` | 4 | TextBox |
| `TextBoxFocusStrokeWidth` | 2 | TextBox |
| `MaterialContentDialogMinWidth` | 288 | ContentDialog |
| `MaterialContentDialogMaxWidth` | 560 | ContentDialog |
| `MaterialContentDialogCornerRadius` | 28 | ContentDialog |

### Interaction States (M3 State Layers)

| State | Opacity | Visual Treatment |
|-------|---------|-----------------|
| Normal | 0 | Base appearance |
| Hover | 0.08 | State layer overlay |
| Focused | 0.12 | State layer + focus indicator |
| Pressed | 0.12 | State layer + Ripple |
| Dragged | 0.16 | State layer overlay |
| Disabled | 0.12 bg + 0.38 content | Reduced opacity |

---

## Control Style Inventory (V2 — ~128 Consumer Styles)

### Button (8 consumer + 2 content dialog) — `Button.xaml`

| Style Key | M3 Name | Description |
|-----------|---------|-------------|
| `MaterialBaseButtonStyle` | *(base)* | Shared setters |
| `MaterialFilledButtonStyle` | Filled | Primary CTA |
| `MaterialElevatedButtonStyle` | Elevated | Surface with shadow |
| `MaterialFilledTonalButtonStyle` | Filled Tonal | Secondary container |
| `MaterialOutlinedButtonStyle` | Outlined | Border only |
| `MaterialTextButtonStyle` | Text | Text only |
| `MaterialIconButtonStyle` | Icon | Icon only (round) |
| `MaterialDefaultButtonStyle` | *(implicit)* | = Filled |
| `MaterialContentDialogButtonStyle` | *(internal)* | Dialog action button |
| `MaterialContentDialogDefaultButtonStyle` | *(internal)* | Dialog default action |

### AppBarButton (2) — `AppBarButton.xaml`

| Style Key | Description |
|-----------|-------------|
| `MaterialAppBarButtonStyle` | AppBar action button |
| `MaterialDefaultAppBarButtonStyle` | *(implicit)* |

### CalendarDatePicker (2) — `CalendarDatePicker.xaml`

| Style Key | Description |
|-----------|-------------|
| `MaterialCalendarDatePickerStyle` | Calendar date selector |
| `MaterialDefaultCalendarDatePickerStyle` | *(implicit)* |

### CalendarView (2+3 internal) — `CalendarView.xaml`

| Style Key | Description |
|-----------|-------------|
| `MaterialCalendarViewStyle` | Calendar grid |
| `MaterialDefaultCalendarViewStyle` | *(implicit)* |
| + `MaterialWeekDayNameStyle`, `MaterialNavigationButtonStyle`, `MaterialScrollViewerStyle` | *(internal)* |

### CheckBox (2) — `CheckBox.xaml`

| Style Key | Description |
|-----------|-------------|
| `MaterialCheckBoxStyle` | M3 checkbox |
| `MaterialDefaultCheckBoxStyle` | *(implicit)* |

### ComboBox (3) — `ComboBox.xaml`

| Style Key | Description |
|-----------|-------------|
| `MaterialComboBoxStyle` | Dropdown selector |
| `MaterialComboBoxItemStyle` | Dropdown item |
| `MaterialDefaultComboBoxStyle` | *(implicit)* |

### CommandBar (3) — `CommandBar.xaml`

| Style Key | Description |
|-----------|-------------|
| `MaterialBaseCommandBarStyle` | *(base)* |
| `MaterialCommandBarStyle` | Top app bar |
| `MaterialDefaultCommandBarStyle` | *(implicit)* |

### ContentDialog (2+2 button) — `ContentDialog.xaml`

| Style Key | Description |
|-----------|-------------|
| `MaterialContentDialogStyle` | M3 dialog |
| `MaterialDefaultContentDialogStyle` | *(implicit)* |

### DatePicker (5) — `DatePicker.xaml`

| Style Key | Description |
|-----------|-------------|
| `MaterialDatePickerStyle` | Date picker |
| `MaterialDatePickerFlyoutButtonStyle` | Picker trigger button |
| `MaterialDatePickerFlyoutPresenterStyle` | Flyout container |
| `MaterialDefaultDatePickerStyle` | *(implicit)* |
| `MaterialDefaultDatePickerFlyoutPresenterStyle` | *(implicit)* |

### FloatingActionButton (12) — `FloatingActionButton.xaml`

| Style Key | M3 Name |
|-----------|---------|
| `MaterialFabStyle` | FAB (primary) |
| `MaterialSurfaceFabStyle` | FAB (surface) |
| `MaterialSecondaryFabStyle` | FAB (secondary) |
| `MaterialTertiaryFabStyle` | FAB (tertiary) |
| `MaterialSmallFabStyle` | Small FAB (primary) |
| `MaterialSurfaceSmallFabStyle` | Small FAB (surface) |
| `MaterialSecondarySmallFabStyle` | Small FAB (secondary) |
| `MaterialTertiarySmallFabStyle` | Small FAB (tertiary) |
| `MaterialLargeFabStyle` | Large FAB (primary) |
| `MaterialSurfaceLargeFabStyle` | Large FAB (surface) |
| `MaterialSecondaryLargeFabStyle` | Large FAB (secondary) |
| `MaterialTertiaryLargeFabStyle` | Large FAB (tertiary) |

### Flyout / MenuFlyout (7+7 defaults) — `Flyout.xaml`

| Style Key | Description |
|-----------|-------------|
| `MaterialFlyoutPresenterStyle` | Generic flyout container |
| `MaterialContentFlyoutPresenterStyle` | Content flyout |
| `MaterialMenuFlyoutPresenterStyle` | Menu flyout container |
| `MaterialMenuFlyoutItemStyle` | Menu item |
| `MaterialToggleMenuFlyoutItemStyle` | Toggle menu item |
| `MaterialRadioMenuFlyoutItemStyle` | Radio menu item |
| `MaterialMenuFlyoutSubItemStyle` | Sub-menu item |
| `MaterialMenuFlyoutSeparatorStyle` | Menu separator |
| + 7 Default* variants | *(implicit)* |

### HyperlinkButton (3) — `HyperlinkButton.xaml`

| Style Key | Description |
|-----------|-------------|
| `MaterialHyperlinkButtonStyle` | Primary link |
| `MaterialSecondaryHyperlinkButtonStyle` | Secondary link |
| `MaterialDefaultHyperlinkButtonStyle` | *(implicit)* |

### ListView (4) — `ListView.xaml`

| Style Key | Description |
|-----------|-------------|
| `MaterialListViewStyle` | List container |
| `MaterialListViewItemStyle` | List item |
| `MaterialDefaultListViewStyle` | *(implicit)* |
| `MaterialDefaultListViewItemStyle` | *(implicit)* |

### MediaPlayerElement (2+5 internal) — `MediaPlayerElement.xaml`

| Style Key | Description |
|-----------|-------------|
| `MaterialMediaTransportControlsStyle` | Media controls |
| `MaterialDefaultMediaTransportControlsStyle` | *(implicit)* |
| + 5 internal styles | *(internal)* |

### NavigationView (6+6 internal) — `NavigationView.xaml`

| Style Key | Description |
|-----------|-------------|
| `MaterialNavigationViewStyle` | Nav drawer |
| `MaterialNavigationViewItemStyle` | Nav item |
| `MaterialNavigationViewIconButtonStyle` | Icon-only button |
| `MaterialNavigationViewIconTextButtonStyle` | Icon + text button |
| `MaterialDefaultNavigationViewStyle` | *(implicit)* |
| `MaterialDefaultNavigationViewItemStyle` | *(implicit)* |
| + 6 internal styles | *(internal)* |

### PasswordBox (4) — `PasswordBox.xaml`

| Style Key | Description |
|-----------|-------------|
| `MaterialFilledPasswordBoxStyle` | Filled password |
| `MaterialOutlinedPasswordBoxStyle` | Outlined password |
| `MaterialRevealButtonStyle` | Eye toggle |
| `MaterialDefaultPasswordBoxStyle` | *(implicit)* = Filled |

### PipsPager (2+6 internal) — `PipsPager.xaml` + `PipsPager.Base.xaml`

| Style Key | Description |
|-----------|-------------|
| `MaterialPipsPagerStyle` | Page indicator dots |
| `MaterialDefaultPipsPagerStyle` | *(implicit)* |
| + 6 internal button styles | *(internal)* |

### ProgressBar (2) — `ProgressBar.xaml`

| Style Key | Description |
|-----------|-------------|
| `MaterialProgressBarStyle` | Linear progress |
| `MaterialDefaultProgressBarStyle` | *(implicit)* |

### ProgressRing (2) — `ProgressRing.xaml`

| Style Key | Description |
|-----------|-------------|
| `MaterialProgressRingStyle` | Circular progress |
| `MaterialDefaultProgressRingStyle` | *(implicit)* |

### RadioButton (2) — `RadioButton.xaml`

| Style Key | Description |
|-----------|-------------|
| `MaterialRadioButtonStyle` | M3 radio |
| `MaterialDefaultRadioButtonStyle` | *(implicit)* |

### RatingControl (3) — `RatingControl.xaml`

| Style Key | Description |
|-----------|-------------|
| `MaterialRatingControlStyle` | Star rating |
| `SecondaryRatingControlStyle` | Secondary variant |
| `MaterialDefaultRatingControlStyle` | *(implicit)* |

### Ripple (2) — `Ripple.xaml`

| Style Key | Description |
|-----------|-------------|
| `MaterialRippleStyle` | Touch ripple effect |
| `MaterialDefaultRippleStyle` | *(implicit)* |

### Slider (3) — `Slider.xaml`

| Style Key | Description |
|-----------|-------------|
| `MaterialSliderStyle` | Range slider |
| `MaterialSliderThumbStyle` | Slider thumb |
| `MaterialDefaultSliderStyle` | *(implicit)* |

### TextBox (4) — `TextBox.xaml`

| Style Key | Description |
|-----------|-------------|
| `MaterialFilledTextBoxStyle` | Filled input |
| `MaterialOutlinedTextBoxStyle` | Outlined input |
| `MaterialDeleteButtonStyle` | Clear button |
| `MaterialDefaultTextBoxStyle` | *(implicit)* = Filled |

### ToggleButton (3) — `ToggleButton.xaml`

| Style Key | Description |
|-----------|-------------|
| `MaterialTextToggleButtonStyle` | Text toggle |
| `MaterialIconToggleButtonStyle` | Icon toggle |
| `MaterialDefaultToggleButtonStyle` | *(implicit)* |

### ToggleSwitch (2) — `ToggleSwitch.xaml`

| Style Key | Description |
|-----------|-------------|
| `MaterialToggleSwitchStyle` | M3 switch |
| `MaterialDefaultToggleSwitchStyle` | *(implicit)* |

---

## _Resources.xaml — Aliases & Implicit Styles

### Implicit Styles (~36)

One per TargetType for all 29 controls + internal types (Ripple, etc.)

### Theme-Agnostic Aliases (~50)

`FilledButtonStyle` → `MaterialFilledButtonStyle`,
`OutlinedTextBoxStyle` → `MaterialOutlinedTextBoxStyle`,
`FabStyle` → `MaterialFabStyle`, etc.

### Typography Aliases (~22)

`DisplayLarge` → `MaterialDisplayLarge`, etc.

---

## Naming Rules (Material-Specific)

1. All keys use `Material` prefix: `Material<Variant><ControlType>Style`
2. Every control has `MaterialDefault<ControlType>Style` for implicit base
3. Variant names follow M3: `Filled`, `Elevated`, `FilledTonal`, `Outlined`,
   `Text`, `Icon`
4. FAB variants: `{Size}{Color}` → `MaterialSmallSecondaryFabStyle`
5. Base styles: `MaterialBase<ControlType>Style`

---

## Lightweight Styling

Material has the **most extensive** lightweight styling in Uno.Themes:

- Per-control `ThemeDictionary` blocks at top of each style file
- Key naming: `{ControlName}{Property}{State}` — e.g.,
  `FilledButtonForegroundPointerOver`, `CheckBoxBackgroundCheckedDisabled`
- All brushes reference semantic tokens via `{ThemeResource}`
- State variants: Normal, PointerOver, Pressed, Disabled, Focused, Selected,
  Checked, CheckedPointerOver, CheckedPressed, CheckedDisabled
- Override via `ColorOverrideSource` / `ColorOverrideDictionary` on `MaterialTheme`

---

## Theme Entry Point

```xml
<MaterialTheme xmlns="using:Uno.Material"
               ColorOverrideSource="ms-appx:///Styles/MyColors.xaml"
               FontOverrideSource="ms-appx:///Styles/MyFonts.xaml" />
```

### Class Hierarchy

- `MaterialTheme` extends `BaseTheme` (from `Uno.Themes.WinUI`)
- Loads `mergedpages.v2.xaml` (all V2 control styles)
- Loads `Fonts.xaml` (Roboto)
- Color loading via `BaseTheme` → `SharedColorPalette` + `SharedColors`

### Version History

| Version | Status | Notes |
|---------|--------|-------|
| V1 | Deprecated | Fewer controls, M2 tokens |
| V2 | **Current** | Full M3 spec, 29 controls |

---

## File Structure

```
src/library/Uno.Material/
├── MaterialTheme.cs                    ← Entry point (extends BaseTheme)
├── MaterialConstants.cs                ← URI path constants (v1 + v2 + common)
├── MaterialColors.cs                   ← Alias → V2
├── MaterialColorsV2.cs                 ← V2 color loader
├── MaterialFonts.cs                    ← Font loader
├── MaterialResources.cs                ← Alias → V2
├── MaterialResourcesV2.cs             ← V2 resource aggregator
├── Controls/
│   └── Ripple.cs                       ← Custom ripple control
├── Converters/
│   └── FromTextBoxEmptyStringToValueConverter.cs
├── Assets/
│   ├── MaterialDeterminate.json        ← Lottie (ProgressBar)
│   └── MaterialIndeterminate.json      ← Lottie (ProgressBar)
├── Styles/
│   ├── Application/
│   │   ├── Common/
│   │   │   ├── AnimationConstants.xaml
│   │   │   ├── Fonts.xaml              ← Roboto
│   │   │   └── TextBoxVariables.xaml
│   │   ├── v1/                         ← Deprecated
│   │   │   ├── ColorPalette.xaml
│   │   │   └── MaterialColors.xaml
│   │   └── v2/
│   │       └── Typography.xaml         ← 484 lines, type scale variables
│   └── Controls/
│       ├── v1/ (27 files — deprecated)
│       └── v2/ (29 files — current)
│           ├── _Resources.xaml         ← Implicit styles + aliases
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
    ├── mergedpages.v1.xaml             ← V1 (deprecated)
    └── mergedpages.v2.xaml             ← V2 (current)
```

---

## Success Criteria

- **SC-001**: All ~128 consumer styles render correctly in Light and Dark themes
- **SC-002**: All ~594 brush resources resolve correctly in both theme modes
- **SC-003**: Lightweight styling overrides work for all controls
- **SC-004**: `ColorOverrideSource` cascades correctly to all brushes
- **SC-005**: `FontOverrideSource` replaces Roboto with custom font
- **SC-006**: Theme-agnostic aliases resolve correctly
- **SC-007**: Ripple effect renders on touch/click for interactive controls
- **SC-008**: `dotnet build` succeeds with zero errors on all TFMs
