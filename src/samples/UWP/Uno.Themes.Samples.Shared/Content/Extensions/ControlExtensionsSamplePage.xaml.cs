using Uno.Themes.Samples.Entities;
using Uno.Themes.Samples.Entities.Data;
using Windows.UI.Xaml.Controls;

namespace Uno.Themes.Samples.Shared.Content.Extensions
{
	[SamplePage(SampleCategory.Helpers, "ControlExtensions", IconPath = Icons.Helpers.ControlExtensions, DataType = typeof(TestCommands))]
	public sealed partial class ControlExtensionsSamplePage : Page
	{
		public ControlExtensionsSamplePage()
		{
			this.InitializeComponent();
		}
	}
}
