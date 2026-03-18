---
name: simple-installation
description: Installs and configures Uno Simple Theme in Uno Platform applications. Use when creating new Uno Platform projects with Simple Design System styling, adding Simple Theme to existing projects, initializing SimpleTheme resources in App.xaml, or customizing the color palette and fonts. Covers CLI creation, UnoFeatures setup, NuGet package installation, and resource dictionary configuration.
metadata:
  author: unoplatform
  version: "1.0"
  framework: uno-platform
  category: simple-theme-setup
---

# Simple Theme — Installation & Setup

## Overview

Uno Simple is a design system library that applies **Simple Design System (SDS)** styling to Uno Platform applications. It provides a complete set of control styles, a color system with Light and Dark theme support, and typography — all enabled with a few lines of configuration.

The Simple theme is based on the [Figma Simple Design System](https://www.figma.com/community/file/1380235722331273046).

## Creating a New Project with Simple Theme

### Step 1: Install Uno Templates

```bash
dotnet new install Uno.Templates
```

### Step 2: Create the Application

```bash
dotnet new unoapp -o MySimpleApp -theme simpletheme
```

This scaffolds a new Uno Platform app with the Simple theme pre-configured — no additional setup needed.

## Adding Simple Theme to an Existing Project

### Single Project Template (Modern)

#### Step 1: Add UnoFeatures

Edit your project file (`PROJECT_NAME.csproj`) and add `SimpleTheme` to the `<UnoFeatures>` property:

```xml
<UnoFeatures>SimpleTheme</UnoFeatures>
```

If you already have other UnoFeatures, append it with a semicolon:

```xml
<UnoFeatures>Extensions;SimpleTheme</UnoFeatures>
```

#### Step 2: Initialize SimpleTheme in App.xaml

Add the `SimpleTheme` resource dictionary to your `App.xaml` merged dictionaries:

```xml
<Application.Resources>
    <ResourceDictionary>
        <ResourceDictionary.MergedDictionaries>

            <!-- Load WinUI resources -->
            <XamlControlsResources xmlns="using:Microsoft.UI.Xaml.Controls" />

            <!-- Load Simple theme resources -->
            <us:SimpleTheme xmlns:us="using:Uno.Simple" />

        </ResourceDictionary.MergedDictionaries>
    </ResourceDictionary>
</Application.Resources>
```

> **Important:** `SimpleTheme` must appear **after** `XamlControlsResources` so that Simple styles override the default WinUI styles.

### Multi-Head Project Template (Legacy)

> Use this path only for older multi-head templates that do not support `UnoFeatures`.

#### Step 1: Install NuGet Package

Install [`Uno.Simple.WinUI`](https://www.nuget.org/packages/Uno.Simple.WinUI) into your App Code Library project:

```bash
dotnet add package Uno.Simple.WinUI
```

Or via Visual Studio: right-click the project > **Manage NuGet Packages** > search for `Uno.Simple.WinUI` > **Install**.

#### Step 2: Initialize SimpleTheme in AppResources.xaml

```xml
<ResourceDictionary>
    <ResourceDictionary.MergedDictionaries>

        <us:SimpleTheme xmlns:us="using:Uno.Simple" />

    </ResourceDictionary.MergedDictionaries>
</ResourceDictionary>
```

## Customization

### Customize Color Palette

Override the default SDS color palette by providing a `ResourceDictionary` to the `ColorOverrideDictionary` property:

```xml
<us:SimpleTheme xmlns:us="using:Uno.Simple">
    <us:SimpleTheme.ColorOverrideDictionary>
        <ResourceDictionary>
            <ResourceDictionary.ThemeDictionaries>
                <ResourceDictionary x:Key="Light">
                    <!-- Override brand colors for Light theme -->
                    <Color x:Key="SimpleBrand500Color">#4F46E5</Color>
                    <Color x:Key="SimpleBrand600Color">#4338CA</Color>
                </ResourceDictionary>
                <ResourceDictionary x:Key="Default">
                    <!-- Override brand colors for Dark theme -->
                    <Color x:Key="SimpleBrand500Color">#818CF8</Color>
                    <Color x:Key="SimpleBrand600Color">#A5B4FC</Color>
                </ResourceDictionary>
            </ResourceDictionary.ThemeDictionaries>
        </ResourceDictionary>
    </us:SimpleTheme.ColorOverrideDictionary>
</us:SimpleTheme>
```

### Customize Fonts

Override the default fonts (Inter) by providing a `ResourceDictionary` to the `FontOverrideDictionary` property:

```xml
<us:SimpleTheme xmlns:us="using:Uno.Simple">
    <us:SimpleTheme.FontOverrideDictionary>
        <ResourceDictionary>
            <FontFamily x:Key="SimpleFontFamily">ms-appx:///Assets/Fonts/MyCustomFont.ttf#My Custom Font</FontFamily>
            <FontFamily x:Key="SimpleFontFamilyMono">ms-appx:///Assets/Fonts/MyMonoFont.ttf#My Mono Font</FontFamily>
        </ResourceDictionary>
    </us:SimpleTheme.FontOverrideDictionary>
</us:SimpleTheme>
```

### Customize Both Colors and Fonts

Both override dictionaries can be used simultaneously:

```xml
<us:SimpleTheme xmlns:us="using:Uno.Simple">
    <us:SimpleTheme.ColorOverrideDictionary>
        <ResourceDictionary>
            <!-- Color overrides -->
        </ResourceDictionary>
    </us:SimpleTheme.ColorOverrideDictionary>
    <us:SimpleTheme.FontOverrideDictionary>
        <ResourceDictionary>
            <!-- Font overrides -->
        </ResourceDictionary>
    </us:SimpleTheme.FontOverrideDictionary>
</us:SimpleTheme>
```

## Lightweight Styling Overrides

After installing the Simple theme, you can further customize individual controls without redefining styles. Override resource keys at the **App**, **Page**, or **Control** level:

### App-Level Override

Affects every instance of the control across the entire application:

```xml
<!-- In App.xaml, after SimpleTheme -->
<ResourceDictionary>
    <ResourceDictionary.ThemeDictionaries>
        <ResourceDictionary x:Key="Light">
            <SolidColorBrush x:Key="SimplePrimaryButtonBackground" Color="DarkGreen" />
        </ResourceDictionary>
        <ResourceDictionary x:Key="Default">
            <SolidColorBrush x:Key="SimplePrimaryButtonBackground" Color="LightGreen" />
        </ResourceDictionary>
    </ResourceDictionary.ThemeDictionaries>
</ResourceDictionary>
```

### Page-Level Override

Scoped to a single page:

```xml
<Page.Resources>
    <SolidColorBrush x:Key="SimplePrimaryButtonBackground" Color="DarkBlue" />
</Page.Resources>
```

### Control-Level Override

Scoped to a single control instance:

```xml
<Button Content="Custom">
    <Button.Resources>
        <SolidColorBrush x:Key="SimplePrimaryButtonBackground" Color="Purple" />
    </Button.Resources>
</Button>
```

## What Gets Installed

When the Simple theme is active, **implicit default styles** are automatically applied to these controls — no explicit `Style` attribute needed:

| Control | Default Implicit Style |
|---------|----------------------|
| Button | `SimplePrimaryButtonStyle` |
| CheckBox | `SimpleCheckBoxStyle` |
| ComboBox | `SimpleSelectFieldStyle` |
| RadioButton | `SimpleRadioButtonStyle` |
| TextBox | `SimpleInputTextBoxStyle` |
| ToggleSwitch | `SimpleToggleSwitchStyle` |
| PasswordBox | `SimplePasswordBoxStyle` |
| Slider | `SimpleSliderStyle` |
| ToolTip | `SimpleToolTipStyle` |
| TextBlock | `SimpleBaseTextBlockStyle` |
| ListView / ListViewItem | `SimpleListViewStyle` / `SimpleListViewItemStyle` |
| ContentDialog | `SimpleContentDialogStyle` |
| MenuFlyoutItem | `SimpleMenuFlyoutItemStyle` |
| MenuFlyoutPresenter | `SimpleMenuFlyoutPresenterStyle` |
| MenuFlyoutSeparator | `SimpleMenuFlyoutSeparatorStyle` |
| MenuFlyoutSubItem | `SimpleMenuFlyoutSubItemStyle` |
| ToggleMenuFlyoutItem | `SimpleToggleMenuFlyoutItemStyle` |
| CalendarView | `SimpleCalendarViewStyle` |
| AppBarButton | `SimpleAppBarButtonStyle` |
| CalendarDatePicker | `SimpleCalendarDatePickerStyle` |
| DatePicker / DatePickerFlyoutPresenter | `SimpleDatePickerStyle` / `SimpleDatePickerFlyoutPresenterStyle` |
| AutoSuggestBox | `SimpleAutoSuggestBoxStyle` |
| Expander | `SimpleExpanderStyle` |
| ToggleButton | `SimpleDefaultToggleButtonStyle` |

## Verification

After installation, build and run your app. All supported controls should render with Simple theme styling automatically. If controls appear unstyled:

1. Verify `SimpleTheme` is in the merged dictionaries and appears **after** `XamlControlsResources`
2. Verify `<UnoFeatures>SimpleTheme</UnoFeatures>` is in your `.csproj`
3. Clean and rebuild the solution

## Related Skills

- [simple-skills-index](../simple-skills-index/SKILL.md) — Full index of all Simple control skills
- [simple-button](../simple-button/SKILL.md) — Button styles and lightweight styling
- [simple-textblock](../simple-textblock/SKILL.md) — Typography styles
