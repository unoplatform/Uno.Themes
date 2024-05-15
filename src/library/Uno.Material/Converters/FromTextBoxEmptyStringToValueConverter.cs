using System;
using System.Collections.Generic;
using System.Text;

#if WinUI
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Data;
#else
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
#endif

namespace Uno.Material
{
	public class FromTextBoxEmptyStringToValueConverter : IValueConverter
	{
		public object NoHeaderAndNoPlaceholderValue { get; set; }

		public object HeaderOnlyValue { get; set; }

		public object HeaderOnlyWithTextValue { get; set; }

		public object PlaceholderOnlyValue { get; set; }

		public object HeaderAndPlaceholderValue { get; set; }

		public object Convert(object value, Type targetType, object parameter, string language)
		{
			var textBox = value as TextBox;

			if (textBox is not null)
			{
				var noHeader = !(textBox.Header is string str1) || string.IsNullOrEmpty(str1);
				var noPlaceholder = !(textBox.PlaceholderText is string str2) || string.IsNullOrEmpty(str2);
				var noText = string.IsNullOrEmpty(textBox.Text);

				if (!noHeader && !noPlaceholder)
				{
					return HeaderAndPlaceholderValue;
				}
				else if (!noHeader && noPlaceholder)
				{
					if (!noText)
					{
						return HeaderOnlyWithTextValue;
					}

					return HeaderOnlyValue;
				}
				else if (noHeader && !noPlaceholder)
				{
					return PlaceholderOnlyValue;
				}

				return NoHeaderAndNoPlaceholderValue;
			}

			return NoHeaderAndNoPlaceholderValue;
		}

		public object ConvertBack(object value, Type targetType, object parameter, string language)
		{
			throw new NotSupportedException();
		}
	}
}
