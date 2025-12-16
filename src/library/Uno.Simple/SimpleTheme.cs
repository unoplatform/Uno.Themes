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
	{ }

	public SimpleTheme(ResourceDictionary colorOverride = null, ResourceDictionary fontOverride = null)
		: base(colorOverride, fontOverride)
	{ }

	protected override ResourceDictionary GenerateSpecificResources()
	{
		var mergedPages = new ResourceDictionary { Source = new Uri(SimpleConstants.ResourcePaths.Version2.MergedPages) };

		var fonts = new ResourceDictionary { Source = new Uri(SimpleConstants.ResourcePaths.Common.Fonts) };
		var colors = new ResourceDictionary { Source = new Uri(SimpleConstants.ResourcePaths.Version2.MaterialColors) };
		colors.MergedDictionaries.Add(new ResourceDictionary { Source = new Uri(SimpleConstants.ResourcePaths.Version2.ColorPalette) });

		if (FontOverrideDictionary is { } fontOverride)
		{
			fonts.SafeMerge(fontOverride);
		}

		mergedPages.MergedDictionaries.Add(fonts);
		mergedPages.MergedDictionaries.Add(colors);
		return mergedPages;
	}
}
