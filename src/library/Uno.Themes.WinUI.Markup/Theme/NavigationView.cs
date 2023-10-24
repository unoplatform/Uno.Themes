using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Media;
using Uno.Extensions.Markup;
using Uno.Extensions.Markup.Internals;

namespace Uno.Themes.Markup
{
	public static partial class Theme
	{
		public static class SplitView
		{
			public static class Resources
			{
				public static class Default
				{
					public static class Pane
					{
						public static class Length
						{
							[ResourceKeyDefinition(typeof(double), "SplitViewOpenPaneThemeLength")]
							public static ThemeResourceKey<double> Open => new("SplitViewOpenPaneThemeLength");

							[ResourceKeyDefinition(typeof(double), "SplitViewCompactPaneThemeLength")]
							public static ThemeResourceKey<double> Compact => new("SplitViewCompactPaneThemeLength");
						}

						public static class AnimationDuration
						{
							[ResourceKeyDefinition(typeof(string), "SplitViewPaneAnimationOpenDuration")]
							public static ThemeResourceKey<string> Open => new("SplitViewPaneAnimationOpenDuration");

							[ResourceKeyDefinition(typeof(string), "SplitViewPaneAnimationOpenPreDuration")]
							public static ThemeResourceKey<string> OpenPre => new("SplitViewPaneAnimationOpenPreDuration");

							[ResourceKeyDefinition(typeof(string), "SplitViewPaneAnimationCloseDuration")]
							public static ThemeResourceKey<string> Close => new("SplitViewPaneAnimationCloseDuration");
						}

						public static class CornerRadiuses
						{
							[ResourceKeyDefinition(typeof(CornerRadius), "SplitViewPaneRootCornerRadius")]
							public static ThemeResourceKey<CornerRadius> Root => new("SplitViewPaneRootCornerRadius");
						}
					}

					public static class BorderThickness
					{
						[ResourceKeyDefinition(typeof(Thickness), "SplitViewLeftBorderThemeThickness")]
						public static ThemeResourceKey<Thickness> Left => new("SplitViewLeftBorderThemeThickness");

						[ResourceKeyDefinition(typeof(Thickness), "SplitViewRightBorderThemeThickness")]
						public static ThemeResourceKey<Thickness> Right => new("SplitViewRightBorderThemeThickness");
					}

					public static class Overlay
					{
						public static class Background
						{
							[ResourceKeyDefinition(typeof(Brush), "NavigationViewSplitViewLightDismissOverlayBackground")]
							public static ThemeResourceKey<Brush> LightDismiss => new("NavigationViewSplitViewLightDismissOverlayBackground");
						}
					}
				}
			}
		}

		public static class NavigationView
		{
			public static class Resources
			{
				public static class Default
				{
					public static class Background
					{
						[ResourceKeyDefinition(typeof(Brush), "NavigationViewBackground")]
						public static ThemeResourceKey<Brush> Default => new("NavigationViewBackground");
					}

					public static class Pane
					{
						public static class Background
						{
							[ResourceKeyDefinition(typeof(Brush), "NavigationViewDefaultPaneBackground")]
							public static ThemeResourceKey<Brush> Default => new("NavigationViewDefaultPaneBackground");

							[ResourceKeyDefinition(typeof(Brush), "NavigationViewPaneBackground")]
							public static ThemeResourceKey<Brush> Normal => new("NavigationViewPaneBackground");

							[ResourceKeyDefinition(typeof(Brush), "NavigationViewTopPaneBackground")]
							public static ThemeResourceKey<Brush> Top => new("NavigationViewTopPaneBackground");
						}

						public static class BorderBrush
						{
							[ResourceKeyDefinition(typeof(Brush), "NavigationViewDefaultPaneBorderBrush")]
							public static ThemeResourceKey<Brush> Default => new("NavigationViewDefaultPaneBorderBrush");
						}
					}

					public static class Button
					{
						public static class Foreground
						{
							[ResourceKeyDefinition(typeof(Brush), "NavigationViewButtonForeground")]
							public static ThemeResourceKey<Brush> Default => new("NavigationViewButtonForeground");

