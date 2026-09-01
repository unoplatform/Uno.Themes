using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Media;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Uno.Simple;
using Uno.UI.RuntimeTests;

namespace Uno.Themes.Samples.RuntimeTests;

/// <summary>
/// Regression tests for font bundling in Uno.Simple.
/// Verifies the single-typeface contract: every font family resource resolves to the bundled
/// Inter variable-font entry point (<c>Inter.ttf</c>, whose weights resolve via the font manifest
/// on static-font platforms), and the per-scale weight nuance lives in the <c>*FontWeight</c>
/// tokens rather than in weight-specific font files.
/// </summary>
[TestClass]
public class Given_Fonts
{
	private const string InterEntryPoint = "ms-appx:///Uno.Fonts.Inter/Fonts/Inter.ttf#Inter";

	private static Grid CreateThemedContainer()
	{
		var theme = new SimpleTheme();
		var container = new Grid();
		container.Resources.MergedDictionaries.Add(theme);
		return container;
	}

	private static FontFamily GetFontFamily(Grid container, string key)
	{
		if (container.Resources.TryGetValue(key, out var value) && value is FontFamily ff)
		{
			return ff;
		}

		Assert.Fail($"Resource '{key}' not found or not of type FontFamily");
		return null!;
	}

	private static string GetString(Grid container, string key)
	{
		if (container.Resources.TryGetValue(key, out var value) && value is string s)
		{
			return s;
		}

		Assert.Fail($"Resource '{key}' not found or not of type string");
		return null!;
	}

	// ─────────────────────────────────────────────────────────────────────
	// Font URI: the root token and the legacy key must use the ms-appx:/// URI of the
	// bundled Inter entry point (not a bare "Inter" system name, and not a weight-baked file).
	// ─────────────────────────────────────────────────────────────────────

	[TestMethod]
	[RunsOnUIThread]
	[DataRow("DefaultFontFamily")]
	[DataRow("SimpleFontFamily")]
	public void When_SimpleThemeLoaded_Then_FontFamilyUsesInterEntryPoint(string resourceKey)
	{
		var container = CreateThemedContainer();
		var fontFamily = GetFontFamily(container, resourceKey);

		Assert.AreEqual(
			InterEntryPoint,
			fontFamily.Source,
			$"Font resource '{resourceKey}' must reference the bundled Inter entry point, " +
			$"so its weights resolve from the single family. Actual Source: '{fontFamily.Source}'");
	}

	// ─────────────────────────────────────────────────────────────────────
	// Typography scale: every type-scale slot derives from the single root.
	// ─────────────────────────────────────────────────────────────────────

	[TestMethod]
	[RunsOnUIThread]
	[DataRow("DisplayLargeFontFamily")]
	[DataRow("DisplayMediumFontFamily")]
	[DataRow("DisplaySmallFontFamily")]
	[DataRow("HeadlineLargeFontFamily")]
	[DataRow("HeadlineMediumFontFamily")]
	[DataRow("HeadlineSmallFontFamily")]
	[DataRow("TitleLargeFontFamily")]
	[DataRow("TitleMediumFontFamily")]
	[DataRow("TitleSmallFontFamily")]
	[DataRow("LabelLargeFontFamily")]
	[DataRow("LabelMediumFontFamily")]
	[DataRow("LabelSmallFontFamily")]
	[DataRow("LabelExtraSmallFontFamily")]
	[DataRow("BodyLargeFontFamily")]
	[DataRow("BodyMediumFontFamily")]
	[DataRow("BodySmallFontFamily")]
	[DataRow("CaptionLargeFontFamily")]
	[DataRow("CaptionMediumFontFamily")]
	[DataRow("CaptionSmallFontFamily")]
	public void When_SimpleThemeLoaded_Then_TypographyScaleDerivesFromRoot(string resourceKey)
	{
		var container = CreateThemedContainer();
		var fontFamily = GetFontFamily(container, resourceKey);

		Assert.AreEqual(
			InterEntryPoint,
			fontFamily.Source,
			$"Typography resource '{resourceKey}' must derive from the single DefaultFontFamily root. " +
			$"Actual Source: '{fontFamily.Source}'");
	}

	// ─────────────────────────────────────────────────────────────────────
	// Weight tokens: Simple's per-scale weight nuance (Bold display, SemiBold titles)
	// is carried by the *FontWeight tokens, resolved from the single family.
	// ─────────────────────────────────────────────────────────────────────

