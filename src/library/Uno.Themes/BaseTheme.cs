using System;
using Microsoft.UI.Xaml;
using Uno.Themes.ColorGeneration;
using Windows.UI;


namespace Uno.Themes;

/// <summary>
/// Controls the spacing density applied to all controls.
/// Drives the base spacing unit used by all Space* tokens.
/// </summary>
public enum Density
{
	/// <summary>Compact — base spacing unit = 3 px, tighter padding for data-dense UIs.</summary>
	Compact = 3,

	/// <summary>Regular (default) — base spacing unit = 4 px, balanced spacing.</summary>
	Regular = 4,

	/// <summary>Comfortable — base spacing unit = 5 px, more generous padding.</summary>
	Comfy = 5,
}

public abstract partial class BaseTheme : ResourceDictionary
{
	private bool _isFontOverrideMuted;

	/// <summary>
	/// Number of times this theme has rebuilt its resource dictionaries via
	/// <see cref="UpdateSource"/>. Exposed for diagnostics and tests.
	/// </summary>
	internal int RebuildCount { get; private set; }

	#region FontOverrideSource (DP)
	/// <summary>
	/// (Optional) Gets or sets a Uniform Resource Identifier (<see cref="Uri"/>) that provides the source location
	/// of a <see cref="ResourceDictionary"/> containing overrides for the default Uno.Material <see cref="FontFamily"/> resources
	/// </summary>
	public string FontOverrideSource
	{
		get => (string)GetValue(FontOverrideSourceProperty);
		set => SetValue(FontOverrideSourceProperty, value);
	}

	public static DependencyProperty FontOverrideSourceProperty { get; } =
		DependencyProperty.Register(
			nameof(FontOverrideSource),
			typeof(string),
			typeof(BaseTheme),
			new PropertyMetadata(null, OnFontOverrideSourceChanged));

	private static void OnFontOverrideSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
	{
		if (d is BaseTheme theme && e.NewValue is string sourceUri)
		{
			theme.FontOverrideDictionary = new ResourceDictionary() { Source = new Uri(sourceUri) };
		}
	}
	#endregion

	#region ColorOverrideSource (DP)
	/// <summary>
	/// (Optional) Gets or sets a Uniform Resource Identifier (<see cref="Uri"/>) that provides the source location
	/// of a <see cref="ResourceDictionary"/> containing overrides for the default <see cref="Color"/> resources.
	/// </summary>
	/// <remarks>The overrides set here should be re-defining the <see cref="Color"/> resources used by the theme, not the <see cref="SolidColorBrush"/> resources.</remarks>
	public string ColorOverrideSource
	{
		get => (string)GetValue(ColorOverrideSourceProperty);
		set => SetValue(ColorOverrideSourceProperty, value);
	}

	public static DependencyProperty ColorOverrideSourceProperty { get; } =
		DependencyProperty.Register(
			nameof(ColorOverrideSource),
			typeof(string),
			typeof(BaseTheme),
			new PropertyMetadata(null, OnColorOverrideSourceChanged));

	private static void OnColorOverrideSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
	{
		if (d is BaseTheme theme)
		{
			if (e.NewValue is string sourceUri && !string.IsNullOrWhiteSpace(sourceUri))
			{
				theme.ColorOverrideDictionary = new ResourceDictionary { Source = new Uri(sourceUri) };
			}
			else
			{
				theme.ColorOverrideDictionary = null;
			}
		}
	}
	#endregion

	#region FontOverrideDictionary (DP)
	/// <summary>
	/// (Optional) Gets or sets a <see cref="ResourceDictionary"/> containing overrides for the default Uno.Material <see cref="FontFamily"/> resources
	/// </summary>
	public ResourceDictionary FontOverrideDictionary
	{
		get => (ResourceDictionary)GetValue(FontOverrideDictionaryProperty);
		set => SetValue(FontOverrideDictionaryProperty, value);
	}

	public static DependencyProperty FontOverrideDictionaryProperty { get; } =
		DependencyProperty.Register(
			nameof(FontOverrideDictionary),
			typeof(ResourceDictionary),
			typeof(BaseTheme),
			new PropertyMetadata(null, OnFontOverrideChanged));

	private static void OnFontOverrideChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
	{
		if (d is BaseTheme { _isFontOverrideMuted: false } theme)
		{
			theme.UpdateSource();
		}
	}
	#endregion

	#region ColorOverrideDictionary (DP)
	/// <summary>
	/// (Optional) Gets or sets a <see cref="ResourceDictionary"/> containing direct overrides for the default
	/// <see cref="Color"/> resources. These take highest precedence, overriding both defaults and seed-generated colors.
	/// </summary>
	/// <remarks>The overrides set here should be re-defining the <see cref="Color"/> resources used by the theme, not the <see cref="SolidColorBrush"/> resources.</remarks>
	public ResourceDictionary ColorOverrideDictionary
	{
		get => (ResourceDictionary)GetValue(ColorOverrideDictionaryProperty);
		set => SetValue(ColorOverrideDictionaryProperty, value);
	}

