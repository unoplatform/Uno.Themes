using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.MarkupHelpers;
using Microsoft.UI.Xaml.Media;

namespace Uno.Themes.Markup
{
	partial class Theme
	{
		public static class Brushes
		{
			public delegate void BrushDependencyPropertyBuilder(IDependencyPropertyBuilder<Brush> builder);
			private static BrushDependencyPropertyBuilder GetBrushBuilder(string key) => delegate { StaticResource.Get<Brush>(key); };

			public static class Primary
			{
				public static BrushDependencyPropertyBuilder Default => GetBrushBuilder("PrimaryBrush");
				public static BrushDependencyPropertyBuilder Hover => GetBrushBuilder("PrimaryHoverBrush");
				public static BrushDependencyPropertyBuilder Focused => GetBrushBuilder("PrimaryFocusedBrush");
				public static BrushDependencyPropertyBuilder Pressed => GetBrushBuilder("PrimaryPressedBrush");
				public static BrushDependencyPropertyBuilder Dragged => GetBrushBuilder("PrimaryDraggedBrush");
				public static BrushDependencyPropertyBuilder Selected => GetBrushBuilder("PrimarySelectedBrush");
				public static BrushDependencyPropertyBuilder Medium => GetBrushBuilder("PrimaryMediumBrush");
				public static BrushDependencyPropertyBuilder Low => GetBrushBuilder("PrimaryLowBrush");
				public static BrushDependencyPropertyBuilder Disabled => GetBrushBuilder("PrimaryDisabledBrush");
				public static BrushDependencyPropertyBuilder DisabledLow => GetBrushBuilder("PrimaryDisabledLowBrush");

				public static class Inverse
				{
					public static BrushDependencyPropertyBuilder Default => GetBrushBuilder("PrimaryInverseBrush");
					public static BrushDependencyPropertyBuilder Hover => GetBrushBuilder("PrimaryInverseHoverBrush");
					public static BrushDependencyPropertyBuilder Focused => GetBrushBuilder("PrimaryInverseFocusedBrush");
					public static BrushDependencyPropertyBuilder Pressed => GetBrushBuilder("PrimaryInversePressedBrush");
					public static BrushDependencyPropertyBuilder Dragged => GetBrushBuilder("PrimaryInverseDraggedBrush");
					public static BrushDependencyPropertyBuilder Selected => GetBrushBuilder("PrimaryInverseSelectedBrush");
					public static BrushDependencyPropertyBuilder Medium => GetBrushBuilder("PrimaryInverseMediumBrush");
					public static BrushDependencyPropertyBuilder Low => GetBrushBuilder("PrimaryInverseLowBrush");
					public static BrushDependencyPropertyBuilder Disabled => GetBrushBuilder("PrimaryInverseDisabledBrush");
					public static BrushDependencyPropertyBuilder DisabledLow => GetBrushBuilder("PrimaryInverseDisabledLowBrush");
				}

				public static class Container
				{
					public static BrushDependencyPropertyBuilder Default => GetBrushBuilder("PrimaryContainerBrush");
					public static BrushDependencyPropertyBuilder Hover => GetBrushBuilder("PrimaryContainerHoverBrush");
					public static BrushDependencyPropertyBuilder Focused => GetBrushBuilder("PrimaryContainerFocusedBrush");
					public static BrushDependencyPropertyBuilder Pressed => GetBrushBuilder("PrimaryContainerPressedBrush");
					public static BrushDependencyPropertyBuilder Dragged => GetBrushBuilder("PrimaryContainerDraggedBrush");
					public static BrushDependencyPropertyBuilder Selected => GetBrushBuilder("PrimaryContainerSelectedBrush");
					public static BrushDependencyPropertyBuilder Medium => GetBrushBuilder("PrimaryContainerMediumBrush");
					public static BrushDependencyPropertyBuilder Low => GetBrushBuilder("PrimaryContainerLowBrush");
					public static BrushDependencyPropertyBuilder Disabled => GetBrushBuilder("PrimaryContainerDisabledBrush");
					public static BrushDependencyPropertyBuilder DisabledLow => GetBrushBuilder("PrimaryContainerDisabledLowBrush");
				}

