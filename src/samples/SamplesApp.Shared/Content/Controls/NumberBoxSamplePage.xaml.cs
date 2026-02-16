namespace Uno.Themes.Samples.Content.Controls;

[SamplePage(SampleCategory.Controls, "Number Box", Description = "This control can be used to set a numeric value.", DocumentationLink = "https://docs.microsoft.com/en-us/windows/uwp/design/controls-and-patterns/number-box", SupportedDesigns = new[] { Design.Cupertino })]
public sealed partial class NumberBoxSamplePage : Page
    {
        public NumberBoxSamplePage()
        {
            this.InitializeComponent();
        }
    }
