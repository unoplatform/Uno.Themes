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
				public static ThemeResourceKey<Brush> Default => new("PrimaryBrush");

				[ResourceKeyDefinition(typeof(Brush), "PrimaryHoverBrush")]
				public static ThemeResourceKey<Brush> Hover => new("PrimaryHoverBrush");

				[ResourceKeyDefinition(typeof(Brush), "PrimaryFocusedBrush")]
				public static ThemeResourceKey<Brush> Focused => new("PrimaryFocusedBrush");

				[ResourceKeyDefinition(typeof(Brush), "PrimaryPressedBrush")]
				public static ThemeResourceKey<Brush> Pressed => new("PrimaryPressedBrush");

				[ResourceKeyDefinition(typeof(Brush), "PrimaryDraggedBrush")]
				public static ThemeResourceKey<Brush> Dragged => new("PrimaryDraggedBrush");

				[ResourceKeyDefinition(typeof(Brush), "PrimarySelectedBrush")]
				public static ThemeResourceKey<Brush> Selected => new("PrimarySelectedBrush");

				[ResourceKeyDefinition(typeof(Brush), "PrimaryMediumBrush")]
				public static ThemeResourceKey<Brush> Medium => new("PrimaryMediumBrush");

				[ResourceKeyDefinition(typeof(Brush), "PrimaryLowBrush")]
				public static ThemeResourceKey<Brush> Low => new("PrimaryLowBrush");

				[ResourceKeyDefinition(typeof(Brush), "PrimaryDisabledBrush")]
				public static ThemeResourceKey<Brush> Disabled => new("PrimaryDisabledBrush");

				[ResourceKeyDefinition(typeof(Brush), "PrimaryDisabledLowBrush")]
				public static ThemeResourceKey<Brush> DisabledLow => new("PrimaryDisabledLowBrush");

				public static class Inverse
				{
					[ResourceKeyDefinition(typeof(Brush), "PrimaryInverseBrush")]
					public static ThemeResourceKey<Brush> Default => new("PrimaryInverseBrush");

					[ResourceKeyDefinition(typeof(Brush), "PrimaryInverseHoverBrush")]
					public static ThemeResourceKey<Brush> Hover => new("PrimaryInverseHoverBrush");

					[ResourceKeyDefinition(typeof(Brush), "PrimaryInverseFocusedBrush")]
					public static ThemeResourceKey<Brush> Focused => new("PrimaryInverseFocusedBrush");

					[ResourceKeyDefinition(typeof(Brush), "PrimaryInversePressedBrush")]
					public static ThemeResourceKey<Brush> Pressed => new("PrimaryInversePressedBrush");

					[ResourceKeyDefinition(typeof(Brush), "PrimaryInverseDraggedBrush")]
					public static ThemeResourceKey<Brush> Dragged => new("PrimaryInverseDraggedBrush");

					[ResourceKeyDefinition(typeof(Brush), "PrimaryInverseSelectedBrush")]
					public static ThemeResourceKey<Brush> Selected => new("PrimaryInverseSelectedBrush");

					[ResourceKeyDefinition(typeof(Brush), "PrimaryInverseMediumBrush")]
					public static ThemeResourceKey<Brush> Medium => new("PrimaryInverseMediumBrush");

					[ResourceKeyDefinition(typeof(Brush), "PrimaryInverseLowBrush")]
					public static ThemeResourceKey<Brush> Low => new("PrimaryInverseLowBrush");

					[ResourceKeyDefinition(typeof(Brush), "PrimaryInverseDisabledBrush")]
					public static ThemeResourceKey<Brush> Disabled => new("PrimaryInverseDisabledBrush");

					[ResourceKeyDefinition(typeof(Brush), "PrimaryInverseDisabledLowBrush")]
					public static ThemeResourceKey<Brush> DisabledLow => new("PrimaryInverseDisabledLowBrush");
				}

				public static class Container
				{
					[ResourceKeyDefinition(typeof(Brush), "PrimaryContainerBrush")]
					public static ThemeResourceKey<Brush> Default => new("PrimaryContainerBrush");

					[ResourceKeyDefinition(typeof(Brush), "PrimaryContainerHoverBrush")]
					public static ThemeResourceKey<Brush> Hover => new("PrimaryContainerHoverBrush");

					[ResourceKeyDefinition(typeof(Brush), "PrimaryContainerFocusedBrush")]
					public static ThemeResourceKey<Brush> Focused => new("PrimaryContainerFocusedBrush");

					[ResourceKeyDefinition(typeof(Brush), "PrimaryContainerPressedBrush")]
					public static ThemeResourceKey<Brush> Pressed => new("PrimaryContainerPressedBrush");

					[ResourceKeyDefinition(typeof(Brush), "PrimaryContainerDraggedBrush")]
					public static ThemeResourceKey<Brush> Dragged => new("PrimaryContainerDraggedBrush");

					[ResourceKeyDefinition(typeof(Brush), "PrimaryContainerSelectedBrush")]
					public static ThemeResourceKey<Brush> Selected => new("PrimaryContainerSelectedBrush");

					[ResourceKeyDefinition(typeof(Brush), "PrimaryContainerMediumBrush")]
					public static ThemeResourceKey<Brush> Medium => new("PrimaryContainerMediumBrush");

					[ResourceKeyDefinition(typeof(Brush), "PrimaryContainerLowBrush")]
					public static ThemeResourceKey<Brush> Low => new("PrimaryContainerLowBrush");

					[ResourceKeyDefinition(typeof(Brush), "PrimaryContainerDisabledBrush")]
					public static ThemeResourceKey<Brush> Disabled => new("PrimaryContainerDisabledBrush");

					[ResourceKeyDefinition(typeof(Brush), "PrimaryContainerDisabledLowBrush")]
					public static ThemeResourceKey<Brush> DisabledLow => new("PrimaryContainerDisabledLowBrush");
				}

				public static class VariantLight
				{
					[ResourceKeyDefinition(typeof(Brush), "PrimaryVariantLightBrush")]
					public static ThemeResourceKey<Brush> Default => new("PrimaryVariantLightBrush");

					[ResourceKeyDefinition(typeof(Brush), "PrimaryVariantLightHoverBrush")]
					public static ThemeResourceKey<Brush> Hover => new("PrimaryVariantLightHoverBrush");

					[ResourceKeyDefinition(typeof(Brush), "PrimaryVariantLightFocusedBrush")]
					public static ThemeResourceKey<Brush> Focused => new("PrimaryVariantLightFocusedBrush");

					[ResourceKeyDefinition(typeof(Brush), "PrimaryVariantLightPressedBrush")]
					public static ThemeResourceKey<Brush> Pressed => new("PrimaryVariantLightPressedBrush");

					[ResourceKeyDefinition(typeof(Brush), "PrimaryVariantLightDraggedBrush")]
					public static ThemeResourceKey<Brush> Dragged => new("PrimaryVariantLightDraggedBrush");

					[ResourceKeyDefinition(typeof(Brush), "PrimaryVariantLightSelectedBrush")]
					public static ThemeResourceKey<Brush> Selected => new("PrimaryVariantLightSelectedBrush");

					[ResourceKeyDefinition(typeof(Brush), "PrimaryVariantLightMediumBrush")]
					public static ThemeResourceKey<Brush> Medium => new("PrimaryVariantLightMediumBrush");

					[ResourceKeyDefinition(typeof(Brush), "PrimaryVariantLightLowBrush")]
					public static ThemeResourceKey<Brush> Low => new("PrimaryVariantLightLowBrush");

					[ResourceKeyDefinition(typeof(Brush), "PrimaryVariantLightDisabledBrush")]
					public static ThemeResourceKey<Brush> Disabled => new("PrimaryVariantLightDisabledBrush");

					[ResourceKeyDefinition(typeof(Brush), "PrimaryVariantLightDisabledLowBrush")]
					public static ThemeResourceKey<Brush> DisabledLow => new("PrimaryVariantLightDisabledLowBrush");
				}

				public static class VariantDark
				{
					[ResourceKeyDefinition(typeof(Brush), "PrimaryVariantDarkBrush")]
					public static ThemeResourceKey<Brush> Default => new("PrimaryVariantDarkBrush");

					[ResourceKeyDefinition(typeof(Brush), "PrimaryVariantDarkHoverBrush")]
					public static ThemeResourceKey<Brush> Hover => new("PrimaryVariantDarkHoverBrush");

					[ResourceKeyDefinition(typeof(Brush), "PrimaryVariantDarkFocusedBrush")]
					public static ThemeResourceKey<Brush> Focused => new("PrimaryVariantDarkFocusedBrush");

					[ResourceKeyDefinition(typeof(Brush), "PrimaryVariantDarkPressedBrush")]
					public static ThemeResourceKey<Brush> Pressed => new("PrimaryVariantDarkPressedBrush");

					[ResourceKeyDefinition(typeof(Brush), "PrimaryVariantDarkDraggedBrush")]
					public static ThemeResourceKey<Brush> Dragged => new("PrimaryVariantDarkDraggedBrush");

					[ResourceKeyDefinition(typeof(Brush), "PrimaryVariantDarkSelectedBrush")]
					public static ThemeResourceKey<Brush> Selected => new("PrimaryVariantDarkSelectedBrush");

					[ResourceKeyDefinition(typeof(Brush), "PrimaryVariantDarkMediumBrush")]
					public static ThemeResourceKey<Brush> Medium => new("PrimaryVariantDarkMediumBrush");

					[ResourceKeyDefinition(typeof(Brush), "PrimaryVariantDarkLowBrush")]
					public static ThemeResourceKey<Brush> Low => new("PrimaryVariantDarkLowBrush");

					[ResourceKeyDefinition(typeof(Brush), "PrimaryVariantDarkDisabledBrush")]
					public static ThemeResourceKey<Brush> Disabled => new("PrimaryVariantDarkDisabledBrush");

					[ResourceKeyDefinition(typeof(Brush), "PrimaryVariantDarkDisabledLowBrush")]
					public static ThemeResourceKey<Brush> DisabledLow => new("PrimaryVariantDarkDisabledLowBrush");
				}
			}

			public static class OnPrimary
			{
				[ResourceKeyDefinition(typeof(Brush), "OnPrimaryBrush")]
				public static ThemeResourceKey<Brush> Default => new("OnPrimaryBrush");

				[ResourceKeyDefinition(typeof(Brush), "OnPrimaryHoverBrush")]
				public static ThemeResourceKey<Brush> Hover => new("OnPrimaryHoverBrush");

				[ResourceKeyDefinition(typeof(Brush), "OnPrimaryFocusedBrush")]
				public static ThemeResourceKey<Brush> Focused => new("OnPrimaryFocusedBrush");

				[ResourceKeyDefinition(typeof(Brush), "OnPrimaryPressedBrush")]
				public static ThemeResourceKey<Brush> Pressed => new("OnPrimaryPressedBrush");

				[ResourceKeyDefinition(typeof(Brush), "OnPrimaryDraggedBrush")]
				public static ThemeResourceKey<Brush> Dragged => new("OnPrimaryDraggedBrush");

				[ResourceKeyDefinition(typeof(Brush), "OnPrimarySelectedBrush")]
				public static ThemeResourceKey<Brush> Selected => new("OnPrimarySelectedBrush");

				[ResourceKeyDefinition(typeof(Brush), "OnPrimaryMediumBrush")]
				public static ThemeResourceKey<Brush> Medium => new("OnPrimaryMediumBrush");

				[ResourceKeyDefinition(typeof(Brush), "OnPrimaryLowBrush")]
				public static ThemeResourceKey<Brush> Low => new("OnPrimaryLowBrush");

				[ResourceKeyDefinition(typeof(Brush), "OnPrimaryDisabledBrush")]
				public static ThemeResourceKey<Brush> Disabled => new("OnPrimaryDisabledBrush");

				[ResourceKeyDefinition(typeof(Brush), "OnPrimaryDisabledLowBrush")]
				public static ThemeResourceKey<Brush> DisabledLow => new("OnPrimaryDisabledLowBrush");

				public static class Container
				{
					[ResourceKeyDefinition(typeof(Brush), "OnPrimaryContainerBrush")]
					public static ThemeResourceKey<Brush> Default => new("OnPrimaryContainerBrush");

					[ResourceKeyDefinition(typeof(Brush), "OnPrimaryContainerHoverBrush")]
					public static ThemeResourceKey<Brush> Hover => new("OnPrimaryContainerHoverBrush");

					[ResourceKeyDefinition(typeof(Brush), "OnPrimaryContainerFocusedBrush")]
					public static ThemeResourceKey<Brush> Focused => new("OnPrimaryContainerFocusedBrush");

					[ResourceKeyDefinition(typeof(Brush), "OnPrimaryContainerPressedBrush")]
					public static ThemeResourceKey<Brush> Pressed => new("OnPrimaryContainerPressedBrush");

					[ResourceKeyDefinition(typeof(Brush), "OnPrimaryContainerDraggedBrush")]
					public static ThemeResourceKey<Brush> Dragged => new("OnPrimaryContainerDraggedBrush");

					[ResourceKeyDefinition(typeof(Brush), "OnPrimaryContainerSelectedBrush")]
					public static ThemeResourceKey<Brush> Selected => new("OnPrimaryContainerSelectedBrush");

					[ResourceKeyDefinition(typeof(Brush), "OnPrimaryContainerMediumBrush")]
					public static ThemeResourceKey<Brush> Medium => new("OnPrimaryContainerMediumBrush");

					[ResourceKeyDefinition(typeof(Brush), "OnPrimaryContainerLowBrush")]
					public static ThemeResourceKey<Brush> Low => new("OnPrimaryContainerLowBrush");

					[ResourceKeyDefinition(typeof(Brush), "OnPrimaryContainerDisabledBrush")]
					public static ThemeResourceKey<Brush> Disabled => new("OnPrimaryContainerDisabledBrush");

					[ResourceKeyDefinition(typeof(Brush), "OnPrimaryContainerDisabledLowBrush")]
					public static ThemeResourceKey<Brush> DisabledLow => new("OnPrimaryContainerDisabledLowBrush");
				}
			}

			public static class Secondary
			{
				[ResourceKeyDefinition(typeof(Brush), "SecondaryBrush")]
				public static ThemeResourceKey<Brush> Default => new("SecondaryBrush");

				[ResourceKeyDefinition(typeof(Brush), "SecondaryHoverBrush")]
				public static ThemeResourceKey<Brush> Hover => new("SecondaryHoverBrush");

				[ResourceKeyDefinition(typeof(Brush), "SecondaryFocusedBrush")]
				public static ThemeResourceKey<Brush> Focused => new("SecondaryFocusedBrush");

				[ResourceKeyDefinition(typeof(Brush), "SecondaryPressedBrush")]
				public static ThemeResourceKey<Brush> Pressed => new("SecondaryPressedBrush");

				[ResourceKeyDefinition(typeof(Brush), "SecondaryDraggedBrush")]
				public static ThemeResourceKey<Brush> Dragged => new("SecondaryDraggedBrush");

				[ResourceKeyDefinition(typeof(Brush), "SecondarySelectedBrush")]
				public static ThemeResourceKey<Brush> Selected => new("SecondarySelectedBrush");

				[ResourceKeyDefinition(typeof(Brush), "SecondaryMediumBrush")]
				public static ThemeResourceKey<Brush> Medium => new("SecondaryMediumBrush");

				[ResourceKeyDefinition(typeof(Brush), "SecondaryLowBrush")]
				public static ThemeResourceKey<Brush> Low => new("SecondaryLowBrush");

				[ResourceKeyDefinition(typeof(Brush), "SecondaryDisabledBrush")]
				public static ThemeResourceKey<Brush> Disabled => new("SecondaryDisabledBrush");

				[ResourceKeyDefinition(typeof(Brush), "SecondaryDisabledLowBrush")]
				public static ThemeResourceKey<Brush> DisabledLow => new("SecondaryDisabledLowBrush");

				public static class Container
				{
					[ResourceKeyDefinition(typeof(Brush), "SecondaryContainerBrush")]
					public static ThemeResourceKey<Brush> Default => new("SecondaryContainerBrush");

					[ResourceKeyDefinition(typeof(Brush), "SecondaryContainerHoverBrush")]
					public static ThemeResourceKey<Brush> Hover => new("SecondaryContainerHoverBrush");

					[ResourceKeyDefinition(typeof(Brush), "SecondaryContainerFocusedBrush")]
					public static ThemeResourceKey<Brush> Focused => new("SecondaryContainerFocusedBrush");

					[ResourceKeyDefinition(typeof(Brush), "SecondaryContainerPressedBrush")]
					public static ThemeResourceKey<Brush> Pressed => new("SecondaryContainerPressedBrush");

					[ResourceKeyDefinition(typeof(Brush), "SecondaryContainerDraggedBrush")]
					public static ThemeResourceKey<Brush> Dragged => new("SecondaryContainerDraggedBrush");

					[ResourceKeyDefinition(typeof(Brush), "SecondaryContainerSelectedBrush")]
					public static ThemeResourceKey<Brush> Selected => new("SecondaryContainerSelectedBrush");

					[ResourceKeyDefinition(typeof(Brush), "SecondaryContainerMediumBrush")]
					public static ThemeResourceKey<Brush> Medium => new("SecondaryContainerMediumBrush");

					[ResourceKeyDefinition(typeof(Brush), "SecondaryContainerLowBrush")]
					public static ThemeResourceKey<Brush> Low => new("SecondaryContainerLowBrush");

					[ResourceKeyDefinition(typeof(Brush), "SecondaryContainerDisabledBrush")]
					public static ThemeResourceKey<Brush> Disabled => new("SecondaryContainerDisabledBrush");

					[ResourceKeyDefinition(typeof(Brush), "SecondaryContainerDisabledLowBrush")]
					public static ThemeResourceKey<Brush> DisabledLow => new("SecondaryContainerDisabledLowBrush");
				}

				public static class VariantLight
				{
					[ResourceKeyDefinition(typeof(Brush), "SecondaryVariantLightBrush")]
					public static ThemeResourceKey<Brush> Default => new("SecondaryVariantLightBrush");

					[ResourceKeyDefinition(typeof(Brush), "SecondaryVariantLightHoverBrush")]
					public static ThemeResourceKey<Brush> Hover => new("SecondaryVariantLightHoverBrush");

					[ResourceKeyDefinition(typeof(Brush), "SecondaryVariantLightFocusedBrush")]
					public static ThemeResourceKey<Brush> Focused => new("SecondaryVariantLightFocusedBrush");

					[ResourceKeyDefinition(typeof(Brush), "SecondaryVariantLightPressedBrush")]
					public static ThemeResourceKey<Brush> Pressed => new("SecondaryVariantLightPressedBrush");

					[ResourceKeyDefinition(typeof(Brush), "SecondaryVariantLightDraggedBrush")]
					public static ThemeResourceKey<Brush> Dragged => new("SecondaryVariantLightDraggedBrush");

					[ResourceKeyDefinition(typeof(Brush), "SecondaryVariantLightSelectedBrush")]
					public static ThemeResourceKey<Brush> Selected => new("SecondaryVariantLightSelectedBrush");

					[ResourceKeyDefinition(typeof(Brush), "SecondaryVariantLightMediumBrush")]
					public static ThemeResourceKey<Brush> Medium => new("SecondaryVariantLightMediumBrush");

					[ResourceKeyDefinition(typeof(Brush), "SecondaryVariantLightLowBrush")]
					public static ThemeResourceKey<Brush> Low => new("SecondaryVariantLightLowBrush");

					[ResourceKeyDefinition(typeof(Brush), "SecondaryVariantLightDisabledBrush")]
					public static ThemeResourceKey<Brush> Disabled => new("SecondaryVariantLightDisabledBrush");

					[ResourceKeyDefinition(typeof(Brush), "SecondaryVariantLightDisabledLowBrush")]
					public static ThemeResourceKey<Brush> DisabledLow => new("SecondaryVariantLightDisabledLowBrush");
				}

				public static class VariantDark
				{
					[ResourceKeyDefinition(typeof(Brush), "SecondaryVariantDarkBrush")]
					public static ThemeResourceKey<Brush> Default => new("SecondaryVariantDarkBrush");

					[ResourceKeyDefinition(typeof(Brush), "SecondaryVariantDarkHoverBrush")]
					public static ThemeResourceKey<Brush> Hover => new("SecondaryVariantDarkHoverBrush");

					[ResourceKeyDefinition(typeof(Brush), "SecondaryVariantDarkFocusedBrush")]
					public static ThemeResourceKey<Brush> Focused => new("SecondaryVariantDarkFocusedBrush");

					[ResourceKeyDefinition(typeof(Brush), "SecondaryVariantDarkPressedBrush")]
					public static ThemeResourceKey<Brush> Pressed => new("SecondaryVariantDarkPressedBrush");

					[ResourceKeyDefinition(typeof(Brush), "SecondaryVariantDarkDraggedBrush")]
					public static ThemeResourceKey<Brush> Dragged => new("SecondaryVariantDarkDraggedBrush");

					[ResourceKeyDefinition(typeof(Brush), "SecondaryVariantDarkSelectedBrush")]
					public static ThemeResourceKey<Brush> Selected => new("SecondaryVariantDarkSelectedBrush");

					[ResourceKeyDefinition(typeof(Brush), "SecondaryVariantDarkMediumBrush")]
					public static ThemeResourceKey<Brush> Medium => new("SecondaryVariantDarkMediumBrush");

					[ResourceKeyDefinition(typeof(Brush), "SecondaryVariantDarkLowBrush")]
					public static ThemeResourceKey<Brush> Low => new("SecondaryVariantDarkLowBrush");

					[ResourceKeyDefinition(typeof(Brush), "SecondaryVariantDarkDisabledBrush")]
					public static ThemeResourceKey<Brush> Disabled => new("SecondaryVariantDarkDisabledBrush");

					[ResourceKeyDefinition(typeof(Brush), "SecondaryVariantDarkDisabledLowBrush")]
					public static ThemeResourceKey<Brush> DisabledLow => new("SecondaryVariantDarkDisabledLowBrush");
				}
			}

			public static class OnSecondary
			{
				[ResourceKeyDefinition(typeof(Brush), "OnSecondaryBrush")]
				public static ThemeResourceKey<Brush> Default => new("OnSecondaryBrush");

				[ResourceKeyDefinition(typeof(Brush), "OnSecondaryHoverBrush")]
				public static ThemeResourceKey<Brush> Hover => new("OnSecondaryHoverBrush");

				[ResourceKeyDefinition(typeof(Brush), "OnSecondaryFocusedBrush")]
				public static ThemeResourceKey<Brush> Focused => new("OnSecondaryFocusedBrush");

				[ResourceKeyDefinition(typeof(Brush), "OnSecondaryPressedBrush")]
				public static ThemeResourceKey<Brush> Pressed => new("OnSecondaryPressedBrush");

				[ResourceKeyDefinition(typeof(Brush), "OnSecondaryDraggedBrush")]
				public static ThemeResourceKey<Brush> Dragged => new("OnSecondaryDraggedBrush");

				[ResourceKeyDefinition(typeof(Brush), "OnSecondarySelectedBrush")]
				public static ThemeResourceKey<Brush> Selected => new("OnSecondarySelectedBrush");

				[ResourceKeyDefinition(typeof(Brush), "OnSecondaryMediumBrush")]
				public static ThemeResourceKey<Brush> Medium => new("OnSecondaryMediumBrush");

				[ResourceKeyDefinition(typeof(Brush), "OnSecondaryLowBrush")]
				public static ThemeResourceKey<Brush> Low => new("OnSecondaryLowBrush");

				[ResourceKeyDefinition(typeof(Brush), "OnSecondaryDisabledBrush")]
				public static ThemeResourceKey<Brush> Disabled => new("OnSecondaryDisabledBrush");

				[ResourceKeyDefinition(typeof(Brush), "OnSecondaryDisabledLowBrush")]
				public static ThemeResourceKey<Brush> DisabledLow => new("OnSecondaryDisabledLowBrush");

				public static class Container
				{
					[ResourceKeyDefinition(typeof(Brush), "OnSecondaryContainerBrush")]
					public static ThemeResourceKey<Brush> Default => new("OnSecondaryContainerBrush");

					[ResourceKeyDefinition(typeof(Brush), "OnSecondaryContainerHoverBrush")]
					public static ThemeResourceKey<Brush> Hover => new("OnSecondaryContainerHoverBrush");

					[ResourceKeyDefinition(typeof(Brush), "OnSecondaryContainerFocusedBrush")]
					public static ThemeResourceKey<Brush> Focused => new("OnSecondaryContainerFocusedBrush");

					[ResourceKeyDefinition(typeof(Brush), "OnSecondaryContainerPressedBrush")]
					public static ThemeResourceKey<Brush> Pressed => new("OnSecondaryContainerPressedBrush");

					public static ThemeResourceKey<Brush> Dragged => new("OnSecondaryContainerDraggedBrush");

					[ResourceKeyDefinition(typeof(Brush), "OnSecondaryContainerSelectedBrush")]
					public static ThemeResourceKey<Brush> Selected => new("OnSecondaryContainerSelectedBrush");

					[ResourceKeyDefinition(typeof(Brush), "OnSecondaryContainerMediumBrush")]
					public static ThemeResourceKey<Brush> Medium => new("OnSecondaryContainerMediumBrush");

					[ResourceKeyDefinition(typeof(Brush), "OnSecondaryContainerLowBrush")]
					public static ThemeResourceKey<Brush> Low => new("OnSecondaryContainerLowBrush");

					[ResourceKeyDefinition(typeof(Brush), "OnSecondaryContainerDisabledBrush")]
					public static ThemeResourceKey<Brush> Disabled => new("OnSecondaryContainerDisabledBrush");

					[ResourceKeyDefinition(typeof(Brush), "OnSecondaryContainerDisabledLowBrush")]
					public static ThemeResourceKey<Brush> DisabledLow => new("OnSecondaryContainerDisabledLowBrush");
				}
			}

			public static class Tertiary
			{
				[ResourceKeyDefinition(typeof(Brush), "TertiaryBrush")]
				public static ThemeResourceKey<Brush> Default => new("TertiaryBrush");

				[ResourceKeyDefinition(typeof(Brush), "TertiaryHoverBrush")]
				public static ThemeResourceKey<Brush> Hover => new("TertiaryHoverBrush");

				[ResourceKeyDefinition(typeof(Brush), "TertiaryFocusedBrush")]
				public static ThemeResourceKey<Brush> Focused => new("TertiaryFocusedBrush");

				[ResourceKeyDefinition(typeof(Brush), "TertiaryPressedBrush")]
				public static ThemeResourceKey<Brush> Pressed => new("TertiaryPressedBrush");

				[ResourceKeyDefinition(typeof(Brush), "TertiaryDraggedBrush")]
				public static ThemeResourceKey<Brush> Dragged => new("TertiaryDraggedBrush");

				[ResourceKeyDefinition(typeof(Brush), "TertiarySelectedBrush")]
				public static ThemeResourceKey<Brush> Selected => new("TertiarySelectedBrush");

				[ResourceKeyDefinition(typeof(Brush), "TertiaryMediumBrush")]
				public static ThemeResourceKey<Brush> Medium => new("TertiaryMediumBrush");

				[ResourceKeyDefinition(typeof(Brush), "TertiaryLowBrush")]
				public static ThemeResourceKey<Brush> Low => new("TertiaryLowBrush");

				[ResourceKeyDefinition(typeof(Brush), "TertiaryDisabledBrush")]
				public static ThemeResourceKey<Brush> Disabled => new("TertiaryDisabledBrush");

				[ResourceKeyDefinition(typeof(Brush), "TertiaryDisabledLowBrush")]
				public static ThemeResourceKey<Brush> DisabledLow => new("TertiaryDisabledLowBrush");

				public static class Container
				{
					[ResourceKeyDefinition(typeof(Brush), "TertiaryContainerBrush")]
					public static ThemeResourceKey<Brush> Default => new("TertiaryContainerBrush");

					[ResourceKeyDefinition(typeof(Brush), "TertiaryContainerHoverBrush")]
					public static ThemeResourceKey<Brush> Hover => new("TertiaryContainerHoverBrush");

					[ResourceKeyDefinition(typeof(Brush), "TertiaryContainerFocusedBrush")]
					public static ThemeResourceKey<Brush> Focused => new("TertiaryContainerFocusedBrush");

					[ResourceKeyDefinition(typeof(Brush), "TertiaryContainerPressedBrush")]
					public static ThemeResourceKey<Brush> Pressed => new("TertiaryContainerPressedBrush");

					[ResourceKeyDefinition(typeof(Brush), "TertiaryContainerDraggedBrush")]
					public static ThemeResourceKey<Brush> Dragged => new("TertiaryContainerDraggedBrush");

					[ResourceKeyDefinition(typeof(Brush), "TertiaryContainerSelectedBrush")]
					public static ThemeResourceKey<Brush> Selected => new("TertiaryContainerSelectedBrush");

					[ResourceKeyDefinition(typeof(Brush), "TertiaryContainerMediumBrush")]
					public static ThemeResourceKey<Brush> Medium => new("TertiaryContainerMediumBrush");

					[ResourceKeyDefinition(typeof(Brush), "TertiaryContainerLowBrush")]
					public static ThemeResourceKey<Brush> Low => new("TertiaryContainerLowBrush");

					[ResourceKeyDefinition(typeof(Brush), "TertiaryContainerDisabledBrush")]
					public static ThemeResourceKey<Brush> Disabled => new("TertiaryContainerDisabledBrush");

					[ResourceKeyDefinition(typeof(Brush), "TertiaryContainerDisabledLowBrush")]
					public static ThemeResourceKey<Brush> DisabledLow => new("TertiaryContainerDisabledLowBrush");
				}
			}

			public static class OnTertiary
			{
				[ResourceKeyDefinition(typeof(Brush), "OnTertiaryBrush")]
				public static ThemeResourceKey<Brush> Default => new("OnTertiaryBrush");

				[ResourceKeyDefinition(typeof(Brush), "OnTertiaryHoverBrush")]
				public static ThemeResourceKey<Brush> Hover => new("OnTertiaryHoverBrush");

				[ResourceKeyDefinition(typeof(Brush), "OnTertiaryFocusedBrush")]
				public static ThemeResourceKey<Brush> Focused => new("OnTertiaryFocusedBrush");

				[ResourceKeyDefinition(typeof(Brush), "OnTertiaryPressedBrush")]
				public static ThemeResourceKey<Brush> Pressed => new("OnTertiaryPressedBrush");

				[ResourceKeyDefinition(typeof(Brush), "OnTertiaryDraggedBrush")]
				public static ThemeResourceKey<Brush> Dragged => new("OnTertiaryDraggedBrush");

				[ResourceKeyDefinition(typeof(Brush), "OnTertiarySelectedBrush")]
				public static ThemeResourceKey<Brush> Selected => new("OnTertiarySelectedBrush");

				[ResourceKeyDefinition(typeof(Brush), "OnTertiaryMediumBrush")]
				public static ThemeResourceKey<Brush> Medium => new("OnTertiaryMediumBrush");

				[ResourceKeyDefinition(typeof(Brush), "OnTertiaryLowBrush")]
				public static ThemeResourceKey<Brush> Low => new("OnTertiaryLowBrush");

				[ResourceKeyDefinition(typeof(Brush), "OnTertiaryDisabledBrush")]
				public static ThemeResourceKey<Brush> Disabled => new("OnTertiaryDisabledBrush");

				[ResourceKeyDefinition(typeof(Brush), "OnTertiaryDisabledLowBrush")]
				public static ThemeResourceKey<Brush> DisabledLow => new("OnTertiaryDisabledLowBrush");

				public static class Container
				{
					[ResourceKeyDefinition(typeof(Brush), "OnTertiaryContainerBrush")]
					public static ThemeResourceKey<Brush> Default => new("OnTertiaryContainerBrush");

					[ResourceKeyDefinition(typeof(Brush), "OnTertiaryContainerHoverBrush")]
					public static ThemeResourceKey<Brush> Hover => new("OnTertiaryContainerHoverBrush");

					[ResourceKeyDefinition(typeof(Brush), "OnTertiaryContainerFocusedBrush")]
					public static ThemeResourceKey<Brush> Focused => new("OnTertiaryContainerFocusedBrush");

					[ResourceKeyDefinition(typeof(Brush), "OnTertiaryContainerPressedBrush")]
					public static ThemeResourceKey<Brush> Pressed => new("OnTertiaryContainerPressedBrush");

					[ResourceKeyDefinition(typeof(Brush), "OnTertiaryContainerDraggedBrush")]
					public static ThemeResourceKey<Brush> Dragged => new("OnTertiaryContainerDraggedBrush");

					[ResourceKeyDefinition(typeof(Brush), "OnTertiaryContainerSelectedBrush")]
					public static ThemeResourceKey<Brush> Selected => new("OnTertiaryContainerSelectedBrush");

					[ResourceKeyDefinition(typeof(Brush), "OnTertiaryContainerMediumBrush")]
					public static ThemeResourceKey<Brush> Medium => new("OnTertiaryContainerMediumBrush");

					[ResourceKeyDefinition(typeof(Brush), "OnTertiaryContainerLowBrush")]
					public static ThemeResourceKey<Brush> Low => new("OnTertiaryContainerLowBrush");

					[ResourceKeyDefinition(typeof(Brush), "OnTertiaryContainerDisabledBrush")]
					public static ThemeResourceKey<Brush> Disabled => new("OnTertiaryContainerDisabledBrush");

					[ResourceKeyDefinition(typeof(Brush), "OnTertiaryContainerDisabledLowBrush")]
					public static ThemeResourceKey<Brush> DisabledLow => new("OnTertiaryContainerDisabledLowBrush");
				}
			}

			public static class Error
			{
				[ResourceKeyDefinition(typeof(Brush), "ErrorBrush")]
				public static ThemeResourceKey<Brush> Default => new("ErrorBrush");

				[ResourceKeyDefinition(typeof(Brush), "ErrorHoverBrush")]
				public static ThemeResourceKey<Brush> Hover => new("ErrorHoverBrush");

				[ResourceKeyDefinition(typeof(Brush), "ErrorFocusedBrush")]
				public static ThemeResourceKey<Brush> Focused => new("ErrorFocusedBrush");

				[ResourceKeyDefinition(typeof(Brush), "ErrorPressedBrush")]
				public static ThemeResourceKey<Brush> Pressed => new("ErrorPressedBrush");

				[ResourceKeyDefinition(typeof(Brush), "ErrorDraggedBrush")]
				public static ThemeResourceKey<Brush> Dragged => new("ErrorDraggedBrush");

				[ResourceKeyDefinition(typeof(Brush), "ErrorSelectedBrush")]
				public static ThemeResourceKey<Brush> Selected => new("ErrorSelectedBrush");

				[ResourceKeyDefinition(typeof(Brush), "ErrorMediumBrush")]
				public static ThemeResourceKey<Brush> Medium => new("ErrorMediumBrush");

				[ResourceKeyDefinition(typeof(Brush), "ErrorLowBrush")]
				public static ThemeResourceKey<Brush> Low => new("ErrorLowBrush");

				[ResourceKeyDefinition(typeof(Brush), "ErrorDisabledBrush")]
				public static ThemeResourceKey<Brush> Disabled => new("ErrorDisabledBrush");

				[ResourceKeyDefinition(typeof(Brush), "ErrorDisabledLowBrush")]
				public static ThemeResourceKey<Brush> DisabledLow => new("ErrorDisabledLowBrush");

				public static class Container
				{
					[ResourceKeyDefinition(typeof(Brush), "ErrorContainerBrush")]
					public static ThemeResourceKey<Brush> Default => new("ErrorContainerBrush");

					[ResourceKeyDefinition(typeof(Brush), "ErrorContainerHoverBrush")]
					public static ThemeResourceKey<Brush> Hover => new("ErrorContainerHoverBrush");

					[ResourceKeyDefinition(typeof(Brush), "ErrorContainerFocusedBrush")]
					public static ThemeResourceKey<Brush> Focused => new("ErrorContainerFocusedBrush");

					[ResourceKeyDefinition(typeof(Brush), "ErrorContainerPressedBrush")]
					public static ThemeResourceKey<Brush> Pressed => new("ErrorContainerPressedBrush");

					[ResourceKeyDefinition(typeof(Brush), "ErrorContainerDraggedBrush")]
					public static ThemeResourceKey<Brush> Dragged => new("ErrorContainerDraggedBrush");

					[ResourceKeyDefinition(typeof(Brush), "ErrorContainerSelectedBrush")]
					public static ThemeResourceKey<Brush> Selected => new("ErrorContainerSelectedBrush");

					[ResourceKeyDefinition(typeof(Brush), "ErrorContainerMediumBrush")]
					public static ThemeResourceKey<Brush> Medium => new("ErrorContainerMediumBrush");

					[ResourceKeyDefinition(typeof(Brush), "ErrorContainerLowBrush")]
					public static ThemeResourceKey<Brush> Low => new("ErrorContainerLowBrush");

					[ResourceKeyDefinition(typeof(Brush), "ErrorContainerDisabledBrush")]
					public static ThemeResourceKey<Brush> Disabled => new("ErrorContainerDisabledBrush");

					[ResourceKeyDefinition(typeof(Brush), "ErrorContainerDisabledLowBrush")]
					public static ThemeResourceKey<Brush> DisabledLow => new("ErrorContainerDisabledLowBrush");
				}
			}

			public static class OnError
			{
				[ResourceKeyDefinition(typeof(Brush), "OnErrorBrush")]
				public static ThemeResourceKey<Brush> Default => new("OnErrorBrush");

				[ResourceKeyDefinition(typeof(Brush), "OnErrorHoverBrush")]
				public static ThemeResourceKey<Brush> Hover => new("OnErrorHoverBrush");

				[ResourceKeyDefinition(typeof(Brush), "OnErrorFocusedBrush")]
				public static ThemeResourceKey<Brush> Focused => new("OnErrorFocusedBrush");

				[ResourceKeyDefinition(typeof(Brush), "OnErrorPressedBrush")]
				public static ThemeResourceKey<Brush> Pressed => new("OnErrorPressedBrush");

				[ResourceKeyDefinition(typeof(Brush), "OnErrorDraggedBrush")]
				public static ThemeResourceKey<Brush> Dragged => new("OnErrorDraggedBrush");

				[ResourceKeyDefinition(typeof(Brush), "OnErrorSelectedBrush")]
				public static ThemeResourceKey<Brush> Selected => new("OnErrorSelectedBrush");

				[ResourceKeyDefinition(typeof(Brush), "OnErrorMediumBrush")]
				public static ThemeResourceKey<Brush> Medium => new("OnErrorMediumBrush");

				[ResourceKeyDefinition(typeof(Brush), "OnErrorLowBrush")]
				public static ThemeResourceKey<Brush> Low => new("OnErrorLowBrush");

				[ResourceKeyDefinition(typeof(Brush), "OnErrorDisabledBrush")]
				public static ThemeResourceKey<Brush> Disabled => new("OnErrorDisabledBrush");

				[ResourceKeyDefinition(typeof(Brush), "OnErrorDisabledLowBrush")]
				public static ThemeResourceKey<Brush> DisabledLow => new("OnErrorDisabledLowBrush");

				public static class Container
				{
					[ResourceKeyDefinition(typeof(Brush), "OnErrorContainerBrush")]
					public static ThemeResourceKey<Brush> Default => new("OnErrorContainerBrush");

					[ResourceKeyDefinition(typeof(Brush), "OnErrorContainerHoverBrush")]
					public static ThemeResourceKey<Brush> Hover => new("OnErrorContainerHoverBrush");

					[ResourceKeyDefinition(typeof(Brush), "OnErrorContainerFocusedBrush")]
					public static ThemeResourceKey<Brush> Focused => new("OnErrorContainerFocusedBrush");

					[ResourceKeyDefinition(typeof(Brush), "OnErrorContainerPressedBrush")]
					public static ThemeResourceKey<Brush> Pressed => new("OnErrorContainerPressedBrush");

					[ResourceKeyDefinition(typeof(Brush), "OnErrorContainerDraggedBrush")]
					public static ThemeResourceKey<Brush> Dragged => new("OnErrorContainerDraggedBrush");

					[ResourceKeyDefinition(typeof(Brush), "OnErrorContainerSelectedBrush")]
					public static ThemeResourceKey<Brush> Selected => new("OnErrorContainerSelectedBrush");

					[ResourceKeyDefinition(typeof(Brush), "OnErrorContainerMediumBrush")]
					public static ThemeResourceKey<Brush> Medium => new("OnErrorContainerMediumBrush");

					[ResourceKeyDefinition(typeof(Brush), "OnErrorContainerLowBrush")]
					public static ThemeResourceKey<Brush> Low => new("OnErrorContainerLowBrush");

					[ResourceKeyDefinition(typeof(Brush), "OnErrorContainerDisabledBrush")]
					public static ThemeResourceKey<Brush> Disabled => new("OnErrorContainerDisabledBrush");

					[ResourceKeyDefinition(typeof(Brush), "OnErrorContainerDisabledLowBrush")]
					public static ThemeResourceKey<Brush> DisabledLow => new("OnErrorContainerDisabledLowBrush");
				}
			}

			public static class Background
			{
				[ResourceKeyDefinition(typeof(Brush), "BackgroundBrush")]
				public static ThemeResourceKey<Brush> Default => new("BackgroundBrush");

				[ResourceKeyDefinition(typeof(Brush), "BackgroundHoverBrush")]
				public static ThemeResourceKey<Brush> Hover => new("BackgroundHoverBrush");

				[ResourceKeyDefinition(typeof(Brush), "BackgroundFocusedBrush")]
				public static ThemeResourceKey<Brush> Focused => new("BackgroundFocusedBrush");

				[ResourceKeyDefinition(typeof(Brush), "BackgroundPressedBrush")]
				public static ThemeResourceKey<Brush> Pressed => new("BackgroundPressedBrush");

				[ResourceKeyDefinition(typeof(Brush), "BackgroundDraggedBrush")]
				public static ThemeResourceKey<Brush> Dragged => new("BackgroundDraggedBrush");

				[ResourceKeyDefinition(typeof(Brush), "BackgroundSelectedBrush")]
				public static ThemeResourceKey<Brush> Selected => new("BackgroundSelectedBrush");

				[ResourceKeyDefinition(typeof(Brush), "BackgroundMediumBrush")]
				public static ThemeResourceKey<Brush> Medium => new("BackgroundMediumBrush");

				[ResourceKeyDefinition(typeof(Brush), "BackgroundLowBrush")]
				public static ThemeResourceKey<Brush> Low => new("BackgroundLowBrush");

				[ResourceKeyDefinition(typeof(Brush), "BackgroundDisabledBrush")]
				public static ThemeResourceKey<Brush> Disabled => new("BackgroundDisabledBrush");

				[ResourceKeyDefinition(typeof(Brush), "BackgroundDisabledLowBrush")]
				public static ThemeResourceKey<Brush> DisabledLow => new("BackgroundDisabledLowBrush");
			}

			public static class OnBackground
			{
				[ResourceKeyDefinition(typeof(Brush), "OnBackgroundBrush")]
				public static ThemeResourceKey<Brush> Default => new("OnBackgroundBrush");

				[ResourceKeyDefinition(typeof(Brush), "OnBackgroundHoverBrush")]
				public static ThemeResourceKey<Brush> Hover => new("OnBackgroundHoverBrush");

				[ResourceKeyDefinition(typeof(Brush), "OnBackgroundFocusedBrush")]
				public static ThemeResourceKey<Brush> Focused => new("OnBackgroundFocusedBrush");

				[ResourceKeyDefinition(typeof(Brush), "OnBackgroundPressedBrush")]
				public static ThemeResourceKey<Brush> Pressed => new("OnBackgroundPressedBrush");

				[ResourceKeyDefinition(typeof(Brush), "OnBackgroundDraggedBrush")]
				public static ThemeResourceKey<Brush> Dragged => new("OnBackgroundDraggedBrush");

				[ResourceKeyDefinition(typeof(Brush), "OnBackgroundSelectedBrush")]
				public static ThemeResourceKey<Brush> Selected => new("OnBackgroundSelectedBrush");

				[ResourceKeyDefinition(typeof(Brush), "OnBackgroundMediumBrush")]
				public static ThemeResourceKey<Brush> Medium => new("OnBackgroundMediumBrush");

				[ResourceKeyDefinition(typeof(Brush), "OnBackgroundLowBrush")]
				public static ThemeResourceKey<Brush> Low => new("OnBackgroundLowBrush");

				[ResourceKeyDefinition(typeof(Brush), "OnBackgroundDisabledBrush")]
				public static ThemeResourceKey<Brush> Disabled => new("OnBackgroundDisabledBrush");

				[ResourceKeyDefinition(typeof(Brush), "OnBackgroundDisabledLowBrush")]
				public static ThemeResourceKey<Brush> DisabledLow => new("OnBackgroundDisabledLowBrush");
			}

			public static class Surface
			{
				[ResourceKeyDefinition(typeof(Brush), "SurfaceBrush")]
				public static ThemeResourceKey<Brush> Default => new("SurfaceBrush");

				[ResourceKeyDefinition(typeof(Brush), "SurfaceHoverBrush")]
				public static ThemeResourceKey<Brush> Hover => new("SurfaceHoverBrush");

				[ResourceKeyDefinition(typeof(Brush), "SurfaceFocusedBrush")]
				public static ThemeResourceKey<Brush> Focused => new("SurfaceFocusedBrush");

				[ResourceKeyDefinition(typeof(Brush), "SurfacePressedBrush")]
				public static ThemeResourceKey<Brush> Pressed => new("SurfacePressedBrush");

				[ResourceKeyDefinition(typeof(Brush), "SurfaceDraggedBrush")]
				public static ThemeResourceKey<Brush> Dragged => new("SurfaceDraggedBrush");

				[ResourceKeyDefinition(typeof(Brush), "SurfaceSelectedBrush")]
				public static ThemeResourceKey<Brush> Selected => new("SurfaceSelectedBrush");

				[ResourceKeyDefinition(typeof(Brush), "SurfaceMediumBrush")]
				public static ThemeResourceKey<Brush> Medium => new("SurfaceMediumBrush");

				[ResourceKeyDefinition(typeof(Brush), "SurfaceLowBrush")]
				public static ThemeResourceKey<Brush> Low => new("SurfaceLowBrush");

				[ResourceKeyDefinition(typeof(Brush), "SurfaceDisabledBrush")]
				public static ThemeResourceKey<Brush> Disabled => new("SurfaceDisabledBrush");

				[ResourceKeyDefinition(typeof(Brush), "SurfaceDisabledLowBrush")]
				public static ThemeResourceKey<Brush> DisabledLow => new("SurfaceDisabledLowBrush");

				public static class Variant
				{
					[ResourceKeyDefinition(typeof(Brush), "SurfaceVariantBrush")]
					public static ThemeResourceKey<Brush> Default => new("SurfaceVariantBrush");

					[ResourceKeyDefinition(typeof(Brush), "SurfaceVariantHoverBrush")]
					public static ThemeResourceKey<Brush> Hover => new("SurfaceVariantHoverBrush");

					[ResourceKeyDefinition(typeof(Brush), "SurfaceVariantFocusedBrush")]
					public static ThemeResourceKey<Brush> Focused => new("SurfaceVariantFocusedBrush");

					[ResourceKeyDefinition(typeof(Brush), "SurfaceVariantPressedBrush")]
					public static ThemeResourceKey<Brush> Pressed => new("SurfaceVariantPressedBrush");

					[ResourceKeyDefinition(typeof(Brush), "SurfaceVariantDraggedBrush")]
					public static ThemeResourceKey<Brush> Dragged => new("SurfaceVariantDraggedBrush");

					[ResourceKeyDefinition(typeof(Brush), "SurfaceVariantSelectedBrush")]
					public static ThemeResourceKey<Brush> Selected => new("SurfaceVariantSelectedBrush");

					[ResourceKeyDefinition(typeof(Brush), "SurfaceVariantMediumBrush")]
					public static ThemeResourceKey<Brush> Medium => new("SurfaceVariantMediumBrush");

					[ResourceKeyDefinition(typeof(Brush), "SurfaceVariantLowBrush")]
					public static ThemeResourceKey<Brush> Low => new("SurfaceVariantLowBrush");

					[ResourceKeyDefinition(typeof(Brush), "SurfaceVariantDisabledBrush")]
					public static ThemeResourceKey<Brush> Disabled => new("SurfaceVariantDisabledBrush");

					[ResourceKeyDefinition(typeof(Brush), "SurfaceVariantDisabledLowBrush")]
					public static ThemeResourceKey<Brush> DisabledLow => new("SurfaceVariantDisabledLowBrush");
				}

				public static class Inverse
				{
					[ResourceKeyDefinition(typeof(Brush), "SurfaceInverseBrush")]
					public static ThemeResourceKey<Brush> Default => new("SurfaceInverseBrush");

					[ResourceKeyDefinition(typeof(Brush), "SurfaceInverseHoverBrush")]
					public static ThemeResourceKey<Brush> Hover => new("SurfaceInverseHoverBrush");

					[ResourceKeyDefinition(typeof(Brush), "SurfaceInverseFocusedBrush")]
					public static ThemeResourceKey<Brush> Focused => new("SurfaceInverseFocusedBrush");

					[ResourceKeyDefinition(typeof(Brush), "SurfaceInversePressedBrush")]
					public static ThemeResourceKey<Brush> Pressed => new("SurfaceInversePressedBrush");

					[ResourceKeyDefinition(typeof(Brush), "SurfaceInverseDraggedBrush")]
					public static ThemeResourceKey<Brush> Dragged => new("SurfaceInverseDraggedBrush");

					[ResourceKeyDefinition(typeof(Brush), "SurfaceInverseSelectedBrush")]
					public static ThemeResourceKey<Brush> Selected => new("SurfaceInverseSelectedBrush");

					[ResourceKeyDefinition(typeof(Brush), "SurfaceInverseMediumBrush")]
					public static ThemeResourceKey<Brush> Medium => new("SurfaceInverseMediumBrush");

					[ResourceKeyDefinition(typeof(Brush), "SurfaceInverseLowBrush")]
					public static ThemeResourceKey<Brush> Low => new("SurfaceInverseLowBrush");

					[ResourceKeyDefinition(typeof(Brush), "SurfaceInverseDisabledBrush")]
					public static ThemeResourceKey<Brush> Disabled => new("SurfaceInverseDisabledBrush");

					[ResourceKeyDefinition(typeof(Brush), "SurfaceInverseDisabledLowBrush")]
					public static ThemeResourceKey<Brush> DisabledLow => new("SurfaceInverseDisabledLowBrush");
				}
			}

			public static class OnSurface
			{
				[ResourceKeyDefinition(typeof(Brush), "OnSurfaceBrush")]
				public static ThemeResourceKey<Brush> Default => new("OnSurfaceBrush");

				[ResourceKeyDefinition(typeof(Brush), "OnSurfaceHoverBrush")]
				public static ThemeResourceKey<Brush> Hover => new("OnSurfaceHoverBrush");

				[ResourceKeyDefinition(typeof(Brush), "OnSurfaceFocusedBrush")]
				public static ThemeResourceKey<Brush> Focused => new("OnSurfaceFocusedBrush");

				[ResourceKeyDefinition(typeof(Brush), "OnSurfacePressedBrush")]
				public static ThemeResourceKey<Brush> Pressed => new("OnSurfacePressedBrush");

				[ResourceKeyDefinition(typeof(Brush), "OnSurfaceDraggedBrush")]
				public static ThemeResourceKey<Brush> Dragged => new("OnSurfaceDraggedBrush");

				[ResourceKeyDefinition(typeof(Brush), "OnSurfaceSelectedBrush")]
				public static ThemeResourceKey<Brush> Selected => new("OnSurfaceSelectedBrush");

				[ResourceKeyDefinition(typeof(Brush), "OnSurfaceMediumBrush")]
				public static ThemeResourceKey<Brush> Medium => new("OnSurfaceMediumBrush");

				[ResourceKeyDefinition(typeof(Brush), "OnSurfaceLowBrush")]
				public static ThemeResourceKey<Brush> Low => new("OnSurfaceLowBrush");

				[ResourceKeyDefinition(typeof(Brush), "OnSurfaceDisabledBrush")]
				public static ThemeResourceKey<Brush> Disabled => new("OnSurfaceDisabledBrush");

				[ResourceKeyDefinition(typeof(Brush), "OnSurfaceDisabledLowBrush")]
				public static ThemeResourceKey<Brush> DisabledLow => new("OnSurfaceDisabledLowBrush");

				public static class Variant
				{
					[ResourceKeyDefinition(typeof(Brush), "OnSurfaceVariantBrush")]
					public static ThemeResourceKey<Brush> Default => new("OnSurfaceVariantBrush");

					[ResourceKeyDefinition(typeof(Brush), "OnSurfaceVariantHoverBrush")]
					public static ThemeResourceKey<Brush> Hover => new("OnSurfaceVariantHoverBrush");

					[ResourceKeyDefinition(typeof(Brush), "OnSurfaceVariantFocusedBrush")]
					public static ThemeResourceKey<Brush> Focused => new("OnSurfaceVariantFocusedBrush");

					[ResourceKeyDefinition(typeof(Brush), "OnSurfaceVariantPressedBrush")]
					public static ThemeResourceKey<Brush> Pressed => new("OnSurfaceVariantPressedBrush");

					[ResourceKeyDefinition(typeof(Brush), "OnSurfaceVariantDraggedBrush")]
					public static ThemeResourceKey<Brush> Dragged => new("OnSurfaceVariantDraggedBrush");

					[ResourceKeyDefinition(typeof(Brush), "OnSurfaceVariantSelectedBrush")]
					public static ThemeResourceKey<Brush> Selected => new("OnSurfaceVariantSelectedBrush");

					[ResourceKeyDefinition(typeof(Brush), "OnSurfaceVariantMediumBrush")]
					public static ThemeResourceKey<Brush> Medium => new("OnSurfaceVariantMediumBrush");

					[ResourceKeyDefinition(typeof(Brush), "OnSurfaceVariantLowBrush")]
					public static ThemeResourceKey<Brush> Low => new("OnSurfaceVariantLowBrush");

					[ResourceKeyDefinition(typeof(Brush), "OnSurfaceVariantDisabledBrush")]
					public static ThemeResourceKey<Brush> Disabled => new("OnSurfaceVariantDisabledBrush");

					[ResourceKeyDefinition(typeof(Brush), "OnSurfaceVariantDisabledLowBrush")]
					public static ThemeResourceKey<Brush> DisabledLow => new("OnSurfaceVariantDisabledLowBrush");
				}

				public static class Inverse
				{
					[ResourceKeyDefinition(typeof(Brush), "OnSurfaceInverseBrush")]
					public static ThemeResourceKey<Brush> Default => new("OnSurfaceInverseBrush");

					[ResourceKeyDefinition(typeof(Brush), "OnSurfaceInverseHoverBrush")]
					public static ThemeResourceKey<Brush> Hover => new("OnSurfaceInverseHoverBrush");

					[ResourceKeyDefinition(typeof(Brush), "OnSurfaceInverseFocusedBrush")]
					public static ThemeResourceKey<Brush> Focused => new("OnSurfaceInverseFocusedBrush");

					[ResourceKeyDefinition(typeof(Brush), "OnSurfaceInversePressedBrush")]
					public static ThemeResourceKey<Brush> Pressed => new("OnSurfaceInversePressedBrush");

					[ResourceKeyDefinition(typeof(Brush), "OnSurfaceInverseDraggedBrush")]
					public static ThemeResourceKey<Brush> Dragged => new("OnSurfaceInverseDraggedBrush");

					[ResourceKeyDefinition(typeof(Brush), "OnSurfaceInverseSelectedBrush")]
					public static ThemeResourceKey<Brush> Selected => new("OnSurfaceInverseSelectedBrush");

					[ResourceKeyDefinition(typeof(Brush), "OnSurfaceInverseMediumBrush")]
					public static ThemeResourceKey<Brush> Medium => new("OnSurfaceInverseMediumBrush");

					[ResourceKeyDefinition(typeof(Brush), "OnSurfaceInverseLowBrush")]
					public static ThemeResourceKey<Brush> Low => new("OnSurfaceInverseLowBrush");

					[ResourceKeyDefinition(typeof(Brush), "OnSurfaceInverseDisabledBrush")]
					public static ThemeResourceKey<Brush> Disabled => new("OnSurfaceInverseDisabledBrush");

					[ResourceKeyDefinition(typeof(Brush), "OnSurfaceInverseDisabledLowBrush")]
					public static ThemeResourceKey<Brush> DisabledLow => new("OnSurfaceInverseDisabledLowBrush");
				}
			}

			public static class Outline
			{
				[ResourceKeyDefinition(typeof(Brush), "OutlineBrush")]
				public static ThemeResourceKey<Brush> Default => new("OutlineBrush");

				[ResourceKeyDefinition(typeof(Brush), "OutlineHoverBrush")]
				public static ThemeResourceKey<Brush> Hover => new("OutlineHoverBrush");

				[ResourceKeyDefinition(typeof(Brush), "OutlineFocusedBrush")]
				public static ThemeResourceKey<Brush> Focused => new("OutlineFocusedBrush");

				[ResourceKeyDefinition(typeof(Brush), "OutlinePressedBrush")]
				public static ThemeResourceKey<Brush> Pressed => new("OutlinePressedBrush");

				[ResourceKeyDefinition(typeof(Brush), "OutlineDraggedBrush")]
				public static ThemeResourceKey<Brush> Dragged => new("OutlineDraggedBrush");

				[ResourceKeyDefinition(typeof(Brush), "OutlineSelectedBrush")]
				public static ThemeResourceKey<Brush> Selected => new("OutlineSelectedBrush");

				[ResourceKeyDefinition(typeof(Brush), "OutlineMediumBrush")]
				public static ThemeResourceKey<Brush> Medium => new("OutlineMediumBrush");

				[ResourceKeyDefinition(typeof(Brush), "OutlineLowBrush")]
				public static ThemeResourceKey<Brush> Low => new("OutlineLowBrush");

				[ResourceKeyDefinition(typeof(Brush), "OutlineDisabledBrush")]
				public static ThemeResourceKey<Brush> Disabled => new("OutlineDisabledBrush");

				[ResourceKeyDefinition(typeof(Brush), "OutlineDisabledLowBrush")]
				public static ThemeResourceKey<Brush> DisabledLow => new("OutlineDisabledLowBrush");

				public static class Variant
				{
					[ResourceKeyDefinition(typeof(Brush), "OutlineVariantBrush")]
					public static ThemeResourceKey<Brush> Default => new("OutlineVariantBrush");

					[ResourceKeyDefinition(typeof(Brush), "OutlineVariantHoverBrush")]
					public static ThemeResourceKey<Brush> Hover => new("OutlineVariantHoverBrush");

					[ResourceKeyDefinition(typeof(Brush), "OutlineVariantFocusedBrush")]
					public static ThemeResourceKey<Brush> Focused => new("OutlineVariantFocusedBrush");

					[ResourceKeyDefinition(typeof(Brush), "OutlineVariantPressedBrush")]
					public static ThemeResourceKey<Brush> Pressed => new("OutlineVariantPressedBrush");

					[ResourceKeyDefinition(typeof(Brush), "OutlineVariantDraggedBrush")]
					public static ThemeResourceKey<Brush> Dragged => new("OutlineVariantDraggedBrush");

					[ResourceKeyDefinition(typeof(Brush), "OutlineVariantSelectedBrush")]
					public static ThemeResourceKey<Brush> Selected => new("OutlineVariantSelectedBrush");

					[ResourceKeyDefinition(typeof(Brush), "OutlineVariantMediumBrush")]
					public static ThemeResourceKey<Brush> Medium => new("OutlineVariantMediumBrush");

					[ResourceKeyDefinition(typeof(Brush), "OutlineVariantLowBrush")]
					public static ThemeResourceKey<Brush> Low => new("OutlineVariantLowBrush");

					[ResourceKeyDefinition(typeof(Brush), "OutlineVariantDisabledBrush")]
					public static ThemeResourceKey<Brush> Disabled => new("OutlineVariantDisabledBrush");

					[ResourceKeyDefinition(typeof(Brush), "OutlineVariantDisabledLowBrush")]
					public static ThemeResourceKey<Brush> DisabledLow => new("OutlineVariantDisabledLowBrush");
				}
			}
		}
	}
}
