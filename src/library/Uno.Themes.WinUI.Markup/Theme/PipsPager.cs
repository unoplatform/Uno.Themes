using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Media;
using Uno.Extensions.Markup;
using Uno.Extensions.Markup.Internals;
using Windows.Devices.Bluetooth.Advertisement;

namespace Uno.Themes.Markup
{
	public static partial class Theme
	{
		public static class PipsPager
		{
			public static class Resources
			{
				public static class SelectionIndicator
				{
					public static class Foreground
					{
						[ResourceKeyDefinition(typeof(Brush), "PipsPagerSelectionIndicatorForeground")]
						public static ThemeResourceKey<Brush> Default => new("PipsPagerSelectionIndicatorForeground");

						[ResourceKeyDefinition(typeof(Brush), "PipsPagerSelectionIndicatorForegroundPointerOver")]
						public static ThemeResourceKey<Brush> PointerOver => new("PipsPagerSelectionIndicatorForegroundPointerOver");

						[ResourceKeyDefinition(typeof(Brush), "PipsPagerSelectionIndicatorForegroundPressed")]
						public static ThemeResourceKey<Brush> Pressed => new("PipsPagerSelectionIndicatorForegroundPressed");

						[ResourceKeyDefinition(typeof(Brush), "PipsPagerSelectionIndicatorForegroundDisabled")]
						public static ThemeResourceKey<Brush> Disabled => new("PipsPagerSelectionIndicatorForegroundDisabled");

						[ResourceKeyDefinition(typeof(Brush), "PipsPagerSelectionIndicatorForegroundFocused")]
						public static ThemeResourceKey<Brush> Focused => new("PipsPagerSelectionIndicatorForegroundFocused");

						[ResourceKeyDefinition(typeof(Brush), "PipsPagerSelectionIndicatorForegroundSelected")]
						public static ThemeResourceKey<Brush> Selected => new("PipsPagerSelectionIndicatorForegroundSelected");
					}

					public static class Background
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

					public static class BorderBrush
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

					public static class VisualStateEllipse
					{
						public static class Fill
						{
							[ResourceKeyDefinition(typeof(Brush), "MaterialPipsPagerVisualStateEllipseFill")]
							public static ThemeResourceKey<Brush> Default => new("MaterialPipsPagerVisualStateEllipseFill");

							[ResourceKeyDefinition(typeof(Brush), "MaterialPipsPagerVisualStateEllipseFillPointerOver")]
							public static ThemeResourceKey<Brush> PointerOver => new("MaterialPipsPagerVisualStateEllipseFillPointerOver");

							[ResourceKeyDefinition(typeof(Brush), "MaterialPipsPagerVisualStateEllipseFillPressed")]
							public static ThemeResourceKey<Brush> Pressed => new("MaterialPipsPagerVisualStateEllipseFillPressed");

							[ResourceKeyDefinition(typeof(Brush), "MaterialPipsPagerVisualStateEllipseFillFocused")]
							public static ThemeResourceKey<Brush> Focused => new("MaterialPipsPagerVisualStateEllipseFillFocused");
						}
					}

					[ResourceKeyDefinition(typeof(double), "PipsPagerHorizontalOrientationButtonWidth")]
					public static ThemeResourceKey<double> HorizontalOrientationWidth => new("PipsPagerHorizontalOrientationButtonWidth");

					[ResourceKeyDefinition(typeof(double), "PipsPagerVerticalOrientationButtonWidth")]
					public static ThemeResourceKey<double> VerticalOrientationWidth => new("PipsPagerVerticalOrientationButtonWidth");

					[ResourceKeyDefinition(typeof(double), "PipsPagerHorizontalOrientationButtonHeight")]
					public static ThemeResourceKey<double> HorizontalOrientationHeight => new("PipsPagerHorizontalOrientationButtonHeight");

					[ResourceKeyDefinition(typeof(double), "PipsPagerVerticalOrientationButtonHeight")]
					public static ThemeResourceKey<double> VerticalOrientationHeight => new("PipsPagerVerticalOrientationButtonHeight");

					[ResourceKeyDefinition(typeof(Thickness), "PipsPagerButtonBorderThickness")]
					public static ThemeResourceKey<Thickness> BorderThickness => new("PipsPagerButtonBorderThickness");
				}

