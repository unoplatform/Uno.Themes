namespace Uno.Themes.Samples.Content.Controls;

[SamplePage(SampleCategory.Controls, "DatePicker", Description = "This control allows users to pick a date value.", DocumentationLink = "https://docs.microsoft.com/en-us/uwp/api/windows.ui.xaml.controls.datepicker", SupportedDesigns = new[] { Design.Material, Design.Cupertino })]
public sealed partial class DatePickerSamplePage : Page
{
	public DatePickerSamplePage()
	{
		this.InitializeComponent();
	}
}
