using Microsoft.UI.Xaml.MarkupHelpers.Internals;
using Microsoft.UI.Xaml.Media;

namespace Uno.Themes.Markup
{
	partial class Theme
	{
		public static class Brushes
		{
			public static class Primary
			{
				[ResourceKeyDefinition(typeof(Brush), "PrimaryBrush")]
				public static ResourceValue<Brush> Default => new("PrimaryBrush");

				[ResourceKeyDefinition(typeof(Brush), "PrimaryHoverBrush")]
				public static ResourceValue<Brush> Hover => new("PrimaryHoverBrush");

				[ResourceKeyDefinition(typeof(Brush), "PrimaryFocusedBrush")]
				public static ResourceValue<Brush> Focused => new("PrimaryFocusedBrush");

				[ResourceKeyDefinition(typeof(Brush), "PrimaryPressedBrush")]
				public static ResourceValue<Brush> Pressed => new("PrimaryPressedBrush");

				[ResourceKeyDefinition(typeof(Brush), "PrimaryDraggedBrush")]
				public static ResourceValue<Brush> Dragged => new("PrimaryDraggedBrush");

				[ResourceKeyDefinition(typeof(Brush), "PrimarySelectedBrush")]
				public static ResourceValue<Brush> Selected => new("PrimarySelectedBrush");

				[ResourceKeyDefinition(typeof(Brush), "PrimaryMediumBrush")]
				public static ResourceValue<Brush> Medium => new("PrimaryMediumBrush");

				[ResourceKeyDefinition(typeof(Brush), "PrimaryLowBrush")]
				public static ResourceValue<Brush> Low => new("PrimaryLowBrush");

				[ResourceKeyDefinition(typeof(Brush), "PrimaryDisabledBrush")]
				public static ResourceValue<Brush> Disabled => new("PrimaryDisabledBrush");

				[ResourceKeyDefinition(typeof(Brush), "PrimaryDisabledLowBrush")]
				public static ResourceValue<Brush> DisabledLow => new("PrimaryDisabledLowBrush");

				public static class Inverse
				{
					[ResourceKeyDefinition(typeof(Brush), "PrimaryInverseBrush")]
					public static ResourceValue<Brush> Default => new("PrimaryInverseBrush");

					[ResourceKeyDefinition(typeof(Brush), "PrimaryInverseHoverBrush")]
					public static ResourceValue<Brush> Hover => new("PrimaryInverseHoverBrush");

					[ResourceKeyDefinition(typeof(Brush), "PrimaryInverseFocusedBrush")]
					public static ResourceValue<Brush> Focused => new("PrimaryInverseFocusedBrush");

					[ResourceKeyDefinition(typeof(Brush), "PrimaryInversePressedBrush")]
					public static ResourceValue<Brush> Pressed => new("PrimaryInversePressedBrush");

					[ResourceKeyDefinition(typeof(Brush), "PrimaryInverseDraggedBrush")]
					public static ResourceValue<Brush> Dragged => new("PrimaryInverseDraggedBrush");

					[ResourceKeyDefinition(typeof(Brush), "PrimaryInverseSelectedBrush")]
					public static ResourceValue<Brush> Selected => new("PrimaryInverseSelectedBrush");

					[ResourceKeyDefinition(typeof(Brush), "PrimaryInverseMediumBrush")]
					public static ResourceValue<Brush> Medium => new("PrimaryInverseMediumBrush");

					[ResourceKeyDefinition(typeof(Brush), "PrimaryInverseLowBrush")]
					public static ResourceValue<Brush> Low => new("PrimaryInverseLowBrush");

					[ResourceKeyDefinition(typeof(Brush), "PrimaryInverseDisabledBrush")]
					public static ResourceValue<Brush> Disabled => new("PrimaryInverseDisabledBrush");

					[ResourceKeyDefinition(typeof(Brush), "PrimaryInverseDisabledLowBrush")]
					public static ResourceValue<Brush> DisabledLow => new("PrimaryInverseDisabledLowBrush");
				}

				public static class Container
				{
					[ResourceKeyDefinition(typeof(Brush), "PrimaryContainerBrush")]
					public static ResourceValue<Brush> Default => new("PrimaryContainerBrush");

					[ResourceKeyDefinition(typeof(Brush), "PrimaryContainerHoverBrush")]
					public static ResourceValue<Brush> Hover => new("PrimaryContainerHoverBrush");

					[ResourceKeyDefinition(typeof(Brush), "PrimaryContainerFocusedBrush")]
					public static ResourceValue<Brush> Focused => new("PrimaryContainerFocusedBrush");

					[ResourceKeyDefinition(typeof(Brush), "PrimaryContainerPressedBrush")]
					public static ResourceValue<Brush> Pressed => new("PrimaryContainerPressedBrush");

					[ResourceKeyDefinition(typeof(Brush), "PrimaryContainerDraggedBrush")]
					public static ResourceValue<Brush> Dragged => new("PrimaryContainerDraggedBrush");

					[ResourceKeyDefinition(typeof(Brush), "PrimaryContainerSelectedBrush")]
					public static ResourceValue<Brush> Selected => new("PrimaryContainerSelectedBrush");

					[ResourceKeyDefinition(typeof(Brush), "PrimaryContainerMediumBrush")]
					public static ResourceValue<Brush> Medium => new("PrimaryContainerMediumBrush");

					[ResourceKeyDefinition(typeof(Brush), "PrimaryContainerLowBrush")]
					public static ResourceValue<Brush> Low => new("PrimaryContainerLowBrush");

					[ResourceKeyDefinition(typeof(Brush), "PrimaryContainerDisabledBrush")]
					public static ResourceValue<Brush> Disabled => new("PrimaryContainerDisabledBrush");

					[ResourceKeyDefinition(typeof(Brush), "PrimaryContainerDisabledLowBrush")]
					public static ResourceValue<Brush> DisabledLow => new("PrimaryContainerDisabledLowBrush");
				}

				public static class VariantLight
				{
					[ResourceKeyDefinition(typeof(Brush), "PrimaryVariantLightBrush")]
					public static ResourceValue<Brush> Default => new("PrimaryVariantLightBrush");

					[ResourceKeyDefinition(typeof(Brush), "PrimaryVariantLightHoverBrush")]
					public static ResourceValue<Brush> Hover => new("PrimaryVariantLightHoverBrush");

					[ResourceKeyDefinition(typeof(Brush), "PrimaryVariantLightFocusedBrush")]
					public static ResourceValue<Brush> Focused => new("PrimaryVariantLightFocusedBrush");

					[ResourceKeyDefinition(typeof(Brush), "PrimaryVariantLightPressedBrush")]
					public static ResourceValue<Brush> Pressed => new("PrimaryVariantLightPressedBrush");

					[ResourceKeyDefinition(typeof(Brush), "PrimaryVariantLightDraggedBrush")]
					public static ResourceValue<Brush> Dragged => new("PrimaryVariantLightDraggedBrush");

					[ResourceKeyDefinition(typeof(Brush), "PrimaryVariantLightSelectedBrush")]
					public static ResourceValue<Brush> Selected => new("PrimaryVariantLightSelectedBrush");

					[ResourceKeyDefinition(typeof(Brush), "PrimaryVariantLightMediumBrush")]
					public static ResourceValue<Brush> Medium => new("PrimaryVariantLightMediumBrush");

