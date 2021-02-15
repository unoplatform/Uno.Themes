using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace Uno.Themes.Samples.ViewModels
{
	public class ModalBottomSheetViewModel : ViewModelBase
	{
		public string DataTemplateCode { get => GetProperty<string>(); set => SetProperty(value); }
		public string CodeBehindSource { get => GetProperty<string>(); set => SetProperty(value); }
		public bool IsOpened { get => GetProperty<bool>(); set => SetProperty(value); }
		public ICommand OpenCommand => new Command(_ => IsOpened = true);
		public ModalBottomSheetViewModel()
		{
			this.CodeBehindSource = GetCodeBehindSource().Replace("\t", "    ");
			this.DataTemplateCode = GetDataTemplateCodeSource().Replace("\t", "    ");
		}

		private string GetCodeBehindSource()
		{
			return
@"private void Button_Clicked(object sender, RoutedEventArgs e)
{
		ModalStandardBottomSheet.IsOpened = true;
}";
		}

		private string GetDataTemplateCodeSource()
		{
			return
@"<Page.Resources>
	<!-- Sample Content Template -->
	<DataTemplate x:Key'ContentTemplate'>
		<StackPanel Background='{ThemeResource MaterialBackgroundColor}'
					HorizontalAlignment='Stretch'
					VerticalAlignment='Stretch'>
			<TextBlock Style='{ThemeResource MaterialHeadline6}'
					   Text'='Sheet Content'
					   Margin='12' />
			<ListView ItemsSource='{Binding}' />
 
		</StackPanel>
</DataTemplate>
</Page.Resources>";
		}

	}
}
