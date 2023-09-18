using Microsoft.UI.Text;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Media;
using Uno.Extensions.Markup;
using Uno.Extensions.Markup.Internals;

namespace Uno.Themes.Markup
{
	public static partial class Theme
	{
		public static class RatingControl
		{
			public static class Resources
			{
				public static class Default
				{
					public static class Foreground
					{
						[ResourceKeyDefinition(typeof(Brush), "RatingControlForeground")]
						public static ThemeResourceKey<Brush> Default => new("RatingControlForeground");

						[ResourceKeyDefinition(typeof(Brush), "RatingControlForegroundUnselected")]
						public static ThemeResourceKey<Brush> Unselected => new("RatingControlForegroundUnselected");

						[ResourceKeyDefinition(typeof(Brush), "RatingControlForegroundUnselectedPointerOver")]
						public static ThemeResourceKey<Brush> UnselectedPointerOver => new("RatingControlForegroundUnselectedPointerOver");

						[ResourceKeyDefinition(typeof(Brush), "RatingControlForegroundSelected")]
						public static ThemeResourceKey<Brush> Selected => new("RatingControlForegroundSelected");

						[ResourceKeyDefinition(typeof(Brush), "RatingControlForegroundSelectedPointerOver")]
						public static ThemeResourceKey<Brush> SelectedPointerOver => new("RatingControlForegroundSelectedPointerOver");

						[ResourceKeyDefinition(typeof(Brush), "RatingControlForegroundSelectedDisabled")]
						public static ThemeResourceKey<Brush> SelectedDisabled => new("RatingControlForegroundSelectedDisabled");
					}

					public static class PlaceholderForeground
					{
						[ResourceKeyDefinition(typeof(Brush), "RatingControlPlaceholderForeground")]
						public static ThemeResourceKey<Brush> Default => new("RatingControlPlaceholderForeground");

						[ResourceKeyDefinition(typeof(Brush), "RatingControlPlaceholderForegroundPointerOver")]
						public static ThemeResourceKey<Brush> PointerOver => new("RatingControlPlaceholderForegroundPointerOver");
					}

					public static class CaptionForeground
					{
						[ResourceKeyDefinition(typeof(Brush), "RatingControlCaptionForeground")]
						public static ThemeResourceKey<Brush> Default => new("RatingControlCaptionForeground");
					}

					public static class Typography
					{
						[ResourceKeyDefinition(typeof(FontFamily), "RatingControlFontFamily")]
						public static ThemeResourceKey<FontFamily> FontFamily => new("RatingControlFontFamily");

						[ResourceKeyDefinition(typeof(FontFamily), "RatingControlCaptionFontFamily")]
						public static ThemeResourceKey<FontFamily> CaptionFontFamily => new("RatingControlCaptionFontFamily");

						[ResourceKeyDefinition(typeof(Style), "RatingControlCaptionStyle")]
						public static ThemeResourceKey<Style> CaptionStyle => new("RatingControlCaptionStyle");
					}

					[ResourceKeyDefinition(typeof(double), "RatingControlHeight")]
					public static ThemeResourceKey<double> Height => new("RatingControlHeight");

					[ResourceKeyDefinition(typeof(double), "RatingControlCaptionHeight")]
					public static ThemeResourceKey<double> CaptionHeight => new("RatingControlCaptionHeight");
				}

				public static class Secondary
				{
					public static class Foreground
					{
						[ResourceKeyDefinition(typeof(Brush), "SecondaryRatingControlForeground")]
						public static ThemeResourceKey<Brush> Default => new("SecondaryRatingControlForeground");

						[ResourceKeyDefinition(typeof(Brush), "SecondaryRatingControlForegroundUnselected")]
						public static ThemeResourceKey<Brush> Unselected => new("SecondaryRatingControlForegroundUnselected");

						[ResourceKeyDefinition(typeof(Brush), "SecondaryRatingControlForegroundUnselectedPointerOver")]
						public static ThemeResourceKey<Brush> UnselectedPointerOver => new("SecondaryRatingControlForegroundUnselectedPointerOver");

						[ResourceKeyDefinition(typeof(Brush), "SecondaryRatingControlForegroundSelected")]
						public static ThemeResourceKey<Brush> Selected => new("SecondaryRatingControlForegroundSelected");

						[ResourceKeyDefinition(typeof(Brush), "SecondaryRatingControlForegroundSelectedPointerOver")]
						public static ThemeResourceKey<Brush> SelectedPointerOver => new("SecondaryRatingControlForegroundSelectedPointerOver");

						[ResourceKeyDefinition(typeof(Brush), "SecondaryRatingControlForegroundSelectedDisabled")]
						public static ThemeResourceKey<Brush> SelectedDisabled => new("SecondaryRatingControlForegroundSelectedDisabled");
					}

					public static class PlaceholderForeground
					{
						[ResourceKeyDefinition(typeof(Brush), "SecondaryRatingControlPlaceholderForeground")]
						public static ThemeResourceKey<Brush> Default => new("SecondaryRatingControlPlaceholderForeground");

						[ResourceKeyDefinition(typeof(Brush), "SecondaryRatingControlPlaceholderForegroundPointerOver")]
						public static ThemeResourceKey<Brush> PointerOver => new("SecondaryRatingControlPlaceholderForegroundPointerOver");
					}

					public static class CaptionForeground
					{
						[ResourceKeyDefinition(typeof(Brush), "SecondaryRatingControlCaptionForeground")]
						public static ThemeResourceKey<Brush> Default => new("SecondaryRatingControlCaptionForeground");
					}

					public static class Typography
					{
						[ResourceKeyDefinition(typeof(FontFamily), "SecondaryRatingControlCaptionFontFamily")]
						public static ThemeResourceKey<FontFamily> CaptionFontFamily => new("SecondaryRatingControlCaptionFontFamily");

						[ResourceKeyDefinition(typeof(Style), "SecondaryRatingControlCaptionStyle")]
						public static ThemeResourceKey<Style> CaptionStyle => new("SecondaryRatingControlCaptionStyle");
					}

					[ResourceKeyDefinition(typeof(double), "SecondaryRatingControlCaptionHeight")]
					public static ThemeResourceKey<double> CaptionHeight => new("SecondaryRatingControlCaptionHeight");
				}
			}

			public static class Styles
			{
				[ResourceKeyDefinition(typeof(Style), "RatingControlStyle", TargetType = typeof(RatingControl))]
				public static StaticResourceKey<Style> Default => new("RatingControlStyle");

				[ResourceKeyDefinition(typeof(Style), "SecondaryRatingControlStyle", TargetType = typeof(RatingControl))]
				public static StaticResourceKey<Style> Secondary => new("SecondaryRatingControlStyle");
			}
		}
	}
}
