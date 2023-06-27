using System;
using System.Collections.Generic;
using System.Text;
using Windows.UI.Xaml.Data;

namespace Uno.Themes.Samples.Converters
{
	public class FromStringToValueConverter : IValueConverter
	{
		public enum CheckMethod { IsNullOrEmpty, IsNullOrWhitespace, IsEqualToParameterValue}

		public CheckMethod Check { get; set; }

		public object TrueValue { get; set; }

		public object FalseValue { get; set; }

		public object Convert(object value, Type targetType, object parameter, string language)
		{
			if (value is string text)
			{
				if (Check == CheckMethod.IsEqualToParameterValue && parameter is string param)
				{
					return text.Equals(param) ? TrueValue : FalseValue;
				}
				else if (Check == CheckMethod.IsNullOrEmpty)
				{
					return string.IsNullOrEmpty(text) ? TrueValue : FalseValue;
				}
				else if (Check == CheckMethod.IsNullOrWhitespace)
				{
					return string.IsNullOrWhiteSpace(text) ? TrueValue : FalseValue;
				}
			}
			
			return FalseValue;
		}

		public object ConvertBack(object value, Type targetType, object parameter, string language) => throw new NotSupportedException("Only one-way conversion is supported.");
	}
}
