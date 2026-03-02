namespace Uno.Themes.Samples.Content.Controls;

[SamplePage(SampleCategory.Controls, "Expander", Description = "An expander lets users show or hide related content by expanding or collapsing a section.", DocumentationLink = "https://learn.microsoft.com/en-us/windows/apps/design/controls/expander", SupportedDesigns = new[] { Design.Simple })]
public sealed partial class ExpanderSamplePage : Page
{
	public ExpanderSamplePage()
	{
		this.InitializeComponent();
	}
}
