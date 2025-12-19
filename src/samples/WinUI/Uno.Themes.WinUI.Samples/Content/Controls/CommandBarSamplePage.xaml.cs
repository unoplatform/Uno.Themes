using Uno.Themes.WinUI.Samples.Content.NestedSamples;

namespace Uno.Themes.WinUI.Samples.Content.Controls;

[SamplePage(SampleCategory.Controls, "CommandBar", Description = "This control provides navigation and related actions for the current page.",
	DocumentationLink = "https://docs.microsoft.com/en-us/uwp/api/windows.ui.xaml.controls.commandbar")]
public sealed partial class CommandBarSamplePage : Page
{
	public CommandBarSamplePage()
	{
		this.InitializeComponent();
	}
}
