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
2. **Lightweight styling portability** — Semantic lightweight styling resource keys
   (e.g. `FilledButtonForeground`, `CheckBoxBackgroundChecked`) resolve to the
   correct theme-specific value under both Material and Simple, giving developers
   a single set of customization knobs.
3. **Material-derived naming** — Semantic names mirror Material v2 key names minus the
   `Material` prefix, giving developers a familiar vocabulary.
4. **Gap visibility** — Clearly surface where Simple lacks an equivalent so those can
   be prioritized for future work.
5. **Non-breaking** — This layer is additive. Existing theme-prefixed keys remain valid.

### Existing State

**Style aliases** — Both themes already publish aliases in `_Resources.xaml`:

- **Material** (`src/library/Uno.Material/Styles/Controls/v2/_Resources.xaml`):
  Already maps semantic keys like `ElevatedButtonStyle → MaterialElevatedButtonStyle`.
  This is effectively the target contract.

- **Simple** (`src/library/Uno.Simple.WinUI/Styles/Controls/_Resources.xaml`):
  Publishes SDS-native aliases (e.g. `PrimaryButtonStyle`, `NeutralButtonStyle`)
  **plus** a set of "legacy / backwards-compatible aliases" that partially map
  Material names to Simple equivalents (e.g. `FilledButtonStyle → SimplePrimaryButtonStyle`).

**Lightweight styling resources** — Both themes define per-control ThemeDictionary
resources for lightweight styling, but with different naming:

- **Material** (e.g. `src/library/Uno.Material/Styles/Controls/v2/Button.xaml`):
  Defines resources with **unprefixed** names like `FilledButtonForeground`,
  `CheckBoxBackgroundChecked`. These are already semantic.

- **Simple** (e.g. `src/library/Uno.Simple.WinUI/Styles/Controls/Button.xaml`):
  Defines resources with **`Simple`-prefixed** names like `SimplePrimaryButtonForeground`,
  `SimpleCheckBoxCheckedBackground`. No semantic aliases exist today.

This spec formalizes and completes both the style mappings and the lightweight
styling resource mappings.

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
   **Then** renders as `SimpleTextBoxStyle`.
3. Same pattern for PasswordBox: `FilledPasswordBoxStyle`, `OutlinedPasswordBoxStyle`.

---

### User Story 3 — Theme-Agnostic Single-Variant Controls (Priority: P2)

Controls with only one style variant (CheckBox, RadioButton, ToggleSwitch, Slider,
etc.) resolve correctly under both themes with a single semantic key.

**Acceptance Scenarios**:

1. **Given** either theme, **When** `CheckBoxStyle` is applied,
   **Then** it resolves to the theme's checkbox style.

---

### User Story 4 — Lightweight Styling Resource Portability (Priority: P1)

A developer customizes a control's appearance using semantic lightweight styling
resource keys. The customization resolves under both themes.

**Why this priority**: Lightweight styling is the primary customization mechanism
for Uno Themes. Without resource-level portability, style-level portability is
incomplete — the control looks right by default but can't be customized portably.

**Acceptance Scenarios**:

1. **Given** Simple theme is active, **When** a developer reads
   `{ThemeResource FilledButtonForeground}`, **Then** it resolves to the same
   value as `SimplePrimaryButtonForeground`.
2. **Given** Material theme is active, **When** a developer reads
   `{ThemeResource FilledButtonForeground}`, **Then** it resolves to the value
   already defined in Material's ThemeDictionaries (no change needed).
3. **Given** either theme, **When** `{ThemeResource CheckBoxBackgroundChecked}`
   is read, **Then** it resolves to the theme's checked checkbox background brush.

**Full Override Parity**:

Both themes support overriding semantic resource keys:

- Under **Material**: Templates already reference unprefixed keys
  (e.g. `{ThemeResource FilledButtonForeground}`). No change needed.
