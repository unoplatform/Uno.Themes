using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Media;
using Uno.Extensions.Markup;
using Uno.Extensions.Markup.Internals;

namespace Uno.Themes.Markup;

public static partial class Theme
{
	public static partial class NavigationViewItem
	{
		public static partial class Resources
		{
			public static partial class Default
			{
				public static partial class Background
				{
					[ResourceKeyDefinition(typeof(Brush), "NavigationViewItemBackground")]
					public static ThemeResourceKey<Brush> Default => new("NavigationViewItemBackground");

					[ResourceKeyDefinition(typeof(Brush), "NavigationViewItemBackgroundPointerOver")]
					public static ThemeResourceKey<Brush> PointerOver => new("NavigationViewItemBackgroundPointerOver");

					[ResourceKeyDefinition(typeof(Brush), "NavigationViewItemBackgroundPressed")]
					public static ThemeResourceKey<Brush> Pressed => new("NavigationViewItemBackgroundPressed");

					[ResourceKeyDefinition(typeof(Brush), "NavigationViewItemBackgroundDisabled")]
					public static ThemeResourceKey<Brush> Disabled => new("NavigationViewItemBackgroundDisabled");

					[ResourceKeyDefinition(typeof(Brush), "NavigationViewItemBackgroundSelected")]
					public static ThemeResourceKey<Brush> Selected => new("NavigationViewItemBackgroundSelected");

					[ResourceKeyDefinition(typeof(Brush), "NavigationViewItemBackgroundSelectedPointerOver")]
					public static ThemeResourceKey<Brush> SelectedPointerOver => new("NavigationViewItemBackgroundSelectedPointerOver");

					[ResourceKeyDefinition(typeof(Brush), "NavigationViewItemBackgroundSelectedPressed")]
					public static ThemeResourceKey<Brush> SelectedPressed => new("NavigationViewItemBackgroundSelectedPressed");

					[ResourceKeyDefinition(typeof(Brush), "NavigationViewItemBackgroundSelectedDisabled")]
					public static ThemeResourceKey<Brush> SelectedDisabled => new("NavigationViewItemBackgroundSelectedDisabled");
				}

				public static partial class Foreground
				{
					[ResourceKeyDefinition(typeof(Brush), "NavigationViewItemForeground")]
					public static ThemeResourceKey<Brush> Default => new("NavigationViewItemForeground");

					[ResourceKeyDefinition(typeof(Brush), "NavigationViewItemForegroundPointerOver")]
					public static ThemeResourceKey<Brush> PointerOver => new("NavigationViewItemForegroundPointerOver");

					[ResourceKeyDefinition(typeof(Brush), "NavigationViewItemForegroundPressed")]
					public static ThemeResourceKey<Brush> Pressed => new("NavigationViewItemForegroundPressed");

					[ResourceKeyDefinition(typeof(Brush), "NavigationViewItemForegroundDisabled")]
					public static ThemeResourceKey<Brush> Disabled => new("NavigationViewItemForegroundDisabled");

					[ResourceKeyDefinition(typeof(Brush), "NavigationViewItemForegroundSelected")]
					public static ThemeResourceKey<Brush> Selected => new("NavigationViewItemForegroundSelected");

					[ResourceKeyDefinition(typeof(Brush), "NavigationViewItemForegroundSelectedPointerOver")]
					public static ThemeResourceKey<Brush> SelectedPointerOver => new("NavigationViewItemForegroundSelectedPointerOver");

					[ResourceKeyDefinition(typeof(Brush), "NavigationViewItemForegroundSelectedPressed")]
					public static ThemeResourceKey<Brush> SelectedPressed => new("NavigationViewItemForegroundSelectedPressed");

					[ResourceKeyDefinition(typeof(Brush), "NavigationViewItemForegroundSelectedDisabled")]
					public static ThemeResourceKey<Brush> SelectedDisabled => new("NavigationViewItemForegroundSelectedDisabled");
				}

				public static partial class BorderBrush
				{
					[ResourceKeyDefinition(typeof(Brush), "NavigationViewItemBorderBrush")]
					public static ThemeResourceKey<Brush> Default => new("NavigationViewItemBorderBrush");

					[ResourceKeyDefinition(typeof(Brush), "NavigationViewItemBorderBrushPointerOver")]
					public static ThemeResourceKey<Brush> PointerOver => new("NavigationViewItemBorderBrushPointerOver");

					[ResourceKeyDefinition(typeof(Brush), "NavigationViewItemBorderBrushPressed")]
					public static ThemeResourceKey<Brush> Pressed => new("NavigationViewItemBorderBrushPressed");

					[ResourceKeyDefinition(typeof(Brush), "NavigationViewItemBorderBrushDisabled")]
					public static ThemeResourceKey<Brush> Disabled => new("NavigationViewItemBorderBrushDisabled");

					[ResourceKeyDefinition(typeof(Brush), "NavigationViewItemBorderBrushSelected")]
					public static ThemeResourceKey<Brush> Selected => new("NavigationViewItemBorderBrushSelected");

					[ResourceKeyDefinition(typeof(Brush), "NavigationViewItemBorderBrushSelectedPointerOver")]
					public static ThemeResourceKey<Brush> SelectedPointerOver => new("NavigationViewItemBorderBrushSelectedPointerOver");

