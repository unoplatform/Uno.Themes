using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Uno.Simple;
using Uno.Themes.ColorGeneration;
using Uno.Themes.ColorGeneration.Hct;
using Uno.UI.RuntimeTests;
using Windows.UI;

namespace Uno.Themes.Samples.RuntimeTests;

/// <summary>
/// Verifies the HCT color space implementation and seed-based palette generation.
/// The HCT tests are pure logic; the palette tests render a seeded theme so they
/// assert the colors a consumer actually sees.
/// </summary>
[TestClass]
public class Given_SeedColorPalette
{
	// The solver delivers the requested chroma exactly whenever it is in gamut, so a
	// round trip is lossless apart from 8-bit rounding. Anything beyond a couple of
	// units means the gamut search has regressed.
	private const int RoundTripTolerance = 2;

	// WCAG AA for normal text.
	private const double WcagAaContrast = 4.5;

	// ─────────────────────────────────────────────────────────────────────
	// HCT round-trip: ARGB → HCT → ARGB.
	// The saturated rows are the ones that matter: a solver that clamps chroma
	// short of the gamut boundary only fails on colors that need high chroma.
	// ─────────────────────────────────────────────────────────────────────

	[TestMethod]
	[DataRow(unchecked((int)0xFF000000), "Black")]
	[DataRow(unchecked((int)0xFFFFFFFF), "White")]
	[DataRow(unchecked((int)0xFF808080), "Mid Gray")]
	[DataRow(unchecked((int)0xFF6750A4), "Material Purple")]
	[DataRow(unchecked((int)0xFF386A20), "Green")]
	[DataRow(unchecked((int)0xFFFF0000), "Pure Red")]
	[DataRow(unchecked((int)0xFF00FF00), "Pure Green")]
	[DataRow(unchecked((int)0xFF0000FF), "Pure Blue")]
	[DataRow(unchecked((int)0xFFFFFF00), "Pure Yellow")]
	[DataRow(unchecked((int)0xFF00FFFF), "Pure Cyan")]
	[DataRow(unchecked((int)0xFFFF00FF), "Pure Magenta")]
	[DataRow(unchecked((int)0xFFB3261E), "M3 Baseline Error")]
	[DataRow(unchecked((int)0xFFEC221F), "Simple Error")]
	public void When_RoundTripping_Argb_Through_Hct_Then_ColorIsPreserved(int argb, string name)
	{
		var hct = HctColor.FromArgb(argb);
		int roundTripped = hct.ToArgb();

		int rOrig = (argb >> 16) & 0xFF;
		int gOrig = (argb >> 8) & 0xFF;
		int bOrig = argb & 0xFF;
		int rRT = (roundTripped >> 16) & 0xFF;
		int gRT = (roundTripped >> 8) & 0xFF;
		int bRT = roundTripped & 0xFF;

		Assert.IsTrue(Math.Abs(rOrig - rRT) <= RoundTripTolerance,
			$"{name}: Red channel off by {Math.Abs(rOrig - rRT)} (expected {rOrig}, got {rRT})");
		Assert.IsTrue(Math.Abs(gOrig - gRT) <= RoundTripTolerance,
			$"{name}: Green channel off by {Math.Abs(gOrig - gRT)} (expected {gOrig}, got {gRT})");
		Assert.IsTrue(Math.Abs(bOrig - bRT) <= RoundTripTolerance,
			$"{name}: Blue channel off by {Math.Abs(bOrig - bRT)} (expected {bOrig}, got {bRT})");
	}

	// ─────────────────────────────────────────────────────────────────────
	// Gamut coverage: the requested chroma must be delivered when it is achievable.
	// material-color-utilities is the oracle — its error palette is published, so a
	// clamping solver is caught by comparing against values we do not control.
	// ─────────────────────────────────────────────────────────────────────

