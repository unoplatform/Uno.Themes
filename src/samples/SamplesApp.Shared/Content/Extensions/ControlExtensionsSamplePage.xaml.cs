namespace Uno.Themes.Samples.Shared.Content.Extensions;

[SamplePage(SampleCategory.Helpers, "ControlExtensions", IconPath = Icons.Helpers.ControlExtensions, SupportedDesigns = new[] { Design.Material })]
public sealed partial class ControlExtensionsSamplePage : Page
{
	public string Items => "abcdefg";
	
	public ControlExtensionsSamplePage()
	{
		this.InitializeComponent();
	}
}
