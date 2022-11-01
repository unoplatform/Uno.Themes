using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.MarkupHelpers;

namespace Uno.Themes.Markup
{
	public static partial class Theme
	{
		public static class Styles
		{
			public delegate void StyleDependencyPropertyBuilder(IDependencyPropertyBuilder<Style> builder);
			private static StyleDependencyPropertyBuilder GetStyleBuilder(string key) => delegate { StaticResource.Get<Style>(key); };

			public static class Button
			{
				public static StyleDependencyPropertyBuilder Elevated => GetStyleBuilder("ElevatedButtonStyle");
				public static StyleDependencyPropertyBuilder Filled => GetStyleBuilder("FilledButtonStyle");
				public static StyleDependencyPropertyBuilder FilledTonal => GetStyleBuilder("FilledTonalButtonStyle");
				public static StyleDependencyPropertyBuilder Outlined => GetStyleBuilder("OutlinedButtonStyle");
				public static StyleDependencyPropertyBuilder Text => GetStyleBuilder("TextButtonStyle");
				public static StyleDependencyPropertyBuilder Icon => GetStyleBuilder("IconButtonStyle");
				public static StyleDependencyPropertyBuilder Fab => GetStyleBuilder("FabStyle");
				public static StyleDependencyPropertyBuilder SurfaceFab => GetStyleBuilder("SurfaceFabStyle");
				public static StyleDependencyPropertyBuilder SecondaryFab => GetStyleBuilder("SecondaryFabStyle");
				public static StyleDependencyPropertyBuilder TertiaryFab => GetStyleBuilder("TertiaryFabStyle");
				public static StyleDependencyPropertyBuilder SmallFab => GetStyleBuilder("SmallFabStyle");
				public static StyleDependencyPropertyBuilder SurfaceSmallFab => GetStyleBuilder("SurfaceSmallFabStyle");
				public static StyleDependencyPropertyBuilder SecondarySmallFab => GetStyleBuilder("SecondarySmallFabStyle");
				public static StyleDependencyPropertyBuilder TertiarySmallFab => GetStyleBuilder("TertiarySmallFabStyle");
				public static StyleDependencyPropertyBuilder LargeFab => GetStyleBuilder("LargeFabStyle");
				public static StyleDependencyPropertyBuilder SurfaceLargeFab => GetStyleBuilder("SurfaceLargeFabStyle");
				public static StyleDependencyPropertyBuilder SecondaryLargeFab => GetStyleBuilder("SecondaryLargeFabStyle");
				public static StyleDependencyPropertyBuilder TertiaryLargeFab => GetStyleBuilder("TertiaryLargeFabStyle");
			}

			public static class CalendarDatePicker
			{
				public static StyleDependencyPropertyBuilder Default => GetStyleBuilder("CalendarDatePickerStyle");
			}

			public static class CalendarView
			{
				public static StyleDependencyPropertyBuilder Default => GetStyleBuilder("CalendarViewStyle");
			}

			public static class CheckBox
			{
				public static StyleDependencyPropertyBuilder Default => GetStyleBuilder("CheckBoxStyle");
				public static StyleDependencyPropertyBuilder Secondary => GetStyleBuilder("SecondaryCheckBoxStyle");
			}

			public static class ComboBox
			{
				public static StyleDependencyPropertyBuilder Default => GetStyleBuilder("ComboBoxStyle");
			}

			public static class AppBarButton
			{
				public static StyleDependencyPropertyBuilder Default => GetStyleBuilder("AppBarButtonStyle");
			}

			public static class CommandBar
			{
				public static StyleDependencyPropertyBuilder Default => GetStyleBuilder("CommandBarStyle");
			}

			public static class ContentDialog
			{
				public static StyleDependencyPropertyBuilder Default => GetStyleBuilder("ContentDialogStyle");
			}

			public static class DatePicker
			{
				public static StyleDependencyPropertyBuilder Default => GetStyleBuilder("DatePickerStyle");
			}

			public static class FlyoutPresenter
			{
				public static StyleDependencyPropertyBuilder Default => GetStyleBuilder("FlyoutPresenterStyle");
			}

			public static class MenuFlyoutPresenter
			{
				public static StyleDependencyPropertyBuilder Default => GetStyleBuilder("MenuFlyoutPresenterStyle");
			}

			public static class HyperlinkButton
			{
				public static StyleDependencyPropertyBuilder Default => GetStyleBuilder("HyperlinkButtonStyle");
				public static StyleDependencyPropertyBuilder Secondary => GetStyleBuilder("SecondaryHyperlinkButtonStyle");
			}

