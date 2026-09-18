namespace Uno.Themes.Samples;

/// <summary>
/// Provides application-specific behavior to supplement the default Application class.
/// </summary>
sealed partial class App : Application
{
	private Shell _shell;
	public static Microsoft.UI.Xaml.Window MainWindow { get; private set; }

	static App() =>
		InitializeLogging();

	/// <summary>
	/// Initializes the singleton application object.
	/// </summary>
	public App()
	{
		ConfigureXamlDisplay();
		SamplePageLayout.ActiveDesign = Design.Simple;

		this.InitializeComponent();

		// After InitializeComponent so the theme is already merged into Resources when shared
		// code picks this up. See NavigationHelper.CurrentApplication for why Application.Current
		// cannot be used instead.
		NavigationHelper.CurrentApplication = this;

#if HAS_UNO || NETFX_CORE
		this.Suspending += OnSuspending;
#endif
	}

	/// <summary>
	/// Invoked when the application is launched normally by the end user.
	/// </summary>
	protected override void OnLaunched(LaunchActivatedEventArgs e)
	{
		// Do not use Window.Current here: it is a process-wide static in the shared Uno.UI, so when
		// this app is hosted in a secondary ALC (ThemesSampleApp) it would grab the host's window.
		// The first new Window() maps to the main window on single-window platforms, so this stays
		// correct standalone too.
		MainWindow = new Microsoft.UI.Xaml.Window();
		NavigationHelper.MainWindow = MainWindow;

		if (MainWindow is Microsoft.UI.Xaml.Window window)
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
