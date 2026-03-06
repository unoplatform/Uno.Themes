using Microsoft.UI.Xaml.Controls;

namespace Uno.Themes.Samples.Content.Controls;

[SamplePage(SampleCategory.Controls, "ToolTip", IconSymbol = Symbol.Comment, SupportedDesigns = new[] { Design.Simple })]
public sealed partial class ToolTipSamplePage : Page
{
	public ToolTipSamplePage()
	{
		this.InitializeComponent();
	}
}
