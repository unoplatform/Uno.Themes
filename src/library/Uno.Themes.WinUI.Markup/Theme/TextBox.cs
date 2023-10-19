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
	public static partial class TextBox
	{
		public static partial class Resources
		{
			public static partial class Filled
			{
				public static partial class Background
				{
					[ResourceKeyDefinition(typeof(Brush), "FilledTextBoxBackground")]
					public static ThemeResourceKey<Brush> Default => new("FilledTextBoxBackground");

					[ResourceKeyDefinition(typeof(Brush), "FilledTextBoxBackgroundPointerOver")]
					public static ThemeResourceKey<Brush> PointerOver => new("FilledTextBoxBackgroundPointerOver");

					[ResourceKeyDefinition(typeof(Brush), "FilledTextBoxBackgroundDisabled")]
					public static ThemeResourceKey<Brush> Disabled => new("FilledTextBoxBackgroundDisabled");

					[ResourceKeyDefinition(typeof(Brush), "FilledTextBoxBackgroundFocused")]
					public static ThemeResourceKey<Brush> Focused => new("FilledTextBoxBackgroundFocused");
				}

				public static partial class BorderBrush
				{
					[ResourceKeyDefinition(typeof(Brush), "FilledTextBoxBorderBrushFocused")]
					public static ThemeResourceKey<Brush> Default => new("FilledTextBoxBorderBrushFocused");

					[ResourceKeyDefinition(typeof(Brush), "FilledTextBoxBorderBrushPointerOver")]
					public static ThemeResourceKey<Brush> PointerOver => new("FilledTextBoxBorderBrushPointerOver");

					[ResourceKeyDefinition(typeof(Brush), "FilledTextBoxBorderBrushDisabled")]
					public static ThemeResourceKey<Brush> Disabled => new("FilledTextBoxBorderBrushDisabled");
				}

				public static partial class BorderThicknessNormal
				{
					[ResourceKeyDefinition(typeof(double), "FilledTextBoxBorderThicknessNormal")]
					public static ThemeResourceKey<double> Default => new("FilledTextBoxBorderThicknessNormal");

					[ResourceKeyDefinition(typeof(double), "MaterialFilledTextBoxBorderHeightFocused")]
					public static ThemeResourceKey<double> Focused => new("MaterialFilledTextBoxBorderHeightFocused");
				}

				public static partial class DeleteButtonForeground
				{
					[ResourceKeyDefinition(typeof(Brush), "FilledTextBoxDeleteButtonForeground")]
					public static ThemeResourceKey<Brush> Default => new("FilledTextBoxDeleteButtonForeground");

					[ResourceKeyDefinition(typeof(Brush), "FilledTextBoxDeleteButtonForegroundPointerOver")]
					public static ThemeResourceKey<Brush> PointerOver => new("FilledTextBoxDeleteButtonForegroundPointerOver");

					[ResourceKeyDefinition(typeof(Brush), "FilledTextBoxDeleteButtonForegroundDisabled")]
					public static ThemeResourceKey<Brush> Disabled => new("FilledTextBoxDeleteButtonForegroundDisabled");

					[ResourceKeyDefinition(typeof(Brush), "FilledTextBoxDeleteButtonForegroundFocused")]
					public static ThemeResourceKey<Brush> Focused => new("FilledTextBoxDeleteButtonForegroundFocused");
				}

				public static partial class Foreground
				{
					[ResourceKeyDefinition(typeof(Brush), "FilledTextBoxForeground")]
					public static ThemeResourceKey<Brush> Default => new("FilledTextBoxForeground");

					[ResourceKeyDefinition(typeof(Brush), "FilledTextBoxForegroundPointerOver")]
					public static ThemeResourceKey<Brush> PointerOver => new("FilledTextBoxForegroundPointerOver");

					[ResourceKeyDefinition(typeof(Brush), "FilledTextBoxForegroundDisabled")]
					public static ThemeResourceKey<Brush> Disabled => new("FilledTextBoxForegroundDisabled");

					[ResourceKeyDefinition(typeof(Brush), "FilledTextBoxForegroundFocused")]
					public static ThemeResourceKey<Brush> Focused => new("FilledTextBoxForegroundFocused");
				}

				public static partial class LeadingIconForeground
				{
					[ResourceKeyDefinition(typeof(Brush), "TextBoxLeadingIconForeground")]
					public static ThemeResourceKey<Brush> Default => new("TextBoxLeadingIconForeground");

					[ResourceKeyDefinition(typeof(Brush), "TextBoxLeadingIconForegroundDisabled")]
					public static ThemeResourceKey<Brush> Disabled => new("TextBoxLeadingIconForegroundDisabled");
				}

				public static partial class PlaceholderForeground
				{
					[ResourceKeyDefinition(typeof(Brush), "FilledTextBoxPlaceholderForeground")]
					public static ThemeResourceKey<Brush> Default => new("FilledTextBoxPlaceholderForeground");

					[ResourceKeyDefinition(typeof(Brush), "FilledTextBoxPlaceholderForegroundPointerOver")]
					public static ThemeResourceKey<Brush> PointerOver => new("FilledTextBoxPlaceholderForegroundPointerOver");

					[ResourceKeyDefinition(typeof(Brush), "FilledTextBoxPlaceholderForegroundDisabled")]
					public static ThemeResourceKey<Brush> Disabled => new("FilledTextBoxPlaceholderForegroundDisabled");

					[ResourceKeyDefinition(typeof(Brush), "FilledTextBoxPlaceholderForegroundFocused")]
					public static ThemeResourceKey<Brush> Focused => new("FilledTextBoxPlaceholderForegroundFocused");
				}

				public static partial class Typography
				{
					[ResourceKeyDefinition(typeof(int), "FilledTextBoxCharacterSpacing")]
					public static ThemeResourceKey<int> CharacterSpacing => new("FilledTextBoxCharacterSpacing");

					[ResourceKeyDefinition(typeof(FontFamily), "FilledTextBoxFontFamily")]
					public static ThemeResourceKey<FontFamily> FontFamily => new("FilledTextBoxFontFamily");

					[ResourceKeyDefinition(typeof(double), "FilledTextBoxFontSize")]
					public static ThemeResourceKey<double> FontSize => new("FilledTextBoxFontSize");

					[ResourceKeyDefinition(typeof(string), "FilledTextBoxFontWeight")]
					public static ThemeResourceKey<string> FontWeight => new("FilledTextBoxFontWeight");
				}

				[ResourceKeyDefinition(typeof(double), "FilledTextBoxBorderThicknessFocused")]
				public static ThemeResourceKey<double> BorderThicknessFocused => new("FilledTextBoxBorderThicknessFocused");

				[ResourceKeyDefinition(typeof(CornerRadius), "MaterialFilledTextBoxCornerRadius")]
				public static ThemeResourceKey<CornerRadius> CornerRadius => new("MaterialFilledTextBoxCornerRadius");

				[ResourceKeyDefinition(typeof(double), "FilledTextBoxForegroundOpacityDisabled")]
				public static ThemeResourceKey<double> ForegroundOpacityDisabled => new("FilledTextBoxForegroundOpacityDisabled");

				[ResourceKeyDefinition(typeof(double), "MaterialFilledTextBoxMinHeight")]
				public static ThemeResourceKey<double> MinHeight => new("MaterialFilledTextBoxMinHeight");

				[ResourceKeyDefinition(typeof(Thickness), "MaterialFilledTextBoxPadding")]
				public static ThemeResourceKey<Thickness> Padding => new("MaterialFilledTextBoxPadding");
			}

			public static partial class Outlined
			{
				public static partial class BorderBrush
				{
					[ResourceKeyDefinition(typeof(Brush), "OutlinedTextBoxBorderBrush")]
					public static ThemeResourceKey<Brush> Default => new("OutlinedTextBoxBorderBrush");

					[ResourceKeyDefinition(typeof(Brush), "OutlinedTextBoxBorderBrushPointerOver")]
					public static ThemeResourceKey<Brush> PointerOver => new("OutlinedTextBoxBorderBrushPointerOver");

					[ResourceKeyDefinition(typeof(Brush), "OutlinedTextBoxBorderBrushDisabled")]
					public static ThemeResourceKey<Brush> Disabled => new("OutlinedTextBoxBorderBrushDisabled");

					[ResourceKeyDefinition(typeof(Brush), "OutlinedTextBoxBorderBrushFocused")]
					public static ThemeResourceKey<Brush> Focused => new("OutlinedTextBoxBorderBrushFocused");
				}

				public static partial class Foreground
				{
					[ResourceKeyDefinition(typeof(Brush), "OutlinedTextBoxForeground")]
					public static ThemeResourceKey<Brush> Default => new("OutlinedTextBoxForeground");

					[ResourceKeyDefinition(typeof(Brush), "OutlinedTextBoxForegroundPointerOver")]
					public static ThemeResourceKey<Brush> PointerOver => new("OutlinedTextBoxForegroundPointerOver");

					[ResourceKeyDefinition(typeof(Brush), "OutlinedTextBoxForegroundDisabled")]
					public static ThemeResourceKey<Brush> Disabled => new("OutlinedTextBoxForegroundDisabled");

					[ResourceKeyDefinition(typeof(Brush), "OutlinedTextBoxForegroundFocused")]
					public static ThemeResourceKey<Brush> Focused => new("OutlinedTextBoxForegroundFocused");
				}

				public static partial class LeadingIconForeground
				{
					[ResourceKeyDefinition(typeof(Brush), "TextBoxLeadingIconForeground")]
					public static ThemeResourceKey<Brush> Default => new("TextBoxLeadingIconForeground");

					[ResourceKeyDefinition(typeof(Brush), "TextBoxLeadingIconForegroundDisabled")]
					public static ThemeResourceKey<Brush> Disabled => new("TextBoxLeadingIconForegroundDisabled");
				}

				public static partial class PlaceholderForeground
				{
					[ResourceKeyDefinition(typeof(Brush), "OutlinedTextBoxPlaceholderForeground")]
					public static ThemeResourceKey<Brush> Default => new("OutlinedTextBoxPlaceholderForeground");

					[ResourceKeyDefinition(typeof(Brush), "OutlinedTextBoxPlaceholderForegroundPointerOver")]
					public static ThemeResourceKey<Brush> PointerOver => new("OutlinedTextBoxPlaceholderForegroundPointerOver");

					[ResourceKeyDefinition(typeof(Brush), "OutlinedTextBoxPlaceholderForegroundDisabled")]
					public static ThemeResourceKey<Brush> Disabled => new("OutlinedTextBoxPlaceholderForegroundDisabled");

					[ResourceKeyDefinition(typeof(Brush), "OutlinedTextBoxPlaceholderForegroundFocused")]
					public static ThemeResourceKey<Brush> Focused => new("OutlinedTextBoxPlaceholderForegroundFocused");
				}

				public static partial class Typography
				{
					[ResourceKeyDefinition(typeof(int), "OutlinedTextBoxCharacterSpacing")]
					public static ThemeResourceKey<int> CharacterSpacing => new("OutlinedTextBoxCharacterSpacing");

					[ResourceKeyDefinition(typeof(FontFamily), "OutlinedTextBoxFontFamily")]
					public static ThemeResourceKey<FontFamily> FontFamily => new("OutlinedTextBoxFontFamily");

					[ResourceKeyDefinition(typeof(double), "OutlinedTextBoxFontSize")]
					public static ThemeResourceKey<double> FontSize => new("OutlinedTextBoxFontSize");

					[ResourceKeyDefinition(typeof(string), "OutlinedTextBoxFontWeight")]
					public static ThemeResourceKey<string> FontWeight => new("OutlinedTextBoxFontWeight");
				}

				[ResourceKeyDefinition(typeof(Thickness), "MaterialOutlinedTextBoxBorderPadding")]
				public static ThemeResourceKey<Thickness> BorderPadding => new("MaterialOutlinedTextBoxBorderPadding");

				[ResourceKeyDefinition(typeof(double), "MaterialOutlinedTextBoxBorderThickness")]
				public static ThemeResourceKey<double> BorderThickness => new("MaterialOutlinedTextBoxBorderThickness");

				[ResourceKeyDefinition(typeof(double), "MaterialOutlinedTextBoxBorderThicknessFocused")]
				public static ThemeResourceKey<double> BorderThicknessFocused => new("MaterialOutlinedTextBoxBorderThicknessFocused");

				[ResourceKeyDefinition(typeof(CornerRadius), "MaterialOutlinedTextBoxCornerRadius")]
				public static ThemeResourceKey<CornerRadius> CornerRadius => new("MaterialOutlinedTextBoxCornerRadius");

				[ResourceKeyDefinition(typeof(double), "OutlinedTextBoxForegroundOpacityDisabled")]
				public static ThemeResourceKey<double> ForegroundOpacityDisabled => new("OutlinedTextBoxForegroundOpacityDisabled");

				[ResourceKeyDefinition(typeof(double), "MaterialOutlinedTextBoxMinHeight")]
				public static ThemeResourceKey<double> MinHeight => new("MaterialOutlinedTextBoxMinHeight");

				[ResourceKeyDefinition(typeof(Thickness), "MaterialOutlinedTextBoxPadding")]
				public static ThemeResourceKey<Thickness> Padding => new("MaterialOutlinedTextBoxPadding");
			}
		}

		public static partial class Styles
		{
			[ResourceKeyDefinition(typeof(Style), "MaterialFilledTextBoxStyle", TargetType = typeof(global::Microsoft.UI.Xaml.Controls.TextBox))]
			public static StaticResourceKey<Style> Filled => new("MaterialFilledTextBoxStyle");

			[ResourceKeyDefinition(typeof(Style), "MaterialOutlinedTextBoxStyle", TargetType = typeof(global::Microsoft.UI.Xaml.Controls.TextBox))]
			public static StaticResourceKey<Style> Outlined => new("MaterialOutlinedTextBoxStyle");
		}
	}
}
