---
uid: Uno.Themes.Material.DSP
---

# Using the Material Theme Builder in Uno Material

It is possible to use the [Material Theme Builder](https://m3.material.io/theme-builder#/custom) from Google to generate a custom color palette. The generated palette is provided in the [DSP format](https://m3.material.io/styles/color/the-color-system/color-dsp) and can be used to override the default Uno Material colors. Importing from a DSP file required the [Uno.Dsp.Tasks](https://nuget.org/packages/Uno.Dsp.Tasks) NuGet package to be included in your project.

> [!NOTE]
> Make sure you are referencing the generated XAML file in your `AppResources.xaml`, as shown in the following example:
>
> ```xml
>  <MaterialTheme xmlns="using:Uno.Material"
>                 ColorOverrideSource="ms-appx:///PROJECT_NAME/Styles/Application/MaterialColorsOverride.xaml" />
> ```
>

## The Uno.Dsp.Tasks NuGet package

This package will be automatically present in your project after [creating a new Uno Platform project](https://aka.platform.uno/get-started) and selecting the Material theme either through the [Wizard](xref:Uno.GettingStarted.CreateAnApp.VS2022) or using the [`dotnet new` templates](xref:Uno.GetStarted.dotnet-new
) with the `-theme material` flag.

![Selection of Material theme when creating a project using the Uno Template Wizard](assets/material-theme-selection-wizard.png)

It is also possible to add it manually to an existing Uno Platform project by adding the following PackageReference to your project files:

```xml
<PackageReference Include="Uno.Dsp.Tasks" Version="[latest version]" />
```

## Exporting the DSP file and overriding the colors

1. Navigate to the [Material Theme Builder](https://m3.material.io/theme-builder#/custom) and select the colors you want to use for your application.
2. Locate the _Export_ button and pick the _Material Tokens (DSP)_ format.

   ![Export Button](assets/material-theme-builder-export1.png) ![DSP Selection](assets/material-theme-builder-export2.png)
3. Save the zip file to your computer.
4. Replace the file `ColorPaletteOverride.zip` in the `Styles` folder, if it already exists, with the one you just downloaded.
5. Build your application. The `ColorPaletterOverride.xaml` file will be automatically generated/updated with the colors present in the DSP zip file.

## More flexibility

This will generate the file at each build, potentially overriding any changes you made to the file. If you want to avoid regenerating the XAML file with each build, you can simply remove the `ColorPaletteOverride.zip` file from the `Styles` folder.

Alternatively, you can also use the [Uno.Dsp.Cli](https://nuget.org/packages/Uno.Dsp.Cli) package to generate the file from the command line. This will allow you to generate the file only when you want to, and not at each build.
