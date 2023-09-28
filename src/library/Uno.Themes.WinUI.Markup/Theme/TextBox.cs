using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Media;
using Uno.Extensions.Markup;
using Uno.Extensions.Markup.Internals;
using Windows.UI.Text;

namespace Uno.Themes.Markup
{
	public static partial class Theme
	{
		public static class TextBox
		{
			public static class Resources
			{
				public static class Filled
				{
					public static class Foreground
					{
						[ResourceKeyDefinition(typeof(Brush), "FilledTextBoxForeground")]
						public static ThemeResourceKey<Brush> Default => new("FilledTextBoxForeground");

						[ResourceKeyDefinition(typeof(Brush), "FilledTextBoxForegroundPointerOver")]
						public static ThemeResourceKey<Brush> PointerOver => new("FilledTextBoxForegroundPointerOver");

						[ResourceKeyDefinition(typeof(Brush), "FilledTextBoxForegroundFocused")]
						public static ThemeResourceKey<Brush> Focused => new("FilledTextBoxForegroundFocused");

						[ResourceKeyDefinition(typeof(Brush), "FilledTextBoxForegroundDisabled")]
						public static ThemeResourceKey<Brush> Disabled => new("FilledTextBoxForegroundDisabled");
					}

					public static class PlaceholderForeground
					{
						[ResourceKeyDefinition(typeof(Brush), "FilledTextBoxPlaceholderForeground")]
						public static ThemeResourceKey<Brush> Default => new("FilledTextBoxPlaceholderForeground");

						[ResourceKeyDefinition(typeof(Brush), "FilledTextBoxPlaceholderForegroundPointerOver")]
						public static ThemeResourceKey<Brush> PointerOver => new("FilledTextBoxPlaceholderForegroundPointerOver");

						[ResourceKeyDefinition(typeof(Brush), "FilledTextBoxPlaceholderForegroundFocused")]
						public static ThemeResourceKey<Brush> Focused => new("FilledTextBoxPlaceholderForegroundFocused");

						[ResourceKeyDefinition(typeof(Brush), "FilledTextBoxPlaceholderForegroundDisabled")]
						public static ThemeResourceKey<Brush> Disabled => new("FilledTextBoxPlaceholderForegroundDisabled");
					}

					public static class DeleteButtonForeground
					{
						[ResourceKeyDefinition(typeof(Brush), "FilledTextBoxDeleteButtonForeground")]
						public static ThemeResourceKey<Brush> Default => new("FilledTextBoxDeleteButtonForeground");

						[ResourceKeyDefinition(typeof(Brush), "FilledTextBoxDeleteButtonForegroundPointerOver")]
						public static ThemeResourceKey<Brush> PointerOver => new("FilledTextBoxDeleteButtonForegroundPointerOver");

						[ResourceKeyDefinition(typeof(Brush), "FilledTextBoxDeleteButtonForegroundFocused")]
						public static ThemeResourceKey<Brush> Focused => new("FilledTextBoxDeleteButtonForegroundFocused");

						[ResourceKeyDefinition(typeof(Brush), "FilledTextBoxDeleteButtonForegroundDisabled")]
						public static ThemeResourceKey<Brush> Disabled => new("FilledTextBoxDeleteButtonForegroundDisabled");
					}

					public static class Background
					{
						[ResourceKeyDefinition(typeof(Brush), "FilledTextBoxBackground")]
						public static ThemeResourceKey<Brush> Default => new("FilledTextBoxBackground");

						[ResourceKeyDefinition(typeof(Brush), "FilledTextBoxBackgroundPointerOver")]
						public static ThemeResourceKey<Brush> PointerOver => new("FilledTextBoxBackgroundPointerOver");

						[ResourceKeyDefinition(typeof(Brush), "FilledTextBoxBackgroundFocused")]
						public static ThemeResourceKey<Brush> Focused => new("FilledTextBoxBackgroundFocused");

						[ResourceKeyDefinition(typeof(Brush), "FilledTextBoxBackgroundDisabled")]
						public static ThemeResourceKey<Brush> Disabled => new("FilledTextBoxBackgroundDisabled");

					}

					public static class BorderBrush
					{
						[ResourceKeyDefinition(typeof(Brush), "FilledTextBoxBorderBrush")]
						public static ThemeResourceKey<Brush> Default => new("FilledTextBoxBorderBrush");

						[ResourceKeyDefinition(typeof(Brush), "FilledTextBoxBorderBrushPointerOver")]
						public static ThemeResourceKey<Brush> PointerOver => new("FilledTextBoxBorderBrushPointerOver");

						[ResourceKeyDefinition(typeof(Brush), "FilledTextBoxBorderBrushFocused")]
						public static ThemeResourceKey<Brush> Focused => new("FilledTextBoxBorderBrushFocused");

						[ResourceKeyDefinition(typeof(Brush), "FilledTextBoxBorderBrushDisabled")]
						public static ThemeResourceKey<Brush> Disabled => new("FilledTextBoxBorderBrushDisabled");
					}

