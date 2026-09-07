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
	[TestMethod]
	[RunsOnUIThread]
	[DataRow(false, false)]
	[DataRow(false, true)]
	[DataRow(true, false)]
	[DataRow(true, true)]
	public void When_ColorOverridesAreNested_SharedBrushesPreserveBothAppearances(bool fluent, bool nestedInsideAppearance)
	{
		var overrides = new ResourceDictionary();
		var earlier = new ResourceDictionary { ["PrimaryColor"] = Microsoft.UI.Colors.Yellow };
		var child = new ResourceDictionary();
		var light = new ResourceDictionary { ["PrimaryColor"] = Microsoft.UI.Colors.Red, ["HoverOpacity"] = 0.21 };
		var dark = new ResourceDictionary { ["PrimaryColor"] = Microsoft.UI.Colors.Blue, ["HoverOpacity"] = 0.67 };
		if (nestedInsideAppearance)
		{
			var lightWrapper = new ResourceDictionary();
			lightWrapper.MergedDictionaries.Add(light);
			var darkWrapper = new ResourceDictionary();
			darkWrapper.MergedDictionaries.Add(dark);
			child.ThemeDictionaries["Light"] = lightWrapper;
			child.ThemeDictionaries["Default"] = darkWrapper;
		}
		else
		{
			child.ThemeDictionaries["Light"] = light;
			child.ThemeDictionaries["Default"] = dark;
		}
		overrides.MergedDictionaries.Add(earlier);
		overrides.MergedDictionaries.Add(child);
		BaseTheme theme = fluent ? new FluentTheme() : new SimpleTheme();
		theme.Colors = new ThemeColors { PrimarySeed = SeedPurple, OverrideDictionary = overrides };
		AssertBranchBrush(theme, "Light", "PrimaryBrush", Microsoft.UI.Colors.Red, 1);
		AssertBranchBrush(theme, "Default", "PrimaryBrush", Microsoft.UI.Colors.Blue, 1);
		AssertBranchBrush(theme, "Light", "PrimaryHoverBrush", Microsoft.UI.Colors.Red, 0.21);
		AssertBranchBrush(theme, "Default", "PrimaryHoverBrush", Microsoft.UI.Colors.Blue, 0.67);

		// An own value outranks merged children in normal XAML resolution.
		overrides["PrimaryColor"] = Microsoft.UI.Colors.Green;
		theme.DefaultSpacing = 6;
		AssertBranchBrush(theme, "Light", "PrimaryBrush", Microsoft.UI.Colors.Green, 1);
		AssertBranchBrush(theme, "Default", "PrimaryBrush", Microsoft.UI.Colors.Green, 1);
	}

	[TestMethod]
	[RunsOnUIThread]
	public void When_HighContrastOverrideIsAbsent_SharedBrushesPreferDarkOverDefault()
	{
		var overrides = new ResourceDictionary();
		overrides.ThemeDictionaries["Dark"] = new ResourceDictionary { ["PrimaryColor"] = Microsoft.UI.Colors.Blue };
		overrides.ThemeDictionaries["Default"] = new ResourceDictionary { ["PrimaryColor"] = Microsoft.UI.Colors.Red };
		var theme = new SimpleTheme { Colors = new ThemeColors { OverrideDictionary = overrides } };
		AssertBranchBrush(theme, "HighContrast", "PrimaryBrush", Microsoft.UI.Colors.Blue, 1);
		overrides.ThemeDictionaries["HighContrast"] = new ResourceDictionary { ["PrimaryColor"] = Microsoft.UI.Colors.Green };
		theme.DefaultSpacing = 6;
		AssertBranchBrush(theme, "HighContrast", "PrimaryBrush", Microsoft.UI.Colors.Green, 1);
	}

	private static void AssertBranchBrush(ResourceDictionary dictionary, string appearance, string key, Color color, double opacity)
	{
		var brush = FindOwnBranchBrush(dictionary, appearance, key);
		Assert.IsNotNull(brush, $"[{appearance}] {key} must resolve from the theme's own dictionaries.");
		Assert.AreEqual(color, brush.Color, $"[{appearance}] {key}");
		Assert.AreEqual(opacity, brush.Opacity, 0.001, $"[{appearance}] {key} opacity");
	}

	private static SolidColorBrush? FindOwnBranchBrush(ResourceDictionary dictionary, string appearance, string key)
	{
		for (var i = dictionary.MergedDictionaries.Count - 1; i >= 0; i--)
		{
			if (FindOwnBranchBrush(dictionary.MergedDictionaries[i], appearance, key) is { } merged)
			{
				return merged;
			}
		}
		if (dictionary.ThemeDictionaries.TryGetValue(appearance, out var value) && value is ResourceDictionary branch)
		{
			foreach (var entry in branch)
			{
				if (Equals(entry.Key, key) && entry.Value is SolidColorBrush brush)
				{
					return brush;
				}
			}
		}
		return null;
	}

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

		// Fluent and Simple share the seed pipeline (the default SeedColorMode, no
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

	// ─────────────────────────────────────────────────────────────────────
	// Unresolvable override sources must degrade, not throw: these setters run
	// property-changed callbacks, and an exception there crashes the consuming app.
	// ─────────────────────────────────────────────────────────────────────

	[TestMethod]
	[RunsOnUIThread]
	public void When_OverrideSourceDoesNotResolve_Then_ThemeKeepsWorking()
	{
		// Well-formed URI, nonexistent dictionary — the common typo case. Uri.TryCreate passes,
		// so only a guarded dictionary load prevents the crash.
		var theme = new SimpleTheme();
		var container = new Grid();
		container.Resources.MergedDictionaries.Add(theme);

		Assert.IsTrue(container.Resources.TryGetValue("PrimaryColor", out var before),
			"Sanity: PrimaryColor should resolve before the override is set");

		theme.Colors = new ThemeColors { OverrideSource = "ms-appx:///DoesNotExist/Nope.xaml" };

		Assert.IsTrue(container.Resources.TryGetValue("PrimaryColor", out var after),
			"The theme must keep its palette when the override source cannot be loaded.");
		Assert.AreEqual((Color)before, (Color)after,
			"An unresolvable override source must leave the palette unchanged.");
		Assert.IsNull(theme.Colors.OverrideDictionary,
			"An unresolvable override source must fall back to no override.");
	}

	[TestMethod]
	[RunsOnUIThread]
	public void When_FontOverrideSourceDoesNotResolve_Then_ThemeKeepsWorking()
	{
		var theme = new SimpleTheme();
		theme.FontOverrideSource = "ms-appx:///DoesNotExist/Nope.xaml";

		var container = new Grid();
		container.Resources.MergedDictionaries.Add(theme);

		Assert.IsTrue(container.Resources.TryGetValue("PrimaryColor", out _),
			"The theme must keep resolving resources when the font override source cannot be loaded.");
	}
}
