using Microsoft.UI.Xaml.Media;
using Uno.Extensions.Markup.Internals;
using Microsoft.UI.Xaml;
using MUXCornerRadius = Microsoft.UI.Xaml.CornerRadius;
using Microsoft.UI.Text;

namespace Uno.Themes.Markup
{
	public static partial class Theme
	{
		public static class CheckBox
		{
			public static class Resources
			{
				public static class Default
				{
					public static class Background
					{
						[ResourceKeyDefinition(typeof(Brush), "CheckBoxBackgroundUnchecked")]
						public static ResourceValue<Brush> Unchecked => new("CheckBoxBackgroundUnchecked", true);

						[ResourceKeyDefinition(typeof(Brush), "CheckBoxBackgroundUncheckedPointerOver")]
						public static ResourceValue<Brush> UncheckedPointerOver => new("CheckBoxBackgroundUncheckedPointerOver", true);

						[ResourceKeyDefinition(typeof(Brush), "CheckBoxBackgroundUncheckedPressed")]
						public static ResourceValue<Brush> UncheckedPressed => new("CheckBoxBackgroundUncheckedPressed", true);

						[ResourceKeyDefinition(typeof(Brush), "CheckBoxBackgroundUncheckedDisabled")]
						public static ResourceValue<Brush> UncheckedDisabled => new("CheckBoxBackgroundUncheckedDisabled", true);

						[ResourceKeyDefinition(typeof(Brush), "CheckBoxBackgroundChecked")]
						public static ResourceValue<Brush> Checked => new("CheckBoxBackgroundChecked", true);

						[ResourceKeyDefinition(typeof(Brush), "CheckBoxBackgroundCheckedPointerOver")]
						public static ResourceValue<Brush> CheckedPointerOver => new("CheckBoxBackgroundCheckedPointerOver", true);

						[ResourceKeyDefinition(typeof(Brush), "CheckBoxBackgroundCheckedPressed")]
						public static ResourceValue<Brush> CheckedPressed => new("CheckBoxBackgroundCheckedPressed", true);

						[ResourceKeyDefinition(typeof(Brush), "CheckBoxBackgroundCheckedDisabled")]
						public static ResourceValue<Brush> CheckedDisabled => new("CheckBoxBackgroundCheckedDisabled", true);

						[ResourceKeyDefinition(typeof(Brush), "CheckBoxBackgroundIndeterminate")]
						public static ResourceValue<Brush> Indeterminate => new("CheckBoxBackgroundIndeterminate", true);

						[ResourceKeyDefinition(typeof(Brush), "CheckBoxBackgroundIndeterminatePointerOver")]
						public static ResourceValue<Brush> IndeterminatePointerOver => new("CheckBoxBackgroundIndeterminatePointerOver", true);

						[ResourceKeyDefinition(typeof(Brush), "CheckBoxBackgroundIndeterminatePressed")]
						public static ResourceValue<Brush> IndeterminatePressed => new("CheckBoxBackgroundIndeterminatePressed", true);

						[ResourceKeyDefinition(typeof(Brush), "CheckBoxBackgroundIndeterminateDisabled")]
						public static ResourceValue<Brush> IndeterminateDisabled => new("CheckBoxBackgroundIndeterminateDisabled", true);
					}

					public static class Foreground
					{
						[ResourceKeyDefinition(typeof(Brush), "CheckBoxForegroundUnchecked")]
						public static ResourceValue<Brush> Unchecked => new("CheckBoxForegroundUnchecked", true);

						[ResourceKeyDefinition(typeof(Brush), "CheckBoxForegroundUncheckedPointerOver")]
						public static ResourceValue<Brush> UncheckedPointerOver => new("CheckBoxForegroundUncheckedPointerOver", true);

						[ResourceKeyDefinition(typeof(Brush), "CheckBoxForegroundUncheckedPressed")]
						public static ResourceValue<Brush> UncheckedPressed => new("CheckBoxForegroundUncheckedPressed", true);

						[ResourceKeyDefinition(typeof(Brush), "CheckBoxForegroundUncheckedDisabled")]
						public static ResourceValue<Brush> UncheckedDisabled => new("CheckBoxForegroundUncheckedDisabled", true);

						[ResourceKeyDefinition(typeof(Brush), "CheckBoxForegroundChecked")]
						public static ResourceValue<Brush> Checked => new("CheckBoxForegroundChecked", true);

						[ResourceKeyDefinition(typeof(Brush), "CheckBoxForegroundCheckedPointerOver")]
						public static ResourceValue<Brush> CheckedPointerOver => new("CheckBoxForegroundCheckedPointerOver", true);

						[ResourceKeyDefinition(typeof(Brush), "CheckBoxForegroundCheckedPressed")]
						public static ResourceValue<Brush> CheckedPressed => new("CheckBoxForegroundCheckedPressed", true);

						[ResourceKeyDefinition(typeof(Brush), "CheckBoxForegroundCheckedDisabled")]
						public static ResourceValue<Brush> CheckedDisabled => new("CheckBoxForegroundCheckedDisabled", true);

