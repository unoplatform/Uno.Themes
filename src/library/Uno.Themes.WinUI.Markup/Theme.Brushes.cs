using Microsoft.UI.Xaml.Media;
using Uno.Extensions.Markup.Internals;

namespace Uno.Themes.Markup
{
	partial class Theme
	{
		public static class Brushes
		{
			public static class Primary
			{
				[ResourceKeyDefinition(typeof(Brush), "PrimaryBrush")]
				public static ResourceValue<Brush> Default => new("PrimaryBrush", true);

				[ResourceKeyDefinition(typeof(Brush), "PrimaryHoverBrush")]
				public static ResourceValue<Brush> Hover => new("PrimaryHoverBrush", true);

				[ResourceKeyDefinition(typeof(Brush), "PrimaryFocusedBrush")]
				public static ResourceValue<Brush> Focused => new("PrimaryFocusedBrush", true);

				[ResourceKeyDefinition(typeof(Brush), "PrimaryPressedBrush")]
				public static ResourceValue<Brush> Pressed => new("PrimaryPressedBrush", true);

				[ResourceKeyDefinition(typeof(Brush), "PrimaryDraggedBrush")]
				public static ResourceValue<Brush> Dragged => new("PrimaryDraggedBrush", true);

				[ResourceKeyDefinition(typeof(Brush), "PrimarySelectedBrush")]
				public static ResourceValue<Brush> Selected => new("PrimarySelectedBrush", true);

				[ResourceKeyDefinition(typeof(Brush), "PrimaryMediumBrush")]
				public static ResourceValue<Brush> Medium => new("PrimaryMediumBrush", true);

				[ResourceKeyDefinition(typeof(Brush), "PrimaryLowBrush")]
				public static ResourceValue<Brush> Low => new("PrimaryLowBrush", true);

				[ResourceKeyDefinition(typeof(Brush), "PrimaryDisabledBrush")]
				public static ResourceValue<Brush> Disabled => new("PrimaryDisabledBrush", true);

				[ResourceKeyDefinition(typeof(Brush), "PrimaryDisabledLowBrush")]
				public static ResourceValue<Brush> DisabledLow => new("PrimaryDisabledLowBrush", true);

				public static class Inverse
				{
					[ResourceKeyDefinition(typeof(Brush), "PrimaryInverseBrush")]
					public static ResourceValue<Brush> Default => new("PrimaryInverseBrush", true);

					[ResourceKeyDefinition(typeof(Brush), "PrimaryInverseHoverBrush")]
					public static ResourceValue<Brush> Hover => new("PrimaryInverseHoverBrush", true);

					[ResourceKeyDefinition(typeof(Brush), "PrimaryInverseFocusedBrush")]
					public static ResourceValue<Brush> Focused => new("PrimaryInverseFocusedBrush", true);

					[ResourceKeyDefinition(typeof(Brush), "PrimaryInversePressedBrush")]
					public static ResourceValue<Brush> Pressed => new("PrimaryInversePressedBrush", true);

					[ResourceKeyDefinition(typeof(Brush), "PrimaryInverseDraggedBrush")]
					public static ResourceValue<Brush> Dragged => new("PrimaryInverseDraggedBrush", true);

					[ResourceKeyDefinition(typeof(Brush), "PrimaryInverseSelectedBrush")]
					public static ResourceValue<Brush> Selected => new("PrimaryInverseSelectedBrush", true);

					[ResourceKeyDefinition(typeof(Brush), "PrimaryInverseMediumBrush")]
					public static ResourceValue<Brush> Medium => new("PrimaryInverseMediumBrush", true);

					[ResourceKeyDefinition(typeof(Brush), "PrimaryInverseLowBrush")]
					public static ResourceValue<Brush> Low => new("PrimaryInverseLowBrush", true);

					[ResourceKeyDefinition(typeof(Brush), "PrimaryInverseDisabledBrush")]
					public static ResourceValue<Brush> Disabled => new("PrimaryInverseDisabledBrush", true);

					[ResourceKeyDefinition(typeof(Brush), "PrimaryInverseDisabledLowBrush")]
					public static ResourceValue<Brush> DisabledLow => new("PrimaryInverseDisabledLowBrush", true);
				}

				public static class Container
				{
					[ResourceKeyDefinition(typeof(Brush), "PrimaryContainerBrush")]
					public static ResourceValue<Brush> Default => new("PrimaryContainerBrush", true);

					[ResourceKeyDefinition(typeof(Brush), "PrimaryContainerHoverBrush")]
					public static ResourceValue<Brush> Hover => new("PrimaryContainerHoverBrush", true);

					[ResourceKeyDefinition(typeof(Brush), "PrimaryContainerFocusedBrush")]
					public static ResourceValue<Brush> Focused => new("PrimaryContainerFocusedBrush", true);

					[ResourceKeyDefinition(typeof(Brush), "PrimaryContainerPressedBrush")]
					public static ResourceValue<Brush> Pressed => new("PrimaryContainerPressedBrush", true);

					[ResourceKeyDefinition(typeof(Brush), "PrimaryContainerDraggedBrush")]
					public static ResourceValue<Brush> Dragged => new("PrimaryContainerDraggedBrush", true);

					[ResourceKeyDefinition(typeof(Brush), "PrimaryContainerSelectedBrush")]
					public static ResourceValue<Brush> Selected => new("PrimaryContainerSelectedBrush", true);

					[ResourceKeyDefinition(typeof(Brush), "PrimaryContainerMediumBrush")]
					public static ResourceValue<Brush> Medium => new("PrimaryContainerMediumBrush", true);

					[ResourceKeyDefinition(typeof(Brush), "PrimaryContainerLowBrush")]
					public static ResourceValue<Brush> Low => new("PrimaryContainerLowBrush", true);

					[ResourceKeyDefinition(typeof(Brush), "PrimaryContainerDisabledBrush")]
					public static ResourceValue<Brush> Disabled => new("PrimaryContainerDisabledBrush", true);

					[ResourceKeyDefinition(typeof(Brush), "PrimaryContainerDisabledLowBrush")]
					public static ResourceValue<Brush> DisabledLow => new("PrimaryContainerDisabledLowBrush", true);
				}

				public static class VariantLight
				{
					[ResourceKeyDefinition(typeof(Brush), "PrimaryVariantLightBrush")]
					public static ResourceValue<Brush> Default => new("PrimaryVariantLightBrush", true);

					[ResourceKeyDefinition(typeof(Brush), "PrimaryVariantLightHoverBrush")]
					public static ResourceValue<Brush> Hover => new("PrimaryVariantLightHoverBrush", true);

					[ResourceKeyDefinition(typeof(Brush), "PrimaryVariantLightFocusedBrush")]
					public static ResourceValue<Brush> Focused => new("PrimaryVariantLightFocusedBrush", true);

					[ResourceKeyDefinition(typeof(Brush), "PrimaryVariantLightPressedBrush")]
					public static ResourceValue<Brush> Pressed => new("PrimaryVariantLightPressedBrush", true);

					[ResourceKeyDefinition(typeof(Brush), "PrimaryVariantLightDraggedBrush")]
					public static ResourceValue<Brush> Dragged => new("PrimaryVariantLightDraggedBrush", true);

					[ResourceKeyDefinition(typeof(Brush), "PrimaryVariantLightSelectedBrush")]
					public static ResourceValue<Brush> Selected => new("PrimaryVariantLightSelectedBrush", true);

					[ResourceKeyDefinition(typeof(Brush), "PrimaryVariantLightMediumBrush")]
					public static ResourceValue<Brush> Medium => new("PrimaryVariantLightMediumBrush", true);

