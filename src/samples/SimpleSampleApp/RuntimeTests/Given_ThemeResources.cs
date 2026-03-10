using Microsoft.VisualStudio.TestTools.UnitTesting;
using Uno.Themes.Samples.RuntimeTests.Helpers;

namespace Uno.Themes.Samples.RuntimeTests;

/// <summary>
/// Validates that key ThemeResource chains resolve correctly across all Simple theme controls.
/// Tests a representative set of brush resources that back lightweight styling overrides.
/// </summary>
[TestClass]
public class Given_ThemeResources
{
	// --- Expander ---

	[TestMethod]
	[DataRow("SimpleExpanderHeaderBackground")]
	[DataRow("SimpleExpanderHeaderForeground")]
	[DataRow("SimpleExpanderHeaderBorderBrush")]
	[DataRow("SimpleExpanderChevronForeground")]
	[DataRow("SimpleExpanderContentBackground")]
	[DataRow("SimpleExpanderContentBorderBrush")]
	[DataRow("SimpleExpanderContentForeground")]
	public void Expander_BrushResource_Resolves(string resourceKey)
	{
		StyleTestHelper.AssertBrushResource(resourceKey);
	}

	// --- Button ---

	[TestMethod]
	[DataRow("SimplePrimaryButtonForeground")]
	[DataRow("SimplePrimaryButtonBackground")]
	[DataRow("SimplePrimaryButtonBorderBrush")]
	[DataRow("SimpleNeutralButtonForeground")]
	[DataRow("SimpleNeutralButtonBackground")]
	[DataRow("SimpleNeutralButtonBorderBrush")]
	[DataRow("SimpleSubtleButtonForeground")]
	[DataRow("SimpleSubtleButtonBackground")]
	[DataRow("SimpleDangerPrimaryButtonForeground")]
	[DataRow("SimpleDangerPrimaryButtonBackground")]
	[DataRow("SimpleDangerSubtleButtonForeground")]
	[DataRow("SimpleDangerSubtleButtonBackground")]
	public void Button_BrushResource_Resolves(string resourceKey)
	{
		StyleTestHelper.AssertBrushResource(resourceKey);
	}

	// --- CheckBox ---

	[TestMethod]
	[DataRow("SimpleCheckBoxForeground")]
	[DataRow("SimpleCheckBoxCheckedBackground")]
	[DataRow("SimpleCheckBoxCheckedBorderBrush")]
	public void CheckBox_BrushResource_Resolves(string resourceKey)
	{
		StyleTestHelper.AssertBrushResource(resourceKey);
	}

	// --- RadioButton ---

	[TestMethod]
	[DataRow("SimpleRadioButtonForeground")]
	[DataRow("SimpleRadioButtonCheckedBackground")]
	[DataRow("SimpleRadioButtonCheckedBorderBrush")]
	public void RadioButton_BrushResource_Resolves(string resourceKey)
	{
		StyleTestHelper.AssertBrushResource(resourceKey);
	}

	// --- TextBox ---

	[TestMethod]
	[DataRow("SimpleTextBoxForeground")]
	[DataRow("SimpleTextBoxBackground")]
	[DataRow("SimpleTextBoxBorderBrush")]
	public void TextBox_BrushResource_Resolves(string resourceKey)
	{
		StyleTestHelper.AssertBrushResource(resourceKey);
	}

	// --- ToggleSwitch ---

	[TestMethod]
	[DataRow("SimpleToggleSwitchForeground")]
	public void ToggleSwitch_BrushResource_Resolves(string resourceKey)
	{
		StyleTestHelper.AssertBrushResource(resourceKey);
	}

	// --- ComboBox ---

	[TestMethod]
	[DataRow("SimpleComboBoxForeground")]
	[DataRow("SimpleComboBoxBackground")]
	[DataRow("SimpleComboBoxBorderBrush")]
	public void ComboBox_BrushResource_Resolves(string resourceKey)
	{
		StyleTestHelper.AssertBrushResource(resourceKey);
	}

	// --- Slider ---

	[TestMethod]
	[DataRow("SimpleSliderTrackFill")]
	[DataRow("SimpleSliderTrackValueFill")]
	[DataRow("SimpleSliderThumbBackground")]
	public void Slider_BrushResource_Resolves(string resourceKey)
	{
		StyleTestHelper.AssertBrushResource(resourceKey);
	}

	// --- PasswordBox ---

	[TestMethod]
	[DataRow("SimplePasswordBoxForeground")]
	[DataRow("SimplePasswordBoxBackground")]
	[DataRow("SimplePasswordBoxBorderBrush")]
	public void PasswordBox_BrushResource_Resolves(string resourceKey)
	{
		StyleTestHelper.AssertBrushResource(resourceKey);
	}
}
