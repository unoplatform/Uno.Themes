using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Media;
using Uno.Extensions.Markup;
using Uno.Extensions.Markup.Internals;

namespace Uno.Themes.Markup;

public static partial class Theme
{
	public static partial class CheckBox
	{
		public static partial class Resources
		{
			public static partial class Default
			{
				public static partial class CheckArea
				{
					[ResourceKeyDefinition(typeof(double), "CheckBoxCheckAreaSize")]
					public static ThemeResourceKey<double> Size => new("CheckBoxCheckAreaSize");

					[ResourceKeyDefinition(typeof(Thickness), "CheckBoxCheckAreaBorderThickness")]
					public static ThemeResourceKey<Thickness> BorderThickness => new("CheckBoxCheckAreaBorderThickness");

					[ResourceKeyDefinition(typeof(CornerRadius), "CheckBoxCheckAreaCornerRadius")]
					public static ThemeResourceKey<CornerRadius> CornerRadius => new("CheckBoxCheckAreaCornerRadius");

					[ResourceKeyDefinition(typeof(Thickness), "CheckBoxCheckAreaPadding")]
					public static ThemeResourceKey<Thickness> Padding => new("CheckBoxCheckAreaPadding");
				}

				public static class Background
				{
					[ResourceKeyDefinition(typeof(Brush), "CheckBoxBackgroundUnchecked")]
					public static ThemeResourceKey<Brush> Unchecked => new("CheckBoxBackgroundUnchecked");

					[ResourceKeyDefinition(typeof(Brush), "CheckBoxBackgroundUncheckedPointerOver")]
					public static ThemeResourceKey<Brush> UncheckedPointerOver => new("CheckBoxBackgroundUncheckedPointerOver");

					[ResourceKeyDefinition(typeof(Brush), "CheckBoxBackgroundUncheckedPressed")]
					public static ThemeResourceKey<Brush> UncheckedPressed => new("CheckBoxBackgroundUncheckedPressed");

					[ResourceKeyDefinition(typeof(Brush), "CheckBoxBackgroundUncheckedDisabled")]
					public static ThemeResourceKey<Brush> UncheckedDisabled => new("CheckBoxBackgroundUncheckedDisabled");

					[ResourceKeyDefinition(typeof(Brush), "CheckBoxBackgroundChecked")]
					public static ThemeResourceKey<Brush> Checked => new("CheckBoxBackgroundChecked");

					[ResourceKeyDefinition(typeof(Brush), "CheckBoxBackgroundCheckedPointerOver")]
					public static ThemeResourceKey<Brush> CheckedPointerOver => new("CheckBoxBackgroundCheckedPointerOver");

					[ResourceKeyDefinition(typeof(Brush), "CheckBoxBackgroundCheckedPressed")]
					public static ThemeResourceKey<Brush> CheckedPressed => new("CheckBoxBackgroundCheckedPressed");

					[ResourceKeyDefinition(typeof(Brush), "CheckBoxBackgroundCheckedDisabled")]
					public static ThemeResourceKey<Brush> CheckedDisabled => new("CheckBoxBackgroundCheckedDisabled");

					[ResourceKeyDefinition(typeof(Brush), "CheckBoxBackgroundIndeterminate")]
					public static ThemeResourceKey<Brush> Indeterminate => new("CheckBoxBackgroundIndeterminate");

					[ResourceKeyDefinition(typeof(Brush), "CheckBoxBackgroundIndeterminatePointerOver")]
					public static ThemeResourceKey<Brush> IndeterminatePointerOver => new("CheckBoxBackgroundIndeterminatePointerOver");

					[ResourceKeyDefinition(typeof(Brush), "CheckBoxBackgroundIndeterminatePressed")]
					public static ThemeResourceKey<Brush> IndeterminatePressed => new("CheckBoxBackgroundIndeterminatePressed");

					[ResourceKeyDefinition(typeof(Brush), "CheckBoxBackgroundIndeterminateDisabled")]
					public static ThemeResourceKey<Brush> IndeterminateDisabled => new("CheckBoxBackgroundIndeterminateDisabled");
				}

				public static class Foreground
				{
					[ResourceKeyDefinition(typeof(Brush), "CheckBoxForegroundUnchecked")]
					public static ThemeResourceKey<Brush> Unchecked => new("CheckBoxForegroundUnchecked");

					[ResourceKeyDefinition(typeof(Brush), "CheckBoxForegroundUncheckedPointerOver")]
					public static ThemeResourceKey<Brush> UncheckedPointerOver => new("CheckBoxForegroundUncheckedPointerOver");

