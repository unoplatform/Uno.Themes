using Uno.Omarchy;

namespace Uno.Themes.Samples.Content;

[SamplePage(SampleCategory.Styles, "Palette", IconPath = Icons.Styles.Colors, Description = "The 22 stock Omarchy palettes. Pick one to re-theme the whole app at runtime, exactly like `omarchy theme set`.", SupportedDesigns = new[] { Design.Omarchy })]
public sealed partial class OmarchyPaletteSamplePage : Page
{
	public OmarchyPaletteSamplePage()
	{
		this.InitializeComponent();

		PaletteSelector.ItemsSource = OmarchyPalettes.All;
		PaletteSelector.SelectedItem = Theme?.Palette;
		UpdateMode();
	}

	private static OmarchyTheme? Theme => Application.Current.GetTheme() as OmarchyTheme;

	private void PaletteSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
	{
		if (Theme is { } theme && PaletteSelector.SelectedItem is OmarchyPalette palette && !ReferenceEquals(theme.Palette, palette))
		{
			theme.Palette = palette;
		}

		UpdateMode();
	}

	private void UpdateMode()
	{
		ModeText.Text = Theme?.Palette.IsLight == true ? "mode = \"light\"" : "mode = \"dark\"";
	}
}