- Under **Simple**: Templates reference `Simple`-prefixed keys which alias
  to the semantic keys (e.g. `{ThemeResource SimplePrimaryButtonForeground}`
  which aliases to `FilledButtonForeground`). Overriding either key works:
  - Override `SimplePrimaryButtonForeground` → template picks it up directly.
  - Override `FilledButtonForeground` → alias propagates the override.

4. **Given** Simple theme is active, **When** a developer overrides
   `FilledButtonForeground` in their app resources, **Then** the override takes
   effect — the button renders with the overridden brush.

**Many-to-One Mapping Constraint**:

Where multiple semantic styles map to the same Simple style (e.g.
`FilledTonalButtonStyle` and `OutlinedButtonStyle` both → `SimpleNeutralButtonStyle`),
the template can only reference **one** set of semantic resource keys. The
**first listed** semantic mapping is the canonical one the template references.
The other semantic key set is aliased to the canonical keys and provides read
parity but not independent override parity. This is an inherent constraint of
many-to-one mappings, not a design limitation.

| Simple Style | Canonical Semantic Keys | Aliased Semantic Keys |
|---|---|---|
| `SimpleNeutralButtonStyle` | `FilledTonalButton*` | `OutlinedButton*` (aliases → `FilledTonalButton*`) |

To override the Neutral button appearance, developers can use either
`FilledTonalButton*` keys or `SimpleNeutralButton*` keys (both reference the
same underlying resource).

---

### User Story 5 — Gap Awareness (Priority: P2)

