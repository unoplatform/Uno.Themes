# Feature Specification: Design-System-Agnostic Semantic Abstraction Layer

**Created**: 2026-03-10
**Status**: Draft
**Scope**: Material (v2) and Simple themes only

## Context

Uno.Themes ships multiple design system implementations (Material, Simple, Cupertino).
Each theme defines its own style keys prefixed by theme name (e.g. `MaterialFilledButtonStyle`,
`SimplePrimaryButtonStyle`). Today, both themes already publish a partial set of
**theme-agnostic aliases** in their respective `_Resources.xaml` files — but these
alias sets are **inconsistent** between themes and were never designed as a formal
abstraction contract.

This spec defines a **canonical semantic style and resource naming layer** that sits
above the individual design-system implementations. The semantic names are based
**almost entirely on Material v2 naming** (with the `Material` prefix stripped),
because Material has the richest and most widely recognized vocabulary.

Each theme is then responsible for mapping every semantic name to the best-fit
theme-specific style. Where a theme has no equivalent, the gap is documented for
future investigation.

### Goals

1. **Write once, theme anywhere** — App developers use `Style="{StaticResource FilledButtonStyle}"`
   and the active theme resolves it to the correct design-system-specific style.
2. **Material-derived naming** — Semantic names mirror Material v2 key names minus the
   `Material` prefix, giving developers a familiar vocabulary.
3. **Gap visibility** — Clearly surface where Simple lacks an equivalent so those can
   be prioritized for future work.
4. **Non-breaking** — This layer is additive. Existing theme-prefixed keys remain valid.

### Existing State

Both themes already publish aliases in `_Resources.xaml`:

- **Material** (`src/library/Uno.Material/Styles/Controls/v2/_Resources.xaml`):
  Already maps semantic keys like `ElevatedButtonStyle → MaterialElevatedButtonStyle`.
  This is effectively the target contract.

- **Simple** (`src/library/Uno.Simple.WinUI/Styles/Controls/_Resources.xaml`):
  Publishes SDS-native aliases (e.g. `PrimaryButtonStyle`, `NeutralButtonStyle`)
  **plus** a set of "legacy / backwards-compatible aliases" that partially map
  Material names to Simple equivalents (e.g. `FilledButtonStyle → SimplePrimaryButtonStyle`).

This spec formalizes and completes those mappings.

---

## User Scenarios & Testing

### User Story 1 — Theme-Agnostic Button Styling (Priority: P1)

A developer building a cross-theme app uses `Style="{StaticResource FilledButtonStyle}"`
on a Button. When the app loads Material theme, the button renders as a Material
Filled button. When the app loads Simple theme, it renders as a Simple Primary button.

**Why this priority**: Buttons are the most common styled control. Getting
the abstraction right here validates the entire pattern.

**Independent Test**: Create a page with all 6 semantic button styles. Verify
rendering under both Material and Simple themes.

**Acceptance Scenarios**:

1. **Given** Material theme is active, **When** `FilledButtonStyle` is applied,
   **Then** the button renders identically to `MaterialFilledButtonStyle`.
2. **Given** Simple theme is active, **When** `FilledButtonStyle` is applied,
   **Then** the button renders identically to `SimplePrimaryButtonStyle`.
3. **Given** Simple theme is active, **When** `ElevatedButtonStyle` is applied,
   **Then** the button renders using the documented best-fit mapping (or falls back
   gracefully if no mapping exists yet).

---

### User Story 2 — Theme-Agnostic Input Styling (Priority: P1)

A developer uses `FilledTextBoxStyle` and `OutlinedTextBoxStyle` on TextBox controls.
Both themes resolve to the correct variant.

**Acceptance Scenarios**:

1. **Given** Material theme, **When** `FilledTextBoxStyle` is applied,
   **Then** renders as `MaterialFilledTextBoxStyle`.
2. **Given** Simple theme, **When** `FilledTextBoxStyle` is applied,
   **Then** renders as `SimpleInputTextBoxStyle`.
3. Same pattern for PasswordBox: `FilledPasswordBoxStyle`, `OutlinedPasswordBoxStyle`.

---

### User Story 3 — Theme-Agnostic Single-Variant Controls (Priority: P2)

Controls with only one style variant (CheckBox, RadioButton, ToggleSwitch, Slider,
etc.) resolve correctly under both themes with a single semantic key.

