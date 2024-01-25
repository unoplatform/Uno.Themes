using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Media;
using Uno.Extensions.Markup;
using Uno.Extensions.Markup.Internals;

namespace Uno.Themes.Markup;

public static partial class Theme
{
	public static partial class RatingControl
	{
		public static partial class Resources
		{
			public static partial class Default
			{
				public static partial class Foreground
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

				public static partial class PlaceholderForeground
				{
					[ResourceKeyDefinition(typeof(Brush), "RatingControlPlaceholderForeground")]
					public static ThemeResourceKey<Brush> Default => new("RatingControlPlaceholderForeground");

					[ResourceKeyDefinition(typeof(Brush), "RatingControlPlaceholderForegroundPointerOver")]
					public static ThemeResourceKey<Brush> PointerOver => new("RatingControlPlaceholderForegroundPointerOver");
				}

				public static partial class Caption
				{
					[ResourceKeyDefinition(typeof(FontFamily), "RatingControlCaptionFontFamily")]
					public static ThemeResourceKey<FontFamily> FontFamily => new("RatingControlCaptionFontFamily");

					public static partial class Foreground
					{
						[ResourceKeyDefinition(typeof(Brush), "RatingControlCaptionForeground")]
						public static ThemeResourceKey<Brush> Default => new("RatingControlCaptionForeground");
					}

					[ResourceKeyDefinition(typeof(double), "RatingControlCaptionHeight")]
					public static ThemeResourceKey<double> Height => new("RatingControlCaptionHeight");

					[ResourceKeyDefinition(typeof(Style), "RatingControlCaptionStyle")]
					public static ThemeResourceKey<Style> Style => new("RatingControlCaptionStyle");
				}

				public static partial class Typography
				{
					public static ThemeResourceKey<FontFamily> FontFamily => Shared.FontFamily;
				}

				[ResourceKeyDefinition(typeof(double), "RatingControlHeight")]
				public static ThemeResourceKey<double> Height => new("RatingControlHeight");
			}

			public static partial class Secondary
			{
				public static partial class Foreground
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

				public static partial class PlaceholderForeground
				{
					[ResourceKeyDefinition(typeof(Brush), "SecondaryRatingControlPlaceholderForeground")]
					public static ThemeResourceKey<Brush> Default => new("SecondaryRatingControlPlaceholderForeground");

					[ResourceKeyDefinition(typeof(Brush), "SecondaryRatingControlPlaceholderForegroundPointerOver")]
					public static ThemeResourceKey<Brush> PointerOver => new("SecondaryRatingControlPlaceholderForegroundPointerOver");
				}

				public static partial class Caption
				{
					[ResourceKeyDefinition(typeof(FontFamily), "SecondaryRatingControlCaptionFontFamily")]
					public static ThemeResourceKey<FontFamily> FontFamily => new("SecondaryRatingControlCaptionFontFamily");

					public static partial class Foreground
					{
						[ResourceKeyDefinition(typeof(Brush), "SecondaryRatingControlCaptionForeground")]
						public static ThemeResourceKey<Brush> Default => new("SecondaryRatingControlCaptionForeground");
					}

					[ResourceKeyDefinition(typeof(double), "SecondaryRatingControlCaptionHeight")]
					public static ThemeResourceKey<double> Height => new("SecondaryRatingControlCaptionHeight");

					[ResourceKeyDefinition(typeof(Style), "SecondaryRatingControlCaptionStyle")]
					public static ThemeResourceKey<Style> Style => new("SecondaryRatingControlCaptionStyle");
				}

				public static partial class Typography
				{
					public static ThemeResourceKey<FontFamily> FontFamily => Shared.FontFamily;
				}
			}

			public static partial class Shared
			{
				[ResourceKeyDefinition(typeof(FontFamily), "RatingControlFontFamily")]
				public static ThemeResourceKey<FontFamily> FontFamily => new("RatingControlFontFamily");
			}
		}

		public static partial class Styles
		{
			[ResourceKeyDefinition(typeof(Style), "RatingControlStyle", TargetType = typeof(global::Microsoft.UI.Xaml.Controls.RatingControl))]
			public static StaticResourceKey<Style> Default => new("RatingControlStyle");

			[ResourceKeyDefinition(typeof(Style), "SecondaryRatingControlStyle", TargetType = typeof(global::Microsoft.UI.Xaml.Controls.RatingControl))]
			public static StaticResourceKey<Style> Secondary => new("SecondaryRatingControlStyle");
		}
	}
}
