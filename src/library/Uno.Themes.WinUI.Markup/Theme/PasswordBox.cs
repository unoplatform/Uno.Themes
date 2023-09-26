using Microsoft.UI.Text;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Media;
using Uno.Extensions.Markup;
using Uno.Extensions.Markup.Internals;

namespace Uno.Themes.Markup
{
	public static partial class Theme
	{
		public static class PasswordBox
		{
			public static class Resources
			{
				public static class Default
				{
					public static class Reveal
					{
						public static class Glyph
						{
							public static class PathData
							{
								[ResourceKeyDefinition(typeof(Geometry), "PasswordBoxRevealGlyphPathData")]
								public static ThemeResourceKey<Geometry> Default => new("PasswordBoxRevealGlyphPathData");
							}
						}

						public static class Button
						{
							public static class Foreground
							{
								[ResourceKeyDefinition(typeof(Brush), "PasswordBoxRevealButtonForeground")]
								public static ThemeResourceKey<Brush> Default => new("PasswordBoxRevealButtonForeground");
							}
						}
					}
				}

				public static class Filled
				{
					public static class Background
					{
						[ResourceKeyDefinition(typeof(Brush), "FilledPasswordBoxBackground")]
						public static ThemeResourceKey<Brush> Default => new("FilledPasswordBoxBackground");

						[ResourceKeyDefinition(typeof(Brush), "FilledPasswordBoxBackgroundPointerOver")]
						public static ThemeResourceKey<Brush> PointerOver => new("FilledPasswordBoxBackgroundPointerOver");

						[ResourceKeyDefinition(typeof(Brush), "FilledPasswordBoxBackgroundFocused")]
						public static ThemeResourceKey<Brush> Focused => new("FilledPasswordBoxBackgroundFocused");

						[ResourceKeyDefinition(typeof(Brush), "FilledPasswordBoxBackgroundDisabled")]
						public static ThemeResourceKey<Brush> Disabled => new("FilledPasswordBoxBackgroundDisabled");
					}

					public static class BorderBrush
					{
						[ResourceKeyDefinition(typeof(Brush), "FilledPasswordBoxBorderBrush")]
						public static ThemeResourceKey<Brush> Default => new("FilledPasswordBoxBorderBrush");

						[ResourceKeyDefinition(typeof(Brush), "FilledPasswordBoxBorderBrushPointerOver")]
						public static ThemeResourceKey<Brush> PointerOver => new("FilledPasswordBoxBorderBrushPointerOver");

						[ResourceKeyDefinition(typeof(Brush), "FilledPasswordBoxBorderBrushFocused")]
						public static ThemeResourceKey<Brush> Focused => new("FilledPasswordBoxBorderBrushFocused");

						[ResourceKeyDefinition(typeof(Brush), "FilledPasswordBoxBorderBrushDisabled")]
						public static ThemeResourceKey<Brush> Disabled => new("FilledPasswordBoxBorderBrushDisabled");
					}

					public static class Foreground
					{
						[ResourceKeyDefinition(typeof(Brush), "FilledPasswordBoxForeground")]
						public static ThemeResourceKey<Brush> Default => new("FilledPasswordBoxForeground");

						[ResourceKeyDefinition(typeof(Brush), "FilledPasswordBoxForegroundPointerOver")]
						public static ThemeResourceKey<Brush> PointerOver => new("FilledPasswordBoxForegroundPointerOver");

						[ResourceKeyDefinition(typeof(Brush), "FilledPasswordBoxForegroundFocused")]
						public static ThemeResourceKey<Brush> Focused => new("FilledPasswordBoxForegroundFocused");

						[ResourceKeyDefinition(typeof(Brush), "FilledPasswordBoxForegroundDisabled")]
						public static ThemeResourceKey<Brush> Disabled => new("FilledPasswordBoxForegroundDisabled");
					}

					public static class Placeholder
					{
						public static class Foreground
						{
							[ResourceKeyDefinition(typeof(Brush), "FilledPasswordBoxPlaceholderForeground")]
							public static ThemeResourceKey<Brush> Default => new("FilledPasswordBoxPlaceholderForeground");

							[ResourceKeyDefinition(typeof(Brush), "FilledPasswordBoxPlaceholderForegroundPointerOver")]
							public static ThemeResourceKey<Brush> PointerOver => new("FilledPasswordBoxPlaceholderForegroundPointerOver");

							[ResourceKeyDefinition(typeof(Brush), "FilledPasswordBoxPlaceholderForegroundFocused")]
							public static ThemeResourceKey<Brush> Focused => new("FilledPasswordBoxPlaceholderForegroundFocused");

							[ResourceKeyDefinition(typeof(Brush), "FilledPasswordBoxPlaceholderForegroundDisabled")]
							public static ThemeResourceKey<Brush> Disabled => new("FilledPasswordBoxPlaceholderForegroundDisabled");
						}
					}

					public static class RevealButton
					{
						public static class Foreground
						{
							[ResourceKeyDefinition(typeof(Brush), "FilledPasswordBoxRevealButtonForeground")]
							public static ThemeResourceKey<Brush> Default => new("FilledPasswordBoxRevealButtonForeground");
						}
					}

					public static class Typography
					{
						[ResourceKeyDefinition(typeof(FontFamily), "FilledPasswordBoxFontFamily")]
						public static StaticResourceKey<FontFamily> FontFamily => new("FilledPasswordBoxFontFamily");

						[ResourceKeyDefinition(typeof(FontWeights), "FilledPasswordBoxFontWeight")]
						public static StaticResourceKey<FontWeights> FontWeight => new("FilledPasswordBoxFontWeight");

						[ResourceKeyDefinition(typeof(double), "FilledPasswordBoxFontSize")]
						public static StaticResourceKey<double> FontSize => new("FilledPasswordBoxFontSize");

