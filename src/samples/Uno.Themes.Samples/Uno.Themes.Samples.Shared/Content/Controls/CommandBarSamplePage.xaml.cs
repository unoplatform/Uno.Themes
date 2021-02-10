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
using Uno.Themes.Samples.Content.NestedSamples;
using Uno.Themes.Samples.Entities;

namespace Uno.Themes.Samples.Content.Controls
{
	[SamplePage(SampleCategory.Controls, "CommandBar", Description = "This control provides navigation and related actions for the current page.",
		DocumentationLink = "https://docs.microsoft.com/en-us/uwp/api/windows.ui.xaml.controls.commandbar")]
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
