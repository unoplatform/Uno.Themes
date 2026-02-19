using Microsoft.UI.Xaml.Controls;

namespace Uno.Themes.Samples.Content.Controls;

[SamplePage(SampleCategory.Controls, "IconButton", IconSymbol = Symbol.TouchPointer, SupportedDesigns = new[] { Design.Simple })]
public sealed partial class IconButtonSamplePage : Page
{
	public IconButtonSamplePage()
	{
		this.InitializeComponent();
	}
}
