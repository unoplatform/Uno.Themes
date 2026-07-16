using Microsoft.VisualStudio.TestTools.UnitTesting;
using Uno.Fluent;
using Uno.Themes.ColorGeneration;
using Uno.Themes.ColorGeneration.Hct;
using Uno.UI.RuntimeTests;
using Windows.UI;

namespace Uno.Themes.Samples.RuntimeTests;

/// <summary>
/// Verifies the FluentTheme reverse accent mapping (specs/05-fluent-theme, §9,
/// goal G5): an active seed color overrides the SystemAccentColor* shades and
/// the accent-derived token closure with tones from the seed's tonal palette,
/// so the BUILT-IN Fluent controls follow the seed too; clearing the seed
/// restores the platform accent. An explicit PrimaryColor override — via any
/// channel (Colors.OverrideDictionary / Colors.OverrideSource or the obsolete
/// BaseTheme ColorOverride* properties) — drives the same cascade with the
/// override value verbatim, taking precedence over the seed.
/// </summary>
[TestClass]
public class Given_FluentSeedAccent
{
	// A distinctive red that is clearly not any platform accent shade.
	private static readonly Color SeedRed = Color.FromArgb(0xFF, 0xB0, 0x00, 0x20);

	// Distinctive override colors, matching RuntimeTests/FluentColorOverride.xaml.
	private static readonly Color OverrideBlue = Color.FromArgb(0xFF, 0x21, 0x96, 0xF3);
	private static readonly Color OverrideGreen = Color.FromArgb(0xFF, 0x66, 0xBB, 0x6A);

	private const string ConsumerOverrideSource = "ms-appx:///RuntimeTests/FluentColorOverride.xaml";

	private static bool IsAmbientDark =>
		Application.Current.RequestedTheme == ApplicationTheme.Dark;

	/// <summary>
	/// Expected tone from the seed's palette, matching FluentTheme's
	/// high-fidelity generation (seed chroma preserved, no M3 minimum floor).
	/// </summary>
	private static Color Tone(Color seed, int tone)
	{
		var hct = HctColor.FromArgb((seed.A << 24) | (seed.R << 16) | (seed.G << 8) | seed.B);
		var argb = new TonalPalette(hct.Hue, hct.Chroma).GetArgb(tone);
		return Color.FromArgb(
			(byte)((argb >> 24) & 0xFF),
			(byte)((argb >> 16) & 0xFF),
			(byte)((argb >> 8) & 0xFF),
			(byte)(argb & 0xFF));
	}

	private static FluentTheme CreateSeededTheme(Color seed)
	{
		var theme = new FluentTheme();
		theme.Colors = new ThemeColors { PrimarySeed = seed };
		return theme;
	}

	private static Grid CreateSeededContainer(Color seed)
	{
		var container = new Grid();
		container.Resources.MergedDictionaries.Add(CreateSeededTheme(seed));
		return container;
	}

	private static Color GetColor(ResourceDictionary resources, string key)
	{
		Assert.IsTrue(
			resources.TryGetValue(key, out var value) && value is Color,
			$"{key} should resolve to a Color");
		return (Color)value;
	}

	// ─────────────────────────────────────────────────────────────────────
	// Shade set (spec §9.1): SystemAccentColor* follow the tonal palette.
	// ─────────────────────────────────────────────────────────────────────

	[TestMethod]
	[RunsOnUIThread]
	[DataRow("SystemAccentColor", 40)]
	[DataRow("SystemAccentColorLight1", 60)]
	[DataRow("SystemAccentColorLight2", 70)]
	[DataRow("SystemAccentColorLight3", 80)]
	[DataRow("SystemAccentColorDark1", 30)]
	[DataRow("SystemAccentColorDark2", 20)]
	[DataRow("SystemAccentColorDark3", 10)]
	public void When_SeedSet_AccentShadesFollowTonalPalette(string shadeKey, int tone)
	{
		var container = CreateSeededContainer(SeedRed);

		Assert.AreEqual(Tone(SeedRed, tone), GetColor(container.Resources, shadeKey),
			$"{shadeKey} must carry tone {tone} of the seed palette (spec 05 §9.1)");

		// The override is scoped to the theme: the app-level accent must be untouched.
		Assert.AreNotEqual(Tone(SeedRed, tone), GetColor(Application.Current.Resources, shadeKey),
			$"a container-scoped seeded FluentTheme must not leak {shadeKey} to app scope");
	}

	// ─────────────────────────────────────────────────────────────────────
	// Accent closure (spec §9.2 / D12): the accent-derived colors and brushes
	// follow the branch mapping — light fill = Dark1 (tone 30), dark fill =
	// Light2 (tone 70), mirroring XCR's own structure (spike S4).
	// ─────────────────────────────────────────────────────────────────────

