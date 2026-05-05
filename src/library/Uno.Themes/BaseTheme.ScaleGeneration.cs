using System.Collections.Generic;


#if WinUI
using Microsoft.UI.Xaml;
#else
using Windows.UI.Xaml;
#endif


namespace Uno.Themes;

public abstract partial class BaseTheme
{
	/// <summary>
	/// Theme dictionary keys used when generating token ResourceDictionaries.
	/// </summary>
	private static readonly string[] ThemeKeys = { "Default", "Light" };

	/// <summary>
	/// Multiplier table for spacing tokens.
	/// Each entry maps a token variant to its multiplier relative to the base unit.
	/// E.g. "200" → 2.0 means Space200 = DefaultSpacing × 2.
	/// </summary>
	private static readonly (string Variant, double Multiplier)[] SpacingScaleMultipliers =
	{
		("0",    0.0),
		("050",  0.5),
		("100",  1.0),
		("150",  1.5),
		("200",  2.0),
		("300",  3.0),
		("400",  4.0),
		("500",  5.0),
		("600",  6.0),
		("800",  8.0),
		("1200", 12.0),
		("1600", 16.0),
		("2400", 24.0),
		("4000", 40.0),
	};

	/// <summary>
	/// Variants that have a HorizontalThickness companion (N,0,N,0).
	/// </summary>
	private static readonly HashSet<string> HorizontalThicknessVariants =
		new() { "0", "050", "100", "150", "200", "300", "400", "500", "600", "800" };

	/// <summary>
	/// Variants that have directional Thickness companions
	/// (VerticalThickness, TopThickness, BottomThickness, LeftThickness, RightThickness).
	/// </summary>
	private static readonly HashSet<string> DirectionalThicknessVariants =
		new() { "0", "050", "100", "150", "200", "300", "400", "500", "600", "800" };

	/// <summary>
	/// Multiplier table for shape (corner radius) tokens.
	/// RadiusFull is always 9999 and is handled separately.
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
		var dict = new ResourceDictionary();

		foreach (var themeKey in ThemeKeys)
		{
			var themed = new ResourceDictionary();

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
	/// Generates a ResourceDictionary containing all shape scale tokens
	/// derived from the given base value. RadiusFull is always 9999.
	/// </summary>
	private static ResourceDictionary GenerateShapeScale(double baseValue)
	{
		var dict = new ResourceDictionary();

		foreach (var themeKey in ThemeKeys)
		{
			var themed = new ResourceDictionary();

			foreach (var (variant, multiplier) in ShapeScaleMultipliers)
			{
				var value = baseValue * multiplier;

				themed[$"Radius{variant}"] = value;
				themed[$"Radius{variant}CornerRadius"] = new CornerRadius(value);
			}

			// RadiusFull is always 9999 (pill shape)
			themed["RadiusFull"] = 9999.0;
			themed["RadiusFullCornerRadius"] = new CornerRadius(9999);

			dict.ThemeDictionaries[themeKey] = themed;
		}

		return dict;
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
		var dict = new ResourceDictionary();

		foreach (var themeKey in ThemeKeys)
		{
			var themed = new ResourceDictionary();

			foreach (var (key, value) in FixedDensityDefaults)
			{
				themed[key] = value;
			}

			dict.ThemeDictionaries[themeKey] = themed;
		}

		return dict;
	}
}
