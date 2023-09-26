using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Media;
using Uno.Extensions.Markup;
using Uno.Extensions.Markup.Internals;

namespace Uno.Themes.Markup
{
	public static partial class Theme
	{
		public static class ComboBox
		{
			public static class Resources
			{
				public static class Default
				{
					public static class Foreground
					{
						[ResourceKeyDefinition(typeof(Brush), "ComboBoxForeground")]
						public static ThemeResourceKey<Brush> Default => new("ComboBoxForeground");

						[ResourceKeyDefinition(typeof(Brush), "ComboBoxForegroundPointerOver")]
						public static ThemeResourceKey<Brush> PointerOver => new("ComboBoxForegroundPointerOver");

						[ResourceKeyDefinition(typeof(Brush), "ComboBoxForegroundPressed")]
						public static ThemeResourceKey<Brush> Pressed => new("ComboBoxForegroundPressed");

						[ResourceKeyDefinition(typeof(Brush), "ComboBoxForegroundDisabled")]
						public static ThemeResourceKey<Brush> Disabled => new("ComboBoxForegroundDisabled");

						[ResourceKeyDefinition(typeof(Brush), "ComboBoxForegroundFocused")]
						public static ThemeResourceKey<Brush> Focused => new("ComboBoxForegroundFocused");

						[ResourceKeyDefinition(typeof(Brush), "ComboBoxForegroundFocusedPressed")]
						public static ThemeResourceKey<Brush> FocusedPressed => new("ComboBoxForegroundFocusedPressed");
					}

					public static class Background
					{
						[ResourceKeyDefinition(typeof(Brush), "ComboBoxBackground")]
						public static ThemeResourceKey<Brush> Default => new("ComboBoxBackground");

						[ResourceKeyDefinition(typeof(Brush), "ComboBoxBackgroundPointerOver")]
						public static ThemeResourceKey<Brush> PointerOver => new("ComboBoxBackgroundPointerOver");

						[ResourceKeyDefinition(typeof(Brush), "ComboBoxBackgroundPressed")]
						public static ThemeResourceKey<Brush> Pressed => new("ComboBoxBackgroundPressed");

						[ResourceKeyDefinition(typeof(Brush), "ComboBoxBackgroundDisabled")]
						public static ThemeResourceKey<Brush> Disabled => new("ComboBoxBackgroundDisabled");

						[ResourceKeyDefinition(typeof(Brush), "ComboBoxBackgroundUnfocused")]
						public static ThemeResourceKey<Brush> Unfocused => new("ComboBoxBackgroundUnfocused");

						[ResourceKeyDefinition(typeof(Brush), "ComboBoxBackgroundFocused")]
						public static ThemeResourceKey<Brush> Focused => new("ComboBoxBackgroundFocused");

						[ResourceKeyDefinition(typeof(Brush), "ComboBoxBackgroundFocusedPressed")]
						public static ThemeResourceKey<Brush> FocusedPressed => new("ComboBoxBackgroundFocusedPressed");
					}

					public static class BorderBrush
					{
						[ResourceKeyDefinition(typeof(Brush), "ComboBoxBorderBrush")]
						public static ThemeResourceKey<Brush> Default => new("ComboBoxBorderBrush");

						[ResourceKeyDefinition(typeof(Brush), "ComboBoxBorderBrushPressed")]
						public static ThemeResourceKey<Brush> Pressed => new("ComboBoxBorderBrushPressed");

						[ResourceKeyDefinition(typeof(Brush), "ComboBoxBorderBrushPointerOver")]
						public static ThemeResourceKey<Brush> PointerOver => new("ComboBoxBorderBrushPointerOver");

						[ResourceKeyDefinition(typeof(Brush), "ComboBoxBorderBrushDisabled")]
						public static ThemeResourceKey<Brush> Disabled => new("ComboBoxBorderBrushDisabled");

						[ResourceKeyDefinition(typeof(Brush), "ComboBoxBorderBrushOpened")]
						public static ThemeResourceKey<Brush> Opened => new("ComboBoxBorderBrushOpened");
					}

					public static class PlaceHolderForeground
					{
						[ResourceKeyDefinition(typeof(Brush), "ComboBoxPlaceHolderForeground")]
						public static ThemeResourceKey<Brush> Default => new("ComboBoxPlaceHolderForeground");