				public static class VariantLight
				{
					public static BrushDependencyPropertyBuilder Default => GetBrushBuilder("PrimaryVariantLightBrush");
					public static BrushDependencyPropertyBuilder Hover => GetBrushBuilder("PrimaryVariantLightHoverBrush");
					public static BrushDependencyPropertyBuilder Focused => GetBrushBuilder("PrimaryVariantLightFocusedBrush");
					public static BrushDependencyPropertyBuilder Pressed => GetBrushBuilder("PrimaryVariantLightPressedBrush");
					public static BrushDependencyPropertyBuilder Dragged => GetBrushBuilder("PrimaryVariantLightDraggedBrush");
					public static BrushDependencyPropertyBuilder Selected => GetBrushBuilder("PrimaryVariantLightSelectedBrush");
					public static BrushDependencyPropertyBuilder Medium => GetBrushBuilder("PrimaryVariantLightMediumBrush");
					public static BrushDependencyPropertyBuilder Low => GetBrushBuilder("PrimaryVariantLightLowBrush");
					public static BrushDependencyPropertyBuilder Disabled => GetBrushBuilder("PrimaryVariantLightDisabledBrush");
					public static BrushDependencyPropertyBuilder DisabledLow => GetBrushBuilder("PrimaryVariantLightDisabledLowBrush");
				}

				public static class VariantDark
				{
					public static BrushDependencyPropertyBuilder Default => GetBrushBuilder("PrimaryVariantDarkBrush");
					public static BrushDependencyPropertyBuilder Hover => GetBrushBuilder("PrimaryVariantDarkHoverBrush");
					public static BrushDependencyPropertyBuilder Focused => GetBrushBuilder("PrimaryVariantDarkFocusedBrush");
					public static BrushDependencyPropertyBuilder Pressed => GetBrushBuilder("PrimaryVariantDarkPressedBrush");
					public static BrushDependencyPropertyBuilder Dragged => GetBrushBuilder("PrimaryVariantDarkDraggedBrush");
					public static BrushDependencyPropertyBuilder Selected => GetBrushBuilder("PrimaryVariantDarkSelectedBrush");
					public static BrushDependencyPropertyBuilder Medium => GetBrushBuilder("PrimaryVariantDarkMediumBrush");
					public static BrushDependencyPropertyBuilder Low => GetBrushBuilder("PrimaryVariantDarkLowBrush");
					public static BrushDependencyPropertyBuilder Disabled => GetBrushBuilder("PrimaryVariantDarkDisabledBrush");
					public static BrushDependencyPropertyBuilder DisabledLow => GetBrushBuilder("PrimaryVariantDarkDisabledLowBrush");
				}
			}

			public static class OnPrimary
			{
				public static BrushDependencyPropertyBuilder Default => GetBrushBuilder("OnPrimaryBrush");
				public static BrushDependencyPropertyBuilder Hover => GetBrushBuilder("OnPrimaryHoverBrush");
				public static BrushDependencyPropertyBuilder Focused => GetBrushBuilder("OnPrimaryFocusedBrush");
				public static BrushDependencyPropertyBuilder Pressed => GetBrushBuilder("OnPrimaryPressedBrush");
				public static BrushDependencyPropertyBuilder Dragged => GetBrushBuilder("OnPrimaryDraggedBrush");
				public static BrushDependencyPropertyBuilder Selected => GetBrushBuilder("OnPrimarySelectedBrush");
				public static BrushDependencyPropertyBuilder Medium => GetBrushBuilder("OnPrimaryMediumBrush");
				public static BrushDependencyPropertyBuilder Low => GetBrushBuilder("OnPrimaryLowBrush");
				public static BrushDependencyPropertyBuilder Disabled => GetBrushBuilder("OnPrimaryDisabledBrush");
				public static BrushDependencyPropertyBuilder DisabledLow => GetBrushBuilder("OnPrimaryDisabledLowBrush");

				public static class Container
				{
					public static BrushDependencyPropertyBuilder Default => GetBrushBuilder("OnPrimaryContainerBrush");
					public static BrushDependencyPropertyBuilder Hover => GetBrushBuilder("OnPrimaryContainerHoverBrush");
					public static BrushDependencyPropertyBuilder Focused => GetBrushBuilder("OnPrimaryContainerFocusedBrush");
					public static BrushDependencyPropertyBuilder Pressed => GetBrushBuilder("OnPrimaryContainerPressedBrush");
					public static BrushDependencyPropertyBuilder Dragged => GetBrushBuilder("OnPrimaryContainerDraggedBrush");
					public static BrushDependencyPropertyBuilder Selected => GetBrushBuilder("OnPrimaryContainerSelectedBrush");
					public static BrushDependencyPropertyBuilder Medium => GetBrushBuilder("OnPrimaryContainerMediumBrush");
					public static BrushDependencyPropertyBuilder Low => GetBrushBuilder("OnPrimaryContainerLowBrush");
					public static BrushDependencyPropertyBuilder Disabled => GetBrushBuilder("OnPrimaryContainerDisabledBrush");
					public static BrushDependencyPropertyBuilder DisabledLow => GetBrushBuilder("OnPrimaryContainerDisabledLowBrush");
				}
			}

