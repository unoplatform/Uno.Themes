namespace Uno.Themes.Samples.Content.Styles;

[SamplePage(
	SampleCategory.Styles,
	"Semantic Styling",
	Description = "Demonstrates semantic style aliases and lightweight styling overrides for the Simple theme. Both semantic keys and Simple-prefixed keys can be overridden.",
	SortOrder = 10,
	SupportedDesigns = new[] { Design.Simple })]
public sealed partial class SemanticStylingSamplePage : Page
{
	public SemanticStylingSamplePage()
	{
		this.InitializeComponent();
	}
}
