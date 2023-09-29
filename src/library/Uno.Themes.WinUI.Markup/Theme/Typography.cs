using System;
using Microsoft.UI.Text;
using Microsoft.UI.Xaml.Media;
using Uno.Extensions.Markup;
using Uno.Extensions.Markup.Internals;

namespace Uno.Themes.Markup
{
	public static partial class Theme
	{
		public static class Typography
		{
			public static class DisplayLarge
			{
				[ResourceKeyDefinition(typeof(FontWeights), "DisplayLargeFontWeight")]
				public static ThemeResourceKey<FontWeights> FontWeight => new("DisplayLargeFontWeight");

				[ResourceKeyDefinition(typeof(double), "DisplayLargeFontSize")]
				public static ThemeResourceKey<double> FontSize => new("DisplayLargeFontSize");

				[ResourceKeyDefinition(typeof(int), "DisplayLargeCharacterSpacing")]
				public static ThemeResourceKey<int> CharacterSpacing => new("DisplayLargeCharacterSpacing");
			}

			public static class DisplayMedium
			{
				[ResourceKeyDefinition(typeof(FontWeights), "DisplayMediumFontWeight")]
				public static ThemeResourceKey<FontWeights> FontWeight => new("DisplayMediumFontWeight");

				[ResourceKeyDefinition(typeof(double), "DisplayMediumFontSize")]
				public static ThemeResourceKey<double> FontSize => new("DisplayMediumFontSize");
			}

			public static class DisplaySmall
			{
				[ResourceKeyDefinition(typeof(FontWeights), "DisplaySmallFontWeight")]
				public static ThemeResourceKey<FontWeights> FontWeight => new("DisplaySmallFontWeight");

				[ResourceKeyDefinition(typeof(double), "DisplaySmallFontSize")]
				public static ThemeResourceKey<double> FontSize => new("DisplaySmallFontSize");
			}

			public static class HeadlineLarge
			{
				[ResourceKeyDefinition(typeof(FontWeights), "HeadlineLargeFontWeight")]
				public static ThemeResourceKey<FontWeights> FontWeight => new("HeadlineLargeFontWeight");

				[ResourceKeyDefinition(typeof(double), "HeadlineLargeFontSize")]
				public static ThemeResourceKey<double> FontSize => new("HeadlineLargeFontSize");
			}

			public static class HeadlineMedium
			{
				[ResourceKeyDefinition(typeof(FontWeights), "HeadlineMediumFontWeight")]
				public static ThemeResourceKey<FontWeights> FontWeight => new("HeadlineMediumFontWeight");

				[ResourceKeyDefinition(typeof(double), "HeadlineMediumFontSize")]
				public static ThemeResourceKey<double> FontSize => new("HeadlineMediumFontSize");
			}

			public static class HeadlineSmall
			{
				[ResourceKeyDefinition(typeof(FontWeights), "HeadlineSmallFontWeight")]
				public static ThemeResourceKey<FontWeights> FontWeight => new("HeadlineSmallFontWeight");

				[ResourceKeyDefinition(typeof(double), "HeadlineSmallFontSize")]
				public static ThemeResourceKey<double> FontSize => new("HeadlineSmallFontSize");
			}

			public static class TitleLarge
			{
				[ResourceKeyDefinition(typeof(FontWeights), "TitleLargeFontWeight")]
				public static ThemeResourceKey<FontWeights> FontWeight => new("TitleLargeFontWeight");

				[ResourceKeyDefinition(typeof(double), "TitleLargeFontSize")]
				public static ThemeResourceKey<double> FontSize => new("TitleLargeFontSize");
			}

			public static class TitleMedium
			{
				[ResourceKeyDefinition(typeof(FontWeights), "TitleMediumFontWeight")]
				public static ThemeResourceKey<FontWeights> FontWeight => new("TitleMediumFontWeight");

				[ResourceKeyDefinition(typeof(double), "TitleMediumFontSize")]
				public static ThemeResourceKey<double> FontSize => new("TitleMediumFontSize");
			}

			public static class TitleSmall
			{
				[ResourceKeyDefinition(typeof(FontWeights), "TitleSmallFontWeight")]
				public static ThemeResourceKey<FontWeights> FontWeight => new("TitleSmallFontWeight");

				[ResourceKeyDefinition(typeof(double), "TitleSmallFontSize")]
				public static ThemeResourceKey<double> FontSize => new("TitleSmallFontSize");
			}

			public static class LabelLarge
			{
				[ResourceKeyDefinition(typeof(FontWeights), "LabelLargeFontWeight")]
				public static ThemeResourceKey<FontWeights> FontWeight => new("LabelLargeFontWeight");

				[ResourceKeyDefinition(typeof(double), "LabelLargeFontSize")]
				public static ThemeResourceKey<double> FontSize => new("LabelLargeFontSize");

				[ResourceKeyDefinition(typeof(int), "LabelLargeCharacterSpacing")]
				public static ThemeResourceKey<int> CharacterSpacing => new("LabelLargeCharacterSpacing");
			}

			public static class LabelMedium
			{
				[ResourceKeyDefinition(typeof(FontWeights), "LabelMediumFontWeight")]
				public static ThemeResourceKey<FontWeights> FontWeight => new("LabelMediumFontWeight");

