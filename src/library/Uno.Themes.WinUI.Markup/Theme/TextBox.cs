using System;
using Windows.UI;
using Microsoft.UI.Text;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Media;
using Uno.Extensions.Markup;
using Uno.Extensions.Markup.Internals;

using ResourceKeyDefinitionAttribute = Uno.Themes.WinUI.Markup.ResourceKeyDefinitionAttribute;

namespace Uno.Themes.Markup
{
	public static partial class Theme
	{
		public static partial class TextBox
		{
			public static partial class Resources
			{
				public static partial class Filled
				{
					public static readonly BackgroundVSG Background = new();
					public partial class BackgroundVSG
					{
						[ResourceKeyDefinition(typeof(Brush), "FilledTextBoxBackground")]
						public ThemeResourceKey<Brush> Default = new("FilledTextBoxBackground");

						[ResourceKeyDefinition(typeof(Brush), "FilledTextBoxBackgroundPointerOver")]
						public ThemeResourceKey<Brush> PointerOver = new("FilledTextBoxBackgroundPointerOver");

						[ResourceKeyDefinition(typeof(Brush), "FilledTextBoxBackgroundDisabled")]
						public ThemeResourceKey<Brush> Disabled = new("FilledTextBoxBackgroundDisabled");

						[ResourceKeyDefinition(typeof(Brush), "FilledTextBoxBackgroundFocused")]
						public ThemeResourceKey<Brush> Focused = new("FilledTextBoxBackgroundFocused");

						public static implicit operator ThemeResourceKey<Brush>(BackgroundVSG self) => self.Default;
					}

					public static readonly BorderBrushVSG BorderBrush = new();
					public partial class BorderBrushVSG
					{
						[ResourceKeyDefinition(typeof(Brush), "FilledTextBoxBorderBrushFocused")]
						public ThemeResourceKey<Brush> Default = new("FilledTextBoxBorderBrushFocused");

						[ResourceKeyDefinition(typeof(Brush), "FilledTextBoxBorderBrushPointerOver")]
						public ThemeResourceKey<Brush> PointerOver = new("FilledTextBoxBorderBrushPointerOver");

						[ResourceKeyDefinition(typeof(Brush), "FilledTextBoxBorderBrushDisabled")]
						public ThemeResourceKey<Brush> Disabled = new("FilledTextBoxBorderBrushDisabled");

						public static implicit operator ThemeResourceKey<Brush>(BorderBrushVSG self) => self.Default;
					}

					public static readonly BorderThicknessNormalVSG BorderThicknessNormal = new();
					public partial class BorderThicknessNormalVSG
					{
						[ResourceKeyDefinition(typeof(double), "FilledTextBoxBorderThicknessNormal")]
						public ThemeResourceKey<double> Default = new("FilledTextBoxBorderThicknessNormal");

						[ResourceKeyDefinition(typeof(double), "MaterialFilledTextBoxBorderHeightFocused")]
						public ThemeResourceKey<double> Focused = new("MaterialFilledTextBoxBorderHeightFocused");

						public static implicit operator ThemeResourceKey<double>(BorderThicknessNormalVSG self) => self.Default;
					}

					public static readonly DeleteButtonForegroundVSG DeleteButtonForeground = new();
					public partial class DeleteButtonForegroundVSG
					{
						[ResourceKeyDefinition(typeof(Brush), "FilledTextBoxDeleteButtonForeground")]
						public ThemeResourceKey<Brush> Default = new("FilledTextBoxDeleteButtonForeground");

						[ResourceKeyDefinition(typeof(Brush), "FilledTextBoxDeleteButtonForegroundPointerOver")]
						public ThemeResourceKey<Brush> PointerOver = new("FilledTextBoxDeleteButtonForegroundPointerOver");

						[ResourceKeyDefinition(typeof(Brush), "FilledTextBoxDeleteButtonForegroundDisabled")]
						public ThemeResourceKey<Brush> Disabled = new("FilledTextBoxDeleteButtonForegroundDisabled");

						[ResourceKeyDefinition(typeof(Brush), "FilledTextBoxDeleteButtonForegroundFocused")]
						public ThemeResourceKey<Brush> Focused = new("FilledTextBoxDeleteButtonForegroundFocused");

						public static implicit operator ThemeResourceKey<Brush>(DeleteButtonForegroundVSG self) => self.Default;
					}

					public static readonly ForegroundVSG Foreground = new();
					public partial class ForegroundVSG
					{
						[ResourceKeyDefinition(typeof(Brush), "FilledTextBoxForeground")]
						public ThemeResourceKey<Brush> Default = new("FilledTextBoxForeground");

						[ResourceKeyDefinition(typeof(Brush), "FilledTextBoxForegroundPointerOver")]
						public ThemeResourceKey<Brush> PointerOver = new("FilledTextBoxForegroundPointerOver");

						[ResourceKeyDefinition(typeof(Brush), "FilledTextBoxForegroundDisabled")]
						public ThemeResourceKey<Brush> Disabled = new("FilledTextBoxForegroundDisabled");

						[ResourceKeyDefinition(typeof(Brush), "FilledTextBoxForegroundFocused")]
						public ThemeResourceKey<Brush> Focused = new("FilledTextBoxForegroundFocused");

						public static implicit operator ThemeResourceKey<Brush>(ForegroundVSG self) => self.Default;
					}

					public static readonly LeadingIconForegroundVSG LeadingIconForeground = new();
					public partial class LeadingIconForegroundVSG
					{
						[ResourceKeyDefinition(typeof(Brush), "TextBoxLeadingIconForeground")]
						public ThemeResourceKey<Brush> Default = new("TextBoxLeadingIconForeground");