						[ResourceKeyDefinition(typeof(Brush), "ComboBoxPlaceHolderForegroundPointerOver")]
						public static ThemeResourceKey<Brush> PointerOver => new("ComboBoxPlaceHolderForegroundPointerOver");

						[ResourceKeyDefinition(typeof(Brush), "ComboBoxPlaceHolderForegroundPressed")]
						public static ThemeResourceKey<Brush> Pressed => new("ComboBoxPlaceHolderForegroundPressed");

						[ResourceKeyDefinition(typeof(Brush), "ComboBoxPlaceHolderForegroundDisabled")]
						public static ThemeResourceKey<Brush> Disabled => new("ComboBoxPlaceHolderForegroundDisabled");

						[ResourceKeyDefinition(typeof(Brush), "ComboBoxPlaceHolderForegroundFocused")]
						public static ThemeResourceKey<Brush> Focused => new("ComboBoxPlaceHolderForegroundFocused");

						[ResourceKeyDefinition(typeof(Brush), "ComboBoxPlaceHolderForegroundFocusedPressed")]
						public static ThemeResourceKey<Brush> FocusedPressed => new("ComboBoxPlaceHolderForegroundFocusedPressed");
					}

					public static class LeadingIconForeground
					{
						[ResourceKeyDefinition(typeof(Brush), "ComboBoxLeadingIconForeground")]
						public static ThemeResourceKey<Brush> Default => new("ComboBoxLeadingIconForeground");

						[ResourceKeyDefinition(typeof(Brush), "ComboBoxLeadingIconForegroundDisabled")]
						public static ThemeResourceKey<Brush> Disabled => new("ComboBoxLeadingIconForegroundDisabled");
					}

					public static class ArrowForeground
					{
						[ResourceKeyDefinition(typeof(Brush), "ComboBoxArrowForeground")]
						public static ThemeResourceKey<Brush> Default => new("ComboBoxArrowForeground");

						[ResourceKeyDefinition(typeof(Brush), "ComboBoxArrowForegroundOpened")]
						public static ThemeResourceKey<Brush> Opened => new("ComboBoxArrowForegroundOpened");
					}

					public static class UpperPlaceHolderForeground
					{
						[ResourceKeyDefinition(typeof(Brush), "ComboBoxUpperPlaceHolderForeground")]
						public static ThemeResourceKey<Brush> Default => new("ComboBoxUpperPlaceHolderForeground");
					}
				}

				public static class ComboBoxItem
				{
					public static class Foreground
					{
						[ResourceKeyDefinition(typeof(Brush), "ComboBoxItemForeground")]
						public static ThemeResourceKey<Brush> Default => new("ComboBoxItemForeground");

						[ResourceKeyDefinition(typeof(Brush), "ComboBoxItemForegroundPressed")]
						public static ThemeResourceKey<Brush> Pressed => new("ComboBoxItemForegroundPressed");

						[ResourceKeyDefinition(typeof(Brush), "ComboBoxItemForegroundPointerOver")]
						public static ThemeResourceKey<Brush> PointerOver => new("ComboBoxItemForegroundPointerOver");

						[ResourceKeyDefinition(typeof(Brush), "ComboBoxItemForegroundDisabled")]
						public static ThemeResourceKey<Brush> Disabled => new("ComboBoxItemForegroundDisabled");
					}

					public static class ForegroundSelected
					{
						[ResourceKeyDefinition(typeof(Brush), "ComboBoxItemForegroundSelected")]
						public static ThemeResourceKey<Brush> Default => new("ComboBoxItemForegroundSelected");

						[ResourceKeyDefinition(typeof(Brush), "ComboBoxItemForegroundSelectedUnfocused")]
						public static ThemeResourceKey<Brush> Unfocused => new("ComboBoxItemForegroundSelectedUnfocused");

						[ResourceKeyDefinition(typeof(Brush), "ComboBoxItemForegroundSelectedPressed")]
						public static ThemeResourceKey<Brush> Pressed => new("ComboBoxItemForegroundSelectedPressed");