					[ResourceKeyDefinition(typeof(Brush), "PrimaryVariantLightLowBrush")]
					public static ResourceValue<Brush> Low => new("PrimaryVariantLightLowBrush");

					[ResourceKeyDefinition(typeof(Brush), "PrimaryVariantLightDisabledBrush")]
					public static ResourceValue<Brush> Disabled => new("PrimaryVariantLightDisabledBrush");

					[ResourceKeyDefinition(typeof(Brush), "PrimaryVariantLightDisabledLowBrush")]
					public static ResourceValue<Brush> DisabledLow => new("PrimaryVariantLightDisabledLowBrush");
				}

				public static class VariantDark
				{
					[ResourceKeyDefinition(typeof(Brush), "PrimaryVariantDarkBrush")]
					public static ResourceValue<Brush> Default => new("PrimaryVariantDarkBrush");

					[ResourceKeyDefinition(typeof(Brush), "PrimaryVariantDarkHoverBrush")]
					public static ResourceValue<Brush> Hover => new("PrimaryVariantDarkHoverBrush");

					[ResourceKeyDefinition(typeof(Brush), "PrimaryVariantDarkFocusedBrush")]
					public static ResourceValue<Brush> Focused => new("PrimaryVariantDarkFocusedBrush");

					[ResourceKeyDefinition(typeof(Brush), "PrimaryVariantDarkPressedBrush")]
					public static ResourceValue<Brush> Pressed => new("PrimaryVariantDarkPressedBrush");

					[ResourceKeyDefinition(typeof(Brush), "PrimaryVariantDarkDraggedBrush")]
					public static ResourceValue<Brush> Dragged => new("PrimaryVariantDarkDraggedBrush");

					[ResourceKeyDefinition(typeof(Brush), "PrimaryVariantDarkSelectedBrush")]
					public static ResourceValue<Brush> Selected => new("PrimaryVariantDarkSelectedBrush");

					[ResourceKeyDefinition(typeof(Brush), "PrimaryVariantDarkMediumBrush")]
					public static ResourceValue<Brush> Medium => new("PrimaryVariantDarkMediumBrush");

					[ResourceKeyDefinition(typeof(Brush), "PrimaryVariantDarkLowBrush")]
					public static ResourceValue<Brush> Low => new("PrimaryVariantDarkLowBrush");

					[ResourceKeyDefinition(typeof(Brush), "PrimaryVariantDarkDisabledBrush")]
					public static ResourceValue<Brush> Disabled => new("PrimaryVariantDarkDisabledBrush");

					[ResourceKeyDefinition(typeof(Brush), "PrimaryVariantDarkDisabledLowBrush")]
					public static ResourceValue<Brush> DisabledLow => new("PrimaryVariantDarkDisabledLowBrush");
				}
			}

			public static class OnPrimary
			{
				[ResourceKeyDefinition(typeof(Brush), "OnPrimaryBrush")]
				public static ResourceValue<Brush> Default => new("OnPrimaryBrush");

				[ResourceKeyDefinition(typeof(Brush), "OnPrimaryHoverBrush")]
				public static ResourceValue<Brush> Hover => new("OnPrimaryHoverBrush");

				[ResourceKeyDefinition(typeof(Brush), "OnPrimaryFocusedBrush")]
				public static ResourceValue<Brush> Focused => new("OnPrimaryFocusedBrush");

				[ResourceKeyDefinition(typeof(Brush), "OnPrimaryPressedBrush")]
				public static ResourceValue<Brush> Pressed => new("OnPrimaryPressedBrush");

				[ResourceKeyDefinition(typeof(Brush), "OnPrimaryDraggedBrush")]
				public static ResourceValue<Brush> Dragged => new("OnPrimaryDraggedBrush");

				[ResourceKeyDefinition(typeof(Brush), "OnPrimarySelectedBrush")]
				public static ResourceValue<Brush> Selected => new("OnPrimarySelectedBrush");

				[ResourceKeyDefinition(typeof(Brush), "OnPrimaryMediumBrush")]
				public static ResourceValue<Brush> Medium => new("OnPrimaryMediumBrush");

				[ResourceKeyDefinition(typeof(Brush), "OnPrimaryLowBrush")]
				public static ResourceValue<Brush> Low => new("OnPrimaryLowBrush");

				[ResourceKeyDefinition(typeof(Brush), "OnPrimaryDisabledBrush")]
				public static ResourceValue<Brush> Disabled => new("OnPrimaryDisabledBrush");

				[ResourceKeyDefinition(typeof(Brush), "OnPrimaryDisabledLowBrush")]
				public static ResourceValue<Brush> DisabledLow => new("OnPrimaryDisabledLowBrush");

				public static class Container
				{
					[ResourceKeyDefinition(typeof(Brush), "OnPrimaryContainerBrush")]
					public static ResourceValue<Brush> Default => new("OnPrimaryContainerBrush");

					[ResourceKeyDefinition(typeof(Brush), "OnPrimaryContainerHoverBrush")]
					public static ResourceValue<Brush> Hover => new("OnPrimaryContainerHoverBrush");

					[ResourceKeyDefinition(typeof(Brush), "OnPrimaryContainerFocusedBrush")]
					public static ResourceValue<Brush> Focused => new("OnPrimaryContainerFocusedBrush");

					[ResourceKeyDefinition(typeof(Brush), "OnPrimaryContainerPressedBrush")]
					public static ResourceValue<Brush> Pressed => new("OnPrimaryContainerPressedBrush");

					[ResourceKeyDefinition(typeof(Brush), "OnPrimaryContainerDraggedBrush")]
					public static ResourceValue<Brush> Dragged => new("OnPrimaryContainerDraggedBrush");

					[ResourceKeyDefinition(typeof(Brush), "OnPrimaryContainerSelectedBrush")]
					public static ResourceValue<Brush> Selected => new("OnPrimaryContainerSelectedBrush");

					[ResourceKeyDefinition(typeof(Brush), "OnPrimaryContainerMediumBrush")]
					public static ResourceValue<Brush> Medium => new("OnPrimaryContainerMediumBrush");

					[ResourceKeyDefinition(typeof(Brush), "OnPrimaryContainerLowBrush")]
					public static ResourceValue<Brush> Low => new("OnPrimaryContainerLowBrush");

					[ResourceKeyDefinition(typeof(Brush), "OnPrimaryContainerDisabledBrush")]
					public static ResourceValue<Brush> Disabled => new("OnPrimaryContainerDisabledBrush");

					[ResourceKeyDefinition(typeof(Brush), "OnPrimaryContainerDisabledLowBrush")]
					public static ResourceValue<Brush> DisabledLow => new("OnPrimaryContainerDisabledLowBrush");
				}
			}

			public static class Secondary
			{
				[ResourceKeyDefinition(typeof(Brush), "SecondaryBrush")]
				public static ResourceValue<Brush> Default => new("SecondaryBrush");

				[ResourceKeyDefinition(typeof(Brush), "SecondaryHoverBrush")]
				public static ResourceValue<Brush> Hover => new("SecondaryHoverBrush");

				[ResourceKeyDefinition(typeof(Brush), "SecondaryFocusedBrush")]
				public static ResourceValue<Brush> Focused => new("SecondaryFocusedBrush");

