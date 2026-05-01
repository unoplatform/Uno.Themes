using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Uno.Simple;
using Uno.UI.RuntimeTests;
using Windows.UI;

namespace Uno.Themes.Samples.RuntimeTests;

/// <summary>
/// Verifies that explicit color overrides take precedence over seed-generated
/// colors in BaseTheme.UpdateSource(). This guards against regressions where
/// seed palette colors "bleed through" user-defined overrides.
/// </summary>
[TestClass]
public class Given_ColorOverridePrecedence
{
	// A distinctive blue that is clearly not from any default palette.
	private static readonly Color OverrideBlue = Color.FromArgb(0xFF, 0x21, 0x96, 0xF3);

	// The seed purple used for generation — should NOT appear when overridden.
	private static readonly Color SeedPurple = Color.FromArgb(0xFF, 0x59, 0x46, 0xD2);

	/// <summary>
	/// Creates a SimpleTheme with a seed color active and an override dictionary
	/// that redefines PrimaryColor. The override should win.
	/// </summary>
	private static SimpleTheme CreateThemeWithSeedAndOverride(Color seedColor, Color overridePrimaryColor)
	{
		var overrideDict = new ResourceDictionary();
		var lightDict = new ResourceDictionary();
		lightDict["PrimaryColor"] = overridePrimaryColor;
		var darkDict = new ResourceDictionary();
		darkDict["PrimaryColor"] = overridePrimaryColor;

		overrideDict.ThemeDictionaries["Light"] = lightDict;
		overrideDict.ThemeDictionaries["Default"] = darkDict;

		var theme = new SimpleTheme();
		theme.Colors = new ThemeColors
		{
			PrimarySeed = seedColor,
			OverrideDictionary = overrideDict,
		};

		return theme;
	}

	// ─────────────────────────────────────────────────────────────────────
	// Color override vs seed: OverrideDictionary should take precedence
	// over seed-generated colors in the merged resource chain.
	// ─────────────────────────────────────────────────────────────────────

	[TestMethod]
	[RunsOnUIThread]
	public async Task When_SeedAndOverrideBothSet_Then_OverrideWins()
	{
		var theme = CreateThemeWithSeedAndOverride(SeedPurple, OverrideBlue);

		var container = new Grid();
		container.Resources.MergedDictionaries.Add(theme);

		// Use a styled button to verify the resolved primary color flows through.
		var style = container.Resources["FilledButtonStyle"] as Style;
		Assert.IsNotNull(style, "FilledButtonStyle should resolve from theme");

		var button = new Button { Content = "Test", Style = style };
		container.Children.Add(button);

		UnitTestsUIContentHelper.Content = container;
		await UnitTestsUIContentHelper.WaitForLoaded(button);
		await UnitTestsUIContentHelper.WaitForIdle();

		var bg = button.Background as SolidColorBrush;
		Assert.IsNotNull(bg, "Button should have a SolidColorBrush Background");

		// The override blue should win over the seed-generated purple.
		Assert.AreEqual(OverrideBlue, bg.Color,
			$"Expected override color #{OverrideBlue} but got #{bg.Color}. " +
			"Seed-generated colors are bleeding through the override.");
	}

	[TestMethod]
	[RunsOnUIThread]
	public async Task When_OverrideSetViaDeprecatedColorOverrideDictionary_Then_OverrideWins()
	{
		// This test exercises the deprecated ColorOverrideDictionary DP path
		// which historically had a bug where UpdateSource() was not called
		// after unmuting the color override callback.
		var overrideDict = new ResourceDictionary();
		var lightDict = new ResourceDictionary();
		lightDict["PrimaryColor"] = OverrideBlue;
		var darkDict = new ResourceDictionary();
		darkDict["PrimaryColor"] = OverrideBlue;
		overrideDict.ThemeDictionaries["Light"] = lightDict;
		overrideDict.ThemeDictionaries["Default"] = darkDict;

		var theme = new SimpleTheme();
		theme.Colors = new ThemeColors { PrimarySeed = SeedPurple };
#pragma warning disable CS0618 // Testing deprecated API path
		theme.ColorOverrideDictionary = overrideDict;
#pragma warning restore CS0618

		var container = new Grid();
		container.Resources.MergedDictionaries.Add(theme);

		var style = container.Resources["FilledButtonStyle"] as Style;
		Assert.IsNotNull(style, "FilledButtonStyle should resolve from theme");

		var button = new Button { Content = "Test", Style = style };
		container.Children.Add(button);

		UnitTestsUIContentHelper.Content = container;
		await UnitTestsUIContentHelper.WaitForLoaded(button);
		await UnitTestsUIContentHelper.WaitForIdle();

		var bg = button.Background as SolidColorBrush;
		Assert.IsNotNull(bg, "Button should have a SolidColorBrush Background");

		Assert.AreEqual(OverrideBlue, bg.Color,
			$"Expected override color #{OverrideBlue} but got #{bg.Color}. " +
			"ColorOverrideDictionary path is not taking precedence over seed colors.");
	}

}
