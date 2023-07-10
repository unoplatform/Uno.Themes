using Microsoft.UI.Xaml.Media;
using Uno.Extensions.Markup.Internals;

namespace Uno.Themes.Markup
{
	public static partial class Theme
	{
		public static class Button
		{
			public static class Resources
			{
				public static class Elevated
				{
					public static class Foreground
					{
						[ResourceKeyDefinition(typeof(Brush), "ElevatedButtonForeground")]
						public static ResourceValue<Brush> Default => new("ElevatedButtonForeground", true);

						[ResourceKeyDefinition(typeof(Brush), "ElevatedButtonForegroundPointerOver")]
						public static ResourceValue<Brush> PointerOver => new("ElevatedButtonForegroundPointerOver", true);

						[ResourceKeyDefinition(typeof(Brush), "ElevatedButtonForegroundPressed")]
						public static ResourceValue<Brush> Pressed => new("ElevatedButtonForegroundPressed", true);

						[ResourceKeyDefinition(typeof(Brush), "ElevatedButtonForegroundFocused")]
						public static ResourceValue<Brush> Focused => new("ElevatedButtonForegroundFocused", true);

						[ResourceKeyDefinition(typeof(Brush), "ElevatedButtonForegroundPointerFocused")]
						public static ResourceValue<Brush> PointerFocused => new("ElevatedButtonForegroundPointerFocused", true);

						[ResourceKeyDefinition(typeof(Brush), "ElevatedButtonForegroundDisabled")]
						public static ResourceValue<Brush> Disabled => new("ElevatedButtonForegroundDisabled", true);
					}

					public static class IconForeground
					{
						[ResourceKeyDefinition(typeof(Brush), "ElevatedButtonIconForeground")]
						public static ResourceValue<Brush> Default => new("ElevatedButtonIconForeground", true);

						[ResourceKeyDefinition(typeof(Brush), "ElevatedButtonIconForegroundPointerOver")]
						public static ResourceValue<Brush> PointerOver => new("ElevatedButtonIconForegroundPointerOver", true);

						[ResourceKeyDefinition(typeof(Brush), "ElevatedButtonIconForegroundPressed")]
						public static ResourceValue<Brush> Pressed => new("ElevatedButtonIconForegroundPressed", true);

						[ResourceKeyDefinition(typeof(Brush), "ElevatedButtonIconForegroundFocused")]
						public static ResourceValue<Brush> Focused => new("ElevatedButtonIconForegroundFocused", true);

						[ResourceKeyDefinition(typeof(Brush), "ElevatedButtonIconForegroundPointerFocused")]
						public static ResourceValue<Brush> PointerFocused => new("ElevatedButtonIconForegroundPointerFocused", true);

						[ResourceKeyDefinition(typeof(Brush), "ElevatedButtonIconForegroundDisabled")]
						public static ResourceValue<Brush> Disabled => new("ElevatedButtonIconForegroundDisabled", true);
					}

					public static class Background
					{
						[ResourceKeyDefinition(typeof(Brush), "ElevatedButtonBackground")]
						public static ResourceValue<Brush> Default => new("ElevatedButtonBackground", true);

						[ResourceKeyDefinition(typeof(Brush), "ElevatedButtonBackgroundPointerOver")]
						public static ResourceValue<Brush> PointerOver => new("ElevatedButtonBackgroundPointerOver", true);

						[ResourceKeyDefinition(typeof(Brush), "ElevatedButtonBackgroundPressed")]
						public static ResourceValue<Brush> Pressed => new("ElevatedButtonBackgroundPressed", true);

						[ResourceKeyDefinition(typeof(Brush), "ElevatedButtonBackgroundFocused")]
						public static ResourceValue<Brush> Focused => new("ElevatedButtonBackgroundFocused", true);

						[ResourceKeyDefinition(typeof(Brush), "ElevatedButtonBackgroundPointerFocused")]
						public static ResourceValue<Brush> PointerFocused => new("ElevatedButtonBackgroundPointerFocused", true);