	[TestMethod]
	[DataRow(10, unchecked((int)0xFF410002))]
	[DataRow(20, unchecked((int)0xFF690005))]
	[DataRow(30, unchecked((int)0xFF93000A))]
	[DataRow(40, unchecked((int)0xFFBA1A1A))]
	[DataRow(80, unchecked((int)0xFFFFB4AB))]
	[DataRow(90, unchecked((int)0xFFFFDAD6))]
	public void When_GeneratingM3ErrorPalette_Then_ToneMatchesReferenceImplementation(int tone, int expectedArgb)
	{
		// The M3 error palette is TonalPalette(hue 25, chroma 84) — the same fixed
		// values material-color-utilities uses, so every tone is directly comparable.
		int actual = new TonalPalette(25, 84).GetArgb(tone);

		Assert.AreEqual(expectedArgb, actual,
			$"Tone {tone}: expected #{expectedArgb & 0xFFFFFF:X6}, got #{actual & 0xFFFFFF:X6}");
	}

	[TestMethod]
	[DataRow(25.0, 84.0, 40)]
	[DataRow(280.0, 48.0, 40)]
	[DataRow(244.0, 16.0, 40)]
	[DataRow(140.0, 32.0, 60)]
	public void When_RequestedChromaIsInGamut_Then_ItIsDelivered(double hue, double chroma, int tone)
	{
		var hct = HctColor.FromArgb(new TonalPalette(hue, chroma).GetArgb(tone));

		Assert.IsTrue(Math.Abs(hct.Chroma - chroma) <= 1.0,
			$"Requested chroma {chroma} at hue {hue} / tone {tone} but got {hct.Chroma:F1} — " +
			"the gamut search is giving up before the sRGB boundary.");
		Assert.IsTrue(Math.Abs(hct.Hue - hue) <= 1.0,
			$"Requested hue {hue} but got {hct.Hue:F1}");
	}

	// ─────────────────────────────────────────────────────────────────────
	// HCT values: verify known colors produce expected HCT ranges.
	// ─────────────────────────────────────────────────────────────────────

	[TestMethod]
	public void When_ConvertingMaterialPurple_Then_HctValuesAreCorrect()
	{
		// #6750A4 — Material Design 3 default primary
		var hct = HctColor.FromArgb(unchecked((int)0xFF6750A4));

		// Hue should be in the purple range (~270-290)
		Assert.IsTrue(hct.Hue > 260 && hct.Hue < 300,
			$"Hue should be ~280 for purple, got {hct.Hue:F1}");
		// Chroma should be moderate (~40-60)
		Assert.IsTrue(hct.Chroma > 30 && hct.Chroma < 70,
			$"Chroma should be ~50 for Material purple, got {hct.Chroma:F1}");
		// Tone should be ~40 (medium-dark)
		Assert.IsTrue(hct.Tone > 30 && hct.Tone < 50,
			$"Tone should be ~40 for Material purple, got {hct.Tone:F1}");
	}

	[TestMethod]
	public void When_ConvertingPureRed_Then_HueIsNearZero()
	{
		var hct = HctColor.FromArgb(unchecked((int)0xFFFF0000));

		// Pure red hue should be near 0/360
		Assert.IsTrue(hct.Hue < 40 || hct.Hue > 340,
			$"Pure red hue should be near 0/360, got {hct.Hue:F1}");
		// High chroma
		Assert.IsTrue(hct.Chroma > 60,
			$"Pure red should have high chroma, got {hct.Chroma:F1}");
	}

	[TestMethod]
	public void When_ConvertingBlack_Then_ToneIsZero()
	{
		var hct = HctColor.FromArgb(unchecked((int)0xFF000000));
		Assert.IsTrue(hct.Tone < 0.5, $"Black tone should be ~0, got {hct.Tone:F1}");
	}

	[TestMethod]
	public void When_ConvertingWhite_Then_ToneIs100()
	{
		var hct = HctColor.FromArgb(unchecked((int)0xFFFFFFFF));
		Assert.IsTrue(hct.Tone > 99.5, $"White tone should be ~100, got {hct.Tone:F1}");
	}

	// ─────────────────────────────────────────────────────────────────────
	// Tonal palette: tone 0 is black, tone 100 is white, monotonically
	// increasing lightness.
	// ─────────────────────────────────────────────────────────────────────

	[TestMethod]
	public void When_GeneratingTonalPalette_Then_Tone0IsBlack_And_Tone100IsWhite()
	{
		var palette = new TonalPalette(280, 48);

		int black = palette.GetArgb(0);
		Assert.AreEqual(unchecked((int)0xFF000000), black, "Tone 0 should be black");

		int white = palette.GetArgb(100);
		Assert.AreEqual(unchecked((int)0xFFFFFFFF), white, "Tone 100 should be white");
	}

