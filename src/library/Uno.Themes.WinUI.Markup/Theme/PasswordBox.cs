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
	public static partial class PasswordBox
	{
		public static partial class Resources
		{
			public static partial class Filled
			{
				public static partial class Background
				{
					[ResourceKeyDefinition(typeof(Brush), "FilledPasswordBoxBackground")]
					public static ThemeResourceKey<Brush> Default => new("FilledPasswordBoxBackground");

					[ResourceKeyDefinition(typeof(Brush), "FilledPasswordBoxBackgroundPointerOver")]
					public static ThemeResourceKey<Brush> PointerOver => new("FilledPasswordBoxBackgroundPointerOver");

					[ResourceKeyDefinition(typeof(Brush), "FilledPasswordBoxBackgroundDisabled")]
					public static ThemeResourceKey<Brush> Disabled => new("FilledPasswordBoxBackgroundDisabled");

					[ResourceKeyDefinition(typeof(Brush), "FilledPasswordBoxBackgroundFocused")]
					public static ThemeResourceKey<Brush> Focused => new("FilledPasswordBoxBackgroundFocused");
				}

				public static partial class BorderBrush
				{
					[ResourceKeyDefinition(typeof(Brush), "FilledPasswordBoxBorderBrushFocused")]
					public static ThemeResourceKey<Brush> Default => new("FilledPasswordBoxBorderBrushFocused");

					[ResourceKeyDefinition(typeof(Brush), "FilledPasswordBoxBorderBrushPointerOver")]
					public static ThemeResourceKey<Brush> PointerOver => new("FilledPasswordBoxBorderBrushPointerOver");

					[ResourceKeyDefinition(typeof(Brush), "FilledPasswordBoxBorderBrushDisabled")]
					public static ThemeResourceKey<Brush> Disabled => new("FilledPasswordBoxBorderBrushDisabled");
				}

				public static partial class BorderHeight
				{
					[ResourceKeyDefinition(typeof(double), "FilledPasswordBoxBorderHeightPointerOver")]
					public static ThemeResourceKey<double> PointerOver => new("FilledPasswordBoxBorderHeightPointerOver");

					[ResourceKeyDefinition(typeof(double), "FilledPasswordBoxBorderHeightFocused")]
					public static ThemeResourceKey<double> Focused => new("FilledPasswordBoxBorderHeightFocused");
				}

				public static partial class Foreground
				{
					[ResourceKeyDefinition(typeof(Brush), "FilledPasswordBoxForeground")]
					public static ThemeResourceKey<Brush> Default => new("FilledPasswordBoxForeground");

					[ResourceKeyDefinition(typeof(Brush), "FilledPasswordBoxForegroundPointerOver")]
					public static ThemeResourceKey<Brush> PointerOver => new("FilledPasswordBoxForegroundPointerOver");

					[ResourceKeyDefinition(typeof(Brush), "FilledPasswordBoxForegroundDisabled")]
					public static ThemeResourceKey<Brush> Disabled => new("FilledPasswordBoxForegroundDisabled");

					[ResourceKeyDefinition(typeof(Brush), "FilledPasswordBoxForegroundFocused")]
					public static ThemeResourceKey<Brush> Focused => new("FilledPasswordBoxForegroundFocused");
				}

				public static partial class LeadingIconForeground
				{
					[ResourceKeyDefinition(typeof(Brush), "PasswordBoxLeadingIconForeground")]
					public static ThemeResourceKey<Brush> Default => new("PasswordBoxLeadingIconForeground");

					[ResourceKeyDefinition(typeof(Brush), "PasswordBoxLeadingIconForegroundDisabled")]
					public static ThemeResourceKey<Brush> Disabled => new("PasswordBoxLeadingIconForegroundDisabled");
				}

				public static partial class PlaceholderForeground
				{
					[ResourceKeyDefinition(typeof(Brush), "FilledPasswordBoxPlaceholderForeground")]
					public static ThemeResourceKey<Brush> Default => new("FilledPasswordBoxPlaceholderForeground");

					[ResourceKeyDefinition(typeof(Brush), "FilledPasswordBoxPlaceholderForegroundPointerOver")]
					public static ThemeResourceKey<Brush> PointerOver => new("FilledPasswordBoxPlaceholderForegroundPointerOver");

					[ResourceKeyDefinition(typeof(Brush), "FilledPasswordBoxPlaceholderForegroundDisabled")]
					public static ThemeResourceKey<Brush> Disabled => new("FilledPasswordBoxPlaceholderForegroundDisabled");

					[ResourceKeyDefinition(typeof(Brush), "FilledPasswordBoxPlaceholderForegroundFocused")]
					public static ThemeResourceKey<Brush> Focused => new("FilledPasswordBoxPlaceholderForegroundFocused");
				}

				public static partial class Typography
				{
					[ResourceKeyDefinition(typeof(int), "FilledPasswordBoxCharacterSpacing")]
					public static ThemeResourceKey<int> CharacterSpacing => new("FilledPasswordBoxCharacterSpacing");

					[ResourceKeyDefinition(typeof(FontFamily), "FilledPasswordBoxFontFamily")]
					public static ThemeResourceKey<FontFamily> FontFamily => new("FilledPasswordBoxFontFamily");

					[ResourceKeyDefinition(typeof(double), "FilledPasswordBoxFontSize")]
					public static ThemeResourceKey<double> FontSize => new("FilledPasswordBoxFontSize");

					[ResourceKeyDefinition(typeof(string), "FilledPasswordBoxFontWeight")]
					public static ThemeResourceKey<string> FontWeight => new("FilledPasswordBoxFontWeight");
				}

				[ResourceKeyDefinition(typeof(CornerRadius), "FilledPasswordBoxCornerRadius")]
				public static ThemeResourceKey<CornerRadius> CornerRadius => new("FilledPasswordBoxCornerRadius");

				[ResourceKeyDefinition(typeof(double), "FilledPasswordBoxForegroundOpacityDisabled")]
				public static ThemeResourceKey<double> ForegroundOpacityDisabled => new("FilledPasswordBoxForegroundOpacityDisabled");

				[ResourceKeyDefinition(typeof(double), "FilledPasswordBoxMinHeight")]
				public static ThemeResourceKey<double> MinHeight => new("FilledPasswordBoxMinHeight");

				[ResourceKeyDefinition(typeof(Thickness), "FilledPasswordBoxPadding")]
				public static ThemeResourceKey<Thickness> Padding => new("FilledPasswordBoxPadding");

				[ResourceKeyDefinition(typeof(Brush), "FilledPasswordBoxRevealButtonForeground")]
				public static ThemeResourceKey<Brush> RevealButtonForeground => new("FilledPasswordBoxRevealButtonForeground");
			}

			public static partial class Outlined
			{
				public static partial class Background
				{
					[ResourceKeyDefinition(typeof(Brush), "OutlinedPasswordBoxBackground")]
					public static ThemeResourceKey<Brush> Default => new("OutlinedPasswordBoxBackground");

					[ResourceKeyDefinition(typeof(Brush), "OutlinedPasswordBoxBackgroundPointerOver")]
					public static ThemeResourceKey<Brush> PointerOver => new("OutlinedPasswordBoxBackgroundPointerOver");

					[ResourceKeyDefinition(typeof(Brush), "OutlinedPasswordBoxBackgroundDisabled")]
					public static ThemeResourceKey<Brush> Disabled => new("OutlinedPasswordBoxBackgroundDisabled");

					[ResourceKeyDefinition(typeof(Brush), "OutlinedPasswordBoxBackgroundFocused")]
					public static ThemeResourceKey<Brush> Focused => new("OutlinedPasswordBoxBackgroundFocused");
				}

				public static partial class BorderBrush
				{
					[ResourceKeyDefinition(typeof(Brush), "OutlinedPasswordBoxBorderBrush")]
					public static ThemeResourceKey<Brush> Default => new("OutlinedPasswordBoxBorderBrush");

					[ResourceKeyDefinition(typeof(Brush), "OutlinedPasswordBoxBorderBrushPointerOver")]
					public static ThemeResourceKey<Brush> PointerOver => new("OutlinedPasswordBoxBorderBrushPointerOver");

					[ResourceKeyDefinition(typeof(Brush), "OutlinedPasswordBoxBorderBrushDisabled")]
					public static ThemeResourceKey<Brush> Disabled => new("OutlinedPasswordBoxBorderBrushDisabled");

					[ResourceKeyDefinition(typeof(Brush), "OutlinedPasswordBoxBorderBrushFocused")]
					public static ThemeResourceKey<Brush> Focused => new("OutlinedPasswordBoxBorderBrushFocused");
				}

				public static partial class BorderPadding
				{
					[ResourceKeyDefinition(typeof(Thickness), "OutlinedPasswordBoxBorderPadding")]
					public static ThemeResourceKey<Thickness> Default => new("OutlinedPasswordBoxBorderPadding");

					[ResourceKeyDefinition(typeof(Thickness), "OutlinedPasswordBoxBorderPaddingPointerOver")]
					public static ThemeResourceKey<Thickness> PointerOver => new("OutlinedPasswordBoxBorderPaddingPointerOver");

					[ResourceKeyDefinition(typeof(Thickness), "OutlinedPasswordBoxBorderPaddingFocused")]
					public static ThemeResourceKey<Thickness> Focused => new("OutlinedPasswordBoxBorderPaddingFocused");
				}

				public static partial class BorderThickness
				{
					[ResourceKeyDefinition(typeof(Thickness), "OutlinedPasswordBoxBorderThickness")]
					public static ThemeResourceKey<Thickness> Default => new("OutlinedPasswordBoxBorderThickness");

					[ResourceKeyDefinition(typeof(Thickness), "OutlinedPasswordBoxBorderThicknessPointerOver")]
					public static ThemeResourceKey<Thickness> PointerOver => new("OutlinedPasswordBoxBorderThicknessPointerOver");

					[ResourceKeyDefinition(typeof(Thickness), "OutlinedPasswordBoxBorderThicknessFocused")]
					public static ThemeResourceKey<Thickness> Focused => new("OutlinedPasswordBoxBorderThicknessFocused");
				}

				public static partial class Foreground
				{
					[ResourceKeyDefinition(typeof(Brush), "OutlinedPasswordBoxForeground")]
					public static ThemeResourceKey<Brush> Default => new("OutlinedPasswordBoxForeground");

					[ResourceKeyDefinition(typeof(Brush), "OutlinedPasswordBoxForegroundPointerOver")]
					public static ThemeResourceKey<Brush> PointerOver => new("OutlinedPasswordBoxForegroundPointerOver");

					[ResourceKeyDefinition(typeof(Brush), "OutlinedPasswordBoxForegroundDisabled")]
					public static ThemeResourceKey<Brush> Disabled => new("OutlinedPasswordBoxForegroundDisabled");

					[ResourceKeyDefinition(typeof(Brush), "OutlinedPasswordBoxForegroundFocused")]
					public static ThemeResourceKey<Brush> Focused => new("OutlinedPasswordBoxForegroundFocused");
				}

				public static partial class LeadingIconForeground
				{
					[ResourceKeyDefinition(typeof(Brush), "PasswordBoxLeadingIconForeground")]
					public static ThemeResourceKey<Brush> Default => new("PasswordBoxLeadingIconForeground");

					[ResourceKeyDefinition(typeof(Brush), "PasswordBoxLeadingIconForegroundDisabled")]
					public static ThemeResourceKey<Brush> Disabled => new("PasswordBoxLeadingIconForegroundDisabled");
				}

				public static partial class PlaceholderForeground
				{
					[ResourceKeyDefinition(typeof(Brush), "OutlinedPasswordBoxPlaceholderForeground")]
					public static ThemeResourceKey<Brush> Default => new("OutlinedPasswordBoxPlaceholderForeground");

					[ResourceKeyDefinition(typeof(Brush), "OutlinedPasswordBoxPlaceholderForegroundPointerOver")]
					public static ThemeResourceKey<Brush> PointerOver => new("OutlinedPasswordBoxPlaceholderForegroundPointerOver");

					[ResourceKeyDefinition(typeof(Brush), "OutlinedPasswordBoxPlaceholderForegroundDisabled")]
					public static ThemeResourceKey<Brush> Disabled => new("OutlinedPasswordBoxPlaceholderForegroundDisabled");

					[ResourceKeyDefinition(typeof(Brush), "OutlinedPasswordBoxPlaceholderForegroundFocused")]
					public static ThemeResourceKey<Brush> Focused => new("OutlinedPasswordBoxPlaceholderForegroundFocused");
				}

				public static partial class Typography
				{
					[ResourceKeyDefinition(typeof(int), "OutlinedPasswordBoxCharacterSpacing")]
					public static ThemeResourceKey<int> CharacterSpacing => new("OutlinedPasswordBoxCharacterSpacing");

					[ResourceKeyDefinition(typeof(FontFamily), "OutlinedPasswordBoxFontFamily")]
					public static ThemeResourceKey<FontFamily> FontFamily => new("OutlinedPasswordBoxFontFamily");

					[ResourceKeyDefinition(typeof(double), "OutlinedPasswordBoxFontSize")]
					public static ThemeResourceKey<double> FontSize => new("OutlinedPasswordBoxFontSize");

					[ResourceKeyDefinition(typeof(string), "OutlinedPasswordBoxFontWeight")]
					public static ThemeResourceKey<string> FontWeight => new("OutlinedPasswordBoxFontWeight");
				}

				[ResourceKeyDefinition(typeof(CornerRadius), "OutlinedPasswordBoxCornerRadius")]
				public static ThemeResourceKey<CornerRadius> CornerRadius => new("OutlinedPasswordBoxCornerRadius");

				[ResourceKeyDefinition(typeof(double), "OutlinedPasswordBoxForegroundOpacityDisabled")]
				public static ThemeResourceKey<double> ForegroundOpacityDisabled => new("OutlinedPasswordBoxForegroundOpacityDisabled");

				[ResourceKeyDefinition(typeof(double), "OutlinedPasswordBoxMinHeight")]
				public static ThemeResourceKey<double> MinHeight => new("OutlinedPasswordBoxMinHeight");

				[ResourceKeyDefinition(typeof(Thickness), "OutlinedPasswordBoxPadding")]
				public static ThemeResourceKey<Thickness> Padding => new("OutlinedPasswordBoxPadding");
			}
		}

		public static partial class Styles
		{
			[ResourceKeyDefinition(typeof(Style), "MaterialFilledPasswordBoxStyle", TargetType = typeof(global::Microsoft.UI.Xaml.Controls.PasswordBox))]
			public static StaticResourceKey<Style> Filled => new("MaterialFilledPasswordBoxStyle");

			[ResourceKeyDefinition(typeof(Style), "MaterialOutlinedPasswordBoxStyle", TargetType = typeof(global::Microsoft.UI.Xaml.Controls.PasswordBox))]
			public static StaticResourceKey<Style> Outlined => new("MaterialOutlinedPasswordBoxStyle");
		}
	}
}
