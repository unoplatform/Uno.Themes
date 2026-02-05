using Microsoft.UI.Xaml.Automation;

namespace Uno.Themes.Samples;

public sealed partial class Shell : UserControl
{
	public static Shell GetForCurrentView() => (Shell)(App.MainWindow?.Content ?? throw new InvalidOperationException("MainWindow not initialized"));

	public MUXC.NavigationView NavigationView => NavigationViewControl;

	public Shell()
	{
		this.InitializeComponent();
		InitializeSafeArea();
		this.Loaded += OnLoaded;
	}

	private void OnLoaded(object sender, RoutedEventArgs e)
	{
		SetDarkLightToggleInitialState();
	}

	/// <summary>
	/// This method handles the top padding for phones like iPhone X.
	/// </summary>
	private void InitializeSafeArea()
	{
		var full = App.MainWindow?.Bounds ?? default;
		var bounds = new Windows.Foundation.Rect(0, 0, 0, 0);

		var topPadding = Math.Abs(full.Top - bounds.Top);

		if (topPadding > 0)
		{
			TopPaddingRow.Height = new GridLength(topPadding);
		}
	}

	private void SetDarkLightToggleInitialState()
	{
		var root = App.MainWindow?.Content as FrameworkElement;
		if (root == null) return;

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
		if (App.MainWindow?.Content is FrameworkElement root)
		{
			switch (root.ActualTheme)
			{
				case ElementTheme.Default:
					root.RequestedTheme = SystemThemeHelper.GetSystemApplicationTheme() == ApplicationTheme.Dark
						? ElementTheme.Light
						: ElementTheme.Dark;
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
				NavigationViewControl.IsPaneOpen = false;
			}
		}
	}

	private void NavigationViewControl_DisplayModeChanged(MUXC.NavigationView sender, MUXC.NavigationViewDisplayModeChangedEventArgs e)
	{
		if (e.DisplayMode == MUXC.NavigationViewDisplayMode.Expanded)
		{
			NavigationViewControl.IsPaneOpen = NavigationViewControl.IsPaneVisible;
		}
	}
}
