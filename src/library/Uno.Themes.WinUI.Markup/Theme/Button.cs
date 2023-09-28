using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Media;
using Uno.Extensions.Markup;
using Uno.Extensions.Markup.Internals;

namespace Uno.Themes.Markup
{
	public static partial class Theme
	{
		public class BrushResource
		{
			private readonly string DefaultKey;
			public BrushResource(string defaultKey)
			{
				DefaultKey = defaultKey;
			}

			public static implicit operator ThemeResourceKey<Brush>(BrushResource brushResource) => brushResource.Default;

			public ThemeResourceKey<Brush> Default => new(DefaultKey);
		}

		public static class Button
		{
			public static class Resources
			{
				public static class Elevated
				{
					public static readonly BrushResource Foreground = new("ElevatedButtonForeground");

					public static class IconForeground
					{
						[ResourceKeyDefinition(typeof(Brush), "ElevatedButtonIconForeground")]
						public static ThemeResourceKey<Brush> Default => new("ElevatedButtonIconForeground");

						[ResourceKeyDefinition(typeof(Brush), "ElevatedButtonIconForegroundPointerOver")]
						public static ThemeResourceKey<Brush> PointerOver => new("ElevatedButtonIconForegroundPointerOver");

						[ResourceKeyDefinition(typeof(Brush), "ElevatedButtonIconForegroundPressed")]
						public static ThemeResourceKey<Brush> Pressed => new("ElevatedButtonIconForegroundPressed");

						[ResourceKeyDefinition(typeof(Brush), "ElevatedButtonIconForegroundFocused")]
						public static ThemeResourceKey<Brush> Focused => new("ElevatedButtonIconForegroundFocused");

						[ResourceKeyDefinition(typeof(Brush), "ElevatedButtonIconForegroundPointerFocused")]
						public static ThemeResourceKey<Brush> PointerFocused => new("ElevatedButtonIconForegroundPointerFocused");

						[ResourceKeyDefinition(typeof(Brush), "ElevatedButtonIconForegroundDisabled")]
						public static ThemeResourceKey<Brush> Disabled => new("ElevatedButtonIconForegroundDisabled");
					}

					public static class Background
					{
						[ResourceKeyDefinition(typeof(Brush), "ElevatedButtonBackground")]
						public static ThemeResourceKey<Brush> Default => new("ElevatedButtonBackground");

						[ResourceKeyDefinition(typeof(Brush), "ElevatedButtonBackgroundPointerOver")]
						public static ThemeResourceKey<Brush> PointerOver => new("ElevatedButtonBackgroundPointerOver");

						[ResourceKeyDefinition(typeof(Brush), "ElevatedButtonBackgroundPressed")]
						public static ThemeResourceKey<Brush> Pressed => new("ElevatedButtonBackgroundPressed");

						[ResourceKeyDefinition(typeof(Brush), "ElevatedButtonBackgroundFocused")]
						public static ThemeResourceKey<Brush> Focused => new("ElevatedButtonBackgroundFocused");

						[ResourceKeyDefinition(typeof(Brush), "ElevatedButtonBackgroundPointerFocused")]
						public static ThemeResourceKey<Brush> PointerFocused => new("ElevatedButtonBackgroundPointerFocused");

						[ResourceKeyDefinition(typeof(Brush), "ElevatedButtonBackgroundDisabled")]
						public static ThemeResourceKey<Brush> Disabled => new("ElevatedButtonBackgroundDisabled");
					}

					public static class BorderBrush
					{
						[ResourceKeyDefinition(typeof(Brush), "ElevatedButtonBorderBrush")]
						public static ThemeResourceKey<Brush> Default => new("ElevatedButtonBorderBrush");

						[ResourceKeyDefinition(typeof(Brush), "ElevatedButtonBorderBrushPointerOver")]
						public static ThemeResourceKey<Brush> PointerOver => new("ElevatedButtonBorderBrushPointerOver");

						[ResourceKeyDefinition(typeof(Brush), "ElevatedButtonBorderBrushPressed")]
						public static ThemeResourceKey<Brush> Pressed => new("ElevatedButtonBorderBrushPressed");

						[ResourceKeyDefinition(typeof(Brush), "ElevatedButtonBorderBrushFocused")]
						public static ThemeResourceKey<Brush> Focused => new("ElevatedButtonBorderBrushFocused");

