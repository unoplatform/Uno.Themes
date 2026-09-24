using Microsoft.UI.Xaml.Controls;

namespace Uno.Themes.Samples.Content.Controls;

[SamplePage(SampleCategory.Controls, "MenuFlyout", IconPath = Icons.Controls.MenuFlyout, SupportedDesigns = new[] { Design.Cupertino, Design.Simple })]
public sealed partial class MenuFlyoutSamplePage : Page
{
	public MenuFlyoutSamplePage()
	{
		this.InitializeComponent();
	}
}
