namespace Uno.Themes.Samples.Content.NestedSamples;

public sealed partial class M3MaterialCommandBarSample_NestedPage2 : Page
{
	public M3MaterialCommandBarSample_NestedPage2()
	{
		this.InitializeComponent();
	}

	private void NavigateBack(object sender, RoutedEventArgs e) => Frame.GoBack();
}
