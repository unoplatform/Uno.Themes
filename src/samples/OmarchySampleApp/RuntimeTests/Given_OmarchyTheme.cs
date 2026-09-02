using Microsoft.UI.Xaml.Markup;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Uno.Omarchy;
using Uno.UI.RuntimeTests;
using Windows.UI;

namespace Uno.Themes.Samples.RuntimeTests;

/// <summary>
/// Verifies the Omarchy palette model: the stock palettes, the Omarchy color/brush resources
/// generated from the active palette, the semantic role mapping, and runtime palette switching
/// (brush instances are mutated in place so rendered controls repaint).
///
/// Each test creates a local OmarchyTheme scoped to the test container so it never touches the
/// application-level theme.
/// </summary>
[TestClass]
public class Given_OmarchyTheme
{
	private const string XamlNamespaces =
		"xmlns='http://schemas.microsoft.com/winfx/2006/xaml/presentation' xmlns:x='http://schemas.microsoft.com/winfx/2006/xaml'";

	private static (Grid Container, OmarchyTheme Theme) CreateThemedContainer()
	{
		var theme = new OmarchyTheme();
		var container = new Grid();
		container.Resources.MergedDictionaries.Add(theme);
		return (container, theme);
	}

	private static Border LoadThemedBorder(string brushKey) =>
		(Border)XamlReader.Load($"<Border {XamlNamespaces} Background='{{ThemeResource {brushKey}}}' Width='10' Height='10' />");

	// ─────────────────────────────────────────────────────────────────────
	// Stock palettes
	// ─────────────────────────────────────────────────────────────────────

	[TestMethod]
	public void When_StockPalettes_Then_AllTwentyTwoShip_AndTokyoNightIsDefault()
	{
		// flutter_omarchy 0.3.0 ships 22 fallback themes (fallback.g.dart).
		Assert.AreEqual(22, OmarchyPalettes.All.Count);
		Assert.AreSame(OmarchyPalettes.TokyoNight, new OmarchyTheme().Palette);

		// Values pinned from fallback.g.dart (tokyoNight).
		var tokyoNight = OmarchyPalettes.TokyoNight;
		Assert.AreEqual(Color.FromArgb(0xFF, 0x1A, 0x1B, 0x26), tokyoNight.Background);
		Assert.AreEqual(Color.FromArgb(0xFF, 0xA9, 0xB1, 0xD6), tokyoNight.Foreground);
		Assert.AreEqual(Color.FromArgb(0xFF, 0x7A, 0xA2, 0xF7), tokyoNight.Accent);
		Assert.AreEqual(Color.FromArgb(0xFF, 0x24, 0x28, 0x3B), tokyoNight.Normal.Black);
		Assert.AreEqual(Color.FromArgb(0xFF, 0x41, 0x48, 0x68), tokyoNight.Bright.Black);
		Assert.AreEqual(Color.FromArgb(0xFF, 0xFF, 0x7A, 0x93), tokyoNight.Bright[OmarchyAnsiColor.Red]);
		Assert.IsFalse(tokyoNight.IsLight);
		Assert.IsTrue(OmarchyPalettes.CatppuccinLatte.IsLight);
	}

	[TestMethod]
	[DataRow("Tokyo Night")]
	[DataRow("tokyo-night")]
	[DataRow("TOKYONIGHT")]
	[DataRow("tokyo_night")]
	public void When_FromName_Then_MatchesIgnoringCaseSpacesAndDashes(string name)
	{
		Assert.AreSame(OmarchyPalettes.TokyoNight, OmarchyPalettes.FromName(name));
	}

	[TestMethod]
	[DataRow("")]
	[DataRow("   ")]
	[DataRow("not-a-palette")]
	public void When_FromName_IsUnknown_Then_ReturnsNull(string name)
	{
		Assert.IsNull(OmarchyPalettes.FromName(name));
	}

	// ─────────────────────────────────────────────────────────────────────
	// Generated resources: Omarchy*Color / Omarchy*Brush under Light and Dark
	// ─────────────────────────────────────────────────────────────────────

