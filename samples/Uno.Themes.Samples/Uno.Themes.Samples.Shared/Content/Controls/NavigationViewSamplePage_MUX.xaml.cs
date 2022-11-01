using Uno.Themes.Samples.Entities;
using Windows.UI.Xaml.Controls;

namespace Uno.Themes.Samples.Content.Controls
{
	[SamplePage(
		SampleCategory.Controls,
		"Navigation View (MUX)",
		Description = "This control is used for application navigation from a menu.",
		DocumentationLink = "https://docs.microsoft.com/en-us/windows/uwp/design/controls-and-patterns/navigationview")]
	public sealed partial class NavigationViewSamplePage_MUX : Page
	{
		public NavigationViewSamplePage_MUX()
		{
			this.InitializeComponent();
		}
	}
}
