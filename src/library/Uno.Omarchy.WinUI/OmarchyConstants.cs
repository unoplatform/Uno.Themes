namespace Uno.Omarchy;

internal static class OmarchyConstants
{
	public const string PackageName = "Uno.Omarchy.WinUI";

	public static class ResourcePaths
	{
		public static readonly string MergedPages = $"ms-appx:///{PackageName}/Generated/mergedpages.xaml";
		public static readonly string Fonts = $"ms-appx:///{PackageName}/Styles/Application/Common/Fonts.xaml";
		public static readonly string Tokens = $"ms-appx:///{PackageName}/Styles/Application/Common/Tokens.xaml";
	}

	/// <summary>
	/// Resource keys generated in code from the active <see cref="OmarchyPalette"/>. Each entry is
	/// the key stem: <c>{stem}Color</c> is the palette color, <c>{stem}Brush</c> the matching brush.
	/// </summary>
	public static class ResourceKeys
	{
		public const string Background = "OmarchyBackground";
		public const string Foreground = "OmarchyForeground";
		public const string Accent = "OmarchyAccent";
		public const string Selection = "OmarchySelection";
		public const string Muted = "OmarchyMuted";
		public const string NormalPrefix = "OmarchyNormal";
		public const string BrightPrefix = "OmarchyBright";

		public const string ColorSuffix = "Color";
		public const string BrushSuffix = "Brush";
	}
}
