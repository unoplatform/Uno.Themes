---
uid: Uno.Themes.Material.GetStarted
---

# Uno.Material

<p align="center">
  <img src="assets/material-design-system.png" alt="Material design system" />
</p>

The Uno.Material library is available as NuGet packages that can be added to any new or existing Uno solution.
Uno Material lets you apply [Material Design 3](https://m3.material.io/) styling to your application with just a few lines of code.

> [!WARNING]
> If you are updating Uno.Material to v2 from an older 1.x version of the package, additional steps will be required. Refer to the [Uno Material Migration Guide](material-migration.md).

## Getting Started

Initialization of the Uno.Material resources is handled by the specialized `MaterialTheme` ResourceDictionary.

### `MaterialTheme`

#### Constructors

| Constructor    | Description                                           |
|----------------|-------------------------------------------------------|
| MaterialTheme()         | Initializes a new instance of the `MaterialTheme` resource dictionary.       |
| MaterialTheme(ResourceDictionary? colorOverride, ResourceDictionary? fontOverride)         | Initializes a new instance of the `MaterialTheme` resource dictionary and applies the given overrides       |

#### Properties

| Property                  | Type              | Description                                                                                   |
|---------------------------|-------------------|-----------------------------------------------------------------------------------------------|
| ColorOverrideSource             | string            | (Optional) Gets or sets a Uniform Resource Identifier that provides the source location of a ResourceDictionary containing overrides for the default Uno.Material Color resources                                            |
| FontOverrideSource     | string      | (Optional) Gets or sets a Uniform Resource Identifier that provides the source location of a ResourceDictionary containing overrides for the default Uno.Material font resources            |

> [!NOTE]
> Uno Material also has support for C# Markup through a [Uno.Material.WinUI.Markup](https://www.nuget.org/packages/Uno.Material.WinUI.Markup) NuGet Package. To get started with Uno Material in your C# Markup application, add the `Uno.Material.WinUI.Markup` NuGet package to your **App Code Library** project and your platform heads.

> [!NOTE]
> As of [Uno Platform 4.7](https://platform.uno/blog/uno-platform-4-7-new-project-template-performance-improvements-and-more/), the solution template of an Uno app has changed. There is no longer a Shared project (.shproj), it has been replaced with a regular cross-platform library containing all user code files, referred to as the **App Code Library** project. This also implies that package references can be included in a single location without the previous need to include those in all project heads.

1. Create a new Uno project using the `Uno Platform App` template.
2. In the Solution Explorer panel, right-click on your app's **App Code Library** project (`PROJECT_NAME.csproj`) and select `Manage NuGet Packages...`
3. Install the [`Uno.Material.WinUI`](https://www.nuget.org/packages/Uno.Material.WinUI) for XAML or [`Uno.Material.WinUI.Markup`](https://www.nuget.org/packages/Uno.Material.WinUI.Markup) for C# Markup.
4. Add the following Material resources to `AppResources`:

# [**XAML**](#tab/xaml)

<ResourceDictionary>
    <ResourceDictionary.MergedDictionaries>
        <!-- Load Uno.Material resources -->
        <MaterialTheme xmlns="using:Uno.Material" />
    </ResourceDictionary.MergedDictionaries>
</ResourceDictionary>

# [**C#**](#tab/csharp)

using Uno.Material;

public sealed class AppResources : ResourceDictionary
{
    public AppResources()
    {
        // Load Uno.Material resources
        this.Build(r => r.Merged(
            new MaterialTheme()));
    }
}

***

### Installing Uno.Material on previous versions of Uno Platform

If your application is based on the older solution template that includes a shared project (.shproj), follow these steps:

1. Open an existing Uno project
2. In the Solution Explorer panel, right-click on your solution name and select `Manage NuGet Packages for Solution ...`. Choose either:
     - The [`Uno.Material`](https://www.nuget.org/packages/Uno.Material/) package when targetting Xamarin/UWP
     - The [`Uno.Material.WinUI`](https://www.nuget.org/packages/Uno.Material.WinUI) package for XAML or [`Uno.Material.WinUI.Markup`](https://www.nuget.org/packages/Uno.Material.WinUI.Markup) for C# Markup, when targetting net6.0+/WinUI

3. Select the following projects for installation:
    - `PROJECT_NAME.Wasm.csproj`
    - `PROJECT_NAME.Mobile.csproj` (or `PROJECT_NAME.iOS.csproj`, `PROJECT_NAME.Droid.csproj`, `PROJECT_NAME.macOS.csproj` if you have an existing project)
    - `PROJECT_NAME.Skia.Gtk.csproj`
    - `PROJECT_NAME.Skia.WPF.csproj`
    - `PROJECT_NAME.Windows.csproj` (or `PROJECT_NAME.UWP.csproj` for existing projects)
4. Add the following resources inside the `App` file:

    ```xml
    <Application ...>
        <Application.Resources>
            <ResourceDictionary>
                <ResourceDictionary.MergedDictionaries>

                    <!-- Load WinUI resources -->
                    <XamlControlsResources xmlns="using:Microsoft.UI.Xaml.Controls" />

                    <!-- Load Uno.Material resources -->
                    <MaterialTheme xmlns="using:Uno.Material" />

                    <!-- Load custom application resources -->
                    <!-- ... -->

                </ResourceDictionary.MergedDictionaries>
            </ResourceDictionary>
        </Application.Resources>
    </Application>
    ```

## Customization

### Color Overrides using _Material Theme Builder_ and DSP format

It is possible to use the [Material Theme Builder](https://m3.material.io/theme-builder#/custom) to generate a custom color palette derived from your own basic colors. The generated palette is provided in the [DSP format](https://m3.material.io/styles/color/the-color-system/color-dsp) and can be used to override the default Uno.Material colors.

The tooling required to generate the _Material Colors Override_ file from a DSP package (zip file) will be present by default when creating a _Uno Extensions_ project with support for Uno.Material.

![Wizard - Theme Selection](assets\material-theme-selection-wizard.png)

Follow this link to get [more Information about the DSP tooling](xref:Uno.Material.DSP).

### Manual Color Overrides

Use this when you want to specify MANUALLY each colors.

1. In the application's **App Code Library** project (`PROJECT_NAME.csproj`), add a new Resource Dictionary named `MaterialColorsOverride.xaml` for XAML or a new class inheriting from `ResourceDictionary` named `ColorPaletteOverride.cs` for C# Markup.
2. Save the new override file within the **App Code Library**, for example, under `Styles/Application`.
3. Replace the content with:
# [**XAML**](#tab/xaml)

<ResourceDictionary xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
                    xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml">
    <ResourceDictionary.ThemeDictionaries>
        <!-- Light Theme -->
        <ResourceDictionary x:Key="Light">
            <!-- Primary -->
            <Color x:Key="PrimaryColor">#6750A4</Color>
            <Color x:Key="PrimaryInverseColor">#D0BCFF</Color>
            <Color x:Key="OnPrimaryColor">#FFFFFF</Color>
            <Color x:Key="PrimaryContainerColor">#EADDFF</Color>
            <Color x:Key="OnPrimaryContainerColor">#21005E</Color>
            <!-- Primary Variant Legacy Colors -->
            <Color x:Key="PrimaryVariantDarkColor">#353FE5</Color>
            <Color x:Key="PrimaryVariantLightColor">#B6A8FB</Color>
            <!-- Secondary -->
            <Color x:Key="SecondaryColor">#625B71</Color>
            <Color x:Key="OnSecondaryColor">#FFFFFF</Color>
            <Color x:Key="SecondaryContainerColor">#E5DFF9</Color>
            <Color x:Key="OnSecondaryContainerColor">#1B192C</Color>
            <!-- Secondary Variant Legacy Colors -->
            <Color x:Key="SecondaryVariantDarkColor">#2BB27E</Color>
            <Color x:Key="SecondaryVariantLightColor">#9CFFDF</Color>
            <!-- Tertiary -->
            <Color x:Key="TertiaryColor">#7D5260</Color>
            <Color x:Key="OnTertiaryColor">#FFFFFF</Color>
            <Color x:Key="TertiaryContainerColor">#FFD8E4</Color>
            <Color x:Key="OnTertiaryContainerColor">#370B1E</Color>
            <!-- Error -->
            <Color x:Key="ErrorColor">#B3261E</Color>
            <Color x:Key="OnErrorColor">#FFFFFF</Color>
            <Color x:Key="ErrorContainerColor">#F9DEDC</Color>
            <Color x:Key="OnErrorContainerColor">#370B1E</Color>
            <!-- Background -->
            <Color x:Key="BackgroundColor">#FFFBFE</Color>
            <Color x:Key="OnBackgroundColor">#1C1B1F</Color>
            <!-- Surface -->
            <Color x:Key="SurfaceColor">#FFFBFE</Color>
            <Color x:Key="OnSurfaceColor">#1C1B1F</Color>
            <Color x:Key="SurfaceVariantColor">#E7E0EC</Color>
            <Color x:Key="OnSurfaceVariantColor">#A5A0AC</Color>
            <Color x:Key="SurfaceInverseColor">#313033</Color>
            <Color x:Key="OnSurfaceInverseColor">#F4EFF4</Color>
            <Color x:Key="SurfaceTintColor">#5946D2</Color>
            <!-- Outline -->
            <Color x:Key="OutlineColor">#79747E</Color>
            <Color x:Key="OutlineVariantColor">#C9C5D0</Color>
        </ResourceDictionary>
        <!-- Dark Theme -->
        <ResourceDictionary x:Key="Dark">
            <!-- Primary -->
            <Color x:Key="PrimaryColor">#D0BCFF</Color>
            <Color x:Key="OnPrimaryColor">#371E73</Color>
            <Color x:Key="PrimaryContainerColor">#4F378B</Color>
            <Color x:Key="OnPrimaryContainerColor">#EADDFF</Color>
            <Color x:Key="PrimaryInverseColor">#6750A4</Color>
            <!-- Primary Variant Legacy Colors -->
            <Color x:Key="PrimaryVariantDarkColor">#353FE5</Color>
            <Color x:Key="PrimaryVariantLightColor">#D4CBFC</Color>
            <!-- Secondary -->
            <Color x:Key="SecondaryColor">#CCC2DC</Color>
            <Color x:Key="OnSecondaryColor">#332D41</Color>
            <Color x:Key="SecondaryContainerColor">#474459</Color>
            <Color x:Key="OnSecondaryContainerColor">#E5DFF9</Color>
            <!-- Secondary Variant Legacy Colors -->
            <Color x:Key="SecondaryVariantDarkColor">#2BB27E</Color>
            <Color x:Key="SecondaryVariantLightColor">#9CFFDF</Color>
            <!-- Tertiary -->
            <Color x:Key="TertiaryColor">#EFB8C8</Color>
            <Color x:Key="OnTertiaryColor">#492532</Color>
            <Color x:Key="TertiaryContainerColor">#633B48</Color>
            <Color x:Key="OnTertiaryContainerColor">#FFD8E4</Color>
            <!-- Error -->
            <Color x:Key="ErrorColor">#F2B8B5</Color>
            <Color x:Key="OnErrorColor">#601410</Color>
            <Color x:Key="ErrorContainerColor">#8C1D18</Color>
            <Color x:Key="OnErrorContainerColor">#F9DEDC</Color>
            <!-- Background -->
            <Color x:Key="BackgroundColor">#1C1B1F</Color>
            <Color x:Key="OnBackgroundColor">#E6E1E5</Color>
            <!-- Surface -->
            <Color x:Key="SurfaceColor">#1C1B1F</Color>
            <Color x:Key="OnSurfaceColor">#E6E1E5</Color>
            <Color x:Key="SurfaceVariantColor">#49454F</Color>
            <Color x:Key="OnSurfaceVariantColor">#CAC4D0</Color>
            <Color x:Key="SurfaceInverseColor">#E6E1E5</Color>
            <Color x:Key="OnSurfaceInverseColor">#313033</Color>
            <Color x:Key="SurfaceTintColor">#C7BFFF</Color>
            <!-- Outline -->
            <Color x:Key="OutlineColor">#938F99</Color>
            <Color x:Key="OutlineVariantColor">#57545D</Color>
        </ResourceDictionary>
    </ResourceDictionary.ThemeDictionaries>
</ResourceDictionary>

# [**C#**](#tab/csharp)

namespace PROJECT_NAME.Styles;

public sealed partial class ColorPaletteOverride : ResourceDictionary
{
    public ColorPaletteOverride()
    {
        this.Build
        (
            r => r
                .Add(Uno.Themes.Markup.Theme.Colors.Primary.Default, light: "#6750A4", dark: "#D0BCFF")
                .Add(Uno.Themes.Markup.Theme.Colors.Primary.Inverse, light: "#D0BCFF", dark: "#6750A4")
                .Add(Uno.Themes.Markup.Theme.Colors.Primary.Container, light: "#EADDFF", dark: "#4F378B")
                .Add(Uno.Themes.Markup.Theme.Colors.OnPrimary.Default, light: "#FFFFFF", dark: "#371E73")
                .Add(Uno.Themes.Markup.Theme.Colors.OnPrimary.Container, light: "#21005E", dark: "#EADDFF")
                .Add(Uno.Themes.Markup.Theme.Colors.Secondary.Default, light: "#625B71", dark: "#CCC2DC")
                .Add(Uno.Themes.Markup.Theme.Colors.Secondary.Container, light: "#E5DFF9", dark: "#474459")
                .Add(Uno.Themes.Markup.Theme.Colors.OnSecondary.Default, light: "#FFFFFF", dark: "#332D41")
                .Add(Uno.Themes.Markup.Theme.Colors.OnSecondary.Container, light: "#1B192C", dark: "#E5DFF9")
                .Add(Uno.Themes.Markup.Theme.Colors.Tertiary.Default, light: "#7D5260", dark: "#EFB8C8")
                .Add(Uno.Themes.Markup.Theme.Colors.Tertiary.Container, light: "#FFD8E4", dark: "#633B48")
                .Add(Uno.Themes.Markup.Theme.Colors.OnTertiary.Default, light: "#FFFFFF", dark: "#492532")
                .Add(Uno.Themes.Markup.Theme.Colors.OnTertiary.Container, light: "#370B1E", dark: "#FFD8E4")
                .Add(Uno.Themes.Markup.Theme.Colors.Error.Default, light: "#B3261E", dark: "#F2B8B5")
                .Add(Uno.Themes.Markup.Theme.Colors.Error.Container, light: "#F9DEDC", dark: "#8C1D18")
                .Add(Uno.Themes.Markup.Theme.Colors.OnError.Default, light: "#FFFFFF", dark: "#601410")
                .Add(Uno.Themes.Markup.Theme.Colors.OnError.Container, light: "#370B1E", dark: "#F9DEDC")
                .Add(Uno.Themes.Markup.Theme.Colors.Background.Default, light: "#FFFBFE", dark: "#1C1B1F")
                .Add(Uno.Themes.Markup.Theme.Colors.OnBackground.Default, light: "#1C1B1F", dark: "#E6E1E5")
                .Add(Uno.Themes.Markup.Theme.Colors.Surface.Default, light: "#FFFBFE", dark: "#1C1B1F")
                .Add(Uno.Themes.Markup.Theme.Colors.Surface.Variant, light: "#E7E0EC", dark: "#49454F")
                .Add(Uno.Themes.Markup.Theme.Colors.Surface.Inverse, light: "#313033", dark: "#E6E1E5")
                .Add(Uno.Themes.Markup.Theme.Colors.Surface.Tint, light: "#5946D2", dark: "#C7BFFF")
                .Add(Uno.Themes.Markup.Theme.Colors.OnSurface.Default, light: "#1C1B1F", dark: "#E6E1E5")
                .Add(Uno.Themes.Markup.Theme.Colors.OnSurface.Variant, light: "#A5A0AC", dark: "#CAC4D0")
                .Add(Uno.Themes.Markup.Theme.Colors.OnSurface.Inverse, light: "#F4EFF4", dark: "#313033")
                .Add(Uno.Themes.Markup.Theme.Colors.Outline.Default, light: "#79747E", dark: "#938F99")
                .Add(Uno.Themes.Markup.Theme.Colors.Outline.Variant, light: "#C9C5D0", dark: "#57545D")		
        );
    }
}

***
4. In `AppResources` file, update `MaterialTheme` with the override from the previous steps:
# [**XAML**](#tab/xaml)

<MaterialTheme xmlns="using:Uno.Material"
                ColorOverrideSource="ms-appx:///PROJECT_NAME/Styles/Application/MaterialColorsOverride.xaml" />

# [**C#**](#tab/csharp)

this.Build(r => r.Merged(
        new MaterialTheme()
            .ColorOverrideDictionary(new Styles.ColorPaletteOverride())));

***
### Change Default Font

By default, Uno.Material comes pre-packaged with the [Roboto](https://fonts.google.com/specimen/Roboto) font families and automatically includes them in your application. Upon installation of the Uno.Material package, you will have the following resources available: `MaterialLightFontFamily`, `MaterialRegularFontFamily`, and `MaterialMediumFontFamily`.

If you would like Uno.Material to use a different font, you can override the default font families following these steps:

1. Add the custom font following [this guide](https://platform.uno/docs/articles/features/custom-fonts.html)
2. In the application's **App Code Library** project (`PROJECT_NAME.csproj`), add a new Resource Dictionary named `MaterialFontsOverride.xaml` for XAML or a new class inheriting from `ResourceDictionary` named `MaterialFontsOverride.cs` for C# Markup
3. Save the new override file within the **App Code Library**, for example, under `Styles/Application`.
4. Assuming the font file has been placed in the **App Code Library** within, for example, a directory such as `Assets/Fonts/MyCustomFont.ttf`, your override file would look like the following:
# [**XAML**](#tab/xaml)

<ResourceDictionary xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
                    xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml">
    <FontFamily x:Key="MaterialLightFontFamily">ms-appx:///PROJECT_NAME/Assets/Fonts/MyCustomFont-Light.ttf#MyCustomFont</FontFamily>
    <FontFamily x:Key="MaterialMediumFontFamily">ms-appx:///PROJECT_NAME/Assets/Fonts/MyCustomFont-Medium.ttf#MyCustomFont</FontFamily>
    <FontFamily x:Key="MaterialRegularFontFamily">ms-appx:///PROJECT_NAME/Assets/Fonts/MyCustomFont-Regular.ttfMyCustomFont</FontFamily>
</ResourceDictionary>

# [**C#**](#tab/csharp)

namespace PROJECT_NAME.Styles;

public sealed class MaterialFontsOverride : ResourceDictionary
{
    public MaterialFontsOverride()
    {
        this.Build(r => r
            .Add<FontFamily>("MaterialLightFontFamily", "ms-appx:///PROJECT_NAME/Assets/Fonts/MyCustomFont-Light.ttf#MyCustomFont")
            .Add<FontFamily>("MaterialMediumFontFamily", "ms-appx:///PROJECT_NAME/Assets/Fonts/MyCustomFont-Medium.ttf#MyCustomFont")
            .Add<FontFamily>("MaterialRegularFontFamily", "ms-appx:///PROJECT_NAME/Assets/Fonts/MyCustomFont-Regular.ttfMyCustomFont"));
    }
}

***

5. In `AppResources` file, update `MaterialTheme` with the override from the previous steps:
# [**XAML**](#tab/xaml)

<MaterialTheme xmlns="using:Uno.Material"
                FontOverrideSource="ms-appx:///PROJECT_NAME/Styles/Application/MaterialFontsOverride.xaml" />

# [**C#**](#tab/csharp)

this.Build(r => r.Merged(
    new MaterialTheme()
        .FontOverrideDictionary(new Styles.MaterialFontsOverride())));

***

## Additional Resources

- [Uno Platform Material Sample App](https://github.com/unoplatform/Uno.Samples/tree/master/UI/UnoMaterialSample)
- [Uno Platform Material Figma File](https://www.figma.com/community/file/1110792522046146058)
- [Official Material Design 3 Guidelines](https://m3.material.io/components)
- [Official Material Design 3 Theme Builder](https://m3.material.io/theme-builder)
