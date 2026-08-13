using System;
using Uno.Themes;


#if WinUI
using Microsoft.UI.Xaml;
using Windows.UI;
#else
using Windows.UI;
using Windows.UI.Xaml;
#endif

namespace Uno.Simple;

/// <summary>
/// Simple Theme resources including colors, fonts, layout values, and styles.
/// </summary>
public class SimpleTheme(ResourceDictionary colorOverride = null, ResourceDictionary fontOverride = null)
	: BaseTheme(GetSimpleColorOverride(colorOverride), fontOverride)
{
	/// <summary>
	/// Simple uses a hand-crafted grayscale palette by default (no seed).
	/// When a user explicitly sets <c>Colors.PrimarySeed</c>, the default
	/// <see cref="ThemeColors.PreserveSeedColor"/> behavior keeps the generated
	/// palette faithful to that seed.
	/// </summary>
	protected override Color? DefaultPrimarySeed => null;

	public SimpleTheme()
		: this(colorOverride: null, fontOverride: null)
	{
	}

	private static ResourceDictionary GetSimpleColorOverride(ResourceDictionary colorOverride)
	{
		// Load the Simple color palette (overrides the default SharedColorPalette values)
		var simpleColors = new ResourceDictionary { Source = new Uri(SimpleConstants.ResourcePaths.ColorPalette) };

		if (colorOverride is { })
		{
			simpleColors.SafeMerge(colorOverride);
		}

		return simpleColors;
	}

	protected override string DefaultStylesSource => SimpleConstants.ResourcePaths.MergedPages;

	protected override void AddThemeSpecificResources()
	{
		base.AddThemeSpecificResources();

		// Base fonts and thickness ship in the Source bundle (BaseDictionaries.xaml); only a
		// consumer-supplied font override is layered dynamically on top to shadow them.
		if (FontOverrideDictionary is { } fontOverride)
		{
			AddThemeDictionary(fontOverride);
		}
	}
}
