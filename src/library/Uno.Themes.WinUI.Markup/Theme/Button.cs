using System;
using Windows.UI;
using Microsoft.UI.Text;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Media;
using Uno.Extensions.Markup;
using Uno.Extensions.Markup.Internals;

namespace Uno.Themes.Markup
{
	public static partial class Theme
	{
		public static partial class Button
		{
			public static partial class Resources
			{
				public static partial class Elevated
				{
					public static readonly BackgroundVSG Background = new();
					public partial class BackgroundVSG
					{
						[ResourceKeyDefinition(typeof(Brush), "ElevatedButtonBackground")]
						public ThemeResourceKey<Brush> Default = new("ElevatedButtonBackground");

						[ResourceKeyDefinition(typeof(Brush), "ElevatedButtonBackgroundPointerOver")]
						public ThemeResourceKey<Brush> PointerOver = new("ElevatedButtonBackgroundPointerOver");

						[ResourceKeyDefinition(typeof(Brush), "ElevatedButtonBackgroundPressed")]
						public ThemeResourceKey<Brush> Pressed = new("ElevatedButtonBackgroundPressed");

						[ResourceKeyDefinition(typeof(Brush), "ElevatedButtonBackgroundDisabled")]
						public ThemeResourceKey<Brush> Disabled = new("ElevatedButtonBackgroundDisabled");

						[ResourceKeyDefinition(typeof(Brush), "ElevatedButtonBackgroundFocused")]
						public ThemeResourceKey<Brush> Focused = new("ElevatedButtonBackgroundFocused");

						[ResourceKeyDefinition(typeof(Brush), "ElevatedButtonBackgroundPointerFocused")]
						public ThemeResourceKey<Brush> PointerFocused = new("ElevatedButtonBackgroundPointerFocused");

						public static implicit operator ThemeResourceKey<Brush>(BackgroundVSG self) => self.Default;
					}

					public static readonly BorderBrushVSG BorderBrush = new();
					public partial class BorderBrushVSG
					{
						[ResourceKeyDefinition(typeof(Brush), "ElevatedButtonBorderBrush")]
						public ThemeResourceKey<Brush> Default = new("ElevatedButtonBorderBrush");

						[ResourceKeyDefinition(typeof(Brush), "ElevatedButtonBorderBrushPointerOver")]
						public ThemeResourceKey<Brush> PointerOver = new("ElevatedButtonBorderBrushPointerOver");

						[ResourceKeyDefinition(typeof(Brush), "ElevatedButtonBorderBrushPressed")]
						public ThemeResourceKey<Brush> Pressed = new("ElevatedButtonBorderBrushPressed");

						[ResourceKeyDefinition(typeof(Brush), "ElevatedButtonBorderBrushDisabled")]
						public ThemeResourceKey<Brush> Disabled = new("ElevatedButtonBorderBrushDisabled");

						[ResourceKeyDefinition(typeof(Brush), "ElevatedButtonBorderBrushFocused")]
						public ThemeResourceKey<Brush> Focused = new("ElevatedButtonBorderBrushFocused");

						[ResourceKeyDefinition(typeof(Brush), "ElevatedButtonBorderBrushPointerFocused")]
						public ThemeResourceKey<Brush> PointerFocused = new("ElevatedButtonBorderBrushPointerFocused");

						public static implicit operator ThemeResourceKey<Brush>(BorderBrushVSG self) => self.Default;
					}

					public static readonly ElevationVSG Elevation = new();
					public partial class ElevationVSG
					{
						[ResourceKeyDefinition(typeof(double), "ElevatedButtonElevation")]
						public ThemeResourceKey<double> Default = new("ElevatedButtonElevation");

						[ResourceKeyDefinition(typeof(double), "ElevatedButtonElevationDisabled")]
						public ThemeResourceKey<double> Disabled = new("ElevatedButtonElevationDisabled");

						public static implicit operator ThemeResourceKey<double>(ElevationVSG self) => self.Default;
					}