						[ResourceKeyDefinition(typeof(Brush), "ElevatedButtonBackgroundDisabled")]
						public static ResourceValue<Brush> Disabled => new("ElevatedButtonBackgroundDisabled", true);
					}

					public static class BorderBrush
					{
						[ResourceKeyDefinition(typeof(Brush), "ElevatedButtonBorderBrush")]
						public static ResourceValue<Brush> Default => new("ElevatedButtonBorderBrush", true);

						[ResourceKeyDefinition(typeof(Brush), "ElevatedButtonBorderBrushPointerOver")]
						public static ResourceValue<Brush> PointerOver => new("ElevatedButtonBorderBrushPointerOver", true);

						[ResourceKeyDefinition(typeof(Brush), "ElevatedButtonBorderBrushPressed")]
						public static ResourceValue<Brush> Pressed => new("ElevatedButtonBorderBrushPressed", true);

						[ResourceKeyDefinition(typeof(Brush), "ElevatedButtonBorderBrushFocused")]
						public static ResourceValue<Brush> Focused => new("ElevatedButtonBorderBrushFocused", true);

						[ResourceKeyDefinition(typeof(Brush), "ElevatedButtonBorderBrushPointerFocused")]
						public static ResourceValue<Brush> PointerFocused => new("ElevatedButtonBorderBrushPointerFocused", true);

						[ResourceKeyDefinition(typeof(Brush), "ElevatedButtonBorderBrushDisabled")]
						public static ResourceValue<Brush> Disabled => new("ElevatedButtonBorderBrushDisabled", true);
					}
				}

				public static class Filled
				{
					public static class Foreground
					{
						[ResourceKeyDefinition(typeof(Brush), "FilledButtonForeground")]
						public static ResourceValue<Brush> Default => new("FilledButtonForeground", true);

						[ResourceKeyDefinition(typeof(Brush), "FilledButtonForegroundPointerOver")]
						public static ResourceValue<Brush> PointerOver => new("FilledButtonForegroundPointerOver", true);

						[ResourceKeyDefinition(typeof(Brush), "FilledButtonForegroundPressed")]
						public static ResourceValue<Brush> Pressed => new("FilledButtonForegroundPressed", true);

						[ResourceKeyDefinition(typeof(Brush), "FilledButtonForegroundFocused")]
						public static ResourceValue<Brush> Focused => new("FilledButtonForegroundFocused", true);

						[ResourceKeyDefinition(typeof(Brush), "FilledButtonForegroundPointerFocused")]
						public static ResourceValue<Brush> PointerFocused => new("FilledButtonForegroundPointerFocused", true);

						[ResourceKeyDefinition(typeof(Brush), "FilledButtonForegroundDisabled")]
						public static ResourceValue<Brush> Disabled => new("FilledButtonForegroundDisabled", true);
					}

					public static class IconForeground
					{
						[ResourceKeyDefinition(typeof(Brush), "FilledButtonIconForeground")]
						public static ResourceValue<Brush> Default => new("FilledButtonIconForeground", true);

						[ResourceKeyDefinition(typeof(Brush), "FilledButtonIconForegroundPointerOver")]
						public static ResourceValue<Brush> PointerOver => new("FilledButtonIconForegroundPointerOver", true);

						[ResourceKeyDefinition(typeof(Brush), "FilledButtonIconForegroundPressed")]
						public static ResourceValue<Brush> Pressed => new("FilledButtonIconForegroundPressed", true);

						[ResourceKeyDefinition(typeof(Brush), "FilledButtonIconForegroundFocused")]
						public static ResourceValue<Brush> Focused => new("FilledButtonIconForegroundFocused", true);

						[ResourceKeyDefinition(typeof(Brush), "FilledButtonIconForegroundPointerFocused")]
						public static ResourceValue<Brush> PointerFocused => new("FilledButtonIconForegroundPointerFocused", true);