	[TestMethod]
	[RunsOnUIThread]
	[DataRow(ElementTheme.Light)]
	[DataRow(ElementTheme.Dark)]
	public async Task When_OmarchyBrushes_AreResolved_Then_TheyCarryThePalette(ElementTheme requestedTheme)
	{
		var (container, theme) = CreateThemedContainer();
		container.RequestedTheme = requestedTheme;
		var palette = theme.Palette;

		var background = LoadThemedBorder("OmarchyBackgroundBrush");
		var accent = LoadThemedBorder("OmarchyAccentBrush");
		var normalRed = LoadThemedBorder("OmarchyNormalRedBrush");
		var brightBlack = LoadThemedBorder("OmarchyBrightBlackBrush");
		var primary = LoadThemedBorder("PrimaryBrush");

		var panel = new StackPanel();
		panel.Children.Add(background);
		panel.Children.Add(accent);
		panel.Children.Add(normalRed);
		panel.Children.Add(brightBlack);
		panel.Children.Add(primary);
		container.Children.Add(panel);

		UnitTestsUIContentHelper.Content = container;
		await UnitTestsUIContentHelper.WaitForLoaded(primary);
		await UnitTestsUIContentHelper.WaitForIdle();

		Assert.AreEqual(palette.Background, ((SolidColorBrush)background.Background).Color);
		Assert.AreEqual(palette.Accent, ((SolidColorBrush)accent.Background).Color);
		Assert.AreEqual(palette.Normal.Red, ((SolidColorBrush)normalRed.Background).Color);
		Assert.AreEqual(palette.Muted, ((SolidColorBrush)brightBlack.Background).Color, "bright.black maps to the muted color");
		Assert.AreEqual(palette.Accent, ((SolidColorBrush)primary.Background).Color, "Primary is the accent");
	}

	[TestMethod]
	[RunsOnUIThread]
	public void When_OmarchyColors_AreResolved_Then_EveryKeyExists()
	{
		var (container, theme) = CreateThemedContainer();
		var palette = theme.Palette;

		Assert.AreEqual(palette.Background, container.Resources["OmarchyBackgroundColor"]);
		Assert.AreEqual(palette.Foreground, container.Resources["OmarchyForegroundColor"]);
		Assert.AreEqual(palette.Accent, container.Resources["OmarchyAccentColor"]);
		Assert.AreEqual(palette.Selection, container.Resources["OmarchySelectionColor"]);
		Assert.AreEqual(palette.Muted, container.Resources["OmarchyMutedColor"]);

		foreach (var name in new[] { "Black", "White", "Red", "Green", "Yellow", "Blue", "Magenta", "Cyan" })
		{
			var ansi = Enum.Parse<OmarchyAnsiColor>(name);
			Assert.AreEqual(palette.Normal[ansi], container.Resources[$"OmarchyNormal{name}Color"], $"OmarchyNormal{name}Color");
			Assert.AreEqual(palette.Bright[ansi], container.Resources[$"OmarchyBright{name}Color"], $"OmarchyBright{name}Color");
			Assert.IsInstanceOfType(container.Resources[$"OmarchyNormal{name}Brush"], typeof(SolidColorBrush), $"OmarchyNormal{name}Brush");
			Assert.IsInstanceOfType(container.Resources[$"OmarchyBright{name}Brush"], typeof(SolidColorBrush), $"OmarchyBright{name}Brush");
		}
	}

	// ─────────────────────────────────────────────────────────────────────
	// Semantic role mapping (specs/08-omarchy-theme/progress.md)
	// ─────────────────────────────────────────────────────────────────────

	[TestMethod]
	[RunsOnUIThread]
	public void When_SemanticRoles_AreResolved_Then_TheyMapFromThePalette()
	{
		var (container, theme) = CreateThemedContainer();
		var palette = theme.Palette;

		Assert.AreEqual(palette.Accent, container.Resources["PrimaryColor"]);
		Assert.AreEqual(palette.Background, container.Resources["OnPrimaryColor"]);
		Assert.AreEqual(palette.Normal.Magenta, container.Resources["SecondaryColor"]);
		Assert.AreEqual(palette.Normal.Cyan, container.Resources["TertiaryColor"]);
		Assert.AreEqual(palette.Normal.Red, container.Resources["ErrorColor"]);
		Assert.AreEqual(palette.Bright.Red, container.Resources["OnErrorContainerColor"]);
		Assert.AreEqual(palette.Background, container.Resources["SurfaceColor"]);
		Assert.AreEqual(palette.Foreground, container.Resources["OnSurfaceColor"]);
		Assert.AreEqual(palette.Normal.Black, container.Resources["SurfaceVariantColor"], "lighter_background is the secondary surface");
		Assert.AreEqual(palette.Muted, container.Resources["OnSurfaceVariantColor"]);
		Assert.AreEqual(palette.Normal.White, container.Resources["OutlineColor"]);
		Assert.AreEqual(palette.Normal.Black, container.Resources["OutlineVariantColor"]);

		// PrimaryContainer is the accent composited at the 0.15 filled-tint alpha over the
		// background: #7AA2F7 @ 0.15 over #1A1B26 = #282F45 for Tokyo Night.
		Assert.AreEqual(Color.FromArgb(0xFF, 0x28, 0x2F, 0x45), container.Resources["PrimaryContainerColor"]);
		Assert.AreEqual(palette.Accent, container.Resources["OnPrimaryContainerColor"]);
	}

