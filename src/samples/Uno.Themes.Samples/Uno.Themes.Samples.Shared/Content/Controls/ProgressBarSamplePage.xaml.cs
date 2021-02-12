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
	[SamplePage(SampleCategory.Controls, "Progress Bar", Description = "This control is used to display progress.", DocumentationLink = "https://docs.microsoft.com/en-us/uwp/api/microsoft.ui.xaml.controls.progressbar")]
	public sealed partial class ProgressBarSamplePage : Page
	{
		public ProgressBarSamplePage()
		{
			this.InitializeComponent();
		}
	}
}
