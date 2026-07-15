using Microsoft.VisualStudio.TestTools.UnitTesting;
using Uno.Fluent;
using Uno.UI.RuntimeTests;

namespace Uno.Themes.Samples.RuntimeTests;

/// <summary>
/// Verifies the FluentTheme typography layer (specs/05-fluent-theme, §7):
/// the 19 semantic slots carry the Fluent type-ramp values (hybrid rule D7),
/// every slot uses the platform-default font (ContentControlThemeFontFamily,
/// D11), and the semantic TextBlock styles apply those values to real controls.
/// </summary>
[TestClass]
public class Given_FluentTypography
{
	private static Grid CreateThemedContainer()
	{
		var container = new Grid();
		container.Resources.MergedDictionaries.Add(new FluentTheme());
		return container;
	}

	private static FontFamily GetPlatformDefaultFontFamily()
	{
		Assert.IsTrue(
			Application.Current.Resources.TryGetValue("ContentControlThemeFontFamily", out var value)
				&& value is FontFamily,
			"ContentControlThemeFontFamily should be provided by XamlControlsResources");
		return (FontFamily)value;
	}

	// ─────────────────────────────────────────────────────────────────────
	// Slot values (spec §7.2): size + weight per the Fluent ramp hybrid rule.
	// ─────────────────────────────────────────────────────────────────────

	[TestMethod]
	[RunsOnUIThread]
	[DataRow("DisplayLarge", 68.0, "SemiBold")]
	[DataRow("DisplayMedium", 54.0, "SemiBold")]
	[DataRow("DisplaySmall", 40.0, "SemiBold")]
	[DataRow("HeadlineLarge", 32.0, "SemiBold")]
	[DataRow("HeadlineMedium", 28.0, "SemiBold")]
	[DataRow("HeadlineSmall", 24.0, "SemiBold")]
	[DataRow("TitleLarge", 20.0, "SemiBold")]
	[DataRow("TitleMedium", 16.0, "SemiBold")]
	[DataRow("TitleSmall", 14.0, "SemiBold")]
	[DataRow("BodyLarge", 18.0, "Normal")]
	[DataRow("BodyMedium", 14.0, "Normal")]
	[DataRow("BodySmall", 12.0, "Normal")]
	[DataRow("LabelLarge", 14.0, "SemiBold")]
	[DataRow("LabelMedium", 12.0, "SemiBold")]
	[DataRow("LabelSmall", 11.0, "SemiBold")]
	[DataRow("LabelExtraSmall", 11.0, "Normal")]
	[DataRow("CaptionLarge", 13.0, "Normal")]
	[DataRow("CaptionMedium", 12.0, "Normal")]
	[DataRow("CaptionSmall", 11.0, "Normal")]
	public void When_Slot_CarriesFluentRampValues(string slot, double expectedSize, string expectedWeight)
	{
		var container = CreateThemedContainer();

		Assert.IsTrue(
			container.Resources.TryGetValue($"{slot}FontSize", out var size) && size is double,
			$"{slot}FontSize should resolve under FluentTheme");
		Assert.AreEqual(expectedSize, (double)size, $"{slot}FontSize");

		Assert.IsTrue(
			container.Resources.TryGetValue($"{slot}FontWeight", out var weight),
			$"{slot}FontWeight should resolve under FluentTheme");
		Assert.AreEqual(expectedWeight, weight as string, $"{slot}FontWeight");

		Assert.IsTrue(
			container.Resources.TryGetValue($"{slot}FontFamily", out var family),
			$"{slot}FontFamily should resolve under FluentTheme");
		var fontFamily = family as FontFamily;
		Assert.IsNotNull(fontFamily, $"{slot}FontFamily should be a FontFamily");
		Assert.AreEqual(GetPlatformDefaultFontFamily().Source, fontFamily.Source,
			$"{slot}FontFamily must be the platform default (ContentControlThemeFontFamily, D11)");
	}

	// ─────────────────────────────────────────────────────────────────────
	// Fluent does not use letter-spacing: every *CharacterSpacing key the
	// shared layer defines is zeroed (spec §7.2).
	// ─────────────────────────────────────────────────────────────────────

	[TestMethod]
	[RunsOnUIThread]
	[DataRow("DisplayLarge")]
	[DataRow("BodyLarge")]
	[DataRow("BodyMedium")]
	[DataRow("BodySmall")]
	[DataRow("LabelLarge")]
	[DataRow("LabelMedium")]
	[DataRow("LabelSmall")]
	[DataRow("LabelExtraSmall")]
	[DataRow("CaptionLarge")]
	[DataRow("CaptionMedium")]
	[DataRow("CaptionSmall")]
	public void When_Slot_CharacterSpacingIsZero(string slot)
	{
		var container = CreateThemedContainer();

		Assert.IsTrue(
			container.Resources.TryGetValue($"{slot}CharacterSpacing", out var spacing) && spacing is int,
			$"{slot}CharacterSpacing should resolve under FluentTheme");
		Assert.AreEqual(0, (int)spacing, $"{slot}CharacterSpacing must be 0 under Fluent");
	}