						[ResourceKeyDefinition(typeof(Brush), "ElevatedButtonBorderBrushPointerFocused")]
						public static ThemeResourceKey<Brush> PointerFocused => new("ElevatedButtonBorderBrushPointerFocused");

						[ResourceKeyDefinition(typeof(Brush), "ElevatedButtonBorderBrushDisabled")]
						public static ThemeResourceKey<Brush> Disabled => new("ElevatedButtonBorderBrushDisabled");
					}
				}

				public static class Filled
				{
					public static class Foreground
					{
						[ResourceKeyDefinition(typeof(Brush), "FilledButtonForeground")]
						public static ThemeResourceKey<Brush> Default => new("FilledButtonForeground");

						[ResourceKeyDefinition(typeof(Brush), "FilledButtonForegroundPointerOver")]
						public static ThemeResourceKey<Brush> PointerOver => new("FilledButtonForegroundPointerOver");

						[ResourceKeyDefinition(typeof(Brush), "FilledButtonForegroundPressed")]
						public static ThemeResourceKey<Brush> Pressed => new("FilledButtonForegroundPressed");

						[ResourceKeyDefinition(typeof(Brush), "FilledButtonForegroundFocused")]
						public static ThemeResourceKey<Brush> Focused => new("FilledButtonForegroundFocused");

						[ResourceKeyDefinition(typeof(Brush), "FilledButtonForegroundPointerFocused")]
						public static ThemeResourceKey<Brush> PointerFocused => new("FilledButtonForegroundPointerFocused");

						[ResourceKeyDefinition(typeof(Brush), "FilledButtonForegroundDisabled")]
						public static ThemeResourceKey<Brush> Disabled => new("FilledButtonForegroundDisabled");
					}

					public static class IconForeground
					{
						[ResourceKeyDefinition(typeof(Brush), "FilledButtonIconForeground")]
						public static ThemeResourceKey<Brush> Default => new("FilledButtonIconForeground");

						[ResourceKeyDefinition(typeof(Brush), "FilledButtonIconForegroundPointerOver")]
						public static ThemeResourceKey<Brush> PointerOver => new("FilledButtonIconForegroundPointerOver");

						[ResourceKeyDefinition(typeof(Brush), "FilledButtonIconForegroundPressed")]
						public static ThemeResourceKey<Brush> Pressed => new("FilledButtonIconForegroundPressed");

						[ResourceKeyDefinition(typeof(Brush), "FilledButtonIconForegroundFocused")]
						public static ThemeResourceKey<Brush> Focused => new("FilledButtonIconForegroundFocused");

						[ResourceKeyDefinition(typeof(Brush), "FilledButtonIconForegroundPointerFocused")]
						public static ThemeResourceKey<Brush> PointerFocused => new("FilledButtonIconForegroundPointerFocused");

						[ResourceKeyDefinition(typeof(Brush), "FilledButtonIconForegroundDisabled")]
						public static ThemeResourceKey<Brush> Disabled => new("FilledButtonIconForegroundDisabled");
					}

					public static class Background
					{
						[ResourceKeyDefinition(typeof(Brush), "FilledButtonBackground")]
						public static ThemeResourceKey<Brush> Default => new("FilledButtonBackground");

						[ResourceKeyDefinition(typeof(Brush), "FilledButtonBackgroundPointerOver")]
						public static ThemeResourceKey<Brush> PointerOver => new("FilledButtonBackgroundPointerOver");

						[ResourceKeyDefinition(typeof(Brush), "FilledButtonBackgroundPressed")]
						public static ThemeResourceKey<Brush> Pressed => new("FilledButtonBackgroundPressed");

						[ResourceKeyDefinition(typeof(Brush), "FilledButtonBackgroundFocused")]
						public static ThemeResourceKey<Brush> Focused => new("FilledButtonBackgroundFocused");

						[ResourceKeyDefinition(typeof(Brush), "FilledButtonBackgroundPointerFocused")]
						public static ThemeResourceKey<Brush> PointerFocused => new("FilledButtonBackgroundPointerFocused");

						[ResourceKeyDefinition(typeof(Brush), "FilledButtonBackgroundDisabled")]
						public static ThemeResourceKey<Brush> Disabled => new("FilledButtonBackgroundDisabled");
					}