					[ResourceKeyDefinition(typeof(Brush), "PrimaryVariantLightLowBrush")]
					public static ResourceValue<Brush> Low => new("PrimaryVariantLightLowBrush", true);

					[ResourceKeyDefinition(typeof(Brush), "PrimaryVariantLightDisabledBrush")]
					public static ResourceValue<Brush> Disabled => new("PrimaryVariantLightDisabledBrush", true);

					[ResourceKeyDefinition(typeof(Brush), "PrimaryVariantLightDisabledLowBrush")]
					public static ResourceValue<Brush> DisabledLow => new("PrimaryVariantLightDisabledLowBrush", true);
				}

				public static class VariantDark
				{
					[ResourceKeyDefinition(typeof(Brush), "PrimaryVariantDarkBrush")]
					public static ResourceValue<Brush> Default => new("PrimaryVariantDarkBrush", true);

					[ResourceKeyDefinition(typeof(Brush), "PrimaryVariantDarkHoverBrush")]
					public static ResourceValue<Brush> Hover => new("PrimaryVariantDarkHoverBrush", true);

					[ResourceKeyDefinition(typeof(Brush), "PrimaryVariantDarkFocusedBrush")]
					public static ResourceValue<Brush> Focused => new("PrimaryVariantDarkFocusedBrush", true);

					[ResourceKeyDefinition(typeof(Brush), "PrimaryVariantDarkPressedBrush")]
					public static ResourceValue<Brush> Pressed => new("PrimaryVariantDarkPressedBrush", true);

					[ResourceKeyDefinition(typeof(Brush), "PrimaryVariantDarkDraggedBrush")]
					public static ResourceValue<Brush> Dragged => new("PrimaryVariantDarkDraggedBrush", true);

					[ResourceKeyDefinition(typeof(Brush), "PrimaryVariantDarkSelectedBrush")]
					public static ResourceValue<Brush> Selected => new("PrimaryVariantDarkSelectedBrush", true);

					[ResourceKeyDefinition(typeof(Brush), "PrimaryVariantDarkMediumBrush")]
					public static ResourceValue<Brush> Medium => new("PrimaryVariantDarkMediumBrush", true);

					[ResourceKeyDefinition(typeof(Brush), "PrimaryVariantDarkLowBrush")]
					public static ResourceValue<Brush> Low => new("PrimaryVariantDarkLowBrush", true);

					[ResourceKeyDefinition(typeof(Brush), "PrimaryVariantDarkDisabledBrush")]
					public static ResourceValue<Brush> Disabled => new("PrimaryVariantDarkDisabledBrush", true);

					[ResourceKeyDefinition(typeof(Brush), "PrimaryVariantDarkDisabledLowBrush")]
					public static ResourceValue<Brush> DisabledLow => new("PrimaryVariantDarkDisabledLowBrush", true);
				}
			}

			public static class OnPrimary
			{
				[ResourceKeyDefinition(typeof(Brush), "OnPrimaryBrush")]
				public static ResourceValue<Brush> Default => new("OnPrimaryBrush", true);

				[ResourceKeyDefinition(typeof(Brush), "OnPrimaryHoverBrush")]
				public static ResourceValue<Brush> Hover => new("OnPrimaryHoverBrush", true);

				[ResourceKeyDefinition(typeof(Brush), "OnPrimaryFocusedBrush")]
				public static ResourceValue<Brush> Focused => new("OnPrimaryFocusedBrush", true);

				[ResourceKeyDefinition(typeof(Brush), "OnPrimaryPressedBrush")]
				public static ResourceValue<Brush> Pressed => new("OnPrimaryPressedBrush", true);

				[ResourceKeyDefinition(typeof(Brush), "OnPrimaryDraggedBrush")]
				public static ResourceValue<Brush> Dragged => new("OnPrimaryDraggedBrush", true);

				[ResourceKeyDefinition(typeof(Brush), "OnPrimarySelectedBrush")]
				public static ResourceValue<Brush> Selected => new("OnPrimarySelectedBrush", true);

				[ResourceKeyDefinition(typeof(Brush), "OnPrimaryMediumBrush")]
				public static ResourceValue<Brush> Medium => new("OnPrimaryMediumBrush", true);

				[ResourceKeyDefinition(typeof(Brush), "OnPrimaryLowBrush")]
				public static ResourceValue<Brush> Low => new("OnPrimaryLowBrush", true);

				[ResourceKeyDefinition(typeof(Brush), "OnPrimaryDisabledBrush")]
				public static ResourceValue<Brush> Disabled => new("OnPrimaryDisabledBrush", true);

				[ResourceKeyDefinition(typeof(Brush), "OnPrimaryDisabledLowBrush")]
				public static ResourceValue<Brush> DisabledLow => new("OnPrimaryDisabledLowBrush", true);

				public static class Container
				{
					[ResourceKeyDefinition(typeof(Brush), "OnPrimaryContainerBrush")]
					public static ResourceValue<Brush> Default => new("OnPrimaryContainerBrush", true);

					[ResourceKeyDefinition(typeof(Brush), "OnPrimaryContainerHoverBrush")]
					public static ResourceValue<Brush> Hover => new("OnPrimaryContainerHoverBrush", true);

					[ResourceKeyDefinition(typeof(Brush), "OnPrimaryContainerFocusedBrush")]
					public static ResourceValue<Brush> Focused => new("OnPrimaryContainerFocusedBrush", true);

					[ResourceKeyDefinition(typeof(Brush), "OnPrimaryContainerPressedBrush")]
					public static ResourceValue<Brush> Pressed => new("OnPrimaryContainerPressedBrush", true);

					[ResourceKeyDefinition(typeof(Brush), "OnPrimaryContainerDraggedBrush")]
					public static ResourceValue<Brush> Dragged => new("OnPrimaryContainerDraggedBrush", true);

					[ResourceKeyDefinition(typeof(Brush), "OnPrimaryContainerSelectedBrush")]
					public static ResourceValue<Brush> Selected => new("OnPrimaryContainerSelectedBrush", true);

					[ResourceKeyDefinition(typeof(Brush), "OnPrimaryContainerMediumBrush")]
					public static ResourceValue<Brush> Medium => new("OnPrimaryContainerMediumBrush", true);

					[ResourceKeyDefinition(typeof(Brush), "OnPrimaryContainerLowBrush")]
					public static ResourceValue<Brush> Low => new("OnPrimaryContainerLowBrush", true);

					[ResourceKeyDefinition(typeof(Brush), "OnPrimaryContainerDisabledBrush")]
					public static ResourceValue<Brush> Disabled => new("OnPrimaryContainerDisabledBrush", true);

					[ResourceKeyDefinition(typeof(Brush), "OnPrimaryContainerDisabledLowBrush")]
					public static ResourceValue<Brush> DisabledLow => new("OnPrimaryContainerDisabledLowBrush", true);
				}
			}

			public static class Secondary
			{
				[ResourceKeyDefinition(typeof(Brush), "SecondaryBrush")]
				public static ResourceValue<Brush> Default => new("SecondaryBrush", true);

				[ResourceKeyDefinition(typeof(Brush), "SecondaryHoverBrush")]
				public static ResourceValue<Brush> Hover => new("SecondaryHoverBrush", true);

				[ResourceKeyDefinition(typeof(Brush), "SecondaryFocusedBrush")]
				public static ResourceValue<Brush> Focused => new("SecondaryFocusedBrush", true);

				[ResourceKeyDefinition(typeof(Brush), "SecondaryPressedBrush")]
				public static ResourceValue<Brush> Pressed => new("SecondaryPressedBrush", true);

				[ResourceKeyDefinition(typeof(Brush), "SecondaryDraggedBrush")]
				public static ResourceValue<Brush> Dragged => new("SecondaryDraggedBrush", true);

