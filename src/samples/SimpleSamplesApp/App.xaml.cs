namespace Uno.Themes.Simple.Samples;

/// <summary>
/// Provides application-specific behavior to supplement the default Application class.
/// </summary>
sealed partial class App : Application
{
    private Shell? _shell;

    static App() =>
        InitializeLogging();

    /// <summary>
    /// Initializes the singleton application object.
    /// </summary>
    public App()
    {
        ConfigureXamlDisplay();
        this.InitializeComponent();
    }

    static void ConfigureXamlDisplay()
    {
        XamlDisplay.Init();
    }

    /// <summary>
    /// Invoked when the application is launched normally by the end user.
    /// </summary>
    protected override void OnLaunched(LaunchActivatedEventArgs e)
    {
#if DEBUG
        if (System.Diagnostics.Debugger.IsAttached)
        {
            // this.DebugSettings.EnableFrameRateCounter = true;
        }
#endif

        var window = Microsoft.UI.Xaml.Window.Current;

        if (window.Content is not Shell)
        {
            window.Content = _shell = BuildShell();
        }

        window.Activate();
    }

    /// <summary>
    /// Configures global Uno Platform logging
    /// </summary>
    internal static void InitializeLogging()
    {
#if DEBUG
        var factory = LoggerFactory.Create(builder =>
        {
#if __WASM__
            builder.AddProvider(new global::Uno.Extensions.Logging.WebAssembly.WebAssemblyConsoleLoggerProvider());
#elif __IOS__ || __MACCATALYST__
            builder.AddProvider(new global::Uno.Extensions.Logging.OSLogLoggerProvider());
#else
            builder.AddConsole();
#endif

            // Exclude logs below this level
            builder.SetMinimumLevel(LogLevel.Information);

            // Default filters for Uno Platform namespaces
            builder.AddFilter("Uno", LogLevel.Warning);
            builder.AddFilter("Windows", LogLevel.Warning);
            builder.AddFilter("Microsoft", LogLevel.Warning);
        });

        global::Uno.Extensions.LogExtensionPoint.AmbientLoggerFactory = factory;

#if HAS_UNO
        global::Uno.UI.Adapter.Microsoft.Extensions.Logging.LoggingAdapter.Initialize();
#endif
#endif
    }
}
