using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;

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

namespace Uno.Themes.Samples.Content.NestedSamples
{
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
}
