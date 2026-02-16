namespace Uno.Themes.Samples.Content.Controls;

[SamplePage(SampleCategory.Controls, "TextBlock", Description = "This control is used to display text.", DocumentationLink = "https://docs.microsoft.com/en-us/uwp/api/windows.ui.xaml.controls.textblock", SupportedDesigns = new[] { Design.Material, Design.Cupertino })]
public sealed partial class TextBlockSamplePage : Page
{
	public TextBlockSamplePage()
	{
		this.InitializeComponent();
	}
}
