using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Media;
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
						public static ResourceValue<Brush> Default => new("RadioButtonForeground", true);

						[ResourceKeyDefinition(typeof(Brush), "RadioButtonForegroundPointerOver")]
						public static ResourceValue<Brush> PointerOver => new("RadioButtonForegroundPointerOver", true);

						[ResourceKeyDefinition(typeof(Brush), "RadioButtonForegroundPressed")]
						public static ResourceValue<Brush> Pressed => new("RadioButtonForegroundPressed", true);

						[ResourceKeyDefinition(typeof(Brush), "RadioButtonForegroundDisabled")]
						public static ResourceValue<Brush> Disabled => new("RadioButtonForegroundDisabled", true);
					}

					public static class Stroke
					{
						[ResourceKeyDefinition(typeof(Brush), "RadioButtonOuterEllipseStroke")]
						public static ResourceValue<Brush> Default => new("RadioButtonOuterEllipseStroke", true);

						[ResourceKeyDefinition(typeof(Brush), "RadioButtonOuterEllipseStrokePointerOver")]
						public static ResourceValue<Brush> PointerOver => new("RadioButtonOuterEllipseStrokePointerOver", true);

						[ResourceKeyDefinition(typeof(Brush), "RadioButtonOuterEllipseStrokePressed")]
						public static ResourceValue<Brush> Pressed => new("RadioButtonOuterEllipseStrokePressed", true);

						[ResourceKeyDefinition(typeof(Brush), "RadioButtonOuterEllipseStrokeDisabled")]
						public static ResourceValue<Brush> Disabled => new("RadioButtonOuterEllipseStrokeDisabled", true);

						[ResourceKeyDefinition(typeof(Brush), "RadioButtonOuterEllipseCheckedStroke")]
						public static ResourceValue<Brush> Checked => new("RadioButtonOuterEllipseCheckedStroke", true);

						[ResourceKeyDefinition(typeof(Brush), "RadioButtonOuterEllipseCheckedStrokePointerOver")]
						public static ResourceValue<Brush> CheckedPointerOver => new("RadioButtonOuterEllipseCheckedStrokePointerOver", true);

						[ResourceKeyDefinition(typeof(Brush), "RadioButtonOuterEllipseCheckedStrokePressed")]
						public static ResourceValue<Brush> CheckedPressed => new("RadioButtonOuterEllipseCheckedStrokePressed", true);

						[ResourceKeyDefinition(typeof(Brush), "RadioButtonOuterEllipseCheckedStrokeDisabled")]
						public static ResourceValue<Brush> CheckedDisabled => new("RadioButtonOuterEllipseCheckedStrokeDisabled", true);
					}

					public static class Fill
					{
						[ResourceKeyDefinition(typeof(Brush), "RadioButtonOuterEllipseFill")]
						public static ResourceValue<Brush> Default => new("RadioButtonOuterEllipseFill", true);

						[ResourceKeyDefinition(typeof(Brush), "RadioButtonOuterEllipseFillPointerOver")]
						public static ResourceValue<Brush> PointerOver => new("RadioButtonOuterEllipseFillPointerOver", true);

						[ResourceKeyDefinition(typeof(Brush), "RadioButtonOuterEllipseFillPressed")]
						public static ResourceValue<Brush> Pressed => new("RadioButtonOuterEllipseFillPressed", true);

						[ResourceKeyDefinition(typeof(Brush), "RadioButtonOuterEllipseFillDisabled")]
						public static ResourceValue<Brush> Disabled => new("RadioButtonOuterEllipseFillDisabled", true);

						[ResourceKeyDefinition(typeof(Brush), "RadioButtonOuterEllipseCheckedFill")]
						public static ResourceValue<Brush> Checked => new("RadioButtonOuterEllipseCheckedFill", true);

						[ResourceKeyDefinition(typeof(Brush), "RadioButtonOuterEllipseCheckedFillPointerOver")]
						public static ResourceValue<Brush> CheckedPointerOver => new("RadioButtonOuterEllipseCheckedFillPointerOver", true);

						[ResourceKeyDefinition(typeof(Brush), "RadioButtonOuterEllipseCheckedFillPressed")]
						public static ResourceValue<Brush> CheckedPressed => new("RadioButtonOuterEllipseCheckedFillPressed", true);

						[ResourceKeyDefinition(typeof(Brush), "RadioButtonOuterEllipseCheckedFillDisabled")]
						public static ResourceValue<Brush> CheckedDisabled => new("RadioButtonOuterEllipseCheckedFillDisabled", true);
					}

					public static class HoverRingFill
					{
						[ResourceKeyDefinition(typeof(Brush), "MaterialRadioButtonHoverRingFillPointerOver")]
						public static ResourceValue<Brush> PointerOver => new("MaterialRadioButtonHoverRingFillPointerOver", true);

						[ResourceKeyDefinition(typeof(Brush), "MaterialRadioButtonHoverRingFillPressed")]
						public static ResourceValue<Brush> Pressed => new("MaterialRadioButtonHoverRingFillPressed", true);
					}

					public static class Typography
					{
						[ResourceKeyDefinition(typeof(FontFamily), "RadioButtonFontFamily")]
						public static ResourceValue<FontFamily> FontFamily => new("RadioButtonFontFamily", true);

						[ResourceKeyDefinition(typeof(FontWeights), "RadioButtonFontWeight")]
						public static ResourceValue<FontWeights> FontWeight => new("RadioButtonFontWeight", true);

						[ResourceKeyDefinition(typeof(double), "RadioButtonFontSize")]
						public static ResourceValue<double> FontSize => new("RadioButtonFontSize", true);

						[ResourceKeyDefinition(typeof(int), "RadioButtonCharacterSpacing")]
						public static ResourceValue<int> CharacterSpacing => new("RadioButtonCharacterSpacing", true);
					}
				}
			}

			public static class Styles
			{
				[ResourceKeyDefinition(typeof(Style), "RadioButtonStyle", TargetType = typeof(RadioButton))]
				public static ResourceValue<Style> Default => new("RadioButtonStyle");

				[ResourceKeyDefinition(typeof(Style), "SecondaryRadioButtonStyle", TargetType = typeof(RadioButton))]
				public static ResourceValue<Style> Secondary => new("SecondaryRadioButtonStyle");
			}
		}
	}
}
