using System.Threading.Tasks;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Shapes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Uno.Cupertino;
using Uno.UI.RuntimeTests;

namespace Uno.Themes.Samples.RuntimeTests;

/// <summary>Guards the iOS field, stepper and picker presentation against Apple's HIG examples.</summary>
[TestClass]
public class Given_CupertinoHigFields
{
	private static Grid Container()
	{
		var host = new Grid { Width = 500, Height = 500 };
		host.Resources.MergedDictionaries.Add(new CupertinoTheme());
		return host;
	}

	private static T? Find<T>(DependencyObject root, string? name = null) where T : FrameworkElement
	{
		for (var i = 0; i < VisualTreeHelper.GetChildrenCount(root); i++)
		{
			var child = VisualTreeHelper.GetChild(root, i);
			if (child is T match && (name is null || match.Name == name))
			{
				return match;
			}
			if (Find<T>(child, name) is { } nested)
			{
				return nested;
			}
		}
		return null;
	}

	private static CalendarViewDayItem? FindDay(DependencyObject root, int month, int day)
	{
		for (var i = 0; i < VisualTreeHelper.GetChildrenCount(root); i++)
		{
			var child = VisualTreeHelper.GetChild(root, i);
			if (child is CalendarViewDayItem item && item.Date.Month == month && item.Date.Day == day)
			{
				return item;
			}
			if (FindDay(child, month, day) is { } nested)
			{
				return nested;
			}
		}
		return null;
	}

	private static async Task Load(Grid host, Control control, string key)
	{
		control.Style = (Style)host.Resources[key];
		control.HorizontalAlignment = HorizontalAlignment.Left;
		control.VerticalAlignment = VerticalAlignment.Top;
		host.Children.Add(control);
		UnitTestsUIContentHelper.Content = host;
		await UnitTestsUIContentHelper.WaitForLoaded(control);
		await UnitTestsUIContentHelper.WaitForIdle();
	}

	[TestMethod]
	[DataRow(false)]
	[DataRow(true)]
	[RunsOnUIThread]
	public async Task When_FieldHasHeader_Then_LabelSurvivesEntryAndBodyUsesIosSizing(bool secure)
	{
		var host = Container();
		Control field = secure
			? new PasswordBox { Header = "Password", Password = "example", Width = 320 }
			: new TextBox { Header = "Name", Text = "Example", Width = 320 };
		try
		{
			await Load(host, field, secure ? "CupertinoPasswordBoxStyle" : "CupertinoTextBoxStyle");
			var header = Find<ContentPresenter>(field, "HeaderContentPresenter");
			Assert.IsNotNull(header, "The persistent label must remain visible while entering text.");
			Assert.AreEqual(secure ? "Password" : "Name", header.Content);
			Assert.IsTrue(header.ActualHeight > 0);
			Assert.AreEqual(17d, field.FontSize);
			Assert.AreEqual(new CornerRadius(10), field.CornerRadius);
		}
		finally
		{
			UnitTestsUIContentHelper.Content = null;
		}
	}

	[TestMethod]
	[RunsOnUIThread]
	public async Task When_TextFieldHasClearButton_Then_TrailingTargetRemainsComfortable()
	{
		var host = Container();
		var field = new TextBox { Width = 320, Text = "Example" };
		try
		{
			await Load(host, field, "CupertinoTextBoxStyle");
			Assert.IsTrue(VisualStateManager.GoToState(field, "ButtonVisible", false));
			await UnitTestsUIContentHelper.WaitForIdle();
			var clear = Find<Button>(field, "DeleteButton");
			Assert.IsNotNull(clear);
			Assert.IsTrue(clear.ActualWidth >= 44 && clear.ActualHeight >= 44, "Clear button requires a 44px target around its small glyph.");
		}
		finally
		{
			UnitTestsUIContentHelper.Content = null;
		}
	}

	[TestMethod]
	[RunsOnUIThread]
	public async Task When_NumberBoxShowsStepper_Then_ValueLabelAndBothTargetsAreLegible()
	{
		var host = Container();
		var field = new NumberBox { Width = 360, Header = "Copies", Value = 3, SpinButtonPlacementMode = NumberBoxSpinButtonPlacementMode.Inline };
		try
		{
			await Load(host, field, "CupertinoNumberBoxStyle");
			foreach (var name in new[] { "DownSpinButton", "UpSpinButton" })
			{
				var button = Find<RepeatButton>(field, name);
				Assert.IsNotNull(button);
				Assert.IsTrue(button.ActualWidth >= 44 && button.ActualHeight >= 44, name);
			}
			var input = Find<TextBox>(field, "InputBox");
			Assert.IsNotNull(input);
			Assert.AreEqual(17d, input.FontSize);
			var header = Find<ContentPresenter>(input, "HeaderContentPresenter");
			Assert.IsNotNull(header);
			Assert.AreEqual(13d, header.FontSize);
		}
		finally
		{
			UnitTestsUIContentHelper.Content = null;
		}
	}

