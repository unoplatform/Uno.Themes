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
	public static partial class ToggleSwitch
	{
		public static partial class Resources
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

				public static partial class OffFill
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

				public static partial class OffShadowFill
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

				public static partial class On
				{
					public static partial class Fill
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

					[ResourceKeyDefinition(typeof(Thickness), "KnobOnMargin")]
					public static ThemeResourceKey<Thickness> Margin => new("KnobOnMargin");
				}

				public static partial class OnShadowFill
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

				public static partial class OnSwitchBoundsFill
				{
					[ResourceKeyDefinition(typeof(Brush), "ToggleSwitchOnSwitchKnobBoundsFill")]
					public static ThemeResourceKey<Brush> Default => new("ToggleSwitchOnSwitchKnobBoundsFill");

					[ResourceKeyDefinition(typeof(Brush), "ToggleSwitchKnobBoundsFill")]
					public static ThemeResourceKey<Brush> Normal => new("ToggleSwitchKnobBoundsFill");

					[ResourceKeyDefinition(typeof(Brush), "ToggleSwitchKnobBoundsFillPointerOver")]
					public static ThemeResourceKey<Brush> PointerOver => new("ToggleSwitchKnobBoundsFillPointerOver");

					[ResourceKeyDefinition(typeof(Brush), "ToggleSwitchKnobBoundsFillFocused")]
					public static ThemeResourceKey<Brush> Focused => new("ToggleSwitchKnobBoundsFillFocused");

					[ResourceKeyDefinition(typeof(Brush), "ToggleSwitchKnobBoundsFillPressed")]
					public static ThemeResourceKey<Brush> Pressed => new("ToggleSwitchKnobBoundsFillPressed");

					[ResourceKeyDefinition(typeof(Brush), "ToggleSwitchKnobBoundsFillDisabled")]
					public static ThemeResourceKey<Brush> Disabled => new("ToggleSwitchKnobBoundsFillDisabled");
				}

				public static partial class Switch
				{
					[ResourceKeyDefinition(typeof(double), "SwitchKnobHeight")]
					public static ThemeResourceKey<double> Height => new("SwitchKnobHeight");

					[ResourceKeyDefinition(typeof(double), "SwitchKnobRadius")]
					public static ThemeResourceKey<double> Radius => new("SwitchKnobRadius");

					[ResourceKeyDefinition(typeof(double), "SwitchKnobStrokeThickness")]
					public static ThemeResourceKey<double> StrokeThickness => new("SwitchKnobStrokeThickness");

					[ResourceKeyDefinition(typeof(double), "SwitchKnobWidth")]
					public static ThemeResourceKey<double> Width => new("SwitchKnobWidth");
				}

				[ResourceKeyDefinition(typeof(double), "SwitchKnobIncludingOffShadowWidth")]
				public static ThemeResourceKey<double> SwitchIncludingOffShadowWidth => new("SwitchKnobIncludingOffShadowWidth");

				[ResourceKeyDefinition(typeof(Thickness), "SwitchKnobOffMargin")]
				public static ThemeResourceKey<Thickness> SwitchOffMargin => new("SwitchKnobOffMargin");

				[ResourceKeyDefinition(typeof(Thickness), "SwitchKnobOffShadowMargin")]
				public static ThemeResourceKey<Thickness> SwitchOffShadowMargin => new("SwitchKnobOffShadowMargin");

				[ResourceKeyDefinition(typeof(Thickness), "SwitchKnobOnMargin")]
				public static ThemeResourceKey<Thickness> SwitchOnMargin => new("SwitchKnobOnMargin");

				[ResourceKeyDefinition(typeof(Thickness), "SwitchKnobOnShadowMargin")]
				public static ThemeResourceKey<Thickness> SwitchOnShadowMargin => new("SwitchKnobOnShadowMargin");

				[ResourceKeyDefinition(typeof(double), "SwitchKnobShadowSize")]
				public static ThemeResourceKey<double> SwitchShadowSize => new("SwitchKnobShadowSize");
			}

			public static partial class OffIconPresenterForeground
			{
				[ResourceKeyDefinition(typeof(Brush), "ToggleSwitchOffIconPresenterForeground")]
				public static ThemeResourceKey<Brush> Default => new("ToggleSwitchOffIconPresenterForeground");

				[ResourceKeyDefinition(typeof(Brush), "ToggleSwitchOffIconPresenterForegroundDisabled")]
				public static ThemeResourceKey<Brush> Disabled => new("ToggleSwitchOffIconPresenterForegroundDisabled");
			}

			public static partial class OffOuterBorder
			{
				public static partial class Fill
				{
					[ResourceKeyDefinition(typeof(Brush), "ToggleSwitchOffOuterBorderFill")]
					public static ThemeResourceKey<Brush> Default => new("ToggleSwitchOffOuterBorderFill");

					[ResourceKeyDefinition(typeof(Brush), "ToggleSwitchOuterBorderFill")]
					public static ThemeResourceKey<Brush> Normal => new("ToggleSwitchOuterBorderFill");
				}

				public static partial class Stroke
				{
					[ResourceKeyDefinition(typeof(Brush), "ToggleSwitchOffOuterBorderStroke")]
					public static ThemeResourceKey<Brush> Default => new("ToggleSwitchOffOuterBorderStroke");

					[ResourceKeyDefinition(typeof(Brush), "ToggleSwitchOuterBorderStroke")]
					public static ThemeResourceKey<Brush> Normal => new("ToggleSwitchOuterBorderStroke");
				}
			}

			public static partial class OnIconPresenterForeground
			{
				[ResourceKeyDefinition(typeof(Brush), "ToggleSwitchOnIconPresenterForeground")]
				public static ThemeResourceKey<Brush> Default => new("ToggleSwitchOnIconPresenterForeground");

				[ResourceKeyDefinition(typeof(Brush), "ToggleSwitchOnIconPresenterForegroundDisabled")]
				public static ThemeResourceKey<Brush> Disabled => new("ToggleSwitchOnIconPresenterForegroundDisabled");
			}

			public static partial class Thumb
			{
				public static partial class MediumSize
				{
					[ResourceKeyDefinition(typeof(double), "MediumThumbSize")]
					public static ThemeResourceKey<double> Default => new("MediumThumbSize");

					[ResourceKeyDefinition(typeof(double), "LargeThumbSize")]
					public static ThemeResourceKey<double> Pressed => new("LargeThumbSize");
				}

				[ResourceKeyDefinition(typeof(Brush), "ToggleSwitchThumb")]
				public static ThemeResourceKey<Brush> Fill => new("ToggleSwitchThumb");

				[ResourceKeyDefinition(typeof(CornerRadius), "LargeThumbCornerRadius")]
				public static ThemeResourceKey<CornerRadius> LargeCornerRadius => new("LargeThumbCornerRadius");
			}

			[ResourceKeyDefinition(typeof(double), "ToggleSwitchThemeMinWidth")]
			public static ThemeResourceKey<double> ThemeMinWidth => new("ToggleSwitchThemeMinWidth");
		}

		public static partial class Styles
		{
			[ResourceKeyDefinition(typeof(Style), "MaterialToggleSwitchStyle", TargetType = typeof(global::Microsoft.UI.Xaml.Controls.ToggleSwitch))]
			public static StaticResourceKey<Style> Default => new("MaterialToggleSwitchStyle");
		}
	}
}