	// ─────────────────────────────────────────────────────────────────────
	// Runtime palette switching mutates the brush instances consumers hold
	// ─────────────────────────────────────────────────────────────────────

	[TestMethod]
	[RunsOnUIThread]
	public async Task When_PaletteChanges_Then_RenderedBrushesRepaintInPlace()
	{
		var (container, theme) = CreateThemedContainer();

		var background = LoadThemedBorder("OmarchyBackgroundBrush");
		var primary = LoadThemedBorder("PrimaryBrush");
		var panel = new StackPanel();
		panel.Children.Add(background);
		panel.Children.Add(primary);
		container.Children.Add(panel);

		UnitTestsUIContentHelper.Content = container;
		await UnitTestsUIContentHelper.WaitForLoaded(primary);
		await UnitTestsUIContentHelper.WaitForIdle();

		var backgroundBrush = (SolidColorBrush)background.Background;
		var primaryBrush = (SolidColorBrush)primary.Background;
		Assert.AreEqual(OmarchyPalettes.TokyoNight.Background, backgroundBrush.Color);

		theme.Palette = OmarchyPalettes.Nord;
		await UnitTestsUIContentHelper.WaitForIdle();

		// Same instances, new colors: nothing was re-resolved, the brushes were rewritten.
		Assert.AreSame(backgroundBrush, background.Background);
		Assert.AreSame(primaryBrush, primary.Background);
		Assert.AreEqual(OmarchyPalettes.Nord.Background, backgroundBrush.Color);
		Assert.AreEqual(OmarchyPalettes.Nord.Accent, primaryBrush.Color);
		Assert.AreEqual(OmarchyPalettes.Nord.Normal.Red, container.Resources["OmarchyNormalRedColor"]);
	}

	[TestMethod]
	[RunsOnUIThread]
	public void When_PaletteIsSetToNull_Then_FallsBackToTokyoNight()
	{
		var (container, theme) = CreateThemedContainer();
		theme.Palette = OmarchyPalettes.Gruvbox;
		Assert.AreEqual(OmarchyPalettes.Gruvbox.Background, container.Resources["OmarchyBackgroundColor"]);

		theme.Palette = null!;

		Assert.AreSame(OmarchyPalettes.TokyoNight, theme.Palette);
		Assert.AreEqual(OmarchyPalettes.TokyoNight.Background, container.Resources["OmarchyBackgroundColor"]);
	}

	// ─────────────────────────────────────────────────────────────────────
	// Consumer overrides keep the last word
	// ─────────────────────────────────────────────────────────────────────

	[TestMethod]
	[RunsOnUIThread]
	public void When_ColorOverride_IsSupplied_Then_ItWinsOverThePalette()
	{
		var overrideColor = Color.FromArgb(0xFF, 0x12, 0x34, 0x56);
		var colorOverride = new ResourceDictionary { ["PrimaryColor"] = overrideColor };

		var theme = new OmarchyTheme(colorOverride: colorOverride);
		var container = new Grid();
		container.Resources.MergedDictionaries.Add(theme);

		Assert.AreEqual(overrideColor, container.Resources["PrimaryColor"]);
		Assert.AreEqual(overrideColor, ((SolidColorBrush)container.Resources["PrimaryBrush"]).Color);
		// The Omarchy-specific resources are untouched by a semantic override.
		Assert.AreEqual(OmarchyPalettes.TokyoNight.Accent, container.Resources["OmarchyAccentColor"]);
	}
}
