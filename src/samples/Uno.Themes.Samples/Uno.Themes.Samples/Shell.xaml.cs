using Windows.UI.Core;

namespace Uno.Themes.Samples;

public sealed partial class Shell : UserControl
{
	public static Shell GetForCurrentView() => (Shell)App.MainWindow.Content;

	public NavigationView NavigationView => NavigationViewControl;

	public Shell()
	{
		this.InitializeComponent();

#if DEBUG
		this.DebuggingToolPanel.Visibility = Visibility.Visible;
#endif
		InitializeSafeArea();
		this.Loaded += OnLoaded;

		NestedSampleFrame.RegisterPropertyChangedCallback(ContentControl.ContentProperty, OnNestedSampleFrameChanged);

#if !WINDOWS_WINUI
		SystemNavigationManager.GetForCurrentView().BackRequested += (s, e) => e.Handled = BackNavigateFromNestedSample();
#endif
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

	/// <summary>
	/// This method handles the top padding for phones like iPhone X.
	/// </summary>
	private void InitializeSafeArea()
	{
		var full = App.MainWindow.Bounds;
#if !WINDOWS_WINUI
		var bounds = ApplicationView.GetForCurrentView().VisibleBounds;
#else
		var bounds = new Rect(0, 0, 0, 0);
#endif

		var topPadding = Math.Abs(full.Top - bounds.Top);

		if (topPadding > 0)
		{
			TopPaddingRow.Height = new GridLength(topPadding);
		}
	}

	private void SetDarkLightToggleInitialState()
	{
		// Initialize the toggle to the current theme.
		var root = App.MainWindow.Content as FrameworkElement;

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
		if (App.MainWindow.Content is FrameworkElement root)
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

			if (NavigationViewControl.PaneDisplayMode == NavigationViewPaneDisplayMode.LeftMinimal)
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

#if !WINDOWS_WINUI
		// toggle built-in back button for wasm (from browser) and uwp (on title bar)
		SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = isInsideNestedSample
			? AppViewBackButtonVisibility.Visible
			: AppViewBackButtonVisibility.Collapsed;
#endif
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

	private void NavigationViewControl_DisplayModeChanged(NavigationView sender, NavigationViewDisplayModeChangedEventArgs e)
	{
		if (e.DisplayMode == NavigationViewDisplayMode.Expanded)
		{
			NavigationViewControl.IsPaneOpen = NavigationViewControl.IsPaneVisible;
		}
	}
}
