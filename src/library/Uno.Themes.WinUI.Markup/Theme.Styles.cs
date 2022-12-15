using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.MarkupHelpers.Internals;

namespace Uno.Themes.Markup
{
	public static partial class Theme
	{
		public static class Styles
		{
			public static class Button
			{
				[ResourceKeyDefinition(typeof(Style), "ElevatedButtonStyle", TargetType = typeof(Button))]
				public static ResourceValue<Style> Elevated => new("ElevatedButtonStyle", false);

				[ResourceKeyDefinition(typeof(Style), "FilledButtonStyle", TargetType = typeof(Button))]
				public static ResourceValue<Style> Filled => new("FilledButtonStyle", false);

				[ResourceKeyDefinition(typeof(Style), "FilledTonalButtonStyle", TargetType = typeof(Button))]
				public static ResourceValue<Style> FilledTonal => new("FilledTonalButtonStyle", false);

				[ResourceKeyDefinition(typeof(Style), "OutlinedButtonStyle", TargetType = typeof(Button))]
				public static ResourceValue<Style> Outlined => new("OutlinedButtonStyle", false);

				[ResourceKeyDefinition(typeof(Style), "TextButtonStyle", TargetType = typeof(Button))]
				public static ResourceValue<Style> Text => new("TextButtonStyle", false);

				[ResourceKeyDefinition(typeof(Style), "IconButtonStyle", TargetType = typeof(Button))]
				public static ResourceValue<Style> Icon => new("IconButtonStyle", false);

				[ResourceKeyDefinition(typeof(Style), "FabStyle", TargetType = typeof(Button))]
				public static ResourceValue<Style> Fab => new("FabStyle", false);

				[ResourceKeyDefinition(typeof(Style), "SurfaceFabStyle", TargetType = typeof(Button))]
				public static ResourceValue<Style> SurfaceFab => new("SurfaceFabStyle", false);

				[ResourceKeyDefinition(typeof(Style), "SecondaryFabStyle", TargetType = typeof(Button))]
				public static ResourceValue<Style> SecondaryFab => new("SecondaryFabStyle", false);

				[ResourceKeyDefinition(typeof(Style), "TertiaryFabStyle", TargetType = typeof(Button))]
				public static ResourceValue<Style> TertiaryFab => new("TertiaryFabStyle", false);

				[ResourceKeyDefinition(typeof(Style), "SmallFabStyle", TargetType = typeof(Button))]
				public static ResourceValue<Style> SmallFab => new("SmallFabStyle", false);

				[ResourceKeyDefinition(typeof(Style), "SurfaceSmallFabStyle", TargetType = typeof(Button))]
				public static ResourceValue<Style> SurfaceSmallFab => new("SurfaceSmallFabStyle", false);

				[ResourceKeyDefinition(typeof(Style), "SecondarySmallFabStyle", TargetType = typeof(Button))]
				public static ResourceValue<Style> SecondarySmallFab => new("SecondarySmallFabStyle", false);

				[ResourceKeyDefinition(typeof(Style), "TertiarySmallFabStyle", TargetType = typeof(Button))]
				public static ResourceValue<Style> TertiarySmallFab => new("TertiarySmallFabStyle", false);

				[ResourceKeyDefinition(typeof(Style), "LargeFabStyle", TargetType = typeof(Button))]
				public static ResourceValue<Style> LargeFab => new("LargeFabStyle", false);

				[ResourceKeyDefinition(typeof(Style), "SurfaceLargeFabStyle", TargetType = typeof(Button))]
				public static ResourceValue<Style> SurfaceLargeFab => new("SurfaceLargeFabStyle", false);

				[ResourceKeyDefinition(typeof(Style), "SecondaryLargeFabStyle", TargetType = typeof(Button))]
				public static ResourceValue<Style> SecondaryLargeFab => new("SecondaryLargeFabStyle", false);

				[ResourceKeyDefinition(typeof(Style), "TertiaryLargeFabStyle", TargetType = typeof(Button))]
				public static ResourceValue<Style> TertiaryLargeFab => new("TertiaryLargeFabStyle", false);
			}

			public static class CalendarDatePicker
			{
				[ResourceKeyDefinition(typeof(Style), "CalendarDatePickerStyle", TargetType = typeof(CalendarDatePicker))]
				public static ResourceValue<Style> Default => new("CalendarDatePickerStyle", false);
			}

			public static class CalendarView
			{
				[ResourceKeyDefinition(typeof(Style), "CalendarViewStyle", TargetType = typeof(CalendarView))]
				public static ResourceValue<Style> Default => new("CalendarViewStyle", false);
			}

			public static class CheckBox
			{
				[ResourceKeyDefinition(typeof(Style), "CheckBoxStyle", TargetType = typeof(CheckBox))]
				public static ResourceValue<Style> Default => new("CheckBoxStyle", false);

				[ResourceKeyDefinition(typeof(Style), "SecondaryCheckBoxStyle", TargetType = typeof(CheckBox))]
				public static ResourceValue<Style> Secondary => new("SecondaryCheckBoxStyle", false);
			}

