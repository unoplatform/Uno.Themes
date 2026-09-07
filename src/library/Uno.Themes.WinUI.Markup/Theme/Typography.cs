using Microsoft.UI.Xaml.Media;
using Windows.UI.Text;
using Uno.Extensions.Markup;
using Uno.Extensions.Markup.Internals;

namespace Uno.Themes.Markup;

public static partial class Theme
{
	public static class Typography
	{
		/// <summary>Gets the root font family resource.</summary>
		[ResourceKeyDefinition(typeof(FontFamily), "DefaultFontFamily")]
		public static ThemeResourceKey<FontFamily> DefaultFontFamily => new("DefaultFontFamily");

		public static class DisplayLarge
		{
			/// <summary>Gets the DisplayLarge font family resource.</summary>
			[ResourceKeyDefinition(typeof(FontFamily), "DisplayLargeFontFamily")]
			public static ThemeResourceKey<FontFamily> FontFamily => new("DisplayLargeFontFamily");

			/// <summary>Gets the font weight resource as a XAML font weight value.</summary>
			[ResourceKeyDefinition(typeof(FontWeight), "DisplayLargeFontWeight")]
			public static ThemeResourceKey<FontWeight> FontWeight => new("DisplayLargeFontWeight");

			[ResourceKeyDefinition(typeof(double), "DisplayLargeFontSize")]
			public static ThemeResourceKey<double> FontSize => new("DisplayLargeFontSize");

			[ResourceKeyDefinition(typeof(int), "DisplayLargeCharacterSpacing")]
			public static ThemeResourceKey<int> CharacterSpacing => new("DisplayLargeCharacterSpacing");
		}

		public static class DisplayMedium
		{
			/// <summary>Gets the DisplayMedium font family resource.</summary>
			[ResourceKeyDefinition(typeof(FontFamily), "DisplayMediumFontFamily")]
			public static ThemeResourceKey<FontFamily> FontFamily => new("DisplayMediumFontFamily");

			/// <summary>Gets the font weight resource as a XAML font weight value.</summary>
			[ResourceKeyDefinition(typeof(FontWeight), "DisplayMediumFontWeight")]
			public static ThemeResourceKey<FontWeight> FontWeight => new("DisplayMediumFontWeight");

			[ResourceKeyDefinition(typeof(double), "DisplayMediumFontSize")]
			public static ThemeResourceKey<double> FontSize => new("DisplayMediumFontSize");
		}

		public static class DisplaySmall
		{
			/// <summary>Gets the DisplaySmall font family resource.</summary>
			[ResourceKeyDefinition(typeof(FontFamily), "DisplaySmallFontFamily")]
			public static ThemeResourceKey<FontFamily> FontFamily => new("DisplaySmallFontFamily");

			/// <summary>Gets the font weight resource as a XAML font weight value.</summary>
			[ResourceKeyDefinition(typeof(FontWeight), "DisplaySmallFontWeight")]
			public static ThemeResourceKey<FontWeight> FontWeight => new("DisplaySmallFontWeight");

			[ResourceKeyDefinition(typeof(double), "DisplaySmallFontSize")]
			public static ThemeResourceKey<double> FontSize => new("DisplaySmallFontSize");
		}

		public static class HeadlineLarge
		{
			/// <summary>Gets the HeadlineLarge font family resource.</summary>
			[ResourceKeyDefinition(typeof(FontFamily), "HeadlineLargeFontFamily")]
			public static ThemeResourceKey<FontFamily> FontFamily => new("HeadlineLargeFontFamily");

			/// <summary>Gets the font weight resource as a XAML font weight value.</summary>
			[ResourceKeyDefinition(typeof(FontWeight), "HeadlineLargeFontWeight")]
			public static ThemeResourceKey<FontWeight> FontWeight => new("HeadlineLargeFontWeight");

			[ResourceKeyDefinition(typeof(double), "HeadlineLargeFontSize")]
			public static ThemeResourceKey<double> FontSize => new("HeadlineLargeFontSize");
		}

		public static class HeadlineMedium
		{
			/// <summary>Gets the HeadlineMedium font family resource.</summary>
			[ResourceKeyDefinition(typeof(FontFamily), "HeadlineMediumFontFamily")]
			public static ThemeResourceKey<FontFamily> FontFamily => new("HeadlineMediumFontFamily");