						[ResourceKeyDefinition(typeof(Brush), "CheckBoxForegroundIndeterminate")]
						public static ResourceValue<Brush> Indeterminate => new("CheckBoxForegroundIndeterminate", true);

						[ResourceKeyDefinition(typeof(Brush), "CheckBoxForegroundIndeterminatePointerOver")]
						public static ResourceValue<Brush> IndeterminatePointerOver => new("CheckBoxForegroundIndeterminatePointerOver", true);

						[ResourceKeyDefinition(typeof(Brush), "CheckBoxForegroundIndeterminatePressed")]
						public static ResourceValue<Brush> IndeterminatePressed => new("CheckBoxForegroundIndeterminatePressed", true);

						[ResourceKeyDefinition(typeof(Brush), "CheckBoxForegroundIndeterminateDisabled")]
						public static ResourceValue<Brush> IndeterminateDisabled => new("CheckBoxForegroundIndeterminateDisabled", true);
					}

					public static class BorderBrush
					{
						[ResourceKeyDefinition(typeof(Brush), "CheckBoxBorderBrushUnchecked")]
						public static ResourceValue<Brush> Unchecked => new("CheckBoxBorderBrushUnchecked", true);

						[ResourceKeyDefinition(typeof(Brush), "CheckBoxBorderBrushUncheckedPointerOver")]
						public static ResourceValue<Brush> UncheckedPointerOver => new("CheckBoxBorderBrushUncheckedPointerOver", true);

						[ResourceKeyDefinition(typeof(Brush), "CheckBoxBorderBrushUncheckedPressed")]
						public static ResourceValue<Brush> UncheckedPressed => new("CheckBoxBorderBrushUncheckedPressed", true);

						[ResourceKeyDefinition(typeof(Brush), "CheckBoxBorderBrushUncheckedDisabled")]
						public static ResourceValue<Brush> UncheckedDisabled => new("CheckBoxBorderBrushUncheckedDisabled", true);

						[ResourceKeyDefinition(typeof(Brush), "CheckBoxBorderBrushChecked")]
						public static ResourceValue<Brush> Checked => new("CheckBoxBorderBrushChecked", true);

						[ResourceKeyDefinition(typeof(Brush), "CheckBoxBorderBrushCheckedPointerOver")]
						public static ResourceValue<Brush> CheckedPointerOver => new("CheckBoxBorderBrushCheckedPointerOver", true);

						[ResourceKeyDefinition(typeof(Brush), "CheckBoxBorderBrushCheckedPressed")]
						public static ResourceValue<Brush> CheckedPressed => new("CheckBoxBorderBrushCheckedPressed", true);

						[ResourceKeyDefinition(typeof(Brush), "CheckBoxBorderBrushCheckedDisabled")]
						public static ResourceValue<Brush> CheckedDisabled => new("CheckBoxBorderBrushCheckedDisabled", true);

						[ResourceKeyDefinition(typeof(Brush), "CheckBoxBorderBrushIndeterminate")]
						public static ResourceValue<Brush> Indeterminate => new("CheckBoxBorderBrushIndeterminate", true);

						[ResourceKeyDefinition(typeof(Brush), "CheckBoxBorderBrushIndeterminatePointerOver")]
						public static ResourceValue<Brush> IndeterminatePointerOver => new("CheckBoxBorderBrushIndeterminatePointerOver", true);

						[ResourceKeyDefinition(typeof(Brush), "CheckBoxBorderBrushIndeterminatePressed")]
						public static ResourceValue<Brush> IndeterminatePressed => new("CheckBoxBorderBrushIndeterminatePressed", true);

						[ResourceKeyDefinition(typeof(Brush), "CheckBoxBorderBrushIndeterminateDisabled")]
						public static ResourceValue<Brush> IndeterminateDisabled => new("CheckBoxBorderBrushIndeterminateDisabled", true);
					}

					public static class Glyph
					{
						public static class Foreground
						{
							[ResourceKeyDefinition(typeof(Brush), "CheckBoxGlyphForegroundUnchecked")]
							public static ResourceValue<Brush> Unchecked => new("CheckBoxGlyphForegroundUnchecked", true);

							[ResourceKeyDefinition(typeof(Brush), "CheckBoxGlyphForegroundUncheckedPointerOver")]
							public static ResourceValue<Brush> UncheckedPointerOver => new("CheckBoxGlyphForegroundUncheckedPointerOver", true);

							[ResourceKeyDefinition(typeof(Brush), "CheckBoxGlyphForegroundUncheckedPressed")]
							public static ResourceValue<Brush> UncheckedPressed => new("CheckBoxGlyphForegroundUncheckedPressed", true);

							[ResourceKeyDefinition(typeof(Brush), "CheckBoxGlyphForegroundUncheckedDisabled")]
							public static ResourceValue<Brush> UncheckedDisabled => new("CheckBoxGlyphForegroundUncheckedDisabled", true);

							[ResourceKeyDefinition(typeof(Brush), "CheckBoxGlyphForegroundChecked")]
							public static ResourceValue<Brush> Checked => new("CheckBoxGlyphForegroundChecked", true);

