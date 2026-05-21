using System;
using Microsoft.Extensions.Logging;
using Uno.Extensions;
using Uno.Logging;
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
	private static readonly ILogger _log = typeof(SimpleTheme).Log();

	/// <summary>
	/// Simple uses a hand-crafted grayscale palette by default (no seed).
	/// When a user explicitly sets <c>Colors.PrimarySeed</c>, high-fidelity
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

	private static ResourceDictionary GetSimpleColorOverride(ResourceDictionary colorOverride)
	{
		// Load the Simple color palette (overrides the default SharedColorPalette values)
		var simpleColors = new ResourceDictionary { Source = new Uri(SimpleConstants.ResourcePaths.ColorPalette) };

		if (colorOverride is { })
		{
			if (_log.IsEnabled(LogLevel.Information))
			{
				_log.LogInformation("[ThemeHR] SimpleTheme.GetSimpleColorOverride: merging ctor colorOverride ({Count} entries) into Simple palette", colorOverride.Count);
			}
			simpleColors.SafeMerge(colorOverride);
		}

		return simpleColors;
	}

	protected override ResourceDictionary GenerateSpecificResources()
	{
		if (_log.IsEnabled(LogLevel.Information))
		{
			_log.LogInformation(
				"[ThemeHR] SimpleTheme.GenerateSpecificResources on {ThemeType}#{Hash}: fontOverrideEntries={FontOverrideEntries}",
				GetType().Name, GetHashCode(),
				FontOverrideDictionary?.Count);
		}

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
