using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace Uno.Themes.Samples.Content.Controls
{
	[SamplePage(
		SampleCategory.Controls,
		"TabView",
		Description = "Tabs for switching between related views.",
		DocumentationLink = "https://learn.microsoft.com/windows/apps/design/controls/tab-view")]
	public sealed partial class TabViewSamplePage : Page
	{
		private int _newTabCounter = 3;

		public TabViewSamplePage()
		{
			InitializeComponent();
		}
		private void AddTabButtonClick(TabView sender, object args)
		{
			var index = _newTabCounter++;
			var newItem = new TabViewItem
			{
				Header = $"Document {index}",
				IsClosable = true,
				Content = new TextBlock { Margin = new Thickness(12), Text = $"Document {index} content" },
				Style = (Style)Application.Current.Resources["MaterialTabViewItemStyle"]
			};

			sender.TabItems.Add(newItem);
			sender.SelectedItem = newItem;
		}

		private void TabCloseRequested(TabView sender, TabViewTabCloseRequestedEventArgs args)
		{
			sender.TabItems.Remove(args.Tab);
		}
	}
}
