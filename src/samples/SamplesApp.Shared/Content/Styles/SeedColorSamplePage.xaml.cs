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
	// The seed picked on this page, if any. Until one is picked, a visit only previews: applying a default seed
	// on load repainted the whole app (FluentTheme loses the system accent; Material's baseline hid it).
	private static Color? _lastSeed;
	private static readonly Color DefaultPreviewSeed = Color.FromArgb(0xFF, 0x67, 0x50, 0xA4);
	private static SeedColorMode _lastSeedColorMode = SeedColorMode.Fidelity;

	// The theme element name for the XAML snippet, per active design.
	private static string ThemeTypeName => SamplePageLayout.ActiveDesign switch
	{
		Design.Simple => "SimpleTheme",
		Design.Fluent => "FluentTheme",
		Design.Cupertino => "CupertinoTheme",
		_ => "MaterialTheme",
	};

	// WinUI raises ColorChanged while InitializeComponent applies the XAML Color, before the named
	// elements ApplySeedColor writes to are connected. The constructor applies the seed itself.
	private readonly bool _isInitialized;

	public SeedColorSamplePage()
	{
		this.InitializeComponent();

		if (SamplePageLayout.ActiveDesign == Design.Fluent)
		{
			DesignNote.Text = "Under FluentTheme the seed also recolors the built-in Fluent controls: SystemAccentColor and its shades follow the seed, so accent buttons, checked check boxes, toggle switches and slider fills change too, and the text on the accent is picked for contrast.";
			DesignNote.Visibility = Visibility.Visible;
		}

		var shown = _lastSeed ?? CurrentPrimary() ?? DefaultPreviewSeed;
		SeedColorPicker.Color = shown;
		SeedColorModeCombo.SelectedIndex = _lastSeedColorMode == SeedColorMode.Fidelity ? 0 : 1;
		_isInitialized = true;

		if (_lastSeed is { } seed)
		{
			ApplySeedColor(seed);
		}
		else
		{
			ShowSeed(shown);
		}
	}

	private static Color? CurrentPrimary()
		=> (SampleThemeHelper.CurrentApplication ?? Application.Current).Resources.TryGetValue("PrimaryColor", out var value) && value is Color color ? color : (Color?)null;

	private void SeedColorPicker_ColorChanged(ColorPicker sender, ColorChangedEventArgs args)
	{
		if (_isInitialized)
		{
			ApplySeedColor(args.NewColor);
		}
	}

	private void SeedColorModeCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
	{
		if (!_isInitialized)
		{
			return;
		}

		_lastSeedColorMode = SeedColorModeCombo.SelectedIndex == 1 ? SeedColorMode.TonalSpot : SeedColorMode.Fidelity;
		ApplySeedColor(SeedColorPicker.Color);
	}

	private void ApplySeedColor(Color seed)
	{
		_lastSeed = seed;

		// Not SemanticThemeHelper: it resolves Application.Current, which is the wrapper app — not
		// this head — when the head is hosted in a secondary ALC by ThemesSampleApp.
		var colors = SampleThemeHelper.GetColorsOrThrow();
		colors.SeedColorMode = _lastSeedColorMode;
		colors.PrimarySeed = seed;
		ShowSeed(seed);
	}

	private void ShowSeed(Color seed)
	{
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
