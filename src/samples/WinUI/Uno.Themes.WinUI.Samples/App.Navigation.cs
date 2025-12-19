			trySynchronizeCurrentItem: false
#endif
		);

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
			.Where(x => x.Namespace?.StartsWith("Uno.Themes.WinUI.Samples") == true)
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

			// add items to the parent menu item or directly to the nav-view
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
