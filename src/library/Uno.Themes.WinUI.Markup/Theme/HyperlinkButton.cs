using System;
using Windows.UI;
using Microsoft.UI.Text;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Media;
using Uno.Extensions.Markup;
using Uno.Extensions.Markup.Internals;

using ResourceKeyDefinitionAttribute = Uno.Themes.WinUI.Markup.ResourceKeyDefinitionAttribute;

namespace Uno.Themes.Markup
{
	public static partial class Theme
	{
		public static partial class HyperlinkButton
		{
			public static partial class Resources
			{
				public static readonly BackgroundVSG Background = new();
				public partial class BackgroundVSG
				{
					[ResourceKeyDefinition(typeof(Brush), "HyperlinkButtonBackground")]
					public ThemeResourceKey<Brush> Normal = new("HyperlinkButtonBackground");

					[ResourceKeyDefinition(typeof(Brush), "HyperlinkButtonBackgroundPointerOver")]
					public ThemeResourceKey<Brush> PointerOver = new("HyperlinkButtonBackgroundPointerOver");

					[ResourceKeyDefinition(typeof(Brush), "HyperlinkButtonBackgroundPressed")]
					public ThemeResourceKey<Brush> Pressed = new("HyperlinkButtonBackgroundPressed");

					[ResourceKeyDefinition(typeof(Brush), "HyperlinkButtonBackgroundDisabled")]
					public ThemeResourceKey<Brush> Disabled = new("HyperlinkButtonBackgroundDisabled");

					public static implicit operator ThemeResourceKey<Brush>(BackgroundVSG self) => self.Normal;
				}

				public static readonly BackgroundOpacityVSG BackgroundOpacity = new();
				public partial class BackgroundOpacityVSG
				{
					[ResourceKeyDefinition(typeof(double), "HyperlinkButtonBackgroundOpacity")]
					public ThemeResourceKey<double> Normal = new("HyperlinkButtonBackgroundOpacity");

					[ResourceKeyDefinition(typeof(double), "HyperlinkButtonBackgroundOpacityPointerOver")]
					public ThemeResourceKey<double> PointerOver = new("HyperlinkButtonBackgroundOpacityPointerOver");

					[ResourceKeyDefinition(typeof(double), "HyperlinkButtonBackgroundOpacityPressed")]
					public ThemeResourceKey<double> Pressed = new("HyperlinkButtonBackgroundOpacityPressed");

					[ResourceKeyDefinition(typeof(double), "HyperlinkButtonBackgroundOpacityDisabled")]
					public ThemeResourceKey<double> Disabled = new("HyperlinkButtonBackgroundOpacityDisabled");

					public static implicit operator ThemeResourceKey<double>(BackgroundOpacityVSG self) => self.Normal;
				}

				public static readonly BorderBrushVSG BorderBrush = new();
				public partial class BorderBrushVSG
				{
					[ResourceKeyDefinition(typeof(Brush), "HyperlinkButtonBorderBrush")]
					public ThemeResourceKey<Brush> Normal = new("HyperlinkButtonBorderBrush");

					[ResourceKeyDefinition(typeof(Brush), "HyperlinkButtonBorderBrushPointerOver")]
					public ThemeResourceKey<Brush> PointerOver = new("HyperlinkButtonBorderBrushPointerOver");

					[ResourceKeyDefinition(typeof(Brush), "HyperlinkButtonBorderBrushPressed")]
					public ThemeResourceKey<Brush> Pressed = new("HyperlinkButtonBorderBrushPressed");

					[ResourceKeyDefinition(typeof(Brush), "HyperlinkButtonBorderBrushDisabled")]
					public ThemeResourceKey<Brush> Disabled = new("HyperlinkButtonBorderBrushDisabled");

					public static implicit operator ThemeResourceKey<Brush>(BorderBrushVSG self) => self.Normal;
				}

				public static partial class Typography
				{
					[ResourceKeyDefinition(typeof(FontFamily), "HyperlinkButtonFontFamily")]
					public static ThemeResourceKey<FontFamily> FontFamily => new("HyperlinkButtonFontFamily");

					[ResourceKeyDefinition(typeof(double), "HyperlinkButtonFontSize")]
					public static ThemeResourceKey<double> FontSize => new("HyperlinkButtonFontSize");
				}

				[ResourceKeyDefinition(typeof(Brush), "HyperlinkButtonForeground")]
				public static ThemeResourceKey<Brush> Foreground => new("HyperlinkButtonForeground");

				[ResourceKeyDefinition(typeof(Thickness), "HyperlinkButtonPadding")]
				public static ThemeResourceKey<Thickness> Padding => new("HyperlinkButtonPadding");
			}

			public static partial class Styles
			{
				[ResourceKeyDefinition(typeof(Style), "MaterialHyperlinkButtonStyle", TargetType = typeof(global::Microsoft.UI.Xaml.Controls.HyperlinkButton))]
				public static StaticResourceKey<Style> Default => new("MaterialHyperlinkButtonStyle");

				[ResourceKeyDefinition(typeof(Style), "MaterialSecondaryHyperlinkButtonStyle", TargetType = typeof(global::Microsoft.UI.Xaml.Controls.HyperlinkButton))]
				public static StaticResourceKey<Style> Secondary => new("MaterialSecondaryHyperlinkButtonStyle");
			}
		}
	}
}
