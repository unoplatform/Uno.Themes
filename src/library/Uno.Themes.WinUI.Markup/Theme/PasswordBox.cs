using System;
using Windows.UI;
using Microsoft.UI.Text;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Media;
using Uno.Extensions.Markup;
using Uno.Extensions.Markup.Internals;

namespace Uno.Themes.Markup
{
	public static partial class Theme
	{
		public static partial class PasswordBox
		{
			public static partial class Resources
			{
				public static partial class Filled
				{
					public static readonly BackgroundVSG Background = new();
					public partial class BackgroundVSG
					{
						[ResourceKeyDefinition(typeof(Brush), "FilledPasswordBoxBackground")]
						public ThemeResourceKey<Brush> Default = new("FilledPasswordBoxBackground");

						[ResourceKeyDefinition(typeof(Brush), "FilledPasswordBoxBackgroundPointerOver")]
						public ThemeResourceKey<Brush> PointerOver = new("FilledPasswordBoxBackgroundPointerOver");

						[ResourceKeyDefinition(typeof(Brush), "FilledPasswordBoxBackgroundDisabled")]
						public ThemeResourceKey<Brush> Disabled = new("FilledPasswordBoxBackgroundDisabled");

						[ResourceKeyDefinition(typeof(Brush), "FilledPasswordBoxBackgroundFocused")]
						public ThemeResourceKey<Brush> Focused = new("FilledPasswordBoxBackgroundFocused");

						public static implicit operator ThemeResourceKey<Brush>(BackgroundVSG self) => self.Default;
					}

					public static readonly BorderBrushVSG BorderBrush = new();
					public partial class BorderBrushVSG
					{
						[ResourceKeyDefinition(typeof(Brush), "FilledPasswordBoxBorderBrushFocused")]
						public ThemeResourceKey<Brush> Default = new("FilledPasswordBoxBorderBrushFocused");

						[ResourceKeyDefinition(typeof(Brush), "FilledPasswordBoxBorderBrushPointerOver")]
						public ThemeResourceKey<Brush> PointerOver = new("FilledPasswordBoxBorderBrushPointerOver");

						[ResourceKeyDefinition(typeof(Brush), "FilledPasswordBoxBorderBrushDisabled")]
						public ThemeResourceKey<Brush> Disabled = new("FilledPasswordBoxBorderBrushDisabled");

						public static implicit operator ThemeResourceKey<Brush>(BorderBrushVSG self) => self.Default;
					}

					public static partial class BorderHeight
					{
						[ResourceKeyDefinition(typeof(double), "FilledPasswordBoxBorderHeightPointerOver")]
						public static ThemeResourceKey<double> PointerOver => new("FilledPasswordBoxBorderHeightPointerOver");

						[ResourceKeyDefinition(typeof(double), "FilledPasswordBoxBorderHeightFocused")]
						public static ThemeResourceKey<double> Focused => new("FilledPasswordBoxBorderHeightFocused");
					}

					public static readonly ForegroundVSG Foreground = new();
					public partial class ForegroundVSG
					{
						[ResourceKeyDefinition(typeof(Brush), "FilledPasswordBoxForeground")]
						public ThemeResourceKey<Brush> Default = new("FilledPasswordBoxForeground");

						[ResourceKeyDefinition(typeof(Brush), "FilledPasswordBoxForegroundPointerOver")]
						public ThemeResourceKey<Brush> PointerOver = new("FilledPasswordBoxForegroundPointerOver");

						[ResourceKeyDefinition(typeof(Brush), "FilledPasswordBoxForegroundDisabled")]
						public ThemeResourceKey<Brush> Disabled = new("FilledPasswordBoxForegroundDisabled");

						[ResourceKeyDefinition(typeof(Brush), "FilledPasswordBoxForegroundFocused")]
						public ThemeResourceKey<Brush> Focused = new("FilledPasswordBoxForegroundFocused");

						public static implicit operator ThemeResourceKey<Brush>(ForegroundVSG self) => self.Default;
					}

					public static readonly LeadingIconForegroundVSG LeadingIconForeground = new();
					public partial class LeadingIconForegroundVSG
					{
						[ResourceKeyDefinition(typeof(Brush), "PasswordBoxLeadingIconForeground")]
						public ThemeResourceKey<Brush> Default = new("PasswordBoxLeadingIconForeground");

