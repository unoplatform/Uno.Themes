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
	/// Material uses the default Material color palette (SharedColorPalette.xaml)
	/// when no seed is set. Seed color generation only runs when a user
	/// explicitly sets <see cref="ThemeColors.PrimarySeed"/>.
	/// </summary>
	protected override Color? DefaultPrimarySeed => null;
	public MaterialTheme()
	{ }

	public MaterialTheme(ResourceDictionary colorOverride = null, ResourceDictionary fontOverride = null)
		: base(colorOverride, fontOverride)
	{ }

	protected override string DefaultStylesSource => MaterialConstants.ResourcePaths.Version2.MergedPages;

	protected override void AddThemeSpecificResources()
	{
		base.AddThemeSpecificResources();

		// Base fonts ship in the Source bundle (BaseDictionaries.xaml); only a
		// consumer-supplied override is layered dynamically on top to shadow them.
		if (FontOverrideDictionary is { } fontOverride)
		{
			AddThemeDictionary(fontOverride);
		}
	}
}