					[ResourceKeyDefinition(typeof(Brush), "CheckBoxForegroundUncheckedPressed")]
					public static ThemeResourceKey<Brush> UncheckedPressed => new("CheckBoxForegroundUncheckedPressed");

					[ResourceKeyDefinition(typeof(Brush), "CheckBoxForegroundUncheckedDisabled")]
					public static ThemeResourceKey<Brush> UncheckedDisabled => new("CheckBoxForegroundUncheckedDisabled");

					[ResourceKeyDefinition(typeof(Brush), "CheckBoxForegroundChecked")]
					public static ThemeResourceKey<Brush> Checked => new("CheckBoxForegroundChecked");

					[ResourceKeyDefinition(typeof(Brush), "CheckBoxForegroundCheckedPointerOver")]
					public static ThemeResourceKey<Brush> CheckedPointerOver => new("CheckBoxForegroundCheckedPointerOver");

					[ResourceKeyDefinition(typeof(Brush), "CheckBoxForegroundCheckedPressed")]
					public static ThemeResourceKey<Brush> CheckedPressed => new("CheckBoxForegroundCheckedPressed");

					[ResourceKeyDefinition(typeof(Brush), "CheckBoxForegroundCheckedDisabled")]
					public static ThemeResourceKey<Brush> CheckedDisabled => new("CheckBoxForegroundCheckedDisabled");

					[ResourceKeyDefinition(typeof(Brush), "CheckBoxForegroundIndeterminate")]
					public static ThemeResourceKey<Brush> Indeterminate => new("CheckBoxForegroundIndeterminate");

					[ResourceKeyDefinition(typeof(Brush), "CheckBoxForegroundIndeterminatePointerOver")]
					public static ThemeResourceKey<Brush> IndeterminatePointerOver => new("CheckBoxForegroundIndeterminatePointerOver");

					[ResourceKeyDefinition(typeof(Brush), "CheckBoxForegroundIndeterminatePressed")]
					public static ThemeResourceKey<Brush> IndeterminatePressed => new("CheckBoxForegroundIndeterminatePressed");

					[ResourceKeyDefinition(typeof(Brush), "CheckBoxForegroundIndeterminateDisabled")]
					public static ThemeResourceKey<Brush> IndeterminateDisabled => new("CheckBoxForegroundIndeterminateDisabled");
				}

				public static class BorderBrush
				{
					[ResourceKeyDefinition(typeof(Brush), "CheckBoxBorderBrushUnchecked")]
					public static ThemeResourceKey<Brush> Unchecked => new("CheckBoxBorderBrushUnchecked");

					[ResourceKeyDefinition(typeof(Brush), "CheckBoxBorderBrushUncheckedPointerOver")]
					public static ThemeResourceKey<Brush> UncheckedPointerOver => new("CheckBoxBorderBrushUncheckedPointerOver");

					[ResourceKeyDefinition(typeof(Brush), "CheckBoxBorderBrushUncheckedPressed")]
					public static ThemeResourceKey<Brush> UncheckedPressed => new("CheckBoxBorderBrushUncheckedPressed");

					[ResourceKeyDefinition(typeof(Brush), "CheckBoxBorderBrushUncheckedDisabled")]
					public static ThemeResourceKey<Brush> UncheckedDisabled => new("CheckBoxBorderBrushUncheckedDisabled");

					[ResourceKeyDefinition(typeof(Brush), "CheckBoxBorderBrushChecked")]
					public static ThemeResourceKey<Brush> Checked => new("CheckBoxBorderBrushChecked");

					[ResourceKeyDefinition(typeof(Brush), "CheckBoxBorderBrushCheckedPointerOver")]
					public static ThemeResourceKey<Brush> CheckedPointerOver => new("CheckBoxBorderBrushCheckedPointerOver");

					[ResourceKeyDefinition(typeof(Brush), "CheckBoxBorderBrushCheckedPressed")]
					public static ThemeResourceKey<Brush> CheckedPressed => new("CheckBoxBorderBrushCheckedPressed");

					[ResourceKeyDefinition(typeof(Brush), "CheckBoxBorderBrushCheckedDisabled")]
					public static ThemeResourceKey<Brush> CheckedDisabled => new("CheckBoxBorderBrushCheckedDisabled");

					[ResourceKeyDefinition(typeof(Brush), "CheckBoxBorderBrushIndeterminate")]
					public static ThemeResourceKey<Brush> Indeterminate => new("CheckBoxBorderBrushIndeterminate");