						[ResourceKeyDefinition(typeof(Brush), "TextBoxLeadingIconForegroundDisabled")]
						public ThemeResourceKey<Brush> Disabled = new("TextBoxLeadingIconForegroundDisabled");

						public static implicit operator ThemeResourceKey<Brush>(LeadingIconForegroundVSG self) => self.Default;
					}

					public static readonly PlaceholderForegroundVSG PlaceholderForeground = new();
					public partial class PlaceholderForegroundVSG
					{
						[ResourceKeyDefinition(typeof(Brush), "FilledTextBoxPlaceholderForeground")]
						public ThemeResourceKey<Brush> Default = new("FilledTextBoxPlaceholderForeground");

						[ResourceKeyDefinition(typeof(Brush), "FilledTextBoxPlaceholderForegroundPointerOver")]
						public ThemeResourceKey<Brush> PointerOver = new("FilledTextBoxPlaceholderForegroundPointerOver");

						[ResourceKeyDefinition(typeof(Brush), "FilledTextBoxPlaceholderForegroundDisabled")]
						public ThemeResourceKey<Brush> Disabled = new("FilledTextBoxPlaceholderForegroundDisabled");

						[ResourceKeyDefinition(typeof(Brush), "FilledTextBoxPlaceholderForegroundFocused")]
						public ThemeResourceKey<Brush> Focused = new("FilledTextBoxPlaceholderForegroundFocused");

						public static implicit operator ThemeResourceKey<Brush>(PlaceholderForegroundVSG self) => self.Default;
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
					public static readonly BorderBrushVSG BorderBrush = new();
					public partial class BorderBrushVSG
					{
						[ResourceKeyDefinition(typeof(Brush), "OutlinedTextBoxBorderBrush")]
						public ThemeResourceKey<Brush> Default = new("OutlinedTextBoxBorderBrush");

						[ResourceKeyDefinition(typeof(Brush), "OutlinedTextBoxBorderBrushPointerOver")]
						public ThemeResourceKey<Brush> PointerOver = new("OutlinedTextBoxBorderBrushPointerOver");

						[ResourceKeyDefinition(typeof(Brush), "OutlinedTextBoxBorderBrushDisabled")]
						public ThemeResourceKey<Brush> Disabled = new("OutlinedTextBoxBorderBrushDisabled");

						[ResourceKeyDefinition(typeof(Brush), "OutlinedTextBoxBorderBrushFocused")]
						public ThemeResourceKey<Brush> Focused = new("OutlinedTextBoxBorderBrushFocused");

						public static implicit operator ThemeResourceKey<Brush>(BorderBrushVSG self) => self.Default;
					}

					public static readonly ForegroundVSG Foreground = new();
					public partial class ForegroundVSG
					{
						[ResourceKeyDefinition(typeof(Brush), "OutlinedTextBoxForeground")]
						public ThemeResourceKey<Brush> Default = new("OutlinedTextBoxForeground");

						[ResourceKeyDefinition(typeof(Brush), "OutlinedTextBoxForegroundPointerOver")]
						public ThemeResourceKey<Brush> PointerOver = new("OutlinedTextBoxForegroundPointerOver");

						[ResourceKeyDefinition(typeof(Brush), "OutlinedTextBoxForegroundDisabled")]
						public ThemeResourceKey<Brush> Disabled = new("OutlinedTextBoxForegroundDisabled");

						[ResourceKeyDefinition(typeof(Brush), "OutlinedTextBoxForegroundFocused")]
						public ThemeResourceKey<Brush> Focused = new("OutlinedTextBoxForegroundFocused");

						public static implicit operator ThemeResourceKey<Brush>(ForegroundVSG self) => self.Default;
					}

					public static readonly LeadingIconForegroundVSG LeadingIconForeground = new();
					public partial class LeadingIconForegroundVSG
					{
						[ResourceKeyDefinition(typeof(Brush), "TextBoxLeadingIconForeground")]
						public ThemeResourceKey<Brush> Default = new("TextBoxLeadingIconForeground");

						[ResourceKeyDefinition(typeof(Brush), "TextBoxLeadingIconForegroundDisabled")]
						public ThemeResourceKey<Brush> Disabled = new("TextBoxLeadingIconForegroundDisabled");

						public static implicit operator ThemeResourceKey<Brush>(LeadingIconForegroundVSG self) => self.Default;
					}

					public static readonly PlaceholderForegroundVSG PlaceholderForeground = new();
					public partial class PlaceholderForegroundVSG
					{
						[ResourceKeyDefinition(typeof(Brush), "OutlinedTextBoxPlaceholderForeground")]
						public ThemeResourceKey<Brush> Default = new("OutlinedTextBoxPlaceholderForeground");

						[ResourceKeyDefinition(typeof(Brush), "OutlinedTextBoxPlaceholderForegroundPointerOver")]
						public ThemeResourceKey<Brush> PointerOver = new("OutlinedTextBoxPlaceholderForegroundPointerOver");

						[ResourceKeyDefinition(typeof(Brush), "OutlinedTextBoxPlaceholderForegroundDisabled")]
						public ThemeResourceKey<Brush> Disabled = new("OutlinedTextBoxPlaceholderForegroundDisabled");

						[ResourceKeyDefinition(typeof(Brush), "OutlinedTextBoxPlaceholderForegroundFocused")]
						public ThemeResourceKey<Brush> Focused = new("OutlinedTextBoxPlaceholderForegroundFocused");

						public static implicit operator ThemeResourceKey<Brush>(PlaceholderForegroundVSG self) => self.Default;
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
}
