using System;
using System.Diagnostics.CodeAnalysis;

#if WinUI
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Controls;
#else
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Controls;
#endif

namespace Uno.Themes;

[Bindable]
public static class ControlExtensions
{
	#region DependencyProperty: Icon

	/// <summary>
	/// Gets or sets the icon for the control. This property is forwarded to <see cref="LeadingIconProperty"/> for backward compatibility.
	/// </summary>
	public static DependencyProperty IconProperty { [DynamicDependency(nameof(GetIcon))] get; } = DependencyProperty.RegisterAttached(
		"Icon",
		typeof(IconElement),
		typeof(ControlExtensions),
		new PropertyMetadata(default(IconElement), OnIconChanged));

	[DynamicDependency(nameof(SetIcon))]
	public static IconElement GetIcon(Control obj) => (IconElement)obj.GetValue(IconProperty);

	[DynamicDependency(nameof(GetIcon))]
	public static void SetIcon(Control obj, IconElement value) => obj.SetValue(IconProperty, value);

	private static void OnIconChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
	{
		if (d is Control control)
		{
			control.SetValue(LeadingIconProperty, e.NewValue);
		}
	}
	#endregion

	#region DependencyProperty: LeadingIcon

	/// <summary>
	/// Gets or sets the leading (start) icon for the control.
	/// </summary>
	public static DependencyProperty LeadingIconProperty { [DynamicDependency(nameof(GetLeadingIcon))] get; } = DependencyProperty.RegisterAttached(
		"LeadingIcon",
		typeof(IconElement),
		typeof(ControlExtensions),
		new PropertyMetadata(default(IconElement)));

	[DynamicDependency(nameof(SetLeadingIcon))]
	public static IconElement GetLeadingIcon(Control obj) => (IconElement)obj.GetValue(LeadingIconProperty);

	[DynamicDependency(nameof(GetLeadingIcon))]
	public static void SetLeadingIcon(Control obj, IconElement value) => obj.SetValue(LeadingIconProperty, value);
	#endregion

	#region DependencyProperty: TrailingIcon

	/// <summary>
	/// Gets or sets the trailing (end) icon for the control.
	/// </summary>
	public static DependencyProperty TrailingIconProperty { [DynamicDependency(nameof(GetTrailingIcon))] get; } = DependencyProperty.RegisterAttached(
		"TrailingIcon",
		typeof(IconElement),
		typeof(ControlExtensions),
		new PropertyMetadata(default(IconElement)));

	[DynamicDependency(nameof(SetTrailingIcon))]
	public static IconElement GetTrailingIcon(Control obj) => (IconElement)obj.GetValue(TrailingIconProperty);

	[DynamicDependency(nameof(GetTrailingIcon))]
	public static void SetTrailingIcon(Control obj, IconElement value) => obj.SetValue(TrailingIconProperty, value);
	#endregion

	#region DependencyProperty: IconHeight
	public static DependencyProperty IconHeightProperty { [DynamicDependency(nameof(GetIconHeight))] get; } = DependencyProperty.RegisterAttached(
		"IconHeight",
		typeof(double),
		typeof(ControlExtensions),
		new PropertyMetadata(Double.NaN));

	[DynamicDependency(nameof(SetIconHeight))]
	public static double GetIconHeight(Control obj) => (double)obj.GetValue(IconHeightProperty);
	[DynamicDependency(nameof(GetIconHeight))]
	public static void SetIconHeight(Control obj, double value) => obj.SetValue(IconHeightProperty, value);
	#endregion

	#region DependencyProperty: IconWidth 
	public static DependencyProperty IconWidthProperty { [DynamicDependency(nameof(GetIconWidth))] get; } = DependencyProperty.RegisterAttached(
		"IconWidth",
		typeof(double),
		typeof(ControlExtensions),
		new PropertyMetadata(Double.NaN));

	[DynamicDependency(nameof(SetIconWidth))]
	public static double GetIconWidth(Control obj) => (double)obj.GetValue(IconWidthProperty);
	[DynamicDependency(nameof(GetIconWidth))]
	public static void SetIconWidth(Control obj, double value) => obj.SetValue(IconWidthProperty, value);

	#endregion

	#region DependencyProperty: AlternateContent

	public static DependencyProperty AlternateContentProperty { [DynamicDependency(nameof(GetAlternateContent))] get; } = DependencyProperty.RegisterAttached(
		"AlternateContent",
		typeof(object),
		typeof(ControlExtensions),
		new PropertyMetadata(default(object)));

	[DynamicDependency(nameof(SetAlternateContent))]
	public static object GetAlternateContent(Control obj) => (object)obj.GetValue(AlternateContentProperty);
	[DynamicDependency(nameof(GetAlternateContent))]
	public static void SetAlternateContent(Control obj, object value) => obj.SetValue(AlternateContentProperty, value);

	#endregion

	#region DependencyProperty: Elevation

	/// <summary>
	/// Gets or sets the level of elevation to depict for the attached view.
	/// </summary>
	public static DependencyProperty ElevationProperty { [DynamicDependency(nameof(ElevationProperty))] get; } = DependencyProperty.RegisterAttached(
		"Elevation",
		typeof(int),
		typeof(ControlExtensions),
		new PropertyMetadata(0));

	[DynamicDependency(nameof(SetElevation))]
	public static int GetElevation(Control obj) => (int)obj.GetValue(ElevationProperty);
	[DynamicDependency(nameof(GetElevation))]
	public static void SetElevation(Control obj, int value) => obj.SetValue(ElevationProperty, value);

	#endregion
}
