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
	[SamplePage(SampleCategory.Controls, "FAB", SourceSdk.UnoMaterial, Description = "Also known as Floating Action Button, the FAB is used for a screen's primary action.", DocumentationLink = "https://material.io/components/buttons-floating-action-button")]
	public sealed partial class FabSamplePage : Page
	{
		public FabSamplePage()
		{
			this.InitializeComponent();
		}
	}
}
