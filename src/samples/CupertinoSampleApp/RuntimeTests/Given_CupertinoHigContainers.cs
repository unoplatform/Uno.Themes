using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Uno.Cupertino;
using Uno.UI.RuntimeTests;
using Windows.Foundation;

namespace Uno.Themes.Samples.RuntimeTests;

/// <summary>Rendered contracts taken from the current iOS and iPadOS HIG artwork.</summary>
[TestClass]
public class Given_CupertinoHigContainers
{
	private static Grid Host()
	{
		var host = new Grid { Width = 600, Height = 500 };
		host.Resources.MergedDictionaries.Add(new CupertinoTheme());
		return host;
	}

	private static T Find<T>(DependencyObject root, string? name = null) where T : FrameworkElement
		=> FindOrDefault<T>(root, name) ?? throw new AssertFailedException($"Missing {typeof(T).Name} {name}");

	private static T? FindOrDefault<T>(DependencyObject root, string? name = null) where T : FrameworkElement
	{
		for (var i = 0; i < VisualTreeHelper.GetChildrenCount(root); i++)
		{
			var child = VisualTreeHelper.GetChild(root, i);
			if (child is T match && (name is null || match.Name == name))
			{
				return match;
			}

			if (FindOrDefault<T>(child, name) is { } descendant)
			{
				return descendant;
			}
		}

		return null;
	}

	[TestMethod]
	[DataRow(false)]
	[DataRow(true)]
	[RunsOnUIThread]
	public async Task When_ToolbarMoreIsShown_Then_EllipsisIsOneSymbol(bool opened)
	{
		var host = Host();
		var bar = new CommandBar { VerticalAlignment = VerticalAlignment.Top };
		bar.SecondaryCommands.Add(new AppBarButton { Label = "Delete", Icon = new SymbolIcon(Symbol.Delete) });
		host.Children.Add(bar);
		try
		{
			UnitTestsUIContentHelper.Content = host;
			await UnitTestsUIContentHelper.WaitForLoaded(bar);
			bar.IsOpen = opened;
			await UnitTestsUIContentHelper.WaitForIdle();
			Assert.AreEqual("\uE10C", Find<FontIcon>(bar, "EllipsisIcon").Glyph, "More must render one ellipsis symbol without encoding corruption");
		}
		finally
		{
			bar.IsOpen = false;
			UnitTestsUIContentHelper.Content = null;
		}
	}

	[TestMethod]
	[RunsOnUIThread]
	public async Task When_OverflowFontSizeIsOverridden_Then_LabelHonorsConsumerValue()
	{
		var host = Host();
		var bar = new CommandBar { VerticalAlignment = VerticalAlignment.Top };
		var command = new AppBarButton { Label = "Copy", FontSize = 24 };
		bar.SecondaryCommands.Add(command);
		host.Children.Add(bar);
		try
		{
			UnitTestsUIContentHelper.Content = host;
			await UnitTestsUIContentHelper.WaitForLoaded(bar);
			bar.IsOpen = true;
			await UnitTestsUIContentHelper.WaitForIdle();
			Assert.AreEqual(24d, Find<TextBlock>(command, "TextLabel").FontSize);
			command.FontSize = 20;
			await UnitTestsUIContentHelper.WaitForIdle();
			Assert.AreEqual(20d, Find<TextBlock>(command, "TextLabel").FontSize);
		}
		finally
		{
			bar.IsOpen = false;
			UnitTestsUIContentHelper.Content = null;
		}
	}

