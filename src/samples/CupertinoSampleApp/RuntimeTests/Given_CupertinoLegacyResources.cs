using Microsoft.VisualStudio.TestTools.UnitTesting;
using Uno.Cupertino;
using Uno.UI.RuntimeTests;
using Windows.UI;

namespace Uno.Themes.Samples.RuntimeTests;

/// <summary>
/// The obsolete <c>CupertinoColors</c> / <c>CupertinoFonts</c> / <c>CupertinoResources</c> trio must keep
/// working as a thin shim over <see cref="CupertinoTheme"/>, declared in the documented order.
/// </summary>
[TestClass]
public class Given_CupertinoLegacyResources
{
	private const string ColorsOverride = "ms-appx:///RuntimeTests/Assets/LegacyColorsOverride.xaml";
	private const string FontsOverride = "ms-appx:///RuntimeTests/Assets/LegacyFontsOverride.xaml";

	private static readonly Color OverrideBlue = Color.FromArgb(0xFF, 0x12, 0x34, 0x56);

#pragma warning disable CS0618 // The obsolete trio is the subject under test.
	private static Grid CreateLegacyContainer(string? colorsOverride, string? fontsOverride, out CupertinoResources resources)
	{
		var colors = new CupertinoColors { OverrideSource = colorsOverride };
		var fonts = new CupertinoFonts { OverrideSource = fontsOverride };
		try
		{
			resources = new CupertinoResources();
		}
		finally
		{
			// The override URIs are recorded process-wide; never let them leak into the next test.
			colors.OverrideSource = null;
			fonts.OverrideSource = null;
		}

		var container = new Grid();
		container.Resources.MergedDictionaries.Add(colors);
		container.Resources.MergedDictionaries.Add(fonts);
		container.Resources.MergedDictionaries.Add(resources);
		return container;
	}

	[TestMethod]
	[RunsOnUIThread]
	public void When_TrioDeclared_Then_ResourcesIsTheTheme()
	{
		var container = CreateLegacyContainer(null, null, out var resources);

		Assert.IsInstanceOfType(resources, typeof(CupertinoTheme));
		Assert.IsTrue(container.Resources.TryGetValue("CupertinoButtonStyle", out var style) && style is Style);
		Assert.IsTrue(container.Resources.TryGetValue("PrimaryBrush", out var brush) && brush is SolidColorBrush, "the shim must bring the semantic brushes");
	}

	[TestMethod]
	[RunsOnUIThread]
	public void When_ColorsOverrideSource_Then_LegacyBrushFollows()
	{
		var container = CreateLegacyContainer(ColorsOverride, null, out _);

		Assert.IsTrue(container.Resources.TryGetValue("CupertinoBlueBrush", out var value));
		Assert.AreEqual(OverrideBlue, ((SolidColorBrush)value).Color);
	}

	[TestMethod]
	[RunsOnUIThread]
	public void When_FontsOverrideRedefinesCupertinoFontFamily_Then_TranslatedToTheRoot()
	{
		var container = CreateLegacyContainer(null, FontsOverride, out var resources);

		Assert.AreEqual("Legacy Override Family", resources.DefaultFontFamily?.Source);
		foreach (var key in new[] { "DefaultFontFamily", "CupertinoFontFamily", "BodyLargeFontFamily" })
		{
			Assert.IsTrue(container.Resources.TryGetValue(key, out var value), $"{key} missing");
			Assert.AreEqual("Legacy Override Family", ((FontFamily)value).Source, key);
		}
	}

	[TestMethod]
	[RunsOnUIThread]
	public void When_OverrideCleared_Then_NextTrioIsUnaffected()
	{
		CreateLegacyContainer(ColorsOverride, FontsOverride, out _);
		var container = CreateLegacyContainer(null, null, out var resources);

		Assert.IsNull(resources.DefaultFontFamily);
		Assert.IsTrue(container.Resources.TryGetValue("CupertinoBlueBrush", out var value));
		Assert.AreNotEqual(OverrideBlue, ((SolidColorBrush)value).Color);
	}

	// Review finding (contract, HIGH): consumers merge <CupertinoColors /> / <CupertinoFonts /> into their own
	// style dictionaries so {StaticResource Cupertino*Brush} resolves there. They must still carry the keys.
	[TestMethod]
	[RunsOnUIThread]
	public void When_RecordersUsedOnTheirOwn_Then_TheyStillCarryTheirKeys()
	{
		var colors = new CupertinoColors();
		var fonts = new CupertinoFonts();

		Assert.IsTrue(colors.TryGetValue("CupertinoBlueColor", out var color) && color is Color, "CupertinoBlueColor");
		Assert.IsTrue(colors.TryGetValue("CupertinoBlueBrush", out var brush) && brush is SolidColorBrush, "CupertinoBlueBrush");
		Assert.AreEqual((Color)color, ((SolidColorBrush)brush).Color, "the brush must be painted from the palette, not left at its parse-time value");
		Assert.IsTrue(fonts.TryGetValue("CupertinoFontFamily", out var family) && family is FontFamily, "CupertinoFontFamily");
	}

	// Review finding (contract + skeptic): a recorder declared WITHOUT an OverrideSource never fires the
	// property-changed callback, so a previously recorded URI used to survive into the next CupertinoResources.
	[TestMethod]
	[RunsOnUIThread]
	public void When_LaterRecorderHasNoOverrideSource_Then_EarlierUriIsNotInherited()
	{
		var first = new CupertinoColors { OverrideSource = ColorsOverride };
		try
		{
			_ = new CupertinoColors();
			var resources = new CupertinoResources();
			var container = new Grid();
			container.Resources.MergedDictionaries.Add(resources);

			Assert.IsTrue(container.Resources.TryGetValue("CupertinoBlueBrush", out var value));
			Assert.AreNotEqual(OverrideBlue, ((SolidColorBrush)value).Color);
		}
		finally
		{
			first.OverrideSource = null;
		}
	}

#pragma warning restore CS0618
}