			public static class Secondary
			{
				public static BrushDependencyPropertyBuilder Default => GetBrushBuilder("SecondaryBrush");
				public static BrushDependencyPropertyBuilder Hover => GetBrushBuilder("SecondaryHoverBrush");
				public static BrushDependencyPropertyBuilder Focused => GetBrushBuilder("SecondaryFocusedBrush");
				public static BrushDependencyPropertyBuilder Pressed => GetBrushBuilder("SecondaryPressedBrush");
				public static BrushDependencyPropertyBuilder Dragged => GetBrushBuilder("SecondaryDraggedBrush");
				public static BrushDependencyPropertyBuilder Selected => GetBrushBuilder("SecondarySelectedBrush");
				public static BrushDependencyPropertyBuilder Medium => GetBrushBuilder("SecondaryMediumBrush");
				public static BrushDependencyPropertyBuilder Low => GetBrushBuilder("SecondaryLowBrush");
				public static BrushDependencyPropertyBuilder Disabled => GetBrushBuilder("SecondaryDisabledBrush");
				public static BrushDependencyPropertyBuilder DisabledLow => GetBrushBuilder("SecondaryDisabledLowBrush");

				public static class Container
				{
					public static BrushDependencyPropertyBuilder Default => GetBrushBuilder("SecondaryContainerBrush");
					public static BrushDependencyPropertyBuilder Hover => GetBrushBuilder("SecondaryContainerHoverBrush");
					public static BrushDependencyPropertyBuilder Focused => GetBrushBuilder("SecondaryContainerFocusedBrush");
					public static BrushDependencyPropertyBuilder Pressed => GetBrushBuilder("SecondaryContainerPressedBrush");
					public static BrushDependencyPropertyBuilder Dragged => GetBrushBuilder("SecondaryContainerDraggedBrush");
					public static BrushDependencyPropertyBuilder Selected => GetBrushBuilder("SecondaryContainerSelectedBrush");
					public static BrushDependencyPropertyBuilder Medium => GetBrushBuilder("SecondaryContainerMediumBrush");
					public static BrushDependencyPropertyBuilder Low => GetBrushBuilder("SecondaryContainerLowBrush");
					public static BrushDependencyPropertyBuilder Disabled => GetBrushBuilder("SecondaryContainerDisabledBrush");
					public static BrushDependencyPropertyBuilder DisabledLow => GetBrushBuilder("SecondaryContainerDisabledLowBrush");
				}

				public static class VariantLight
				{
					public static BrushDependencyPropertyBuilder Default => GetBrushBuilder("SecondaryVariantLightBrush");
					public static BrushDependencyPropertyBuilder Hover => GetBrushBuilder("SecondaryVariantLightHoverBrush");
					public static BrushDependencyPropertyBuilder Focused => GetBrushBuilder("SecondaryVariantLightFocusedBrush");
					public static BrushDependencyPropertyBuilder Pressed => GetBrushBuilder("SecondaryVariantLightPressedBrush");
					public static BrushDependencyPropertyBuilder Dragged => GetBrushBuilder("SecondaryVariantLightDraggedBrush");
					public static BrushDependencyPropertyBuilder Selected => GetBrushBuilder("SecondaryVariantLightSelectedBrush");
					public static BrushDependencyPropertyBuilder Medium => GetBrushBuilder("SecondaryVariantLightMediumBrush");
					public static BrushDependencyPropertyBuilder Low => GetBrushBuilder("SecondaryVariantLightLowBrush");
					public static BrushDependencyPropertyBuilder Disabled => GetBrushBuilder("SecondaryVariantLightDisabledBrush");
					public static BrushDependencyPropertyBuilder DisabledLow => GetBrushBuilder("SecondaryVariantLightDisabledLowBrush");
				}

				public static class VariantDark
				{
					public static BrushDependencyPropertyBuilder Default => GetBrushBuilder("SecondaryVariantDarkBrush");
					public static BrushDependencyPropertyBuilder Hover => GetBrushBuilder("SecondaryVariantDarkHoverBrush");
					public static BrushDependencyPropertyBuilder Focused => GetBrushBuilder("SecondaryVariantDarkFocusedBrush");
					public static BrushDependencyPropertyBuilder Pressed => GetBrushBuilder("SecondaryVariantDarkPressedBrush");
					public static BrushDependencyPropertyBuilder Dragged => GetBrushBuilder("SecondaryVariantDarkDraggedBrush");
					public static BrushDependencyPropertyBuilder Selected => GetBrushBuilder("SecondaryVariantDarkSelectedBrush");
					public static BrushDependencyPropertyBuilder Medium => GetBrushBuilder("SecondaryVariantDarkMediumBrush");
					public static BrushDependencyPropertyBuilder Low => GetBrushBuilder("SecondaryVariantDarkLowBrush");
					public static BrushDependencyPropertyBuilder Disabled => GetBrushBuilder("SecondaryVariantDarkDisabledBrush");
					public static BrushDependencyPropertyBuilder DisabledLow => GetBrushBuilder("SecondaryVariantDarkDisabledLowBrush");
				}
			}

