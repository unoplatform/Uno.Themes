namespace Uno.Themes.WinUI.Samples;

/// <summary>
/// Provides application-specific behavior to supplement the default Application class.
/// </summary>
sealed partial class App : Application
{
	private Shell? _shell;
	public static Window? MainWindow { get; private set; }

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
	}

	/// <summary>
	/// Invoked when the application is launched normally by the end user.
	/// </summary>
	/// <param name="e">Details about the launch request and process.</param>
	protected override void OnLaunched(LaunchActivatedEventArgs args)
	{
#if NET6_0_OR_GREATER && WINDOWS && !HAS_UNO
		MainWindow = new Window();
#else
		MainWindow = Microsoft.UI.Xaml.Window.Current;
#endif

#if DEBUG
		MainWindow.EnableHotReload();
#endif

		if (MainWindow.Content is not Shell)
		{
			MainWindow.Content = _shell = BuildShell();
		}

		MainWindow.Activate();
	}

	/// <summary>
	/// Invoked when Navigation to a certain page fails
	/// </summary>
	void OnNavigationFailed(object sender, NavigationFailedEventArgs e)
	{
		throw new Exception($"Failed to load {e.SourcePageType.FullName}: {e.Exception}");
	}

	public void ShellNavigateTo(Sample sample) => ShellNavigateTo(sample, trySynchronizeCurrentItem: true);

	private void ShellNavigateTo<TPage>(bool trySynchronizeCurrentItem = true) where TPage : Page
	{
		var type = typeof(TPage);
		var attribute = type.GetCustomAttribute<SamplePageAttribute>()
			?? throw new NotSupportedException($"{type} isn't tagged with [{nameof(SamplePageAttribute)}].");
		var sample = new Sample(attribute, type);

		ShellNavigateTo(sample, trySynchronizeCurrentItem);
	}

	private void ShellNavigateTo(Sample sample, bool trySynchronizeCurrentItem)
	{
		if (_shell == null) return;
		
		var nv = _shell.NavigationView;
		if (nv.Content?.GetType() != sample.ViewType)
		{
			var selected = trySynchronizeCurrentItem
				? HierarchyHelper
					.Flatten(nv.MenuItems.OfType<NavigationViewItem>(), x => x.MenuItems.OfType<NavigationViewItem>())
					.FirstOrDefault(x => (x.DataContext as Sample)?.ViewType == sample.ViewType)
				: default;
			if (selected != null)
			{
				nv.SelectedItem = selected;
			}

			var page = (Page?)Activator.CreateInstance(sample.ViewType);
			if (page != null)
			{
				page.DataContext = sample;
				_shell.NavigationView.Content = page;
			}
		}
	}

	private Shell BuildShell()
	{
		_shell = new Shell();
		var nv = _shell.NavigationView;
		AddNavigationItems(nv);

		// landing navigation - navigate to overview page
		// ShellNavigateTo<OverviewPage>(trySynchronizeCurrentItem: false);

		// navigation + setting handler
		nv.ItemInvoked += OnNavigationItemInvoked;

		return _shell;
	}

	private void OnNavigationItemInvoked(NavigationView sender, NavigationViewItemInvokedEventArgs e)
	{
		if (e.InvokedItemContainer.DataContext is Sample sample)
		{
			ShellNavigateTo(sample, trySynchronizeCurrentItem: false);
		}
	}

	/// <summary>
	/// Configures global Uno Platform logging
	/// </summary>
	private static void InitializeLogging()
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

	static void ConfigureXamlDisplay()
	{
		XamlDisplay.Init();
	}
}
