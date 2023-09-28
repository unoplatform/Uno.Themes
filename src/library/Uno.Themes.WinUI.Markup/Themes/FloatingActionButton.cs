using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Media;
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
						public static ResourceValue<Brush> Default => new("FabForeground", true);

						[ResourceKeyDefinition(typeof(Brush), "FabForegroundPressed")]
						public static ResourceValue<Brush> Pressed => new("FabForegroundPressed", true);

						[ResourceKeyDefinition(typeof(Brush), "FabForegroundPointerOver")]
						public static ResourceValue<Brush> PointerOver => new("FabForegroundPointerOver", true);

						[ResourceKeyDefinition(typeof(Brush), "FabForegroundDisabled")]
						public static ResourceValue<Brush> Disabled => new("FabForegroundDisabled", true);
					}

					public static class Background
					{
						[ResourceKeyDefinition(typeof(Brush), "FabBackground")]
						public static ResourceValue<Brush> Default => new("FabBackground", true);

						[ResourceKeyDefinition(typeof(Brush), "FabBackgroundPressed")]
						public static ResourceValue<Brush> Pressed => new("FabBackgroundPressed", true);

						[ResourceKeyDefinition(typeof(Brush), "FabBackgroundPointerOver")]
						public static ResourceValue<Brush> PointerOver => new("FabBackgroundPointerOver", true);

						[ResourceKeyDefinition(typeof(Brush), "FabBackgroundDisabled")]
						public static ResourceValue<Brush> Disabled => new("FabBackgroundDisabled", true);
					}

					public static class StateOverlay
					{
						public static class Background
						{
							[ResourceKeyDefinition(typeof(Brush), "FabStateOverlayBackground")]
							public static ResourceValue<Brush> Default => new("FabStateOverlayBackground", true);

							[ResourceKeyDefinition(typeof(Brush), "FabStateOverlayBackgroundPressed")]
							public static ResourceValue<Brush> Pressed => new("FabStateOverlayBackgroundPressed", true);

							[ResourceKeyDefinition(typeof(Brush), "FabStateOverlayBackgroundPointerOver")]
							public static ResourceValue<Brush> PointerOver => new("FabStateOverlayBackgroundPointerOver", true);

							[ResourceKeyDefinition(typeof(Brush), "FabStateOverlayBackgroundFocused")]
							public static ResourceValue<Brush> Focused => new("FabStateOverlayBackgroundFocused", true);

							[ResourceKeyDefinition(typeof(Brush), "FabStateOverlayBackgroundDisabled")]
							public static ResourceValue<Brush> Disabled => new("FabStateOverlayBackgroundDisabled", true);
						}
					}
				}

				public static class Surface
				{
					public static class Foreground
					{
						[ResourceKeyDefinition(typeof(Brush), "FabSurfaceForeground")]
						public static ResourceValue<Brush> Default => new("FabSurfaceForeground", true);

						[ResourceKeyDefinition(typeof(Brush), "FabSurfaceForegroundPressed")]
						public static ResourceValue<Brush> Pressed => new("FabSurfaceForegroundPressed", true);

						[ResourceKeyDefinition(typeof(Brush), "FabSurfaceForegroundPointerOver")]
						public static ResourceValue<Brush> PointerOver => new("FabSurfaceForegroundPointerOver", true);

						[ResourceKeyDefinition(typeof(Brush), "FabSurfaceForegroundDisabled")]
						public static ResourceValue<Brush> Disabled => new("FabSurfaceForegroundDisabled", true);
					}

					public static class Background
					{
						[ResourceKeyDefinition(typeof(Brush), "FabSurfaceBackground")]
						public static ResourceValue<Brush> Default => new("FabSurfaceBackground", true);

						[ResourceKeyDefinition(typeof(Brush), "FabSurfaceBackgroundPressed")]
						public static ResourceValue<Brush> Pressed => new("FabSurfaceBackgroundPressed", true);

						[ResourceKeyDefinition(typeof(Brush), "FabSurfaceBackgroundPointerOver")]
						public static ResourceValue<Brush> PointerOver => new("FabSurfaceBackgroundPointerOver", true);

						[ResourceKeyDefinition(typeof(Brush), "FabSurfaceBackgroundDisabled")]
						public static ResourceValue<Brush> Disabled => new("FabSurfaceBackgroundDisabled", true);
					}
				}

				public static class StateOverlay
				{
					public static class Background
					{
						[ResourceKeyDefinition(typeof(Brush), "FabSurfaceStateOverlayBackground")]
						public static ResourceValue<Brush> Default => new("FabSurfaceStateOverlayBackground", true);

						[ResourceKeyDefinition(typeof(Brush), "FabSurfaceStateOverlayBackgroundPressed")]
						public static ResourceValue<Brush> Pressed => new("FabSurfaceStateOverlayBackgroundPressed", true);

						[ResourceKeyDefinition(typeof(Brush), "FabSurfaceStateOverlayBackgroundPointerOver")]
						public static ResourceValue<Brush> PointerOver => new("FabSurfaceStateOverlayBackgroundPointerOver", true);

						[ResourceKeyDefinition(typeof(Brush), "FabSurfaceStateOverlayBackgroundFocused")]
						public static ResourceValue<Brush> Focused => new("FabSurfaceStateOverlayBackgroundFocused", true);

						[ResourceKeyDefinition(typeof(Brush), "FabSurfaceStateOverlayBackgroundDisabled")]
						public static ResourceValue<Brush> Disabled => new("FabSurfaceStateOverlayBackgroundDisabled", true);
					}
				}

				public static class Secondary
				{
					public static class Foreground
					{
						[ResourceKeyDefinition(typeof(Brush), "FabSecondaryForeground")]
						public static ResourceValue<Brush> Default => new("FabSecondaryForeground", true);

						[ResourceKeyDefinition(typeof(Brush), "FabSecondaryForegroundPressed")]
						public static ResourceValue<Brush> Pressed => new("FabSecondaryForegroundPressed", true);

						[ResourceKeyDefinition(typeof(Brush), "FabSecondaryForegroundPointerOver")]
						public static ResourceValue<Brush> PointerOver => new("FabSecondaryForegroundPointerOver", true);

						[ResourceKeyDefinition(typeof(Brush), "FabSecondaryForegroundDisabled")]
						public static ResourceValue<Brush> Disabled => new("FabSecondaryForegroundDisabled", true);
					}

					public static class Background
					{
						[ResourceKeyDefinition(typeof(Brush), "FabSecondaryBackground")]
						public static ResourceValue<Brush> Default => new("FabSecondaryBackground", true);

						[ResourceKeyDefinition(typeof(Brush), "FabSecondaryBackgroundPressed")]
						public static ResourceValue<Brush> Pressed => new("FabSecondaryBackgroundPressed", true);

						[ResourceKeyDefinition(typeof(Brush), "FabSecondaryBackgroundPointerOver")]
						public static ResourceValue<Brush> PointerOver => new("FabSecondaryBackgroundPointerOver", true);

						[ResourceKeyDefinition(typeof(Brush), "FabSecondaryBackgroundDisabled")]
						public static ResourceValue<Brush> Disabled => new("FabSecondaryBackgroundDisabled", true);
					}

					public static class StateOverlay
					{
						public static class Background
						{
							[ResourceKeyDefinition(typeof(Brush), "FabSecondaryStateOverlayBackground")]
							public static ResourceValue<Brush> Default => new("FabSecondaryStateOverlayBackground", true);

							[ResourceKeyDefinition(typeof(Brush), "FabSecondaryStateOverlayBackgroundPressed")]
							public static ResourceValue<Brush> Pressed => new("FabSecondaryStateOverlayBackgroundPressed", true);

							[ResourceKeyDefinition(typeof(Brush), "FabSecondaryStateOverlayBackgroundPointerOver")]
							public static ResourceValue<Brush> PointerOver => new("FabSecondaryStateOverlayBackgroundPointerOver", true);

							[ResourceKeyDefinition(typeof(Brush), "FabSecondaryStateOverlayBackgroundFocused")]
							public static ResourceValue<Brush> Focused => new("FabSecondaryStateOverlayBackgroundFocused", true);

							[ResourceKeyDefinition(typeof(Brush), "FabSecondaryStateOverlayBackgroundDisabled")]
							public static ResourceValue<Brush> Disabled => new("FabSecondaryStateOverlayBackgroundDisabled", true);
						}
					}
				}

				public static class Tertiary
				{
					public static class Foreground
					{
						[ResourceKeyDefinition(typeof(Brush), "FabTertiaryForeground")]
						public static ResourceValue<Brush> Default => new("FabTertiaryForeground", true);

						[ResourceKeyDefinition(typeof(Brush), "FabTertiaryForegroundPressed")]
						public static ResourceValue<Brush> Pressed => new("FabTertiaryForegroundPressed", true);

						[ResourceKeyDefinition(typeof(Brush), "FabTertiaryForegroundPointerOver")]
						public static ResourceValue<Brush> PointerOver => new("FabTertiaryForegroundPointerOver", true);

						[ResourceKeyDefinition(typeof(Brush), "FabTertiaryForegroundDisabled")]
						public static ResourceValue<Brush> Disabled => new("FabTertiaryForegroundDisabled", true);
					}

					public static class Background
					{
						[ResourceKeyDefinition(typeof(Brush), "FabTertiaryBackground")]
						public static ResourceValue<Brush> Default => new("FabTertiaryBackground", true);

						[ResourceKeyDefinition(typeof(Brush), "FabTertiaryBackgroundPressed")]
						public static ResourceValue<Brush> Pressed => new("FabTertiaryBackgroundPressed", true);

						[ResourceKeyDefinition(typeof(Brush), "FabTertiaryBackgroundPointerOver")]
						public static ResourceValue<Brush> PointerOver => new("FabTertiaryBackgroundPointerOver", true);

						[ResourceKeyDefinition(typeof(Brush), "FabTertiaryBackgroundDisabled")]
						public static ResourceValue<Brush> Disabled => new("FabTertiaryBackgroundDisabled", true);
					}

					public static class StateOverlay
					{
						public static class Background
						{
							[ResourceKeyDefinition(typeof(Brush), "FabTertiaryStateOverlayBackground")]
							public static ResourceValue<Brush> Default => new("FabTertiaryStateOverlayBackground", true);

							[ResourceKeyDefinition(typeof(Brush), "FabTertiaryStateOverlayBackgroundPressed")]
							public static ResourceValue<Brush> Pressed => new("FabTertiaryStateOverlayBackgroundPressed", true);

							[ResourceKeyDefinition(typeof(Brush), "FabTertiaryStateOverlayBackgroundPointerOver")]
							public static ResourceValue<Brush> PointerOver => new("FabTertiaryStateOverlayBackgroundPointerOver", true);

							[ResourceKeyDefinition(typeof(Brush), "FabTertiaryStateOverlayBackgroundFocused")]
							public static ResourceValue<Brush> Focused => new("FabTertiaryStateOverlayBackgroundFocused", true);

							[ResourceKeyDefinition(typeof(Brush), "FabTertiaryStateOverlayBackgroundDisabled")]
							public static ResourceValue<Brush> Disabled => new("FabTertiaryStateOverlayBackgroundDisabled", true);
						}
					}
				}
			}

			public static class Styles
			{
				[ResourceKeyDefinition(typeof(Style), "MaterialFabStyle", TargetType = typeof(Button))]
				public static ResourceValue<Style> Default => new("MaterialFabStyle");

				[ResourceKeyDefinition(typeof(Style), "MaterialSurfaceFabStyle", TargetType = typeof(Button))]
				public static ResourceValue<Style> Surface => new("MaterialSurfaceFabStyle");

				[ResourceKeyDefinition(typeof(Style), "MaterialSecondaryFabStyle", TargetType = typeof(Button))]
				public static ResourceValue<Style> Secondary => new("MaterialSecondaryFabStyle");

				[ResourceKeyDefinition(typeof(Style), "MaterialTertiaryFabStyle", TargetType = typeof(Button))]
				public static ResourceValue<Style> Tertiary => new("MaterialTertiaryFabStyle");

				[ResourceKeyDefinition(typeof(Style), "MaterialSmallFabStyle", TargetType = typeof(Button))]
				public static ResourceValue<Style> Small => new("MaterialSmallFabStyle");

				[ResourceKeyDefinition(typeof(Style), "MaterialSurfaceSmallFabStyle", TargetType = typeof(Button))]
				public static ResourceValue<Style> SurfaceSmall => new("MaterialSurfaceSmallFabStyle");

				[ResourceKeyDefinition(typeof(Style), "MaterialSecondarySmallFabStyle", TargetType = typeof(Button))]
				public static ResourceValue<Style> SecondarySmall => new("MaterialSecondarySmallFabStyle");

				[ResourceKeyDefinition(typeof(Style), "MaterialTertiarySmallFabStyle", TargetType = typeof(Button))]
				public static ResourceValue<Style> TertiarySmall => new("MaterialTertiarySmallFabStyle");

				[ResourceKeyDefinition(typeof(Style), "MaterialLargeFabStyle", TargetType = typeof(Button))]
				public static ResourceValue<Style> Large => new("MaterialLargeFabStyle");

				[ResourceKeyDefinition(typeof(Style), "MaterialSurfaceLargeFabStyle", TargetType = typeof(Button))]
				public static ResourceValue<Style> SurfaceLarge => new("MaterialSurfaceLargeFabStyle");

				[ResourceKeyDefinition(typeof(Style), "MaterialSecondaryLargeFabStyle", TargetType = typeof(Button))]
				public static ResourceValue<Style> SecondaryLarge => new("MaterialSecondaryLargeFabStyle");

				[ResourceKeyDefinition(typeof(Style), "MaterialTertiaryLargeFabStyle", TargetType = typeof(Button))]
				public static ResourceValue<Style> TertiaryLarge => new("MaterialTertiaryLargeFabStyle");
			}
		}
	}
}
