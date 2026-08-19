using Uno.Themes.ColorGeneration.Hct;
using Windows.UI;

namespace Uno.Themes.Samples.Content.Styles;

[SamplePage(
	SampleCategory.Styles,
	"Seed Color",
	IconPath = Icons.Styles.Colors,
	Description = "Generate a full color palette from a single seed color using the HCT color space.",
	SupportedDesigns = new[] { Design.Material, Design.Simple })]
public sealed partial class SeedColorSamplePage : Page
{
	private static Color _lastSeed = Color.FromArgb(0xFF, 0x67, 0x50, 0xA4);
	private static SeedColorMode _lastSeedColorMode = SeedColorMode.Fidelity;

	public SeedColorSamplePage()
	{
		this.InitializeComponent();
		SeedColorPicker.Color = _lastSeed;
		SeedColorModeToggle.IsOn = _lastSeedColorMode == SeedColorMode.Fidelity;
		ApplySeedColor(_lastSeed);

		// On theme switch, pulse the seed to force a full theme rebuild so the
		// now-active theme gets the correct seed-derived colors.
		this.ActualThemeChanged += (s, e) =>
		{
			SemanticThemeHelper.PrimarySeed = null;
			SemanticThemeHelper.PrimarySeed = _lastSeed;
		};
	}

	private void SeedColorPicker_ColorChanged(ColorPicker sender, ColorChangedEventArgs args)
	{
		ApplySeedColor(args.NewColor);
	}

	private void SeedColorModeToggle_Toggled(object sender, RoutedEventArgs e)
	{
		_lastSeedColorMode = SeedColorModeToggle.IsOn ? SeedColorMode.Fidelity : SeedColorMode.TonalSpot;
		ApplySeedColor(_lastSeed);
	}

	private void ApplySeedColor(Color seed)
	{
		_lastSeed = seed;
		SemanticThemeHelper.SeedColorMode = _lastSeedColorMode;
		SemanticThemeHelper.PrimarySeed = seed;

		var hct = HctColor.FromArgb(ColorToArgb(seed));

		SeedSwatch.Background = new SolidColorBrush(seed);
		SeedHex.Text = $"#{seed.R:X2}{seed.G:X2}{seed.B:X2}";
		SeedHctText.Text = $"H:{hct.Hue:F0}  C:{hct.Chroma:F0}  T:{hct.Tone:F0}";

		var modeAttribute = _lastSeedColorMode == SeedColorMode.Fidelity ? string.Empty : "\n                 SeedColorMode=\"TonalSpot\"";
		XamlSnippet.Text = $"<MaterialTheme>\n  <MaterialTheme.Colors>\n    <ThemeColors PrimarySeed=\"#{seed.R:X2}{seed.G:X2}{seed.B:X2}\"{modeAttribute} />\n  </MaterialTheme.Colors>\n</MaterialTheme>";
	}

	private static int ColorToArgb(Color c) => (c.A << 24) | (c.R << 16) | (c.G << 8) | c.B;
}
