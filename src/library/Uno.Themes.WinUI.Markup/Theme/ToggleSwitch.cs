using System;
using Windows.UI;
using Microsoft.UI.Text;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Media;
using Uno.Extensions.Markup;
using Uno.Extensions.Markup.Internals;
using Windows.Media.Streaming.Adaptive;

namespace Uno.Themes.Markup;

public static partial class Theme
{
	public static partial class ToggleSwitch
	{
		public static partial class Resources
		{
			public static class Default
			{
				public static partial class Knob
				{
					public static partial class Icon
					{
						[ResourceKeyDefinition(typeof(Thickness), "KnobIconPadding")]
						public static ThemeResourceKey<Thickness> Padding => new("KnobIconPadding");

						[ResourceKeyDefinition(typeof(double), "KnobIconSize")]
						public static ThemeResourceKey<double> Size => new("KnobIconSize");
					}

					public static partial class Fill
					{
						[ResourceKeyDefinition(typeof(Brush), "ToggleSwitchKnobOffFill")]
						public static ThemeResourceKey<Brush> Off => new("ToggleSwitchKnobOffFill");

						[ResourceKeyDefinition(typeof(Brush), "ToggleSwitchKnobOffFillPointerOver")]
						public static ThemeResourceKey<Brush> OffPointerOver => new("ToggleSwitchKnobOffFillPointerOver");

						[ResourceKeyDefinition(typeof(Brush), "ToggleSwitchKnobOffFillFocused")]
						public static ThemeResourceKey<Brush> OffFocused => new("ToggleSwitchKnobOffFillFocused");

						[ResourceKeyDefinition(typeof(Brush), "ToggleSwitchKnobOffFillPressed")]
						public static ThemeResourceKey<Brush> OffPressed => new("ToggleSwitchKnobOffFillPressed");

						[ResourceKeyDefinition(typeof(Brush), "ToggleSwitchKnobOffFillDisabled")]
						public static ThemeResourceKey<Brush> OffDisabled => new("ToggleSwitchKnobOffFillDisabled");

						[ResourceKeyDefinition(typeof(Brush), "ToggleSwitchKnobOnFill")]
						public static ThemeResourceKey<Brush> On => new("ToggleSwitchKnobOnFill");

						[ResourceKeyDefinition(typeof(Brush), "ToggleSwitchKnobOnFillPointerOver")]
						public static ThemeResourceKey<Brush> OnPointerOver => new("ToggleSwitchKnobOnFillPointerOver");

						[ResourceKeyDefinition(typeof(Brush), "ToggleSwitchKnobOnFillFocused")]
						public static ThemeResourceKey<Brush> OnFocused => new("ToggleSwitchKnobOnFillFocused");

						[ResourceKeyDefinition(typeof(Brush), "ToggleSwitchKnobOnFillPressed")]
						public static ThemeResourceKey<Brush> OnPressed => new("ToggleSwitchKnobOnFillPressed");

						[ResourceKeyDefinition(typeof(Brush), "ToggleSwitchKnobOnFillDisabled")]
						public static ThemeResourceKey<Brush> OnDisabled => new("ToggleSwitchKnobOnFillDisabled");
					}

					public static partial class ShadowFill
					{
						[ResourceKeyDefinition(typeof(Brush), "ToggleSwitchKnobOffShadowFill")]
						public static ThemeResourceKey<Brush> Off => new("ToggleSwitchKnobOffShadowFill");

						[ResourceKeyDefinition(typeof(Brush), "ToggleSwitchKnobOffShadowFillPointerOver")]
						public static ThemeResourceKey<Brush> OffPointerOver => new("ToggleSwitchKnobOffShadowFillPointerOver");

						[ResourceKeyDefinition(typeof(Brush), "ToggleSwitchKnobOffShadowFillFocused")]
						public static ThemeResourceKey<Brush> OffFocused => new("ToggleSwitchKnobOffShadowFillFocused");