	[TestMethod]
	[RunsOnUIThread]
	[DataRow("DisplayLargeFontWeight", "Bold")]
	[DataRow("DisplayMediumFontWeight", "Bold")]
	[DataRow("DisplaySmallFontWeight", "Normal")]
	[DataRow("HeadlineMediumFontWeight", "SemiBold")]
	[DataRow("TitleLargeFontWeight", "SemiBold")]
	[DataRow("TitleMediumFontWeight", "SemiBold")]
	[DataRow("LabelLargeFontWeight", "SemiBold")]
	[DataRow("BodyLargeFontWeight", "Normal")]
	[DataRow("CaptionMediumFontWeight", "Normal")]
	// Control-level weight tokens: buttons kept the Medium the old Inter-Medium-baked family
	// carried, and the named weights consumed by Expander/CalendarView are now actually defined.
	[DataRow("SimpleButtonFontWeight", "Medium")]
	[DataRow("SimpleToggleButtonFontWeight", "Medium")]
	[DataRow("SimpleRegularFontWeight", "Normal")]
	[DataRow("SimpleSemiBoldFontWeight", "SemiBold")]
	public void When_SimpleThemeLoaded_Then_WeightTokenCarriesTheScaleNuance(
		string resourceKey, string expectedWeight)
	{
		var container = CreateThemedContainer();

		Assert.AreEqual(
			expectedWeight,
			GetString(container, resourceKey),
			$"'{resourceKey}' must carry Simple's per-scale weight nuance now that the family is shared.");
	}

	// ─────────────────────────────────────────────────────────────────────
	// Light and Dark both declare the root: a dropped theme-dictionary entry
	// must not pass green just because the ambient theme still has one.
	// ─────────────────────────────────────────────────────────────────────

	[TestMethod]
	[RunsOnUIThread]
	public async Task When_ThemeChanges_Then_RootResolvesUnderBothAppearances()
	{
		var container = CreateThemedContainer();
		var text = new TextBlock
		{
			Text = "probe",
			Style = (Style)container.Resources["BodyMedium"],
		};
		container.Children.Add(text);

		UnitTestsUIContentHelper.Content = container;
		await UnitTestsUIContentHelper.WaitForLoaded(text);

		try
		{
			container.RequestedTheme = ElementTheme.Dark;
			await UnitTestsUIContentHelper.WaitForIdle();
			Assert.AreEqual(InterEntryPoint, text.FontFamily?.Source,
				"The Default (dark) theme dictionary must declare the Inter root.");

			container.RequestedTheme = ElementTheme.Light;
			await UnitTestsUIContentHelper.WaitForIdle();
			Assert.AreEqual(InterEntryPoint, text.FontFamily?.Source,
				"The Light theme dictionary must declare the Inter root.");
		}
		finally
		{
			container.RequestedTheme = ElementTheme.Default;
			UnitTestsUIContentHelper.Content = null;
		}
	}

	// ─────────────────────────────────────────────────────────────────────
	// Rendered-weight guard: the whole single-family design hangs on FontWeight
	// resolving a real weight from the one entry point (variable font, or its
	// font manifest on static-font platforms). Every other test asserts resource
	// strings; this one asserts glyphs. It fails when the referenced Uno.Fonts
	// packages do not carry the entry point + manifest — which is the merge gate
	// on unoplatform/uno.fonts#75 made visible.
	// ─────────────────────────────────────────────────────────────────────

	[TestMethod]
	[RunsOnUIThread]
	public async Task When_WeightsDiffer_Then_RenderedWidthsDiffer()
	{
		var family = new FontFamily(InterEntryPoint);
		const string probe = "Weight probe 0123456789";

		var normal = new TextBlock { Text = probe, FontFamily = family, FontSize = 24, FontWeight = Microsoft.UI.Text.FontWeights.Normal };
		var bold = new TextBlock { Text = probe, FontFamily = family, FontSize = 24, FontWeight = Microsoft.UI.Text.FontWeights.Bold };

		var container = CreateThemedContainer();
		container.Children.Add(new StackPanel { Children = { normal, bold } });

		UnitTestsUIContentHelper.Content = container;
		await UnitTestsUIContentHelper.WaitForLoaded(bold);
		await UnitTestsUIContentHelper.WaitForIdle();

		try
		{
			Assert.AreNotEqual(normal.ActualWidth, bold.ActualWidth,
				"Bold and Normal must select different faces from the single Inter reference. " +
				"Equal widths mean FontWeight is being ignored — the referenced Uno.Fonts.Inter " +
				"package is missing the Inter.ttf entry point or its font manifest.");
		}
		finally
		{
			UnitTestsUIContentHelper.Content = null;
		}
	}
}
