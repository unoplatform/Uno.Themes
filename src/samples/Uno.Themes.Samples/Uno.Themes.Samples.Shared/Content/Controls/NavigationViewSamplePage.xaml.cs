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

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Uno.Themes.Samples.Content.Controls
{
	/// <summary>
	/// An empty page that can be used on its own or navigated to within a Frame.
	/// </summary>
	[SamplePage(SampleCategory.Controls, "Navigation View", DataType = typeof(NavigationViewViewModel))]
	public sealed partial class NavigationViewSamplePage : Page
	{
		public NavigationViewSamplePage()
		{
			this.InitializeComponent();
		}
	}
}
