using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Uno.Disposables;
using Uno.Material.Samples.Content.Controls;
using Uno.Material.Samples.Content.Styles;
using Uno.Material.Samples.Shared.Content.Extensions;
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

		#region Navigation View
		// note: This section of code is showcased in NavigationViewSamplePage.
		// Be sure to keep it updated, if this section receives a major change.

		private void NavView_Loaded(object sender, RoutedEventArgs e)
		{
			if (NavView.MenuItems.Any())
			{
				// on android, this can also fire when the app is resumed from background ("alt-tabbed back")
				return;
			}

#if WINDOWS_UWP
			NavView.IsSettingsVisible = true;
			// Change the settings text to toggle the theme
			var settingsItem = (NavigationViewItem)NavView.SettingsItem;
			settingsItem.Content = "Toggle Light/Dark theme";
#else
			NavView.IsSettingsVisible = false;
#endif

			// Adding NavigationView items in code behind
			InitializeNavigationViewItems();

			// Set the starting page
			var type = typeof(ColorsSamplePage);
			var item = NavView.MenuItems
				.OfType<NavigationViewItem>()
				.FirstOrDefault(x => (Type)x.Tag == type)
				?? throw new Exception($"Navigation item for {type} was not found.");

			NavView.SelectedItem = item;
			NavView.Header = item.Content;
			ContentFrame.Navigate((Type)item.Tag);
		}

		private void NavView_ItemInvoked(NavigationView sender, NavigationViewItemInvokedEventArgs args)
		{
			if (args.IsSettingsInvoked)
			{
				ToggleTheme();
			}
			else if (args.InvokedItemContainer is NavigationViewItem item)
			{
				NavView.Header = item.Content;
				ContentNavigation((Type)item.Tag);
			}
		}

		private async void ContentNavigation(Type page)
		{
#if __ANDROID__
			// Workaround #251 - By closing the navigation pane with a delay, prior to navigation, we avoid flickers from too much work being done on the UI Thread.
			NavView.IsPaneOpen = false;
			await Task.Delay(TimeSpan.FromSeconds(.25));
#endif

			ContentFrame.Navigate(page);
		}

		private void InitializeNavigationViewItems()
		{
			// Styles
			using (AddMenuHeader("Styles"))
			{
				AddMenuItem<ColorsSamplePage>(icon: "ColorsIcon");
			}

			// Controls
			using (AddMenuHeader("Controls"))
			{
				AddMenuItem<BottomNavigationBarSamplePage>(content: "BottomBarNavigation", icon: "BottomBarIcon");
				AddMenuItem<ButtonSamplePage>(icon: "ButtonIcon");
				AddMenuItem<CardSamplePage>(icon: "CardsIcon");
				AddMenuItem<CheckBoxSamplePage>(icon: "CheckboxIcon");
				AddMenuItem<ComboBoxSamplePage>(icon: "ComboBoxIcon");
				// TODO Add divider icon?
				AddMenuItem<DividerSamplePage>();
				AddMenuItem<ExpandingBottomSheetSamplePage>(icon: "ExpandingBottomSheetIcon");
				AddMenuItem<FabSamplePage>(content: "FAB", icon: "FabIcon");
				AddMenuItem<HyperlinkButtonSamplePage>(icon: "ButtonIcon");
				AddMenuItem<ModalBottomSheetSamplePage>(icon: "ModalBottomSheetIcon");
				AddMenuItem<ModalStandardBottomSheetSamplePage>(icon: "StandardBottomSheetIcon");
				AddMenuItem<NavigationViewSamplePage>(icon: "NavigationViewIcon");
				AddMenuItem<PasswordBoxSamplePage>(icon: "PasswordBoxIcon");
				AddMenuItem<ProgressBarSamplePage>(icon: "ProgressBarIcon");
				AddMenuItem<RadioButtonSamplePage>(icon: "RadioButtonIcon");
				// Incomplete : AddMenuItem<SnackBarSamplePage>(icon: "SnackBarIcon");
				AddMenuItem<SliderSamplePage>(icon: "SliderIcon");
				AddMenuItem<StandardBottomSheetSamplePage>(icon: "StandardBottomSheetIcon");
				AddMenuItem<TextBlockSamplePage>(icon: "TextBlockIcon");
				AddMenuItem<TextBoxSamplePage>(icon: "TextBoxIcon");
#if __IOS__ || __ANDROID__
				AddMenuItem<TimePickerSamplePage>(icon: "TimePickerIcon");
#endif
				AddMenuItem<ToggleSwitchSamplePage>(icon: "ToggleSwitchIcon");
			}

			// Controls
			using (AddMenuHeader("Helpers"))
			{
				AddMenuItem<ControlExtensionsSamplePage>();
			}

			IDisposable AddMenuHeader(string content)
			{
				NavView.MenuItems.Add(new NavigationViewItemHeader()
				{
					Content = content,
				});
				return Disposable.Empty;
			}
			void AddMenuItem<TSamplePage>(string content = null, string icon = null)
				where TSamplePage : Page
			{
				NavView.MenuItems.Add(new NavigationViewItem()
				{
					Content = content ?? Regex.Replace(typeof(TSamplePage).Name, @"SamplePage$", string.Empty),
					Icon = icon != null
						? new BitmapIcon()
						{
							UriSource = new Uri($"ms-appx:///Assets/NavigationViewIcons/{icon}.png"),
#if __IOS__
							// workaround for BitmapIcon not displaying on ios
							ShowAsMonochrome = false
#endif
						}
						: null,
					Tag = typeof(TSamplePage),
				});
			}
		}

#endregion

		void ToggleTheme()
		{
#if WINDOWS_UWP
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
#endif
		}
	}
}
