using System;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.MarkupHelpers;

namespace Uno.Themes.Markup
{
	public static partial class Theme
	{
		public static class Styles
		{
			public static class Button
			{
				public static Action<IDependencyPropertyBuilder<Style>> Elevated => StaticResource.Get<Style>("ElevatedButtonStyle");
				public static Action<IDependencyPropertyBuilder<Style>> Filled => StaticResource.Get<Style>("FilledButtonStyle");
				public static Action<IDependencyPropertyBuilder<Style>> FilledTonal => StaticResource.Get<Style>("FilledTonalButtonStyle");
				public static Action<IDependencyPropertyBuilder<Style>> Outlined => StaticResource.Get<Style>("OutlinedButtonStyle");
				public static Action<IDependencyPropertyBuilder<Style>> Text => StaticResource.Get<Style>("TextButtonStyle");
				public static Action<IDependencyPropertyBuilder<Style>> Icon => StaticResource.Get<Style>("IconButtonStyle");
				public static Action<IDependencyPropertyBuilder<Style>> Fab => StaticResource.Get<Style>("FabStyle");
				public static Action<IDependencyPropertyBuilder<Style>> SurfaceFab => StaticResource.Get<Style>("SurfaceFabStyle");
				public static Action<IDependencyPropertyBuilder<Style>> SecondaryFab => StaticResource.Get<Style>("SecondaryFabStyle");
				public static Action<IDependencyPropertyBuilder<Style>> TertiaryFab => StaticResource.Get<Style>("TertiaryFabStyle");
				public static Action<IDependencyPropertyBuilder<Style>> SmallFab => StaticResource.Get<Style>("SmallFabStyle");
				public static Action<IDependencyPropertyBuilder<Style>> SurfaceSmallFab => StaticResource.Get<Style>("SurfaceSmallFabStyle");
				public static Action<IDependencyPropertyBuilder<Style>> SecondarySmallFab => StaticResource.Get<Style>("SecondarySmallFabStyle");
				public static Action<IDependencyPropertyBuilder<Style>> TertiarySmallFab => StaticResource.Get<Style>("TertiarySmallFabStyle");
				public static Action<IDependencyPropertyBuilder<Style>> LargeFab => StaticResource.Get<Style>("LargeFabStyle");
				public static Action<IDependencyPropertyBuilder<Style>> SurfaceLargeFab => StaticResource.Get<Style>("SurfaceLargeFabStyle");
				public static Action<IDependencyPropertyBuilder<Style>> SecondaryLargeFab => StaticResource.Get<Style>("SecondaryLargeFabStyle");
				public static Action<IDependencyPropertyBuilder<Style>> TertiaryLargeFab => StaticResource.Get<Style>("TertiaryLargeFabStyle");
			}

			public static class CalendarDatePicker
			{
				public static Action<IDependencyPropertyBuilder<Style>> Default => StaticResource.Get<Style>("CalendarDatePickerStyle");
			}

			public static class CalendarView
			{
				public static Action<IDependencyPropertyBuilder<Style>> Default => StaticResource.Get<Style>("CalendarViewStyle");
			}

			public static class CheckBox
			{
				public static Action<IDependencyPropertyBuilder<Style>> Default => StaticResource.Get<Style>("CheckBoxStyle");
				public static Action<IDependencyPropertyBuilder<Style>> Secondary => StaticResource.Get<Style>("SecondaryCheckBoxStyle");
			}

			public static class ComboBox
			{
				public static Action<IDependencyPropertyBuilder<Style>> Default => StaticResource.Get<Style>("ComboBoxStyle");
			}

			public static class AppBarButton
			{
				public static Action<IDependencyPropertyBuilder<Style>> Default => StaticResource.Get<Style>("AppBarButtonStyle");
			}

			public static class CommandBar
			{
				public static Action<IDependencyPropertyBuilder<Style>> Default => StaticResource.Get<Style>("CommandBarStyle");
			}

			public static class ContentDialog
			{
				public static Action<IDependencyPropertyBuilder<Style>> Default => StaticResource.Get<Style>("ContentDialogStyle");
			}

			public static class DatePicker
			{
				public static Action<IDependencyPropertyBuilder<Style>> Default => StaticResource.Get<Style>("DatePickerStyle");
			}

			public static class FlyoutPresenter
			{
				public static Action<IDependencyPropertyBuilder<Style>> Default => StaticResource.Get<Style>("FlyoutPresenterStyle");
			}

			public static class MenuFlyoutPresenter
			{
				public static Action<IDependencyPropertyBuilder<Style>> Default => StaticResource.Get<Style>("MenuFlyoutPresenterStyle");
			}

			public static class HyperlinkButton
			{
				public static Action<IDependencyPropertyBuilder<Style>> Default => StaticResource.Get<Style>("HyperlinkButtonStyle");
				public static Action<IDependencyPropertyBuilder<Style>> Secondary => StaticResource.Get<Style>("SecondaryHyperlinkButtonStyle");
			}

			public static class ListViewItem
			{
				public static Action<IDependencyPropertyBuilder<Style>> Default => StaticResource.Get<Style>("ListViewItemStyle");
			}

