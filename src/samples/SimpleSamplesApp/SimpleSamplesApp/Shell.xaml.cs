namespace Uno.Themes.Simple.Samples;

public sealed partial class Shell : UserControl
{
    public MUXC.NavigationView NavigationView => NavigationViewControl;

    public Shell()
    {
        this.InitializeComponent();

        InitializeSafeArea();
        this.Loaded += OnLoaded;
    }

    private void OnLoaded(object sender, RoutedEventArgs e)
    {
        SetDarkLightToggleInitialState();
    }
    
    private void InitializeSafeArea()
    {
        var window = Microsoft.UI.Xaml.Window.Current;
        var full = window.Bounds;
        var bounds = new Windows.Foundation.Rect(0, 0, 0, 0);

        var topPadding = Math.Abs(full.Top - bounds.Top);

        if (topPadding > 0)
        {
            TopPaddingRow.Height = new GridLength(topPadding);
        }
    }

    private void SetDarkLightToggleInitialState()
    {
        var window = Microsoft.UI.Xaml.Window.Current;
        var root = window.Content as FrameworkElement;
        if (root is null) return;

        switch (root.ActualTheme)
        {
            case ElementTheme.Default:
                DarkLightModeToggle.IsChecked = Helpers.SystemThemeHelper.GetSystemApplicationTheme() == ApplicationTheme.Dark;
                break;
            case ElementTheme.Light:
                DarkLightModeToggle.IsChecked = false;
                break;
            case ElementTheme.Dark:
                DarkLightModeToggle.IsChecked = true;
                break;
        }

        UpdateToggleContent();
    }

    private void ToggleButton_Click(object sender, RoutedEventArgs e)
    {
        var window = Microsoft.UI.Xaml.Window.Current;
        if (window.Content is FrameworkElement root)
        {
            switch (root.ActualTheme)
            {
                case ElementTheme.Default:
                    root.RequestedTheme = Helpers.SystemThemeHelper.GetSystemApplicationTheme() == ApplicationTheme.Dark
                        ? ElementTheme.Light
                        : ElementTheme.Dark;
                    break;
                case ElementTheme.Light:
                    root.RequestedTheme = ElementTheme.Dark;
                    break;
                case ElementTheme.Dark:
                    root.RequestedTheme = ElementTheme.Light;
                    break;
            }

            UpdateToggleContent();

            if (NavigationViewControl.PaneDisplayMode == MUXC.NavigationViewPaneDisplayMode.LeftMinimal)
            {
                NavigationViewControl.IsPaneOpen = false;
            }
        }
    }

    private void UpdateToggleContent()
    {
        DarkLightModeToggle.Content = DarkLightModeToggle.IsChecked == true ? "Light" : "Dark";
    }

    private void NavigationViewControl_DisplayModeChanged(MUXC.NavigationView sender, MUXC.NavigationViewDisplayModeChangedEventArgs e)
    {
        if (e.DisplayMode == MUXC.NavigationViewDisplayMode.Expanded)
        {
            NavigationViewControl.IsPaneOpen = NavigationViewControl.IsPaneVisible;
        }
    }
}
