namespace Uno.Themes.Samples.Content.Controls;

[SamplePage(SampleCategory.Controls, "Flyout", Description = "A flyout is a UI container that can be light dismissed. It can contain other flyouts or context menus to create a nested experience.", DocumentationLink = "https://docs.microsoft.com/en-us/windows/uwp/design/controls-and-patterns/dialogs-and-flyouts/flyouts")]
public sealed partial class FlyoutSamplePage : Page
{
	public FlyoutSamplePage()
	{
		this.InitializeComponent();
	}

	private void CloseFlyout(object sender, RoutedEventArgs e)
	{
		var appBarButton = sender as AppBarButton;
		var button = (Button)appBarButton.Tag;
		button.Flyout?.Hide();
	}
}

