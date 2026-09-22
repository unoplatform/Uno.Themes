using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Uno.Cupertino;
using Uno.UI.RuntimeTests;

namespace Uno.Themes.Samples.RuntimeTests;

/// <summary>Exercises Cupertino commands in a toolbar and in its stock overflow popup.</summary>
[TestClass]
public class Given_CupertinoCommandBar
{
	private static T Resource<T>(FrameworkElement scope, string key)
		=> scope.Resources.TryGetValue(key, out var value) && value is T typed
			? typed
			: throw new AssertFailedException($"Resource '{key}' not found or not a {typeof(T).Name}");

	private static T Find<T>(DependencyObject root, string name) where T : FrameworkElement
		=> FindOrDefault<T>(root, name) ?? throw new AssertFailedException($"Missing {name}");

	private static T? FindOrDefault<T>(DependencyObject root, string name) where T : FrameworkElement
	{
		for (var i = 0; i < VisualTreeHelper.GetChildrenCount(root); i++)
		{
			var child = VisualTreeHelper.GetChild(root, i);
			if (child is T match && match.Name == name)
			{
				return match;
			}

			if (FindOrDefault<T>(child, name) is { } nested)
			{
				return nested;
			}
		}

		return null;
	}

	private static Grid Container(ElementTheme appearance)
	{
		var container = new Grid { Width = 500, RequestedTheme = appearance };
		container.Resources.MergedDictionaries.Add(new CupertinoTheme());
		return container;
	}

	[TestMethod]
	[DataRow(ElementTheme.Light)]
	[DataRow(ElementTheme.Dark)]
	[RunsOnUIThread]
	public async Task When_AppBarButtonLoaded_Then_LabelIconAndInteractionStatesAreCupertino(ElementTheme appearance)
	{
		var container = Container(appearance);
		var foreground = (TextBlock)Microsoft.UI.Xaml.Markup.XamlReader.Load(
			"""<TextBlock xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation" Foreground="{ThemeResource OnSurfaceBrush}" />""");
		container.Children.Add(foreground);
		var button = new AppBarButton
		{
			Label = "Add",
			Icon = new SymbolIcon(Symbol.Add),
			Style = Resource<Style>(container, "AppBarButtonStyle"),
			HorizontalAlignment = HorizontalAlignment.Left,
			VerticalAlignment = VerticalAlignment.Top,
		};
		try
		{
			container.Children.Add(button);
			UnitTestsUIContentHelper.Content = container;
			await UnitTestsUIContentHelper.WaitForLoaded(button);
			await UnitTestsUIContentHelper.WaitForIdle();

			Assert.AreEqual("Add", Find<TextBlock>(button, "TextLabel").Text);
			Assert.AreEqual(22d, Find<Viewbox>(button, "ContentViewbox").Width);
			Assert.IsTrue(button.ActualWidth >= 44 && button.ActualHeight >= 44);
			Assert.AreEqual(((SolidColorBrush)foreground.Foreground).Color, ((SolidColorBrush)button.Foreground).Color);
			var root = Find<Grid>(button, "Root");
			foreach (var (state, opacity) in new[] { ("PointerOver", 0.85), ("Pressed", 0.6), ("Normal", 1d) })
			{
				Assert.IsTrue(VisualStateManager.GoToState(button, state, false));
				Assert.AreEqual(opacity, root.Opacity, 0.001, state);
			}

			Assert.IsTrue(VisualStateManager.GoToState(button, "LabelOnRight", false));
			await UnitTestsUIContentHelper.WaitForIdle();
			var icon = Find<Viewbox>(button, "ContentViewbox");
			var text = Find<TextBlock>(button, "TextLabel");
			Assert.IsTrue(text.TransformToVisual(button).TransformPoint(new Windows.Foundation.Point()).X >= icon.TransformToVisual(button).TransformPoint(new Windows.Foundation.Point()).X + icon.ActualWidth, "LabelOnRight places the label after the icon");
			Assert.IsTrue(VisualStateManager.GoToState(button, "LabelCollapsed", false));
			Assert.AreEqual(Visibility.Collapsed, Find<TextBlock>(button, "TextLabel").Visibility);
			Assert.IsTrue(VisualStateManager.GoToState(button, "FullSize", false));
			await UnitTestsUIContentHelper.WaitForIdle();
			Assert.IsTrue(text.TransformToVisual(button).TransformPoint(new Windows.Foundation.Point()).Y >= icon.TransformToVisual(button).TransformPoint(new Windows.Foundation.Point()).Y + icon.ActualHeight, "FullSize places the label below the icon");

			button.IsCompact = true;
			await UnitTestsUIContentHelper.WaitForIdle();
			Assert.AreEqual(Visibility.Collapsed, Find<TextBlock>(button, "TextLabel").Visibility);
			button.IsCompact = false;
			await UnitTestsUIContentHelper.WaitForIdle();
			Assert.AreEqual(Visibility.Visible, Find<TextBlock>(button, "TextLabel").Visibility);
			button.ClearValue(AppBarButton.IconProperty);
			button.Label = "Done";
			await UnitTestsUIContentHelper.WaitForIdle();
			Assert.AreEqual(Visibility.Collapsed, Find<Viewbox>(button, "ContentViewbox").Visibility);
			Assert.AreEqual("Done", Find<TextBlock>(button, "TextLabel").Text);
			button.IsEnabled = false;
			await UnitTestsUIContentHelper.WaitForIdle();
			Assert.AreEqual(0.5, root.Opacity, 0.001);
		}
		finally
		{
			UnitTestsUIContentHelper.Content = null;
		}
	}

