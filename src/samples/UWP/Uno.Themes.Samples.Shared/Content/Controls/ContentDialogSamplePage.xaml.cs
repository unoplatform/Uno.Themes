namespace Uno.Themes.Samples.Content.Controls;

[SamplePage(
	SampleCategory.Controls,
	nameof(ContentDialog),
	Description = "Represents a dialog box that can be customized to contain checkboxes, hyperlinks, buttons and any other XAML content.",
	DocumentationLink = "https://docs.microsoft.com/en-us/uwp/api/Windows.UI.Xaml.Controls.ContentDialog"
)]
public sealed partial class ContentDialogSamplePage : Page
{
	private const string SomeText = "Supporting text should be consice and provide clear action instructions to the users.";

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

			await dialog.ShowAsync();
		}
		else
		{
			throw new KeyNotFoundException($"The given key '{(sender as Button)?.Tag as string}' was not present in the dictionary.");
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
			Title =  new StackPanel
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

		Grid VGridLayout(params (GridLength Height, FrameworkElement Content)[] rows)
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
	}

	private PathIcon CreateHeaderIcon() => new PathIcon
	{
		Data = (Geometry)XamlBindingHelper.ConvertValue(typeof(Geometry), "M7 0C3.13 0 0 3.13 0 7C0 12.25 7 20 7 20C7 20 14 12.25 14 7C14 3.13 10.87 0 7 0ZM7 9.5C5.62 9.5 4.5 8.38 4.5 7C4.5 5.62 5.62 4.5 7 4.5C8.38 4.5 9.5 5.62 9.5 7C9.5 8.38 8.38 9.5 7 9.5Z"),
		Foreground = (Brush)XamlBindingHelper.ConvertValue(typeof(SolidColorBrush), "#006C47"),
		Width = 24,
		Height = 24,
	};
}
