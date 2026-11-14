# Default styles keep their shape at any DefaultCornerRadius (issue #1747)

Implements https://github.com/unoplatform/Uno.Themes/issues/1747.

## Design

The shape scale grows with `DefaultCornerRadius`; a control's size does not. A step that suits a
40px control at the default of 4 passes half its height at 8 or 16, and Uno clamps each side of an
oversized radius independently, so the control becomes an ellipse instead of a pill.

- **Multipliers and the default of 4 are unchanged.** The softer-growth option (recommendation 6)
  is not taken: the existing token tests (`6 → Radius300 = 18`) must pass as they are.
- **Validation.** A non-finite or negative `DefaultCornerRadius` falls back to 4, like `DefaultSpacing`.
- **Directional companions.** Every step gets `Top` (r,r,0,0), `Left` (r,0,0,r), `Bottom` (0,0,r,r)
  and `TopLeft` (r,0,0,0) `CornerRadius` keys, so one-sided literals can follow the scale.
- **Size-capped radii.** `BaseTheme.CappedRadii` lets a theme declare `min(base × multiplier, cap)`
  roles. They are generated on every rebuild into a plain dictionary (not `SemanticResources`: the
  keys are theme-specific), as `SimpleShape*` / `MaterialShape*`, each with the same double,
  `CornerRadius` and directional companions as a scale step. The existing per-control keys keep
  their name and type and alias these roles.
- **Caps are constants.** Every capped control has a fixed size (`ControlHeight*`, `IconSize*`,
  literals); none is driven by `DefaultSpacing`. Each cap is at least the value at 4.
- **Literals follow the scale** where a token gives the same value at 4.

### Decisions (with the user, 2026-11-13)

- A cap is computed from `DefaultCornerRadius`, so overriding a `Radius*` token no longer reaches a
  capped control. Accepted and documented; the per-control key is the override point.
- One PR for core, Simple and Material v2.

### Deviations from the issue's tables

- Simple containers (Expander, Flyout, menu presenter, ContentDialog, AutoSuggestBox suggestions)
  are capped at 16 and ToolTip at 10. The issue lists them as root causes but gives no cap.
- Circle literals (Material slider thumb, large switch thumb, CalendarView day items) stay literals.
  They are bound to the part's size, not to the scale, and `RadiusFull` would change nothing at 4.
- Thin bars are capped too, which the issue does not list: the Simple slider track and progress bar
  (2), slider fill (4), and the Material slider fill (4) and NavigationView selection indicator (2).
  Turning their literals into plain tokens made them grow past half their thickness; the page
  capture at 16 showed the Simple slider track ending in a point.
- WinUI's own `ControlCornerRadius` / `OverlayCornerRadius` are not remapped (not verified).
- Material `SwitchKnobRadius` was already a fixed 16 on this branch, not `Space400`.

## Checklist

- [x] Validation, directional companions and `CappedRadii` generation (`BaseTheme.cs`, `BaseTheme.ScaleGeneration.cs`, `CappedRadius.cs`)
- [x] Simple roles and aliases (`SimpleTheme.cs`, `Styles/Controls/*.xaml`)
- [x] Material v2 roles, aliases and literals (`MaterialTheme.cs`, `Styles/Controls/v2/*.xaml`)
- [x] `check-xaml-resource-keys.ps1` allows the generated `SimpleShape*` / `MaterialShape*` prefixes
- [x] Runtime tests in both `Given_DesignTokens.cs`
- [x] `doc/design-tokens.md`, `doc/simple-controls-styles.md`, Corner Radius Styles sample labels
- [x] Generated style pages (`GenerateStyleDocs.cs`, which now knows the generated keys)
- [x] Runtime tests green on Simple (337/337) and Material (143/143) desktop, Debug
- [x] Formatters (`xstyler`, `dotnet format`), `check-xaml-resource-keys.ps1`, style-docs `--check`
- [x] Rendering checked at 0, 4, 8 and 16: the Corner Radius Styles page rendered to PNG on both desktop heads (flyouts and dialogs, which only exist once opened, not captured)
- [x] Before/after comparison sheets in `assets/` (Light theme, desktop head, base 0, 2, 4, 8, 16; a magenta frame marks a tile that differs from the base commit; none differs at 4)
- [ ] WinAppSDK head and native targets (not run)

## Review

- The tests were written after the fix, so the red step was not observed. The token tests cannot
  pass on `master` (the keys do not exist there); the rendered ones assert values `master` exceeds.
- One existing Material test changed: `When_FilledButton_WithCustomCornerRadius_Then_ButtonReflectsIt`
  used base 6 and expected 30, which is now capped at 20. It asserts base 3 → 15; the cap has its own test.
- A per-control key is a `StaticResource` alias and resolves from the scope doing the lookup, so a
  test must read it through a loaded element, not `container.Resources.TryGetValue`.
- The issue's three "check before implementing" items were not investigated in a running app. The
  NavigationViewItem literal `28,0,0,28` is now a capped token, so it is 0 at base 0.