			/// <summary>Gets the font weight resource as a XAML font weight value.</summary>
			[ResourceKeyDefinition(typeof(FontWeight), "HeadlineMediumFontWeight")]
			public static ThemeResourceKey<FontWeight> FontWeight => new("HeadlineMediumFontWeight");

			[ResourceKeyDefinition(typeof(double), "HeadlineMediumFontSize")]
			public static ThemeResourceKey<double> FontSize => new("HeadlineMediumFontSize");
		}

		public static class HeadlineSmall
		{
			/// <summary>Gets the HeadlineSmall font family resource.</summary>
			[ResourceKeyDefinition(typeof(FontFamily), "HeadlineSmallFontFamily")]
			public static ThemeResourceKey<FontFamily> FontFamily => new("HeadlineSmallFontFamily");

			/// <summary>Gets the font weight resource as a XAML font weight value.</summary>
			[ResourceKeyDefinition(typeof(FontWeight), "HeadlineSmallFontWeight")]
			public static ThemeResourceKey<FontWeight> FontWeight => new("HeadlineSmallFontWeight");

			[ResourceKeyDefinition(typeof(double), "HeadlineSmallFontSize")]
			public static ThemeResourceKey<double> FontSize => new("HeadlineSmallFontSize");
		}

		public static class TitleLarge
		{
			/// <summary>Gets the TitleLarge font family resource.</summary>
			[ResourceKeyDefinition(typeof(FontFamily), "TitleLargeFontFamily")]
			public static ThemeResourceKey<FontFamily> FontFamily => new("TitleLargeFontFamily");

			/// <summary>Gets the font weight resource as a XAML font weight value.</summary>
			[ResourceKeyDefinition(typeof(FontWeight), "TitleLargeFontWeight")]
			public static ThemeResourceKey<FontWeight> FontWeight => new("TitleLargeFontWeight");

			[ResourceKeyDefinition(typeof(double), "TitleLargeFontSize")]
			public static ThemeResourceKey<double> FontSize => new("TitleLargeFontSize");
		}

		public static class TitleMedium
		{
			/// <summary>Gets the TitleMedium font family resource.</summary>
			[ResourceKeyDefinition(typeof(FontFamily), "TitleMediumFontFamily")]
			public static ThemeResourceKey<FontFamily> FontFamily => new("TitleMediumFontFamily");

			/// <summary>Gets the font weight resource as a XAML font weight value.</summary>
			[ResourceKeyDefinition(typeof(FontWeight), "TitleMediumFontWeight")]
			public static ThemeResourceKey<FontWeight> FontWeight => new("TitleMediumFontWeight");

			[ResourceKeyDefinition(typeof(double), "TitleMediumFontSize")]
			public static ThemeResourceKey<double> FontSize => new("TitleMediumFontSize");
		}

		public static class TitleSmall
		{
			/// <summary>Gets the TitleSmall font family resource.</summary>
			[ResourceKeyDefinition(typeof(FontFamily), "TitleSmallFontFamily")]
			public static ThemeResourceKey<FontFamily> FontFamily => new("TitleSmallFontFamily");

			/// <summary>Gets the font weight resource as a XAML font weight value.</summary>
			[ResourceKeyDefinition(typeof(FontWeight), "TitleSmallFontWeight")]
			public static ThemeResourceKey<FontWeight> FontWeight => new("TitleSmallFontWeight");

			[ResourceKeyDefinition(typeof(double), "TitleSmallFontSize")]
			public static ThemeResourceKey<double> FontSize => new("TitleSmallFontSize");
		}

		public static class LabelLarge
		{
			/// <summary>Gets the LabelLarge font family resource.</summary>
			[ResourceKeyDefinition(typeof(FontFamily), "LabelLargeFontFamily")]
			public static ThemeResourceKey<FontFamily> FontFamily => new("LabelLargeFontFamily");

			/// <summary>Gets the font weight resource as a XAML font weight value.</summary>
			[ResourceKeyDefinition(typeof(FontWeight), "LabelLargeFontWeight")]
			public static ThemeResourceKey<FontWeight> FontWeight => new("LabelLargeFontWeight");

			[ResourceKeyDefinition(typeof(double), "LabelLargeFontSize")]
			public static ThemeResourceKey<double> FontSize => new("LabelLargeFontSize");

			[ResourceKeyDefinition(typeof(int), "LabelLargeCharacterSpacing")]
			public static ThemeResourceKey<int> CharacterSpacing => new("LabelLargeCharacterSpacing");
		}

