using System;
using System.Globalization;

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
		return !string.IsNullOrEmpty(format)
			? string.Format(GetCulture(language), format, value)
			: value;
	}

	// Respect the binding's language (BCP-47) so culture-sensitive formats such as
	// the DatePicker's "{0:d}" stay localized; fall back to the current UI culture
	// when no language is supplied or the tag is unrecognized.
	private static CultureInfo GetCulture(string language)
	{
		if (string.IsNullOrEmpty(language))
		{
			return CultureInfo.CurrentCulture;
		}

		try
		{
			return CultureInfo.GetCultureInfo(language);
		}
		catch (CultureNotFoundException)
		{
			return CultureInfo.CurrentCulture;
		}
	}

	public object ConvertBack(object value, Type targetType, object parameter, string language)
	{
		throw new NotImplementedException();
	}
}
