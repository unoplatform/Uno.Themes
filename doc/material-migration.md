---
uid: uno.themes.material.migration
---

# Updating to Uno.Material v3

Uno.Material v3 (not to be confused with [Material Design 3](https://m3.material.io/) from Google) introduces support for [Lightweight Styling](lightweight-styling.md) as well as some breaking changes to the default style keys for some controls. Refer to the tables below for the changes that have been made within Uno.Material.

## Styles

### Removed

Key|TargetType
-|-
DefaultMaterialCalendarViewStyle|CalendarView
MaterialSecondaryCheckBoxStyle|CheckBox
MaterialSecondaryRadioButtonStyle|RadioButton

### Added

Key|Aliased Key|TargetType|Implicit Style
-|-|-|-
MaterialRatingControlStyle|RatingControlStyle|muxc:RatingControl|true
MaterialRippleStyle|RippleStyle|um:Ripple|true

### Modified

Key|Aliased Key|TargetType|Implicit Style
-|-|-|-
MaterialToggleMenuFlyoutItemStyle|ToggleMenuFlyoutItemStyle|ToggleMenuFlyoutItem|false -> true
MaterialMenuFlyoutSubItemStyle|MenuFlyoutSubItemStyle|MenuFlyoutSubItem|false -> true
MaterialMenuFlyoutSeparatorStyle|MenuFlyoutSeparatorStyle|MenuFlyoutSeparator|false -> true
MaterialFilledPasswordBoxStyle|FilledPasswordBoxStyle|PasswordBox|true -> false
MaterialOutlinedPasswordBoxStyle|OutlinedPasswordBoxStyle|PasswordBox|false -> true
MaterialFilledTextBoxStyle|FilledTextBoxStyle|TextBox|true -> false
MaterialOutlinedTextBoxStyle|OutlinedTextBoxStyle|TextBox|false -> true
MaterialTextToggleButtonStyle|TextToggleButtonStyle|ToggleButton|true -> false
MaterialIconToggleButtonStyle|IconToggleButtonStyle|ToggleButton|false -> true

## Resources

As a result of the Lightweight Styling support, many resource keys have been added as well as renamed. For a list of all the new resource keys, please refer to the [Lightweight Styling documentation](lightweight-styling.md#resource-keys).

Along with the above list of new resource keys, below is a list of the resource keys that have been removed or renamed.

> [!NOTE]
> Most resources, including those that have been added or renamed, have now been placed inside of a `ThemeDictionary`. This means that the resources should now be referenced using the `ThemeResource` markup extension instead of `StaticResource`. For more information on theme resources, please refer to the [XAML theme resources documentation](https://learn.microsoft.com/en-us/windows/apps/design/style/xaml-theme-resources).

### Button

Old Key|New Key|Value
-|-|-
MaterialButtonHorizontalContentAlignment|ButtonHorizontalContentAlignment|Center
MaterialButtonVerticalContentAlignment|ButtonVerticalContentAlignment|Center
MaterialButtonCornerRadius|**_REMOVED_**|20
MaterialOutlinedButtonBorderThickness|**_REMOVED_**|1
MaterialButtonBorderThickness|**_REMOVED_**|0
MaterialTextButtonPadding|**_REMOVED_**|12,0
MaterialButtonPadding|**_REMOVED_**|16,0
MaterialButtonMinWidth|**_REMOVED_**|40
MaterialButtonMinHeight|**_REMOVED_**|40
MaterialOutlinedButtonBorderBrush|**_REMOVED_**|OutlineBrush
MaterialNullToTextButtonMarginConverter|NullToTextButtonMarginConverter|

### CalendarDatePicker

Old Key|New Key|Value
-|-|-
MaterialCalendarDatePickerBackground|**_REMOVED_**|OnSurfaceColor*0.12

### CheckBox

Old Key|New Key|Value
-|-|-
CheckAreaCornerRadius|**_REMOVED_**|4
CheckAreaSize|**_REMOVED_**|18
FocusAreaSize|**_REMOVED_**|40
CheckAreaLength|**_REMOVED_**|40
CheckBoxCheckGlyphPathStyle|**_REMOVED_**|M28.718018,0L32,3.2819897 10.666016,24.616999 0,13.951997 3.2810059,10.670007 10.666016,18.055033z
CheckBoxHyphenGlyphPathStyle|**_REMOVED_**|M0,0L32,0 32,5.3 0,5.3z

### ComboBox

Old Key|New Key|Value
-|-|-
MaterialComboBoxPadding|**_REMOVED_**|16,0
MaterialComboBoxOpenedBorderThickness|**_REMOVED_**|2
MaterialComboBoxBorderThickness|**_REMOVED_**|1
DownArrowPathData|ComboBoxDownArrowPathData|M0 0L5 5L10 0H0Z
UpArrowPathData|ComboBoxUpArrowPathData|M0 0L-5 -5L-10 0H0Z
MaterialComboBoxCornerRadius|**_REMOVED_**|4

### DatePicker

Old Key|New Key|Value
-|-|-
MaterialDatePickerBackgroundColorBrush|**_REMOVED_**|OnSurfaceColor*0.12
MaterialDatePickerFlyoutPresenterSpacerFill|**_REMOVED_**|OnSurfaceFocusedBrush
MaterialDatePickerFlyoutElevation|**_REMOVED_**|8
MaterialDatePickerFlyoutPresenterHighlightFill|**_REMOVED_**|PrimaryColor*0.2
MaterialDatePickerFlyoutPresenterBorderBrush|**_REMOVED_**|OnSurfaceFocusedBrush
MaterialDatePickerHostPadding|**_REMOVED_**|24,24,8,8
MaterialDatePickerFlyoutPresenterBackgroundBrush|**_REMOVED_**|SurfaceBrush
MaterialDatePickerHeight|**_REMOVED_**|53
MaterialDatePickerSpacerThemeWidth|**_REMOVED_**|1
MaterialDateTimeFlyoutBorderThickness|**_REMOVED_**|1

### FloatingActionButton

Old Key|New Key|Value
-|-|-
MaterialFabIconTextPadding|**_REMOVED_**|12
MaterialLargeFabCornerRadius|**_REMOVED_**|28
MaterialLargeFabContentWidthOrHeight|**_REMOVED_**|24
MaterialLargeFabPadding|**_REMOVED_**|36
MaterialSmallFabCornerRadius|**_REMOVED_**|12
MaterialSmallFabContentWidthOrHeight|**_REMOVED_**|16
MaterialSmallFabPadding|**_REMOVED_**|12
MaterialFabCornerRadius|**_REMOVED_**|16
MaterialFabContentWidthOrHeight|**_REMOVED_**|16
MaterialFabPadding|**_REMOVED_**|20
MaterialTertiaryFabDisabledStateOverlay|**_REMOVED_**|OnSurfaceDisabledLowBrush
MaterialTertiaryFabPressedStateOverlay|**_REMOVED_**|OnTertiaryContainerFocusedBrush
MaterialTertiaryFabFocusedStateOverlay|**_REMOVED_**|OnTertiaryContainerFocusedBrush
MaterialTertiaryFabPointerOverStateOverlay|**_REMOVED_**|OnTertiaryContainerHoverBrush
MaterialTertiaryFabDisabledForeground|**_REMOVED_**|OnSurfaceDisabledBrush
MaterialTertiaryFabDisabledBackground|**_REMOVED_**|SystemControlTransparentBrush
MaterialTertiaryFabBackground|**_REMOVED_**|TertiaryContainerBrush
MaterialTertiaryFabForeground|**_REMOVED_**|OnTertiaryContainerBrush
MaterialSecondaryFabDisabledStateOverlay|**_REMOVED_**|OnSurfaceDisabledLowBrush
MaterialSecondaryFabPressedStateOverlay|**_REMOVED_**|OnSecondaryContainerPressedBrush
MaterialSecondaryFabFocusedStateOverlay|**_REMOVED_**|OnSecondaryContainerFocusedBrush
MaterialSecondaryFabPointerOverStateOverlay|**_REMOVED_**|OnSecondaryContainerHoverBrush
MaterialSecondaryFabDisabledForeground|**_REMOVED_**|OnSurfaceDisabledBrush
MaterialSecondaryFabDisabledBackground|**_REMOVED_**|SystemControlTransparentBrush
MaterialSecondaryFabBackground|**_REMOVED_**|SecondaryContainerBrush
MaterialSecondaryFabForeground|**_REMOVED_**|OnSecondaryContainerBrush
MaterialSurfaceFabDisabledStateOverlay|**_REMOVED_**|OnSurfaceDisabledLowBrush
MaterialSurfaceFabDisabledBackground|**_REMOVED_**|SystemControlTransparentBrush
MaterialSurfaceFabPressedStateOverlay|**_REMOVED_**|PrimaryPressedBrush
MaterialSurfaceFabFocusedStateOverlay|**_REMOVED_**|PrimaryFocusedBrush
MaterialSurfaceFabPointerOverStateOverlay|**_REMOVED_**|PrimaryHoverBrush
MaterialSurfaceFabDisabledForeground|**_REMOVED_**|OnSurfaceDisabledBrush
MaterialSurfaceFabBackground|**_REMOVED_**|SurfaceBrush
MaterialSurfaceFabForeground|**_REMOVED_**|PrimaryBrush
MaterialFabDisabledStateOverlay|**_REMOVED_**|OnSurfaceDisabledLowBrush
MaterialFabPressedStateOverlay|**_REMOVED_**|OnPrimaryContainerPressedBrush
MaterialFabFocusedStateOverlay|**_REMOVED_**|OnPrimaryContainerFocusedBrush
MaterialFabPointerOverStateOverlay|**_REMOVED_**|OnPrimaryContainerHoverBrush
MaterialFabDisabledForeground|**_REMOVED_**|OnSurfaceDisabledBrush
MaterialFabDisabledBackground|**_REMOVED_**|SystemControlTransparentBrush
MaterialFabBackground|**_REMOVED_**|PrimaryContainerBrush
MaterialFabForeground|**_REMOVED_**|OnPrimaryContainerBrush

### NavigationView

Old Key|New Key|Value
-|-|-
NavigationViewItemExpandedGlyph|**_REMOVED_**|
NavigationViewItemExpandedGlyphFontSize|**_REMOVED_**|8
NavigationViewItemChildrenMenuFlyoutPadding|**_REMOVED_**|0,8
TopNavigationViewOverflowMenuPadding|**_REMOVED_**|0,8
NavigationViewMinimalContentGridCornerRadius|**_REMOVED_**|0
TopNavigationViewContentGridCornerRadius|**_REMOVED_**|0
NavigationViewContentGridCornerRadius|**_REMOVED_**|8,0,0,0
TopNavigationViewItemOnOverflowExpandChevronPadding|**_REMOVED_**|12,0
TopNavigationViewItemOnOverflowExpandChevronMargin|**_REMOVED_**|-4,0,-8,0
TopNavigationViewItemOnOverflowNoIconContentPresenterMargin|**_REMOVED_**|16,0,20,0
TopNavigationViewItemOnOverflowContentPresenterMargin|**_REMOVED_**|12,0,20,0
TopNavigationViewItemContentOnlyExpandChevronMargin|**_REMOVED_**|-12,0,0,0
TopNavigationViewItemIconOnlyExpandChevronMargin|**_REMOVED_**|0
TopNavigationViewItemExpandChevronMargin|**_REMOVED_**|-16,0,0,0
NavigationViewItemExpandChevronMargin|**_REMOVED_**|0,0,-14,0
TopNavigationViewItemContentOnlyContentPresenterMargin|**_REMOVED_**|12,0
TopNavigationViewItemContentPresenterMargin|**_REMOVED_**|8,-1,12,-1
NavigationViewCompactItemContentPresenterMargin|**_REMOVED_**|0
NavigationViewItemContentPresenterMargin|**_REMOVED_**|4,-1,8,-1
TopNavigationViewOverflowButtonMargin|**_REMOVED_**|0
TopNavigationViewItemSeparatorMargin|**_REMOVED_**|3,0,4,0
NavigationViewCompactItemSeparatorMargin|**_REMOVED_**|0,3,0,4
NavigationViewItemSeparatorMargin|**_REMOVED_**|0,3,0,4
TopNavigationViewItemMargin|**_REMOVED_**|0
NavigationViewItemMargin|**_REMOVED_**|0
NavigationViewPaneTitlePresenterMargin|**_REMOVED_**|8,4,0,0
TopNavigationViewContentMargin|**_REMOVED_**|0
NavigationViewMinimalContentMargin|**_REMOVED_**|0
NavigationViewContentMargin|**_REMOVED_**|0
NavigationViewContentPresenterMargin|**_REMOVED_**|0
NavigationViewHeaderMargin|**_REMOVED_**|56,44,0,0
NavigationViewBorderThickness|**_REMOVED_**|1
TopNavigationViewTopNavGridMargin|**_REMOVED_**|4,0
TopNavigationViewContentGridBorderThickness|**_REMOVED_**|0,1,0,0
NavigationViewMinimalContentGridBorderThickness|**_REMOVED_**|0,1,0,0
NavigationViewContentGridBorderThickness|**_REMOVED_**|1,1,0,0
NavigationViewPaneContentGridMargin|**_REMOVED_**|-1,3
NavigationViewButtonHolderGridMargin|**_REMOVED_**|0
NavigationViewMinimalHeaderMargin|**_REMOVED_**|-24,44,0,0
TopNavigationViewItemInnerHeaderMargin|**_REMOVED_**|12,0
NavigationViewItemInnerHeaderMargin|**_REMOVED_**|16,0
NavigationViewItemButtonMargin|**_REMOVED_**|12,0
NavigationViewItemOnLeftIconBoxMargin|**_REMOVED_**|3
NavigationViewItemBorderThickness|**_REMOVED_**|1
NavigationViewToggleBorderThickness|**_REMOVED_**|0
TopNavigationViewItemSeparatorWidth|**_REMOVED_**|1
NavigationViewItemSeparatorHeight|**_REMOVED_**|1
NavigationViewSelectionIndicatorRadius|**_REMOVED_**|2
NavigationViewSelectionIndicatorHeight|**_REMOVED_**|24
NavigationViewSelectionIndicatorWidth|**_REMOVED_**|3
NavigationViewItemOnLeftIconBoxHeight|**_REMOVED_**|24
NavigationViewPaneHeaderRowMinHeight|**_REMOVED_**|56
NavigationViewItemOnLeftMinHeight|**_REMOVED_**|56
TopNavigationViewSettingsButtonHeight|**_REMOVED_**|40
TopNavigationViewSettingsButtonWidth|**_REMOVED_**|40
TopNavigationViewOverflowButtonHeight|**_REMOVED_**|40
TopNavigationViewOverflowButtonWidth|**_REMOVED_**|40
TopNavigationViewPaneCustomContentMinWidth|**_REMOVED_**|80
NavigationViewAutoSuggestAreaHeight|**_REMOVED_**|40
NavigationViewTopPaneHeight|**_REMOVED_**|48
NavigationViewIconBoxWidth|**_REMOVED_**|40
NavigationViewCompactPaneLength|**_REMOVED_**|80
PaneToggleButtonWidth|**_REMOVED_**|80
PaneToggleButtonHeight|**_REMOVED_**|56
TopNavigationViewAutoSuggestBoxMargin|**_REMOVED_**|4,0
NavigationViewAutoSuggestBoxMargin|**_REMOVED_**|16,0
MaterialNavigationViewItemRippleFeedback|**_REMOVED_**|PrimaryPressedBrush
MaterialNavigationViewItemBackgroundDisabled|**_REMOVED_**|OnSurfaceDisabledLowBrush
MaterialNavigationViewItemBackgroundPointerOver|**_REMOVED_**|PrimaryHoverBrush
MaterialNavigationViewItemBackgroundPressed|**_REMOVED_**|PrimaryPressedBrush
MaterialNavigationViewItemBackgroundSelected|**_REMOVED_**|PrimarySelectedBrush
MaterialNavigationItemContentMarginWithoutIcon|**_REMOVED_**|0
MaterialNavigationItemContentMargin|**_REMOVED_**|12,0,0,0
MaterialNavigationItemIconLength|**_REMOVED_**|24
MaterialNavigationItemLeftMargin|**_REMOVED_**|12,0
MaterialNavigationViewItemCornerRadiusLeftOnly|**_REMOVED_**|28,0,0,28
MaterialNavigationViewItemCornerRadius|**_REMOVED_**|28
MaterialNavigationItemHeight|**_REMOVED_**|56
MaterialNavigationViewButtonRippleFeedback|**_REMOVED_**|PrimaryPressedBrush
MaterialNavigationViewButtonBackgroundPointerOver|**_REMOVED_**|PrimaryHoverBrush
MaterialNavigationViewButtonBackgroundPressed|**_REMOVED_**|PrimaryPressedBrush
MaterialNavigationViewButtonForegroundDisabled|**_REMOVED_**|OnSurfaceDisabledLowBrush
MaterialNavigationViewButtonMarginWhenStackedVertically|**_REMOVED_**|12,0
MaterialNavigationViewButtonIconSymbolFontSize|**_REMOVED_**|24
MaterialNavigationViewButtonIconLength|**_REMOVED_**|24
MaterialNavigationViewButtonCornerRadius|**_REMOVED_**|28
MaterialNavigationViewButtonWidth|**_REMOVED_**|56
MaterialNavigationViewContentGridCornerRadius|**_REMOVED_**|14,0,0,14
MaterialNavigationViewSplitViewCornerRadius|**_REMOVED_**|0,14,14,0
MaterialNavigationViewPaneBackground|**_REMOVED_**|SurfaceBrush
MaterialNavigationViewBackground|**_REMOVED_**|SurfaceBrush

### PasswordBox

Old Key|New Key|Value
-|-|-
MaterialDisabledOutlinedPasswordBoxBorderBrush|**_REMOVED_**|OnSurfaceColor*0.12
MaterialRevealGlyphPathData|**_REMOVED_**|M11 0.5C6 0.5 1.73 3.61 0 8C1.73 12.39 6 15.5 11 15.5C16 15.5 20.27 12.39 22 8C20.27 3.61 16 0.5 11 0.5ZM11 13C8.24 13 6 10.76 6 8C6 5.24 8.24 3 11 3C13.76 3 16 5.24 16 8C16 10.76 13.76 13 11 13ZM11 5C9.34 5 8 6.34 8 8C8 9.66 9.34 11 11 11C12.66 11 14 9.66 14 8C14 6.34 12.66 5 11 5Z

### PipsPager

Old Key|New Key|Value
-|-|-
MaterialPipsPagerNormalEllipseSize|**_REMOVED_**|8
MaterialPipsPagerSelectedEllipseSize|**_REMOVED_**|8
MaterialPipsPagerNavigationVisualStatesEllipseHeight|**_REMOVED_**|12
MaterialPipsPagerNavigationVisualStatesEllipseWidth|**_REMOVED_**|12
MaterialPipsPagerNavigationButtonWidth|**_REMOVED_**|40
MaterialPipsPagerNavigationButtonHeight|**_REMOVED_**|40
MaterialPipsPagerNextPageButtonData|**_REMOVED_**|M4.66319 5.67785C4.36844 6.10738 3.63156 6.10738 3.33681 5.67785L0.103738 0.966444C-0.191015 0.536913 0.177425 6.83871e-07 0.766931 6.32335e-07L7.23307 6.70473e-08C7.82257 1.55111e-08 8.19101 0.536912 7.89626 0.966443L4.66319 5.67785Z
MaterialPipsPagerPreviousPageButtonData|**_REMOVED_**|M3.33681 0.322148C3.63156 -0.107383 4.36844 -0.107382 4.66319 0.322148L7.89626 5.03356C8.19101 5.46309 7.82257 6 7.23307 6L0.766932 6C0.177427 6 -0.191014 5.46309 0.103739 5.03356L3.33681 0.322148Z
MaterialPipsPagerNavigationButtonBorderThickness|**_REMOVED_**|0
MaterialPipsPagerButtonBorderThickness|**_REMOVED_**|0
MaterialPipsPagerVerticalOrientationButtonHeight|**_REMOVED_**|12
MaterialPipsPagerVerticalOrientationButtonWidth|**_REMOVED_**|12
MaterialPipsPagerHorizontalOrientationButtonHeight|**_REMOVED_**|12
MaterialPipsPagerHorizontalOrientationButtonWidth|**_REMOVED_**|12

### ProgressBar

Old Key|New Key|Value
-|-|-
MaterialProgressBarHeight|**_REMOVED_**|4
MaterialProgressBarMinWidth|**_REMOVED_**|250

### ProgressRing

Old Key|New Key|Value
-|-|-
M3MaterialIndeterminateAnimation_Uno|**_REMOVED_**|embedded://Uno.Material/Uno.Material.Assets.MaterialIndeterminate.json
M3MaterialDeterminateAnimation_Uno|**_REMOVED_**|embedded://Uno.Material/Uno.Material.Assets.MaterialDeterminate.json
M3MaterialIndeterminateAnimation_Win|**_REMOVED_**|ms-appx:///Uno.Material/Assets/MaterialIndeterminate.json
M3MaterialDeterminateAnimation_Win|**_REMOVED_**|ms-appx:///Uno.Material/Assets/MaterialDeterminate.json

### RadioButton

Old Key|New Key|Value
-|-|-
RadioBorderThickness|**_REMOVED_**|2
RadioCheckAreaSize|**_REMOVED_**|20
RadioStatesAreaSize|**_REMOVED_**|40
RadioStatesAreaLength|**_REMOVED_**|40

### Slider

Old Key|New Key|Value
-|-|-
MaterialSliderThumbBackgroundDisabled|**_REMOVED_**|SystemControlDisabledChromeDisabledHighBrush
MaterialSliderInlineTickBarFill|**_REMOVED_**|SystemControlBackgroundAltHighBrush
MaterialSliderTickBarFillDisabled|**_REMOVED_**|SystemControlDisabledBaseMediumLowBrush
MaterialSliderTickBarFill|**_REMOVED_**|OnSecondaryLowBrush
MaterialSliderTrackBrush|**_REMOVED_**|MaterialSliderTrackColor

### TextBox

Old Key|New Key|Value
-|-|-
MaterialDisabledOutlinedTextBoxBorderBrush|**_REMOVED_**|OnSurfaceColor*0.12
M3ClearGlyphPathData|**_REMOVED_**|M10 0C4.47 0 0 4.47 0 10C0 15.53 4.47 20 10 20C15.53 20 20 15.53 20 10C20 4.47 15.53 0 10 0ZM10 18C5.59 18 2 14.41 2 10C2 5.59 5.59 2 10 2C14.41 2 18 5.59 18 10C18 14.41 14.41 18 10 18ZM10 8.59L13.59 5L15 6.41L11.41 10L15 13.59L13.59 15L10 11.41L6.41 15L5 13.59L8.59 10L5 6.41L6.41 5L10 8.59Z

### ToggleButton

Old Key|New Key|Value
-|-|-
MaterialToggleButtonPadding|**_REMOVED_**|16,8
MaterialToggleButtonBorderRadius|**_REMOVED_**|4
MaterialToggleButtonForegroundThemeBrush|**_REMOVED_**|OnSurfaceBrush
MaterialToggleButtonTextLabelBrush|**_REMOVED_**|PrimaryColor

### ToggleSwitch

Old Key|New Key|Value
-|-|-
MaterialPrimaryVariantLowThumbColorBrush|**_REMOVED_**|MaterialPrimaryVariantLowThumbColor
MaterialToggleSwitchOnLowBackgroundBrush|**_REMOVED_**|OnSurfaceSelectedBrush
MaterialToggleSwitchOnLowButtonBrush|**_REMOVED_**|OnSurfaceSelectedBrush
MaterialToggleSwitchOffBackgroundBrush|**_REMOVED_**|OnSurfaceLowBrush
MaterialToggleSwitchOffButtonBrush|**_REMOVED_**|OnPrimaryBrush
MaterialToggleSwitchOnBackgroundBrush|**_REMOVED_**|PrimaryVariantLightBrush
MaterialToggleSwitchOnButtonBrush|**_REMOVED_**|PrimaryBrush
MaterialToggleSwitchHeaderMargin|**_REMOVED_**|0,0,12,0

# Updating to Uno.Material v2
Starting with version 2.0.0 of the [Uno.Material](https://www.nuget.org/packages/Uno.Material/2.0.0) and [Uno.Material.WinUI](https://www.nuget.org/packages/Uno.Material.WinUI/2.0.0) packages, users can now take advantage of the new [Material Design 3](https://m3.material.io/) design system from Google.
Along with the new Material Design 3 styles, our Uno.Material NuGet packages will continue to support the previous Material Design 2 styles. In order to achieve this backward compatibility, we have had to make some changes to the way Uno.Material is initialized within your `App.xaml`. 

> [!NOTE]
> In order to avoid any confusion going forward in this article, we will be referring to the new collection of Material Design 3 styles as the "v2" styles and the previous collection of Material Design 2 styles will be referred to as the "v1" styles.

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

### Notable Package Changes

Namespace|Previous Package|New Package|
-|-|-
ControlExtensions.Icon|Uno.Material.Extensions|Uno.Material|
Ripple|Uno.Material.Controls|Uno.Material|