					public static class BorderThickness
					{
						[ResourceKeyDefinition(typeof(Thickness), "FilledTextBoxBorderThicknessNormal")]
						public static ThemeResourceKey<Thickness> Default => new("FilledTextBoxBorderThicknessNormal");

						[ResourceKeyDefinition(typeof(Thickness), "FilledTextBoxBorderThicknessFocused")]
						public static ThemeResourceKey<Thickness> Focused => new("FilledTextBoxBorderThicknessFocused");
					}

					public static class Typography
					{
						[ResourceKeyDefinition(typeof(FontFamily), "FilledTextBoxFontFamily")]
						public static ThemeResourceKey<FontFamily> FontFamily => new("FilledTextBoxFontFamily");

						[ResourceKeyDefinition(typeof(double), "FilledTextBoxFontSize")]
						public static ThemeResourceKey<double> FontSize => new("FilledTextBoxFontSize");

						[ResourceKeyDefinition(typeof(FontWeight), "FilledTextBoxFontWeight")]
						public static ThemeResourceKey<FontWeight> FontWeight => new("FilledTextBoxFontWeight");

						[ResourceKeyDefinition(typeof(int), "FilledTextBoxCharacterSpacing")]
						public static ThemeResourceKey<int> CharacterSpacing => new("FilledTextBoxCharacterSpacing");
					}

					[ResourceKeyDefinition(typeof(Microsoft.UI.Xaml.CornerRadius), "MaterialFilledTextBoxCornerRadius")]
					public static ThemeResourceKey<Microsoft.UI.Xaml.CornerRadius> CornerRadius => new("MaterialFilledTextBoxCornerRadius");

					[ResourceKeyDefinition(typeof(Thickness), "MaterialFilledTextBoxPadding")]
					public static ThemeResourceKey<Thickness> Padding => new("MaterialFilledTextBoxPadding");

					[ResourceKeyDefinition(typeof(double), "MaterialFilledTextBoxMinHeight")]
					public static ThemeResourceKey<double> MinHeight => new("MaterialFilledTextBoxMinHeight");

					[ResourceKeyDefinition(typeof(double), "MaterialFilledTextBoxBorderHeightFocused")]
					public static ThemeResourceKey<double> BorderHeightFocused => new("MaterialFilledTextBoxBorderHeightFocused");
				}

				public static class Outlined
				{
					public static class Foreground
					{
						[ResourceKeyDefinition(typeof(Brush), "OutlinedTextBoxForeground")]
						public static ThemeResourceKey<Brush> Default => new("OutlinedTextBoxForeground");

						[ResourceKeyDefinition(typeof(Brush), "OutlinedTextBoxForegroundPointerOver")]
						public static ThemeResourceKey<Brush> PointerOver => new("OutlinedTextBoxForegroundPointerOver");

						[ResourceKeyDefinition(typeof(Brush), "OutlinedTextBoxForegroundFocused")]
						public static ThemeResourceKey<Brush> Focused => new("OutlinedTextBoxForegroundFocused");

						[ResourceKeyDefinition(typeof(Brush), "OutlinedTextBoxForegroundDisabled")]
						public static ThemeResourceKey<Brush> Disabled => new("OutlinedTextBoxForegroundDisabled");
					}

					public static class PlaceholderForeground
					{
						[ResourceKeyDefinition(typeof(Brush), "OutlinedTextBoxPlaceholderForeground")]
						public static ThemeResourceKey<Brush> Default => new("OutlinedTextBoxPlaceholderForeground");

						[ResourceKeyDefinition(typeof(Brush), "OutlinedTextBoxPlaceholderForegroundPointerOver")]
						public static ThemeResourceKey<Brush> PointerOver => new("OutlinedTextBoxPlaceholderForegroundPointerOver");

						[ResourceKeyDefinition(typeof(Brush), "OutlinedTextBoxPlaceholderForegroundFocused")]
						public static ThemeResourceKey<Brush> Focused => new("OutlinedTextBoxPlaceholderForegroundFocused");

						[ResourceKeyDefinition(typeof(Brush), "OutlinedTextBoxPlaceholderForegroundDisabled")]
						public static ThemeResourceKey<Brush> Disabled => new("OutlinedTextBoxPlaceholderForegroundDisabled");
					}

					public static class BorderBrush
					{
						[ResourceKeyDefinition(typeof(Brush), "OutlinedTextBoxBorderBrush")]
						public static ThemeResourceKey<Brush> Default => new("OutlinedTextBoxBorderBrush");

						[ResourceKeyDefinition(typeof(Brush), "OutlinedTextBoxBorderBrushPointerOver")]
						public static ThemeResourceKey<Brush> PointerOver => new("OutlinedTextBoxBorderBrushPointerOver");

						[ResourceKeyDefinition(typeof(Brush), "OutlinedTextBoxBorderBrushFocused")]
						public static ThemeResourceKey<Brush> Focused => new("OutlinedTextBoxBorderBrushFocused");

