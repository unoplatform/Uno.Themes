using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Media;
using Uno.Extensions.Markup;
using Uno.Extensions.Markup.Internals;

namespace Uno.Themes.Markup;

public static partial class Theme
{
	public static partial class ToggleButton
	{
		public static partial class Resources
		{
			public static partial class Icon
			{
				public static partial class Background
				{
					[ResourceKeyDefinition(typeof(Brush), "IconToggleButtonBackground")]
					public static ThemeResourceKey<Brush> Default => new("IconToggleButtonBackground");

					[ResourceKeyDefinition(typeof(Brush), "IconToggleButtonBackgroundPointerOver")]
					public static ThemeResourceKey<Brush> PointerOver => new("IconToggleButtonBackgroundPointerOver");

					[ResourceKeyDefinition(typeof(Brush), "IconToggleButtonBackgroundPressed")]
					public static ThemeResourceKey<Brush> Pressed => new("IconToggleButtonBackgroundPressed");

					[ResourceKeyDefinition(typeof(Brush), "IconToggleButtonBackgroundDisabled")]
					public static ThemeResourceKey<Brush> Disabled => new("IconToggleButtonBackgroundDisabled");

					[ResourceKeyDefinition(typeof(Brush), "IconToggleButtonBackgroundChecked")]
					public static ThemeResourceKey<Brush> Checked => new("IconToggleButtonBackgroundChecked");

					[ResourceKeyDefinition(typeof(Brush), "IconToggleButtonBackgroundCheckedPointerOver")]
					public static ThemeResourceKey<Brush> CheckedPointerOver => new("IconToggleButtonBackgroundCheckedPointerOver");

					[ResourceKeyDefinition(typeof(Brush), "IconToggleButtonBackgroundCheckedPressed")]
					public static ThemeResourceKey<Brush> CheckedPressed => new("IconToggleButtonBackgroundCheckedPressed");

					[ResourceKeyDefinition(typeof(Brush), "IconToggleButtonBackgroundCheckedDisabled")]
					public static ThemeResourceKey<Brush> CheckedDisabled => new("IconToggleButtonBackgroundCheckedDisabled");

					[ResourceKeyDefinition(typeof(Brush), "IconToggleButtonBackgroundIndeterminate")]
					public static ThemeResourceKey<Brush> Indeterminate => new("IconToggleButtonBackgroundIndeterminate");

					[ResourceKeyDefinition(typeof(Brush), "IconToggleButtonBackgroundIndeterminatePointerOver")]
					public static ThemeResourceKey<Brush> IndeterminatePointerOver => new("IconToggleButtonBackgroundIndeterminatePointerOver");

					[ResourceKeyDefinition(typeof(Brush), "IconToggleButtonBackgroundIndeterminatePressed")]
					public static ThemeResourceKey<Brush> IndeterminatePressed => new("IconToggleButtonBackgroundIndeterminatePressed");

					[ResourceKeyDefinition(typeof(Brush), "IconToggleButtonBackgroundIndeterminateDisabled")]
					public static ThemeResourceKey<Brush> IndeterminateDisabled => new("IconToggleButtonBackgroundIndeterminateDisabled");
				}

				public static partial class BorderBrush
				{
					[ResourceKeyDefinition(typeof(Brush), "IconToggleButtonBorderBrush")]
					public static ThemeResourceKey<Brush> Default => new("IconToggleButtonBorderBrush");

					[ResourceKeyDefinition(typeof(Brush), "IconToggleButtonBorderBrushPointerOver")]
					public static ThemeResourceKey<Brush> PointerOver => new("IconToggleButtonBorderBrushPointerOver");

					[ResourceKeyDefinition(typeof(Brush), "IconToggleButtonBorderBrushPressed")]
					public static ThemeResourceKey<Brush> Pressed => new("IconToggleButtonBorderBrushPressed");

					[ResourceKeyDefinition(typeof(Brush), "IconToggleButtonBorderBrushDisabled")]
					public static ThemeResourceKey<Brush> Disabled => new("IconToggleButtonBorderBrushDisabled");

					[ResourceKeyDefinition(typeof(Brush), "IconToggleButtonBorderBrushChecked")]
					public static ThemeResourceKey<Brush> Checked => new("IconToggleButtonBorderBrushChecked");

					[ResourceKeyDefinition(typeof(Brush), "IconToggleButtonBorderBrushCheckedPointerOver")]
					public static ThemeResourceKey<Brush> CheckedPointerOver => new("IconToggleButtonBorderBrushCheckedPointerOver");

