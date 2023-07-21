using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Media;
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
						public static ResourceValue<Brush> Default => new("ComboBoxForeground", true);

						[ResourceKeyDefinition(typeof(Brush), "ComboBoxForegroundPointerOver")]
						public static ResourceValue<Brush> PointerOver => new("ComboBoxForegroundPointerOver", true);

						[ResourceKeyDefinition(typeof(Brush), "ComboBoxForegroundPressed")]
						public static ResourceValue<Brush> Pressed => new("ComboBoxForegroundPressed", true);

						[ResourceKeyDefinition(typeof(Brush), "ComboBoxForegroundDisabled")]
						public static ResourceValue<Brush> Disabled => new("ComboBoxForegroundDisabled", true);

						[ResourceKeyDefinition(typeof(Brush), "ComboBoxForegroundFocused")]
						public static ResourceValue<Brush> Focused => new("ComboBoxForegroundFocused", true);

						[ResourceKeyDefinition(typeof(Brush), "ComboBoxForegroundFocusedPressed")]
						public static ResourceValue<Brush> FocusedPressed => new("ComboBoxForegroundFocusedPressed", true);
					}

					public static class Background
					{
						[ResourceKeyDefinition(typeof(Brush), "ComboBoxBackground")]
						public static ResourceValue<Brush> Default => new("ComboBoxBackground", true);

						[ResourceKeyDefinition(typeof(Brush), "ComboBoxBackgroundPointerOver")]
						public static ResourceValue<Brush> PointerOver => new("ComboBoxBackgroundPointerOver", true);

						[ResourceKeyDefinition(typeof(Brush), "ComboBoxBackgroundPressed")]
						public static ResourceValue<Brush> Pressed => new("ComboBoxBackgroundPressed", true);

						[ResourceKeyDefinition(typeof(Brush), "ComboBoxBackgroundDisabled")]
						public static ResourceValue<Brush> Disabled => new("ComboBoxBackgroundDisabled", true);

						[ResourceKeyDefinition(typeof(Brush), "ComboBoxBackgroundUnfocused")]
						public static ResourceValue<Brush> Unfocused => new("ComboBoxBackgroundUnfocused", true);

						[ResourceKeyDefinition(typeof(Brush), "ComboBoxBackgroundFocused")]
						public static ResourceValue<Brush> Focused => new("ComboBoxBackgroundFocused", true);

						[ResourceKeyDefinition(typeof(Brush), "ComboBoxBackgroundFocusedPressed")]
						public static ResourceValue<Brush> FocusedPressed => new("ComboBoxBackgroundFocusedPressed", true);
					}

					public static class BorderBrush
					{
						[ResourceKeyDefinition(typeof(Brush), "ComboBoxBorderBrush")]
						public static ResourceValue<Brush> Default => new("ComboBoxBorderBrush", true);

						[ResourceKeyDefinition(typeof(Brush), "ComboBoxBorderBrushPressed")]
						public static ResourceValue<Brush> Pressed => new("ComboBoxBorderBrushPressed", true);

						[ResourceKeyDefinition(typeof(Brush), "ComboBoxBorderBrushPointerOver")]
						public static ResourceValue<Brush> PointerOver => new("ComboBoxBorderBrushPointerOver", true);

						[ResourceKeyDefinition(typeof(Brush), "ComboBoxBorderBrushDisabled")]
						public static ResourceValue<Brush> Disabled => new("ComboBoxBorderBrushDisabled", true);

						[ResourceKeyDefinition(typeof(Brush), "ComboBoxBorderBrushOpened")]
						public static ResourceValue<Brush> Opened => new("ComboBoxBorderBrushOpened", true);
					}

					public static class PlaceHolderForeground
					{
						[ResourceKeyDefinition(typeof(Brush), "ComboBoxPlaceHolderForeground")]
						public static ResourceValue<Brush> Default => new("ComboBoxPlaceHolderForeground", true);

						[ResourceKeyDefinition(typeof(Brush), "ComboBoxPlaceHolderForegroundPointerOver")]
						public static ResourceValue<Brush> PointerOver => new("ComboBoxPlaceHolderForegroundPointerOver", true);

						[ResourceKeyDefinition(typeof(Brush), "ComboBoxPlaceHolderForegroundPressed")]
						public static ResourceValue<Brush> Pressed => new("ComboBoxPlaceHolderForegroundPressed", true);

						[ResourceKeyDefinition(typeof(Brush), "ComboBoxPlaceHolderForegroundDisabled")]
						public static ResourceValue<Brush> Disabled => new("ComboBoxPlaceHolderForegroundDisabled", true);

						[ResourceKeyDefinition(typeof(Brush), "ComboBoxPlaceHolderForegroundFocused")]
						public static ResourceValue<Brush> Focused => new("ComboBoxPlaceHolderForegroundFocused", true);

						[ResourceKeyDefinition(typeof(Brush), "ComboBoxPlaceHolderForegroundFocusedPressed")]
						public static ResourceValue<Brush> FocusedPressed => new("ComboBoxPlaceHolderForegroundFocusedPressed", true);
					}

					public static class LeadingIconForeground
					{
						[ResourceKeyDefinition(typeof(Brush), "ComboBoxLeadingIconForeground")]
						public static ResourceValue<Brush> Default => new("ComboBoxLeadingIconForeground", true);

						[ResourceKeyDefinition(typeof(Brush), "ComboBoxLeadingIconForegroundDisabled")]
						public static ResourceValue<Brush> Disabled => new("ComboBoxLeadingIconForegroundDisabled", true);
					}

					public static class ArrowForeground
					{
						[ResourceKeyDefinition(typeof(Brush), "ComboBoxArrowForeground")]
						public static ResourceValue<Brush> Default => new("ComboBoxArrowForeground", true);

						[ResourceKeyDefinition(typeof(Brush), "ComboBoxArrowForegroundOpened")]
						public static ResourceValue<Brush> Opened => new("ComboBoxArrowForegroundOpened", true);
					}

					public static class UpperPlaceHolderForeground
					{
						[ResourceKeyDefinition(typeof(Brush), "ComboBoxUpperPlaceHolderForeground")]
						public static ResourceValue<Brush> Default => new("ComboBoxUpperPlaceHolderForeground", true);
					}
				}

				public static class ComboBoxItem
				{
					public static class Foreground
					{
						[ResourceKeyDefinition(typeof(Brush), "ComboBoxItemForeground")]
						public static ResourceValue<Brush> Default => new("ComboBoxItemForeground", true);

						[ResourceKeyDefinition(typeof(Brush), "ComboBoxItemForegroundPressed")]
						public static ResourceValue<Brush> Pressed => new("ComboBoxItemForegroundPressed", true);

						[ResourceKeyDefinition(typeof(Brush), "ComboBoxItemForegroundPointerOver")]
						public static ResourceValue<Brush> PointerOver => new("ComboBoxItemForegroundPointerOver", true);

						[ResourceKeyDefinition(typeof(Brush), "ComboBoxItemForegroundDisabled")]
						public static ResourceValue<Brush> Disabled => new("ComboBoxItemForegroundDisabled", true);
					}

					public static class ForegroundSelected
					{
						[ResourceKeyDefinition(typeof(Brush), "ComboBoxItemForegroundSelected")]
						public static ResourceValue<Brush> Default => new("ComboBoxItemForegroundSelected", true);

						[ResourceKeyDefinition(typeof(Brush), "ComboBoxItemForegroundSelectedUnfocused")]
						public static ResourceValue<Brush> Unfocused => new("ComboBoxItemForegroundSelectedUnfocused", true);

						[ResourceKeyDefinition(typeof(Brush), "ComboBoxItemForegroundSelectedPressed")]
						public static ResourceValue<Brush> Pressed => new("ComboBoxItemForegroundSelectedPressed", true);

						[ResourceKeyDefinition(typeof(Brush), "ComboBoxItemForegroundSelectedPointerOver")]
						public static ResourceValue<Brush> PointerOver => new("ComboBoxItemForegroundSelectedPointerOver", true);

						[ResourceKeyDefinition(typeof(Brush), "ComboBoxItemForegroundSelectedDisabled")]
						public static ResourceValue<Brush> Disabled => new("ComboBoxItemForegroundSelectedDisabled", true);
					}

					public static class Background
					{
						[ResourceKeyDefinition(typeof(Brush), "ComboBoxItemBackground")]
						public static ResourceValue<Brush> Default => new("ComboBoxItemBackground", true);

						[ResourceKeyDefinition(typeof(Brush), "ComboBoxItemBackgroundPressed")]
						public static ResourceValue<Brush> Pressed => new("ComboBoxItemBackgroundPressed", true);

						[ResourceKeyDefinition(typeof(Brush), "ComboBoxItemBackgroundPointerOver")]
						public static ResourceValue<Brush> PointerOver => new("ComboBoxItemBackgroundPointerOver", true);

						[ResourceKeyDefinition(typeof(Brush), "ComboBoxItemBackgroundDisabled")]
						public static ResourceValue<Brush> Disabled => new("ComboBoxItemBackgroundDisabled", true);
					}

					public static class BackgroundSelected
					{
						[ResourceKeyDefinition(typeof(Brush), "ComboBoxItemBackgroundSelected")]
						public static ResourceValue<Brush> Default => new("ComboBoxItemBackgroundSelected", true);

						[ResourceKeyDefinition(typeof(Brush), "ComboBoxItemBackgroundSelectedUnfocused")]
						public static ResourceValue<Brush> Unfocused => new("ComboBoxItemBackgroundSelectedUnfocused", true);

						[ResourceKeyDefinition(typeof(Brush), "ComboBoxItemBackgroundSelectedPressed")]
						public static ResourceValue<Brush> Pressed => new("ComboBoxItemBackgroundSelectedPressed", true);

						[ResourceKeyDefinition(typeof(Brush), "ComboBoxItemBackgroundSelectedPointerOver")]
						public static ResourceValue<Brush> PointerOver => new("ComboBoxItemBackgroundSelectedPointerOver", true);

						[ResourceKeyDefinition(typeof(Brush), "ComboBoxItemBackgroundSelectedDisabled")]
						public static ResourceValue<Brush> Disabled => new("ComboBoxItemBackgroundSelectedDisabled", true);
					}

					public static class BorderBrush
					{
						[ResourceKeyDefinition(typeof(Brush), "ComboBoxItemBorderBrush")]
						public static ResourceValue<Brush> Default => new("ComboBoxItemBorderBrush", true);

						[ResourceKeyDefinition(typeof(Brush), "ComboBoxItemBorderBrushPressed")]
						public static ResourceValue<Brush> Pressed => new("ComboBoxItemBorderBrushPressed", true);

						[ResourceKeyDefinition(typeof(Brush), "ComboBoxItemBorderBrushPointerOver")]
						public static ResourceValue<Brush> PointerOver => new("ComboBoxItemBorderBrushPointerOver", true);

						[ResourceKeyDefinition(typeof(Brush), "ComboBoxItemBorderBrushDisabled")]
						public static ResourceValue<Brush> Disabled => new("ComboBoxItemBorderBrushDisabled", true);
					}

					public static class BorderBrushSelected
					{
						[ResourceKeyDefinition(typeof(Brush), "ComboBoxItemBorderBrushSelected")]
						public static ResourceValue<Brush> Default => new("ComboBoxItemBorderBrushSelected", true);

						[ResourceKeyDefinition(typeof(Brush), "ComboBoxItemBorderBrushSelectedUnfocused")]
						public static ResourceValue<Brush> Unfocused => new("ComboBoxItemBorderBrushSelectedUnfocused", true);

						[ResourceKeyDefinition(typeof(Brush), "ComboBoxItemBorderBrushSelectedPressed")]
						public static ResourceValue<Brush> Pressed => new("ComboBoxItemBorderBrushSelectedPressed", true);

						[ResourceKeyDefinition(typeof(Brush), "ComboBoxItemBorderBrushSelectedPointerOver")]
						public static ResourceValue<Brush> PointerOver => new("ComboBoxItemBorderBrushSelectedPointerOver", true);

						[ResourceKeyDefinition(typeof(Brush), "ComboBoxItemBorderBrushSelectedDisabled")]
						public static ResourceValue<Brush> Disabled => new("ComboBoxItemBorderBrushSelectedDisabled", true);
					}

					public static class PillFill
					{
						[ResourceKeyDefinition(typeof(Brush), "ComboBoxItemPillFillBrush")]
						public static ResourceValue<Brush> Default => new("ComboBoxItemPillFillBrush", true);
					}
				}

				public static class DropDown
				{
					public static class GlyphForeground
					{
						[ResourceKeyDefinition(typeof(Brush), "ComboBoxDropDownGlyphForeground")]
						public static ResourceValue<Brush> Default => new("ComboBoxDropDownGlyphForeground", true);

						[ResourceKeyDefinition(typeof(Brush), "ComboBoxDropDownGlyphForegroundFocusedPressed")]
						public static ResourceValue<Brush> FocusedPressed => new("ComboBoxDropDownGlyphForegroundFocusedPressed", true);

						[ResourceKeyDefinition(typeof(Brush), "ComboBoxDropDownGlyphForegroundFocused")]
						public static ResourceValue<Brush> Focused => new("ComboBoxDropDownGlyphForegroundFocused", true);

						[ResourceKeyDefinition(typeof(Brush), "ComboBoxDropDownGlyphForegroundDisabled")]
						public static ResourceValue<Brush> Disabled => new("ComboBoxDropDownGlyphForegroundDisabled", true);
					}

					public static class Foreground
					{
						[ResourceKeyDefinition(typeof(Brush), "ComboBoxDropDownForeground")]
						public static ResourceValue<Brush> Default => new("ComboBoxDropDownForeground", true);
					}

					public static class BorderBrush
					{
						[ResourceKeyDefinition(typeof(Brush), "ComboBoxDropDownBorderBrush")]
						public static ResourceValue<Brush> Default => new("ComboBoxDropDownBorderBrush", true);
					}

					public static class Background
					{
						[ResourceKeyDefinition(typeof(Brush), "ComboBoxDropDownBackground")]
						public static ResourceValue<Brush> Default => new("ComboBoxDropDownBackground", true);

						[ResourceKeyDefinition(typeof(Brush), "ComboBoxDropDownBackgroundPointerOver")]
						public static ResourceValue<Brush> PointerOver => new("ComboBoxDropDownBackgroundPointerOver", true);

						[ResourceKeyDefinition(typeof(Brush), "ComboBoxDropDownBackgroundPointerPressed")]
						public static ResourceValue<Brush> PointerPressed => new("ComboBoxDropDownBackgroundPointerPressed", true);

						[ResourceKeyDefinition(typeof(Brush), "ComboBoxDropDownBackgroundPressed")]
						public static ResourceValue<Brush> Pressed => new("ComboBoxDropDownBackgroundPressed", true);

						[ResourceKeyDefinition(typeof(Brush), "ComboBoxDropDownBackgroundFocused")]
						public static ResourceValue<Brush> Focused => new("ComboBoxDropDownBackgroundFocused", true);

						[ResourceKeyDefinition(typeof(Brush), "ComboBoxDropDownBackgroundFocusedPressed")]
						public static ResourceValue<Brush> FocusedPressed => new("ComboBoxDropDownBackgroundFocusedPressed", true);
					}
				}

				public static class EditableDropDown
				{
					public static class GlyphForeground
					{
						[ResourceKeyDefinition(typeof(Brush), "ComboBoxEditableDropDownGlyphForeground")]
						public static ResourceValue<Brush> Default => new("ComboBoxEditableDropDownGlyphForeground", true);
					}
				}
			}

			public static class Styles
			{
				[ResourceKeyDefinition(typeof(Style), "ComboBoxStyle", TargetType = typeof(ComboBox))]
				public static ResourceValue<Style> Default => new("ComboBoxStyle");
			}
		}
	}
}
