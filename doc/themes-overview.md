---
uid: Uno.Themes.Overview
---

# Themes Overview

<p align="center">
  <img src="assets/themes-design-systems.png" alt="Themes design systems" />
</p>

> [!IMPORTANT]
> UnoFeatures: **Material**, **Cupertino**, or **SimpleTheme** — enable these themes by adding `<UnoFeatures>Material</UnoFeatures>`, `<UnoFeatures>Cupertino</UnoFeatures>`, or `<UnoFeatures>SimpleTheme</UnoFeatures>` to your app's `.csproj`.

## Summary

- [Material Overview](material-getting-started.md)
- [Cupertino Overview](cupertino-getting-started.md)
- [Simple Overview](simple-getting-started.md)
- [Omarchy Overview](omarchy-getting-started.md)
- [Fluent Overview](fluent-getting-started.md)

Cross-cutting guides shared by the theme libraries:

- [Seed Color Palette](seed-colors.md) — generate the full Light and Dark color theme from a single seed color (Material and Simple)
- [Semantic Styles](semantic-styles.md) — design-system-agnostic style names, so you can switch themes without changing your XAML
- [Design Tokens](design-tokens.md) — spacing, shape, and density values you can adjust globally
- [Lightweight Styling](lightweight-styling.md) — override individual colors, brushes, and sizes per control without redefining the control template

## Uno Themes Styles

[Uno Themes](https://github.com/unoplatform/Uno.Themes) is the repository for add-ons enabled through UnoFeatures that can be added to any new or existing Uno solution.

It contains five libraries:

- `Uno Themes`: a library that contains the base resources, extensions, and helper classes for the different design system libraries
- `Uno Material`: a library that contains styles following the [Material 3](https://m3.material.io/) Design System
- `Uno Cupertino`: a library that contains styles following the [Human Interface Guidelines](https://developer.apple.com/design/human-interface-guidelines)
- `Uno Simple`: a library that contains styles following the [Figma Simple Design System](https://www.figma.com/community/file/1380235722331273046)
- `Uno Omarchy`: a library that contains terminal-inspired styles following the [Omarchy](https://omarchy.org) design system (a port of [flutter_omarchy](https://github.com/aloisdeniel/flutter_omarchy)), with its 22 stock palettes

`Material`, `Cupertino`, `Simple`, and `Omarchy` libraries help you style your application with a few lines of code including:

- Color system for both Light and Dark themes — customizable wholesale from a single [seed color](seed-colors.md), or key-by-key via overrides, with helper classes such as `SemanticThemeHelper` for changing colors at runtime
- Styles for existing WinUI controls like Buttons, TextBox, etc.

## Fluent Controls Styles

Uno Platform 3.0 and above supports control styles conforming to the [Fluent design system](https://www.microsoft.com/design/fluent).