						[ResourceKeyDefinition(typeof(Brush), "ComboBoxItemForegroundSelectedPointerOver")]
						public static ThemeResourceKey<Brush> PointerOver => new("ComboBoxItemForegroundSelectedPointerOver");

						[ResourceKeyDefinition(typeof(Brush), "ComboBoxItemForegroundSelectedDisabled")]
						public static ThemeResourceKey<Brush> Disabled => new("ComboBoxItemForegroundSelectedDisabled");
					}

					public static class Background
					{
						[ResourceKeyDefinition(typeof(Brush), "ComboBoxItemBackground")]
						public static ThemeResourceKey<Brush> Default => new("ComboBoxItemBackground");

						[ResourceKeyDefinition(typeof(Brush), "ComboBoxItemBackgroundPressed")]
						public static ThemeResourceKey<Brush> Pressed => new("ComboBoxItemBackgroundPressed");

						[ResourceKeyDefinition(typeof(Brush), "ComboBoxItemBackgroundPointerOver")]
						public static ThemeResourceKey<Brush> PointerOver => new("ComboBoxItemBackgroundPointerOver");

						[ResourceKeyDefinition(typeof(Brush), "ComboBoxItemBackgroundDisabled")]
						public static ThemeResourceKey<Brush> Disabled => new("ComboBoxItemBackgroundDisabled");
					}

					public static class BackgroundSelected
					{
						[ResourceKeyDefinition(typeof(Brush), "ComboBoxItemBackgroundSelected")]
						public static ThemeResourceKey<Brush> Default => new("ComboBoxItemBackgroundSelected");

						[ResourceKeyDefinition(typeof(Brush), "ComboBoxItemBackgroundSelectedUnfocused")]
						public static ThemeResourceKey<Brush> Unfocused => new("ComboBoxItemBackgroundSelectedUnfocused");

						[ResourceKeyDefinition(typeof(Brush), "ComboBoxItemBackgroundSelectedPressed")]
						public static ThemeResourceKey<Brush> Pressed => new("ComboBoxItemBackgroundSelectedPressed");

						[ResourceKeyDefinition(typeof(Brush), "ComboBoxItemBackgroundSelectedPointerOver")]
						public static ThemeResourceKey<Brush> PointerOver => new("ComboBoxItemBackgroundSelectedPointerOver");

						[ResourceKeyDefinition(typeof(Brush), "ComboBoxItemBackgroundSelectedDisabled")]
						public static ThemeResourceKey<Brush> Disabled => new("ComboBoxItemBackgroundSelectedDisabled");
					}

					public static class BorderBrush
					{
						[ResourceKeyDefinition(typeof(Brush), "ComboBoxItemBorderBrush")]
						public static ThemeResourceKey<Brush> Default => new("ComboBoxItemBorderBrush");

						[ResourceKeyDefinition(typeof(Brush), "ComboBoxItemBorderBrushPressed")]
						public static ThemeResourceKey<Brush> Pressed => new("ComboBoxItemBorderBrushPressed");

						[ResourceKeyDefinition(typeof(Brush), "ComboBoxItemBorderBrushPointerOver")]
						public static ThemeResourceKey<Brush> PointerOver => new("ComboBoxItemBorderBrushPointerOver");

						[ResourceKeyDefinition(typeof(Brush), "ComboBoxItemBorderBrushDisabled")]
						public static ThemeResourceKey<Brush> Disabled => new("ComboBoxItemBorderBrushDisabled");
					}

					public static class BorderBrushSelected
					{
						[ResourceKeyDefinition(typeof(Brush), "ComboBoxItemBorderBrushSelected")]
						public static ThemeResourceKey<Brush> Default => new("ComboBoxItemBorderBrushSelected");

						[ResourceKeyDefinition(typeof(Brush), "ComboBoxItemBorderBrushSelectedUnfocused")]
						public static ThemeResourceKey<Brush> Unfocused => new("ComboBoxItemBorderBrushSelectedUnfocused");

						[ResourceKeyDefinition(typeof(Brush), "ComboBoxItemBorderBrushSelectedPressed")]
						public static ThemeResourceKey<Brush> Pressed => new("ComboBoxItemBorderBrushSelectedPressed");