				[ResourceKeyDefinition(typeof(Brush), "SecondarySelectedBrush")]
				public static ResourceValue<Brush> Selected => new("SecondarySelectedBrush", true);

				[ResourceKeyDefinition(typeof(Brush), "SecondaryMediumBrush")]
				public static ResourceValue<Brush> Medium => new("SecondaryMediumBrush", true);

				[ResourceKeyDefinition(typeof(Brush), "SecondaryLowBrush")]
				public static ResourceValue<Brush> Low => new("SecondaryLowBrush", true);

				[ResourceKeyDefinition(typeof(Brush), "SecondaryDisabledBrush")]
				public static ResourceValue<Brush> Disabled => new("SecondaryDisabledBrush", true);

				[ResourceKeyDefinition(typeof(Brush), "SecondaryDisabledLowBrush")]
				public static ResourceValue<Brush> DisabledLow => new("SecondaryDisabledLowBrush", true);

				public static class Container
				{
					[ResourceKeyDefinition(typeof(Brush), "SecondaryContainerBrush")]
					public static ResourceValue<Brush> Default => new("SecondaryContainerBrush", true);

					[ResourceKeyDefinition(typeof(Brush), "SecondaryContainerHoverBrush")]
					public static ResourceValue<Brush> Hover => new("SecondaryContainerHoverBrush", true);

					[ResourceKeyDefinition(typeof(Brush), "SecondaryContainerFocusedBrush")]
					public static ResourceValue<Brush> Focused => new("SecondaryContainerFocusedBrush", true);

					[ResourceKeyDefinition(typeof(Brush), "SecondaryContainerPressedBrush")]
					public static ResourceValue<Brush> Pressed => new("SecondaryContainerPressedBrush", true);

					[ResourceKeyDefinition(typeof(Brush), "SecondaryContainerDraggedBrush")]
					public static ResourceValue<Brush> Dragged => new("SecondaryContainerDraggedBrush", true);

					[ResourceKeyDefinition(typeof(Brush), "SecondaryContainerSelectedBrush")]
					public static ResourceValue<Brush> Selected => new("SecondaryContainerSelectedBrush", true);

					[ResourceKeyDefinition(typeof(Brush), "SecondaryContainerMediumBrush")]
					public static ResourceValue<Brush> Medium => new("SecondaryContainerMediumBrush", true);

					[ResourceKeyDefinition(typeof(Brush), "SecondaryContainerLowBrush")]
					public static ResourceValue<Brush> Low => new("SecondaryContainerLowBrush", true);

					[ResourceKeyDefinition(typeof(Brush), "SecondaryContainerDisabledBrush")]
					public static ResourceValue<Brush> Disabled => new("SecondaryContainerDisabledBrush", true);

					[ResourceKeyDefinition(typeof(Brush), "SecondaryContainerDisabledLowBrush")]
					public static ResourceValue<Brush> DisabledLow => new("SecondaryContainerDisabledLowBrush", true);
				}

				public static class VariantLight
				{
					[ResourceKeyDefinition(typeof(Brush), "SecondaryVariantLightBrush")]
					public static ResourceValue<Brush> Default => new("SecondaryVariantLightBrush", true);

					[ResourceKeyDefinition(typeof(Brush), "SecondaryVariantLightHoverBrush")]
					public static ResourceValue<Brush> Hover => new("SecondaryVariantLightHoverBrush", true);

					[ResourceKeyDefinition(typeof(Brush), "SecondaryVariantLightFocusedBrush")]
					public static ResourceValue<Brush> Focused => new("SecondaryVariantLightFocusedBrush", true);

					[ResourceKeyDefinition(typeof(Brush), "SecondaryVariantLightPressedBrush")]
					public static ResourceValue<Brush> Pressed => new("SecondaryVariantLightPressedBrush", true);

					[ResourceKeyDefinition(typeof(Brush), "SecondaryVariantLightDraggedBrush")]
					public static ResourceValue<Brush> Dragged => new("SecondaryVariantLightDraggedBrush", true);

					[ResourceKeyDefinition(typeof(Brush), "SecondaryVariantLightSelectedBrush")]
					public static ResourceValue<Brush> Selected => new("SecondaryVariantLightSelectedBrush", true);

					[ResourceKeyDefinition(typeof(Brush), "SecondaryVariantLightMediumBrush")]
					public static ResourceValue<Brush> Medium => new("SecondaryVariantLightMediumBrush", true);

					[ResourceKeyDefinition(typeof(Brush), "SecondaryVariantLightLowBrush")]
					public static ResourceValue<Brush> Low => new("SecondaryVariantLightLowBrush", true);

					[ResourceKeyDefinition(typeof(Brush), "SecondaryVariantLightDisabledBrush")]
					public static ResourceValue<Brush> Disabled => new("SecondaryVariantLightDisabledBrush", true);

					[ResourceKeyDefinition(typeof(Brush), "SecondaryVariantLightDisabledLowBrush")]
					public static ResourceValue<Brush> DisabledLow => new("SecondaryVariantLightDisabledLowBrush", true);
				}

				public static class VariantDark
				{
					[ResourceKeyDefinition(typeof(Brush), "SecondaryVariantDarkBrush")]
					public static ResourceValue<Brush> Default => new("SecondaryVariantDarkBrush", true);

					[ResourceKeyDefinition(typeof(Brush), "SecondaryVariantDarkHoverBrush")]
					public static ResourceValue<Brush> Hover => new("SecondaryVariantDarkHoverBrush", true);

					[ResourceKeyDefinition(typeof(Brush), "SecondaryVariantDarkFocusedBrush")]
					public static ResourceValue<Brush> Focused => new("SecondaryVariantDarkFocusedBrush", true);

					[ResourceKeyDefinition(typeof(Brush), "SecondaryVariantDarkPressedBrush")]
					public static ResourceValue<Brush> Pressed => new("SecondaryVariantDarkPressedBrush", true);

					[ResourceKeyDefinition(typeof(Brush), "SecondaryVariantDarkDraggedBrush")]
					public static ResourceValue<Brush> Dragged => new("SecondaryVariantDarkDraggedBrush", true);

					[ResourceKeyDefinition(typeof(Brush), "SecondaryVariantDarkSelectedBrush")]
					public static ResourceValue<Brush> Selected => new("SecondaryVariantDarkSelectedBrush", true);

					[ResourceKeyDefinition(typeof(Brush), "SecondaryVariantDarkMediumBrush")]
					public static ResourceValue<Brush> Medium => new("SecondaryVariantDarkMediumBrush", true);

					[ResourceKeyDefinition(typeof(Brush), "SecondaryVariantDarkLowBrush")]
					public static ResourceValue<Brush> Low => new("SecondaryVariantDarkLowBrush", true);

					[ResourceKeyDefinition(typeof(Brush), "SecondaryVariantDarkDisabledBrush")]
					public static ResourceValue<Brush> Disabled => new("SecondaryVariantDarkDisabledBrush", true);

					[ResourceKeyDefinition(typeof(Brush), "SecondaryVariantDarkDisabledLowBrush")]
					public static ResourceValue<Brush> DisabledLow => new("SecondaryVariantDarkDisabledLowBrush", true);
				}
			}

			public static class OnSecondary
			{
				[ResourceKeyDefinition(typeof(Brush), "OnSecondaryBrush")]
				public static ResourceValue<Brush> Default => new("OnSecondaryBrush", true);

				[ResourceKeyDefinition(typeof(Brush), "OnSecondaryHoverBrush")]
				public static ResourceValue<Brush> Hover => new("OnSecondaryHoverBrush", true);

				[ResourceKeyDefinition(typeof(Brush), "OnSecondaryFocusedBrush")]
				public static ResourceValue<Brush> Focused => new("OnSecondaryFocusedBrush", true);

