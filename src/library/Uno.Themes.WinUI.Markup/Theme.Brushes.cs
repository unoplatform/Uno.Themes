using System;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.MarkupHelpers;
using Microsoft.UI.Xaml.Media;

namespace Uno.Themes.Markup
{
	partial class Theme
	{
		public static class Brushes
		{
			public static class Primary
			{
				public static Action<IDependencyPropertyBuilder<Brush>> Default => StaticResource.Get<Brush>("PrimaryBrush");
				public static Action<IDependencyPropertyBuilder<Brush>> Hover => StaticResource.Get<Brush>("PrimaryHoverBrush");
				public static Action<IDependencyPropertyBuilder<Brush>> Focused => StaticResource.Get<Brush>("PrimaryFocusedBrush");
				public static Action<IDependencyPropertyBuilder<Brush>> Pressed => StaticResource.Get<Brush>("PrimaryPressedBrush");
				public static Action<IDependencyPropertyBuilder<Brush>> Dragged => StaticResource.Get<Brush>("PrimaryDraggedBrush");
				public static Action<IDependencyPropertyBuilder<Brush>> Selected => StaticResource.Get<Brush>("PrimarySelectedBrush");
				public static Action<IDependencyPropertyBuilder<Brush>> Medium => StaticResource.Get<Brush>("PrimaryMediumBrush");
				public static Action<IDependencyPropertyBuilder<Brush>> Low => StaticResource.Get<Brush>("PrimaryLowBrush");
				public static Action<IDependencyPropertyBuilder<Brush>> Disabled => StaticResource.Get<Brush>("PrimaryDisabledBrush");
				public static Action<IDependencyPropertyBuilder<Brush>> DisabledLow => StaticResource.Get<Brush>("PrimaryDisabledLowBrush");

				public static class Inverse
				{
					public static Action<IDependencyPropertyBuilder<Brush>> Default => StaticResource.Get<Brush>("PrimaryInverseBrush");
					public static Action<IDependencyPropertyBuilder<Brush>> Hover => StaticResource.Get<Brush>("PrimaryInverseHoverBrush");
					public static Action<IDependencyPropertyBuilder<Brush>> Focused => StaticResource.Get<Brush>("PrimaryInverseFocusedBrush");
					public static Action<IDependencyPropertyBuilder<Brush>> Pressed => StaticResource.Get<Brush>("PrimaryInversePressedBrush");
					public static Action<IDependencyPropertyBuilder<Brush>> Dragged => StaticResource.Get<Brush>("PrimaryInverseDraggedBrush");
					public static Action<IDependencyPropertyBuilder<Brush>> Selected => StaticResource.Get<Brush>("PrimaryInverseSelectedBrush");
					public static Action<IDependencyPropertyBuilder<Brush>> Medium => StaticResource.Get<Brush>("PrimaryInverseMediumBrush");
					public static Action<IDependencyPropertyBuilder<Brush>> Low => StaticResource.Get<Brush>("PrimaryInverseLowBrush");
					public static Action<IDependencyPropertyBuilder<Brush>> Disabled => StaticResource.Get<Brush>("PrimaryInverseDisabledBrush");
					public static Action<IDependencyPropertyBuilder<Brush>> DisabledLow => StaticResource.Get<Brush>("PrimaryInverseDisabledLowBrush");
				}

				public static class Container
				{
					public static Action<IDependencyPropertyBuilder<Brush>> Default => StaticResource.Get<Brush>("PrimaryContainerBrush");
					public static Action<IDependencyPropertyBuilder<Brush>> Hover => StaticResource.Get<Brush>("PrimaryContainerHoverBrush");
					public static Action<IDependencyPropertyBuilder<Brush>> Focused => StaticResource.Get<Brush>("PrimaryContainerFocusedBrush");
					public static Action<IDependencyPropertyBuilder<Brush>> Pressed => StaticResource.Get<Brush>("PrimaryContainerPressedBrush");
					public static Action<IDependencyPropertyBuilder<Brush>> Dragged => StaticResource.Get<Brush>("PrimaryContainerDraggedBrush");
					public static Action<IDependencyPropertyBuilder<Brush>> Selected => StaticResource.Get<Brush>("PrimaryContainerSelectedBrush");
					public static Action<IDependencyPropertyBuilder<Brush>> Medium => StaticResource.Get<Brush>("PrimaryContainerMediumBrush");
					public static Action<IDependencyPropertyBuilder<Brush>> Low => StaticResource.Get<Brush>("PrimaryContainerLowBrush");
					public static Action<IDependencyPropertyBuilder<Brush>> Disabled => StaticResource.Get<Brush>("PrimaryContainerDisabledBrush");
					public static Action<IDependencyPropertyBuilder<Brush>> DisabledLow => StaticResource.Get<Brush>("PrimaryContainerDisabledLowBrush");
				}

				public static class VariantLight
				{
					public static Action<IDependencyPropertyBuilder<Brush>> Default => StaticResource.Get<Brush>("PrimaryVariantLightBrush");
					public static Action<IDependencyPropertyBuilder<Brush>> Hover => StaticResource.Get<Brush>("PrimaryVariantLightHoverBrush");
					public static Action<IDependencyPropertyBuilder<Brush>> Focused => StaticResource.Get<Brush>("PrimaryVariantLightFocusedBrush");
					public static Action<IDependencyPropertyBuilder<Brush>> Pressed => StaticResource.Get<Brush>("PrimaryVariantLightPressedBrush");
					public static Action<IDependencyPropertyBuilder<Brush>> Dragged => StaticResource.Get<Brush>("PrimaryVariantLightDraggedBrush");
					public static Action<IDependencyPropertyBuilder<Brush>> Selected => StaticResource.Get<Brush>("PrimaryVariantLightSelectedBrush");
					public static Action<IDependencyPropertyBuilder<Brush>> Medium => StaticResource.Get<Brush>("PrimaryVariantLightMediumBrush");
					public static Action<IDependencyPropertyBuilder<Brush>> Low => StaticResource.Get<Brush>("PrimaryVariantLightLowBrush");
					public static Action<IDependencyPropertyBuilder<Brush>> Disabled => StaticResource.Get<Brush>("PrimaryVariantLightDisabledBrush");
					public static Action<IDependencyPropertyBuilder<Brush>> DisabledLow => StaticResource.Get<Brush>("PrimaryVariantLightDisabledLowBrush");
				}

