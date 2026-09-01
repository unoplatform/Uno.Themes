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
	/// <see cref="SeedColorMode.Fidelity"/> mode keeps the generated
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

	/// <summary>
	/// The font family alias keys Simple declares — StaticResource aliases that snapshot at parse
	/// time and therefore need regenerating for a runtime <see cref="BaseTheme.DefaultFontFamily"/>
	/// change to reach the control templates. Keep in sync with the <c>*FontFamily</c>
	/// lightweight-styling keys under <c>Styles/Controls</c>.
	/// </summary>
	internal override string[] FontFamilyAliasKeys { get; } =
	{
		"SimpleButtonFontFamily",
		"SimpleToggleButtonFontFamily",
		"DatePickerFlyoutPresenterFontFamily",
	};
}
