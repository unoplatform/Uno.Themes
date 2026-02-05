namespace Uno.Themes.Samples.Content.Controls;

[SamplePage(SampleCategory.Controls, "ContentDialog")]
public sealed partial class ContentDialogSamplePage : Page
{
	private const string SomeText = "Supporting text should be concise and provide clear action instructions to the users.";

	public ContentDialogSamplePage()
	{
		this.InitializeComponent();
	}

	private async void ShowContentDialog(object sender, RoutedEventArgs e)
	{
		var mappings = new Dictionary<string, Func<ContentDialog>>
		{
			[nameof(BuildBasicContentDialog)] = BuildBasicContentDialog,
			[nameof(BuildRichContentDialog)] = BuildRichContentDialog,
		};

		if ((sender as Button)?.Tag is string context && mappings.TryGetValue(context, out var builder))
		{
			var dialog = builder();
			dialog.XamlRoot = this.XamlRoot;
			await dialog.ShowAsync();
		}
	}

	private ContentDialog BuildBasicContentDialog() => new ContentDialog()
	{
		Title = new StackPanel
		{
			Spacing = 18,
			Children =
			{
				CreateHeaderIcon(),
				new ContentControl { Content = "Title" },
			}
		},
		Content = SomeText,
		PrimaryButtonText = "Action",
		CloseButtonText = "Dismiss",
	};

	private ContentDialog BuildRichContentDialog()
	{
		var dialog = new ContentDialog()
		{
			Title = new StackPanel
			{
				Spacing = 18,
				Children =
				{
					CreateHeaderIcon(),
					new ContentControl { Content = "Title" },
				},
			},
			Content = VGridLayout(
				(GridLength.Auto, new TextBlock { Text = SomeText }),
				(new GridLength(1, GridUnitType.Star), new ListView
				{
					ItemsSource = Enumerable.Range(1, 3).Select(x => $"Contact Name {x}"),
					ItemContainerStyle = (Style)this.Resources["CustomSelectableItemStyle"],
					SelectionMode = ListViewSelectionMode.Multiple,
					Margin = new Thickness(0, 24, 0, 0),
				})
			),
			PrimaryButtonText = "Action",
			CloseButtonText = "Dismiss",
		};

		return dialog;
	}

	private static Grid VGridLayout(params (GridLength Height, FrameworkElement Content)[] rows)
	{
		var grid = new Grid();
		foreach (var row in rows)
		{
			grid.RowDefinitions.Add(new RowDefinition { Height = row.Height });
			Grid.SetRow(row.Content, grid.RowDefinitions.Count - 1);
			grid.Children.Add(row.Content);
		}
		return grid;
	}

	private static PathIcon CreateHeaderIcon() => new PathIcon
	{
		Data = (Geometry)Microsoft.UI.Xaml.Markup.XamlBindingHelper.ConvertValue(typeof(Geometry), "M10.7999 8.40002V6.80005H9.19995V8.40002H10.7999ZM10.7999 14.8001V10H9.19995V14.8001H10.7999ZM9.99994 2C5.58398 2 2 5.58398 2 10C2 14.416 5.58398 18 9.99994 18C14.4159 18 18 14.416 18 10C18 5.58398 14.4159 2 9.99994 2ZM10 16.4C6.46802 16.4 3.60001 13.532 3.60001 10C3.60001 6.46802 6.46802 3.60001 10 3.60001C13.532 3.60001 16.4 6.46802 16.4 10C16.4 13.532 13.532 16.4 10 16.4Z"),
		HorizontalAlignment = HorizontalAlignment.Center,
		Width = 24,
		Height = 24,
	};
}
