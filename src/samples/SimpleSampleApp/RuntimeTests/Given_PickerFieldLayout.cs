#nullable enable

using System;
using System.Threading.Tasks;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Uno.Simple;
using Uno.UI.RuntimeTests;

namespace Uno.Themes.Samples.RuntimeTests;

/// <summary>
/// Field-layout rules the Simple <see cref="DatePicker"/> and <see cref="TimePicker"/> share:
/// the field sizes to its content, and the <c>Header</c> renders once, as the in-field placeholder.
///
/// Sizing is not only cosmetic: <c>TimePickerFlyout</c> / <c>DatePickerFlyout</c> assign
/// <c>presenter.Width = target.ActualWidth</c> when opening, so a field that stretches to its
/// container produces a flyout stretched to the same width — the picker columns then spread across
/// the whole window instead of reading as a compact picker.
/// </summary>
[TestClass]
public class Given_PickerFieldLayout
{
	private static Grid CreateThemedContainer()
	{
		var container = new Grid();
		container.Resources.MergedDictionaries.Add(new SimpleTheme());
		return container;
	}

	private static FrameworkElement CreatePicker(string styleKey, Grid container, string? header = null)
	{
		FrameworkElement picker = styleKey.Contains("Time")
			? new TimePicker { Header = header }
			: new DatePicker { Header = header };
		picker.Style = container.Resources[styleKey] as Style;
		return picker;
	}

	[TestMethod]
	[RunsOnUIThread]
	[DataRow("SimpleTimePickerStyle", "TimePickerFlyoutPresenterMinWidth")]
	[DataRow("SimpleDatePickerStyle", "DatePickerFlyoutPresenterMinWidth")]
	public async Task When_PlacedInAWideContainer_ThenTheFieldSizesToContent(string styleKey, string minWidthKey)
	{
		var container = CreateThemedContainer();
		container.Width = 900;

		var picker = CreatePicker(styleKey, container);
		container.Children.Add(picker);

		UnitTestsUIContentHelper.Content = container;
		await UnitTestsUIContentHelper.WaitForLoaded(picker);
		await UnitTestsUIContentHelper.WaitForIdle();

		Assert.IsTrue(
			picker.ActualWidth < container.ActualWidth,
			$"{styleKey} should size to content, but it filled {picker.ActualWidth} of {container.ActualWidth}px");

		var minWidth = (double)container.Resources[minWidthKey];
		Assert.IsTrue(
			picker.ActualWidth >= minWidth,
			$"{styleKey} should keep its {minWidth}px floor from {minWidthKey}, but measured {picker.ActualWidth}px");
	}

	[TestMethod]
	[RunsOnUIThread]
	[DataRow("SimpleTimePickerStyle")]
	[DataRow("SimpleDatePickerStyle")]
	public async Task When_HeaderIsSet_ThenItRendersOnce(string styleKey)
	{
		// The header used to render twice — as a label above the field and again as the
		// header-bound placeholder inside it.
		const string header = "Date of Birth";

		var container = CreateThemedContainer();
		var picker = CreatePicker(styleKey, container, header);
		container.Children.Add(picker);

		UnitTestsUIContentHelper.Content = container;
		await UnitTestsUIContentHelper.WaitForLoaded(picker);
		await UnitTestsUIContentHelper.WaitForIdle();

		Assert.AreEqual(
			1,
			CountVisibleTextBlocksWithText(picker, header),
			$"{styleKey} should render '{header}' exactly once, as the placeholder");
	}

	private static int CountVisibleTextBlocksWithText(DependencyObject root, string text)
	{
		var count = 0;
		var children = VisualTreeHelper.GetChildrenCount(root);
		for (var i = 0; i < children; i++)
		{
			var child = VisualTreeHelper.GetChild(root, i);
			if (child is TextBlock { Visibility: Visibility.Visible } textBlock && textBlock.Text == text)
			{
				count++;
			}

			if (child is not FrameworkElement { Visibility: Visibility.Collapsed })
			{
				count += CountVisibleTextBlocksWithText(child, text);
			}
		}

		return count;
	}
}
