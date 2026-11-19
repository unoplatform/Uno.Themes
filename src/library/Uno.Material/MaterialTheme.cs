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

	protected override string SemanticStylesSource => MaterialConstants.ResourcePaths.Version2.SemanticStyles;

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

	/// <summary>
	/// The radii Material v2 caps by the size of the control they sit on (unoplatform/Uno.Themes#1747).
	/// Each cap is at least the value at the default base of 4, so the default look is unchanged.
	/// Keep in sync with the <c>MaterialShape*</c> references under <c>Styles/Controls/v2</c>.
	/// </summary>
	internal override CappedRadius[] CappedRadii { get; } =
	{
		// 40px button pill: half the height.
		new("MaterialShapeButton", 5, 20),
		// 18px check box: a quarter of its side, so it never reads as a RadioButton.
		new("MaterialShapeCheckBox", 0.5, 4.5),
		// 56px, 40px and 96px FABs: 0.3 × the side, so they stay rounded squares.
		new("MaterialShapeFab", 4, 16.8),
		new("MaterialShapeFabSmall", 3, 12),
		new("MaterialShapeFabLarge", 7, 28.8),
		// ContentDialog: content stays inside the corner.
		new("MaterialShapeDialog", 7, 32),
		// 56px NavigationViewItem pill: half the height.
		new("MaterialShapeNavigationItem", 7, 28),
		// 32px compact NavigationViewItem and pane buttons: half the height.
		new("MaterialShapeNavigationItemCompact", 4, 16),
		// 32px ToggleSwitch track: half the height.
		new("MaterialShapeSwitchTrack", 4, 16),
		// Slider fill and NavigationView selection indicator: thin bars that keep their rounded ends.
		new("MaterialShapeSliderFill", 1, 4),
		new("MaterialShapeIndicator", 0.5, 2),
	};
}
