using Microsoft.UI.Xaml;
using Uno.Extensions.Markup;
using Uno.Extensions.Markup.Internals;

namespace Uno.Themes.Markup
{
	public static partial class Theme
	{
		public static class Styles
		{
			public static class CalendarDatePicker
			{
				[ResourceKeyDefinition(typeof(Style), "CalendarDatePickerStyle", TargetType = typeof(CalendarDatePicker))]
				public static StaticResourceKey<Style> Default => new("CalendarDatePickerStyle");
			}

			public static class CalendarView
			{
				[ResourceKeyDefinition(typeof(Style), "CalendarViewStyle", TargetType = typeof(CalendarView))]
				public static StaticResourceKey<Style> Default => new("CalendarViewStyle");
			}

			public static class AppBarButton
			{
				[ResourceKeyDefinition(typeof(Style), "AppBarButtonStyle", TargetType = typeof(AppBarButton))]
				public static StaticResourceKey<Style> Default => new("AppBarButtonStyle");
			}

			public static class CommandBar
			{
				[ResourceKeyDefinition(typeof(Style), "CommandBarStyle", TargetType = typeof(CommandBar))]
				public static StaticResourceKey<Style> Default => new("CommandBarStyle");
			}

			public static class ContentDialog
			{
				[ResourceKeyDefinition(typeof(Style), "ContentDialogStyle", TargetType = typeof(ContentDialog))]
				public static StaticResourceKey<Style> Default => new("ContentDialogStyle");
			}

			public static class FlyoutPresenter
			{
				[ResourceKeyDefinition(typeof(Style), "FlyoutPresenterStyle", TargetType = typeof(FlyoutPresenter))]
				public static StaticResourceKey<Style> Default => new("FlyoutPresenterStyle");
			}

			public static class MenuFlyoutItem
			{
				[ResourceKeyDefinition(typeof(Style), "MenuFlyoutItemStyle", TargetType = typeof(MenuFlyoutItem))]
				public static StaticResourceKey<Style> Default => new("MenuFlyoutItemStyle");
			}

			public static class MenuFlyoutPresenter
			{
				[ResourceKeyDefinition(typeof(Style), "MenuFlyoutPresenterStyle", TargetType = typeof(MenuFlyoutPresenter))]
				public static StaticResourceKey<Style> Default => new("MenuFlyoutPresenterStyle");
			}

			public static class ListViewItem
			{
				[ResourceKeyDefinition(typeof(Style), "ListViewItemStyle", TargetType = typeof(ListViewItem))]
				public static StaticResourceKey<Style> Default => new("ListViewItemStyle");
			}

			public static class ListView
			{
				[ResourceKeyDefinition(typeof(Style), "ListViewStyle", TargetType = typeof(ListView))]
				public static StaticResourceKey<Style> Default => new("ListViewStyle");
			}

			public static class NavigationView
			{
				[ResourceKeyDefinition(typeof(Style), "NavigationViewStyle", TargetType = typeof(NavigationView))]
				public static StaticResourceKey<Style> Default => new("NavigationViewStyle");
			}

			public static class NavigationViewItem
			{
				[ResourceKeyDefinition(typeof(Style), "NavigationViewItemStyle", TargetType = typeof(NavigationViewItem))]
				public static StaticResourceKey<Style> Default => new("NavigationViewItemStyle");
			}

			public static class PipsPager
			{
				[ResourceKeyDefinition(typeof(Style), "PipsPagerStyle", TargetType = typeof(PipsPager))]
				public static StaticResourceKey<Style> Default => new("PipsPagerStyle");
			}

			public static class Slider
			{
				[ResourceKeyDefinition(typeof(Style), "SliderStyle", TargetType = typeof(Slider))]
				public static StaticResourceKey<Style> Default => new("SliderStyle");
			}

			public static class ToggleButton
			{
				[ResourceKeyDefinition(typeof(Style), "TextToggleButtonStyle", TargetType = typeof(ToggleButton))]
				public static StaticResourceKey<Style> Text => new("TextToggleButtonStyle");

				[ResourceKeyDefinition(typeof(Style), "IconToggleButtonStyle", TargetType = typeof(ToggleButton))]
				public static StaticResourceKey<Style> Icon => new("IconToggleButtonStyle");
			}
		}
	}
}
