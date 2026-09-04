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
	/// The root typeface token every type scale derives from.
	/// </summary>
	public const string DefaultFontFamilyKey = "DefaultFontFamily";

	/// <summary>
	/// Every semantic type-scale family key generated from <c>BaseTheme.DefaultFontFamily</c>,
	/// alongside the root typeface token they derive from.
	/// </summary>
	/// <remarks>
	/// The keys are listed here — rather than left to the <c>StaticResource</c> aliases
	/// <c>SharedTypography.xaml</c> declares for them — because those aliases are not the whole story:
	/// a design system may re-declare its slot families over them, and a later dictionary wins, so an
	/// alias-only cascade depends on where each design system declares its slots and in which order
	/// its dictionaries merge. Generating the slot keys from the root — the same way the spacing and
	/// shape scales are generated from their base unit — makes one font family reach every scale on
	/// every theme regardless. A slot added to <c>SharedTypography.xaml</c> must be added here too;
	/// <c>Given_DefaultFontFamily.When_DefaultFontFamilySet_Then_EverySharedTypographySlotFollows</c>
	/// guards the two against drifting.
	/// </remarks>
	public static readonly string[] TypefaceScaleKeys =
	{
		DefaultFontFamilyKey,
		"DisplayLargeFontFamily",
		"DisplayMediumFontFamily",
		"DisplaySmallFontFamily",
		"HeadlineLargeFontFamily",
		"HeadlineMediumFontFamily",
		"HeadlineSmallFontFamily",
		"TitleLargeFontFamily",
		"TitleMediumFontFamily",
		"TitleSmallFontFamily",
		"LabelLargeFontFamily",
		"LabelMediumFontFamily",
		"LabelSmallFontFamily",
		"LabelExtraSmallFontFamily",
		"BodyLargeFontFamily",
		"BodyMediumFontFamily",
		"BodySmallFontFamily",
		"CaptionLargeFontFamily",
		"CaptionMediumFontFamily",
		"CaptionSmallFontFamily",
	};

	/// <summary>
	/// Theme dictionary keys the color layer is generated for. "Default" is the dark theme.
	/// </summary>
	public static readonly string[] ThemeDictionaryKeys = { "Light", "Default" };

	/// <summary>
	/// Every theme dictionary declared by <c>SharedColors.xaml</c>, paired with the color theme keys
	/// its brushes are resolved from, in decreasing precedence.
	/// </summary>
	/// <remarks>
	/// <para>
	/// The candidate chains mirror the framework's own theme-dictionary resolution: the dark theme
	/// tries <c>Dark</c> before <c>Default</c> — consumer overrides are documented with
	/// <c>x:Key="Dark"</c> while the library's own layers use <c>Default</c> — and every theme falls
	/// back to <c>Default</c>.
	/// </para>
	/// <para>
	/// This is deliberately wider than <see cref="ThemeDictionaryKeys"/>: the brush dictionary carries a
	/// third, <c>HighContrast</c> block, but no color layer in the libraries defines HighContrast
	/// values — those brushes reference the same <c>*Color</c> role keys as the others. Its chain
	/// honors an explicit consumer <c>HighContrast</c> block first, then follows the dark palette,
	/// which keeps those brushes tracking seed and override changes instead of being frozen at
	/// whatever the ambient scope held when the dictionary was parsed.
	/// </para>
	/// </remarks>
	public static readonly (string BrushTheme, string[] ColorThemes)[] BrushThemeSources =
	{
		("Light", new[] { "Light", "Default" }),
		("Default", new[] { "Dark", "Default" }),
		("HighContrast", new[] { "HighContrast", "Dark", "Default" }),
	};

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
