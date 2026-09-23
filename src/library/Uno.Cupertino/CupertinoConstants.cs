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

	/// <summary>The resource <see cref="CupertinoTheme.GlassRenderingMode"/> is published under, as the enum member's name.</summary>
	public const string GlassRenderingModeKey = "CupertinoGlassRenderingMode";

	public const string SpacingBaseKey = "Space100";
	public const string TouchTargetMinSizeKey = "TouchTargetMinSize";

	private const string PrimaryColor = "PrimaryColor";

	// Composite spacing keys retain Apple's default proportions while following the shared spacing base.
	public static readonly (string Key, double Left, double Top, double Right, double Bottom)[] SpacingMultipliers =
	{
		("CupertinoButtonPadding", 4, 1.75, 4, 1.75),
		("MaterialDatePickerHostPadding", 6, 6, 2, 2),
		("CupertinoAlertTitleMargin", 5, 5, 5, 1.5),
		("CupertinoAlertContentMargin", 5, 0, 5, 4),
		("CupertinoToolTipPadding", 2.5, 1.5, 2.5, 1.5),
		("CupertinoAlertActionsMargin", 3, 0, 3, 3),
		("CupertinoCalendarDatePickerPadding", 3, 0, 3, 0.5),
	};


	/// <summary>The accent brushes of <see cref="BrushColorKeys"/> with the color key each is published under.</summary>
	public static readonly (string Brush, string Color)[] AccentColorKeys =
	{
		("CupertinoBlueBrush", "CupertinoBlueColor"),
		("CupertinoLinkBrush", "LinkColor"),
	};

	/// <summary>The theme blocks <c>ColorPalette.xaml</c> declares; the accent colors are re-emitted for each.</summary>
	public static readonly string[] AccentThemeKeys = { "Light", "Default" };

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
		("CupertinoGlassProminentTintBrush", new[] { PrimaryColor }),

		// Glass tints: the surface laid over a popover's or an alert's glass, the opacity is the tint strength
		("CupertinoPopoverTintBrush", new[] { "SurfaceColor" }),
		("CupertinoBarTintBrush", new[] { "SurfaceColor" }),
		("CupertinoDialogTintBrush", new[] { "SurfaceColor" }),

		// Live field and calendar brushes (opacity remains in CupertinoBrushes.xaml).
		("CupertinoFieldBorderBrush", new[] { "LabelColor", "OnSurfaceColor" }),
		("CupertinoHeaderForegroundBrush", new[] { "LabelColor", "OnSurfaceColor" }),
		("CupertinoDetailsLightBrush", new[] { "CupertinoPrimaryGrayColor" }),
		("CupertinoCalendarViewSelectedBackground", new[] { "CupertinoBlueColor", PrimaryColor }),
		("CupertinoCalendarDatePickerBorderBrushPointerOver", new[] { "CupertinoPrimaryGrayColor" }),
		("CupertinoCalendarDatePickerBorderBrushPressed", new[] { "CupertinoPrimaryGrayColor" }),

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
		("CupertinoLabelBrush", new[] { "LabelColor", "OnSurfaceColor" }),
		("CupertinoSecondaryLabelBrush", new[] { "SecondaryLabelColor", "OnSurfaceVariantColor" }),
		("CupertinoTertiaryLabelBrush", new[] { "TertiaryLabelColor", "OnSurfaceVariantColor" }),
		("CupertinoQuaternaryLabelBrush", new[] { "QuaternaryLabelColor", "OnSurfaceVariantColor" }),
		("CupertinoPlaceholderTextBrush", new[] { "PlaceholderTextColor", "OnSurfaceVariantColor" }),
		("CupertinoSystemFillBrush", new[] { "SystemFillColor" }),
		("CupertinoSecondarySystemFillBrush", new[] { "SecondarySystemFillColor" }),
		("CupertinoTertiarySystemFillBrush", new[] { "TertiarySystemFillColor" }),
		("CupertinoQuaternarySystemFillBrush", new[] { "QuaternarySystemFillColor" }),
		("CupertinoSystemBackgroundBrush", new[] { "SystemBackgroundColor", "BackgroundColor" }),
		("CupertinoSecondarySystemBackgroundBrush", new[] { "SecondarySystemBackgroundColor", "SurfaceColor" }),
		("CupertinoTertiarySystemBackgroundBrush", new[] { "TertiarySystemBackgroundColor", "SurfaceColor" }),
		("CupertinoSystemGroupedBackgroundBrush", new[] { "SystemGroupedBackgroundColor", "SurfaceVariantColor" }),
		("CupertinoSecondarySystemGroupedBackgroundBrush", new[] { "SecondarySystemGroupedBackgroundColor", "SurfaceColor" }),
		("CupertinoTertiarySystemGroupedBackgroundBrush", new[] { "TertiarySystemGroupedBackgroundColor", "SurfaceColor" }),
		("CupertinoSeparatorBrush", new[] { "SeparatorColor", "OutlineVariantColor" }),
		("CupertinoOpaqueSeparatorBrush", new[] { "OpaqueSeparatorColor", "OutlineColor" }),
		("RadioButtonBackgroundBrush", new[] { "RadioButtonBackgroundColor" }),
	};
}
