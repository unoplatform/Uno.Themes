using System;
using System.Collections.Generic;
using Uno.Themes.ColorGeneration;


#if WinUI
using Microsoft.UI;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Media;
using Windows.UI;
#else
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;
#endif


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
	private bool _isUpdatingColorOverrides;
	private bool _isFontOverrideMuted;
	private ResourceDictionary _baseColorOverride;

	// Tracks dictionaries appended by UpdateSource() so subsequent rebuilds can remove
	// just those without touching the URI-backed entries Uno populated via CopyFrom
	// when Source was set.
	private readonly List<ResourceDictionary> _dynamicDictionaries = new();
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
	/// of a <see cref="ResourceDictionary"/> containing overrides for the default Uno.Material <see cref="Color"/> resources
	/// </summary>
	/// <remarks>The overrides set here should be re-defining the <see cref="Color"/> resources used by Uno.Material, not the <see cref="SolidColorBrush"/> resources</remarks>
	[Obsolete("Use Colors.OverrideSource on ThemeColors instead. This property will be removed in a future version.")]
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
			try
			{
				theme._isUpdatingColorOverrides = true;
				if (e.NewValue is string sourceUri)
				{
					var tc = theme.EnsureColors();
					tc.OverrideSource = sourceUri;
				}
				else if (theme.Colors is { } tc)
				{
					tc.OverrideDictionary = null;
				}

				theme.UpdateSource();
			}
			finally
			{
				theme._isUpdatingColorOverrides = false;
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
	/// (Optional) Gets or sets a <see cref="ResourceDictionary"/> containing overrides for the default Uno.Material <see cref="Color"/> resources
	/// </summary>
	/// <remarks>The overrides set here should be re-defining the <see cref="Color"/> resources used by Uno.Material, not the <see cref="SolidColorBrush"/> resources</remarks>
	[Obsolete("Use Colors.OverrideDictionary on ThemeColors instead. This property will be removed in a future version.")]
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
			new PropertyMetadata(null, OnColorOverrideChanged));

	private static void OnColorOverrideChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
	{
		if (d is BaseTheme { _isUpdatingColorOverrides: false } theme)
		{
			try
			{
				theme._isUpdatingColorOverrides = true;
				if (e.NewValue is ResourceDictionary dict)
				{
					var tc = theme.EnsureColors();
					tc.OverrideDictionary = dict;
				}
				else
				{
					if (theme.Colors is { } tc)
					{
						tc.OverrideDictionary = null;
					}
				}

				theme.UpdateSource();
			}
			finally
			{
				theme._isUpdatingColorOverrides = false;
			}
		}
	}
	#endregion

	#region Colors (DP)
	/// <summary>
	/// Gets or sets a <see cref="ThemeColors"/> object that groups all color-related configuration
	/// including seed colors, overrides, and the palette generation algorithm.
	/// This is the recommended way to configure theme colors.
	/// </summary>
	public ThemeColors Colors
	{
		get => (ThemeColors)GetValue(ColorsProperty);
		set => SetValue(ColorsProperty, value);
	}

	public static DependencyProperty ColorsProperty { get; } =
		DependencyProperty.Register(
			nameof(Colors),
			typeof(ThemeColors),
			typeof(BaseTheme),
			new PropertyMetadata(null, OnColorsChanged));

	private static void OnColorsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
	{
		if (d is BaseTheme theme)
		{
			if (e.OldValue is ThemeColors old)
			{
				old.SetChangedCallback(null);
			}

			if (e.NewValue is ThemeColors tc)
			{
				tc.SetChangedCallback((isStructural) =>
				{
					if (theme._isUpdatingColorOverrides)
					{
						return;
					}

					// Seed color changes use the fast path (in-place brush update).
					// Structural changes (override dictionaries) need a full rebuild.
					if (isStructural)
					{
						theme.UpdateSource();
					}
					else
					{
						theme.UpdateSeedColors();
					}
				});
			}

			if (!theme._isUpdatingColorOverrides)
			{
				theme.UpdateSource();
			}
		}
	}
	#endregion

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
	/// without an explicit <see cref="ThemeColors.PrimarySeed"/> — so the
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
		RegisterInstance(this);

		if (colorOverride is { })
		{
			_baseColorOverride = colorOverride;
		}

		if (fontOverride is { })
		{
			SetFontOverrideSilently(fontOverride);
		}

		// Set once: triggers CopyFrom to populate MergedDictionaries with the URI-backed
		// base layer (shared converters/typography/colours plus theme-specific fonts/
		// thickness/palette). Hot reload of those entries is handled by Uno's own pipeline
		// — UpdateSource() below only manages the dynamic layers on top.
		Source = new Uri(DefaultStylesSource);
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

	private ThemeColors EnsureColors()
	{
		var colors = Colors;
		if (colors is null)
		{
			colors = new ThemeColors();
			Colors = colors;
		}
		return colors;
	}

	protected void UpdateSource()
	{
		System.Console.WriteLine($"[STEVE-HR] BaseTheme.UpdateSource() running on {GetType().FullName}; Colors?.OverrideDictionary.Source={Colors?.OverrideDictionary?.Source?.OriginalString ?? "<null>"}, PrimarySeed={Colors?.PrimarySeed?.ToString() ?? "<null>"}");

		// Remove only the dictionaries we appended in previous calls. The URI-backed
		// entries Uno populated via CopyFrom when Source was set stay in place; Uno's
		// own hot-reload pipeline refreshes their content directly.
		foreach (var d in _dynamicDictionaries)
		{
			MergedDictionaries.Remove(d);
		}
		_dynamicDictionaries.Clear();

		var baseSpacing = Enum.IsDefined(DefaultDensity) ? (double)DefaultDensity : 4.0;
		AddThemeDictionary(GenerateSpacingScale(baseSpacing));
		AddThemeDictionary(GenerateShapeScale(DefaultCornerRadius));
		AddThemeDictionary(GenerateDensityDefaults());

		// --- Colour + brush layer (regression fix for #1679) ---
		// The semantic brushes in SharedColors.xaml are defined as
		//   <SolidColorBrush x:Key="PrimaryBrush" Color="{StaticResource PrimaryColor}" />
		// — a StaticResource into the colour palette, which is the correct pattern for resources
		// inside ThemeDictionaries (see WinUI theme-resource guidance). For an override to reach those
		// brushes, the brushes must resolve their StaticResource in the SAME merged scope that carries
		// the override colours.
		//
		// Before #1679, UpdateSource() built a single `colors` dictionary that owned the brushes and had
		// the palette + overrides merged into it, so {StaticResource PrimaryColor} resolved to the
		// override. #1679 moved the palette + brushes into an immutable base layer (Source) and added the
		// override as a *sibling* dictionary, so the brushes kept resolving to the base palette colours —
		// overriding a colour no longer changed the rendered brush. We restore the old behaviour by
		// rebuilding the colour layer (brushes on top of palette + overrides) and layering it above the
		// immutable base so its brushes shadow the base ones and resolve to the overridden colours.
		var effectivePrimary = Colors?.PrimarySeed ?? DefaultPrimarySeed;
		var seedPalette = effectivePrimary is { } seed
			? SeedColorPaletteGenerator.Default.Generate(seed, Colors?.SecondarySeed, Colors?.TertiarySeed, UseHighFidelityColors)
			: null;
		var userOverride = Colors?.OverrideDictionary;

			var colors = new ResourceDictionary { Source = new Uri(ThemesConstants.SharedColorsResourcePath) };
			colors.MergedDictionaries.Add(new ResourceDictionary { Source = new Uri(ThemesConstants.SharedColorPaletteResourcePath) });

		if (_baseColorOverride is not null || seedPalette is not null || userOverride is not null)
		{
			if (ColorPaletteSource is { } colorPaletteSource)
			{
				// Theme-specific base palette (e.g. Simple's grayscale) — keeps non-overridden roles on the
				// theme's defaults instead of the shared palette.
				colors.MergedDictionaries.Add(new ResourceDictionary { Source = new Uri(colorPaletteSource) });
			}

			if (_baseColorOverride is { } baseColorOverride)
			{
				colors.SafeMerge(baseColorOverride);
			}

			if (seedPalette is not null)
			{
				colors.SafeMerge(seedPalette);
			}

			if (userOverride is not null)
			{
				// Re-resolve URI-backed overrides each rebuild so hot-reload edits to the underlying XAML
				// propagate; the in-memory pairs of the original instance were loaded at init time.
				colors.SafeMerge(userOverride.Source is { } src
					? new ResourceDictionary { Source = src }
					: userOverride);
			}
			System.Console.WriteLine($"[STEVE-HR] BaseTheme.UpdateSource: built dynamic colour layer (baseOverlay={_baseColorOverride is not null}, seed={seedPalette is not null}, userOverride={userOverride is not null}, themePalette={ColorPaletteSource is not null})");
		}
		else
		{
			System.Console.WriteLine($"[STEVE-HR] BaseTheme.UpdateSource: no colour customization; using immutable base layer.");
		}

		AddThemeDictionary(colors);

		if (FontOverrideDictionary is { } fontOverride)
		{
			AddThemeDictionary(fontOverride);
		}

		// Final layer: let subclasses append their own resource dictionaries
		// (e.g. a toolkit theme stacking additional control styles) on top of the
		// fully-generated token/palette/override set.
		AddThemeSpecificResources();

		// [STEVE-HR] Probe what the theme NOW resolves for representative Color vs Brush keys.
		// If PrimaryColor reflects the override but PrimaryBrush does not, the override only
		// changed the Color and the StaticResource-bound rendered brushes never updated.
		HrProbe("PrimaryColor");
		HrProbe("PrimaryBrush");
		HrProbe("SurfaceColor");
		HrProbe("SurfaceBrush");
		HrProbe("BackgroundColor");
		HrProbe("BackgroundBrush");
	}

	// [STEVE-HR] Resolve a key through this theme dictionary (theme-aware) and log its effective value.
	private void HrProbe(string key)
	{
		try
		{
			if (TryGetValue(key, out var v))
			{
				System.Console.WriteLine($"[STEVE-HR]   theme resolves '{key}' = {v}");
			}
			else
			{
				System.Console.WriteLine($"[STEVE-HR]   theme resolves '{key}' = <not found>");
			}
		}
		catch (Exception ex)
		{
			System.Console.WriteLine($"[STEVE-HR]   theme resolves '{key}' -> error {ex.GetType().Name}: {ex.Message}");
		}
	}

	/// <summary>
	/// Appends design-system- or consumer-specific resource dictionaries on top of the
	/// theme layers generated by the base implementation (spacing/shape/density scales,
	/// the seed palette, and user color/font overrides).
	/// </summary>
	/// <remarks>
	/// Called at the end of every rebuild pass — on construction, on any theme-property
	/// change, and on hot reload. Override in a subclass and call
	/// <see cref="AddThemeDictionary(ResourceDictionary)"/> for each dictionary so it
	/// participates in the dynamic lifecycle (removed and re-added on every rebuild)
	/// instead of being orphaned after the first pass. Because this runs last, resources
	/// added here resolve against the fully-overridden token and palette set.
	/// </remarks>
	protected virtual void AddThemeSpecificResources()
	{
	}

	/// <summary>
	/// Adds <paramref name="dictionary"/> to the theme's dynamic layer. Tracked so it is
	/// removed and re-added on the next rebuild; the URI-backed entries Uno populated when
	/// <c>Source</c> was set are left untouched. Intended for use from
	/// <see cref="AddThemeSpecificResources"/>.
	/// </summary>
	/// <remarks>
	/// For URI-backed dictionaries, construct a fresh <c>new ResourceDictionary { Source = … }</c>
	/// on each call so hot-reload edits to the underlying XAML propagate; reusing a cached
	/// instance would keep the values loaded at first resolution.
	/// </remarks>
	protected void AddThemeDictionary(ResourceDictionary dictionary)
	{
		MergedDictionaries.Add(dictionary);
		_dynamicDictionaries.Add(dictionary);
	}

	// [STEVE-HR] Diagnostic: dump the resolved override dictionary's theme-dictionary colors so we can
	// see whether ms-appx:///ThemeColors.xaml resolved to real values (or empty) at this point.
	private static void HrDumpOverride(ResourceDictionary dict, string label)
	{
		try
		{
			foreach (var themeKvp in dict.ThemeDictionaries)
			{
				if (themeKvp.Value is ResourceDictionary themeDict)
				{
					var c = 0;
					foreach (var kvp in themeDict)
					{
						System.Console.WriteLine($"[STEVE-HR]   override[{label}] theme['{themeKvp.Key}']['{kvp.Key}'] = {kvp.Value}");
						if (++c >= 6)
						{
							System.Console.WriteLine($"[STEVE-HR]   override[{label}] theme['{themeKvp.Key}']: ...({themeDict.Count} total)");
							break;
						}
					}
					if (c == 0)
					{
						System.Console.WriteLine($"[STEVE-HR]   override[{label}] theme['{themeKvp.Key}'] is EMPTY");
					}
				}
			}
			if (dict.ThemeDictionaries.Count == 0)
			{
				System.Console.WriteLine($"[STEVE-HR]   override[{label}] has NO theme dictionaries (top-level keys={dict.Count})");
			}
		}
		catch (Exception ex)
		{
			System.Console.WriteLine($"[STEVE-HR]   override[{label}] dump error: {ex.GetType().Name}: {ex.Message}");
		}
	}

	/// <summary>
	/// URI of the merged-pages resource dictionary for this theme. Owns the
	/// static base layer: control styles plus the design-system's default
	/// converters, typography, colours, fonts and thickness.
	/// </summary>
	protected abstract string DefaultStylesSource { get; }

	/// <summary>
	/// Optional URI of the theme's own base colour palette (e.g. Simple's grayscale
	/// <c>ColorPalette.xaml</c>) that shadows the shared palette. When set, it is included in the
	/// dynamic colour layer built by <see cref="UpdateSource"/> so that — when a colour override or seed
	/// is active — non-overridden roles keep the theme's default colours while overridden roles still
	/// flow through to the <c>StaticResource</c>-bound semantic brushes. Return <see langword="null"/>
	/// for seed-driven themes (e.g. Material) that don't ship a static palette.
	/// </summary>
	protected virtual string ColorPaletteSource => null;
}
