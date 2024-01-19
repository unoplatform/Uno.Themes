﻿using System;

#if WinUI
using Microsoft.UI.Xaml.Data;
#else
using Windows.UI.Xaml.Data;
#endif

namespace Uno.Themes;

public class FromBoolToValueConverter : IValueConverter
{
	public object NullValue { get; set; }

	public object FalseValue { get; set; }

	public object TrueValue { get; set; }

	public object NullOrFalseValue
	{
		get { return FalseValue; }
		set { FalseValue = NullValue = value; }
	}

	public object NullOrTrueValue
	{
		get { return TrueValue; }
		set { TrueValue = NullValue = value; }
	}

	public object Convert(object value, Type targetType, object parameter, string language)
	{
		if (value == null)
		{
			return NullValue;
		}

		if (System.Convert.ToBoolean(value))
		{
			return TrueValue;
		}
		else
		{
			return FalseValue;
		}
	}

	public object ConvertBack(object value, Type targetType, object parameter, string language)
	{
		throw new NotImplementedException();
	}
}