						[ResourceKeyDefinition(typeof(int), "FilledPasswordBoxCharacterSpacing")]
						public static StaticResourceKey<int> CharacterSpacing => new("FilledPasswordBoxCharacterSpacing");
					}
				}

				public static class Outlined
				{
					public static class Background
					{
						[ResourceKeyDefinition(typeof(Brush), "OutlinedPasswordBoxBackground")]
						public static ThemeResourceKey<Brush> Default => new("OutlinedPasswordBoxBackground");

						[ResourceKeyDefinition(typeof(Brush), "OutlinedPasswordBoxBackgroundPointerOver")]
						public static ThemeResourceKey<Brush> PointerOver => new("OutlinedPasswordBoxBackgroundPointerOver");

						[ResourceKeyDefinition(typeof(Brush), "OutlinedPasswordBoxBackgroundFocused")]
						public static ThemeResourceKey<Brush> Focused => new("OutlinedPasswordBoxBackgroundFocused");

						[ResourceKeyDefinition(typeof(Brush), "OutlinedPasswordBoxBackgroundDisabled")]
						public static ThemeResourceKey<Brush> Disabled => new("OutlinedPasswordBoxBackgroundDisabled");
					}

					public static class BorderBrush
					{
						[ResourceKeyDefinition(typeof(Brush), "OutlinedPasswordBoxBorderBrush")]
						public static ThemeResourceKey<Brush> Default => new("OutlinedPasswordBoxBorderBrush");

						[ResourceKeyDefinition(typeof(Brush), "OutlinedPasswordBoxBorderBrushPointerOver")]
						public static ThemeResourceKey<Brush> PointerOver => new("OutlinedPasswordBoxBorderBrushPointerOver");

						[ResourceKeyDefinition(typeof(Brush), "OutlinedPasswordBoxBorderBrushFocused")]
						public static ThemeResourceKey<Brush> Focused => new("OutlinedPasswordBoxBorderBrushFocused");

						[ResourceKeyDefinition(typeof(Brush), "OutlinedPasswordBoxBorderBrushDisabled")]
						public static ThemeResourceKey<Brush> Disabled => new("OutlinedPasswordBoxBorderBrushDisabled");
					}

					public static class Foreground
					{
						[ResourceKeyDefinition(typeof(Brush), "OutlinedPasswordBoxForeground")]
						public static ThemeResourceKey<Brush> Default => new("OutlinedPasswordBoxForeground");

						[ResourceKeyDefinition(typeof(Brush), "OutlinedPasswordBoxForegroundPointerOver")]
						public static ThemeResourceKey<Brush> PointerOver => new("OutlinedPasswordBoxForegroundPointerOver");

						[ResourceKeyDefinition(typeof(Brush), "OutlinedPasswordBoxForegroundFocused")]
						public static ThemeResourceKey<Brush> Focused => new("OutlinedPasswordBoxForegroundFocused");

						[ResourceKeyDefinition(typeof(Brush), "OutlinedPasswordBoxForegroundDisabled")]
						public static ThemeResourceKey<Brush> Disabled => new("OutlinedPasswordBoxForegroundDisabled");
					}

					public static class Placeholder
					{
						public static class Foreground
						{
							[ResourceKeyDefinition(typeof(Brush), "OutlinedPasswordBoxPlaceholderForeground")]
							public static ThemeResourceKey<Brush> Default => new("OutlinedPasswordBoxPlaceholderForeground");

							[ResourceKeyDefinition(typeof(Brush), "OutlinedPasswordBoxPlaceholderForegroundPointerOver")]
							public static ThemeResourceKey<Brush> PointerOver => new("OutlinedPasswordBoxPlaceholderForegroundPointerOver");

							[ResourceKeyDefinition(typeof(Brush), "OutlinedPasswordBoxPlaceholderForegroundFocused")]
							public static ThemeResourceKey<Brush> Focused => new("OutlinedPasswordBoxPlaceholderForegroundFocused");

							[ResourceKeyDefinition(typeof(Brush), "OutlinedPasswordBoxPlaceholderForegroundDisabled")]
							public static ThemeResourceKey<Brush> Disabled => new("OutlinedPasswordBoxPlaceholderForegroundDisabled");
						}
					}

					public static class Typography
					{
						[ResourceKeyDefinition(typeof(FontFamily), "OutlinedPasswordBoxFontFamily")]
						public static StaticResourceKey<FontFamily> FontFamily => new("OutlinedPasswordBoxFontFamily");

						[ResourceKeyDefinition(typeof(FontWeights), "OutlinedPasswordBoxFontWeight")]
						public static StaticResourceKey<FontWeights> FontWeight => new("OutlinedPasswordBoxFontWeight");

						[ResourceKeyDefinition(typeof(double), "OutlinedPasswordBoxFontSize")]
						public static StaticResourceKey<double> FontSize => new("OutlinedPasswordBoxFontSize");

						[ResourceKeyDefinition(typeof(int), "OutlinedPasswordBoxCharacterSpacing")]
						public static StaticResourceKey<int> CharacterSpacing => new("OutlinedPasswordBoxCharacterSpacing");
					}
				}
			}

			public static class Styles
			{
				[ResourceKeyDefinition(typeof(Style), "FilledPasswordBoxStyle", TargetType = typeof(PasswordBox))]
				public static StaticResourceKey<Style> Filled => new("FilledPasswordBoxStyle");

				[ResourceKeyDefinition(typeof(Style), "OutlinedPasswordBoxStyle", TargetType = typeof(PasswordBox))]
				public static StaticResourceKey<Style> Outlined => new("OutlinedPasswordBoxStyle");
			}
		}
	}
}
