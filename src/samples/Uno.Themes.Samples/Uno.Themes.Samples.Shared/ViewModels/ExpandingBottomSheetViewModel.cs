using System;
using System.Collections.Generic;
using System.Text;

namespace Uno.Themes.Samples.ViewModels
{
	public class ExpandingBottomSheetViewModel : ViewModelBase
	{
		public string DataTemplateCode { get => GetProperty<string>(); set => SetProperty(value); }
		public string CodeBehindSource { get => GetProperty<string>(); set => SetProperty(value); }
		public bool IsToggled { get => GetProperty<bool>(); set => SetProperty(value); }

		public ExpandingBottomSheetViewModel()
		{
			this.DataTemplateCode = GetCodeBehindSource().Replace("\t", "    ");
			this.CodeBehindSource = GetDataTemplateCodeSource().Replace("\t", "    ");
		}
		private string GetCodeBehindSource()
		{
			return
@"private void ToggleSwitch_Toggled(object sender, RoutedEventArgs e)
{
	if (sender is ToggleSwitch toggleSwitch)
	{
		var value = toggleSwitch.IsOn;
		ExpandingBottomSheet.IsMinimalSheet = value;
	}
}";
		}

		private string GetDataTemplateCodeSource()
		{
			return
@"<Page.Resources>
	<!-- Sample Collapsed Template -->
	<DataTemplate x:Key='CollapsedTemplate'>
		<StackPanel Orientation='Horizontal'
					VerticalAlignment='Center'>
			<Viewbox Height='14'
						 Width='14'>
				<SymbolIcon Symbol='Add'
							Foreground='{ThemeResource ExpandingBottomSheetForegroundBrush}' />
			</Viewbox>

			<TextBlock Text='{Binding}'
					   Style='{ThemeResource MaterialBody2}'
					   Foreground='{ThemeResource ExpandingBottomSheetForegroundBrush}'
					   Margin='8,0,0,0'
					   VerticalAlignment='Center' />
		</StackPanel>
	</DataTemplate>

	<!--Sample Minimal Collapsed Template-- >
	<DataTemplate x:Key='MinimalCollapsedTemplate'> 
		<StackPanel Orientation='Horizontal'
					VerticalAlignment='Center'>

			<Viewbox Height='14'
					 Width='14'>
				<SymbolIcon Symbol='Add'
							Foreground='{ThemeResource ExpandingBottomSheetForegroundBrush}' />
			</Viewbox>
		</StackPanel>
	</DataTemplate >

	<!--Sample Expanded Template -->
	<DataTemplate x:Key='ExpandedTemplate'>
  
		<StackPanel Background='{ThemeResource MaterialBackgroundColor}'
					HorizontalAlignment='Stretch'
					VerticalAlignment='Stretch'>

			<TextBlock Style='{ThemeResource MaterialHeadline6}'
					   Text='Sheet Content'
					   Margin='12' />
		</StackPanel>
	</DataTemplate>
</Page.Resources>";
		}

	}
}
