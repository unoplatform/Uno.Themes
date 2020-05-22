using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Markup;

namespace Uno.Material.Samples.MarkupExtensions
{
	public class StringEquals : MarkupExtension, IValueConverter
	{
		public string Match { get; set; }

		protected override object ProvideValue() => this;

		public object Convert(object value, Type targetType, object parameter, string language)
		{
			return (value is string str && string.Equals(str, Match, StringComparison.OrdinalIgnoreCase))
				? Visibility.Visible
				: Visibility.Collapsed;
		}

		public object ConvertBack(object value, Type targetType, object parameter, string language)
		{
			throw new NotSupportedException();
		}
	}
}
