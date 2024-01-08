using System;
using System.Linq;
using Uno.Themes.Samples.Helpers;
using Windows.ApplicationModel.DataTransfer;
using Windows.UI.Core;
using Windows.UI.ViewManagement;
#if IS_WINUI
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Automation;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Markup;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using XamlWindow = Microsoft.UI.Xaml.Window;

#else
using Windows.UI.Xaml;
using Windows.UI.Xaml.Automation;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Markup;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using XamlWindow = Windows.UI.Xaml.Window;

#endif
using MUXC = Microsoft.UI.Xaml.Controls;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace Uno.Themes.Samples
{
	public sealed partial class Shell : UserControl
	{
		public static Shell GetForCurrentView() => (Shell)XamlWindow.Current.Content;

		public MUXC.NavigationView NavigationView => NavigationViewControl;

		public Shell()
		{
			this.InitializeComponent();

#if DEBUG
			this.DebuggingToolPanel.Visibility = Visibility.Visible;
#endif

#if !WINDOWS
			InitializeSafeArea();
			SystemNavigationManager.GetForCurrentView().BackRequested += (s, e) => e.Handled = BackNavigateFromNestedSample();
#endif

			this.Loaded += OnLoaded;

			NestedSampleFrame.RegisterPropertyChangedCallback(ContentControl.ContentProperty, OnNestedSampleFrameChanged);
		}

		private void OnLoaded(object sender, RoutedEventArgs e)
		{
			SetDarkLightToggleInitialState();
			//MonitorVisualStatesLive(NavigationView);
		}

		private void MonitorVisualStatesLive(Control control)
		{
			control.MonitorVisualStates(includeInitialStates: true, onStatesChanged: summary =>
			{
				DebugVisualStatesText.Text = summary;
				DebugVisualStatesText.Visibility = Visibility.Visible;

#if WINDOWS_UWP
				var data = new DataPackage();
				data.SetText(summary);
				Clipboard.SetContent(data);
#endif
			});
		}

		private void DebugVisualTree(object sender, RoutedEventArgs e)
		{
			var tree = this.TreeGraph();
			
#if WINDOWS_UWP
			var data = new DataPackage();
			data.SetText(string.Join("\n===\n", "visual-tree", tree));
			Clipboard.SetContent(data);
#endif

			// uwp: the visual tree should be copied to clipboard
			// uno: set a breakpoint on the next line and inspect `tree`
		}

#if !WINDOWS
		/// <summary>
		/// This method handles the top padding for phones like iPhone X.
		/// </summary>
		private void InitializeSafeArea()
		{
			var full = XamlWindow.Current.Bounds;
			var bounds = ApplicationView.GetForCurrentView().VisibleBounds;

			var topPadding = Math.Abs(full.Top - bounds.Top);

			if (topPadding > 0)
			{
				TopPaddingRow.Height = new GridLength(topPadding);
			}
		}
#endif

		private void SetDarkLightToggleInitialState()
		{
			// Initialize the toggle to the current theme.
			var root = this.XamlRoot.Content as FrameworkElement;

			switch (root.ActualTheme)
			{
				case ElementTheme.Default:
					DarkLightModeToggle.IsChecked = SystemThemeHelper.GetSystemApplicationTheme() == ApplicationTheme.Dark;
					break;
				case ElementTheme.Light:
					DarkLightModeToggle.IsChecked = false;
					break;
				case ElementTheme.Dark:
					DarkLightModeToggle.IsChecked = true;
					break;
			}
		}

		private void ToggleButton_Click(object sender, RoutedEventArgs e)
		{
			// Set theme for window root.
			if (this.XamlRoot.Content is FrameworkElement root)
			{
				switch (root.ActualTheme)
				{
					case ElementTheme.Default:
						if (SystemThemeHelper.GetSystemApplicationTheme() == ApplicationTheme.Dark)
						{
							root.RequestedTheme = ElementTheme.Light;
						}
						else
						{
							root.RequestedTheme = ElementTheme.Dark;
						}
						break;
					case ElementTheme.Light:
						root.RequestedTheme = ElementTheme.Dark;
						break;
					case ElementTheme.Dark:
						root.RequestedTheme = ElementTheme.Light;
						break;
				}

				if (NavigationViewControl.PaneDisplayMode == MUXC.NavigationViewPaneDisplayMode.LeftMinimal)
				{
					// Close navigation view when changing the theme
					// to allow the user to see the difference between the themes.
					NavigationViewControl.IsPaneOpen = false;
				}
			}
		}

		private void OnNestedSampleFrameChanged(DependencyObject sender, DependencyProperty dp)
		{
			var isInsideNestedSample = NestedSampleFrame.Content != null;

			// prevent empty frame from blocking the content (nav-view) behind it
			NestedSampleFrame.Visibility = isInsideNestedSample
				? Visibility.Visible
				: Visibility.Collapsed;

			// toggle built-in back button for wasm (from browser) and uwp (on title bar)
			SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = isInsideNestedSample
				? AppViewBackButtonVisibility.Visible
				: AppViewBackButtonVisibility.Collapsed;
		}

		public void ShowNestedSample<TPage>(bool? clearStack = null) where TPage : Page
		{
			var wasFrameEmpty = NestedSampleFrame.Content == null;
			if (NestedSampleFrame.Navigate(typeof(TPage)) && (clearStack ?? wasFrameEmpty))
			{
				NestedSampleFrame.BackStack.Clear();
			}
		}

		public bool BackNavigateFromNestedSample()
		{
			if (NestedSampleFrame.Content == null)
			{
				return false;
			}

			if (NestedSampleFrame.CanGoBack)
			{
				NestedSampleFrame.GoBack();
			}
			else
			{
				NestedSampleFrame.Content = null;

#if __IOS__
				// This will force reset the UINavigationController, to prevent the back button from appearing when the stack is supposely empty.
				// note: Merely setting the Frame.Content to null, doesnt fully reset the stack.
				// When revisiting the page1 again, the previous page1 is still in the UINavigationController stack
				// causing a back button to appear that takes us back to the previous page1
				NestedSampleFrame.BackStack.Add(default);
				NestedSampleFrame.BackStack.Clear();
#endif
			}

			return true;
		}

		private void NavigationViewControl_DisplayModeChanged(MUXC.NavigationView sender, MUXC.NavigationViewDisplayModeChangedEventArgs e)
		{
			if (e.DisplayMode == MUXC.NavigationViewDisplayMode.Expanded)
			{
				NavigationViewControl.IsPaneOpen = NavigationViewControl.IsPaneVisible;
			}
		}
	}
}