				[ResourceKeyDefinition(typeof(Brush), "SecondaryPressedBrush")]
				public static ResourceValue<Brush> Pressed => new("SecondaryPressedBrush");

				[ResourceKeyDefinition(typeof(Brush), "SecondaryDraggedBrush")]
				public static ResourceValue<Brush> Dragged => new("SecondaryDraggedBrush");

				[ResourceKeyDefinition(typeof(Brush), "SecondarySelectedBrush")]
				public static ResourceValue<Brush> Selected => new("SecondarySelectedBrush");

				[ResourceKeyDefinition(typeof(Brush), "SecondaryMediumBrush")]
				public static ResourceValue<Brush> Medium => new("SecondaryMediumBrush");

				[ResourceKeyDefinition(typeof(Brush), "SecondaryLowBrush")]
				public static ResourceValue<Brush> Low => new("SecondaryLowBrush");

				[ResourceKeyDefinition(typeof(Brush), "SecondaryDisabledBrush")]
				public static ResourceValue<Brush> Disabled => new("SecondaryDisabledBrush");

				[ResourceKeyDefinition(typeof(Brush), "SecondaryDisabledLowBrush")]
				public static ResourceValue<Brush> DisabledLow => new("SecondaryDisabledLowBrush");

				public static class Container
				{
					[ResourceKeyDefinition(typeof(Brush), "SecondaryContainerBrush")]
					public static ResourceValue<Brush> Default => new("SecondaryContainerBrush");

					[ResourceKeyDefinition(typeof(Brush), "SecondaryContainerHoverBrush")]
					public static ResourceValue<Brush> Hover => new("SecondaryContainerHoverBrush");

					[ResourceKeyDefinition(typeof(Brush), "SecondaryContainerFocusedBrush")]
					public static ResourceValue<Brush> Focused => new("SecondaryContainerFocusedBrush");

					[ResourceKeyDefinition(typeof(Brush), "SecondaryContainerPressedBrush")]
					public static ResourceValue<Brush> Pressed => new("SecondaryContainerPressedBrush");

					[ResourceKeyDefinition(typeof(Brush), "SecondaryContainerDraggedBrush")]
					public static ResourceValue<Brush> Dragged => new("SecondaryContainerDraggedBrush");

					[ResourceKeyDefinition(typeof(Brush), "SecondaryContainerSelectedBrush")]
					public static ResourceValue<Brush> Selected => new("SecondaryContainerSelectedBrush");

					[ResourceKeyDefinition(typeof(Brush), "SecondaryContainerMediumBrush")]
					public static ResourceValue<Brush> Medium => new("SecondaryContainerMediumBrush");

					[ResourceKeyDefinition(typeof(Brush), "SecondaryContainerLowBrush")]
					public static ResourceValue<Brush> Low => new("SecondaryContainerLowBrush");

					[ResourceKeyDefinition(typeof(Brush), "SecondaryContainerDisabledBrush")]
					public static ResourceValue<Brush> Disabled => new("SecondaryContainerDisabledBrush");

					[ResourceKeyDefinition(typeof(Brush), "SecondaryContainerDisabledLowBrush")]
					public static ResourceValue<Brush> DisabledLow => new("SecondaryContainerDisabledLowBrush");
				}

				public static class VariantLight
				{
					[ResourceKeyDefinition(typeof(Brush), "SecondaryVariantLightBrush")]
					public static ResourceValue<Brush> Default => new("SecondaryVariantLightBrush");

					[ResourceKeyDefinition(typeof(Brush), "SecondaryVariantLightHoverBrush")]
					public static ResourceValue<Brush> Hover => new("SecondaryVariantLightHoverBrush");

					[ResourceKeyDefinition(typeof(Brush), "SecondaryVariantLightFocusedBrush")]
					public static ResourceValue<Brush> Focused => new("SecondaryVariantLightFocusedBrush");

					[ResourceKeyDefinition(typeof(Brush), "SecondaryVariantLightPressedBrush")]
					public static ResourceValue<Brush> Pressed => new("SecondaryVariantLightPressedBrush");

					[ResourceKeyDefinition(typeof(Brush), "SecondaryVariantLightDraggedBrush")]
					public static ResourceValue<Brush> Dragged => new("SecondaryVariantLightDraggedBrush");

					[ResourceKeyDefinition(typeof(Brush), "SecondaryVariantLightSelectedBrush")]
					public static ResourceValue<Brush> Selected => new("SecondaryVariantLightSelectedBrush");

					[ResourceKeyDefinition(typeof(Brush), "SecondaryVariantLightMediumBrush")]
					public static ResourceValue<Brush> Medium => new("SecondaryVariantLightMediumBrush");

					[ResourceKeyDefinition(typeof(Brush), "SecondaryVariantLightLowBrush")]
					public static ResourceValue<Brush> Low => new("SecondaryVariantLightLowBrush");

					[ResourceKeyDefinition(typeof(Brush), "SecondaryVariantLightDisabledBrush")]
					public static ResourceValue<Brush> Disabled => new("SecondaryVariantLightDisabledBrush");

					[ResourceKeyDefinition(typeof(Brush), "SecondaryVariantLightDisabledLowBrush")]
					public static ResourceValue<Brush> DisabledLow => new("SecondaryVariantLightDisabledLowBrush");
				}

				public static class VariantDark
				{
					[ResourceKeyDefinition(typeof(Brush), "SecondaryVariantDarkBrush")]
					public static ResourceValue<Brush> Default => new("SecondaryVariantDarkBrush");

					[ResourceKeyDefinition(typeof(Brush), "SecondaryVariantDarkHoverBrush")]
					public static ResourceValue<Brush> Hover => new("SecondaryVariantDarkHoverBrush");

					[ResourceKeyDefinition(typeof(Brush), "SecondaryVariantDarkFocusedBrush")]
					public static ResourceValue<Brush> Focused => new("SecondaryVariantDarkFocusedBrush");

					[ResourceKeyDefinition(typeof(Brush), "SecondaryVariantDarkPressedBrush")]
					public static ResourceValue<Brush> Pressed => new("SecondaryVariantDarkPressedBrush");

					[ResourceKeyDefinition(typeof(Brush), "SecondaryVariantDarkDraggedBrush")]
					public static ResourceValue<Brush> Dragged => new("SecondaryVariantDarkDraggedBrush");

					[ResourceKeyDefinition(typeof(Brush), "SecondaryVariantDarkSelectedBrush")]
					public static ResourceValue<Brush> Selected => new("SecondaryVariantDarkSelectedBrush");

					[ResourceKeyDefinition(typeof(Brush), "SecondaryVariantDarkMediumBrush")]
					public static ResourceValue<Brush> Medium => new("SecondaryVariantDarkMediumBrush");

					[ResourceKeyDefinition(typeof(Brush), "SecondaryVariantDarkLowBrush")]
					public static ResourceValue<Brush> Low => new("SecondaryVariantDarkLowBrush");

					[ResourceKeyDefinition(typeof(Brush), "SecondaryVariantDarkDisabledBrush")]
					public static ResourceValue<Brush> Disabled => new("SecondaryVariantDarkDisabledBrush");

