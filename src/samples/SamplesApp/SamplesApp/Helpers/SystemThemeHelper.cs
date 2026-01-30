namespace Uno.Themes.Samples.Helpers;

public static class SystemThemeHelper
{
	/// <summary>
	/// Get the ApplicationTheme of the device/system
	/// </summary>
	public static ApplicationTheme GetSystemApplicationTheme()
	{
		var settings = new UISettings();
		var systemBackground = settings.GetColorValue(UIColorType.Background);
		var black = Windows.UI.Color.FromArgb(255, 0, 0, 0);
		return systemBackground == black ? ApplicationTheme.Dark : ApplicationTheme.Light;
	}
}
 
