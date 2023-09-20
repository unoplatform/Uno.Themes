using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Media;
using Uno.Extensions.Markup.Internals;

namespace Uno.Themes.Markup
{
	public static partial class Theme
	{
		public static class HyperlinkButton
		{
			public static class Resources
			{
				public static class Foreground
				{
					[ResourceKeyDefinition(typeof(Brush), "HyperlinkButtonForeground")]
					public static ResourceValue<Brush> Default => new("HyperlinkButtonForeground", true);

					[ResourceKeyDefinition(typeof(Brush), "HyperlinkButtonForegroundPointerOver")]
					public static ResourceValue<Brush> PointerOver => new("HyperlinkButtonForegroundPointerOver", true);

					[ResourceKeyDefinition(typeof(Brush), "HyperlinkButtonForegroundPressed")]
					public static ResourceValue<Brush> Pressed => new("HyperlinkButtonForegroundPressed", true);

					[ResourceKeyDefinition(typeof(Brush), "HyperlinkButtonForegroundDisabled")]
					public static ResourceValue<Brush> Disabled => new("HyperlinkButtonForegroundDisabled", true);
				}

				public static class Background
				{
					[ResourceKeyDefinition(typeof(Brush), "HyperlinkButtonBackground")]
					public static ResourceValue<Brush> Default => new("HyperlinkButtonBackground", true);

					[ResourceKeyDefinition(typeof(Brush), "HyperlinkButtonBackgroundPointerOver")]
					public static ResourceValue<Brush> PointerOver => new("HyperlinkButtonBackgroundPointerOver", true);

					[ResourceKeyDefinition(typeof(Brush), "HyperlinkButtonBackgroundPressed")]
					public static ResourceValue<Brush> Pressed => new("HyperlinkButtonBackgroundPressed", true);

					[ResourceKeyDefinition(typeof(Brush), "HyperlinkButtonBackgroundDisabled")]
					public static ResourceValue<Brush> Disabled => new("HyperlinkButtonBackgroundDisabled", true);
				}

				public static class BorderBrush
				{
					[ResourceKeyDefinition(typeof(Brush), "HyperlinkButtonBorderBrush")]
					public static ResourceValue<Brush> Default => new("HyperlinkButtonBorderBrush", true);

					[ResourceKeyDefinition(typeof(Brush), "HyperlinkButtonBorderBrushPointerOver")]
					public static ResourceValue<Brush> PointerOver => new("HyperlinkButtonBorderBrushPointerOver", true);

					[ResourceKeyDefinition(typeof(Brush), "HyperlinkButtonBorderBrushPressed")]
					public static ResourceValue<Brush> Pressed => new("HyperlinkButtonBorderBrushPressed", true);

					[ResourceKeyDefinition(typeof(Brush), "HyperlinkButtonBorderBrushDisabled")]
					public static ResourceValue<Brush> Disabled => new("HyperlinkButtonBorderBrushDisabled", true);
				}

				public static class Typography
				{
					[ResourceKeyDefinition(typeof(FontFamily), "HyperlinkButtonFontFamily")]
					public static ResourceValue<FontFamily> FontFamily => new("HyperlinkButtonFontFamily", true);

					[ResourceKeyDefinition(typeof(double), "HyperlinkButtonFontSize")]
					public static ResourceValue<double> FontSize => new("HyperlinkButtonFontSize", true);
				}

				public static class Opacity
				{
					[ResourceKeyDefinition(typeof(double), "HyperlinkButtonBackgroundOpacity")]
					public static ResourceValue<double> Default => new("HyperlinkButtonBackgroundOpacity", true);

					[ResourceKeyDefinition(typeof(double), "HyperlinkButtonBackgroundOpacityPointerOver")]
					public static ResourceValue<double> PointerOver => new("HyperlinkButtonBackgroundOpacityPointerOver", true);

					[ResourceKeyDefinition(typeof(double), "HyperlinkButtonBackgroundOpacityPressed")]
					public static ResourceValue<double> Pressed => new("HyperlinkButtonBackgroundOpacityPressed", true);

					[ResourceKeyDefinition(typeof(double), "HyperlinkButtonBackgroundOpacityDisabled")]
					public static ResourceValue<double> Disabled => new("HyperlinkButtonBackgroundOpacityDisabled", true);
				}
			}

			public static class Styles
			{
				[ResourceKeyDefinition(typeof(Style), "HyperlinkButtonStyle", TargetType = typeof(HyperlinkButton))]
				public static ResourceValue<Style> Default => new("HyperlinkButtonStyle");

				[ResourceKeyDefinition(typeof(Style), "SecondaryHyperlinkButtonStyle", TargetType = typeof(HyperlinkButton))]
				public static ResourceValue<Style> Secondary => new("SecondaryHyperlinkButtonStyle");
			}
		}
	}
}
