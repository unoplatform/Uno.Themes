using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Uno.Material;
using Uno.UI.RuntimeTests;

namespace Uno.Themes.Samples.RuntimeTests;

/// <summary>
/// Regression tests for font bundling in Uno.Material. Lives in the Material head because the
/// Simple host does not reference Uno.Fonts.Roboto: the root token, every type-scale slot and the
/// re-pointed v2 per-control keys must resolve to the bundled Roboto variable-font entry point
/// (<c>Roboto.ttf</c>, whose weights resolve via the font manifest on static-font platforms), and
/// that entry point must actually load and honor <c>FontWeight</c>.
/// </summary>
[TestClass]
public class Given_Fonts
{
	private const string RobotoEntryPoint = "ms-appx:///Uno.Fonts.Roboto/Fonts/Roboto.ttf#Roboto";

	// A family that does not exist anywhere: what a missing entry point degrades to.
	private const string MissingEntryPoint = "ms-appx:///Uno.Fonts.Roboto/Fonts/DoesNotExist.ttf#Missing";

	private static Grid CreateThemedContainer()
	{
		var container = new Grid();
		container.Resources.MergedDictionaries.Add(new MaterialTheme());
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

	/// <summary>
	/// Skia loads file fonts asynchronously and re-measures text when they land; polls until the
	/// condition holds or a bounded wait elapses, so the assertion that follows judges settled layout.
	/// </summary>
	private static async Task WaitForFontLoad(Func<bool> settled)
	{
		for (var attempt = 0; attempt < 50 && !settled(); attempt++)
		{
			await Task.Delay(100);
			await UnitTestsUIContentHelper.WaitForIdle();
		}
	}

	[TestMethod]
	[RunsOnUIThread]
	public void When_MaterialThemeLoaded_Then_RootUsesRobotoEntryPoint()
	{
		var container = CreateThemedContainer();
		var fontFamily = GetFontFamily(container, "DefaultFontFamily");

		Assert.AreEqual(
			RobotoEntryPoint,
			fontFamily.Source,
			"DefaultFontFamily must reference the bundled Roboto entry point, " +
			$"so its weights resolve from the single family. Actual Source: '{fontFamily.Source}'");
	}

	// ─────────────────────────────────────────────────────────────────────
	// Typography scale and the v2 per-control keys: every FontFamily key that the
	// collapse re-pointed derives from the single root.
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
	[DataRow("HyperlinkButtonFontFamily")]
	[DataRow("SliderFontFamily")]
	[DataRow("TextToggleButtonFontFamily")]
	[DataRow("DatePickerFlyoutPresenterFontFamily")]
	[DataRow("RatingControlCaptionFontFamily")]
	[DataRow("SecondaryRatingControlCaptionFontFamily")]
	public void When_MaterialThemeLoaded_Then_FontFamilyKeyDerivesFromRoot(string resourceKey)
	{
		var container = CreateThemedContainer();
		var fontFamily = GetFontFamily(container, resourceKey);

		Assert.AreEqual(
			RobotoEntryPoint,
			fontFamily.Source,
			$"'{resourceKey}' must derive from the single DefaultFontFamily root. " +
			$"Actual Source: '{fontFamily.Source}'");
	}

	// ─────────────────────────────────────────────────────────────────────
	// Rendered-weight guard, the Roboto twin of the Simple host's: a missing
	// Roboto.ttf silently falls back to the platform default, which has its own Bold,
	// so the probe first proves the entry point loaded at all by measuring it against
	// a family that is known not to exist, then checks that FontWeight selects a face.
	// ─────────────────────────────────────────────────────────────────────

	[TestMethod]
	[RunsOnUIThread]
	public async Task When_WeightsDiffer_Then_RenderedWidthsDiffer()
	{
		const string probe = "Weight probe 0123456789";
		var roboto = new FontFamily(RobotoEntryPoint);

		var normal = new TextBlock { Text = probe, FontFamily = roboto, FontSize = 24, FontWeight = Microsoft.UI.Text.FontWeights.Normal };
		var bold = new TextBlock { Text = probe, FontFamily = roboto, FontSize = 24, FontWeight = Microsoft.UI.Text.FontWeights.Bold };
		var fallback = new TextBlock { Text = probe, FontFamily = new FontFamily(MissingEntryPoint), FontSize = 24, FontWeight = Microsoft.UI.Text.FontWeights.Normal };

		var container = CreateThemedContainer();
		container.Children.Add(new StackPanel { Children = { normal, bold, fallback } });

		try
		{
			UnitTestsUIContentHelper.Content = container;
			await UnitTestsUIContentHelper.WaitForLoaded(fallback);
			await WaitForFontLoad(() => normal.ActualWidth != fallback.ActualWidth && normal.ActualWidth != bold.ActualWidth);

			Assert.AreNotEqual(fallback.ActualWidth, normal.ActualWidth,
				"The Roboto entry point did not load: its text measures exactly like a family that does not " +
				"exist, so both fell back to the platform default. The referenced Uno.Fonts.Roboto package is " +
				"missing the Roboto.ttf entry point.");
			Assert.AreNotEqual(normal.ActualWidth, bold.ActualWidth,
				"Bold and Normal must select different faces from the single Roboto reference. " +
				"Equal widths mean FontWeight is being ignored: the Roboto.ttf entry point loaded " +
				"but its font manifest is missing.");
		}
		finally
		{
			UnitTestsUIContentHelper.Content = null;
		}
	}
}
