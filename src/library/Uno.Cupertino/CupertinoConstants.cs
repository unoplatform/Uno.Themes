namespace Uno.Cupertino;

internal static class CupertinoConstants
{
	public static string PackageName =
#if WinUI
		"Uno.Cupertino.WinUI";
#else
		"Uno.Cupertino";
#endif

	public static string AnimationConstants = $"ms-appx:///{PackageName}/Styles/Application/AnimationConstants.xaml";
	public static string ColorPalette = $"ms-appx:///{PackageName}/Styles/Application/ColorPalette.xaml";
	public static string Colors = $"ms-appx:///{PackageName}/Styles/Application/CupertinoColors.xaml";
	public static string Fonts = $"ms-appx:///{PackageName}/Styles/Application/Fonts.xaml";
	public static string StateConstants = $"ms-appx:///{PackageName}/Styles/Application/StateConstants.xaml";
	public static string MergedPages = $"ms-appx:///{PackageName}/Generated/mergedpages.xaml";

}