					[ResourceKeyDefinition(typeof(Brush), "NavigationViewItemBorderBrushSelectedPressed")]
					public static ThemeResourceKey<Brush> SelectedPressed => new("NavigationViewItemBorderBrushSelectedPressed");

					[ResourceKeyDefinition(typeof(Brush), "NavigationViewItemBorderBrushSelectedDisabled")]
					public static ThemeResourceKey<Brush> SelectedDisabled => new("NavigationViewItemBorderBrushSelectedDisabled");
				}

				public static partial class SeparatorForeground
				{
					[ResourceKeyDefinition(typeof(Brush), "NavigationViewItemSeparatorForeground")]
					public static ThemeResourceKey<Brush> Default => new("NavigationViewItemSeparatorForeground");
				}

				public static partial class HeaderForeground
				{
					[ResourceKeyDefinition(typeof(Brush), "NavigationViewItemHeaderForeground")]
					public static ThemeResourceKey<Brush> Default => new("NavigationViewItemHeaderForeground");
				}

				public static partial class RippleFeedback
				{
					[ResourceKeyDefinition(typeof(Brush), "NavigationViewItemRippleFeedback")]
					public static ThemeResourceKey<Brush> Default => new("NavigationViewItemRippleFeedback");
				}
			}

			public static partial class Top
			{
				public static partial class Foreground
				{
					[ResourceKeyDefinition(typeof(Brush), "TopNavigationViewItemForeground")]
					public static ThemeResourceKey<Brush> Default => new("TopNavigationViewItemForeground");

					[ResourceKeyDefinition(typeof(Brush), "TopNavigationViewItemForegroundPointerOver")]
					public static ThemeResourceKey<Brush> PointerOver => new("TopNavigationViewItemForegroundPointerOver");

					[ResourceKeyDefinition(typeof(Brush), "TopNavigationViewItemForegroundPressed")]
					public static ThemeResourceKey<Brush> Pressed => new("TopNavigationViewItemForegroundPressed");

					[ResourceKeyDefinition(typeof(Brush), "TopNavigationViewItemForegroundDisabled")]
					public static ThemeResourceKey<Brush> Disabled => new("TopNavigationViewItemForegroundDisabled");

					[ResourceKeyDefinition(typeof(Brush), "TopNavigationViewItemForegroundSelected")]
					public static ThemeResourceKey<Brush> Selected => new("TopNavigationViewItemForegroundSelected");

					[ResourceKeyDefinition(typeof(Brush), "TopNavigationViewItemForegroundSelectedPointerOver")]
					public static ThemeResourceKey<Brush> SelectedPointerOver => new("TopNavigationViewItemForegroundSelectedPointerOver");

					[ResourceKeyDefinition(typeof(Brush), "TopNavigationViewItemForegroundSelectedPressed")]
					public static ThemeResourceKey<Brush> SelectedPressed => new("TopNavigationViewItemForegroundSelectedPressed");

					[ResourceKeyDefinition(typeof(Brush), "TopNavigationViewItemForegroundSelectedDisabled")]
					public static ThemeResourceKey<Brush> SelectedDisabled => new("TopNavigationViewItemForegroundSelectedDisabled");
				}

				public static partial class Background
				{
					[ResourceKeyDefinition(typeof(Brush), "TopNavigationViewItemBackground")]
					public static ThemeResourceKey<Brush> Default => new("TopNavigationViewItemBackground");

					[ResourceKeyDefinition(typeof(Brush), "TopNavigationViewItemBackgroundPointerOver")]
					public static ThemeResourceKey<Brush> PointerOver => new("TopNavigationViewItemBackgroundPointerOver");

					[ResourceKeyDefinition(typeof(Brush), "TopNavigationViewItemBackgroundPressed")]
					public static ThemeResourceKey<Brush> Pressed => new("TopNavigationViewItemBackgroundPressed");

					[ResourceKeyDefinition(typeof(Brush), "TopNavigationViewItemBackgroundDisabled")]
					public static ThemeResourceKey<Brush> Disabled => new("TopNavigationViewItemBackgroundDisabled");

					[ResourceKeyDefinition(typeof(Brush), "TopNavigationViewItemBackgroundSelected")]
					public static ThemeResourceKey<Brush> Selected => new("TopNavigationViewItemBackgroundSelected");

					[ResourceKeyDefinition(typeof(Brush), "TopNavigationViewItemBackgroundSelectedPointerOver")]
					public static ThemeResourceKey<Brush> SelectedPointerOver => new("TopNavigationViewItemBackgroundSelectedPointerOver");

					[ResourceKeyDefinition(typeof(Brush), "TopNavigationViewItemBackgroundSelectedPressed")]
					public static ThemeResourceKey<Brush> SelectedPressed => new("TopNavigationViewItemBackgroundSelectedPressed");
				}

				public static partial class SeparatorForeground
				{
					[ResourceKeyDefinition(typeof(Brush), "TopNavigationViewItemSeparatorForeground")]
					public static ThemeResourceKey<Brush> Default => new("TopNavigationViewItemSeparatorForeground");
				}
			}
		}
		public static partial class Styles
		{
			[ResourceKeyDefinition(typeof(Style), "NavigationViewItemStyle", TargetType = typeof(NavigationViewItem))]
			public static StaticResourceKey<Style> Default => new("NavigationViewItemStyle");
		}
	}
}
