using System;
using System.Collections.Generic;
using System.Text;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace Uno.Material.Converter
{
	public class MultiplyDoubleConverter : IValueConverter
	{
		/// <summary>
		/// Multiplication factor
		/// </summary>
		public double Factor { get; set; }

		/// <summary>
		/// Multiply the converter value with the Factor
		/// </summary>
		public object Convert(object value, Type targetType, object parameter, string language)
		{
			double input = 0.0;

			if (value is double doubleValue)
			{
				input = doubleValue;
			}
			else if (double.TryParse(value.ToString(), out double result))
			{
				input = result;
			}

			return input * Factor;
		}

		public object ConvertBack(object value, Type targetType, object parameter, string language)
		{
			throw new NotSupportedException();
		}
	}
}
