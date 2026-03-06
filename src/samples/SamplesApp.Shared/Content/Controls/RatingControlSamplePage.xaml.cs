namespace Uno.Themes.Samples.Content.Controls;

[SamplePage(SampleCategory.Controls, "RatingControl", Description = "This control allows a user to enter a star rating.", DocumentationLink = "https://docs.microsoft.com/en-us/uwp/api/windows.ui.xaml.controls.ratingcontrol", SupportedDesigns = new[] { Design.Material })]
public sealed partial class RatingControlSamplePage : Page
{
	public RatingControlSamplePage()
	{
		this.InitializeComponent();
	}
}