							[ResourceKeyDefinition(typeof(Brush), "NavigationViewButtonForegroundPointerOver")]
							public static ThemeResourceKey<Brush> PointerOver => new("NavigationViewButtonForegroundPointerOver");

							[ResourceKeyDefinition(typeof(Brush), "NavigationViewButtonForegroundPressed")]
							public static ThemeResourceKey<Brush> Pressed => new("NavigationViewButtonForegroundPressed");

							[ResourceKeyDefinition(typeof(Brush), "NavigationViewButtonForegroundDisabled")]
							public static ThemeResourceKey<Brush> Disabled => new("NavigationViewButtonForegroundDisabled");
						}

						public static class Background
						{
							[ResourceKeyDefinition(typeof(Brush), "NavigationViewButtonBackground")]
							public static ThemeResourceKey<Brush> Default => new("NavigationViewButtonBackground");

							[ResourceKeyDefinition(typeof(Brush), "NavigationViewButtonBackgroundPointerOver")]
							public static ThemeResourceKey<Brush> PointerOver => new("NavigationViewButtonBackgroundPointerOver");

							[ResourceKeyDefinition(typeof(Brush), "NavigationViewButtonBackgroundPressed")]
							public static ThemeResourceKey<Brush> Pressed => new("NavigationViewButtonBackgroundPressed");

							[ResourceKeyDefinition(typeof(Brush), "NavigationViewButtonBackgroundDisabled")]
							public static ThemeResourceKey<Brush> Disabled => new("NavigationViewButtonBackgroundDisabled");
						}

						public static class RippleFeedback
						{
							[ResourceKeyDefinition(typeof(Brush), "NavigationViewButtonRippleFeedback")]
							public static ThemeResourceKey<Brush> Default => new("NavigationViewButtonRippleFeedback");
						}
					}

					public static class Item
					{
						public static class Background
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

						public static class Foreground
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

						public static class BorderBrush
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

						public static class Separator
						{
							public static class Foreground
							{
								[ResourceKeyDefinition(typeof(Brush), "NavigationViewItemSeparatorForeground")]
								public static ThemeResourceKey<Brush> Default => new("NavigationViewItemSeparatorForeground");
							}
						}

						public static class Header
						{
							public static class Foreground
							{
								[ResourceKeyDefinition(typeof(Brush), "NavigationViewItemHeaderForeground")]
								public static ThemeResourceKey<Brush> Default => new("NavigationViewItemHeaderForeground");
							}
						}

						public static class RippleFeedback
						{
							[ResourceKeyDefinition(typeof(Brush), "NavigationViewItemRippleFeedback")]
							public static ThemeResourceKey<Brush> Default => new("NavigationViewItemRippleFeedback");
						}
					}

					public static class SelectionIndicator
					{
						public static class Foreground
						{
							[ResourceKeyDefinition(typeof(Brush), "NavigationViewSelectionIndicatorForeground")]
							public static ThemeResourceKey<Brush> Default => new("NavigationViewSelectionIndicatorForeground");
						}
					}
				}

				public static class Top
				{
					public static class Item
					{
						public static class Foreground
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

						public static class Background
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

						public static class Separator
						{
							public static class Foreground
							{
								[ResourceKeyDefinition(typeof(Brush), "TopNavigationViewItemSeparatorForeground")]
								public static ThemeResourceKey<Brush> Default => new("TopNavigationViewItemSeparatorForeground");
							}
						}
					}
				}
			}

			public static class Styles
			{
				[ResourceKeyDefinition(typeof(Style), "NavigationViewStyle", TargetType = typeof(NavigationView))]
				public static StaticResourceKey<Style> Default => new("NavigationViewStyle");

				public static class NavigationViewItem
				{
					[ResourceKeyDefinition(typeof(Style), "NavigationViewItemStyle", TargetType = typeof(NavigationViewItem))]
					public static StaticResourceKey<Style> DefaultItem => new("NavigationViewItemStyle");
				}
			}
		}
	}
}
