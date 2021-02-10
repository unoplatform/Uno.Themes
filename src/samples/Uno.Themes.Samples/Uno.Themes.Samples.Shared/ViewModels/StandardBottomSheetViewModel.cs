using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Windows.UI.Xaml.Input;

namespace Uno.Themes.Samples.ViewModels
{
	public class StandardBottomSheetViewModel : ViewModelBase
	{
		public string DataTemplateCode { get => GetProperty<string>(); set => SetProperty(value); }
		public bool DefaultSelected { get => GetProperty<bool>(); set => SetProperty(value); }
		public bool SnapsSelected { get => GetProperty<bool>(); set => SetProperty(value); }
		public bool CustomHeaderSelected { get => GetProperty<bool>(); set => SetProperty(value); }

		public ICommand DefaultCommand => new Command(OnDefaultSelected);
		public ICommand SnapsCommand => new Command(On3SnapsSelected);
		public ICommand CustomHeaderCommand => new Command(OnCustomHeaderSelected);


		public StandardBottomSheetViewModel()
		{
			DataTemplateCode = GetDataTemplateCodeSource().Replace("\t", "    ");
		}

		private void OnDefaultSelected(object obj)
		{
			DefaultSelected = true;
			SnapsSelected = false;
			CustomHeaderSelected = false;
		}

		private void On3SnapsSelected(object obj)
		{
			DefaultSelected = false;
			SnapsSelected = true;
			CustomHeaderSelected = false;
		}

		private void OnCustomHeaderSelected(object obj)
		{
			DefaultSelected = false;
			SnapsSelected = false;
			CustomHeaderSelected = true;
		}	

		private string GetDataTemplateCodeSource()
		{
			return
@"<Page.Resources>
	<!-- Sample Content Template -->
	<DataTemplate x:Key='ContentTemplate'>
		<StackPanel Background = '{ThemeResource MaterialBackgroundColor}'>
 
			<TextBlock Style='{ThemeResource MaterialHeadline6}'
					   Text='Sheet Content'
					   Margin='12' />

			<ListView ItemsSource='{Binding}' />
		</StackPanel>
	</DataTemplate>
 
	< !--Sample Header Template -->
	<DataTemplate x:Key='HeaderTemplate'>
		<StackPanel Background='{ThemeResource MaterialPrimaryColor}'
					HorizontalAlignment='Stretch'
					Orientation='Horizontal'>

			<Viewbox Height='14'
					 Width='14'
					 Margin='12'>
				<SymbolIcon Symbol='Sort'
							Foreground='{ThemeResource ExpandingBottomSheetForegroundBrush}' />
			</Viewbox>

			<TextBlock Style='{ThemeResource MaterialHeadline6}'
					   Text='{Binding}'
					   Foreground='{ThemeResource MaterialOnPrimaryColor}'
					   Margin='12' />
		</StackPanel>
	</DataTemplate>
</Page.Resources>";
		}

	}
}
