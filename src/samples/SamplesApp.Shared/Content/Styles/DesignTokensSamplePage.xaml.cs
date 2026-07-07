namespace Uno.Themes.Samples.Content.Styles;

[SamplePage(
	SampleCategory.Styles,
	"Design Tokens",
	Description = "Demonstrates shared design tokens (spacing, shape, density, typography) and how to override them via BaseTheme properties.",
	SortOrder = 20,
	SupportedDesigns = new[] { Design.Material, Design.Simple })]
public sealed partial class DesignTokensSamplePage : Page
{
	public DesignTokensSamplePage()
	{
		this.InitializeComponent();
	}
}
