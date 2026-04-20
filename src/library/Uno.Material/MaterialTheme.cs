using System;
using Uno.Themes;


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
#endif

namespace Uno.Material;

/// <summary>
/// Material Theme resources including colors, fonts, layout values, and styles
/// </summary>
public class MaterialTheme : BaseTheme
{
	/// <summary>
	/// Default primary seed color that generates the standard Material color palette.
	/// When no explicit <see cref="ThemeColors.PrimarySeed"/> is set, this seed is
	/// used so that seed color generation is always active.
	/// </summary>
	protected override Color? DefaultPrimarySeed { get; } = Color.FromArgb(0xFF, 0x59, 0x46, 0xD2);
	public MaterialTheme()
	{ }

	public MaterialTheme(ResourceDictionary colorOverride = null, ResourceDictionary fontOverride = null)
		: base(colorOverride, fontOverride)
	{ }

	protected override ResourceDictionary GenerateSpecificResources()
	{
		var mergedPages = new ResourceDictionary { Source = new Uri(MaterialConstants.ResourcePaths.Version2.MergedPages) };

		var fonts = new ResourceDictionary { Source = new Uri(MaterialConstants.ResourcePaths.Common.Fonts) };

		if (FontOverrideDictionary is { } fontOverride)
		{
			fonts.SafeMerge(fontOverride);
		}

		mergedPages.MergedDictionaries.Add(fonts);
		return mergedPages;
	}
}