	[TestMethod]
	[RunsOnUIThread]
	public void When_SeedSet_AccentClosureFollowsBranchMapping()
	{
		var container = CreateSeededContainer(SeedRed);

		var expectedFill = Tone(SeedRed, IsAmbientDark ? 70 : 30);
		var expectedAccentText = Tone(SeedRed, IsAmbientDark ? 80 : 20);

		Assert.IsTrue(
			container.Resources.TryGetValue("AccentFillColorDefaultBrush", out var fillValue)
				&& fillValue is SolidColorBrush,
			"AccentFillColorDefaultBrush should resolve under a seeded FluentTheme");
		Assert.AreEqual(expectedFill, ((SolidColorBrush)fillValue).Color,
			"the accent fill must carry the branch-mapped seed tone (light: Dark1/30, dark: Light2/70)");

		Assert.IsTrue(
			container.Resources.TryGetValue("AccentFillColorSecondaryBrush", out var secondaryValue)
				&& secondaryValue is SolidColorBrush,
			"AccentFillColorSecondaryBrush should resolve under a seeded FluentTheme");
		var secondary = (SolidColorBrush)secondaryValue;
		Assert.AreEqual(expectedFill, secondary.Color, "the secondary fill uses the same tone as the default fill");
		Assert.AreEqual(0.9, secondary.Opacity, 0.001, "the secondary fill is the default fill at 90% brush opacity (XCR structure)");

		Assert.IsTrue(
			container.Resources.TryGetValue("AccentTextFillColorPrimaryBrush", out var textValue)
				&& textValue is SolidColorBrush,
			"AccentTextFillColorPrimaryBrush should resolve under a seeded FluentTheme");
		Assert.AreEqual(expectedAccentText, ((SolidColorBrush)textValue).Color,
			"accent text must carry the branch-mapped seed tone (light: Dark2/20, dark: Light3/80)");
	}

	// ─────────────────────────────────────────────────────────────────────
	// G5, rendered: a BUILT-IN Fluent control follows the seed in the
	// documented consumer topology (FluentTheme at app scope, after XCR).
	// ─────────────────────────────────────────────────────────────────────

	[TestMethod]
	[RunsOnUIThread]
	public async Task When_SeedSet_RenderedAccentButtonFollowsSeed()
	{
		var expectedFill = Tone(SeedRed, IsAmbientDark ? 70 : 30);

		var appDictionaries = Application.Current.Resources.MergedDictionaries;
		var theme = CreateSeededTheme(SeedRed);
		appDictionaries.Add(theme);
		try
		{
			var button = new Button
			{
				Content = "seeded",
				Style = (Style)Application.Current.Resources["AccentButtonStyle"],
			};
			var host = new Grid();
			host.Children.Add(button);

			UnitTestsUIContentHelper.Content = host;
			await UnitTestsUIContentHelper.WaitForLoaded(button);
			await UnitTestsUIContentHelper.WaitForIdle();

			var background = button.Background as SolidColorBrush;
			Assert.IsNotNull(background, "the accent button should have a SolidColorBrush background");
			Assert.AreEqual(expectedFill, background.Color,
				"the built-in accent button must render with the seed's branch-mapped accent fill (G5)");
		}
		finally
		{
			appDictionaries.Remove(theme);
		}
	}

	// ─────────────────────────────────────────────────────────────────────
	// Seed cleared → the platform accent is restored (no residue).
	// ─────────────────────────────────────────────────────────────────────

	[TestMethod]
	[RunsOnUIThread]
	public async Task When_SeedCleared_PlatformAccentRestored()
	{
		var appDictionaries = Application.Current.Resources.MergedDictionaries;
		var platformAccent = GetColor(Application.Current.Resources, "SystemAccentColor");
		Assert.AreNotEqual(Tone(SeedRed, 40), platformAccent,
			"sanity: the platform accent must differ from the seed tone for this test to be meaningful");

		var theme = CreateSeededTheme(SeedRed);
		appDictionaries.Add(theme);
		try
		{
			Assert.AreEqual(Tone(SeedRed, 40), GetColor(Application.Current.Resources, "SystemAccentColor"),
				"the seeded accent should be active before clearing");

			// In-place clear: everything the THEME owns restores immediately.
			// (XCR's own materialized accent brushes can keep the last value
			// until the next app-scope resource change — platform cache
			// behavior, documented in seed-colors.md and spike-results.md S4.)
			theme.Colors.PrimarySeed = null;

			Assert.AreEqual(platformAccent, GetColor(Application.Current.Resources, "SystemAccentColor"),
				"clearing the seed must restore the platform SystemAccentColor");
		}
		finally
		{
			appDictionaries.Remove(theme);
		}

		// Unmerging is an app-scope resource change: on the next render pass
		// XCR re-materializes its accent brushes, so a freshly rendered
		// built-in control must carry the PLATFORM accent fill again (the S4
		// clean-restore flow) — no seeded residue.
		var expectedPlatformFill = GetColor(
			Application.Current.Resources,
			IsAmbientDark ? "SystemAccentColorLight2" : "SystemAccentColorDark1");

		var button = new Button
		{
			Content = "restored",
			Style = (Style)Application.Current.Resources["AccentButtonStyle"],
		};
		var host = new Grid();
		host.Children.Add(button);

		UnitTestsUIContentHelper.Content = host;
		await UnitTestsUIContentHelper.WaitForLoaded(button);
		await UnitTestsUIContentHelper.WaitForIdle();

		var background = button.Background as SolidColorBrush;
		Assert.IsNotNull(background, "the restored accent button should have a SolidColorBrush background");
		Assert.AreEqual(expectedPlatformFill, background.Color,
			"a rendered built-in control must carry the platform accent fill after the seeded theme is unmerged");
	}

