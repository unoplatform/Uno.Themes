namespace Uno.Themes.Samples.Content.Controls;

#if !__WASM__ && !__MACOS__
[SamplePage(SampleCategory.Controls, "TimePicker", Description = "This control allows users to pick a time value.", DocumentationLink = "https://docs.microsoft.com/en-us/uwp/api/windows.ui.xaml.controls.timepicker")]
#endif
public sealed partial class TimePickerSamplePage : Page
{
	public TimePickerSamplePage()
	{
		this.InitializeComponent();
	}
}
