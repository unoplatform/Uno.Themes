using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Media;
using Uno.Extensions.Markup;
using Uno.Extensions.Markup.Internals;

namespace Uno.Themes.Markup;

public static partial class Theme
{
	public static class PipsPager
	{
		public static partial class Resources
		{
			public static partial class Default
			{
				public static partial class SelectionIndicator
				{
					public static partial class Background
					{
						[ResourceKeyDefinition(typeof(Brush), "PipsPagerSelectionIndicatorBackground")]
						public static ThemeResourceKey<Brush> Default => new("PipsPagerSelectionIndicatorBackground");

						[ResourceKeyDefinition(typeof(Brush), "PipsPagerSelectionIndicatorBackgroundPointerOver")]
						public static ThemeResourceKey<Brush> PointerOver => new("PipsPagerSelectionIndicatorBackgroundPointerOver");

						[ResourceKeyDefinition(typeof(Brush), "PipsPagerSelectionIndicatorBackgroundPressed")]
						public static ThemeResourceKey<Brush> Pressed => new("PipsPagerSelectionIndicatorBackgroundPressed");

						[ResourceKeyDefinition(typeof(Brush), "PipsPagerSelectionIndicatorBackgroundSelected")]
						public static ThemeResourceKey<Brush> Selected => new("PipsPagerSelectionIndicatorBackgroundSelected");

						[ResourceKeyDefinition(typeof(Brush), "PipsPagerSelectionIndicatorBackgroundDisabled")]
						public static ThemeResourceKey<Brush> Disabled => new("PipsPagerSelectionIndicatorBackgroundDisabled");
					}

					public static partial class Foreground
					{
						[ResourceKeyDefinition(typeof(Brush), "PipsPagerSelectionIndicatorForeground")]
						public static ThemeResourceKey<Brush> Default => new("PipsPagerSelectionIndicatorForeground");

						[ResourceKeyDefinition(typeof(Brush), "PipsPagerSelectionIndicatorForegroundPointerOver")]
						public static ThemeResourceKey<Brush> PointerOver => new("PipsPagerSelectionIndicatorForegroundPointerOver");

						[ResourceKeyDefinition(typeof(Brush), "PipsPagerSelectionIndicatorForegroundPressed")]
						public static ThemeResourceKey<Brush> Pressed => new("PipsPagerSelectionIndicatorForegroundPressed");

						[ResourceKeyDefinition(typeof(Brush), "PipsPagerSelectionIndicatorForegroundSelected")]
						public static ThemeResourceKey<Brush> Selected => new("PipsPagerSelectionIndicatorForegroundSelected");

						[ResourceKeyDefinition(typeof(Brush), "PipsPagerSelectionIndicatorForegroundFocused")]
						public static ThemeResourceKey<Brush> Focused => new("PipsPagerSelectionIndicatorForegroundFocused");

						[ResourceKeyDefinition(typeof(Brush), "PipsPagerSelectionIndicatorForegroundDisabled")]
						public static ThemeResourceKey<Brush> Disabled => new("PipsPagerSelectionIndicatorForegroundDisabled");
					}

					public static partial class BorderBrush
					{
						[ResourceKeyDefinition(typeof(Brush), "PipsPagerSelectionIndicatorBorderBrush")]
						public static ThemeResourceKey<Brush> Default => new("PipsPagerSelectionIndicatorBorderBrush");

						[ResourceKeyDefinition(typeof(Brush), "PipsPagerSelectionIndicatorBorderBrushPointerOver")]
						public static ThemeResourceKey<Brush> PointerOver => new("PipsPagerSelectionIndicatorBorderBrushPointerOver");

						[ResourceKeyDefinition(typeof(Brush), "PipsPagerSelectionIndicatorBorderBrushPressed")]
						public static ThemeResourceKey<Brush> Pressed => new("PipsPagerSelectionIndicatorBorderBrushPressed");

						[ResourceKeyDefinition(typeof(Brush), "PipsPagerSelectionIndicatorBorderBrushSelected")]
						public static ThemeResourceKey<Brush> Selected => new("PipsPagerSelectionIndicatorBorderBrushSelected");

						[ResourceKeyDefinition(typeof(Brush), "PipsPagerSelectionIndicatorBorderBrushDisabled")]
						public static ThemeResourceKey<Brush> Disabled => new("PipsPagerSelectionIndicatorBorderBrushDisabled");
					}
				}

				public static partial class NavigationButton
				{
					public static partial class Background
					{
						[ResourceKeyDefinition(typeof(Brush), "PipsPagerNavigationButtonBackground")]
						public static ThemeResourceKey<Brush> Default => new("PipsPagerNavigationButtonBackground");

						[ResourceKeyDefinition(typeof(Brush), "PipsPagerNavigationButtonBackgroundPointerOver")]
						public static ThemeResourceKey<Brush> PointerOver => new("PipsPagerNavigationButtonBackgroundPointerOver");

						[ResourceKeyDefinition(typeof(Brush), "PipsPagerNavigationButtonBackgroundPressed")]
						public static ThemeResourceKey<Brush> Pressed => new("PipsPagerNavigationButtonBackgroundPressed");

						[ResourceKeyDefinition(typeof(Brush), "PipsPagerNavigationButtonBackgroundDisabled")]
						public static ThemeResourceKey<Brush> Disabled => new("PipsPagerNavigationButtonBackgroundDisabled");
					}

					public static partial class Foreground
					{
						[ResourceKeyDefinition(typeof(Brush), "PipsPagerNavigationButtonForeground")]
						public static ThemeResourceKey<Brush> Default => new("PipsPagerNavigationButtonForeground");