	// ─────────────────────────────────────────────────────────────────────
	// Root typeface tokens (single-key font swap surface) follow D11 too.
	// ─────────────────────────────────────────────────────────────────────

	[TestMethod]
	[RunsOnUIThread]
	[DataRow("TypefacePlain")]
	[DataRow("TypefaceBrand")]
	public void When_RootTypeface_IsPlatformDefault(string key)
	{
		var container = CreateThemedContainer();

		Assert.IsTrue(
			container.Resources.TryGetValue(key, out var value),
			$"{key} should resolve under FluentTheme");
		var fontFamily = value as FontFamily;
		Assert.IsNotNull(fontFamily, $"{key} should be a FontFamily");
		Assert.AreEqual(GetPlatformDefaultFontFamily().Source, fontFamily.Source,
			$"{key} must be the platform default (ContentControlThemeFontFamily, D11)");
	}

	// ─────────────────────────────────────────────────────────────────────
	// TextBlock styles (spec §7.3): the semantic alias resolves to the Fluent-
	// prefixed style, and applying it carries the slot values to the control.
	// ─────────────────────────────────────────────────────────────────────

	[TestMethod]
	[RunsOnUIThread]
	[DataRow("DisplayLarge", "FluentDisplayLarge")]
	[DataRow("DisplayMedium", "FluentDisplayMedium")]
	[DataRow("DisplaySmall", "FluentDisplaySmall")]
	[DataRow("HeadlineLarge", "FluentHeadlineLarge")]
	[DataRow("HeadlineMedium", "FluentHeadlineMedium")]
	[DataRow("HeadlineSmall", "FluentHeadlineSmall")]
	[DataRow("TitleLarge", "FluentTitleLarge")]
	[DataRow("TitleMedium", "FluentTitleMedium")]
	[DataRow("TitleSmall", "FluentTitleSmall")]
	[DataRow("BodyLarge", "FluentBodyLarge")]
	[DataRow("BodyMedium", "FluentBodyMedium")]
	[DataRow("BodySmall", "FluentBodySmall")]
	[DataRow("LabelLarge", "FluentLabelLarge")]
	[DataRow("LabelMedium", "FluentLabelMedium")]
	[DataRow("LabelSmall", "FluentLabelSmall")]
	[DataRow("LabelExtraSmall", "FluentLabelExtraSmall")]
	[DataRow("CaptionLarge", "FluentCaptionLarge")]
	[DataRow("CaptionMedium", "FluentCaptionMedium")]
	[DataRow("CaptionSmall", "FluentCaptionSmall")]
	public void When_TypographyAlias_ResolvesToFluentStyle(string semanticKey, string fluentKey)
	{
		var container = CreateThemedContainer();

		var semantic = container.Resources[semanticKey] as Style;
		var fluent = container.Resources[fluentKey] as Style;
		Assert.IsNotNull(semantic, $"{semanticKey} should resolve");
		Assert.IsNotNull(fluent, $"{fluentKey} should resolve");
		Assert.AreSame(fluent, semantic, $"{semanticKey} should alias {fluentKey}");
		Assert.AreEqual(typeof(TextBlock), fluent.TargetType);
	}

	[TestMethod]
	[RunsOnUIThread]
	public async Task When_TextBlockStyleApplied_SlotValuesReachTheControl()
	{
		var container = CreateThemedContainer();

		var textBlock = new TextBlock
		{
			Text = "fluent",
			Style = (Style)container.Resources["DisplayLarge"],
		};
		container.Children.Add(textBlock);

		UnitTestsUIContentHelper.Content = container;
		await UnitTestsUIContentHelper.WaitForLoaded(textBlock);
		await UnitTestsUIContentHelper.WaitForIdle();

		Assert.AreEqual(68.0, textBlock.FontSize, "DisplayLarge must apply the Fluent Display size");
		// 600 == FontWeights.SemiBold (compared numerically to stay portable
		// across the Windows.UI.Text / Microsoft.UI.Text projections)
		Assert.AreEqual((ushort)600, textBlock.FontWeight.Weight,
			"DisplayLarge must apply the Fluent Display weight");
		Assert.AreEqual(0, textBlock.CharacterSpacing, "DisplayLarge must apply zero letter-spacing");
		Assert.AreEqual(GetPlatformDefaultFontFamily().Source, textBlock.FontFamily.Source,
			"DisplayLarge must apply the platform-default font family");
	}
}
