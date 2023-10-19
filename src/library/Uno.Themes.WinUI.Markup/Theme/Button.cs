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
	public static partial class Button
	{
		public static partial class Resources
		{
			public static partial class Elevated
			{
				public static partial class Background
				{
					[ResourceKeyDefinition(typeof(Brush), "ElevatedButtonBackground")]
					public static ThemeResourceKey<Brush> Default => new("ElevatedButtonBackground");

					[ResourceKeyDefinition(typeof(Brush), "ElevatedButtonBackgroundPointerOver")]
					public static ThemeResourceKey<Brush> PointerOver => new("ElevatedButtonBackgroundPointerOver");

					[ResourceKeyDefinition(typeof(Brush), "ElevatedButtonBackgroundPressed")]
					public static ThemeResourceKey<Brush> Pressed => new("ElevatedButtonBackgroundPressed");

					[ResourceKeyDefinition(typeof(Brush), "ElevatedButtonBackgroundDisabled")]
					public static ThemeResourceKey<Brush> Disabled => new("ElevatedButtonBackgroundDisabled");

					[ResourceKeyDefinition(typeof(Brush), "ElevatedButtonBackgroundFocused")]
					public static ThemeResourceKey<Brush> Focused => new("ElevatedButtonBackgroundFocused");

					[ResourceKeyDefinition(typeof(Brush), "ElevatedButtonBackgroundPointerFocused")]
					public static ThemeResourceKey<Brush> PointerFocused => new("ElevatedButtonBackgroundPointerFocused");
				}

				public static partial class BorderBrush
				{
					[ResourceKeyDefinition(typeof(Brush), "ElevatedButtonBorderBrush")]
					public static ThemeResourceKey<Brush> Default => new("ElevatedButtonBorderBrush");

					[ResourceKeyDefinition(typeof(Brush), "ElevatedButtonBorderBrushPointerOver")]
					public static ThemeResourceKey<Brush> PointerOver => new("ElevatedButtonBorderBrushPointerOver");

					[ResourceKeyDefinition(typeof(Brush), "ElevatedButtonBorderBrushPressed")]
					public static ThemeResourceKey<Brush> Pressed => new("ElevatedButtonBorderBrushPressed");

					[ResourceKeyDefinition(typeof(Brush), "ElevatedButtonBorderBrushDisabled")]
					public static ThemeResourceKey<Brush> Disabled => new("ElevatedButtonBorderBrushDisabled");

					[ResourceKeyDefinition(typeof(Brush), "ElevatedButtonBorderBrushFocused")]
					public static ThemeResourceKey<Brush> Focused => new("ElevatedButtonBorderBrushFocused");

					[ResourceKeyDefinition(typeof(Brush), "ElevatedButtonBorderBrushPointerFocused")]
					public static ThemeResourceKey<Brush> PointerFocused => new("ElevatedButtonBorderBrushPointerFocused");
				}

				public static partial class Elevation
				{
					[ResourceKeyDefinition(typeof(double), "ElevatedButtonElevation")]
					public static ThemeResourceKey<double> Default => new("ElevatedButtonElevation");

					[ResourceKeyDefinition(typeof(double), "ElevatedButtonElevationDisabled")]
					public static ThemeResourceKey<double> Disabled => new("ElevatedButtonElevationDisabled");
				}

				public static partial class Margin
				{
					[ResourceKeyDefinition(typeof(Thickness), "ElevatedButtonMargin")]
					public static ThemeResourceKey<Thickness> Default => new("ElevatedButtonMargin");

					[ResourceKeyDefinition(typeof(Thickness), "ElevatedButtonDisabledMargin")]
					public static ThemeResourceKey<Thickness> Disabled => new("ElevatedButtonDisabledMargin");
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
				public static partial class Background
				{
					[ResourceKeyDefinition(typeof(Brush), "FilledButtonBackground")]
					public static ThemeResourceKey<Brush> Default => new("FilledButtonBackground");

					[ResourceKeyDefinition(typeof(Brush), "FilledButtonBackgroundPointerOver")]
					public static ThemeResourceKey<Brush> PointerOver => new("FilledButtonBackgroundPointerOver");

					[ResourceKeyDefinition(typeof(Brush), "FilledButtonBackgroundPressed")]
					public static ThemeResourceKey<Brush> Pressed => new("FilledButtonBackgroundPressed");

					[ResourceKeyDefinition(typeof(Brush), "FilledButtonBackgroundDisabled")]
					public static ThemeResourceKey<Brush> Disabled => new("FilledButtonBackgroundDisabled");

					[ResourceKeyDefinition(typeof(Brush), "FilledButtonBackgroundFocused")]
					public static ThemeResourceKey<Brush> Focused => new("FilledButtonBackgroundFocused");

					[ResourceKeyDefinition(typeof(Brush), "FilledButtonBackgroundPointerFocused")]
					public static ThemeResourceKey<Brush> PointerFocused => new("FilledButtonBackgroundPointerFocused");
				}

				public static partial class BorderBrush
				{
					[ResourceKeyDefinition(typeof(Brush), "FilledButtonBorderBrush")]
					public static ThemeResourceKey<Brush> Default => new("FilledButtonBorderBrush");

					[ResourceKeyDefinition(typeof(Brush), "FilledButtonBorderBrushPointerOver")]
					public static ThemeResourceKey<Brush> PointerOver => new("FilledButtonBorderBrushPointerOver");

					[ResourceKeyDefinition(typeof(Brush), "FilledButtonBorderBrushPressed")]
					public static ThemeResourceKey<Brush> Pressed => new("FilledButtonBorderBrushPressed");

					[ResourceKeyDefinition(typeof(Brush), "FilledButtonBorderBrushDisabled")]
					public static ThemeResourceKey<Brush> Disabled => new("FilledButtonBorderBrushDisabled");

					[ResourceKeyDefinition(typeof(Brush), "FilledButtonBorderBrushFocused")]
					public static ThemeResourceKey<Brush> Focused => new("FilledButtonBorderBrushFocused");

					[ResourceKeyDefinition(typeof(Brush), "FilledButtonBorderBrushPointerFocused")]
					public static ThemeResourceKey<Brush> PointerFocused => new("FilledButtonBorderBrushPointerFocused");
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
				public static partial class Background
				{
					[ResourceKeyDefinition(typeof(Brush), "FilledTonalButtonBackground")]
					public static ThemeResourceKey<Brush> Default => new("FilledTonalButtonBackground");

					[ResourceKeyDefinition(typeof(Brush), "FilledTonalButtonBackgroundPointerOver")]
					public static ThemeResourceKey<Brush> PointerOver => new("FilledTonalButtonBackgroundPointerOver");

					[ResourceKeyDefinition(typeof(Brush), "FilledTonalButtonBackgroundPressed")]
					public static ThemeResourceKey<Brush> Pressed => new("FilledTonalButtonBackgroundPressed");

					[ResourceKeyDefinition(typeof(Brush), "FilledTonalButtonBackgroundDisabled")]
					public static ThemeResourceKey<Brush> Disabled => new("FilledTonalButtonBackgroundDisabled");

					[ResourceKeyDefinition(typeof(Brush), "FilledTonalButtonBackgroundFocused")]
					public static ThemeResourceKey<Brush> Focused => new("FilledTonalButtonBackgroundFocused");

					[ResourceKeyDefinition(typeof(Brush), "FilledTonalButtonBackgroundPointerFocused")]
					public static ThemeResourceKey<Brush> PointerFocused => new("FilledTonalButtonBackgroundPointerFocused");
				}

				public static partial class BorderBrush
				{
					[ResourceKeyDefinition(typeof(Brush), "FilledTonalButtonBorderBrush")]
					public static ThemeResourceKey<Brush> Default => new("FilledTonalButtonBorderBrush");

					[ResourceKeyDefinition(typeof(Brush), "FilledTonalButtonBorderBrushPointerOver")]
					public static ThemeResourceKey<Brush> PointerOver => new("FilledTonalButtonBorderBrushPointerOver");

					[ResourceKeyDefinition(typeof(Brush), "FilledTonalButtonBorderBrushPressed")]
					public static ThemeResourceKey<Brush> Pressed => new("FilledTonalButtonBorderBrushPressed");

					[ResourceKeyDefinition(typeof(Brush), "FilledTonalButtonBorderBrushDisabled")]
					public static ThemeResourceKey<Brush> Disabled => new("FilledTonalButtonBorderBrushDisabled");

					[ResourceKeyDefinition(typeof(Brush), "FilledTonalButtonBorderBrushFocused")]
					public static ThemeResourceKey<Brush> Focused => new("FilledTonalButtonBorderBrushFocused");

					[ResourceKeyDefinition(typeof(Brush), "FilledTonalButtonBorderBrushPointerFocused")]
					public static ThemeResourceKey<Brush> PointerFocused => new("FilledTonalButtonBorderBrushPointerFocused");
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

				public static partial class OpacityHiddenState
				{
					[ResourceKeyDefinition(typeof(double), "IconButtonOpacityHiddenState")]
					public static ThemeResourceKey<double> Default => new("IconButtonOpacityHiddenState");

					[ResourceKeyDefinition(typeof(double), "IconButtonOpacityVisibleState")]
					public static ThemeResourceKey<double> PointerOver => new("IconButtonOpacityVisibleState");
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
				public static partial class Background
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
				}

				public static partial class BorderBrush
				{
					[ResourceKeyDefinition(typeof(Brush), "OutlinedButtonBorderBrush")]
					public static ThemeResourceKey<Brush> Default => new("OutlinedButtonBorderBrush");

					[ResourceKeyDefinition(typeof(Brush), "OutlinedButtonBorderBrushPointerOver")]
					public static ThemeResourceKey<Brush> PointerOver => new("OutlinedButtonBorderBrushPointerOver");

					[ResourceKeyDefinition(typeof(Brush), "OutlinedButtonBorderBrushPressed")]
					public static ThemeResourceKey<Brush> Pressed => new("OutlinedButtonBorderBrushPressed");

					[ResourceKeyDefinition(typeof(Brush), "OutlinedButtonBackgroundDisabled")]
					public static ThemeResourceKey<Brush> Disabled => new("OutlinedButtonBackgroundDisabled");

					[ResourceKeyDefinition(typeof(Brush), "OutlinedButtonBorderBrushFocused")]
					public static ThemeResourceKey<Brush> Focused => new("OutlinedButtonBorderBrushFocused");

					[ResourceKeyDefinition(typeof(Brush), "OutlinedButtonBorderBrushPointerFocused")]
					public static ThemeResourceKey<Brush> PointerFocused => new("OutlinedButtonBorderBrushPointerFocused");
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
				public static partial class Background
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
				}

				public static partial class BorderBrush
				{
					[ResourceKeyDefinition(typeof(Brush), "TextButtonBorderBrush")]
					public static ThemeResourceKey<Brush> Default => new("TextButtonBorderBrush");

					[ResourceKeyDefinition(typeof(Brush), "TextButtonBorderBrushPointerOver")]
					public static ThemeResourceKey<Brush> PointerOver => new("TextButtonBorderBrushPointerOver");

					[ResourceKeyDefinition(typeof(Brush), "TextButtonBorderBrushPressed")]
					public static ThemeResourceKey<Brush> Pressed => new("TextButtonBorderBrushPressed");

					[ResourceKeyDefinition(typeof(Brush), "TextButtonBorderBrushDisabled")]
					public static ThemeResourceKey<Brush> Disabled => new("TextButtonBorderBrushDisabled");

					[ResourceKeyDefinition(typeof(Brush), "TextButtonBorderBrushFocused")]
					public static ThemeResourceKey<Brush> Focused => new("TextButtonBorderBrushFocused");

					[ResourceKeyDefinition(typeof(Brush), "TextButtonBorderBrushPointerFocused")]
					public static ThemeResourceKey<Brush> PointerFocused => new("TextButtonBorderBrushPointerFocused");
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
