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
	: BaseTheme(colorOverride, fontOverride)
{
	/// <summary>
	/// Simple ships a hand-crafted grayscale palette as part of mergedpages.xaml and
	/// does not derive its default colours from a seed. When a consumer sets
	/// <c>Colors.PrimarySeed</c>, high-fidelity mode preserves the source chroma so
	/// low-chroma seeds stay neutral instead of being boosted by the M3 minimum-chroma
	/// floor.
	/// </summary>
	protected override Color? DefaultPrimarySeed => null;

	/// <inheritdoc />
	protected override bool UseHighFidelityColors => true;

	public SimpleTheme()
		: this(colorOverride: null, fontOverride: null)
	{
	}

	protected override string DefaultStylesSource => SimpleConstants.ResourcePaths.MergedPages;
}
