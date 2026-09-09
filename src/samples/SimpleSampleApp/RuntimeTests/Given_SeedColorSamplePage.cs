using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Uno.Themes.Samples.Content.Styles;
using Uno.UI.RuntimeTests;
using Windows.UI;

namespace Uno.Themes.Samples.RuntimeTests;

/// <summary>
/// Verifies that navigating to the seed sample constructs the page and keeps its live controls working.
/// </summary>
[TestClass]
public class Given_SeedColorSamplePage
{
	[TestMethod]
	[RunsOnUIThread]
	[DataRow(ElementTheme.Light, 0, false)]
	[DataRow(ElementTheme.Light, 0, true)]
	[DataRow(ElementTheme.Light, 1, false)]
	[DataRow(ElementTheme.Light, 1, true)]
	[DataRow(ElementTheme.Dark, 0, false)]
	[DataRow(ElementTheme.Dark, 0, true)]
	[DataRow(ElementTheme.Dark, 1, false)]
	[DataRow(ElementTheme.Dark, 1, true)]
	public async Task When_NavigatingToSeedColor_Then_ControlsUpdateAndSelectionSurvivesReentry(ElementTheme appearance, int modeIndex, bool separateApplicationResources)
	{
		var theme = NavigationHelper.SampleTheme;
		Assert.IsNotNull(theme);
		var originalColors = theme.Colors;
		var appDictionaries = Application.Current.Resources.MergedDictionaries;
		var themeIndex = appDictionaries.IndexOf(theme);
		SeedColorSamplePage? page = null;
		var initialPickerSeed = default(Color);
		var initialModeIndex = 0;

		try
		{
			// Keep XAML resources resolvable, but remove the theme from Application.Current's
			// direct dictionaries, as in the wrapper app. The sample still owns its theme.
			theme.Colors = new ThemeColors();
			if (separateApplicationResources && themeIndex >= 0)
			{
				appDictionaries[themeIndex] = new ResourceDictionary { MergedDictionaries = { theme } };
			}

			if (separateApplicationResources)
			{
				Assert.IsNull(Application.Current.GetTheme(), "The ambient application must not expose the sample theme.");
			}

			// NavigationView.ItemInvoked constructs this page before displaying it.
			page = new SeedColorSamplePage();
			page.RequestedTheme = appearance;
			UnitTestsUIContentHelper.Content = page;
			var picker = (ColorPicker)page.FindName("SeedColorPicker");
			var mode = (ComboBox)page.FindName("SeedColorModeCombo");
			initialPickerSeed = picker.Color;
			initialModeIndex = mode.SelectedIndex;
			await UnitTestsUIContentHelper.WaitForLoaded(page);

			var seed = Color.FromArgb(0xFF, 0x38, 0x6A, 0x20);
			picker.Color = seed;
			mode.SelectedIndex = modeIndex;
			await UnitTestsUIContentHelper.WaitForIdle();

			Assert.AreEqual(seed, theme.Colors.PrimarySeed);
			Assert.AreEqual(modeIndex == 0 ? SeedColorMode.Fidelity : SeedColorMode.TonalSpot, theme.Colors.SeedColorMode);
			Assert.AreEqual("#386A20", ((TextBlock)page.FindName("SeedHex")).Text);
			Assert.AreEqual(seed, ((SolidColorBrush)((Border)page.FindName("SeedSwatch")).Background).Color);
			StringAssert.Contains(((TextBlock)page.FindName("XamlSnippet")).Text, "PrimarySeed=\"#386A20\"");

			// Returning must restore the user's seed and mode without events resetting them during XAML loading.
			UnitTestsUIContentHelper.Content = null;
			var returnedPage = new SeedColorSamplePage();
			returnedPage.RequestedTheme = appearance;
			UnitTestsUIContentHelper.Content = returnedPage;
			await UnitTestsUIContentHelper.WaitForLoaded(returnedPage);
			Assert.AreNotSame(page, returnedPage);
			Assert.AreEqual(seed, ((ColorPicker)returnedPage.FindName("SeedColorPicker")).Color);
			Assert.AreEqual(modeIndex, ((ComboBox)returnedPage.FindName("SeedColorModeCombo")).SelectedIndex);
			Assert.AreEqual("#386A20", ((TextBlock)returnedPage.FindName("SeedHex")).Text);
		}
		finally
		{
			UnitTestsUIContentHelper.Content = null;
			if (themeIndex >= 0)
			{
				appDictionaries[themeIndex] = theme;
			}
			if (page is not null)
			{
				((ColorPicker)page.FindName("SeedColorPicker")).Color = initialPickerSeed;
				((ComboBox)page.FindName("SeedColorModeCombo")).SelectedIndex = initialModeIndex;
			}

			theme.Colors = originalColors;
		}
	}
}
