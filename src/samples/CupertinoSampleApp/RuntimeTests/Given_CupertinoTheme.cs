using Microsoft.VisualStudio.TestTools.UnitTesting;
using Uno.Cupertino;
using Uno.UI.RuntimeTests;
using Windows.UI;

namespace Uno.Themes.Samples.RuntimeTests;

/// <summary>
/// Verifies that <see cref="CupertinoTheme"/> sits on the shared semantic system: Apple's palette
/// mapped onto the shared colour roles, live semantic brushes, seed colours and generated tokens.
/// </summary>
[TestClass]
public class Given_CupertinoTheme
{
	private const string ColorPalettePath = "ms-appx:///Uno.Cupertino.WinUI/Styles/Application/ColorPalette.xaml";

	private static Grid CreateThemedContainer(CupertinoTheme theme)
	{
		var container = new Grid();
		container.Resources.MergedDictionaries.Add(theme);
		return container;
	}

	private static T GetResource<T>(Grid container, string key)
	{
		if (container.Resources.TryGetValue(key, out var value) && value is T typed)
		{
			return typed;
		}

		Assert.Fail($"Resource '{key}' not found or not of type {typeof(T).Name}");
		return default!;
	}

	private static Color Parse(string hex) =>
		Color.FromArgb(0xFF, Convert.ToByte(hex[1..3], 16), Convert.ToByte(hex[3..5], 16), Convert.ToByte(hex[5..7], 16));

	// ResourceDictionary.TryGetValue resolves ThemeDictionaries against the application theme, so the two
	// appearances are read from the palette's theme blocks directly (see specs/lessons.md).
	[TestMethod]
	[RunsOnUIThread]
	[DataRow("PrimaryColor", "#0088FF", "#0091FF")]
	[DataRow("OnPrimaryColor", "#FFFFFF", "#FFFFFF")]
	[DataRow("PrimaryContainerColor", "#D9EDFF", "#001D33")]
	[DataRow("SecondaryColor", "#8E8E93", "#8E8E93")]
	[DataRow("TertiaryColor", "#6155F5", "#6D7CFF")]
	[DataRow("ErrorColor", "#FF383C", "#FF4245")]
	[DataRow("BackgroundColor", "#F2F2F7", "#000000")]
	[DataRow("SurfaceColor", "#FFFFFF", "#1C1C1E")]
	[DataRow("OnSurfaceVariantColor", "#8A8A8E", "#8D8D93")]
	[DataRow("OutlineColor", "#C6C6C8", "#38383A")]
	public void When_PaletteLoaded_Then_SharedRolesCarryAppleValues(string key, string light, string dark)
	{
		var palette = new ResourceDictionary { Source = new Uri(ColorPalettePath) };

		Assert.IsTrue(palette.ThemeDictionaries.TryGetValue("Light", out var lightBlock), "Light block missing");
		Assert.IsTrue(palette.ThemeDictionaries.TryGetValue("Default", out var darkBlock), "Default block missing");

		Assert.AreEqual(Parse(light), (Color)((ResourceDictionary)lightBlock)[key], $"{key} (Light)");
		Assert.AreEqual(Parse(dark), (Color)((ResourceDictionary)darkBlock)[key], $"{key} (Dark)");
	}

	[TestMethod]
	[RunsOnUIThread]
	public void When_ThemeLoaded_Then_SemanticBrushesFollowApplePalette()
	{
		var container = CreateThemedContainer(new CupertinoTheme());

		var primary = GetResource<Color>(container, "PrimaryColor");
		var brush = GetResource<SolidColorBrush>(container, "PrimaryBrush");

		// Light or Dark depending on the host; either way it is Apple's blue, not the shared M3 purple.
		Assert.IsTrue(primary == Parse("#0088FF") || primary == Parse("#0091FF"), $"PrimaryColor was {primary}");
		Assert.AreEqual(primary, brush.Color, "PrimaryBrush must be rewritten from the Cupertino palette");
	}

	[TestMethod]
	[RunsOnUIThread]
	public void When_PrimarySeedSet_Then_PrimaryBrushFollowsSeed()
	{
		var theme = new CupertinoTheme();
		var container = CreateThemedContainer(theme);
		var brush = GetResource<SolidColorBrush>(container, "PrimaryBrush");
		var unseeded = brush.Color;

		theme.Colors = new ThemeColors { PrimarySeed = Parse("#2E7D32") };

		Assert.AreNotEqual(unseeded, brush.Color, "the same brush instance must repaint from the seed palette");
		Assert.AreEqual(GetResource<Color>(container, "PrimaryColor"), brush.Color);
	}

	[TestMethod]
	[RunsOnUIThread]
	public void When_ThemeLoaded_Then_DesignTokensGenerate()
	{
		var container = CreateThemedContainer(new CupertinoTheme { DefaultSpacing = 6, DefaultCornerRadius = 5 });

		Assert.AreEqual(12d, GetResource<double>(container, "Space200"));
		Assert.AreEqual(new CornerRadius(5), GetResource<CornerRadius>(container, "Radius100CornerRadius"));
	}

	[TestMethod]
	[RunsOnUIThread]
	[DataRow("CupertinoButtonStyle")]
	[DataRow("CupertinoTextBoxStyle")]
	[DataRow("CupertinoToggleSwitchStyle")]
	public void When_ThemeLoaded_Then_ExistingStyleKeysResolve(string key)
	{
		var container = CreateThemedContainer(new CupertinoTheme());

		Assert.IsNotNull(GetResource<Style>(container, key).TargetType);
	}
}
