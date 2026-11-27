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
	/// Generates a <see cref="SemanticResources"/> dictionary carrying the root typeface token and
	/// every type-scale family key (<see cref="ThemesConstants.TypefaceScaleKeys"/>), all set to
	/// <paramref name="family"/> — or <c>null</c> when no family is set.
	/// </summary>
	/// <param name="family">The theme's font family, or <c>null</c> to leave the type scale alone.</param>
	/// <returns>The generated layer, or <c>null</c> when there is nothing to generate.</returns>
	private static ResourceDictionary GenerateFontFamilyScale(FontFamily family)
		=> GenerateTypefaceLayer(family, ThemesConstants.TypefaceScaleKeys, static () => new SemanticResources());

	/// <summary>
	/// Generates a plain dictionary carrying the design system's own <see cref="FontFamilyAliasKeys"/>
	/// set to <paramref name="family"/> — or <c>null</c> when no family is set or the theme declares
	/// no aliases. Kept apart from <see cref="GenerateFontFamilyScale"/> because those keys are
	/// theme-specific (<c>SimpleButtonFontFamily</c>) and must not land in a <see cref="SemanticResources"/>.
	/// </summary>
	/// <param name="family">The theme's font family, or <c>null</c>.</param>
	/// <returns>The generated layer, or <c>null</c> when there is nothing to generate.</returns>
	private ResourceDictionary GenerateFontFamilyAliases(FontFamily family)
		=> FontFamilyAliasKeys.Length == 0
			? null
			: GenerateTypefaceLayer(family, FontFamilyAliasKeys, static () => new ResourceDictionary());

	private static ResourceDictionary GenerateTypefaceLayer(FontFamily family, string[] keys, Func<ResourceDictionary> create)
	{
		if (family is null)
		{
			return null;
		}

		var dict = create();

		foreach (var themeKey in TypefaceThemeKeys)
		{
			// The theme dictionaries exist so a lookup under any appearance resolves here rather
			// than falling through to the theme's own Fonts.xaml.
			var themed = create();

			foreach (var key in keys)
			{
				themed[key] = family;
			}

			dict.ThemeDictionaries[themeKey] = themed;
		}

		return dict;
	}

	/// <summary>
	/// Multiplier table for spacing tokens: Material 3's steps (0–9× plus 0.25×, 0.5×, 0.75×, 1.25×)
	/// and the 1.5×, 2.5×, 12× and 20× extensions, which keep a token for every value of the previous 4-based scale.
	/// E.g. "200" → 2.0 means Space200 = DefaultSpacing × 2.
	/// </summary>
	private static readonly (string Variant, double Multiplier)[] SpacingScaleMultipliers =
	{
		("0",    0.0),
		("025",  0.25),
		("050",  0.5),
		("075",  0.75),
		("100",  1.0),
		("125",  1.25),
		("150",  1.5),
		("200",  2.0),
		("250",  2.5),
		("300",  3.0),
		("400",  4.0),
		("500",  5.0),
		("600",  6.0),
		("700",  7.0),
		("800",  8.0),
		("900",  9.0),
		("1200", 12.0),
		("2000", 20.0),
	};

	/// <summary>
	/// Variants that have a HorizontalThickness companion (N,0,N,0).
	/// </summary>
	private static readonly HashSet<string> HorizontalThicknessVariants =
		new() { "0", "025", "050", "075", "100", "125", "150", "200", "250", "300", "400", "500", "600", "700", "800", "900" };

	/// <summary>
	/// Variants that have directional Thickness companions
	/// (VerticalThickness, TopThickness, BottomThickness, LeftThickness, RightThickness).
	/// </summary>
	private static readonly HashSet<string> DirectionalThicknessVariants =
		new() { "0", "025", "050", "075", "100", "125", "150", "200", "250", "300", "400", "500", "600", "700", "800", "900" };

	/// <summary>
	/// Multiplier table for shape (corner radius) tokens.
	/// RadiusFull is always 9999 and is handled separately (deprecated, see GenerateShapeScale).
	/// The values at the default base of 4 are today's look: do not change the multipliers.
	/// </summary>
	private static readonly (string Variant, double Multiplier)[] ShapeScaleMultipliers =
	{
		("0",   0.0),
		("050", 0.5),
		("100", 1.0),
		("200", 2.0),
		("300", 3.0),
		("400", 4.0),
		("500", 5.0),
		("700", 7.0),
	};

	/// <summary>
	/// Generates a ResourceDictionary containing all spacing scale tokens
	/// derived from the given base value.
	/// </summary>
	private static ResourceDictionary GenerateSpacingScale(double baseValue)
	{
		var dict = new SemanticResources();

		foreach (var themeKey in ThemeKeys)
		{
			var themed = new SemanticResources();

			foreach (var (variant, multiplier) in SpacingScaleMultipliers)
			{
				var value = baseValue * multiplier;

				themed[$"Space{variant}"] = value;
				themed[$"Space{variant}Thickness"] = new Thickness(value);

				if (HorizontalThicknessVariants.Contains(variant))
				{
					themed[$"Space{variant}HorizontalThickness"] = new Thickness(value, 0, value, 0);
				}

				if (DirectionalThicknessVariants.Contains(variant))
				{
					themed[$"Space{variant}VerticalThickness"] = new Thickness(0, value, 0, value);
					themed[$"Space{variant}TopThickness"] = new Thickness(0, value, 0, 0);
					themed[$"Space{variant}BottomThickness"] = new Thickness(0, 0, 0, value);
					themed[$"Space{variant}LeftThickness"] = new Thickness(value, 0, 0, 0);
					themed[$"Space{variant}RightThickness"] = new Thickness(0, 0, value, 0);
				}
			}

			dict.ThemeDictionaries[themeKey] = themed;
		}

		return dict;
	}

	/// <summary>
	/// The default base corner radius (px) when <see cref="DefaultCornerRadius"/> is not set
	/// or holds an invalid (non-finite or negative) value.
	/// </summary>
	private const double DefaultBaseCornerRadius = 4.0;

	/// <summary>
	/// Returns <paramref name="requested"/> when it is a usable base radius, otherwise the default
	/// of 4. A negative radius throws from the <see cref="CornerRadius"/> constructor and a NaN one
	/// poisons every Radius* token, and this runs from a property-changed callback.
	/// </summary>
	private static double SanitizeCornerRadius(double requested)
		=> double.IsFinite(requested) && requested >= 0 ? requested : DefaultBaseCornerRadius;

	/// <summary>
	/// The radii this design system caps by the size of the control they sit on, generated on every
	/// rebuild as <c>min(DefaultCornerRadius × Multiplier, Cap)</c>. The shape scale grows with
	/// <see cref="DefaultCornerRadius"/> but a control's size does not, so an uncapped step ends up
	/// larger than half the control and turns it into an ellipse (unoplatform/Uno.Themes#1747).
	/// Concrete themes override this with the roles their Styles tree aliases; the cap of each must
	/// be at least its value at the default base of 4, so the default look is unchanged.
	/// </summary>
	internal virtual CappedRadius[] CappedRadii => Array.Empty<CappedRadius>();

	/// <summary>
	/// Generates a ResourceDictionary containing all shape scale tokens
	/// derived from the given base value. RadiusFull is always 9999.
	/// </summary>
	/// <remarks>
	/// RadiusFull is deprecated and kept only because its keys are public. It is not a pill: WinUI
	/// (<c>CGeometryBuilder::CalculateRoundedCornersRectangle</c>) and Uno fit an oversized radius to
	/// each edge apart, so 9999 becomes half the width by half the height, an ellipse on any element
	/// that is not square (unoplatform/Uno.Themes#1789). No fixed value can mean "half the shorter side".
	/// </remarks>
	private static ResourceDictionary GenerateShapeScale(double baseValue)
	{
		var dict = new SemanticResources();

		foreach (var themeKey in ThemeKeys)
		{
			var themed = new SemanticResources();

			foreach (var (variant, multiplier) in ShapeScaleMultipliers)
			{
				AddRadius(themed, $"Radius{variant}", baseValue * multiplier);
			}

			// Deprecated: a circle only on a square element, an ellipse on anything else (see remarks).
			themed["RadiusFull"] = 9999.0;
			themed["RadiusFullCornerRadius"] = new CornerRadius(9999);

			dict.ThemeDictionaries[themeKey] = themed;
		}

		return dict;
	}

	/// <summary>
	/// Generates a plain dictionary carrying this design system's <see cref="CappedRadii"/>, or
	/// <c>null</c> when it declares none. Kept apart from <see cref="GenerateShapeScale"/> because
	/// the keys are theme-specific and must not land in a <see cref="SemanticResources"/>.
	/// </summary>
	private ResourceDictionary GenerateCappedRadii(double baseValue)
	{
		var radii = CappedRadii;
		if (radii.Length == 0)
		{
			return null;
		}

		var dict = new ResourceDictionary();

		foreach (var themeKey in ThemeKeys)
		{
			var themed = new ResourceDictionary();

			foreach (var radius in radii)
			{
				AddRadius(themed, radius.Key, Math.Min(baseValue * radius.Multiplier, radius.Cap));
			}

			dict.ThemeDictionaries[themeKey] = themed;
		}

		return dict;
	}

	/// <summary>
	/// Adds a radius under <paramref name="key"/> with its <see cref="CornerRadius"/> companions:
	/// uniform, and the Top (r,r,0,0), Left (r,0,0,r), Bottom (0,0,r,r) and TopLeft (r,0,0,0)
	/// directional ones, mirroring the spacing scale's directional thicknesses.
	/// </summary>
	private static void AddRadius(ResourceDictionary target, string key, double value)
	{
		target[key] = value;
		target[$"{key}CornerRadius"] = new CornerRadius(value);
		target[$"{key}TopCornerRadius"] = new CornerRadius(value, value, 0, 0);
		target[$"{key}LeftCornerRadius"] = new CornerRadius(value, 0, 0, value);
		target[$"{key}BottomCornerRadius"] = new CornerRadius(0, 0, value, value);
		target[$"{key}TopLeftCornerRadius"] = new CornerRadius(value, 0, 0, 0);
	}

	/// <summary>
	/// Fixed density token values. These are constants that do not scale with density.
	/// Includes icon sizes, touch target, and control heights.
	/// </summary>
	private static readonly (string Key, double Value)[] FixedDensityDefaults =
	{
		("TouchTargetMinSize",      48),
		("IconSizeSmall",           16),
		("IconSizeMedium",          24),
		("IconSizeLarge",           32),
		("ControlHeightSmall",      32),
		("ControlHeightMedium",     40),
		("ControlHeightMediumLarge", 44),
		("ControlHeightLarge",      48),
	};

	/// <summary>
	/// Generates a ResourceDictionary containing all density tokens.
	/// All values are fixed constants — they do not scale with density.
	/// Only spacing (padding, margins) changes across density presets.
	/// </summary>
	private static ResourceDictionary GenerateDensityDefaults()
	{
		var dict = new SemanticResources();

		foreach (var themeKey in ThemeKeys)
		{
			var themed = new SemanticResources();

			foreach (var (key, value) in FixedDensityDefaults)
			{
				themed[key] = value;
			}

			dict.ThemeDictionaries[themeKey] = themed;
		}

		return dict;
	}
}
