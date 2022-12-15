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
				public static ResourceValue<Color> Default => new("PrimaryColor");

				[ResourceKeyDefinition(typeof(Color), "PrimaryInverseColor")]
				public static ResourceValue<Color> Inverse => new("PrimaryInverseColor");

				[ResourceKeyDefinition(typeof(Color), "PrimaryContainerColor")]
				public static ResourceValue<Color> Container => new("PrimaryContainerColor");

				[ResourceKeyDefinition(typeof(Color), "PrimaryVariantDarkColor")]
				public static ResourceValue<Color> VariantDark => new("PrimaryVariantDarkColor");

				[ResourceKeyDefinition(typeof(Color), "PrimaryVariantLightColor")]
				public static ResourceValue<Color> VariantLight => new("PrimaryVariantLightColor");
			}

			public static class OnPrimary
			{
				[ResourceKeyDefinition(typeof(Color), "OnPrimaryColor")]
				public static ResourceValue<Color> Default => new("OnPrimaryColor");

				[ResourceKeyDefinition(typeof(Color), "OnPrimaryContainerColor")]
				public static ResourceValue<Color> Container => new("OnPrimaryContainerColor");
			}

			public static class Secondary
			{
				[ResourceKeyDefinition(typeof(Color), "SecondaryColor")]
				public static ResourceValue<Color> Default => new("SecondaryColor");

				[ResourceKeyDefinition(typeof(Color), "SecondaryContainerColor")]
				public static ResourceValue<Color> Container => new("SecondaryContainerColor");

				[ResourceKeyDefinition(typeof(Color), "SecondaryVariantDarkColor")]
				public static ResourceValue<Color> VariantDark => new("SecondaryVariantDarkColor");

				[ResourceKeyDefinition(typeof(Color), "SecondaryVariantLightColor")]
				public static ResourceValue<Color> VariantLight => new("SecondaryVariantLightColor");
			}

			public static class OnSecondary
			{
				[ResourceKeyDefinition(typeof(Color), "OnSecondaryColor")]
				public static ResourceValue<Color> Default => new("OnSecondaryColor");

				[ResourceKeyDefinition(typeof(Color), "OnSecondaryContainerColor")]
				public static ResourceValue<Color> Container => new("OnSecondaryContainerColor");
			}

			public static class Tertiary
			{
				[ResourceKeyDefinition(typeof(Color), "TertiaryColor")]
				public static ResourceValue<Color> Default => new("TertiaryColor");

				[ResourceKeyDefinition(typeof(Color), "TertiaryContainerColor")]
				public static ResourceValue<Color> Container => new("TertiaryContainerColor");
			}

			public static class OnTertiary
			{
				[ResourceKeyDefinition(typeof(Color), "OnTertiaryColor")]
				public static ResourceValue<Color> Default => new("OnTertiaryColor");

				[ResourceKeyDefinition(typeof(Color), "OnTertiaryContainerColor")]
				public static ResourceValue<Color> Container => new("OnTertiaryContainerColor");
			}

			public static class Error
			{
				[ResourceKeyDefinition(typeof(Color), "ErrorColor")]
				public static ResourceValue<Color> Default => new("ErrorColor");

				[ResourceKeyDefinition(typeof(Color), "ErrorContainerColor")]
				public static ResourceValue<Color> Container => new("ErrorContainerColor");
			}

			public static class OnError
			{
				[ResourceKeyDefinition(typeof(Color), "OnErrorColor")]
				public static ResourceValue<Color> Default => new("OnErrorColor");

				[ResourceKeyDefinition(typeof(Color), "OnErrorContainerColor")]
				public static ResourceValue<Color> Container => new("OnErrorContainerColor");
			}

			public static class Background
			{
				[ResourceKeyDefinition(typeof(Color), "BackgroundColor")]
				public static ResourceValue<Color> Default => new("BackgroundColor");
			}

			public static class OnBackground
			{
				[ResourceKeyDefinition(typeof(Color), "OnBackgroundColor")]
				public static ResourceValue<Color> Default => new("OnBackgroundColor");
			}

			public static class Surface
			{
				[ResourceKeyDefinition(typeof(Color), "SurfaceColor")]
				public static ResourceValue<Color> Default => new("SurfaceColor");

				[ResourceKeyDefinition(typeof(Color), "SurfaceVariantColor")]
				public static ResourceValue<Color> Variant => new("SurfaceVariantColor");

				[ResourceKeyDefinition(typeof(Color), "SurfaceInverseColor")]
				public static ResourceValue<Color> Inverse => new("SurfaceInverseColor");
			}

			public static class OnSurface
			{
				[ResourceKeyDefinition(typeof(Color), "OnSurfaceColor")]
				public static ResourceValue<Color> Default => new("OnSurfaceColor");

				[ResourceKeyDefinition(typeof(Color), "OnSurfaceVariantColor")]
				public static ResourceValue<Color> Variant => new("OnSurfaceVariantColor");

				[ResourceKeyDefinition(typeof(Color), "OnSurfaceInverseColor")]
				public static ResourceValue<Color> Inverse => new("OnSurfaceInverseColor");
			}

			public static class Outline
			{
				[ResourceKeyDefinition(typeof(Color), "OutlineColor")]
				public static ResourceValue<Color> Default => new("OutlineColor");
			}

		}
	}
}
