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
	public static string SharedTypographyResourcePath = $"ms-appx:///{PackageName}/Styles/Applications/Common/SharedTypography.xaml";
	public static string SharedSpacingResourcePath = $"ms-appx:///{PackageName}/Styles/Applications/Common/SharedSpacing.xaml";
	public static string SharedShapeResourcePath = $"ms-appx:///{PackageName}/Styles/Applications/Common/SharedShape.xaml";
	public static string SharedDensityResourcePath = $"ms-appx:///{PackageName}/Styles/Applications/Common/SharedDensity.xaml";

	public static class Presets
	{
		public static string SpacingCompact = $"ms-appx:///{PackageName}/Styles/Applications/Presets/SpacingCompact.xaml";
		public static string SpacingCozy = $"ms-appx:///{PackageName}/Styles/Applications/Presets/SpacingCozy.xaml";
		public static string SpacingComfortable = $"ms-appx:///{PackageName}/Styles/Applications/Presets/SpacingComfortable.xaml";
	}
}
