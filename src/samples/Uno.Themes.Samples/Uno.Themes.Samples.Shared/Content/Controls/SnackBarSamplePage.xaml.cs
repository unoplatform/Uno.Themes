using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Windows.Input;
using Uno.Material.Controls;
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
	[SamplePage(SampleCategory.Controls, "Snack Bar", SourceSdk.UnoMaterial, DataType = typeof(SnackBarViewModel))]
	public sealed partial class SnackBarSamplePage : Page
	{
		public SnackBarSamplePage()
		{
			this.InitializeComponent();
		}
	}
}
