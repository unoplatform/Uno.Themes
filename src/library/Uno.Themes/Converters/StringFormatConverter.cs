using System;

#if WinUI
using Microsoft.UI.Xaml.Data;
#else
using Windows.UI.Xaml.Data;
#endif

namespace Uno.Themes;

public class StringFormatConverter : IValueConverter
{
	public object Convert(object value, Type targetType, object parameter, string language)
	{
		var format = parameter as string;
		return !string.IsNullOrEmpty(format) ? string.Format(format, value) : value;
	}

	public object ConvertBack(object value, Type targetType, object parameter, string language)
	{
		throw new NotImplementedException();
	}
}
