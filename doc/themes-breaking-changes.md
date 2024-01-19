---
uid: Uno.Themes.BreakingChanges
---

# Overview

The version 5.0.0 of Uno.Themes, introduced breaking changes on Converters, and Control Extensions. If you use one of those items on your project you will need to do some changes in order to keep using it.

## Converter

All Converters were moved to Uno.Themes library, and the new `namespace` is `Uno.Themes`.

Before:

```xaml
<Page xmlns:um="using:Uno.Material">

    <Page.Resources>
        <um:FromNullToValueConverter x:Key="NotNullVisibilityConverter">
    </Page.Resources>
</Page>
```

After:

```xaml
<Page xmlns:ut="using:Uno.Themes">

    <Page.Resources>
        <ut:FromNullToValueConverter x:Key="NotNullVisibilityConverter">
    </Page.Resources>
</Page>
```

## Control Extensions

All Controls Extensions were moved to `Uno.Themes` library, and the new `namespace` is `Uno.Themes`.

Before:

```xaml
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

```xaml
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

## Conclusion

With that you should be able to update and get it working again for you app. If you've any other issue during the migration don't hesitate to open an issue on our [repo](https://github.com/unoplatform/Uno.Themes).
