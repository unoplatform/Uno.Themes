namespace Uno.Themes.Samples.Content.Extensions;

[SamplePage(SampleCategory.Helpers, "ControlExtensions", IconPath = Icons.Helpers.ControlExtensions)]
public sealed partial class ControlExtensionsSamplePage : Page
{
	public string Items => "abcdefg";
	
	public ControlExtensionsSamplePage()
	{
		this.InitializeComponent();
	}
}
