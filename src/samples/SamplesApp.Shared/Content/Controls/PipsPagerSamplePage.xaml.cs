namespace Uno.Themes.Samples.Content.Controls;

[SamplePage(SampleCategory.Controls, "PipsPager", Description = "Represents a control that enables navigation within linearly paginated content using a configurable collection of glyphs, each of which represents a single \"page\" within a limitless range.", DocumentationLink = "https://docs.microsoft.com/en-us/uwp/api/microsoft.ui.xaml.controls.pipspager", SupportedDesigns = new[] { Design.Material })]
public sealed partial class PipsPagerSamplePage : Page
{
	public PipsPagerSamplePage()
	{
		this.InitializeComponent();
	}
}
