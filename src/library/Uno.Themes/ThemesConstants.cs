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

	/// <summary>
	/// Theme dictionary keys the color layer is generated for. "Default" is the dark theme.
	/// </summary>
	public static readonly string[] ThemeDictionaryKeys = { "Light", "Default" };

	/// <summary>
	/// Every semantic color role declared in <c>SharedColorPalette.xaml</c> and consumed by the
	/// brushes in <c>SharedColors.xaml</c>. Order is irrelevant; completeness is not — a role
	/// missing here keeps its parse-time brush color when a seed or override changes it.
	/// </summary>
	public static readonly string[] SemanticColorKeys =
	{
		"PrimaryColor", "OnPrimaryColor", "PrimaryContainerColor", "OnPrimaryContainerColor",
		"PrimaryInverseColor", "PrimaryVariantDarkColor", "PrimaryVariantLightColor",
		"SecondaryColor", "OnSecondaryColor", "SecondaryContainerColor", "OnSecondaryContainerColor",
		"SecondaryVariantDarkColor", "SecondaryVariantLightColor",
		"TertiaryColor", "OnTertiaryColor", "TertiaryContainerColor", "OnTertiaryContainerColor",
		"ErrorColor", "OnErrorColor", "ErrorContainerColor", "OnErrorContainerColor",
		"BackgroundColor", "OnBackgroundColor",
		"SurfaceColor", "OnSurfaceColor", "SurfaceVariantColor", "OnSurfaceVariantColor",
		"SurfaceInverseColor", "OnSurfaceInverseColor", "SurfaceTintColor",
		"OutlineColor", "OutlineVariantColor",
	};

	/// <summary>
	/// The interaction-state suffixes <c>SharedColors.xaml</c> appends between a color role and
	/// <c>Brush</c> — e.g. <c>Primary</c> + <c>Hover</c> + <c>Brush</c>. The empty entry is the
	/// opaque base brush.
	/// </summary>
	public static readonly string[] BrushStateSuffixes =
	{
		"", "Hover", "Focused", "Pressed", "Dragged", "Selected", "Medium", "Low", "Disabled",
	};
}
