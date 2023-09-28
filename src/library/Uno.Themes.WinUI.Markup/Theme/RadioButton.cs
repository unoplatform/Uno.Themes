using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Media;
using Uno.Extensions.Markup;
using Uno.Extensions.Markup.Internals;
using Microsoft.UI.Text;

namespace Uno.Themes.Markup
{
	public static partial class Theme
	{
		public static class RadioButton
		{
			public static class Resources
			{
				public static class Default
				{
					public static class Foreground
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

					public class Stroke
					{
						public static implicit operator ThemeResourceKey<Brush>(Stroke @class) => @class.Default;

						public Stroke()
						{
							Theme.RadioButton.Resources.Default.Stroke
						}

						[ResourceKeyDefinition(typeof(Brush), "RadioButtonOuterEllipseStroke")]
						public ThemeResourceKey<Brush> Default => new("RadioButtonOuterEllipseStroke");

						[ResourceKeyDefinition(typeof(Brush), "RadioButtonOuterEllipseStrokePointerOver")]
						public ThemeResourceKey<Brush> PointerOver => new("RadioButtonOuterEllipseStrokePointerOver")

						// etc.
					}

					public static class Fill
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

					public static class HoverRingFill
					{
						[ResourceKeyDefinition(typeof(Brush), "MaterialRadioButtonHoverRingFillPointerOver")]
						public static ThemeResourceKey<Brush> PointerOver => new("MaterialRadioButtonHoverRingFillPointerOver");

						[ResourceKeyDefinition(typeof(Brush), "MaterialRadioButtonHoverRingFillPressed")]
						public static ThemeResourceKey<Brush> Pressed => new("MaterialRadioButtonHoverRingFillPressed");
					}

					public static class Typography
					{
						[ResourceKeyDefinition(typeof(FontFamily), "RadioButtonFontFamily")]
						public static StaticResourceKey<FontFamily> FontFamily => new("RadioButtonFontFamily");

						[ResourceKeyDefinition(typeof(FontWeights), "RadioButtonFontWeight")]
						public static StaticResourceKey<FontWeights> FontWeight => new("RadioButtonFontWeight");

						[ResourceKeyDefinition(typeof(double), "RadioButtonFontSize")]
						public static StaticResourceKey<double> FontSize => new("RadioButtonFontSize");

						[ResourceKeyDefinition(typeof(int), "RadioButtonCharacterSpacing")]
						public static StaticResourceKey<int> CharacterSpacing => new("RadioButtonCharacterSpacing");
					}
				}
			}

			public static class Styles
			{
				[ResourceKeyDefinition(typeof(Style), "RadioButtonStyle", TargetType = typeof(RadioButton))]
				public static StaticResourceKey<Style> Default => new("RadioButtonStyle");

				[ResourceKeyDefinition(typeof(Style), "SecondaryRadioButtonStyle", TargetType = typeof(RadioButton))]
				public static StaticResourceKey<Style> Secondary => new("SecondaryRadioButtonStyle");
			}
		}
	}
}
