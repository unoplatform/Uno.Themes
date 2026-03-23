namespace Uno.Themes.Samples.Content.Controls;

[SamplePage(
	SampleCategory.Controls,
	"New Simple Styles (Test)",
	IconSymbol = Symbol.PreviewLink,
	Description = "Temporary test page showcasing all newly added Simple theme styles.",
	SupportedDesigns = new[] { Design.Simple })]
public sealed partial class NewSimpleStylesTestPage : Page
{
	public NewSimpleStylesTestPage()
	{
		this.InitializeComponent();
	}
}
