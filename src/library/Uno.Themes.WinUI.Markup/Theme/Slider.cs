using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Media;
using Uno.Extensions.Markup;
using Uno.Extensions.Markup.Internals;

namespace Uno.Themes.Markup
{
	public static partial class Theme
	{
		public static class Slider
		{
			public static class Resources
			{
				public static class Typography
				{
					[ResourceKeyDefinition(typeof(FontFamily), "SliderFontFamily")]
					public static ThemeResourceKey<FontFamily> FontFamily => new("SliderFontFamily");

					[ResourceKeyDefinition(typeof(double), "SliderFontSize")]
					public static ThemeResourceKey<double> FontSize => new("SliderFontSize");
				}

				public static class Track
				{
					public static class Fill
					{
						[ResourceKeyDefinition(typeof(Brush), "SliderTrackFill")]
						public static ThemeResourceKey<Brush> Default => new("SliderTrackFill");

						[ResourceKeyDefinition(typeof(Brush), "SliderTrackFillDisabled")]
						public static ThemeResourceKey<Brush> Disabled => new("SliderTrackFillDisabled");
					}

					public static class ValueFill
					{
						[ResourceKeyDefinition(typeof(Brush), "SliderTrackValueFill")]
						public static ThemeResourceKey<Brush> Default => new("SliderTrackFill");

						[ResourceKeyDefinition(typeof(Brush), "SliderTrackValueFillDisabled")]
						public static ThemeResourceKey<Brush> Disabled => new("SliderTrackFillDisabled");
					}

					[ResourceKeyDefinition(typeof(CornerRadius), "SliderTrackCornerRadius")]
					public static ThemeResourceKey<CornerRadius> CornerRadius => new("SliderTrackCornerRadius");
				}

				public static class TickBar
				{
					public static class Fill
					{
						[ResourceKeyDefinition(typeof(Brush), "SliderTickBarFill")]
						public static ThemeResourceKey<Brush> Default => new("SliderTickBarFill");

						[ResourceKeyDefinition(typeof(Brush), "SliderTickBarFillDisabled")]
						public static ThemeResourceKey<Brush> Disabled => new("SliderTickBarFillDisabled");
					}

					[ResourceKeyDefinition(typeof(Brush), "SliderInlineTickBarFill")]
					public static ThemeResourceKey<Brush> InlineFill => new("SliderInlineTickBarFill");
				}

				public static class Thumb
				{
					public static class Background
					{
						[ResourceKeyDefinition(typeof(Brush), "SliderThumbBackground")]
						public static ThemeResourceKey<Brush> Default => new("SliderThumbBackground");

						[ResourceKeyDefinition(typeof(Brush), "SliderThumbBackgroundDisabled")]
						public static ThemeResourceKey<Brush> Disabled => new("SliderThumbBackgroundDisabled");
					}
				}
			}

			public static class Styles
			{
				[ResourceKeyDefinition(typeof(Style), "SliderStyle", TargetType = typeof(Slider))]
				public static StaticResourceKey<Style> Default => new("SliderStyle");
			}
		}
	}
}
