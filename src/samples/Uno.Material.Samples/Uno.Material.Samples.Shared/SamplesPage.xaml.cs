using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Uno.Material.Samples.Content.Controls;
using Uno.Material.Samples.Content.Styles;


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
			{ Content = "TextBox Overview", Icon = new SymbolIcon(Symbol.Next), Tag = "TextBoxSamplePage" });
			NavView.MenuItems.Add(new NavigationViewItem()
			{ Content = "TextBlock Overview", Icon = new SymbolIcon(Symbol.Next), Tag = "TextBlockSamplePage" });
			NavView.MenuItems.Add(new NavigationViewItem()
            { Content = "CheckBox Overview", Icon = new SymbolIcon(Symbol.Next), Tag = "CheckBoxSamplePage" });
            NavView.MenuItems.Add(new NavigationViewItem()
            { Content = "RadioButton Overview", Icon = new SymbolIcon(Symbol.Next), Tag = "RadioButtonSamplePage" });
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
            // TO-DO : Verify if we want a SettingPage
            //if (args.IsSettingsInvoked)
            //{
            //    ContentFrame.Navigate(typeof(SettingsPage));
            //}
            //else
            //{
                // Find NavigationViewItem with Content that equals InvokedItem
                var item = sender.MenuItems.OfType<NavigationViewItem>().First(x => (string)x.Content == (string)args.InvokedItem);
                NavView_Navigate(item as NavigationViewItem);
            //}
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
				case "TextBoxSamplePage":
					ContentFrame.Navigate(typeof(TextBoxSamplePage));
					break;
				case "TextBlockSamplePage":
					ContentFrame.Navigate(typeof(TextBlockSamplePage));
					break;
				case "CheckBoxSamplePage":
                    ContentFrame.Navigate(typeof(CheckBoxSamplePage));
                    break;
                case "RadioButtonSamplePage":
                    ContentFrame.Navigate(typeof(RadioButtonSamplePage));
                    break;
				case "ToggleSwitchSamplePage":
					ContentFrame.Navigate(typeof(ToggleSwitchSamplePage));
					break;
			}
        }

        public void SetHeader(string header)
        {
            NavView.Header = header;
        }
    }
}