When a developer uses a semantic key that has no Simple equivalent (e.g.
`CommandBarStyle`), the behavior is gracefully handled (control uses WinUI
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
- What happens when a developer overrides a semantic lightweight styling resource?
  → Under both themes, the override takes effect. Material templates reference
  semantic keys directly. Simple templates reference `Simple`-prefixed keys which
  alias to the semantic keys, so overriding the semantic key propagates through
  the alias chain. Overriding the `Simple`-prefixed key also works (template
  picks it up directly).
- What happens when two semantic style variants map to the same Simple style
  (e.g. `OutlinedButtonStyle` and `FilledTonalButtonStyle` both → `SimpleNeutralButtonStyle`)?
  → The template references `SimpleNeutralButton*` keys (which alias to the
  canonical `FilledTonalButton*` keys). Overriding `SimpleNeutralButtonForeground`
  or `FilledTonalButtonForeground` both work. The `OutlinedButton*` set is aliased
  to the canonical keys for read parity but does not independently affect the
  template. This is an inherent constraint of many-to-one, not a design limitation.
- What about Material-specific resource concepts (StateLayer, Elevation, Shadow)?
  → These are Material implementation details with no Simple equivalent. They are
  excluded from the semantic resource layer and documented as gaps.

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
- **FR-005**: This layer MUST NOT introduce new control styles. It consists of
  naming/aliasing for styles and resources, plus updating Simple's existing
  templates to reference semantic resource keys instead of `Simple`-prefixed keys.
- **FR-006**: Existing theme-prefixed keys (e.g. `MaterialFilledButtonStyle`,
  `SimplePrimaryButtonStyle`) MUST remain valid and unchanged. `Simple`-prefixed
  lightweight styling resource keys MUST become backwards-compatible aliases
  pointing to the corresponding semantic keys.
- **FR-007**: For every control with a semantic style mapping, Simple's control
  templates MUST reference `Simple`-prefixed lightweight styling resource keys
  (which alias to the semantic keys). This ensures overriding either the
  `Simple`-prefixed key or the semantic key affects the control.
- **FR-008**: Simple's ThemeDictionaries MUST define the semantic keys as the
  source of truth (mapping to the SDS foundation tokens), and MUST define the
  `Simple`-prefixed keys as aliases pointing to the semantic keys. Templates
  MUST reference the `Simple`-prefixed keys.
- **FR-009**: Material's control XAML files already define unprefixed lightweight
  styling resource keys. No changes are needed on the Material side.
- **FR-010**: Semantic resource keys MUST be limited to resources where a
  reasonable type-compatible Simple equivalent exists. Material-specific concepts
  (StateLayer, Elevation, FocusCircle, Shadow) MUST be omitted.
- **FR-011**: Semantic resource keys MUST be defined in **both** ThemeDictionary
  entries (`Light` and `Default`) in each Simple control XAML file.
- **FR-012**: For many-to-one style mappings, the template MUST reference the
  **canonical** semantic key set (first listed mapping). The other semantic key
  sets MUST be defined as aliases to the canonical keys.

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

FAB is a **Material-specific concept** but Simple provides **compatibility aliases**
that map FAB keys to existing Simple icon button styles. The 12 FAB aliases
(`FabStyle`, `SmallFabStyle`, `LargeFabStyle`, and their Surface/Secondary/Tertiary
variants) resolve to Simple icon button styles under Simple theme and Material FAB
styles under Material theme.

| Semantic Key | Material v2 | Simple | Notes |
|---|---|---|---|
| `FabStyle` | `MaterialFabStyle` | `SimpleIconButtonPrimaryStyle` | Primary icon button as FAB equivalent |
| `SmallFabStyle` | `MaterialSmallFabStyle` | `SimpleSmallIconButtonPrimaryStyle` | Small primary icon button |
| `LargeFabStyle` | `MaterialLargeFabStyle` | `SimpleIconButtonPrimaryStyle` | Same as default (no large variant) |
| `SecondaryFabStyle` | `MaterialSecondaryFabStyle` | `SimpleIconButtonNeutralStyle` | Neutral icon button |
| `SecondarySmallFabStyle` | `MaterialSecondarySmallFabStyle` | `SimpleSmallIconButtonNeutralStyle` | |
| `SecondaryLargeFabStyle` | `MaterialSecondaryLargeFabStyle` | `SimpleIconButtonNeutralStyle` | |
| `TertiaryFabStyle` | `MaterialTertiaryFabStyle` | `SimpleIconButtonSubtleStyle` | Subtle icon button |
| `TertiarySmallFabStyle` | `MaterialTertiarySmallFabStyle` | `SimpleSmallIconButtonSubtleStyle` | |
| `TertiaryLargeFabStyle` | `MaterialTertiaryLargeFabStyle` | `SimpleIconButtonSubtleStyle` | |
| `SurfaceFabStyle` | `MaterialSurfaceFabStyle` | `SimpleIconButtonNeutralStyle` | Neutral icon button |
| `SurfaceSmallFabStyle` | `MaterialSurfaceSmallFabStyle` | `SimpleSmallIconButtonNeutralStyle` | |
| `SurfaceLargeFabStyle` | `MaterialSurfaceLargeFabStyle` | `SimpleIconButtonNeutralStyle` | |

### ToggleButton

| Semantic Key | Material v2 | Simple | Notes |
|---|---|---|---|
| `TextToggleButtonStyle` | `MaterialTextToggleButtonStyle` | `SimpleDefaultToggleButtonStyle` | Simple has one base toggle style |
| `IconToggleButtonStyle` | `MaterialIconToggleButtonStyle` | `SimpleIconToggleButtonStyle` | Compact circular icon-only toggle |

### TextBox

| Semantic Key | Material v2 | Simple | Notes |
|---|---|---|---|
| `FilledTextBoxStyle` | `MaterialFilledTextBoxStyle` | `SimpleTextBoxStyle` | Simple "Input" = single SDS Input design |
| `OutlinedTextBoxStyle` | `MaterialOutlinedTextBoxStyle` | `SimpleOutlinedTextBoxStyle` | Surface background, visible border |

### PasswordBox

| Semantic Key | Material v2 | Simple | Notes |
|---|---|---|---|
| `FilledPasswordBoxStyle` | `MaterialFilledPasswordBoxStyle` | `SimplePasswordBoxStyle` | Simple has one style (filled) |
| `OutlinedPasswordBoxStyle` | `MaterialOutlinedPasswordBoxStyle` | `SimpleOutlinedPasswordBoxStyle` | Surface background, visible border |

### HyperlinkButton

| Semantic Key | Material v2 | Simple | Notes |
|---|---|---|---|
| `HyperlinkButtonStyle` | `MaterialHyperlinkButtonStyle` | `SimpleHyperlinkButtonStyle` | Primary underlined link |
| `SecondaryHyperlinkButtonStyle` | `MaterialSecondaryHyperlinkButtonStyle` | `SimpleSecondaryHyperlinkButtonStyle` | Secondary underlined link |

### ComboBox

| Semantic Key | Material v2 | Simple | Notes |
|---|---|---|---|
| `ComboBoxStyle` | `MaterialComboBoxStyle` | `SimpleComboBoxStyle` | Direct match |

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
| `ProgressBarStyle` | `MaterialProgressBarStyle` | `SimpleProgressBarStyle` | Horizontal indicator with brand colors |

### ProgressRing

| Semantic Key | Material v2 | Simple | Notes |
|---|---|---|---|
| `ProgressRingStyle` | `MaterialProgressRingStyle` | `SimpleProgressRingStyle` | Circular indicator with brand colors |

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
| `NavigationViewStyle` | `MaterialNavigationViewStyle` | `SimpleNavigationViewStyle` | Lightweight ThemeResource overrides |
| `NavigationViewItemStyle` | `MaterialNavigationViewItemStyle` | `SimpleNavigationViewItemStyle` | |

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
| `PipsPagerStyle` | `MaterialPipsPagerStyle` | `SimplePipsPagerStyle` | Pagination dots with selection states |

### RatingControl

| Semantic Key | Material v2 | Simple | Notes |
|---|---|---|---|
| `RatingControlStyle` | `MaterialRatingControlStyle` | `SimpleRatingControlStyle` | Star rating with brand colors |

### Flyout / MenuFlyout

| Semantic Key | Material v2 | Simple | Notes |
|---|---|---|---|
| `FlyoutPresenterStyle` | `MaterialFlyoutPresenterStyle` | `SimpleFlyoutPresenterStyle` | Surface color, corner radius, border |
| `MenuFlyoutPresenterStyle` | `MaterialMenuFlyoutPresenterStyle` | `SimpleMenuFlyoutPresenterStyle` | ✅ Direct match |
| `MenuFlyoutItemStyle` | `MaterialMenuFlyoutItemStyle` | `SimpleMenuFlyoutItemStyle` | ✅ Direct match |
| `ToggleMenuFlyoutItemStyle` | `MaterialToggleMenuFlyoutItemStyle` | `SimpleToggleMenuFlyoutItemStyle` | ✅ Direct match |
| `RadioMenuFlyoutItemStyle` | `MaterialRadioMenuFlyoutItemStyle` | `SimpleRadioMenuFlyoutItemStyle` | Radio bullet indicator |
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

## Semantic Lightweight Styling Resource Mappings

Beyond style aliases, each theme exposes **lightweight styling resources** —
ThemeDictionary entries (brushes, thicknesses, corner radii, etc.) that control
templates reference via `{ThemeResource}`. These are the knobs developers use to
customize appearance without re-templating.

**Material** already defines these with **unprefixed** names (e.g.
`FilledButtonForeground`) — these are the semantic keys.

**Simple** defines them with **`Simple`-prefixed** names (e.g.
`SimplePrimaryButtonForeground`).

This section specifies the aliases that Simple must add in its per-control XAML
ThemeDictionaries so that Material's unprefixed keys resolve under Simple.

### Convention

```
Semantic Resource Key = Material unprefixed ThemeDictionary key
```

The semantic key becomes the **source of truth** in Simple's ThemeDictionaries,
and Simple's templates reference the `Simple`-prefixed key (which aliases to the
semantic key). This allows overriding **either** the `Simple`-prefixed key **or**
the semantic key:

