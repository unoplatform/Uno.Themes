using Uno.Themes.ColorGeneration.Hct;
using Windows.UI;

namespace Uno.Themes.Samples.Content.Styles;

[SamplePage(
	SampleCategory.Styles,
	"Seed Color",
	IconPath = Icons.Styles.Colors,
	Description = "Generate a full color palette from a single seed color using the HCT color space.",
	SupportedDesigns = new[] { Design.Material, Design.Simple, Design.Fluent })]
public sealed partial class SeedColorSamplePage : Page
{
	private static Color _lastSeed = Color.FromArgb(0xFF, 0x67, 0x50, 0xA4);
	private static SeedColorMode _lastSeedColorMode = SeedColorMode.Fidelity;

	// The theme element name for the XAML snippet, per active design.
	private static string ThemeTypeName => SamplePageLayout.ActiveDesign switch
	{
		Design.Simple => "SimpleTheme",
		Design.Fluent => "FluentTheme",
		Design.Cupertino => "CupertinoTheme",
		_ => "MaterialTheme",
	};

	public SeedColorSamplePage()
	{
		this.InitializeComponent();

		if (SamplePageLayout.ActiveDesign == Design.Fluent)
		{
			DesignNote.Text = "Under FluentTheme the seed also recolors the built-in Fluent controls: SystemAccentColor and its shades follow the seed, so accent buttons, checked check boxes, toggle switches and slider fills change too, and the text on the accent is picked for contrast.";
			DesignNote.Visibility = Visibility.Visible;
		}

		SeedColorPicker.Color = _lastSeed;
		SeedColorModeCombo.SelectedIndex = _lastSeedColorMode == SeedColorMode.Fidelity ? 0 : 1;
		ApplySeedColor(_lastSeed);
	}

	private void SeedColorPicker_ColorChanged(ColorPicker sender, ColorChangedEventArgs args)
	{
		ApplySeedColor(args.NewColor);
	}

	private void SeedColorModeCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
	{
		_lastSeedColorMode = SeedColorModeCombo.SelectedIndex == 1 ? SeedColorMode.TonalSpot : SeedColorMode.Fidelity;
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
		SeedColorModeDescription.Text = _lastSeedColorMode == SeedColorMode.Fidelity
			? "Fidelity (default): the palette keeps the seed's own saturation and the light Primary is the seed hex verbatim."
			: "Tonal spot: Material's standard vibrant recipe — a minimum saturation is enforced, so the exact seed color is not reproduced.";

		var modeAttribute = _lastSeedColorMode == SeedColorMode.Fidelity ? string.Empty : "\n                 SeedColorMode=\"TonalSpot\"";
		var theme = ThemeTypeName;
		XamlSnippet.Text = $"<{theme}>\n  <{theme}.Colors>\n    <ThemeColors PrimarySeed=\"#{seed.R:X2}{seed.G:X2}{seed.B:X2}\"{modeAttribute} />\n  </{theme}.Colors>\n</{theme}>";
	}

	private static int ColorToArgb(Color c) => (c.A << 24) | (c.R << 16) | (c.G << 8) | c.B;
}