					[ResourceKeyDefinition(typeof(Brush), "SecondaryVariantDarkDisabledLowBrush")]
					public static ResourceValue<Brush> DisabledLow => new("SecondaryVariantDarkDisabledLowBrush");
				}
			}

			public static class OnSecondary
			{
				[ResourceKeyDefinition(typeof(Brush), "OnSecondaryBrush")]
				public static ResourceValue<Brush> Default => new("OnSecondaryBrush");

				[ResourceKeyDefinition(typeof(Brush), "OnSecondaryHoverBrush")]
				public static ResourceValue<Brush> Hover => new("OnSecondaryHoverBrush");

				[ResourceKeyDefinition(typeof(Brush), "OnSecondaryFocusedBrush")]
				public static ResourceValue<Brush> Focused => new("OnSecondaryFocusedBrush");

				[ResourceKeyDefinition(typeof(Brush), "OnSecondaryPressedBrush")]
				public static ResourceValue<Brush> Pressed => new("OnSecondaryPressedBrush");

				[ResourceKeyDefinition(typeof(Brush), "OnSecondaryDraggedBrush")]
				public static ResourceValue<Brush> Dragged => new("OnSecondaryDraggedBrush");

				[ResourceKeyDefinition(typeof(Brush), "OnSecondarySelectedBrush")]
				public static ResourceValue<Brush> Selected => new("OnSecondarySelectedBrush");

				[ResourceKeyDefinition(typeof(Brush), "OnSecondaryMediumBrush")]
				public static ResourceValue<Brush> Medium => new("OnSecondaryMediumBrush");

				[ResourceKeyDefinition(typeof(Brush), "OnSecondaryLowBrush")]
				public static ResourceValue<Brush> Low => new("OnSecondaryLowBrush");

				[ResourceKeyDefinition(typeof(Brush), "OnSecondaryDisabledBrush")]
				public static ResourceValue<Brush> Disabled => new("OnSecondaryDisabledBrush");

				[ResourceKeyDefinition(typeof(Brush), "OnSecondaryDisabledLowBrush")]
				public static ResourceValue<Brush> DisabledLow => new("OnSecondaryDisabledLowBrush");

				public static class Container
				{
					[ResourceKeyDefinition(typeof(Brush), "OnSecondaryContainerBrush")]
					public static ResourceValue<Brush> Default => new("OnSecondaryContainerBrush");

					[ResourceKeyDefinition(typeof(Brush), "OnSecondaryContainerHoverBrush")]
					public static ResourceValue<Brush> Hover => new("OnSecondaryContainerHoverBrush");

					[ResourceKeyDefinition(typeof(Brush), "OnSecondaryContainerFocusedBrush")]
					public static ResourceValue<Brush> Focused => new("OnSecondaryContainerFocusedBrush");

					[ResourceKeyDefinition(typeof(Brush), "OnSecondaryContainerPressedBrush")]
					public static ResourceValue<Brush> Pressed => new("OnSecondaryContainerPressedBrush");

					public static ResourceValue<Brush> Dragged => new("OnSecondaryContainerDraggedBrush");

					[ResourceKeyDefinition(typeof(Brush), "OnSecondaryContainerSelectedBrush")]
					public static ResourceValue<Brush> Selected => new("OnSecondaryContainerSelectedBrush");

					[ResourceKeyDefinition(typeof(Brush), "OnSecondaryContainerMediumBrush")]
					public static ResourceValue<Brush> Medium => new("OnSecondaryContainerMediumBrush");

					[ResourceKeyDefinition(typeof(Brush), "OnSecondaryContainerLowBrush")]
					public static ResourceValue<Brush> Low => new("OnSecondaryContainerLowBrush");

					[ResourceKeyDefinition(typeof(Brush), "OnSecondaryContainerDisabledBrush")]
					public static ResourceValue<Brush> Disabled => new("OnSecondaryContainerDisabledBrush");

					[ResourceKeyDefinition(typeof(Brush), "OnSecondaryContainerDisabledLowBrush")]
					public static ResourceValue<Brush> DisabledLow => new("OnSecondaryContainerDisabledLowBrush");
				}
			}

			public static class Tertiary
			{
				[ResourceKeyDefinition(typeof(Brush), "TertiaryBrush")]
				public static ResourceValue<Brush> Default => new("TertiaryBrush");

				[ResourceKeyDefinition(typeof(Brush), "TertiaryHoverBrush")]
				public static ResourceValue<Brush> Hover => new("TertiaryHoverBrush");

				[ResourceKeyDefinition(typeof(Brush), "TertiaryFocusedBrush")]
				public static ResourceValue<Brush> Focused => new("TertiaryFocusedBrush");

				[ResourceKeyDefinition(typeof(Brush), "TertiaryPressedBrush")]
				public static ResourceValue<Brush> Pressed => new("TertiaryPressedBrush");

				[ResourceKeyDefinition(typeof(Brush), "TertiaryDraggedBrush")]
				public static ResourceValue<Brush> Dragged => new("TertiaryDraggedBrush");

				[ResourceKeyDefinition(typeof(Brush), "TertiarySelectedBrush")]
				public static ResourceValue<Brush> Selected => new("TertiarySelectedBrush");

				[ResourceKeyDefinition(typeof(Brush), "TertiaryMediumBrush")]
				public static ResourceValue<Brush> Medium => new("TertiaryMediumBrush");

				[ResourceKeyDefinition(typeof(Brush), "TertiaryLowBrush")]
				public static ResourceValue<Brush> Low => new("TertiaryLowBrush");

				[ResourceKeyDefinition(typeof(Brush), "TertiaryDisabledBrush")]
				public static ResourceValue<Brush> Disabled => new("TertiaryDisabledBrush");

				[ResourceKeyDefinition(typeof(Brush), "TertiaryDisabledLowBrush")]
				public static ResourceValue<Brush> DisabledLow => new("TertiaryDisabledLowBrush");

				public static class Container
				{
					[ResourceKeyDefinition(typeof(Brush), "TertiaryContainerBrush")]
					public static ResourceValue<Brush> Default => new("TertiaryContainerBrush");

					[ResourceKeyDefinition(typeof(Brush), "TertiaryContainerHoverBrush")]
					public static ResourceValue<Brush> Hover => new("TertiaryContainerHoverBrush");

					[ResourceKeyDefinition(typeof(Brush), "TertiaryContainerFocusedBrush")]
					public static ResourceValue<Brush> Focused => new("TertiaryContainerFocusedBrush");

					[ResourceKeyDefinition(typeof(Brush), "TertiaryContainerPressedBrush")]
					public static ResourceValue<Brush> Pressed => new("TertiaryContainerPressedBrush");

					[ResourceKeyDefinition(typeof(Brush), "TertiaryContainerDraggedBrush")]
					public static ResourceValue<Brush> Dragged => new("TertiaryContainerDraggedBrush");

					[ResourceKeyDefinition(typeof(Brush), "TertiaryContainerSelectedBrush")]
					public static ResourceValue<Brush> Selected => new("TertiaryContainerSelectedBrush");

					[ResourceKeyDefinition(typeof(Brush), "TertiaryContainerMediumBrush")]
					public static ResourceValue<Brush> Medium => new("TertiaryContainerMediumBrush");

					[ResourceKeyDefinition(typeof(Brush), "TertiaryContainerLowBrush")]
					public static ResourceValue<Brush> Low => new("TertiaryContainerLowBrush");