				public static class VariantDark
				{
					public static Action<IDependencyPropertyBuilder<Brush>> Default => StaticResource.Get<Brush>("PrimaryVariantDarkBrush");
					public static Action<IDependencyPropertyBuilder<Brush>> Hover => StaticResource.Get<Brush>("PrimaryVariantDarkHoverBrush");
					public static Action<IDependencyPropertyBuilder<Brush>> Focused => StaticResource.Get<Brush>("PrimaryVariantDarkFocusedBrush");
					public static Action<IDependencyPropertyBuilder<Brush>> Pressed => StaticResource.Get<Brush>("PrimaryVariantDarkPressedBrush");
					public static Action<IDependencyPropertyBuilder<Brush>> Dragged => StaticResource.Get<Brush>("PrimaryVariantDarkDraggedBrush");
					public static Action<IDependencyPropertyBuilder<Brush>> Selected => StaticResource.Get<Brush>("PrimaryVariantDarkSelectedBrush");
					public static Action<IDependencyPropertyBuilder<Brush>> Medium => StaticResource.Get<Brush>("PrimaryVariantDarkMediumBrush");
					public static Action<IDependencyPropertyBuilder<Brush>> Low => StaticResource.Get<Brush>("PrimaryVariantDarkLowBrush");
					public static Action<IDependencyPropertyBuilder<Brush>> Disabled => StaticResource.Get<Brush>("PrimaryVariantDarkDisabledBrush");
					public static Action<IDependencyPropertyBuilder<Brush>> DisabledLow => StaticResource.Get<Brush>("PrimaryVariantDarkDisabledLowBrush");
				}
			}

			public static class OnPrimary
			{
				public static Action<IDependencyPropertyBuilder<Brush>> Default => StaticResource.Get<Brush>("OnPrimaryBrush");
				public static Action<IDependencyPropertyBuilder<Brush>> Hover => StaticResource.Get<Brush>("OnPrimaryHoverBrush");
				public static Action<IDependencyPropertyBuilder<Brush>> Focused => StaticResource.Get<Brush>("OnPrimaryFocusedBrush");
				public static Action<IDependencyPropertyBuilder<Brush>> Pressed => StaticResource.Get<Brush>("OnPrimaryPressedBrush");
				public static Action<IDependencyPropertyBuilder<Brush>> Dragged => StaticResource.Get<Brush>("OnPrimaryDraggedBrush");
				public static Action<IDependencyPropertyBuilder<Brush>> Selected => StaticResource.Get<Brush>("OnPrimarySelectedBrush");
				public static Action<IDependencyPropertyBuilder<Brush>> Medium => StaticResource.Get<Brush>("OnPrimaryMediumBrush");
				public static Action<IDependencyPropertyBuilder<Brush>> Low => StaticResource.Get<Brush>("OnPrimaryLowBrush");
				public static Action<IDependencyPropertyBuilder<Brush>> Disabled => StaticResource.Get<Brush>("OnPrimaryDisabledBrush");
				public static Action<IDependencyPropertyBuilder<Brush>> DisabledLow => StaticResource.Get<Brush>("OnPrimaryDisabledLowBrush");

				public static class Container
				{
					public static Action<IDependencyPropertyBuilder<Brush>> Default => StaticResource.Get<Brush>("OnPrimaryContainerBrush");
					public static Action<IDependencyPropertyBuilder<Brush>> Hover => StaticResource.Get<Brush>("OnPrimaryContainerHoverBrush");
					public static Action<IDependencyPropertyBuilder<Brush>> Focused => StaticResource.Get<Brush>("OnPrimaryContainerFocusedBrush");
					public static Action<IDependencyPropertyBuilder<Brush>> Pressed => StaticResource.Get<Brush>("OnPrimaryContainerPressedBrush");
					public static Action<IDependencyPropertyBuilder<Brush>> Dragged => StaticResource.Get<Brush>("OnPrimaryContainerDraggedBrush");
					public static Action<IDependencyPropertyBuilder<Brush>> Selected => StaticResource.Get<Brush>("OnPrimaryContainerSelectedBrush");
					public static Action<IDependencyPropertyBuilder<Brush>> Medium => StaticResource.Get<Brush>("OnPrimaryContainerMediumBrush");
					public static Action<IDependencyPropertyBuilder<Brush>> Low => StaticResource.Get<Brush>("OnPrimaryContainerLowBrush");
					public static Action<IDependencyPropertyBuilder<Brush>> Disabled => StaticResource.Get<Brush>("OnPrimaryContainerDisabledBrush");
					public static Action<IDependencyPropertyBuilder<Brush>> DisabledLow => StaticResource.Get<Brush>("OnPrimaryContainerDisabledLowBrush");
				}
			}

