using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Media;
using Uno.Extensions.Markup;
using Uno.Extensions.Markup.Internals;

namespace Uno.Themes.Markup;

public static partial class Theme
{
	/// <summary>
	/// C# Markup accessors for the <c>TimePicker*</c> lightweight-styling keys, mirroring
	/// <see cref="DatePicker"/>.
	///
	/// <c>TimePicker</c> only reports the <c>Normal</c> / <c>Disabled</c> and <c>HasTime</c> /
	/// <c>HasNoTime</c> visual states — it has no <c>PointerOver</c> or <c>Pressed</c> state of its
	/// own — so the <c>PointerOver</c> / <c>Pressed</c> members present on <see cref="DatePicker"/>
	/// are deliberately absent here rather than exposed as keys nothing resolves.
	/// </summary>
	public static partial class TimePicker
	{
		public static partial class Resources
		{
			public static class Default
			{
				public static partial class Button
				{
					public static partial class Background
					{
						[ResourceKeyDefinition(typeof(Brush), "TimePickerButtonBackground")]
						public static ThemeResourceKey<Brush> Default => new("TimePickerButtonBackground");

						[ResourceKeyDefinition(typeof(Brush), "TimePickerButtonBackgroundDisabled")]
						public static ThemeResourceKey<Brush> Disabled => new("TimePickerButtonBackgroundDisabled");
					}

					public static partial class BorderBrush
					{
						[ResourceKeyDefinition(typeof(Brush), "TimePickerButtonBorderBrush")]
						public static ThemeResourceKey<Brush> Default => new("TimePickerButtonBorderBrush");

						[ResourceKeyDefinition(typeof(Brush), "TimePickerButtonBorderBrushDisabled")]
						public static ThemeResourceKey<Brush> Disabled => new("TimePickerButtonBorderBrushDisabled");
					}

					public static partial class TimeTextForeground
					{
						[ResourceKeyDefinition(typeof(Brush), "TimePickerButtonTimeTextForeground")]
						public static ThemeResourceKey<Brush> Default => new("TimePickerButtonTimeTextForeground");

						[ResourceKeyDefinition(typeof(Brush), "TimePickerButtonTimeTextForegroundDisabled")]
						public static ThemeResourceKey<Brush> Disabled => new("TimePickerButtonTimeTextForegroundDisabled");
					}

					[ResourceKeyDefinition(typeof(double), "TimePickerButtonContentHeight")]
					public static ThemeResourceKey<double> ContentHeight => new("TimePickerButtonContentHeight");

					[ResourceKeyDefinition(typeof(Thickness), "TimePickerButtonContentMargin")]
					public static ThemeResourceKey<Thickness> ContentMargin => new("TimePickerButtonContentMargin");

					[ResourceKeyDefinition(typeof(double), "TimePickerButtonBottomBorderHeight")]
					public static ThemeResourceKey<double> BottomBorderHeight => new("TimePickerButtonBottomBorderHeight");

					[ResourceKeyDefinition(typeof(Thickness), "TimePickerButtonPlaceholderMargin")]
					public static ThemeResourceKey<Thickness> PlaceholderMargin => new("TimePickerButtonPlaceholderMargin");
				}

				public static partial class Header
				{
					[ResourceKeyDefinition(typeof(Brush), "TimePickerHeaderForeground")]
					public static ThemeResourceKey<Brush> Foreground => new("TimePickerHeaderForeground");

					[ResourceKeyDefinition(typeof(Brush), "TimePickerHeaderForegroundDisabled")]
					public static ThemeResourceKey<Brush> ForegroundDisabled => new("TimePickerHeaderForegroundDisabled");

					/// <summary>
					/// How far the header shrinks once it floats above the value. The vertical movement
					/// is layout-driven, so there is no companion offset key.
					/// </summary>
					[ResourceKeyDefinition(typeof(double), "TimePickerHeaderFloatScale")]
					public static ThemeResourceKey<double> FloatScale => new("TimePickerHeaderFloatScale");
				}

				public static partial class ColumnDivider
				{
					[ResourceKeyDefinition(typeof(double), "TimePickerColumnDividerWidth")]
					public static ThemeResourceKey<double> Width => new("TimePickerColumnDividerWidth");

					[ResourceKeyDefinition(typeof(Thickness), "TimePickerColumnDividerMargin")]
					public static ThemeResourceKey<Thickness> Margin => new("TimePickerColumnDividerMargin");
				}

				public static partial class SpacerFill
				{
					[ResourceKeyDefinition(typeof(Brush), "TimePickerSpacerFill")]
					public static ThemeResourceKey<Brush> Default => new("TimePickerSpacerFill");

					[ResourceKeyDefinition(typeof(Brush), "TimePickerSpacerFillDisabled")]
					public static ThemeResourceKey<Brush> Disabled => new("TimePickerSpacerFillDisabled");
				}

				public static class Flyout
				{
					public static partial class Background
					{
						[ResourceKeyDefinition(typeof(Brush), "TimePickerFlyoutPresenterBackground")]
						public static ThemeResourceKey<Brush> Default => new("TimePickerFlyoutPresenterBackground");
					}

					public static partial class BorderBrush
					{
						[ResourceKeyDefinition(typeof(Brush), "TimePickerFlyoutPresenterBorderBrush")]
						public static ThemeResourceKey<Brush> Default => new("TimePickerFlyoutPresenterBorderBrush");
					}

