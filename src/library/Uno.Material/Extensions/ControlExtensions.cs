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

namespace Uno.Material.Extensions
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