			public static class Secondary
			{
				public static Action<IDependencyPropertyBuilder<Brush>> Default => StaticResource.Get<Brush>("SecondaryBrush");
				public static Action<IDependencyPropertyBuilder<Brush>> Hover => StaticResource.Get<Brush>("SecondaryHoverBrush");
				public static Action<IDependencyPropertyBuilder<Brush>> Focused => StaticResource.Get<Brush>("SecondaryFocusedBrush");
				public static Action<IDependencyPropertyBuilder<Brush>> Pressed => StaticResource.Get<Brush>("SecondaryPressedBrush");
				public static Action<IDependencyPropertyBuilder<Brush>> Dragged => StaticResource.Get<Brush>("SecondaryDraggedBrush");
				public static Action<IDependencyPropertyBuilder<Brush>> Selected => StaticResource.Get<Brush>("SecondarySelectedBrush");
				public static Action<IDependencyPropertyBuilder<Brush>> Medium => StaticResource.Get<Brush>("SecondaryMediumBrush");
				public static Action<IDependencyPropertyBuilder<Brush>> Low => StaticResource.Get<Brush>("SecondaryLowBrush");
				public static Action<IDependencyPropertyBuilder<Brush>> Disabled => StaticResource.Get<Brush>("SecondaryDisabledBrush");
				public static Action<IDependencyPropertyBuilder<Brush>> DisabledLow => StaticResource.Get<Brush>("SecondaryDisabledLowBrush");

				public static class Container
				{
					public static Action<IDependencyPropertyBuilder<Brush>> Default => StaticResource.Get<Brush>("SecondaryContainerBrush");
					public static Action<IDependencyPropertyBuilder<Brush>> Hover => StaticResource.Get<Brush>("SecondaryContainerHoverBrush");
					public static Action<IDependencyPropertyBuilder<Brush>> Focused => StaticResource.Get<Brush>("SecondaryContainerFocusedBrush");
					public static Action<IDependencyPropertyBuilder<Brush>> Pressed => StaticResource.Get<Brush>("SecondaryContainerPressedBrush");
					public static Action<IDependencyPropertyBuilder<Brush>> Dragged => StaticResource.Get<Brush>("SecondaryContainerDraggedBrush");
					public static Action<IDependencyPropertyBuilder<Brush>> Selected => StaticResource.Get<Brush>("SecondaryContainerSelectedBrush");
					public static Action<IDependencyPropertyBuilder<Brush>> Medium => StaticResource.Get<Brush>("SecondaryContainerMediumBrush");
					public static Action<IDependencyPropertyBuilder<Brush>> Low => StaticResource.Get<Brush>("SecondaryContainerLowBrush");
					public static Action<IDependencyPropertyBuilder<Brush>> Disabled => StaticResource.Get<Brush>("SecondaryContainerDisabledBrush");
					public static Action<IDependencyPropertyBuilder<Brush>> DisabledLow => StaticResource.Get<Brush>("SecondaryContainerDisabledLowBrush");
				}

				public static class VariantLight
				{
					public static Action<IDependencyPropertyBuilder<Brush>> Default => StaticResource.Get<Brush>("SecondaryVariantLightBrush");
					public static Action<IDependencyPropertyBuilder<Brush>> Hover => StaticResource.Get<Brush>("SecondaryVariantLightHoverBrush");
					public static Action<IDependencyPropertyBuilder<Brush>> Focused => StaticResource.Get<Brush>("SecondaryVariantLightFocusedBrush");
					public static Action<IDependencyPropertyBuilder<Brush>> Pressed => StaticResource.Get<Brush>("SecondaryVariantLightPressedBrush");
					public static Action<IDependencyPropertyBuilder<Brush>> Dragged => StaticResource.Get<Brush>("SecondaryVariantLightDraggedBrush");
					public static Action<IDependencyPropertyBuilder<Brush>> Selected => StaticResource.Get<Brush>("SecondaryVariantLightSelectedBrush");
					public static Action<IDependencyPropertyBuilder<Brush>> Medium => StaticResource.Get<Brush>("SecondaryVariantLightMediumBrush");
					public static Action<IDependencyPropertyBuilder<Brush>> Low => StaticResource.Get<Brush>("SecondaryVariantLightLowBrush");
					public static Action<IDependencyPropertyBuilder<Brush>> Disabled => StaticResource.Get<Brush>("SecondaryVariantLightDisabledBrush");
					public static Action<IDependencyPropertyBuilder<Brush>> DisabledLow => StaticResource.Get<Brush>("SecondaryVariantLightDisabledLowBrush");
				}

				public static class VariantDark
				{
					public static Action<IDependencyPropertyBuilder<Brush>> Default => StaticResource.Get<Brush>("SecondaryVariantDarkBrush");
					public static Action<IDependencyPropertyBuilder<Brush>> Hover => StaticResource.Get<Brush>("SecondaryVariantDarkHoverBrush");
					public static Action<IDependencyPropertyBuilder<Brush>> Focused => StaticResource.Get<Brush>("SecondaryVariantDarkFocusedBrush");
					public static Action<IDependencyPropertyBuilder<Brush>> Pressed => StaticResource.Get<Brush>("SecondaryVariantDarkPressedBrush");
					public static Action<IDependencyPropertyBuilder<Brush>> Dragged => StaticResource.Get<Brush>("SecondaryVariantDarkDraggedBrush");
					public static Action<IDependencyPropertyBuilder<Brush>> Selected => StaticResource.Get<Brush>("SecondaryVariantDarkSelectedBrush");
					public static Action<IDependencyPropertyBuilder<Brush>> Medium => StaticResource.Get<Brush>("SecondaryVariantDarkMediumBrush");
					public static Action<IDependencyPropertyBuilder<Brush>> Low => StaticResource.Get<Brush>("SecondaryVariantDarkLowBrush");
					public static Action<IDependencyPropertyBuilder<Brush>> Disabled => StaticResource.Get<Brush>("SecondaryVariantDarkDisabledBrush");
					public static Action<IDependencyPropertyBuilder<Brush>> DisabledLow => StaticResource.Get<Brush>("SecondaryVariantDarkDisabledLowBrush");
				}
			}