					public static readonly MarginVSG Margin = new();
					public partial class MarginVSG
					{
						[ResourceKeyDefinition(typeof(Thickness), "ElevatedButtonMargin")]
						public ThemeResourceKey<Thickness> Default = new("ElevatedButtonMargin");

						[ResourceKeyDefinition(typeof(Thickness), "ElevatedButtonDisabledMargin")]
						public ThemeResourceKey<Thickness> Disabled = new("ElevatedButtonDisabledMargin");

						public static implicit operator ThemeResourceKey<Thickness>(MarginVSG self) => self.Default;
					}

					public static partial class StateLayerBackground
					{
						[ResourceKeyDefinition(typeof(Brush), "ElevatedButtonStateLayerBackgroundPointerOver")]
						public static ThemeResourceKey<Brush> PointerOver => new("ElevatedButtonStateLayerBackgroundPointerOver");

						[ResourceKeyDefinition(typeof(Brush), "ElevatedButtonStateLayerBackgroundPressed")]
						public static ThemeResourceKey<Brush> Pressed => new("ElevatedButtonStateLayerBackgroundPressed");

						[ResourceKeyDefinition(typeof(Brush), "ElevatedButtonStateLayerBackgroundFocused")]
						public static ThemeResourceKey<Brush> Focused => new("ElevatedButtonStateLayerBackgroundFocused");

						[ResourceKeyDefinition(typeof(Brush), "ElevatedButtonStateLayerBackgroundPointerFocused")]
						public static ThemeResourceKey<Brush> PointerFocused => new("ElevatedButtonStateLayerBackgroundPointerFocused");
					}

					[ResourceKeyDefinition(typeof(Brush), "ElevatedButtonForeground")]
					public static ThemeResourceKey<Brush> Foreground => new("ElevatedButtonForeground");

					[ResourceKeyDefinition(typeof(bool), "ElevatedButtonIsTintEnabled")]
					public static StaticResourceKey<bool> IsTintEnabled => new("ElevatedButtonIsTintEnabled");
				}

				public static partial class Filled
				{
					public static readonly BackgroundVSG Background = new();
					public partial class BackgroundVSG
					{
						[ResourceKeyDefinition(typeof(Brush), "FilledButtonBackground")]
						public ThemeResourceKey<Brush> Default = new("FilledButtonBackground");

						[ResourceKeyDefinition(typeof(Brush), "FilledButtonBackgroundPointerOver")]
						public ThemeResourceKey<Brush> PointerOver = new("FilledButtonBackgroundPointerOver");

						[ResourceKeyDefinition(typeof(Brush), "FilledButtonBackgroundPressed")]
						public ThemeResourceKey<Brush> Pressed = new("FilledButtonBackgroundPressed");

						[ResourceKeyDefinition(typeof(Brush), "FilledButtonBackgroundDisabled")]
						public ThemeResourceKey<Brush> Disabled = new("FilledButtonBackgroundDisabled");

						[ResourceKeyDefinition(typeof(Brush), "FilledButtonBackgroundFocused")]
						public ThemeResourceKey<Brush> Focused = new("FilledButtonBackgroundFocused");

						[ResourceKeyDefinition(typeof(Brush), "FilledButtonBackgroundPointerFocused")]
						public ThemeResourceKey<Brush> PointerFocused = new("FilledButtonBackgroundPointerFocused");

						public static implicit operator ThemeResourceKey<Brush>(BackgroundVSG self) => self.Default;
					}

					public static readonly BorderBrushVSG BorderBrush = new();
					public partial class BorderBrushVSG
					{
						[ResourceKeyDefinition(typeof(Brush), "FilledButtonBorderBrush")]
						public ThemeResourceKey<Brush> Default = new("FilledButtonBorderBrush");

						[ResourceKeyDefinition(typeof(Brush), "FilledButtonBorderBrushPointerOver")]
						public ThemeResourceKey<Brush> PointerOver = new("FilledButtonBorderBrushPointerOver");

						[ResourceKeyDefinition(typeof(Brush), "FilledButtonBorderBrushPressed")]
						public ThemeResourceKey<Brush> Pressed = new("FilledButtonBorderBrushPressed");

