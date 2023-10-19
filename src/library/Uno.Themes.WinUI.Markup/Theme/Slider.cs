using System;
using Windows.UI;
using Microsoft.UI.Text;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Media;
using Uno.Extensions.Markup;
using Uno.Extensions.Markup.Internals;

namespace Uno.Themes.Markup;

public static partial class Theme
{
	public static partial class Slider
	{
		public static partial class Resources
		{
			public static partial class Thumb
			{
				[ResourceKeyDefinition(typeof(Brush), "SliderThumbBackgroundDisabled")]
				public static ThemeResourceKey<Brush> BackgroundDisabled => new("SliderThumbBackgroundDisabled");

				[ResourceKeyDefinition(typeof(double), "SliderThumbHeight")]
				public static ThemeResourceKey<double> Height => new("SliderThumbHeight");

				[ResourceKeyDefinition(typeof(double), "SliderThumbWidth")]
				public static ThemeResourceKey<double> Width => new("SliderThumbWidth");
			}

			public static partial class TickBar
			{
				public static partial class Bottom
				{
					[ResourceKeyDefinition(typeof(double), "SliderBottomTickBarHeight")]
					public static ThemeResourceKey<double> Height => new("SliderBottomTickBarHeight");

					[ResourceKeyDefinition(typeof(Thickness), "SliderBottomTickBarMargin")]
					public static ThemeResourceKey<Thickness> Margin => new("SliderBottomTickBarMargin");
				}

				public static partial class Fill
				{
					[ResourceKeyDefinition(typeof(Brush), "SliderTickBarFill")]
					public static ThemeResourceKey<Brush> Default => new("SliderTickBarFill");

					[ResourceKeyDefinition(typeof(Brush), "SliderTickBarFillDisabled")]
					public static ThemeResourceKey<Brush> Disabled => new("SliderTickBarFillDisabled");
				}

				public static partial class Left
				{
					[ResourceKeyDefinition(typeof(Thickness), "SliderLeftTickBarMargin")]
					public static ThemeResourceKey<Thickness> Margin => new("SliderLeftTickBarMargin");

					[ResourceKeyDefinition(typeof(double), "SliderLeftTickBarWidth")]
					public static ThemeResourceKey<double> Width => new("SliderLeftTickBarWidth");
				}

				public static partial class Right
				{
					[ResourceKeyDefinition(typeof(Thickness), "SliderRightTickBarMargin")]
					public static ThemeResourceKey<Thickness> Margin => new("SliderRightTickBarMargin");

					[ResourceKeyDefinition(typeof(double), "SliderRightTickBarWidth")]
					public static ThemeResourceKey<double> Width => new("SliderRightTickBarWidth");
				}

				public static partial class Top
				{
					[ResourceKeyDefinition(typeof(double), "SliderTopTickBarHeight")]
					public static ThemeResourceKey<double> Height => new("SliderTopTickBarHeight");

					[ResourceKeyDefinition(typeof(Thickness), "SliderTopTickBarMargin")]
					public static ThemeResourceKey<Thickness> Margin => new("SliderTopTickBarMargin");
				}

				[ResourceKeyDefinition(typeof(double), "SliderHorizontalInlineTickBarHeight")]
				public static ThemeResourceKey<double> HorizontalInlineHeight => new("SliderHorizontalInlineTickBarHeight");

				[ResourceKeyDefinition(typeof(Brush), "SliderInlineTickBarFill")]
				public static ThemeResourceKey<Brush> InlineFill => new("SliderInlineTickBarFill");

				[ResourceKeyDefinition(typeof(double), "SliderVerticallInlineTickBarWidth")]
				public static ThemeResourceKey<double> VerticallInlineWidth => new("SliderVerticallInlineTickBarWidth");
			}

			public static partial class Track
			{
				public static partial class Fill
				{
					[ResourceKeyDefinition(typeof(Brush), "SliderTrackFill")]
					public static ThemeResourceKey<Brush> Default => new("SliderTrackFill");

					[ResourceKeyDefinition(typeof(Brush), "SliderTrackFillDisabled")]
					public static ThemeResourceKey<Brush> Disabled => new("SliderTrackFillDisabled");
				}

				public static partial class ValueFill
				{
					[ResourceKeyDefinition(typeof(Brush), "SliderTrackValueFill")]
					public static ThemeResourceKey<Brush> Default => new("SliderTrackValueFill");

					[ResourceKeyDefinition(typeof(Brush), "SliderTrackValueFillDisabled")]
					public static ThemeResourceKey<Brush> Disabled => new("SliderTrackValueFillDisabled");
				}

				[ResourceKeyDefinition(typeof(CornerRadius), "SliderTrackCornerRadius")]
				public static ThemeResourceKey<CornerRadius> CornerRadius => new("SliderTrackCornerRadius");

				[ResourceKeyDefinition(typeof(double), "SliderTrackThickness")]
				public static ThemeResourceKey<double> Thickness => new("SliderTrackThickness");
			}

			public static partial class Typography
			{
				[ResourceKeyDefinition(typeof(FontFamily), "SliderFontFamily")]
				public static ThemeResourceKey<FontFamily> FontFamily => new("SliderFontFamily");

				[ResourceKeyDefinition(typeof(double), "SliderFontSize")]
				public static ThemeResourceKey<double> FontSize => new("SliderFontSize");
			}

			[ResourceKeyDefinition(typeof(double), "SliderFillThickness")]
			public static ThemeResourceKey<double> FillThickness => new("SliderFillThickness");

			[ResourceKeyDefinition(typeof(Thickness), "SliderFocusVisualMargin")]
			public static ThemeResourceKey<Thickness> FocusVisualMargin => new("SliderFocusVisualMargin");

			[ResourceKeyDefinition(typeof(CornerRadius), "SliderHorizontalFillCornerRadius")]
			public static ThemeResourceKey<CornerRadius> HorizontalFillCornerRadius => new("SliderHorizontalFillCornerRadius");

			[ResourceKeyDefinition(typeof(Thickness), "SliderHorizontalFocusVisualMargin")]
			public static ThemeResourceKey<Thickness> HorizontalFocusVisualMargin => new("SliderHorizontalFocusVisualMargin");

			[ResourceKeyDefinition(typeof(CornerRadius), "SliderVerticalFillCornerRadius")]
			public static ThemeResourceKey<CornerRadius> VerticalFillCornerRadius => new("SliderVerticalFillCornerRadius");

			[ResourceKeyDefinition(typeof(Thickness), "SliderVerticalFocusVisualMargin")]
			public static ThemeResourceKey<Thickness> VerticalFocusVisualMargin => new("SliderVerticalFocusVisualMargin");
		}

		public static partial class Styles
		{
			[ResourceKeyDefinition(typeof(Style), "MaterialSliderStyle", TargetType = typeof(global::Microsoft.UI.Xaml.Controls.Slider))]
			public static StaticResourceKey<Style> Default => new("MaterialSliderStyle");
		}
	}
}
