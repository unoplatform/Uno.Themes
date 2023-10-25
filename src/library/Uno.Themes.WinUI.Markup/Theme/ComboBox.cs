using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Media;
using Uno.Extensions.Markup;
using Uno.Extensions.Markup.Internals;

namespace Uno.Themes.Markup;

public static partial class Theme
{
	public static partial class ComboBox
	{
		public static partial class Resources
		{
			public static class Default
			{
				public static partial class ArrowForeground
				{
					[ResourceKeyDefinition(typeof(Brush), "ComboBoxArrowForeground")]
					public static ThemeResourceKey<Brush> Default => new("ComboBoxArrowForeground");

					[ResourceKeyDefinition(typeof(Brush), "ComboBoxArrowForegroundOpened")]
					public static ThemeResourceKey<Brush> Opened => new("ComboBoxArrowForegroundOpened");
				}

				public static partial class Background
				{
					[ResourceKeyDefinition(typeof(Brush), "ComboBoxBackground")]
					public static ThemeResourceKey<Brush> Default => new("ComboBoxBackground");

					[ResourceKeyDefinition(typeof(Brush), "ComboBoxBackgroundDisabled")]
					public static ThemeResourceKey<Brush> Disabled => new("ComboBoxBackgroundDisabled");

					[ResourceKeyDefinition(typeof(Brush), "ComboBoxBackgroundPointerOver")]
					public static ThemeResourceKey<Brush> PointerOver => new("ComboBoxBackgroundPointerOver");

					[ResourceKeyDefinition(typeof(Brush), "ComboBoxBackgroundPressed")]
					public static ThemeResourceKey<Brush> Pressed => new("ComboBoxBackgroundPressed");

					[ResourceKeyDefinition(typeof(Brush), "ComboBoxBackgroundFocused")]
					public static ThemeResourceKey<Brush> Focused => new("ComboBoxBackgroundFocused");

					[ResourceKeyDefinition(typeof(Brush), "ComboBoxBackgroundFocusedPressed")]
					public static ThemeResourceKey<Brush> FocusedPressed => new("ComboBoxBackgroundFocusedPressed");

					[ResourceKeyDefinition(typeof(Brush), "ComboBoxBackgroundUnfocused")]
					public static ThemeResourceKey<Brush> Unfocused => new("ComboBoxBackgroundUnfocused");
				}

				public static partial class BorderBrush
				{
					[ResourceKeyDefinition(typeof(Brush), "ComboBoxBorderBrush")]
					public static ThemeResourceKey<Brush> Default => new("ComboBoxBorderBrush");

					[ResourceKeyDefinition(typeof(Brush), "ComboBoxBorderBrushDisabled")]
					public static ThemeResourceKey<Brush> Disabled => new("ComboBoxBorderBrushDisabled");

					[ResourceKeyDefinition(typeof(Brush), "ComboBoxBorderBrushPointerOver")]
					public static ThemeResourceKey<Brush> PointerOver => new("ComboBoxBorderBrushPointerOver");

					[ResourceKeyDefinition(typeof(Brush), "ComboBoxBorderBrushPressed")]
					public static ThemeResourceKey<Brush> Pressed => new("ComboBoxBorderBrushPressed");

					[ResourceKeyDefinition(typeof(Brush), "ComboBoxBorderBrushOpened")]
					public static ThemeResourceKey<Brush> Focused => new("ComboBoxBorderBrushOpened");
				}

				public static partial class BorderThickness
				{
					[ResourceKeyDefinition(typeof(Thickness), "ComboBoxBorderThickness")]
					public static ThemeResourceKey<Thickness> Default => new("ComboBoxBorderThickness");

					[ResourceKeyDefinition(typeof(Thickness), "ComboBoxOpenedBorderThickness")]
					public static ThemeResourceKey<Thickness> Focused => new("ComboBoxOpenedBorderThickness");
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
					public static partial class Background
					{
						[ResourceKeyDefinition(typeof(Brush), "ComboBoxDropDownBackground")]
						public static ThemeResourceKey<Brush> Default => new("ComboBoxDropDownBackground");

