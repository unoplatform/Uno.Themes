namespace Uno.Themes.Samples;

/// <summary>
/// Provides application-specific behavior to supplement the default Application class.
/// </summary>
sealed partial class App : Application
{
	private Shell _shell;
	public static XamlWindow MainWindow { get; private set; }

	static App() =>
		InitializeLogging();

	/// <summary>
	/// Initializes the singleton application object.
	/// </summary>
	public App()
	{
		ConfigureXamlDisplay();
		SamplePageLayout.ActiveDesign = Design.Material;

		this.InitializeComponent();

#if HAS_UNO || NETFX_CORE
		this.Suspending += OnSuspending;
#endif
	}

	/// <summary>
	/// Invoked when the application is launched normally by the end user.
	/// </summary>
	protected override void OnLaunched(LaunchActivatedEventArgs e)
	{
		MainWindow = XamlWindow.Current;
		NavigationHelper.MainWindow = MainWindow;

		if (MainWindow is XamlWindow window)
		{
			if (!(window.Content is Shell))
			{
				window.Content = _shell = NavigationHelper.BuildShell();
			}
		}

		NavigationHelper.ShellNavigateToHandler = sample =>
			NavigationHelper.NavigateTo(_shell, sample, trySynchronizeCurrentItem: true);

		MainWindow.Activate();
	}

	private void OnSuspending(object sender, SuspendingEventArgs e)
	{
		var deferral = e.SuspendingOperation.GetDeferral();
		deferral.Complete();
	}

	public static void InitializeLogging()
	{
#if DEBUG
		var factory = LoggerFactory.Create(builder =>
		{
			var UINamespace = typeof(UIElement).Namespace ?? string.Empty;
#if __WASM__
			builder.AddProvider(new global::Uno.Extensions.Logging.WebAssembly.WebAssemblyConsoleLoggerProvider());
#elif __IOS__ || __MACCATALYST__
			builder.AddProvider(new global::Uno.Extensions.Logging.OSLogLoggerProvider());
#else
			builder.AddConsole();
#endif

			builder.SetMinimumLevel(LogLevel.Information);
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

	static void ConfigureXamlDisplay()
	{
		XamlDisplay.Init();
	}
}
