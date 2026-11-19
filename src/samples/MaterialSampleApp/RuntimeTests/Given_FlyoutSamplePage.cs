#nullable enable

using System.Linq;
using System.Threading.Tasks;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Media;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Uno.Themes.Samples.Content.Controls;
using Uno.Themes.Samples.Helpers;
using Uno.UI.RuntimeTests;

namespace Uno.Themes.Samples.RuntimeTests;

/// <summary>
/// Opens the Material modal flyouts of the Flyout sample page. Their content only materializes when the
/// flyout opens, so the all-pages smoke test never reaches it.
/// </summary>
[TestClass]
public class Given_FlyoutSamplePage
{
	[TestMethod]
	[RunsOnUIThread]
	[DataRow("ShowModalCenteredFlyoutButton")]
	[DataRow("ShowModalBottomSheetFlyoutButton")]
	public async Task When_ModalFlyoutOpened_Then_ItShows(string buttonName)
	{
		var page = new FlyoutSamplePage();

		UnitTestsUIContentHelper.Content = page;
		await UnitTestsUIContentHelper.WaitForLoaded(page);
		await UnitTestsUIContentHelper.WaitForIdle();

		var button = page.FindFirstDescendant<Button>(x => x.Name == buttonName && x.IsLoaded);
		Assert.IsNotNull(button, $"The Material Flyout sample should render '{buttonName}'.");
		Assert.IsNotNull(button.Flyout, $"'{buttonName}' should carry its flyout.");

		try
		{
			button.Flyout.ShowAt(button);

			var opened = false;
			for (var attempt = 0; attempt < 30 && !opened; attempt++)
			{
				await Task.Delay(100);
				opened = VisualTreeHelper.GetOpenPopupsForXamlRoot(page.XamlRoot)
					.Any(popup => popup.Child is FlyoutPresenter || popup.Child?.FindFirstDescendant<FlyoutPresenter>() is not null);
			}

			Assert.IsTrue(opened, $"The flyout behind '{buttonName}' should open.");
		}
		finally
		{
			button.Flyout.Hide();
			await UnitTestsUIContentHelper.WaitForIdle();
		}
	}
}
