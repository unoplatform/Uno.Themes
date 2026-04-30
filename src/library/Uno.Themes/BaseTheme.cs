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
public abstract partial class BaseTheme : ResourceDictionary
{
	private bool _isUpdatingColorOverrides;
	private bool _isFontOverrideMuted;
	private ResourceDictionary _baseColorOverride;
	private List<(string themeKey, string brushKey, SolidColorBrush brush)> _originalBrushes;
	private bool _isInResourceTree;
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

	#region DefaultSpacing (DP)
	/// <summary>
	/// Gets or sets the base spacing unit (in pixels). Default is 4.
	/// All spacing scale tokens (Space100, Space200, …) are computed
	/// as multiples of this value — e.g. <c>DefaultSpacing="10"</c> makes
	/// Space100=10, Space200=20, Space400=40, etc.
	/// Individual tokens can still be overridden via lightweight styling.
	/// </summary>
	public double DefaultSpacing
	{
		get => (double)GetValue(DefaultSpacingProperty);
		set => SetValue(DefaultSpacingProperty, value);
	}

	public static DependencyProperty DefaultSpacingProperty { get; } =
		DependencyProperty.Register(
			nameof(DefaultSpacing),
			typeof(double),
			typeof(BaseTheme),
			new PropertyMetadata(4.0, OnDefaultSpacingChanged));

	private static void OnDefaultSpacingChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
	{
		if (d is BaseTheme theme)
		{
			theme.UpdateSource();
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
		if (colorOverride is { })
		{
			_baseColorOverride = colorOverride;
		}

		if (fontOverride is { })
		{
			SetFontOverrideSilently(fontOverride);
		}

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
		// Only capture brush references after this theme has been added to the app's
		// resource tree. During XAML init, UpdateSource() is called from the constructor
		// and DP setters before the theme is in the tree — those calls create transient
		// brush generations that the UI never resolves to. The brushes that matter are
		// the ones present when the first runtime change occurs (post-init).
		if (_originalBrushes == null)
		{
			if (!_isInResourceTree)
			{
				_isInResourceTree = IsInResourceTree();
			}

			if (_isInResourceTree && Application.Current?.Resources is { } appRes)
			{
				var collected = new List<(string themeKey, string brushKey, SolidColorBrush brush)>();
				CollectBrushes(appRes, null, collected);
				if (collected.Count > 0)
				{
					_originalBrushes = collected;
				}
			}
		}

#if !HAS_UNO
		Source = null;
#endif
		ThemeDictionaries.Clear();
		MergedDictionaries.Clear();
		this.Clear();

		var converters = new ResourceDictionary { Source = new Uri(ThemesConstants.ConverterResourcePath) };
		var colors = new ResourceDictionary { Source = new Uri(ThemesConstants.SharedColorsResourcePath) };

		colors.MergedDictionaries.Add(new ResourceDictionary { Source = new Uri(ThemesConstants.SharedColorPaletteResourcePath) });

		// Theme-specific base colors (e.g. SimpleTheme's grayscale palette) are merged
		// before the seed so that seed-generated colors take precedence.
		if (_baseColorOverride is { } baseColorOverride)
		{
			colors.SafeMerge(baseColorOverride);
		}

		// Resolve seed colors from Colors property, falling back to theme default
		var effectivePrimary = Colors?.PrimarySeed ?? DefaultPrimarySeed;
		var effectiveSecondary = Colors?.SecondarySeed;
		var effectiveTertiary = Colors?.TertiarySeed;

		if (effectivePrimary is { } seed)
		{
			var seedPalette = SeedColorPaletteGenerator.Default.Generate(seed, effectiveSecondary, effectiveTertiary, UseHighFidelityColors);
			colors.SafeMerge(seedPalette);
		}

		// Explicit user overrides from Colors.OverrideDictionary take highest precedence
		if (Colors?.OverrideDictionary is { } userOverride)
		{
			colors.SafeMerge(userOverride);
		}

		var typography = new ResourceDictionary { Source = new Uri(ThemesConstants.SharedTypographyResourcePath) };

		// Generate spacing, shape, and density scales from code
		var spacing = GenerateSpacingScale(DefaultSpacing);
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

		// Track new brush instances created by the rebuild so that UI elements
		// that resolve {ThemeResource} after this point also get updated.
		if (_originalBrushes is not null)
		{
			var existingBrushes = new HashSet<SolidColorBrush>(_originalBrushes.ConvertAll(e => e.brush));
			var newInstances = new List<(string themeKey, string brushKey, SolidColorBrush brush)>();
			CollectBrushes(this, null, newInstances);
			foreach (var entry in newInstances)
			{
				if (existingBrushes.Add(entry.brush))
				{
					_originalBrushes.Add(entry);
				}
			}
		}

		// Also capture from the full app resource tree (catches instances resolved
		// by UI elements via {ThemeResource} that aren't in the BaseTheme itself).
		if (_originalBrushes is not null && Application.Current?.Resources is { } appRes2)
		{
			var existingBrushes2 = new HashSet<SolidColorBrush>(_originalBrushes.ConvertAll(e => e.brush));
			var appInstances = new List<(string themeKey, string brushKey, SolidColorBrush brush)>();
			CollectBrushes(appRes2, null, appInstances);
			foreach (var entry in appInstances)
			{
				if (existingBrushes2.Add(entry.brush))
				{
					_originalBrushes.Add(entry);
				}
			}
		}

		// Update the original brush instances (held by UI elements) with new color values.
		if (_originalBrushes is { Count: > 0 })
		{
			UpdateOldBrushes(_originalBrushes, colors);
		}
	}

	protected abstract ResourceDictionary GenerateSpecificResources();
}