			public static class OnSecondary
			{
				public static Action<IDependencyPropertyBuilder<Brush>> Default => StaticResource.Get<Brush>("OnSecondaryBrush");
				public static Action<IDependencyPropertyBuilder<Brush>> Hover => StaticResource.Get<Brush>("OnSecondaryHoverBrush");
				public static Action<IDependencyPropertyBuilder<Brush>> Focused => StaticResource.Get<Brush>("OnSecondaryFocusedBrush");
				public static Action<IDependencyPropertyBuilder<Brush>> Pressed => StaticResource.Get<Brush>("OnSecondaryPressedBrush");
				public static Action<IDependencyPropertyBuilder<Brush>> Dragged => StaticResource.Get<Brush>("OnSecondaryDraggedBrush");
				public static Action<IDependencyPropertyBuilder<Brush>> Selected => StaticResource.Get<Brush>("OnSecondarySelectedBrush");
				public static Action<IDependencyPropertyBuilder<Brush>> Medium => StaticResource.Get<Brush>("OnSecondaryMediumBrush");
				public static Action<IDependencyPropertyBuilder<Brush>> Low => StaticResource.Get<Brush>("OnSecondaryLowBrush");
				public static Action<IDependencyPropertyBuilder<Brush>> Disabled => StaticResource.Get<Brush>("OnSecondaryDisabledBrush");
				public static Action<IDependencyPropertyBuilder<Brush>> DisabledLow => StaticResource.Get<Brush>("OnSecondaryDisabledLowBrush");

				public static class Container
				{
					public static Action<IDependencyPropertyBuilder<Brush>> Default => StaticResource.Get<Brush>("OnSecondaryContainerBrush");
					public static Action<IDependencyPropertyBuilder<Brush>> Hover => StaticResource.Get<Brush>("OnSecondaryContainerHoverBrush");
					public static Action<IDependencyPropertyBuilder<Brush>> Focused => StaticResource.Get<Brush>("OnSecondaryContainerFocusedBrush");
					public static Action<IDependencyPropertyBuilder<Brush>> Pressed => StaticResource.Get<Brush>("OnSecondaryContainerPressedBrush");
					public static Action<IDependencyPropertyBuilder<Brush>> Dragged => StaticResource.Get<Brush>("OnSecondaryContainerDraggedBrush");
					public static Action<IDependencyPropertyBuilder<Brush>> Selected => StaticResource.Get<Brush>("OnSecondaryContainerSelectedBrush");
					public static Action<IDependencyPropertyBuilder<Brush>> Medium => StaticResource.Get<Brush>("OnSecondaryContainerMediumBrush");
					public static Action<IDependencyPropertyBuilder<Brush>> Low => StaticResource.Get<Brush>("OnSecondaryContainerLowBrush");
					public static Action<IDependencyPropertyBuilder<Brush>> Disabled => StaticResource.Get<Brush>("OnSecondaryContainerDisabledBrush");
					public static Action<IDependencyPropertyBuilder<Brush>> DisabledLow => StaticResource.Get<Brush>("OnSecondaryContainerDisabledLowBrush");
				}
			}

			public static class Tertiary
			{
				public static Action<IDependencyPropertyBuilder<Brush>> Default => StaticResource.Get<Brush>("TertiaryBrush");
				public static Action<IDependencyPropertyBuilder<Brush>> Hover => StaticResource.Get<Brush>("TertiaryHoverBrush");
				public static Action<IDependencyPropertyBuilder<Brush>> Focused => StaticResource.Get<Brush>("TertiaryFocusedBrush");
				public static Action<IDependencyPropertyBuilder<Brush>> Pressed => StaticResource.Get<Brush>("TertiaryPressedBrush");
				public static Action<IDependencyPropertyBuilder<Brush>> Dragged => StaticResource.Get<Brush>("TertiaryDraggedBrush");
				public static Action<IDependencyPropertyBuilder<Brush>> Selected => StaticResource.Get<Brush>("TertiarySelectedBrush");
				public static Action<IDependencyPropertyBuilder<Brush>> Medium => StaticResource.Get<Brush>("TertiaryMediumBrush");
				public static Action<IDependencyPropertyBuilder<Brush>> Low => StaticResource.Get<Brush>("TertiaryLowBrush");
				public static Action<IDependencyPropertyBuilder<Brush>> Disabled => StaticResource.Get<Brush>("TertiaryDisabledBrush");
				public static Action<IDependencyPropertyBuilder<Brush>> DisabledLow => StaticResource.Get<Brush>("TertiaryDisabledLowBrush");

				public static class Container
				{
					public static Action<IDependencyPropertyBuilder<Brush>> Default => StaticResource.Get<Brush>("TertiaryContainerBrush");
					public static Action<IDependencyPropertyBuilder<Brush>> Hover => StaticResource.Get<Brush>("TertiaryContainerHoverBrush");
					public static Action<IDependencyPropertyBuilder<Brush>> Focused => StaticResource.Get<Brush>("TertiaryContainerFocusedBrush");
					public static Action<IDependencyPropertyBuilder<Brush>> Pressed => StaticResource.Get<Brush>("TertiaryContainerPressedBrush");
					public static Action<IDependencyPropertyBuilder<Brush>> Dragged => StaticResource.Get<Brush>("TertiaryContainerDraggedBrush");
					public static Action<IDependencyPropertyBuilder<Brush>> Selected => StaticResource.Get<Brush>("TertiaryContainerSelectedBrush");
					public static Action<IDependencyPropertyBuilder<Brush>> Medium => StaticResource.Get<Brush>("TertiaryContainerMediumBrush");
					public static Action<IDependencyPropertyBuilder<Brush>> Low => StaticResource.Get<Brush>("TertiaryContainerLowBrush");
					public static Action<IDependencyPropertyBuilder<Brush>> Disabled => StaticResource.Get<Brush>("TertiaryContainerDisabledBrush");
					public static Action<IDependencyPropertyBuilder<Brush>> DisabledLow => StaticResource.Get<Brush>("TertiaryContainerDisabledLowBrush");
				}
			}

