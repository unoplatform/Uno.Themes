using Microsoft.UI.Xaml.Controls;

namespace Uno.Themes.Samples.Content.Controls;

[SamplePage(SampleCategory.Controls, "PersonPicture", IconPath = Icons.Controls.PersonPicture, SupportedDesigns = new[] { Design.Simple })]
public sealed partial class PersonPictureSamplePage : Page
{
	public PersonPictureSamplePage()
	{
		this.InitializeComponent();
	}
}