	[TestMethod]
	public void When_GeneratingTonalPalette_Then_LightnessIncreasesMonotonically()
	{
		var palette = new TonalPalette(280, 48);
		int[] tones = { 0, 10, 20, 30, 40, 50, 60, 70, 80, 90, 95, 99, 100 };

		double prevLstar = -1;
		foreach (int tone in tones)
		{
			int argb = palette.GetArgb(tone);
			double lstar = ColorMathAccessor.LstarFromArgb(argb);
			Assert.IsTrue(lstar >= prevLstar,
				$"L* should increase: tone {tone} has L*={lstar:F1} but previous was {prevLstar:F1}");
			prevLstar = lstar;
		}
	}

	[TestMethod]
	public void When_GeneratingTonalPalette_Then_ToneMatchesLstar()
	{
		var palette = new TonalPalette(280, 48);
		int[] tones = { 10, 20, 30, 40, 50, 60, 70, 80, 90 };

		foreach (int tone in tones)
		{
			int argb = palette.GetArgb(tone);
			double lstar = ColorMathAccessor.LstarFromArgb(argb);
			Assert.IsTrue(Math.Abs(lstar - tone) <= 2.0,
				$"Tone {tone}: expected L*≈{tone}, got L*={lstar:F1}");
		}
	}

	// ─────────────────────────────────────────────────────────────────────
	// Contrast: On* colors should have sufficient contrast with their
	// background roles (WCAG AA ≥ 4.5:1).
	// ─────────────────────────────────────────────────────────────────────

	[TestMethod]
	[DataRow(40, 100, "Light Primary/OnPrimary")]
	[DataRow(90, 10, "Light Container/OnContainer")]
	[DataRow(80, 20, "Dark Primary/OnPrimary")]
	[DataRow(30, 90, "Dark Container/OnContainer")]
	[DataRow(99, 10, "Light Background/OnBackground")]
	[DataRow(10, 90, "Dark Background/OnBackground")]
	public void When_PairingRoles_Then_ContrastExceedsWcagAA(
		int bgTone, int fgTone, string pairName)
	{
		var palette = new TonalPalette(280, 48);
		int bgArgb = palette.GetArgb(bgTone);
		int fgArgb = palette.GetArgb(fgTone);

		double bgLstar = ColorMathAccessor.LstarFromArgb(bgArgb);
		double fgLstar = ColorMathAccessor.LstarFromArgb(fgArgb);

		// Approximate contrast ratio from L* values
		// Using relative luminance: Y = YFromLstar(L*)
		double bgY = ColorMathAccessor.YFromLstar(bgLstar) / 100.0;
		double fgY = ColorMathAccessor.YFromLstar(fgLstar) / 100.0;
		double lighter = Math.Max(bgY, fgY);
		double darker = Math.Min(bgY, fgY);
		double contrast = (lighter + 0.05) / (darker + 0.05);

		Assert.IsTrue(contrast >= 4.5,
			$"{pairName} (tones {bgTone}/{fgTone}): contrast ratio {contrast:F1}:1 is below WCAG AA (4.5:1)");
	}

	// ─────────────────────────────────────────────────────────────────────
	// Exact-seed Primary: the light PrimaryColor is the seed hex verbatim,
	// and the dark one stays derived so it remains legible on a dark surface.
	// ─────────────────────────────────────────────────────────────────────

	[TestMethod]
	[RunsOnUIThread]
	[DataRow(unchecked((int)0xFF006495), "Mid blue")]
	[DataRow(unchecked((int)0xFFFF0000), "Pure red — maximum chroma")]
	[DataRow(unchecked((int)0xFF808080), "Gray — near-zero chroma")]
	[DataRow(unchecked((int)0xFFB3D4A0), "Pale green — high tone")]
	[DataRow(unchecked((int)0xFF2B1B4D), "Deep violet — low tone")]
	[DataRow(unchecked((int)0xFF4A4E69), "Muted indigo")]
	public void When_SeedIsSet_Then_LightPrimaryIsTheSeedExactly(int seedArgb, string name)
	{
		var seed = ToColor(seedArgb);

		var primary = GetGeneratedColor(seed, "Light", "PrimaryColor");

		Assert.AreEqual(seed, primary,
			$"{name}: light PrimaryColor should be the seed verbatim but was generated as {primary}.");
	}

