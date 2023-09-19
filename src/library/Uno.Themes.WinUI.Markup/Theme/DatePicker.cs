using System;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Media;
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
						public static ResourceValue<Brush> Default => new("DatePickerFlyoutButtonBackground", true);
					}
				}

				public static class FlyoutPresenter
				{
					public static class Background
					{
						[ResourceKeyDefinition(typeof(Brush), "DatePickerFlyoutPresenterBackground")]
						public static ResourceValue<Brush> Default => new("DatePickerFlyoutPresenterBackground", true);
					}

					public static class BorderBrush
					{
						[ResourceKeyDefinition(typeof(Brush), "DatePickerFlyoutPresenterBorderBrush")]
						public static ResourceValue<Brush> Default => new("DatePickerFlyoutPresenterBorderBrush", true);
					}

					public static class SpacerFill
					{
						[ResourceKeyDefinition(typeof(Brush), "DatePickerFlyoutPresenterSpacerFill")]
						public static ResourceValue<Brush> Default => new("DatePickerFlyoutPresenterSpacerFill", true);
					}

					public static class HighlightFill
					{
						[ResourceKeyDefinition(typeof(Brush), "DatePickerFlyoutPresenterHighlightFill")]
						public static ResourceValue<Brush> Default => new("DatePickerFlyoutPresenterHighlightFill", true);
					}

					public static class ContainerCornerRadius
					{
						[ResourceKeyDefinition(typeof(CornerRadius), "DatePickerFlyoutPresenterCornerRadius")]
						public static ResourceValue<CornerRadius> Default => new("DatePickerFlyoutPresenterCornerRadius", true);
					}

					public static class Typography
					{
						[ResourceKeyDefinition(typeof(FontFamily), "DatePickerFlyoutPresenterFontFamily")]
						public static ResourceValue<FontFamily> FontFamily => new("DatePickerFlyoutPresenterFontFamily", true);

						[ResourceKeyDefinition(typeof(double), "DatePickerFlyoutPresenterFontSize")]
						public static ResourceValue<double> FontSize => new("DatePickerFlyoutPresenterFontSize", true);
					}
				}

				public static class Button
				{
					public static class Background
					{
						[ResourceKeyDefinition(typeof(Brush), "DatePickerButtonBackground")]
						public static ResourceValue<Brush> Default => new("DatePickerButtonBackground", true);

						[ResourceKeyDefinition(typeof(Brush), "DatePickerButtonBackgroundPointerOver")]
						public static ResourceValue<Brush> PointerOver => new("DatePickerButtonBackgroundPointerOver", true);

						[ResourceKeyDefinition(typeof(Brush), "DatePickerButtonBackgroundPressed")]
						public static ResourceValue<Brush> Pressed => new("DatePickerButtonBackgroundPressed", true);

						[ResourceKeyDefinition(typeof(Brush), "DatePickerButtonBackgroundDisabled")]
						public static ResourceValue<Brush> Disabled => new("DatePickerButtonBackgroundDisabled", true);
					}

					public static class Foreground
					{
						[ResourceKeyDefinition(typeof(Brush), "DatePickerButtonForeground")]
						public static ResourceValue<Brush> Default => new("DatePickerButtonForeground", true);

						[ResourceKeyDefinition(typeof(Brush), "DatePickerButtonForegroundPointerOver")]
						public static ResourceValue<Brush> PointerOver => new("DatePickerButtonForegroundPointerOver", true);

						[ResourceKeyDefinition(typeof(Brush), "DatePickerButtonForegroundPressed")]
						public static ResourceValue<Brush> Pressed => new("DatePickerButtonForegroundPressed", true);

						[ResourceKeyDefinition(typeof(Brush), "DatePickerButtonForegroundDisabled")]
						public static ResourceValue<Brush> Disabled => new("DatePickerButtonForegroundDisabled", true);
					}

					public static class BorderBrush
					{
						[ResourceKeyDefinition(typeof(Brush), "DatePickerButtonBorderBrush")]
						public static ResourceValue<Brush> Default => new("DatePickerButtonBorderBrush", true);

						[ResourceKeyDefinition(typeof(Brush), "DatePickerButtonBorderBrushPointerOver")]
						public static ResourceValue<Brush> PointerOver => new("DatePickerButtonBorderBrushPointerOver", true);

						[ResourceKeyDefinition(typeof(Brush), "DatePickerButtonBorderBrushPressed")]
						public static ResourceValue<Brush> Pressed => new("DatePickerButtonBorderBrushPressed", true);

						[ResourceKeyDefinition(typeof(Brush), "DatePickerButtonBorderBrushDisabled")]
						public static ResourceValue<Brush> Disabled => new("DatePickerButtonBorderBrushDisabled", true);
					}

					public static class ContainerCornerRadius
					{
						[ResourceKeyDefinition(typeof(CornerRadius), "DatePickerFlyoutPresenterCornerRadius")]
						public static ResourceValue<CornerRadius> Default => new("DatePickerFlyoutPresenterCornerRadius", true);
					}
				}

				public static class ButtonDateText
				{
					public static class Foreground
					{
						[ResourceKeyDefinition(typeof(Brush), "DatePickerButtonDateTextForeground")]
						public static ResourceValue<Brush> Default => new("DatePickerButtonDateTextForeground", true);

						[ResourceKeyDefinition(typeof(Brush), "DatePickerButtonDateTextForegroundPointerOver")]
						public static ResourceValue<Brush> PointerOver => new("DatePickerButtonDateTextForegroundPointerOver", true);

						[ResourceKeyDefinition(typeof(Brush), "DatePickerButtonDateTextForegroundPressed")]
						public static ResourceValue<Brush> Pressed => new("DatePickerButtonDateTextForegroundPressed", true);

						[ResourceKeyDefinition(typeof(Brush), "DatePickerButtonDateTextForegroundDisabled")]
						public static ResourceValue<Brush> Disabled => new("DatePickerButtonDateTextForegroundDisabled", true);
					}
				}

				public static class PlaceholderText
				{
					public static class Foreground
					{
						[ResourceKeyDefinition(typeof(Brush), "DatePickerPlaceholderTextForeground")]
						public static ResourceValue<Brush> Default => new("DatePickerPlaceholderTextForeground", true);
					}
				}
			}

			public static class Styles
			{
				[ResourceKeyDefinition(typeof(Style), "DatePickerStyle", TargetType = typeof(DatePicker))]
				public static ResourceValue<Style> Default => new("DatePickerStyle");
			}
		}
	}
}