	[TestMethod]
	[RunsOnUIThread]
	public async Task When_ComboBoxLoaded_Then_FieldUsesIosBodyAndTargetSize()
	{
		var host = Container();
		var field = new ComboBox { Width = 250, PlaceholderText = "Choose" };
		try
		{
			await Load(host, field, "CupertinoComboBoxStyle");
			Assert.AreEqual(17d, field.FontSize);
			Assert.IsTrue(field.ActualHeight >= 44);
			Assert.AreEqual(new CornerRadius(10), field.CornerRadius);
		}
		finally
		{
			UnitTestsUIContentHelper.Content = null;
		}
	}

	[TestMethod]
	[RunsOnUIThread]
	public async Task When_CalendarLoaded_Then_DayLabelsMatchInlinePickerScale()
	{
		var host = Container();
		var calendar = new CalendarView();
		try
		{
			await Load(host, calendar, "CupertinoCalendarViewStyle");
			Assert.AreEqual(20d, calendar.DayItemFontSize);
			Assert.AreEqual(17d, calendar.MonthYearItemFontSize);
		}
		finally
		{
			UnitTestsUIContentHelper.Content = null;
		}
	}

	[TestMethod]
	[RunsOnUIThread]
	public async Task When_CompactCalendarLoaded_Then_DateAffordanceHasComfortableTarget()
	{
		var host = Container();
		var field = new CalendarDatePicker { Width = 240, Date = System.DateTimeOffset.Now };
		try
		{
			await Load(host, field, "CupertinoCalendarDatePickerStyle");
			Assert.IsTrue(field.ActualHeight >= 44);
			Assert.AreEqual(17d, field.FontSize);
			Assert.AreEqual(new CornerRadius(22), field.CornerRadius);
		}
		finally
		{
			UnitTestsUIContentHelper.Content = null;
		}
	}

	[TestMethod]
	[RunsOnUIThread]
	public async Task When_DatePickerLoaded_Then_CompactDateHasRoundedTouchTarget()
	{
		var host = Container();
		var field = new DatePicker { Width = 240, SelectedDate = System.DateTimeOffset.Now };
		try
		{
			await Load(host, field, "CupertinoDatePickerStyle");
			var button = Find<Button>(field, "FlyoutButton");
			Assert.IsNotNull(button);
			Assert.IsTrue(button.ActualHeight >= 44);
			Assert.AreEqual(new CornerRadius(22), field.CornerRadius);
			Assert.AreEqual(17d, field.FontSize);
		}
		finally
		{
			UnitTestsUIContentHelper.Content = null;
		}
	}

	[TestMethod]
	[RunsOnUIThread]
	public async Task When_WheelPickerPresented_Then_PopoverUsesGlassAndCapsuleSelection()
	{
		var host = Container();
		var presenter = new DatePickerFlyoutPresenter();
		try
		{
			await Load(host, presenter, "CupertinoDatePickerFlyoutPresenterStyle");
			var glass = Find<GlassPanel>(presenter);
			Assert.IsNotNull(glass, "Wheel picker popover must match the other Cupertino popovers.");
			Assert.AreEqual(GlassMaterial.Thick, glass.Material);
			Assert.AreEqual(new CornerRadius(26), glass.CornerRadius);
			var highlight = Find<Rectangle>(presenter, "HighlightRect");
			Assert.IsNotNull(highlight);
			Assert.IsTrue(highlight.RadiusX >= 16 && highlight.RadiusY >= 16, "Selection band must have capsule ends.");
		}
		finally
		{
			UnitTestsUIContentHelper.Content = null;
		}
	}

