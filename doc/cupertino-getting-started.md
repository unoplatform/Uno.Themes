---
uid: Uno.Themes.Cupertino.GetStarted
---

# Uno Cupertino

<p align="center">
  <img src="assets/cupertino-design-system.png" alt="Cupertino design system" />
</p>

> [!IMPORTANT]
> UnoFeatures: **Cupertino** — add `<UnoFeatures>Cupertino</UnoFeatures>` to your app's `.csproj` to enable Uno Cupertino.

Uno Cupertino is enabled through the `Cupertino` UnoFeatures and lets you apply [Cupertino - Human Interface Guideline styling](https://developer.apple.com/design/human-interface-guidelines) to your application with a few lines of code.

## Getting Started

> [!NOTE]
> Make sure to setup your environment first by [following our instructions](xref:Uno.GetStarted.vs2022).

### Creating a new project with Uno Cupertino

1. Install the [`dotnet new` CLI templates](xref:Uno.GetStarted.dotnet-new) with:

    ```bash
    dotnet new install Uno.Templates
    ```

2. Create a new application with:

    ```bash
    dotnet new unoapp -o UnoCupertinoApp -theme cupertino
    ```

### Installing Uno Cupertino in an existing project

Uno Platform 7.0 builds applications with the `Uno.Sdk` single project only. If your app still uses the older multi-head project structure, it fails to build with `UNOB0029`: migrate it first by following [Migrating Projects to Single Project](xref:Uno.Development.MigratingToSingleProject).

1. Edit your project file (`PROJECT_NAME.csproj`) and add `Cupertino` to the list of `UnoFeatures`:

    ```xml
    <UnoFeatures>Cupertino</UnoFeatures>
    ```

2. Initialize the Cupertino resources in the `App.xaml`:

    ```xml
    <Application.Resources>
        <ResourceDictionary>
            <ResourceDictionary.MergedDictionaries>

                <!-- Code ommitted of brevity -->

                <uc:CupertinoColors xmlns="using:Uno.Cupertino" />
                <uc:CupertinoFonts xmlns="using:Uno.Cupertino" />
                <uc:CupertinoResources xmlns="using:Uno.Cupertino" />
            </ResourceDictionary.MergedDictionaries>
        </ResourceDictionary>
    </Application.Resources>
    ```

## Customization

The following guides require the creation of new `ResourceDictionary` files in your application project. For more information on how to define styles and resources in a separate `ResourceDictionary`, refer to the [resource management documentation](xref:Guide.HowTo.Create-Control-Library#moving-the-control-style-in-a-separate-resource-dictionary).

> [!NOTE]
> [Seed color palette generation](xref:Uno.Themes.SeedColors) is available for the Material and Simple themes only — Cupertino colors are customized through the manual overrides below.

### Customize Color Palette

1. Add a new Resource Dictionary named `CupertinoColorsOverride.xaml` to the application project, for example, under `Styles/Application`.
2. Replace the content with:

    ```xml
    <ResourceDictionary xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
                        xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml">
        <ResourceDictionary.ThemeDictionaries>

            <!-- Light Theme -->
            <ResourceDictionary x:Key="Light">
                <!-- Override CupertinoBlueBrush -->
                <Color x:Key="CupertinoBlueBrush">#6750A4</Color>

                <!-- Add more overrides here -->
                <!-- ... -->
            </ResourceDictionary>

            <!-- Dark Theme -->
            <ResourceDictionary x:Key="Dark">
                <!-- Override CupertinoBlueBrush -->
                <Color x:Key="CupertinoBlueBrush">#D0BCFF</Color>

                <!-- Add more overrides here -->
                <!-- ... -->
            </ResourceDictionary>

        </ResourceDictionary.ThemeDictionaries>
    </ResourceDictionary>
    ```

3. In `App.xaml`, update `<CupertinoColors />` with the override from the previous steps:

    ```xml
    <CupertinoColors xmlns="using:Uno.Cupertino"
                     OverrideSource="ms-appx:///Styles/Application/CupertinoColorsOverride.xaml" />
    ```

### Change Default Font

By default, Uno Cupertino comes pre-packaged with the [SF Pro](https://developer.apple.com/fonts/) `FontFamily` and automatically includes them in your application. Upon installation of the Uno Cupertino package, you will have a `CupertinoFontFamily` resource available. It is an alias of the shared `DefaultFontFamily` root token (see [Design Tokens](design-tokens.md#typography)); Cupertino has no semantic type scale, so `CupertinoFontFamily` stays the key to override.

If you would like Uno Cupertino to use a different font, you can override the default `FontFamily` by following these steps:

1. Add the custom font following [Custom Fonts documentation](https://platform.uno/docs/articles/features/custom-fonts.html).
2. Add a new Resource Dictionary named `CupertinoFontsOverride.xaml` to the application project, for example, under `Styles/Application`.
3. Assuming the font file has been placed in a directory such as `Assets/Fonts/MyCustomFont.ttf`, your override file would look like the following:

    ```xml
    <ResourceDictionary xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
                        xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml">

        <FontFamily x:Key="CupertinoFontFamily">ms-appx:///Assets/Fonts/MyCustomFont.ttf</FontFamily>

    </ResourceDictionary>
    ```

4. In the `App.xaml`, update `<CupertinoFonts />` with the override from the previous steps:

    ```xml
    <CupertinoFonts xmlns="using:Uno.Cupertino"
                    OverrideSource="ms-appx:///Styles/Application/CupertinoFontsOverride.xaml" />
    ```