	public static DependencyProperty ColorOverrideDictionaryProperty { get; } =
		DependencyProperty.Register(
			nameof(ColorOverrideDictionary),
			typeof(ResourceDictionary),
			typeof(BaseTheme),
			new PropertyMetadata(null, OnSeedOrOverrideChanged));
	#endregion

	#region PrimarySeedColor (DP)
	/// <summary>
	/// Gets or sets the primary seed <see cref="Color"/> used to algorithmically generate
	/// the full color palette. When set, all semantic color roles are derived from this color.
	/// </summary>
	public Color? PrimarySeedColor
	{
		get => (Color?)GetValue(PrimarySeedColorProperty);
		set => SetValue(PrimarySeedColorProperty, value);
	}

	public static DependencyProperty PrimarySeedColorProperty { get; } =
		DependencyProperty.Register(
			nameof(PrimarySeedColor),
			typeof(Color?),
			typeof(BaseTheme),
			new PropertyMetadata(null, OnSeedOrOverrideChanged));
	#endregion

	#region SecondarySeedColor (DP)
	/// <summary>
	/// Gets or sets the secondary seed <see cref="Color"/>. If not set, the Secondary
	/// palette is auto-derived from <see cref="PrimarySeedColor"/>.
	/// </summary>
	public Color? SecondarySeedColor
	{
		get => (Color?)GetValue(SecondarySeedColorProperty);
		set => SetValue(SecondarySeedColorProperty, value);
	}

	public static DependencyProperty SecondarySeedColorProperty { get; } =
		DependencyProperty.Register(
			nameof(SecondarySeedColor),
			typeof(Color?),
			typeof(BaseTheme),
			new PropertyMetadata(null, OnSeedOrOverrideChanged));
	#endregion

	#region TertiarySeedColor (DP)
	/// <summary>
	/// Gets or sets the tertiary seed <see cref="Color"/>. If not set, the Tertiary
	/// palette is auto-derived from <see cref="PrimarySeedColor"/>.
	/// </summary>
	public Color? TertiarySeedColor
	{
		get => (Color?)GetValue(TertiarySeedColorProperty);
		set => SetValue(TertiarySeedColorProperty, value);
	}

	public static DependencyProperty TertiarySeedColorProperty { get; } =
		DependencyProperty.Register(
			nameof(TertiarySeedColor),
			typeof(Color?),
			typeof(BaseTheme),
			new PropertyMetadata(null, OnSeedOrOverrideChanged));
	#endregion

	private static void OnSeedOrOverrideChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
	{
		if (d is BaseTheme theme)
		{
			theme.UpdateSource();
		}
	}

	#region DefaultCornerRadius (DP)
	/// <summary>
	/// Gets or sets the base corner radius unit (in pixels). Default is 4.
	/// All shape scale tokens (Radius100, Radius200, …) are computed
	/// as multiples of this value — e.g. <c>DefaultCornerRadius="6"</c> makes
	/// Radius100=6, Radius200=12, Radius400=24, etc.
	/// <c>RadiusFull</c> always remains 9999 (pill shape).
	/// Individual tokens can still be overridden via lightweight styling.
	/// </summary>
	public double DefaultCornerRadius
	{
		get => (double)GetValue(DefaultCornerRadiusProperty);
		set => SetValue(DefaultCornerRadiusProperty, value);
	}

	public static DependencyProperty DefaultCornerRadiusProperty { get; } =
		DependencyProperty.Register(
			nameof(DefaultCornerRadius),
			typeof(double),
			typeof(BaseTheme),
			new PropertyMetadata(4.0, OnDefaultCornerRadiusChanged));

	private static void OnDefaultCornerRadiusChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
	{
		if (d is BaseTheme theme)
		{
			theme.UpdateSource();
		}
	}
	#endregion

	#region DefaultDensity (DP)
	/// <summary>
	/// Gets or sets the density preset for the theme. Default is <see cref="Density.Regular"/>.
	/// This drives the base spacing unit used by all Space* tokens:
	/// Compact = 3, Regular = 4, Comfy = 5.
	/// Control heights and icon sizes remain constant across densities.
	/// </summary>
	public Density DefaultDensity
	{
		get => (Density)GetValue(DefaultDensityProperty);
		set => SetValue(DefaultDensityProperty, value);
	}

	public static DependencyProperty DefaultDensityProperty { get; } =
		DependencyProperty.Register(
			nameof(DefaultDensity),
			typeof(Density),
			typeof(BaseTheme),
			new PropertyMetadata(Density.Regular, OnDefaultDensityChanged));

