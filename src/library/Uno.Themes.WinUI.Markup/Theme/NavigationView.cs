using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Media;
using Uno.Extensions.Markup;
using Uno.Extensions.Markup.Internals;

namespace Uno.Themes.Markup;

public static partial class Theme
{
	public static partial class NavigationView
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

					public static class SelectionIndicator
					{
						public static class Foreground
						{
							[ResourceKeyDefinition(typeof(Brush), "NavigationViewSelectionIndicatorForeground")]
							public static ThemeResourceKey<Brush> Default => new("NavigationViewSelectionIndicatorForeground");
						}
					}
				}
			}
		public static partial class Styles
		{
			[ResourceKeyDefinition(typeof(Style), "NavigationViewStyle", TargetType = typeof(NavigationView))]
			public static StaticResourceKey<Style> Default => new("NavigationViewStyle");
		}
	}
}
