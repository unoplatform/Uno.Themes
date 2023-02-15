<p align="center">
  <img src="assets/material-design-system.png">
</p>

# Uno.Material

The Uno.Material library is available as NuGet packages that can be added to any new or existing Uno solution.
Uno Material lets you apply [Material Design 3](https://m3.material.io/) styling to your application with just a few lines of code.

> [!WARNING]
> If you are updating Uno.Material to v2 from an older 1.x version of the package, additional steps will be required. Refer to the [Uno Material Migration Guide](material-migration.md).

## Getting Started

Initialization of the Uno.Material  resources is handled by the specialized `MaterialTheme` ResourceDictionary.

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
> As of [Uno Platform 4.7](https://platform.uno/blog/uno-platform-4-7-new-project-template-performance-improvements-and-more/), the solution template of an Uno app has changed. There is no longer a Shared project (.shproj), it has been replaced with a regular cross-platform library containing all user code files, referred to as the **App Code Library** project. This also implies that package references can be included in a single location without the previous need to include those in all project heads.

1. Create a new Uno project using the `Uno Platform App` template.
2. In the Solution Explorer panel, right-click on your app's **App Code Library** project (`PROJECT_NAME.csproj`) and select `Manage NuGet Packages...`
3. Install the [`Uno.Material.WinUI`](https://www.nuget.org/packages/Uno.Material.WinUI)
4. Add the following Material resources to `AppResources.xaml`:

    ```xml
    <ResourceDictionary>
        <ResourceDictionary.MergedDictionaries>

            <!-- Load Uno.Material resources -->
            <MaterialTheme xmlns="using:Uno.Material" />

        </ResourceDictionary.MergedDictionaries>
    </ResourceDictionary>
    ```

### Installing Uno.Material on previous versions of Uno Platform

If your application is based on the older solution template that includes a shared project (.shproj), follow these steps:

1. Open an existing Uno project
2. In the Solution Explorer panel, right-click on your solution name and select `Manage NuGet Packages for Solution ...`. Choose either:
     - The [`Uno.Material`](https://www.nuget.org/packages/Uno.Material/) package when targetting Xamarin/UWP
     - The [`Uno.Material.WinUI`](https://www.nuget.org/packages/Uno.Material.WinUI) package when targetting net6.0+/WinUI

3. Select the following projects for installation:
    - `PROJECT_NAME.Wasm.csproj`
    - `PROJECT_NAME.Mobile.csproj` (or `PROJECT_NAME.iOS.csproj`, `PROJECT_NAME.Droid.csproj`, `PROJECT_NAME.macOS.csproj` if you have an existing project)
    - `PROJECT_NAME.Skia.Gtk.csproj`
    - `PROJECT_NAME.Skia.WPF.csproj`
    - `PROJECT_NAME.Windows.csproj` (or `PROJECT_NAME.UWP.csproj` for existing projects)
4. Add the following resources inside `App.xaml`:

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

### Customize Color Palette

1. In the application's **App Code Library** project (`PROJECT_NAME.csproj`), add a new Resource Dictionary named `MaterialColorsOverride.xaml`
2. Save the new override file within the **App Code Library**, for example, under `Style/Application`.
3. Replace the content with:

    ```xml
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

                <!-- Outline -->
                <Color x:Key="OutlineColor">#79747E</Color>
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

                <!-- Outline -->
                <Color x:Key="OutlineColor">#938F99</Color>
            </ResourceDictionary>

        </ResourceDictionary.ThemeDictionaries>
    </ResourceDictionary>
    ```

4. In `AppResources.xaml`, update `<MaterialTheme />` with the override from the previous steps:

    ```xml
    <MaterialTheme xmlns="using:Uno.Material"
                   ColorOverrideSource="ms-appx:///PROJECT_NAME/Style/Application/MaterialColorsOverride.xaml" />
    ```

### Change Default Font

By default, Uno.Material comes pre-packaged with the [Roboto](https://fonts.google.com/specimen/Roboto) font families and automatically includes them in your application. Upon installation of the Uno.Material package, you will have the following resources available: `MaterialLightFontFamily`, `MaterialRegularFontFamily`, and `MaterialMediumFontFamily`.

If you would like Uno.Material to use a different font, you can override the default font families following these steps:

1. Add the custom font following [this guide](https://platform.uno/docs/articles/features/custom-fonts.html)
2. In the application's **App Code Library** project (`PROJECT_NAME.csproj`), add a new Resource Dictionary named `MaterialFontsOverride.xaml`
3. Save the new override file within the **App Code Library**, for example, under `Style/Application`.
4. Assuming the font file has been placed in the **App Code Library** within, for example, a directory such as `Assets/Fonts/MyCustomFont.ttf`, your override file would look like the following:

    ```xml
    <ResourceDictionary xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
                        xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml">

        <FontFamily x:Key="MaterialLightFontFamily">ms-appx:///PROJECT_NAME/Assets/Fonts/MyCustomFont-Light.ttf#MyCustomFont</FontFamily>
        <FontFamily x:Key="MaterialMediumFontFamily">ms-appx:///PROJECT_NAME/Assets/Fonts/MyCustomFont-Medium.ttf#MyCustomFont</FontFamily>
        <FontFamily x:Key="MaterialRegularFontFamily">ms-appx:///PROJECT_NAME/Assets/Fonts/MyCustomFont-Regular.ttfMyCustomFont</FontFamily>

    </ResourceDictionary>
    ```

5. In `AppResources.xaml`, update `<MaterialTheme />` with the override from the previous steps:

    ```xml
    <MaterialTheme xmlns="using:Uno.Material"
                   FontOverrideSource="ms-appx:///PROJECT_NAME/Style/Application/MaterialFontsOverride.xaml" />
    ```
