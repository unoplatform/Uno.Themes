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

	// A muted (low-chroma) seed: TonalSpot's minimum-chroma floor visibly moves it,
	// so the two generation modes produce different accents for it.
	private static readonly Color MutedSeed = Color.FromArgb(0xFF, 0x6B, 0x72, 0x80);

	// A very dark brand color (tone ≈ 12): the dark shades must stay darker than it.
	private static readonly Color Navy = Color.FromArgb(0xFF, 0x00, 0x1F, 0x3F);

	// A pale brand color (tone ≈ 94): white on-accent text would be unreadable on it.
	private static readonly Color PaleYellow = Color.FromArgb(0xFF, 0xFF, 0xF1, 0x76);

	private static readonly Color White = Color.FromArgb(0xFF, 0xFF, 0xFF, 0xFF);
	private static readonly Color Black = Color.FromArgb(0xFF, 0x00, 0x00, 0x00);

	// Distinctive override colors, matching RuntimeTests/FluentColorOverride.xaml.
	private static readonly Color OverrideBlue = Color.FromArgb(0xFF, 0x21, 0x96, 0xF3);
	private static readonly Color OverrideGreen = Color.FromArgb(0xFF, 0x66, 0xBB, 0x6A);

	private const string ConsumerOverrideSource = "ms-appx:///RuntimeTests/FluentColorOverride.xaml";

	private static bool IsAmbientDark =>
		Application.Current.RequestedTheme == ApplicationTheme.Dark;

	/// <summary>
	/// Expected tone from the seed's primary palette under a generation mode — the
	/// recipe SeedColorPaletteGenerator applies: Fidelity (the default) keeps the
	/// seed's chroma, TonalSpot enforces M3's minimum of 48.
	/// </summary>
	private static Color Tone(Color seed, int tone, SeedColorMode mode = SeedColorMode.Fidelity)
	{
		var hct = HctColor.FromArgb((seed.A << 24) | (seed.R << 16) | (seed.G << 8) | seed.B);
		var chroma = mode == SeedColorMode.TonalSpot ? Math.Max(hct.Chroma, 48) : hct.Chroma;
		var argb = new TonalPalette(hct.Hue, chroma).GetArgb(tone);
		return Color.FromArgb(
			(byte)((argb >> 24) & 0xFF),
			(byte)((argb >> 16) & 0xFF),
			(byte)((argb >> 8) & 0xFF),
			(byte)(argb & 0xFF));
	}

	/// <summary>
	/// OWN entries only: ResourceDictionary.TryGetValue falls back to the system
	/// resources (the AMBIENT theme's XCR values) for a key the dictionary does not
	/// hold, which would defeat a per-branch assertion on a system key such as
	/// TextOnAccentFillColorPrimary. XAML-backed dictionaries cannot be enumerated
	/// on Uno; none of the keys asserted through this helper live in one.
	/// </summary>
	private static object? TryGetOwnValue(ResourceDictionary dictionary, string key)
	{
		try
		{
			foreach (var pair in dictionary)
			{
				if (pair.Key is string entryKey && entryKey == key)
				{
					return pair.Value;
				}
			}
		}
		catch (NotSupportedException)
		{
			// XAML-backed dictionary: not enumerable, and not a holder of the asserted keys.
		}

		return null;
	}

	/// <summary>
	/// Expected accent shade under the Fluent shade rule (spec 05 §9.1, relative):
	/// the dark shades sit at 3/4, 1/2 and 1/4 of the accent's own tone; Light2 is
	/// anchored at the dark-theme Primary tone (80), Light1 midway between the
	/// accent and it, Light3 midway between it and white. The accent tone is the
	/// seed's own under Fidelity and the nominal 40 under TonalSpot.
	/// </summary>
	private static Color Shade(Color seed, string shadeKey, SeedColorMode mode = SeedColorMode.Fidelity)
	{
		var hct = HctColor.FromArgb((seed.A << 24) | (seed.R << 16) | (seed.G << 8) | seed.B);
		var chroma = mode == SeedColorMode.TonalSpot ? Math.Max(hct.Chroma, 48) : hct.Chroma;
		var accentTone = mode == SeedColorMode.TonalSpot ? 40.0 : hct.Tone;
		var tone = shadeKey switch
		{
			"SystemAccentColorDark1" => accentTone * 0.75,
			"SystemAccentColorDark2" => accentTone * 0.5,
			"SystemAccentColorDark3" => accentTone * 0.25,
			"SystemAccentColorLight1" => (accentTone + 80) / 2,
			"SystemAccentColorLight2" => 80,
			"SystemAccentColorLight3" => 90,
			_ => throw new ArgumentOutOfRangeException(nameof(shadeKey), shadeKey, "not an accent shade key"),
		};
		var argb = new TonalPalette(hct.Hue, chroma).GetArgb((int)Math.Round(tone));
		return Color.FromArgb(
			(byte)((argb >> 24) & 0xFF),
			(byte)((argb >> 16) & 0xFF),
			(byte)((argb >> 8) & 0xFF),
			(byte)(argb & 0xFF));
	}

	private static double ToneOf(Color color)
		=> HctColor.FromArgb((color.A << 24) | (color.R << 16) | (color.G << 8) | color.B).Tone;

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

	/// <summary>
	/// The first definition of <paramref name="key"/> for <paramref name="branchKey"/>
	/// in the theme's resource graph, searching later (winning) merged dictionaries
	/// first — i.e. the value the framework resolves for that theme branch. Lets a
	/// test assert BOTH branches regardless of the ambient app theme
	/// (see specs/lessons.md, "dark-branch rendering is not testable in the CI host").
	/// </summary>
	private static Color? FindBranchColor(ResourceDictionary dictionary, string branchKey, string key)
		=> FindBranchValue(dictionary, branchKey, key) is Color color ? color : null;

	private static object? FindBranchValue(ResourceDictionary dictionary, string branchKey, string key)
	{
		if (dictionary.ThemeDictionaries.TryGetValue(branchKey, out var branchValue)
			&& branchValue is ResourceDictionary branch
			&& TryGetOwnValue(branch, key) is { } value)
		{
			return value;
		}

		for (var i = dictionary.MergedDictionaries.Count - 1; i >= 0; i--)
		{
			if (FindBranchValue(dictionary.MergedDictionaries[i], branchKey, key) is { } nested)
			{
				return nested;
			}
		}

		return null;
	}

	// ─────────────────────────────────────────────────────────────────────
	// Shade set (spec §9.1): SystemAccentColor* are derived RELATIVE to the
	// accent's own tone (dark shades at 3/4, 1/2, 1/4 of it; Light2 anchored at
	// the dark-theme Primary tone 80). The base accent is the seed itself under
	// the default Fidelity mode — see the dedicated test.
	// ─────────────────────────────────────────────────────────────────────

	[TestMethod]
	[RunsOnUIThread]
	[DataRow("SystemAccentColorLight1")]
	[DataRow("SystemAccentColorLight2")]
	[DataRow("SystemAccentColorLight3")]
	[DataRow("SystemAccentColorDark1")]
	[DataRow("SystemAccentColorDark2")]
	[DataRow("SystemAccentColorDark3")]
	public void When_SeedSet_AccentShadesFollowTheAccentTone(string shadeKey)
	{
		var container = CreateSeededContainer(SeedRed);

		Assert.AreEqual(Shade(SeedRed, shadeKey), GetColor(container.Resources, shadeKey),
			$"{shadeKey} must be the shade relative to the seed's own tone (spec 05 §9.1)");

		// The override is scoped to the theme: the app-level accent must be untouched.
		Assert.AreNotEqual(Shade(SeedRed, shadeKey), GetColor(Application.Current.Resources, shadeKey),
			$"a container-scoped seeded FluentTheme must not leak {shadeKey} to app scope");
	}

	[TestMethod]
	[RunsOnUIThread]
	[DataRow("SystemAccentColorLight1", 60)]
	[DataRow("SystemAccentColorLight2", 80)]
	[DataRow("SystemAccentColorLight3", 90)]
	[DataRow("SystemAccentColorDark1", 30)]
	[DataRow("SystemAccentColorDark2", 20)]
	[DataRow("SystemAccentColorDark3", 10)]
	public void When_TonalSpotSeedSet_ShadesAreTheSpecTones(string shadeKey, int tone)
	{
		// TonalSpot puts the accent at tone 40 — the position the relative rule
		// was calibrated on — so the shades are the spec §9.1 table exactly.
		var theme = new FluentTheme();
		theme.Colors = new ThemeColors { PrimarySeed = SeedRed, SeedColorMode = SeedColorMode.TonalSpot };
		var container = new Grid();
		container.Resources.MergedDictionaries.Add(theme);

		Assert.AreEqual(Tone(SeedRed, tone, SeedColorMode.TonalSpot), GetColor(container.Resources, shadeKey),
			$"under TonalSpot, {shadeKey} must be tone {tone} (spec 05 §9.1)");
	}

	[TestMethod]
	[RunsOnUIThread]
	public void When_DarkSeedSet_ShadesStayOrderedAroundTheAccent()
	{
		// A navy brand color (tone ≈ 12): absolute tones would make every dark
		// shade — including the light-theme accent fill, Dark1 — LIGHTER than
		// the brand color. The shades must stay ordered around the accent.
		var container = CreateSeededContainer(Navy);

		var accent = ToneOf(GetColor(container.Resources, "SystemAccentColor"));
		var dark1 = ToneOf(GetColor(container.Resources, "SystemAccentColorDark1"));
		var dark2 = ToneOf(GetColor(container.Resources, "SystemAccentColorDark2"));
		var dark3 = ToneOf(GetColor(container.Resources, "SystemAccentColorDark3"));
		var light1 = ToneOf(GetColor(container.Resources, "SystemAccentColorLight1"));
		var light2 = ToneOf(GetColor(container.Resources, "SystemAccentColorLight2"));
		var light3 = ToneOf(GetColor(container.Resources, "SystemAccentColorLight3"));

		Assert.IsTrue(dark3 < dark2 && dark2 < dark1 && dark1 < accent,
			$"the dark shades must be ordered and darker than the accent: Dark3 {dark3:F1} < Dark2 {dark2:F1} < Dark1 {dark1:F1} < accent {accent:F1}");
		Assert.IsTrue(accent < light1 && light1 < light2 && light2 < light3,
			$"the light shades must be ordered and lighter than the accent: accent {accent:F1} < Light1 {light1:F1} < Light2 {light2:F1} < Light3 {light3:F1}");
	}

	[TestMethod]
	[RunsOnUIThread]
	public void When_SeedSet_SystemAccentColorIsTheSeed()
	{
		var container = CreateSeededContainer(SeedRed);

		// Fidelity (the default SeedColorMode) pins the generated light PrimaryColor
		// to the seed verbatim, so the base accent must be the seed too (§9.3).
		Assert.AreEqual(SeedRed, GetColor(container.Resources, "SystemAccentColor"),
			"under the default Fidelity mode, SystemAccentColor must be the seed verbatim");
		Assert.AreNotEqual(SeedRed, GetColor(Application.Current.Resources, "SystemAccentColor"),
			"a container-scoped seeded FluentTheme must not leak SystemAccentColor to app scope");
	}

	[TestMethod]
	[RunsOnUIThread]
	public void When_SeedSetInTonalSpotMode_AccentFollowsBoostedPalette()
	{
		var theme = new FluentTheme();
		theme.Colors = new ThemeColors { PrimarySeed = MutedSeed, SeedColorMode = SeedColorMode.TonalSpot };
		var container = new Grid();
		container.Resources.MergedDictionaries.Add(theme);

		// TonalSpot: the M3 recipe — tone 40 of the chroma-boosted palette — is
		// both the generated light PrimaryColor and the base accent; a muted seed
		// is visibly re-saturated (sanity: the two modes must differ here).
		var expectedAccent = Tone(MutedSeed, 40, SeedColorMode.TonalSpot);
		Assert.AreNotEqual(MutedSeed, expectedAccent,
			"sanity: TonalSpot must move a muted seed, or this test proves nothing");

		Assert.AreEqual(expectedAccent, GetColor(container.Resources, "SystemAccentColor"),
			"under TonalSpot, SystemAccentColor must be tone 40 of the chroma-boosted palette");
		Assert.AreEqual(Tone(MutedSeed, 80, SeedColorMode.TonalSpot), GetColor(container.Resources, "SystemAccentColorLight2"),
			"under TonalSpot, the shades must come from the chroma-boosted palette too");

		// Forward/reverse agreement (§9.3) holds in this mode as well.
		var expectedAccentKey = IsAmbientDark ? "SystemAccentColorLight2" : "SystemAccentColor";
		Assert.AreEqual(
			GetColor(container.Resources, expectedAccentKey),
			GetColor(container.Resources, "PrimaryColor"),
			$"the TonalSpot semantic PrimaryColor and the reverse-mapped {expectedAccentKey} must agree (§9.3)");
	}

	// ─────────────────────────────────────────────────────────────────────
	// Accent closure (spec §9.2 / D12): the accent-derived colors and brushes
	// follow the branch mapping — light fill = Dark1, dark fill = Light2,
	// mirroring XCR's own structure (spike S4).
	// ─────────────────────────────────────────────────────────────────────

	[TestMethod]
	[RunsOnUIThread]
	public void When_SeedSet_AccentClosureFollowsBranchMapping()
	{
		var container = CreateSeededContainer(SeedRed);

		var expectedFill = Shade(SeedRed, IsAmbientDark ? "SystemAccentColorLight2" : "SystemAccentColorDark1");
		var expectedAccentText = Shade(SeedRed, IsAmbientDark ? "SystemAccentColorLight3" : "SystemAccentColorDark2");

		Assert.IsTrue(
			container.Resources.TryGetValue("AccentFillColorDefaultBrush", out var fillValue)
				&& fillValue is SolidColorBrush,
			"AccentFillColorDefaultBrush should resolve under a seeded FluentTheme");
		Assert.AreEqual(expectedFill, ((SolidColorBrush)fillValue).Color,
			"the accent fill must carry the branch-mapped seed shade (light: Dark1, dark: Light2)");

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
			"accent text must carry the branch-mapped seed shade (light: Dark2, dark: Light3)");
	}

	// ─────────────────────────────────────────────────────────────────────
	// On-accent text: Fluent's stock white (light) / black (dark) families
	// assume a mid-tone platform accent. A derived fill can be pale or very
	// dark, so the family is picked by contrast against the branch's fill —
	// both for seeds and for verbatim PrimaryColor overrides.
	// ─────────────────────────────────────────────────────────────────────

	private static Color GetBranchColor(FluentTheme theme, string branchKey, string key)
	{
		var value = FindBranchColor(theme, branchKey, key);
		Assert.IsNotNull(value, $"[{branchKey}] {key} should be written by the accent cascade");
		return value.Value;
	}

	[TestMethod]
	[RunsOnUIThread]
	[DataRow(true)]
	[DataRow(false)]
	public void When_PrimaryColorOverridden_OnAccentTextContrastsWithTheFill(bool pale)
	{
		// A flat override drives BOTH branches with the basis verbatim as the fill,
		// so the expectation is the same for each branch regardless of the ambient theme.
		var overrideDict = new ResourceDictionary();
		overrideDict["PrimaryColor"] = pale ? PaleYellow : Navy;

		var theme = new FluentTheme();
		theme.Colors = new ThemeColors { OverrideDictionary = overrideDict };
		var container = new Grid();
		container.Resources.MergedDictionaries.Add(theme);

		var expected = pale ? Black : White;
		foreach (var branch in new[] { "Light", "Default" })
		{
			Assert.AreEqual(expected, GetBranchColor(theme, branch, "TextOnAccentFillColorPrimary"),
				$"[{branch}] on-accent text must contrast with a {(pale ? "pale" : "very dark")} verbatim accent fill");
			var brush = FindBranchValue(theme, branch, "TextOnAccentFillColorPrimaryBrush") as SolidColorBrush;
			Assert.IsNotNull(brush, $"[{branch}] TextOnAccentFillColorPrimaryBrush should be written by the accent cascade");
			Assert.AreEqual(expected, brush.Color,
				$"[{branch}] the on-accent text BRUSH must carry the same family (XCR templates consume the brush)");
		}
	}

	[TestMethod]
	[RunsOnUIThread]
	public void When_PaleSeedSet_LightThemeOnAccentTextIsDark()
	{
		// Seed mode: the light-theme fill is Dark1 = 3/4 of the seed's tone. For a
		// pale seed that is still a light fill, so the light branch needs black
		// text; the dark branch fill (tone 80) keeps the stock black too.
		var theme = CreateSeededTheme(PaleYellow);
		var container = new Grid();
		container.Resources.MergedDictionaries.Add(theme);

		Assert.AreEqual(Black, GetBranchColor(theme, "Light", "TextOnAccentFillColorPrimary"),
			"a pale seed's light-theme accent fill must get black on-accent text");
		Assert.AreEqual(Black, GetBranchColor(theme, "Default", "TextOnAccentFillColorPrimary"),
			"the dark-theme accent fill (tone 80) keeps black on-accent text");
	}

	[TestMethod]
	[RunsOnUIThread]
	public void When_MidToneSeedSet_OnAccentTextIsTheStockFamily()
	{
		// A mid-tone seed reproduces Fluent's stock families: white on the light
		// fill (Dark1), black on the dark fill (Light2).
		var theme = CreateSeededTheme(SeedRed);
		var container = new Grid();
		container.Resources.MergedDictionaries.Add(theme);

		Assert.AreEqual(White, GetBranchColor(theme, "Light", "TextOnAccentFillColorPrimary"),
			"a mid-tone seed keeps white on-accent text in the light theme");
		Assert.AreEqual(Black, GetBranchColor(theme, "Default", "TextOnAccentFillColorPrimary"),
			"a mid-tone seed keeps black on-accent text in the dark theme");
	}

	[TestMethod]
	[RunsOnUIThread]
	public async Task When_PalePrimaryColorOverridden_RenderedAccentButtonTextIsDark()
	{
		var overrideDict = new ResourceDictionary();
		overrideDict["PrimaryColor"] = PaleYellow;

		var theme = new FluentTheme();
		theme.Colors = new ThemeColors { OverrideDictionary = overrideDict };

		var appDictionaries = Application.Current.Resources.MergedDictionaries;
		appDictionaries.Add(theme);
		try
		{
			var button = new Button
			{
				Content = "pale",
				Style = (Style)Application.Current.Resources["AccentButtonStyle"],
			};
			var host = new Grid();
			host.Children.Add(button);

			UnitTestsUIContentHelper.Content = host;
			await UnitTestsUIContentHelper.WaitForLoaded(button);
			await UnitTestsUIContentHelper.WaitForIdle();

			var foreground = button.Foreground as SolidColorBrush;
			Assert.IsNotNull(foreground, "the accent button should have a SolidColorBrush foreground");
			Assert.AreEqual(Black, foreground.Color,
				"a built-in accent button on a pale verbatim accent must render black text (readable)");
		}
		finally
		{
			appDictionaries.Remove(theme);
		}
	}

	// ─────────────────────────────────────────────────────────────────────
	// G5, rendered: a BUILT-IN Fluent control follows the seed in the
	// documented consumer topology (FluentTheme at app scope, after XCR).
	// ─────────────────────────────────────────────────────────────────────

	[TestMethod]
	[RunsOnUIThread]
	public async Task When_SeedSet_RenderedAccentButtonFollowsSeed()
	{
		var expectedFill = Shade(SeedRed, IsAmbientDark ? "SystemAccentColorLight2" : "SystemAccentColorDark1");

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
		Assert.AreNotEqual(SeedRed, platformAccent,
			"sanity: the platform accent must differ from the seed for this test to be meaningful");

		var theme = CreateSeededTheme(SeedRed);
		appDictionaries.Add(theme);
		try
		{
			Assert.AreEqual(SeedRed, GetColor(Application.Current.Resources, "SystemAccentColor"),
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

		// Seeded semantic PrimaryColor: the seed verbatim (light branch, Fidelity)
		// / tone 80 (dark branch) — which the reverse mapping exposes as
		// SystemAccentColor and SystemAccentColorLight2 (Fluent's dark-theme
		// accent fill, anchored at that tone) respectively.
		var expectedAccentKey = IsAmbientDark ? "SystemAccentColorLight2" : "SystemAccentColor";

		Assert.AreEqual(
			GetColor(container.Resources, expectedAccentKey),
			GetColor(container.Resources, "PrimaryColor"),
			$"the seeded semantic PrimaryColor and the reverse-mapped {expectedAccentKey} must agree (§9.3)");
	}

	[TestMethod]
	[RunsOnUIThread]
	public void When_SeedSet_ForwardAndReverseFlowsAgree_InBothBranches()
	{
		// The ambient-branch test above can only see one branch per host (the
		// CI host runs Light, a dark-mode developer machine runs Dark). Compare
		// the generated palette's OWN branch values against the (theme-invariant)
		// reverse-mapped shades so the contract is proven for both branches in
		// either host.
		var theme = CreateSeededTheme(SeedRed);
		var container = new Grid();
		container.Resources.MergedDictionaries.Add(theme);

		var lightPrimary = FindBranchColor(theme, "Light", "PrimaryColor");
		var darkPrimary = FindBranchColor(theme, "Default", "PrimaryColor");
		Assert.IsNotNull(lightPrimary, "the seeded theme should carry a Light-branch PrimaryColor");
		Assert.IsNotNull(darkPrimary, "the seeded theme should carry a Default (dark) branch PrimaryColor");

		Assert.AreEqual(lightPrimary, GetColor(container.Resources, "SystemAccentColor"),
			"the LIGHT semantic PrimaryColor must equal the reverse-mapped base accent (§9.3)");
		Assert.AreEqual(darkPrimary, GetColor(container.Resources, "SystemAccentColorLight2"),
			"the DARK semantic PrimaryColor (tone 80) must equal the reverse-mapped Light2 shade — Fluent's dark-theme accent fill (§9.3)");
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

	[TestMethod]
	[DataRow(false)]
	[DataRow(true)]
	[RunsOnUIThread]
	public void When_AccentOverridesAreMerged_AppearanceAndSiblingPrecedenceArePreserved(bool mergeInsideAppearance)
	{
		var overrides = new ResourceDictionary();
		foreach (var appearance in new[] { "Light", "Dark" })
		{
			var expected = appearance == "Light" ? OverrideBlue : OverrideGreen;
			var first = new ResourceDictionary { ["PrimaryColor"] = SeedRed };
			var last = new ResourceDictionary { ["PrimaryColor"] = expected };
			var branch = new ResourceDictionary();
			if (mergeInsideAppearance)
			{
				branch.MergedDictionaries.Add(first);
				branch.MergedDictionaries.Add(last);
				overrides.ThemeDictionaries[appearance] = branch;
			}
			else
			{
				var earlier = new ResourceDictionary();
				earlier.ThemeDictionaries[appearance] = first;
				var later = new ResourceDictionary();
				later.ThemeDictionaries[appearance] = last;
				overrides.MergedDictionaries.Add(earlier);
				overrides.MergedDictionaries.Add(later);
			}
		}

		var theme = new FluentTheme { Colors = new ThemeColors { OverrideDictionary = overrides } };

		Assert.AreEqual(OverrideBlue, GetBranchColor(theme, "Light", "AccentFillColorDefault"),
			"the later merged Light override must become the native Light accent fill");
		Assert.AreEqual(OverrideGreen, GetBranchColor(theme, "Default", "AccentFillColorDefault"),
			"the later merged Dark override must become the native Dark accent fill");
	}

	[TestMethod]
	[RunsOnUIThread]
	public void When_MergedNativeAccentBrushOverridesSeed_ExplicitBrushAndOpacityWin()
	{
		var expected = new SolidColorBrush(OverrideBlue) { Opacity = 0.43 };
		var overrides = new ResourceDictionary();
		overrides.MergedDictionaries.Add(new ResourceDictionary { ["AccentFillColorDefaultBrush"] = expected });
		var theme = new FluentTheme
		{
			Colors = new ThemeColors { PrimarySeed = SeedRed, OverrideDictionary = overrides },
		};

		foreach (var appearance in new[] { "Light", "Default" })
		{
			var actual = FindBranchValue(theme, appearance, "AccentFillColorDefaultBrush") as SolidColorBrush;
			Assert.IsNotNull(actual, $"{appearance} must expose the explicit native fill brush");
			Assert.AreEqual(expected.Color, actual.Color, $"{appearance} must preserve the explicit brush color");
			Assert.AreEqual(expected.Opacity, actual.Opacity, 0.0001, $"{appearance} must preserve the explicit brush opacity");
		}
	}

	[TestMethod]
	[DataRow("PrimaryBrush", false)]
	[DataRow("PrimaryBrush", true)]
	[DataRow("OnPrimaryColor", false)]
	[DataRow("OnPrimaryColor", true)]
	[DataRow("OnPrimaryBrush", false)]
	[DataRow("OnPrimaryBrush", true)]
	[RunsOnUIThread]
	public async Task When_SemanticAccentOverrideSet_RenderedNativeButtonUsesColorAndOpacity(string key, bool withSeed)
	{
		var overrides = new ResourceDictionary();
		foreach (var appearance in new[] { "Light", "Dark" })
		{
			var color = appearance == "Light" ? OverrideBlue : OverrideGreen;
			var branch = new ResourceDictionary();
			branch[key] = key == "OnPrimaryColor" ? color : new SolidColorBrush(color) { Opacity = 0.43 };
			// A brush override must win over the color it normally derives from.
			if (key == "PrimaryBrush")
			{
				branch["PrimaryColor"] = Navy;
			}
			else if (key == "OnPrimaryBrush")
			{
				branch["OnPrimaryColor"] = PaleYellow;
			}
			overrides.ThemeDictionaries[appearance] = branch;
		}
		var theme = new FluentTheme
		{
			Colors = new ThemeColors { PrimarySeed = withSeed ? SeedRed : null, OverrideDictionary = overrides },
		};
		var appDictionaries = Application.Current.Resources.MergedDictionaries;
		appDictionaries.Add(theme);
		try
		{
			var button = new Button { Content = key, Style = (Style)Application.Current.Resources["FilledButtonStyle"] };
			var host = new Grid();
			host.Children.Add(button);
			UnitTestsUIContentHelper.Content = host;
			await UnitTestsUIContentHelper.WaitForLoaded(button);
			await UnitTestsUIContentHelper.WaitForIdle();

			var actual = (key == "PrimaryBrush" ? button.Background : button.Foreground) as SolidColorBrush;
			Assert.IsNotNull(actual, "the native button must use the semantic solid brush override");
			Assert.AreEqual(IsAmbientDark ? OverrideGreen : OverrideBlue, actual.Color,
				"the same semantic accent override must reach the native button in the active appearance");
			Assert.AreEqual(key == "OnPrimaryColor" ? 1.0 : 0.43, actual.Opacity, 0.0001,
				"a semantic brush override must preserve its opacity on the rendered button");
		}
		finally
		{
			appDictionaries.Remove(theme);
		}
	}

	[TestMethod]
	[DataRow("Light")]
	[DataRow("Dark")]
	[RunsOnUIThread]
	public async Task When_PrimaryOverrideIsAppearanceSpecific_OtherAppearanceKeepsNativeAccent(string overrideAppearance)
	{
		var expected = GetColor(Application.Current.Resources,
			IsAmbientDark ? "SystemAccentColorLight2" : "SystemAccentColorDark1");
		var overrides = new ResourceDictionary();
		overrides.ThemeDictionaries[overrideAppearance] = new ResourceDictionary { ["PrimaryColor"] = OverrideBlue };
		var theme = new FluentTheme { Colors = new ThemeColors { OverrideDictionary = overrides } };
		var appDictionaries = Application.Current.Resources.MergedDictionaries;
		appDictionaries.Add(theme);
		try
		{
			var button = new Button { Content = overrideAppearance, Style = (Style)Application.Current.Resources["FilledButtonStyle"] };
			var host = new Grid();
			host.Children.Add(button);
			UnitTestsUIContentHelper.Content = host;
			await UnitTestsUIContentHelper.WaitForLoaded(button);
			await UnitTestsUIContentHelper.WaitForIdle();

			var actual = button.Background as SolidColorBrush;
			Assert.IsNotNull(actual, "the native accent button must retain a solid background");
			var applies = overrideAppearance == (IsAmbientDark ? "Dark" : "Light");
			Assert.AreEqual(applies ? OverrideBlue : expected, actual.Color,
				"an appearance-specific override must not recolor the opposite appearance through Default fallback");
		}
		finally
		{
			appDictionaries.Remove(theme);
		}
	}
}