		public static class LabelMedium
		{
			/// <summary>Gets the LabelMedium font family resource.</summary>
			[ResourceKeyDefinition(typeof(FontFamily), "LabelMediumFontFamily")]
			public static ThemeResourceKey<FontFamily> FontFamily => new("LabelMediumFontFamily");

			/// <summary>Gets the font weight resource as a XAML font weight value.</summary>
			[ResourceKeyDefinition(typeof(FontWeight), "LabelMediumFontWeight")]
			public static ThemeResourceKey<FontWeight> FontWeight => new("LabelMediumFontWeight");

			[ResourceKeyDefinition(typeof(double), "LabelMediumFontSize")]
			public static ThemeResourceKey<double> FontSize => new("LabelMediumFontSize");

			[ResourceKeyDefinition(typeof(int), "LabelMediumCharacterSpacing")]
			public static ThemeResourceKey<int> CharacterSpacing => new("LabelMediumCharacterSpacing");
		}

		public static class LabelSmall
		{
			/// <summary>Gets the LabelSmall font family resource.</summary>
			[ResourceKeyDefinition(typeof(FontFamily), "LabelSmallFontFamily")]
			public static ThemeResourceKey<FontFamily> FontFamily => new("LabelSmallFontFamily");

			/// <summary>Gets the font weight resource as a XAML font weight value.</summary>
			[ResourceKeyDefinition(typeof(FontWeight), "LabelSmallFontWeight")]
			public static ThemeResourceKey<FontWeight> FontWeight => new("LabelSmallFontWeight");

			[ResourceKeyDefinition(typeof(double), "LabelSmallFontSize")]
			public static ThemeResourceKey<double> FontSize => new("LabelSmallFontSize");

			[ResourceKeyDefinition(typeof(int), "LabelSmallCharacterSpacing")]
			public static ThemeResourceKey<int> CharacterSpacing => new("LabelSmallCharacterSpacing");
		}

		public static class LabelExtraSmall
		{
			/// <summary>Gets the LabelExtraSmall font family resource.</summary>
			[ResourceKeyDefinition(typeof(FontFamily), "LabelExtraSmallFontFamily")]
			public static ThemeResourceKey<FontFamily> FontFamily => new("LabelExtraSmallFontFamily");

			/// <summary>Gets the font weight resource as a XAML font weight value.</summary>
			[ResourceKeyDefinition(typeof(FontWeight), "LabelExtraSmallFontWeight")]
			public static ThemeResourceKey<FontWeight> FontWeight => new("LabelExtraSmallFontWeight");

			[ResourceKeyDefinition(typeof(double), "LabelExtraSmallFontSize")]
			public static ThemeResourceKey<double> FontSize => new("LabelExtraSmallFontSize");

			[ResourceKeyDefinition(typeof(int), "LabelExtraSmallCharacterSpacing")]
			public static ThemeResourceKey<int> CharacterSpacing => new("LabelExtraSmallCharacterSpacing");
		}

		public static class BodyLarge
		{
			/// <summary>Gets the BodyLarge font family resource.</summary>
			[ResourceKeyDefinition(typeof(FontFamily), "BodyLargeFontFamily")]
			public static ThemeResourceKey<FontFamily> FontFamily => new("BodyLargeFontFamily");

			/// <summary>Gets the font weight resource as a XAML font weight value.</summary>
			[ResourceKeyDefinition(typeof(FontWeight), "BodyLargeFontWeight")]
			public static ThemeResourceKey<FontWeight> FontWeight => new("BodyLargeFontWeight");

			[ResourceKeyDefinition(typeof(double), "BodyLargeFontSize")]
			public static ThemeResourceKey<double> FontSize => new("BodyLargeFontSize");

			[ResourceKeyDefinition(typeof(int), "BodyLargeCharacterSpacing")]
			public static ThemeResourceKey<int> CharacterSpacing => new("BodyLargeCharacterSpacing");
		}

		public static class BodyMedium
		{
			/// <summary>Gets the BodyMedium font family resource.</summary>
			[ResourceKeyDefinition(typeof(FontFamily), "BodyMediumFontFamily")]
			public static ThemeResourceKey<FontFamily> FontFamily => new("BodyMediumFontFamily");

			/// <summary>Gets the font weight resource as a XAML font weight value.</summary>
			[ResourceKeyDefinition(typeof(FontWeight), "BodyMediumFontWeight")]
			public static ThemeResourceKey<FontWeight> FontWeight => new("BodyMediumFontWeight");

