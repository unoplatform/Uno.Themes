using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Media;
using Uno.Extensions.Markup;
using Uno.Extensions.Markup.Internals;

namespace Uno.Themes.Markup
{
	public static partial class Theme
	{
		public static class FloatingActionButton
		{
			public static class Resources
			{
				public static class Default
				{
					public static class Foreground
					{
						[ResourceKeyDefinition(typeof(Brush), "FabForeground")]
						public static ThemeResourceKey<Brush> Default => new("FabForeground");

						[ResourceKeyDefinition(typeof(Brush), "FabForegroundPressed")]
						public static ThemeResourceKey<Brush> Pressed => new("FabForegroundPressed");

						[ResourceKeyDefinition(typeof(Brush), "FabForegroundPointerOver")]
						public static ThemeResourceKey<Brush> PointerOver => new("FabForegroundPointerOver");

						[ResourceKeyDefinition(typeof(Brush), "FabForegroundDisabled")]
						public static ThemeResourceKey<Brush> Disabled => new("FabForegroundDisabled");
					}

					public static class Background
					{
						[ResourceKeyDefinition(typeof(Brush), "FabBackground")]
						public static ThemeResourceKey<Brush> Default => new("FabBackground");

						[ResourceKeyDefinition(typeof(Brush), "FabBackgroundPressed")]
						public static ThemeResourceKey<Brush> Pressed => new("FabBackgroundPressed");

						[ResourceKeyDefinition(typeof(Brush), "FabBackgroundPointerOver")]
						public static ThemeResourceKey<Brush> PointerOver => new("FabBackgroundPointerOver");

						[ResourceKeyDefinition(typeof(Brush), "FabBackgroundDisabled")]
						public static ThemeResourceKey<Brush> Disabled => new("FabBackgroundDisabled");
					}

					public static class StateOverlay
					{
						public static class Background
						{
							[ResourceKeyDefinition(typeof(Brush), "FabStateOverlayBackground")]
							public static ThemeResourceKey<Brush> Default => new("FabStateOverlayBackground");

							[ResourceKeyDefinition(typeof(Brush), "FabStateOverlayBackgroundPressed")]
							public static ThemeResourceKey<Brush> Pressed => new("FabStateOverlayBackgroundPressed");

							[ResourceKeyDefinition(typeof(Brush), "FabStateOverlayBackgroundPointerOver")]
							public static ThemeResourceKey<Brush> PointerOver => new("FabStateOverlayBackgroundPointerOver");

							[ResourceKeyDefinition(typeof(Brush), "FabStateOverlayBackgroundFocused")]
							public static ThemeResourceKey<Brush> Focused => new("FabStateOverlayBackgroundFocused");

							[ResourceKeyDefinition(typeof(Brush), "FabStateOverlayBackgroundDisabled")]
							public static ThemeResourceKey<Brush> Disabled => new("FabStateOverlayBackgroundDisabled");
						}
					}
				}

				public static class Surface
				{
					public static class Foreground
					{
						[ResourceKeyDefinition(typeof(Brush), "FabSurfaceForeground")]
						public static ThemeResourceKey<Brush> Default => new("FabSurfaceForeground");

						[ResourceKeyDefinition(typeof(Brush), "FabSurfaceForegroundPressed")]
						public static ThemeResourceKey<Brush> Pressed => new("FabSurfaceForegroundPressed");

						[ResourceKeyDefinition(typeof(Brush), "FabSurfaceForegroundPointerOver")]
						public static ThemeResourceKey<Brush> PointerOver => new("FabSurfaceForegroundPointerOver");

						[ResourceKeyDefinition(typeof(Brush), "FabSurfaceForegroundDisabled")]
						public static ThemeResourceKey<Brush> Disabled => new("FabSurfaceForegroundDisabled");
					}

					public static class Background
					{
						[ResourceKeyDefinition(typeof(Brush), "FabSurfaceBackground")]
						public static ThemeResourceKey<Brush> Default => new("FabSurfaceBackground");

						[ResourceKeyDefinition(typeof(Brush), "FabSurfaceBackgroundPressed")]
						public static ThemeResourceKey<Brush> Pressed => new("FabSurfaceBackgroundPressed");

						[ResourceKeyDefinition(typeof(Brush), "FabSurfaceBackgroundPointerOver")]
						public static ThemeResourceKey<Brush> PointerOver => new("FabSurfaceBackgroundPointerOver");

						[ResourceKeyDefinition(typeof(Brush), "FabSurfaceBackgroundDisabled")]
						public static ThemeResourceKey<Brush> Disabled => new("FabSurfaceBackgroundDisabled");
					}
				}

				public static class StateOverlay
				{
					public static class Background
					{
						[ResourceKeyDefinition(typeof(Brush), "FabSurfaceStateOverlayBackground")]
						public static ThemeResourceKey<Brush> Default => new("FabSurfaceStateOverlayBackground");

