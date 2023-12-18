---
uid: Uno.Themes.Material.Extensions
---

# Material Control Extensions

> [!NOTE]
> Uno Material also has support for C# Markup through a [Uno.Material.WinUI.Markup](https://www.nuget.org/packages/Uno.Material.WinUI.Markup) NuGet Package. To get started with Uno Material in your C# Markup application, add the `Uno.Material.WinUI.Markup` NuGet package to your **App Code Library** project and your platform heads.

## Icon

This feature allows for the addition of icon on the supported controls. Those icons could be any of the [`IconElement`](https://docs.microsoft.com/en-us/windows/windows-app-sdk/api/winrt/microsoft.ui.xaml.controls.iconelement)s: `BitmapIcon`, `FontIcon`, `PathIcon` or `SymbolIcon`.

Here are supported control with samples:

* TextBox:

# [**XAML**](#tab/xaml)
    ```xml
    <TextBox Style="{StaticResource MaterialFilledTextBoxStyle}">
        <um:ControlExtensions.Icon>
            <SymbolIcon Symbol="SolidStar" />
        </um:ControlExtensions.Icon>
    </ComboBox>
    ```

# [**C#**](#tab/csharp)
    ```csharp
    new TextBox()
        .Style(Theme.TextBox.Styles.Filled)
        .ControlExtensions
        (
            icon:
                new SymbolIcon()
                    .Symbol(Symbol.SolidStar)
        )
    ```

***

* ComboBox:

# [**XAML**](#tab/xaml)
    ```xml
    <ComboBox Style="{StaticResource MaterialComboBoxStyle}">
        <um:ControlExtensions.Icon>
            <SymbolIcon Symbol="SolidStar" />
        </um:ControlExtensions.Icon>
    </ComboBox>
    ```

# [**C#**](#tab/csharp)
    ```csharp
    new ComboBox()
        .Style(Theme.ComboBox.Styles.Default)
        .ControlExtensions
        (
            icon:
                new SymbolIcon()
                    .Symbol(Symbol.SolidStar)
        )
    ```
***

## Alternate Content

This feature allows putting different content on a control when the state changes.
It's control specific and for now, you can only use it with the ToggleButton control.

### Alternate Content on ToggleButton

# [**XAML**](#tab/xaml)
    ```xml
    <ToggleButton Style="{StaticResource MaterialToggleButtonIconStyle}">
        <!-- This is the default content - which is when the control state is UNCHECKED (the default value of a ToggleButton) -->
        <PathIcon Data="{StaticResource Icon_more_horizontal}" />

        <!-- This is the alternate content - which is when the control state is CHECKED -->
        <um:ControlExtensions.AlternateContent>
            <PathIcon Data="{StaticResource Icon_more_vertical}" />
        </um:ControlExtensions.AlternateContent>
    </ToggleButton>
    ```

# [**C#**](#tab/csharp)
    ```csharp
    new ToggleButton()
        .Style(Theme.ToggleButton.Styles.Icon)
        // This is the default content - which is when the control state is UNCHECKED (the default value of a ToggleButton)
        .Content
        (
            new PathIcon()
                .Data(StaticResource.Get<Geometry>("Icon_more_horizontal"))
        )
        // This is the alternate content - which is when the control state is CHECKED
        .ControlExtensions
        (
            alternateContent:
                new PathIcon()
                    .Data(StaticResource.Get<Geometry>("Icon_more_vertical"))
        )
    ```
***

## Elevation

This feature allows to set the level of elevation to depict on the supported control.

Setting the elevation on [supported controls](#supported-controls) can result in changes to both the shadow and the [surface tint](#surface-tint).

[Material Design Elevation Guidance](https://m3.material.io/styles/elevation/overview)

## Surface Tint

The surface tint properties allow for customization of how elevation can be depicted on certain controls. While the Background of a control remains static, the surface color can change based on the level of elevation.

### TintedBackground

This is a readonly property that will provide a SolidColorBrush depicting the control's current background color overlayed with the surface tint color at a certain opacity based on the elevation of the control.

### IsTintEnabled

This feature allows for enabling or disabling the surface tint that may be applied to an elevated control. When `IsTintEnabled` is `false`, the `TintedBackground` property will remain the same value as the control's background color.

### Example Usage for `Button`

The `ElevatedButtonStyle` in Uno Material supports elevation and surface tints through the use of the `TintedBackground`, `IsTintEnabled`, and `Elevation` attached properties.

`ElevatedButtonStyle` contains the following `Setter`s:

```xml
...

<Setter Property="um:ControlExtensions.Elevation"
        Value="1" />
<Setter Property="um:ControlExtensions.IsTintEnabled"
        Value="True" />
...
```

Within the `ControlTemplate` of the `ElevatedButtonStyle`, instead of performing a `TemplateBinding` to the `Background` property of the `Button`, we instead bind to the `TintedBackground` attached property:

```xml
<Grid x:Name="Root"
      ...
      Background="{Binding Path=(um:ControlExtensions.TintedBackground), RelativeSource={RelativeSource TemplatedParent}}">
      <!-- Remaining content omitted for brevity -->
</Grid>
```

Applying the surface tint for elevated controls is optional and must be explicitly enabled through the use of the `IsTintEnabled` attached property. Below is an example of how an elevated control may appear  with or without a surface tint:

# [**XAML**](#tab/xaml)
```xml
<StackPanel Spacing="8">
    <Button Content="Elevation 0"
            um:ControlExtensions.Elevation="0"
            Style="{StaticResource MaterialElevatedButtonStyle}" />
    <Button Content="Elevation 1"
            um:ControlExtensions.Elevation="1"
            Style="{StaticResource MaterialElevatedButtonStyle}" />
    <Button Content="Elevation 2"
            um:ControlExtensions.Elevation="2"
            Style="{StaticResource MaterialElevatedButtonStyle}" />
    <Button Content="Elevation 3"
            um:ControlExtensions.Elevation="3"
            Style="{StaticResource MaterialElevatedButtonStyle}" />
    <Button Content="Elevation 4"
            um:ControlExtensions.Elevation="4"
            Style="{StaticResource MaterialElevatedButtonStyle}" />
    <Button Content="Elevation 5"
            um:ControlExtensions.Elevation="5"
            Style="{StaticResource MaterialElevatedButtonStyle}" />
</StackPanel> 
```

# [**C#**](#tab/csharp)
```csharp
new StackPanel()
    .Spacing(8)
    .Children(
        new Button()
            .Content("Elevation 0")
            .ControlExtensions(elevation: 0)
            .Style(Theme.Button.Styles.Elevated),
        new Button()
            .Content("Elevation 1")
            .ControlExtensions(elevation: 1)
            .Style(Theme.Button.Styles.Elevated),
        new Button()
            .Content("Elevation 2")
            .ControlExtensions(elevation: 2)
            .Style(Theme.Button.Styles.Elevated),
        new Button()
            .Content("Elevation 3")
            .ControlExtensions(elevation: 3)
            .Style(Theme.Button.Styles.Elevated),
        new Button()
            .Content("Elevation 4")
            .ControlExtensions(elevation: 4)
            .Style(Theme.Button.Styles.Elevated),
        new Button()
            .Content("Elevation 5")
            .ControlExtensions(elevation: 5)
            .Style(Theme.Button.Styles.Elevated)
    )
```

***

The above code will produce the following result:

![Uno Material Elevation Buttons with Tint Enabled](assets/material-elevation-buttons.png)

If we were to alter the code above and set `IsTintEnabled` to `False` (XAML: `um:ControlExtensions.IsTintEnabled="False"` / C#: `ControlExtensions(isTintEnabled: false)`) on each of the buttons, we would see elevated buttons without tints:

![Uno Material Elevation Buttons with Tint Disabled](assets/material-elevation-buttons-shadow-only.png)

### Supported Controls

The following control styles have support for surface tint:

| Control | Supporting Styles   |
|---------|---------------------|
| Button  | ElevatedButtonStyle |
