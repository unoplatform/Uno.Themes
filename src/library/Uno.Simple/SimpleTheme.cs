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
public class SimpleTheme : BaseTheme
{
	public SimpleTheme()
		: this(colorOverride: null, fontOverride: null)
	{
	}

	public SimpleTheme(ResourceDictionary colorOverride = null, ResourceDictionary fontOverride = null)
		: base(GetSimpleColorOverride(colorOverride), fontOverride)
	{
	}

	private static ResourceDictionary GetSimpleColorOverride(ResourceDictionary colorOverride)
	{
		// Create a color override dictionary that loads SimpleColors
		var simpleColors = new ResourceDictionary { Source = new Uri(SimpleConstants.ResourcePaths.SimpleColors) };
		
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