						[ResourceKeyDefinition(typeof(Brush), "ToggleSwitchKnobOffShadowFillPressed")]
						public static ThemeResourceKey<Brush> OffPressed => new("ToggleSwitchKnobOffShadowFillPressed");

						[ResourceKeyDefinition(typeof(Brush), "ToggleSwitchKnobOnShadowFill")]
						public static ThemeResourceKey<Brush> On => new("ToggleSwitchKnobOnShadowFill");

						[ResourceKeyDefinition(typeof(Brush), "ToggleSwitchKnobOnShadowFillPointerOver")]
						public static ThemeResourceKey<Brush> OnPointerOver => new("ToggleSwitchKnobOnShadowFillPointerOver");

						[ResourceKeyDefinition(typeof(Brush), "ToggleSwitchKnobOnShadowFillFocused")]
						public static ThemeResourceKey<Brush> OnFocused => new("ToggleSwitchKnobOnShadowFillFocused");

						[ResourceKeyDefinition(typeof(Brush), "ToggleSwitchKnobOnShadowFillPressed")]
						public static ThemeResourceKey<Brush> OnPressed => new("ToggleSwitchKnobOnShadowFillPressed");
					}

					public static partial class BoundsFill
					{
						[ResourceKeyDefinition(typeof(Brush), "ToggleSwitchKnobBoundsFill")]
						public static ThemeResourceKey<Brush> Default => new("ToggleSwitchKnobBoundsFill");

						[ResourceKeyDefinition(typeof(Brush), "ToggleSwitchKnobBoundsFillPointerOver")]
						public static ThemeResourceKey<Brush> PointerOver => new("ToggleSwitchKnobBoundsFillPointerOver");

						[ResourceKeyDefinition(typeof(Brush), "ToggleSwitchKnobBoundsFillFocused")]
						public static ThemeResourceKey<Brush> Focused => new("ToggleSwitchKnobBoundsFillFocused");

						[ResourceKeyDefinition(typeof(Brush), "ToggleSwitchKnobBoundsFillPressed")]
						public static ThemeResourceKey<Brush> Pressed => new("ToggleSwitchKnobBoundsFillPressed");

						[ResourceKeyDefinition(typeof(Brush), "ToggleSwitchKnobBoundsFillDisabled")]
						public static ThemeResourceKey<Brush> Disabled => new("ToggleSwitchKnobBoundsFillDisabled");

						[ResourceKeyDefinition(typeof(Brush), "ToggleSwitchOnSwitchKnobBoundsFill")]
						public static ThemeResourceKey<Brush> On => new("ToggleSwitchOnSwitchKnobBoundsFill");
					}

					public static partial class Margin
					{
						[ResourceKeyDefinition(typeof(Thickness), "SwitchKnobOnMargin")]
						public static ThemeResourceKey<Thickness> On => new("SwitchKnobOnMargin");
	
						[ResourceKeyDefinition(typeof(Thickness), "SwitchKnobOffMargin")]
						public static ThemeResourceKey<Thickness> Off => new("SwitchKnobOffMargin");
					}

					public static partial class ShadowMargin
					{
						[ResourceKeyDefinition(typeof(Thickness), "SwitchKnobOnShadowMargin")]
						public static ThemeResourceKey<Thickness> On => new("SwitchKnobOnShadowMargin");

						[ResourceKeyDefinition(typeof(Thickness), "SwitchKnobOffShadowMargin")]
						public static ThemeResourceKey<Thickness> Off => new("SwitchKnobOffShadowMargin");
					}

					[ResourceKeyDefinition(typeof(double), "SwitchKnobHeight")]
					public static ThemeResourceKey<double> Height => new("SwitchKnobHeight");

					[ResourceKeyDefinition(typeof(double), "SwitchKnobRadius")]
					public static ThemeResourceKey<double> Radius => new("SwitchKnobRadius");

					[ResourceKeyDefinition(typeof(double), "SwitchKnobStrokeThickness")]
					public static ThemeResourceKey<double> StrokeThickness => new("SwitchKnobStrokeThickness");

