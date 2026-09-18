#nullable enable

using System;
using System.Threading.Tasks;
using Microsoft.UI.Xaml.Automation;
using Microsoft.UI.Xaml.Controls;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Uno.Themes.Samples.Content.NestedSamples;
using Uno.Themes.Samples.Helpers;
using Uno.UI.RuntimeTests;

namespace Uno.Themes.Samples.RuntimeTests;

/// <summary>
/// Nested sample pages are hosted full-screen by <see cref="Shell"/>'s NestedSampleFrame, on top of
/// the navigation view. Their in-page back button is the only way out on platforms with no system
/// back affordance (Skia desktop, Windows), so a page that renders none traps the user.
///
/// Regression guard: these pages used to declare the button through
/// <c>CommandBarExtensions.NavigationCommand</c>, a slot that only exists in the Material v1
/// CommandBar template. Under v2 (the default) it rendered nothing at all.
/// </summary>
[TestClass]
public class Given_NestedSamplePages
{
	private const string BackButtonAutomationId = "NestedSampleBackButton";

	[TestMethod]
	[RunsOnUIThread]
	[DataRow(typeof(MediaPlayerElementSample_NestedPage1))]
	[DataRow(typeof(MediaPlayerElementSample_NestedPage2))]
	[DataRow(typeof(MediaPlayerElementSample_NestedPage3))]
	[DataRow(typeof(MediaPlayerElementSample_NestedPage4))]
	[DataRow(typeof(MediaPlayerElementSample_NestedPage5))]
	public async Task When_NestedSamplePageLoaded_Then_BackButtonIsRendered(Type pageType)
	{
		var page = (Page)Activator.CreateInstance(pageType)!;

		UnitTestsUIContentHelper.Content = page;
		await UnitTestsUIContentHelper.WaitForLoaded(page);
		await UnitTestsUIContentHelper.WaitForIdle();

		var backButton = page.FindFirstDescendant<AppBarButton>(
			x => AutomationProperties.GetAutomationId(x) == BackButtonAutomationId);

		Assert.IsNotNull(backButton, $"{pageType.Name} should render a back button to exit nested navigation.");
		Assert.AreEqual(Visibility.Visible, backButton.Visibility, $"{pageType.Name}'s back button should be visible.");
		Assert.IsTrue(backButton.ActualWidth > 0 && backButton.ActualHeight > 0,
			$"{pageType.Name}'s back button should be laid out (was {backButton.ActualWidth}x{backButton.ActualHeight}).");
	}
}
