using System;
using System.Collections.Generic;
using System.Text;
using Uno.Material.Controls;
using Windows.UI.Xaml.Data;

namespace Uno.Themes.Samples.Converters
{
	public class FromBoolToSnackBarStatusConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, string language) => value is bool x
			? (x ? SnackBarStatus.Visible : SnackBarStatus.Hidden)
			: SnackBarStatus.Default;

		public object ConvertBack(object value, Type targetType, object parameter, string language) => throw new NotSupportedException("Only one-way conversion is supported.");
	}
}
