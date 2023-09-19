using System;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Media;
using Uno.Extensions.Markup.Internals;

namespace Uno.Themes.Markup
{
	public static partial class Theme
	{
		public static class ToggleSwitch
		{
			public static class Resources
			{
				public static class OuterBorder
				{
					public static class Fill
					{
						[ResourceKeyDefinition(typeof(Brush), "ToggleSwitchOuterBorderFill")]
						public static ResourceValue<Brush> Default => new("ToggleSwitchOuterBorderFill", true);

						[ResourceKeyDefinition(typeof(Brush), "ToggleSwitchOffOuterBorderFill")]
						public static ResourceValue<Brush> Off => new("ToggleSwitchOffOuterBorderFill", true);

					}

					public static class Stroke
					{
						[ResourceKeyDefinition(typeof(Brush), "ToggleSwitchOuterBorderStroke")]
						public static ResourceValue<Brush> Default => new("ToggleSwitchOuterBorderStroke", true);

						[ResourceKeyDefinition(typeof(Brush), "ToggleSwitchOffOuterBorderStroke")]
						public static ResourceValue<Brush> Off => new("ToggleSwitchOffOuterBorderStroke", true);
					}
				}

				public static class Knob
				{
					public static class Fill
					{
						[ResourceKeyDefinition(typeof(Brush), "ToggleSwitchKnobOffFill")]
						public static ResourceValue<Brush> Off => new("ToggleSwitchKnobOffFill", true);

						[ResourceKeyDefinition(typeof(Brush), "ToggleSwitchKnobOffFillPointerOver")]
						public static ResourceValue<Brush> OffPointerOver => new("ToggleSwitchKnobOffFillPointerOver", true);

						[ResourceKeyDefinition(typeof(Brush), "ToggleSwitchKnobOffFillFocused")]
						public static ResourceValue<Brush> OffFocused => new("ToggleSwitchKnobOffFillFocused", true);

						[ResourceKeyDefinition(typeof(Brush), "ToggleSwitchKnobOffFillPressed")]
						public static ResourceValue<Brush> OffPressed => new("ToggleSwitchKnobOffFillPressed", true);

						[ResourceKeyDefinition(typeof(Brush), "ToggleSwitchKnobOffFillDisabled")]
						public static ResourceValue<Brush> OffDisabled => new("ToggleSwitchKnobOffFillDisabled", true);

						[ResourceKeyDefinition(typeof(Brush), "ToggleSwitchKnobOnFill")]
						public static ResourceValue<Brush> On => new("ToggleSwitchKnobOnFill", true);

						[ResourceKeyDefinition(typeof(Brush), "ToggleSwitchKnobOnFillPointerOver")]
						public static ResourceValue<Brush> OnPointerOver => new("ToggleSwitchKnobOnFillPointerOver", true);

						[ResourceKeyDefinition(typeof(Brush), "ToggleSwitchKnobOnFillFocused")]
						public static ResourceValue<Brush> OnFocused => new("ToggleSwitchKnobOnFillFocused", true);

						[ResourceKeyDefinition(typeof(Brush), "ToggleSwitchKnobOnFillPressed")]
						public static ResourceValue<Brush> OnPressed => new("ToggleSwitchKnobOnFillPressed", true);

						[ResourceKeyDefinition(typeof(Brush), "ToggleSwitchKnobOnFillDisabled")]
						public static ResourceValue<Brush> OnDisabled => new("ToggleSwitchKnobOnFillDisabled", true);
					}

					public static class Shadow
					{
						[ResourceKeyDefinition(typeof(Brush), "ToggleSwitchKnobOnShadowFill")]
						public static ResourceValue<Brush> On => new("ToggleSwitchKnobOnShadowFill", true);

						[ResourceKeyDefinition(typeof(Brush), "ToggleSwitchKnobOnShadowFillPointerOver")]
						public static ResourceValue<Brush> OnPointerOver => new("ToggleSwitchKnobOnShadowFillPointerOver", true);

						[ResourceKeyDefinition(typeof(Brush), "ToggleSwitchKnobOnShadowFillFocused")]
						public static ResourceValue<Brush> OnFocused => new("ToggleSwitchKnobOnShadowFillFocused", true);

						[ResourceKeyDefinition(typeof(Brush), "ToggleSwitchKnobOnShadowFillPressed")]
						public static ResourceValue<Brush> OnPressed => new("ToggleSwitchKnobOnShadowFillPressed", true);

						[ResourceKeyDefinition(typeof(Brush), "ToggleSwitchKnobOffShadowFill")]
						public static ResourceValue<Brush> Off => new("ToggleSwitchKnobOffShadowFill", true);

						[ResourceKeyDefinition(typeof(Brush), "ToggleSwitchKnobOffShadowFillPointerOver")]
						public static ResourceValue<Brush> OffPointerOver => new("ToggleSwitchKnobOffShadowFillPointerOver", true);

						[ResourceKeyDefinition(typeof(Brush), "ToggleSwitchKnobOffShadowFillFocused")]
						public static ResourceValue<Brush> OffFocused => new("ToggleSwitchKnobOffShadowFillFocused", true);

						[ResourceKeyDefinition(typeof(Brush), "ToggleSwitchKnobOffShadowFillPressed")]
						public static ResourceValue<Brush> OffPressed => new("ToggleSwitchKnobOffShadowFillPressed", true);

						[ResourceKeyDefinition(typeof(double), "SwitchKnobShadowSize")]
						public static ResourceValue<double> Size => new("SwitchKnobShadowSize", true);

						[ResourceKeyDefinition(typeof(Thickness), "SwitchKnobOnShadowMargin")]
						public static ResourceValue<Thickness> OnMargin => new("SwitchKnobOnShadowMargin", true);

