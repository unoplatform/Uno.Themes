using System;
using System.Threading.Tasks;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Uno.Themes.Samples.RuntimeTests.Helpers;
using Uno.UI.RuntimeTests;

namespace Uno.Themes.Samples.RuntimeTests;

/// <summary>
/// Validates that every Simple theme control type resolves an implicit style
/// when the SimpleTheme is registered in Application.Resources.
/// </summary>
[TestClass]
[RunsOnUIThread]
public class Given_SimpleStyles
{
	[TestMethod]
	[DataRow(typeof(AppBarButton))]
	[DataRow(typeof(AutoSuggestBox))]
	[DataRow(typeof(Button))]
	[DataRow(typeof(CalendarDatePicker))]
	[DataRow(typeof(CalendarView))]
	[DataRow(typeof(CheckBox))]
	[DataRow(typeof(ComboBox))]
	[DataRow(typeof(ContentDialog))]
	[DataRow(typeof(DatePicker))]
	[DataRow(typeof(Expander))]
	[DataRow(typeof(ListView))]
	[DataRow(typeof(PasswordBox))]
	[DataRow(typeof(PersonPicture))]
	[DataRow(typeof(RadioButton))]
	[DataRow(typeof(Slider))]
	[DataRow(typeof(TextBlock))]
	[DataRow(typeof(TextBox))]
	[DataRow(typeof(ToggleButton))]
	[DataRow(typeof(ToggleSwitch))]
	[DataRow(typeof(ToolTip))]
	public async Task Control_HasImplicitStyle(Type controlType)
	{
		var control = (FrameworkElement)Activator.CreateInstance(controlType);
		UnitTestsUIContentHelper.Content = control;
		await UnitTestsUIContentHelper.WaitForIdle();

		Assert.IsNotNull(control.Style,
			$"No implicit style resolved for {controlType.Name}");
	}

	[TestMethod]
	[DataRow("SimplePrimaryButtonStyle", typeof(Button))]
	[DataRow("SimpleNeutralButtonStyle", typeof(Button))]
	[DataRow("SimpleSubtleButtonStyle", typeof(Button))]
	[DataRow("SimpleDangerPrimaryButtonStyle", typeof(Button))]
	[DataRow("SimpleDangerSubtleButtonStyle", typeof(Button))]
	[DataRow("SimpleDefaultExpanderStyle", typeof(Expander))]
	[DataRow("SimpleExpanderStyle", typeof(Expander))]
	[DataRow("SimpleCheckBoxStyle", typeof(CheckBox))]
	[DataRow("SimpleRadioButtonStyle", typeof(RadioButton))]
	[DataRow("SimpleDefaultToggleButtonStyle", typeof(ToggleButton))]
	[DataRow("SimpleToggleSwitchStyle", typeof(ToggleSwitch))]
	[DataRow("SimpleInputTextBoxStyle", typeof(TextBox))]
	[DataRow("SimplePasswordBoxStyle", typeof(PasswordBox))]
	[DataRow("SimpleSliderStyle", typeof(Slider))]
	[DataRow("SimpleSelectFieldStyle", typeof(ComboBox))]
	public async Task NamedStyle_ResolvesAndApplies(string styleKey, Type controlType)
	{
		Assert.IsTrue(
			Application.Current.Resources.TryGetValue(styleKey, out var resource),
			$"Named style '{styleKey}' not found in Application.Current.Resources");

		var style = resource as Style;
		Assert.IsNotNull(style, $"Resource '{styleKey}' is not a Style (got {resource?.GetType().Name})");

		var control = (FrameworkElement)Activator.CreateInstance(controlType);
		control.Style = style;
		UnitTestsUIContentHelper.Content = control;
		await UnitTestsUIContentHelper.WaitForIdle();

		Assert.AreEqual(style, control.Style,
			$"Style '{styleKey}' was not retained after applying to {controlType.Name}");
	}
}
