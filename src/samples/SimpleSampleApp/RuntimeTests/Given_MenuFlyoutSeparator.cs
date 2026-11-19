#nullable enable

using System.Threading.Tasks;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Shapes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Uno.Simple;
using Uno.Themes.Samples.Helpers;
using Uno.UI.RuntimeTests;

namespace Uno.Themes.Samples.RuntimeTests;

/// <summary>
/// The Simple separator template sized its line with <c>SimpleMenuFlyoutSeparatorHeight</c>, a key that
/// was never defined. Uno dropped the height silently; WinUI failed the template and took the app down
/// the first time a Simple MenuFlyout with a separator opened.
/// </summary>
[TestClass]
public class Given_MenuFlyoutSeparator
{
	[TestMethod]
	[RunsOnUIThread]
	public async Task When_SimpleSeparatorLoaded_Then_ItDrawsAOnePixelLine()
	{
		var container = new StackPanel { Width = 200 };
		container.Resources.MergedDictionaries.Add(new SimpleTheme());
		var separator = new MenuFlyoutSeparator { Style = container.Resources["SimpleMenuFlyoutSeparatorStyle"] as Style };
		container.Children.Add(separator);

		UnitTestsUIContentHelper.Content = container;
		await UnitTestsUIContentHelper.WaitForLoaded(separator);
		await UnitTestsUIContentHelper.WaitForIdle();

		var line = separator.FindFirstDescendant<Rectangle>();
		Assert.IsNotNull(line, "The Simple separator template should render its line.");
		Assert.AreEqual(1d, line.Height, "SimpleMenuFlyoutSeparatorHeight should size the separator line.");
	}
}