				[ResourceKeyDefinition(typeof(Brush), "OnSecondaryPressedBrush")]
				public static ResourceValue<Brush> Pressed => new("OnSecondaryPressedBrush", true);

				[ResourceKeyDefinition(typeof(Brush), "OnSecondaryDraggedBrush")]
				public static ResourceValue<Brush> Dragged => new("OnSecondaryDraggedBrush", true);

				[ResourceKeyDefinition(typeof(Brush), "OnSecondarySelectedBrush")]
				public static ResourceValue<Brush> Selected => new("OnSecondarySelectedBrush", true);

				[ResourceKeyDefinition(typeof(Brush), "OnSecondaryMediumBrush")]
				public static ResourceValue<Brush> Medium => new("OnSecondaryMediumBrush", true);

				[ResourceKeyDefinition(typeof(Brush), "OnSecondaryLowBrush")]
				public static ResourceValue<Brush> Low => new("OnSecondaryLowBrush", true);

				[ResourceKeyDefinition(typeof(Brush), "OnSecondaryDisabledBrush")]
				public static ResourceValue<Brush> Disabled => new("OnSecondaryDisabledBrush", true);

				[ResourceKeyDefinition(typeof(Brush), "OnSecondaryDisabledLowBrush")]
				public static ResourceValue<Brush> DisabledLow => new("OnSecondaryDisabledLowBrush", true);

				public static class Container
				{
					[ResourceKeyDefinition(typeof(Brush), "OnSecondaryContainerBrush")]
					public static ResourceValue<Brush> Default => new("OnSecondaryContainerBrush", true);

					[ResourceKeyDefinition(typeof(Brush), "OnSecondaryContainerHoverBrush")]
					public static ResourceValue<Brush> Hover => new("OnSecondaryContainerHoverBrush", true);

					[ResourceKeyDefinition(typeof(Brush), "OnSecondaryContainerFocusedBrush")]
					public static ResourceValue<Brush> Focused => new("OnSecondaryContainerFocusedBrush", true);

					[ResourceKeyDefinition(typeof(Brush), "OnSecondaryContainerPressedBrush")]
					public static ResourceValue<Brush> Pressed => new("OnSecondaryContainerPressedBrush", true);

					public static ResourceValue<Brush> Dragged => new("OnSecondaryContainerDraggedBrush", true);

					[ResourceKeyDefinition(typeof(Brush), "OnSecondaryContainerSelectedBrush")]
					public static ResourceValue<Brush> Selected => new("OnSecondaryContainerSelectedBrush", true);

					[ResourceKeyDefinition(typeof(Brush), "OnSecondaryContainerMediumBrush")]
					public static ResourceValue<Brush> Medium => new("OnSecondaryContainerMediumBrush", true);

					[ResourceKeyDefinition(typeof(Brush), "OnSecondaryContainerLowBrush")]
					public static ResourceValue<Brush> Low => new("OnSecondaryContainerLowBrush", true);

					[ResourceKeyDefinition(typeof(Brush), "OnSecondaryContainerDisabledBrush")]
					public static ResourceValue<Brush> Disabled => new("OnSecondaryContainerDisabledBrush", true);

					[ResourceKeyDefinition(typeof(Brush), "OnSecondaryContainerDisabledLowBrush")]
					public static ResourceValue<Brush> DisabledLow => new("OnSecondaryContainerDisabledLowBrush", true);
				}
			}

			public static class Tertiary
			{
				[ResourceKeyDefinition(typeof(Brush), "TertiaryBrush")]
				public static ResourceValue<Brush> Default => new("TertiaryBrush", true);

				[ResourceKeyDefinition(typeof(Brush), "TertiaryHoverBrush")]
				public static ResourceValue<Brush> Hover => new("TertiaryHoverBrush", true);

				[ResourceKeyDefinition(typeof(Brush), "TertiaryFocusedBrush")]
				public static ResourceValue<Brush> Focused => new("TertiaryFocusedBrush", true);

				[ResourceKeyDefinition(typeof(Brush), "TertiaryPressedBrush")]
				public static ResourceValue<Brush> Pressed => new("TertiaryPressedBrush", true);

				[ResourceKeyDefinition(typeof(Brush), "TertiaryDraggedBrush")]
				public static ResourceValue<Brush> Dragged => new("TertiaryDraggedBrush", true);

				[ResourceKeyDefinition(typeof(Brush), "TertiarySelectedBrush")]
				public static ResourceValue<Brush> Selected => new("TertiarySelectedBrush", true);

				[ResourceKeyDefinition(typeof(Brush), "TertiaryMediumBrush")]
				public static ResourceValue<Brush> Medium => new("TertiaryMediumBrush", true);

				[ResourceKeyDefinition(typeof(Brush), "TertiaryLowBrush")]
				public static ResourceValue<Brush> Low => new("TertiaryLowBrush", true);

				[ResourceKeyDefinition(typeof(Brush), "TertiaryDisabledBrush")]
				public static ResourceValue<Brush> Disabled => new("TertiaryDisabledBrush", true);

				[ResourceKeyDefinition(typeof(Brush), "TertiaryDisabledLowBrush")]
				public static ResourceValue<Brush> DisabledLow => new("TertiaryDisabledLowBrush", true);

				public static class Container
				{
					[ResourceKeyDefinition(typeof(Brush), "TertiaryContainerBrush")]
					public static ResourceValue<Brush> Default => new("TertiaryContainerBrush", true);

					[ResourceKeyDefinition(typeof(Brush), "TertiaryContainerHoverBrush")]
					public static ResourceValue<Brush> Hover => new("TertiaryContainerHoverBrush", true);

					[ResourceKeyDefinition(typeof(Brush), "TertiaryContainerFocusedBrush")]
					public static ResourceValue<Brush> Focused => new("TertiaryContainerFocusedBrush", true);

					[ResourceKeyDefinition(typeof(Brush), "TertiaryContainerPressedBrush")]
					public static ResourceValue<Brush> Pressed => new("TertiaryContainerPressedBrush", true);

					[ResourceKeyDefinition(typeof(Brush), "TertiaryContainerDraggedBrush")]
					public static ResourceValue<Brush> Dragged => new("TertiaryContainerDraggedBrush", true);

					[ResourceKeyDefinition(typeof(Brush), "TertiaryContainerSelectedBrush")]
					public static ResourceValue<Brush> Selected => new("TertiaryContainerSelectedBrush", true);

					[ResourceKeyDefinition(typeof(Brush), "TertiaryContainerMediumBrush")]
					public static ResourceValue<Brush> Medium => new("TertiaryContainerMediumBrush", true);

					[ResourceKeyDefinition(typeof(Brush), "TertiaryContainerLowBrush")]
					public static ResourceValue<Brush> Low => new("TertiaryContainerLowBrush", true);

					[ResourceKeyDefinition(typeof(Brush), "TertiaryContainerDisabledBrush")]
					public static ResourceValue<Brush> Disabled => new("TertiaryContainerDisabledBrush", true);

					[ResourceKeyDefinition(typeof(Brush), "TertiaryContainerDisabledLowBrush")]
					public static ResourceValue<Brush> DisabledLow => new("TertiaryContainerDisabledLowBrush", true);
				}
			}

			public static class OnTertiary
			{
				[ResourceKeyDefinition(typeof(Brush), "OnTertiaryBrush")]
				public static ResourceValue<Brush> Default => new("OnTertiaryBrush", true);

				[ResourceKeyDefinition(typeof(Brush), "OnTertiaryHoverBrush")]
				public static ResourceValue<Brush> Hover => new("OnTertiaryHoverBrush", true);