					[ResourceKeyDefinition(typeof(Brush), "TertiaryContainerDisabledBrush")]
					public static ResourceValue<Brush> Disabled => new("TertiaryContainerDisabledBrush");

					[ResourceKeyDefinition(typeof(Brush), "TertiaryContainerDisabledLowBrush")]
					public static ResourceValue<Brush> DisabledLow => new("TertiaryContainerDisabledLowBrush");
				}
			}

			public static class OnTertiary
			{
				[ResourceKeyDefinition(typeof(Brush), "OnTertiaryBrush")]
				public static ResourceValue<Brush> Default => new("OnTertiaryBrush");

				[ResourceKeyDefinition(typeof(Brush), "OnTertiaryHoverBrush")]
				public static ResourceValue<Brush> Hover => new("OnTertiaryHoverBrush");

				[ResourceKeyDefinition(typeof(Brush), "OnTertiaryFocusedBrush")]
				public static ResourceValue<Brush> Focused => new("OnTertiaryFocusedBrush");

				[ResourceKeyDefinition(typeof(Brush), "OnTertiaryPressedBrush")]
				public static ResourceValue<Brush> Pressed => new("OnTertiaryPressedBrush");

				[ResourceKeyDefinition(typeof(Brush), "OnTertiaryDraggedBrush")]
				public static ResourceValue<Brush> Dragged => new("OnTertiaryDraggedBrush");

				[ResourceKeyDefinition(typeof(Brush), "OnTertiarySelectedBrush")]
				public static ResourceValue<Brush> Selected => new("OnTertiarySelectedBrush");

				[ResourceKeyDefinition(typeof(Brush), "OnTertiaryMediumBrush")]
				public static ResourceValue<Brush> Medium => new("OnTertiaryMediumBrush");

				[ResourceKeyDefinition(typeof(Brush), "OnTertiaryLowBrush")]
				public static ResourceValue<Brush> Low => new("OnTertiaryLowBrush");

				[ResourceKeyDefinition(typeof(Brush), "OnTertiaryDisabledBrush")]
				public static ResourceValue<Brush> Disabled => new("OnTertiaryDisabledBrush");

				[ResourceKeyDefinition(typeof(Brush), "OnTertiaryDisabledLowBrush")]
				public static ResourceValue<Brush> DisabledLow => new("OnTertiaryDisabledLowBrush");

				public static class Container
				{
					[ResourceKeyDefinition(typeof(Brush), "OnTertiaryContainerBrush")]
					public static ResourceValue<Brush> Default => new("OnTertiaryContainerBrush");

					[ResourceKeyDefinition(typeof(Brush), "OnTertiaryContainerHoverBrush")]
					public static ResourceValue<Brush> Hover => new("OnTertiaryContainerHoverBrush");

					[ResourceKeyDefinition(typeof(Brush), "OnTertiaryContainerFocusedBrush")]
					public static ResourceValue<Brush> Focused => new("OnTertiaryContainerFocusedBrush");

					[ResourceKeyDefinition(typeof(Brush), "OnTertiaryContainerPressedBrush")]
					public static ResourceValue<Brush> Pressed => new("OnTertiaryContainerPressedBrush");

					[ResourceKeyDefinition(typeof(Brush), "OnTertiaryContainerDraggedBrush")]
					public static ResourceValue<Brush> Dragged => new("OnTertiaryContainerDraggedBrush");

					[ResourceKeyDefinition(typeof(Brush), "OnTertiaryContainerSelectedBrush")]
					public static ResourceValue<Brush> Selected => new("OnTertiaryContainerSelectedBrush");

					[ResourceKeyDefinition(typeof(Brush), "OnTertiaryContainerMediumBrush")]
					public static ResourceValue<Brush> Medium => new("OnTertiaryContainerMediumBrush");

					[ResourceKeyDefinition(typeof(Brush), "OnTertiaryContainerLowBrush")]
					public static ResourceValue<Brush> Low => new("OnTertiaryContainerLowBrush");

					[ResourceKeyDefinition(typeof(Brush), "OnTertiaryContainerDisabledBrush")]
					public static ResourceValue<Brush> Disabled => new("OnTertiaryContainerDisabledBrush");

					[ResourceKeyDefinition(typeof(Brush), "OnTertiaryContainerDisabledLowBrush")]
					public static ResourceValue<Brush> DisabledLow => new("OnTertiaryContainerDisabledLowBrush");
				}
			}

			public static class Error
			{
				[ResourceKeyDefinition(typeof(Brush), "ErrorBrush")]
				public static ResourceValue<Brush> Default => new("ErrorBrush");

				[ResourceKeyDefinition(typeof(Brush), "ErrorHoverBrush")]
				public static ResourceValue<Brush> Hover => new("ErrorHoverBrush");

				[ResourceKeyDefinition(typeof(Brush), "ErrorFocusedBrush")]
				public static ResourceValue<Brush> Focused => new("ErrorFocusedBrush");

				[ResourceKeyDefinition(typeof(Brush), "ErrorPressedBrush")]
				public static ResourceValue<Brush> Pressed => new("ErrorPressedBrush");

				[ResourceKeyDefinition(typeof(Brush), "ErrorDraggedBrush")]
				public static ResourceValue<Brush> Dragged => new("ErrorDraggedBrush");

				[ResourceKeyDefinition(typeof(Brush), "ErrorSelectedBrush")]
				public static ResourceValue<Brush> Selected => new("ErrorSelectedBrush");

				[ResourceKeyDefinition(typeof(Brush), "ErrorMediumBrush")]
				public static ResourceValue<Brush> Medium => new("ErrorMediumBrush");

				[ResourceKeyDefinition(typeof(Brush), "ErrorLowBrush")]
				public static ResourceValue<Brush> Low => new("ErrorLowBrush");

				[ResourceKeyDefinition(typeof(Brush), "ErrorDisabledBrush")]
				public static ResourceValue<Brush> Disabled => new("ErrorDisabledBrush");

				[ResourceKeyDefinition(typeof(Brush), "ErrorDisabledLowBrush")]
				public static ResourceValue<Brush> DisabledLow => new("ErrorDisabledLowBrush");

				public static class Container
				{
					[ResourceKeyDefinition(typeof(Brush), "ErrorContainerBrush")]
					public static ResourceValue<Brush> Default => new("ErrorContainerBrush");

					[ResourceKeyDefinition(typeof(Brush), "ErrorContainerHoverBrush")]
					public static ResourceValue<Brush> Hover => new("ErrorContainerHoverBrush");

					[ResourceKeyDefinition(typeof(Brush), "ErrorContainerFocusedBrush")]
					public static ResourceValue<Brush> Focused => new("ErrorContainerFocusedBrush");

					[ResourceKeyDefinition(typeof(Brush), "ErrorContainerPressedBrush")]
					public static ResourceValue<Brush> Pressed => new("ErrorContainerPressedBrush");

					[ResourceKeyDefinition(typeof(Brush), "ErrorContainerDraggedBrush")]
					public static ResourceValue<Brush> Dragged => new("ErrorContainerDraggedBrush");

					[ResourceKeyDefinition(typeof(Brush), "ErrorContainerSelectedBrush")]
					public static ResourceValue<Brush> Selected => new("ErrorContainerSelectedBrush");

