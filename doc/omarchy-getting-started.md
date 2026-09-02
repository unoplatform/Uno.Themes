---
uid: Uno.Themes.Omarchy.GetStarted
---

# Uno Omarchy

Uno Omarchy brings the look of [Omarchy](https://omarchy.org) — the terminal-inspired Arch Linux setup — to Uno Platform applications. It is a port of the [flutter_omarchy](https://github.com/aloisdeniel/flutter_omarchy) design system: one monospace face, sharp corners, 2 px borders, translucent tinted fills, an accent stroke for focus and selection, and the 8-color ANSI palette (normal and bright) that terminal themes are built on.

The package ships the 22 stock Omarchy palettes (Tokyo Night, Catppuccin, Nord, Gruvbox, Everforest, Rosé Pine, …) and the CaskaydiaMono Nerd Font Mono face.

## Getting Started

> [!NOTE]
> Make sure to setup your environment first by [following our instructions](xref:Uno.GetStarted.vs2022).

### Installing Uno Omarchy

1. In the Solution Explorer panel, right-click on your app project (`PROJECT_NAME.csproj`) and select `Manage NuGet Packages...`
1. Install the [`Uno.Omarchy.WinUI`](https://www.nuget.org/packages/Uno.Omarchy.WinUI) package
1. Initialize the Omarchy theme resources in the `App.xaml`:

    ```xml
    <Application.Resources>
        <ResourceDictionary>
            <ResourceDictionary.MergedDictionaries>

                <!-- Code omitted for brevity -->

                <uo:OmarchyTheme xmlns:uo="using:Uno.Omarchy" />
            </ResourceDictionary.MergedDictionaries>
        </ResourceDictionary>
    </Application.Resources>
    ```

Every control the theme covers picks up its Omarchy style implicitly; the [semantic style keys](semantic-styles.md) (`FilledButtonStyle`, `OutlinedTextBoxStyle`, …) resolve to the Omarchy styles, so XAML written against them works unchanged. See [Omarchy Controls Styles](omarchy-controls-styles.md) for the full list of styles and resources.

## Palettes

An Omarchy theme is a single **palette** (`OmarchyPalette`): the values of an Omarchy `colors.toml` — `Background`, `Foreground`, `Accent`, `Selection`, `Muted`, and the `Normal` and `Bright` ANSI colors (Black, White, Red, Green, Yellow, Blue, Magenta, Cyan). The default is Tokyo Night, as in Omarchy itself.

### Picking a stock palette

Name any of the stock palettes in XAML (matching ignores case, spaces and dashes, so the Omarchy theme slug works too):

```xml
<uo:OmarchyTheme xmlns:uo="using:Uno.Omarchy" Palette="Nord" />
<uo:OmarchyTheme xmlns:uo="using:Uno.Omarchy" Palette="catppuccin-latte" />
```

Or in code:

```csharp
var theme = new OmarchyTheme { Palette = OmarchyPalettes.Gruvbox };
```

`OmarchyPalettes.All` lists every stock palette; `OmarchyPalettes.FromName(...)` resolves a name.

### Switching palettes at runtime

The palette can be changed at any time — the equivalent of `omarchy theme set`. Every Omarchy brush and every shared semantic brush is rewritten in place, so controls already on screen repaint:

```csharp
if (Application.Current.GetTheme() is OmarchyTheme theme)
{
    theme.Palette = OmarchyPalettes.RosePine;
}
```

### Light and dark

A palette is either light or dark (`OmarchyPalette.IsLight`); it has no light and dark variants. The theme therefore applies the same colors under both the `Light` and `Dark` XAML themes. To follow the system theme, switch palettes — for example `CatppuccinLatte`, `FlexokiLight` or `White` for light mode:

```csharp
theme.Palette = root.ActualTheme == ElementTheme.Light
    ? OmarchyPalettes.CatppuccinLatte
    : OmarchyPalettes.Catppuccin;
```

### Custom palettes

`OmarchyPalette` is a record; build one from your own `colors.toml` values:

```csharp
var mine = OmarchyPalettes.TokyoNight with
{
    Name = "Mine",
    Accent = Color.FromArgb(0xFF, 0xFF, 0x9E, 0x64),
};
```

## Using the palette in your XAML

The active palette is exposed as `Omarchy*Color` and `Omarchy*Brush` resources — `OmarchyBackgroundBrush`, `OmarchyForegroundBrush`, `OmarchyAccentBrush`, `OmarchySelectionBrush`, `OmarchyMutedBrush`, and `OmarchyNormal{Black|White|Red|Green|Yellow|Blue|Magenta|Cyan}Brush` / `OmarchyBright{…}Brush`:

```xml
<TextBlock Text="error" Foreground="{ThemeResource OmarchyBrightRedBrush}" />
```

The shared semantic roles are mapped from the palette as well (`PrimaryBrush` is the accent, `ErrorBrush` is normal red, `SurfaceVariantBrush` is the `lighter_background`, …), so theme-agnostic XAML written for Material or Simple keeps working — see the mapping table in [Omarchy Controls Styles](omarchy-controls-styles.md#semantic-color-roles).

Most controls come in an **ANSI accent variant** too, mirroring the `accent` parameter of the flutter_omarchy widgets: `OmarchyFilledButtonRedStyle`, `OmarchyOutlinedButtonGreenStyle`, `OmarchyCheckBoxBlueStyle`, `OmarchyToggleSwitchMagentaStyle`, `OmarchySliderCyanStyle`, …

## Customization

### Colors

Individual semantic colors can still be overridden through the `Colors` property, exactly as with the other themes — overrides take precedence over the palette:

```xml
<uo:OmarchyTheme xmlns:uo="using:Uno.Omarchy" Palette="Nord">
    <uo:OmarchyTheme.Colors>
        <ut:ThemeColors xmlns:ut="using:Uno.Themes">
            <ut:ThemeColors.OverrideDictionary>
                <ResourceDictionary>
                    <Color x:Key="ErrorColor">#FFBF616A</Color>
                </ResourceDictionary>
            </ut:ThemeColors.OverrideDictionary>
        </ut:ThemeColors>
    </uo:OmarchyTheme.Colors>
</uo:OmarchyTheme>
```

### Fonts

Omarchy uses the CaskaydiaMono Nerd Font Mono face bundled with the package (regular, bold and italic cuts). To use another monospace face, override the font resources:

```xml
<uo:OmarchyTheme xmlns:uo="using:Uno.Omarchy">
    <uo:OmarchyTheme.FontOverrideDictionary>
        <ResourceDictionary>
            <FontFamily x:Key="OmarchyRegularFontFamily">ms-appx:///Fonts/JetBrainsMono-Regular.ttf#JetBrains Mono</FontFamily>
            <FontFamily x:Key="OmarchyBoldFontFamily">ms-appx:///Fonts/JetBrainsMono-Bold.ttf#JetBrains Mono</FontFamily>
            <FontFamily x:Key="OmarchyItalicFontFamily">ms-appx:///Fonts/JetBrainsMono-Italic.ttf#JetBrains Mono</FontFamily>
        </ResourceDictionary>
    </uo:OmarchyTheme.FontOverrideDictionary>
</uo:OmarchyTheme>
```

### Spacing and density

`DefaultSpacing` and `DefaultDensity` work as for the other themes (see [Design Tokens](design-tokens.md)). `DefaultCornerRadius` is accepted but no Omarchy style consumes the `Radius*` scale: sharp corners are the identity of the theme.
