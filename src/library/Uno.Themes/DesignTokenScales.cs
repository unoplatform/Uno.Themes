using System.Collections.Generic;

#if WinUI
using Microsoft.UI.Xaml;
#else
using Windows.UI.Xaml;
#endif

namespace Uno.Themes;

/// <summary>
/// The spacing, shape and density token tables, and the key/value pairs each generated scale holds.
/// </summary>
/// <remarks>
/// Kept apart from <see cref="BaseTheme"/> so <see cref="SemanticResourceKeys"/> can enumerate the
/// generated keys without running <see cref="BaseTheme"/>'s static initializer, which registers
/// dependency properties.
/// </remarks>
internal static class DesignTokenScales
{
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
	/// Enumerates every spacing token derived from a base unit: each <c>Space*</c> value and its
	/// <see cref="Thickness"/> companions.
	/// </summary>
	/// <param name="baseValue">The spacing base unit the multipliers apply to.</param>
	/// <returns>The token keys paired with their values.</returns>
	internal static IEnumerable<(string Key, object Value)> SpacingTokens(double baseValue)
	{
		foreach (var (variant, multiplier) in SpacingScaleMultipliers)
		{
			var value = baseValue * multiplier;

			yield return ($"Space{variant}", value);
			yield return ($"Space{variant}Thickness", new Thickness(value));

			if (HorizontalThicknessVariants.Contains(variant))
			{
				yield return ($"Space{variant}HorizontalThickness", new Thickness(value, 0, value, 0));
			}

			if (DirectionalThicknessVariants.Contains(variant))
			{
				yield return ($"Space{variant}VerticalThickness", new Thickness(0, value, 0, value));
				yield return ($"Space{variant}TopThickness", new Thickness(0, value, 0, 0));
				yield return ($"Space{variant}BottomThickness", new Thickness(0, 0, 0, value));
				yield return ($"Space{variant}LeftThickness", new Thickness(value, 0, 0, 0));
				yield return ($"Space{variant}RightThickness", new Thickness(0, 0, value, 0));
			}
		}
	}

	/// <summary>
	/// Enumerates every shape token derived from a base unit: each <c>Radius*</c> value and its
	/// <see cref="CornerRadius"/> companion. RadiusFull is always 9999.
	/// </summary>
	/// <param name="baseValue">The corner radius base unit the multipliers apply to.</param>
	/// <returns>The token keys paired with their values.</returns>
	internal static IEnumerable<(string Key, object Value)> ShapeTokens(double baseValue)
	{
		foreach (var (variant, multiplier) in ShapeScaleMultipliers)
		{
			var value = baseValue * multiplier;

			yield return ($"Radius{variant}", value);
			yield return ($"Radius{variant}CornerRadius", new CornerRadius(value));
		}

		// RadiusFull is always 9999 (pill shape)
		yield return ("RadiusFull", 9999.0);
		yield return ("RadiusFullCornerRadius", new CornerRadius(9999));
	}

	/// <summary>
	/// Enumerates the fixed density tokens: control heights, icon sizes and the touch target.
	/// </summary>
	/// <returns>The token keys paired with their values.</returns>
	internal static IEnumerable<(string Key, object Value)> DensityTokens()
	{
		foreach (var (key, value) in FixedDensityDefaults)
		{
			yield return (key, value);
		}
	}
}