						[ResourceKeyDefinition(typeof(Brush), "FilledButtonBorderBrushDisabled")]
						public ThemeResourceKey<Brush> Disabled = new("FilledButtonBorderBrushDisabled");

						[ResourceKeyDefinition(typeof(Brush), "FilledButtonBorderBrushFocused")]
						public ThemeResourceKey<Brush> Focused = new("FilledButtonBorderBrushFocused");

						[ResourceKeyDefinition(typeof(Brush), "FilledButtonBorderBrushPointerFocused")]
						public ThemeResourceKey<Brush> PointerFocused = new("FilledButtonBorderBrushPointerFocused");

						public static implicit operator ThemeResourceKey<Brush>(BorderBrushVSG self) => self.Default;
					}

					public static partial class StateLayerBackground
					{
						[ResourceKeyDefinition(typeof(Brush), "FilledButtonStateLayerBackgroundPointerOver")]
						public static ThemeResourceKey<Brush> PointerOver => new("FilledButtonStateLayerBackgroundPointerOver");

						[ResourceKeyDefinition(typeof(Brush), "FilledButtonStateLayerBackgroundPressed")]
						public static ThemeResourceKey<Brush> Pressed => new("FilledButtonStateLayerBackgroundPressed");

						[ResourceKeyDefinition(typeof(Brush), "FilledButtonStateLayerBackgroundFocused")]
						public static ThemeResourceKey<Brush> Focused => new("FilledButtonStateLayerBackgroundFocused");

						[ResourceKeyDefinition(typeof(Brush), "FilledButtonStateLayerBackgroundPointerFocused")]
						public static ThemeResourceKey<Brush> PointerFocused => new("FilledButtonStateLayerBackgroundPointerFocused");
					}

					[ResourceKeyDefinition(typeof(double), "ButtonElevation")]
					public static ThemeResourceKey<double> Elevation => new("ButtonElevation");

					[ResourceKeyDefinition(typeof(Brush), "FilledButtonForeground")]
					public static ThemeResourceKey<Brush> Foreground => new("FilledButtonForeground");

					[ResourceKeyDefinition(typeof(Thickness), "ButtonMargin")]
					public static ThemeResourceKey<Thickness> Margin => new("ButtonMargin");
				}

				public static partial class FilledTonal
				{
					public static readonly BackgroundVSG Background = new();
					public partial class BackgroundVSG
					{
						[ResourceKeyDefinition(typeof(Brush), "FilledTonalButtonBackground")]
						public ThemeResourceKey<Brush> Default = new("FilledTonalButtonBackground");

						[ResourceKeyDefinition(typeof(Brush), "FilledTonalButtonBackgroundPointerOver")]
						public ThemeResourceKey<Brush> PointerOver = new("FilledTonalButtonBackgroundPointerOver");

						[ResourceKeyDefinition(typeof(Brush), "FilledTonalButtonBackgroundPressed")]
						public ThemeResourceKey<Brush> Pressed = new("FilledTonalButtonBackgroundPressed");

						[ResourceKeyDefinition(typeof(Brush), "FilledTonalButtonBackgroundDisabled")]
						public ThemeResourceKey<Brush> Disabled = new("FilledTonalButtonBackgroundDisabled");

						[ResourceKeyDefinition(typeof(Brush), "FilledTonalButtonBackgroundFocused")]
						public ThemeResourceKey<Brush> Focused = new("FilledTonalButtonBackgroundFocused");

						[ResourceKeyDefinition(typeof(Brush), "FilledTonalButtonBackgroundPointerFocused")]
						public ThemeResourceKey<Brush> PointerFocused = new("FilledTonalButtonBackgroundPointerFocused");

						public static implicit operator ThemeResourceKey<Brush>(BackgroundVSG self) => self.Default;
					}

					public static readonly BorderBrushVSG BorderBrush = new();
					public partial class BorderBrushVSG
					{
						[ResourceKeyDefinition(typeof(Brush), "FilledTonalButtonBorderBrush")]
						public ThemeResourceKey<Brush> Default = new("FilledTonalButtonBorderBrush");