					[ResourceKeyDefinition(typeof(Brush), "CheckBoxBorderBrushIndeterminatePointerOver")]
					public static ThemeResourceKey<Brush> IndeterminatePointerOver => new("CheckBoxBorderBrushIndeterminatePointerOver");

					[ResourceKeyDefinition(typeof(Brush), "CheckBoxBorderBrushIndeterminatePressed")]
					public static ThemeResourceKey<Brush> IndeterminatePressed => new("CheckBoxBorderBrushIndeterminatePressed");

					[ResourceKeyDefinition(typeof(Brush), "CheckBoxBorderBrushIndeterminateDisabled")]
					public static ThemeResourceKey<Brush> IndeterminateDisabled => new("CheckBoxBorderBrushIndeterminateDisabled");
				}

				public static partial class CheckBackgroundFill
				{
					[ResourceKeyDefinition(typeof(Brush), "CheckBoxCheckBackgroundFill")]
					public static ThemeResourceKey<Brush> Default => new("CheckBoxCheckBackgroundFill");

					[ResourceKeyDefinition(typeof(Brush), "CheckBoxCheckBackgroundFillUnchecked")]
					public static ThemeResourceKey<Brush> Unchecked => new("CheckBoxCheckBackgroundFillUnchecked");

					[ResourceKeyDefinition(typeof(Brush), "CheckBoxCheckBackgroundFillUncheckedPointerOver")]
					public static ThemeResourceKey<Brush> UncheckedPointerOver => new("CheckBoxCheckBackgroundFillUncheckedPointerOver");

					[ResourceKeyDefinition(typeof(Brush), "CheckBoxCheckBackgroundFillUncheckedPressed")]
					public static ThemeResourceKey<Brush> UncheckedPressed => new("CheckBoxCheckBackgroundFillUncheckedPressed");

					[ResourceKeyDefinition(typeof(Brush), "CheckBoxCheckBackgroundFillUncheckedDisabled")]
					public static ThemeResourceKey<Brush> UncheckedDisabled => new("CheckBoxCheckBackgroundFillUncheckedDisabled");

					[ResourceKeyDefinition(typeof(Brush), "CheckBoxCheckBackgroundFillChecked")]
					public static ThemeResourceKey<Brush> Checked => new("CheckBoxCheckBackgroundFillChecked");

					[ResourceKeyDefinition(typeof(Brush), "CheckBoxCheckBackgroundFillCheckedPointerOver")]
					public static ThemeResourceKey<Brush> CheckedPointerOver => new("CheckBoxCheckBackgroundFillCheckedPointerOver");

					[ResourceKeyDefinition(typeof(Brush), "CheckBoxCheckBackgroundFillCheckedPressed")]
					public static ThemeResourceKey<Brush> CheckedPressed => new("CheckBoxCheckBackgroundFillCheckedPressed");

					[ResourceKeyDefinition(typeof(Brush), "CheckBoxCheckBackgroundFillCheckedDisabled")]
					public static ThemeResourceKey<Brush> CheckedDisabled => new("CheckBoxCheckBackgroundFillCheckedDisabled");

					[ResourceKeyDefinition(typeof(Brush), "CheckBoxCheckBackgroundFillIndeterminate")]
					public static ThemeResourceKey<Brush> Indeterminate => new("CheckBoxCheckBackgroundFillIndeterminate");

					[ResourceKeyDefinition(typeof(Brush), "CheckBoxCheckBackgroundFillIndeterminatePointerOver")]
					public static ThemeResourceKey<Brush> IndeterminatePointerOver => new("CheckBoxCheckBackgroundFillIndeterminatePointerOver");

					[ResourceKeyDefinition(typeof(Brush), "CheckBoxCheckBackgroundFillIndeterminatePressed")]
					public static ThemeResourceKey<Brush> IndeterminatePressed => new("CheckBoxCheckBackgroundFillIndeterminatePressed");

					[ResourceKeyDefinition(typeof(Brush), "CheckBoxCheckBackgroundFillIndeterminateDisabled")]
					public static ThemeResourceKey<Brush> IndeterminateDisabled => new("CheckBoxCheckBackgroundFillIndeterminateDisabled");
				}

				public static partial class CheckBackgroundStroke
				{
					[ResourceKeyDefinition(typeof(Brush), "CheckBoxCheckBackgroundStroke")]
					public static ThemeResourceKey<Brush> Default => new("CheckBoxCheckBackgroundStroke");

					[ResourceKeyDefinition(typeof(Brush), "CheckBoxCheckBackgroundStrokeUnchecked")]
					public static ThemeResourceKey<Brush> Unchecked => new("CheckBoxCheckBackgroundStrokeUnchecked");

