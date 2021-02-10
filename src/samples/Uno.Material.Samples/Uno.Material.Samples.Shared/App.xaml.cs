using Microsoft.Extensions.Logging;
using ShowMeTheXAML;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices.WindowsRuntime;
using Uno.Extensions;
using Uno.Material.Samples.Content.Styles;
using Uno.Material.Samples.Entities;
using Uno.Material.Samples.Helpers;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Automation;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using MUXC = Microsoft.UI.Xaml.Controls;
using MUXCP = Microsoft.UI.Xaml.Controls.Primitives;

namespace Uno.Material.Samples
{
	/// <summary>
	/// Provides application-specific behavior to supplement the default Application class.
	/// </summary>
	sealed partial class App : Application
	{

		private Shell _shell;

		/// <summary>
		/// Initializes the singleton application object.  This is the first line of authored code
		/// executed, and as such is the logical equivalent of main() or WinMain().
		/// </summary>
		public App()
		{
			ConfigureFilters(global::Uno.Extensions.LogExtensionPoint.AmbientLoggerFactory);
			ConfigureXamlDisplay();

			this.InitializeComponent();
			this.Suspending += OnSuspending;
		}

		/// <summary>
		/// Invoked when the application is launched normally by the end user.  Other entry points
		/// will be used such as when the application is launched to open a specific file.
		/// </summary>
		/// <param name="e">Details about the launch request and process.</param>
		protected override void OnLaunched(LaunchActivatedEventArgs e)
		{
			Uno.Material.Resources.Init(this, new ResourceDictionary() { Source = new Uri("ms-appx:///ColorPaletteOverride.xaml") });
			Uno.Cupertino.Resources.Init(this, null);

#if DEBUG
			if (System.Diagnostics.Debugger.IsAttached)
			{
				// this.DebugSettings.EnableFrameRateCounter = true;
			}
#endif

#if WINDOWS_UWP
			ApplicationView.GetForCurrentView().SetPreferredMinSize(new Size(320, 568)); // (size of the iPhone SE)
#endif

			var window = Windows.UI.Xaml.Window.Current;
			if (!(window.Content is Shell))
			{
				window.Content = _shell = BuildShell();
			}

			// Ensure the current window is active
			window.Activate();
		}

		/// <summary>
		/// Invoked when Navigation to a certain page fails
		/// </summary>
		/// <param name="sender">The Frame which failed navigation</param>
		/// <param name="e">Details about the navigation failure</param>
		void OnNavigationFailed(object sender, NavigationFailedEventArgs e)
		{
			throw new Exception($"Failed to load {e.SourcePageType.FullName}: {e.Exception}");
		}

		/// <summary>
		/// Invoked when application execution is being suspended.  Application state is saved
		/// without knowing whether the application will be terminated or resumed with the contents
		/// of memory still intact.
		/// </summary>
		/// <param name="sender">The source of the suspend request.</param>
		/// <param name="e">Details about the suspend request.</param>
		private void OnSuspending(object sender, SuspendingEventArgs e)
		{
			var deferral = e.SuspendingOperation.GetDeferral();
			//TODO: Save application state and stop any background activity
			deferral.Complete();
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
					? nv.MenuItems
						.OfType<MUXC.NavigationViewItem>()
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
			ShellNavigateTo<ColorsSamplePage>(
#if !WINDOWS_UWP
				// workaround for uno#5069: setting NavView.SelectedItem at launch bricks it
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
				.Where(x => x.Namespace?.StartsWith("Uno.Material.Samples") == true)
				.Select(x => new { TypeInfo = x, SamplePageAttribute = x.GetCustomAttribute<SamplePageAttribute>() })
				.Where(x => x.SamplePageAttribute != null)
				.Select(x => new Sample(x.SamplePageAttribute, x.TypeInfo.AsType()))
				.OrderByDescending(x => x.SortOrder.HasValue)
				.ThenBy(x => x.SortOrder)
				.ThenBy(x => x.Title)
				.GroupBy(x => x.Category);

			foreach (var category in categories.OrderBy(x => x.Key))
			{
				var tier = 1;

				var parentItem = default(MUXC.NavigationViewItem);
				if (category.Key != SampleCategory.None)
				{
					parentItem = new MUXC.NavigationViewItem
					{
						Content = category.Key.GetDescription() ?? category.Key.ToString(),
						SelectsOnInvoked = false,
						Style = (Style)Resources[$"T{tier++}NavigationViewItemStyle"]
					}.Apply(NavViewItemVisualStateFix);
					AutomationProperties.SetAutomationId(parentItem, "Section_" + parentItem.Content);

					nv.MenuItems.Add(parentItem);
				}

				foreach (var sample in category)
				{
					var item = new MUXC.NavigationViewItem
					{
						Content = sample.Title,
						DataContext = sample,
						Style = (Style)Resources[$"T{tier}NavigationViewItemStyle"]
					}.Apply(NavViewItemVisualStateFix);
					AutomationProperties.SetAutomationId(item, "Section_" + item.Content);

					(parentItem?.MenuItems ?? nv.MenuItems).Add(item);
				}
			}

			void NavViewItemVisualStateFix(MUXC.NavigationViewItem nvi)
			{
				// gallery#107: on uwp and uno, deselecting a NVI by selecting another NVI will leave the former in the "Selected" state
				// to workaround this, we force reset the visual state when IsSelected becomes false
				nvi.RegisterPropertyChangedCallback(MUXC.NavigationViewItemBase.IsSelectedProperty, (s, e) =>
				{
					if (!nvi.IsSelected)
					{
						// depending on the DisplayMode, a NVIP may or may not be used.
						var nvip = VisualTreeHelperEx.GetFirstDescendant<MUXCP.NavigationViewItemPresenter>(nvi, x => x.Name == "NavigationViewItemPresenter");
						VisualStateManager.GoToState((Control)nvip ?? nvi, "Normal", true);
					}
				});
			}
		}

		/// <summary>
		/// Configures global logging
		/// </summary>
		/// <param name="factory"></param>
		static void ConfigureFilters(ILoggerFactory factory)
		{
			factory
				.WithFilter(new FilterLoggerSettings
					{
						{ "Uno", LogLevel.Warning },
						{ "Windows", LogLevel.Warning },

						// Debug JS interop
						// { "Uno.Foundation.WebAssemblyRuntime", LogLevel.Debug },

						// Generic Xaml events
						// { "Windows.UI.Xaml", LogLevel.Debug },
						// { "Windows.UI.Xaml.VisualStateGroup", LogLevel.Debug },
						// { "Windows.UI.Xaml.StateTriggerBase", LogLevel.Debug },
						// { "Windows.UI.Xaml.UIElement", LogLevel.Debug },

						// Layouter specific messages
						// { "Windows.UI.Xaml.Controls", LogLevel.Debug },
						// { "Windows.UI.Xaml.Controls.Layouter", LogLevel.Debug },
						// { "Windows.UI.Xaml.Controls.Panel", LogLevel.Debug },
						// { "Windows.Storage", LogLevel.Debug },

						// Binding related messages
						// { "Windows.UI.Xaml.Data", LogLevel.Debug },

						// DependencyObject memory references tracking
						// { "ReferenceHolder", LogLevel.Debug },
					}
				)
#if DEBUG
				.AddConsole(LogLevel.Debug);
#else
				.AddConsole(LogLevel.Information);
#endif
		}

		static void ConfigureXamlDisplay()
		{
			XamlDisplay.Init();
		}
	}
}
