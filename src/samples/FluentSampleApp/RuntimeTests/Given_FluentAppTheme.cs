using Microsoft.VisualStudio.TestTools.UnitTesting;
using Uno.UI.RuntimeTests;
using Windows.UI;
using Windows.UI.ViewManagement;

namespace Uno.Themes.Samples.RuntimeTests;

/// <summary>
/// The <c>FluentTheme</c> declared in this head's App.xaml, after <c>XamlControlsResources</c>: the
/// consumer topology the CI host only builds in code.
/// </summary>
[TestClass]
public class Given_FluentAppTheme
{
	private static readonly Color SeedRed = Color.FromArgb(0xFF, 0xB0, 0x00, 0x20);

	[TestMethod]
	[RunsOnUIThread]
	public async Task When_Started_AccentButtonUsesTheSystemAccent()
	{
		var isDark = Application.Current.RequestedTheme == ApplicationTheme.Dark;
		var expected = new UISettings().GetColorValue(isDark ? UIColorType.AccentLight2 : UIColorType.AccentDark1);

		Assert.AreEqual(expected, await GetAccentButtonFill());
	}

	[TestMethod]
	[RunsOnUIThread]
	public async Task When_AppThemeSeededThenCleared_AccentButtonFollowsAndRestores()
	{
		var seedBefore = SemanticThemeHelper.PrimarySeed;
		var platformFill = await GetAccentButtonFill();
		try
		{
			SemanticThemeHelper.PrimarySeed = SeedRed;
			Assert.AreNotEqual(platformFill, await GetAccentButtonFill(), "the seed should recolor the accent button");
		}
		finally
		{
			SemanticThemeHelper.PrimarySeed = seedBefore;
		}

		Assert.AreEqual(platformFill, await GetAccentButtonFill(), "clearing the seed should restore the system accent");
	}

	private static async Task<Color> GetAccentButtonFill()
	{
		var button = new Button { Content = "accent", Style = (Style)Application.Current.Resources["AccentButtonStyle"] };
		try
		{
			UnitTestsUIContentHelper.Content = button;
			await UnitTestsUIContentHelper.WaitForLoaded(button);
			await UnitTestsUIContentHelper.WaitForIdle();
			return Assert.IsInstanceOfType<SolidColorBrush>(button.Background).Color;
		}
		finally
		{
			UnitTestsUIContentHelper.Content = null;
		}
	}
}
