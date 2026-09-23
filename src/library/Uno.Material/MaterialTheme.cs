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

	/// <summary>
	/// The per-control font family alias keys Material v2 declares — StaticResource aliases that
	/// snapshot at parse time and therefore need regenerating for a runtime
	/// <see cref="BaseTheme.DefaultFontFamily"/> change to reach the control templates.
	/// Keep in sync with the <c>*FontFamily</c> lightweight-styling keys under
	/// <c>Styles/Controls/v2</c> that alias the root token or a type-scale slot.
	/// </summary>
	internal override string[] FontFamilyAliasKeys { get; } =
	{
		"CheckBoxFontFamily",
		"DatePickerFlyoutPresenterFontFamily",
		"FilledPasswordBoxFontFamily",
		"FilledTextBoxFontFamily",
		"HyperlinkButtonFontFamily",
		"OutlinedPasswordBoxFontFamily",
		"OutlinedTextBoxFontFamily",
		"RadioButtonFontFamily",
		"RatingControlCaptionFontFamily",
		"SecondaryRatingControlCaptionFontFamily",
		"SliderFontFamily",
		"TextToggleButtonFontFamily",
	};
}
