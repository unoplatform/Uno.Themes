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

Depending on the type of project template that the Uno Platform application was created with, follow the instructions below to install Uno Cupertino.

#### [**Single Project Template**](#tab/singleproj)

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

#### [**Multi-Head Project Template (Legacy)**](#tab/multihead)

> [!NOTE]
> Use this only when working on older multi-head templates that do not support `UnoFeatures`. Modern templates should use the Single Project instructions above.

1. In the Solution Explorer panel, right-click on your app's **App Code Library** project (`PROJECT_NAME.csproj`) and select `Manage NuGet Packages...`
1. Install the [`Uno.Cupertino.WinUI`](https://www.nuget.org/packages/Uno.Cupertino.WinUI)
1. Add the following Cupertino resources to `AppResources.xaml`:

    ```xml
    <ResourceDictionary>
        <ResourceDictionary.MergedDictionaries>

        <CupertinoColors xmlns="using:Uno.Cupertino" />
        <CupertinoFonts xmlns="using:Uno.Cupertino" />
        <CupertinoResources xmlns="using:Uno.Cupertino" />

        </ResourceDictionary.MergedDictionaries>
    </ResourceDictionary>
    ```

#### [**Shared Project (.shproj) Template (Legacy)**](#tab/shproj)

> [!NOTE]
> Use this only when working on older `.shproj`-based solutions that do not support `UnoFeatures`. Modern templates should use the Single Project instructions above.

1. In the Solution Explorer panel, right-click on your solution name and select `Manage NuGet Packages for Solution ...`. Choose either:
    - The [`Uno.Cupertino`](https://www.nuget.org/packages/Uno.Cupertino/) package when targetting Xamarin/UWP
    - The [`Uno.Cupertino.WinUI`](https://www.nuget.org/packages/Uno.Cupertino.WinUI) package when targetting net6.0+/WinUI

2. Select the following projects for installation:
    - `PROJECT_NAME.Wasm.csproj`
    - `PROJECT_NAME.Mobile.csproj` (or `PROJECT_NAME.iOS.csproj`, `PROJECT_NAME.Droid.csproj`, and `PROJECT_NAME.macOS.csproj` if you have an existing project)
    - `PROJECT_NAME.Skia.Gtk.csproj`
    - `PROJECT_NAME.Skia.WPF.csproj`
    - `PROJECT_NAME.Windows.csproj` (or `PROJECT_NAME.UWP.csproj` for existing projects)
3. Add the following resources inside `App.xaml`:

    ```xml
    <Application>
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

---

## Using `CupertinoTheme`

`CupertinoTheme` puts Cupertino on the same semantic system as `MaterialTheme` and `SimpleTheme`: a single dictionary that brings the styles, maps Apple's system palette onto the [shared color roles](xref:Uno.Themes.SemanticStyles) (`PrimaryBrush`, `SurfaceBrush`, `OutlineBrush`, …), maps the iOS text styles onto the shared type scale (`BodyLargeFontSize`, `TitleMediumFontWeight`, …), and generates the [design tokens](xref:Uno.Themes.DesignTokens) (`Space*`, `Radius*`, `ControlHeight*`).

```xml
<Application.Resources>
    <ResourceDictionary>
        <ResourceDictionary.MergedDictionaries>
            <XamlControlsResources xmlns="using:Microsoft.UI.Xaml.Controls" />

            <CupertinoTheme xmlns="using:Uno.Cupertino" />
        </ResourceDictionary.MergedDictionaries>
    </ResourceDictionary>
</Application.Resources>
```

It accepts the same properties as the other themes — `ColorOverrideSource`, `FontOverrideSource`, `DefaultFontFamily`, `DefaultSpacing`, `DefaultDensity`, `DefaultCornerRadius` and `Colors`:

```xml
<CupertinoTheme xmlns="using:Uno.Cupertino"
                xmlns:ut="using:Uno.Themes"
                DefaultCornerRadius="6">
    <CupertinoTheme.Colors>
        <ut:ThemeColors PrimarySeed="#2E7D32" />
    </CupertinoTheme.Colors>
</CupertinoTheme>
```

The semantic style keys Cupertino has a style for are aliased (`FilledButtonStyle`, `TextButtonStyle`, `OutlinedTextBoxStyle`, `OutlinedPasswordBoxStyle`, `ComboBoxStyle`, `ComboBoxItemStyle`, `CheckBoxStyle`, `RadioButtonStyle`, `ToggleSwitchStyle`, `SliderStyle`, `HyperlinkButtonStyle`, `CalendarViewStyle`, `CalendarDatePickerStyle`, `DatePickerStyle`, `ProgressBarStyle`), so markup written against those keys moves between design systems unchanged.

> [!NOTE]
> With a [seed color](xref:Uno.Themes.SeedColors), the container roles follow the Material 3 recipe rather than Apple's tinted fills: the generated scheme is coherent, but it is no longer the stock Apple palette. The built-in `Cupertino*Style` control styles still paint from the `Cupertino*` color keys described below, so a seed or a shared-role override reaches the semantic brushes and anything styled with them, not yet those control styles.

## Customization

The following guides require the creation of new `ResourceDictionary` files in your application project. For more information on how to define styles and resources in a separate `ResourceDictionary`, refer to the [resource management documentation](xref:Guide.HowTo.Create-Control-Library#moving-the-control-style-in-a-separate-resource-dictionary).

> [!NOTE]
> The overrides below apply to the `CupertinoColors` / `CupertinoFonts` / `CupertinoResources` setup. With `CupertinoTheme`, use `ColorOverrideSource` / `FontOverrideSource` instead, as for the [other themes](xref:Uno.Themes.Overview).

### Customize Color Palette

1. Add a new Resource Dictionary named `CupertinoColorsOverride.xaml` to the application project, for example, under `Styles/Application`.
2. Replace the content with:

    ```xml
    <ResourceDictionary xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
                        xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml">
        <ResourceDictionary.ThemeDictionaries>

            <!-- Light Theme -->
            <ResourceDictionary x:Key="Light">
                <!-- Override CupertinoBlueColor -->
                <Color x:Key="CupertinoBlueColor">#6750A4</Color>

                <!-- Add more overrides here -->
                <!-- ... -->
            </ResourceDictionary>

            <!-- Dark Theme -->
            <ResourceDictionary x:Key="Dark">
                <!-- Override CupertinoBlueColor -->
                <Color x:Key="CupertinoBlueColor">#D0BCFF</Color>

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