						[ResourceKeyDefinition(typeof(Brush), "FilledTonalButtonBorderBrushPointerOver")]
						public ThemeResourceKey<Brush> PointerOver = new("FilledTonalButtonBorderBrushPointerOver");

						[ResourceKeyDefinition(typeof(Brush), "FilledTonalButtonBorderBrushPressed")]
						public ThemeResourceKey<Brush> Pressed = new("FilledTonalButtonBorderBrushPressed");

						[ResourceKeyDefinition(typeof(Brush), "FilledTonalButtonBorderBrushDisabled")]
						public ThemeResourceKey<Brush> Disabled = new("FilledTonalButtonBorderBrushDisabled");

						[ResourceKeyDefinition(typeof(Brush), "FilledTonalButtonBorderBrushFocused")]
						public ThemeResourceKey<Brush> Focused = new("FilledTonalButtonBorderBrushFocused");

						[ResourceKeyDefinition(typeof(Brush), "FilledTonalButtonBorderBrushPointerFocused")]
						public ThemeResourceKey<Brush> PointerFocused = new("FilledTonalButtonBorderBrushPointerFocused");

						public static implicit operator ThemeResourceKey<Brush>(BorderBrushVSG self) => self.Default;
					}

					public static partial class StateLayerBackground
					{
						[ResourceKeyDefinition(typeof(Brush), "FilledTonalButtonStateLayerBackgroundPointerOver")]
						public static ThemeResourceKey<Brush> PointerOver => new("FilledTonalButtonStateLayerBackgroundPointerOver");

						[ResourceKeyDefinition(typeof(Brush), "FilledTonalButtonStateLayerBackgroundPressed")]
						public static ThemeResourceKey<Brush> Pressed => new("FilledTonalButtonStateLayerBackgroundPressed");

						[ResourceKeyDefinition(typeof(Brush), "FilledTonalButtonStateLayerBackgroundFocused")]
						public static ThemeResourceKey<Brush> Focused => new("FilledTonalButtonStateLayerBackgroundFocused");

						[ResourceKeyDefinition(typeof(Brush), "FilledTonalButtonStateLayerBackgroundPointerFocused")]
						public static ThemeResourceKey<Brush> PointerFocused => new("FilledTonalButtonStateLayerBackgroundPointerFocused");
					}

					[ResourceKeyDefinition(typeof(double), "ButtonElevation")]
					public static ThemeResourceKey<double> Elevation => new("ButtonElevation");

					[ResourceKeyDefinition(typeof(Brush), "FilledTonalButtonForeground")]
					public static ThemeResourceKey<Brush> Foreground => new("FilledTonalButtonForeground");

					[ResourceKeyDefinition(typeof(Thickness), "ButtonMargin")]
					public static ThemeResourceKey<Thickness> Margin => new("ButtonMargin");
				}

				public static partial class Icon
				{
					public static partial class Ellipse
					{
						[ResourceKeyDefinition(typeof(string), "IconButtonEllipseHorizontalAlignment")]
						public static StaticResourceKey<string> HorizontalAlignment => new("IconButtonEllipseHorizontalAlignment");

						[ResourceKeyDefinition(typeof(string), "IconButtonEllipseVerticalAlignment")]
						public static StaticResourceKey<string> VerticalAlignment => new("IconButtonEllipseVerticalAlignment");
					}

					public static readonly OpacityHiddenStateVSG OpacityHiddenState = new();
					public partial class OpacityHiddenStateVSG
					{
						[ResourceKeyDefinition(typeof(double), "IconButtonOpacityHiddenState")]
						public ThemeResourceKey<double> Default = new("IconButtonOpacityHiddenState");

						[ResourceKeyDefinition(typeof(double), "IconButtonOpacityVisibleState")]
						public ThemeResourceKey<double> PointerOver = new("IconButtonOpacityVisibleState");

						public static implicit operator ThemeResourceKey<double>(OpacityHiddenStateVSG self) => self.Default;
					}