						[ResourceKeyDefinition(typeof(Brush), "PasswordBoxLeadingIconForegroundDisabled")]
						public ThemeResourceKey<Brush> Disabled = new("PasswordBoxLeadingIconForegroundDisabled");

						public static implicit operator ThemeResourceKey<Brush>(LeadingIconForegroundVSG self) => self.Default;
					}

					public static readonly PlaceholderForegroundVSG PlaceholderForeground = new();
					public partial class PlaceholderForegroundVSG
					{
						[ResourceKeyDefinition(typeof(Brush), "FilledPasswordBoxPlaceholderForeground")]
						public ThemeResourceKey<Brush> Default = new("FilledPasswordBoxPlaceholderForeground");

						[ResourceKeyDefinition(typeof(Brush), "FilledPasswordBoxPlaceholderForegroundPointerOver")]
						public ThemeResourceKey<Brush> PointerOver = new("FilledPasswordBoxPlaceholderForegroundPointerOver");

						[ResourceKeyDefinition(typeof(Brush), "FilledPasswordBoxPlaceholderForegroundDisabled")]
						public ThemeResourceKey<Brush> Disabled = new("FilledPasswordBoxPlaceholderForegroundDisabled");

						[ResourceKeyDefinition(typeof(Brush), "FilledPasswordBoxPlaceholderForegroundFocused")]
						public ThemeResourceKey<Brush> Focused = new("FilledPasswordBoxPlaceholderForegroundFocused");

						public static implicit operator ThemeResourceKey<Brush>(PlaceholderForegroundVSG self) => self.Default;
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
					public static readonly BackgroundVSG Background = new();
					public partial class BackgroundVSG
					{
						[ResourceKeyDefinition(typeof(Brush), "OutlinedPasswordBoxBackground")]
						public ThemeResourceKey<Brush> Default = new("OutlinedPasswordBoxBackground");

						[ResourceKeyDefinition(typeof(Brush), "OutlinedPasswordBoxBackgroundPointerOver")]
						public ThemeResourceKey<Brush> PointerOver = new("OutlinedPasswordBoxBackgroundPointerOver");

						[ResourceKeyDefinition(typeof(Brush), "OutlinedPasswordBoxBackgroundDisabled")]
						public ThemeResourceKey<Brush> Disabled = new("OutlinedPasswordBoxBackgroundDisabled");

						[ResourceKeyDefinition(typeof(Brush), "OutlinedPasswordBoxBackgroundFocused")]
						public ThemeResourceKey<Brush> Focused = new("OutlinedPasswordBoxBackgroundFocused");

						public static implicit operator ThemeResourceKey<Brush>(BackgroundVSG self) => self.Default;
					}

					public static readonly BorderBrushVSG BorderBrush = new();
					public partial class BorderBrushVSG
					{
						[ResourceKeyDefinition(typeof(Brush), "OutlinedPasswordBoxBorderBrush")]
						public ThemeResourceKey<Brush> Default = new("OutlinedPasswordBoxBorderBrush");

						[ResourceKeyDefinition(typeof(Brush), "OutlinedPasswordBoxBorderBrushPointerOver")]
						public ThemeResourceKey<Brush> PointerOver = new("OutlinedPasswordBoxBorderBrushPointerOver");

						[ResourceKeyDefinition(typeof(Brush), "OutlinedPasswordBoxBorderBrushDisabled")]
						public ThemeResourceKey<Brush> Disabled = new("OutlinedPasswordBoxBorderBrushDisabled");

						[ResourceKeyDefinition(typeof(Brush), "OutlinedPasswordBoxBorderBrushFocused")]
						public ThemeResourceKey<Brush> Focused = new("OutlinedPasswordBoxBorderBrushFocused");

						public static implicit operator ThemeResourceKey<Brush>(BorderBrushVSG self) => self.Default;
					}

					public static readonly BorderPaddingVSG BorderPadding = new();
					public partial class BorderPaddingVSG
					{
						[ResourceKeyDefinition(typeof(Thickness), "OutlinedPasswordBoxBorderPadding")]
						public ThemeResourceKey<Thickness> Default = new("OutlinedPasswordBoxBorderPadding");

						[ResourceKeyDefinition(typeof(Thickness), "OutlinedPasswordBoxBorderPaddingPointerOver")]
						public ThemeResourceKey<Thickness> PointerOver = new("OutlinedPasswordBoxBorderPaddingPointerOver");