						[ResourceKeyDefinition(typeof(Brush), "ComboBoxBackgroundDisabled")]
						public static ThemeResourceKey<Brush> Disabled => new("ComboBoxBackgroundDisabled");

						[ResourceKeyDefinition(typeof(Brush), "ComboBoxDropDownBackgroundPointerOver")]
						public static ThemeResourceKey<Brush> PointerOver => new("ComboBoxDropDownBackgroundPointerOver");

						[ResourceKeyDefinition(typeof(Brush), "ComboBoxDropDownBackgroundPressed")]
						public static ThemeResourceKey<Brush> Pressed => new("ComboBoxDropDownBackgroundPressed");

						[ResourceKeyDefinition(typeof(Brush), "ComboBoxDropDownBackgroundFocused")]
						public static ThemeResourceKey<Brush> Focused => new("ComboBoxDropDownBackgroundFocused");

						[ResourceKeyDefinition(typeof(Brush), "ComboBoxDropDownBackgroundFocusedPressed")]
						public static ThemeResourceKey<Brush> FocusedPressed => new("ComboBoxDropDownBackgroundFocusedPressed");

						[ResourceKeyDefinition(typeof(Brush), "ComboBoxBackgroundUnfocused")]
						public static ThemeResourceKey<Brush> Unfocused => new("ComboBoxBackgroundUnfocused");
					}

					[ResourceKeyDefinition(typeof(Brush), "ComboBoxDropDownBorderBrush")]
					public static ThemeResourceKey<Brush> BorderBrush => new("ComboBoxDropDownBorderBrush");

					[ResourceKeyDefinition(typeof(Brush), "ComboBoxDropDownForeground")]
					public static ThemeResourceKey<Brush> Foreground => new("ComboBoxDropDownForeground");
				}

				public static partial class Foreground
				{
					[ResourceKeyDefinition(typeof(Brush), "ComboBoxForeground")]
					public static ThemeResourceKey<Brush> Default => new("ComboBoxForeground");

					[ResourceKeyDefinition(typeof(Brush), "ComboBoxForegroundDisabled")]
					public static ThemeResourceKey<Brush> Disabled => new("ComboBoxForegroundDisabled");

					[ResourceKeyDefinition(typeof(Brush), "ComboBoxForegroundPointerOver")]
					public static ThemeResourceKey<Brush> PointerOver => new("ComboBoxForegroundPointerOver");

					[ResourceKeyDefinition(typeof(Brush), "ComboBoxForegroundPressed")]
					public static ThemeResourceKey<Brush> Pressed => new("ComboBoxForegroundPressed");

					[ResourceKeyDefinition(typeof(Brush), "ComboBoxForegroundFocused")]
					public static ThemeResourceKey<Brush> Focused => new("ComboBoxForegroundFocused");

					[ResourceKeyDefinition(typeof(Brush), "ComboBoxForegroundFocusedPressed")]
					public static ThemeResourceKey<Brush> FocusedPressed => new("ComboBoxForegroundFocusedPressed");
				}

				public static partial class LeadingIcon
				{
					public static partial class Foreground
					{
						[ResourceKeyDefinition(typeof(Brush), "ComboBoxLeadingIconForeground")]
						public static ThemeResourceKey<Brush> Default => new("ComboBoxLeadingIconForeground");

						[ResourceKeyDefinition(typeof(Brush), "ComboBoxLeadingIconForegroundDisabled")]
						public static ThemeResourceKey<Brush> Disabled => new("ComboBoxLeadingIconForegroundDisabled");
					}

					[ResourceKeyDefinition(typeof(Thickness), "ComboBoxLeadingIconMargin")]
					public static ThemeResourceKey<Thickness> Margin => new("ComboBoxLeadingIconMargin");

