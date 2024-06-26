﻿using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Media;
using Uno.Extensions.Markup;
using Uno.Extensions.Markup.Internals;

namespace Uno.Themes.Markup;

public static partial class Theme
{
	public static partial class HyperlinkButton
	{
		public static partial class Resources
		{
			public static partial class Default
			{

				public static partial class Foreground
				{
					[ResourceKeyDefinition(typeof(Brush), "HyperlinkButtonForeground")]
					public static ThemeResourceKey<Brush> Default => new("HyperlinkButtonForeground");

					[ResourceKeyDefinition(typeof(Brush), "HyperlinkButtonForegroundPointerOver")]
					public static ThemeResourceKey<Brush> PointerOver => new("HyperlinkButtonForegroundPointerOver");

					[ResourceKeyDefinition(typeof(Brush), "HyperlinkButtonForegroundPressed")]
					public static ThemeResourceKey<Brush> Pressed => new("HyperlinkButtonForegroundPressed");

					[ResourceKeyDefinition(typeof(Brush), "HyperlinkButtonForegroundDisabled")]
					public static ThemeResourceKey<Brush> Disabled => new("HyperlinkButtonForegroundDisabled");
				}

				public static partial class Background
				{
					[ResourceKeyDefinition(typeof(Brush), "HyperlinkButtonBackground")]
					public static ThemeResourceKey<Brush> Default => new("HyperlinkButtonBackground");

					[ResourceKeyDefinition(typeof(Brush), "HyperlinkButtonBackgroundPointerOver")]
					public static ThemeResourceKey<Brush> PointerOver => new("HyperlinkButtonBackgroundPointerOver");

					[ResourceKeyDefinition(typeof(Brush), "HyperlinkButtonBackgroundPressed")]
					public static ThemeResourceKey<Brush> Pressed => new("HyperlinkButtonBackgroundPressed");

					[ResourceKeyDefinition(typeof(Brush), "HyperlinkButtonBackgroundDisabled")]
					public static ThemeResourceKey<Brush> Disabled => new("HyperlinkButtonBackgroundDisabled");
				}

				public static partial class BackgroundOpacity
				{
					[ResourceKeyDefinition(typeof(double), "HyperlinkButtonBackgroundOpacity")]
					public static ThemeResourceKey<double> Default => new("HyperlinkButtonBackgroundOpacity");

					[ResourceKeyDefinition(typeof(double), "HyperlinkButtonBackgroundOpacityPointerOver")]
					public static ThemeResourceKey<double> PointerOver => new("HyperlinkButtonBackgroundOpacityPointerOver");

					[ResourceKeyDefinition(typeof(double), "HyperlinkButtonBackgroundOpacityPressed")]
					public static ThemeResourceKey<double> Pressed => new("HyperlinkButtonBackgroundOpacityPressed");

					[ResourceKeyDefinition(typeof(double), "HyperlinkButtonBackgroundOpacityDisabled")]
					public static ThemeResourceKey<double> Disabled => new("HyperlinkButtonBackgroundOpacityDisabled");
				}

				public static partial class BorderBrush
				{
					[ResourceKeyDefinition(typeof(Brush), "HyperlinkButtonBorderBrush")]
					public static ThemeResourceKey<Brush> Default => new("HyperlinkButtonBorderBrush");

					[ResourceKeyDefinition(typeof(Brush), "HyperlinkButtonBorderBrushPointerOver")]
					public static ThemeResourceKey<Brush> PointerOver => new("HyperlinkButtonBorderBrushPointerOver");

					[ResourceKeyDefinition(typeof(Brush), "HyperlinkButtonBorderBrushPressed")]
					public static ThemeResourceKey<Brush> Pressed => new("HyperlinkButtonBorderBrushPressed");

					[ResourceKeyDefinition(typeof(Brush), "HyperlinkButtonBorderBrushDisabled")]
					public static ThemeResourceKey<Brush> Disabled => new("HyperlinkButtonBorderBrushDisabled");
				}

				public static partial class Typography
				{
					[ResourceKeyDefinition(typeof(FontFamily), "HyperlinkButtonFontFamily")]
					public static ThemeResourceKey<FontFamily> FontFamily => new("HyperlinkButtonFontFamily");

					[ResourceKeyDefinition(typeof(double), "HyperlinkButtonFontSize")]
					public static ThemeResourceKey<double> FontSize => new("HyperlinkButtonFontSize");
				}

				[ResourceKeyDefinition(typeof(Thickness), "HyperlinkButtonPadding")]
				public static ThemeResourceKey<Thickness> Padding => new("HyperlinkButtonPadding");
			}
		}

		public static partial class Styles
		{
			[ResourceKeyDefinition(typeof(Style), "HyperlinkButtonStyle", TargetType = typeof(global::Microsoft.UI.Xaml.Controls.HyperlinkButton))]
			public static StaticResourceKey<Style> Default => new("HyperlinkButtonStyle");

			[ResourceKeyDefinition(typeof(Style), "SecondaryHyperlinkButtonStyle", TargetType = typeof(global::Microsoft.UI.Xaml.Controls.HyperlinkButton))]
			public static StaticResourceKey<Style> Secondary => new("SecondaryHyperlinkButtonStyle");
		}
	}
}
