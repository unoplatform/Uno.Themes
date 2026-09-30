namespace Uno.Themes.Samples.Content.Controls;

#if !__WASM__ && !__MACOS__
// Not listed for Material: its only Material content is v1 (Uno.Material v2 has no TimePicker style), and the
// sample app loads v2 only. Uno rendered it unstyled; WinUI fails on the missing v1 keys.
[SamplePage(SampleCategory.Controls, "TimePicker", Description = "This control allows users to pick a time value.", DocumentationLink = "https://docs.microsoft.com/en-us/uwp/api/windows.ui.xaml.controls.timepicker", SupportedDesigns = new[] { Design.Cupertino })]
#endif
public sealed partial class TimePickerSamplePage : Page
{
	public TimePickerSamplePage()
	{
		this.InitializeComponent();
	}
}
