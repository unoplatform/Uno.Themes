using System;
using System.Windows.Input;
using static Uno.Themes.Samples.ViewModels.ViewModelBase;
using Windows.UI.Xaml.Controls;

namespace Uno.Themes.Samples.Entities.Data
{
    public class TestCommands
    {
		public ICommand MyCommand { get; private set; }

		public TestCommands()
		{
			MyCommand = new Command(ExecuteMyCommand);
		}

		private async void ShowWarning()
		{
			var messageDialog = new ContentDialog
			{
				Title = "Command Dialog",
				Content = "A command was fired",

				CloseButtonText = "Ok"
			};
			await messageDialog.ShowAsync();
		}

		private void ExecuteMyCommand(object parameter)
		{
			ShowWarning();
		}
	}
}