				[ResourceKeyDefinition(typeof(Brush), "OnTertiaryFocusedBrush")]
				public static ResourceValue<Brush> Focused => new("OnTertiaryFocusedBrush", true);

				[ResourceKeyDefinition(typeof(Brush), "OnTertiaryPressedBrush")]
				public static ResourceValue<Brush> Pressed => new("OnTertiaryPressedBrush", true);

				[ResourceKeyDefinition(typeof(Brush), "OnTertiaryDraggedBrush")]
				public static ResourceValue<Brush> Dragged => new("OnTertiaryDraggedBrush", true);

				[ResourceKeyDefinition(typeof(Brush), "OnTertiarySelectedBrush")]
				public static ResourceValue<Brush> Selected => new("OnTertiarySelectedBrush", true);

				[ResourceKeyDefinition(typeof(Brush), "OnTertiaryMediumBrush")]
				public static ResourceValue<Brush> Medium => new("OnTertiaryMediumBrush", true);

				[ResourceKeyDefinition(typeof(Brush), "OnTertiaryLowBrush")]
				public static ResourceValue<Brush> Low => new("OnTertiaryLowBrush", true);

				[ResourceKeyDefinition(typeof(Brush), "OnTertiaryDisabledBrush")]
				public static ResourceValue<Brush> Disabled => new("OnTertiaryDisabledBrush", true);

				[ResourceKeyDefinition(typeof(Brush), "OnTertiaryDisabledLowBrush")]
				public static ResourceValue<Brush> DisabledLow => new("OnTertiaryDisabledLowBrush", true);

				public static class Container
				{
					[ResourceKeyDefinition(typeof(Brush), "OnTertiaryContainerBrush")]
					public static ResourceValue<Brush> Default => new("OnTertiaryContainerBrush", true);

					[ResourceKeyDefinition(typeof(Brush), "OnTertiaryContainerHoverBrush")]
					public static ResourceValue<Brush> Hover => new("OnTertiaryContainerHoverBrush", true);

					[ResourceKeyDefinition(typeof(Brush), "OnTertiaryContainerFocusedBrush")]
					public static ResourceValue<Brush> Focused => new("OnTertiaryContainerFocusedBrush", true);

					[ResourceKeyDefinition(typeof(Brush), "OnTertiaryContainerPressedBrush")]
					public static ResourceValue<Brush> Pressed => new("OnTertiaryContainerPressedBrush", true);

					[ResourceKeyDefinition(typeof(Brush), "OnTertiaryContainerDraggedBrush")]
					public static ResourceValue<Brush> Dragged => new("OnTertiaryContainerDraggedBrush", true);

					[ResourceKeyDefinition(typeof(Brush), "OnTertiaryContainerSelectedBrush")]
					public static ResourceValue<Brush> Selected => new("OnTertiaryContainerSelectedBrush", true);

					[ResourceKeyDefinition(typeof(Brush), "OnTertiaryContainerMediumBrush")]
					public static ResourceValue<Brush> Medium => new("OnTertiaryContainerMediumBrush", true);

					[ResourceKeyDefinition(typeof(Brush), "OnTertiaryContainerLowBrush")]
					public static ResourceValue<Brush> Low => new("OnTertiaryContainerLowBrush", true);

					[ResourceKeyDefinition(typeof(Brush), "OnTertiaryContainerDisabledBrush")]
					public static ResourceValue<Brush> Disabled => new("OnTertiaryContainerDisabledBrush", true);

					[ResourceKeyDefinition(typeof(Brush), "OnTertiaryContainerDisabledLowBrush")]
					public static ResourceValue<Brush> DisabledLow => new("OnTertiaryContainerDisabledLowBrush", true);
				}
			}

			public static class Error
			{
				[ResourceKeyDefinition(typeof(Brush), "ErrorBrush")]
				public static ResourceValue<Brush> Default => new("ErrorBrush", true);

				[ResourceKeyDefinition(typeof(Brush), "ErrorHoverBrush")]
				public static ResourceValue<Brush> Hover => new("ErrorHoverBrush", true);

				[ResourceKeyDefinition(typeof(Brush), "ErrorFocusedBrush")]
				public static ResourceValue<Brush> Focused => new("ErrorFocusedBrush", true);

				[ResourceKeyDefinition(typeof(Brush), "ErrorPressedBrush")]
				public static ResourceValue<Brush> Pressed => new("ErrorPressedBrush", true);

				[ResourceKeyDefinition(typeof(Brush), "ErrorDraggedBrush")]
				public static ResourceValue<Brush> Dragged => new("ErrorDraggedBrush", true);

				[ResourceKeyDefinition(typeof(Brush), "ErrorSelectedBrush")]
				public static ResourceValue<Brush> Selected => new("ErrorSelectedBrush", true);

				[ResourceKeyDefinition(typeof(Brush), "ErrorMediumBrush")]
				public static ResourceValue<Brush> Medium => new("ErrorMediumBrush", true);

				[ResourceKeyDefinition(typeof(Brush), "ErrorLowBrush")]
				public static ResourceValue<Brush> Low => new("ErrorLowBrush", true);

				[ResourceKeyDefinition(typeof(Brush), "ErrorDisabledBrush")]
				public static ResourceValue<Brush> Disabled => new("ErrorDisabledBrush", true);

				[ResourceKeyDefinition(typeof(Brush), "ErrorDisabledLowBrush")]
				public static ResourceValue<Brush> DisabledLow => new("ErrorDisabledLowBrush", true);

				public static class Container
				{
					[ResourceKeyDefinition(typeof(Brush), "ErrorContainerBrush")]
					public static ResourceValue<Brush> Default => new("ErrorContainerBrush", true);

					[ResourceKeyDefinition(typeof(Brush), "ErrorContainerHoverBrush")]
					public static ResourceValue<Brush> Hover => new("ErrorContainerHoverBrush", true);

					[ResourceKeyDefinition(typeof(Brush), "ErrorContainerFocusedBrush")]
					public static ResourceValue<Brush> Focused => new("ErrorContainerFocusedBrush", true);

					[ResourceKeyDefinition(typeof(Brush), "ErrorContainerPressedBrush")]
					public static ResourceValue<Brush> Pressed => new("ErrorContainerPressedBrush", true);

					[ResourceKeyDefinition(typeof(Brush), "ErrorContainerDraggedBrush")]
					public static ResourceValue<Brush> Dragged => new("ErrorContainerDraggedBrush", true);

					[ResourceKeyDefinition(typeof(Brush), "ErrorContainerSelectedBrush")]
					public static ResourceValue<Brush> Selected => new("ErrorContainerSelectedBrush", true);

					[ResourceKeyDefinition(typeof(Brush), "ErrorContainerMediumBrush")]
					public static ResourceValue<Brush> Medium => new("ErrorContainerMediumBrush", true);

					[ResourceKeyDefinition(typeof(Brush), "ErrorContainerLowBrush")]
					public static ResourceValue<Brush> Low => new("ErrorContainerLowBrush", true);

					[ResourceKeyDefinition(typeof(Brush), "ErrorContainerDisabledBrush")]
					public static ResourceValue<Brush> Disabled => new("ErrorContainerDisabledBrush", true);

					[ResourceKeyDefinition(typeof(Brush), "ErrorContainerDisabledLowBrush")]
					public static ResourceValue<Brush> DisabledLow => new("ErrorContainerDisabledLowBrush", true);
				}
			}

			public static class OnError
			{
				[ResourceKeyDefinition(typeof(Brush), "OnErrorBrush")]
				public static ResourceValue<Brush> Default => new("OnErrorBrush", true);

				[ResourceKeyDefinition(typeof(Brush), "OnErrorHoverBrush")]
				public static ResourceValue<Brush> Hover => new("OnErrorHoverBrush", true);

