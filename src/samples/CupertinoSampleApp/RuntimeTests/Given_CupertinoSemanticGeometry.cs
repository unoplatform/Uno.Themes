using System.Linq;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Uno.Cupertino;
using Uno.UI.RuntimeTests;
using Windows.Foundation;

namespace Uno.Themes.Samples.RuntimeTests;

[TestClass]
public class Given_CupertinoSemanticGeometry
{
	[TestMethod]
	[DataRow(ElementTheme.Light)]
	[DataRow(ElementTheme.Dark)]
	[RunsOnUIThread]
	public async Task When_ScalarTokensChange_Then_RenderedGeometryFollowsAfterThemeRefresh(ElementTheme appearance)
	{
		var theme = (CupertinoTheme)Application.Current.GetTheme();
		var originalSpacing = theme.DefaultSpacing;
		var originalRadius = theme.DefaultCornerRadius;
		var host = new Grid { RequestedTheme = appearance, Width = 600, Height = 600 };
		var content = new StackPanel();
		var row = new ListViewItem { Content = "Row" };
		var group = new ListView();
		var field = new TextBox { Text = "Field" };
		var popup = new FlyoutPresenter { Content = "Popover" };
		var button = new Button { Content = "Action" };
		var check = new CheckBox { Content = "Check" };
		content.Children.Add(row);
		content.Children.Add(group);
		content.Children.Add(field);
		content.Children.Add(popup);
		content.Children.Add(button);
		content.Children.Add(check);
		host.Children.Add(content);
		try
		{
			UnitTestsUIContentHelper.Content = host;
			await UnitTestsUIContentHelper.WaitForLoaded(content);
			await UnitTestsUIContentHelper.WaitForIdle();
			Assert.AreEqual(new Thickness(16, 0, 16, 0), row.Padding);
			Assert.AreEqual(new CornerRadius(26), group.CornerRadius);
			Assert.AreEqual(new CornerRadius(10), field.CornerRadius);
			Assert.AreEqual(new CornerRadius(26), popup.CornerRadius);
			Assert.AreEqual(new Thickness(16, 7, 16, 7), button.Padding);
			Assert.AreEqual(44d, check.MinHeight, "Default iOS hit target must remain 44");
			Assert.AreEqual(44d, Application.Current.Resources["TouchTargetMinSize"]);

			theme.DefaultSpacing = 8;
			theme.DefaultCornerRadius = 8;
			host.RequestedTheme = appearance == ElementTheme.Light ? ElementTheme.Dark : ElementTheme.Light;
			await UnitTestsUIContentHelper.WaitForIdle();
			host.RequestedTheme = appearance;
			await UnitTestsUIContentHelper.WaitForIdle();

			Assert.AreEqual(new Thickness(32, 0, 32, 0), row.Padding, "Spacing must reach the rendered row");
			Assert.AreEqual(new CornerRadius(52), group.CornerRadius, "Shape must reach grouped lists");
			Assert.AreEqual(new CornerRadius(20), field.CornerRadius, "Shape must reach fields");
			Assert.AreEqual(new CornerRadius(52), popup.CornerRadius, "Shape must reach popovers");
			Assert.AreEqual(new Thickness(32, 14, 32, 14), button.Padding, "Composite padding must follow spacing");
		}
		finally
		{
			theme.DefaultSpacing = originalSpacing;
			theme.DefaultCornerRadius = originalRadius;
			UnitTestsUIContentHelper.Content = null;
		}
	}

