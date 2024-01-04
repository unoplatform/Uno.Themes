using Uno.Themes.Samples.Content.NestedSamples;
using Uno.Themes.Samples.Entities;
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

namespace Uno.Themes.Samples.Content.Controls
{
	[SamplePage(SampleCategory.Controls, "MediaPlayerElement", Description = "Represents an object that uses a MediaPlayer to render audio and video to the display.", DocumentationLink = "https://learn.microsoft.com/en-us/uwp/api/windows.ui.xaml.controls.mediaplayerelement")]
	public sealed partial class MediaPlayerElementSamplePage : Page
    {
        public MediaPlayerElementSamplePage()
        {
            this.InitializeComponent();
        }

		private void LaunchMediaPlayerElementSample1(object sender, RoutedEventArgs e)
		{
			Shell.GetForCurrentView().ShowNestedSample<MediaPlayerElementSample_NestedPage1>(clearStack: true);
		}

		private void LaunchMediaPlayerElementSample2(object sender, RoutedEventArgs e)
		{
			Shell.GetForCurrentView().ShowNestedSample<MediaPlayerElementSample_NestedPage2>(clearStack: true);
		}

		private void LaunchMediaPlayerElementSample3(object sender, RoutedEventArgs e)
		{
			Shell.GetForCurrentView().ShowNestedSample<MediaPlayerElementSample_NestedPage3>(clearStack: true);
		}

		private void LaunchMediaPlayerElementSample4(object sender, RoutedEventArgs e)
		{
			Shell.GetForCurrentView().ShowNestedSample<MediaPlayerElementSample_NestedPage4>(clearStack: true);
		}

		private void LaunchMediaPlayerElementSample5(object sender, RoutedEventArgs e)
		{
			Shell.GetForCurrentView().ShowNestedSample<MediaPlayerElementSample_NestedPage5>(clearStack: true);
		}
	}
}
