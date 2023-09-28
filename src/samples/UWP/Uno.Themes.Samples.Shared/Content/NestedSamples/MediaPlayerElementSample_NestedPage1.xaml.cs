﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace Uno.Themes.Samples.Content.NestedSamples
{
	public sealed partial class MediaPlayerElementSample_NestedPage1 : Page
	{
		public MediaPlayerElementSample_NestedPage1()
		{
			this.InitializeComponent();
			Unloaded += MediaPlayerElementSample_NestedPage1_Unloaded;
		}

		private void NavigateBack(object sender, RoutedEventArgs e) => Shell.GetForCurrentView().BackNavigateFromNestedSample();

		private void MediaPlayerElementSample_NestedPage1_Unloaded(object sender, RoutedEventArgs e)
		{
			MediaPlayerElementSample1.MediaPlayer.Pause();
		}
	}
}
