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
	/// Multiplier table for spacing tokens.
	/// Each entry maps a token suffix to its multiplier relative to the base unit.
	/// E.g. "200" → 2.0 means Space200 = DefaultSpacing × 2.
	/// </summary>
	private static readonly (string Suffix, double Multiplier)[] SpacingScaleMultipliers =
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
	/// Suffixes that have a HorizontalThickness companion (N,0,N,0).
	/// </summary>
	private static readonly HashSet<string> HorizontalThicknessSuffixes =
		new() { "0", "050", "100", "150", "200", "300", "400", "500", "600", "800" };

	/// <summary>
	/// Suffixes that get full directional Thickness companions:
	/// VerticalThickness (0,N,0,N), TopThickness (0,N,0,0), BottomThickness (0,0,0,N),
	/// LeftThickness (N,0,0,0), RightThickness (0,0,N,0).
	/// </summary>
	private static readonly HashSet<string> DirectionalThicknessSuffixes =
		new() { "0", "050", "100", "150", "200", "300", "400", "500", "600", "800" };

	/// <summary>
	/// Multiplier table for shape (corner radius) tokens.
	/// RadiusFull is always 9999 and is handled separately.
	/// </summary>
	private static readonly (string Suffix, double Multiplier)[] ShapeScaleMultipliers =
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

		foreach (var themeKey in new[] { "Default", "Light" })
		{
			var themed = new ResourceDictionary();

			foreach (var (suffix, multiplier) in SpacingScaleMultipliers)
			{
				var value = baseValue * multiplier;

				themed[$"Space{suffix}"] = value;
				themed[$"Space{suffix}Thickness"] = new Thickness(value);

				if (HorizontalThicknessSuffixes.Contains(suffix))
				{
					themed[$"Space{suffix}HorizontalThickness"] = new Thickness(value, 0, value, 0);
				}

				if (DirectionalThicknessSuffixes.Contains(suffix))
				{
					themed[$"Space{suffix}VerticalThickness"] = new Thickness(0, value, 0, value);
					themed[$"Space{suffix}TopThickness"] = new Thickness(0, value, 0, 0);
					themed[$"Space{suffix}BottomThickness"] = new Thickness(0, 0, 0, value);
					themed[$"Space{suffix}LeftThickness"] = new Thickness(value, 0, 0, 0);
					themed[$"Space{suffix}RightThickness"] = new Thickness(0, 0, value, 0);
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

		foreach (var themeKey in new[] { "Default", "Light" })
		{
			var themed = new ResourceDictionary();

			foreach (var (suffix, multiplier) in ShapeScaleMultipliers)
			{
				var value = baseValue * multiplier;

				themed[$"Radius{suffix}"] = value;
				themed[$"Radius{suffix}CornerRadius"] = new CornerRadius(value);
			}

			// RadiusFull is always 9999 (pill shape)
			themed["RadiusFull"] = 9999.0;
			themed["RadiusFullCornerRadius"] = new CornerRadius(9999);

			dict.ThemeDictionaries[themeKey] = themed;
		}

		return dict;
	}

	/// <summary>
	/// Default density token values. These are fixed constants, not scale-derived.
	/// </summary>
	private static readonly (string Key, double Value)[] DensityDefaults =
	{
		("ControlHeightSmall",      32),
		("ControlHeightMedium",     40),
		("ControlHeightMediumLarge", 44),
		("ControlHeightLarge",      48),
		("TouchTargetMinSize",      48),
		("IconSizeSmall",           16),
		("IconSizeMedium",          24),
		("IconSizeLarge",           32),
	};

	/// <summary>
	/// Generates a ResourceDictionary containing all density tokens.
	/// </summary>
	private static ResourceDictionary GenerateDensityDefaults()
	{
		var dict = new ResourceDictionary();

		foreach (var themeKey in new[] { "Default", "Light" })
		{
			var themed = new ResourceDictionary();

			foreach (var (key, value) in DensityDefaults)
			{
				themed[key] = value;
			}

			dict.ThemeDictionaries[themeKey] = themed;
		}

		return dict;
	}
}