	[TestMethod]
	[RunsOnUIThread]
	[DataRow(unchecked((int)0xFF2B1B4D), "Deep violet — low tone")]
	[DataRow(unchecked((int)0xFF006495), "Mid blue")]
	public void When_SeedIsSet_Then_DarkPrimaryStaysDerived(int seedArgb, string name)
	{
		// A dark brand color pinned onto a dark surface would be unreadable, so the dark
		// role stays at tone 80 rather than following the seed.
		var seed = ToColor(seedArgb);

		var primary = GetGeneratedColor(seed, "Default", "PrimaryColor");

		Assert.AreNotEqual(seed, primary, $"{name}: dark PrimaryColor must not be pinned to the seed.");

		double tone = ColorMathAccessor.LstarFromArgb(ToArgb(primary));
		Assert.IsTrue(Math.Abs(tone - 80) <= 2.0,
			$"{name}: dark PrimaryColor should sit at tone 80, got {tone:F1}.");
	}

	[TestMethod]
	[RunsOnUIThread]
	[DataRow(unchecked((int)0xFFC5D8F0), "Pale seed (~T85)")]
	[DataRow(unchecked((int)0xFF808080), "Mid seed (~T54)")]
	[DataRow(unchecked((int)0xFF797900), "Mid seed at the contrast worst case (~T49)")]
	[DataRow(unchecked((int)0xFF006495), "Mid-dark seed (~T40)")]
	[DataRow(unchecked((int)0xFF2B1B4D), "Dark seed (~T15)")]
	public void When_PrimaryIsPinned_Then_OnPrimaryClearsWcagAA(int seedArgb, string name)
	{
		// With Primary pinned, its tone is whatever the consumer chose, so OnPrimary can no
		// longer be a fixed tone — it flips to whichever extreme reads on the seed.
		var seed = ToColor(seedArgb);

		var primary = GetGeneratedColor(seed, "Light", "PrimaryColor");
		var onPrimary = GetGeneratedColor(seed, "Light", "OnPrimaryColor");

		double contrast = ColorMathAccessor.ContrastRatio(ToArgb(primary), ToArgb(onPrimary));

		Assert.IsTrue(contrast >= WcagAaContrast,
			$"{name}: OnPrimary {onPrimary} on Primary {primary} is {contrast:F2}:1, below WCAG AA ({WcagAaContrast}:1).");
	}

	// ─────────────────────────────────────────────────────────────────────
	// Preserve-seed (content) mode scales every supporting palette from the seed's
	// own chroma; the pre-8.0 tonal-spot behavior remains available as an opt-out.
	// ─────────────────────────────────────────────────────────────────────

	[TestMethod]
	[RunsOnUIThread]
	[DataRow("Light")]
	[DataRow("Default")]
	public void When_GraySeedAndPreserveSeedColor_Then_SupportingPalettesStayNeutral(string themeKey)
	{
		// Regression guard: with the fixed tonal-spot chromas a gray seed produced a teal
		// Secondary (#4B6367) and a blue Tertiary (#525D7D).
		var seed = ToColor(unchecked((int)0xFF808080));

		foreach (var key in new[] { "PrimaryColor", "SecondaryColor", "TertiaryColor" })
		{
			var color = GetGeneratedColor(seed, themeKey, key);

			double chroma = HctColor.FromArgb(ToArgb(color)).Chroma;
			Assert.IsTrue(chroma < 5.0,
				$"{themeKey}/{key} generated from a gray seed should stay neutral, got chroma {chroma:F1} ({color}).");
		}
	}

