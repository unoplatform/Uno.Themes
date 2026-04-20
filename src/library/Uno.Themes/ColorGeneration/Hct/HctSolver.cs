// Ported from material-color-utilities by Google under the Apache License 2.0.
// Original source: https://github.com/material-foundation/material-color-utilities
// SPDX-License-Identifier: Apache-2.0

using System;

namespace Uno.Themes.ColorGeneration.Hct;

/// <summary>
/// Finds the ARGB color that best matches a target Hue, Chroma, and Tone (L*).
/// Uses CAM16 for hue/chroma and CIE L* for tone, with bisection on chroma
/// to find the maximum achievable chroma within the sRGB gamut.
/// </summary>
internal static class HctSolver
{
	/// <summary>
	/// Find the ARGB color matching the given HCT coordinates.
	/// If the requested chroma is not achievable in sRGB, the maximum
	/// in-gamut chroma at the requested hue and tone is used.
	/// </summary>
	internal static int SolveToArgb(double hue, double chroma, double tone)
	{
		if (tone < 0.0001)
		{
			return ColorMath.ArgbFromRgb(0, 0, 0);
		}
		if (tone > 99.9999)
		{
			return ColorMath.ArgbFromRgb(255, 255, 255);
		}
		if (chroma < 0.5)
		{
			return GrayFromTone(tone);
		}

		// Compute the CAM16 J that corresponds to this tone (L*)
		// by running CAM16 forward on a gray at the target tone.
		double j = JFromTone(tone);

		// Use CAM16 inverse to get the ARGB at the desired hue, chroma, and J
		int argb = Cam16.ToArgb(hue, chroma, j, out bool inGamut);
		double actualTone = ColorMath.LstarFromArgb(argb);

		// Accept if close in tone AND within sRGB gamut
		if (Math.Abs(actualTone - tone) <= 1.0 && inGamut)
		{
			return argb;
		}

		// The requested chroma may not be achievable at this tone.
		// Bisect on chroma to find the maximum in-gamut chroma.
		return BisectChroma(hue, chroma, tone, j);
	}

	/// <summary>Compute a gray ARGB from a tone (L*) value.</summary>
	internal static int GrayFromTone(double tone)
	{
		double y = ColorMath.YFromLstar(tone);
		int component = ColorMath.Delinearized(y);
		return ColorMath.ArgbFromRgb(component, component, component);
	}

	/// <summary>
	/// Find the CAM16 J value that corresponds to a given CIE L* (tone)
	/// by running the CAM16 forward model on a gray at that tone.
	/// </summary>
	private static double JFromTone(double tone)
	{
		int grayArgb = GrayFromTone(tone);
		return Cam16.FromArgb(grayArgb).J;
	}

	private static int BisectChroma(double hue, double requestedChroma, double tone, double j)
	{
		double low = 0.0;
		double high = requestedChroma;
		int bestArgb = GrayFromTone(tone);

		for (int i = 0; i < 32; i++)
		{
			double mid = (low + high) / 2.0;
			int candidate = Cam16.ToArgb(hue, mid, j, out bool inGamut);
			double actualTone = ColorMath.LstarFromArgb(candidate);

			if (Math.Abs(actualTone - tone) <= 1.0 && inGamut)
			{
				bestArgb = candidate;
				low = mid;
			}
			else
			{
				high = mid;
			}
		}

		return bestArgb;
	}
}
