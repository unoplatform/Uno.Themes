using System;
using System.Linq;
using Uno.Material.Samples.Content.Controls;
using Uno.Material.Samples.Content.Styles;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;


namespace Uno.Material.Samples
{
	public sealed partial class SamplesPage : Page
	{
        public SamplesPage()
        {
            this.InitializeComponent();
        }

        private void NavView_Loaded(object sender, RoutedEventArgs e)
        {
#if WINDOWS_UWP
			NavView.IsSettingsVisible = true;
			// Change the settings text to toggle the theme
			var settingsItem = (NavigationViewItem)NavView.SettingsItem;
			settingsItem.Content = "Toggle Light/Dark theme";
#else
			NavView.IsSettingsVisible = false;
#endif

			// Adding NavigationView items in code behind

			// Styles
			NavView.MenuItems.Add(new NavigationViewItemHeader() { Content = "Styles" });
            NavView.MenuItems.Add(new NavigationViewItem()
            { Content = "Colors Overview", Icon = new SymbolIcon(Symbol.Next), Tag = "ColorsSamplePage" });
            NavView.MenuItems.Add(new NavigationViewItemSeparator());

            // Controls
            NavView.MenuItems.Add(new NavigationViewItemHeader() { Content = "Controls" });
            NavView.MenuItems.Add(new NavigationViewItem()
            { Content = "Button Overview", Icon = new SymbolIcon(Symbol.Next), Tag = "ButtonSamplePage" });
			NavView.MenuItems.Add(new NavigationViewItem()
			{ Content = "Cards Overview", Icon = new SymbolIcon(Symbol.Next), Tag = "CardsSamplePage" });
			NavView.MenuItems.Add(new NavigationViewItem()
			{ Content = "CheckBox Overview", Icon = new SymbolIcon(Symbol.Next), Tag = "CheckBoxSamplePage" });
			NavView.MenuItems.Add(new NavigationViewItem()
			{ Content = "ComboBox Overview", Icon = new SymbolIcon(Symbol.Next), Tag = "ComboBoxSamplePage" });
			NavView.MenuItems.Add(new NavigationViewItem()
			{ Content = "FAB Overview", Icon = new SymbolIcon(Symbol.Next), Tag = "FabSamplePage" });
			NavView.MenuItems.Add(new NavigationViewItem()
			{ Content = "NavigationView Overview", Icon = new SymbolIcon(Symbol.Next), Tag = "NavigationViewSamplePage" });
			NavView.MenuItems.Add(new NavigationViewItem()
			{ Content = "RadioButton Overview", Icon = new SymbolIcon(Symbol.Next), Tag = "RadioButtonSamplePage" });
			NavView.MenuItems.Add(new NavigationViewItem()
			{ Content = "SnackBar Overview", Icon = new SymbolIcon(Symbol.Next), Tag = "SnackBarSamplePage" });
			NavView.MenuItems.Add(new NavigationViewItem()
			{ Content = "TextBlock Overview", Icon = new SymbolIcon(Symbol.Next), Tag = "TextBlockSamplePage" });
			NavView.MenuItems.Add(new NavigationViewItem()
			{ Content = "TextBox Overview", Icon = new SymbolIcon(Symbol.Next), Tag = "TextBoxSamplePage" });
			NavView.MenuItems.Add(new NavigationViewItem()
			{ Content = "ToggleSwitch Overview", Icon = new SymbolIcon(Symbol.Next), Tag = "ToggleSwitchSamplePage" });

			// Set the initial SelectedItem 
			foreach (NavigationViewItemBase item in NavView.MenuItems)
            {
                if (item is NavigationViewItem && item.Tag.ToString() == "TextBoxSamplePage")
                {
                    NavView.SelectedItem = item;
                    SetHeader(item.Content.ToString());
                    ContentFrame.Navigate(typeof(TextBoxSamplePage));
                    break;
                }
            }
        }

        private void NavView_ItemInvoked(NavigationView sender, NavigationViewItemInvokedEventArgs args)
        {
            if (args.IsSettingsInvoked)
            {
#if WINDOWS_UWP
				ToggleTheme();
				void ToggleTheme()
				{
					// Set theme for window root.
					if (Window.Current.Content is FrameworkElement frameworkElement)
					{
						if (frameworkElement.ActualTheme == ElementTheme.Dark)
						{
							frameworkElement.RequestedTheme = ElementTheme.Light;
						}
						else
						{
							frameworkElement.RequestedTheme = ElementTheme.Dark;
						}
					}
				}
#endif
			}
            else
            {
              // Find NavigationViewItem with Content that equals InvokedItem
              var item = sender.MenuItems.OfType<NavigationViewItem>().First(x => (string)x.Content == (string)args.InvokedItem);
              NavView_Navigate(item as NavigationViewItem);
            }
        }



        private void NavView_Navigate(NavigationViewItem item)
        {
            SetHeader(item.Content.ToString());

            switch (item.Tag)
            {
                //Styles
                case "ColorsSamplePage":
                    ContentFrame.Navigate(typeof(ColorsSamplePage));
                    break;
                //Controls
                case "ButtonSamplePage":
                    ContentFrame.Navigate(typeof(ButtonSamplePage));
					break;
				case "CardsSamplePage":
					ContentFrame.Navigate(typeof(CardsSamplePage));
					break;
				case "TextBoxSamplePage":
					ContentFrame.Navigate(typeof(TextBoxSamplePage));
					break;
				case "TextBlockSamplePage":
					ContentFrame.Navigate(typeof(TextBlockSamplePage));
					break;
				case "CheckBoxSamplePage":
					ContentFrame.Navigate(typeof(CheckBoxSamplePage));
					break;
				case "ComboBoxSamplePage":
					ContentFrame.Navigate(typeof(ComboBoxSamplePage));
					break;
				case "NavigationViewSamplePage":
					ContentFrame.Navigate(typeof(NavigationViewSamplePage));
					break;
				case "RadioButtonSamplePage":
					ContentFrame.Navigate(typeof(RadioButtonSamplePage));
					break;
				case "SnackBarSamplePage":
					ContentFrame.Navigate(typeof(SnackBarSamplePage));
					break;
				case "ToggleSwitchSamplePage":
					ContentFrame.Navigate(typeof(ToggleSwitchSamplePage));
					break;
				case "FabSamplePage":
					ContentFrame.Navigate(typeof(FabSamplePage));
					break;
			}
        }

        public void SetHeader(string header)
        {
            NavView.Header = header;
        }
    }
}