	[TestMethod]
	[RunsOnUIThread]
	public async Task When_WheelPickerBorderOverridden_Then_GlassPreservesPresenterContract()
	{
		var host = Container();
		var border = new SolidColorBrush(Windows.UI.Color.FromArgb(255, 220, 20, 60));
		var presenter = new DatePickerFlyoutPresenter { BorderBrush = border, BorderThickness = new Thickness(3) };
		try
		{
			await Load(host, presenter, "CupertinoDatePickerFlyoutPresenterStyle");
			var glass = Find<GlassPanel>(presenter);
			Assert.IsNotNull(glass);
			Assert.AreSame(border, glass.BorderBrush);
			Assert.AreEqual(new Thickness(3), glass.BorderThickness);
		}
		finally
		{
			UnitTestsUIContentHelper.Content = null;
		}
	}
	[TestMethod]
	[RunsOnUIThread]
	public async Task When_ComboBoxHasSelection_Then_HeaderAndDisclosureRemainVisible()
	{
		var host = Container();
		var field = new ComboBox { Width = 320, Header = "Destination" };
		field.Items.Add("Cupertino");
		field.SelectedIndex = 0;
		try
		{
			await Load(host, field, "CupertinoComboBoxStyle");
			var header = Find<ContentPresenter>(field, "HeaderContentPresenter");
			Assert.IsNotNull(header, "The destination label must not disappear.");
			Assert.AreEqual("Destination", header.Content);
			Assert.IsTrue(header.ActualHeight > 0);
			var selected = Find<ContentPresenter>(field, "ContentPresenter");
			Assert.IsNotNull(selected);
			Assert.AreEqual(((SolidColorBrush)field.Foreground).Color, ((SolidColorBrush)selected.Foreground).Color, "Selection must not look disabled.");
			var arrow = Find<FontIcon>(field, "DropDownGlyph");
			Assert.IsNotNull(arrow, "A closed picker needs its disclosure affordance.");
			Assert.IsTrue(arrow.ActualWidth > 0 && arrow.Visibility == Visibility.Visible);
		}
		finally
		{
			UnitTestsUIContentHelper.Content = null;
		}
	}

	[TestMethod]
	[RunsOnUIThread]
	public async Task When_DateIsEmpty_Then_CompactTriggerDoesNotRepeatItsLabel()
	{
		var host = Container();
		var field = new DatePicker { Header = "Departure", Style = (Style)host.Resources["CupertinoDatePickerStyle"] };
		host.Children.Add(field);
		try
		{
			UnitTestsUIContentHelper.Content = host;
			await UnitTestsUIContentHelper.WaitForLoaded(field);
			await UnitTestsUIContentHelper.WaitForIdle();
			var placeholder = Find<TextBlock>(field, "PlaceholderText");
			Assert.IsNotNull(placeholder);
			Assert.AreNotEqual("Departure", placeholder.Text, "The value affordance must not duplicate the external label.");
			Assert.IsTrue(field.ActualWidth < host.ActualWidth, "Compact date controls should size to their content by default.");
			var header = Find<TextBlock>(field, "HeaderTextBlock");
			Assert.IsNotNull(header);
			Assert.AreEqual(13d, header.FontSize);
		}
		finally
		{
			UnitTestsUIContentHelper.Content = null;
		}
	}

	[TestMethod]
	[RunsOnUIThread]
	public async Task When_ComboBoxStretches_Then_DisclosureStaysAtTrailingEdge()
	{
		var host = Container();
		var field = new ComboBox { Width = 400 };
		field.Items.Add("Cupertino");
		field.SelectedIndex = 0;
		try
		{
			await Load(host, field, "CupertinoComboBoxStyle");
			var glyph = Find<FontIcon>(field, "DropDownGlyph");
			Assert.IsNotNull(glyph);
			var position = glyph.TransformToVisual(field).TransformPoint(new Windows.Foundation.Point());
			Assert.IsTrue(position.X > field.ActualWidth - 50, $"Disclosure was at {position.X} in a {field.ActualWidth} field.");
		}
		finally
		{
			UnitTestsUIContentHelper.Content = null;
		}
	}

	[TestMethod]
	[RunsOnUIThread]
	public async Task When_DatePickerSizesToContent_Then_NoLegacyWidthPadsItsPrompt()
	{
		var host = Container();
		var field = new DatePicker { Header = "Departure" };
		try
		{
			await Load(host, field, "CupertinoDatePickerStyle");
			var prompt = Find<TextBlock>(field, "PlaceholderText");
			Assert.IsNotNull(prompt);
			Assert.IsTrue(field.ActualWidth <= prompt.ActualWidth + 34, $"Date width {field.ActualWidth} must follow prompt width {prompt.ActualWidth} plus 32px padding.");
		}
		finally
		{
			UnitTestsUIContentHelper.Content = null;
		}
	}

