// Ported from material-color-utilities by Google under the Apache License 2.0.
// Original source: https://github.com/material-foundation/material-color-utilities
// SPDX-License-Identifier: Apache-2.0

using System;

namespace Uno.Themes.ColorGeneration.Hct;

/// <summary>
/// Finds the ARGB color that best matches a target Hue, Chroma, and Tone (L*).
/// Uses CAM16 for hue/chroma and CIE L* for tone. The requested chroma is
/// delivered exactly whenever it is achievable in sRGB; otherwise the maximum
/// in-gamut chroma at that hue and tone is used.
/// </summary>
internal static class HctSolver
{
	// Relative luminance coefficients (CIE Y from linear sRGB), matching ColorMath.ArgbToXyz.
	private const double YFromLinearR = 0.2126;
	private const double YFromLinearG = 0.7152;
	private const double YFromLinearB = 0.0722;

	// Newton's method converges in 2-3 rounds for in-gamut requests; 5 is the reference
	// implementation's cap and is only reached for coordinates near the gamut boundary.
	private const int JIterations = 5;

	// Chroma resolution of requested/2^20 — far finer than the 8-bit output can express.
	private const int ChromaBisections = 20;

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

		double y = ColorMath.YFromLstar(tone);

		if (TrySolveExact(hue, chroma, y, out int argb))
		{
			return argb;
		}

		// The requested chroma is outside the sRGB gamut at this hue and tone.
		// Bisect down to the largest chroma that is achievable.
		return BisectChroma(hue, chroma, y, tone);
	}

	/// <summary>Compute a gray ARGB from a tone (L*) value.</summary>
	internal static int GrayFromTone(double tone)
	{
		double y = ColorMath.YFromLstar(tone);
		int component = ColorMath.Delinearized(y);
		return ColorMath.ArgbFromRgb(component, component, component);
	}

	/// <summary>
	/// Solve for the color at the requested hue and chroma whose relative luminance is
	/// <paramref name="targetY"/>, by Newton iteration on the CAM16 lightness J.
	/// Returns <c>false</c> when the coordinates are outside the sRGB gamut.
	/// </summary>
	/// <remarks>
	/// J is solved for rather than derived from the tone because the J that yields a given
	/// L* depends on the chroma — taking J from a gray at the target tone (as the previous
	/// implementation did) is only correct at chroma 0, and forced the chroma search to give
	/// up far short of the gamut boundary for any saturated color.
	/// </remarks>
	private static bool TrySolveExact(double hue, double chroma, double targetY, out int argb)
	{
		var vc = ViewingConditions.Default;
		double j = Math.Sqrt(targetY) * 11.0;

		for (int i = 0; i < JIterations; i++)
		{
			Cam16.ToLinrgb(hue, chroma, j, vc, out double r, out double g, out double b);

			if (r < 0.0 || g < 0.0 || b < 0.0)
			{
				argb = 0;
				return false;
			}

			double y = YFromLinearR * r + YFromLinearG * g + YFromLinearB * b;
			if (y <= 0.0)
			{
				argb = 0;
				return false;
			}

			if (i == JIterations - 1 || Math.Abs(y - targetY) < 0.002)
			{
				if (r > 100.01 || g > 100.01 || b > 100.01)
				{
					argb = 0;
					return false;
				}

				argb = ColorMath.ArgbFromLinrgb(r, g, b);
				return true;
			}

			// Newton step, using 2 * f(j) / j as the approximation of f'(j).
			j -= (y - targetY) * j / (2.0 * y);
		}

		argb = 0;
		return false;
	}

	private static int BisectChroma(double hue, double requestedChroma, double targetY, double tone)
	{
		double low = 0.0;
		double high = requestedChroma;
		int bestArgb = GrayFromTone(tone);

		for (int i = 0; i < ChromaBisections; i++)
		{
			double mid = (low + high) / 2.0;
			if (TrySolveExact(hue, mid, targetY, out int candidate))
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
