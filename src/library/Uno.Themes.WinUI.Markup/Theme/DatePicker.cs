using System;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Media;
using Uno.Extensions.Markup;
using Uno.Extensions.Markup.Internals;

namespace Uno.Themes.Markup
{
	public static partial class Theme
	{
		public static class DatePicker
		{
			public static class Resources
			{
				public static class FlyoutButton
				{
					public static class Background
					{
						[ResourceKeyDefinition(typeof(Brush), "DatePickerFlyoutButtonBackground")]
						public static ThemeResourceKey<Brush> Default => new("DatePickerFlyoutButtonBackground");
					}
				}

				public static class FlyoutPresenter
				{
					public static class Background
					{
						[ResourceKeyDefinition(typeof(Brush), "DatePickerFlyoutPresenterBackground")]
						public static ThemeResourceKey<Brush> Default => new("DatePickerFlyoutPresenterBackground");
					}

					public static class BorderBrush
					{
						[ResourceKeyDefinition(typeof(Brush), "DatePickerFlyoutPresenterBorderBrush")]
						public static ThemeResourceKey<Brush> Default => new("DatePickerFlyoutPresenterBorderBrush");
					}

					public static class SpacerFill
					{
						[ResourceKeyDefinition(typeof(Brush), "DatePickerFlyoutPresenterSpacerFill")]
						public static ThemeResourceKey<Brush> Default => new("DatePickerFlyoutPresenterSpacerFill");
					}

					public static class HighlightFill
					{
						[ResourceKeyDefinition(typeof(Brush), "DatePickerFlyoutPresenterHighlightFill")]
						public static ThemeResourceKey<Brush> Default => new("DatePickerFlyoutPresenterHighlightFill");
					}

					public static class ContainerCornerRadius
					{
						[ResourceKeyDefinition(typeof(CornerRadius), "DatePickerFlyoutPresenterCornerRadius")]
						public static ThemeResourceKey<CornerRadius> Default => new("DatePickerFlyoutPresenterCornerRadius");
					}

					public static class Typography
					{
						[ResourceKeyDefinition(typeof(FontFamily), "DatePickerFlyoutPresenterFontFamily")]
						public static ThemeResourceKey<FontFamily> FontFamily => new("DatePickerFlyoutPresenterFontFamily");

						[ResourceKeyDefinition(typeof(double), "DatePickerFlyoutPresenterFontSize")]
						public static ThemeResourceKey<double> FontSize => new("DatePickerFlyoutPresenterFontSize");
					}
				}

				public static class Button
				{
					public static class Background
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

					public static class Foreground
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

					public static class BorderBrush
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

					public static class ContainerCornerRadius
					{
						[ResourceKeyDefinition(typeof(CornerRadius), "DatePickerFlyoutPresenterCornerRadius")]
						public static ThemeResourceKey<CornerRadius> Default => new("DatePickerFlyoutPresenterCornerRadius");
					}
				}

				public static class ButtonDateText
				{
					public static class Foreground
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
				}

				public static class PlaceholderText
				{
					public static class Foreground
					{
						[ResourceKeyDefinition(typeof(Brush), "DatePickerPlaceholderTextForeground")]
						public static ThemeResourceKey<Brush> Default => new("DatePickerPlaceholderTextForeground");
					}
				}
			}

			public static class Styles
			{
				[ResourceKeyDefinition(typeof(Style), "DatePickerStyle", TargetType = typeof(DatePicker))]
				public static StaticResourceKey<Style> Default => new("DatePickerStyle");
			}
		}
	}
}
