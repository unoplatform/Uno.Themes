namespace Uno.Themes.Samples;

partial class App
{
	/// <summary>
	/// Invoked when Navigation to a certain page fails
	/// </summary>
	/// <param name="sender">The Frame which failed navigation</param>
	/// <param name="e">Details about the navigation failure</param>
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

			var page = (Page)Activator.CreateInstance(sample.ViewType);
			page.DataContext = sample;


			_shell.NavigationView.Content = page;
		}
	}

	private Shell BuildShell()
	{
		_shell = new Shell();
		var nv = _shell.NavigationView;
		AddNavigationItems(nv);

		// landing navigation
		ShellNavigateTo<OverviewPage>(
#if WINDOWS_UWP
			// note: on uwp, NavigationView.SelectedItem MUST be set on launch to avoid entering compact-mode
			trySynchronizeCurrentItem: true
#else
			// workaround for uno#5069: setting NavView.SelectedItem at launch bricks it
			trySynchronizeCurrentItem: false
#endif
		);

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

	private void AddNavigationItems(NavigationView nv)
	{
		var categories = Assembly.GetExecutingAssembly().DefinedTypes
			.Where(x => x.Namespace?.StartsWith("Uno.Themes.Samples") == true)
			.Select(x => new { TypeInfo = x, SamplePageAttribute = x.GetCustomAttribute<SamplePageAttribute>() })
			.Where(x => x.SamplePageAttribute != null)
			.Select(x => new Sample(x.SamplePageAttribute, x.TypeInfo.AsType()))
			.OrderByDescending(x => x.SortOrder.HasValue)
			.ThenBy(x => x.SortOrder)
			.ThenBy(x => x.Title)
			.GroupBy(x => x.Category);

		/* the menu items are grouped by their [SamplePage].Category, some items can be "ungrouped" (category=None):
		 *	- {any item where category=none}...
		 *	- Category xyz
		 *		- {any item with category=xyz}... */

		foreach (var category in categories.OrderBy(x => x.Key))
		{
			// create parent menu item for this category
			var parentItem = default(NavigationViewItem);
			if (category.Key != SampleCategory.None)
			{
				parentItem = new NavigationViewItem
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

			// add items to the parent menu item or directly to the nav-view
			foreach (var sample in category)
			{
				var item = new NavigationViewItem
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
