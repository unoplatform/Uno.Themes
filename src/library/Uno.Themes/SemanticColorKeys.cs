namespace Uno.Themes;

/// <summary>
/// The semantic color role keys declared in <c>SharedColorPalette.xaml</c> and consumed by the
/// brushes in <c>SharedColors.xaml</c>, named so the code that writes them (seed generation, the
/// Fluent accent mapping) and the code that reads them (the brush updater) cannot drift by typo.
/// </summary>
/// <remarks>
/// <see cref="All"/> is the complete role set; its order is irrelevant, its completeness is not —
/// a role missing there keeps its parse-time brush color when a seed or override changes it.
/// <see cref="Shadow"/> has no brushes in <c>SharedColors.xaml</c> (the updater skips roles whose
/// brushes do not exist); it is listed so the set matches the palette one for one.
/// </remarks>
internal static class SemanticColorKeys
{
	public const string Primary = "PrimaryColor";
	public const string OnPrimary = "OnPrimaryColor";
	public const string PrimaryContainer = "PrimaryContainerColor";
	public const string OnPrimaryContainer = "OnPrimaryContainerColor";
	public const string PrimaryInverse = "PrimaryInverseColor";
	public const string PrimaryVariantDark = "PrimaryVariantDarkColor";
	public const string PrimaryVariantLight = "PrimaryVariantLightColor";

	public const string Secondary = "SecondaryColor";
	public const string OnSecondary = "OnSecondaryColor";
	public const string SecondaryContainer = "SecondaryContainerColor";
	public const string OnSecondaryContainer = "OnSecondaryContainerColor";
	public const string SecondaryVariantDark = "SecondaryVariantDarkColor";
	public const string SecondaryVariantLight = "SecondaryVariantLightColor";

	public const string Tertiary = "TertiaryColor";
	public const string OnTertiary = "OnTertiaryColor";
	public const string TertiaryContainer = "TertiaryContainerColor";
	public const string OnTertiaryContainer = "OnTertiaryContainerColor";

	public const string Error = "ErrorColor";
	public const string OnError = "OnErrorColor";
	public const string ErrorContainer = "ErrorContainerColor";
	public const string OnErrorContainer = "OnErrorContainerColor";

	public const string Background = "BackgroundColor";
	public const string OnBackground = "OnBackgroundColor";

	public const string Surface = "SurfaceColor";
	public const string OnSurface = "OnSurfaceColor";
	public const string SurfaceVariant = "SurfaceVariantColor";
	public const string OnSurfaceVariant = "OnSurfaceVariantColor";
	public const string SurfaceInverse = "SurfaceInverseColor";
	public const string OnSurfaceInverse = "OnSurfaceInverseColor";
	public const string SurfaceTint = "SurfaceTintColor";

	public const string Outline = "OutlineColor";
	public const string OutlineVariant = "OutlineVariantColor";

	public const string Shadow = "ShadowColor";

	/// <summary>
	/// Every semantic color role, in declaration order.
	/// </summary>
	public static readonly string[] All =
	{
		Primary, OnPrimary, PrimaryContainer, OnPrimaryContainer,
		PrimaryInverse, PrimaryVariantDark, PrimaryVariantLight,
		Secondary, OnSecondary, SecondaryContainer, OnSecondaryContainer,
		SecondaryVariantDark, SecondaryVariantLight,
		Tertiary, OnTertiary, TertiaryContainer, OnTertiaryContainer,
		Error, OnError, ErrorContainer, OnErrorContainer,
		Background, OnBackground,
		Surface, OnSurface, SurfaceVariant, OnSurfaceVariant,
		SurfaceInverse, OnSurfaceInverse, SurfaceTint,
		Outline, OutlineVariant,
		Shadow,
	};
}
