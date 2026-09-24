using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Microsoft.UI.Xaml.Media.Animation;
using Microsoft.UI.Xaml.Media.Imaging;
using Microsoft.UI.Xaml.Shapes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Uno.Cupertino;
using Uno.UI.RuntimeTests;
using Windows.Foundation;
using Windows.UI;

namespace Uno.Themes.Samples.RuntimeTests;

/// <summary>
/// Findings of the 2026-09-24 review of the whole theme against Apple's HIG artwork and iOS: separators
/// and fills that were invisible, Fluent chrome that leaked through, and a calendar that hid today.
/// Each test failed before its fix (specs/10-cupertino-liquid-glass/hig-review-2026-09-24.md).
/// </summary>
[TestClass]
public class Given_CupertinoHigReview
{
	private static Grid CreateThemedContainer(ElementTheme theme = ElementTheme.Light)
	{
		var container = new Grid { Width = 360, Height = 400, RequestedTheme = theme };
		container.Resources.MergedDictionaries.Add(new CupertinoTheme());
		return container;
	}

	private static T Resource<T>(FrameworkElement scope, string key)
		=> scope.Resources.TryGetValue(key, out var value) && value is T typed
			? typed
			: throw new AssertFailedException($"Resource '{key}' not found or not a {typeof(T).Name}");

	private static Color? ColorOf(Brush? brush) => (brush as SolidColorBrush)?.Color;

	// A scoped lookup resolves theme dictionaries against the application theme, not the container's
	// RequestedTheme (specs/lessons.md), so the expected value of a themed key is read from the theme's own
	// Light or Default dictionary.
	private static T Themed<T>(ElementTheme theme, string key)
	{
		var name = theme == ElementTheme.Dark ? "Default" : "Light";
		return Find(new CupertinoTheme()) ?? throw new AssertFailedException($"'{key}' is not a {typeof(T).Name} in the {name} dictionary");

		T? Find(ResourceDictionary dictionary)
		{
			if (dictionary.ThemeDictionaries.TryGetValue(name, out var themed)
				&& themed is ResourceDictionary scoped
				&& scoped.TryGetValue(key, out var value)
				&& value is T typed)
			{
				return typed;
			}

			foreach (var merged in dictionary.MergedDictionaries)
			{
				if (Find(merged) is { } nested)
				{
					return nested;
				}
			}

			return default;
		}
	}

	private static Color ThemedColor(ElementTheme theme, string brushKey) => Themed<SolidColorBrush>(theme, brushKey).Color;

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

	private static T Find<T>(DependencyObject root, string? name = null)
		where T : FrameworkElement
		=> FindDescendant<T>(root, name) ?? throw new AssertFailedException($"Missing {typeof(T).Name} {name}");

	private static System.Collections.Generic.IEnumerable<T> AllDescendants<T>(DependencyObject root)
		where T : DependencyObject
	{
		for (var i = 0; i < VisualTreeHelper.GetChildrenCount(root); i++)
		{
			var child = VisualTreeHelper.GetChild(root, i);
			if (child is T match)
			{
				yield return match;
			}

			foreach (var nested in AllDescendants<T>(child))
			{
				yield return nested;
			}
		}
	}

	private static async Task<(byte[] Pixels, int Width, double Scale)> Render(FrameworkElement element, int scale = 1)
	{
		var bitmap = new RenderTargetBitmap();
		await bitmap.RenderAsync(element, (int)(element.ActualWidth * scale), (int)(element.ActualHeight * scale));
		var pixels = (await bitmap.GetPixelsAsync()).ToArray();
		return (pixels, bitmap.PixelWidth, bitmap.PixelWidth / element.ActualWidth);
	}

	private static int Luma((byte[] Pixels, int Width, double Scale) render, double x, double y)
	{
		var i = (((int)(y * render.Scale) * render.Width) + (int)(x * render.Scale)) * 4;
		return (render.Pixels[i] + render.Pixels[i + 1] + render.Pixels[i + 2]) / 3;
	}