```xml
<!-- BEFORE (current Simple Button.xaml): -->
<!-- ThemeDictionary: -->
<StaticResource x:Key="SimplePrimaryButtonForeground" ResourceKey="SimpleTextBrandOnBrandBrush" />
<!-- Template: -->
<Setter Property="Foreground" Value="{ThemeResource SimplePrimaryButtonForeground}" />

<!-- AFTER (updated Simple Button.xaml): -->
<!-- ThemeDictionary — semantic key is source of truth: -->
<StaticResource x:Key="FilledButtonForeground" ResourceKey="SimpleTextBrandOnBrandBrush" />
<!-- ThemeDictionary — Simple-prefixed alias (referenced by template): -->
<StaticResource x:Key="SimplePrimaryButtonForeground" ResourceKey="FilledButtonForeground" />
<!-- Template — references Simple-prefixed key: -->
<Setter Property="Foreground" Value="{ThemeResource SimplePrimaryButtonForeground}" />
```

This ensures overriding **either** `SimplePrimaryButtonForeground` **or**
`FilledButtonForeground` takes effect:

- Override `SimplePrimaryButtonForeground` → works (template references it directly)
- Override `FilledButtonForeground` → works (`SimplePrimaryButtonForeground` aliases
  to it, so the override propagates through the alias chain)

