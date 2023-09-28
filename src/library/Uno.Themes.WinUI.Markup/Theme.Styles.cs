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

			public static class TextBlock
			{
				[ResourceKeyDefinition(typeof(Style), "DisplayLarge", TargetType = typeof(TextBlock))]
				public static StaticResourceKey<Style> DisplayLarge => new("DisplayLarge");

				[ResourceKeyDefinition(typeof(Style), "DisplayMedium", TargetType = typeof(TextBlock))]
				public static StaticResourceKey<Style> DisplayMedium => new("DisplayMedium");

				[ResourceKeyDefinition(typeof(Style), "DisplaySmall", TargetType = typeof(TextBlock))]
				public static StaticResourceKey<Style> DisplaySmall => new("DisplaySmall");

				[ResourceKeyDefinition(typeof(Style), "HeadlineLarge", TargetType = typeof(TextBlock))]
				public static StaticResourceKey<Style> HeadlineLarge => new("HeadlineLarge");

				[ResourceKeyDefinition(typeof(Style), "HeadlineMedium", TargetType = typeof(TextBlock))]
				public static StaticResourceKey<Style> HeadlineMedium => new("HeadlineMedium");

				[ResourceKeyDefinition(typeof(Style), "HeadlineSmall", TargetType = typeof(TextBlock))]
				public static StaticResourceKey<Style> HeadlineSmall => new("HeadlineSmall");

				[ResourceKeyDefinition(typeof(Style), "TitleLarge", TargetType = typeof(TextBlock))]
				public static StaticResourceKey<Style> TitleLarge => new("TitleLarge");

				[ResourceKeyDefinition(typeof(Style), "TitleMedium", TargetType = typeof(TextBlock))]
				public static StaticResourceKey<Style> TitleMedium => new("TitleMedium");

				[ResourceKeyDefinition(typeof(Style), "TitleSmall", TargetType = typeof(TextBlock))]
				public static StaticResourceKey<Style> TitleSmall => new("TitleSmall");

				[ResourceKeyDefinition(typeof(Style), "LabelLarge", TargetType = typeof(TextBlock))]
				public static StaticResourceKey<Style> LabelLarge => new("LabelLarge");

				[ResourceKeyDefinition(typeof(Style), "LabelMedium", TargetType = typeof(TextBlock))]
				public static StaticResourceKey<Style> LabelMedium => new("LabelMedium");

				[ResourceKeyDefinition(typeof(Style), "LabelSmall", TargetType = typeof(TextBlock))]
				public static StaticResourceKey<Style> LabelSmall => new("LabelSmall");

				[ResourceKeyDefinition(typeof(Style), "LabelExtraSmall", TargetType = typeof(TextBlock))]
				public static StaticResourceKey<Style> LabelExtraSmall => new("LabelExtraSmall");

				[ResourceKeyDefinition(typeof(Style), "BodyLarge", TargetType = typeof(TextBlock))]
				public static StaticResourceKey<Style> BodyLarge => new("BodyLarge");

				[ResourceKeyDefinition(typeof(Style), "BodyMedium", TargetType = typeof(TextBlock))]
				public static StaticResourceKey<Style> BodyMedium => new("BodyMedium");

				[ResourceKeyDefinition(typeof(Style), "BodySmall", TargetType = typeof(TextBlock))]
				public static StaticResourceKey<Style> BodySmall => new("BodySmall");

				[ResourceKeyDefinition(typeof(Style), "CaptionLarge", TargetType = typeof(TextBlock))]
				public static StaticResourceKey<Style> CaptionLarge => new("CaptionLarge");

				[ResourceKeyDefinition(typeof(Style), "CaptionMedium", TargetType = typeof(TextBlock))]
				public static StaticResourceKey<Style> CaptionMedium => new("CaptionMedium");

				[ResourceKeyDefinition(typeof(Style), "CaptionSmall", TargetType = typeof(TextBlock))]
				public static StaticResourceKey<Style> CaptionSmall => new("CaptionSmall");
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