						[ResourceKeyDefinition(typeof(Thickness), "SwitchKnobOffShadowMargin")]
						public static ResourceValue<Thickness> OffMargin => new("SwitchKnobOffShadowMargin", true);

						[ResourceKeyDefinition(typeof(double), "SwitchKnobIncludingOffShadowWidth")]
						public static ResourceValue<double> IncludingOffShadowWidth => new("SwitchKnobIncludingOffShadowWidth", true);
					}

					public static class BoundsFill
					{
						[ResourceKeyDefinition(typeof(Brush), "ToggleSwitchOnSwitchKnobBoundsFill")]
						public static ResourceValue<Brush> OnSwitch => new("ToggleSwitchOnSwitchKnobBoundsFill", true);

						[ResourceKeyDefinition(typeof(Brush), "ToggleSwitchKnobBoundsFill")]
						public static ResourceValue<Brush> Default => new("ToggleSwitchKnobBoundsFill", true);

						[ResourceKeyDefinition(typeof(Brush), "ToggleSwitchKnobBoundsFillPointerOver")]
						public static ResourceValue<Brush> PointerOver => new("ToggleSwitchKnobBoundsFillPointerOver", true);

						[ResourceKeyDefinition(typeof(Brush), "ToggleSwitchKnobBoundsFillFocused")]
						public static ResourceValue<Brush> Focused => new("ToggleSwitchKnobBoundsFillFocused", true);

						[ResourceKeyDefinition(typeof(Brush), "ToggleSwitchKnobBoundsFillPressed")]
						public static ResourceValue<Brush> Pressed => new("ToggleSwitchKnobBoundsFillPressed", true);

						[ResourceKeyDefinition(typeof(Brush), "ToggleSwitchKnobBoundsFillDisabled")]
						public static ResourceValue<Brush> Disabled => new("ToggleSwitchKnobBoundsFillDisabled", true);
					}
			
					[ResourceKeyDefinition(typeof(Thickness), "SwitchKnobStrokeThickness")]
					public static ResourceValue<Thickness> StrokeThickness => new("SwitchKnobStrokeThickness", true);

					[ResourceKeyDefinition(typeof(double), "SwitchKnobWidth")]
					public static ResourceValue<double> Width => new("SwitchKnobWidth", true);
					
					[ResourceKeyDefinition(typeof(double), "KnobIconSize")]
					public static ResourceValue<double> IconSize => new("KnobIconSize", true);

					[ResourceKeyDefinition(typeof(Thickness), "KnobIconPadding")]
					public static ResourceValue<Thickness> IconPadding => new("KnobIconPadding", true);

					[ResourceKeyDefinition(typeof(Thickness), "KnobOnMargin")]
					public static ResourceValue<Thickness> OnMargin => new("KnobOnMargin", true);

					[ResourceKeyDefinition(typeof(double), "SwitchKnobHeight")]
					public static ResourceValue<double> Height => new("SwitchKnobHeight", true);

					[ResourceKeyDefinition(typeof(double), "SwitchKnobRadius")]
					public static ResourceValue<double> Radius => new("SwitchKnobRadius", true);

					[ResourceKeyDefinition(typeof(Thickness), "SwitchKnobOnMargin")]
					public static ResourceValue<Thickness> SwitchOnMargin => new("SwitchKnobOnMargin", true);

					[ResourceKeyDefinition(typeof(Thickness), "SwitchKnobOffMargin")]
					public static ResourceValue<Thickness> SwitchOffMargin => new("SwitchKnobOffMargin", true);
				}

				public static class IconPresenter
				{
					public static class Foreground
					{
						[ResourceKeyDefinition(typeof(Brush), "ToggleSwitchOffIconPresenterForeground")]
						public static ResourceValue<Brush> Off => new("ToggleSwitchOffIconPresenterForeground", true);

						[ResourceKeyDefinition(typeof(Brush), "ToggleSwitchOffIconPresenterForegroundDisabled")]
						public static ResourceValue<Brush> OffDisabled => new("ToggleSwitchOffIconPresenterForegroundDisabled", true);

						[ResourceKeyDefinition(typeof(Brush), "ToggleSwitchOnIconPresenterForeground")]
						public static ResourceValue<Brush> On => new("ToggleSwitchOnIconPresenterForeground", true);

						[ResourceKeyDefinition(typeof(Brush), "ToggleSwitchOnIconPresenterForegroundDisabled")]
						public static ResourceValue<Brush> OnDisabled => new("ToggleSwitchOnIconPresenterForegroundDisabled", true);
					}
				}

				public static class Thumb
				{
					public static class Size
					{
						[ResourceKeyDefinition(typeof(double), "SmallThumbSize")]
						public static ResourceValue<double> Small => new("SmallThumbSize", true);

						[ResourceKeyDefinition(typeof(double), "MediumThumbSize")]
						public static ResourceValue<double> Medium => new("MediumThumbSize", true);

						[ResourceKeyDefinition(typeof(double), "LargeThumbSize")]
						public static ResourceValue<double> Large => new("LargeThumbSize", true);
					}

					[ResourceKeyDefinition(typeof(Brush), "ToggleSwitchThumb")]
					public static ResourceValue<Brush> Default => new("ToggleSwitchThumb", true);

					[ResourceKeyDefinition(typeof(int), "LargeThumbCornerRadius")]
					public static ResourceValue<int> LargeCornerRadius => new("LargeThumbCornerRadius", true);
				}
			}

			public static class Styles
			{
				[ResourceKeyDefinition(typeof(Style), "ToggleSwitchStyle", TargetType = typeof(ToggleSwitch))]
				public static ResourceValue<Style> Default => new("ToggleSwitchStyle");
			}
		}
	}
}