	private static int Darkest((byte[] Pixels, int Width, double Scale) render, double x, double fromY, double toY)
	{
		var darkest = 255;
		for (var y = fromY; y < toY; y += 0.25)
		{
			darkest = Math.Min(darkest, Luma(render, x, y));
		}

		return darkest;
	}

	// Uno does not paint the day item that is "today" once DayItemCornerRadius is non-zero (radius 0 shows
	// it; nothing else about the item matters). Until that is fixed upstream the theme must not lose the
	// date: today's number has to be on screen.
	[TestMethod]
	[RunsOnUIThread]
	public async Task When_CalendarRendered_Then_TodayIsVisible()
	{
		var container = CreateThemedContainer();
		container.Background = new SolidColorBrush(Colors.White);
		try
		{
			var calendar = await Load(container, new CalendarView { Width = 300, VerticalAlignment = VerticalAlignment.Top });
			await Task.Delay(300);
			var today = FindToday(calendar) ?? throw new AssertFailedException("today is not in the visible month");
			var origin = today.TransformToVisual(container).TransformPoint(new Point(0, 0));
			var render = await Render(container);
			var inked = 0;
			for (var y = origin.Y + 4; y < origin.Y + today.ActualHeight - 4; y++)
			{
				for (var x = origin.X + 4; x < origin.X + today.ActualWidth - 4; x++)
				{
					if (Luma(render, x, y) < 240)
					{
						inked++;
					}
				}
			}

			Assert.IsTrue(inked > 20, $"today's number should be drawn, only {inked} inked pixels in its cell");
		}
		finally
		{
			UnitTestsUIContentHelper.Content = null;
		}
	}

	private static CalendarViewDayItem? FindToday(DependencyObject root)
	{
		for (var i = 0; i < VisualTreeHelper.GetChildrenCount(root); i++)
		{
			var child = VisualTreeHelper.GetChild(root, i);
			if (child is CalendarViewDayItem item && item.Date.Date == DateTimeOffset.Now.Date)
			{
				return item;
			}

			if (FindToday(child) is { } nested)
			{
				return nested;
			}
		}

		return null;
	}

	// iOS separators are the label at 29 %, clearly visible on the row surface, and an inset group draws
	// none under its last row (HIG lists-and-tables artwork).
	[TestMethod]
	[RunsOnUIThread]
	public async Task When_ListRendered_Then_SeparatorsShowAndTheGroupEndsClean()
	{
		var container = CreateThemedContainer();
		container.Background = new SolidColorBrush(Colors.White);
		var list = new ListView { Width = 300, VerticalAlignment = VerticalAlignment.Top };
		list.Items.Add(new ListViewItem { Content = "First" });
		list.Items.Add(new ListViewItem { Content = "Second" });
		list.Items.Add(new ListViewItem { Content = "Third" });
		try
		{
			await Load(container, list);
			var first = (ListViewItem)list.ContainerFromIndex(0);
			var firstBottom = first.TransformToVisual(container).TransformPoint(new Point(0, first.ActualHeight)).Y;
			var groupBottom = list.TransformToVisual(container).TransformPoint(new Point(0, list.ActualHeight)).Y;
			// Rendered at 2× so the half-point hairline is one crisp device pixel, as on a Retina display.
			var render = await Render(container, 2);

			// The darkest device row around the first row's bottom edge, mid-row: the hairline. The software
			// renderer anti-aliases the half-point line to a quarter of its 29 % ink; the old separator colour
			// (systemGray5 on white) left under 3 levels.
			var separator = Darkest(render, 150, firstBottom - 1.5, firstBottom + 0.5);
			var surface = Luma(render, 150, firstBottom - 8);
			Assert.IsTrue(surface - separator >= 8, $"the separator should read against the row: surface {surface}, separator {separator}");

			// The last row ends on the group's edge without a line.
			var groupEdge = Darkest(render, 150, groupBottom - 1.5, groupBottom);
			Assert.IsTrue(groupEdge >= 250, $"no separator under the last row, luma was {groupEdge}");
		}
		finally
		{
			UnitTestsUIContentHelper.Content = null;
		}
	}

