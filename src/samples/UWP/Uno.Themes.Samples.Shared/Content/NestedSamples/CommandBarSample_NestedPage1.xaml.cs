namespace Uno.Themes.Samples.Content.NestedSamples;

public sealed partial class CommandBarSample_NestedPage1 : Page
{
	public CommandBarSample_NestedPage1()
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
		Frame.Navigate(typeof(CommandBarSample_NestedPage2));

	private void NavigateBack(object sender, RoutedEventArgs e)
	{
		// Normally we would've just called `Frame.GoBack();` if we only have a single frame.
		// However, a nested frame is used to show-case fullscreen sample, so we need some
		// custom handling to hide the nested frame on back navigation when the stack is empty.
		Shell.GetForCurrentView()?.BackNavigateFromNestedSample();
	}
}