					[ResourceKeyDefinition(typeof(Brush), "ErrorContainerMediumBrush")]
					public static ResourceValue<Brush> Medium => new("ErrorContainerMediumBrush");

					[ResourceKeyDefinition(typeof(Brush), "ErrorContainerLowBrush")]
					public static ResourceValue<Brush> Low => new("ErrorContainerLowBrush");

					[ResourceKeyDefinition(typeof(Brush), "ErrorContainerDisabledBrush")]
					public static ResourceValue<Brush> Disabled => new("ErrorContainerDisabledBrush");

					[ResourceKeyDefinition(typeof(Brush), "ErrorContainerDisabledLowBrush")]
					public static ResourceValue<Brush> DisabledLow => new("ErrorContainerDisabledLowBrush");
				}
			}

			public static class OnError
			{
				[ResourceKeyDefinition(typeof(Brush), "OnErrorBrush")]
				public static ResourceValue<Brush> Default => new("OnErrorBrush");

				[ResourceKeyDefinition(typeof(Brush), "OnErrorHoverBrush")]
				public static ResourceValue<Brush> Hover => new("OnErrorHoverBrush");

				[ResourceKeyDefinition(typeof(Brush), "OnErrorFocusedBrush")]
				public static ResourceValue<Brush> Focused => new("OnErrorFocusedBrush");

				[ResourceKeyDefinition(typeof(Brush), "OnErrorPressedBrush")]
				public static ResourceValue<Brush> Pressed => new("OnErrorPressedBrush");

				[ResourceKeyDefinition(typeof(Brush), "OnErrorDraggedBrush")]
				public static ResourceValue<Brush> Dragged => new("OnErrorDraggedBrush");

				[ResourceKeyDefinition(typeof(Brush), "OnErrorSelectedBrush")]
				public static ResourceValue<Brush> Selected => new("OnErrorSelectedBrush");

				[ResourceKeyDefinition(typeof(Brush), "OnErrorMediumBrush")]
				public static ResourceValue<Brush> Medium => new("OnErrorMediumBrush");

				[ResourceKeyDefinition(typeof(Brush), "OnErrorLowBrush")]
				public static ResourceValue<Brush> Low => new("OnErrorLowBrush");

				[ResourceKeyDefinition(typeof(Brush), "OnErrorDisabledBrush")]
				public static ResourceValue<Brush> Disabled => new("OnErrorDisabledBrush");

				[ResourceKeyDefinition(typeof(Brush), "OnErrorDisabledLowBrush")]
				public static ResourceValue<Brush> DisabledLow => new("OnErrorDisabledLowBrush");

				public static class Container
				{
					[ResourceKeyDefinition(typeof(Brush), "OnErrorContainerBrush")]
					public static ResourceValue<Brush> Default => new("OnErrorContainerBrush");

					[ResourceKeyDefinition(typeof(Brush), "OnErrorContainerHoverBrush")]
					public static ResourceValue<Brush> Hover => new("OnErrorContainerHoverBrush");

					[ResourceKeyDefinition(typeof(Brush), "OnErrorContainerFocusedBrush")]
					public static ResourceValue<Brush> Focused => new("OnErrorContainerFocusedBrush");

					[ResourceKeyDefinition(typeof(Brush), "OnErrorContainerPressedBrush")]
					public static ResourceValue<Brush> Pressed => new("OnErrorContainerPressedBrush");

					[ResourceKeyDefinition(typeof(Brush), "OnErrorContainerDraggedBrush")]
					public static ResourceValue<Brush> Dragged => new("OnErrorContainerDraggedBrush");

					[ResourceKeyDefinition(typeof(Brush), "OnErrorContainerSelectedBrush")]
					public static ResourceValue<Brush> Selected => new("OnErrorContainerSelectedBrush");

					[ResourceKeyDefinition(typeof(Brush), "OnErrorContainerMediumBrush")]
					public static ResourceValue<Brush> Medium => new("OnErrorContainerMediumBrush");

					[ResourceKeyDefinition(typeof(Brush), "OnErrorContainerLowBrush")]
					public static ResourceValue<Brush> Low => new("OnErrorContainerLowBrush");

					[ResourceKeyDefinition(typeof(Brush), "OnErrorContainerDisabledBrush")]
					public static ResourceValue<Brush> Disabled => new("OnErrorContainerDisabledBrush");

					[ResourceKeyDefinition(typeof(Brush), "OnErrorContainerDisabledLowBrush")]
					public static ResourceValue<Brush> DisabledLow => new("OnErrorContainerDisabledLowBrush");
				}
			}

			public static class Background
			{
				[ResourceKeyDefinition(typeof(Brush), "BackgroundBrush")]
				public static ResourceValue<Brush> Default => new("BackgroundBrush");

				[ResourceKeyDefinition(typeof(Brush), "BackgroundHoverBrush")]
				public static ResourceValue<Brush> Hover => new("BackgroundHoverBrush");

				[ResourceKeyDefinition(typeof(Brush), "BackgroundFocusedBrush")]
				public static ResourceValue<Brush> Focused => new("BackgroundFocusedBrush");

				[ResourceKeyDefinition(typeof(Brush), "BackgroundPressedBrush")]
				public static ResourceValue<Brush> Pressed => new("BackgroundPressedBrush");

				[ResourceKeyDefinition(typeof(Brush), "BackgroundDraggedBrush")]
				public static ResourceValue<Brush> Dragged => new("BackgroundDraggedBrush");

				[ResourceKeyDefinition(typeof(Brush), "BackgroundSelectedBrush")]
				public static ResourceValue<Brush> Selected => new("BackgroundSelectedBrush");

				[ResourceKeyDefinition(typeof(Brush), "BackgroundMediumBrush")]
				public static ResourceValue<Brush> Medium => new("BackgroundMediumBrush");

				[ResourceKeyDefinition(typeof(Brush), "BackgroundLowBrush")]
				public static ResourceValue<Brush> Low => new("BackgroundLowBrush");

				[ResourceKeyDefinition(typeof(Brush), "BackgroundDisabledBrush")]
				public static ResourceValue<Brush> Disabled => new("BackgroundDisabledBrush");

				[ResourceKeyDefinition(typeof(Brush), "BackgroundDisabledLowBrush")]
				public static ResourceValue<Brush> DisabledLow => new("BackgroundDisabledLowBrush");
			}

			public static class OnBackground
			{
				[ResourceKeyDefinition(typeof(Brush), "OnBackgroundBrush")]
				public static ResourceValue<Brush> Default => new("OnBackgroundBrush");

				[ResourceKeyDefinition(typeof(Brush), "OnBackgroundHoverBrush")]
				public static ResourceValue<Brush> Hover => new("OnBackgroundHoverBrush");

				[ResourceKeyDefinition(typeof(Brush), "OnBackgroundFocusedBrush")]
				public static ResourceValue<Brush> Focused => new("OnBackgroundFocusedBrush");

				[ResourceKeyDefinition(typeof(Brush), "OnBackgroundPressedBrush")]
				public static ResourceValue<Brush> Pressed => new("OnBackgroundPressedBrush");

				[ResourceKeyDefinition(typeof(Brush), "OnBackgroundDraggedBrush")]
				public static ResourceValue<Brush> Dragged => new("OnBackgroundDraggedBrush");

