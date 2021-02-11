using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Windows.Input;
using Uno.Material.Controls;
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
	public sealed partial class SnackBarSamplePage : Page
	{
		public SnackBarSamplePage()
		{
			this.InitializeComponent();
			this.SeeCodeBehindButton.Content = GetCodeBehindSource().Replace("\t", "    ");
		}

		private string GetCodeBehindSource()
		{
			return
@"private void ToggleSwitch_Toggled(object sender, RoutedEventArgs e)
{
	if (sender is ToggleSwitch toggleSwitch)
	{
		var value = toggleSwitch.IsOn ? SnackBarStatus.Visible : SnackBarStatus.Hidden;

		SnackBar_1.SnackBarStatus = value;
		SnackBar_2.SnackBarStatus = value;
		SnackBar_3.SnackBarStatus = value;
	}
}";
		}

		private void ToggleSwitch_Toggled(object sender, RoutedEventArgs e)
		{
			if (sender is ToggleSwitch toggleSwitch)
			{
				var value = toggleSwitch.IsOn ? SnackBarStatus.Visible : SnackBarStatus.Hidden;

				SnackBar_1.SnackBarStatus = value;
				SnackBar_2.SnackBarStatus = value;
				SnackBar_3.SnackBarStatus = value;
			}
		}
	}
}
