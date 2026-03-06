namespace Uno.Themes.Samples;

/// <summary>
/// Provides shared navigation logic for sample apps.
/// </summary>
public static class NavigationHelper
{
	/// <summary>
	/// Set by the app-specific App class to provide window access to shared code.
	/// </summary>
	public static XamlWindow MainWindow { get; set; }

	/// <summary>
	/// Callback set by the app to handle shell navigation from shared code (e.g. OverviewSampleView).
	/// </summary>
	public static Action<Sample> ShellNavigateToHandler { get; set; }

	public static void NavigateTo(Shell shell, Sample sample, bool trySynchronizeCurrentItem)
	{
		var nv = shell.NavigationView;
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

			var page = (Page)Activator.CreateInstance(sample.ViewType);
			page.DataContext = sample;

			shell.NavigationView.Content = page;
		}
	}

	public static void NavigateTo<TPage>(Shell shell, bool trySynchronizeCurrentItem = true) where TPage : Page
	{
		var type = typeof(TPage);
		var attribute = type.GetCustomAttribute<SamplePageAttribute>()
			?? throw new NotSupportedException($"{type} isn't tagged with [{nameof(SamplePageAttribute)}].");
		var sample = new Sample(attribute, type);

		NavigateTo(shell, sample, trySynchronizeCurrentItem);
	}

	public static Shell BuildShell()
	{
		var shell = new Shell();
		var nv = shell.NavigationView;
		AddNavigationItems(nv);

		// landing navigation
		NavigateTo<OverviewPage>(shell,
#if WINDOWS_UWP
			// note: on uwp, NavigationView.SelectedItem MUST be set on launch to avoid entering compact-mode
			trySynchronizeCurrentItem: true
#else
			// workaround for uno#5069: setting NavView.SelectedItem at launch bricks it
			trySynchronizeCurrentItem: false
#endif
		);

		// navigation + setting handler
		nv.ItemInvoked += (sender, e) =>
		{
			if (e.InvokedItemContainer.DataContext is Sample sample)
			{
				NavigateTo(shell, sample, trySynchronizeCurrentItem: false);
			}
		};

		return shell;
	}

	private static void AddNavigationItems(MUXC.NavigationView nv)
	{
		// Scan the shared assembly for sample pages
		var categories = typeof(OverviewPage).Assembly.DefinedTypes
			.Where(x => x.Namespace?.StartsWith("Uno.Themes.Samples") == true)
			.Select(x => new { TypeInfo = x, SamplePageAttribute = x.GetCustomAttribute<SamplePageAttribute>() })
			.Where(x => x.SamplePageAttribute != null)
			.Select(x => new Sample(x.SamplePageAttribute, x.TypeInfo.AsType()))
			.Where(x => x.SupportedDesigns.Contains(SamplePageLayout.ActiveDesign))
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
					switch (category.Key)
					{
						case SampleCategory.Styles: return Icons.Styles.CategoryHeader;
						case SampleCategory.Controls: return Icons.Controls.CategoryHeader;
						case SampleCategory.Helpers: return Icons.Helpers.CategoryHeader;

						default: return Icons.Shared.Placeholder;
					}
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

		IconElement CreateIconElement(object source)
		{
			if (source is string path)
			{
				return new PathIcon()
				{
					Data = (Geometry)XamlBindingHelper.ConvertValue(typeof(Geometry), path)
				};
			}
			if (source is Symbol symbol && symbol != default)
			{
				return new SymbolIcon(symbol);
			}

			return new PathIcon()
			{
				Data = (Geometry)XamlBindingHelper.ConvertValue(typeof(Geometry), Icons.Shared.Placeholder)
			};
		}
	}
}