	// A selected table row on iPhone is highlighted in a neutral gray, never in the accent.
	[TestMethod]
	[RunsOnUIThread]
	public async Task When_ListRowSelected_Then_HighlightIsNeutral()
	{
		var container = CreateThemedContainer();
		var list = new ListView { Width = 300, VerticalAlignment = VerticalAlignment.Top };
		list.Items.Add(new ListViewItem { Content = "First" });
		list.Items.Add(new ListViewItem { Content = "Second", IsSelected = true });
		try
		{
			await Load(container, list);
			var selected = (ListViewItem)list.ContainerFromIndex(1);
			var fill = ColorOf(Find<Grid>(selected, "ContentBorder").Background) ?? throw new AssertFailedException("no fill");
			Assert.IsTrue(Math.Abs(fill.R - fill.B) <= 10 && Math.Abs(fill.G - fill.B) <= 10, $"a neutral gray, was {fill}");
			Assert.IsTrue(fill.A < 255, $"a translucent fill that also reads in Dark, alpha was {fill.A}");
		}
		finally
		{
			UnitTestsUIContentHelper.Content = null;
		}
	}

	// UIProgressView: a 4 pt track in the system fill under the accent, not a 1 px Fluent hairline.
	[TestMethod]
	[RunsOnUIThread]
	public async Task When_ProgressBarRendered_Then_TrackIsFourPixelsOfSystemFill()
	{
		var container = CreateThemedContainer();
		try
		{
			var bar = await Load(container, new ProgressBar { Value = 50, Width = 300, VerticalAlignment = VerticalAlignment.Top });
			var track = Find<Rectangle>(bar, "ProgressBarTrack");
			Assert.AreEqual(4d, track.ActualHeight, "track height");
			Assert.AreEqual(ThemedColor(ElementTheme.Light, "CupertinoSystemFillBrush"), ColorOf(track.Fill), "track fill");
		}
		finally
		{
			UnitTestsUIContentHelper.Content = null;
		}
	}

	// UISwitch glides; a Duration="0" knob jump is not a toggle animation.
	[TestMethod]
	[RunsOnUIThread]
	public async Task When_SwitchToggles_Then_TheKnobGlides()
	{
		var container = CreateThemedContainer();
		try
		{
			var toggle = await Load(container, new ToggleSwitch());
			var root = (FrameworkElement)VisualTreeHelper.GetChild(toggle, 0);
			var on = VisualStateManager.GetVisualStateGroups(root)
				.First(g => g.Name == "ToggleStates").States.First(s => s.Name == "On");
			var glide = on.Storyboard.Children.OfType<DoubleAnimation>()
				.First(a => Storyboard.GetTargetName(a) == "KnobTranslateTransform");
			Assert.IsTrue(glide.Duration.TimeSpan.TotalMilliseconds >= 150, $"the knob should glide, duration was {glide.Duration.TimeSpan.TotalMilliseconds} ms");
			Assert.IsNotNull(glide.EasingFunction, "eased, not linear");
		}
		finally
		{
			UnitTestsUIContentHelper.Content = null;
		}
	}

	// The Windows-only storyboard left CheckedPressed empty everywhere else, so a checked box flashed to
	// unchecked for as long as it was pressed.
	[TestMethod]
	[RunsOnUIThread]
	public async Task When_CheckedCheckBoxPressed_Then_TheCheckStays()
	{
		var container = CreateThemedContainer();
		try
		{
			var checkBox = await Load(container, new CheckBox { Content = "Keep", IsChecked = true });
			Assert.IsTrue(VisualStateManager.GoToState(checkBox, "CheckedPressed", false), "state exists");
			await UnitTestsUIContentHelper.WaitForIdle();
			Assert.AreEqual(1d, Find<Microsoft.UI.Xaml.Shapes.Path>(checkBox, "CheckGlyph").Opacity, "the check glyph stays while pressed");
			Assert.AreEqual(1d, Find<Grid>(checkBox, "CheckedBackgroundBorder").Opacity, "the filled box stays while pressed");
		}
		finally
		{
			UnitTestsUIContentHelper.Content = null;
		}
	}

