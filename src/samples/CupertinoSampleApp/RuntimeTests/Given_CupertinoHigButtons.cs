using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Uno.UI.RuntimeTests;

namespace Uno.Themes.Samples.RuntimeTests;

[TestClass]
public class Given_CupertinoHigButtons
{
	[TestMethod]
	[DataRow("CupertinoTextToggleButtonStyle")]
	[DataRow("CupertinoCheckBoxStyle")]
	[DataRow("CupertinoRadioButtonStyle")]
	[RunsOnUIThread]
	public async Task When_SelectionLabelIsShort_Then_TouchTargetRemains44Points(string key)
	{
		Microsoft.UI.Xaml.Controls.Primitives.ToggleButton control = key switch
		{
			"CupertinoCheckBoxStyle" => new CheckBox(),
			"CupertinoRadioButtonStyle" => new RadioButton(),
			_ => new Microsoft.UI.Xaml.Controls.Primitives.ToggleButton(),
		};
		control.Content = ".";
		control.Style = (Style)Application.Current.Resources[key];
		control.HorizontalAlignment = HorizontalAlignment.Left;
		control.VerticalAlignment = VerticalAlignment.Top;
		try
		{
			UnitTestsUIContentHelper.Content = control;
			await UnitTestsUIContentHelper.WaitForLoaded(control);
			await UnitTestsUIContentHelper.WaitForIdle();
			Assert.IsTrue(control.ActualWidth >= 44, $"Selection hit target was {control.ActualWidth} px wide");
			Assert.IsTrue(control.ActualHeight >= 44, $"Selection hit target was {control.ActualHeight} px high");
		}
		finally
		{
			UnitTestsUIContentHelper.Content = null;
		}
	}

	// HIG Buttons: a minimum 44 by 44 point hit region, even for a short text label.
	[TestMethod]
	[RunsOnUIThread]
	[DataRow("CupertinoPlainButtonStyle")]
	[DataRow("CupertinoProminentButtonStyle")]
	[DataRow("CupertinoTintedButtonStyle")]
	[DataRow("CupertinoGlassButtonStyle")]
	public async Task When_ButtonLabelIsShort_Then_TouchTargetRemains44Points(string key)
	{
		var button = new Button
		{
			Content = ".",
			Style = (Style)Application.Current.Resources[key],
			HorizontalAlignment = HorizontalAlignment.Left,
			VerticalAlignment = VerticalAlignment.Top,
		};
		try
		{
			UnitTestsUIContentHelper.Content = button;
			await UnitTestsUIContentHelper.WaitForLoaded(button);
			await UnitTestsUIContentHelper.WaitForIdle();
			Assert.IsTrue(button.ActualWidth >= 44, $"Short label hit target was {button.ActualWidth} px wide");
			Assert.IsTrue(button.ActualHeight >= 44, $"Short label hit target was {button.ActualHeight} px high");
		}
		finally
		{
			UnitTestsUIContentHelper.Content = null;
		}
	}
}
