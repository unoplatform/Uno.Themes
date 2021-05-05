using System;
using System.Collections.Generic;
using System.Text;

#if WinUI
using Microsoft.UI.Xaml.Data;
#else
using Windows.UI.Xaml.Data;
#endif

namespace Uno.Cupertino.Converters
{
	public class StringFormatConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, string language)
		{
			var format = parameter as string;
			if (!string.IsNullOrEmpty(format))
				return string.Format(format, value);

			return value;
		}

		public object ConvertBack(object value, Type targetType, object parameter, string language)
		{
			throw new NotImplementedException();
		}
	}
}
