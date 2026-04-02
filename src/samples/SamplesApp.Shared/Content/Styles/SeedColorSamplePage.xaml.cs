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

	public SeedColorSamplePage()
	{
		this.InitializeComponent();
		SeedColorPicker.Color = _lastSeed;
		ApplySeedColor(_lastSeed);
	}

	private void SeedColorPicker_ColorChanged(ColorPicker sender, ColorChangedEventArgs args)
	{
		ApplySeedColor(args.NewColor);
	}

	private void ApplySeedColor(Color seed)
	{
		_lastSeed = seed;
		SemanticThemeHelper.PrimarySeed = seed;

		var hct = HctColor.FromArgb(ColorToArgb(seed));

		SeedSwatch.Background = new SolidColorBrush(seed);
		SeedHex.Text = $"#{seed.R:X2}{seed.G:X2}{seed.B:X2}";
		SeedHctText.Text = $"H:{hct.Hue:F0}  C:{hct.Chroma:F0}  T:{hct.Tone:F0}";

		XamlSnippet.Text = $"<MaterialTheme>\n  <MaterialTheme.Colors>\n    <ThemeColors PrimarySeed=\"#{seed.R:X2}{seed.G:X2}{seed.B:X2}\" />\n  </MaterialTheme.Colors>\n</MaterialTheme>";
	}

	private static int ColorToArgb(Color c) => (c.A << 24) | (c.R << 16) | (c.G << 8) | c.B;
}
