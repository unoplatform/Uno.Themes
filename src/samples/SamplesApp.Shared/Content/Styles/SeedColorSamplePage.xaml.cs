using Uno.Themes.ColorGeneration;
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
	public SeedColorSamplePage()
	{
		this.InitializeComponent();
		ApplySeedColor(Color.FromArgb(0xFF, 0x67, 0x50, 0xA4));
	}

	private void SeedColorPicker_ColorChanged(ColorPicker sender, ColorChangedEventArgs args)
	{
		ApplySeedColor(args.NewColor);
	}

	private void ApplySeedColor(Color seed)
	{
		// Update the actual app theme so the entire app re-themes
		SemanticThemeHelper.PrimarySeed = seed;

		var hct = HctColor.FromArgb(ColorToArgb(seed));

		// Generate palettes
		var primary = new TonalPalette(hct.Hue, Math.Max(hct.Chroma, 48));
		var secondary = new TonalPalette(hct.Hue, hct.Chroma / 3.0);
		var tertiary = new TonalPalette(hct.Hue + 60.0, hct.Chroma / 2.0);
		var neutral = new TonalPalette(hct.Hue, Math.Min(hct.Chroma / 12.0, 4.0));
		var neutralVariant = new TonalPalette(hct.Hue, Math.Min(hct.Chroma / 6.0, 8.0));
		var error = new TonalPalette(25.0, 84.0);

		// Seed display
		SeedSwatch.Background = new SolidColorBrush(seed);
		SeedHex.Text = ColorToHex(seed);
		SeedHctText.Text = $"H:{hct.Hue:F0}  C:{hct.Chroma:F0}  T:{hct.Tone:F0}";

		// Primary row
		SetSwatch(PrimaryP0, primary, 0);
		SetSwatch(PrimaryP10, primary, 10);
		SetSwatch(PrimaryP20, primary, 20);
		SetSwatch(PrimaryP30, primary, 30);
		SetSwatch(PrimaryP40, primary, 40);
		SetSwatch(PrimaryP50, primary, 50);
		SetSwatch(PrimaryP60, primary, 60);
		SetSwatch(PrimaryP70, primary, 70);
		SetSwatch(PrimaryP80, primary, 80);
		SetSwatch(PrimaryP90, primary, 90);
		SetSwatch(PrimaryP95, primary, 95);
		SetSwatch(PrimaryP99, primary, 99);
		SetSwatch(PrimaryP100, primary, 100);

		// Secondary row
		SetSwatch(SecondaryP0, secondary, 0);
		SetSwatch(SecondaryP10, secondary, 10);
		SetSwatch(SecondaryP20, secondary, 20);
		SetSwatch(SecondaryP30, secondary, 30);
		SetSwatch(SecondaryP40, secondary, 40);
		SetSwatch(SecondaryP50, secondary, 50);
		SetSwatch(SecondaryP60, secondary, 60);
		SetSwatch(SecondaryP70, secondary, 70);
		SetSwatch(SecondaryP80, secondary, 80);
		SetSwatch(SecondaryP90, secondary, 90);
		SetSwatch(SecondaryP95, secondary, 95);
		SetSwatch(SecondaryP99, secondary, 99);
		SetSwatch(SecondaryP100, secondary, 100);

		// Tertiary row
		SetSwatch(TertiaryP0, tertiary, 0);
		SetSwatch(TertiaryP10, tertiary, 10);
		SetSwatch(TertiaryP20, tertiary, 20);
		SetSwatch(TertiaryP30, tertiary, 30);
		SetSwatch(TertiaryP40, tertiary, 40);
		SetSwatch(TertiaryP50, tertiary, 50);
		SetSwatch(TertiaryP60, tertiary, 60);
		SetSwatch(TertiaryP70, tertiary, 70);
		SetSwatch(TertiaryP80, tertiary, 80);
		SetSwatch(TertiaryP90, tertiary, 90);
		SetSwatch(TertiaryP95, tertiary, 95);
		SetSwatch(TertiaryP99, tertiary, 99);
		SetSwatch(TertiaryP100, tertiary, 100);

		// Neutral row
		SetSwatch(NeutralP0, neutral, 0);
		SetSwatch(NeutralP10, neutral, 10);
		SetSwatch(NeutralP20, neutral, 20);
		SetSwatch(NeutralP30, neutral, 30);
		SetSwatch(NeutralP40, neutral, 40);
		SetSwatch(NeutralP50, neutral, 50);
		SetSwatch(NeutralP60, neutral, 60);
		SetSwatch(NeutralP70, neutral, 70);
		SetSwatch(NeutralP80, neutral, 80);
		SetSwatch(NeutralP90, neutral, 90);
		SetSwatch(NeutralP95, neutral, 95);
		SetSwatch(NeutralP99, neutral, 99);
		SetSwatch(NeutralP100, neutral, 100);

		// Neutral Variant row
		SetSwatch(NeutralVarP0, neutralVariant, 0);
		SetSwatch(NeutralVarP10, neutralVariant, 10);
		SetSwatch(NeutralVarP20, neutralVariant, 20);
		SetSwatch(NeutralVarP30, neutralVariant, 30);
		SetSwatch(NeutralVarP40, neutralVariant, 40);
		SetSwatch(NeutralVarP50, neutralVariant, 50);
		SetSwatch(NeutralVarP60, neutralVariant, 60);
		SetSwatch(NeutralVarP70, neutralVariant, 70);
		SetSwatch(NeutralVarP80, neutralVariant, 80);
		SetSwatch(NeutralVarP90, neutralVariant, 90);
		SetSwatch(NeutralVarP95, neutralVariant, 95);
		SetSwatch(NeutralVarP99, neutralVariant, 99);
		SetSwatch(NeutralVarP100, neutralVariant, 100);

		// Error row
		SetSwatch(ErrorP0, error, 0);
		SetSwatch(ErrorP10, error, 10);
		SetSwatch(ErrorP20, error, 20);
		SetSwatch(ErrorP30, error, 30);
		SetSwatch(ErrorP40, error, 40);
		SetSwatch(ErrorP50, error, 50);
		SetSwatch(ErrorP60, error, 60);
		SetSwatch(ErrorP70, error, 70);
		SetSwatch(ErrorP80, error, 80);
		SetSwatch(ErrorP90, error, 90);
		SetSwatch(ErrorP95, error, 95);
		SetSwatch(ErrorP99, error, 99);
		SetSwatch(ErrorP100, error, 100);

		// Semantic roles - Light
		SetSemanticSwatch(LightPrimary, primary, 40);
		SetSemanticSwatch(LightOnPrimary, primary, 100);
		SetSemanticSwatch(LightPrimaryContainer, primary, 90);
		SetSemanticSwatch(LightOnPrimaryContainer, primary, 10);
		SetSemanticSwatch(LightSecondary, secondary, 40);
		SetSemanticSwatch(LightOnSecondary, secondary, 100);
		SetSemanticSwatch(LightSecondaryContainer, secondary, 90);
		SetSemanticSwatch(LightOnSecondaryContainer, secondary, 10);
		SetSemanticSwatch(LightTertiary, tertiary, 40);
		SetSemanticSwatch(LightOnTertiary, tertiary, 100);
		SetSemanticSwatch(LightTertiaryContainer, tertiary, 90);
		SetSemanticSwatch(LightOnTertiaryContainer, tertiary, 10);
		SetSemanticSwatch(LightError, error, 40);
		SetSemanticSwatch(LightOnError, error, 100);
		SetSemanticSwatch(LightErrorContainer, error, 90);
		SetSemanticSwatch(LightOnErrorContainer, error, 10);
		SetSemanticSwatch(LightBackground, neutral, 99);
		SetSemanticSwatch(LightOnBackground, neutral, 10);
		SetSemanticSwatch(LightSurface, neutral, 99);
		SetSemanticSwatch(LightOnSurface, neutral, 10);
		SetSemanticSwatch(LightOutline, neutralVariant, 50);
		SetSemanticSwatch(LightOutlineVariant, neutralVariant, 80);

		// Semantic roles - Dark
		SetSemanticSwatch(DarkPrimary, primary, 80);
		SetSemanticSwatch(DarkOnPrimary, primary, 20);
		SetSemanticSwatch(DarkPrimaryContainer, primary, 30);
		SetSemanticSwatch(DarkOnPrimaryContainer, primary, 90);
		SetSemanticSwatch(DarkSecondary, secondary, 80);
		SetSemanticSwatch(DarkOnSecondary, secondary, 20);
		SetSemanticSwatch(DarkSecondaryContainer, secondary, 30);
		SetSemanticSwatch(DarkOnSecondaryContainer, secondary, 90);
		SetSemanticSwatch(DarkTertiary, tertiary, 80);
		SetSemanticSwatch(DarkOnTertiary, tertiary, 20);
		SetSemanticSwatch(DarkTertiaryContainer, tertiary, 30);
		SetSemanticSwatch(DarkOnTertiaryContainer, tertiary, 90);
		SetSemanticSwatch(DarkError, error, 80);
		SetSemanticSwatch(DarkOnError, error, 20);
		SetSemanticSwatch(DarkErrorContainer, error, 30);
		SetSemanticSwatch(DarkOnErrorContainer, error, 90);
		SetSemanticSwatch(DarkBackground, neutral, 10);
		SetSemanticSwatch(DarkOnBackground, neutral, 90);
		SetSemanticSwatch(DarkSurface, neutral, 20);
		SetSemanticSwatch(DarkOnSurface, neutral, 90);
		SetSemanticSwatch(DarkOutline, neutralVariant, 60);
		SetSemanticSwatch(DarkOutlineVariant, neutralVariant, 30);

		// XAML snippet
		XamlSnippet.Text = $"<MaterialTheme>\n  <MaterialTheme.Colors>\n    <ThemeColors PrimarySeed=\"{ColorToHex(seed)}\" />\n  </MaterialTheme.Colors>\n</MaterialTheme>";
	}

	private void SetSwatch(Border border, TonalPalette palette, int tone)
	{
		var color = ArgbToColor(palette.GetArgb(tone));
		border.Background = new SolidColorBrush(color);

		if (border.Child is TextBlock tb)
		{
			tb.Foreground = new SolidColorBrush(tone > 50 ? Colors.Black : Colors.White);
		}
	}

	private void SetSemanticSwatch(Border border, TonalPalette palette, int tone)
	{
		var color = ArgbToColor(palette.GetArgb(tone));
		border.Background = new SolidColorBrush(color);

		if (border.Child is TextBlock tb)
		{
			tb.Foreground = new SolidColorBrush(tone > 60 ? Colors.Black : Colors.White);
		}
	}

	private static string ColorToHex(Color c) => $"#{c.R:X2}{c.G:X2}{c.B:X2}";

	private static int ColorToArgb(Color c) => (c.A << 24) | (c.R << 16) | (c.G << 8) | c.B;

	private static Color ArgbToColor(int argb) => Color.FromArgb(
		(byte)((argb >> 24) & 0xFF),
		(byte)((argb >> 16) & 0xFF),
		(byte)((argb >> 8) & 0xFF),
		(byte)(argb & 0xFF));
}
