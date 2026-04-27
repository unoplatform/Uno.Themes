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

public abstract partial class BaseTheme
{
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

		var effectivePrimary = Colors?.PrimarySeed ?? DefaultPrimarySeed;
		var effectiveSecondary = Colors?.SecondarySeed;
		var effectiveTertiary = Colors?.TertiarySeed;

		if (effectivePrimary is not { } seed)
		{
			// No seed available (no explicit seed and no theme default) — full rebuild
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
}
