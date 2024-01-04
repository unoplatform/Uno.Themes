using Uno.Themes.Samples.Entities;
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
[SamplePage(SampleCategory.Controls, "Number Box", Description = "This control can be used to set a numeric value.", DocumentationLink = "https://docs.microsoft.com/en-us/windows/uwp/design/controls-and-patterns/number-box")]
	public sealed partial class NumberBoxSamplePage : Page
    {
        public NumberBoxSamplePage()
        {
            this.InitializeComponent();
        }
    }
}
