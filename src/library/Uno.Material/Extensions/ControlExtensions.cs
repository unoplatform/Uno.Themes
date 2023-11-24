using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics.CodeAnalysis;
using Windows.UI;
using Uno.Disposables;
using System.Windows.Input;

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

namespace Uno.Material
{
	[Bindable]
	public static class ControlExtensions
	{
		#region DependencyProperty: Icon

		public static DependencyProperty IconProperty { [DynamicDependency(nameof(GetIcon))] get; } = DependencyProperty.RegisterAttached(
			"Icon",
			typeof(IconElement),
			typeof(ControlExtensions),
			new PropertyMetadata(default, OnIconChanged));

		[DynamicDependency(nameof(SetIcon))]
		public static IconElement GetIcon(Control obj) => (IconElement)obj.GetValue(IconProperty);

		[DynamicDependency(nameof(GetIcon))]
		public static void SetIcon(Control obj, IconElement value) => obj.SetValue(IconProperty, value);
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

		#region DependencyProperty: LeadingIcon

		public static DependencyProperty LeadingIconProperty { [DynamicDependency(nameof(GetLeadingIcon))] get; } = DependencyProperty.RegisterAttached(
			"LeadingIcon",
			typeof(IconElement),
			typeof(ControlExtensions),
			new PropertyMetadata(default));

		[DynamicDependency(nameof(SetLeadingIcon))]
		public static IconElement GetLeadingIcon(Control obj) => (IconElement)obj.GetValue(LeadingIconProperty);

		[DynamicDependency(nameof(GetLeadingIcon))]
		public static void SetLeadingIcon(Control obj, IconElement value) => obj.SetValue(LeadingIconProperty, value);

		#endregion

		#region DependencyProperty: IsLeadingIconVisible

		public static DependencyProperty IsLeadingIconVisibleProperty { [DynamicDependency(nameof(GetIsLeadingIconVisible))] get; } = DependencyProperty.RegisterAttached(
			"IsLeadingIconVisible",
			typeof(bool),
			typeof(ControlExtensions),
			new PropertyMetadata(true));

		[DynamicDependency(nameof(SetIsLeadingIconVisible))]
		public static bool GetIsLeadingIconVisible(Control obj) => (bool)obj.GetValue(IsLeadingIconVisibleProperty);

		[DynamicDependency(nameof(GetIsLeadingIconVisible))]
		public static void SetIsLeadingIconVisible(Control obj, bool value) => obj.SetValue(IsLeadingIconVisibleProperty, value);

		#endregion

		#region DependencyProperty: LeadingCommand
		public static DependencyProperty LeadingCommandProperty { [DynamicDependency(nameof(GetLeadingCommand))] get; } = DependencyProperty.RegisterAttached(
			"LeadingCommand",
			typeof(ICommand),
			typeof(ControlExtensions),
			new PropertyMetadata(default));

		[DynamicDependency(nameof(GetLeadingCommand))]
		public static ICommand GetLeadingCommand(Control obj) => (ICommand)obj.GetValue(LeadingCommandProperty);

		[DynamicDependency(nameof(SetLeadingCommand))]
		public static void SetLeadingCommand(Control obj, ICommand value) => obj.SetValue(LeadingCommandProperty, value);
		#endregion

		#region DependencyProperty: TrailingIcon

		public static DependencyProperty TrailingIconProperty { [DynamicDependency(nameof(GetTrailingIcon))] get; } = DependencyProperty.RegisterAttached(
			"TrailingIcon",
			typeof(IconElement),
			typeof(ControlExtensions),
			new PropertyMetadata(default));

		[DynamicDependency(nameof(SetTrailingIcon))]
		public static IconElement GetTrailingIcon(Control obj) => (IconElement)obj.GetValue(TrailingIconProperty);

		[DynamicDependency(nameof(GetTrailingIcon))]
		public static void SetTrailingIcon(Control obj, IconElement value) => obj.SetValue(TrailingIconProperty, value);
		#endregion

		#region DependencyProperty: IsTrailingIconVisible

		public static DependencyProperty IsTrailingIconVisibleProperty { [DynamicDependency(nameof(GetIsTrailingIconVisible))] get; } = DependencyProperty.RegisterAttached(
			"IsTrailingIconVisible",
			typeof(bool),
			typeof(ControlExtensions),
			new PropertyMetadata(true));

