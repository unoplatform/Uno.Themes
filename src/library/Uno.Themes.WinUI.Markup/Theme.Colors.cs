using Uno.Extensions.Markup.Internals;
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
				public static ThemeResourceKey<Color> Default => new("PrimaryColor");

				[ResourceKeyDefinition(typeof(Color), "PrimaryInverseColor")]
				public static ThemeResourceKey<Color> Inverse => new("PrimaryInverseColor");

				[ResourceKeyDefinition(typeof(Color), "PrimaryContainerColor")]
				public static ThemeResourceKey<Color> Container => new("PrimaryContainerColor");

				[ResourceKeyDefinition(typeof(Color), "PrimaryVariantDarkColor")]
				public static ThemeResourceKey<Color> VariantDark => new("PrimaryVariantDarkColor");

				[ResourceKeyDefinition(typeof(Color), "PrimaryVariantLightColor")]
				public static ThemeResourceKey<Color> VariantLight => new("PrimaryVariantLightColor");
			}

			public static class OnPrimary
			{
				[ResourceKeyDefinition(typeof(Color), "OnPrimaryColor")]
				public static ThemeResourceKey<Color> Default => new("OnPrimaryColor");

				[ResourceKeyDefinition(typeof(Color), "OnPrimaryContainerColor")]
				public static ThemeResourceKey<Color> Container => new("OnPrimaryContainerColor");
			}

			public static class Secondary
			{
				[ResourceKeyDefinition(typeof(Color), "SecondaryColor")]
				public static ThemeResourceKey<Color> Default => new("SecondaryColor");

				[ResourceKeyDefinition(typeof(Color), "SecondaryContainerColor")]
				public static ThemeResourceKey<Color> Container => new("SecondaryContainerColor");

				[ResourceKeyDefinition(typeof(Color), "SecondaryVariantDarkColor")]
				public static ThemeResourceKey<Color> VariantDark => new("SecondaryVariantDarkColor");

				[ResourceKeyDefinition(typeof(Color), "SecondaryVariantLightColor")]
				public static ThemeResourceKey<Color> VariantLight => new("SecondaryVariantLightColor");
			}

			public static class OnSecondary
			{
				[ResourceKeyDefinition(typeof(Color), "OnSecondaryColor")]
				public static ThemeResourceKey<Color> Default => new("OnSecondaryColor");

				[ResourceKeyDefinition(typeof(Color), "OnSecondaryContainerColor")]
				public static ThemeResourceKey<Color> Container => new("OnSecondaryContainerColor");
			}

			public static class Tertiary
			{
				[ResourceKeyDefinition(typeof(Color), "TertiaryColor")]
				public static ThemeResourceKey<Color> Default => new("TertiaryColor");

				[ResourceKeyDefinition(typeof(Color), "TertiaryContainerColor")]
				public static ThemeResourceKey<Color> Container => new("TertiaryContainerColor");
			}

			public static class OnTertiary
			{
				[ResourceKeyDefinition(typeof(Color), "OnTertiaryColor")]
				public static ThemeResourceKey<Color> Default => new("OnTertiaryColor");

				[ResourceKeyDefinition(typeof(Color), "OnTertiaryContainerColor")]
				public static ThemeResourceKey<Color> Container => new("OnTertiaryContainerColor");
			}

			public static class Error
			{
				[ResourceKeyDefinition(typeof(Color), "ErrorColor")]
				public static ThemeResourceKey<Color> Default => new("ErrorColor");

				[ResourceKeyDefinition(typeof(Color), "ErrorContainerColor")]
				public static ThemeResourceKey<Color> Container => new("ErrorContainerColor");
			}

			public static class OnError
			{
				[ResourceKeyDefinition(typeof(Color), "OnErrorColor")]
				public static ThemeResourceKey<Color> Default => new("OnErrorColor");

				[ResourceKeyDefinition(typeof(Color), "OnErrorContainerColor")]
				public static ThemeResourceKey<Color> Container => new("OnErrorContainerColor");
			}

			public static class Background
			{
				[ResourceKeyDefinition(typeof(Color), "BackgroundColor")]
				public static ThemeResourceKey<Color> Default => new("BackgroundColor");
			}

			public static class OnBackground
			{
				[ResourceKeyDefinition(typeof(Color), "OnBackgroundColor")]
				public static ThemeResourceKey<Color> Default => new("OnBackgroundColor");
			}

			public static class Surface
			{
				[ResourceKeyDefinition(typeof(Color), "SurfaceColor")]
				public static ThemeResourceKey<Color> Default => new("SurfaceColor");

				[ResourceKeyDefinition(typeof(Color), "SurfaceVariantColor")]
				public static ThemeResourceKey<Color> Variant => new("SurfaceVariantColor");

				[ResourceKeyDefinition(typeof(Color), "SurfaceInverseColor")]
				public static ThemeResourceKey<Color> Inverse => new("SurfaceInverseColor");

				[ResourceKeyDefinition(typeof(Color), "SurfaceTintColor")]
				public static ThemeResourceKey<Color> Tint => new("SurfaceTintColor");
			}

			public static class OnSurface
			{
				[ResourceKeyDefinition(typeof(Color), "OnSurfaceColor")]
				public static ThemeResourceKey<Color> Default => new("OnSurfaceColor");

				[ResourceKeyDefinition(typeof(Color), "OnSurfaceVariantColor")]
				public static ThemeResourceKey<Color> Variant => new("OnSurfaceVariantColor");

				[ResourceKeyDefinition(typeof(Color), "OnSurfaceInverseColor")]
				public static ThemeResourceKey<Color> Inverse => new("OnSurfaceInverseColor");
			}

			public static class Outline
			{
				[ResourceKeyDefinition(typeof(Color), "OutlineColor")]
				public static ThemeResourceKey<Color> Default => new("OutlineColor");

				[ResourceKeyDefinition(typeof(Color), "OutlineVariantColor")]
				public static ThemeResourceKey<Color> Variant => new ("OutlineVariantColor");
			}

		}
	}
}
