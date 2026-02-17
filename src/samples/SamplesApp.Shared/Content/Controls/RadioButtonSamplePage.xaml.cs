namespace Uno.Themes.Samples.Content.Controls;

[SamplePage(SampleCategory.Controls, "RadioButton", Description = "This button allows user to select a single option from a group of options.", DocumentationLink = "https://docs.microsoft.com/en-us/uwp/api/windows.ui.xaml.controls.radiobutton", SupportedDesigns = new[] { Design.Material, Design.Cupertino })]
public sealed partial class RadioButtonSamplePage : Page
{
	public RadioButtonSamplePage()
	{
		this.InitializeComponent();
	}
}