				[ResourceKeyDefinition(typeof(Brush), "OnBackgroundSelectedBrush")]
				public static ResourceValue<Brush> Selected => new("OnBackgroundSelectedBrush");

				[ResourceKeyDefinition(typeof(Brush), "OnBackgroundMediumBrush")]
				public static ResourceValue<Brush> Medium => new("OnBackgroundMediumBrush");

				[ResourceKeyDefinition(typeof(Brush), "OnBackgroundLowBrush")]
				public static ResourceValue<Brush> Low => new("OnBackgroundLowBrush");

				[ResourceKeyDefinition(typeof(Brush), "OnBackgroundDisabledBrush")]
				public static ResourceValue<Brush> Disabled => new("OnBackgroundDisabledBrush");

				[ResourceKeyDefinition(typeof(Brush), "OnBackgroundDisabledLowBrush")]
				public static ResourceValue<Brush> DisabledLow => new("OnBackgroundDisabledLowBrush");
			}

			public static class Surface
			{
				[ResourceKeyDefinition(typeof(Brush), "SurfaceBrush")]
				public static ResourceValue<Brush> Default => new("SurfaceBrush");

				[ResourceKeyDefinition(typeof(Brush), "SurfaceHoverBrush")]
				public static ResourceValue<Brush> Hover => new("SurfaceHoverBrush");

				[ResourceKeyDefinition(typeof(Brush), "SurfaceFocusedBrush")]
				public static ResourceValue<Brush> Focused => new("SurfaceFocusedBrush");

				[ResourceKeyDefinition(typeof(Brush), "SurfacePressedBrush")]
				public static ResourceValue<Brush> Pressed => new("SurfacePressedBrush");

				[ResourceKeyDefinition(typeof(Brush), "SurfaceDraggedBrush")]
				public static ResourceValue<Brush> Dragged => new("SurfaceDraggedBrush");

				[ResourceKeyDefinition(typeof(Brush), "SurfaceSelectedBrush")]
				public static ResourceValue<Brush> Selected => new("SurfaceSelectedBrush");

				[ResourceKeyDefinition(typeof(Brush), "SurfaceMediumBrush")]
				public static ResourceValue<Brush> Medium => new("SurfaceMediumBrush");

				[ResourceKeyDefinition(typeof(Brush), "SurfaceLowBrush")]
				public static ResourceValue<Brush> Low => new("SurfaceLowBrush");

				[ResourceKeyDefinition(typeof(Brush), "SurfaceDisabledBrush")]
				public static ResourceValue<Brush> Disabled => new("SurfaceDisabledBrush");

				[ResourceKeyDefinition(typeof(Brush), "SurfaceDisabledLowBrush")]
				public static ResourceValue<Brush> DisabledLow => new("SurfaceDisabledLowBrush");

				public static class Variant
				{
					[ResourceKeyDefinition(typeof(Brush), "SurfaceVariantBrush")]
					public static ResourceValue<Brush> Default => new("SurfaceVariantBrush");

					[ResourceKeyDefinition(typeof(Brush), "SurfaceVariantHoverBrush")]
					public static ResourceValue<Brush> Hover => new("SurfaceVariantHoverBrush");

					[ResourceKeyDefinition(typeof(Brush), "SurfaceVariantFocusedBrush")]
					public static ResourceValue<Brush> Focused => new("SurfaceVariantFocusedBrush");

					[ResourceKeyDefinition(typeof(Brush), "SurfaceVariantPressedBrush")]
					public static ResourceValue<Brush> Pressed => new("SurfaceVariantPressedBrush");

					[ResourceKeyDefinition(typeof(Brush), "SurfaceVariantDraggedBrush")]
					public static ResourceValue<Brush> Dragged => new("SurfaceVariantDraggedBrush");

					[ResourceKeyDefinition(typeof(Brush), "SurfaceVariantSelectedBrush")]
					public static ResourceValue<Brush> Selected => new("SurfaceVariantSelectedBrush");

					[ResourceKeyDefinition(typeof(Brush), "SurfaceVariantMediumBrush")]
					public static ResourceValue<Brush> Medium => new("SurfaceVariantMediumBrush");

					[ResourceKeyDefinition(typeof(Brush), "SurfaceVariantLowBrush")]
					public static ResourceValue<Brush> Low => new("SurfaceVariantLowBrush");

					[ResourceKeyDefinition(typeof(Brush), "SurfaceVariantDisabledBrush")]
					public static ResourceValue<Brush> Disabled => new("SurfaceVariantDisabledBrush");

					[ResourceKeyDefinition(typeof(Brush), "SurfaceVariantDisabledLowBrush")]
					public static ResourceValue<Brush> DisabledLow => new("SurfaceVariantDisabledLowBrush");
				}

				public static class Inverse
				{
					[ResourceKeyDefinition(typeof(Brush), "SurfaceInverseBrush")]
					public static ResourceValue<Brush> Default => new("SurfaceInverseBrush");

					[ResourceKeyDefinition(typeof(Brush), "SurfaceInverseHoverBrush")]
					public static ResourceValue<Brush> Hover => new("SurfaceInverseHoverBrush");

					[ResourceKeyDefinition(typeof(Brush), "SurfaceInverseFocusedBrush")]
					public static ResourceValue<Brush> Focused => new("SurfaceInverseFocusedBrush");

					[ResourceKeyDefinition(typeof(Brush), "SurfaceInversePressedBrush")]
					public static ResourceValue<Brush> Pressed => new("SurfaceInversePressedBrush");

					[ResourceKeyDefinition(typeof(Brush), "SurfaceInverseDraggedBrush")]
					public static ResourceValue<Brush> Dragged => new("SurfaceInverseDraggedBrush");

					[ResourceKeyDefinition(typeof(Brush), "SurfaceInverseSelectedBrush")]
					public static ResourceValue<Brush> Selected => new("SurfaceInverseSelectedBrush");

					[ResourceKeyDefinition(typeof(Brush), "SurfaceInverseMediumBrush")]
					public static ResourceValue<Brush> Medium => new("SurfaceInverseMediumBrush");

					[ResourceKeyDefinition(typeof(Brush), "SurfaceInverseLowBrush")]
					public static ResourceValue<Brush> Low => new("SurfaceInverseLowBrush");

					[ResourceKeyDefinition(typeof(Brush), "SurfaceInverseDisabledBrush")]
					public static ResourceValue<Brush> Disabled => new("SurfaceInverseDisabledBrush");

					[ResourceKeyDefinition(typeof(Brush), "SurfaceInverseDisabledLowBrush")]
					public static ResourceValue<Brush> DisabledLow => new("SurfaceInverseDisabledLowBrush");
				}
			}

			public static class OnSurface
			{
				[ResourceKeyDefinition(typeof(Brush), "OnSurfaceBrush")]
				public static ResourceValue<Brush> Default => new("OnSurfaceBrush");

				[ResourceKeyDefinition(typeof(Brush), "OnSurfaceHoverBrush")]
				public static ResourceValue<Brush> Hover => new("OnSurfaceHoverBrush");

				[ResourceKeyDefinition(typeof(Brush), "OnSurfaceFocusedBrush")]
				public static ResourceValue<Brush> Focused => new("OnSurfaceFocusedBrush");

