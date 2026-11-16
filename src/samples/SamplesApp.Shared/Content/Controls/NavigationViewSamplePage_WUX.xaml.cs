namespace Uno.Themes.Samples.Content.Controls;

// Not listed: the sample only uses Material v1 (WUX) styles, and the sample app loads v2 only. Uno rendered it
// unstyled; WinUI fails on the missing v1 keys. NavigationViewSamplePage_MUX covers the v2 NavigationView.
[SamplePage(
	SampleCategory.Controls,
	"Navigation View (WUX)",
	Description = "This control is used for application navigation from a menu.",
	DocumentationLink = "https://docs.microsoft.com/en-us/windows/uwp/design/controls-and-patterns/navigationview")]
public sealed partial class NavigationViewSamplePage_WUX : Page
{
	public NavigationViewSamplePage_WUX()
	{
		this.InitializeComponent();
	}
}
