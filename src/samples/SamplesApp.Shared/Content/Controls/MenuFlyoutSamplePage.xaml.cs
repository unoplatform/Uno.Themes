using Microsoft.UI.Xaml.Controls;

namespace Uno.Themes.Samples.Content.Controls;

[SamplePage(SampleCategory.Controls, "MenuFlyout", IconSymbol = Symbol.List, SupportedDesigns = new[] { Design.Simple })]
public sealed partial class MenuFlyoutSamplePage : Page
{
	public MenuFlyoutSamplePage()
	{
		this.InitializeComponent();
	}
}
