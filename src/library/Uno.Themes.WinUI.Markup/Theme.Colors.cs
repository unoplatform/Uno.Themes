using System;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.MarkupHelpers;
using Windows.UI;

namespace Uno.Themes.Markup
{
	partial class Theme
	{
		public static class Colors
		{
			public static class Primary
			{
				public static Action<IDependencyPropertyBuilder<Color>> Default => ThemeResource.Get<Color>("PrimaryColor");
				public static Action<IDependencyPropertyBuilder<Color>> Inverse => ThemeResource.Get<Color>("PrimaryInverseColor");
				public static Action<IDependencyPropertyBuilder<Color>> Container => ThemeResource.Get<Color>("PrimaryContainerColor");
				public static Action<IDependencyPropertyBuilder<Color>> VariantDark => ThemeResource.Get<Color>("PrimaryVariantDarkColor");
				public static Action<IDependencyPropertyBuilder<Color>> VariantLight => ThemeResource.Get<Color>("PrimaryVariantLightColor");
			}

			public static class OnPrimary
			{
				public static Action<IDependencyPropertyBuilder<Color>> Default => ThemeResource.Get<Color>("OnPrimaryColor");
				public static Action<IDependencyPropertyBuilder<Color>> Container => ThemeResource.Get<Color>("OnPrimaryContainerColor");
			}

			public static class Secondary
			{
				public static Action<IDependencyPropertyBuilder<Color>> Default => ThemeResource.Get<Color>("SecondaryColor");
				public static Action<IDependencyPropertyBuilder<Color>> Container => ThemeResource.Get<Color>("SecondaryContainerColor");
				public static Action<IDependencyPropertyBuilder<Color>> VariantDark => ThemeResource.Get<Color>("SecondaryVariantDarkColor");
				public static Action<IDependencyPropertyBuilder<Color>> VariantLight => ThemeResource.Get<Color>("SecondaryVariantLightColor");
			}

			public static class OnSecondary
			{
				public static Action<IDependencyPropertyBuilder<Color>> Default => ThemeResource.Get<Color>("OnSecondaryColor");
				public static Action<IDependencyPropertyBuilder<Color>> Container => ThemeResource.Get<Color>("OnSecondaryContainerColor");
			}

			public static class Tertiary
			{
				public static Action<IDependencyPropertyBuilder<Color>> Default => ThemeResource.Get<Color>("TertiaryColor");
				public static Action<IDependencyPropertyBuilder<Color>> Container => ThemeResource.Get<Color>("TertiaryContainerColor");
			}

			public static class OnTertiary
			{
				public static Action<IDependencyPropertyBuilder<Color>> Default => ThemeResource.Get<Color>("OnTertiaryColor");
				public static Action<IDependencyPropertyBuilder<Color>> Container => ThemeResource.Get<Color>("OnTertiaryContainerColor");
			}

			public static class Error
			{
				public static Action<IDependencyPropertyBuilder<Color>> Default => ThemeResource.Get<Color>("ErrorColor");
				public static Action<IDependencyPropertyBuilder<Color>> Container => ThemeResource.Get<Color>("ErrorContainerColor");
			}

			public static class OnError
			{
				public static Action<IDependencyPropertyBuilder<Color>> Default => ThemeResource.Get<Color>("OnErrorColor");
				public static Action<IDependencyPropertyBuilder<Color>> Container => ThemeResource.Get<Color>("OnErrorContainerColor");
			}

			public static class Background
			{
				public static Action<IDependencyPropertyBuilder<Color>> Default => ThemeResource.Get<Color>("BackgroundColor");
			}

			public static class OnBackground
			{
				public static Action<IDependencyPropertyBuilder<Color>> Default => ThemeResource.Get<Color>("OnBackgroundColor");
			}

			public static class Surface
			{
				public static Action<IDependencyPropertyBuilder<Color>> Default => ThemeResource.Get<Color>("SurfaceColor");
				public static Action<IDependencyPropertyBuilder<Color>> Variant => ThemeResource.Get<Color>("SurfaceVariantColor");
				public static Action<IDependencyPropertyBuilder<Color>> Inverse => ThemeResource.Get<Color>("SurfaceInverseColor");
			}

			public static class OnSurface
			{
				public static Action<IDependencyPropertyBuilder<Color>> Default => ThemeResource.Get<Color>("OnSurfaceColor");
				public static Action<IDependencyPropertyBuilder<Color>> Variant => ThemeResource.Get<Color>("OnSurfaceVariantColor");
				public static Action<IDependencyPropertyBuilder<Color>> Inverse => ThemeResource.Get<Color>("OnSurfaceInverseColor");
			}

			public static class Outline
			{
				public static Action<IDependencyPropertyBuilder<Color>> Default => ThemeResource.Get<Color>("OutlineColor");
			}

		}
	}
}
