using Microsoft.Extensions.Logging;
using Microsoft.UI.Xaml;
using Uno.UI.RuntimeTests;

namespace Uno.Themes.RuntimeTests;

/// <summary>
/// Minimal application host for the Uno.Themes runtime tests. It loads only WinUI resources and
/// the runtime-test engine UI — no sample gallery — so the hot-reload XAML compile stays clean.
/// </summary>
public partial class App : Application
{
	public App()
	{
		this.InitializeComponent();
	}

	private Window? MainWindow { get; set; }

	protected override void OnLaunched(LaunchActivatedEventArgs args)
	{
		MainWindow = new Window();
		MainWindow.Content ??= new UnitTestsControl();
		MainWindow.Activate();
	}

	public static void InitializeLogging()
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

			builder.SetMinimumLevel(LogLevel.Information);
			builder.AddFilter("Uno", LogLevel.Warning);
			builder.AddFilter("Uno.UI.RuntimeTests", LogLevel.Information);
			builder.AddFilter("Uno.UI.RemoteControl", LogLevel.Information);
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