**Acceptance Scenarios**:

1. **Given** either theme, **When** `CheckBoxStyle` is applied,
   **Then** it resolves to the theme's checkbox style.

---

### User Story 4 — Gap Awareness (Priority: P2)

When a developer uses a semantic key that has no Simple equivalent (e.g.
`NavigationViewStyle`), the behavior is gracefully handled (control uses WinUI
default style) and the gap is documented.

---

### Edge Cases

- What happens when a semantic key exists in Material but not in Simple?
  → The control falls back to WinUI default styling. The gap table below tracks these.
- What happens with Material-only controls like Ripple?
  → These are **not** part of the semantic layer (theme-specific controls are excluded).
- Simple has styles with no Material equivalent (e.g. `PersonPictureStyle`, `ExpanderStyle`).
  → These remain as Simple-only aliases. They may be promoted to semantic keys if Material
  adds equivalents in the future.

---

## Requirements

### Functional Requirements

- **FR-001**: The semantic layer MUST define a canonical key for every consumer-facing
  Material v2 style (excluding internal/base styles and the Ripple control).
- **FR-002**: Each theme MUST map every applicable semantic key to its best-fit
  theme-specific style via `<StaticResource>` alias in `_Resources.xaml`.
- **FR-003**: Where a theme has no equivalent style for a semantic key, the alias
  MUST be omitted (not mapped to a wrong-fit style) and documented in the gap table.
- **FR-004**: Semantic keys for typography MUST follow the existing Material type
  scale names (DisplayLarge, HeadlineMedium, BodySmall, etc.) — both themes already
  implement these.
- **FR-005**: This layer MUST NOT introduce new control styles — it is purely a
  naming/aliasing contract over existing styles.
- **FR-006**: Existing theme-prefixed keys (e.g. `MaterialFilledButtonStyle`,
  `SimplePrimaryButtonStyle`) MUST remain valid and unchanged.

---

## Semantic Style Mappings

Convention: **Semantic Key** = Material v2 key with `Material` prefix removed.

### Button

| Semantic Key | Material v2 | Simple | Notes |
|---|---|---|---|
| `FilledButtonStyle` | `MaterialFilledButtonStyle` | `SimplePrimaryButtonStyle` | Simple "Primary" = filled appearance |
| `ElevatedButtonStyle` | `MaterialElevatedButtonStyle` | ⚠️ **GAP** | Simple has no elevated/shadow variant |
| `FilledTonalButtonStyle` | `MaterialFilledTonalButtonStyle` | `SimpleNeutralButtonStyle` | Simple "Neutral" is closest tonal match |
| `OutlinedButtonStyle` | `MaterialOutlinedButtonStyle` | `SimpleNeutralButtonStyle` | Simple "Neutral" has outline treatment |
| `TextButtonStyle` | `MaterialTextButtonStyle` | `SimpleSubtleButtonStyle` | Simple "Subtle" = text-only appearance |
| `IconButtonStyle` | `MaterialIconButtonStyle` | `SimpleIconButtonPrimaryStyle` | Simple splits icon buttons by color; Primary is default |

> **Note — `FilledTonalButtonStyle` and `OutlinedButtonStyle`** both map to
> `SimpleNeutralButtonStyle`. This is intentional — Simple's "Neutral" concept
> covers both tonal and outlined treatments. No disambiguation needed.

### Floating Action Button (FAB)

