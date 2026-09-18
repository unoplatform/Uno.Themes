using System;
using Uno.Themes;
using Uno.Themes.Helpers;

#if WinUI
using Microsoft.UI.Xaml;
using Windows.UI;
#else
using Windows.UI;
using Windows.UI.Xaml;
#endif

namespace Uno.Cupertino;

/// <summary>
/// Cupertino Theme resources including colors, fonts, layout values, and styles.
/// </summary>
public class CupertinoTheme(ResourceDictionary colorOverride = null, ResourceDictionary fontOverride = null)
	: BaseTheme(GetCupertinoColorOverride(colorOverride), fontOverride)
{
	/// <summary>
	/// Cupertino uses Apple's hand-crafted system palette by default (no seed).
	/// When a user explicitly sets <c>Colors.PrimarySeed</c>, the generated palette
	/// replaces it, following the M3 container recipe rather than Apple's tinted fills.
	/// </summary>
	protected override Color? DefaultPrimarySeed => null;

	/// <summary>
	/// Initializes a new instance of the <see cref="CupertinoTheme"/> class with no overrides.
	/// </summary>
	public CupertinoTheme()
		: this(colorOverride: null, fontOverride: null)
	{
	}

	private static ResourceDictionary GetCupertinoColorOverride(ResourceDictionary colorOverride)
	{
		// Load the Cupertino color palette (overrides the default SharedColorPalette values)
		var cupertinoColors = new ResourceDictionary { Source = new Uri(CupertinoConstants.ColorPalette) };

		if (colorOverride is { })
		{
			cupertinoColors.SafeMerge(colorOverride);
		}

		return cupertinoColors;
	}

	/// <inheritdoc />
	protected override string DefaultStylesSource => CupertinoConstants.MergedPages;

	// Created once and kept for the lifetime of the theme: consumers hold {ThemeResource Cupertino*Brush}
	// references to these instances, so they are rewritten in place, never replaced (see SemanticBrushUpdater).
	private ResourceDictionary _cupertinoBrushes;

	/// <inheritdoc />
	protected override void AddThemeSpecificResources()
	{
		_cupertinoBrushes ??= new ResourceDictionary { Source = new Uri(CupertinoConstants.Brushes) };
		SemanticBrushUpdater.Apply(_cupertinoBrushes, ColorLayers, CupertinoConstants.BrushColorKeys);
		AddThemeDictionary(_cupertinoBrushes);
	}

	/// <summary>
	/// The font family alias keys Cupertino declares — StaticResource aliases that snapshot at parse
	/// time and therefore need regenerating for a runtime <see cref="BaseTheme.DefaultFontFamily"/>
	/// change to reach the control templates. Only keys declared inside <c>ThemeDictionaries</c> can be
	/// listed: a generated layer is a merged dictionary and cannot shadow a top-level key.
	/// </summary>
	internal override string[] FontFamilyAliasKeys { get; } =
	{
		"CupertinoFontFamily",
	};
}
