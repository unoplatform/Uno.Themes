using System;
using System.Collections.Generic;
using System.Text;
using Windows.UI.Xaml.Data;

namespace Uno.Cupertino.Converters
{
	public class StringFormatConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, string language)
		{
			var format = parameter as string;
			if (!String.IsNullOrEmpty(format))
				return String.Format(format, value);

			return value;
		}

		public object ConvertBack(object value, Type targetType, object parameter, string language)
		{
			throw new NotImplementedException();
		}
	}
}