			public static class ListViewItem
			{
				public static StyleDependencyPropertyBuilder Default => GetStyleBuilder("ListViewItemStyle");
			}

			public static class ListView
			{
				public static StyleDependencyPropertyBuilder Default => GetStyleBuilder("ListViewStyle");
			}

			public static class NavigationView
			{
				public static StyleDependencyPropertyBuilder Default => GetStyleBuilder("NavigationViewStyle");
			}

			public static class NavigationViewItem
			{
				public static StyleDependencyPropertyBuilder Default => GetStyleBuilder("NavigationViewItemStyle");
			}

			public static class PasswordBox
			{
				public static StyleDependencyPropertyBuilder Filled => GetStyleBuilder("FilledPasswordBoxStyle");
				public static StyleDependencyPropertyBuilder Outlined => GetStyleBuilder("OutlinedPasswordBoxStyle");
			}

			public static class ProgressBar
			{
				public static StyleDependencyPropertyBuilder Default => GetStyleBuilder("ProgressBarStyle");
			}

			public static class ProgressRing
			{
				public static StyleDependencyPropertyBuilder Default => GetStyleBuilder("ProgressRingStyle");
			}

			public static class RadioButton
			{
				public static StyleDependencyPropertyBuilder Default => GetStyleBuilder("RadioButtonStyle");
				public static StyleDependencyPropertyBuilder Secondary => GetStyleBuilder("SecondaryRadioButtonStyle");
			}

			public static class Slider
			{
				public static StyleDependencyPropertyBuilder Default => GetStyleBuilder("SliderStyle");
			}

			public static class TextBlock
			{
				public static StyleDependencyPropertyBuilder DisplayLarge => GetStyleBuilder("DisplayLarge");
				public static StyleDependencyPropertyBuilder DisplayMedium => GetStyleBuilder("DisplayMedium");
				public static StyleDependencyPropertyBuilder DisplaySmall => GetStyleBuilder("DisplaySmall");
				public static StyleDependencyPropertyBuilder HeadlineLarge => GetStyleBuilder("HeadlineLarge");
				public static StyleDependencyPropertyBuilder HeadlineMedium => GetStyleBuilder("HeadlineMedium");
				public static StyleDependencyPropertyBuilder HeadlineSmall => GetStyleBuilder("HeadlineSmall");
				public static StyleDependencyPropertyBuilder TitleLarge => GetStyleBuilder("TitleLarge");
				public static StyleDependencyPropertyBuilder TitleMedium => GetStyleBuilder("TitleMedium");
				public static StyleDependencyPropertyBuilder TitleSmall => GetStyleBuilder("TitleSmall");
				public static StyleDependencyPropertyBuilder LabelLarge => GetStyleBuilder("LabelLarge");
				public static StyleDependencyPropertyBuilder LabelMedium => GetStyleBuilder("LabelMedium");
				public static StyleDependencyPropertyBuilder LabelSmall => GetStyleBuilder("LabelSmall");
				public static StyleDependencyPropertyBuilder LabelExtraSmall => GetStyleBuilder("LabelExtraSmall");
				public static StyleDependencyPropertyBuilder BodyLarge => GetStyleBuilder("BodyLarge");
				public static StyleDependencyPropertyBuilder BodyMedium => GetStyleBuilder("BodyMedium");
				public static StyleDependencyPropertyBuilder BodySmall => GetStyleBuilder("BodySmall");
				public static StyleDependencyPropertyBuilder CaptionLarge => GetStyleBuilder("CaptionLarge");
				public static StyleDependencyPropertyBuilder CaptionMedium => GetStyleBuilder("CaptionMedium");
				public static StyleDependencyPropertyBuilder CaptionSmall => GetStyleBuilder("CaptionSmall");
			}

			public static class TextBox
			{
				public static StyleDependencyPropertyBuilder Filled => GetStyleBuilder("FilledTextBoxStyle");
				public static StyleDependencyPropertyBuilder Outlined => GetStyleBuilder("OutlinedTextBoxStyle");
			}

			public static class ToggleButton
			{
				public static StyleDependencyPropertyBuilder Text => GetStyleBuilder("TextToggleButtonStyle");
				public static StyleDependencyPropertyBuilder Icon => GetStyleBuilder("IconToggleButtonStyle");
			}

			public static class ToggleSwitch
			{
				public static StyleDependencyPropertyBuilder Default => GetStyleBuilder("ToggleSwitchStyle");
			}
		}
	}
}