			public static class OnSecondary
			{
				public static BrushDependencyPropertyBuilder Default => GetBrushBuilder("OnSecondaryBrush");
				public static BrushDependencyPropertyBuilder Hover => GetBrushBuilder("OnSecondaryHoverBrush");
				public static BrushDependencyPropertyBuilder Focused => GetBrushBuilder("OnSecondaryFocusedBrush");
				public static BrushDependencyPropertyBuilder Pressed => GetBrushBuilder("OnSecondaryPressedBrush");
				public static BrushDependencyPropertyBuilder Dragged => GetBrushBuilder("OnSecondaryDraggedBrush");
				public static BrushDependencyPropertyBuilder Selected => GetBrushBuilder("OnSecondarySelectedBrush");
				public static BrushDependencyPropertyBuilder Medium => GetBrushBuilder("OnSecondaryMediumBrush");
				public static BrushDependencyPropertyBuilder Low => GetBrushBuilder("OnSecondaryLowBrush");
				public static BrushDependencyPropertyBuilder Disabled => GetBrushBuilder("OnSecondaryDisabledBrush");
				public static BrushDependencyPropertyBuilder DisabledLow => GetBrushBuilder("OnSecondaryDisabledLowBrush");

				public static class Container
				{
					public static BrushDependencyPropertyBuilder Default => GetBrushBuilder("OnSecondaryContainerBrush");
					public static BrushDependencyPropertyBuilder Hover => GetBrushBuilder("OnSecondaryContainerHoverBrush");
					public static BrushDependencyPropertyBuilder Focused => GetBrushBuilder("OnSecondaryContainerFocusedBrush");
					public static BrushDependencyPropertyBuilder Pressed => GetBrushBuilder("OnSecondaryContainerPressedBrush");
					public static BrushDependencyPropertyBuilder Dragged => GetBrushBuilder("OnSecondaryContainerDraggedBrush");
					public static BrushDependencyPropertyBuilder Selected => GetBrushBuilder("OnSecondaryContainerSelectedBrush");
					public static BrushDependencyPropertyBuilder Medium => GetBrushBuilder("OnSecondaryContainerMediumBrush");
					public static BrushDependencyPropertyBuilder Low => GetBrushBuilder("OnSecondaryContainerLowBrush");
					public static BrushDependencyPropertyBuilder Disabled => GetBrushBuilder("OnSecondaryContainerDisabledBrush");
					public static BrushDependencyPropertyBuilder DisabledLow => GetBrushBuilder("OnSecondaryContainerDisabledLowBrush");
				}
			}

			public static class Tertiary
			{
				public static BrushDependencyPropertyBuilder Default => GetBrushBuilder("TertiaryBrush");
				public static BrushDependencyPropertyBuilder Hover => GetBrushBuilder("TertiaryHoverBrush");
				public static BrushDependencyPropertyBuilder Focused => GetBrushBuilder("TertiaryFocusedBrush");
				public static BrushDependencyPropertyBuilder Pressed => GetBrushBuilder("TertiaryPressedBrush");
				public static BrushDependencyPropertyBuilder Dragged => GetBrushBuilder("TertiaryDraggedBrush");
				public static BrushDependencyPropertyBuilder Selected => GetBrushBuilder("TertiarySelectedBrush");
				public static BrushDependencyPropertyBuilder Medium => GetBrushBuilder("TertiaryMediumBrush");
				public static BrushDependencyPropertyBuilder Low => GetBrushBuilder("TertiaryLowBrush");
				public static BrushDependencyPropertyBuilder Disabled => GetBrushBuilder("TertiaryDisabledBrush");
				public static BrushDependencyPropertyBuilder DisabledLow => GetBrushBuilder("TertiaryDisabledLowBrush");

				public static class Container
				{
					public static BrushDependencyPropertyBuilder Default => GetBrushBuilder("TertiaryContainerBrush");
					public static BrushDependencyPropertyBuilder Hover => GetBrushBuilder("TertiaryContainerHoverBrush");
					public static BrushDependencyPropertyBuilder Focused => GetBrushBuilder("TertiaryContainerFocusedBrush");
					public static BrushDependencyPropertyBuilder Pressed => GetBrushBuilder("TertiaryContainerPressedBrush");
					public static BrushDependencyPropertyBuilder Dragged => GetBrushBuilder("TertiaryContainerDraggedBrush");
					public static BrushDependencyPropertyBuilder Selected => GetBrushBuilder("TertiaryContainerSelectedBrush");
					public static BrushDependencyPropertyBuilder Medium => GetBrushBuilder("TertiaryContainerMediumBrush");
					public static BrushDependencyPropertyBuilder Low => GetBrushBuilder("TertiaryContainerLowBrush");
					public static BrushDependencyPropertyBuilder Disabled => GetBrushBuilder("TertiaryContainerDisabledBrush");
					public static BrushDependencyPropertyBuilder DisabledLow => GetBrushBuilder("TertiaryContainerDisabledLowBrush");
				}
			}

