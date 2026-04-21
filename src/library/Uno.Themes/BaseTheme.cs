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
public abstract class BaseTheme : ResourceDictionary
{
	private bool _isColorOverrideMuted;
	private bool _isFontOverrideMuted;
	private ResourceDictionary _baseColorOverride;
	private List<(string themeKey, string brushKey, SolidColorBrush brush)> _trackedBrushes;
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
		if (d is BaseTheme theme && e.NewValue is string sourceUri)
		{
			try
			{
				theme._isColorOverrideMuted = true;
				var tc = theme.EnsureColors();
				tc.OverrideSource = sourceUri;
			}
			finally
			{
				theme._isColorOverrideMuted = false;
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
		if (d is BaseTheme { _isColorOverrideMuted: false } theme)
		{
			try
			{
				theme._isColorOverrideMuted = true;
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
					else
					{
						theme.UpdateSource();
					}
				}
			}
			finally
			{
				theme._isColorOverrideMuted = false;
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
				tc.SetChangedCallback((_) =>
				{
					if (!theme._isColorOverrideMuted)
					{
						theme.UpdateSource();
					}
				});
			}

			if (!theme._isColorOverrideMuted)
			{
				theme.UpdateSource();
			}
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
		// Before clearing, capture existing brush instances so we can
		// patch them after the rebuild. UI elements hold references to
		// these instances via {ThemeResource}; without this step those
		// references would become stale.
		if (_trackedBrushes == null && MergedDictionaries.Count > 0)
		{
			var collected = new List<(string, string, SolidColorBrush)>();
			CollectBrushes(this, null, collected);
			if (collected.Count > 0)
			{
				_trackedBrushes = collected;
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
			var seedPalette = SeedColorPaletteGenerator.Default.Generate(seed, effectiveSecondary, effectiveTertiary);
			colors.SafeMerge(seedPalette);
		}

		// Explicit user overrides from Colors.OverrideDictionary take highest precedence
		if (Colors?.OverrideDictionary is { } userOverride)
		{
			colors.SafeMerge(userOverride);
		}

		var typography = new ResourceDictionary { Source = new Uri(ThemesConstants.SharedTypographyResourcePath) };

		var mergedPages = GenerateSpecificResources();

		mergedPages.MergedDictionaries.Add(colors);
		mergedPages.MergedDictionaries.Add(converters);

		MergedDictionaries.Add(typography);
		MergedDictionaries.Add(mergedPages);

		// Patch tracked brush instances with new color values so that
		// UI elements holding old references see the updates.
		SyncTrackedBrushes();
	}

	/// <summary>
	/// Updates previously-captured brush instances with the colors from the
	/// rebuilt dictionaries. Matching is done by resource key — no string
	/// derivation or suffix stripping needed.
	/// </summary>
	private void SyncTrackedBrushes()
	{
		if (_trackedBrushes is not { Count: > 0 })
		{
			return;
		}

		// Index new brush instances from the rebuilt dictionary
		var newBrushes = new Dictionary<(string themeKey, string brushKey), SolidColorBrush>();
		IndexBrushes(this, null, newBrushes);

		// Update each tracked instance from the corresponding new instance
		foreach (var (themeKey, brushKey, oldBrush) in _trackedBrushes)
		{
			if (newBrushes.TryGetValue((themeKey, brushKey), out var newBrush)
				&& !ReferenceEquals(oldBrush, newBrush)
				&& oldBrush.Color != newBrush.Color)
			{
				oldBrush.Color = newBrush.Color;
			}
		}

		// Track new instances so future syncs also update them
		var known = new HashSet<SolidColorBrush>(_trackedBrushes.ConvertAll(e => e.brush));
		foreach (var ((themeKey, brushKey), brush) in newBrushes)
		{
			if (known.Add(brush))
			{
				_trackedBrushes.Add((themeKey, brushKey, brush));
			}
		}
	}

	/// <summary>
	/// Builds a dictionary of brush instances keyed by (themeKey, brushKey)
	/// from the given resource dictionary tree.
	/// </summary>
	private static void IndexBrushes(
		ResourceDictionary dict,
		string themeKey,
		Dictionary<(string themeKey, string brushKey), SolidColorBrush> result)
	{
		foreach (var kvp in dict.ThemeDictionaries)
		{
			if (kvp.Value is ResourceDictionary themed)
			{
				IndexBrushes(themed, kvp.Key as string, result);
			}
		}

		foreach (var key in dict.Keys)
		{
			if (key is string k && dict[k] is SolidColorBrush brush)
			{
				result.TryAdd((themeKey, k), brush);
			}
		}

		foreach (var merged in dict.MergedDictionaries)
		{
			IndexBrushes(merged, themeKey, result);
		}
	}

	/// <summary>
	/// Recursively collects all <see cref="SolidColorBrush"/> instances from the
	/// resource dictionary tree, tagging each with its ThemeDictionary key.
	/// </summary>
	private static void CollectBrushes(
		ResourceDictionary dict,
		string themeKey,
		List<(string themeKey, string brushKey, SolidColorBrush brush)> result)
	{
		foreach (var kvp in dict.ThemeDictionaries)
		{
			if (kvp.Value is ResourceDictionary themed)
			{
				CollectBrushes(themed, kvp.Key as string, result);
			}
		}

		foreach (var key in dict.Keys)
		{
			if (key is string k && dict[k] is SolidColorBrush brush)
			{
				result.Add((themeKey, k, brush));
			}
		}

		foreach (var merged in dict.MergedDictionaries)
		{
			CollectBrushes(merged, themeKey, result);
		}
	}

	protected abstract ResourceDictionary GenerateSpecificResources();
}
