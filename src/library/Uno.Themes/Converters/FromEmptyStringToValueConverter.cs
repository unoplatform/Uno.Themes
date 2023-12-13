using System;

#if WinUI
using Microsoft.UI.Xaml.Data;
#else
using Windows.UI.Xaml.Data;
#endif

namespace Uno.Themes;

public class FromEmptyStringToValueConverter : IValueConverter
{
	public object NullOrEmptyValue { get; set; }

	public object NotNullOrEmptyValue { get; set; }

	public object Convert(object value, Type targetType, object parameter, string language)
	{
		return value is not string str || string.IsNullOrEmpty(str) ? NullOrEmptyValue : NotNullOrEmptyValue;
	}

	public object ConvertBack(object value, Type targetType, object parameter, string language)
	{
		throw new NotSupportedException();
	}
}
