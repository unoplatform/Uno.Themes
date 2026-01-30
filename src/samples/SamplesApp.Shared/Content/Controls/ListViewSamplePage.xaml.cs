// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Uno.Themes.Samples.Content.Controls;

/// <summary>
/// An empty page that can be used on its own or navigated to within a Frame.
/// </summary>
[SamplePage(SampleCategory.Controls, "ListView", Description = "Represents a control that displays data items in a vertical stack.", DocumentationLink = "https://docs.microsoft.com/en-us/windows/winui/api/microsoft.ui.xaml.controls.ListView?view=winui-3.0", SupportedDesigns = new[] { Design.Material })]
public sealed partial class ListViewSamplePage : Page
    {
		public string Letters => "abcdefghijklmn";

		public ListViewSamplePage()
        {
            this.InitializeComponent();
        }
    }
