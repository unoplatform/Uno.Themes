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
	public sealed partial class M3MaterialCommandBarSample_NestedPage1 : Page
	{
		public M3MaterialCommandBarSample_NestedPage1()
		{
			this.InitializeComponent();

			this.SeeCodeBehindButton.Content = GetCodeBehind();
			this.SeeAdditionSetupButton.Content = GetAdditionSetup();
		}

		private string GetCodeBehind()
		{
			return @"
private void NavigateToNextPage(object sender, RoutedEventArgs e) =>
	Frame.Navigate(typeof(CommandBarSample_NestedPage2));

private void NavigateBack(object sender, RoutedEventArgs e) =>
	Frame.GoBack();
".Trim("\r\n".ToCharArray());
		}

		private string GetAdditionSetup()
		{
			return @"
// in the MainPage or Shell:
public MainPage()
{
	this.InitializeComponent();

	SystemNavigationManager.GetForCurrentView().BackRequested += OnBackRequested;
}

private void OnBackRequested(object sender, BackRequestedEventArgs e)
{
	if (ContentFrame.CanGoBack)
	{
		ContentFrame.GoBack();

		// prevent default behaviors (android: back to home screen, browser: navigate back)
		// since we just handled in app.
		e.Handled = true;
	}
}
".Trim("\r\n".ToCharArray());
		}

		private void NavigateToNextPage(object sender, RoutedEventArgs e) =>
			Frame.Navigate(typeof(M3MaterialCommandBarSample_NestedPage2));

		private void NavigateBack(object sender, RoutedEventArgs e)
		{
			// Normally we would've just called `Frame.GoBack();` if we only have a single frame.
			// However, a nested frame is used to show-case fullscreen sample, so we need some
			// custom handling to hide the nested frame on back navigation when the stack is empty.
			Shell.GetForCurrentView()?.BackNavigateFromNestedSample();
		}
	}
}
