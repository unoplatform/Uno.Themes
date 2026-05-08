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
public class SimpleTheme : BaseTheme
{
	/// <summary>
	/// Simple uses a hand-crafted grayscale palette by default (no seed).
	/// When a user explicitly sets <see cref="BaseTheme.PrimarySeedColor"/>, high-fidelity
	/// mode preserves the source chroma so low-chroma seeds stay neutral
	/// instead of being boosted by the M3 minimum-chroma floor.
	/// </summary>
	protected override Color? DefaultPrimarySeed => null;

	/// <inheritdoc />
	protected override bool UseHighFidelityColors => true;

	public SimpleTheme()
		: this(colorOverride: null, fontOverride: null)
	{
	}

	public SimpleTheme(ResourceDictionary colorOverride = null, ResourceDictionary fontOverride = null)
		: base(colorOverride, fontOverride)
	{
	}

	protected override ResourceDictionary GetDefaultColorPalette() =>
		new ResourceDictionary { Source = new Uri(SimpleConstants.ResourcePaths.ColorPalette) };

	protected override ResourceDictionary GenerateSpecificResources()
	{
		var mergedPages = new ResourceDictionary { Source = new Uri(SimpleConstants.ResourcePaths.MergedPages) };

		var thickness = new ResourceDictionary { Source = new Uri(SimpleConstants.ResourcePaths.Thickness) };
		mergedPages.MergedDictionaries.Add(thickness);

		var fonts = new ResourceDictionary { Source = new Uri(SimpleConstants.ResourcePaths.Common.Fonts) };

		if (FontOverrideDictionary is { } fontOverride)
		{
			fonts.SafeMerge(fontOverride);
		}

		mergedPages.MergedDictionaries.Add(fonts);
		return mergedPages;
	}
}
