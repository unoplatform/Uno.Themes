namespace Uno.Material;

internal static class MaterialConstants
{
	public static string PackageName =
#if WinUI
		"Uno.Material.WinUI";
#else
		"Uno.Material";
#endif

	public static class ResourcePaths
	{
		public static class Version1
		{
			public static string ColorPalette = $"ms-appx:///{PackageName}/Styles/Application/v1/ColorPalette.xaml";
			public static string MaterialColors = $"ms-appx:///{PackageName}/Styles/Application/v1/MaterialColors.xaml";
			public static string MergedPages = $"ms-appx:///{PackageName}/Generated/mergedpages.v1.xaml";
		}

		public static class Version2
		{
			public static string Typography = $"ms-appx:///{PackageName}/Styles/Application/v2/MaterialFonts.xaml";
			public static string MergedPages = $"ms-appx:///{PackageName}/Generated/mergedpages.v2.xaml";
		}

		public static class Common
		{
			public static string AnimationConstants = $"ms-appx:///{PackageName}/Styles/Application/Common/AnimationConstants.xaml";
			public static string Fonts = $"ms-appx:///{PackageName}/Styles/Application/Common/Fonts.xaml";
			public static string TextBoxVariables = $"ms-appx:///{PackageName}/Styles/Application/Common/TextBoxVariables.xaml";
		}
	}
}
