using System;
using System.Windows.Input;
using Uno.Themes.Samples.Entities;
using Uno.Themes.Samples.ViewModels;
using Windows.UI.Xaml.Controls;

namespace Uno.Themes.Samples.Shared.Content.Extensions
{
	[SamplePage(SampleCategory.Helpers, "ControlExtensions", IconPath = Icons.Helpers.ControlExtensions, DataType = typeof(ControlExtensionsSampleViewModel))]
	public sealed partial class ControlExtensionsSamplePage : Page
	{
		public ControlExtensionsSamplePage()
		{
			this.InitializeComponent();
		}
	}

	public class ControlExtensionsSampleViewModel : ViewModelBase
	{
		public ICommand ShowWarning => new Command(ShowWarningImpl);

		private async void ShowWarningImpl(object parameter)
		{
			var messageDialog = new ContentDialog
			{
				Title = "Command Dialog",
				Content = "A command was fired",

				CloseButtonText = "Ok"
			};
			await messageDialog.ShowAsync();
		}
	}
}
