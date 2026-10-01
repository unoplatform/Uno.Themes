---
uid: Uno.Themes.Simple.GetStarted
---

# Uno Simple

> [!IMPORTANT]
> UnoFeatures: **SimpleTheme** — add `<UnoFeatures>SimpleTheme</UnoFeatures>` to your app's `.csproj` to enable Uno Simple.

Uno Simple is enabled through the `SimpleTheme` UnoFeatures and lets you apply Simple Design System (SDS) styling to your application with a few lines of code.

## Getting Started

> [!NOTE]
> Make sure to setup your environment first by [following our instructions](xref:Uno.GetStarted.vs2022).

### Creating a new project with Uno Simple

1. Install the [`dotnet new` CLI templates](xref:Uno.GetStarted.dotnet-new) with:

    ```bash
    dotnet new install Uno.Templates
    ```

2. Create a new application with:

    ```bash
    dotnet new unoapp -o UnoSimpleApp -theme simple
    ```

### Installing Uno Simple in an existing project

Depending on the type of project template that the Uno Platform application was created with, follow the instructions below to install Uno Simple.

#### [**Single Project Template**](#tab/singleproj)

1. Edit your project file (`PROJECT_NAME.csproj`) and add `SimpleTheme` to the list of `UnoFeatures`:

    ```xml
    <UnoFeatures>SimpleTheme</UnoFeatures>
    ```

2. Initialize the Simple theme resources in the `App.xaml`:

    ```xml
    <Application.Resources>
        <ResourceDictionary>
            <ResourceDictionary.MergedDictionaries>

                <!-- Code omitted for brevity -->

                <us:SimpleTheme xmlns:us="using:Uno.Simple" />
            </ResourceDictionary.MergedDictionaries>
        </ResourceDictionary>
    </Application.Resources>
    ```

#### [**Multi-Head Project Template (Legacy)**](#tab/multihead)

> [!NOTE]
> Use this only when working on older multi-head templates that do not support `UnoFeatures`. Modern templates should use the Single Project instructions above.

1. In the Solution Explorer panel, right-click on your app's **App Code Library** project (`PROJECT_NAME.csproj`) and select `Manage NuGet Packages...`
1. Install the [`Uno.Simple.WinUI`](https://www.nuget.org/packages/Uno.Simple.WinUI)
1. Add the following Simple resources to `AppResources.xaml`:

    ```xml
    <ResourceDictionary>
        <ResourceDictionary.MergedDictionaries>

            <us:SimpleTheme xmlns:us="using:Uno.Simple" />

        </ResourceDictionary.MergedDictionaries>
    </ResourceDictionary>
    ```

---

## Customization

### Seed Color Palette

The fastest way to a custom color theme: provide a single **seed color** — typically your brand color — and the library generates the complete Light and Dark palette from it, with readable text contrast built in. No resource dictionaries to write, and the seed can even be changed at runtime. See the [Seed Color Palette documentation](xref:Uno.Themes.SeedColors).

The guides below give you full manual control instead. They require the creation of new `ResourceDictionary` files in your application project. For more information on how to define styles and resources in a separate `ResourceDictionary`, refer to the [resource management documentation](xref:Guide.HowTo.Create-Control-Library#moving-the-control-style-in-a-separate-resource-dictionary).

### Customize Color Palette

You can override the default Simple color palette by providing a `ResourceDictionary` with color overrides through the `Colors` property (a `ThemeColors` object):

```xml
<us:SimpleTheme xmlns:us="using:Uno.Simple">
    <us:SimpleTheme.Colors>
        <ut:ThemeColors xmlns:ut="using:Uno.Themes">
            <ut:ThemeColors.OverrideDictionary>
                <ResourceDictionary>
                    <!-- Add color overrides here -->
                </ResourceDictionary>
            </ut:ThemeColors.OverrideDictionary>
        </ut:ThemeColors>
    </us:SimpleTheme.Colors>
</us:SimpleTheme>
```

### Customize Fonts

Uno Simple ships with the [Inter](https://fonts.google.com/specimen/Inter) font family. Every type scale derives from the single `DefaultFontFamily` root token, so to use a different font for the whole app, set the `DefaultFontFamily` property on the theme:

```xml
<us:SimpleTheme xmlns:us="using:Uno.Simple"
                DefaultFontFamily="ms-appx:///Assets/Fonts/MyCustomFont.ttf#MyCustomFont" />
```

Alternatively, redefine the root token in a `ResourceDictionary` provided as font overrides:

```xml
<us:SimpleTheme xmlns:us="using:Uno.Simple">
    <us:SimpleTheme.FontOverrideDictionary>
        <ResourceDictionary>
            <FontFamily x:Key="DefaultFontFamily">ms-appx:///Assets/Fonts/MyCustomFont.ttf#MyCustomFont</FontFamily>
        </ResourceDictionary>
    </us:SimpleTheme.FontOverrideDictionary>
</us:SimpleTheme>
```

A root-only override cascades to the type scales only when the `DefaultFontFamily` property is left unset and the theme is merged at the application level; otherwise redefine the individual `*FontFamily` keys (`BodyMediumFontFamily`, …). See [Typography Font Swap](design-tokens.md#typography-font-swap).

### Spacing, Density & Shape

The spacing and shape scales, and the typeface, are generated from a few properties `SimpleTheme` inherits from `BaseTheme`:

| Property              | Type         | Description                                                                                                         |
|-----------------------|--------------|---------------------------------------------------------------------------------------------------------------------|
| `DefaultSpacing`      | `double`     | Base spacing unit (default 4) generating the `Space*` tokens, scaled by the `DefaultDensity` mode.                  |
| `DefaultDensity`      | `Density`    | Density mode (`Compact` / `Regular` / `Comfy`) scaling the spacing base unit by ×0.75 / ×1 / ×1.25.                 |
| `DefaultCornerRadius` | `double`     | Base corner-radius unit (default 4) generating the `Radius*` tokens.                                                |
| `DefaultFontFamily`   | `FontFamily` | The font the whole type scale is generated from; left unset, Inter stands. See [Customize Fonts](#customize-fonts). |

```xml
<us:SimpleTheme xmlns:us="using:Uno.Simple"
                DefaultSpacing="6"
                DefaultDensity="Compact"
                DefaultCornerRadius="2" />
```

See [Design Tokens](design-tokens.md) for the generated scales and how runtime changes reach controls.

Simple's control styles consume the shared tokens through a layer of `Simple`-prefixed aliases declared in `Styles/Application/Common/Thickness.xaml`. Each alias resolves to the shared token it names, so overriding a shared token or changing a scale property reaches it:

- `SimpleSpace*` for every `Space*` step, and `SimpleSpace*Thickness` for the steps `0` through `800`; `SimpleSpace200HorizontalThickness` and `SimpleSpace400HorizontalThickness`.
- `SimpleRadius*` and `SimpleRadius*CornerRadius` for the steps `050` through `400`, plus `SimpleRadiusFull` / `SimpleRadiusFullCornerRadius`.
- `SimpleIconSmall` / `SimpleIconMedium` (aliases of `IconSizeMedium` / `IconSizeLarge`) and a fixed `SimpleIconLarge` (40).
- `SimpleStrokeBorder` / `SimpleStrokeFocusRing` (and their `*Thickness` companions), which are fixed Simple values.
