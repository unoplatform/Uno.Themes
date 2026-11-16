#nullable enable

using System.Threading.Tasks;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Uno.Material;
using Uno.Themes.Samples.Helpers;
using Uno.UI.RuntimeTests;

namespace Uno.Themes.Samples.RuntimeTests;

/// <summary>
/// Guards how the Material AppBarButton template renders <c>Icon</c> and <c>Content</c>.
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
	private const string StyleKey = "MaterialAppBarButtonStyle";

	private static (Grid Container, AppBarButton Button) CreateThemedButton()
	{
		var container = new Grid();
		container.Resources.MergedDictionaries.Add(new MaterialTheme());

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

		var text = FindVisibleText(button, "Hello");
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

	/// <summary>
	/// Mirrors a NavigationBar / CommandBar toolbar whose PrimaryCommands mix Icon-only, Content-only and
	/// Icon + Content buttons, the way Uno.Toolkit.UI.NavigationBar hosts them (its primary commands use a
	/// style based on the theme AppBarButton style).
	/// </summary>
	[TestMethod]
	[RunsOnUIThread]
	public async Task When_HostedAsCommandBarPrimaryCommands_Then_IconAndContentVariantsAllRender()
	{
		var (container, _) = CreateThemedButton();
		container.Children.Clear();
		var style = (Style)container.Resources[StyleKey];

		var iconOnly = new SymbolIcon(Symbol.Add);
		var stringMarker = "Save";
		var elementMarker = new TextBlock { Text = "Hello" };
		var bothIcon = new SymbolIcon(Symbol.Edit);
		var bothLabel = "Edit";

		var commandBar = new CommandBar { IsOpen = false, IsDynamicOverflowEnabled = false };
		commandBar.PrimaryCommands.Add(new AppBarButton { Style = style, Icon = iconOnly });
		commandBar.PrimaryCommands.Add(new AppBarButton { Style = style, Content = stringMarker });
		commandBar.PrimaryCommands.Add(new AppBarButton { Style = style, Content = new Grid { Children = { elementMarker } } });
		commandBar.PrimaryCommands.Add(new AppBarButton { Style = style, Icon = bothIcon, Content = bothLabel });
		container.Children.Add(commandBar);

		UnitTestsUIContentHelper.Content = container;
		await UnitTestsUIContentHelper.WaitForLoaded(commandBar);
		await UnitTestsUIContentHelper.WaitForIdle();

		var buttons = commandBar.PrimaryCommands.Cast<AppBarButton>().ToArray();
		AssertRenderedInside(buttons[0], iconOnly, "The icon-only command's Icon");
		AssertRenderedInside(buttons[1], FindVisibleText(buttons[1], stringMarker), "The string-content command's Content");
		AssertRenderedInside(buttons[2], elementMarker, "The UIElement-content command's Content");
		AssertRenderedInside(buttons[3], bothIcon, "The icon + content command's Icon");
		AssertRenderedInside(buttons[3], FindVisibleText(buttons[3], bothLabel), "The icon + content command's Content");
	}

	// A string may legitimately be hosted by more than one presenter; the visible one is what matters.
	private static TextBlock? FindVisibleText(AppBarButton button, string text) => button.EnumerateDescendants()
		.OfType<TextBlock>()
		.FirstOrDefault(x => x.Text == text
			&& x.FindFirstAncestor<FrameworkElement>(a => a.Visibility == Visibility.Collapsed) is null);
}
