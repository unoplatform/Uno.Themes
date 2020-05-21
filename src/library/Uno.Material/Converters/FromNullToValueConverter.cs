using System;
using System.Collections.Generic;
using System.Text;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace Uno.Material.Converters
{
	public class FromNullToValueConverter : IValueConverter
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