						[ResourceKeyDefinition(typeof(Brush), "ComboBoxItemBorderBrushSelectedPointerOver")]
						public static ThemeResourceKey<Brush> PointerOver => new("ComboBoxItemBorderBrushSelectedPointerOver");

						[ResourceKeyDefinition(typeof(Brush), "ComboBoxItemBorderBrushSelectedDisabled")]
						public static ThemeResourceKey<Brush> Disabled => new("ComboBoxItemBorderBrushSelectedDisabled");
					}

					public static class PillFill
					{
						[ResourceKeyDefinition(typeof(Brush), "ComboBoxItemPillFillBrush")]
						public static ThemeResourceKey<Brush> Default => new("ComboBoxItemPillFillBrush");
					}
				}

				public static class DropDown
				{
					public static class GlyphForeground
					{
						[ResourceKeyDefinition(typeof(Brush), "ComboBoxDropDownGlyphForeground")]
						public static ThemeResourceKey<Brush> Default => new("ComboBoxDropDownGlyphForeground");

						[ResourceKeyDefinition(typeof(Brush), "ComboBoxDropDownGlyphForegroundFocusedPressed")]
						public static ThemeResourceKey<Brush> FocusedPressed => new("ComboBoxDropDownGlyphForegroundFocusedPressed");

						[ResourceKeyDefinition(typeof(Brush), "ComboBoxDropDownGlyphForegroundFocused")]
						public static ThemeResourceKey<Brush> Focused => new("ComboBoxDropDownGlyphForegroundFocused");

						[ResourceKeyDefinition(typeof(Brush), "ComboBoxDropDownGlyphForegroundDisabled")]
						public static ThemeResourceKey<Brush> Disabled => new("ComboBoxDropDownGlyphForegroundDisabled");
					}

					public static class Foreground
					{
						[ResourceKeyDefinition(typeof(Brush), "ComboBoxDropDownForeground")]
						public static ThemeResourceKey<Brush> Default => new("ComboBoxDropDownForeground");
					}

					public static class BorderBrush
					{
						[ResourceKeyDefinition(typeof(Brush), "ComboBoxDropDownBorderBrush")]
						public static ThemeResourceKey<Brush> Default => new("ComboBoxDropDownBorderBrush");
					}

					public static class Background
					{
						[ResourceKeyDefinition(typeof(Brush), "ComboBoxDropDownBackground")]
						public static ThemeResourceKey<Brush> Default => new("ComboBoxDropDownBackground");

						[ResourceKeyDefinition(typeof(Brush), "ComboBoxDropDownBackgroundPointerOver")]
						public static ThemeResourceKey<Brush> PointerOver => new("ComboBoxDropDownBackgroundPointerOver");

						[ResourceKeyDefinition(typeof(Brush), "ComboBoxDropDownBackgroundPointerPressed")]
						public static ThemeResourceKey<Brush> PointerPressed => new("ComboBoxDropDownBackgroundPointerPressed");

						[ResourceKeyDefinition(typeof(Brush), "ComboBoxDropDownBackgroundPressed")]
						public static ThemeResourceKey<Brush> Pressed => new("ComboBoxDropDownBackgroundPressed");

						[ResourceKeyDefinition(typeof(Brush), "ComboBoxDropDownBackgroundFocused")]
						public static ThemeResourceKey<Brush> Focused => new("ComboBoxDropDownBackgroundFocused");

						[ResourceKeyDefinition(typeof(Brush), "ComboBoxDropDownBackgroundFocusedPressed")]
						public static ThemeResourceKey<Brush> FocusedPressed => new("ComboBoxDropDownBackgroundFocusedPressed");
					}
				}

				public static class EditableDropDown
				{
					public static class GlyphForeground
					{
						[ResourceKeyDefinition(typeof(Brush), "ComboBoxEditableDropDownGlyphForeground")]
						public static ThemeResourceKey<Brush> Default => new("ComboBoxEditableDropDownGlyphForeground");
					}
				}
			}

			public static class Styles
			{
				[ResourceKeyDefinition(typeof(Style), "ComboBoxStyle", TargetType = typeof(ComboBox))]
				public static StaticResourceKey<Style> Default => new("ComboBoxStyle");
			}
		}
	}
}
