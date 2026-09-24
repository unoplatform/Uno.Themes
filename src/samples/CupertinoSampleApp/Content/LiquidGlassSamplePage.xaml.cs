namespace Uno.Themes.Samples.Content;

[SamplePage(
	SampleCategory.Styles,
	"Liquid Glass",
	IconPath = Icons.Styles.LiquidGlass,
	Description = "GlassPanel: every material, a tint, and the Liquid and Solid rendering tiers over scrolling content.",
	SortOrder = 30,
	SupportedDesigns = new[] { Design.Cupertino })]
public sealed partial class LiquidGlassSamplePage : Page
{
	public LiquidGlassSamplePage()
	{
		this.InitializeComponent();
	}
}