					public static class BorderBrush
					{
						[ResourceKeyDefinition(typeof(Brush), "FilledButtonBorderBrush")]
						public static ThemeResourceKey<Brush> Default => new("FilledButtonBorderBrush");

						[ResourceKeyDefinition(typeof(Brush), "FilledButtonBorderBrushPointerOver")]
						public static ThemeResourceKey<Brush> PointerOver => new("FilledButtonBorderBrushPointerOver");

						[ResourceKeyDefinition(typeof(Brush), "FilledButtonBorderBrushPressed")]
						public static ThemeResourceKey<Brush> Pressed => new("FilledButtonBorderBrushPressed");

						[ResourceKeyDefinition(typeof(Brush), "FilledButtonBorderBrushFocused")]
						public static ThemeResourceKey<Brush> Focused => new("FilledButtonBorderBrushFocused");

						[ResourceKeyDefinition(typeof(Brush), "FilledButtonBorderBrushPointerFocused")]
						public static ThemeResourceKey<Brush> PointerFocused => new("FilledButtonBorderBrushPointerFocused");

						[ResourceKeyDefinition(typeof(Brush), "FilledButtonBorderBrushDisabled")]
						public static ThemeResourceKey<Brush> Disabled => new("FilledButtonBorderBrushDisabled");
					}
				}

				public static class FilledTonal
				{
					public static class Foreground
					{
						[ResourceKeyDefinition(typeof(Brush), "FilledTonalButtonForeground")]
						public static ThemeResourceKey<Brush> Default => new("FilledTonalButtonForeground");

						[ResourceKeyDefinition(typeof(Brush), "FilledTonalButtonForegroundPointerOver")]
						public static ThemeResourceKey<Brush> PointerOver => new("FilledTonalButtonForegroundPointerOver");

						[ResourceKeyDefinition(typeof(Brush), "FilledTonalButtonForegroundPressed")]
						public static ThemeResourceKey<Brush> Pressed => new("FilledTonalButtonForegroundPressed");

						[ResourceKeyDefinition(typeof(Brush), "FilledTonalButtonForegroundFocused")]
						public static ThemeResourceKey<Brush> Focused => new("FilledTonalButtonForegroundFocused");

						[ResourceKeyDefinition(typeof(Brush), "FilledTonalButtonForegroundPointerFocused")]
						public static ThemeResourceKey<Brush> PointerFocused => new("FilledTonalButtonForegroundPointerFocused");

						[ResourceKeyDefinition(typeof(Brush), "FilledTonalButtonForegroundDisabled")]
						public static ThemeResourceKey<Brush> Disabled => new("FilledTonalButtonForegroundDisabled");
					}

					public static class IconForeground
					{
						[ResourceKeyDefinition(typeof(Brush), "FilledTonalButtonIconForeground")]
						public static ThemeResourceKey<Brush> Default => new("FilledTonalButtonIconForeground");

						[ResourceKeyDefinition(typeof(Brush), "FilledTonalButtonIconForegroundPointerOver")]
						public static ThemeResourceKey<Brush> PointerOver => new("FilledTonalButtonIconForegroundPointerOver");

						[ResourceKeyDefinition(typeof(Brush), "FilledTonalButtonIconForegroundPressed")]
						public static ThemeResourceKey<Brush> Pressed => new("FilledTonalButtonIconForegroundPressed");

						[ResourceKeyDefinition(typeof(Brush), "FilledTonalButtonIconForegroundFocused")]
						public static ThemeResourceKey<Brush> Focused => new("FilledTonalButtonIconForegroundFocused");

						[ResourceKeyDefinition(typeof(Brush), "FilledTonalButtonIconForegroundPointerFocused")]
						public static ThemeResourceKey<Brush> PointerFocused => new("FilledTonalButtonIconForegroundPointerFocused");

						[ResourceKeyDefinition(typeof(Brush), "FilledTonalButtonIconForegroundDisabled")]
						public static ThemeResourceKey<Brush> Disabled => new("FilledTonalButtonIconForegroundDisabled");
					}

					public static class Background
					{
						[ResourceKeyDefinition(typeof(Brush), "FilledTonalButtonBackground")]
						public static ThemeResourceKey<Brush> Default => new("FilledTonalButtonBackground");

						[ResourceKeyDefinition(typeof(Brush), "FilledTonalButtonBackgroundPointerOver")]
						public static ThemeResourceKey<Brush> PointerOver => new("FilledTonalButtonBackgroundPointerOver");

