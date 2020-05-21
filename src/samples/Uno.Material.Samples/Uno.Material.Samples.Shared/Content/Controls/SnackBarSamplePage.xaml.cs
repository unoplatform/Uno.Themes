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

namespace Uno.Material.Samples.Content.Controls
{
	public sealed partial class SnackBarSamplePage : Page
	{
		public SnackBarSamplePage()
		{
			this.InitializeComponent();
		}

		private void ToggleSwitch_Toggled(object sender, RoutedEventArgs e)
		{
			ToggleSwitch toggleSwitch = sender as ToggleSwitch;
			if (toggleSwitch != null)
			{
				if (toggleSwitch.IsOn == true)
				{
					SnackBar_1.SnackBarStatus = SnackBarStatus.Visible;
					SnackBar_2.SnackBarStatus = SnackBarStatus.Visible;
					SnackBar_3.SnackBarStatus = SnackBarStatus.Visible;
				}
				else
				{
					SnackBar_1.SnackBarStatus = SnackBarStatus.Hidden;
					SnackBar_2.SnackBarStatus = SnackBarStatus.Hidden;
					SnackBar_3.SnackBarStatus = SnackBarStatus.Hidden;
				}
			}
		}
	}
}