					[ResourceKeyDefinition(typeof(Brush), "IconToggleButtonBorderBrushCheckedPressed")]
					public static ThemeResourceKey<Brush> CheckedPressed => new("IconToggleButtonBorderBrushCheckedPressed");

					[ResourceKeyDefinition(typeof(Brush), "IconToggleButtonBorderBrushCheckedDisabled")]
					public static ThemeResourceKey<Brush> CheckedDisabled => new("IconToggleButtonBorderBrushCheckedDisabled");

					[ResourceKeyDefinition(typeof(Brush), "IconToggleButtonBorderBrushIndeterminate")]
					public static ThemeResourceKey<Brush> Indeterminate => new("IconToggleButtonBorderBrushIndeterminate");

					[ResourceKeyDefinition(typeof(Brush), "IconToggleButtonBorderBrushIndeterminatePointerOver")]
					public static ThemeResourceKey<Brush> IndeterminatePointerOver => new("IconToggleButtonBorderBrushIndeterminatePointerOver");

					[ResourceKeyDefinition(typeof(Brush), "IconToggleButtonBorderBrushIndeterminatePressed")]
					public static ThemeResourceKey<Brush> IndeterminatePressed => new("IconToggleButtonBorderBrushIndeterminatePressed");

					[ResourceKeyDefinition(typeof(Brush), "IconToggleButtonBorderBrushIndeterminateDisabled")]
					public static ThemeResourceKey<Brush> IndeterminateDisabled => new("IconToggleButtonBorderBrushIndeterminateDisabled");

					public static partial class Overlay
					{
						[ResourceKeyDefinition(typeof(Brush), "IconToggleButtonBorderBrushFocusedOverlay")]
						public static ThemeResourceKey<Brush> Focused => new("IconToggleButtonBorderBrushFocusedOverlay");

						[ResourceKeyDefinition(typeof(Brush), "IconToggleButtonBorderBrushHoverOverlay")]
						public static ThemeResourceKey<Brush> Hover => new("IconToggleButtonBorderBrushHoverOverlay");

						[ResourceKeyDefinition(typeof(Brush), "IconToggleButtonBorderBrushSelectedOverlay")]
						public static ThemeResourceKey<Brush> Selected => new("IconToggleButtonBorderBrushSelectedOverlay");
					}
				}

				public static partial class Foreground
				{
					[ResourceKeyDefinition(typeof(Brush), "IconToggleButtonForeground")]
					public static ThemeResourceKey<Brush> Default => new("IconToggleButtonForeground");

					[ResourceKeyDefinition(typeof(Brush), "IconToggleButtonForegroundPointerOver")]
					public static ThemeResourceKey<Brush> PointerOver => new("IconToggleButtonForegroundPointerOver");

					[ResourceKeyDefinition(typeof(Brush), "IconToggleButtonForegroundPressed")]
					public static ThemeResourceKey<Brush> Pressed => new("IconToggleButtonForegroundPressed");

					[ResourceKeyDefinition(typeof(Brush), "IconToggleButtonForegroundDisabled")]
					public static ThemeResourceKey<Brush> Disabled => new("IconToggleButtonForegroundDisabled");

					[ResourceKeyDefinition(typeof(Brush), "IconToggleButtonForegroundChecked")]
					public static ThemeResourceKey<Brush> Checked => new("IconToggleButtonForegroundChecked");

					[ResourceKeyDefinition(typeof(Brush), "IconToggleButtonForegroundCheckedPointerOver")]
					public static ThemeResourceKey<Brush> CheckedPointerOver => new("IconToggleButtonForegroundCheckedPointerOver");

					[ResourceKeyDefinition(typeof(Brush), "IconToggleButtonForegroundCheckedPressed")]
					public static ThemeResourceKey<Brush> CheckedPressed => new("IconToggleButtonForegroundCheckedPressed");

					[ResourceKeyDefinition(typeof(Brush), "IconToggleButtonForegroundCheckedDisabled")]
					public static ThemeResourceKey<Brush> CheckedDisabled => new("IconToggleButtonForegroundCheckedDisabled");

					[ResourceKeyDefinition(typeof(Brush), "IconToggleButtonForegroundIndeterminate")]
					public static ThemeResourceKey<Brush> Indeterminate => new("IconToggleButtonForegroundIndeterminate");

					[ResourceKeyDefinition(typeof(Brush), "IconToggleButtonForegroundIndeterminatePointerOver")]
					public static ThemeResourceKey<Brush> IndeterminatePointerOver => new("IconToggleButtonForegroundIndeterminatePointerOver");