### Mapping Pattern Per Control

The style-level mapping (e.g. `FilledButtonStyle → SimplePrimaryButtonStyle`) determines
the resource-level mapping prefix (e.g. `FilledButton* → SimplePrimaryButton*`).

For each mapped style variant, the resource properties follow this pattern:

| Resource Suffix | Example (Button) |
|---|---|
| `{Foreground\|Background\|BorderBrush}{State}` | `FilledButtonForeground`, `FilledButtonBackgroundPointerOver` |
| `Icon{Foreground}{State}` | `FilledButtonIconForegroundPressed` |
| Non-brush: `CornerRadius`, `MinHeight`, `Padding`, etc. | `FilledTextBoxCornerRadius` |

States: *(none/default)*, `PointerOver`, `Pressed`, `Disabled`, `Focused`,
`Checked`, `Unchecked`, `Indeterminate` — varies by control.

### Controls Requiring Aliases

Only controls where **both** conditions are met:
1. A semantic style mapping exists (not a GAP)
2. Material defines unprefixed lightweight styling resources

| Control | Simple XAML File | Alias Prefix Mapping | Approx. Aliases |
|---|---|---|---|
| Button | `Button.xaml` | `FilledButton*→SimplePrimaryButton*`, `FilledTonalButton*→SimpleNeutralButton*`, `OutlinedButton*→SimpleNeutralButton*`, `TextButton*→SimpleSubtleButton*`, `IconButton*→SimpleIconButtonPrimaryButton*` | ~75/dict |
| TextBox | `TextBox.xaml` | `FilledTextBox*→SimpleTextBox*`, `OutlinedTextBox*→FilledTextBox*` (many-to-one) | ~45/dict |
| PasswordBox | `PasswordBox.xaml` | `FilledPasswordBox*→SimplePasswordBox*` | ~25/dict |
| ComboBox | `ComboBox.xaml` | `ComboBox*→SimpleComboBox*` | ~40/dict |
| CheckBox | `CheckBox.xaml` | `CheckBox*→SimpleCheckBox*` (word-order differs) | ~60/dict |
| RadioButton | `RadioButton.xaml` | `RadioButton*→SimpleRadioButton*` (naming differs) | ~21/dict |
| ToggleSwitch | `ToggleSwitch.xaml` | `ToggleSwitch*→SimpleToggleSwitch*` (naming differs significantly) | ~12/dict |
| Slider | `Slider.xaml` | `Slider*→SimpleSlider*` | ~21/dict |
| ToggleButton | `ToggleButton.xaml` | `TextToggleButton*→SimpleToggleButton*` | ~39/dict |
| DatePicker | `DatePicker.xaml` | `DatePicker*→SimpleDatePicker*` | ~35/dict |
| CalendarDatePicker | `CalendarDatePicker.xaml` | `CalendarDatePicker*→SimpleCalendarDatePicker*` | ~24/dict |
| AppBarButton | `AppBarButton.xaml` | `AppBarButton*→SimpleAppBarButton*` | ~2/dict |

