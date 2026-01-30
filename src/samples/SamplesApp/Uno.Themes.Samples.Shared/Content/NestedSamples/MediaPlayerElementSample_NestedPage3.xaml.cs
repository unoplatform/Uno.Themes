namespace Uno.Themes.Samples.Content.NestedSamples;

public sealed partial class MediaPlayerElementSample_NestedPage3 : Page
    {
        public MediaPlayerElementSample_NestedPage3()
        {
            this.InitializeComponent();
		Unloaded += MediaPlayerElementSample_NestedPage3_Unloaded;
	}

	private void NavigateBack(object sender, RoutedEventArgs e) => Shell.GetForCurrentView().BackNavigateFromNestedSample();

	private void MediaPlayerElementSample_NestedPage3_Unloaded(object sender, RoutedEventArgs e)
	{
		MediaPlayerElementSample3.MediaPlayer.Pause();
	}
}
