namespace Uno.Themes.Samples.Content.NestedSamples;

public sealed partial class MediaPlayerElementSample_NestedPage2 : Page
{
	public MediaPlayerElementSample_NestedPage2()
	{
		this.InitializeComponent();
		Unloaded += MediaPlayerElementSample_NestedPage2_Unloaded;
	}

	private void NavigateBack(object sender, RoutedEventArgs e) => Shell.GetForCurrentView().BackNavigateFromNestedSample();

	private void MediaPlayerElementSample_NestedPage2_Unloaded(object sender, RoutedEventArgs e)
	{
		// MediaPlayer is null when no MediaPlayer extension is registered (e.g. Skia desktop without a
		// media backend). Throwing here would break the back navigation that triggered the unload.
		MediaPlayerElementSample2.MediaPlayer?.Pause();
	}
}