			public static class ComboBox
			{
				[ResourceKeyDefinition(typeof(Style), "ComboBoxStyle", TargetType = typeof(ComboBox))]
				public static ResourceValue<Style> Default => new("ComboBoxStyle", false);
			}

			public static class AppBarButton
			{
				[ResourceKeyDefinition(typeof(Style), "AppBarButtonStyle", TargetType = typeof(AppBarButton))]
				public static ResourceValue<Style> Default => new("AppBarButtonStyle", false);
			}

			public static class CommandBar
			{
				[ResourceKeyDefinition(typeof(Style), "CommandBarStyle", TargetType = typeof(CommandBar))]
				public static ResourceValue<Style> Default => new("CommandBarStyle", false);
			}

			public static class ContentDialog
			{
				[ResourceKeyDefinition(typeof(Style), "ContentDialogStyle", TargetType = typeof(ContentDialog))]
				public static ResourceValue<Style> Default => new("ContentDialogStyle", false);
			}

			public static class DatePicker
			{
				[ResourceKeyDefinition(typeof(Style), "DatePickerStyle", TargetType = typeof(DatePicker))]
				public static ResourceValue<Style> Default => new("DatePickerStyle", false);
			}

			public static class FlyoutPresenter
			{
				[ResourceKeyDefinition(typeof(Style), "FlyoutPresenterStyle", TargetType = typeof(FlyoutPresenter))]
				public static ResourceValue<Style> Default => new("FlyoutPresenterStyle", false);
			}

			public static class MenuFlyoutPresenter
			{
				[ResourceKeyDefinition(typeof(Style), "MenuFlyoutPresenterStyle", TargetType = typeof(MenuFlyoutPresenter))]
				public static ResourceValue<Style> Default => new("MenuFlyoutPresenterStyle", false);
			}

			public static class HyperlinkButton
			{
				[ResourceKeyDefinition(typeof(Style), "HyperlinkButtonStyle", TargetType = typeof(HyperlinkButton))]
				public static ResourceValue<Style> Default => new("HyperlinkButtonStyle", false);

				[ResourceKeyDefinition(typeof(Style), "SecondaryHyperlinkButtonStyle", TargetType = typeof(HyperlinkButton))]
				public static ResourceValue<Style> Secondary => new("SecondaryHyperlinkButtonStyle", false);
			}

			public static class ListViewItem
			{
				[ResourceKeyDefinition(typeof(Style), "ListViewItemStyle", TargetType = typeof(ListViewItem))]
				public static ResourceValue<Style> Default => new("ListViewItemStyle", false);
			}

			public static class ListView
			{
				[ResourceKeyDefinition(typeof(Style), "ListViewStyle", TargetType = typeof(ListView))]
				public static ResourceValue<Style> Default => new("ListViewStyle", false);
			}

			public static class NavigationView
			{
				[ResourceKeyDefinition(typeof(Style), "NavigationViewStyle", TargetType = typeof(NavigationView))]
				public static ResourceValue<Style> Default => new("NavigationViewStyle", false);
			}

			public static class NavigationViewItem
			{
				[ResourceKeyDefinition(typeof(Style), "NavigationViewItemStyle", TargetType = typeof(NavigationViewItem))]
				public static ResourceValue<Style> Default => new("NavigationViewItemStyle", false);
			}

			public static class PasswordBox
			{
				[ResourceKeyDefinition(typeof(Style), "FilledPasswordBoxStyle", TargetType = typeof(PasswordBox))]
				public static ResourceValue<Style> Filled => new("FilledPasswordBoxStyle", false);

				[ResourceKeyDefinition(typeof(Style), "OutlinedPasswordBoxStyle", TargetType = typeof(PasswordBox))]
				public static ResourceValue<Style> Outlined => new("OutlinedPasswordBoxStyle", false);
			}

			public static class ProgressBar
			{
				[ResourceKeyDefinition(typeof(Style), "ProgressBarStyle", TargetType = typeof(ProgressBar))]
				public static ResourceValue<Style> Default => new("ProgressBarStyle", false);
			}

			public static class ProgressRing
			{
				[ResourceKeyDefinition(typeof(Style), "ProgressRingStyle", TargetType = typeof(ProgressRing))]
				public static ResourceValue<Style> Default => new("ProgressRingStyle", false);
			}

			public static class RadioButton
			{
				[ResourceKeyDefinition(typeof(Style), "RadioButtonStyle", TargetType = typeof(RadioButton))]
				public static ResourceValue<Style> Default => new("RadioButtonStyle", false);

				[ResourceKeyDefinition(typeof(Style), "SecondaryRadioButtonStyle", TargetType = typeof(RadioButton))]
				public static ResourceValue<Style> Secondary => new("SecondaryRadioButtonStyle", false);
			}

			public static class Slider
			{
				[ResourceKeyDefinition(typeof(Style), "SliderStyle", TargetType = typeof(Slider))]
				public static ResourceValue<Style> Default => new("SliderStyle", false);
			}