						[ResourceKeyDefinition(typeof(Brush), "FilledButtonIconForegroundDisabled")]
						public static ResourceValue<Brush> Disabled => new("FilledButtonIconForegroundDisabled", true);
					}

					public static class Background
					{
						[ResourceKeyDefinition(typeof(Brush), "FilledButtonBackground")]
						public static ResourceValue<Brush> Default => new("FilledButtonBackground", true);

						[ResourceKeyDefinition(typeof(Brush), "FilledButtonBackgroundPointerOver")]
						public static ResourceValue<Brush> PointerOver => new("FilledButtonBackgroundPointerOver", true);

						[ResourceKeyDefinition(typeof(Brush), "FilledButtonBackgroundPressed")]
						public static ResourceValue<Brush> Pressed => new("FilledButtonBackgroundPressed", true);

						[ResourceKeyDefinition(typeof(Brush), "FilledButtonBackgroundFocused")]
						public static ResourceValue<Brush> Focused => new("FilledButtonBackgroundFocused", true);

						[ResourceKeyDefinition(typeof(Brush), "FilledButtonBackgroundPointerFocused")]
						public static ResourceValue<Brush> PointerFocused => new("FilledButtonBackgroundPointerFocused", true);

						[ResourceKeyDefinition(typeof(Brush), "FilledButtonBackgroundDisabled")]
						public static ResourceValue<Brush> Disabled => new("FilledButtonBackgroundDisabled", true);
					}

					public static class BorderBrush
					{
						[ResourceKeyDefinition(typeof(Brush), "FilledButtonBorderBrush")]
						public static ResourceValue<Brush> Default => new("FilledButtonBorderBrush", true);

						[ResourceKeyDefinition(typeof(Brush), "FilledButtonBorderBrushPointerOver")]
						public static ResourceValue<Brush> PointerOver => new("FilledButtonBorderBrushPointerOver", true);

						[ResourceKeyDefinition(typeof(Brush), "FilledButtonBorderBrushPressed")]
						public static ResourceValue<Brush> Pressed => new("FilledButtonBorderBrushPressed", true);

						[ResourceKeyDefinition(typeof(Brush), "FilledButtonBorderBrushFocused")]
						public static ResourceValue<Brush> Focused => new("FilledButtonBorderBrushFocused", true);

						[ResourceKeyDefinition(typeof(Brush), "FilledButtonBorderBrushPointerFocused")]
						public static ResourceValue<Brush> PointerFocused => new("FilledButtonBorderBrushPointerFocused", true);

						[ResourceKeyDefinition(typeof(Brush), "FilledButtonBorderBrushDisabled")]
						public static ResourceValue<Brush> Disabled => new("FilledButtonBorderBrushDisabled", true);
					}
				}

				public static class FilledTonal
				{
					public static class Foreground
					{
						[ResourceKeyDefinition(typeof(Brush), "FilledTonalButtonForeground")]
						public static ResourceValue<Brush> Default => new("FilledTonalButtonForeground", true);

						[ResourceKeyDefinition(typeof(Brush), "FilledTonalButtonForegroundPointerOver")]
						public static ResourceValue<Brush> PointerOver => new("FilledTonalButtonForegroundPointerOver", true);

						[ResourceKeyDefinition(typeof(Brush), "FilledTonalButtonForegroundPressed")]
						public static ResourceValue<Brush> Pressed => new("FilledTonalButtonForegroundPressed", true);

						[ResourceKeyDefinition(typeof(Brush), "FilledTonalButtonForegroundFocused")]
						public static ResourceValue<Brush> Focused => new("FilledTonalButtonForegroundFocused", true);

						[ResourceKeyDefinition(typeof(Brush), "FilledTonalButtonForegroundPointerFocused")]
						public static ResourceValue<Brush> PointerFocused => new("FilledTonalButtonForegroundPointerFocused", true);

