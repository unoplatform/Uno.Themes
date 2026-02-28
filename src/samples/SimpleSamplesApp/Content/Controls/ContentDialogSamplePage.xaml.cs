using Uno.Themes.Simple.Samples.Entities;

namespace Uno.Themes.Simple.Samples.Content.Controls;

[SamplePage(SampleCategory.Controls, "ContentDialog", Symbol = Symbol.NewWindow)]
public sealed partial class ContentDialogSamplePage : Page
{
    public ContentDialogSamplePage()
    {
        this.InitializeComponent();
    }

    private async void ShowBasicDialog_Click(object sender, RoutedEventArgs e)
    {
        var dialog = new ContentDialog
        {
            Title = "Information",
            Content = "This is a basic content dialog with a single close button.",
            CloseButtonText = "OK",
            XamlRoot = this.XamlRoot,
        };

        var result = await dialog.ShowAsync();
        DialogResultText.Text = $"Result: {result}";
    }

    private async void ShowConfirmDialog_Click(object sender, RoutedEventArgs e)
    {
        var dialog = new ContentDialog
        {
            Title = "Confirm Action",
            Content = "Are you sure you want to proceed? This action cannot be undone.",
            PrimaryButtonText = "Confirm",
            CloseButtonText = "Cancel",
            DefaultButton = ContentDialogButton.Primary,
            XamlRoot = this.XamlRoot,
        };

        var result = await dialog.ShowAsync();
        DialogResultText.Text = $"Result: {result}";
    }

    private async void ShowThreeButtonDialog_Click(object sender, RoutedEventArgs e)
    {
        var dialog = new ContentDialog
        {
            Title = "Unsaved Changes",
            Content = "You have unsaved changes. What would you like to do?",
            PrimaryButtonText = "Save",
            SecondaryButtonText = "Don't Save",
            CloseButtonText = "Cancel",
            DefaultButton = ContentDialogButton.Primary,
            XamlRoot = this.XamlRoot,
        };

        var result = await dialog.ShowAsync();
        DialogResultText.Text = $"Result: {result}";
    }

    private async void ShowCustomContentDialog_Click(object sender, RoutedEventArgs e)
    {
        var panel = new StackPanel { Spacing = 12 };
        panel.Children.Add(new TextBlock
        {
            Text = "Enter your name to continue:",
            TextWrapping = TextWrapping.Wrap,
        });
        panel.Children.Add(new TextBox
        {
            PlaceholderText = "Full name",
        });

        var dialog = new ContentDialog
        {
            Title = "User Input",
            Content = panel,
            PrimaryButtonText = "Submit",
            CloseButtonText = "Cancel",
            DefaultButton = ContentDialogButton.Primary,
            XamlRoot = this.XamlRoot,
        };

        var result = await dialog.ShowAsync();
        DialogResultText.Text = $"Result: {result}";
    }
}
