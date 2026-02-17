namespace Uno.Themes.Simple.Samples.Helpers;

public static class SystemThemeHelper
{
    public static ApplicationTheme GetSystemApplicationTheme()
    {
        var uiSettings = new Windows.UI.ViewManagement.UISettings();
        var color = uiSettings.GetColorValue(Windows.UI.ViewManagement.UIColorType.Background);

        return color == Microsoft.UI.Colors.Black
            ? ApplicationTheme.Dark
            : ApplicationTheme.Light;
    }
}
