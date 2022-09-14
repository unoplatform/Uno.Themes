using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Uno.Themes.Samples.Entities;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Uno.Themes.Samples.Content.Controls
{
	[SamplePage(
		SampleCategory.Controls,
		nameof(ContentDialog),
		Description = "Represents a dialog box that can be customized to contain checkboxes, hyperlinks, buttons and any other XAML content.",
		DocumentationLink = "https://docs.microsoft.com/en-us/uwp/api/Windows.UI.Xaml.Controls.ContentDialog"
	)]
	public sealed partial class ContentDialogSamplePage : Page
	{
		public ContentDialogSamplePage()
		{
			this.InitializeComponent();
		}

		private async void ShowContentDialog(object sender, RoutedEventArgs e)
		{
			var mappings = new Dictionary<string, Func<ContentDialog>>
			{
				["Alert"] = BuildAlertContentDialog,
				["Simple"] = BuildSimpleContentDialog,
				["Confirmation"] = BuildConfirmationContentDialog,
			};

			if ((sender as Button)?.Tag is string context && mappings.TryGetValue(context, out var builder))
			{
				var dialog = builder();

				await dialog.ShowAsync();
			}
			else
			{
				throw new InvalidOperationException();
			}
		}

		private ContentDialog BuildAlertContentDialog() => new ContentDialog()
		{
			Title = "Dialog header",
			Content = "Dialog body text",

			PrimaryButtonText = "OK",
			CloseButtonText = "CANCEL",
		};

		private ContentDialog BuildSimpleContentDialog()
		{
			var list = new ListView
			{
				ItemsSource = Enumerable.Range(1, 3).Select(x => $"Item {x}"),
				IsItemClickEnabled = true,
			};
			var dialog = new ContentDialog()
			{
				Title = "Dialog header",
				Content = list,
			};
			list.ItemClick += (s, e) => dialog.Hide();

			return dialog;
		}

		private ContentDialog BuildConfirmationContentDialog()
		{
			var list = new ListView
			{
				ItemsSource = Enumerable.Range(1, 3).Select(x => $"Item {x}"),
			};
			var dialog = new ContentDialog()
			{
				Title = "Dialog header",
				Content = list,
				
				PrimaryButtonText = "OK",
				IsPrimaryButtonEnabled = false,
				CloseButtonText = "CANCEL",
			};
			list.SelectionChanged += (s, e) => dialog.IsPrimaryButtonEnabled = list.SelectedIndex != -1;

			return dialog;
		}
	}
}