						[ResourceKeyDefinition(typeof(Brush), "FilledTonalButtonForegroundDisabled")]
						public static ResourceValue<Brush> Disabled => new("FilledTonalButtonForegroundDisabled", true);
					}

					public static class IconForeground
					{
						[ResourceKeyDefinition(typeof(Brush), "FilledTonalButtonIconForeground")]
						public static ResourceValue<Brush> Default => new("FilledTonalButtonIconForeground", true);

						[ResourceKeyDefinition(typeof(Brush), "FilledTonalButtonIconForegroundPointerOver")]
						public static ResourceValue<Brush> PointerOver => new("FilledTonalButtonIconForegroundPointerOver", true);

						[ResourceKeyDefinition(typeof(Brush), "FilledTonalButtonIconForegroundPressed")]
						public static ResourceValue<Brush> Pressed => new("FilledTonalButtonIconForegroundPressed", true);

						[ResourceKeyDefinition(typeof(Brush), "FilledTonalButtonIconForegroundFocused")]
						public static ResourceValue<Brush> Focused => new("FilledTonalButtonIconForegroundFocused", true);

						[ResourceKeyDefinition(typeof(Brush), "FilledTonalButtonIconForegroundPointerFocused")]
						public static ResourceValue<Brush> PointerFocused => new("FilledTonalButtonIconForegroundPointerFocused", true);

						[ResourceKeyDefinition(typeof(Brush), "FilledTonalButtonIconForegroundDisabled")]
						public static ResourceValue<Brush> Disabled => new("FilledTonalButtonIconForegroundDisabled", true);
					}

					public static class Background
					{
						[ResourceKeyDefinition(typeof(Brush), "FilledTonalButtonBackground")]
						public static ResourceValue<Brush> Default => new("FilledTonalButtonBackground", true);

						[ResourceKeyDefinition(typeof(Brush), "FilledTonalButtonBackgroundPointerOver")]
						public static ResourceValue<Brush> PointerOver => new("FilledTonalButtonBackgroundPointerOver", true);

						[ResourceKeyDefinition(typeof(Brush), "FilledTonalButtonBackgroundPressed")]
						public static ResourceValue<Brush> Pressed => new("FilledTonalButtonBackgroundPressed", true);

						[ResourceKeyDefinition(typeof(Brush), "FilledTonalButtonBackgroundFocused")]
						public static ResourceValue<Brush> Focused => new("FilledTonalButtonBackgroundFocused", true);

						[ResourceKeyDefinition(typeof(Brush), "FilledTonalButtonBackgroundPointerFocused")]
						public static ResourceValue<Brush> PointerFocused => new("FilledTonalButtonBackgroundPointerFocused", true);

						[ResourceKeyDefinition(typeof(Brush), "FilledTonalButtonBackgroundDisabled")]
						public static ResourceValue<Brush> Disabled => new("FilledTonalButtonBackgroundDisabled", true);
					}

					public static class BorderBrush
					{
						[ResourceKeyDefinition(typeof(Brush), "FilledTonalButtonBorderBrush")]
						public static ResourceValue<Brush> Default => new("FilledTonalButtonBorderBrush", true);

						[ResourceKeyDefinition(typeof(Brush), "FilledTonalButtonBorderBrushPointerOver")]
						public static ResourceValue<Brush> PointerOver => new("FilledTonalButtonBorderBrushPointerOver", true);

						[ResourceKeyDefinition(typeof(Brush), "FilledTonalButtonBorderBrushPressed")]
						public static ResourceValue<Brush> Pressed => new("FilledTonalButtonBorderBrushPressed", true);

						[ResourceKeyDefinition(typeof(Brush), "FilledTonalButtonBorderBrushFocused")]
						public static ResourceValue<Brush> Focused => new("FilledTonalButtonBorderBrushFocused", true);

