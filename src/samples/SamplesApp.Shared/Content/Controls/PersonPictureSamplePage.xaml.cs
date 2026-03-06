using Microsoft.UI.Xaml.Controls;

namespace Uno.Themes.Samples.Content.Controls;

[SamplePage(SampleCategory.Controls, "PersonPicture", IconSymbol = Symbol.Contact, SupportedDesigns = new[] { Design.Simple })]
public sealed partial class PersonPictureSamplePage : Page
{
	public PersonPictureSamplePage()
	{
		this.InitializeComponent();
	}
}