			public static class TextBlock
			{
				[ResourceKeyDefinition(typeof(Style), "DisplayLarge", TargetType = typeof(TextBlock))]
				public static ResourceValue<Style> DisplayLarge => new("DisplayLarge", false);

				[ResourceKeyDefinition(typeof(Style), "DisplayMedium", TargetType = typeof(TextBlock))]
				public static ResourceValue<Style> DisplayMedium => new("DisplayMedium", false);

				[ResourceKeyDefinition(typeof(Style), "DisplaySmall", TargetType = typeof(TextBlock))]
				public static ResourceValue<Style> DisplaySmall => new("DisplaySmall", false);

				[ResourceKeyDefinition(typeof(Style), "HeadlineLarge", TargetType = typeof(TextBlock))]
				public static ResourceValue<Style> HeadlineLarge => new("HeadlineLarge", false);

				[ResourceKeyDefinition(typeof(Style), "HeadlineMedium", TargetType = typeof(TextBlock))]
				public static ResourceValue<Style> HeadlineMedium => new("HeadlineMedium", false);

				[ResourceKeyDefinition(typeof(Style), "HeadlineSmall", TargetType = typeof(TextBlock))]
				public static ResourceValue<Style> HeadlineSmall => new("HeadlineSmall", false);

				[ResourceKeyDefinition(typeof(Style), "TitleLarge", TargetType = typeof(TextBlock))]
				public static ResourceValue<Style> TitleLarge => new("TitleLarge", false);

				[ResourceKeyDefinition(typeof(Style), "TitleMedium", TargetType = typeof(TextBlock))]
				public static ResourceValue<Style> TitleMedium => new("TitleMedium", false);

				[ResourceKeyDefinition(typeof(Style), "TitleSmall", TargetType = typeof(TextBlock))]
				public static ResourceValue<Style> TitleSmall => new("TitleSmall", false);

				[ResourceKeyDefinition(typeof(Style), "LabelLarge", TargetType = typeof(TextBlock))]
				public static ResourceValue<Style> LabelLarge => new("LabelLarge", false);

				[ResourceKeyDefinition(typeof(Style), "LabelMedium", TargetType = typeof(TextBlock))]
				public static ResourceValue<Style> LabelMedium => new("LabelMedium", false);

				[ResourceKeyDefinition(typeof(Style), "LabelSmall", TargetType = typeof(TextBlock))]
				public static ResourceValue<Style> LabelSmall => new("LabelSmall", false);

				[ResourceKeyDefinition(typeof(Style), "LabelExtraSmall", TargetType = typeof(TextBlock))]
				public static ResourceValue<Style> LabelExtraSmall => new("LabelExtraSmall", false);

				[ResourceKeyDefinition(typeof(Style), "BodyLarge", TargetType = typeof(TextBlock))]
				public static ResourceValue<Style> BodyLarge => new("BodyLarge", false);

				[ResourceKeyDefinition(typeof(Style), "BodyMedium", TargetType = typeof(TextBlock))]
				public static ResourceValue<Style> BodyMedium => new("BodyMedium", false);

				[ResourceKeyDefinition(typeof(Style), "BodySmall", TargetType = typeof(TextBlock))]
				public static ResourceValue<Style> BodySmall => new("BodySmall", false);

				[ResourceKeyDefinition(typeof(Style), "CaptionLarge", TargetType = typeof(TextBlock))]
				public static ResourceValue<Style> CaptionLarge => new("CaptionLarge", false);

				[ResourceKeyDefinition(typeof(Style), "CaptionMedium", TargetType = typeof(TextBlock))]
				public static ResourceValue<Style> CaptionMedium => new("CaptionMedium", false);

				[ResourceKeyDefinition(typeof(Style), "CaptionSmall", TargetType = typeof(TextBlock))]
				public static ResourceValue<Style> CaptionSmall => new("CaptionSmall", false);
			}

			public static class TextBox
			{
				[ResourceKeyDefinition(typeof(Style), "FilledTextBoxStyle", TargetType = typeof(TextBox))]
				public static ResourceValue<Style> Filled => new("FilledTextBoxStyle", false);

				[ResourceKeyDefinition(typeof(Style), "OutlinedTextBoxStyle", TargetType = typeof(TextBox))]
				public static ResourceValue<Style> Outlined => new("OutlinedTextBoxStyle", false);
			}

			public static class ToggleButton
			{
				[ResourceKeyDefinition(typeof(Style), "TextToggleButtonStyle", TargetType = typeof(ToggleButton))]
				public static ResourceValue<Style> Text => new("TextToggleButtonStyle", false);

				[ResourceKeyDefinition(typeof(Style), "IconToggleButtonStyle", TargetType = typeof(ToggleButton))]
				public static ResourceValue<Style> Icon => new("IconToggleButtonStyle", false);
			}

			public static class ToggleSwitch
			{
				[ResourceKeyDefinition(typeof(Style), "ToggleSwitchStyle", TargetType = typeof(ToggleSwitch))]
				public static ResourceValue<Style> Default => new("ToggleSwitchStyle", false);
			}
		}
	}
}
