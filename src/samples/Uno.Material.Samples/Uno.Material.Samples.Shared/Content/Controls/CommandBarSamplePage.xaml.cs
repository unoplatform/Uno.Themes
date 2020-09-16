using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Uno.Material.Samples.Content.NestedSamples;

namespace Uno.Material.Samples.Content.Controls
{
	public sealed partial class CommandBarSamplePage : Page
	{
		public CommandBarSamplePage()
		{
			this.InitializeComponent();
		}
		private void ShowSampleInNestedFrame(object sender, RoutedEventArgs e)
		{
			SamplesPage.GetForCurrentView()?.ShowNestedSample<CommandBarSample_NestedPage1>(clearStack: true);
		}
	}
}
