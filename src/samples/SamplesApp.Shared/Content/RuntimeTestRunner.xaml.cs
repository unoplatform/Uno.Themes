namespace Uno.Themes.Samples.Content;

[SamplePage(SampleCategory.None, "Runtime Tests", IconPath = Icons.Shared.RuntimeTests, SupportedDesigns = new[] { Design.Material, Design.Cupertino, Design.Simple })]
public sealed partial class RuntimeTestRunner : Page
{
	public RuntimeTestRunner()
	{
		this.InitializeComponent();
	}
}
