using Microsoft.UI.Xaml.Controls;
using Uno.Themes.Samples.Entities;
using Windows.UI.Xaml.Controls;

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