	// ─────────────────────────────────────────────────────────────────────
	// §9.3 — forward (semantic palette) and reverse (accent closure) flows
	// must agree on what "Primary" is under a seed.
	// ─────────────────────────────────────────────────────────────────────

	[TestMethod]
	[RunsOnUIThread]
	public void When_SeedSet_ForwardAndReverseFlowsAgree()
	{
		var container = CreateSeededContainer(SeedRed);

		// Seeded semantic PrimaryColor: tone 40 (light branch) / tone 80 (dark
		// branch) — which the reverse mapping exposes as SystemAccentColor and
		// SystemAccentColorLight3 respectively.
		var expectedAccentKey = IsAmbientDark ? "SystemAccentColorLight3" : "SystemAccentColor";

		Assert.AreEqual(
			GetColor(container.Resources, expectedAccentKey),
			GetColor(container.Resources, "PrimaryColor"),
			$"the seeded semantic PrimaryColor and the reverse-mapped {expectedAccentKey} must agree (§9.3)");
	}

	// ─────────────────────────────────────────────────────────────────────
	// Override-driven accent: an explicit PrimaryColor override recolors the
	// built-in Fluent controls too — parity with Material/Simple, where a
	// PrimaryColor override visibly recolors the theme's controls. Unlike the
	// seed (a generator input, mapped through tones), the override value is
	// the accent VERBATIM per branch.
	// ─────────────────────────────────────────────────────────────────────

	private static ResourceDictionary CreateBranchedPrimaryOverride(Color light, Color dark)
	{
		var overrideDict = new ResourceDictionary();
		var lightBranch = new ResourceDictionary();
		lightBranch["PrimaryColor"] = light;
		var darkBranch = new ResourceDictionary();
		darkBranch["PrimaryColor"] = dark;
		overrideDict.ThemeDictionaries["Light"] = lightBranch;
		// "Dark" — the branch key consumers use (see ColorPaletteOverride.xaml
		// in the sample heads) — must be honored alongside "Default".
		overrideDict.ThemeDictionaries["Dark"] = darkBranch;
		return overrideDict;
	}

	[TestMethod]
	[RunsOnUIThread]
	public void When_PrimaryColorOverridden_AccentCascadeFollows()
	{
		var theme = new FluentTheme();
		theme.Colors = new ThemeColors
		{
			OverrideDictionary = CreateBranchedPrimaryOverride(OverrideBlue, OverrideGreen),
		};
		var container = new Grid();
		container.Resources.MergedDictionaries.Add(theme);

		var expected = IsAmbientDark ? OverrideGreen : OverrideBlue;

		Assert.AreEqual(expected, GetColor(container.Resources, "SystemAccentColor"),
			"a PrimaryColor override must drive SystemAccentColor with the branch's value verbatim");

		Assert.IsTrue(
			container.Resources.TryGetValue("AccentFillColorDefaultBrush", out var fillValue)
				&& fillValue is SolidColorBrush,
			"AccentFillColorDefaultBrush should resolve under an overridden FluentTheme");
		Assert.AreEqual(expected, ((SolidColorBrush)fillValue).Color,
			"the accent fill must carry the override value verbatim (not a derived tone)");

		// Forward/reverse agreement (§9.3) holds under overrides too.
		Assert.AreEqual(expected, GetColor(container.Resources, "PrimaryColor"),
			"the semantic PrimaryColor and the override-driven accent must agree");
	}

