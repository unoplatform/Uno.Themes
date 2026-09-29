using System;
using System.Collections.Generic;


#if WinUI
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Media;
#else
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;
#endif


namespace Uno.Themes;

public abstract partial class BaseTheme
{
	/// <summary>
	/// Theme dictionary keys used when generating token ResourceDictionaries.
	/// </summary>
	private static readonly string[] ThemeKeys = { "Default", "Light" };

	/// <summary>
	/// Theme dictionary keys the typeface layer is generated for. Unlike the other scales, this
	/// includes <c>HighContrast</c>: Material and Cupertino declare their root typeface in a
	/// HighContrast font dictionary, which would otherwise win over a generated layer that carries
	/// no HighContrast entry. A font family is appearance-independent, so all three carry the same value.
	/// </summary>
	private static readonly string[] TypefaceThemeKeys = { "Default", "Light", "HighContrast" };

	/// <summary>
	/// Per-control font family alias keys this design system declares over the root token or a
	/// type-scale slot (e.g. Simple's <c>SimpleButtonFontFamily</c>, Material's <c>SliderFontFamily</c>).
	/// Those aliases are <c>StaticResource</c> references that snapshot at XAML parse time, so a
	/// runtime <see cref="DefaultFontFamily"/> change only reaches the control templates when the
	/// generated layer regenerates these keys too. Concrete themes override this with the keys their
	/// Styles tree declares — keep the override in sync when adding a per-control
	/// <c>*FontFamily</c> lightweight-styling key.
	/// </summary>
	internal virtual string[] FontFamilyAliasKeys => Array.Empty<string>();

	/// <summary>
	/// Generates a ResourceDictionary carrying the root typeface token, every type-scale family key
	/// (<see cref="ThemesConstants.TypefaceScaleKeys"/>), and the design system's own
	/// <see cref="FontFamilyAliasKeys"/>, all set to <paramref name="family"/> — or <c>null</c>
	/// when no family is set.
	/// </summary>
	/// <param name="family">The theme's font family, or <c>null</c> to leave the type scale alone.</param>
	/// <returns>The generated layer, or <c>null</c> when there is nothing to generate.</returns>
	private ResourceDictionary GenerateFontFamilyScale(FontFamily family)
	{
		if (family is null)
		{
			return null;
		}

		var dict = new ResourceDictionary();

		foreach (var themeKey in TypefaceThemeKeys)
		{
			// The theme dictionaries exist so a lookup under any appearance resolves here rather
			// than falling through to the theme's own Fonts.xaml.
			var themed = new ResourceDictionary();

			foreach (var key in ThemesConstants.TypefaceScaleKeys)
			{
				themed[key] = family;
			}

			foreach (var key in FontFamilyAliasKeys)
			{
				themed[key] = family;
			}

			dict.ThemeDictionaries[themeKey] = themed;
		}

		return dict;
	}

	/// <summary>
	/// Generates a ResourceDictionary containing all spacing scale tokens
	/// derived from the given base value.
	/// </summary>
	private static ResourceDictionary GenerateSpacingScale(double baseValue)
		=> GenerateThemedTokens(DesignTokenScales.SpacingTokens(baseValue));

	/// <summary>
	/// Generates a ResourceDictionary containing all shape scale tokens
	/// derived from the given base value. RadiusFull is always 9999.
	/// </summary>
	private static ResourceDictionary GenerateShapeScale(double baseValue)
		=> GenerateThemedTokens(DesignTokenScales.ShapeTokens(baseValue));

	/// <summary>
	/// Generates a ResourceDictionary containing all density tokens.
	/// All values are fixed constants — they do not scale with density.
	/// Only spacing (padding, margins) changes across density presets.
	/// </summary>
	private static ResourceDictionary GenerateDensityDefaults()
		=> GenerateThemedTokens(DesignTokenScales.DensityTokens());

	/// <summary>
	/// Generates a ResourceDictionary carrying <paramref name="tokens"/> under every entry of
	/// <see cref="ThemeKeys"/>.
	/// </summary>
	/// <param name="tokens">The token keys and values to write; enumerated once per theme key.</param>
	/// <returns>The generated dictionary.</returns>
	private static ResourceDictionary GenerateThemedTokens(IEnumerable<(string Key, object Value)> tokens)
	{
		var dict = new ResourceDictionary();

		foreach (var themeKey in ThemeKeys)
		{
			var themed = new ResourceDictionary();

			foreach (var (key, value) in tokens)
			{
				themed[key] = value;
			}

			dict.ThemeDictionaries[themeKey] = themed;
		}

		return dict;
	}
}
