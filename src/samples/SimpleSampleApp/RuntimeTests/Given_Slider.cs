#nullable enable

using System.Threading.Tasks;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Uno.Simple;
using Uno.Themes.Samples.Helpers;
using Uno.UI.RuntimeTests;

namespace Uno.Themes.Samples.RuntimeTests;

/// <summary>
/// Guards the Simple Slider template's content margins. <c>SliderPreContentMargin</c> and
/// <c>SliderPostContentMargin</c> are <c>x:Double</c> (matching WinUI) but fill <c>GridLength</c> rows
/// and columns. Only <c>{ThemeResource}</c> converts between the two on WinUI; the template used
/// <c>{StaticResource}</c>, which Uno tolerates but which failed the template on WinAppSDK and took
/// the app down as soon as any Simple Slider was laid out.
/// </summary>
[TestClass]
public class Given_Slider
{
	[TestMethod]
	[RunsOnUIThread]
	[DataRow(Orientation.Horizontal)]
	[DataRow(Orientation.Vertical)]
	public async Task When_SimpleSliderLoaded_Then_ContentMarginsAreApplied(Orientation orientation)
	{
		var container = new Grid();
		container.Resources.MergedDictionaries.Add(new SimpleTheme());
		var slider = new Slider
		{
			Orientation = orientation,
			Style = container.Resources["SimpleSliderStyle"] as Style,
			Width = 200,
			Height = 200,
		};
		container.Children.Add(slider);

		UnitTestsUIContentHelper.Content = container;
		await UnitTestsUIContentHelper.WaitForLoaded(slider);
		await UnitTestsUIContentHelper.WaitForIdle();

		var templateName = orientation == Orientation.Horizontal ? "HorizontalTemplate" : "VerticalTemplate";
		var template = slider.FindFirstDescendant<Grid>(x => x.Name == templateName);
		Assert.IsNotNull(template, $"The Simple Slider should render its {templateName}.");

		var (pre, post) = orientation == Orientation.Horizontal
			? (template.RowDefinitions[0].Height, template.RowDefinitions[2].Height)
			: (template.ColumnDefinitions[0].Width, template.ColumnDefinitions[2].Width);
		Assert.AreEqual(new GridLength(14), pre, "SliderPreContentMargin should size the leading track margin.");
		Assert.AreEqual(new GridLength(14), post, "SliderPostContentMargin should size the trailing track margin.");
	}
}
