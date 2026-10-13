namespace Uno.Themes.Samples.Content.Controls;

[SamplePage(SampleCategory.Helpers, "Input Validation", Description = "TextBox, PasswordBox, AutoSuggestBox and ComboBox presenting INotifyDataErrorInfo errors through the theme's validation visual states.", SupportedDesigns = new[] { Design.Simple })]
public sealed partial class InputValidationSamplePage : Page
{
	public InputValidationSamplePage()
	{
		this.InitializeComponent();
	}
}
