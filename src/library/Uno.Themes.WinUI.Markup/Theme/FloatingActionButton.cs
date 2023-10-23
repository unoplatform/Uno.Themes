using System;
using Windows.UI;
using Microsoft.UI.Text;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Media;
using Uno.Extensions.Markup;
using Uno.Extensions.Markup.Internals;

namespace Uno.Themes.Markup;

public static partial class Theme
{
	public static partial class FloatingActionButton
	{
		public static partial class Resources
		{
			public static partial class Default
			{
				public static partial class Background
				{
					[ResourceKeyDefinition(typeof(Brush), "FabBackground")]
					public static ThemeResourceKey<Brush> Default => new("FabBackground");

					[ResourceKeyDefinition(typeof(Brush), "FabBackgroundPointerOver")]
					public static ThemeResourceKey<Brush> PointerOver => new("FabBackgroundPointerOver");

					[ResourceKeyDefinition(typeof(Brush), "FabBackgroundPressed")]
					public static ThemeResourceKey<Brush> Pressed => new("FabBackgroundPressed");

					[ResourceKeyDefinition(typeof(Brush), "FabBackgroundDisabled")]
					public static ThemeResourceKey<Brush> Disabled => new("FabBackgroundDisabled");
				}

				public static partial class Foreground
				{
					[ResourceKeyDefinition(typeof(Brush), "FabForeground")]
					public static ThemeResourceKey<Brush> Default => new("FabForeground");

					[ResourceKeyDefinition(typeof(Brush), "FabForegroundPointerOver")]
					public static ThemeResourceKey<Brush> PointerOver => new("FabForegroundPointerOver");

					[ResourceKeyDefinition(typeof(Brush), "FabForegroundPressed")]
					public static ThemeResourceKey<Brush> Pressed => new("FabForegroundPressed");

					[ResourceKeyDefinition(typeof(Brush), "FabForegroundDisabled")]
					public static ThemeResourceKey<Brush> Disabled => new("FabForegroundDisabled");
				}

				public static partial class StateOverlayBackground
				{
					[ResourceKeyDefinition(typeof(Brush), "FabStateOverlayBackgroundPointerOver")]
					public static ThemeResourceKey<Brush> PointerOver => new("FabStateOverlayBackgroundPointerOver");

					[ResourceKeyDefinition(typeof(Brush), "FabStateOverlayBackgroundPressed")]
					public static ThemeResourceKey<Brush> Pressed => new("FabStateOverlayBackgroundPressed");

					[ResourceKeyDefinition(typeof(Brush), "FabStateOverlayBackgroundDisabled")]
					public static ThemeResourceKey<Brush> Disabled => new("FabStateOverlayBackgroundDisabled");

					[ResourceKeyDefinition(typeof(Brush), "FabStateOverlayBackgroundFocused")]
					public static ThemeResourceKey<Brush> Focused => new("FabStateOverlayBackgroundFocused");
				}
			}

			public static partial class Secondary
			{
				public static partial class Background
				{
					[ResourceKeyDefinition(typeof(Brush), "FabSecondaryBackground")]
					public static ThemeResourceKey<Brush> Default => new("FabSecondaryBackground");

					[ResourceKeyDefinition(typeof(Brush), "FabSecondaryBackgroundPointerOver")]
					public static ThemeResourceKey<Brush> PointerOver => new("FabSecondaryBackgroundPointerOver");

					[ResourceKeyDefinition(typeof(Brush), "FabSecondaryBackgroundPressed")]
					public static ThemeResourceKey<Brush> Pressed => new("FabSecondaryBackgroundPressed");

					[ResourceKeyDefinition(typeof(Brush), "FabSecondaryBackgroundDisabled")]
					public static ThemeResourceKey<Brush> Disabled => new("FabSecondaryBackgroundDisabled");
				}

				public static partial class Foreground
				{
					[ResourceKeyDefinition(typeof(Brush), "FabSecondaryForeground")]
					public static ThemeResourceKey<Brush> Default => new("FabSecondaryForeground");

					[ResourceKeyDefinition(typeof(Brush), "FabSecondaryForegroundPointerOver")]
					public static ThemeResourceKey<Brush> PointerOver => new("FabSecondaryForegroundPointerOver");

					[ResourceKeyDefinition(typeof(Brush), "FabSecondaryForegroundPressed")]
					public static ThemeResourceKey<Brush> Pressed => new("FabSecondaryForegroundPressed");

					[ResourceKeyDefinition(typeof(Brush), "FabSecondaryForegroundDisabled")]
					public static ThemeResourceKey<Brush> Disabled => new("FabSecondaryForegroundDisabled");
				}

