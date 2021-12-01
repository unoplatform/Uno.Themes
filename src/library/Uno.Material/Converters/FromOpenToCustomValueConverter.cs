using System;
using System.Collections.Generic;
using System.Text;

#if WinUI
using Microsoft.UI.Xaml.Data;
#else
using Windows.UI.Xaml.Data;
#endif

namespace Uno.Material
{
	public class FromOpenToCustomValueConverter : IValueConverter
	{
		public object ValueIfOpen { get; set; }

		public object ValueIfClosed { get; set; }

		public object Convert(object value, Type targetType, object parameter, string language)
		{
			if ((bool)value)
			{
				return ValueIfOpen;
			}

			return ValueIfClosed;
		}

		public object ConvertBack(object value, Type targetType, object parameter, string language)
		{
			throw new NotSupportedException();
		}
	}
}