						[ResourceKeyDefinition(typeof(Brush), "FabSurfaceStateOverlayBackgroundPressed")]
						public static ThemeResourceKey<Brush> Pressed => new("FabSurfaceStateOverlayBackgroundPressed");

						[ResourceKeyDefinition(typeof(Brush), "FabSurfaceStateOverlayBackgroundPointerOver")]
						public static ThemeResourceKey<Brush> PointerOver => new("FabSurfaceStateOverlayBackgroundPointerOver");

						[ResourceKeyDefinition(typeof(Brush), "FabSurfaceStateOverlayBackgroundFocused")]
						public static ThemeResourceKey<Brush> Focused => new("FabSurfaceStateOverlayBackgroundFocused");

						[ResourceKeyDefinition(typeof(Brush), "FabSurfaceStateOverlayBackgroundDisabled")]
						public static ThemeResourceKey<Brush> Disabled => new("FabSurfaceStateOverlayBackgroundDisabled");
					}
				}

				public static class Secondary
				{
					public static class Foreground
					{
						[ResourceKeyDefinition(typeof(Brush), "FabSecondaryForeground")]
						public static ThemeResourceKey<Brush> Default => new("FabSecondaryForeground");

						[ResourceKeyDefinition(typeof(Brush), "FabSecondaryForegroundPressed")]
						public static ThemeResourceKey<Brush> Pressed => new("FabSecondaryForegroundPressed");

						[ResourceKeyDefinition(typeof(Brush), "FabSecondaryForegroundPointerOver")]
						public static ThemeResourceKey<Brush> PointerOver => new("FabSecondaryForegroundPointerOver");

						[ResourceKeyDefinition(typeof(Brush), "FabSecondaryForegroundDisabled")]
						public static ThemeResourceKey<Brush> Disabled => new("FabSecondaryForegroundDisabled");
					}

					public static class Background
					{
						[ResourceKeyDefinition(typeof(Brush), "FabSecondaryBackground")]
						public static ThemeResourceKey<Brush> Default => new("FabSecondaryBackground");

						[ResourceKeyDefinition(typeof(Brush), "FabSecondaryBackgroundPressed")]
						public static ThemeResourceKey<Brush> Pressed => new("FabSecondaryBackgroundPressed");

						[ResourceKeyDefinition(typeof(Brush), "FabSecondaryBackgroundPointerOver")]
						public static ThemeResourceKey<Brush> PointerOver => new("FabSecondaryBackgroundPointerOver");

						[ResourceKeyDefinition(typeof(Brush), "FabSecondaryBackgroundDisabled")]
						public static ThemeResourceKey<Brush> Disabled => new("FabSecondaryBackgroundDisabled");
					}

					public static class StateOverlay
					{
						public static class Background
						{
							[ResourceKeyDefinition(typeof(Brush), "FabSecondaryStateOverlayBackground")]
							public static ThemeResourceKey<Brush> Default => new("FabSecondaryStateOverlayBackground");

							[ResourceKeyDefinition(typeof(Brush), "FabSecondaryStateOverlayBackgroundPressed")]
							public static ThemeResourceKey<Brush> Pressed => new("FabSecondaryStateOverlayBackgroundPressed");

							[ResourceKeyDefinition(typeof(Brush), "FabSecondaryStateOverlayBackgroundPointerOver")]
							public static ThemeResourceKey<Brush> PointerOver => new("FabSecondaryStateOverlayBackgroundPointerOver");

							[ResourceKeyDefinition(typeof(Brush), "FabSecondaryStateOverlayBackgroundFocused")]
							public static ThemeResourceKey<Brush> Focused => new("FabSecondaryStateOverlayBackgroundFocused");

							[ResourceKeyDefinition(typeof(Brush), "FabSecondaryStateOverlayBackgroundDisabled")]
							public static ThemeResourceKey<Brush> Disabled => new("FabSecondaryStateOverlayBackgroundDisabled");
						}
					}
				}

				public static class Tertiary
				{
					public static class Foreground
					{
						[ResourceKeyDefinition(typeof(Brush), "FabTertiaryForeground")]
						public static ThemeResourceKey<Brush> Default => new("FabTertiaryForeground");

						[ResourceKeyDefinition(typeof(Brush), "FabTertiaryForegroundPressed")]
						public static ThemeResourceKey<Brush> Pressed => new("FabTertiaryForegroundPressed");

						[ResourceKeyDefinition(typeof(Brush), "FabTertiaryForegroundPointerOver")]
						public static ThemeResourceKey<Brush> PointerOver => new("FabTertiaryForegroundPointerOver");

						[ResourceKeyDefinition(typeof(Brush), "FabTertiaryForegroundDisabled")]
						public static ThemeResourceKey<Brush> Disabled => new("FabTertiaryForegroundDisabled");
					}

