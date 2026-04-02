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
			theme.ColorOverrideDictionary = new ResourceDictionary() { Source = new Uri(sourceUri) };
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
			theme.UpdateSource();
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
				tc.SetChangedCallback(() =>
				{
					if (!theme._isColorOverrideMuted)
					{
						theme.UpdateSeedColors();
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

	public BaseTheme() : this(colorOverride: null, fontOverride: null)
	{

	}

	public BaseTheme(ResourceDictionary colorOverride = null, ResourceDictionary fontOverride = null)
	{
		if (colorOverride is { })
		{
			SetColorOverrideSilently(colorOverride);
		}

		if (fontOverride is { })
		{
			SetFontOverrideSilently(fontOverride);
		}

		UpdateSource();
	}

	private void SetColorOverrideSilently(ResourceDictionary colorOverride)
	{
		try
		{
			_isColorOverrideMuted = true;
			ColorOverrideDictionary = colorOverride;
		}
		finally
		{
			_isColorOverrideMuted = false;
		}
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
		if (ColorOverrideDictionary is { } baseColorOverride)
		{
			colors.SafeMerge(baseColorOverride);
		}

		// Resolve seed colors from Colors property
		var effectivePrimary = Colors?.PrimarySeed;
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

	/// <summary>
	/// Fast path for seed color changes: updates existing brush Colors in-place
	/// without clearing/rebuilding the resource tree. This preserves brush instance
	/// identity so that UI elements holding references to these brushes see the
	/// changes immediately.
	/// Falls back to <see cref="UpdateSource"/> if the tree hasn't been built yet.
	/// </summary>
	private void UpdateSeedColors()
	{
		// If the tree hasn't been built yet, do a full rebuild
		if (MergedDictionaries.Count == 0)
		{
			UpdateSource();
			return;
		}

		var effectivePrimary = Colors?.PrimarySeed;
		var effectiveSecondary = Colors?.SecondarySeed;
		var effectiveTertiary = Colors?.TertiarySeed;

		if (effectivePrimary is not { } seed)
		{
			// Seed was cleared — need a full rebuild to restore defaults
			UpdateSource();
			return;
		}

		// Generate the new seed palette
		var seedPalette = SeedColorPaletteGenerator.Default.Generate(seed, effectiveSecondary, effectiveTertiary);

		// Build a color map from the seed palette (theme-keyed)
		var colorsByTheme = new Dictionary<string, Dictionary<string, Color>>();
		CollectThemedColors(seedPalette, colorsByTheme);

		// Walk all brushes in the current resource tree and update their Colors
		if (Application.Current?.Resources is { } appRes)
		{
			UpdateBrushColorsInPlace(appRes, colorsByTheme);
		}
	}

	/// <summary>
	/// Walks the resource tree and updates SolidColorBrush.Color in-place
	/// for brushes whose corresponding color key exists in the seed palette.
	/// </summary>
	private static void UpdateBrushColorsInPlace(
		ResourceDictionary dict,
		Dictionary<string, Dictionary<string, Color>> colorsByTheme)
	{
		foreach (var kvp in dict.ThemeDictionaries)
		{
			if (kvp.Value is ResourceDictionary themed && kvp.Key is string themeKey
				&& colorsByTheme.TryGetValue(themeKey, out var themeColorMap))
			{
				UpdateBrushEntriesInPlace(themed, themeColorMap);
			}
		}

		// Non-themed brushes: use any available color map as fallback
		if (colorsByTheme.TryGetValue(string.Empty, out var defaultMap) && defaultMap.Count > 0)
		{
			UpdateBrushEntriesInPlace(dict, defaultMap);
		}

		foreach (var merged in dict.MergedDictionaries)
		{
			UpdateBrushColorsInPlace(merged, colorsByTheme);
		}
	}

	private static void UpdateBrushEntriesInPlace(
		ResourceDictionary dict,
		Dictionary<string, Color> colorMap)
	{
		foreach (var key in dict.Keys)
		{
			if (key is string brushKey
				&& brushKey.EndsWith("Brush")
				&& dict[brushKey] is SolidColorBrush brush
				&& TryGetColorKeyForBrush(brushKey, out var colorKey)
				&& colorMap.TryGetValue(colorKey, out var newColor)
				&& brush.Color != newColor)
			{
				brush.Color = newColor;
			}
		}
	}

	/// <summary>
	/// Collects all <see cref="SolidColorBrush"/> instances from the resource tree
	/// before the dictionaries are cleared, so they can be updated in-place afterwards.
	/// Each brush is tagged with the ThemeDictionary key it came from (e.g. "Light", "Default")
	/// so it can be updated with the correct theme's color later.
	/// </summary>
	private static void CollectBrushes(
		ResourceDictionary dict,
		string currentThemeKey,
		List<(string themeKey, string brushKey, SolidColorBrush brush)> brushes)
	{
		foreach (var kvp in dict.ThemeDictionaries)
		{
			if (kvp.Value is ResourceDictionary themed)
			{
				CollectBrushEntries(themed, kvp.Key as string, brushes);
			}
		}

		CollectBrushEntries(dict, currentThemeKey, brushes);

		foreach (var merged in dict.MergedDictionaries)
		{
			CollectBrushes(merged, currentThemeKey, brushes);
		}
	}

	private static void CollectBrushEntries(
		ResourceDictionary dict,
		string themeKey,
		List<(string themeKey, string brushKey, SolidColorBrush brush)> brushes)
	{
		foreach (var key in dict.Keys)
		{
			if (key is string brushKey
				&& brushKey.EndsWith("Brush")
				&& dict[brushKey] is SolidColorBrush brush
				&& TryGetColorKeyForBrush(brushKey, out _))
			{
				brushes.Add((themeKey, brushKey, brush));
			}
		}
	}

	/// <summary>
	/// Updates all old brush instances (captured before clearing) with new color values
	/// from the rebuilt color dictionaries. Each brush is matched to its theme's colors
	/// using the theme key captured during collection.
	/// </summary>
	private static void UpdateOldBrushes(
		List<(string themeKey, string brushKey, SolidColorBrush brush)> oldBrushes,
		ResourceDictionary newColors)
	{
		var colorsByTheme = new Dictionary<string, Dictionary<string, Color>>();
		CollectThemedColors(newColors, colorsByTheme);

		foreach (var (themeKey, brushKey, brush) in oldBrushes)
		{
			if (!TryGetColorKeyForBrush(brushKey, out var colorKey))
			{
				continue;
			}

			// Look up the color from the same theme dict the brush came from
			if (themeKey is not null
				&& colorsByTheme.TryGetValue(themeKey, out var themedMap)
				&& themedMap.TryGetValue(colorKey, out var themedColor))
			{
				if (brush.Color != themedColor)
				{
					brush.Color = themedColor;
				}
			}
			// Fall back to non-themed colors
			else if (colorsByTheme.TryGetValue(string.Empty, out var defaultMap)
				&& defaultMap.TryGetValue(colorKey, out var defaultColor))
			{
				if (brush.Color != defaultColor)
				{
					brush.Color = defaultColor;
				}
			}
		}
	}

	/// <summary>
	/// Collects color values grouped by ThemeDictionary key.
	/// Non-themed colors use <see cref="string.Empty"/> as the key.
	/// </summary>
	private static void CollectThemedColors(ResourceDictionary dict, Dictionary<string, Dictionary<string, Color>> colorsByTheme)
	{
		foreach (var kvp in dict.ThemeDictionaries)
		{
			if (kvp.Value is ResourceDictionary themed && kvp.Key is string themeKey)
			{
				if (!colorsByTheme.TryGetValue(themeKey, out var map))
				{
					map = new Dictionary<string, Color>();
					colorsByTheme[themeKey] = map;
				}

				CollectColorEntries(themed, map);
			}
		}

		if (!colorsByTheme.TryGetValue(string.Empty, out var directMap))
		{
			directMap = new Dictionary<string, Color>();
			colorsByTheme[string.Empty] = directMap;
		}

		CollectColorEntries(dict, directMap);

		foreach (var merged in dict.MergedDictionaries)
		{
			CollectThemedColors(merged, colorsByTheme);
		}
	}

	private static void CollectColorEntries(ResourceDictionary dict, Dictionary<string, Color> colorMap)
	{
		foreach (var key in dict.Keys)
		{
			if (key is string k && k.EndsWith("Color") && dict[k] is Color c)
			{
				colorMap[k] = c;
			}
		}
	}

	/// <summary>
	/// Checks whether this theme dictionary is reachable from <see cref="Application.Current.Resources"/>.
	/// During XAML init the theme is constructed and configured before being added to the tree,
	/// so this returns false until the XAML parser actually adds it.
	/// </summary>
	private bool IsInResourceTree()
	{
		if (Application.Current?.Resources is not { } res) return false;
		return IsReachableFrom(res);
	}

	private bool IsReachableFrom(ResourceDictionary dict)
	{
		if (ReferenceEquals(dict, this)) return true;

		foreach (var merged in dict.MergedDictionaries)
		{
			if (IsReachableFrom(merged)) return true;
		}

		foreach (var themeDict in dict.ThemeDictionaries.Values)
		{
			if (themeDict is ResourceDictionary rd && IsReachableFrom(rd)) return true;
		}

		return false;
	}

	/// <summary>
	/// Derives the color resource key from a brush resource key.
	/// E.g. "PrimaryBrush" → "PrimaryColor", "PrimaryHoverBrush" → "PrimaryColor",
	/// "OnSurfaceVariantDisabledBrush" → "OnSurfaceVariantColor".
	/// </summary>
	private static bool TryGetColorKeyForBrush(string brushKey, out string colorKey)
	{
		colorKey = null;

		if (!brushKey.EndsWith("Brush"))
		{
			return false;
		}

		// Strip the "Brush" suffix
		var baseName = brushKey.Substring(0, brushKey.Length - 5); // "PrimaryHover", "Primary", etc.

		// Known state suffixes in order of longest first
		string[] stateSuffixes = { "Disabled", "Selected", "Dragged", "Pressed", "Focused", "Medium", "Hover", "Low" };

		foreach (var suffix in stateSuffixes)
		{
			if (baseName.EndsWith(suffix))
			{
				baseName = baseName.Substring(0, baseName.Length - suffix.Length);
				break;
			}
		}

		colorKey = baseName + "Color";
		return true;
	}

	protected abstract ResourceDictionary GenerateSpecificResources();
}
