using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Uno.Fluent;
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

	// ─────────────────────────────────────────────────────────────────────
	// 3. FluentTheme: the same precedence contract holds for the code-built
	//    Fluent palette (specs/05-fluent-theme §6.1) — base palette < seed
	//    < consumer override.
	// ─────────────────────────────────────────────────────────────────────

	[TestMethod]
	[RunsOnUIThread]
	public void When_FluentThemeSeedSet_Then_SeedWinsOverFluentPalette()
	{
		var seeded = new FluentTheme();
		seeded.Colors = new ThemeColors { PrimarySeed = SeedPurple };
		var seededContainer = new Grid();
		seededContainer.Resources.MergedDictionaries.Add(seeded);

		var unseededContainer = new Grid();
		unseededContainer.Resources.MergedDictionaries.Add(new FluentTheme());

		Assert.IsTrue(
			seededContainer.Resources.TryGetValue("PrimaryColor", out var seededValue) && seededValue is Color,
			"PrimaryColor should resolve from the seeded FluentTheme");
		Assert.IsTrue(
			unseededContainer.Resources.TryGetValue("PrimaryColor", out var unseededValue) && unseededValue is Color,
			"PrimaryColor should resolve from the unseeded FluentTheme");

		Assert.AreNotEqual((Color)unseededValue, (Color)seededValue,
			"a seed must take precedence over the code-built Fluent palette");

		// Fluent and Simple share the seed pipeline (high-fidelity generation, no
		// default seed) — the same seed must produce the same PrimaryColor.
		var simpleSeeded = new SimpleTheme();
		simpleSeeded.Colors = new ThemeColors { PrimarySeed = SeedPurple };
		var simpleContainer = new Grid();
		simpleContainer.Resources.MergedDictionaries.Add(simpleSeeded);

		Assert.IsTrue(
			simpleContainer.Resources.TryGetValue("PrimaryColor", out var simpleValue) && simpleValue is Color,
			"PrimaryColor should resolve from the seeded SimpleTheme");
		Assert.AreEqual((Color)simpleValue, (Color)seededValue,
			"the same seed must generate the same PrimaryColor under FluentTheme and SimpleTheme");
	}

	[TestMethod]
	[RunsOnUIThread]
	public async Task When_FluentThemeSeedAndOverrideBothSet_Then_OverrideWins()
	{
		var overrideDict = CreateColorAndBrushOverride("PrimaryColor", "PrimaryBrush", OverrideBlue);

		var theme = new FluentTheme();
		theme.Colors = new ThemeColors
		{
			PrimarySeed = SeedPurple,
			OverrideDictionary = overrideDict,
		};

		var container = new Grid();
		container.Resources.MergedDictionaries.Add(theme);

		var style = container.Resources["FilledButtonStyle"] as Style;
		Assert.IsNotNull(style, "FilledButtonStyle should resolve from FluentTheme");

		// The semantic brush must carry the override; the button itself keeps
		// Fluent's own accent fill because FilledButtonStyle IS the untouched
		// XCR AccentButtonStyle (adapter architecture — reverse accent mapping
		// is Phase 2, specs/05-fluent-theme §9).
		Assert.IsTrue(
			container.Resources.TryGetValue("PrimaryColor", out var colorValue) && colorValue is Color,
			"PrimaryColor should resolve from FluentTheme");
		Assert.AreEqual(OverrideBlue, (Color)colorValue,
			"the consumer override must win over both the seed palette and the Fluent palette");

		Assert.IsTrue(
			container.Resources.TryGetValue("PrimaryBrush", out var brushValue) && brushValue is SolidColorBrush,
			"PrimaryBrush should resolve from FluentTheme");
		Assert.AreEqual(OverrideBlue, ((SolidColorBrush)brushValue).Color,
			"the overridden PrimaryBrush must carry the override color");

		var button = new Button { Content = "Test", Style = style };
		container.Children.Add(button);

		UnitTestsUIContentHelper.Content = container;
		await UnitTestsUIContentHelper.WaitForLoaded(button);
		await UnitTestsUIContentHelper.WaitForIdle();

		Assert.IsTrue(button.IsLoaded, "the styled button should load under an overridden FluentTheme");
	}
}