	[TestMethod]
	[RunsOnUIThread]
	public async Task When_CalendarPopupOpens_Then_OnlySelectedDatesHaveFilledDiscs()
	{
		var host = Container();
		var field = new CalendarDatePicker { Date = new System.DateTimeOffset(2026, 9, 21, 12, 0, 0, System.TimeSpan.Zero) };
		try
		{
			await Load(host, field, "CupertinoCalendarDatePickerStyle");
			field.IsCalendarOpen = true;
			await UnitTestsUIContentHelper.WaitForIdle();
			FlyoutPresenter? popupPresenter = null;
			foreach (var popup in VisualTreeHelper.GetOpenPopupsForXamlRoot(field.XamlRoot))
			{
				if (popup.Child is { } child)
				{
					popupPresenter = child as FlyoutPresenter ?? Find<FlyoutPresenter>(child);
					if (popupPresenter is not null)
					{
						break;
					}
				}
			}
			Assert.IsNotNull(popupPresenter);
			var glass = Find<GlassPanel>(popupPresenter);
			Assert.IsNotNull(glass, "The actual popup needs a rounded glass backing, not a square default presenter.");
			Assert.AreEqual(new CornerRadius(26), glass.CornerRadius);
			var clip = Find<Border>(popupPresenter, "CalendarPopupClip");
			Assert.IsNotNull(clip);
			Assert.AreEqual(new CornerRadius(26), clip.CornerRadius);
			var calendar = Find<CalendarView>(popupPresenter);
			Assert.IsNotNull(calendar);
			Assert.AreEqual((byte)0, ((SolidColorBrush)calendar.OutOfScopeBackground).Color.A, "Adjacent month dates must not acquire filled discs.");
			Assert.AreEqual((byte)0, ((SolidColorBrush)calendar.CalendarItemBackground).Color.A, "Unselected dates must leave the popup backing visible.");
			var adjacentDay = FindDay(calendar, 8, 31);
			var currentDay = FindDay(calendar, 9, 1);
			Assert.IsNotNull(adjacentDay, "The preceding month's trailing dates should stay available.");
			Assert.IsNotNull(currentDay);
			var adjacentLabel = Find<TextBlock>(adjacentDay);
			var currentLabel = Find<TextBlock>(currentDay);
			Assert.IsNotNull(adjacentLabel);
			Assert.IsNotNull(currentLabel);
			Assert.AreNotEqual(((SolidColorBrush)currentLabel.Foreground).Color, ((SolidColorBrush)adjacentLabel.Foreground).Color,
				"Adjacent-month date text must remain visually distinct after its background disc is removed.");
		}
		finally
		{
			field.IsCalendarOpen = false;
			UnitTestsUIContentHelper.Content = null;
		}
	}

	[TestMethod]
	[RunsOnUIThread]
	public async Task When_CalendarViewStyleIsCustomized_Then_PopupHonorsItsSurface()
	{
		var host = Container();
		var background = new SolidColorBrush(Microsoft.UI.Colors.Crimson);
		var style = new Style(typeof(CalendarView)) { BasedOn = (Style)host.Resources["CupertinoCalendarViewStyle"] };
		style.Setters.Add(new Setter(Control.BackgroundProperty, background));
		style.Setters.Add(new Setter(Control.BorderThicknessProperty, new Thickness(3)));
		var field = new CalendarDatePicker { CalendarViewStyle = style };
		try
		{
			await Load(host, field, "CupertinoCalendarDatePickerStyle");
			field.IsCalendarOpen = true;
			await UnitTestsUIContentHelper.WaitForIdle();
			CalendarView? calendar = null;
			foreach (var popup in VisualTreeHelper.GetOpenPopupsForXamlRoot(field.XamlRoot))
			{
				if (popup.Child is { } child && Find<CalendarView>(child) is { } found)
				{
					calendar = found;
					break;
				}
			}
			Assert.IsNotNull(calendar);
			Assert.AreSame(background, calendar.Background, "CalendarViewStyle must control the popup calendar background.");
			Assert.AreEqual(new Thickness(3), calendar.BorderThickness);
		}
		finally
		{
			field.IsCalendarOpen = false;
			UnitTestsUIContentHelper.Content = null;
		}
	}

}
