using Uno.Themes.Samples.Content.NestedSamples;
using Uno.Themes.Samples.Entities;
using Windows.UI.Xaml.Controls;


namespace Uno.Themes.Samples.Content.Controls
{
	[SamplePage(SampleCategory.Controls, "MediaPlayerElement", Description = "Represents an object that uses a MediaPlayer to render audio and video to the display.", DocumentationLink = "https://learn.microsoft.com/en-us/uwp/api/windows.ui.xaml.controls.mediaplayerelement")]
	public sealed partial class MediaPlayerElementSamplePage : Page
    {
        public MediaPlayerElementSamplePage()
        {
            this.InitializeComponent();
        }

		private void LaunchMediaPlayerElementSample1(object sender, Windows.UI.Xaml.RoutedEventArgs e)
		{
			Shell.GetForCurrentView().ShowNestedSample<MediaPlayerElementSample_NestedPage1>(clearStack: true);
		}

		private void LaunchMediaPlayerElementSample2(object sender, Windows.UI.Xaml.RoutedEventArgs e)
		{
			Shell.GetForCurrentView().ShowNestedSample<MediaPlayerElementSample_NestedPage2>(clearStack: true);
		}

		private void LaunchMediaPlayerElementSample3(object sender, Windows.UI.Xaml.RoutedEventArgs e)
		{
			Shell.GetForCurrentView().ShowNestedSample<MediaPlayerElementSample_NestedPage3>(clearStack: true);
		}

		private void LaunchMediaPlayerElementSample4(object sender, Windows.UI.Xaml.RoutedEventArgs e)
		{
			Shell.GetForCurrentView().ShowNestedSample<MediaPlayerElementSample_NestedPage4>(clearStack: true);
		}

		private void LaunchMediaPlayerElementSample5(object sender, Windows.UI.Xaml.RoutedEventArgs e)
		{
			Shell.GetForCurrentView().ShowNestedSample<MediaPlayerElementSample_NestedPage5>(clearStack: true);
		}
	}
}
