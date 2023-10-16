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
		public static partial class ComboBox
		{
			public static partial class Resources
			{
				public static readonly ArrowForegroundVSG ArrowForeground = new();
				public partial class ArrowForegroundVSG
				{
					[ResourceKeyDefinition(typeof(Brush), "ComboBoxArrowForeground")]
					public ThemeResourceKey<Brush> Default = new("ComboBoxArrowForeground");

					[ResourceKeyDefinition(typeof(Brush), "ComboBoxArrowForegroundOpened")]
					public ThemeResourceKey<Brush> Opened = new("ComboBoxArrowForegroundOpened");

					public static implicit operator ThemeResourceKey<Brush>(ArrowForegroundVSG self) => self.Default;
				}

				public static readonly BackgroundVSG Background = new();
				public partial class BackgroundVSG
				{
					[ResourceKeyDefinition(typeof(Brush), "ComboBoxBackground")]
					public ThemeResourceKey<Brush> Default = new("ComboBoxBackground");

					[ResourceKeyDefinition(typeof(Brush), "ComboBoxBackgroundDisabled")]
					public ThemeResourceKey<Brush> Disabled = new("ComboBoxBackgroundDisabled");

					[ResourceKeyDefinition(typeof(Brush), "ComboBoxBackgroundPointerOver")]
					public ThemeResourceKey<Brush> PointerOver = new("ComboBoxBackgroundPointerOver");

					[ResourceKeyDefinition(typeof(Brush), "ComboBoxBackgroundPressed")]
					public ThemeResourceKey<Brush> Pressed = new("ComboBoxBackgroundPressed");

					[ResourceKeyDefinition(typeof(Brush), "ComboBoxBackgroundFocused")]
					public ThemeResourceKey<Brush> Focused = new("ComboBoxBackgroundFocused");

					[ResourceKeyDefinition(typeof(Brush), "ComboBoxBackgroundFocusedPressed")]
					public ThemeResourceKey<Brush> FocusedPressed = new("ComboBoxBackgroundFocusedPressed");

					[ResourceKeyDefinition(typeof(Brush), "ComboBoxBackgroundUnfocused")]
					public ThemeResourceKey<Brush> Unfocused = new("ComboBoxBackgroundUnfocused");

					public static implicit operator ThemeResourceKey<Brush>(BackgroundVSG self) => self.Default;
				}

				public static readonly BorderBrushVSG BorderBrush = new();
				public partial class BorderBrushVSG
				{
					[ResourceKeyDefinition(typeof(Brush), "ComboBoxBorderBrush")]
					public ThemeResourceKey<Brush> Default = new("ComboBoxBorderBrush");

					[ResourceKeyDefinition(typeof(Brush), "ComboBoxBorderBrushDisabled")]
					public ThemeResourceKey<Brush> Disabled = new("ComboBoxBorderBrushDisabled");

					[ResourceKeyDefinition(typeof(Brush), "ComboBoxBorderBrushPointerOver")]
					public ThemeResourceKey<Brush> PointerOver = new("ComboBoxBorderBrushPointerOver");

					[ResourceKeyDefinition(typeof(Brush), "ComboBoxBorderBrushPressed")]
					public ThemeResourceKey<Brush> Pressed = new("ComboBoxBorderBrushPressed");

					[ResourceKeyDefinition(typeof(Brush), "ComboBoxBorderBrushOpened")]
					public ThemeResourceKey<Brush> Focused = new("ComboBoxBorderBrushOpened");

					public static implicit operator ThemeResourceKey<Brush>(BorderBrushVSG self) => self.Default;
				}

				public static readonly BorderThicknessVSG BorderThickness = new();
				public partial class BorderThicknessVSG
				{
					[ResourceKeyDefinition(typeof(Thickness), "ComboBoxBorderThickness")]
					public ThemeResourceKey<Thickness> Default = new("ComboBoxBorderThickness");

					[ResourceKeyDefinition(typeof(Thickness), "ComboBoxOpenedBorderThickness")]
					public ThemeResourceKey<Thickness> Focused = new("ComboBoxOpenedBorderThickness");

					public static implicit operator ThemeResourceKey<Thickness>(BorderThicknessVSG self) => self.Default;
				}

				public static partial class DownGlyph
				{
					[ResourceKeyDefinition(typeof(double), "ComboBoxDownGlyphHeight")]
					public static ThemeResourceKey<double> Height => new("ComboBoxDownGlyphHeight");

					[ResourceKeyDefinition(typeof(Thickness), "ComboBoxDownGlyphMargin")]
					public static ThemeResourceKey<Thickness> Margin => new("ComboBoxDownGlyphMargin");

					[ResourceKeyDefinition(typeof(double), "ComboBoxDownGlyphWidth")]
					public static ThemeResourceKey<double> Width => new("ComboBoxDownGlyphWidth");
				}

				public static partial class DropDown
				{
					public static readonly BackgroundVSG Background = new();
					public partial class BackgroundVSG
					{
						[ResourceKeyDefinition(typeof(Brush), "ComboBoxDropDownBackground")]
						public ThemeResourceKey<Brush> Default = new("ComboBoxDropDownBackground");

						[ResourceKeyDefinition(typeof(Brush), "ComboBoxBackgroundDisabled")]
						public ThemeResourceKey<Brush> Disabled = new("ComboBoxBackgroundDisabled");

						[ResourceKeyDefinition(typeof(Brush), "ComboBoxDropDownBackgroundPointerOver")]
						public ThemeResourceKey<Brush> PointerOver = new("ComboBoxDropDownBackgroundPointerOver");

						[ResourceKeyDefinition(typeof(Brush), "ComboBoxDropDownBackgroundPressed")]
						public ThemeResourceKey<Brush> Pressed = new("ComboBoxDropDownBackgroundPressed");

						[ResourceKeyDefinition(typeof(Brush), "ComboBoxDropDownBackgroundFocused")]
						public ThemeResourceKey<Brush> Focused = new("ComboBoxDropDownBackgroundFocused");

						[ResourceKeyDefinition(typeof(Brush), "ComboBoxDropDownBackgroundFocusedPressed")]
						public ThemeResourceKey<Brush> FocusedPressed = new("ComboBoxDropDownBackgroundFocusedPressed");

						[ResourceKeyDefinition(typeof(Brush), "ComboBoxBackgroundUnfocused")]
						public ThemeResourceKey<Brush> Unfocused = new("ComboBoxBackgroundUnfocused");

						public static implicit operator ThemeResourceKey<Brush>(BackgroundVSG self) => self.Default;
					}

					[ResourceKeyDefinition(typeof(Brush), "ComboBoxDropDownBorderBrush")]
					public static ThemeResourceKey<Brush> BorderBrush => new("ComboBoxDropDownBorderBrush");

					[ResourceKeyDefinition(typeof(Brush), "ComboBoxDropDownForeground")]
					public static ThemeResourceKey<Brush> Foreground => new("ComboBoxDropDownForeground");
				}

				public static readonly ForegroundVSG Foreground = new();
				public partial class ForegroundVSG
				{
					[ResourceKeyDefinition(typeof(Brush), "ComboBoxForeground")]
					public ThemeResourceKey<Brush> Default = new("ComboBoxForeground");

					[ResourceKeyDefinition(typeof(Brush), "ComboBoxForegroundDisabled")]
					public ThemeResourceKey<Brush> Disabled = new("ComboBoxForegroundDisabled");

					[ResourceKeyDefinition(typeof(Brush), "ComboBoxForegroundPointerOver")]
					public ThemeResourceKey<Brush> PointerOver = new("ComboBoxForegroundPointerOver");

					[ResourceKeyDefinition(typeof(Brush), "ComboBoxForegroundPressed")]
					public ThemeResourceKey<Brush> Pressed = new("ComboBoxForegroundPressed");

					[ResourceKeyDefinition(typeof(Brush), "ComboBoxForegroundFocused")]
					public ThemeResourceKey<Brush> Focused = new("ComboBoxForegroundFocused");

					[ResourceKeyDefinition(typeof(Brush), "ComboBoxForegroundFocusedPressed")]
					public ThemeResourceKey<Brush> FocusedPressed = new("ComboBoxForegroundFocusedPressed");

					public static implicit operator ThemeResourceKey<Brush>(ForegroundVSG self) => self.Default;
				}

				public static partial class LeadingIcon
				{
					public static readonly ForegroundVSG Foreground = new();
					public partial class ForegroundVSG
					{
						[ResourceKeyDefinition(typeof(Brush), "ComboBoxLeadingIconForeground")]
						public ThemeResourceKey<Brush> Default = new("ComboBoxLeadingIconForeground");

						[ResourceKeyDefinition(typeof(Brush), "ComboBoxLeadingIconForegroundDisabled")]
						public ThemeResourceKey<Brush> Disabled = new("ComboBoxLeadingIconForegroundDisabled");

						public static implicit operator ThemeResourceKey<Brush>(ForegroundVSG self) => self.Default;
					}

					[ResourceKeyDefinition(typeof(Thickness), "ComboBoxLeadingIconMargin")]
					public static ThemeResourceKey<Thickness> Margin => new("ComboBoxLeadingIconMargin");

					[ResourceKeyDefinition(typeof(double), "ComboBoxLeadingIconWidth")]
					public static ThemeResourceKey<double> Width => new("ComboBoxLeadingIconWidth");
				}

				public static readonly PlaceHolderForegroundVSG PlaceHolderForeground = new();
				public partial class PlaceHolderForegroundVSG
				{
					[ResourceKeyDefinition(typeof(Brush), "ComboBoxPlaceHolderForeground")]
					public ThemeResourceKey<Brush> Default = new("ComboBoxPlaceHolderForeground");

					[ResourceKeyDefinition(typeof(Brush), "ComboBoxPlaceHolderForegroundDisabled")]
					public ThemeResourceKey<Brush> Disabled = new("ComboBoxPlaceHolderForegroundDisabled");

					[ResourceKeyDefinition(typeof(Brush), "ComboBoxPlaceHolderForegroundPointerOver")]
					public ThemeResourceKey<Brush> PointerOver = new("ComboBoxPlaceHolderForegroundPointerOver");

					[ResourceKeyDefinition(typeof(Brush), "ComboBoxPlaceHolderForegroundPressed")]
					public ThemeResourceKey<Brush> Pressed = new("ComboBoxPlaceHolderForegroundPressed");

					[ResourceKeyDefinition(typeof(Brush), "ComboBoxPlaceHolderForegroundFocused")]
					public ThemeResourceKey<Brush> Focused = new("ComboBoxPlaceHolderForegroundFocused");

					[ResourceKeyDefinition(typeof(Brush), "ComboBoxPlaceHolderForegroundFocusedPressed")]
					public ThemeResourceKey<Brush> FocusedPressed = new("ComboBoxPlaceHolderForegroundFocusedPressed");

					public static implicit operator ThemeResourceKey<Brush>(PlaceHolderForegroundVSG self) => self.Default;
				}

				public static partial class UpGlyph
				{
					[ResourceKeyDefinition(typeof(double), "ComboBoxUpGlyphHeight")]
					public static ThemeResourceKey<double> Height => new("ComboBoxUpGlyphHeight");

					[ResourceKeyDefinition(typeof(Thickness), "ComboBoxUpGlyphMargin")]
					public static ThemeResourceKey<Thickness> Margin => new("ComboBoxUpGlyphMargin");

					[ResourceKeyDefinition(typeof(double), "ComboBoxUpGlyphWidth")]
					public static ThemeResourceKey<double> Width => new("ComboBoxUpGlyphWidth");
				}

				public static readonly UpperPlaceHolderForegroundVSG UpperPlaceHolderForeground = new();
				public partial class UpperPlaceHolderForegroundVSG
				{
					[ResourceKeyDefinition(typeof(Brush), "ComboBoxUpperPlaceHolderForeground")]
					public ThemeResourceKey<Brush> Default = new("ComboBoxUpperPlaceHolderForeground");

					[ResourceKeyDefinition(typeof(Brush), "ComboBoxUpperPlaceHolderForegroundDisabled")]
					public ThemeResourceKey<Brush> Disabled = new("ComboBoxUpperPlaceHolderForegroundDisabled");

					[ResourceKeyDefinition(typeof(Brush), "ComboBoxUpperPlaceHolderForegroundPointerOver")]
					public ThemeResourceKey<Brush> PointerOver = new("ComboBoxUpperPlaceHolderForegroundPointerOver");

					[ResourceKeyDefinition(typeof(Brush), "ComboBoxUpperPlaceHolderForegroundPressed")]
					public ThemeResourceKey<Brush> Pressed = new("ComboBoxUpperPlaceHolderForegroundPressed");

					[ResourceKeyDefinition(typeof(Brush), "ComboBoxUpperPlaceHolderForegroundFocused")]
					public ThemeResourceKey<Brush> Focused = new("ComboBoxUpperPlaceHolderForegroundFocused");

					[ResourceKeyDefinition(typeof(Brush), "ComboBoxUpperPlaceHolderForegroundFocusedPressed")]
					public ThemeResourceKey<Brush> FocusedPressed = new("ComboBoxUpperPlaceHolderForegroundFocusedPressed");

					[ResourceKeyDefinition(typeof(Brush), "ComboBoxUpperPlaceHolderForegroundOpened")]
					public ThemeResourceKey<Brush> Opened = new("ComboBoxUpperPlaceHolderForegroundOpened");

					public static implicit operator ThemeResourceKey<Brush>(UpperPlaceHolderForegroundVSG self) => self.Default;
				}

				[ResourceKeyDefinition(typeof(CornerRadius), "ComboBoxCornerRadius")]
				public static ThemeResourceKey<CornerRadius> CornerRadius => new("ComboBoxCornerRadius");

				[ResourceKeyDefinition(typeof(string), "ComboBoxDownArrowPathData")]
				public static StaticResourceKey<string> DownArrowPathData => new("ComboBoxDownArrowPathData");

				[ResourceKeyDefinition(typeof(double), "ComboBoxMinHeight")]
				public static ThemeResourceKey<double> MinHeight => new("ComboBoxMinHeight");

				[ResourceKeyDefinition(typeof(Thickness), "ComboBoxPadding")]
				public static ThemeResourceKey<Thickness> Padding => new("ComboBoxPadding");

				[ResourceKeyDefinition(typeof(string), "ComboBoxUpArrowPathData")]
				public static StaticResourceKey<string> UpArrowPathData => new("ComboBoxUpArrowPathData");
			}

			public static partial class Styles
			{
				[ResourceKeyDefinition(typeof(Style), "MaterialComboBoxStyle", TargetType = typeof(global::Microsoft.UI.Xaml.Controls.ComboBox))]
				public static StaticResourceKey<Style> Default => new("MaterialComboBoxStyle");
			}
		}
	}
}
