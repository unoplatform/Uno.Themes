using System.Threading.Tasks;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Media;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Uno.Themes.Samples.Helpers;
using Uno.Themes.Samples.RuntimeTests.Helpers;
using Uno.UI.RuntimeTests;

namespace Uno.Themes.Samples.RuntimeTests;

/// <summary>
/// Deep tests for Simple theme Expander: template parts, theme resources, visual states.
/// </summary>
[TestClass]
[RunsOnUIThread]
public class Given_ExpanderStyle
{
	// --- Template Parts ---

	[TestMethod]
	public async Task ExpanderHeader_TemplatePart_Exists()
	{
		var expander = new Expander { Header = "Test", Content = new TextBlock { Text = "Content" } };
		await StyleTestHelper.LoadAndWait(expander);

		Assert.IsNotNull(
			StyleTestHelper.FindTemplatePart<ToggleButton>(expander, "ExpanderHeader"),
			"ExpanderHeader template part missing");
	}

	[TestMethod]
	public async Task ExpanderContent_TemplatePart_Exists()
	{
		var expander = new Expander { Header = "Test", Content = new TextBlock { Text = "Content" } };
		await StyleTestHelper.LoadAndWait(expander);

		Assert.IsNotNull(
			StyleTestHelper.FindTemplatePart<Border>(expander, "ExpanderContent"),
			"ExpanderContent template part missing");
	}

	[TestMethod]
	public async Task ExpanderContentClip_TemplatePart_Exists()
	{
		var expander = new Expander { Header = "Test", Content = new TextBlock { Text = "Content" } };
		await StyleTestHelper.LoadAndWait(expander);

		Assert.IsNotNull(
			StyleTestHelper.FindTemplatePart<Border>(expander, "ExpanderContentClip"),
			"ExpanderContentClip template part missing");
	}

	[TestMethod]
	public async Task ExpandCollapseChevron_TemplatePart_Exists()
	{
		var expander = new Expander { Header = "Test" };
		await StyleTestHelper.LoadAndWait(expander);

		var header = StyleTestHelper.FindTemplatePart<ToggleButton>(expander, "ExpanderHeader");
		Assert.IsNotNull(header, "ExpanderHeader not found");

		Assert.IsNotNull(
			StyleTestHelper.FindTemplatePart<FontIcon>(header, "ExpandCollapseChevron"),
			"ExpandCollapseChevron template part missing inside ExpanderHeader");
	}

	// --- Theme Resources ---

	[TestMethod]
	[DataRow("SimpleExpanderHeaderBackground")]
	[DataRow("SimpleExpanderHeaderForeground")]
	[DataRow("SimpleExpanderChevronForeground")]
	[DataRow("SimpleExpanderContentBackground")]
	[DataRow("SimpleExpanderContentBorderBrush")]
	[DataRow("SimpleExpanderHeaderBorderBrush")]
	[DataRow("SimpleExpanderHeaderExpandedBackground")]
	[DataRow("SimpleExpanderHeaderExpandedForeground")]
	[DataRow("SimpleExpanderHeaderExpandedBorderBrush")]
	[DataRow("SimpleExpanderHeaderBackgroundPointerOver")]
	[DataRow("SimpleExpanderHeaderBackgroundPressed")]
	[DataRow("SimpleExpanderHeaderBackgroundDisabled")]
	[DataRow("SimpleExpanderChevronForegroundPointerOver")]
	[DataRow("SimpleExpanderChevronForegroundPressed")]
	[DataRow("SimpleExpanderChevronForegroundDisabled")]
	public void Expander_ThemeResource_ResolvesToBrush(string resourceKey)
	{
		StyleTestHelper.AssertBrushResource(resourceKey);
	}

	// --- Visual State: Expand/Collapse ---

	[TestMethod]
	public async Task Expander_WhenExpanded_ChevronRotates()
	{
		var expander = new Expander { Header = "Test", IsExpanded = false };
		await StyleTestHelper.LoadAndWait(expander);

		var header = StyleTestHelper.FindTemplatePart<ToggleButton>(expander, "ExpanderHeader");
		var chevron = StyleTestHelper.FindTemplatePart<FontIcon>(header, "ExpandCollapseChevron");
		Assert.IsNotNull(chevron, "ExpandCollapseChevron not found");

		var transform = chevron.RenderTransform as RotateTransform;
		Assert.IsNotNull(transform, "Chevron RenderTransform is not a RotateTransform");
		Assert.AreEqual(0d, transform.Angle, "Chevron should be 0° when collapsed");

		expander.IsExpanded = true;
		await UnitTestsUIContentHelper.WaitForIdle();

		Assert.AreEqual(180d, transform.Angle, "Chevron should be 180° when expanded");
	}

	[TestMethod]
	public async Task Expander_ContentArea_IsCollapsedByDefault()
	{
		var expander = new Expander { Header = "Test", Content = new TextBlock { Text = "Body" } };
		await StyleTestHelper.LoadAndWait(expander);

		var content = StyleTestHelper.FindTemplatePart<Border>(expander, "ExpanderContent");
		Assert.IsNotNull(content, "ExpanderContent template part missing");
		Assert.AreEqual(Visibility.Collapsed, content.Visibility,
			"ExpanderContent should be collapsed by default");
	}

	[TestMethod]
	public async Task Expander_WhenExpanded_ContentBecomesVisible()
	{
		var expander = new Expander { Header = "Test", Content = new TextBlock { Text = "Body" }, IsExpanded = false };
		await StyleTestHelper.LoadAndWait(expander);

		expander.IsExpanded = true;
		await UnitTestsUIContentHelper.WaitForIdle();

		var content = StyleTestHelper.FindTemplatePart<Border>(expander, "ExpanderContent");
		Assert.IsNotNull(content, "ExpanderContent template part missing");
		Assert.AreEqual(Visibility.Visible, content.Visibility,
			"ExpanderContent should be visible when expanded");
	}
}
