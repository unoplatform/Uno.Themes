using System.Threading.Tasks;
using Microsoft.UI.Xaml.Shapes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Uno.Cupertino;
using Uno.UI.RuntimeTests;
using Windows.Foundation;
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
	public async Task When_ListViewRendered_Then_ItIsAnInsetGroupOf44PxRows()
	{
		var container = CreateThemedContainer();
		var listView = new ListView { Style = Resource<Style>(container, "ListViewStyle"), Width = 300 };
		listView.Items.Add(new ListViewItem { Content = "First" });
		listView.Items.Add(new ListViewItem { Content = "Second", IsSelected = true });
		listView.Items.Add(new ListViewItem { Content = "Third", IsEnabled = false });

		try
		{
			await Load(container, listView);

			var group = FindDescendant<Border>(listView, "Group") ?? throw new AssertFailedException("the group border is missing");
			Assert.AreEqual(26d, group.CornerRadius.TopLeft, "the inset-grouped radius");
			Assert.AreEqual(ColorOf(container, "SurfaceBrush"), ColorOf(group.Background), "the group is a surface");

			var rows = new[] { 0, 1, 2 }.Select(i => (ListViewItem)listView.ContainerFromIndex(i)).ToArray();
			foreach (var row in rows)
			{
				Assert.IsTrue(row.ActualHeight >= 44, $"a row is 44 high, was {row.ActualHeight}");
				Assert.AreEqual(ColorOf(container, "OnSurfaceBrush"), ColorOf(row.Foreground), "label colour");
				var separator = FindDescendant<Rectangle>(row, "Separator") ?? throw new AssertFailedException("no hairline");
				Assert.AreEqual(0.5, separator.ActualHeight, "a hairline");
				Assert.AreEqual(16d, separator.Margin.Left, "inset from the leading edge");
			}

			var selected = FindDescendant<Grid>(rows[1], "ContentBorder") ?? throw new AssertFailedException("template root missing");
			Assert.AreEqual(ColorOf(container, "PrimarySelectedBrush"), ColorOf(selected.Background), "a selected row takes the selection tint");
			Assert.AreEqual(0.5, FindDescendant<Grid>(rows[2], "ContentBorder")?.Opacity, "a disabled row dims");
		}
		finally
		{
			UnitTestsUIContentHelper.Content = null;
		}
	}

	[TestMethod]
	[RunsOnUIThread]
	public async Task When_ContentDialogShown_Then_ItIsAnAlertOnThickGlass()
	{
		var container = CreateThemedContainer();
		var dialog = new ContentDialog
		{
			Title = "Delete note?",
			Content = "This cannot be undone.",
			PrimaryButtonText = "Delete",
			CloseButtonText = "Cancel",
			DefaultButton = ContentDialogButton.Primary,
			Style = Resource<Style>(container, "ContentDialogStyle"),
		};
		IAsyncOperation<ContentDialogResult>? showing = null;

		try
		{
			var anchor = await Load(container, new Button { Content = "Anchor" });
			dialog.XamlRoot = anchor.XamlRoot;
			showing = dialog.ShowAsync();
			await UnitTestsUIContentHelper.WaitForIdle();

			var glass = FindDescendant<GlassPanel>(dialog) ?? throw new AssertFailedException("no glass behind the alert");
			Assert.AreEqual(GlassMaterial.Thick, glass.Material);
			Assert.AreEqual(30d, glass.CornerRadius.TopLeft, "the alert radius");
			Assert.AreEqual(ColorOf(container, "CupertinoDialogTintBrush"), ColorOf(glass.Tint), "the alert tint");
			// Uno dims through the popup's light-dismiss overlay (an internal property), painted from this resource.
			Assert.AreEqual(Color.FromArgb(0x59, 0, 0, 0), ColorOf(container, "ContentDialogLightDismissOverlayBackground"), "the dimming layer");

			var primary = FindDescendant<Button>(dialog, "PrimaryButton") ?? throw new AssertFailedException("no primary button");
			var close = FindDescendant<Button>(dialog, "CloseButton") ?? throw new AssertFailedException("no close button");
			Assert.IsTrue(primary.ActualHeight >= 44, $"an action row is 44 high, was {primary.ActualHeight}");
			Assert.IsTrue(glass.ActualWidth >= 270, $"an alert is at least 270 wide, was {glass.ActualWidth}");
			Assert.AreEqual(primary.ActualWidth, close.ActualWidth, "two capsule actions share the available width");
			Assert.AreEqual(Microsoft.UI.Text.FontWeights.SemiBold.Weight, primary.FontWeight.Weight, "the default action is bold");
			Assert.AreEqual(Microsoft.UI.Text.FontWeights.Normal.Weight, close.FontWeight.Weight, "the other actions are not");
			Assert.AreEqual(ColorOf(container, "OnSurfaceBrush"), ColorOf(close.Foreground), "secondary actions use label text");
			Assert.IsTrue(primary.CornerRadius.TopLeft >= 22 && close.CornerRadius.TopLeft >= 22, "actions have capsule corners");
			Assert.IsTrue(primary.TransformToVisual(dialog).TransformPoint(new Point()).X > close.TransformToVisual(dialog).TransformPoint(new Point()).X, "Cancel leads and the default action trails");
			Assert.AreEqual(Visibility.Collapsed, FindDescendant<Button>(dialog, "SecondaryButton")?.Visibility, "no secondary action");
		}
		finally
		{
			dialog.Hide();
			if (showing is not null)
			{
				await showing;
			}

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
