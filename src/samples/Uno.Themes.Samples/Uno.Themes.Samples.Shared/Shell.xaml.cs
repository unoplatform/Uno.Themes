using System;
using Uno.Themes.Samples.Helpers;
using Windows.UI.Core;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using MUXC = Microsoft.UI.Xaml.Controls;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace Uno.Themes.Samples
{
	public sealed partial class Shell : UserControl
	{
		public Shell()
		{
			this.InitializeComponent();

			InitializeSafeArea();
			this.Loaded += OnLoaded;

			NestedSampleFrame.RegisterPropertyChangedCallback(ContentControl.ContentProperty, OnNestedSampleFrameChanged);
			SystemNavigationManager.GetForCurrentView().BackRequested += (s, e) => e.Handled = BackNavigateFromNestedSample();
		}

		public static Shell GetForCurrentView() => (Shell)Windows.UI.Xaml.Window.Current.Content;

		public MUXC.NavigationView NavigationView => NavigationViewControl;

		private void OnLoaded(object sender, RoutedEventArgs e)
		{
			SetDarkLightToggleInitialState();
		}

		private void SetDarkLightToggleInitialState()
		{
			// Initialize the toggle to the current theme.
			var root = global::Windows.UI.Xaml.Window.Current.Content as FrameworkElement;

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

		/// <summary>
		/// This method handles the top padding for phones like iPhone X.
		/// </summary>
		private void InitializeSafeArea()
		{
			var full = Windows.UI.Xaml.Window.Current.Bounds;
			var bounds = ApplicationView.GetForCurrentView().VisibleBounds;

			var topPadding = Math.Abs(full.Top - bounds.Top);

			if (topPadding > 0)
			{
				TopPaddingRow.Height = new GridLength(topPadding);
			}
		}

		private void ToggleButton_Click(object sender, RoutedEventArgs e)
		{
			// Set theme for window root.
			if (global::Windows.UI.Xaml.Window.Current.Content is FrameworkElement root)
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

			NavViewToggleButton.Visibility = isInsideNestedSample
				? Visibility.Collapsed
				: Visibility.Visible;

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

		private void NavViewToggleButton_Click(object sender, RoutedEventArgs e)
		{
			if (NavigationViewControl.PaneDisplayMode == MUXC.NavigationViewPaneDisplayMode.LeftMinimal)
			{
				NavigationViewControl.IsPaneOpen = !NavigationViewControl.IsPaneOpen;
			}
			else if (NavigationViewControl.PaneDisplayMode == MUXC.NavigationViewPaneDisplayMode.Left)
			{
				NavigationViewControl.IsPaneVisible = !NavigationViewControl.IsPaneVisible;
				NavigationViewControl.IsPaneOpen = NavigationViewControl.IsPaneVisible;
			}
		}

		private void NavigationViewControl_SizeChanged(object sender, SizeChangedEventArgs e)
		{
			// This could be done using VisualState with Adaptive triggers, but an issue prevents this currently - https://github.com/unoplatform/uno/issues/5168
			var desktopWidth = (double)Application.Current.Resources["DesktopAdaptiveThresholdWidth"];
			if (e.NewSize.Width >= desktopWidth && NavigationViewControl.PaneDisplayMode != MUXC.NavigationViewPaneDisplayMode.Left)
			{
				NavigationViewControl.PaneDisplayMode = MUXC.NavigationViewPaneDisplayMode.Left;
				NavigationViewControl.IsPaneOpen = true;
			}
			else if (e.NewSize.Width < desktopWidth && NavigationViewControl.PaneDisplayMode != MUXC.NavigationViewPaneDisplayMode.LeftMinimal)
			{
				NavigationViewControl.IsPaneVisible = true;
				NavigationViewControl.PaneDisplayMode = MUXC.NavigationViewPaneDisplayMode.LeftMinimal;
			}
		}
	}
}
