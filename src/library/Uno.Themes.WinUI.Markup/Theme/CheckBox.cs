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
		public static partial class CheckBox
		{
			public static partial class Resources
			{
				public static partial class CheckArea
				{
					[ResourceKeyDefinition(typeof(Thickness), "CheckBoxCheckAreaBorderThickness")]
					public static ThemeResourceKey<Thickness> BorderThickness => new("CheckBoxCheckAreaBorderThickness");

					[ResourceKeyDefinition(typeof(CornerRadius), "CheckBoxCheckAreaCornerRadius")]
					public static ThemeResourceKey<CornerRadius> CornerRadius => new("CheckBoxCheckAreaCornerRadius");

					[ResourceKeyDefinition(typeof(Thickness), "CheckBoxCheckAreaPadding")]
					public static ThemeResourceKey<Thickness> Padding => new("CheckBoxCheckAreaPadding");
				}

				public static readonly CheckBackgroundFillUncheckedVSG CheckBackgroundFillUnchecked = new();
				public partial class CheckBackgroundFillUncheckedVSG
				{
					[ResourceKeyDefinition(typeof(Brush), "CheckBoxCheckBackgroundFillUnchecked")]
					public ThemeResourceKey<Brush> Default = new("CheckBoxCheckBackgroundFillUnchecked");

					[ResourceKeyDefinition(typeof(Brush), "CheckBoxCheckBackgroundFillUncheckedPointerOver")]
					public ThemeResourceKey<Brush> UncheckedPointerOver = new("CheckBoxCheckBackgroundFillUncheckedPointerOver");

					[ResourceKeyDefinition(typeof(Brush), "CheckBoxCheckBackgroundFillUncheckedPressed")]
					public ThemeResourceKey<Brush> UncheckedPressed = new("CheckBoxCheckBackgroundFillUncheckedPressed");

					[ResourceKeyDefinition(typeof(Brush), "CheckBoxCheckBackgroundFillUncheckedDisabled")]
					public ThemeResourceKey<Brush> UncheckedDisabled = new("CheckBoxCheckBackgroundFillUncheckedDisabled");

					[ResourceKeyDefinition(typeof(Brush), "CheckBoxCheckBackgroundFillChecked")]
					public ThemeResourceKey<Brush> CheckedNormal = new("CheckBoxCheckBackgroundFillChecked");

					[ResourceKeyDefinition(typeof(Brush), "CheckBoxCheckBackgroundFillCheckedPointerOver")]
					public ThemeResourceKey<Brush> CheckedPointerOver = new("CheckBoxCheckBackgroundFillCheckedPointerOver");

					[ResourceKeyDefinition(typeof(Brush), "CheckBoxCheckBackgroundFillCheckedPressed")]
					public ThemeResourceKey<Brush> CheckedPressed = new("CheckBoxCheckBackgroundFillCheckedPressed");

					[ResourceKeyDefinition(typeof(Brush), "CheckBoxCheckBackgroundFillCheckedDisabled")]
					public ThemeResourceKey<Brush> CheckedDisabled = new("CheckBoxCheckBackgroundFillCheckedDisabled");

					[ResourceKeyDefinition(typeof(Brush), "CheckBoxCheckBackgroundFillIndeterminate")]
					public ThemeResourceKey<Brush> IndeterminateNormal = new("CheckBoxCheckBackgroundFillIndeterminate");

					[ResourceKeyDefinition(typeof(Brush), "CheckBoxCheckBackgroundFillIndeterminatePointerOver")]
					public ThemeResourceKey<Brush> IndeterminatePointerOver = new("CheckBoxCheckBackgroundFillIndeterminatePointerOver");

					[ResourceKeyDefinition(typeof(Brush), "CheckBoxCheckBackgroundFillIndeterminatePressed")]
					public ThemeResourceKey<Brush> IndeterminatePressed = new("CheckBoxCheckBackgroundFillIndeterminatePressed");

					[ResourceKeyDefinition(typeof(Brush), "CheckBoxCheckBackgroundFillIndeterminateDisabled")]
					public ThemeResourceKey<Brush> IndeterminateDisabled = new("CheckBoxCheckBackgroundFillIndeterminateDisabled");

					public static implicit operator ThemeResourceKey<Brush>(CheckBackgroundFillUncheckedVSG self) => self.Default;
				}

				public static readonly CheckBackgroundStrokeUncheckedVSG CheckBackgroundStrokeUnchecked = new();
				public partial class CheckBackgroundStrokeUncheckedVSG
				{
					[ResourceKeyDefinition(typeof(Brush), "CheckBoxCheckBackgroundStrokeUnchecked")]
					public ThemeResourceKey<Brush> Default = new("CheckBoxCheckBackgroundStrokeUnchecked");

					[ResourceKeyDefinition(typeof(Brush), "CheckBoxCheckBackgroundStrokeUncheckedPointerOver")]
					public ThemeResourceKey<Brush> UncheckedPointerOver = new("CheckBoxCheckBackgroundStrokeUncheckedPointerOver");

					[ResourceKeyDefinition(typeof(Brush), "CheckBoxCheckBackgroundStrokeUncheckedPressed")]
					public ThemeResourceKey<Brush> UncheckedPressed = new("CheckBoxCheckBackgroundStrokeUncheckedPressed");

					[ResourceKeyDefinition(typeof(Brush), "CheckBoxCheckBackgroundStrokeUncheckedDisabled")]
					public ThemeResourceKey<Brush> UncheckedDisabled = new("CheckBoxCheckBackgroundStrokeUncheckedDisabled");

					[ResourceKeyDefinition(typeof(Brush), "CheckBoxCheckBackgroundStrokeChecked")]
					public ThemeResourceKey<Brush> CheckedNormal = new("CheckBoxCheckBackgroundStrokeChecked");

					[ResourceKeyDefinition(typeof(Brush), "CheckBoxCheckBackgroundStrokeCheckedPointerOver")]
					public ThemeResourceKey<Brush> CheckedPointerOver = new("CheckBoxCheckBackgroundStrokeCheckedPointerOver");

					[ResourceKeyDefinition(typeof(Brush), "CheckBoxCheckBackgroundStrokeCheckedPressed")]
					public ThemeResourceKey<Brush> CheckedPressed = new("CheckBoxCheckBackgroundStrokeCheckedPressed");

					[ResourceKeyDefinition(typeof(Brush), "CheckBoxCheckBackgroundStrokeCheckedDisabled")]
					public ThemeResourceKey<Brush> CheckedDisabled = new("CheckBoxCheckBackgroundStrokeCheckedDisabled");

					[ResourceKeyDefinition(typeof(Brush), "CheckBoxCheckBackgroundStrokeIndeterminate")]
					public ThemeResourceKey<Brush> IndeterminateNormal = new("CheckBoxCheckBackgroundStrokeIndeterminate");

					[ResourceKeyDefinition(typeof(Brush), "CheckBoxCheckBackgroundStrokeIndeterminatePointerOver")]
					public ThemeResourceKey<Brush> IndeterminatePointerOver = new("CheckBoxCheckBackgroundStrokeIndeterminatePointerOver");

					[ResourceKeyDefinition(typeof(Brush), "CheckBoxCheckBackgroundStrokeIndeterminatePressed")]
					public ThemeResourceKey<Brush> IndeterminatePressed = new("CheckBoxCheckBackgroundStrokeIndeterminatePressed");

					[ResourceKeyDefinition(typeof(Brush), "CheckBoxCheckBackgroundStrokeIndeterminateDisabled")]
					public ThemeResourceKey<Brush> IndeterminateDisabled = new("CheckBoxCheckBackgroundStrokeIndeterminateDisabled");

					public static implicit operator ThemeResourceKey<Brush>(CheckBackgroundStrokeUncheckedVSG self) => self.Default;
				}

				public static readonly GlyphForegroundCheckedVSG GlyphForegroundChecked = new();
				public partial class GlyphForegroundCheckedVSG
				{
					[ResourceKeyDefinition(typeof(Brush), "CheckBoxGlyphForegroundChecked")]
					public ThemeResourceKey<Brush> Default = new("CheckBoxGlyphForegroundChecked");

					[ResourceKeyDefinition(typeof(Brush), "CheckBoxGlyphForegroundUnchecked")]
					public ThemeResourceKey<Brush> UncheckedNormal = new("CheckBoxGlyphForegroundUnchecked");

					[ResourceKeyDefinition(typeof(Brush), "CheckBoxGlyphForegroundUncheckedPointerOver")]
					public ThemeResourceKey<Brush> UncheckedPointerOver = new("CheckBoxGlyphForegroundUncheckedPointerOver");

					[ResourceKeyDefinition(typeof(Brush), "CheckBoxGlyphForegroundUncheckedPressed")]
					public ThemeResourceKey<Brush> UncheckedPressed = new("CheckBoxGlyphForegroundUncheckedPressed");

					[ResourceKeyDefinition(typeof(Brush), "CheckBoxGlyphForegroundUncheckedDisabled")]
					public ThemeResourceKey<Brush> UncheckedDisabled = new("CheckBoxGlyphForegroundUncheckedDisabled");

					[ResourceKeyDefinition(typeof(Brush), "CheckBoxGlyphForegroundCheckedPointerOver")]
					public ThemeResourceKey<Brush> CheckedPointerOver = new("CheckBoxGlyphForegroundCheckedPointerOver");

					[ResourceKeyDefinition(typeof(Brush), "CheckBoxGlyphForegroundCheckedPressed")]
					public ThemeResourceKey<Brush> CheckedPressed = new("CheckBoxGlyphForegroundCheckedPressed");

					[ResourceKeyDefinition(typeof(Brush), "CheckBoxGlyphForegroundCheckedDisabled")]
					public ThemeResourceKey<Brush> CheckedDisabled = new("CheckBoxGlyphForegroundCheckedDisabled");

					public static implicit operator ThemeResourceKey<Brush>(GlyphForegroundCheckedVSG self) => self.Default;
				}

				public static readonly GlyphForegroundIndeterminateVSG GlyphForegroundIndeterminate = new();
				public partial class GlyphForegroundIndeterminateVSG
				{
					[ResourceKeyDefinition(typeof(Brush), "CheckBoxGlyphForegroundIndeterminate")]
					public ThemeResourceKey<Brush> Default = new("CheckBoxGlyphForegroundIndeterminate");

					[ResourceKeyDefinition(typeof(Brush), "CheckBoxGlyphForegroundIndeterminatePointerOver")]
					public ThemeResourceKey<Brush> IndeterminatePointerOver = new("CheckBoxGlyphForegroundIndeterminatePointerOver");

					[ResourceKeyDefinition(typeof(Brush), "CheckBoxGlyphForegroundIndeterminatePressed")]
					public ThemeResourceKey<Brush> IndeterminatePressed = new("CheckBoxGlyphForegroundIndeterminatePressed");

					[ResourceKeyDefinition(typeof(Brush), "CheckBoxGlyphForegroundIndeterminateDisabled")]
					public ThemeResourceKey<Brush> IndeterminateDisabled = new("CheckBoxGlyphForegroundIndeterminateDisabled");

					public static implicit operator ThemeResourceKey<Brush>(GlyphForegroundIndeterminateVSG self) => self.Default;
				}

				public static readonly StateCircleFillCheckedVSG StateCircleFillChecked = new();
				public partial class StateCircleFillCheckedVSG
				{
					[ResourceKeyDefinition(typeof(Brush), "CheckBoxStateCircleFillChecked")]
					public ThemeResourceKey<Brush> Default = new("CheckBoxStateCircleFillChecked");

					[ResourceKeyDefinition(typeof(Brush), "CheckBoxStateCircleFillUnchecked")]
					public ThemeResourceKey<Brush> UncheckedPointerOver = new("CheckBoxStateCircleFillUnchecked");

					public static implicit operator ThemeResourceKey<Brush>(StateCircleFillCheckedVSG self) => self.Default;
				}

				public static partial class StateCircleOpacityPointerOver
				{
					[ResourceKeyDefinition(typeof(double), "CheckBoxStateCircleOpacityPointerOver")]
					public static ThemeResourceKey<double> UncheckedPointerOver => new("CheckBoxStateCircleOpacityPointerOver");

					[ResourceKeyDefinition(typeof(double), "CheckBoxStateCircleOpacityPressed")]
					public static ThemeResourceKey<double> UncheckedPressed => new("CheckBoxStateCircleOpacityPressed");
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

				[ResourceKeyDefinition(typeof(Brush), "CheckBoxBackgroundUnchecked")]
				public static ThemeResourceKey<Brush> BackgroundUnchecked => new("CheckBoxBackgroundUnchecked");

				[ResourceKeyDefinition(typeof(Brush), "CheckBoxBorderBrushUnchecked")]
				public static ThemeResourceKey<Brush> BorderBrushUnchecked => new("CheckBoxBorderBrushUnchecked");

				[ResourceKeyDefinition(typeof(double), "CheckBoxCheckAreaSize")]
				public static ThemeResourceKey<double> CheckAreaSize => new("CheckBoxCheckAreaSize");

				[ResourceKeyDefinition(typeof(string), "CheckBoxCheckGlyphPathData")]
				public static ThemeResourceKey<string> CheckGlyphPathData => new("CheckBoxCheckGlyphPathData");

				[ResourceKeyDefinition(typeof(double), "CheckBoxFocusAreaSize")]
				public static ThemeResourceKey<double> FocusAreaSize => new("CheckBoxFocusAreaSize");

				[ResourceKeyDefinition(typeof(Brush), "CheckBoxForegroundUnchecked")]
				public static ThemeResourceKey<Brush> ForegroundUnchecked => new("CheckBoxForegroundUnchecked");

				[ResourceKeyDefinition(typeof(string), "CheckBoxHyphenGlyphPathData")]
				public static ThemeResourceKey<string> HyphenGlyphPathData => new("CheckBoxHyphenGlyphPathData");

				[ResourceKeyDefinition(typeof(double), "CheckBoxStateCircleOpacityFocused")]
				public static ThemeResourceKey<double> StateCircleOpacityFocused => new("CheckBoxStateCircleOpacityFocused");
			}

			public static partial class Styles
			{
				[ResourceKeyDefinition(typeof(Style), "MaterialCheckBoxStyle", TargetType = typeof(global::Microsoft.UI.Xaml.Controls.CheckBox))]
				public static StaticResourceKey<Style> Default => new("MaterialCheckBoxStyle");
			}
		}
	}
}