					[ResourceKeyDefinition(typeof(Brush), "CheckBoxCheckBackgroundStrokeUncheckedPointerOver")]
					public static ThemeResourceKey<Brush> UncheckedPointerOver => new("CheckBoxCheckBackgroundStrokeUncheckedPointerOver");

					[ResourceKeyDefinition(typeof(Brush), "CheckBoxCheckBackgroundStrokeUncheckedPressed")]
					public static ThemeResourceKey<Brush> UncheckedPressed => new("CheckBoxCheckBackgroundStrokeUncheckedPressed");

					[ResourceKeyDefinition(typeof(Brush), "CheckBoxCheckBackgroundStrokeUncheckedDisabled")]
					public static ThemeResourceKey<Brush> UncheckedDisabled => new("CheckBoxCheckBackgroundStrokeUncheckedDisabled");

					[ResourceKeyDefinition(typeof(Brush), "CheckBoxCheckBackgroundStrokeChecked")]
					public static ThemeResourceKey<Brush> Checked => new("CheckBoxCheckBackgroundStrokeChecked");

					[ResourceKeyDefinition(typeof(Brush), "CheckBoxCheckBackgroundStrokeCheckedPointerOver")]
					public static ThemeResourceKey<Brush> CheckedPointerOver => new("CheckBoxCheckBackgroundStrokeCheckedPointerOver");

					[ResourceKeyDefinition(typeof(Brush), "CheckBoxCheckBackgroundStrokeCheckedPressed")]
					public static ThemeResourceKey<Brush> CheckedPressed => new("CheckBoxCheckBackgroundStrokeCheckedPressed");

					[ResourceKeyDefinition(typeof(Brush), "CheckBoxCheckBackgroundStrokeCheckedDisabled")]
					public static ThemeResourceKey<Brush> CheckedDisabled => new("CheckBoxCheckBackgroundStrokeCheckedDisabled");

					[ResourceKeyDefinition(typeof(Brush), "CheckBoxCheckBackgroundStrokeIndeterminate")]
					public static ThemeResourceKey<Brush> Indeterminate => new("CheckBoxCheckBackgroundStrokeIndeterminate");

					[ResourceKeyDefinition(typeof(Brush), "CheckBoxCheckBackgroundStrokeIndeterminatePointerOver")]
					public static ThemeResourceKey<Brush> IndeterminatePointerOver => new("CheckBoxCheckBackgroundStrokeIndeterminatePointerOver");

					[ResourceKeyDefinition(typeof(Brush), "CheckBoxCheckBackgroundStrokeIndeterminatePressed")]
					public static ThemeResourceKey<Brush> IndeterminatePressed => new("CheckBoxCheckBackgroundStrokeIndeterminatePressed");

					[ResourceKeyDefinition(typeof(Brush), "CheckBoxCheckBackgroundStrokeIndeterminateDisabled")]
					public static ThemeResourceKey<Brush> IndeterminateDisabled => new("CheckBoxCheckBackgroundStrokeIndeterminateDisabled");
				}

				public static partial class GlyphForeground
				{
					[ResourceKeyDefinition(typeof(Brush), "CheckBoxGlyphForeground")]
					public static ThemeResourceKey<Brush> Default => new("CheckBoxGlyphForeground");

					[ResourceKeyDefinition(typeof(Brush), "CheckBoxGlyphForegroundIndeterminate")]
					public static ThemeResourceKey<Brush> Indeterminate => new("CheckBoxGlyphForegroundIndeterminate");

					[ResourceKeyDefinition(typeof(Brush), "CheckBoxGlyphForegroundIndeterminatePointerOver")]
					public static ThemeResourceKey<Brush> IndeterminatePointerOver => new("CheckBoxGlyphForegroundIndeterminatePointerOver");

					[ResourceKeyDefinition(typeof(Brush), "CheckBoxGlyphForegroundIndeterminatePressed")]
					public static ThemeResourceKey<Brush> IndeterminatePressed => new("CheckBoxGlyphForegroundIndeterminatePressed");

					[ResourceKeyDefinition(typeof(Brush), "CheckBoxGlyphForegroundIndeterminateDisabled")]
					public static ThemeResourceKey<Brush> IndeterminateDisabled => new("CheckBoxGlyphForegroundIndeterminateDisabled");

					[ResourceKeyDefinition(typeof(Brush), "CheckBoxGlyphForegroundUnchecked")]
					public static ThemeResourceKey<Brush> Unchecked => new("CheckBoxGlyphForegroundUnchecked");