			public static class OnTertiary
			{
				public static Action<IDependencyPropertyBuilder<Brush>> Default => StaticResource.Get<Brush>("OnTertiaryBrush");
				public static Action<IDependencyPropertyBuilder<Brush>> Hover => StaticResource.Get<Brush>("OnTertiaryHoverBrush");
				public static Action<IDependencyPropertyBuilder<Brush>> Focused => StaticResource.Get<Brush>("OnTertiaryFocusedBrush");
				public static Action<IDependencyPropertyBuilder<Brush>> Pressed => StaticResource.Get<Brush>("OnTertiaryPressedBrush");
				public static Action<IDependencyPropertyBuilder<Brush>> Dragged => StaticResource.Get<Brush>("OnTertiaryDraggedBrush");
				public static Action<IDependencyPropertyBuilder<Brush>> Selected => StaticResource.Get<Brush>("OnTertiarySelectedBrush");
				public static Action<IDependencyPropertyBuilder<Brush>> Medium => StaticResource.Get<Brush>("OnTertiaryMediumBrush");
				public static Action<IDependencyPropertyBuilder<Brush>> Low => StaticResource.Get<Brush>("OnTertiaryLowBrush");
				public static Action<IDependencyPropertyBuilder<Brush>> Disabled => StaticResource.Get<Brush>("OnTertiaryDisabledBrush");
				public static Action<IDependencyPropertyBuilder<Brush>> DisabledLow => StaticResource.Get<Brush>("OnTertiaryDisabledLowBrush");

				public static class Container
				{
					public static Action<IDependencyPropertyBuilder<Brush>> Default => StaticResource.Get<Brush>("OnTertiaryContainerBrush");
					public static Action<IDependencyPropertyBuilder<Brush>> Hover => StaticResource.Get<Brush>("OnTertiaryContainerHoverBrush");
					public static Action<IDependencyPropertyBuilder<Brush>> Focused => StaticResource.Get<Brush>("OnTertiaryContainerFocusedBrush");
					public static Action<IDependencyPropertyBuilder<Brush>> Pressed => StaticResource.Get<Brush>("OnTertiaryContainerPressedBrush");
					public static Action<IDependencyPropertyBuilder<Brush>> Dragged => StaticResource.Get<Brush>("OnTertiaryContainerDraggedBrush");
					public static Action<IDependencyPropertyBuilder<Brush>> Selected => StaticResource.Get<Brush>("OnTertiaryContainerSelectedBrush");
					public static Action<IDependencyPropertyBuilder<Brush>> Medium => StaticResource.Get<Brush>("OnTertiaryContainerMediumBrush");
					public static Action<IDependencyPropertyBuilder<Brush>> Low => StaticResource.Get<Brush>("OnTertiaryContainerLowBrush");
					public static Action<IDependencyPropertyBuilder<Brush>> Disabled => StaticResource.Get<Brush>("OnTertiaryContainerDisabledBrush");
					public static Action<IDependencyPropertyBuilder<Brush>> DisabledLow => StaticResource.Get<Brush>("OnTertiaryContainerDisabledLowBrush");
				}
			}

			public static class Error
			{
				public static Action<IDependencyPropertyBuilder<Brush>> Default => StaticResource.Get<Brush>("ErrorBrush");
				public static Action<IDependencyPropertyBuilder<Brush>> Hover => StaticResource.Get<Brush>("ErrorHoverBrush");
				public static Action<IDependencyPropertyBuilder<Brush>> Focused => StaticResource.Get<Brush>("ErrorFocusedBrush");
				public static Action<IDependencyPropertyBuilder<Brush>> Pressed => StaticResource.Get<Brush>("ErrorPressedBrush");
				public static Action<IDependencyPropertyBuilder<Brush>> Dragged => StaticResource.Get<Brush>("ErrorDraggedBrush");
				public static Action<IDependencyPropertyBuilder<Brush>> Selected => StaticResource.Get<Brush>("ErrorSelectedBrush");
				public static Action<IDependencyPropertyBuilder<Brush>> Medium => StaticResource.Get<Brush>("ErrorMediumBrush");
				public static Action<IDependencyPropertyBuilder<Brush>> Low => StaticResource.Get<Brush>("ErrorLowBrush");
				public static Action<IDependencyPropertyBuilder<Brush>> Disabled => StaticResource.Get<Brush>("ErrorDisabledBrush");
				public static Action<IDependencyPropertyBuilder<Brush>> DisabledLow => StaticResource.Get<Brush>("ErrorDisabledLowBrush");

				public static class Container
				{
					public static Action<IDependencyPropertyBuilder<Brush>> Default => StaticResource.Get<Brush>("ErrorContainerBrush");
					public static Action<IDependencyPropertyBuilder<Brush>> Hover => StaticResource.Get<Brush>("ErrorContainerHoverBrush");
					public static Action<IDependencyPropertyBuilder<Brush>> Focused => StaticResource.Get<Brush>("ErrorContainerFocusedBrush");
					public static Action<IDependencyPropertyBuilder<Brush>> Pressed => StaticResource.Get<Brush>("ErrorContainerPressedBrush");
					public static Action<IDependencyPropertyBuilder<Brush>> Dragged => StaticResource.Get<Brush>("ErrorContainerDraggedBrush");
					public static Action<IDependencyPropertyBuilder<Brush>> Selected => StaticResource.Get<Brush>("ErrorContainerSelectedBrush");
					public static Action<IDependencyPropertyBuilder<Brush>> Medium => StaticResource.Get<Brush>("ErrorContainerMediumBrush");
					public static Action<IDependencyPropertyBuilder<Brush>> Low => StaticResource.Get<Brush>("ErrorContainerLowBrush");
					public static Action<IDependencyPropertyBuilder<Brush>> Disabled => StaticResource.Get<Brush>("ErrorContainerDisabledBrush");
					public static Action<IDependencyPropertyBuilder<Brush>> DisabledLow => StaticResource.Get<Brush>("ErrorContainerDisabledLowBrush");
				}
			}