						[ResourceKeyDefinition(typeof(Brush), "FilledTonalButtonBorderBrushPointerFocused")]
						public static ResourceValue<Brush> PointerFocused => new("FilledTonalButtonBorderBrushPointerFocused", true);

						[ResourceKeyDefinition(typeof(Brush), "FilledTonalButtonBorderBrushDisabled")]
						public static ResourceValue<Brush> Disabled => new("FilledTonalButtonBorderBrushDisabled", true);
					}
				}

				public static class Outlined
				{
					public static class Foreground
					{
						[ResourceKeyDefinition(typeof(Brush), "OutlinedButtonForeground")]
						public static ResourceValue<Brush> Default => new("OutlinedButtonForeground", true);

						[ResourceKeyDefinition(typeof(Brush), "OutlinedButtonForegroundPointerOver")]
						public static ResourceValue<Brush> PointerOver => new("OutlinedButtonForegroundPointerOver", true);

						[ResourceKeyDefinition(typeof(Brush), "OutlinedButtonForegroundPressed")]
						public static ResourceValue<Brush> Pressed => new("OutlinedButtonForegroundPressed", true);

						[ResourceKeyDefinition(typeof(Brush), "OutlinedButtonForegroundFocused")]
						public static ResourceValue<Brush> Focused => new("OutlinedButtonForegroundFocused", true);

						[ResourceKeyDefinition(typeof(Brush), "OutlinedButtonForegroundPointerFocused")]
						public static ResourceValue<Brush> PointerFocused => new("OutlinedButtonForegroundPointerFocused", true);

						[ResourceKeyDefinition(typeof(Brush), "OutlinedButtonForegroundDisabled")]
						public static ResourceValue<Brush> Disabled => new("OutlinedButtonForegroundDisabled", true);
					}

					public static class IconForeground
					{
						[ResourceKeyDefinition(typeof(Brush), "OutlinedButtonIconForeground")]
						public static ResourceValue<Brush> Default => new("OutlinedButtonIconForeground", true);

						[ResourceKeyDefinition(typeof(Brush), "OutlinedButtonIconForegroundPointerOver")]
						public static ResourceValue<Brush> PointerOver => new("OutlinedButtonIconForegroundPointerOver", true);

						[ResourceKeyDefinition(typeof(Brush), "OutlinedButtonIconForegroundPressed")]
						public static ResourceValue<Brush> Pressed => new("OutlinedButtonIconForegroundPressed", true);

						[ResourceKeyDefinition(typeof(Brush), "OutlinedButtonIconForegroundFocused")]
						public static ResourceValue<Brush> Focused => new("OutlinedButtonIconForegroundFocused", true);

						[ResourceKeyDefinition(typeof(Brush), "OutlinedButtonIconForegroundPointerFocused")]
						public static ResourceValue<Brush> PointerFocused => new("OutlinedButtonIconForegroundPointerFocused", true);

						[ResourceKeyDefinition(typeof(Brush), "OutlinedButtonIconForegroundDisabled")]
						public static ResourceValue<Brush> Disabled => new("OutlinedButtonIconForegroundDisabled", true);
					}

					public static class Background
					{
						[ResourceKeyDefinition(typeof(Brush), "OutlinedButtonBackground")]
						public static ResourceValue<Brush> Default => new("OutlinedButtonBackground", true);

						[ResourceKeyDefinition(typeof(Brush), "OutlinedButtonBackgroundPointerOver")]
						public static ResourceValue<Brush> PointerOver => new("OutlinedButtonBackgroundPointerOver", true);

						[ResourceKeyDefinition(typeof(Brush), "OutlinedButtonBackgroundPressed")]
						public static ResourceValue<Brush> Pressed => new("OutlinedButtonBackgroundPressed", true);

						[ResourceKeyDefinition(typeof(Brush), "OutlinedButtonBackgroundFocused")]
						public static ResourceValue<Brush> Focused => new("OutlinedButtonBackgroundFocused", true);

