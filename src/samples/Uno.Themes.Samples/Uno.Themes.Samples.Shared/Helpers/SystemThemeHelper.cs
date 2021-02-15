using System;
using System.Collections.Generic;
using System.Text;
using Windows.UI;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;

namespace Uno.Themes.Samples.Helpers
{
	public static class SystemThemeHelper
	{
		/// <summary>
		/// Get the ApplicationTheme of the device/system
		/// </summary>
		public static ApplicationTheme GetSystemApplicationTheme()
		{
			var settings = new UISettings();
			var systemBackground = settings.GetColorValue(UIColorType.Background);
			var black = Color.FromArgb(255, 0, 0, 0);
			return systemBackground == black ? ApplicationTheme.Dark : ApplicationTheme.Light;
		}
	}
}
