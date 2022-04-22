![Uno Material](./images/MaterialBanner.png)

This library is designed to help you use the [material design system](https://material.io/design).
It includes :
- Color system for both Light and Dark theme
- Styles for existing WinUI controls like Buttons, TextBox, etc.

Quickly visualize all the available controls through this [zeplin link](https://zpl.io/scene/bzgq8wG)

Platform support:
- WinUI / UWP
- iOS
- macOS
- Android
- WebAssembly
- Linux (Skia.Gtk)

![Uno Material](./images/UnoMaterial.png)

Uno Material Design Guideline is a resource for designers and software developers that combines Material and Uno design guidance in single document. It is an easy way to kickstart design and implementation of cross-platform experiences with unified Material design system look and feel, using Sketch and Uno Platform.

Download the [Uno Platform Design Guidelines sketch file](./design/Uno-Platform-Design-Guidelines.sketch) to get started.

It includes:
- Uno-Material components
- Uno type resource names
- Uno asset naming and export guidance

<!-- TODO : Insert build status, nuget.org badge, etc -->
[![License](https://img.shields.io/badge/License-Apache%202.0-blue.svg)](LICENSE)

## Getting Started

1. Install the nuget package Uno.Material.
2. Unless you want our default color palette (inspired by our Uno logo), you'll want to override the following color resources in you application. We suggest creating a ColorPaletteOverride.xaml `ResourceDictionary`.
For more information on the color system, consult this [page](https://material.io/design/color/the-color-system.html#color-theme-creation) for all the official documentation and tools to help you create your own palette.
Here is what ColorPaletteOverride.xaml would contain if you want both light and dark theme.
```xaml
<ResourceDictionary
    xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
    xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml">

	<!--

	For more information on the color system, consult this page
	https://material.io/design/color/the-color-system.html#color-theme-creation

	-->
	<ResourceDictionary.ThemeDictionaries>
		<!-- Light Theme -->
		<ResourceDictionary x:Key="Light">
			<Color x:Key="MaterialPrimaryColor">#5B4CF5</Color>
			<Color x:Key="MaterialPrimaryVariantDarkColor">#353FE5</Color>
			<Color x:Key="MaterialPrimaryVariantLightColor">#B6A8FB</Color>
			<Color x:Key="MaterialSecondaryColor">#67E5AD</Color>
			<Color x:Key="MaterialSecondaryVariantDarkColor">#2BB27E</Color>
			<Color x:Key="MaterialSecondaryVariantLightColor">#9CFFDF</Color>
			<Color x:Key="MaterialBackgroundColor">#FFFFFF</Color>
			<Color x:Key="MaterialSurfaceColor">#FFFFFF</Color>
			<Color x:Key="MaterialErrorColor">#F85977</Color>
			<Color x:Key="MaterialOnPrimaryColor">#FFFFFF</Color>
			<Color x:Key="MaterialOnSecondaryColor">#000000</Color>
			<Color x:Key="MaterialOnBackgroundColor">#000000</Color>
			<Color x:Key="MaterialOnSurfaceColor">#000000</Color>
			<Color x:Key="MaterialOnErrorColor">#000000</Color>
			<Color x:Key="MaterialOverlayColor">#51000000</Color>
		</ResourceDictionary>

		<!-- Dark Theme -->
		<ResourceDictionary x:Key="Dark">
			<Color x:Key="MaterialPrimaryColor">#B6A8FB</Color>
			<Color x:Key="MaterialPrimaryVariantDarkColor">#353FE5</Color>
			<Color x:Key="MaterialPrimaryVariantLightColor">#D4CBFC</Color>
			<Color x:Key="MaterialSecondaryColor">#67E5AD</Color>
			<Color x:Key="MaterialSecondaryVariantDarkColor">#2BB27E</Color>
			<Color x:Key="MaterialSecondaryVariantLightColor">#9CFFDF</Color>
			<Color x:Key="MaterialBackgroundColor">#121212</Color>
			<Color x:Key="MaterialSurfaceColor">#121212</Color>
			<Color x:Key="MaterialErrorColor">#CF6679</Color>
			<Color x:Key="MaterialOnPrimaryColor">#000000</Color>
			<Color x:Key="MaterialOnSecondaryColor">#000000</Color>
			<Color x:Key="MaterialOnBackgroundColor">#FFFFFF</Color>
			<Color x:Key="MaterialOnSurfaceColor">#DEFFFFFF</Color>
			<Color x:Key="MaterialOnErrorColor">#000000</Color>
			<Color x:Key="MaterialOverlayColor">#51FFFFFF</Color>
		</ResourceDictionary>
	</ResourceDictionary.ThemeDictionaries>
</ResourceDictionary>

```


3. Initialize the material resources. The order in which the different resources are loaded is important. Add this to `App.xaml`
```xml
<MaterialColors xmlns="using:Uno.Material"
				OverrideSource="ms-appx:///ColorPaletteOverride.xaml" />
<MaterialResources xmlns="using:Uno.Material" />
```

4. (Optional) The material ProgressBar is built on top for the WinUI ProgressBar so make sure you include the appropriate resources in your `App.xaml`

```xml
	<Application.Resources>
		<ResourceDictionary>
			<ResourceDictionary.MergedDictionaries>
				<!-- Load WinUI resources -->
				<XamlControlsResources xmlns="using:Microsoft.UI.Xaml.Controls"/>

				<MaterialColors xmlns="using:Uno.Material"
								OverrideSource="ms-appx:///ColorPaletteOverride.xaml" />
				<MaterialResources xmlns="using:Uno.Material" />
				<!-- Application's custom styles -->
				<!-- other ResourceDictionaries -->
			</ResourceDictionary.MergedDictionaries>
		</ResourceDictionary>
	</Application.Resources>
```
5. Start using the styles in your pages!
- To use styles, just find the name of the style from our documentation or sample app and use it like this
```xaml
<Button Content="CONTAINED"
	Style="{StaticResource MaterialContainedButtonStyle}"/>
```

6. In order to display the appropriate font with the material styles on Webassembly,
make sure that the *Roboto* font is defined on `font.css` located at `[YourProject].Wasm/WasmCSS`.
This make sure that the font is loaded correctly [Related Issue](https://github.com/unoplatform/uno/issues/693).
It should look like [this](https://github.com/unoplatform/Uno.Material/blob/master/src/samples/Uno.Themes.Samples/Uno.Themes.Samples.Wasm/WasmCSS/Fonts.css):

```css
@font-face {
  font-family: "Symbols";
  /* winjs-symbols.woff2: https://github.com/Microsoft/fonts/tree/master/Symbols */
  src: url(data:application/x-font-woff;charset=utf-8;base64,[...]);
}

@font-face {
  font-family: "Roboto";
  src: url(data:application/x-font-woff;charset=utf-8;base64,[...]);
}

body::after {
	font-family: 'Roboto';
	background: transparent;
	content: "";
	opacity: 0;
	pointer-events: none;
	position: absolute;
}
```

7. (Optional) Set material styles as the default for your whole application.
	```xml
	<MaterialResources xmlns="using:Uno.Material" WithImplicitStyles="True" />
	```

	Alternatively, if you wish to only have the default styles for certain controls, simply add the implicit styles to your App.xaml:
	```xml
	<Application.Resources>
		<ResourceDictionary>
			<ResourceDictionary.MergedDictionaries>
				<MaterialColors xmlns="using:Uno.Material" />
				<MaterialResources xmlns="using:Uno.Material" />

				<!-- implicit styles -->
				<ResourceDictionary>
					<Style TargetType="Button" BasedOn="{StaticResource MaterialContainedButtonStyle}"/>
					<Style TargetType="TextBox" BasedOn="{StaticResource MaterialFilledTextBoxStyle}"/>
				</ResourceDictionary>
			</ResourceDictionary.MergedDictionaries>
		</ResourceDictionary>
	</Application.Resources>
	```

	Learn more about implicit styles from the Microsoft documentation [here](https://docs.microsoft.com/en-us/windows/uwp/design/controls-and-patterns/xaml-styles#apply-an-implicit-or-explicit-style)

8. (Optional) Per-control customization.
Just like WinUI, we documented a set of control-specific resources you can override to further customize our controls.
For example, if you would like change the `CornerRadius` of all the `Buttons` using our material styles, you could simply override the `ButtonBorderRadius` value in your resources (in App.xaml would be the simplest way to put the following code)
```xaml
<CornerRadius x:Key="ButtonBorderRadius">4</CornerRadius>
```

9. (Optional) If you are using our [ToggleSwitches](#toggleSwitch) to get proper Material styling in Android there is some extra code to be added to the Android Project Head. (Click the component name to see how to set them up)

10. (Optional) If you are using our [DatePickers, and TimePickers](#datePickers-and-timePickers) to get proper Material styling in Android there is some extra code to be added to the Android Project Head. (Click the component name to see how to set them up)

<!-- TODO: Add reference on where to get those resource names -->

## Features

### Quickly visualize all the available controls through this [zeplin link](https://zpl.io/scene/bzgq8wG)

### Styles for basic controls

| **Controls** | **StyleNames** |
|-|-|
| Button | MaterialContainedButtonStyle <br> MaterialOutlinedButtonStyle <br> MaterialTextButtonStyle <br> MaterialButtonIconStyle <br> MaterialContainedSecondaryButtonStyle <br> MaterialOutlinedSecondaryButtonStyle<br> MaterialTextSecondaryButtonStyle <br> MaterialButtonIconStyle |
| Button (FAB) <br> _Floating Action Button_ | MaterialFabStyle <br> MaterialSmallFabStyle <br> MaterialSecondaryFabStyle <br> MaterialPrimaryInvertedFabStyle <br> MaterialSecondaryInvertedFabStyle |
| CalendarDatePicker | MaterialCalendarDatePickerStyle |
| CalendarView | MaterialCalendarViewStyle |
| CheckBox | MaterialCheckBoxStyle <br> MaterialSecondaryCheckBoxStyle |
| ComboBox | MaterialComboBoxStyle <br> MaterialComboBoxItemStyle |
| CommandBar | MaterialCommandBarStyle <br> MaterialAppBarButton |
| DatePicker | MaterialDatePickerStyle |
| Flyout | MaterialFlyoutPresenterStyle <br> MaterialContentFlyoutPresenterStyle |
| MenuFlyout | MaterialMenuFMaterialMUXNoCompactMenuNavigationViewStylelyoutPresenterStyle <br> MaterialMenuFlyoutItemStyle <br> MaterialToggleMenuFlyoutItemStyle <br> MaterialMenuFlyoutSubItemStyle <br> MaterialMenuFlyoutSeparatorStyle |
| HyperlinkButton | MaterialHyperlinkButtonStyle <br> MaterialSecondaryHyperlinkButtonStyle |
| muxc:InfoBar | MaterialInfoBarStyle |
| ListView | MaterialListViewStyle <br> MaterialListViewDetailsStyle <br> MaterialListViewItemStyle |
| NavigationView | MaterialWUXNavigationViewStyle <br> MaterialWUXNoCompactMenuNavigationViewStyle <br> MaterialWUXNavigationViewItemStyle |
| muxc:NavigationView | MaterialNavigationViewStyle <br> MaterialNavigationViewItemStyle |
| PasswordBox | MaterialFilledPasswordBoxStyle <br> MaterialOutlinedPasswordBoxStyle |
| muxc:ProgressBar | MaterialProgressBarStyle <br> MaterialSecondaryProgressBarStyle |
| muxc:ProgressRing | MaterialProgressRingStyle <br> MaterialSecondaryProgressRingStyle |
| RadioButton | MaterialRadioButtonStyle <br> MaterialSecondaryRadioButtonStyle |
| muxc:RatingControl | MaterialRatingControlStyle <br> MaterialSecondaryRatingControlStyle |
| muxc:Slider | MaterialSliderStyle <br> MaterialSecondarySliderStyle |
| TextBlock        | MaterialHeadline1 <br> MaterialHeadline2 <br> MaterialHeadline3 <br> MaterialHeadline4 <br> MaterialHeadline5 <br> MaterialHeadline6 <br> MaterialSubtitle1 <br> MaterialSubtitle2 <br> MaterialBody1 <br> MaterialBody2 <br> MaterialButtonText <br> MaterialCaption <br> MaterialOverline |
| TextBox          | MaterialFilledTextBoxStyle <br> MaterialOutlinedTextBoxStyle |
| ToggleButton     | MaterialTextToggleButtonStyle <br> MaterialToggleButtonIconStyle |
| ToggleSwitch     | MaterialToggleSwitchStyle <br> MaterialSecondaryToggleSwitchStyle |

## Controls Setup (Specialized)

### ToggleSwitch

If you are using our ToggleSwitches to get the proper native colors on android their is some modification needed.
The reasoning for this is to apply the native android shadowing on the off value of the ToggleSwitch, and proper focus shadow colors when ToggleSwitches are clicked

1. From your Android project head go to YourProject.Droid/Resources/values/Styles.xml
Inside your AppTheme add two item's "colorControlActivated" (the on color for your ToggleSwitches thumb) and "colorSwitchThumbNormal" (the off color for your ToggleSwitches thumb) you may add your colors here directly, for example #ffffff, or by files (see our example code below)

```xml
<?xml version="1.0" encoding="utf-8" ?>
<resources>
	<style name="AppTheme" parent="Theme.AppCompat.Light">

		<!-- Color style for toggle switch -->
		<item name="colorControlActivated">@color/MaterialPrimaryColor</item>
		<item name="colorSwitchThumbNormal">@color/MaterialSurfaceVariantColor</item>
	</style>
</resources>

```

2. (Optional) If your application uses Light/Dark color palettes.
2.1 Inside the Styles.xml file change the AppTheme's parent to Theme.Compat.DayNight
```xml
<?xml version="1.0" encoding="utf-8" ?>
<resources>
	<style name="AppTheme" parent="Theme.AppCompat.DayNight">

		<!-- Color style for toggle switch -->
		<item name="colorControlActivated">@color/MaterialPrimaryColor</item>
		<item name="colorSwitchThumbNormal">@color/MaterialSurfaceVariantColor</item>
	</style>
</resources>

```

2.2 From your Android project head go to YourProject.Droid/Resources/values create a file called "colors.xml", inside include your "Light" theme colors.
```xml
<?xml version="1.0" encoding="utf-8" ?>
<resources>
	<color name="MaterialPrimaryColor">#5B4CF5</color>
	<!-- SurfaceColor -->
	<color name="MaterialSurfaceVariantColor">#FFFFFF</color>
</resources>

```

2.3 From your Android project head go to YourProject.Droid/Resources create a folder called "values-night", inside the folder add a file called "colors.xml", and inside the file include your "Dark" theme colors.
```xml
<?xml version="1.0" encoding="utf-8" ?>
<resources>
	<color name="MaterialPrimaryColor">#B6A8FB</color>
	<!-- A variant of OnSurfaceMediumColor without alpha opacity (can't use alphas with android colors)  -->
	 <color name="MaterialSurfaceVariantColor">#808080</color>
</resources>

```

2.3 (Optional) If you have changed the material color palette for your application (2.) then there are two more colors that must be overridden for android native ToggleSwitch disabled colors to be properly applied.
Colors are named PrimaryVariantDisabledThumbColor and SurfaceVariantLightColor, they can be overridden in your colors.xaml file.
PrimaryVariantDisabledThumbColor is a non-transparent version of PrimaryDisabled color ("Light") in "Light" palette, and a non-transparent version of PrimaryMedium color ("Dark") in "Dark" palette.
SurfaceVariantLightColor is the Surface color however in "Light" Palette is an off white color to be visible on light backgrounds.

```xaml
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

### DatePickers and TimePickers

If your application uses DatePickers and/or TimePickers (these are native components).
To apply your material colors to these android components, do the following (this will affect every DatePicker/TimePicker in the application).

<!-- 8.1 Override the IOS TimePicker ColorBrushes in a binded ResourceDictionary such as colors.xaml:

```xaml
<SolidColorBrush x:Key="IOSTimePickerAcceptButtonForegroundBrush"
				 Color="{StaticResource MaterialPrimaryBrush}" />
<SolidColorBrush x:Key="IOSTimePickerDismissButtonForegroundBrush"
				 Color="{StaticResource MaterialPrimaryBrush}" />
``` -->

1. From your Android project head go to YourProject.Droid/Resources/values/Styles.xml
Inside your AppTheme add two item's "datePickerDialogTheme" (the style for your DatePicker) and "timePickerDialogTheme" (the style for your TimePicker), and a new Style with the MaterialPrimary Color as AccentColor (see our example code below)

```xml
<?xml version="1.0" encoding="utf-8" ?>
<resources>
	<style name="AppTheme" parent="Theme.AppCompat.Light">

		<!-- Color style for Time/Date Pickers -->
		<item name="android:datePickerDialogTheme">@style/AppCompatDialogStyle</item>
		<item name="android:timePickerDialogTheme">@style/AppCompatDialogStyle</item>
	</style>

	<style name="AppCompatDialogStyle" parent="Theme.AppCompat.Light.Dialog">
		<item name="colorAccent">@color/MaterialPrimaryColor</item>
	</style>
</resources>

```

2. (Optional) If your application uses Light/Dark color palettes.
2.1 Inside the Styles.xml file change the AppTheme's parent of both styles to Theme.Compat.DayNight
```xml
<?xml version="1.0" encoding="utf-8" ?>
<resources>
	<style name="AppTheme" parent="Theme.AppCompat.DayNight">

		<!-- Color style for Time/Date Pickers -->
		<item name="android:datePickerDialogTheme">@style/AppCompatDialogStyle</item>
		<item name="android:timePickerDialogTheme">@style/AppCompatDialogStyle</item>
	</style>

	<style name="AppCompatDialogStyle" parent="Theme.AppCompat.DayNight.Dialog">
		<item name="colorAccent">@color/MaterialPrimaryColor</item>
	</style>
</resources>

```

2.2 From your Android project head go to YourProject.Droid/Resources/values create a file called "colors.xml", inside include your "Light" theme colors.
```xml
<?xml version="1.0" encoding="utf-8" ?>
<resources>
	<color name="MaterialPrimaryColor">#5B4CF5</color>
</resources>
```

2.3 From your Android project head go to YourProject.Droid/Resources create a folder called "values-night", inside the folder add a file called "colors.xml", and inside the file include your "Dark" theme colors.
```xml
<?xml version="1.0" encoding="utf-8" ?>
<resources>
	<color name="MaterialPrimaryColor">#B6A8FB</color>
</resources>
```

## Migration

### 1.0 to 1.1

- Color Palette Override

	Now you have the possibility to override the Material color palette with your own color palette. See the #Getting Started section for more details.
	```xml
	<MaterialColors xmlns="using:Uno.Material"
					OverrideSource="ms-appx:///ColorPaletteOverride.xaml" />
	```

- Namespace breaking changes

	BREAKING CHANGE: Everything (controls, extensions, converters, ...), previously under `Uno.Material.*` or `Uno.Cupertino.*`, has now been moved under `Uno.Material` or `Uno.Cupertino`.

	```xml
	xmlns:um="using:Uno.Material"
	xmlns:uc="using:Uno.Cupertino"
	```

- Some controls have been moved to [Uno.Toolkit.UI](https://github.com/unoplatform/uno.toolkit.ui)

	List of the controls and styles that have been moved to [Uno.Toolkit.UI](https://github.com/unoplatform/uno.toolkit.ui):

	| **Controls**              | **StyleNames**                                                                |
	|---------------------------|-------------------------------------------------------------------------------|
	| Card                      | MaterialOutlinedCardStyle <br> MaterialElevatedCardStyle <br> MaterialAvatarOutlinedCardStyle <br> MaterialAvatarElevatedCardStyle <br> MaterialSmallMediaOutlinedCardStyle <br> MaterialSmallMediaElevatedCardStyle |
	| Chip                      | MaterialFilledInputChipStyle<br>MaterialFilledChoiceChipStyle<br>MaterialFilledFilterChipStyle<br>MaterialFilledActionChipStyle<br>MaterialOutlinedInputChipStyle<br>MaterialOutlinedChoiceChipStyle<br>MaterialOutlinedFilterChipStyle<br>MaterialOutlinedActionChipStyle |
	| ChipGroup                 | MaterialFilledInputChipGroupStyle<br>MaterialFilledChoiceChipGroupStyle<br>MaterialFilledFilterChipGroupStyle<br>MaterialFilledActionChipGroupStyle<br>MaterialOutlinedInputChipGroupStyle<br>MaterialOutlinedChoiceChipGroupStyle<br>MaterialOutlinedFilterChipGroupStyle<br>MaterialOutlinedActionChipGroupStyle |
	| Divider                   | MaterialDividerStyle |

- Some controls have been removed

	List of the controls and styles that have been removed from Uno.Themes:

	| **Controls**              | **StyleNames**                                                                |
	|---------------------------|------------------------------------------------------------------------------|
	| BottomNavigationBar       | MaterialBottomNavigationBarStyle                                              |
	| ExpandingBottomSheet      | MaterialExpandingBottomSheetStyle                                             |
	| ModalStandardBottomSheet  | MaterialModalStandardBottomSheetStyle                                         |
	| StandardBottomSheet       | MaterialStandardBottomSheetStyle                                              |
	| SnackBar                  | MaterialSnackBarStyle                                                         |

	- BottomNavigationBar was replaced by TabBar in [Uno.Toolkit.UI](https://github.com/unoplatform/uno.toolkit.ui), but it is not an exact 1:1 replacement.
	In the mean time, if you really need the badge and/or other customizability, two options are available:

		Import locally the old sources (control + style) from Uno.Themes;

		OR

		Copy the MaterialBottomTabBarItemStyle from [Uno.Toolkit.UI](https://github.com/unoplatform/uno.toolkit.ui), and modify the style to suit your needs.
		(Note that there are two copies of the style, one for iOS and Android and one for rest of the platforms: UWP, Skia, WASM, etc...);

	- For StandardBottomSheet and ModalStandardBottomSheet
	It's replaced by using a Flyout and the MaterialFlyoutPresenterStyle that is allowing you to have a bottom sheet.

		For example:
		```xml
		<Flyout Placement="Full"
                LightDismissOverlayMode="On"
                FlyoutPresenterStyle="{StaticResource MaterialFlyoutPresenterStyle}">
			<Grid MaxHeight="370"
				  VerticalAlignment="Bottom">
				...Your bottom sheet content...
			</Grid>
		</Flyout>
		```

	- For SnackBar: so far no replacement for SnackBar has been added to [Uno.Toolkit.UI](https://github.com/unoplatform/uno.toolkit.ui), but it's planned to add one in a future version.

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
- [Material Design In XAML](https://github.com/MaterialDesignInXAML) as inspiration for UWP ripple effect
- [ShowMeTheXaml](https://github.com/Keboo/ShowMeTheXAML) for code snippets. Through [our Fork](https://github.com/unoplatform/ShowMeTheXAML)
- [WinUI](https://microsoft.github.io/microsoft-ui-xaml/)

