using System.Threading.Tasks;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Uno.Cupertino;
using Uno.UI.RuntimeTests;
using Windows.UI;

namespace Uno.Themes.Samples.RuntimeTests;

/// <summary>
/// Verifies Cupertino navigation colours and geometry through the stock navigation presenters.
/// </summary>
[TestClass]
public class Given_CupertinoNavigation
{
	[TestMethod]
	[DataRow(ElementTheme.Light)]
	[DataRow(ElementTheme.Dark)]
	[RunsOnUIThread]
	public async Task When_SurfaceOverrideChanges_Then_RealizedBarTintRepaints(ElementTheme appearance)
	{
		var theme = new CupertinoTheme();
		var container = new Grid { Width = 600, Height = 200, RequestedTheme = appearance };
		container.Resources.MergedDictionaries.Add(theme);
		var bar = new CommandBar { Style = Resource<Style>(container, "CommandBarStyle") };
		bar.PrimaryCommands.Add(new AppBarButton { Icon = new SymbolIcon(Symbol.Add), Label = "Add" });
		container.Children.Add(bar);
		try
		{
			UnitTestsUIContentHelper.Content = container;
			await UnitTestsUIContentHelper.WaitForLoaded(bar);
			await UnitTestsUIContentHelper.WaitForIdle();
			var glass = FindDescendant<GlassPanel>(bar, "CommandGroupGlass");
			Assert.IsNotNull(glass);
			var tint = glass.Tint as SolidColorBrush;
			Assert.IsNotNull(tint);
			Assert.AreEqual(AppearanceColor(appearance, "SurfaceColor"), tint.Color);
			var overrides = new ResourceDictionary { ["SurfaceColor"] = Colors.Coral };
			theme.Colors = new ThemeColors { OverrideDictionary = overrides };
			await UnitTestsUIContentHelper.WaitForIdle();
			Assert.AreSame(tint, glass.Tint, "An existing navigation surface must keep its live brush");
			Assert.AreEqual(Colors.Coral, tint.Color);
			Assert.AreEqual(0.35, tint.Opacity, 0.001, "Color overrides retain the translucent bar treatment");
		}
		finally
		{
			UnitTestsUIContentHelper.Content = null;
		}
	}

	private static T Resource<T>(FrameworkElement scope, string key)
		=> scope.Resources.TryGetValue(key, out var value) && value is T typed
			? typed
			: throw new AssertFailedException($"Resource '{key}' not found or not a {typeof(T).Name}");

	// A direct Resources lookup follows the application appearance, not the scoped container's theme.
	// Read literal palette colours from the requested block to keep Light and Dark independent.
	private static Color AppearanceColor(ElementTheme appearance, string key)
	{
		var palette = new ResourceDictionary
		{
			Source = new Uri("ms-appx:///Uno.Cupertino.WinUI/Styles/Application/ColorPalette.xaml"),
		};
		var block = (ResourceDictionary)palette.ThemeDictionaries[appearance == ElementTheme.Light ? "Light" : "Default"];
		return (Color)block[key];
	}

	private static T? FindDescendant<T>(DependencyObject root, string name)
		where T : FrameworkElement
	{
		for (var i = 0; i < VisualTreeHelper.GetChildrenCount(root); i++)
		{
			var child = VisualTreeHelper.GetChild(root, i);
			if (child is T match && match.Name == name)
			{
				return match;
			}

			if (FindDescendant<T>(child, name) is { } nested)
			{
				return nested;
			}
		}

		return null;
	}

