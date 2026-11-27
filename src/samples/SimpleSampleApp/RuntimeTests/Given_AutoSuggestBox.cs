#nullable enable

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Uno.Simple;
using Uno.Themes.Samples.Helpers;
using Uno.UI.RuntimeTests;

namespace Uno.Themes.Samples.RuntimeTests;

/// <summary>
/// Guards the Simple AutoSuggestBox header. The header is drawn by the inner TextBox; the outer template only
/// reserves its height (to center the input validation icon on the field), so it must never present the
/// header itself. An element header can have only one parent: on WinUI an invisible outer stand-in that also
/// presented it took it, and the header disappeared.
/// </summary>
[TestClass]
public class Given_AutoSuggestBox
{
	[TestMethod]
	[RunsOnUIThread]
	public async Task When_HeaderIsAnElement_Then_InnerTextBoxPresentsIt()
	{
		// Arrange
		var container = new Grid { Width = 400 };
		container.Resources.MergedDictionaries.Add(new SimpleTheme());
		var header = new TextBlock { Text = "Header" };
		var autoSuggestBox = new AutoSuggestBox
		{
			Style = (Style)container.Resources["SimpleAutoSuggestBoxStyle"],
			Header = header,
		};
		container.Children.Add(autoSuggestBox);

		// Act
		UnitTestsUIContentHelper.Content = container;
		await UnitTestsUIContentHelper.WaitForLoaded(autoSuggestBox);
		await UnitTestsUIContentHelper.WaitForIdle();

		// Assert
		var textBox = autoSuggestBox.FindFirstDescendant<TextBox>(x => x.Name == "TextBox");
		Assert.IsNotNull(textBox, "The Simple AutoSuggestBox should render its inner TextBox.");
		Assert.IsTrue(GetAncestors(header).Contains(textBox), "The header element should be shown by the inner TextBox.");
		Assert.IsTrue(header.ActualHeight > 0, "The header element should be laid out.");
	}

	private static IEnumerable<DependencyObject> GetAncestors(DependencyObject element)
	{
		for (var parent = VisualTreeHelper.GetParent(element); parent is not null; parent = VisualTreeHelper.GetParent(parent))
		{
			yield return parent;
		}
	}
}