	[TestMethod]
	[RunsOnUIThread]
	public void When_PreserveSeedColorIsFalse_Then_TonalSpotBehaviorApplies()
	{
		// The pre-8.0 path: Primary is derived at tone 40 with a chroma floor of 48, so a
		// gray seed is deliberately boosted into a vivid color rather than reproduced.
		var seed = ToColor(unchecked((int)0xFF808080));

		var primary = GetGeneratedColor(seed, "Light", "PrimaryColor", preserveSeedColor: false);

		Assert.AreNotEqual(seed, primary, "PreserveSeedColor=false must not pin Primary to the seed.");

		// The chroma-48 floor is requested but capped by the sRGB gamut at this hue (~35 here),
		// so assert against the seed's own chroma of ~1.9 rather than the requested figure.
		var hct = HctColor.FromArgb(ToArgb(primary));
		Assert.IsTrue(hct.Chroma > 20,
			$"PreserveSeedColor=false should apply the M3 minimum chroma to a near-gray seed, got {hct.Chroma:F1}.");
		Assert.IsTrue(Math.Abs(hct.Tone - 40) <= 2.0,
			$"PreserveSeedColor=false should derive Primary at tone 40, got {hct.Tone:F1}.");
	}

	[TestMethod]
	[RunsOnUIThread]
	public void When_SecondarySeedIsExplicit_Then_ItsOwnHueAndChromaAreUsed()
	{
		// An explicit supporting seed is never routed through the content-chroma math.
		var secondarySeed = ToColor(unchecked((int)0xFFB3261E));
		var expected = HctColor.FromArgb(ToArgb(secondarySeed));

		var secondary = GetGeneratedColor(
			ToColor(unchecked((int)0xFF808080)), "Light", "SecondaryColor",
			configure: colors => colors.SecondarySeed = secondarySeed);

		var actual = HctColor.FromArgb(ToArgb(secondary));
		Assert.IsTrue(Math.Abs(actual.Hue - expected.Hue) <= 2.0,
			$"Explicit SecondarySeed hue {expected.Hue:F1} was not used (got {actual.Hue:F1}).");
		Assert.IsTrue(actual.Chroma > 30,
			$"Explicit SecondarySeed chroma should survive, got {actual.Chroma:F1} — " +
			"the seed was routed through the derived-chroma math.");
	}

	// ─────────────────────────────────────────────────────────────────────
	// Brush propagation: a seed change has to reach the *Brush resources that
	// controls actually paint with, on the instances consumers already hold.
	// ─────────────────────────────────────────────────────────────────────

	[TestMethod]
	[RunsOnUIThread]
	public void When_SeedChanges_Then_BrushIsRecoloredInPlace()
	{
		// {ThemeResource PrimaryBrush} resolves to a brush *instance* and re-evaluates only on a
		// theme change, so a rebuild that replaces the brush leaves every rendered element on the
		// old one. The instance must survive and its Color must change.
		var container = CreateThemedContainer(out var theme);

		var held = GetBrush(container, "PrimaryBrush");
		Assert.IsNotNull(held, "PrimaryBrush should resolve from the theme");

		theme.Colors = new ThemeColors { PrimarySeed = ToColor(unchecked((int)0xFFFF0000)) };

		Assert.AreSame(held, GetBrush(container, "PrimaryBrush"),
			"PrimaryBrush must stay the same instance across a rebuild, or live elements keep the old color.");
		Assert.AreEqual(GetColor(container, "PrimaryColor"), held.Color,
			"PrimaryBrush should have been recolored to the generated PrimaryColor.");
	}

	[TestMethod]
	[RunsOnUIThread]
	public void When_SeedChanges_Then_StateBrushesFollowAndKeepTheirOpacity()
	{
		var container = CreateThemedContainer(out var theme);

		var hover = GetBrush(container, "PrimaryHoverBrush");
		Assert.IsNotNull(hover, "PrimaryHoverBrush should resolve from the theme");
		var originalOpacity = hover.Opacity;

		theme.Colors = new ThemeColors { PrimarySeed = ToColor(unchecked((int)0xFFFF0000)) };

		Assert.AreEqual(GetColor(container, "PrimaryColor"), hover.Color,
			"State brushes are tinted from the same color role and must follow it.");
		Assert.AreEqual(originalOpacity, hover.Opacity, 0.0001,
			"The per-state Opacity comes from XAML and must survive recoloring.");
	}

	[TestMethod]
	[RunsOnUIThread]
	public void When_SeedIsCleared_Then_BrushesRevertToTheBasePalette()
	{
		var container = CreateThemedContainer(out var theme);

		var held = GetBrush(container, "PrimaryBrush");
		var baseColor = held.Color;

		theme.Colors = new ThemeColors { PrimarySeed = ToColor(unchecked((int)0xFFFF0000)) };
		Assert.AreNotEqual(baseColor, held.Color, "Sanity: the seed should have changed the brush.");

		theme.Colors.PrimarySeed = null;

		Assert.AreEqual(baseColor, held.Color,
			"Clearing the seed must restore the theme's own palette, not leave the last seed applied.");
	}

