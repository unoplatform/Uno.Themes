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
	private Dictionary<string, SolidColorBrush> _originalBrushes;
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
				var collected = new Dictionary<string, SolidColorBrush>();
				CollectBrushes(appRes, collected);
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

		// Resolve seed colors from Colors property
		var effectivePrimary = Colors?.PrimarySeed;
		var effectiveSecondary = Colors?.SecondarySeed;
		var effectiveTertiary = Colors?.TertiarySeed;

		if (effectivePrimary is { } seed)
		{
			var seedPalette = SeedColorPaletteGenerator.Default.Generate(seed, effectiveSecondary, effectiveTertiary);
			colors.SafeMerge(seedPalette);
		}

		// Resolve color overrides: Colors.OverrideDictionary takes precedence
		var effectiveColorOverride = Colors?.OverrideDictionary ?? ColorOverrideDictionary;
		if (effectiveColorOverride is { } colorOverride)
		{
			colors.SafeMerge(colorOverride);
		}

		var typography = new ResourceDictionary { Source = new Uri(ThemesConstants.SharedTypographyResourcePath) };

		var mergedPages = GenerateSpecificResources();

		mergedPages.MergedDictionaries.Add(colors);
		mergedPages.MergedDictionaries.Add(converters);

		MergedDictionaries.Add(typography);
		MergedDictionaries.Add(mergedPages);

		// Update the original brush instances (held by UI elements) with new color values.
		if (_originalBrushes is { Count: > 0 })
		{
			UpdateOldBrushes(_originalBrushes, colors);
		}
	}

	/// <summary>
	/// Collects all <see cref="SolidColorBrush"/> instances from the resource tree
	/// before the dictionaries are cleared, so they can be updated in-place afterwards.
	/// </summary>
	private static void CollectBrushes(ResourceDictionary dict, Dictionary<string, SolidColorBrush> brushes)
	{
		foreach (var themeDict in dict.ThemeDictionaries.Values)
		{
			if (themeDict is ResourceDictionary themed)
			{
				CollectBrushEntries(themed, brushes);
			}
		}

		CollectBrushEntries(dict, brushes);

		foreach (var merged in dict.MergedDictionaries)
		{
			CollectBrushes(merged, brushes);
		}
	}

	private static void CollectBrushEntries(ResourceDictionary dict, Dictionary<string, SolidColorBrush> brushes)
	{
		foreach (var key in dict.Keys)
		{
			if (key is string brushKey
				&& brushKey.EndsWith("Brush")
				&& dict[brushKey] is SolidColorBrush brush
				&& TryGetColorKeyForBrush(brushKey, out _))
			{
				// Keep the first (outermost) instance — that's what UI elements resolve to
				brushes.TryAdd(brushKey, brush);
			}
		}
	}

	/// <summary>
	/// Updates the old brush instances (captured before clearing) with new color values
	/// from the rebuilt color dictionaries.
	/// </summary>
	private static void UpdateOldBrushes(Dictionary<string, SolidColorBrush> oldBrushes, ResourceDictionary newColors)
	{
		var colorMap = new Dictionary<string, Color>();
		CollectColors(newColors, colorMap);

		foreach (var (brushKey, brush) in oldBrushes)
		{
			if (TryGetColorKeyForBrush(brushKey, out var colorKey)
				&& colorMap.TryGetValue(colorKey, out var newColor)
				&& brush.Color != newColor)
			{
				brush.Color = newColor;
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

	private static void CollectColors(ResourceDictionary dict, Dictionary<string, Color> colorMap)
	{
		// Collect from theme dictionaries
		foreach (var themeDict in dict.ThemeDictionaries.Values)
		{
			if (themeDict is ResourceDictionary themed)
			{
				foreach (var key in themed.Keys)
				{
					if (key is string k && k.EndsWith("Color") && themed[k] is Color c)
					{
						colorMap[k] = c;
					}
				}
			}
		}

		// Collect from direct entries
		foreach (var key in dict.Keys)
		{
			if (key is string k && k.EndsWith("Color") && dict[k] is Color c)
			{
				colorMap[k] = c;
			}
		}

		// Recurse into merged dictionaries
		foreach (var merged in dict.MergedDictionaries)
		{
			CollectColors(merged, colorMap);
		}
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