				public static partial class StateOverlayBackground
				{
					[ResourceKeyDefinition(typeof(Brush), "FabSecondaryStateOverlayBackgroundPointerOver")]
					public static ThemeResourceKey<Brush> PointerOver => new("FabSecondaryStateOverlayBackgroundPointerOver");

					[ResourceKeyDefinition(typeof(Brush), "FabSecondaryStateOverlayBackgroundPressed")]
					public static ThemeResourceKey<Brush> Pressed => new("FabSecondaryStateOverlayBackgroundPressed");

					[ResourceKeyDefinition(typeof(Brush), "FabSecondaryStateOverlayBackgroundDisabled")]
					public static ThemeResourceKey<Brush> Disabled => new("FabSecondaryStateOverlayBackgroundDisabled");

					[ResourceKeyDefinition(typeof(Brush), "FabSecondaryStateOverlayBackgroundFocused")]
					public static ThemeResourceKey<Brush> Focused => new("FabSecondaryStateOverlayBackgroundFocused");
				}
			}

			public static partial class Surface
			{
				public static partial class Background
				{
					[ResourceKeyDefinition(typeof(Brush), "FabSurfaceBackground")]
					public static ThemeResourceKey<Brush> Default => new("FabSurfaceBackground");

					[ResourceKeyDefinition(typeof(Brush), "FabSurfaceBackgroundPointerOver")]
					public static ThemeResourceKey<Brush> PointerOver => new("FabSurfaceBackgroundPointerOver");

					[ResourceKeyDefinition(typeof(Brush), "FabSurfaceBackgroundPressed")]
					public static ThemeResourceKey<Brush> Pressed => new("FabSurfaceBackgroundPressed");

					[ResourceKeyDefinition(typeof(Brush), "FabSurfaceBackgroundDisabled")]
					public static ThemeResourceKey<Brush> Disabled => new("FabSurfaceBackgroundDisabled");
				}

				public static partial class Foreground
				{
					[ResourceKeyDefinition(typeof(Brush), "FabSurfaceForeground")]
					public static ThemeResourceKey<Brush> Default => new("FabSurfaceForeground");

					[ResourceKeyDefinition(typeof(Brush), "FabSurfaceForegroundPointerOver")]
					public static ThemeResourceKey<Brush> PointerOver => new("FabSurfaceForegroundPointerOver");

					[ResourceKeyDefinition(typeof(Brush), "FabSurfaceForegroundPressed")]
					public static ThemeResourceKey<Brush> Pressed => new("FabSurfaceForegroundPressed");

					[ResourceKeyDefinition(typeof(Brush), "FabSurfaceForegroundDisabled")]
					public static ThemeResourceKey<Brush> Disabled => new("FabSurfaceForegroundDisabled");
				}

				public static partial class StateOverlayBackground
				{
					[ResourceKeyDefinition(typeof(Brush), "FabSurfaceStateOverlayBackgroundPointerOver")]
					public static ThemeResourceKey<Brush> PointerOver => new("FabSurfaceStateOverlayBackgroundPointerOver");

					[ResourceKeyDefinition(typeof(Brush), "FabSurfaceStateOverlayBackgroundPressed")]
					public static ThemeResourceKey<Brush> Pressed => new("FabSurfaceStateOverlayBackgroundPressed");

					[ResourceKeyDefinition(typeof(Brush), "FabSurfaceStateOverlayBackgroundDisabled")]
					public static ThemeResourceKey<Brush> Disabled => new("FabSurfaceStateOverlayBackgroundDisabled");

					[ResourceKeyDefinition(typeof(Brush), "FabSurfaceStateOverlayBackgroundFocused")]
					public static ThemeResourceKey<Brush> Focused => new("FabSurfaceStateOverlayBackgroundFocused");
				}
			}

			public static partial class Tertiary
			{
				public static partial class Background
				{
					[ResourceKeyDefinition(typeof(Brush), "FabTertiaryBackground")]
					public static ThemeResourceKey<Brush> Default => new("FabTertiaryBackground");

					[ResourceKeyDefinition(typeof(Brush), "FabTertiaryBackgroundPointerOver")]
					public static ThemeResourceKey<Brush> PointerOver => new("FabTertiaryBackgroundPointerOver");

					[ResourceKeyDefinition(typeof(Brush), "FabTertiaryBackgroundPressed")]
					public static ThemeResourceKey<Brush> Pressed => new("FabTertiaryBackgroundPressed");

					[ResourceKeyDefinition(typeof(Brush), "FabTertiaryBackgroundDisabled")]
					public static ThemeResourceKey<Brush> Disabled => new("FabTertiaryBackgroundDisabled");
				}

				public static partial class Foreground
				{
					[ResourceKeyDefinition(typeof(Brush), "FabTertiaryForeground")]
					public static ThemeResourceKey<Brush> Default => new("FabTertiaryForeground");

