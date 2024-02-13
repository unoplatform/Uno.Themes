namespace Uno.Themes.Samples.Content.NestedSamples;

public sealed partial class CommandBarSample_NestedPage2 : Page
{
	public CommandBarSample_NestedPage2()
	{
		this.InitializeComponent();
	}

	private void NavigateBack(object sender, RoutedEventArgs e) => Frame.GoBack();
}