				public static class NavigationButton
				{
					public static class Background
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

					public static class BorderBrush
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

					public static class Foreground
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

					public static class VisualStateEllipse
					{

						[ResourceKeyDefinition(typeof(double), "PipsPagerNavigationVisualStatesEllipseHeight")]
						public static ThemeResourceKey<double> Height => new("PipsPagerNavigationVisualStatesEllipseHeight");


						[ResourceKeyDefinition(typeof(double), "PipsPagerNavigationVisualStatesEllipseWidth")]
						public static ThemeResourceKey<double> Width => new("PipsPagerNavigationVisualStatesEllipseWidth");

						public static class Fill
						{
							[ResourceKeyDefinition(typeof(Brush), "MaterialPipsPagerNavigationButtonVisualStateEllipseFill")]
							public static ThemeResourceKey<Brush> Default => new("MaterialPipsPagerNavigationButtonVisualStateEllipseFill");

							[ResourceKeyDefinition(typeof(Brush), "MaterialPipsPagerNavigationButtonVisualStateEllipseFillPointerOver")]
							public static ThemeResourceKey<Brush> PointerOver => new("MaterialPipsPagerNavigationButtonVisualStateEllipseFillPointerOver");

							[ResourceKeyDefinition(typeof(Brush), "MaterialPipsPagerNavigationButtonVisualStateEllipseFillPressed")]
							public static ThemeResourceKey<Brush> Pressed => new("MaterialPipsPagerNavigationButtonVisualStateEllipseFillPressed");

							[ResourceKeyDefinition(typeof(Brush), "MaterialPipsPagerNavigationButtonVisualStateEllipseFillFocused")]
							public static ThemeResourceKey<Brush> Focused => new("MaterialPipsPagerNavigationButtonVisualStateEllipseFillFocused");

							[ResourceKeyDefinition(typeof(Brush), "MaterialPipsPagerNavigationButtonVisualStateEllipseFillUnfocused")]
							public static ThemeResourceKey<Brush> Unfocused => new("MaterialPipsPagerNavigationButtonVisualStateEllipseFillUnfocused");
						}
					}

					[ResourceKeyDefinition(typeof(Geometry), "PipsPagerPreviousPageButtonData")]
					public static ThemeResourceKey<Geometry> PreviousPageData => new("PipsPagerPreviousPageButtonData");


					[ResourceKeyDefinition(typeof(Geometry), "PipsPagerNextPageButtonData")]
					public static ThemeResourceKey<Geometry> NextPageData => new("PipsPagerNextPageButtonData");


					[ResourceKeyDefinition(typeof(double), "PipsPagerNavigationButtonHeight")]
					public static ThemeResourceKey<double> Height => new("PipsPagerNavigationButtonHeight");


					[ResourceKeyDefinition(typeof(double), "PipsPagerNavigationButtonWidth")]
					public static ThemeResourceKey<double> Width => new("PipsPagerNavigationButtonWidth");

					[ResourceKeyDefinition(typeof(Thickness), "PipsPagerNavigationButtonBorderThickness")]
					public static ThemeResourceKey<Thickness> BorderThickness => new("PipsPagerNavigationButtonBorderThickness");

					[ResourceKeyDefinition(typeof(Thickness), "PipsPagerNavigationButtonPadding")]
					public static ThemeResourceKey<Thickness> Padding => new("PipsPagerNavigationButtonPadding");
				}

				[ResourceKeyDefinition(typeof(double), "PipsPagerNormalEllipseSize")]
				public static ThemeResourceKey<double> NormalEllipseSize => new("PipsPagerNormalEllipseSize");

				[ResourceKeyDefinition(typeof(double), "PipsPagerSelectedEllipseSize")]
				public static ThemeResourceKey<double> SelectedEllipseSize => new("PipsPagerSelectedEllipseSize");
			}

			public static class Styles
			{
				[ResourceKeyDefinition(typeof(Style), "PipsPagerStyle", TargetType = typeof(PipsPager))]
				public static StaticResourceKey<Style> Default => new("PipsPagerStyle");
			}
		}
	}
}


