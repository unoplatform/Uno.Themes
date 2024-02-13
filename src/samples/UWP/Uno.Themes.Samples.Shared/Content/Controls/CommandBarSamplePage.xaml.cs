using Uno.Themes.Samples.Content.NestedSamples;

namespace Uno.Themes.Samples.Content.Controls;

[SamplePage(SampleCategory.Controls, "CommandBar", Description = "This control provides navigation and related actions for the current page.",
	DocumentationLink = "https://docs.microsoft.com/en-us/uwp/api/windows.ui.xaml.controls.commandbar")]
public sealed partial class CommandBarSamplePage : Page
{
	public CommandBarSamplePage()
	{
		this.InitializeComponent();
	}
	private void ShowSampleInNestedFrame(object sender, RoutedEventArgs e)
	{
		Shell.GetForCurrentView()?.ShowNestedSample<CommandBarSample_NestedPage1>(clearStack: true);
	}

	private void ShowM3SampleInNestedFrame(object sender, RoutedEventArgs e)
	{
		Shell.GetForCurrentView()?.ShowNestedSample<M3MaterialCommandBarSample_NestedPage1>(clearStack: true);
	}
}
