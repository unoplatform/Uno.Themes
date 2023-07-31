---
uid: uno.themes.lightweightstyling
---
# Lightweight Styling

[Lightweight styling](https://learn.microsoft.com/windows/apps/design/style/xaml-styles#lightweight-styling) is a way to customize the appearance of XAML controls by **overriding** their default brushes, fonts and numeric properties. Lightweight styles are changed by providing alternate resources with the same key:

```xml
<Button Content="Default Button Style" Style="{StaticResource MaterialFilledButtonStyle}" />

<Button Content="Overridden Button Style" Style="{StaticResource MaterialFilledButtonStyle}" BorderThickness="2">
	<Button.Resources>
		<SolidColorBrush x:Key="FilledButtonForeground" Color="DarkGreen" />
		<SolidColorBrush x:Key="FilledButtonBackground" Color="LightGreen" />
		<SolidColorBrush x:Key="FilledButtonBorderBrush" Color="DarkGreen" />
	</Button.Resources>
</Button>
```

![Material - Button lightweight styling](assets/material-lightweight-styling-anatomy.png)

All interactive controls have multiple states, such as **PointerOver** (mouse is hovered over), **Pressed** (control is pressed on), and **Disabled** (control is not interactable). These states are appended onto the endings of the resource keys: ButtonForeground*PointerOver*, ButtonForeground*Pressed*, and ButtonForeground*Disabled*. Combined with these, the CheckBox and RadioButton controls also have **Checked** and **Unchecked** states. [These links](lightweight-styling#resource-keys) list the resource keys for each control.

```xml
<Button Content="Overridden Button Style" Style="{StaticResource MaterialFilledButtonStyle}" BorderThickness="2">
	<Button.Resources>
		<SolidColorBrush x:Key="FilledButtonForeground" Color="DarkGreen" />
		<SolidColorBrush x:Key="FilledButtonBackground" Color="LightGreen" />
		<SolidColorBrush x:Key="FilledButtonBorderBrush" Color="DarkGreen" />

		<!-- Overriding the PointerOver brushes -->
		<SolidColorBrush x:Key="FilledButtonForegroundPointerOver" Color="DarkRed" />
		<SolidColorBrush x:Key="FilledButtonBackgroundPointerOver" Color="LightPink" />
		<SolidColorBrush x:Key="FilledButtonBorderBrushPointerOver" Color="DarkRed" />
	</Button.Resources>
</Button>
```

![Material - Button lightweight styling](assets/material-button-pointerover-lightweight-styling.png)

## Resource Keys

For more information about the lightweight styling resource keys used in each control, check out the following links:

- [Button](styles/Button.md)
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

https://learn.microsoft.com/windows/apps/design/style/xaml-styles#lightweight-styling
