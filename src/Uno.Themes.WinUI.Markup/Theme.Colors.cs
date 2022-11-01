
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.MarkupHelpers;
using Windows.UI;

namespace Uno.Themes.Markup
{
	partial class Theme
	{
		public static class Colors
		{
			public delegate void ColorDependencyPropertyBuilder(IDependencyPropertyBuilder<Color> builder);
			private static ColorDependencyPropertyBuilder GetColorBuilder(string key) => delegate { ThemeResource.Get<Color>(key); };

			public static class Primary
			{
				public static ColorDependencyPropertyBuilder Default => GetColorBuilder("PrimaryColor");
				public static ColorDependencyPropertyBuilder Inverse => GetColorBuilder("PrimaryInverseColor");
				public static ColorDependencyPropertyBuilder Container => GetColorBuilder("PrimaryContainerColor");
				public static ColorDependencyPropertyBuilder VariantDark => GetColorBuilder("PrimaryVariantDarkColor");
				public static ColorDependencyPropertyBuilder VariantLight => GetColorBuilder("PrimaryVariantLightColor");
			}

			public static class OnPrimary
			{
				public static ColorDependencyPropertyBuilder Default => GetColorBuilder("OnPrimaryColor");
				public static ColorDependencyPropertyBuilder Container => GetColorBuilder("OnPrimaryContainerColor");
			}

			public static class Secondary
			{
				public static ColorDependencyPropertyBuilder Default => GetColorBuilder("SecondaryColor");
				public static ColorDependencyPropertyBuilder Container => GetColorBuilder("SecondaryContainerColor");
				public static ColorDependencyPropertyBuilder VariantDark => GetColorBuilder("SecondaryVariantDarkColor");
				public static ColorDependencyPropertyBuilder VariantLight => GetColorBuilder("SecondaryVariantLightColor");
			}

			public static class OnSecondary
			{
				public static ColorDependencyPropertyBuilder Default => GetColorBuilder("OnSecondaryColor");
				public static ColorDependencyPropertyBuilder Container => GetColorBuilder("OnSecondaryContainerColor");
			}

			public static class Tertiary
			{
				public static ColorDependencyPropertyBuilder Default => GetColorBuilder("TertiaryColor");
				public static ColorDependencyPropertyBuilder Container => GetColorBuilder("TertiaryContainerColor");
			}

			public static class OnTertiary
			{
				public static ColorDependencyPropertyBuilder Default => GetColorBuilder("OnTertiaryColor");
				public static ColorDependencyPropertyBuilder Container => GetColorBuilder("OnTertiaryContainerColor");
			}

			public static class Error
			{
				public static ColorDependencyPropertyBuilder Default => GetColorBuilder("ErrorColor");
				public static ColorDependencyPropertyBuilder Container => GetColorBuilder("ErrorContainerColor");
			}

			public static class OnError
			{
				public static ColorDependencyPropertyBuilder Default => GetColorBuilder("OnErrorColor");
				public static ColorDependencyPropertyBuilder Container => GetColorBuilder("OnErrorContainerColor");
			}

			public static class Background
			{
				public static ColorDependencyPropertyBuilder Default => GetColorBuilder("BackgroundColor");
			}

			public static class OnBackground
			{
				public static ColorDependencyPropertyBuilder Default => GetColorBuilder("OnBackgroundColor");
			}

			public static class Surface
			{
				public static ColorDependencyPropertyBuilder Default => GetColorBuilder("SurfaceColor");
				public static ColorDependencyPropertyBuilder Variant => GetColorBuilder("SurfaceVariantColor");
				public static ColorDependencyPropertyBuilder Inverse => GetColorBuilder("SurfaceInverseColor");
			}

			public static class OnSurface
			{
				public static ColorDependencyPropertyBuilder Default => GetColorBuilder("OnSurfaceColor");
				public static ColorDependencyPropertyBuilder Variant => GetColorBuilder("OnSurfaceVariantColor");
				public static ColorDependencyPropertyBuilder Inverse => GetColorBuilder("OnSurfaceInverseColor");
			}

			public static class Outline
			{
				public static ColorDependencyPropertyBuilder Default => GetColorBuilder("OutlineColor");
			}

		}
	}
}
