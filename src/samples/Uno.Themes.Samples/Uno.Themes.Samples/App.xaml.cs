using ShowMeTheXAML;

namespace Uno.Themes.Samples;

/// <summary>
/// Provides application-specific behavior to supplement the default Application class.
/// </summary>
sealed partial class App : Application
{
	private Shell _shell;
	public static Window MainWindow { get; private set; }

	static App() =>
		InitializeLogging();

	/// <summary>
	/// Initializes the singleton application object.  This is the first line of authored code
	/// executed, and as such is the logical equivalent of main() or WinMain().
	/// </summary>
	public App()
	{
		ConfigureXamlDisplay();

		this.InitializeComponent();

#if HAS_UNO || NETFX_CORE
			this.Suspending += OnSuspending;
#endif
	}

	/// <summary>
	/// Invoked when the application is launched normally by the end user.  Other entry points
	/// will be used such as when the application is launched to open a specific file.
	/// </summary>
	/// <param name="e">Details about the launch request and process.</param>
	protected override void OnLaunched(LaunchActivatedEventArgs e)
	{
#if DEBUG
		if (System.Diagnostics.Debugger.IsAttached)
		{
			// this.DebugSettings.EnableFrameRateCounter = true;
		}
#endif

#if WINDOWS_UWP
		ApplicationView.GetForCurrentView().SetPreferredMinSize(new Size(320, 568)); // (size of the iPhone SE)
#endif

#if NET6_0_OR_GREATER && WINDOWS && !HAS_UNO
		MainWindow = new Window();
#else
		MainWindow = Window.Current;
#endif

		if (MainWindow is Window window)
		{
			if (!(window.Content is Shell))
			{
				window.Content = _shell = BuildShell();
			}
		}

		MainWindow.Activate();
	}



	/// <summary>
	/// Invoked when application execution is being suspended.  Application state is saved
	/// without knowing whether the application will be terminated or resumed with the contents
	/// of memory still intact.
	/// </summary>
	/// <param name="sender">The source of the suspend request.</param>
	/// <param name="e">Details about the suspend request.</param>
	private void OnSuspending(object sender, SuspendingEventArgs e)
	{
		var deferral = e.SuspendingOperation.GetDeferral();
		//TODO: Save application state and stop any background activity
		deferral.Complete();
	}

	/// <summary>
	/// Configures global Uno Platform logging
	/// </summary>
	private static void InitializeLogging()
	{
#if DEBUG
		// Logging is disabled by default for release builds, as it incurs a significant
		// initialization cost from Microsoft.Extensions.Logging setup. If startup performance
		// is a concern for your application, keep this disabled. If you're running on the web or
		// desktop targets, you can use URL or command line parameters to enable it.
		//
		// For more performance documentation: https://platform.uno/docs/articles/Uno-UI-Performance.html

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

			// Exclude logs below this level
			builder.SetMinimumLevel(LogLevel.Information);

			// Default filters for Uno Platform namespaces
			builder.AddFilter("Uno", LogLevel.Warning);
			builder.AddFilter("Windows", LogLevel.Warning);
			builder.AddFilter("Microsoft", LogLevel.Warning);

			// Generic Xaml events
			// builder.AddFilter($"{UINamespace}", LogLevel.Debug );
			// builder.AddFilter($"{UINamespace}.VisualStateGroup", LogLevel.Debug );
			// builder.AddFilter($"{UINamespace}.StateTriggerBase", LogLevel.Debug );
			// builder.AddFilter($"{UINamespace}.UIElement", LogLevel.Debug );
			// builder.AddFilter($"{UINamespace}.FrameworkElement", LogLevel.Trace );

			// Layouter specific messages
			// builder.AddFilter($"{UINamespace}.Controls", LogLevel.Debug );
			// builder.AddFilter($"{UINamespace}.Controls.Layouter", LogLevel.Debug );
			// builder.AddFilter($"{UINamespace}.Controls.Panel", LogLevel.Debug );

			// builder.AddFilter("Windows.Storage", LogLevel.Debug );

			// Binding related messages
			// builder.AddFilter($"{UINamespace}.Data", LogLevel.Debug );
			// builder.AddFilter($"{UINamespace}.Data", LogLevel.Debug );

			// Binder memory references tracking
			// builder.AddFilter("Uno.UI.DataBinding.BinderReferenceHolder", LogLevel.Debug );

			// DevServer and HotReload related
			// builder.AddFilter("Uno.UI.RemoteControl", LogLevel.Information);

			// Debug JS interop
			// builder.AddFilter("Uno.Foundation.WebAssemblyRuntime", LogLevel.Debug );
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