			public static class OnTertiary
			{
				public static BrushDependencyPropertyBuilder Default => GetBrushBuilder("OnTertiaryBrush");
				public static BrushDependencyPropertyBuilder Hover => GetBrushBuilder("OnTertiaryHoverBrush");
				public static BrushDependencyPropertyBuilder Focused => GetBrushBuilder("OnTertiaryFocusedBrush");
				public static BrushDependencyPropertyBuilder Pressed => GetBrushBuilder("OnTertiaryPressedBrush");
				public static BrushDependencyPropertyBuilder Dragged => GetBrushBuilder("OnTertiaryDraggedBrush");
				public static BrushDependencyPropertyBuilder Selected => GetBrushBuilder("OnTertiarySelectedBrush");
				public static BrushDependencyPropertyBuilder Medium => GetBrushBuilder("OnTertiaryMediumBrush");
				public static BrushDependencyPropertyBuilder Low => GetBrushBuilder("OnTertiaryLowBrush");
				public static BrushDependencyPropertyBuilder Disabled => GetBrushBuilder("OnTertiaryDisabledBrush");
				public static BrushDependencyPropertyBuilder DisabledLow => GetBrushBuilder("OnTertiaryDisabledLowBrush");

				public static class Container
				{
					public static BrushDependencyPropertyBuilder Default => GetBrushBuilder("OnTertiaryContainerBrush");
					public static BrushDependencyPropertyBuilder Hover => GetBrushBuilder("OnTertiaryContainerHoverBrush");
					public static BrushDependencyPropertyBuilder Focused => GetBrushBuilder("OnTertiaryContainerFocusedBrush");
					public static BrushDependencyPropertyBuilder Pressed => GetBrushBuilder("OnTertiaryContainerPressedBrush");
					public static BrushDependencyPropertyBuilder Dragged => GetBrushBuilder("OnTertiaryContainerDraggedBrush");
					public static BrushDependencyPropertyBuilder Selected => GetBrushBuilder("OnTertiaryContainerSelectedBrush");
					public static BrushDependencyPropertyBuilder Medium => GetBrushBuilder("OnTertiaryContainerMediumBrush");
					public static BrushDependencyPropertyBuilder Low => GetBrushBuilder("OnTertiaryContainerLowBrush");
					public static BrushDependencyPropertyBuilder Disabled => GetBrushBuilder("OnTertiaryContainerDisabledBrush");
					public static BrushDependencyPropertyBuilder DisabledLow => GetBrushBuilder("OnTertiaryContainerDisabledLowBrush");
				}
			}

			public static class Error
			{
				public static BrushDependencyPropertyBuilder Default => GetBrushBuilder("ErrorBrush");
				public static BrushDependencyPropertyBuilder Hover => GetBrushBuilder("ErrorHoverBrush");
				public static BrushDependencyPropertyBuilder Focused => GetBrushBuilder("ErrorFocusedBrush");
				public static BrushDependencyPropertyBuilder Pressed => GetBrushBuilder("ErrorPressedBrush");
				public static BrushDependencyPropertyBuilder Dragged => GetBrushBuilder("ErrorDraggedBrush");
				public static BrushDependencyPropertyBuilder Selected => GetBrushBuilder("ErrorSelectedBrush");
				public static BrushDependencyPropertyBuilder Medium => GetBrushBuilder("ErrorMediumBrush");
				public static BrushDependencyPropertyBuilder Low => GetBrushBuilder("ErrorLowBrush");
				public static BrushDependencyPropertyBuilder Disabled => GetBrushBuilder("ErrorDisabledBrush");
				public static BrushDependencyPropertyBuilder DisabledLow => GetBrushBuilder("ErrorDisabledLowBrush");

