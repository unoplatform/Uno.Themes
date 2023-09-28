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

					public static class PlaceholderForeground
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

					public static class UpperPlaceholderForeground
					{
						[ResourceKeyDefinition(typeof(Brush), "ComboBoxUpperPlaceHolderForeground")]
						public static ThemeResourceKey<Brush> Default => new("ComboBoxUpperPlaceHolderForeground");

						[ResourceKeyDefinition(typeof(Brush), "ComboBoxUpperPlaceHolderForegroundPointerOver")]
						public static ThemeResourceKey<Brush> PointerOver => new("ComboBoxUpperPlaceHolderForegroundPointerOver");

						[ResourceKeyDefinition(typeof(Brush), "ComboBoxUpperPlaceHolderForegroundPressed")]
						public static ThemeResourceKey<Brush> Pressed => new("ComboBoxUpperPlaceHolderForegroundPressed");

						[ResourceKeyDefinition(typeof(Brush), "ComboBoxUpperPlaceHolderForegroundDisabled")]
						public static ThemeResourceKey<Brush> Disabled => new("ComboBoxUpperPlaceHolderForegroundDisabled");

						[ResourceKeyDefinition(typeof(Brush), "ComboBoxUpperPlaceHolderForegroundFocused")]
						public static ThemeResourceKey<Brush> Focused => new("ComboBoxUpperPlaceHolderForegroundFocused");

						[ResourceKeyDefinition(typeof(Brush), "ComboBoxUpperPlaceHolderForegroundFocusedPressed")]
						public static ThemeResourceKey<Brush> FocusedPressed => new("ComboBoxUpperPlaceHolderForegroundFocusedPressed");

						[ResourceKeyDefinition(typeof(Brush), "ComboBoxUpperPlaceHolderForegroundOpened")]
						public static ThemeResourceKey<Brush> Opened => new("ComboBoxUpperPlaceHolderForegroundOpened");
					}

					public static class LeadingIcon
					{
						public static class Foreground
						{
							[ResourceKeyDefinition(typeof(Brush), "ComboBoxLeadingIconForeground")]
							public static ThemeResourceKey<Brush> Default => new("ComboBoxLeadingIconForeground");

							[ResourceKeyDefinition(typeof(Brush), "ComboBoxLeadingIconForegroundDisabled")]
							public static ThemeResourceKey<Brush> Disabled => new("ComboBoxLeadingIconForegroundDisabled");
						}

						[ResourceKeyDefinition(typeof(double), "ComboBoxLeadingIconWidth")]
						public static ThemeResourceKey<double> Width => new("ComboBoxLeadingIconWidth");

						[ResourceKeyDefinition(typeof(Thickness), "ComboBoxLeadingIconMargin")]
						public static ThemeResourceKey<Thickness> Margin => new("ComboBoxLeadingIconMargin");
					}

					public static class ArrowForeground
					{
						[ResourceKeyDefinition(typeof(Brush), "ComboBoxArrowForeground")]
						public static ThemeResourceKey<Brush> Default => new("ComboBoxArrowForeground");

						[ResourceKeyDefinition(typeof(Brush), "ComboBoxArrowForegroundOpened")]
						public static ThemeResourceKey<Brush> Opened => new("ComboBoxArrowForegroundOpened");
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

					public static class Item
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

							[ResourceKeyDefinition(typeof(Brush), "ComboBoxItemForegroundSelected")]
							public static ThemeResourceKey<Brush> Selected => new("ComboBoxItemForegroundSelected");

							[ResourceKeyDefinition(typeof(Brush), "ComboBoxItemForegroundSelectedUnfocused")]
							public static ThemeResourceKey<Brush> SelectedUnfocused => new("ComboBoxItemForegroundSelectedUnfocused");

							[ResourceKeyDefinition(typeof(Brush), "ComboBoxItemForegroundSelectedPressed")]
							public static ThemeResourceKey<Brush> SelectedPressed => new("ComboBoxItemForegroundSelectedPressed");

							[ResourceKeyDefinition(typeof(Brush), "ComboBoxItemForegroundSelectedPointerOver")]
							public static ThemeResourceKey<Brush> SelectedPointerOver => new("ComboBoxItemForegroundSelectedPointerOver");

							[ResourceKeyDefinition(typeof(Brush), "ComboBoxItemForegroundSelectedDisabled")]
							public static ThemeResourceKey<Brush> SelectedDisabled => new("ComboBoxItemForegroundSelectedDisabled");
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

							[ResourceKeyDefinition(typeof(Brush), "ComboBoxItemBackgroundSelected")]
							public static ThemeResourceKey<Brush> Selected => new("ComboBoxItemBackgroundSelected");

							[ResourceKeyDefinition(typeof(Brush), "ComboBoxItemBackgroundSelectedUnfocused")]
							public static ThemeResourceKey<Brush> SelectedUnfocused => new("ComboBoxItemBackgroundSelectedUnfocused");

							[ResourceKeyDefinition(typeof(Brush), "ComboBoxItemBackgroundSelectedPressed")]
							public static ThemeResourceKey<Brush> SelectedPressed => new("ComboBoxItemBackgroundSelectedPressed");

							[ResourceKeyDefinition(typeof(Brush), "ComboBoxItemBackgroundSelectedPointerOver")]
							public static ThemeResourceKey<Brush> SelectedPointerOver => new("ComboBoxItemBackgroundSelectedPointerOver");

							[ResourceKeyDefinition(typeof(Brush), "ComboBoxItemBackgroundSelectedDisabled")]
							public static ThemeResourceKey<Brush> SelectedDisabled => new("ComboBoxItemBackgroundSelectedDisabled");
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

							[ResourceKeyDefinition(typeof(Brush), "ComboBoxItemBorderBrushSelected")]
							public static ThemeResourceKey<Brush> Selected => new("ComboBoxItemBorderBrushSelected");

							[ResourceKeyDefinition(typeof(Brush), "ComboBoxItemBorderBrushSelectedUnfocused")]
							public static ThemeResourceKey<Brush> SelectedUnfocused => new("ComboBoxItemBorderBrushSelectedUnfocused");

							[ResourceKeyDefinition(typeof(Brush), "ComboBoxItemBorderBrushSelectedPressed")]
							public static ThemeResourceKey<Brush> SelectedPressed => new("ComboBoxItemBorderBrushSelectedPressed");

							[ResourceKeyDefinition(typeof(Brush), "ComboBoxItemBorderBrushSelectedPointerOver")]
							public static ThemeResourceKey<Brush> SelectedPointerOver => new("ComboBoxItemBorderBrushSelectedPointerOver");

							[ResourceKeyDefinition(typeof(Brush), "ComboBoxItemBorderBrushSelectedDisabled")]
							public static ThemeResourceKey<Brush> SelectedDisabled => new("ComboBoxItemBorderBrushSelectedDisabled");
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
							public static ThemeResourceKey<Brush> Default => new("ComboBoxDropDownForeground");
						}

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

						public static class EditableGlyphForeground
						{
							[ResourceKeyDefinition(typeof(Brush), "ComboBoxEditableDropDownGlyphForeground")]
							public static ThemeResourceKey<Brush> Default => new("ComboBoxEditableDropDownGlyphForeground");
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

					[ResourceKeyDefinition(typeof(CornerRadius), "ComboBoxCornerRadius")]
					public static ThemeResourceKey<CornerRadius> CornerRadius => new("ComboBoxCornerRadius");

					[ResourceKeyDefinition(typeof(double), "ComboBoxHeight")]
					public static ThemeResourceKey<double> Height => new("ComboBoxHeight");

					[ResourceKeyDefinition(typeof(double), "ComboBoxMinHeight")]
					public static ThemeResourceKey<double> MinHeight => new("ComboBoxMinHeight");

					[ResourceKeyDefinition(typeof(double), "ComboBoxDownGlyphWidth")]
					public static ThemeResourceKey<double> DownGlyphWidth => new("ComboBoxDownGlyphWidth");

					[ResourceKeyDefinition(typeof(double), "ComboBoxUpGlyphWidth")]
					public static ThemeResourceKey<double> UpGlyphWidth => new("ComboBoxUpGlyphWidth");

					[ResourceKeyDefinition(typeof(double), "ComboBoxDownGlyphHeight")]
					public static ThemeResourceKey<double> DownGlyphHeight => new("ComboBoxDownGlyphHeight");

					[ResourceKeyDefinition(typeof(double), "ComboBoxUpGlyphHeight")]
					public static ThemeResourceKey<double> UpGlyphHeight => new("ComboBoxUpGlyphHeight");

					[ResourceKeyDefinition(typeof(Thickness), "ComboBoxDownGlyphMargin")]
					public static ThemeResourceKey<Thickness> DownGlyphMargin => new("ComboBoxDownGlyphMargin");

					[ResourceKeyDefinition(typeof(Thickness), "ComboBoxUpGlyphMargin")]
					public static ThemeResourceKey<Thickness> UpGlyphMargin => new("ComboBoxUpGlyphMargin");

					[ResourceKeyDefinition(typeof(Thickness), "ComboBoxBorderThickness")]
					public static ThemeResourceKey<Thickness> BorderThickness => new("ComboBoxBorderThickness");

					[ResourceKeyDefinition(typeof(Thickness), "ComboBoxOpenedBorderThickness")]
					public static ThemeResourceKey<Thickness> OpenedBorderThickness => new("ComboBoxOpenedBorderThickness");

					[ResourceKeyDefinition(typeof(Thickness), "ComboBoxPadding")]
					public static ThemeResourceKey<Thickness> Padding => new("ComboBoxPadding");
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
