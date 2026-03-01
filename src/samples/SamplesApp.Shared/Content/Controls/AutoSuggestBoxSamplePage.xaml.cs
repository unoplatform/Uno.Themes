namespace Uno.Themes.Samples.Content.Controls;

[SamplePage(SampleCategory.Controls, "AutoSuggestBox", Description = "A text control that makes suggestions to users as they type, useful for search scenarios.", DocumentationLink = "https://learn.microsoft.com/en-us/windows/apps/design/controls/auto-suggest-box", SupportedDesigns = new[] { Design.Simple })]
public sealed partial class AutoSuggestBoxSamplePage : Page
{
	private readonly string[] _fruits = new[]
	{
		"Apple", "Apricot", "Avocado",
		"Banana", "Blackberry", "Blueberry",
		"Cherry", "Coconut", "Cranberry",
		"Date", "Dragonfruit",
		"Fig",
		"Grape", "Grapefruit", "Guava",
		"Kiwi",
		"Lemon", "Lime", "Lychee",
		"Mango", "Melon",
		"Nectarine",
		"Orange",
		"Papaya", "Peach", "Pear", "Pineapple", "Plum", "Pomegranate",
		"Raspberry",
		"Strawberry",
		"Tangerine",
		"Watermelon"
	};

	public AutoSuggestBoxSamplePage()
	{
		this.InitializeComponent();
	}

	private void SuggestBox_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
	{
		if (args.Reason == AutoSuggestionBoxTextChangeReason.UserInput)
		{
			var query = sender.Text?.Trim();
			if (string.IsNullOrEmpty(query))
			{
				sender.ItemsSource = null;
			}
			else
			{
				sender.ItemsSource = _fruits
					.Where(f => f.Contains(query, StringComparison.OrdinalIgnoreCase))
					.ToArray();
			}
		}
	}
}