	// An unchecked radio is an outlined circle in both appearances, not a solid gray disc in Dark.
	[TestMethod]
	[RunsOnUIThread]
	public async Task When_RadioButtonUncheckedInDark_Then_ItIsAnOutline()
	{
		var container = CreateThemedContainer(ElementTheme.Dark);
		try
		{
			var radio = await Load(container, new RadioButton { Content = "Sort by date" });
			var ellipse = Find<Ellipse>(radio, "UncheckEllipse");
			Assert.AreEqual((byte)0, ColorOf(ellipse.Fill)?.A ?? (byte)0, "no fill");
			Assert.IsTrue(ellipse.StrokeThickness > 0, "an outline");
		}
		finally
		{
			UnitTestsUIContentHelper.Content = null;
		}
	}

	// An iOS pop-up button is a gray capsule with a tinted value, like the compact date pickers; iOS has no
	// outlined controls.
	[TestMethod]
	[RunsOnUIThread]
	public async Task When_ComboBoxRendered_Then_ItIsAGrayCapsule()
	{
		var container = CreateThemedContainer();
		try
		{
			var comboBox = await Load(container, new ComboBox { ItemsSource = new[] { "One", "Two" }, SelectedIndex = 0, VerticalAlignment = VerticalAlignment.Top });
			var frame = Find<Grid>(comboBox, "ComboBoxContent");
			Assert.AreEqual(0d, frame.BorderThickness.Left, "no outline");
			Assert.AreEqual(22d, frame.CornerRadius.TopLeft, "a capsule");
			Assert.AreEqual(ThemedColor(ElementTheme.Light, "CupertinoQuinaryGrayBrush"), ColorOf(frame.Background), "the compact-picker gray");
			Assert.AreEqual(ThemedColor(ElementTheme.Light, "PrimaryBrush"), ColorOf(Find<FontIcon>(comboBox, "DropDownGlyph").Foreground), "a tinted chevron");
		}
		finally
		{
			UnitTestsUIContentHelper.Content = null;
		}
	}

	// Hover, pressed and selected fills were the opaque secondary container: #1C1C1F on a #1C1C1E surface in
	// Dark, invisible. iOS uses translucent system fills that read on any surface.
	[TestMethod]
	[DataRow("ListViewItemBackgroundPointerOver")]
	[DataRow("ListViewItemBackgroundPressed")]
	[DataRow("MenuFlyoutItemBackgroundPointerOver")]
	[DataRow("MenuFlyoutItemBackgroundPressed")]
	[DataRow("NavigationViewItemBackgroundPointerOver")]
	[DataRow("NavigationViewItemBackgroundPressed")]
	[RunsOnUIThread]
	public async Task When_HighlightFillResolvesInDark_Then_ItIsTranslucent(string key)
	{
		var container = CreateThemedContainer(ElementTheme.Dark);
		try
		{
			await Load(container, new Border());
			var fill = Resource<SolidColorBrush>(container, key).Color;
			Assert.IsTrue(fill.A > 0 && fill.A < 255, $"{key} should be a translucent fill, was {fill}");
		}
		finally
		{
			UnitTestsUIContentHelper.Content = null;
		}
	}