					public static partial class SpacerFill
					{
						[ResourceKeyDefinition(typeof(Brush), "TimePickerFlyoutPresenterSpacerFill")]
						public static ThemeResourceKey<Brush> Default => new("TimePickerFlyoutPresenterSpacerFill");
					}

					public static partial class HighlightFill
					{
						[ResourceKeyDefinition(typeof(Brush), "TimePickerFlyoutPresenterHighlightFill")]
						public static ThemeResourceKey<Brush> Default => new("TimePickerFlyoutPresenterHighlightFill");
					}

					public static partial class Typography
					{
						[ResourceKeyDefinition(typeof(FontFamily), "TimePickerFlyoutPresenterFontFamily")]
						public static ThemeResourceKey<FontFamily> FontFamily => new("TimePickerFlyoutPresenterFontFamily");

						[ResourceKeyDefinition(typeof(double), "TimePickerFlyoutPresenterFontSize")]
						public static ThemeResourceKey<double> FontSize => new("TimePickerFlyoutPresenterFontSize");
					}

					public static partial class Button
					{
						public static partial class Background
						{
							[ResourceKeyDefinition(typeof(Brush), "TimePickerFlyoutButtonBackground")]
							public static ThemeResourceKey<Brush> Default => new("TimePickerFlyoutButtonBackground");
						}

						public static class Opacity
						{
							[ResourceKeyDefinition(typeof(double), "TimePickerFlyoutButtonOpacityPressed")]
							public static ThemeResourceKey<double> Pressed => new("TimePickerFlyoutButtonOpacityPressed");

							[ResourceKeyDefinition(typeof(double), "TimePickerFlyoutButtonOpacityDisabled")]
							public static ThemeResourceKey<double> Disabled => new("TimePickerFlyoutButtonOpacityDisabled");
						}

						[ResourceKeyDefinition(typeof(Thickness), "TimePickerFlyoutButtonPadding")]
						public static ThemeResourceKey<Thickness> Padding => new("TimePickerFlyoutButtonPadding");
					}

					[ResourceKeyDefinition(typeof(CornerRadius), "TimePickerFlyoutPresenterCornerRadius")]
					public static ThemeResourceKey<CornerRadius> CornerRadius => new("TimePickerFlyoutPresenterCornerRadius");

					[ResourceKeyDefinition(typeof(double), "TimePickerFlyoutBorderThickness")]
					public static ThemeResourceKey<double> BorderThickness => new("TimePickerFlyoutBorderThickness");

					[ResourceKeyDefinition(typeof(double), "TimePickerFlyoutElevation")]
					public static ThemeResourceKey<double> Elevation => new("TimePickerFlyoutElevation");

					[ResourceKeyDefinition(typeof(double), "TimePickerFlyoutPresenterWidth")]
					public static ThemeResourceKey<double> Width => new("TimePickerFlyoutPresenterWidth");

					[ResourceKeyDefinition(typeof(double), "TimePickerFlyoutPresenterMinWidth")]
					public static ThemeResourceKey<double> MinWidth => new("TimePickerFlyoutPresenterMinWidth");

					[ResourceKeyDefinition(typeof(double), "TimePickerFlyoutPresenterMaxWidth")]
					public static ThemeResourceKey<double> MaxWidth => new("TimePickerFlyoutPresenterMaxWidth");

					[ResourceKeyDefinition(typeof(double), "TimePickerFlyoutPresenterMaxHeight")]
					public static ThemeResourceKey<double> MaxHeight => new("TimePickerFlyoutPresenterMaxHeight");

					[ResourceKeyDefinition(typeof(double), "TimePickerFlyoutPresenterAcceptDismissHostGridHeight")]
					public static ThemeResourceKey<double> AcceptDismissHostGridHeight => new("TimePickerFlyoutPresenterAcceptDismissHostGridHeight");

					[ResourceKeyDefinition(typeof(double), "TimePickerFlyoutPresenterHighlightHeight")]
					public static ThemeResourceKey<double> HighlightHeight => new("TimePickerFlyoutPresenterHighlightHeight");
				}

				[ResourceKeyDefinition(typeof(CornerRadius), "TimePickerCornerRadius")]
				public static ThemeResourceKey<CornerRadius> CornerRadius => new("TimePickerCornerRadius");

				[ResourceKeyDefinition(typeof(double), "TimePickerHeight")]
				public static ThemeResourceKey<double> Height => new("TimePickerHeight");

				public static partial class PlaceholderTextForeground
				{
					[ResourceKeyDefinition(typeof(Brush), "TimePickerPlaceholderTextForeground")]
					public static ThemeResourceKey<Brush> Default => new("TimePickerPlaceholderTextForeground");
				}
			}
		}

		public static partial class Styles
		{
			[ResourceKeyDefinition(typeof(Style), "TimePickerStyle", TargetType = typeof(global::Microsoft.UI.Xaml.Controls.TimePicker))]
			public static StaticResourceKey<Style> Default => new("TimePickerStyle");

			[ResourceKeyDefinition(typeof(Style), "TimePickerFlyoutPresenterStyle", TargetType = typeof(global::Microsoft.UI.Xaml.Controls.TimePickerFlyoutPresenter))]
			public static StaticResourceKey<Style> FlyoutPresenter => new("TimePickerFlyoutPresenterStyle");
		}
	}
}