					[ResourceKeyDefinition(typeof(Brush), "IconToggleButtonForegroundIndeterminatePressed")]
					public static ThemeResourceKey<Brush> IndeterminatePressed => new("IconToggleButtonForegroundIndeterminatePressed");

					[ResourceKeyDefinition(typeof(Brush), "IconToggleButtonForegroundIndeterminateDisabled")]
					public static ThemeResourceKey<Brush> IndeterminateDisabled => new("IconToggleButtonForegroundIndeterminateDisabled");
				}

				public static partial class StateCircle
				{
					public static partial class Opacity
					{
						[ResourceKeyDefinition(typeof(double), "IconToggleButtonStateCircleOpacityPointerOver")]
						public static ThemeResourceKey<double> PointerOver => new("IconToggleButtonStateCircleOpacityPointerOver");

						[ResourceKeyDefinition(typeof(double), "IconToggleButtonStateCircleOpacityPressed")]
						public static ThemeResourceKey<double> Pressed => new("IconToggleButtonStateCircleOpacityPressed");

						[ResourceKeyDefinition(typeof(double), "IconToggleButtonStateCircleOpacityFocused")]
						public static ThemeResourceKey<double> Focused => new("IconToggleButtonStateCircleOpacityFocused");
					}

					[ResourceKeyDefinition(typeof(Brush), "IconToggleButtonStateCircleFill")]
					public static ThemeResourceKey<Brush> Fill => new("IconToggleButtonStateCircleFill");
				}

				[ResourceKeyDefinition(typeof(Thickness), "IconToggleButtonBorderThickness")]
				public static ThemeResourceKey<Thickness> BorderThickness => new("IconToggleButtonBorderThickness");

				[ResourceKeyDefinition(typeof(double), "IconToggleButtonMinHeight")]
				public static ThemeResourceKey<double> MinHeight => new("IconToggleButtonMinHeight");

				[ResourceKeyDefinition(typeof(double), "IconToggleButtonMinWidth")]
				public static ThemeResourceKey<double> MinWidth => new("IconToggleButtonMinWidth");
			}

			public static partial class Text
			{
				public static partial class Background
				{
					[ResourceKeyDefinition(typeof(Brush), "TextToggleButtonBackground")]
					public static ThemeResourceKey<Brush> Default => new("TextToggleButtonBackground");

					[ResourceKeyDefinition(typeof(Brush), "TextToggleButtonBackgroundPointerOver")]
					public static ThemeResourceKey<Brush> PointerOver => new("TextToggleButtonBackgroundPointerOver");

					[ResourceKeyDefinition(typeof(Brush), "TextToggleButtonBackgroundPressed")]
					public static ThemeResourceKey<Brush> Pressed => new("TextToggleButtonBackgroundPressed");

					[ResourceKeyDefinition(typeof(Brush), "TextToggleButtonBackgroundDisabled")]
					public static ThemeResourceKey<Brush> Disabled => new("TextToggleButtonBackgroundDisabled");

					[ResourceKeyDefinition(typeof(Brush), "TextToggleButtonBackgroundChecked")]
					public static ThemeResourceKey<Brush> Checked => new("TextToggleButtonBackgroundChecked");

					[ResourceKeyDefinition(typeof(Brush), "TextToggleButtonBackgroundCheckedPointerOver")]
					public static ThemeResourceKey<Brush> CheckedPointerOver => new("TextToggleButtonBackgroundCheckedPointerOver");

					[ResourceKeyDefinition(typeof(Brush), "TextToggleButtonBackgroundCheckedPressed")]
					public static ThemeResourceKey<Brush> CheckedPressed => new("TextToggleButtonBackgroundCheckedPressed");

					[ResourceKeyDefinition(typeof(Brush), "TextToggleButtonBackgroundCheckedDisabled")]
					public static ThemeResourceKey<Brush> CheckedDisabled => new("TextToggleButtonBackgroundCheckedDisabled");

					[ResourceKeyDefinition(typeof(Brush), "TextToggleButtonBackgroundIndeterminate")]
					public static ThemeResourceKey<Brush> Indeterminate => new("TextToggleButtonBackgroundIndeterminate");

					[ResourceKeyDefinition(typeof(Brush), "TextToggleButtonBackgroundIndeterminatePointerOver")]
					public static ThemeResourceKey<Brush> IndeterminatePointerOver => new("TextToggleButtonBackgroundIndeterminatePointerOver");

