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
	public static partial class RadioButton
	{
		public static partial class Resources
		{
			public static partial class Default
			{
				public static partial class Foreground
				{
					[ResourceKeyDefinition(typeof(Brush), "RadioButtonForeground")]
					public static ThemeResourceKey<Brush> Default => new("RadioButtonForeground");

					[ResourceKeyDefinition(typeof(Brush), "RadioButtonForegroundPointerOver")]
					public static ThemeResourceKey<Brush> PointerOver => new("RadioButtonForegroundPointerOver");

					[ResourceKeyDefinition(typeof(Brush), "RadioButtonForegroundPressed")]
					public static ThemeResourceKey<Brush> Pressed => new("RadioButtonForegroundPressed");

					[ResourceKeyDefinition(typeof(Brush), "RadioButtonForegroundDisabled")]
					public static ThemeResourceKey<Brush> Disabled => new("RadioButtonForegroundDisabled");
				}

				public static partial class OuterEllipse
				{
					public static partial class Fill
					{
						[ResourceKeyDefinition(typeof(Brush), "RadioButtonOuterEllipseFill")]
						public static ThemeResourceKey<Brush> Default => new("RadioButtonOuterEllipseFill");

						[ResourceKeyDefinition(typeof(Brush), "RadioButtonOuterEllipseFillPointerOver")]
						public static ThemeResourceKey<Brush> PointerOver => new("RadioButtonOuterEllipseFillPointerOver");

						[ResourceKeyDefinition(typeof(Brush), "RadioButtonOuterEllipseFillPressed")]
						public static ThemeResourceKey<Brush> Pressed => new("RadioButtonOuterEllipseFillPressed");

						[ResourceKeyDefinition(typeof(Brush), "RadioButtonOuterEllipseFillDisabled")]
						public static ThemeResourceKey<Brush> Disabled => new("RadioButtonOuterEllipseFillDisabled");

						[ResourceKeyDefinition(typeof(Brush), "RadioButtonOuterEllipseCheckedFill")]
						public static ThemeResourceKey<Brush> Checked => new("RadioButtonOuterEllipseCheckedFill");

						[ResourceKeyDefinition(typeof(Brush), "RadioButtonOuterEllipseCheckedFillPointerOver")]
						public static ThemeResourceKey<Brush> CheckedPointerOver => new("RadioButtonOuterEllipseCheckedFillPointerOver");

						[ResourceKeyDefinition(typeof(Brush), "RadioButtonOuterEllipseCheckedFillPressed")]
						public static ThemeResourceKey<Brush> CheckedPressed => new("RadioButtonOuterEllipseCheckedFillPressed");

						[ResourceKeyDefinition(typeof(Brush), "RadioButtonOuterEllipseCheckedFillDisabled")]
						public static ThemeResourceKey<Brush> CheckedDisabled => new("RadioButtonOuterEllipseCheckedFillDisabled");
					}

					public static partial class Stroke
					{
						[ResourceKeyDefinition(typeof(Brush), "RadioButtonOuterEllipseStroke")]
						public static ThemeResourceKey<Brush> Default => new("RadioButtonOuterEllipseStroke");

						[ResourceKeyDefinition(typeof(Brush), "RadioButtonOuterEllipseStrokePointerOver")]
						public static ThemeResourceKey<Brush> PointerOver => new("RadioButtonOuterEllipseStrokePointerOver");

						[ResourceKeyDefinition(typeof(Brush), "RadioButtonOuterEllipseStrokePressed")]
						public static ThemeResourceKey<Brush> Pressed => new("RadioButtonOuterEllipseStrokePressed");

						[ResourceKeyDefinition(typeof(Brush), "RadioButtonOuterEllipseStrokeDisabled")]
						public static ThemeResourceKey<Brush> Disabled => new("RadioButtonOuterEllipseStrokeDisabled");

						[ResourceKeyDefinition(typeof(Brush), "RadioButtonOuterEllipseCheckedStroke")]
						public static ThemeResourceKey<Brush> Checked => new("RadioButtonOuterEllipseCheckedStroke");

						[ResourceKeyDefinition(typeof(Brush), "RadioButtonOuterEllipseCheckedStrokePointerOver")]
						public static ThemeResourceKey<Brush> CheckedPointerOver => new("RadioButtonOuterEllipseCheckedStrokePointerOver");

						[ResourceKeyDefinition(typeof(Brush), "RadioButtonOuterEllipseCheckedStrokePressed")]
						public static ThemeResourceKey<Brush> CheckedPressed => new("RadioButtonOuterEllipseCheckedStrokePressed");

						[ResourceKeyDefinition(typeof(Brush), "RadioButtonOuterEllipseCheckedStrokeDisabled")]
						public static ThemeResourceKey<Brush> CheckedDisabled => new("RadioButtonOuterEllipseCheckedStrokeDisabled");
					}
				}

				public static partial class StateCircle
				{
					public static partial class Background
					{
						[ResourceKeyDefinition(typeof(Brush), "RadioButtonStateCircleBackgroundChecked")]
						public static ThemeResourceKey<Brush> Checked => new("RadioButtonStateCircleBackgroundChecked");

						[ResourceKeyDefinition(typeof(Brush), "RadioButtonStateCircleBackgroundUnchecked")]
						public static ThemeResourceKey<Brush> Unchecked => new("RadioButtonStateCircleBackgroundUnchecked");
					}

					public static partial class Opacity
					{
						[ResourceKeyDefinition(typeof(double), "RadioButtonStateCircleOpacityPointerOver")]
						public static ThemeResourceKey<double> PointerOver => new("RadioButtonStateCircleOpacityPointerOver");

						[ResourceKeyDefinition(typeof(double), "RadioButtonStateCircleOpacityPressed")]
						public static ThemeResourceKey<double> Pressed => new("RadioButtonStateCircleOpacityPressed");

						[ResourceKeyDefinition(typeof(double), "RadioButtonStateCircleOpacityFocused")]
						public static ThemeResourceKey<double> Focused => new("RadioButtonStateCircleOpacityFocused");
					}
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

				[ResourceKeyDefinition(typeof(Thickness), "RadioButtonPadding")]
				public static ThemeResourceKey<Thickness> Padding => new("RadioButtonPadding");

				[ResourceKeyDefinition(typeof(double), "RadioButtonStrokeThickness")]
				public static ThemeResourceKey<double> StrokeThickness => new("RadioButtonStrokeThickness");

				[ResourceKeyDefinition(typeof(double), "RadioButtonWidth")]
				public static ThemeResourceKey<double> Width => new("RadioButtonWidth");
			}
		}

		public static partial class Styles
		{
			[ResourceKeyDefinition(typeof(Style), "RadioButtonStyle", TargetType = typeof(global::Microsoft.UI.Xaml.Controls.RadioButton))]
			public static StaticResourceKey<Style> Default => new("RadioButtonStyle");
		}
	}
}
