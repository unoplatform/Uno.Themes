using System;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Media;
using Uno.Extensions.Markup;
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
						public static ThemeResourceKey<Brush> Default => new("ToggleSwitchOuterBorderFill");
					}

					public static class OffFill
					{
						[ResourceKeyDefinition(typeof(Brush), "ToggleSwitchOffOuterBorderFill")]
						public static ThemeResourceKey<Brush> Default => new("ToggleSwitchOffOuterBorderFill");
					}

					public static class Stroke
					{
						[ResourceKeyDefinition(typeof(Brush), "ToggleSwitchOuterBorderStroke")]
						public static ThemeResourceKey<Brush> Default => new("ToggleSwitchOuterBorderStroke");
					}

					public static class OffStroke
					{
						[ResourceKeyDefinition(typeof(Brush), "ToggleSwitchOffOuterBorderStroke")]
						public static ThemeResourceKey<Brush> Default => new("ToggleSwitchOffOuterBorderStroke");
					}
				}

				public static class Knob
				{
					public static class OnFill
					{
						[ResourceKeyDefinition(typeof(Brush), "ToggleSwitchKnobOnFill")]
						public static ThemeResourceKey<Brush> Default => new("ToggleSwitchKnobOnFill");

						[ResourceKeyDefinition(typeof(Brush), "ToggleSwitchKnobOnFillPointerOver")]
						public static ThemeResourceKey<Brush> PointerOver => new("ToggleSwitchKnobOnFillPointerOver");

						[ResourceKeyDefinition(typeof(Brush), "ToggleSwitchKnobOnFillFocused")]
						public static ThemeResourceKey<Brush> Focused => new("ToggleSwitchKnobOnFillFocused");

						[ResourceKeyDefinition(typeof(Brush), "ToggleSwitchKnobOnFillPressed")]
						public static ThemeResourceKey<Brush> Pressed => new("ToggleSwitchKnobOnFillPressed");

						[ResourceKeyDefinition(typeof(Brush), "ToggleSwitchKnobOnFillDisabled")]
						public static ThemeResourceKey<Brush> Disabled => new("ToggleSwitchKnobOnFillDisabled");
					}

					public static class OffFill
					{
						[ResourceKeyDefinition(typeof(Brush), "ToggleSwitchKnobOffFill")]
						public static ThemeResourceKey<Brush> Default => new("ToggleSwitchKnobOffFill");

						[ResourceKeyDefinition(typeof(Brush), "ToggleSwitchKnobOffFillPointerOver")]
						public static ThemeResourceKey<Brush> PointerOver => new("ToggleSwitchKnobOffFillPointerOver");

						[ResourceKeyDefinition(typeof(Brush), "ToggleSwitchKnobOffFillFocused")]
						public static ThemeResourceKey<Brush> Focused => new("ToggleSwitchKnobOffFillFocused");

						[ResourceKeyDefinition(typeof(Brush), "ToggleSwitchKnobOffFillPressed")]
						public static ThemeResourceKey<Brush> Pressed => new("ToggleSwitchKnobOffFillPressed");

						[ResourceKeyDefinition(typeof(Brush), "ToggleSwitchKnobOffFillDisabled")]
						public static ThemeResourceKey<Brush> Disabled => new("ToggleSwitchKnobOffFillDisabled");
					}

					public static class Shadow
					{
						public static class OnFill
						{
							[ResourceKeyDefinition(typeof(Brush), "ToggleSwitchKnobOnShadowFill")]
							public static ThemeResourceKey<Brush> Default => new("ToggleSwitchKnobOnShadowFill");

							[ResourceKeyDefinition(typeof(Brush), "ToggleSwitchKnobOnShadowFillPointerOver")]
							public static ThemeResourceKey<Brush> PointerOver => new("ToggleSwitchKnobOnShadowFillPointerOver");

							[ResourceKeyDefinition(typeof(Brush), "ToggleSwitchKnobOnShadowFillFocused")]
							public static ThemeResourceKey<Brush> Focused => new("ToggleSwitchKnobOnShadowFillFocused");

							[ResourceKeyDefinition(typeof(Brush), "ToggleSwitchKnobOnShadowFillPressed")]
							public static ThemeResourceKey<Brush> Pressed => new("ToggleSwitchKnobOnShadowFillPressed");
						}

						public static class OffFill
						{
							[ResourceKeyDefinition(typeof(Brush), "ToggleSwitchKnobOffShadowFill")]
							public static ThemeResourceKey<Brush> Default => new("ToggleSwitchKnobOffShadowFill");

							[ResourceKeyDefinition(typeof(Brush), "ToggleSwitchKnobOffShadowFillPointerOver")]
							public static ThemeResourceKey<Brush> PointerOver => new("ToggleSwitchKnobOffShadowFillPointerOver");

							[ResourceKeyDefinition(typeof(Brush), "ToggleSwitchKnobOffShadowFillFocused")]
							public static ThemeResourceKey<Brush> Focused => new("ToggleSwitchKnobOffShadowFillFocused");

							[ResourceKeyDefinition(typeof(Brush), "ToggleSwitchKnobOffShadowFillPressed")]
							public static ThemeResourceKey<Brush> Pressed => new("ToggleSwitchKnobOffShadowFillPressed");
						}
						

						[ResourceKeyDefinition(typeof(double), "SwitchKnobShadowSize")]
						public static ThemeResourceKey<double> Size => new("SwitchKnobShadowSize");

						[ResourceKeyDefinition(typeof(Thickness), "SwitchKnobOnShadowMargin")]
						public static ThemeResourceKey<Thickness> OnMargin => new("SwitchKnobOnShadowMargin");

						[ResourceKeyDefinition(typeof(Thickness), "SwitchKnobOffShadowMargin")]
						public static ThemeResourceKey<Thickness> OffMargin => new("SwitchKnobOffShadowMargin");

						[ResourceKeyDefinition(typeof(double), "SwitchKnobIncludingOffShadowWidth")]
						public static ThemeResourceKey<double> IncludingOffShadowWidth => new("SwitchKnobIncludingOffShadowWidth");
					}


					public static class BoundsFill
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
					}

					public static class OnBoundsFill
					{
						[ResourceKeyDefinition(typeof(Brush), "ToggleSwitchOnSwitchKnobBoundsFill")]
						public static ThemeResourceKey<Brush> Default => new("ToggleSwitchOnSwitchKnobBoundsFill");
					}
			
					[ResourceKeyDefinition(typeof(Thickness), "SwitchKnobStrokeThickness")]
					public static ThemeResourceKey<Thickness> StrokeThickness => new("SwitchKnobStrokeThickness");

					[ResourceKeyDefinition(typeof(double), "SwitchKnobWidth")]
					public static ThemeResourceKey<double> Width => new("SwitchKnobWidth");
					
					[ResourceKeyDefinition(typeof(double), "KnobIconSize")]
					public static ThemeResourceKey<double> IconSize => new("KnobIconSize");

					[ResourceKeyDefinition(typeof(Thickness), "KnobIconPadding")]
					public static ThemeResourceKey<Thickness> IconPadding => new("KnobIconPadding");

					[ResourceKeyDefinition(typeof(Thickness), "KnobOnMargin")]
					public static ThemeResourceKey<Thickness> OnMargin => new("KnobOnMargin");

					[ResourceKeyDefinition(typeof(double), "SwitchKnobHeight")]
					public static ThemeResourceKey<double> Height => new("SwitchKnobHeight");

					[ResourceKeyDefinition(typeof(double), "SwitchKnobRadius")]
					public static ThemeResourceKey<double> Radius => new("SwitchKnobRadius");

					[ResourceKeyDefinition(typeof(Thickness), "SwitchKnobOnMargin")]
					public static ThemeResourceKey<Thickness> SwitchOnMargin => new("SwitchKnobOnMargin");

					[ResourceKeyDefinition(typeof(Thickness), "SwitchKnobOffMargin")]
					public static ThemeResourceKey<Thickness> SwitchOffMargin => new("SwitchKnobOffMargin");
				}

				public static class IconPresenter
				{
					public static class OnForeground
					{

						[ResourceKeyDefinition(typeof(Brush), "ToggleSwitchOnIconPresenterForeground")]
						public static ThemeResourceKey<Brush> Default => new("ToggleSwitchOnIconPresenterForeground");

						[ResourceKeyDefinition(typeof(Brush), "ToggleSwitchOnIconPresenterForegroundDisabled")]
						public static ThemeResourceKey<Brush> Disabled => new("ToggleSwitchOnIconPresenterForegroundDisabled");
					}

					public static class OffForeground
					{
						[ResourceKeyDefinition(typeof(Brush), "ToggleSwitchOffIconPresenterForeground")]
						public static ThemeResourceKey<Brush> Default => new("ToggleSwitchOffIconPresenterForeground");

						[ResourceKeyDefinition(typeof(Brush), "ToggleSwitchOffIconPresenterForegroundDisabled")]
						public static ThemeResourceKey<Brush> Disabled => new("ToggleSwitchOffIconPresenterForegroundDisabled");
					}
				}

				public static class Thumb
				{
					
					[ResourceKeyDefinition(typeof(double), "SmallThumbSize")]
					public static ThemeResourceKey<double> SmallSize => new("SmallThumbSize");

					[ResourceKeyDefinition(typeof(double), "MediumThumbSize")]
					public static ThemeResourceKey<double> MediumSize => new("MediumThumbSize");

					[ResourceKeyDefinition(typeof(double), "LargeThumbSize")]
					public static ThemeResourceKey<double> LargeSize => new("LargeThumbSize");

					public static class Fill
					{
						[ResourceKeyDefinition(typeof(Brush), "ToggleSwitchThumb")]
						public static ThemeResourceKey<Brush> Default => new("ToggleSwitchThumb");
					}

					[ResourceKeyDefinition(typeof(int), "LargeThumbCornerRadius")]
					public static ThemeResourceKey<int> LargeCornerRadius => new("LargeThumbCornerRadius");
				}
			}

			public static class Styles
			{
				[ResourceKeyDefinition(typeof(Style), "ToggleSwitchStyle", TargetType = typeof(ToggleSwitch))]
				public static StaticResourceKey<Style> Default => new("ToggleSwitchStyle");
			}
		}
	}
}