						[ResourceKeyDefinition(typeof(Brush), "FilledTonalButtonBackgroundPressed")]
						public static ThemeResourceKey<Brush> Pressed => new("FilledTonalButtonBackgroundPressed");

						[ResourceKeyDefinition(typeof(Brush), "FilledTonalButtonBackgroundFocused")]
						public static ThemeResourceKey<Brush> Focused => new("FilledTonalButtonBackgroundFocused");

						[ResourceKeyDefinition(typeof(Brush), "FilledTonalButtonBackgroundPointerFocused")]
						public static ThemeResourceKey<Brush> PointerFocused => new("FilledTonalButtonBackgroundPointerFocused");

						[ResourceKeyDefinition(typeof(Brush), "FilledTonalButtonBackgroundDisabled")]
						public static ThemeResourceKey<Brush> Disabled => new("FilledTonalButtonBackgroundDisabled");
					}

					public static class BorderBrush
					{
						[ResourceKeyDefinition(typeof(Brush), "FilledTonalButtonBorderBrush")]
						public static ThemeResourceKey<Brush> Default => new("FilledTonalButtonBorderBrush");

						[ResourceKeyDefinition(typeof(Brush), "FilledTonalButtonBorderBrushPointerOver")]
						public static ThemeResourceKey<Brush> PointerOver => new("FilledTonalButtonBorderBrushPointerOver");

						[ResourceKeyDefinition(typeof(Brush), "FilledTonalButtonBorderBrushPressed")]
						public static ThemeResourceKey<Brush> Pressed => new("FilledTonalButtonBorderBrushPressed");

						[ResourceKeyDefinition(typeof(Brush), "FilledTonalButtonBorderBrushFocused")]
						public static ThemeResourceKey<Brush> Focused => new("FilledTonalButtonBorderBrushFocused");

						[ResourceKeyDefinition(typeof(Brush), "FilledTonalButtonBorderBrushPointerFocused")]
						public static ThemeResourceKey<Brush> PointerFocused => new("FilledTonalButtonBorderBrushPointerFocused");

						[ResourceKeyDefinition(typeof(Brush), "FilledTonalButtonBorderBrushDisabled")]
						public static ThemeResourceKey<Brush> Disabled => new("FilledTonalButtonBorderBrushDisabled");
					}
				}

				public static class Outlined
				{
					public static class Foreground
					{
						[ResourceKeyDefinition(typeof(Brush), "OutlinedButtonForeground")]
						public static ThemeResourceKey<Brush> Default => new("OutlinedButtonForeground");

						[ResourceKeyDefinition(typeof(Brush), "OutlinedButtonForegroundPointerOver")]
						public static ThemeResourceKey<Brush> PointerOver => new("OutlinedButtonForegroundPointerOver");

						[ResourceKeyDefinition(typeof(Brush), "OutlinedButtonForegroundPressed")]
						public static ThemeResourceKey<Brush> Pressed => new("OutlinedButtonForegroundPressed");

						[ResourceKeyDefinition(typeof(Brush), "OutlinedButtonForegroundFocused")]
						public static ThemeResourceKey<Brush> Focused => new("OutlinedButtonForegroundFocused");

						[ResourceKeyDefinition(typeof(Brush), "OutlinedButtonForegroundPointerFocused")]
						public static ThemeResourceKey<Brush> PointerFocused => new("OutlinedButtonForegroundPointerFocused");

						[ResourceKeyDefinition(typeof(Brush), "OutlinedButtonForegroundDisabled")]
						public static ThemeResourceKey<Brush> Disabled => new("OutlinedButtonForegroundDisabled");
					}

					public static class IconForeground
					{
						[ResourceKeyDefinition(typeof(Brush), "OutlinedButtonIconForeground")]
						public static ThemeResourceKey<Brush> Default => new("OutlinedButtonIconForeground");

						[ResourceKeyDefinition(typeof(Brush), "OutlinedButtonIconForegroundPointerOver")]
						public static ThemeResourceKey<Brush> PointerOver => new("OutlinedButtonIconForegroundPointerOver");

						[ResourceKeyDefinition(typeof(Brush), "OutlinedButtonIconForegroundPressed")]
						public static ThemeResourceKey<Brush> Pressed => new("OutlinedButtonIconForegroundPressed");

