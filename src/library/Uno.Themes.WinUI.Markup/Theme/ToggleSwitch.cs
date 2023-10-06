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

					public static readonly OffFillVSG OffFill = new();
					public partial class OffFillVSG
					{
						[ResourceKeyDefinition(typeof(Brush), "ToggleSwitchKnobOffFill")]
						public ThemeResourceKey<Brush> Default = new("ToggleSwitchKnobOffFill");

						[ResourceKeyDefinition(typeof(Brush), "ToggleSwitchKnobOffFillPointerOver")]
						public ThemeResourceKey<Brush> PointerOver = new("ToggleSwitchKnobOffFillPointerOver");

						[ResourceKeyDefinition(typeof(Brush), "ToggleSwitchKnobOffFillFocused")]
						public ThemeResourceKey<Brush> Focused = new("ToggleSwitchKnobOffFillFocused");

						[ResourceKeyDefinition(typeof(Brush), "ToggleSwitchKnobOffFillPressed")]
						public ThemeResourceKey<Brush> Pressed = new("ToggleSwitchKnobOffFillPressed");

						[ResourceKeyDefinition(typeof(Brush), "ToggleSwitchKnobOffFillDisabled")]
						public ThemeResourceKey<Brush> Disabled = new("ToggleSwitchKnobOffFillDisabled");

						public static implicit operator ThemeResourceKey<Brush>(OffFillVSG self) => self.Default;
					}

					public static readonly OffShadowFillVSG OffShadowFill = new();
					public partial class OffShadowFillVSG
					{
						[ResourceKeyDefinition(typeof(Brush), "ToggleSwitchKnobOffShadowFill")]
						public ThemeResourceKey<Brush> Default = new("ToggleSwitchKnobOffShadowFill");

						[ResourceKeyDefinition(typeof(Brush), "ToggleSwitchKnobOffShadowFillPointerOver")]
						public ThemeResourceKey<Brush> PointerOver = new("ToggleSwitchKnobOffShadowFillPointerOver");

						[ResourceKeyDefinition(typeof(Brush), "ToggleSwitchKnobOffShadowFillFocused")]
						public ThemeResourceKey<Brush> Focused = new("ToggleSwitchKnobOffShadowFillFocused");

						[ResourceKeyDefinition(typeof(Brush), "ToggleSwitchKnobOffShadowFillPressed")]
						public ThemeResourceKey<Brush> Pressed = new("ToggleSwitchKnobOffShadowFillPressed");

						public static implicit operator ThemeResourceKey<Brush>(OffShadowFillVSG self) => self.Default;
					}

					public static partial class On
					{
						public static readonly FillVSG Fill = new();
						public partial class FillVSG
						{
							[ResourceKeyDefinition(typeof(Brush), "ToggleSwitchKnobOnFill")]
							public ThemeResourceKey<Brush> Default = new("ToggleSwitchKnobOnFill");

							[ResourceKeyDefinition(typeof(Brush), "ToggleSwitchKnobOnFillPointerOver")]
							public ThemeResourceKey<Brush> PointerOver = new("ToggleSwitchKnobOnFillPointerOver");

							[ResourceKeyDefinition(typeof(Brush), "ToggleSwitchKnobOnFillFocused")]
							public ThemeResourceKey<Brush> Focused = new("ToggleSwitchKnobOnFillFocused");

							[ResourceKeyDefinition(typeof(Brush), "ToggleSwitchKnobOnFillPressed")]
							public ThemeResourceKey<Brush> Pressed = new("ToggleSwitchKnobOnFillPressed");

							[ResourceKeyDefinition(typeof(Brush), "ToggleSwitchKnobOnFillDisabled")]
							public ThemeResourceKey<Brush> Disabled = new("ToggleSwitchKnobOnFillDisabled");

							public static implicit operator ThemeResourceKey<Brush>(FillVSG self) => self.Default;
						}

						[ResourceKeyDefinition(typeof(Thickness), "KnobOnMargin")]
						public static ThemeResourceKey<Thickness> Margin => new("KnobOnMargin");
					}

					public static readonly OnShadowFillVSG OnShadowFill = new();
					public partial class OnShadowFillVSG
					{
						[ResourceKeyDefinition(typeof(Brush), "ToggleSwitchKnobOnShadowFill")]
						public ThemeResourceKey<Brush> Default = new("ToggleSwitchKnobOnShadowFill");

						[ResourceKeyDefinition(typeof(Brush), "ToggleSwitchKnobOnShadowFillPointerOver")]
						public ThemeResourceKey<Brush> PointerOver = new("ToggleSwitchKnobOnShadowFillPointerOver");

						[ResourceKeyDefinition(typeof(Brush), "ToggleSwitchKnobOnShadowFillFocused")]
						public ThemeResourceKey<Brush> Focused = new("ToggleSwitchKnobOnShadowFillFocused");

						[ResourceKeyDefinition(typeof(Brush), "ToggleSwitchKnobOnShadowFillPressed")]
						public ThemeResourceKey<Brush> Pressed = new("ToggleSwitchKnobOnShadowFillPressed");

						public static implicit operator ThemeResourceKey<Brush>(OnShadowFillVSG self) => self.Default;
					}

					public static readonly OnSwitchBoundsFillVSG OnSwitchBoundsFill = new();
					public partial class OnSwitchBoundsFillVSG
					{
						[ResourceKeyDefinition(typeof(Brush), "ToggleSwitchOnSwitchKnobBoundsFill")]
						public ThemeResourceKey<Brush> Default = new("ToggleSwitchOnSwitchKnobBoundsFill");

						[ResourceKeyDefinition(typeof(Brush), "ToggleSwitchKnobBoundsFill")]
						public ThemeResourceKey<Brush> Normal = new("ToggleSwitchKnobBoundsFill");

						[ResourceKeyDefinition(typeof(Brush), "ToggleSwitchKnobBoundsFillPointerOver")]
						public ThemeResourceKey<Brush> PointerOver = new("ToggleSwitchKnobBoundsFillPointerOver");

						[ResourceKeyDefinition(typeof(Brush), "ToggleSwitchKnobBoundsFillFocused")]
						public ThemeResourceKey<Brush> Focused = new("ToggleSwitchKnobBoundsFillFocused");

						[ResourceKeyDefinition(typeof(Brush), "ToggleSwitchKnobBoundsFillPressed")]
						public ThemeResourceKey<Brush> Pressed = new("ToggleSwitchKnobBoundsFillPressed");

						[ResourceKeyDefinition(typeof(Brush), "ToggleSwitchKnobBoundsFillDisabled")]
						public ThemeResourceKey<Brush> Disabled = new("ToggleSwitchKnobBoundsFillDisabled");

						public static implicit operator ThemeResourceKey<Brush>(OnSwitchBoundsFillVSG self) => self.Default;
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

				public static readonly OffIconPresenterForegroundVSG OffIconPresenterForeground = new();
				public partial class OffIconPresenterForegroundVSG
				{
					[ResourceKeyDefinition(typeof(Brush), "ToggleSwitchOffIconPresenterForeground")]
					public ThemeResourceKey<Brush> Default = new("ToggleSwitchOffIconPresenterForeground");

					[ResourceKeyDefinition(typeof(Brush), "ToggleSwitchOffIconPresenterForegroundDisabled")]
					public ThemeResourceKey<Brush> Disabled = new("ToggleSwitchOffIconPresenterForegroundDisabled");

					public static implicit operator ThemeResourceKey<Brush>(OffIconPresenterForegroundVSG self) => self.Default;
				}

				public static partial class OffOuterBorder
				{
					public static readonly FillVSG Fill = new();
					public partial class FillVSG
					{
						[ResourceKeyDefinition(typeof(Brush), "ToggleSwitchOffOuterBorderFill")]
						public ThemeResourceKey<Brush> Default = new("ToggleSwitchOffOuterBorderFill");

						[ResourceKeyDefinition(typeof(Brush), "ToggleSwitchOuterBorderFill")]
						public ThemeResourceKey<Brush> Normal = new("ToggleSwitchOuterBorderFill");

						public static implicit operator ThemeResourceKey<Brush>(FillVSG self) => self.Default;
					}

					public static readonly StrokeVSG Stroke = new();
					public partial class StrokeVSG
					{
						[ResourceKeyDefinition(typeof(Brush), "ToggleSwitchOffOuterBorderStroke")]
						public ThemeResourceKey<Brush> Default = new("ToggleSwitchOffOuterBorderStroke");

						[ResourceKeyDefinition(typeof(Brush), "ToggleSwitchOuterBorderStroke")]
						public ThemeResourceKey<Brush> Normal = new("ToggleSwitchOuterBorderStroke");

						public static implicit operator ThemeResourceKey<Brush>(StrokeVSG self) => self.Default;
					}
				}

				public static readonly OnIconPresenterForegroundVSG OnIconPresenterForeground = new();
				public partial class OnIconPresenterForegroundVSG
				{
					[ResourceKeyDefinition(typeof(Brush), "ToggleSwitchOnIconPresenterForeground")]
					public ThemeResourceKey<Brush> Default = new("ToggleSwitchOnIconPresenterForeground");

					[ResourceKeyDefinition(typeof(Brush), "ToggleSwitchOnIconPresenterForegroundDisabled")]
					public ThemeResourceKey<Brush> Disabled = new("ToggleSwitchOnIconPresenterForegroundDisabled");

					public static implicit operator ThemeResourceKey<Brush>(OnIconPresenterForegroundVSG self) => self.Default;
				}

				public static partial class Thumb
				{
					public static readonly MediumSizeVSG MediumSize = new();
					public partial class MediumSizeVSG
					{
						[ResourceKeyDefinition(typeof(double), "MediumThumbSize")]
						public ThemeResourceKey<double> Default = new("MediumThumbSize");

						[ResourceKeyDefinition(typeof(double), "LargeThumbSize")]
						public ThemeResourceKey<double> Pressed = new("LargeThumbSize");

						public static implicit operator ThemeResourceKey<double>(MediumSizeVSG self) => self.Default;
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
}