	[TestMethod]
	[DataRow(ElementTheme.Light, false)]
	[DataRow(ElementTheme.Dark, false)]
	[DataRow(ElementTheme.Light, true)]
	[DataRow(ElementTheme.Dark, true)]
	[RunsOnUIThread]
	public async Task When_GeometryTokensAreOverridden_Then_ControlAliasesHonorPrecedence(ElementTheme appearance, bool overrideControlKey)
	{
		var host = new Grid { RequestedTheme = appearance, Width = 600, Height = 600 };
		host.Resources["Radius100CornerRadius"] = new CornerRadius(15);
		var resources = Application.Current.Resources;
		var hadSpacing = resources.Keys.Contains("Space400HorizontalThickness");
		var originalSpacing = hadSpacing ? resources["Space400HorizontalThickness"] : null;
		var hadRadius = resources.Keys.Contains("Radius650CornerRadius");
		var originalRadius = hadRadius ? resources["Radius650CornerRadius"] : null;
		resources["Radius650CornerRadius"] = new CornerRadius(19);
		var hadHeight = resources.Keys.Contains("ControlHeightMediumLarge");
		var originalHeight = hadHeight ? resources["ControlHeightMediumLarge"] : null;
		resources["Space400HorizontalThickness"] = new Thickness(31, 0, 31, 0);
		resources["ControlHeightMediumLarge"] = 64d;
		if (overrideControlKey)
		{
			host.Resources["CupertinoRowPadding"] = new Thickness(23, 1, 23, 1);
		}
		var row = new ListViewItem { Content = "Row" };
		host.Children.Add(row);
		var group = new ListView();
		host.Children.Add(group);
		var calendar = new CalendarView();
		host.Children.Add(calendar);
		try
		{
			UnitTestsUIContentHelper.Content = host;
			await UnitTestsUIContentHelper.WaitForLoaded(row);
			await UnitTestsUIContentHelper.WaitForIdle();
			Assert.AreEqual(overrideControlKey ? new Thickness(23, 1, 23, 1) : new Thickness(31, 0, 31, 0), row.Padding);
			Assert.AreEqual(new CornerRadius(19), group.CornerRadius);
			Assert.AreEqual(new CornerRadius(15), calendar.CornerRadius, "Direct shared shape override reaches calendar outer shell");
			Assert.AreEqual(64d, row.MinHeight, "Control height token must reach the row independently of spacing");
		}
		finally
		{
			resources.Remove("Space400HorizontalThickness");
			resources.Remove("ControlHeightMediumLarge");
			resources.Remove("Radius650CornerRadius");
			if (hadRadius)
			{
				resources["Radius650CornerRadius"] = originalRadius;
			}
			if (hadSpacing)
			{
				resources["Space400HorizontalThickness"] = originalSpacing;
			}
			if (hadHeight)
			{
				resources["ControlHeightMediumLarge"] = originalHeight;
			}
			UnitTestsUIContentHelper.Content = null;
		}
	}

	[TestMethod]
	[DataRow(ElementTheme.Light)]
	[DataRow(ElementTheme.Dark)]
	[RunsOnUIThread]
	public async Task When_DensityIsCompact_Then_OnlySpacingShrinks(ElementTheme appearance)
	{
		var theme = (CupertinoTheme)Application.Current.GetTheme();
		var originalDensity = theme.DefaultDensity;
		theme.DefaultDensity = Density.Compact;
		var host = new Grid { RequestedTheme = appearance, Width = 600, Height = 600 };
		var row = new ListViewItem { Content = "Row" };
		host.Children.Add(row);
		try
		{
			UnitTestsUIContentHelper.Content = host;
			await UnitTestsUIContentHelper.WaitForLoaded(row);
			await UnitTestsUIContentHelper.WaitForIdle();
			Assert.AreEqual(new Thickness(12, 0, 12, 0), row.Padding);
			Assert.AreEqual(44d, row.MinHeight, "Density must preserve the iOS touch geometry");
		}
		finally
		{
			theme.DefaultDensity = originalDensity;
			UnitTestsUIContentHelper.Content = null;
		}
	}

