using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Media.Core;
using Windows.Media.Playback;

#if IS_WINUI
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
#else
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
#endif

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
