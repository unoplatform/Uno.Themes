namespace Uno.Themes.Samples.Content.Controls;

// Not listed: the sample only has Material v1 content (Uno.Material v2 has no InfoBar style), and the sample
// app loads v2 only. Uno rendered it unstyled; WinUI fails on the missing v1 keys. Relist with a v2 template.
[SamplePage(SampleCategory.Controls, "Info Bar", Description = "This control is an inline notification for essential app-wide messages.", DocumentationLink = "https://docs.microsoft.com/en-us/windows/winui/api/microsoft.ui.xaml.controls.infobar?view=winui-2.5")]
public sealed partial class InfoBarSamplePage : Page
{
	public InfoBarSamplePage()
	{
		this.InitializeComponent();
	}
}
