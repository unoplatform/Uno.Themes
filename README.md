# Uno Material

<!-- TODO : Insert logo/branding -->

This library is designed to help you use the [material design system](https://material.io/design):
- Color system for both Light and Dark theme
- Styles for existing WinUI controls like Buttons, TextBox, etc.
- Custom Controls for Material Components not offered out of the box by WinUI, such as Cards and BottomNavigationBar.

Platform support: 
- WinUI / UWP
- iOS
- Android
- WebAssembly

<!-- TODO : Insert build status, nuget.org badge, etc -->
[![License](https://img.shields.io/badge/License-Apache%202.0-blue.svg)](LICENSE)

## Getting Started

1. Install the nuget package Uno.Material. You can find the nuget on [this feed instead of nuget.org](https://dev.azure.com/uno-platform/Uno%20Platform/_packaging?_a=feed&feed=unoplatformdev)
2. Unless you want our default color palette (inspired by our Uno logo), you'll want to override the following color resources in you application. We suggest creating a Color.xaml `ResourceDictionary`.
For more information on the color system, consult this [page](https://material.io/design/color/the-color-system.html#color-theme-creation) for all the official documentation and tools to help you create your own palette. 
Here is what Colors.xaml would contain if you want both light and dark theme. (see 7. for android native ToggleSwitch colors)
```xaml
<ResourceDictionary
    xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation" 
    xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
    xmlns:local="using:Uno.Material.Samples.Shared.Content">

	<!-- 
	
	For more information on the color system, consult this page
	https://material.io/design/color/the-color-system.html#color-theme-creation 
	
	-->
	<ResourceDictionary.ThemeDictionaries>
		<!-- Light Theme -->
		<ResourceDictionary x:Key="Light">
			<Color x:Key="PrimaryColor">#5B4CF5</Color>
			<Color x:Key="PrimaryVariantDarkColor">#353FE5</Color>
			<Color x:Key="PrimaryVariantLightColor">#B6A8FB</Color>
			<Color x:Key="SecondaryColor">#67E5AD</Color>
			<Color x:Key="SecondaryVariantDarkColor">#2BB27E</Color>
			<Color x:Key="SecondaryVariantLightColor">#9CFFDF</Color>
			<Color x:Key="BackgroundColor">#FFFFFF</Color>
			<Color x:Key="SurfaceColor">#FFFFFF</Color>
			<Color x:Key="ErrorColor">#F85977</Color>
			<Color x:Key="OnPrimaryColor">#FFFFFF</Color>
			<Color x:Key="OnSecondaryColor">#000000</Color>
			<Color x:Key="OnBackgroundColor">#000000</Color>
			<Color x:Key="OnSurfaceColor">#000000</Color>
			<Color x:Key="OnErrorColor">#000000</Color>
			<Color x:Key="OverlayColor">#51000000</Color>
		</ResourceDictionary>

		<!-- Dark Theme -->
		<ResourceDictionary x:Key="Dark">
			<Color x:Key="PrimaryColor">#B6A8FB</Color>
			<Color x:Key="PrimaryVariantDarkColor">#353FE5</Color>
			<Color x:Key="PrimaryVariantLightColor">#D4CBFC</Color>
			<Color x:Key="SecondaryColor">#67E5AD</Color>
			<Color x:Key="SecondaryVariantDarkColor">#2BB27E</Color>
			<Color x:Key="SecondaryVariantLightColor">#9CFFDF</Color>
			<Color x:Key="BackgroundColor">#121212</Color>
			<Color x:Key="SurfaceColor">#121212</Color>
			<Color x:Key="ErrorColor">#CF6679</Color>
			<Color x:Key="OnPrimaryColor">#000000</Color>
			<Color x:Key="OnSecondaryColor">#000000</Color>
			<Color x:Key="OnBackgroundColor">#FFFFFF</Color>
			<Color x:Key="OnSurfaceColor">#DEFFFFFF</Color>
			<Color x:Key="OnErrorColor">#000000</Color>
			<Color x:Key="OverlayColor">#51FFFFFF</Color>
		</ResourceDictionary>
	</ResourceDictionary.ThemeDictionaries>
</ResourceDictionary>

```


3. Include the MergedDictionary in your application resources. We recommend in App.xaml like this:
```xaml
<Application x:Class="Uno.Material.Samples.App"
             xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
             xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
             xmlns:material="using:Uno.Material">
    <Application.Resources>
        <ResourceDictionary>
	    <ResourceDictionary.MergedDictionaries>
		<!-- Import all styles from Uno.Material -->
		<material:MaterialLibraryResources />
		<!-- Adjust Path accordingly, this path assumes Colors.xaml is in the same directory as App.xaml -->
		<ResourceDictionary Source="Colors.xaml" />
	    </ResourceDictionary.MergedDictionaries>
	</ResourceDictionary>
    </Application.Resources>
</Application>
```

4. Start using the styles in your pages! 
- To use styles, just find the name of the style from our documentation or sample app and use it like this
```
<Button Content="CONTAINED"
	Style="{StaticResource MaterialContainedButtonStyle}"/>
```

- Here is how to use our custom controls like a Card
```
xmlns:material="using:Uno.Material.Controls"

[...]

<material:Card Header="Outlined card"
	       SubHeader="With title and subitle"
	       Style="{StaticResource MaterialOutlinedCardStyle}" />
```
5. (Optional) Set material styles as the default for your whole application.
For example, if you wish to use our ToggleSwitch style as your default style, simply set it as an implicit style in your app by adding the following code in your App.xaml
```
<Style TargetType="ToggleSwitch"
       BasedOn="{StaticResource MaterialToggleSwitchStyle}"/>
```
You can do the same for each control!
Learn more about implicit styles from the Microsoft documentation [here](https://docs.microsoft.com/en-us/windows/uwp/design/controls-and-patterns/xaml-styles#apply-an-implicit-or-explicit-style)

6. (Optional) Per-control customization.
Just like WinUI, we documented a set of control-specific resources you can override to further customize our controls.
For example, if you would like change the `CornerRadius` of all the `Buttons` using our material styles, you could simply override the `ButtonBorderRadius` value in your resources (in App.xaml would be the simplest way to put the following code)
```
<CornerRadius x:Key="ButtonBorderRadius">4</CornerRadius>
```

7. If you are using our ToggleSwitches to get the proper native colors on android their is some modification needed.
The reasoning for this is to apply the native android shadowing on the off value of the ToggleSwitch, and proper focus shadow colors when ToggleSwitches are clicked

7.1 From your Android project head go to YourProject.Droid/Resources/values/Styles.xml
Inside your AppTheme add two item's "colorControlActivated" (the on color for your ToggleSwitches thumb) and "colorSwitchThumbNormal" (the off color for your ToggleSwitches thumb) you may add your colors here directly, for example #ffffff, or by files (see our example code below)

```
<?xml version="1.0" encoding="utf-8" ?>
<resources>
	<style name="AppTheme" parent="Theme.AppCompat.Light">

		<!-- Color style for toggle switch -->
		<item name="colorControlActivated">@color/PrimaryColor</item>
		<item name="colorSwitchThumbNormal">@color/SurfaceColor</item>
	</style>
</resources>

```

7.2 (Optional) If your application uses Light/Dark color palettes.
7.2.1 Inside the Styles.xml file change the AppTheme's parent to Theme.Compat.DayNight
```
<?xml version="1.0" encoding="utf-8" ?>
<resources>
	<style name="AppTheme" parent="Theme.AppCompat.DayNight">

		<!-- Color style for toggle switch -->
		<item name="colorControlActivated">@color/MaterialPrimaryColor</item>
		<item name="colorSwitchThumbNormal">@color/MaterialSurfaceVariantColor</item>
	</style>
</resources>

```

7.2.2 From your Android project head go to YourProject.Droid/Resources/values create a file called "colors.xml", inside include your "Light" theme colors.
```
<?xml version="1.0" encoding="utf-8" ?>
<resources>
	<color name="MaterialPrimaryColor">#5B4CF5</color>
	<!-- SurfaceColor -->
	<color name="MaterialSurfaceVariantColor">#FFFFFF</color>
</resources>

```

7.2.3 From your Android project head go to YourProject.Droid/Resources create a folder called "values-night", inside the folder add a file called "colors.xml", and inside the file include your "Dark" theme colors.
```
<?xml version="1.0" encoding="utf-8" ?>
<resources>
	<color name="MaterialPrimaryColor">#B6A8FB</color>
	<!-- A variant of OnSurfaceMediumColor without alpha opacity (can't use alphas with android colors)  -->
	 <color name="MaterialSurfaceVariantColor">#808080</color>
</resources>

```

7.3 (Optional) If you have changed the material color palette for your application (2.) then there are two more colors that must be overridden for android native ToggleSwitch disabled colors to be properly applied.
Colors are named PrimaryVariantDisabledThumbColor and SurfaceVariantLightColor, they can be overridden in your colors.xaml file.
PrimaryVariantDisabledThumbColor is a non-transparent version of PrimaryDisabled color ("Light") in "Light" palette, and a non-transparent version of PrimaryMedium color ("Dark") in "Dark" palette.
SurfaceVariantLightColor is the Surface color however in "Light" Palette is an off white color to be visible on light backgrounds.

```
<!-- Variant Colors: Needed for android thumbtints. If a thumbtint color contains opacity, it will actually turn the thumb transparent. (Unwanted behavior) -->
	<ResourceDictionary.ThemeDictionaries>

		<!-- Light Theme -->
		<ResourceDictionary x:Key="Light">
			<!-- Non-opaque/transparent primary disabled color -->
			<Color x:Key="PrimaryVariantDisabledThumbColor">#E9E5FA</Color>
			<!-- Non-opaque/transparent white color that shows on white surfaces -->
			<Color x:Key="SurfaceVariantLightColor">#F7F7F7</Color>
		</ResourceDictionary>

		<!-- Dark Theme -->
		<ResourceDictionary x:Key="Dark">
			<!-- Non-opaque/transparent primary medium color -->
			<Color x:Key="PrimaryVariantDisabledThumbColor">#57507C</Color>
			<Color x:Key="SurfaceVariantLightColor">#121212</Color>
		</ResourceDictionary>
	</ResourceDictionary.ThemeDictionaries>
```


<!-- TODO: Add reference on where to get those resource names -->

## Features

### Styles for basic controls

| **Controls** | **StyleNames**                                                                        | **Visual Reference** |
|----------|-------------------------------------------------------------------------------------------|------------------|
| Button   | MaterialContainedButtonStyle <br> MaterialOutlinedButtonStyle<br> MaterialTextButtonStyle | TODO             |
| CheckBox         | MaterialCheckBoxStyle                                                             | TODO             |
| ComboBox         | MaterialComboBoxStyle                                                             | TODO             |
| CommandBar       | MaterialCommandBarStyle                                                           | TODO             |
| NavigationView   | MaterialNavigationViewStyle <br> MaterialNoCompactMenuNavigationViewStyle         | TODO             |
| PasswordBox      | MaterialFilledPasswordBoxStyle <br> MaterialOutlinedPasswordBoxStyle              | TODO             |
| RadioButton      | MaterialRadioButtonStyle                                                          | TODO             |
| TextBlock        | Headline1 <br> Headline2 <br> Headline3 <br> Headline4 <br> Headline5 <br> Headline6 <br> Subtitle1 <br> Subtitle2 <br> Body1 <br> Body2 <br> Button <br> Caption <br> Overline                                 | TODO             |
| TextBox          | MaterialFilledTextBoxStyle <br> MaterialOutlinedTextBoxStyle                      | TODO             |
| ToggleButton     | MaterialTextToggleButtonStyle                                                     | TODO             |
| ToggleSwitch     | MaterialToggleSwitchStyle                                                         | TODO             |
                                                                               
### Styles for custom controls
| **Controls** | **StyleNames**                                                                        | **Visual Reference** |
|--------------|---------------------------------------------------------------------------------------|------------------|
| Card         | MaterialOutlinedCardStyle <br>  MaterialOtherOutlinedLayoutCardStyle                  | TODO             |
| SnackBar     | MaterialSnackBarStyle                                                                 | TODO             |


## Changelog

Please consult the [CHANGELOG](CHANGELOG.md) for more information about version
history.

## License

This project is licensed under the Apache 2.0 license - see the
[LICENSE](LICENSE) file for details.

## Contributing

Please read [CONTRIBUTING.md](CONTRIBUTING.md) for details on the process for
contributing to this project.

Be mindful of our [Code of Conduct](CODE_OF_CONDUCT.md).

## Acknowledgments
- [Uno Platform](https://platform.uno)
- [Material Design](https://material.io/design)
- [Material Design In XAML, for uwp ripple effect](https://github.com/MaterialDesignInXAML)
- [WinUI](https://microsoft.github.io/microsoft-ui-xaml/)