			public static class ListView
			{
				public static Action<IDependencyPropertyBuilder<Style>> Default => StaticResource.Get<Style>("ListViewStyle");
			}

			public static class NavigationView
			{
				public static Action<IDependencyPropertyBuilder<Style>> Default => StaticResource.Get<Style>("NavigationViewStyle");
			}

			public static class NavigationViewItem
			{
				public static Action<IDependencyPropertyBuilder<Style>> Default => StaticResource.Get<Style>("NavigationViewItemStyle");
			}

			public static class PasswordBox
			{
				public static Action<IDependencyPropertyBuilder<Style>> Filled => StaticResource.Get<Style>("FilledPasswordBoxStyle");
				public static Action<IDependencyPropertyBuilder<Style>> Outlined => StaticResource.Get<Style>("OutlinedPasswordBoxStyle");
			}

			public static class ProgressBar
			{
				public static Action<IDependencyPropertyBuilder<Style>> Default => StaticResource.Get<Style>("ProgressBarStyle");
			}

			public static class ProgressRing
			{
				public static Action<IDependencyPropertyBuilder<Style>> Default => StaticResource.Get<Style>("ProgressRingStyle");
			}

			public static class RadioButton
			{
				public static Action<IDependencyPropertyBuilder<Style>> Default => StaticResource.Get<Style>("RadioButtonStyle");
				public static Action<IDependencyPropertyBuilder<Style>> Secondary => StaticResource.Get<Style>("SecondaryRadioButtonStyle");
			}

			public static class Slider
			{
				public static Action<IDependencyPropertyBuilder<Style>> Default => StaticResource.Get<Style>("SliderStyle");
			}

			public static class TextBlock
			{
				public static Action<IDependencyPropertyBuilder<Style>> DisplayLarge => StaticResource.Get<Style>("DisplayLarge");
				public static Action<IDependencyPropertyBuilder<Style>> DisplayMedium => StaticResource.Get<Style>("DisplayMedium");
				public static Action<IDependencyPropertyBuilder<Style>> DisplaySmall => StaticResource.Get<Style>("DisplaySmall");
				public static Action<IDependencyPropertyBuilder<Style>> HeadlineLarge => StaticResource.Get<Style>("HeadlineLarge");
				public static Action<IDependencyPropertyBuilder<Style>> HeadlineMedium => StaticResource.Get<Style>("HeadlineMedium");
				public static Action<IDependencyPropertyBuilder<Style>> HeadlineSmall => StaticResource.Get<Style>("HeadlineSmall");
				public static Action<IDependencyPropertyBuilder<Style>> TitleLarge => StaticResource.Get<Style>("TitleLarge");
				public static Action<IDependencyPropertyBuilder<Style>> TitleMedium => StaticResource.Get<Style>("TitleMedium");
				public static Action<IDependencyPropertyBuilder<Style>> TitleSmall => StaticResource.Get<Style>("TitleSmall");
				public static Action<IDependencyPropertyBuilder<Style>> LabelLarge => StaticResource.Get<Style>("LabelLarge");
				public static Action<IDependencyPropertyBuilder<Style>> LabelMedium => StaticResource.Get<Style>("LabelMedium");
				public static Action<IDependencyPropertyBuilder<Style>> LabelSmall => StaticResource.Get<Style>("LabelSmall");
				public static Action<IDependencyPropertyBuilder<Style>> LabelExtraSmall => StaticResource.Get<Style>("LabelExtraSmall");
				public static Action<IDependencyPropertyBuilder<Style>> BodyLarge => StaticResource.Get<Style>("BodyLarge");
				public static Action<IDependencyPropertyBuilder<Style>> BodyMedium => StaticResource.Get<Style>("BodyMedium");
				public static Action<IDependencyPropertyBuilder<Style>> BodySmall => StaticResource.Get<Style>("BodySmall");
				public static Action<IDependencyPropertyBuilder<Style>> CaptionLarge => StaticResource.Get<Style>("CaptionLarge");
				public static Action<IDependencyPropertyBuilder<Style>> CaptionMedium => StaticResource.Get<Style>("CaptionMedium");
				public static Action<IDependencyPropertyBuilder<Style>> CaptionSmall => StaticResource.Get<Style>("CaptionSmall");
			}

			public static class TextBox
			{
				public static Action<IDependencyPropertyBuilder<Style>> Filled => StaticResource.Get<Style>("FilledTextBoxStyle");
				public static Action<IDependencyPropertyBuilder<Style>> Outlined => StaticResource.Get<Style>("OutlinedTextBoxStyle");
			}

			public static class ToggleButton
			{
				public static Action<IDependencyPropertyBuilder<Style>> Text => StaticResource.Get<Style>("TextToggleButtonStyle");
				public static Action<IDependencyPropertyBuilder<Style>> Icon => StaticResource.Get<Style>("IconToggleButtonStyle");
			}

			public static class ToggleSwitch
			{
				public static Action<IDependencyPropertyBuilder<Style>> Default => StaticResource.Get<Style>("ToggleSwitchStyle");
			}
		}
	}
}
