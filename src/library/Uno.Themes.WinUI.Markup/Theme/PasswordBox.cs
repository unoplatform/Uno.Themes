using Microsoft.UI.Text;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Media;
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
								public static ResourceValue<Geometry> Default => new("PasswordBoxRevealGlyphPathData", true);
							}
						}

						public static class Button
						{
							public static class Foreground
							{
								[ResourceKeyDefinition(typeof(Brush), "PasswordBoxRevealButtonForeground")]
								public static ResourceValue<Brush> Default => new("PasswordBoxRevealButtonForeground", true);
							}
						}
					}
				}

				public static class Filled
				{
					public static class Background
					{
						[ResourceKeyDefinition(typeof(Brush), "FilledPasswordBoxBackground")]
						public static ResourceValue<Brush> Default => new("FilledPasswordBoxBackground", true);

						[ResourceKeyDefinition(typeof(Brush), "FilledPasswordBoxBackgroundPointerOver")]
						public static ResourceValue<Brush> PointerOver => new("FilledPasswordBoxBackgroundPointerOver", true);

						[ResourceKeyDefinition(typeof(Brush), "FilledPasswordBoxBackgroundFocused")]
						public static ResourceValue<Brush> Focused => new("FilledPasswordBoxBackgroundFocused", true);

						[ResourceKeyDefinition(typeof(Brush), "FilledPasswordBoxBackgroundDisabled")]
						public static ResourceValue<Brush> Disabled => new("FilledPasswordBoxBackgroundDisabled", true);
					}

					public static class BorderBrush
					{
						[ResourceKeyDefinition(typeof(Brush), "FilledPasswordBoxBorderBrush")]
						public static ResourceValue<Brush> Default => new("FilledPasswordBoxBorderBrush", true);

						[ResourceKeyDefinition(typeof(Brush), "FilledPasswordBoxBorderBrushPointerOver")]
						public static ResourceValue<Brush> PointerOver => new("FilledPasswordBoxBorderBrushPointerOver", true);

						[ResourceKeyDefinition(typeof(Brush), "FilledPasswordBoxBorderBrushFocused")]
						public static ResourceValue<Brush> Focused => new("FilledPasswordBoxBorderBrushFocused", true);

						[ResourceKeyDefinition(typeof(Brush), "FilledPasswordBoxBorderBrushDisabled")]
						public static ResourceValue<Brush> Disabled => new("FilledPasswordBoxBorderBrushDisabled", true);
					}

					public static class Foreground
					{
						[ResourceKeyDefinition(typeof(Brush), "FilledPasswordBoxForeground")]
						public static ResourceValue<Brush> Default => new("FilledPasswordBoxForeground", true);

						[ResourceKeyDefinition(typeof(Brush), "FilledPasswordBoxForegroundPointerOver")]
						public static ResourceValue<Brush> PointerOver => new("FilledPasswordBoxForegroundPointerOver", true);

						[ResourceKeyDefinition(typeof(Brush), "FilledPasswordBoxForegroundFocused")]
						public static ResourceValue<Brush> Focused => new("FilledPasswordBoxForegroundFocused", true);

						[ResourceKeyDefinition(typeof(Brush), "FilledPasswordBoxForegroundDisabled")]
						public static ResourceValue<Brush> Disabled => new("FilledPasswordBoxForegroundDisabled", true);
					}

					public static class Placeholder
					{
						public static class Foreground
						{
							[ResourceKeyDefinition(typeof(Brush), "FilledPasswordBoxPlaceholderForeground")]
							public static ResourceValue<Brush> Default => new("FilledPasswordBoxPlaceholderForeground", true);

							[ResourceKeyDefinition(typeof(Brush), "FilledPasswordBoxPlaceholderForegroundPointerOver")]
							public static ResourceValue<Brush> PointerOver => new("FilledPasswordBoxPlaceholderForegroundPointerOver", true);

							[ResourceKeyDefinition(typeof(Brush), "FilledPasswordBoxPlaceholderForegroundFocused")]
							public static ResourceValue<Brush> Focused => new("FilledPasswordBoxPlaceholderForegroundFocused", true);

							[ResourceKeyDefinition(typeof(Brush), "FilledPasswordBoxPlaceholderForegroundDisabled")]
							public static ResourceValue<Brush> Disabled => new("FilledPasswordBoxPlaceholderForegroundDisabled", true);
						}
					}

					public static class RevealButton
					{
						public static class Foreground
						{
							[ResourceKeyDefinition(typeof(Brush), "FilledPasswordBoxRevealButtonForeground")]
							public static ResourceValue<Brush> Default => new("FilledPasswordBoxRevealButtonForeground", true);
						}
					}

					public static class Typography
					{
						[ResourceKeyDefinition(typeof(FontFamily), "FilledPasswordBoxFontFamily")]
						public static ResourceValue<FontFamily> FontFamily => new("FilledPasswordBoxFontFamily", true);

						[ResourceKeyDefinition(typeof(FontWeights), "FilledPasswordBoxFontWeight")]
						public static ResourceValue<FontWeights> FontWeight => new("FilledPasswordBoxFontWeight", true);

						[ResourceKeyDefinition(typeof(double), "FilledPasswordBoxFontSize")]
						public static ResourceValue<double> FontSize => new("FilledPasswordBoxFontSize", true);

						[ResourceKeyDefinition(typeof(int), "FilledPasswordBoxCharacterSpacing")]
						public static ResourceValue<int> CharacterSpacing => new("FilledPasswordBoxCharacterSpacing", true);
					}
				}

				public static class Outlined
				{
					public static class Background
					{
						[ResourceKeyDefinition(typeof(Brush), "OutlinedPasswordBoxBackground")]
						public static ResourceValue<Brush> Default => new("OutlinedPasswordBoxBackground", true);

						[ResourceKeyDefinition(typeof(Brush), "OutlinedPasswordBoxBackgroundPointerOver")]
						public static ResourceValue<Brush> PointerOver => new("OutlinedPasswordBoxBackgroundPointerOver", true);

						[ResourceKeyDefinition(typeof(Brush), "OutlinedPasswordBoxBackgroundFocused")]
						public static ResourceValue<Brush> Focused => new("OutlinedPasswordBoxBackgroundFocused", true);

						[ResourceKeyDefinition(typeof(Brush), "OutlinedPasswordBoxBackgroundDisabled")]
						public static ResourceValue<Brush> Disabled => new("OutlinedPasswordBoxBackgroundDisabled", true);
					}

					public static class BorderBrush
					{
						[ResourceKeyDefinition(typeof(Brush), "OutlinedPasswordBoxBorderBrush")]
						public static ResourceValue<Brush> Default => new("OutlinedPasswordBoxBorderBrush", true);

						[ResourceKeyDefinition(typeof(Brush), "OutlinedPasswordBoxBorderBrushPointerOver")]
						public static ResourceValue<Brush> PointerOver => new("OutlinedPasswordBoxBorderBrushPointerOver", true);

						[ResourceKeyDefinition(typeof(Brush), "OutlinedPasswordBoxBorderBrushFocused")]
						public static ResourceValue<Brush> Focused => new("OutlinedPasswordBoxBorderBrushFocused", true);

						[ResourceKeyDefinition(typeof(Brush), "OutlinedPasswordBoxBorderBrushDisabled")]
						public static ResourceValue<Brush> Disabled => new("OutlinedPasswordBoxBorderBrushDisabled", true);
					}

					public static class Foreground
					{
						[ResourceKeyDefinition(typeof(Brush), "OutlinedPasswordBoxForeground")]
						public static ResourceValue<Brush> Default => new("OutlinedPasswordBoxForeground", true);

						[ResourceKeyDefinition(typeof(Brush), "OutlinedPasswordBoxForegroundPointerOver")]
						public static ResourceValue<Brush> PointerOver => new("OutlinedPasswordBoxForegroundPointerOver", true);

						[ResourceKeyDefinition(typeof(Brush), "OutlinedPasswordBoxForegroundFocused")]
						public static ResourceValue<Brush> Focused => new("OutlinedPasswordBoxForegroundFocused", true);

						[ResourceKeyDefinition(typeof(Brush), "OutlinedPasswordBoxForegroundDisabled")]
						public static ResourceValue<Brush> Disabled => new("OutlinedPasswordBoxForegroundDisabled", true);
					}

					public static class Placeholder
					{
						public static class Foreground
						{
							[ResourceKeyDefinition(typeof(Brush), "OutlinedPasswordBoxPlaceholderForeground")]
							public static ResourceValue<Brush> Default => new("OutlinedPasswordBoxPlaceholderForeground", true);

							[ResourceKeyDefinition(typeof(Brush), "OutlinedPasswordBoxPlaceholderForegroundPointerOver")]
							public static ResourceValue<Brush> PointerOver => new("OutlinedPasswordBoxPlaceholderForegroundPointerOver", true);

							[ResourceKeyDefinition(typeof(Brush), "OutlinedPasswordBoxPlaceholderForegroundFocused")]
							public static ResourceValue<Brush> Focused => new("OutlinedPasswordBoxPlaceholderForegroundFocused", true);

							[ResourceKeyDefinition(typeof(Brush), "OutlinedPasswordBoxPlaceholderForegroundDisabled")]
							public static ResourceValue<Brush> Disabled => new("OutlinedPasswordBoxPlaceholderForegroundDisabled", true);
						}
					}

					public static class Typography
					{
						[ResourceKeyDefinition(typeof(FontFamily), "OutlinedPasswordBoxFontFamily")]
						public static ResourceValue<FontFamily> FontFamily => new("OutlinedPasswordBoxFontFamily", true);

						[ResourceKeyDefinition(typeof(FontWeights), "OutlinedPasswordBoxFontWeight")]
						public static ResourceValue<FontWeights> FontWeight => new("OutlinedPasswordBoxFontWeight", true);

						[ResourceKeyDefinition(typeof(double), "OutlinedPasswordBoxFontSize")]
						public static ResourceValue<double> FontSize => new("OutlinedPasswordBoxFontSize", true);

						[ResourceKeyDefinition(typeof(int), "OutlinedPasswordBoxCharacterSpacing")]
						public static ResourceValue<int> CharacterSpacing => new("OutlinedPasswordBoxCharacterSpacing", true);
					}
				}
			}

			public static class Styles
			{
				[ResourceKeyDefinition(typeof(Style), "FilledPasswordBoxStyle", TargetType = typeof(PasswordBox))]
				public static ResourceValue<Style> Filled => new("FilledPasswordBoxStyle");

				[ResourceKeyDefinition(typeof(Style), "OutlinedPasswordBoxStyle", TargetType = typeof(PasswordBox))]
				public static ResourceValue<Style> Outlined => new("OutlinedPasswordBoxStyle");
			}
		}
	}
}