						[ResourceKeyDefinition(typeof(Brush), "OutlinedButtonIconForegroundFocused")]
						public static ThemeResourceKey<Brush> Focused => new("OutlinedButtonIconForegroundFocused");

						[ResourceKeyDefinition(typeof(Brush), "OutlinedButtonIconForegroundPointerFocused")]
						public static ThemeResourceKey<Brush> PointerFocused => new("OutlinedButtonIconForegroundPointerFocused");

						[ResourceKeyDefinition(typeof(Brush), "OutlinedButtonIconForegroundDisabled")]
						public static ThemeResourceKey<Brush> Disabled => new("OutlinedButtonIconForegroundDisabled");
					}

					public static class Background
					{
						[ResourceKeyDefinition(typeof(Brush), "OutlinedButtonBackground")]
						public static ThemeResourceKey<Brush> Default => new("OutlinedButtonBackground");

						[ResourceKeyDefinition(typeof(Brush), "OutlinedButtonBackgroundPointerOver")]
						public static ThemeResourceKey<Brush> PointerOver => new("OutlinedButtonBackgroundPointerOver");

						[ResourceKeyDefinition(typeof(Brush), "OutlinedButtonBackgroundPressed")]
						public static ThemeResourceKey<Brush> Pressed => new("OutlinedButtonBackgroundPressed");

						[ResourceKeyDefinition(typeof(Brush), "OutlinedButtonBackgroundFocused")]
						public static ThemeResourceKey<Brush> Focused => new("OutlinedButtonBackgroundFocused");

						[ResourceKeyDefinition(typeof(Brush), "OutlinedButtonBackgroundPointerFocused")]
						public static ThemeResourceKey<Brush> PointerFocused => new("OutlinedButtonBackgroundPointerFocused");

						[ResourceKeyDefinition(typeof(Brush), "OutlinedButtonBackgroundDisabled")]
						public static ThemeResourceKey<Brush> Disabled => new("OutlinedButtonBackgroundDisabled");
					}

					public static class BorderBrush
					{
						[ResourceKeyDefinition(typeof(Brush), "OutlinedButtonBorderBrush")]
						public static ThemeResourceKey<Brush> Default => new("OutlinedButtonBorderBrush");

						[ResourceKeyDefinition(typeof(Brush), "OutlinedButtonBorderBrushPointerOver")]
						public static ThemeResourceKey<Brush> PointerOver => new("OutlinedButtonBorderBrushPointerOver");

						[ResourceKeyDefinition(typeof(Brush), "OutlinedButtonBorderBrushPressed")]
						public static ThemeResourceKey<Brush> Pressed => new("OutlinedButtonBorderBrushPressed");

						[ResourceKeyDefinition(typeof(Brush), "OutlinedButtonBorderBrushFocused")]
						public static ThemeResourceKey<Brush> Focused => new("OutlinedButtonBorderBrushFocused");

						[ResourceKeyDefinition(typeof(Brush), "OutlinedButtonBorderBrushPointerFocused")]
						public static ThemeResourceKey<Brush> PointerFocused => new("OutlinedButtonBorderBrushPointerFocused");

						[ResourceKeyDefinition(typeof(Brush), "OutlinedButtonBorderBrushDisabled")]
						public static ThemeResourceKey<Brush> Disabled => new("OutlinedButtonBorderBrushDisabled");
					}
				}

				public static class Text
				{
					public static class Foreground
					{
						[ResourceKeyDefinition(typeof(Brush), "TextButtonForeground")]
						public static ThemeResourceKey<Brush> Default => new("TextButtonForeground");

						[ResourceKeyDefinition(typeof(Brush), "TextButtonForegroundPointerOver")]
						public static ThemeResourceKey<Brush> PointerOver => new("TextButtonForegroundPointerOver");

						[ResourceKeyDefinition(typeof(Brush), "TextButtonForegroundPressed")]
						public static ThemeResourceKey<Brush> Pressed => new("TextButtonForegroundPressed");

						[ResourceKeyDefinition(typeof(Brush), "TextButtonForegroundFocused")]
						public static ThemeResourceKey<Brush> Focused => new("TextButtonForegroundFocused");

						[ResourceKeyDefinition(typeof(Brush), "TextButtonForegroundPointerFocused")]
						public static ThemeResourceKey<Brush> PointerFocused => new("TextButtonForegroundPointerFocused");