	[TestMethod]
	[DataRow(ElementTheme.Light)]
	[DataRow(ElementTheme.Dark)]
	[RunsOnUIThread]
	public async Task When_CommandBarOpened_Then_PrimaryAndOverflowCommandsRemainUsable(ElementTheme appearance)
	{
		var container = Container(appearance);
		// A theme-bound probe resolves in the same visual appearance as the bar; direct dictionary
		// lookup follows the application's appearance rather than this container's RequestedTheme.
		var surface = (Border)Microsoft.UI.Xaml.Markup.XamlReader.Load(
			"""<Border xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation" Background="{ThemeResource SurfaceBrush}" />""");
		container.Children.Add(surface);
		var bar = new CommandBar
		{
			Style = Resource<Style>(container, "CommandBarStyle"),
			VerticalAlignment = VerticalAlignment.Top,
		};
		var primary = new AppBarButton { Label = "Add", Icon = new SymbolIcon(Symbol.Add), Style = Resource<Style>(container, "AppBarButtonStyle") };
		var secondary = new AppBarButton { Label = "Delete", Icon = new SymbolIcon(Symbol.Delete), Style = primary.Style };
		bar.PrimaryCommands.Add(primary);
		bar.SecondaryCommands.Add(secondary);
		try
		{
			container.Children.Add(bar);
			UnitTestsUIContentHelper.Content = container;
			await UnitTestsUIContentHelper.WaitForLoaded(bar);
			await UnitTestsUIContentHelper.WaitForIdle();
			Assert.AreEqual(((SolidColorBrush)surface.Background).Color, ((SolidColorBrush)bar.Background).Color);
			Assert.AreEqual(44d, Find<Grid>(bar, "ContentRoot").MinHeight);
			Assert.AreEqual(Visibility.Collapsed, Find<TextBlock>(primary, "TextLabel").Visibility);

			bar.IsOpen = true;
			await UnitTestsUIContentHelper.WaitForIdle();
			Assert.AreEqual("Delete", Find<TextBlock>(secondary, "TextLabel").Text);
			Assert.AreEqual(Visibility.Visible, Find<TextBlock>(secondary, "TextLabel").Visibility);
			var overflowIcon = Find<Viewbox>(secondary, "ContentViewbox");
			var overflowLabel = Find<TextBlock>(secondary, "TextLabel");
			Assert.IsTrue(overflowIcon.TransformToVisual(secondary).TransformPoint(new Windows.Foundation.Point()).X >= overflowLabel.TransformToVisual(secondary).TransformPoint(new Windows.Foundation.Point()).X + overflowLabel.ActualWidth, "Overflow presents label and icon in a horizontal row");
			Assert.IsTrue(secondary.ActualHeight >= 44);
			var label = Find<TextBlock>(secondary, "TextLabel");
			var naturalLabel = new TextBlock { Text = secondary.Label, FontFamily = label.FontFamily, FontSize = label.FontSize, FontWeight = label.FontWeight };
			naturalLabel.Measure(new Windows.Foundation.Size(double.PositiveInfinity, double.PositiveInfinity));
			var labelOrigin = label.TransformToVisual(secondary).TransformPoint(new Windows.Foundation.Point());
			Assert.IsTrue(labelOrigin.X + naturalLabel.DesiredSize.Width <= secondary.ActualWidth, "The complete overflow label must fit inside its command");
			Assert.IsTrue(VisualTreeHelper.GetOpenPopupsForXamlRoot(bar.XamlRoot).Count > 0, "Overflow popup must open");
		}
		finally
		{
			bar.IsOpen = false;
			UnitTestsUIContentHelper.Content = null;
		}
	}