			public static class OnError
			{
				public static Action<IDependencyPropertyBuilder<Brush>> Default => StaticResource.Get<Brush>("OnErrorBrush");
				public static Action<IDependencyPropertyBuilder<Brush>> Hover => StaticResource.Get<Brush>("OnErrorHoverBrush");
				public static Action<IDependencyPropertyBuilder<Brush>> Focused => StaticResource.Get<Brush>("OnErrorFocusedBrush");
				public static Action<IDependencyPropertyBuilder<Brush>> Pressed => StaticResource.Get<Brush>("OnErrorPressedBrush");
				public static Action<IDependencyPropertyBuilder<Brush>> Dragged => StaticResource.Get<Brush>("OnErrorDraggedBrush");
				public static Action<IDependencyPropertyBuilder<Brush>> Selected => StaticResource.Get<Brush>("OnErrorSelectedBrush");
				public static Action<IDependencyPropertyBuilder<Brush>> Medium => StaticResource.Get<Brush>("OnErrorMediumBrush");
				public static Action<IDependencyPropertyBuilder<Brush>> Low => StaticResource.Get<Brush>("OnErrorLowBrush");
				public static Action<IDependencyPropertyBuilder<Brush>> Disabled => StaticResource.Get<Brush>("OnErrorDisabledBrush");
				public static Action<IDependencyPropertyBuilder<Brush>> DisabledLow => StaticResource.Get<Brush>("OnErrorDisabledLowBrush");

				public static class Container
				{
					public static Action<IDependencyPropertyBuilder<Brush>> Default => StaticResource.Get<Brush>("OnErrorContainerBrush");
					public static Action<IDependencyPropertyBuilder<Brush>> Hover => StaticResource.Get<Brush>("OnErrorContainerHoverBrush");
					public static Action<IDependencyPropertyBuilder<Brush>> Focused => StaticResource.Get<Brush>("OnErrorContainerFocusedBrush");
					public static Action<IDependencyPropertyBuilder<Brush>> Pressed => StaticResource.Get<Brush>("OnErrorContainerPressedBrush");
					public static Action<IDependencyPropertyBuilder<Brush>> Dragged => StaticResource.Get<Brush>("OnErrorContainerDraggedBrush");
					public static Action<IDependencyPropertyBuilder<Brush>> Selected => StaticResource.Get<Brush>("OnErrorContainerSelectedBrush");
					public static Action<IDependencyPropertyBuilder<Brush>> Medium => StaticResource.Get<Brush>("OnErrorContainerMediumBrush");
					public static Action<IDependencyPropertyBuilder<Brush>> Low => StaticResource.Get<Brush>("OnErrorContainerLowBrush");
					public static Action<IDependencyPropertyBuilder<Brush>> Disabled => StaticResource.Get<Brush>("OnErrorContainerDisabledBrush");
					public static Action<IDependencyPropertyBuilder<Brush>> DisabledLow => StaticResource.Get<Brush>("OnErrorContainerDisabledLowBrush");
				}
			}

			public static class Background
			{
				public static Action<IDependencyPropertyBuilder<Brush>> Default => StaticResource.Get<Brush>("BackgroundBrush");
				public static Action<IDependencyPropertyBuilder<Brush>> Hover => StaticResource.Get<Brush>("BackgroundHoverBrush");
				public static Action<IDependencyPropertyBuilder<Brush>> Focused => StaticResource.Get<Brush>("BackgroundFocusedBrush");
				public static Action<IDependencyPropertyBuilder<Brush>> Pressed => StaticResource.Get<Brush>("BackgroundPressedBrush");
				public static Action<IDependencyPropertyBuilder<Brush>> Dragged => StaticResource.Get<Brush>("BackgroundDraggedBrush");
				public static Action<IDependencyPropertyBuilder<Brush>> Selected => StaticResource.Get<Brush>("BackgroundSelectedBrush");
				public static Action<IDependencyPropertyBuilder<Brush>> Medium => StaticResource.Get<Brush>("BackgroundMediumBrush");
				public static Action<IDependencyPropertyBuilder<Brush>> Low => StaticResource.Get<Brush>("BackgroundLowBrush");
				public static Action<IDependencyPropertyBuilder<Brush>> Disabled => StaticResource.Get<Brush>("BackgroundDisabledBrush");
				public static Action<IDependencyPropertyBuilder<Brush>> DisabledLow => StaticResource.Get<Brush>("BackgroundDisabledLowBrush");
			}

