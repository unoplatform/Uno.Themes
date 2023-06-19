using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Uno.Themes.Samples.Entities;
using Uno.Themes.Samples.Helpers;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace Uno.Themes.Samples.Content.Controls
{
	[SamplePage(SampleCategory.Controls, "FAB", SourceSdk.UnoMaterial, Description = "Also known as Floating Action Button, the FAB is used for a screen's primary action.", DocumentationLink = "https://material.io/components/buttons-floating-action-button")]
	public sealed partial class FabSamplePage : Page
	{
		public FabSamplePage()
		{
			this.InitializeComponent();

		}

		private void OnClick(object sender, RoutedEventArgs e)
		{
			//App.MyMaterialTheme.UseImplicitStyles = false;

			App.MyMaterialTheme.ColorOverrideDictionary = null;

			//ToggleApplicationTheme((sender as Button).XamlRoot);
			//ToggleApplicationTheme((sender as Button).XamlRoot);
		}
		private void OnClick2(object sender, RoutedEventArgs e)
		{
			//App.MyMaterialTheme.UseImplicitStyles = true;
			App.MyMaterialTheme.ColorOverrideDictionary = new ResourceDictionary { Source = new Uri("ms-appx:///MyOverride.xaml") };

			//ToggleApplicationTheme((sender as Button).XamlRoot);
			//ToggleApplicationTheme((sender as Button).XamlRoot);
		}

		public static ApplicationTheme GetCurrentOsTheme()
		{
			var settings = new UISettings();
			var systemBackground = settings.GetColorValue(UIColorType.Background);
			var black = Color.FromArgb(255, 0, 0, 0);

			return systemBackground == black ? ApplicationTheme.Dark : ApplicationTheme.Light;
		}

		/// <summary>
		/// Gets a <see cref="ApplicationTheme"/> from the provided XamlRoot.
		/// </summary>
		public static ApplicationTheme GetRootTheme(XamlRoot root)
		{
			return (root?.Content as FrameworkElement)?.ActualTheme switch
			{
				ElementTheme.Light => ApplicationTheme.Light,
				ElementTheme.Dark => ApplicationTheme.Dark,

				_ => GetCurrentOsTheme(),
			};
		}

		/// <summary>
		/// Get if the application is currently in dark mode.
		/// </summary>
		public static bool IsAppInDarkMode(XamlRoot root)
			=> GetRootTheme(root) == ApplicationTheme.Dark;

		public static bool IsRootInDarkMode(XamlRoot root)
			=> GetRootTheme(root) == ApplicationTheme.Dark;


		/// <summary>
		/// Sets the theme for the provided XamlRoot
		/// </summary>
		/// <param name="root"></param>
		/// <param name="darkMode"></param>
		public static void SetRootTheme(XamlRoot root, bool darkMode)
			=> SetApplicationTheme(root, darkMode ? ElementTheme.Dark : ElementTheme.Light);

		public static void SetApplicationTheme(XamlRoot? root, ElementTheme theme)
		{
			if (root?.Content is FrameworkElement fe)
			{
				fe.RequestedTheme = theme;
			}
		}

		public static void ToggleApplicationTheme(XamlRoot root)
			=> SetRootTheme(root, !IsAppInDarkMode(root));
	}
}
