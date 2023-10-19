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
	public static partial class RatingControl
	{
		public static partial class Resources
		{
			public static partial class RatingControl
			{
				[ResourceKeyDefinition(typeof(Brush), "RatingControlForeground")]
				public static ThemeResourceKey<Brush> Foreground => new("RatingControlForeground");

				[ResourceKeyDefinition(typeof(double), "RatingControlHeight")]
				public static ThemeResourceKey<double> Height => new("RatingControlHeight");
			}

			public static partial class RatingControlCaption
			{
				[ResourceKeyDefinition(typeof(Brush), "RatingControlCaptionForeground")]
				public static ThemeResourceKey<Brush> Foreground => new("RatingControlCaptionForeground");

				[ResourceKeyDefinition(typeof(double), "RatingControlCaptionHeight")]
				public static ThemeResourceKey<double> Height => new("RatingControlCaptionHeight");
			}

			public static partial class RatingControlForegroundSelected
			{
				[ResourceKeyDefinition(typeof(Brush), "RatingControlForegroundSelectedDisabled")]
				public static ThemeResourceKey<Brush> Disabled => new("RatingControlForegroundSelectedDisabled");

				[ResourceKeyDefinition(typeof(Brush), "RatingControlPlaceholderForeground")]
				public static ThemeResourceKey<Brush> Placeholder => new("RatingControlPlaceholderForeground");

				[ResourceKeyDefinition(typeof(Brush), "RatingControlPlaceholderForegroundPointerOver")]
				public static ThemeResourceKey<Brush> PointerOverPlaceholder => new("RatingControlPlaceholderForegroundPointerOver");

				[ResourceKeyDefinition(typeof(Brush), "RatingControlForegroundUnselectedPointerOver")]
				public static ThemeResourceKey<Brush> PointerOverUnselected => new("RatingControlForegroundUnselectedPointerOver");

				[ResourceKeyDefinition(typeof(Brush), "RatingControlForegroundSelected")]
				public static ThemeResourceKey<Brush> Set => new("RatingControlForegroundSelected");
			}

			public static partial class Secondary
			{
				public static partial class Caption
				{
					[ResourceKeyDefinition(typeof(Brush), "SecondaryRatingControlCaptionForeground")]
					public static ThemeResourceKey<Brush> Foreground => new("SecondaryRatingControlCaptionForeground");

					[ResourceKeyDefinition(typeof(double), "SecondaryRatingControlCaptionHeight")]
					public static ThemeResourceKey<double> Height => new("SecondaryRatingControlCaptionHeight");
				}

				public static partial class ForegroundSelected
				{
					[ResourceKeyDefinition(typeof(Brush), "SecondaryRatingControlForegroundSelectedDisabled")]
					public static ThemeResourceKey<Brush> Disabled => new("SecondaryRatingControlForegroundSelectedDisabled");

					[ResourceKeyDefinition(typeof(Brush), "SecondaryRatingControlPlaceholderForeground")]
					public static ThemeResourceKey<Brush> Placeholder => new("SecondaryRatingControlPlaceholderForeground");

					[ResourceKeyDefinition(typeof(Brush), "SecondaryRatingControlPlaceholderForegroundPointerOver")]
					public static ThemeResourceKey<Brush> PointerOverPlaceholder => new("SecondaryRatingControlPlaceholderForegroundPointerOver");

					[ResourceKeyDefinition(typeof(Brush), "SecondaryRatingControlForegroundUnselectedPointerOver")]
					public static ThemeResourceKey<Brush> PointerOverUnselected => new("SecondaryRatingControlForegroundUnselectedPointerOver");

					[ResourceKeyDefinition(typeof(Brush), "SecondaryRatingControlForegroundSelected")]
					public static ThemeResourceKey<Brush> Set => new("SecondaryRatingControlForegroundSelected");
				}

				[ResourceKeyDefinition(typeof(Brush), "SecondaryRatingControlForeground")]
				public static ThemeResourceKey<Brush> Foreground => new("SecondaryRatingControlForeground");

				[ResourceKeyDefinition(typeof(FontFamily), "SecondaryRatingControlCaptionFontFamily")]
				public static ThemeResourceKey<FontFamily> Typography => new("SecondaryRatingControlCaptionFontFamily");
			}

			public static partial class Typography
			{
				[ResourceKeyDefinition(typeof(FontFamily), "RatingControlCaptionFontFamily")]
				public static ThemeResourceKey<FontFamily> CaptionFontFamily => new("RatingControlCaptionFontFamily");

				[ResourceKeyDefinition(typeof(FontFamily), "RatingControlFontFamily")]
				public static ThemeResourceKey<FontFamily> FontFamily => new("RatingControlFontFamily");
			}
		}

		public static partial class Styles
		{
			[ResourceKeyDefinition(typeof(Style), "MaterialRatingControlStyle", TargetType = typeof(global::Microsoft.UI.Xaml.Controls.RatingControl))]
			public static StaticResourceKey<Style> Default => new("MaterialRatingControlStyle");

			[ResourceKeyDefinition(typeof(Style), "SecondaryRatingControlStyle", TargetType = typeof(global::Microsoft.UI.Xaml.Controls.RatingControl))]
			public static StaticResourceKey<Style> Secondary => new("SecondaryRatingControlStyle");
		}
	}
}
