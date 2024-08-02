namespace Uno.Themes.Samples.Content.Controls;

[SamplePage(SampleCategory.Controls, "TextBox", Description = "This control allows users to input a textual value.", DocumentationLink = "https://docs.microsoft.com/en-us/uwp/api/windows.ui.xaml.controls.textbox")]
public sealed partial class TextBoxSamplePage : Page
{
	public TextBoxSamplePage()
	{
		this.InitializeComponent();
	}

	private void AddHeader(object sender, RoutedEventArgs e)
	{
		txtMyTb.Header = "Header";
		txtMyTb1.Header = "Header";
	}

	private void RemoveHeader(object sender, RoutedEventArgs e)
	{
		txtMyTb.Header = null;
		txtMyTb1.Header = null;
	}

	private void AddPlaceholder(object sender, RoutedEventArgs e)
	{
		txtMyTb.PlaceholderText = "Placeholder";
		txtMyTb1.PlaceholderText = "Placeholder";
	}

	private void RemovePlaceholder(object sender, RoutedEventArgs e)
	{
		txtMyTb.PlaceholderText = null;
		txtMyTb1.PlaceholderText = null;
	}

	private void AddText(object sender, RoutedEventArgs e)
	{
		txtMyTb.Text = "Text";
		txtMyTb1.Text = "Text";
	}

	private void RemoveText(object sender, RoutedEventArgs e)
	{
		txtMyTb.Text = null;
		txtMyTb1.Text = null;
	}
}