						[ResourceKeyDefinition(typeof(Brush), "TextButtonForegroundDisabled")]
						public static ThemeResourceKey<Brush> Disabled => new("TextButtonForegroundDisabled");
					}

					public static class IconForeground
					{
						[ResourceKeyDefinition(typeof(Brush), "TextButtonIconForeground")]
						public static ThemeResourceKey<Brush> Default => new("TextButtonIconForeground");

						[ResourceKeyDefinition(typeof(Brush), "TextButtonIconForegroundPointerOver")]
						public static ThemeResourceKey<Brush> PointerOver => new("TextButtonIconForegroundPointerOver");

						[ResourceKeyDefinition(typeof(Brush), "TextButtonIconForegroundPressed")]
						public static ThemeResourceKey<Brush> Pressed => new("TextButtonIconForegroundPressed");

						[ResourceKeyDefinition(typeof(Brush), "TextButtonIconForegroundFocused")]
						public static ThemeResourceKey<Brush> Focused => new("TextButtonIconForegroundFocused");

						[ResourceKeyDefinition(typeof(Brush), "TextButtonIconForegroundPointerFocused")]
						public static ThemeResourceKey<Brush> PointerFocused => new("TextButtonIconForegroundPointerFocused");

						[ResourceKeyDefinition(typeof(Brush), "TextButtonIconForegroundDisabled")]
						public static ThemeResourceKey<Brush> Disabled => new("TextButtonIconForegroundDisabled");
					}

					public static class Background
					{
						[ResourceKeyDefinition(typeof(Brush), "TextButtonBackground")]
						public static ThemeResourceKey<Brush> Default => new("TextButtonBackground");

						[ResourceKeyDefinition(typeof(Brush), "TextButtonBackgroundPointerOver")]
						public static ThemeResourceKey<Brush> PointerOver => new("TextButtonBackgroundPointerOver");

						[ResourceKeyDefinition(typeof(Brush), "TextButtonBackgroundPressed")]
						public static ThemeResourceKey<Brush> Pressed => new("TextButtonBackgroundPressed");

						[ResourceKeyDefinition(typeof(Brush), "TextButtonBackgroundFocused")]
						public static ThemeResourceKey<Brush> Focused => new("TextButtonBackgroundFocused");

						[ResourceKeyDefinition(typeof(Brush), "TextButtonBackgroundPointerFocused")]
						public static ThemeResourceKey<Brush> PointerFocused => new("TextButtonBackgroundPointerFocused");

						[ResourceKeyDefinition(typeof(Brush), "TextButtonBackgroundDisabled")]
						public static ThemeResourceKey<Brush> Disabled => new("TextButtonBackgroundDisabled");
					}

					public static class BorderBrush
					{
						[ResourceKeyDefinition(typeof(Brush), "TextButtonBorderBrush")]
						public static ThemeResourceKey<Brush> Default => new("TextButtonBorderBrush");

						[ResourceKeyDefinition(typeof(Brush), "TextButtonBorderBrushPointerOver")]
						public static ThemeResourceKey<Brush> PointerOver => new("TextButtonBorderBrushPointerOver");

						[ResourceKeyDefinition(typeof(Brush), "TextButtonBorderBrushPressed")]
						public static ThemeResourceKey<Brush> Pressed => new("TextButtonBorderBrushPressed");

						[ResourceKeyDefinition(typeof(Brush), "TextButtonBorderBrushFocused")]
						public static ThemeResourceKey<Brush> Focused => new("TextButtonBorderBrushFocused");

						[ResourceKeyDefinition(typeof(Brush), "TextButtonBorderBrushPointerFocused")]
						public static ThemeResourceKey<Brush> PointerFocused => new("TextButtonBorderBrushPointerFocused");

						[ResourceKeyDefinition(typeof(Brush), "TextButtonBorderBrushDisabled")]
						public static ThemeResourceKey<Brush> Disabled => new("TextButtonBorderBrushDisabled");
					}
				}

				public static class Icon
				{
					[ResourceKeyDefinition(typeof(Brush), "IconButtonForeground")]
					public static BrushResource Foreground => new("IconButtonForeground");

					public static class EllipseFill
					{
						[ResourceKeyDefinition(typeof(Brush), "IconButtonEllipseFillPointerOver")]
						public static ThemeResourceKey<Brush> PointerOver => new("IconButtonEllipseFillPointerOver");