### Controls NOT Requiring Aliases

- **GAP controls** (no Simple style): CommandBar, MediaTransportControls
- **Controls where Material uses only `Material`-prefixed resource keys** (no
  unprefixed keys to alias): ListView, ContentDialog, CalendarView,
  MenuFlyoutPresenter, MenuFlyoutItem, MenuFlyoutSeparator, MenuFlyoutSubItem,
  ToggleMenuFlyoutItem

### Naming Challenges

Some controls have **different naming conventions** between themes that require
careful hand-mapping rather than mechanical prefix substitution:

| Control | Material Pattern | Simple Pattern | Example |
|---|---|---|---|
| CheckBox | `CheckBox{Property}{State}` | `SimpleCheckBox{State}{Property}` | `CheckBoxBackgroundChecked` → `SimpleCheckBoxCheckedBackground` |
| RadioButton | `RadioButton{Part}{State}` | `SimpleRadioButton{State}{Property}` | `RadioButtonOuterEllipseStroke` → `SimpleRadioButtonUncheckedBorderBrush` |
| ToggleSwitch | `ToggleSwitch{Knob\|KnobBounds\|OuterBorder}*` | `SimpleToggleSwitch{Thumb\|Track}*` | `ToggleSwitchKnobOnFill` → `SimpleToggleSwitchThumbOnFill` |

### Excluded Resource Types

The following Material resource categories are **excluded** from the semantic
resource layer (no Simple equivalent exists):

- **StateLayer resources** (`*StateLayerBackground*`) — Material-specific overlay pattern
- **Elevation/Shadow resources** (`*Elevation*`, `*Shadow*`) — Material-specific depth system
- **FocusCircle/StateCircle resources** — Material-specific focus indicators
- **Delete/Reveal button resources** — Internal sub-control styling
- **Opacity resources** (`*OpacityDisabled`) — Material uses opacity; Simple uses distinct brushes
- **Animation/transition resources** — Theme-specific motion

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
| `SimpleTextBoxErrorStyle` | TextBox | Dedicated error style |
| `SimpleTextBoxSmallStyle` | TextBox | Size variant |
| `SimpleComboBoxErrorStyle` | ComboBox | Dedicated error style |
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

| Semantic Key | Priority | Status |
|---|---|---|
| `ElevatedButtonStyle` | 🔴 High | ⚠️ **GAP** — Simple has no elevated/shadow button variant |
| `CommandBarStyle` | 🟡 Medium | ⚠️ **GAP** — Noted in Simple's `_Resources.xaml` TODO |
| `MediaTransportControlsStyle` | 🟠 Low | ⚠️ **GAP** — Niche control |

### Resolved Gaps (Issues #1639–#1650)