				[ResourceKeyDefinition(typeof(Brush), "OnErrorFocusedBrush")]
				public static ResourceValue<Brush> Focused => new("OnErrorFocusedBrush", true);

				[ResourceKeyDefinition(typeof(Brush), "OnErrorPressedBrush")]
				public static ResourceValue<Brush> Pressed => new("OnErrorPressedBrush", true);

				[ResourceKeyDefinition(typeof(Brush), "OnErrorDraggedBrush")]
				public static ResourceValue<Brush> Dragged => new("OnErrorDraggedBrush", true);

				[ResourceKeyDefinition(typeof(Brush), "OnErrorSelectedBrush")]
				public static ResourceValue<Brush> Selected => new("OnErrorSelectedBrush", true);

				[ResourceKeyDefinition(typeof(Brush), "OnErrorMediumBrush")]
				public static ResourceValue<Brush> Medium => new("OnErrorMediumBrush", true);

				[ResourceKeyDefinition(typeof(Brush), "OnErrorLowBrush")]
				public static ResourceValue<Brush> Low => new("OnErrorLowBrush", true);

				[ResourceKeyDefinition(typeof(Brush), "OnErrorDisabledBrush")]
				public static ResourceValue<Brush> Disabled => new("OnErrorDisabledBrush", true);

				[ResourceKeyDefinition(typeof(Brush), "OnErrorDisabledLowBrush")]
				public static ResourceValue<Brush> DisabledLow => new("OnErrorDisabledLowBrush", true);

				public static class Container
				{
					[ResourceKeyDefinition(typeof(Brush), "OnErrorContainerBrush")]
					public static ResourceValue<Brush> Default => new("OnErrorContainerBrush", true);

					[ResourceKeyDefinition(typeof(Brush), "OnErrorContainerHoverBrush")]
					public static ResourceValue<Brush> Hover => new("OnErrorContainerHoverBrush", true);

					[ResourceKeyDefinition(typeof(Brush), "OnErrorContainerFocusedBrush")]
					public static ResourceValue<Brush> Focused => new("OnErrorContainerFocusedBrush", true);

					[ResourceKeyDefinition(typeof(Brush), "OnErrorContainerPressedBrush")]
					public static ResourceValue<Brush> Pressed => new("OnErrorContainerPressedBrush", true);

					[ResourceKeyDefinition(typeof(Brush), "OnErrorContainerDraggedBrush")]
					public static ResourceValue<Brush> Dragged => new("OnErrorContainerDraggedBrush", true);

					[ResourceKeyDefinition(typeof(Brush), "OnErrorContainerSelectedBrush")]
					public static ResourceValue<Brush> Selected => new("OnErrorContainerSelectedBrush", true);

					[ResourceKeyDefinition(typeof(Brush), "OnErrorContainerMediumBrush")]
					public static ResourceValue<Brush> Medium => new("OnErrorContainerMediumBrush", true);

					[ResourceKeyDefinition(typeof(Brush), "OnErrorContainerLowBrush")]
					public static ResourceValue<Brush> Low => new("OnErrorContainerLowBrush", true);

					[ResourceKeyDefinition(typeof(Brush), "OnErrorContainerDisabledBrush")]
					public static ResourceValue<Brush> Disabled => new("OnErrorContainerDisabledBrush", true);

					[ResourceKeyDefinition(typeof(Brush), "OnErrorContainerDisabledLowBrush")]
					public static ResourceValue<Brush> DisabledLow => new("OnErrorContainerDisabledLowBrush", true);
				}
			}

			public static class Background
			{
				[ResourceKeyDefinition(typeof(Brush), "BackgroundBrush")]
				public static ResourceValue<Brush> Default => new("BackgroundBrush", true);

				[ResourceKeyDefinition(typeof(Brush), "BackgroundHoverBrush")]
				public static ResourceValue<Brush> Hover => new("BackgroundHoverBrush", true);

				[ResourceKeyDefinition(typeof(Brush), "BackgroundFocusedBrush")]
				public static ResourceValue<Brush> Focused => new("BackgroundFocusedBrush", true);

				[ResourceKeyDefinition(typeof(Brush), "BackgroundPressedBrush")]
				public static ResourceValue<Brush> Pressed => new("BackgroundPressedBrush", true);

				[ResourceKeyDefinition(typeof(Brush), "BackgroundDraggedBrush")]
				public static ResourceValue<Brush> Dragged => new("BackgroundDraggedBrush", true);

				[ResourceKeyDefinition(typeof(Brush), "BackgroundSelectedBrush")]
				public static ResourceValue<Brush> Selected => new("BackgroundSelectedBrush", true);

				[ResourceKeyDefinition(typeof(Brush), "BackgroundMediumBrush")]
				public static ResourceValue<Brush> Medium => new("BackgroundMediumBrush", true);

				[ResourceKeyDefinition(typeof(Brush), "BackgroundLowBrush")]
				public static ResourceValue<Brush> Low => new("BackgroundLowBrush", true);

				[ResourceKeyDefinition(typeof(Brush), "BackgroundDisabledBrush")]
				public static ResourceValue<Brush> Disabled => new("BackgroundDisabledBrush", true);

				[ResourceKeyDefinition(typeof(Brush), "BackgroundDisabledLowBrush")]
				public static ResourceValue<Brush> DisabledLow => new("BackgroundDisabledLowBrush", true);
			}

			public static class OnBackground
			{
				[ResourceKeyDefinition(typeof(Brush), "OnBackgroundBrush")]
				public static ResourceValue<Brush> Default => new("OnBackgroundBrush", true);

				[ResourceKeyDefinition(typeof(Brush), "OnBackgroundHoverBrush")]
				public static ResourceValue<Brush> Hover => new("OnBackgroundHoverBrush", true);

				[ResourceKeyDefinition(typeof(Brush), "OnBackgroundFocusedBrush")]
				public static ResourceValue<Brush> Focused => new("OnBackgroundFocusedBrush", true);

				[ResourceKeyDefinition(typeof(Brush), "OnBackgroundPressedBrush")]
				public static ResourceValue<Brush> Pressed => new("OnBackgroundPressedBrush", true);

				[ResourceKeyDefinition(typeof(Brush), "OnBackgroundDraggedBrush")]
				public static ResourceValue<Brush> Dragged => new("OnBackgroundDraggedBrush", true);

				[ResourceKeyDefinition(typeof(Brush), "OnBackgroundSelectedBrush")]
				public static ResourceValue<Brush> Selected => new("OnBackgroundSelectedBrush", true);

				[ResourceKeyDefinition(typeof(Brush), "OnBackgroundMediumBrush")]
				public static ResourceValue<Brush> Medium => new("OnBackgroundMediumBrush", true);

				[ResourceKeyDefinition(typeof(Brush), "OnBackgroundLowBrush")]
				public static ResourceValue<Brush> Low => new("OnBackgroundLowBrush", true);

				[ResourceKeyDefinition(typeof(Brush), "OnBackgroundDisabledBrush")]
				public static ResourceValue<Brush> Disabled => new("OnBackgroundDisabledBrush", true);

				[ResourceKeyDefinition(typeof(Brush), "OnBackgroundDisabledLowBrush")]
				public static ResourceValue<Brush> DisabledLow => new("OnBackgroundDisabledLowBrush", true);
			}

			public static class Surface
			{
				[ResourceKeyDefinition(typeof(Brush), "SurfaceBrush")]
				public static ResourceValue<Brush> Default => new("SurfaceBrush", true);

				[ResourceKeyDefinition(typeof(Brush), "SurfaceHoverBrush")]
				public static ResourceValue<Brush> Hover => new("SurfaceHoverBrush", true);