						[ResourceKeyDefinition(typeof(Brush), "IconButtonEllipseFillPressed")]
						public static ThemeResourceKey<Brush> Pressed => new("IconButtonEllipseFillPressed");

						[ResourceKeyDefinition(typeof(Brush), "IconButtonEllipseFillFocused")]
						public static ThemeResourceKey<Brush> Focused => new("IconButtonEllipseFillFocused");
					}
				}
			}

			public static class Styles
			{
				[ResourceKeyDefinition(typeof(Style), "ElevatedButtonStyle", TargetType = typeof(Button))]
				public static StaticResourceKey<Style> Elevated => new("ElevatedButtonStyle");

				[ResourceKeyDefinition(typeof(Style), "FilledButtonStyle", TargetType = typeof(Button))]
				public static StaticResourceKey<Style> Filled => new("FilledButtonStyle");

				[ResourceKeyDefinition(typeof(Style), "FilledTonalButtonStyle", TargetType = typeof(Button))]
				public static StaticResourceKey<Style> FilledTonal => new("FilledTonalButtonStyle");

				[ResourceKeyDefinition(typeof(Style), "OutlinedButtonStyle", TargetType = typeof(Button))]
				public static StaticResourceKey<Style> Outlined => new("OutlinedButtonStyle");

				[ResourceKeyDefinition(typeof(Style), "TextButtonStyle", TargetType = typeof(Button))]
				public static StaticResourceKey<Style> Text => new("TextButtonStyle");

				[ResourceKeyDefinition(typeof(Style), "IconButtonStyle", TargetType = typeof(Button))]
				public static StaticResourceKey<Style> Icon => new("IconButtonStyle");

				[ResourceKeyDefinition(typeof(Style), "FabStyle", TargetType = typeof(Button))]
				public static StaticResourceKey<Style> Fab => new("FabStyle");

				[ResourceKeyDefinition(typeof(Style), "SurfaceFabStyle", TargetType = typeof(Button))]
				public static StaticResourceKey<Style> SurfaceFab => new("SurfaceFabStyle");

				[ResourceKeyDefinition(typeof(Style), "SecondaryFabStyle", TargetType = typeof(Button))]
				public static StaticResourceKey<Style> SecondaryFab => new("SecondaryFabStyle");

				[ResourceKeyDefinition(typeof(Style), "TertiaryFabStyle", TargetType = typeof(Button))]
				public static StaticResourceKey<Style> TertiaryFab => new("TertiaryFabStyle");

				[ResourceKeyDefinition(typeof(Style), "SmallFabStyle", TargetType = typeof(Button))]
				public static StaticResourceKey<Style> SmallFab => new("SmallFabStyle");

				[ResourceKeyDefinition(typeof(Style), "SurfaceSmallFabStyle", TargetType = typeof(Button))]
				public static StaticResourceKey<Style> SurfaceSmallFab => new("SurfaceSmallFabStyle");

				[ResourceKeyDefinition(typeof(Style), "SecondarySmallFabStyle", TargetType = typeof(Button))]
				public static StaticResourceKey<Style> SecondarySmallFab => new("SecondarySmallFabStyle");

				[ResourceKeyDefinition(typeof(Style), "TertiarySmallFabStyle", TargetType = typeof(Button))]
				public static StaticResourceKey<Style> TertiarySmallFab => new("TertiarySmallFabStyle");

				[ResourceKeyDefinition(typeof(Style), "LargeFabStyle", TargetType = typeof(Button))]
				public static StaticResourceKey<Style> LargeFab => new("LargeFabStyle");

				[ResourceKeyDefinition(typeof(Style), "SurfaceLargeFabStyle", TargetType = typeof(Button))]
				public static StaticResourceKey<Style> SurfaceLargeFab => new("SurfaceLargeFabStyle");

				[ResourceKeyDefinition(typeof(Style), "SecondaryLargeFabStyle", TargetType = typeof(Button))]
				public static StaticResourceKey<Style> SecondaryLargeFab => new("SecondaryLargeFabStyle");

				[ResourceKeyDefinition(typeof(Style), "TertiaryLargeFabStyle", TargetType = typeof(Button))]
				public static StaticResourceKey<Style> TertiaryLargeFab => new("TertiaryLargeFabStyle");
			}
		}
	}
}