					[ResourceKeyDefinition(typeof(Brush), "TextToggleButtonBackgroundIndeterminatePressed")]
					public static ThemeResourceKey<Brush> IndeterminatePressed => new("TextToggleButtonBackgroundIndeterminatePressed");

					[ResourceKeyDefinition(typeof(Brush), "TextToggleButtonBackgroundIndeterminateDisabled")]
					public static ThemeResourceKey<Brush> IndeterminateDisabled => new("TextToggleButtonBackgroundIndeterminateDisabled");
				}

				public static partial class BorderBrush
				{
					[ResourceKeyDefinition(typeof(Brush), "TextToggleButtonBorderBrush")]
					public static ThemeResourceKey<Brush> Default => new("TextToggleButtonBorderBrush");

					[ResourceKeyDefinition(typeof(Brush), "TextToggleButtonBorderBrushPointerOver")]
					public static ThemeResourceKey<Brush> PointerOver => new("TextToggleButtonBorderBrushPointerOver");

					[ResourceKeyDefinition(typeof(Brush), "TextToggleButtonBorderBrushPressed")]
					public static ThemeResourceKey<Brush> Pressed => new("TextToggleButtonBorderBrushPressed");

					[ResourceKeyDefinition(typeof(Brush), "TextToggleButtonBorderBrushDisabled")]
					public static ThemeResourceKey<Brush> Disabled => new("TextToggleButtonBorderBrushDisabled");

					[ResourceKeyDefinition(typeof(Brush), "TextToggleButtonBorderBrushChecked")]
					public static ThemeResourceKey<Brush> Checked => new("TextToggleButtonBorderBrushChecked");

					[ResourceKeyDefinition(typeof(Brush), "TextToggleButtonBorderBrushCheckedPointerOver")]
					public static ThemeResourceKey<Brush> CheckedPointerOver => new("TextToggleButtonBorderBrushCheckedPointerOver");

					[ResourceKeyDefinition(typeof(Brush), "TextToggleButtonBorderBrushCheckedPressed")]
					public static ThemeResourceKey<Brush> CheckedPressed => new("TextToggleButtonBorderBrushCheckedPressed");

					[ResourceKeyDefinition(typeof(Brush), "TextToggleButtonBorderBrushCheckedDisabled")]
					public static ThemeResourceKey<Brush> CheckedDisabled => new("TextToggleButtonBorderBrushCheckedDisabled");

					[ResourceKeyDefinition(typeof(Brush), "TextToggleButtonBorderBrushIndeterminate")]
					public static ThemeResourceKey<Brush> Indeterminate => new("TextToggleButtonBorderBrushIndeterminate");

					[ResourceKeyDefinition(typeof(Brush), "TextToggleButtonBorderBrushIndeterminatePointerOver")]
					public static ThemeResourceKey<Brush> IndeterminatePointerOver => new("TextToggleButtonBorderBrushIndeterminatePointerOver");

					[ResourceKeyDefinition(typeof(Brush), "TextToggleButtonBorderBrushIndeterminatePressed")]
					public static ThemeResourceKey<Brush> IndeterminatePressed => new("TextToggleButtonBorderBrushIndeterminatePressed");

					[ResourceKeyDefinition(typeof(Brush), "TextToggleButtonBorderBrushIndeterminateDisabled")]
					public static ThemeResourceKey<Brush> IndeterminateDisabled => new("TextToggleButtonBorderBrushIndeterminateDisabled");

					public static partial class Overlay
					{
						[ResourceKeyDefinition(typeof(Brush), "TextToggleButtonBorderBrushFocusedOverlay")]
						public static ThemeResourceKey<Brush> Focused => new("TextToggleButtonBorderBrushFocusedOverlay");

						[ResourceKeyDefinition(typeof(Brush), "TextToggleButtonBorderBrushHoverOverlay")]
						public static ThemeResourceKey<Brush> Hover => new("TextToggleButtonBorderBrushHoverOverlay");

						[ResourceKeyDefinition(typeof(Brush), "TextToggleButtonBorderBrushSelectedOverlay")]
						public static ThemeResourceKey<Brush> Selected => new("TextToggleButtonBorderBrushSelectedOverlay");
					}
				}

				public static partial class Foreground
				{
					[ResourceKeyDefinition(typeof(Brush), "TextToggleButtonForeground")]
					public static ThemeResourceKey<Brush> Default => new("TextToggleButtonForeground");

					[ResourceKeyDefinition(typeof(Brush), "TextToggleButtonForegroundPointerOver")]
					public static ThemeResourceKey<Brush> PointerOver => new("TextToggleButtonForegroundPointerOver");