						[ResourceKeyDefinition(typeof(Thickness), "OutlinedPasswordBoxBorderPaddingFocused")]
						public ThemeResourceKey<Thickness> Focused = new("OutlinedPasswordBoxBorderPaddingFocused");

						public static implicit operator ThemeResourceKey<Thickness>(BorderPaddingVSG self) => self.Default;
					}

					public static readonly BorderThicknessVSG BorderThickness = new();
					public partial class BorderThicknessVSG
					{
						[ResourceKeyDefinition(typeof(Thickness), "OutlinedPasswordBoxBorderThickness")]
						public ThemeResourceKey<Thickness> Default = new("OutlinedPasswordBoxBorderThickness");

						[ResourceKeyDefinition(typeof(Thickness), "OutlinedPasswordBoxBorderThicknessPointerOver")]
						public ThemeResourceKey<Thickness> PointerOver = new("OutlinedPasswordBoxBorderThicknessPointerOver");

						[ResourceKeyDefinition(typeof(Thickness), "OutlinedPasswordBoxBorderThicknessFocused")]
						public ThemeResourceKey<Thickness> Focused = new("OutlinedPasswordBoxBorderThicknessFocused");

						public static implicit operator ThemeResourceKey<Thickness>(BorderThicknessVSG self) => self.Default;
					}

					public static readonly ForegroundVSG Foreground = new();
					public partial class ForegroundVSG
					{
						[ResourceKeyDefinition(typeof(Brush), "OutlinedPasswordBoxForeground")]
						public ThemeResourceKey<Brush> Default = new("OutlinedPasswordBoxForeground");

						[ResourceKeyDefinition(typeof(Brush), "OutlinedPasswordBoxForegroundPointerOver")]
						public ThemeResourceKey<Brush> PointerOver = new("OutlinedPasswordBoxForegroundPointerOver");

						[ResourceKeyDefinition(typeof(Brush), "OutlinedPasswordBoxForegroundDisabled")]
						public ThemeResourceKey<Brush> Disabled = new("OutlinedPasswordBoxForegroundDisabled");

						[ResourceKeyDefinition(typeof(Brush), "OutlinedPasswordBoxForegroundFocused")]
						public ThemeResourceKey<Brush> Focused = new("OutlinedPasswordBoxForegroundFocused");

						public static implicit operator ThemeResourceKey<Brush>(ForegroundVSG self) => self.Default;
					}

					public static readonly LeadingIconForegroundVSG LeadingIconForeground = new();
					public partial class LeadingIconForegroundVSG
					{
						[ResourceKeyDefinition(typeof(Brush), "PasswordBoxLeadingIconForeground")]
						public ThemeResourceKey<Brush> Default = new("PasswordBoxLeadingIconForeground");

						[ResourceKeyDefinition(typeof(Brush), "PasswordBoxLeadingIconForegroundDisabled")]
						public ThemeResourceKey<Brush> Disabled = new("PasswordBoxLeadingIconForegroundDisabled");

						public static implicit operator ThemeResourceKey<Brush>(LeadingIconForegroundVSG self) => self.Default;
					}

					public static readonly PlaceholderForegroundVSG PlaceholderForeground = new();
					public partial class PlaceholderForegroundVSG
					{
						[ResourceKeyDefinition(typeof(Brush), "OutlinedPasswordBoxPlaceholderForeground")]
						public ThemeResourceKey<Brush> Default = new("OutlinedPasswordBoxPlaceholderForeground");

						[ResourceKeyDefinition(typeof(Brush), "OutlinedPasswordBoxPlaceholderForegroundPointerOver")]
						public ThemeResourceKey<Brush> PointerOver = new("OutlinedPasswordBoxPlaceholderForegroundPointerOver");

						[ResourceKeyDefinition(typeof(Brush), "OutlinedPasswordBoxPlaceholderForegroundDisabled")]
						public ThemeResourceKey<Brush> Disabled = new("OutlinedPasswordBoxPlaceholderForegroundDisabled");

						[ResourceKeyDefinition(typeof(Brush), "OutlinedPasswordBoxPlaceholderForegroundFocused")]
						public ThemeResourceKey<Brush> Focused = new("OutlinedPasswordBoxPlaceholderForegroundFocused");

						public static implicit operator ThemeResourceKey<Brush>(PlaceholderForegroundVSG self) => self.Default;
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
}