					[ResourceKeyDefinition(typeof(Brush), "CheckBoxGlyphForegroundUncheckedPointerOver")]
					public static ThemeResourceKey<Brush> UncheckedPointerOver => new("CheckBoxGlyphForegroundUncheckedPointerOver");

					[ResourceKeyDefinition(typeof(Brush), "CheckBoxGlyphForegroundUncheckedPressed")]
					public static ThemeResourceKey<Brush> UncheckedPressed => new("CheckBoxGlyphForegroundUncheckedPressed");

					[ResourceKeyDefinition(typeof(Brush), "CheckBoxGlyphForegroundUncheckedDisabled")]
					public static ThemeResourceKey<Brush> UncheckedDisabled => new("CheckBoxGlyphForegroundUncheckedDisabled");

					[ResourceKeyDefinition(typeof(Brush), "CheckBoxGlyphForegroundChecked")]
					public static ThemeResourceKey<Brush> Checked => new("CheckBoxGlyphForegroundChecked");

					[ResourceKeyDefinition(typeof(Brush), "CheckBoxGlyphForegroundCheckedPointerOver")]
					public static ThemeResourceKey<Brush> CheckedPointerOver => new("CheckBoxGlyphForegroundCheckedPointerOver");

					[ResourceKeyDefinition(typeof(Brush), "CheckBoxGlyphForegroundCheckedPressed")]
					public static ThemeResourceKey<Brush> CheckedPressed => new("CheckBoxGlyphForegroundCheckedPressed");

					[ResourceKeyDefinition(typeof(Brush), "CheckBoxGlyphForegroundCheckedDisabled")]
					public static ThemeResourceKey<Brush> CheckedDisabled => new("CheckBoxGlyphForegroundCheckedDisabled");
				}

				public static partial class StateCircle
				{
					public static partial class Opacity
					{
						[ResourceKeyDefinition(typeof(double), "CheckBoxStateCircleOpacityPointerOver")]
						public static ThemeResourceKey<double> PointerOver => new("CheckBoxStateCircleOpacityPointerOver");

						[ResourceKeyDefinition(typeof(double), "CheckBoxStateCircleOpacityPressed")]
						public static ThemeResourceKey<double> Pressed => new("CheckBoxStateCircleOpacityPressed");

						[ResourceKeyDefinition(typeof(double), "CheckBoxStateCircleOpacityFocused")]
						public static ThemeResourceKey<double> Focused => new("CheckBoxStateCircleOpacityFocused");
					}

					public static partial class Fill
					{

						[ResourceKeyDefinition(typeof(Brush), "CheckBoxStateCircleFillChecked")]
						public static ThemeResourceKey<Brush> Checked => new("CheckBoxStateCircleFillChecked");

						[ResourceKeyDefinition(typeof(Brush), "CheckBoxStateCircleFillUnchecked")]
						public static ThemeResourceKey<Brush> Unchecked => new("CheckBoxStateCircleFillUnchecked");
					}
				}

				public static partial class Typography
				{
					[ResourceKeyDefinition(typeof(int), "CheckBoxCharacterSpacing")]
					public static ThemeResourceKey<int> CharacterSpacing => new("CheckBoxCharacterSpacing");

					[ResourceKeyDefinition(typeof(FontFamily), "CheckBoxFontFamily")]
					public static ThemeResourceKey<FontFamily> FontFamily => new("CheckBoxFontFamily");

					[ResourceKeyDefinition(typeof(double), "CheckBoxFontSize")]
					public static ThemeResourceKey<double> FontSize => new("CheckBoxFontSize");

					[ResourceKeyDefinition(typeof(string), "CheckBoxFontWeight")]
					public static ThemeResourceKey<string> FontWeight => new("CheckBoxFontWeight");
				}

				[ResourceKeyDefinition(typeof(string), "CheckBoxCheckGlyphPathData")]
				public static ThemeResourceKey<string> CheckGlyphPathData => new("CheckBoxCheckGlyphPathData");

				[ResourceKeyDefinition(typeof(double), "CheckBoxFocusAreaSize")]
				public static ThemeResourceKey<double> FocusAreaSize => new("CheckBoxFocusAreaSize");

				[ResourceKeyDefinition(typeof(string), "CheckBoxHyphenGlyphPathData")]
				public static ThemeResourceKey<string> HyphenGlyphPathData => new("CheckBoxHyphenGlyphPathData");
			}
		}

		public static partial class Styles
		{
			[ResourceKeyDefinition(typeof(Style), "CheckBoxStyle", TargetType = typeof(global::Microsoft.UI.Xaml.Controls.CheckBox))]
			public static StaticResourceKey<Style> Default => new("CheckBoxStyle");
		}
	}
}