	[TestMethod]
	[DataRow(ElementTheme.Light)]
	[DataRow(ElementTheme.Dark)]
	[RunsOnUIThread]
	public async Task When_ToolbarOverflowHasIcon_Then_MatchesIosMenuTypographyAndTrailingSymbol(ElementTheme appearance)
	{
		var host = Host();
		host.RequestedTheme = appearance;
		var bar = new CommandBar { VerticalAlignment = VerticalAlignment.Top };
		var command = new AppBarButton { Label = "Copy", Icon = new SymbolIcon(Symbol.Copy), KeyboardAcceleratorTextOverride = "Ctrl+C" };
		bar.SecondaryCommands.Add(command);
		host.Children.Add(bar);
		try
		{
			UnitTestsUIContentHelper.Content = host;
			await UnitTestsUIContentHelper.WaitForLoaded(bar);
			bar.IsOpen = true;
			await UnitTestsUIContentHelper.WaitForIdle();
			var label = Find<TextBlock>(command, "TextLabel");
			var icon = Find<Viewbox>(command, "ContentViewbox");
			Assert.AreEqual(17d, label.FontSize, "Overflow uses the iOS menu body size");
			Assert.IsTrue(command.ActualWidth >= 250, "Overflow rows share the Cupertino menu minimum width");
			var shortcut = Find<TextBlock>(command, "KeyboardAcceleratorTextLabel");
			var shortcutPoint = shortcut.TransformToVisual(command).TransformPoint(new Point());
			// The shared accelerator column includes its leading margin in ActualWidth on Skia.
			// Measure the visible text separately, as in the existing overflow-label regression.
			var naturalShortcut = new TextBlock
			{
				Text = shortcut.Text,
				FontFamily = shortcut.FontFamily,
				FontSize = shortcut.FontSize,
				FontWeight = shortcut.FontWeight,
			};
			naturalShortcut.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
			Assert.AreEqual(16d, Find<Grid>(command, "Root").Padding.Right);
			Assert.IsTrue(shortcutPoint.X + naturalShortcut.DesiredSize.Width <= command.ActualWidth - 16 + 0.5,
				"Shortcut text keeps the menu trailing inset clear of the rounded edge");
			var labelPoint = label.TransformToVisual(command).TransformPoint(new Point());
			var iconPoint = icon.TransformToVisual(command).TransformPoint(new Point());
			Assert.IsTrue(iconPoint.X >= labelPoint.X + label.ActualWidth + 8, "The action symbol follows the menu label with a clear gutter");
			Assert.AreEqual(Visibility.Visible, Find<TextBlock>(command, "KeyboardAcceleratorTextLabel").Visibility);
		}
		finally
		{
			bar.IsOpen = false;
			UnitTestsUIContentHelper.Content = null;
		}
	}

	[TestMethod]
	[RunsOnUIThread]
	public async Task When_CompactToolbarCommandHasNoIcon_Then_TextLabelRemainsVisible()
	{
		var host = Host();
		var command = new AppBarButton { Label = "Edit" };
		var bar = new CommandBar { VerticalAlignment = VerticalAlignment.Top };
		bar.PrimaryCommands.Add(command);
		host.Children.Add(bar);
		try
		{
			UnitTestsUIContentHelper.Content = host;
			await UnitTestsUIContentHelper.WaitForLoaded(bar);
			await UnitTestsUIContentHelper.WaitForIdle();
			Assert.IsTrue(command.IsCompact);
			var label = Find<TextBlock>(command, "TextLabel");
			Assert.AreEqual(Visibility.Visible, label.Visibility, "HIG explicitly allows text actions such as Edit when no symbol communicates the action");
			Assert.IsTrue(label.ActualWidth > 0);
		}
		finally
		{
			UnitTestsUIContentHelper.Content = null;
		}
	}

	[TestMethod]
	[RunsOnUIThread]
	public async Task When_ToolbarNarrowsAndWidens_Then_DynamicOverflowMovesCommandsBack()
	{
		var host = Host();
		var bar = new CommandBar { Width = 140, VerticalAlignment = VerticalAlignment.Top, IsDynamicOverflowEnabled = true };
		for (var i = 0; i < 6; i++)
		{
			bar.PrimaryCommands.Add(new AppBarButton { Label = $"Action {i}", Icon = new SymbolIcon(Symbol.Add) });
		}
		host.Children.Add(bar);
		try
		{
			UnitTestsUIContentHelper.Content = host;
			await UnitTestsUIContentHelper.WaitForLoaded(bar);
			await UnitTestsUIContentHelper.WaitForIdle();
			bar.IsOpen = true;
			await UnitTestsUIContentHelper.WaitForIdle();
			CommandBarOverflowPresenter? overflow = null;
			foreach (var popup in VisualTreeHelper.GetOpenPopupsForXamlRoot(bar.XamlRoot))
			{
				if (popup.Child is { } child && FindOrDefault<CommandBarOverflowPresenter>(child) is { } found)
				{
					overflow = found;
					break;
				}
			}
			Assert.IsNotNull(overflow);
			Assert.IsTrue(overflow.Items.Count > 0, "Commands that do not fit move to overflow");
			bar.IsOpen = false;
			bar.Width = 580;
			await UnitTestsUIContentHelper.WaitForIdle();
			Assert.AreEqual(0, overflow.Items.Count, "Commands return when there is room again");
		}
		finally
		{
			bar.IsOpen = false;
			UnitTestsUIContentHelper.Content = null;
		}
	}

