# Simple: state colours that leave the control's own role (#1784)

Issue: https://github.com/unoplatform/Uno.Themes/issues/1784

## Problem

Several Simple lightweight-styling keys paint a hover / pressed / checked state with a
different colour role than the resting state:

- `FilledButtonBackground{PointerOver,Pressed}` → `PrimaryVariantDarkBrush` while the label stays
  `OnPrimaryBrush`. The seed generator emits `PrimaryVariantDark` at tone 30 in **both** themes and
  `OnPrimary` at tone 20 in dark, so the label is dark-on-dark in any seeded dark theme.
- `FilledTonalButtonBackgroundPointerOver` → `PrimaryContainerBrush`,
  `FilledTonalButtonBackgroundPressed` → `TertiaryContainerBrush`: with a coloured palette the button
  changes hue (a brown button turns green when pressed).

The stock grey palette hides both: every role is a grey, so each state reads as a shade.

## Approach

Mirror Material v2: keep the fill on the resting role in every interactive state and paint the
state as a translucent **state layer** (`On<Role>` at the shared Hover/Pressed opacity) over it.

- Template: `Root` no longer carries `Padding`; a `StateLayer` `Border` sits under the content,
  and the content carries the padding as a margin.
- Keys: `…BackgroundPointerOver` / `…BackgroundPressed` now resolve to the resting brush. New
  additive keys `<Control>StateLayerBackground{PointerOver,Pressed}` carry the tint, named as in
  Material v2.
- Behaviour change for the stock grey palette: hover/pressed become a light `On<Role>` tint instead
  of a swap to a darker/lighter grey.

## Plan

- [x] Red test: seeded palette (issue's seeds + dark `TertiaryContainerColor` override), Light and
      Dark, each button style in PointerOver/Pressed: fill stays the resting fill and the label
      contrasts with fill + state layer.
- [x] Commit 1 (issue scope): `FilledButtonStyle`, `FilledTonalButtonStyle`, and the icon styles
      sharing their keys (`IconButtonStyle`, `IconButtonNeutralStyle`).
- [x] Update `Given_SemanticResources.When_LightweightBrushesResolve_Then_ButtonStateValuesArePreserved`.
- [x] Further same-bug fixes (follow-up commit), see sweep below. Done per control:
  - TextToggleButton / IconToggleButton, CheckBox, ComboBoxItem, ListViewItem: state layer + `*StateLayerBackground*` keys.
  - ToggleSwitch: one `SwitchStateLayer` over both tracks (`ToggleSwitchStateLayerFillPressed` = `OnSurfacePressed`).
  - MenuFlyout items (4 templates) and Expander headers (2): state layer with template brushes (they had no keys).
  - Slider: pressed keeps `Primary` (nothing sits on the thumb, and the slider's states can't reach into the thumb template).
  - CalendarView selected hover / pressed border, RatingControl hovered selected stars: `Primary`.
  - RadioButton: `…Pressed` keys are unused by the template; pointed at the resting roles for consistency.
- [x] Left as is: AppBarButton, TextButton, NavigationViewItem and PipsPager pressed/selected use
      `PrimaryContainer` under a label that still pairs with it (same colour family, readable).
- [x] Gates: XAML styler, `dotnet format`, `GenerateStyleDocs.cs`, `check-xaml-resource-keys.ps1`
      (needs WinAppSDK restore, Windows only), runtime tests Light + Dark.
- [x] Docs: note under the generated tables in `doc/styles/simple/Button.md` (the generated tables pick up the new keys).

## Sweep: other Simple state keys that change role

Same bug (a `Primary` fill whose state switches to `PrimaryVariantDark`, or a pressed state that
switches to `TertiaryContainer`):

| Control | Keys |
|---|---|
| CheckBox | `CheckBoxBackground{Checked,Indeterminate}Pressed` → `PrimaryVariantDark` |
| ComboBoxItem | `ComboBoxItemBackgroundSelected{PointerOver,Pressed}` → `PrimaryVariantDark`; `ComboBoxItemBackgroundPressed` → `TertiaryContainer` |
| RadioButton | `RadioButtonOuterEllipseCheckedFillPressed` (+ stroke) → `PrimaryVariantDark` |
| Slider | `SliderThumbBackgroundPressed`, `SliderTrackValueFillPressed` → `PrimaryVariantDark` |
| ToggleButton | `TextToggleButtonBackgroundChecked{PointerOver,Pressed}` → `PrimaryVariantDark`; `…{,Indeterminate}PointerOver` → `PrimaryContainer`; `…{,Indeterminate}Pressed` → `TertiaryContainer` |
| ToggleSwitch | `ToggleSwitchKnobBoundsFillPressed` → `PrimaryVariantDark`; `ToggleSwitchOuterBorderFillPressed` → `TertiaryContainer` |
| MenuFlyout | item `Pressed` sets `PrimaryVariantDarkBrush` on the item under an `OnPrimary` label, directly in the template (no lightweight key) |
| CalendarView | `SelectedHoverBorderBrush` / `SelectedPressedBorderBrush` → `PrimaryVariantDark` |
| Expander | header pressed → `TertiaryContainer`, pointer-over → `PrimaryContainer` |

Also fixed in the follow-up (they were judgment calls in the first sweep): unselected
`ComboBoxItem` / `ListViewItem` hover and pressed (transparent + tint instead of `SurfaceVariant` /
`TertiaryContainer` / `Outline`), `CheckBoxBackgroundUncheckedPressed`, and the unused RadioButton
`…Pressed` keys. Left as is: `AppBarButton`, `TextButton`, `NavigationViewItem` and `PipsPager`
pressed/selected `PrimaryContainer`, whose labels still pair with it.

Feedback removed on purpose (nothing sits on these, and a darker shade disappears on a dark
surface): Slider thumb and value track no longer change when pressed, hovered selected
RatingControl stars stay `Primary`, and CalendarView's selected-day border no longer changes on
hover / press.

Not covered here: on Uno, a `VisualState` setter resolves `{ThemeResource}` from the app theme, so a
seeded theme merged below app level (`<Grid.Resources><SimpleTheme .../>`) paints its controls'
states in the app palette. Pre-existing and not specific to these keys.

## Review

- Red on master: all 12 `Given_ButtonStateColors` rows failed. In dark, the filled button went from
  `#EABDA0` (Primary, tone 80) to `#5F402A` (PrimaryVariantDark, tone 30) under a tone-20 label. The
  tonal button pressed to `#34503A`, the green tertiary-container override.
- The test seeds the **application** theme (restored in `finally`, as `Given_DesignTokens` does).
  On Uno, a visual-state setter's `{ThemeResource}` resolves from the app's theme, not from a
  theme merged into the test container, so a scoped seeded theme reports stock grey state colours.
- The generator is not at fault: `PrimaryVariantDark` is a dark role in dark mode in the shared
  palette (`#4128BA`) and Material v1. Simple's stock grey palette is the outlier (`#D9D9D9`), which
  is why the styles used it as a "slightly darker Primary".
- The same misuse remains in the controls listed in the sweep. Each needs its own state layer
  (the fill element differs per template), so they are not part of the issue commit.
- Full Simple runtime suite on desktop (macOS, Release): 366/366 pass in Light and 366/366 in Dark.
  `check-xaml-resource-keys.ps1` not run and nothing run on the WinAppSDK head (needs Windows); nine
  templates were restructured, so run both there before merging.
