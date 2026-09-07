using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Uno.Material;
using Uno.UI.RuntimeTests;
using Windows.UI;

namespace Uno.Themes.Samples.RuntimeTests;

// Material templates are available only in this head; the Simple host uses different templates.
[TestClass]
public class Given_SemanticOverrides
{
	[TestMethod]
	[RunsOnUIThread]
	[DataRow("PipsPagerPreviousPageButtonData")]
	[DataRow("PipsPagerNextPageButtonData")]
	public void When_PipsNavigationDataIsResolved_Then_ItIsAPathString(string key)
	{
		var resources = new MaterialTheme();
		Assert.IsInstanceOfType(resources[key], typeof(string), key);
		Assert.IsInstanceOfType(Microsoft.UI.Xaml.Markup.XamlBindingHelper.ConvertValue(typeof(Geometry), resources[key]), typeof(Geometry), key);
	}

	[TestMethod]
	[RunsOnUIThread]
	[DataRow(ElementTheme.Light, false)]
	[DataRow(ElementTheme.Dark, false)]
	[DataRow(ElementTheme.Light, true)]
	[DataRow(ElementTheme.Dark, true)]
	public async Task When_SelectedRatingIsHovered_Then_SemanticHoverBrushIsUsed(ElementTheme appearance, bool secondary)
	{
		var host = CreateHost(appearance);
		var prefix = secondary ? "SecondaryRatingControl" : "RatingControl";
		host.Resources[prefix + "SelectedForeground"] = new SolidColorBrush(Colors.Blue);
		host.Resources[prefix + "SelectedForegroundPointerOver"] = new SolidColorBrush(Colors.Magenta);
		var rating = new RatingControl { Value = 3, Style = (Style)host.Resources[prefix + "Style"] };
		host.Children.Add(rating);
		try
		{
			UnitTestsUIContentHelper.Content = host;
			await UnitTestsUIContentHelper.WaitForLoaded(rating);
			await UnitTestsUIContentHelper.WaitForIdle();
			var presenter = Find<ContentPresenter>(rating, "ForegroundContentPresenter");
			Assert.IsTrue(VisualStateManager.GoToState(rating, "Set", false));
			await UnitTestsUIContentHelper.WaitForIdle();
			Assert.AreEqual(Colors.Blue, ((SolidColorBrush)presenter.Foreground).Color);
			Assert.IsTrue(VisualStateManager.GoToState(rating, "PointerOverSet", false));
			await UnitTestsUIContentHelper.WaitForIdle();
			Assert.AreEqual(Colors.Magenta, ((SolidColorBrush)presenter.Foreground).Color);
		}
		finally
		{
			UnitTestsUIContentHelper.Content = null;
		}
	}

	[TestMethod]
	[RunsOnUIThread]
	[DataRow(ElementTheme.Light)]
	[DataRow(ElementTheme.Dark)]
	public async Task When_CalendarGlyphBrushIsOverridden_Then_TemplateConsumesNormalAndDisabledKeys(ElementTheme appearance)
	{
		var host = CreateHost(appearance);
		host.Resources["CalendarDatePickerCalendarGlyphForeground"] = new SolidColorBrush(Colors.Magenta);
		host.Resources["CalendarDatePickerCalendarGlyphForegroundDisabled"] = new SolidColorBrush(Colors.Blue);
		var picker = new CalendarDatePicker { Style = (Style)host.Resources["CalendarDatePickerStyle"] };
		host.Children.Add(picker);
		try
		{
			UnitTestsUIContentHelper.Content = host;
			await UnitTestsUIContentHelper.WaitForLoaded(picker);
			await UnitTestsUIContentHelper.WaitForIdle();
			var glyph = Find<FontIcon>(picker, "CalendarGlyph");
			Assert.AreEqual(Colors.Magenta, ((SolidColorBrush)glyph.Foreground).Color);
			picker.IsEnabled = false;
			await UnitTestsUIContentHelper.WaitForIdle();
			Assert.AreEqual(Colors.Blue, ((SolidColorBrush)glyph.Foreground).Color);
			picker.IsEnabled = true;
			await UnitTestsUIContentHelper.WaitForIdle();
			Assert.AreEqual(Colors.Magenta, ((SolidColorBrush)glyph.Foreground).Color);
		}
		finally
		{
			UnitTestsUIContentHelper.Content = null;
		}
	}

	private static Grid CreateHost(ElementTheme appearance)
	{
		var host = new Grid { RequestedTheme = appearance };
		host.Resources.MergedDictionaries.Add(new MaterialTheme());
		return host;
	}

	private static T Find<T>(DependencyObject root, string name) where T : FrameworkElement
	{
		if (root is T element && element.Name == name)
		{
			return element;
		}
		for (var i = 0; i < VisualTreeHelper.GetChildrenCount(root); i++)
		{
			if (FindOrNull<T>(VisualTreeHelper.GetChild(root, i), name) is { } found)
			{
				return found;
			}
		}
		throw new AssertFailedException($"Template part '{name}' ({typeof(T).Name}) was not found.");
	}

	private static T? FindOrNull<T>(DependencyObject root, string name) where T : FrameworkElement
	{
		if (root is T element && element.Name == name)
		{
			return element;
		}
		for (var i = 0; i < VisualTreeHelper.GetChildrenCount(root); i++)
		{
			if (FindOrNull<T>(VisualTreeHelper.GetChild(root, i), name) is { } found)
			{
				return found;
			}
		}
		return null;
	}
}
