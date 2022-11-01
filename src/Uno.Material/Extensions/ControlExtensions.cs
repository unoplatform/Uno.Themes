using System;
using System.Collections.Generic;
using System.Text;

#if WinUI
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Controls;
#else
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Controls;
#endif

namespace Uno.Material
{
	[Bindable]
	public static class ControlExtensions
	{
#region DependencyProperty: Icon

		public static DependencyProperty IconProperty { get; } = DependencyProperty.RegisterAttached(
			"Icon",
			typeof(IconElement),
			typeof(ControlExtensions),
			new PropertyMetadata(default));

		public static IconElement GetIcon(Control obj) => (IconElement)obj.GetValue(IconProperty);
		public static void SetIcon(Control obj, IconElement value) => obj.SetValue(IconProperty, value);
#endregion

#region DependencyProperty: IconHeight
		public static DependencyProperty IconHeightProperty { get; } = DependencyProperty.RegisterAttached(
			"IconHeight",
			typeof(double),
			typeof(ControlExtensions),
			new PropertyMetadata(Double.NaN));

		public static double GetIconHeight(Control obj) => (double)obj.GetValue(IconHeightProperty);
		public static void SetIconHeight(Control obj, double value) => obj.SetValue(IconHeightProperty, value);
#endregion

#region DependencyProperty: IconWidth 
		public static DependencyProperty IconWidthProperty { get; } = DependencyProperty.RegisterAttached(
			"IconWidth",
			typeof(double),
			typeof(ControlExtensions),
			new PropertyMetadata(Double.NaN));

		public static double GetIconWidth(Control obj) => (double)obj.GetValue(IconWidthProperty);
		public static void SetIconWidth(Control obj, double value) => obj.SetValue(IconWidthProperty, value);

#endregion

#region DependencyProperty: AlternateContent

		public static DependencyProperty AlternateContentProperty { get; } = DependencyProperty.RegisterAttached(
			"AlternateContent",
			typeof(object),
			typeof(ControlExtensions),
			new PropertyMetadata(default(object)));

		public static object GetAlternateContent(Control obj) => (object)obj.GetValue(AlternateContentProperty);
		public static void SetAlternateContent(Control obj, object value) => obj.SetValue(AlternateContentProperty, value);

#endregion
	}
}
