using Windows.UI.ViewManagement;
using Microsoft.UI.Xaml.Markup;

namespace Uno.Themes.Samples;

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
	/// Initializes the singleton application object.
	/// </summary>
	public App()
	{
		ConfigureXamlDisplay();
		this.InitializeComponent();
	}

	/// <summary>
	/// Invoked when the application is launched normally by the end user.
	/// </summary>
	protected override void OnLaunched(Microsoft.UI.Xaml.LaunchActivatedEventArgs e)
	{
		MainWindow = Microsoft.UI.Xaml.Window.Current;

		if (MainWindow is Window window)
		{
			if (window.Content is not Shell)
			{
				window.Content = _shell = BuildShell();
			}
		}

		MainWindow?.Activate();
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
					.Flatten(nv.MenuItems.OfType<MUXC.NavigationViewItem>(), x => x.MenuItems.OfType<MUXC.NavigationViewItem>())
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

		// landing navigation
		ShellNavigateTo<OverviewPage>(trySynchronizeCurrentItem: false);

		// navigation + setting handler
		nv.ItemInvoked += OnNavigationItemInvoked;

		return _shell;
	}

	private void OnNavigationItemInvoked(MUXC.NavigationView sender, MUXC.NavigationViewItemInvokedEventArgs e)
	{
		if (e.InvokedItemContainer.DataContext is Sample sample)
		{
			ShellNavigateTo(sample, trySynchronizeCurrentItem: false);
		}
	}

	private void AddNavigationItems(MUXC.NavigationView nv)
	{
		var categories = Assembly.GetExecutingAssembly().DefinedTypes
			.Where(x => x.Namespace?.StartsWith("Uno.Themes.Samples") == true)
			.Select(x => new { TypeInfo = x, SamplePageAttribute = x.GetCustomAttribute<SamplePageAttribute>() })
			.Where(x => x.SamplePageAttribute != null)
			.Select(x => new Sample(x.SamplePageAttribute!, x.TypeInfo.AsType()))
			.OrderByDescending(x => x.SortOrder.HasValue)
			.ThenBy(x => x.SortOrder)
			.ThenBy(x => x.Title)
			.GroupBy(x => x.Category);

		foreach (var category in categories.OrderBy(x => x.Key))
		{
			var parentItem = default(MUXC.NavigationViewItem);
			if (category.Key != SampleCategory.None)
			{
				parentItem = new MUXC.NavigationViewItem
				{
					Content = category.Key.GetDescription() ?? category.Key.ToString(),
					Icon = CreateIconElement(GetCategoryIconSource()),
					SelectsOnInvoked = false,
				};
				AutomationProperties.SetAutomationId(parentItem, "Section_" + parentItem.Content);

				nv.MenuItems.Add(parentItem);

				object GetCategoryIconSource()
				{
					return category.Key switch
					{
						SampleCategory.Styles => Icons.Styles.CategoryHeader,
						SampleCategory.Controls => Icons.Controls.CategoryHeader,
						SampleCategory.Helpers => Icons.Helpers.CategoryHeader,
						_ => Icons.Shared.Placeholder
					};
				}
			}

			foreach (var sample in category)
			{
				var item = new MUXC.NavigationViewItem
				{
					Content = sample.Title,
					Icon = CreateIconElement(sample.IconSource),
					DataContext = sample,
				};
				AutomationProperties.SetAutomationId(item, "Section_" + item.Content);

				(parentItem?.MenuItems ?? nv.MenuItems).Add(item);
			}
		}

		static IconElement CreateIconElement(object? source)
		{
			if (source is string path)
			{
				return new PathIcon()
				{
					Data = (Microsoft.UI.Xaml.Media.Geometry)XamlBindingHelper.ConvertValue(typeof(Microsoft.UI.Xaml.Media.Geometry), path)
				};
			}
			if (source is Symbol symbol && symbol != default)
			{
				return new SymbolIcon(symbol);
			}

			return new PathIcon()
			{
				Data = (Microsoft.UI.Xaml.Media.Geometry)XamlBindingHelper.ConvertValue(typeof(Microsoft.UI.Xaml.Media.Geometry), Icons.Shared.Placeholder)
			};
		}
	}

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

	private static void ConfigureXamlDisplay()
	{
		XamlDisplay.Init();
	}
}
