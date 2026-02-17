namespace Uno.Themes.Samples.Content.Controls;

[SamplePage(SampleCategory.Controls, "TextBox", Description = "This control allows users to input a textual value.", DocumentationLink = "https://docs.microsoft.com/en-us/uwp/api/windows.ui.xaml.controls.textbox", SupportedDesigns = new[] { Design.Material, Design.Cupertino })]
public sealed partial class TextBoxSamplePage : Page
{
	public TextBoxSamplePage()
	{
		this.InitializeComponent();
	}
}
