using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Media;
using Uno.Extensions.Markup;
using Uno.Extensions.Markup.Internals;

namespace Uno.Themes.Markup;

public static partial class Theme
{
	public static partial class DatePicker
	{
		public static partial class Resources
		{
			public static class Default
			{
				public static partial class Button
				{
					public static partial class Background
					{
						[ResourceKeyDefinition(typeof(Brush), "DatePickerButtonBackground")]
						public static ThemeResourceKey<Brush> Default => new("DatePickerButtonBackground");

						[ResourceKeyDefinition(typeof(Brush), "DatePickerButtonBackgroundPointerOver")]
						public static ThemeResourceKey<Brush> PointerOver => new("DatePickerButtonBackgroundPointerOver");

						[ResourceKeyDefinition(typeof(Brush), "DatePickerButtonBackgroundPressed")]
						public static ThemeResourceKey<Brush> Pressed => new("DatePickerButtonBackgroundPressed");

						[ResourceKeyDefinition(typeof(Brush), "DatePickerButtonBackgroundDisabled")]
						public static ThemeResourceKey<Brush> Disabled => new("DatePickerButtonBackgroundDisabled");
					}

					public static partial class Foreground
					{
						[ResourceKeyDefinition(typeof(Brush), "DatePickerButtonForeground")]
						public static ThemeResourceKey<Brush> Default => new("DatePickerButtonForeground");

						[ResourceKeyDefinition(typeof(Brush), "DatePickerButtonForegroundPointerOver")]
						public static ThemeResourceKey<Brush> PointerOver => new("DatePickerButtonForegroundPointerOver");

						[ResourceKeyDefinition(typeof(Brush), "DatePickerButtonForegroundPressed")]
						public static ThemeResourceKey<Brush> Pressed => new("DatePickerButtonForegroundPressed");

						[ResourceKeyDefinition(typeof(Brush), "DatePickerButtonForegroundDisabled")]
						public static ThemeResourceKey<Brush> Disabled => new("DatePickerButtonForegroundDisabled");
					}

					public static partial class BorderBrush
					{
						[ResourceKeyDefinition(typeof(Brush), "DatePickerButtonBorderBrush")]
						public static ThemeResourceKey<Brush> Default => new("DatePickerButtonBorderBrush");

						[ResourceKeyDefinition(typeof(Brush), "DatePickerButtonBorderBrushPointerOver")]
						public static ThemeResourceKey<Brush> PointerOver => new("DatePickerButtonBorderBrushPointerOver");

						[ResourceKeyDefinition(typeof(Brush), "DatePickerButtonBorderBrushPressed")]
						public static ThemeResourceKey<Brush> Pressed => new("DatePickerButtonBorderBrushPressed");

						[ResourceKeyDefinition(typeof(Brush), "DatePickerButtonBorderBrushDisabled")]
						public static ThemeResourceKey<Brush> Disabled => new("DatePickerButtonBorderBrushDisabled");
					}

					public static partial class DateTextForeground
					{
						[ResourceKeyDefinition(typeof(Brush), "DatePickerButtonDateTextForeground")]
						public static ThemeResourceKey<Brush> Default => new("DatePickerButtonDateTextForeground");

						[ResourceKeyDefinition(typeof(Brush), "DatePickerButtonDateTextForegroundPointerOver")]
						public static ThemeResourceKey<Brush> PointerOver => new("DatePickerButtonDateTextForegroundPointerOver");

						[ResourceKeyDefinition(typeof(Brush), "DatePickerButtonDateTextForegroundPressed")]
						public static ThemeResourceKey<Brush> Pressed => new("DatePickerButtonDateTextForegroundPressed");

						[ResourceKeyDefinition(typeof(Brush), "DatePickerButtonDateTextForegroundDisabled")]
						public static ThemeResourceKey<Brush> Disabled => new("DatePickerButtonDateTextForegroundDisabled");
					}

					[ResourceKeyDefinition(typeof(double), "DatePickerButtonContentHeight")]
					public static ThemeResourceKey<double> ContentHeight => new("DatePickerButtonContentHeight");

					[ResourceKeyDefinition(typeof(Thickness), "DatePickerButtonContentMargin")]
					public static ThemeResourceKey<Thickness> ContentMargin => new("DatePickerButtonContentMargin");

					[ResourceKeyDefinition(typeof(double), "DatePickerButtonBottomBorderHeight")]
					public static ThemeResourceKey<double> BottomBorderHeight => new("DatePickerButtonBottomBorderHeight");

					[ResourceKeyDefinition(typeof(Thickness), "DatePickerButtonHeaderMargin")]
					public static ThemeResourceKey<Thickness> HeaderMargin => new("DatePickerButtonHeaderMargin");

					[ResourceKeyDefinition(typeof(Thickness), "DatePickerButtonPlaceholderMargin")]
					public static ThemeResourceKey<Thickness> PlaceholderMargin => new("DatePickerButtonPlaceholderMargin");
				}

