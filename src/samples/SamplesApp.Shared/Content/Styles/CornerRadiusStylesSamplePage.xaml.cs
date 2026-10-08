using Uno.Themes.Samples.Helpers;

namespace Uno.Themes.Samples.Content.Styles;

[SamplePage(
	SampleCategory.Styles,
	"Corner Radius Styles",
	Description = "Every style whose corner radius comes from the shape scale, directly or through a control alias. Change DefaultCornerRadius on the theme to see them all respond.",
	SortOrder = 26,
	SupportedDesigns = new[] { Design.Material, Design.Simple })]
public sealed partial class CornerRadiusStylesSamplePage : Page
{
	public CornerRadiusStylesSamplePage()
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
		Content = IsMaterial ? "CornerRadius = Radius700" : "CornerRadius = Radius200",
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
