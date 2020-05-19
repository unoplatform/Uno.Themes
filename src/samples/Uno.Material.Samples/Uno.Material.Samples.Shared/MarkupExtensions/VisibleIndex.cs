using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Markup;

namespace Uno.Material.Samples.MarkupExtensions
{
	public class VisibleIndex : MarkupExtension, IValueConverter
	{
		public int TargetIndex { get; set; }

		protected override object ProvideValue() => this;

		public object Convert(object value, Type targetType, object parameter, string language)
		{
			return (value is int index && index == TargetIndex)
				? Visibility.Visible
				: Visibility.Collapsed;
		}

		public object ConvertBack(object value, Type targetType, object parameter, string language)
		{
			throw new NotSupportedException();
		}
	}
}
