using Uno.Themes.Samples.Entities;
using Uno.Themes.Samples.ViewModels;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Uno.Themes.Samples.Content.Controls
{
	/// <summary>
	/// An empty page that can be used on its own or navigated to within a Frame.
	/// </summary>
	[SamplePage(SampleCategory.Controls, "Standard Bottom Sheet", DataType = typeof(StandardBottomSheetViewModel))]
	public sealed partial class StandardBottomSheetSamplePage : Page
	{
		public StandardBottomSheetSamplePage()
		{
			this.InitializeComponent();
		}
	}
}
