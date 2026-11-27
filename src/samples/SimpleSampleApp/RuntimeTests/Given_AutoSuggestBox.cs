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

	[TestMethod]
	[RunsOnUIThread]
	public async Task When_TextBoxStyleUsedWithTag_Then_NoErrorLook()
	{
		// Arrange: the public inner style on a plain TextBox whose Tag carries the app's own data
		var container = new Grid { Width = 400 };
		container.Resources.MergedDictionaries.Add(new SimpleTheme());
		var textBox = new TextBox
		{
			Style = (Style)container.Resources["SimpleAutoSuggestBoxTextBoxStyle"],
			Tag = "app data",
		};
		container.Children.Add(textBox);

		// Act
		UnitTestsUIContentHelper.Content = container;
		await UnitTestsUIContentHelper.WaitForLoaded(textBox);
		await UnitTestsUIContentHelper.WaitForIdle();

		// Assert
		AssertErrorLook(textBox, expected: false);
	}

	[TestMethod]
	[RunsOnUIThread]
	public async Task When_IsInErrorSetThenCleared_Then_ErrorLookFollows()
	{
		// Arrange
		var container = new Grid { Width = 400 };
		container.Resources.MergedDictionaries.Add(new SimpleTheme());
		var textBox = new TextBox { Style = (Style)container.Resources["SimpleAutoSuggestBoxTextBoxStyle"] };
		container.Children.Add(textBox);

		UnitTestsUIContentHelper.Content = container;
		await UnitTestsUIContentHelper.WaitForLoaded(textBox);
		await UnitTestsUIContentHelper.WaitForIdle();
		AssertErrorLook(textBox, expected: false);

		// Act: set
		ControlExtensions.SetIsInError(textBox, true);
		await UnitTestsUIContentHelper.WaitForIdle();

		// Assert
		AssertErrorLook(textBox, expected: true);

		// Act: clear
		ControlExtensions.SetIsInError(textBox, false);
		await UnitTestsUIContentHelper.WaitForIdle();

		// Assert
		AssertErrorLook(textBox, expected: false);
	}

	private static void AssertErrorLook(TextBox textBox, bool expected)
	{
		var state = expected ? "in error" : "not in error";
		Assert.AreEqual(expected ? Visibility.Visible : Visibility.Collapsed, FindPart(textBox, "ErrorBorderElement").Visibility, $"The error ring should {(expected ? "show" : "hide")} {state}");
		Assert.AreEqual(expected ? Visibility.Visible : Visibility.Collapsed, FindPart(textBox, "ErrorBackgroundElement").Visibility, $"The error tint should {(expected ? "show" : "hide")} {state}");
		Assert.AreEqual(expected ? Visibility.Collapsed : Visibility.Visible, FindPart(textBox, "BorderStrokeElement").Visibility, $"The stroke should {(expected ? "hide" : "show")} {state}");
	}

	private static FrameworkElement FindPart(Control control, string name) =>
		control.FindFirstDescendant<FrameworkElement>(x => x.Name == name)
			?? throw new AssertFailedException($"{name} should be part of the template");

	private static IEnumerable<DependencyObject> GetAncestors(DependencyObject element)
	{
		for (var parent = VisualTreeHelper.GetParent(element); parent is not null; parent = VisualTreeHelper.GetParent(parent))
		{
			yield return parent;
		}
	}
}
