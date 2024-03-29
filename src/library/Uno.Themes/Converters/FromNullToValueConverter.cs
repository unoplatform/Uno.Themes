﻿using System;

#if WinUI
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Data;
#else
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;
#endif

namespace Uno.Themes;

public class FromNullToValueConverter : IValueConverter
{
	public object NullValue { get; set; }

	public object NotNullValue { get; set; }

	public object Convert(object value, Type targetType, object parameter, string language)
	{
		return value is null || value == DependencyProperty.UnsetValue ? NullValue : NotNullValue;
	}

	public object ConvertBack(object value, Type targetType, object parameter, string language)
	{
		throw new NotSupportedException();
	}
}
