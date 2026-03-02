using System;
using Uno.Themes;


#if WinUI
using Microsoft.UI;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Media;
#else
using Windows.UI.Xaml;
#endif

namespace Uno.Simple;

/// <summary>
/// Simple Theme resources including colors, fonts, layout values, and styles
/// </summary>
public class SimpleTheme(ResourceDictionary colorOverride = null, ResourceDictionary fontOverride = null)
	: BaseTheme(GetSimpleColorOverride(colorOverride), fontOverride)
{
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

	protected override ResourceDictionary GenerateSpecificResources()
	{
		var mergedPages = new ResourceDictionary { Source = new Uri(SimpleConstants.ResourcePaths.MergedPages) };

		var fonts = new ResourceDictionary { Source = new Uri(SimpleConstants.ResourcePaths.Common.Fonts) };

		if (FontOverrideDictionary is { } fontOverride)
		{
			fonts.SafeMerge(fontOverride);
		}

		mergedPages.MergedDictionaries.Add(fonts);
		return mergedPages;
	}
}
