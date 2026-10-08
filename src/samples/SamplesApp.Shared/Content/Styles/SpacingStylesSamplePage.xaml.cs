using Uno.Themes.Samples.Helpers;

namespace Uno.Themes.Samples.Content.Styles;

[SamplePage(
	SampleCategory.Styles,
	"Spacing Styles",
	Description = "Every style that reads the spacing scale, directly or through a control alias. Change DefaultSpacing on the theme to see them all respond.",
	SortOrder = 25,
	SupportedDesigns = new[] { Design.Material, Design.Simple })]
public sealed partial class SpacingStylesSamplePage : Page
{
	public SpacingStylesSamplePage()
	{
		this.InitializeComponent();
	}

	private static bool IsMaterial => SamplePageLayout.ActiveDesign == Design.Material;

	/// <summary>
	/// Builds a dialog styled with the active design's ContentDialog style.
	/// </summary>
	/// <param name="xamlRoot">The root the dialog is shown in.</param>
	/// <returns>The configured, not yet shown, dialog.</returns>
	private static ContentDialog CreateSampleDialog(XamlRoot xamlRoot) => new()
	{
		XamlRoot = xamlRoot,
		Title = "ContentDialog",
		Content = IsMaterial ? "PanelPadding = Space600Thickness" : "PanelPadding = Space800Thickness",
		PrimaryButtonText = "Primary",
		CloseButtonText = "Close",
		Style = (Style)(SampleThemeHelper.CurrentApplication ?? Application.Current).Resources[IsMaterial ? "MaterialContentDialogStyle" : "SimpleContentDialogStyle"],
	};

	private async void OnShowContentDialogClick(object sender, RoutedEventArgs e)
	{
		try
		{
			await CreateSampleDialog(XamlRoot).ShowAsync();
		}
		catch (Exception ex)
		{
			System.Diagnostics.Debug.WriteLine($"Failed to show the sample ContentDialog: {ex}");
		}
	}
}