				[ResourceKeyDefinition(typeof(Brush), "OnSurfacePressedBrush")]
				public static ResourceValue<Brush> Pressed => new("OnSurfacePressedBrush");

				[ResourceKeyDefinition(typeof(Brush), "OnSurfaceDraggedBrush")]
				public static ResourceValue<Brush> Dragged => new("OnSurfaceDraggedBrush");

				[ResourceKeyDefinition(typeof(Brush), "OnSurfaceSelectedBrush")]
				public static ResourceValue<Brush> Selected => new("OnSurfaceSelectedBrush");

				[ResourceKeyDefinition(typeof(Brush), "OnSurfaceMediumBrush")]
				public static ResourceValue<Brush> Medium => new("OnSurfaceMediumBrush");

				[ResourceKeyDefinition(typeof(Brush), "OnSurfaceLowBrush")]
				public static ResourceValue<Brush> Low => new("OnSurfaceLowBrush");

				[ResourceKeyDefinition(typeof(Brush), "OnSurfaceDisabledBrush")]
				public static ResourceValue<Brush> Disabled => new("OnSurfaceDisabledBrush");

				[ResourceKeyDefinition(typeof(Brush), "OnSurfaceDisabledLowBrush")]
				public static ResourceValue<Brush> DisabledLow => new("OnSurfaceDisabledLowBrush");

				public static class Variant
				{
					[ResourceKeyDefinition(typeof(Brush), "OnSurfaceVariantBrush")]
					public static ResourceValue<Brush> Default => new("OnSurfaceVariantBrush");

					[ResourceKeyDefinition(typeof(Brush), "OnSurfaceVariantHoverBrush")]
					public static ResourceValue<Brush> Hover => new("OnSurfaceVariantHoverBrush");

					[ResourceKeyDefinition(typeof(Brush), "OnSurfaceVariantFocusedBrush")]
					public static ResourceValue<Brush> Focused => new("OnSurfaceVariantFocusedBrush");

					[ResourceKeyDefinition(typeof(Brush), "OnSurfaceVariantPressedBrush")]
					public static ResourceValue<Brush> Pressed => new("OnSurfaceVariantPressedBrush");

					[ResourceKeyDefinition(typeof(Brush), "OnSurfaceVariantDraggedBrush")]
					public static ResourceValue<Brush> Dragged => new("OnSurfaceVariantDraggedBrush");

					[ResourceKeyDefinition(typeof(Brush), "OnSurfaceVariantSelectedBrush")]
					public static ResourceValue<Brush> Selected => new("OnSurfaceVariantSelectedBrush");

					[ResourceKeyDefinition(typeof(Brush), "OnSurfaceVariantMediumBrush")]
					public static ResourceValue<Brush> Medium => new("OnSurfaceVariantMediumBrush");

					[ResourceKeyDefinition(typeof(Brush), "OnSurfaceVariantLowBrush")]
					public static ResourceValue<Brush> Low => new("OnSurfaceVariantLowBrush");

					[ResourceKeyDefinition(typeof(Brush), "OnSurfaceVariantDisabledBrush")]
					public static ResourceValue<Brush> Disabled => new("OnSurfaceVariantDisabledBrush");

					[ResourceKeyDefinition(typeof(Brush), "OnSurfaceVariantDisabledLowBrush")]
					public static ResourceValue<Brush> DisabledLow => new("OnSurfaceVariantDisabledLowBrush");
				}

				public static class Inverse
				{
					[ResourceKeyDefinition(typeof(Brush), "OnSurfaceInverseBrush")]
					public static ResourceValue<Brush> Default => new("OnSurfaceInverseBrush");

					[ResourceKeyDefinition(typeof(Brush), "OnSurfaceInverseHoverBrush")]
					public static ResourceValue<Brush> Hover => new("OnSurfaceInverseHoverBrush");

					[ResourceKeyDefinition(typeof(Brush), "OnSurfaceInverseFocusedBrush")]
					public static ResourceValue<Brush> Focused => new("OnSurfaceInverseFocusedBrush");

					[ResourceKeyDefinition(typeof(Brush), "OnSurfaceInversePressedBrush")]
					public static ResourceValue<Brush> Pressed => new("OnSurfaceInversePressedBrush");

					[ResourceKeyDefinition(typeof(Brush), "OnSurfaceInverseDraggedBrush")]
					public static ResourceValue<Brush> Dragged => new("OnSurfaceInverseDraggedBrush");

					[ResourceKeyDefinition(typeof(Brush), "OnSurfaceInverseSelectedBrush")]
					public static ResourceValue<Brush> Selected => new("OnSurfaceInverseSelectedBrush");

					[ResourceKeyDefinition(typeof(Brush), "OnSurfaceInverseMediumBrush")]
					public static ResourceValue<Brush> Medium => new("OnSurfaceInverseMediumBrush");

					[ResourceKeyDefinition(typeof(Brush), "OnSurfaceInverseLowBrush")]
					public static ResourceValue<Brush> Low => new("OnSurfaceInverseLowBrush");

					[ResourceKeyDefinition(typeof(Brush), "OnSurfaceInverseDisabledBrush")]
					public static ResourceValue<Brush> Disabled => new("OnSurfaceInverseDisabledBrush");

					[ResourceKeyDefinition(typeof(Brush), "OnSurfaceInverseDisabledLowBrush")]
					public static ResourceValue<Brush> DisabledLow => new("OnSurfaceInverseDisabledLowBrush");
				}
			}

			public static class Outline
			{
				[ResourceKeyDefinition(typeof(Brush), "OutlineBrush")]
				public static ResourceValue<Brush> Default => new("OutlineBrush");

				[ResourceKeyDefinition(typeof(Brush), "OutlineHoverBrush")]
				public static ResourceValue<Brush> Hover => new("OutlineHoverBrush");

				[ResourceKeyDefinition(typeof(Brush), "OutlineFocusedBrush")]
				public static ResourceValue<Brush> Focused => new("OutlineFocusedBrush");

				[ResourceKeyDefinition(typeof(Brush), "OutlinePressedBrush")]
				public static ResourceValue<Brush> Pressed => new("OutlinePressedBrush");

				[ResourceKeyDefinition(typeof(Brush), "OutlineDraggedBrush")]
				public static ResourceValue<Brush> Dragged => new("OutlineDraggedBrush");

				[ResourceKeyDefinition(typeof(Brush), "OutlineSelectedBrush")]
				public static ResourceValue<Brush> Selected => new("OutlineSelectedBrush");

				[ResourceKeyDefinition(typeof(Brush), "OutlineMediumBrush")]
				public static ResourceValue<Brush> Medium => new("OutlineMediumBrush");

				[ResourceKeyDefinition(typeof(Brush), "OutlineLowBrush")]
				public static ResourceValue<Brush> Low => new("OutlineLowBrush");

				[ResourceKeyDefinition(typeof(Brush), "OutlineDisabledBrush")]
				public static ResourceValue<Brush> Disabled => new("OutlineDisabledBrush");

				[ResourceKeyDefinition(typeof(Brush), "OutlineDisabledLowBrush")]
				public static ResourceValue<Brush> DisabledLow => new("OutlineDisabledLowBrush");
			}
		}
	}
}
