# Updating to Uno.Material v2
Starting with version 2.0.0 of the [Uno.Material](https://www.nuget.org/packages/Uno.Material/2.0.0) and [Uno.Material.WinUI](https://www.nuget.org/packages/Uno.Material.WinUI/2.0.0) packages, users can now take advantage of the new [Material Design 3](https://m3.material.io/) design system from Google.
Along with the new Material Design 3 styles, our Uno.Material NuGet packages will continue to support the previous Material Design 2 styles. In order to achieve this backward compatibility, we have had to make some changes to the way Uno.Material is initialized within your `App.xaml`. 

> [!NOTE]
In order to avoid any confusion going forward in this article, we will be referring to the new collection of Material Design 3 styles as the "v2" styles and the previous collection of Material Design 2 styles will be referred to as the "v1" styles.

 There are two available paths once you have updated to the new Uno.Material v2 package: 

- Continue to keep using the v1 styles; or,
- Migrate app to reference the new v2 styles


## Continue Using v1 Styles
> [!WARNING]
> In order to continue using the v1 styles, some changes are required in your `App.xaml`.


The Uno.Material v2 NuGet package contains both sets of v1 and v2 styles. Within your `App.xaml`, you will need to replace the references to `MaterialColors` and `MaterialResouces` with `MaterialColorsV1` and `MaterialResourcesV1`. `MaterialFonts` remains unchanged.

## Migrating to v2 Styles
Both `MaterialColors` and `MaterialResources` will now always initialize the latest version of the Material styles. It is also possible to directly reference the `MaterialColorsV2` and `MaterialResourcesV2` ResourceDictionaries if needed.

The v2 styles introduce a new naming system for its resource keys. Refer to the [Material Styles Matrix](material-controls-styles.md) for the updated style keys. In addition to the new style keys, the Uno.Material color palette and text resources have also been updated to align with the [Material Design 3 Color System](https://m3.material.io/styles/color/the-color-system/key-colors-tones) and the [Material Design 3 Typography Guidelines](https://m3.material.io/styles/typography/type-scale-tokens).

### Color Update
 A new color palette has been created for v2, meaning any color palette overrides ResourceDictionary in your app must be updated with the new resource keys. An example of the new color palette can be seen in the new [Uno.Material default palette](https://github.com/unoplatform/Uno.Themes/blob/master/src/library/Uno.Material/Styles/Application/v2/SharedColorPalette.xaml). For more information on the updated colors, you can refer to the "Colors and Themes" section of the [Material 3 Migration Guide](https://material.io/blog/migrating-material-3).

 ### Typography Updates
 Text styles have also been modified in v2. There are no 1-to-1 mapping between v1 and v2 text styles in terms of font-sizes and usages. The "Typography" section of the [Material 3 Migration Guide](https://material.io/blog/migrating-material-3) can be helpful for choosing the right style.

Below is a table that is not a strict guideline, but, is provided to you as a suggestion of mapping text styles from v1 to v2:

| Previous Style     	| New Style               	|
|--------------------	|-------------------------	|
| MaterialHeadline1  	| MaterialDisplaySmall    	|
| MaterialHeadline2  	| MaterialHeadlineLarge   	|
| MaterialHeadline3  	| MaterialHeadlineMedium  	|
| MaterialHeadline4  	| MaterialHeadlineSmall   	|
| MaterialHeadline5  	| MaterialTitleLarge      	|
| MaterialSubtitle1  	| MaterialTitleMedium     	|
| MaterialSubtitle2  	| MaterialTitleSmall      	|
| MaterialBody1      	| MaterialBodyLarge       	|
| MaterialBody2      	| MaterialBodyMedium      	|
| MaterialCaption    	| MaterialBodySmall       	|
| MaterialButtonText 	| MaterialLabelLarge      	|
| MaterialOverline   	| MaterialLabelMedium     	|
| N/A                	| MaterialLabelSmall      	|
| N/A                	| MaterialLabelExtraSmall 	|
| N/A                	| MaterialCaptionLarge    	|
| N/A                	| MaterialCaptionMedium   	|
| N/A                	| MaterialCaptionSmall    	|
| N/A                	| MaterialDisplayLarge    	|
| N/A                	| MaterialDisplayMedium   	|

### Notable Style Key Changes

Control|Previous Style Key|New Style Key|
-|-|-
Button|MaterialButtonIconStyle|MaterialIconButtonStyle|
Button|MaterialContainedButtonStyle|MaterialFilledButtonStyle|
ToggleButton|MaterialToggleButtonIconStyle|MaterialIconToggleButtonStyle|



