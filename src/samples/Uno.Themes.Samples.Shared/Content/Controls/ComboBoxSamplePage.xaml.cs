using Uno.Themes.Samples.Entities;
using Uno.Themes.Samples.Entities.Data;
#if IS_WINUI
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
#else
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
#endif


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