					[ResourceKeyDefinition(typeof(Brush), "IconButtonEllipseFillFocused")]
					public static ThemeResourceKey<Brush> EllipseFillFocused => new("IconButtonEllipseFillFocused");

					[ResourceKeyDefinition(typeof(Brush), "IconButtonEllipseFillPointerOver")]
					public static ThemeResourceKey<Brush> EllipseFillPointerOver => new("IconButtonEllipseFillPointerOver");

					[ResourceKeyDefinition(typeof(Brush), "IconButtonEllipseFillPressed")]
					public static ThemeResourceKey<Brush> EllipseFillPressed => new("IconButtonEllipseFillPressed");

					[ResourceKeyDefinition(typeof(Brush), "IconButtonForeground")]
					public static ThemeResourceKey<Brush> Foreground => new("IconButtonForeground");

					[ResourceKeyDefinition(typeof(Brush), "IconButtonForegroundDisabled")]
					public static ThemeResourceKey<Brush> ForegroundDisabled => new("IconButtonForegroundDisabled");
				}

				public static partial class Outlined
				{
					public static readonly BackgroundVSG Background = new();
					public partial class BackgroundVSG
					{
						[ResourceKeyDefinition(typeof(Brush), "OutlinedButtonBackground")]
						public ThemeResourceKey<Brush> Default = new("OutlinedButtonBackground");

						[ResourceKeyDefinition(typeof(Brush), "OutlinedButtonBackgroundPointerOver")]
						public ThemeResourceKey<Brush> PointerOver = new("OutlinedButtonBackgroundPointerOver");

						[ResourceKeyDefinition(typeof(Brush), "OutlinedButtonBackgroundPressed")]
						public ThemeResourceKey<Brush> Pressed = new("OutlinedButtonBackgroundPressed");

						[ResourceKeyDefinition(typeof(Brush), "OutlinedButtonBackgroundFocused")]
						public ThemeResourceKey<Brush> Focused = new("OutlinedButtonBackgroundFocused");

						[ResourceKeyDefinition(typeof(Brush), "OutlinedButtonBackgroundPointerFocused")]
						public ThemeResourceKey<Brush> PointerFocused = new("OutlinedButtonBackgroundPointerFocused");

						public static implicit operator ThemeResourceKey<Brush>(BackgroundVSG self) => self.Default;
					}

					public static readonly BorderBrushVSG BorderBrush = new();
					public partial class BorderBrushVSG
					{
						[ResourceKeyDefinition(typeof(Brush), "OutlinedButtonBorderBrush")]
						public ThemeResourceKey<Brush> Default = new("OutlinedButtonBorderBrush");

						[ResourceKeyDefinition(typeof(Brush), "OutlinedButtonBorderBrushPointerOver")]
						public ThemeResourceKey<Brush> PointerOver = new("OutlinedButtonBorderBrushPointerOver");

						[ResourceKeyDefinition(typeof(Brush), "OutlinedButtonBorderBrushPressed")]
						public ThemeResourceKey<Brush> Pressed = new("OutlinedButtonBorderBrushPressed");

						[ResourceKeyDefinition(typeof(Brush), "OutlinedButtonBackgroundDisabled")]
						public ThemeResourceKey<Brush> Disabled = new("OutlinedButtonBackgroundDisabled");

						[ResourceKeyDefinition(typeof(Brush), "OutlinedButtonBorderBrushFocused")]
						public ThemeResourceKey<Brush> Focused = new("OutlinedButtonBorderBrushFocused");

						[ResourceKeyDefinition(typeof(Brush), "OutlinedButtonBorderBrushPointerFocused")]
						public ThemeResourceKey<Brush> PointerFocused = new("OutlinedButtonBorderBrushPointerFocused");

						public static implicit operator ThemeResourceKey<Brush>(BorderBrushVSG self) => self.Default;
					}

					public static partial class StateLayerBackground
					{
						[ResourceKeyDefinition(typeof(Brush), "OutlinedButtonStateLayerBackgroundPointerOver")]
						public static ThemeResourceKey<Brush> PointerOver => new("OutlinedButtonStateLayerBackgroundPointerOver");

