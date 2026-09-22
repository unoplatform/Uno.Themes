using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;
using Uno.Cupertino;
using Uno.UI.RuntimeTests;
using Windows.UI;

namespace Uno.Themes.Samples.RuntimeTests;

[TestClass]
public class Given_CupertinoFieldBrushes
{
	[TestMethod]
	[RunsOnUIThread]
	[DataRow(ElementTheme.Light)]
	[DataRow(ElementTheme.Dark)]
	public async Task When_RenderedFieldColorChanges_Then_BorderFollowsInBothAppearances(ElementTheme appearance)
	{
		var theme = (CupertinoTheme)Application.Current.GetTheme();
		var original = theme.Colors;
		var field = new TextBox { Text = "Live border", Style = (Style)Application.Current.Resources["CupertinoTextBoxStyle"] };
		var container = new Grid { RequestedTheme = appearance, Children = { field } };
		try
		{
			UnitTestsUIContentHelper.Content = container;
			await UnitTestsUIContentHelper.WaitForLoaded(field);
			var border = (SolidColorBrush)field.BorderBrush;
			var initialColor = border.Color;
			var replacement = Color.FromArgb(255, 18, 52, 86);
			theme.Colors = new ThemeColors { OverrideDictionary = new ResourceDictionary { ["LabelColor"] = replacement } };
			await UnitTestsUIContentHelper.WaitForIdle();
			Assert.AreSame(border, field.BorderBrush);
			Assert.AreEqual(replacement, border.Color);
			Assert.AreEqual(0.2, border.Opacity, 0.001);
			theme.Colors = original;
			await UnitTestsUIContentHelper.WaitForIdle();
			Assert.AreEqual(initialColor, border.Color);
		}
		finally
		{
			theme.Colors = original;
			UnitTestsUIContentHelper.Content = null;
		}
	}

	[TestMethod]
	[RunsOnUIThread]
	[DataRow("CupertinoTextBoxBorderBrush", "LabelColor", 0.2)]
	[DataRow("CupertinoPasswordBoxBorderBrush", "LabelColor", 0.2)]
	[DataRow("CupertinoComboBoxBorderBrush", "LabelColor", 0.2)]
	[DataRow("CupertinoDatePickerBorderBrush", "LabelColor", 0.2)]
	[DataRow("CupertinoDeleteButtonTextBoxBrush", "LabelColor", 0.2)]
	[DataRow("CupertinoHeaderForegroundBrush", "LabelColor", 0.7)]
	[DataRow("CupertinoDetailsLightBrush", "CupertinoPrimaryGrayColor", 0.3)]
	[DataRow("CupertinoCalendarDatePickerBorderBrushPointerOver", "CupertinoPrimaryGrayColor", 0.85)]
	[DataRow("CupertinoCalendarDatePickerBorderBrushPressed", "CupertinoPrimaryGrayColor", 0.4)]
	[DataRow("CupertinoDatePickerFlyoutPresenterHighlightFill", "CupertinoQuinaryGrayColor", 1.0)]
	[DataRow("CupertinoCalendarViewSelectedBackground", "CupertinoBlueColor", 0.27)]
	public void When_FieldColorOverridden_Then_ExistingBrushRepaintsAndKeepsOpacity(string brushKey, string colorKey, double opacity)
	{
		// Aliases resolve through the application scope, as they do in a consumer's App.xaml.
		var theme = (CupertinoTheme)Application.Current.GetTheme();
		var original = theme.Colors;
		var brush = (SolidColorBrush)Application.Current.Resources[brushKey];
		var originalColor = brush.Color;
		var first = Color.FromArgb(255, 18, 52, 86);
		var second = Color.FromArgb(255, 101, 67, 33);
		try
		{
			theme.Colors = new ThemeColors { OverrideDictionary = new ResourceDictionary { [colorKey] = first } };
			Assert.AreEqual(first, brush.Color, brushKey + " first override");
			Assert.AreEqual(opacity, brush.Opacity, 0.001, brushKey + " opacity");
			theme.Colors = new ThemeColors { OverrideDictionary = new ResourceDictionary { [colorKey] = second } };
			Assert.AreSame(brush, Application.Current.Resources[brushKey], "the brush instance must survive");
			Assert.AreEqual(second, brush.Color, brushKey + " replacement override");
		}
		finally
		{
			theme.Colors = original;
		}

		Assert.AreEqual(originalColor, brush.Color, "clearing the override must restore the palette");
	}

	[TestMethod]
	[RunsOnUIThread]
	public void When_CalendarAccentSeedChanges_Then_ExistingSelectionBrushFollows()
	{
		var theme = (CupertinoTheme)Application.Current.GetTheme();
		var original = theme.Colors;
		var brush = (SolidColorBrush)Application.Current.Resources["CupertinoCalendarViewSelectedBackground"];
		var originalColor = brush.Color;
		try
		{
			theme.Colors = new ThemeColors { PrimarySeed = Color.FromArgb(255, 46, 125, 50) };
			Assert.AreNotEqual(originalColor, brush.Color);
			Assert.AreEqual(((SolidColorBrush)Application.Current.Resources["PrimaryBrush"]).Color, brush.Color);
			Assert.AreEqual(0.27, brush.Opacity, 0.001);
		}
		finally
		{
			theme.Colors = original;
		}
		Assert.AreEqual(originalColor, brush.Color);
	}
}
