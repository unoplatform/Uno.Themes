using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Media;
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
				public static class DeleteButton
				{
					public static class Foreground
					{
						[ResourceKeyDefinition(typeof(Brush), "TextBoxDeleteButtonForeground")]
						public static ResourceValue<Brush> Default => new("TextBoxDeleteButtonForeground", true);

						[ResourceKeyDefinition(typeof(Brush), "TextBoxDeleteButtonForegroundPointerOver")]
						public static ResourceValue<Brush> PointerOver => new("TextBoxDeleteButtonForegroundPointerOver", true);

						[ResourceKeyDefinition(typeof(Brush), "TextBoxDeleteButtonForegroundPressed")]
						public static ResourceValue<Brush> Pressed => new("TextBoxDeleteButtonForegroundPressed", true);

						[ResourceKeyDefinition(typeof(Brush), "TextBoxDeleteButtonForegroundDisabled")]
						public static ResourceValue<Brush> Disabled => new("TextBoxDeleteButtonForegroundDisabled", true);
					}
				}

				public static class Filled
				{
					public static class Foreground
					{
						[ResourceKeyDefinition(typeof(Brush), "FilledTextBoxForeground")]
						public static ResourceValue<Brush> Default => new("FilledTextBoxForeground", true);

						[ResourceKeyDefinition(typeof(Brush), "FilledTextBoxForegroundPointerOver")]
						public static ResourceValue<Brush> PointerOver => new("FilledTextBoxForegroundPointerOver", true);

						[ResourceKeyDefinition(typeof(Brush), "FilledTextBoxForegroundFocused")]
						public static ResourceValue<Brush> Focused => new("FilledTextBoxForegroundFocused", true);

						[ResourceKeyDefinition(typeof(Brush), "FilledTextBoxForegroundDisabled")]
						public static ResourceValue<Brush> Disabled => new("FilledTextBoxForegroundDisabled", true);

						[ResourceKeyDefinition(typeof(Brush), "FilledTextBoxPlaceholderForeground")]
						public static ResourceValue<Brush> Placeholder => new("FilledTextBoxPlaceholderForeground", true);

						[ResourceKeyDefinition(typeof(Brush), "FilledTextBoxPlaceholderForegroundPointerOver")]
						public static ResourceValue<Brush> PlaceholderPointerOver => new("FilledTextBoxPlaceholderForegroundPointerOver", true);

						[ResourceKeyDefinition(typeof(Brush), "FilledTextBoxPlaceholderForegroundFocused")]
						public static ResourceValue<Brush> PlaceholderFocused => new("FilledTextBoxPlaceholderForegroundFocused", true);

						[ResourceKeyDefinition(typeof(Brush), "FilledTextBoxPlaceholderForegroundDisabled")]
						public static ResourceValue<Brush> PlaceholderDisabled => new("FilledTextBoxPlaceholderForegroundDisabled", true);

						[ResourceKeyDefinition(typeof(Brush), "FilledTextBoxDeleteButtonForeground")]
						public static ResourceValue<Brush> DeleteButton => new("FilledTextBoxDeleteButtonForeground", true);

						[ResourceKeyDefinition(typeof(Brush), "FilledTextBoxDeleteButtonForegroundPointerOver")]
						public static ResourceValue<Brush> DeleteButtonPointerOver => new("FilledTextBoxDeleteButtonForegroundPointerOver", true);

						[ResourceKeyDefinition(typeof(Brush), "FilledTextBoxDeleteButtonForegroundFocused")]
						public static ResourceValue<Brush> DeleteButtonFocused => new("FilledTextBoxDeleteButtonForegroundFocused", true);

						[ResourceKeyDefinition(typeof(Brush), "FilledTextBoxDeleteButtonForegroundDisabled")]
						public static ResourceValue<Brush> DeleteButtonDisabled => new("FilledTextBoxDeleteButtonForegroundDisabled", true);
					}

					public static class Background
					{
						[ResourceKeyDefinition(typeof(Brush), "FilledTextBoxBackground")]
						public static ResourceValue<Brush> Default => new("FilledTextBoxBackground", true);

						[ResourceKeyDefinition(typeof(Brush), "FilledTextBoxBackgroundPointerOver")]
						public static ResourceValue<Brush> PointerOver => new("FilledTextBoxBackgroundPointerOver", true);

						[ResourceKeyDefinition(typeof(Brush), "FilledTextBoxBackgroundFocused")]
						public static ResourceValue<Brush> Focused => new("FilledTextBoxBackgroundFocused", true);

						[ResourceKeyDefinition(typeof(Brush), "FilledTextBoxBackgroundDisabled")]
						public static ResourceValue<Brush> Disabled => new("FilledTextBoxBackgroundDisabled", true);

					}

					public static class BorderBrush
					{
						[ResourceKeyDefinition(typeof(Brush), "FilledTextBoxBorderBrush")]
						public static ResourceValue<Brush> Default => new("FilledTextBoxBorderBrush", true);

						[ResourceKeyDefinition(typeof(Brush), "FilledTextBoxBorderBrushPointerOver")]
						public static ResourceValue<Brush> PointerOver => new("FilledTextBoxBorderBrushPointerOver", true);

						[ResourceKeyDefinition(typeof(Brush), "FilledTextBoxBorderBrushFocused")]
						public static ResourceValue<Brush> Focused => new("FilledTextBoxBorderBrushFocused", true);

						[ResourceKeyDefinition(typeof(Brush), "FilledTextBoxBorderBrushDisabled")]
						public static ResourceValue<Brush> Disabled => new("FilledTextBoxBorderBrushDisabled", true);
					}

					public static class BorderThickness
					{
						[ResourceKeyDefinition(typeof(Thickness), "FilledTextBoxBorderThicknessNormal")]
						public static ResourceValue<Thickness> Normal => new("FilledTextBoxBorderThicknessNormal", true);

						[ResourceKeyDefinition(typeof(Thickness), "FilledTextBoxBorderThicknessFocused")]
						public static ResourceValue<Thickness> Focused => new("FilledTextBoxBorderThicknessFocused", true);
					}

					public static class Typography
					{
						[ResourceKeyDefinition(typeof(FontFamily), "FilledTextBoxFontFamily")]
						public static ResourceValue<FontFamily> FontFamily => new("FilledTextBoxFontFamily", true);

						[ResourceKeyDefinition(typeof(double), "FilledTextBoxFontSize")]
						public static ResourceValue<double> FontSize => new("FilledTextBoxFontSize", true);
					}

					[ResourceKeyDefinition(typeof(FontWeight), "FilledTextBoxFontWeight")]
					public static ResourceValue<FontWeight> FontWeight => new("FilledTextBoxFontWeight", true);

					[ResourceKeyDefinition(typeof(int), "FilledTextBoxCharacterSpacing")]
					public static ResourceValue<int> CharacterSpacing => new("FilledTextBoxCharacterSpacing", true);
				}

				public static class Outlined
				{
					public static class Foreground
					{
						[ResourceKeyDefinition(typeof(Brush), "OutlinedTextBoxForeground")]
						public static ResourceValue<Brush> Default => new("OutlinedTextBoxForeground", true);

						[ResourceKeyDefinition(typeof(Brush), "OutlinedTextBoxForegroundPointerOver")]
						public static ResourceValue<Brush> PointerOver => new("OutlinedTextBoxForegroundPointerOver", true);

						[ResourceKeyDefinition(typeof(Brush), "OutlinedTextBoxForegroundFocused")]
						public static ResourceValue<Brush> Focused => new("OutlinedTextBoxForegroundFocused", true);

						[ResourceKeyDefinition(typeof(Brush), "OutlinedTextBoxForegroundDisabled")]
						public static ResourceValue<Brush> Disabled => new("OutlinedTextBoxForegroundDisabled", true);

						[ResourceKeyDefinition(typeof(Brush), "OutlinedTextBoxPlaceholderForeground")]
						public static ResourceValue<Brush> Placeholder => new("OutlinedTextBoxPlaceholderForeground", true);

						[ResourceKeyDefinition(typeof(Brush), "OutlinedTextBoxPlaceholderForegroundPointerOver")]
						public static ResourceValue<Brush> PlaceholderPointerOver => new("OutlinedTextBoxPlaceholderForegroundPointerOver", true);

						[ResourceKeyDefinition(typeof(Brush), "OutlinedTextBoxPlaceholderForegroundFocused")]
						public static ResourceValue<Brush> PlaceholderFocused => new("OutlinedTextBoxPlaceholderForegroundFocused", true);

						[ResourceKeyDefinition(typeof(Brush), "OutlinedTextBoxPlaceholderForegroundDisabled")]
						public static ResourceValue<Brush> PlaceholderDisabled => new("OutlinedTextBoxPlaceholderForegroundDisabled", true);
					}

					public static class BorderBrush
					{
						[ResourceKeyDefinition(typeof(Brush), "OutlinedTextBoxBorderBrush")]
						public static ResourceValue<Brush> Default => new("OutlinedTextBoxBorderBrush", true);

						[ResourceKeyDefinition(typeof(Brush), "OutlinedTextBoxBorderBrushPointerOver")]
						public static ResourceValue<Brush> PointerOver => new("OutlinedTextBoxBorderBrushPointerOver", true);

						[ResourceKeyDefinition(typeof(Brush), "OutlinedTextBoxBorderBrushFocused")]
						public static ResourceValue<Brush> Focused => new("OutlinedTextBoxBorderBrushFocused", true);

						[ResourceKeyDefinition(typeof(Brush), "OutlinedTextBoxBorderBrushDisabled")]
						public static ResourceValue<Brush> Disabled => new("OutlinedTextBoxBorderBrushDisabled", true);
					}

					public static class Typography
					{
						[ResourceKeyDefinition(typeof(FontFamily), "OutlinedTextBoxFontFamily")]
						public static ResourceValue<FontFamily> FontFamily => new("OutlinedTextBoxFontFamily", true);


						[ResourceKeyDefinition(typeof(double), "OutlinedTextBoxFontSize")]
						public static ResourceValue<double> FontSize => new("OutlinedTextBoxFontSize", true);
					}

					[ResourceKeyDefinition(typeof(FontWeight), "OutlinedTextBoxFontWeight")]
					public static ResourceValue<FontWeight> FontWeight => new("OutlinedTextBoxFontWeight", true);

					[ResourceKeyDefinition(typeof(int), "OutlinedTextBoxCharacterSpacing")]
					public static ResourceValue<int> CharacterSpacing => new("OutlinedTextBoxCharacterSpacing", true);
				}
			}

			public static class Styles
			{
				[ResourceKeyDefinition(typeof(Style), "FilledTextBoxStyle", TargetType = typeof(TextBox))]
				public static ResourceValue<Style> Filled => new("FilledTextBoxStyle");

				[ResourceKeyDefinition(typeof(Style), "OutlinedTextBoxStyle", TargetType = typeof(TextBox))]
				public static ResourceValue<Style> Outlined => new("OutlinedTextBoxStyle");
			}
		}
	}
}