	[TestMethod]
	[RunsOnUIThread]
	public void When_SeedIsSet_Then_ErrorBrushesKeepTheBasePalette()
	{
		// Error is deliberately not generated from the seed (M3 pins it), so its brushes must
		// not drift when a seed is applied.
		var container = CreateThemedContainer(out var theme);

		var error = GetBrush(container, "ErrorBrush");
		var baseError = error.Color;

		theme.Colors = new ThemeColors { PrimarySeed = ToColor(unchecked((int)0xFF006495)) };

		Assert.AreEqual(baseError, error.Color, "ErrorBrush must not follow the seed.");
		Assert.AreEqual(GetColor(container, "ErrorColor"), error.Color,
			"ErrorBrush should still match the theme's ErrorColor.");
	}

	[TestMethod]
	[RunsOnUIThread]
	public async Task When_SeedIsSet_Then_ARenderedControlPaintsWithIt()
	{
		// End to end: what the generator produced is what a themed control actually paints.
		var seed = ToColor(unchecked((int)0xFF006495));
		var theme = new SimpleTheme { Colors = new ThemeColors { PrimarySeed = seed } };

		var container = new Grid();
		container.Resources.MergedDictionaries.Add(theme);

		var style = container.Resources["FilledButtonStyle"] as Style;
		Assert.IsNotNull(style, "FilledButtonStyle should resolve from the theme");

		var button = new Button { Content = "Test", Style = style };
		container.Children.Add(button);

		UnitTestsUIContentHelper.Content = container;
		await UnitTestsUIContentHelper.WaitForLoaded(button);
		await UnitTestsUIContentHelper.WaitForIdle();

		var background = button.Background as SolidColorBrush;
		Assert.IsNotNull(background, "Button should have a SolidColorBrush Background");

		var expected = GetGeneratedColor(seed, ActiveThemeKey, "PrimaryColor");
		Assert.AreEqual(expected, background.Color,
			$"A filled button should paint with the generated PrimaryColor for the active ({ActiveThemeKey}) theme.");
	}

	[TestMethod]
	[RunsOnUIThread]
	public async Task When_SeedChangesAfterRender_Then_TheControlRepaints()
	{
		// The color-picker scenario: the control is already on screen when the seed changes.
		// Nothing re-navigates and no theme change occurs, so this only works if the brush the
		// control is painting with is recolored in place.
		var theme = new SimpleTheme();
		var container = new Grid();
		container.Resources.MergedDictionaries.Add(theme);

		var style = container.Resources["FilledButtonStyle"] as Style;
		Assert.IsNotNull(style, "FilledButtonStyle should resolve from the theme");

		var button = new Button { Content = "Test", Style = style };
		container.Children.Add(button);

		UnitTestsUIContentHelper.Content = container;
		await UnitTestsUIContentHelper.WaitForLoaded(button);
		await UnitTestsUIContentHelper.WaitForIdle();

		var background = button.Background as SolidColorBrush;
		Assert.IsNotNull(background, "Button should have a SolidColorBrush Background");
		var before = background.Color;

		var seed = ToColor(unchecked((int)0xFF1EA41D));
		theme.Colors = new ThemeColors { PrimarySeed = seed };
		await UnitTestsUIContentHelper.WaitForIdle();

		var expected = GetGeneratedColor(seed, ActiveThemeKey, "PrimaryColor");
		Assert.AreNotEqual(before, expected, "Sanity: the seed must actually change the color.");
		Assert.AreEqual(expected, (button.Background as SolidColorBrush)?.Color,
			"An already-rendered control must repaint when the seed changes.");
	}

	// ─────────────────────────────────────────────────────────────────────
	// Helpers
	// ─────────────────────────────────────────────────────────────────────

	/// <summary>The theme dictionary key matching the application's current theme.</summary>
	private static string ActiveThemeKey =>
		Application.Current.RequestedTheme == ApplicationTheme.Light ? "Light" : "Default";