	[TestMethod]
	[DataRow(ElementTheme.Light)]
	[DataRow(ElementTheme.Dark)]
	[RunsOnUIThread]
	public async Task When_AlertHasSecondaryCapsule_Then_FillIsDistinctFromSurface(ElementTheme appearance)
	{
		var host = Host();
		var dialog = new ContentDialog
		{
			Style = (Style)host.Resources["ContentDialogStyle"],
			Title = "Keep Changes?",
			CloseButtonText = "Cancel",
			RequestedTheme = appearance,
		};
		IAsyncOperation<ContentDialogResult>? showing = null;
		try
		{
			UnitTestsUIContentHelper.Content = host;
			await UnitTestsUIContentHelper.WaitForLoaded(host);
			dialog.XamlRoot = host.XamlRoot;
			showing = dialog.ShowAsync();
			await UnitTestsUIContentHelper.WaitForIdle();
			var button = Find<Button>(dialog, "CloseButton");
			var surface = ((SolidColorBrush)Find<GlassPanel>(dialog).Background).Color;
			var fill = ((SolidColorBrush)button.Background).Color;
			var largestChannelDifference = Math.Max(Math.Abs(fill.R - surface.R), Math.Max(Math.Abs(fill.G - surface.G), Math.Abs(fill.B - surface.B)));
			Assert.IsTrue(largestChannelDifference >= 12, $"The secondary capsule needs a visibly distinct neutral fill: {fill} over {surface}");
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
	public async Task When_AlertLabelIsLong_Then_RenderedTextStaysInsideCapsule()
	{
		var host = Host();
		var dialog = new ContentDialog
		{
			Style = (Style)host.Resources["ContentDialogStyle"],
			Title = "Save Changes?",
			PrimaryButtonText = "Save All Changes to Every Open Document Before Leaving",
			CloseButtonText = "Cancel",
		};
		IAsyncOperation<ContentDialogResult>? showing = null;
		try
		{
			UnitTestsUIContentHelper.Content = host;
			await UnitTestsUIContentHelper.WaitForLoaded(host);
			dialog.XamlRoot = host.XamlRoot;
			showing = dialog.ShowAsync();
			await UnitTestsUIContentHelper.WaitForIdle();
			var button = Find<Button>(dialog, "PrimaryButton");
			var text = Find<ContentPresenter>(button, "ContentPresenter");
			var point = text.TransformToVisual(button).TransformPoint(new Point());
			Assert.IsTrue(point.X >= 0 && point.X + text.ActualWidth <= button.ActualWidth + 0.5, $"Text {point.X}+{text.ActualWidth} must fit button {button.ActualWidth}");
			Assert.IsTrue(button.ActualHeight > 44, "A long label wraps to multiple lines");
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
	[DataRow(ContentDialogButton.Close, false, false)]
	[DataRow(ContentDialogButton.Secondary, true, false)]
	[DataRow(ContentDialogButton.Primary, false, true)]
	[RunsOnUIThread]
	public async Task When_AlertDefaultOrFlowChanges_Then_VisibleActionsKeepExpectedOrder(ContentDialogButton defaultButton, bool hiddenDefault, bool rtl)
	{
		var host = Host();
		var dialog = new ContentDialog
		{
			Style = (Style)host.Resources["ContentDialogStyle"],
			Title = "Save Changes?",
			PrimaryButtonText = "Save",
			CloseButtonText = defaultButton == ContentDialogButton.Close ? "Done" : "Cancel",
			DefaultButton = defaultButton,
			FlowDirection = rtl ? FlowDirection.RightToLeft : FlowDirection.LeftToRight,
		};
		IAsyncOperation<ContentDialogResult>? showing = null;
		try
		{
			UnitTestsUIContentHelper.Content = host;
			await UnitTestsUIContentHelper.WaitForLoaded(host);
			dialog.XamlRoot = host.XamlRoot;
			showing = dialog.ShowAsync();
			await UnitTestsUIContentHelper.WaitForIdle();
			var primary = Find<Button>(dialog, "PrimaryButton");
			var close = Find<Button>(dialog, "CloseButton");
			var preferred = defaultButton == ContentDialogButton.Close ? close : primary;
			var other = preferred == primary ? close : primary;
			var preferredX = preferred.TransformToVisual(null).TransformPoint(new Point()).X;
			var otherX = other.TransformToVisual(null).TransformPoint(new Point()).X;
			Assert.IsTrue(rtl ? preferredX < otherX : preferredX > otherX, "Default, or primary when default is hidden, occupies the trailing position");
			if (hiddenDefault)
			{
				Assert.AreEqual(Visibility.Collapsed, Find<Button>(dialog, "SecondaryButton").Visibility);
			}
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
	public async Task When_SidebarItemUnselected_Then_IconUsesAccentAndLabelUsesTextColor()
	{
		var host = Host();
		var item = new NavigationViewItem { Content = "Library", Icon = new SymbolIcon(Symbol.Library) };
		var navigation = new NavigationView { PaneDisplayMode = NavigationViewPaneDisplayMode.Left, IsPaneOpen = true };
		navigation.MenuItems.Add(item);
		host.Children.Add(navigation);
		try
		{
			UnitTestsUIContentHelper.Content = host;
			await UnitTestsUIContentHelper.WaitForLoaded(navigation);
			await UnitTestsUIContentHelper.WaitForIdle();
			var icon = Find<ContentPresenter>(item, "Icon");
			var label = Find<ContentPresenter>(item, "ContentPresenter");
			Assert.AreEqual(((SolidColorBrush)host.Resources["PrimaryBrush"]).Color, ((SolidColorBrush)icon.Foreground).Color, "Sidebar symbols use the app accent by default");
			Assert.AreEqual(((SolidColorBrush)host.Resources["OnSurfaceBrush"]).Color, ((SolidColorBrush)label.Foreground).Color);
		}
		finally
		{
			UnitTestsUIContentHelper.Content = null;
		}
	}

	[TestMethod]
	[RunsOnUIThread]
	public async Task When_MenuIncludesChecks_Then_LabelsShareLeadingGutter()
	{
		var host = Host();
		var anchor = new Button { Content = "Menu" };
		host.Children.Add(anchor);
		var plain = new MenuFlyoutItem { Text = "Copy" };
		var toggle = new ToggleMenuFlyoutItem { Text = "Show Details", IsChecked = true };
		var menu = new MenuFlyout();
		menu.Items.Add(plain);
		menu.Items.Add(toggle);
		try
		{
			UnitTestsUIContentHelper.Content = host;
			await UnitTestsUIContentHelper.WaitForLoaded(anchor);
			menu.ShowAt(anchor);
			await UnitTestsUIContentHelper.WaitForIdle();
			var plainLabel = Find<TextBlock>(plain, "TextBlock");
			var toggleLabel = Find<TextBlock>(toggle, "TextBlock");
			var check = Find<FontIcon>(toggle, "CheckGlyph");
			var plainX = plainLabel.TransformToVisual(plain).TransformPoint(new Point()).X;
			var toggleX = toggleLabel.TransformToVisual(toggle).TransformPoint(new Point()).X;
			Assert.AreEqual(plainX, toggleX, 0.5, "Checking an item must not misalign its label");
			Assert.IsTrue(toggleX >= check.TransformToVisual(toggle).TransformPoint(new Point()).X + check.ActualWidth + 8, "The checkmark has a readable gap before the label");
		}
		finally
		{
			menu.Hide();
			UnitTestsUIContentHelper.Content = null;
		}
	}

	[TestMethod]
	[DataRow(false)]
	[DataRow(true)]
	[RunsOnUIThread]
	public async Task When_AlertNeedsStackedActions_Then_DefaultIsFirstAndLabelsFit(bool longLabels)
	{
		var host = Host();
		var dialog = new ContentDialog
		{
			Style = (Style)host.Resources["ContentDialogStyle"],
			Title = "Save Changes?",
			PrimaryButtonText = longLabels ? "Save All Document Changes" : "Save",
			SecondaryButtonText = longLabels ? "Discard All Document Changes" : "Discard",
			CloseButtonText = longLabels ? "" : "Cancel",
			DefaultButton = ContentDialogButton.Secondary,
		};
		IAsyncOperation<ContentDialogResult>? showing = null;
		try
		{
			UnitTestsUIContentHelper.Content = host;
			await UnitTestsUIContentHelper.WaitForLoaded(host);
			dialog.XamlRoot = host.XamlRoot;
			showing = dialog.ShowAsync();
			await UnitTestsUIContentHelper.WaitForIdle();
			var primary = Find<Button>(dialog, "PrimaryButton");
			var secondary = Find<Button>(dialog, "SecondaryButton");
			var primaryPosition = primary.TransformToVisual(dialog).TransformPoint(new Point());
			var secondaryPosition = secondary.TransformToVisual(dialog).TransformPoint(new Point());
			Assert.IsTrue(secondaryPosition.Y + secondary.ActualHeight <= primaryPosition.Y, "The default action leads a vertical stack");
			Assert.IsTrue(primary.ActualHeight >= 44 && secondary.ActualHeight >= 44);
			Assert.AreEqual(primary.ActualWidth, secondary.ActualWidth, 0.5);
			if (!longLabels)
			{
				var close = Find<Button>(dialog, "CloseButton");
				Assert.IsTrue(close.TransformToVisual(dialog).TransformPoint(new Point()).Y > primaryPosition.Y, "Cancel stays at the bottom");
			}
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
	public async Task When_MenuHasAnActionIcon_Then_IconIsTrailing()
	{
		var host = Host();
		var anchor = new Button { Content = "Menu" };
		host.Children.Add(anchor);
		var item = new MenuFlyoutItem { Text = "Copy", Icon = new SymbolIcon(Symbol.Copy) };
		var menu = new MenuFlyout();
		menu.Items.Add(item);
		try
		{
			UnitTestsUIContentHelper.Content = host;
			await UnitTestsUIContentHelper.WaitForLoaded(anchor);
			menu.ShowAt(anchor);
			await UnitTestsUIContentHelper.WaitForIdle();
			var icon = Find<Viewbox>(item, "IconRoot");
			var label = Find<TextBlock>(item, "TextBlock");
			var iconPosition = icon.TransformToVisual(item).TransformPoint(new Point());
			var labelPosition = label.TransformToVisual(item).TransformPoint(new Point());
			Assert.IsTrue(iconPosition.X >= labelPosition.X + label.ActualWidth, "iOS menu action icons trail their text; leading icons are the macOS treatment");
		}
		finally
		{
			menu.Hide();
			UnitTestsUIContentHelper.Content = null;
		}
	}

	[TestMethod]
	[RunsOnUIThread]
	public async Task When_SidebarShown_Then_RegularGlassBacksNavigation()
	{
		var host = Host();
		var navigation = new NavigationView
		{
			Style = (Style)host.Resources["NavigationViewStyle"],
			PaneDisplayMode = NavigationViewPaneDisplayMode.Left,
			IsPaneOpen = true,
		};
		navigation.MenuItems.Add(new NavigationViewItem { Content = "Library" });
		host.Children.Add(navigation);
		try
		{
			UnitTestsUIContentHelper.Content = host;
			await UnitTestsUIContentHelper.WaitForLoaded(navigation);
			await UnitTestsUIContentHelper.WaitForIdle();
			var pane = Find<Grid>(navigation, "PaneContentGrid");
			var glass = Find<GlassPanel>(pane);
			Assert.AreEqual(GlassMaterial.Regular, glass.Material);
			Assert.IsTrue(glass.ActualHeight >= 100 && glass.ActualWidth >= 100, "Glass covers the realized pane");
			var split = Find<SplitView>(navigation, "RootSplitView");
			Assert.IsTrue(split.PaneBackground is null || split.PaneBackground is SolidColorBrush { Color.A: 0 }, "An opaque split pane must not hide the backdrop");
		}
		finally
		{
			UnitTestsUIContentHelper.Content = null;
		}
	}

	[TestMethod]
	[RunsOnUIThread]
	public async Task When_ToolbarShown_Then_CommandsShareOneFloatingGlassGroup()
	{
		var host = Host();
		var bar = new CommandBar { Style = (Style)host.Resources["CommandBarStyle"], Content = "Document", VerticalAlignment = VerticalAlignment.Top };
		bar.PrimaryCommands.Add(new AppBarButton { Label = "Add", Icon = new SymbolIcon(Symbol.Add) });
		bar.PrimaryCommands.Add(new AppBarButton { Label = "Share", Icon = new SymbolIcon(Symbol.Share) });
		bar.SecondaryCommands.Add(new AppBarButton { Label = "Copy", Icon = new SymbolIcon(Symbol.Copy) });
		host.Children.Add(bar);
		try
		{
			UnitTestsUIContentHelper.Content = host;
			await UnitTestsUIContentHelper.WaitForLoaded(bar);
			await UnitTestsUIContentHelper.WaitForIdle();
			var glass = Find<GlassPanel>(bar, "CommandGroupGlass");
			Assert.AreEqual(GlassMaterial.Regular, glass.Material);
			Assert.IsTrue(glass.ActualWidth < bar.ActualWidth, "Glass belongs to the commands, not a full-width toolbar strip");
			Assert.IsTrue(glass.ActualHeight >= 44);
			Assert.IsTrue(glass.CornerRadius.TopLeft >= 22, "The command group has a capsule silhouette");
			bar.IsOpen = true;
			await UnitTestsUIContentHelper.WaitForIdle();
			CommandBarOverflowPresenter? presenter = null;
			foreach (var popup in VisualTreeHelper.GetOpenPopupsForXamlRoot(bar.XamlRoot))
			{
				if (popup.Child is { } child && FindOrDefault<CommandBarOverflowPresenter>(child) is { } found)
				{
					presenter = found;
					break;
				}
			}
			Assert.IsNotNull(presenter);
			Assert.AreEqual(GlassMaterial.Thick, Find<GlassPanel>(presenter).Material, "Overflow commands share the menu material");
		}
		finally
		{
			bar.IsOpen = false;
			UnitTestsUIContentHelper.Content = null;
		}
	}

	[TestMethod]
	[DataRow(ContentDialogButton.Primary)]
	[DataRow(ContentDialogButton.Secondary)]
	[RunsOnUIThread]
	public async Task When_AlertHasTwoActions_Then_DefaultCapsuleIsTrailing(ContentDialogButton defaultButton)
	{
		var host = Host();
		var dialog = new ContentDialog
		{
			Style = (Style)host.Resources["ContentDialogStyle"],
			Title = "Save Changes?",
			Content = "Keep your changes before leaving.",
			PrimaryButtonText = "Save",
			SecondaryButtonText = "Discard",
			DefaultButton = defaultButton,
		};
		IAsyncOperation<ContentDialogResult>? showing = null;
		try
		{
			UnitTestsUIContentHelper.Content = host;
			await UnitTestsUIContentHelper.WaitForLoaded(host);
			dialog.XamlRoot = host.XamlRoot;
			showing = dialog.ShowAsync();
			await UnitTestsUIContentHelper.WaitForIdle();
			var primary = Find<Button>(dialog, "PrimaryButton");
			var secondary = Find<Button>(dialog, "SecondaryButton");
			var primaryPosition = primary.TransformToVisual(dialog).TransformPoint(new Point());
			var secondaryPosition = secondary.TransformToVisual(dialog).TransformPoint(new Point());
			Assert.AreEqual(primaryPosition.Y, secondaryPosition.Y, 0.5, "Two short actions share one row");
			Assert.IsTrue(defaultButton == ContentDialogButton.Primary ? primaryPosition.X > secondaryPosition.X : secondaryPosition.X > primaryPosition.X, "The default action is trailing");
			Assert.IsTrue(primary.CornerRadius.TopLeft >= 22 && secondary.CornerRadius.TopLeft >= 22, "Alert actions are capsules");
			Assert.IsTrue(primary.Background is SolidColorBrush { Color.A: > 0 } && secondary.Background is SolidColorBrush { Color.A: > 0 }, "Both capsules have a visible fill");
			Assert.AreEqual(HorizontalAlignment.Left, Find<ContentControl>(dialog, "Title").HorizontalContentAlignment);
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
}
