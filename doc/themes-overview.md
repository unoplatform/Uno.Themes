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
- [Fluent Overview](fluent-getting-started.md)

## Uno Themes Styles

[Uno Themes](https://github.com/unoplatform/Uno.Themes) is the repository for add-ons enabled through UnoFeatures that can be added to any new or existing Uno solution.

It contains five libraries:

- `Uno Themes`: a library that contains the base resources, extensions, and helper classes for the different design system libraries
- `Uno Material`: a library that contains styles following the [Material 3](https://m3.material.io/) Design System
- `Uno Cupertino`: a library that contains styles following the [Human Interface Guidelines](https://developer.apple.com/design/human-interface-guidelines)
- `Uno Simple`: a library that contains styles following the [Figma Simple Design System](https://www.figma.com/community/file/1380235722331273046)
- `Uno Fluent`: an adapter library that maps the Uno Themes semantic layer onto the built-in [Fluent design system](https://www.microsoft.com/design/fluent) styles provided by WinUI and Uno Platform

`Material`, `Cupertino`, and `Simple` libraries help you style your application with a few lines of code including:

- Color system for both Light and Dark themes
- Styles for existing WinUI controls like Buttons, TextBox, etc.

## Fluent Controls Styles

Uno Platform 3.0 and above supports control styles conforming to the [Fluent design system](https://www.microsoft.com/design/fluent) out of the box, via `XamlControlsResources`.

On top of that, the `Uno.Fluent.WinUI` library (`FluentTheme`) lets apps written against the Uno Themes [semantic styles](semantic-styles.md) — semantic style keys, color palette, and typography — render with the platform-default Fluent look, without rewriting their XAML against the WinUI style names. See [Fluent getting started](fluent-getting-started.md).