	[TestMethod]
	[DataRow(false)]
	[DataRow(true)]
	[RunsOnUIThread]
	public async Task When_OverflowCommandHasAffordance_Then_ShortcutOrFlyoutIndicatorIsVisible(bool hasFlyout)
	{
		var container = Container(ElementTheme.Light);
		var bar = new CommandBar { Style = Resource<Style>(container, "CommandBarStyle"), VerticalAlignment = VerticalAlignment.Top };
		var command = new AppBarButton
		{
			Label = hasFlyout ? "More actions" : "Copy",
			Style = Resource<Style>(container, "AppBarButtonStyle"),
		};
		if (hasFlyout)
		{
			var flyout = new MenuFlyout();
			flyout.Items.Add(new MenuFlyoutItem { Text = "Details" });
			command.Flyout = flyout;
		}
		else
		{
			command.KeyboardAcceleratorTextOverride = "Ctrl+C";
		}
		bar.SecondaryCommands.Add(command);
		container.Children.Add(bar);
		try
		{
			UnitTestsUIContentHelper.Content = container;
			await UnitTestsUIContentHelper.WaitForLoaded(bar);
			bar.IsOpen = true;
			await UnitTestsUIContentHelper.WaitForIdle();
			var label = Find<TextBlock>(command, "TextLabel");
			Assert.IsTrue(label.ActualWidth > 0);
			if (hasFlyout)
			{
				var chevron = Find<FontIcon>(command, "SubItemChevron");
				Assert.AreEqual(Visibility.Visible, chevron.Visibility);
				Assert.IsTrue(chevron.ActualWidth > 0 && chevron.ActualHeight > 0);
				Assert.IsTrue(VisualStateManager.GoToState(command, "OverflowPointerOver", false));
				Assert.AreEqual(0.85, Find<Grid>(command, "Root").Opacity, 0.001);
			}
			else
			{
				var shortcut = Find<TextBlock>(command, "KeyboardAcceleratorTextLabel");
				Assert.AreEqual("Ctrl+C", shortcut.Text);
				Assert.AreEqual(Visibility.Visible, shortcut.Visibility);
				Assert.IsTrue(shortcut.ActualWidth > 0 && shortcut.ActualHeight > 0);
			}
		}
		finally
		{
			bar.IsOpen = false;
			UnitTestsUIContentHelper.Content = null;
		}
	}
}
