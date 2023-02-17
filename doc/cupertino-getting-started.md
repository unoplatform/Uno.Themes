<p align="center">
  <img src="assets/cupertino-design-system.png">
</p>

# Uno.Cupertino

Uno Cupertino is an add-on package that lets you apply [Cupertino - Human Interface Guideline styling](https://developer.apple.com/design/human-interface-guidelines) to your application with a few lines of code.

## Getting Started

> [!NOTE]
> As of [Uno Platform 4.7](https://platform.uno/blog/uno-platform-4-7-new-project-template-performance-improvements-and-more/), the solution template of an Uno app has changed. There is no longer a Shared project (.shproj), it has been replaced with a regular cross-platform library containing all user code files, referred to as the **App Code Library** project. This also implies that package references can be included in a single location without the previous need to include those in all project heads.

1. Create a new Uno project using the `Uno Platform App` template.
2. In the Solution Explorer panel, right-click on your app's **App Code Library** project (`PROJECT_NAME.csproj`) and select `Manage NuGet Packages...`
3. Install the [`Uno.Cupertino.WinUI`](https://www.nuget.org/packages/Uno.Cupertino.WinUI)
4. Add the following Cupertino resources `AppResources.xaml`:

    ```xml
    <ResourceDictionary>
        <ResourceDictionary.MergedDictionaries>

        <!-- Load Uno.Cupertino resources -->
        <CupertinoColors xmlns="using:Uno.Cupertino" />
        <CupertinoFonts xmlns="using:Uno.Cupertino" />
        <CupertinoResources xmlns="using:Uno.Cupertino" />

        </ResourceDictionary.MergedDictionaries>
    </ResourceDictionary>
    ```

### Installing Uno.Cupertino on previous versions of Uno Platform

If your application is based on the older solution template that includes a shared project (.shproj), follow these steps:

1. Open an existing Uno project
2. In the Solution Explorer panel, right-click on your solution name and select `Manage NuGet Packages for Solution ...`. Choose either:
    - The [`Uno.Cupertino`](https://www.nuget.org/packages/Uno.Cupertino/) package when targetting Xamarin/UWP
    - The [`Uno.Cupertino.WinUI`](https://www.nuget.org/packages/Uno.Cupertino.WinUI) package when targetting net6.0+/WinUI

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

                    <!-- Load Uno.Cupertino resources -->
                    <CupertinoColors xmlns="using:Uno.Cupertino" />
                    <CupertinoFonts xmlns="using:Uno.Cupertino" />
                    <CupertinoResources xmlns="using:Uno.Cupertino" />

                    <!-- Load custom application resources -->
                    <!-- ... -->

                </ResourceDictionary.MergedDictionaries>
            </ResourceDictionary>
        </Application.Resources>
    </Application>
    ```

## Customization

### Customize Color Palette

1. In the application's **App Code Library** project (`PROJECT_NAME.csproj`), add a new Resource Dictionary named `CupertinoColorsOverride.xaml`
2. Save the new override file within the **App Code Library**, for example, under `Style/Application`.
3. Replace the content with:

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

4. In `AppResources.xaml`, update `<CupertinoColors />` with the override from the previous steps:

    ```xml
    <CupertinoColors xmlns="using:Uno.Cupertino"
                     OverrideSource="ms-appx:///PROJECT_NAME/Style/Application/CupertinoColorsOverride.xaml" />
    ```

### Change Default Font

By default, Uno.Cupertino comes pre-packaged with the [SF Pro](https://developer.apple.com/fonts/) `FontFamily` and automatically includes them in your application. Upon installation of the Uno.Cupertino package, you will have a `CupertinoFontFamily` resource available.

If you would like Uno.Cupertino to use a different font, you can override the default `FontFamily` following these steps:

1. Add the custom font following [this guide](https://platform.uno/docs/articles/features/custom-fonts.html)
2. In the application's **App Code Library** project (`PROJECT_NAME.csproj`), add a new Resource Dictionary named `CupertinoFontsOverride.xaml`
3. Save the new override file within the **App Code Library**, for example, under `Style/Application`.
4. Assuming the font file has been placed in the **App Code Library** within, for example, a directory such as `Assets/Fonts/MyCustomFont.ttf`, your override file would look like the following:

    ```xml
    <ResourceDictionary xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
                        xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml">

        <FontFamily x:Key="CupertinoFontFamily">ms-appx:///PROJECT_NAME/Assets/Fonts/MyCustomFont.ttf</FontFamily>

    </ResourceDictionary>
    ```

5. In `AppResources.xaml`, update `<CupertinoFonts />` with the override from the previous steps:

    ```xml

    <CupertinoFonts xmlns="using:Uno.Cupertino"
                    OverrideSource="ms-appx:///PROJECT_NAME/Style/Application/CupertinoFontsOverride.xaml" />
    ```
