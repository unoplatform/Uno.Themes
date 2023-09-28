using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Media;
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
							public static ResourceValue<double> Open => new("SplitViewOpenPaneThemeLength", true);

							[ResourceKeyDefinition(typeof(double), "SplitViewCompactPaneThemeLength")]
							public static ResourceValue<double> Compact => new("SplitViewCompactPaneThemeLength", true);
						}

						public static class AnimationDuration
						{
							[ResourceKeyDefinition(typeof(string), "SplitViewPaneAnimationOpenDuration")]
							public static ResourceValue<string> Open => new("SplitViewPaneAnimationOpenDuration", true);

							[ResourceKeyDefinition(typeof(string), "SplitViewPaneAnimationOpenPreDuration")]
							public static ResourceValue<string> OpenPre => new("SplitViewPaneAnimationOpenPreDuration", true);

							[ResourceKeyDefinition(typeof(string), "SplitViewPaneAnimationCloseDuration")]
							public static ResourceValue<string> Close => new("SplitViewPaneAnimationCloseDuration", true);
						}

						public static class CornerRadiuses
						{
							[ResourceKeyDefinition(typeof(CornerRadius), "SplitViewPaneRootCornerRadius")]
							public static ResourceValue<CornerRadius> Root => new("SplitViewPaneRootCornerRadius", true);
						}
					}

					public static class BorderThickness
					{
						[ResourceKeyDefinition(typeof(Thickness), "SplitViewLeftBorderThemeThickness")]
						public static ResourceValue<Thickness> Left => new("SplitViewLeftBorderThemeThickness", true);

						[ResourceKeyDefinition(typeof(Thickness), "SplitViewRightBorderThemeThickness")]
						public static ResourceValue<Thickness> Right => new("SplitViewRightBorderThemeThickness", true);
					}

					public static class Overlay
					{
						public static class Background
						{
							[ResourceKeyDefinition(typeof(Brush), "NavigationViewSplitViewLightDismissOverlayBackground")]
							public static ResourceValue<Brush> LightDismiss => new("NavigationViewSplitViewLightDismissOverlayBackground", true);
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
						public static ResourceValue<Brush> Default => new("NavigationViewBackground", true);
					}

					public static class Pane
					{
						public static class Background
						{
							[ResourceKeyDefinition(typeof(Brush), "NavigationViewDefaultPaneBackground")]
							public static ResourceValue<Brush> Default => new("NavigationViewDefaultPaneBackground", true);

							[ResourceKeyDefinition(typeof(Brush), "NavigationViewPaneBackground")]
							public static ResourceValue<Brush> Normal => new("NavigationViewPaneBackground", true);

							[ResourceKeyDefinition(typeof(Brush), "NavigationViewTopPaneBackground")]
							public static ResourceValue<Brush> Top => new("NavigationViewTopPaneBackground", true);
						}

						public static class BorderBrush
						{
							[ResourceKeyDefinition(typeof(Brush), "NavigationViewDefaultPaneBorderBrush")]
							public static ResourceValue<Brush> Default => new("NavigationViewDefaultPaneBorderBrush", true);
						}
					}

					public static class Button
					{
						public static class Foreground
						{
							[ResourceKeyDefinition(typeof(Brush), "NavigationViewButtonForeground")]
							public static ResourceValue<Brush> Default => new("NavigationViewButtonForeground", true);

							[ResourceKeyDefinition(typeof(Brush), "NavigationViewButtonForegroundPointerOver")]
							public static ResourceValue<Brush> PointerOver => new("NavigationViewButtonForegroundPointerOver", true);

							[ResourceKeyDefinition(typeof(Brush), "NavigationViewButtonForegroundPressed")]
							public static ResourceValue<Brush> Pressed => new("NavigationViewButtonForegroundPressed", true);

							[ResourceKeyDefinition(typeof(Brush), "NavigationViewButtonForegroundDisabled")]
							public static ResourceValue<Brush> Disabled => new("NavigationViewButtonForegroundDisabled", true);
						}

						public static class Background
						{
							[ResourceKeyDefinition(typeof(Brush), "NavigationViewButtonBackground")]
							public static ResourceValue<Brush> Default => new("NavigationViewButtonBackground", true);

							[ResourceKeyDefinition(typeof(Brush), "NavigationViewButtonBackgroundPointerOver")]
							public static ResourceValue<Brush> PointerOver => new("NavigationViewButtonBackgroundPointerOver", true);

							[ResourceKeyDefinition(typeof(Brush), "NavigationViewButtonBackgroundPressed")]
							public static ResourceValue<Brush> Pressed => new("NavigationViewButtonBackgroundPressed", true);

							[ResourceKeyDefinition(typeof(Brush), "NavigationViewButtonBackgroundDisabled")]
							public static ResourceValue<Brush> Disabled => new("NavigationViewButtonBackgroundDisabled", true);
						}

						public static class RippleFeedback
						{
							[ResourceKeyDefinition(typeof(Brush), "NavigationViewButtonRippleFeedback")]
							public static ResourceValue<Brush> Default => new("NavigationViewButtonRippleFeedback", true);
						}
					}

					public static class Item
					{
						public static class Background
						{
							[ResourceKeyDefinition(typeof(Brush), "NavigationViewItemBackground")]
							public static ResourceValue<Brush> Default => new("NavigationViewItemBackground", true);

							[ResourceKeyDefinition(typeof(Brush), "NavigationViewItemBackgroundPointerOver")]
							public static ResourceValue<Brush> PointerOver => new("NavigationViewItemBackgroundPointerOver", true);

							[ResourceKeyDefinition(typeof(Brush), "NavigationViewItemBackgroundPressed")]
							public static ResourceValue<Brush> Pressed => new("NavigationViewItemBackgroundPressed", true);

							[ResourceKeyDefinition(typeof(Brush), "NavigationViewItemBackgroundDisabled")]
							public static ResourceValue<Brush> Disabled => new("NavigationViewItemBackgroundDisabled", true);

							[ResourceKeyDefinition(typeof(Brush), "NavigationViewItemBackgroundSelected")]
							public static ResourceValue<Brush> Selected => new("NavigationViewItemBackgroundSelected", true);

							[ResourceKeyDefinition(typeof(Brush), "NavigationViewItemBackgroundSelectedPointerOver")]
							public static ResourceValue<Brush> SelectedPointerOver => new("NavigationViewItemBackgroundSelectedPointerOver", true);

							[ResourceKeyDefinition(typeof(Brush), "NavigationViewItemBackgroundSelectedPressed")]
							public static ResourceValue<Brush> SelectedPressed => new("NavigationViewItemBackgroundSelectedPressed", true);

							[ResourceKeyDefinition(typeof(Brush), "NavigationViewItemBackgroundSelectedDisabled")]
							public static ResourceValue<Brush> SelectedDisabled => new("NavigationViewItemBackgroundSelectedDisabled", true);
						}

						public static class Foreground
						{
							[ResourceKeyDefinition(typeof(Brush), "NavigationViewItemForeground")]
							public static ResourceValue<Brush> Default => new("NavigationViewItemForeground", true);

							[ResourceKeyDefinition(typeof(Brush), "NavigationViewItemForegroundPointerOver")]
							public static ResourceValue<Brush> PointerOver => new("NavigationViewItemForegroundPointerOver", true);

							[ResourceKeyDefinition(typeof(Brush), "NavigationViewItemForegroundPressed")]
							public static ResourceValue<Brush> Pressed => new("NavigationViewItemForegroundPressed", true);

							[ResourceKeyDefinition(typeof(Brush), "NavigationViewItemForegroundDisabled")]
							public static ResourceValue<Brush> Disabled => new("NavigationViewItemForegroundDisabled", true);

							[ResourceKeyDefinition(typeof(Brush), "NavigationViewItemForegroundSelected")]
							public static ResourceValue<Brush> Selected => new("NavigationViewItemForegroundSelected", true);

							[ResourceKeyDefinition(typeof(Brush), "NavigationViewItemForegroundSelectedPointerOver")]
							public static ResourceValue<Brush> SelectedPointerOver => new("NavigationViewItemForegroundSelectedPointerOver", true);

							[ResourceKeyDefinition(typeof(Brush), "NavigationViewItemForegroundSelectedPressed")]
							public static ResourceValue<Brush> SelectedPressed => new("NavigationViewItemForegroundSelectedPressed", true);

							[ResourceKeyDefinition(typeof(Brush), "NavigationViewItemForegroundSelectedDisabled")]
							public static ResourceValue<Brush> SelectedDisabled => new("NavigationViewItemForegroundSelectedDisabled", true);
						}

						public static class BorderBrush
						{
							[ResourceKeyDefinition(typeof(Brush), "NavigationViewItemBorderBrush")]
							public static ResourceValue<Brush> Default => new("NavigationViewItemBorderBrush", true);

							[ResourceKeyDefinition(typeof(Brush), "NavigationViewItemBorderBrushPointerOver")]
							public static ResourceValue<Brush> PointerOver => new("NavigationViewItemBorderBrushPointerOver", true);

							[ResourceKeyDefinition(typeof(Brush), "NavigationViewItemBorderBrushPressed")]
							public static ResourceValue<Brush> Pressed => new("NavigationViewItemBorderBrushPressed", true);

							[ResourceKeyDefinition(typeof(Brush), "NavigationViewItemBorderBrushDisabled")]
							public static ResourceValue<Brush> Disabled => new("NavigationViewItemBorderBrushDisabled", true);

							[ResourceKeyDefinition(typeof(Brush), "NavigationViewItemBorderBrushSelected")]
							public static ResourceValue<Brush> Selected => new("NavigationViewItemBorderBrushSelected", true);

							[ResourceKeyDefinition(typeof(Brush), "NavigationViewItemBorderBrushSelectedPointerOver")]
							public static ResourceValue<Brush> SelectedPointerOver => new("NavigationViewItemBorderBrushSelectedPointerOver", true);

							[ResourceKeyDefinition(typeof(Brush), "NavigationViewItemBorderBrushSelectedPressed")]
							public static ResourceValue<Brush> SelectedPressed => new("NavigationViewItemBorderBrushSelectedPressed", true);

							[ResourceKeyDefinition(typeof(Brush), "NavigationViewItemBorderBrushSelectedDisabled")]
							public static ResourceValue<Brush> SelectedDisabled => new("NavigationViewItemBorderBrushSelectedDisabled", true);
						}

						public static class Separator
						{
							public static class Foreground
							{
								[ResourceKeyDefinition(typeof(Brush), "NavigationViewItemSeparatorForeground")]
								public static ResourceValue<Brush> Default => new("NavigationViewItemSeparatorForeground", true);
							}
						}

						public static class Header
						{
							public static class Foreground
							{
								[ResourceKeyDefinition(typeof(Brush), "NavigationViewItemHeaderForeground")]
								public static ResourceValue<Brush> Default => new("NavigationViewItemHeaderForeground", true);
							}
						}

						public static class RippleFeedback
						{
							[ResourceKeyDefinition(typeof(Brush), "NavigationViewItemRippleFeedback")]
							public static ResourceValue<Brush> Default => new("NavigationViewItemRippleFeedback", true);
						}
					}

					public static class SelectionIndicator
					{
						public static class Foreground
						{
							[ResourceKeyDefinition(typeof(Brush), "NavigationViewSelectionIndicatorForeground")]
							public static ResourceValue<Brush> Default => new("NavigationViewSelectionIndicatorForeground", true);
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
							public static ResourceValue<Brush> Default => new("TopNavigationViewItemForeground", true);

							[ResourceKeyDefinition(typeof(Brush), "TopNavigationViewItemForegroundPointerOver")]
							public static ResourceValue<Brush> PointerOver => new("TopNavigationViewItemForegroundPointerOver", true);

							[ResourceKeyDefinition(typeof(Brush), "TopNavigationViewItemForegroundPressed")]
							public static ResourceValue<Brush> Pressed => new("TopNavigationViewItemForegroundPressed", true);

							[ResourceKeyDefinition(typeof(Brush), "TopNavigationViewItemForegroundDisabled")]
							public static ResourceValue<Brush> Disabled => new("TopNavigationViewItemForegroundDisabled", true);

							[ResourceKeyDefinition(typeof(Brush), "TopNavigationViewItemForegroundSelected")]
							public static ResourceValue<Brush> Selected => new("TopNavigationViewItemForegroundSelected", true);

							[ResourceKeyDefinition(typeof(Brush), "TopNavigationViewItemForegroundSelectedPointerOver")]
							public static ResourceValue<Brush> SelectedPointerOver => new("TopNavigationViewItemForegroundSelectedPointerOver", true);

							[ResourceKeyDefinition(typeof(Brush), "TopNavigationViewItemForegroundSelectedPressed")]
							public static ResourceValue<Brush> SelectedPressed => new("TopNavigationViewItemForegroundSelectedPressed", true);

							[ResourceKeyDefinition(typeof(Brush), "TopNavigationViewItemForegroundSelectedDisabled")]
							public static ResourceValue<Brush> SelectedDisabled => new("TopNavigationViewItemForegroundSelectedDisabled", true);
						}

						public static class Background
						{
							[ResourceKeyDefinition(typeof(Brush), "TopNavigationViewItemBackground")]
							public static ResourceValue<Brush> Default => new("TopNavigationViewItemBackground", true);

							[ResourceKeyDefinition(typeof(Brush), "TopNavigationViewItemBackgroundPointerOver")]
							public static ResourceValue<Brush> PointerOver => new("TopNavigationViewItemBackgroundPointerOver", true);

							[ResourceKeyDefinition(typeof(Brush), "TopNavigationViewItemBackgroundPressed")]
							public static ResourceValue<Brush> Pressed => new("TopNavigationViewItemBackgroundPressed", true);

							[ResourceKeyDefinition(typeof(Brush), "TopNavigationViewItemBackgroundDisabled")]
							public static ResourceValue<Brush> Disabled => new("TopNavigationViewItemBackgroundDisabled", true);

							[ResourceKeyDefinition(typeof(Brush), "TopNavigationViewItemBackgroundSelected")]
							public static ResourceValue<Brush> Selected => new("TopNavigationViewItemBackgroundSelected", true);

							[ResourceKeyDefinition(typeof(Brush), "TopNavigationViewItemBackgroundSelectedPointerOver")]
							public static ResourceValue<Brush> SelectedPointerOver => new("TopNavigationViewItemBackgroundSelectedPointerOver", true);

							[ResourceKeyDefinition(typeof(Brush), "TopNavigationViewItemBackgroundSelectedPressed")]
							public static ResourceValue<Brush> SelectedPressed => new("TopNavigationViewItemBackgroundSelectedPressed", true);
						}

						public static class Separator
						{
							public static class Foreground
							{
								[ResourceKeyDefinition(typeof(Brush), "TopNavigationViewItemSeparatorForeground")]
								public static ResourceValue<Brush> Default => new("TopNavigationViewItemSeparatorForeground", true);
							}
						}
					}
				}
			}

			public static class Styles
			{
				[ResourceKeyDefinition(typeof(Style), "NavigationViewStyle", TargetType = typeof(NavigationView))]
				public static ResourceValue<Style> Default => new("NavigationViewStyle");

				public static class NavigationViewItem
				{
					[ResourceKeyDefinition(typeof(Style), "NavigationViewItemStyle", TargetType = typeof(NavigationViewItem))]
					public static ResourceValue<Style> DefaultItem => new("NavigationViewItemStyle");
				}
			}
		}
	}
}
