using System;
using System.Collections.Generic;
using System.Text;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Uno.Material.Extensions
{
	public static class ControlExtensions
	{
		#region Property: Icon

		public static DependencyProperty IconProperty { get; } = DependencyProperty.RegisterAttached(
			"Icon",
			typeof(IconElement),
			typeof(ControlExtensions),
			new PropertyMetadata(default));

		public static IconElement GetIcon(Control obj) => (IconElement)obj.GetValue(IconProperty);
		public static void SetIcon(Control obj, IconElement value) => obj.SetValue(IconProperty, value);

		#endregion
	}
}
