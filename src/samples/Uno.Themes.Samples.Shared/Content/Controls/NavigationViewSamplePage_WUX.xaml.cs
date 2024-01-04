using Uno.Themes.Samples.Entities;
#if IS_WINUI
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
#else
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
#endif
namespace Uno.Themes.Samples.Content.Controls
{
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
}
