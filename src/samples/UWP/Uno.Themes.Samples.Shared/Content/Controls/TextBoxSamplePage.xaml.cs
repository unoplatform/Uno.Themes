using Uno.Themes.Samples.Entities;
using Uno.Themes.Samples.Entities.Data;
using Windows.UI.Xaml.Controls;

namespace Uno.Themes.Samples.Content.Controls
{
	[SamplePage(SampleCategory.Controls, "TextBox", Description = "This control allows users to input a textual value.", DocumentationLink = "https://docs.microsoft.com/en-us/uwp/api/windows.ui.xaml.controls.textbox", DataType = typeof(TestCommands))]
	public sealed partial class TextBoxSamplePage : Page
	{
		public TextBoxSamplePage()
		{
			this.InitializeComponent();
		}
	}
}
