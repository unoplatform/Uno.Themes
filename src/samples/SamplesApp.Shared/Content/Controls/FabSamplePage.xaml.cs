namespace Uno.Themes.Samples.Content.Controls;

[SamplePage(SampleCategory.Controls, "FAB", SourceSdk.UnoMaterial, Description = "Also known as Floating Action Button, the FAB is used for a screen's primary action.", DocumentationLink = "https://material.io/components/buttons-floating-action-button", SupportedDesigns = new[] { Design.Material, Design.Omarchy })]
public sealed partial class FabSamplePage : Page
{
	public FabSamplePage()
	{
		this.InitializeComponent();
	}
}