			[ResourceKeyDefinition(typeof(double), "BodyMediumFontSize")]
			public static ThemeResourceKey<double> FontSize => new("BodyMediumFontSize");

			[ResourceKeyDefinition(typeof(int), "BodyMediumCharacterSpacing")]
			public static ThemeResourceKey<int> CharacterSpacing => new("BodyMediumCharacterSpacing");
		}

		public static class BodySmall
		{
			/// <summary>Gets the BodySmall font family resource.</summary>
			[ResourceKeyDefinition(typeof(FontFamily), "BodySmallFontFamily")]
			public static ThemeResourceKey<FontFamily> FontFamily => new("BodySmallFontFamily");

			/// <summary>Gets the font weight resource as a XAML font weight value.</summary>
			[ResourceKeyDefinition(typeof(FontWeight), "BodySmallFontWeight")]
			public static ThemeResourceKey<FontWeight> FontWeight => new("BodySmallFontWeight");


			[ResourceKeyDefinition(typeof(double), "BodySmallFontSize")]
			public static ThemeResourceKey<double> FontSize => new("BodySmallFontSize");

			[ResourceKeyDefinition(typeof(int), "BodySmallCharacterSpacing")]
			public static ThemeResourceKey<int> CharacterSpacing => new("BodySmallCharacterSpacing");
		}

		public static class CaptionLarge
		{
			/// <summary>Gets the CaptionLarge font family resource.</summary>
			[ResourceKeyDefinition(typeof(FontFamily), "CaptionLargeFontFamily")]
			public static ThemeResourceKey<FontFamily> FontFamily => new("CaptionLargeFontFamily");

			/// <summary>Gets the font weight resource as a XAML font weight value.</summary>
			[ResourceKeyDefinition(typeof(FontWeight), "CaptionLargeFontWeight")]
			public static ThemeResourceKey<FontWeight> FontWeight => new("CaptionLargeFontWeight");

			[ResourceKeyDefinition(typeof(double), "CaptionLargeFontSize")]
			public static ThemeResourceKey<double> FontSize => new("CaptionLargeFontSize");

			[ResourceKeyDefinition(typeof(int), "CaptionLargeCharacterSpacing")]
			public static ThemeResourceKey<int> CharacterSpacing => new("CaptionLargeCharacterSpacing");
		}

		public static class CaptionMedium
		{
			/// <summary>Gets the CaptionMedium font family resource.</summary>
			[ResourceKeyDefinition(typeof(FontFamily), "CaptionMediumFontFamily")]
			public static ThemeResourceKey<FontFamily> FontFamily => new("CaptionMediumFontFamily");

			/// <summary>Gets the font weight resource as a XAML font weight value.</summary>
			[ResourceKeyDefinition(typeof(FontWeight), "CaptionMediumFontWeight")]
			public static ThemeResourceKey<FontWeight> FontWeight => new("CaptionMediumFontWeight");

			[ResourceKeyDefinition(typeof(double), "CaptionMediumFontSize")]
			public static ThemeResourceKey<double> FontSize => new("CaptionMediumFontSize");

			[ResourceKeyDefinition(typeof(int), "CaptionMediumCharacterSpacing")]
			public static ThemeResourceKey<int> CharacterSpacing => new("CaptionMediumCharacterSpacing");
		}

		public static class CaptionSmall
		{
			/// <summary>Gets the CaptionSmall font family resource.</summary>
			[ResourceKeyDefinition(typeof(FontFamily), "CaptionSmallFontFamily")]
			public static ThemeResourceKey<FontFamily> FontFamily => new("CaptionSmallFontFamily");

			/// <summary>Gets the font weight resource as a XAML font weight value.</summary>
			[ResourceKeyDefinition(typeof(FontWeight), "CaptionSmallFontWeight")]
			public static ThemeResourceKey<FontWeight> FontWeight => new("CaptionSmallFontWeight");

			[ResourceKeyDefinition(typeof(double), "CaptionSmallFontSize")]
			public static ThemeResourceKey<double> FontSize => new("CaptionSmallFontSize");

			[ResourceKeyDefinition(typeof(int), "CaptionSmallCharacterSpacing")]
			public static ThemeResourceKey<int> CharacterSpacing => new("CaptionSmallCharacterSpacing");
		}
	}
}
