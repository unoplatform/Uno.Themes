using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Media;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Uno.Simple;
using Uno.UI.RuntimeTests;

namespace Uno.Themes.Samples.RuntimeTests;

/// <summary>
/// Regression tests for font bundling in Uno.Simple.
/// Verifies that Inter font files are properly bundled and referenced via ms-appx URIs
/// rather than relying on a system-installed "Inter" font 
/// </summary>
[TestClass]
public class Given_Fonts
{
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

	// ─────────────────────────────────────────────────────────────────────
	// Font URI: all font family resources must use ms-appx:/// URIs
	// pointing to bundled Inter TTF files (not bare "Inter" system name).
	// ─────────────────────────────────────────────────────────────────────

	[TestMethod]
	[RunsOnUIThread]
	[DataRow("SimpleFontFamily")]
	[DataRow("SimpleRegularFontFamily")]
	[DataRow("SimpleMediumFontFamily")]
	[DataRow("SimpleSemiBoldFontFamily")]
	[DataRow("SimpleBoldFontFamily")]
	[DataRow("TypefacePlain")]
	[DataRow("TypefaceBrand")]
	public void When_SimpleThemeLoaded_Then_FontFamilyUsesAppxUri(string resourceKey)
	{
		var container = CreateThemedContainer();
		var fontFamily = GetFontFamily(container, resourceKey);

		StringAssert.StartsWith(
			fontFamily.Source,
			"ms-appx:///Uno.Fonts.Inter/",
			$"Font resource '{resourceKey}' must use a ms-appx:///Uno.Fonts.Inter/ URI to a bundled font file, " +
			$"not a bare system font name. Actual Source: '{fontFamily.Source}'");
	}

	[TestMethod]
	[RunsOnUIThread]
	[DataRow("SimpleFontFamily")]
	[DataRow("SimpleRegularFontFamily")]
	[DataRow("SimpleMediumFontFamily")]
	[DataRow("SimpleSemiBoldFontFamily")]
	[DataRow("SimpleBoldFontFamily")]
	public void When_SimpleThemeLoaded_Then_FontFamilyReferencesInterFile(string resourceKey)
	{
		var container = CreateThemedContainer();
		var fontFamily = GetFontFamily(container, resourceKey);

		// Source should be like "ms-appx:///Uno.Fonts.Inter/Fonts/Inter-*.ttf#Inter"
		StringAssert.Contains(
			fontFamily.Source,
			"Inter-",
			$"Font resource '{resourceKey}' should reference an Inter TTF file. " +
			$"Actual Source: '{fontFamily.Source}'");

		StringAssert.EndsWith(
			fontFamily.Source,
			"#Inter",
			$"Font resource '{resourceKey}' should specify the Inter face name. " +
			$"Actual Source: '{fontFamily.Source}'");
	}

	// ─────────────────────────────────────────────────────────────────────
	// Weight mapping: each weight slot should use the correct TTF file.
	// ─────────────────────────────────────────────────────────────────────

	[TestMethod]
	[RunsOnUIThread]
	[DataRow("SimpleRegularFontFamily", "Inter-Regular.ttf")]
	[DataRow("SimpleMediumFontFamily", "Inter-Medium.ttf")]
	[DataRow("SimpleSemiBoldFontFamily", "Inter-SemiBold.ttf")]
	[DataRow("SimpleBoldFontFamily", "Inter-Bold.ttf")]
	public void When_SimpleThemeLoaded_Then_WeightSpecificFontFamilyMapsToCorrectFile(
		string resourceKey, string expectedFileName)
	{
		var container = CreateThemedContainer();
		var fontFamily = GetFontFamily(container, resourceKey);

		StringAssert.Contains(
			fontFamily.Source,
			expectedFileName,
			$"'{resourceKey}' should map to '{expectedFileName}'. " +
			$"Actual Source: '{fontFamily.Source}'");
	}

	// ─────────────────────────────────────────────────────────────────────
	// Typography scale: type scale slots must reference bundled fonts.
	// ─────────────────────────────────────────────────────────────────────

	[TestMethod]
	[RunsOnUIThread]
	[DataRow("DisplayLargeFontFamily")]
	[DataRow("HeadlineMediumFontFamily")]
	[DataRow("TitleMediumFontFamily")]
	[DataRow("BodyLargeFontFamily")]
	[DataRow("LabelMediumFontFamily")]
	[DataRow("CaptionMediumFontFamily")]
	public void When_SimpleThemeLoaded_Then_TypographyScaleFontFamilyUsesAppxUri(string resourceKey)
	{
		var container = CreateThemedContainer();
		var fontFamily = GetFontFamily(container, resourceKey);

		StringAssert.StartsWith(
			fontFamily.Source,
			"ms-appx:///Uno.Fonts.Inter/",
			$"Typography resource '{resourceKey}' must use a ms-appx:///Uno.Fonts.Inter/ URI. " +
			$"Actual Source: '{fontFamily.Source}'");

		StringAssert.Contains(
			fontFamily.Source,
			"Inter-",
			$"Typography resource '{resourceKey}' should reference a bundled Inter font file. " +
			$"Actual Source: '{fontFamily.Source}'");
	}

	// ─────────────────────────────────────────────────────────────────────
	// Bold/SemiBold slots: display and headline slots must use the correct weight file.
	// ─────────────────────────────────────────────────────────────────────

	[TestMethod]
	[RunsOnUIThread]
	[DataRow("DisplayLargeFontFamily")]
	[DataRow("DisplayMediumFontFamily")]
	public void When_SimpleThemeLoaded_Then_BoldDisplaySlotsUseBoldFontFile(string resourceKey)
	{
		var container = CreateThemedContainer();
		var fontFamily = GetFontFamily(container, resourceKey);

		StringAssert.Contains(
			fontFamily.Source,
			"Inter-Bold.ttf",
			$"'{resourceKey}' is a Bold display slot and must reference 'Inter-Bold.ttf'. " +
			$"Actual Source: '{fontFamily.Source}'");
	}

	[TestMethod]
	[RunsOnUIThread]
	[DataRow("HeadlineMediumFontFamily")]
	[DataRow("TitleMediumFontFamily")]
	[DataRow("LabelLargeFontFamily")]
	public void When_SimpleThemeLoaded_Then_SemiBoldSlotUseSemiBoldFontFile(string resourceKey)
	{
		var container = CreateThemedContainer();
		var fontFamily = GetFontFamily(container, resourceKey);

		StringAssert.Contains(
			fontFamily.Source,
			"Inter-SemiBold.ttf",
			$"'{resourceKey}' is a SemiBold slot and must reference 'Inter-SemiBold.ttf'. " +
			$"Actual Source: '{fontFamily.Source}'");
	}
}
