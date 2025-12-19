namespace Uno.Themes.WinUI.Samples.Content.NestedSamples;

public sealed partial class MediaPlayerElementSample_NestedPage1 : Page
{
	public MediaPlayerElementSample_NestedPage1()
	{
		this.InitializeComponent();
		Unloaded += MediaPlayerElementSample_NestedPage1_Unloaded;
	}

	private void NavigateBack(object sender, RoutedEventArgs e) => Shell.GetForCurrentView().BackNavigateFromNestedSample();

	private void MediaPlayerElementSample_NestedPage1_Unloaded(object sender, RoutedEventArgs e)
	{
		MediaPlayerElementSample1.MediaPlayer.Pause();
	}
}
