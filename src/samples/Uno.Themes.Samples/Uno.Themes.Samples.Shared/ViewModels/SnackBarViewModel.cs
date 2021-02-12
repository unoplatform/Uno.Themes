using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace Uno.Themes.Samples.ViewModels
{
	public class SnackBarViewModel : ViewModelBase
	{
		public string DataTemplateCode { get => GetProperty<string>(); set => SetProperty(value); }
		public bool IsVisible { get => GetProperty<bool>(); set => SetProperty(value); }

		public SnackBarViewModel()
		{
			DataTemplateCode = GetDataTemplateCodeSource().Replace("\t", "    ");
		}

		private string GetDataTemplateCodeSource()
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
	}
}
