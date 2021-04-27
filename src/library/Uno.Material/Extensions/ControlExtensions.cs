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