						[ResourceKeyDefinition(typeof(Brush), "OutlinedButtonStateLayerBackgroundPressed")]
						public static ThemeResourceKey<Brush> Pressed => new("OutlinedButtonStateLayerBackgroundPressed");

						[ResourceKeyDefinition(typeof(Brush), "OutlinedButtonStateLayerBackgroundFocused")]
						public static ThemeResourceKey<Brush> Focused => new("OutlinedButtonStateLayerBackgroundFocused");

						[ResourceKeyDefinition(typeof(Brush), "OutlinedButtonStateLayerBackgroundPointerFocused")]
						public static ThemeResourceKey<Brush> PointerFocused => new("OutlinedButtonStateLayerBackgroundPointerFocused");
					}

					[ResourceKeyDefinition(typeof(Thickness), "OutlinedButtonBorderThickness")]
					public static ThemeResourceKey<Thickness> BorderThickness => new("OutlinedButtonBorderThickness");

					[ResourceKeyDefinition(typeof(Brush), "OutlinedButtonForeground")]
					public static ThemeResourceKey<Brush> Foreground => new("OutlinedButtonForeground");
				}

				public static partial class Text
				{
					public static readonly BackgroundVSG Background = new();
					public partial class BackgroundVSG
					{
						[ResourceKeyDefinition(typeof(Brush), "TextButtonBackground")]
						public ThemeResourceKey<Brush> Default = new("TextButtonBackground");

						[ResourceKeyDefinition(typeof(Brush), "TextButtonBackgroundPointerOver")]
						public ThemeResourceKey<Brush> PointerOver = new("TextButtonBackgroundPointerOver");

						[ResourceKeyDefinition(typeof(Brush), "TextButtonBackgroundPressed")]
						public ThemeResourceKey<Brush> Pressed = new("TextButtonBackgroundPressed");

						[ResourceKeyDefinition(typeof(Brush), "TextButtonBackgroundFocused")]
						public ThemeResourceKey<Brush> Focused = new("TextButtonBackgroundFocused");

						[ResourceKeyDefinition(typeof(Brush), "TextButtonBackgroundPointerFocused")]
						public ThemeResourceKey<Brush> PointerFocused = new("TextButtonBackgroundPointerFocused");

						public static implicit operator ThemeResourceKey<Brush>(BackgroundVSG self) => self.Default;
					}

					public static readonly BorderBrushVSG BorderBrush = new();
					public partial class BorderBrushVSG
					{
						[ResourceKeyDefinition(typeof(Brush), "TextButtonBorderBrush")]
						public ThemeResourceKey<Brush> Default = new("TextButtonBorderBrush");

						[ResourceKeyDefinition(typeof(Brush), "TextButtonBorderBrushPointerOver")]
						public ThemeResourceKey<Brush> PointerOver = new("TextButtonBorderBrushPointerOver");

						[ResourceKeyDefinition(typeof(Brush), "TextButtonBorderBrushPressed")]
						public ThemeResourceKey<Brush> Pressed = new("TextButtonBorderBrushPressed");

						[ResourceKeyDefinition(typeof(Brush), "TextButtonBorderBrushDisabled")]
						public ThemeResourceKey<Brush> Disabled = new("TextButtonBorderBrushDisabled");

						[ResourceKeyDefinition(typeof(Brush), "TextButtonBorderBrushFocused")]
						public ThemeResourceKey<Brush> Focused = new("TextButtonBorderBrushFocused");

						[ResourceKeyDefinition(typeof(Brush), "TextButtonBorderBrushPointerFocused")]
						public ThemeResourceKey<Brush> PointerFocused = new("TextButtonBorderBrushPointerFocused");

						public static implicit operator ThemeResourceKey<Brush>(BorderBrushVSG self) => self.Default;
					}

					public static partial class StateLayerBackground
					{
						[ResourceKeyDefinition(typeof(Brush), "TextButtonStateLayerBackgroundPointerOver")]
						public static ThemeResourceKey<Brush> PointerOver => new("TextButtonStateLayerBackgroundPointerOver");

