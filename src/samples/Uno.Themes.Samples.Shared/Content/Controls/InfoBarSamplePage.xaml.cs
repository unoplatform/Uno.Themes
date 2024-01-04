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
	[SamplePage(SampleCategory.Controls, "Info Bar", Description = "This control is an inline notification for essential app-wide messages.", DocumentationLink = "https://docs.microsoft.com/en-us/windows/winui/api/microsoft.ui.xaml.controls.infobar?view=winui-2.5")]
	public sealed partial class InfoBarSamplePage : Page
	{
		public InfoBarSamplePage()
		{
			this.InitializeComponent();
		}
	}
}
