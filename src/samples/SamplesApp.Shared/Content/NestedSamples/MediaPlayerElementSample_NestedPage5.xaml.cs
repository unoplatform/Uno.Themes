using Windows.Media.Core;
using Windows.Media.Playback;

namespace Uno.Themes.Samples.Content.NestedSamples;

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
		// MediaPlayer is null when no MediaPlayer extension is registered (e.g. Skia desktop without a
		// media backend). The page must still load so the back button stays reachable.
		if (MediaPlayerElementSample5.MediaPlayer is not { } mediaPlayer)
		{
			return;
		}

		var mediaPlaybackList = new MediaPlaybackList();

		mediaPlaybackList.Items.Add(new MediaPlaybackItem(MediaSource.CreateFromUri(new Uri("https://uno-assets.platform.uno/tests/videos/Mobile_Development_in_VS_Code_with_Uno_Platform_orDotNetMAUI.mp4"))));
		mediaPlaybackList.Items.Add(new MediaPlaybackItem(MediaSource.CreateFromUri(new Uri("https://uno-assets.platform.uno/tests/audio/Getting_Started_with_Uno_Platform_and_Visual_Studio_Code.mp3"))));
		mediaPlaybackList.Items.Add(new MediaPlaybackItem(MediaSource.CreateFromUri(new Uri("https://uno-assets.platform.uno/tests/videos/Getting_Started_with_Uno_Platform_and_Visual_Studio_Code.mp4"))));

		mediaPlayer.Source = mediaPlaybackList;
	}

	private void NavigateBack(object sender, RoutedEventArgs e) => Shell.GetForCurrentView().BackNavigateFromNestedSample();

	private void MediaPlayerElementSample_NestedPage5_Unloaded(object sender, RoutedEventArgs e)
	{
		MediaPlayerElementSample5.MediaPlayer?.Pause();
	}
}
