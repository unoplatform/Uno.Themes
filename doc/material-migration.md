---
uid: Uno.Themes.Material.Migration
---

# Upgrading Uno Material

## Upgrading to Uno Themes v5

The Uno Material v5 packages introduce a new dependency on the [Uno Themes](https://www.nuget.org/packages/Uno.Themes.WinUI) package. Uno Themes is the base library for all design system implementations going forward. As a result, the following changes have been made:

### Upgrading to Uno Themes V-Next

We have standardized the naming of styles and resource keys. This change aims to simplify resource key usage and align the naming convention across Uno Themes.

#### Changes to Resource Keys

| Old Resource Key                                  | New Resource Key                                |
|---------------------------------------------------|-------------------------------------------------|
| MaterialTextBoxClearGlyphWidth                    | TextBoxClearGlyphWidth                          |
| MaterialTextBoxClearGlyphHeight                   | TextBoxClearGlyphHeight                         |
| MaterialFilledTextBoxCornerRadius                 | FilledTextBoxCornerRadius                       |
| MaterialFilledTextBoxPadding                      | FilledTextBoxPadding                            |
| MaterialFilledTextBoxMinHeight                    | FilledTextBoxMinHeight                          |
| MaterialFilledTextBoxBorderHeightFocused          | FilledTextBoxBorderHeightFocused                |
| MaterialOutlinedTextBoxBorderThickness            | OutlinedTextBoxBorderThickness                  |
| MaterialOutlinedTextBoxCornerRadius               | OutlinedTextBoxCornerRadius                     |
| MaterialOutlinedTextBoxPadding                    | OutlinedTextBoxPadding                          |
| MaterialOutlinedTextBoxMinHeight                  | OutlinedTextBoxMinHeight                        |
| MaterialOutlinedTextBoxBorderThicknessFocused     | OutlinedTextBoxBorderThicknessFocused           |
| MaterialOutlinedTextBoxBorderThicknessPointerOver | OutlinedTextBoxBorderThicknessPointerOver       |
| OutlinedPasswordBoxBorderPadding                  | ***REMOVED***                                   |
| OutlinedPasswordBoxBorderPaddingPointerOver       | ***REMOVED***                                   |
| OutlinedPasswordBoxBorderPaddingFocused           | ***REMOVED***                                   |

### Upgrading to Uno Themes 5.1

#### TextBox `PlaceholderText` and `Header`

Previously, the `PlaceholderText` property acted as a header, displaying in the normal state and animating upwards when the text is set or being entered. And, the `Header` property did nothing. Now, both `PlaceholderText` and `Header` are supported and their behaviors have changed.

##### New Behavior

- **PlaceholderText**: Displays inside the TextBox when it's empty. Disappears when text is entered.
- **Header**: Acts as a label. Animates upwards when text is entered.

##### Example

**Old Usage:**

```xml
<TextBox PlaceholderText="Enter your name" />
```

**New Usage:**

```xml
<TextBox PlaceholderText="Enter your name" Header="Name" />
```

Update your TextBox elements to use `Header` for labels and `PlaceholderText` for placeholders.

### Converters

All Converters were moved to the base `Uno.Themes` library, and the new `namespace` is `Uno.Themes`.

Before:

```xml
<Page xmlns:um="using:Uno.Material">
    <Page.Resources>
        <um:FromNullToValueConverter x:Key="NotNullVisibilityConverter">
    </Page.Resources>
</Page>
```

After:

```xml
<Page xmlns:ut="using:Uno.Themes">
    <Page.Resources>
        <ut:FromNullToValueConverter x:Key="NotNullVisibilityConverter">
    </Page.Resources>
</Page>
```

Some `Converters` have been renamed, you can find the `Old Name` and `New Name` in the table below:

| Old Name | New Name |
| ---- | ---- |
| MaterialTrueToVisible | TrueToVisibleConverter |
| MaterialTrueToCollapsed | TrueToCollapsedConverter |
| CheckBoxIsCheckedToFocusStateCircleVisible | FalseToCollapsedConverter |
| CheckBoxIsCheckedToFocusStateCircleCollapsed | FalseToVisibleConverter |
| MaterialEmptyToFalse/<br>CupertinoEmptyToFalse | EmptyToFalseConverter |
| MaterialEmptyToTrue/<br>CupertinoEmptyToTrue | EmptyToTrueConverter |
| MaterialEmptyToVisible | EmptyToVisibleConverter |
| MaterialEmptyToCollapsed | EmptyToCollapsedConverter |
| MaterialNullToCollapsedConverter/<br>CupertinoNullToCollapsedConverter | NullToCollapsedConverter |
| MaterialNullToVisibleConverter | NullToVisibleConverter |
| MaterialNullToTransparent | NullToTransparentConverter |
| MaterialEmptyOrNullToVisible | EmptyOrNullToVisibleConverter |
| MaterialEmptyOrNullToCollapsed | EmptyOrNullToCollapsedConverter |
| StringFormatConverter | StringFormatConverter |
| FirstCharacterConverter | FirstCharacterConverter |
| ToUpperConverter | ToUpperConverter |

### Control Extensions

All Controls Extensions were moved to the `Uno.Themes` library, and the new namespace is `Uno.Themes`.

Before:

```xml
<Page xmlns:um="using:Uno.Material">
    <StackPanel>
        <Button Content="OUTLINED"
            Style="{StaticResource MaterialOutlinedButtonStyle}">
            <um:ControlExtensions.Icon>
                <FontIcon Glyph="&#xE946;" />
            </um:ControlExtensions.Icon>
        </Button>
    </StackPanel>
</Page>
```

After:

```xml
<Page xmlns:ut="using:Uno.Themes">
    <StackPanel>
        <Button Content="OUTLINED"
            Style="{StaticResource MaterialOutlinedButtonStyle}">
            <ut:ControlExtensions.Icon>
                <FontIcon Glyph="&#xE946;" />
            </ut:ControlExtensions.Icon>
        </Button>
    </StackPanel>
</Page>
```

### Opacity and brushes

The opacity values of certain brushes has been adjusted to better align with the Figma design file:

| Opacity variant | Old Value | New Value |
|-----------------|-----------|-----------|
| Medium          | 0.54      | 0.64      |
| Disabled        | 0.38      | 0.12      |
| DisabledLow     | 0.12      | *removed* |

Existing explicit references to `-DisabledLow` resources should now be updated to use `-Disabled`.

### Resources

Certain Lightweight Styling resource keys have been edited to align with those coming from `Microsoft.UI.Xaml`.

#### RatingControl

| Old Key                                    | New Key                            |
|--------------------------------------------|------------------------------------|
| `RatingControlForegroundUnselected`                     | `RatingControlUnselectedForeground`                     |
| `RatingControlForegroundSelected`                       | `RatingControlSelectedForeground`                       |
| `RatingControlForegroundPointerOverUnselected`          | `RatingControlUnselectedForegroundPointerOver`          |
| `RatingControlForegroundPointerOverSelected`            | `RatingControlSelectedForegroundPointerOver`            |
| `RatingControlForegroundDisabledSelected`               | `RatingControlSelectedForegroundDisabled`               |
| `SecondaryRatingControlForegroundUnselected`            | `SecondaryRatingControlUnselectedForeground`            |
| `SecondaryRatingControlForegroundSelected`              | `SecondaryRatingControlSelectedForeground`              |
| `SecondaryRatingControlForegroundPointerOverUnselected` | `SecondaryRatingControlUnselectedForegroundPointerOver` |
| `SecondaryRatingControlForegroundPointerOverSelected`   | `SecondaryRatingControlSelectedForegroundPointerOver`   |
| `SecondaryRatingControlForegroundDisabledSelected`      | `SecondaryRatingControlSelectedForegroundDisabled`      |

## Upgrading to Uno.Material v3

Uno.Material v3 (not to be confused with [Material Design 3](https://m3.material.io/) from Google) introduces support for [Lightweight Styling](lightweight-styling.md) as well as some breaking changes to the default style keys for some controls. Refer to the tables below for the changes that have been made within Uno.Material.

### Styles

#### Removed

| Key                                 | TargetType     |
|-------------------------------------|----------------|
| `DefaultMaterialCalendarViewStyle`  | `CalendarView` |
| `MaterialSecondaryCheckBoxStyle`    | `CheckBox`     |
| `MaterialSecondaryRadioButtonStyle` | `RadioButton`  |

#### Added

| Key                          | Aliased Key          | TargetType           | Implicit Style |
|------------------------------|----------------------|----------------------|----------------|
| `MaterialRatingControlStyle` | `RatingControlStyle` | `muxc:RatingControl` | true           |
| `MaterialRippleStyle`        | `RippleStyle`        | `um:Ripple`          | true           |

#### Modified

| Key                                 | Aliased Key                 | TargetType             | Implicit Style |
|-------------------------------------|-----------------------------|------------------------|----------------|
| `MaterialToggleMenuFlyoutItemStyle` | `ToggleMenuFlyoutItemStyle` | `ToggleMenuFlyoutItem` | false -> true  |
| `MaterialMenuFlyoutSubItemStyle`    | `MenuFlyoutSubItemStyle`    | `MenuFlyoutSubItem`    | false -> true  |
| `MaterialMenuFlyoutSeparatorStyle`  | `MenuFlyoutSeparatorStyle`  | `MenuFlyoutSeparator`  | false -> true  |
| `MaterialFilledPasswordBoxStyle`    | `FilledPasswordBoxStyle`    | `PasswordBox`          | true -> false  |
| `MaterialOutlinedPasswordBoxStyle`  | `OutlinedPasswordBoxStyle`  | `PasswordBox`          | false -> true  |
| `MaterialFilledTextBoxStyle`        | `FilledTextBoxStyle`        | `TextBox`              | true -> false  |
| `MaterialOutlinedTextBoxStyle`      | `OutlinedTextBoxStyle`      | `TextBox`              | false -> true  |
| `MaterialTextToggleButtonStyle`     | `TextToggleButtonStyle`     | `ToggleButton`         | true -> false  |
| `MaterialIconToggleButtonStyle`     | `IconToggleButtonStyle`     | `ToggleButton`         | false -> true  |

### Resources

As a result of the Lightweight Styling support, many resource keys have been added as well as renamed. For a list of all the new resource keys, please refer to the [Lightweight Styling documentation](lightweight-styling.md#resource-keys).

Along with the above list of new resource keys, below is a list of the resource keys that have been removed or renamed.

> [!NOTE]
> Most resources, including those that have been added or renamed, have now been placed inside of a `ThemeDictionary`. This means that the resources should now be referenced using the `ThemeResource` markup extension instead of `StaticResource`. For more information on theme resources, see [XAML theme resources documentation](https://learn.microsoft.com/windows/apps/design/style/xaml-theme-resources).

#### Button

| Old Key                                    | New Key                            | Value          |
|--------------------------------------------|------------------------------------|----------------|
| `MaterialButtonHorizontalContentAlignment` | `ButtonHorizontalContentAlignment` | `Center`       |
| `MaterialButtonVerticalContentAlignment`   | `ButtonVerticalContentAlignment`   | `Center`       |
| `MaterialButtonCornerRadius`               | ***REMOVED***                      | 20             |
| `MaterialOutlinedButtonBorderThickness`    | ***REMOVED***                      | 1              |
| `MaterialButtonBorderThickness`            | ***REMOVED***                      | 0              |
| `MaterialTextButtonPadding`                | ***REMOVED***                      | 12,0           |
| `MaterialButtonPadding`                    | ***REMOVED***                      | 16,0           |
| `MaterialButtonMinWidth`                   | ***REMOVED***                      | 40             |
| `MaterialButtonMinHeight`                  | ***REMOVED***                      | 40             |
| `MaterialOutlinedButtonBorderBrush`        | ***REMOVED***                      | `OutlineBrush` |
| `MaterialNullToTextButtonMarginConverter`  | `NullToTextButtonMarginConverter`  |                |

#### CalendarDatePicker

| Old Key                                | New Key       | Value               |
|----------------------------------------|---------------|---------------------|
| `MaterialCalendarDatePickerBackground` | ***REMOVED*** | OnSurfaceColor*0.12 |

#### CheckBox

| Old Key                        | New Key       | Value                                                                                                |
|--------------------------------|---------------|------------------------------------------------------------------------------------------------------|
| `CheckAreaCornerRadius`        | ***REMOVED*** | 4                                                                                                    |
| `CheckAreaSize`                | ***REMOVED*** | 18                                                                                                   |
| `FocusAreaSize`                | ***REMOVED*** | 40                                                                                                   |
| `CheckAreaLength`              | ***REMOVED*** | 40                                                                                                   |
| `CheckBoxCheckGlyphPathStyle`  | ***REMOVED*** | `M28.718018,0L32,3.2819897 10.666016,24.616999 0,13.951997 3.2810059,10.670007 10.666016,18.055033z` |
| `CheckBoxHyphenGlyphPathStyle` | ***REMOVED*** | `M0,0L32,0 32,5.3 0,5.3z`                                                                            |

#### ComboBox

| Old Key                                 | New Key                     | Value                 |
|-----------------------------------------|-----------------------------|-----------------------|
| `MaterialComboBoxPadding`               | ***REMOVED***               | 16,0                  |
| `MaterialComboBoxOpenedBorderThickness` | ***REMOVED***               | 2                     |
| `MaterialComboBoxBorderThickness`       | ***REMOVED***               | 1                     |
| `DownArrowPathData`                     | `ComboBoxDownArrowPathData` | `M0 0L5 5L10 0H0Z`    |
| `UpArrowPathData`                       | `ComboBoxUpArrowPathData`   | `M0 0L-5 -5L-10 0H0Z` |
| `MaterialComboBoxCornerRadius`          | ***REMOVED***               | 4                     |

#### DatePicker

| Old Key                                            | New Key       | Value                 |
|----------------------------------------------------|---------------|-----------------------|
| `MaterialDatePickerBackgroundColorBrush`           | ***REMOVED*** | OnSurfaceColor*0.12   |
| `MaterialDatePickerFlyoutPresenterSpacerFill`      | ***REMOVED*** | OnSurfaceFocusedBrush |
| `MaterialDatePickerFlyoutElevation`                | ***REMOVED*** | 8                     |
| `MaterialDatePickerFlyoutPresenterHighlightFill`   | ***REMOVED*** | PrimaryColor*0.2      |
| `MaterialDatePickerFlyoutPresenterBorderBrush`     | ***REMOVED*** | OnSurfaceFocusedBrush |
| `MaterialDatePickerHostPadding`                    | ***REMOVED*** | 24,24,8,8             |
| `MaterialDatePickerFlyoutPresenterBackgroundBrush` | ***REMOVED*** | SurfaceBrush          |
| `MaterialDatePickerHeight`                         | ***REMOVED*** | 53                    |
| `MaterialDatePickerSpacerThemeWidth`               | ***REMOVED*** | 1                     |
| `MaterialDateTimeFlyoutBorderThickness`            | ***REMOVED*** | 1                     |

#### FloatingActionButton

| Old Key                                       | New Key       | Value                              |
|-----------------------------------------------|---------------|------------------------------------|
| `MaterialFabIconTextPadding`                  | ***REMOVED*** | 12                                 |
| `MaterialLargeFabCornerRadius`                | ***REMOVED*** | 28                                 |
| `MaterialLargeFabContentWidthOrHeight`        | ***REMOVED*** | 24                                 |
| `MaterialLargeFabPadding`                     | ***REMOVED*** | 36                                 |
| `MaterialSmallFabCornerRadius`                | ***REMOVED*** | 12                                 |
| `MaterialSmallFabContentWidthOrHeight`        | ***REMOVED*** | 16                                 |
| `MaterialSmallFabPadding`                     | ***REMOVED*** | 12                                 |
| `MaterialFabCornerRadius`                     | ***REMOVED*** | 16                                 |
| `MaterialFabContentWidthOrHeight`             | ***REMOVED*** | 16                                 |
| `MaterialFabPadding`                          | ***REMOVED*** | 20                                 |
| `MaterialTertiaryFabDisabledStateOverlay`     | ***REMOVED*** | `OnSurfaceDisabledLowBrush`        |
| `MaterialTertiaryFabPressedStateOverlay`      | ***REMOVED*** | `OnTertiaryContainerFocusedBrush`  |
| `MaterialTertiaryFabFocusedStateOverlay`      | ***REMOVED*** | `OnTertiaryContainerFocusedBrush`  |
| `MaterialTertiaryFabPointerOverStateOverlay`  | ***REMOVED*** | `OnTertiaryContainerHoverBrush`    |
| `MaterialTertiaryFabDisabledForeground`       | ***REMOVED*** | `OnSurfaceDisabledBrush`           |
| `MaterialTertiaryFabDisabledBackground`       | ***REMOVED*** | `SystemControlTransparentBrush`    |
| `MaterialTertiaryFabBackground`               | ***REMOVED*** | `TertiaryContainerBrush`           |
| `MaterialTertiaryFabForeground`               | ***REMOVED*** | `OnTertiaryContainerBrush`         |
| `MaterialSecondaryFabDisabledStateOverlay`    | ***REMOVED*** | `OnSurfaceDisabledLowBrush`        |
| `MaterialSecondaryFabPressedStateOverlay`     | ***REMOVED*** | `OnSecondaryContainerPressedBrush` |
| `MaterialSecondaryFabFocusedStateOverlay`     | ***REMOVED*** | `OnSecondaryContainerFocusedBrush` |
| `MaterialSecondaryFabPointerOverStateOverlay` | ***REMOVED*** | `OnSecondaryContainerHoverBrush`   |
| `MaterialSecondaryFabDisabledForeground`      | ***REMOVED*** | `OnSurfaceDisabledBrush`           |
| `MaterialSecondaryFabDisabledBackground`      | ***REMOVED*** | `SystemControlTransparentBrush`    |
| `MaterialSecondaryFabBackground`              | ***REMOVED*** | `SecondaryContainerBrush`          |
| `MaterialSecondaryFabForeground`              | ***REMOVED*** | `OnSecondaryContainerBrush`        |
| `MaterialSurfaceFabDisabledStateOverlay`      | ***REMOVED*** | `OnSurfaceDisabledLowBrush`        |
| `MaterialSurfaceFabDisabledBackground`        | ***REMOVED*** | `SystemControlTransparentBrush`    |
| `MaterialSurfaceFabPressedStateOverlay`       | ***REMOVED*** | `PrimaryPressedBrush`              |
| `MaterialSurfaceFabFocusedStateOverlay`       | ***REMOVED*** | `PrimaryFocusedBrush`              |
| `MaterialSurfaceFabPointerOverStateOverlay`   | ***REMOVED*** | `PrimaryHoverBrush`                |
| `MaterialSurfaceFabDisabledForeground`        | ***REMOVED*** | `OnSurfaceDisabledBrush`           |
| `MaterialSurfaceFabBackground`                | ***REMOVED*** | SurfaceBrush                       |
| `MaterialSurfaceFabForeground`                | ***REMOVED*** | PrimaryBrush                       |
| `MaterialFabDisabledStateOverlay`             | ***REMOVED*** | `OnSurfaceDisabledLowBrush`        |
| `MaterialFabPressedStateOverlay`              | ***REMOVED*** | `OnPrimaryContainerPressedBrush`   |
| `MaterialFabFocusedStateOverlay`              | ***REMOVED*** | `OnPrimaryContainerFocusedBrush`   |
| `MaterialFabPointerOverStateOverlay`          | ***REMOVED*** | `OnPrimaryContainerHoverBrush`     |
| `MaterialFabDisabledForeground`               | ***REMOVED*** | `OnSurfaceDisabledBrush`           |
| `MaterialFabDisabledBackground`               | ***REMOVED*** | `SystemControlTransparentBrush`    |
| `MaterialFabBackground`                       | ***REMOVED*** | `PrimaryContainerBrush`            |
| `MaterialFabForeground`                       | ***REMOVED*** | `OnPrimaryContainerBrush`          |

#### NavigationView

| Old Key                                                       | New Key       | Value                       |
|---------------------------------------------------------------|---------------|-----------------------------|
| `NavigationViewItemExpandedGlyph`                             | ***REMOVED*** |                            |
| `NavigationViewItemExpandedGlyphFontSize`                     | ***REMOVED*** | 8                           |
| `NavigationViewItemChildrenMenuFlyoutPadding`                 | ***REMOVED*** | 0,8                         |
| `TopNavigationViewOverflowMenuPadding`                        | ***REMOVED*** | 0,8                         |
| `NavigationViewMinimalContentGridCornerRadius`                | ***REMOVED*** | 0                           |
| `TopNavigationViewContentGridCornerRadius`                    | ***REMOVED*** | 0                           |
| `NavigationViewContentGridCornerRadius`                       | ***REMOVED*** | 8,0,0,0                     |
| `TopNavigationViewItemOnOverflowExpandChevronPadding`         | ***REMOVED*** | 12,0                        |
| `TopNavigationViewItemOnOverflowExpandChevronMargin`          | ***REMOVED*** | -4,0,-8,0                   |
| `TopNavigationViewItemOnOverflowNoIconContentPresenterMargin` | ***REMOVED*** | 16,0,20,0                   |
| `TopNavigationViewItemOnOverflowContentPresenterMargin`       | ***REMOVED*** | 12,0,20,0                   |
| `TopNavigationViewItemContentOnlyExpandChevronMargin`         | ***REMOVED*** | -12,0,0,0                   |
| `TopNavigationViewItemIconOnlyExpandChevronMargin`            | ***REMOVED*** | 0                           |
| `TopNavigationViewItemExpandChevronMargin`                    | ***REMOVED*** | -16,0,0,0                   |
| `NavigationViewItemExpandChevronMargin`                       | ***REMOVED*** | 0,0,-14,0                   |
| `TopNavigationViewItemContentOnlyContentPresenterMargin`      | ***REMOVED*** | 12,0                        |
| `TopNavigationViewItemContentPresenterMargin`                 | ***REMOVED*** | 8,-1,12,-1                  |
| `NavigationViewCompactItemContentPresenterMargin`             | ***REMOVED*** | 0                           |
| `NavigationViewItemContentPresenterMargin`                    | ***REMOVED*** | 4,-1,8,-1                   |
| `TopNavigationViewOverflowButtonMargin`                       | ***REMOVED*** | 0                           |
| `TopNavigationViewItemSeparatorMargin`                        | ***REMOVED*** | 3,0,4,0                     |
| `NavigationViewCompactItemSeparatorMargin`                    | ***REMOVED*** | 0,3,0,4                     |
| `NavigationViewItemSeparatorMargin`                           | ***REMOVED*** | 0,3,0,4                     |
| `TopNavigationViewItemMargin`                                 | ***REMOVED*** | 0                           |
| `NavigationViewItemMargin`                                    | ***REMOVED*** | 0                           |
| `NavigationViewPaneTitlePresenterMargin`                      | ***REMOVED*** | 8,4,0,0                     |
| `TopNavigationViewContentMargin`                              | ***REMOVED*** | 0                           |
| `NavigationViewMinimalContentMargin`                          | ***REMOVED*** | 0                           |
| `NavigationViewContentMargin`                                 | ***REMOVED*** | 0                           |
| `NavigationViewContentPresenterMargin`                        | ***REMOVED*** | 0                           |
| `NavigationViewHeaderMargin`                                  | ***REMOVED*** | 56,44,0,0                   |
| `NavigationViewBorderThickness`                               | ***REMOVED*** | 1                           |
| `TopNavigationViewTopNavGridMargin`                           | ***REMOVED*** | 4,0                         |
| `TopNavigationViewContentGridBorderThickness`                 | ***REMOVED*** | 0,1,0,0                     |
| `NavigationViewMinimalContentGridBorderThickness`             | ***REMOVED*** | 0,1,0,0                     |
| `NavigationViewContentGridBorderThickness`                    | ***REMOVED*** | 1,1,0,0                     |
| `NavigationViewPaneContentGridMargin`                         | ***REMOVED*** | -1,3                        |
| `NavigationViewButtonHolderGridMargin`                        | ***REMOVED*** | 0                           |
| `NavigationViewMinimalHeaderMargin`                           | ***REMOVED*** | -24,44,0,0                  |
| `TopNavigationViewItemInnerHeaderMargin`                      | ***REMOVED*** | 12,0                        |
| `NavigationViewItemInnerHeaderMargin`                         | ***REMOVED*** | 16,0                        |
| `NavigationViewItemButtonMargin`                              | ***REMOVED*** | 12,0                        |
| `NavigationViewItemOnLeftIconBoxMargin`                       | ***REMOVED*** | 3                           |
| `NavigationViewItemBorderThickness`                           | ***REMOVED*** | 1                           |
| `NavigationViewToggleBorderThickness`                         | ***REMOVED*** | 0                           |
| `TopNavigationViewItemSeparatorWidth`                         | ***REMOVED*** | 1                           |
| `NavigationViewItemSeparatorHeight`                           | ***REMOVED*** | 1                           |
| `NavigationViewSelectionIndicatorRadius`                      | ***REMOVED*** | 2                           |
| `NavigationViewSelectionIndicatorHeight`                      | ***REMOVED*** | 24                          |
| `NavigationViewSelectionIndicatorWidth`                       | ***REMOVED*** | 3                           |
| `NavigationViewItemOnLeftIconBoxHeight`                       | ***REMOVED*** | 24                          |
| `NavigationViewPaneHeaderRowMinHeight`                        | ***REMOVED*** | 56                          |
| `NavigationViewItemOnLeftMinHeight`                           | ***REMOVED*** | 56                          |
| `TopNavigationViewSettingsButtonHeight`                       | ***REMOVED*** | 40                          |
| `TopNavigationViewSettingsButtonWidth`                        | ***REMOVED*** | 40                          |
| `TopNavigationViewOverflowButtonHeight`                       | ***REMOVED*** | 40                          |
| `TopNavigationViewOverflowButtonWidth`                        | ***REMOVED*** | 40                          |
| `TopNavigationViewPaneCustomContentMinWidth`                  | ***REMOVED*** | 80                          |
| `NavigationViewAutoSuggestAreaHeight`                         | ***REMOVED*** | 40                          |
| `NavigationViewTopPaneHeight`                                 | ***REMOVED*** | 48                          |
| `NavigationViewIconBoxWidth`                                  | ***REMOVED*** | 40                          |
| `NavigationViewCompactPaneLength`                             | ***REMOVED*** | 80                          |
| `PaneToggleButtonWidth`                                       | ***REMOVED*** | 80                          |
| `PaneToggleButtonHeight`                                      | ***REMOVED*** | 56                          |
| `TopNavigationViewAutoSuggestBoxMargin`                       | ***REMOVED*** | 4,0                         |
| `NavigationViewAutoSuggestBoxMargin`                          | ***REMOVED*** | 16,0                        |
| `MaterialNavigationViewItemRippleFeedback`                    | ***REMOVED*** | `PrimaryPressedBrush`       |
| `MaterialNavigationViewItemBackgroundDisabled`                | ***REMOVED*** | `OnSurfaceDisabledLowBrush` |
| `MaterialNavigationViewItemBackgroundPointerOver`             | ***REMOVED*** | `PrimaryHoverBrush`         |
| `MaterialNavigationViewItemBackgroundPressed`                 | ***REMOVED*** | `PrimaryPressedBrush`       |
| `MaterialNavigationViewItemBackgroundSelected`                | ***REMOVED*** | `PrimarySelectedBrush`      |
| `MaterialNavigationItemContentMarginWithoutIcon`              | ***REMOVED*** | 0                           |
| `MaterialNavigationItemContentMargin`                         | ***REMOVED*** | 12,0,0,0                    |
| `MaterialNavigationItemIconLength`                            | ***REMOVED*** | 24                          |
| `MaterialNavigationItemLeftMargin`                            | ***REMOVED*** | 12,0                        |
| `MaterialNavigationViewItemCornerRadiusLeftOnly`              | ***REMOVED*** | 28,0,0,28                   |
| `MaterialNavigationViewItemCornerRadius`                      | ***REMOVED*** | 28                          |
| `MaterialNavigationItemHeight`                                | ***REMOVED*** | 56                          |
| `MaterialNavigationViewButtonRippleFeedback`                  | ***REMOVED*** | `PrimaryPressedBrush`       |
| `MaterialNavigationViewButtonBackgroundPointerOver`           | ***REMOVED*** | `PrimaryHoverBrush`         |
| `MaterialNavigationViewButtonBackgroundPressed`               | ***REMOVED*** | `PrimaryPressedBrush`       |
| `MaterialNavigationViewButtonForegroundDisabled`              | ***REMOVED*** | `OnSurfaceDisabledLowBrush` |
| `MaterialNavigationViewButtonMarginWhenStackedVertically`     | ***REMOVED*** | 12,0                        |
| `MaterialNavigationViewButtonIconSymbolFontSize`              | ***REMOVED*** | 24                          |
| `MaterialNavigationViewButtonIconLength`                      | ***REMOVED*** | 24                          |
| `MaterialNavigationViewButtonCornerRadius`                    | ***REMOVED*** | 28                          |
| `MaterialNavigationViewButtonWidth`                           | ***REMOVED*** | 56                          |
| `MaterialNavigationViewContentGridCornerRadius`               | ***REMOVED*** | 14,0,0,14                   |
| `MaterialNavigationViewSplitViewCornerRadius`                 | ***REMOVED*** | 0,14,14,0                   |
| `MaterialNavigationViewPaneBackground`                        | ***REMOVED*** | `SurfaceBrush`              |
| `MaterialNavigationViewBackground`                            | ***REMOVED*** | `SurfaceBrush`              |

#### PasswordBox

| Old Key                                          | New Key       | Value                                                                                                                                                                                                                                                                                         |
|--------------------------------------------------|---------------|-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|
| `MaterialDisabledOutlinedPasswordBoxBorderBrush` | ***REMOVED*** | OnSurfaceColor*0.12                                                                                                                                                                                                                                                                           |
| `MaterialRevealGlyphPathData`                    | ***REMOVED*** | `M11 0.5C6 0.5 1.73 3.61 0 8C1.73 12.39 6 15.5 11 15.5C16 15.5 20.27 12.39 22 8C20.27 3.61 16 0.5 11 0.5ZM11 13C8.24 13 6 10.76 6 8C6 5.24 8.24 3 11 3C13.76 3 16 5.24 16 8C16 10.76 13.76 13 11 13ZM11 5C9.34 5 8 6.34 8 8C8 9.66 9.34 11 11 11C12.66 11 14 9.66 14 8C14 6.34 12.66 5 11 5Z` |

#### PipsPager

| Old Key                                                | New Key       | Value                                                                                                                                                                                                                                        |
|--------------------------------------------------------|---------------|----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|
| `MaterialPipsPagerNormalEllipseSize`                   | ***REMOVED*** | 8                                                                                                                                                                                                                                            |
| `MaterialPipsPagerSelectedEllipseSize`                 | ***REMOVED*** | 8                                                                                                                                                                                                                                            |
| `MaterialPipsPagerNavigationVisualStatesEllipseHeight` | ***REMOVED*** | 12                                                                                                                                                                                                                                           |
| `MaterialPipsPagerNavigationVisualStatesEllipseWidth`  | ***REMOVED*** | 12                                                                                                                                                                                                                                           |
| `MaterialPipsPagerNavigationButtonWidth`               | ***REMOVED*** | 40                                                                                                                                                                                                                                           |
| `MaterialPipsPagerNavigationButtonHeight`              | ***REMOVED*** | 40                                                                                                                                                                                                                                           |
| `MaterialPipsPagerNextPageButtonData`                  | ***REMOVED*** | `M4.66319 5.67785C4.36844 6.10738 3.63156 6.10738 3.33681 5.67785L0.103738 0.966444C-0.191015 0.536913 0.177425 6.83871e-07 0.766931 6.32335e-07L7.23307 6.70473e-08C7.82257 1.55111e-08 8.19101 0.536912 7.89626 0.966443L4.66319 5.67785Z` |
| `MaterialPipsPagerPreviousPageButtonData`              | ***REMOVED*** | `M3.33681 0.322148C3.63156 -0.107383 4.36844 -0.107382 4.66319 0.322148L7.89626 5.03356C8.19101 5.46309 7.82257 6 7.23307 6L0.766932 6C0.177427 6 -0.191014 5.46309 0.103739 5.03356L3.33681 0.322148Z`                                      |
| `MaterialPipsPagerNavigationButtonBorderThickness`     | ***REMOVED*** | 0                                                                                                                                                                                                                                            |
| `MaterialPipsPagerButtonBorderThickness`               | ***REMOVED*** | 0                                                                                                                                                                                                                                            |
| `MaterialPipsPagerVerticalOrientationButtonHeight`     | ***REMOVED*** | 12                                                                                                                                                                                                                                           |
| `MaterialPipsPagerVerticalOrientationButtonWidth`      | ***REMOVED*** | 12                                                                                                                                                                                                                                           |
| `MaterialPipsPagerHorizontalOrientationButtonHeight`   | ***REMOVED*** | 12                                                                                                                                                                                                                                           |
| `MaterialPipsPagerHorizontalOrientationButtonWidth`    | ***REMOVED*** | 12                                                                                                                                                                                                                                           |

#### ProgressBar

| Old Key                       | New Key       | Value |
|-------------------------------|---------------|-------|
| `MaterialProgressBarHeight`   | ***REMOVED*** | 4     |
| `MaterialProgressBarMinWidth` | ***REMOVED*** | 250   |

#### ProgressRing

| Old Key                                | New Key       | Value                                                                    |
|----------------------------------------|---------------|--------------------------------------------------------------------------|
| `M3MaterialIndeterminateAnimation_Uno` | ***REMOVED*** | `embedded://Uno.Material/Uno.Material.Assets.MaterialIndeterminate.json` |
| `M3MaterialDeterminateAnimation_Uno`   | ***REMOVED*** | `embedded://Uno.Material/Uno.Material.Assets.MaterialDeterminate.json`   |
| `M3MaterialIndeterminateAnimation_Win` | ***REMOVED*** | `ms-appx:///Uno.Material/Assets/MaterialIndeterminate.json`              |
| `M3MaterialDeterminateAnimation_Win`   | ***REMOVED*** | `ms-appx:///Uno.Material/Assets/MaterialDeterminate.json`                |

#### RadioButton

| Old Key                 | New Key       | Value |
|-------------------------|---------------|-------|
| `RadioBorderThickness`  | ***REMOVED*** | 2     |
| `RadioCheckAreaSize`    | ***REMOVED*** | 20    |
| `RadioStatesAreaSize`   | ***REMOVED*** | 40    |
| `RadioStatesAreaLength` | ***REMOVED*** | 40    |

#### Slider

| Old Key                                 | New Key       | Value                                          |
|-----------------------------------------|---------------|------------------------------------------------|
| `MaterialSliderThumbBackgroundDisabled` | ***REMOVED*** | `SystemControlDisabledChromeDisabledHighBrush` |
| `MaterialSliderInlineTickBarFill`       | ***REMOVED*** | `SystemControlBackgroundAltHighBrush`          |
| `MaterialSliderTickBarFillDisabled`     | ***REMOVED*** | `SystemControlDisabledBaseMediumLowBrush`      |
| `MaterialSliderTickBarFill`             | ***REMOVED*** | `OnSecondaryLowBrush`                          |
| `MaterialSliderTrackBrush`              | ***REMOVED*** | `MaterialSliderTrackColor`                     |

#### TextBox

| Old Key                                      | New Key       | Value                                                                                                                                                                                                                                                                                                  |
|----------------------------------------------|---------------|--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|
| `MaterialDisabledOutlinedTextBoxBorderBrush` | ***REMOVED*** | OnSurfaceColor*0.12                                                                                                                                                                                                                                                                                    |
| `M3ClearGlyphPathData`                       | ***REMOVED*** | `M10 0C4.47 0 0 4.47 0 10C0 15.53 4.47 20 10 20C15.53 20 20 15.53 20 10C20 4.47 15.53 0 10 0ZM10 18C5.59 18 2 14.41 2 10C2 5.59 5.59 2 10 2C14.41 2 18 5.59 18 10C18 14.41 14.41 18 10 18ZM10 8.59L13.59 5L15 6.41L11.41 10L15 13.59L13.59 15L10 11.41L6.41 15L5 13.59L8.59 10L5 6.41L6.41 5L10 8.59Z` |

#### ToggleButton

| Old Key                                    | New Key       | Value            |
|--------------------------------------------|---------------|------------------|
| `MaterialToggleButtonPadding`              | ***REMOVED*** | 16,8             |
| `MaterialToggleButtonBorderRadius`         | ***REMOVED*** | 4                |
| `MaterialToggleButtonForegroundThemeBrush` | ***REMOVED*** | `OnSurfaceBrush` |
| `MaterialToggleButtonTextLabelBrush`       | ***REMOVED*** | `PrimaryColor`   |

#### ToggleSwitch

| Old Key                                    | New Key       | Value                                 |
|--------------------------------------------|---------------|---------------------------------------|
| `MaterialPrimaryVariantLowThumbColorBrush` | ***REMOVED*** | `MaterialPrimaryVariantLowThumbColor` |
| `MaterialToggleSwitchOnLowBackgroundBrush` | ***REMOVED*** | `OnSurfaceSelectedBrush`              |
| `MaterialToggleSwitchOnLowButtonBrush`     | ***REMOVED*** | `OnSurfaceSelectedBrush`              |
| `MaterialToggleSwitchOffBackgroundBrush`   | ***REMOVED*** | `OnSurfaceLowBrush`                   |
| `MaterialToggleSwitchOffButtonBrush`       | ***REMOVED*** | `OnPrimaryBrush`                      |
| `MaterialToggleSwitchOnBackgroundBrush`    | ***REMOVED*** | `PrimaryVariantLightBrush`            |
| `MaterialToggleSwitchOnButtonBrush`        | ***REMOVED*** | `PrimaryBrush`                        |
| `MaterialToggleSwitchHeaderMargin`         | ***REMOVED*** | 0,0,12,0                              |

## Upgrading to Uno.Material v2

Starting with version 2.0.0 of the [Uno.Material](https://www.nuget.org/packages/Uno.Material/2.0.0) and [Uno.Material.WinUI](https://www.nuget.org/packages/Uno.Material.WinUI/2.0.0) packages, users can now take advantage of the new [Material Design 3](https://m3.material.io/) design system from Google.
Along with the new Material Design 3 styles, our Uno.Material NuGet packages will continue to support the previous Material Design 2 styles. In order to achieve this backward compatibility, we have had to make some changes to the way Uno.Material is initialized within your `App.xaml`.

> [!NOTE]
> In order to avoid any confusion going forward in this article, we will be referring to the new collection of Material Design 3 styles as the "v2" styles and the previous collection of Material Design 2 styles will be referred to as the "v1" styles.

 There are two available paths once you have updated to the new Uno.Material v2 package:

- Continue to keep using the v1 styles; or,
- Migrate app to reference the new v2 styles

### Continue Using v1 Styles

> [!WARNING]
> In order to continue using the v1 styles, some changes are required in your `App.xaml`.

The Uno.Material v2 NuGet package contains both sets of v1 and v2 styles. Within your `App.xaml`, you will need to replace the references to `MaterialColors` and `MaterialResources` with `MaterialColorsV1` and `MaterialResourcesV1`. `MaterialFonts` remains unchanged.

### Migrating to v2 Styles

Both `MaterialColors` and `MaterialResources` will now always initialize the latest version of the Material styles. It is also possible to directly reference the `MaterialColorsV2` and `MaterialResourcesV2` ResourceDictionaries if needed.

The v2 styles introduce a new naming system for its resource keys. Refer to the [Material Styles Matrix](xref:Uno.Themes.Material.Styles) for the updated style keys. In addition to the new style keys, the Uno.Material color palette and text resources have also been updated to align with the [Material Design 3 Color System](https://m3.material.io/styles/color/the-color-system/key-colors-tones) and the [Material Design 3 Typography Guidelines](https://m3.material.io/styles/typography/type-scale-tokens).

#### Color Update

A new color palette has been created for v2, meaning any color palette overrides ResourceDictionary in your app must be updated with the new resource keys. An example of the new color palette can be seen in the new [Uno.Material default palette](https://github.com/unoplatform/Uno.Themes/blob/master/src/library/Uno.Themes/Styles/Applications/Common/SharedColorPalette.xaml#L4). For more information on the updated colors, you can refer to the "Colors and Themes" section of the [Material 3 Migration Guide](https://material.io/blog/migrating-material-3).

> [!NOTE]
> As of v2, the Brush resources have been relocated to `ThemeDictionaries`. To reference these Brush resources, use the `ThemeResource` binding.
> More information on theme resources can be found in [XAML theme resources documentation](https://learn.microsoft.com/windows/apps/design/style/xaml-theme-resources).

#### Typography Updates

Text styles have also been modified in v2. There is no 1-to-1 mapping between v1 and v2 text styles in terms of font-sizes and usages. The "Typography" section of the [Material 3 Migration Guide](https://material.io/blog/migrating-material-3) can be helpful for choosing the right style.

Below is a table that is not a strict guideline, but, is provided to you as a suggestion of mapping text styles from v1 to v2:

| Previous Style       | New Style                 |
|----------------------|---------------------------|
| `MaterialHeadline1`  | `MaterialDisplaySmall`    |
| `MaterialHeadline2`  | `MaterialHeadlineLarge`   |
| `MaterialHeadline3`  | `MaterialHeadlineMedium`  |
| `MaterialHeadline4`  | `MaterialHeadlineSmall`   |
| `MaterialHeadline5`  | `MaterialTitleLarge`      |
| `MaterialSubtitle1`  | `MaterialTitleMedium`     |
| `MaterialSubtitle2`  | `MaterialTitleSmall`      |
| `MaterialBody1`      | `MaterialBodyLarge`       |
| `MaterialBody2`      | `MaterialBodyMedium`      |
| `MaterialCaption`    | `MaterialBodySmall`       |
| `MaterialButtonText` | `MaterialLabelLarge`      |
| `MaterialOverline`   | `MaterialLabelMedium`     |
| N/A                  | `MaterialLabelSmall`      |
| N/A                  | `MaterialLabelExtraSmall` |
| N/A                  | `MaterialCaptionLarge`    |
| N/A                  | `MaterialCaptionMedium`   |
| N/A                  | `MaterialCaptionSmall`    |
| N/A                  | `MaterialDisplayLarge`    |
| N/A                  | `MaterialDisplayMedium`   |

#### Notable Style Key Changes

| Control        | Previous Style Key              | New Style Key                   |
|----------------|---------------------------------|---------------------------------|
| `Button`       | `MaterialButtonIconStyle`       | `MaterialIconButtonStyle`       |
| `Button`       | `MaterialContainedButtonStyle`  | `MaterialFilledButtonStyle`     |
| `ToggleButton` | `MaterialToggleButtonIconStyle` | `MaterialIconToggleButtonStyle` |

#### Notable Package Changes

| Namespace                | Previous Package          | New Package    |
|--------------------------|---------------------------|----------------|
| `ControlExtensions.Icon` | `Uno.Material.Extensions` | `Uno.Material` |
| `Ripple`                 | `Uno.Material.Controls`   | `Uno.Material` |
