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

	protected override string SemanticStylesSource => SimpleConstants.ResourcePaths.SemanticStyles;

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

	/// <summary>
	/// The radii Simple caps by the size of the control they sit on (unoplatform/Uno.Themes#1747).
	/// Each cap is at least the value at the default base of 4, so the default look is unchanged.
	/// Keep in sync with the <c>SimpleShape*</c> references under <c>Styles/Controls</c>.
	/// </summary>
	internal override CappedRadius[] CappedRadii { get; } =
	{
		// 40px controls (buttons, inputs, pickers, list items) and tooltips: a quarter of the height.
		new("SimpleShapeControl", 2, 10),
		// 40px ComboBoxItem, which follows Radius100.
		new("SimpleShapeItem", 1, 10),
		// 36px menu items: a quarter of the height.
		new("SimpleShapeMenuItem", 2, 9),
		// Flyouts, menus, dialogs, expanders: a fixed maximum, twice the default, so content stays inside the corner.
		new("SimpleShapeContainer", 2, 16),
		// 16px check box: a quarter of its side, so it never reads as a RadioButton.
		new("SimpleShapeCheckBox", 1, 4),
		// 32px and 24px square avatars: a quarter of the side.
		new("SimpleShapeAvatar", 2, 8),
		new("SimpleShapeAvatarSmall", 1, 6),
		// 40px AutoSuggestBox pill: half the height.
		new("SimpleShapePill", 5, 20),
		// 24px ToggleSwitch track: half the height.
		new("SimpleShapeSwitchTrack", 3, 12),
		// CalendarView and date flyouts: the header text stays inside the corner.
		new("SimpleShapeCalendar", 4, 24),
		// 4px slider track and progress bar, 8px slider fill: half the thickness.
		new("SimpleShapeTrack", 0.5, 2),
		new("SimpleShapeSliderFill", 1, 4),
	};
}
