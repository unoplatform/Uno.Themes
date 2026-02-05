using System.Reflection;
using Uno.Themes.Samples.Entities;

namespace Uno.Themes.Samples;

public sealed partial class App : Application
{
	public const string DesignSystem = "Simple";

	public App()
	{
		this.InitializeComponent();
	}

	protected override void OnLaunched(Microsoft.UI.Xaml.LaunchActivatedEventArgs e)
	{
		var shell = BuildShell();
#if NET6_0_OR_GREATER && WINDOWS && !HAS_UNO
		_window = new Window();
#else
		_window = Window.Current;
#endif
		_window.Content = shell;
		_window.Activate();
	}

	private Window? _window;

	public static Sample[] GetSamples()
	{
		return Assembly.GetExecutingAssembly().DefinedTypes
			.Where(t => t.Namespace?.StartsWith("Uno.Themes.Samples.Content") == true
						&& t.GetCustomAttribute<SamplePageAttribute>() is { })
			.Select(t =>
			{
				var attr = t.GetCustomAttribute<SamplePageAttribute>()!;
				return new Sample(attr, t.AsType());
			})
			.OrderBy(s => s.Category)
			.ThenBy(s => s.SortOrder ?? int.MaxValue)
			.ThenBy(s => s.Title)
			.ToArray();
	}

	public static object[] BuildMainMenu()
	{
		var samples = GetSamples();
		var categories = samples
			.Select(s => s.Category)
			.Distinct()
			.OrderBy(c => c)
			.ToArray();

		var menuItems = new List<object>();

		foreach (var category in categories)
		{
			// Add category header
			var headerItem = new NavigationViewItemHeader
			{
				Content = category.GetDescription()
			};
			menuItems.Add(headerItem);

			// Add samples under this category
			var categorySamples = samples.Where(s => s.Category == category);
			foreach (var sample in categorySamples)
			{
				var navItem = new NavigationViewItem
				{
					Content = sample.Title,
					Icon = new PathIcon
					{
						Data = PathGeometry.Parse(sample.IconPath ?? Icons.Controls.Control)
					},
					Tag = sample
				};
				menuItems.Add(navItem);
			}
		}

		return menuItems.ToArray();
	}

	private UIElement BuildShell()
	{
		var shell = new Shell();

		// Build navigation menu
		var menuItems = BuildMainMenu();
		foreach (var item in menuItems)
		{
			shell.NavigationView.MenuItems.Add(item);
		}

		// Navigate to first sample
		if (shell.NavigationView.MenuItems
				.OfType<NavigationViewItem>()
				.FirstOrDefault() is { } firstItem)
		{
			shell.NavigationView.SelectedItem = firstItem;
			var sample = (Sample)firstItem.Tag!;
			shell.ContentFrame.Navigate(sample.ViewType);
		}

		return shell;
	}
}
