# Getting Started

## Uno.Material
The Uno.Material library is available as NuGet packages that can be added to any new or existing Uno solution.
Uno Material lets you apply [Material Design 3](https://m3.material.io/) styling to your application with a few lines of code. 

It features:
- Color system for both Light and Dark themes
- Styles for existing WinUI controls like Buttons, TextBox, etc.

### Getting Started
1. Open an existing Uno project, or create a new Uno project using the `Uno Platform App` template.
2. In the Solution Explorer panel, right-click on your solution name and select `Manage NuGet Packages for Solution ...`. Choose [`Uno.Material`](https://www.nuget.org/packages/Uno.Material/) package for Uno UWP solution or choose the [`Uno.Material.WinUI`](https://www.nuget.org/packages/Uno.Material.WinUI) package for Uno WinUI solution. Then, select the following projects to include it:
	- `PROJECT_NAME.Wasm.csproj`
	- `PROJECT_NAME.Mobile.csproj` (or `PROJECT_NAME.iOS.csproj`, `PROJECT_NAME.Droid.csproj`, `PROJECT_NAME.macOS.csproj` if you have an existing project)
	- `PROJECT_NAME.Skia.Gtk.csproj`
	- `PROJECT_NAME.Skia.WPF.csproj`
	- `PROJECT_NAME.Windows.csproj` (or `PROJECT_NAME.UWP.csproj` for existing projects)
3. Add the following resources inside `App.xaml`:
	```xml
	<Application ...>
		<Application.Resources>
			<ResourceDictionary>
				<ResourceDictionary.MergedDictionaries>

					<!-- Load WinUI resources -->
					<XamlControlsResources xmlns="using:Microsoft.UI.Xaml.Controls" />

					<!-- Load Uno.Material resources -->
					<MaterialColors xmlns="using:Uno.Material" />
					<MaterialFonts xmlns="using:Uno.Material" />
					<MaterialResources xmlns="using:Uno.Material" />

					<!-- Load custom application resources -->
					<!-- ... -->

				</ResourceDictionary.MergedDictionaries>
			</ResourceDictionary>
		</Application.Resources>
	</Application>
	```

### Customization
#### Customize Color Palette
2. In the `.Shared` project, add a new Resource Dictionary named `MaterialColorsOverride.xaml` under `Style\Application`.
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
				<Color x:Key="SecondaryContainerColor">#E8DEF8</Color>
				<Color x:Key="OnSecondaryContainerColor">#1E192B</Color>

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
				<Color x:Key="OnSurfaceVariantColor">#49454E</Color>
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
				<Color x:Key="SecondaryContainerColor">#4A4458</Color>
				<Color x:Key="OnSecondaryContainerColor">#E8DEF8</Color>

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
4. In `App.xaml`, update `<MaterialColors />` with the override from the previous steps:
	```xml
	<MaterialColors xmlns="using:Uno.Material"
					OverrideSource="ms-appx:///Style/Application/MaterialColorsOverride.xaml" />
	```

#### Change Default Font
The default font for material design is [`Roboto`](https://fonts.google.com/specimen/Roboto). You can change it following the steps below:
1. Add the custom font following [this guide](https://platform.uno/docs/articles/features/custom-fonts.html)
2. Add a new Resource Dictionary named `MaterialFontsOverride.xaml` to the `.Shared` project, under `Style\Application`.
3. Replace the content with:
	```xml
	<ResourceDictionary xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
						xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml">

		<FontFamily x:Key="MaterialLightFontFamily">ms-appx:///Assets/Fonts/Material/Roboto-Light.ttf#Roboto</FontFamily>
		<FontFamily x:Key="MaterialMediumFontFamily">ms-appx:///Assets/Fonts/Material/Roboto-Medium.ttf#Roboto</FontFamily>
		<FontFamily x:Key="MaterialRegularFontFamily">ms-appx:///Assets/Fonts/Material/Roboto-Regular.ttf#Roboto</FontFamily>

	</ResourceDictionary>
	```
4. In `App.xaml`, update `<MaterialFonts />` with the override from the previous steps:
	```xml
	<MaterialFonts xmlns="using:Uno.Material"
				   OverrideSource="ms-appx:///Style/Application/MaterialFontsOverride.xaml" />
	```

## Uno.Cupertino
Uno Cupertino is an add-on package that lets you apply [Cupertino - Human Interface Guideline styling](https://developer.apple.com/design/human-interface-guidelines) to your application with a few lines of code. 

It features:
- Color system for both Light and Dark themes
- Styles for existing WinUI controls like Buttons, TextBox, etc.

### Getting Started
1. Open an existing Uno project, or create a new Uno project using the `Uno Platform App` template.
2. In the Solution Explorer panel, right-click on your solution name and select `Manage NuGet Packages for Solution ...`. Choose [`Uno.Cupertino`](https://www.nuget.org/packages/Uno.Cupertino/) package for Uno UWP solution or choose the [`Uno.Cupertino.WinUI`](https://www.nuget.org/packages/Uno.Cupertino.WinUI) package for Uno WinUI solution. Then, select the following projects to include it:
	- `PROJECT_NAME.Wasm.csproj`
	- `PROJECT_NAME.Mobile.csproj` (or `PROJECT_NAME.iOS.csproj`, `PROJECT_NAME.Droid.csproj`, `PROJECT_NAME.macOS.csproj` if you have an existing project)
	- `PROJECT_NAME.Skia.Gtk.csproj`
	- `PROJECT_NAME.Skia.WPF.csproj`
	- `PROJECT_NAME.Windows.csproj` (or `PROJECT_NAME.UWP.csproj` for existing projects)
1. Add the following resources inside `App.xaml`:
    ```xml
    <Application.Resources>
        <ResourceDictionary>
            <ResourceDictionary.MergedDictionaries>
                <!-- Load WinUI resources -->
                <XamlControlsResources xmlns="using:Microsoft.UI.Xaml.Controls" />

                <CupertinoColors xmlns="using:Uno.Cupertino"  />
				<CupertinoResources xmlns="using:Uno.Cupertino" />

                <!-- Load custom application resources -->
				<!-- ... -->

            </ResourceDictionary.MergedDictionaries>
        </ResourceDictionary>
    </Application.Resources>
    ```

For complete instructions on using Uno Cupertino in your projects, check out this walkthrough: [How to use Uno.Cupertino](https://platform.uno/docs/articles/guides/uno-cupertino-walkthrough.html).

### Customization
#### Customize Color Palette
The colors used in the Cupertino styles are part of the color palette system, which can be customized to suit the theme of your application. Since this is decoupled from the styles, the application theme can be changed, without having to make a copy of every style and edit each of them.