	private static Grid CreateThemedContainer(out SimpleTheme theme)
	{
		theme = new SimpleTheme();
		var container = new Grid();
		container.Resources.MergedDictionaries.Add(theme);
		return container;
	}

	private static SolidColorBrush GetBrush(Grid container, string key)
	{
		Assert.IsTrue(container.Resources.TryGetValue(key, out var value), $"{key} should resolve from the theme");
		return value as SolidColorBrush;
	}

	private static Color GetColor(Grid container, string key)
	{
		Assert.IsTrue(container.Resources.TryGetValue(key, out var value), $"{key} should resolve from the theme");
		return (Color)value;
	}

	/// <summary>
	/// Reads one generated color for an explicit theme.
	/// </summary>
	/// <remarks>
	/// <see cref="ResourceDictionary.TryGetValue"/> always resolves theme dictionaries against
	/// the <em>application</em> theme, so it cannot be used to assert the Light and Dark roles
	/// independently. The seed palette is merged last of the code-generated color layers, so
	/// taking the last matching theme dictionary in the theme's merge tree yields the value the
	/// generator produced. <c>Given_ColorOverridePrecedence</c> covers precedence against a
	/// consumer override separately.
	/// </remarks>
	private static Color GetGeneratedColor(
		Color seed,
		string themeKey,
		string colorKey,
		bool? preserveSeedColor = null,
		Action<ThemeColors>? configure = null)
	{
		var colors = new ThemeColors { PrimarySeed = seed };
		if (preserveSeedColor is { } preserve)
		{
			colors.PreserveSeedColor = preserve;
		}
		configure?.Invoke(colors);

		var theme = new SimpleTheme { Colors = colors };

		object? found = null;
		Visit(theme);

		Assert.IsNotNull(found, $"{colorKey} was not generated into the '{themeKey}' theme dictionary");
		return (Color)found;

		void Visit(ResourceDictionary dictionary)
		{
			if (dictionary.ThemeDictionaries.TryGetValue(themeKey, out var themed)
				&& themed is ResourceDictionary themedDictionary
				&& themedDictionary.TryGetValue(colorKey, out var value))
			{
				found = value;
			}

			foreach (var merged in dictionary.MergedDictionaries)
			{
				Visit(merged);
			}
		}
	}

	private static Color ToColor(int argb) => Color.FromArgb(
		(byte)((argb >> 24) & 0xFF), (byte)((argb >> 16) & 0xFF), (byte)((argb >> 8) & 0xFF), (byte)(argb & 0xFF));

	private static int ToArgb(Color color) => (color.A << 24) | (color.R << 16) | (color.G << 8) | color.B;

	/// <summary>
	/// Accessor for internal ColorMath methods used by tests.
	/// </summary>
	private static class ColorMathAccessor
	{
		public static double ContrastRatio(int argb1, int argb2)
		{
			double y1 = YFromArgb(argb1) / 100.0;
			double y2 = YFromArgb(argb2) / 100.0;
			return (Math.Max(y1, y2) + 0.05) / (Math.Min(y1, y2) + 0.05);
		}

		private static double YFromArgb(int argb) =>
			0.2126 * Linearized((argb >> 16) & 0xFF)
			+ 0.7152 * Linearized((argb >> 8) & 0xFF)
			+ 0.0722 * Linearized(argb & 0xFF);

		public static double LstarFromArgb(int argb)
		{
			double yNorm = YFromArgb(argb) / 100.0;
			double e = 216.0 / 24389.0;
			double k = 24389.0 / 27.0;
			return yNorm <= e ? k * yNorm : 116.0 * Math.Pow(yNorm, 1.0 / 3.0) - 16.0;
		}

		public static double YFromLstar(double lstar)
		{
			if (lstar <= 8.0)
				return lstar / (24389.0 / 27.0) * 100.0;
			double cubeRoot = (lstar + 16.0) / 116.0;
			return cubeRoot * cubeRoot * cubeRoot * 100.0;
		}

		private static double Linearized(int component)
		{
			double normalized = component / 255.0;
			return normalized <= 0.040449936
				? normalized / 12.92 * 100.0
				: Math.Pow((normalized + 0.055) / 1.055, 2.4) * 100.0;
		}
	}
}
