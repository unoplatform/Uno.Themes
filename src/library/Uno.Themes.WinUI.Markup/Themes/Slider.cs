using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Media;
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
					public static ResourceValue<FontFamily> FontFamily => new("SliderFontFamily", true);

					[ResourceKeyDefinition(typeof(double), "SliderFontSize")]
					public static ResourceValue<double> FontSize => new("SliderFontSize", true);
				}

				public static class Track
				{
					public static class Fill
					{
						[ResourceKeyDefinition(typeof(Brush), "SliderTrackFill")]
						public static ResourceValue<Brush> Default => new("SliderTrackFill", true);

						[ResourceKeyDefinition(typeof(Brush), "SliderTrackFillDisabled")]
						public static ResourceValue<Brush> Disabled => new("SliderTrackFillDisabled", true);
					}

					public static class ValueFill
					{
						[ResourceKeyDefinition(typeof(Brush), "SliderTrackFill")]
						public static ResourceValue<Brush> Default => new("SliderTrackFill", true);

						[ResourceKeyDefinition(typeof(Brush), "SliderTrackFillDisabled")]
						public static ResourceValue<Brush> Disabled => new("SliderTrackFillDisabled", true);
					}

					[ResourceKeyDefinition(typeof(CornerRadius), "SliderTrackCornerRadius")]
					public static ResourceValue<CornerRadius> CornerRadius => new("SliderTrackCornerRadius", true);
				}

				public static class TickBar
				{
					public static class Fill
					{
						[ResourceKeyDefinition(typeof(Brush), "SliderTickBarFill")]
						public static ResourceValue<Brush> Default => new("SliderTickBarFill", true);

						[ResourceKeyDefinition(typeof(Brush), "SliderTickBarFillDisabled")]
						public static ResourceValue<Brush> Disabled => new("SliderTickBarFillDisabled", true);
					}

					[ResourceKeyDefinition(typeof(Brush), "SliderInlineTickBarFill")]
					public static ResourceValue<Brush> InlineFill => new("SliderInlineTickBarFill", true);
				}

				public static class Thumb
				{
					public static class Background
					{
						[ResourceKeyDefinition(typeof(Brush), "SliderThumbBackground")]
						public static ResourceValue<Brush> Default => new("SliderThumbBackground", true);

						[ResourceKeyDefinition(typeof(Brush), "SliderThumbBackgroundDisabled")]
						public static ResourceValue<Brush> Disabled => new("SliderThumbBackgroundDisabled", true);
					}
				}
			}

			public static class Styles
			{
				[ResourceKeyDefinition(typeof(Style), "MaterialSliderStyle", TargetType = typeof(Slider))]
				public static ResourceValue<Style> Default => new("MaterialSliderStyle");

				public static class Thumb
				{
					[ResourceKeyDefinition(typeof(Style), "MaterialSliderThumbStyle", TargetType = typeof(Microsoft.UI.Xaml.Controls.Primitives.Thumb))]
					public static ResourceValue<Style> Default => new("MaterialSliderThumbStyle");
				}
			}
		}
	}
}
