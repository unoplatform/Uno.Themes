using Uno.Themes.Simple.Samples.Content;
using Uno.Themes.Simple.Samples.Entities;
using Uno.Themes.Simple.Samples.Helpers;

namespace Uno.Themes.Simple.Samples;

partial class App
{
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
        if (_shell is null) return;

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

            var page = (Page)Activator.CreateInstance(sample.ViewType)!;
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
            .Where(x => x.Namespace?.StartsWith("Uno.Themes.Simple.Samples") == true)
            .Select(x => new { TypeInfo = x, SamplePageAttribute = x.GetCustomAttribute<SamplePageAttribute>() })
            .Where(x => x.SamplePageAttribute != null)
            .Select(x => new Sample(x.SamplePageAttribute!, x.TypeInfo.AsType()))
            .OrderBy(x => x.SortOrder)
            .ThenBy(x => x.Title)
            .GroupBy(x => x.Category);

        foreach (var category in categories.OrderBy(x => x.Key))
        {
            // create parent menu item for this category
            var parentItem = default(MUXC.NavigationViewItem);
            if (category.Key != SampleCategory.None)
            {
                parentItem = new MUXC.NavigationViewItem
                {
                    Content = category.Key.ToString(),
                    SelectsOnInvoked = false,
                };
                AutomationProperties.SetAutomationId(parentItem, "Section_" + parentItem.Content);

                nv.MenuItems.Add(parentItem);
            }

            // add items to the parent menu item or directly to the nav-view
            foreach (var sample in category)
            {
                var item = new MUXC.NavigationViewItem
                {
                    Content = sample.Title,
                    DataContext = sample,
                };

                AutomationProperties.SetAutomationId(item, "Section_" + item.Content);

                (parentItem?.MenuItems ?? nv.MenuItems).Add(item);
            }
        }
    }
}
