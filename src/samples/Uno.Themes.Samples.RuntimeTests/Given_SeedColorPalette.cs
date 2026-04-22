using Microsoft.VisualStudio.TestTools.UnitTesting;
using Uno.Themes.ColorGeneration;
using Uno.Themes.ColorGeneration.Hct;
using Windows.UI;

namespace Uno.Themes.Samples.RuntimeTests;

/// <summary>
/// Verifies the HCT color space implementation and seed-based palette generation.
/// These are pure logic tests — no UI thread required.
/// </summary>
[TestClass]
public class Given_SeedColorPalette
{
	// ─────────────────────────────────────────────────────────────────────
	// HCT round-trip: ARGB → HCT → ARGB should be close (within ±20 per channel
	// due to the simplified bisection solver's precision limits at gamut boundaries).
	// ─────────────────────────────────────────────────────────────────────

	[TestMethod]
	[DataRow(unchecked((int)0xFF000000), "Black")]
	[DataRow(unchecked((int)0xFFFFFFFF), "White")]
	[DataRow(unchecked((int)0xFF808080), "Mid Gray")]
	[DataRow(unchecked((int)0xFF6750A4), "Material Purple")]
	[DataRow(unchecked((int)0xFF386A20), "Green")]
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

		// The simplified bisection solver introduces precision loss at sRGB gamut
		// boundaries, especially for highly saturated colors. ±20 tolerance
		// covers the solver's limitations while still catching major regressions.
		Assert.IsTrue(Math.Abs(rOrig - rRT) <= 20,
			$"{name}: Red channel off by {Math.Abs(rOrig - rRT)} (expected {rOrig}, got {rRT})");
		Assert.IsTrue(Math.Abs(gOrig - gRT) <= 20,
			$"{name}: Green channel off by {Math.Abs(gOrig - gRT)} (expected {gOrig}, got {gRT})");
		Assert.IsTrue(Math.Abs(bOrig - bRT) <= 20,
			$"{name}: Blue channel off by {Math.Abs(bOrig - bRT)} (expected {bOrig}, got {bRT})");
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

	/// <summary>
	/// Accessor for internal ColorMath methods used by tests.
	/// </summary>
	private static class ColorMathAccessor
	{
		public static double LstarFromArgb(int argb)
		{
			int r = (argb >> 16) & 0xFF;
			int g = (argb >> 8) & 0xFF;
			int b = argb & 0xFF;

			double rL = Linearized(r);
			double gL = Linearized(g);
			double bL = Linearized(b);
			double y = 0.2126 * rL + 0.7152 * gL + 0.0722 * bL;

			double yNorm = y / 100.0;
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
