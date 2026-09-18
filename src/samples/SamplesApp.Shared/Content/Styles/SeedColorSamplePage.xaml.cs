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
		ApplySeedToTheme(seed);

		var hct = HctColor.FromArgb(ColorToArgb(seed));

		SeedSwatch.Background = new SolidColorBrush(seed);
		SeedHex.Text = $"#{seed.R:X2}{seed.G:X2}{seed.B:X2}";
		SeedHctText.Text = $"H:{hct.Hue:F0}  C:{hct.Chroma:F0}  T:{hct.Tone:F0}";
		SeedColorModeDescription.Text = _lastSeedColorMode == SeedColorMode.Fidelity
			? "Fidelity (default): the palette keeps the seed's own saturation and the light Primary is the seed hex verbatim."
			: "Tonal spot: Material's standard vibrant recipe — a minimum saturation is enforced, so the exact seed color is not reproduced.";

		var modeAttribute = _lastSeedColorMode == SeedColorMode.Fidelity ? string.Empty : "\n                 SeedColorMode=\"TonalSpot\"";
		XamlSnippet.Text = $"<MaterialTheme>\n  <MaterialTheme.Colors>\n    <ThemeColors PrimarySeed=\"#{seed.R:X2}{seed.G:X2}{seed.B:X2}\"{modeAttribute} />\n  </MaterialTheme.Colors>\n</MaterialTheme>";
	}

	/// <summary>
	/// Pushes the seed and generation mode onto this application's theme.
	/// </summary>
	/// <remarks>
	/// Resolved through <see cref="NavigationHelper.CurrentApplication"/> rather than the static
	/// <c>SemanticThemeHelper</c>, which reads <c>Application.Current</c>: that is the *host's*
	/// application when this sample is hosted in a secondary ALC (ThemesSampleApp), and the host is
	/// deliberately theme-free, so the static path threw and the page could not even be constructed.
	/// This is the instance-based access documented in doc/seed-colors.md.
	/// Missing theme degrades to a no-op so the rest of the page still renders.
	/// </remarks>
	private static void ApplySeedToTheme(Color seed)
	{
		var application = NavigationHelper.CurrentApplication ?? Application.Current;
		if (application.GetTheme() is not { } theme)
		{
			return;
		}

		// Matches the lazy creation the static helper performed.
		theme.Colors ??= new ThemeColors();
		theme.Colors.SeedColorMode = _lastSeedColorMode;
		theme.Colors.PrimarySeed = seed;
	}

	private static int ColorToArgb(Color c) => (c.A << 24) | (c.R << 16) | (c.G << 8) | c.B;
}