					[ResourceKeyDefinition(typeof(Brush), "TextToggleButtonForegroundPressed")]
					public static ThemeResourceKey<Brush> Pressed => new("TextToggleButtonForegroundPressed");

					[ResourceKeyDefinition(typeof(Brush), "TextToggleButtonForegroundDisabled")]
					public static ThemeResourceKey<Brush> Disabled => new("TextToggleButtonForegroundDisabled");

					[ResourceKeyDefinition(typeof(Brush), "TextToggleButtonForegroundChecked")]
					public static ThemeResourceKey<Brush> Checked => new("TextToggleButtonForegroundChecked");

					[ResourceKeyDefinition(typeof(Brush), "TextToggleButtonForegroundCheckedPointerOver")]
					public static ThemeResourceKey<Brush> CheckedPointerOver => new("TextToggleButtonForegroundCheckedPointerOver");

					[ResourceKeyDefinition(typeof(Brush), "TextToggleButtonForegroundCheckedPressed")]
					public static ThemeResourceKey<Brush> CheckedPressed => new("TextToggleButtonForegroundCheckedPressed");

					[ResourceKeyDefinition(typeof(Brush), "TextToggleButtonForegroundCheckedDisabled")]
					public static ThemeResourceKey<Brush> CheckedDisabled => new("TextToggleButtonForegroundCheckedDisabled");

					[ResourceKeyDefinition(typeof(Brush), "TextToggleButtonForegroundIndeterminate")]
					public static ThemeResourceKey<Brush> Indeterminate => new("TextToggleButtonForegroundIndeterminate");

					[ResourceKeyDefinition(typeof(Brush), "TextToggleButtonForegroundIndeterminatePointerOver")]
					public static ThemeResourceKey<Brush> IndeterminatePointerOver => new("TextToggleButtonForegroundIndeterminatePointerOver");

					[ResourceKeyDefinition(typeof(Brush), "TextToggleButtonForegroundIndeterminatePressed")]
					public static ThemeResourceKey<Brush> IndeterminatePressed => new("TextToggleButtonForegroundIndeterminatePressed");

					[ResourceKeyDefinition(typeof(Brush), "TextToggleButtonForegroundIndeterminateDisabled")]
					public static ThemeResourceKey<Brush> IndeterminateDisabled => new("TextToggleButtonForegroundIndeterminateDisabled");
				}

				public static partial class Typography
				{
					[ResourceKeyDefinition(typeof(FontFamily), "TextToggleButtonFontFamily")]
					public static ThemeResourceKey<FontFamily> FontFamily => new("TextToggleButtonFontFamily");

					[ResourceKeyDefinition(typeof(double), "TextToggleButtonFontSize")]
					public static ThemeResourceKey<double> FontSize => new("TextToggleButtonFontSize");
				}

				[ResourceKeyDefinition(typeof(Thickness), "TextToggleButtonBorderThickness")]
				public static ThemeResourceKey<Thickness> BorderThickness => new("TextToggleButtonBorderThickness");

				[ResourceKeyDefinition(typeof(CornerRadius), "TextToggleButtonCornerRadius")]
				public static ThemeResourceKey<CornerRadius> CornerRadius => new("TextToggleButtonCornerRadius");

				[ResourceKeyDefinition(typeof(Brush), "TextToggleButtonFeedbackFocused")]
				public static ThemeResourceKey<Brush> FeedbackFocused => new("TextToggleButtonFeedbackFocused");

				[ResourceKeyDefinition(typeof(double), "TextToggleButtonMinHeight")]
				public static ThemeResourceKey<double> MinHeight => new("TextToggleButtonMinHeight");

				[ResourceKeyDefinition(typeof(Thickness), "TextToggleButtonPadding")]
				public static ThemeResourceKey<Thickness> Padding => new("TextToggleButtonPadding");
			}
		}

		public static partial class Styles
		{
			[ResourceKeyDefinition(typeof(Style), "TextToggleButtonStyle", TargetType = typeof(global::Microsoft.UI.Xaml.Controls.Primitives.ToggleButton))]
			public static StaticResourceKey<Style> Text => new("TextToggleButtonStyle");

			[ResourceKeyDefinition(typeof(Style), "IconToggleButtonStyle", TargetType = typeof(global::Microsoft.UI.Xaml.Controls.Primitives.ToggleButton))]
			public static StaticResourceKey<Style> Icon => new("IconToggleButtonStyle");
		}
	}
}
