using Microsoft.UI.Text;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Media;
using Uno.Extensions.Markup;
using Uno.Extensions.Markup.Internals;
using MUXCornerRadius = Microsoft.UI.Xaml.CornerRadius;

namespace Uno.Themes.Markup
{
	public static partial class Theme
	{
		public static class CheckBox
		{
			[ResourceKeyDefinition(typeof(double), "CheckBoxFocusAreaSize")]
			public static ThemeResourceKey<double> FocusAreaSize => new("CheckBoxFocusAreaSize");

			public static class Resources
			{
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

				public static class Glyph
				{
					[ResourceKeyDefinition(typeof(string), "CheckBoxHyphenGlyphPathData")]
					public static ThemeResourceKey<string> HyphenPathData => new("CheckBoxHyphenGlyphPathData");

					[ResourceKeyDefinition(typeof(string), "CheckBoxCheckGlyphPathData")]
					public static ThemeResourceKey<string> CheckPathData => new("CheckBoxCheckGlyphPathData");

					public static class Foreground
					{
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

						[ResourceKeyDefinition(typeof(Brush), "CheckBoxGlyphForegroundIndeterminate")]
						public static ThemeResourceKey<Brush> Indeterminate => new("CheckBoxGlyphForegroundIndeterminate");

						[ResourceKeyDefinition(typeof(Brush), "CheckBoxGlyphForegroundIndeterminatePointerOver")]
						public static ThemeResourceKey<Brush> IndeterminatePointerOver => new("CheckBoxGlyphForegroundIndeterminatePointerOver");

						[ResourceKeyDefinition(typeof(Brush), "CheckBoxGlyphForegroundIndeterminatePressed")]
						public static ThemeResourceKey<Brush> IndeterminatePressed => new("CheckBoxGlyphForegroundIndeterminatePressed");

						[ResourceKeyDefinition(typeof(Brush), "CheckBoxGlyphForegroundIndeterminateDisabled")]
						public static ThemeResourceKey<Brush> IndeterminateDisabled => new("CheckBoxGlyphForegroundIndeterminateDisabled");
					}
				}

				public static class Typography
				{
					[ResourceKeyDefinition(typeof(FontFamily), "CheckBoxFontFamily")]
					public static ThemeResourceKey<FontFamily> FontFamily => new("CheckBoxFontFamily");

					[ResourceKeyDefinition(typeof(FontWeights), "CheckBoxFontWeight")]
					public static ThemeResourceKey<FontWeights> FontWeight => new("CheckBoxFontWeight");

					[ResourceKeyDefinition(typeof(double), "CheckBoxFontSize")]
					public static ThemeResourceKey<double> FontSize => new("CheckBoxFontSize");

					[ResourceKeyDefinition(typeof(double), "CheckBoxCharacterSpacing")]
					public static ThemeResourceKey<double> CharacterSpacing => new("CheckBoxCharacterSpacing");
				}

				public static class CheckArea
				{
					[ResourceKeyDefinition(typeof(double), "CheckBoxCheckAreaSize")]
					public static ThemeResourceKey<double> Size => new("CheckBoxCheckAreaSize");

					[ResourceKeyDefinition(typeof(GridLength), "CheckBoxCheckAreaLength")]
					public static ThemeResourceKey<GridLength> Length => new("CheckBoxCheckAreaLength");

					[ResourceKeyDefinition(typeof(MUXCornerRadius), "CheckBoxCheckAreaCornerRadius")]
					public static ThemeResourceKey<MUXCornerRadius> CornerRadius => new("CheckBoxCheckAreaCornerRadius");

					[ResourceKeyDefinition(typeof(Thickness), "CheckBoxCheckAreaBorderThickness")]
					public static ThemeResourceKey<Thickness> BorderThickness => new("CheckBoxCheckAreaBorderThickness");

					[ResourceKeyDefinition(typeof(Thickness), "CheckBoxCheckAreaPadding")]
					public static ThemeResourceKey<Thickness> Padding => new("CheckBoxCheckAreaPadding");
				}
			}

			public static class Styles
			{
				[ResourceKeyDefinition(typeof(Style), "CheckBoxStyle", TargetType = typeof(CheckBox))]
				public static StaticResourceKey<Style> Default => new("CheckBoxStyle");

				[ResourceKeyDefinition(typeof(Style), "SecondaryCheckBoxStyle", TargetType = typeof(CheckBox))]
				public static StaticResourceKey<Style> Secondary => new("SecondaryCheckBoxStyle");
			}
		}
	}
}