	[TestMethod]
	[DataRow(ElementTheme.Light)]
	[DataRow(ElementTheme.Dark)]
	[RunsOnUIThread]
	public async Task When_TouchTargetIsOverridden_Then_SelectionHitAreaFollows(ElementTheme appearance)
	{
		var host = new Grid { RequestedTheme = appearance };
		host.Resources["TouchTargetMinSize"] = 68d;
		var check = new CheckBox { Content = "Check" };
		host.Children.Add(check);
		var slider = new Slider();
		var vertical = new Slider { Orientation = Orientation.Vertical };
		var toggle = new ToggleSwitch();
		host.Children.Add(slider);
		host.Children.Add(vertical);
		host.Children.Add(toggle);
		try
		{
			UnitTestsUIContentHelper.Content = host;
			await UnitTestsUIContentHelper.WaitForLoaded(check);
			await UnitTestsUIContentHelper.WaitForIdle();
			Assert.AreEqual(68d, check.MinWidth);
			Assert.AreEqual(68d, check.MinHeight);
			Assert.AreEqual(68d, FindElement<Grid>(slider, "HorizontalTemplate").MinHeight);
			Assert.AreEqual(68d, FindElement<Grid>(vertical, "VerticalTemplate").MinWidth);
			Assert.AreEqual(68d, toggle.MinHeight);
			Assert.AreEqual(64d, FindElement<FrameworkElement>(toggle, "OuterBorder").Width);
			Assert.AreEqual(28d, FindElement<FrameworkElement>(toggle, "OuterBorder").Height);
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
	public async Task When_AlertSpacingTokenChanges_Then_ActionGapFollows(ElementTheme appearance)
	{
		var theme = (CupertinoTheme)Application.Current.GetTheme();
		var originalSpacing = theme.DefaultSpacing;
		var host = new Grid { RequestedTheme = appearance, Width = 600, Height = 600 };
		var dialog = new ContentDialog
		{
			Title = "Save?",
			Content = "Keep changes.",
			PrimaryButtonText = "Yes",
			SecondaryButtonText = "No",
			DefaultButton = ContentDialogButton.Primary,
		};
		IAsyncOperation<ContentDialogResult>? showing = null;
		try
		{
			theme.DefaultSpacing = 8;
			UnitTestsUIContentHelper.Content = host;
			await UnitTestsUIContentHelper.WaitForLoaded(host);
			dialog.XamlRoot = host.XamlRoot;
			showing = dialog.ShowAsync();
			await UnitTestsUIContentHelper.WaitForIdle();
			var primary = FindElement<Button>(dialog, "PrimaryButton");
			var secondary = FindElement<Button>(dialog, "SecondaryButton");
			var first = secondary.TransformToVisual(dialog).TransformPoint(new Point());
			var second = primary.TransformToVisual(dialog).TransformPoint(new Point());
			Assert.AreEqual(first.Y, second.Y, 0.5);
			Assert.AreEqual(16d, second.X - first.X - secondary.ActualWidth, 0.5, "Action gap follows Space200");
		}
		finally
		{
			dialog.Hide();
			if (showing is not null)
			{
				await showing;
			}
			theme.DefaultSpacing = originalSpacing;
			UnitTestsUIContentHelper.Content = null;
		}
	}

	private static T FindElement<T>(DependencyObject root, string name) where T : FrameworkElement
	{
		T? Find(DependencyObject current)
		{
			for (var i = 0; i < VisualTreeHelper.GetChildrenCount(current); i++)
			{
				var child = VisualTreeHelper.GetChild(current, i);
				if (child is T element && element.Name == name)
				{
					return element;
				}
				if (Find(child) is { } descendant)
				{
					return descendant;
				}
			}
			return null;
		}
		return Find(root) ?? throw new AssertFailedException($"Missing {typeof(T).Name} {name}");
	}

	[TestMethod]
	[DataRow(ElementTheme.Light)]
	[DataRow(ElementTheme.Dark)]
	[RunsOnUIThread]
	public async Task When_IconTokenIsOverridden_Then_MenuGutterFollows(ElementTheme appearance)
	{
		var host = new Grid { RequestedTheme = appearance };
		host.Resources["IconSizeMedium"] = 36d;
		var item = new MenuFlyoutItem { Text = "Copy" };
		host.Children.Add(item);
		try
		{
			UnitTestsUIContentHelper.Content = host;
			await UnitTestsUIContentHelper.WaitForLoaded(item);
			await UnitTestsUIContentHelper.WaitForIdle();
			Assert.AreEqual(36d, FindElement<Border>(item, "CheckGutter").Width);
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
	public async Task When_RadiusIsZeroAndReset_Then_FieldShapeFollows(ElementTheme appearance)
	{
		var theme = (CupertinoTheme)Application.Current.GetTheme();
		var originalRadius = theme.DefaultCornerRadius;
		var host = new Grid { RequestedTheme = appearance };
		var field = new TextBox { Text = "Field" };
		host.Children.Add(field);
		try
		{
			theme.DefaultCornerRadius = 0;
			UnitTestsUIContentHelper.Content = host;
			await UnitTestsUIContentHelper.WaitForLoaded(field);
			await UnitTestsUIContentHelper.WaitForIdle();
			Assert.AreEqual(new CornerRadius(0), field.CornerRadius);
			theme.DefaultCornerRadius = 4;
			host.RequestedTheme = appearance == ElementTheme.Light ? ElementTheme.Dark : ElementTheme.Light;
			await UnitTestsUIContentHelper.WaitForIdle();
			host.RequestedTheme = appearance;
			await UnitTestsUIContentHelper.WaitForIdle();
			Assert.AreEqual(new CornerRadius(10), field.CornerRadius);
		}
		finally
		{
			theme.DefaultCornerRadius = originalRadius;
			UnitTestsUIContentHelper.Content = null;
		}
	}

	// Parts the 2026-09-24 review restyled: the NumberBox padding, the menu group band and the progress
	// track must follow the spacing, radius and progress-bar keys rather than literals.
	[TestMethod]
	[RunsOnUIThread]
	public async Task When_ReviewedPartTokensOverridden_Then_TheyFollow()
	{
		var resources = Application.Current.Resources;
		var overrides = new (string Key, object Value)[]
		{
			("Space175HorizontalThickness", new Thickness(13, 0, 13, 0)),
			("Space200", 11d),
			("CupertinoProgressBarHeight", 6d),
			("Radius050CornerRadius", new CornerRadius(3)),
		};
		var saved = overrides.Select(o => (o.Key, Had: resources.Keys.Contains(o.Key), Value: resources.Keys.Contains(o.Key) ? resources[o.Key] : null)).ToArray();
		foreach (var (key, value) in overrides)
		{
			resources[key] = value;
		}

		var host = new StackPanel { Width = 400 };
		var number = new NumberBox { Value = 1 };
		var bar = new ProgressBar { Value = 50, Width = 300 };
		var anchor = new Button { Content = "Anchor" };
		host.Children.Add(number);
		host.Children.Add(bar);
		host.Children.Add(anchor);
		var separator = new MenuFlyoutSeparator();
		var menu = new MenuFlyout();
		menu.Items.Add(new MenuFlyoutItem { Text = "Copy" });
		menu.Items.Add(separator);
		menu.Items.Add(new MenuFlyoutItem { Text = "Delete" });
		try
		{
			UnitTestsUIContentHelper.Content = host;
			await UnitTestsUIContentHelper.WaitForLoaded(bar);
			await UnitTestsUIContentHelper.WaitForIdle();
			menu.ShowAt(anchor);
			await UnitTestsUIContentHelper.WaitForIdle();

			Assert.AreEqual(new Thickness(13, 0, 13, 0), number.Padding, "NumberBox padding follows Space175");
			Assert.AreEqual(11d, Descendant<Microsoft.UI.Xaml.Shapes.Rectangle>(separator).ActualHeight, "menu band follows Space200");
			var track = Descendant<Microsoft.UI.Xaml.Shapes.Rectangle>(bar, "ProgressBarTrack");
			Assert.AreEqual(6d, track.ActualHeight, "track height follows CupertinoProgressBarHeight");
			Assert.AreEqual(3d, track.RadiusX, "track radius follows Radius050");
		}
		finally
		{
			menu.Hide();
			foreach (var (key, had, value) in saved)
			{
				resources.Remove(key);
				if (had)
				{
					resources[key] = value;
				}
			}

			UnitTestsUIContentHelper.Content = null;
		}
	}

	private static T Descendant<T>(DependencyObject root, string? name = null)
		where T : FrameworkElement
	{
		for (var i = 0; i < VisualTreeHelper.GetChildrenCount(root); i++)
		{
			var child = VisualTreeHelper.GetChild(root, i);
			if (child is T match && (name is null || match.Name == name))
			{
				return match;
			}

			if (FindOrNull<T>(child, name) is { } nested)
			{
				return nested;
			}
		}

		throw new AssertFailedException($"Missing {typeof(T).Name} {name}");
	}

	private static T? FindOrNull<T>(DependencyObject root, string? name)
		where T : FrameworkElement
	{
		for (var i = 0; i < VisualTreeHelper.GetChildrenCount(root); i++)
		{
			var child = VisualTreeHelper.GetChild(root, i);
			if (child is T match && (name is null || match.Name == name))
			{
				return match;
			}

			if (FindOrNull<T>(child, name) is { } nested)
			{
				return nested;
			}
		}

		return null;
	}
}