				[ResourceKeyDefinition(typeof(Brush), "SurfaceFocusedBrush")]
				public static ResourceValue<Brush> Focused => new("SurfaceFocusedBrush", true);

				[ResourceKeyDefinition(typeof(Brush), "SurfacePressedBrush")]
				public static ResourceValue<Brush> Pressed => new("SurfacePressedBrush", true);

				[ResourceKeyDefinition(typeof(Brush), "SurfaceDraggedBrush")]
				public static ResourceValue<Brush> Dragged => new("SurfaceDraggedBrush", true);

				[ResourceKeyDefinition(typeof(Brush), "SurfaceSelectedBrush")]
				public static ResourceValue<Brush> Selected => new("SurfaceSelectedBrush", true);

				[ResourceKeyDefinition(typeof(Brush), "SurfaceMediumBrush")]
				public static ResourceValue<Brush> Medium => new("SurfaceMediumBrush", true);

				[ResourceKeyDefinition(typeof(Brush), "SurfaceLowBrush")]
				public static ResourceValue<Brush> Low => new("SurfaceLowBrush", true);

				[ResourceKeyDefinition(typeof(Brush), "SurfaceDisabledBrush")]
				public static ResourceValue<Brush> Disabled => new("SurfaceDisabledBrush", true);

				[ResourceKeyDefinition(typeof(Brush), "SurfaceDisabledLowBrush")]
				public static ResourceValue<Brush> DisabledLow => new("SurfaceDisabledLowBrush", true);

				public static class Variant
				{
					[ResourceKeyDefinition(typeof(Brush), "SurfaceVariantBrush")]
					public static ResourceValue<Brush> Default => new("SurfaceVariantBrush", true);

					[ResourceKeyDefinition(typeof(Brush), "SurfaceVariantHoverBrush")]
					public static ResourceValue<Brush> Hover => new("SurfaceVariantHoverBrush", true);

					[ResourceKeyDefinition(typeof(Brush), "SurfaceVariantFocusedBrush")]
					public static ResourceValue<Brush> Focused => new("SurfaceVariantFocusedBrush", true);

					[ResourceKeyDefinition(typeof(Brush), "SurfaceVariantPressedBrush")]
					public static ResourceValue<Brush> Pressed => new("SurfaceVariantPressedBrush", true);

					[ResourceKeyDefinition(typeof(Brush), "SurfaceVariantDraggedBrush")]
					public static ResourceValue<Brush> Dragged => new("SurfaceVariantDraggedBrush", true);

					[ResourceKeyDefinition(typeof(Brush), "SurfaceVariantSelectedBrush")]
					public static ResourceValue<Brush> Selected => new("SurfaceVariantSelectedBrush", true);

					[ResourceKeyDefinition(typeof(Brush), "SurfaceVariantMediumBrush")]
					public static ResourceValue<Brush> Medium => new("SurfaceVariantMediumBrush", true);

					[ResourceKeyDefinition(typeof(Brush), "SurfaceVariantLowBrush")]
					public static ResourceValue<Brush> Low => new("SurfaceVariantLowBrush", true);

					[ResourceKeyDefinition(typeof(Brush), "SurfaceVariantDisabledBrush")]
					public static ResourceValue<Brush> Disabled => new("SurfaceVariantDisabledBrush", true);

					[ResourceKeyDefinition(typeof(Brush), "SurfaceVariantDisabledLowBrush")]
					public static ResourceValue<Brush> DisabledLow => new("SurfaceVariantDisabledLowBrush", true);
				}

				public static class Inverse
				{
					[ResourceKeyDefinition(typeof(Brush), "SurfaceInverseBrush")]
					public static ResourceValue<Brush> Default => new("SurfaceInverseBrush", true);

					[ResourceKeyDefinition(typeof(Brush), "SurfaceInverseHoverBrush")]
					public static ResourceValue<Brush> Hover => new("SurfaceInverseHoverBrush", true);

					[ResourceKeyDefinition(typeof(Brush), "SurfaceInverseFocusedBrush")]
					public static ResourceValue<Brush> Focused => new("SurfaceInverseFocusedBrush", true);

					[ResourceKeyDefinition(typeof(Brush), "SurfaceInversePressedBrush")]
					public static ResourceValue<Brush> Pressed => new("SurfaceInversePressedBrush", true);

					[ResourceKeyDefinition(typeof(Brush), "SurfaceInverseDraggedBrush")]
					public static ResourceValue<Brush> Dragged => new("SurfaceInverseDraggedBrush", true);

					[ResourceKeyDefinition(typeof(Brush), "SurfaceInverseSelectedBrush")]
					public static ResourceValue<Brush> Selected => new("SurfaceInverseSelectedBrush", true);

					[ResourceKeyDefinition(typeof(Brush), "SurfaceInverseMediumBrush")]
					public static ResourceValue<Brush> Medium => new("SurfaceInverseMediumBrush", true);

					[ResourceKeyDefinition(typeof(Brush), "SurfaceInverseLowBrush")]
					public static ResourceValue<Brush> Low => new("SurfaceInverseLowBrush", true);

					[ResourceKeyDefinition(typeof(Brush), "SurfaceInverseDisabledBrush")]
					public static ResourceValue<Brush> Disabled => new("SurfaceInverseDisabledBrush", true);

					[ResourceKeyDefinition(typeof(Brush), "SurfaceInverseDisabledLowBrush")]
					public static ResourceValue<Brush> DisabledLow => new("SurfaceInverseDisabledLowBrush", true);
				}
			}

			public static class OnSurface
			{
				[ResourceKeyDefinition(typeof(Brush), "OnSurfaceBrush")]
				public static ResourceValue<Brush> Default => new("OnSurfaceBrush", true);

				[ResourceKeyDefinition(typeof(Brush), "OnSurfaceHoverBrush")]
				public static ResourceValue<Brush> Hover => new("OnSurfaceHoverBrush", true);

				[ResourceKeyDefinition(typeof(Brush), "OnSurfaceFocusedBrush")]
				public static ResourceValue<Brush> Focused => new("OnSurfaceFocusedBrush", true);

				[ResourceKeyDefinition(typeof(Brush), "OnSurfacePressedBrush")]
				public static ResourceValue<Brush> Pressed => new("OnSurfacePressedBrush", true);

				[ResourceKeyDefinition(typeof(Brush), "OnSurfaceDraggedBrush")]
				public static ResourceValue<Brush> Dragged => new("OnSurfaceDraggedBrush", true);

				[ResourceKeyDefinition(typeof(Brush), "OnSurfaceSelectedBrush")]
				public static ResourceValue<Brush> Selected => new("OnSurfaceSelectedBrush", true);

				[ResourceKeyDefinition(typeof(Brush), "OnSurfaceMediumBrush")]
				public static ResourceValue<Brush> Medium => new("OnSurfaceMediumBrush", true);

				[ResourceKeyDefinition(typeof(Brush), "OnSurfaceLowBrush")]
				public static ResourceValue<Brush> Low => new("OnSurfaceLowBrush", true);

				[ResourceKeyDefinition(typeof(Brush), "OnSurfaceDisabledBrush")]
				public static ResourceValue<Brush> Disabled => new("OnSurfaceDisabledBrush", true);

				[ResourceKeyDefinition(typeof(Brush), "OnSurfaceDisabledLowBrush")]
				public static ResourceValue<Brush> DisabledLow => new("OnSurfaceDisabledLowBrush", true);

				public static class Variant
				{
					[ResourceKeyDefinition(typeof(Brush), "OnSurfaceVariantBrush")]
					public static ResourceValue<Brush> Default => new("OnSurfaceVariantBrush", true);