				public static class Container
				{
					public static BrushDependencyPropertyBuilder Default => GetBrushBuilder("ErrorContainerBrush");
					public static BrushDependencyPropertyBuilder Hover => GetBrushBuilder("ErrorContainerHoverBrush");
					public static BrushDependencyPropertyBuilder Focused => GetBrushBuilder("ErrorContainerFocusedBrush");
					public static BrushDependencyPropertyBuilder Pressed => GetBrushBuilder("ErrorContainerPressedBrush");
					public static BrushDependencyPropertyBuilder Dragged => GetBrushBuilder("ErrorContainerDraggedBrush");
					public static BrushDependencyPropertyBuilder Selected => GetBrushBuilder("ErrorContainerSelectedBrush");
					public static BrushDependencyPropertyBuilder Medium => GetBrushBuilder("ErrorContainerMediumBrush");
					public static BrushDependencyPropertyBuilder Low => GetBrushBuilder("ErrorContainerLowBrush");
					public static BrushDependencyPropertyBuilder Disabled => GetBrushBuilder("ErrorContainerDisabledBrush");
					public static BrushDependencyPropertyBuilder DisabledLow => GetBrushBuilder("ErrorContainerDisabledLowBrush");
				}
			}

			public static class OnError
			{
				public static BrushDependencyPropertyBuilder Default => GetBrushBuilder("OnErrorBrush");
				public static BrushDependencyPropertyBuilder Hover => GetBrushBuilder("OnErrorHoverBrush");
				public static BrushDependencyPropertyBuilder Focused => GetBrushBuilder("OnErrorFocusedBrush");
				public static BrushDependencyPropertyBuilder Pressed => GetBrushBuilder("OnErrorPressedBrush");
				public static BrushDependencyPropertyBuilder Dragged => GetBrushBuilder("OnErrorDraggedBrush");
				public static BrushDependencyPropertyBuilder Selected => GetBrushBuilder("OnErrorSelectedBrush");
				public static BrushDependencyPropertyBuilder Medium => GetBrushBuilder("OnErrorMediumBrush");
				public static BrushDependencyPropertyBuilder Low => GetBrushBuilder("OnErrorLowBrush");
				public static BrushDependencyPropertyBuilder Disabled => GetBrushBuilder("OnErrorDisabledBrush");
				public static BrushDependencyPropertyBuilder DisabledLow => GetBrushBuilder("OnErrorDisabledLowBrush");

				public static class Container
				{
					public static BrushDependencyPropertyBuilder Default => GetBrushBuilder("OnErrorContainerBrush");
					public static BrushDependencyPropertyBuilder Hover => GetBrushBuilder("OnErrorContainerHoverBrush");
					public static BrushDependencyPropertyBuilder Focused => GetBrushBuilder("OnErrorContainerFocusedBrush");
					public static BrushDependencyPropertyBuilder Pressed => GetBrushBuilder("OnErrorContainerPressedBrush");
					public static BrushDependencyPropertyBuilder Dragged => GetBrushBuilder("OnErrorContainerDraggedBrush");
					public static BrushDependencyPropertyBuilder Selected => GetBrushBuilder("OnErrorContainerSelectedBrush");
					public static BrushDependencyPropertyBuilder Medium => GetBrushBuilder("OnErrorContainerMediumBrush");
					public static BrushDependencyPropertyBuilder Low => GetBrushBuilder("OnErrorContainerLowBrush");
					public static BrushDependencyPropertyBuilder Disabled => GetBrushBuilder("OnErrorContainerDisabledBrush");
					public static BrushDependencyPropertyBuilder DisabledLow => GetBrushBuilder("OnErrorContainerDisabledLowBrush");
				}
			}

			public static class Background
			{
				public static BrushDependencyPropertyBuilder Default => GetBrushBuilder("BackgroundBrush");
				public static BrushDependencyPropertyBuilder Hover => GetBrushBuilder("BackgroundHoverBrush");
				public static BrushDependencyPropertyBuilder Focused => GetBrushBuilder("BackgroundFocusedBrush");
				public static BrushDependencyPropertyBuilder Pressed => GetBrushBuilder("BackgroundPressedBrush");
				public static BrushDependencyPropertyBuilder Dragged => GetBrushBuilder("BackgroundDraggedBrush");
				public static BrushDependencyPropertyBuilder Selected => GetBrushBuilder("BackgroundSelectedBrush");
				public static BrushDependencyPropertyBuilder Medium => GetBrushBuilder("BackgroundMediumBrush");
				public static BrushDependencyPropertyBuilder Low => GetBrushBuilder("BackgroundLowBrush");
				public static BrushDependencyPropertyBuilder Disabled => GetBrushBuilder("BackgroundDisabledBrush");
				public static BrushDependencyPropertyBuilder DisabledLow => GetBrushBuilder("BackgroundDisabledLowBrush");
			}

