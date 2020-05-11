using System;
using System.Collections.Generic;
using System.Text;
using Windows.UI.Xaml.Data;

namespace Uno.Material.Converters
{
	public class FromEmptyStringToValueConverter : IValueConverter
	{
		public object NullOrEmptyValue { get; set; }

		public object NotNullOrEmptyValue { get; set; }

		public object Convert(object value, Type targetType, object parameter, string language)
		{
			if (!(value is string str) || string.IsNullOrEmpty(str))
			{
				return NullOrEmptyValue;
			}

			return NotNullOrEmptyValue;
		}

		public object ConvertBack(object value, Type targetType, object parameter, string language)
		{
			throw new NotSupportedException();
		}
	}
}
