using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Uno.Themes;
using Uno.Themes.Samples.Content.Styles;
using Uno.UI.RuntimeTests;

namespace Uno.Themes.Samples.RuntimeTests;

/// <summary>
/// End-to-end cover for the Design Tokens page's font family tuner: driving its combo must set the
/// running application theme's font family and restyle text that is <em>already rendered</em>.
/// </summary>
/// <remarks>
/// These tests write to the application-wide theme, which is what the control is for, so each one
/// restores the family in a <c>finally</c>.
/// </remarks>
[TestClass]
public class Given_FontFamilyTuner
{
	private const int OpenSansIndex = 3;
	private const int DesignSystemDefaultIndex = 0;

	private const string OpenSans = "ms-appx:///Uno.Fonts.OpenSans/Fonts/OpenSans.ttf";

	[TestMethod]
	[RunsOnUIThread]
	public async Task When_ComboChanged_Then_AppThemeCarriesTheFamily()
	{
		var theme = SemanticThemeHelper.GetTheme();
		Assert.IsNotNull(theme, "The Simple sample app must have a BaseTheme in its application resources.");

		var original = theme.DefaultFontFamily;

		try
		{
			var tuner = new FontFamilyTunerControl();
			UnitTestsUIContentHelper.Content = tuner;
			await UnitTestsUIContentHelper.WaitForLoaded(tuner);

			((ComboBox)tuner.FindName("FamilyCombo")).SelectedIndex = OpenSansIndex;
			await UnitTestsUIContentHelper.WaitForIdle();

			Assert.AreEqual(OpenSans, theme.DefaultFontFamily?.Source,
				"Selecting a font must set it on the running theme.");
		}
		finally
		{
			theme.DefaultFontFamily = original;
			UnitTestsUIContentHelper.Content = null;
		}
	}

	[TestMethod]
	[RunsOnUIThread]
	public async Task When_ComboChanged_Then_AlreadyRenderedTextRestyles()
	{
		var theme = SemanticThemeHelper.GetTheme();
		Assert.IsNotNull(theme, "The Simple sample app must have a BaseTheme in its application resources.");

		var original = theme.DefaultFontFamily;

		try
		{
			// Both TextBlocks are rendered *before* the family changes, so they can only pick the new
			// one up through the theme-change pass the control drives. Display and Body sit at
			// opposite ends of the type scale, so the single property is shown reaching every scale.
			var display = new TextBlock
			{
				Text = "Display Large",
				Style = (Style)Application.Current.Resources["DisplayLarge"],
			};
			var body = new TextBlock
			{
				Text = "Body Medium",
				Style = (Style)Application.Current.Resources["BodyMedium"],
			};
			var tuner = new FontFamilyTunerControl();
			var root = new StackPanel();
			root.Children.Add(display);
			root.Children.Add(body);
			root.Children.Add(tuner);

			UnitTestsUIContentHelper.Content = root;
			await UnitTestsUIContentHelper.WaitForLoaded(display);
			await UnitTestsUIContentHelper.WaitForLoaded(tuner);
			await UnitTestsUIContentHelper.WaitForIdle();

			var displayBefore = display.FontFamily?.Source;
			var bodyBefore = body.FontFamily?.Source;

			((ComboBox)tuner.FindName("FamilyCombo")).SelectedIndex = OpenSansIndex;
			await UnitTestsUIContentHelper.WaitForIdle();

			Assert.AreEqual(OpenSans, display.FontFamily?.Source,
				$"The rendered DisplayLarge TextBlock must re-resolve its family (was '{displayBefore}').");
			Assert.AreEqual(OpenSans, body.FontFamily?.Source,
				$"The rendered BodyMedium TextBlock must re-resolve its family too (was '{bodyBefore}').");
		}
		finally
		{
			theme.DefaultFontFamily = original;
			UnitTestsUIContentHelper.Content = null;
		}
	}

	[TestMethod]
	[RunsOnUIThread]
	public async Task When_DesignSystemDefaultSelected_Then_FamilyIsUnset()
	{
		var theme = SemanticThemeHelper.GetTheme();
		Assert.IsNotNull(theme, "The Simple sample app must have a BaseTheme in its application resources.");

		var original = theme.DefaultFontFamily;

		try
		{
			var tuner = new FontFamilyTunerControl();
			UnitTestsUIContentHelper.Content = tuner;
			await UnitTestsUIContentHelper.WaitForLoaded(tuner);

			var combo = (ComboBox)tuner.FindName("FamilyCombo");

			combo.SelectedIndex = OpenSansIndex;
			await UnitTestsUIContentHelper.WaitForIdle();

			combo.SelectedIndex = DesignSystemDefaultIndex;
			await UnitTestsUIContentHelper.WaitForIdle();

			Assert.IsNull(theme.DefaultFontFamily,
				"The design-system entry must hand the type scale back by leaving the property unset.");
		}
		finally
		{
			theme.DefaultFontFamily = original;
			UnitTestsUIContentHelper.Content = null;
		}
	}
}