			public static class OnBackground
			{
				public static BrushDependencyPropertyBuilder Default => GetBrushBuilder("OnBackgroundBrush");
				public static BrushDependencyPropertyBuilder Hover => GetBrushBuilder("OnBackgroundHoverBrush");
				public static BrushDependencyPropertyBuilder Focused => GetBrushBuilder("OnBackgroundFocusedBrush");
				public static BrushDependencyPropertyBuilder Pressed => GetBrushBuilder("OnBackgroundPressedBrush");
				public static BrushDependencyPropertyBuilder Dragged => GetBrushBuilder("OnBackgroundDraggedBrush");
				public static BrushDependencyPropertyBuilder Selected => GetBrushBuilder("OnBackgroundSelectedBrush");
				public static BrushDependencyPropertyBuilder Medium => GetBrushBuilder("OnBackgroundMediumBrush");
				public static BrushDependencyPropertyBuilder Low => GetBrushBuilder("OnBackgroundLowBrush");
				public static BrushDependencyPropertyBuilder Disabled => GetBrushBuilder("OnBackgroundDisabledBrush");
				public static BrushDependencyPropertyBuilder DisabledLow => GetBrushBuilder("OnBackgroundDisabledLowBrush");
			}

			public static class Surface
			{
				public static BrushDependencyPropertyBuilder Default => GetBrushBuilder("SurfaceBrush");
				public static BrushDependencyPropertyBuilder Hover => GetBrushBuilder("SurfaceHoverBrush");
				public static BrushDependencyPropertyBuilder Focused => GetBrushBuilder("SurfaceFocusedBrush");
				public static BrushDependencyPropertyBuilder Pressed => GetBrushBuilder("SurfacePressedBrush");
				public static BrushDependencyPropertyBuilder Dragged => GetBrushBuilder("SurfaceDraggedBrush");
				public static BrushDependencyPropertyBuilder Selected => GetBrushBuilder("SurfaceSelectedBrush");
				public static BrushDependencyPropertyBuilder Medium => GetBrushBuilder("SurfaceMediumBrush");
				public static BrushDependencyPropertyBuilder Low => GetBrushBuilder("SurfaceLowBrush");
				public static BrushDependencyPropertyBuilder Disabled => GetBrushBuilder("SurfaceDisabledBrush");
				public static BrushDependencyPropertyBuilder DisabledLow => GetBrushBuilder("SurfaceDisabledLowBrush");

				public static class Variant
				{
					public static BrushDependencyPropertyBuilder Default => GetBrushBuilder("SurfaceVariantBrush");
					public static BrushDependencyPropertyBuilder Hover => GetBrushBuilder("SurfaceVariantHoverBrush");
					public static BrushDependencyPropertyBuilder Focused => GetBrushBuilder("SurfaceVariantFocusedBrush");
					public static BrushDependencyPropertyBuilder Pressed => GetBrushBuilder("SurfaceVariantPressedBrush");
					public static BrushDependencyPropertyBuilder Dragged => GetBrushBuilder("SurfaceVariantDraggedBrush");
					public static BrushDependencyPropertyBuilder Selected => GetBrushBuilder("SurfaceVariantSelectedBrush");
					public static BrushDependencyPropertyBuilder Medium => GetBrushBuilder("SurfaceVariantMediumBrush");
					public static BrushDependencyPropertyBuilder Low => GetBrushBuilder("SurfaceVariantLowBrush");
					public static BrushDependencyPropertyBuilder Disabled => GetBrushBuilder("SurfaceVariantDisabledBrush");
					public static BrushDependencyPropertyBuilder DisabledLow => GetBrushBuilder("SurfaceVariantDisabledLowBrush");
				}

				public static class Inverse
				{
					public static BrushDependencyPropertyBuilder Default => GetBrushBuilder("SurfaceInverseBrush");
					public static BrushDependencyPropertyBuilder Hover => GetBrushBuilder("SurfaceInverseHoverBrush");
					public static BrushDependencyPropertyBuilder Focused => GetBrushBuilder("SurfaceInverseFocusedBrush");
					public static BrushDependencyPropertyBuilder Pressed => GetBrushBuilder("SurfaceInversePressedBrush");
					public static BrushDependencyPropertyBuilder Dragged => GetBrushBuilder("SurfaceInverseDraggedBrush");
					public static BrushDependencyPropertyBuilder Selected => GetBrushBuilder("SurfaceInverseSelectedBrush");
					public static BrushDependencyPropertyBuilder Medium => GetBrushBuilder("SurfaceInverseMediumBrush");
					public static BrushDependencyPropertyBuilder Low => GetBrushBuilder("SurfaceInverseLowBrush");
					public static BrushDependencyPropertyBuilder Disabled => GetBrushBuilder("SurfaceInverseDisabledBrush");
					public static BrushDependencyPropertyBuilder DisabledLow => GetBrushBuilder("SurfaceInverseDisabledLowBrush");
				}
			}

