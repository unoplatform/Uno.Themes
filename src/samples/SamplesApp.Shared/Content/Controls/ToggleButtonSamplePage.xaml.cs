namespace Uno.Themes.Samples.Content.Controls;

[SamplePage(SampleCategory.Controls, "ToggleButton", Description = "Represents a control that a user can select (check) or clear (uncheck).", DocumentationLink = "https://learn.microsoft.com/en-us/uwp/api/windows.ui.xaml.controls.primitives.togglebutton", SupportedDesigns = new[] { Design.Material })]
public sealed partial class ToggleButtonSamplePage : Page
{
	public ToggleButtonSamplePage()
	{
		this.InitializeComponent();
	}
}
