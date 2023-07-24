using Microsoft.UI.Xaml;
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
				public static ResourceValue<Style> Default => new("CalendarDatePickerStyle");
			}

			public static class CalendarView
			{
				[ResourceKeyDefinition(typeof(Style), "CalendarViewStyle", TargetType = typeof(CalendarView))]
				public static ResourceValue<Style> Default => new("CalendarViewStyle");
			}

			public static class CheckBox
			{
				[ResourceKeyDefinition(typeof(Style), "CheckBoxStyle", TargetType = typeof(CheckBox))]
				public static ResourceValue<Style> Default => new("CheckBoxStyle");

				[ResourceKeyDefinition(typeof(Style), "SecondaryCheckBoxStyle", TargetType = typeof(CheckBox))]
				public static ResourceValue<Style> Secondary => new("SecondaryCheckBoxStyle");
			}

			public static class AppBarButton
			{
				[ResourceKeyDefinition(typeof(Style), "AppBarButtonStyle", TargetType = typeof(AppBarButton))]
				public static ResourceValue<Style> Default => new("AppBarButtonStyle");
			}

			public static class CommandBar
			{
				[ResourceKeyDefinition(typeof(Style), "CommandBarStyle", TargetType = typeof(CommandBar))]
				public static ResourceValue<Style> Default => new("CommandBarStyle");
			}

			public static class ContentDialog
			{
				[ResourceKeyDefinition(typeof(Style), "ContentDialogStyle", TargetType = typeof(ContentDialog))]
				public static ResourceValue<Style> Default => new("ContentDialogStyle");
			}

			public static class DatePicker
			{
				[ResourceKeyDefinition(typeof(Style), "DatePickerStyle", TargetType = typeof(DatePicker))]
				public static ResourceValue<Style> Default => new("DatePickerStyle");
			}

			public static class FlyoutPresenter
			{
				[ResourceKeyDefinition(typeof(Style), "FlyoutPresenterStyle", TargetType = typeof(FlyoutPresenter))]
				public static ResourceValue<Style> Default => new("FlyoutPresenterStyle");
			}

			public static class MenuFlyoutItem
			{
				[ResourceKeyDefinition(typeof(Style), "MenuFlyoutItemStyle", TargetType = typeof(MenuFlyoutItem))]
				public static ResourceValue<Style> Default => new("MenuFlyoutItemStyle");
			}

			public static class MenuFlyoutPresenter
			{
				[ResourceKeyDefinition(typeof(Style), "MenuFlyoutPresenterStyle", TargetType = typeof(MenuFlyoutPresenter))]
				public static ResourceValue<Style> Default => new("MenuFlyoutPresenterStyle");
			}

			public static class HyperlinkButton
			{
				[ResourceKeyDefinition(typeof(Style), "HyperlinkButtonStyle", TargetType = typeof(HyperlinkButton))]
				public static ResourceValue<Style> Default => new("HyperlinkButtonStyle");

				[ResourceKeyDefinition(typeof(Style), "SecondaryHyperlinkButtonStyle", TargetType = typeof(HyperlinkButton))]
				public static ResourceValue<Style> Secondary => new("SecondaryHyperlinkButtonStyle");
			}

			public static class ListViewItem
			{
				[ResourceKeyDefinition(typeof(Style), "ListViewItemStyle", TargetType = typeof(ListViewItem))]
				public static ResourceValue<Style> Default => new("ListViewItemStyle");
			}

			public static class ListView
			{
				[ResourceKeyDefinition(typeof(Style), "ListViewStyle", TargetType = typeof(ListView))]
				public static ResourceValue<Style> Default => new("ListViewStyle");
			}

			public static class NavigationView
			{
				[ResourceKeyDefinition(typeof(Style), "NavigationViewStyle", TargetType = typeof(NavigationView))]
				public static ResourceValue<Style> Default => new("NavigationViewStyle");
			}

			public static class NavigationViewItem
			{
				[ResourceKeyDefinition(typeof(Style), "NavigationViewItemStyle", TargetType = typeof(NavigationViewItem))]
				public static ResourceValue<Style> Default => new("NavigationViewItemStyle");
			}

			public static class PipsPager
			{
				[ResourceKeyDefinition(typeof(Style), "PipsPagerStyle", TargetType = typeof(PipsPager))]
				public static ResourceValue<Style> Default => new("PipsPagerStyle");
			}

			public static class ProgressBar
			{
				[ResourceKeyDefinition(typeof(Style), "ProgressBarStyle", TargetType = typeof(ProgressBar))]
				public static ResourceValue<Style> Default => new("ProgressBarStyle");
			}

			public static class ProgressRing
			{
				[ResourceKeyDefinition(typeof(Style), "ProgressRingStyle", TargetType = typeof(ProgressRing))]
				public static ResourceValue<Style> Default => new("ProgressRingStyle");
			}

			public static class RadioButton
			{
				[ResourceKeyDefinition(typeof(Style), "RadioButtonStyle", TargetType = typeof(RadioButton))]
				public static ResourceValue<Style> Default => new("RadioButtonStyle");

				[ResourceKeyDefinition(typeof(Style), "SecondaryRadioButtonStyle", TargetType = typeof(RadioButton))]
				public static ResourceValue<Style> Secondary => new("SecondaryRadioButtonStyle");
			}

			public static class Slider
			{
				[ResourceKeyDefinition(typeof(Style), "SliderStyle", TargetType = typeof(Slider))]
				public static ResourceValue<Style> Default => new("SliderStyle");
			}

			public static class TextBlock
			{
				[ResourceKeyDefinition(typeof(Style), "DisplayLarge", TargetType = typeof(TextBlock))]
				public static ResourceValue<Style> DisplayLarge => new("DisplayLarge");

				[ResourceKeyDefinition(typeof(Style), "DisplayMedium", TargetType = typeof(TextBlock))]
				public static ResourceValue<Style> DisplayMedium => new("DisplayMedium");

				[ResourceKeyDefinition(typeof(Style), "DisplaySmall", TargetType = typeof(TextBlock))]
				public static ResourceValue<Style> DisplaySmall => new("DisplaySmall");

				[ResourceKeyDefinition(typeof(Style), "HeadlineLarge", TargetType = typeof(TextBlock))]
				public static ResourceValue<Style> HeadlineLarge => new("HeadlineLarge");

				[ResourceKeyDefinition(typeof(Style), "HeadlineMedium", TargetType = typeof(TextBlock))]
				public static ResourceValue<Style> HeadlineMedium => new("HeadlineMedium");

				[ResourceKeyDefinition(typeof(Style), "HeadlineSmall", TargetType = typeof(TextBlock))]
				public static ResourceValue<Style> HeadlineSmall => new("HeadlineSmall");

				[ResourceKeyDefinition(typeof(Style), "TitleLarge", TargetType = typeof(TextBlock))]
				public static ResourceValue<Style> TitleLarge => new("TitleLarge");

				[ResourceKeyDefinition(typeof(Style), "TitleMedium", TargetType = typeof(TextBlock))]
				public static ResourceValue<Style> TitleMedium => new("TitleMedium");

				[ResourceKeyDefinition(typeof(Style), "TitleSmall", TargetType = typeof(TextBlock))]
				public static ResourceValue<Style> TitleSmall => new("TitleSmall");

				[ResourceKeyDefinition(typeof(Style), "LabelLarge", TargetType = typeof(TextBlock))]
				public static ResourceValue<Style> LabelLarge => new("LabelLarge");

				[ResourceKeyDefinition(typeof(Style), "LabelMedium", TargetType = typeof(TextBlock))]
				public static ResourceValue<Style> LabelMedium => new("LabelMedium");

				[ResourceKeyDefinition(typeof(Style), "LabelSmall", TargetType = typeof(TextBlock))]
				public static ResourceValue<Style> LabelSmall => new("LabelSmall");

				[ResourceKeyDefinition(typeof(Style), "LabelExtraSmall", TargetType = typeof(TextBlock))]
				public static ResourceValue<Style> LabelExtraSmall => new("LabelExtraSmall");

				[ResourceKeyDefinition(typeof(Style), "BodyLarge", TargetType = typeof(TextBlock))]
				public static ResourceValue<Style> BodyLarge => new("BodyLarge");

				[ResourceKeyDefinition(typeof(Style), "BodyMedium", TargetType = typeof(TextBlock))]
				public static ResourceValue<Style> BodyMedium => new("BodyMedium");

				[ResourceKeyDefinition(typeof(Style), "BodySmall", TargetType = typeof(TextBlock))]
				public static ResourceValue<Style> BodySmall => new("BodySmall");

				[ResourceKeyDefinition(typeof(Style), "CaptionLarge", TargetType = typeof(TextBlock))]
				public static ResourceValue<Style> CaptionLarge => new("CaptionLarge");

				[ResourceKeyDefinition(typeof(Style), "CaptionMedium", TargetType = typeof(TextBlock))]
				public static ResourceValue<Style> CaptionMedium => new("CaptionMedium");

				[ResourceKeyDefinition(typeof(Style), "CaptionSmall", TargetType = typeof(TextBlock))]
				public static ResourceValue<Style> CaptionSmall => new("CaptionSmall");
			}

			public static class TextBox
			{
				[ResourceKeyDefinition(typeof(Style), "FilledTextBoxStyle", TargetType = typeof(TextBox))]
				public static ResourceValue<Style> Filled => new("FilledTextBoxStyle");

				[ResourceKeyDefinition(typeof(Style), "OutlinedTextBoxStyle", TargetType = typeof(TextBox))]
				public static ResourceValue<Style> Outlined => new("OutlinedTextBoxStyle");
			}

			public static class ToggleButton
			{
				[ResourceKeyDefinition(typeof(Style), "TextToggleButtonStyle", TargetType = typeof(ToggleButton))]
				public static ResourceValue<Style> Text => new("TextToggleButtonStyle");

				[ResourceKeyDefinition(typeof(Style), "IconToggleButtonStyle", TargetType = typeof(ToggleButton))]
				public static ResourceValue<Style> Icon => new("IconToggleButtonStyle");
			}

			public static class ToggleSwitch
			{
				[ResourceKeyDefinition(typeof(Style), "ToggleSwitchStyle", TargetType = typeof(ToggleSwitch))]
				public static ResourceValue<Style> Default => new("ToggleSwitchStyle");
			}
		}
	}
}
