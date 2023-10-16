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
		public static partial class RadioButton
		{
			public static partial class Resources
			{
				public static readonly ForegroundVSG Foreground = new();
				public partial class ForegroundVSG
				{
					[ResourceKeyDefinition(typeof(Brush), "RadioButtonForeground")]
					public ThemeResourceKey<Brush> Default = new("RadioButtonForeground");

					[ResourceKeyDefinition(typeof(Brush), "RadioButtonForegroundPointerOver")]
					public ThemeResourceKey<Brush> PointerOver = new("RadioButtonForegroundPointerOver");

					[ResourceKeyDefinition(typeof(Brush), "RadioButtonForegroundPressed")]
					public ThemeResourceKey<Brush> Pressed = new("RadioButtonForegroundPressed");

					[ResourceKeyDefinition(typeof(Brush), "RadioButtonForegroundDisabled")]
					public ThemeResourceKey<Brush> Disabled = new("RadioButtonForegroundDisabled");

					public static implicit operator ThemeResourceKey<Brush>(ForegroundVSG self) => self.Default;
				}

				public static partial class OuterEllipse
				{
					public static readonly FillVSG Fill = new();
					public partial class FillVSG
					{
						[ResourceKeyDefinition(typeof(Brush), "RadioButtonOuterEllipseFill")]
						public ThemeResourceKey<Brush> Normal = new("RadioButtonOuterEllipseFill");

						[ResourceKeyDefinition(typeof(Brush), "RadioButtonOuterEllipseFillPointerOver")]
						public ThemeResourceKey<Brush> PointerOver = new("RadioButtonOuterEllipseFillPointerOver");

						[ResourceKeyDefinition(typeof(Brush), "RadioButtonOuterEllipseFillPressed")]
						public ThemeResourceKey<Brush> Pressed = new("RadioButtonOuterEllipseFillPressed");

						[ResourceKeyDefinition(typeof(Brush), "RadioButtonOuterEllipseFillDisabled")]
						public ThemeResourceKey<Brush> Disabled = new("RadioButtonOuterEllipseFillDisabled");

						public static implicit operator ThemeResourceKey<Brush>(FillVSG self) => self.Normal;
					}

					public static readonly StrokeVSG Stroke = new();
					public partial class StrokeVSG
					{
						[ResourceKeyDefinition(typeof(Brush), "RadioButtonOuterEllipseStroke")]
						public ThemeResourceKey<Brush> Normal = new("RadioButtonOuterEllipseStroke");

						[ResourceKeyDefinition(typeof(Brush), "RadioButtonOuterEllipseStrokePointerOver")]
						public ThemeResourceKey<Brush> PointerOver = new("RadioButtonOuterEllipseStrokePointerOver");

						[ResourceKeyDefinition(typeof(Brush), "RadioButtonOuterEllipseStrokePressed")]
						public ThemeResourceKey<Brush> Pressed = new("RadioButtonOuterEllipseStrokePressed");

						[ResourceKeyDefinition(typeof(Brush), "RadioButtonOuterEllipseStrokeDisabled")]
						public ThemeResourceKey<Brush> Disabled = new("RadioButtonOuterEllipseStrokeDisabled");

						public static implicit operator ThemeResourceKey<Brush>(StrokeVSG self) => self.Normal;
					}
				}

				public static readonly OuterEllipseCheckedFillVSG OuterEllipseCheckedFill = new();
				public partial class OuterEllipseCheckedFillVSG
				{
					[ResourceKeyDefinition(typeof(Brush), "RadioButtonOuterEllipseCheckedFill")]
					public ThemeResourceKey<Brush> Normal = new("RadioButtonOuterEllipseCheckedFill");

					[ResourceKeyDefinition(typeof(Brush), "RadioButtonOuterEllipseCheckedFillPointerOver")]
					public ThemeResourceKey<Brush> PointerOver = new("RadioButtonOuterEllipseCheckedFillPointerOver");

					[ResourceKeyDefinition(typeof(Brush), "RadioButtonOuterEllipseCheckedFillPressed")]
					public ThemeResourceKey<Brush> Pressed = new("RadioButtonOuterEllipseCheckedFillPressed");

					[ResourceKeyDefinition(typeof(Brush), "RadioButtonOuterEllipseCheckedFillDisabled")]
					public ThemeResourceKey<Brush> Disabled = new("RadioButtonOuterEllipseCheckedFillDisabled");

					public static implicit operator ThemeResourceKey<Brush>(OuterEllipseCheckedFillVSG self) => self.Normal;
				}

				public static partial class StateCircle
				{
					public static partial class Opacity
					{
						[ResourceKeyDefinition(typeof(double), "RadioButtonStateCircleOpacityPointerOver")]
						public static ThemeResourceKey<double> PointerOver => new("RadioButtonStateCircleOpacityPointerOver");

						[ResourceKeyDefinition(typeof(double), "RadioButtonStateCircleOpacityPressed")]
						public static ThemeResourceKey<double> Pressed => new("RadioButtonStateCircleOpacityPressed");
					}

					[ResourceKeyDefinition(typeof(double), "RadioButtonStateCircleOpacityFocused")]
					public static ThemeResourceKey<double> OpacityFocused => new("RadioButtonStateCircleOpacityFocused");
				}

				public static partial class Typography
				{
					[ResourceKeyDefinition(typeof(int), "RadioButtonCharacterSpacing")]
					public static ThemeResourceKey<int> CharacterSpacing => new("RadioButtonCharacterSpacing");

					[ResourceKeyDefinition(typeof(FontFamily), "RadioButtonFontFamily")]
					public static ThemeResourceKey<FontFamily> FontFamily => new("RadioButtonFontFamily");

					[ResourceKeyDefinition(typeof(double), "RadioButtonFontSize")]
					public static ThemeResourceKey<double> FontSize => new("RadioButtonFontSize");

					[ResourceKeyDefinition(typeof(string), "RadioButtonFontWeight")]
					public static ThemeResourceKey<string> FontWeight => new("RadioButtonFontWeight");
				}

				[ResourceKeyDefinition(typeof(Thickness), "RadioButtonCheckEllipsePadding")]
				public static ThemeResourceKey<Thickness> CheckEllipsePadding => new("RadioButtonCheckEllipsePadding");

				[ResourceKeyDefinition(typeof(double), "RadioButtonCheckGlyphSize")]
				public static ThemeResourceKey<double> CheckGlyphSize => new("RadioButtonCheckGlyphSize");

				[ResourceKeyDefinition(typeof(double), "RadioButtonHeight")]
				public static ThemeResourceKey<double> Height => new("RadioButtonHeight");

				[ResourceKeyDefinition(typeof(double), "RadioButtonMinHeight")]
				public static ThemeResourceKey<double> MinHeight => new("RadioButtonMinHeight");

				[ResourceKeyDefinition(typeof(double), "RadioButtonMinWidth")]
				public static ThemeResourceKey<double> MinWidth => new("RadioButtonMinWidth");

				[ResourceKeyDefinition(typeof(Brush), "RadioButtonOuterEllipseCheckedStroke")]
				public static ThemeResourceKey<Brush> OuterEllipseCheckedStroke => new("RadioButtonOuterEllipseCheckedStroke");

				[ResourceKeyDefinition(typeof(Thickness), "RadioButtonPadding")]
				public static ThemeResourceKey<Thickness> Padding => new("RadioButtonPadding");

				[ResourceKeyDefinition(typeof(Brush), "RadioButtonStateCircleBackgroundChecked")]
				public static ThemeResourceKey<Brush> StateCircleBackgroundChecked => new("RadioButtonStateCircleBackgroundChecked");

				[ResourceKeyDefinition(typeof(Brush), "RadioButtonStateCircleBackgroundUnchecked")]
				public static ThemeResourceKey<Brush> StateCircleBackgroundUnchecked => new("RadioButtonStateCircleBackgroundUnchecked");

				[ResourceKeyDefinition(typeof(double), "RadioButtonStrokeThickness")]
				public static ThemeResourceKey<double> StrokeThickness => new("RadioButtonStrokeThickness");

				[ResourceKeyDefinition(typeof(double), "RadioButtonWidth")]
				public static ThemeResourceKey<double> Width => new("RadioButtonWidth");
			}

			public static partial class Styles
			{
				[ResourceKeyDefinition(typeof(Style), "MaterialRadioButtonStyle", TargetType = typeof(global::Microsoft.UI.Xaml.Controls.RadioButton))]
				public static StaticResourceKey<Style> Default => new("MaterialRadioButtonStyle");
			}
		}
	}
}
