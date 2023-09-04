---
uid: uno.themes.lightweightstyling
---
# Lightweight Styling

[Lightweight styling](https://learn.microsoft.com/windows/apps/design/style/xaml-styles#lightweight-styling) is a way to customize the appearance of XAML controls by **overriding** their default brushes, fonts, and numeric properties. Lightweight styles are changed by providing alternate resources with the same key. All Uno Material styles support the capability to be customized through resource overrides without the need to re-define the style.

An example of the anatomy of the lightweight styling resources available for something like Uno Material’s FilledButtonStyle can be seen below. The provided XAML code and image depict a Button using the default FilledButtonStyle followed by a second button, also with FilledButtonStyle applied, but now with specific resource keys overridden to customize its appearance.

```xml
<Button Content="Default Button Style" Style="{StaticResource FilledButtonStyle}" />

<Button Content="Overridden Button Style" Style="{StaticResource FilledButtonStyle}">
	<Button.Resources>
		<Thickness x:Key="ButtonBorderThickness">2</Thickness>
		<SolidColorBrush x:Key="FilledButtonForeground" Color="DarkGreen" />
		<SolidColorBrush x:Key="FilledButtonBackground" Color="LightGreen" />
		<SolidColorBrush x:Key="FilledButtonBorderBrush" Color="DarkGreen" />
	</Button.Resources>
</Button>
```

![Material - Button lightweight styling](assets/material-lightweight-styling-anatomy.png)

Lightweight Styling allows for fine-grained control over the look of your UI components across all visual states. All interactive controls have multiple states, such as **PointerOver** (mouse is hovered over), **Pressed** (control is pressed on), and **Disabled** (control is not interactable). These states are appended onto the endings of the resource keys: ButtonForeground*PointerOver*, ButtonForeground*Pressed*, and ButtonForeground*Disabled*. Combined with these, the CheckBox and RadioButton controls also have **Checked** and **Unchecked** states. This means that it is possible to customize the appearance of your Uno Material styled controls across any visual state without the need to re-define the style. As an example, the XAML below defines three Buttons, all using FilledButtonStyle from Uno Material:

1. A Default Button with no changes
2. A Button with several brush resources overridden for the **Normal** visual state
3. A Button that overrides resources that are used with FilledButtonStyle’s **PointerOver** visual state

```xml
<!-- #1 -->
<Button Content="Default Button Style"
		Style="{StaticResource FilledButtonStyle}" />

<!-- #2 -->
<Button Content="Overridden Button Style"
		Style="{StaticResource FilledButtonStyle}">
	<Button.Resources>
		<SolidColorBrush x:Key="FilledButtonForeground" Color="DarkGreen" />
		<SolidColorBrush x:Key="FilledButtonBackground" Color="LightGreen" />
		<SolidColorBrush x:Key="FilledButtonBorderBrush" Color="DarkGreen" />
	</Button.Resources>
</Button>

<!-- #3 -->
<Button Content="Overridden Button Style (PointerOver)"
		Style="{StaticResource FilledButtonStyle}">
	<Button.Resources>
		<Thickness x:Key="ButtonBorderThickness">2</Thickness>
		<SolidColorBrush x:Key="FilledButtonForegroundPointerOver" Color="DarkRed" />
		<SolidColorBrush x:Key="FilledButtonBackgroundPointerOver" Color="LightPink" />
		<SolidColorBrush x:Key="FilledButtonBorderBrushPointerOver" Color="DarkRed" />
	</Button.Resources>
</Button>
```

With this XAML we are given the following visual result, notice the third Button has a new BorderThickness applied and takes on different colors while in the **PointerOver** state.

![Material - Button lightweight styling](assets/material-button-pointerover-lightweight-styling.png)

## Resource Keys

For more information about the lightweight styling resource keys used in each control, check out the following links:

- [Button](styles/Button.md)
- [CalendarDatePicker](styles/CalendarDatePicker.md)
- [CheckBox](styles/CheckBox.md)
- [ComboBox](styles/ComboBox.md)
- [DatePicker](styles/DatePicker.md)
- [FloatingActionButton](styles/FloatingActionButton.md)
- [HyperlinkButton](styles/HyperlinkButton.md)
- [NavigationView](styles/NavigationView.md)
- [PasswordBox](styles/PasswordBox.md)
- [PipsPager](styles/PipsPager.md)
- [ProgressBar](styles/ProgressBar.md)
- [ProgressRing](styles/ProgressRing.md)
- [RadioButton](styles/RadioButton.md)
- [RatingControl](styles/RatingControl.md)
- [Slider](styles/Slider.md)
- [TextBlock](styles/Typography.md)
- [TextBox](styles/TextBox.md)
- [ToggleButton](styles/ToggleButton.md)
- [ToggleSwitch](styles/ToggleSwitch.md)

## Toolkit

Toolkit also has controls that allow lightweight styling, check out [Lightweight Styling in Uno.Toolkit](xref:Toolkit.LightweightStyling).

### Further Reading

[Lightweight Styling (Windows Dev Docs)](https://learn.microsoft.com/windows/apps/design/style/xaml-styles#lightweight-styling)
