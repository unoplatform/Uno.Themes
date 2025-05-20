using System.Threading.Tasks;

namespace Uno.Themes.Samples.Content.Controls;

[SamplePage(SampleCategory.Controls, "Button", Description = "A button is used to interpret a user click or tap interaction. They're often bound to commands.", DocumentationLink = "https://docs.microsoft.com/en-us/windows/uwp/design/controls-and-patterns/buttons")]
public sealed partial class ButtonSamplePage : Page
{
	public ButtonSamplePage()
	{
		this.InitializeComponent();
	}

	private async void Button_Click(object sender, RoutedEventArgs e)
	{
		if (sender is Button b)
		{
			b.IsEnabled = false;
			var debug = 1;
		}
    }
}