						[ResourceKeyDefinition(typeof(Brush), "OutlinedButtonBackgroundPointerFocused")]
						public static ResourceValue<Brush> PointerFocused => new("OutlinedButtonBackgroundPointerFocused", true);

						[ResourceKeyDefinition(typeof(Brush), "OutlinedButtonBackgroundDisabled")]
						public static ResourceValue<Brush> Disabled => new("OutlinedButtonBackgroundDisabled", true);
					}

					public static class BorderBrush
					{
						[ResourceKeyDefinition(typeof(Brush), "OutlinedButtonBorderBrush")]
						public static ResourceValue<Brush> Default => new("OutlinedButtonBorderBrush", true);

						[ResourceKeyDefinition(typeof(Brush), "OutlinedButtonBorderBrushPointerOver")]
						public static ResourceValue<Brush> PointerOver => new("OutlinedButtonBorderBrushPointerOver", true);

						[ResourceKeyDefinition(typeof(Brush), "OutlinedButtonBorderBrushPressed")]
						public static ResourceValue<Brush> Pressed => new("OutlinedButtonBorderBrushPressed", true);

						[ResourceKeyDefinition(typeof(Brush), "OutlinedButtonBorderBrushFocused")]
						public static ResourceValue<Brush> Focused => new("OutlinedButtonBorderBrushFocused", true);

						[ResourceKeyDefinition(typeof(Brush), "OutlinedButtonBorderBrushPointerFocused")]
						public static ResourceValue<Brush> PointerFocused => new("OutlinedButtonBorderBrushPointerFocused", true);

						[ResourceKeyDefinition(typeof(Brush), "OutlinedButtonBorderBrushDisabled")]
						public static ResourceValue<Brush> Disabled => new("OutlinedButtonBorderBrushDisabled", true);
					}
				}

				public static class Text
				{
					public static class Foreground
					{
						[ResourceKeyDefinition(typeof(Brush), "TextButtonForeground")]
						public static ResourceValue<Brush> Default => new("TextButtonForeground", true);

						[ResourceKeyDefinition(typeof(Brush), "TextButtonForegroundPointerOver")]
						public static ResourceValue<Brush> PointerOver => new("TextButtonForegroundPointerOver", true);

						[ResourceKeyDefinition(typeof(Brush), "TextButtonForegroundPressed")]
						public static ResourceValue<Brush> Pressed => new("TextButtonForegroundPressed", true);

						[ResourceKeyDefinition(typeof(Brush), "TextButtonForegroundFocused")]
						public static ResourceValue<Brush> Focused => new("TextButtonForegroundFocused", true);

						[ResourceKeyDefinition(typeof(Brush), "TextButtonForegroundPointerFocused")]
						public static ResourceValue<Brush> PointerFocused => new("TextButtonForegroundPointerFocused", true);

						[ResourceKeyDefinition(typeof(Brush), "TextButtonForegroundDisabled")]
						public static ResourceValue<Brush> Disabled => new("TextButtonForegroundDisabled", true);
					}

					public static class IconForeground
					{
						[ResourceKeyDefinition(typeof(Brush), "TextButtonIconForeground")]
						public static ResourceValue<Brush> Default => new("TextButtonIconForeground", true);

						[ResourceKeyDefinition(typeof(Brush), "TextButtonIconForegroundPointerOver")]
						public static ResourceValue<Brush> PointerOver => new("TextButtonIconForegroundPointerOver", true);

						[ResourceKeyDefinition(typeof(Brush), "TextButtonIconForegroundPressed")]
						public static ResourceValue<Brush> Pressed => new("TextButtonIconForegroundPressed", true);

						[ResourceKeyDefinition(typeof(Brush), "TextButtonIconForegroundFocused")]
						public static ResourceValue<Brush> Focused => new("TextButtonIconForegroundFocused", true);

