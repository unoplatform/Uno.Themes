using Uno.Themes.Samples.Entities;
using Windows.UI.Xaml.Controls;

namespace Uno.Themes.Samples.Content.Controls
{
[SamplePage(SampleCategory.Controls, "Number Box", Description = "This control can be used to set a numeric value.", DocumentationLink = "https://docs.microsoft.com/en-us/windows/uwp/design/controls-and-patterns/number-box")]
	public sealed partial class NumberBoxSamplePage : Page
    {
        public NumberBoxSamplePage()
        {
            this.InitializeComponent();
        }
    }
}
