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

- [ ] Red test: seeded palette (issue's seeds + dark `TertiaryContainerColor` override), Light and
      Dark, each button style in PointerOver/Pressed: fill stays the resting fill and the label
      contrasts with fill + state layer.
- [ ] Commit 1 (issue scope): `FilledButtonStyle`, `FilledTonalButtonStyle`, and the icon styles
      sharing their keys (`IconButtonStyle`, `IconButtonNeutralStyle`).
- [ ] Update `Given_SemanticResources.When_LightweightBrushesResolve_Then_ButtonStateValuesArePreserved`.
- [ ] Further same-bug fixes (separate commits), see sweep below.
- [ ] Gates: XAML styler, `dotnet format`, `GenerateStyleDocs.cs`, `check-xaml-resource-keys.ps1`
      (needs WinAppSDK restore, Windows only), runtime tests Light + Dark.
- [ ] Docs: `doc/simple-controls-styles.md`, `doc/lightweight-styling.md`.

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
| MenuFlyout | template sets `PrimaryVariantDarkBrush` on item backgrounds directly |
| CalendarView | `SelectedHoverBorderBrush` / `SelectedPressedBorderBrush` → `PrimaryVariantDark` |
| Expander | header pressed → `TertiaryContainer`, pointer-over → `PrimaryContainer` |

Judgment calls (transparent / surface fill becoming a container or outline colour, label keeps
its own colour): `AppBarButtonBackground{PointerOver,Pressed}`, `TextButtonBackgroundPressed`
(`PrimaryContainer`), `ListViewItemBackgroundPressed` (`Outline`),
`CheckBoxBackgroundUncheckedPressed` / `RadioButtonOuterEllipseFillPressed` (`PrimaryContainer`).

## Review

_(filled in when done)_