			public static class OnBackground
			{
				public static Action<IDependencyPropertyBuilder<Brush>> Default => StaticResource.Get<Brush>("OnBackgroundBrush");
				public static Action<IDependencyPropertyBuilder<Brush>> Hover => StaticResource.Get<Brush>("OnBackgroundHoverBrush");
				public static Action<IDependencyPropertyBuilder<Brush>> Focused => StaticResource.Get<Brush>("OnBackgroundFocusedBrush");
				public static Action<IDependencyPropertyBuilder<Brush>> Pressed => StaticResource.Get<Brush>("OnBackgroundPressedBrush");
				public static Action<IDependencyPropertyBuilder<Brush>> Dragged => StaticResource.Get<Brush>("OnBackgroundDraggedBrush");
				public static Action<IDependencyPropertyBuilder<Brush>> Selected => StaticResource.Get<Brush>("OnBackgroundSelectedBrush");
				public static Action<IDependencyPropertyBuilder<Brush>> Medium => StaticResource.Get<Brush>("OnBackgroundMediumBrush");
				public static Action<IDependencyPropertyBuilder<Brush>> Low => StaticResource.Get<Brush>("OnBackgroundLowBrush");
				public static Action<IDependencyPropertyBuilder<Brush>> Disabled => StaticResource.Get<Brush>("OnBackgroundDisabledBrush");
				public static Action<IDependencyPropertyBuilder<Brush>> DisabledLow => StaticResource.Get<Brush>("OnBackgroundDisabledLowBrush");
			}

			public static class Surface
			{
				public static Action<IDependencyPropertyBuilder<Brush>> Default => StaticResource.Get<Brush>("SurfaceBrush");
				public static Action<IDependencyPropertyBuilder<Brush>> Hover => StaticResource.Get<Brush>("SurfaceHoverBrush");
				public static Action<IDependencyPropertyBuilder<Brush>> Focused => StaticResource.Get<Brush>("SurfaceFocusedBrush");
				public static Action<IDependencyPropertyBuilder<Brush>> Pressed => StaticResource.Get<Brush>("SurfacePressedBrush");
				public static Action<IDependencyPropertyBuilder<Brush>> Dragged => StaticResource.Get<Brush>("SurfaceDraggedBrush");
				public static Action<IDependencyPropertyBuilder<Brush>> Selected => StaticResource.Get<Brush>("SurfaceSelectedBrush");
				public static Action<IDependencyPropertyBuilder<Brush>> Medium => StaticResource.Get<Brush>("SurfaceMediumBrush");
				public static Action<IDependencyPropertyBuilder<Brush>> Low => StaticResource.Get<Brush>("SurfaceLowBrush");
				public static Action<IDependencyPropertyBuilder<Brush>> Disabled => StaticResource.Get<Brush>("SurfaceDisabledBrush");
				public static Action<IDependencyPropertyBuilder<Brush>> DisabledLow => StaticResource.Get<Brush>("SurfaceDisabledLowBrush");

				public static class Variant
				{
					public static Action<IDependencyPropertyBuilder<Brush>> Default => StaticResource.Get<Brush>("SurfaceVariantBrush");
					public static Action<IDependencyPropertyBuilder<Brush>> Hover => StaticResource.Get<Brush>("SurfaceVariantHoverBrush");
					public static Action<IDependencyPropertyBuilder<Brush>> Focused => StaticResource.Get<Brush>("SurfaceVariantFocusedBrush");
					public static Action<IDependencyPropertyBuilder<Brush>> Pressed => StaticResource.Get<Brush>("SurfaceVariantPressedBrush");
					public static Action<IDependencyPropertyBuilder<Brush>> Dragged => StaticResource.Get<Brush>("SurfaceVariantDraggedBrush");
					public static Action<IDependencyPropertyBuilder<Brush>> Selected => StaticResource.Get<Brush>("SurfaceVariantSelectedBrush");
					public static Action<IDependencyPropertyBuilder<Brush>> Medium => StaticResource.Get<Brush>("SurfaceVariantMediumBrush");
					public static Action<IDependencyPropertyBuilder<Brush>> Low => StaticResource.Get<Brush>("SurfaceVariantLowBrush");
					public static Action<IDependencyPropertyBuilder<Brush>> Disabled => StaticResource.Get<Brush>("SurfaceVariantDisabledBrush");
					public static Action<IDependencyPropertyBuilder<Brush>> DisabledLow => StaticResource.Get<Brush>("SurfaceVariantDisabledLowBrush");
				}

				public static class Inverse
				{
					public static Action<IDependencyPropertyBuilder<Brush>> Default => StaticResource.Get<Brush>("SurfaceInverseBrush");
					public static Action<IDependencyPropertyBuilder<Brush>> Hover => StaticResource.Get<Brush>("SurfaceInverseHoverBrush");
					public static Action<IDependencyPropertyBuilder<Brush>> Focused => StaticResource.Get<Brush>("SurfaceInverseFocusedBrush");
					public static Action<IDependencyPropertyBuilder<Brush>> Pressed => StaticResource.Get<Brush>("SurfaceInversePressedBrush");
					public static Action<IDependencyPropertyBuilder<Brush>> Dragged => StaticResource.Get<Brush>("SurfaceInverseDraggedBrush");
					public static Action<IDependencyPropertyBuilder<Brush>> Selected => StaticResource.Get<Brush>("SurfaceInverseSelectedBrush");
					public static Action<IDependencyPropertyBuilder<Brush>> Medium => StaticResource.Get<Brush>("SurfaceInverseMediumBrush");
					public static Action<IDependencyPropertyBuilder<Brush>> Low => StaticResource.Get<Brush>("SurfaceInverseLowBrush");
					public static Action<IDependencyPropertyBuilder<Brush>> Disabled => StaticResource.Get<Brush>("SurfaceInverseDisabledBrush");
					public static Action<IDependencyPropertyBuilder<Brush>> DisabledLow => StaticResource.Get<Brush>("SurfaceInverseDisabledLowBrush");
				}
			}