					[ResourceKeyDefinition(typeof(double), "ComboBoxLeadingIconWidth")]
					public static ThemeResourceKey<double> Width => new("ComboBoxLeadingIconWidth");
				}

				public static partial class PlaceHolderForeground
				{
					[ResourceKeyDefinition(typeof(Brush), "ComboBoxPlaceHolderForeground")]
					public static ThemeResourceKey<Brush> Default => new("ComboBoxPlaceHolderForeground");

					[ResourceKeyDefinition(typeof(Brush), "ComboBoxPlaceHolderForegroundDisabled")]
					public static ThemeResourceKey<Brush> Disabled => new("ComboBoxPlaceHolderForegroundDisabled");

					[ResourceKeyDefinition(typeof(Brush), "ComboBoxPlaceHolderForegroundPointerOver")]
					public static ThemeResourceKey<Brush> PointerOver => new("ComboBoxPlaceHolderForegroundPointerOver");

					[ResourceKeyDefinition(typeof(Brush), "ComboBoxPlaceHolderForegroundPressed")]
					public static ThemeResourceKey<Brush> Pressed => new("ComboBoxPlaceHolderForegroundPressed");

					[ResourceKeyDefinition(typeof(Brush), "ComboBoxPlaceHolderForegroundFocused")]
					public static ThemeResourceKey<Brush> Focused => new("ComboBoxPlaceHolderForegroundFocused");

					[ResourceKeyDefinition(typeof(Brush), "ComboBoxPlaceHolderForegroundFocusedPressed")]
					public static ThemeResourceKey<Brush> FocusedPressed => new("ComboBoxPlaceHolderForegroundFocusedPressed");
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

				public static partial class UpperPlaceHolderForeground
				{
					[ResourceKeyDefinition(typeof(Brush), "ComboBoxUpperPlaceHolderForeground")]
					public static ThemeResourceKey<Brush> Default => new("ComboBoxUpperPlaceHolderForeground");

					[ResourceKeyDefinition(typeof(Brush), "ComboBoxUpperPlaceHolderForegroundDisabled")]
					public static ThemeResourceKey<Brush> Disabled => new("ComboBoxUpperPlaceHolderForegroundDisabled");

					[ResourceKeyDefinition(typeof(Brush), "ComboBoxUpperPlaceHolderForegroundPointerOver")]
					public static ThemeResourceKey<Brush> PointerOver => new("ComboBoxUpperPlaceHolderForegroundPointerOver");

					[ResourceKeyDefinition(typeof(Brush), "ComboBoxUpperPlaceHolderForegroundPressed")]
					public static ThemeResourceKey<Brush> Pressed => new("ComboBoxUpperPlaceHolderForegroundPressed");

					[ResourceKeyDefinition(typeof(Brush), "ComboBoxUpperPlaceHolderForegroundFocused")]
					public static ThemeResourceKey<Brush> Focused => new("ComboBoxUpperPlaceHolderForegroundFocused");

					[ResourceKeyDefinition(typeof(Brush), "ComboBoxUpperPlaceHolderForegroundFocusedPressed")]
					public static ThemeResourceKey<Brush> FocusedPressed => new("ComboBoxUpperPlaceHolderForegroundFocusedPressed");

					[ResourceKeyDefinition(typeof(Brush), "ComboBoxUpperPlaceHolderForegroundOpened")]
					public static ThemeResourceKey<Brush> Opened => new("ComboBoxUpperPlaceHolderForegroundOpened");
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
		}

		public static partial class Styles
		{
			[ResourceKeyDefinition(typeof(Style), "ComboBoxStyle", TargetType = typeof(global::Microsoft.UI.Xaml.Controls.ComboBox))]
			public static StaticResourceKey<Style> Default => new("ComboBoxStyle");
		}
	}
}