	// iOS menus separate groups with a thick band of fill, not a hairline inside a gap.
	[TestMethod]
	[RunsOnUIThread]
	public async Task When_MenuSeparatorRendered_Then_ItIsABand()
	{
		var container = CreateThemedContainer();
		var menu = new MenuFlyout();
		menu.Items.Add(new MenuFlyoutItem { Text = "Copy" });
		var separator = new MenuFlyoutSeparator();
		menu.Items.Add(separator);
		menu.Items.Add(new MenuFlyoutItem { Text = "Delete" });
		try
		{
			var anchor = await Load(container, new Button { Content = "Anchor" });
			menu.ShowAt(anchor);
			await UnitTestsUIContentHelper.WaitForIdle();
			var band = Find<Rectangle>(separator);
			Assert.AreEqual(8d, band.ActualHeight, "an 8 px band");
			Assert.IsTrue(ColorOf(band.Fill)?.A is > 0 and < 255, "a translucent fill");
		}
		finally
		{
			menu.Hide();
			UnitTestsUIContentHelper.Content = null;
		}
	}

	// The content area inherited Fluent's card stroke and 8 px top-left corner; an iPadOS detail column has
	// neither. The header inherited Fluent's 28 pt semibold; the iOS large title is 34 pt bold.
	[TestMethod]
	[RunsOnUIThread]
	public async Task When_NavigationViewRendered_Then_ContentHasNoCardChromeAndALargeTitle()
	{
		var container = CreateThemedContainer();
		var navigation = new NavigationView { Header = "Library", IsSettingsVisible = false, IsBackButtonVisible = NavigationViewBackButtonVisible.Collapsed };
		navigation.MenuItems.Add(new NavigationViewItem { Content = "Library" });
		try
		{
			await Load(container, navigation);
			var content = Find<Grid>(navigation, "ContentGrid");
			Assert.AreEqual(new Thickness(0), content.BorderThickness, "no card stroke");
			Assert.AreEqual(new CornerRadius(0), content.CornerRadius, "no card corner");
			var header = Find<ContentControl>(navigation, "HeaderContent");
			Assert.AreEqual(Resource<double>(container, "DisplayMediumFontSize"), header.FontSize, "large title size");
			Assert.AreEqual(Microsoft.UI.Text.FontWeights.Bold.Weight, header.FontWeight.Weight, "large title weight");
		}
		finally
		{
			UnitTestsUIContentHelper.Content = null;
		}
	}

	// An alert title wraps; clamping it to two lines silently loses text.
	[TestMethod]
	[RunsOnUIThread]
	public async Task When_AlertShown_Then_TitleIsNotClamped()
	{
		var container = CreateThemedContainer();
		var dialog = new ContentDialog
		{
			Title = "A title long enough to need a third line when the alert is at its minimum width of two hundred and seventy points",
			Content = "Body",
			PrimaryButtonText = "OK",
			Style = Resource<Style>(container, "ContentDialogStyle"),
		};
		IAsyncOperation<ContentDialogResult>? showing = null;
		try
		{
			await Load(container, new Border());
			dialog.XamlRoot = container.XamlRoot;
			showing = dialog.ShowAsync();
			await UnitTestsUIContentHelper.WaitForIdle();
			var clamped = AllDescendants<ContentPresenter>(dialog).Where(p => p.MaxLines > 0).ToArray();
			Assert.AreEqual(0, clamped.Length, "no presenter in the alert clamps its lines");
		}
		finally
		{
			dialog.Hide();
			if (showing is { })
			{
				await showing;
			}

			UnitTestsUIContentHelper.Content = null;
		}
	}

	// A pressed compact picker dims like a pressed button, through the same token.
	[TestMethod]
	[RunsOnUIThread]
	public async Task When_CompactPickerPressed_Then_ItDimsWithTheButtonPressedOpacity()
	{
		var container = CreateThemedContainer();
		try
		{
			var calendar = await Load(container, new CalendarDatePicker { VerticalAlignment = VerticalAlignment.Top });
			var date = await Load(container, new DatePicker { VerticalAlignment = VerticalAlignment.Bottom });
			var expected = Resource<double>(container, "CupertinoButtonPressedOpacity");

			Assert.IsTrue(VisualStateManager.GoToState(calendar, "Pressed", false), "calendar picker Pressed state");
			Assert.AreEqual(expected, Find<Border>(calendar, "Background").Opacity, "calendar picker");

			var button = Find<Button>(date, "FlyoutButton");
			Assert.IsTrue(VisualStateManager.GoToState(button, "Pressed", false), "date picker Pressed state");
			Assert.AreEqual(expected, Find<Grid>(button, "RootGrid").Opacity, "date picker");
		}
		finally
		{
			UnitTestsUIContentHelper.Content = null;
		}
	}

