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
						public static ResourceValue<Brush> FocusedPressed => new("ComboBoxForegroundFocusedPressed", true);

						[ResourceKeyDefinition(typeof(Brush), "ComboBoxUpperPlaceHolderForeground")]
						public static ResourceValue<Brush> UpperPlaceHolder => new("ComboBoxUpperPlaceHolderForeground", true);

						[ResourceKeyDefinition(typeof(Brush), "ComboBoxPlaceHolderForeground")]
						public static ResourceValue<Brush> PlaceHolder => new("ComboBoxPlaceHolderForeground", true);

						[ResourceKeyDefinition(typeof(Brush), "ComboBoxPlaceHolderForegroundPointerOver")]
						public static ResourceValue<Brush> PlaceHolderPointerOver => new("ComboBoxPlaceHolderForegroundPointerOver", true);

						[ResourceKeyDefinition(typeof(Brush), "ComboBoxPlaceHolderForegroundPressed")]
						public static ResourceValue<Brush> PlaceHolderPressed => new("ComboBoxPlaceHolderForegroundPressed", true);

						[ResourceKeyDefinition(typeof(Brush), "ComboBoxPlaceHolderForegroundDisabled")]
						public static ResourceValue<Brush> PlaceHolderDisabled => new("ComboBoxPlaceHolderForegroundDisabled", true);

						[ResourceKeyDefinition(typeof(Brush), "ComboBoxPlaceHolderForegroundFocused")]
						public static ResourceValue<Brush> PlaceHolderFocused => new("ComboBoxPlaceHolderForegroundFocused", true);

						[ResourceKeyDefinition(typeof(Brush), "ComboBoxPlaceHolderForegroundFocusedPressed")]
						public static ResourceValue<Brush> PlaceHolderFocusedPressed => new("ComboBoxPlaceHolderForegroundFocusedPressed", true);

						[ResourceKeyDefinition(typeof(Brush), "ComboBoxLeadingIconForeground")]
						public static ResourceValue<Brush> LeadingIcon => new("ComboBoxLeadingIconForeground", true);

						[ResourceKeyDefinition(typeof(Brush), "ComboBoxLeadingIconForegroundDisabled")]
						public static ResourceValue<Brush> LeadingIconDisabled => new("ComboBoxLeadingIconForegroundDisabled", true);

						[ResourceKeyDefinition(typeof(Brush), "ComboBoxArrowForeground")]
						public static ResourceValue<Brush> Arrow => new("ComboBoxArrowForeground", true);

						[ResourceKeyDefinition(typeof(Brush), "ComboBoxArrowForegroundOpened")]
						public static ResourceValue<Brush> ArrowOpened => new("ComboBoxArrowForegroundOpened", true);
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
						public static ResourceValue<Brush> Disabled => new("ComboBoxItemForegroundDisabled", true);

						[ResourceKeyDefinition(typeof(Brush), "ComboBoxItemForegroundSelected")]
						public static ResourceValue<Brush> Selected => new("ComboBoxItemForegroundSelected", true);

						[ResourceKeyDefinition(typeof(Brush), "ComboBoxItemForegroundSelectedUnfocused")]
						public static ResourceValue<Brush> SelectedUnfocused => new("ComboBoxItemForegroundSelectedUnfocused", true);

						[ResourceKeyDefinition(typeof(Brush), "ComboBoxItemForegroundSelectedPressed")]
						public static ResourceValue<Brush> SelectedPressed => new("ComboBoxItemForegroundSelectedPressed", true);

						[ResourceKeyDefinition(typeof(Brush), "ComboBoxItemForegroundSelectedPointerOver")]
						public static ResourceValue<Brush> SelectedPointerOver => new("ComboBoxItemForegroundSelectedPointerOver", true);

						[ResourceKeyDefinition(typeof(Brush), "ComboBoxItemForegroundSelectedDisabled")]
						public static ResourceValue<Brush> SelectedDisabled => new("ComboBoxItemForegroundSelectedDisabled", true);
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
						public static ResourceValue<Brush> Disabled => new("ComboBoxItemBackgroundDisabled", true);

						[ResourceKeyDefinition(typeof(Brush), "ComboBoxItemBackgroundSelected")]
						public static ResourceValue<Brush> Selected => new("ComboBoxItemBackgroundSelected", true);

						[ResourceKeyDefinition(typeof(Brush), "ComboBoxItemBackgroundSelectedUnfocused")]
						public static ResourceValue<Brush> SelectedUnfocused => new("ComboBoxItemBackgroundSelectedUnfocused", true);

						[ResourceKeyDefinition(typeof(Brush), "ComboBoxItemBackgroundSelectedPressed")]
						public static ResourceValue<Brush> SelectedPressed => new("ComboBoxItemBackgroundSelectedPressed", true);

						[ResourceKeyDefinition(typeof(Brush), "ComboBoxItemBackgroundSelectedPointerOver")]
						public static ResourceValue<Brush> SelectedPointerOver => new("ComboBoxItemBackgroundSelectedPointerOver", true);

						[ResourceKeyDefinition(typeof(Brush), "ComboBoxItemBackgroundSelectedDisabled")]
						public static ResourceValue<Brush> SelectedDisabled => new("ComboBoxItemBackgroundSelectedDisabled", true);
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
						public static ResourceValue<Brush> Disabled => new("ComboBoxItemBorderBrushDisabled", true);

						[ResourceKeyDefinition(typeof(Brush), "ComboBoxItemBorderBrushSelected")]
						public static ResourceValue<Brush> Selected => new("ComboBoxItemBorderBrushSelected", true);

						[ResourceKeyDefinition(typeof(Brush), "ComboBoxItemBorderBrushSelectedUnfocused")]
						public static ResourceValue<Brush> SelectedUnfocused => new("ComboBoxItemBorderBrushSelectedUnfocused", true);

						[ResourceKeyDefinition(typeof(Brush), "ComboBoxItemBorderBrushSelectedPressed")]
						public static ResourceValue<Brush> SelectedPressed => new("ComboBoxItemBorderBrushSelectedPressed", true);

						[ResourceKeyDefinition(typeof(Brush), "ComboBoxItemBorderBrushSelectedPointerOver")]
						public static ResourceValue<Brush> SelectedPointerOver => new("ComboBoxItemBorderBrushSelectedPointerOver", true);

						[ResourceKeyDefinition(typeof(Brush), "ComboBoxItemBorderBrushSelectedDisabled")]
						public static ResourceValue<Brush> SelectedDisabled => new("ComboBoxItemBorderBrushSelectedDisabled", true);
					}

					public static class PillFill
					{
						[ResourceKeyDefinition(typeof(Brush), "ComboBoxItemPillFillBrush")]
						public static ThemeResourceKey<Brush> Default => new("ComboBoxItemPillFillBrush");
					}
				}

				public static class DropDown
				{
					public static class Foreground
					{
						[ResourceKeyDefinition(typeof(Brush), "ComboBoxDropDownForeground")]
						public static ResourceValue<Brush> Default => new("ComboBoxDropDownForeground", true);

						[ResourceKeyDefinition(typeof(Brush), "ComboBoxDropDownGlyphForeground")]
						public static ResourceValue<Brush> Glyph => new("ComboBoxDropDownGlyphForeground", true);

						[ResourceKeyDefinition(typeof(Brush), "ComboBoxDropDownGlyphForegroundFocusedPressed")]
						public static ResourceValue<Brush> GlyphFocusedPressed => new("ComboBoxDropDownGlyphForegroundFocusedPressed", true);

						[ResourceKeyDefinition(typeof(Brush), "ComboBoxDropDownGlyphForegroundFocused")]
						public static ResourceValue<Brush> GlyphFocused => new("ComboBoxDropDownGlyphForegroundFocused", true);

						[ResourceKeyDefinition(typeof(Brush), "ComboBoxDropDownGlyphForegroundDisabled")]
						public static ResourceValue<Brush> GlyphDisabled => new("ComboBoxDropDownGlyphForegroundDisabled", true);
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
					public static class Foreground
					{
						[ResourceKeyDefinition(typeof(Brush), "ComboBoxEditableDropDownGlyphForeground")]
						public static ResourceValue<Brush> Glyph => new("ComboBoxEditableDropDownGlyphForeground", true);
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