				[ResourceKeyDefinition(typeof(double), "LabelMediumFontSize")]
				public static ThemeResourceKey<double> FontSize => new("LabelMediumFontSize");

				[ResourceKeyDefinition(typeof(int), "LabelMediumCharacterSpacing")]
				public static ThemeResourceKey<int> CharacterSpacing => new("LabelMediumCharacterSpacing");
			}

			public static class LabelSmall
			{
				[ResourceKeyDefinition(typeof(FontWeights), "LabelSmallFontWeight")]
				public static ThemeResourceKey<FontWeights> FontWeight => new("LabelSmallFontWeight");

				[ResourceKeyDefinition(typeof(double), "LabelSmallFontSize")]
				public static ThemeResourceKey<double> FontSize => new("LabelSmallFontSize");

				[ResourceKeyDefinition(typeof(int), "LabelSmallCharacterSpacing")]
				public static ThemeResourceKey<int> CharacterSpacing => new("LabelSmallCharacterSpacing");
			}

			public static class LabelExtraSmall
			{
				[ResourceKeyDefinition(typeof(FontWeights), "LabelExtraSmallFontWeight")]
				public static ThemeResourceKey<FontWeights> FontWeight => new("LabelExtraSmallFontWeight");

				[ResourceKeyDefinition(typeof(double), "LabelExtraSmallFontSize")]
				public static ThemeResourceKey<double> FontSize => new("LabelExtraSmallFontSize");

				[ResourceKeyDefinition(typeof(int), "LabelExtraSmallCharacterSpacing")]
				public static ThemeResourceKey<int> CharacterSpacing => new("LabelExtraSmallCharacterSpacing");
			}

			public static class BodyLarge
			{
				[ResourceKeyDefinition(typeof(FontWeights), "BodyLargeFontWeight")]
				public static ThemeResourceKey<FontWeights> FontWeight => new("BodyLargeFontWeight");

				[ResourceKeyDefinition(typeof(double), "BodyLargeFontSize")]
				public static ThemeResourceKey<double> FontSize => new("BodyLargeFontSize");

				[ResourceKeyDefinition(typeof(int), "BodyLargeCharacterSpacing")]
				public static ThemeResourceKey<int> CharacterSpacing => new("BodyLargeCharacterSpacing");
			}

			public static class BodyMedium
			{
				[ResourceKeyDefinition(typeof(FontWeights), "BodyMediumFontWeight")]
				public static ThemeResourceKey<FontWeights> FontWeight => new("BodyMediumFontWeight");

				[ResourceKeyDefinition(typeof(double), "BodyMediumFontSize")]
				public static ThemeResourceKey<double> FontSize => new("BodyMediumFontSize");

				[ResourceKeyDefinition(typeof(int), "BodyMediumCharacterSpacing")]
				public static ThemeResourceKey<int> CharacterSpacing => new("BodyMediumCharacterSpacing");
			}

			public static class BodySmall
			{
				[ResourceKeyDefinition(typeof(FontWeights), "BodySmallFontWeight")]
				public static ThemeResourceKey<FontWeights> FontWeight => new("BodySmallFontWeight");


				[ResourceKeyDefinition(typeof(double), "BodySmallFontSize")]
				public static ThemeResourceKey<double> FontSize => new("BodySmallFontSize");

				[ResourceKeyDefinition(typeof(int), "BodySmallCharacterSpacing")]
				public static ThemeResourceKey<int> CharacterSpacing => new("BodySmallCharacterSpacing");
			}

			public static class CaptionLarge
			{
				[ResourceKeyDefinition(typeof(FontWeights), "CaptionLargeFontWeight")]
				public static ThemeResourceKey<FontWeights> FontWeight => new("CaptionLargeFontWeight");

				[ResourceKeyDefinition(typeof(double), "CaptionLargeFontSize")]
				public static ThemeResourceKey<double> FontSize => new("CaptionLargeFontSize");

				[ResourceKeyDefinition(typeof(int), "CaptionLargeCharacterSpacing")]
				public static ThemeResourceKey<int> CharacterSpacing => new("CaptionLargeCharacterSpacing");
			}

			public static class CaptionMedium
			{
				[ResourceKeyDefinition(typeof(FontWeights), "CaptionMediumFontWeight")]
				public static ThemeResourceKey<FontWeights> FontWeight => new("CaptionMediumFontWeight");

				[ResourceKeyDefinition(typeof(double), "CaptionMediumFontSize")]
				public static ThemeResourceKey<double> FontSize => new("CaptionMediumFontSize");

				[ResourceKeyDefinition(typeof(int), "CaptionMediumCharacterSpacing")]
				public static ThemeResourceKey<int> CharacterSpacing => new("CaptionMediumCharacterSpacing");
			}

			public static class CaptionSmall
			{
				[ResourceKeyDefinition(typeof(FontWeights), "CaptionSmallFontWeight")]
				public static ThemeResourceKey<FontWeights> FontWeight => new("CaptionSmallFontWeight");

				[ResourceKeyDefinition(typeof(double), "CaptionSmallFontSize")]
				public static ThemeResourceKey<double> FontSize => new("CaptionSmallFontSize");

				[ResourceKeyDefinition(typeof(int), "CaptionSmallCharacterSpacing")]
				public static ThemeResourceKey<int> CharacterSpacing => new("CaptionSmallCharacterSpacing");
			}
		}
	}
}
