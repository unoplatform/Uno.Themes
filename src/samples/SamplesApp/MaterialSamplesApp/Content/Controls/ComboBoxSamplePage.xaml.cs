using Uno.Themes.Samples.Entities.Data;

namespace Uno.Themes.Samples.Content.Controls;

[SamplePage(SampleCategory.Controls, "ComboBox", DataType = typeof(TestCollections))]
public sealed partial class ComboBoxSamplePage : Page
{
	public ComboBoxSamplePage()
	{
		this.InitializeComponent();
		this.DataContext = new TestCollections();
	}
}
