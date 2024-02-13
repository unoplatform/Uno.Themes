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
		MediaPlayerElementSample2.MediaPlayer.Pause();
	}
}
