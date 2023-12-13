using System;
using System.Collections.Generic;
using System.Text;

#if WinUI
using Microsoft.UI.Xaml.Data;
#else
using Windows.UI.Xaml.Data;
#endif

namespace Uno.Themes;

public class FromOpenToCustomValueConverter : IValueConverter
{
	public object ValueIfOpen { get; set; }

	public object ValueIfClosed { get; set; }

	public object Convert(object value, Type targetType, object parameter, string language)
	{
		return (bool)value ? ValueIfOpen : ValueIfClosed;
	}

	public object ConvertBack(object value, Type targetType, object parameter, string language)
	{
		throw new NotSupportedException();
	}
}
