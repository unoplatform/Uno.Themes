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
///
/// Architecture note: SharedColors.xaml defines brushes via
///   <c>&lt;SolidColorBrush Color="{StaticResource PrimaryColor}" /&gt;</c>
/// which is a one-time resolution. Overriding PrimaryColor in the
/// OverrideDictionary correctly updates the Color resource, but the
/// already-constructed brush retains its original color.
/// To override the rendered brush, include PrimaryBrush in the override dict.
/// </summary>
[TestClass]
public class Given_ColorOverridePrecedence
{
	// A distinctive blue that is clearly not from any default palette.
	private static readonly Color OverrideBlue = Color.FromArgb(0xFF, 0x21, 0x96, 0xF3);

	// The seed purple used for generation — should NOT appear when overridden.
	private static readonly Color SeedPurple = Color.FromArgb(0xFF, 0x59, 0x46, 0xD2);

	/// <summary>
	/// Creates an override dictionary that sets both the Color resource AND
	/// the corresponding Brush, so the override flows through to rendered controls.
	/// </summary>
	private static ResourceDictionary CreateColorAndBrushOverride(
		string colorKey, string brushKey, Color overrideColor)
	{
		var overrideDict = new ResourceDictionary();
		foreach (var themeKey in new[] { "Light", "Default" })
		{
			var themed = new ResourceDictionary();
			themed[colorKey] = overrideColor;
			themed[brushKey] = new SolidColorBrush(overrideColor);
			overrideDict.ThemeDictionaries[themeKey] = themed;
		}
		return overrideDict;
	}

	// ─────────────────────────────────────────────────────────────────────
	// 1. Color-level override verification (no rendering required)
	// ─────────────────────────────────────────────────────────────────────

	[TestMethod]
	[RunsOnUIThread]
	public void When_SeedAndOverrideBothSet_Then_ColorResourceIsOverridden()
	{
		var overrideDict = new ResourceDictionary();
		foreach (var themeKey in new[] { "Light", "Default" })
		{
			var themed = new ResourceDictionary();
			themed["PrimaryColor"] = OverrideBlue;
			overrideDict.ThemeDictionaries[themeKey] = themed;
		}

		var theme = new SimpleTheme();
		theme.Colors = new ThemeColors
		{
			PrimarySeed = SeedPurple,
			OverrideDictionary = overrideDict,
		};

		var container = new Grid();
		container.Resources.MergedDictionaries.Add(theme);

		Assert.IsTrue(
			container.Resources.TryGetValue("PrimaryColor", out var colorVal),
			"PrimaryColor should be resolvable from the theme");

		Assert.AreEqual(OverrideBlue, (Color)colorVal,
			"PrimaryColor should be the override value, not the seed-generated value.");
	}

	// ─────────────────────────────────────────────────────────────────────
	// 2. Full E2E: override Color+Brush, verify rendered button
	// ─────────────────────────────────────────────────────────────────────

	[TestMethod]
	[RunsOnUIThread]
	public async Task When_SeedAndOverrideBothSet_Then_OverrideWins()
	{
		var overrideDict = CreateColorAndBrushOverride("PrimaryColor", "PrimaryBrush", OverrideBlue);

		var theme = new SimpleTheme();
		theme.Colors = new ThemeColors
		{
			PrimarySeed = SeedPurple,
			OverrideDictionary = overrideDict,
		};

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
			"Seed-generated colors are bleeding through the override.");
	}

	[TestMethod]
	[RunsOnUIThread]
	public async Task When_OverrideSetViaDeprecatedColorOverrideDictionary_Then_OverrideWins()
	{
		var overrideDict = CreateColorAndBrushOverride("PrimaryColor", "PrimaryBrush", OverrideBlue);

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