| Semantic Key | Resolution |
|---|---|
| `IconToggleButtonStyle` | ✅ `SimpleIconToggleButtonStyle` (#1640) |
| `OutlinedTextBoxStyle` | ✅ `SimpleOutlinedTextBoxStyle` (#1641) |
| `OutlinedPasswordBoxStyle` | ✅ `SimpleOutlinedPasswordBoxStyle` (#1642) |
| `HyperlinkButtonStyle` | ✅ `SimpleHyperlinkButtonStyle` (#1643) |
| `SecondaryHyperlinkButtonStyle` | ✅ `SimpleSecondaryHyperlinkButtonStyle` (#1643) |
| `RadioMenuFlyoutItemStyle` | ✅ `SimpleRadioMenuFlyoutItemStyle` (#1644) |
| `FlyoutPresenterStyle` | ✅ `SimpleFlyoutPresenterStyle` (#1645) |
| `NavigationViewStyle` | ✅ `SimpleNavigationViewStyle` (#1646) |
| `NavigationViewItemStyle` | ✅ `SimpleNavigationViewItemStyle` (#1646) |
| `ProgressBarStyle` | ✅ `SimpleProgressBarStyle` (#1647) |
| `ProgressRingStyle` | ✅ `SimpleProgressRingStyle` (#1648) |
| `PipsPagerStyle` | ✅ `SimplePipsPagerStyle` (#1649) |
| `RatingControlStyle` | ✅ `SimpleRatingControlStyle` (#1650) |
| FAB aliases (12 variants) | ✅ Mapped to existing Simple icon button styles (#1639) |

### Ambiguous Mappings — Design Decisions

| Semantic Key | Issue | Resolution |
|---|---|---|
| `FilledTonalButtonStyle` / `OutlinedButtonStyle` | Both map to `SimpleNeutralButtonStyle` | Intentional — Simple's Neutral covers both; no action needed |
| `IconButtonStyle` | Material has one icon button; Simple has 5 color variants | Mapped to `SimpleIconButtonPrimaryStyle` as default |
| FAB styles (all 12) | Material-specific concept | **Resolved**: Simple maps to existing icon button styles (#1639) |

---

## Success Criteria

- **SC-001**: Every semantic style key defined in this spec resolves correctly when the
  corresponding theme is active — verified by a test page rendering all styles
  under both Material and Simple.
- **SC-002**: No existing app using `Material`-prefixed or `Simple`-prefixed keys
  experiences any regression.
- **SC-003**: The gap table is accurate — every ⚠️ GAP entry truly has no
  reasonable Simple equivalent.
- **SC-004**: Both `_Resources.xaml` files are fully aligned with this spec's
  style mapping tables after implementation.
- **SC-005**: Every semantic lightweight styling resource key defined in this spec
  resolves to the correct default value under both themes — verified by reading
  the resource value at runtime under both Material and Simple.
- **SC-006**: Overriding a semantic resource key (e.g. `FilledButtonForeground`)
  in app resources takes effect under **both** themes for 1:1 mappings.
- **SC-007**: Simple's per-control XAML ThemeDictionaries define semantic keys as
  the source of truth and `Simple`-prefixed keys as backwards-compatible aliases,
  in both `Light` and `Default` dicts.
- **SC-008**: Simple's control templates reference `Simple`-prefixed keys
  (which alias to semantic keys), enabling override at either level.

---

## Implementation Notes

### Mechanism

The semantic layer has two tiers:

**Tier 1 — Style aliases** (in `_Resources.xaml`):
`<StaticResource>` aliases that map semantic style names to theme-specific styles.

```xml
<!-- In Material _Resources.xaml (already exists): -->
<StaticResource x:Key="FilledButtonStyle" ResourceKey="MaterialFilledButtonStyle" />

<!-- In Simple _Resources.xaml (update needed): -->
<StaticResource x:Key="FilledButtonStyle" ResourceKey="SimplePrimaryButtonStyle" />
```

**Tier 2 — Lightweight styling resources** (in per-control XAML files):
Simple's templates and ThemeDictionaries are updated so that semantic resource
keys are the source of truth and templates reference them directly.

```xml
<!-- In Simple's Button.xaml ThemeDictionaries (Light and Default): -->

<!-- Semantic key — source of truth: -->
<StaticResource x:Key="FilledButtonForeground" ResourceKey="SimpleTextBrandOnBrandBrush" />
<StaticResource x:Key="FilledButtonBackground" ResourceKey="SimpleBackgroundBrandDefaultBrush" />

<!-- Simple-prefixed alias — referenced by template: -->
<StaticResource x:Key="SimplePrimaryButtonForeground" ResourceKey="FilledButtonForeground" />
<StaticResource x:Key="SimplePrimaryButtonBackground" ResourceKey="FilledButtonBackground" />
```

```xml
<!-- In Simple's Button.xaml template — references Simple-prefixed keys: -->
<Setter Property="Foreground" Value="{ThemeResource SimplePrimaryButtonForeground}" />
<Setter Property="Background" Value="{ThemeResource SimplePrimaryButtonBackground}" />
```

Material's control XAML files already define unprefixed resource keys — no
Material-side changes are needed for Tier 2.

### Resource Chain

For a 1:1 mapping (e.g. `FilledButton → SimplePrimary`):

```
Template references:     {ThemeResource SimplePrimaryButtonForeground}
ThemeDictionary defines: FilledButtonForeground → SimpleTextBrandOnBrandBrush   (semantic = source of truth)
Template-level alias:    SimplePrimaryButtonForeground → FilledButtonForeground  (referenced by template)
```

- Override `SimplePrimaryButtonForeground` → works (template references it directly)
- Override `FilledButtonForeground` → works (alias propagates the override)

For a many-to-one mapping (e.g. `FilledTonal + Outlined → SimpleNeutral`):

```
Template references:     {ThemeResource SimpleNeutralButtonForeground}
ThemeDictionary defines: FilledTonalButtonForeground → SimpleTextDefaultDefaultBrush  (canonical semantic key)
Template-level alias:    SimpleNeutralButtonForeground → FilledTonalButtonForeground
Read-parity alias:       OutlinedButtonForeground → FilledTonalButtonForeground
```

- Override `SimpleNeutralButtonForeground` → works (template references it directly)
- Override `FilledTonalButtonForeground` → works (alias propagates the override)
- Override `OutlinedButtonForeground` → read parity only (does not affect template)

### What Needs to Change

1. **Audit Material `_Resources.xaml`** — Verify all semantic style keys from this spec
   are present. Material's file is largely complete already.

2. **Update Simple `_Resources.xaml`** — Add missing semantic style aliases, remove or
   reorganize the current ad-hoc "legacy / backwards-compatible aliases" section
   to align with this spec.

3. **Document style gaps** — Add XAML comments in Simple's `_Resources.xaml` for each
   gap, referencing this spec.

4. **Update Simple's per-control XAML ThemeDictionaries** — For each control listed
   in the "Controls Requiring Aliases" table:
   a. Rename `Simple`-prefixed resource keys to their semantic equivalents
      (the semantic key becomes the source of truth, pointing to the SDS
      foundation token).
   b. Add `Simple`-prefixed keys as backwards-compatible aliases pointing to
      the semantic keys.
   c. For many-to-one mappings, add the non-canonical semantic keys as aliases
      pointing to the canonical semantic keys.
   d. Apply to **both** `Light` and `Default` ThemeDictionaries.

5. **Update Simple's control templates** — Change all `{ThemeResource SemanticKey}`
   references in template setters and visual states to reference the
   corresponding `Simple`-prefixed keys instead (which alias to the semantic keys).
   This ensures overriding either key affects the control.

6. **Document resource gaps** — For Material resource keys with no type-compatible
   Simple equivalent (StateLayer, Elevation, etc.), add XAML comments noting the
   gap.

### Relationship to Simple-Native Aliases

Simple's `_Resources.xaml` currently has **two alias sections**:
- "Theme-agnostic aliases (preferred — matches Figma SDS naming)" — e.g. `PrimaryButtonStyle`
- "Legacy / backwards-compatible aliases" — e.g. `FilledButtonStyle`

After this spec is implemented, the structure should be:
1. **Semantic aliases** (this spec) — Material-derived names like `FilledButtonStyle`
2. **SDS-native aliases** — Simple's own vocabulary like `PrimaryButtonStyle`, `NeutralButtonStyle`
3. Both coexist; neither replaces the other.
