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
	public static partial class ToggleButton
	{
		public static partial class Resources
		{
			public static partial class Icon
			{
				public static partial class Background
				{
					[ResourceKeyDefinition(typeof(Brush), "IconToggleButtonBackground")]
					public static ThemeResourceKey<Brush> Default => new("IconToggleButtonBackground");

					[ResourceKeyDefinition(typeof(Brush), "IconToggleButtonBackgroundPointerOver")]
					public static ThemeResourceKey<Brush> PointerOver => new("IconToggleButtonBackgroundPointerOver");

					[ResourceKeyDefinition(typeof(Brush), "IconToggleButtonBackgroundPressed")]
					public static ThemeResourceKey<Brush> Pressed => new("IconToggleButtonBackgroundPressed");

					[ResourceKeyDefinition(typeof(Brush), "IconToggleButtonBackgroundDisabled")]
					public static ThemeResourceKey<Brush> Disabled => new("IconToggleButtonBackgroundDisabled");

					[ResourceKeyDefinition(typeof(Brush), "IconToggleButtonBackgroundChecked")]
					public static ThemeResourceKey<Brush> Checked => new("IconToggleButtonBackgroundChecked");

					[ResourceKeyDefinition(typeof(Brush), "IconToggleButtonBackgroundCheckedPointerOver")]
					public static ThemeResourceKey<Brush> CheckedPointerOver => new("IconToggleButtonBackgroundCheckedPointerOver");

					[ResourceKeyDefinition(typeof(Brush), "IconToggleButtonBackgroundCheckedPressed")]
					public static ThemeResourceKey<Brush> CheckedPressed => new("IconToggleButtonBackgroundCheckedPressed");

					[ResourceKeyDefinition(typeof(Brush), "IconToggleButtonBackgroundCheckedDisabled")]
					public static ThemeResourceKey<Brush> CheckedDisabled => new("IconToggleButtonBackgroundCheckedDisabled");

					[ResourceKeyDefinition(typeof(Brush), "IconToggleButtonBackgroundIndeterminate")]
					public static ThemeResourceKey<Brush> IndeterminateNormal => new("IconToggleButtonBackgroundIndeterminate");

					[ResourceKeyDefinition(typeof(Brush), "IconToggleButtonBackgroundIndeterminatePointerOver")]
					public static ThemeResourceKey<Brush> IndeterminatePointerOver => new("IconToggleButtonBackgroundIndeterminatePointerOver");

					[ResourceKeyDefinition(typeof(Brush), "IconToggleButtonBackgroundIndeterminatePressed")]
					public static ThemeResourceKey<Brush> IndeterminatePressed => new("IconToggleButtonBackgroundIndeterminatePressed");

					[ResourceKeyDefinition(typeof(Brush), "IconToggleButtonBackgroundIndeterminateDisabled")]
					public static ThemeResourceKey<Brush> IndeterminateDisabled => new("IconToggleButtonBackgroundIndeterminateDisabled");
				}

				public static partial class BorderBrush
				{
					[ResourceKeyDefinition(typeof(Brush), "IconToggleButtonBorderBrush")]
					public static ThemeResourceKey<Brush> Default => new("IconToggleButtonBorderBrush");

					[ResourceKeyDefinition(typeof(Brush), "IconToggleButtonBorderBrushPointerOver")]
					public static ThemeResourceKey<Brush> PointerOver => new("IconToggleButtonBorderBrushPointerOver");

					[ResourceKeyDefinition(typeof(Brush), "IconToggleButtonBorderBrushPressed")]
					public static ThemeResourceKey<Brush> Pressed => new("IconToggleButtonBorderBrushPressed");

					[ResourceKeyDefinition(typeof(Brush), "IconToggleButtonBorderBrushDisabled")]
					public static ThemeResourceKey<Brush> Disabled => new("IconToggleButtonBorderBrushDisabled");

					[ResourceKeyDefinition(typeof(Brush), "IconToggleButtonBorderBrushChecked")]
					public static ThemeResourceKey<Brush> Checked => new("IconToggleButtonBorderBrushChecked");

					[ResourceKeyDefinition(typeof(Brush), "IconToggleButtonBorderBrushCheckedPointerOver")]
					public static ThemeResourceKey<Brush> CheckedPointerOver => new("IconToggleButtonBorderBrushCheckedPointerOver");

					[ResourceKeyDefinition(typeof(Brush), "IconToggleButtonBorderBrushCheckedPressed")]
					public static ThemeResourceKey<Brush> CheckedPressed => new("IconToggleButtonBorderBrushCheckedPressed");

					[ResourceKeyDefinition(typeof(Brush), "IconToggleButtonBorderBrushCheckedDisabled")]
					public static ThemeResourceKey<Brush> CheckedDisabled => new("IconToggleButtonBorderBrushCheckedDisabled");

					[ResourceKeyDefinition(typeof(Brush), "IconToggleButtonBorderBrushIndeterminate")]
					public static ThemeResourceKey<Brush> IndeterminateNormal => new("IconToggleButtonBorderBrushIndeterminate");

					[ResourceKeyDefinition(typeof(Brush), "IconToggleButtonBorderBrushIndeterminatePointerOver")]
					public static ThemeResourceKey<Brush> IndeterminatePointerOver => new("IconToggleButtonBorderBrushIndeterminatePointerOver");

					[ResourceKeyDefinition(typeof(Brush), "IconToggleButtonBorderBrushIndeterminatePressed")]
					public static ThemeResourceKey<Brush> IndeterminatePressed => new("IconToggleButtonBorderBrushIndeterminatePressed");

					[ResourceKeyDefinition(typeof(Brush), "IconToggleButtonBorderBrushIndeterminateDisabled")]
					public static ThemeResourceKey<Brush> IndeterminateDisabled => new("IconToggleButtonBorderBrushIndeterminateDisabled");
				}

				public static partial class Foreground
				{
					[ResourceKeyDefinition(typeof(Brush), "IconToggleButtonForeground")]
					public static ThemeResourceKey<Brush> Default => new("IconToggleButtonForeground");

					[ResourceKeyDefinition(typeof(Brush), "IconToggleButtonForegroundPointerOver")]
					public static ThemeResourceKey<Brush> PointerOver => new("IconToggleButtonForegroundPointerOver");

					[ResourceKeyDefinition(typeof(Brush), "IconToggleButtonForegroundPressed")]
					public static ThemeResourceKey<Brush> Pressed => new("IconToggleButtonForegroundPressed");

					[ResourceKeyDefinition(typeof(Brush), "IconToggleButtonForegroundDisabled")]
					public static ThemeResourceKey<Brush> Disabled => new("IconToggleButtonForegroundDisabled");

					[ResourceKeyDefinition(typeof(Brush), "IconToggleButtonForegroundChecked")]
					public static ThemeResourceKey<Brush> Checked => new("IconToggleButtonForegroundChecked");

					[ResourceKeyDefinition(typeof(Brush), "IconToggleButtonForegroundCheckedPointerOver")]
					public static ThemeResourceKey<Brush> CheckedPointerOver => new("IconToggleButtonForegroundCheckedPointerOver");

					[ResourceKeyDefinition(typeof(Brush), "IconToggleButtonForegroundCheckedPressed")]
					public static ThemeResourceKey<Brush> CheckedPressed => new("IconToggleButtonForegroundCheckedPressed");

					[ResourceKeyDefinition(typeof(Brush), "IconToggleButtonForegroundCheckedDisabled")]
					public static ThemeResourceKey<Brush> CheckedDisabled => new("IconToggleButtonForegroundCheckedDisabled");

					[ResourceKeyDefinition(typeof(Brush), "IconToggleButtonForegroundIndeterminate")]
					public static ThemeResourceKey<Brush> IndeterminateNormal => new("IconToggleButtonForegroundIndeterminate");

					[ResourceKeyDefinition(typeof(Brush), "IconToggleButtonForegroundIndeterminatePointerOver")]
					public static ThemeResourceKey<Brush> IndeterminatePointerOver => new("IconToggleButtonForegroundIndeterminatePointerOver");

					[ResourceKeyDefinition(typeof(Brush), "IconToggleButtonForegroundIndeterminatePressed")]
					public static ThemeResourceKey<Brush> IndeterminatePressed => new("IconToggleButtonForegroundIndeterminatePressed");

					[ResourceKeyDefinition(typeof(Brush), "IconToggleButtonForegroundIndeterminateDisabled")]
					public static ThemeResourceKey<Brush> IndeterminateDisabled => new("IconToggleButtonForegroundIndeterminateDisabled");
				}

				public static partial class StateCircle
				{
					public static partial class Opacity
					{
						[ResourceKeyDefinition(typeof(double), "IconToggleButtonStateCircleOpacityPointerOver")]
						public static ThemeResourceKey<double> PointerOver => new("IconToggleButtonStateCircleOpacityPointerOver");

						[ResourceKeyDefinition(typeof(double), "IconToggleButtonStateCircleOpacityPressed")]
						public static ThemeResourceKey<double> Pressed => new("IconToggleButtonStateCircleOpacityPressed");
					}

					[ResourceKeyDefinition(typeof(Brush), "IconToggleButtonStateCircleFill")]
					public static ThemeResourceKey<Brush> Fill => new("IconToggleButtonStateCircleFill");

					[ResourceKeyDefinition(typeof(double), "IconToggleButtonStateCircleOpacityFocused")]
					public static ThemeResourceKey<double> OpacityFocused => new("IconToggleButtonStateCircleOpacityFocused");
				}

				[ResourceKeyDefinition(typeof(Thickness), "IconToggleButtonBorderThickness")]
				public static ThemeResourceKey<Thickness> BorderThickness => new("IconToggleButtonBorderThickness");

				[ResourceKeyDefinition(typeof(double), "IconToggleButtonMinHeight")]
				public static ThemeResourceKey<double> MinHeight => new("IconToggleButtonMinHeight");

				[ResourceKeyDefinition(typeof(double), "IconToggleButtonMinWidth")]
				public static ThemeResourceKey<double> MinWidth => new("IconToggleButtonMinWidth");
			}

			public static partial class Text
			{
				public static partial class Background
				{
					[ResourceKeyDefinition(typeof(Brush), "TextToggleButtonBackground")]
					public static ThemeResourceKey<Brush> Default => new("TextToggleButtonBackground");

					[ResourceKeyDefinition(typeof(Brush), "TextToggleButtonBackgroundPointerOver")]
					public static ThemeResourceKey<Brush> PointerOver => new("TextToggleButtonBackgroundPointerOver");

					[ResourceKeyDefinition(typeof(Brush), "TextToggleButtonBackgroundPressed")]
					public static ThemeResourceKey<Brush> Pressed => new("TextToggleButtonBackgroundPressed");

					[ResourceKeyDefinition(typeof(Brush), "TextToggleButtonBackgroundDisabled")]
					public static ThemeResourceKey<Brush> Disabled => new("TextToggleButtonBackgroundDisabled");

					[ResourceKeyDefinition(typeof(Brush), "TextToggleButtonBackgroundChecked")]
					public static ThemeResourceKey<Brush> Checked => new("TextToggleButtonBackgroundChecked");

					[ResourceKeyDefinition(typeof(Brush), "TextToggleButtonBackgroundIndeterminate")]
					public static ThemeResourceKey<Brush> IndeterminateNormal => new("TextToggleButtonBackgroundIndeterminate");

					[ResourceKeyDefinition(typeof(Brush), "TextToggleButtonBackgroundIndeterminatePointerOver")]
					public static ThemeResourceKey<Brush> IndeterminatePointerOver => new("TextToggleButtonBackgroundIndeterminatePointerOver");

					[ResourceKeyDefinition(typeof(Brush), "TextToggleButtonBackgroundIndeterminatePressed")]
					public static ThemeResourceKey<Brush> IndeterminatePressed => new("TextToggleButtonBackgroundIndeterminatePressed");

					[ResourceKeyDefinition(typeof(Brush), "TextToggleButtonBackgroundIndeterminateDisabled")]
					public static ThemeResourceKey<Brush> IndeterminateDisabled => new("TextToggleButtonBackgroundIndeterminateDisabled");
				}

				public static partial class BorderBrush
				{
					[ResourceKeyDefinition(typeof(Brush), "TextToggleButtonBorderBrush")]
					public static ThemeResourceKey<Brush> Default => new("TextToggleButtonBorderBrush");

					[ResourceKeyDefinition(typeof(Brush), "TextToggleButtonBorderBrushPointerOver")]
					public static ThemeResourceKey<Brush> PointerOver => new("TextToggleButtonBorderBrushPointerOver");

					[ResourceKeyDefinition(typeof(Brush), "TextToggleButtonBorderBrushPressed")]
					public static ThemeResourceKey<Brush> Pressed => new("TextToggleButtonBorderBrushPressed");

					[ResourceKeyDefinition(typeof(Brush), "TextToggleButtonBorderBrushDisabled")]
					public static ThemeResourceKey<Brush> Disabled => new("TextToggleButtonBorderBrushDisabled");

					[ResourceKeyDefinition(typeof(Brush), "TextToggleButtonBorderBrushChecked")]
					public static ThemeResourceKey<Brush> Checked => new("TextToggleButtonBorderBrushChecked");

					[ResourceKeyDefinition(typeof(Brush), "TextToggleButtonBorderBrushIndeterminate")]
					public static ThemeResourceKey<Brush> IndeterminateNormal => new("TextToggleButtonBorderBrushIndeterminate");

					[ResourceKeyDefinition(typeof(Brush), "TextToggleButtonBorderBrushIndeterminatePointerOver")]
					public static ThemeResourceKey<Brush> IndeterminatePointerOver => new("TextToggleButtonBorderBrushIndeterminatePointerOver");

					[ResourceKeyDefinition(typeof(Brush), "TextToggleButtonBorderBrushIndeterminatePressed")]
					public static ThemeResourceKey<Brush> IndeterminatePressed => new("TextToggleButtonBorderBrushIndeterminatePressed");

					[ResourceKeyDefinition(typeof(Brush), "TextToggleButtonBorderBrushIndeterminateDisabled")]
					public static ThemeResourceKey<Brush> IndeterminateDisabled => new("TextToggleButtonBorderBrushIndeterminateDisabled");
				}

				public static partial class Foreground
				{
					[ResourceKeyDefinition(typeof(Color), "TextToggleButtonForeground")]
					public static ThemeResourceKey<Color> Default => new("TextToggleButtonForeground");

					[ResourceKeyDefinition(typeof(Color), "TextToggleButtonForegroundPointerOver")]
					public static ThemeResourceKey<Color> PointerOver => new("TextToggleButtonForegroundPointerOver");

					[ResourceKeyDefinition(typeof(Color), "TextToggleButtonForegroundPressed")]
					public static ThemeResourceKey<Color> Pressed => new("TextToggleButtonForegroundPressed");

					[ResourceKeyDefinition(typeof(Brush), "TextToggleButtonForegroundDisabled")]
					public static ThemeResourceKey<Brush> Disabled => new("TextToggleButtonForegroundDisabled");

					[ResourceKeyDefinition(typeof(Color), "TextToggleButtonForegroundChecked")]
					public static ThemeResourceKey<Color> Checked => new("TextToggleButtonForegroundChecked");

					[ResourceKeyDefinition(typeof(Color), "TextToggleButtonForegroundIndeterminate")]
					public static ThemeResourceKey<Color> IndeterminateNormal => new("TextToggleButtonForegroundIndeterminate");

					[ResourceKeyDefinition(typeof(Color), "TextToggleButtonForegroundIndeterminatePointerOver")]
					public static ThemeResourceKey<Color> IndeterminatePointerOver => new("TextToggleButtonForegroundIndeterminatePointerOver");

					[ResourceKeyDefinition(typeof(Color), "TextToggleButtonForegroundIndeterminatePressed")]
					public static ThemeResourceKey<Color> IndeterminatePressed => new("TextToggleButtonForegroundIndeterminatePressed");

					[ResourceKeyDefinition(typeof(Brush), "TextToggleButtonForegroundIndeterminateDisabled")]
					public static ThemeResourceKey<Brush> IndeterminateDisabled => new("TextToggleButtonForegroundIndeterminateDisabled");
				}

				public static partial class Typography
				{
					[ResourceKeyDefinition(typeof(FontFamily), "TextToggleButtonFontFamily")]
					public static ThemeResourceKey<FontFamily> FontFamily => new("TextToggleButtonFontFamily");

					[ResourceKeyDefinition(typeof(double), "TextToggleButtonFontSize")]
					public static ThemeResourceKey<double> FontSize => new("TextToggleButtonFontSize");
				}

				[ResourceKeyDefinition(typeof(Brush), "TextToggleButtonBorderBrushFocusedOverlay")]
				public static ThemeResourceKey<Brush> BorderBrushFocusedOverlay => new("TextToggleButtonBorderBrushFocusedOverlay");

				[ResourceKeyDefinition(typeof(Brush), "TextToggleButtonBorderBrushHoverOverlay")]
				public static ThemeResourceKey<Brush> BorderBrushHoverOverlay => new("TextToggleButtonBorderBrushHoverOverlay");

				[ResourceKeyDefinition(typeof(Brush), "TextToggleButtonBorderBrushSelectedOverlay")]
				public static ThemeResourceKey<Brush> BorderBrushSelectedOverlay => new("TextToggleButtonBorderBrushSelectedOverlay");

				[ResourceKeyDefinition(typeof(Thickness), "TextToggleButtonBorderThickness")]
				public static ThemeResourceKey<Thickness> BorderThickness => new("TextToggleButtonBorderThickness");

				[ResourceKeyDefinition(typeof(CornerRadius), "TextToggleButtonCornerRadius")]
				public static ThemeResourceKey<CornerRadius> CornerRadius => new("TextToggleButtonCornerRadius");

				[ResourceKeyDefinition(typeof(Brush), "TextToggleButtonFeedbackFocused")]
				public static ThemeResourceKey<Brush> FeedbackFocused => new("TextToggleButtonFeedbackFocused");

				[ResourceKeyDefinition(typeof(double), "TextToggleButtonMinHeight")]
				public static ThemeResourceKey<double> MinHeight => new("TextToggleButtonMinHeight");

				[ResourceKeyDefinition(typeof(Thickness), "TextToggleButtonPadding")]
				public static ThemeResourceKey<Thickness> Padding => new("TextToggleButtonPadding");
			}
		}

		public static partial class Styles
		{
			[ResourceKeyDefinition(typeof(Style), "MaterialTextToggleButtonStyle", TargetType = typeof(global::Microsoft.UI.Xaml.Controls.Primitives.ToggleButton))]
			public static StaticResourceKey<Style> Text => new("MaterialTextToggleButtonStyle");

			[ResourceKeyDefinition(typeof(Style), "MaterialIconToggleButtonStyle", TargetType = typeof(global::Microsoft.UI.Xaml.Controls.Primitives.ToggleButton))]
			public static StaticResourceKey<Style> Icon => new("MaterialIconToggleButtonStyle");
		}
	}
}
