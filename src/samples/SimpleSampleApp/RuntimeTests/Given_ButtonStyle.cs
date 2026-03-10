using System;
using System.Threading.Tasks;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Uno.Themes.Samples.Helpers;
using Uno.Themes.Samples.RuntimeTests.Helpers;
using Uno.UI.RuntimeTests;

namespace Uno.Themes.Samples.RuntimeTests;

/// <summary>
/// Tests for Simple theme Button: named variant resolution, template parts, visual states.
/// </summary>
[TestClass]
[RunsOnUIThread]
public class Given_ButtonStyle
{
	// --- Named Variant Resolution ---

	[TestMethod]
	[DataRow("SimplePrimaryButtonStyle")]
	[DataRow("SimpleNeutralButtonStyle")]
	[DataRow("SimpleSubtleButtonStyle")]
	[DataRow("SimpleDangerPrimaryButtonStyle")]
	[DataRow("SimpleDangerSubtleButtonStyle")]
	public void ButtonVariant_StyleExists(string styleKey)
	{
		Assert.IsTrue(
			Application.Current.Resources.TryGetValue(styleKey, out var resource),
			$"Button variant style '{styleKey}' not found");
		Assert.IsInstanceOfType(resource, typeof(Style),
			$"Resource '{styleKey}' is not a Style");
	}

	[TestMethod]
	[DataRow("SimpleSmallPrimaryButtonStyle")]
	[DataRow("SimpleSmallNeutralButtonStyle")]
	[DataRow("SimpleSmallSubtleButtonStyle")]
	[DataRow("SimpleSmallDangerPrimaryButtonStyle")]
	[DataRow("SimpleSmallDangerSubtleButtonStyle")]
	[DataRow("SimpleMediumPrimaryButtonStyle")]
	[DataRow("SimpleMediumNeutralButtonStyle")]
	[DataRow("SimpleMediumSubtleButtonStyle")]
	[DataRow("SimpleMediumDangerPrimaryButtonStyle")]
	[DataRow("SimpleMediumDangerSubtleButtonStyle")]
	public void ButtonSizeVariant_StyleExists(string styleKey)
	{
		Assert.IsTrue(
			Application.Current.Resources.TryGetValue(styleKey, out var resource),
			$"Button size variant style '{styleKey}' not found");
		Assert.IsInstanceOfType(resource, typeof(Style),
			$"Resource '{styleKey}' is not a Style");
	}

	// --- Theme-agnostic aliases ---

	[TestMethod]
	[DataRow("PrimaryButtonStyle")]
	[DataRow("NeutralButtonStyle")]
	[DataRow("SubtleButtonStyle")]
	public void ButtonAlias_ResolvesToStyle(string aliasKey)
	{
		Assert.IsTrue(
			Application.Current.Resources.TryGetValue(aliasKey, out var resource),
			$"Theme-agnostic alias '{aliasKey}' not found");
		Assert.IsInstanceOfType(resource, typeof(Style),
			$"Alias '{aliasKey}' did not resolve to a Style");
	}

	// --- Template Parts ---

	[TestMethod]
	[DataRow("SimplePrimaryButtonStyle")]
	[DataRow("SimpleNeutralButtonStyle")]
	[DataRow("SimpleSubtleButtonStyle")]
	[DataRow("SimpleDangerPrimaryButtonStyle")]
	[DataRow("SimpleDangerSubtleButtonStyle")]
	public async Task ButtonVariant_HasRootGrid(string styleKey)
	{
		Application.Current.Resources.TryGetValue(styleKey, out var resource);
		var style = (Style)resource;

		var button = new Button { Content = "Test", Style = style };
		await StyleTestHelper.LoadAndWait(button);

		Assert.IsNotNull(
			StyleTestHelper.FindTemplatePart<Grid>(button, "Root"),
			$"Root Grid template part missing in {styleKey}");
	}

	[TestMethod]
	[DataRow("SimplePrimaryButtonStyle")]
	[DataRow("SimpleNeutralButtonStyle")]
	[DataRow("SimpleSubtleButtonStyle")]
	public async Task ButtonVariant_HasContentPresenter(string styleKey)
	{
		Application.Current.Resources.TryGetValue(styleKey, out var resource);
		var style = (Style)resource;

		var button = new Button { Content = "Test", Style = style };
		await StyleTestHelper.LoadAndWait(button);

		Assert.IsNotNull(
			StyleTestHelper.FindTemplatePart<ContentPresenter>(button, "ContentPresenter"),
			$"ContentPresenter template part missing in {styleKey}");
	}

	// --- Theme Resource Brushes ---

	[TestMethod]
	[DataRow("SimplePrimaryButtonForeground")]
	[DataRow("SimplePrimaryButtonBackground")]
	[DataRow("SimplePrimaryButtonBorderBrush")]
	[DataRow("SimplePrimaryButtonForegroundPointerOver")]
	[DataRow("SimplePrimaryButtonBackgroundPointerOver")]
	[DataRow("SimplePrimaryButtonForegroundPressed")]
	[DataRow("SimplePrimaryButtonBackgroundPressed")]
	[DataRow("SimplePrimaryButtonForegroundDisabled")]
	[DataRow("SimplePrimaryButtonBackgroundDisabled")]
	[DataRow("SimpleNeutralButtonForeground")]
	[DataRow("SimpleNeutralButtonBackground")]
	[DataRow("SimpleNeutralButtonBorderBrush")]
	[DataRow("SimpleSubtleButtonForeground")]
	[DataRow("SimpleSubtleButtonBackground")]
	[DataRow("SimpleDangerPrimaryButtonForeground")]
	[DataRow("SimpleDangerPrimaryButtonBackground")]
	[DataRow("SimpleDangerSubtleButtonForeground")]
	[DataRow("SimpleDangerSubtleButtonBackground")]
	public void Button_ThemeResource_ResolvesToBrush(string resourceKey)
	{
		StyleTestHelper.AssertBrushResource(resourceKey);
	}
}