	// The disabled state assigned a Color to a Brush property, so the value never dimmed.
	[TestMethod]
	[RunsOnUIThread]
	public async Task When_DatePickerDisabled_Then_TheValueDims()
	{
		var container = CreateThemedContainer();
		try
		{
			var picker = await Load(container, new DatePicker { IsEnabled = false, VerticalAlignment = VerticalAlignment.Top });
			var text = Find<TextBlock>(picker, "DateText");
			Assert.AreEqual(ThemedColor(ElementTheme.Light, "CupertinoPrimaryGrayBrush"), ColorOf(text.Foreground), "a dimmed value");
		}
		finally
		{
			UnitTestsUIContentHelper.Content = null;
		}
	}

	// Apple's semantic alphas, rounded to the nearest byte (0.30 → 0x4D, 0.18 → 0x2E, 0.16 → 0x29, 0.29 → 0x4A),
	// read from the live brushes the theme paints from its palette.
	[TestMethod]
	[DataRow(ElementTheme.Light, "CupertinoTertiaryLabelBrush", 0x4D)]
	[DataRow(ElementTheme.Light, "CupertinoQuaternaryLabelBrush", 0x2E)]
	[DataRow(ElementTheme.Light, "CupertinoSecondarySystemFillBrush", 0x29)]
	[DataRow(ElementTheme.Light, "CupertinoTertiarySystemFillBrush", 0x1F)]
	[DataRow(ElementTheme.Light, "CupertinoSeparatorBrush", 0x4A)]
	[DataRow(ElementTheme.Dark, "CupertinoTertiaryLabelBrush", 0x4D)]
	[DataRow(ElementTheme.Dark, "CupertinoQuaternaryLabelBrush", 0x29)]
	[DataRow(ElementTheme.Dark, "CupertinoSystemFillBrush", 0x5C)]
	[DataRow(ElementTheme.Dark, "CupertinoSecondarySystemFillBrush", 0x52)]
	[DataRow(ElementTheme.Dark, "CupertinoQuaternarySystemFillBrush", 0x2E)]
	[DataRow(ElementTheme.Dark, "CupertinoSeparatorBrush", 0x99)]
	[RunsOnUIThread]
	public void When_LiveBrushesResolve_Then_AlphasMatchApple(ElementTheme theme, string key, int alpha)
	{
		Assert.AreEqual((byte)alpha, ThemedColor(theme, key).A, $"{key}: {ThemedColor(theme, key)}");
	}

	// An iOS stepper field: the header sits above a 44 pt frame, as for every other field; the header had
	// been drawn inside an outlined box that grew to 62 pt around 48 pt spin buttons.
	[TestMethod]
	[RunsOnUIThread]
	public async Task When_NumberBoxRendered_Then_HeaderSitsAboveAFortyFourPointFrame()
	{
		var container = CreateThemedContainer();
		try
		{
			var box = await Load(container, new NumberBox { Header = "Guests", Value = 2, VerticalAlignment = VerticalAlignment.Top });
			var frame = Find<Border>(box, "ContentBorder");
			Assert.AreEqual(44d, frame.ActualHeight, "frame height");
			var header = AllDescendants<TextBlock>(box).First(t => t.Text == "Guests");
			var headerBottom = header.TransformToVisual(box).TransformPoint(new Point(0, header.ActualHeight)).Y;
			var frameTop = frame.TransformToVisual(box).TransformPoint(new Point(0, 0)).Y;
			Assert.IsTrue(headerBottom <= frameTop, $"the header ends at {headerBottom:0.#} before the frame starts at {frameTop:0.#}");
		}
		finally
		{
			UnitTestsUIContentHelper.Content = null;
		}
	}
}
