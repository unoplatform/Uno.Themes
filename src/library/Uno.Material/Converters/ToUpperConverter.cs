using System;
using System.Collections.Generic;
using System.Text;

#if WinUI
using Microsoft.UI.Xaml.Data;
#else
using Windows.UI.Xaml.Data;
#endif

namespace Uno.Material.Converters
{
	public class ToUpperConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, string language)
		{
			return value is string text ? text.ToUpperInvariant() : value;
		}

		public object ConvertBack(object value, Type targetType, object parameter, string language)
		{
			return null;
		}
	}
}