					[ResourceKeyDefinition(typeof(Brush), "OnSurfaceVariantHoverBrush")]
					public static ResourceValue<Brush> Hover => new("OnSurfaceVariantHoverBrush", true);

					[ResourceKeyDefinition(typeof(Brush), "OnSurfaceVariantFocusedBrush")]
					public static ResourceValue<Brush> Focused => new("OnSurfaceVariantFocusedBrush", true);

					[ResourceKeyDefinition(typeof(Brush), "OnSurfaceVariantPressedBrush")]
					public static ResourceValue<Brush> Pressed => new("OnSurfaceVariantPressedBrush", true);

					[ResourceKeyDefinition(typeof(Brush), "OnSurfaceVariantDraggedBrush")]
					public static ResourceValue<Brush> Dragged => new("OnSurfaceVariantDraggedBrush", true);

					[ResourceKeyDefinition(typeof(Brush), "OnSurfaceVariantSelectedBrush")]
					public static ResourceValue<Brush> Selected => new("OnSurfaceVariantSelectedBrush", true);

					[ResourceKeyDefinition(typeof(Brush), "OnSurfaceVariantMediumBrush")]
					public static ResourceValue<Brush> Medium => new("OnSurfaceVariantMediumBrush", true);

					[ResourceKeyDefinition(typeof(Brush), "OnSurfaceVariantLowBrush")]
					public static ResourceValue<Brush> Low => new("OnSurfaceVariantLowBrush", true);

					[ResourceKeyDefinition(typeof(Brush), "OnSurfaceVariantDisabledBrush")]
					public static ResourceValue<Brush> Disabled => new("OnSurfaceVariantDisabledBrush", true);

					[ResourceKeyDefinition(typeof(Brush), "OnSurfaceVariantDisabledLowBrush")]
					public static ResourceValue<Brush> DisabledLow => new("OnSurfaceVariantDisabledLowBrush", true);
				}

				public static class Inverse
				{
					[ResourceKeyDefinition(typeof(Brush), "OnSurfaceInverseBrush")]
					public static ResourceValue<Brush> Default => new("OnSurfaceInverseBrush", true);

					[ResourceKeyDefinition(typeof(Brush), "OnSurfaceInverseHoverBrush")]
					public static ResourceValue<Brush> Hover => new("OnSurfaceInverseHoverBrush", true);

					[ResourceKeyDefinition(typeof(Brush), "OnSurfaceInverseFocusedBrush")]
					public static ResourceValue<Brush> Focused => new("OnSurfaceInverseFocusedBrush", true);

					[ResourceKeyDefinition(typeof(Brush), "OnSurfaceInversePressedBrush")]
					public static ResourceValue<Brush> Pressed => new("OnSurfaceInversePressedBrush", true);

					[ResourceKeyDefinition(typeof(Brush), "OnSurfaceInverseDraggedBrush")]
					public static ResourceValue<Brush> Dragged => new("OnSurfaceInverseDraggedBrush", true);

					[ResourceKeyDefinition(typeof(Brush), "OnSurfaceInverseSelectedBrush")]
					public static ResourceValue<Brush> Selected => new("OnSurfaceInverseSelectedBrush", true);

					[ResourceKeyDefinition(typeof(Brush), "OnSurfaceInverseMediumBrush")]
					public static ResourceValue<Brush> Medium => new("OnSurfaceInverseMediumBrush", true);

					[ResourceKeyDefinition(typeof(Brush), "OnSurfaceInverseLowBrush")]
					public static ResourceValue<Brush> Low => new("OnSurfaceInverseLowBrush", true);

					[ResourceKeyDefinition(typeof(Brush), "OnSurfaceInverseDisabledBrush")]
					public static ResourceValue<Brush> Disabled => new("OnSurfaceInverseDisabledBrush", true);

					[ResourceKeyDefinition(typeof(Brush), "OnSurfaceInverseDisabledLowBrush")]
					public static ResourceValue<Brush> DisabledLow => new("OnSurfaceInverseDisabledLowBrush", true);
				}
			}

			public static class Outline
			{
				[ResourceKeyDefinition(typeof(Brush), "OutlineBrush")]
				public static ResourceValue<Brush> Default => new("OutlineBrush", true);

				[ResourceKeyDefinition(typeof(Brush), "OutlineHoverBrush")]
				public static ResourceValue<Brush> Hover => new("OutlineHoverBrush", true);

				[ResourceKeyDefinition(typeof(Brush), "OutlineFocusedBrush")]
				public static ResourceValue<Brush> Focused => new("OutlineFocusedBrush", true);

				[ResourceKeyDefinition(typeof(Brush), "OutlinePressedBrush")]
				public static ResourceValue<Brush> Pressed => new("OutlinePressedBrush", true);

				[ResourceKeyDefinition(typeof(Brush), "OutlineDraggedBrush")]
				public static ResourceValue<Brush> Dragged => new("OutlineDraggedBrush", true);

				[ResourceKeyDefinition(typeof(Brush), "OutlineSelectedBrush")]
				public static ResourceValue<Brush> Selected => new("OutlineSelectedBrush", true);

				[ResourceKeyDefinition(typeof(Brush), "OutlineMediumBrush")]
				public static ResourceValue<Brush> Medium => new("OutlineMediumBrush", true);

				[ResourceKeyDefinition(typeof(Brush), "OutlineLowBrush")]
				public static ResourceValue<Brush> Low => new("OutlineLowBrush", true);

				[ResourceKeyDefinition(typeof(Brush), "OutlineDisabledBrush")]
				public static ResourceValue<Brush> Disabled => new("OutlineDisabledBrush", true);

				[ResourceKeyDefinition(typeof(Brush), "OutlineDisabledLowBrush")]
				public static ResourceValue<Brush> DisabledLow => new("OutlineDisabledLowBrush", true);

				public static class Variant
				{
					[ResourceKeyDefinition(typeof(Brush), "OutlineVariantBrush")]
					public static ResourceValue<Brush> Default => new("OutlineVariantBrush", true);

					[ResourceKeyDefinition(typeof(Brush), "OutlineVariantHoverBrush")]
					public static ResourceValue<Brush> Hover => new("OutlineVariantHoverBrush", true);

					[ResourceKeyDefinition(typeof(Brush), "OutlineVariantFocusedBrush")]
					public static ResourceValue<Brush> Focused => new("OutlineVariantFocusedBrush", true);

					[ResourceKeyDefinition(typeof(Brush), "OutlineVariantPressedBrush")]
					public static ResourceValue<Brush> Pressed => new("OutlineVariantPressedBrush", true);

					[ResourceKeyDefinition(typeof(Brush), "OutlineVariantDraggedBrush")]
					public static ResourceValue<Brush> Dragged => new("OutlineVariantDraggedBrush", true);

					[ResourceKeyDefinition(typeof(Brush), "OutlineVariantSelectedBrush")]
					public static ResourceValue<Brush> Selected => new("OutlineVariantSelectedBrush", true);

					[ResourceKeyDefinition(typeof(Brush), "OutlineVariantMediumBrush")]
					public static ResourceValue<Brush> Medium => new("OutlineVariantMediumBrush", true);

					[ResourceKeyDefinition(typeof(Brush), "OutlineVariantLowBrush")]
					public static ResourceValue<Brush> Low => new("OutlineVariantLowBrush", true);

					[ResourceKeyDefinition(typeof(Brush), "OutlineVariantDisabledBrush")]
					public static ResourceValue<Brush> Disabled => new("OutlineVariantDisabledBrush", true);

					[ResourceKeyDefinition(typeof(Brush), "OutlineVariantDisabledLowBrush")]
					public static ResourceValue<Brush> DisabledLow => new("OutlineVariantDisabledLowBrush", true);
				}
			}
		}
	}
}
