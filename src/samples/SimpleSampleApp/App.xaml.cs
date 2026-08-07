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

#if HAS_UNO || NETFX_CORE
		this.Suspending += OnSuspending;
#endif
	}

	/// <summary>
	/// Invoked when the application is launched normally by the end user.
	/// </summary>
	protected override void OnLaunched(LaunchActivatedEventArgs e)
	{
		MainWindow = Microsoft.UI.Xaml.Window.Current;
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

#if DEBUG && HAS_UNO
		// Wire up the Uno hot-reload client to this window so metadata updates
		// produced by the dev-server can be dispatched onto the UI thread.
		// Without this call, ClientHotReloadProcessor.CurrentWindow stays null and
		// the runtime emits the warning :
		//     "Unable to access Dispatcher/DispatcherQueue in order to invoke
		//      ReloadWithUpdatedTypes. Make sure you have enabled hot-reload
		//      (Window.UseStudio()) in app startup."
		// `showHotReloadIndicator: false` keeps the on-screen HR indicator off — we
		// only need the dispatcher hookup for runtime-tests HR delta application,
		// not the visual badge that adds noise (and pixels) to a headless run.
		// Provided by Uno.UI.HotDesign.Client (transitively pulled in by Uno.Sdk.Private
		// in non-Optimize builds), so this code only compiles in Debug.
		MainWindow.UseStudio(showHotReloadIndicator: false);
#endif
	}

	private void OnSuspending(object sender, SuspendingEventArgs e)
	{
		var deferral = e.SuspendingOperation.GetDeferral();
		deferral.Complete();
	}

	public static void InitializeLogging()
	{
#if DEBUG
		var runtimeTestsConfig = Environment.GetEnvironmentVariable("UNO_RUNTIME_TESTS_RUN_TESTS") ?? string.Empty;
		var isHotReloadRuntimeTests = runtimeTestsConfig.IndexOf("HotReload", StringComparison.OrdinalIgnoreCase) >= 0;
		var isSecondaryApp = string.Equals(
			Environment.GetEnvironmentVariable("UNO_RUNTIME_TESTS_IS_SECONDARY_APP"),
			"true",
			StringComparison.OrdinalIgnoreCase);

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

			if (isHotReloadRuntimeTests)
			{
				builder.AddFilter("Uno.UI.RuntimeTests", LogLevel.Debug);
				builder.AddFilter("Uno.UI.RuntimeTests.Internal.Helpers", LogLevel.Trace);
				builder.AddFilter("Uno.UI.RemoteControl", LogLevel.Trace);
				builder.AddFilter("Uno.WinUI.DevServer", LogLevel.Debug);
			}
		});

		global::Uno.Extensions.LogExtensionPoint.AmbientLoggerFactory = factory;

		if (isHotReloadRuntimeTests)
		{
			factory.CreateLogger("Uno.Themes.RuntimeTests.HotReload.Boot")
				.LogInformation(
					"Hot-reload runtime-tests logging enabled. SecondaryApp={IsSecondaryApp}; DevServer={Host}:{Port}; RuntimeTestsConfig={RuntimeTestsConfig}",
					isSecondaryApp,
					Environment.GetEnvironmentVariable("UNO_DEV_SERVER_HOST"),
					Environment.GetEnvironmentVariable("UNO_DEV_SERVER_PORT"),
					runtimeTestsConfig);
		}

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
