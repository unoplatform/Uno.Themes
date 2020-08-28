using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Uno.Material.Samples.Content.Controls
{
	/// <summary>
	/// An empty page that can be used on its own or navigated to within a Frame.
	/// </summary>
	public sealed partial class StandardBottomSheetSamplePage : Page
	{
		public StandardBottomSheetSamplePage()
		{
			this.InitializeComponent();
			this.SeeDataTemplateCodeButton.Content = GetDataTemplateCodeSource().Replace("\t", "    ");
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

		private void RadioButton_Default_Checked(object sender, RoutedEventArgs e)
		{
			if (sender is RadioButton radioButton && StandardBottomSheet != null)
			{
				var value = radioButton.IsChecked;

				if (value ?? false)
				{
					StandardBottomSheet.Visibility = Visibility.Visible;
					StandardBottomSheet_3Snaps.Visibility = Visibility.Collapsed;
					StandardBottomSheet_CustomHeader.Visibility = Visibility.Collapsed;
				}
			}
		}

		private void RadioButton_3Snaps_Checked(object sender, RoutedEventArgs e)
		{
			if (sender is RadioButton radioButton && StandardBottomSheet_3Snaps != null)
			{
				var value = radioButton.IsChecked;

				if (value ?? false)
				{
					StandardBottomSheet_3Snaps.Visibility = Visibility.Visible;
					StandardBottomSheet.Visibility = Visibility.Collapsed;
					StandardBottomSheet_CustomHeader.Visibility = Visibility.Collapsed;
				}
			}
		}

		private void RadioButton_CustomHeader_Checked(object sender, RoutedEventArgs e)
		{
			if (sender is RadioButton radioButton && StandardBottomSheet_CustomHeader != null)
			{
				var value = radioButton.IsChecked;

				if (value ?? false)
				{
					StandardBottomSheet_CustomHeader.Visibility = Visibility.Visible;
					StandardBottomSheet.Visibility = Visibility.Collapsed;
					StandardBottomSheet_3Snaps.Visibility = Visibility.Collapsed;
				}
			}
		}
	}
}
