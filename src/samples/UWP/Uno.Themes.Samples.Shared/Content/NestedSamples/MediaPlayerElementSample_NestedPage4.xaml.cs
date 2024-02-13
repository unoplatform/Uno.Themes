namespace Uno.Themes.Samples.Content.NestedSamples;

public sealed partial class MediaPlayerElementSample_NestedPage4 : Page
    {
        public MediaPlayerElementSample_NestedPage4()
        {
            this.InitializeComponent();
		Unloaded += MediaPlayerElementSample_NestedPage4_Unloaded;
	}

	private void NavigateBack(object sender, RoutedEventArgs e) => Shell.GetForCurrentView().BackNavigateFromNestedSample();

	private void MediaPlayerElementSample_NestedPage4_Unloaded(object sender, RoutedEventArgs e)
	{
		MediaPlayerElementSample4.MediaPlayer.Pause();
	}
}