						[ResourceKeyDefinition(typeof(Brush), "OutlinedTextBoxBorderBrushDisabled")]
						public static ThemeResourceKey<Brush> Disabled => new("OutlinedTextBoxBorderBrushDisabled");
					}

					public static class Typography
					{
						[ResourceKeyDefinition(typeof(FontFamily), "OutlinedTextBoxFontFamily")]
						public static ThemeResourceKey<FontFamily> FontFamily => new("OutlinedTextBoxFontFamily");

						[ResourceKeyDefinition(typeof(double), "OutlinedTextBoxFontSize")]
						public static ThemeResourceKey<double> FontSize => new("OutlinedTextBoxFontSize");

						[ResourceKeyDefinition(typeof(FontWeight), "OutlinedTextBoxFontWeight")]
						public static ThemeResourceKey<FontWeight> FontWeight => new("OutlinedTextBoxFontWeight");

						[ResourceKeyDefinition(typeof(int), "OutlinedTextBoxCharacterSpacing")]
						public static ThemeResourceKey<int> CharacterSpacing => new("OutlinedTextBoxCharacterSpacing");
					}

					[ResourceKeyDefinition(typeof(double), "MaterialOutlinedTextBoxBorderThickness")]
					public static ThemeResourceKey<double> BorderThickness => new("MaterialOutlinedTextBoxBorderThickness");

					[ResourceKeyDefinition(typeof(Microsoft.UI.Xaml.CornerRadius), "MaterialOutlinedTextBoxCornerRadius")]
					public static ThemeResourceKey<Microsoft.UI.Xaml.CornerRadius> CornerRadius => new("MaterialOutlinedTextBoxCornerRadius");

					[ResourceKeyDefinition(typeof(Thickness), "MaterialOutlinedTextBoxPadding")]
					public static ThemeResourceKey<Thickness> Padding => new("MaterialOutlinedTextBoxPadding");

					[ResourceKeyDefinition(typeof(double), "MaterialOutlinedTextBoxMinHeight")]
					public static ThemeResourceKey<double> MinHeight => new("MaterialOutlinedTextBoxMinHeight");

					[ResourceKeyDefinition(typeof(double), "MaterialOutlinedTextBoxBorderPadding")]
					public static ThemeResourceKey<double> BorderPadding => new("MaterialOutlinedTextBoxBorderPadding");

					[ResourceKeyDefinition(typeof(double), "MaterialOutlinedTextBoxBorderThicknessFocused")]
					public static ThemeResourceKey<double> BorderThicknessFocused => new("MaterialOutlinedTextBoxBorderThicknessFocused");
				}

				public static class DeleteButtonForeground
				{
					[ResourceKeyDefinition(typeof(Brush), "TextBoxDeleteButtonForeground")]
					public static ThemeResourceKey<Brush> Default => new("TextBoxDeleteButtonForeground");

					[ResourceKeyDefinition(typeof(Brush), "TextBoxDeleteButtonForegroundPointerOver")]
					public static ThemeResourceKey<Brush> PointerOver => new("TextBoxDeleteButtonForegroundPointerOver");

					[ResourceKeyDefinition(typeof(Brush), "TextBoxDeleteButtonForegroundPressed")]
					public static ThemeResourceKey<Brush> Pressed => new("TextBoxDeleteButtonForegroundPressed");

					[ResourceKeyDefinition(typeof(Brush), "TextBoxDeleteButtonForegroundDisabled")]
					public static ThemeResourceKey<Brush> Disabled => new("TextBoxDeleteButtonForegroundDisabled");
				}

				public static class LeadingIconForeground
				{
					[ResourceKeyDefinition(typeof(Brush), "TextBoxLeadingIconForeground")]
					public static ThemeResourceKey<Brush> Default => new("TextBoxLeadingIconForeground");

					[ResourceKeyDefinition(typeof(Brush), "TextBoxLeadingIconForegroundDisabled")]
					public static ThemeResourceKey<Brush> Disabled => new("TextBoxLeadingIconForegroundDisabled");
				}

				[ResourceKeyDefinition(typeof(double), "MaterialTextBoxClearGlyphWidth")]
				public static ThemeResourceKey<double> ClearGlyphWidth => new("MaterialTextBoxClearGlyphWidth");

				[ResourceKeyDefinition(typeof(double), "MaterialTextBoxClearGlyphHeight")]
				public static ThemeResourceKey<double> ClearGlyphHeight => new("MaterialTextBoxClearGlyphHeight");
			}

			public static class Styles
			{
				[ResourceKeyDefinition(typeof(Style), "FilledTextBoxStyle", TargetType = typeof(TextBox))]
				public static StaticResourceKey<Style> Filled => new("FilledTextBoxStyle");

				[ResourceKeyDefinition(typeof(Style), "OutlinedTextBoxStyle", TargetType = typeof(TextBox))]
				public static StaticResourceKey<Style> Outlined => new("OutlinedTextBoxStyle");
			}
		}
	}
}
