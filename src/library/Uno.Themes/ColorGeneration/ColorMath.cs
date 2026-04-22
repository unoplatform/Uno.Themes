// Ported from material-color-utilities by Google under the Apache License 2.0.
// Original source: https://github.com/material-foundation/material-color-utilities
// SPDX-License-Identifier: Apache-2.0

using System;

namespace Uno.Themes.ColorGeneration;

/// <summary>
/// Low-level color math: sRGB ↔ linear RGB ↔ CIE XYZ ↔ CIE L*a*b* conversions.
/// All methods are pure, stateless functions.
/// </summary>
internal static class ColorMath
{
	// CIE standard illuminant D65 white point
	internal static readonly double[] WhitePointD65 = { 95.047, 100.0, 108.883 };

	/// <summary>Linearize an 8-bit sRGB component (0-255) to a 0-100 scale.</summary>
	internal static double Linearized(int rgbComponent)
	{
		double normalized = rgbComponent / 255.0;
		return normalized <= 0.040449936
			? normalized / 12.92 * 100.0
			: Math.Pow((normalized + 0.055) / 1.055, 2.4) * 100.0;
	}

	/// <summary>Delinearize a 0-100 linear component back to 0-255 sRGB.</summary>
	internal static int Delinearized(double rgbComponent)
	{
		return Clamp8Bit(DelinearizedRaw(rgbComponent));
	}

	/// <summary>Delinearize without clamping — result may be outside 0-255 for out-of-gamut colors.</summary>
	internal static int DelinearizedRaw(double rgbComponent)
	{
		double normalized = rgbComponent / 100.0;
		double srgb = normalized <= 0.0031308
			? normalized * 12.92
			: 1.055 * Math.Pow(normalized, 1.0 / 2.4) - 0.055;
		return (int)Math.Round(srgb * 255.0);
	}

	internal static int Clamp8Bit(int value) => Math.Max(0, Math.Min(255, value));

	/// <summary>Convert an ARGB int to CIE XYZ (D65).</summary>
	internal static double[] ArgbToXyz(int argb)
	{
		double linearR = Linearized((argb >> 16) & 0xFF);
		double linearG = Linearized((argb >> 8) & 0xFF);
		double linearB = Linearized(argb & 0xFF);
		return new[]
		{
			0.41233895 * linearR + 0.35762064 * linearG + 0.18051042 * linearB,
			0.2126 * linearR + 0.7152 * linearG + 0.0722 * linearB,
			0.01932141 * linearR + 0.11916382 * linearG + 0.95034478 * linearB,
		};
	}

	/// <summary>Convert CIE XYZ to an ARGB int (alpha = 0xFF).</summary>
	internal static int XyzToArgb(double x, double y, double z)
	{
		int r = Delinearized(3.2413774792388685 * x - 1.5376652402851851 * y - 0.49885366846268053 * z);
		int g = Delinearized(-0.9691452513005321 * x + 1.8758853451067872 * y + 0.04156585616912061 * z);
		int b = Delinearized(0.05562093689691305 * x - 0.20395524564742123 * y + 1.0571799993703593 * z);
		return (0xFF << 24) | (r << 16) | (g << 8) | b;
	}

	/// <summary>CIE L* (perceptual lightness, 0-100) from CIE Y.</summary>
	internal static double LstarFromY(double y)
	{
		double yNorm = y / 100.0;
		double e = 216.0 / 24389.0;
		double k = 24389.0 / 27.0;
		return yNorm <= e
			? k * yNorm
			: 116.0 * Math.Pow(yNorm, 1.0 / 3.0) - 16.0;
	}

	/// <summary>CIE Y from L* (inverse of LstarFromY).</summary>
	internal static double YFromLstar(double lstar)
	{
		double ke = 8.0;
		if (lstar <= ke)
		{
			return lstar / (24389.0 / 27.0) * 100.0;
		}
		double cubeRoot = (lstar + 16.0) / 116.0;
		return cubeRoot * cubeRoot * cubeRoot * 100.0;
	}

	/// <summary>CIE L* from an ARGB int.</summary>
	internal static double LstarFromArgb(int argb)
	{
		double y = ArgbToXyz(argb)[1];
		return LstarFromY(y);
	}

	/// <summary>Pack r, g, b bytes into an ARGB int with alpha = 0xFF.</summary>
	internal static int ArgbFromRgb(int r, int g, int b) =>
		(0xFF << 24) | ((r & 0xFF) << 16) | ((g & 0xFF) << 8) | (b & 0xFF);
}
