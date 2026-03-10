using Microsoft.VisualStudio.TestTools.UnitTesting;
using Uno.Themes.Samples.RuntimeTests.Helpers;

namespace Uno.Themes.Samples.RuntimeTests;

/// <summary>
/// Validates that every Simple theme control XAML file with ThemeDictionaries
/// has the same set of resource keys in both Light and Default (Dark) dictionaries.
/// </summary>
[TestClass]
public class Given_ThemeDictionaryParity
{
	private const string BaseUri = "ms-appx:///Uno.Simple.WinUI/Styles/Controls/";

	[TestMethod]
	[DataRow("AppBarButton.xaml")]
	[DataRow("AutoSuggestBox.xaml")]
	[DataRow("Button.xaml")]
	[DataRow("CalendarDatePicker.xaml")]
	[DataRow("CalendarView.xaml")]
	[DataRow("CheckBox.xaml")]
	[DataRow("ComboBox.xaml")]
	[DataRow("ContentDialog.xaml")]
	[DataRow("DatePicker.xaml")]
	[DataRow("Expander.xaml")]
	[DataRow("ListView.xaml")]
	[DataRow("MenuFlyout.xaml")]
	[DataRow("PasswordBox.xaml")]
	[DataRow("PersonPicture.xaml")]
	[DataRow("RadioButton.xaml")]
	[DataRow("Slider.xaml")]
	[DataRow("TextBlock.xaml")]
	[DataRow("TextBox.xaml")]
	[DataRow("ToggleButton.xaml")]
	[DataRow("ToggleSwitch.xaml")]
	[DataRow("ToolTip.xaml")]
	public void StyleFile_LightAndDark_HaveSameResourceKeys(string fileName)
	{
		ThemeDictionaryTestHelper.AssertThemeParity(BaseUri + fileName);
	}
}