		[DynamicDependency(nameof(SetIsTrailingIconVisible))]
		public static bool GetIsTrailingIconVisible(Control obj) => (bool)obj.GetValue(IsTrailingIconVisibleProperty);

		[DynamicDependency(nameof(GetIsTrailingIconVisible))]
		public static void SetIsTrailingIconVisible(Control obj, bool value) => obj.SetValue(IsTrailingIconVisibleProperty, value);

		#endregion

		#region DependencyProperty: TrailingCommand
		public static DependencyProperty TrailingCommandProperty { [DynamicDependency(nameof(GetTrailingCommand))] get; } = DependencyProperty.RegisterAttached(
			"TrailingCommand",
			typeof(ICommand),
			typeof(ControlExtensions),
			new PropertyMetadata(default));

		[DynamicDependency(nameof(GetTrailingCommand))]
		public static ICommand GetTrailingCommand(Control obj) => (ICommand)obj.GetValue(TrailingCommandProperty);

		[DynamicDependency(nameof(SetTrailingCommand))]
		public static void SetTrailingCommand(Control obj, ICommand value) => obj.SetValue(TrailingCommandProperty, value);
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
		/// Gets or sets the level of elevation to depict for the attached view. In Material, elevation can be used to drive both the shadow and the surface tint color (https://m3.material.io/styles/elevation/overview)
		/// </summary>
		public static DependencyProperty ElevationProperty { [DynamicDependency(nameof(ElevationProperty))] get; } = DependencyProperty.RegisterAttached(
			"Elevation",
			typeof(int),
			typeof(ControlExtensions),
			new PropertyMetadata(0, OnElevationChanged));

		[DynamicDependency(nameof(SetElevation))]
		public static int GetElevation(Control obj) => (int)obj.GetValue(ElevationProperty);
		[DynamicDependency(nameof(GetElevation))]
		public static void SetElevation(Control obj, int value) => obj.SetValue(ElevationProperty, value);

		#endregion

		#region DependencyProperty: TintedBackground
		/// <summary>
		/// Gets or sets the SolidColorBrush to use when depicting a surface tint on an elevated view.
		/// </summary>
		public static DependencyProperty TintedBackgroundProperty { [DynamicDependency(nameof(GetTintedBackground))] get; } = DependencyProperty.RegisterAttached(
			"TintedBackground",
			typeof(SolidColorBrush),
			typeof(ControlExtensions),
			new PropertyMetadata(default(SolidColorBrush)));

		[DynamicDependency(nameof(SetTintedBackground))]
		public static SolidColorBrush GetTintedBackground(UIElement obj) => (SolidColorBrush)obj.GetValue(TintedBackgroundProperty);
		[DynamicDependency(nameof(GetTintedBackground))]
		public static void SetTintedBackground(UIElement obj, SolidColorBrush value) => obj.SetValue(TintedBackgroundProperty, value);

		#endregion
		#region DependencyProperty: IsTintEnabled
		/// <summary>
		/// Gets or sets whether or not the SurfaceTintColor should be applied for elevated views
		/// </summary>
		public static DependencyProperty IsTintEnabledProperty { [DynamicDependency(nameof(GetIsTintEnabled))] get; } = DependencyProperty.RegisterAttached(
			"IsTintEnabled",
			typeof(bool),
			typeof(ControlExtensions),
			new PropertyMetadata(false, OnIsTintEnabledChanged));

		[DynamicDependency(nameof(SetIsTintEnabled))]
		public static bool GetIsTintEnabled(UIElement obj) => (bool)obj.GetValue(IsTintEnabledProperty);
		[DynamicDependency(nameof(GetIsTintEnabled))]
		public static void SetIsTintEnabled(UIElement obj, bool value) => obj.SetValue(IsTintEnabledProperty, value);

		#endregion

		private static void OnIconChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			if (d is TextBox textBox)
			{
				textBox.SetValue(LeadingIconProperty, e.NewValue);
			}
		}

		private static void OnElevationChanged(DependencyObject element, DependencyPropertyChangedEventArgs e)
			=> SurfaceTintExtensions.OnElevationChanged(element, (int)e.NewValue);

		private static void OnIsTintEnabledChanged(DependencyObject element, DependencyPropertyChangedEventArgs e)
			=> SurfaceTintExtensions.OnIsTintEnabledChanged(element, (bool)e.NewValue);
	}
}
