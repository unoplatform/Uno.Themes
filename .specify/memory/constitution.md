# Uno.Themes Constitution

## Core Principles

### I. Design-System Fidelity

Every theme in this repository is a pixel-faithful translation of an external
design system (Material Design, Cupertino, Simple Design System / SDS, etc.).
Styles, colors, typography, spacing, and states MUST match the source design
specification. When in doubt, the Figma source of truth wins.

### II. Theme Isolation & Parity

Each theme (Material, Cupertino, Simple) is a self-contained library that:

- Lives under `src/library/Uno.<ThemeName>.WinUI/`
- Exposes styles via a `<ThemeName>Theme.cs` entry point extending `BaseTheme`
- Uses `<ThemeName>`-prefixed style keys (e.g., `SimplePrimaryButtonStyle`,
  `MaterialFilledButtonStyle`)
- Maintains its own `ColorPalette.xaml`, color brushes, and typography resources
- Can be consumed independently — no cross-theme dependencies

New themes MUST follow the same structural pattern. All themes SHOULD aspire
to cover the same control surface area over time.

### III. Naming Convention — Follow the Design System

Style key names MUST reflect the **design system's vocabulary**, not WinUI/UWP
implementation concepts:

- **Simple/SDS**: `SimplePrimaryButtonStyle`, `SimpleInputTextBoxStyle`,
  `SimpleSelectFieldStyle` — names match the Figma component naming
- **Material**: `MaterialFilledButtonStyle`, `MaterialOutlinedCardStyle` —
  names match Material Design 3 naming
- **Cupertino**: `CupertinoDefaultButtonStyle` — names match Apple HIG naming

When a design system renames a component, styles MUST be renamed to match.
Legacy aliases (e.g., `FilledButtonStyle` → `SimplePrimaryButtonStyle`) are
provided in `_Resources.xaml` for backwards compatibility but the canonical
key is always the design-system-native name.

### IV. Layered Architecture

The styling stack has clear layers with distinct responsibilities:

| Layer | Location | Purpose |
|-------|----------|---------|
| **Color Palette** | `ColorPalette.xaml` | Raw color values from design system |
| **Semantic Brushes** | `<Theme>Colors.xaml` | Named brushes (`PrimaryBrush`, `SurfaceBrush`, etc.) |
| **Typography** | `Fonts.xaml` | Font families, sizes, weights as named styles |
| **Control Styles** | `Styles/Controls/<Control>.xaml` | Full style + ControlTemplate per control |
| **Implicit Styles & Aliases** | `_Resources.xaml` | Default implicit styles + theme-agnostic keys |
| **Theme Entry Point** | `<Theme>Theme.cs` | Loads all resources via merged dictionaries |

New controls MUST populate all layers. Skipping a layer (e.g., hardcoding
colors instead of using semantic brushes) is not permitted.

### V. Toolkit Boundary

**Uno.Themes** provides styles for **WinUI built-in controls** (Button, TextBox,
CheckBox, ComboBox, ToggleSwitch, PersonPicture, etc.).

**Uno.Toolkit.UI** provides additional controls (Card, CardContentControl, Chip,
TabBar, NavigationBar, etc.) and their theme-specific styles live in companion
libraries (e.g., `Uno.Toolkit.Material`, `Uno.Toolkit.Simple`).

Styles targeting Toolkit controls MUST NOT be placed in Uno.Themes. They belong
in the corresponding Toolkit theme library, which references both
`Uno.Toolkit.WinUI` (for control types) and `Uno.<Theme>.WinUI` (for brushes).

### VI. Resource Key Lightweight Styling

All control styles SHOULD expose lightweight styling resource keys in
`ThemeDictionaries` so consumers can override individual visual properties
(background, border, corner radius, etc.) without re-templating. Follow the
naming pattern:

```
<Variant><ControlType><Property>
```

Example: `FilledCardContentBackground`, `OutlinedCardContentBorderBrush`,
`PrimaryButtonForeground`.

### VII. Sample App Coverage

Every named style key (`x:Key="Simple*"`, `x:Key="Material*"`, etc.) MUST have
a corresponding sample in its theme's sample app. The sample MUST:

- Use `<smtx:XamlDisplay>` to show live XAML source alongside the rendered control
- Demonstrate the style's normal, disabled, and variant states
- Use the theme-agnostic alias where one exists (to validate the alias chain)

### VIII. Build & Quality Gates

- All target frameworks (`net10.0-desktop`, `net10.0-browserwasm`,
  `net10.0-android`) MUST build with **zero errors** before merge
- Warnings from `CS8602` (null dereference) and `NU1507` (package source) are
  tracked but not blocking
- XAML should pass `XamlStyler` formatting (see `Settings.XamlStyler`)
- No hardcoded hex colors in control XAML — always reference semantic brush
  resources

## Theme-Specific Addenda

Each theme has its own addendum document that extends this constitution with
theme-specific rules:

| Theme | Addendum |
|-------|----------|
| Simple (SDS) | [`.specify/specs/simple-theme-spec.md`](../specs/simple-theme-spec.md) |
| Material | [`.specify/specs/material-theme-spec.md`](../specs/material-theme-spec.md) |
| Cupertino | [`.specify/specs/cupertino-theme-spec.md`](../specs/cupertino-theme-spec.md) |
| Simple Toolkit | [`.specify/specs/simple-toolkit-styles/`](../specs/simple-toolkit-styles/) (spec + plan + tasks) |

## Governance

- This constitution supersedes ad-hoc practices and historical precedent
- Amendments require PR review and sign-off from at least one maintainer
- All PRs and code reviews MUST verify compliance with these principles
- When principles conflict, higher-numbered principles yield to lower-numbered
  ones (design-system fidelity is paramount)

**Version**: 1.0.0 | **Ratified**: 2026-02-16 | **Last Amended**: 2026-02-16
