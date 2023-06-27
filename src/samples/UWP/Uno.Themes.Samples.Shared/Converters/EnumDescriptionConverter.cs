using System;
using System.Collections.Generic;
using System.Text;
using Uno.Themes.Samples.Helpers;
using Windows.UI.Xaml.Data;

namespace Uno.Themes.Samples.Converters
{
	public class EnumDescriptionConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, string language)
		{
			if (value == null) return null;
			if (value is Enum @enum)
			{
				return @enum.GetDescription() ?? @enum.ToString();
			}
			else
			{
				throw new ArgumentNullException($"A value of type enum is expected for {nameof(EnumDescriptionConverter)}. (Type={value.GetType()})");
			}
		}

		public object ConvertBack(object value, Type targetType, object parameter, string language) => throw new NotSupportedException("Only one-way conversion is supported.");
	}
}
