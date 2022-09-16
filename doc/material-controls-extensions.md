# Material Control Extensions

## Icon
This feature allows for the addition of icon on the supported controls. Those icons could be any of the [`IconElement`](https://docs.microsoft.com/en-us/windows/windows-app-sdk/api/winrt/microsoft.ui.xaml.controls.iconelement)s: `<BitmapIcon />`, `<FontIcon />`, `<PathIcon />` or `<SymbolIcon />`.

Here are supported control with samples:
* TextBox:
  ``` xml
    <TextBox Style="{StaticResource MaterialFilledTextBoxStyle}">
        <um:ControlExtensions.Icon>
            <SymbolIcon Symbol="SolidStar" />
        </um:ControlExtensions.Icon>
    </ComboBox>
  ```
* ComboBox:
  ``` xml
    <ComboBox Style="{StaticResource MaterialComboBoxStyle}">
        <um:ControlExtensions.Icon>
            <SymbolIcon Symbol="SolidStar" />
        </um:ControlExtensions.Icon>
    </ComboBox>
  ```

## Alternate Content
This feature allows putting different content on a control when the state changes.
It's control specific and for now, you can only use it with the ToggleButton control.

### Alternate Content on ToggleButton
``` xml
    <ToggleButton Style="{StaticResource MaterialToggleButtonIconStyle}">
        <!-- This is the default content - which is when the control state is UNCHECKED (the default value of a ToggleButton) -->
        <PathIcon Data="{StaticResource Icon_more_horizontal}" />

        <!-- This is the alternate content - which is when the control state is CHECKED -->
        <um:ControlExtensions.AlternateContent>
            <PathIcon Data="{StaticResource Icon_more_vertical}" />
        </um:ControlExtensions.AlternateContent>
    </ToggleButton>
```