	private static void OnDefaultDensityChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
	{
		if (d is BaseTheme theme)
		{
			theme.UpdateSource();
		}
	}
	#endregion

	/// <summary>
	/// Gets the default primary seed color for this theme.
	/// When not <c>null</c>, seed color generation is always active — even
	/// without an explicit <see cref="PrimarySeedColor"/> — so the
	/// theme uses algorithmically-derived colors by default.
	/// Override in subclasses to provide a theme-specific default.
	/// </summary>
	protected virtual Color? DefaultPrimarySeed => null;

	/// <summary>
	/// When <c>true</c>, seed color generation preserves the source color's
	/// actual chroma (high-fidelity / "color match" mode). When <c>false</c>
	/// (default), the standard M3 minimum chroma of 48 is enforced, which
	/// guarantees vibrant colors but distorts low-chroma seeds like gray.
	/// </summary>
	protected virtual bool UseHighFidelityColors => false;

	public BaseTheme() : this(colorOverride: null, fontOverride: null)
	{

	}

	public BaseTheme(ResourceDictionary colorOverride = null, ResourceDictionary fontOverride = null)
	{
		if (colorOverride is { })
		{
			ColorOverrideDictionary = colorOverride;
		}

		if (fontOverride is { })
		{
			SetFontOverrideSilently(fontOverride);
		}

		// Run a synchronous initial rebuild so that {StaticResource …} lookups
		// against this dictionary resolve correctly from the moment the parser
		// finishes constructing it. WinUI/Uno's ResourceDictionary does not
		// expose an IsParsing / EndInit signal we could hook to coalesce the
		// ctor + N XAML-driven DP setters into one rebuild, so we accept the
		// cost of rebuilding once per DP set during XAML init.
		UpdateSource();
	}

	private void SetFontOverrideSilently(ResourceDictionary fontOverride)
	{
		try
		{
			_isFontOverrideMuted = true;
			FontOverrideDictionary = fontOverride;
		}
		finally
		{
			_isFontOverrideMuted = false;
		}
	}

	protected void UpdateSource()
	{
		RebuildCount++;

#if !HAS_UNO
		Source = null;
#endif
		ThemeDictionaries.Clear();
		MergedDictionaries.Clear();
		this.Clear();

		var converters = new ResourceDictionary { Source = new Uri(ThemesConstants.ConverterResourcePath) };
		var colors = new ResourceDictionary { Source = new Uri(ThemesConstants.SharedColorsResourcePath) };

		// Theme-specific default fallback palette (Material's M3 defaults, Simple's
		// grayscale palette, etc.). Always merged first so that seed-generated and
		// user-override colors take precedence.
		colors.MergedDictionaries.Add(GetDefaultColorPalette());

		// Resolve seed colors, falling back to theme default for the primary
		var effectivePrimary = PrimarySeedColor ?? DefaultPrimarySeed;
		var effectiveSecondary = SecondarySeedColor;
		var effectiveTertiary = TertiarySeedColor;

		if (effectivePrimary is { } seed)
		{
			var seedPalette = SeedColorPaletteGenerator.Default.Generate(seed, effectiveSecondary, effectiveTertiary, UseHighFidelityColors);
			colors.SafeMerge(seedPalette);
		}

		// Explicit user overrides from ColorOverrideDictionary take highest precedence
		if (ColorOverrideDictionary is { } userOverride)
		{
			colors.MergedDictionaries.Add(userOverride);
		}

		var typography = new ResourceDictionary { Source = new Uri(ThemesConstants.SharedTypographyResourcePath) };

		// Generate spacing, shape, and density scales from code
		var baseSpacing = Enum.IsDefined(DefaultDensity) ? (double)DefaultDensity : 4.0;
		var spacing = GenerateSpacingScale(baseSpacing);
		var shape = GenerateShapeScale(DefaultCornerRadius);
		var density = GenerateDensityDefaults();

		var mergedPages = GenerateSpecificResources();

		mergedPages.MergedDictionaries.Add(colors);
		mergedPages.MergedDictionaries.Add(converters);

		MergedDictionaries.Add(typography);
		MergedDictionaries.Add(spacing);
		MergedDictionaries.Add(shape);
		MergedDictionaries.Add(density);
		MergedDictionaries.Add(mergedPages);
	}

	protected abstract ResourceDictionary GenerateSpecificResources();

	/// <summary>
	/// Provides the theme's fallback <see cref="ResourceDictionary"/> of default
	/// <see cref="Color"/> resources. This palette is loaded as the baseline before
	/// seed-generated and user-override colors are merged, so any color role that is
	/// not produced by seed generation (e.g. <c>ErrorColor</c>) falls back to it.
	/// </summary>
	protected abstract ResourceDictionary GetDefaultColorPalette();
}
