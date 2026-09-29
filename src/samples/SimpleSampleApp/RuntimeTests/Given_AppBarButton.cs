#nullable enable

using System.Threading.Tasks;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Uno.Simple;
using Uno.Themes.Samples.Helpers;
using Uno.UI.RuntimeTests;

namespace Uno.Themes.Samples.RuntimeTests;

/// <summary>
/// Guards how the Simple AppBarButton template renders <c>Icon</c> and <c>Content</c>.
///
/// Regression guard for unoplatform/Uno.Themes#1735: Uno's <see cref="AppBarButton"/> looks up the
/// template part named <c>Content</c> and force-sets it to <c>Icon ?? Content</c>. The theme template
/// used that name for the icon presenter nested in the Viewbox that collapses when no Icon is set, so a
/// <see cref="UIElement"/> assigned to <c>Content</c> was re-parented into that collapsed presenter and
/// never rendered. A string survived only because it can be displayed by two presenters at once.
/// </summary>
[TestClass]
public class Given_AppBarButton
{
	private const string StyleKey = "SimpleAppBarButtonStyle";

	private static (Grid Container, AppBarButton Button) CreateThemedButton()
	{
		var container = new Grid();
		container.Resources.MergedDictionaries.Add(new SimpleTheme());

		var style = container.Resources[StyleKey] as Style;
		Assert.IsNotNull(style, $"{StyleKey} should be resolvable from the theme.");

		var button = new AppBarButton { Style = style };
		container.Children.Add(button);
		return (container, button);
	}

	private static async Task LoadAsync(Grid container, AppBarButton button)
	{
		UnitTestsUIContentHelper.Content = container;
		await UnitTestsUIContentHelper.WaitForLoaded(button);
		await UnitTestsUIContentHelper.WaitForIdle();
	}

	private static void AssertRenderedInside(AppBarButton button, FrameworkElement? element, string what)
	{
		Assert.IsNotNull(element, $"{what} should be part of the AppBarButton visual tree.");
		Assert.AreSame(button, element.FindFirstAncestor<AppBarButton>(), $"{what} should be hosted by the AppBarButton under test.");

		var collapsedAncestor = element.FindFirstAncestor<FrameworkElement>(x => x.Visibility == Visibility.Collapsed);
		Assert.IsNull(collapsedAncestor,
			$"{what} should not be nested under a collapsed element (found {collapsedAncestor?.GetType().Name}#{collapsedAncestor?.Name}).");
		Assert.IsTrue(element.ActualWidth > 0 && element.ActualHeight > 0,
			$"{what} should be laid out (was {element.ActualWidth}x{element.ActualHeight}).");
	}

	[TestMethod]
	[RunsOnUIThread]
	public async Task When_ContentIsUIElement_And_NoIcon_Then_ContentIsRendered()
	{
		var (container, button) = CreateThemedButton();
		var marker = new TextBlock { Text = "Hello" };
		button.Content = new Grid { Children = { marker } };

		await LoadAsync(container, button);

		AssertRenderedInside(button, marker, "The UIElement Content");
	}

	[TestMethod]
	[RunsOnUIThread]
	public async Task When_ContentIsUIElement_And_IconIsSet_Then_BothAreRendered()
	{
		var (container, button) = CreateThemedButton();
		var icon = new SymbolIcon(Symbol.Add);
		var marker = new TextBlock { Text = "Hello" };
		button.Icon = icon;
		button.Content = new Grid { Children = { marker } };

		await LoadAsync(container, button);

		AssertRenderedInside(button, icon, "The Icon");
		AssertRenderedInside(button, marker, "The UIElement Content");
	}

	[TestMethod]
	[RunsOnUIThread]
	public async Task When_ContentIsString_Then_TextIsRendered()
	{
		var (container, button) = CreateThemedButton();
		button.Content = "Hello";

		await LoadAsync(container, button);

		// Pick the visible TextBlock: a string may legitimately be hosted by more than one presenter.
		var text = button.EnumerateDescendants()
			.OfType<TextBlock>()
			.FirstOrDefault(x => x.Text == "Hello"
				&& x.FindFirstAncestor<FrameworkElement>(a => a.Visibility == Visibility.Collapsed) is null);
		AssertRenderedInside(button, text, "The string Content");
	}

	[TestMethod]
	[RunsOnUIThread]
	public async Task When_IconOnly_Then_IconIsRendered()
	{
		var (container, button) = CreateThemedButton();
		var icon = new SymbolIcon(Symbol.Add);
		button.Icon = icon;

		await LoadAsync(container, button);

		AssertRenderedInside(button, icon, "The Icon");
	}
}