						[ResourceKeyDefinition(typeof(Brush), "TextButtonIconForegroundPointerFocused")]
						public static ResourceValue<Brush> PointerFocused => new("TextButtonIconForegroundPointerFocused", true);

						[ResourceKeyDefinition(typeof(Brush), "TextButtonIconForegroundDisabled")]
						public static ResourceValue<Brush> Disabled => new("TextButtonIconForegroundDisabled", true);
					}

					public static class Background
					{
						[ResourceKeyDefinition(typeof(Brush), "TextButtonBackground")]
						public static ResourceValue<Brush> Default => new("TextButtonBackground", true);

						[ResourceKeyDefinition(typeof(Brush), "TextButtonBackgroundPointerOver")]
						public static ResourceValue<Brush> PointerOver => new("TextButtonBackgroundPointerOver", true);

						[ResourceKeyDefinition(typeof(Brush), "TextButtonBackgroundPressed")]
						public static ResourceValue<Brush> Pressed => new("TextButtonBackgroundPressed", true);

						[ResourceKeyDefinition(typeof(Brush), "TextButtonBackgroundFocused")]
						public static ResourceValue<Brush> Focused => new("TextButtonBackgroundFocused", true);

						[ResourceKeyDefinition(typeof(Brush), "TextButtonBackgroundPointerFocused")]
						public static ResourceValue<Brush> PointerFocused => new("TextButtonBackgroundPointerFocused", true);

						[ResourceKeyDefinition(typeof(Brush), "TextButtonBackgroundDisabled")]
						public static ResourceValue<Brush> Disabled => new("TextButtonBackgroundDisabled", true);
					}

					public static class BorderBrush
					{
						[ResourceKeyDefinition(typeof(Brush), "TextButtonBorderBrush")]
						public static ResourceValue<Brush> Default => new("TextButtonBorderBrush", true);

						[ResourceKeyDefinition(typeof(Brush), "TextButtonBorderBrushPointerOver")]
						public static ResourceValue<Brush> PointerOver => new("TextButtonBorderBrushPointerOver", true);

						[ResourceKeyDefinition(typeof(Brush), "TextButtonBorderBrushPressed")]
						public static ResourceValue<Brush> Pressed => new("TextButtonBorderBrushPressed", true);

						[ResourceKeyDefinition(typeof(Brush), "TextButtonBorderBrushFocused")]
						public static ResourceValue<Brush> Focused => new("TextButtonBorderBrushFocused", true);

						[ResourceKeyDefinition(typeof(Brush), "TextButtonBorderBrushPointerFocused")]
						public static ResourceValue<Brush> PointerFocused => new("TextButtonBorderBrushPointerFocused", true);

						[ResourceKeyDefinition(typeof(Brush), "TextButtonBorderBrushDisabled")]
						public static ResourceValue<Brush> Disabled => new("TextButtonBorderBrushDisabled", true);
					}
				}

				public static class Icon
				{
					public static class Foreground
					{
						[ResourceKeyDefinition(typeof(Brush), "IconButtonForeground")]
						public static ResourceValue<Brush> Default => new("IconButtonForeground", true);

						[ResourceKeyDefinition(typeof(Brush), "IconButtonForegroundDisabled")]
						public static ResourceValue<Brush> Disabled => new("IconButtonForegroundDisabled", true);
					}

					public static class EllipseFill
					{
						[ResourceKeyDefinition(typeof(Brush), "IconButtonEllipseFillPointerOver")]
						public static ResourceValue<Brush> PointerOver => new("IconButtonEllipseFillPointerOver", true);

						[ResourceKeyDefinition(typeof(Brush), "IconButtonEllipseFillPressed")]
						public static ResourceValue<Brush> Pressed => new("IconButtonEllipseFillPressed", true);

						[ResourceKeyDefinition(typeof(Brush), "IconButtonEllipseFillFocused")]
						public static ResourceValue<Brush> Focused => new("IconButtonEllipseFillFocused", true);
					}
				}
			}
		}
	}
}
