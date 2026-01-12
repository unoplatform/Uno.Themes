namespace Uno.Simple;

internal static class SimpleConstants
{
	public static readonly string PackageName =
#if WinUI
		"Uno.Simple.WinUI";
#else
		"Uno.Simple";
#endif

	public static class ResourcePaths
	{
		public static string ColorPalette = $"ms-appx:///{PackageName}/Styles/Application/ColorPalette.xaml";
		public static readonly string SimpleColors = $"ms-appx:///{PackageName}/Styles/Application/SimpleColors.xaml";
		public static string Thickness = $"ms-appx:///{PackageName}/Styles/Application/Thickness.xaml";
		public static readonly string MergedPages = $"ms-appx:///{PackageName}/Generated/mergedpages.xaml";
		
		public static class Common
		{
			public static readonly string Fonts = $"ms-appx:///{PackageName}/Styles/Application/Common/Fonts.xaml";
		}
	}
}