				public static class Flyout
				{
					[ResourceKeyDefinition(typeof(Brush), "DatePickerFlyoutPresenterBackground")]
					public static ThemeResourceKey<Brush> Background => new("DatePickerFlyoutPresenterBackground");

					[ResourceKeyDefinition(typeof(Brush), "DatePickerFlyoutPresenterBorderBrush")]
					public static ThemeResourceKey<Brush> BorderBrush => new("DatePickerFlyoutPresenterBorderBrush");

					[ResourceKeyDefinition(typeof(Brush), "DatePickerFlyoutPresenterSpacerFill")]
					public static ThemeResourceKey<Brush> SpacerFill => new("DatePickerFlyoutPresenterSpacerFill");

					[ResourceKeyDefinition(typeof(Brush), "DatePickerFlyoutPresenterHighlightFill")]
					public static ThemeResourceKey<Brush> HighlightFill => new("DatePickerFlyoutPresenterHighlightFill");

					public static class Typography
					{
						[ResourceKeyDefinition(typeof(FontFamily), "DatePickerFlyoutPresenterFontFamily")]
						public static ThemeResourceKey<FontFamily> FontFamily => new("DatePickerFlyoutPresenterFontFamily");

						[ResourceKeyDefinition(typeof(double), "DatePickerFlyoutPresenterFontSize")]
						public static ThemeResourceKey<double> FontSize => new("DatePickerFlyoutPresenterFontSize");
					}

					[ResourceKeyDefinition(typeof(Brush), "DatePickerFlyoutButtonBackground")]
					public static ThemeResourceKey<Brush> ButtonBackground => new("DatePickerFlyoutButtonBackground");

					public static class ButtonOpacity
					{
						[ResourceKeyDefinition(typeof(double), "DatePickerFlyoutButtonOpacityPressed")]
						public static ThemeResourceKey<double> Pressed => new("DatePickerFlyoutButtonOpacityPressed");

						[ResourceKeyDefinition(typeof(double), "DatePickerFlyoutButtonOpacityDisabled")]
						public static ThemeResourceKey<double> Disabled => new("DatePickerFlyoutButtonOpacityDisabled");
					}

					[ResourceKeyDefinition(typeof(Thickness), "DatePickerFlyoutButtonPadding")]
					public static ThemeResourceKey<Thickness> ButtonPadding => new("DatePickerFlyoutButtonPadding");

					[ResourceKeyDefinition(typeof(CornerRadius), "DatePickerFlyoutPresenterCornerRadius")]
					public static ThemeResourceKey<CornerRadius> CornerRadius => new("DatePickerFlyoutPresenterCornerRadius");

					[ResourceKeyDefinition(typeof(double), "DatePickerFlyoutBorderThickness")]
					public static ThemeResourceKey<double> BorderThickness => new("DatePickerFlyoutBorderThickness");

					[ResourceKeyDefinition(typeof(double), "DatePickerFlyoutElevation")]
					public static ThemeResourceKey<double> Elevation => new("DatePickerFlyoutElevation");

					[ResourceKeyDefinition(typeof(double), "DatePickerFlyoutPresenterWidth")]
					public static ThemeResourceKey<double> Width => new("DatePickerFlyoutPresenterWidth");

					[ResourceKeyDefinition(typeof(double), "DatePickerFlyoutPresenterMinWidth")]
					public static ThemeResourceKey<double> MinWidth => new("DatePickerFlyoutPresenterMinWidth");

					[ResourceKeyDefinition(typeof(double), "DatePickerFlyoutPresenterMaxHeight")]
					public static ThemeResourceKey<double> MaxHeight => new("DatePickerFlyoutPresenterMaxHeight");

					[ResourceKeyDefinition(typeof(double), "DatePickerFlyoutPresenterAcceptDismissHostGridHeight")]
					public static ThemeResourceKey<double> AcceptDismissHostGridHeight => new("DatePickerFlyoutPresenterAcceptDismissHostGridHeight");

					[ResourceKeyDefinition(typeof(double), "DatePickerFlyoutPresenterHighlightHeight")]
					public static ThemeResourceKey<double> HighlightHeight => new("DatePickerFlyoutPresenterHighlightHeight");
				}

				[ResourceKeyDefinition(typeof(CornerRadius), "DatePickerCornerRadius")]
				public static ThemeResourceKey<CornerRadius> CornerRadius => new("DatePickerCornerRadius");

				[ResourceKeyDefinition(typeof(double), "DatePickerHeight")]
				public static ThemeResourceKey<double> Height => new("DatePickerHeight");

				[ResourceKeyDefinition(typeof(Brush), "DatePickerPlaceholderTextForeground")]
				public static ThemeResourceKey<Brush> PlaceholderTextForeground => new("DatePickerPlaceholderTextForeground");
			}
		}

		public static partial class Styles
		{
			[ResourceKeyDefinition(typeof(Style), "DatePickerStyle", TargetType = typeof(global::Microsoft.UI.Xaml.Controls.DatePicker))]
			public static StaticResourceKey<Style> Default => new("DatePickerStyle");
		}
	}
}