						[ResourceKeyDefinition(typeof(Brush), "PipsPagerNavigationButtonForegroundPointerOver")]
						public static ThemeResourceKey<Brush> PointerOver => new("PipsPagerNavigationButtonForegroundPointerOver");

						[ResourceKeyDefinition(typeof(Brush), "PipsPagerNavigationButtonForegroundPressed")]
						public static ThemeResourceKey<Brush> Pressed => new("PipsPagerNavigationButtonForegroundPressed");

						[ResourceKeyDefinition(typeof(Brush), "PipsPagerNavigationButtonForegroundDisabled")]
						public static ThemeResourceKey<Brush> Disabled => new("PipsPagerNavigationButtonForegroundDisabled");
					}

					public static partial class BorderBrush
					{
						[ResourceKeyDefinition(typeof(Brush), "PipsPagerNavigationButtonBorderBrush")]
						public static ThemeResourceKey<Brush> Default => new("PipsPagerNavigationButtonBorderBrush");

						[ResourceKeyDefinition(typeof(Brush), "PipsPagerNavigationButtonBorderBrushPointerOver")]
						public static ThemeResourceKey<Brush> PointerOver => new("PipsPagerNavigationButtonBorderBrushPointerOver");

						[ResourceKeyDefinition(typeof(Brush), "PipsPagerNavigationButtonBorderBrushPressed")]
						public static ThemeResourceKey<Brush> Pressed => new("PipsPagerNavigationButtonBorderBrushPressed");

						[ResourceKeyDefinition(typeof(Brush), "PipsPagerNavigationButtonBorderBrushDisabled")]
						public static ThemeResourceKey<Brush> Disabled => new("PipsPagerNavigationButtonBorderBrushDisabled");
					}

					[ResourceKeyDefinition(typeof(Thickness), "PipsPagerNavigationButtonBorderThickness")]
					public static ThemeResourceKey<Thickness> BorderThickness => new("PipsPagerNavigationButtonBorderThickness");

					[ResourceKeyDefinition(typeof(double), "PipsPagerNavigationButtonHeight")]
					public static ThemeResourceKey<double> Height => new("PipsPagerNavigationButtonHeight");

					[ResourceKeyDefinition(typeof(double), "PipsPagerNavigationButtonWidth")]
					public static ThemeResourceKey<double> Width => new("PipsPagerNavigationButtonWidth");

					[ResourceKeyDefinition(typeof(Thickness), "PipsPagerNavigationButtonPadding")]
					public static ThemeResourceKey<Thickness> Padding => new("PipsPagerNavigationButtonPadding");

					[ResourceKeyDefinition(typeof(double), "PipsPagerNavigationVisualStatesEllipseWidth")]
					public static ThemeResourceKey<double> VisualStatesEllipseWidth => new("PipsPagerNavigationVisualStatesEllipseWidth");

					[ResourceKeyDefinition(typeof(double), "PipsPagerNavigationVisualStatesEllipseHeight")]
					public static ThemeResourceKey<double> VisualStatesEllipseHeight => new("PipsPagerNavigationVisualStatesEllipseHeight");
				}

				[ResourceKeyDefinition(typeof(double), "PipsPagerSelectedEllipseSize")]
				public static ThemeResourceKey<double> SelectedEllipseSize => new("PipsPagerSelectedEllipseSize");

				[ResourceKeyDefinition(typeof(double), "PipsPagerNormalEllipseSize")]
				public static ThemeResourceKey<double> NormalEllipseSize => new("PipsPagerNormalEllipseSize");

				[ResourceKeyDefinition(typeof(double), "PipsPagerPreviousPageButtonData")]
				public static ThemeResourceKey<double> PreviousPageButtonData => new("PipsPagerPreviousPageButtonData");

				[ResourceKeyDefinition(typeof(double), "PipsPagerNextPageButtonData")]
				public static ThemeResourceKey<double> NextPageButtonData => new("PipsPagerNextPageButtonData");

				[ResourceKeyDefinition(typeof(Thickness), "PipsPagerButtonBorderThickness")]
				public static ThemeResourceKey<Thickness> ButtonBorderThickness => new("PipsPagerButtonBorderThickness");

				[ResourceKeyDefinition(typeof(double), "PipsPagerHorizontalOrientationButtonWidth")]
				public static ThemeResourceKey<double> HorizontalOrientationButtonWidth => new("PipsPagerHorizontalOrientationButtonWidth");

				[ResourceKeyDefinition(typeof(double), "PipsPagerHorizontalOrientationButtonHeight")]
				public static ThemeResourceKey<double> HorizontalOrientationButtonHeight => new("PipsPagerHorizontalOrientationButtonHeight");

				[ResourceKeyDefinition(typeof(double), "PipsPagerVerticalOrientationButtonWidth")]
				public static ThemeResourceKey<double> VerticalOrientationButtonWidth => new("PipsPagerVerticalOrientationButtonWidth");

				[ResourceKeyDefinition(typeof(double), "PipsPagerVerticalOrientationButtonHeight")]
				public static ThemeResourceKey<double> VerticalOrientationButtonHeight => new("PipsPagerVerticalOrientationButtonHeight");
			}
		}
		public static partial class Styles
		{
			[ResourceKeyDefinition(typeof(Style), "PipsPagerStyle", TargetType = typeof(PipsPager))]
			public static StaticResourceKey<Style> Default => new("PipsPagerStyle");
		}
	}
}
