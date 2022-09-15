using Uno.Themes.Samples.Entities;
using Uno.Themes.Samples.Entities.Data;
using Windows.UI.Xaml.Controls;


namespace Uno.Themes.Samples.Content.Controls
{
	[SamplePage(SampleCategory.Controls, "ComboBox", Description = "This control used for selection is a drop-down list that shows its selection in a box.", DocumentationLink = "https://docs.microsoft.com/en-us/uwp/api/windows.ui.xaml.controls.combobox", DataType = typeof(TestCollections))]
	public sealed partial class ComboBoxSamplePage : Page
	{
		public ComboBoxSamplePage()
		{
			this.InitializeComponent();
		}
	}
}