FAB is a **Material-specific concept** and is **excluded from the semantic layer**.
The 12 FAB styles (`MaterialFabStyle`, `MaterialSmallFabStyle`, `MaterialLargeFabStyle`,
and their Surface/Secondary/Tertiary variants) remain accessible only via their
`Material`-prefixed keys and the existing Material-only aliases (`FabStyle`,
`SmallFabStyle`, etc. in Material's `_Resources.xaml`).

### ToggleButton

| Semantic Key | Material v2 | Simple | Notes |
|---|---|---|---|
| `TextToggleButtonStyle` | `MaterialTextToggleButtonStyle` | `SimpleDefaultToggleButtonStyle` | Simple has one base toggle style |
| `IconToggleButtonStyle` | `MaterialIconToggleButtonStyle` | ⚠️ **GAP** | Simple has no icon-only toggle variant |

### TextBox

| Semantic Key | Material v2 | Simple | Notes |
|---|---|---|---|
| `FilledTextBoxStyle` | `MaterialFilledTextBoxStyle` | `SimpleInputTextBoxStyle` | Simple "Input" = filled appearance |
| `OutlinedTextBoxStyle` | `MaterialOutlinedTextBoxStyle` | `SimpleOutlinedInputTextBoxStyle` | Direct match |

### PasswordBox

| Semantic Key | Material v2 | Simple | Notes |
|---|---|---|---|
| `FilledPasswordBoxStyle` | `MaterialFilledPasswordBoxStyle` | `SimplePasswordBoxStyle` | Simple has one style (filled) |
| `OutlinedPasswordBoxStyle` | `MaterialOutlinedPasswordBoxStyle` | ⚠️ **GAP** | Simple has no outlined password variant |

### HyperlinkButton

| Semantic Key | Material v2 | Simple | Notes |
|---|---|---|---|
| `HyperlinkButtonStyle` | `MaterialHyperlinkButtonStyle` | ⚠️ **GAP** | Simple has no HyperlinkButton style |
| `SecondaryHyperlinkButtonStyle` | `MaterialSecondaryHyperlinkButtonStyle` | ⚠️ **GAP** | |

### ComboBox

| Semantic Key | Material v2 | Simple | Notes |
|---|---|---|---|
| `ComboBoxStyle` | `MaterialComboBoxStyle` | `SimpleSelectFieldStyle` | Simple names it "SelectField" |

### CheckBox

| Semantic Key | Material v2 | Simple | Notes |
|---|---|---|---|
| `CheckBoxStyle` | `MaterialCheckBoxStyle` | `SimpleCheckBoxStyle` | ✅ Direct match |

### RadioButton

| Semantic Key | Material v2 | Simple | Notes |
|---|---|---|---|
| `RadioButtonStyle` | `MaterialRadioButtonStyle` | `SimpleRadioButtonStyle` | ✅ Direct match |

### ToggleSwitch

| Semantic Key | Material v2 | Simple | Notes |
|---|---|---|---|
| `ToggleSwitchStyle` | `MaterialToggleSwitchStyle` | `SimpleToggleSwitchStyle` | ✅ Direct match |

### Slider

| Semantic Key | Material v2 | Simple | Notes |
|---|---|---|---|
| `SliderStyle` | `MaterialSliderStyle` | `SimpleSliderStyle` | ✅ Direct match |

### ProgressBar

| Semantic Key | Material v2 | Simple | Notes |
|---|---|---|---|
| `ProgressBarStyle` | `MaterialProgressBarStyle` | ⚠️ **GAP** | Simple has no ProgressBar style |

### ProgressRing

| Semantic Key | Material v2 | Simple | Notes |
|---|---|---|---|
| `ProgressRingStyle` | `MaterialProgressRingStyle` | ⚠️ **GAP** | Simple has no ProgressRing style |

### ListView

| Semantic Key | Material v2 | Simple | Notes |
|---|---|---|---|
| `ListViewStyle` | `MaterialListViewStyle` | `SimpleListViewStyle` | ✅ Direct match |
| `ListViewItemStyle` | `MaterialListViewItemStyle` | `SimpleListViewItemStyle` | ✅ Direct match |

### ContentDialog

| Semantic Key | Material v2 | Simple | Notes |
|---|---|---|---|
| `ContentDialogStyle` | `MaterialContentDialogStyle` | `SimpleContentDialogStyle` | ✅ Direct match |

### CommandBar

| Semantic Key | Material v2 | Simple | Notes |
|---|---|---|---|
| `CommandBarStyle` | `MaterialCommandBarStyle` | ⚠️ **GAP** | Simple has no CommandBar style |

### AppBarButton

| Semantic Key | Material v2 | Simple | Notes |
|---|---|---|---|
| `AppBarButtonStyle` | `MaterialAppBarButtonStyle` | `SimpleAppBarButtonStyle` | ✅ Direct match |

### NavigationView

| Semantic Key | Material v2 | Simple | Notes |
|---|---|---|---|
| `NavigationViewStyle` | `MaterialNavigationViewStyle` | ⚠️ **GAP** | Simple has no NavigationView style |
| `NavigationViewItemStyle` | `MaterialNavigationViewItemStyle` | ⚠️ **GAP** | |

### CalendarView

| Semantic Key | Material v2 | Simple | Notes |
|---|---|---|---|
| `CalendarViewStyle` | `MaterialCalendarViewStyle` | `SimpleCalendarViewStyle` | ✅ Direct match |

### CalendarDatePicker

| Semantic Key | Material v2 | Simple | Notes |
|---|---|---|---|
| `CalendarDatePickerStyle` | `MaterialCalendarDatePickerStyle` | `SimpleCalendarDatePickerStyle` | ✅ Direct match |

### DatePicker

| Semantic Key | Material v2 | Simple | Notes |
|---|---|---|---|
| `DatePickerStyle` | `MaterialDatePickerStyle` | `SimpleDatePickerStyle` | ✅ Direct match |

### MediaPlayerElement

| Semantic Key | Material v2 | Simple | Notes |
|---|---|---|---|
| `MediaTransportControlsStyle` | `MaterialMediaTransportControlsStyle` | ⚠️ **GAP** | Simple has no media style |

### PipsPager

| Semantic Key | Material v2 | Simple | Notes |
|---|---|---|---|
| `PipsPagerStyle` | `MaterialPipsPagerStyle` | ⚠️ **GAP** | Simple has no PipsPager style |

### RatingControl

| Semantic Key | Material v2 | Simple | Notes |
|---|---|---|---|
| `RatingControlStyle` | `MaterialRatingControlStyle` | ⚠️ **GAP** | Simple has no RatingControl style |

### Flyout / MenuFlyout

| Semantic Key | Material v2 | Simple | Notes |
|---|---|---|---|
| `FlyoutPresenterStyle` | `MaterialFlyoutPresenterStyle` | ⚠️ **GAP** | Simple has no FlyoutPresenter style |
| `MenuFlyoutPresenterStyle` | `MaterialMenuFlyoutPresenterStyle` | `SimpleMenuFlyoutPresenterStyle` | ✅ Direct match |
| `MenuFlyoutItemStyle` | `MaterialMenuFlyoutItemStyle` | `SimpleMenuFlyoutItemStyle` | ✅ Direct match |
| `ToggleMenuFlyoutItemStyle` | `MaterialToggleMenuFlyoutItemStyle` | `SimpleToggleMenuFlyoutItemStyle` | ✅ Direct match |
| `RadioMenuFlyoutItemStyle` | `MaterialRadioMenuFlyoutItemStyle` | ⚠️ **GAP** | Simple has no RadioMenuFlyoutItem |
| `MenuFlyoutSubItemStyle` | `MaterialMenuFlyoutSubItemStyle` | `SimpleMenuFlyoutSubItemStyle` | ✅ Direct match |
| `MenuFlyoutSeparatorStyle` | `MaterialMenuFlyoutSeparatorStyle` | `SimpleMenuFlyoutSeparatorStyle` | ✅ Direct match |

---

## Semantic Typography Mappings

Both themes already publish identical alias keys for typography. This layer is
**already complete** and requires no changes.

| Semantic Key | Material v2 | Simple |
|---|---|---|
| `DisplayLarge` | `MaterialDisplayLarge` | `SimpleDisplayLarge` |
| `DisplayMedium` | `MaterialDisplayMedium` | `SimpleDisplayMedium` |
| `DisplaySmall` | `MaterialDisplaySmall` | `SimpleDisplaySmall` |
| `HeadlineLarge` | `MaterialHeadlineLarge` | `SimpleHeadlineLarge` |
| `HeadlineMedium` | `MaterialHeadlineMedium` | `SimpleHeadlineMedium` |
| `HeadlineSmall` | `MaterialHeadlineSmall` | `SimpleHeadlineSmall` |
| `TitleLarge` | `MaterialTitleLarge` | `SimpleTitleLarge` |
| `TitleMedium` | `MaterialTitleMedium` | `SimpleTitleMedium` |
| `TitleSmall` | `MaterialTitleSmall` | `SimpleTitleSmall` |
| `BodyLarge` | `MaterialBodyLarge` | `SimpleBodyLarge` |
| `BodyMedium` | `MaterialBodyMedium` | `SimpleBodyMedium` |
| `BodySmall` | `MaterialBodySmall` | `SimpleBodySmall` |
| `LabelLarge` | `MaterialLabelLarge` | `SimpleLabelLarge` |
| `LabelMedium` | `MaterialLabelMedium` | `SimpleLabelMedium` |
| `LabelSmall` | `MaterialLabelSmall` | `SimpleLabelSmall` |
| `LabelExtraSmall` | `MaterialLabelExtraSmall` | `SimpleLabelExtraSmall` |
| `CaptionLarge` | `MaterialCaptionLarge` | `SimpleCaptionLarge` |
| `CaptionMedium` | `MaterialCaptionMedium` | `SimpleCaptionMedium` |
| `CaptionSmall` | `MaterialCaptionSmall` | `SimpleCaptionSmall` |

---

## Simple-Only Styles (Not in Semantic Layer)

These styles exist in Simple but have no Material equivalent. They remain
accessible only via their `Simple`-prefixed keys (and the SDS-native aliases
already in Simple's `_Resources.xaml`).

| Simple Style | Category | Notes |
|---|---|---|
| `SimpleDangerPrimaryButtonStyle` | Button | Destructive filled action — no M3 equivalent |
| `SimpleDangerSubtleButtonStyle` | Button | Destructive text action — no M3 equivalent |
| `SimpleIconButtonNeutralStyle` | Button | Simple splits icon buttons by color |
| `SimpleIconButtonSubtleStyle` | Button | |
| `SimpleIconButtonDangerPrimaryStyle` | Button | |
| `SimpleIconButtonDangerSubtleStyle` | Button | |
| All `SimpleSmall*` / `SimpleMedium*` button variants | Button | Size variants — Material doesn't have explicit size variants |
| `SimpleInputTextBoxErrorStyle` | TextBox | Dedicated error style |
| `SimpleInputTextBoxSmallStyle` | TextBox | Size variant |
| `SimpleSelectFieldErrorStyle` | ComboBox | Dedicated error style |
| `SimplePersonPictureStyle` (6 variants) | PersonPicture | Material has no PersonPicture |
| `SimpleExpanderStyle` | Expander | Material has no Expander style |
| `SimpleAutoSuggestBoxStyle` | AutoSuggestBox | Material has no AutoSuggestBox style |
| `SimpleToolTipStyle` | ToolTip | Material has no ToolTip style |
| `SimpleDefaultToggleButtonStyle` (+ size variants) | ToggleButton | Different variant model |

> **Future consideration**: Some of these (Expander, AutoSuggestBox, ToolTip) may be
> promoted to the semantic layer if Material adds equivalent styles.

---

## Material-Only Styles (Excluded from Semantic Layer)

These are Material-internal styles that should **not** be part of the semantic
contract because they are implementation details or theme-specific controls.

| Material Style | Reason for Exclusion |
|---|---|
| `MaterialRippleStyle` | Ripple is a Material-specific custom control |
| `MaterialBaseButtonStyle` | Internal base — not consumer-facing |
| `MaterialBaseTextBlockStyle` | Internal base |
| `MaterialBaseCommandBarStyle` | Internal base |
| `MaterialDeleteButtonStyle` | TextBox internal button |
| `MaterialRevealButtonStyle` | PasswordBox internal button |
| `MaterialSliderThumbStyle` | Slider internal part |
| `MaterialContentDialogButtonStyle` | Dialog internal button |
| `MaterialContentDialogDefaultButtonStyle` | Dialog internal button |
| `MaterialContentFlyoutPresenterStyle` | Internal variant |
| `MaterialDatePickerFlyoutButtonStyle` | DatePicker internal |
| `MaterialDatePickerFlyoutPresenterStyle` | DatePicker internal |
| `MaterialComboBoxItemStyle` | ComboBox internal |
| `MaterialNavigationViewIconButtonStyle` | NavigationView internal |
| `MaterialNavigationViewIconTextButtonStyle` | NavigationView internal |
| `MaterialPipsPager*ButtonStyle` (6 styles) | PipsPager internals |
| CalendarView internal styles | CalendarView internals |
| All `MaterialDefault*Style` keys | Implicit style targets — not semantic aliases |

---

## Gap Summary

### Simple Gaps — No Equivalent Style Exists

| Semantic Key | Priority | Recommendation |
|---|---|---|
| `ElevatedButtonStyle` | 🔴 High | Investigate adding a Simple elevated/shadow button variant |
| ~~`FilledTonalButtonStyle`~~ | — | **Resolved**: Mapping to NeutralButton is intentional |
| `IconToggleButtonStyle` | 🟡 Medium | Add icon-only toggle to Simple |
| `OutlinedPasswordBoxStyle` | 🟡 Medium | Add outlined variant to Simple |
| `HyperlinkButtonStyle` | 🟡 Medium | Add HyperlinkButton to Simple |
| `SecondaryHyperlinkButtonStyle` | 🟠 Low | Secondary variant — lower priority |
| ~~`FabStyle` (all 12 variants)~~ | — | **Resolved**: Excluded from semantic layer |
| `ProgressBarStyle` | 🔴 High | Common control — should be in Simple |
| `ProgressRingStyle` | 🔴 High | Common control — should be in Simple |
| `CommandBarStyle` | 🟡 Medium | Already noted in Simple's `_Resources.xaml` TODO |
| `NavigationViewStyle` | 🟡 Medium | Already noted in Simple's `_Resources.xaml` TODO |
| `NavigationViewItemStyle` | 🟡 Medium | Depends on NavigationView |
| `FlyoutPresenterStyle` | 🟡 Medium | Already noted in Simple's `_Resources.xaml` TODO |
| `RadioMenuFlyoutItemStyle` | 🟠 Low | Already noted in Simple's `_Resources.xaml` TODO |
| `MediaTransportControlsStyle` | 🟠 Low | Niche control |
| `PipsPagerStyle` | 🟠 Low | Already noted in Simple's `_Resources.xaml` TODO |
| `RatingControlStyle` | 🟠 Low | Already noted in Simple's `_Resources.xaml` TODO |

### Ambiguous Mappings — Need Design Decision

| Semantic Key | Issue | Recommendation |
|---|---|---|
| `FilledTonalButtonStyle` / `OutlinedButtonStyle` | Both map to `SimpleNeutralButtonStyle` | Intentional — Simple's Neutral covers both; no action needed |
| `IconButtonStyle` | Material has one icon button; Simple has 5 color variants | Map to `SimpleIconButtonPrimaryStyle` as default; consider if semantic layer needs color sub-variants |
| ~~FAB styles (all 12)~~ | ~~Material-specific concept~~ | **Resolved**: Excluded from semantic layer — Material-only |

---

## Success Criteria

- **SC-001**: Every semantic key defined in this spec resolves correctly when the
  corresponding theme is active — verified by a test page rendering all styles
  under both Material and Simple.
- **SC-002**: No existing app using `Material`-prefixed or `Simple`-prefixed keys
  experiences any regression.
- **SC-003**: The gap table is accurate — every ⚠️ GAP entry truly has no
  reasonable Simple equivalent.
- **SC-004**: Both `_Resources.xaml` files are fully aligned with this spec's
  mapping tables after implementation.

---

## Implementation Notes

### Mechanism

The semantic layer is implemented purely through `<StaticResource>` aliases in
each theme's `_Resources.xaml`. No new controls, no new XAML templates, no code changes.

```xml
<!-- In Material _Resources.xaml (already exists): -->
<StaticResource x:Key="FilledButtonStyle" ResourceKey="MaterialFilledButtonStyle" />

<!-- In Simple _Resources.xaml (update needed): -->
<StaticResource x:Key="FilledButtonStyle" ResourceKey="SimplePrimaryButtonStyle" />
```

### What Needs to Change

1. **Audit Material `_Resources.xaml`** — Verify all semantic keys from this spec are present.
   Material's file is largely complete already.

2. **Update Simple `_Resources.xaml`** — Add missing semantic aliases, remove or
   reorganize the current ad-hoc "legacy / backwards-compatible aliases" section
   to align with this spec. The existing legacy aliases (`FilledButtonStyle`,
   `OutlinedButtonStyle`, `TextButtonStyle`, etc.) already partially implement
   this — they need to be validated against this spec and completed.

3. **Document gaps** — Add XAML comments in Simple's `_Resources.xaml` for each
   gap, referencing this spec.

### Relationship to Simple-Native Aliases

Simple's `_Resources.xaml` currently has **two alias sections**:
- "Theme-agnostic aliases (preferred — matches Figma SDS naming)" — e.g. `PrimaryButtonStyle`
- "Legacy / backwards-compatible aliases" — e.g. `FilledButtonStyle`

After this spec is implemented, the structure should be:
1. **Semantic aliases** (this spec) — Material-derived names like `FilledButtonStyle`
2. **SDS-native aliases** — Simple's own vocabulary like `PrimaryButtonStyle`, `NeutralButtonStyle`
3. Both coexist; neither replaces the other.
