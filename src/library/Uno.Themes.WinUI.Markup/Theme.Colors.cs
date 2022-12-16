using Microsoft.UI.Xaml.MarkupHelpers.Internals;
using Windows.UI;

namespace Uno.Themes.Markup
{
	partial class Theme
	{
		public static class Colors
		{
			public static class Primary
			{
				[ResourceKeyDefinition(typeof(Color), "PrimaryColor")]
				public static ResourceValue<Color> Default => new("PrimaryColor", true);

				[ResourceKeyDefinition(typeof(Color), "PrimaryInverseColor")]
				public static ResourceValue<Color> Inverse => new("PrimaryInverseColor", true);

				[ResourceKeyDefinition(typeof(Color), "PrimaryContainerColor")]
				public static ResourceValue<Color> Container => new("PrimaryContainerColor", true);

				[ResourceKeyDefinition(typeof(Color), "PrimaryVariantDarkColor")]
				public static ResourceValue<Color> VariantDark => new("PrimaryVariantDarkColor", true);

				[ResourceKeyDefinition(typeof(Color), "PrimaryVariantLightColor")]
				public static ResourceValue<Color> VariantLight => new("PrimaryVariantLightColor", true);
			}

			public static class OnPrimary
			{
				[ResourceKeyDefinition(typeof(Color), "OnPrimaryColor")]
				public static ResourceValue<Color> Default => new("OnPrimaryColor", true);

				[ResourceKeyDefinition(typeof(Color), "OnPrimaryContainerColor")]
				public static ResourceValue<Color> Container => new("OnPrimaryContainerColor", true);
			}

			public static class Secondary
			{
				[ResourceKeyDefinition(typeof(Color), "SecondaryColor")]
				public static ResourceValue<Color> Default => new("SecondaryColor", true);

				[ResourceKeyDefinition(typeof(Color), "SecondaryContainerColor")]
				public static ResourceValue<Color> Container => new("SecondaryContainerColor", true);

				[ResourceKeyDefinition(typeof(Color), "SecondaryVariantDarkColor")]
				public static ResourceValue<Color> VariantDark => new("SecondaryVariantDarkColor", true);

				[ResourceKeyDefinition(typeof(Color), "SecondaryVariantLightColor")]
				public static ResourceValue<Color> VariantLight => new("SecondaryVariantLightColor", true);
			}

			public static class OnSecondary
			{
				[ResourceKeyDefinition(typeof(Color), "OnSecondaryColor")]
				public static ResourceValue<Color> Default => new("OnSecondaryColor", true);

				[ResourceKeyDefinition(typeof(Color), "OnSecondaryContainerColor")]
				public static ResourceValue<Color> Container => new("OnSecondaryContainerColor", true);
			}

			public static class Tertiary
			{
				[ResourceKeyDefinition(typeof(Color), "TertiaryColor")]
				public static ResourceValue<Color> Default => new("TertiaryColor", true);

				[ResourceKeyDefinition(typeof(Color), "TertiaryContainerColor")]
				public static ResourceValue<Color> Container => new("TertiaryContainerColor", true);
			}

			public static class OnTertiary
			{
				[ResourceKeyDefinition(typeof(Color), "OnTertiaryColor")]
				public static ResourceValue<Color> Default => new("OnTertiaryColor", true);

				[ResourceKeyDefinition(typeof(Color), "OnTertiaryContainerColor")]
				public static ResourceValue<Color> Container => new("OnTertiaryContainerColor", true);
			}

			public static class Error
			{
				[ResourceKeyDefinition(typeof(Color), "ErrorColor")]
				public static ResourceValue<Color> Default => new("ErrorColor", true);

				[ResourceKeyDefinition(typeof(Color), "ErrorContainerColor")]
				public static ResourceValue<Color> Container => new("ErrorContainerColor", true);
			}

			public static class OnError
			{
				[ResourceKeyDefinition(typeof(Color), "OnErrorColor")]
				public static ResourceValue<Color> Default => new("OnErrorColor", true);

				[ResourceKeyDefinition(typeof(Color), "OnErrorContainerColor")]
				public static ResourceValue<Color> Container => new("OnErrorContainerColor", true);
			}

			public static class Background
			{
				[ResourceKeyDefinition(typeof(Color), "BackgroundColor")]
				public static ResourceValue<Color> Default => new("BackgroundColor", true);
			}

			public static class OnBackground
			{
				[ResourceKeyDefinition(typeof(Color), "OnBackgroundColor")]
				public static ResourceValue<Color> Default => new("OnBackgroundColor", true);
			}

			public static class Surface
			{
				[ResourceKeyDefinition(typeof(Color), "SurfaceColor")]
				public static ResourceValue<Color> Default => new("SurfaceColor", true);

				[ResourceKeyDefinition(typeof(Color), "SurfaceVariantColor")]
				public static ResourceValue<Color> Variant => new("SurfaceVariantColor", true);

				[ResourceKeyDefinition(typeof(Color), "SurfaceInverseColor")]
				public static ResourceValue<Color> Inverse => new("SurfaceInverseColor", true);
			}

			public static class OnSurface
			{
				[ResourceKeyDefinition(typeof(Color), "OnSurfaceColor")]
				public static ResourceValue<Color> Default => new("OnSurfaceColor", true);

				[ResourceKeyDefinition(typeof(Color), "OnSurfaceVariantColor")]
				public static ResourceValue<Color> Variant => new("OnSurfaceVariantColor", true);

				[ResourceKeyDefinition(typeof(Color), "OnSurfaceInverseColor")]
				public static ResourceValue<Color> Inverse => new("OnSurfaceInverseColor", true);
			}

			public static class Outline
			{
				[ResourceKeyDefinition(typeof(Color), "OutlineColor")]
				public static ResourceValue<Color> Default => new("OutlineColor", true);
			}

		}
	}
}