	[TestMethod]
	[DataRow(ElementTheme.Light, NavigationViewPaneDisplayMode.Left)]
	[DataRow(ElementTheme.Dark, NavigationViewPaneDisplayMode.Left)]
	[DataRow(ElementTheme.Light, NavigationViewPaneDisplayMode.Top)]
	[DataRow(ElementTheme.Dark, NavigationViewPaneDisplayMode.Top)]
	[RunsOnUIThread]
	public async Task When_ItemSelected_Then_PresenterUsesCupertinoSelection(ElementTheme theme, NavigationViewPaneDisplayMode mode)
	{
		var container = new Grid { Width = 800, Height = 500, RequestedTheme = theme };
		container.Resources.MergedDictionaries.Add(new CupertinoTheme());
		var itemStyle = Resource<Style>(container, "NavigationViewItemStyle");
		var first = new NavigationViewItem { Content = "Library", Icon = new SymbolIcon(Symbol.Library), Style = itemStyle };
		var second = new NavigationViewItem { Content = "Downloads", Style = itemStyle };
		var navigation = new NavigationView
		{
			Style = Resource<Style>(container, "NavigationViewStyle"),
			PaneDisplayMode = mode,
			IsPaneOpen = true,
			IsSettingsVisible = false,
			IsBackButtonVisible = NavigationViewBackButtonVisible.Collapsed,
		};
		navigation.MenuItems.Add(first);
		navigation.MenuItems.Add(second);
		navigation.SelectedItem = first;
		container.Children.Add(navigation);

		try
		{
			UnitTestsUIContentHelper.Content = container;
			await UnitTestsUIContentHelper.WaitForLoaded(navigation);
			await UnitTestsUIContentHelper.WaitForIdle();

			Assert.AreEqual(AppearanceColor(theme, "SurfaceColor"), (navigation.Background as SolidColorBrush)?.Color);
			Assert.IsTrue(first.ActualHeight >= 44, $"Navigation row must be at least 44 px; was {first.ActualHeight}");
			Assert.AreEqual(new CornerRadius(10), first.CornerRadius);
			AssertSelected(theme, first);

			// Selection transfer exercises the reset of the old row as well as the newly selected row.
			navigation.SelectedItem = second;
			await UnitTestsUIContentHelper.WaitForIdle();
			Assert.IsFalse(first.IsSelected);
			Assert.IsTrue(second.IsSelected);
			AssertSelected(theme, second);
			var oldLabel = FindDescendant<ContentPresenter>(first, "ContentPresenter");
			Assert.IsNotNull(oldLabel);
			Assert.AreEqual(AppearanceColor(theme, "OnSurfaceColor"), (oldLabel.Foreground as SolidColorBrush)?.Color);
		}
		finally
		{
			UnitTestsUIContentHelper.Content = null;
		}
	}

	private static void AssertSelected(ElementTheme theme, NavigationViewItem item)
	{
		var presenter = FindDescendant<NavigationViewItemPresenter>(item, "NavigationViewItemPresenter");
		Assert.IsNotNull(presenter, "The stock navigation item presenter must be realized");
		var indicator = FindDescendant<Microsoft.UI.Xaml.Shapes.Rectangle>(presenter, "SelectionIndicator");
		Assert.IsNotNull(indicator, "Keep the native selection animation part");
		Assert.AreEqual((byte)0, ((SolidColorBrush)indicator.Fill).Color.A, "iOS selection uses the rounded row without a separate WinUI selection line");
		var layout = FindDescendant<Grid>(presenter, "LayoutRoot");
		var label = FindDescendant<ContentPresenter>(presenter, "ContentPresenter");
		Assert.IsNotNull(layout);
		Assert.IsNotNull(label);
		Assert.AreEqual(AppearanceColor(theme, "PrimaryColor"), (layout.Background as SolidColorBrush)?.Color);
		Assert.AreEqual(AppearanceColor(theme, "PrimaryColor"), (label.Foreground as SolidColorBrush)?.Color);
		Assert.AreEqual(0.08, layout.Background.Opacity, 0.001, "selected background must remain a tint");
		Assert.AreEqual(1d, label.Foreground.Opacity, 0.001, "selected text must remain opaque");
		Assert.AreEqual(new CornerRadius(10), layout.CornerRadius);
	}
}
