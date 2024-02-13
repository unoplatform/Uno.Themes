namespace Uno.Themes;

internal static class ThemesConstants
{
	public static string PackageName =
#if WinUI
		"Uno.Themes.WinUI";
#else
		"Uno.Themes";
#endif	

	public static string ConverterResourcePath = $"ms-appx:///{PackageName}/Styles/Applications/Common/Converters.xaml";
	public static string SharedColorsResourcePath = $"ms-appx:///{PackageName}/Styles/Applications/Common/SharedColors.xaml";
	public static string SharedColorPaletteResourcePath = $"ms-appx:///{PackageName}/Styles/Applications/Common/SharedColorPalette.xaml";
}
