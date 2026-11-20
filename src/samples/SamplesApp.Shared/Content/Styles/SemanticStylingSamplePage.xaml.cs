namespace Uno.Themes.Samples.Content.Styles;

[SamplePage(
	SampleCategory.Styles,
	"Semantic Styling",
	Description = "Demonstrates semantic style aliases and lightweight styling overrides. Under Simple, both semantic keys and Simple-prefixed keys can be overridden; under Fluent, page-scoped overrides target the built-in Fluent per-control keys.",
	SortOrder = 10,
	SupportedDesigns = new[] { Design.Simple, Design.Fluent })]
public sealed partial class SemanticStylingSamplePage : Page
{
	public SemanticStylingSamplePage()
	{
		this.InitializeComponent();
	}
}
