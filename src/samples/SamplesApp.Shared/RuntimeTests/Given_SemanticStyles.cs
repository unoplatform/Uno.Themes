using Microsoft.VisualStudio.TestTools.UnitTesting;
using Uno.UI.RuntimeTests;

namespace Uno.Themes.Samples.RuntimeTests;

[TestClass]
public class Given_SemanticStyles
{
	[TestMethod]
	[RunsOnUIThread]
	[DataRow("FilledButtonStyle")]
	[DataRow("FilledTonalButtonStyle")]
	[DataRow("OutlinedButtonStyle")]
	[DataRow("TextButtonStyle")]
	[DataRow("IconButtonStyle")]
	public async Task When_SemanticStyleKey_IsAvailable(string styleKey)
	{
		var style = Application.Current.Resources[styleKey] as Style;
		Assert.IsNotNull(style, $"{styleKey} should be available in application resources");
	}

	[TestMethod]
	[RunsOnUIThread]
	[DataRow("FilledButtonStyle")]
	[DataRow("FilledTonalButtonStyle")]
	[DataRow("TextButtonStyle")]
	public async Task When_SemanticStyle_IsApplied_ButtonIsStyled(string styleKey)
	{
		var style = Application.Current.Resources[styleKey] as Style;
		Assert.IsNotNull(style, $"{styleKey} should be available");

		var button = new Button { Content = "Test", Style = style };
		UnitTestsUIContentHelper.Content = button;
		await UnitTestsUIContentHelper.WaitForLoaded(button);
		await UnitTestsUIContentHelper.WaitForIdle();

		// The button should have a resolved Background from the theme
		Assert.IsNotNull(button.Background, $"Button with {styleKey} should have a Background set by the style");
	}

	[TestMethod]
	[RunsOnUIThread]
	public async Task When_FilledButtonStyle_Applied_HasNonTransparentBackground()
	{
		var style = Application.Current.Resources["FilledButtonStyle"] as Style;
		Assert.IsNotNull(style);

		var button = new Button { Content = "Test", Style = style };
		UnitTestsUIContentHelper.Content = button;
		await UnitTestsUIContentHelper.WaitForLoaded(button);
		await UnitTestsUIContentHelper.WaitForIdle();

		var bg = button.Background as SolidColorBrush;
		Assert.IsNotNull(bg, "FilledButton Background should be a SolidColorBrush");
		Assert.AreNotEqual(Colors.Transparent, bg.Color, "FilledButton Background should not be transparent");
	}

	[TestMethod]
	[RunsOnUIThread]
	[DataRow("FilledButtonForeground")]
	[DataRow("FilledButtonBackground")]
	[DataRow("FilledTonalButtonForeground")]
	[DataRow("FilledTonalButtonBackground")]
	[DataRow("TextButtonForeground")]
	public async Task When_SemanticResourceKey_Exists(string resourceKey)
	{
		var resource = Application.Current.Resources[resourceKey];
		Assert.IsNotNull(resource, $"Semantic resource key '{resourceKey}' should exist in application resources");
	}
}
