namespace Uno.Themes.Samples.Content.Controls;

[SamplePage(SampleCategory.Controls, "Progress Bar", Description = "This control is used to display progress.", DocumentationLink = "https://docs.microsoft.com/en-us/uwp/api/microsoft.ui.xaml.controls.progressbar", SupportedDesigns = new[] { Design.Material, Design.Cupertino })]
public sealed partial class ProgressBarSamplePage : Page
{
	public ProgressBarSamplePage()
	{
		this.InitializeComponent();
	}
}
