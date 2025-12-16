namespace Uno.Simple;

internal static class SimpleConstants
{
	public static string PackageName =
#if WinUI
		"Uno.Simple.WinUI";
#else
		"Uno.Simple";
#endif

	public static class ResourcePaths
	{
		public static class Version2
		{
			public static string ColorPalette = $"ms-appx:///{PackageName}/Styles/Application/v2/ColorPalette.xaml";
			public static string MaterialColors = $"ms-appx:///{PackageName}/Styles/Application/v2/MaterialColors.xaml";
			public static string Typography = $"ms-appx:///{PackageName}/Styles/Application/v2/Typography.xaml";
			public static string MergedPages = $"ms-appx:///{PackageName}/Generated/mergedpages.xaml";
		}

		public static class Common
		{
			public static string AnimationConstants = $"ms-appx:///{PackageName}/Styles/Application/Common/AnimationConstants.xaml";
			public static string Fonts = $"ms-appx:///{PackageName}/Styles/Application/Common/Fonts.xaml";
			public static string TextBoxVariables = $"ms-appx:///{PackageName}/Styles/Application/Common/TextBoxVariables.xaml";
		}
	}
}
