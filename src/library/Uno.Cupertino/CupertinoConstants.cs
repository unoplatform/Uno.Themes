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
	public static string Brushes = $"ms-appx:///{PackageName}/Styles/Application/CupertinoBrushes.xaml";
	public static string Fonts = $"ms-appx:///{PackageName}/Styles/Application/Fonts.xaml";
	public static string StateConstants = $"ms-appx:///{PackageName}/Styles/Application/StateConstants.xaml";
	public static string MergedPages = $"ms-appx:///{PackageName}/Generated/mergedpages.xaml";

	/// <summary>The font key a pre-<see cref="CupertinoTheme"/> font override file redefines.</summary>
	public const string LegacyFontFamilyKey = "CupertinoFontFamily";

	private const string PrimaryColor = "PrimaryColor";

	/// <summary>
	/// Every brush in <c>CupertinoBrushes.xaml</c> with the color keys that may supply it. A XAML-backed
	/// dictionary cannot be enumerated on Uno, so this list is the source of truth — keep it in sync with
	/// that file. The accent brushes name the semantic primary second, so they follow a seed color or a
	/// <c>PrimaryColor</c> override while a consumer override of their own color still wins.
	/// </summary>
	public static readonly (string Brush, string[] Colors)[] BrushColorKeys =
	{
		// Accent
		("CupertinoBlueBrush", new[] { "CupertinoBlueColor", PrimaryColor }),
		("CupertinoLinkBrush", new[] { "LinkColor", PrimaryColor }),

		// System colors
		("CupertinoRedBrush", new[] { "CupertinoRedColor" }),
		("CupertinoOrangeBrush", new[] { "CupertinoOrangeColor" }),
		("CupertinoYellowBrush", new[] { "CupertinoYellowColor" }),
		("CupertinoGreenBrush", new[] { "CupertinoGreenColor" }),
		("CupertinoMintBrush", new[] { "CupertinoMintColor" }),
		("CupertinoTealBrush", new[] { "CupertinoTealColor" }),
		("CupertinoCyanBrush", new[] { "CupertinoCyanColor" }),
		("CupertinoIndigoBrush", new[] { "CupertinoIndigoColor" }),
		("CupertinoPurpleBrush", new[] { "CupertinoPurpleColor" }),
		("CupertinoPinkBrush", new[] { "CupertinoPinkColor" }),
		("CupertinoBrownBrush", new[] { "CupertinoBrownColor" }),
		("CupertinoWhiteBrush", new[] { "CupertinoWhiteColor" }),
		("CupertinoBlackBrush", new[] { "CupertinoBlackColor" }),

		// Grays
		("CupertinoPrimaryGrayBrush", new[] { "CupertinoPrimaryGrayColor" }),
		("CupertinoSecondaryGrayBrush", new[] { "CupertinoSecondaryGrayColor" }),
		("CupertinoTertiaryGrayBrush", new[] { "CupertinoTertiaryGrayColor" }),
		("CupertinoQuaternaryGrayBrush", new[] { "CupertinoQuaternaryGrayColor" }),
		("CupertinoQuinaryGrayBrush", new[] { "CupertinoQuinaryGrayColor" }),
		("CupertinoSenaryGrayBrush", new[] { "CupertinoSenaryGrayColor" }),

		// Labels, fills, backgrounds, separators
		("CupertinoLabelBrush", new[] { "LabelColor" }),
		("CupertinoSecondaryLabelBrush", new[] { "SecondaryLabelColor" }),
		("CupertinoTertiaryLabelBrush", new[] { "TertiaryLabelColor" }),
		("CupertinoQuaternaryLabelBrush", new[] { "QuaternaryLabelColor" }),
		("CupertinoPlaceholderTextBrush", new[] { "PlaceholderTextColor" }),
		("CupertinoSystemFillBrush", new[] { "SystemFillColor" }),
		("CupertinoSecondarySystemFillBrush", new[] { "SecondarySystemFillColor" }),
		("CupertinoTertiarySystemFillBrush", new[] { "TertiarySystemFillColor" }),
		("CupertinoQuaternarySystemFillBrush", new[] { "QuaternarySystemFillColor" }),
		("CupertinoSystemBackgroundBrush", new[] { "SystemBackgroundColor" }),
		("CupertinoSecondarySystemBackgroundBrush", new[] { "SecondarySystemBackgroundColor" }),
		("CupertinoTertiarySystemBackgroundBrush", new[] { "TertiarySystemBackgroundColor" }),
		("CupertinoSystemGroupedBackgroundBrush", new[] { "SystemGroupedBackgroundColor" }),
		("CupertinoSecondarySystemGroupedBackgroundBrush", new[] { "SecondarySystemGroupedBackgroundColor" }),
		("CupertinoTertiarySystemGroupedBackgroundBrush", new[] { "TertiarySystemGroupedBackgroundColor" }),
		("CupertinoSeparatorBrush", new[] { "SeparatorColor" }),
		("CupertinoOpaqueSeparatorBrush", new[] { "OpaqueSeparatorColor" }),
		("RadioButtonBackgroundBrush", new[] { "RadioButtonBackgroundColor" }),
	};
}
