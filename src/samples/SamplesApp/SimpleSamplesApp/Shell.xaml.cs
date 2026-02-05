using Uno.Themes.Samples.Entities;
using Uno.Themes.Samples.Helpers;

namespace Uno.Themes.Samples;

public sealed partial class Shell : Page
{
	public Shell()
	{
		this.InitializeComponent();
		Loaded += OnLoaded;
	}

	public NavigationView NavigationView => this.NavigationView;
	public Frame ContentFrame => this.ContentFrame;

	private void OnLoaded(object sender, RoutedEventArgs e)
	{
		// Handle responsive pane display mode
		SizeChanged += OnSizeChanged;
		UpdatePaneDisplayMode();
	}

	private void OnSizeChanged(object sender, SizeChangedEventArgs e)
	{
		UpdatePaneDisplayMode();
	}

	private void UpdatePaneDisplayMode()
	{
		var threshold = (double)Application.Current.Resources["DesktopAdaptiveThresholdWidth"];
		NavigationView.PaneDisplayMode = ActualWidth >= threshold
			? NavigationViewPaneDisplayMode.Left
			: NavigationViewPaneDisplayMode.LeftMinimal;
	}

	private void OnNavigationViewBackRequested(NavigationView sender, NavigationViewBackRequestedEventArgs args)
	{
		if (ContentFrame.CanGoBack)
		{
			ContentFrame.GoBack();
			UpdateSelectedItem();
		}
	}

	private void OnNavigationViewItemInvoked(NavigationView sender, NavigationViewItemInvokedEventArgs args)
	{
		if (args.InvokedItemContainer is NavigationViewItem navItem &&
			navItem.Tag is Sample sample)
		{
			ContentFrame.Navigate(sample.ViewType);
		}
	}

	private void UpdateSelectedItem()
	{
		var currentPageType = ContentFrame.CurrentSourcePageType;
		if (currentPageType == null) return;

		foreach (var item in NavigationView.MenuItems.OfType<NavigationViewItem>())
		{
			if (item.Tag is Sample sample && sample.ViewType == currentPageType)
			{
				NavigationView.SelectedItem = item;
				return;
			}
		}
	}

	private void OnThemeToggleClick(object sender, RoutedEventArgs e)
	{
		var isDark = SystemThemeHelper.IsDarkTheme;
		if (XamlRoot?.Content is FrameworkElement rootElement)
		{
			rootElement.RequestedTheme = isDark ? ElementTheme.Light : ElementTheme.Dark;
		}
	}
}