			public static class OnSurface
			{
				public static BrushDependencyPropertyBuilder Default => GetBrushBuilder("OnSurfaceBrush");
				public static BrushDependencyPropertyBuilder Hover => GetBrushBuilder("OnSurfaceHoverBrush");
				public static BrushDependencyPropertyBuilder Focused => GetBrushBuilder("OnSurfaceFocusedBrush");
				public static BrushDependencyPropertyBuilder Pressed => GetBrushBuilder("OnSurfacePressedBrush");
				public static BrushDependencyPropertyBuilder Dragged => GetBrushBuilder("OnSurfaceDraggedBrush");
				public static BrushDependencyPropertyBuilder Selected => GetBrushBuilder("OnSurfaceSelectedBrush");
				public static BrushDependencyPropertyBuilder Medium => GetBrushBuilder("OnSurfaceMediumBrush");
				public static BrushDependencyPropertyBuilder Low => GetBrushBuilder("OnSurfaceLowBrush");
				public static BrushDependencyPropertyBuilder Disabled => GetBrushBuilder("OnSurfaceDisabledBrush");
				public static BrushDependencyPropertyBuilder DisabledLow => GetBrushBuilder("OnSurfaceDisabledLowBrush");

				public static class Variant
				{
					public static BrushDependencyPropertyBuilder Default => GetBrushBuilder("OnSurfaceVariantBrush");
					public static BrushDependencyPropertyBuilder Hover => GetBrushBuilder("OnSurfaceVariantHoverBrush");
					public static BrushDependencyPropertyBuilder Focused => GetBrushBuilder("OnSurfaceVariantFocusedBrush");
					public static BrushDependencyPropertyBuilder Pressed => GetBrushBuilder("OnSurfaceVariantPressedBrush");
					public static BrushDependencyPropertyBuilder Dragged => GetBrushBuilder("OnSurfaceVariantDraggedBrush");
					public static BrushDependencyPropertyBuilder Selected => GetBrushBuilder("OnSurfaceVariantSelectedBrush");
					public static BrushDependencyPropertyBuilder Medium => GetBrushBuilder("OnSurfaceVariantMediumBrush");
					public static BrushDependencyPropertyBuilder Low => GetBrushBuilder("OnSurfaceVariantLowBrush");
					public static BrushDependencyPropertyBuilder Disabled => GetBrushBuilder("OnSurfaceVariantDisabledBrush");
					public static BrushDependencyPropertyBuilder DisabledLow => GetBrushBuilder("OnSurfaceVariantDisabledLowBrush");
				}

				public static class Inverse
				{
					public static BrushDependencyPropertyBuilder Default => GetBrushBuilder("OnSurfaceInverseBrush");
					public static BrushDependencyPropertyBuilder Hover => GetBrushBuilder("OnSurfaceInverseHoverBrush");
					public static BrushDependencyPropertyBuilder Focused => GetBrushBuilder("OnSurfaceInverseFocusedBrush");
					public static BrushDependencyPropertyBuilder Pressed => GetBrushBuilder("OnSurfaceInversePressedBrush");
					public static BrushDependencyPropertyBuilder Dragged => GetBrushBuilder("OnSurfaceInverseDraggedBrush");
					public static BrushDependencyPropertyBuilder Selected => GetBrushBuilder("OnSurfaceInverseSelectedBrush");
					public static BrushDependencyPropertyBuilder Medium => GetBrushBuilder("OnSurfaceInverseMediumBrush");
					public static BrushDependencyPropertyBuilder Low => GetBrushBuilder("OnSurfaceInverseLowBrush");
					public static BrushDependencyPropertyBuilder Disabled => GetBrushBuilder("OnSurfaceInverseDisabledBrush");
					public static BrushDependencyPropertyBuilder DisabledLow => GetBrushBuilder("OnSurfaceInverseDisabledLowBrush");
				}
			}

			public static class Outline
			{
				public static BrushDependencyPropertyBuilder Default => GetBrushBuilder("OutlineBrush");
				public static BrushDependencyPropertyBuilder Hover => GetBrushBuilder("OutlineHoverBrush");
				public static BrushDependencyPropertyBuilder Focused => GetBrushBuilder("OutlineFocusedBrush");
				public static BrushDependencyPropertyBuilder Pressed => GetBrushBuilder("OutlinePressedBrush");
				public static BrushDependencyPropertyBuilder Dragged => GetBrushBuilder("OutlineDraggedBrush");
				public static BrushDependencyPropertyBuilder Selected => GetBrushBuilder("OutlineSelectedBrush");
				public static BrushDependencyPropertyBuilder Medium => GetBrushBuilder("OutlineMediumBrush");
				public static BrushDependencyPropertyBuilder Low => GetBrushBuilder("OutlineLowBrush");
				public static BrushDependencyPropertyBuilder Disabled => GetBrushBuilder("OutlineDisabledBrush");
				public static BrushDependencyPropertyBuilder DisabledLow => GetBrushBuilder("OutlineDisabledLowBrush");
			}
		}
	}
}
