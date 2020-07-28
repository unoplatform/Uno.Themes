using System;
using System.Collections.Generic;
using System.Text;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace Uno.Material.Converters
{
	public class FromEmptyStringOrNullObjectToValueConverter : IValueConverter
	{
		public object EmptyOrNullValue { get; set; }

		public object NotEmptyOrNullValue { get; set; }

		public object Convert(object value, Type targetType, object parameter, string language)
		{
			if (!(value is string str) || string.IsNullOrEmpty(str))
			{
				return EmptyOrNullValue;
			}

			if (value == null || value == DependencyProperty.UnsetValue)
			{
				return EmptyOrNullValue;
			}

			return NotEmptyOrNullValue;
		}

		public object ConvertBack(object value, Type targetType, object parameter, string language)
		{
			throw new NotSupportedException();
		}
	}
}
