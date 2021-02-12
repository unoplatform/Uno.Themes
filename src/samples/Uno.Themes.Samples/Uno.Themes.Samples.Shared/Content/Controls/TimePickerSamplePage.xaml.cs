using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Uno.Themes.Samples.Entities;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace Uno.Themes.Samples.Content.Controls
{
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
}