	[TestMethod]
	[RunsOnUIThread]
	public async Task When_PrimaryColorOverridden_RenderedAccentButtonFollows()
	{
		// Flat (theme-invariant) override: both branches carry the same value,
		// so the expectation is deterministic regardless of the ambient theme.
		var overrideDict = new ResourceDictionary();
		overrideDict["PrimaryColor"] = OverrideBlue;

		var theme = new FluentTheme();
		theme.Colors = new ThemeColors { OverrideDictionary = overrideDict };

		var appDictionaries = Application.Current.Resources.MergedDictionaries;
		appDictionaries.Add(theme);
		try
		{
			var button = new Button
			{
				Content = "overridden",
				Style = (Style)Application.Current.Resources["AccentButtonStyle"],
			};
			var host = new Grid();
			host.Children.Add(button);

			UnitTestsUIContentHelper.Content = host;
			await UnitTestsUIContentHelper.WaitForLoaded(button);
			await UnitTestsUIContentHelper.WaitForIdle();

			var background = button.Background as SolidColorBrush;
			Assert.IsNotNull(background, "the accent button should have a SolidColorBrush background");
			Assert.AreEqual(OverrideBlue, background.Color,
				"the built-in accent button must render with the overridden PrimaryColor (G5 parity with Material/Simple)");
		}
		finally
		{
			appDictionaries.Remove(theme);
		}
	}

	[TestMethod]
	[RunsOnUIThread]
	public void When_SeedAndPrimaryColorOverrideBothSet_OverrideDrivesAccent()
	{
		var overrideDict = new ResourceDictionary();
		overrideDict["PrimaryColor"] = OverrideBlue;

		var theme = new FluentTheme();
		theme.Colors = new ThemeColors
		{
			PrimarySeed = SeedRed,
			OverrideDictionary = overrideDict,
		};
		var container = new Grid();
		container.Resources.MergedDictionaries.Add(theme);

		Assert.AreEqual(OverrideBlue, GetColor(container.Resources, "SystemAccentColor"),
			"an explicit PrimaryColor override must drive the accent, taking precedence over the seed");

		Assert.IsTrue(
			container.Resources.TryGetValue("AccentFillColorDefaultBrush", out var fillValue)
				&& fillValue is SolidColorBrush,
			"AccentFillColorDefaultBrush should resolve under an overridden FluentTheme");
		Assert.AreEqual(OverrideBlue, ((SolidColorBrush)fillValue).Color,
			"the accent fill must carry the override, not the seed's branch-mapped tone");
	}

	[TestMethod]
	[RunsOnUIThread]
	public void When_ExplicitAccentShadeOverridden_ExplicitWins()
	{
		// The consumer override has the highest precedence for EVERY key it
		// defines — including the accent-family keys the reverse mapping would
		// otherwise derive (Given_ColorOverridePrecedence contract).
		var overrideDict = new ResourceDictionary();
		overrideDict["SystemAccentColorLight2"] = OverrideGreen;

		var theme = new FluentTheme();
		theme.Colors = new ThemeColors
		{
			PrimarySeed = SeedRed,
			OverrideDictionary = overrideDict,
		};
		var container = new Grid();
		container.Resources.MergedDictionaries.Add(theme);

		Assert.AreEqual(OverrideGreen, GetColor(container.Resources, "SystemAccentColorLight2"),
			"an explicit accent-shade override must win over the seed-derived shade");
	}

	[TestMethod]
	[RunsOnUIThread]
	public void When_PrimaryColorOverriddenViaDeprecatedDictionary_AccentFollows()
	{
		var overrideDict = new ResourceDictionary();
		overrideDict["PrimaryColor"] = OverrideBlue;

		var theme = new FluentTheme();
#pragma warning disable CS0618 // Testing the deprecated BaseTheme channel
		theme.ColorOverrideDictionary = overrideDict;
#pragma warning restore CS0618
		var container = new Grid();
		container.Resources.MergedDictionaries.Add(theme);

		Assert.AreEqual(OverrideBlue, GetColor(container.Resources, "SystemAccentColor"),
			"the obsolete ColorOverrideDictionary channel must drive the accent cascade like Colors.OverrideDictionary");
	}

	[TestMethod]
	[RunsOnUIThread]
	public void When_PrimaryColorOverriddenViaDeprecatedSource_AccentFollows()
	{
		var theme = new FluentTheme();
#pragma warning disable CS0618 // Testing the deprecated BaseTheme channel
		theme.ColorOverrideSource = ConsumerOverrideSource;
#pragma warning restore CS0618
		var container = new Grid();
		container.Resources.MergedDictionaries.Add(theme);

		var expected = IsAmbientDark ? OverrideGreen : OverrideBlue;

		Assert.AreEqual(expected, GetColor(container.Resources, "PrimaryColor"),
			"the obsolete ColorOverrideSource channel must reach the semantic palette under FluentTheme");
		Assert.AreEqual(expected, GetColor(container.Resources, "SystemAccentColor"),
			"the obsolete ColorOverrideSource channel must drive the accent cascade (branch-correct, incl. the 'Dark' key)");
	}
}
