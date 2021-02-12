using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Uno.Themes.Samples.Entities;
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
	[SamplePage(SampleCategory.Controls, "Expanding Bottom Sheet")]
	public sealed partial class ExpandingBottomSheetSamplePage : Page
	{
		public ExpandingBottomSheetSamplePage()
		{
			this.InitializeComponent();
			this.SeeCodeBehindButton.Content = GetCodeBehindSource().Replace("\t", "    ");
			this.SeeDataTemplateCodeButton.Content = GetDataTemplateCodeSource().Replace("\t", "    ");
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

		private void ToggleSwitch_Toggled(object sender, RoutedEventArgs e)
		{
			if (sender is ToggleSwitch toggleSwitch)
			{
				var value = toggleSwitch.IsOn;
				ExpandingBottomSheet.IsMinimalSheet = value;
			}
		}
	}
}
