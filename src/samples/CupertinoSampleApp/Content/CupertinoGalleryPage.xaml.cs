namespace Uno.Themes.Samples.Content;

[SamplePage(
	SampleCategory.Styles,
	"Cupertino Gallery",
	Description = "Everyday iOS and iPadOS controls in context, including settings, forms, actions and navigation.",
	SortOrder = 0,
	SupportedDesigns = new[] { Design.Cupertino })]
public sealed partial class CupertinoGalleryPage : Page
{
	public CupertinoGalleryPage()
	{
		InitializeComponent();
	}

	private async void ShowAlert(object sender, RoutedEventArgs args)
	{
		try
		{
			var dialog = new ContentDialog
			{
				Title = "Save your changes?",
				Content = "Your preferences will be available the next time you open the app.",
				PrimaryButtonText = "Save",
				CloseButtonText = "Cancel",
				DefaultButton = ContentDialogButton.Primary,
				XamlRoot = XamlRoot,
			};
			await dialog.ShowAsync();
		}
		catch (InvalidOperationException exception)
		{
			Console.Error.WriteLine($"Unable to open the gallery alert: {exception}");
		}
		catch (Exception exception)
		{
			Console.Error.WriteLine($"Gallery alert failed: {exception}");
		}
	}
}