					[ResourceKeyDefinition(typeof(Brush), "FabTertiaryForegroundPointerOver")]
					public static ThemeResourceKey<Brush> PointerOver => new("FabTertiaryForegroundPointerOver");

					[ResourceKeyDefinition(typeof(Brush), "FabTertiaryForegroundPressed")]
					public static ThemeResourceKey<Brush> Pressed => new("FabTertiaryForegroundPressed");

					[ResourceKeyDefinition(typeof(Brush), "FabTertiaryForegroundDisabled")]
					public static ThemeResourceKey<Brush> Disabled => new("FabTertiaryForegroundDisabled");
				}

				public static partial class StateOverlayBackground
				{
					[ResourceKeyDefinition(typeof(Brush), "FabTertiaryStateOverlayBackgroundPointerOver")]
					public static ThemeResourceKey<Brush> PointerOver => new("FabTertiaryStateOverlayBackgroundPointerOver");

					[ResourceKeyDefinition(typeof(Brush), "FabTertiaryStateOverlayBackgroundPressed")]
					public static ThemeResourceKey<Brush> Pressed => new("FabTertiaryStateOverlayBackgroundPressed");

					[ResourceKeyDefinition(typeof(Brush), "FabTertiaryStateOverlayBackgroundDisabled")]
					public static ThemeResourceKey<Brush> Disabled => new("FabTertiaryStateOverlayBackgroundDisabled");

					[ResourceKeyDefinition(typeof(Brush), "FabTertiaryStateOverlayBackgroundFocused")]
					public static ThemeResourceKey<Brush> Focused => new("FabTertiaryStateOverlayBackgroundFocused");
				}
			}
		}

		public static partial class Styles
		{
			[ResourceKeyDefinition(typeof(Style), "FabStyle", TargetType = typeof(global::Microsoft.UI.Xaml.Controls.Button))]
			public static StaticResourceKey<Style> Default => new("FabStyle");

			[ResourceKeyDefinition(typeof(Style), "SurfaceFabStyle", TargetType = typeof(global::Microsoft.UI.Xaml.Controls.Button))]
			public static StaticResourceKey<Style> Surface => new("SurfaceFabStyle");

			[ResourceKeyDefinition(typeof(Style), "SecondaryFabStyle", TargetType = typeof(global::Microsoft.UI.Xaml.Controls.Button))]
			public static StaticResourceKey<Style> Secondary => new("SecondaryFabStyle");

			[ResourceKeyDefinition(typeof(Style), "TertiaryFabStyle", TargetType = typeof(global::Microsoft.UI.Xaml.Controls.Button))]
			public static StaticResourceKey<Style> Tertiary => new("TertiaryFabStyle");

			[ResourceKeyDefinition(typeof(Style), "SmallFabStyle", TargetType = typeof(global::Microsoft.UI.Xaml.Controls.Button))]
			public static StaticResourceKey<Style> Small => new("SmallFabStyle");

			[ResourceKeyDefinition(typeof(Style), "SurfaceSmallFabStyle", TargetType = typeof(global::Microsoft.UI.Xaml.Controls.Button))]
			public static StaticResourceKey<Style> SurfaceSmall => new("SurfaceSmallFabStyle");

			[ResourceKeyDefinition(typeof(Style), "SecondarySmallFabStyle", TargetType = typeof(global::Microsoft.UI.Xaml.Controls.Button))]
			public static StaticResourceKey<Style> SecondarySmall => new("SecondarySmallFabStyle");

			[ResourceKeyDefinition(typeof(Style), "TertiarySmallFabStyle", TargetType = typeof(global::Microsoft.UI.Xaml.Controls.Button))]
			public static StaticResourceKey<Style> TertiarySmall => new("TertiarySmallFabStyle");

			[ResourceKeyDefinition(typeof(Style), "LargeFabStyle", TargetType = typeof(global::Microsoft.UI.Xaml.Controls.Button))]
			public static StaticResourceKey<Style> Large => new("LargeFabStyle");

			[ResourceKeyDefinition(typeof(Style), "SurfaceLargeFabStyle", TargetType = typeof(global::Microsoft.UI.Xaml.Controls.Button))]
			public static StaticResourceKey<Style> SurfaceLarge => new("SurfaceLargeFabStyle");

			[ResourceKeyDefinition(typeof(Style), "SecondaryLargeFabStyle", TargetType = typeof(global::Microsoft.UI.Xaml.Controls.Button))]
			public static StaticResourceKey<Style> SecondaryLarge => new("SecondaryLargeFabStyle");

			[ResourceKeyDefinition(typeof(Style), "TertiaryLargeFabStyle", TargetType = typeof(global::Microsoft.UI.Xaml.Controls.Button))]
			public static StaticResourceKey<Style> TertiaryLarge => new("TertiaryLargeFabStyle");
		}
	}
}
