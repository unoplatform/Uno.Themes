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
	[SamplePage(SampleCategory.Controls, "HyperlinkButton", Description = "Represents a button control that functions as a hyperlink.", DocumentationLink = "https://docs.microsoft.com/en-us/uwp/api/windows.ui.xaml.controls.hyperlinkbutton")]
	public sealed partial class HyperlinkButtonSamplePage : Page
	{
		public HyperlinkButtonSamplePage()
		{
			this.InitializeComponent();
		}
	}
}