						[ResourceKeyDefinition(typeof(Brush), "TextButtonStateLayerBackgroundPressed")]
						public static ThemeResourceKey<Brush> Pressed => new("TextButtonStateLayerBackgroundPressed");

						[ResourceKeyDefinition(typeof(Brush), "TextButtonStateLayerBackgroundFocused")]
						public static ThemeResourceKey<Brush> Focused => new("TextButtonStateLayerBackgroundFocused");

						[ResourceKeyDefinition(typeof(Brush), "TextButtonStateLayerBackgroundPointerFocused")]
						public static ThemeResourceKey<Brush> PointerFocused => new("TextButtonStateLayerBackgroundPointerFocused");
					}

					[ResourceKeyDefinition(typeof(Brush), "TextButtonForeground")]
					public static ThemeResourceKey<Brush> Foreground => new("TextButtonForeground");

					[ResourceKeyDefinition(typeof(Thickness), "TextButtonPadding")]
					public static ThemeResourceKey<Thickness> Padding => new("TextButtonPadding");
				}

				[ResourceKeyDefinition(typeof(Thickness), "ButtonBorderThickness")]
				public static ThemeResourceKey<Thickness> BorderThickness => new("ButtonBorderThickness");

				[ResourceKeyDefinition(typeof(CornerRadius), "ButtonCornerRadius")]
				public static ThemeResourceKey<CornerRadius> CornerRadius => new("ButtonCornerRadius");

				[ResourceKeyDefinition(typeof(string), "ButtonHorizontalContentAlignment")]
				public static StaticResourceKey<string> HorizontalContentAlignment => new("ButtonHorizontalContentAlignment");

				[ResourceKeyDefinition(typeof(double), "ButtonMinHeight")]
				public static ThemeResourceKey<double> MinHeight => new("ButtonMinHeight");

				[ResourceKeyDefinition(typeof(double), "ButtonMinWidth")]
				public static ThemeResourceKey<double> MinWidth => new("ButtonMinWidth");

				[ResourceKeyDefinition(typeof(Thickness), "ButtonPadding")]
				public static ThemeResourceKey<Thickness> Padding => new("ButtonPadding");

				[ResourceKeyDefinition(typeof(string), "ButtonVerticalContentAlignment")]
				public static StaticResourceKey<string> VerticalContentAlignment => new("ButtonVerticalContentAlignment");
			}

			public static partial class Styles
			{
				[ResourceKeyDefinition(typeof(Style), "MaterialElevatedButtonStyle", TargetType = typeof(global::Microsoft.UI.Xaml.Controls.Button))]
				public static StaticResourceKey<Style> Elevated => new("MaterialElevatedButtonStyle");

				[ResourceKeyDefinition(typeof(Style), "MaterialFilledButtonStyle", TargetType = typeof(global::Microsoft.UI.Xaml.Controls.Button))]
				public static StaticResourceKey<Style> Filled => new("MaterialFilledButtonStyle");

				[ResourceKeyDefinition(typeof(Style), "MaterialFilledTonalButtonStyle", TargetType = typeof(global::Microsoft.UI.Xaml.Controls.Button))]
				public static StaticResourceKey<Style> FilledTonal => new("MaterialFilledTonalButtonStyle");

				[ResourceKeyDefinition(typeof(Style), "MaterialOutlinedButtonStyle", TargetType = typeof(global::Microsoft.UI.Xaml.Controls.Button))]
				public static StaticResourceKey<Style> Outlined => new("MaterialOutlinedButtonStyle");

				[ResourceKeyDefinition(typeof(Style), "MaterialTextButtonStyle", TargetType = typeof(global::Microsoft.UI.Xaml.Controls.Button))]
				public static StaticResourceKey<Style> Text => new("MaterialTextButtonStyle");

				[ResourceKeyDefinition(typeof(Style), "MaterialIconButtonStyle", TargetType = typeof(global::Microsoft.UI.Xaml.Controls.Button))]
				public static StaticResourceKey<Style> Icon => new("MaterialIconButtonStyle");
			}
		}
	}
}
