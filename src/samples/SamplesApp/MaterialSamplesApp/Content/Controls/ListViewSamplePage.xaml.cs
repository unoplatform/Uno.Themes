using Uno.Themes.Samples.Entities.Data;

namespace Uno.Themes.Samples.Content.Controls;

[SamplePage(SampleCategory.Controls, "ListView")]
public sealed partial class ListViewSamplePage : Page
{
	public ListViewSamplePage()
	{
		this.InitializeComponent();
		this.DataContext = new TestCollections();
	}
}