					public static class Background
					{
						[ResourceKeyDefinition(typeof(Brush), "FabTertiaryBackground")]
						public static ThemeResourceKey<Brush> Default => new("FabTertiaryBackground");

						[ResourceKeyDefinition(typeof(Brush), "FabTertiaryBackgroundPressed")]
						public static ThemeResourceKey<Brush> Pressed => new("FabTertiaryBackgroundPressed");

						[ResourceKeyDefinition(typeof(Brush), "FabTertiaryBackgroundPointerOver")]
						public static ThemeResourceKey<Brush> PointerOver => new("FabTertiaryBackgroundPointerOver");

						[ResourceKeyDefinition(typeof(Brush), "FabTertiaryBackgroundDisabled")]
						public static ThemeResourceKey<Brush> Disabled => new("FabTertiaryBackgroundDisabled");
					}

					public static class StateOverlay
					{
						public static class Background
						{
							[ResourceKeyDefinition(typeof(Brush), "FabTertiaryStateOverlayBackground")]
							public static ThemeResourceKey<Brush> Default => new("FabTertiaryStateOverlayBackground");

							[ResourceKeyDefinition(typeof(Brush), "FabTertiaryStateOverlayBackgroundPressed")]
							public static ThemeResourceKey<Brush> Pressed => new("FabTertiaryStateOverlayBackgroundPressed");

							[ResourceKeyDefinition(typeof(Brush), "FabTertiaryStateOverlayBackgroundPointerOver")]
							public static ThemeResourceKey<Brush> PointerOver => new("FabTertiaryStateOverlayBackgroundPointerOver");

							[ResourceKeyDefinition(typeof(Brush), "FabTertiaryStateOverlayBackgroundFocused")]
							public static ThemeResourceKey<Brush> Focused => new("FabTertiaryStateOverlayBackgroundFocused");

							[ResourceKeyDefinition(typeof(Brush), "FabTertiaryStateOverlayBackgroundDisabled")]
							public static ThemeResourceKey<Brush> Disabled => new("FabTertiaryStateOverlayBackgroundDisabled");
						}
					}
				}
			}

			public static class Styles
			{
				[ResourceKeyDefinition(typeof(Style), "MaterialFabStyle", TargetType = typeof(Button))]
				public static StaticResourceKey<Style> Default => new("MaterialFabStyle");

				[ResourceKeyDefinition(typeof(Style), "MaterialSurfaceFabStyle", TargetType = typeof(Button))]
				public static StaticResourceKey<Style> Surface => new("MaterialSurfaceFabStyle");

				[ResourceKeyDefinition(typeof(Style), "MaterialSecondaryFabStyle", TargetType = typeof(Button))]
				public static StaticResourceKey<Style> Secondary => new("MaterialSecondaryFabStyle");

				[ResourceKeyDefinition(typeof(Style), "MaterialTertiaryFabStyle", TargetType = typeof(Button))]
				public static StaticResourceKey<Style> Tertiary => new("MaterialTertiaryFabStyle");

				[ResourceKeyDefinition(typeof(Style), "MaterialSmallFabStyle", TargetType = typeof(Button))]
				public static StaticResourceKey<Style> Small => new("MaterialSmallFabStyle");

				[ResourceKeyDefinition(typeof(Style), "MaterialSurfaceSmallFabStyle", TargetType = typeof(Button))]
				public static StaticResourceKey<Style> SurfaceSmall => new("MaterialSurfaceSmallFabStyle");

				[ResourceKeyDefinition(typeof(Style), "MaterialSecondarySmallFabStyle", TargetType = typeof(Button))]
				public static StaticResourceKey<Style> SecondarySmall => new("MaterialSecondarySmallFabStyle");

				[ResourceKeyDefinition(typeof(Style), "MaterialTertiarySmallFabStyle", TargetType = typeof(Button))]
				public static StaticResourceKey<Style> TertiarySmall => new("MaterialTertiarySmallFabStyle");

				[ResourceKeyDefinition(typeof(Style), "MaterialLargeFabStyle", TargetType = typeof(Button))]
				public static StaticResourceKey<Style> Large => new("MaterialLargeFabStyle");

				[ResourceKeyDefinition(typeof(Style), "MaterialSurfaceLargeFabStyle", TargetType = typeof(Button))]
				public static StaticResourceKey<Style> SurfaceLarge => new("MaterialSurfaceLargeFabStyle");

				[ResourceKeyDefinition(typeof(Style), "MaterialSecondaryLargeFabStyle", TargetType = typeof(Button))]
				public static StaticResourceKey<Style> SecondaryLarge => new("MaterialSecondaryLargeFabStyle");

				[ResourceKeyDefinition(typeof(Style), "MaterialTertiaryLargeFabStyle", TargetType = typeof(Button))]
				public static StaticResourceKey<Style> TertiaryLarge => new("MaterialTertiaryLargeFabStyle");
			}
		}
	}
}