							[ResourceKeyDefinition(typeof(Brush), "CheckBoxGlyphForegroundCheckedPointerOver")]
							public static ResourceValue<Brush> CheckedPointerOver => new("CheckBoxGlyphForegroundCheckedPointerOver", true);

							[ResourceKeyDefinition(typeof(Brush), "CheckBoxGlyphForegroundCheckedPressed")]
							public static ResourceValue<Brush> CheckedPressed => new("CheckBoxGlyphForegroundCheckedPressed", true);

							[ResourceKeyDefinition(typeof(Brush), "CheckBoxGlyphForegroundCheckedDisabled")]
							public static ResourceValue<Brush> CheckedDisabled => new("CheckBoxGlyphForegroundCheckedDisabled", true);

							[ResourceKeyDefinition(typeof(Brush), "CheckBoxGlyphForegroundIndeterminate")]
							public static ResourceValue<Brush> Indeterminate => new("CheckBoxGlyphForegroundIndeterminate", true);

							[ResourceKeyDefinition(typeof(Brush), "CheckBoxGlyphForegroundIndeterminatePointerOver")]
							public static ResourceValue<Brush> IndeterminatePointerOver => new("CheckBoxGlyphForegroundIndeterminatePointerOver", true);

							[ResourceKeyDefinition(typeof(Brush), "CheckBoxGlyphForegroundIndeterminatePressed")]
							public static ResourceValue<Brush> IndeterminatePressed => new("CheckBoxGlyphForegroundIndeterminatePressed", true);

							[ResourceKeyDefinition(typeof(Brush), "CheckBoxGlyphForegroundIndeterminateDisabled")]
							public static ResourceValue<Brush> IndeterminateDisabled => new("CheckBoxGlyphForegroundIndeterminateDisabled", true);
						}

						public static class PathData
						{
							[ResourceKeyDefinition(typeof(string), "CheckBoxHyphenGlyphPathData")]
							public static ResourceValue<string> Hyphen => new("CheckBoxHyphenGlyphPathData", true);

							[ResourceKeyDefinition(typeof(string), "CheckBoxCheckGlyphPathData")]
							public static ResourceValue<string> Check => new("CheckBoxCheckGlyphPathData", true);
						}
					}

					public static class Typography
					{
						[ResourceKeyDefinition(typeof(FontFamily), "CheckBoxFontFamily")]
						public static ResourceValue<FontFamily> FontFamily => new("CheckBoxFontFamily", true);

						[ResourceKeyDefinition(typeof(FontWeights), "CheckBoxFontWeight")]
						public static ResourceValue<FontWeights> FontWeight => new("CheckBoxFontWeight", true);

						[ResourceKeyDefinition(typeof(double), "CheckBoxFontSize")]
						public static ResourceValue<double> FontSize => new("CheckBoxFontSize", true);

						[ResourceKeyDefinition(typeof(double), "CheckBoxCharacterSpacing")]
						public static ResourceValue<double> CharacterSpacing => new("CheckBoxCharacterSpacing", true);
					}

					public static class FocusArea
					{
						public static class Size
						{
							[ResourceKeyDefinition(typeof(double), "CheckBoxFocusAreaSize")]
							public static ResourceValue<double> Default => new("CheckBoxFocusAreaSize", true);
						}
					}

					public static class CheckArea
					{
						public static class Size
						{
							[ResourceKeyDefinition(typeof(double), "CheckBoxCheckAreaSize")]
							public static ResourceValue<double> Default => new("CheckBoxCheckAreaSize", true);
						}

						public static class Length
						{
							[ResourceKeyDefinition(typeof(GridLength), "CheckBoxCheckAreaLength")]
							public static ResourceValue<GridLength> Default => new("CheckBoxCheckAreaLength", true);
						}

						public static class CornerRadius
						{
							[ResourceKeyDefinition(typeof(MUXCornerRadius), "CheckBoxCheckAreaCornerRadius")]
							public static ResourceValue<MUXCornerRadius> Default => new("CheckBoxCheckAreaCornerRadius", true);
						}

						public static class BorderThickness
						{
							[ResourceKeyDefinition(typeof(Thickness), "CheckBoxCheckAreaBorderThickness")]
							public static ResourceValue<Thickness> Default => new("CheckBoxCheckAreaBorderThickness", true);
						}

						public static class Padding
						{
							[ResourceKeyDefinition(typeof(Thickness), "CheckBoxCheckAreaPadding")]
							public static ResourceValue<Thickness> Default => new("CheckBoxCheckAreaPadding", true);
						}
					}
				}
			}

			public static class Styles
			{
				[ResourceKeyDefinition(typeof(Style), "CheckBoxStyle", TargetType = typeof(CheckBox))]
				public static ResourceValue<Style> Default => new("CheckBoxStyle");

				[ResourceKeyDefinition(typeof(Style), "SecondaryCheckBoxStyle", TargetType = typeof(CheckBox))]
				public static ResourceValue<Style> Secondary => new("SecondaryCheckBoxStyle");
			}
		}
	}
}
