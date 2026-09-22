using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Uno.Cupertino;
using Uno.UI.RuntimeTests;
using Windows.UI;

namespace Uno.Themes.Samples.RuntimeTests;

/// <summary>
/// Verifies the Cupertino container styles on realized controls: what a popover, a menu, a list or a
/// dialog ends up rendering.
/// </summary>
[TestClass]
public class Given_CupertinoContainers
{
	private static Grid CreateThemedContainer()
	{
		var container = new Grid();
		container.Resources.MergedDictionaries.Add(new CupertinoTheme());
		return container;
	}

	private static T Resource<T>(FrameworkElement scope, string key)
		=> scope.Resources.TryGetValue(key, out var value) && value is T typed
			? typed
			: throw new AssertFailedException($"Resource '{key}' not found or not a {typeof(T).Name}");

	// Brushes inside a popup resolve against the application theme, another instance of the same palette,
	// so colours are compared, never brush instances (specs/lessons.md).
	private static Color ColorOf(FrameworkElement scope, string key) => Resource<SolidColorBrush>(scope, key).Color;

	private static Color? ColorOf(Brush? brush) => (brush as SolidColorBrush)?.Color;

	private static async Task<T> Load<T>(Grid container, T control)
		where T : FrameworkElement
	{
		container.Children.Add(control);
		UnitTestsUIContentHelper.Content = container;
		await UnitTestsUIContentHelper.WaitForLoaded(control);
		await UnitTestsUIContentHelper.WaitForIdle();
		return control;
	}

	private static T? FindDescendant<T>(DependencyObject root, string? name = null)
		where T : FrameworkElement
	{
		for (var i = 0; i < VisualTreeHelper.GetChildrenCount(root); i++)
		{
			var child = VisualTreeHelper.GetChild(root, i);
			if (child is T match && (name is null || match.Name == name))
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

	// The presenter an open flyout put in the popup layer.
	private static T OpenPresenter<T>(FrameworkElement anchor)
		where T : FrameworkElement
	{
		foreach (var popup in VisualTreeHelper.GetOpenPopupsForXamlRoot(anchor.XamlRoot))
		{
			if (popup.Child is { } child && (child as T ?? FindDescendant<T>(child)) is { } presenter)
			{
				return presenter;
			}
		}

		throw new AssertFailedException($"no open {typeof(T).Name}");
	}

	private static void AssertPopoverGlass(Grid container, FrameworkElement presenter, string what)
	{
		var glass = FindDescendant<GlassPanel>(presenter) ?? throw new AssertFailedException($"{what}: no glass behind it");
		Assert.AreEqual(GlassMaterial.Thick, glass.Material, $"{what}: material");
		Assert.AreEqual(26d, glass.CornerRadius.TopLeft, $"{what}: the popover radius");
		Assert.AreEqual(ColorOf(container, "CupertinoPopoverTintBrush"), ColorOf(glass.Tint), $"{what}: the popover tint");
	}

	[TestMethod]
	[RunsOnUIThread]
	public async Task When_FlyoutOpened_Then_ItIsAThickGlassPopover()
	{
		var container = CreateThemedContainer();
		var flyout = new Flyout
		{
			Content = new TextBlock { Text = "Popover" },
			FlyoutPresenterStyle = Resource<Style>(container, "FlyoutPresenterStyle"),
		};

		try
		{
			var anchor = await Load(container, new Button { Content = "Anchor" });
			flyout.ShowAt(anchor);
			await UnitTestsUIContentHelper.WaitForIdle();

			var presenter = OpenPresenter<FlyoutPresenter>(anchor);
			AssertPopoverGlass(container, presenter, "flyout");
			Assert.AreEqual(new Thickness(16), presenter.Padding);
		}
		finally
		{
			flyout.Hide();
			UnitTestsUIContentHelper.Content = null;
		}
	}

	[TestMethod]
	[RunsOnUIThread]
	public async Task When_MenuFlyoutOpened_Then_RowsAre44HighOnTheSameGlass()
	{
		var container = CreateThemedContainer();
		var item = new MenuFlyoutItem { Text = "Copy", Icon = new SymbolIcon(Symbol.Copy) };
		var toggle = new ToggleMenuFlyoutItem { Text = "Show toolbar", IsChecked = true };
		var sub = new MenuFlyoutSubItem { Text = "Sort by" };
		sub.Items.Add(new MenuFlyoutItem { Text = "Name" });
		var radio = new RadioMenuFlyoutItem { Text = "Ascending", IsChecked = true };
		var menu = new MenuFlyout { MenuFlyoutPresenterStyle = Resource<Style>(container, "MenuFlyoutPresenterStyle") };
		menu.Items.Add(item);
		menu.Items.Add(toggle);
		menu.Items.Add(new MenuFlyoutSeparator());
		menu.Items.Add(sub);
		menu.Items.Add(radio);

		try
		{
			var anchor = await Load(container, new Button { Content = "Anchor" });
			menu.ShowAt(anchor);
			await UnitTestsUIContentHelper.WaitForIdle();

			var presenter = OpenPresenter<MenuFlyoutPresenter>(anchor);
			AssertPopoverGlass(container, presenter, "menu");
			Assert.IsTrue(presenter.ActualWidth >= 250, $"a menu is at least 250 wide, was {presenter.ActualWidth}");

			var label = ColorOf(container, "OnSurfaceBrush");
			foreach (var row in new Control[] { item, toggle, sub, radio })
			{
				Assert.IsTrue(row.ActualHeight >= 44, $"{row.GetType().Name}: a row is 44 high, was {row.ActualHeight}");
				Assert.AreEqual(label, ColorOf(row.Foreground), $"{row.GetType().Name}: label colour");
			}

			Assert.AreEqual(Visibility.Visible, FindDescendant<Viewbox>(item, "IconRoot")?.Visibility, "an item with an icon shows it");
			Assert.AreEqual(1d, FindDescendant<FontIcon>(toggle, "CheckGlyph")?.Opacity, "a checked toggle shows its checkmark");
			Assert.AreEqual(1d, FindDescendant<FontIcon>(radio, "CheckGlyph")?.Opacity, "a checked radio shows a checkmark too");
			Assert.IsNotNull(FindDescendant<FontIcon>(sub, "SubItemChevron"), "a sub-item shows its chevron");
		}
		finally
		{
			menu.Hide();
			UnitTestsUIContentHelper.Content = null;
		}
	}

	[TestMethod]
	[RunsOnUIThread]
	public async Task When_ComboBoxOpened_Then_ItsPopupIsTheSameGlassMenu()
	{
		var container = CreateThemedContainer();
		var comboBox = new ComboBox { ItemsSource = new[] { "One", "Two", "Three" }, Style = Resource<Style>(container, "ComboBoxStyle") };

		try
		{
			await Load(container, comboBox);
			comboBox.IsDropDownOpen = true;
			await UnitTestsUIContentHelper.WaitForIdle();

			var popup = OpenPresenter<Grid>(comboBox);
			AssertPopoverGlass(container, popup, "combo box popup");

			var row = FindDescendant<ComboBoxItem>(popup) ?? throw new AssertFailedException("no realized item");
			Assert.AreEqual(44d, row.ActualHeight, "a row is 44 high");
		}
		finally
		{
			comboBox.IsDropDownOpen = false;
			UnitTestsUIContentHelper.Content = null;
		}
	}
}
