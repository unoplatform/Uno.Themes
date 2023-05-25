using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

#if WinUI
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Data;
#else
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;
#endif

namespace Uno.Material
{
	public class FromNullToFalseConverter : IValueConverter
	{
		public object NullValue { get; set; }

		public object NotNullValue { get; set; }

		public object Convert(object value, Type targetType, object parameter, string language)
		{
			if (value == null || value == DependencyProperty.UnsetValue)
			{
				return NullValue;
			}

			return NotNullValue;
		}

		public object ConvertBack(object value, Type targetType, object parameter, string language)
		{
			throw new NotSupportedException();
		}
	}
}