					[ResourceKeyDefinition(typeof(double), "SwitchKnobWidth")]
					public static ThemeResourceKey<double> Width => new("SwitchKnobWidth");

					[ResourceKeyDefinition(typeof(double), "SwitchKnobIncludingOffShadowWidth")]
					public static ThemeResourceKey<double> IncludingOffShadowWidth => new("SwitchKnobIncludingOffShadowWidth");

					[ResourceKeyDefinition(typeof(double), "SwitchKnobShadowSize")]
					public static ThemeResourceKey<double> ShadowSize => new("SwitchKnobShadowSize");
				}

				public static partial class OuterBorder
				{
					public static partial class Fill
					{
						[ResourceKeyDefinition(typeof(Brush), "ToggleSwitchOuterBorderFill")]
						public static ThemeResourceKey<Brush> Default => new("ToggleSwitchOuterBorderFill");

						[ResourceKeyDefinition(typeof(Brush), "ToggleSwitchOffOuterBorderFill")]
						public static ThemeResourceKey<Brush> Off => new("ToggleSwitchOffOuterBorderFill");
					}

					public static partial class Stroke
					{
						[ResourceKeyDefinition(typeof(Brush), "ToggleSwitchOuterBorderStroke")]
						public static ThemeResourceKey<Brush> Default => new("ToggleSwitchOuterBorderStroke");

						[ResourceKeyDefinition(typeof(Brush), "ToggleSwitchOffOuterBorderStroke")]
						public static ThemeResourceKey<Brush> Off => new("ToggleSwitchOffOuterBorderStroke");
					}
				}

				public static partial class IconPresenterForeground
				{
					[ResourceKeyDefinition(typeof(Brush), "ToggleSwitchOnIconPresenterForeground")]
					public static ThemeResourceKey<Brush> On => new("ToggleSwitchOnIconPresenterForeground");

					[ResourceKeyDefinition(typeof(Brush), "ToggleSwitchOnIconPresenterForegroundDisabled")]
					public static ThemeResourceKey<Brush> OnDisabled => new("ToggleSwitchOnIconPresenterForegroundDisabled");

					[ResourceKeyDefinition(typeof(Brush), "ToggleSwitchOffIconPresenterForeground")]
					public static ThemeResourceKey<Brush> Off => new("ToggleSwitchOffIconPresenterForeground");

					[ResourceKeyDefinition(typeof(Brush), "ToggleSwitchOffIconPresenterForegroundDisabled")]
					public static ThemeResourceKey<Brush> OffDisabled => new("ToggleSwitchOffIconPresenterForegroundDisabled");
				}

				public static partial class Thumb
				{
					[ResourceKeyDefinition(typeof(double), "MediumThumbSize")]
					public static ThemeResourceKey<double> MediumSize => new("MediumThumbSize");

					[ResourceKeyDefinition(typeof(double), "LargeThumbSize")]
					public static ThemeResourceKey<double> LargeSize => new("LargeThumbSize");

					[ResourceKeyDefinition(typeof(Brush), "ToggleSwitchThumb")]
					public static ThemeResourceKey<Brush> Fill => new("ToggleSwitchThumb");

					[ResourceKeyDefinition(typeof(CornerRadius), "LargeThumbCornerRadius")]
					public static ThemeResourceKey<CornerRadius> LargeCornerRadius => new("LargeThumbCornerRadius");
				}

				[ResourceKeyDefinition(typeof(double), "ToggleSwitchThemeMinWidth")]
				public static ThemeResourceKey<double> MinWidth => new("ToggleSwitchThemeMinWidth");
			}
		}

		public static partial class Styles
		{
			[ResourceKeyDefinition(typeof(Style), "ToggleSwitchStyle", TargetType = typeof(global::Microsoft.UI.Xaml.Controls.ToggleSwitch))]
			public static StaticResourceKey<Style> Default => new("ToggleSwitchStyle");
		}
	}
}
