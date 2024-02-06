using System;
using System.Collections.Generic;
using System.Text;

#if WinUI
using Microsoft.UI.Xaml.Data;
#else
using Windows.UI.Xaml.Data;
#endif

namespace Uno.Themes;

public class FirstCharacterConverter : IValueConverter
{
	public object Convert(object value, Type targetType, object parameter, string language)
	{
		var str = value as string;
		return string.IsNullOrWhiteSpace(str) ? string.Empty : (object)str.Substring(0, 1);
	}

	public object ConvertBack(object value, Type targetType, object parameter, string language)
	{
		throw new NotImplementedException();
	}
}
