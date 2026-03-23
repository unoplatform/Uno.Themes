# Simple Theme — Missing Control Styles

Draft titles and descriptions for each control with a gap (❌) in the Simple theme, based on the [Semantic Styles matrix](semantic-styles.md).

---

## Floating Action Button (FAB)

**Title:** Add Floating Action Button (FAB) aliases to Simple theme

**Description:** The FAB control is currently Material-only. Rather than creating new styles, alias the twelve semantic FAB keys to existing Simple icon button styles using the following mapping:

| Semantic Key | Simple Alias | Rationale |
|---|---|---|
| `FabStyle` | `SimpleIconButtonPrimaryStyle` | Primary FAB → Primary icon button |
| `SmallFabStyle` | `SimpleSmallIconButtonPrimaryStyle` | Small FAB → Small primary icon button |
| `LargeFabStyle` | `SimpleIconButtonPrimaryStyle` | No large variant; fall back to default primary |
| `SecondaryFabStyle` | `SimpleIconButtonNeutralStyle` | Secondary FAB → Neutral icon button |
| `SecondarySmallFabStyle` | `SimpleSmallIconButtonNeutralStyle` | Small secondary → Small neutral icon button |
| `SecondaryLargeFabStyle` | `SimpleIconButtonNeutralStyle` | No large variant; fall back to default neutral |
| `TertiaryFabStyle` | `SimpleIconButtonSubtleStyle` | Tertiary FAB → Subtle icon button |
| `TertiarySmallFabStyle` | `SimpleSmallIconButtonSubtleStyle` | Small tertiary → Small subtle icon button |
| `TertiaryLargeFabStyle` | `SimpleIconButtonSubtleStyle` | No large variant; fall back to default subtle |
| `SurfaceFabStyle` | `SimpleIconButtonNeutralStyle` | Surface FAB → Neutral icon button (shared with Secondary) |
| `SurfaceSmallFabStyle` | `SimpleSmallIconButtonNeutralStyle` | Small surface → Small neutral icon button |
| `SurfaceLargeFabStyle` | `SimpleIconButtonNeutralStyle` | No large variant; fall back to default neutral |

No new control templates are needed — only semantic alias registrations.

---

## ToggleButton — IconToggleButtonStyle

**Title:** Add IconToggleButtonStyle to Simple theme

**Description:** Simple has a default toggle button but no icon-only toggle variant. Add a `SimpleIconToggleButtonStyle` that presents a compact, icon-centric toggle (similar to Simple's existing `IconButtonStyle` but with toggle state visuals) and register it as the `IconToggleButtonStyle` semantic key.

---

## TextBox — OutlinedTextBoxStyle

**Title:** Add OutlinedTextBoxStyle to Simple theme

**Description:** Simple currently maps both `FilledTextBoxStyle` and `OutlinedTextBoxStyle` to its single `SimpleTextBoxStyle` but has no outlined variant at all. Add a `SimpleOutlinedTextBoxStyle` with a visible border and no fill, consistent with how other outlined input controls look in Simple, and register it as the `OutlinedTextBoxStyle` semantic key. The existing `SimpleTextBoxStyle` should map to `FilledTextBoxStyle`.

---

## PasswordBox — OutlinedPasswordBoxStyle

**Title:** Add OutlinedPasswordBoxStyle to Simple theme

**Description:** Simple currently maps `FilledPasswordBoxStyle` to its single `SimplePasswordBoxStyle` but has no outlined variant at all. Add a `SimpleOutlinedPasswordBoxStyle` with a visible border and no fill, consistent with how other outlined input controls look in Simple, and register it as the `OutlinedPasswordBoxStyle` semantic key.

---

## HyperlinkButton

**Title:** Add HyperlinkButton styles to Simple theme

**Description:** Simple has no HyperlinkButton styling. Add a `SimpleHyperlinkButtonStyle` that renders an underlined, text-only link matching Simple's typography and color tokens, plus a `SimpleSecondaryHyperlinkButtonStyle` using a secondary/muted color. Register them as the `HyperlinkButtonStyle` and `SecondaryHyperlinkButtonStyle` semantic keys respectively.

---

## MenuFlyout — RadioMenuFlyoutItemStyle

**Title:** Add RadioMenuFlyoutItemStyle to Simple theme

**Description:** Simple supports most MenuFlyout parts but is missing the `RadioMenuFlyoutItem` style. Add a `SimpleRadioMenuFlyoutItemStyle` that shows a radio-bullet indicator for the selected item within a menu group, consistent with the existing Simple menu item visuals, and register it as the `RadioMenuFlyoutItemStyle` semantic key.

---

## FlyoutPresenter

**Title:** Add FlyoutPresenterStyle to Simple theme

**Description:** Simple has no `FlyoutPresenter` style, meaning generic flyouts fall back to the platform default. Add a `SimpleFlyoutPresenterStyle` with Simple's standard surface color, corner radius, and shadow tokens so that flyouts look consistent with the rest of the Simple design language. Register it as the `FlyoutPresenterStyle` semantic key.

---

## NavigationView

**Title:** Add NavigationView styles to Simple theme

**Description:** Simple has no `NavigationView` or `NavigationViewItem` styles. Add `SimpleNavigationViewStyle` and `SimpleNavigationViewItemStyle` that provide left-rail / top-nav navigation consistent with Simple's layout and color tokens, including selection indicators, expand/collapse behavior, and compact/expanded modes. Register them as the `NavigationViewStyle` and `NavigationViewItemStyle` semantic keys.

---

## ProgressBar

**Title:** Add ProgressBar style to Simple theme

**Description:** Simple has no `ProgressBar` style. Add a `SimpleProgressBarStyle` that renders a horizontal progress indicator using Simple's accent and track colors, supporting both determinate and indeterminate states. Register it as the `ProgressBarStyle` semantic key.

---

## ProgressRing

**Title:** Add ProgressRing style to Simple theme

**Description:** Simple has no `ProgressRing` style. Add a `SimpleProgressRingStyle` that renders a circular/ring progress indicator using Simple's accent and track colors, supporting both determinate and indeterminate states. Register it as the `ProgressRingStyle` semantic key.

---

## PipsPager

**Title:** Add PipsPager style to Simple theme

**Description:** Simple has no `PipsPager` style. Add a `SimplePipsPagerStyle` that renders pagination dots with Simple's standard sizing, spacing, and color tokens, including selected/unselected states and optional navigation arrows. Register it as the `PipsPagerStyle` semantic key.

---

## RatingControl

**Title:** Add RatingControl style to Simple theme

**Description:** Simple has no `RatingControl` style. Add a `SimpleRatingControlStyle` that renders star (or similar) rating icons using Simple's icon and accent color tokens, supporting filled, half-filled, and empty states, along with read-only and interactive modes. Register it as the `RatingControlStyle` semantic key.

---