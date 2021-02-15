using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Uno.Themes.Samples.Entities;
using Uno.Themes.Samples.ViewModels;
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
	[SamplePage(SampleCategory.Controls,
		"Expanding Bottom Sheet",
		SourceSdk.UnoMaterial,
		DataType = typeof(ExpandingBottomSheetViewModel),
		Description = "This control allows users to toggle optional page content.",
		DocumentationLink = "https://material.io/components/sheets-bottom#expanding-bottom-sheet")]
	public sealed partial class ExpandingBottomSheetSamplePage : Page
	{
		public ExpandingBottomSheetSamplePage()
		{
			this.InitializeComponent();
		}
	}
}
