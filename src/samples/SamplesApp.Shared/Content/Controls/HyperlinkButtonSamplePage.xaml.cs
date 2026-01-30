namespace Uno.Themes.Samples.Content.Controls;

[SamplePage(SampleCategory.Controls, "HyperlinkButton", Description = "Represents a button control that functions as a hyperlink.", DocumentationLink = "https://docs.microsoft.com/en-us/uwp/api/windows.ui.xaml.controls.hyperlinkbutton", SupportedDesigns = new[] { Design.Material, Design.Cupertino })]
public sealed partial class HyperlinkButtonSamplePage : Page
{
	public HyperlinkButtonSamplePage()
	{
		this.InitializeComponent();
	}
}
