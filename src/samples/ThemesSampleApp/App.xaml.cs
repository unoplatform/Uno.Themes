using Microsoft.Extensions.Logging;
using Microsoft.UI.Xaml;

namespace Uno.Themes.WrapperApp;

/// <summary>
/// Wrapper application hosting the theme sample apps in secondary AssemblyLoadContexts.
/// </summary>
public sealed partial class App : Application
{
	/// <summary>
	/// Gets the main window of the wrapper app.
	/// </summary>
	public static Window? MainWindow { get; private set; }

	/// <summary>
	/// Initializes the singleton application object.
	/// </summary>
	public App()
	{
		this.InitializeComponent();
	}

	/// <summary>
	/// Invoked when the application is launched normally by the end user.
	/// </summary>
	protected override void OnLaunched(LaunchActivatedEventArgs e)
	{
		MainWindow = new Window
		{
			Title = "Uno.Themes Samples",
		};

		if (MainWindow.Content is not MainPage)
		{
			MainWindow.Content = new MainPage();
		}

		MainWindow.Activate();
	}

	/// <summary>
	/// Configures global Uno Platform logging. Unlike the sample heads this is not DEBUG-only:
	/// the wrapper's whole purpose is hosting guest apps, and its hosting diagnostics
	/// (teardown failures, ALC collection status) must stay visible in Release builds.
	/// </summary>
	public static void InitializeLogging()
	{
		var factory = LoggerFactory.Create(builder =>
		{
#if __WASM__
			builder.AddProvider(new global::Uno.Extensions.Logging.WebAssembly.WebAssemblyConsoleLoggerProvider());
#else
			builder.AddConsole();
#endif

			builder.SetMinimumLevel(LogLevel.Information);
			builder.AddFilter("Uno", LogLevel.Warning);
			builder.AddFilter("Windows", LogLevel.Warning);
			builder.AddFilter("Microsoft", LogLevel.Warning);

			// Guest-hosting diagnostics stay at Information even though "Uno" is capped at
			// Warning above (longest-matching filter prefix wins).
			builder.AddFilter("Uno.Themes.WrapperApp", LogLevel.Information);
		});

		global::Uno.Extensions.LogExtensionPoint.AmbientLoggerFactory = factory;

#if HAS_UNO
		global::Uno.UI.Adapter.Microsoft.Extensions.Logging.LoggingAdapter.Initialize();
#endif
	}
}
