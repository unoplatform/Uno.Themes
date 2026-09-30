#nullable enable

using System.Linq;
using System.Threading.Tasks;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Automation.Peers;
using Microsoft.UI.Xaml.Automation.Provider;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Uno.Themes.Samples.Content.Controls;
using Uno.Themes.Samples.Content.Styles;
using Uno.Themes.Samples.Helpers;
using Uno.UI.RuntimeTests;

namespace Uno.Themes.Samples.RuntimeTests;

/// <summary>
/// Sample pages resolve their control styles through <c>{StaticResource}</c>. A key the theme does not
/// define is silently dropped on Uno (the control renders unstyled) but is a XAML parse error on WinUI,
/// which takes the WinAppSDK head down as soon as the page is opened.
/// </summary>
[TestClass]
public class Given_SamplePages
{
	[TestMethod]
	[RunsOnUIThread]
	[DataRow("Filled Button")]
	[DataRow("Outlined Button")]
	public async Task When_DesignTokensPageLoaded_Then_PreviewButtonsAreStyled(string content)
	{
		var page = new DesignTokensSamplePage();

		UnitTestsUIContentHelper.Content = page;
		await UnitTestsUIContentHelper.WaitForLoaded(page);
		await UnitTestsUIContentHelper.WaitForIdle();

		var button = page.FindFirstDescendant<Button>(x => content.Equals(x.Content));

		Assert.IsNotNull(button, $"The Design Tokens page should render the '{content}' preview.");
		Assert.IsNotNull(button.Style, $"The '{content}' preview should resolve its Simple style.");
	}

	[TestMethod]
	[RunsOnUIThread]
	[DataRow("BuildSimpleBasicDialog")]
	[DataRow("BuildSimpleConfirmDialog")]
	[DataRow("BuildSimpleThreeButtonDialog")]
	[DataRow("BuildSimpleCustomContentDialog")]
	public async Task When_ContentDialogSampleButtonClicked_Then_TheDialogOpens(string builder)
	{
		// WinUI only shows a ContentDialog that has a XamlRoot; the sample never set one, and its
		// async void click handler let the exception take the WinAppSDK head down.
		var page = new ContentDialogSamplePage();

		UnitTestsUIContentHelper.Content = page;
		await UnitTestsUIContentHelper.WaitForLoaded(page);
		await UnitTestsUIContentHelper.WaitForIdle();

		var button = page.FindFirstDescendant<Button>(x => builder.Equals(x.Tag));
		Assert.IsNotNull(button, $"The ContentDialog page should render the '{builder}' button.");

		((IInvokeProvider)new ButtonAutomationPeer(button).GetPattern(PatternInterface.Invoke)).Invoke();

		ContentDialog? dialog = null;
		for (var attempt = 0; attempt < 30 && dialog is null; attempt++)
		{
			await Task.Delay(100);
			dialog = VisualTreeHelper.GetOpenPopupsForXamlRoot(page.XamlRoot)
				.Select(popup => popup.Child as ContentDialog ?? popup.Child?.FindFirstDescendant<ContentDialog>())
				.FirstOrDefault(x => x is not null);
		}

		try
		{
			Assert.IsNotNull(dialog, $"Clicking '{builder}' should open its ContentDialog.");
		}
		finally
		{
			dialog?.Hide();
			await UnitTestsUIContentHelper.WaitForIdle();
		}
	}

	[TestMethod]
	[RunsOnUIThread]
	public async Task When_SeedColorPageLoaded_Then_TheApplicationThemeTakesItsSeed()
	{
		// The page seeds the application's own theme, so put the app back for the tests that follow.
		using var snapshot = new ThemeSeedSnapshot();
		var colors = snapshot.Colors;

		var page = new SeedColorSamplePage();

		UnitTestsUIContentHelper.Content = page;
		await UnitTestsUIContentHelper.WaitForLoaded(page);
		await UnitTestsUIContentHelper.WaitForIdle();

		Assert.IsNotNull(colors.PrimarySeed, "The Seed Color page should apply its seed to the application theme.");

		// Picking another color rebuilds the live application theme under the rendered page.
		var picked = Windows.UI.Color.FromArgb(0xFF, 0x21, 0x96, 0xF3);
		var picker = page.FindName("SeedColorPicker") as ColorPicker;
		Assert.IsNotNull(picker, "The Seed Color page should render its color picker.");
		picker.Color = picked;
		await UnitTestsUIContentHelper.WaitForIdle();

		Assert.AreEqual(picked, colors.PrimarySeed, "Picking a color should reseed the application theme.");
	}
}