			public static class OnSurface
			{
				public static Action<IDependencyPropertyBuilder<Brush>> Default => StaticResource.Get<Brush>("OnSurfaceBrush");
				public static Action<IDependencyPropertyBuilder<Brush>> Hover => StaticResource.Get<Brush>("OnSurfaceHoverBrush");
				public static Action<IDependencyPropertyBuilder<Brush>> Focused => StaticResource.Get<Brush>("OnSurfaceFocusedBrush");
				public static Action<IDependencyPropertyBuilder<Brush>> Pressed => StaticResource.Get<Brush>("OnSurfacePressedBrush");
				public static Action<IDependencyPropertyBuilder<Brush>> Dragged => StaticResource.Get<Brush>("OnSurfaceDraggedBrush");
				public static Action<IDependencyPropertyBuilder<Brush>> Selected => StaticResource.Get<Brush>("OnSurfaceSelectedBrush");
				public static Action<IDependencyPropertyBuilder<Brush>> Medium => StaticResource.Get<Brush>("OnSurfaceMediumBrush");
				public static Action<IDependencyPropertyBuilder<Brush>> Low => StaticResource.Get<Brush>("OnSurfaceLowBrush");
				public static Action<IDependencyPropertyBuilder<Brush>> Disabled => StaticResource.Get<Brush>("OnSurfaceDisabledBrush");
				public static Action<IDependencyPropertyBuilder<Brush>> DisabledLow => StaticResource.Get<Brush>("OnSurfaceDisabledLowBrush");

				public static class Variant
				{
					public static Action<IDependencyPropertyBuilder<Brush>> Default => StaticResource.Get<Brush>("OnSurfaceVariantBrush");
					public static Action<IDependencyPropertyBuilder<Brush>> Hover => StaticResource.Get<Brush>("OnSurfaceVariantHoverBrush");
					public static Action<IDependencyPropertyBuilder<Brush>> Focused => StaticResource.Get<Brush>("OnSurfaceVariantFocusedBrush");
					public static Action<IDependencyPropertyBuilder<Brush>> Pressed => StaticResource.Get<Brush>("OnSurfaceVariantPressedBrush");
					public static Action<IDependencyPropertyBuilder<Brush>> Dragged => StaticResource.Get<Brush>("OnSurfaceVariantDraggedBrush");
					public static Action<IDependencyPropertyBuilder<Brush>> Selected => StaticResource.Get<Brush>("OnSurfaceVariantSelectedBrush");
					public static Action<IDependencyPropertyBuilder<Brush>> Medium => StaticResource.Get<Brush>("OnSurfaceVariantMediumBrush");
					public static Action<IDependencyPropertyBuilder<Brush>> Low => StaticResource.Get<Brush>("OnSurfaceVariantLowBrush");
					public static Action<IDependencyPropertyBuilder<Brush>> Disabled => StaticResource.Get<Brush>("OnSurfaceVariantDisabledBrush");
					public static Action<IDependencyPropertyBuilder<Brush>> DisabledLow => StaticResource.Get<Brush>("OnSurfaceVariantDisabledLowBrush");
				}

				public static class Inverse
				{
					public static Action<IDependencyPropertyBuilder<Brush>> Default => StaticResource.Get<Brush>("OnSurfaceInverseBrush");
					public static Action<IDependencyPropertyBuilder<Brush>> Hover => StaticResource.Get<Brush>("OnSurfaceInverseHoverBrush");
					public static Action<IDependencyPropertyBuilder<Brush>> Focused => StaticResource.Get<Brush>("OnSurfaceInverseFocusedBrush");
					public static Action<IDependencyPropertyBuilder<Brush>> Pressed => StaticResource.Get<Brush>("OnSurfaceInversePressedBrush");
					public static Action<IDependencyPropertyBuilder<Brush>> Dragged => StaticResource.Get<Brush>("OnSurfaceInverseDraggedBrush");
					public static Action<IDependencyPropertyBuilder<Brush>> Selected => StaticResource.Get<Brush>("OnSurfaceInverseSelectedBrush");
					public static Action<IDependencyPropertyBuilder<Brush>> Medium => StaticResource.Get<Brush>("OnSurfaceInverseMediumBrush");
					public static Action<IDependencyPropertyBuilder<Brush>> Low => StaticResource.Get<Brush>("OnSurfaceInverseLowBrush");
					public static Action<IDependencyPropertyBuilder<Brush>> Disabled => StaticResource.Get<Brush>("OnSurfaceInverseDisabledBrush");
					public static Action<IDependencyPropertyBuilder<Brush>> DisabledLow => StaticResource.Get<Brush>("OnSurfaceInverseDisabledLowBrush");
				}
			}

			public static class Outline
			{
				public static Action<IDependencyPropertyBuilder<Brush>> Default => StaticResource.Get<Brush>("OutlineBrush");
				public static Action<IDependencyPropertyBuilder<Brush>> Hover => StaticResource.Get<Brush>("OutlineHoverBrush");
				public static Action<IDependencyPropertyBuilder<Brush>> Focused => StaticResource.Get<Brush>("OutlineFocusedBrush");
				public static Action<IDependencyPropertyBuilder<Brush>> Pressed => StaticResource.Get<Brush>("OutlinePressedBrush");
				public static Action<IDependencyPropertyBuilder<Brush>> Dragged => StaticResource.Get<Brush>("OutlineDraggedBrush");
				public static Action<IDependencyPropertyBuilder<Brush>> Selected => StaticResource.Get<Brush>("OutlineSelectedBrush");
				public static Action<IDependencyPropertyBuilder<Brush>> Medium => StaticResource.Get<Brush>("OutlineMediumBrush");
				public static Action<IDependencyPropertyBuilder<Brush>> Low => StaticResource.Get<Brush>("OutlineLowBrush");
				public static Action<IDependencyPropertyBuilder<Brush>> Disabled => StaticResource.Get<Brush>("OutlineDisabledBrush");
				public static Action<IDependencyPropertyBuilder<Brush>> DisabledLow => StaticResource.Get<Brush>("OutlineDisabledLowBrush");
			}
		}
	}
}
