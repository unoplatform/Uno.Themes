namespace Uno.Themes.Samples.Content;

[SamplePage(SampleCategory.None, "Runtime Tests", IconSymbol = Symbol.Repair,
	SupportedDesigns = new[] { Design.Material, Design.Cupertino, Design.Simple })]
public sealed partial class RuntimeTestRunner : Page
{
	public RuntimeTestRunner()
	{
		this.InitializeComponent();
	}
}
