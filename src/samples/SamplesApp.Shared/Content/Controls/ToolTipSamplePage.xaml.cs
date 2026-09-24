using Microsoft.UI.Xaml.Controls;

namespace Uno.Themes.Samples.Content.Controls;

[SamplePage(SampleCategory.Controls, "ToolTip", IconPath = Icons.Controls.ToolTip, SupportedDesigns = new[] { Design.Cupertino, Design.Simple })]
public sealed partial class ToolTipSamplePage : Page
{
	public ToolTipSamplePage()
	{
		this.InitializeComponent();
	}
}
