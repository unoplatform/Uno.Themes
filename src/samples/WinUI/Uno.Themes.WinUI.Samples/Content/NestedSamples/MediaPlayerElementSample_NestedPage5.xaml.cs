using Windows.Media.Core;
using Windows.Media.Playback;

namespace Uno.Themes.WinUI.Samples.Content.NestedSamples;

public sealed partial class MediaPlayerElementSample_NestedPage5 : Page
{
    public MediaPlayerElementSample_NestedPage5()
    {
        this.InitializeComponent();
	InitializePlaybackList();
	Unloaded += MediaPlayerElementSample_NestedPage5_Unloaded;
}

private void InitializePlaybackList()
{
	var mediaPlaybackList = new MediaPlaybackList();

	mediaPlaybackList.Items.Add(new MediaPlaybackItem(MediaSource.CreateFromUri(new Uri("https://uno-assets.platform.uno/tests/videos/Mobile_Development_in_VS_Code_with_Uno_Platform_orDotNetMAUI.mp4"))));
	mediaPlaybackList.Items.Add(new MediaPlaybackItem(MediaSource.CreateFromUri(new Uri("https://uno-assets.platform.uno/tests/audio/Getting_Started_with_Uno_Platform_and_Visual_Studio_Code.mp3"))));
	mediaPlaybackList.Items.Add(new MediaPlaybackItem(MediaSource.CreateFromUri(new Uri("https://uno-assets.platform.uno/tests/videos/Getting_Started_with_Uno_Platform_and_Visual_Studio_Code.mp4"))));

	MediaPlayerElementSample5.MediaPlayer.Source = mediaPlaybackList;
}

private void NavigateBack(object sender, RoutedEventArgs e) => Shell.GetForCurrentView().BackNavigateFromNestedSample();

private void MediaPlayerElementSample_NestedPage5_Unloaded(object sender, RoutedEventArgs e)
{
	MediaPlayerElementSample5.MediaPlayer.Pause();
}
}
